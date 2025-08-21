' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module Form1SummaryTabHelpers

    ''' <summary>
    '''  Updates the Summary tab <see cref="DataGridView"/> with the provided
    '''  <paramref name="classCollection"/> of summary records.
    '''  Optionally sorts the records, initializes the <see cref="DataGridView"/>,
    '''  sets the DataSource, manages the current cell selection, and resizes columns.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to update.</param>
    ''' <param name="classCollection">
    '''  The list of <see cref="SummaryRecord"/> objects to display.
    ''' </param>
    ''' <param name="sort">
    '''  If set to <see langword="True"/> sorts the summary records before displaying.
    ''' </param>
    <Extension>
    Friend Sub UpdateSummaryTab(
        dgv As DataGridView,
        classCollection As List(Of SummaryRecord),
        sort As Boolean)

        If sort Then
            s_listOfSummaryRecords.Sort()
        End If
        dgv.InitializeDgv()
        dgv.DataSource = ClassCollectionToDataTable(classCollection)
        dgv.RowHeadersVisible = False
        If s_currentSummaryRow <> 0 AndAlso dgv.Name = My.Forms.Form1.DgvSummary.Name Then
            dgv.CurrentCell = dgv.Rows(index:=s_currentSummaryRow).Cells(index:=2)
        End If
        dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
    End Sub

End Module
