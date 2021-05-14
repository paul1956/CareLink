' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module Json

    Public Function LoadList(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim options As New JsonSerializerOptions() With {
                .IgnoreNullValues = True,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        Dim deserializeList As List(Of Dictionary(Of String, Object)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options)
        For Each deserializeItem As Dictionary(Of String, Object) In deserializeList
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each Item As KeyValuePair(Of String, Object) In deserializeItem
                If Item.Value Is Nothing Then
                    resultDictionary.Add(Item.Key, Nothing)
                Else
                    resultDictionary.Add(Item.Key, Item.Value.ToString())
                End If
            Next
            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray
    End Function

    Public Function Loads(value As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)
        Dim options As New JsonSerializerOptions() With {
                .IgnoreNullValues = True,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        For Each Item As KeyValuePair(Of String, Object) In JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
            If Item.Value Is Nothing Then
                resultDictionary.Add(Item.Key, Nothing)
            Else
                resultDictionary.Add(Item.Key, Item.Value.ToString())
            End If
        Next
        Return resultDictionary
    End Function

End Module
