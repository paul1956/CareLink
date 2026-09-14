' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DeliverySettings

    <Extension>
    Friend Sub DeliverySettings1BolusWizardSetup(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(leftPanel:=True,
                title:="Bolus Wizard:",
                value:=$"{pdf.Bolus.BolusWizard.BolusWizard}")
            .AppendNewLine

            Dim count As Integer = pdf.Bolus.DeviceCarbohydrateRatios.Count
            Dim text As String =
                count.ToUnits(unit:=$"{Indent4}Carbohydrate Ratio",
                              suffix:=":",
                              includeValue:=False)

            .AppendTextNewFont(text,
                               newFont:=FixedWidthBoldFont,
                               includeNewLine:=True)

            Dim timeFormat As String = pdf.Utilities.TimeFormat
            For Each item As CarbRatioRecord In pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                .AppendTimeValueRow(startTime:=item.StartTime.ToString(),
                                    endTime:=item.EndTime.ToString(),
                                    value:=$"{item.CarbRatio} g/U")
            Next
            .AppendNewLine

            Dim timeUnits As String = pdf.Bolus.BolusWizard.ActiveInsulinTime.ToHoursMinutes()
            With rtb
                .AppendKeyValue(leftPanel:=True,
                    title:="Active Insulin Time:",
                    value:=$"{timeUnits} hr")
                .AppendNewLine
            End With

            count = pdf.Bolus.InsulinSensitivity.Count
            text = count.ToUnits(unit:=$"{Indent4}Insulin Sensitivity Factor", suffix:=":", includeValue:=False)
            .AppendTextNewFont(text, newFont:=FixedWidthBoldFont, includeNewLine:=True)
            For Each e As IndexClass(Of InsulinSensitivityRecord) In pdf.Bolus.InsulinSensitivity.WithIndex
                Dim item As InsulinSensitivityRecord = e.Value
                If Not item.IsValid Then
                    Exit For
                End If

                Dim startTime As TimeOnly = item.Time
                Dim endTime As TimeOnly =
                    If(e.IsLast,
                       Midnight,
                       pdf.Bolus.InsulinSensitivity(index:=e.Index + 1).Time)

                Dim sensitivity As String =
                    If(item.Sensitivity < 0.01,
                       "0",
                       item.Sensitivity.RoundTo025.ToString(format:="F1"))

                Dim value As String =
                    $"{sensitivity} {pdf.Bolus.BolusWizard.Units.CarbUnits}/U"
                .AppendTimeValueRow(startTime:=startTime.ToString,
                    endTime:=endTime.ToString,
                    value:=value,
                    indent:=pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine

            count = pdf.Bolus.BloodGlucoseTarget.Count
            text = count.ToUnits(unit:=$"{Indent4}Blood Glucose Target", suffix:=":", includeValue:=False)
            .AppendTextNewFont(text, newFont:=FixedWidthBoldFont, includeNewLine:=True)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In pdf.Bolus.BloodGlucoseTarget.WithIndex
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
                .AppendTimeValueRow(startTime,
                                    endTime,
                                    value,
                                    indent:=pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings2BasalPatternSetup(rtb As RichTextBox,
                                                  pdf As PdfSettingsRecord)

        With rtb
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In pdf.Basal.NamedBasal
                If Not item.Value.IsValid Then
                    Continue For
                End If
                .AppendTextNewFont(text:=$"{Indent4}{item.Key}:", newFont:=FixedWidthBoldFont, includeNewLine:=True)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.BasalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        Exit For
                    End If
                    Dim startTime As TimeOnly = basalRate.Time
                    Dim endTime As TimeOnly = If(e.IsLast,
                                                 Eleven59,
                                                 item.Value.BasalRates(index:=e.Index + 1).Time)

                    Dim value As String = $"{basalRate.UnitsPerHr:F3} U/hr"
                    .AppendTimeValueRow(startTime:=startTime.ToString(),
                        endTime:=endTime.ToString(),
                        value:=value,
                        indent:=pdf.Utilities.TimeFormat)
                Next
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings3MaxBasalBolus(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(leftPanel:=True, title:="Max Basal:", value:=$"{pdf.Basal.MaximumBasalRate} U/hr")
            .AppendKeyValue(leftPanel:=True, title:="Max Bolus:", value:=$"{pdf.Bolus.BolusWizard.MaximumBolus} U")
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings4DualSquareWave(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(leftPanel:=True, title:="Dual:", value:=$"{pdf.Bolus.EasyBolus.DualSquare.Dual,3}")
            .AppendKeyValue(leftPanel:=True, title:="Square:", value:=$"{pdf.Bolus.EasyBolus.DualSquare.Square,3}")
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings5BolusIncrement(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim bolusIncrement As Single = pdf.Bolus.EasyBolus.BolusIncrement
            Dim bolusIncrementText As String = If(Single.IsNaN(bolusIncrement),
                                                  "---",
                                                  $"{bolusIncrement:F1} U")
            .AppendKeyValue(leftPanel:=True,
                title:="Increment:",
                value:=bolusIncrementText)
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings6BolusSpeed(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            .AppendKeyValue(leftPanel:=True, title:="Bolus Speed:", value:=$"{pdf.Bolus.EasyBolus.BolusSpeed}")
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings7PresetBolusSetup(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim firstFound As Boolean = False
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In pdf.PresetBolus
                If item.Value.IsValid Then
                    If Not firstFound Then
                        .AppendTextWithSymbol(text:=$"Menu>{Gear}>Delivery Settings > Preset Bolus Setup")
                        firstFound = True
                    End If
                    .AppendTextNewFont(text:=$"{Indent4}{item.Key}:", newFont:=FixedWidthBoldFont)
                Else
                    Continue For
                End If
                Dim presetBolus As PresetBolusRecord = item.Value
                Dim bolusType As String = If(presetBolus.BolusTypeNormal,
                                             "Normal",
                                             "Square")
                Dim text As String = $"{Indent4}Bolus: {presetBolus.Bolus}{Indent4}Type: {bolusType}"
                .AppendTextNewFont(text, newFont:=FixedWidthFont)
                If Not item.Value.BolusTypeNormal Then
                    text = $"{Indent4}Duration: {presetBolus.Duration} hr"
                    .AppendTextNewFont(text, newFont:=FixedWidthFont)
                End If
                .AppendNewLine
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub DeliverySettings8PresetTempSetup(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim firstFound As Boolean = False
            For Each item As KeyValuePair(Of String, PresetTempRecord) In pdf.PresetTemp
                If item.Value.IsValid Then
                    If Not firstFound Then
                        .AppendTextWithSymbol(text:=$"Menu>{Gear}>Delivery Settings > Preset Temp Setup")
                        firstFound = True
                    End If
                    .AppendTextNewFont(text:=$"{Indent4}{item.Key}:", newFont:=FixedWidthBoldFont)
                Else
                    Continue For
                End If
                .AppendTextNewFont(text:=$"{Indent4}{item.Key}:", newFont:=FixedWidthBoldFont)
                Dim presetTempRecord As PresetTempRecord = item.Value
                If presetTempRecord.IsValid Then
                    Dim text As String = $"{Indent4}{Indent4}{presetTempRecord.PresetAmount}"
                    .AppendTextNewFont(text, newFont:=FixedWidthFont)
                    Dim duration As TimeSpan = presetTempRecord.Duration
                    Dim durationText As String = duration.ToFormattedTimeSpan(unit:="U/hr")
                    text = $"{Indent4}Duration:{Indent4}{durationText.Trim}"
                    .AppendTextNewFont(text, newFont:=FixedWidthFont, includeNewLine:=True)
                Else
                    .AppendNewLine
                End If
            Next
            .AppendNewLine
        End With
    End Sub

End Module
