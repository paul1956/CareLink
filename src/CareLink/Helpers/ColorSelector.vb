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
                        {"Low Limit", KnownColor.Red}
                    }

    Public Function GetGraphColor(lineName As String) As Color
        Return GraphColorDictionary(lineName).ToColor
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
