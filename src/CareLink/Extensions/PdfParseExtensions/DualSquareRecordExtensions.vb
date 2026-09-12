' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DualSquareRecordExtensions

    <Extension>
    Public Sub InitializeFromString(this As DualSquareRecord, line As String)
        If line Is Nothing Then
            this.Dual = "Off"
            this.Square = "Off"
            Return
        End If
        Try
            If IsNullOrWhiteSpace(value:=line) Then
                this.Dual = "Off"
                this.Square = "Off"
                Return
            End If
            Dim values() As String = line.Split(separator:="/"c)
            If values.Length >= 2 Then
                this.Dual = values(0)
                this.Square = values(1)
            End If
        Catch
        End Try
    End Sub

End Module
