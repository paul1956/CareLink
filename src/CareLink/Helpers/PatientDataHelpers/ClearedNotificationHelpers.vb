' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ClearedNotificationHelpers
    Private ReadOnly s_columnsToHide As New List(Of String)

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        If e.Column.Name = "RecordNumber" Then
            e.Column.Visible = False
        End If
        e.DgvColumnAdded(
            cellStyle:=GetCellStyle(e.Column.Name),
            wrapHeader:=False,
            forceReadOnly:=True,
            caption:=CType(CType(sender, DataGridView).DataSource, DataTable).Columns(e.Column.Index).Caption)
        If e.Column.Name = "Value" Then
            e.Column.MinimumWidth = 350
        ElseIf e.Column.Name = "Message" Then
            e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            e.Column.FillWeight = 100
        End If
        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
    End Sub

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of ClearedNotifications)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithoutExcel
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        RemoveHandler dgv.DataBindingComplete, AddressOf Form1.DGV_DataBindingComplete
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithoutExcel
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataBindingComplete, AddressOf Form1.DGV_DataBindingComplete
    End Sub

End Module
