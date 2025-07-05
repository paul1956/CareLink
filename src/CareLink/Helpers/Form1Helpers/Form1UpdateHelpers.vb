' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

''' <summary>
'''  Module containing helper methods for updating and processing data in <see cref="Form1"/>.
''' </summary>
''' <remarks>
'''  <para>
'''   Provides utility functions for data transformation, file name generation, and summary record handling
'''   related to pump and sensor data in the application.
'''  </para>
''' </remarks>
Friend Module Form1UpdateHelpers

    Private ReadOnly s_700Models As New List(Of String) From {
        "MMT-1812",
        "MMT-1880"}

    ''' <summary>
    '''  Converts a date string to a formatted date string using the specified provider,
    '''  or returns an empty string if parsing fails.
    ''' </summary>
    ''' <param name="dateAsString">The date string to parse.</param>
    ''' <param name="key">The key associated with the date value.</param>
    ''' <param name="provider">The format provider to use for formatting the date.</param>
    ''' <returns>
    '''  The formatted date string if parsing succeeds; otherwise, an empty string.
    ''' </returns>
    <Extension>
    Private Function CDateOrDefault(dateAsString As String, key As String, provider As IFormatProvider) As String
        Dim resultDate As Date
        Return If(TryParseDate(dateAsString, resultDate, key),
                  resultDate.ToString(provider),
                  ""
                 )
    End Function

    ''' <summary>
    '''  Converts a percentage value (as a string) representing time out of 24 hours
    '''  to a display string in hours and minutes.
    ''' </summary>
    ''' <param name="rowValue">The percentage value as a string.</param>
    ''' <returns>
    '''  A string describing the time in hours and minutes out of the last 24 hours.
    ''' </returns>
    Private Function ConvertPercent24HoursToDisplayValueString(rowValue As String) As String
        Dim val As Decimal = CDec(Convert.ToInt32(rowValue) * 0.24)
        Dim hours As Integer = Convert.ToInt32(val)
        Dim minutes As Integer = CInt((val Mod 1) * 60)
        Return If(minutes = 0,
                  $"{hours} hours, out of last 24 hours.",
                  $"{hours} hours and {minutes} minutes, out of last 24 hours."
                 )
    End Function

    ''' <summary>
    '''  Gets the display name of a pump model based on its model number.
    ''' </summary>
    ''' <param name="value">The model number of the pump.</param>
    ''' <returns>
    '''  The display name of the pump if recognized; otherwise, "Unknown".
    ''' </returns>
    Friend Function GetPumpName(value As String) As String
        Select Case value
            Case "MMT-1812"
                Return "Medtronic MiniMed™ 740G--mg/dL"
            Case "MMT-1880"
                Return "Medtronic MiniMed™ 770G"
            Case "MMT-1884"
                Return "Medtronic MiniMed™ 780G-US Update"
            Case "MMT-1885"
                Return "Medtronic MiniMed™ 780G-mmol/L"
            Case "MMT-1886"
                Return "Medtronic MiniMed™ 780G-mg/dL"
            Case Else
                Return "Unknown"
        End Select
    End Function

    ''' <summary>
    '''  Generates a possibly unique file name for data export, based on the specified base name, culture, and extension.
    ''' </summary>
    ''' <param name="baseName">The first part of the file name.</param>
    ''' <param name="cultureName">A valid culture name in the form of language-CountryCode.</param>
    ''' <param name="extension">The extension for the file.</param>
    ''' <param name="mustBeUnique">If <see langword="True"/>, ensures the file name is unique.</param>
    ''' <returns>
    '''  A <see cref="FileNameStruct"/> containing the full and short file name, or an empty struct on error.
    ''' </returns>
    ''' <example>
    '''  GetUniqueDataFileName("MyFile", "en-US", "txt", mustBeUnique:=True)
    ''' </example>
    Friend Function GetUniqueDataFileName(
        baseName As String,
        cultureName As String,
        extension As String,
        mustBeUnique As Boolean) As FileNameStruct

        If String.IsNullOrWhiteSpace(baseName) Then
            Throw New ArgumentException($"'{NameOf(baseName)}' cannot be null or whitespace.", paramName:=NameOf(baseName))
        End If

        If String.IsNullOrWhiteSpace(cultureName) Then
            Throw New ArgumentException($"'{NameOf(cultureName)}' cannot be null or whitespace.", paramName:=NameOf(cultureName))
        End If

        If String.IsNullOrWhiteSpace(extension) Then
            Throw New ArgumentException($"'{NameOf(extension)}' cannot be null or whitespace.", paramName:=NameOf(extension))
        End If

        Try
            Dim filenameWithoutExtension As String = $"{baseName}({cultureName}){s_userName}"
            Dim filenameWithExtension As String = $"{filenameWithoutExtension}.{extension}"
            Dim filenameFullPath As String = Path.Join(DirectoryForProjectData, filenameWithExtension)

            If mustBeUnique AndAlso File.Exists(filenameFullPath) Then
                'Get unique file name
                Dim count As Long
                Do
                    count += 1
                    filenameFullPath = Path.Join(DirectoryForProjectData, $"{filenameWithoutExtension}{count}.{extension}")
                    filenameWithExtension = Path.GetFileName(filenameFullPath)
                Loop While File.Exists(filenameFullPath)
            End If

            Return New FileNameStruct(filenameFullPath, filenameWithExtension)
        Catch ex As Exception
            Stop
        End Try
        Return New FileNameStruct

    End Function

    ''' <summary>
    '''  Handles complex items in a key-value row, splitting and processing values for summary records.
    ''' </summary>
    ''' <param name="kvp">The key-value pair row to process.</param>
    ''' <param name="recordNumber">The index of the row.</param>
    ''' <param name="key">The key for the summary record.</param>
    ''' <param name="listOfSummaryRecords">The list to which summary records are added.</param>
    Friend Sub HandleComplexItems(
        kvp As KeyValuePair(Of String, String),
        recordNumber As Single,
        key As String,
        listOfSummaryRecords As List(Of SummaryRecord))

        Dim valueList As String() = GetValueList(kvp.Value)
        For Each e As IndexClass(Of String) In valueList.WithIndex
            Dim message As String = String.Empty
            Dim strings As String() = e.Value.Split(separator:=" = ")
            If kvp.Key.EqualsIgnoreCase("AdditionalInfo") Then
                Dim additionalInfo As Dictionary(Of String, String) = GetAdditionalInformation(jsonString:=kvp.Value)
                If strings(0).EqualsIgnoreCase("sensorUpdateTime") Then
                    message = GetSensorUpdateTime(key:=strings(1))
                End If
            End If
            Dim item As New SummaryRecord(
                recordNumber:=CSng(recordNumber + ((e.Index + 1) / 10)),
                key:=$"{key}:{strings(0).Trim}",
                value:=strings(1).Trim,
                message)
            listOfSummaryRecords.Add(item)
        Next
    End Sub

    ''' <summary>
    '''  Checks if the <see cref="PatientData.MedicalDeviceInformation.ModelNumber"/> is a 700 series model.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if the model number is a 700 series model; otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function Is700Series() As Boolean
        If RecentDataEmpty() Then Return False
        Return s_700Models.Contains(PatientData.MedicalDeviceInformation.ModelNumber)
    End Function

    ''' <summary>
    '''  Checks if the <see cref="RecentData"/> is empty or not.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if <see cref="RecentData"/> is empty; otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function RecentDataEmpty() As Boolean
        Return RecentData Is Nothing OrElse RecentData.Count = 0
    End Function

    ''' <summary>
    '''  Updates the data tables and summary records in the <paramref name="mainForm"/> based on the most recent data.
    ''' </summary>
    ''' <param name="mainForm">The main form instance to update.</param>
    Friend Sub UpdateDataTables(mainForm As Form1)

        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()

        Dim value As String = ""
        If RecentData.TryGetValue("clientTimeZoneName", value) Then
            PumpTimeZoneInfo = CalculateTimeZone(value)
        End If
        Dim bgUnitsNative As String = String.Empty
        Dim bgUnits As String = String.Empty
        If RecentData.TryGetValue("bgUnits", bgUnitsNative) AndAlso
            UnitsStrings.TryGetValue(bgUnitsNative, bgUnits) Then
            NativeMmolL = bgUnits.Equals("mmol/L")
        Else
            Stop
        End If

        If RecentData.TryGetValue("therapyAlgorithmState", value) Then
            s_therapyAlgorithmStateValue = LoadIndexedItems(value)
            InAutoMode = s_therapyAlgorithmStateValue.Count > 0 AndAlso {"AUTO_BASAL", "SAFE_BASAL"}.Contains(s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmState.AutoModeShieldState)))
        End If

        s_listOfSgRecords = If(RecentData.TryGetValue("sgs", value),
            JsonToLisOfSgs(value),
            New List(Of SG))

        mainForm.MaxBasalPerHourLabel.Text =
            If(RecentData.TryGetValue(key:="markers", value),
                CollectMarkers(),
                String.Empty)

        s_systemStatusTimeRemaining = Nothing
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In RecentData.WithIndex()

            Dim kvp As KeyValuePair(Of String, String) = c.Value
            If kvp.Value Is Nothing Then
                kvp = KeyValuePair.Create(kvp.Key, "")
            End If

            Dim key As ServerDataIndexes = CType(c.Index, ServerDataIndexes)
            Dim recordNumber As Single = c.Index
            Dim message As String
            Select Case kvp.Key
                Case NameOf(ServerDataIndexes.clientTimeZoneName)
                    If s_useLocalTimeZone Then
                        PumpTimeZoneInfo = TimeZoneInfo.Local
                    Else
                        PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=PatientData.ClientTimeZoneName)
                        Dim messageButtons As MessageBoxButtons
                        If PumpTimeZoneInfo Is Nothing Then
                            If String.IsNullOrWhiteSpace(kvp.Value.ToString) Then
                                message = $"Your pump appears To be off-line, " &
                                    "some values will be wrong do you want to continue? " &
                                    "If you select OK '{TimeZoneInfo.Local.Id}' will be used as you local time " &
                                    "and you will not be prompted further. Cancel will Exit."
                                messageButtons = MessageBoxButtons.OKCancel
                            Else
                                message = $"Your pump TimeZone '{kvp.Value}' is not recognized, do you want to exit? " &
                                    "If you select No permanently use '{TimeZoneInfo.Local.Id}''? " &
                                    "If you select Yes '{TimeZoneInfo.Local.Id}' will be used " &
                                    "and you will not be prompted further. " &
                                    "No will use '{TimeZoneInfo.Local.Id}' until you restart program. " &
                                    "Cancel will exit program. Please open an issue and provide the name '{kvp.Value}'. " &
                                    "After selecting 'Yes' you can change the behavior under the Options Menu."
                                messageButtons = MessageBoxButtons.YesNoCancel
                            End If
                            Dim result As DialogResult = MessageBox.Show(
                                text:=message,
                                caption:="TimeZone Unknown",
                                buttons:=messageButtons,
                                icon:=MessageBoxIcon.Question)

                            s_useLocalTimeZone = True
                            PumpTimeZoneInfo = TimeZoneInfo.Local
                            Select Case result
                                Case DialogResult.Yes
                                    My.Settings.UseLocalTimeZone = True
                                Case DialogResult.Cancel
                                    mainForm.Close()
                            End Select
                        End If
                    End If
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, key, kvp.Value))

                Case NameOf(ServerDataIndexes.lastName)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.firstName)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, key, value:=PatientData.FirstName))

                Case NameOf(ServerDataIndexes.appModelType)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.appModelNumber)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.currentServerTime)
                    message = kvp.Value.Epoch2DateTimeString
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.conduitSerialNumber)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.conduitBatteryLevel)
                    message = $"Phone battery is at {kvp.Value}%."
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.conduitBatteryStatus)
                    message = $"Phone battery status is {kvp.Value.ToLower()}."
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.lastConduitDateTime)
                    kvp = New KeyValuePair(Of String, String)(
                        key:=NameOf(ServerDataIndexes.lastConduitDateTime),
                        value:=kvp.Value.CDateOrDefault(key:=NameOf(ServerDataIndexes.lastConduitDateTime), Provider))
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message:=$"Phone time is {kvp.Value}"))

                Case NameOf(ServerDataIndexes.lastConduitUpdateServerDateTime)
                    message = kvp.Value.Epoch2DateTimeString
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.medicalDeviceFamily)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.medicalDeviceInformation)
                    HandleComplexItems(
                        kvp,
                        recordNumber,
                        key:="medicalDeviceInformation",
                        listOfSummaryRecords:=s_listOfSummaryRecords)
                    message = $"{ PatientData.MedicalDeviceInformation.DeviceSerialNumber} Details..."
                    mainForm.SerialNumberButton.Text = message

                Case NameOf(ServerDataIndexes.medicalDeviceTime)
                    message = kvp.Value.Epoch2DateTimeString
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime)
                    message = kvp.Value.Epoch2DateTimeString
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.cgmInfo)
                    HandleComplexItems(kvp, recordNumber, key:="cgmInfo", listOfSummaryRecords:=s_listOfSummaryRecords)

                Case NameOf(ServerDataIndexes.calFreeSensor)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.calibStatus)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                        recordNumber,
                                                        kvp,
                                                        messages:=s_calibrationMessages,
                                                        messageTableName:=NameOf(s_calibrationMessages)))

                Case NameOf(ServerDataIndexes.calibrationIconId)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.timeToNextEarlyCalibrationMinutes)
                    If kvp.Value = "15555" Then
                        s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                            recordNumber,
                                                            kvp,
                                                            message:="Calibration Free Sensor"))
                    Else
                        s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))
                    End If

                Case NameOf(ServerDataIndexes.timeToNextCalibrationMinutes),
                     NameOf(ServerDataIndexes.timeToNextCalibrationRecommendedMinutes)
                    If kvp.Value = "-1" Then
                        s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                            recordNumber,
                                                            kvp,
                                                            message:="Calibration Free Sensor"))
                    Else
                        s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))
                    End If

                Case NameOf(ServerDataIndexes.timeToNextCalibHours)
                    Dim timeToNextCalibrationHours As Byte = Byte.Parse(kvp.Value)
                    If timeToNextCalibrationHours = Byte.MaxValue Then
                        s_timeToNextCalibrationMinutes = -1
                        s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                            recordNumber,
                                                            kvp,
                                                            message:="Calibration Free Sensor"))
                    Else
                        s_timeToNextCalibrationMinutes = timeToNextCalibrationHours
                        s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                            recordNumber,
                                                            kvp,
                                                            message:=$"{CInt(timeToNextCalibrationHours).ToHours}"))
                    End If

                Case NameOf(ServerDataIndexes.finalCalibration)
                    If Boolean.Parse(kvp.Value) Then
                        s_timeToNextCalibrationMinutes = -1
                    End If
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.sensorDurationMinutes)
                    Dim sensorDurationMinutes As Integer = CInt(kvp.Value.ToString())
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       kvp,
                                                       message:=sensorDurationMinutes.MinutesToDaysHoursMinutes))

                Case NameOf(ServerDataIndexes.sensorDurationHours)
                    Dim sensorDurationHours As Integer = CInt(kvp.Value.ToString())
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                            recordNumber,
                                                            kvp,
                                                            message:=sensorDurationHours.HoursToDaysAndHours))

                Case NameOf(ServerDataIndexes.transmitterPairedTime)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.systemStatusTimeRemaining)
                    s_systemStatusTimeRemaining = New TimeSpan(
                                                    hours:=0,
                                                    minutes:=PatientData.SystemStatusTimeRemaining,
                                                    seconds:=0)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.gstBatteryLevel)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message:=$"Phone battery is at {kvp.Value}%."))

                Case NameOf(ServerDataIndexes.pumpBannerState)
                    s_pumpBannerStateValue = JsonToDictionaryList(kvp.Value)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, key, value:=ClickToShowDetails))
                    mainForm.PumpBannerStateLabel.Visible = s_pumpBannerStateValue.Count > 0

                Case NameOf(ServerDataIndexes.therapyAlgorithmState)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, key, value:=ClickToShowDetails))

                Case NameOf(ServerDataIndexes.reservoirLevelPercent)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       kvp,
                                                       message:=$"Reservoir is {PatientData.ReservoirLevelPercent}% full."))

                Case NameOf(ServerDataIndexes.reservoirAmount)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       kvp,
                                                       message:=$"Full reservoir holds {PatientData.ReservoirAmount}U."))

                Case NameOf(ServerDataIndexes.pumpSuspended)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.pumpBatteryLevelPercent)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message:=$"Pump battery is at {kvp.Value}%."))

                Case NameOf(ServerDataIndexes.reservoirRemainingUnits)
                    message = $"Reservoir has {PatientData.ReservoirRemainingUnits}U remaining."
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.conduitInRange)
                    message = $"Phone {If(PatientData.ConduitInRange, "is", "is not")} in range of pump."
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.conduitMedicalDeviceInRange)
                    message = $"Pump {If(CBool(kvp.Value), "is", "is not")} in range of phone"
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.conduitSensorInRange)
                    message = $"Transmitter {If(PatientData.ConduitSensorInRange, "is", "is not")} in range of pump."
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.systemStatusMessage)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                        recordNumber,
                                                        kvp,
                                                        messages:=s_sensorMessages,
                                                        messageTableName:=NameOf(s_sensorMessages)))

                Case NameOf(ServerDataIndexes.sensorState)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                        recordNumber,
                                                        kvp,
                                                        messages:=s_sensorMessages,
                                                        messageTableName:=NameOf(s_sensorMessages)))

                Case NameOf(ServerDataIndexes.gstCommunicationState)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.pumpCommunicationState)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.timeFormat)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))
                Case NameOf(ServerDataIndexes.bgUnits)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message:=GetBgUnitsString()))

                Case NameOf(ServerDataIndexes.maxAutoBasalRate)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.maxBolusAmount)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.sgBelowLimit)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.approvedForTreatment)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.lastAlarm)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       key,
                                                       value:=ClickToShowDetails))
                    s_lastAlarmValue = LoadIndexedItems(jsonString:=kvp.Value)

                Case NameOf(ServerDataIndexes.activeInsulin)
                    s_activeInsulin = PatientData.ActiveInsulin
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       key,
                                                       value:=ClickToShowDetails))

                Case NameOf(ServerDataIndexes.basal)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       key,
                                                       value:=ClickToShowDetails))
                    s_basalList(0) = If(String.IsNullOrWhiteSpace(kvp.Value), New Basal, PatientData.Basal)
                Case NameOf(ServerDataIndexes.lastSensorTime)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.lastSG)
                    s_lastSg = New SG(PatientData.LastSG)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       key,
                                                       value:=ClickToShowDetails))

                Case NameOf(ServerDataIndexes.lastSGTrend)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.limits)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                       recordNumber,
                                                       key,
                                                       value:=ClickToShowDetails))
                    s_listOfLimitRecords = PatientData.Limits

                Case NameOf(ServerDataIndexes.belowHypoLimit)
                    message = $"Time below limit = {ConvertPercent24HoursToDisplayValueString(kvp.Value)}"
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.aboveHyperLimit)
                    message = $"Time above limit = {ConvertPercent24HoursToDisplayValueString(kvp.Value)}"
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.timeInRange)
                    message = $"Time in range = {ConvertPercent24HoursToDisplayValueString(kvp.Value)}"
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp, message))

                Case NameOf(ServerDataIndexes.averageSGFloat)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.averageSG)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.markers)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, key, value:=ClickToShowDetails))

                Case NameOf(ServerDataIndexes.sgs)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, key, value:=ClickToShowDetails))
                    s_lastSgValue = 0
                    If s_listOfSgRecords.Count > 2 Then
                        s_lastSgValue = s_listOfSgRecords.Item(s_listOfSgRecords.Count - 2).sg
                    End If

                Case NameOf(ServerDataIndexes.notificationHistory)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                        recordNumber:=CSng(c.Index + 0.1),
                                                        key:="activeNotification"))
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(
                                                        recordNumber:=CSng(c.Index + 0.2),
                                                        key:="clearedNotifications"))
                    s_notificationHistoryValue = LoadIndexedItems(kvp.Value)

                Case NameOf(ServerDataIndexes.sensorLifeText)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.sensorLifeIcon)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case Else
                    Stop
            End Select
        Next

    End Sub

    ''' <summary>
    '''  Updates the marker tabs in the <paramref name="mainForm"/> with the latest marker data.
    ''' </summary>
    ''' <param name="mainForm">The main form instance to update.</param>
    Friend Sub UpdateMarkerTabs(mainForm As Form1)
        With mainForm
            .TableLayoutPanelAutoBasalDelivery.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfAutoBasalDeliveryMarkers),
                className:=NameOf(AutoBasalDelivery), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelAutoModeStatus.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfAutoModeStatusMarkers),
                className:=NameOf(AutoModeStatus), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelBgReadings.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfBgReadingMarkers),
                className:=NameOf(BgReading), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelInsulin.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfInsulinMarkers),
                className:=NameOf(Insulin), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelMeal.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfMealMarkers),
                className:=NameOf(Meal), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelCalibration.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfCalibrationMarkers),
                className:=NameOf(Calibration), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelLowGlucoseSuspended.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfLowGlucoseSuspendedMarkers),
                className:=NameOf(LowGlucoseSuspended), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelTimeChange.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_listOfTimeChangeMarkers),
                className:=NameOf(TimeChange), rowIndex:=ServerDataIndexes.markers)

            DisplayDataTableInDGV(
                realPanel:=Nothing,
                table:=ClassCollectionToDataTable(classCollection:=s_listOfBasalPerHour),
                dgv:=mainForm.DgvBasalPerHour,
                rowIndex:=0)
            mainForm.DgvBasalPerHour.AutoSize = True
        End With

    End Sub

    ''' <summary>
    '''  Updates the pump banner state tab in the <paramref name="mainForm"/> with the latest banner state data.
    ''' </summary>
    ''' <param name="mainForm">The main form instance to update.</param>
    Friend Sub UpdatePumpBannerStateTab(mainForm As Form1)
        Dim listOfBannerState As New List(Of BannerState)
        For Each dic As Dictionary(Of String, String) In s_pumpBannerStateValue
            Dim typeValue As String = ""
            If dic.TryGetValue(key:="type", value:=typeValue) Then
                Dim bannerStateRecord1 As BannerState = DictionaryToClass(Of BannerState)(dic, recordNumber:=listOfBannerState.Count + 1)
                listOfBannerState.Add(bannerStateRecord1)
                mainForm.PumpBannerStateLabel.Font = New Font(FamilyName, emSize:=8.25F, style:=FontStyle.Bold)
                Select Case typeValue
                    Case "TEMP_TARGET"
                        Dim minutes As Integer = bannerStateRecord1.TimeRemaining
                        mainForm.PumpBannerStateLabel.BackColor = Color.Lime
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = $"Target {If(NativeMmolL, "8.3", "150")}  {minutes.ToHours} hr"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "BG_REQUIRED"
                        mainForm.PumpBannerStateLabel.BackColor = Color.CadetBlue
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.ContrastingColor

                        mainForm.PumpBannerStateLabel.Text = "Enter BG Now"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "DELIVERY_SUSPEND"
                        mainForm.PumpBannerStateLabel.BackColor = Color.IndianRed
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = "Delivery Suspended"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "LOAD_RESERVOIR"
                        mainForm.PumpBannerStateLabel.BackColor = Color.Yellow
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = "Load Reservoir"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "PROCESSING_BG"
                        Stop
                    Case "SUSPENDED_BEFORE_LOW", "SUSPENDED_ON_LOW"
                        mainForm.PumpBannerStateLabel.BackColor = Color.IndianRed
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = typeValue.ToTitle()
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        mainForm.PumpBannerStateLabel.Font = New Font(FamilyName, emSize:=7.0F, style:=FontStyle.Bold)
                    Case "TEMP_BASAL"
                        mainForm.PumpBannerStateLabel.BackColor = Color.Lime
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        Dim hours As String = PatientData.PumpBannerState(index:=0).TimeRemaining.ToHours
                        mainForm.PumpBannerStateLabel.Text = $"Temp Basal {hours} hr"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        mainForm.PumpBannerStateLabel.Font = New Font(FamilyName, emSize:=7.0F, style:=FontStyle.Bold)
                    Case "WAIT_TO_ENTER_BG"
                        Stop
                    Case Else
                        If Debugger.IsAttached Then
                            MsgBox(
                                heading:=$"{typeValue} Is unknown banner message!",
                                text:="",
                                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                                title:=GetTitleFromStack(New StackFrame(skipFrames:=0, needFileInfo:=True)))
                        End If
                End Select
                mainForm.PumpBannerStateLabel.ForeColor = ContrastingColor(baseColor:=mainForm.PumpBannerStateLabel.BackColor)
            Else
                Stop
            End If
        Next

        Dim safeBasalDurationStr As String = ""
        If s_therapyAlgorithmStateValue?.TryGetValue(NameOf(TherapyAlgorithmState.SafeBasalDuration), safeBasalDurationStr) Then
            Dim safeBasalDuration As UInteger = CUInt(safeBasalDurationStr)
            If safeBasalDuration > 0 Then
                mainForm.LastSgOrExitTimeLabel.Text = $"Exit In { TimeSpan.FromMinutes(safeBasalDuration).ToFormattedTimeSpan("hr")}"
                mainForm.LastSgOrExitTimeLabel.Visible = True
            End If
        End If
        mainForm.TableLayoutPanelPumpBannerState.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:=listOfBannerState),
            className:=NameOf(BannerState), rowIndex:=ServerDataIndexes.pumpBannerState)
    End Sub

End Module
