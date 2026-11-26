' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.RegularExpressions

''' <summary>
'''  Provides helper methods for summarizing and translating patient data notifications,
'''  extracting variables from messages, and generating summary records.
''' </summary>
Friend Module SummaryHelpers

    Private ReadOnly s_notificationPlaceholderValues As Dictionary(Of String, String) =
        CreateNotificationPlaceholderValues()

    ''' <summary>
    '''  Maps sensor update time keys to their corresponding human-readable time strings.
    ''' </summary>
    Friend ReadOnly s_sensorUpdateTimes As New Dictionary(Of String, String)(Comparer) From {
        {"INITIAL_ALERT_SHORT", "30 minutes"},
        {"INITIAL_ALERT_MEDIUM", "60 minutes"},
        {"INITIAL_ALERT_LONG", "90 more minutes"},
        {"DURATION_CHANGED_TO_SHORT", "30 minutes"},
        {"DURATION_CHANGED_TO_MEDIUM", "90 more minutes"},
        {"DURATION_CHANGED_TO_LONG", "2 hours"}}

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

    Private Function CreateNotificationPlaceholderValues() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String)(comparer:=StringComparer.Ordinal) From {
            {"alertClearType", Nothing},
            {"basalName", Nothing},
            {"bgValue", Nothing},
            {"criticalLow", Nothing},
            {"deliveredAmount", Nothing},
            {"lastSetChange", Nothing},
            {"lowLimit", Nothing},
            {"notDeliveredAmount", Nothing},
            {"programmedAmount", Nothing},
            {"reminderName", Nothing},
            {"resolved", Nothing},
            {"secondaryTime", Nothing},
            {"secondaryTimeReminder", Nothing},
            {"sensorUpdateTime", Nothing},
            {"suspendedSince", Nothing},
            {"sg", Nothing},
            {"sg64", Nothing},
            {"triggeredDateTime", Nothing},
            {"activeInsulinResetTime", Nothing},
            {"units", Nothing},
            {"unitsRemaining", Nothing},
            {"vbCrLf", Nothing}}
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
            Dim recordNumber As Single = 0
            For Each kvp As KeyValuePair(Of String, String) In jsonDictionary
                If kvp.Value Is Nothing Then
                    Continue For
                End If
                If rowsToHide IsNot Nothing AndAlso
                   rowsToHide.Contains(value:=kvp.Key, Comparer) Then

                    Continue For
                End If
                Dim item As SummaryRecord = Nothing

                Select Case True
                    Case kvp.Key = "faultId"
                        Dim faultId As String = kvp.Value
                        Dim message As String = String.Empty

                        Dim success As Boolean =
                            TranslateNotificationMessageId(jsonDictionary, faultId:=kvp.Value, message)

                        If kvp.Value = "811" Then
                            Dim key As String = NameOf(ActiveNotification.TriggeredDateTime)
                            If jsonDictionary.TryGetValue(key, value:=s_suspendedSince) Then
                                Dim result As Date = Nothing
                                key = NameOf(ActiveNotification.TriggeredDateTime)
                                s_suspendedSince = If(s_suspendedSince.TryParseDate(key, result),
                                                      result.ToString(format:=s_timeWithMinuteFormat),
                                                      "???")
                            End If
                        ElseIf kvp.Value = "BC_SID_MAX_FILL_DROPS_QUESITION" Then
                            Dim question As String = jsonDictionary(key:="deliveredAmount")
                            If question.StartsWith(value:="3"c) Then
                                message &= "Did you see drops at the end of the tubing?"
                            Else
                                message &= "Remove reservoir and select rewind, restart New reservoir procedure."
                            End If
                        ElseIf Not success Then
                            message = kvp.Value.ToTitle
                        End If
                        item = New SummaryRecord(recordNumber, kvp, message)
                    Case kvp.Key = "autoModeReadinessState"
                        s_autoModeReadinessState =
                            New SummaryRecord(recordNumber,
                                              kvp,
                                              messages:=s_sensorMessages,
                                              messageTableName:=NameOf(s_sensorMessages))
                        listOfSummaryRecords.Add(item:=s_autoModeReadinessState)
                    Case kvp.Key = "autoModeShieldState"
                        item = New SummaryRecord(recordNumber,
                                                  kvp,
                                                  messages:=s_autoModeShieldMessages,
                                                  messageTableName:=NameOf(s_autoModeShieldMessages))
                    Case kvp.Key = "plgmLgsState"
                        item = New SummaryRecord(recordNumber,
                                                 kvp,
                                                 messages:=s_plgmLgsMessages,
                                                 messageTableName:=NameOf(s_plgmLgsMessages))
                    Case New List(Of String)() From {
                        "dateTime",
                        "secondaryTime",
                        "secondaryTimeReminder",
                        "triggeredDateTime"}.StartsWithAnyNoCase(value:=kvp.Key)

                        Dim key As String = kvp.Key
                        item = New SummaryRecord(recordNumber,
                                                 kvp,
                                                 message:=kvp.Value.ParseDate(key).ToShortDateTime)

                    Case kvp.Key.StartsWithNoCase(value:="additionalInfo")
                        HandleComplexItemsString(kvp,
                                                 recordNumber,
                                                 key:="additionalInfo",
                                                 listOfSummaryRecords,
                                                 isTitle:=Not kvp.Key.ContainsNoCase(value:="bundle"))
                    Case kvp.Key.StartsWithNoCase(value:="cleared")
                        HandleComplexItemsString(kvp,
                                                 recordNumber,
                                                 key:="cleared",
                                                 listOfSummaryRecords,
                                                 isTitle:=True)
                    Case kvp.Key.StartsWithNoCase(value:="acknowledged")
                        HandleComplexItemsString(kvp,
                                                 recordNumber,
                                                 key:="acknowledged",
                                                 listOfSummaryRecords,
                                                 isTitle:=True)
                    Case kvp.Key.StartsWithNoCase(value:="snoozed")
                        HandleComplexItemsString(kvp,
                                                 recordNumber,
                                                 key:="snoozed",
                                                 listOfSummaryRecords,
                                                 isTitle:=True)

                    Case kvp.Key.StartsWithNoCase(value:="type")
                        item = New SummaryRecord(recordNumber,
                                                 kvp,
                                                 message:=kvp.Value.ToTitle)
                    Case Else
                        item = New SummaryRecord(recordNumber, kvp)
                End Select
                recordNumber += 1
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
            Stop
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

    Friend Function IsSupportedNotificationPlaceholder(name As String) As Boolean
        Return s_notificationPlaceholderValues.ContainsKey(key:=name)
    End Function

    ''' <summary>
    '''  Normalize spacing around colons in the given text while preserving
    '''  time-like tokens (HH:mm or HH:mm:ss). Times are temporarily replaced
    '''  with placeholders before normalization and restored afterwards.
    ''' </summary>
    ''' <param name="input">The input string to normalize.</param>
    ''' <returns>The normalized string with times preserved.</returns>
    Friend Function NormalizeColonSpacingPreservingTimes(input As String) As String
        If input Is Nothing Then
            Return input
        End If

        Dim times As New List(Of String)

        ' Replace times with placeholders and collect originals
        ' Use a placeholder pattern less likely to be altered by editors or templates: ##TIME{n}##
        Dim evaluator As MatchEvaluator =
            Function(m As Match)
                times.Add(item:=m.Value)
                Return $"##TIME{times.Count - 1}##"
            End Function

        ' Deliberately does not use \b at the start:
        ' ISO dates may join the time with a literal "T", e.g. 2026-08-26t13:40:31.
        Const timePattern As String =
            "(?<!\d)(?:[01]?\d|2[0-3]):[0-5]\d(?::[0-5]\d)?\b"

        Dim protectedText As String =
            Regex.Replace(input, pattern:=timePattern, evaluator)

        protectedText =
            Regex.Replace(input:=protectedText, pattern:="\s*:\s*", replacement:=" : ")

        Dim evaluator1 As MatchEvaluator =
            Function(m As Match)
                Dim index As Integer = CInt(m.Groups(groupnum:=1).Value)
                Return If(index >= 0 AndAlso index < times.Count,
                          times(index),
                          m.Value)
            End Function

        ' Restore original times from the collected list
        Return Regex.Replace(input:=protectedText,
                             pattern:="##TIME(\d+)##",
                             evaluator:=evaluator1)
    End Function

    ''' <summary>
    '''  Translates the notification message ID to a user-friendly message.
    '''  It replaces variables in parentheses with their corresponding values
    '''  from the JSON dictionary.
    ''' </summary>
    ''' <param name="jsonDictionary">
    '''  A dictionary containing additional information for the notification.
    ''' </param>
    ''' <param name="faultId">The fault ID to translate.</param>
    ''' <param name="message">Receives the translated notification message.</param>
    ''' <param name="lookups">The lookup source, or <see langword="Nothing"/> to use the application source.</param>
    ''' <returns><see langword="True"/> if a template was found; otherwise <see langword="False"/>.</returns>
    Friend Function TranslateNotificationMessageId(jsonDictionary As Dictionary(Of String, String),
                                                   faultId As String,
                                                   ByRef message As String,
                                                   Optional lookups As PersistentLookups = Nothing) As Boolean

        If String.IsNullOrEmpty(value:=faultId) OrElse Not Char.IsAsciiDigit(c:=faultId(index:=0)) Then
            Return False
        End If

        ' Initialize variables to hold extracted values Pre-Flex
        Dim basalName As String = Nothing
        Dim bgValue As String = Nothing
        Dim alertClearType As String = Nothing
        Dim criticalLow As String = Nothing
        Dim deliveredAmount As String = Nothing
        Dim lastSetChange As String = Nothing
        Dim lowLimit As String = Nothing
        Dim notDeliveredAmount As String = Nothing
        Dim programmedAmount As String = Nothing
        Dim reminderName As String = Nothing
        Dim secondaryTime As String = Nothing
        Dim secondaryTimeReminder As String = Nothing
        Dim sg As String = Nothing
        Dim sg64 As String = If(NativeMmolL,
                                "3.5 mmol/L",
                                "64 mg/dL")
        Dim sensorUpdateTime As String = Nothing
        Dim triggeredDate As String = Nothing
        Dim unitsRemaining As String = Nothing

        ' Initialize variables to hold extracted values Post-Flex
        Dim activeInsulinResetTime As String = Nothing
        Dim resolved As String = Nothing
        If lookups Is Nothing Then
            lookups = Form1.ErrorLookups
        End If
        Dim template As PersistentLookups.NotificationTemplate = lookups.ErrorLookup(faultId)
        If template Is Nothing Then
            Return False
        End If
        message = template.Message
        Dim list As IReadOnlyList(Of String) = template.Placeholders
        If list.Count = 0 Then
            Return True
        End If
        Dim triggerTime As TimeOnly
        Dim key As String = "triggeredDateTime"
        If list.Contains(value:=key) Then
            If Not jsonDictionary.TryGetValue(key, value:=triggeredDate) Then
                key = "dateTime"
                jsonDictionary.TryGetValue(key, value:=triggeredDate)
            End If
            If triggeredDate IsNot Nothing Then
                Dim time As Date = triggeredDate.ParseDate(key)
                triggerTime = TimeOnly.FromDateTime(time)
                triggeredDate = $" {time.ToNotificationString}"
            End If
        End If
        If list.Contains(value:="activeInsulinResetTime") Then
            key = "dateTime"
            If jsonDictionary.TryGetValue(key, value:=activeInsulinResetTime) Then
                Dim endTime As Date = activeInsulinResetTime.ParseDate(key).AddHours(value:=2)
                activeInsulinResetTime = $" {endTime.ToString(format:=s_timeWithMinuteFormat)}"
            End If
        End If

        Dim additionalInfo As New Dictionary(Of String, String)(Comparer)
        key = "AdditionalInfo"
        Dim jsonString As String = String.Empty
        If jsonDictionary.TryGetValue(key, value:=jsonString) Then
            Dim entries As Dictionary(Of String, JsonElement) =
                jsonString.FromJson(Of Dictionary(Of String, JsonElement))()
            If entries Is Nothing Then
                message = "Notification additional information must be a JSON object."
                Throw New InvalidOperationException(message)
            End If
            For Each entry As KeyValuePair(Of String, JsonElement) In entries
                additionalInfo.Add(entry.Key, value:=entry.Value.ElementToString())
            Next
        Else
            Const prefix As String = "AdditionalInfo:"
            For Each kvp As KeyValuePair(Of String, String) In jsonDictionary
                If kvp.Key.StartsWithNoCase(value:=prefix) Then
                    additionalInfo.Add(key:=kvp.Key.Substring(startIndex:=prefix.Length), value:=kvp.Value)
                End If
            Next
        End If

        For Each keyWord As String In list
            Dim value As String = Nothing
            Select Case keyWord
                Case "resolved"
                    If jsonDictionary.TryGetValue(key:="acknowledged", value:=value) Then
                        Dim record As AcknowledgedRecord = value.FromJson(Of AcknowledgedRecord)()
                        If record IsNot Nothing Then
                            resolved = $" {record.Time.ToNotificationString}"
                        End If
                    End If
                Case "alertClearType"
                    If additionalInfo.TryGetValue(key:="alert_clear_type", value:=value) OrElse
                       additionalInfo.TryGetValue(key:="alertClearType", value:=value) Then
                        alertClearType = value.ToTitle()
                    End If
                Case "secondaryTime"
                    key = NameOf(ActiveNotification.SecondaryTime)
                    If jsonDictionary.TryGetValue(key, value:=value) Then
                        Dim time As Date = value.ParseDate(key)
                        triggerTime = TimeOnly.FromDateTime(time)
                        secondaryTime = $" {time.ToNotificationString}"
                    End If
                Case "secondaryTimeReminder"
                    If additionalInfo.TryGetValue(key:="secondaryTimeReminder", value:=value) OrElse
                       additionalInfo.TryGetValue(key:="secondaryTime", value:=value) Then
                        secondaryTimeReminder = value.FormatTimeText()
                    End If
                Case "basalName"
                    If additionalInfo.TryGetValue(key:=keyWord, value:=value) Then
                        basalName = value.ToTitle(separateDigits:=True)
                    End If
                Case "bgValue"
                    additionalInfo.TryGetValue(key:=keyWord, value:=bgValue)
                Case "criticalLow"
                    additionalInfo.TryGetValue(key:=keyWord, value:=criticalLow)
                Case "deliveredAmount"
                    additionalInfo.TryGetValue(key:=keyWord, value:=deliveredAmount)
                Case "lastSetChange"
                    If additionalInfo.TryGetValue(key:=keyWord, value:=value) Then
                        lastSetChange = s_oneToNineteen(index:=CInt(value)).ToTitle()
                    End If
                Case "notDeliveredAmount"
                    additionalInfo.TryGetValue(key:=keyWord, value:=notDeliveredAmount)
                Case "programmedAmount"
                    additionalInfo.TryGetValue(key:=keyWord, value:=programmedAmount)
                Case "reminderName"
                    additionalInfo.TryGetValue(key:=keyWord, value:=reminderName)
                Case "sensorUpdateTime"
                    If additionalInfo.TryGetValue(key:=keyWord, value:=value) Then
                        sensorUpdateTime = GetSensorUpdateTime(value)
                    End If
                Case "sg"
                    If additionalInfo.TryGetValue(key:=keyWord, value:=sg) AndAlso faultId = "827" Then
                        lowLimit = If(CSng(sg) < 65 AndAlso CSng(sg) > 20, "64 mg/dL", "3.5  mmol/L")
                    ElseIf sg Is Nothing AndAlso faultId = "787" AndAlso
                           CurrentPdf IsNot Nothing AndAlso CurrentPdf.LowAlerts IsNot Nothing Then
                        Dim lowAlertRec As LowAlertRecord = LowAlertsRecord.GetLowAlertRecord(triggerTime)
                        If lowAlertRec IsNot Nothing Then
                            lowLimit = $"{lowAlertRec.LowLimit} {BgUnits()}"
                        End If
                    End If
                Case "lowLimit"
                    If additionalInfo.TryGetValue(key:=keyWord, value:=value) Then
                        lowLimit = value
                    End If
                Case "unitsRemaining"
                    additionalInfo.TryGetValue(key:=keyWord, value:=unitsRemaining)
            End Select
        Next

        Dim replacements As Dictionary(Of String, String) = CreateNotificationPlaceholderValues()
        replacements(key:="alertClearType") = alertClearType
        replacements(key:="basalName") = basalName
        replacements(key:="bgValue") = bgValue
        replacements(key:="criticalLow") = criticalLow
        replacements(key:="deliveredAmount") = deliveredAmount
        replacements(key:="lastSetChange") = lastSetChange
        replacements(key:="lowLimit") = lowLimit
        replacements(key:="notDeliveredAmount") = notDeliveredAmount
        replacements(key:="programmedAmount") = programmedAmount
        replacements(key:="reminderName") = reminderName
        replacements(key:="resolved") = resolved
        replacements(key:="secondaryTime") = secondaryTime
        replacements(key:="secondaryTimeReminder") = secondaryTimeReminder
        replacements(key:="sensorUpdateTime") = sensorUpdateTime
        replacements(key:="suspendedSince") = s_suspendedSince
        replacements(key:="sg") = sg
        replacements(key:="sg64") = sg64
        replacements(key:="triggeredDateTime") = triggeredDate
        replacements(key:="activeInsulinResetTime") = activeInsulinResetTime
        replacements(key:="units") = BgUnits()
        replacements(key:="unitsRemaining") = unitsRemaining
        replacements(key:="vbCrLf") = vbCrLf
        message = template.Render(values:=replacements)
        Return True
    End Function

End Module
