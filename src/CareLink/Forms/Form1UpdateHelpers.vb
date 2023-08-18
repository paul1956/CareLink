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

    Private Sub HandleComplexItems(row As KeyValuePair(Of String, String), rowIndex As ItemIndexes, key As String)
        Dim valueList As String() = Loads(row.Value).ToCsv.
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

    Private Sub HandleObsoleteTimes(row As KeyValuePair(Of String, String), rowIndex As ItemIndexes)
        If row.Value = "0" Then
            s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, "", "Obsolete"))
        Else
            s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))
        End If
    End Sub

    Friend Function GetPumpName(value As String) As String
        Select Case value
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
            Debug.Print($"Exiting {NameOf(UpdateDataTables)}, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If

        s_listOfSummaryRecords.Clear()

        Dim markerRowString As String = ""
        If RecentData.TryGetValue(ItemIndexes.clientTimeZoneName.ToString, markerRowString) Then
            PumpTimeZoneInfo = CalculateTimeZone(markerRowString)
        End If

        s_lastMedicalDeviceDataUpdateServerEpoch = CLng(RecentData(ItemIndexes.lastMedicalDeviceDataUpdateServerTime.ToString))
        If RecentData.TryGetValue(ItemIndexes.therapyAlgorithmState.ToString, markerRowString) Then
            s_therapyAlgorithmStateValue = Loads(markerRowString)
            InAutoMode = s_therapyAlgorithmStateValue.Count > 0 AndAlso {"AUTO_BASAL", "SAFE_BASAL"}.Contains(s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmStateRecord.autoModeShieldState)))
        End If

#Region "Update all Markers"

        If RecentData.TryGetValue(ItemIndexes.sgs.ToString, markerRowString) Then
            s_listOfSgRecords = LoadList(markerRowString).ToSgList()
        End If

        If RecentData.TryGetValue(ItemIndexes.basal.ToString, markerRowString) Then
            Dim item As BasalRecord = DictionaryToClass(Of BasalRecord)(Loads(markerRowString), recordNumber:=0)
            item.OaDateTime(s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime)
            s_listOfManualBasal.Add(item)
        End If
        Form1.MaxBasalPerHourLabel.Text = If(RecentData.TryGetValue(ItemIndexes.markers.ToString, markerRowString),
                                             CollectMarkers(markerRowString),
                                             ""
                                            )

