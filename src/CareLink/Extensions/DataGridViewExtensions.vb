' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DataGridViewExtensions

    <Extension>
    Friend Sub InitializeDgv(dGV As DataGridView)
        With dGV
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                    .BackColor = Color.Silver
                }
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                    .Alignment = DataGridViewContentAlignment.MiddleCenter,
                    .BackColor = SystemColors.Control,
                    .Font = New Font("Segoe UI", 9.0!, FontStyle.Regular, GraphicsUnit.Point),
                    .ForeColor = SystemColors.WindowText,
                    .SelectionBackColor = SystemColors.Highlight,
                    .SelectionForeColor = SystemColors.HighlightText,
                    .WrapMode = DataGridViewTriState.True
                }
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .Dock = DockStyle.Fill
            .Location = New Point(3, 3)
            .ReadOnly = True
            .RowTemplate.Height = 25
            .SelectionMode = DataGridViewSelectionMode.CellSelect
            .TabIndex = 0
        End With
    End Sub

    <Extension>
    Public Sub dgvCellFormatting(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, key As String)
        If e.Value Is Nothing Then
            Return
        End If
        If dgv.Columns(e.ColumnIndex).Name.Equals(key, StringComparison.Ordinal) Then
            Dim dateValue As Date = e.Value.ToString.ParseDate("")
            e.Value = dateValue.ToShortDateTimeString
        End If
    End Sub

End Module
