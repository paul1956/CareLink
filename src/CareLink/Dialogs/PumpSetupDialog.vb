' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PumpSetupDialog

    ''' <summary>
    '''  Backing field for the <see cref="Pdf"/> property.
    ''' </summary>
    Private _pdf As PdfSettingsRecord

    ''' <summary>
    '''  Sets the PDF settings record used to populate the dialog.
    ''' </summary>
    ''' <value>The <see cref="PdfSettingsRecord"/> containing pump configuration data.</value>
    Public WriteOnly Property Pdf As PdfSettingsRecord
        Set
            _pdf = Value
        End Set
    End Property

    ''' <summary>
    '''  Returns a string representation of a <see cref="TimeOnly"/> value, padded to a standard width if necessary.
    ''' </summary>
    ''' <param name="tOnly">The <see cref="TimeOnly"/> value to format.</param>
    ''' <returns>A string representation of the time, padded to a standard width.</returns>
    Private Shared Function StandardTimeOnlyWidth(tOnly As TimeOnly) As String
        Dim tAsString As String = tOnly.ToString
        If tAsString.Length < 7 Then Return tAsString
        Return tAsString.PadLeft(9)
    End Function

    ''' <summary>
    '''  Handles the <see cref="DataGridView.Paint"/> event for high and low alert DataGridViews.
    '''  Paints a message if no records are found.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
    Private Sub DataGridView_Paint(sender As Object, e As PaintEventArgs) Handles DataGridViewHighAlert.Paint, DataGridViewLowAlert.Paint
        DgvPaintNoRecordsFound(sender, e)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.SelectionChanged"/> event for high and low alert DataGridViews.
    '''  Clears the selection to prevent user selection.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub DataGridViewHighAlert_SelectionChanged(sender As Object, e As EventArgs) Handles _
        DataGridViewHighAlert.SelectionChanged, DataGridViewLowAlert.SelectionChanged

        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.ClearSelection()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="OK_Button"/> click event.
    '''  Sets the dialog result to OK and closes the dialog.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ''' <summary>
    '''  Handles the dialog's <see cref="Form.Shown"/> event.
    '''  Populates all UI controls with data from the <see cref="_pdf"/> settings record.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e1">The <see cref="EventArgs"/> instance containing the event data.</param>
    ''' <exception cref="NullReferenceException">Thrown if <see cref="_pdf"/> is not set.</exception>
    Private Sub PumpSetupDialog_Shown(sender As Object, e1 As EventArgs) Handles MyBase.Shown
        If _pdf Is Nothing Then
            Throw New NullReferenceException(NameOf(_pdf))
        End If


        Dim defaultBoldFont As New Font(family:=Me.RtbMainLeft.Font.FontFamily, emSize:=14, style:=FontStyle.Bold)
        Dim defaultFont As New Font(family:=Me.RtbMainLeft.Font.FontFamily, emSize:=14, style:=FontStyle.Regular)
        Dim headingBoldFont As New Font(familyName:="Tahoma", emSize:=18, style:=FontStyle.Bold)
        Dim subheadingBoldtFont As New Font(familyName:="Tahoma", emSize:=16, style:=FontStyle.Bold)
        Dim subheadingFont As New Font(familyName:="Tahoma", emSize:=16, style:=FontStyle.Regular)
        Me.RtbMainLeft.Clear()
        Me.RtbMainRight.Clear()


        Const gear As Char = ChrW(&H2699)
        Const tab As String = vbTab
        Dim bolusWizard As BolusWizardRecord = _pdf.Bolus.BolusWizard
        Dim endTime As String
        Dim startTime As String

        With Me.RtbMainLeft
            .ReadOnly = False

            .AppendLine(text:=$"Delivery Settings", newFont:=headingBoldFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Bolus Wizard Setup",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Bolus Wizard: {bolusWizard.BolusWizard}", newFont:=defaultFont)
            .AppendNewLine

            Dim optionalS As String = If(_pdf.Bolus.DeviceCarbohydrateRatios.Count > 1, "s", "")
            .AppendLine(text:=$"Carb Ratio{optionalS}:", newFont:=defaultBoldFont)
            Dim text As String
            For Each item As CarbRatioRecord In _pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                text = $"{tab}{StandardTimeOnlyWidth(tOnly:=item.StartTime)}"
                .AppendLine($"{tab}{StandardTimeOnlyWidth(tOnly:=item.StartTime)}", newFont:=defaultFont)

                text = $"{tab}-{tab}{StandardTimeOnlyWidth(tOnly:=item.EndTime)}{tab}{item.CarbRatio} g/U"
                .AppendLine(text, newFont:=defaultFont)
            Next
            .AppendNewLine

            optionalS = If(_pdf.Bolus.InsulinSensitivity.Count > 1, "s", "")
            .AppendLine(text:=$"Insulin Sensitivity Factor{optionalS}:", newFont:=defaultBoldFont)
            For Each e As IndexClass(Of InsulinSensitivityRecord) In _pdf.Bolus.InsulinSensitivity.WithIndex
                Dim item As InsulinSensitivityRecord = e.Value
                If Not item.IsValid Then
                    .AppendNewLine
                    Exit For
                End If

                endTime = If(e.IsLast,
                             MidnightStr,
                             StandardTimeOnlyWidth(tOnly:=_pdf.Bolus.InsulinSensitivity(e.Index + 1).Time))
                startTime = StandardTimeOnlyWidth(tOnly:=item.Time)
                Dim sensitivity As String = If(item.Sensitivity < 0.01,
                    "0.00",
                    item.Sensitivity.RoundTo025.ToString("F2"))
                .AppendLine(
                    text:=$"{tab}{startTime}{tab}-{tab}{endTime}{tab}{sensitivity} {bolusWizard.Units.CarbUnits}/U",
                    newFont:=defaultFont)
            Next
            .AppendNewLine

            optionalS = If(_pdf.Bolus.BloodGlucoseTarget.Count > 1,
                           "s",
                           "")
            .AppendLine(text:=$"BG Target{optionalS}:", newFont:=defaultBoldFont)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In _pdf.Bolus.BloodGlucoseTarget.WithIndex
                Dim item As BloodGlucoseTargetRecord = e.Value
                If Not item.IsValid Then
                    .AppendNewLine
                    Exit For
                End If

                endTime = If(e.IsLast,
                             Eleven59Str,
                             StandardTimeOnlyWidth(tOnly:=_pdf.Bolus.BloodGlucoseTarget(e.Index + 1).Time))
                startTime = StandardTimeOnlyWidth(tOnly:=item.Time)
                .AppendLine(
                    text:=$"{tab}{startTime}{tab}-{tab}{endTime}{tab}{item.Low}-{item.High} {bolusWizard.Units.BgUnits}",
                    newFont:=defaultFont)
            Next
            .AppendNewLine

            .AppendLine(text:=$"Active Insulin Time: {bolusWizard.ActiveInsulinTime} hr", newFont:=defaultBoldFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Basal Pattern(s) Setup",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In _pdf.Basal.NamedBasal
                .AppendLine(text:=$"{item.Key}:", newFont:=defaultFont)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.basalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        .AppendNewLine
                        Exit For
                    End If
                    startTime = StandardTimeOnlyWidth(tOnly:=basalRate.Time)
                    endTime = If(e.IsLast,
                                 Eleven59Str,
                                 StandardTimeOnlyWidth(tOnly:=item.Value.basalRates(e.Index + 1).Time))
                    .AppendLine(text:=$"{tab}{startTime}{tab}-{tab}{endTime,9}{tab}{basalRate.UnitsPerHr:F3} U/hr",
                                newFont:=defaultFont)
                Next
                .AppendNewLine
            Next
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Preset Temp Setup",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)

            For Each item As KeyValuePair(Of String, PresetTempRecord) In _pdf.PresetTemp
                .AppendLine(text:=$"{item.Key}:", newFont:=defaultFont)
                Dim presetTempRecord As PresetTempRecord = item.Value

                If presetTempRecord.IsValid Then
                    .AppendLine(text:=$"{tab}{presetTempRecord.PresetAmount}", newFont:=defaultFont)
                    .AppendLine(
                        text:=$"{tab}Duration:{tab}{presetTempRecord.Duration.ToFormattedTimeSpan("U/hr").Trim}",
                        newFont:=defaultFont)
                Else
                    .AppendNewLine
                End If
            Next
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Dual/Square Wave",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Dual:  {tab}{_pdf.Bolus.EasyBolus.DualSquare.Dual,2}", newFont:=defaultFont)
            .AppendLine(text:=$"{tab}Square:{tab}{_pdf.Bolus.EasyBolus.DualSquare.Square,2}", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Preset Bolus Setup",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In _pdf.PresetBolus
                .AppendLine(text:=item.Key, newFont:=defaultFont)
                If item.Value.IsValid Then
                    Dim presetBolus As PresetBolusRecord = item.Value
                    Dim bolusType As String = If(presetBolus.BolusTypeNormal, "Normal", "Square")
                    .AppendLine(
                        text:=$"{tab}Bolus: {presetBolus.Bolus}{tab}Type: {bolusType}",
                        newFont:=defaultFont)
                    If Not item.Value.BolusTypeNormal Then
                        .AppendLine(
                            text:=$"{tab}Duration: {presetBolus.Duration} hr",
                            newFont:=defaultFont)
                    End If
                End If
                .AppendNewLine
            Next
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbMainRight
            .AppendLine(text:=$"Delivery Settings, Device Settings, SmartGuard", newFont:=headingBoldFont)
            .AppendNewLine
            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Bolus Increment",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Bolus Increment: {_pdf.Bolus.EasyBolus.BolusIncrement:F3}", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Max Basal/Bolus",
                newFont:=subheadingFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Max Basal:  {_pdf.Basal.MaximumBasalRate} U/hr", newFont:=defaultFont)
            .AppendLine(text:=$"{tab}Max Bolus: {bolusWizard.MaximumBolus} U", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Bolus Speed",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Bolus Speed: {_pdf.Bolus.EasyBolus.BolusSpeed}", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {ChrW(CharCode:=&H2699)} > Delivery Settings > Easy Bolus",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Easy Bolus: {_pdf.Bolus.EasyBolus.EasyBolus}", newFont:=defaultFont)
            .AppendLine(text:=$"{tab}Step Size:   {_pdf.Bolus.EasyBolus.BolusIncrement} U", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {ChrW(CharCode:=&H2699)} > Delivery Settings > Auto Suspend",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Alarm: {_pdf.Utilities.AutoSuspend.Alarm}", newFont:=defaultFont)
            If _pdf.Utilities.AutoSuspend.Alarm <> "Off" Then
                .AppendLine(
                    text:=$"Time: {_pdf.Utilities.AutoSuspend.Time.ToFormattedTimeSpan("hr")}",
                    newFont:=defaultFont)
            End If
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {gear} > Delivery Settings > Sensor",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Sensor: {_pdf.Sensor.SensorOn}", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {ChrW(CharCode:=&H2699)} > Delivery Settings > Display Options",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Brightness: {_pdf.Utilities.Brightness}", newFont:=defaultFont)
            Dim backlightTimeout As String = _pdf.Utilities.BackLightTimeout.ToFormattedTimeSpan("min")
            .AppendLine(
                text:=$"{tab}Backlight: {backlightTimeout.TrimStart("0"c)}",
                newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {ChrW(CharCode:=&H2699)}",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}SmartGuard: {_pdf.SmartGuard.SmartGuard}", newFont:=defaultFont)
            .AppendNewLine

            .AppendLine(
                text:=$"Menu > {ChrW(CharCode:=&H2699)} > Delivery Settings > SmartGuard Settings",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont)
            .AppendLine(text:=$"{tab}Target: {_pdf.SmartGuard.Target}", newFont:=defaultFont)
            .AppendLine(text:=$"{tab}Auto Correction: {_pdf.SmartGuard.AutoCorrection}", newFont:=defaultFont)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbHighAlertMenu
            .Text = ""
            .AppendLine(
                text:=$"Menu > {ChrW(CharCode:=&H2699)} > Alert Settings > High Alert",
                newFont:=defaultBoldFont,
                highlightText:=ChrW(CharCode:=&H2699),
                highlightFont:=subheadingBoldtFont,
                 includeNewLine:=False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.DataGridViewHighAlert
            .ColumnHeadersDefaultCellStyle.Font = New Font(prototype:= .Font, newStyle:=FontStyle.Bold)
            .Rows.Clear()
            For Each c As DataGridViewColumn In .Columns
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            For Each h As HighAlertRecord In _pdf.HighAlerts.HighAlert
                .Rows.Add(
                    h.Start,
                    h.End,
                    $"{h.HighLimit} {h.ValueUnits}",
                    h.AlertBeforeHigh,
                    h.TimeBeforeHigh,
                    h.AlertOnHigh,
                    h.RiseAlert,
                    h.RaiseLimit)
                .Columns(NameOf(ColumnTimeBeforeHighText)).Visible = h.AlertBeforeHigh
            Next
        End With

        With Me.RtbLowAlertMenu
            .Text = ""
            .AppendLine(
                text:=$"Menu > {gear} > Alert Settings > Low Alert",
                newFont:=defaultBoldFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont,
                includeNewLine:=False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.DataGridViewLowAlert
            .Rows.Clear()
            .ColumnHeadersDefaultCellStyle.Font = New Font(prototype:= .Font, newStyle:=FontStyle.Bold)
            For Each c As DataGridViewColumn In .Columns
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            Next
            For Each l As LowAlertRecord In _pdf.LowAlerts.LowAlert
                .Rows.Add(
                    l.Start,
                    l.End,
                    $"{l.LowLimit} {l.ValueUnits}",
                    l.Suspend,
                    l.AlertOnLow,
                    l.AlertBeforeLow,
                    l.ResumeBasalAlert)
                .Columns("ColumnResumeBasalAlert").Visible = String.IsNullOrWhiteSpace(l.Suspend)
            Next

        End With

        With Me.RtbHighSnoozeMenu
            .Text = ""
            .AppendLine(
                text:=$"Menu > {gear} > Snooze Menu > Snooze High & Low > High Snooze: {_pdf.HighAlerts}",
                newFont:=defaultBoldFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont,
                includeNewLine:=False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbLowSnoozeMenu
            .Text = ""
            .AppendLine(
                text:=$"Menu > {gear} > Snooze Menu > Snooze High & Low > Low Snooze: {_pdf.LowAlerts}",
                newFont:=defaultBoldFont,
                highlightText:=gear,
                highlightFont:=subheadingBoldtFont,
                includeNewLine:=False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

    End Sub

End Class
