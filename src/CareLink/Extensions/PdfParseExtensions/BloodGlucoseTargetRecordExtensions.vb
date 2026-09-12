' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module BloodGlucoseTargetRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As BloodGlucoseTargetRecord, row As StringTable.Row)
        If row Is Nothing Then Return
        Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
        Dim s() As String = row.Columns(index:=0).Split(separator:=" "c, options)
        If s.Length = 2 Then
            If TimeOnly.TryParse(s:=s(0), result:=this.Time) Then
                this.Low = ParseSingle(s:=s(1))
                this.High = ParseSingle(s:=row.Columns(index:=1))
                this.IsValid = True
            Else
                ' preserve behavior
            End If
        End If
    End Sub

End Module
