' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods and delegates for displaying <see cref="DataTable"/> objects in <see cref="DataGridView"/> controls
'''  within a <see cref="TableLayoutPanel"/>. Handles initialization, data binding, and optional column visibility.
''' </summary>
Friend Module DgvDataTableHelpers

    ''' <summary>
    '''  Delegate for attaching event handlers to a <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to attach handlers to.</param>
    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    ''' <summary>
    '''  Displays a <see cref="DataTable"/> in a <see cref="DataGridView"/> within a <see cref="TableLayoutPanel"/>.
    '''  Initializes the <see cref="DataGridView"/>, sets its data source, and refreshes the panel.
    ''' </summary>
    ''' <param name="realPanel">The <see cref="TableLayoutPanel"/> containing the <see cref="DataGridView"/>.</param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="dgv">The <see cref="DataGridView"/> to display the data in.</param>
    ''' <param name="rowIndex">The row index in the panel, typically of type <see cref="ServerDataIndexes"/>.</param>
    <Extension>
    Friend Sub DisplayDataTableInDGV(
        realPanel As TableLayoutPanel,
        table As DataTable,
        dgv As DataGridView,
        rowIndex As ServerDataIndexes)

        realPanel?.SetTableName(rowIndex, isClearedNotifications:=False)
        dgv.InitializeDgv()
        dgv.DataSource = table
        dgv.RowHeadersVisible = False
        realPanel?.Refresh()
    End Sub

    ''' <summary>
    '''  Displays a <see cref="DataTable"/> in a <see cref="DataGridView"/> within a <see cref="TableLayoutPanel"/>,
    '''  using the class name and row index to identify the context. Optionally hides the "RecordNumber" column.
    '''  If the table is empty, displays an empty <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="realPanel">The <see cref="TableLayoutPanel"/> containing the <see cref="DataGridView"/>.</param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="className">The class name context for the data.</param>
    ''' <param name="rowIndex">The row index in the panel, typically of type <see cref="ServerDataIndexes"/>.</param>
    ''' <param name="hideRecordNumberColumn">If true, hides the "RecordNumber" column if present.</param>
    <Extension>
    Friend Sub DisplayDataTableInDGV(
        realPanel As TableLayoutPanel,
        table As DataTable,
        className As String,
        rowIndex As ServerDataIndexes,
        Optional hideRecordNumberColumn As Boolean = False)

        realPanel.SetTableName(rowIndex, isClearedNotifications:=False)
        If table?.Rows.Count > 0 Then
            Dim dgvIndex As Integer = realPanel.Controls.Count - 1
            Dim dgv As DataGridView = TryCast(realPanel.Controls(dgvIndex), DataGridView)

            If dgv Is Nothing Then
                Stop
            Else
                dgv.InitializeDgv()
                dgv.AutoSize = True
            End If
            dgv.Dock = DockStyle.Fill
            dgv.DataSource = Nothing
            dgv.DataSource = table
            dgv.RowHeadersVisible = False
            If hideRecordNumberColumn AndAlso dgv.Columns(index:=0).Name = "RecordNumber" Then
                dgv.Columns(columnName:="RecordNumber").Visible = False
            End If
        Else
            DgvNoRecordsFound(realPanel, className)
        End If
        realPanel.Refresh()
    End Sub

End Module
