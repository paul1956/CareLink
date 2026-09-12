' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module BasalRateRecordExtensions

    <Extension>
    Public Sub InitializeFromString(this As BasalRateRecord, value As String)
        If value Is Nothing Then Return
        If value.Trim() = "-- --" Then
            Return
        End If
        If value.Trim().Length > 0 Then
            Dim lineParts() As String = value.Split(separator:=" "c)
            If lineParts.Length >= 2 AndAlso IsNumeric(Expression:=lineParts(1)) Then
                this.[Time] = TimeOnly.Parse(s:=lineParts(0))
                this.UnitsPerHr = ParseSingle(s:=lineParts(1))
                this.IsValid = True
            End If
        End If
    End Sub

End Module
