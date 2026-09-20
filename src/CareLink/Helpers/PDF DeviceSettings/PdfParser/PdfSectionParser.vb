' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.RegularExpressions
Imports CareLink

Public Class PdfSectionParser
    Private ReadOnly _lines As List(Of String)

    Private ReadOnly _record As PdfSettingsRecord

    Public Sub New(allLines As List(Of String), record As PdfSettingsRecord)
        _lines = allLines
        _record = record
    End Sub

    Private Shared ReadOnly Property ComparisonType As StringComparison =
                    StringComparison.OrdinalIgnoreCase

    Private Shared ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

    Private Shared ReadOnly Property Separator As String() = {"  "}

    Private Shared Function SplitColumns(line As String) As String()
        If String.IsNullOrEmpty(line) Then
            Return Array.Empty(Of String)()
        End If
        ' Split on two-or-more whitespace characters so we tolerate single-space tokens
        ' and the original double-space column separators produced by the PDF extractor.
        Return Regex.Split(input:=line.Trim(), pattern:="\s{2,}")
    End Function

    Private Function GetValueForSearchWord(searchWord As String, Optional endsWith As String = Nothing) As String
        Dim line As String = _lines.FindLineContaining(searchWord)
        Return If(String.IsNullOrEmpty(value:=line),
                  String.Empty,
                  If(String.IsNullOrEmpty(value:=endsWith),
                     line.ExtractBetween(startWord:=searchWord).Trim(),
                     line.ExtractBetween(startWord:=searchWord, endWord:=endsWith).Trim()))
    End Function

    Private Function SafeLineAt(index As Integer) As String
        Return If(index < 0 OrElse index >= _lines.Count,
            String.Empty,
            _lines(index).Trim())
    End Function

    Public Sub ParseAlertsAndUtilities()
        Dim searchWord As String = "Glucose settings - lows"
        Dim lineNumber As Integer = _lines.FindLineNumber(searchWord)
        If lineNumber < 0 Then Return

        ' Base index: first data line after the marker
        Dim baseIndex As Integer = lineNumber

        Dim headerLine As String = Me.SafeLineAt(index:=baseIndex)
        Dim split As String() = SplitColumns(headerLine)
        If split.Length < 3 Then Return
        Dim nameDay As String = If(split.Length > 1, split(1), String.Empty)
        Dim nameNight As String = If(split.Length > 2, split(2), String.Empty)

        Dim startTimeDay As String =
            Me.SafeLineAt(index:=baseIndex + 1).Split(Separator, Options).ElementAtOrDefault(index:=1)
        Dim startTimeNight As String =
            Me.SafeLineAt(index:=baseIndex + 1).Split(Separator, Options).ElementAtOrDefault(index:=2)

        Dim lowLimitDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 2)).ElementAtOrDefault(index:=1)
        Dim lowLimitNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 2)).ElementAtOrDefault(index:=2)

        Dim fallLimitDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 3)).ElementAtOrDefault(index:=1)
        Dim fallLimitNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 3)).ElementAtOrDefault(index:=2)

        Dim alertOnLowDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 4)).ElementAtOrDefault(index:=1)
        Dim alertOnLowNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 4)).ElementAtOrDefault(index:=2)

        Dim alertBeforeLowDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 5)).ElementAtOrDefault(index:=1)
        Dim alertBeforeLowNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 5)).ElementAtOrDefault(index:=2)

        Dim maxVolumeAtNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 6)).ElementAtOrDefault(index:=2)

        Dim snoozeDurationDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 7)).ElementAtOrDefault(index:=1)
        Dim snoozeDurationNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 7)).ElementAtOrDefault(index:=2)

        Dim suspendLimitDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 8)).ElementAtOrDefault(index:=1)
        Dim suspendLimitNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 8)).ElementAtOrDefault(index:=2)

        Dim suspendBeforeLowDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 9)).ElementAtOrDefault(index:=1)
        Dim suspendBeforeLowNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 9)).ElementAtOrDefault(index:=2)

        Dim alertForSuspendBeforeLowDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 10)).ElementAtOrDefault(index:=1)
        Dim alertForSuspendBeforeLowNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 10)).ElementAtOrDefault(index:=2)

        Dim suspendOnLowDay As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 11)).ElementAtOrDefault(index:=1)
        Dim suspendOnLowNight As String =
            SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 11)).ElementAtOrDefault(index:=2)

        Dim itemLow As New LowAlertRecord With {
            .Name = nameDay,
            .Start = startTimeDay,
            .LowLimit = lowLimitDay,
            .FallAlert = fallLimitDay,
            .AlertOnLow = alertOnLowDay,
            .AlertBeforeLow = alertBeforeLowDay,
            .MaxVolumeAtNight = "N/A",
            .SnoozeDuration = snoozeDurationDay,
            .SuspendLimit = suspendLimitDay,
            .SuspendBeforeLow = suspendBeforeLowDay,
            .AlertForSuspendBeforeLow = alertForSuspendBeforeLowDay,
            .SuspendOnLow = suspendOnLowDay,
            .IsValid = True}
        _record.LowAlerts.LowAlert.Add(item:=itemLow)

        itemLow = New LowAlertRecord With {
            .Name = nameNight,
            .Start = startTimeNight,
            .LowLimit = lowLimitNight,
            .FallAlert = fallLimitNight,
            .AlertOnLow = alertOnLowNight,
            .AlertBeforeLow = alertBeforeLowNight,
            .MaxVolumeAtNight = maxVolumeAtNight,
            .SnoozeDuration = snoozeDurationNight,
            .SuspendLimit = suspendLimitNight,
            .SuspendBeforeLow = suspendBeforeLowNight,
            .AlertForSuspendBeforeLow = alertForSuspendBeforeLowNight,
            .SuspendOnLow = suspendOnLowNight,
            .IsValid = True}
        _record.LowAlerts.LowAlert.Add(item:=itemLow)

        ' Highs
        searchWord = "Glucose settings - highs"
        lineNumber = _lines.FindLineNumber(searchWord)
        If lineNumber < 0 Then Return
        baseIndex = lineNumber
        headerLine = Me.SafeLineAt(baseIndex)
        split = SplitColumns(headerLine)
        If split.Length < 3 Then Return
        nameDay = If(split.Length > 1, split(1), String.Empty)
        nameNight = If(split.Length > 2, split(2), String.Empty)

        startTimeDay = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 1)).ElementAtOrDefault(index:=1)
        startTimeNight = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 1)).ElementAtOrDefault(index:=2)

        Dim highLimitDay As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 2)).ElementAtOrDefault(index:=1)
        Dim highLimitNight As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 2)).ElementAtOrDefault(index:=2)

        Dim riseAlertDay As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 3)).ElementAtOrDefault(index:=1)
        Dim riseAlertNight As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 3)).ElementAtOrDefault(index:=2)

        Dim alertOnHighDay As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 4)).ElementAtOrDefault(index:=1)
        Dim alertOnHighNight As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 4)).ElementAtOrDefault(index:=2)

        Dim alertBeforeHighDay As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 5)).ElementAtOrDefault(index:=1)
        Dim alertBeforeHighNight As String = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 5)).ElementAtOrDefault(index:=2)

        maxVolumeAtNight = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 6)).ElementAtOrDefault(index:=2)

        snoozeDurationDay = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 7)).ElementAtOrDefault(index:=1)
        snoozeDurationNight = SplitColumns(line:=Me.SafeLineAt(index:=baseIndex + 7)).ElementAtOrDefault(index:=2)

        Dim itemHigh As New HighAlertRecord With {
            .Name = nameDay,
            .Start = startTimeDay.ToString(),
            .HighLimit = highLimitDay,
            .AlertBeforeHigh = alertBeforeHighDay,
            .TimeBeforeHigh = "",
            .AlertOnHigh = alertOnHighDay,
            .RiseAlert = riseAlertDay,
            .MaxVolumeAtNight = "N/A",
            .SnoozeTime = snoozeDurationDay,
            .IsValid = True}
        _record.HighAlerts.HighAlert.Add(item:=itemHigh)

        itemHigh = New HighAlertRecord With {
            .Name = nameNight,
            .Start = startTimeNight.ToString(),
            .HighLimit = highLimitNight,
            .AlertBeforeHigh = alertBeforeHighNight,
            .TimeBeforeHigh = "",
            .AlertOnHigh = alertOnHighNight,
            .RiseAlert = riseAlertNight,
            .MaxVolumeAtNight = maxVolumeAtNight,
            .SnoozeTime = snoozeDurationNight,
            .IsValid = True}
        _record.HighAlerts.HighAlert.Add(item:=itemHigh)

        ' Utilities
        searchWord = "Alert volume and mute"
        lineNumber = _lines.FindLineNumber(searchWord)
        If lineNumber < 0 Then Return
        Dim extractWord As String = Me.GetValueForSearchWord(searchWord:="Pump sound")
        _record.Utilities.PumpSounds = extractWord

        extractWord = Me.GetValueForSearchWord(searchWord:="Pump vibration")
        _record.Utilities.PumpVibrations = extractWord

        extractWord = Me.GetValueForSearchWord(searchWord:="Lost communication")
        _record.Utilities.LostCommunication = extractWord
    End Sub

    Public Sub ParseBasal()
        ' Max Basal Rate (may be absent)
        Dim searchWord As String = "Max Basal Rate"
        Dim result As String = _lines.FindLineContaining(searchWord)
        If result IsNot Nothing Then
            _record.Basal.MaximumBasalRate =
                result.GetSingleLineValue(Of Single)(key:=searchWord, endsWith:="U/Hr")

        End If

        ' Basal pattern settings table
        searchWord = "Basal pattern settings"
        Dim lineNumber As Integer = _lines.FindLineIndexContaining(searchWord)
        If lineNumber >= 0 Then
            Dim line As String
            Dim split As String()
            Do
                lineNumber += 1
                line = Me.SafeLineAt(index:=lineNumber)
                If line.Length = 0 Then Exit Do
                split = SplitColumns(line)
                ' If the row doesn't have expected columns, assume we've reached the next section.
                If split.Length < 4 Then
                    Exit Do
                End If
                Dim isActive As Boolean =
                    line.Contains(value:="(active)", ComparisonType)
                Dim split0 As String() = split(0).Split(separator:=" "c, Options)
                Dim name As String = split0.FirstOrDefault().ToTitle
                Dim namedBasal As New NamedBasalRecord With {
                    .Total24Hour = split(3),
                    .Active = isActive,
                    .BasalRates = New List(Of BasalRateRecord)(),
                    .IsValid = True}
                Dim timeText As String = split(2)
                Dim timePart As String = timeText.Split(separator:=" - ")(0)
                Dim parsedTime As TimeOnly = TimeOnly.Parse(s:=timePart)
                Dim units As Single = ParseSingle(s:=split(3))
                _record.Basal.NamedBasal(key:=name) = namedBasal
                namedBasal.BasalRates.Add(item:=New BasalRateRecord With {
                    .[Time] = parsedTime,
                    .UnitsPerHr = units,
                    .IsValid = True})
                ' Note: original code did not add namedBasal to record; preserve that behavior for now.
            Loop
        End If
    End Sub

    Public Sub ParseBolus()
        Dim bw As New BolusWizardRecord()
        Dim searchWord As String = "Active insulin time"
        Dim line As String = _lines.FindLineContaining(searchWord)
        Dim extractWord As String
        Dim split As String()
        If line = String.Empty Then
            bw.ActiveInsulinTime = 2
        Else
            extractWord =
                Me.GetValueForSearchWord(searchWord, endsWith:="h").Replace(oldValue:="(", newValue:="")
            bw.ActiveInsulinTime =
                If(extractWord.Contains(value:=":"c),
                   AitLengths(key:=extractWord),
                   ParseSingle(s:=extractWord))
        End If
        bw.BolusWizard = "On"
        _record.Bolus.BolusWizard = bw

        searchWord = "Max bolus"
        extractWord = Me.GetValueForSearchWord(searchWord, endsWith:="u")
        bw.MaximumBolus = ParseSingle(s:=extractWord)

        bw.Units = New DeviceUnitsRecord With {
            .CarbUnits = "grams",
            .BgUnits = If(_lines.Contains(value:="mg/dL"), "mg/dL", "mmol/L")}

        line = _lines.FindLineContaining(searchWord:="Bolus increments")
        split = If(String.IsNullOrEmpty(value:=line),
                   Array.Empty(Of String)(),
                   SplitColumns(line))
        Dim bolusIncrement As Single
        If split.Length = 4 Then
            Dim split3 As String = split(3).RemoveSuffix(suffix:=" U")
            bolusIncrement = If(split3.Contains(value:="--"),
                                Single.NaN,
                                ParseSingle(s:=split3))
        End If

        Dim dualSquareRecord As New DualSquareRecord With {.Dual = "Off", .Square = "Off"}
        _record.Bolus.EasyBolus = New EasyBolusRecord With {
            .EasyBolus = "Off",
            .BolusSpeed = "10 U/Minute",
            .BolusIncrement = bolusIncrement,
            .DualSquare = dualSquareRecord}

        Dim lineNumber As Integer = _lines.FindLineNumber(searchWord:="Carb ratio (g/U)")
        If lineNumber >= 0 Then
            line = Me.SafeLineAt(lineNumber + 1)
            Dim carbRatio As String = line
            _record.Bolus.DeviceCarbohydrateRatios.Add(item:=New DeviceCarbRatioRecord With {
                .Ratio = ParseSingle(s:=carbRatio),
                .Time = TimeOnly.Parse("0:00"),
                .IsValid = True})
        End If

        lineNumber = _lines.FindLineNumber(searchWord:="Insulin sensitivity")
        If lineNumber >= 0 Then
            line = Me.SafeLineAt(lineNumber - 1)
            _record.Bolus.InsulinSensitivity.Add(item:=New InsulinSensitivityRecord With {
                .Sensitivity = ParseSingle(s:=line),
                .Time = TimeOnly.Parse(s:="0:00"),
                .IsValid = True})
        End If

        lineNumber = _lines.FindLineNumber(searchWord:="Blood Glucose Target Range")
        If lineNumber >= 0 Then
            line = Me.SafeLineAt(lineNumber - 1)
            split = line.Split(separator:="-", Options)
            If split.Length >= 2 Then
                Dim low As String = split(0).Trim()
                Dim high As String = split(1).Trim()
                _record.Bolus.BloodGlucoseTarget.Add(item:=New BloodGlucoseTargetRecord With {
                    .Low = ParseSingle(s:=low),
                    .High = ParseSingle(s:=high),
                    .Time = TimeOnly.Parse(s:="0:00"),
                    .IsValid = True})
            End If
        End If
    End Sub

    Public Sub ParseSmartGuardAndReminders()
        Dim smartGuardIndex As Integer = _lines.FindLineNumber(searchWord:="SmartGuard™ settings")
        If smartGuardIndex < 0 Then Return
        Dim lineNumber As Integer = smartGuardIndex + 1
        Dim line As String = Me.SafeLineAt(index:=lineNumber + 1)
        Dim split As String() = SplitColumns(line)
        Dim smartGuard As String = If(split.Length > 1, split(1), String.Empty)

        line = Me.SafeLineAt(index:=lineNumber + 2)
        split = SplitColumns(line)
        Dim target As String = String.Empty
        If split.Length > 1 Then
            Dim targetField As String = split(1).Trim()
            Dim parts As String() =
                targetField.Split(separator:=" "c, Options)
            If parts.Length > 1 Then
                target = parts(1)
            ElseIf parts.Length = 1 Then
                target = parts(0)
            End If
        End If

        line = Me.SafeLineAt(index:=lineNumber + 3)
        split = SplitColumns(line)
        Dim autoCorrection As String =
            If(split.Length > 1,
               split(1),
               String.Empty)

        Dim sg As New SmartGuardRecord With {
            .SmartGuard = smartGuard,
            .Target = ParseSingle(s:=target),
            .AutoCorrection = autoCorrection}
        _record.SmartGuard = sg

        ' Reminders
        Dim searchWord As String = "Low reservoir"
        Dim extractWord As String = Me.GetValueForSearchWord(searchWord)
        Dim rmd As New RemindersRecord With {
            .LowReservoirWarning = "Insulin Units",
            .Amount = extractWord}

        searchWord = "Low pump battery"
        extractWord = Me.GetValueForSearchWord(searchWord)
        split = extractWord.Split(separator:="  ", Options)
        Dim onOff As String = If(split.Length > 0,
                                 split(0),
                                 String.Empty)
        Dim timeLeft As String = If(split.Length > 1,
                                    split(1),
                                    String.Empty)
        rmd.LowPumpBattery = New AlertRecord(onOff, timeLeft)
        _record.Reminders = rmd

        searchWord = "Low sensor life"
        lineNumber = _lines.FindLineNumber(searchWord)
        extractWord = Me.GetValueForSearchWord(searchWord)
        split = extractWord.Split(separator:="  ", Options)
        Dim sensorEndingRecord As New SensorEndingRecord() With {
            .LessThan24Hours = split(0)}
        Dim sensorRecord As New SensorRecord With {
            .SensorOn = "On",
            .SensorEnding = sensorEndingRecord}
        _record.Reminders.LowSensorLife = sensorRecord

        line = Me.SafeLineAt(index:=lineNumber + 1)
        split = SplitColumns(line)
        If split.Length = 5 Then
            _record.Reminders.LowSensorLife.SensorEnding.LessThan12State = split(3)
        End If

        line = Me.SafeLineAt(index:=lineNumber + 2)
        split = SplitColumns(line)
        If split.Length = 6 Then
            _record.Reminders.SetChange =
                New AlertRecord(onOff:=split(4), timeLeft:=split(5))
        End If
    End Sub

End Class
