' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SummaryTabHelpers

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(SummaryRecord.RecordNumber), StringComparison.OrdinalIgnoreCase) Then
            Dim value As Single = CSng(dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
            If Math.Truncate(value) = ItemIndexes.medicalDeviceInformation Then
                e.Value = value.ToString("F1")
                e.FormattingApplied = True
            End If
        End If

    End Sub

    <Extension>
    Friend Sub UpdateSummaryTab(dgvSummary As DataGridView)
        s_listOfSummaryRecords.Sort()
        dgvSummary.InitializeDgv()
        dgvSummary.DataSource = ClassCollectionToDataTable(s_listOfSummaryRecords)
        dgvSummary.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        dgvSummary.RowHeadersVisible = False
        RemoveHandler dgvSummary.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgvSummary.CellFormatting, AddressOf DataGridView_CellFormatting
    End Sub

End Module
