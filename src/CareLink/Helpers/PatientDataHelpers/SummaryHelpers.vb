' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.RegularExpressions

Friend Module SummaryHelpers

    Private ReadOnly s_sensorUpdateTimes As New Dictionary(Of String, String) From {
        {"INITIAL_ALERT_SHORT", "30 minutes"}}

    'Private ReadOnly s_variablesUsedInMessages As New HashSet(Of String)
    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private s_wordsInParentheses As Dictionary(Of String, List(Of String))

    Private Function CAnyType(Of T)(UTO As Object) As T
        Return CType(UTO, T)
    End Function

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.CellFormattingSetForegroundColor(e, sorted:=False)
    End Sub

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(.Name),
                wrapHeader:=False,
                forceReadOnly:=True,
                caption:=CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
            If .Name = "Value" Then
                e.Column.MinimumWidth = 350
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    ''' <summary>
    '''  Extracts a dictionary(of String, List(of String) from the input:
    '''  where the variable names are in parentheses, and are associated with
    '''  with the Key of the error message
    ''' </summary>
    ''' <Result>A dictionary(of String, List(of String)</Result>
    Private Sub ExtractErrorMessageVariables()
        If s_wordsInParentheses IsNot Nothing Then
            Return
        End If
        ' Initialize output lists
        s_wordsInParentheses = New Dictionary(Of String, List(Of String))

        ' Regex to match words in parentheses
        Dim parenthesesRegex As New Regex("\(([^)]+)\)")

        ' Process each string in the input list
        For Each kvp As KeyValuePair(Of String, String) In s_notificationMessages
            Dim msgList As New List(Of String)
            ' Find matches for parentheses
            For Each match As Match In parenthesesRegex.Matches(kvp.Value)
                Dim word As String = match.Groups(1).Value
                Dim item As String = match.Value.TrimStart("("c).TrimEnd(")"c)
                msgList.Add(item)
                's_variablesUsedInMessages.Add(item)
            Next
            s_wordsInParentheses.Add(kvp.Key, msgList)
        Next
    End Sub

    Private Function GetMessageVariables() As List(Of String)
        Dim result As New List(Of String)
        Dim pattern As String = "\(([^)]+)\)"
        Dim valuesArray() As String = s_notificationMessages.Values.ToArray()
        Dim joinedString As String = String.Join(", ", valuesArray)

        For Each match As Match In Regex.Matches(joinedString, pattern)
            result.Add(match.Value)
        Next
        Return result.Distinct().ToList()
    End Function

    Private Function TranslateNotificationMessageId(jsonDictionary As Dictionary(Of String, String), faultId As String) As String
        ExtractErrorMessageVariables()
        Dim originalMessage As String = String.Empty
        Try
            If s_notificationMessages.TryGetValue(faultId, originalMessage) Then
                If s_wordsInParentheses(faultId).Count = 0 Then
                    Return originalMessage
                End If
                Dim basalName As String = String.Empty
                Dim bgValue As String = String.Empty
                Dim criticalLow As String = String.Empty
                Dim deliveredAmount As String = String.Empty
                Dim lastSetChange As String = String.Empty
                Dim notDeliveredAmount As String = String.Empty
                Dim programmedAmount As String = String.Empty
                Dim reminderName As String = String.Empty
                Dim secondaryTime As String = String.Empty
                Dim sg As String = String.Empty
                Dim sensorUpdateTime As String = String.Empty
                Dim triggeredDateTime As String = String.Empty
                Dim unitsRemaining As String = Nothing
                For Each key As String In s_wordsInParentheses(faultId)
                    If key = "triggeredDateTime" Then
                        If jsonDictionary.TryGetValue(NameOf(ClearedNotifications.triggeredDateTime), triggeredDateTime) Then
                            triggeredDateTime = $" { triggeredDateTime.ParseDate(NameOf(ClearedNotifications.dateTime)).ToNotificationDateTimeString}"
                        ElseIf jsonDictionary.TryGetValue(NameOf(ClearedNotifications.dateTime), triggeredDateTime) Then
                            triggeredDateTime = $" { triggeredDateTime.ParseDate(NameOf(ClearedNotifications.dateTime)).ToNotificationDateTimeString}"
                        Else
                            Stop
                        End If
                    ElseIf key = "secondaryTime" Then
                        If jsonDictionary.TryGetValue(NameOf(ActiveNotification.secondaryTime), secondaryTime) Then
                            secondaryTime = $" { secondaryTime.ParseDate(NameOf(ActiveNotification.secondaryTime)).ToNotificationDateTimeString}"
                        Else
                            Stop
                        End If
                    ElseIf key = "dateTime" Then
                        Stop
                    Else
                        Dim jsonString As String = String.Empty
                        If jsonDictionary.TryGetValue(NameOf(AdditionalInfo), jsonString) Then

                            Dim additionalInfo As Dictionary(Of String, String) = GetAdditionalInformation(jsonString)
                            Select Case key
                                Case "basalName"
                                    If additionalInfo.TryGetValue(key, basalName) Then
                                    Else
                                        Stop
                                    End If
                                Case "bgValue"
                                    If additionalInfo.TryGetValue(key, bgValue) Then
                                    Else
                                        Stop
                                    End If
                                Case "criticalLow"
                                    If additionalInfo.TryGetValue(key, criticalLow) Then
                                    Else
                                        Stop
                                    End If
                                Case "deliveredAmount"
                                    If additionalInfo.TryGetValue(key, deliveredAmount) Then
                                    Else
                                        Stop
                                    End If
                                Case "lastSetChange"
                                    lastSetChange = s_oneToNineteen(CInt(additionalInfo(key))).ToTitle
                                Case "notDeliveredAmount"
                                    If additionalInfo.TryGetValue("notDeliveredAmount", notDeliveredAmount) Then
                                        If String.IsNullOrWhiteSpace(notDeliveredAmount) Then
                                        Else
                                            Stop
                                        End If
                                    Else
                                        Stop
                                    End If
                                Case "programmedAmount"
                                    If additionalInfo.TryGetValue(key, programmedAmount) Then
                                    Else
                                        Stop

                                    End If
                                Case "reminderName"
                                    If additionalInfo.TryGetValue(key, reminderName) Then
                                    Else
                                        Stop
                                    End If
                                Case "sensorUpdateTime"
                                    If additionalInfo.TryGetValue(key, sensorUpdateTime) Then
                                        sensorUpdateTime = s_sensorUpdateTimes(sensorUpdateTime)
                                    Else
                                        Stop
                                        sensorUpdateTime = $"Unknown key {key}"
                                    End If
                                Case "sg"
                                    If additionalInfo.TryGetValue(key, sg) Then
                                    Else
                                        Stop
                                    End If
                                Case "units"
                        ' handled elsewhere
                                Case "unitsRemaining"
                                    If additionalInfo.TryGetValue(key, unitsRemaining) Then
                                    Else
                                        Stop
                                    End If
                            End Select
                        End If
                    End If
                Next

                Return originalMessage _
                    .Replace("(basalName)", basalName) _
                    .Replace("(bgValue)", bgValue) _
                    .Replace("(criticalLow)", criticalLow) _
                    .Replace("(deliveredAmount)", deliveredAmount) _
                    .Replace("(notDeliveredAmount)", notDeliveredAmount) _
                    .Replace("(programmedAmount)", programmedAmount) _
                    .Replace("(deliveredAmount)", deliveredAmount) _
                    .Replace("(lastSetChange)", lastSetChange) _
                    .Replace("(notDeliveredAmount)", notDeliveredAmount) _
                    .Replace("(programmedAmount)", programmedAmount) _
                    .Replace("(reminderName)", reminderName) _
                    .Replace("(secondaryTime)", secondaryTime) _
                    .Replace("(sensorUpdateTime)", sensorUpdateTime) _
                    .Replace("(triggeredDateTime)", triggeredDateTime) _
                    .Replace("(units)", BgUnitsNativeString) _
                    .Replace("(unitsRemaining)", unitsRemaining) _
                    .Replace("(vbCrLf)", vbCrLf)
            Else
                Dim text As String = $"faultId = '{faultId}'"
                If Debugger.IsAttached Then
                    Stop
                    MsgBox(
                        heading:="Unknown faultId",
                        text,
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                        title:=GetTitleFromStack(New StackFrame(skipFrames:=0, needFileInfo:=True)))
                End If
                Return text
            End If
        Catch ex As Exception
            Stop
        End Try
        Return originalMessage
    End Function

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of SummaryRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function GetSummaryRecords(dic As Dictionary(Of String, String), Optional rowsToHide As List(Of String) = Nothing) As List(Of SummaryRecord)
        Dim messageVariables As List(Of String) = GetMessageVariables()
        Dim dictionaryOfMessages As String = CreateDictionarySortedByValue(s_notificationMessages)

        Dim summaryList As New List(Of SummaryRecord)
        For Each row As KeyValuePair(Of String, String) In dic
            If row.Value Is Nothing OrElse (rowsToHide IsNot Nothing AndAlso rowsToHide.Contains(row.Key)) Then
                Continue For
            End If

            Select Case row.Key
                Case "faultId"
                    Dim message As String = String.Empty
                    If s_notificationMessages.TryGetValue(row.Value, message) Then
                        message = TranslateNotificationMessageId(dic, row.Value)
                        If row.Value = "BC_SID_MAX_FILL_DROPS_QUESITION" Then
                            If dic("deliveredAmount").StartsWith("3"c) Then
                                message &= "Did you see drops at the end of the tubing?"
                            Else
                                message &= "Remove reservoir and select Rewind restart New reservoir procedure."
                            End If
                        End If
                    Else
                        If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(row.Value) Then
                            MsgBox(
                                heading:=$"{row.Value} is unknown Notification Messages",
                                text:=String.Empty,
                                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                                title:=GetTitleFromStack(stackFrame:=New StackFrame(skipFrames:=0, needFileInfo:=True)))
                        End If
                        message = row.Value.ToTitle
                    End If
                    summaryList.Add(New SummaryRecord(recordNumber:=summaryList.Count, row, message))
                Case "autoModeReadinessState"
                    s_autoModeReadinessState = New SummaryRecord(recordNumber:=summaryList.Count, row, messages:=s_sensorMessages, messageTableName:=NameOf(s_sensorMessages))
                    summaryList.Add(s_autoModeReadinessState)
                Case "autoModeShieldState"
                    summaryList.Add(New SummaryRecord(recordNumber:=summaryList.Count, row, messages:=s_autoModeShieldMessages, messageTableName:=NameOf(s_autoModeShieldMessages)))
                Case "plgmLgsState"
                    summaryList.Add(New SummaryRecord(recordNumber:=summaryList.Count, row, messages:=s_plgmLgsMessages, messageTableName:=NameOf(s_plgmLgsMessages)))
                Case NameOf(ClearedNotifications.dateTime)
                    summaryList.Add(New SummaryRecord(recordNumber:=summaryList.Count, row, message:=row.Value.ParseDate(key:=NameOf(ClearedNotifications.dateTime)).ToShortDateTimeString))
                Case "additionalInfo"
                    HandleComplexItems(row, rowIndex:=CType(summaryList.Count, ServerDataIndexes), key:="additionalInfo", listOfSummaryRecords:=summaryList)
                Case Else
                    summaryList.Add(New SummaryRecord(summaryList.Count, row))
            End Select
        Next
        Return summaryList

    End Function

    Friend Function GetTabIndexFromName(tabPageName As String) As Integer
        Return CInt(tabPageName.Replace(NameOf(TabPage), String.Empty).Substring(0, 2)) - 1
    End Function

    <Extension>
    Friend Function GetValue(Of T)(l As List(Of SummaryRecord), Key As String, throwError As Boolean, Optional defaultValue As T = Nothing) As T

        Try
            For Each s As SummaryRecord In l
                If s.Key = Key Then
                    Return CAnyType(Of T)(s.Value)
                End If
            Next
            If throwError Then
                Stop
                Throw New ArgumentException("Key not found", NameOf(Key))
            End If
        Catch ex As Exception

        End Try
        If defaultValue IsNot Nothing Then
            Return defaultValue
        End If

        Dim tReturnType As Type = GetType(T)
        If tReturnType Is GetType(String) Then
            Return CAnyType(Of T)(String.Empty)
        ElseIf tReturnType Is GetType(Boolean) Then
            Return CAnyType(Of T)(False)
        ElseIf tReturnType Is GetType(Integer) Then
            Return CAnyType(Of T)(0)
        ElseIf tReturnType Is GetType(Single) Then
            Return CAnyType(Of T)(0.0)
        ElseIf tReturnType Is GetType(UShort) Then
            Return CAnyType(Of T)(UShort.MaxValue)
        Else
            If Debugger.IsAttached Then
                MsgBox($"{tReturnType} type is not yet defined.", String.Empty, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
            End If
            Return Nothing
        End If

    End Function

    <Extension>
    Friend Function GetValue(Of T)(l As List(Of SummaryRecord), Key As String) As T
        For Each s As SummaryRecord In l
            If s.Key = Key Then
                Return CAnyType(Of T)(s.Value)
            End If
        Next
        Throw New ArgumentException("Key not found", NameOf(Key))
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithoutExcel
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithoutExcel
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
    End Sub

    Public Function CreateDictionarySortedByValue(myDictionary As Dictionary(Of String, String)) As String
        Dim strBuilder As New StringBuilder
        strBuilder.AppendLine("Dim sortedDict As New Dictionary(Of String, String) With {")
        For Each kvp As KeyValuePair(Of String, String) In myDictionary.OrderBy(Function(x) x.Value)
            strBuilder.AppendLine($"    {{String.Empty{kvp.Key}String.Empty, String.Empty{kvp.Value}String.Empty}},")
        Next
        strBuilder.AppendLine("}")
        Return strBuilder.ToString
    End Function

End Module
