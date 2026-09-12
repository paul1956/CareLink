' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module SensorRecordExtensions

    <Extension>
    Public Sub UpdateCalibrationReminder(this As SensorRecord, sTable As StringTable)
        ArgumentNullException.ThrowIfNull(sTable)
        If sTable.IsValid Then
            this.CalibrationReminder = sTable.GetSingleLineValue(Of String)("Calibration Reminder ")
            this.CalibrationReminderTime = sTable.GetSingleLineValue(Of String)("Calibration Reminder Time ")
            If sTable.Rows.Count = 3 Then
                this.AutoCalibration = sTable.GetSingleLineValue(Of String)("Auto Calibration ")
            End If
        End If
    End Sub

    <Extension>
    Public Sub InitializeFromStringTable(this As SensorRecord, sTable As StringTable)
        ArgumentNullException.ThrowIfNull(sTable)
        MeArgCheck(sTable)
        this.SensorOn = sTable.GetSingleLineValue(Of String)("Sensor")
    End Sub

    Private Sub MeArgCheck(sTable As StringTable)
        ' Placeholder for any common validation used by Sensor parsing extensions.
        If Not sTable.IsValid Then
            ' no-op; caller will handle missing values
        End If
    End Sub

End Module
