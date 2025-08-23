' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PumpSetupDialog

    ''' <summary>
    '''  Sets the PDF settings record used to populate the dialog.
    ''' </summary>
    ''' <value>The <see cref="PdfSettingsRecord"/> containing pump configuration data.</value>
    Public Property Pdf As PdfSettingsRecord

    Private Sub DeliverySettingsAutoSuspend(rtb As RichTextBox)
        With rtb
            .AppendKeyValue(key:="Alarm:", value:=Me.Pdf.Utilities.AutoSuspend.Alarm)
            Dim bufferLength As Integer = .Text.Length
            .AppendTextWithFontChange(text:=$"{Indent4}Time:", newFont:=FixedWidthBoldFont)
            If Me.Pdf.Utilities.AutoSuspend.Alarm = "Off" Then
                .AppendTextWithFontChange(
                    text:="12:00 hr".AlignCenter,
                    newFont:=FixedWidthFont,
                    includeNewLine:=True)
                .Select(start:=bufferLength, length:= .Text.Length - bufferLength)
                .SelectionBackColor = SystemColors.Window
                .SelectionColor = SystemColors.GrayText
                .SelectionStart = .Text.Length
                .SelectionBackColor = SystemColors.Window
                .SelectionColor = SystemColors.WindowText
            Else
                .AppendTextWithFontChange(
                    text:=$"{Indent4}{Me.Pdf.Utilities.AutoSuspend.Time.ToFormattedTimeSpan(unit:="hr")}",
                    newFont:=FixedWidthFont,
                    includeNewLine:=True)
            End If
        End With
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

    Private Sub PumpSetupDialog_Load(sender As Object, e As EventArgs) Handles Me.Load

        With Me.RtbMainLeft
            .ReadOnly = False
            .AppendTextWithSymbol(text:=$"Menu>{Gear}>Delivery Settings")
        End With

        With Me.RtbMainRight
            .ReadOnly = False
            .AppendTextWithSymbol(text:=$"Menu>{Gear}>Alert Settings")
        End With
    End Sub

    ''' <summary>
    '''  Handles the dialog's <see cref="Form.Shown"/> event.
    '''  Populates all UI controls with data from the <see cref="Pdf"/> settings record.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e1">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <exception cref="NullReferenceException">
    '''  Thrown if <see cref="Pdf"/> is not set.
    ''' </exception>
    Private Sub PumpSetupDialog_Shown(sender As Object, e1 As EventArgs) _
        Handles MyBase.Shown

        If Me.Pdf Is Nothing Then
            Throw New NullReferenceException(message:=NameOf(Pdf))
        End If

        If Not Me.Pdf.IsValid Then
            Const message As String = "The PDF settings record is not valid."
            Throw New InvalidOperationException(message:=message)
        End If

        Me.Text = $"Pump Setup Instructions For {Me.Pdf.UserName}"
        Application.DoEvents()

        Dim rtb As RichTextBox = Me.RtbMainLeft
        With rtb
            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Bolus Wizard Setup")
            rtb.DeliverySettings1BolusWizardSetup(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Basal Pattern Setup")
            rtb.DeliverySettings2BasalPatternSetup(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Max Basal/Bolus")
            rtb.DeliverySettings3MaxBasalBolus(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Dual/Square Wave")
            rtb.DeliverySettings4DualSquareWave(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Bolus Increment")
            rtb.DeliverySettings5BolusIncrement(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Bolus Speed")
            rtb.DeliverySettings6BolusSpeed(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Preset Bolus Setup")
            rtb.DeliverySettings7PresetBolusSetup(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Preset Temp Setup")
            rtb.DeliverySettings8PresetTempSetup(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Delivery Settings > Auto Suspend")
            Me.DeliverySettingsAutoSuspend(rtb)
            .AppendNewLine

            .ReadOnly = True
            .SelectionStart = 0
        End With

        rtb = Me.RtbMainRight
        Dim symbol As String
        With rtb
            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Alert Settings > High Alert")
            .AlertSettings1HighAlert(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Alert Settings > Low Alert")
            .AlertSettings2LowAlert(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Alert Settings > Snooze High & Low")
            .AppendKeyValue(
                key:="High Snooze:",
                value:=$"{Me.Pdf.HighAlerts}")

            .AppendKeyValue(
                key:="Low Snooze:",
                value:=$"{Me.Pdf.LowAlerts}")
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Alert Settings>Reminders > Low Reservoir")
            .AlertSettings4Reminders(Me.Pdf)

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Device Settings")
            .AppendKeyValue(key:=$"Sensor:", value:=$"{Me.Pdf.Sensor.SensorOn}")
            .AppendNewLine

            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Device Settings > Time & Date")
            .AppendKeyValue(key:="Time Format:", value:=Me.Pdf.Utilities.TimeFormat)

            .AppendNewLine
            .AppendTextWithSymbol(text:=$"Menu>{Gear}>Device Settings > Display")
            .AppendKeyValue(key:="Brightness:", value:=Me.Pdf.Utilities.Brightness)
            Dim value As String =
                Me.Pdf.Utilities.BackLightTimeout.ToFormattedTimeSpan(unit:="min")
            .AppendKeyValue(key:="Backlight:", value)

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu>{Gear}>Device Settings > Easy Bolus")
            .AppendKeyValue(key:="Easy Bolus:", value:=Me.Pdf.Bolus.EasyBolus.EasyBolus)
            .AppendKeyValue(
                key:="Step Size: ",
                value:=$"{Me.Pdf.Bolus.EasyBolus.BolusIncrement} U")
            .AppendNewLine

            symbol = Shield
            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu>{Shield}>SmartGuard > SmartGuard Settings", symbol)

            value = $"{Me.Pdf.SmartGuard.Target.RoundToSingle(digits:=0, considerValue:=True)}"
            .AppendKeyValue(key:="Target:", value)
            .AppendKeyValue(key:="Auto Correction:", value:=$"{Me.Pdf.SmartGuard.SmartGuard}")

            .AppendNewLine
            .AppendTextWithSymbol(
                text:=$"Menu>{Shield}>SmartGuard", symbol)
            .AppendKeyValue(
                key:="SmartGuard:",
                value:=$"{Me.Pdf.SmartGuard.AutoCorrection}")

            .AppendNewLine
            symbol = "🔊"
            .AppendTextWithSymbol(
                text:=$"Menu>{"🔊"}>Sound & Vibration",
                symbol)
            .AppendKeyValue(
                key:="Volume:",
                value:=$"{Me.Pdf.Utilities.AlarmVolume}")

            Dim audioOptions As String = Me.Pdf.Utilities.AudioOptions
            .AppendKeyValue(
                key:="Sound:",
                value:=$"{audioOptions.ContainsIgnoreCase(value:="Audio").BoolToOnOff()}")
            .AppendKeyValue(
                key:="Vibration:",
                value:=$"{audioOptions.ContainsIgnoreCase(value:="Vibrate").BoolToOnOff()}")

            .ReadOnly = True
            .SelectionStart = 0
        End With
    End Sub

    Private Sub SettingsAlert(rtb As RichTextBox)
        With rtb
            For Each index As IndexClass(Of KeyValuePair(Of String, NamedBasalRecord)) In
                Me.Pdf.Basal.NamedBasal.WithIndex

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
                    .AppendTimeValueRow(startTime, endTime, value, Me.Pdf.Utilities.TimeFormat)
                Next
            Next
        End With
    End Sub

End Class
