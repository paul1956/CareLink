' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Friend Module ManageColorDictionary

    Private Property GraphColorDictionary As New Dictionary(Of String, KnownColor) From {
                        {"Active Insulin", KnownColor.Lime},
                        {"Auto Correction", KnownColor.Aqua},
                        {"Basal Series", KnownColor.HotPink},
                        {"Min Basal", KnownColor.LightYellow},
                        {"BG Series", KnownColor.White},
                        {"High Limit", KnownColor.Yellow},
                        {"Low Limit", KnownColor.Red},
                        {"Time Change", KnownColor.White}
                    }

    Public Sub ColorDictionaryBackup(ByRef SaveGraphColorDictionary As Dictionary(Of String, KnownColor))
        SaveGraphColorDictionary = GraphColorDictionary.Clone
    End Sub

    Public Sub ColorDictionaryFromBackup(SavedGraphColorDictionary As Dictionary(Of String, KnownColor))
        GraphColorDictionary = SavedGraphColorDictionary.Clone
    End Sub

    Public Sub ColorDictionaryFromFile(repoName As String)

        Using fileStream As FileStream = File.OpenRead(GetGraphColorsFileNameWithPath(repoName))
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

    Public Sub ColorDictionaryToFile(repoName As String)
        Using fileStream As FileStream = File.OpenWrite(GetGraphColorsFileNameWithPath(repoName))
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

    Public Function GetGraphColorsFileNameWithPath(repoName As String) As String
        Return Path.Combine(MyDocumentsPath, $"{repoName}GraphColors.Csv")
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
