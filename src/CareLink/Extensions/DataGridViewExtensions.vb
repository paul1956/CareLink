' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DataGridViewExtensions

    <Extension>
    Public Function SetCellStyle(cellStyle As DataGridViewCellStyle, alignment As DataGridViewContentAlignment, padding As Padding) As DataGridViewCellStyle
        cellStyle.Alignment = alignment
        cellStyle.Padding = padding
        Return cellStyle
    End Function

    <Extension>
    Public Sub dgvCellFormatting(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, key As String)
        If e.Value Is Nothing Then
            Return
        End If
        Dim columnName As String = dgv.Columns(e.ColumnIndex).Name
        If columnName.Equals(key, StringComparison.Ordinal) Then
            Dim dateValue As Date = e.Value.ToString.ParseDate(columnName)
            e.Value = dateValue.ToShortDateTimeString
        End If
    End Sub

    <Extension>
    Public Sub DgvColumnAdded(ByRef e As DataGridViewColumnEventArgs, cellStyle As DataGridViewCellStyle, wrapHeader As Boolean, forceReadOnly As Boolean, caption As String)
        e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Dim idHeaderName As Boolean = e.Column.DataPropertyName = "ID"
        e.Column.ReadOnly = forceReadOnly OrElse idHeaderName
        e.Column.Resizable = DataGridViewTriState.False
        Dim title As New StringBuilder
        Dim titleInTitleCase As String = If(e.Column.DataPropertyName.Length < 4, e.Column.Name, e.Column.Name.ToTitleCase())
        If wrapHeader Then
            Dim titleSplit As String() = titleInTitleCase.Replace("A I T", "AIT").Split(" "c)
            For Each s As String In titleSplit
                If s.Length < 5 Then
                    title.Append(s)
                Else
                    title.AppendLine(s)
                End If
            Next
        Else
            title.Append(titleInTitleCase.Replace("Care Link", "CareLink").Replace("O Adate Time", "OA Date Time"))
        End If
        e.Column.HeaderText = title.ToString.TrimEnd(Environment.NewLine)
        e.Column.DefaultCellStyle = cellStyle
        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
        If String.IsNullOrWhiteSpace(caption) Then Return
        e.Column.HeaderText = caption
        If e.Column.DataPropertyName = "message" Then
            e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If

    End Sub

End Module
