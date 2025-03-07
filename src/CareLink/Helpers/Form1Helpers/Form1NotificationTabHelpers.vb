' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Form1NotificationTabHelpers

    Private ReadOnly s_rowsToHide As New List(Of String) From {
            NameOf(ActiveNotification.instanceId),
            NameOf(ActiveNotification.relativeOffset),
            NameOf(ActiveNotification.version),
            NameOf(ActiveNotification.pnpId),
            NameOf(ClearedNotifications.RecordNumber),
            NameOf(ClearedNotifications.ReferenceGUID)
        }

    Private Sub CreateNotificationTables(notificationDictionary As Dictionary(Of String, String))
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationDictionary.WithIndex()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = JsonToLisOfDictionary(notificationType.Value)
            Stop
            If notificationType.Key = "clearedNotifications" Then
                If innerJson.Count > 0 Then
                    innerJson.Reverse()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(
                            realPanel:=Form1.TableLayoutPanelNotificationsCleared,
                            table:=ClassCollectionToDataTable(classCollection:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf SummaryHelpers.AttachHandlers,
                            rowIndex:=innerDictionary.Index + 1)
                    Next
                Else
                    Form1.TableLayoutPanelNotificationActiveTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    DisplayEmptyDGV(realPanel:=Form1.TableLayoutPanelNotificationsCleared, name:="clearedNotifications")
                End If
            Else
                If innerJson.Count > 0 Then
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(
                            realPanel:=Form1.TableLayoutPanelNotificationActive,
                            table:=ClassCollectionToDataTable(classCollection:=GetSummaryRecords(dic:=innerDictionary.Value, rowsToHide:=s_rowsToHide)),
                            className:=NameOf(SummaryRecord),
                            attachHandlers:=AddressOf SummaryHelpers.AttachHandlers,
                            rowIndex:=innerDictionary.Index + 1)
                    Next
                Else
                    Form1.TableLayoutPanelNotificationActiveTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    DisplayEmptyDGV(realPanel:=Form1.TableLayoutPanelNotificationActive, name:="activeNotification")
                End If
            End If

        Next
    End Sub

    Private Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As Integer)
        Dim dGV As New DataGridView With {
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            .ColumnHeadersVisible = False,
            .Name = $"DataGridView{className}",
            .RowHeadersVisible = False
        }
        realPanel.Controls.Add(dGV, 0, rowIndex)
        dGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dGV.InitializeDgv()
        dGV.DataSource = table
        dGV.AutoSize = True
        realPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        realPanel.AutoSize = True
        attachHandlers(dGV)
    End Sub

    Private Sub DisplayEmptyDGV(realPanel As TableLayoutPanel, name As String)
        Dim dGV As New DataGridView With {
            .AutoGenerateColumns = False,
            .AutoSize = False,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            .ColumnHeadersVisible = False,
            .DataSource = Nothing,
            .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold),
            .Name = $"DataGridView{name}",
            .RowHeadersVisible = False}
        dGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dGV.Columns.Clear()

        Dim colMessage As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Message",
            .HeaderText = "Message",
            .ReadOnly = True,
            .MinimumWidth = 150,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader}

        dGV.Columns.Add(colMessage)

        Dim dt As New DataTable()
        dt.Columns.Add("Message")
        Dim newRow As DataRow = dt.NewRow()
        newRow("Message") = "No records found"
        dt.Rows.Add(newRow)

        realPanel.Controls.Add(control:=dGV, column:=0, row:=1)
        Dim bs As New BindingSource With {.DataSource = dt}
        dGV.DataSource = bs
    End Sub

    Friend Sub UpdateNotificationTabs()
        Try
            Form1.TableLayoutPanelNotificationActive.AutoScroll = True
            Form1.TableLayoutPanelNotificationActive.SetTabName(ServerDataIndexes.notificationHistory, isClearedNotifications:=False)
            For i As Integer = Form1.TableLayoutPanelNotificationActive.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationsCleared.Controls.RemoveAt(i)
            Next
            Form1.TableLayoutPanelNotificationsCleared.AutoScroll = True
            Form1.TableLayoutPanelNotificationsCleared.SetTabName(ServerDataIndexes.notificationHistory, isClearedNotifications:=True)
            For i As Integer = Form1.TableLayoutPanelNotificationsCleared.Controls.Count - 1 To 1 Step -1
                Form1.TableLayoutPanelNotificationsCleared.Controls.RemoveAt(i)
            Next
            CreateNotificationTables(notificationDictionary:=s_notificationHistoryValue)
        Catch ex As Exception
            Stop
            Throw
        End Try
    End Sub

End Module
