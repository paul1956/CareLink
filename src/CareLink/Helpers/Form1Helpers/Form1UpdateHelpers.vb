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

    Friend Sub HandleComplexItems(row As KeyValuePair(Of String, String), rowIndex As ServerDataIndexes, key As String, listOfSummaryRecords As List(Of SummaryRecord))
        Dim valueList As String() = GetValueList(row.Value)
        For Each e As IndexClass(Of String) In valueList.WithIndex
            Dim item As New SummaryRecord(
                recordNumber:=CSng(CSng(rowIndex) + ((e.Index + 1) / 10)),
                key,
                value:=e.Value.Split(" = ")(0).Trim,
                message:=e.Value.Split(" = ")(1).Trim)
            listOfSummaryRecords.Add(item)
            If item.Value = "hardwareRevision" Then
                s_pumpHardwareRevision = item.Message
            End If

        Next
    End Sub


    Friend Function RecentDataEmpty() As Boolean
        Return RecentData Is Nothing OrElse RecentData.Count = 0
    End Function

    Friend Sub UpdateDataTables()

        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()
        s_listOfUserSummaryRecord.Clear()

        Dim markerRowString As String = ""
        If RecentData.TryGetValue("clientTimeZoneName", markerRowString) Then
            PumpTimeZoneInfo = CalculateTimeZone(markerRowString)
        End If

        If Not RecentData.TryGetValue("bgUnits", BgUnitsNativeString) Then
            Stop
        End If

        s_lastMedicalDeviceDataUpdateServerEpoch = CLng(RecentData("lastMedicalDeviceDataUpdateServerTime"))

        If RecentData.TryGetValue("therapyAlgorithmState", markerRowString) Then
            s_therapyAlgorithmStateValue = LoadIndexedItems(markerRowString)
            InAutoMode = s_therapyAlgorithmStateValue.Count > 0 AndAlso {"AUTO_BASAL", "SAFE_BASAL"}.Contains(s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmState.AutoModeShieldState)))
        End If

        If RecentData.TryGetValue("sgs", markerRowString) Then
            s_listOfSgRecords = JsonToLisOfSgs(markerRowString)
        End If

        If RecentData.TryGetValue("basal", markerRowString) Then
            Dim item As Basal = DictionaryToClass(Of Basal)(LoadIndexedItems(markerRowString), recordNumber:=0)
            item.OaDateTime(s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime)
            s_listOfManualBasal.Add(item)
        End If
        My.Forms.Form1.MaxBasalPerHourLabel.Text = If(RecentData.TryGetValue("markers", markerRowString),
                                             CollectMarkers(),
                                             ""
                                            )

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
                            Dim result As DialogResult = MessageBox.Show(message, "TimeZone Unknown",
                                                                                 messageButtons,
                                                                                 MessageBoxIcon.Question)
                            s_useLocalTimeZone = True
                            PumpTimeZoneInfo = TimeZoneInfo.Local
                            Select Case result
                                Case DialogResult.Yes
                                    My.Settings.UseLocalTimeZone = True
                                Case DialogResult.Cancel
                                    Form1.Close()
                            End Select
                        End If
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), row.Value))

                Case NameOf(ServerDataIndexes.lastName)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.firstName)
                    s_firstName = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), s_firstName))

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
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Phone battery status is {row.Value}"))

                Case NameOf(ServerDataIndexes.lastConduitDateTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, New KeyValuePair(Of String, String)(NameOf(ServerDataIndexes.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ServerDataIndexes.lastConduitDateTime), CurrentUICulture))))

                Case NameOf(ServerDataIndexes.lastConduitUpdateServerDateTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, row.Value.Epoch2DateTimeString))

                Case NameOf(ServerDataIndexes.medicalDeviceFamily)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.medicalDeviceInformation)
                    HandleComplexItems(row, recordNumber, "medicalDeviceInformation", s_listOfSummaryRecords)
                    s_pumpModelNumber = PatientData.MedicalDeviceInformation.ModelNumber
                    Form1.SerialNumberButton.Text = $"{ PatientData.MedicalDeviceInformation.DeviceSerialNumber} Details..."

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
                    s_timeToNextCalibrationHours = Short.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.finalCalibration)
                    If Boolean.Parse(row.Value) Then
                        s_timeToNextCalibrationMinutes = -1
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.sensorDurationMinutes)
                    s_listOfSummaryRecords.Add(New SummaryRecord(ServerDataIndexes.sensorDurationMinutes, "-1", "No data from pump"))

                Case NameOf(ServerDataIndexes.sensorDurationHours)
                    s_sensorDurationHours = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.transmitterPairedTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.systemStatusTimeRemaining)
                    s_systemStatusTimeRemaining = New TimeSpan(0, CInt(row.Value), 0)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.gstBatteryLevel)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.pumpBannerState)
                    s_pumpBannerStateValue = JsonToLisOfDictionary(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))
                    Form1.PumpBannerStateLabel.Visible = False

                Case NameOf(ServerDataIndexes.therapyAlgorithmState)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))

                Case NameOf(ServerDataIndexes.reservoirLevelPercent)
                    s_reservoirLevelPercent = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Reservoir is {row.Value}%"))

                Case NameOf(ServerDataIndexes.reservoirAmount)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Full reservoir holds {row.Value}U"))

                Case NameOf(ServerDataIndexes.pumpSuspended)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.pumpBatteryLevelPercent)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.reservoirRemainingUnits)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Reservoir has {row.Value}U remaining"))

                Case NameOf(ServerDataIndexes.conduitInRange)
                    s_pumpInRangeOfPhone = CBool(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Phone {If(s_pumpInRangeOfPhone, "is", "is not")} in range of pump"))

                Case NameOf(ServerDataIndexes.conduitMedicalDeviceInRange)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Pump {If(CBool(row.Value), "is", "is not")} in range of phone"))

                Case NameOf(ServerDataIndexes.conduitSensorInRange)
                    s_pumpInRangeOfTransmitter = CBool(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Transmitter {If(s_pumpInRangeOfTransmitter, "is", "is not")} in range of pump"))

                Case NameOf(ServerDataIndexes.systemStatusMessage)
                    s_systemStatusMessage = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case NameOf(ServerDataIndexes.sensorState)
                    s_sensorState = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case NameOf(ServerDataIndexes.gstCommunicationState)
                    s_gstCommunicationState = Boolean.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.pumpCommunicationState)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.timeFormat)
                    s_timeFormat = PatientData.TimeFormat
                    s_timeWithMinuteFormat = If(s_timeFormat = "HR_12", TimeFormatTwelveHourWithMinutes, TimeFormatMilitaryWithMinutes)
                    s_timeWithoutMinuteFormat = If(s_timeFormat = "HR_12", TimeFormatTwelveHourWithoutMinutes, TimeFormatMilitaryWithoutMinutes)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))
                Case NameOf(ServerDataIndexes.bgUnits)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.maxAutoBasalRate)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.maxBolusAmount)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.sgBelowLimit)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.approvedForTreatment)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.lastAlarm)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))
                    s_lastAlarmValue = LoadIndexedItems(row.Value)

                Case NameOf(ServerDataIndexes.activeInsulin)
                    s_activeInsulin = PatientData.ActiveInsulin
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))

                Case NameOf(ServerDataIndexes.basal)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))

                Case NameOf(ServerDataIndexes.lastSensorTime)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.lastSG)
                    s_lastSgRecord = New SG(PatientData.LastSG, 0)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))

                Case NameOf(ServerDataIndexes.lastSGTrend)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.limits)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))
                    s_listOfLimitRecords = PatientData.Limits

                Case NameOf(ServerDataIndexes.belowHypoLimit)
                    s_belowHypoLimit = PatientData.BelowHypoLimit.GetRoundedValue(decimalDigits:=1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Time below limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case NameOf(ServerDataIndexes.aboveHyperLimit)
                    s_aboveHyperLimit = PatientData.AboveHyperLimit.GetRoundedValue(decimalDigits:=1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Time above limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case NameOf(ServerDataIndexes.timeInRange)
                    s_timeInRange = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row, $"Time in range = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case NameOf(ServerDataIndexes.averageSGFloat)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.averageSG)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, row))

                Case NameOf(ServerDataIndexes.markers)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))

                Case NameOf(ServerDataIndexes.sgs)
                    s_listOfSummaryRecords.Add(New SummaryRecord(recordNumber, CType(recordNumber, ServerDataIndexes), ClickToShowDetails))
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

    Friend Sub UpdateMarkerTabs()
        With Form1
            .TableLayoutPanelAutoBasalDelivery.DisplayDataTableInDGV(
                dGV:= .DgvAutoBasalDelivery,
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfAutoBasalDeliveryMarkers),
                rowIndex:=ServerDataIndexes.markers)
            .TableLayoutPanelAutoModeStatus.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfAutoModeStatusMarkers),
                className:=NameOf(AutoModeStatus),
                attachHandlers:=AddressOf AutoModeStatusHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
            Dim table As DataTable = ClassCollectionToDataTable(listOfClass:=s_listOfBgReadingMarkers)
            .TableLayoutPanelBgReadings.DisplayDataTableInDGV(
                table,
                className:=NameOf(BgReading),
                attachHandlers:=AddressOf BgReadingHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
            .TableLayoutPanelInsulin.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfInsulinMarkers),
                className:=NameOf(Insulin),
                attachHandlers:=AddressOf InsulinHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
            .TableLayoutPanelMeal.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfMealMarkers),
                className:=NameOf(Meal),
                attachHandlers:=AddressOf MealHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
            .TableLayoutPanelCalibration.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfCalibrationMarkers),
                className:=NameOf(Calibration),
                attachHandlers:=AddressOf CalibrationHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
            .TableLayoutPanelLowGlucoseSuspended.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfLowGlucoseSuspendedMarkers),
                className:=NameOf(LowGlucoseSuspended),
                attachHandlers:=AddressOf LowGlucoseSuspendedHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
            .TableLayoutPanelTimeChange.DisplayDataTableInDGV(
                table:=ClassCollectionToDataTable(listOfClass:=s_listOfTimeChangeMarkers),
                className:=NameOf(TimeChange),
                attachHandlers:=AddressOf TimeChangeHelpers.AttachHandlers,
                rowIndex:=ServerDataIndexes.markers,
                hideRecordNumberColumn:=False)
        End With

    End Sub

    Friend Sub UpdatePumpBannerStateTab()
        Dim listOfPumpBannerState As New List(Of BannerState)
        For Each dic As Dictionary(Of String, String) In s_pumpBannerStateValue
            Dim typeValue As String = ""
            If dic.TryGetValue("type", typeValue) Then
                Dim bannerStateRecord1 As BannerState = DictionaryToClass(Of BannerState)(dic, listOfPumpBannerState.Count + 1)
                listOfPumpBannerState.Add(bannerStateRecord1)
                Form1.PumpBannerStateLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point)
                Select Case typeValue
                    Case "TEMP_TARGET"
                        Dim minutes As Integer = bannerStateRecord1.timeRemaining
                        Form1.PumpBannerStateLabel.BackColor = Color.Lime
                        Form1.PumpBannerStateLabel.ForeColor = Form1.PumpBannerStateLabel.BackColor.GetContrastingColor
                        Form1.PumpBannerStateLabel.Text = $"Target {If(NativeMmolL, "8.3", "150")}  {minutes.ToHours} hr"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "BG_REQUIRED"
                        Form1.PumpBannerStateLabel.BackColor = Color.CadetBlue
                        Form1.PumpBannerStateLabel.ForeColor = Form1.PumpBannerStateLabel.BackColor.GetContrastingColor

                        Form1.PumpBannerStateLabel.Text = "Enter BG"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "DELIVERY_SUSPEND"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.ForeColor = Form1.PumpBannerStateLabel.BackColor.GetContrastingColor
                        Form1.PumpBannerStateLabel.Text = "Delivery Suspended"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "LOAD_RESERVOIR"
                        Form1.PumpBannerStateLabel.BackColor = Color.Yellow
                        Form1.PumpBannerStateLabel.ForeColor = Form1.PumpBannerStateLabel.BackColor.GetContrastingColor
                        Form1.PumpBannerStateLabel.Text = "Load Reservoir"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "PROCESSING_BG"
                        Stop
                    Case "SUSPENDED_BEFORE_LOW"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.ForeColor = Form1.PumpBannerStateLabel.BackColor.GetContrastingColor
                        Form1.PumpBannerStateLabel.Text = "Suspended before low"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        Form1.PumpBannerStateLabel.Font = New Font("Segoe UI", 7.0F, FontStyle.Bold, GraphicsUnit.Point)
                    Case "TEMP_BASAL"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.ForeColor = Form1.PumpBannerStateLabel.BackColor.GetContrastingColor
                        Form1.PumpBannerStateLabel.Text = "Temp Basal"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        Form1.PumpBannerStateLabel.Font = New Font("Segoe UI", 7.0F, FontStyle.Bold, GraphicsUnit.Point)
                    Case "WAIT_TO_ENTER_BG"
                        Stop
                    Case Else
                        If Debugger.IsAttached Then
                            MsgBox($"{typeValue} Is unknown banner message!", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
                        End If
                End Select
                Form1.PumpBannerStateLabel.ForeColor = GetContrastingColor(Form1.PumpBannerStateLabel.BackColor)
            Else
                Stop
            End If
        Next
        Dim safeBasalDurationStr As String = ""
        If s_therapyAlgorithmStateValue.TryGetValue(NameOf(TherapyAlgorithmState.SafeBasalDuration), safeBasalDurationStr) Then
            Dim safeBasalDuration As UInteger = CUInt(safeBasalDurationStr)
            If safeBasalDuration > 0 Then
                Form1.LastSgOrExitTimeLabel.Text = $"Exit In { TimeSpan.FromMinutes(safeBasalDuration).ToFormattedTimeSpan("hr")}"
                Form1.LastSgOrExitTimeLabel.Visible = True
            End If
        End If
        Form1.TableLayoutPanelBannerState.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(listOfClass:=listOfPumpBannerState),
            className:=NameOf(BannerState),
            attachHandlers:=AddressOf BannerStateHelpers.AttachHandlers,
            rowIndex:=ServerDataIndexes.pumpBannerState,
            hideRecordNumberColumn:=False)
    End Sub

    ''' <summary>
    ''' Returns a unique file name in MyDocuments of the form baseName(CultureCode).Extension
    ''' given an filename and culture as a seed
    ''' </summary>
    ''' <param Name="baseName">The first part of the file name</param>
    ''' <param Name="cultureName">A valid Culture Name in the form of language-CountryCode</param>
    ''' <param Name="extension">The extension for the file</param>
    ''' <returns>
    ''' A unique file name valid in MyDocuments folder or
    ''' An empty file name on error.
    ''' </returns>
    ''' <param Name="MustBeUnique"></param>
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
            Dim baseWithCultureAndExtension As String = $"{baseName}({cultureName}).{extension}"
            Dim fileNameWithPath As String = Path.Join(DirectoryForProjectData, baseWithCultureAndExtension)

            If MustBeUnique AndAlso File.Exists(fileNameWithPath) Then
                'Get unique file name
                Dim count As Long
                Do
                    count += 1
                    fileNameWithPath = Path.Join(DirectoryForProjectData, $"{baseName}({cultureName}){count}.{extension}")
                    baseWithCultureAndExtension = Path.GetFileName(fileNameWithPath)
                Loop While File.Exists(fileNameWithPath)
            End If

            Return New FileNameStruct(fileNameWithPath, baseWithCultureAndExtension)
        Catch ex As Exception
            Stop
        End Try
        Return New FileNameStruct

    End Function

End Module
