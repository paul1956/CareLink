' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DataGridViewHelper

    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    Friend Function CreateDefaultDataGridView(dgvName As String) As DataGridView
        Dim dGV As New DataGridView With {
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                    .BackColor = Color.Silver
                },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                    .Alignment = DataGridViewContentAlignment.MiddleCenter,
                    .BackColor = SystemColors.Control,
                    .Font = New System.Drawing.Font("Segoe UI", 9.0!, FontStyle.Regular, GraphicsUnit.Point),
                    .ForeColor = SystemColors.WindowText,
                    .SelectionBackColor = SystemColors.Highlight,
                    .SelectionForeColor = SystemColors.HighlightText,
                    .WrapMode = DataGridViewTriState.True
                },
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            .Dock = DockStyle.Fill,
            .Location = New System.Drawing.Point(3, 3),
            .Name = dgvName,
            .[ReadOnly] = True,
            .SelectionMode = DataGridViewSelectionMode.CellSelect,
            .TabIndex = 0
        }
        dGV.RowTemplate.Height = 25
        Return dGV
    End Function

    <Extension>
    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, dGV As DataGridView, table As DataTable, rowIndex As ItemIndexes)
        realPanel.SetTabName(rowIndex)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

    <Extension>
    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As ItemIndexes, hideRecordNumberColumn As Boolean)
        Dim button1 As Button = TryCast(realPanel.Controls(0), Button)
        If button1 Is Nothing Then
            realPanel.SetTabName(rowIndex)
        Else
            realPanel.SetTabName(rowIndex)
        End If
        Dim dGVIndex As Integer = realPanel.Controls.Count - 1
        Dim dGV As DataGridView = TryCast(realPanel.Controls(dGVIndex), DataGridView)

        If dGV Is Nothing Then
            dGV = CreateDefaultDataGridView($"DataGridView{className}")
            realPanel.Controls.Add(dGV, 0, 1)
            attachHandlers(dGV)
        End If
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
        If hideRecordNumberColumn AndAlso dGV.Columns(0).Name = "RecordNumber" Then
            dGV.Columns(0).Visible = False
        End If
    End Sub

    <Extension>
    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As Integer)
        Dim dGV As DataGridView = CreateDefaultDataGridView($"DataGridView{className}")
        dGV.AllowUserToResizeRows = False
        dGV.AutoSize = False
        dGV.ColumnHeadersVisible = False
        dGV.ReadOnly = True
        realPanel.Controls.Add(dGV, 0, rowIndex)
        attachHandlers(dGV)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
        dGV.Height = table.Rows.Count * 30
    End Sub

End Module
