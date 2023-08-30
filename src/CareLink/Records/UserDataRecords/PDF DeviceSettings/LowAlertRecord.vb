' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LowAlertRecord

    Public Sub New(s As StringTable.Row, valueUnits As String)
        Me.ValueUnits = valueUnits
        Stop
        Dim s1() As String = s.Columns(0).Split(" ")
        If Not s.Columns(0) = "" Then
            Stop
            Exit Sub
        End If
    End Sub

    Public Property ValueUnits As String
    Public Property LowLimit As DeviceLimitRecord
    Public Property AlertBeforeLow As Boolean
    Public Property AlertOnLow As Boolean
    Public Property SuspendBeforelow As Boolean
    Public Property SuspendOnlow As Boolean
    Public Property ResumeBasalAlert As Boolean
    Public Property IsValid As Boolean = False
End Class
