' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module PresetTempRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As PresetTempRecord, row As StringTable.Row, key As String)
        If row Is Nothing Then Return
        ' Replicate parsing logic from original constructor
        If row.Columns.Count < 3 OrElse row.Columns(index:=0).Length = 0 Then
            Return
        End If
        Dim column0Trim As String = row.Columns(index:=0).Replace(oldValue:=key, newValue:=String.Empty).Trim()
        If column0Trim.Length = 0 Then
            If row.Columns(index:=1).Length = 0 AndAlso row.Columns(index:=2).Length = 0 Then
                Return
            End If
            Dim pa As New PresetAmountRecord()
            pa.InitializeFromString(row.Columns(index:=1))
            this.PresetAmount = pa
            this.Duration = TimeSpan.Parse(s:=row.Columns(index:=2))
            this.IsValid = True
        Else
            Dim pa2 As New PresetAmountRecord()
            pa2.InitializeFromString(column0Trim)
            this.PresetAmount = pa2
            this.Duration = TimeSpan.Parse(s:=row.Columns(index:=1))
            this.IsValid = True
        End If
    End Sub

End Module
