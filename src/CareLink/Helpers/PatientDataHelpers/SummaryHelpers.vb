' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

''' <summary>
'''  Provides helper methods for summarizing and translating patient data notifications,
'''  extracting variables from messages, and generating summary records.
''' </summary>
Friend Module SummaryHelpers

    ''' <summary>
    '''  Maps sensor update time keys to their corresponding human-readable time strings.
    ''' </summary>
    Friend ReadOnly s_sensorUpdateTimes As New Dictionary(Of String, String) From {
        {"INITIAL_ALERT_SHORT", "30 minutes"},
        {"INITIAL_ALERT_MEDIUM", "60 minutes"},
        {"INITIAL_ALERT_LONG", "90 minutes"}}

    ''' <summary>
    '''  Stores extracted variable names (words in parentheses) from notification messages,
    '''  keyed by the message key.
    ''' </summary>
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
    '''  Extracts a <see cref="Dictionary(Of String, List(Of String))"/> from the input:
    '''  where the variable names are in parentheses, and are associated with
    '''  the Key of the error message.
    ''' </summary>
    ''' <remarks>
    '''  Populates <c>s_wordsInParentheses</c> with variable names found in parentheses for each notification message.
    ''' </remarks>
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
            Dim value As New List(Of String)
            ' Find matches for parentheses
            For Each match As Match In parenthesesRegex.Matches(kvp.Value)
                Dim word As String = match.Groups(groupnum:=1).Value
                Dim item As String = match.Value.TrimStart(trimChar:="("c).TrimEnd(trimChar:=")"c)
                value.Add(item)
                's_variablesUsedInMessages.Add(item)
            Next
            s_wordsInParentheses.Add(kvp.Key, value)
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
        Try
            If s_notificationMessages.TryGetValue(key:=faultId, value:=originalMessage) Then
                If s_wordsInParentheses(key:=faultId).Count = 0 Then
                    Return originalMessage
                End If
                For Each key As String In s_wordsInParentheses(key:=faultId)
                    If key = "triggeredDateTime" Then
                        If jsonDictionary.TryGetValue(key:=NameOf(ClearedNotifications.triggeredDateTime), value:=triggeredDateTime) Then
                            triggeredDateTime = $" { triggeredDateTime.ParseDate(key:=NameOf(ClearedNotifications.dateTime)).ToNotificationDateTimeString}"
                        ElseIf jsonDictionary.TryGetValue(key:=NameOf(ClearedNotifications.dateTime), value:=triggeredDateTime) Then
                            triggeredDateTime = $" { triggeredDateTime.ParseDate(key:=NameOf(ClearedNotifications.dateTime)).ToNotificationDateTimeString}"
                        Else
                            Stop
                        End If
                    ElseIf key = "secondaryTime" Then
                        If jsonDictionary.TryGetValue(key:=NameOf(ActiveNotification.SecondaryTime), value:=secondaryTime) Then
                            secondaryTime = $" { secondaryTime.ParseDate(key:=NameOf(ActiveNotification.SecondaryTime)).ToNotificationDateTimeString}"
                        Else
                            Stop
                        End If
                    ElseIf key = "dateTime" Then
                        Stop
                    Else
                        Dim jsonString As String = String.Empty
                        If jsonDictionary.TryGetValue(key:="AdditionalInfo", value:=jsonString) Then

                            Dim additionalInfo As Dictionary(Of String, String) = GetAdditionalInformation(jsonString)
                            Select Case key
                                Case "basalName"
                                    If additionalInfo.TryGetValue(key, value:=basalName) Then
                                        basalName = basalName.ToTitle(separateNumbers:=True)
                                    Else
                                        Stop
                                    End If
                                Case "bgValue"
                                    If Not additionalInfo.TryGetValue(key, value:=bgValue) Then
                                        Stop
                                    End If
                                Case "criticalLow"
                                    If Not additionalInfo.TryGetValue(key, value:=criticalLow) Then
                                        Stop
                                    End If
                                Case "deliveredAmount"
                                    If Not additionalInfo.TryGetValue(key, value:=deliveredAmount) Then
                                        Stop
                                    End If
                                Case "lastSetChange"
                                    lastSetChange = s_oneToNineteen(index:=CInt(additionalInfo(key))).ToTitle
                                Case "notDeliveredAmount"
                                    If additionalInfo.TryGetValue(key:="notDeliveredAmount", value:=notDeliveredAmount) Then
                                        If Not String.IsNullOrWhiteSpace(notDeliveredAmount) Then
                                            Stop
                                        End If
                                    Else
                                        Stop
                                    End If
                                Case "programmedAmount"
                                    If additionalInfo.TryGetValue(key, value:=programmedAmount) Then
                                        Exit Select
                                    End If
                                    Stop
                                Case "reminderName"
                                    If Not additionalInfo.TryGetValue(key, value:=reminderName) Then
                                        Stop
                                    End If
                                Case "sensorUpdateTime"
                                    If Not additionalInfo.TryGetValue(key, value:=sensorUpdateTime) Then
                                        Stop
                                    Else
                                        sensorUpdateTime = GetSensorUpdateTime(key:=sensorUpdateTime)
                                    End If
                                Case "sg"
                                    If Not additionalInfo.TryGetValue(key, value:=sg) Then
                                        Stop
                                    Else
                                        If faultId = "827" Then
                                            lowLimit = If(CSng(sg) < 65 AndAlso CSng(sg) > 20,
                                                          "64",
                                                          "3.5")
                                        End If
                                    End If
                                Case "units"
                        ' handled elsewhere
                                Case "unitsRemaining"
                                    If Not additionalInfo.TryGetValue(key, value:=unitsRemaining) Then
                                        unitsRemaining = "0"
                                    End If
                            End Select
                        End If
                    End If
                Next

                Return originalMessage _
                    .Replace(oldValue:="(basalName)", newValue:=basalName) _
                    .Replace(oldValue:="(bgValue)", newValue:=bgValue) _
                    .Replace(oldValue:="(criticalLow)", newValue:=criticalLow) _
                    .Replace(oldValue:="(deliveredAmount)", newValue:=deliveredAmount) _
                    .Replace(oldValue:="(notDeliveredAmount)", newValue:=notDeliveredAmount) _
                    .Replace(oldValue:="(programmedAmount)", newValue:=programmedAmount) _
                    .Replace(oldValue:="(deliveredAmount)", newValue:=deliveredAmount) _
                    .Replace(oldValue:="(lastSetChange)", newValue:=lastSetChange) _
                    .Replace(oldValue:="(lowLimit)", newValue:=lowLimit) _
                    .Replace(oldValue:="(notDeliveredAmount)", newValue:=notDeliveredAmount) _
                    .Replace(oldValue:="(programmedAmount)", newValue:=programmedAmount) _
                    .Replace(oldValue:="(reminderName)", newValue:=reminderName) _
                    .Replace(oldValue:="(secondaryTime)", newValue:=secondaryTime) _
                    .Replace(oldValue:="(sensorUpdateTime)", newValue:=sensorUpdateTime) _
                    .Replace(oldValue:="(suspendedSince)", newValue:=s_suspendedSince) _
                    .Replace(oldValue:="(sg)", newValue:=sg) _
                    .Replace(oldValue:="(triggeredDateTime)", newValue:=triggeredDateTime) _
                    .Replace(oldValue:="(units)", newValue:=GetBgUnits()) _
                    .Replace(oldValue:="(unitsRemaining)", newValue:=unitsRemaining) _
                    .Replace(oldValue:="(vbCrLf)", newValue:=vbCrLf)
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

    ''' <summary>
    '''  Gets the human-readable sensor update time string for a given key.
    ''' </summary>
    ''' <param name="key">The key representing the sensor update time.</param>
    ''' <returns>The corresponding sensor update time string, or an error message if not found.</returns>
    Friend Function GetSensorUpdateTime(key As String) As String
        Dim sensorUpdateTime As String = String.Empty
        If s_sensorUpdateTimes.TryGetValue(key, sensorUpdateTime) Then
            Return sensorUpdateTime
        End If
        Stop
        Return $"Unknown key {key}"
    End Function

    ''' <summary>
    '''  Generates a list of <see cref="SummaryRecord"/> objects from a dictionary of key-value pairs.
    '''  Optionally hides specified rows.
    ''' </summary>
    ''' <param name="jsonDictionary">The dictionary containing summary data.</param>
    ''' <param name="rowsToHide">An optional list of row keys to hide.</param>
    ''' <returns>A list of <see cref="SummaryRecord"/> objects representing the summary.</returns>
    Friend Function GetSummaryRecords(
        jsonDictionary As Dictionary(Of String, String),
        Optional rowsToHide As List(Of String) = Nothing) As List(Of SummaryRecord)

        Dim listOfSummaryRecords As New List(Of SummaryRecord)
        If jsonDictionary IsNot Nothing Then
            For Each kvp As KeyValuePair(Of String, String) In jsonDictionary
                If kvp.Value Is Nothing Then
                    Continue For
                End If
                If rowsToHide IsNot Nothing AndAlso
                    rowsToHide.Contains(value:=kvp.Key, comparer:=StringComparer.OrdinalIgnoreCase) Then
                    Continue For
                End If
                Select Case kvp.Key
                    Case "faultId"
                        Dim message As String = String.Empty
                        If s_notificationMessages.TryGetValue(key:=kvp.Value, value:=message) Then
                            message = TranslateNotificationMessageId(jsonDictionary, faultId:=kvp.Value)
                            If kvp.Value = "811" Then
                                Dim key As String = NameOf(ActiveNotification.triggeredDateTime)
                                If jsonDictionary.TryGetValue(key, value:=s_suspendedSince) Then
                                    Dim resultDate As Date = Nothing
                                    key = NameOf(ActiveNotification.triggeredDateTime)
                                    s_suspendedSince = If(TryParseDate(s:=s_suspendedSince, resultDate, key),
                                                          resultDate.ToString(format:=s_timeWithMinuteFormat),
                                                          "???")
                                End If
                            End If
                            If kvp.Value = "BC_SID_MAX_FILL_DROPS_QUESITION" Then
                                If jsonDictionary(key:="deliveredAmount").StartsWith(value:="3"c) Then
                                    message &= "Did you see drops at the end of the tubing?"
                                Else
                                    message &= "Remove reservoir and select Rewind restart New reservoir procedure."
                                End If
                            End If
                        Else
                            If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(kvp.Value) Then
                                Dim stackFrame As New StackFrame(skipFrames:=0, needFileInfo:=True)
                                MsgBox(
                                    heading:=$"{kvp.Value} is unknown Notification Messages",
                                    text:=String.Empty,
                                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                                    title:=GetTitleFromStack(stackFrame))
                            End If
                            message = kvp.Value.ToTitle
                        End If
                        listOfSummaryRecords.Add(New SummaryRecord(recordNumber:=listOfSummaryRecords.Count, kvp, message))
                    Case "autoModeReadinessState"
                        s_autoModeReadinessState = New SummaryRecord(recordNumber:=listOfSummaryRecords.Count, kvp, messages:=s_sensorMessages, messageTableName:=NameOf(s_sensorMessages))
                        listOfSummaryRecords.Add(s_autoModeReadinessState)
                    Case "autoModeShieldState"
                        listOfSummaryRecords.Add(New SummaryRecord(recordNumber:=listOfSummaryRecords.Count, kvp, messages:=s_autoModeShieldMessages, messageTableName:=NameOf(s_autoModeShieldMessages)))
                    Case "plgmLgsState"
                        listOfSummaryRecords.Add(New SummaryRecord(recordNumber:=listOfSummaryRecords.Count, kvp, messages:=s_plgmLgsMessages, messageTableName:=NameOf(s_plgmLgsMessages)))
                    Case NameOf(ClearedNotifications.dateTime)
                        listOfSummaryRecords.Add(New SummaryRecord(recordNumber:=listOfSummaryRecords.Count, kvp, message:=kvp.Value.ParseDate(key:=NameOf(ClearedNotifications.dateTime)).ToShortDateTimeString))
                    Case "additionalInfo"
                        HandleComplexItems(kvp, recordNumber:=CType(listOfSummaryRecords.Count, ServerDataIndexes), key:="additionalInfo", listOfSummaryRecords)
                    Case Else
                        listOfSummaryRecords.Add(New SummaryRecord(listOfSummaryRecords.Count, kvp))
                End Select
            Next
        End If
        Return listOfSummaryRecords

    End Function

    ''' <summary>
    '''  Gets the tab index from a tab page name.
    ''' </summary>
    ''' <param name="tabPageName">The name of the tab page.</param>
    ''' <returns>The zero-based tab index.</returns>
    Friend Function GetTabIndexFromName(tabPageName As String) As Integer
        Return CInt(tabPageName.Substring(startIndex:=NameOf(TabPage).Length, length:=2)) - 1
    End Function

    ''' <summary>
    '''  Gets the value of a specified key from a list of <see cref="SummaryRecord"/> objects.
    '''  Returns a default value or throws an exception if not found, based on parameters.
    ''' </summary>
    ''' <typeparam name="T">The expected type of the value.</typeparam>
    ''' <param name="l">The list of <see cref="SummaryRecord"/> objects.</param>
    ''' <param name="key">The key to search for.</param>
    ''' <param name="throwError">Whether to throw an exception if the key is not found.</param>
    ''' <param name="defaultValue">The default value to return if the key is not found and <paramref name="throwError"/> is false.</param>
    ''' <returns>The value associated with the key, or the default value.</returns>
    <Extension>
    Friend Function GetValue(Of T)(
        l As List(Of SummaryRecord),
        key As String,
        throwError As Boolean,
        Optional defaultValue As T = Nothing) As T

        Try
            For Each s As SummaryRecord In l
                If s.Key = key Then
                    Return CAnyType(Of T)(UTO:=s.Value)
                End If
            Next
            If throwError Then
                Stop
                Throw New ArgumentException(message:="Key not found", paramName:=NameOf(key))
            End If
        Catch ex As Exception

        End Try
        If defaultValue IsNot Nothing Then
            Return defaultValue
        End If

        Dim tReturnType As Type = GetType(T)
        If tReturnType Is GetType(String) Then
            Return CAnyType(Of T)(UTO:=String.Empty)
        ElseIf tReturnType Is GetType(Boolean) Then
            Return CAnyType(Of T)(UTO:=False)
        ElseIf tReturnType Is GetType(Integer) Then
            Return CAnyType(Of T)(UTO:=0)
        ElseIf tReturnType Is GetType(Single) Then
            Return CAnyType(Of T)(UTO:=0.0)
        ElseIf tReturnType Is GetType(UShort) Then
            Return CAnyType(Of T)(UTO:=UShort.MaxValue)
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

    ''' <summary>
    '''  Gets the value of a specified key from a list of <see cref="SummaryRecord"/> objects.
    '''  Throws an exception if the key is not found.
    ''' </summary>
    ''' <typeparam name="T">The expected type of the value.</typeparam>
    ''' <param name="l">The list of <see cref="SummaryRecord"/> objects.</param>
    ''' <param name="key">The key to search for.</param>
    ''' <returns>The value associated with the key.</returns>
    ''' <exception cref="ArgumentException">Thrown if the key is not found.</exception>
    <Extension>
    Friend Function GetValue(Of T)(l As List(Of SummaryRecord), key As String) As T
        For Each s As SummaryRecord In l
            If s.Key = key Then
                Return CAnyType(Of T)(UTO:=s.Value)
            End If
        Next
        Throw New ArgumentException(message:="Key not found", paramName:=NameOf(key))
    End Function

End Module
