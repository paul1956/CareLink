' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class mfaRecord
    Private ReadOnly _asList As New List(Of KeyValuePair(Of String, String))

    Public Sub New(jsonValue As String)
        Dim values As Dictionary(Of String, String) = Loads(jsonValue)
        If values.Keys.Count <> 6 Then
            Throw New Exception($"{NameOf(mfaRecord)}({jsonValue}) contains {values.Count} entries, 6 expected.")
        End If
        Me.status = values(NameOf(status))
        _asList.Add(KeyValuePair.Create(NameOf(status), Me.status))
        Me.fromDate = values(NameOf(fromDate))
        _asList.Add(KeyValuePair.Create(NameOf(fromDate), Me.fromDate))
        Me.gracePeriod = values(NameOf(gracePeriod))
        _asList.Add(KeyValuePair.Create(NameOf(gracePeriod), Me.gracePeriod))
        Me.codeValidityDuration = values(NameOf(codeValidityDuration))
        _asList.Add(KeyValuePair.Create(NameOf(codeValidityDuration), Me.codeValidityDuration))
        Me.maxAttempts = values(NameOf(maxAttempts))
        _asList.Add(KeyValuePair.Create(NameOf(maxAttempts), Me.maxAttempts))
        Me.rememberPeriod = values(NameOf(rememberPeriod))
        _asList.Add(KeyValuePair.Create(NameOf(rememberPeriod), Me.rememberPeriod))

    End Sub

    Public Property codeValidityDuration As String

    Public Property fromDate As String

    Public Property gracePeriod As String

    Public Property maxAttempts As String

    Public Property rememberPeriod As String

    Public Property status As String

    Public Function ToList() As List(Of KeyValuePair(Of String, String))
        Return _asList
    End Function

End Class
