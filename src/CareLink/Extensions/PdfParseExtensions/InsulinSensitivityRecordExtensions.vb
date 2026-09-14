' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module InsulinSensitivityRecordExtensions

    Private ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

    <Extension>
    Public Sub InitializeFromRow(this As InsulinSensitivityRecord, row As StringTable.Row)
        If row Is Nothing Then Return
        If IsNullOrWhiteSpace(value:=row.Columns(index:=0)) Then
            Return
        End If
        Dim s() As String = row.Columns(index:=0).Split(separator:=" "c, Options)
        If s.Length <> 2 Then
            Return
        End If
        If TimeOnly.TryParse(s:=s(0), result:=this.Time) Then
            this.Sensitivity = ParseSingle(s:=s(1))
            this.IsValid = True
        Else
            ' preserve behavior
        End If
    End Sub

End Module
