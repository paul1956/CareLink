' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Public Module RichTextBoxExtensions
    Private Const TotalWidth As Integer = 28
    Public Const Indent4 As String = "    "
    Public Const Indent8 As String = "        "

    Public ReadOnly Property FixedWidthBoldFont As New Font(familyName:="Consolas", emSize:=14, style:=FontStyle.Bold)

    Public ReadOnly Property FixedWidthFont As New Font(familyName:="Consolas", emSize:=14, style:=FontStyle.Regular)

    Public ReadOnly Property HeadingBoldFont As New Font(familyName:="Segoe UI", emSize:=16, style:=FontStyle.Bold)

    Public ReadOnly Property HeadingFont As New Font(familyName:="Segoe UI", emSize:=16, style:=FontStyle.Regular)

    ''' <summary>
    '''  Returns a string representation of a <see cref="TimeOnly"/> value,
    '''  padded to a standard width if necessary.
    ''' </summary>
    ''' <param name="tOnly">The <see cref="TimeOnly"/> value to format.</param>
    ''' <returns>A string representation of the time, padded to a standard width.</returns>
    ''' <param name="timeFormat"></param>
    <Extension>
    Private Function StandardWidth(tOnly As TimeOnly, timeFormat As String) As String
        If timeFormat = "12 Hr" Then
            ' Ensure the hour is always two digits in 12-hour format
            Return tOnly.ToString("hh:mm tt", provider:=CultureInfo.InvariantCulture)
        Else
            ' Ensure the hour is always two digits in 24-hour format
            Return tOnly.ToString("HH:mm", provider:=CultureInfo.InvariantCulture)
        End If
    End Function

    ''' <summary>
    '''  Returns a string representation of the given text, centered within
    '''  a specified total width.
    ''' </summary>
    ''' <param name="text">The text to center.</param>
    ''' <returns>A centered string representation of the text.</returns>
    <Extension>
    Friend Function AlignCenter(text As String) As String
        Dim pad As Integer = (TotalWidth - text.Length) \ 2
        Return If(pad > 0,
                  text.PadLeft(totalWidth:=text.Length + pad).PadRight(TotalWidth),
                  text.PadRight(TotalWidth))
    End Function

    ''' <summary>
    '''  Appends a text value pair to the current text in a <see cref="RichTextBox"/>,
    '''  with the text formatted in bold and the value in regular font.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append text to.
    ''' </param>
    ''' <param name="key">
    '''  The label or description text to append.
    ''' </param>
    ''' <param name="value">
    '''  The value associated with the label, which will be appended in regular font.
    ''' </param>
    ''' <param name="singleIndent"></param>
    <Extension>
    Friend Sub AppendKeyValue(rtb As RichTextBox, key As String, value As String, Optional indent As String = Indent4)
        rtb.AppendTextWithFontChange(text:=$"{indent}{key}", newFont:=FixedWidthBoldFont)
        rtb.AppendTextWithFontChange(text:=value.AlignCenter(), newFont:=FixedWidthFont, includeNewLine:=True)
    End Sub

    ''' <summary>
    '''  Appends line break to current text of a <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append a new line to.
    ''' </param>
    <Extension>
    Friend Sub AppendNewLine(rtb As RichTextBox)
        rtb.AppendText(text:=vbCrLf)
    End Sub

    ''' <summary>
    '''  Appends <paramref name="text"/> to current text of a <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append text to.
    ''' </param>
    ''' <param name="text">
    '''  The text to append.
    ''' </param>
    ''' <param name="newFont">
    '''  The <see cref="Font"/> to use for future appended text.
    ''' </param>
    <Extension>
    Friend Sub AppendTextWithFontChange(
        rtb As RichTextBox,
        text As String,
        newFont As Font,
        Optional padRight As Integer = TotalWidth,
        Optional includeNewLine As Boolean = False)

        Dim start As Integer = rtb.TextLength
        If padRight > 0 Then
            text = text.PadRight(totalWidth:=padRight)
        End If
        rtb.AppendText(text)
        rtb.Select(start, length:=text.Length)
        rtb.SelectionFont = newFont
        rtb.SelectionStart = rtb.TextLength
        rtb.SelectionLength = 0
        If includeNewLine Then
            rtb.AppendNewLine
        End If
    End Sub

    ''' <summary>
    '''  Appends text to current text in a <see cref="RichTextBox"/> with an option
    '''  to append a line break. The text is formatted with a specified <see cref="Font"/>,
    '''  and optionally highlights a specific substring with a different <see cref="Font"/>
    '''  and <see cref="Color"/>.
    ''' </summary>
    ''' <param name="rtb">The <see cref="RichTextBox"/> to append text to.</param>
    ''' <param name="text">The text to append.</param>
    ''' <param name="symbol">
    '''  The symbol to use to represent button to press with Gear, Shield, or other symbols.
    '''  Defaults to Gear symbol.
    ''' </param>
    ''' <param name="includeNewLine">
    '''  If <see langword="True"/>, appends a new line after the text.
    ''' </param>
    <Extension>
    Friend Sub AppendTextWithSymbol(
        rtb As RichTextBox,
        text As String,
        Optional symbol As String = Gear,
        Optional includeNewLine As Boolean = True)

        Dim splitText() As String = text.Split(separator:=symbol, options:=StringSplitOptions.None)
        rtb.AppendTextWithFontChange(text:=splitText(0), newFont:=HeadingBoldFont, padRight:=0)
        If splitText.Length > 1 Then
            Dim bufferLength As Integer = rtb.Text.Length
            rtb.AppendTextWithFontChange(text:=symbol, newFont:=HeadingBoldFont, padRight:=0)
            rtb.Select(start:=bufferLength, length:=symbol.Length)
            rtb.SelectionBackColor = SystemColors.Window
            Select Case symbol
                Case Gear
                    rtb.SelectionColor = Color.Yellow
                Case Shield
                    rtb.SelectionColor = Color.CadetBlue
                Case Else
                    rtb.SelectionColor = Color.HotPink
            End Select
            rtb.SelectionStart = rtb.Text.Length
            rtb.SelectionBackColor = SystemColors.Window
            rtb.SelectionColor = SystemColors.WindowText
            rtb.AppendTextWithFontChange(
                text:=splitText(1),
                newFont:=HeadingBoldFont,
                padRight:=0)
        End If
        If includeNewLine Then
            rtb.AppendNewLine
        End If
    End Sub

    ''' <summary>
    '''  Appends a time value row to the <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append the time value row to.
    ''' </param>
    ''' <param name="startTime">The start time of the row.</param>
    ''' <param name="endTime">The end time of the row.</param>
    ''' <param name="value">The value associated with the time range.</param>
    ''' <param name="timeFormat"></param>
    ''' <remarks>
    '''  The time values are formatted to a standard width for consistency.
    ''' </remarks>
    ''' <param name="indent">If true, applies a single indent to the row.</param>
    ''' <param name="heading"></param>
    <Extension>
    Friend Sub AppendTimeValueRow(
        rtb As RichTextBox,
        startTime As TimeOnly,
        endTime As TimeOnly,
        value As String,
        timeFormat As String,
        Optional indent As String = Indent8,
        Optional heading As Boolean = False)

        Dim startTimeStr As String = startTime.StandardWidth(timeFormat)
        Dim endTimeStr As String = endTime.StandardWidth(timeFormat)
        Dim timeRange As String = $"{startTimeStr} - {endTimeStr}"
        Dim newFont As Font = If(heading, FixedWidthBoldFont, FixedWidthFont)

        rtb.AppendTextWithFontChange(text:=$"{indent}{timeRange}", newFont)
        Dim text As String = $"{value}".AlignCenter()
        rtb.AppendTextWithFontChange(text, newFont:=FixedWidthFont, includeNewLine:=True)
    End Sub

    ' <summary>
    '''  Appends a time value row to the <see cref="RichTextBox"/> with start
    '''  and end times as strings.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append the time value row to.
    ''' </param>
    ''' <param name="key">The label for the time value row.</param>
    ''' <param name="startTime">The start time of the row as a string.</param>
    ''' <param name="endTime">The end time of the row as a string.</param>
    <Extension>
    Friend Sub AppendTimeValueRow(
        rtb As RichTextBox,
        key As String,
        startTime As String,
        Optional endTime As String = "")

        Dim text As String = $"{Indent4}{key}".PadRight(TotalWidth)
        rtb.AppendTextWithFontChange(text, newFont:=FixedWidthFont)

        Dim separator As String
        If endTime = String.Empty Then
            separator = String.Empty
        Else
            separator = "-"
            If startTime <> "Off" Then
                startTime = startTime.PadLeft(totalWidth:=8)
                endTime = endTime.PadLeft(totalWidth:=8)
            End If
        End If

        text = $"{startTime}{separator}{endTime}".AlignCenter()
        rtb.AppendTextWithFontChange(text, newFont:=FixedWidthFont, includeNewLine:=True)
    End Sub

    ''' <summary>
    '''  Makes all occurrences of a specified string in a <see cref="RichTextBox"/> bold.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to modify.
    ''' </param>
    ''' <param name="str">
    '''  The string to make bold.
    ''' </param>
    ''' <remarks>
    '''  This method searches for all occurrences of the specified string
    '''  and applies bold formatting to them.
    ''' </remarks>
    <Extension>
    Public Sub BoldText(
        rtb As RichTextBox,
        str As String,
        Optional options As RichTextBoxFinds = RichTextBoxFinds.MatchCase)

        Const newStyle As FontStyle = FontStyle.Bold
        Dim start As Integer = 0
        Dim length As Integer = str.Length
        While start < rtb.TextLength - 1
            Dim wordStartIndex As Integer = rtb.Find(str, start, [end]:=rtb.TextLength - 1, options)
            If wordStartIndex = -1 Then
                Exit While ' No more occurrences found
            Else
                rtb.Select(start:=wordStartIndex, length)
                rtb.SelectionFont = New Font(prototype:=rtb.SelectionFont, newStyle)
                start = wordStartIndex + length
            End If
        End While
        rtb.SelectionLength = 0
    End Sub

    ''' <summary>
    '''  Converts a boolean value to "On" or "Off" string representation.
    ''' </summary>
    ''' <param name="boolValue"></param>
    ''' <returns> "On" if true, "Off" if false</returns>
    <Extension>
    Friend Function BoolToOnOff(boolValue As Boolean) As String
        Return If(boolValue, "On", "Off")
    End Function

End Module
