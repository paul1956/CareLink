' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SensorRecord

    Public Sub New(sensorOn As String, sTable As StringTable)
        _SensorOn = sensorOn
        If Not sTable.IsValid Then
            Stop
            Exit Sub
        End If
        Me.CalibrationReminder = sTable.GetSingleLineValue(Of String)("Calibration Reminder ")
        Me.CalibrationReminderTime = sTable.GetSingleLineValue(Of String)("Calibration Reminder Time ")
    End Sub

    Public Property SensorOn As String
    Public Property CalibrationReminder As String
    Public Property CalibrationReminderTime As String
End Class
