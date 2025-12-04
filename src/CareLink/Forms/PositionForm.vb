' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

Public Class PositionForm
    Inherits Form

    Private ReadOnly _contextMenu As New ContextMenuStrip()
    Private ReadOnly _copyToExcelMenuItem As New ToolStripMenuItem("Save To Excel")
    Private ReadOnly _copyWithHeaderMenuItem As New ToolStripMenuItem("Copy with Header")
    Private ReadOnly _copyWithoutHeaderMenuItem As New ToolStripMenuItem("Copy without Header")

    Public Sub New()
        MyBase.New()
        Me.InitializePositionForm()
    End Sub

    ''' <summary>
    '''  Handler for column header clicks — delegates sorting to DgvExportHelpers.SortByColumn.
    ''' </summary>
    Private Sub DataGridView1_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.ColumnIndex < 0 OrElse e.ColumnIndex >= dgv.Columns.Count Then
            Return
        End If

        ' Delegate sorting to the shared helper which creates a snapshot and rebinds.
        dgv.SortByColumn(e.ColumnIndex)
    End Sub

    Private Sub InitializePositionForm()
        Me.InitializeComponent()
        ' Build context menu and wire to existing shared helpers in DgvExportHelpers
        _contextMenu.Items.AddRange(New ToolStripItem() {
            _copyWithHeaderMenuItem,
            _copyWithoutHeaderMenuItem,
            New ToolStripSeparator(),
            _copyToExcelMenuItem
        })

        ' Use DgvExportHelpers public methods as handlers (they expect the sender ToolStripMenuItem)
        AddHandler _copyWithHeaderMenuItem.Click, AddressOf DgvExportHelpers.CopyToClipboardWithHeaders
        AddHandler _copyWithoutHeaderMenuItem.Click, AddressOf DgvExportHelpers.CopyToClipboardWithoutHeaders
        AddHandler _copyToExcelMenuItem.Click, AddressOf DgvExportHelpers.CopyDataToExcel

        Me.DataGridView1.ContextMenuStrip = _contextMenu

        ' Hook header click to the shared sort helper
        AddHandler Me.DataGridView1.ColumnHeaderMouseClick, AddressOf Me.DataGridView1_ColumnHeaderMouseClick

    End Sub

End Class
