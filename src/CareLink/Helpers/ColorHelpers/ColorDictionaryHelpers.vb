' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Friend Module ColorDictionaryHelpers

    Private Property GraphColorDictionary As New Dictionary(Of String, KnownColor) From {
                        {"Active Insulin", KnownColor.Lime},
                        {"Auto Correction", KnownColor.Aqua},
                        {"Basal Series", KnownColor.HotPink},
                        {"High Limit", KnownColor.Yellow},
                        {"Low Limit", KnownColor.Red},
                        {"Min Basal", KnownColor.LightYellow},
                        {"SG Series", KnownColor.White},
                        {"SG Target", KnownColor.Green},
                        {"Time Change", KnownColor.White}
                    }

    <Extension>
    Private Function GetContrastingKnownColor(knownClrBase As KnownColor) As KnownColor
        Dim clrBase As Color = knownClrBase.ToColor
        ' Y is the "brightness"
        Dim y As Double = (0.299 * clrBase.R) + (0.587 * clrBase.G) + (0.114 * clrBase.B)
        If y < 140 Then
            Return KnownColor.White
        Else
            Return KnownColor.Black
        End If
    End Function

    Public Sub ColorDictionaryBackup(ByRef SaveGraphColorDictionary As Dictionary(Of String, KnownColor))
        SaveGraphColorDictionary = GraphColorDictionary.Clone
    End Sub

    Public Sub ColorDictionaryFromBackup(SavedGraphColorDictionary As Dictionary(Of String, KnownColor))
        GraphColorDictionary = SavedGraphColorDictionary.Clone
    End Sub

    Public Sub GetColorDictionaryFromFile()

        Using fileStream As FileStream = File.OpenRead(GetPathToGraphColorsFile(True))
            Using sr As New StreamReader(fileStream)
                sr.ReadLine()
                While sr.Peek() <> -1
                    Dim line As String = sr.ReadLine()
                    If Not line.Any Then
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

    Public Sub WriteColorDictionaryToFile()
        Using fileStream As FileStream = File.OpenWrite(GetPathToGraphColorsFile(True))
            Using sw As New StreamWriter(fileStream)
                sw.WriteLine($"Key,ForegroundColor,BackgroundColor")
                For Each kvp As KeyValuePair(Of String, KnownColor) In GraphColorDictionary
                    Dim contrastingColor As KnownColor = kvp.Value.GetContrastingKnownColor
                    sw.WriteLine($"{kvp.Key},{kvp.Value},{contrastingColor}")
                Next
                sw.Flush()
                sw.Close()
            End Using
        End Using
    End Sub

    Public Function GetGraphColorsBindingSource() As BindingSource
        Return New BindingSource(GraphColorDictionary, Nothing)
    End Function

    Public Function GetGraphLineColor(lineName As String) As Color
        Return GraphColorDictionary(lineName).ToColor
    End Function

    Public Sub UpdateColorDictionary(key As String, item As KnownColor)
        GraphColorDictionary(key) = item
    End Sub

    Public Sub UpdateColorDictionary(key As String, colorName As String)
        GraphColorDictionary(key) = GetKnownColorFromName(colorName)
    End Sub

End Module
