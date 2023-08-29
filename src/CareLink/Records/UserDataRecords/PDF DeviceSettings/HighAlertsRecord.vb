' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertsRecord
    Private _snoozeTime As New TimeSpan(1, 0, 0)

    Public Sub New(snoozeTime As TimeSpan, snoozeOn As Boolean, highAlert As List(Of HighAlertRecord))
        Me.SnoozeTime = snoozeTime
        Me.SnoozeOn = snoozeOn
        Me.HighAlert = highAlert

    End Sub

    Public Sub New()
    End Sub

    Public Property HighAlert As New List(Of HighAlertRecord)

    Public WriteOnly Property SnoozeTime As TimeSpan
        Set
            _snoozeTime = Value
        End Set
    End Property

    Public Property SnoozeOn As Boolean = False

    Public Overrides Function ToString() As String
        Return If(Me.SnoozeOn, $"{_snoozeTime.Hours} hr", "Off")
    End Function

End Class
