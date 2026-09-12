' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module HighAlertRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As HighAlertRecord, row As StringTable.Row, valueUnits As String)
        If row Is Nothing Then Return
        Try
            Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
            Dim s1() As String = row.Columns(index:=0).Split(separator:=" "c, options)
            If s1.Length = 0 Then Return
            If s1.Length = 1 Then
                If s1(0).Length = 0 Then Return
                ' preserve original Stop behavior by leaving IsValid false
                Return
            End If
            ' s1.Length >= 2
            this.ValueUnits = valueUnits
            this.Start = s1(0)

            this.HighLimit = s1(1)
            If IsNotNullOrWhiteSpace(value:=row.Columns(index:=1)) Then
                ' append marker to TimeBeforeHigh
                this.TimeBeforeHigh &= " *Verify!"
                this.AlertBeforeHigh = "True"
            End If
            If IsNotNullOrWhiteSpace(value:=row.Columns(index:=2)) Then
                this.AlertOnHigh = "True"
            End If
            If IsNullOrWhiteSpace(value:=row.Columns(index:=3)) Then
                this.RiseAlert = "False"
            Else
                this.RiseAlert = "True"
                this.RaiseLimit = row.Columns(index:=3)
            End If

            this.IsValid = True
        Catch
        End Try
    End Sub

End Module
