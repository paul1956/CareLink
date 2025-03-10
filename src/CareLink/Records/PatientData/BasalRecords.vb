' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BasalRecords
    Private ReadOnly _buffer As New List(Of Basal)
    Private ReadOnly _size As Integer

    Public Sub New(size As Integer)
        _size = size
    End Sub

    Friend Sub Add(item As Basal)
        If _buffer.Any(Function(r As Basal) r.GetOaGetTime = item.GetOaGetTime) Then
            Exit Sub
        End If
        If _buffer.Count <> 0 Then
            If item.Equals(_buffer.Last) Then
                _buffer.Last.OaDateTime(Date.FromOADate(item.GetOaGetTime))
                Exit Sub
            End If
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
        Dim title As String = ""
        If InAutoMode Then
            Dim automodeState As String = s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmState.AutoModeShieldState))
            Select Case automodeState
                Case "AUTO_BASAL"
                    title = If(Is700Series(), "AutoMode", "SmartGuard")
                Case "SAFE_BASAL"
                    title = automodeState.ToTitle
                    Dim safeBasalDuration As UInteger = CUInt(s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmState.SafeBasalDuration)))
                    If safeBasalDuration > 0 Then
                        title &= $", {TimeSpan.FromMinutes(safeBasalDuration):h\:mm} left."
                    End If
            End Select
        Else
            If _buffer.Count <> 0 Then
                Return $"{_buffer.Last().ActiveBasalPattern} rate = {_buffer.Last().GetBasalPerHour} U Per Hour".CleanSpaces
            End If
        End If
        Return title
    End Function

    Friend Function ToList() As List(Of Basal)
        Return _buffer
    End Function

End Class
