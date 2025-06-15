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
    '''  Creates notification tables for active and cleared notifications and displays them in the main form.
    ''' </summary>
    ''' <param name="mainForm">The main form containing notification panels.</param>
    ''' <param name="notificationDictionary">Dictionary of notification types and their JSON data.</param>
    Private Sub CreateNotificationTables(mainForm As Form1, notificationDictionary As Dictionary(Of String, String))
        mainForm.TableLayoutPanelNotificationsCleared.Controls.Clear()
        For i As Integer = mainForm.TableLayoutPanelNotificationActive.Controls.Count - 1 To 1 Step -1
            mainForm.TableLayoutPanelNotificationActive.Controls.RemoveAt(i)
        Next
        mainForm.TableLayoutPanelNotificationActive.Controls.Clear()
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationDictionary.WithIndex()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = JsonToLisOfDictionary(notificationType.Value)
            If notificationType.Key = "clearedNotifications" Then
                If innerJson.Count > 0 Then
                    innerJson.Reverse()
                    mainForm.TableLayoutPanelNotificationsCleared.SuspendLayout()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayNotificationDataTableInDGV(
                            realPanel:=mainForm.TableLayoutPanelNotificationsCleared,
                            table:=ClassCollectionToDataTable(listOfClass:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf NotificationHelpers.AttachHandlers,
                            row:=innerDictionary.Index)
                    Next
                    mainForm.TableLayoutPanelNotificationsCleared.ResumeLayout()
                    ResizePanelToFitContents(mainForm.TableLayoutPanelNotificationsCleared)
                Else
                    mainForm.TableLayoutPanelNotificationsCleared.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    mainForm.TableLayoutPanelNotificationsCleared.DisplayEmptyDGV(className:="clearedNotifications")
                End If
            Else
                If innerJson.Count > 0 Then
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayNotificationDataTableInDGV(
                            realPanel:=mainForm.TableLayoutPanelNotificationActive,
                            table:=ClassCollectionToDataTable(listOfClass:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf NotificationHelpers.AttachHandlers,
                            row:=innerDictionary.Index + 1)
                    Next
                Else
                    mainForm.TableLayoutPanelNotificationActive.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    mainForm.TableLayoutPanelNotificationActive.DisplayEmptyDGV(className:="activeNotification")
                End If
            End If
            Application.DoEvents()
        Next
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellContextMenuStripNeeded"/> event to provide
    '''  a context menu for copying data.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing context menu information.</param>
    Private Sub DgvNotification_CellContextMenuStripNeededWithoutExcel(
        sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs)

        If e.RowIndex >= 0 AndAlso CType(sender, DataGridView).SelectedCells.Count > 0 Then
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

        If e.Value.ToString().StartsWith("additionalInfo", StringComparison.OrdinalIgnoreCase) Then
            e.Value = e.Value.ToString.Replace(":", " : ")
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event to configure column properties for notification tables.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing column information.</param>
    Private Sub DgvNotification_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If s_filterJsonData AndAlso s_columnsToHide.Contains(.Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=ClassPropertiesToColumnAlignment(Of SummaryRecord)(s_alignmentTable, .Name),
                forceReadOnly:=True,
                caption:=CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
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
        Dim totalHeight As Integer = 0
        For Each row As DataGridViewRow In dgv.Rows
            If row.Visible Then
                totalHeight += row.Height
            End If
        Next
        ' Adjust DataGridView size if necessary
        If dgv.ClientSize.Height <> totalHeight Then
            dgv.ClientSize = New Size(dgv.ClientSize.Width, totalHeight)
        End If
    End Sub

    ''' <summary>
    '''  Displays a notification data table in a <see cref="DataGridView"/> within the specified panel.
    ''' </summary>
    ''' <param name="realPanel">The <see cref="TableLayoutPanel"/> to add the DataGridView to.</param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="className">The class name for naming the DataGridView.</param>
    ''' <param name="attachHandlers">Delegate to attach event handlers to the DataGridView.</param>
    ''' <param name="row">The row index in the panel to add the DataGridView.</param>
    Private Sub DisplayNotificationDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, row As Integer)
        Dim dGV As New DataGridView With {
            .AutoSize = False,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
            .ColumnHeadersVisible = False,
            .Dock = DockStyle.Top,
            .Name = $"DataGridView{className}",
            .RowHeadersVisible = False}
        realPanel.AutoSize = True
        realPanel.Controls.Add(control:=dGV, column:=0, row)

        dGV.InitializeDgv()
        dGV.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        attachHandlers?(dGV)
        For Each column As DataGridViewColumn In dGV.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        Next
        Dim rowIndex As Integer = 0
        dGV.DataSource = table
        For Each dgvRow As DataGridViewRow In dGV.Rows
            dGV.AutoResizeRow(rowIndex, autoSizeRowMode:=DataGridViewAutoSizeRowMode.AllCellsExceptHeader)
            rowIndex += 1
            dgvRow.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        Next
        dGV.Width = dGV.PreferredSize.Width
        dGV.Height = dGV.PreferredSize.Height
    End Sub

    ''' <summary>
    '''  Resizes the specified panel to fit its contents.
    '''  This method calculates the maximum width and height of the controls within the panel
    '''  and adjusts the panel's size accordingly.
    ''' </summary>
    ''' <param name="panel">The <see cref="Panel"/> to resize.</param>
    Private Sub ResizePanelToFitContents(panel As Panel)
        Dim maxWidth As Integer = 0
        Dim maxHeight As Integer = 0

        ' Iterate through each control in the panel to find the maximum width and height
        For Each ctrl As Control In panel.Controls
            If ctrl.Right > maxWidth Then
                maxWidth = ctrl.Right
            End If

            Dim height As Integer = Math.Abs(ctrl.Bottom)
            If height > maxHeight Then
                maxHeight = height
            End If
        Next

        ' Set the panel size to fit all controls
        panel.Size = New Size(maxWidth + panel.Padding.Right, maxHeight + panel.Padding.Bottom)
    End Sub

    ''' <summary>
    '''  Updates the notification tabs in the main form by clearing and recreating notification tables.
    ''' </summary>
    ''' <param name="mainForm">The main form containing notification panels.</param>
    Friend Sub UpdateNotificationTabs(mainForm As Form1)
        Try
            mainForm.TableLayoutPanelNotificationActive.AutoScroll = True
            mainForm.TableLayoutPanelNotificationActive.SetTableName(rowIndex:=ServerDataIndexes.notificationHistory, isClearedNotifications:=False)
            For i As Integer = mainForm.TableLayoutPanelNotificationActive.Controls.Count - 1 To 1 Step -1
                mainForm.TableLayoutPanelNotificationActive.Controls.RemoveAt(i)
            Next

            mainForm.TableLayoutPanelNotificationsCleared.SetTableName(rowIndex:=ServerDataIndexes.notificationHistory, isClearedNotifications:=True)
            For i As Integer = mainForm.TableLayoutPanelNotificationsCleared.Controls.Count - 1 To 1 Step -1
                mainForm.TableLayoutPanelNotificationsCleared.Controls.RemoveAt(i)
            Next
            CreateNotificationTables(mainForm:=mainForm, notificationDictionary:=s_notificationHistoryValue)
            mainForm.TableLayoutPanelNotificationsCleared.AutoScroll = True
        Catch ex As Exception
            Stop
            Throw
        End Try
    End Sub

End Module
