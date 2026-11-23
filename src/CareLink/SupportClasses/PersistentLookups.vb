' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Text.Json
Imports System.Text.RegularExpressions

Public Class PersistentLookups

    ''' <summary>
    '''  These messages have parameters in (), (units) will be replaced by localized units.
    ''' </summary>
    Private Shared ReadOnly s_notificationMessages As New Dictionary(Of Integer, String) From {
        {2, "Pump Error. Delivery Stopped"},
        {3, "Battery out of limit"},
        {4, "Delivery stopped. Check BG"},
        {5, "Pump battery depleted. Insulin delivery stopped"},
        {6, "Insert battery(triggeredDateTime). Delivery stopped." &
               " Insert a new battery now."},
        {7, "Insulin flow blocked(triggeredDateTime). Check BG. Consider testing keytones.  " &
               "Check reservior and infusion set."},
        {11, "Battery failed(triggeredDateTime). Replace Pump Battery Now."},
        {12, "Auto suspend Limit reached(triggeredDateTime). " &
               "Insulin delivery suspended. No buttons pressed within " &
               "time set in Auto Suspend."},
        {16, "Pump reset. Insulin delivery stopped"},
        {19, "Loading Incomplete During Infusion Set Change(triggeredDateTime)"},
        {24, "Critical Pump Error. Stop Pump Use. Use Other Treatment"},
        {25, "Pump Power Error. Record Settings"},
        {29, "Pump Restarted. Delivery Stopped"},
        {37, "Pump Motor Error. Delivery Stopped"},
        {43, "Pump motor error. Insulin delivery stopped"},
        {50, "Bolus stopped"},
        {51, "Bolus stopped(triggeredDateTime). Cannot resume bolus for cannula fill, " &
                 "(deliveredAmount) of (programmedAmount) U delivered." &
                 $"{vbCrLf}(notDeliveredAmount) U not delivered. " &
                 "If needed, enter values again."},
        {52, "Delivery Limit Exceeded. Check BG"},
        {55, "Pump battery failed. Replace battery"},
        {57, "Pump Battery Not Compatible"},
        {58, "Insert A New AA Battery"},
        {59, "Button error"},
        {61, "Stuck button(triggeredDateTime). Button pressed for more than 3 minutes."},
        {62, "New Notification Received From Pump"},
        {66, "No Reservoir Detected During Infusion Set Change"},
        {69, "Loading incomplete(triggeredDateTime)." &
                 " Restart the Reservoir & Set procedure"},
        {71, "Unknown notification received from pump(triggeredDateTime). " &
                 "Delivered Amount (deliveredAmount)."},
        {73, "Replace Pump Battery Now"},
        {74, "Finish loading"},
        {77, "Pump Settings Error. Delivery Stopped"},
        {81, "Replace pump battery now"},
        {82, "Low Reservoir"},
        {83, "Check BG"},
        {84, "Insert battery(triggeredDateTime). Delivery stopped. " &
                 "Insert a new battery now."},
        {85, "Max fill reached"},
        {86, "Weak battery detected"},
        {87, "Missed bolus"},
        {88, "Silenced sensor alert. Check alarm history"},
        {100, "Bolus Not delivered. Bolus entry timed out before delivery. " &
                 "If bolus was intended, enter values again."},
        {103, "BG Check Reminder"},
        {104, "Battery low pump(triggeredDateTime). Replace battery soon."},
        {105, "Low Reservoir(triggeredDateTime). (unitsRemaining) units remaining. " &
                 "Change reservoir."},
        {107, "Missed Meal Bolus Reminder"},
        {109, "Set change reminder(triggeredDateTime). (lastSetChange) days since" &
                 " last set change. Time to change the infusion set."},
        {110, "Silenced Sensor Alert. Check Alarm History"},
        {113, "Reservoir estimate at (unitsRemaining) U(triggeredDateTime). " &
                 "To ensure insulin delivery, change reservoir."},
        {117, "Active Insulin Cleared"},
        {128, "Sensor grace period starting soon(triggeredDateTime). " &
                 $"Less than (secondaryTimeReminder) before grace periods starts.{vbCrLf}Change sensor soon."},
        {130, "Rewind Required(triggeredDateTime). Delivery stopped. " &
                 "Rewind was required due to pump error. Select OK to continue."},
        {140, "Delivery Suspended. Connect Infusion Set"},
        {775, "Enter BG now(triggeredDateTime). Enter BG to calibrate sensor."},
        {776, "Calibration Not accepted(triggeredDateTime). " &
                 "Wait at least 15 minutes. Wash hands, test BG again and calibrate."},
        {777, "Change Sensor(triggeredDateTime). Sensor not working properly. " &
                 "Insert new sensor."},
        {779, "Recharge Transmitter Now"},
        {780, "Lost sensor signal(triggeredDateTime). Move pump closer to sensor. " &
                 "May take 15 minutes to find signal."},
        {781, "Possible signal interface(triggeredDateTime). " &
                 "Move away from electronic devices. May take 15 minutes to find signal."},
        {784, "Raise Alert(triggeredDateTime). Sensor glucose raising rapidly."},
        {787, "Alert on low (sg) (units)(triggeredDateTime). Low sensor glucose. Check BG."},
        {794, "Sensor expired(triggeredDateTime). Insert New sensor."},
        {795, "Lost Sensor Signal. Check Transmitter"},
        {796, "No Sensor Signal"},
        {797, "Sensor connected(triggeredDateTime). Start new sensor."},
        {798, "Sensor connected(triggeredDateTime). If new sensor, select Start New. " &
                 "If not, select Reconnect."},
        {801, "Sensor updating(triggeredDateTime), Updating will take (sensorUpdateTime). " &
                 $"Monitor BG.{vbCrLf}Entered BGs will not calibrate the sensor, " &
                 "but can still be used for therapy."},
        {802, "Alert on low (sg) (units)(triggeredDateTime)." &
                 " Low sensor glucose. Check BG."},
        {803, "Alert on low (sg) (units)(triggeredDateTime). Low sensor glucose. " &
                 "Insulin delivery suspended since (suspendedSince). Check BG."},
        {805, "Alert before low(triggeredDateTime). " &
                 "Sensor glucose approaching Low Limit. Check BG."},
        {807, "Basal delivery resumed at (secondaryTime) after suspend by sensor, " &
                 "Check BG."},
        {809, "Suspend on low(triggeredDateTime). Delivery stopped. " &
                 "Sensor glucose (sg) (units). Check BG."},
        {811, "Suspend before low. Delivery stopped. " &
                 "Sensor glucose approaching Low Limit. Check BG."},
        {812, "Call for emergency(triggeredDateTime)."},
        {814, "Basal Resumed. SG Still Under Low Limit. Check BG"},
        {815, "Low Limit Changed. Basal Manually Resumed. Check BG"},
        {816, "Alert on high (sg) (units)(triggeredDateTime). High sensor glucose. " &
                 "Check BG."},
        {817, "Alert before high(triggeredDateTime). Sensor glucose approaching " &
                 "High Limit. Check BG."},
        {819, "Auto Mode exit(triggeredDateTime). (basalName) started. " &
                 "Would you Like to review Auto Mode Readiness Screen?"},
        {820, "SmartGuard exit(triggeredDateTime). (basalName) started. " &
                 "Would you Like to review SmartGuard Checklist?"},
        {821, "Enter BG now(triggeredDateTime). SmartGuard has reached the time limit " &
                 "for minimum delivery rate. Enter BG to continue in SmartGuard."},
        {822, "SmartGuard maximum delivery. Auto Mode has been at maximum " &
                 "delivery for 4 hours. Enter BG to continue in SmartGuard."},
        {823, "High Sensor Glucose(triggeredDateTime). BG has been high over 1 hour. " &
                 "Change infusion set. Check Ketones. Monitor BG."},
        {827, "Low Sg (sg) (units)(triggeredDateTime) SG is under (lowLimit). " &
                 "Check BG and treat"},
        {829, "BG required(triggeredDateTime). Enter a New BG for SmartGuard."},
        {831, "Enter BG now(triggeredDateTime). Enter a BG to continue in SmartGuard."},
        {832, "Calibration Required"},
        {833, "Bolus recommended(triggeredDateTime). For (bgValue) (units) entered, " &
                 "a correction bolus is recommended. Select Bolus to program a bolus."},
        {838, "Calibration not accepted(triggeredDateTime).Sensor information is " &
                 "unavailable for up to 2 hours. Entered BGs may not calibrate the " &
                 "sensor but can be used for therapy."},
        {841, "Enter BG now(triggeredDateTime). Enter a BG to continue in SmartGuard."},
        {869, "Reminder time to take (reminderName)."},
        {870, "Recharge transmitter within 24 hours(triggeredDateTime)."}}

    Private Shared ReadOnly s_notificationMessagesFlex As New Dictionary(Of Decimal, String) From {
            {1D, "Urgent low sensor glucose(triggeredDateTime), your sensor glucose is below (sg64)."},
            {1.2D, "Change Infusion Set(triggeredDateTime)."},
            {1.05D, "Unknown(triggeredDateTime) 1.050"},
            {1.205D, "No SG values (> 30 minutes)(triggeredDateTime), (alertClearType)"},
            {1.301D, "Pump error restart needed(triggeredDateTime), (alertClearType)"},
            {1.307D, $"Time setting Error(triggeredDateTime), Insulin delivery stopped And SG unavailable.{vbCrLf}" &
                      "Time settings need To be transferred To pump"},
            {1.31D, "Insulin flow blocked(triggeredDateTime), (alertClearType)"},
            {1.312D, "Active insulin reset To 0, Active insulin will be incorrect until(activeInsulinResetTime)."},
            {2.209D, $"Check blood glucose With meter(triggeredDateTime),{vbCrLf}" &
                      "Sensor glucose Not available until a BG Is accepted."},
            {2.21D, "blood glucose Not accepted(triggeredDateTime). Wait at least a 15 minutes before trying again."},
            {3.221D, $"Sensor grace period ending soon(triggeredDateTime), " &
                      "you will need To change your sensor within 12 hours."},
            {2.253D, "Bolus Not delivered (alertClearType)"},
            {2.326D, "Reservoir Low(triggeredDateTime), Change reservoir soon."},
            {3.327D, "Low reservoir(triggeredDateTime)."},
            {3.328D, "Change Infusion Set(triggeredDateTime). Rewind required To Resume basal delivery."},
            {3.329D, "Prolonged low sensor glucose?"},
            {3.33D, "Sensor glucose approaching high limit?"},
            {3.331D, "Sensor glucose approaching low limit?"},
            {3.332D, "Sensor glucose raising rapidly?"},
            {3.333D, "Sensor glucose falling rapidly?"},
            {3.334D, "Sensor glucose under low limit?"},
            {3.335D, "Sensor glucose over high limit?"},
            {2.34D, "Delivery suspended(triggeredDateTime), Insulin delivery has been stopped For > 30 minutes."},
            {2.344D, "Pump suspended(triggeredDateTime), insulin delivery has been stopped For > 30 minutes."},
            {2.35D, "Blous stopped(triggeredDateTime). (alertClearType)!"},
            {2.353D, "Bolus Not delivered(triggeredDateTime), due To time out. (alertClearType)!"},
            {3.38D, "Phones times zone has changed.(triggeredDateTime). (alertClearType)!."},
            {2.5D, $"Reservoir Low(triggeredDateTime). (unitsRemaining) units remaining.{vbCrLf}" &
                      "Change reservoir?"},
            {4.6D, "Lost pump communications at(triggeredDateTime). (alertClearType)!."}
            }

    Friend NotInheritable Class NotificationTemplate

        Friend Sub New(message As String)
            Me.Message = message
            Dim selector As Func(Of Match, String) =
                Function(match As Match)
                    Return match.Groups(groupnum:=1).Value
                End Function
            Me.Placeholders = s_placeholderRegex.Matches(input:=message).
                Cast(Of Match)().Select(selector).
                Distinct(Comparer).ToArray()
        End Sub

        Friend ReadOnly Property Message As String
        Friend ReadOnly Property Placeholders As IReadOnlyList(Of String)

        Friend Function Render(values As IReadOnlyDictionary(Of String, String)) As String
            Return s_placeholderRegex.Replace(Me.Message,
                evaluator:=Function(match As Match)
                               Dim value As String = Nothing
                               Dim key As String = match.Groups(groupnum:=1).Value
                               Return If(values.TryGetValue(key, value) AndAlso value IsNot Nothing,
                              value,
                              match.Value)
                           End Function)
        End Function

    End Class

    Private Shared ReadOnly s_placeholderRegex As New Regex(pattern:="\(([a-z][A-Za-z0-9]*)\)",
                                                            options:=RegexOptions.CultureInvariant)

    Private Shared ReadOnly s_legacyTemplates As Dictionary(Of Integer, NotificationTemplate) =
        CreateTemplates(s_notificationMessages)

    Private Shared ReadOnly s_flexTemplates As Dictionary(Of Decimal, NotificationTemplate) =
        CreateTemplates(s_notificationMessagesFlex)

    Private ReadOnly _flexErrorFileName As String
    Private ReadOnly _flexErrors As Dictionary(Of Decimal, NotificationTemplate)
    Private ReadOnly _legacyErrorFileName As String
    Private ReadOnly _legacyErrors As Dictionary(Of Integer, NotificationTemplate)
    Private ReadOnly _prompt As Func(Of String, String, String)
    Private ReadOnly _confirm As Func(Of String, DialogResult)

    ''' <summary>
    '''  Loads user notification templates from the project data directory.
    ''' </summary>
    Public Sub New()
        Me.New(GetProjectDataDirectory(),
               Function(id As String, current As String) Interaction.InputBox(
                   Prompt:=$"Enter a description for error {id}:",
                   Title:="New Error lookup value", DefaultResponse:=current),
               Function(warning As String) MessageBox.Show(
                   text:=warning, caption:="Unsupported placeholders",
                   buttons:=MessageBoxButtons.YesNoCancel, icon:=MessageBoxIcon.Warning))
    End Sub

    Friend Sub New(folder As String, prompt As Func(Of String, String, String),
                   confirm As Func(Of String, DialogResult))
        Directory.CreateDirectory(path:=folder)
        _prompt = prompt
        _confirm = confirm
        _flexErrorFileName = Path.Combine(folder, "FlexErrors.json")
        _legacyErrorFileName = Path.Combine(folder, "LegacyErrors.json")
        _flexErrors = LoadDictionary(_flexErrorFileName, Function(key As String) ParseFlexKey(key))
        _legacyErrors = LoadDictionary(_legacyErrorFileName, Function(key As String) ParseLegacyKey(key))
    End Sub

    Private Shared Function CreateTemplates(Of TKey)(messages As Dictionary(Of TKey, String)) As Dictionary(Of TKey, NotificationTemplate)
        Return messages.ToDictionary(Function(pair As KeyValuePair(Of TKey, String)) pair.Key,
                                     Function(pair As KeyValuePair(Of TKey, String)) New NotificationTemplate(pair.Value))
    End Function

    Private Shared Function ParseFlexKey(key As String) As Decimal
        Dim value As Decimal
        If Not Regex.IsMatch(key, "^[0-9]+(?:\.[0-9]+)?$") OrElse
           Not Decimal.TryParse(key, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, value) Then
            Throw New FormatException($"Invalid Flex fault ID: {key}")
        End If
        Dim parts As String() = key.Split("."c)
        Dim whole As String = parts(0).TrimStart("0"c)
        If whole.Length = 0 Then
            whole = "0"
        End If
        Dim fraction As String = If(parts.Length = 2, parts(1).TrimEnd("0"c), String.Empty)
        Dim normalized As String = whole & If(fraction.Length > 0, "." & fraction, String.Empty)
        If normalized <> value.ToString("0.############################", CultureInfo.InvariantCulture) Then
            Throw New FormatException($"Flex fault ID cannot be represented exactly: {key}")
        End If
        Return value
    End Function

    Private Shared Function ParseLegacyKey(key As String) As Integer
        Dim value As Integer
        If Not Regex.IsMatch(key, "^[0-9]+$") OrElse
           Not Integer.TryParse(key, NumberStyles.None, CultureInfo.InvariantCulture, value) Then
            Throw New FormatException($"Invalid legacy fault ID: {key}")
        End If
        Return value
    End Function

    Private Shared Function LoadDictionary(Of TKey)(fileName As String, parseKey As Func(Of String, TKey)) As Dictionary(Of TKey, NotificationTemplate)
        Dim values As New Dictionary(Of TKey, NotificationTemplate)()
        If Not File.Exists(path:=fileName) Then
            Return values
        End If

        Using document As JsonDocument = JsonDocument.Parse(File.ReadAllText(path:=fileName))
            If document.RootElement.ValueKind <> JsonValueKind.Object Then
                Throw New InvalidDataException($"The lookup file must contain an object: {fileName}")
            End If
            For Each item As JsonProperty In document.RootElement.EnumerateObject()
                Dim key As TKey = parseKey(item.Name)
                If item.Value.ValueKind <> JsonValueKind.String OrElse String.IsNullOrWhiteSpace(item.Value.GetString()) Then
                    Throw New InvalidDataException($"Invalid message for fault ID {item.Name} in {fileName}.")
                End If
                If Not values.TryAdd(key, New NotificationTemplate(item.Value.GetString())) Then
                    Throw New InvalidDataException($"Duplicate fault ID {item.Name} in {fileName}.")
                End If
            Next
        End Using
        Return values
    End Function

    Private Function LookupCore(Of TKey)(faultId As String, key As TKey,
                                         builtIn As Dictionary(Of TKey, NotificationTemplate),
                                         values As Dictionary(Of TKey, NotificationTemplate),
                                         fileName As String) As NotificationTemplate
        Dim template As NotificationTemplate = Nothing
        If builtIn.TryGetValue(key, template) OrElse values.TryGetValue(key, template) Then
            Return template
        End If

        Dim message As String = String.Empty
        Do
            message = _prompt(faultId, message)
            If String.IsNullOrWhiteSpace(message) Then
                Return Nothing
            End If
            template = New NotificationTemplate(message)
            Dim unsupported As String() = template.Placeholders.
                Where(Function(name As String) Not IsSupportedNotificationPlaceholder(name)).ToArray()
            If unsupported.Length = 0 Then
                Exit Do
            End If
            Dim warning As String = $"Unsupported placeholders: {String.Join(", ", unsupported)}.{vbCrLf}" &
                "These will remain in the message until a future release supports them." & vbCrLf &
                "Choose Yes to save anyway, No to edit, or Cancel to discard."
            Select Case _confirm(warning)
                Case DialogResult.Yes
                    Exit Do
                Case DialogResult.No
                    Continue Do
                Case Else
                    Return Nothing
            End Select
        Loop

        values.Add(key, template)
        Try
            SaveDictionary(fileName, values.ToDictionary(
                Function(pair As KeyValuePair(Of TKey, NotificationTemplate)) pair.Key,
                Function(pair As KeyValuePair(Of TKey, NotificationTemplate)) pair.Value.Message))
        Catch
            values.Remove(key)
            Throw
        End Try
        Return template
    End Function

    Private Shared Sub SaveDictionary(Of TKey)(fileName As String, values As Dictionary(Of TKey, String))
        Dim json As String = values.ToJson()
        Dim temporaryFile As String = $"{fileName}.tmp"

        Try
            Using stream As New FileStream(path:=temporaryFile,
                                           mode:=FileMode.Create,
                                           access:=FileAccess.Write,
                                           share:=FileShare.None)

                ' Leave the stream open so it can be flushed after the writer.
                Dim encoding As New Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:=False)
                Using writer As New StreamWriter(stream,
                                                 encoding,
                                                 bufferSize:=1024,
                                                 leaveOpen:=True)

                    writer.Write(value:=json)
                End Using

                stream.Flush(flushToDisk:=True)
            End Using

            If File.Exists(path:=fileName) Then
                File.Replace(sourceFileName:=temporaryFile,
                             destinationFileName:=fileName,
                             destinationBackupFileName:=Nothing)
            Else
                File.Move(sourceFileName:=temporaryFile,
                          destFileName:=fileName)
            End If
        Finally
            If File.Exists(path:=temporaryFile) Then
                File.Delete(path:=temporaryFile)
            End If
        End Try
    End Sub

    Friend Function ErrorLookup(faultId As String) As NotificationTemplate
        If String.IsNullOrWhiteSpace(value:=faultId) Then
            Throw New ArgumentException(message:="A fault ID is required.", paramName:=NameOf(faultId))
        End If
        If faultId.Contains(value:="."c) Then
            Dim key As Decimal = ParseFlexKey(faultId)
            Return Me.LookupCore(faultId, key, s_flexTemplates, _flexErrors, _flexErrorFileName)
        End If
        Dim legacyKey As Integer = ParseLegacyKey(faultId)
        Return Me.LookupCore(faultId, legacyKey, s_legacyTemplates, _legacyErrors, _legacyErrorFileName)
    End Function

End Class
