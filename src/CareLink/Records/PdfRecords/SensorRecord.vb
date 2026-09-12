' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SensorRecord

    Public Sub New()
    End Sub

    Public Property AutoCalibration As String = "Off"
    Public Property CalibrationReminder As String = "Off"
    Public Property CalibrationReminderTime As String = "Off"
    Public Property SensorOn As String = "Off"
    Public Property SensorEnding As New SensorEndingRecord

End Class
