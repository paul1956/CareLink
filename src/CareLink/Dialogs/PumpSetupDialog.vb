' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PumpSetupDialog
    Private _pdf As PdfSettingsRecord

    Public WriteOnly Property Pdf As PdfSettingsRecord
        Set
            _pdf = Value
        End Set
    End Property

    Private Shared Function StandardTimeOnlyWidth(tOnly As TimeOnly) As String
        Dim tAsString As String = tOnly.ToString
        If tAsString.Length < 7 Then Return tAsString
        Return tAsString.PadLeft(9)
    End Function

    Private Sub DataGridView_Paint(sender As Object, e As PaintEventArgs) Handles DataGridViewHighAlert.Paint, DataGridViewLowAlert.Paint
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Rows.Count = 0 Then
            TextRenderer.DrawText(e.Graphics, "No records found.",
                New Font(dgv.Font.FontFamily, 20), dgv.ClientRectangle,
                dgv.ForeColor, dgv.BackgroundColor,
                TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End If

    End Sub

    Private Sub DataGridViewHighAlert_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewHighAlert.SelectionChanged, DataGridViewLowAlert.SelectionChanged
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.ClearSelection()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub PumpSetupDialog_Shown(sender As Object, e1 As EventArgs) Handles MyBase.Shown
        If _pdf Is Nothing Then
            Throw New NullReferenceException(NameOf(_pdf))
        End If

        Dim defaultFont As New Font(Me.RtbMainLeft.Font.FontFamily, 14, FontStyle.Regular)
        Dim defaultBoldFont As New Font(Me.RtbMainLeft.Font.FontFamily, 14, FontStyle.Bold)
        Dim tahomaBoldFont As New Font("Tahoma", 16, FontStyle.Bold)
        Dim tahomaFont As New Font("Tahoma", 16, FontStyle.Regular)
        Dim headingBoldFont As New Font("Tahoma", 18, FontStyle.Bold)
        Me.RtbMainLeft.Clear()
        Me.RtbMainRight.Clear()
        With Me.RtbMainLeft
            .ReadOnly = False

            .AppendLine($"Delivery Settings", headingBoldFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Wizard Setup", tahomaFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Bolus Wizard: {_pdf.Bolus.BolusWizard.BolusWizard}", defaultFont)
            .AppendLine

            Dim optionalS As String = If(_pdf.Bolus.DeviceCarbohydrateRatios.Count > 1, "s", "")
            .AppendLine($"Carb Ratio{optionalS}:", defaultBoldFont)
            For Each item As CarbRatioRecord In _pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                .AppendLine($"{vbTab}{StandardTimeOnlyWidth(item.StartTime)}{vbTab}-{vbTab}{StandardTimeOnlyWidth(item.EndTime)}{vbTab}{item.CarbRatio} g/U", defaultFont)
            Next
            .AppendLine

            optionalS = If(_pdf.Bolus.InsulinSensivity.Count > 1, "s", "")
            .AppendLine($"Insulin Sensitivity Factor{optionalS}:", defaultBoldFont)
            For Each e As IndexClass(Of InsulinSensivityRecord) In _pdf.Bolus.InsulinSensivity.WithIndex
                Dim item As InsulinSensivityRecord = e.Value
                If Not item.IsValid Then
                    .AppendLine
                    Exit For
                End If
                Dim endTime As String = If(e.IsLast, s_midnight, StandardTimeOnlyWidth(_pdf.Bolus.InsulinSensivity(e.Index + 1).Time))
                .AppendLine($"{vbTab}{StandardTimeOnlyWidth(item.Time)}{vbTab}-{vbTab}{endTime}{vbTab}{item.Sensitivity.RoundTo025:F1} {_pdf.Bolus.BolusWizard.Units.CarbUnits}/U", defaultFont)
            Next
            .AppendLine

            optionalS = If(_pdf.Bolus.BloodGlucoseTarget.Count > 1, "s", "")
            .AppendLine($"BG Target{optionalS}:", defaultBoldFont)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In _pdf.Bolus.BloodGlucoseTarget.WithIndex
                Dim item As BloodGlucoseTargetRecord = e.Value
                If Not item.IsValid Then
                    .AppendLine
                    Exit For
                End If
                Dim endTime As String = If(e.IsLast, s_midnight, StandardTimeOnlyWidth(_pdf.Bolus.BloodGlucoseTarget(e.Index + 1).Time))
                .AppendLine($"{vbTab}{StandardTimeOnlyWidth(item.Time)}{vbTab}-{vbTab}{endTime}{vbTab}{item.Low}-{item.High} {_pdf.Bolus.BolusWizard.Units.BgUnits}", defaultFont)
            Next
            .AppendLine

            .AppendLine($"Active Insulin Time: {_pdf.Bolus.BolusWizard.ActiveInsulinTime} hr", defaultBoldFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Basal Pattern(s) Setup", tahomaFont, ChrW(&H2699), tahomaBoldFont)
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In _pdf.Basal.NamedBasals
                .AppendLine($"{item.Key}:", defaultFont)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.basalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        .AppendLine
                        Exit For
                    End If
                    Dim endTime As String = If(e.IsLast, s_midnight, StandardTimeOnlyWidth(item.Value.basalRates(e.Index + 1).Time))
                    .AppendLine($"{vbTab}{StandardTimeOnlyWidth(basalRate.Time)}{vbTab}-{vbTab}{endTime,9}{vbTab}{basalRate.UnitsPerHr:F3} U/hr", defaultFont)
                Next
                .AppendLine
            Next
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Preset Temp Setup", tahomaFont, ChrW(&H2699), tahomaBoldFont)

            For Each item As KeyValuePair(Of String, PresetTempRecord) In _pdf.PresetTemp
                .AppendLine($"{item.Key}:", defaultFont)
                Dim presetTempRecord As PresetTempRecord = item.Value

                If presetTempRecord.IsValid Then
                    .AppendLine($"{vbTab}{presetTempRecord.Type}", defaultFont)
                    .AppendLine($"{vbTab}Duration:{vbTab}{presetTempRecord.Duration.ToFormattedTimeSpan("U/hr").Trim}", defaultFont)
                Else
                    .AppendLine
                End If
            Next
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Dual/Square Wave", tahomaFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Dual:  {vbTab}{_pdf.Bolus.EasyBolus.DualSquare.Dual,2}", defaultFont)
            .AppendLine($"{vbTab}Square:{vbTab}{_pdf.Bolus.EasyBolus.DualSquare.Square,2}", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Preset Bolus Setup", tahomaFont, ChrW(&H2699), tahomaBoldFont)
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In _pdf.PresetBolus
                .AppendLine(item.Key, defaultFont)
                If item.Value.IsValid Then
                    Dim presetBolus As PresetBolusRecord = item.Value
                    .AppendLine($"{vbTab}Bolus: {presetBolus.Bolus}{vbTab}Type: {If(item.Value.BolusTypeNormal, "Normal", "Square")}", defaultFont)
                    If Not item.Value.BolusTypeNormal Then
                        .AppendLine($"{vbTab}Duration: {presetBolus.Duration} hr", defaultFont)
                    End If
                End If
                .AppendLine
            Next
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbMainRight
            .AppendLine($"Delivery Settings, Device Settings, SmartGuard", headingBoldFont)
            .AppendLine
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Increment", tahomaFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Bolus Increment: {_pdf.Bolus.EasyBolus.BolusIncrement:F3}", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Max Basal/Bolus", tahomaFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Max Basal:  {_pdf.Basal.MaximumBasalRate} U/hr", defaultFont)
            .AppendLine($"{vbTab}Max Bolus: {_pdf.Bolus.BolusWizard.MaximumBolus} U", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Speed", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Bolus Speed: {_pdf.Bolus.EasyBolus.BolusSpeed}", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Easy Bolus", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Easy Bolus: {_pdf.Bolus.EasyBolus.EasyBolus}", defaultFont)
            .AppendLine($"{vbTab}Step Size:   {_pdf.Bolus.EasyBolus.BolusIncrement} U", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Auto Suspend", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Alarm: {_pdf.Utilities.AutoSuspend.Alarm}", defaultFont)
            If _pdf.Utilities.AutoSuspend.Alarm <> "Off" Then
                .AppendLine($"Time: {_pdf.Utilities.AutoSuspend.Time.ToFormattedTimeSpan("hr")}", defaultFont)
            End If
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Sensor", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Sensor: {_pdf.Sensor.SensorOn}", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Display Options", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            Call .AppendLine($"{vbTab}Brightness: {_pdf.Utilities.Brightness}", defaultFont)
            Call .AppendLine($"{vbTab}Backlight:  {_pdf.Utilities.BackLightTimeout.ToFormattedTimeSpan("min").TrimStart("0"c)}", defaultFont)
            Call .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)}", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}SmartGuard: {_pdf.SmartGuard.SmartGuard}", defaultFont)
            .AppendLine

            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > SmartGuard Settings", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{vbTab}Target: {_pdf.SmartGuard.Target}", defaultFont)
            .AppendLine($"{vbTab}Auto Correction: {_pdf.SmartGuard.AutoCorrection}", defaultFont)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbHighAlertMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Alert Settings > High Alert", defaultBoldFont, ChrW(&H2699), tahomaBoldFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.DataGridViewHighAlert
            .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
            .Rows.Clear()
            For Each c As DataGridViewColumn In .Columns
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            For Each h As HighAlertRecord In _pdf.HighAlerts.HighAlert
                .Rows.Add(h.Start, h.End, $"{h.HighLimit} {h.ValueUnits}", h.AlertBeforeHigh, h.TimeBeforeHigh, h.AlertOnHigh, h.RiseAlert, h.RaiseLimit)
                .Columns(NameOf(ColumnTimeBeforeHighText)).Visible = h.AlertBeforeHigh
            Next
        End With

        With Me.RtbHighSnoozeMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Snooze Menu > Snooze High & Low > High Snooze: {_pdf.HighAlerts}", defaultBoldFont, ChrW(&H2699), tahomaBoldFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbLowAlertMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Alert Settings > Snooze High & Low > Low Snooze:", defaultBoldFont, ChrW(&H2699), tahomaBoldFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.DataGridViewLowAlert
            .Rows.Clear()
            .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
            For Each c As DataGridViewColumn In .Columns
                c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            Next
            For Each l As LowAlertRecord In _pdf.LowAlerts.LowAlert
                .Rows.Add(l.Start, l.End, $"{l.LowLimit} {l.ValueUnits}", l.Suspend, l.AlertOnLow, l.AlertBeforeLow, l.ResumeBasalAlert)
                .Columns("ColumnResumeBasalAlert").Visible = String.IsNullOrWhiteSpace(l.Suspend)
            Next

        End With

        With Me.RtbLowSnoozeMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Snooze Menu > Snooze High & Low > Low Snooze: {_pdf.LowAlerts}", defaultBoldFont, ChrW(&H2699), tahomaBoldFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With

    End Sub

End Class
