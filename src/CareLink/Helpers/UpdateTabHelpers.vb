﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Friend Module UpdateTabHelpers

    Private Sub CreateNotificationTables(notificationJson As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel, itemIndex As ItemIndexs, filterJsonData As Boolean)
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

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In notificationJson.WithIndex()
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
                    tableLayoutPanel2.BackColor = Color.LightGreen
                    innerJson.Reverse()
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        tableLayoutPanel2.DisplayDataTableInDGV(
                            ClassToDatatable(GetSummaryRecords(innerDictionary.Value, NotificationsRecordHelpers.rowsToHide).ToArray),
                            NameOf(SummaryRecord),
                            AddressOf SummaryRecordHelpers.AttachHandlers,
                            innerDictionary.Index)
                    Next
                Else
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        tableLayoutPanel2.BackColor = Color.PaleVioletRed
                        tableLayoutPanel2.DisplayDataTableInDGV(
                            ClassToDatatable(GetSummaryRecords(innerDictionary.Value, NotificationsRecordHelpers.rowsToHide).ToArray),
                            NameOf(SummaryRecord),
                            AddressOf SummaryRecordHelpers.AttachHandlers,
                            innerDictionary.Index)
                    Next
                End If
                tableLevel1Blue.Controls.Add(tableLayoutPanel2, 1, c.Index)
            End If
        Next
        Application.DoEvents()
    End Sub

    <Extension>
    Friend Sub UpdateMarkerTabs(MainForm As Form1)
        With MainForm
            .TableLayoutPanelAutoBasalDelivery.DisplayDataTableInDGV(
                              .DataGridViewAutoBasalDelivery,
                              ClassToDatatable(s_listOfAutoBasalDeliveryMarkers.ToArray),
                              ItemIndexs.markers)
            .TableLayoutPanelAutoModeStatus.DisplayDataTableInDGV(
                              ClassToDatatable(
                              s_listOfAutoModeStatusMarkers.ToArray),
                              NameOf(AutoModeStatusRecord),
                              AddressOf AutoModeStatusRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
            .TableLayoutPanelBgReadings.DisplayDataTableInDGV(
                              ClassToDatatable(
                              s_listOfBgReadingMarkers.ToArray),
                              NameOf(BGReadingRecord),
                              AddressOf BGReadingRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
            .TableLayoutPanelInsulin.DisplayDataTableInDGV(
                              .DataGridViewInsulin,
                              ClassToDatatable(s_listOfInsulinMarkers.ToArray),
                              ItemIndexs.markers)
            .TableLayoutPanelMeal.DisplayDataTableInDGV(
                              ClassToDatatable(s_listOfMealMarkers.ToArray),
                              NameOf(MealRecord),
                              AddressOf MealRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
            .TableLayoutPanelCalibration.DisplayDataTableInDGV(
                              ClassToDatatable(s_listOfCalibrationMarkers.ToArray),
                              NameOf(CalibrationRecord),
                              AddressOf CalibrationRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
            .TableLayoutPanelLowGlucoseSuspended.DisplayDataTableInDGV(
                              ClassToDatatable(s_listOfLowGlucoseSuspendedMarkers.ToArray),
                              NameOf(LowGlusoceSuspendRecord),
                              AddressOf LowGlusoceSuspendRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
            .TableLayoutPanelTimeChange.DisplayDataTableInDGV(
                              ClassToDatatable(s_listOfTimeChangeMarkers.ToArray),
                              NameOf(TimeChangeRecord),
                              AddressOf TimeChangeRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
        End With

    End Sub

    <Extension>
    Friend Sub UpdateNotificationTab(MainForm As Form1)
        Try
            MainForm.TableLayoutPanelNotificationHistory.AutoScroll = True
            MainForm.TableLayoutPanelNotificationHistory.SetTabName(ItemIndexs.notificationHistory)
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
            For i As Integer = MainForm.TableLayoutPanelNotificationHistory.Controls.Count - 1 To 1 Step -1
                MainForm.TableLayoutPanelNotificationHistory.Controls.RemoveAt(i)
            Next
            MainForm.TableLayoutPanelNotificationHistory.Controls.Add(innerTableBlue, 0, 1)
            CreateNotificationTables(s_notificationHistoryValue,
                                     innerTableBlue,
                                     ItemIndexs.notificationHistory,
                                     s_filterJsonData)
        Catch ex As Exception
            Stop
            Throw
        End Try
    End Sub

    <Extension>
    Friend Sub UpdatePumpBannerStateTab(MainForm As Form1)
        Dim listOfPumpBannerState As New List(Of BannerStateRecord)
        For Each dic As Dictionary(Of String, String) In s_pumpBannerStateValue
            Dim typeValue As String = ""
            If dic.TryGetValue("type", typeValue) Then
                Dim bannerStateRecord1 As BannerStateRecord = DictionaryToClass(Of BannerStateRecord)(dic, listOfPumpBannerState.Count + 1)
                listOfPumpBannerState.Add(bannerStateRecord1)
                Select Case typeValue
                    Case "TEMP_TARGET"
                        Dim minutes As Integer = bannerStateRecord1.timeRemaining
                        MainForm.TempTargetLabel.Text = $"Target 150   {New TimeSpan(0, minutes \ 60, minutes Mod 60).ToString.Substring(4)} hr"
                        MainForm.TempTargetLabel.Visible = True
                    Case "BG_REQUIRED"
                    Case "DELIVERY_SUSPEND"
                    Case "LOAD_RESERVOIR"
                    Case "PROCESSING_BG"
                    Case "SUSPENDED_BEFORE_LOW"
                    Case "TEMP_BASAL"
                    Case "WAIT_TO_ENTER_BG"
                    Case Else
                        If Debugger.IsAttached Then
                            MsgBox($"{typeValue} is unknown banner message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                        End If
                End Select
            Else
                Stop
            End If
        Next
        MainForm.TableLayoutPanelBannerState.DisplayDataTableInDGV(
                              ClassToDatatable(listOfPumpBannerState.ToArray),
                              NameOf(BannerStateRecord),
                              AddressOf BannerStateRecordHelpers.AttachHandlers,
                              ItemIndexs.pumpBannerState,
                              False)
    End Sub

    <Extension>
    Friend Sub UpdateSgsTab(MainForm As Form1)
        DisplayDataTableInDGV(MainForm.TableLayoutPanelSgs,
                              MainForm.DataGridViewSGs,
                              ClassToDatatable(s_listOfSGs.ToArray),
                              ItemIndexs.sgs)
        MainForm.DataGridViewSGs.Sort(MainForm.DataGridViewSGs.Columns(0), ListSortDirection.Descending)
    End Sub

    <Extension>
    Friend Sub UpdateSummaryTab(MainForm As Form1)
        s_listOfSummaryRecords.Sort()
        MainForm.DataGridViewSummary.DataSource = ClassToDatatable(s_listOfSummaryRecords.ToArray)
        MainForm.DataGridViewSummary.RowHeadersVisible = False
    End Sub

End Module