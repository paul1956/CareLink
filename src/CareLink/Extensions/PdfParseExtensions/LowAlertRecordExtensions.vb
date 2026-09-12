' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module LowAlertRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As LowAlertRecord, row As StringTable.Row, valueUnits As String)
        If row Is Nothing Then Return
        Try
            If row.Columns.Count <> 5 Then Return
            Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
            Dim s1() As String = row.Columns(index:=0).Split(separator:=" "c, options)
            If s1.Length = 0 Then Return
            If s1.Length = 1 Then
                If s1(0).Length = 0 Then Return
                Return
            End If

            this.ValueUnits = valueUnits
            this.Start = s1(0)

            this.LowLimit = s1(1)
            this.Suspend = row.Columns(index:=1)
            this.AlertForSuspendBeforeLow = this.Suspend.ContainsNoCase(value:="On").ToString()
            this.AlertOnLow = (row.Columns(index:=2) = "x").ToString()
            this.AlertBeforeLow = (row.Columns(index:=3) = "x").ToString()
            this.ResumeBasalAlert = (row.Columns(index:=4) = "x").ToString()
            this.IsValid = True
        Catch
        End Try
    End Sub

End Module
