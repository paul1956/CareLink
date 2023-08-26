' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SummaryRecordHelpers

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Function CAnyType(Of T)(UTO As Object) As T
        Return CType(UTO, T)
    End Function

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            e.DgvColumnAdded(GetCellStyle(.Name),
                             False,
                             True,
                             CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
            If .Name = "value" Then
                e.Column.MinimumWidth = 300
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    <Extension>
    Private Function FormatTimeOnly(rawTime As String, format As String) As String
        Return New TimeOnly(CInt(rawTime.Substring(0, 2)), CInt(rawTime.Substring(3, 2))).ToString(format)
    End Function

    Private Function TranslateNotificationMessageId(jsonDictionary As Dictionary(Of String, String), entryValue As String) As String
        Dim formattedMessage As String = ""
        Try
            If s_notificationMessages.TryGetValue(entryValue, formattedMessage) Then
                Dim splitMessageValue As String() = formattedMessage.Split(":")
                Dim key As String = ""
                Dim replacementValue As String = ""
                If splitMessageValue.Length > 1 Then
                    key = splitMessageValue(1)
                    If key = "lastSetChange" Then
                        replacementValue = s_oneToNineteen(CInt(jsonDictionary(key))).ToTitle
                    Else
                        replacementValue = jsonDictionary(key)
                        Dim resultDate As Date
                        If replacementValue.TryParseDate(resultDate, key) Then
                            replacementValue = resultDate.ToString
                        End If
                    End If
                End If

                Dim secondaryTime As String = Nothing
                secondaryTime = If(jsonDictionary.TryGetValue(NameOf(ClearedNotificationsRecord.secondaryTime), secondaryTime),
                                   secondaryTime.FormatTimeOnly(s_timeWithMinuteFormat),
                                   ""
                                  )

                Dim deliveredAmount As String = ""
                If jsonDictionary.TryGetValue("deliveredAmount", deliveredAmount) Then
                End If

                Dim programmedAmount As String = ""
                Dim notDeliveredAmount As String = ""
                If jsonDictionary.TryGetValue("programmedAmount", programmedAmount) Then
                    notDeliveredAmount = (programmedAmount.ParseSingle(3) - deliveredAmount.ParseSingle(3)).ToString("F3", CurrentUICulture)
                End If

                Dim triggeredDateTime As String = ""
                If jsonDictionary.TryGetValue(NameOf(ClearedNotificationsRecord.triggeredDateTime), triggeredDateTime) Then
                    triggeredDateTime = $" { triggeredDateTime.ParseDate("triggeredDateTime").ToShortTimeString}"
                ElseIf jsonDictionary.TryGetValue(NameOf(SgRecord.datetime), triggeredDateTime) Then
                    triggeredDateTime = $" {triggeredDateTime.ParseDate(NameOf(SgRecord.datetime)).ToShortTimeString}"
                ElseIf jsonDictionary.TryGetValue(NameOf(TimeChangeRecord.dateTime), triggeredDateTime) Then
                    triggeredDateTime = $" {triggeredDateTime.ParseDate("dateTime").ToShortTimeString}"
                Else
                    Stop
                End If

                formattedMessage = splitMessageValue(0) _
                .Replace("(0)", replacementValue) _
                .Replace($"({NameOf(deliveredAmount)})", deliveredAmount) _
                .Replace($"({NameOf(programmedAmount)})", programmedAmount) _
                .Replace($"({NameOf(notDeliveredAmount)})", notDeliveredAmount) _
                .Replace($"({NameOf(ClearedNotificationsRecord.secondaryTime)})", secondaryTime) _
                .Replace("(triggeredDateTime)", $", happened at {triggeredDateTime}") _
                .Replace("(units)", SgUnitsNativeString)
            Else
                If Debugger.IsAttached Then
                    Stop
                    MsgBox("Unknown Notification Message ", $"'{entryValue}'", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
                End If
                formattedMessage = entryValue.Replace("_", " ")
            End If
        Catch ex As Exception
            Stop
        End Try
        Return formattedMessage
    End Function

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of SummaryRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function GetSummaryRecords(dic As Dictionary(Of String, String), Optional rowsToHide As List(Of String) = Nothing) As List(Of SummaryRecord)
        Dim summaryList As New List(Of SummaryRecord)
        For Each row As KeyValuePair(Of String, String) In dic
            If row.Value Is Nothing OrElse (rowsToHide IsNot Nothing AndAlso rowsToHide.Contains(row.Key)) Then
                Continue For
            End If

            Select Case row.Key
                Case "messageId"
                    Dim message As String = ""
                    If s_notificationMessages.TryGetValue(row.Value, message) Then
                        message = TranslateNotificationMessageId(dic, row.Value)
                        If row.Value = "BC_SID_MAX_FILL_DROPS_QUESITION" Then
                            If dic("deliveredAmount").StartsWith("3") Then
                                message &= "Did you see drops at the end of the tubing?"
                            Else
                                message &= "Remove reservoir and select Rewind restart New reservoir procedure."
                            End If
                        End If
                    Else
                        If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(row.Value) Then
                            MsgBox($"{row.Value} is unknown Notification Messages", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
                        End If
                        message = row.Value.ToTitle
                    End If
                    summaryList.Add(New SummaryRecord(summaryList.Count, row, message))
                Case "autoModeReadinessState"
                    summaryList.Add(New SummaryRecord(summaryList.Count, row, s_sensorMessages, NameOf(s_sensorMessages)))
                Case "autoModeShieldState"
                    summaryList.Add(New SummaryRecord(summaryList.Count, row, s_autoModeShieldMessages, NameOf(s_autoModeShieldMessages)))
                Case "plgmLgsState"
                    summaryList.Add(New SummaryRecord(summaryList.Count, row, s_plgmLgsMessages, NameOf(s_plgmLgsMessages)))
                Case Else
                    summaryList.Add(New SummaryRecord(summaryList.Count, row))
            End Select
        Next
        Return summaryList

    End Function

    Friend Function GetTabIndexFromName(tabPageName As String) As Integer
        Return CInt(tabPageName.Replace(NameOf(TabPage), "").Substring(0, 2)) - 1
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
            Return CAnyType(Of T)("")
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
                MsgBox($"{tReturnType} type is not yet defined.", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
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
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithoutExcel
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
    End Sub

End Module
