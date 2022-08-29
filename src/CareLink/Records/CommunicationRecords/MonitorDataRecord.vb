' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MonitorDataRecord
    Private _hasValue As Boolean

#Region "Single Items"

    Public ReadOnly Property deviceFamily As String

#End Region 'Single Items

    Public Sub New(jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If
        If jsonData.Count <> 1 Then
            Throw New Exception($"{NameOf(MonitorDataRecord)}({NameOf(jsonData)}) contains {jsonData.Count} entries, 1 expected.")
        End If

        Me.deviceFamily = jsonData(NameOf(deviceFamily))

        _hasValue = True
    End Sub

    Public Sub New()
        _hasValue = False
    End Sub

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
