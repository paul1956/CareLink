' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BasalRecords
    Private ReadOnly _size As Integer
    Private ReadOnly _buffer As New List(Of BasalRecord)

    Public Sub New(size As Integer)
        _size = size
    End Sub

    Public Sub Add(item As BasalRecord)
        If _buffer.Any(Function(r As BasalRecord) r.GetOaGetTime = item.GetOaGetTime) Then
            Exit Sub
        End If
        If _buffer.Count = _size Then
            _buffer.RemoveAt(0)
        End If
        _buffer.Add(item)
    End Sub

    Public Function ToList() As List(Of BasalRecord)
        Return _buffer
    End Function

    Public Function GetSubTitle() As String
        Dim lastBasalRecord As BasalRecord = _buffer.Last
        Select Case _buffer.Last.activeBasalPattern
            Case "BASAL1"
                Return If(lastBasalRecord.GetBasal = 0, "", $" BASAL1, current rate={lastBasalRecord.GetBasal}")
            Case Else
                Return $" {lastBasalRecord.activeBasalPattern} rate = {lastBasalRecord.GetBasal}".Replace("  ", " ")
        End Select

    End Function

    Default ReadOnly Property Value(index As Integer) As BasalRecord
        Get
            Return _buffer(index)
        End Get
    End Property

End Class
