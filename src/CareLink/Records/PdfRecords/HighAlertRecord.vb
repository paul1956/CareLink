' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertRecord

    Private Shared ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

    Public Sub New(row As StringTable.Row, valueUnits As String)
        Dim s1() As String = row.Columns(index:=0).Split(separator:=" ", Options)
        Select Case s1.Length
            Case 0
                Exit Sub
            Case 1
                If s1(0).Length = 0 Then
                    Exit Sub
                End If
                Stop
                Exit Sub
            Case 2
            Case Else
                Stop
        End Select

        Me.ValueUnits = valueUnits
        Me.Start = s1(0)

        Me.HighLimit = s1(1)
        If IsNotNullOrWhiteSpace(value:=row.Columns(index:=1)) Then
            Me.AlertBeforeHigh = "True"
            Me.TimeBeforeHigh &= " *Verify!"
        End If
        Me.AlertOnHigh = If(IsNotNullOrWhiteSpace(value:=row.Columns(index:=2)), "True", "False")
        If IsNullOrWhiteSpace(value:=row.Columns(index:=3)) Then
            Me.RiseAlert = "False"
        Else
            Me.RiseAlert = "True"
            Me.RaiseLimit = row.Columns(index:=3)
        End If

        Me.IsValid = True
    End Sub

    Public Sub New()
    End Sub

    Public Property [End] As String
    Public Property AlertBeforeHigh As String = "False"
    Public Property AlertOnHigh As String = "False"
    Public Property HighLimit As String
    Public Property IsValid As Boolean = False
    Public Property Name As String = ""
    Public Property MaxVolumeAtNight As String = ""
    Public Property RaiseLimit As String
    Public Property RiseAlert As String
    Public Property SnoozeTime As String
    Public Property SnoozeOn As String = "Off"
    Public Property Start As String
    Public Property TimeBeforeHigh As String = "15 Min"
    Public Property ValueUnits As String
End Class
