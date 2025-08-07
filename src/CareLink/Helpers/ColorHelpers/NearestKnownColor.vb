' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides helper methods for finding the nearest <see cref="KnownColor"/> to a given <see cref="Color"/>.
''' </summary>
Public Module NearestKnownColor

    ''' <summary>
    '''  Calculates the difference between two colors in HSB (Hue, Saturation, Brightness) space.
    ''' </summary>
    ''' <param name="color1">The first color to compare.</param>
    ''' <param name="color2">The second color to compare.</param>
    ''' <returns>
    '''  The sum of the absolute differences of hue, saturation, and brightness between the two colors.
    ''' </returns>
    Private Function HsbDifference(color1 As Color, color2 As Color) As Single
        Dim h, s, b As Single
        h = Math.Abs(value:=color1.GetHue - color2.GetHue)
        s = Math.Abs(value:=color1.GetSaturation - color2.GetSaturation)
        b = Math.Abs(value:=color1.GetBrightness - color2.GetBrightness)
        Return h + s + b
    End Function

    ''' <summary>
    '''  Calculates the squared difference between two colors in RGB space.
    ''' </summary>
    ''' <param name="color1">The first color to compare.</param>
    ''' <param name="color2">The second color to compare.</param>
    ''' <returns>
    '''  The sum of the squares of the differences of the red, green, and blue components.
    ''' </returns>
    Private Function RgbDifference(color1 As Color, color2 As Color) As Integer
        Dim red As Integer = Math.Abs(value:=color1.R - CInt(color2.R))
        Dim green As Integer = Math.Abs(value:=color1.G - CInt(color2.G))
        Dim blue As Integer = Math.Abs(value:=color1.B - CInt(color2.B))
        Return CInt((red ^ 2) + (green ^ 2) + (blue ^ 2))
    End Function

    ''' <summary>
    '''  Finds the nearest <see cref="KnownColor"/> to the specified RGB values.
    ''' </summary>
    ''' <param name="red">The red component (0-255).</param>
    ''' <param name="green">The green component (0-255).</param>
    ''' <param name="blue">The blue component (0-255).</param>
    ''' <param name="excludeSystemColors">
    '''  If <see langword="True"/>, system colors are excluded from the search. Default is <see langword="True"/>.
    ''' </param>
    ''' <returns>
    '''  The <see cref="KnownColor"/> that is closest to the specified RGB values.
    ''' </returns>
    Public Function GetNearestKnownColor(red As Integer, green As Integer, blue As Integer, Optional excludeSystemColors As Boolean = True) As KnownColor
        Return GetNearestKnownColor(Color.FromArgb(red, green, blue), excludeSystemColors)
    End Function

    ''' <summary>
    '''  Finds the nearest <see cref="KnownColor"/> to the specified <see cref="Color"/>.
    ''' </summary>
    ''' <param name="col">The color to match.</param>
    ''' <param name="excludeSystemColors">
    '''  If <see langword="True"/>, system colors are excluded from the search. Default is <see langword="True"/>.
    ''' </param>
    ''' <returns>
    '''  The <see cref="KnownColor"/> that is closest to the specified <see cref="Color"/>.
    ''' </returns>
    Public Function GetNearestKnownColor(col As Color, Optional excludeSystemColors As Boolean = True) As KnownColor
        Dim rgbList As New SortedList(Of Long, KnownColor)
        Dim key As Integer
        Dim hsb As Single, kColor As Color
        For Each color As KnownColor In [Enum].GetValues(Of KnownColor)
            kColor = Drawing.Color.FromKnownColor(color)
            If Not excludeSystemColors OrElse Not kColor.IsSystemColor Then
                key = RgbDifference(kColor, col)
                If Not rgbList.ContainsKey(key) Then
                    rgbList.Add(key, value:=color)
                End If
            End If
        Next
        Dim hsbList As New SortedList(Of Single, KnownColor)
        For i As Integer = 0 To 4
            kColor = Color.FromKnownColor(color:=rgbList.Values(index:=i))
            hsb = HsbDifference(col, kColor)
            If Not hsbList.ContainsKey(key:=hsb) Then
                hsbList.Add(key:=hsb, value:=rgbList.Values(index:=i))
            End If
        Next
        Return hsbList.Values(index:=0)
    End Function

End Module
