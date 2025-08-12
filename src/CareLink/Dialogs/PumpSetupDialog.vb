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

        Me.RtbMainLeft.Clear()
        Me.RtbMainRight.Clear()

        With Me.RtbMainLeft
            .ReadOnly = False

            .AppendTextWithSymbol(text:=$"Menu > {Gear} > Delivery Settings > Bolus Wizard Setup", symbol:=Gear, includeNewLine:=True)
            Me.Settings2DeliverySettings1BolusWizardSetup1BolusWizard()
            Me.Settings2DeliverySettings1BolusWizardSetup2CarbRatio()
            Me.Settings2DeliverySettings1BolusWizardSetup3ActiveInsulinTime()
            Me.Settings2DeliverySettings1BolusWizardSetup4InsulinSensitivityFactor()
            Me.Settings2DeliverySettings1BolusWizardSetup5BgTarget()

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Basal Pattern Setup", symbol:=Gear)
            Me.Settings2DeliverySettings2BasalPatternSetup()
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Max Basal/Bolus", symbol:=Gear)
            Me.Settings2DeliverySettings3MaxBasalBolus()
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Dual/Square Wave", symbol:=Gear)
            Me.Settings2DeliverySettings4DualSquareWave()
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Bolus Increment", symbol:=Gear)
            Me.Settings2DeliverySettings5BolusIncrement()
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Bolus Speed", symbol:=Gear)
            Me.Settings2DeliverySettings6BolusSpeed()
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Preset Bolus Setup", symbol:=Gear)
            Me.Settings2DeliverySettings7PresetBolusSetup()
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Preset Temp Setup", symbol:=Gear)
            Me.Settings2DeliverySettings8PresetTempSetup()

            .ReadOnly = True
            .SelectionStart = 0
        End With

        With Me.RtbMainRight
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Device Settings", symbol:=Gear)
            Me.Settings2DeviceSettings()

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Device Settings > Time & Date", symbol:=Gear)
            .AppendKeyValue(key:="Time Format:", value:=_pdf.Utilities.TimeFormat)

            .AppendNewLine
            .AppendTextWithSymbol(text:=$"Menu > {Gear} > Device Settings > Display", symbol:=Gear)
            .AppendKeyValue(key:="Brightness:", value:=_pdf.Utilities.Brightness)
            Dim value As String = _pdf.Utilities.BackLightTimeout.ToFormattedTimeSpan(unit:="min")
            .AppendKeyValue(key:="Backlight:", value)

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Device Settings > Easy Bolus", symbol:=Gear)
            .AppendKeyValue(key:="Easy Bolus:", value:=_pdf.Bolus.EasyBolus.EasyBolus)
            .AppendKeyValue(
                key:="Step Size: ",
                value:=$"{_pdf.Bolus.EasyBolus.BolusIncrement} U")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Delivery Settings > Auto Suspend", symbol:=Gear)
            .AppendKeyValue(key:="Alarm:", value:=_pdf.Utilities.AutoSuspend.Alarm)
            Dim bufferLength As Integer = .Text.Length
            .AppendTextWithFontChange(text:=$"{Indent4}Time:", newFont:=FixedWidthBoldFont)
            If _pdf.Utilities.AutoSuspend.Alarm = "Off" Then
                .AppendTextWithFontChange(text:="12:00 hr".AlignCenter, newFont:=FixedWidthFont, includeNewLine:=True)
                .Select(start:=bufferLength, length:= .Text.Length - bufferLength)
                .SelectionBackColor = SystemColors.Window
                .SelectionColor = SystemColors.GrayText
                .SelectionStart = .Text.Length
                .SelectionBackColor = SystemColors.Window
                .SelectionColor = SystemColors.WindowText
            Else
                .AppendTextWithFontChange(
                    text:=$"{Indent4}{_pdf.Utilities.AutoSuspend.Time.ToFormattedTimeSpan(unit:="hr")}",
                    newFont:=FixedWidthFont,
                    includeNewLine:=True)
            End If

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > High Alert",
                symbol:=Gear)
            Me.Settings1AlertSettings1HighAlert()

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Low Alert",
                symbol:=Gear)
            Me.Settings1AlertSettings2LowAlert()

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Snooze High & Low",
                symbol:=Gear)
            Me.Settings1AlertSettings3SnoozeHighLow()

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Reminders > Low Reservoir",
                symbol:=Gear)
            Me.Settings1AlertSettings4Reminders()

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Shield} > SmartGuard > SmartGuard Settings", symbol:=Shield)
            .AppendKeyValue(
                key:="Target:",
                value:=$"{_pdf.SmartGuard.Target.RoundToSingle(digits:=0, considerValue:=True)}")
            .AppendKeyValue(
                key:="Auto Correction:",
                value:=$"{_pdf.SmartGuard.SmartGuard}")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Shield} > SmartGuard", symbol:=Shield)
            .AppendKeyValue(
                key:="SmartGuard:",
                value:=$"{_pdf.SmartGuard.AutoCorrection}")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Speaker} > Sound & Vibration",
                symbol:=Speaker)
            .AppendKeyValue(
                key:="Volume:",
                value:=$"{_pdf.Utilities.AlarmVolume}")
            .AppendKeyValue(
                key:="Sound:",
                value:=$"{_pdf.Utilities.AudioOptions.ContainsIgnoreCase(value:="Audio").BoolToOnOff()}")
            .AppendKeyValue(
                key:="Vibration:",
                value:=$"{_pdf.Utilities.AudioOptions.ContainsIgnoreCase(value:="Vibrate").BoolToOnOff()}")

            .ReadOnly = True
            .SelectionStart = 0
        End With
    End Sub

    Private Sub Settings1Alert()
        With Me.RtbMainLeft
            For Each index As IndexClass(Of KeyValuePair(Of String, NamedBasalRecord)) In _pdf.Basal.NamedBasal.WithIndex
                Dim item As KeyValuePair(Of String, NamedBasalRecord) = index.Value
                .AppendTextWithFontChange(
                    text:=$"{Indent4}{item.Key}:",
                    newFont:=FixedWidthBoldFont,
                    includeNewLine:=True)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.basalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        Exit For
                    End If
                    Dim startTime As TimeOnly = basalRate.Time
                    Dim endTime As TimeOnly = If(e.IsLast,
                                                 Eleven59,
                                                 item.Value.basalRates(index:=e.Index + 1).Time)
                    Dim value As String = $"{basalRate.UnitsPerHr:F3} U/hr"
                    .AppendTimeValueRow(startTime, endTime, value, _pdf.Utilities.TimeFormat)
                Next
            Next
        End With
    End Sub

    Private Sub Settings1AlertSettings1HighAlert()
        With Me.RtbMainRight
            For Each h As HighAlertRecord In _pdf.HighAlerts.HighAlert
                .AppendTimeValueRow(
                    startTime:=h.Start,
                    endTime:=h.End,
                    value:=$"{h.HighLimit}",
                    timeFormat:=_pdf.Utilities.TimeFormat,
                    indent:=Indent4, heading:=True)
                .AppendKeyValue(key:="Alert Before High:", value:=h.AlertBeforeHigh.BoolToOnOff(), indent:=Indent8)
                .AppendKeyValue(key:="Time Before High:", value:=h.TimeBeforeHigh, indent:=Indent8)
                .AppendKeyValue(key:="Alert on High:", value:=h.AlertOnHigh.BoolToOnOff(), indent:=Indent8)
                .AppendKeyValue(key:="Rise Alert:", value:=h.RiseAlert.BoolToOnOff(), indent:=Indent8)
            Next
        End With
    End Sub

    Private Sub Settings1AlertSettings2LowAlert()
        With Me.RtbMainRight
            For Each l As LowAlertRecord In _pdf.LowAlerts.LowAlert
                .AppendTimeValueRow(
                    startTime:=l.Start,
                    endTime:=l.End,
                    value:=$"{l.LowLimit}",
                    timeFormat:=_pdf.Utilities.TimeFormat,
                    indent:=Indent4, heading:=True)
                .AppendKeyValue(key:=$"Suspend:", value:=$"{l.Suspend}", indent:=Indent8)
                .AppendKeyValue(key:="Alert Before Low:", value:=l.AlertBeforeLow.BoolToOnOff(), indent:=Indent8)
                .AppendKeyValue(key:="Alert on Low:", value:=l.AlertOnLow.BoolToOnOff(), indent:=Indent8)
                .AppendKeyValue(key:="Resume Basal Alert:", value:=$"{l.ResumeBasalAlert}", indent:=Indent8)
            Next
        End With
    End Sub

    Private Sub Settings1AlertSettings3SnoozeHighLow()
        With Me.RtbMainRight
            .AppendKeyValue(
                key:="High Snooze:",
                value:=$"{_pdf.HighAlerts}")

            .AppendKeyValue(
                key:="Low Snooze:",
                value:=$"{_pdf.LowAlerts}")
        End With
    End Sub

    Private Sub Settings1AlertSettings4Reminders()
        With Me.RtbMainRight
            .AppendKeyValue(
            key:="Low Reservoir Warning:",
                value:=$"{_pdf.Reminders.LowReservoirWarning}")
            .AppendKeyValue(
                key:="Type:",
                value:="Units")
            .AppendKeyValue(
                key:="Units:",
                value:=$"{_pdf.Reminders.Amount}")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Reminders > Set Change",
                symbol:=Gear)
            .AppendKeyValue(
                key:="Set Change:",
                value:=$"{_pdf.Reminders.SetChange}")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Reminders > Bolus BG Check",
                symbol:=Gear)
            .AppendKeyValue(
                key:="Reminder:",
                value:=$"{_pdf.Reminders.BolusBgCheck}")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Reminders > Missed Meal",
                symbol:=Gear)
            For Each item As KeyValuePair(Of String, MealStartEndRecord) In _pdf.Reminders.MissedMealBolus
                Dim startTime As String = item.Value.Start
                Dim endTime As String = item.Value.End
                .AppendTimeValueRow(item.Key, startTime, endTime)
            Next

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu > {Gear} > Alert Settings > Reminders > Personal",
                symbol:=Gear)
            For Each item As KeyValuePair(Of String, PersonalRemindersRecord) In _pdf.Reminders.PersonalReminders
                Dim startTime As String = item.Value.Time
                .AppendTimeValueRow(key:=item.Key, startTime)
            Next

        End With
    End Sub

    Private Sub Settings2DeliverySettings1BolusWizardSetup1BolusWizard()
        With Me.RtbMainLeft
            .AppendKeyValue(
                key:="Bolus Wizard:",
                value:=$"{_pdf.Bolus.BolusWizard.BolusWizard}")
            .AppendNewLine
        End With
    End Sub

    Private Sub Settings2DeliverySettings1BolusWizardSetup2CarbRatio()
        With Me.RtbMainLeft

            Dim text As String = $"{_pdf.Bolus.DeviceCarbohydrateRatios.Count.ToUnits(
                unit:=$"{Indent4}Carbohydrate Ratio",
                suffix:=":",
                includeValue:=False)}"
            .AppendTextWithFontChange(text, newFont:=FixedWidthBoldFont, includeNewLine:=True)

            For Each item As CarbRatioRecord In _pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                Dim value As String = $"{item.CarbRatio} g/U"
                .AppendTimeValueRow(item.StartTime, item.EndTime, value, _pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine
        End With
    End Sub

    Private Sub Settings2DeliverySettings1BolusWizardSetup3ActiveInsulinTime()
        Dim timeUnits As String = _pdf.Bolus.BolusWizard.ActiveInsulinTime.ToHoursMinutes()
        With Me.RtbMainLeft
            .AppendKeyValue(key:="Active Insulin Time:", value:=$"{timeUnits} hr")
            .AppendNewLine
        End With
    End Sub

    Private Sub Settings2DeliverySettings1BolusWizardSetup4InsulinSensitivityFactor()

        With Me.RtbMainLeft
            Dim text As String =
                _pdf.Bolus.InsulinSensitivity.Count.ToUnits(
                    unit:=$"{Indent4}Insulin Sensitivity Factor",
                    suffix:=":",
                    includeValue:=False)
            .AppendTextWithFontChange(
                text,
                newFont:=FixedWidthBoldFont,
                includeNewLine:=True)
            For Each e As IndexClass(Of InsulinSensitivityRecord) In _pdf.Bolus.InsulinSensitivity.WithIndex
                Dim item As InsulinSensitivityRecord = e.Value
                If Not item.IsValid Then
                    Exit For
                End If

                Dim startTime As TimeOnly = item.Time
                Dim endTime As TimeOnly =
                    If(e.IsLast,
                       Midnight,
                       _pdf.Bolus.InsulinSensitivity(index:=e.Index + 1).Time)
                Dim sensitivity As String = If(item.Sensitivity < 0.01,
                                               "0.00",
                                               item.Sensitivity.RoundTo025.ToString(format:="F3"))
                Dim value As String = $"{sensitivity} {_pdf.Bolus.BolusWizard.Units.CarbUnits}/U"
                .AppendTimeValueRow(startTime, endTime, value, _pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine
        End With
    End Sub

    Private Sub Settings2DeliverySettings1BolusWizardSetup5BgTarget()
        Dim text As String = _pdf.Bolus.BloodGlucoseTarget.Count.ToUnits(
            unit:=$"{Indent4}Blood Glucose Target",
            suffix:=":",
            includeValue:=False)

        With Me.RtbMainLeft
            .AppendTextWithFontChange(
                text,
                newFont:=FixedWidthBoldFont,
                includeNewLine:=True)
            For Each e As IndexClass(Of BloodGlucoseTargetRecord) In _pdf.Bolus.BloodGlucoseTarget.WithIndex
                Dim item As BloodGlucoseTargetRecord = e.Value
                If Not item.IsValid Then
                    .AppendNewLine
                    Exit For
                End If

                Dim startTime As TimeOnly

                Dim endTime As TimeOnly = If(e.IsLast,
                                             Eleven59,
                                             _pdf.Bolus.BloodGlucoseTarget(index:=e.Index + 1).Time)
                startTime = item.Time
                Dim value As String = $"{item.Low}-{item.High} {_pdf.Bolus.BolusWizard.Units.BgUnits}"
                .AppendTimeValueRow(startTime, endTime, value, _pdf.Utilities.TimeFormat)
            Next
            .AppendNewLine
        End With
    End Sub

    Private Sub Settings2DeliverySettings2BasalPatternSetup()
        With Me.RtbMainLeft
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In _pdf.Basal.NamedBasal
                .AppendTextWithFontChange(
                    text:=$"{Indent4}{item.Key}:",
                    newFont:=FixedWidthBoldFont,
                    includeNewLine:=True)
                For Each e As IndexClass(Of BasalRateRecord) In item.Value.basalRates.WithIndex
                    Dim basalRate As BasalRateRecord = e.Value
                    If Not basalRate.IsValid Then
                        Exit For
                    End If
                    Dim startTime As TimeOnly = basalRate.Time
                    Dim endTime As TimeOnly = If(e.IsLast,
                                                 Eleven59,
                                                 item.Value.basalRates(index:=e.Index + 1).Time)
                    Dim value As String = $"{basalRate.UnitsPerHr:F3} U/hr"
                    .AppendTimeValueRow(startTime, endTime, value, _pdf.Utilities.TimeFormat)
                Next
            Next
        End With
    End Sub

    Private Sub Settings2DeliverySettings3MaxBasalBolus()
        With Me.RtbMainLeft
            .AppendKeyValue(
                key:="Max Basal:",
                value:=$"{_pdf.Basal.MaximumBasalRate:2} U/hr")

            .AppendKeyValue(
                key:="Max Bolus:",
                value:=$"{_pdf.Bolus.BolusWizard.MaximumBolus:2} U")
        End With
    End Sub

    Private Sub Settings2DeliverySettings4DualSquareWave()
        With Me.RtbMainLeft
            .AppendKeyValue(
                key:="Dual:",
                value:=$"{_pdf.Bolus.EasyBolus.DualSquare.Dual,3}")

            .AppendKeyValue(
                key:="Square:",
                value:=$"{_pdf.Bolus.EasyBolus.DualSquare.Square,3}")
        End With
    End Sub

    Private Sub Settings2DeliverySettings5BolusIncrement()
        With Me.RtbMainLeft
            .AppendKeyValue(
                key:="Increment:",
                value:=$"{_pdf.Bolus.EasyBolus.BolusIncrement:F3} U")
        End With
    End Sub

    Private Sub Settings2DeliverySettings6BolusSpeed()
        With Me.RtbMainLeft
            .AppendKeyValue(
                key:="Bolus Speed:",
                value:=$"{_pdf.Bolus.EasyBolus.BolusSpeed}")
        End With
    End Sub

    Private Sub Settings2DeliverySettings7PresetBolusSetup()
        With Me.RtbMainLeft
            For Each item As KeyValuePair(Of String, PresetBolusRecord) In _pdf.PresetBolus
                .AppendTextWithFontChange(text:=$"{Indent4}{item.Key}:", newFont:=FixedWidthBoldFont)
                If item.Value.IsValid Then
                    Dim presetBolus As PresetBolusRecord = item.Value
                    Dim bolusType As String = If(presetBolus.BolusTypeNormal,
                                                 "Normal",
                                                 "Square")
                    .AppendTextWithFontChange(
                        text:=$"{Indent4}Bolus: {presetBolus.Bolus}{Indent4}Type: {bolusType}",
                        newFont:=FixedWidthFont)
                    If Not item.Value.BolusTypeNormal Then
                        .AppendTextWithFontChange(
                            text:=$"{Indent4}Duration: {presetBolus.Duration} hr",
                            newFont:=FixedWidthFont)
                    End If
                End If
                .AppendNewLine
            Next

        End With
    End Sub

    Private Sub Settings2DeliverySettings8PresetTempSetup()
        With Me.RtbMainLeft
            For Each item As KeyValuePair(Of String, PresetTempRecord) In _pdf.PresetTemp

                .AppendTextWithFontChange(text:=$"{Indent4}{item.Key}:", newFont:=FixedWidthBoldFont)
                Dim presetTempRecord As PresetTempRecord = item.Value

                If presetTempRecord.IsValid Then
                    .AppendTextWithFontChange(
                        text:=$"{Indent4}{Indent4}{presetTempRecord.PresetAmount}",
                        newFont:=FixedWidthFont)
                    .AppendTextWithFontChange(
                        text:=$"{Indent4}Duration:{Indent4}{presetTempRecord.Duration.ToFormattedTimeSpan(unit:="U/hr").Trim}",
                        newFont:=FixedWidthFont,
                        includeNewLine:=True)
                Else
                    .AppendNewLine
                End If
            Next
            .AppendNewLine
        End With
    End Sub

    Private Sub Settings2DeviceSettings()
        Me.RtbMainRight.AppendKeyValue(key:=$"Sensor:", value:=$"{_pdf.Sensor.SensorOn}")
    End Sub

End Class
