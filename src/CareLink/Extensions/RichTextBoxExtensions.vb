' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module RichTextBoxExtensions

    <Extension>
    Public Sub AppendTextWithFontAndColor(rtb As RichTextBox, newText As String, newFont As Font, Optional newColor? As Color = Nothing, Optional appendNewLine As Boolean = True)
        rtb.Select(rtb.Text.Length, 0)
        rtb.SelectionFont = newFont
        If newColor IsNot Nothing Then
            rtb.SelectionColor = CType(newColor, Color)
        End If
        rtb.SelectedText = $"{newText}{If(appendNewLine, s_environmentNewLine, "")}"
    End Sub

End Module
