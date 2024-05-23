' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module NearestKnownColor

    Private Function HsbDifference(color1 As Color, color2 As Color) As Single
        Dim h, s, b As Single
        h = Math.Abs(color1.GetHue - color2.GetHue)
        s = Math.Abs(color1.GetSaturation - color2.GetSaturation)
        b = Math.Abs(color1.GetBrightness - color2.GetBrightness)
        Return h + s + b
    End Function

    Private Function RgbDifference(color1 As Color, color2 As Color) As Integer
        Dim red As Integer = Math.Abs(color1.R - CInt(color2.R))
        Dim green As Integer = Math.Abs(color1.G - CInt(color2.G))
        Dim blue As Integer = Math.Abs(color1.B - CInt(color2.B))
        Return CInt((red ^ 2) + (green ^ 2) + (blue ^ 2))
    End Function

    Public Function GetNearestKnownColor(red As Integer, green As Integer, blue As Integer, Optional excludeSystemColors As Boolean = True) As KnownColor
        Return GetNearestKnownColor(Color.FromArgb(red, green, blue), excludeSystemColors)
    End Function

    Public Function GetNearestKnownColor(col As Color, Optional excludeSystemColors As Boolean = True) As KnownColor
        Dim rgbList As New SortedList(Of Long, KnownColor)
        Dim rgb As Integer, hsb As Single, kColor As Color
        For Each known As KnownColor In [Enum].GetValues(Of KnownColor)
            kColor = Color.FromKnownColor(known)
            If Not excludeSystemColors OrElse Not kColor.IsSystemColor Then
                rgb = RgbDifference(kColor, col)
                If Not rgbList.ContainsKey(rgb) Then
                    rgbList.Add(rgb, known)
                End If
            End If
        Next
        Dim hsbList As New SortedList(Of Single, KnownColor)
        For i As Integer = 0 To 4
            kColor = Color.FromKnownColor(rgbList.Values(i))
            hsb = HsbDifference(col, kColor)
            If Not hsbList.ContainsKey(hsb) Then
                hsbList.Add(hsb, rgbList.Values(i))
            End If
        Next
        Return hsbList.Values(0)
    End Function

End Module
