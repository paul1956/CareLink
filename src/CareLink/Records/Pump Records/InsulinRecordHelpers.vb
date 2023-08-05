' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module InsulinRecordHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
            NameOf(InsulinRecord.dateTimeAsString),
            NameOf(InsulinRecord.effectiveDuration),
            NameOf(InsulinRecord.id),
            NameOf(InsulinRecord.kind),
            NameOf(InsulinRecord.OAdateTime),
            NameOf(InsulinRecord.relativeOffset),
            NameOf(InsulinRecord.type),
            NameOf(InsulinRecord.version)
        }

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dateTimeCellFormatting(e, NameOf(CalibrationRecord.dateTime))
        Dim value As String = e.Value.ToString
        Select Case value
            Case "AUTOCORRECTION"
                e.Value = "Auto Correction"
                FormatCell(e, GetGraphLineColor("Auto Correction"))
                Exit Sub
            Case "FAST"
                e.Value = "Fast"
                e.FormattingApplied = True
                Exit Sub
            Case "RECOMMENDED"
                e.Value = "Recommended"
                e.FormattingApplied = True
                Exit Sub
            Case "UNDETERMINED"
                e.Value = "Undetermined"
                e.FormattingApplied = True
                Exit Sub
        End Select
    End Sub

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            If HideColumn(.Name) Then
                .Visible = False
                Exit Sub
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(GetCellStyle(.Name),
                         True,
                         True,
                         CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Friend Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithExcel
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        RemoveHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithExcel
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of InsulinRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

End Module
