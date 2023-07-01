' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ColorHelpers

    <Extension>
    Public Function GetContrastingColor(baseColor As Color) As Color
        ' Y is the "brightness"
        Dim y As Double = (0.299 * baseColor.R) + (0.587 * baseColor.G) + (0.114 * baseColor.B)
        Return If(y < 140,
                  Color.White,
                  Color.Black
                 )
    End Function

End Module
