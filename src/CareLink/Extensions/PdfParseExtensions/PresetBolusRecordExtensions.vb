' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module PresetBolusRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As PresetBolusRecord, row As StringTable.Row, key As String)
        If row Is Nothing Then
            Return
        End If
        If row.Columns.Count <> 3 OrElse row.Columns(index:=0).Length = 0 Then
            Return
        End If
        Dim column0Trim As String = row.Columns(index:=0).Replace(oldValue:=key, newValue:=String.Empty).Trim()
        If column0Trim.Length = 0 Then
            If row.Columns(index:=1).Length = 0 AndAlso row.Columns(index:=2).Length = 0 Then
                Return
            End If
            If row.Columns(index:=1).Contains("-"c) Then
                this.BolusTypeNormal = True
                this.Bolus = column0Trim
            Else
                this.BolusTypeNormal = False
                Dim squareSplit() As String = row.Columns(index:=1).Split("-"c)
                this.Bolus = squareSplit(0)
                this.Duration = squareSplit(1)
            End If
        Else
            this.BolusTypeNormal = True
            this.Bolus = column0Trim
            this.Square = row.Columns(index:=1)
        End If
        this.IsValid = True
    End Sub

End Module
