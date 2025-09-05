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
    Private Const DeviceSettings As String = "Device Settings (1 of 2)"
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

    ''' <summary>
    '''  Initializes a new instance of the <see cref="PdfSettingsRecord"/> class
    '''  by extracting data from the specified PDF file.
    ''' </summary>
    ''' <param name="pdfFilePath">
    '''  The full path to the PDF file containing device settings.
    ''' </param>
    ''' <param name="notTesting">
    '''  If set to <see langword="True"/>, WinForms support is required so the cursor
    '''  will change to a wait cursor during processing.
    ''' </param>
    Public Sub New(pdfFilePath As String)
        Dim allText As String =
            ExtractTextFromPage(
                filename:=pdfFilePath,
                startPageNumber:=0,
                endPageNumber:=1)
        Dim startIndex As Integer = allText.IndexOf(value:=DeviceSettings)
        Dim tempString As String =
            allText.Substring(startIndex:=startIndex + DeviceSettings.Length).TrimStart
        Dim length As Integer = tempString.IndexOf(value:="  ")
        Me.UserName = tempString.Trim.Substring(startIndex:=0, length).Trim()
        If Me.UserName.Contains(value:=", ") Then
            Dim split As String() = Me.UserName.Split(separator:=", ")
            If split.Length = 2 Then
                Me.UserName = $"{split(1)} {split(0)}"
            End If
        End If
        If allText.Contains(value:="Device Settings(1 of 2)") Then
            Me.IsValid = False
            Return
        End If
        Dim tables As Dictionary(Of String, PdfTable) =
            GetTableList(pdfFilePath, startPageNumber:=0, endPageNumber:=1)
        Dim listOfAllTextLines As List(Of String) = allText.SplitLines(Trim:=True)

        ' Get Sensor and Basal 4 Line to determine Active Basal later
        Dim basal4Line As String
        For Each s As String In listOfAllTextLines
            If s.Contains(value:="Basal 4") Then
                basal4Line = s
            End If
        Next

        Dim presetTempKeyIndex As Integer = 0
        For Each kvp As KeyValuePair(Of String, PdfTable) In tables
            Try
                Dim itemKey As String = kvp.Key
                Dim sTable As StringTable
                Dim table As PdfTable = kvp.Value
                Dim tableHeader As String
                Select Case True
                    Case String.IsNullOrWhiteSpace(value:=itemKey)
                        Continue For
                    Case itemKey.StartsWith(value:=MaximumBasalRateHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table:=tables.Values(index:=0),
                                tableHeader:=MaximumBasalRateHeader)
                        Dim key As String = MaximumBasalRateHeader
                        Me.Basal.MaximumBasalRate =
                            sTable.GetSingleLineValue(Of Single)(key)
                    Case itemKey.StartsWith(NamedBasalHeader)
                        Dim tableNumber As Integer = ExtractIndex(itemKey)
                        Dim key As String = Me.Basal.NamedBasal.Keys(index:=tableNumber - 1)
                        Dim indexOfKey As Integer = allText.IndexOf(value:=key)
                        Dim isActive As Boolean =
                            allText.Substring(
                                startIndex:=indexOfKey + key.Length + 2,
                                length:=1) = "("c
                        Me.Basal.NamedBasal(key) = New NamedBasalRecord(table, isActive)
                    Case itemKey.StartsWith(value:=BolusWizardHeader)
                        tableHeader = BolusWizardHeader
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader)
                        Me.Bolus.BolusWizard = New BolusWizardRecord(sTable)
                    Case itemKey.StartsWith(value:=EasyBolusHeader)
                        tableHeader = EasyBolusHeader
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader)
                        Me.Bolus.EasyBolus = New EasyBolusRecord(sTable)
                    Case itemKey.StartsWith(value:=CarbohydrateRatiosHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=CarbohydrateRatiosHeader)
                        Me.Bolus.DeviceCarbohydrateRatios.Clear()
                        For Each e As IndexClass(Of StringTable.Row) In
                            sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim item As New DeviceCarbRatioRecord(e.Value)
                            If Not item.IsValid Then Exit For
                            Me.Bolus.DeviceCarbohydrateRatios.Add(item)
                        Next
                    Case itemKey.StartsWith(InsulinSensitivityHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=InsulinSensitivityHeader)
                        Me.Bolus.InsulinSensitivity.Clear()
                        For Each e As IndexClass(Of StringTable.Row) In
                            sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim item As New InsulinSensitivityRecord(e.Value)
                            If Not item.IsValid Then Exit For
                            Me.Bolus.InsulinSensitivity.Add(item)
                        Next
                    Case itemKey.StartsWith(value:=BloodGlucoseTargetHeader)
                        Me.Bolus.BloodGlucoseTarget.Clear()
                        sTable =
                            ConvertPdfTableToStringTable(
                            table,
                            tableHeader:=BloodGlucoseTargetHeader)

                        For Each e As IndexClass(Of StringTable.Row) In
                            sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim item As New BloodGlucoseTargetRecord(r:=e.Value)
                            If Not item.IsValid Then Exit For
                            Me.Bolus.BloodGlucoseTarget.Add(item)
                        Next
                    Case itemKey.StartsWith(value:=PresetBolusHeader)
                        tableHeader = PresetBolusHeader
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader)

                        For Each e As IndexClass(Of StringTable.Row) In
                            sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim key As String = Me.PresetBolus.Keys(index:=e.Index - 1)
                            Me.PresetBolus(key) = New PresetBolusRecord(e.Value, key)
                        Next
                    Case itemKey.StartsWith(value:=BasalRatesHeader)
                        Dim index As Integer = ExtractIndex(itemKey) - 1
                        Dim key As String =
                            Me.Basal.NamedBasal.Keys(index)
                        sTable = ConvertPdfTableToStringTable(
                            table,
                            tableHeader:=BasalRatesHeader)

                        Me.Basal.NamedBasal(key).UpdateBasalRates(sTable)
                    Case itemKey.StartsWith(value:=PresetTempHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=PresetTempHeader)

                        For Each e As IndexClass(Of StringTable.Row) In
                            sTable.Rows.WithIndex

                            If e.IsFirst Then Continue For
                            Dim key As String = Me.PresetTemp.Keys(index:=presetTempKeyIndex)
                            presetTempKeyIndex += 1
                            Me.PresetTemp(key) = New PresetTempRecord(r:=e.Value, key)
                        Next
                    Case itemKey.StartsWith(value:=SmartGuardHeader)
                        tableHeader = SmartGuardHeader
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader)
                        If sTable.Rows.Count = 3 Then
                            Dim smartGuard As String =
                                sTable.GetSingleLineValue(Of String)(key:=SmartGuardHeader)
                            Me.SmartGuard = New SmartGuardRecord(sTable, smartGuard)
                        Else
                            Dim smartGuard As String = "Off"
                            Const options As StringSplitOptions =
                                StringSplitOptions.RemoveEmptyEntries
                            For Each s As IndexClass(Of String) In
                                listOfAllTextLines.WithIndex

                                If s.Value.StartsWith(value:=SmartGuardHeader) Then
                                    s.MoveNext()
                                    smartGuard =
                                        s.Value.Split(separator:=" ", options) _
                                               .ToList(index:=1)
                                    Exit For
                                End If
                            Next
                            Me.SmartGuard = New SmartGuardRecord(sTable, smartGuard)
                        End If
                    Case itemKey.StartsWith(value:=LowReservoirRemindersHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=LowReservoirRemindersHeader)
                        Me.Reminders = New RemindersRecord(sTable)
                    Case itemKey.StartsWith(value:=HighAlertsHeader)
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader:=HighAlertsHeader)
                        Me.HighAlerts = New HighAlertsRecord(sTable, listOfAllTextLines)
                    Case itemKey.StartsWith(value:=MissedMealBolusHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=MissedMealBolusHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String =
                                Me.Reminders.MissedMealBolus.Keys(index:=e.Index - 1)
                            Me.Reminders.MissedMealBolus(key) =
                                New MealStartEndRecord(r:=e.Value, key)
                        Next
                    Case itemKey.StartsWith(value:=LowAlertsHeader)
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader:=LowAlertsHeader)
                        Me.LowAlerts = New LowAlertsRecord(sTable, listOfAllTextLines)
                    Case itemKey.StartsWith(value:=SensorHeader)
                        tableHeader = SensorHeader
                        sTable = ConvertPdfTableToStringTable(table, tableHeader)
                        Me.Sensor = New SensorRecord(sTable)
                    Case itemKey.StartsWith(value:=PersonalRemindersHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=PersonalRemindersHeader)
                        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                            If e.IsFirst Then Continue For
                            Dim key As String =
                                Me.Reminders.PersonalReminders.Keys(index:=e.Index - 1)
                            Me.Reminders.PersonalReminders(key) =
                                New PersonalRemindersRecord(row:=e.Value, key)
                        Next

                    Case itemKey.StartsWith(value:=CalibrationRemindersHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=CalibrationRemindersHeader)
                        Me.Sensor.UpdateCalibrationReminder(sTable)
                    Case itemKey.StartsWith(value:=UtilitiesHeader)
                        sTable =
                            ConvertPdfTableToStringTable(table, tableHeader:=UtilitiesHeader)
                        Me.Utilities = New UtilitiesRecord(sTable)
                        Me.IsValid = True
                    Case itemKey.StartsWith(value:=AutoCalibrationHeader)
                        sTable =
                            ConvertPdfTableToStringTable(
                                table,
                                tableHeader:=AutoCalibrationHeader)
                        Me.Sensor.UpdateCalibrationReminder(sTable)
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
            End Try
        Next
    End Sub

    ''' <summary>
    '''  Extract <see langword="Integer"/> index from itemKey in the form
    '''  of "24 Hour Total({index})"
    ''' </summary>
    ''' <param name="itemKey"></param>
    ''' <returns>index</returns>
    Private Shared Function ExtractIndex(itemKey As String) As Integer
        Dim startIndex As Integer = itemKey.IndexOf("("c) + 1
        Return CInt(itemKey.Substring(startIndex).Replace(oldValue:=")", newValue:=""))
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

    Public Property UserName As String

    Public Property Utilities As New UtilitiesRecord
    Public ReadOnly Property IsValid As Boolean = False

    Public Shared Sub GetSnoozeInfo(
        listOfAllTextLines As List(Of String),
        target As String,
        ByRef snoozeOn As String,
        ByRef snoozeTime As TimeSpan)

        Dim snoozeLine As String
        snoozeOn = "Off"
        snoozeLine = listOfAllTextLines.FindLine(value:=target)
        Dim index As Integer = snoozeLine.IndexOf(value:=")"c)
        If index >= 0 Then
            snoozeLine = snoozeLine.Substring(startIndex:=0, length:=index + 1)
            index = snoozeLine.IndexOf(value:="Snooze ")
            snoozeLine = snoozeLine.Substring(startIndex:=index).Trim(trimChar:=")"c)
            Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
            Dim splitSnoozeLine As String() =
                snoozeLine.Split(separator:=" ", options)
            If splitSnoozeLine.Length = 2 Then
                snoozeOn = "On"
                If Not TimeSpan.TryParse(s:=splitSnoozeLine(1), result:=snoozeTime) Then
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
