' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module Json

    Private Function GetItemAsString(item As KeyValuePair(Of String, Object)) As String
        Dim itemValue As JsonElement = CType(item.Value, JsonElement)
        Dim itemAsString As String = itemValue.ToString
        Dim result As String = itemAsString
        Select Case itemValue.ValueKind
            Case JsonValueKind.False
                result = "False"
            Case JsonValueKind.Null
                result = ""
            Case JsonValueKind.Number
            Case JsonValueKind.True
                result = "True"
            Case JsonValueKind.String
                Try
                    If Char.IsDigit(result(0)) Then
                        Dim dateSplit As String() = itemAsString.Split("T")
                        If dateSplit.Length = 2 Then
                            Dim zDateString As String() = dateSplit(0).Split("-"c)
                            Dim zTimeString As String() = dateSplit(1).TrimEnd("Z"c).Replace("+", ".").Split(":")
                            Select Case item.Key
                                Case "techHours"
                                Case "lastConduitDateTime",
                                     "medicalDeviceTimeAsString" ' "2021-05-17T01:02:22.307-07:00"
                                    result = $"{ New DateTime(CInt(zDateString(0)),
                                                             CInt(zDateString(1)),
                                                             CInt(zDateString(2)),
                                                             CInt(zTimeString(0)),
                                                             CInt(zTimeString(1)),
                                                             CInt(zTimeString(2).Substring(0, 2)),
                                                             CInt(zTimeString(2).Substring(3, 3)), DateTimeKind.Local)}{ _
                                            itemAsString.Substring(itemAsString.Length - 6)}"
                                Case "lastSensorTSAsString",
                                    "sLastSensorTime",
                                    "sMedicalDeviceTime",
                                    "triggeredDateTime" '2021-05-16T20:28:00.000Z
                                    result = New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Local).ToString()
                                Case "loginDateUTC" ' UTC 2021-05-16T20:28:00.000Z
                                    result = New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Utc).ToString()
                                Case "datetime"
                                    If item.Value.ToString().EndsWith("Z"c) Then
                                        result = New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Local).ToString()
                                    Else
                                        ' "2021-05-17T01:02:22.307-07:00"
                                        result = $"{ New DateTime(CInt(zDateString(0)),
                                                                 CInt(zDateString(1)),
                                                                 CInt(zDateString(2)),
                                                                 CInt(zTimeString(0)),
                                                                 CInt(zTimeString(1)),
                                                                 CInt(zTimeString(2).Substring(0, 2)),
                                                                 CInt(zTimeString(2).Substring(3, 3)), DateTimeKind.Local)}{ _
                                            itemAsString.Substring(itemAsString.Length - 6)}"
                                    End If
                                Case Else
                                    Stop
                            End Select

                        End If
                    End If
                Catch ex As Exception
                    Stop
                End Try
        End Select
        Return result
    End Function

    Public Function LoadList(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim options As New JsonSerializerOptions() With {
                .IgnoreNullValues = True,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        Dim deserializeList As List(Of Dictionary(Of String, Object)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options)
        For Each deserializeItem As Dictionary(Of String, Object) In deserializeList
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each item As KeyValuePair(Of String, Object) In deserializeItem
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                Else
                    resultDictionary.Add(item.Key, GetItemAsString(item))
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

        For Each item As KeyValuePair(Of String, Object) In JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, Nothing)
            Else
                resultDictionary.Add(item.Key, GetItemAsString(item))
            End If
        Next
        Return resultDictionary
    End Function

    <Extension>
    Public Function UtcToLocalTime(utcTime As String) As String
        Dim convertedDate As Date = Date.SpecifyKind(Date.Parse(utcTime), DateTimeKind.Utc)
        Return convertedDate.ToLocalTime.ToString
    End Function

End Module
