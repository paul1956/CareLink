' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Class PdfSettingsRecord

    Public Sub New(filename As String)
        Dim tables As List(Of PdfTable) = GetTableList(filename, 0, 1)
        Dim allText As String = ExtractTextFromPage(filename, 0, 1)
        Dim lines As List(Of String)

        ' 0
        lines = ExtractPdfTableLines(tables(0), "Maximum Basal Rate ")
        Me.Basal.MaximumBasalRate = lines.GetSingleLineValue(Of Single)("Maximum Basal Rate ")

        '1, 2, 3 (10,11,12)
        For i As Integer = 1 To 3
            Dim name As String = Me.Basal.NamedBasals.Keys(i - 1)
            Me.Basal.NamedBasals(name) = New NamedBasalRecord(tables, i, allText, name)
        Next

        ' 4 Bolus Wizard
        lines = ExtractPdfTableLines(tables(4), "Bolus Wizard")
        Me.Bolus.BolusWizard = New BolusWizardRecord(lines)

        ' 5 Easy Bolus
        lines = ExtractPdfTableLines(tables(5), "Easy Bolus")
        Me.Bolus.EasyBolus = New EasyBolusRecord(lines)

        ' 6 Carb Ratio
        lines = ExtractPdfTableLines(tables(6), DeviceCarbRatioRecord.GetColumnTitle)
        Me.Bolus.DeviceCarbohydrateRatios.Clear()
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim item As New DeviceCarbRatioRecord(e.Value.CleanSpaces)
            If Not item.IsValid Then Exit For
            Me.Bolus.DeviceCarbohydrateRatios.Add(item)
        Next

        ' 7 Time Sensitivity
        lines = ExtractPdfTableLines(tables(7), InsulinSensivityRecord.GetColumnTitle)
        Me.Bolus.InsulinSensivity.Clear()
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim item As New InsulinSensivityRecord(e.Value.CleanSpaces)
            If Not item.IsValid Then Exit For
            Me.Bolus.InsulinSensivity.Add(item)
        Next

        ' 8 Blood Glucose Target
        lines = ExtractPdfTableLines(tables(8), BloodGlucoseTargetRecord.GetColumnTitle)
        Me.Bolus.BloodGlucoseTarget.Clear()
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim item As New BloodGlucoseTargetRecord(e.Value.CleanSpaces)
            If Not item.IsValid Then Exit For
            Me.Bolus.BloodGlucoseTarget.Add(item)
        Next

        ' 9 Preset Bolus
        lines = ExtractPdfTableLines(tables(9), PresetBolusRecord.GetColumnTitle)
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim line As String = e.Value
            For Each p As KeyValuePair(Of String, PresetBolusRecord) In Me.PresetBolus
                If line.StartsWith(p.Key) Then
                    line = line.Replace($"{p.Key} ", " ")
                    Me.PresetBolus(p.Key) = New PresetBolusRecord(line)
                    Exit For
                End If
            Next
        Next

        ' 13-14 Preset Temp
        For i As Integer = 13 To 14
            lines = ExtractPdfTableLines(tables(i), PresetTempRecord.GetColumnTitle)
            For Each e As IndexClass(Of String) In lines.WithIndex
                If e.IsFirst Then Continue For
                Dim line As String = e.Value
                For Each p As KeyValuePair(Of String, PresetTempRecord) In Me.PresetTemp
                    If line.StartsWith(p.Key) Then
                        line = line.Replace($"{p.Key} ", " ")
                        Me.PresetTemp(p.Key) = New PresetTempRecord(line)
                        Exit For
                    End If
                Next
            Next
        Next

        Me.SmartGuard = New SmartGuardRecord(ExtractPdfTableLines(tables(15), "SmartGuard"))
        Me.Reminders = New RemindersRecord(ExtractPdfTableLines(tables(16), "Low Reservoir Warning "))
        ' 17 Ignore its titles in odd format

        ' 18
        lines = ExtractPdfTableLines(tables(18), "")
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim line As String = e.Value
            For Each p As KeyValuePair(Of String, MealStartEndRecord) In Me.Reminders.MissedMealBolus
                If line.StartsWith(p.Key) Then
                    line = line.Replace($"{p.Key} ", " ")
                    Me.Reminders.MissedMealBolus(p.Key) = New MealStartEndRecord(line)
                    Exit For
                End If
            Next
        Next

        ' 19 Ignore its titles in odd format

        ' 20
        lines = ExtractPdfTableLines(tables(20), "")
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim line As String = e.Value
            For Each p As KeyValuePair(Of String, PersonalRemindersRecord) In Me.Reminders.PersonalReminders
                If line.StartsWith(p.Key) Then
                    line = line.Replace($"{p.Key} ", " ")
                    Me.Reminders.PersonalReminders(p.Key) = New PersonalRemindersRecord(line)
                    Exit For
                End If
            Next
        Next

        ' Get Sensor
        Dim sensorOn As String = "Off"
        Dim basal4Line As String = ""
        For Each s As IndexClass(Of String) In allText.SplitLines(True).WithIndex
            If s.Value.StartsWith("Sensor") Then
                s.MoveNext()
                lines = s.Value.CleanSpaces.Split(" ").ToList
                sensorOn = lines(1)
                Exit For
            ElseIf s.Value.StartsWith("Basal 4") Then
                basal4Line = s.Value
            End If
        Next
        lines = ExtractPdfTableLines(tables(21), "Calibration Reminder")
        Me.Sensor = New SensorRecord(sensorOn, lines)

        '22, 23, 24 25, 26
        For i As Integer = 22 To 26
            Dim name As String = Me.Basal.NamedBasals.Keys(i - 19)
            Me.Basal.NamedBasals(name) = New NamedBasalRecord(tables, i, basal4Line, name)
        Next

        '27
        lines = ExtractPdfTableLines(tables(27), "Block Mode ")
        Me.Utilities = New UtilitiesRecord(lines)

        Stop
    End Sub

    Public Property Basal As New PumpBasalRecord
    Public Property Bolus As New DeviceBolusRecord

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

    Public Property SmartGuard As SmartGuardRecord
    Public Property HighAlerts As New HighAlertsRecord
    Public Property LowAlerts As New LowAlertsRecord
    Public Property Sensor As SensorRecord
    Public Property Notes As New NotesRecord
    Public Property Reminders As New RemindersRecord()
    Public Property Utilities As UtilitiesRecord
End Class
