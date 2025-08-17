' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SupportExtensions
    ''' <summary>
    '''  Makes all occurrences of a specified string in a <see cref="RichTextBox"/> bold.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to modify.
    ''' </param>
    ''' <param name="text">
    '''  The string to make bold.
    ''' </param>
    ''' <remarks>
    '''  This method searches for all occurrences of the specified string and applies bold formatting to them.
    ''' </remarks>
    <Extension>
    Public Sub BoldText(rtb As RichTextBox, text As String)
        Const newStyle As FontStyle = FontStyle.Bold
        Dim start As Integer = 0
        Dim length As Integer = text.Length
        While start < rtb.TextLength
            Dim wordStartIndex As Integer =
                rtb.Find(
                    str:=text,
                    start,
                    [end]:=rtb.TextLength,
                    options:=RichTextBoxFinds.MatchCase)
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

End Module
