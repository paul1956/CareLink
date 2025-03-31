' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ColorHelpers

    <Extension>
    Public Function GetContrastingColor(baseColor As Color) As Color
        ' Y is the "brightness"
        Dim y As Double = (0.299 * baseColor.R) + (0.587 * baseColor.G) + (0.114 * baseColor.B)
        Return If(y < 140,
                  Color.White,
                  Color.Black
                 )
    End Function

    Public Function GetContrastingKnownColor(knownClrBase As KnownColor) As KnownColor
        Dim clrBase As Color = knownClrBase.ToColor
        ' Y is the "brightness"
        Dim y As Double = (0.299 * clrBase.R) + (0.587 * clrBase.G) + (0.114 * clrBase.B)
        Return If(y < 140,
                  KnownColor.White,
                  KnownColor.Black
                 )
    End Function

    <Extension>
    Public Function InvertColor(myColor As Color) As Color
        Return Color.FromArgb(myColor.ToArgb() Xor &HFFFFFF)
    End Function

    Public Function IsDarkMode() As Boolean
#Disable Warning WFO5001 ' Type is for evaluation purposes only and is subject to change or removal in future updates.
        Return Application.ColorMode = SystemColorMode.Dark
#Enable Warning WFO5001

    End Function

End Module
