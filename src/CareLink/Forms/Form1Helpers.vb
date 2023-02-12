' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Friend Module Form1Helpers

    Private Function ConvertPercent24HoursToDisplayValueString(rowValue As String) As String
        Dim val As Decimal = CDec(Convert.ToInt32(rowValue) * 0.24)
        Dim hours As Integer = Convert.ToInt32(val)
        Dim minutes As Integer = CInt((val Mod 1) * 60)
        If minutes = 0 Then
            Return $"{hours} hours, out of last 24 hours."
        Else
            Return $"{hours} hours and {minutes} minutes, out of last 24 hours."
        End If
    End Function

    <Extension>
    Friend Function DoOptionalLoginAndUpdateData(MainForm As Form1, UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        MainForm.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        s_listOfAutoBasalDeliveryMarkers.Clear()
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                MainForm.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(SavedLastDownloadName)
                MainForm.RecentData = Loads(File.ReadAllText(LastDownloadWithPath))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(LastDownloadWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.TestData
                MainForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim testDataWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
                MainForm.RecentData = Loads(File.ReadAllText(testDataWithPath))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(testDataWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.Login
                MainForm.Text = SavedTitle
                Do Until MainForm.LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop

                If MainForm.Client Is Nothing OrElse Not MainForm.Client.LoggedIn Then
                    MainForm.ServerUpdateTimer.Interval = s_fiveMinutesInMilliseconds
                    MainForm.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If NetworkDown Then
                        ReportLoginStatus(MainForm.LoginStatus)
                        Return False
                    End If

                    MainForm.LastUpdateTime.Text = "Unknown"
                    Return False
                End If
                s_listOfManualBasal.Clear()
                MainForm.RecentData = MainForm.Client.GetRecentData(MainForm)
                MainForm.ServerUpdateTimer.Interval = s_oneMinutesInMilliseconds
                MainForm.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkDown Then
                    ReportLoginStatus(MainForm.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(MainForm.LoginStatus, MainForm.RecentData Is Nothing OrElse MainForm.RecentData.Count = 0, MainForm.Client.GetLastErrorMessage)

                MainForm.MenuShowMiniDisplay.Visible = True
        End Select
        MainForm.FinishInitialization()
        If UpdateAllTabs Then
            MainForm.UpdateAllTabPages()
        End If
        Return True
    End Function

    <Extension>
    Friend Sub FinishInitialization(MainForm As Form1)
        MainForm.Cursor = Cursors.Default
        Application.DoEvents()

        MainForm.InitializeSummaryTabCharts()
        MainForm.InitializeActiveInsulinTabChart()
        MainForm.InitializeTimeInRangeArea()

        MainForm.Initialized = True
    End Sub

    Friend Sub SetTreatmentInsulinRow()
        Select Case MaxBasalPerDose
            Case < 0.25
                TreatmentInsulinRow = 0.4
            Case < 0.5
                TreatmentInsulinRow = 0.5
            Case < 0.75
                TreatmentInsulinRow = 0.75
            Case < 1
                TreatmentInsulinRow = 1
            Case < 1.25
                TreatmentInsulinRow = 1.25
            Case < 1.5
                TreatmentInsulinRow = 1.5
            Case < 1.75
                TreatmentInsulinRow = 1.75
            Case < 2
                TreatmentInsulinRow = 2
            Case Else
                TreatmentInsulinRow = CSng(MaxBasalPerDose + 0.025)
        End Select
        TreatmentInsulinRow = TreatmentInsulinRow.RoundSingle(3)
    End Sub

    Friend Sub UpdateDataTables(mainForm As Form1, recentData As Dictionary(Of String, String))

        If recentData Is Nothing Then
            Debug.Print($"Exiting {NameOf(UpdateDataTables)}, {NameOf(recentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()

        s_lastMedicalDeviceDataUpdateServerEpoch = CLng(recentData(ItemIndexes.lastMedicalDeviceDataUpdateServerTime.ToString))
        Dim markerRowString As String = ""
        If recentData.TryGetValue(ItemIndexes.therapyAlgorithmState.ToString, markerRowString) Then
            s_therapyAlgorithmStateValue = Loads(markerRowString)
            InAutoMode = s_therapyAlgorithmStateValue.Count > 0 AndAlso s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmStateRecord.autoModeShieldState)) = "AUTO_BASAL"
        End If

#Region "Update all Markers"

        If recentData.TryGetValue(ItemIndexes.sgs.ToString, markerRowString) Then
            s_listOfSGs = LoadList(markerRowString).ToSgList()
        End If

        If recentData.TryGetValue(ItemIndexes.basal.ToString, markerRowString) Then
            Dim item As BasalRecord = DictionaryToClass(Of BasalRecord)(Loads(markerRowString), recordNumber:=0)
            item.OaDateTime(s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2DateTime)
            s_listOfManualBasal.Add(item)
        End If
        If recentData.TryGetValue(ItemIndexes.markers.ToString, markerRowString) Then
            mainForm.MaxBasalPerHourLabel.Text = CollectMarkers(markerRowString)
        Else
            mainForm.MaxBasalPerHourLabel.Text = ""
        End If

#End Region ' Update all Markers

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In recentData.WithIndex()

            Dim row As KeyValuePair(Of String, String) = c.Value
            If row.Value Is Nothing Then
                row = KeyValuePair.Create(row.Key, "")
            End If
            Dim rowIndex As ItemIndexes = GetItemIndex(row.Key)
            Dim summaryItem As SummaryRecord
            Select Case rowIndex
                Case ItemIndexes.lastSensorTS,
                     ItemIndexes.medicalDeviceTimeAsString,
                     ItemIndexes.lastSensorTSAsString,
                     ItemIndexes.kind,
                     ItemIndexes.version,
                     ItemIndexes.pumpModelNumber
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.currentServerTime,
                     ItemIndexes.lastConduitTime,
                     ItemIndexes.lastConduitUpdateServerTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexes.lastMedicalDeviceDataUpdateServerTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexes.firstName
                    summaryItem = New SummaryRecord(rowIndex, row.Value)
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_firstName = summaryItem.Value

                Case ItemIndexes.lastName
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.conduitSerialNumber
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.conduitBatteryLevel
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery is at {row.Value}%."))

                Case ItemIndexes.conduitBatteryStatus
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery status is {row.Value}"))

                Case ItemIndexes.conduitInRange
                    summaryItem = New SummaryRecord(rowIndex, row, $"Phone {If(s_pumpInRangeOfPhone, "is", "is not")} in range of pump")
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_pumpInRangeOfPhone = CBool(summaryItem.Value)

                Case ItemIndexes.conduitMedicalDeviceInRange
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump {If(CBool(row.Value), "is", "is not")} in range of phone"))

                Case ItemIndexes.conduitSensorInRange
                    summaryItem = New SummaryRecord(rowIndex, row, $"Transmitter {If(s_pumpInRangeOfTransmitter, "is", "is not")} in range of pump")
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_pumpInRangeOfTransmitter = CBool(summaryItem.Value)

                Case ItemIndexes.medicalDeviceFamily
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.sensorState
                    summaryItem = New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages))
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_sensorState = summaryItem.Value

                Case ItemIndexes.medicalDeviceSerialNumber
                    summaryItem = New SummaryRecord(rowIndex, row, $"Pump serial number is {row.Value}.")
                    s_listOfSummaryRecords.Add(summaryItem)
                    mainForm.SerialNumberLabel.Text = summaryItem.Value

                Case ItemIndexes.medicalDeviceTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.sMedicalDeviceTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.reservoirLevelPercent
                    summaryItem = New SummaryRecord(rowIndex, row, $"Reservoir is {row.Value}%")
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_reservoirLevelPercent = CInt(summaryItem.Value)

                Case ItemIndexes.reservoirAmount
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Full reservoir holds {row.Value}U"))

                Case ItemIndexes.reservoirRemainingUnits
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir has {row.Value}U remaining"))

                Case ItemIndexes.medicalDeviceBatteryLevelPercent
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump battery is at {row.Value}%"))

                Case ItemIndexes.sensorDurationHours
                    summaryItem = New SummaryRecord(rowIndex, row)
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_sensorDurationHours = CInt(summaryItem.Value)

                Case ItemIndexes.timeToNextCalibHours
                    summaryItem = New SummaryRecord(rowIndex, row)
                    s_listOfSummaryRecords.Add(summaryItem)
                    s_timeToNextCalibrationHours = CUShort(summaryItem.Value)

                Case ItemIndexes.calibStatus
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_calibrationMessages, NameOf(s_calibrationMessages)))

                Case ItemIndexes.bgUnits,
                     ItemIndexes.timeFormat,
                     ItemIndexes.lastSensorTime,
                     ItemIndexes.sLastSensorTime,
                     ItemIndexes.medicalDeviceSuspended,
                     ItemIndexes.lastSGTrend
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.lastSG
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_lastSgRecord = New SgRecord(Loads(row.Value), 0)

                Case ItemIndexes.lastAlarm
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_lastAlarmValue = Loads(row.Value)

                Case ItemIndexes.activeInsulin
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_activeInsulin = DictionaryToClass(Of ActiveInsulinRecord)(Loads(row.Value), 0)

                Case ItemIndexes.sgs
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    If s_listOfSGs.Count > 2 Then
                        s_lastBGValue = s_listOfSGs.Item(s_listOfSGs.Count - 2).sg
                    End If

                Case ItemIndexes.limits
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    UpdateListOfLimitRecords(row)

                Case ItemIndexes.markers
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ItemIndexes.notificationHistory
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_notificationHistoryValue = Loads(row.Value)

                Case ItemIndexes.therapyAlgorithmState
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ItemIndexes.pumpBannerState
                    s_pumpBannerStateValue = LoadList(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    mainForm.TempTargetLabel.Visible = False

                Case ItemIndexes.basal
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ItemIndexes.systemStatusMessage
                    s_systemStatusMessage = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case ItemIndexes.averageSG
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.belowHypoLimit
                    s_belowHypoLimit = row.Value.ParseSingle(1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time below limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ItemIndexes.aboveHyperLimit
                    s_aboveHyperLimit = row.Value.ParseSingle(1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time above limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ItemIndexes.timeInRange
                    s_timeInRange = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time in range = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ItemIndexes.pumpCommunicationState
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.gstCommunicationState
                    s_gstCommunicationState = Boolean.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                    Dim gstBatteryLevel As String = Nothing
                    If recentData.TryGetValue(NameOf(ItemIndexes.gstBatteryLevel), gstBatteryLevel) Then
                        Continue For
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexes.gstBatteryLevel, "-1", "No data from pump"))

                Case ItemIndexes.gstBatteryLevel
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"{row.Value}%"))

                Case ItemIndexes.lastConduitDateTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ItemIndexes.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ItemIndexes.lastConduitDateTime), CurrentUICulture))))

                Case ItemIndexes.maxAutoBasalRate
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.maxBolusAmount
                    s_listOfSummaryRecords.Add(New SummaryRecord(GetItemIndex(c.Value.Key), row))
                    Dim tempStr As String = Nothing
                    If Not recentData.TryGetValue(NameOf(ItemIndexes.sensorDurationMinutes), tempStr) Then
                        s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexes.sensorDurationMinutes, "-1", "No data from pump"))
                    End If
                    If Not recentData.TryGetValue(NameOf(ItemIndexes.timeToNextCalibrationMinutes), tempStr) Then
                        s_timeToNextCalibrationMinutes = UShort.MaxValue
                        s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexes.timeToNextCalibrationMinutes, s_timeToNextCalibrationMinutes.ToString, "No data from pump"))
                    End If

                Case ItemIndexes.sensorDurationMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.timeToNextCalibrationMinutes
                    s_timeToNextCalibrationMinutes = CUShort(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.clientTimeZoneName
                    s_clientTimeZoneName = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, s_clientTimeZoneName))

                Case ItemIndexes.sgBelowLimit,
                        ItemIndexes.averageSGFloat
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.timeToNextCalibrationRecommendedMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.calFreeSensor,
                         ItemIndexes.finalCalibration
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
            End Select
        Next

    End Sub

End Module
