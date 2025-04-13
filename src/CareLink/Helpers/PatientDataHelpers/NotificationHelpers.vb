' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module NotificationHelpers
    Private ReadOnly s_columnsToHide As New List(Of String)

    Private ReadOnly s_rowsToHide As New List(Of String) From {
        NameOf(ActiveNotification.Version),
        NameOf(ClearedNotifications.RecordNumber),
        NameOf(ClearedNotifications.ReferenceGUID)}

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub CreateNotificationTables(notificationDictionary As Dictionary(Of String, String))
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationDictionary.WithIndex()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = JsonToLisOfDictionary(notificationType.Value)
            If notificationType.Key = "clearedNotifications" Then
                If innerJson.Count > 0 Then
                    innerJson.Reverse()
                    Form1.TableLayoutPanelNotificationsCleared.SuspendLayout()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayNotificationDataTableInDGV(
                            realPanel:=Form1.TableLayoutPanelNotificationsCleared,
                            table:=ClassCollectionToDataTable(listOfClass:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf NotificationHelpers.AttachHandlers,
                            row:=innerDictionary.Index + 1)
                    Next
                    Form1.TableLayoutPanelNotificationsCleared.ResumeLayout()
                Else
                    Form1.TableLayoutPanelNotificationsCleared.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    Form1.TableLayoutPanelNotificationsCleared.DisplayEmptyDGV(className:="clearedNotifications")
                End If
            Else
                If innerJson.Count > 0 Then
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayNotificationDataTableInDGV(
                            realPanel:=Form1.TableLayoutPanelNotificationActive,
                            table:=ClassCollectionToDataTable(listOfClass:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf NotificationHelpers.AttachHandlers,
                            row:=innerDictionary.Index + 1)
                    Next
                Else
                    Form1.TableLayoutPanelNotificationActive.AutoSizeMode = AutoSizeMode.GrowOnly
                    Form1.TableLayoutPanelNotificationActive.DisplayEmptyDGV(className:="activeNotification")
                End If
            End If
            Application.DoEvents()
        Next
    End Sub

    Private Sub DgvNotification_CellContextMenuStripNeededWithoutExcel(
        sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs)

        If e.RowIndex >= 0 AndAlso CType(sender, DataGridView).SelectedCells.Count > 0 Then
            e.ContextMenuStrip = Form1.DgvCopyWithoutExcelMenuStrip
        End If
    End Sub

    Private Sub DgvNotification_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)

        If e.Value.ToString().StartsWith("additionalInfo", StringComparison.OrdinalIgnoreCase) Then
            e.Value = e.Value.ToString.Replace(":", " : ")
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    Private Sub DgvNotification_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(.Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(.Name),
                wrapHeader:=False,
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

    Private Sub DisplayNotificationDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, row As Integer)
        Dim dGV As New DataGridView With {
            .AutoSize = False,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
            .ColumnHeadersVisible = False,
            .Dock = DockStyle.Top,
            .Name = $"DataGridView{className}",
            .RowHeadersVisible = False}
        realPanel.AutoSize = True
        realPanel.AutoSizeMode = AutoSizeMode.GrowOnly
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
    End Sub

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of SummaryRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Friend Sub UpdateNotificationTabs()
        Try
            Form1.TableLayoutPanelNotificationActive.AutoScroll = True
            Form1.TableLayoutPanelNotificationActive.SetTableName(ServerDataIndexes.notificationHistory, isClearedNotifications:=False)
            For i As Integer = Form1.TableLayoutPanelNotificationActive.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationActive.Controls.RemoveAt(i)
            Next

            Form1.TableLayoutPanelNotificationsCleared.SetTableName(ServerDataIndexes.notificationHistory, isClearedNotifications:=True)
            For i As Integer = Form1.TableLayoutPanelNotificationsCleared.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationsCleared.Controls.RemoveAt(i)
            Next
            CreateNotificationTables(notificationDictionary:=s_notificationHistoryValue)
            Form1.TableLayoutPanelNotificationsCleared.AutoScroll = True
        Catch ex As Exception
            Stop
            '       Throw
        End Try
    End Sub

    ''' <summary>
    '''  Attaches the handlers to the DataGridView for notifications.
    '''  This is used to set up the DataGridView for displaying notifications.
    '''  It includes handlers for context menu, cell formatting, column addition,
    '''  data binding completion, and layout events.
    ''' </summary>
    ''' <param name="dgv"></param>
    Public Sub AttachHandlers(dgv As DataGridView)
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

End Module
