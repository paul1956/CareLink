' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertsRecord
    Private _snoozeTime As TimeSpan

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        _snoozeTime = OneHourSpan
        PdfSettingsRecord.GetSnoozeInfo(
            listOfAllTextLines,
            target:="High Alerts",
            Me.SnoozeOn,
            snoozeTime:=_snoozeTime)
        Dim valueUnits As String = ""
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            Dim row As StringTable.Row = e.Value
            If e.IsFirst Then
                Dim oldValue As String = "Start High Time ("
                valueUnits =
                    row.Columns(index:=0) _
                       .Replace(oldValue, newValue:="") _
                       .Trim(trimChar:=")"c)
                Continue For
            End If

            Dim options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
            Dim value As String = sTable.Rows(index:=e.Index + 1).Columns(index:=0)
            Dim item As New HighAlertRecord(row, valueUnits) With {
                .End =
                    If(e.IsLast OrElse String.IsNullOrWhiteSpace(value),
                        Midnight,
                        TimeOnly.Parse(s:=sTable.Rows(index:=e.Index + 1) _
                                                .Columns(index:=0) _
                                                .Split(separator:=" ", options)(0)))}
            If item.IsValid Then
                Me.HighAlert.Add(item)
            Else
                Exit For
            End If

        Next
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
