' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module RichTextBoxExtensions

    ''' <summary>
    '''  Appends line break to current text of a <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append a new line to.
    ''' </param>
    <Extension>
    Public Sub AppendNewLine(rtb As RichTextBox)
        rtb.AppendText(text:=vbCrLf)
    End Sub

    ''' <summary>
    '''  Appends text to current text in a <see cref="RichTextBox"/> with an option to append a line break.
    '''  The text is formatted with a specified <see cref="Font"/>, and optionally highlights a specific substring
    '''  with a different <see cref="Font"/> and <see cref="Color"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append text to.
    ''' </param>
    ''' <param name="text">
    '''  The text to append.
    ''' </param>
    ''' <param name="includeNewLine">
    '''  If <see langword="True"/>, appends a new line after the text.
    ''' </param>
    <Extension>
    Public Sub AppendTexWithGear(
        rtb As RichTextBox,
        text As String,
        Optional includeNewLine As Boolean = True)

        Dim subheadingBoldtFont As New Font(familyName:="Tahoma", emSize:=16, style:=FontStyle.Bold)
        Dim subheadingFont As New Font(familyName:="Tahoma", emSize:=16, style:=FontStyle.Regular)

        Dim splitText() As String = text.Split(separator:=Gear, options:=StringSplitOptions.None)
        rtb.AppendTextWithFontChange(text:=splitText(0), newFont:=subheadingFont)
        If splitText.Length > 1 Then
            Dim bufferLength As Integer = rtb.Text.Length
            rtb.AppendTextWithFontChange(text:=Gear, newFont:=subheadingFont)
            rtb.Select(bufferLength, length:=Gear.Length)
            rtb.SelectionBackColor = Color.Black
            rtb.SelectionColor = Color.Yellow
            rtb.SelectionStart = rtb.Text.Length
            rtb.SelectionBackColor = SystemColors.Window
            rtb.SelectionColor = SystemColors.WindowText
            rtb.AppendTextWithFontChange(text:=splitText(1), newFont:=subheadingFont)
        End If
        If includeNewLine Then
            rtb.AppendNewLine
        End If
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
    Public Sub AppendTextWithFontChange(
        rtb As RichTextBox,
        text As String,
        newFont As Font,
        Optional includeNewLine As Boolean = False)

        Dim bufferLength As Integer = rtb.TextLength
        rtb.AppendText(text)
        rtb.Select(bufferLength, length:=text.Length)
        rtb.SelectionFont = newFont
        rtb.SelectionStart = rtb.TextLength
        rtb.SelectionLength = 0
        If includeNewLine Then
            rtb.AppendNewLine
        End If
    End Sub

End Module
