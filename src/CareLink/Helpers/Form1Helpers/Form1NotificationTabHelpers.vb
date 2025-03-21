' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Form1NotificationTabHelpers

    Private ReadOnly s_rowsToHide As New List(Of String) From {
            NameOf(ActiveNotification.Version),
            NameOf(ClearedNotifications.RecordNumber),
            NameOf(ClearedNotifications.ReferenceGUID)}

    Private Sub CreateNotificationTables(notificationDictionary As Dictionary(Of String, String))
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationDictionary.WithIndex()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = JsonToLisOfDictionary(notificationType.Value)
            If notificationType.Key = "clearedNotifications" Then
                If innerJson.Count > 0 Then
                    innerJson.Reverse()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(
                            realPanel:=Form1.TableLayoutPanelNotificationsCleared,
                            table:=ClassCollectionToDataTable(listOfClass:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf ClearedNotificationHelpers.AttachHandlers,
                            rowIndex:=innerDictionary.Index + 1)
                    Next
                Else
                    Form1.TableLayoutPanelNotificationsCleared.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    Form1.TableLayoutPanelNotificationsCleared.DisplayEmptyDGV(className:="clearedNotifications")
                End If
            Else
                If innerJson.Count > 0 Then
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(
                            realPanel:=Form1.TableLayoutPanelNotificationActive,
                            table:=ClassCollectionToDataTable(listOfClass:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf ActiveNotificationHelpers.AttachHandlers,
                            rowIndex:=innerDictionary.Index + 1)
                    Next
                Else
                    Form1.TableLayoutPanelNotificationActive.AutoSizeMode = AutoSizeMode.GrowOnly
                    Form1.TableLayoutPanelNotificationActive.DisplayEmptyDGV(className:="activeNotification")
                End If
            End If
            Application.DoEvents()
        Next
    End Sub

    Private Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As Integer)
        Dim dGV As New DataGridView With {
            .AutoSize = True,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            .ColumnHeadersVisible = False,
            .Name = $"DataGridView{className}",
            .RowHeadersVisible = False
        }
        realPanel.Controls.Add(control:=dGV, column:=0, row:=rowIndex)
        dGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dGV.InitializeDgv()
        dGV.DataSource = table
        For Each column As DataGridViewColumn In dGV.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        Next
        realPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        realPanel.AutoSize = True
        attachHandlers?(dGV)
    End Sub

    Friend Sub UpdateNotificationTabs()
        Try
            Form1.TableLayoutPanelNotificationActive.AutoScroll = True
            Form1.TableLayoutPanelNotificationActive.SetTabName(ServerDataIndexes.notificationHistory, isClearedNotifications:=False)
            For i As Integer = Form1.TableLayoutPanelNotificationActive.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationActive.Controls.RemoveAt(i)
            Next

            Form1.TableLayoutPanelNotificationsCleared.SetTabName(ServerDataIndexes.notificationHistory, isClearedNotifications:=True)
            For i As Integer = Form1.TableLayoutPanelNotificationsCleared.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationsCleared.Controls.RemoveAt(i)
            Next
            CreateNotificationTables(notificationDictionary:=s_notificationHistoryValue)
            Form1.TableLayoutPanelNotificationsCleared.AutoScroll = True
        Catch ex As Exception
            Stop
            Throw
        End Try
    End Sub

End Module
