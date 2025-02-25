﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices

Friend Module DataGridViewCellStyleExtensions

    <Extension>
    Public Function GetFormattedStyle(cell As DataGridViewCell) As DataGridViewCellStyle
        Dim dgv As DataGridView = cell.DataGridView
        If dgv Is Nothing Then
            Return cell.InheritedStyle
        End If
        Dim e As New DataGridViewCellFormattingEventArgs(cell.ColumnIndex, cell.RowIndex, cell.Value, cell.FormattedValueType, cell.InheritedStyle)
        Dim m As MethodInfo = dgv.GetType().GetMethod("OnCellFormatting", BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase, Nothing, New Type() {GetType(DataGridViewCellFormattingEventArgs)}, Nothing)
        m.Invoke(dgv, New Object() {e})
        Return e.CellStyle
    End Function

    <Extension>
    Public Function SetCellStyle(cellStyle As DataGridViewCellStyle, alignment As DataGridViewContentAlignment, padding As Padding) As DataGridViewCellStyle
        cellStyle.Alignment = alignment
        cellStyle.Padding = padding
        Return cellStyle
    End Function

End Module
