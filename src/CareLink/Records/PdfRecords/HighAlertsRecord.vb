' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertsRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        Me.InitializeFromStringTable(sTable, listOfAllTextLines)
    End Sub

    Public Property HighAlert As New List(Of HighAlertRecord)

    Public Overrides Function ToString() As String
        Dim highAlertRecord As HighAlertRecord = Me.HighAlert(index:=0)
        Dim ret As String = "Daytime Snooze: " &
            If(highAlertRecord.SnoozeOn = "On",
               highAlertRecord.SnoozeTime,
               "Off")

        If Me.HighAlert.Count > 1 Then
            highAlertRecord = Me.HighAlert(index:=1)
            Dim nightSnooze As String =
                If(highAlertRecord.SnoozeOn = "On",
                   highAlertRecord.SnoozeTime,
                   "Off")
            ret &= $"Nighttime Snooze: {nightSnooze}"

        End If
        Return ret
    End Function

End Class
