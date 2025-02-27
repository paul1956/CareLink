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

    Private Sub HandleComplexItems(row As KeyValuePair(Of String, String), rowIndex As ServerDataIndexes, key As String)
        Dim valueList As String() = JsonToDictionary(row.Value).ToCsv.
                                                     Replace("{", "").
                                                     Replace("}", "").
                                                     Split(",")
        For Each e As IndexClass(Of String) In valueList.WithIndex
            Dim item As New SummaryRecord(CSng(CSng(rowIndex) + ((e.Index + 1) / 10)),
                                          key,
                                          e.Value.Split(" = ")(0).Trim,
                                          e.Value.Split(" = ")(1).Trim)
            s_listOfSummaryRecords.Add(item)
            If item.Value = "hardwareRevision" Then
                s_pumpHardwareRevision = item.Message
            End If

        Next
    End Sub

    Private Sub HandleObsoleteTimes(row As KeyValuePair(Of String, String), rowIndex As ServerDataIndexes)
        If row.Value = "0" Then
            s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, "", "Obsolete"))
        Else
            s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))
        End If
    End Sub

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

    Friend Function RecentDataEmpty() As Boolean
        Return RecentData Is Nothing OrElse RecentData.Count = 0
    End Function

    Friend Sub UpdateDataTables()

        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()

        Dim markerRowString As String = ""
        If RecentData.TryGetValue("clientTimeZoneName", markerRowString) Then
            PumpTimeZoneInfo = CalculateTimeZone(markerRowString)
        End If

        s_lastMedicalDeviceDataUpdateServerEpoch = CLng(RecentData("lastMedicalDeviceDataUpdateServerTime"))

        If RecentData.TryGetValue("therapyAlgorithmState", markerRowString) Then
            s_therapyAlgorithmStateValue = LoadIndexedItems(markerRowString)
            InAutoMode = s_therapyAlgorithmStateValue.Count > 0 AndAlso {"AUTO_BASAL", "SAFE_BASAL"}.Contains(s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmStateRecord.autoModeShieldState)))
        End If

        If RecentData.TryGetValue("sgs", markerRowString) Then
            s_listOfSgRecords = JsonToLisOfSgs(markerRowString)
        End If

        If RecentData.TryGetValue("basal", markerRowString) Then
            Dim item As Basal = DictionaryToClass(Of Basal)(LoadIndexedItems(markerRowString), recordNumber:=0)
            item.OaDateTime(s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime)
            s_listOfManualBasal.Add(item)
        End If
        Form1.MaxBasalPerHourLabel.Text = If(RecentData.TryGetValue("markers", markerRowString),
                                             CollectMarkers(),
                                             ""
                                            )

        s_systemStatusTimeRemaining = Nothing
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In RecentData.WithIndex()

            Dim row As KeyValuePair(Of String, String) = c.Value
            If row.Value Is Nothing Then
                row = KeyValuePair.Create(row.Key, "")
            End If

            Dim rowIndex As ServerDataIndexes = CType(c.Index, ServerDataIndexes)
            Select Case row.Key

                Case ServerDataIndexes.clientTimeZoneName.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Value))

                Case ServerDataIndexes.lastName.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.firstName.ToString
                    s_firstName = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, s_firstName))

                Case ServerDataIndexes.appModelType.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.appModelNumber.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.currentServerTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ServerDataIndexes.conduitSerialNumber.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.conduitBatteryLevel.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery is at {row.Value}%."))

                Case ServerDataIndexes.conduitBatteryStatus.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery status is {row.Value}"))

                Case ServerDataIndexes.lastConduitDateTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ServerDataIndexes.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ServerDataIndexes.lastConduitDateTime), CurrentUICulture))))

                Case ServerDataIndexes.lastConduitUpdateServerDateTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ServerDataIndexes.medicalDeviceFamily.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.medicalDeviceInformation.ToString
                    HandleComplexItems(row, rowIndex, "medicalDeviceInformation")
                    ' TODO
                    s_pumpModelNumber = PatientData.MedicalDeviceInformation.ModelNumber
                    '    ItemIndexes.deviceSerialNumber.ToString
                    Form1.SerialNumberButton.Text = $"{ PatientData.MedicalDeviceInformation.DeviceSerialNumber} Details..."

                Case ServerDataIndexes.medicalDeviceTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ServerDataIndexes.cgmInfo.ToString
                    HandleComplexItems(row, rowIndex, "cgmInfo")

                Case ServerDataIndexes.calFreeSensor.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.calibStatus.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_calibrationMessages, NameOf(s_calibrationMessages)))

                Case ServerDataIndexes.calibrationIconId.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.timeToNextEarlyCalibrationMinutes.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.timeToNextCalibrationMinutes.ToString
                    s_timeToNextCalibrationMinutes = Short.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(ServerDataIndexes.timeToNextCalibrationMinutes, s_timeToNextCalibrationMinutes.ToString, "No data from pump"))

                Case ServerDataIndexes.timeToNextCalibrationRecommendedMinutes.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.timeToNextCalibHours.ToString
                    s_timeToNextCalibrationHours = Short.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.finalCalibration.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.sensorDurationMinutes.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(ServerDataIndexes.sensorDurationMinutes, "-1", "No data from pump"))

                Case ServerDataIndexes.sensorDurationHours.ToString
                    s_sensorDurationHours = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.transmitterPairedTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.systemStatusTimeRemaining.ToString
                    s_systemStatusTimeRemaining = New TimeSpan(0, CInt(row.Value), 0)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.gstBatteryLevel.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.pumpBannerState.ToString
                    s_pumpBannerStateValue = JsonToLisOfDictionary(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    Form1.PumpBannerStateLabel.Visible = False

                Case ServerDataIndexes.therapyAlgorithmState.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ServerDataIndexes.reservoirLevelPercent.ToString
                    s_reservoirLevelPercent = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir is {row.Value}%"))

                Case ServerDataIndexes.reservoirAmount.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Full reservoir holds {row.Value}U"))

                Case ServerDataIndexes.pumpSuspended.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.pumpBatteryLevelPercent.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.reservoirRemainingUnits.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir has {row.Value}U remaining"))

                Case ServerDataIndexes.conduitInRange.ToString
                    s_pumpInRangeOfPhone = CBool(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone {If(s_pumpInRangeOfPhone, "is", "is not")} in range of pump"))

                Case ServerDataIndexes.conduitMedicalDeviceInRange.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump {If(CBool(row.Value), "is", "is not")} in range of phone"))

                Case ServerDataIndexes.conduitSensorInRange.ToString
                    s_pumpInRangeOfTransmitter = CBool(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Transmitter {If(s_pumpInRangeOfTransmitter, "is", "is not")} in range of pump"))

                Case ServerDataIndexes.systemStatusMessage.ToString
                    s_systemStatusMessage = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case ServerDataIndexes.sensorState.ToString
                    s_sensorState = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case ServerDataIndexes.gstCommunicationState.ToString
                    s_gstCommunicationState = Boolean.Parse(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.pumpCommunicationState.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.timeFormat.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.bgUnits.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.maxAutoBasalRate.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.maxBolusAmount.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.sgBelowLimit.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.approvedForTreatment.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.lastAlarm.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_lastAlarmValue = LoadIndexedItems(row.Value)

                Case ServerDataIndexes.activeInsulin.ToString
                    s_activeInsulin = PatientData.ActiveInsulin
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ServerDataIndexes.basal.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ServerDataIndexes.lastSensorTime.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.lastSG.ToString
                    s_lastSgRecord = New SG(PatientData.LastSG, 0)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ServerDataIndexes.lastSGTrend.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.limits.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_listOfLimitRecords = PatientData.Limits

                Case ServerDataIndexes.belowHypoLimit.ToString
                    s_belowHypoLimit = PatientData.BelowHypoLimit.GetRoundedValue(decimalDigits:=1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time below limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ServerDataIndexes.aboveHyperLimit.ToString
                    s_aboveHyperLimit = PatientData.AboveHyperLimit.GetRoundedValue(decimalDigits:=1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time above limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ServerDataIndexes.timeInRange.ToString
                    s_timeInRange = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time in range = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ServerDataIndexes.averageSGFloat.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.averageSG.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.markers.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ServerDataIndexes.sgs.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_lastSgValue = 0
                    If s_listOfSgRecords.Count > 2 Then
                        s_lastSgValue = s_listOfSgRecords.Item(s_listOfSgRecords.Count - 2).sg
                    End If

                Case ServerDataIndexes.notificationHistory.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_notificationHistoryValue = LoadIndexedItems(row.Value)

                Case ServerDataIndexes.sensorLifeText.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ServerDataIndexes.sensorLifeIcon.ToString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case Else
                    Stop
            End Select
        Next

    End Sub

    Friend Sub UpdateMarkerTabs()
        With Form1
            .TableLayoutPanelAutoBasalDelivery.DisplayDataTableInDGV(
                              .DgvAutoBasalDelivery,
                              ClassCollectionToDataTable(s_listOfAutoBasalDeliveryMarkers),
                              ServerDataIndexes.markers)
            .TableLayoutPanelAutoModeStatus.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfAutoModeStatusMarkers),
                              NameOf(AutoModeStatus),
                              AddressOf AutoModeStatusRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
                              False)
            .TableLayoutPanelSgReadings.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfSgReadingMarkers),
                              NameOf(SgReadingRecord),
                              AddressOf SgReadingRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
                              False)
            .TableLayoutPanelInsulin.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfInsulinMarkers),
                              NameOf(InsulinRecord),
                              AddressOf InsulinRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
            False)
            .TableLayoutPanelMeal.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfMealMarkers),
                              NameOf(MealRecord),
                              AddressOf MealRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
                              False)
            .TableLayoutPanelCalibration.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfCalibrationMarkers),
                              NameOf(CalibrationRecord),
                              AddressOf CalibrationRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
                              False)
            .TableLayoutPanelLowGlucoseSuspended.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfLowGlucoseSuspendedMarkers),
                              NameOf(LowGlucoseSuspendRecord),
                              AddressOf LowGlucoseSuspendRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
                              False)
            .TableLayoutPanelTimeChange.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfTimeChangeMarkers),
                              NameOf(TimeChangeRecord),
                              AddressOf TimeChangeRecordHelpers.AttachHandlers,
                              ServerDataIndexes.markers,
                              False)
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
                        Form1.PumpBannerStateLabel.Text = $"Target {If(NativeMmolL, "8.3", "150")}  {minutes.ToHours} hr"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "BG_REQUIRED"
                        Form1.PumpBannerStateLabel.BackColor = Color.CadetBlue
                        Form1.PumpBannerStateLabel.Text = "Enter BG"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Top
                    Case "DELIVERY_SUSPEND"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.Text = "Delivery Suspended"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "LOAD_RESERVOIR"
                        Form1.PumpBannerStateLabel.BackColor = Color.Yellow
                        Form1.PumpBannerStateLabel.Text = "Load Reservoir"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "PROCESSING_BG"
                        Stop
                    Case "SUSPENDED_BEFORE_LOW"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.Text = "Suspended before low"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                        Form1.PumpBannerStateLabel.Font = New Font("Segoe UI", 7.0F, FontStyle.Bold, GraphicsUnit.Point)
                    Case "TEMP_BASAL"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
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
        If s_therapyAlgorithmStateValue.TryGetValue(NameOf(TherapyAlgorithmStateRecord.safeBasalDuration), safeBasalDurationStr) Then
            Dim safeBasalDuration As UInteger = CUInt(safeBasalDurationStr)
            If safeBasalDuration > 0 Then
                Form1.LastSgOrExitTimeLabel.Text = $"Exit In { TimeSpan.FromMinutes(safeBasalDuration).ToFormattedTimeSpan("hr")}"
                Form1.LastSgOrExitTimeLabel.Visible = True
            End If
        End If
        Form1.TableLayoutPanelBannerState.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(listOfPumpBannerState),
                              NameOf(BannerState),
                              AddressOf BannerStateRecordHelpers.AttachHandlers,
                              ServerDataIndexes.pumpBannerState,
                              False)
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
            Dim fileNameWithPath As String = Path.Combine(DirectoryForProjectData, baseWithCultureAndExtension)

            If MustBeUnique AndAlso File.Exists(fileNameWithPath) Then
                'Get unique file name
                Dim count As Long
                Do
                    count += 1
                    fileNameWithPath = Path.Combine(DirectoryForProjectData, $"{baseName}({cultureName}){count}.{extension}")
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
