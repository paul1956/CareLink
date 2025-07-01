' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for <see cref="DataGridView"/> to simplify initialization and configuration.
''' </summary>
Public Module DataGridViewExtensions

    ''' <summary>
    '''  Initializes the specified <see cref="DataGridView"/> with default settings for appearance and behavior.
    ''' </summary>
    ''' <param name="dgv">
    '''  The <see cref="DataGridView"/> to initialize.
    ''' </param>
    <Extension>
    Friend Sub InitializeDgv(dgv As DataGridView)
        Dim emSize As Single = If(dgv.Name = NameOf(Form1.DgvBasalPerHour), 12.0!, 10.0!)
        With dgv
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.FromArgb(red:=45, green:=45, blue:=45),
                .ForeColor = Color.White,
                .SelectionBackColor = Color.FromArgb(red:=51, green:=153, blue:=255),
                .SelectionForeColor = Color.White}
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .Alignment = DataGridViewContentAlignment.MiddleCenter,
                .BackColor = Color.Black,
                .Font = New Font(
                    FamilyName,
                    emSize,
                    style:=FontStyle.Bold),
                .WrapMode = DataGridViewTriState.True}
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .DataSource = Nothing
            .Location = New Point(x:=0, y:=0)
            .ReadOnly = True
            .Rows.Clear()
            .RowsDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.FromArgb(red:=180, green:=180, blue:=180),
                .ForeColor = Color.Black,
                .SelectionBackColor = Color.FromArgb(red:=51, green:=153, blue:=255),
                .SelectionForeColor = Color.White}
            .RowTemplate.Height = 24
            .TabIndex = 0
        End With
    End Sub

End Module
