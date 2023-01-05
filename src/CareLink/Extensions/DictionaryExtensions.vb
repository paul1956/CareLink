' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DictionaryExtensions

    <Extension>
    Public Function Clone(Of T)(dic As Dictionary(Of String, T)) As Dictionary(Of String, T)
        Return (From x In dic Select x).ToDictionary(Function(p) p.Key, Function(p) p.Value)
    End Function

    <Extension>
    Public Function IndexOfValue(dic As Dictionary(Of String, KnownColor), item As KnownColor) As Integer
        Return dic.Values.ToList.IndexOf(item)
    End Function

    <Extension>
    Public Function ToCsv(dic As Dictionary(Of String, String)) As String
        If dic Is Nothing Then
            Return "{}"
        End If

        Dim result As New StringBuilder
        For Each kvp As KeyValuePair(Of String, String) In dic
            result.Append($"{kvp.Key} = {kvp.Value}, ")
        Next
        result.TrimEnd(", ")
        Return $"{{{result}}}"
    End Function

End Module
