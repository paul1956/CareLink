' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LowAlertsRecord
    Private _snoozeTime As TimeSpan

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        _snoozeTime = New TimeSpan(0, 20, 0)
        PdfSettingsRecord.GetSnoozeInfo(listOfAllTextLines, "Low Alerts", Me.SnoozeOn, _snoozeTime)

        Dim valueUnits As String = ""
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            Dim s As StringTable.Row = e.Value
            If e.IsFirst Then
                valueUnits = s.Columns(0).Replace("Start Low Time (", "").Trim(")"c)
                Continue For
            End If
            Dim item As New LowAlertRecord(s, valueUnits) With {
                .End = If(e.IsLast OrElse String.IsNullOrWhiteSpace(sTable.Rows(e.Index + 1).Columns(0)),
                          s_midnight,
                          TimeOnly.Parse(sTable.Rows(e.Index + 1).Columns(0).Split(" ", StringSplitOptions.RemoveEmptyEntries)(0))
                         )
            }
            If item.IsValid Then
                Me.LowAlert.Add(item)
            Else
                Exit For
            End If
        Next

    End Sub

    Public Sub New()
    End Sub

    Public Property LowAlert As New List(Of LowAlertRecord)

    Public Property SnoozeOn As String = "Off"

    Public WriteOnly Property SnoozeTime As TimeSpan
        Set
            _snoozeTime = Value
        End Set
    End Property

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

    Public Overrides Function ToString() As String
        Return If(Me.SnoozeOn = "On", _snoozeTime.ToFormattedTimeSpan("hr"), "Off")
    End Function

End Class
