' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertRecord

    Public Sub New(s As StringTable.Row, valueUnits As String)
        Dim s1() As String = s.Columns(0).Split(" ", StringSplitOptions.RemoveEmptyEntries)
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
        If Not TimeOnly.TryParse(s1(0), Me.Start) Then
            Stop
        End If

        Me.HighLimit = Single.Parse(s1(1), Provider)
        If Not String.IsNullOrWhiteSpace(s.Columns(1)) Then
            Me.AlertBeforeHigh = True
            Me.TimeBeforeHigh &= " Please Verify!"
        End If
        Me.AlertOnHigh = Not String.IsNullOrWhiteSpace(s.Columns(2))
        If String.IsNullOrWhiteSpace(s.Columns(3)) Then
            Me.RiseAlert = False
        Else
            Me.RiseAlert = True
            Me.RaiseLimit = s.Columns(3)
        End If

        Me.IsValid = True
    End Sub

    Public Property [End] As TimeOnly
    Public ReadOnly Property AlertBeforeHigh As Boolean = False
    Public ReadOnly Property AlertOnHigh As Boolean
    Public ReadOnly Property HighLimit As Single
    Public ReadOnly Property IsValid As Boolean = False
    Public ReadOnly Property RaiseLimit As String
    Public ReadOnly Property RiseAlert As Boolean
    Public ReadOnly Property Start As TimeOnly
    Public ReadOnly Property TimeBeforeHigh As String = "15* Min"
    Public Property ValueUnits As String
End Class
