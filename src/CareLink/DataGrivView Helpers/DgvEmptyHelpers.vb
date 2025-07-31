' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DgvEmptyHelpers

    ''' <summary>
    '''  Displays an empty <see cref="DataGridView"/> in the specified panel if one does not already exist.
    ''' </summary>
    ''' <param name="realPanel">The <see cref="TableLayoutPanel"/> to add the DataGridView to.</param>
    ''' <param name="className">The class name to use for naming the DataGridView.</param>
    <Extension>
    Friend Sub DgvNoRecordsFound(realPanel As TableLayoutPanel, className As String)
        Dim dgvIndex As Integer = realPanel.Controls.Count - 1
        Dim dgv As DataGridView = Nothing
        If dgvIndex >= 0 Then
            dgv = TryCast(realPanel.Controls(dgvIndex), DataGridView)
        End If
        If dgv Is Nothing Then
            dgv = New DataGridView With {
                .AutoSize = True,
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                .ColumnHeadersVisible = False,
                .Dock = DockStyle.Fill,
                .Name = $"DataGridView{className}",
                .RowHeadersVisible = False}
            realPanel.Controls.Add(control:=dgv, column:=0, row:=1)
        Else
            If dgv.DataSource IsNot Nothing Then
                dgv.DataSource = Nothing
            Else
                dgv.Rows.Clear()
                dgv.Columns.Clear()
            End If
        End If
        dgv.BorderStyle = BorderStyle.None
        dgv.Margin = New Padding(all:=0)
        dgv.Padding = New Padding(all:=0)
        RemoveHandler dgv.Paint, AddressOf DgvNoRecordsFoundPaint
        AddHandler dgv.Paint, AddressOf DgvNoRecordsFoundPaint
        dgv.Refresh()
    End Sub

    ''' <summary>
    '''  Paints a "No records found." message on the <see cref="DataGridView"/> if it contains no rows.
    ''' </summary>
    ''' <param name="sender">The DataGridView being painted.</param>
    ''' <param name="e">The <see cref="PaintEventArgs"/> for the paint event.</param>
    Friend Sub DgvNoRecordsFoundPaint(sender As Object, e As PaintEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Rows.Count = 0 Then
            TextRenderer.DrawText(
                dc:=e.Graphics,
                text:="No records found.",
                font:=New Font(family:=dgv.Font.FontFamily, emSize:=20),
                bounds:=dgv.DisplayRectangle,
                dgv.ForeColor,
                backColor:=dgv.BackgroundColor,
                flags:=TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End If
    End Sub

End Module
