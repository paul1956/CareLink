' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

''' <summary>
'''  Module containing helper methods for updating and
'''  processing data in <see cref="Form1"/>.
''' </summary>
''' <remarks>
'''  Provides utility functions for data transformation, file name generation,
'''  and summary record handling related to pump and sensor data in the application.
''' </remarks>
Friend Module Form1UpdateHelpers

    Private ReadOnly s_700Models As New List(Of String) From {
        "MMT-1812",
        "MMT-1880"}

    ''' <summary>
    '''  Converts a date string to a formatted date string using the specified provider,
    '''  or returns an empty string if parsing fails.
    ''' </summary>
    ''' <param name="s">The date string to parse.</param>
    ''' <param name="key">The key associated with the date value.</param>
    ''' <param name="provider">
    '''  The format provider to use for formatting the date.
    ''' </param>
    ''' <returns>
    '''  The formatted date string if parsing succeeds;
    '''  otherwise, an empty string.
    ''' </returns>
    <Extension>
    Private Function CDateOrDefault(
        s As String,
        key As String,
        provider As IFormatProvider) As String

        Dim result As Date
        Return If(TryParseDate(s, key, result),
                  result.ToString(provider),
                  "")
    End Function

    ''' <summary>
    '''  Converts a percentage value (as a string) representing time out of 24 hours
    '''  to a display string in hours and minutes.
    ''' </summary>
    ''' <param name="value">The percentage value as a string.</param>
    ''' <returns>
    '''  A string describing the time in hours and minutes out of the last 24 hours.
    ''' </returns>
    Private Function PercentOf24HoursToString(value As String) As String
        Dim d As Decimal = CDec(Convert.ToInt32(value) * 0.24)
        Dim hours As Integer = CInt(Math.Floor(d))
        Dim minutes As Integer = CInt(d.FractionalPart() * 60)
        Return If(d.FractionalPart().AlmostZero(),
                  $"{hours} hours, out of last 24 hours.",
                  $"{hours} hours and {minutes} minutes, out of last 24 hours.")
    End Function

    Private Sub SetupPumpTimeZoneInfo(
        mainForm As Form1,
        kvp As KeyValuePair(Of String, String))

        If s_useLocalTimeZone Then
            PumpTimeZoneInfo = TimeZoneInfo.Local
        Else
            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=PatientData.ClientTimeZoneName)
            Dim messageButtons As MessageBoxButtons
            If PumpTimeZoneInfo Is Nothing Then
                Dim text As String
                If String.IsNullOrWhiteSpace(kvp.Value) Then
                    text = $"Your pump appears To be off-line, " &
                        "some values will be wrong do you want to continue? " &
                        $"If you select OK '{TimeZoneInfo.Local.Id}' will be " &
                        "used as you local time and you will not be prompted further. " &
                        "Cancel will Exit."
                    messageButtons = MessageBoxButtons.OKCancel
                Else
                    text = $"Your pump TimeZone '{kvp.Value}' is not recognized," &
                        " do you want to exit? " &
                        $"If you select No permanently use '{TimeZoneInfo.Local.Id}''? " &
                        $"If you select Yes '{TimeZoneInfo.Local.Id}' will be used " &
                        "and you will not be prompted further. " &
                        $"No will use '{TimeZoneInfo.Local.Id}' until you restart program. " &
                        "Cancel will exit program. Please open an issue and provide " &
                        $"the name '{kvp.Value}'. " &
                        "After selecting 'Yes' you can change the behavior " &
                        "under the Options Menu."
                    messageButtons = MessageBoxButtons.YesNoCancel
                End If
                Dim result As DialogResult = MessageBox.Show(
                    text,
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
    End Sub

    ''' <summary>
    '''  Gets the display name of a pump model based on its model number.
    ''' </summary>
    ''' <param name="value">The model number of the pump.</param>
    ''' <returns>
    '''  The display name of the pump if recognized;
    '''  otherwise, "Unknown".
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
    '''  Generates a possibly unique file name for data export,
    '''  based on the specified base name, culture, and extension.
    ''' </summary>
    ''' <param name="baseName">The first part of the file name.</param>
    ''' <param name="cultureName">
    '''  A valid culture name in the form of language-CountryCode.
    ''' </param>
    ''' <param name="extension">The extension for the file.</param>
    ''' <param name="mustBeUnique">
    '''  If <see langword="True"/>, ensures the file name is unique.
    ''' </param>
    ''' <returns>
    '''  A <see cref="FileNameStruct"/> containing the full and short file name,
    '''  or an empty struct on error.
    ''' </returns>
    ''' <example>
    '''  GetUniqueDataFileName("MyFile", "en-US", "txt", mustBeUnique:=True)
    ''' </example>
    Friend Function GetUniqueDataFileName(
        baseName As String,
        cultureName As String,
        extension As String,
        mustBeUnique As Boolean) As FileNameStruct
        Dim message As String
        If String.IsNullOrWhiteSpace(baseName) Then
            message = $"'{NameOf(baseName)}' cannot be null or whitespace."
            Throw New ArgumentException(message, paramName:=NameOf(baseName))
        End If

        If String.IsNullOrWhiteSpace(cultureName) Then
            message = $"'{NameOf(cultureName)}' cannot be null or whitespace."
            Throw New ArgumentException(message, paramName:=NameOf(cultureName))
        End If

        If String.IsNullOrWhiteSpace(extension) Then
            message = $"'{NameOf(extension)}' cannot be null or whitespace."
            Throw New ArgumentException(message, paramName:=NameOf(extension))
        End If

        Try
            Dim filenameWithoutExtension As String = $"{baseName}({cultureName}){s_userName}"
            Dim filenameWithExtension As String = $"{filenameWithoutExtension}.{extension}"
            Dim withPath As String =
                Path.Join(GetProjectDataDirectory(), filenameWithExtension)

            If mustBeUnique AndAlso File.Exists(path:=withPath) Then
                'Get unique file name
                Dim count As Long
                Do
                    count += 1
                    Dim filename As String = $"{filenameWithoutExtension}{count}.{extension}"
                    withPath = Path.Join(GetProjectDataDirectory(), filename)
                    filenameWithExtension = Path.GetFileName(path:=withPath)
                Loop While File.Exists(path:=withPath)
            End If

            Return New FileNameStruct(withPath, withoutPath:=filenameWithExtension)
        Catch ex As Exception
            Stop
        End Try
        Return New FileNameStruct

    End Function

    ''' <summary>
    '''  Handles complex items in a key-value row, splitting and processing values
    '''  for summary records.
    ''' </summary>
    ''' <param name="kvp">The key-value pair row to process.</param>
    ''' <param name="recordNumber">The index of the row.</param>
    ''' <param name="key">The key for the summary record.</param>
    ''' <param name="listOfSummaryRecords">
    '''  The list to which summary records are added.
    ''' </param>
    Friend Sub HandleComplexItems(
        kvp As KeyValuePair(Of String, String),
        recordNumber As Single,
        key As String,
        listOfSummaryRecords As List(Of SummaryRecord))

        Dim valueList As String() = GetValueList(kvp.Value)
        For Each e As IndexClass(Of String) In valueList.WithIndex
            Dim message As String = String.Empty
            Dim strings As String() = e.Value.Split(separator:=" = ")
            If kvp.Key.EqualsNoCase("AdditionalInfo") Then
                Dim additionalInfo As Dictionary(Of String, String) =
                    GetAdditionalInformation(jsonString:=kvp.Value)
                If strings(0).EqualsNoCase("sensorUpdateTime") Then
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
    '''  Checks if the <see cref="PatientData.MedicalDeviceInformation.ModelNumber"/>
    '''  is a 700 series model.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if the model number is a 700 series model;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function Is700Series() As Boolean
        If RecentDataEmpty() Then Return False
        Return s_700Models.Contains(PatientData.MedicalDeviceInformation.ModelNumber)
    End Function

    ''' <summary>
    '''  Checks if the <see cref="RecentData"/> is empty or not.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if <see cref="RecentData"/> is empty;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function RecentDataEmpty() As Boolean
        Return RecentData Is Nothing OrElse RecentData.Count = 0
    End Function

    ''' <summary>
    '''  Updates the data tables and summary records in the
    '''  <paramref name="mainForm"/> based on the most recent data.
    ''' </summary>
    ''' <param name="mainForm">The main form instance to update.</param>
    Friend Sub UpdateDataTables(mainForm As Form1)
        If RecentDataEmpty() Then
            DebugPrint(message:=$"Exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()

        Dim value As String = ""
        If RecentData.TryGetValue(key:="clientTimeZoneName", value) Then
            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=value)
        End If
        Dim bgUnitsNative As String = String.Empty
        Dim bgUnits As String = String.Empty
        If RecentData.TryGetValue("bgUnits", value:=bgUnitsNative) AndAlso
            UnitsStrings.TryGetValue(key:=bgUnitsNative, value:=bgUnits) Then
            NativeMmolL = bgUnits.Equals("mmol/L")
        Else
            Stop
        End If

        If RecentData.TryGetValue(key:="therapyAlgorithmState", value) Then
            s_therapyAlgorithmStateValue = LoadIndexedItems(json:=value)
            Dim key As String = NameOf(TherapyAlgorithmState.AutoModeShieldState)
            Dim basalTypes As IEnumerable(Of String) =
                {"AUTO_BASAL", "SAFE_BASAL"}
            InAutoMode = s_therapyAlgorithmStateValue.Count > 0 AndAlso
                basalTypes.Contains(value:=s_therapyAlgorithmStateValue(key))
        End If

        s_sgRecords = If(RecentData.TryGetValue(key:="sgs", value),
                         JsonToListOfSgs(json:=value),
                         New List(Of SG))

        mainForm.MaxBasalPerHourLabel.Text =
            If(RecentData.TryGetValue(key:="markers", value),
               CollectMarkers(),
               String.Empty)

        s_systemStatusTimeRemaining = Nothing
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In RecentData.WithIndex()

            Dim kvp As KeyValuePair(Of String, String) = c.Value
            If kvp.Value Is Nothing Then
                kvp = KeyValuePair.Create(kvp.Key, value:="")
            End If

            Dim key As ServerDataIndexes = CType(c.Index, ServerDataIndexes)
            Dim recordNumber As Single = c.Index
            Dim message As String
            Dim item As SummaryRecord
            Select Case kvp.Key
                Case NameOf(ServerDataIndexes.clientTimeZoneName)
                    item = New SummaryRecord(recordNumber, key, kvp.Value)
                    s_listOfSummaryRecords.Add(item)
                    SetupPumpTimeZoneInfo(mainForm, kvp)
                Case NameOf(ServerDataIndexes.lastName)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.firstName)
                    item = New SummaryRecord(
                        recordNumber,
                        key,
                        value:=PatientData.FirstName)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.appModelType)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.appModelNumber)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.currentServerTime)
                    message = kvp.Value.Epoch2DateTimeString
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.conduitSerialNumber)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.conduitBatteryLevel)
                    message = $"Phone battery is at {kvp.Value}%."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.conduitBatteryStatus)
                    message = $"Phone battery status is {kvp.Value.ToLower()}."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.lastConduitDateTime)
                    Dim provider As CultureInfo = CultureInfo.CurrentUICulture
                    kvp = New KeyValuePair(Of String, String)(
                        key:=NameOf(ServerDataIndexes.lastConduitDateTime),
                        value:=kvp.Value.CDateOrDefault(kvp.Key, provider))
                    message = $"Phone time is {kvp.Value}"
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.lastConduitUpdateServerDateTime)
                    message = kvp.Value.Epoch2DateTimeString
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.medicalDeviceFamily)
                    item = New SummaryRecord(recordNumber, kvp)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.medicalDeviceInformation)
                    HandleComplexItems(
                        kvp,
                        recordNumber,
                        key:="medicalDeviceInformation",
                        listOfSummaryRecords:=s_listOfSummaryRecords)
                    Dim deviceSerialNumber As String =
                        PatientData.MedicalDeviceInformation.DeviceSerialNumber
                    mainForm.SerialNumberButton.Text = $"{deviceSerialNumber} Details..."

                Case NameOf(ServerDataIndexes.medicalDeviceTime)
                    message = kvp.Value.Epoch2DateTimeString
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime)
                    message = kvp.Value.Epoch2DateTimeString
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.cgmInfo)
                    HandleComplexItems(
                        kvp,
                        recordNumber,
                        key:="cgmInfo",
                        listOfSummaryRecords:=s_listOfSummaryRecords)

                Case NameOf(ServerDataIndexes.calFreeSensor)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.calibStatus)
                    Dim messageTableName As String = NameOf(s_calibrationMessages)
                    item = New SummaryRecord(
                        recordNumber,
                        kvp,
                        messages:=s_calibrationMessages,
                        messageTableName)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.calibrationIconId)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.timeToNextEarlyCalibrationMinutes)
                    If kvp.Value = "15555" Then
                        message = "Calibration Free Sensor"
                        item = New SummaryRecord(recordNumber, kvp, message)
                        s_listOfSummaryRecords.Add(item)
                    Else
                        item = New SummaryRecord(recordNumber, kvp)
                        s_listOfSummaryRecords.Add(item)
                    End If

                Case NameOf(ServerDataIndexes.timeToNextCalibrationMinutes),
                     NameOf(ServerDataIndexes.timeToNextCalibrationRecommendedMinutes)
                    If kvp.Value = "-1" Then
                        message = "Calibration Free Sensor"
                        item = New SummaryRecord(recordNumber, kvp, message)
                        s_listOfSummaryRecords.Add(item)
                    Else
                        item = New SummaryRecord(recordNumber, kvp)
                        s_listOfSummaryRecords.Add(item)
                    End If

                Case NameOf(ServerDataIndexes.timeToNextCalibHours)
                    Dim timeToNextCalibrationHours As Byte = Byte.Parse(kvp.Value)
                    If timeToNextCalibrationHours = Byte.MaxValue Then
                        message = "Calibration Free Sensor"
                        item = New SummaryRecord(recordNumber, kvp, message)
                        s_listOfSummaryRecords.Add(item)
                    Else
                        message = $"{CInt(timeToNextCalibrationHours).ToHoursMinutes}"
                        item = New SummaryRecord(recordNumber, kvp, message)
                        s_listOfSummaryRecords.Add(item)
                    End If

                Case NameOf(ServerDataIndexes.finalCalibration)
                    If Boolean.Parse(kvp.Value) Then
                        s_timeToNextCalibrationMinutes = -1
                    End If
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.sensorDurationMinutes)
                    Dim sensorDurationMinutes As Integer = CInt(kvp.Value)
                    message = sensorDurationMinutes.MinutesToDaysHoursMinutes
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.sensorDurationHours)
                    Dim sensorDurationHours As Integer = CInt(kvp.Value)
                    message = sensorDurationHours.HoursToDaysAndHours
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.transmitterPairedTime)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.systemStatusTimeRemaining)
                    s_systemStatusTimeRemaining =
                        New TimeSpan(
                        hours:=0,
                        minutes:=PatientData.SystemStatusTimeRemaining,
                        seconds:=0)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.gstBatteryLevel)
                    message = $"Transmitter battery is at {kvp.Value}%."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.pumpBannerState)
                    s_pumpBannerStateValue = JsonToDictionaryList(kvp.Value)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)
                    mainForm.PumpBannerStateLabel.Visible = s_pumpBannerStateValue.Count > 0

                Case NameOf(ServerDataIndexes.therapyAlgorithmState)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.reservoirLevelPercent)
                    message = $"Reservoir is {PatientData.ReservoirLevelPercent}% full."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.reservoirAmount)
                    message = $"Full reservoir holds {PatientData.ReservoirAmount}U."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.pumpSuspended)
                    item = New SummaryRecord(recordNumber, kvp)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.pumpBatteryLevelPercent)
                    message = $"Pump battery is at {kvp.Value}%."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.reservoirRemainingUnits)
                    message =
                        $"Reservoir has {PatientData.ReservoirRemainingUnits}U remaining."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.conduitInRange)
                    message =
                        $"Phone {If(PatientData.ConduitInRange,
                                 "is",
                                 "is not")} in range of pump."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.conduitMedicalDeviceInRange)
                    message = $"Pump {If(CBool(kvp.Value), "is", "is not")} in range of phone"
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.conduitSensorInRange)
                    message =
                        $"Transmitter {If(PatientData.ConduitSensorInRange,
                                          "is",
                                          "is not")} in range of pump."
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.systemStatusMessage)
                    Dim messageTableName As String = NameOf(s_sensorMessages)
                    item = New SummaryRecord(
                        recordNumber,
                        kvp,
                        messages:=s_sensorMessages,
                        messageTableName)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.sensorState)
                    item = New SummaryRecord(
                        recordNumber,
                        kvp,
                        messages:=s_sensorMessages,
                        messageTableName:=NameOf(s_sensorMessages))
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.gstCommunicationState)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.pumpCommunicationState)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.timeFormat)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))
                Case NameOf(ServerDataIndexes.bgUnits)
                    item = New SummaryRecord(recordNumber, kvp, message:=bgUnits)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.maxAutoBasalRate)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.maxBolusAmount)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.sgBelowLimit)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.approvedForTreatment)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.lastAlarm)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)
                    s_lastAlarmValue = LoadIndexedItems(json:=kvp.Value)

                Case NameOf(ServerDataIndexes.activeInsulin)
                    s_activeInsulin = PatientData.ActiveInsulin
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.basal)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)
                    s_basalList(index:=0) =
                        If(String.IsNullOrWhiteSpace(kvp.Value), New Basal, PatientData.Basal)
                Case NameOf(ServerDataIndexes.lastSensorTime)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.lastSG)
                    s_lastSg = New SG(PatientData.LastSG)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.lastSGTrend)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.limits)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)
                    s_limitRecords = PatientData.Limits

                Case NameOf(ServerDataIndexes.belowHypoLimit)
                    message =
                        $"Time below limit = {PercentOf24HoursToString(kvp.Value)}"
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.aboveHyperLimit)
                    message =
                        $"Time above limit = {PercentOf24HoursToString(kvp.Value)}"
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.timeInRange)
                    message =
                        $"Time in range = {PercentOf24HoursToString(kvp.Value)}"
                    item = New SummaryRecord(recordNumber, kvp, message)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.averageSGFloat)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.averageSG)
                    s_listOfSummaryRecords.Add(item:=New SummaryRecord(recordNumber, kvp))

                Case NameOf(ServerDataIndexes.markers)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)

                Case NameOf(ServerDataIndexes.sgs)
                    item = New SummaryRecord(recordNumber, key, value:=ClickToShowDetails)
                    s_listOfSummaryRecords.Add(item)
                    s_lastSgValue = 0
                    If s_sgRecords.Count > 2 Then
                        s_lastSgValue = s_sgRecords.Item(index:=s_sgRecords.Count - 2).sg
                    End If

                Case NameOf(ServerDataIndexes.notificationHistory)
                    item = New SummaryRecord(
                        recordNumber:=CSng(c.Index + 0.1),
                        key:="activeNotification")
                    s_listOfSummaryRecords.Add(item)
                    item = New SummaryRecord(
                        recordNumber:=CSng(c.Index + 0.2),
                        key:="clearedNotifications")
                    s_listOfSummaryRecords.Add(item)
                    s_notificationHistoryValue = LoadIndexedItems(json:=kvp.Value)

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
    '''  Updates the marker tabs in the <paramref name="mainForm"/>
    '''  with the latest marker data.
    ''' </summary>
    ''' <param name="mainForm">The main form instance to update.</param>
    Friend Sub UpdateMarkerTabs(mainForm As Form1)
        With mainForm
            Dim classCollection As List(Of AutoBasalDelivery) = s_autoBasalDeliveryMarkers
            .TlpAutoBasalDelivery.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection),
                className:=NameOf(AutoBasalDelivery), rowIndex:=ServerDataIndexes.markers)

            .TlpAutoModeStatus.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_autoModeStatusMarkers),
                className:=NameOf(AutoModeStatus), rowIndex:=ServerDataIndexes.markers)

            .TlpBgReadings.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_bgReadingMarkers),
                className:=NameOf(BgReading), rowIndex:=ServerDataIndexes.markers)

            .TlpInsulin.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_insulinMarkers),
                className:=NameOf(Insulin), rowIndex:=ServerDataIndexes.markers)

            .TlpMeal.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_mealMarkers),
                className:=NameOf(Meal), rowIndex:=ServerDataIndexes.markers)

            .TlpCalibration.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_calibrationMarkers),
                className:=NameOf(Calibration), rowIndex:=ServerDataIndexes.markers)

            .TlpLowGlucoseSuspended.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_suspendedMarkers),
                className:=NameOf(LowGlucoseSuspended), rowIndex:=ServerDataIndexes.markers)

            .TlpTimeChange.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(classCollection:=s_timeChangeMarkers),
                className:=NameOf(TimeChange), rowIndex:=ServerDataIndexes.markers)

            DisplayDataTableInDGV(
                realPanel:=Nothing,
                table:=ClassCollectionToDataTable(classCollection:=s_basalPerHour),
                dgv:=mainForm.DgvBasalPerHour,
                rowIndex:=0)
            mainForm.DgvBasalPerHour.AutoSize = True
        End With

    End Sub

    ''' <summary>
    '''  Updates the pump banner state tab in the <paramref name="mainForm"/>
    '''  with the latest banner state data.
    ''' </summary>
    ''' <param name="mainForm">The main form instance to update.</param>
    Friend Sub UpdatePumpBannerStateTab(mainForm As Form1)
        Dim listOfBannerState As New List(Of BannerState)
        For Each dic As Dictionary(Of String, String) In s_pumpBannerStateValue
            Dim typeValue As String = ""
            If dic.TryGetValue(key:="type", value:=typeValue) Then
                Dim recordNumber As Integer = listOfBannerState.Count + 1
                Dim bannerStateRecord1 As BannerState =
                    DictionaryToClass(Of BannerState)(dic, recordNumber)
                listOfBannerState.Add(bannerStateRecord1)
                mainForm.PumpBannerStateLabel.Font =
                    New Font(FamilyName, emSize:=8.25F, style:=FontStyle.Bold)
                Select Case typeValue
                    Case "TEMP_TARGET"
                        Dim minutes As Integer = bannerStateRecord1.TimeRemaining
                        mainForm.PumpBannerStateLabel.BackColor = Color.Lime
                        mainForm.PumpBannerStateLabel.ForeColor =
                            mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        Dim target150 As String = If(NativeMmolL, "8.3", "150")
                        mainForm.PumpBannerStateLabel.Text =
                            $"Target {target150} {minutes.ToHoursMinutes}/hr"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "BG_REQUIRED"
                        mainForm.PumpBannerStateLabel.BackColor = Color.CadetBlue
                        mainForm.PumpBannerStateLabel.ForeColor =
                            mainForm.PumpBannerStateLabel.BackColor.ContrastingColor

                        mainForm.PumpBannerStateLabel.Text = "Enter BG Now"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "DELIVERY_SUSPEND"
                        mainForm.PumpBannerStateLabel.BackColor = Color.IndianRed
                        mainForm.PumpBannerStateLabel.ForeColor =
                            mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = "Delivery Suspended"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "LOAD_RESERVOIR"
                        mainForm.PumpBannerStateLabel.BackColor = Color.Yellow
                        mainForm.PumpBannerStateLabel.ForeColor =
                            mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = "Load Reservoir"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "PROCESSING_BG"
                        Stop
                    Case "SUSPENDED_BEFORE_LOW", "SUSPENDED_ON_LOW"
                        mainForm.PumpBannerStateLabel.BackColor = Color.IndianRed
                        mainForm.PumpBannerStateLabel.ForeColor =
                            mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        mainForm.PumpBannerStateLabel.Text = typeValue.ToTitle()
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        mainForm.PumpBannerStateLabel.Font =
                            New Font(FamilyName, emSize:=7.0F, style:=FontStyle.Bold)
                    Case "TEMP_BASAL"
                        mainForm.PumpBannerStateLabel.BackColor = Color.Lime
                        mainForm.PumpBannerStateLabel.ForeColor =
                            mainForm.PumpBannerStateLabel.BackColor.ContrastingColor
                        Dim bannerState As BannerState =
                            PatientData.PumpBannerState(index:=0)
                        Dim hours As String =
                            bannerState.TimeRemaining.ToHoursMinutes
                        mainForm.PumpBannerStateLabel.Text = $"Temp Basal {hours} hr"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        mainForm.PumpBannerStateLabel.Font =
                            New Font(FamilyName, emSize:=7.0F, style:=FontStyle.Bold)
                    Case "WAIT_TO_ENTER_BG"
                        Stop
                    Case Else
                        Dim stackFrame As StackFrame
                        If Debugger.IsAttached Then
                            stackFrame = New StackFrame(skipFrames:=0, needFileInfo:=True)
                            MsgBox(
                                heading:=$"{typeValue} Is unknown banner message!",
                                prompt:="",
                                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                                title:=GetTitleFromStack(stackFrame))
                        End If
                End Select
                mainForm.PumpBannerStateLabel.ForeColor =
                    ContrastingColor(baseColor:=mainForm.PumpBannerStateLabel.BackColor)
            Else
                Stop
            End If
        Next

        Dim safeBasalDurationStr As String = ""
        Dim key As String = NameOf(TherapyAlgorithmState.SafeBasalDuration)
        If s_therapyAlgorithmStateValue?.TryGetValue(key, value:=safeBasalDurationStr) Then
            Dim safeBasalDuration As Integer = CInt(safeBasalDurationStr)
            If safeBasalDuration > 0 Then
                mainForm.LastSgOrExitTimeLabel.Text =
                    $"Exit In { TimeSpan.FromMinutes(safeBasalDuration) _
                                        .ToFormattedTimeSpan(unit:="hr")}"
                mainForm.LastSgOrExitTimeLabel.Visible = True
            End If
        End If
        mainForm.TlpPumpBannerState.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:=listOfBannerState),
            className:=NameOf(BannerState), rowIndex:=ServerDataIndexes.pumpBannerState)
    End Sub

End Module
