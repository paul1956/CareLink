' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DataGridViewExtensions
    <Extension>
    Public Sub AddLeftRow(dgv As DataGridView, category As String, ByRef currentLeftRow As Integer, itemIndex As String, row As KeyValuePair(Of String, String))
        dgv.Rows.Add((New String() _
            {itemIndex, category, row.Key, row.Value, "", "", "", ""}))
        currentLeftRow += 1
    End Sub

    <Extension>
    Public Sub AddLeft(dgv As DataGridView, category As String, itemIndex As String, ByRef currentLeftRow As Integer, ByRef currentRightRow As Integer, row As KeyValuePair(Of String, String))
        If currentLeftRow < currentRightRow Then
            dgv.Rows(currentLeftRow).FillCellValues(category, itemIndex, row, False)
            currentLeftRow += 1
        Else
            dgv.AddLeftRow(category, currentLeftRow, itemIndex, row)
        End If
    End Sub

    <Extension>
    Public Sub AddRight(dgv As DataGridView, category As String, itemIndex As String, ByRef currentLeftRow As Integer, ByRef currentRightRow As Integer, row As KeyValuePair(Of String, String))
        If currentLeftRow > currentRightRow Then
            dgv.Rows(currentRightRow).FillCellValues(category, itemIndex, row, True)
            currentRightRow += 1
        Else
            dgv.AddRightRow(category, itemIndex, currentRightRow, row)
        End If
    End Sub

    <Extension>
    Public Sub AddRightRow(dgv As DataGridView, category As String, itemIndex As String, ByRef currentRightRow As Integer, row As KeyValuePair(Of String, String))
        dgv.Rows.Add((New String() _
        {"", "", "", "", itemIndex, category, row.Key, row.Value}))
        currentRightRow += 1
    End Sub

    <Extension>
    Public Sub FillCellValues(dgvRow As DataGridViewRow, category As String, itemIndex As String, row As KeyValuePair(Of String, String), rightItem As Boolean)
        Dim startIndex As Integer = If(rightItem, 4, 0)
        dgvRow.Cells(startIndex).Value = $"{itemIndex}"
        dgvRow.Cells(startIndex + 1).Value = category
        dgvRow.Cells(startIndex + 2).Value = row.Key
        dgvRow.Cells(startIndex + 3).Value = row.Value
    End Sub

    <Extension>
    Public Function CellStyleMiddleCenter(cellStyle As DataGridViewCellStyle) As DataGridViewCellStyle
        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        cellStyle.Padding = New Padding(1)
        Return cellStyle
    End Function

    <Extension>
    Public Function CellStyleMiddleLeft(cellStyle As DataGridViewCellStyle) As DataGridViewCellStyle
        cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        cellStyle.Padding = New Padding(1)
        Return cellStyle
    End Function

    <Extension>
    Public Function CellStyleMiddleRight(cellStyle As DataGridViewCellStyle, leftPadding As Integer) As DataGridViewCellStyle
        cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        cellStyle.Padding = New Padding(leftPadding, 1, 1, 1)
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
    Public Sub DgvColumnAdded(ByRef e As DataGridViewColumnEventArgs, cellStyle As DataGridViewCellStyle, wrapHeader As Boolean)
        e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        e.Column.ReadOnly = True
        e.Column.Resizable = DataGridViewTriState.False
        Dim title As New StringBuilder
        Dim titleInTitleCase As String = e.Column.Name.ToTitleCase()
        If wrapHeader Then
            Dim titleSplit As String() = titleInTitleCase.Split(" "c)
            For Each s As String In titleSplit
                If s.Length < 5 Then
                    title.Append(s)
                Else
                    title.AppendLine(s)
                End If
            Next
        Else
            title.Append(titleInTitleCase)
        End If
        e.Column.HeaderText = title.ToString.TrimEnd(Environment.NewLine)
        e.Column.DefaultCellStyle = cellStyle
        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
    End Sub

End Module
