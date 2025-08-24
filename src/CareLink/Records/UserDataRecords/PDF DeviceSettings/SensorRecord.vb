' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SensorRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable)
        _SensorOn = sTable.GetSingleLineValue(Of String)("Sensor")
    End Sub

    Public Property AutoCalibration As String = "Off"
    Public Property CalibrationReminder As String = "Off"
    Public Property CalibrationReminderTime As String = "Off"
    Public Property SensorOn As String = "Off"

    Public Sub UpdateCalibrationReminder(sTable As StringTable)
        ArgumentNullException.ThrowIfNull(sTable)
        If sTable.IsValid Then
            Me.CalibrationReminder =
                sTable.GetSingleLineValue(Of String)("Calibration Reminder ")
            Me.CalibrationReminderTime =
                sTable.GetSingleLineValue(Of String)("Calibration Reminder Time ")
            If sTable.Rows.Count = 3 Then
                Me.AutoCalibration =
                    sTable.GetSingleLineValue(Of String)("Auto Calibration ")
            End If
        End If
    End Sub

End Class
