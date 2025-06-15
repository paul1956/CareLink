' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Class PdfSettingsRecord
    Private Const AutoCalibrationHeader As String = "Auto Calibration"
    Private Const BasalRatesHeader As String = "Time U/Hr"
    Private Const BloodGlucoseTargetHeader As String = "Time Low"
    Private Const BolusWizardHeader As String = "Bolus Wizard"
    Private Const CalibrationRemindersHeader As String = "Calibration Reminder"
    Private Const CarbohydrateRatiosHeader As String = "Time Ratio"
    Private Const EasyBolusHeader As String = "Easy Bolus"
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
    Private Const SensorHeader As String = "Sensor"
    Private Const SmartGuardHeader As String = "SmartGuard"
    Private Const UtilitiesHeader As String = "Block Mode"

    Public Sub New(filename As String)
        Try
            Dim tables As Dictionary(Of String, PdfTable) = GetTableList(filename, 0, 1)
            Dim allText As String = ExtractTextFromPage(filename, 0, 1)
            Dim listOfAllTextLines As List(Of String) = allText.SplitLines(Trim:=True)

            ' Get Sensor and Basal 4 Line to determine Active Basal later
            Dim basal4Line As String
            For Each s As IndexClass(Of String) In listOfAllTextLines.WithIndex
                If s.Value.Contains("Basal 4") Then
                    basal4Line = s.Value
                End If
            Next

            Dim presetTempKeyIndex As Integer = 0
            For Each kvp As KeyValuePair(Of String, PdfTable) In tables
                Dim itemKey As String = kvp.Key
                Dim sTable As StringTable
                Select Case True
                    Case itemKey.StartsWith(MaximumBasalRateHeader)
                        sTable = ConvertPdfTableToStringTable(tables.Values(0), MaximumBasalRateHeader)
                        Me.Basal.MaximumBasalRate = sTable.GetSingleLineValue(Of Single)(MaximumBasalRateHeader)
                    Case itemKey.StartsWith(NamedBasalHeader)
                        Dim tableNumber As Integer = ExtractIndex(itemKey)
                        Dim key As String = Me.Basal.NamedBasal.Keys(tableNumber - 1)
                        Dim indexOfKey As Integer = allText.IndexOf(key)
                        Dim isActive As Boolean = allText.Substring(indexOfKey + key.Length + 2, 1) = "("c
                        Me.Basal.NamedBasal(key) = New NamedBasalRecord(kvp.Value, isActive)
                    Case itemKey.StartsWith(BolusWizardHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, BolusWizardHeader)
                        Me.Bolus.BolusWizard = New BolusWizardRecord(sTable)
                    Case itemKey.StartsWith(EasyBolusHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, EasyBolusHeader)
                        Me.Bolus.EasyBolus = New EasyBolusRecord(sTable)
                    Case itemKey.StartsWith(CarbohydrateRatiosHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, CarbohydrateRatiosHeader)
                        Me.Bolus.DeviceCarbohydrateRatios.Clear()
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim item As New DeviceCarbRatioRecord(e.Value)
                            If Not item.IsValid Then Exit For
                            Me.Bolus.DeviceCarbohydrateRatios.Add(item)
                        Next
                    Case itemKey.StartsWith(InsulinSensitivityHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, InsulinSensitivityHeader)
                        Me.Bolus.InsulinSensitivity.Clear()
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim item As New InsulinSensitivityRecord(e.Value)
                            If Not item.IsValid Then Exit For
                            Me.Bolus.InsulinSensitivity.Add(item)
                        Next
                    Case itemKey.StartsWith(BloodGlucoseTargetHeader)
                        Me.Bolus.BloodGlucoseTarget.Clear()
                        sTable = ConvertPdfTableToStringTable(kvp.Value, BloodGlucoseTargetHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim item As New BloodGlucoseTargetRecord(e.Value)
                            If Not item.IsValid Then Exit For
                            Me.Bolus.BloodGlucoseTarget.Add(item)
                        Next
                    Case itemKey.StartsWith(PresetBolusHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, PresetBolusHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String = Me.PresetBolus.Keys(e.Index - 1)
                            Me.PresetBolus(key) = New PresetBolusRecord(e.Value, key)
                        Next
                    Case itemKey.StartsWith(BasalRatesHeader)
                        Dim index As Integer = ExtractIndex(itemKey)
                        Dim key As String = Me.Basal.NamedBasal.Keys(index - 1)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, BasalRatesHeader)
                        Me.Basal.NamedBasal(key).UpdateBasalRates(sTable)
                    Case itemKey.StartsWith(PresetTempHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, PresetTempHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String = Me.PresetTemp.Keys(presetTempKeyIndex)
                            presetTempKeyIndex += 1
                            Me.PresetTemp(key) = New PresetTempRecord(e.Value, key)
                        Next
                    Case itemKey.StartsWith(SmartGuardHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, SmartGuardHeader)
                        If sTable.Rows.Count = 3 Then
                            Me.SmartGuard = New SmartGuardRecord(sTable, sTable.GetSingleLineValue(Of String)(SmartGuardHeader))
                        Else
                            Dim smartGuard As String = "Off"
                            For Each s As IndexClass(Of String) In listOfAllTextLines.WithIndex
                                If s.Value.StartsWith(SmartGuardHeader) Then
                                    s.MoveNext()
                                    smartGuard = s.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(1)
                                    Exit For
                                End If
                            Next
                            Me.SmartGuard = New SmartGuardRecord(sTable, smartGuard)
                        End If
                    Case itemKey.StartsWith(LowReservoirRemindersHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, LowReservoirRemindersHeader)
                        Me.Reminders = New RemindersRecord(sTable)
                    Case itemKey.StartsWith(HighAlertsHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, HighAlertsHeader)
                        Me.HighAlerts = New HighAlertsRecord(sTable, listOfAllTextLines)
                    Case itemKey.StartsWith(MissedMealBolusHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, MissedMealBolusHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String = Me.Reminders.MissedMealBolus.Keys(e.Index - 1)
                            Me.Reminders.MissedMealBolus(key) = New MealStartEndRecord(e.Value, key)
                        Next
                    Case itemKey.StartsWith(LowAlertsHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, LowAlertsHeader)
                        Me.LowAlerts = New LowAlertsRecord(sTable, listOfAllTextLines)
                    Case itemKey.StartsWith(SensorHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, SensorHeader)
                        Me.Sensor = New SensorRecord(sTable)
                    Case itemKey.StartsWith(PersonalRemindersHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, PersonalRemindersHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String = Me.Reminders.PersonalReminders.Keys(e.Index - 1)
                            Me.Reminders.PersonalReminders(key) = New PersonalRemindersRecord(e.Value, key)
                        Next

                    Case itemKey.StartsWith(CalibrationRemindersHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, CalibrationRemindersHeader)
                        Me.Sensor.UpdateCalibrationReminder(sTable)
                    Case itemKey.StartsWith(UtilitiesHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, UtilitiesHeader)
                        Me.Utilities = New UtilitiesRecord(sTable)
                        Me.IsValid = True
                    Case itemKey.StartsWith(AutoCalibrationHeader)
                        sTable = ConvertPdfTableToStringTable(kvp.Value, AutoCalibrationHeader)
                        Me.Sensor.UpdateCalibrationReminder(sTable)
                    Case String.IsNullOrWhiteSpace(itemKey)
                        Continue For
                    Case Else
                        Stop
                End Select
            Next
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''  Extract <see langword="Integer"/> index from itemKey in the form of "24 Hour Total({index})"
    ''' </summary>
    ''' <param name="itemKey"></param>
    ''' <returns>index</returns>
    Private Shared Function ExtractIndex(itemKey As String) As Integer
        Dim index As Integer = itemKey.IndexOf("("c) + 1
        Return CInt(itemKey.Substring(index).Replace(")", ""))
    End Function

    Public Property Basal As New PumpBasalRecord

    Public Property Bolus As New DeviceBolusRecord

    Public Property HighAlerts As New HighAlertsRecord

    Public Property LowAlerts As New LowAlertsRecord

    Public Property Notes As New NotesRecord

    Public Property PresetBolus As New Dictionary(Of String, PresetBolusRecord) From {
                {"Bolus 1", New PresetBolusRecord},
                {"Breakfast", New PresetBolusRecord},
                {"Dinner", New PresetBolusRecord},
                {"Lunch", New PresetBolusRecord},
                {"Snack", New PresetBolusRecord},
                {"Bolus 2", New PresetBolusRecord},
                {"Bolus 3", New PresetBolusRecord},
                {"Bolus 4", New PresetBolusRecord}
            }

    Public Property PresetTemp As New Dictionary(Of String, PresetTempRecord) From {
            {"High Activity", New PresetTempRecord},
            {"Moderate Activity", New PresetTempRecord},
            {"Low Activity", New PresetTempRecord},
            {"Sick", New PresetTempRecord},
            {"Temp 1", New PresetTempRecord},
            {"Temp 2", New PresetTempRecord},
            {"Temp 3", New PresetTempRecord},
            {"Temp 4", New PresetTempRecord}
        }

    Public Property Reminders As New RemindersRecord()

    Public Property Sensor As New SensorRecord

    Public Property SmartGuard As New SmartGuardRecord

    Public Property Utilities As New UtilitiesRecord
    Public ReadOnly Property IsValid As Boolean = False

    Public Shared Sub GetSnoozeInfo(listOfAllTextLines As List(Of String), target As String, ByRef snoozeOn As String, ByRef snoozeTime As TimeSpan)
        Dim snoozeLine As String
        snoozeOn = "Off"
        snoozeLine = listOfAllTextLines.FindLineContaining(target)
        Dim index As Integer = snoozeLine.IndexOf(")"c)
        If index >= 0 Then
            snoozeLine = snoozeLine.Substring(0, index + 1)
            index = snoozeLine.IndexOf("Snooze ")
            snoozeLine = snoozeLine.Substring(index).Trim(")"c)
            Dim splitSnoozeLine As String() = snoozeLine.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            If splitSnoozeLine.Length = 2 Then
                snoozeOn = "On"
                If Not TimeSpan.TryParse(splitSnoozeLine(1), snoozeTime) Then
                    Stop
                End If
            Else
                Stop
            End If
        Else
            Stop
        End If
    End Sub

End Class
