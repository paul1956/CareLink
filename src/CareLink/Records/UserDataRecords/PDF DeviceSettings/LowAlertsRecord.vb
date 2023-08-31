' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LowAlertsRecord
    Private _snoozeTime As TimeSpan

    Public Sub New(snoozeOn As String, snoozeTime As TimeSpan, sTable As StringTable)
        If snoozeOn = "Off" Then
            Me.SnoozeOn = "Off"
        Else
            _snoozeTime = snoozeTime
            Me.SnoozeOn = snoozeOn
        End If

        Dim valueUnits As String = ""
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            Dim s As StringTable.Row = e.Value
            If e.IsFirst Then
                valueUnits = s.Columns(0).Replace("Start Low Time (", "").Trim(")"c)
                Continue For
            End If
            Dim item As New LowAlertRecord(s, valueUnits) With {
                .End = If(e.IsLast OrElse sTable.Rows(e.Index + 1).Columns(0).CleanSpaces.Length = 0,
                          New TimeOnly(0, 0),
                          TimeOnly.Parse(sTable.Rows(e.Index + 1).Columns(0).CleanSpaces.Split(" ")(0))
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

    Public WriteOnly Property SnoozeTime As TimeSpan
        Set
            _snoozeTime = Value
        End Set
    End Property

    Public Property SnoozeOn As String = "Off"

    Public Overrides Function ToString() As String
        Return If(Me.SnoozeOn = "On", $"{_snoozeTime.Hours}:{_snoozeTime.Minutes:D2} hr", "Off")
    End Function

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

End Class
