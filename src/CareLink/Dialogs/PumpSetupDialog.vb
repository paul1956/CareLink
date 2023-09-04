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
        Me.RtbMainLeft.Clear()
        Me.RtbMainRight.Clear()
        With Me.RtbMainLeft
            .ReadOnly = False

            .AppendLine($"Delivery Settings", defaultBoldFont)
            .AppendLine

            .AppendLine($"DELIVERY SETTINGS", tahomaFont)
            .AppendLine($"BOLUS WIZARD SETUP:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Wizard Setup", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"Bolus Wizard: {_pdf.Bolus.BolusWizard.BolusWizard}", defaultBoldFont)
            .AppendLine

            .AppendLine($"Carb Ratio:", defaultBoldFont)
            For Each item As CarbRatioRecord In _pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                .AppendLine($"{StandardTimeOnlyWidth(item.StartTime)}{vbTab}-{vbTab}{StandardTimeOnlyWidth(item.EndTime)}{vbTab}{item.CarbRatio} g/U", defaultBoldFont)
            Next
            .AppendLine

            .AppendLine($"Insulin Sensitivity Factor:", defaultBoldFont)
            For Each e As IndexClass(Of InsulinSensivityRecord) In _pdf.Bolus.InsulinSensivity.WithIndex
                Dim item As InsulinSensivityRecord = e.Value
                If Not item.IsValid Then
                    .AppendLine
                    Exit For
                End If
                Dim endTime As String = If(e.IsLast, s_midnight, StandardTimeOnlyWidth(_pdf.Bolus.InsulinSensivity(e.Index + 1).Time))
                .AppendLine($"{StandardTimeOnlyWidth(item.Time)}{vbTab}-{vbTab}{endTime}{vbTab}{item.Sensitivity.RoundTo025:F1} {_pdf.Bolus.BolusWizard.Units.CarbUnits}/U", defaultFont)
            Next
            .AppendLine

            .AppendLine($"BG Target:", defaultBoldFont)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In _pdf.Bolus.BloodGlucoseTarget.WithIndex
                Dim item As BloodGlucoseTargetRecord = e.Value
                If Not item.IsValid Then
                    .AppendLine
                    Exit For
                End If
                Dim endTime As String = If(e.IsLast, s_midnight, StandardTimeOnlyWidth(_pdf.Bolus.BloodGlucoseTarget(e.Index + 1).Time))
                .AppendLine($"{StandardTimeOnlyWidth(item.Time)}{vbTab}-{vbTab}{endTime}{vbTab}{item.Low}-{item.High} {_pdf.Bolus.BolusWizard.Units.BgUnits}", defaultFont)
            Next
            .AppendLine

            .AppendLine($"Active Insulin Time: {_pdf.Bolus.BolusWizard.ActiveInsulinTime} hr", defaultBoldFont)
            .AppendLine

            .AppendLine($"BASAL PATTERN SETUP:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Basal Pattern Setup", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In _pdf.Basal.NamedBasals
                .AppendLine($"{item.Key}:", defaultFont)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.basalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        .AppendLine
                        Exit For
                    End If
                    Dim endTime As String = If(e.IsLast, s_midnight, StandardTimeOnlyWidth(item.Value.basalRates(e.Index + 1).Time))
                    .AppendLine($"{StandardTimeOnlyWidth(basalRate.Time)}{vbTab}-{vbTab}{endTime,9}{vbTab}{basalRate.UnitsPerHr:F3} U/hr", defaultFont)
                Next
                .AppendLine
            Next
            .AppendLine

            .AppendLine($"PRESET TEMP SETUP: ", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Preset Temp Setup", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)

            For Each item As KeyValuePair(Of String, PresetTempRecord) In _pdf.PresetTemp
                .AppendLine($"{item.Key}:", defaultFont)
                If item.Value.IsValid Then
                    .AppendText($"{item.Value.Type.Value}", defaultFont)
                    .AppendLine($"{item.Value.Duration}:", defaultFont)
                Else
                    .AppendLine
                End If
            Next
            .AppendLine

            .AppendLine($"DUAL/SQUARE WAVE:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Dual/Square Wave", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"Dual:    {_pdf.Bolus.EasyBolus.DualSquare.Dual}", defaultFont)
            .AppendLine($"Square: {_pdf.Bolus.EasyBolus.DualSquare.Square}", defaultFont)
            .AppendLine

            .AppendLine($"PRESET BOLUS SETUP:,tahomaBoldFont", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Preset Bolus Setup", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In _pdf.PresetBolus
                .AppendLine(item.Key, defaultFont)
                If item.Value.IsValid Then
                    Dim presetBolus As PresetBolusRecord = item.Value
                    .AppendLine($"Bolus: {presetBolus.Bolus}{vbTab}Type: {If(item.Value.BolusTypeNormal, "Normal", "Square")}", defaultFont)
                    If Not item.Value.BolusTypeNormal Then
                        .AppendLine($"Square: {presetBolus.Square}", defaultFont)
                        .AppendLine($"Duration: {presetBolus.Duration} hr", defaultFont)
                    Else
                        .AppendLine($"Normal: {presetBolus.Bolus}", defaultFont)
                    End If
                End If
                .AppendLine
            Next
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbMainRight
            .AppendLine($"Delivery Settings, Device Settings, SmartGuard", defaultBoldFont)
            .AppendLine

            .AppendLine($"DELIVERY SETTINGS (CONT.)", tahomaFont)
            .AppendLine($"BOLUS INCREMENT:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Increment", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{_pdf.Bolus.EasyBolus.BolusIncrement:F3}", defaultFont)
            .AppendLine

            .AppendLine($"MAX BASAL/BOLUS:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Max Basal/Bolus", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"Max Basal: {_pdf.Basal.MaximumBasalRate} U/hr", defaultFont)
            .AppendLine($"Max Bolus: {_pdf.Bolus.BolusWizard.MaximumBolus} U", defaultFont)
            .AppendLine

            .AppendLine($"BOLUS SPEED:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Bolus Speed", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{_pdf.Bolus.EasyBolus.BolusSpeed}", defaultFont)
            .AppendLine

            .AppendLine($"EASY BOLUS:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Easy Bolus", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{_pdf.Bolus.EasyBolus.EasyBolus} Step Size {_pdf.Bolus.EasyBolus.BolusIncrement} U", defaultFont)
            .AppendLine

            .AppendLine($"AUTO SUSPEND:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Auto Suspend", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"Alarm: {_pdf.Utilities.AutoSuspend.Alarm}", defaultBoldFont)
            If _pdf.Utilities.AutoSuspend.Alarm <> "Off" Then
                .AppendLine($"Time: {_pdf.Utilities.AutoSuspend.Time} hr", defaultFont)
            End If
            .AppendLine

            .AppendLine($"DEVICE SETTINGS", tahomaFont)
            .AppendLine($"SENSOR:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Sensor", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"{_pdf.Sensor.SensorOn}", defaultFont)
            .AppendLine

            .AppendLine($"DISPLAY:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > Display Options", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"Brightness {_pdf.Utilities.Brightness}", defaultFont)
            Dim backLightTimeout As String = If(_pdf.Utilities.BackLightTimeout < New TimeSpan(0, 1, 0),
                                          $"{_pdf.Utilities.BackLightTimeout.Seconds} sec",
                                          $"{_pdf.Utilities.BackLightTimeout.Minutes}:{_pdf.Utilities.BackLightTimeout.Seconds:D2} min"
                                         )
            .AppendLine($"Backlight {backLightTimeout}", defaultFont)
            .AppendLine

            .AppendLine($"SMARTGUARD:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)}", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"SmartGuard: {_pdf.SmartGuard.SmartGuard}", defaultFont)
            .AppendLine

            .AppendLine($"SMARTGUARD SETTINGS:", tahomaFont)
            .AppendLine($"Menu > {ChrW(&H2699)} > Delivery Settings > SmartGuard Settings", defaultBoldFont, ChrW(&H2699), tahomaBoldFont)
            .AppendLine($"Target: {_pdf.SmartGuard.Target}", defaultFont)
            .AppendLine($"Auto Correction: {_pdf.SmartGuard.AutoCorrection}", defaultFont)
            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbHighAlertMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Alert Settings > High Alert", defaultBoldFont, ChrW(&H2699), tahomaBoldFont, False)
            .ReadOnly = True
            .SelectionStart = 0
        End With
        With Me.RtbHighSnoozeMenu
            .Text = ""
            .AppendLine($"Menu > {ChrW(&H2699)} > Snooze Menu > Snooze High & Low > High Snooze: {_pdf.HighAlerts}", defaultBoldFont, ChrW(&H2699), tahomaBoldFont, False)
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
