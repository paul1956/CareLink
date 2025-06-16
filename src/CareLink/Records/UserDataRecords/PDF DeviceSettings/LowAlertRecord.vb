' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LowAlertRecord

    Public Sub New(s As StringTable.Row, valueUnits As String)
        If s.Columns.Count <> 5 Then
            Stop
        End If
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

        Me.LowLimit = s1(1).ParseSingleInvariant
        Me.Suspend = s.Columns(1)
        Me.SuspendOnLow = Me.Suspend.Contains("On", StringComparison.OrdinalIgnoreCase)
        Me.AlertOnLow = s.Columns(2) = "x"
        Me.AlertBeforeLow = s.Columns(3) = "x"
        Me.ResumeBasalAlert = s.Columns(4) = "x"
        Me.IsValid = True
    End Sub

    Public Property [End] As TimeOnly
    Public Property AlertBeforeLow As Boolean
    Public Property AlertOnLow As Boolean
    Public Property IsValid As Boolean = False
    Public Property LowLimit As Single
    Public Property ResumeBasalAlert As Boolean
    Public Property Start As TimeOnly
    Public Property Suspend As String
    Public Property SuspendOnLow As Boolean
    Public Property ValueUnits As String
End Class
