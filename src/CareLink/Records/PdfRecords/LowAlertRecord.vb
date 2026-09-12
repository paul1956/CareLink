' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LowAlertRecord

    Public Sub New(s As StringTable.Row, valueUnits As String)
        If s.Columns.Count <> 5 Then
            Stop
        End If
        Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
        Dim s1() As String = s.Columns(index:=0).Split(separator:=" ", options)
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

        Me.LowLimit = s1(1)
        Me.Suspend = s.Columns(index:=1)
        Me.AlertForSuspendBeforeLow = If(Me.Suspend.ContainsNoCase(value:="On"), "True", "False")
        Me.AlertOnLow = If(s.Columns(index:=2) = "x", "True", "False")
        Me.AlertBeforeLow = If(s.Columns(index:=3) = "x", "True", "False")
        Me.ResumeBasalAlert = If(s.Columns(index:=4) = "x", "True", "False")
        Me.IsValid = True
    End Sub

    Public Sub New()
    End Sub

    Public Property Name As String = ""
    Public Property LowLimit As String
    Public Property FallAlert As String
    Public Property AlertOnLow As String
    Public Property AlertBeforeLow As String
    Public Property MaxVolumeAtNight As String
    Public Property SnoozeDuration As String
    Public Property SuspendLimit As String
    Public Property SuspendBeforeLow As String
    Public Property AlertForSuspendBeforeLow As String
    Public Property SuspendOnLow As String
    Public Property IsValid As Boolean = False

    ' Legacy property names for compatibility with existing code
    Public Property [End] As String

    Public Property ResumeBasalAlert As String
    Public Property Start As String
    Public Property Suspend As String
    Public Property ValueUnits As String
End Class
