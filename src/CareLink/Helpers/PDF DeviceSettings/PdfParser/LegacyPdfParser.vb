' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Module LegacyPdfParser
    Private Const AutoCalibrationHeader As String = "Auto Calibration"
    Private Const BasalRatesHeader As String = "Time U/Hr"
    Private Const BloodGlucoseTargetHeader As String = "Time Low"
    Private Const BolusWizardHeader As String = "Bolus Wizard"
    Private Const CalibrationRemindersHeader As String = "Calibration Reminder"
    Private Const CarbohydrateRatiosHeader As String = "Time Ratio"
    Private Const DeviceSettings As String = "Device Settings (1 of 2)"
    Private Const EasyBolusHeader As String = "Easy Bolus"
    Private Const EndReminder As String = " 24 hour Sensor End Reminder"
    Private Const HighAlertsHeader As String = "Start High"
    Private Const InsulinSensitivityHeader As String = "Time Sensitivity"
    Private Const LowAlertsHeader As String = "Start Low"
    Private Const LowReservoirRemindersHeader As String = "Low Reservoir"
    Private Const MaximumBasalRateHeader As String = "Maximum Basal Rate"
    Private Const MissedMealBolusHeader As String = "Name Start"
    Private Const NamedBasalHeader As String = "24 Hour Total"
    Private Const PersonalRemindersHeader As String = "Name Time"
    Private Const PresetBolusHeader As String = "Name Normal"
    Private Const PresetTempHeader As String = "Name Rate"
    Private Const Sensor24HourEndReminder As String = "< 24 hour Sensor End Reminder"
    Private Const SensorCustimerEndReminder As String = "Custom Sensor End Reminder setting"
    Private Const SensorHeader As String = "Sensor"
    Private Const SmartGuardHeader As String = "SmartGuard"
    Private Const UtilitiesHeader As String = "Block Mode"

    Public Sub ParseLegacy(record As PdfSettingsRecord, tables As Dictionary(Of String, PdfTable), pageText As String)
        Dim listOfAllTextLines As List(Of String) =
            pageText.SplitLines(Trim:=True)

        Dim length As Integer
        Dim startIndex As Integer
        Dim tempString As String

        startIndex = pageText.IndexOf(value:=DeviceSettings)
        If startIndex >= 0 Then
            tempString = pageText.Substring(startIndex:=startIndex + DeviceSettings.Length).TrimStart()
            length = tempString.IndexOf(value:="  ")
            If length > 0 Then
                record.UserName = tempString.Trim().Substring(startIndex:=0, length).Trim()
            End If
            If record.UserName.Contains(value:=", ") Then
                Dim split As String() = record.UserName.Split(separator:=", ")
                If split.Length = 2 Then
                    record.UserName = $"{split(1)} {split(0)}"
                End If
            End If
        Else
            record.UserName = "Unknown User"
        End If

        If Not pageText.Contains(value:="Device Settings") Then
            SetIsValid(record, value:=False)
            Return
        End If

        ' Get Sensor and Basal 4 Line to determine Active Basal later
        Dim basal4Line As String
        If pageText.Contains(value:="Basal 4") Then
            Dim predicate As Func(Of String, Boolean) =
                Function(s As String) As Boolean
                    Return s.Contains(value:="Basal 4")
                End Function
            basal4Line = pageText.SplitLines(Trim:=True).FirstOrDefault(predicate)
        End If

        Dim presetTempKeyIndex As Integer = 0
        For Each kvp As KeyValuePair(Of String, PdfTable) In tables
            Try
                Dim itemKey As String = kvp.Key
                Dim sTable As StringTable
                Dim table As PdfTable = kvp.Value
                Dim tableHeader As String
                Select Case True
                    Case IsNullOrWhiteSpace(value:=itemKey)
                        Continue For

                    Case itemKey.StartsWith(value:=MaximumBasalRateHeader)
                        tableHeader = MaximumBasalRateHeader
                        '  Maximum Basal Rate is not always present, so skip if there are not at least 2 tables
                        '  (header and value)
                        If tables.Values.Count < 2 Then
                            Continue For
                        End If
                        sTable = tables.Values(index:=1).PdfTableToStringTable(tableHeader)

                        Dim key As String = MaximumBasalRateHeader
                        record.Basal.MaximumBasalRate = sTable.GetSingleLineValue(Of Single)(key, endsWith:="U/Hr")

                    Case itemKey.StartsWith(NamedBasalHeader)
                        Dim tableNumber As Integer = PDFParserUtilities.ExtractIndex(itemKey)
                        Dim key As String = record.Basal.NamedBasal.Keys(index:=tableNumber - 1)
                        Dim indexOfKey As Integer = pageText.IndexOf(value:=key) + key.Length + 2
                        Dim isActive As Boolean = pageText.Substring(startIndex:=indexOfKey, length:=1) = "("c
                        Dim named As New NamedBasalRecord()
                        named.InitializeFromPdfTable(table, isActive)
                        record.Basal.NamedBasal(key) = named

                    Case itemKey.StartsWith("24-Hour") AndAlso itemKey.EndsWith("Total")
                        Continue For

                    Case itemKey.StartsWith(value:=BolusWizardHeader)
                        tableHeader = BolusWizardHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        Dim bw As New BolusWizardRecord(sTable)
                        bw.InitializeFromStringTable(sTable)
                        record.Bolus.BolusWizard = bw

                    Case itemKey.StartsWith(value:=EasyBolusHeader)
                        tableHeader = EasyBolusHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        Dim eb As New EasyBolusRecord()
                        eb.InitializeFromStringTable(sTable)
                        record.Bolus.EasyBolus = eb

                    Case itemKey.StartsWith(value:=CarbohydrateRatiosHeader)
                        tableHeader = CarbohydrateRatiosHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Bolus.DeviceCarbohydrateRatios.Clear()
                        For Each e As IndexClass(Of StringTable.Row) In
                        sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim item As New DeviceCarbRatioRecord()
                            item.InitializeFromRow(row:=e.Value)
                            If Not item.IsValid Then Exit For
                            record.Bolus.DeviceCarbohydrateRatios.Add(item)
                        Next

                    Case itemKey.StartsWith(InsulinSensitivityHeader)
                        tableHeader = InsulinSensitivityHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Bolus.InsulinSensitivity.Clear()
                        For Each e As IndexClass(Of StringTable.Row) In
                        sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim item As New InsulinSensitivityRecord()
                            item.InitializeFromRow(row:=e.Value)
                            If Not item.IsValid Then Exit For
                            record.Bolus.InsulinSensitivity.Add(item)
                        Next

                    Case itemKey.StartsWith(value:=BloodGlucoseTargetHeader)
                        record.Bolus.BloodGlucoseTarget.Clear()
                        tableHeader = BloodGlucoseTargetHeader
                        sTable = table.PdfTableToStringTable(tableHeader)

                        For Each e As IndexClass(Of StringTable.Row) In
                        sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim item As New BloodGlucoseTargetRecord()
                            item.InitializeFromRow(row:=e.Value)
                            If Not item.IsValid Then Exit For
                            record.Bolus.BloodGlucoseTarget.Add(item)
                        Next

                    Case itemKey.StartsWith(value:=PresetBolusHeader)
                        tableHeader = PresetBolusHeader
                        sTable = table.PdfTableToStringTable(tableHeader)

                        For Each e As IndexClass(Of StringTable.Row) In
                        sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim key As String = record.PresetBolus.Keys(index:=e.Index - 1)
                            ' Initialize preset bolus record via extension to centralize parsing logic
                            Dim pb As New PresetBolusRecord()
                            pb.InitializeFromRow(row:=e.Value, key:=key)
                            record.PresetBolus(key) = pb
                        Next

                    Case itemKey.StartsWith(value:=BasalRatesHeader)
                        Dim index As Integer = ExtractIndex(itemKey) - 1
                        tableHeader = BasalRatesHeader
                        sTable = table.PdfTableToStringTable(tableHeader)

                        Dim key As String = record.Basal.NamedBasal.Keys(index)
                        record.Basal.NamedBasal(key).UpdateBasalRates(sTable)

                    Case itemKey.StartsWith(value:=PresetTempHeader)
                        tableHeader = PresetTempHeader
                        sTable = table.PdfTableToStringTable(tableHeader)

                        For Each e As IndexClass(Of StringTable.Row) In
                        sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim key As String = record.PresetTemp.Keys(index:=presetTempKeyIndex)
                            presetTempKeyIndex += 1
                            Dim pt As New PresetTempRecord()
                            pt.InitializeFromRow(row:=e.Value, key:=key)
                            record.PresetTemp(key) = pt
                        Next

                    Case itemKey.StartsWith(value:=SmartGuardHeader)
                        tableHeader = SmartGuardHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        If sTable.Rows.Count = 3 Then
                            Dim smartGuard As String = sTable.GetSingleLineValue(Of String)(key:=SmartGuardHeader)
                            Dim sg As New SmartGuardRecord()
                            sg.InitializeFromStringTable(sTable, smartGuard)
                            record.SmartGuard = sg
                        Else
                            Dim smartGuard As String = "Off"
                            Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
                            For Each s As IndexClass(Of String) In
                            listOfAllTextLines.WithIndex

                                If s.Value.StartsWith(value:=SmartGuardHeader) Then
                                    s.MoveNext()
                                    smartGuard = s.Value.Split(separator:=" ", options).ToList()(1)
                                    Exit For
                                End If
                            Next
                            Dim sg2 As New SmartGuardRecord()
                            sg2.InitializeFromStringTable(sTable, smartGuard)
                            record.SmartGuard = sg2
                        End If

                    Case itemKey.StartsWith(value:=LowReservoirRemindersHeader)
                        tableHeader = LowReservoirRemindersHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        Dim rmd As New RemindersRecord()
                        rmd.InitializeFromStringTable(sTable)
                        record.Reminders = rmd

                    Case itemKey.StartsWith(value:=HighAlertsHeader)
                        tableHeader = HighAlertsHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        Dim ha As New HighAlertsRecord()
                        ha.InitializeFromStringTable(sTable, listOfAllTextLines)
                        record.HighAlerts = ha

                    Case itemKey.StartsWith(value:=MissedMealBolusHeader)
                        tableHeader = MissedMealBolusHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String = record.Reminders.MissedMealBolus.Keys(index:=e.Index - 1)
                            Dim ms As New MealStartEndRecord()
                            ms.InitializeFromRow(row:=e.Value, key:=key)
                            record.Reminders.MissedMealBolus(key) = ms
                        Next

                    Case itemKey.StartsWith(value:=LowAlertsHeader)
                        tableHeader = LowAlertsHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        Dim la As New LowAlertsRecord()
                        la.InitializeFromStringTable(sTable, listOfAllTextLines)
                        record.LowAlerts = la

                    Case itemKey.StartsWith(value:=SensorHeader)
                        tableHeader = SensorHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        ' Prefer extension-based initialization to centralize parsing logic.
                        record.Sensor = New SensorRecord()
                        record.Sensor.InitializeFromStringTable(sTable)

                    Case itemKey.StartsWith(value:=PersonalRemindersHeader)
                        tableHeader = PersonalRemindersHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String = record.Reminders.PersonalReminders.Keys(index:=e.Index - 1)
                            Dim pr As New PersonalRemindersRecord()
                            pr.InitializeFromRow(row:=e.Value, key:=key)
                            record.Reminders.PersonalReminders(key) = pr
                        Next

                    Case itemKey.StartsWith(value:=CalibrationRemindersHeader)
                        tableHeader = CalibrationRemindersHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Sensor.UpdateCalibrationReminder(sTable)

                    Case itemKey.StartsWith(value:=UtilitiesHeader)
                        tableHeader = UtilitiesHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        Dim ur As New UtilitiesRecord()
                        ur.InitializeFromStringTable(sTable)
                        record.Utilities = ur
                        SetIsValid(record, value:=True)

                    Case itemKey.StartsWith(value:=AutoCalibrationHeader)
                        tableHeader = AutoCalibrationHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Sensor.UpdateCalibrationReminder(sTable)

                    Case itemKey.StartsWith(value:=AutoCalibrationHeader)
                        tableHeader = AutoCalibrationHeader
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Sensor.UpdateCalibrationReminder(sTable)

                    Case itemKey.StartsWith(value:=EndReminder)
                        tableHeader = EndReminder
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Sensor.UpdateCalibrationReminder(sTable)

                    Case itemKey.StartsWith(value:=Sensor24HourEndReminder)
                        tableHeader = Sensor24HourEndReminder
                        sTable = table.PdfTableToStringTable(tableHeader)
                        record.Sensor.SensorEnding.LessThan24Hours = sTable.GetSingleLineValue(Of String)(Sensor24HourEndReminder)
                        Dim result As String = sTable.GetSingleLineValue(Of String)(SensorCustimerEndReminder)
                        Dim state As String = result.Split(separator:=" "c)(0)
                        record.Sensor.SensorEnding.LessThan2State = state
                        If state = "On" Then
                            record.Sensor.SensorEnding.LessThan2 =
                                result.Split(separator:="<"c)(1).Split(separator:="1"c)(0).Trim
                        End If

                    Case Else
                        Throw New UnreachableException(message:=itemKey)
                End Select
            Catch ex As Exception
                Stop
            End Try
        Next
    End Sub

End Module
