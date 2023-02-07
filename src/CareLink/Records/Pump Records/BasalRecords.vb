' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BasalRecords
    Private ReadOnly _buffer As New List(Of BasalRecord)
    Private ReadOnly _size As Integer

    Public Sub New(size As Integer)
        _size = size
    End Sub

    Friend Sub Add(item As BasalRecord)
        If _buffer.Any(Function(r As BasalRecord) r.GetOaGetTime = item.GetOaGetTime) Then
            Exit Sub
        End If
        If _buffer.Count = _size Then
            _buffer.RemoveAt(0)
        End If
        _buffer.Add(item)
    End Sub

    Friend Sub Clear()
        _buffer.Clear()
    End Sub

    Friend Function Count() As Integer
        Return _buffer.Count
    End Function

    Friend Function GetSubTitle() As String
        If InAutoMode Then
            Return "AutoMode"
        End If
        Return $"{_buffer.Last().activeBasalPattern} rate = {_buffer.Last().GetBasalPerHour}U Per Hour".Replace("  ", " ")

    End Function

    Friend Function ToList() As List(Of BasalRecord)
        Return _buffer
    End Function

End Class
