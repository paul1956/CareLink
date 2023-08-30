' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertRecord

    Public Sub New(s As StringTable.Row, valueUnits As String)
        Me.ValueUnits = valueUnits
        Stop
        Dim s1() As String = s.Columns(0).Split(" ")
        If Not s.Columns(0) = "" Then
            Stop
            Exit Sub
        End If
        If TimeOnly.TryParse(s1(0), Me.TimeBeforeHigh) Then

        End If
    End Sub

    Public Property ValueUnits As String
    Public Property HighLimit As DeviceLimitRecord
    Public Property AlertBeforeHigh As Boolean
    Public Property TimeBeforeHigh As TimeOnly
    Public Property AlertOnHigh As Boolean
    Public Property RiseAlert As Boolean
    Public Property RaiseLimit As String

    Public Property IsValid As Boolean = False
End Class