#End Region ' Update all Markers

        s_systemStatusTimeRemaining = Nothing
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In RecentData.WithIndex()

            Dim row As KeyValuePair(Of String, String) = c.Value
            If row.Value Is Nothing Then
                row = KeyValuePair.Create(row.Key, "")
            End If

            Dim rowIndex As ItemIndexes = CType(c.Index, ItemIndexes)
            Select Case GetItemIndex(row.Key)
                Case ItemIndexes.lastSensorTS
                    HandleObsoleteTimes(row, rowIndex)

                Case ItemIndexes.medicalDeviceTimeAsString
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                    s_medicalDeviceTimeAsString = row.Value
                Case ItemIndexes.lastSensorTSAsString,
                       ItemIndexes.kind,
                       ItemIndexes.version
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.pumpModelNumber
                    s_pumpModelNumber = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, GetPumpName(s_pumpModelNumber)))

                Case ItemIndexes.currentServerTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexes.lastConduitTime
                    HandleObsoleteTimes(row, rowIndex)

                Case ItemIndexes.lastConduitUpdateServerTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexes.lastMedicalDeviceDataUpdateServerTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexes.firstName
                    s_firstName = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, s_firstName))

                Case ItemIndexes.lastName
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.conduitSerialNumber
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.conduitBatteryLevel
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery is at {row.Value}%."))

                Case ItemIndexes.conduitBatteryStatus
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery status is {row.Value}"))

                Case ItemIndexes.conduitInRange
                    s_pumpInRangeOfPhone = CBool(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone {If(s_pumpInRangeOfPhone, "is", "is not")} in range of pump"))

                Case ItemIndexes.conduitMedicalDeviceInRange
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump {If(CBool(row.Value), "is", "is not")} in range of phone"))

                Case ItemIndexes.conduitSensorInRange
                    s_pumpInRangeOfTransmitter = CBool(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Transmitter {If(s_pumpInRangeOfTransmitter, "is", "is not")} in range of pump"))

                Case ItemIndexes.medicalDeviceFamily
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.sensorState
                    s_sensorState = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case ItemIndexes.medicalDeviceSerialNumber
                    Form1.SerialNumberButton.Text = $"{row.Value} Details..."
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump serial number is {row.Value}."))

                Case ItemIndexes.medicalDeviceTime
                    HandleObsoleteTimes(row, rowIndex)

                Case ItemIndexes.sMedicalDeviceTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.reservoirLevelPercent
                    s_reservoirLevelPercent = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir is {row.Value}%"))

                Case ItemIndexes.reservoirAmount
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Full reservoir holds {row.Value}U"))

                Case ItemIndexes.reservoirRemainingUnits
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir has {row.Value}U remaining"))

                Case ItemIndexes.medicalDeviceBatteryLevelPercent
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump battery is at {row.Value}%"))

                Case ItemIndexes.sensorDurationHours
                    s_sensorDurationHours = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.timeToNextCalibHours
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                    s_timeToNextCalibrationHours = CUShort(row.Value)

                Case ItemIndexes.calibStatus
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_calibrationMessages, NameOf(s_calibrationMessages)))

                Case ItemIndexes.bgUnits,
                     ItemIndexes.timeFormat
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.lastSensorTime
                    HandleObsoleteTimes(row, rowIndex)

                Case ItemIndexes.sLastSensorTime,
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
                    If s_listOfSgRecords.Count > 2 Then
                        s_lastSgValue = s_listOfSgRecords.Item(s_listOfSgRecords.Count - 2).sg
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
                    Form1.PumpBannerStateLabel.Visible = False

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
                    If RecentData.TryGetValue(NameOf(ItemIndexes.gstBatteryLevel), gstBatteryLevel) Then
                        Continue For
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexes.gstBatteryLevel, "-1", "No data from pump"))

                Case ItemIndexes.gstBatteryLevel
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Transmitter battery is at {row.Value}%"))

                Case ItemIndexes.lastConduitDateTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ItemIndexes.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ItemIndexes.lastConduitDateTime), CurrentUICulture))))

                Case ItemIndexes.maxAutoBasalRate
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.maxBolusAmount
                    s_listOfSummaryRecords.Add(New SummaryRecord(GetItemIndex(c.Value.Key), row))
                    Dim tempStr As String = Nothing
                    If Not RecentData.TryGetValue(NameOf(ItemIndexes.sensorDurationMinutes), tempStr) Then
                        s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexes.sensorDurationMinutes, "-1", "No data from pump"))
                    End If
                    If Not RecentData.TryGetValue(NameOf(ItemIndexes.timeToNextCalibrationMinutes), tempStr) Then
                        s_timeToNextCalibrationMinutes = UShort.MaxValue
                        s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexes.timeToNextCalibrationMinutes, s_timeToNextCalibrationMinutes.ToString, "No data from pump"))
                    End If

                Case ItemIndexes.sensorDurationMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.timeToNextCalibrationMinutes
                    s_timeToNextCalibrationMinutes = CUShort(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.clientTimeZoneName
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Value))

                Case ItemIndexes.sgBelowLimit,
                     ItemIndexes.averageSGFloat
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.timeToNextCalibrationRecommendedMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.calFreeSensor,
                     ItemIndexes.finalCalibration
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.appModelType
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.cgmInfo
                    HandleComplexItems(row, rowIndex, ItemIndexes.cgmInfo.ToString)

                Case ItemIndexes.systemStatusTimeRemaining
                    s_systemStatusTimeRemaining = New TimeSpan(0, CInt(row.Value), 0)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.medicalDeviceInformation
                    HandleComplexItems(row, rowIndex, ItemIndexes.medicalDeviceInformation.ToString)

                Case ItemIndexes.timeToNextEarlyCalibrationMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexes.calibrationIcon
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_calibrationIconMessages, NameOf(s_calibrationIconMessages)))

                Case ItemIndexes.typeCast
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
                              ItemIndexes.markers)
            .TableLayoutPanelAutoModeStatus.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfAutoModeStatusMarkers),
                              NameOf(AutoModeStatusRecord),
                              AddressOf AutoModeStatusRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
                              False)
            .TableLayoutPanelSgReadings.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfSgReadingMarkers),
                              NameOf(SgReadingRecord),
                              AddressOf SgReadingRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
                              False)
            .TableLayoutPanelInsulin.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfInsulinMarkers),
                              NameOf(InsulinRecord),
                               AddressOf InsulinRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
            False)
            .TableLayoutPanelMeal.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfMealMarkers),
                              NameOf(MealRecord),
                              AddressOf MealRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
                              False)
            .TableLayoutPanelCalibration.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfCalibrationMarkers),
                              NameOf(CalibrationRecord),
                              AddressOf CalibrationRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
                              False)
            .TableLayoutPanelLowGlucoseSuspended.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfLowGlucoseSuspendedMarkers),
                              NameOf(LowGlucoseSuspendRecord),
                              AddressOf LowGlucoseSuspendRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
                              False)
            .TableLayoutPanelTimeChange.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfTimeChangeMarkers),
                              NameOf(TimeChangeRecord),
                              AddressOf TimeChangeRecordHelpers.AttachHandlers,
                              ItemIndexes.markers,
                              False)
        End With

    End Sub

    Friend Sub UpdatePumpBannerStateTab()
        Dim listOfPumpBannerState As New List(Of BannerStateRecord)
        For Each dic As Dictionary(Of String, String) In s_pumpBannerStateValue
            Dim typeValue As String = ""
            If dic.TryGetValue("type", typeValue) Then
                Dim bannerStateRecord1 As BannerStateRecord = DictionaryToClass(Of BannerStateRecord)(dic, listOfPumpBannerState.Count + 1)
                listOfPumpBannerState.Add(bannerStateRecord1)
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
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "DELIVERY_SUSPEND"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.Text = "Delivery Suspended"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "LOAD_RESERVOIR"
                    Case "PROCESSING_BG"
                    Case "SUSPENDED_BEFORE_LOW"
                        Form1.PumpBannerStateLabel.BackColor = Color.IndianRed
                        Form1.PumpBannerStateLabel.Text = "Suspended before low"
                        Form1.PumpBannerStateLabel.Visible = True
                        Form1.PumpBannerStateLabel.Dock = DockStyle.Bottom
                    Case "TEMP_BASAL"
                    Case "WAIT_TO_ENTER_BG"
                    Case Else
                        If Debugger.IsAttached Then
                            MsgBox($"{typeValue} is unknown banner message!", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
                        End If
                End Select
            Else
                Stop
            End If
        Next
        Dim safeBasalDurationStr As String = ""
        If s_therapyAlgorithmStateValue.TryGetValue(NameOf(TherapyAlgorithmStateRecord.safeBasalDuration), safeBasalDurationStr) Then
            Dim safeBasalDuration As UInteger = CUInt(safeBasalDurationStr)
            If safeBasalDuration > 0 Then
                Form1.LastSgOrExitTimeLabel.Text = $"Exit in {TimeSpan.FromMinutes(safeBasalDuration):h\:mm} hr"
                Form1.LastSgOrExitTimeLabel.Visible = True
            End If
        End If
        Form1.TableLayoutPanelBannerState.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(listOfPumpBannerState),
                              NameOf(BannerStateRecord),
                              AddressOf BannerStateRecordHelpers.AttachHandlers,
                              ItemIndexes.pumpBannerState,
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
    Public Function GetDataFileName(baseName As String, cultureName As String, extension As String, MustBeUnique As Boolean) As FileNameStruct
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
            Dim fileNameWithPath As String = Path.Combine(GetDirectoryForProjectData(), baseWithCultureAndExtension)

            If MustBeUnique AndAlso File.Exists(fileNameWithPath) Then
                'Get unique file name
                Dim count As Long
                Do
                    count += 1
                    fileNameWithPath = Path.Combine(GetDirectoryForProjectData(), $"{baseName}({cultureName}){count}.{extension}")
                    baseWithCultureAndExtension = Path.GetFileName(fileNameWithPath)
                Loop While File.Exists(fileNameWithPath)
            End If

            Return New FileNameStruct(fileNameWithPath, baseWithCultureAndExtension)
        Catch ex As Exception
        End Try
        Return New FileNameStruct

    End Function

End Module
