' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DisplayDataTableInDgvHelpers

    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    Private Function CreateDefaultDataGridView(dgvName As String) As DataGridView
        Dim dGV As New DataGridView With {
            .Name = dgvName
        }
        dGV.InitializeDgv()
        Return dGV
    End Function

    <Extension>
    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, dGV As DataGridView, table As DataTable, rowIndex As ItemIndexes)
        realPanel.SetTabName(rowIndex)
        dGV.InitializeDgv()
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

    <Extension>
    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As ItemIndexes, hideRecordNumberColumn As Boolean)
        realPanel.SetTabName(rowIndex)
        Dim dGVIndex As Integer = realPanel.Controls.Count - 1
        Dim dGV As DataGridView = CreateDefaultDataGridView($"DataGridView{className}")
        realPanel.Controls.Add(dGV, 0, 1)
        attachHandlers?(dGV)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
        If hideRecordNumberColumn AndAlso dGV.Columns(0).Name = "RecordNumber" Then
            dGV.Columns("RecordNumber").Visible = False
        End If
    End Sub

    <Extension>
    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As Integer)
        Dim dGV As DataGridView = CreateDefaultDataGridView($"DataGridView{className}")
        dGV.AutoSize = False
        dGV.ColumnHeadersVisible = False
        realPanel.Controls.Add(dGV, 0, rowIndex)
        attachHandlers(dGV)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
        dGV.Height = table.Rows.Count * 30
    End Sub

End Module
