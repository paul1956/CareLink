' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module OptionsDialogHelpers

    Public Function CKvp(selectedItem As Object) As KeyValuePair(Of String, KnownColor)
        Return DirectCast(selectedItem, KeyValuePair(Of String, KnownColor))
    End Function

    <Extension>
    Public Function GetContrastingColor(baseColor As Color) As Color
        ' Y is the "brightness"
        Dim y As Double = (0.299 * baseColor.R) + (0.587 * baseColor.G) + (0.114 * baseColor.B)
        If y < 140 Then
            Return Color.White
        Else
            Return Color.Black
        End If
    End Function

    <Extension>
    Public Function GetContrastingKnownColor(knownClrBase As KnownColor) As KnownColor
        Dim clrBase As Color = knownClrBase.ToColor
        ' Y is the "brightness"
        Dim y As Double = (0.299 * clrBase.R) + (0.587 * clrBase.G) + (0.114 * clrBase.B)
        If y < 140 Then
            Return KnownColor.White
        Else
            Return KnownColor.Black
        End If
    End Function

End Module
