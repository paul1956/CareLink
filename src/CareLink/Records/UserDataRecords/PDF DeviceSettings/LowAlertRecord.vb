' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LowAlertRecord

    Public Sub New(s As StringTable.Row, valueUnits As String)
        If s.Columns.Count <> 5 Then
            Stop
        End If
        Dim s1() As String = s.Columns(0).CleanSpaces.Split(" ")
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

        Me.LowLimit = Single.Parse(s1(1))
        Select Case s.Columns(1)
            Case "Before Low"
                Me.SuspendBeforelow = True
                Me.SuspendOnlow = False
            Case "On Low"
                Me.SuspendBeforelow = False
                Me.SuspendOnlow = True
            Case Else
                Stop
        End Select
        Me.AlertOnLow = s.Columns(2) = "x"
        Me.AlertBeforeLow = s.Columns(3) = "x"
        Me.ResumeBasalAlert = s.Columns(4) = "x"
        Me.IsValid = True
    End Sub

    Public Property Start As TimeOnly
    Public Property [End] As TimeOnly
    Public Property LowLimit As Single
    Public Property ValueUnits As String
    Public Property AlertBeforeLow As Boolean
    Public Property AlertOnLow As Boolean
    Public Property SuspendBeforelow As Boolean
    Public Property SuspendOnlow As Boolean
    Public Property ResumeBasalAlert As Boolean
    Public Property IsValid As Boolean = False
End Class
