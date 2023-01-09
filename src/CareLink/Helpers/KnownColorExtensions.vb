' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module KnownColorExtensions

    Private Function HSBdiff(col1 As Color, col2 As Color) As Single
        Dim h, s, b As Single
        h = Math.Abs(col1.GetHue - col2.GetHue)
        s = Math.Abs(col1.GetSaturation - col2.GetSaturation)
        b = Math.Abs(col1.GetBrightness - col2.GetBrightness)
        Return h + s + b
    End Function

    Private Function RGBdiff(col1 As Color, col2 As Color) As Integer
        Dim red As Integer = Math.Abs(col1.R - CInt(col2.R))
        Dim green As Integer = Math.Abs(col1.G - CInt(col2.G))
        Dim blue As Integer = Math.Abs(col1.B - CInt(col2.B))
        Return CInt((red ^ 2) + (green ^ 2) + (blue ^ 2))
    End Function

    Public Function GetNearestKnownColor(red As Integer, green As Integer, blue As Integer, Optional excludeSystemColors As Boolean = True) As KnownColor
        Return GetNearestKnownColor(Color.FromArgb(red, green, blue), excludeSystemColors)
    End Function

    Public Function GetNearestKnownColor(col As Color, Optional excludeSystemColors As Boolean = True) As KnownColor
        Dim rgblist As New SortedList(Of Long, KnownColor)
        Dim rgb As Integer, hsb As Single, kcol As Color
        For Each known As KnownColor In [Enum].GetValues(GetType(KnownColor))
            kcol = Color.FromKnownColor(known)
            If Not excludeSystemColors OrElse Not kcol.IsSystemColor Then
                rgb = RGBdiff(kcol, col)
                If Not rgblist.ContainsKey(rgb) Then
                    rgblist.Add(rgb, known)
                End If
            End If
        Next
        Dim hsblist As New SortedList(Of Single, KnownColor)
        For i As Integer = 0 To 4
            kcol = Color.FromKnownColor(rgblist.Values(i))
            hsb = HSBdiff(col, kcol)
            If Not hsblist.ContainsKey(hsb) Then
                hsblist.Add(hsb, rgblist.Values(i))
            End If
        Next
        Return hsblist.Values(0)
    End Function

    <Extension>
    Public Function ToColor(c As KnownColor) As Color
        Return Color.FromKnownColor(c)
    End Function

End Module
