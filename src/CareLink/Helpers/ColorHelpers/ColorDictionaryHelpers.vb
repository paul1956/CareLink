' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Friend Module ColorDictionaryHelpers

    Friend Property GraphColorDictionary As New Dictionary(Of String, KnownColor) From {
        {"Active Insulin", KnownColor.Lime},
        {"Auto Correction", KnownColor.Aqua},
        {"Basal Series", KnownColor.HotPink},
        {"High Limit", KnownColor.Yellow},
        {"Low Limit", KnownColor.Red},
        {"Min Basal", KnownColor.LightYellow},
        {"SG Series", KnownColor.White},
        {"SG Target", KnownColor.Blue},
        {"Suspend", KnownColor.Red},
        {"Time Change", KnownColor.White}}
    Friend Function GetGraphLineColor(legendText As String) As Color
        Dim toColor As Color = GraphColorDictionary(legendText).ToColor
        Return If(legendText = "Suspend",
                  Color.FromArgb(128, toColor),
                  toColor
                 )
    End Function

    Public Function GetColorDictionaryBindingSource() As BindingSource
        Return New BindingSource(GraphColorDictionary, Nothing)
    End Function

    Public Sub GetColorDictionaryFromFile()

        Using fileStream As FileStream = File.OpenRead(GetGraphColorsFileNameWithPath())
            Using sr As New StreamReader(fileStream)
                sr.ReadLine()
                While sr.Peek() <> -1
                    Dim line As String = sr.ReadLine()
                    If line.Length = 0 Then
                        Continue While
                    End If
                    Dim splitLine() As String = line.Split(","c)
                    Dim key As String = splitLine(0)
                    If GraphColorDictionary.ContainsKey(key) Then
                        GraphColorDictionary(key) = GetKnownColorFromName(splitLine(1))
                    End If
                End While
                sr.Close()
            End Using

            fileStream.Close()
        End Using
    End Sub

    Public Sub UpdateColorDictionary(key As String, item As KnownColor)
        GraphColorDictionary(key) = item
    End Sub

    Public Sub WriteColorDictionaryToFile()
        Using fileStream As FileStream = File.OpenWrite(GetGraphColorsFileNameWithPath())
            Using sw As New StreamWriter(fileStream)
                sw.WriteLine($"Key,ForegroundColor,BackgroundColor")
                For Each kvp As KeyValuePair(Of String, KnownColor) In GraphColorDictionary
                    Dim contrastingColor As KnownColor = GetContrastingKnownColor(kvp.Value)
                    sw.WriteLine($"{kvp.Key},{kvp.Value},{contrastingColor}")
                Next
                sw.Flush()
                sw.Close()
            End Using
        End Using
    End Sub

End Module
