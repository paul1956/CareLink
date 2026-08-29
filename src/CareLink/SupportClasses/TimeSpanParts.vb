' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class TimeSpanParts

    Public Sub New(hours As Integer, shortHr As Boolean)
        Dim hourStr As String = If(shortHr,
                                   "hr",
                                   "hour")
        _Days = hours \ 24
        _RemainderHours = hours Mod 24
        _DayPart = If(_Days = 1,
                      "1 day",
                      $"{_Days} days")
        _HourPart = If(_RemainderHours = 1,
                       $"1 {hourStr}",
                       $"{_RemainderHours} {hourStr}s")
        If _Days > 0 And _RemainderHours > 0 Then
            _Result = $"{_DayPart}, {_HourPart}"
        ElseIf _Days > 0 Then
            _Result = _DayPart
        Else
            _Result = _HourPart
        End If
    End Sub

    Public ReadOnly Property DayPart As String
    Public ReadOnly Property Days As Integer
    Public ReadOnly Property HourPart As String
    Public ReadOnly Property RemainderHours As Integer
    Public ReadOnly Property Result As String
    Public ReadOnly Property TimeSpanInMinutes As Integer

    Public Function HoursToDaysAndHours() As String
        Return Me.Result
    End Function

End Class
