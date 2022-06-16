' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Module Json

    <Extension>
    Private Function ItemAsString(item As KeyValuePair(Of String, Object)) As String
        Dim itemValue As JsonElement = CType(item.Value, JsonElement)
        Dim valueAsString As String = itemValue.ToString
        Select Case itemValue.ValueKind
            Case JsonValueKind.False
                Return "False"
            Case JsonValueKind.Null
                Return ""
            Case JsonValueKind.Number
                Return valueAsString
            Case JsonValueKind.True
                Return "True"
            Case JsonValueKind.String
                Try
                    If Char.IsDigit(valueAsString(0)) Then
                        Dim dateSplit As String() = valueAsString.Split("T")
                        If dateSplit.Length = 2 Then
                            Dim zDateString As String() = dateSplit(0).Split("-"c)
                            Dim zTimeString As String() = dateSplit(1).TrimEnd("Z"c).Replace("+", ".").Split(":")
                            Select Case item.Key
                                Case "techHours"
                                Case "lastConduitDateTime",
                                     "medicalDeviceTimeAsString",
                                     "previousDateTime" ' "2021-05-17T01:02:22.307-07:00"
                                    Return $"{ New DateTime(CInt(zDateString(0)),
                                                             CInt(zDateString(1)),
                                                             CInt(zDateString(2)),
                                                             CInt(zTimeString(0)),
                                                             CInt(zTimeString(1)),
                                                             CInt(zTimeString(2).Substring(0, 2)),
                                                             CInt(zTimeString(2).Substring(3, 3)), DateTimeKind.Local)}{ _
                                                    valueAsString.Substring(valueAsString.Length - 6)}"
                                Case "lastSensorTSAsString",
                                    "sLastSensorTime",
                                    "sMedicalDeviceTime",
                                    "triggeredDateTime" '2021-05-16T20:28:00.000Z
                                    Return New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Local).ToString()
                                Case "loginDateUTC" ' UTC 2021-05-16T20:28:00.000Z
                                    Return New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Utc).ToString()
                                Case "datetime"
                                    If item.Value.ToString().EndsWith("Z"c) Then
                                        Return New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Local).ToString()
                                    End If
                                    ' "2021-05-17T01:02:22.307-07:00"
                                    Return $"{ New DateTime(CInt(zDateString(0)),
                                                             CInt(zDateString(1)),
                                                             CInt(zDateString(2)),
                                                             CInt(zTimeString(0)),
                                                             CInt(zTimeString(1)),
                                                             CInt(zTimeString(2).Substring(0, 2)),
                                                             CInt(zTimeString(2).Substring(3, 3)), DateTimeKind.Local)}{ _
                                                    valueAsString.Substring(valueAsString.Length - 6)}"

                                Case Else
                                    Stop
                            End Select

                        End If
                    End If
                Catch ex As Exception
                    Stop
                End Try
        End Select
        Return valueAsString
    End Function

    Public Function LoadList(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        Dim deserializeList As List(Of Dictionary(Of String, Object)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options)
        For Each deserializeItem As Dictionary(Of String, Object) In deserializeList
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each item As KeyValuePair(Of String, Object) In deserializeItem
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                Else
                    resultDictionary.Add(item.Key, item.ItemAsString)
                End If
            Next
            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray
    End Function

    Public Function Loads(value As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        For Each item As KeyValuePair(Of String, Object) In JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, Nothing)
            Else
                resultDictionary.Add(item.Key, item.ItemAsString)
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
