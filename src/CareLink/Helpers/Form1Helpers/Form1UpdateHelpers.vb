' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Friend Structure FileNameStruct
    Public withPath As String
    Public withoutPath As String

    Public Sub New(withPath As String, withoutPath As String)
        Me.withPath = withPath
        Me.withoutPath = withoutPath
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf obj Is FileNameStruct) Then
            Return False
        End If

        Dim other As FileNameStruct = DirectCast(obj, FileNameStruct)
        Return withPath = other.withPath AndAlso
               withoutPath = other.withoutPath
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(withPath, withoutPath)
    End Function

End Structure

Friend Module Form1UpdateHelpers

    Private ReadOnly s_700Models As New List(Of String) From {
        "MMT-1812",
        "MMT-1880"}

    <Extension>
    Private Function CDateOrDefault(dateAsString As String, key As String, provider As IFormatProvider) As String
        Dim resultDate As Date
        Return If(TryParseDate(dateAsString, resultDate, key),
                  resultDate.ToString(provider),
                  ""
                 )
    End Function

    Private Function ConvertPercent24HoursToDisplayValueString(rowValue As String) As String
        Dim val As Decimal = CDec(Convert.ToInt32(rowValue) * 0.24)
        Dim hours As Integer = Convert.ToInt32(val)
        Dim minutes As Integer = CInt((val Mod 1) * 60)
        Return If(minutes = 0,
                  $"{hours} hours, out of last 24 hours.",
                  $"{hours} hours and {minutes} minutes, out of last 24 hours."
                 )
    End Function

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

    Friend Sub HandleComplexItems(row As KeyValuePair(Of String, String), rowIndex As Integer, key As String, listOfSummaryRecords As List(Of SummaryRecord))
        Dim valueList As String() = GetValueList(row.Value)
        For Each e As IndexClass(Of String) In valueList.WithIndex
            Dim message As String = String.Empty
            Dim strings As String() = e.Value.Split(" = ")
            If row.Key = "additionalInfo" Then
                Dim additionalInfo As Dictionary(Of String, String) = GetAdditionalInformation(row.Value)
                If strings(0) = "sensorUpdateTime" Then
                    message = GetSensorUpdateTime(strings(1))
                End If
            End If
            Dim item As New SummaryRecord(
                recordNumber:=CSng(CSng(rowIndex) + ((e.Index + 1) / 10)),
                $"{key}:{strings(0).Trim}",
                value:=strings(1).Trim,
                message)
            listOfSummaryRecords.Add(item)
        Next
    End Sub

    Public Function Is700Series() As Boolean
        If RecentDataEmpty() Then Return False
        Return s_700Models.Contains(PatientData.MedicalDeviceInformation.ModelNumber)
    End Function

    Friend Function RecentDataEmpty() As Boolean
        Return RecentData Is Nothing OrElse RecentData.Count = 0
    End Function

    Friend Sub UpdateDataTables(mainForm As Form1)

        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()
        s_listOfUserSummaryRecord.Clear()

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

            Dim row As KeyValuePair(Of String, String) = c.Value
            If row.Value Is Nothing Then
                row = KeyValuePair.Create(row.Key, "")
            End If

            Dim recordNumber As ServerDataIndexes = CType(c.Index, ServerDataIndexes)
            Select Case row.Key
                Case NameOf(ServerDataIndexes.clientTimeZoneName)
                    If s_useLocalTimeZone Then
                        PumpTimeZoneInfo = TimeZoneInfo.Local
                    Else
                        PumpTimeZoneInfo = CalculateTimeZone(PatientData.ClientTimeZoneName)
                        Dim message As String
                        Dim messageButtons As MessageBoxButtons
                        If PumpTimeZoneInfo Is Nothing Then
                            If String.IsNullOrWhiteSpace(row.Value.ToString) Then
                                message = $"Your pump appears To be off-line, some values will be wrong do you want to continue? If you select OK '{TimeZoneInfo.Local.Id}' will be used as you local time and you will not be prompted further. Cancel will Exit."
                                messageButtons = MessageBoxButtons.OKCancel
                            Else
                                message = $"Your pump TimeZone '{row.Value}' is not recognized, do you want to exit? If you select No permanently use '{TimeZoneInfo.Local.Id}''? If you select Yes '{TimeZoneInfo.Local.Id}' will be used and you will not be prompted further. No will use '{TimeZoneInfo.Local.Id}' until you restart program. Cancel will exit program. Please open an issue and provide the name '{row.Value}'. After selecting 'Yes' you can change the behavior under the Options Menu."
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
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, row.Value))

                Case NameOf(ServerDataIndexes.lastName)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.firstName)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, PatientData.FirstName))

                Case NameOf(ServerDataIndexes.appModelType)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.appModelNumber)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.currentServerTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, row.Value.Epoch2DateTimeString))

                Case NameOf(ServerDataIndexes.conduitSerialNumber)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.conduitBatteryLevel)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Phone battery is at {row.Value}%."))

                Case NameOf(ServerDataIndexes.conduitBatteryStatus)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Phone battery status is {row.Value.ToLower}."))

                Case NameOf(ServerDataIndexes.lastConduitDateTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, New KeyValuePair(Of String, String)(NameOf(ServerDataIndexes.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ServerDataIndexes.lastConduitDateTime), Provider))))

                Case NameOf(ServerDataIndexes.lastConduitUpdateServerDateTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, row.Value.Epoch2DateTimeString))

                Case NameOf(ServerDataIndexes.medicalDeviceFamily)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.medicalDeviceInformation)
                    HandleComplexItems(row, recordNumber, "medicalDeviceInformation", s_listOfSummaryRecords)
                    mainForm.SerialNumberButton.Text = $"{ PatientData.MedicalDeviceInformation.DeviceSerialNumber} Details..."

                Case NameOf(ServerDataIndexes.medicalDeviceTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, row.Value.Epoch2DateTimeString))

                Case NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, row.Value.Epoch2DateTimeString))

                Case NameOf(ServerDataIndexes.cgmInfo)
                    HandleComplexItems(row, recordNumber, "cgmInfo", s_listOfSummaryRecords)

                Case NameOf(ServerDataIndexes.calFreeSensor)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.calibStatus)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, s_calibrationMessages, NameOf(s_calibrationMessages)))

                Case NameOf(ServerDataIndexes.calibrationIconId)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.timeToNextEarlyCalibrationMinutes)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.timeToNextCalibrationMinutes)
                    s_timeToNextCalibrationMinutes = Short.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(ServerDataIndexes.timeToNextCalibrationMinutes, row, "No data from pump"))

                Case NameOf(ServerDataIndexes.timeToNextCalibrationRecommendedMinutes)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.timeToNextCalibHours)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.finalCalibration)
                    If Boolean.Parse(row.Value) Then
                        s_timeToNextCalibrationMinutes = -1
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.sensorDurationMinutes)
                    s_listOfSummaryRecords.Add(New SummaryRecord(ServerDataIndexes.sensorDurationMinutes, "-1", "No data from pump"))

                Case NameOf(ServerDataIndexes.sensorDurationHours)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.transmitterPairedTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.systemStatusTimeRemaining)
                    s_systemStatusTimeRemaining = New TimeSpan(0, PatientData.SystemStatusTimeRemaining, 0)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.gstBatteryLevel)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.pumpBannerState)
                    s_pumpBannerStateValue = JsonToLisOfDictionary(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))
                    mainForm.PumpBannerStateLabel.Visible = s_pumpBannerStateValue.Count > 0

                Case NameOf(ServerDataIndexes.therapyAlgorithmState)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))

                Case NameOf(ServerDataIndexes.reservoirLevelPercent)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Reservoir is {PatientData.ReservoirLevelPercent}%"))

                Case NameOf(ServerDataIndexes.reservoirAmount)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Full reservoir holds {PatientData.ReservoirAmount}U"))

                Case NameOf(ServerDataIndexes.pumpSuspended)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.pumpBatteryLevelPercent)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.reservoirRemainingUnits)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Reservoir has {PatientData.ReservoirRemainingUnits}U remaining"))

                Case NameOf(ServerDataIndexes.conduitInRange)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Phone {If(PatientData.ConduitInRange, "is", "is not")} in range of pump"))

                Case NameOf(ServerDataIndexes.conduitMedicalDeviceInRange)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Pump {If(CBool(row.Value), "is", "is not")} in range of phone"))

                Case NameOf(ServerDataIndexes.conduitSensorInRange)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Transmitter {If(PatientData.ConduitSensorInRange, "is", "is not")} in range of pump"))

                Case NameOf(ServerDataIndexes.systemStatusMessage)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case NameOf(ServerDataIndexes.sensorState)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case NameOf(ServerDataIndexes.gstCommunicationState)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.pumpCommunicationState)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.timeFormat)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))
                Case NameOf(ServerDataIndexes.bgUnits)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, BgUnitsNativeString))

                Case NameOf(ServerDataIndexes.maxAutoBasalRate)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.maxBolusAmount)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.sgBelowLimit)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.approvedForTreatment)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.lastAlarm)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))
                    s_lastAlarmValue = LoadIndexedItems(row.Value)

                Case NameOf(ServerDataIndexes.activeInsulin)
                    s_activeInsulin = PatientData.ActiveInsulin
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))

                Case NameOf(ServerDataIndexes.basal)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))
                    s_basalList(0) = If(String.IsNullOrWhiteSpace(row.Value), New Basal, PatientData.Basal)
                Case NameOf(ServerDataIndexes.lastSensorTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.lastSG)
                    s_lastSg = New SG(PatientData.LastSG)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))

                Case NameOf(ServerDataIndexes.lastSGTrend)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.limits)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))
                    s_listOfLimitRecords = PatientData.Limits

                Case NameOf(ServerDataIndexes.belowHypoLimit)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Time below limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case NameOf(ServerDataIndexes.aboveHyperLimit)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Time above limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case NameOf(ServerDataIndexes.timeInRange)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Time in range = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case NameOf(ServerDataIndexes.averageSGFloat)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.averageSG)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.markers)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))

                Case NameOf(ServerDataIndexes.sgs)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, recordNumber, ClickToShowDetails))
                    s_lastSgValue = 0
                    If s_listOfSgRecords.Count > 2 Then
                        s_lastSgValue = s_listOfSgRecords.Item(s_listOfSgRecords.Count - 2).sg
                    End If

                Case NameOf(ServerDataIndexes.notificationHistory)
                    s_listOfSummaryRecords.Add(New SummaryRecord(CSng(c.Index + 0.1), "activeNotification"))
                    s_listOfSummaryRecords.Add(New SummaryRecord(CSng(c.Index + 0.2), "clearedNotifications"))
                    s_notificationHistoryValue = LoadIndexedItems(row.Value)

                Case NameOf(ServerDataIndexes.sensorLifeText)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.sensorLifeIcon)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case Else
                    Stop
            End Select
        Next

    End Sub

    Friend Sub UpdateMarkerTabs(mainForm As Form1)
        With mainForm
            .TableLayoutPanelAutoBasalDelivery.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfAutoBasalDeliveryMarkers),
                className:=NameOf(AutoBasalDelivery), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelAutoModeStatus.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfAutoModeStatusMarkers),
                className:=NameOf(AutoModeStatus), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelBgReadings.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfBgReadingMarkers),
                className:=NameOf(BgReading), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelInsulin.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfInsulinMarkers),
                className:=NameOf(Insulin), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelMeal.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfMealMarkers),
                className:=NameOf(Meal), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelCalibration.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfCalibrationMarkers),
                className:=NameOf(Calibration), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelLowGlucoseSuspended.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfLowGlucoseSuspendedMarkers),
                className:=NameOf(LowGlucoseSuspended), rowIndex:=ServerDataIndexes.markers)

            .TableLayoutPanelTimeChange.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfTimeChangeMarkers),
                className:=NameOf(TimeChange), rowIndex:=ServerDataIndexes.markers)

            DisplayDataTableInDGV(
                realPanel:=Nothing,
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfBasalPerHour),
                dGV:=mainForm.DgvBasalPerHour,
                rowIndex:=0)
            mainForm.DgvBasalPerHour.AutoSize = True
        End With

    End Sub

    Friend Sub UpdatePumpBannerStateTab(mainForm As Form1)
        Dim listOfBannerState As New List(Of BannerState)
        For Each dic As Dictionary(Of String, String) In s_pumpBannerStateValue
            Dim typeValue As String = ""
            If dic.TryGetValue(key:="type", value:=typeValue) Then
                Dim bannerStateRecord1 As BannerState = DictionaryToClass(Of BannerState)(dic, listOfBannerState.Count + 1)
                listOfBannerState.Add(bannerStateRecord1)
                mainForm.PumpBannerStateLabel.Font = New Font(familyName:="Segoe UI", emSize:=8.25F, style:=FontStyle.Bold, unit:=GraphicsUnit.Point)
                Select Case typeValue
                    Case "TEMP_TARGET"
                        Dim minutes As Integer = bannerStateRecord1.TimeRemaining
                        mainForm.PumpBannerStateLabel.BackColor = Color.Lime
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.GetContrastingColor
                        mainForm.PumpBannerStateLabel.Text = $"Target {If(NativeMmolL, "8.3", "150")}  {minutes.ToHours} hr"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "BG_REQUIRED"
                        mainForm.PumpBannerStateLabel.BackColor = Color.CadetBlue
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.GetContrastingColor

                        mainForm.PumpBannerStateLabel.Text = "Enter BG Now"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "DELIVERY_SUSPEND"
                        mainForm.PumpBannerStateLabel.BackColor = Color.IndianRed
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.GetContrastingColor
                        mainForm.PumpBannerStateLabel.Text = "Delivery Suspended"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "LOAD_RESERVOIR"
                        mainForm.PumpBannerStateLabel.BackColor = Color.Yellow
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.GetContrastingColor
                        mainForm.PumpBannerStateLabel.Text = "Load Reservoir"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "PROCESSING_BG"
                        Stop
                    Case "SUSPENDED_BEFORE_LOW", "SUSPENDED_ON_LOW"
                        mainForm.PumpBannerStateLabel.BackColor = Color.IndianRed
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.GetContrastingColor
                        mainForm.PumpBannerStateLabel.Text = typeValue.ToTitle()
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        mainForm.PumpBannerStateLabel.Font = New Font(
                        familyName:="Segoe UI",
                        emSize:=7.0F,
                        style:=FontStyle.Bold,
                        unit:=GraphicsUnit.Point)
                    Case "TEMP_BASAL"
                        mainForm.PumpBannerStateLabel.BackColor = Color.Lime
                        mainForm.PumpBannerStateLabel.ForeColor = mainForm.PumpBannerStateLabel.BackColor.GetContrastingColor
                        mainForm.PumpBannerStateLabel.Text = $"Temp Basal {PatientData.PumpBannerState(0).TimeRemaining.ToHours} hr"
                        mainForm.PumpBannerStateLabel.Visible = True
                        mainForm.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        mainForm.PumpBannerStateLabel.Font = New Font(
                        familyName:="Segoe UI",
                        emSize:=7.0F,
                        style:=FontStyle.Bold,
                        unit:=GraphicsUnit.Point)
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
                mainForm.PumpBannerStateLabel.ForeColor = GetContrastingColor(baseColor:=mainForm.PumpBannerStateLabel.BackColor)
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
        mainForm.TableLayoutPanelBannerState.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(listOfClass:=listOfBannerState),
            className:=NameOf(BannerState), rowIndex:=ServerDataIndexes.pumpBannerState)
    End Sub

    ''' <summary>
    '''  Possibility unique file name of the form baseName(<paramref name="cultureName"/>)<see cref="s_userName"/>.<paramref name="extension"/>
    '''  given an <paramref name="baseName"/> and culture as a seed.
    '''  If <paramref name="MustBeUnique"/> is true, a unique file name is created by appending a number to the file name.
    '''  The file name is created in the <see cref="DirectoryForProjectData"/>
    '''  If <paramref name="MustBeUnique"/> is false, the file name may not be unique.
    ''' </summary>
    ''' <param Name="baseName">The first part of the file name</param>
    ''' <param Name="cultureName">A valid Culture Name in the form of language-CountryCode</param>
    ''' <param Name="extension">The extension for the file</param>
    ''' <param name="MustBeUnique">True if the file name must be unique</param>
    ''' <returns>
    '''  A unique file name valid in <see cref="DirectoryForProjectData"/> folder or an empty file name on error.
    ''' </returns>
    ''' <example>
    '''  GetUniqueDataFileName("MyFile", "en-US", "txt", True)
    ''' </example>
    Public Function GetUniqueDataFileName(baseName As String, cultureName As String, extension As String, MustBeUnique As Boolean) As FileNameStruct
        If String.IsNullOrWhiteSpace(baseName) Then
            Throw New ArgumentException($"'{NameOf(baseName)}' cannot be null or whitespace.", NameOf(baseName))
        End If

        If String.IsNullOrWhiteSpace(cultureName) Then
            Throw New ArgumentException($"'{NameOf(cultureName)}' cannot be null or whitespace.", NameOf(cultureName))
        End If

        If String.IsNullOrWhiteSpace(extension) Then
            Throw New ArgumentException($"'{NameOf(extension)}' cannot be null or whitespace.", NameOf(extension))
        End If

        Try
            Dim filenameWithoutExtension As String = $"{baseName}({cultureName}){s_userName}"
            Dim filenameWithExtension As String = $"{filenameWithoutExtension}.{extension}"
            Dim filenameFullPath As String = Path.Join(DirectoryForProjectData, filenameWithExtension)

            If MustBeUnique AndAlso File.Exists(filenameFullPath) Then
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

End Module
