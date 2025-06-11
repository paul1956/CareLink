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
        rtb.AppendText(vbCrLf)
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
    ''' <param name="newFont">
    '''  The <see cref="Font"/> to use for the appended text.
    ''' </param>
    ''' <param name="highlightText">
    '''  The substring to highlight within the appended text.
    ''' </param>
    ''' <param name="highlightFont">
    '''  The <see cref="Font"/> to use for the highlighted substring.
    ''' </param>
    ''' <param name="includeNewLine">
    '''  If <see langword="True"/>, appends a new line after the text.
    ''' </param>
    <Extension>
    Public Sub AppendLine(
        rtb As RichTextBox,
        text As String,
        newFont As Font,
        Optional highlightText As String = "",
        Optional highlightFont As Font = Nothing,
        Optional includeNewLine As Boolean = True)

        Dim splitText() As String = text.Split(highlightText)
        rtb.AppendText(splitText(0), newFont)
        If splitText.Length > 1 Then
            Dim bufferLength As Integer = rtb.Text.Length
            rtb.AppendText(highlightText, highlightFont)
            rtb.Select(bufferLength, 1)
            rtb.SelectionBackColor = Color.Black
            rtb.SelectionColor = Color.Yellow
            rtb.SelectionStart = rtb.Text.Length
            rtb.SelectionBackColor = SystemColors.Window
            rtb.SelectionColor = SystemColors.WindowText
            rtb.AppendText(splitText(1), newFont)
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
    '''  The <see cref="Font"/> to use for the appended text.
    ''' </param>
    <Extension>
    Public Sub AppendText(rtb As RichTextBox, text As String, newFont As Font)
        Dim bufferLength As Integer = rtb.TextLength
        rtb.AppendText(text)
        rtb.Select(bufferLength, text.Length)
        rtb.SelectionFont = newFont
    End Sub

    ''' <summary>
    '''  Appends <paramref name="text"/> to current text of a <see cref="RichTextBox"/> with a specified <see cref="Font"/> and
    '''  <see cref="Color"/> and optional line break.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append text to.
    ''' </param>
    ''' <param name="text">
    '''  The text to append.
    ''' </param>
    ''' <param name="newFont">
    '''  The <see cref="Font"/> to use for the appended text.
    ''' </param>
    ''' <param name="newColor">
    '''  The <see cref="Color"/> to use for the appended text.
    ''' </param>
    ''' <param name="appendNewLine">
    '''  If <see langword="True"/>, appends a new line after the text.
    ''' </param>
    <Extension>
    Public Sub AppendTextWithFontAndColor(rtb As RichTextBox, text As String, newFont As Font, Optional newColor? As Color = Nothing, Optional appendNewLine As Boolean = True)
        Dim bufferLength As Integer = rtb.TextLength
        rtb.AppendText(text)
        rtb.Select(bufferLength, text.Length)
        rtb.SelectionFont = newFont
        If newColor IsNot Nothing Then
            rtb.SelectionColor = CType(newColor, Color)
        End If
        If appendNewLine Then
            rtb.AppendText(vbCrLf)
        End If
    End Sub

End Module
