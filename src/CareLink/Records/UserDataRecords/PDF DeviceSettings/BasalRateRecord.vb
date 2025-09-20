﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BasalRateRecord

    Public Sub New(value As String)
        If value.Trim = "-- --" Then
            Exit Sub
        End If
        If value.Trim.Length > 0 Then
            Dim lineParts() As String = value.Split(separator:=" ")
            If lineParts.Length >= 2 AndAlso IsNumeric(Expression:=lineParts(1)) Then
                Me.[Time] = TimeOnly.Parse(s:=lineParts(0))
                Me.UnitsPerHr = ParseSingle(s:=lineParts(1))
                Me.IsValid = True
            Else
                Stop
            End If
        End If
    End Sub

    Public Property [Time] As TimeOnly
    Public Property UnitsPerHr As Single
    Public Property IsValid As Boolean = False

    Public Overrides Function ToString() As String
        Return If(Me.IsValid,
                  $"{Me.Time} {Me.UnitsPerHr}",
                  " ")
    End Function

End Class
