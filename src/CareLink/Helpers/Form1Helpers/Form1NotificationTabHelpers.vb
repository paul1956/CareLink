' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Form1NotificationTabHelpers

    Private ReadOnly s_rowsToHide As New List(Of String) From {
            NameOf(ActiveNotification.GUID),
            NameOf(ActiveNotification.instanceId),
            NameOf(ActiveNotification.kind),
            NameOf(ActiveNotification.relativeOffset),
            NameOf(ActiveNotification.version),
            NameOf(ClearedNotificationsRecord.faultId),
            NameOf(ClearedNotificationsRecord.pnpId),
            NameOf(ClearedNotificationsRecord.RecordNumber),
            NameOf(ClearedNotificationsRecord.referenceGUID)
        }

    Private Sub CreateNotificationTables(notificationDictionary As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel)
        tableLevel1Blue.AutoScroll = True
        tableLevel1Blue.AutoSize = True
        tableLevel1Blue.BorderStyle = BorderStyle.FixedSingle
        tableLevel1Blue.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        tableLevel1Blue.Controls.Clear()
        tableLevel1Blue.ColumnCount = 2
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        tableLevel1Blue.Dock = DockStyle.Fill
        tableLevel1Blue.Location = New Point(6, 30)
        tableLevel1Blue.Name = "TableLayoutPanelNotificationHistoryTop"
        tableLevel1Blue.RowCount = 2
        tableLevel1Blue.RowStyles.Clear()
        tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tableLevel1Blue.TabIndex = 1

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationDictionary.WithIndex()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = JsonToLisOfDictionary(notificationType.Value)
            Dim tableLayoutPanel2 As New TableLayoutPanel With {
             .AutoScroll = False,
             .AutoSize = True,
             .ColumnCount = 1,
             .Dock = DockStyle.Fill,
             .Name = $"tableLayoutPanel{c.Index}",
             .RowCount = 1,
             .TabIndex = 0
         }
            Dim control As New Label With {
                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                .AutoSize = True,
                .TextAlign = ContentAlignment.MiddleLeft,
                .Text = notificationType.Key.ToTitleCase.Replace(" ", vbCrLf)}

            tableLevel1Blue.Controls.Add(control, 0, c.Index)
            If innerJson.Count > 0 Then
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
                If notificationType.Key = "clearedNotifications" Then
                    tableLayoutPanel2.BackColor = Color.Green
                    innerJson.Reverse()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(tableLayoutPanel2,
                                              ClassCollectionToDataTable(GetSummaryRecords(innerDictionary.Value, s_rowsToHide)),
                                              NameOf(SummaryRecord),
                                              AddressOf SummaryRecordHelpers.AttachHandlers,
                                              innerDictionary.Index
                                             )
                    Next
                Else
                    tableLayoutPanel2.BackColor = Color.PaleVioletRed
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(tableLayoutPanel2,
                                              ClassCollectionToDataTable(GetSummaryRecords(innerDictionary.Value, s_rowsToHide)),
                                              NameOf(SummaryRecord),
                                              AddressOf SummaryRecordHelpers.AttachHandlers,
                                              innerDictionary.Index
                                             )
                    Next
                End If
                tableLevel1Blue.Controls.Add(tableLayoutPanel2, 1, c.Index)
            End If
        Next
    End Sub

    Private Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As Integer)
        Dim dGV As New DataGridView With {
            .AutoSize = False,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            .ColumnHeadersVisible = False,
            .Height = table.Rows.Count * 30,
            .Name = $"DataGridView{className}",
            .RowHeadersVisible = False
        }
        dGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dGV.InitializeDgv()
        realPanel.Controls.Add(dGV, 0, rowIndex)
        attachHandlers(dGV)
        dGV.DataSource = table
    End Sub

    Friend Sub UpdateNotificationTab()
        Try
            Form1.TableLayoutPanelNotificationHistory.AutoScroll = True
            Form1.TableLayoutPanelNotificationHistory.SetTabName(ItemIndexes.notificationHistory)
            Dim tableLevel1Blue As New TableLayoutPanel With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .AutoScroll = True,
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .BackColor = Color.LightBlue,
                    .BorderStyle = BorderStyle.FixedSingle,
                    .ColumnCount = 2,
                    .Dock = DockStyle.Fill,
                    .Margin = New Padding(3),
                    .Name = NameOf(tableLevel1Blue),
                    .Padding = New Padding(3),
                    .RowCount = 0
                }
            For i As Integer = Form1.TableLayoutPanelNotificationHistory.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationHistory.Controls.RemoveAt(i)
            Next
            Form1.TableLayoutPanelNotificationHistory.Controls.Add(tableLevel1Blue, 0, 1)
            CreateNotificationTables(
                notificationDictionary:=s_notificationHistoryValue,
                tableLevel1Blue)
        Catch ex As Exception
            Stop
            Throw
        End Try
    End Sub

End Module
