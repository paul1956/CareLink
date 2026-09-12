' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports Spire.Pdf.Utilities

Public Module FlexPdfParser

    Private Const ComparisonType As StringComparison =
        StringComparison.OrdinalIgnoreCase

    Private Const Options As StringSplitOptions =
                    StringSplitOptions.RemoveEmptyEntries

    Public Sub ParseFlex(record As PdfSettingsRecord, pageText As String)
        Dim allLines As List(Of String) = pageText.SplitLines(Trim:=True)
        Try
            ' Get user name from the first line of the header text
            Dim firstLine As String = allLines.FirstOrDefault()
            Dim length As Integer = firstLine.IndexOf(value:="  ")
            If length > 0 Then
                record.UserName =
                    firstLine.Substring(startIndex:=0, length).Trim()
            End If

            '  Maximum Basal Rate is not always present,
            '  so skip if there are not at least 2 tables
            '  (header and searchWord)
            Dim searchWord As String = "Max Basal Rate"
            Dim result As String = allLines.FindLineContaining(searchWord)
            If result IsNot Nothing Then
                record.Basal.MaximumBasalRate =
                    result.GetSingleLineValue(Of Single)(key:=searchWord, endsWith:="U/Hr")
            End If

            searchWord = "Basal pattern settings"
            Dim lineNumber As Integer =
                FindLineIndexContaining(allLines, searchWord)
            Dim line As String
            Dim split As String()
            If lineNumber >= 0 Then
                Do
                    lineNumber += 1
                    line = allLines(index:=lineNumber)
                    If line.Length = 0 Then
                        Exit Do
                    End If
                    split = line.Split(separator:="  ", Options)
                    Dim isActive As Boolean =
                        line.Contains(value:="(active)", ComparisonType)
                    Dim named As New NamedBasalRecord With {
                        .Total24Hour = split(3),
                        .Active = isActive,
                        .BasalRates = New List(Of BasalRateRecord)()}
                    named.BasalRates.Add(
                        item:=New BasalRateRecord With {
                            .[Time] = TimeOnly.Parse(s:=split(2).Split(separator:=" - ")(0)),
                            .UnitsPerHr = ParseSingle(s:=split(3)),
                            .IsValid = True})
                Loop Until line.Length = 0
            End If

            Dim bw As New BolusWizardRecord()
            searchWord = "Active insulin time"
            line = allLines.FindLineContaining(searchWord)
            Dim extractWord As String
            If line = String.Empty Then
                bw.ActiveInsulinTime = 2
            Else
                extractWord =
                    line.ExtractBetween(startWord:=searchWord, endWord:="h").
                         Replace(oldValue:="(", newValue:="")
                bw.ActiveInsulinTime = If(extractWord.Contains(value:=":"c),
                                          AitLengths(key:=extractWord),
                                          ParseSingle(s:=extractWord))
            End If
            bw.BolusWizard = "On"

            searchWord = "Max bolus"
            line = allLines.FindLineContaining(searchWord)
            extractWord =
                line.ExtractBetween(startWord:=searchWord, endWord:="u")

            bw.MaximumBolus = ParseSingle(s:=extractWord)

            bw.Units = New DeviceUnitsRecord With {
                .CarbUnits = "grams",
                .BgUnits = If(allLines.Contains(value:="mg/dL"),
                              "mg/dL",
                              "mmol/L")}

            line = allLines.FindLineContaining(searchWord:="Bolus increments")

            split = line.Split(separator:="  ", Options)
            Dim bolusIncrement As Single = Single.NaN
            If split.Length = 4 Then
                Dim split3 As String = split(3).RemoveSuffix(suffix:=" U")
                bolusIncrement =
                    If(split3.Contains(value:="--"),
                       Single.NaN,
                       ParseSingle(s:=split3))
            End If

            Dim eb As New EasyBolusRecord()
            Dim dualSquareRecord As New DualSquareRecord With {
                .Dual = "Off",
                .Square = "Off"}
            record.Bolus.EasyBolus = New EasyBolusRecord With {
                .EasyBolus = "Off",
                .BolusSpeed = "10 U/Minute",
                .BolusIncrement = bolusIncrement,
                .DualSquare = dualSquareRecord}

            lineNumber = allLines.FindLineNumber(searchWord:="Carb ratio (g/U)")
            line = allLines(index:=lineNumber - 1).Trim()
            Dim carbRatio As String = line

            record.Bolus.DeviceCarbohydrateRatios.Add(item:=New DeviceCarbRatioRecord With {
                .Ratio = ParseSingle(s:=carbRatio),
                .Time = TimeOnly.Parse("0:00"),
                .IsValid = True})

            lineNumber = allLines.FindLineNumber(searchWord:="Insulin sensitivity")
            line = allLines(index:=lineNumber - 1).Trim()
            record.Bolus.InsulinSensitivity.Add(item:=New InsulinSensitivityRecord With {
                .Sensitivity = ParseSingle(s:=line),
                .Time = TimeOnly.Parse(s:="0:00"),
                .IsValid = True})

            lineNumber = allLines.FindLineNumber(searchWord:="Blood Glucose Target Range")
            line = allLines(index:=lineNumber - 1).Trim()
            split = line.Split(separator:="-", Options)
            Dim low As String = split(0).Trim()
            Dim high As String = split(1).Trim()
            record.Bolus.BloodGlucoseTarget.Add(item:=New BloodGlucoseTargetRecord With {
                .Low = ParseSingle(s:=low),
                .High = ParseSingle(s:=high),
                .Time = TimeOnly.Parse(s:="0:00"),
                .IsValid = True})

            lineNumber = allLines.FindLineNumber(searchWord:="SmartGuard™ settings") + 1
            line = allLines(index:=lineNumber).Trim()
            split = line.Split(separator:="  ", Options)
            Dim smartGuard As String = split(1)
            line = allLines(index:=lineNumber + 1).Trim()
            split = line.Split(separator:="  ", Options)
            Dim target As String = split(1).Trim.Split(separator:=" ")(1)
            line = allLines(index:=lineNumber + 2).Trim()
            split = line.Split(separator:="  ", Options)
            Dim sg As New SmartGuardRecord With {
                .SmartGuard = smartGuard,
                .Target = ParseSingle(s:=target),
                .AutoCorrection = split(1)}
            record.SmartGuard = sg

            searchWord = "Low reservoir"
            line = allLines.FindLineContaining(searchWord)
            split = line.Split(separator:="  ", Options)
            extractWord =
                line.ExtractBetween(startWord:=searchWord)
            Dim rmd As New RemindersRecord With {
                .LowReservoirWarning = "Insulin Units",
                .Amount = extractWord}

            searchWord = "Low pump battery"
            line = allLines.FindLineContaining(searchWord)
            split = line.Split(separator:="  ", Options)
            extractWord =
                line.ExtractBetween(startWord:=searchWord)
            split = extractWord.Split(separator:="  ", Options)
            record.Reminders.LowPumpBattery =
                New AlertRecord(onOff:=split(0), timeLeft:=split(1))
            record.Reminders = rmd

            searchWord = "Low sensor life"
            lineNumber = allLines.FindLineNumber(searchWord)
            line = allLines(index:=lineNumber).Trim()
            extractWord =
                line.ExtractBetween(startWord:=searchWord)
            split = extractWord.Split(separator:="  ", Options)
            Dim sensorEndingRecord As New SensorEndingRecord() With
                {.LessThan24Hours = split(0)}
            Dim sensorRecord As New SensorRecord With {
                .SensorOn = "On",
                .SensorEnding = sensorEndingRecord}
            record.Reminders.LowSensorLife = sensorRecord

            line = allLines(index:=lineNumber + 1).Trim()
            split = line.Split(separator:="  ", Options)
            If split.Length = 5 Then
                record.Reminders.LowSensorLife.SensorEnding.LessThan12State = split(3)
            End If

            searchWord = "Glucose settings - lows"
            lineNumber = allLines.FindLineNumber(searchWord)
            line = allLines(index:=lineNumber).Trim()
            split = line.Split(separator:="  ", Options)
            If split.Length < 3 Then
                Stop
            End If
            Dim nameNight As String = split(2)
            Dim nameDay As String = split(1)

            line = allLines(index:=lineNumber + 1).Trim()
            split = line.Split(separator:="  ", Options)
            Dim startTimeDay As String = split(1)
            Dim startTimeNight As String = split(2)

            line = allLines(index:=lineNumber + 2).Trim()
            split = line.Split(separator:="  ", Options)
            Dim lowLimitDay As String = split(1)
            Dim lowLimitNight As String = split(2)

            line = allLines(index:=lineNumber + 3).Trim()
            split = line.Split(separator:="  ", Options)
            Dim fallLimitDay As String = split(1)
            Dim fallLimitNight As String = split(2)

            line = allLines(index:=lineNumber + 4).Trim()
            split = line.Split(separator:="  ", Options)
            Dim alertOnLowDay As String = split(1)
            Dim alertOnLowNight As String = split(2)

            line = allLines(index:=lineNumber + 5).Trim()
            split = line.Split(separator:="  ", Options)
            Dim alertBeforeLowDay As String = split(1)
            Dim alertBeforeLowNight As String = split(2)

            line = allLines(index:=lineNumber + 6).Trim()
            split = line.Split(separator:="  ", Options)
            Dim maxVolumeAtNight As String = split(2)

            line = allLines(index:=lineNumber + 7).Trim()
            split = line.Split(separator:="  ", Options)
            Dim snoozeDurationDay As String = split(1)
            Dim snoozeDurationNight As String = split(2)

            line = allLines(index:=lineNumber + 8).Trim()
            split = line.Split(separator:="  ", Options)
            Dim suspendLimitDay As String = split(1)
            Dim suspendLimitNight As String = split(2)

            line = allLines(index:=lineNumber + 9).Trim()
            split = line.Split(separator:="  ", Options)
            Dim suspendBeforeLowDay As String = split(1)
            Dim suspendBeforeLowNight As String = split(2)

            line = allLines(index:=lineNumber + 10).Trim()
            split = line.Split(separator:="  ", Options)
            Dim alertForSuspendBeforeLowDay As String = split(1)
            Dim alertForSuspendBeforeLowNight As String = split(2)

            line = allLines(index:=lineNumber + 11).Trim()
            split = line.Split(separator:="  ", Options)
            Dim suspendOnLowDay As String = split(1)
            Dim suspendOnLowNight As String = split(2)

            Dim itemLow As New LowAlertRecord With {
                .Name = nameDay,
                .Start = startTimeDay,
                .LowLimit = lowLimitDay,
                .FallAlert = fallLimitDay,
                .AlertOnLow = alertOnLowDay,
                .AlertBeforeLow = alertBeforeLowDay,
                .MaxVolumeAtNight = "N/A",
                .SuspendLimit = suspendLimitDay,
                .SuspendBeforeLow = suspendBeforeLowDay,
                .AlertForSuspendBeforeLow = alertForSuspendBeforeLowDay,
                .SuspendOnLow = suspendOnLowDay,
            .IsValid = True}
            record.LowAlerts.LowAlert.Add(item:=itemLow)

            itemLow = New LowAlertRecord With {
                .Name = nameNight,
                .Start = startTimeNight,
                .LowLimit = lowLimitNight,
                .FallAlert = fallLimitNight,
                .AlertOnLow = alertOnLowNight,
                .AlertBeforeLow = alertBeforeLowNight,
                .MaxVolumeAtNight = maxVolumeAtNight,
                .SuspendLimit = suspendLimitNight,
                .SuspendBeforeLow = suspendBeforeLowNight,
                .AlertForSuspendBeforeLow = alertForSuspendBeforeLowNight,
                .SuspendOnLow = suspendOnLowNight,
               .IsValid = True}
            record.LowAlerts.LowAlert.Add(item:=itemLow)

            searchWord = "Glucose settings - highs"
            lineNumber = allLines.FindLineNumber(searchWord)
            line = allLines(index:=lineNumber).Trim()
            split = line.Split(separator:="  ", Options)
            If split.Length < 3 Then
                Stop
            End If
            nameDay = split(1)
            nameNight = split(2)

            line = allLines(index:=lineNumber + 1).Trim()
            split = line.Split(separator:="  ", Options)
            startTimeDay = split(1)
            startTimeNight = split(2)

            line = allLines(index:=lineNumber + 2).Trim()
            split = line.Split(separator:="  ", Options)
            Dim highLimitDay As String = split(1)
            Dim highLimitNight As String = split(2)

            line = allLines(index:=lineNumber + 3).Trim()
            split = line.Split(separator:="  ", Options)
            Dim riseAlertDay As String = split(1)
            Dim riseAlertNight As String = split(2)

            line = allLines(index:=lineNumber + 4).Trim()
            split = line.Split(separator:="  ", Options)
            Dim alertOnHighDay As String = split(1)
            Dim alertOnHighNight As String = split(2)

            line = allLines(index:=lineNumber + 5).Trim()
            split = line.Split(separator:="  ", Options)
            Dim alertBeforeHighDay As String = split(1)
            Dim alertBeforeHighNight As String = split(2)

            line = allLines(index:=lineNumber + 6).Trim()
            split = line.Split(separator:="  ", Options)
            maxVolumeAtNight = split(2)

            line = allLines(index:=lineNumber + 7).Trim()
            split = line.Split(separator:="  ", Options)
            snoozeDurationDay = split(1)
            snoozeDurationNight = split(2)

            Dim itemHigh As New HighAlertRecord With {
                .Name = nameDay,
                .Start = startTimeDay.ToString(),
                .HighLimit = highLimitDay, '3
                .AlertBeforeHigh = alertBeforeHighDay,
                .TimeBeforeHigh = "",
                .AlertOnHigh = alertOnHighDay,
                .RiseAlert = riseAlertDay,
                .MaxVolumeAtNight = "N/A",
                .SnoozeTime = snoozeDurationDay,
                .IsValid = True}
            record.HighAlerts.HighAlert.Add(item:=itemHigh)

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
            record.HighAlerts.HighAlert.Add(item:=itemHigh)

            searchWord = "Alert volume and mute"
            lineNumber = allLines.FindLineNumber(searchWord)
            line = allLines(index:=lineNumber + 1).Trim()
            extractWord = line.ExtractBetween(startWord:="Pump sound")
            record.Utilities.PumpSounds = extractWord

            line = allLines(index:=lineNumber + 2).Trim()
            extractWord = line.ExtractBetween(startWord:="Pump vibration")
            record.Utilities.PumpVibrations = extractWord

            line = allLines(index:=lineNumber + 3).Trim()
            extractWord = line.ExtractBetween(startWord:="Lost communication")
            record.Utilities.LostCommunication = extractWord
            SetIsValid(record, value:=True)
        Catch ex As Exception
            SetIsValid(record, value:=False)
        End Try
    End Sub

End Module
