' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

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

    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, dGV As DataGridView, table As DataTable, rowIndex As ItemIndexs)
        initializeTableLayoutPanel(realPanel, rowIndex)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As ItemIndexs)
        initializeTableLayoutPanel(realPanel, rowIndex)
        Dim dGV As DataGridView
        If realPanel.Controls.Count > 1 Then
            dGV = CType(realPanel.Controls(1), DataGridView)
        Else
            dGV = CreateDefaultDataGridView($"DataGridView{className}")
            realPanel.Controls.Add(dGV, 0, 1)
            attachHandlers(dGV)
        End If
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

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
