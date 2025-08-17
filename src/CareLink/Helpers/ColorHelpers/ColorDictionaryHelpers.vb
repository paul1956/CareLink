' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Friend Module ColorDictionaryHelpers

    Friend Property GraphColorDictionary As New Dictionary(Of String, KnownColor) From {
        {"Active Insulin", KnownColor.Lime},
        {"Auto Correction", KnownColor.Aqua},
        {"Basal Series", KnownColor.HotPink},
        {"High Alert", KnownColor.Yellow},
        {"Low Alert", KnownColor.Red},
        {"Min Basal", KnownColor.LightYellow},
        {"SG Series", KnownColor.White},
        {"SG Target", KnownColor.Blue},
        {"Suspend", KnownColor.Red},
        {"Time Change", KnownColor.White}}

    ''' <summary>
    '''  Gets the <see cref="Color"/> for a given legend text, applying transparency for "Suspend".
    ''' </summary>
    ''' <param name="key">The legend text key.</param>
    ''' <returns>The corresponding <see cref="Color"/>.</returns>
    Friend Function GetGraphLineColor(key As String) As Color
        Dim baseColor As Color = GraphColorDictionary(key).ToColor
        Return If(key = "Suspend",
                  Color.FromArgb(alpha:=128, baseColor),
                  baseColor
                 )
    End Function

    ''' <summary>
    '''  Gets a <see cref="BindingSource"/> for the color dictionary, suitable for data binding.
    ''' </summary>
    ''' <returns>A <see cref="BindingSource"/> bound to <see cref="GraphColorDictionary"/>.</returns>
    Public Function GetColorDictionaryBindingSource() As BindingSource
        Return New BindingSource(dataSource:=GraphColorDictionary, dataMember:=Nothing)
    End Function

    ''' <summary>
    '''  Loads the color dictionary from a file, updating <see cref="KnownColor"/> for existing keys.
    ''' </summary>
    Public Sub GetColorDictionaryFromFile()

        Using stream As FileStream = File.OpenRead(path:=GetGraphColorsFileNameWithPath())
            Using sr As New StreamReader(stream)
                sr.ReadLine()
                While sr.Peek() <> -1
                    Dim line As String = sr.ReadLine()
                    If line.Length = 0 Then
                        Continue While
                    End If
                    Dim splitLine() As String = line.Split(separator:=","c)
                    Dim key As String = splitLine(0)
                    If GraphColorDictionary.ContainsKey(key) Then
                        GraphColorDictionary(key) = GetKnownColorFromName(key:=splitLine(1))
                    End If
                End While
                sr.Close()
            End Using

            stream.Close()
        End Using
    End Sub

    ''' <summary>
    '''  Updates the color dictionary with a new <see cref="KnownColor"/> for the specified key.
    ''' </summary>
    ''' <param name="key">The legend text key.</param>
    ''' <param name="item">The <see cref="KnownColor"/> to assign.</param>
    Public Sub UpdateColorDictionary(key As String, item As KnownColor)
        GraphColorDictionary(key) = item
    End Sub

    ''' <summary>
    '''  Writes the current color dictionary to a file, including contrasting background colors.
    ''' </summary>
    Public Sub WriteColorDictionaryToFile()
        Using stream As FileStream = File.OpenWrite(path:=GetGraphColorsFileNameWithPath())
            Using sw As New StreamWriter(stream)
                sw.WriteLine($"Key,ForegroundColor,BackgroundColor")
                For Each kvp As KeyValuePair(Of String, KnownColor) In GraphColorDictionary
                    Dim contrastingColor As KnownColor = GetContrastingKnownColor(knownColor:=kvp.Value)
                    sw.WriteLine(value:=$"{kvp.Key},{kvp.Value},{contrastingColor}")
                Next
                sw.Flush()
                sw.Close()
            End Using
        End Using
    End Sub

End Module
