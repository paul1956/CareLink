' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports TableLayputPanelTop

Friend Module InitializeTableLayoutPanelsTops

    Friend Sub InitializeTableLayoutPanelTops()
        Form1.TableLayoutPanelInsulinTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelMealTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelActiveInsulinTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelSgsTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelLimitsTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelNotificationHistoryTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelTherapyAlgorithmTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelBannerStateTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelBasalTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelLastSgTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelLastAlarmTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelAutoBasalDeliveryTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelAutoModeStatusTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelBgReadingsTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelCalibrationTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelLowGlucoseSuspendedTop = New TableLayoutPanelTopEx()
        Form1.TableLayoutPanelTimeChangeTop = New TableLayoutPanelTopEx()
        '
        ' TableLayoutPanelInsulinTop
        '
        Form1.TableLayoutPanelInsulinTop.AutoSize = True
        Form1.TableLayoutPanelInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelInsulinTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelInsulinTop.ColumnCount = 2
        Form1.TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelInsulinTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelInsulinTop.LabelText = "Insulin"
        Form1.TableLayoutPanelInsulinTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelInsulinTop.Name = "TableLayoutPanelInsulinTop"
        Form1.TableLayoutPanelInsulinTop.RowCount = 1
        Form1.TableLayoutPanelInsulinTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelInsulinTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelInsulinTop.TabIndex = 1
        Form1.TableLayoutPanelInsulin.Controls.Add(Form1.TableLayoutPanelInsulinTop, 0, 0)
        '
        ' TableLayoutPanelMealTop
        '
        Form1.TableLayoutPanelMealTop.AutoSize = True
        Form1.TableLayoutPanelMealTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelMealTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelMealTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelMealTop.ColumnCount = 2
        Form1.TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelMealTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelMealTop.LabelText = "Meal"
        Form1.TableLayoutPanelMealTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelMealTop.Name = "TableLayoutPanelMealTop"
        Form1.TableLayoutPanelMealTop.RowCount = 1
        Form1.TableLayoutPanelMealTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelMealTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelMealTop.TabIndex = 1
        Form1.TableLayoutPanelMeal.Controls.Add(Form1.TableLayoutPanelMealTop, 0, 0)
        '
        ' TableLayoutPanelActiveInsulinTop
        '
        Form1.TableLayoutPanelActiveInsulinTop.AutoSize = True
        Form1.TableLayoutPanelActiveInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelActiveInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelActiveInsulinTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelActiveInsulinTop.ColumnCount = 2
        Form1.TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelActiveInsulinTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelActiveInsulinTop.LabelText = "Active Insulin"
        Form1.TableLayoutPanelActiveInsulinTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelActiveInsulinTop.Name = "TableLayoutPanelActiveInsulinTop"
        Form1.TableLayoutPanelActiveInsulinTop.RowCount = 1
        Form1.TableLayoutPanelActiveInsulinTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelActiveInsulinTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelActiveInsulinTop.TabIndex = 1
        Form1.TableLayoutPanelActiveInsulin.Controls.Add(Form1.TableLayoutPanelActiveInsulinTop, 0, 0)
        '
        ' TableLayoutPanelSgsTop
        '
        Form1.TableLayoutPanelSgsTop.AutoSize = True
        Form1.TableLayoutPanelSgsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelSgsTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelSgsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelSgsTop.ColumnCount = 2
        Form1.TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelSgsTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelSgsTop.LabelText = "SGs"
        Form1.TableLayoutPanelSgsTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelSgsTop.Name = "TableLayoutPanelSgsTop"
        Form1.TableLayoutPanelSgsTop.RowCount = 1
        Form1.TableLayoutPanelSgsTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelSgsTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelSgsTop.TabIndex = 1
        Form1.TableLayoutPanelSgs.Controls.Add(Form1.TableLayoutPanelSgsTop, 0, 0)
        '
        ' TableLayoutPanelLimitsTop
        '
        Form1.TableLayoutPanelLimitsTop.AutoSize = True
        Form1.TableLayoutPanelLimitsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelLimitsTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelLimitsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelLimitsTop.ColumnCount = 2
        Form1.TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelLimitsTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelLimitsTop.LabelText = "Limits"
        Form1.TableLayoutPanelLimitsTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelLimitsTop.Name = "TableLayoutPanelLimitsTop"
        Form1.TableLayoutPanelLimitsTop.RowCount = 1
        Form1.TableLayoutPanelLimitsTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelLimitsTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelLimitsTop.TabIndex = 1
        Form1.TableLayoutPanelLimits.Controls.Add(Form1.TableLayoutPanelLimitsTop, 0, 0)
        '
        ' TableLayoutPanelNotificationHistoryTop
        '
        Form1.TableLayoutPanelNotificationHistoryTop.AutoSize = True
        Form1.TableLayoutPanelNotificationHistoryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelNotificationHistoryTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelNotificationHistoryTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelNotificationHistoryTop.ColumnCount = 2
        Form1.TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelNotificationHistoryTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelNotificationHistoryTop.LabelText = "Notification History"
        Form1.TableLayoutPanelNotificationHistoryTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelNotificationHistoryTop.Name = "TableLayoutPanelNotificationHistoryTop"
        Form1.TableLayoutPanelNotificationHistoryTop.RowCount = 1
        Form1.TableLayoutPanelNotificationHistoryTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelNotificationHistoryTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelNotificationHistoryTop.TabIndex = 1
        Form1.TableLayoutPanelNotificationHistory.Controls.Add(Form1.TableLayoutPanelNotificationHistoryTop, 1, 0)
        '
        ' TableLayoutPanelTherapyAlgorithmTop
        '
        Form1.TableLayoutPanelTherapyAlgorithmTop.AutoSize = True
        Form1.TableLayoutPanelTherapyAlgorithmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelTherapyAlgorithmTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelTherapyAlgorithmTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelTherapyAlgorithmTop.ColumnCount = 2
        Form1.TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelTherapyAlgorithmTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelTherapyAlgorithmTop.LabelText = "Therapy Algorithm"
        Form1.TableLayoutPanelTherapyAlgorithmTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelTherapyAlgorithmTop.Name = "TableLayoutPanelTherapyAlgorithmTop"
        Form1.TableLayoutPanelTherapyAlgorithmTop.RowCount = 1
        Form1.TableLayoutPanelTherapyAlgorithmTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelTherapyAlgorithmTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelTherapyAlgorithmTop.TabIndex = 1
        Form1.TableLayoutPanelTherapyAlgorithm.Controls.Add(Form1.TableLayoutPanelTherapyAlgorithmTop, 0, 0)
        '
        ' TableLayoutPanelBannerStateTop
        '
        Form1.TableLayoutPanelBannerStateTop.AutoSize = True
        Form1.TableLayoutPanelBannerStateTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelBannerStateTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelBannerStateTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelBannerStateTop.ColumnCount = 2
        Form1.TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelBannerStateTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelBannerStateTop.LabelText = "Banner State"
        Form1.TableLayoutPanelBannerStateTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelBannerStateTop.Name = "TableLayoutPanelBannerStateTop"
        Form1.TableLayoutPanelBannerStateTop.RowCount = 1
        Form1.TableLayoutPanelBannerStateTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelBannerStateTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelBannerStateTop.TabIndex = 1
        Form1.TableLayoutPanelBannerState.Controls.Add(Form1.TableLayoutPanelBannerStateTop, 0, 0)
        '
        ' TableLayoutPanelBasalTop
        '
        Form1.TableLayoutPanelBasalTop.AutoSize = True
        Form1.TableLayoutPanelBasalTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelBasalTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelBasalTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelBasalTop.ColumnCount = 2
        Form1.TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelBasalTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelBasalTop.LabelText = "Basal"
        Form1.TableLayoutPanelBasalTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelBasalTop.Name = "TableLayoutPanelBasalTop"
        Form1.TableLayoutPanelBasalTop.RowCount = 1
        Form1.TableLayoutPanelBasalTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelBasalTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelBasalTop.TabIndex = 1
        Form1.TableLayoutPanelBasal.Controls.Add(Form1.TableLayoutPanelBasalTop, 0, 0)
        '
        ' TableLayoutPanelLastSgTop
        '
        Form1.TableLayoutPanelLastSgTop.AutoSize = True
        Form1.TableLayoutPanelLastSgTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelLastSgTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelLastSgTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelLastSgTop.ColumnCount = 2
        Form1.TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelLastSgTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelLastSgTop.LabelText = "last SG"
        Form1.TableLayoutPanelLastSgTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelLastSgTop.Name = "TableLayoutPanelLastSgTop"
        Form1.TableLayoutPanelLastSgTop.RowCount = 1
        Form1.TableLayoutPanelLastSgTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelLastSgTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelLastSgTop.TabIndex = 1
        Form1.TableLayoutPanelLastSG.Controls.Add(Form1.TableLayoutPanelLastSgTop, 0, 0)
        '
        ' TableLayoutPanelLastAlarmTop
        '
        Form1.TableLayoutPanelLastAlarmTop.AutoSize = True
        Form1.TableLayoutPanelLastAlarmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelLastAlarmTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelLastAlarmTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelLastAlarmTop.ColumnCount = 2
        Form1.TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelLastAlarmTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelLastAlarmTop.LabelText = "Last Alarm"
        Form1.TableLayoutPanelLastAlarmTop.Location = New Point(3, 3)
        Form1.TableLayoutPanelLastAlarmTop.Name = "TableLayoutPanelLastAlarmTop"
        Form1.TableLayoutPanelLastAlarmTop.RowCount = 1
        Form1.TableLayoutPanelLastAlarmTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelLastAlarmTop.Size = New Size(1364, 37)
        Form1.TableLayoutPanelLastAlarmTop.TabIndex = 1
        Form1.TableLayoutPanelLastAlarm.Controls.Add(Form1.TableLayoutPanelLastAlarmTop, 0, 0)
        '
        ' TableLayoutPanelAutoBasalDeliveryTop
        '
        Form1.TableLayoutPanelAutoBasalDeliveryTop.AutoSize = True
        Form1.TableLayoutPanelAutoBasalDeliveryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelAutoBasalDeliveryTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelAutoBasalDeliveryTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelAutoBasalDeliveryTop.ColumnCount = 2
        Form1.TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelAutoBasalDeliveryTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelAutoBasalDeliveryTop.LabelText = "Basal"
        Form1.TableLayoutPanelAutoBasalDeliveryTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelAutoBasalDeliveryTop.Name = "TableLayoutPanelAutoBasalDeliveryTop"
        Form1.TableLayoutPanelAutoBasalDeliveryTop.RowCount = 1
        Form1.TableLayoutPanelAutoBasalDeliveryTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelAutoBasalDeliveryTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelAutoBasalDeliveryTop.TabIndex = 1
        Form1.TableLayoutPanelAutoBasalDelivery.Controls.Add(Form1.TableLayoutPanelAutoBasalDeliveryTop, 0, 0)
        '
        ' TableLayoutPanelAutoModeStatusTop
        '
        Form1.TableLayoutPanelAutoModeStatusTop.AutoSize = True
        Form1.TableLayoutPanelAutoModeStatusTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelAutoModeStatusTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelAutoModeStatusTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelAutoModeStatusTop.ColumnCount = 2
        Form1.TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelAutoModeStatusTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelAutoModeStatusTop.LabelText = "Auto Mode Status"
        Form1.TableLayoutPanelAutoModeStatusTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelAutoModeStatusTop.Name = "TableLayoutPanelAutoModeStatusTop"
        Form1.TableLayoutPanelAutoModeStatusTop.RowCount = 1
        Form1.TableLayoutPanelAutoModeStatusTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelAutoModeStatusTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelAutoModeStatusTop.TabIndex = 1
        Form1.TableLayoutPanelAutoModeStatus.Controls.Add(Form1.TableLayoutPanelAutoModeStatusTop, 0, 0)
        '
        ' TableLayoutPanelBgReadingsTop
        '
        Form1.TableLayoutPanelBgReadingsTop.AutoSize = True
        Form1.TableLayoutPanelBgReadingsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelBgReadingsTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelBgReadingsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelBgReadingsTop.ColumnCount = 2
        Form1.TableLayoutPanelBgReadingsTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelBgReadingsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelBgReadingsTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelBgReadingsTop.LabelText = "Sg Readings"
        Form1.TableLayoutPanelBgReadingsTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelBgReadingsTop.Name = "TableLayoutPanelBgReadingsTop"
        Form1.TableLayoutPanelBgReadingsTop.RowCount = 1
        Form1.TableLayoutPanelBgReadingsTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelBgReadingsTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelBgReadingsTop.TabIndex = 1
        Form1.TableLayoutPanelSgReadings.Controls.Add(Form1.TableLayoutPanelBgReadingsTop, 0, 0)
        '
        ' TableLayoutPanelCalibrationTop
        '
        Form1.TableLayoutPanelCalibrationTop.AutoSize = True
        Form1.TableLayoutPanelCalibrationTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelCalibrationTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelCalibrationTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelCalibrationTop.ColumnCount = 2
        Form1.TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelCalibrationTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelCalibrationTop.LabelText = "Calibration"
        Form1.TableLayoutPanelCalibrationTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelCalibrationTop.Name = "TableLayoutPanelCalibrationTop"
        Form1.TableLayoutPanelCalibrationTop.RowCount = 1
        Form1.TableLayoutPanelCalibrationTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelCalibrationTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelCalibrationTop.TabIndex = 1
        Form1.TableLayoutPanelCalibration.Controls.Add(Form1.TableLayoutPanelCalibrationTop, 0, 0)
        '
        ' TableLayoutPanelLowGlucoseSuspendedTop
        '
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.AutoSize = True
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.ColumnCount = 2
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.LabelText = "Low Glucose Suspended"
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.Name = "TableLayoutPanelLowGlucoseSuspendedTop"
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.RowCount = 1
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelLowGlucoseSuspendedTop.TabIndex = 1
        Form1.TableLayoutPanelLowGlucoseSuspended.Controls.Add(Form1.TableLayoutPanelLowGlucoseSuspendedTop, 0, 0)
        '
        ' TableLayoutPanelTimeChangeTop
        '
        Form1.TableLayoutPanelTimeChangeTop.AutoSize = True
        Form1.TableLayoutPanelTimeChangeTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Form1.TableLayoutPanelTimeChangeTop.ButtonText = "Return To 'Summary Data' Tab"
        Form1.TableLayoutPanelTimeChangeTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Form1.TableLayoutPanelTimeChangeTop.ColumnCount = 2
        Form1.TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle())
        Form1.TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Form1.TableLayoutPanelTimeChangeTop.Dock = DockStyle.Fill
        Form1.TableLayoutPanelTimeChangeTop.LabelText = "Time Change"
        Form1.TableLayoutPanelTimeChangeTop.Location = New Point(6, 6)
        Form1.TableLayoutPanelTimeChangeTop.Name = "TableLayoutPanelTimeChangeTop"
        Form1.TableLayoutPanelTimeChangeTop.RowCount = 1
        Form1.TableLayoutPanelTimeChangeTop.RowStyles.Add(New RowStyle())
        Form1.TableLayoutPanelTimeChangeTop.Size = New Size(1358, 37)
        Form1.TableLayoutPanelTimeChangeTop.TabIndex = 1
        Form1.TableLayoutPanelTimeChange.Controls.Add(Form1.TableLayoutPanelTimeChangeTop, 0, 0)

    End Sub

End Module
