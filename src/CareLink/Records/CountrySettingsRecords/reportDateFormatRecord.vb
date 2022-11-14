' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class reportDateFormatRecord
    Private ReadOnly _asList As New List(Of KeyValuePair(Of String, String))

    Public dateSeparator As String

    Public longTimePattern12 As String

    Public longTimePattern24 As String

    Public shortTimePattern12 As String

    Public shortTimePattern24 As String

    Public timeSeparator As String

    Public Sub New(jsonValue As String)
        For Each kvp As KeyValuePair(Of String, String) In Loads(jsonValue)
            _asList.Add(KeyValuePair.Create(kvp.Key, kvp.Value))
            Select Case kvp.Key
                Case NameOf(longTimePattern12)
                    longTimePattern12 = kvp.Value
                Case NameOf(longTimePattern24)
                    longTimePattern24 = kvp.Value
                Case NameOf(shortTimePattern12)
                    shortTimePattern12 = kvp.Value
                Case NameOf(shortTimePattern24)
                    shortTimePattern24 = kvp.Value
                Case NameOf(dateSeparator)
                    dateSeparator = kvp.Value
                Case NameOf(timeSeparator)
                    timeSeparator = kvp.Value
            End Select
        Next
    End Sub

    Public Function ToList() As List(Of KeyValuePair(Of String, String))
        Return _asList
    End Function

End Class
