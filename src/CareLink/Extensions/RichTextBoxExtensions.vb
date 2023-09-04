' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module RichTextBoxExtensions

    ''' <summary>
    ''' Appends NewLine to current text of a text box.
    ''' </summary>
    ''' <param name="rtb"></param>
    <Extension>
    Public Sub AppendLine(rtb As RichTextBox)
        rtb.AppendText(Environment.NewLine)
    End Sub

    ''' <summary>
    ''' Appends text to current text of a text box. Followed by a Environment.NewLine
    ''' </summary>
    ''' <param name="rtb"></param>
    ''' <param name="text"></param>
    ''' <param name="newFont"></param>
    <Extension>
    Public Sub AppendLine(rtb As RichTextBox, text As String, newFont As Font, Optional highlightText As String = "", Optional highlightFont As Font = Nothing, Optional includeNewLine As Boolean = True)
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
            rtb.AppendText(Environment.NewLine)
        End If
    End Sub

    ''' <summary>
    ''' Appends text to current text of a text box.
    ''' </summary>
    ''' <param name="rtb"></param>
    <Extension>
    Public Sub AppendTab(rtb As RichTextBox, tabColor As Color)
        Dim bufferLength As Integer = rtb.TextLength
        rtb.AppendText(vbTab)
        rtb.Select(bufferLength, vbTab.Length)
        rtb.SelectionBackColor = tabColor
        rtb.Select(rtb.TextLength, 0)
        rtb.SelectionBackColor = SystemColors.Window
    End Sub

    ''' <summary>
    ''' Appends text to current text of a text box.
    ''' </summary>
    ''' <param name="rtb"></param>
    ''' <param name="text"></param>
    ''' <param name="newFont">New Font</param>
    <Extension>
    Public Sub AppendText(rtb As RichTextBox, text As String, newFont As Font)
        Dim bufferLength As Integer = rtb.TextLength
        rtb.AppendText(text)
        rtb.Select(bufferLength, text.Length)
        rtb.SelectionFont = newFont
    End Sub

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
