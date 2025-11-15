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

    Private s_secondaryTimeReminder As String

    ''' <summary>
    '''  Stores extracted variable names (words in parentheses) from
    '''  notification messages, keyed by the message key.
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
    '''  Populates s_wordsInParentheses with variable names found in parentheses
    '''  for each notification message.
    ''' </remarks>
    Private Sub ExtractErrorMessageVariables()
        If s_wordsInParentheses IsNot Nothing Then
            Return
        End If
        ' Initialize output lists
        s_wordsInParentheses = New Dictionary(Of String, List(Of String))

        ' Regex to match words in parentheses
        Dim parenthesesRegex As New Regex(pattern:="\(([^)]+)\)")

        ' Process each string in the input list
        For Each kvp As KeyValuePair(Of String, String) In s_notificationMessages
            Dim value As New List(Of String)
            ' Find matches for parentheses
            For Each match As Match In parenthesesRegex.Matches(input:=kvp.Value)
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
    '''  It replaces variables in parentheses with their corresponding values
    '''  from the JSON dictionary.
    ''' </summary>
    ''' <param name="jsonDictionary">
    '''  A dictionary containing additional information for the notification.
    ''' </param>
    ''' <param name="faultId">The fault ID to translate.</param>
    ''' <returns>A translated notification message.</returns>
    Private Function TranslateNotificationMessageId(
        jsonDictionary As Dictionary(Of String, String),
        faultId As String) As String

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
        Dim triggeredDate As String = String.Empty
        Dim unitsRemaining As String = Nothing
        Try
            Dim faultIdFound As Boolean = s_notificationMessages.TryGetValue(key:=faultId, value:=originalMessage)
            If Not faultIdFound Then
                Dim prompt As String = $"faultId = '{faultId}'"
                If Debugger.IsAttached Then
                    Stop
                    Dim stackFrame As New StackFrame(skipFrames:=0, needFileInfo:=True)
                    MsgBox(
                        heading:="Unknown faultId",
                        prompt,
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                        title:=GetTitleFromStack(stackFrame))
                End If
                Return prompt
            End If
            Dim list As List(Of String) = s_wordsInParentheses(key:=faultId)
            If list.Count = 0 Then
                Return originalMessage
            End If

            Dim key As String = "triggeredDateTime"
            If list.Contains(item:=key) Then
                key = NameOf(ClearedNotifications.dateTime)
                If jsonDictionary.TryGetValue(key, value:=triggeredDate) Then
                    triggeredDate = $" { triggeredDate.ParseDate(key).ToNotificationString}"
                ElseIf jsonDictionary.TryGetValue(key, value:=triggeredDate) Then
                    triggeredDate = $" { triggeredDate.ParseDate(key).ToNotificationString}"
                Else
                    Stop
                End If

            End If

            Dim triggerTime As TimeOnly
            For Each keyWord As String In list
                Select Case keyWord
                    Case "triggeredDateTime"
                       ' handled above
                    Case "secondaryTime", "secondaryTimeReminder"
                        key = NameOf(ActiveNotification.SecondaryTime)
                        If jsonDictionary.TryGetValue(key, value:=secondaryTime) Then
                            triggerTime = TimeOnly.FromDateTime(secondaryTime.ParseDate(key))
                            secondaryTime = $" { secondaryTime.ParseDate(key).ToNotificationString}"
                        Else
                            Dim jsonString As String = String.Empty
                            key = "AdditionalInfo"
                            If jsonDictionary.TryGetValue(key, value:=jsonString) Then
                                Dim addInfo As Dictionary(Of String, String) = GetAdditionalInformation(json:=jsonString)
                                If addInfo.TryGetValue("secondaryTime", value:=secondaryTime) Then
                                    s_secondaryTimeReminder = secondaryTime.FormatTimeText()
                                Else
                                    Stop
                                End If
                            End If
                        End If
                    Case "dateTime"
                        Stop
                    Case Else
                        Dim jsonString As String = String.Empty
                        key = "AdditionalInfo"
                        If jsonDictionary.TryGetValue(key, value:=jsonString) Then
                            Dim addInfo As Dictionary(Of String, String) = GetAdditionalInformation(json:=jsonString)
                            key = keyWord
                            Select Case keyWord
                                Case "basalName"
                                    If addInfo.TryGetValue(key, value:=basalName) Then
                                        basalName = basalName.ToTitle(separateDigits:=True)
                                    Else
                                        Stop
                                    End If
                                Case "bgValue"
                                    If addInfo.TryGetValue(key, value:=bgValue) Then
                                    Else
                                        Stop
                                    End If
                                Case "criticalLow"
                                    If addInfo.TryGetValue(key, value:=criticalLow) Then
                                    Else
                                        Stop
                                    End If
                                Case "deliveredAmount"
                                    If addInfo.TryGetValue(key, value:=deliveredAmount) Then
                                    Else
                                        Stop
                                    End If
                                Case "lastSetChange"
                                    lastSetChange = s_oneToNineteen(index:=CInt(addInfo(key))).ToTitle
                                Case "notDeliveredAmount"
                                    If addInfo.TryGetValue(key, value:=notDeliveredAmount) Then
                                    Else
                                        Stop
                                    End If
                                Case "programmedAmount"
                                    If addInfo.TryGetValue(key, value:=programmedAmount) Then
                                    Else
                                        Stop
                                    End If
                                    Stop
                                Case "reminderName"
                                    If addInfo.TryGetValue(key, value:=reminderName) Then
                                    Else
                                        Stop
                                    End If
                                Case "sensorUpdateTime"
                                    If addInfo.TryGetValue(key, value:=sensorUpdateTime) Then
                                    Else
                                        sensorUpdateTime = GetSensorUpdateTime(key:=sensorUpdateTime)
                                    End If
                                Case "sg"
                                    Dim lowAlertRec As LowAlertRecord
                                    If addInfo.TryGetValue(key, value:=sg) Then
                                        If faultId = "827" Then
                                            lowLimit = If(CSng(sg) < 65 AndAlso CSng(sg) > 20,
                                                          "64",
                                                          "3.5")
                                        End If
                                    ElseIf faultId = "787" Then
                                        lowAlertRec = LowAlertsRecord.GetLowAlertRecord(triggerTime)
                                        lowLimit = $"{lowAlertRec.LowLimit} {BgUnits()}"
                                    Else
                                        Stop
                                    End If
                                Case "units"
                        ' handled elsewhere
                                Case "unitsRemaining"
                                    If Not addInfo.TryGetValue(key, value:=unitsRemaining) Then
                                        unitsRemaining = "0"
                                    End If
                            End Select
                        End If
                End Select
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
                .Replace(oldValue:="(secondaryTimeReminder)", newValue:=s_secondaryTimeReminder) _
                .Replace(oldValue:="(sensorUpdateTime)", newValue:=sensorUpdateTime) _
                .Replace(oldValue:="(suspendedSince)", newValue:=s_suspendedSince) _
                .Replace(oldValue:="(sg)", newValue:=sg) _
                .Replace(oldValue:="(triggeredDateTime)", newValue:=triggeredDate) _
                .Replace(oldValue:="(units)", newValue:=BgUnits()) _
                .Replace(oldValue:="(unitsRemaining)", newValue:=unitsRemaining) _
                .Replace(oldValue:="(vbCrLf)", newValue:=vbCrLf)
        Catch ex As Exception
            Stop
        End Try
        Return originalMessage
    End Function

    ''' <summary>
    '''  Gets the human-readable sensor update time string for a given key.
    ''' </summary>
    ''' <param name="key">The key representing the sensor update time.</param>
    ''' <returns>
    '''  The corresponding sensor update time string, or an error message if not found.
    ''' </returns>
    Friend Function GetSensorUpdateTime(key As String) As String
        Dim sensorUpdateTime As String = String.Empty
        If s_sensorUpdateTimes.TryGetValue(key, value:=sensorUpdateTime) Then
            Return sensorUpdateTime
        End If
        Stop
        Return $"Unknown key {key}"
    End Function

    ''' <summary>
    '''  Generates a list of <see cref="SummaryRecord"/> objects from a dictionary
    '''  of key-value pairs.
    '''  Optionally hides specified rows.
    ''' </summary>
    ''' <param name="jsonDictionary">The dictionary containing summary data.</param>
    ''' <param name="rowsToHide">An optional list of row keys to hide.</param>
    ''' <returns>
    '''  A list of <see cref="SummaryRecord"/> objects representing the summary.
    ''' </returns>
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
                Dim recordNumber As Integer = listOfSummaryRecords.Count
                Dim item As SummaryRecord = Nothing
                Select Case kvp.Key
                    Case "faultId"
                        Dim message As String = String.Empty
                        If s_notificationMessages.TryGetValue(key:=kvp.Value, value:=message) Then
                            message = TranslateNotificationMessageId(jsonDictionary, faultId:=kvp.Value)
                            If kvp.Value = "811" Then
                                Dim key As String = NameOf(ActiveNotification.triggeredDateTime)
                                If jsonDictionary.TryGetValue(key, value:=s_suspendedSince) Then
                                    Dim result As Date = Nothing
                                    key = NameOf(ActiveNotification.triggeredDateTime)
                                    s_suspendedSince = If(s_suspendedSince.TryParseDate(key, result),
                                                          result.ToString(format:=s_timeWithMinuteFormat),
                                                          "???")
                                End If
                            End If
                            If kvp.Value = "BC_SID_MAX_FILL_DROPS_QUESITION" Then
                                Dim question As String = jsonDictionary(key:="deliveredAmount")
                                If question.StartsWith(value:="3"c) Then
                                    message &= "Did you see drops at the end of the tubing?"
                                Else
                                    message &= "Remove reservoir and select rewind, restart New reservoir procedure."
                                End If
                            End If
                        Else
                            Dim stackFrame As StackFrame
                            If Debugger.IsAttached AndAlso IsNotNullOrWhiteSpace(kvp.Value) Then
                                stackFrame = New StackFrame(skipFrames:=0, needFileInfo:=True)
                                MsgBox(
                                    heading:=$"{kvp.Value} is unknown Notification Messages",
                                    prompt:=String.Empty,
                                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                                    title:=GetTitleFromStack(stackFrame))
                            End If
                            message = kvp.Value.ToTitle
                        End If
                        item = New SummaryRecord(recordNumber, kvp, message)
                    Case "autoModeReadinessState"
                        s_autoModeReadinessState = New SummaryRecord(
                                                        recordNumber,
                                                        kvp,
                                                        messages:=s_sensorMessages,
                                                        messageTableName:=NameOf(s_sensorMessages))
                        listOfSummaryRecords.Add(s_autoModeReadinessState)
                    Case "autoModeShieldState"
                        item = New SummaryRecord(
                                    recordNumber,
                                    kvp,
                                    messages:=s_autoModeShieldMessages,
                                    messageTableName:=NameOf(s_autoModeShieldMessages))
                    Case "plgmLgsState"
                        item = New SummaryRecord(
                                    recordNumber,
                                    kvp,
                                    messages:=s_plgmLgsMessages,
                                    messageTableName:=NameOf(s_plgmLgsMessages))
                    Case NameOf(ClearedNotifications.dateTime)
                        Dim key As String = NameOf(ClearedNotifications.dateTime)
                        item = New SummaryRecord(
                                    recordNumber,
                                    kvp,
                                    message:=kvp.Value.ParseDate(key).ToShortDateTime)
                    Case "additionalInfo"
                        HandleComplexItems(
                            kvp,
                            recordNumber,
                            key:="additionalInfo",
                            listOfSummaryRecords)
                    Case Else
                        item = New SummaryRecord(recordNumber, kvp)
                End Select
                If item IsNot Nothing Then
                    listOfSummaryRecords.Add(item)
                End If
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
    ''' <param name="throwError">
    '''  Whether to throw an exception if the key is not found.
    ''' </param>
    ''' <param name="defaultValue">
    '''  The default value to return if the key is not found and
    '''  <paramref name="throwError"/> is false.
    ''' </param>
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
                Dim stackFrame As New StackFrame(skipFrames:=0, needFileInfo:=True)
                MsgBox(
                    heading:=$"{tReturnType} type is not yet defined.",
                    prompt:=String.Empty,
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    title:=GetTitleFromStack(stackFrame))
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
