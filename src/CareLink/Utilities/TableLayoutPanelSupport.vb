' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TableLayoutPanelSupport

    Public Delegate Sub attachHandlers(dgv As DataGridView)

    Friend Sub CreateNotificationTables(notificationJson As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel, itemIndex As ItemIndexs, filterJsonData As Boolean, isScaledForm As Boolean)
        tableLevel1Blue.AutoScroll = False
        tableLevel1Blue.AutoSize = True
        tableLevel1Blue.BorderStyle = BorderStyle.FixedSingle
        tableLevel1Blue.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        tableLevel1Blue.Controls.Clear()
        tableLevel1Blue.ColumnCount = 2
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15.39028!))
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 84.60972!))
        tableLevel1Blue.Dock = DockStyle.Fill
        tableLevel1Blue.Location = New Point(6, 30)
        tableLevel1Blue.Name = "TableLayoutPanel1"
        tableLevel1Blue.RowCount = 2
        tableLevel1Blue.RowStyles.Clear()
        tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
        tableLevel1Blue.Size = New Size(1358, 597)
        tableLevel1Blue.TabIndex = 1

        Dim tableLayoutPanel2 As New TableLayoutPanel With {
            .AutoScroll = True,
            .AutoSize = True,
            .ColumnCount = 1,
            .Dock = DockStyle.Fill,
            .Name = $"tableLayoutPanel{itemIndex}",
            .RowCount = 1,
            .TabIndex = 0
        }
        tableLevel1Blue.BackColor = Color.LightBlue

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationJson.WithIndex()
            Application.DoEvents()
            Dim notificationType As KeyValuePair(Of String, String) = c.Value
            Dim innerJson As List(Of Dictionary(Of String, String)) = LoadList(notificationType.Value)

            tableLevel1Blue.Controls.Add(CreateBasicLabel(notificationType.Key), 0, c.Index)
            If innerJson.Count > 0 Then
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
                If notificationType.Key = "clearedNotifications" Then
                    innerJson.Reverse()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        DisplayDataTableInDGV(tableLayoutPanel2,
                                              ClassToDatatable(GetSummaryRecords(innerDictionary.Value, NotificationsRecordHelpers.rowsToHide).ToArray),
                                              NameOf(SummaryRecord),
                                              AddressOf SummaryRecordHelpers.AttachHandlers,
                                              innerDictionary.Index)
                    Next
                Else
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        tableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Absolute, 4 + (dic.Keys.Count * 22)))
                        Dim tableLevel3 As TableLayoutPanel = CreateTableLayoutPanel(NameOf(tableLevel3), 0, Color.Aqua)
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            Dim eValue As KeyValuePair(Of String, String) = e.Value

                            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) Then
                                If s_zFilterList(itemIndex).Contains(eValue.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3.RowCount += 1
                            tableLevel3.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0))

                            Dim valueLabel As Label = CreateBasicLabel(eValue.Key)
                            If eValue.Key.Equals("messageid", StringComparison.OrdinalIgnoreCase) Then
                                tableLevel3.Controls.AddRange({valueLabel, CreateBasicTextBox(eValue.Value)})
                                valueLabel = CreateBasicLabel("Message")
                            End If
                            tableLevel3.Controls.AddRange({valueLabel, CreateValueTextBox(dic, eValue, isScaledForm)})
                            Application.DoEvents()
                        Next
                        tableLevel3.Height += 40
                        tableLevel3.Controls.Add(tableLevel3, 0, innerDictionary.Index)
                        tableLevel3.Height += 4
                        tableLevel1Blue.Controls.Add(tableLevel3)
                        Application.DoEvents()
                    Next
                End If
            End If
        Next
        tableLevel1Blue.Controls.Add(tableLayoutPanel2, 1, 1)

        Application.DoEvents()
    End Sub

    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, dGV As DataGridView, table As DataTable, rowIndex As ItemIndexs)
        initializeTableLayoutPanel(realPanel, rowIndex)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As ItemIndexs)
        initializeTableLayoutPanel(realPanel, rowIndex)
        Dim dGV As DataGridView
        If realPanel.Controls.Count > 1 Then
            dGV = CType(realPanel.Controls(1), DataGridView)
        Else
            dGV = CreateDefaultDataGridView($"DataGridView{className}")
            realPanel.Controls.Add(dGV, 0, 1)
            attachHandlers(dGV)
        End If
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

    Friend Sub DisplayDataTableInDGV(realPanel As TableLayoutPanel, table As DataTable, className As String, attachHandlers As attachHandlers, rowIndex As Integer)
        Dim dGV As DataGridView = CreateDefaultDataGridView($"DataGridView{className}")
        dGV.AllowUserToResizeRows = False
        dGV.AutoSize = False
        dGV.ColumnHeadersVisible = False
        dGV.ReadOnly = True
        realPanel.Controls.Add(dGV, 0, rowIndex)
        attachHandlers(dGV)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
        dGV.Height = table.Rows.Count * 30
    End Sub

End Module
