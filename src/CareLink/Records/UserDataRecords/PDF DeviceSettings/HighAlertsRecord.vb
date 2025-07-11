' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertsRecord
    Private _snoozeTime As TimeSpan

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        _snoozeTime = OneHourSpan
        PdfSettingsRecord.GetSnoozeInfo(listOfAllTextLines, "High Alerts", Me.SnoozeOn, snoozeTime:=_snoozeTime)
        Dim valueUnits As String = ""
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            Dim s As StringTable.Row = e.Value
            If e.IsFirst Then
                valueUnits = s.Columns(0).Replace("Start High Time (", "").Trim(")"c)
                Continue For
            End If

            Dim item As New HighAlertRecord(s, valueUnits) With {
                .End = If(e.IsLast OrElse String.IsNullOrWhiteSpace(sTable.Rows(e.Index + 1).Columns(0)),
                          Midnight,
                          TimeOnly.Parse(sTable.Rows(e.Index + 1).Columns(0).Split(" ", StringSplitOptions.RemoveEmptyEntries)(0))
                         )
            }
            If item.IsValid Then
                Me.HighAlert.Add(item)
            Else
                Exit For
            End If

        Next
    End Sub

    Public Sub New()
    End Sub

    Public Property HighAlert As New List(Of HighAlertRecord)

    Public WriteOnly Property SnoozeTime As TimeSpan
        Set
            _snoozeTime = Value
        End Set
    End Property

    Public Property SnoozeOn As String = "Off"

    Public Overrides Function ToString() As String
        Return If(Me.SnoozeOn = "On", $"{_snoozeTime.Hours}:{_snoozeTime.Seconds:D2} hr", "Off")
    End Function

End Class
