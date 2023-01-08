' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Partial Public Module ColorSelector

    Public Property GraphColorDictionary As New Dictionary(Of String, KnownColor) From {
                        {"Active Insulin", KnownColor.Lime},
                        {"Auto Correction", KnownColor.Aqua},
                        {"Basal Series", KnownColor.HotPink},
                        {"Min Basal", KnownColor.LightYellow},
                        {"BG Series", KnownColor.White},
                        {"High Limit", KnownColor.Yellow},
                        {"Low Limit", KnownColor.Red},
                        {"Time Change", KnownColor.White}
                    }

    Private Function HSBdiff(col1 As Color, col2 As Color) As Single
        Dim h, s, b As Single
        h = Math.Abs(col1.GetHue - col2.GetHue)
        s = Math.Abs(col1.GetSaturation - col2.GetSaturation)
        b = Math.Abs(col1.GetBrightness - col2.GetBrightness)
        Return h + s + b
    End Function

    Private Function RGBdiff(col1 As Color, col2 As Color) As Integer
        Dim r, g, b As Integer
        r = Math.Abs(CInt(col1.R) - CInt(col2.R))
        g = Math.Abs(CInt(col1.G) - CInt(col2.G))
        b = Math.Abs(CInt(col1.B) - CInt(col2.B))
        Return CInt((r ^ 2) + (g ^ 2) + (b ^ 2))
    End Function

    Public Function GetGraphColor(lineName As String) As Color
        Return GraphColorDictionary(lineName).ToColor
    End Function

    Public Function GetNearestKnownColor(col As Color, Optional excludeSystemColors As Boolean = True) As KnownColor
        Dim rgblist As New SortedList(Of Long, KnownColor)
        Dim rgb As Integer, hsb As Single, kcol As Color
        For Each known As KnownColor In System.Enum.GetValues(GetType(KnownColor))
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

    Public Sub LoadColorDictionaryFromFile(ByRef lineColorDictionary As Dictionary(Of String, KnownColor))

        Using fileStream As FileStream = File.OpenRead(GetSavedGraphColorsFileNameWithPath())
            Using sr As New StreamReader(fileStream)
                sr.ReadLine()
                While sr.Peek() <> -1
                    Dim line As String = sr.ReadLine()
                    If Not line.Any Then
                        Continue While
                    End If
                    Dim splitLine() As String = line.Split(","c)
                    Dim key As String = splitLine(0)
                    If lineColorDictionary.ContainsKey(key) Then
                        lineColorDictionary(key) = AllKnownColors(splitLine(1))
                    End If
                End While
                sr.Close()
            End Using

            fileStream.Close()
        End Using
    End Sub

    <Extension>
    Public Function ToColor(c As KnownColor) As Color
        Return Color.FromKnownColor(c)
    End Function

    Public Sub WriteColorDictionaryToFile(graphColors As Dictionary(Of String, KnownColor))
        Using fileStream As FileStream = File.OpenWrite(GetSavedGraphColorsFileNameWithPath)
            Using sw As New StreamWriter(fileStream)
                sw.WriteLine($"Key,ForegroundColor,BackgroundColor")
                For Each kvp As KeyValuePair(Of String, KnownColor) In graphColors
                    Dim contrastingColor As KnownColor = kvp.Value.GetContrastingKnownColor
                    sw.WriteLine($"{kvp.Key},{kvp.Value},{contrastingColor}")
                Next
                sw.Flush()
                sw.Close()
            End Using
        End Using
    End Sub

End Module
