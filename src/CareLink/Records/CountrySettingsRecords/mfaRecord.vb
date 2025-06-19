' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MfaRecord
    Private ReadOnly _asList As New Dictionary(Of String, String)

    Public Sub New(jsonData As String)
        _asList = JsonToDictionary(jsonData)
        If _asList.Keys.Count <> 6 Then
            Throw New ApplicationException(
                message:=$"{NameOf(MfaRecord)}({NameOf(jsonData)}) contains {_asList.Count} entries, 6 expected.",
                innerException:=New ApplicationException("Invalid Mfa record structure."))
        End If
        Me.status = _asList(NameOf(status))
        Me.fromDate = _asList(NameOf(fromDate))
        Me.gracePeriod = _asList(NameOf(gracePeriod))
        Me.codeValidityDuration = _asList(NameOf(codeValidityDuration))
        Me.maxAttempts = _asList(NameOf(maxAttempts))
        Me.rememberPeriod = _asList(NameOf(rememberPeriod))

    End Sub

    Public Property codeValidityDuration As String
    Public Property fromDate As String
    Public Property gracePeriod As String
    Public Property maxAttempts As String
    Public Property rememberPeriod As String
    Public Property status As String

    Public Function ToList() As Dictionary(Of String, String)
        Return _asList
    End Function

End Class
