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
                .BackColor = Color.Silver}
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .Alignment = DataGridViewContentAlignment.MiddleCenter,
                .Font = New Font("Segoe UI", 9.0!, FontStyle.Regular, GraphicsUnit.Point),
                .SelectionBackColor = SystemColors.Highlight,
                .SelectionForeColor = SystemColors.HighlightText,
                .WrapMode = DataGridViewTriState.False}
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .Location = New Point(0, 0)
            .ReadOnly = True
            .RowTemplate.Height = 24
            .TabIndex = 0
        End With
    End Sub

End Module
