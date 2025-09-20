' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ColorHelpers

    ''' <summary>
    '''  Returns a contrasting color (black or white) based on the brightness
    '''  of the base color. This is useful for ensuring text is readable against
    '''  a colored background.
    ''' </summary>
    ''' <param name="baseColor">The base color to evaluate.</param>
    ''' <returns>A contrasting color based on the brightness.</returns>
    <Extension>
    Public Function ContrastingColor(baseColor As Color) As Color
        ' Y is the "brightness"
        Dim y As Double = (0.299 * baseColor.R) + (0.587 * baseColor.G) + (0.114 * baseColor.B)
        Return If(y < 140,
                  Color.White,
                  Color.Black)
    End Function

    ''' <summary>
    '''  Returns a contrasting color (black or white) based on the brightness
    '''  of the specified KnownColor. This is useful for ensuring text is readable
    '''  against a colored background.
    ''' </summary>
    ''' <param name="knownColor">The KnownColor to evaluate.</param>
    ''' <returns>A contrasting KnownColor based on the brightness.</returns>
    Public Function GetContrastingKnownColor(knownColor As KnownColor) As KnownColor
        Dim clrBase As Color = knownColor.ToColor
        ' Y is the "brightness"
        Dim y As Double = (0.299 * clrBase.R) + (0.587 * clrBase.G) + (0.114 * clrBase.B)
        Return If(y < 140,
                  KnownColor.White,
                  KnownColor.Black)
    End Function

    ''' <summary>
    '''  Returns a text color based on the row's background color.
    '''  If the text color and the row's background are both dark or both light,
    '''  the text color is inverted for contrast.
    '''  Otherwise, the original text color is returned.
    ''' </summary>
    ''' <param name="row">The <see cref="DataGridViewRow"/> to evaluate.</param>
    ''' <param name="textColor">The base <see cref="Color"/> to use for text.</param>
    ''' <returns>
    '''  The appropriate <see cref="Color"/> for text to ensure readability
    '''  against the row's background.
    ''' </returns>
    <Extension>
    Public Function GetTextColor(row As DataGridViewRow, textColor As Color) As Color
        Return If(textColor.IsDarkColor() = row.IsDarkRow(),
                  textColor.InvertColor,
                  textColor)
    End Function

    ''' <summary>
    '''  Determines if the row's background color is dark.
    ''' </summary>
    ''' <param name="row">The <see cref="DataGridViewRow"/> to evaluate.</param>
    ''' <returns>
    '''  <see langword="True"/> if the row's background color is dark;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function InvertColor(myColor As Color) As Color
        Return Color.FromArgb(myColor.ToArgb() Xor &HFFFFFF)
    End Function

    ''' <summary>
    '''  Determines if the specified color is dark based on its RGB values.
    '''  This method uses the luminance formula to determine if a color is dark.
    ''' </summary>
    ''' <param name="backColor">The <see cref="Color"/> to evaluate.</param>
    ''' <returns><see langword="True"/> if the color is dark;
    ''' otherwise, <see langword="False"/>.</returns>
    <Extension>
    Public Function IsDarkColor(backColor As Color) As Boolean
        Return (0.2126 * backColor.R) + (0.7152 * backColor.G) + (0.0722 * backColor.B) < 128
    End Function

End Module
