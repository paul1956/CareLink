﻿' Licensed to the .NET Foundation under one or more agreements.
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
    Private Sub DataGridViewHighAlert_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewHighAlert.SelectionChanged, DataGridViewLowAlert.SelectionChanged
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
        With Me.RtbMainLeft
            .ReadOnly = False

            .AppendLine($"Delivery Settings", headingBoldFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Wizard Setup", subheadingFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Bolus Wizard: {_pdf.Bolus.BolusWizard.BolusWizard}", defaultFont)
            .AppendNewLine

            Dim optionalS As String = If(_pdf.Bolus.DeviceCarbohydrateRatios.Count > 1, "s", "")
            .AppendLine($"Carb Ratio{optionalS}:", defaultBoldFont)
            For Each item As CarbRatioRecord In _pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                .AppendLine($"{vbTab}{StandardTimeOnlyWidth(item.StartTime)}{vbTab}-{vbTab}{StandardTimeOnlyWidth(item.EndTime)}{vbTab}{item.CarbRatio} g/U", defaultFont)
            Next
            .AppendNewLine

            optionalS = If(_pdf.Bolus.InsulinSensitivity.Count > 1, "s", "")
            .AppendLine($"Insulin Sensitivity Factor{optionalS}:", defaultBoldFont)
            For Each e As IndexClass(Of InsulinSensitivityRecord) In _pdf.Bolus.InsulinSensitivity.WithIndex
                Dim item As InsulinSensitivityRecord = e.Value
                If Not item.IsValid Then
                    .AppendNewLine
                    Exit For
                End If
                Dim endTime As String = If(e.IsLast, MidnightStr, StandardTimeOnlyWidth(_pdf.Bolus.InsulinSensitivity(e.Index + 1).Time))
                .AppendLine($"{vbTab}{StandardTimeOnlyWidth(item.Time)}{vbTab}-{vbTab}{endTime}{vbTab}{item.Sensitivity.RoundTo025:F1} {_pdf.Bolus.BolusWizard.Units.CarbUnits}/U", defaultFont)
            Next
            .AppendNewLine

            optionalS = If(_pdf.Bolus.BloodGlucoseTarget.Count > 1, "s", "")
            .AppendLine($"BG Target{optionalS}:", defaultBoldFont)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In _pdf.Bolus.BloodGlucoseTarget.WithIndex
                Dim item As BloodGlucoseTargetRecord = e.Value
                If Not item.IsValid Then
                    .AppendNewLine
                    Exit For
                End If
                Dim endTime As String = If(e.IsLast, Eleven59Str, StandardTimeOnlyWidth(_pdf.Bolus.BloodGlucoseTarget(e.Index + 1).Time))
                .AppendLine($"{vbTab}{StandardTimeOnlyWidth(item.Time)}{vbTab}-{vbTab}{endTime}{vbTab}{item.Low}-{item.High} {_pdf.Bolus.BolusWizard.Units.BgUnits}", defaultFont)
            Next
            .AppendNewLine

            .AppendLine($"Active Insulin Time: {_pdf.Bolus.BolusWizard.ActiveInsulinTime} hr", defaultBoldFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Basal Pattern(s) Setup", subheadingFont, ChrW(&H2699), subheadingBoldtFont)
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In _pdf.Basal.NamedBasal
                .AppendLine($"{item.Key}:", defaultFont)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.basalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        .AppendNewLine
                        Exit For
                    End If
                    Dim endTime As String = If(e.IsLast, Eleven59Str, StandardTimeOnlyWidth(item.Value.basalRates(e.Index + 1).Time))
                    .AppendLine($"{vbTab}{StandardTimeOnlyWidth(basalRate.Time)}{vbTab}-{vbTab}{endTime,9}{vbTab}{basalRate.UnitsPerHr:F3} U/hr", defaultFont)
                Next
                .AppendNewLine
            Next
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Preset Temp Setup", subheadingFont, ChrW(&H2699), subheadingBoldtFont)

            For Each item As KeyValuePair(Of String, PresetTempRecord) In _pdf.PresetTemp
                .AppendLine($"{item.Key}:", defaultFont)
                Dim presetTempRecord As PresetTempRecord = item.Value

                If presetTempRecord.IsValid Then
                    .AppendLine($"{vbTab}{presetTempRecord.PresetAmount}", defaultFont)
                    .AppendLine($"{vbTab}Duration:{vbTab}{presetTempRecord.Duration.ToFormattedTimeSpan("U/hr").Trim}", defaultFont)
                Else
                    .AppendNewLine
                End If
            Next
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Dual/Square Wave", subheadingFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Dual:  {vbTab}{_pdf.Bolus.EasyBolus.DualSquare.Dual,2}", defaultFont)
            .AppendLine($"{vbTab}Square:{vbTab}{_pdf.Bolus.EasyBolus.DualSquare.Square,2}", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Preset Bolus Setup", subheadingFont, ChrW(&H2699), subheadingBoldtFont)
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In _pdf.PresetBolus
                .AppendLine(item.Key, defaultFont)
                If item.Value.IsValid Then
                    Dim presetBolus As PresetBolusRecord = item.Value
                    .AppendLine($"{vbTab}Bolus: {presetBolus.Bolus}{vbTab}Type: {If(item.Value.BolusTypeNormal, "Normal", "Square")}", defaultFont)
                    If Not item.Value.BolusTypeNormal Then
                        .AppendLine($"{vbTab}Duration: {presetBolus.Duration} hr", defaultFont)
                    End If
                End If
                .AppendNewLine
            Next
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbMainRight
            .AppendLine($"Delivery Settings, Device Settings, SmartGuard", headingBoldFont)
            .AppendNewLine
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Increment", subheadingFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Bolus Increment: {_pdf.Bolus.EasyBolus.BolusIncrement:F3}", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Max Basal/Bolus", subheadingFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Max Basal:  {_pdf.Basal.MaximumBasalRate} U/hr", defaultFont)
            .AppendLine($"{vbTab}Max Bolus: {_pdf.Bolus.BolusWizard.MaximumBolus} U", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Speed", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Bolus Speed: {_pdf.Bolus.EasyBolus.BolusSpeed}", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Easy Bolus", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Easy Bolus: {_pdf.Bolus.EasyBolus.EasyBolus}", defaultFont)
            .AppendLine($"{vbTab}Step Size:   {_pdf.Bolus.EasyBolus.BolusIncrement} U", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Auto Suspend", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Alarm: {_pdf.Utilities.AutoSuspend.Alarm}", defaultFont)
            If _pdf.Utilities.AutoSuspend.Alarm <> "Off" Then
                .AppendLine($"Time: {_pdf.Utilities.AutoSuspend.Time.ToFormattedTimeSpan("hr")}", defaultFont)
            End If
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Sensor", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Sensor: {_pdf.Sensor.SensorOn}", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Display Options", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Brightness: {_pdf.Utilities.Brightness}", defaultFont)
            .AppendLine($"{vbTab}Backlight:  {_pdf.Utilities.BackLightTimeout.ToFormattedTimeSpan("min").TrimStart("0"c)}", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)}", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}SmartGuard: {_pdf.SmartGuard.SmartGuard}", defaultFont)
            .AppendNewLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > SmartGuard Settings", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont)
            .AppendLine($"{vbTab}Target: {_pdf.SmartGuard.Target}", defaultFont)
            .AppendLine($"{vbTab}Auto Correction: {_pdf.SmartGuard.AutoCorrection}", defaultFont)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbHighAlertMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Alert Settings > High Alert", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont, False)
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
                .Rows.Add(h.Start, h.End, $"{h.HighLimit} {h.ValueUnits}", h.AlertBeforeHigh, h.TimeBeforeHigh, h.AlertOnHigh, h.RiseAlert, h.RaiseLimit)
                .Columns(NameOf(ColumnTimeBeforeHighText)).Visible = h.AlertBeforeHigh
            Next
        End With

        With Me.RtbLowAlertMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Alert Settings > Low Alert", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont, False)
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
                .Rows.Add(l.Start, l.End, $"{l.LowLimit} {l.ValueUnits}", l.Suspend, l.AlertOnLow, l.AlertBeforeLow, l.ResumeBasalAlert)
                .Columns("ColumnResumeBasalAlert").Visible = String.IsNullOrWhiteSpace(l.Suspend)
            Next

        End With

        With Me.RtbHighSnoozeMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Snooze Menu > Snooze High & Low > High Snooze: {_pdf.HighAlerts}", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbLowSnoozeMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Snooze Menu > Snooze High & Low > Low Snooze: {_pdf.LowAlerts}", defaultBoldFont, ChrW(&H2699), subheadingBoldtFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

    End Sub

End Class
