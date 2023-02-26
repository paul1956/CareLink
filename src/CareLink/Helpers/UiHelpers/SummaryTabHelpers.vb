' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SummaryTabHelpers

    <Extension>
    Friend Sub UpdateSummaryTab(dgvSummary As DataGridView)
        s_listOfSummaryRecords.Sort()
        dgvSummary.InitializeDgv()
        dgvSummary.DataSource = ClassCollectionToDataTable(s_listOfSummaryRecords)
        dgvSummary.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        dgvSummary.RowHeadersVisible = False
    End Sub

End Module
