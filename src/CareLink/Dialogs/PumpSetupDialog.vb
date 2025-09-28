' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Drawing.Printing

Public Class PumpSetupDialog

    Private _charFrom As Integer
    Private _charTo As Integer
    Private _printRtb As RichTextBox

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

                Dim autoSuspendTimeSpan As TimeSpan = Me.Pdf.Utilities.AutoSuspend.Time
                .AppendTextWithFontChange(
                    text:=$"{Indent4}{autoSuspendTimeSpan.ToFormattedTimeSpan(unit:="hr")}",
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
    ''' <param name="e">The <see cref="EventArgs"/>
    '''  instance containing the event data.
    ''' </param>
    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.PrintToolStripMenuItem.Enabled = False
        Application.DoEvents()
        Me.DialogResult = DialogResult.OK
        Me.PrintToolStripMenuItem.Enabled = True
        Me.Close()
    End Sub

    ''' <summary>
    '''  Prepare the RichTextBox for printing: white background and convert white text to black
    ''' </summary>
    Private Sub PreparePrintRichTextBox()
        Using tmpRtb As New RichTextBox With {.Rtf = Me.RtbMainLeft.Rtf}
            ' Move caret to end
            tmpRtb.Select(start:=tmpRtb.TextLength, length:=0)
            ' Append second RichTextBox content preserving formatting
            tmpRtb.SelectedRtf = Me.RtbMainRight.Rtf
            _printRtb = New RichTextBox With {.Rtf = tmpRtb.Rtf, .BackColor = Color.White}
        End Using

        Dim start As Integer = 0
        While start < _printRtb.TextLength
            _printRtb.Select(start, length:=1)
            Dim selBack As Color = _printRtb.SelectionBackColor
            Dim selColor As Color = _printRtb.SelectionColor

            ' Adjust colors for printing
            If selBack = SystemColors.Window Then
                ' Background is white; no change needed
            Else
                ' If you want to enforce white background
                _printRtb.SelectionBackColor = Color.White
            End If

            If selColor.ToArgb = SystemColors.GrayText.ToArgb Then
                _printRtb.SelectionColor = Color.Black
            ElseIf selColor.ToArgb = SystemColors.WindowText.ToArgb Then
                _printRtb.SelectionColor = Color.Black
            ElseIf selColor.ToArgb = Color.White.ToArgb Then
                ' If text is white (dark mode), convert to black for printing
                _printRtb.SelectionColor = Color.Black
            End If

            start += 1
        End While
        Me.ReduceFontSizes(reductionFactor:=0.53)
    End Sub

    ' PrintPage event handler: render RichTextBox content to graphics page
    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        Dim printArea As STRUCT_RECT
        ' 14.4 = pixels to twips conversion (96 dpi * 15)
        printArea.Top = CInt(e.MarginBounds.Top * 14.4)
        printArea.Bottom = CInt(e.MarginBounds.Bottom * 14.4)
        printArea.Left = CInt(e.MarginBounds.Left * 14.4)
        printArea.Right = CInt(e.MarginBounds.Right * 14.4)

        Dim pageArea As STRUCT_RECT
        pageArea.Top = CInt(e.PageBounds.Top * 14.4)
        pageArea.Bottom = CInt(e.PageBounds.Bottom * 14.4)
        pageArea.Left = CInt(e.PageBounds.Left * 14.4)
        pageArea.Right = CInt(e.PageBounds.Right * 14.4)

        Dim fmtRange As STRUCT_FORMATRANGE
        fmtRange.chrg.cpMin = _charFrom
        fmtRange.chrg.cpMax = _charTo
        fmtRange.hdc = e.Graphics.GetHdc()
        fmtRange.hdcTarget = fmtRange.hdc
        fmtRange.rc = printArea
        fmtRange.rcPage = pageArea

        Dim res As IntPtr =
            SendMessage(hWnd:=_printRtb.Handle, msg:=EM_FORMATRANGE, wParam:=CType(1, IntPtr), lParam:=fmtRange)
        e.Graphics.ReleaseHdc(fmtRange.hdc)

        Dim charsPrinted As Integer = res.ToInt32()
        If charsPrinted < _charTo Then
            _charFrom = charsPrinted
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            ' Free cached memory after printing
            SendMessage(hWnd:=_printRtb.Handle, msg:=EM_FORMATRANGE, wParam:=CType(0, IntPtr), lParam:=fmtRange)
        End If
    End Sub

    Private Sub PrintRichTextBox()
        Me.PreparePrintRichTextBox()

        Using pd As New PrintDocument()
            AddHandler pd.PrintPage, AddressOf Me.PrintPageHandler

            Using printDialog As New PrintDialog()
                printDialog.Document = pd
                printDialog.AllowSomePages = True
                printDialog.AllowSelection = False
                printDialog.UseEXDialog = True

                If printDialog.ShowDialog() = DialogResult.OK Then
                    pd.PrinterSettings = printDialog.PrinterSettings
                    _charFrom = 0
                    _charTo = _printRtb.TextLength
                    Try
                        pd.Print()
                    Catch ex As Exception
                        ' ignore printing errors
                    End Try
                End If
            End Using
        End Using
    End Sub

    Private Sub PrintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintToolStripMenuItem.Click

        Me.PrintRichTextBox()
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
    Private Sub PumpSetupDialog_Shown(sender As Object, e1 As EventArgs) Handles MyBase.Shown

        If Me.Pdf Is Nothing Then
            Throw New NullReferenceException(message:=NameOf(Pdf))
        End If

        If Not Me.Pdf.IsValid Then
            Throw New InvalidOperationException(message:="The PDF settings record is not valid.")
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

            value =
                $"{Me.Pdf.SmartGuard.Target.RoundToSingle(digits:=0, considerValue:=True)}"

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
                value:=$"{audioOptions.ContainsNoCase(value:="Audio").BoolToOnOff()}")
            .AppendKeyValue(
                key:="Vibration:",
                value:=$"{audioOptions.ContainsNoCase(value:="Vibrate").BoolToOnOff()}")

            .ReadOnly = True
            .SelectionStart = 0
        End With
    End Sub

    Private Sub ReduceFontSizes(reductionFactor As Single)
        Dim start As Integer = 0
        Dim textLength As Integer = _printRtb.TextLength

        While start < textLength
            _printRtb.Select(start, length:=1)
            Dim currentFont As Font = _printRtb.SelectionFont

            ' Find the range with the same font
            Dim endIndex As Integer = start + 1
            While endIndex < textLength
                _printRtb.Select(start:=endIndex, length:=1)
                If _printRtb.SelectionFont Is Nothing OrElse
                    Not _printRtb.SelectionFont.Equals(obj:=currentFont) Then
                    Exit While
                End If
                endIndex += 1
            End While

            ' Select the whole range with the same font
            _printRtb.Select(start, length:=endIndex - start)

            ' Create and apply new font with reduced size
            Dim newFont As New Font(
                family:=currentFont.FontFamily,
                emSize:=Math.Max(1.0F, currentFont.Size * reductionFactor),
                style:=currentFont.Style)
            _printRtb.SelectionFont = newFont

            start = endIndex
        End While

        _printRtb.DeselectAll()
    End Sub

    Private Sub SettingsAlert(rtb As RichTextBox)
        With rtb
            For Each item As KeyValuePair(Of String, NamedBasalRecord) In Me.Pdf.Basal.NamedBasal
                Dim text As String = $"{Indent4}{item.Key}:"
                .AppendTextWithFontChange(text, newFont:=FixedWidthBoldFont, includeNewLine:=True)
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

    ''' <summary>
    '''  Overrides the OnHandleCreated method to enable dark mode
    '''  for the dialog when its handle is created.
    ''' </summary>
    ''' <param name="e">The event data.</param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        EnableDarkMode(hwnd:=Me.Handle)
    End Sub

End Class
