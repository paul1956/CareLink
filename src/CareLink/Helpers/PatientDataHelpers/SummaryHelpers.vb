' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Friend Module SummaryHelpers

    Friend ReadOnly s_sensorUpdateTimes As New Dictionary(Of String, String) From {
        {"INITIAL_ALERT_SHORT", "30 minutes"},
        {"INITIAL_ALERT_MEDIUM", "60 minutes"},
        {"INITIAL_ALERT_LONG", "90 minutes"}}

    Private s_wordsInParentheses As Dictionary(Of String, List(Of String))

    ''' <summary>
    '''  Converts an object to a specified type.
    '''  This is a helper function to avoid casting issues and ensure type safety.
    ''' </summary>
    ''' <typeparam name="T">The type to convert the object to.</typeparam>
    ''' <param name="UTO">The object to convert.</param>
    ''' <returns>The converted object of type T.</returns>
    Private Function CAnyType(Of T)(UTO As Object) As T
        Return CType(UTO, T)
    End Function

    ''' <summary>
    '''  Extracts a dictionary(Of String, List(Of String)) from the input:
    '''  where the variable names are in parentheses, and are associated with
    '''  the Key of the error message
    ''' </summary>
    ''' <returns>A dictionary(Of String, List(Of String))</returns>
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

    ''' <summary>
    '''  Translates the notification message ID to a user-friendly message.
    '''  It replaces variables in parentheses with their corresponding values from the JSON dictionary.
    ''' </summary>
    ''' <param name="jsonDictionary">A dictionary containing additional information for the notification.</param>
    ''' <param name="faultId">The fault ID to translate.</param>
    ''' <returns>A translated notification message.</returns>
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
                Dim lowLimit As String = String.Empty
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
                        If jsonDictionary.TryGetValue(NameOf(ActiveNotification.SecondaryTime), secondaryTime) Then
                            secondaryTime = $" { secondaryTime.ParseDate(NameOf(ActiveNotification.SecondaryTime)).ToNotificationDateTimeString}"
                        Else
                            Stop
                        End If
                    ElseIf key = "dateTime" Then
                        Stop
                    Else
                        Dim jsonString As String = String.Empty
                        If jsonDictionary.TryGetValue("AdditionalInfo", jsonString) Then

                            Dim additionalInfo As Dictionary(Of String, String) = GetAdditionalInformation(jsonString)
                            Select Case key
                                Case "basalName"
                                    If additionalInfo.TryGetValue(key, basalName) Then
                                        basalName = basalName.ToTitle(separateNumbers:=True)
                                    Else
                                        Stop
                                    End If
                                Case "bgValue"
                                    If Not additionalInfo.TryGetValue(key, bgValue) Then
                                        Stop
                                    End If
                                Case "criticalLow"
                                    If Not additionalInfo.TryGetValue(key, criticalLow) Then
                                        Stop
                                    End If
                                Case "deliveredAmount"
                                    If Not additionalInfo.TryGetValue(key, deliveredAmount) Then
                                        Stop
                                    End If
                                Case "lastSetChange"
                                    lastSetChange = s_oneToNineteen(CInt(additionalInfo(key))).ToTitle
                                Case "notDeliveredAmount"
                                    If additionalInfo.TryGetValue("notDeliveredAmount", notDeliveredAmount) Then
                                        If Not String.IsNullOrWhiteSpace(notDeliveredAmount) Then
                                            Stop
                                        End If
                                    Else
                                        Stop
                                    End If
                                Case "programmedAmount"
                                    If additionalInfo.TryGetValue(key, programmedAmount) Then
                                        Exit Select
                                    End If
                                    Stop
                                Case "reminderName"
                                    If Not additionalInfo.TryGetValue(key, reminderName) Then
                                        Stop
                                    End If
                                Case "sensorUpdateTime"
                                    If Not additionalInfo.TryGetValue(key, sensorUpdateTime) Then
                                        Stop
                                    Else
                                        sensorUpdateTime = GetSensorUpdateTime(sensorUpdateTime)
                                    End If
                                Case "sg"
                                    If Not additionalInfo.TryGetValue(key, sg) Then
                                        Stop
                                    Else
                                        If faultId = "827" Then
                                            lowLimit = If(CSng(sg) < 65 AndAlso CSng(sg) > 20, "64", "3.5")
                                        End If
                                    End If
                                Case "units"
                        ' handled elsewhere
                                Case "unitsRemaining"
                                    If Not additionalInfo.TryGetValue(key, unitsRemaining) Then
                                        unitsRemaining = "0"
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
                    .Replace("(lowLimit)", lowLimit) _
                    .Replace("(notDeliveredAmount)", notDeliveredAmount) _
                    .Replace("(programmedAmount)", programmedAmount) _
                    .Replace("(reminderName)", reminderName) _
                    .Replace("(secondaryTime)", secondaryTime) _
                    .Replace("(sensorUpdateTime)", sensorUpdateTime) _
                    .Replace("(suspendedSince)", s_suspendedSince) _
                    .Replace("(sg)", sg) _
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
                        title:=GetTitleFromStack(stackFrame:=New StackFrame(skipFrames:=0, needFileInfo:=True)))
                End If
                Return text
            End If
        Catch ex As Exception
            Stop
        End Try
        Return originalMessage
    End Function

    Friend Function GetSensorUpdateTime(key As String) As String
        Dim sensorUpdateTime As String = String.Empty
        If s_sensorUpdateTimes.TryGetValue(key, sensorUpdateTime) Then
            Return sensorUpdateTime
        End If
        Stop
        Return $"Unknown key {key}"
    End Function

    Friend Function GetSummaryRecords(dic As Dictionary(Of String, String), Optional rowsToHide As List(Of String) = Nothing) As List(Of SummaryRecord)
        Dim summaryList As New List(Of SummaryRecord)
        If dic IsNot Nothing Then
            For Each row As KeyValuePair(Of String, String) In dic
                If row.Value Is Nothing OrElse
                   (rowsToHide IsNot Nothing AndAlso rowsToHide.Contains(row.Key, StringComparer.OrdinalIgnoreCase)) Then
                    Continue For
                End If

                Select Case row.Key
                    Case "faultId"
                        Dim message As String = String.Empty
                        If s_notificationMessages.TryGetValue(row.Value, message) Then
                            message = TranslateNotificationMessageId(dic, row.Value)
                            If row.Value = "811" Then
                                If dic.TryGetValue(NameOf(ActiveNotification.triggeredDateTime), s_suspendedSince) Then
                                    Dim resultDate As Date = Nothing
                                    s_suspendedSince = If(TryParseDate(s_suspendedSince, resultDate, NameOf(ActiveNotification.triggeredDateTime)),
                                        resultDate.ToString(s_timeWithMinuteFormat),
                                        "???")
                                End If
                            End If
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
        End If
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
                MsgBox(
                    heading:=$"{tReturnType} type is not yet defined.",
                    text:=String.Empty,
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    title:=GetTitleFromStack(stackFrame:=New StackFrame(skipFrames:=0, needFileInfo:=True)))
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

End Module
