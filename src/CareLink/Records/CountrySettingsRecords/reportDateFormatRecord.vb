' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class ReportDateFormatRecord
    Private ReadOnly _asList As New List(Of KeyValuePair(Of String, String))

    Public Property dateSeparator As String

    Public Property longTimePattern12 As String

    Public Property longTimePattern24 As String

    Public Property shortTimePattern12 As String

    Public Property shortTimePattern24 As String

    Public Property timeSeparator As String

    Public Sub New(jsonValue As String)
        For Each kvp As KeyValuePair(Of String, String) In JsonToDictionary(jsonValue)
            _asList.Add(KeyValuePair.Create(kvp.Key, kvp.Value))
            Select Case kvp.Key
                Case NameOf(longTimePattern12)
                    Me.longTimePattern12 = kvp.Value
                Case NameOf(longTimePattern24)
                    Me.longTimePattern24 = kvp.Value
                Case NameOf(shortTimePattern12)
                    Me.shortTimePattern12 = kvp.Value
                Case NameOf(shortTimePattern24)
                    Me.shortTimePattern24 = kvp.Value
                Case NameOf(dateSeparator)
                    Me.dateSeparator = kvp.Value
                Case NameOf(timeSeparator)
                    Me.timeSeparator = kvp.Value
            End Select
        Next
    End Sub

    Public Function ToList() As List(Of KeyValuePair(Of String, String))
        Return _asList
    End Function

End Class
