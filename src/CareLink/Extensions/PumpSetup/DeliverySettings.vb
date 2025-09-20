' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DeliverySettings

    <Extension>
    Friend Sub DeliverySettings1BolusWizardSetup(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(
                key:="Bolus Wizard:",
                value:=$"{pdf.Bolus.BolusWizard.BolusWizard}")
            .AppendNewLine

            .AppendTextWithFontChange(
            text:=$"{pdf.Bolus.DeviceCarbohydrateRatios.Count.ToUnits(
                unit:=$"{Indent4}Carbohydrate Ratio",
                suffix:=":",
                includeValue:=False)}",
            newFont:=FixedWidthBoldFont,
            includeNewLine:=True)

            For Each item As CarbRatioRecord In
            pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList

                .AppendTimeValueRow(
                item.StartTime,
                item.EndTime,
                value:=$"{item.CarbRatio} g/U",
                pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine

            Dim timeUnits As String = pdf.Bolus.BolusWizard.ActiveInsulinTime.ToHoursMinutes()
            With rtb
                .AppendKeyValue(key:="Active Insulin Time:", value:=$"{timeUnits} hr")
                .AppendNewLine
            End With

            .AppendTextWithFontChange(
           text:=pdf.Bolus.InsulinSensitivity.Count.ToUnits(
                unit:=$"{Indent4}Insulin Sensitivity Factor",
                suffix:=":",
                includeValue:=False),
            newFont:=FixedWidthBoldFont,
            includeNewLine:=True)
            For Each e As IndexClass(Of InsulinSensitivityRecord) In
            pdf.Bolus.InsulinSensitivity.WithIndex

                Dim item As InsulinSensitivityRecord = e.Value
                If Not item.IsValid Then
                    Exit For
                End If

                Dim startTime As TimeOnly = item.Time
                Dim endTime As TimeOnly = If(e.IsLast,
                                             Midnight,
                                             pdf.Bolus.InsulinSensitivity(index:=e.Index + 1).Time)

                Dim sensitivity As String = If(item.Sensitivity < 0.01,
                                               "0.00",
                                               item.Sensitivity.RoundTo025.ToString(format:="F3"))

                Dim value As String = $"{sensitivity} {pdf.Bolus.BolusWizard.Units.CarbUnits}/U"
                .AppendTimeValueRow(startTime, endTime, value, pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine

            .AppendTextWithFontChange(
           text:=pdf.Bolus.BloodGlucoseTarget.Count.ToUnits(
                unit:=$"{Indent4}Blood Glucose Target",
                suffix:=":",
                includeValue:=False),
            newFont:=FixedWidthBoldFont,
            includeNewLine:=True)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In
            pdf.Bolus.BloodGlucoseTarget.WithIndex

                Dim item As BloodGlucoseTargetRecord = e.Value
                If Not item.IsValid Then
                    .AppendNewLine
                    Exit For
                End If

                Dim startTime As TimeOnly = item.Time
                Dim endTime As TimeOnly = If(e.IsLast,
                                             Eleven59,
                                             pdf.Bolus.BloodGlucoseTarget(index:=e.Index + 1).Time)

                Dim value As String = $"{item.Low}-{item.High} {pdf.Bolus.BolusWizard.Units.BgUnits}"
                .AppendTimeValueRow(startTime, endTime, value, pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings2BasalPatternSetup(
        rtb As RichTextBox, pdf As PdfSettingsRecord)

        With rtb
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In
                pdf.Basal.NamedBasal

                .AppendTextWithFontChange(
                    text:=$"{Indent4}{item.Key}:",
                    newFont:=FixedWidthBoldFont,
                    includeNewLine:=True)
                For Each e As IndexClass(Of BasalRateRecord) In
                    item.Value.basalRates.WithIndex

                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        Exit For
                    End If
                    Dim startTime As TimeOnly = basalRate.Time
                    Dim endTime As TimeOnly = If(e.IsLast,
                                                 Eleven59,
                                                 item.Value.basalRates(index:=e.Index + 1).Time)

                    Dim value As String = $"{basalRate.UnitsPerHr:F3} U/hr"
                    .AppendTimeValueRow(
                        startTime,
                        endTime,
                        value,
                        pdf.Utilities.TimeFormat)
                Next
            Next
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings3MaxBasalBolus(
        rtb As RichTextBox,
        pdf As PdfSettingsRecord)

        With rtb
            .AppendKeyValue(
                key:="Max Basal:",
                value:=$"{pdf.Basal.MaximumBasalRate:2} U/hr")

            .AppendKeyValue(
                key:="Max Bolus:",
                value:=$"{pdf.Bolus.BolusWizard.MaximumBolus:2} U")
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings4DualSquareWave(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(
                key:="Dual:",
                value:=$"{pdf.Bolus.EasyBolus.DualSquare.Dual,3}")

            .AppendKeyValue(
                key:="Square:",
                value:=$"{pdf.Bolus.EasyBolus.DualSquare.Square,3}")
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings5BolusIncrement(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(
                key:="Increment:",
                value:=$"{pdf.Bolus.EasyBolus.BolusIncrement:F3} U")
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings6BolusSpeed(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(
                key:="Bolus Speed:",
                value:=$"{pdf.Bolus.EasyBolus.BolusSpeed}")
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings7PresetBolusSetup(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In
                pdf.PresetBolus

                .AppendTextWithFontChange(
                    text:=$"{Indent4}{item.Key}:",
                    newFont:=FixedWidthBoldFont)

                If item.Value.IsValid Then
                    Dim presetBolus As PresetBolusRecord = item.Value
                    Dim bolusType As String = If(presetBolus.BolusTypeNormal,
                                                 "Normal",
                                                 "Square")
                    Dim text As String =
                        $"{Indent4}Bolus: {presetBolus.Bolus}{Indent4}Type: {bolusType}"
                    .AppendTextWithFontChange(text, newFont:=FixedWidthFont)
                    If Not item.Value.BolusTypeNormal Then
                        .AppendTextWithFontChange(
                            text:=$"{Indent4}Duration: {presetBolus.Duration} hr",
                            newFont:=FixedWidthFont)
                    End If
                End If
                .AppendNewLine
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings8PresetTempSetup(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            For Each item As KeyValuePair(Of String, PresetTempRecord) In pdf.PresetTemp
                .AppendTextWithFontChange(
                    text:=$"{Indent4}{item.Key}:",
                    newFont:=FixedWidthBoldFont)

                Dim presetTempRecord As PresetTempRecord = item.Value
                If presetTempRecord.IsValid Then
                    .AppendTextWithFontChange(
                        text:=$"{Indent4}{Indent4}{presetTempRecord.PresetAmount}",
                        newFont:=FixedWidthFont)
                    Dim duration As TimeSpan = presetTempRecord.Duration
                    Dim durationText As String = duration.ToFormattedTimeSpan(unit:="U/hr")
                    .AppendTextWithFontChange(
                        text:=$"{Indent4}Duration:{Indent4}{durationText.Trim}",
                        newFont:=FixedWidthFont,
                        includeNewLine:=True)
                Else
                    .AppendNewLine
                End If
            Next
            .AppendNewLine
        End With
    End Sub

End Module
