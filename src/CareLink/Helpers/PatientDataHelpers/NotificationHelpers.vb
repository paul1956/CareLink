' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Text.RegularExpressions

''' <summary>
'''  Provides helper methods for displaying and managing notification data
'''  in <see cref="DataGridView"/> controls. Handles attaching event handlers,
'''  formatting, and layout for notification tables.
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
    ''' <param name="dgv">
    '''  The <see cref="DataGridView"/> to which the handlers will be attached.
    ''' </param>
    Private Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded,
            AddressOf DgvNotification_CellContextMenuStripNeededWithoutExcel
        RemoveHandler dgv.CellFormatting, AddressOf DgvNotification_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DgvNotification_ColumnAdded
        RemoveHandler dgv.DataBindingComplete, AddressOf DgvNotification_DataBindingComplete
        RemoveHandler dgv.Layout, AddressOf DgvNotification_Layout
        AddHandler dgv.CellContextMenuStripNeeded,
            AddressOf DgvNotification_CellContextMenuStripNeededWithoutExcel
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
            Invoke(owner:=My.Forms.Form1,
                   method:=Sub()
                               e.ContextMenuStrip = My.Forms.Form1.DgvCopyWithoutExcelMenuStrip
                           End Sub)
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event to format notification
    '''  cell values.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing formatting information.</param>
    Private Sub DgvNotification_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        ' Ignore header/invalid rows and only handle format the "Message" column
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Exit Sub
        End If

        Dim input As String = e.Value.ToString()

        ' Normalize spacing around colons but preserve any time-like tokens (HH:mm or HH:mm:ss)
        e.Value = NormalizeColonSpacingPreservingTimes(input)
        dgv.CellFormattingSetForegroundColor(e)

        If e.ColumnIndex <> dgv.Columns(columnName:="Message").Index Then
            Exit Sub
        End If
        Try
            ' Safely get the Key cell as a string (handles DBNull/Nothing)
            Dim keyValue As String =
                Convert.ToString(dgv.Rows(index:=e.RowIndex).Cells(columnName:="Key").Value)
            ' Only apply if Key column equals "backgroundColor"
            If keyValue.EqualsNoCase("backgroundColor") Then
                Dim colorString As String =
                    dgv.Rows(index:=e.RowIndex).Cells(columnName:="Value").Value?.ToString()

                ' Validate and parse color string
                If IsNotNullOrWhiteSpace(value:=colorString) AndAlso
                    colorString.StartsWithNoCase(value:="0x") Then
                    Dim argb As Integer
                    If Integer.TryParse(colorString.AsSpan(start:=2),
                                         style:=NumberStyles.HexNumber,
                                         provider:=Nothing,
                                         result:=argb) Then
                        ' Convert ARGB integer to Color
                        Dim c As Color = Color.FromArgb(argb)
                        e.CellStyle.BackColor = c
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(text:=$"Error formatting cell: {ex.Message}")
        End Try

    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event to configure
    '''  column properties for notification tables.
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
            Dim cellStyle As DataGridViewCellStyle =
                ClassPropertiesToColumnAlignment(Of SummaryRecord)(alignmentTable:=s_alignmentTable, .Name)

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
    Private Sub DgvNotification_DataBindingComplete(
            sender As Object,
            e As DataGridViewBindingCompleteEventArgs)

        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.ColumnCount > 0 Then
            dgv.ScrollBars = ScrollBars.None
            Dim dataGridViewLastColumn As DataGridViewColumn = dgv.Columns(index:=dgv.ColumnCount - 1)
            If dataGridViewLastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill Then
                dataGridViewLastColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.False
            End If
        End If
        dgv.ClearSelection()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.Layout"/> event to
    '''  adjust the DataGridView size to fit its rows.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Layout event arguments.</param>
    Private Sub DgvNotification_Layout(sender As Object, e As LayoutEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim dgvParent As TableLayoutPanel = CType(dgv.Parent, TableLayoutPanel)
        ' Calculate total height of rows and headers
        Dim height As Integer = 0
        For i As Integer = 0 To dgv.Rows.Count - 1
            Dim dgvRow As DataGridViewRow = dgv.Rows(index:=i)
            If dgvRow.Visible Then
                height += dgvRow.Height
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
        panel.RowStyles(index).Height = dgv.ClientSize.Height
        panel.BackColor = If(index > 2,
                             Color.White,
                             panel.BackColor)
        panel.BorderStyle = BorderStyle.FixedSingle
    End Sub

    ''' <summary>
    '''  Displays a notification data table in a <see cref="DataGridView"/> within
    '''  the specified panel.
    ''' </summary>
    ''' <param name="realPanel">
    '''  The <see cref="TableLayoutPanel"/> to add the DataGridView to.
    ''' </param>
    ''' <param name="table">The <see cref="DataTable"/> to display.</param>
    ''' <param name="className">The class name for naming the DataGridView.</param>
    ''' <param name="attachHandlers">
    '''  Delegate to attach event handlers to the DataGridView.
    ''' </param>
    Private Sub DisplayNotificationDataTableInDGV(ByRef realPanel As TableLayoutPanel,
                                                  table As DataTable,
                                                  className As String,
                                                  attachHandlers As attachHandlers)

        Dim dgv As New DataGridView With {
                .AutoSize = True,
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
                .BorderStyle = BorderStyle.None,
                .ColumnHeadersVisible = False,
                .Dock = DockStyle.Top,
                .Name = $"DataGridView{className}",
                .RowHeadersVisible = False}
        realPanel.AutoSize = True
        realPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
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
        dgv.InitializeDgv(dock:=DockStyle.Top)
        dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        attachHandlers?(dgv)
        For Each column As DataGridViewColumn In dgv.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        Next
        dgv.DataSource = table
    End Sub

    ''' <summary>
    '''  Creates notification tables for active and cleared notifications
    '''  and displays them in the main form.
    ''' </summary>
    ''' <param name="mainForm">The main form containing notification panels.</param>
    Friend Sub UpdateNotificationTabs(mainForm As Form1)
        Const rowIndex As ServerDataEnum = ServerDataEnum.notificationHistory

        Dim innerJson As List(Of Dictionary(Of String, String))
        Dim classCollection As List(Of SummaryRecord)
        Dim jsonDictionary As Dictionary(Of String, String)

        With mainForm.TlpNotificationsCleared
            .SetTableName(rowIndex, isClearedNotifications:=True)
            .Controls.Clear()
            .RowStyles.Clear()
            .RowCount = 0


            ' Force a full garbage collection and allow background GC if enabled
            GC.Collect(generation:=GC.MaxGeneration,
                   mode:=GCCollectionMode.Optimized,
                   blocking:=False,
                   compacting:=False)

            ' clearedNotifications
            Dim json As String = s_notificationHistoryValue(key:="clearedNotifications")
            innerJson = JsonToListOfDictionary(json)
            If innerJson.Count > 0 Then
                For Each jsonDictionary In innerJson
                    ClassCollection = GetSummaryRecords(jsonDictionary, rowsToHide:=s_rowsToHide)
                    DisplayNotificationDataTableInDGV(
                        realPanel:=mainForm.TlpNotificationsCleared,
                        table:=ClassCollectionToDataTable(ClassCollection),
                        className:=NameOf(SummaryRecord),
                        attachHandlers:=AddressOf AttachHandlers)
                Next
                .HorizontalScroll.Enabled = False
                .HorizontalScroll.Visible = False
            Else
                .AutoSizeMode = AutoSizeMode.GrowAndShrink
                Dim className As String = "clearedNotifications"
                .DgvNoRecordsFound(className)
            End If
            .AutoScroll = True
        End With

        ' activeNotifications
        innerJson = JsonToListOfDictionary(json:=s_notificationHistoryValue(key:="activeNotifications"))
        With mainForm.TlpNotificationActive
            If innerJson.Count > 0 Then
                .SetTableName(rowIndex, isClearedNotifications:=False)
                If .Controls.Count > 1 Then
                    .Controls.RemoveAt(index:=1)
                    If .RowStyles.Count > 1 Then
                        .RowStyles.RemoveAt(index:=1)
                    End If
                    .RowCount = 1
                End If
                For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                    jsonDictionary = innerDictionary.Value
                    classCollection = GetSummaryRecords(jsonDictionary, rowsToHide:=s_rowsToHide)
                    DisplayNotificationDataTableInDGV(
                        realPanel:=mainForm.TlpNotificationActive,
                        table:=ClassCollectionToDataTable(classCollection),
                        className:="ActiveNotifications",
                        attachHandlers:=AddressOf AttachHandlers)
                Next
            Else
                .AutoSizeMode = AutoSizeMode.GrowAndShrink
                .DgvNoRecordsFound(className:="activeNotification")
            End If
            .AutoScroll = True
        End With
        Application.DoEvents()
    End Sub

End Module
