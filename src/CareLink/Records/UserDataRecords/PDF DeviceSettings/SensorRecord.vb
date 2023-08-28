' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SensorRecord

    Public Sub New(sensorOn As String, lines As List(Of String))
        _SensorOn = sensorOn
        If lines.Count <> 2 Then
            Stop
            Exit Sub
        End If
        Me.CalibrationReminder = lines.GetSingleLineValue(Of String)("Calibration Reminder ")
        Me.CalibrationReminderTime = lines.GetSingleLineValue(Of String)("Calibration Reminder Time ")
    End Sub

    Public Property SensorOn As String
    Public Property CalibrationReminder As String
    Public Property CalibrationReminderTime As String
End Class
