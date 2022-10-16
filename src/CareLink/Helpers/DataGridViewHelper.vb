' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module DataGridViewHelper

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

End Module
