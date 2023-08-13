' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports DocumentFormat.OpenXml.Spreadsheet

Friend Module SummaryTabHelpers

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(SummaryRecord.RecordNumber)
                If dgv.Rows(e.RowIndex).Cells("key").Value.Equals(ItemIndexes.medicalDeviceInformation.ToString) Then
                    CellFormattingSingleValue(e, 1)
                End If
        End Select

    End Sub

    <Extension>
    Friend Sub UpdateSummaryTab(dgvSummary As DataGridView)
        s_listOfSummaryRecords.Sort()
        dgvSummary.InitializeDgv()
        dgvSummary.DataSource = ClassCollectionToDataTable(s_listOfSummaryRecords)
        dgvSummary.RowHeadersVisible = False
        If s_currentSummaryRow <> 0 Then
            dgvSummary.CurrentCell = dgvSummary.Rows(s_currentSummaryRow).Cells(2)
        End If
        RemoveHandler dgvSummary.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgvSummary.CellFormatting, AddressOf DataGridView_CellFormatting
    End Sub

End Module
