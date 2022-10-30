' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Friend Module TabHelpers

    <Extension>
    Friend Sub UpdateMarkerTabs(MyForm As Form1)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelAutoBasalDelivery,
                              MyForm.DataGridViewAutoBasalDelivery,
                              ClassToDatatable(s_listOfAutoBasalDeliveryMarkers.ToArray),
                              ItemIndexs.markers)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelAutoModeStatus,
                              ClassToDatatable(s_listOfAutoModeStatusMarkers.ToArray),
                              NameOf(AutoModeStatusRecord),
                              AddressOf AutoModeStatusRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelBgReadings,
                              ClassToDatatable(s_listOfBgReadingMarkers.ToArray),
                              NameOf(BGReadingRecord),
                              AddressOf BGReadingRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelInsulin,
                              MyForm.DataGridViewInsulin,
                              ClassToDatatable(s_listOfInsulinMarkers.ToArray),
                              ItemIndexs.markers)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelMeal,
                              ClassToDatatable(s_listOfMealMarkers.ToArray),
                              NameOf(MealRecord),
                              AddressOf MealRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelCalibration,
                              ClassToDatatable(s_listOfCalibrationMarkers.ToArray),
                              NameOf(CalibrationRecord),
                              AddressOf CalibrationRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelLowGlucoseSuspended,
                              ClassToDatatable(s_listOfLowGlucoseSuspendedMarkers.ToArray),
                              NameOf(LowGlusoceSuspendRecord),
                              AddressOf LowGlusoceSuspendRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelTimeChange,
                              ClassToDatatable(s_listOfTimeChangeMarkers.ToArray),
                              NameOf(TimeChangeRecord),
                              AddressOf TimeChangeRecordHelpers.AttachHandlers,
                              ItemIndexs.markers,
                              False)
    End Sub

    <Extension>
    Friend Sub UpdateSgsTab(MyForm As Form1)
        DisplayDataTableInDGV(MyForm.TableLayoutPanelSgs,
                              MyForm.DataGridViewSGs,
                              ClassToDatatable(s_listOfSGs.ToArray),
                              ItemIndexs.sgs)
        MyForm.DataGridViewSGs.Sort(MyForm.DataGridViewSGs.Columns(0), ListSortDirection.Descending)
    End Sub

    <Extension>
    Friend Sub UpdateSummaryTab(MyForm As Form1)
        s_listOfSummaryRecords.Sort()
        MyForm.DataGridViewSummary.DataSource = ClassToDatatable(s_listOfSummaryRecords.ToArray)
        MyForm.DataGridViewSummary.RowHeadersVisible = False
    End Sub

    <Extension>
    Friend Sub UpdateTransmitterBatttery(MyForm As Form1)
        Dim gstBatteryLevel As Integer = CInt(s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.gstBatteryLevel), False))
        MyForm.TransmatterBatterPercentLabel.Text = $"{gstBatteryLevel}%"
        If CBool(s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.conduitSensorInRange))) Then
            Select Case gstBatteryLevel
                Case 100
                    MyForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryFull
                Case > 50
                    MyForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryOK
                Case > 20
                    MyForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryMedium
                Case > 0
                    MyForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryLow
            End Select
        Else
            MyForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryUnknown
            MyForm.TransmatterBatterPercentLabel.Text = $"???"
        End If

    End Sub

End Module
