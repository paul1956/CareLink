' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module NotificationTabHelpers

    Private Sub CreateNotificationTables(notificationDictionary As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel, itemIndex As ItemIndexes, filterJsonData As Boolean)
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
        tableLevel1Blue.Name = "TableLayoutPanelSgsTop"
        tableLevel1Blue.RowCount = 2
        tableLevel1Blue.RowStyles.Clear()
        tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tableLevel1Blue.TabIndex = 1

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationDictionary.WithIndex()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = LoadList(notificationType.Value)
            Dim tableLayoutPanel2 As New TableLayoutPanel With {
             .AutoScroll = False,
             .AutoSize = True,
             .ColumnCount = 1,
             .Dock = DockStyle.Fill,
             .Name = $"tableLayoutPanel{c.Index}",
             .RowCount = 1,
             .TabIndex = 0
         }
            tableLevel1Blue.Controls.Add(New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                .AutoSize = True,
                                                                .TextAlign = ContentAlignment.MiddleLeft,
                                                                .Text = notificationType.Key.ToTitleCase.Replace(" ", Environment.NewLine)
                                                               },
                                         0,
                                         c.Index)
            If innerJson.Count > 0 Then
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
                If notificationType.Key = "clearedNotifications" Then
                    tableLayoutPanel2.BackColor = Color.Green
                    innerJson.Reverse()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        tableLayoutPanel2.DisplayDataTableInDGV(
                            ClassCollectionToDataTable(GetSummaryRecords(innerDictionary.Value, NotificationsRecordHelpers.s_rowsToHide)),
                            NameOf(SummaryRecord),
                            AddressOf SummaryRecordHelpers.AttachHandlers,
                            innerDictionary.Index)
                    Next
                Else
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        tableLayoutPanel2.BackColor = Color.PaleVioletRed
                        tableLayoutPanel2.DisplayDataTableInDGV(
                            ClassCollectionToDataTable(GetSummaryRecords(innerDictionary.Value, NotificationsRecordHelpers.s_rowsToHide)),
                            NameOf(SummaryRecord),
                            AddressOf SummaryRecordHelpers.AttachHandlers,
                            innerDictionary.Index)
                    Next
                End If
                tableLevel1Blue.Controls.Add(tableLayoutPanel2, 1, c.Index)
            End If
        Next
    End Sub

    <Extension>
    Friend Sub UpdateNotificationTab(mainForm As Form1)
        Try
            mainForm.TableLayoutPanelNotificationHistory.AutoScroll = True
            mainForm.TableLayoutPanelNotificationHistory.SetTabName(ItemIndexes.notificationHistory)
            Dim innerTableBlue As New TableLayoutPanel With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .AutoScroll = True,
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .BackColor = Color.LightBlue,
                    .BorderStyle = BorderStyle.FixedSingle,
                    .ColumnCount = 2,
                    .Dock = DockStyle.Fill,
                    .Margin = New Padding(3),
                    .Name = NameOf(innerTableBlue),
                    .Padding = New Padding(3),
                    .RowCount = 0
                }
            For i As Integer = mainForm.TableLayoutPanelNotificationHistory.Controls.Count - 1 To 1 Step -1
                mainForm.TableLayoutPanelNotificationHistory.Controls.RemoveAt(i)
            Next
            mainForm.TableLayoutPanelNotificationHistory.Controls.Add(innerTableBlue, 0, 1)
            CreateNotificationTables(s_notificationHistoryValue,
                                     innerTableBlue,
                                     ItemIndexes.notificationHistory,
                                     s_filterJsonData)
        Catch ex As Exception
            Stop
            Throw
        End Try
    End Sub

End Module
