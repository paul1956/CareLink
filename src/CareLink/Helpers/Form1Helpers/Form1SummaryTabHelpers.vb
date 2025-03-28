' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module Form1SummaryTabHelpers

    <Extension>
    Friend Sub UpdateSummaryTab(dgvSummary As DataGridView, classCollection As List(Of SummaryRecord), sort As Boolean)
        If sort Then
            s_listOfSummaryRecords.Sort()
        End If
        dgvSummary.InitializeDgv()
        dgvSummary.DataSource = ClassCollectionToDataTable(classCollection)
        dgvSummary.RowHeadersVisible = False
        If s_currentSummaryRow <> 0 Then
            dgvSummary.CurrentCell = dgvSummary.Rows(s_currentSummaryRow).Cells(2)
        End If
        dgvSummary.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
    End Sub

End Module
