' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module MealStartEndRecordExtensions

    <Extension>
    Public Sub InitializeFromRow(this As MealStartEndRecord, row As StringTable.Row, key As String)
        If row Is Nothing Then Return
        ' replicates original constructor behaviour minimally
        Try
            If row.Columns.Count > 0 Then
                Dim raw As String = row.Columns(index:=0).Replace(oldValue:=key, newValue:=String.Empty).Trim()
                If raw.Contains("-"c) Then
                    Dim parts() As String = raw.Split("-"c)
                    this.Start = parts(0).Trim()
                    this.End = If(parts.Length > 1, parts(1).Trim(), String.Empty)
                Else
                    this.Start = raw
                    this.End = String.Empty
                End If
            End If
        Catch
        End Try
    End Sub

End Module
