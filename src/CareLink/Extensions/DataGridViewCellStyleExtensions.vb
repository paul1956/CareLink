' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices

Friend Module DataGridViewCellStyleExtensions

    Friend Sub FormatCell(ByRef e As DataGridViewCellFormattingEventArgs, highlightColor As Color)
        e.Value = e.Value.ToString
        With e.CellStyle
            If e.RowIndex Mod 2 = 0 Then
                .BackColor = highlightColor
                .ForeColor = highlightColor.GetContrastingColor()
            Else
                .ForeColor = highlightColor
                .BackColor = highlightColor.GetContrastingColor()
            End If
            .Font = New Font(.Font, FontStyle.Bold)
        End With
        e.FormattingApplied = True
    End Sub

    <Extension>
    Public Function GetFormattedStyle(cell As DataGridViewCell) As DataGridViewCellStyle
        Dim dgv As DataGridView = cell.DataGridView
        If dgv Is Nothing Then
            Return cell.InheritedStyle
        End If
        Dim e As New DataGridViewCellFormattingEventArgs(cell.ColumnIndex, cell.RowIndex, cell.Value, cell.FormattedValueType, cell.InheritedStyle)
        Dim m As MethodInfo = dgv.GetType().GetMethod("OnCellFormatting", BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Type() {GetType(DataGridViewCellFormattingEventArgs)}, Nothing)
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
