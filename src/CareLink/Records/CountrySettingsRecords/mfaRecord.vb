' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MfaRecord
    Private ReadOnly _asList As New Dictionary(Of String, String)

    Public Sub New(json As String)
        _asList = JsonToDictionary(json)
        If _asList.Keys.Count <> 6 Then
            Dim mfaError As New ApplicationException(message:="Invalid Mfa record structure.")
            Throw New ApplicationException(
                message:=$"{NameOf(MfaRecord)}({NameOf(json)}) contains {_asList.Count} entries, 6 expected.",
                innerException:=mfaError)
        End If
        Me.status = _asList(key:=NameOf(status))
        Me.fromDate = _asList(key:=NameOf(fromDate))
        Me.gracePeriod = _asList(key:=NameOf(gracePeriod))
        Me.codeValidityDuration = _asList(key:=NameOf(codeValidityDuration))
        Me.maxAttempts = _asList(key:=NameOf(maxAttempts))
        Me.rememberPeriod = _asList(key:=NameOf(rememberPeriod))

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
