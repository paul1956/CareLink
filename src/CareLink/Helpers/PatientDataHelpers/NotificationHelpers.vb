' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides helper methods for displaying and managing notification data in <see cref="DataGridView"/> controls.
'''  Handles attaching event handlers, formatting, and layout for notification tables.
''' </summary>
Friend Module NotificationHelpers
    Private ReadOnly s_columnsToHide As New List(Of String)

    Private ReadOnly s_rowsToHide As New List(Of String) From {
        NameOf(ActiveNotification.Version),
        NameOf(ClearedNotifications.RecordNumber),
        NameOf(ClearedNotifications.ReferenceGUID)}

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    ''' <summary>
    '''  Attaches the handlers to the <see cref="DataGridView"/> for notifications.
    '''  This is used to set up the DataGridView for displaying notifications.
    '''  It includes handlers for context menu, cell formatting, column addition,
    '''  data binding completion, and layout events.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to which the handlers will be attached.</param>
    Private Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded, AddressOf DgvNotification_CellContextMenuStripNeededWithoutExcel
        RemoveHandler dgv.CellFormatting, AddressOf DgvNotification_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DgvNotification_ColumnAdded
        RemoveHandler dgv.DataBindingComplete, AddressOf DgvNotification_DataBindingComplete
        RemoveHandler dgv.Layout, AddressOf DgvNotification_Layout
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf DgvNotification_CellContextMenuStripNeededWithoutExcel
        AddHandler dgv.CellFormatting, AddressOf DgvNotification_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DgvNotification_ColumnAdded
        AddHandler dgv.DataBindingComplete, AddressOf DgvNotification_DataBindingComplete
        AddHandler dgv.Layout, AddressOf DgvNotification_Layout
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellContextMenuStripNeeded"/> event to provide
    '''  a context menu for copying data.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing context menu information.</param>
    Private Sub DgvNotification_CellContextMenuStripNeededWithoutExcel(
        sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs)

        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.RowIndex >= 0 AndAlso dgv.SelectedCells.Count > 0 Then
            e.ContextMenuStrip = My.Forms.Form1.DgvCopyWithoutExcelMenuStrip
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event to format notification cell values.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing formatting information.</param>
    Private Sub DgvNotification_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.Value.ToString().StartsWithIgnoreCase(value:="additionalInfo") Then
            e.Value = e.Value.ToString.Replace(oldValue:=":", newValue:=" : ")
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event to configure column properties for notification tables.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing column information.</param>
    Private Sub DgvNotification_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If s_filterJsonData AndAlso s_columnsToHide.Contains(item:= .Name) Then
                .Visible = False
            End If
            Dim cellStyle As DataGridViewCellStyle = ClassPropertiesToColumnAlignment(Of SummaryRecord)(
                alignmentTable:=s_alignmentTable,
                columnName:= .Name)

            e.DgvColumnAdded(
                cellStyle,
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
            If e.Column.Index = 0 Then
                e.Column.MinimumWidth = 45
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            ElseIf e.Column.Name <> "Message" Then
                e.Column.MinimumWidth = 300
            Else
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.DataBindingComplete"/> event to
    '''  finalize DataGridView appearance after binding.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments for data binding completion.</param>
    Private Sub DgvNotification_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.ColumnCount > 0 Then
            dgv.ScrollBars = ScrollBars.None
            Dim dataGridViewLastColumn As DataGridViewColumn = dgv.Columns(dgv.ColumnCount - 1)
            If dataGridViewLastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill Then
                dataGridViewLastColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True
            End If
        End If
        dgv.ClearSelection()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.Layout"/> event to adjust the DataGridView size to fit its rows.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Layout event arguments.</param>
    Private Sub DgvNotification_Layout(sender As Object, e As LayoutEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.AutoSize = False

        ' Calculate total height of rows and headers
        Dim height As Integer = 5
        For Each row As DataGridViewRow In dgv.Rows
            If row.Visible Then
                height += row.Height
            End If
        Next
        ' Adjust DataGridView size if necessary
        If dgv.ClientSize.Height <> height Then
            dgv.ClientSize = New Size(dgv.ClientSize.Width, height)
        End If

        ' Set panel row to absolute height
        Dim index As Integer = dgv.Parent.Controls.IndexOf(control:=dgv)
        Dim panel As TableLayoutPanel = CType(dgv.Parent, TableLayoutPanel)
        panel.RowStyles(index).SizeType = SizeType.Absolute
        panel.RowStyles(index).Height = dgv.ClientSize.Height + 5
    End Sub

    ''' <summary>
    '''  Displays a notification data table in a <see cref="DataGridView"/> within the specified panel.
    ''' </summary>
    ''' <param name="realPanel">The <see cref="TableLayoutPanel"/> to add the DataGridView to.</param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="className">The class name for naming the DataGridView.</param>
    ''' <param name="attachHandlers">Delegate to attach event handlers to the DataGridView.</param>
    Private Sub DisplayNotificationDataTableInDGV(
        ByRef realPanel As TableLayoutPanel,
        table As DataTable,
        className As String,
        attachHandlers As attachHandlers)

        Dim dgv As New DataGridView With {
                .AutoSize = False,
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
                .BorderStyle = BorderStyle.None,
                .ColumnHeadersVisible = False,
                .Dock = DockStyle.Top,
                .Name = $"DataGridView{className}",
                .RowHeadersVisible = False}
        realPanel.AutoSize = True
        realPanel.RowStyles.Add(rowStyle:=New RowStyle(sizeType:=SizeType.AutoSize))
        realPanel.RowCount += 1
        If className = "activeNotifications" Then
            realPanel.Dock = DockStyle.Fill
            realPanel.RowCount = 2
            realPanel.Controls.Add(control:=dgv, column:=0, row:=1)
            realPanel.RowStyles(index:=1).SizeType = SizeType.AutoSize
        Else
            realPanel.Controls.Add(control:=dgv, column:=0, row:=realPanel.RowCount - 1)
        End If
        dgv.InitializeDgv()
        dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        attachHandlers?(dgv)
        For Each column As DataGridViewColumn In dgv.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        Next
        Dim rowIndex As Integer = 0
        dgv.DataSource = table
        If className = "activeNotifications" Then
        Else
            For Each dgvRow As DataGridViewRow In dgv.Rows
                dgv.AutoResizeRow(rowIndex, autoSizeRowMode:=DataGridViewAutoSizeRowMode.AllCellsExceptHeader)
                rowIndex += 1
                dgvRow.DefaultCellStyle.WrapMode = DataGridViewTriState.False
            Next
        End If

    End Sub

    ''' <summary>
    '''  Creates notification tables for active and cleared notifications and displays them in the main form.
    ''' </summary>
    ''' <param name="mainForm">The main form containing notification panels.</param>
    Friend Sub UpdateNotificationTabs(mainForm As Form1)
        Const rowIndex As ServerDataIndexes = ServerDataIndexes.notificationHistory

        mainForm.TableLayoutPanelNotificationsCleared.SetTableName(rowIndex, isClearedNotifications:=True)
        mainForm.TableLayoutPanelNotificationsCleared.Controls.Clear()
        mainForm.TableLayoutPanelNotificationsCleared.RowStyles.Clear()
        mainForm.TableLayoutPanelNotificationsCleared.RowCount = 0

        ' Force a full garbage collection and allow background GC if enabled
        GC.Collect(generation:=GC.MaxGeneration, mode:=GCCollectionMode.Optimized, blocking:=False, compacting:=False)

        Dim innerJson As List(Of Dictionary(Of String, String))

        ' clearedNotifications
        innerJson = JsonToDictionaryList(s_notificationHistoryValue(key:="clearedNotifications"))
        Dim classCollection As List(Of SummaryRecord)
        If innerJson.Count > 0 Then
            innerJson.Reverse()
            For Each jsonDictionary As Dictionary(Of String, String) In innerJson
                classCollection = GetSummaryRecords(jsonDictionary, rowsToHide:=s_rowsToHide)
                DisplayNotificationDataTableInDGV(
                    realPanel:=mainForm.TableLayoutPanelNotificationsCleared,
                    table:=ClassCollectionToDataTable(classCollection),
                    className:=NameOf(SummaryRecord),
                    attachHandlers:=AddressOf NotificationHelpers.AttachHandlers)
            Next
            mainForm.TableLayoutPanelNotificationsCleared.HorizontalScroll.Enabled = False
            mainForm.TableLayoutPanelNotificationsCleared.HorizontalScroll.Visible = False
        Else
            mainForm.TableLayoutPanelNotificationsCleared.AutoSizeMode = AutoSizeMode.GrowAndShrink
            mainForm.TableLayoutPanelNotificationsCleared.DisplayEmptyDGV(className:="clearedNotifications")
        End If

        ' activeNotifications
        innerJson = JsonToDictionaryList(s_notificationHistoryValue(key:="activeNotifications"))
        If innerJson.Count > 0 Then
            mainForm.TableLayoutPanelNotificationActive.SetTableName(rowIndex, isClearedNotifications:=False)
            If mainForm.TableLayoutPanelNotificationActive.Controls.Count > 1 Then
                mainForm.TableLayoutPanelNotificationActive.Controls.RemoveAt(index:=1)
                mainForm.TableLayoutPanelNotificationActive.RowStyles.RemoveAt(index:=1)
                mainForm.TableLayoutPanelNotificationActive.RowCount = 1
            End If
            For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                Dim jsonDictionary As Dictionary(Of String, String) = innerDictionary.Value
                classCollection = GetSummaryRecords(jsonDictionary, rowsToHide:=s_rowsToHide)
                DisplayNotificationDataTableInDGV(
                    realPanel:=mainForm.TableLayoutPanelNotificationActive,
                    table:=ClassCollectionToDataTable(classCollection),
                    className:="ActiveNotifications",
                    attachHandlers:=AddressOf NotificationHelpers.AttachHandlers)
            Next
        Else
            mainForm.TableLayoutPanelNotificationActive.AutoSizeMode = AutoSizeMode.GrowAndShrink
            mainForm.TableLayoutPanelNotificationActive.DisplayEmptyDGV(className:="activeNotification")
        End If
        mainForm.TableLayoutPanelNotificationActive.AutoScroll = True
        mainForm.TableLayoutPanelNotificationsCleared.AutoScroll = True
        Application.DoEvents()
    End Sub

End Module
