<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip1 = New MenuStrip()
        Me.MenuStartHere = New ToolStripMenuItem()
        Me.MenuStartHereLogin = New ToolStripMenuItem()
        Me.ToolStripSeparator1 = New ToolStripSeparator()
        Me.MenuStartHereLoadSavedDataFile = New ToolStripMenuItem()
        Me.MenuStartHereExceptionReportLoad = New ToolStripMenuItem()
        Me.ToolStripSeparator4 = New ToolStripSeparator()
        Me.MenuStartHereUseLastSavedFile = New ToolStripMenuItem()
        Me.MenuStartHereUseTestData = New ToolStripMenuItem()
        Me.ToolStripSeparator2 = New ToolStripSeparator()
        Me.MenuStartHereSnapshotSave = New ToolStripMenuItem()
        Me.ToolStripSeparator3 = New ToolStripSeparator()
        Me.StartHereExit = New ToolStripMenuItem()
        Me.MenuOptions = New ToolStripMenuItem()
        Me.MenuOptionsColorPicker = New ToolStripMenuItem()
        Me.MenuOptionsShowLegend = New ToolStripMenuItem()
        Me.ToolStripSeparator5 = New ToolStripSeparator()
        Me.MenuOptionsAutoLogin = New ToolStripMenuItem()
        Me.ToolStripSeparator6 = New ToolStripSeparator()
        Me.OptionsMenuAdvancedOptions = New ToolStripMenuItem()
        Me.MenuOptionsFilterRawJSONData = New ToolStripMenuItem()
        Me.MenuOptionsUseLocalTimeZone = New ToolStripMenuItem()
        Me.ToolStripSeparator7 = New ToolStripSeparator()
        Me.MenuOptionsEditPumpSettings = New ToolStripMenuItem()
        Me.MenuHelp = New ToolStripMenuItem()
        Me.MenuHelpReportAnIssue = New ToolStripMenuItem()
        Me.MenuHelpCheckForUpdates = New ToolStripMenuItem()
        Me.MenuHelpAbout = New ToolStripMenuItem()
        Me.MenuShowMiniDisplay = New ToolStripMenuItem()
        Me.AboveHighLimitMessageLabel = New Label()
        Me.AboveHighLimitValueLabel = New Label()
        Me.ActiveInsulinValue = New Label()
        Me.FullNameLabel = New Label()
        Me.AverageSGMessageLabel = New Label()
        Me.AverageSGValueLabel = New Label()
        Me.BannerStateButton = New Button()
        Me.BannerStateLabel = New Label()
        Me.BasalButton = New Button()
        Me.BasalLabel = New Label()
        Me.BelowLowLimitMessageLabel = New Label()
        Me.BelowLowLimitValueLabel = New Label()
        Me.CalibrationDueImage = New PictureBox()
        Me.CalibrationShieldPanel = New Panel()
        Me.TempTargetLabel = New Label()
        Me.ShieldUnitsLabel = New Label()
        Me.LastSGTimeLabel = New Label()
        Me.CurrentBGLabel = New Label()
        Me.SensorMessage = New Label()
        Me.CalibrationShieldPictureBox = New PictureBox()
        Me.CareLinkUserDataRecordBindingSource = New BindingSource(components)
        Me.CursorMessage1Label = New Label()
        Me.CursorMessage2Label = New Label()
        Me.CursorMessage3Label = New Label()
        Me.CursorPanel = New Panel()
        Me.CursorPictureBox = New PictureBox()
        Me.CursorTimer = New Timer(components)
        Me.DgvAutoBasalDelivery = New DataGridView()
        Me.DgvCountryDataPg1 = New DataGridView()
        Me.DgvCountryDataPg1RecordNumber = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg1Category = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg1Key = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg1Value = New DataGridViewTextBoxColumn()
        Me.DgvCareLinkUsers = New DataGridView()
        Me.DgvCountryDataPg3 = New DataGridView()
        Me.DgvCountryDataPg3RecordNumber = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg3Category = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg3Key = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg3Value = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg3OnlyFor = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg3NotFor = New DataGridViewTextBoxColumn()
        Me.DgvCurrentUser = New DataGridView()
        Me.DgvInsulin = New DataGridView()
        Me.DgvMeal = New DataGridView()
        Me.DgvSGs = New DataGridView()
        Me.DgvSummary = New DataGridView()
        Me.DgvSessionProfile = New DataGridView()
        Me.ImageList1 = New ImageList(components)
        Me.InRangeMessageLabel = New Label()
        Me.InsulinLevelPictureBox = New PictureBox()
        Me.LabelSgTrend = New Label()
        Me.LabelTimeChange = New Label()
        Me.LabelTrendArrows = New Label()
        Me.LabelTrendValue = New Label()
        Me.Last24AutoCorrectionLabel = New Label()
        Me.Last24CarbsValueLabel = New Label()
        Me.Last24DailyDoseLabel = New Label()
        Me.Last24HourBasalLabel = New Label()
        Me.Last24HoursLabel = New Label()
        Me.Last24HTotalsPanel = New Panel()
        Me.Last24TotalsLabel = New Label()
        Me.Last24ManualBolusLabel = New Label()
        Me.MaxBasalPerHourLabel = New Label()
        Me.ModelLabel = New Label()
        Me.PumpNameLabel = New Label()
        Me.NotifyIcon1 = New NotifyIcon(components)
        Me.PumpBatteryPictureBox = New PictureBox()
        Me.PumpBatteryRemainingLabel = New Label()
        Me.InsulinTypeLabel = New Label()
        Me.ReadingsLabel = New Label()
        Me.RemainingInsulinUnits = New Label()
        Me.SensorDaysLeftLabel = New Label()
        Me.SensorTimeLeftLabel = New Label()
        Me.SensorTimeLeftPanel = New Panel()
        Me.SensorTimeLeftPictureBox = New PictureBox()
        Me.SerialNumberLabel = New Label()
        Me.ServerUpdateTimer = New Timer(components)
        Me.SplitContainer2 = New SplitContainer()
        Me.PumpAITLabel = New Label()
        Me.PumpBatteryRemaining2Label = New Label()
        Me.TransmitterBatteryPercentLabel = New Label()
        Me.TransmitterBatteryPictureBox = New PictureBox()
        Me.SplitContainer3 = New SplitContainer()
        Me.TimeInRangeLabel = New Label()
        Me.TimeInRangeSummaryPercentCharLabel = New Label()
        Me.TimeInRangeChartLabel = New Label()
        Me.TimeInRangeValueLabel = New Label()
        Me.SmartGuardLabel = New Label()
        Me.TabControlPage1 = New TabControl()
        Me.TabPage01HomePage = New TabPage()
        Me.TabPage02RunningIOB = New TabPage()
        Me.SplitContainer1 = New SplitContainer()
        Me.TemporaryUseAdvanceAITDecayCheckBox = New CheckBox()
        Me.TabPage03TreatmentDetails = New TabPage()
        Me.TabPage04SummaryData = New TabPage()
        Me.TabPage05Insulin = New TabPage()
        Me.TableLayoutPanelInsulin = New TableLayoutPanel()
        Me.TableLayoutPanelInsulinTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage06Meal = New TabPage()
        Me.TableLayoutPanelMeal = New TableLayoutPanel()
        Me.TableLayoutPanelMealTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage07ActiveInsulin = New TabPage()
        Me.TableLayoutPanelActiveInsulin = New TableLayoutPanel()
        Me.TableLayoutPanelActiveInsulinTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage08SensorGlucose = New TabPage()
        Me.TableLayoutPanelSgs = New TableLayoutPanel()
        Me.TableLayoutPanelSgsTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage09Limits = New TabPage()
        Me.TableLayoutPanelLimits = New TableLayoutPanel()
        Me.TableLayoutPanelLimitsTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage10NotificationHistory = New TabPage()
        Me.TableLayoutPanelNotificationHistory = New TableLayoutPanel()
        Me.TableLayoutPanelNotificationHistoryTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage11TherapyAlgorithm = New TabPage()
        Me.TableLayoutPanelTherapyAlgorithm = New TableLayoutPanel()
        Me.TableLayoutPanelTherapyAlgorithmTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage12BannerState = New TabPage()
        Me.TableLayoutPanelBannerState = New TableLayoutPanel()
        Me.TableLayoutPanelBannerStateTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage13Basal = New TabPage()
        Me.TableLayoutPanelBasal = New TableLayoutPanel()
        Me.TableLayoutPanelBasalTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPage14Markers = New TabPage()
        Me.TabPageLastSG = New TabPage()
        Me.TableLayoutPanelLastSG = New TableLayoutPanel()
        Me.TableLayoutPanelLastSgTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageLastAlarm = New TabPage()
        Me.TableLayoutPanelLastAlarm = New TableLayoutPanel()
        Me.TableLayoutPanelLastAlarmTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabControlPage2 = New TabControl()
        Me.TabPageAutoBasalDelivery = New TabPage()
        Me.TableLayoutPanelAutoBasalDelivery = New TableLayoutPanel()
        Me.TableLayoutPanelAutoBasalDeliveryTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageAutoModeStatus = New TabPage()
        Me.TableLayoutPanelAutoModeStatus = New TableLayoutPanel()
        Me.TableLayoutPanelAutoModeStatusTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageBgReadings = New TabPage()
        Me.TableLayoutPanelBgReadings = New TableLayoutPanel()
        Me.TableLayoutPanelBgReadingsTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageCalibration = New TabPage()
        Me.TableLayoutPanelCalibration = New TableLayoutPanel()
        Me.TableLayoutPanelCalibrationTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageLowGlucoseSuspended = New TabPage()
        Me.TableLayoutPanelLowGlucoseSuspended = New TableLayoutPanel()
        Me.TableLayoutPanelLowGlucoseSuspendedTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageTimeChange = New TabPage()
        Me.TableLayoutPanelTimeChange = New TableLayoutPanel()
        Me.TableLayoutPanelTimeChangeTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        Me.TabPageCountryDataPg1 = New TabPage()
        Me.TabPageCountryDataPg2 = New TabPage()
        Me.TabPageCountryDataPg3 = New TabPage()
        Me.TabPageUserProfile = New TabPage()
        Me.TabPageCurrentUser = New TabPage()
        Me.TabPageAllUsers = New TabPage()
        Me.TabPageBackToHomePage = New TabPage()
        Me.ToolTip1 = New ToolTip(components)
        Me.StatusStrip1 = New StatusStrip()
        Me.LoginStatus = New ToolStripStatusLabel()
        Me.LastUpdateTime = New ToolStripStatusLabel()
        Me.ToolStripSpacer = New ToolStripStatusLabel()
        Me.TimeZoneLabel = New ToolStripStatusLabel()
        Me.CountryDataPg2TableLayoutPanel = New TableLayoutPanel()
        Me.DgvCountryDataPg2 = New DataGridView()
        Me.DgvCountryDataPg2RecordNumber = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg2Category = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg2Key = New DataGridViewTextBoxColumn()
        Me.DgvCountryDataPg2Value = New DataGridViewTextBoxColumn()
        Me.WebView = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.CalibrationDueImage, ComponentModel.ISupportInitialize).BeginInit()
        Me.CalibrationShieldPanel.SuspendLayout()
        CType(Me.CalibrationShieldPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).BeginInit()
        Me.CursorPanel.SuspendLayout()
        CType(Me.CursorPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvCountryDataPg1, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvCareLinkUsers, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvCountryDataPg3, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvCurrentUser, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvInsulin, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvMeal, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvSGs, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvSummary, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvSessionProfile, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsulinLevelPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Me.Last24HTotalsPanel.SuspendLayout()
        CType(Me.PumpBatteryPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Me.SensorTimeLeftPanel.SuspendLayout()
        CType(Me.SensorTimeLeftPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.TransmitterBatteryPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer3, ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.TabControlPage1.SuspendLayout()
        Me.TabPage01HomePage.SuspendLayout()
        Me.TabPage02RunningIOB.SuspendLayout()
        CType(Me.SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TabPage04SummaryData.SuspendLayout()
        Me.TabPage05Insulin.SuspendLayout()
        Me.TableLayoutPanelInsulin.SuspendLayout()
        Me.TabPage06Meal.SuspendLayout()
        Me.TableLayoutPanelMeal.SuspendLayout()
        Me.TabPage07ActiveInsulin.SuspendLayout()
        Me.TableLayoutPanelActiveInsulin.SuspendLayout()
        Me.TabPage08SensorGlucose.SuspendLayout()
        Me.TableLayoutPanelSgs.SuspendLayout()
        Me.TabPage09Limits.SuspendLayout()
        Me.TableLayoutPanelLimits.SuspendLayout()
        Me.TabPage10NotificationHistory.SuspendLayout()
        Me.TableLayoutPanelNotificationHistory.SuspendLayout()
        Me.TabPage11TherapyAlgorithm.SuspendLayout()
        Me.TableLayoutPanelTherapyAlgorithm.SuspendLayout()
        Me.TabPage12BannerState.SuspendLayout()
        Me.TableLayoutPanelBannerState.SuspendLayout()
        Me.TabPage13Basal.SuspendLayout()
        Me.TableLayoutPanelBasal.SuspendLayout()
        Me.TabPageLastSG.SuspendLayout()
        Me.TableLayoutPanelLastSG.SuspendLayout()
        Me.TabPageLastAlarm.SuspendLayout()
        Me.TableLayoutPanelLastAlarm.SuspendLayout()
        Me.TabControlPage2.SuspendLayout()
        Me.TabPageAutoBasalDelivery.SuspendLayout()
        Me.TableLayoutPanelAutoBasalDelivery.SuspendLayout()
        Me.TabPageAutoModeStatus.SuspendLayout()
        Me.TableLayoutPanelAutoModeStatus.SuspendLayout()
        Me.TabPageBgReadings.SuspendLayout()
        Me.TableLayoutPanelBgReadings.SuspendLayout()
        Me.TabPageCalibration.SuspendLayout()
        Me.TableLayoutPanelCalibration.SuspendLayout()
        Me.TabPageLowGlucoseSuspended.SuspendLayout()
        Me.TableLayoutPanelLowGlucoseSuspended.SuspendLayout()
        Me.TabPageTimeChange.SuspendLayout()
        Me.TableLayoutPanelTimeChange.SuspendLayout()
        Me.TabPageCountryDataPg1.SuspendLayout()
        Me.TabPageCountryDataPg2.SuspendLayout()
        Me.CountryDataPg2TableLayoutPanel.SuspendLayout()
        CType(Me.DgvCountryDataPg2, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WebView, ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageCountryDataPg3.SuspendLayout()
        Me.TabPageUserProfile.SuspendLayout()
        Me.TabPageCurrentUser.SuspendLayout()
        Me.TabPageAllUsers.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.CountryDataPg2TableLayoutPanel.SuspendLayout()
        CType(Me.DgvCountryDataPg2, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New ToolStripItem() {Me.MenuStartHere, Me.MenuOptions, Me.MenuHelp, Me.MenuShowMiniDisplay})
        Me.MenuStrip1.Location = New Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New Size(1384, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        ' MenuStartHere
        '
        Me.MenuStartHere.DropDownItems.AddRange(New ToolStripItem() {Me.MenuStartHereLogin, Me.ToolStripSeparator1, Me.MenuStartHereLoadSavedDataFile, Me.MenuStartHereExceptionReportLoad, Me.ToolStripSeparator4, Me.MenuStartHereUseLastSavedFile, Me.MenuStartHereUseTestData, Me.ToolStripSeparator2, Me.MenuStartHereSnapshotSave, Me.ToolStripSeparator3, Me.StartHereExit})
        Me.MenuStartHere.Name = "MenuStartHere"
        Me.MenuStartHere.Size = New Size(71, 20)
        Me.MenuStartHere.Text = "Start Here"
        '
        ' MenuStartHereLogin
        '
        Me.MenuStartHereLogin.Name = "MenuStartHereLogin"
        Me.MenuStartHereLogin.Size = New Size(211, 22)
        Me.MenuStartHereLogin.Text = "Login"
        '
        ' ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New Size(208, 6)
        '
        ' MenuStartHereLoadSavedDataFile
        '
        Me.MenuStartHereLoadSavedDataFile.Name = "MenuStartHereLoadSavedDataFile"
        Me.MenuStartHereLoadSavedDataFile.Size = New Size(211, 22)
        Me.MenuStartHereLoadSavedDataFile.Text = "Load A Saved Data File"
        '
        ' MenuStartHereExceptionReportLoad
        '
        Me.MenuStartHereExceptionReportLoad.Name = "MenuStartHereExceptionReportLoad"
        Me.MenuStartHereExceptionReportLoad.Size = New Size(211, 22)
        Me.MenuStartHereExceptionReportLoad.Text = "Load An Exception Report"
        '
        ' ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New Size(208, 6)
        '
        ' MenuStartHereUseLastSavedFile
        '
        Me.MenuStartHereUseLastSavedFile.Name = "MenuStartHereUseLastSavedFile"
        Me.MenuStartHereUseLastSavedFile.Size = New Size(211, 22)
        Me.MenuStartHereUseLastSavedFile.Text = "Use Last Data File"
        '
        ' MenuStartHereUseTestData
        '
        Me.MenuStartHereUseTestData.Name = "MenuStartHereUseTestData"
        Me.MenuStartHereUseTestData.Size = New Size(211, 22)
        Me.MenuStartHereUseTestData.Text = "Use Test Data"
        '
        ' ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New Size(208, 6)
        '
        ' MenuStartHereSnapshotSave
        '
        Me.MenuStartHereSnapshotSave.Name = "MenuStartHereSnapshotSave"
        Me.MenuStartHereSnapshotSave.ShortcutKeys = Keys.Control Or Keys.S
        Me.MenuStartHereSnapshotSave.Size = New Size(211, 22)
        Me.MenuStartHereSnapshotSave.Text = "Snapshot &Save"
        '
        ' ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New Size(208, 6)
        '
        ' StartHereExit
        '
        Me.StartHereExit.Image = My.Resources.Resources.AboutBox
        Me.StartHereExit.Name = "StartHereExit"
        Me.StartHereExit.ShortcutKeys = Keys.Alt Or Keys.X
        Me.StartHereExit.Size = New Size(211, 22)
        Me.StartHereExit.Text = "E&xit"
        '
        ' MenuOptions
        '
        Me.MenuOptions.DropDownItems.AddRange(New ToolStripItem() {Me.MenuOptionsColorPicker, Me.MenuOptionsShowLegend, Me.ToolStripSeparator5, Me.MenuOptionsAutoLogin, Me.ToolStripSeparator6, Me.OptionsMenuAdvancedOptions, Me.MenuOptionsFilterRawJSONData, Me.MenuOptionsUseLocalTimeZone, Me.ToolStripSeparator7, Me.MenuOptionsEditPumpSettings})
        Me.MenuOptions.Name = "MenuOptions"
        Me.MenuOptions.Size = New Size(61, 20)
        Me.MenuOptions.Text = "Options"
        '
        ' MenuOptionsColorPicker
        '
        Me.MenuOptionsColorPicker.Name = "MenuOptionsColorPicker"
        Me.MenuOptionsColorPicker.Size = New Size(183, 22)
        Me.MenuOptionsColorPicker.Text = "Color Picker..."
        '
        ' MenuOptionsShowLegend
        '
        Me.MenuOptionsShowLegend.Checked = True
        Me.MenuOptionsShowLegend.CheckOnClick = True
        Me.MenuOptionsShowLegend.CheckState = CheckState.Checked
        Me.MenuOptionsShowLegend.Name = "MenuOptionsShowLegend"
        Me.MenuOptionsShowLegend.Size = New Size(183, 22)
        Me.MenuOptionsShowLegend.Text = "Show Legend"
        '
        ' ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New Size(180, 6)
        '
        ' MenuOptionsAutoLogin
        '
        Me.MenuOptionsAutoLogin.CheckOnClick = True
        Me.MenuOptionsAutoLogin.Name = "MenuOptionsAutoLogin"
        Me.MenuOptionsAutoLogin.Size = New Size(183, 22)
        Me.MenuOptionsAutoLogin.Text = "Auto Login"
        '
        ' ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New Size(180, 6)
        '
        ' OptionsMenuAdvancedOptions
        '
        Me.OptionsMenuAdvancedOptions.Enabled = False
        Me.OptionsMenuAdvancedOptions.Name = "OptionsMenuAdvancedOptions"
        Me.OptionsMenuAdvancedOptions.Size = New Size(183, 22)
        Me.OptionsMenuAdvancedOptions.Text = "Advanced Options"
        '
        ' MenuOptionsFilterRawJSONData
        '
        Me.MenuOptionsFilterRawJSONData.Checked = True
        Me.MenuOptionsFilterRawJSONData.CheckOnClick = True
        Me.MenuOptionsFilterRawJSONData.CheckState = CheckState.Checked
        Me.MenuOptionsFilterRawJSONData.Name = "MenuOptionsFilterRawJSONData"
        Me.MenuOptionsFilterRawJSONData.Size = New Size(183, 22)
        Me.MenuOptionsFilterRawJSONData.Text = "Filter Raw JSON Data"
        '
        ' MenuOptionsUseLocalTimeZone
        '
        Me.MenuOptionsUseLocalTimeZone.Checked = True
        Me.MenuOptionsUseLocalTimeZone.CheckOnClick = True
        Me.MenuOptionsUseLocalTimeZone.CheckState = CheckState.Indeterminate
        Me.MenuOptionsUseLocalTimeZone.Name = "MenuOptionsUseLocalTimeZone"
        Me.MenuOptionsUseLocalTimeZone.Size = New Size(183, 22)
        Me.MenuOptionsUseLocalTimeZone.Text = "Use Local TImeZone"
        '
        ' ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New Size(180, 6)
        '
        ' MenuOptionsEditPumpSettings
        '
        Me.MenuOptionsEditPumpSettings.Name = "MenuOptionsEditPumpSettings"
        Me.MenuOptionsEditPumpSettings.Size = New Size(183, 22)
        Me.MenuOptionsEditPumpSettings.Text = "Edit Pump Settings"
        '
        ' MenuHelp
        '
        Me.MenuHelp.DropDownItems.AddRange(New ToolStripItem() {Me.MenuHelpReportAnIssue, Me.MenuHelpCheckForUpdates, Me.MenuHelpAbout})
        Me.MenuHelp.Name = "MenuHelp"
        Me.MenuHelp.ShortcutKeys = Keys.Alt Or Keys.H
        Me.MenuHelp.Size = New Size(44, 20)
        Me.MenuHelp.Text = "&Help"
        '
        ' MenuHelpReportAnIssue
        '
        Me.MenuHelpReportAnIssue.Image = My.Resources.Resources.FeedbackSmile_16x
        Me.MenuHelpReportAnIssue.ImageScaling = ToolStripItemImageScaling.None
        Me.MenuHelpReportAnIssue.Name = "MenuHelpReportAnIssue"
        Me.MenuHelpReportAnIssue.Size = New Size(177, 22)
        Me.MenuHelpReportAnIssue.Text = "Report A Problem..."
        '
        ' MenuHelpCheckForUpdates
        '
        Me.MenuHelpCheckForUpdates.Name = "MenuHelpCheckForUpdates"
        Me.MenuHelpCheckForUpdates.Size = New Size(177, 22)
        Me.MenuHelpCheckForUpdates.Text = "Check For Updates"
        '
        ' MenuHelpAbout
        '
        Me.MenuHelpAbout.Image = My.Resources.Resources.AboutBox
        Me.MenuHelpAbout.Name = "MenuHelpAbout"
        Me.MenuHelpAbout.Size = New Size(177, 22)
        Me.MenuHelpAbout.Text = "&About..."
        '
        ' MenuShowMiniDisplay
        '
        Me.MenuShowMiniDisplay.ForeColor = Color.Red
        Me.MenuShowMiniDisplay.Image = My.Resources.Resources.ExitFullScreen
        Me.MenuShowMiniDisplay.Name = "MenuShowMiniDisplay"
        Me.MenuShowMiniDisplay.Padding = New Padding(10, 0, 10, 0)
        Me.MenuShowMiniDisplay.ShortcutKeyDisplayString = "Alt+W"
        Me.MenuShowMiniDisplay.ShortcutKeys = Keys.Control Or Keys.W
        Me.MenuShowMiniDisplay.Size = New Size(154, 20)
        Me.MenuShowMiniDisplay.Text = "Show &Widget Alt+W"
        Me.MenuShowMiniDisplay.ToolTipText = "Minimize and show Widget"
        Me.MenuShowMiniDisplay.Visible = False
        '
        ' AboveHighLimitMessageLabel
        '
        Me.AboveHighLimitMessageLabel.Anchor = AnchorStyles.Top
        Me.AboveHighLimitMessageLabel.BackColor = Color.Transparent
        Me.AboveHighLimitMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.AboveHighLimitMessageLabel.ForeColor = Color.Yellow
        Me.AboveHighLimitMessageLabel.Location = New Point(30, 248)
        Me.AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        Me.AboveHighLimitMessageLabel.Size = New Size(170, 21)
        Me.AboveHighLimitMessageLabel.TabIndex = 28
        Me.AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        Me.AboveHighLimitMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' AboveHighLimitValueLabel
        '
        Me.AboveHighLimitValueLabel.Anchor = AnchorStyles.Top
        Me.AboveHighLimitValueLabel.BackColor = Color.Black
        Me.AboveHighLimitValueLabel.Font = New Font("Microsoft Sans Serif", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.AboveHighLimitValueLabel.ForeColor = Color.White
        Me.AboveHighLimitValueLabel.Location = New Point(55, 215)
        Me.AboveHighLimitValueLabel.Name = "AboveHighLimitValueLabel"
        Me.AboveHighLimitValueLabel.Size = New Size(120, 33)
        Me.AboveHighLimitValueLabel.TabIndex = 22
        Me.AboveHighLimitValueLabel.Text = "8 %"
        Me.AboveHighLimitValueLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' ActiveInsulinValue
        '
        Me.ActiveInsulinValue.BackColor = Color.Transparent
        Me.ActiveInsulinValue.BorderStyle = BorderStyle.FixedSingle
        Me.ActiveInsulinValue.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.ActiveInsulinValue.ForeColor = Color.White
        Me.ActiveInsulinValue.Location = New Point(995, 53)
        Me.ActiveInsulinValue.Name = "ActiveInsulinValue"
        Me.ActiveInsulinValue.Size = New Size(128, 48)
        Me.ActiveInsulinValue.TabIndex = 0
        Me.ActiveInsulinValue.Text = "Active Insulin 0.000 U"
        Me.ActiveInsulinValue.TextAlign = ContentAlignment.TopCenter
        '
        ' FullNameLabel
        '
        Me.FullNameLabel.BackColor = Color.Transparent
        Me.FullNameLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.FullNameLabel.ForeColor = Color.White
        Me.FullNameLabel.Location = New Point(1140, 3)
        Me.FullNameLabel.Margin = New Padding(0)
        Me.FullNameLabel.Name = "FullNameLabel"
        Me.FullNameLabel.Size = New Size(230, 21)
        Me.FullNameLabel.TabIndex = 8
        Me.FullNameLabel.Text = "User Name"
        Me.FullNameLabel.TextAlign = ContentAlignment.MiddleLeft
        '
        ' AverageSGMessageLabel
        '
        Me.AverageSGMessageLabel.Anchor = AnchorStyles.Top
        Me.AverageSGMessageLabel.BackColor = Color.Transparent
        Me.AverageSGMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.AverageSGMessageLabel.ForeColor = Color.White
        Me.AverageSGMessageLabel.Location = New Point(3, 433)
        Me.AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        Me.AverageSGMessageLabel.Size = New Size(224, 21)
        Me.AverageSGMessageLabel.TabIndex = 0
        Me.AverageSGMessageLabel.Text = "Average SG in XX/XX"
        Me.AverageSGMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' AverageSGValueLabel
        '
        Me.AverageSGValueLabel.Anchor = AnchorStyles.Top
        Me.AverageSGValueLabel.BackColor = Color.Black
        Me.AverageSGValueLabel.Font = New Font("Microsoft Sans Serif", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.AverageSGValueLabel.ForeColor = Color.White
        Me.AverageSGValueLabel.Location = New Point(55, 398)
        Me.AverageSGValueLabel.Margin = New Padding(0)
        Me.AverageSGValueLabel.Name = "AverageSGValueLabel"
        Me.AverageSGValueLabel.Size = New Size(120, 33)
        Me.AverageSGValueLabel.TabIndex = 1
        Me.AverageSGValueLabel.Text = "100 %"
        Me.AverageSGValueLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' BannerStateButton
        '
        Me.BannerStateButton.AutoSize = True
        Me.BannerStateButton.Location = New Point(6, 6)
        Me.BannerStateButton.Name = "BannerStateButton"
        Me.BannerStateButton.Size = New Size(142, 25)
        Me.BannerStateButton.TabIndex = 0
        Me.BannerStateButton.Text = "Return To 'Summary Data' Tab"
        '
        ' BannerStateLabel
        '
        Me.BannerStateLabel.AutoSize = True
        Me.BannerStateLabel.Dock = DockStyle.Fill
        Me.BannerStateLabel.Location = New Point(157, 6)
        Me.BannerStateLabel.Margin = New Padding(3)
        Me.BannerStateLabel.Name = "BannerStateLabel"
        Me.BannerStateLabel.Size = New Size(1201, 25)
        Me.BannerStateLabel.TabIndex = 0
        Me.BannerStateLabel.Text = "Banner State"
        Me.BannerStateLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' BasalButton
        '
        Me.BasalButton.AutoSize = True
        Me.BasalButton.Location = New Point(6, 6)
        Me.BasalButton.Name = "BasalButton"
        Me.BasalButton.Size = New Size(142, 25)
        Me.BasalButton.TabIndex = 0
        Me.BasalButton.Text = "Return To 'Summary Data' Tab"
        '
        ' BasalLabel
        '
        Me.BasalLabel.AutoSize = True
        Me.BasalLabel.Dock = DockStyle.Fill
        Me.BasalLabel.Location = New Point(157, 6)
        Me.BasalLabel.Margin = New Padding(3)
        Me.BasalLabel.Name = "BasalLabel"
        Me.BasalLabel.Size = New Size(1201, 25)
        Me.BasalLabel.TabIndex = 1
        Me.BasalLabel.Text = "Basal"
        Me.BasalLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' BelowLowLimitMessageLabel
        '
        Me.BelowLowLimitMessageLabel.Anchor = AnchorStyles.Top
        Me.BelowLowLimitMessageLabel.BackColor = Color.Transparent
        Me.BelowLowLimitMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.BelowLowLimitMessageLabel.ForeColor = Color.Red
        Me.BelowLowLimitMessageLabel.Location = New Point(30, 370)
        Me.BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        Me.BelowLowLimitMessageLabel.Size = New Size(170, 21)
        Me.BelowLowLimitMessageLabel.TabIndex = 32
        Me.BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        Me.BelowLowLimitMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' BelowLowLimitValueLabel
        '
        Me.BelowLowLimitValueLabel.Anchor = AnchorStyles.Top
        Me.BelowLowLimitValueLabel.BackColor = Color.Black
        Me.BelowLowLimitValueLabel.Font = New Font("Microsoft Sans Serif", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.BelowLowLimitValueLabel.ForeColor = Color.White
        Me.BelowLowLimitValueLabel.Location = New Point(55, 337)
        Me.BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        Me.BelowLowLimitValueLabel.Size = New Size(120, 33)
        Me.BelowLowLimitValueLabel.TabIndex = 26
        Me.BelowLowLimitValueLabel.Text = "2 %"
        Me.BelowLowLimitValueLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' CalibrationDueImage
        '
        Me.CalibrationDueImage.BackColor = Color.Transparent
        Me.CalibrationDueImage.Image = My.Resources.Resources.CalibrationUnavailable
        Me.CalibrationDueImage.Location = New Point(474, 0)
        Me.CalibrationDueImage.Name = "CalibrationDueImage"
        Me.CalibrationDueImage.Size = New Size(58, 58)
        Me.CalibrationDueImage.SizeMode = PictureBoxSizeMode.CenterImage
        Me.CalibrationDueImage.TabIndex = 5
        Me.CalibrationDueImage.TabStop = False
        '
        ' CalibrationShieldPanel
        '
        Me.CalibrationShieldPanel.Controls.Add(Me.TempTargetLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.ShieldUnitsLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.LastSGTimeLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.CurrentBGLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.SensorMessage)
        Me.CalibrationShieldPanel.Controls.Add(Me.CalibrationShieldPictureBox)
        Me.CalibrationShieldPanel.Dock = DockStyle.Left
        Me.CalibrationShieldPanel.Location = New Point(0, 0)
        Me.CalibrationShieldPanel.Margin = New Padding(0)
        Me.CalibrationShieldPanel.Name = "CalibrationShieldPanel"
        Me.CalibrationShieldPanel.Size = New Size(116, 130)
        Me.CalibrationShieldPanel.TabIndex = 64
        '
        ' TempTargetLabel
        '
        Me.TempTargetLabel.BackColor = Color.Lime
        Me.TempTargetLabel.Dock = DockStyle.Top
        Me.TempTargetLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point)
        Me.TempTargetLabel.ForeColor = Color.Black
        Me.TempTargetLabel.Location = New Point(0, 0)
        Me.TempTargetLabel.Name = "TempTargetLabel"
        Me.TempTargetLabel.Size = New Size(116, 21)
        Me.TempTargetLabel.TabIndex = 56
        Me.TempTargetLabel.Text = "Target 150 2:00 Hr"
        Me.TempTargetLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' ShieldUnitsLabel
        '
        Me.ShieldUnitsLabel.AutoSize = True
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.ShieldUnitsLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point)
        Me.ShieldUnitsLabel.ForeColor = Color.White
        Me.ShieldUnitsLabel.Location = New Point(38, 76)
        Me.ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        Me.ShieldUnitsLabel.Size = New Size(40, 13)
        Me.ShieldUnitsLabel.TabIndex = 8
        Me.ShieldUnitsLabel.Text = "XX/XX"
        '
        ' LastSGTimeLabel
        '
        Me.LastSGTimeLabel.BackColor = Color.Transparent
        Me.LastSGTimeLabel.Dock = DockStyle.Bottom
        Me.LastSGTimeLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.LastSGTimeLabel.ForeColor = Color.White
        Me.LastSGTimeLabel.Location = New Point(0, 109)
        Me.LastSGTimeLabel.Name = "LastSGTimeLabel"
        Me.LastSGTimeLabel.Size = New Size(116, 21)
        Me.LastSGTimeLabel.TabIndex = 55
        Me.LastSGTimeLabel.Text = "Time"
        Me.LastSGTimeLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' CurrentBGLabel
        '
        Me.CurrentBGLabel.BackColor = Color.Transparent
        Me.CurrentBGLabel.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.CurrentBGLabel.ForeColor = Color.White
        Me.CurrentBGLabel.Location = New Point(22, 35)
        Me.CurrentBGLabel.Name = "CurrentBGLabel"
        Me.CurrentBGLabel.Size = New Size(72, 32)
        Me.CurrentBGLabel.TabIndex = 9
        Me.CurrentBGLabel.Text = "---"
        Me.CurrentBGLabel.TextAlign = ContentAlignment.MiddleCenter
        Me.CurrentBGLabel.Visible = False
        '
        ' SensorMessage
        '
        Me.SensorMessage.BackColor = Color.Transparent
        Me.SensorMessage.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point)
        Me.SensorMessage.ForeColor = Color.White
        Me.SensorMessage.Location = New Point(2, 15)
        Me.SensorMessage.Name = "SensorMessage"
        Me.SensorMessage.Size = New Size(112, 57)
        Me.SensorMessage.TabIndex = 1
        Me.SensorMessage.Text = "Calibration Required"
        Me.SensorMessage.TextAlign = ContentAlignment.MiddleCenter
        '
        ' CalibrationShieldPictureBox
        '
        Me.CalibrationShieldPictureBox.Image = My.Resources.Resources.Shield
        Me.CalibrationShieldPictureBox.Location = New Point(0, 0)
        Me.CalibrationShieldPictureBox.Margin = New Padding(5)
        Me.CalibrationShieldPictureBox.Name = "CalibrationShieldPictureBox"
        Me.CalibrationShieldPictureBox.Size = New Size(116, 116)
        Me.CalibrationShieldPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        Me.CalibrationShieldPictureBox.TabIndex = 5
        Me.CalibrationShieldPictureBox.TabStop = False
        '
        ' CareLinkUserDataRecordBindingSource
        '
        Me.CareLinkUserDataRecordBindingSource.DataSource = GetType(CareLinkUserDataRecord)
        '
        ' CursorMessage1Label
        '
        Me.CursorMessage1Label.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.CursorMessage1Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.CursorMessage1Label.ForeColor = Color.White
        Me.CursorMessage1Label.Location = New Point(0, 63)
        Me.CursorMessage1Label.Name = "CursorMessage1Label"
        Me.CursorMessage1Label.Size = New Size(178, 21)
        Me.CursorMessage1Label.TabIndex = 39
        Me.CursorMessage1Label.Text = "Blood Glucose"
        Me.CursorMessage1Label.TextAlign = ContentAlignment.MiddleCenter
        '
        ' CursorMessage2Label
        '
        Me.CursorMessage2Label.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.CursorMessage2Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.CursorMessage2Label.ForeColor = Color.White
        Me.CursorMessage2Label.Location = New Point(0, 84)
        Me.CursorMessage2Label.Name = "CursorMessage2Label"
        Me.CursorMessage2Label.Size = New Size(178, 21)
        Me.CursorMessage2Label.TabIndex = 40
        Me.CursorMessage2Label.Text = "Calibration Accepted"
        Me.CursorMessage2Label.TextAlign = ContentAlignment.MiddleCenter
        '
        ' CursorMessage3Label
        '
        Me.CursorMessage3Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.CursorMessage3Label.ForeColor = Color.White
        Me.CursorMessage3Label.Location = New Point(0, 105)
        Me.CursorMessage3Label.Name = "CursorMessage3Label"
        Me.CursorMessage3Label.Size = New Size(178, 21)
        Me.CursorMessage3Label.TabIndex = 41
        Me.CursorMessage3Label.Text = "156 ml/dl"
        Me.CursorMessage3Label.TextAlign = ContentAlignment.MiddleCenter
        '
        ' CursorPanel
        '
        Me.CursorPanel.Controls.Add(Me.CursorPictureBox)
        Me.CursorPanel.Controls.Add(Me.CursorMessage1Label)
        Me.CursorPanel.Controls.Add(Me.CursorMessage2Label)
        Me.CursorPanel.Controls.Add(Me.CursorMessage3Label)
        Me.CursorPanel.Location = New Point(284, 0)
        Me.CursorPanel.Margin = New Padding(0)
        Me.CursorPanel.Name = "CursorPanel"
        Me.CursorPanel.Size = New Size(178, 135)
        Me.CursorPanel.TabIndex = 63
        '
        ' CursorPictureBox
        '
        Me.CursorPictureBox.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.CursorPictureBox.Image = CType(resources.GetObject("CursorPictureBox.Image"), Image)
        Me.CursorPictureBox.InitialImage = Nothing
        Me.CursorPictureBox.Location = New Point(68, 0)
        Me.CursorPictureBox.Name = "CursorPictureBox"
        Me.CursorPictureBox.Size = New Size(42, 56)
        Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.CenterImage
        Me.CursorPictureBox.TabIndex = 42
        Me.CursorPictureBox.TabStop = False
        '
        ' CursorTimer
        '
        Me.CursorTimer.Interval = 60000
        '
        ' DgvAutoBasalDelivery
        '
        Me.DgvAutoBasalDelivery.Dock = DockStyle.Fill
        Me.DgvAutoBasalDelivery.Location = New Point(6, 52)
        Me.DgvAutoBasalDelivery.Name = "DgvAutoBasalDelivery"
        Me.DgvAutoBasalDelivery.ReadOnly = True
        Me.DgvAutoBasalDelivery.RowTemplate.Height = 25
        Me.DgvAutoBasalDelivery.Size = New Size(1358, 573)
        Me.DgvAutoBasalDelivery.TabIndex = 0
        '
        ' DgvCountryDataPg1
        '
        Me.DgvCountryDataPg1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvCountryDataPg1.Columns.AddRange(New DataGridViewColumn() {Me.DgvCountryDataPg1RecordNumber, Me.DgvCountryDataPg1Category, Me.DgvCountryDataPg1Key, Me.DgvCountryDataPg1Value})
        Me.DgvCountryDataPg1.Dock = DockStyle.Fill
        Me.DgvCountryDataPg1.Location = New Point(3, 3)
        Me.DgvCountryDataPg1.Name = "DgvCountryDataPg1"
        Me.DgvCountryDataPg1.ReadOnly = True
        Me.DgvCountryDataPg1.RowTemplate.Height = 25
        Me.DgvCountryDataPg1.Size = New Size(1370, 631)
        Me.DgvCountryDataPg1.TabIndex = 1
        '
        ' DgvCountryDataPg1RecordNumber
        '
        Me.DgvCountryDataPg1RecordNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DgvCountryDataPg1RecordNumber.HeaderText = "Record Number"
        Me.DgvCountryDataPg1RecordNumber.MinimumWidth = 60
        Me.DgvCountryDataPg1RecordNumber.Name = "DgvCountryDataPg1RecordNumber"
        Me.DgvCountryDataPg1RecordNumber.ReadOnly = True
        Me.DgvCountryDataPg1RecordNumber.Width = 60
        '
        ' DgvCountryDataPg1Category
        '
        Me.DgvCountryDataPg1Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg1Category.HeaderText = "Category"
        Me.DgvCountryDataPg1Category.Name = "DgvCountryDataPg1Category"
        Me.DgvCountryDataPg1Category.ReadOnly = True
        Me.DgvCountryDataPg1Category.Width = 80
        '
        ' DgvCountryDataPg1Key
        '
        Me.DgvCountryDataPg1Key.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg1Key.HeaderText = "Key"
        Me.DgvCountryDataPg1Key.Name = "DgvCountryDataPg1Key"
        Me.DgvCountryDataPg1Key.ReadOnly = True
        Me.DgvCountryDataPg1Key.Width = 51
        '
        ' DgvCountryDataPg1Value
        '
        Me.DgvCountryDataPg1Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Me.DgvCountryDataPg1Value.HeaderText = "Value"
        Me.DgvCountryDataPg1Value.Name = "DgvCountryDataPg1Value"
        Me.DgvCountryDataPg1Value.ReadOnly = True
        '
        ' DgvCareLinkUsers
        '
        Me.DgvCareLinkUsers.AllowUserToAddRows = False
        Me.DgvCareLinkUsers.AllowUserToResizeColumns = False
        Me.DgvCareLinkUsers.AllowUserToResizeRows = False
        Me.DgvCareLinkUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Me.DgvCareLinkUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        Me.DgvCareLinkUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvCareLinkUsers.Dock = DockStyle.Fill
        Me.DgvCareLinkUsers.EditMode = DataGridViewEditMode.EditOnEnter
        Me.DgvCareLinkUsers.Location = New Point(3, 3)
        Me.DgvCareLinkUsers.Name = "DgvCareLinkUsers"
        Me.DgvCareLinkUsers.RowTemplate.Height = 25
        Me.DgvCareLinkUsers.SelectionMode = DataGridViewSelectionMode.CellSelect
        Me.DgvCareLinkUsers.Size = New Size(1370, 631)
        Me.DgvCareLinkUsers.TabIndex = 0
        '
        ' DgvCountryDataPg3
        '
        Me.DgvCountryDataPg3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvCountryDataPg3.Columns.AddRange(New DataGridViewColumn() {Me.DgvCountryDataPg3RecordNumber, Me.DgvCountryDataPg3Category, Me.DgvCountryDataPg3Key, Me.DgvCountryDataPg3Value, Me.DgvCountryDataPg3OnlyFor, Me.DgvCountryDataPg3NotFor})
        Me.DgvCountryDataPg3.Dock = DockStyle.Fill
        Me.DgvCountryDataPg3.Location = New Point(3, 3)
        Me.DgvCountryDataPg3.Name = "DgvCountryDataPg3"
        Me.DgvCountryDataPg3.ReadOnly = True
        Me.DgvCountryDataPg3.RowTemplate.Height = 25
        Me.DgvCountryDataPg3.Size = New Size(1370, 631)
        Me.DgvCountryDataPg3.TabIndex = 1
        '
        ' DgvCountryDataPg3RecordNumber
        '
        Me.DgvCountryDataPg3RecordNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DgvCountryDataPg3RecordNumber.HeaderText = "Record Number"
        Me.DgvCountryDataPg3RecordNumber.MinimumWidth = 60
        Me.DgvCountryDataPg3RecordNumber.Name = "DgvCountryDataPg3RecordNumber"
        Me.DgvCountryDataPg3RecordNumber.ReadOnly = True
        Me.DgvCountryDataPg3RecordNumber.Width = 60
        '
        ' DgvCountryDataPg3Category
        '
        Me.DgvCountryDataPg3Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg3Category.HeaderText = "Category"
        Me.DgvCountryDataPg3Category.Name = "DgvCountryDataPg3Category"
        Me.DgvCountryDataPg3Category.ReadOnly = True
        Me.DgvCountryDataPg3Category.Width = 80
        '
        ' DgvCountryDataPg3Key
        '
        Me.DgvCountryDataPg3Key.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg3Key.HeaderText = "Key"
        Me.DgvCountryDataPg3Key.Name = "DgvCountryDataPg3Key"
        Me.DgvCountryDataPg3Key.ReadOnly = True
        Me.DgvCountryDataPg3Key.Width = 51
        '
        ' DgvCountryDataPg3Value
        '
        Me.DgvCountryDataPg3Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg3Value.HeaderText = "Value"
        Me.DgvCountryDataPg3Value.Name = "DgvCountryDataPg3Value"
        Me.DgvCountryDataPg3Value.ReadOnly = True
        Me.DgvCountryDataPg3Value.Width = 60
        '
        ' DgvCountryDataPg3OnlyFor
        '
        Me.DgvCountryDataPg3OnlyFor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Me.DgvCountryDataPg3OnlyFor.HeaderText = "Report Only For"
        Me.DgvCountryDataPg3OnlyFor.Name = "DgvCountryDataPg3OnlyFor"
        Me.DgvCountryDataPg3OnlyFor.ReadOnly = True
        '
        ' DgvCountryDataPg3NotFor
        '
        Me.DgvCountryDataPg3NotFor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Me.DgvCountryDataPg3NotFor.HeaderText = "Report Not For"
        Me.DgvCountryDataPg3NotFor.Name = "DgvCountryDataPg3NotFor"
        Me.DgvCountryDataPg3NotFor.ReadOnly = True
        '
        ' DgvCurrentUser
        '
        Me.DgvCurrentUser.AllowUserToAddRows = False
        Me.DgvCurrentUser.AllowUserToDeleteRows = False
        Me.DgvCurrentUser.AllowUserToResizeColumns = False
        Me.DgvCurrentUser.AllowUserToResizeRows = False
        Me.DgvCurrentUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Me.DgvCurrentUser.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        Me.DgvCurrentUser.Dock = DockStyle.Fill
        Me.DgvCurrentUser.Location = New Point(0, 0)
        Me.DgvCurrentUser.Name = "DgvCurrentUser"
        Me.DgvCurrentUser.ReadOnly = True
        Me.DgvCurrentUser.RowHeadersVisible = False
        Me.DgvCurrentUser.RowTemplate.Height = 25
        Me.DgvCurrentUser.Size = New Size(1376, 637)
        Me.DgvCurrentUser.TabIndex = 0
        '
        ' DgvInsulin
        '
        Me.DgvInsulin.Dock = DockStyle.Fill
        Me.DgvInsulin.Location = New Point(6, 52)
        Me.DgvInsulin.Name = "DgvInsulin"
        Me.DgvInsulin.ReadOnly = True
        Me.DgvInsulin.RowTemplate.Height = 25
        Me.DgvInsulin.SelectionMode = DataGridViewSelectionMode.CellSelect
        Me.DgvInsulin.Size = New Size(1358, 573)
        Me.DgvInsulin.TabIndex = 0
        '
        ' DgvMeal
        '
        Me.DgvMeal.Dock = DockStyle.Fill
        Me.DgvMeal.Location = New Point(6, 52)
        Me.DgvMeal.Name = "DgvMeal"
        Me.DgvMeal.Size = New Size(1358, 573)
        Me.DgvMeal.TabIndex = 2
        '
        ' DgvSGs
        '
        Me.DgvSGs.Location = New Point(3, 46)
        Me.DgvSGs.Name = "DgvSGs"
        Me.DgvSGs.RowTemplate.Height = 25
        Me.DgvSGs.Size = New Size(1358, 582)
        Me.DgvSGs.TabIndex = 1
        '
        ' DgvSummary
        '
        Me.DgvSummary.Dock = DockStyle.Fill
        Me.DgvSummary.Location = New Point(3, 3)
        Me.DgvSummary.Name = "DgvSummary"
        Me.DgvSummary.ReadOnly = True
        Me.DgvSummary.RowTemplate.Height = 25
        Me.DgvSummary.SelectionMode = DataGridViewSelectionMode.CellSelect
        Me.DgvSummary.Size = New Size(1370, 631)
        Me.DgvSummary.TabIndex = 0
        '
        ' DgvSessionProfile
        '
        Me.DgvSessionProfile.Dock = DockStyle.Fill
        Me.DgvSessionProfile.Location = New Point(3, 3)
        Me.DgvSessionProfile.Name = "DgvSessionProfile"
        Me.DgvSessionProfile.ReadOnly = True
        Me.DgvSessionProfile.RowHeadersVisible = False
        Me.DgvSessionProfile.RowTemplate.Height = 25
        Me.DgvSessionProfile.Size = New Size(1370, 631)
        Me.DgvSessionProfile.TabIndex = 0
        '
        ' ImageList1
        '
        Me.ImageList1.ColorDepth = ColorDepth.Depth32Bit
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), ImageListStreamer)
        Me.ImageList1.TransparentColor = Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "ReservoirRemains0.png")
        Me.ImageList1.Images.SetKeyName(1, "ReservoirRemains1+.png")
        Me.ImageList1.Images.SetKeyName(2, "ReservoirRemains15+.png")
        Me.ImageList1.Images.SetKeyName(3, "ReservoirRemains29+.png")
        Me.ImageList1.Images.SetKeyName(4, "ReservoirRemains43+.png")
        Me.ImageList1.Images.SetKeyName(5, "ReservoirRemains57+.png")
        Me.ImageList1.Images.SetKeyName(6, "ReservoirRemains71+.png")
        Me.ImageList1.Images.SetKeyName(7, "ReservoirRemains85+.png")
        Me.ImageList1.Images.SetKeyName(8, "ReservoirRemainsUnknown.png")
        '
        ' InRangeMessageLabel
        '
        Me.InRangeMessageLabel.Anchor = AnchorStyles.Top
        Me.InRangeMessageLabel.AutoSize = True
        Me.InRangeMessageLabel.BackColor = Color.Transparent
        Me.InRangeMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.InRangeMessageLabel.ForeColor = Color.Lime
        Me.InRangeMessageLabel.Location = New Point(81, 309)
        Me.InRangeMessageLabel.Name = "InRangeMessageLabel"
        Me.InRangeMessageLabel.Size = New Size(73, 21)
        Me.InRangeMessageLabel.TabIndex = 30
        Me.InRangeMessageLabel.Text = "In range"
        Me.InRangeMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' InsulinLevelPictureBox
        '
        Me.InsulinLevelPictureBox.Image = CType(resources.GetObject("InsulinLevelPictureBox.Image"), Image)
        Me.InsulinLevelPictureBox.InitialImage = Nothing
        Me.InsulinLevelPictureBox.Location = New Point(221, 0)
        Me.InsulinLevelPictureBox.Name = "InsulinLevelPictureBox"
        Me.InsulinLevelPictureBox.Padding = New Padding(10)
        Me.InsulinLevelPictureBox.Size = New Size(51, 67)
        Me.InsulinLevelPictureBox.SizeMode = PictureBoxSizeMode.Zoom
        Me.InsulinLevelPictureBox.TabIndex = 12
        Me.InsulinLevelPictureBox.TabStop = False
        '
        ' LabelSgTrend
        '
        Me.LabelSgTrend.BackColor = Color.Black
        Me.LabelSgTrend.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.LabelSgTrend.ForeColor = Color.White
        Me.LabelSgTrend.Location = New Point(461, 64)
        Me.LabelSgTrend.Name = "LabelSgTrend"
        Me.LabelSgTrend.Size = New Size(84, 21)
        Me.LabelSgTrend.TabIndex = 61
        Me.LabelSgTrend.Text = "SG Trend"
        Me.LabelSgTrend.TextAlign = ContentAlignment.MiddleCenter
        '
        ' LabelTimeChange
        '
        Me.LabelTimeChange.AutoSize = True
        Me.LabelTimeChange.Dock = DockStyle.Fill
        Me.LabelTimeChange.Location = New Point(6, 6)
        Me.LabelTimeChange.Margin = New Padding(3)
        Me.LabelTimeChange.Name = "LabelTimeChange"
        Me.LabelTimeChange.Size = New Size(174, 15)
        Me.LabelTimeChange.TabIndex = 0
        Me.LabelTimeChange.Text = "Time Change"
        Me.LabelTimeChange.TextAlign = ContentAlignment.MiddleCenter
        '
        ' LabelTrendArrows
        '
        Me.LabelTrendArrows.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
        Me.LabelTrendArrows.ForeColor = Color.White
        Me.LabelTrendArrows.Location = New Point(461, 106)
        Me.LabelTrendArrows.Name = "LabelTrendArrows"
        Me.LabelTrendArrows.Size = New Size(84, 21)
        Me.LabelTrendArrows.TabIndex = 62
        Me.LabelTrendArrows.Text = "↑↔↓"
        Me.LabelTrendArrows.TextAlign = ContentAlignment.MiddleCenter
        '
        ' LabelTrendValue
        '
        Me.LabelTrendValue.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.LabelTrendValue.ForeColor = Color.White
        Me.LabelTrendValue.Location = New Point(461, 89)
        Me.LabelTrendValue.Name = "LabelTrendValue"
        Me.LabelTrendValue.Size = New Size(84, 21)
        Me.LabelTrendValue.TabIndex = 68
        Me.LabelTrendValue.Text = "+ 5"
        Me.LabelTrendValue.TextAlign = ContentAlignment.MiddleCenter
        '
        ' Last24AutoCorrectionLabel
        '
        Me.Last24AutoCorrectionLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.Last24AutoCorrectionLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24AutoCorrectionLabel.ForeColor = Color.White
        Me.Last24AutoCorrectionLabel.Location = New Point(0, 84)
        Me.Last24AutoCorrectionLabel.Name = "Last24AutoCorrectionLabel"
        Me.Last24AutoCorrectionLabel.Size = New Size(235, 21)
        Me.Last24AutoCorrectionLabel.TabIndex = 64
        Me.Last24AutoCorrectionLabel.Text = "Auto Correction 20 U | 20%"
        Me.Last24AutoCorrectionLabel.TextAlign = ContentAlignment.MiddleLeft
        '
        ' Last24CarbsValueLabel
        '
        Me.Last24CarbsValueLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.Last24CarbsValueLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24CarbsValueLabel.ForeColor = Color.White
        Me.Last24CarbsValueLabel.Location = New Point(0, 105)
        Me.Last24CarbsValueLabel.Name = "Last24CarbsValueLabel"
        Me.Last24CarbsValueLabel.Size = New Size(218, 21)
        Me.Last24CarbsValueLabel.TabIndex = 66
        Me.Last24CarbsValueLabel.Text = "Carbs 100 Grams"
        Me.Last24CarbsValueLabel.TextAlign = ContentAlignment.MiddleLeft
        '
        ' Last24DailyDoseLabel
        '
        Me.Last24DailyDoseLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.Last24DailyDoseLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24DailyDoseLabel.ForeColor = Color.White
        Me.Last24DailyDoseLabel.Location = New Point(0, 21)
        Me.Last24DailyDoseLabel.Name = "Last24DailyDoseLabel"
        Me.Last24DailyDoseLabel.Size = New Size(235, 21)
        Me.Last24DailyDoseLabel.TabIndex = 61
        Me.Last24DailyDoseLabel.Text = "Dose 100 U"
        Me.Last24DailyDoseLabel.TextAlign = ContentAlignment.MiddleLeft
        '
        ' Last24HourBasalLabel
        '
        Me.Last24HourBasalLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.Last24HourBasalLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24HourBasalLabel.ForeColor = Color.White
        Me.Last24HourBasalLabel.Location = New Point(0, 42)
        Me.Last24HourBasalLabel.Name = "Last24HourBasalLabel"
        Me.Last24HourBasalLabel.Size = New Size(235, 21)
        Me.Last24HourBasalLabel.TabIndex = 62
        Me.Last24HourBasalLabel.Text = "Basal 50 U | 50%"
        Me.Last24HourBasalLabel.TextAlign = ContentAlignment.MiddleLeft
        '
        ' Last24HoursLabel
        '
        Me.Last24HoursLabel.Anchor = AnchorStyles.Top
        Me.Last24HoursLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24HoursLabel.ForeColor = Color.White
        Me.Last24HoursLabel.Location = New Point(30, 26)
        Me.Last24HoursLabel.Name = "Last24HoursLabel"
        Me.Last24HoursLabel.Size = New Size(170, 21)
        Me.Last24HoursLabel.TabIndex = 34
        Me.Last24HoursLabel.Text = "Last 24 hours"
        Me.Last24HoursLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' Last24HTotalsPanel
        '
        Me.Last24HTotalsPanel.BorderStyle = BorderStyle.FixedSingle
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24CarbsValueLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24TotalsLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24AutoCorrectionLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24ManualBolusLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24HourBasalLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24DailyDoseLabel)
        Me.Last24HTotalsPanel.Location = New Point(740, 0)
        Me.Last24HTotalsPanel.Name = "Last24HTotalsPanel"
        Me.Last24HTotalsPanel.Size = New Size(237, 129)
        Me.Last24HTotalsPanel.TabIndex = 66
        '
        ' Last24TotalsLabel
        '
        Me.Last24TotalsLabel.BorderStyle = BorderStyle.FixedSingle
        Me.Last24TotalsLabel.Dock = DockStyle.Top
        Me.Last24TotalsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24TotalsLabel.ForeColor = Color.White
        Me.Last24TotalsLabel.Location = New Point(0, 0)
        Me.Last24TotalsLabel.Name = "Last24TotalsLabel"
        Me.Last24TotalsLabel.Size = New Size(235, 23)
        Me.Last24TotalsLabel.TabIndex = 65
        Me.Last24TotalsLabel.Text = "Last 24 Hr Totals"
        Me.Last24TotalsLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' Last24ManualBolusLabel
        '
        Me.Last24ManualBolusLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Me.Last24ManualBolusLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.Last24ManualBolusLabel.ForeColor = Color.White
        Me.Last24ManualBolusLabel.Location = New Point(0, 63)
        Me.Last24ManualBolusLabel.Name = "Last24ManualBolusLabel"
        Me.Last24ManualBolusLabel.Size = New Size(235, 21)
        Me.Last24ManualBolusLabel.TabIndex = 63
        Me.Last24ManualBolusLabel.Text = "Manual Bolus 30 U | 30%"
        Me.Last24ManualBolusLabel.TextAlign = ContentAlignment.MiddleLeft
        '
        ' MaxBasalPerHourLabel
        '
        Me.MaxBasalPerHourLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.MaxBasalPerHourLabel.ForeColor = Color.White
        Me.MaxBasalPerHourLabel.Location = New Point(1140, 106)
        Me.MaxBasalPerHourLabel.Name = "MaxBasalPerHourLabel"
        Me.MaxBasalPerHourLabel.Size = New Size(230, 21)
        Me.MaxBasalPerHourLabel.TabIndex = 67
        Me.MaxBasalPerHourLabel.Text = "Max Basal/Hr ~ 2.0 U"
        '
        ' ModelLabel
        '
        Me.ModelLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.ModelLabel.ForeColor = Color.White
        Me.ModelLabel.Location = New Point(1140, 28)
        Me.ModelLabel.Name = "ModelLabel"
        Me.ModelLabel.Size = New Size(230, 21)
        Me.ModelLabel.TabIndex = 57
        Me.ModelLabel.Text = "Model"
        '
        ' PumpNameLabel
        '
        Me.PumpNameLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.PumpNameLabel.ForeColor = Color.White
        Me.PumpNameLabel.Location = New Point(1140, 53)
        Me.PumpNameLabel.Name = "PumpNameLabel"
        Me.PumpNameLabel.Size = New Size(230, 21)
        Me.PumpNameLabel.TabIndex = 70
        Me.PumpNameLabel.Text = "Pump Name"
        '
        ' NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), Icon)
        Me.NotifyIcon1.Text = "CareLink For Windows"
        Me.NotifyIcon1.Visible = True
        '
        ' PumpBatteryPictureBox
        '
        Me.PumpBatteryPictureBox.ErrorImage = Nothing
        Me.PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryFull
        Me.PumpBatteryPictureBox.Location = New Point(124, 0)
        Me.PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        Me.PumpBatteryPictureBox.Size = New Size(74, 84)
        Me.PumpBatteryPictureBox.TabIndex = 43
        Me.PumpBatteryPictureBox.TabStop = False
        '
        ' PumpBatteryRemainingLabel
        '
        Me.PumpBatteryRemainingLabel.BackColor = Color.Transparent
        Me.PumpBatteryRemainingLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.PumpBatteryRemainingLabel.ForeColor = Color.White
        Me.PumpBatteryRemainingLabel.Location = New Point(119, 89)
        Me.PumpBatteryRemainingLabel.Name = "PumpBatteryRemainingLabel"
        Me.PumpBatteryRemainingLabel.Size = New Size(84, 21)
        Me.PumpBatteryRemainingLabel.TabIndex = 11
        Me.PumpBatteryRemainingLabel.Text = "Unknown"
        Me.PumpBatteryRemainingLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' InsulinTypeLabel
        '
        Me.InsulinTypeLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.InsulinTypeLabel.ForeColor = Color.White
        Me.InsulinTypeLabel.Location = New Point(978, 3)
        Me.InsulinTypeLabel.Name = "InsulinTypeLabel"
        Me.InsulinTypeLabel.Size = New Size(162, 21)
        Me.InsulinTypeLabel.TabIndex = 54
        Me.InsulinTypeLabel.Text = "Humalog/Novolog"
        Me.InsulinTypeLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' ReadingsLabel
        '
        Me.ReadingsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.ReadingsLabel.ForeColor = Color.White
        Me.ReadingsLabel.Location = New Point(977, 106)
        Me.ReadingsLabel.Name = "ReadingsLabel"
        Me.ReadingsLabel.Size = New Size(165, 21)
        Me.ReadingsLabel.TabIndex = 53
        Me.ReadingsLabel.Text = "280/288 Readings"
        Me.ReadingsLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' RemainingInsulinUnits
        '
        Me.RemainingInsulinUnits.BackColor = Color.Transparent
        Me.RemainingInsulinUnits.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.RemainingInsulinUnits.ForeColor = Color.White
        Me.RemainingInsulinUnits.Location = New Point(206, 90)
        Me.RemainingInsulinUnits.Name = "RemainingInsulinUnits"
        Me.RemainingInsulinUnits.Size = New Size(80, 21)
        Me.RemainingInsulinUnits.TabIndex = 12
        Me.RemainingInsulinUnits.Text = "000.0 U"
        Me.RemainingInsulinUnits.TextAlign = ContentAlignment.MiddleCenter
        '
        ' SensorDaysLeftLabel
        '
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.SensorDaysLeftLabel.ForeColor = Color.White
        Me.SensorDaysLeftLabel.Location = New Point(0, 16)
        Me.SensorDaysLeftLabel.Name = "SensorDaysLeftLabel"
        Me.SensorDaysLeftLabel.Size = New Size(55, 40)
        Me.SensorDaysLeftLabel.TabIndex = 45
        Me.SensorDaysLeftLabel.Text = "<1"
        Me.SensorDaysLeftLabel.TextAlign = ContentAlignment.MiddleCenter
        Me.SensorDaysLeftLabel.Visible = False
        '
        ' SensorTimeLeftLabel
        '
        Me.SensorTimeLeftLabel.BackColor = Color.Transparent
        Me.SensorTimeLeftLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.SensorTimeLeftLabel.ForeColor = Color.White
        Me.SensorTimeLeftLabel.Location = New Point(0, 89)
        Me.SensorTimeLeftLabel.Name = "SensorTimeLeftLabel"
        Me.SensorTimeLeftLabel.Size = New Size(94, 21)
        Me.SensorTimeLeftLabel.TabIndex = 46
        Me.SensorTimeLeftLabel.Text = "???"
        Me.SensorTimeLeftLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' SensorTimeLeftPanel
        '
        Me.SensorTimeLeftPanel.Controls.Add(Me.SensorDaysLeftLabel)
        Me.SensorTimeLeftPanel.Controls.Add(Me.SensorTimeLeftLabel)
        Me.SensorTimeLeftPanel.Controls.Add(Me.SensorTimeLeftPictureBox)
        Me.SensorTimeLeftPanel.Location = New Point(638, 0)
        Me.SensorTimeLeftPanel.Name = "SensorTimeLeftPanel"
        Me.SensorTimeLeftPanel.Size = New Size(94, 129)
        Me.SensorTimeLeftPanel.TabIndex = 65
        '
        ' SensorTimeLeftPictureBox
        '
        Me.SensorTimeLeftPictureBox.ErrorImage = Nothing
        Me.SensorTimeLeftPictureBox.Image = My.Resources.Resources.SensorExpirationUnknown
        Me.SensorTimeLeftPictureBox.Location = New Point(15, 0)
        Me.SensorTimeLeftPictureBox.Name = "SensorTimeLeftPictureBox"
        Me.SensorTimeLeftPictureBox.Size = New Size(74, 84)
        Me.SensorTimeLeftPictureBox.TabIndex = 47
        Me.SensorTimeLeftPictureBox.TabStop = False
        '
        ' SerialNumberLabel
        '
        Me.SerialNumberLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.SerialNumberLabel.ForeColor = Color.White
        Me.SerialNumberLabel.Location = New Point(1140, 78)
        Me.SerialNumberLabel.Name = "SerialNumberLabel"
        Me.SerialNumberLabel.Size = New Size(230, 21)
        Me.SerialNumberLabel.TabIndex = 56
        Me.SerialNumberLabel.Text = "Serial Number"
        '
        ' ServerUpdateTimer
        '
        Me.ServerUpdateTimer.Interval = 300000
        '
        ' SplitContainer2
        '
        Me.SplitContainer2.Dock = DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New Point(3, 3)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = Orientation.Horizontal
        '
        ' SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpAITLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelTrendValue)
        Me.SplitContainer2.Panel1.Controls.Add(Me.MaxBasalPerHourLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Last24HTotalsPanel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorTimeLeftPanel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelTrendArrows)
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelSgTrend)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ModelLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpNameLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SerialNumberLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.InsulinTypeLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ReadingsLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryRemainingLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryRemaining2Label)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TransmitterBatteryPercentLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TransmitterBatteryPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.FullNameLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.RemainingInsulinUnits)
        Me.SplitContainer2.Panel1.Controls.Add(Me.InsulinLevelPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ActiveInsulinValue)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CalibrationDueImage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CursorPanel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CalibrationShieldPanel)
        '
        ' SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Size = New Size(1370, 631)
        Me.SplitContainer2.SplitterDistance = 130
        Me.SplitContainer2.TabIndex = 52
        '
        ' PumpAITLabel
        '
        Me.PumpAITLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.PumpAITLabel.ForeColor = Color.White
        Me.PumpAITLabel.Location = New Point(978, 28)
        Me.PumpAITLabel.Name = "PumpAITLabel"
        Me.PumpAITLabel.Size = New Size(162, 21)
        Me.PumpAITLabel.TabIndex = 71
        Me.PumpAITLabel.Text = "Pump AIT 3:00"
        Me.PumpAITLabel.TextAlign = ContentAlignment.BottomCenter
        '
        ' PumpBatteryRemaining2Label
        '
        Me.PumpBatteryRemaining2Label.BackColor = Color.Transparent
        Me.PumpBatteryRemaining2Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.PumpBatteryRemaining2Label.ForeColor = Color.White
        Me.PumpBatteryRemaining2Label.Location = New Point(119, 106)
        Me.PumpBatteryRemaining2Label.Name = "PumpBatteryRemaining2Label"
        Me.PumpBatteryRemaining2Label.Size = New Size(84, 21)
        Me.PumpBatteryRemaining2Label.TabIndex = 69
        Me.PumpBatteryRemaining2Label.TextAlign = ContentAlignment.MiddleCenter
        '
        ' TransmitterBatteryPercentLabel
        '
        Me.TransmitterBatteryPercentLabel.BackColor = Color.Transparent
        Me.TransmitterBatteryPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.TransmitterBatteryPercentLabel.ForeColor = Color.White
        Me.TransmitterBatteryPercentLabel.Location = New Point(549, 89)
        Me.TransmitterBatteryPercentLabel.Name = "TransmitterBatteryPercentLabel"
        Me.TransmitterBatteryPercentLabel.Size = New Size(85, 21)
        Me.TransmitterBatteryPercentLabel.TabIndex = 13
        Me.TransmitterBatteryPercentLabel.Text = "???"
        Me.TransmitterBatteryPercentLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' TransmitterBatteryPictureBox
        '
        Me.TransmitterBatteryPictureBox.ErrorImage = Nothing
        Me.TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryUnknown
        Me.TransmitterBatteryPictureBox.Location = New Point(554, 0)
        Me.TransmitterBatteryPictureBox.Name = "TransmitterBatteryPictureBox"
        Me.TransmitterBatteryPictureBox.Size = New Size(74, 84)
        Me.TransmitterBatteryPictureBox.TabIndex = 47
        Me.TransmitterBatteryPictureBox.TabStop = False
        '
        ' SplitContainer3
        '
        Me.SplitContainer3.IsSplitterFixed = True
        Me.SplitContainer3.Location = New Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        ' SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.BackColor = Color.Black
        '
        ' SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.Last24HoursLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeSummaryPercentCharLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeChartLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.InRangeMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AboveHighLimitMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.BelowLowLimitValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AboveHighLimitValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.BelowLowLimitMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AverageSGValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AverageSGMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.SmartGuardLabel)
        Me.SplitContainer3.Size = New Size(1370, 498)
        Me.SplitContainer3.SplitterDistance = 1136
        Me.SplitContainer3.TabIndex = 0
        '
        ' TimeInRangeLabel
        '
        Me.TimeInRangeLabel.Anchor = AnchorStyles.Top
        Me.TimeInRangeLabel.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold, GraphicsUnit.Point)
        Me.TimeInRangeLabel.ForeColor = Color.White
        Me.TimeInRangeLabel.Location = New Point(30, 0)
        Me.TimeInRangeLabel.Name = "TimeInRangeLabel"
        Me.TimeInRangeLabel.Size = New Size(170, 21)
        Me.TimeInRangeLabel.TabIndex = 33
        Me.TimeInRangeLabel.Text = "Time in range"
        Me.TimeInRangeLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' TimeInRangeSummaryPercentCharLabel
        '
        Me.TimeInRangeSummaryPercentCharLabel.Anchor = AnchorStyles.Top
        Me.TimeInRangeSummaryPercentCharLabel.AutoSize = True
        Me.TimeInRangeSummaryPercentCharLabel.BackColor = Color.Transparent
        Me.TimeInRangeSummaryPercentCharLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.TimeInRangeSummaryPercentCharLabel.ForeColor = Color.White
        Me.TimeInRangeSummaryPercentCharLabel.Location = New Point(94, 133)
        Me.TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        Me.TimeInRangeSummaryPercentCharLabel.Size = New Size(42, 40)
        Me.TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        Me.TimeInRangeSummaryPercentCharLabel.Text = "%"
        '
        ' TimeInRangeChartLabel
        '
        Me.TimeInRangeChartLabel.Anchor = AnchorStyles.Top
        Me.TimeInRangeChartLabel.BackColor = Color.Black
        Me.TimeInRangeChartLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.TimeInRangeChartLabel.ForeColor = Color.White
        Me.TimeInRangeChartLabel.Location = New Point(65, 94)
        Me.TimeInRangeChartLabel.Name = "TimeInRangeChartLabel"
        Me.TimeInRangeChartLabel.Size = New Size(100, 47)
        Me.TimeInRangeChartLabel.TabIndex = 2
        Me.TimeInRangeChartLabel.Text = "100"
        Me.TimeInRangeChartLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' TimeInRangeValueLabel
        '
        Me.TimeInRangeValueLabel.Anchor = AnchorStyles.Top
        Me.TimeInRangeValueLabel.BackColor = Color.Black
        Me.TimeInRangeValueLabel.Font = New Font("Microsoft Sans Serif", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        Me.TimeInRangeValueLabel.ForeColor = Color.White
        Me.TimeInRangeValueLabel.Location = New Point(55, 276)
        Me.TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        Me.TimeInRangeValueLabel.Size = New Size(120, 33)
        Me.TimeInRangeValueLabel.TabIndex = 24
        Me.TimeInRangeValueLabel.Text = "90 %"
        Me.TimeInRangeValueLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' SmartGuardLabel
        '
        Me.SmartGuardLabel.Anchor = AnchorStyles.Top
        Me.SmartGuardLabel.BackColor = Color.Transparent
        Me.SmartGuardLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
        Me.SmartGuardLabel.ForeColor = Color.DodgerBlue
        Me.SmartGuardLabel.Location = New Point(3, 463)
        Me.SmartGuardLabel.Name = "SmartGuardLabel"
        Me.SmartGuardLabel.Size = New Size(224, 21)
        Me.SmartGuardLabel.TabIndex = 35
        Me.SmartGuardLabel.Text = "SmartGuard 100%"
        Me.SmartGuardLabel.TextAlign = ContentAlignment.MiddleCenter
        '
        ' TabControlPage1
        '
        Me.TabControlPage1.Appearance = TabAppearance.Buttons
        Me.TabControlPage1.Controls.Add(Me.TabPage01HomePage)
        Me.TabControlPage1.Controls.Add(Me.TabPage02RunningIOB)
        Me.TabControlPage1.Controls.Add(Me.TabPage03TreatmentDetails)
        Me.TabControlPage1.Controls.Add(Me.TabPage04SummaryData)
        Me.TabControlPage1.Controls.Add(Me.TabPage05Insulin)
        Me.TabControlPage1.Controls.Add(Me.TabPage06Meal)
        Me.TabControlPage1.Controls.Add(Me.TabPage07ActiveInsulin)
        Me.TabControlPage1.Controls.Add(Me.TabPage08SensorGlucose)
        Me.TabControlPage1.Controls.Add(Me.TabPage09Limits)
        Me.TabControlPage1.Controls.Add(Me.TabPage10NotificationHistory)
        Me.TabControlPage1.Controls.Add(Me.TabPage11TherapyAlgorithm)
        Me.TabControlPage1.Controls.Add(Me.TabPage12BannerState)
        Me.TabControlPage1.Controls.Add(Me.TabPage13Basal)
        Me.TabControlPage1.Controls.Add(Me.TabPage14Markers)
        Me.TabControlPage1.Dock = DockStyle.Fill
        Me.TabControlPage1.Location = New Point(0, 0)
        Me.TabControlPage1.Name = "TabControlPage1"
        Me.TabControlPage1.SelectedIndex = 0
        Me.TabControlPage1.Size = New Size(1384, 716)
        Me.TabControlPage1.TabIndex = 0
        '
        ' TabPage01HomePage
        '
        Me.TabPage01HomePage.BackColor = Color.Black
        Me.TabPage01HomePage.Controls.Add(Me.SplitContainer2)
        Me.TabPage01HomePage.Location = New Point(4, 27)
        Me.TabPage01HomePage.Name = "TabPage01HomePage"
        Me.TabPage01HomePage.Padding = New Padding(3)
        Me.TabPage01HomePage.Size = New Size(1376, 637)
        Me.TabPage01HomePage.TabIndex = 7
        Me.TabPage01HomePage.Text = "Summary"
        '
        ' TabPage02RunningIOB
        '
        Me.TabPage02RunningIOB.Controls.Add(Me.SplitContainer1)
        Me.TabPage02RunningIOB.Location = New Point(4, 27)
        Me.TabPage02RunningIOB.Name = "TabPage02RunningIOB"
        Me.TabPage02RunningIOB.Padding = New Padding(3)
        Me.TabPage02RunningIOB.Size = New Size(1376, 637)
        Me.TabPage02RunningIOB.TabIndex = 15
        Me.TabPage02RunningIOB.Text = "Running IOB"
        Me.TabPage02RunningIOB.UseVisualStyleBackColor = True
        '
        ' SplitContainer1
        '
        Me.SplitContainer1.Dock = DockStyle.Fill
        Me.SplitContainer1.FixedPanel = FixedPanel.Panel1
        Me.SplitContainer1.Location = New Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = Orientation.Horizontal
        '
        ' SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = Color.Black
        Me.SplitContainer1.Panel1.Controls.Add(Me.TemporaryUseAdvanceAITDecayCheckBox)
        Me.SplitContainer1.Size = New Size(1370, 631)
        Me.SplitContainer1.SplitterDistance = 30
        Me.SplitContainer1.TabIndex = 0
        '
        ' TemporaryUseAdvanceAITDecayCheckBox
        '
        Me.TemporaryUseAdvanceAITDecayCheckBox.AutoSize = True
        Me.TemporaryUseAdvanceAITDecayCheckBox.BackColor = SystemColors.ControlText
        Me.TemporaryUseAdvanceAITDecayCheckBox.ForeColor = SystemColors.ControlLightLight
        Me.TemporaryUseAdvanceAITDecayCheckBox.Location = New Point(12, 6)
        Me.TemporaryUseAdvanceAITDecayCheckBox.Name = "TemporaryUseAdvanceAITDecayCheckBox"
        Me.TemporaryUseAdvanceAITDecayCheckBox.Size = New Size(146, 19)
        Me.TemporaryUseAdvanceAITDecayCheckBox.TabIndex = 0
        Me.TemporaryUseAdvanceAITDecayCheckBox.Text = "AIT Decay over 3 hours"
        Me.TemporaryUseAdvanceAITDecayCheckBox.UseVisualStyleBackColor = False
        '
        ' TabPage03TreatmentDetails
        '
        Me.TabPage03TreatmentDetails.Location = New Point(4, 27)
        Me.TabPage03TreatmentDetails.Name = "TabPage03TreatmentDetails"
        Me.TabPage03TreatmentDetails.Padding = New Padding(3)
        Me.TabPage03TreatmentDetails.Size = New Size(1376, 637)
        Me.TabPage03TreatmentDetails.TabIndex = 8
        Me.TabPage03TreatmentDetails.Text = "Treatment Details"
        Me.TabPage03TreatmentDetails.UseVisualStyleBackColor = True
        '
        ' TabPage04SummaryData
        '
        Me.TabPage04SummaryData.Controls.Add(Me.DgvSummary)
        Me.TabPage04SummaryData.Location = New Point(4, 27)
        Me.TabPage04SummaryData.Name = "TabPage04SummaryData"
        Me.TabPage04SummaryData.Padding = New Padding(3)
        Me.TabPage04SummaryData.Size = New Size(1376, 637)
        Me.TabPage04SummaryData.TabIndex = 0
        Me.TabPage04SummaryData.Text = "Summary Data As Table"
        Me.TabPage04SummaryData.UseVisualStyleBackColor = True
        '
        ' TabPage05Insulin
        '
        Me.TabPage05Insulin.Controls.Add(Me.TableLayoutPanelInsulin)
        Me.TabPage05Insulin.Location = New Point(4, 27)
        Me.TabPage05Insulin.Name = "TabPage05Insulin"
        Me.TabPage05Insulin.Padding = New Padding(3)
        Me.TabPage05Insulin.Size = New Size(1376, 637)
        Me.TabPage05Insulin.TabIndex = 4
        Me.TabPage05Insulin.Text = "Insulin"
        Me.TabPage05Insulin.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelInsulin
        '
        Me.TableLayoutPanelInsulin.AutoScroll = True
        Me.TableLayoutPanelInsulin.AutoSize = True
        Me.TableLayoutPanelInsulin.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelInsulin.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelInsulin.ColumnCount = 1
        Me.TableLayoutPanelInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelInsulin.Controls.Add(Me.TableLayoutPanelInsulinTop, 0, 0)
        Me.TableLayoutPanelInsulin.Controls.Add(Me.DgvInsulin, 0, 1)
        Me.TableLayoutPanelInsulin.Dock = DockStyle.Fill
        Me.TableLayoutPanelInsulin.Location = New Point(3, 3)
        Me.TableLayoutPanelInsulin.Name = "TableLayoutPanelInsulin"
        Me.TableLayoutPanelInsulin.RowCount = 2
        Me.TableLayoutPanelInsulin.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelInsulin.Size = New Size(1370, 631)
        Me.TableLayoutPanelInsulin.TabIndex = 1
        '
        ' TableLayoutPanelInsulinTop
        '
        Me.TableLayoutPanelInsulinTop.AutoSize = True
        Me.TableLayoutPanelInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelInsulinTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelInsulinTop.ColumnCount = 2
        Me.TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelInsulinTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelInsulinTop.LabelText = "Insulin"
        Me.TableLayoutPanelInsulinTop.Location = New Point(6, 6)
        Me.TableLayoutPanelInsulinTop.Name = "TableLayoutPanelInsulinTop"
        Me.TableLayoutPanelInsulinTop.RowCount = 1
        Me.TableLayoutPanelInsulinTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelInsulinTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelInsulinTop.TabIndex = 1
        '
        ' TabPage06Meal
        '
        Me.TabPage06Meal.Controls.Add(Me.TableLayoutPanelMeal)
        Me.TabPage06Meal.Location = New Point(4, 27)
        Me.TabPage06Meal.Name = "TabPage06Meal"
        Me.TabPage06Meal.Padding = New Padding(3)
        Me.TabPage06Meal.Size = New Size(1376, 637)
        Me.TabPage06Meal.TabIndex = 6
        Me.TabPage06Meal.Text = "Meal"
        Me.TabPage06Meal.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelMeal
        '
        Me.TableLayoutPanelMeal.AutoScroll = True
        Me.TableLayoutPanelMeal.AutoSize = True
        Me.TableLayoutPanelMeal.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelMeal.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelMeal.ColumnCount = 1
        Me.TableLayoutPanelMeal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelMeal.Controls.Add(Me.TableLayoutPanelMealTop, 0, 0)
        Me.TableLayoutPanelMeal.Controls.Add(Me.DgvMeal, 0, 1)
        Me.TableLayoutPanelMeal.Dock = DockStyle.Fill
        Me.TableLayoutPanelMeal.Location = New Point(3, 3)
        Me.TableLayoutPanelMeal.Name = "TableLayoutPanelMeal"
        Me.TableLayoutPanelMeal.RowCount = 2
        Me.TableLayoutPanelMeal.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelMeal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelMeal.Size = New Size(1370, 631)
        Me.TableLayoutPanelMeal.TabIndex = 1
        '
        ' TableLayoutPanelMealTop
        '
        Me.TableLayoutPanelMealTop.AutoSize = True
        Me.TableLayoutPanelMealTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelMealTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelMealTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelMealTop.ColumnCount = 2
        Me.TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelMealTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelMealTop.LabelText = "Meal"
        Me.TableLayoutPanelMealTop.Location = New Point(6, 6)
        Me.TableLayoutPanelMealTop.Name = "TableLayoutPanelMealTop"
        Me.TableLayoutPanelMealTop.RowCount = 1
        Me.TableLayoutPanelMealTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelMealTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelMealTop.TabIndex = 1
        '
        ' TabPage07ActiveInsulin
        '
        Me.TabPage07ActiveInsulin.Controls.Add(Me.TableLayoutPanelActiveInsulin)
        Me.TabPage07ActiveInsulin.Location = New Point(4, 27)
        Me.TabPage07ActiveInsulin.Name = "TabPage07ActiveInsulin"
        Me.TabPage07ActiveInsulin.Padding = New Padding(3)
        Me.TabPage07ActiveInsulin.Size = New Size(1376, 637)
        Me.TabPage07ActiveInsulin.TabIndex = 18
        Me.TabPage07ActiveInsulin.Text = "Active Insulin"
        Me.TabPage07ActiveInsulin.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelActiveInsulin
        '
        Me.TableLayoutPanelActiveInsulin.AutoSize = True
        Me.TableLayoutPanelActiveInsulin.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelActiveInsulin.ColumnCount = 1
        Me.TableLayoutPanelActiveInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelActiveInsulin.Controls.Add(Me.TableLayoutPanelActiveInsulinTop, 0, 0)
        Me.TableLayoutPanelActiveInsulin.Dock = DockStyle.Fill
        Me.TableLayoutPanelActiveInsulin.Location = New Point(3, 3)
        Me.TableLayoutPanelActiveInsulin.Name = "TableLayoutPanelActiveInsulin"
        Me.TableLayoutPanelActiveInsulin.RowCount = 2
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelActiveInsulin.Size = New Size(1370, 631)
        Me.TableLayoutPanelActiveInsulin.TabIndex = 0
        '
        ' TableLayoutPanelActiveInsulinTop
        '
        Me.TableLayoutPanelActiveInsulinTop.AutoSize = True
        Me.TableLayoutPanelActiveInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelActiveInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelActiveInsulinTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelActiveInsulinTop.ColumnCount = 2
        Me.TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelActiveInsulinTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelActiveInsulinTop.LabelText = "Active Insulin"
        Me.TableLayoutPanelActiveInsulinTop.Location = New Point(3, 3)
        Me.TableLayoutPanelActiveInsulinTop.Name = "TableLayoutPanelActiveInsulinTop"
        Me.TableLayoutPanelActiveInsulinTop.RowCount = 1
        Me.TableLayoutPanelActiveInsulinTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelActiveInsulinTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelActiveInsulinTop.TabIndex = 1
        '
        ' TabPage08SensorGlucose
        '
        Me.TabPage08SensorGlucose.Controls.Add(Me.TableLayoutPanelSgs)
        Me.TabPage08SensorGlucose.Location = New Point(4, 27)
        Me.TabPage08SensorGlucose.Name = "TabPage08SensorGlucose"
        Me.TabPage08SensorGlucose.Padding = New Padding(3)
        Me.TabPage08SensorGlucose.Size = New Size(1376, 637)
        Me.TabPage08SensorGlucose.TabIndex = 19
        Me.TabPage08SensorGlucose.Text = "Sensor Glucose"
        Me.TabPage08SensorGlucose.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelSgs
        '
        Me.TableLayoutPanelSgs.AutoSize = True
        Me.TableLayoutPanelSgs.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelSgs.ColumnCount = 1
        Me.TableLayoutPanelSgs.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelSgs.Controls.Add(Me.TableLayoutPanelSgsTop, 0, 0)
        Me.TableLayoutPanelSgs.Controls.Add(Me.DgvSGs, 0, 1)
        Me.TableLayoutPanelSgs.Dock = DockStyle.Fill
        Me.TableLayoutPanelSgs.Location = New Point(3, 3)
        Me.TableLayoutPanelSgs.Name = "TableLayoutPanelSgs"
        Me.TableLayoutPanelSgs.RowCount = 2
        Me.TableLayoutPanelSgs.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelSgs.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelSgs.Size = New Size(1370, 631)
        Me.TableLayoutPanelSgs.TabIndex = 1
        '
        ' TableLayoutPanelSgsTop
        '
        Me.TableLayoutPanelSgsTop.AutoSize = True
        Me.TableLayoutPanelSgsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelSgsTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelSgsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelSgsTop.ColumnCount = 2
        Me.TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelSgsTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelSgsTop.LabelText = "SGs"
        Me.TableLayoutPanelSgsTop.Location = New Point(3, 3)
        Me.TableLayoutPanelSgsTop.Name = "TableLayoutPanelSgsTop"
        Me.TableLayoutPanelSgsTop.RowCount = 1
        Me.TableLayoutPanelSgsTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelSgsTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelSgsTop.TabIndex = 1
        '
        ' TabPage09Limits
        '
        Me.TabPage09Limits.Controls.Add(Me.TableLayoutPanelLimits)
        Me.TabPage09Limits.Location = New Point(4, 27)
        Me.TabPage09Limits.Name = "TabPage09Limits"
        Me.TabPage09Limits.Padding = New Padding(3)
        Me.TabPage09Limits.Size = New Size(1376, 637)
        Me.TabPage09Limits.TabIndex = 20
        Me.TabPage09Limits.Text = "Limits"
        Me.TabPage09Limits.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelLimits
        '
        Me.TableLayoutPanelLimits.AutoSize = True
        Me.TableLayoutPanelLimits.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLimits.ColumnCount = 1
        Me.TableLayoutPanelLimits.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLimits.Controls.Add(Me.TableLayoutPanelLimitsTop, 0, 0)
        Me.TableLayoutPanelLimits.Dock = DockStyle.Fill
        Me.TableLayoutPanelLimits.Location = New Point(3, 3)
        Me.TableLayoutPanelLimits.Name = "TableLayoutPanelLimits"
        Me.TableLayoutPanelLimits.RowCount = 2
        Me.TableLayoutPanelLimits.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLimits.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLimits.Size = New Size(1370, 631)
        Me.TableLayoutPanelLimits.TabIndex = 0
        '
        ' TableLayoutPanelLimitsTop
        '
        Me.TableLayoutPanelLimitsTop.AutoSize = True
        Me.TableLayoutPanelLimitsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLimitsTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLimitsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLimitsTop.ColumnCount = 2
        Me.TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLimitsTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelLimitsTop.LabelText = "Limits"
        Me.TableLayoutPanelLimitsTop.Location = New Point(3, 3)
        Me.TableLayoutPanelLimitsTop.Name = "TableLayoutPanelLimitsTop"
        Me.TableLayoutPanelLimitsTop.RowCount = 1
        Me.TableLayoutPanelLimitsTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLimitsTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelLimitsTop.TabIndex = 1
        '
        ' TabPage10NotificationHistory
        '
        Me.TabPage10NotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistory)
        Me.TabPage10NotificationHistory.Location = New Point(4, 27)
        Me.TabPage10NotificationHistory.Name = "TabPage10NotificationHistory"
        Me.TabPage10NotificationHistory.Padding = New Padding(3)
        Me.TabPage10NotificationHistory.Size = New Size(1376, 637)
        Me.TabPage10NotificationHistory.TabIndex = 5
        Me.TabPage10NotificationHistory.Text = "Notification History"
        Me.TabPage10NotificationHistory.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelNotificationHistory
        '
        Me.TableLayoutPanelNotificationHistory.AutoScroll = True
        Me.TableLayoutPanelNotificationHistory.AutoSize = True
        Me.TableLayoutPanelNotificationHistory.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelNotificationHistory.ColumnCount = 1
        Me.TableLayoutPanelNotificationHistory.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelNotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistoryTop, 1, 0)
        Me.TableLayoutPanelNotificationHistory.Dock = DockStyle.Fill
        Me.TableLayoutPanelNotificationHistory.Location = New Point(3, 3)
        Me.TableLayoutPanelNotificationHistory.Name = "TableLayoutPanelNotificationHistory"
        Me.TableLayoutPanelNotificationHistory.RowCount = 2
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelNotificationHistory.Size = New Size(1370, 631)
        Me.TableLayoutPanelNotificationHistory.TabIndex = 0
        '
        ' TableLayoutPanelNotificationHistoryTop
        '
        Me.TableLayoutPanelNotificationHistoryTop.AutoSize = True
        Me.TableLayoutPanelNotificationHistoryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelNotificationHistoryTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelNotificationHistoryTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelNotificationHistoryTop.ColumnCount = 2
        Me.TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelNotificationHistoryTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelNotificationHistoryTop.LabelText = "Notification History"
        Me.TableLayoutPanelNotificationHistoryTop.Location = New Point(3, 3)
        Me.TableLayoutPanelNotificationHistoryTop.Name = "TableLayoutPanelNotificationHistoryTop"
        Me.TableLayoutPanelNotificationHistoryTop.RowCount = 1
        Me.TableLayoutPanelNotificationHistoryTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelNotificationHistoryTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelNotificationHistoryTop.TabIndex = 1
        '
        ' TabPage11TherapyAlgorithm
        '
        Me.TabPage11TherapyAlgorithm.Controls.Add(Me.TableLayoutPanelTherapyAlgorithm)
        Me.TabPage11TherapyAlgorithm.Location = New Point(4, 27)
        Me.TabPage11TherapyAlgorithm.Name = "TabPage11TherapyAlgorithm"
        Me.TabPage11TherapyAlgorithm.Padding = New Padding(3)
        Me.TabPage11TherapyAlgorithm.Size = New Size(1376, 637)
        Me.TabPage11TherapyAlgorithm.TabIndex = 21
        Me.TabPage11TherapyAlgorithm.Text = "Therapy Algorithm"
        Me.TabPage11TherapyAlgorithm.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelTherapyAlgorithm
        '
        Me.TableLayoutPanelTherapyAlgorithm.AutoSize = True
        Me.TableLayoutPanelTherapyAlgorithm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTherapyAlgorithm.ColumnCount = 1
        Me.TableLayoutPanelTherapyAlgorithm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelTherapyAlgorithm.Controls.Add(Me.TableLayoutPanelTherapyAlgorithmTop, 0, 0)
        Me.TableLayoutPanelTherapyAlgorithm.Dock = DockStyle.Fill
        Me.TableLayoutPanelTherapyAlgorithm.Location = New Point(3, 3)
        Me.TableLayoutPanelTherapyAlgorithm.Name = "TableLayoutPanelTherapyAlgorithm"
        Me.TableLayoutPanelTherapyAlgorithm.RowCount = 2
        Me.TableLayoutPanelTherapyAlgorithm.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelTherapyAlgorithm.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelTherapyAlgorithm.Size = New Size(1370, 631)
        Me.TableLayoutPanelTherapyAlgorithm.TabIndex = 0
        '
        ' TableLayoutPanelTherapyAlgorithmTop
        '
        Me.TableLayoutPanelTherapyAlgorithmTop.AutoSize = True
        Me.TableLayoutPanelTherapyAlgorithmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTherapyAlgorithmTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelTherapyAlgorithmTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTherapyAlgorithmTop.ColumnCount = 2
        Me.TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelTherapyAlgorithmTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelTherapyAlgorithmTop.LabelText = "Therapy Algorithm"
        Me.TableLayoutPanelTherapyAlgorithmTop.Location = New Point(3, 3)
        Me.TableLayoutPanelTherapyAlgorithmTop.Name = "TableLayoutPanelTherapyAlgorithmTop"
        Me.TableLayoutPanelTherapyAlgorithmTop.RowCount = 1
        Me.TableLayoutPanelTherapyAlgorithmTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelTherapyAlgorithmTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelTherapyAlgorithmTop.TabIndex = 1
        '
        ' TabPage12BannerState
        '
        Me.TabPage12BannerState.Controls.Add(Me.TableLayoutPanelBannerState)
        Me.TabPage12BannerState.Location = New Point(4, 27)
        Me.TabPage12BannerState.Name = "TabPage12BannerState"
        Me.TabPage12BannerState.Padding = New Padding(3)
        Me.TabPage12BannerState.Size = New Size(1376, 637)
        Me.TabPage12BannerState.TabIndex = 22
        Me.TabPage12BannerState.Text = "Banner State"
        Me.TabPage12BannerState.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelBannerState
        '
        Me.TableLayoutPanelBannerState.AutoSize = True
        Me.TableLayoutPanelBannerState.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBannerState.ColumnCount = 1
        Me.TableLayoutPanelBannerState.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBannerState.Controls.Add(Me.TableLayoutPanelBannerStateTop, 0, 0)
        Me.TableLayoutPanelBannerState.Dock = DockStyle.Fill
        Me.TableLayoutPanelBannerState.Location = New Point(3, 3)
        Me.TableLayoutPanelBannerState.Margin = New Padding(0)
        Me.TableLayoutPanelBannerState.Name = "TableLayoutPanelBannerState"
        Me.TableLayoutPanelBannerState.RowCount = 2
        Me.TableLayoutPanelBannerState.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelBannerState.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBannerState.Size = New Size(1370, 631)
        Me.TableLayoutPanelBannerState.TabIndex = 0
        '
        ' TableLayoutPanelBannerStateTop
        '
        Me.TableLayoutPanelBannerStateTop.AutoSize = True
        Me.TableLayoutPanelBannerStateTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBannerStateTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelBannerStateTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBannerStateTop.ColumnCount = 2
        Me.TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBannerStateTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelBannerStateTop.LabelText = "Banner State"
        Me.TableLayoutPanelBannerStateTop.Location = New Point(3, 3)
        Me.TableLayoutPanelBannerStateTop.Name = "TableLayoutPanelBannerStateTop"
        Me.TableLayoutPanelBannerStateTop.RowCount = 1
        Me.TableLayoutPanelBannerStateTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelBannerStateTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelBannerStateTop.TabIndex = 1
        '
        ' TabPage13Basal
        '
        Me.TabPage13Basal.Controls.Add(Me.TableLayoutPanelBasal)
        Me.TabPage13Basal.Location = New Point(4, 27)
        Me.TabPage13Basal.Name = "TabPage13Basal"
        Me.TabPage13Basal.Padding = New Padding(3)
        Me.TabPage13Basal.Size = New Size(1376, 637)
        Me.TabPage13Basal.TabIndex = 23
        Me.TabPage13Basal.Text = "Basal"
        Me.TabPage13Basal.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelBasal
        '
        Me.TableLayoutPanelBasal.AutoScroll = True
        Me.TableLayoutPanelBasal.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBasal.ColumnCount = 1
        Me.TableLayoutPanelBasal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBasal.Controls.Add(Me.TableLayoutPanelBasalTop, 0, 0)
        Me.TableLayoutPanelBasal.Dock = DockStyle.Fill
        Me.TableLayoutPanelBasal.Location = New Point(3, 3)
        Me.TableLayoutPanelBasal.Name = "TableLayoutPanelBasal"
        Me.TableLayoutPanelBasal.RowCount = 2
        Me.TableLayoutPanelBasal.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelBasal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBasal.Size = New Size(1370, 631)
        Me.TableLayoutPanelBasal.TabIndex = 0
        '
        ' TableLayoutPanelBasalTop
        '
        Me.TableLayoutPanelBasalTop.AutoSize = True
        Me.TableLayoutPanelBasalTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBasalTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelBasalTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBasalTop.ColumnCount = 2
        Me.TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBasalTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelBasalTop.LabelText = "Basal"
        Me.TableLayoutPanelBasalTop.Location = New Point(3, 3)
        Me.TableLayoutPanelBasalTop.Name = "TableLayoutPanelBasalTop"
        Me.TableLayoutPanelBasalTop.RowCount = 1
        Me.TableLayoutPanelBasalTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelBasalTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelBasalTop.TabIndex = 1
        '
        ' TabPage14Markers
        '
        Me.TabPage14Markers.BackColor = SystemColors.MenuHighlight
        Me.TabPage14Markers.Location = New Point(4, 27)
        Me.TabPage14Markers.Name = "TabPage14Markers"
        Me.TabPage14Markers.Padding = New Padding(3)
        Me.TabPage14Markers.Size = New Size(1376, 637)
        Me.TabPage14Markers.TabIndex = 24
        Me.TabPage14Markers.Text = "More..."
        '
        ' TabPageLastSG
        '
        Me.TabPageLastSG.Controls.Add(Me.TableLayoutPanelLastSG)
        Me.TabPageLastSG.Location = New Point(4, 27)
        Me.TabPageLastSG.Name = "TabPageLastSG"
        Me.TabPageLastSG.Padding = New Padding(3)
        Me.TabPageLastSG.Size = New Size(1376, 637)
        Me.TabPageLastSG.TabIndex = 16
        Me.TabPageLastSG.Text = "Last SG"
        Me.TabPageLastSG.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelLastSG
        '
        Me.TableLayoutPanelLastSG.AutoSize = True
        Me.TableLayoutPanelLastSG.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastSG.ColumnCount = 1
        Me.TableLayoutPanelLastSG.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLastSG.Controls.Add(Me.TableLayoutPanelLastSgTop, 0, 0)
        Me.TableLayoutPanelLastSG.Dock = DockStyle.Fill
        Me.TableLayoutPanelLastSG.Location = New Point(3, 3)
        Me.TableLayoutPanelLastSG.Name = "TableLayoutPanelLastSG"
        Me.TableLayoutPanelLastSG.RowCount = 2
        Me.TableLayoutPanelLastSG.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLastSG.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLastSG.Size = New Size(1370, 631)
        Me.TableLayoutPanelLastSG.TabIndex = 1
        '
        ' TableLayoutPanelLastSgTop
        '
        Me.TableLayoutPanelLastSgTop.AutoSize = True
        Me.TableLayoutPanelLastSgTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastSgTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLastSgTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLastSgTop.ColumnCount = 2
        Me.TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLastSgTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelLastSgTop.LabelText = "last SG"
        Me.TableLayoutPanelLastSgTop.Location = New Point(3, 3)
        Me.TableLayoutPanelLastSgTop.Name = "TableLayoutPanelLastSgTop"
        Me.TableLayoutPanelLastSgTop.RowCount = 1
        Me.TableLayoutPanelLastSgTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLastSgTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelLastSgTop.TabIndex = 1
        '
        ' TabPageLastAlarm
        '
        Me.TabPageLastAlarm.Controls.Add(Me.TableLayoutPanelLastAlarm)
        Me.TabPageLastAlarm.Location = New Point(4, 27)
        Me.TabPageLastAlarm.Name = "TabPageLastAlarm"
        Me.TabPageLastAlarm.Padding = New Padding(3)
        Me.TabPageLastAlarm.Size = New Size(1376, 637)
        Me.TabPageLastAlarm.TabIndex = 17
        Me.TabPageLastAlarm.Text = "Last Alarm"
        Me.TabPageLastAlarm.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelLastAlarm
        '
        Me.TableLayoutPanelLastAlarm.AutoSize = True
        Me.TableLayoutPanelLastAlarm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastAlarm.ColumnCount = 1
        Me.TableLayoutPanelLastAlarm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLastAlarm.Controls.Add(Me.TableLayoutPanelLastAlarmTop, 0, 0)
        Me.TableLayoutPanelLastAlarm.Dock = DockStyle.Fill
        Me.TableLayoutPanelLastAlarm.Location = New Point(3, 3)
        Me.TableLayoutPanelLastAlarm.Margin = New Padding(0)
        Me.TableLayoutPanelLastAlarm.Name = "TableLayoutPanelLastAlarm"
        Me.TableLayoutPanelLastAlarm.RowCount = 2
        Me.TableLayoutPanelLastAlarm.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLastAlarm.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLastAlarm.Size = New Size(1370, 631)
        Me.TableLayoutPanelLastAlarm.TabIndex = 0
        '
        ' TableLayoutPanelLastAlarmTop
        '
        Me.TableLayoutPanelLastAlarmTop.AutoSize = True
        Me.TableLayoutPanelLastAlarmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastAlarmTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLastAlarmTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLastAlarmTop.ColumnCount = 2
        Me.TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLastAlarmTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelLastAlarmTop.LabelText = "Last Alarm"
        Me.TableLayoutPanelLastAlarmTop.Location = New Point(3, 3)
        Me.TableLayoutPanelLastAlarmTop.Name = "TableLayoutPanelLastAlarmTop"
        Me.TableLayoutPanelLastAlarmTop.RowCount = 1
        Me.TableLayoutPanelLastAlarmTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLastAlarmTop.Size = New Size(1364, 37)
        Me.TableLayoutPanelLastAlarmTop.TabIndex = 1
        '
        ' TabControlPage2
        '
        Me.TabControlPage2.Appearance = TabAppearance.Buttons
        Me.TabControlPage2.Controls.Add(Me.TabPageAutoBasalDelivery)
        Me.TabControlPage2.Controls.Add(Me.TabPageAutoModeStatus)
        Me.TabControlPage2.Controls.Add(Me.TabPageBgReadings)
        Me.TabControlPage2.Controls.Add(Me.TabPageCalibration)
        Me.TabControlPage2.Controls.Add(Me.TabPageLowGlucoseSuspended)
        Me.TabControlPage2.Controls.Add(Me.TabPageTimeChange)
        Me.TabControlPage2.Controls.Add(Me.TabPageLastSG)
        Me.TabControlPage2.Controls.Add(Me.TabPageLastAlarm)
        Me.TabControlPage2.Controls.Add(Me.TabPageCountryDataPg1)
        Me.TabControlPage2.Controls.Add(Me.TabPageCountryDataPg2)
        Me.TabControlPage2.Controls.Add(Me.TabPageCountryDataPg3)
        Me.TabControlPage2.Controls.Add(Me.TabPageUserProfile)
        Me.TabControlPage2.Controls.Add(Me.TabPageCurrentUser)
        Me.TabControlPage2.Controls.Add(Me.TabPageAllUsers)
        Me.TabControlPage2.Controls.Add(Me.TabPageBackToHomePage)
        Me.TabControlPage2.Dock = DockStyle.Fill
        Me.TabControlPage2.Location = New Point(0, 24)
        Me.TabControlPage2.Name = "TabControlPage2"
        Me.TabControlPage2.SelectedIndex = 0
        Me.TabControlPage2.Size = New Size(1384, 668)
        Me.TabControlPage2.TabIndex = 0
        '
        ' TabPageAutoBasalDelivery
        '
        Me.TabPageAutoBasalDelivery.Controls.Add(Me.TableLayoutPanelAutoBasalDelivery)
        Me.TabPageAutoBasalDelivery.Location = New Point(4, 27)
        Me.TabPageAutoBasalDelivery.Name = "TabPageAutoBasalDelivery"
        Me.TabPageAutoBasalDelivery.Padding = New Padding(3)
        Me.TabPageAutoBasalDelivery.Size = New Size(1376, 637)
        Me.TabPageAutoBasalDelivery.TabIndex = 1
        Me.TabPageAutoBasalDelivery.Text = "Auto Basal Delivery"
        Me.TabPageAutoBasalDelivery.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelAutoBasalDelivery
        '
        Me.TableLayoutPanelAutoBasalDelivery.AutoSize = True
        Me.TableLayoutPanelAutoBasalDelivery.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoBasalDelivery.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoBasalDelivery.ColumnCount = 1
        Me.TableLayoutPanelAutoBasalDelivery.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelAutoBasalDelivery.Controls.Add(Me.TableLayoutPanelAutoBasalDeliveryTop, 0, 0)
        Me.TableLayoutPanelAutoBasalDelivery.Controls.Add(Me.DgvAutoBasalDelivery, 0, 1)
        Me.TableLayoutPanelAutoBasalDelivery.Dock = DockStyle.Fill
        Me.TableLayoutPanelAutoBasalDelivery.Location = New Point(3, 3)
        Me.TableLayoutPanelAutoBasalDelivery.Name = "TableLayoutPanelAutoBasalDelivery"
        Me.TableLayoutPanelAutoBasalDelivery.RowCount = 2
        Me.TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelAutoBasalDelivery.Size = New Size(1370, 631)
        Me.TableLayoutPanelAutoBasalDelivery.TabIndex = 0
        '
        ' TableLayoutPanelAutoBasalDeliveryTop
        '
        Me.TableLayoutPanelAutoBasalDeliveryTop.AutoSize = True
        Me.TableLayoutPanelAutoBasalDeliveryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoBasalDeliveryTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelAutoBasalDeliveryTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoBasalDeliveryTop.ColumnCount = 2
        Me.TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelAutoBasalDeliveryTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelAutoBasalDeliveryTop.LabelText = "Basal"
        Me.TableLayoutPanelAutoBasalDeliveryTop.Location = New Point(6, 6)
        Me.TableLayoutPanelAutoBasalDeliveryTop.Name = "TableLayoutPanelAutoBasalDeliveryTop"
        Me.TableLayoutPanelAutoBasalDeliveryTop.RowCount = 1
        Me.TableLayoutPanelAutoBasalDeliveryTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelAutoBasalDeliveryTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelAutoBasalDeliveryTop.TabIndex = 1
        '
        ' TabPageAutoModeStatus
        '
        Me.TabPageAutoModeStatus.Controls.Add(Me.TableLayoutPanelAutoModeStatus)
        Me.TabPageAutoModeStatus.Location = New Point(4, 27)
        Me.TabPageAutoModeStatus.Name = "TabPageAutoModeStatus"
        Me.TabPageAutoModeStatus.Padding = New Padding(3)
        Me.TabPageAutoModeStatus.Size = New Size(1376, 637)
        Me.TabPageAutoModeStatus.TabIndex = 0
        Me.TabPageAutoModeStatus.Text = "Auto Mode Status"
        Me.TabPageAutoModeStatus.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelAutoModeStatus
        '
        Me.TableLayoutPanelAutoModeStatus.AutoScroll = True
        Me.TableLayoutPanelAutoModeStatus.AutoSize = True
        Me.TableLayoutPanelAutoModeStatus.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoModeStatus.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoModeStatus.ColumnCount = 1
        Me.TableLayoutPanelAutoModeStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelAutoModeStatus.Controls.Add(Me.TableLayoutPanelAutoModeStatusTop, 0, 0)
        Me.TableLayoutPanelAutoModeStatus.Dock = DockStyle.Fill
        Me.TableLayoutPanelAutoModeStatus.Location = New Point(3, 3)
        Me.TableLayoutPanelAutoModeStatus.Name = "TableLayoutPanelAutoModeStatus"
        Me.TableLayoutPanelAutoModeStatus.RowCount = 2
        Me.TableLayoutPanelAutoModeStatus.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelAutoModeStatus.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelAutoModeStatus.Size = New Size(1370, 631)
        Me.TableLayoutPanelAutoModeStatus.TabIndex = 0
        '
        ' TableLayoutPanelAutoModeStatusTop
        '
        Me.TableLayoutPanelAutoModeStatusTop.AutoSize = True
        Me.TableLayoutPanelAutoModeStatusTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoModeStatusTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelAutoModeStatusTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoModeStatusTop.ColumnCount = 2
        Me.TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelAutoModeStatusTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelAutoModeStatusTop.LabelText = "Auto Mode Status"
        Me.TableLayoutPanelAutoModeStatusTop.Location = New Point(6, 6)
        Me.TableLayoutPanelAutoModeStatusTop.Name = "TableLayoutPanelAutoModeStatusTop"
        Me.TableLayoutPanelAutoModeStatusTop.RowCount = 1
        Me.TableLayoutPanelAutoModeStatusTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelAutoModeStatusTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelAutoModeStatusTop.TabIndex = 1
        '
        ' TabPageBgReadings
        '
        Me.TabPageBgReadings.Controls.Add(Me.TableLayoutPanelBgReadings)
        Me.TabPageBgReadings.Location = New Point(4, 27)
        Me.TabPageBgReadings.Name = "TabPageBgReadings"
        Me.TabPageBgReadings.Padding = New Padding(3)
        Me.TabPageBgReadings.Size = New Size(1376, 637)
        Me.TabPageBgReadings.TabIndex = 2
        Me.TabPageBgReadings.Text = "BG Readings"
        Me.TabPageBgReadings.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelBgReadings
        '
        Me.TableLayoutPanelBgReadings.AutoScroll = True
        Me.TableLayoutPanelBgReadings.AutoSize = True
        Me.TableLayoutPanelBgReadings.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBgReadings.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBgReadings.ColumnCount = 1
        Me.TableLayoutPanelBgReadings.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBgReadings.Controls.Add(Me.TableLayoutPanelBgReadingsTop, 0, 0)
        Me.TableLayoutPanelBgReadings.Dock = DockStyle.Fill
        Me.TableLayoutPanelBgReadings.Location = New Point(3, 3)
        Me.TableLayoutPanelBgReadings.Name = "TableLayoutPanelBgReadings"
        Me.TableLayoutPanelBgReadings.RowCount = 2
        Me.TableLayoutPanelBgReadings.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelBgReadings.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBgReadings.Size = New Size(1370, 631)
        Me.TableLayoutPanelBgReadings.TabIndex = 1
        '
        ' TableLayoutPanelBgReadingsTop
        '
        Me.TableLayoutPanelBgReadingsTop.AutoSize = True
        Me.TableLayoutPanelBgReadingsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBgReadingsTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelBgReadingsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBgReadingsTop.ColumnCount = 2
        Me.TableLayoutPanelBgReadingsTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelBgReadingsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelBgReadingsTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelBgReadingsTop.LabelText = "BG Readings"
        Me.TableLayoutPanelBgReadingsTop.Location = New Point(6, 6)
        Me.TableLayoutPanelBgReadingsTop.Name = "TableLayoutPanelBgReadingsTop"
        Me.TableLayoutPanelBgReadingsTop.RowCount = 1
        Me.TableLayoutPanelBgReadingsTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelBgReadingsTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelBgReadingsTop.TabIndex = 1
        '
        ' TabPageCalibration
        '
        Me.TabPageCalibration.Controls.Add(Me.TableLayoutPanelCalibration)
        Me.TabPageCalibration.Location = New Point(4, 27)
        Me.TabPageCalibration.Name = "TabPageCalibration"
        Me.TabPageCalibration.Padding = New Padding(3)
        Me.TabPageCalibration.Size = New Size(1376, 637)
        Me.TabPageCalibration.TabIndex = 3
        Me.TabPageCalibration.Text = "Calibration"
        Me.TabPageCalibration.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelCalibration
        '
        Me.TableLayoutPanelCalibration.AutoScroll = True
        Me.TableLayoutPanelCalibration.AutoSize = True
        Me.TableLayoutPanelCalibration.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelCalibration.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelCalibration.ColumnCount = 1
        Me.TableLayoutPanelCalibration.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelCalibration.Controls.Add(Me.TableLayoutPanelCalibrationTop, 0, 0)
        Me.TableLayoutPanelCalibration.Dock = DockStyle.Fill
        Me.TableLayoutPanelCalibration.Location = New Point(3, 3)
        Me.TableLayoutPanelCalibration.Name = "TableLayoutPanelCalibration"
        Me.TableLayoutPanelCalibration.RowCount = 2
        Me.TableLayoutPanelCalibration.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelCalibration.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelCalibration.Size = New Size(1370, 631)
        Me.TableLayoutPanelCalibration.TabIndex = 1
        '
        ' TableLayoutPanelCalibrationTop
        '
        Me.TableLayoutPanelCalibrationTop.AutoSize = True
        Me.TableLayoutPanelCalibrationTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelCalibrationTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelCalibrationTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelCalibrationTop.ColumnCount = 2
        Me.TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelCalibrationTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelCalibrationTop.LabelText = "Calibration"
        Me.TableLayoutPanelCalibrationTop.Location = New Point(6, 6)
        Me.TableLayoutPanelCalibrationTop.Name = "TableLayoutPanelCalibrationTop"
        Me.TableLayoutPanelCalibrationTop.RowCount = 1
        Me.TableLayoutPanelCalibrationTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelCalibrationTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelCalibrationTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelCalibrationTop.TabIndex = 1
        '
        ' TabPageLowGlucoseSuspended
        '
        Me.TabPageLowGlucoseSuspended.Controls.Add(Me.TableLayoutPanelLowGlucoseSuspended)
        Me.TabPageLowGlucoseSuspended.Location = New Point(4, 27)
        Me.TabPageLowGlucoseSuspended.Name = "TabPageLowGlucoseSuspended"
        Me.TabPageLowGlucoseSuspended.Padding = New Padding(3)
        Me.TabPageLowGlucoseSuspended.Size = New Size(1376, 637)
        Me.TabPageLowGlucoseSuspended.TabIndex = 5
        Me.TabPageLowGlucoseSuspended.Text = "Low Glucose Suspended"
        Me.TabPageLowGlucoseSuspended.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelLowGlucoseSuspended
        '
        Me.TableLayoutPanelLowGlucoseSuspended.AutoScroll = True
        Me.TableLayoutPanelLowGlucoseSuspended.AutoSize = True
        Me.TableLayoutPanelLowGlucoseSuspended.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLowGlucoseSuspended.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLowGlucoseSuspended.ColumnCount = 1
        Me.TableLayoutPanelLowGlucoseSuspended.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLowGlucoseSuspended.Controls.Add(Me.TableLayoutPanelLowGlucoseSuspendedTop, 0, 0)
        Me.TableLayoutPanelLowGlucoseSuspended.Dock = DockStyle.Fill
        Me.TableLayoutPanelLowGlucoseSuspended.Location = New Point(3, 3)
        Me.TableLayoutPanelLowGlucoseSuspended.Name = "TableLayoutPanelLowGlucoseSuspended"
        Me.TableLayoutPanelLowGlucoseSuspended.RowCount = 2
        Me.TableLayoutPanelLowGlucoseSuspended.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLowGlucoseSuspended.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLowGlucoseSuspended.Size = New Size(1370, 631)
        Me.TableLayoutPanelLowGlucoseSuspended.TabIndex = 1
        '
        ' TableLayoutPanelLowGlucoseSuspendedTop
        '
        Me.TableLayoutPanelLowGlucoseSuspendedTop.AutoSize = True
        Me.TableLayoutPanelLowGlucoseSuspendedTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLowGlucoseSuspendedTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ColumnCount = 2
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelLowGlucoseSuspendedTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelLowGlucoseSuspendedTop.LabelText = "Low Glucose Suspended"
        Me.TableLayoutPanelLowGlucoseSuspendedTop.Location = New Point(6, 6)
        Me.TableLayoutPanelLowGlucoseSuspendedTop.Name = "TableLayoutPanelLowGlucoseSuspendedTop"
        Me.TableLayoutPanelLowGlucoseSuspendedTop.RowCount = 1
        Me.TableLayoutPanelLowGlucoseSuspendedTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelLowGlucoseSuspendedTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelLowGlucoseSuspendedTop.TabIndex = 1
        '
        ' TabPageTimeChange
        '
        Me.TabPageTimeChange.Controls.Add(Me.TableLayoutPanelTimeChange)
        Me.TabPageTimeChange.Location = New Point(4, 27)
        Me.TabPageTimeChange.Name = "TabPageTimeChange"
        Me.TabPageTimeChange.Padding = New Padding(3)
        Me.TabPageTimeChange.Size = New Size(1376, 637)
        Me.TabPageTimeChange.TabIndex = 7
        Me.TabPageTimeChange.Text = "Time Change"
        Me.TabPageTimeChange.UseVisualStyleBackColor = True
        '
        ' TableLayoutPanelTimeChange
        '
        Me.TableLayoutPanelTimeChange.AutoScroll = True
        Me.TableLayoutPanelTimeChange.AutoSize = True
        Me.TableLayoutPanelTimeChange.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTimeChange.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTimeChange.ColumnCount = 1
        Me.TableLayoutPanelTimeChange.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelTimeChange.Controls.Add(Me.TableLayoutPanelTimeChangeTop, 0, 0)
        Me.TableLayoutPanelTimeChange.Dock = DockStyle.Top
        Me.TableLayoutPanelTimeChange.Location = New Point(3, 3)
        Me.TableLayoutPanelTimeChange.Name = "TableLayoutPanelTimeChange"
        Me.TableLayoutPanelTimeChange.RowCount = 2
        Me.TableLayoutPanelTimeChange.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelTimeChange.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelTimeChange.Size = New Size(1370, 52)
        Me.TableLayoutPanelTimeChange.TabIndex = 1
        '
        ' TableLayoutPanelTimeChangeTop
        '
        Me.TableLayoutPanelTimeChangeTop.AutoSize = True
        Me.TableLayoutPanelTimeChangeTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTimeChangeTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelTimeChangeTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTimeChangeTop.ColumnCount = 2
        Me.TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Me.TableLayoutPanelTimeChangeTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelTimeChangeTop.LabelText = "Time Change"
        Me.TableLayoutPanelTimeChangeTop.Location = New Point(6, 6)
        Me.TableLayoutPanelTimeChangeTop.Name = "TableLayoutPanelTimeChangeTop"
        Me.TableLayoutPanelTimeChangeTop.RowCount = 1
        Me.TableLayoutPanelTimeChangeTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelTimeChangeTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelTimeChangeTop.TabIndex = 1
        '
        ' TabPageCountryDataPg1
        '
        Me.TabPageCountryDataPg1.Controls.Add(Me.DgvCountryDataPg1)
        Me.TabPageCountryDataPg1.Location = New Point(4, 27)
        Me.TabPageCountryDataPg1.Name = "TabPageCountryDataPg1"
        Me.TabPageCountryDataPg1.Padding = New Padding(3)
        Me.TabPageCountryDataPg1.Size = New Size(1376, 637)
        Me.TabPageCountryDataPg1.TabIndex = 11
        Me.TabPageCountryDataPg1.Text = "Country Data Pg1"
        Me.TabPageCountryDataPg1.UseVisualStyleBackColor = True
        '
        ' TabPageCountryDataPg2
        '
        Me.TabPageCountryDataPg2.Controls.Add(Me.CountryDataPg2TableLayoutPanel)
        Me.TabPageCountryDataPg2.Location = New Point(4, 27)
        Me.TabPageCountryDataPg2.Name = "TabPageCountryDataPg2"
        Me.TabPageCountryDataPg2.Padding = New Padding(3)
        Me.TabPageCountryDataPg2.Size = New Size(1376, 637)
        Me.TabPageCountryDataPg2.TabIndex = 11
        Me.TabPageCountryDataPg2.Text = "Country Data Pg2"
        Me.TabPageCountryDataPg2.UseVisualStyleBackColor = True
        '
        ' TabPageCountryDataPg3
        '
        Me.TabPageCountryDataPg3.Controls.Add(Me.DgvCountryDataPg3)
        Me.TabPageCountryDataPg3.Location = New Point(4, 27)
        Me.TabPageCountryDataPg3.Name = "TabPageCountryDataPg3"
        Me.TabPageCountryDataPg3.Padding = New Padding(3)
        Me.TabPageCountryDataPg3.Size = New Size(1376, 637)
        Me.TabPageCountryDataPg3.TabIndex = 11
        Me.TabPageCountryDataPg3.Text = "Country Data Pg3"
        Me.TabPageCountryDataPg3.UseVisualStyleBackColor = True
        '
        ' TabPageUserProfile
        '
        Me.TabPageUserProfile.Controls.Add(Me.DgvSessionProfile)
        Me.TabPageUserProfile.Location = New Point(4, 27)
        Me.TabPageUserProfile.Name = "TabPageUserProfile"
        Me.TabPageUserProfile.Padding = New Padding(3)
        Me.TabPageUserProfile.Size = New Size(1376, 637)
        Me.TabPageUserProfile.TabIndex = 12
        Me.TabPageUserProfile.Text = "User Profile"
        Me.TabPageUserProfile.UseVisualStyleBackColor = True
        '
        ' TabPageCurrentUser
        '
        Me.TabPageCurrentUser.Controls.Add(Me.DgvCurrentUser)
        Me.TabPageCurrentUser.Location = New Point(4, 27)
        Me.TabPageCurrentUser.Name = "TabPageCurrentUser"
        Me.TabPageCurrentUser.Size = New Size(1376, 637)
        Me.TabPageCurrentUser.TabIndex = 13
        Me.TabPageCurrentUser.Text = "Current User"
        Me.TabPageCurrentUser.UseVisualStyleBackColor = True
        '
        ' TabPageAllUsers
        '
        Me.TabPageAllUsers.Controls.Add(Me.DgvCareLinkUsers)
        Me.TabPageAllUsers.Location = New Point(4, 27)
        Me.TabPageAllUsers.Name = "TabPageAllUsers"
        Me.TabPageAllUsers.Padding = New Padding(3)
        Me.TabPageAllUsers.Size = New Size(1376, 637)
        Me.TabPageAllUsers.TabIndex = 14
        Me.TabPageAllUsers.Text = "All Users"
        Me.TabPageAllUsers.UseVisualStyleBackColor = True
        '
        ' TabPageBackToHomePage
        '
        Me.TabPageBackToHomePage.BackColor = SystemColors.MenuHighlight
        Me.TabPageBackToHomePage.Location = New Point(4, 27)
        Me.TabPageBackToHomePage.Name = "TabPageBackToHomePage"
        Me.TabPageBackToHomePage.Padding = New Padding(3)
        Me.TabPageBackToHomePage.Size = New Size(1376, 637)
        Me.TabPageBackToHomePage.TabIndex = 8
        Me.TabPageBackToHomePage.Text = "Back.."
        '
        ' StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New ToolStripItem() {Me.LoginStatus, Me.LastUpdateTime, Me.ToolStripSpacer, Me.TimeZoneLabel})
        Me.StatusStrip1.Location = New Point(0, 692)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New Size(1384, 24)
        Me.StatusStrip1.TabIndex = 53
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        ' LoginStatus
        '
        Me.LoginStatus.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Right
        Me.LoginStatus.BorderStyle = Border3DStyle.RaisedOuter
        Me.LoginStatus.DisplayStyle = ToolStripItemDisplayStyle.Text
        Me.LoginStatus.Name = "LoginStatus"
        Me.LoginStatus.Size = New Size(133, 19)
        Me.LoginStatus.Text = "Login Status: Unknown"
        '
        ' LastUpdateTime
        '
        Me.LastUpdateTime.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Right
        Me.LastUpdateTime.BorderStyle = Border3DStyle.RaisedOuter
        Me.LastUpdateTime.Name = "LastUpdateTime"
        Me.LastUpdateTime.Size = New Size(1176, 19)
        Me.LastUpdateTime.Spring = True
        Me.LastUpdateTime.Text = "                           Last Update Time: Unknown"
        '
        ' ToolStripSpacer
        '
        Me.ToolStripSpacer.Name = "ToolStripSpacer"
        Me.ToolStripSpacer.Size = New Size(0, 19)
        '
        ' TimeZoneLabel
        '
        Me.TimeZoneLabel.Name = "TimeZoneLabel"
        Me.TimeZoneLabel.Size = New Size(60, 19)
        Me.TimeZoneLabel.Text = "TImeZone"
        '
        ' CountryDataPg2TableLayoutPanel
        '
        Me.CountryDataPg2TableLayoutPanel.ColumnCount = 2
        Me.CountryDataPg2TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 43.2116776F))
        Me.CountryDataPg2TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 56.7883224F))
        Me.CountryDataPg2TableLayoutPanel.Controls.Add(Me.DgvCountryDataPg2, 0, 0)
        Me.CountryDataPg2TableLayoutPanel.Controls.Add(Me.WebView, 1, 0)
        Me.CountryDataPg2TableLayoutPanel.Dock = DockStyle.Fill
        Me.CountryDataPg2TableLayoutPanel.Location = New Point(3, 3)
        Me.CountryDataPg2TableLayoutPanel.Name = "CountryDataPg2TableLayoutPanel"
        Me.CountryDataPg2TableLayoutPanel.RowCount = 1
        Me.CountryDataPg2TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        Me.CountryDataPg2TableLayoutPanel.Size = New Size(1370, 631)
        Me.CountryDataPg2TableLayoutPanel.TabIndex = 2
        '
        ' DgvCountryDataPg2
        '
        Me.DgvCountryDataPg2.Columns.AddRange(New DataGridViewColumn() {Me.DgvCountryDataPg2RecordNumber, Me.DgvCountryDataPg2Category, Me.DgvCountryDataPg2Key, Me.DgvCountryDataPg2Value})
        Me.DgvCountryDataPg2.Location = New Point(3, 3)
        Me.DgvCountryDataPg2.Name = "DgvCountryDataPg2"
        Me.DgvCountryDataPg2.ReadOnly = True
        Me.DgvCountryDataPg2.RowTemplate.Height = 25
        Me.DgvCountryDataPg2.Size = New Size(583, 612)
        Me.DgvCountryDataPg2.TabIndex = 2
        '
        ' DgvCountryDataPg2RecordNumber
        '
        Me.DgvCountryDataPg2RecordNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DgvCountryDataPg2RecordNumber.HeaderText = "Record Number"
        Me.DgvCountryDataPg2RecordNumber.MinimumWidth = 60
        Me.DgvCountryDataPg2RecordNumber.Name = "DgvCountryDataPg2RecordNumber"
        Me.DgvCountryDataPg2RecordNumber.ReadOnly = True
        Me.DgvCountryDataPg2RecordNumber.Width = 60
        '
        ' DgvCountryDataPg2Category
        '
        Me.DgvCountryDataPg2Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg2Category.HeaderText = "Category"
        Me.DgvCountryDataPg2Category.Name = "DgvCountryDataPg2Category"
        Me.DgvCountryDataPg2Category.ReadOnly = True
        Me.DgvCountryDataPg2Category.Width = 80
        '
        ' DgvCountryDataPg2Key
        '
        Me.DgvCountryDataPg2Key.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.DgvCountryDataPg2Key.HeaderText = "Key"
        Me.DgvCountryDataPg2Key.Name = "DgvCountryDataPg2Key"
        Me.DgvCountryDataPg2Key.ReadOnly = True
        Me.DgvCountryDataPg2Key.Width = 51
        '
        ' DgvCountryDataPg2Value
        '
        Me.DgvCountryDataPg2Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Me.DgvCountryDataPg2Value.HeaderText = "Value"
        Me.DgvCountryDataPg2Value.Name = "DgvCountryDataPg2Value"
        Me.DgvCountryDataPg2Value.ReadOnly = True
        '
        ' WebView
        '
        Me.WebView.AllowExternalDrop = True
        Me.WebView.CreationProperties = Nothing
        Me.WebView.DefaultBackgroundColor = Color.White
        Me.WebView.Dock = DockStyle.Fill
        Me.WebView.Location = New Point(595, 3)
        Me.WebView.Name = "WebView"
        Me.WebView.Size = New Size(772, 625)
        Me.WebView.TabIndex = 3
        Me.WebView.ZoomFactor = 1.0R
        '
        ' Form1
        '
        Me.AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.ClientSize = New Size(1384, 716)
        Me.Controls.Add(Me.TabControlPage1)
        Me.Controls.Add(Me.TabControlPage2)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximumSize = New Size(1400, 960)
        Me.Name = "Form1"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "CareLink For Windows"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.CalibrationDueImage, ComponentModel.ISupportInitialize).EndInit()
        Me.CalibrationShieldPanel.ResumeLayout(False)
        Me.CalibrationShieldPanel.PerformLayout()
        CType(Me.CalibrationShieldPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).EndInit()
        Me.CursorPanel.ResumeLayout(False)
        CType(Me.CursorPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvCountryDataPg1, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvCareLinkUsers, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvCountryDataPg3, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvCurrentUser, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvInsulin, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvMeal, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvSGs, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvSummary, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvSessionProfile, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsulinLevelPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Me.Last24HTotalsPanel.ResumeLayout(False)
        CType(Me.PumpBatteryPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Me.SensorTimeLeftPanel.ResumeLayout(False)
        CType(Me.SensorTimeLeftPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.TransmitterBatteryPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.Panel2.PerformLayout()
        CType(Me.SplitContainer3, ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.TabControlPage1.ResumeLayout(False)
        Me.TabPage01HomePage.ResumeLayout(False)
        Me.TabPage02RunningIOB.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        CType(Me.SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TabPage04SummaryData.ResumeLayout(False)
        Me.TabPage05Insulin.ResumeLayout(False)
        Me.TabPage05Insulin.PerformLayout()
        Me.TableLayoutPanelInsulin.ResumeLayout(False)
        Me.TableLayoutPanelInsulin.PerformLayout()
        Me.TabPage06Meal.ResumeLayout(False)
        Me.TabPage06Meal.PerformLayout()
        Me.TableLayoutPanelMeal.ResumeLayout(False)
        Me.TableLayoutPanelMeal.PerformLayout()
        Me.TabPage07ActiveInsulin.ResumeLayout(False)
        Me.TabPage07ActiveInsulin.PerformLayout()
        Me.TableLayoutPanelActiveInsulin.ResumeLayout(False)
        Me.TableLayoutPanelActiveInsulin.PerformLayout()
        Me.TabPage08SensorGlucose.ResumeLayout(False)
        Me.TabPage08SensorGlucose.PerformLayout()
        Me.TableLayoutPanelSgs.ResumeLayout(False)
        Me.TableLayoutPanelSgs.PerformLayout()
        Me.TabPage09Limits.ResumeLayout(False)
        Me.TabPage09Limits.PerformLayout()
        Me.TableLayoutPanelLimits.ResumeLayout(False)
        Me.TableLayoutPanelLimits.PerformLayout()
        Me.TabPage10NotificationHistory.ResumeLayout(False)
        Me.TabPage10NotificationHistory.PerformLayout()
        Me.TableLayoutPanelNotificationHistory.ResumeLayout(False)
        Me.TableLayoutPanelNotificationHistory.PerformLayout()
        Me.TabPage11TherapyAlgorithm.ResumeLayout(False)
        Me.TabPage11TherapyAlgorithm.PerformLayout()
        Me.TableLayoutPanelTherapyAlgorithm.ResumeLayout(False)
        Me.TableLayoutPanelTherapyAlgorithm.PerformLayout()
        Me.TabPage12BannerState.ResumeLayout(False)
        Me.TabPage12BannerState.PerformLayout()
        Me.TableLayoutPanelBannerState.ResumeLayout(False)
        Me.TableLayoutPanelBannerState.PerformLayout()
        Me.TabPage13Basal.ResumeLayout(False)
        Me.TableLayoutPanelBasal.ResumeLayout(False)
        Me.TableLayoutPanelBasal.PerformLayout()
        Me.TabPageLastSG.ResumeLayout(False)
        Me.TabPageLastSG.PerformLayout()
        Me.TableLayoutPanelLastSG.ResumeLayout(False)
        Me.TableLayoutPanelLastSG.PerformLayout()
        Me.TabPageLastAlarm.ResumeLayout(False)
        Me.TabPageLastAlarm.PerformLayout()
        Me.TableLayoutPanelLastAlarm.ResumeLayout(False)
        Me.TableLayoutPanelLastAlarm.PerformLayout()
        Me.TabControlPage2.ResumeLayout(False)
        Me.TabPageAutoBasalDelivery.ResumeLayout(False)
        Me.TabPageAutoBasalDelivery.PerformLayout()
        Me.TableLayoutPanelAutoBasalDelivery.ResumeLayout(False)
        Me.TableLayoutPanelAutoBasalDelivery.PerformLayout()
        Me.TabPageAutoModeStatus.ResumeLayout(False)
        Me.TabPageAutoModeStatus.PerformLayout()
        Me.TableLayoutPanelAutoModeStatus.ResumeLayout(False)
        Me.TableLayoutPanelAutoModeStatus.PerformLayout()
        Me.TabPageBgReadings.ResumeLayout(False)
        Me.TabPageBgReadings.PerformLayout()
        Me.TableLayoutPanelBgReadings.ResumeLayout(False)
        Me.TableLayoutPanelBgReadings.PerformLayout()
        Me.TabPageCalibration.ResumeLayout(False)
        Me.TabPageCalibration.PerformLayout()
        Me.TableLayoutPanelCalibration.ResumeLayout(False)
        Me.TableLayoutPanelCalibration.PerformLayout()
        Me.TabPageLowGlucoseSuspended.ResumeLayout(False)
        Me.TabPageLowGlucoseSuspended.PerformLayout()
        Me.TableLayoutPanelLowGlucoseSuspended.ResumeLayout(False)
        Me.TableLayoutPanelLowGlucoseSuspended.PerformLayout()
        Me.TabPageTimeChange.ResumeLayout(False)
        Me.TabPageTimeChange.PerformLayout()
        Me.TableLayoutPanelTimeChange.ResumeLayout(False)
        Me.TableLayoutPanelTimeChange.PerformLayout()
        Me.TabPageCountryDataPg1.ResumeLayout(False)
        Me.TabPageCountryDataPg2.ResumeLayout(False)
        Me.TabPageCountryDataPg3.ResumeLayout(False)
        Me.TabPageUserProfile.ResumeLayout(False)
        Me.TabPageCurrentUser.ResumeLayout(False)
        Me.TabPageAllUsers.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.CountryDataPg2TableLayoutPanel.ResumeLayout(False)
        CType(Me.DgvCountryDataPg2, ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents AboveHighLimitMessageLabel As Label
    Friend WithEvents AboveHighLimitValueLabel As Label
    Friend WithEvents ActiveInsulinValue As Label
    Friend WithEvents AverageSGMessageLabel As Label
    Friend WithEvents AverageSGValueLabel As Label
    Friend WithEvents BannerStateButton As Button
    Friend WithEvents BannerStateLabel As Label
    Friend WithEvents BasalButton As Button
    Friend WithEvents BasalLabel As Label
    Friend WithEvents BelowLowLimitMessageLabel As Label
    Friend WithEvents BelowLowLimitValueLabel As Label
    Friend WithEvents CalibrationDueImage As PictureBox
    Friend WithEvents CalibrationShieldPanel As Panel
    Friend WithEvents CalibrationShieldPictureBox As PictureBox
    Friend WithEvents CareLinkUserDataRecordBindingSource As BindingSource
    Friend WithEvents CurrentBGLabel As Label
    Friend WithEvents CursorMessage1Label As Label
    Friend WithEvents CursorMessage2Label As Label
    Friend WithEvents CursorMessage3Label As Label
    Friend WithEvents CursorPanel As Panel
    Friend WithEvents CursorPictureBox As PictureBox
    Friend WithEvents CursorTimer As Timer
    Friend WithEvents DgvAutoBasalDelivery As DataGridView
    Friend WithEvents DgvCareLinkUsers As DataGridView
    Friend WithEvents DgvCareLinkUsersOutGoingMailServer As DataGridViewTextBoxColumn
    Friend WithEvents DgvCareLinkUsersUserPassword As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1 As DataGridView
    Friend WithEvents DgvCountryDataPg1Category As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1Key As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1RecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1Value As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg3 As DataGridView
    Friend WithEvents DgvCountryDataPg3Category As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg3Key As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg3NotFor As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg3OnlyFor As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg3RecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg3Value As DataGridViewTextBoxColumn
    Friend WithEvents DgvCurrentUser As DataGridView
    Friend WithEvents DgvInsulin As DataGridView
    Friend WithEvents DgvMeal As DataGridView
    Friend WithEvents DgvSGs As DataGridView
    Friend WithEvents DgvSummary As DataGridView
    Friend WithEvents DgvSessionProfile As DataGridView
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents InRangeMessageLabel As Label
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents LabelSgTrend As Label
    Friend WithEvents LabelTimeChange As Label
    Friend WithEvents LabelTrendArrows As Label
    Friend WithEvents LabelTrendValue As Label
    Friend WithEvents Last24AutoCorrectionLabel As Label
    Friend WithEvents Last24CarbsValueLabel As Label
    Friend WithEvents Last24DailyDoseLabel As Label
    Friend WithEvents Last24HourBasalLabel As Label
    Friend WithEvents Last24HoursLabel As Label
    Friend WithEvents Last24HTotalsPanel As Panel
    Friend WithEvents Last24ManualBolusLabel As Label
    Friend WithEvents Last24TotalsLabel As Label
    Friend WithEvents LastSGTimeLabel As Label
    Friend WithEvents ListView1 As ListView
    Friend WithEvents MaxBasalPerHourLabel As Label
    Friend WithEvents MenuHelp As ToolStripMenuItem
    Friend WithEvents MenuHelpAbout As ToolStripMenuItem
    Friend WithEvents MenuHelpCheckForUpdates As ToolStripMenuItem
    Friend WithEvents MenuHelpReportAnIssue As ToolStripMenuItem
    Friend WithEvents MenuOptions As ToolStripMenuItem
    Friend WithEvents MenuOptionsAutoLogin As ToolStripMenuItem
    Friend WithEvents MenuOptionsColorPicker As ToolStripMenuItem
    Friend WithEvents MenuOptionsFilterRawJSONData As ToolStripMenuItem
    Friend WithEvents MenuOptionsShowLegend As ToolStripMenuItem
    Friend WithEvents MenuOptionsUseLocalTimeZone As ToolStripMenuItem
    Friend WithEvents MenuStartHere As ToolStripMenuItem
    Friend WithEvents MenuStartHereExceptionReportLoad As ToolStripMenuItem
    Friend WithEvents MenuStartHereLoadSavedDataFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereLogin As ToolStripMenuItem
    Friend WithEvents MenuStartHereSnapshotSave As ToolStripMenuItem
    Friend WithEvents MenuStartHereUseLastSavedFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereUseTestData As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ModelLabel As Label
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents OptionsMenuAdvancedOptions As ToolStripMenuItem
    Friend WithEvents PumpBatteryPictureBox As PictureBox
    Friend WithEvents PumpBatteryRemainingLabel As Label
    Friend WithEvents InsulinTypeLabel As Label
    Friend WithEvents ReadingsLabel As Label
    Friend WithEvents RemainingInsulinUnits As Label
    Friend WithEvents SensorDaysLeftLabel As Label
    Friend WithEvents SensorMessage As Label
    Friend WithEvents SensorTimeLeftLabel As Label
    Friend WithEvents SensorTimeLeftPanel As Panel
    Friend WithEvents SensorTimeLeftPictureBox As PictureBox
    Friend WithEvents SerialNumberLabel As Label
    Friend WithEvents ServerUpdateTimer As Timer
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents MenuShowMiniDisplay As ToolStripMenuItem
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents StartHereExit As ToolStripMenuItem
    Friend WithEvents TabControlPage1 As TabControl
    Friend WithEvents TabControlPage2 As TabControl
    Friend WithEvents TableLayoutPanelActiveInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelActiveInsulinTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelAutoBasalDelivery As TableLayoutPanel
    Friend WithEvents TableLayoutPanelAutoBasalDeliveryTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelAutoModeStatus As TableLayoutPanel
    Friend WithEvents TableLayoutPanelAutoModeStatusTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelBannerState As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBannerStateTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelBasal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBasalTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelBgReadings As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBgReadingsTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelCalibration As TableLayoutPanel
    Friend WithEvents TableLayoutPanelCalibrationTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelInsulinTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLastAlarm As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLastAlarmTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLastSG As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLastSgTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLimits As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLimitsTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLowGlucoseSuspended As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLowGlucoseSuspendedTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelMeal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelMealTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelNotificationHistory As TableLayoutPanel
    Friend WithEvents TableLayoutPanelNotificationHistoryTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelSgs As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSgsTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelTherapyAlgorithm As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTherapyAlgorithmTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelTimeChange As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTimeChangeTop As TableLayputPanelTop.TableLayoutPanelTopEx
    Friend WithEvents TabPage01HomePage As TabPage
    Friend WithEvents TabPage02RunningIOB As TabPage
    Friend WithEvents TabPage03TreatmentDetails As TabPage
    Friend WithEvents TabPage04SummaryData As TabPage
    Friend WithEvents TabPageLastSG As TabPage
    Friend WithEvents TabPageLastAlarm As TabPage
    Friend WithEvents TabPage07ActiveInsulin As TabPage
    Friend WithEvents TabPage08SensorGlucose As TabPage
    Friend WithEvents TabPage09Limits As TabPage
    Friend WithEvents TabPage10NotificationHistory As TabPage
    Friend WithEvents TabPage11TherapyAlgorithm As TabPage
    Friend WithEvents TabPage12BannerState As TabPage
    Friend WithEvents TabPage13Basal As TabPage
    Friend WithEvents TabPage14Markers As TabPage
    Friend WithEvents TabPageAllUsers As TabPage
    Friend WithEvents TabPageAutoBasalDelivery As TabPage
    Friend WithEvents TabPageAutoModeStatus As TabPage
    Friend WithEvents TabPageBackToHomePage As TabPage
    Friend WithEvents TabPageBgReadings As TabPage
    Friend WithEvents TabPageCalibration As TabPage
    Friend WithEvents TabPageCountryDataPg1 As TabPage
    Friend WithEvents TabPageCountryDataPg2 As TabPage
    Friend WithEvents TabPageCountryDataPg3 As TabPage
    Friend WithEvents TabPageCurrentUser As TabPage
    Friend WithEvents TabPage05Insulin As TabPage
    Friend WithEvents TabPageLowGlucoseSuspended As TabPage
    Friend WithEvents TabPage06Meal As TabPage
    Friend WithEvents TabPageTimeChange As TabPage
    Friend WithEvents TabPageUserProfile As TabPage
    Friend WithEvents TempTargetLabel As Label
    Friend WithEvents TimeInRangeChartLabel As Label
    Friend WithEvents TimeInRangeLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TransmitterBatteryPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents PumpBatteryRemaining2Label As Label
    Friend WithEvents PumpNameLabel As Label
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents MenuOptionsEditPumpSettings As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents LoginStatus As ToolStripStatusLabel
    Friend WithEvents ToolStripSplitButton1 As ToolStripSplitButton
    Friend WithEvents LastUpdateTime As ToolStripStatusLabel
    Friend WithEvents ToolStripSplitButton2 As ToolStripSplitButton
    Friend WithEvents ToolStripSpacer As ToolStripStatusLabel
    Friend WithEvents PumpAITLabel As Label
    Friend WithEvents FullNameLabel As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TemporaryUseAdvanceAITDecayCheckBox As CheckBox
    Friend WithEvents TimeZoneLabel As ToolStripStatusLabel
    Friend WithEvents SmartGuardLabel As Label
    Friend WithEvents CountryDataPg2TableLayoutPanel As TableLayoutPanel
    Friend WithEvents DgvCountryDataPg2 As DataGridView
    Friend WithEvents DgvCountryDataPg2RecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2Category As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2Key As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2Value As DataGridViewTextBoxColumn
    Friend WithEvents WebView As Microsoft.Web.WebView2.WinForms.WebView2
End Class
