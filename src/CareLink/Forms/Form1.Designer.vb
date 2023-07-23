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
        MenuStrip1 = New MenuStrip()
        MenuStartHere = New ToolStripMenuItem()
        MenuStartHereLogin = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        MenuStartHereLoadSavedDataFile = New ToolStripMenuItem()
        MenuStartHereExceptionReportLoad = New ToolStripMenuItem()
        ToolStripSeparator4 = New ToolStripSeparator()
        MenuStartHereUseLastSavedFile = New ToolStripMenuItem()
        MenuStartHereUseTestData = New ToolStripMenuItem()
        ToolStripSeparator2 = New ToolStripSeparator()
        MenuStartHereSnapshotSave = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        StartHereExit = New ToolStripMenuItem()
        MenuOptions = New ToolStripMenuItem()
        MenuOptionsAudioAlerts = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionEnabled = New ToolStripMenuItem()
        MenuOptionsShowChartLegends = New ToolStripMenuItem()
        ToolStripSeparator5 = New ToolStripSeparator()
        MenuOptionsAutoLogin = New ToolStripMenuItem()
        ToolStripSeparator6 = New ToolStripSeparator()
        OptionsMenuAdvancedOptions = New ToolStripMenuItem()
        MenuOptionsFilterRawJSONData = New ToolStripMenuItem()
        MenuOptionsUseLocalTimeZone = New ToolStripMenuItem()
        ToolStripSeparator7 = New ToolStripSeparator()
        MenuOptionsColorPicker = New ToolStripMenuItem()
        MenuOptionsEditPumpSettings = New ToolStripMenuItem()
        MenuHelp = New ToolStripMenuItem()
        MenuHelpReportAnIssue = New ToolStripMenuItem()
        MenuHelpCheckForUpdates = New ToolStripMenuItem()
        MenuHelpAbout = New ToolStripMenuItem()
        MenuShowMiniDisplay = New ToolStripMenuItem()
        AboveHighLimitMessageLabel = New Label()
        AboveHighLimitValueLabel = New Label()
        ActiveInsulinValue = New Label()
        FullNameLabel = New Label()
        AverageSGMessageLabel = New Label()
        AverageSGValueLabel = New Label()
        BannerStateButton = New Button()
        BannerStateLabel = New Label()
        BasalButton = New Button()
        BasalLabel = New Label()
        BelowLowLimitMessageLabel = New Label()
        BelowLowLimitValueLabel = New Label()
        CalibrationDueImage = New PictureBox()
        CalibrationShieldPanel = New Panel()
        TempTargetLabel = New Label()
        ShieldUnitsLabel = New Label()
        LastSGTimeLabel = New Label()
        CurrentSgLabel = New Label()
        SensorMessage = New Label()
        CalibrationShieldPictureBox = New PictureBox()
        CareLinkUserDataRecordBindingSource = New BindingSource(components)
        CursorMessage1Label = New Label()
        CursorMessage2Label = New Label()
        CursorMessage3Label = New Label()
        CursorMessage4Label = New Label()
        CursorPanel = New Panel()
        CursorPictureBox = New PictureBox()
        CursorTimer = New Timer(components)
        DgvAutoBasalDelivery = New DataGridView()
        DgvCountryDataPg1 = New DataGridView()
        DgvCountryDataPg1RecordNumber = New DataGridViewTextBoxColumn()
        DgvCountryDataPg1Category = New DataGridViewTextBoxColumn()
        DgvCountryDataPg1Key = New DataGridViewTextBoxColumn()
        DgvCountryDataPg1Value = New DataGridViewTextBoxColumn()
        DgvCareLinkUsers = New DataGridView()
        DgvCountryDataPg3 = New DataGridView()
        DgvCountryDataPg3RecordNumber = New DataGridViewTextBoxColumn()
        DgvCountryDataPg3Category = New DataGridViewTextBoxColumn()
        DgvCountryDataPg3Key = New DataGridViewTextBoxColumn()
        DgvCountryDataPg3Value = New DataGridViewTextBoxColumn()
        DgvCountryDataPg3OnlyFor = New DataGridViewTextBoxColumn()
        DgvCountryDataPg3NotFor = New DataGridViewTextBoxColumn()
        DgvCurrentUser = New DataGridView()
        DgvInsulin = New DataGridView()
        DgvMeal = New DataGridView()
        DgvSGs = New DataGridView()
        DgvSummary = New DataGridView()
        DgvSessionProfile = New DataGridView()
        ImageList1 = New ImageList(components)
        InRangeMessageLabel = New Label()
        InsulinLevelPictureBox = New PictureBox()
        LabelSgTrend = New Label()
        LabelTimeChange = New Label()
        LabelTrendArrows = New Label()
        LabelTrendValue = New Label()
        Last24AutoCorrectionLabel = New Label()
        Last24CarbsValueLabel = New Label()
        Last24DailyDoseLabel = New Label()
        Last24BasalLabel = New Label()
        Last24HoursGraphLabel = New Label()
        Last24HTotalsPanel = New Panel()
        Last24ManualBolusLabel = New Label()
        Last24TotalsLabel = New Label()
        MaxBasalPerHourLabel = New Label()
        ModelLabel = New Label()
        PumpNameLabel = New Label()
        NotifyIcon1 = New NotifyIcon(components)
        PumpBatteryPictureBox = New PictureBox()
        PumpBatteryRemainingLabel = New Label()
        InsulinTypeLabel = New Label()
        ReadingsLabel = New Label()
        RemainingInsulinUnits = New Label()
        SensorDaysLeftLabel = New Label()
        SensorTimeLeftLabel = New Label()
        SensorTimeLeftPanel = New Panel()
        SensorTimeLeftPictureBox = New PictureBox()
        SerialNumberButton = New Button()
        ServerUpdateTimer = New Timer(components)
        SplitContainer2 = New SplitContainer()
        PumpAITLabel = New Label()
        PumpBatteryRemaining2Label = New Label()
        TransmitterBatteryPercentLabel = New Label()
        TransmitterBatteryPictureBox = New PictureBox()
        SplitContainer3 = New SplitContainer()
        TimeInRangeLabel = New Label()
        TimeInRangeChartLabel = New Label()
        TimeInRangeSummaryPercentCharLabel = New Label()
        TimeInRangeValueLabel = New Label()
        TirComplianceLabel = New Label()
        HighTirComplianceLabel = New Label()
        LowTirComplianceLabel = New Label()
        SmartGuardLabel = New Label()
        TabControlPage1 = New TabControl()
        TabPage01HomePage = New TabPage()
        TabPage02RunningIOB = New TabPage()
        SplitContainer1 = New SplitContainer()
        TemporaryUseAdvanceAITDecayCheckBox = New CheckBox()
        TabPage03TreatmentDetails = New TabPage()
        TabPage04SummaryData = New TabPage()
        TabPage05Insulin = New TabPage()
        TableLayoutPanelInsulin = New TableLayoutPanel()
        Me.TableLayoutPanelInsulinTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage06Meal = New TabPage()
        TableLayoutPanelMeal = New TableLayoutPanel()
        Me.TableLayoutPanelMealTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage07ActiveInsulin = New TabPage()
        TableLayoutPanelActiveInsulin = New TableLayoutPanel()
        Me.TableLayoutPanelActiveInsulinTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage08SensorGlucose = New TabPage()
        TableLayoutPanelSgs = New TableLayoutPanel()
        Me.TableLayoutPanelSgsTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage09Limits = New TabPage()
        TableLayoutPanelLimits = New TableLayoutPanel()
        Me.TableLayoutPanelLimitsTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage10NotificationHistory = New TabPage()
        TableLayoutPanelNotificationHistory = New TableLayoutPanel()
        Me.TableLayoutPanelNotificationHistoryTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage11TherapyAlgorithm = New TabPage()
        TableLayoutPanelTherapyAlgorithm = New TableLayoutPanel()
        Me.TableLayoutPanelTherapyAlgorithmTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage12BannerState = New TabPage()
        TableLayoutPanelBannerState = New TableLayoutPanel()
        Me.TableLayoutPanelBannerStateTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage13Basal = New TabPage()
        TableLayoutPanelBasal = New TableLayoutPanel()
        Me.TableLayoutPanelBasalTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPage14Markers = New TabPage()
        TabPageLastSG = New TabPage()
        TableLayoutPanelLastSG = New TableLayoutPanel()
        Me.TableLayoutPanelLastSgTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageLastAlarm = New TabPage()
        TableLayoutPanelLastAlarm = New TableLayoutPanel()
        Me.TableLayoutPanelLastAlarmTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabControlPage2 = New TabControl()
        TabPageAutoBasalDelivery = New TabPage()
        TableLayoutPanelAutoBasalDelivery = New TableLayoutPanel()
        Me.TableLayoutPanelAutoBasalDeliveryTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageAutoModeStatus = New TabPage()
        TableLayoutPanelAutoModeStatus = New TableLayoutPanel()
        Me.TableLayoutPanelAutoModeStatusTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageSgReadings = New TabPage()
        TableLayoutPanelSgReadings = New TableLayoutPanel()
        Me.TableLayoutPanelSgReadingsTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageCalibration = New TabPage()
        TableLayoutPanelCalibration = New TableLayoutPanel()
        Me.TableLayoutPanelCalibrationTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageLowGlucoseSuspended = New TabPage()
        TableLayoutPanelLowGlucoseSuspended = New TableLayoutPanel()
        Me.TableLayoutPanelLowGlucoseSuspendedTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageTimeChange = New TabPage()
        TableLayoutPanelTimeChange = New TableLayoutPanel()
        Me.TableLayoutPanelTimeChangeTop = New TableLayputPanelTop.TableLayoutPanelTopEx()
        TabPageCountryDataPg1 = New TabPage()
        TabPageCountryDataPg2 = New TabPage()
        CountryDataPg2TableLayoutPanel = New TableLayoutPanel()
        DgvCountryDataPg2 = New DataGridView()
        DgvCountryDataPg2RecordNumber = New DataGridViewTextBoxColumn()
        DgvCountryDataPg2Category = New DataGridViewTextBoxColumn()
        DgvCountryDataPg2Key = New DataGridViewTextBoxColumn()
        DgvCountryDataPg2Value = New DataGridViewTextBoxColumn()
        Me.WebView = New Microsoft.Web.WebView2.WinForms.WebView2()
        TabPageCountryDataPg3 = New TabPage()
        TabPageSessionProfile = New TabPage()
        TabPageCurrentUser = New TabPage()
        TabPageAllUsers = New TabPage()
        TabPageBackToHomePage = New TabPage()
        ToolTip1 = New ToolTip(components)
        StatusStrip1 = New StatusStrip()
        LoginStatus = New ToolStripStatusLabel()
        StatusStripSpeech = New ToolStripStatusLabel()
        LastUpdateTimeToolStripStatusLabel = New ToolStripStatusLabel()
        TimeZoneToolStripStatusLabel = New ToolStripStatusLabel()
        StatusStripSpacerRight = New ToolStripStatusLabel()
        UpdateAvailableStatusStripLabel = New ToolStripStatusLabel()
        MenuStrip1.SuspendLayout()
        CType(CalibrationDueImage, ComponentModel.ISupportInitialize).BeginInit()
        CalibrationShieldPanel.SuspendLayout()
        CType(CalibrationShieldPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).BeginInit()
        CursorPanel.SuspendLayout()
        CType(CursorPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCountryDataPg1, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCareLinkUsers, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCountryDataPg3, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCurrentUser, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvInsulin, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvMeal, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSGs, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSummary, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSessionProfile, ComponentModel.ISupportInitialize).BeginInit()
        CType(InsulinLevelPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Last24HTotalsPanel.SuspendLayout()
        CType(PumpBatteryPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SensorTimeLeftPanel.SuspendLayout()
        CType(SensorTimeLeftPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        CType(TransmitterBatteryPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer3, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer3.Panel2.SuspendLayout()
        SplitContainer3.SuspendLayout()
        TabControlPage1.SuspendLayout()
        TabPage01HomePage.SuspendLayout()
        TabPage02RunningIOB.SuspendLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.SuspendLayout()
        TabPage04SummaryData.SuspendLayout()
        TabPage05Insulin.SuspendLayout()
        TableLayoutPanelInsulin.SuspendLayout()
        TabPage06Meal.SuspendLayout()
        TableLayoutPanelMeal.SuspendLayout()
        TabPage07ActiveInsulin.SuspendLayout()
        TableLayoutPanelActiveInsulin.SuspendLayout()
        TabPage08SensorGlucose.SuspendLayout()
        TableLayoutPanelSgs.SuspendLayout()
        TabPage09Limits.SuspendLayout()
        TableLayoutPanelLimits.SuspendLayout()
        TabPage10NotificationHistory.SuspendLayout()
        TableLayoutPanelNotificationHistory.SuspendLayout()
        TabPage11TherapyAlgorithm.SuspendLayout()
        TableLayoutPanelTherapyAlgorithm.SuspendLayout()
        TabPage12BannerState.SuspendLayout()
        TableLayoutPanelBannerState.SuspendLayout()
        TabPage13Basal.SuspendLayout()
        TableLayoutPanelBasal.SuspendLayout()
        TabPageLastSG.SuspendLayout()
        TableLayoutPanelLastSG.SuspendLayout()
        TabPageLastAlarm.SuspendLayout()
        TableLayoutPanelLastAlarm.SuspendLayout()
        TabControlPage2.SuspendLayout()
        TabPageAutoBasalDelivery.SuspendLayout()
        TableLayoutPanelAutoBasalDelivery.SuspendLayout()
        TabPageAutoModeStatus.SuspendLayout()
        TableLayoutPanelAutoModeStatus.SuspendLayout()
        TabPageSgReadings.SuspendLayout()
        TableLayoutPanelSgReadings.SuspendLayout()
        TabPageCalibration.SuspendLayout()
        TableLayoutPanelCalibration.SuspendLayout()
        TabPageLowGlucoseSuspended.SuspendLayout()
        TableLayoutPanelLowGlucoseSuspended.SuspendLayout()
        TabPageTimeChange.SuspendLayout()
        TableLayoutPanelTimeChange.SuspendLayout()
        TabPageCountryDataPg1.SuspendLayout()
        TabPageCountryDataPg2.SuspendLayout()
        CountryDataPg2TableLayoutPanel.SuspendLayout()
        CType(DgvCountryDataPg2, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WebView, ComponentModel.ISupportInitialize).BeginInit()
        TabPageCountryDataPg3.SuspendLayout()
        TabPageSessionProfile.SuspendLayout()
        TabPageCurrentUser.SuspendLayout()
        TabPageAllUsers.SuspendLayout()
        StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {MenuStartHere, MenuOptions, MenuHelp, MenuShowMiniDisplay})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1384, 24)
        MenuStrip1.TabIndex = 0
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' MenuStartHere
        ' 
        MenuStartHere.DropDownItems.AddRange(New ToolStripItem() {MenuStartHereLogin, ToolStripSeparator1, MenuStartHereLoadSavedDataFile, MenuStartHereExceptionReportLoad, ToolStripSeparator4, MenuStartHereUseLastSavedFile, MenuStartHereUseTestData, ToolStripSeparator2, MenuStartHereSnapshotSave, ToolStripSeparator3, StartHereExit})
        MenuStartHere.Name = "MenuStartHere"
        MenuStartHere.Size = New Size(71, 20)
        MenuStartHere.Text = "Start Here"
        ' 
        ' MenuStartHereLogin
        ' 
        MenuStartHereLogin.Name = "MenuStartHereLogin"
        MenuStartHereLogin.Size = New Size(211, 22)
        MenuStartHereLogin.Text = "Login"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(208, 6)
        ' 
        ' MenuStartHereLoadSavedDataFile
        ' 
        MenuStartHereLoadSavedDataFile.Name = "MenuStartHereLoadSavedDataFile"
        MenuStartHereLoadSavedDataFile.Size = New Size(211, 22)
        MenuStartHereLoadSavedDataFile.Text = "Load A Saved Data File"
        ' 
        ' MenuStartHereExceptionReportLoad
        ' 
        MenuStartHereExceptionReportLoad.Name = "MenuStartHereExceptionReportLoad"
        MenuStartHereExceptionReportLoad.Size = New Size(211, 22)
        MenuStartHereExceptionReportLoad.Text = "Load An Exception Report"
        ' 
        ' ToolStripSeparator4
        ' 
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New Size(208, 6)
        ' 
        ' MenuStartHereUseLastSavedFile
        ' 
        MenuStartHereUseLastSavedFile.Name = "MenuStartHereUseLastSavedFile"
        MenuStartHereUseLastSavedFile.Size = New Size(211, 22)
        MenuStartHereUseLastSavedFile.Text = "Use Last Data File"
        ' 
        ' MenuStartHereUseTestData
        ' 
        MenuStartHereUseTestData.Name = "MenuStartHereUseTestData"
        MenuStartHereUseTestData.Size = New Size(211, 22)
        MenuStartHereUseTestData.Text = "Use Test Data"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(208, 6)
        ' 
        ' MenuStartHereSnapshotSave
        ' 
        MenuStartHereSnapshotSave.Name = "MenuStartHereSnapshotSave"
        MenuStartHereSnapshotSave.ShortcutKeys = Keys.Control Or Keys.S
        MenuStartHereSnapshotSave.Size = New Size(211, 22)
        MenuStartHereSnapshotSave.Text = "Snapshot &Save"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(208, 6)
        ' 
        ' StartHereExit
        ' 
        StartHereExit.Image = My.Resources.Resources.AboutBox
        StartHereExit.Name = "StartHereExit"
        StartHereExit.ShortcutKeys = Keys.Alt Or Keys.X
        StartHereExit.Size = New Size(211, 22)
        StartHereExit.Text = "E&xit"
        ' 
        ' MenuOptions
        ' 
        MenuOptions.DropDownItems.AddRange(New ToolStripItem() {MenuOptionsAudioAlerts, MenuOptionsSpeechRecognitionEnabled, MenuOptionsShowChartLegends, ToolStripSeparator5, MenuOptionsAutoLogin, ToolStripSeparator6, OptionsMenuAdvancedOptions, MenuOptionsFilterRawJSONData, MenuOptionsUseLocalTimeZone, ToolStripSeparator7, MenuOptionsColorPicker, MenuOptionsEditPumpSettings})
        MenuOptions.Name = "MenuOptions"
        MenuOptions.Size = New Size(61, 20)
        MenuOptions.Text = "Options"
        ' 
        ' MenuOptionsAudioAlerts
        ' 
        MenuOptionsAudioAlerts.Checked = True
        MenuOptionsAudioAlerts.CheckOnClick = True
        MenuOptionsAudioAlerts.CheckState = CheckState.Checked
        MenuOptionsAudioAlerts.Name = "MenuOptionsAudioAlerts"
        MenuOptionsAudioAlerts.Size = New Size(224, 22)
        MenuOptionsAudioAlerts.Text = "Audio Alerts Enabled"
        ' 
        ' MenuOptionsSpeechRecognitionEnabled
        ' 
        MenuOptionsSpeechRecognitionEnabled.Checked = True
        MenuOptionsSpeechRecognitionEnabled.CheckOnClick = True
        MenuOptionsSpeechRecognitionEnabled.CheckState = CheckState.Checked
        MenuOptionsSpeechRecognitionEnabled.Name = "MenuOptionsSpeechRecognitionEnabled"
        MenuOptionsSpeechRecognitionEnabled.Size = New Size(224, 22)
        MenuOptionsSpeechRecognitionEnabled.Text = "Speech Recognition Enabled"
        ' 
        ' MenuOptionsShowChartLegends
        ' 
        MenuOptionsShowChartLegends.Checked = True
        MenuOptionsShowChartLegends.CheckOnClick = True
        MenuOptionsShowChartLegends.CheckState = CheckState.Checked
        MenuOptionsShowChartLegends.Name = "MenuOptionsShowChartLegends"
        MenuOptionsShowChartLegends.Size = New Size(224, 22)
        MenuOptionsShowChartLegends.Text = "Show Chart Legends"
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(221, 6)
        ' 
        ' MenuOptionsAutoLogin
        ' 
        MenuOptionsAutoLogin.CheckOnClick = True
        MenuOptionsAutoLogin.Name = "MenuOptionsAutoLogin"
        MenuOptionsAutoLogin.Size = New Size(224, 22)
        MenuOptionsAutoLogin.Text = "Auto Login"
        ' 
        ' ToolStripSeparator6
        ' 
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New Size(221, 6)
        ' 
        ' OptionsMenuAdvancedOptions
        ' 
        OptionsMenuAdvancedOptions.Enabled = False
        OptionsMenuAdvancedOptions.Name = "OptionsMenuAdvancedOptions"
        OptionsMenuAdvancedOptions.Size = New Size(224, 22)
        OptionsMenuAdvancedOptions.Text = "Advanced Options"
        ' 
        ' MenuOptionsFilterRawJSONData
        ' 
        MenuOptionsFilterRawJSONData.Checked = True
        MenuOptionsFilterRawJSONData.CheckOnClick = True
        MenuOptionsFilterRawJSONData.CheckState = CheckState.Checked
        MenuOptionsFilterRawJSONData.Name = "MenuOptionsFilterRawJSONData"
        MenuOptionsFilterRawJSONData.Size = New Size(224, 22)
        MenuOptionsFilterRawJSONData.Text = "Filter Raw JSON Data"
        ' 
        ' MenuOptionsUseLocalTimeZone
        ' 
        MenuOptionsUseLocalTimeZone.Checked = True
        MenuOptionsUseLocalTimeZone.CheckOnClick = True
        MenuOptionsUseLocalTimeZone.CheckState = CheckState.Indeterminate
        MenuOptionsUseLocalTimeZone.Name = "MenuOptionsUseLocalTimeZone"
        MenuOptionsUseLocalTimeZone.Size = New Size(224, 22)
        MenuOptionsUseLocalTimeZone.Text = "Use Local TImeZone"
        ' 
        ' ToolStripSeparator7
        ' 
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New Size(221, 6)
        ' 
        ' MenuOptionsColorPicker
        ' 
        MenuOptionsColorPicker.Name = "MenuOptionsColorPicker"
        MenuOptionsColorPicker.Size = New Size(224, 22)
        MenuOptionsColorPicker.Text = "Color Picker..."
        ' 
        ' MenuOptionsEditPumpSettings
        ' 
        MenuOptionsEditPumpSettings.Name = "MenuOptionsEditPumpSettings"
        MenuOptionsEditPumpSettings.Size = New Size(224, 22)
        MenuOptionsEditPumpSettings.Text = "Edit Pump Settings..."
        ' 
        ' MenuHelp
        ' 
        MenuHelp.DropDownItems.AddRange(New ToolStripItem() {MenuHelpReportAnIssue, MenuHelpCheckForUpdates, MenuHelpAbout})
        MenuHelp.Name = "MenuHelp"
        MenuHelp.ShortcutKeys = Keys.Alt Or Keys.H
        MenuHelp.Size = New Size(44, 20)
        MenuHelp.Text = "&Help"
        ' 
        ' MenuHelpReportAnIssue
        ' 
        MenuHelpReportAnIssue.Image = My.Resources.Resources.FeedbackSmile_16x
        MenuHelpReportAnIssue.ImageScaling = ToolStripItemImageScaling.None
        MenuHelpReportAnIssue.Name = "MenuHelpReportAnIssue"
        MenuHelpReportAnIssue.Size = New Size(177, 22)
        MenuHelpReportAnIssue.Text = "Report A Problem..."
        ' 
        ' MenuHelpCheckForUpdates
        ' 
        MenuHelpCheckForUpdates.Name = "MenuHelpCheckForUpdates"
        MenuHelpCheckForUpdates.Size = New Size(177, 22)
        MenuHelpCheckForUpdates.Text = "Check For Updates"
        ' 
        ' MenuHelpAbout
        ' 
        MenuHelpAbout.Image = My.Resources.Resources.AboutBox
        MenuHelpAbout.Name = "MenuHelpAbout"
        MenuHelpAbout.Size = New Size(177, 22)
        MenuHelpAbout.Text = "&About..."
        ' 
        ' MenuShowMiniDisplay
        ' 
        MenuShowMiniDisplay.ForeColor = Color.Red
        MenuShowMiniDisplay.Image = My.Resources.Resources.ExitFullScreen
        MenuShowMiniDisplay.Name = "MenuShowMiniDisplay"
        MenuShowMiniDisplay.Padding = New Padding(10, 0, 10, 0)
        MenuShowMiniDisplay.ShortcutKeyDisplayString = "Alt+W"
        MenuShowMiniDisplay.ShortcutKeys = Keys.Control Or Keys.W
        MenuShowMiniDisplay.Size = New Size(154, 20)
        MenuShowMiniDisplay.Text = "Show &Widget Alt+W"
        MenuShowMiniDisplay.ToolTipText = "Minimize and show Widget"
        MenuShowMiniDisplay.Visible = False
        ' 
        ' AboveHighLimitMessageLabel
        ' 
        AboveHighLimitMessageLabel.Anchor = AnchorStyles.Top
        AboveHighLimitMessageLabel.BackColor = Color.Transparent
        AboveHighLimitMessageLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        AboveHighLimitMessageLabel.ForeColor = Color.Yellow
        AboveHighLimitMessageLabel.Location = New Point(30, 208)
        AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        AboveHighLimitMessageLabel.Size = New Size(170, 21)
        AboveHighLimitMessageLabel.TabIndex = 28
        AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        AboveHighLimitMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' AboveHighLimitValueLabel
        ' 
        AboveHighLimitValueLabel.Anchor = AnchorStyles.Top
        AboveHighLimitValueLabel.BackColor = Color.Black
        AboveHighLimitValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        AboveHighLimitValueLabel.ForeColor = Color.White
        AboveHighLimitValueLabel.Location = New Point(55, 175)
        AboveHighLimitValueLabel.Name = "AboveHighLimitValueLabel"
        AboveHighLimitValueLabel.Size = New Size(120, 33)
        AboveHighLimitValueLabel.TabIndex = 22
        AboveHighLimitValueLabel.Text = "8 %"
        AboveHighLimitValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' ActiveInsulinValue
        ' 
        ActiveInsulinValue.BackColor = Color.Transparent
        ActiveInsulinValue.BorderStyle = BorderStyle.FixedSingle
        ActiveInsulinValue.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        ActiveInsulinValue.ForeColor = Color.White
        ActiveInsulinValue.Location = New Point(995, 53)
        ActiveInsulinValue.Name = "ActiveInsulinValue"
        ActiveInsulinValue.Size = New Size(128, 48)
        ActiveInsulinValue.TabIndex = 0
        ActiveInsulinValue.Text = "Active Insulin 0.000U"
        ActiveInsulinValue.TextAlign = ContentAlignment.TopCenter
        ' 
        ' FullNameLabel
        ' 
        FullNameLabel.BackColor = Color.Transparent
        FullNameLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        FullNameLabel.ForeColor = Color.White
        FullNameLabel.Location = New Point(1140, 0)
        FullNameLabel.Margin = New Padding(0)
        FullNameLabel.Name = "FullNameLabel"
        FullNameLabel.Size = New Size(230, 21)
        FullNameLabel.TabIndex = 8
        FullNameLabel.Text = "User Name"
        FullNameLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' AverageSGMessageLabel
        ' 
        AverageSGMessageLabel.Anchor = AnchorStyles.Top
        AverageSGMessageLabel.BackColor = Color.Transparent
        AverageSGMessageLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        AverageSGMessageLabel.ForeColor = Color.White
        AverageSGMessageLabel.Location = New Point(3, 393)
        AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        AverageSGMessageLabel.Size = New Size(224, 21)
        AverageSGMessageLabel.TabIndex = 0
        AverageSGMessageLabel.Text = "Average SG in XX/XX"
        AverageSGMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' AverageSGValueLabel
        ' 
        AverageSGValueLabel.Anchor = AnchorStyles.Top
        AverageSGValueLabel.BackColor = Color.Black
        AverageSGValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        AverageSGValueLabel.ForeColor = Color.White
        AverageSGValueLabel.Location = New Point(55, 356)
        AverageSGValueLabel.Margin = New Padding(0)
        AverageSGValueLabel.Name = "AverageSGValueLabel"
        AverageSGValueLabel.Size = New Size(120, 33)
        AverageSGValueLabel.TabIndex = 1
        AverageSGValueLabel.Text = "100 %"
        AverageSGValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' BannerStateButton
        ' 
        BannerStateButton.AutoSize = True
        BannerStateButton.Location = New Point(6, 6)
        BannerStateButton.Name = "BannerStateButton"
        BannerStateButton.Size = New Size(142, 25)
        BannerStateButton.TabIndex = 0
        BannerStateButton.Text = "Return To 'Summary Data' Tab"
        ' 
        ' BannerStateLabel
        ' 
        BannerStateLabel.AutoSize = True
        BannerStateLabel.Dock = DockStyle.Fill
        BannerStateLabel.Location = New Point(157, 6)
        BannerStateLabel.Margin = New Padding(3)
        BannerStateLabel.Name = "BannerStateLabel"
        BannerStateLabel.Size = New Size(1201, 25)
        BannerStateLabel.TabIndex = 0
        BannerStateLabel.Text = "Banner State"
        BannerStateLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' BasalButton
        ' 
        BasalButton.AutoSize = True
        BasalButton.Location = New Point(6, 6)
        BasalButton.Name = "BasalButton"
        BasalButton.Size = New Size(142, 25)
        BasalButton.TabIndex = 0
        BasalButton.Text = "Return To 'Summary Data' Tab"
        ' 
        ' BasalLabel
        ' 
        BasalLabel.AutoSize = True
        BasalLabel.Dock = DockStyle.Fill
        BasalLabel.Location = New Point(157, 6)
        BasalLabel.Margin = New Padding(3)
        BasalLabel.Name = "BasalLabel"
        BasalLabel.Size = New Size(1201, 25)
        BasalLabel.TabIndex = 1
        BasalLabel.Text = "Basal"
        BasalLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' BelowLowLimitMessageLabel
        ' 
        BelowLowLimitMessageLabel.Anchor = AnchorStyles.Top
        BelowLowLimitMessageLabel.BackColor = Color.Transparent
        BelowLowLimitMessageLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        BelowLowLimitMessageLabel.ForeColor = Color.Red
        BelowLowLimitMessageLabel.Location = New Point(30, 330)
        BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        BelowLowLimitMessageLabel.Size = New Size(170, 21)
        BelowLowLimitMessageLabel.TabIndex = 32
        BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        BelowLowLimitMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' BelowLowLimitValueLabel
        ' 
        BelowLowLimitValueLabel.Anchor = AnchorStyles.Top
        BelowLowLimitValueLabel.BackColor = Color.Black
        BelowLowLimitValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        BelowLowLimitValueLabel.ForeColor = Color.White
        BelowLowLimitValueLabel.Location = New Point(55, 297)
        BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        BelowLowLimitValueLabel.Size = New Size(120, 33)
        BelowLowLimitValueLabel.TabIndex = 26
        BelowLowLimitValueLabel.Text = "2 %"
        BelowLowLimitValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CalibrationDueImage
        ' 
        CalibrationDueImage.BackColor = Color.Transparent
        CalibrationDueImage.Image = My.Resources.Resources.CalibrationUnavailable
        CalibrationDueImage.Location = New Point(474, 0)
        CalibrationDueImage.Name = "CalibrationDueImage"
        CalibrationDueImage.Size = New Size(58, 58)
        CalibrationDueImage.SizeMode = PictureBoxSizeMode.CenterImage
        CalibrationDueImage.TabIndex = 5
        CalibrationDueImage.TabStop = False
        ' 
        ' CalibrationShieldPanel
        ' 
        CalibrationShieldPanel.Controls.Add(TempTargetLabel)
        CalibrationShieldPanel.Controls.Add(ShieldUnitsLabel)
        CalibrationShieldPanel.Controls.Add(LastSGTimeLabel)
        CalibrationShieldPanel.Controls.Add(CurrentSgLabel)
        CalibrationShieldPanel.Controls.Add(SensorMessage)
        CalibrationShieldPanel.Controls.Add(CalibrationShieldPictureBox)
        CalibrationShieldPanel.Dock = DockStyle.Left
        CalibrationShieldPanel.Location = New Point(0, 0)
        CalibrationShieldPanel.Margin = New Padding(0)
        CalibrationShieldPanel.Name = "CalibrationShieldPanel"
        CalibrationShieldPanel.Size = New Size(116, 134)
        CalibrationShieldPanel.TabIndex = 64
        ' 
        ' TempTargetLabel
        ' 
        TempTargetLabel.BackColor = Color.Lime
        TempTargetLabel.Dock = DockStyle.Top
        TempTargetLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point)
        TempTargetLabel.ForeColor = Color.Black
        TempTargetLabel.Location = New Point(0, 0)
        TempTargetLabel.Name = "TempTargetLabel"
        TempTargetLabel.Size = New Size(116, 21)
        TempTargetLabel.TabIndex = 56
        TempTargetLabel.Text = "Target 150 2:00 Hr"
        TempTargetLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' ShieldUnitsLabel
        ' 
        ShieldUnitsLabel.AutoSize = True
        ShieldUnitsLabel.BackColor = Color.Transparent
        ShieldUnitsLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point)
        ShieldUnitsLabel.ForeColor = Color.White
        ShieldUnitsLabel.Location = New Point(38, 76)
        ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        ShieldUnitsLabel.Size = New Size(40, 13)
        ShieldUnitsLabel.TabIndex = 8
        ShieldUnitsLabel.Text = "XX/XX"
        ' 
        ' LastSGTimeLabel
        ' 
        LastSGTimeLabel.BackColor = Color.Transparent
        LastSGTimeLabel.Dock = DockStyle.Bottom
        LastSGTimeLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        LastSGTimeLabel.ForeColor = Color.White
        LastSGTimeLabel.Location = New Point(0, 113)
        LastSGTimeLabel.Name = "LastSGTimeLabel"
        LastSGTimeLabel.Size = New Size(116, 21)
        LastSGTimeLabel.TabIndex = 55
        LastSGTimeLabel.Text = "Time"
        LastSGTimeLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CurrentSgLabel
        ' 
        CurrentSgLabel.BackColor = Color.Transparent
        CurrentSgLabel.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point)
        CurrentSgLabel.ForeColor = Color.White
        CurrentSgLabel.Location = New Point(22, 35)
        CurrentSgLabel.Name = "CurrentSgLabel"
        CurrentSgLabel.Size = New Size(72, 32)
        CurrentSgLabel.TabIndex = 9
        CurrentSgLabel.Text = "---"
        CurrentSgLabel.TextAlign = ContentAlignment.MiddleCenter
        CurrentSgLabel.Visible = False
        ' 
        ' SensorMessage
        ' 
        SensorMessage.BackColor = Color.Transparent
        SensorMessage.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
        SensorMessage.ForeColor = Color.White
        SensorMessage.Location = New Point(0, 13)
        SensorMessage.Name = "SensorMessage"
        SensorMessage.Size = New Size(116, 57)
        SensorMessage.TabIndex = 1
        SensorMessage.Text = "Calibration Required"
        SensorMessage.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CalibrationShieldPictureBox
        ' 
        CalibrationShieldPictureBox.Image = My.Resources.Resources.Shield
        CalibrationShieldPictureBox.Location = New Point(0, 0)
        CalibrationShieldPictureBox.Margin = New Padding(5)
        CalibrationShieldPictureBox.Name = "CalibrationShieldPictureBox"
        CalibrationShieldPictureBox.Size = New Size(116, 116)
        CalibrationShieldPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        CalibrationShieldPictureBox.TabIndex = 5
        CalibrationShieldPictureBox.TabStop = False
        ' 
        ' CareLinkUserDataRecordBindingSource
        ' 
        CareLinkUserDataRecordBindingSource.DataSource = GetType(CareLinkUserDataRecord)
        ' 
        ' CursorMessage1Label
        ' 
        CursorMessage1Label.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        CursorMessage1Label.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        CursorMessage1Label.ForeColor = Color.White
        CursorMessage1Label.Location = New Point(0, 43)
        CursorMessage1Label.Name = "CursorMessage1Label"
        CursorMessage1Label.Size = New Size(178, 21)
        CursorMessage1Label.TabIndex = 39
        CursorMessage1Label.Text = "Blood Glucose"
        CursorMessage1Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CursorMessage2Label
        ' 
        CursorMessage2Label.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        CursorMessage2Label.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        CursorMessage2Label.ForeColor = Color.White
        CursorMessage2Label.Location = New Point(0, 65)
        CursorMessage2Label.Name = "CursorMessage2Label"
        CursorMessage2Label.Size = New Size(178, 21)
        CursorMessage2Label.TabIndex = 40
        CursorMessage2Label.Text = "Calibration Accepted"
        CursorMessage2Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CursorMessage3Label
        ' 
        CursorMessage3Label.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        CursorMessage3Label.ForeColor = Color.White
        CursorMessage3Label.Location = New Point(0, 86)
        CursorMessage3Label.Name = "CursorMessage3Label"
        CursorMessage3Label.Size = New Size(178, 21)
        CursorMessage3Label.TabIndex = 41
        CursorMessage3Label.Text = "138 ml/dl"
        CursorMessage3Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CursorMessage4Label
        ' 
        CursorMessage4Label.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        CursorMessage4Label.ForeColor = Color.White
        CursorMessage4Label.Location = New Point(0, 107)
        CursorMessage4Label.Name = "CursorMessage4Label"
        CursorMessage4Label.Size = New Size(178, 21)
        CursorMessage4Label.TabIndex = 41
        CursorMessage4Label.Text = "7.6 mmol/L"
        CursorMessage4Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' CursorPanel
        ' 
        CursorPanel.Controls.Add(CursorPictureBox)
        CursorPanel.Controls.Add(CursorMessage1Label)
        CursorPanel.Controls.Add(CursorMessage2Label)
        CursorPanel.Controls.Add(CursorMessage3Label)
        CursorPanel.Controls.Add(CursorMessage4Label)
        CursorPanel.Location = New Point(284, 0)
        CursorPanel.Margin = New Padding(0)
        CursorPanel.Name = "CursorPanel"
        CursorPanel.Size = New Size(178, 135)
        CursorPanel.TabIndex = 63
        ' 
        ' CursorPictureBox
        ' 
        CursorPictureBox.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        CursorPictureBox.Image = CType(resources.GetObject("CursorPictureBox.Image"), Image)
        CursorPictureBox.InitialImage = Nothing
        CursorPictureBox.Location = New Point(72, 0)
        CursorPictureBox.Name = "CursorPictureBox"
        CursorPictureBox.Size = New Size(34, 43)
        CursorPictureBox.SizeMode = PictureBoxSizeMode.CenterImage
        CursorPictureBox.TabIndex = 42
        CursorPictureBox.TabStop = False
        ' 
        ' CursorTimer
        ' 
        CursorTimer.Interval = 60000
        ' 
        ' DgvAutoBasalDelivery
        ' 
        DgvAutoBasalDelivery.Dock = DockStyle.Fill
        DgvAutoBasalDelivery.Location = New Point(6, 52)
        DgvAutoBasalDelivery.Name = "DgvAutoBasalDelivery"
        DgvAutoBasalDelivery.ReadOnly = True
        DgvAutoBasalDelivery.RowTemplate.Height = 25
        DgvAutoBasalDelivery.Size = New Size(1358, 597)
        DgvAutoBasalDelivery.TabIndex = 0
        ' 
        ' DgvCountryDataPg1
        ' 
        DgvCountryDataPg1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DgvCountryDataPg1.Columns.AddRange(New DataGridViewColumn() {DgvCountryDataPg1RecordNumber, DgvCountryDataPg1Category, DgvCountryDataPg1Key, DgvCountryDataPg1Value})
        DgvCountryDataPg1.Dock = DockStyle.Fill
        DgvCountryDataPg1.Location = New Point(3, 3)
        DgvCountryDataPg1.Name = "DgvCountryDataPg1"
        DgvCountryDataPg1.ReadOnly = True
        DgvCountryDataPg1.RowTemplate.Height = 25
        DgvCountryDataPg1.Size = New Size(1370, 655)
        DgvCountryDataPg1.TabIndex = 1
        ' 
        ' DgvCountryDataPg1RecordNumber
        ' 
        DgvCountryDataPg1RecordNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        DgvCountryDataPg1RecordNumber.HeaderText = "Record Number"
        DgvCountryDataPg1RecordNumber.MinimumWidth = 60
        DgvCountryDataPg1RecordNumber.Name = "DgvCountryDataPg1RecordNumber"
        DgvCountryDataPg1RecordNumber.ReadOnly = True
        DgvCountryDataPg1RecordNumber.Width = 60
        ' 
        ' DgvCountryDataPg1Category
        ' 
        DgvCountryDataPg1Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg1Category.HeaderText = "Category"
        DgvCountryDataPg1Category.Name = "DgvCountryDataPg1Category"
        DgvCountryDataPg1Category.ReadOnly = True
        DgvCountryDataPg1Category.Width = 80
        ' 
        ' DgvCountryDataPg1Key
        ' 
        DgvCountryDataPg1Key.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg1Key.HeaderText = "Key"
        DgvCountryDataPg1Key.Name = "DgvCountryDataPg1Key"
        DgvCountryDataPg1Key.ReadOnly = True
        DgvCountryDataPg1Key.Width = 51
        ' 
        ' DgvCountryDataPg1Value
        ' 
        DgvCountryDataPg1Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DgvCountryDataPg1Value.HeaderText = "Value"
        DgvCountryDataPg1Value.Name = "DgvCountryDataPg1Value"
        DgvCountryDataPg1Value.ReadOnly = True
        ' 
        ' DgvCareLinkUsers
        ' 
        DgvCareLinkUsers.AllowUserToAddRows = False
        DgvCareLinkUsers.AllowUserToResizeColumns = False
        DgvCareLinkUsers.AllowUserToResizeRows = False
        DgvCareLinkUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DgvCareLinkUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        DgvCareLinkUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DgvCareLinkUsers.Dock = DockStyle.Fill
        DgvCareLinkUsers.EditMode = DataGridViewEditMode.EditOnEnter
        DgvCareLinkUsers.Location = New Point(3, 3)
        DgvCareLinkUsers.Name = "DgvCareLinkUsers"
        DgvCareLinkUsers.RowTemplate.Height = 25
        DgvCareLinkUsers.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvCareLinkUsers.Size = New Size(1370, 655)
        DgvCareLinkUsers.TabIndex = 0
        ' 
        ' DgvCountryDataPg3
        ' 
        DgvCountryDataPg3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DgvCountryDataPg3.Columns.AddRange(New DataGridViewColumn() {DgvCountryDataPg3RecordNumber, DgvCountryDataPg3Category, DgvCountryDataPg3Key, DgvCountryDataPg3Value, DgvCountryDataPg3OnlyFor, DgvCountryDataPg3NotFor})
        DgvCountryDataPg3.Dock = DockStyle.Fill
        DgvCountryDataPg3.Location = New Point(3, 3)
        DgvCountryDataPg3.Name = "DgvCountryDataPg3"
        DgvCountryDataPg3.ReadOnly = True
        DgvCountryDataPg3.RowTemplate.Height = 25
        DgvCountryDataPg3.Size = New Size(1370, 655)
        DgvCountryDataPg3.TabIndex = 1
        ' 
        ' DgvCountryDataPg3RecordNumber
        ' 
        DgvCountryDataPg3RecordNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        DgvCountryDataPg3RecordNumber.HeaderText = "Record Number"
        DgvCountryDataPg3RecordNumber.MinimumWidth = 60
        DgvCountryDataPg3RecordNumber.Name = "DgvCountryDataPg3RecordNumber"
        DgvCountryDataPg3RecordNumber.ReadOnly = True
        DgvCountryDataPg3RecordNumber.Width = 60
        ' 
        ' DgvCountryDataPg3Category
        ' 
        DgvCountryDataPg3Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg3Category.HeaderText = "Category"
        DgvCountryDataPg3Category.Name = "DgvCountryDataPg3Category"
        DgvCountryDataPg3Category.ReadOnly = True
        DgvCountryDataPg3Category.Width = 80
        ' 
        ' DgvCountryDataPg3Key
        ' 
        DgvCountryDataPg3Key.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg3Key.HeaderText = "Key"
        DgvCountryDataPg3Key.Name = "DgvCountryDataPg3Key"
        DgvCountryDataPg3Key.ReadOnly = True
        DgvCountryDataPg3Key.Width = 51
        ' 
        ' DgvCountryDataPg3Value
        ' 
        DgvCountryDataPg3Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg3Value.HeaderText = "Value"
        DgvCountryDataPg3Value.Name = "DgvCountryDataPg3Value"
        DgvCountryDataPg3Value.ReadOnly = True
        DgvCountryDataPg3Value.Width = 60
        ' 
        ' DgvCountryDataPg3OnlyFor
        ' 
        DgvCountryDataPg3OnlyFor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DgvCountryDataPg3OnlyFor.HeaderText = "Report Only For"
        DgvCountryDataPg3OnlyFor.Name = "DgvCountryDataPg3OnlyFor"
        DgvCountryDataPg3OnlyFor.ReadOnly = True
        ' 
        ' DgvCountryDataPg3NotFor
        ' 
        DgvCountryDataPg3NotFor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DgvCountryDataPg3NotFor.HeaderText = "Report Not For"
        DgvCountryDataPg3NotFor.Name = "DgvCountryDataPg3NotFor"
        DgvCountryDataPg3NotFor.ReadOnly = True
        ' 
        ' DgvCurrentUser
        ' 
        DgvCurrentUser.AllowUserToAddRows = False
        DgvCurrentUser.AllowUserToDeleteRows = False
        DgvCurrentUser.AllowUserToResizeColumns = False
        DgvCurrentUser.AllowUserToResizeRows = False
        DgvCurrentUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DgvCurrentUser.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        DgvCurrentUser.Dock = DockStyle.Fill
        DgvCurrentUser.Location = New Point(0, 0)
        DgvCurrentUser.Name = "DgvCurrentUser"
        DgvCurrentUser.ReadOnly = True
        DgvCurrentUser.RowHeadersVisible = False
        DgvCurrentUser.RowTemplate.Height = 25
        DgvCurrentUser.Size = New Size(1376, 661)
        DgvCurrentUser.TabIndex = 0
        ' 
        ' DgvInsulin
        ' 
        DgvInsulin.Dock = DockStyle.Fill
        DgvInsulin.Location = New Point(6, 52)
        DgvInsulin.Name = "DgvInsulin"
        DgvInsulin.ReadOnly = True
        DgvInsulin.RowTemplate.Height = 25
        DgvInsulin.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvInsulin.Size = New Size(1358, 597)
        DgvInsulin.TabIndex = 0
        ' 
        ' DgvMeal
        ' 
        DgvMeal.Dock = DockStyle.Fill
        DgvMeal.Location = New Point(6, 52)
        DgvMeal.Name = "DgvMeal"
        DgvMeal.Size = New Size(1358, 597)
        DgvMeal.TabIndex = 2
        ' 
        ' DgvSGs
        ' 
        DgvSGs.Dock = DockStyle.Fill
        DgvSGs.Location = New Point(3, 46)
        DgvSGs.Name = "DgvSGs"
        DgvSGs.RowTemplate.Height = 25
        DgvSGs.Size = New Size(1364, 606)
        DgvSGs.TabIndex = 1
        ' 
        ' DgvSummary
        ' 
        DgvSummary.Dock = DockStyle.Fill
        DgvSummary.Location = New Point(3, 3)
        DgvSummary.Name = "DgvSummary"
        DgvSummary.ReadOnly = True
        DgvSummary.RowTemplate.Height = 25
        DgvSummary.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvSummary.Size = New Size(1370, 655)
        DgvSummary.TabIndex = 0
        ' 
        ' DgvSessionProfile
        ' 
        DgvSessionProfile.Dock = DockStyle.Fill
        DgvSessionProfile.Location = New Point(3, 3)
        DgvSessionProfile.Name = "DgvSessionProfile"
        DgvSessionProfile.ReadOnly = True
        DgvSessionProfile.RowHeadersVisible = False
        DgvSessionProfile.RowTemplate.Height = 25
        DgvSessionProfile.Size = New Size(1370, 655)
        DgvSessionProfile.TabIndex = 0
        ' 
        ' ImageList1
        ' 
        ImageList1.ColorDepth = ColorDepth.Depth32Bit
        ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), ImageListStreamer)
        ImageList1.TransparentColor = Color.Transparent
        ImageList1.Images.SetKeyName(0, "ReservoirRemains0.png")
        ImageList1.Images.SetKeyName(1, "ReservoirRemains1+.png")
        ImageList1.Images.SetKeyName(2, "ReservoirRemains15+.png")
        ImageList1.Images.SetKeyName(3, "ReservoirRemains29+.png")
        ImageList1.Images.SetKeyName(4, "ReservoirRemains43+.png")
        ImageList1.Images.SetKeyName(5, "ReservoirRemains57+.png")
        ImageList1.Images.SetKeyName(6, "ReservoirRemains71+.png")
        ImageList1.Images.SetKeyName(7, "ReservoirRemains85+.png")
        ImageList1.Images.SetKeyName(8, "ReservoirRemainsUnknown.png")
        ' 
        ' InRangeMessageLabel
        ' 
        InRangeMessageLabel.Anchor = AnchorStyles.Top
        InRangeMessageLabel.AutoSize = True
        InRangeMessageLabel.BackColor = Color.Transparent
        InRangeMessageLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        InRangeMessageLabel.ForeColor = Color.Lime
        InRangeMessageLabel.Location = New Point(81, 269)
        InRangeMessageLabel.Name = "InRangeMessageLabel"
        InRangeMessageLabel.Size = New Size(73, 21)
        InRangeMessageLabel.TabIndex = 30
        InRangeMessageLabel.Text = "In range"
        InRangeMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' InsulinLevelPictureBox
        ' 
        InsulinLevelPictureBox.Image = CType(resources.GetObject("InsulinLevelPictureBox.Image"), Image)
        InsulinLevelPictureBox.InitialImage = Nothing
        InsulinLevelPictureBox.Location = New Point(221, 0)
        InsulinLevelPictureBox.Name = "InsulinLevelPictureBox"
        InsulinLevelPictureBox.Padding = New Padding(10)
        InsulinLevelPictureBox.Size = New Size(51, 67)
        InsulinLevelPictureBox.SizeMode = PictureBoxSizeMode.Zoom
        InsulinLevelPictureBox.TabIndex = 12
        InsulinLevelPictureBox.TabStop = False
        ' 
        ' LabelSgTrend
        ' 
        LabelSgTrend.BackColor = Color.Black
        LabelSgTrend.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        LabelSgTrend.ForeColor = Color.White
        LabelSgTrend.Location = New Point(461, 64)
        LabelSgTrend.Name = "LabelSgTrend"
        LabelSgTrend.Size = New Size(84, 21)
        LabelSgTrend.TabIndex = 61
        LabelSgTrend.Text = "SG Trend"
        LabelSgTrend.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' LabelTimeChange
        ' 
        LabelTimeChange.AutoSize = True
        LabelTimeChange.Dock = DockStyle.Fill
        LabelTimeChange.Location = New Point(6, 6)
        LabelTimeChange.Margin = New Padding(3)
        LabelTimeChange.Name = "LabelTimeChange"
        LabelTimeChange.Size = New Size(174, 15)
        LabelTimeChange.TabIndex = 0
        LabelTimeChange.Text = "Time Change"
        LabelTimeChange.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' LabelTrendArrows
        ' 
        LabelTrendArrows.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
        LabelTrendArrows.ForeColor = Color.White
        LabelTrendArrows.Location = New Point(461, 103)
        LabelTrendArrows.Name = "LabelTrendArrows"
        LabelTrendArrows.Size = New Size(84, 24)
        LabelTrendArrows.TabIndex = 62
        LabelTrendArrows.Text = "↑↔↓"
        LabelTrendArrows.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' LabelTrendValue
        ' 
        LabelTrendValue.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        LabelTrendValue.ForeColor = Color.White
        LabelTrendValue.Location = New Point(461, 89)
        LabelTrendValue.Name = "LabelTrendValue"
        LabelTrendValue.Size = New Size(84, 21)
        LabelTrendValue.TabIndex = 68
        LabelTrendValue.Text = "+ 5"
        LabelTrendValue.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Last24AutoCorrectionLabel
        ' 
        Last24AutoCorrectionLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24AutoCorrectionLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24AutoCorrectionLabel.ForeColor = Color.White
        Last24AutoCorrectionLabel.Location = New Point(0, 84)
        Last24AutoCorrectionLabel.Name = "Last24AutoCorrectionLabel"
        Last24AutoCorrectionLabel.Size = New Size(235, 21)
        Last24AutoCorrectionLabel.TabIndex = 64
        Last24AutoCorrectionLabel.Text = "Auto Correction 20U | 20%"
        Last24AutoCorrectionLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24CarbsValueLabel
        ' 
        Last24CarbsValueLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24CarbsValueLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24CarbsValueLabel.ForeColor = Color.White
        Last24CarbsValueLabel.Location = New Point(0, 105)
        Last24CarbsValueLabel.Name = "Last24CarbsValueLabel"
        Last24CarbsValueLabel.Size = New Size(218, 21)
        Last24CarbsValueLabel.TabIndex = 66
        Last24CarbsValueLabel.Text = "Carbs 100 Grams"
        Last24CarbsValueLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24DailyDoseLabel
        ' 
        Last24DailyDoseLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24DailyDoseLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24DailyDoseLabel.ForeColor = Color.White
        Last24DailyDoseLabel.Location = New Point(0, 23)
        Last24DailyDoseLabel.Name = "Last24DailyDoseLabel"
        Last24DailyDoseLabel.Size = New Size(235, 21)
        Last24DailyDoseLabel.TabIndex = 61
        Last24DailyDoseLabel.Text = "Insulin Dose 100U"
        Last24DailyDoseLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24BasalLabel
        ' 
        Last24BasalLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24BasalLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24BasalLabel.ForeColor = Color.White
        Last24BasalLabel.Location = New Point(0, 42)
        Last24BasalLabel.Name = "Last24BasalLabel"
        Last24BasalLabel.Size = New Size(235, 21)
        Last24BasalLabel.TabIndex = 62
        Last24BasalLabel.Text = "Basal 50U | 50%"
        Last24BasalLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HoursGraphLabel
        ' 
        Last24HoursGraphLabel.Anchor = AnchorStyles.Top
        Last24HoursGraphLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24HoursGraphLabel.ForeColor = Color.White
        Last24HoursGraphLabel.Location = New Point(30, 26)
        Last24HoursGraphLabel.Name = "Last24HoursGraphLabel"
        Last24HoursGraphLabel.Size = New Size(170, 21)
        Last24HoursGraphLabel.TabIndex = 34
        Last24HoursGraphLabel.Text = "Last 24 hours"
        Last24HoursGraphLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Last24HTotalsPanel
        ' 
        Last24HTotalsPanel.BorderStyle = BorderStyle.Fixed3D
        Last24HTotalsPanel.Controls.Add(Last24CarbsValueLabel)
        Last24HTotalsPanel.Controls.Add(Last24AutoCorrectionLabel)
        Last24HTotalsPanel.Controls.Add(Last24ManualBolusLabel)
        Last24HTotalsPanel.Controls.Add(Last24BasalLabel)
        Last24HTotalsPanel.Controls.Add(Last24DailyDoseLabel)
        Last24HTotalsPanel.Controls.Add(Last24TotalsLabel)
        Last24HTotalsPanel.Location = New Point(740, 0)
        Last24HTotalsPanel.Name = "Last24HTotalsPanel"
        Last24HTotalsPanel.Size = New Size(237, 129)
        Last24HTotalsPanel.TabIndex = 66
        ' 
        ' Last24ManualBolusLabel
        ' 
        Last24ManualBolusLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24ManualBolusLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24ManualBolusLabel.ForeColor = Color.White
        Last24ManualBolusLabel.Location = New Point(0, 63)
        Last24ManualBolusLabel.Name = "Last24ManualBolusLabel"
        Last24ManualBolusLabel.Size = New Size(233, 21)
        Last24ManualBolusLabel.TabIndex = 63
        Last24ManualBolusLabel.Text = "Manual Bolus 30U | 30%"
        Last24ManualBolusLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24TotalsLabel
        ' 
        Last24TotalsLabel.BackColor = Color.White
        Last24TotalsLabel.Dock = DockStyle.Top
        Last24TotalsLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Last24TotalsLabel.ForeColor = Color.Black
        Last24TotalsLabel.Location = New Point(0, 0)
        Last24TotalsLabel.Name = "Last24TotalsLabel"
        Last24TotalsLabel.Size = New Size(233, 23)
        Last24TotalsLabel.TabIndex = 65
        Last24TotalsLabel.Text = "Last 24 Hr Totals"
        Last24TotalsLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' MaxBasalPerHourLabel
        ' 
        MaxBasalPerHourLabel.AutoSize = True
        MaxBasalPerHourLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        MaxBasalPerHourLabel.ForeColor = Color.White
        MaxBasalPerHourLabel.Location = New Point(978, 108)
        MaxBasalPerHourLabel.Name = "MaxBasalPerHourLabel"
        MaxBasalPerHourLabel.Size = New Size(161, 21)
        MaxBasalPerHourLabel.TabIndex = 67
        MaxBasalPerHourLabel.Text = "Max Basal/Hr ~2.0U"
        MaxBasalPerHourLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' ModelLabel
        ' 
        ModelLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        ModelLabel.ForeColor = Color.White
        ModelLabel.Location = New Point(1140, 26)
        ModelLabel.Name = "ModelLabel"
        ModelLabel.Size = New Size(230, 21)
        ModelLabel.TabIndex = 57
        ModelLabel.Text = "Model"
        ' 
        ' PumpNameLabel
        ' 
        PumpNameLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        PumpNameLabel.ForeColor = Color.White
        PumpNameLabel.Location = New Point(1140, 52)
        PumpNameLabel.Name = "PumpNameLabel"
        PumpNameLabel.Size = New Size(230, 21)
        PumpNameLabel.TabIndex = 70
        PumpNameLabel.Text = "Pump Name"
        ' 
        ' NotifyIcon1
        ' 
        NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), Icon)
        NotifyIcon1.Text = "CareLink™ For Windows"
        ' 
        ' PumpBatteryPictureBox
        ' 
        PumpBatteryPictureBox.ErrorImage = Nothing
        PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryFull
        PumpBatteryPictureBox.Location = New Point(124, 0)
        PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        PumpBatteryPictureBox.Size = New Size(74, 84)
        PumpBatteryPictureBox.TabIndex = 43
        PumpBatteryPictureBox.TabStop = False
        ' 
        ' PumpBatteryRemainingLabel
        ' 
        PumpBatteryRemainingLabel.BackColor = Color.Transparent
        PumpBatteryRemainingLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        PumpBatteryRemainingLabel.ForeColor = Color.White
        PumpBatteryRemainingLabel.Location = New Point(119, 89)
        PumpBatteryRemainingLabel.Name = "PumpBatteryRemainingLabel"
        PumpBatteryRemainingLabel.Size = New Size(84, 21)
        PumpBatteryRemainingLabel.TabIndex = 11
        PumpBatteryRemainingLabel.Text = "Unknown"
        PumpBatteryRemainingLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' InsulinTypeLabel
        ' 
        InsulinTypeLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        InsulinTypeLabel.ForeColor = Color.White
        InsulinTypeLabel.Location = New Point(978, 3)
        InsulinTypeLabel.Name = "InsulinTypeLabel"
        InsulinTypeLabel.Size = New Size(162, 21)
        InsulinTypeLabel.TabIndex = 54
        InsulinTypeLabel.Text = "Humalog/Novolog"
        InsulinTypeLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' ReadingsLabel
        ' 
        ReadingsLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        ReadingsLabel.ForeColor = Color.White
        ReadingsLabel.Location = New Point(1140, 106)
        ReadingsLabel.Name = "ReadingsLabel"
        ReadingsLabel.Size = New Size(235, 21)
        ReadingsLabel.TabIndex = 53
        ReadingsLabel.Text = "280/288 SG Readings"
        ReadingsLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' RemainingInsulinUnits
        ' 
        RemainingInsulinUnits.BackColor = Color.Transparent
        RemainingInsulinUnits.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        RemainingInsulinUnits.ForeColor = Color.White
        RemainingInsulinUnits.Location = New Point(206, 90)
        RemainingInsulinUnits.Name = "RemainingInsulinUnits"
        RemainingInsulinUnits.Size = New Size(80, 21)
        RemainingInsulinUnits.TabIndex = 12
        RemainingInsulinUnits.Text = "000.0U"
        RemainingInsulinUnits.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SensorDaysLeftLabel
        ' 
        SensorDaysLeftLabel.BackColor = Color.Transparent
        SensorDaysLeftLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        SensorDaysLeftLabel.ForeColor = Color.White
        SensorDaysLeftLabel.Location = New Point(0, 16)
        SensorDaysLeftLabel.Name = "SensorDaysLeftLabel"
        SensorDaysLeftLabel.Size = New Size(55, 40)
        SensorDaysLeftLabel.TabIndex = 45
        SensorDaysLeftLabel.Text = "<1"
        SensorDaysLeftLabel.TextAlign = ContentAlignment.MiddleCenter
        SensorDaysLeftLabel.Visible = False
        ' 
        ' SensorTimeLeftLabel
        ' 
        SensorTimeLeftLabel.BackColor = Color.Transparent
        SensorTimeLeftLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        SensorTimeLeftLabel.ForeColor = Color.White
        SensorTimeLeftLabel.Location = New Point(0, 89)
        SensorTimeLeftLabel.Name = "SensorTimeLeftLabel"
        SensorTimeLeftLabel.Size = New Size(94, 21)
        SensorTimeLeftLabel.TabIndex = 46
        SensorTimeLeftLabel.Text = "???"
        SensorTimeLeftLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SensorTimeLeftPanel
        ' 
        SensorTimeLeftPanel.Controls.Add(SensorDaysLeftLabel)
        SensorTimeLeftPanel.Controls.Add(SensorTimeLeftLabel)
        SensorTimeLeftPanel.Controls.Add(SensorTimeLeftPictureBox)
        SensorTimeLeftPanel.Location = New Point(638, 0)
        SensorTimeLeftPanel.Name = "SensorTimeLeftPanel"
        SensorTimeLeftPanel.Size = New Size(94, 129)
        SensorTimeLeftPanel.TabIndex = 65
        ' 
        ' SensorTimeLeftPictureBox
        ' 
        SensorTimeLeftPictureBox.ErrorImage = Nothing
        SensorTimeLeftPictureBox.Image = My.Resources.Resources.SensorExpirationUnknown
        SensorTimeLeftPictureBox.Location = New Point(15, 0)
        SensorTimeLeftPictureBox.Name = "SensorTimeLeftPictureBox"
        SensorTimeLeftPictureBox.Size = New Size(74, 84)
        SensorTimeLeftPictureBox.TabIndex = 47
        SensorTimeLeftPictureBox.TabStop = False
        ' 
        ' SerialNumberButton
        ' 
        SerialNumberButton.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        SerialNumberButton.ForeColor = Color.White
        SerialNumberButton.Location = New Point(1140, 74)
        SerialNumberButton.Name = "SerialNumberButton"
        SerialNumberButton.Size = New Size(230, 29)
        SerialNumberButton.TabIndex = 56
        SerialNumberButton.Text = "Serial Number Details..."
        ToolTip1.SetToolTip(SerialNumberButton, "Click for details")
        ' 
        ' ServerUpdateTimer
        ' 
        ServerUpdateTimer.Interval = 300000
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.IsSplitterFixed = True
        SplitContainer2.Location = New Point(3, 3)
        SplitContainer2.Name = "SplitContainer2"
        SplitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(PumpAITLabel)
        SplitContainer2.Panel1.Controls.Add(LabelTrendValue)
        SplitContainer2.Panel1.Controls.Add(MaxBasalPerHourLabel)
        SplitContainer2.Panel1.Controls.Add(Last24HTotalsPanel)
        SplitContainer2.Panel1.Controls.Add(SensorTimeLeftPanel)
        SplitContainer2.Panel1.Controls.Add(LabelTrendArrows)
        SplitContainer2.Panel1.Controls.Add(LabelSgTrend)
        SplitContainer2.Panel1.Controls.Add(ModelLabel)
        SplitContainer2.Panel1.Controls.Add(PumpNameLabel)
        SplitContainer2.Panel1.Controls.Add(SerialNumberButton)
        SplitContainer2.Panel1.Controls.Add(InsulinTypeLabel)
        SplitContainer2.Panel1.Controls.Add(ReadingsLabel)
        SplitContainer2.Panel1.Controls.Add(PumpBatteryRemainingLabel)
        SplitContainer2.Panel1.Controls.Add(PumpBatteryRemaining2Label)
        SplitContainer2.Panel1.Controls.Add(TransmitterBatteryPercentLabel)
        SplitContainer2.Panel1.Controls.Add(TransmitterBatteryPictureBox)
        SplitContainer2.Panel1.Controls.Add(PumpBatteryPictureBox)
        SplitContainer2.Panel1.Controls.Add(FullNameLabel)
        SplitContainer2.Panel1.Controls.Add(RemainingInsulinUnits)
        SplitContainer2.Panel1.Controls.Add(InsulinLevelPictureBox)
        SplitContainer2.Panel1.Controls.Add(ActiveInsulinValue)
        SplitContainer2.Panel1.Controls.Add(CalibrationDueImage)
        SplitContainer2.Panel1.Controls.Add(CursorPanel)
        SplitContainer2.Panel1.Controls.Add(CalibrationShieldPanel)
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(SplitContainer3)
        SplitContainer2.Size = New Size(1370, 655)
        SplitContainer2.SplitterDistance = 134
        SplitContainer2.TabIndex = 52
        ' 
        ' PumpAITLabel
        ' 
        PumpAITLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        PumpAITLabel.ForeColor = Color.White
        PumpAITLabel.Location = New Point(978, 28)
        PumpAITLabel.Name = "PumpAITLabel"
        PumpAITLabel.Size = New Size(162, 21)
        PumpAITLabel.TabIndex = 71
        PumpAITLabel.Text = "Pump AIT 3:00"
        PumpAITLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' PumpBatteryRemaining2Label
        ' 
        PumpBatteryRemaining2Label.BackColor = Color.Transparent
        PumpBatteryRemaining2Label.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        PumpBatteryRemaining2Label.ForeColor = Color.White
        PumpBatteryRemaining2Label.Location = New Point(119, 106)
        PumpBatteryRemaining2Label.Name = "PumpBatteryRemaining2Label"
        PumpBatteryRemaining2Label.Size = New Size(84, 21)
        PumpBatteryRemaining2Label.TabIndex = 69
        PumpBatteryRemaining2Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TransmitterBatteryPercentLabel
        ' 
        TransmitterBatteryPercentLabel.BackColor = Color.Transparent
        TransmitterBatteryPercentLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point)
        TransmitterBatteryPercentLabel.ForeColor = Color.White
        TransmitterBatteryPercentLabel.Location = New Point(549, 89)
        TransmitterBatteryPercentLabel.Name = "TransmitterBatteryPercentLabel"
        TransmitterBatteryPercentLabel.Size = New Size(85, 21)
        TransmitterBatteryPercentLabel.TabIndex = 13
        TransmitterBatteryPercentLabel.Text = "???"
        TransmitterBatteryPercentLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TransmitterBatteryPictureBox
        ' 
        TransmitterBatteryPictureBox.ErrorImage = Nothing
        TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryUnknown
        TransmitterBatteryPictureBox.Location = New Point(554, 0)
        TransmitterBatteryPictureBox.Name = "TransmitterBatteryPictureBox"
        TransmitterBatteryPictureBox.Size = New Size(74, 84)
        TransmitterBatteryPictureBox.TabIndex = 47
        TransmitterBatteryPictureBox.TabStop = False
        ' 
        ' SplitContainer3
        ' 
        SplitContainer3.IsSplitterFixed = True
        SplitContainer3.Location = New Point(0, 0)
        SplitContainer3.Name = "SplitContainer3"
        ' 
        ' SplitContainer3.Panel1
        ' 
        SplitContainer3.Panel1.BackColor = Color.Black
        ' 
        ' SplitContainer3.Panel2
        ' 
        SplitContainer3.Panel2.Controls.Add(TimeInRangeLabel)
        SplitContainer3.Panel2.Controls.Add(Last24HoursGraphLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeChartLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeSummaryPercentCharLabel)
        SplitContainer3.Panel2.Controls.Add(AboveHighLimitValueLabel)
        SplitContainer3.Panel2.Controls.Add(AboveHighLimitMessageLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeValueLabel)
        SplitContainer3.Panel2.Controls.Add(InRangeMessageLabel)
        SplitContainer3.Panel2.Controls.Add(BelowLowLimitValueLabel)
        SplitContainer3.Panel2.Controls.Add(BelowLowLimitMessageLabel)
        SplitContainer3.Panel2.Controls.Add(AverageSGValueLabel)
        SplitContainer3.Panel2.Controls.Add(AverageSGMessageLabel)
        SplitContainer3.Panel2.Controls.Add(TirComplianceLabel)
        SplitContainer3.Panel2.Controls.Add(HighTirComplianceLabel)
        SplitContainer3.Panel2.Controls.Add(LowTirComplianceLabel)
        SplitContainer3.Panel2.Controls.Add(SmartGuardLabel)
        SplitContainer3.Size = New Size(1370, 513)
        SplitContainer3.SplitterDistance = 1136
        SplitContainer3.TabIndex = 0
        ' 
        ' TimeInRangeLabel
        ' 
        TimeInRangeLabel.Anchor = AnchorStyles.Top
        TimeInRangeLabel.Font = New Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        TimeInRangeLabel.ForeColor = Color.White
        TimeInRangeLabel.Location = New Point(30, 0)
        TimeInRangeLabel.Name = "TimeInRangeLabel"
        TimeInRangeLabel.Size = New Size(170, 21)
        TimeInRangeLabel.TabIndex = 33
        TimeInRangeLabel.Text = "Time in range"
        TimeInRangeLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInRangeChartLabel
        ' 
        TimeInRangeChartLabel.Anchor = AnchorStyles.Top
        TimeInRangeChartLabel.BackColor = Color.Black
        TimeInRangeChartLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        TimeInRangeChartLabel.ForeColor = Color.White
        TimeInRangeChartLabel.Location = New Point(73, 74)
        TimeInRangeChartLabel.Name = "TimeInRangeChartLabel"
        TimeInRangeChartLabel.Size = New Size(76, 47)
        TimeInRangeChartLabel.TabIndex = 2
        TimeInRangeChartLabel.Text = "100"
        TimeInRangeChartLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInRangeSummaryPercentCharLabel
        ' 
        TimeInRangeSummaryPercentCharLabel.Anchor = AnchorStyles.Top
        TimeInRangeSummaryPercentCharLabel.AutoSize = True
        TimeInRangeSummaryPercentCharLabel.BackColor = Color.Transparent
        TimeInRangeSummaryPercentCharLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        TimeInRangeSummaryPercentCharLabel.ForeColor = Color.White
        TimeInRangeSummaryPercentCharLabel.Location = New Point(94, 113)
        TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        TimeInRangeSummaryPercentCharLabel.Size = New Size(42, 40)
        TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        TimeInRangeSummaryPercentCharLabel.Text = "%"
        ' 
        ' TimeInRangeValueLabel
        ' 
        TimeInRangeValueLabel.Anchor = AnchorStyles.Top
        TimeInRangeValueLabel.BackColor = Color.Black
        TimeInRangeValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point)
        TimeInRangeValueLabel.ForeColor = Color.White
        TimeInRangeValueLabel.Location = New Point(55, 236)
        TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        TimeInRangeValueLabel.Size = New Size(120, 33)
        TimeInRangeValueLabel.TabIndex = 24
        TimeInRangeValueLabel.Text = "90 %"
        TimeInRangeValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TirComplianceLabel
        ' 
        TirComplianceLabel.Anchor = AnchorStyles.Top
        TirComplianceLabel.BackColor = Color.Transparent
        TirComplianceLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
        TirComplianceLabel.ForeColor = Color.White
        TirComplianceLabel.Location = New Point(3, 438)
        TirComplianceLabel.Name = "TirComplianceLabel"
        TirComplianceLabel.Size = New Size(226, 25)
        TirComplianceLabel.TabIndex = 35
        TirComplianceLabel.Text = "TIR Compliance"
        TirComplianceLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' HighTirComplianceLabel
        ' 
        HighTirComplianceLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        HighTirComplianceLabel.ForeColor = Color.LimeGreen
        HighTirComplianceLabel.Location = New Point(115, 460)
        HighTirComplianceLabel.Name = "HighTirComplianceLabel"
        HighTirComplianceLabel.Size = New Size(112, 57)
        HighTirComplianceLabel.TabIndex = 37
        HighTirComplianceLabel.Text = "High" & vbCrLf & "Excellent"
        HighTirComplianceLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' LowTirComplianceLabel
        ' 
        LowTirComplianceLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        LowTirComplianceLabel.ForeColor = Color.Red
        LowTirComplianceLabel.Location = New Point(3, 460)
        LowTirComplianceLabel.Name = "LowTirComplianceLabel"
        LowTirComplianceLabel.Size = New Size(112, 57)
        LowTirComplianceLabel.TabIndex = 36
        LowTirComplianceLabel.Text = "(6.3) Low" & vbCrLf & "Needs Improvement"
        LowTirComplianceLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SmartGuardLabel
        ' 
        SmartGuardLabel.Anchor = AnchorStyles.Top
        SmartGuardLabel.BackColor = Color.Transparent
        SmartGuardLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
        SmartGuardLabel.ForeColor = Color.DodgerBlue
        SmartGuardLabel.Location = New Point(3, 413)
        SmartGuardLabel.Name = "SmartGuardLabel"
        SmartGuardLabel.Size = New Size(224, 21)
        SmartGuardLabel.TabIndex = 35
        SmartGuardLabel.Text = "SmartGuard 100%"
        SmartGuardLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TabControlPage1
        ' 
        TabControlPage1.Appearance = TabAppearance.Buttons
        TabControlPage1.Controls.Add(TabPage01HomePage)
        TabControlPage1.Controls.Add(TabPage02RunningIOB)
        TabControlPage1.Controls.Add(TabPage03TreatmentDetails)
        TabControlPage1.Controls.Add(TabPage04SummaryData)
        TabControlPage1.Controls.Add(TabPage05Insulin)
        TabControlPage1.Controls.Add(TabPage06Meal)
        TabControlPage1.Controls.Add(TabPage07ActiveInsulin)
        TabControlPage1.Controls.Add(TabPage08SensorGlucose)
        TabControlPage1.Controls.Add(TabPage09Limits)
        TabControlPage1.Controls.Add(TabPage10NotificationHistory)
        TabControlPage1.Controls.Add(TabPage11TherapyAlgorithm)
        TabControlPage1.Controls.Add(TabPage12BannerState)
        TabControlPage1.Controls.Add(TabPage13Basal)
        TabControlPage1.Controls.Add(TabPage14Markers)
        TabControlPage1.Dock = DockStyle.Fill
        TabControlPage1.Location = New Point(0, 24)
        TabControlPage1.Name = "TabControlPage1"
        TabControlPage1.SelectedIndex = 0
        TabControlPage1.Size = New Size(1384, 692)
        TabControlPage1.TabIndex = 0
        ' 
        ' TabPage01HomePage
        ' 
        TabPage01HomePage.BackColor = Color.Black
        TabPage01HomePage.Controls.Add(SplitContainer2)
        TabPage01HomePage.Location = New Point(4, 27)
        TabPage01HomePage.Name = "TabPage01HomePage"
        TabPage01HomePage.Padding = New Padding(3)
        TabPage01HomePage.Size = New Size(1376, 661)
        TabPage01HomePage.TabIndex = 7
        TabPage01HomePage.Text = "Summary"
        ' 
        ' TabPage02RunningIOB
        ' 
        TabPage02RunningIOB.Controls.Add(SplitContainer1)
        TabPage02RunningIOB.Location = New Point(4, 27)
        TabPage02RunningIOB.Name = "TabPage02RunningIOB"
        TabPage02RunningIOB.Padding = New Padding(3)
        TabPage02RunningIOB.Size = New Size(1376, 661)
        TabPage02RunningIOB.TabIndex = 15
        TabPage02RunningIOB.Text = "Running IOB"
        TabPage02RunningIOB.UseVisualStyleBackColor = True
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.FixedPanel = FixedPanel.Panel1
        SplitContainer1.Location = New Point(3, 3)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.BackColor = Color.Black
        SplitContainer1.Panel1.Controls.Add(TemporaryUseAdvanceAITDecayCheckBox)
        SplitContainer1.Size = New Size(1370, 655)
        SplitContainer1.SplitterDistance = 30
        SplitContainer1.TabIndex = 0
        ' 
        ' TemporaryUseAdvanceAITDecayCheckBox
        ' 
        TemporaryUseAdvanceAITDecayCheckBox.AutoSize = True
        TemporaryUseAdvanceAITDecayCheckBox.BackColor = SystemColors.ControlText
        TemporaryUseAdvanceAITDecayCheckBox.ForeColor = SystemColors.ControlLightLight
        TemporaryUseAdvanceAITDecayCheckBox.Location = New Point(12, 6)
        TemporaryUseAdvanceAITDecayCheckBox.Name = "TemporaryUseAdvanceAITDecayCheckBox"
        TemporaryUseAdvanceAITDecayCheckBox.Size = New Size(146, 19)
        TemporaryUseAdvanceAITDecayCheckBox.TabIndex = 0
        TemporaryUseAdvanceAITDecayCheckBox.Text = "AIT Decay over 3 hours"
        TemporaryUseAdvanceAITDecayCheckBox.UseVisualStyleBackColor = False
        ' 
        ' TabPage03TreatmentDetails
        ' 
        TabPage03TreatmentDetails.Location = New Point(4, 27)
        TabPage03TreatmentDetails.Name = "TabPage03TreatmentDetails"
        TabPage03TreatmentDetails.Padding = New Padding(3)
        TabPage03TreatmentDetails.Size = New Size(1376, 661)
        TabPage03TreatmentDetails.TabIndex = 8
        TabPage03TreatmentDetails.Text = "Treatment Details"
        TabPage03TreatmentDetails.UseVisualStyleBackColor = True
        ' 
        ' TabPage04SummaryData
        ' 
        TabPage04SummaryData.Controls.Add(DgvSummary)
        TabPage04SummaryData.Location = New Point(4, 27)
        TabPage04SummaryData.Name = "TabPage04SummaryData"
        TabPage04SummaryData.Padding = New Padding(3)
        TabPage04SummaryData.Size = New Size(1376, 661)
        TabPage04SummaryData.TabIndex = 0
        TabPage04SummaryData.Text = "Summary Data As Table"
        TabPage04SummaryData.UseVisualStyleBackColor = True
        ' 
        ' TabPage05Insulin
        ' 
        TabPage05Insulin.Controls.Add(TableLayoutPanelInsulin)
        TabPage05Insulin.Location = New Point(4, 27)
        TabPage05Insulin.Name = "TabPage05Insulin"
        TabPage05Insulin.Padding = New Padding(3)
        TabPage05Insulin.Size = New Size(1376, 661)
        TabPage05Insulin.TabIndex = 4
        TabPage05Insulin.Text = "Insulin"
        TabPage05Insulin.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelInsulin
        ' 
        TableLayoutPanelInsulin.AutoScroll = True
        TableLayoutPanelInsulin.AutoSize = True
        TableLayoutPanelInsulin.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelInsulin.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelInsulin.ColumnCount = 1
        TableLayoutPanelInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelInsulin.Controls.Add(Me.TableLayoutPanelInsulinTop, 0, 0)
        TableLayoutPanelInsulin.Controls.Add(DgvInsulin, 0, 1)
        TableLayoutPanelInsulin.Dock = DockStyle.Fill
        TableLayoutPanelInsulin.Location = New Point(3, 3)
        TableLayoutPanelInsulin.Name = "TableLayoutPanelInsulin"
        TableLayoutPanelInsulin.RowCount = 2
        TableLayoutPanelInsulin.RowStyles.Add(New RowStyle())
        TableLayoutPanelInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelInsulin.Size = New Size(1370, 655)
        TableLayoutPanelInsulin.TabIndex = 1
        ' 
        ' TableLayoutPanelInsulinTop
        ' 
        Me.TableLayoutPanelInsulinTop.AutoSize = True
        Me.TableLayoutPanelInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelInsulinTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelInsulinTop.ColumnCount = 2
        Me.TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage06Meal.Controls.Add(TableLayoutPanelMeal)
        TabPage06Meal.Location = New Point(4, 27)
        TabPage06Meal.Name = "TabPage06Meal"
        TabPage06Meal.Padding = New Padding(3)
        TabPage06Meal.Size = New Size(1376, 661)
        TabPage06Meal.TabIndex = 6
        TabPage06Meal.Text = "Meal"
        TabPage06Meal.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelMeal
        ' 
        TableLayoutPanelMeal.AutoScroll = True
        TableLayoutPanelMeal.AutoSize = True
        TableLayoutPanelMeal.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelMeal.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelMeal.ColumnCount = 1
        TableLayoutPanelMeal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelMeal.Controls.Add(Me.TableLayoutPanelMealTop, 0, 0)
        TableLayoutPanelMeal.Controls.Add(DgvMeal, 0, 1)
        TableLayoutPanelMeal.Dock = DockStyle.Fill
        TableLayoutPanelMeal.Location = New Point(3, 3)
        TableLayoutPanelMeal.Name = "TableLayoutPanelMeal"
        TableLayoutPanelMeal.RowCount = 2
        TableLayoutPanelMeal.RowStyles.Add(New RowStyle())
        TableLayoutPanelMeal.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelMeal.Size = New Size(1370, 655)
        TableLayoutPanelMeal.TabIndex = 1
        ' 
        ' TableLayoutPanelMealTop
        ' 
        Me.TableLayoutPanelMealTop.AutoSize = True
        Me.TableLayoutPanelMealTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelMealTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelMealTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelMealTop.ColumnCount = 2
        Me.TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage07ActiveInsulin.Controls.Add(TableLayoutPanelActiveInsulin)
        TabPage07ActiveInsulin.Location = New Point(4, 27)
        TabPage07ActiveInsulin.Name = "TabPage07ActiveInsulin"
        TabPage07ActiveInsulin.Padding = New Padding(3)
        TabPage07ActiveInsulin.Size = New Size(1376, 661)
        TabPage07ActiveInsulin.TabIndex = 18
        TabPage07ActiveInsulin.Text = "Active Insulin"
        TabPage07ActiveInsulin.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelActiveInsulin
        ' 
        TableLayoutPanelActiveInsulin.AutoSize = True
        TableLayoutPanelActiveInsulin.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelActiveInsulin.ColumnCount = 1
        TableLayoutPanelActiveInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelActiveInsulin.Controls.Add(Me.TableLayoutPanelActiveInsulinTop, 0, 0)
        TableLayoutPanelActiveInsulin.Dock = DockStyle.Fill
        TableLayoutPanelActiveInsulin.Location = New Point(3, 3)
        TableLayoutPanelActiveInsulin.Name = "TableLayoutPanelActiveInsulin"
        TableLayoutPanelActiveInsulin.RowCount = 2
        TableLayoutPanelActiveInsulin.RowStyles.Add(New RowStyle())
        TableLayoutPanelActiveInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelActiveInsulin.Size = New Size(1370, 655)
        TableLayoutPanelActiveInsulin.TabIndex = 0
        ' 
        ' TableLayoutPanelActiveInsulinTop
        ' 
        Me.TableLayoutPanelActiveInsulinTop.AutoSize = True
        Me.TableLayoutPanelActiveInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelActiveInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelActiveInsulinTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelActiveInsulinTop.ColumnCount = 2
        Me.TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage08SensorGlucose.Controls.Add(TableLayoutPanelSgs)
        TabPage08SensorGlucose.Location = New Point(4, 27)
        TabPage08SensorGlucose.Name = "TabPage08SensorGlucose"
        TabPage08SensorGlucose.Padding = New Padding(3)
        TabPage08SensorGlucose.Size = New Size(1376, 661)
        TabPage08SensorGlucose.TabIndex = 19
        TabPage08SensorGlucose.Text = "Sensor Glucose"
        TabPage08SensorGlucose.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelSgs
        ' 
        TableLayoutPanelSgs.AutoSize = True
        TableLayoutPanelSgs.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelSgs.ColumnCount = 1
        TableLayoutPanelSgs.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelSgs.Controls.Add(Me.TableLayoutPanelSgsTop, 0, 0)
        TableLayoutPanelSgs.Controls.Add(DgvSGs, 0, 1)
        TableLayoutPanelSgs.Dock = DockStyle.Fill
        TableLayoutPanelSgs.Location = New Point(3, 3)
        TableLayoutPanelSgs.Name = "TableLayoutPanelSgs"
        TableLayoutPanelSgs.RowCount = 2
        TableLayoutPanelSgs.RowStyles.Add(New RowStyle())
        TableLayoutPanelSgs.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelSgs.Size = New Size(1370, 655)
        TableLayoutPanelSgs.TabIndex = 1
        ' 
        ' TableLayoutPanelSgsTop
        ' 
        Me.TableLayoutPanelSgsTop.AutoSize = True
        Me.TableLayoutPanelSgsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelSgsTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelSgsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelSgsTop.ColumnCount = 2
        Me.TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage09Limits.Controls.Add(TableLayoutPanelLimits)
        TabPage09Limits.Location = New Point(4, 27)
        TabPage09Limits.Name = "TabPage09Limits"
        TabPage09Limits.Padding = New Padding(3)
        TabPage09Limits.Size = New Size(1376, 661)
        TabPage09Limits.TabIndex = 20
        TabPage09Limits.Text = "Limits"
        TabPage09Limits.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelLimits
        ' 
        TableLayoutPanelLimits.AutoSize = True
        TableLayoutPanelLimits.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLimits.ColumnCount = 1
        TableLayoutPanelLimits.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelLimits.Controls.Add(Me.TableLayoutPanelLimitsTop, 0, 0)
        TableLayoutPanelLimits.Dock = DockStyle.Fill
        TableLayoutPanelLimits.Location = New Point(3, 3)
        TableLayoutPanelLimits.Name = "TableLayoutPanelLimits"
        TableLayoutPanelLimits.RowCount = 2
        TableLayoutPanelLimits.RowStyles.Add(New RowStyle())
        TableLayoutPanelLimits.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelLimits.Size = New Size(1370, 655)
        TableLayoutPanelLimits.TabIndex = 0
        ' 
        ' TableLayoutPanelLimitsTop
        ' 
        Me.TableLayoutPanelLimitsTop.AutoSize = True
        Me.TableLayoutPanelLimitsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLimitsTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLimitsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLimitsTop.ColumnCount = 2
        Me.TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage10NotificationHistory.Controls.Add(TableLayoutPanelNotificationHistory)
        TabPage10NotificationHistory.Location = New Point(4, 27)
        TabPage10NotificationHistory.Name = "TabPage10NotificationHistory"
        TabPage10NotificationHistory.Padding = New Padding(3)
        TabPage10NotificationHistory.Size = New Size(1376, 661)
        TabPage10NotificationHistory.TabIndex = 5
        TabPage10NotificationHistory.Text = "Notification History"
        TabPage10NotificationHistory.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelNotificationHistory
        ' 
        TableLayoutPanelNotificationHistory.AutoScroll = True
        TableLayoutPanelNotificationHistory.AutoSize = True
        TableLayoutPanelNotificationHistory.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelNotificationHistory.ColumnCount = 1
        TableLayoutPanelNotificationHistory.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelNotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistoryTop, 1, 0)
        TableLayoutPanelNotificationHistory.Dock = DockStyle.Fill
        TableLayoutPanelNotificationHistory.Location = New Point(3, 3)
        TableLayoutPanelNotificationHistory.Name = "TableLayoutPanelNotificationHistory"
        TableLayoutPanelNotificationHistory.RowCount = 2
        TableLayoutPanelNotificationHistory.RowStyles.Add(New RowStyle())
        TableLayoutPanelNotificationHistory.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelNotificationHistory.Size = New Size(1370, 655)
        TableLayoutPanelNotificationHistory.TabIndex = 0
        ' 
        ' TableLayoutPanelNotificationHistoryTop
        ' 
        Me.TableLayoutPanelNotificationHistoryTop.AutoSize = True
        Me.TableLayoutPanelNotificationHistoryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelNotificationHistoryTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelNotificationHistoryTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelNotificationHistoryTop.ColumnCount = 2
        Me.TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage11TherapyAlgorithm.Controls.Add(TableLayoutPanelTherapyAlgorithm)
        TabPage11TherapyAlgorithm.Location = New Point(4, 27)
        TabPage11TherapyAlgorithm.Name = "TabPage11TherapyAlgorithm"
        TabPage11TherapyAlgorithm.Padding = New Padding(3)
        TabPage11TherapyAlgorithm.Size = New Size(1376, 661)
        TabPage11TherapyAlgorithm.TabIndex = 21
        TabPage11TherapyAlgorithm.Text = "Therapy Algorithm"
        TabPage11TherapyAlgorithm.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelTherapyAlgorithm
        ' 
        TableLayoutPanelTherapyAlgorithm.AutoSize = True
        TableLayoutPanelTherapyAlgorithm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelTherapyAlgorithm.ColumnCount = 1
        TableLayoutPanelTherapyAlgorithm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelTherapyAlgorithm.Controls.Add(Me.TableLayoutPanelTherapyAlgorithmTop, 0, 0)
        TableLayoutPanelTherapyAlgorithm.Dock = DockStyle.Fill
        TableLayoutPanelTherapyAlgorithm.Location = New Point(3, 3)
        TableLayoutPanelTherapyAlgorithm.Name = "TableLayoutPanelTherapyAlgorithm"
        TableLayoutPanelTherapyAlgorithm.RowCount = 2
        TableLayoutPanelTherapyAlgorithm.RowStyles.Add(New RowStyle())
        TableLayoutPanelTherapyAlgorithm.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelTherapyAlgorithm.Size = New Size(1370, 655)
        TableLayoutPanelTherapyAlgorithm.TabIndex = 0
        ' 
        ' TableLayoutPanelTherapyAlgorithmTop
        ' 
        Me.TableLayoutPanelTherapyAlgorithmTop.AutoSize = True
        Me.TableLayoutPanelTherapyAlgorithmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTherapyAlgorithmTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelTherapyAlgorithmTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTherapyAlgorithmTop.ColumnCount = 2
        Me.TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage12BannerState.Controls.Add(TableLayoutPanelBannerState)
        TabPage12BannerState.Location = New Point(4, 27)
        TabPage12BannerState.Name = "TabPage12BannerState"
        TabPage12BannerState.Padding = New Padding(3)
        TabPage12BannerState.Size = New Size(1376, 661)
        TabPage12BannerState.TabIndex = 22
        TabPage12BannerState.Text = "Banner State"
        TabPage12BannerState.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelBannerState
        ' 
        TableLayoutPanelBannerState.AutoSize = True
        TableLayoutPanelBannerState.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelBannerState.ColumnCount = 1
        TableLayoutPanelBannerState.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelBannerState.Controls.Add(Me.TableLayoutPanelBannerStateTop, 0, 0)
        TableLayoutPanelBannerState.Dock = DockStyle.Fill
        TableLayoutPanelBannerState.Location = New Point(3, 3)
        TableLayoutPanelBannerState.Margin = New Padding(0)
        TableLayoutPanelBannerState.Name = "TableLayoutPanelBannerState"
        TableLayoutPanelBannerState.RowCount = 2
        TableLayoutPanelBannerState.RowStyles.Add(New RowStyle())
        TableLayoutPanelBannerState.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelBannerState.Size = New Size(1370, 655)
        TableLayoutPanelBannerState.TabIndex = 0
        ' 
        ' TableLayoutPanelBannerStateTop
        ' 
        Me.TableLayoutPanelBannerStateTop.AutoSize = True
        Me.TableLayoutPanelBannerStateTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBannerStateTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelBannerStateTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBannerStateTop.ColumnCount = 2
        Me.TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage13Basal.Controls.Add(TableLayoutPanelBasal)
        TabPage13Basal.Location = New Point(4, 27)
        TabPage13Basal.Name = "TabPage13Basal"
        TabPage13Basal.Padding = New Padding(3)
        TabPage13Basal.Size = New Size(1376, 661)
        TabPage13Basal.TabIndex = 23
        TabPage13Basal.Text = "Basal"
        TabPage13Basal.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelBasal
        ' 
        TableLayoutPanelBasal.AutoScroll = True
        TableLayoutPanelBasal.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelBasal.ColumnCount = 1
        TableLayoutPanelBasal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelBasal.Controls.Add(Me.TableLayoutPanelBasalTop, 0, 0)
        TableLayoutPanelBasal.Dock = DockStyle.Fill
        TableLayoutPanelBasal.Location = New Point(3, 3)
        TableLayoutPanelBasal.Name = "TableLayoutPanelBasal"
        TableLayoutPanelBasal.RowCount = 2
        TableLayoutPanelBasal.RowStyles.Add(New RowStyle())
        TableLayoutPanelBasal.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelBasal.Size = New Size(1370, 655)
        TableLayoutPanelBasal.TabIndex = 0
        ' 
        ' TableLayoutPanelBasalTop
        ' 
        Me.TableLayoutPanelBasalTop.AutoSize = True
        Me.TableLayoutPanelBasalTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBasalTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelBasalTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBasalTop.ColumnCount = 2
        Me.TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPage14Markers.BackColor = SystemColors.MenuHighlight
        TabPage14Markers.Location = New Point(4, 27)
        TabPage14Markers.Name = "TabPage14Markers"
        TabPage14Markers.Padding = New Padding(3)
        TabPage14Markers.Size = New Size(1376, 661)
        TabPage14Markers.TabIndex = 24
        TabPage14Markers.Text = "More..."
        ' 
        ' TabPageLastSG
        ' 
        TabPageLastSG.Controls.Add(TableLayoutPanelLastSG)
        TabPageLastSG.Location = New Point(4, 27)
        TabPageLastSG.Name = "TabPageLastSG"
        TabPageLastSG.Padding = New Padding(3)
        TabPageLastSG.Size = New Size(1376, 661)
        TabPageLastSG.TabIndex = 16
        TabPageLastSG.Text = "Last SG"
        TabPageLastSG.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelLastSG
        ' 
        TableLayoutPanelLastSG.AutoSize = True
        TableLayoutPanelLastSG.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLastSG.ColumnCount = 1
        TableLayoutPanelLastSG.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelLastSG.Controls.Add(Me.TableLayoutPanelLastSgTop, 0, 0)
        TableLayoutPanelLastSG.Dock = DockStyle.Fill
        TableLayoutPanelLastSG.Location = New Point(3, 3)
        TableLayoutPanelLastSG.Name = "TableLayoutPanelLastSG"
        TableLayoutPanelLastSG.RowCount = 2
        TableLayoutPanelLastSG.RowStyles.Add(New RowStyle())
        TableLayoutPanelLastSG.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelLastSG.Size = New Size(1370, 655)
        TableLayoutPanelLastSG.TabIndex = 1
        ' 
        ' TableLayoutPanelLastSgTop
        ' 
        Me.TableLayoutPanelLastSgTop.AutoSize = True
        Me.TableLayoutPanelLastSgTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastSgTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLastSgTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLastSgTop.ColumnCount = 2
        Me.TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPageLastAlarm.Controls.Add(TableLayoutPanelLastAlarm)
        TabPageLastAlarm.Location = New Point(4, 27)
        TabPageLastAlarm.Name = "TabPageLastAlarm"
        TabPageLastAlarm.Padding = New Padding(3)
        TabPageLastAlarm.Size = New Size(1376, 661)
        TabPageLastAlarm.TabIndex = 17
        TabPageLastAlarm.Text = "Last Alarm"
        TabPageLastAlarm.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelLastAlarm
        ' 
        TableLayoutPanelLastAlarm.AutoSize = True
        TableLayoutPanelLastAlarm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLastAlarm.ColumnCount = 1
        TableLayoutPanelLastAlarm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelLastAlarm.Controls.Add(Me.TableLayoutPanelLastAlarmTop, 0, 0)
        TableLayoutPanelLastAlarm.Dock = DockStyle.Fill
        TableLayoutPanelLastAlarm.Location = New Point(3, 3)
        TableLayoutPanelLastAlarm.Margin = New Padding(0)
        TableLayoutPanelLastAlarm.Name = "TableLayoutPanelLastAlarm"
        TableLayoutPanelLastAlarm.RowCount = 2
        TableLayoutPanelLastAlarm.RowStyles.Add(New RowStyle())
        TableLayoutPanelLastAlarm.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelLastAlarm.Size = New Size(1370, 655)
        TableLayoutPanelLastAlarm.TabIndex = 0
        ' 
        ' TableLayoutPanelLastAlarmTop
        ' 
        Me.TableLayoutPanelLastAlarmTop.AutoSize = True
        Me.TableLayoutPanelLastAlarmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastAlarmTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLastAlarmTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLastAlarmTop.ColumnCount = 2
        Me.TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabControlPage2.Appearance = TabAppearance.Buttons
        TabControlPage2.Controls.Add(TabPageAutoBasalDelivery)
        TabControlPage2.Controls.Add(TabPageAutoModeStatus)
        TabControlPage2.Controls.Add(TabPageSgReadings)
        TabControlPage2.Controls.Add(TabPageCalibration)
        TabControlPage2.Controls.Add(TabPageLowGlucoseSuspended)
        TabControlPage2.Controls.Add(TabPageTimeChange)
        TabControlPage2.Controls.Add(TabPageLastSG)
        TabControlPage2.Controls.Add(TabPageLastAlarm)
        TabControlPage2.Controls.Add(TabPageCountryDataPg1)
        TabControlPage2.Controls.Add(TabPageCountryDataPg2)
        TabControlPage2.Controls.Add(TabPageCountryDataPg3)
        TabControlPage2.Controls.Add(TabPageSessionProfile)
        TabControlPage2.Controls.Add(TabPageCurrentUser)
        TabControlPage2.Controls.Add(TabPageAllUsers)
        TabControlPage2.Controls.Add(TabPageBackToHomePage)
        TabControlPage2.Dock = DockStyle.Fill
        TabControlPage2.Location = New Point(0, 24)
        TabControlPage2.Name = "TabControlPage2"
        TabControlPage2.SelectedIndex = 0
        TabControlPage2.Size = New Size(1384, 692)
        TabControlPage2.TabIndex = 0
        ' 
        ' TabPageAutoBasalDelivery
        ' 
        TabPageAutoBasalDelivery.Controls.Add(TableLayoutPanelAutoBasalDelivery)
        TabPageAutoBasalDelivery.Location = New Point(4, 27)
        TabPageAutoBasalDelivery.Name = "TabPageAutoBasalDelivery"
        TabPageAutoBasalDelivery.Padding = New Padding(3)
        TabPageAutoBasalDelivery.Size = New Size(1376, 661)
        TabPageAutoBasalDelivery.TabIndex = 1
        TabPageAutoBasalDelivery.Text = "Auto Basal Delivery"
        TabPageAutoBasalDelivery.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelAutoBasalDelivery
        ' 
        TableLayoutPanelAutoBasalDelivery.AutoSize = True
        TableLayoutPanelAutoBasalDelivery.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelAutoBasalDelivery.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelAutoBasalDelivery.ColumnCount = 1
        TableLayoutPanelAutoBasalDelivery.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelAutoBasalDelivery.Controls.Add(Me.TableLayoutPanelAutoBasalDeliveryTop, 0, 0)
        TableLayoutPanelAutoBasalDelivery.Controls.Add(DgvAutoBasalDelivery, 0, 1)
        TableLayoutPanelAutoBasalDelivery.Dock = DockStyle.Fill
        TableLayoutPanelAutoBasalDelivery.Location = New Point(3, 3)
        TableLayoutPanelAutoBasalDelivery.Name = "TableLayoutPanelAutoBasalDelivery"
        TableLayoutPanelAutoBasalDelivery.RowCount = 2
        TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New RowStyle())
        TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelAutoBasalDelivery.Size = New Size(1370, 655)
        TableLayoutPanelAutoBasalDelivery.TabIndex = 0
        ' 
        ' TableLayoutPanelAutoBasalDeliveryTop
        ' 
        Me.TableLayoutPanelAutoBasalDeliveryTop.AutoSize = True
        Me.TableLayoutPanelAutoBasalDeliveryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoBasalDeliveryTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelAutoBasalDeliveryTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoBasalDeliveryTop.ColumnCount = 2
        Me.TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPageAutoModeStatus.Controls.Add(TableLayoutPanelAutoModeStatus)
        TabPageAutoModeStatus.Location = New Point(4, 27)
        TabPageAutoModeStatus.Name = "TabPageAutoModeStatus"
        TabPageAutoModeStatus.Padding = New Padding(3)
        TabPageAutoModeStatus.Size = New Size(1376, 661)
        TabPageAutoModeStatus.TabIndex = 0
        TabPageAutoModeStatus.Text = "Auto Mode Status"
        TabPageAutoModeStatus.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelAutoModeStatus
        ' 
        TableLayoutPanelAutoModeStatus.AutoScroll = True
        TableLayoutPanelAutoModeStatus.AutoSize = True
        TableLayoutPanelAutoModeStatus.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelAutoModeStatus.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelAutoModeStatus.ColumnCount = 1
        TableLayoutPanelAutoModeStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelAutoModeStatus.Controls.Add(Me.TableLayoutPanelAutoModeStatusTop, 0, 0)
        TableLayoutPanelAutoModeStatus.Dock = DockStyle.Fill
        TableLayoutPanelAutoModeStatus.Location = New Point(3, 3)
        TableLayoutPanelAutoModeStatus.Name = "TableLayoutPanelAutoModeStatus"
        TableLayoutPanelAutoModeStatus.RowCount = 2
        TableLayoutPanelAutoModeStatus.RowStyles.Add(New RowStyle())
        TableLayoutPanelAutoModeStatus.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelAutoModeStatus.Size = New Size(1370, 655)
        TableLayoutPanelAutoModeStatus.TabIndex = 0
        ' 
        ' TableLayoutPanelAutoModeStatusTop
        ' 
        Me.TableLayoutPanelAutoModeStatusTop.AutoSize = True
        Me.TableLayoutPanelAutoModeStatusTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoModeStatusTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelAutoModeStatusTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoModeStatusTop.ColumnCount = 2
        Me.TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        Me.TableLayoutPanelAutoModeStatusTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelAutoModeStatusTop.LabelText = "Auto Mode Status"
        Me.TableLayoutPanelAutoModeStatusTop.Location = New Point(6, 6)
        Me.TableLayoutPanelAutoModeStatusTop.Name = "TableLayoutPanelAutoModeStatusTop"
        Me.TableLayoutPanelAutoModeStatusTop.RowCount = 1
        Me.TableLayoutPanelAutoModeStatusTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelAutoModeStatusTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelAutoModeStatusTop.TabIndex = 1
        ' 
        ' TabPageSgReadings
        ' 
        TabPageSgReadings.Controls.Add(TableLayoutPanelSgReadings)
        TabPageSgReadings.Location = New Point(4, 27)
        TabPageSgReadings.Name = "TabPageSgReadings"
        TabPageSgReadings.Padding = New Padding(3)
        TabPageSgReadings.Size = New Size(1376, 661)
        TabPageSgReadings.TabIndex = 2
        TabPageSgReadings.Text = "Sg Readings"
        TabPageSgReadings.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelSgReadings
        ' 
        TableLayoutPanelSgReadings.AutoScroll = True
        TableLayoutPanelSgReadings.AutoSize = True
        TableLayoutPanelSgReadings.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelSgReadings.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelSgReadings.ColumnCount = 1
        TableLayoutPanelSgReadings.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelSgReadings.Controls.Add(Me.TableLayoutPanelSgReadingsTop, 0, 0)
        TableLayoutPanelSgReadings.Dock = DockStyle.Fill
        TableLayoutPanelSgReadings.Location = New Point(3, 3)
        TableLayoutPanelSgReadings.Name = "TableLayoutPanelSgReadings"
        TableLayoutPanelSgReadings.RowCount = 2
        TableLayoutPanelSgReadings.RowStyles.Add(New RowStyle())
        TableLayoutPanelSgReadings.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelSgReadings.Size = New Size(1370, 655)
        TableLayoutPanelSgReadings.TabIndex = 1
        ' 
        ' TableLayoutPanelSgReadingsTop
        ' 
        Me.TableLayoutPanelSgReadingsTop.AutoSize = True
        Me.TableLayoutPanelSgReadingsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelSgReadingsTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelSgReadingsTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelSgReadingsTop.ColumnCount = 2
        Me.TableLayoutPanelSgReadingsTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelSgReadingsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        Me.TableLayoutPanelSgReadingsTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelSgReadingsTop.LabelText = "Sg Readings"
        Me.TableLayoutPanelSgReadingsTop.Location = New Point(6, 6)
        Me.TableLayoutPanelSgReadingsTop.Name = "TableLayoutPanelSgReadingsTop"
        Me.TableLayoutPanelSgReadingsTop.RowCount = 1
        Me.TableLayoutPanelSgReadingsTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelSgReadingsTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelSgReadingsTop.TabIndex = 1
        ' 
        ' TabPageCalibration
        ' 
        TabPageCalibration.Controls.Add(TableLayoutPanelCalibration)
        TabPageCalibration.Location = New Point(4, 27)
        TabPageCalibration.Name = "TabPageCalibration"
        TabPageCalibration.Padding = New Padding(3)
        TabPageCalibration.Size = New Size(1376, 661)
        TabPageCalibration.TabIndex = 3
        TabPageCalibration.Text = "Calibration"
        TabPageCalibration.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelCalibration
        ' 
        TableLayoutPanelCalibration.AutoScroll = True
        TableLayoutPanelCalibration.AutoSize = True
        TableLayoutPanelCalibration.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelCalibration.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelCalibration.ColumnCount = 1
        TableLayoutPanelCalibration.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelCalibration.Controls.Add(Me.TableLayoutPanelCalibrationTop, 0, 0)
        TableLayoutPanelCalibration.Dock = DockStyle.Fill
        TableLayoutPanelCalibration.Location = New Point(3, 3)
        TableLayoutPanelCalibration.Name = "TableLayoutPanelCalibration"
        TableLayoutPanelCalibration.RowCount = 2
        TableLayoutPanelCalibration.RowStyles.Add(New RowStyle())
        TableLayoutPanelCalibration.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelCalibration.Size = New Size(1370, 655)
        TableLayoutPanelCalibration.TabIndex = 1
        ' 
        ' TableLayoutPanelCalibrationTop
        ' 
        Me.TableLayoutPanelCalibrationTop.AutoSize = True
        Me.TableLayoutPanelCalibrationTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelCalibrationTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelCalibrationTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelCalibrationTop.ColumnCount = 2
        Me.TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        Me.TableLayoutPanelCalibrationTop.Dock = DockStyle.Fill
        Me.TableLayoutPanelCalibrationTop.LabelText = "Calibration"
        Me.TableLayoutPanelCalibrationTop.Location = New Point(6, 6)
        Me.TableLayoutPanelCalibrationTop.Name = "TableLayoutPanelCalibrationTop"
        Me.TableLayoutPanelCalibrationTop.RowCount = 1
        Me.TableLayoutPanelCalibrationTop.RowStyles.Add(New RowStyle())
        Me.TableLayoutPanelCalibrationTop.Size = New Size(1358, 37)
        Me.TableLayoutPanelCalibrationTop.TabIndex = 1
        ' 
        ' TabPageLowGlucoseSuspended
        ' 
        TabPageLowGlucoseSuspended.Controls.Add(TableLayoutPanelLowGlucoseSuspended)
        TabPageLowGlucoseSuspended.Location = New Point(4, 27)
        TabPageLowGlucoseSuspended.Name = "TabPageLowGlucoseSuspended"
        TabPageLowGlucoseSuspended.Padding = New Padding(3)
        TabPageLowGlucoseSuspended.Size = New Size(1376, 661)
        TabPageLowGlucoseSuspended.TabIndex = 5
        TabPageLowGlucoseSuspended.Text = "Low Glucose Suspended"
        TabPageLowGlucoseSuspended.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelLowGlucoseSuspended
        ' 
        TableLayoutPanelLowGlucoseSuspended.AutoScroll = True
        TableLayoutPanelLowGlucoseSuspended.AutoSize = True
        TableLayoutPanelLowGlucoseSuspended.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLowGlucoseSuspended.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelLowGlucoseSuspended.ColumnCount = 1
        TableLayoutPanelLowGlucoseSuspended.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelLowGlucoseSuspended.Controls.Add(Me.TableLayoutPanelLowGlucoseSuspendedTop, 0, 0)
        TableLayoutPanelLowGlucoseSuspended.Dock = DockStyle.Fill
        TableLayoutPanelLowGlucoseSuspended.Location = New Point(3, 3)
        TableLayoutPanelLowGlucoseSuspended.Name = "TableLayoutPanelLowGlucoseSuspended"
        TableLayoutPanelLowGlucoseSuspended.RowCount = 2
        TableLayoutPanelLowGlucoseSuspended.RowStyles.Add(New RowStyle())
        TableLayoutPanelLowGlucoseSuspended.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelLowGlucoseSuspended.Size = New Size(1370, 655)
        TableLayoutPanelLowGlucoseSuspended.TabIndex = 1
        ' 
        ' TableLayoutPanelLowGlucoseSuspendedTop
        ' 
        Me.TableLayoutPanelLowGlucoseSuspendedTop.AutoSize = True
        Me.TableLayoutPanelLowGlucoseSuspendedTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelLowGlucoseSuspendedTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ColumnCount = 2
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPageTimeChange.Controls.Add(TableLayoutPanelTimeChange)
        TabPageTimeChange.Location = New Point(4, 27)
        TabPageTimeChange.Name = "TabPageTimeChange"
        TabPageTimeChange.Padding = New Padding(3)
        TabPageTimeChange.Size = New Size(1376, 661)
        TabPageTimeChange.TabIndex = 7
        TabPageTimeChange.Text = "Time Change"
        TabPageTimeChange.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelTimeChange
        ' 
        TableLayoutPanelTimeChange.AutoScroll = True
        TableLayoutPanelTimeChange.AutoSize = True
        TableLayoutPanelTimeChange.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelTimeChange.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelTimeChange.ColumnCount = 1
        TableLayoutPanelTimeChange.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanelTimeChange.Controls.Add(Me.TableLayoutPanelTimeChangeTop, 0, 0)
        TableLayoutPanelTimeChange.Dock = DockStyle.Top
        TableLayoutPanelTimeChange.Location = New Point(3, 3)
        TableLayoutPanelTimeChange.Name = "TableLayoutPanelTimeChange"
        TableLayoutPanelTimeChange.RowCount = 2
        TableLayoutPanelTimeChange.RowStyles.Add(New RowStyle())
        TableLayoutPanelTimeChange.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanelTimeChange.Size = New Size(1370, 52)
        TableLayoutPanelTimeChange.TabIndex = 1
        ' 
        ' TableLayoutPanelTimeChangeTop
        ' 
        Me.TableLayoutPanelTimeChangeTop.AutoSize = True
        Me.TableLayoutPanelTimeChangeTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTimeChangeTop.ButtonText = "Return To 'Summary Data' Tab"
        Me.TableLayoutPanelTimeChangeTop.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTimeChangeTop.ColumnCount = 2
        Me.TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle())
        Me.TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
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
        TabPageCountryDataPg1.Controls.Add(DgvCountryDataPg1)
        TabPageCountryDataPg1.Location = New Point(4, 27)
        TabPageCountryDataPg1.Name = "TabPageCountryDataPg1"
        TabPageCountryDataPg1.Padding = New Padding(3)
        TabPageCountryDataPg1.Size = New Size(1376, 661)
        TabPageCountryDataPg1.TabIndex = 11
        TabPageCountryDataPg1.Text = "Country Data Pg1"
        TabPageCountryDataPg1.UseVisualStyleBackColor = True
        ' 
        ' TabPageCountryDataPg2
        ' 
        TabPageCountryDataPg2.Controls.Add(CountryDataPg2TableLayoutPanel)
        TabPageCountryDataPg2.Location = New Point(4, 27)
        TabPageCountryDataPg2.Name = "TabPageCountryDataPg2"
        TabPageCountryDataPg2.Padding = New Padding(3)
        TabPageCountryDataPg2.Size = New Size(1376, 661)
        TabPageCountryDataPg2.TabIndex = 11
        TabPageCountryDataPg2.Text = "Country Data Pg2"
        TabPageCountryDataPg2.UseVisualStyleBackColor = True
        ' 
        ' CountryDataPg2TableLayoutPanel
        ' 
        CountryDataPg2TableLayoutPanel.ColumnCount = 2
        CountryDataPg2TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 43.2116776F))
        CountryDataPg2TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 56.7883224F))
        CountryDataPg2TableLayoutPanel.Controls.Add(DgvCountryDataPg2, 0, 0)
        CountryDataPg2TableLayoutPanel.Controls.Add(Me.WebView, 1, 0)
        CountryDataPg2TableLayoutPanel.Dock = DockStyle.Fill
        CountryDataPg2TableLayoutPanel.Location = New Point(3, 3)
        CountryDataPg2TableLayoutPanel.Name = "CountryDataPg2TableLayoutPanel"
        CountryDataPg2TableLayoutPanel.RowCount = 1
        CountryDataPg2TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        CountryDataPg2TableLayoutPanel.Size = New Size(1370, 655)
        CountryDataPg2TableLayoutPanel.TabIndex = 2
        ' 
        ' DgvCountryDataPg2
        ' 
        DgvCountryDataPg2.Columns.AddRange(New DataGridViewColumn() {DgvCountryDataPg2RecordNumber, DgvCountryDataPg2Category, DgvCountryDataPg2Key, DgvCountryDataPg2Value})
        DgvCountryDataPg2.Location = New Point(3, 3)
        DgvCountryDataPg2.Name = "DgvCountryDataPg2"
        DgvCountryDataPg2.ReadOnly = True
        DgvCountryDataPg2.RowTemplate.Height = 25
        DgvCountryDataPg2.Size = New Size(583, 612)
        DgvCountryDataPg2.TabIndex = 2
        ' 
        ' DgvCountryDataPg2RecordNumber
        ' 
        DgvCountryDataPg2RecordNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        DgvCountryDataPg2RecordNumber.HeaderText = "Record Number"
        DgvCountryDataPg2RecordNumber.MinimumWidth = 60
        DgvCountryDataPg2RecordNumber.Name = "DgvCountryDataPg2RecordNumber"
        DgvCountryDataPg2RecordNumber.ReadOnly = True
        DgvCountryDataPg2RecordNumber.Width = 60
        ' 
        ' DgvCountryDataPg2Category
        ' 
        DgvCountryDataPg2Category.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg2Category.HeaderText = "Category"
        DgvCountryDataPg2Category.Name = "DgvCountryDataPg2Category"
        DgvCountryDataPg2Category.ReadOnly = True
        DgvCountryDataPg2Category.Width = 80
        ' 
        ' DgvCountryDataPg2Key
        ' 
        DgvCountryDataPg2Key.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DgvCountryDataPg2Key.HeaderText = "Key"
        DgvCountryDataPg2Key.Name = "DgvCountryDataPg2Key"
        DgvCountryDataPg2Key.ReadOnly = True
        DgvCountryDataPg2Key.Width = 51
        ' 
        ' DgvCountryDataPg2Value
        ' 
        DgvCountryDataPg2Value.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DgvCountryDataPg2Value.HeaderText = "Value"
        DgvCountryDataPg2Value.Name = "DgvCountryDataPg2Value"
        DgvCountryDataPg2Value.ReadOnly = True
        ' 
        ' WebView
        ' 
        Me.WebView.AllowExternalDrop = False
        Me.WebView.CreationProperties = Nothing
        Me.WebView.DefaultBackgroundColor = Color.White
        Me.WebView.Dock = DockStyle.Fill
        Me.WebView.Location = New Point(595, 3)
        Me.WebView.Name = "WebView"
        Me.WebView.Size = New Size(772, 649)
        Me.WebView.TabIndex = 3
        Me.WebView.ZoomFactor = 1R
        ' 
        ' TabPageCountryDataPg3
        ' 
        TabPageCountryDataPg3.Controls.Add(DgvCountryDataPg3)
        TabPageCountryDataPg3.Location = New Point(4, 27)
        TabPageCountryDataPg3.Name = "TabPageCountryDataPg3"
        TabPageCountryDataPg3.Padding = New Padding(3)
        TabPageCountryDataPg3.Size = New Size(1376, 661)
        TabPageCountryDataPg3.TabIndex = 11
        TabPageCountryDataPg3.Text = "Country Data Pg3"
        TabPageCountryDataPg3.UseVisualStyleBackColor = True
        ' 
        ' TabPageSessionProfile
        ' 
        TabPageSessionProfile.Controls.Add(DgvSessionProfile)
        TabPageSessionProfile.Location = New Point(4, 27)
        TabPageSessionProfile.Name = "TabPageSessionProfile"
        TabPageSessionProfile.Padding = New Padding(3)
        TabPageSessionProfile.Size = New Size(1376, 661)
        TabPageSessionProfile.TabIndex = 12
        TabPageSessionProfile.Text = "Session Profile"
        TabPageSessionProfile.UseVisualStyleBackColor = True
        ' 
        ' TabPageCurrentUser
        ' 
        TabPageCurrentUser.Controls.Add(DgvCurrentUser)
        TabPageCurrentUser.Location = New Point(4, 27)
        TabPageCurrentUser.Name = "TabPageCurrentUser"
        TabPageCurrentUser.Size = New Size(1376, 661)
        TabPageCurrentUser.TabIndex = 13
        TabPageCurrentUser.Text = "Current User"
        TabPageCurrentUser.UseVisualStyleBackColor = True
        ' 
        ' TabPageAllUsers
        ' 
        TabPageAllUsers.Controls.Add(DgvCareLinkUsers)
        TabPageAllUsers.Location = New Point(4, 27)
        TabPageAllUsers.Name = "TabPageAllUsers"
        TabPageAllUsers.Padding = New Padding(3)
        TabPageAllUsers.Size = New Size(1376, 661)
        TabPageAllUsers.TabIndex = 14
        TabPageAllUsers.Text = "All Users"
        TabPageAllUsers.UseVisualStyleBackColor = True
        ' 
        ' TabPageBackToHomePage
        ' 
        TabPageBackToHomePage.BackColor = SystemColors.MenuHighlight
        TabPageBackToHomePage.Location = New Point(4, 27)
        TabPageBackToHomePage.Name = "TabPageBackToHomePage"
        TabPageBackToHomePage.Padding = New Padding(3)
        TabPageBackToHomePage.Size = New Size(1376, 661)
        TabPageBackToHomePage.TabIndex = 8
        TabPageBackToHomePage.Text = "Back.."
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {LoginStatus, StatusStripSpeech, LastUpdateTimeToolStripStatusLabel, TimeZoneToolStripStatusLabel, StatusStripSpacerRight, UpdateAvailableStatusStripLabel})
        StatusStrip1.Location = New Point(0, 716)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(1384, 25)
        StatusStrip1.TabIndex = 53
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' LoginStatus
        ' 
        LoginStatus.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Right
        LoginStatus.BorderStyle = Border3DStyle.RaisedOuter
        LoginStatus.DisplayStyle = ToolStripItemDisplayStyle.Text
        LoginStatus.Name = "LoginStatus"
        LoginStatus.Size = New Size(133, 20)
        LoginStatus.Text = "Login Status: Unknown"
        ' 
        ' StatusStripSpeech
        ' 
        StatusStripSpeech.BorderSides = ToolStripStatusLabelBorderSides.Left
        StatusStripSpeech.BorderStyle = Border3DStyle.RaisedOuter
        StatusStripSpeech.Name = "StatusStripSpeech"
        StatusStripSpeech.Size = New Size(423, 20)
        StatusStripSpeech.Spring = True
        StatusStripSpeech.Text = " "
        ' 
        ' LastUpdateTimeToolStripStatusLabel
        ' 
        LastUpdateTimeToolStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
        LastUpdateTimeToolStripStatusLabel.BorderStyle = Border3DStyle.RaisedOuter
        LastUpdateTimeToolStripStatusLabel.Name = "LastUpdateTimeToolStripStatusLabel"
        LastUpdateTimeToolStripStatusLabel.Size = New Size(159, 20)
        LastUpdateTimeToolStripStatusLabel.Text = "Last Update Time: Unknown"
        ' 
        ' TimeZoneToolStripStatusLabel
        ' 
        TimeZoneToolStripStatusLabel.Name = "TimeZoneToolStripStatusLabel"
        TimeZoneToolStripStatusLabel.Size = New Size(114, 20)
        TimeZoneToolStripStatusLabel.Text = "TimeZone Unknown"
        ' 
        ' StatusStripSpacerRight
        ' 
        StatusStripSpacerRight.BorderSides = ToolStripStatusLabelBorderSides.Right
        StatusStripSpacerRight.BorderStyle = Border3DStyle.RaisedOuter
        StatusStripSpacerRight.Name = "StatusStripSpacerRight"
        StatusStripSpacerRight.Size = New Size(423, 20)
        StatusStripSpacerRight.Spring = True
        StatusStripSpacerRight.Text = " "
        ' 
        ' UpdateAvailableStatusStripLabel
        ' 
        UpdateAvailableStatusStripLabel.BorderSides = ToolStripStatusLabelBorderSides.Left Or ToolStripStatusLabelBorderSides.Right
        UpdateAvailableStatusStripLabel.BorderStyle = Border3DStyle.RaisedOuter
        UpdateAvailableStatusStripLabel.ForeColor = Color.Red
        UpdateAvailableStatusStripLabel.Image = My.Resources.Resources.NotificationAlertRed_16x
        UpdateAvailableStatusStripLabel.Name = "UpdateAvailableStatusStripLabel"
        UpdateAvailableStatusStripLabel.Size = New Size(116, 20)
        UpdateAvailableStatusStripLabel.Text = "Update Available"
        ' 
        ' Form1
        ' 
        Me.AutoScaleDimensions = New SizeF(96F, 96F)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.ClientSize = New Size(1384, 741)
        Me.Controls.Add(TabControlPage1)
        Me.Controls.Add(TabControlPage2)
        Me.Controls.Add(MenuStrip1)
        Me.Controls.Add(StatusStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Me.MainMenuStrip = MenuStrip1
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximumSize = New Size(1400, 960)
        Me.Name = "Form1"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "CareLink™ For Windows"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        CType(CalibrationDueImage, ComponentModel.ISupportInitialize).EndInit()
        CalibrationShieldPanel.ResumeLayout(False)
        CalibrationShieldPanel.PerformLayout()
        CType(CalibrationShieldPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).EndInit()
        CursorPanel.ResumeLayout(False)
        CType(CursorPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCountryDataPg1, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCareLinkUsers, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCountryDataPg3, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCurrentUser, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvInsulin, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvMeal, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSGs, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSummary, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSessionProfile, ComponentModel.ISupportInitialize).EndInit()
        CType(InsulinLevelPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Last24HTotalsPanel.ResumeLayout(False)
        CType(PumpBatteryPictureBox, ComponentModel.ISupportInitialize).EndInit()
        SensorTimeLeftPanel.ResumeLayout(False)
        CType(SensorTimeLeftPictureBox, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel1.PerformLayout()
        SplitContainer2.Panel2.ResumeLayout(False)
        CType(SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        CType(TransmitterBatteryPictureBox, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer3.Panel2.ResumeLayout(False)
        SplitContainer3.Panel2.PerformLayout()
        CType(SplitContainer3, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer3.ResumeLayout(False)
        TabControlPage1.ResumeLayout(False)
        TabPage01HomePage.ResumeLayout(False)
        TabPage02RunningIOB.ResumeLayout(False)
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel1.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        TabPage04SummaryData.ResumeLayout(False)
        TabPage05Insulin.ResumeLayout(False)
        TabPage05Insulin.PerformLayout()
        TableLayoutPanelInsulin.ResumeLayout(False)
        TableLayoutPanelInsulin.PerformLayout()
        TabPage06Meal.ResumeLayout(False)
        TabPage06Meal.PerformLayout()
        TableLayoutPanelMeal.ResumeLayout(False)
        TableLayoutPanelMeal.PerformLayout()
        TabPage07ActiveInsulin.ResumeLayout(False)
        TabPage07ActiveInsulin.PerformLayout()
        TableLayoutPanelActiveInsulin.ResumeLayout(False)
        TableLayoutPanelActiveInsulin.PerformLayout()
        TabPage08SensorGlucose.ResumeLayout(False)
        TabPage08SensorGlucose.PerformLayout()
        TableLayoutPanelSgs.ResumeLayout(False)
        TableLayoutPanelSgs.PerformLayout()
        TabPage09Limits.ResumeLayout(False)
        TabPage09Limits.PerformLayout()
        TableLayoutPanelLimits.ResumeLayout(False)
        TableLayoutPanelLimits.PerformLayout()
        TabPage10NotificationHistory.ResumeLayout(False)
        TabPage10NotificationHistory.PerformLayout()
        TableLayoutPanelNotificationHistory.ResumeLayout(False)
        TableLayoutPanelNotificationHistory.PerformLayout()
        TabPage11TherapyAlgorithm.ResumeLayout(False)
        TabPage11TherapyAlgorithm.PerformLayout()
        TableLayoutPanelTherapyAlgorithm.ResumeLayout(False)
        TableLayoutPanelTherapyAlgorithm.PerformLayout()
        TabPage12BannerState.ResumeLayout(False)
        TabPage12BannerState.PerformLayout()
        TableLayoutPanelBannerState.ResumeLayout(False)
        TableLayoutPanelBannerState.PerformLayout()
        TabPage13Basal.ResumeLayout(False)
        TableLayoutPanelBasal.ResumeLayout(False)
        TableLayoutPanelBasal.PerformLayout()
        TabPageLastSG.ResumeLayout(False)
        TabPageLastSG.PerformLayout()
        TableLayoutPanelLastSG.ResumeLayout(False)
        TableLayoutPanelLastSG.PerformLayout()
        TabPageLastAlarm.ResumeLayout(False)
        TabPageLastAlarm.PerformLayout()
        TableLayoutPanelLastAlarm.ResumeLayout(False)
        TableLayoutPanelLastAlarm.PerformLayout()
        TabControlPage2.ResumeLayout(False)
        TabPageAutoBasalDelivery.ResumeLayout(False)
        TabPageAutoBasalDelivery.PerformLayout()
        TableLayoutPanelAutoBasalDelivery.ResumeLayout(False)
        TableLayoutPanelAutoBasalDelivery.PerformLayout()
        TabPageAutoModeStatus.ResumeLayout(False)
        TabPageAutoModeStatus.PerformLayout()
        TableLayoutPanelAutoModeStatus.ResumeLayout(False)
        TableLayoutPanelAutoModeStatus.PerformLayout()
        TabPageSgReadings.ResumeLayout(False)
        TabPageSgReadings.PerformLayout()
        TableLayoutPanelSgReadings.ResumeLayout(False)
        TableLayoutPanelSgReadings.PerformLayout()
        TabPageCalibration.ResumeLayout(False)
        TabPageCalibration.PerformLayout()
        TableLayoutPanelCalibration.ResumeLayout(False)
        TableLayoutPanelCalibration.PerformLayout()
        TabPageLowGlucoseSuspended.ResumeLayout(False)
        TabPageLowGlucoseSuspended.PerformLayout()
        TableLayoutPanelLowGlucoseSuspended.ResumeLayout(False)
        TableLayoutPanelLowGlucoseSuspended.PerformLayout()
        TabPageTimeChange.ResumeLayout(False)
        TabPageTimeChange.PerformLayout()
        TableLayoutPanelTimeChange.ResumeLayout(False)
        TableLayoutPanelTimeChange.PerformLayout()
        TabPageCountryDataPg1.ResumeLayout(False)
        TabPageCountryDataPg2.ResumeLayout(False)
        CountryDataPg2TableLayoutPanel.ResumeLayout(False)
        CType(DgvCountryDataPg2, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WebView, ComponentModel.ISupportInitialize).EndInit()
        TabPageCountryDataPg3.ResumeLayout(False)
        TabPageSessionProfile.ResumeLayout(False)
        TabPageCurrentUser.ResumeLayout(False)
        TabPageAllUsers.ResumeLayout(False)
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
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
    Friend WithEvents CountryDataPg2TableLayoutPanel As TableLayoutPanel
    Friend WithEvents CurrentSgLabel As Label
    Friend WithEvents CursorMessage1Label As Label
    Friend WithEvents CursorMessage2Label As Label
    Friend WithEvents CursorMessage3Label As Label
    Friend WithEvents CursorMessage4Label As Label
    Friend WithEvents CursorPanel As Panel
    Friend WithEvents CursorPictureBox As PictureBox
    Friend WithEvents CursorTimer As Timer
    Friend WithEvents DgvAutoBasalDelivery As DataGridView
    Friend WithEvents DgvCareLinkUsers As DataGridView
    Friend WithEvents DgvCareLinkUsersUserPassword As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1 As DataGridView
    Friend WithEvents DgvCountryDataPg1Category As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1Key As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1RecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg1Value As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2 As DataGridView
    Friend WithEvents DgvCountryDataPg2Category As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2Key As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2RecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DgvCountryDataPg2Value As DataGridViewTextBoxColumn
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
    Friend WithEvents DgvSessionProfile As DataGridView
    Friend WithEvents DgvSGs As DataGridView
    Friend WithEvents DgvSummary As DataGridView
    Friend WithEvents FullNameLabel As Label
    Friend WithEvents HighTirComplianceLabel As Label
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents InRangeMessageLabel As Label
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents InsulinTypeLabel As Label
    Friend WithEvents LabelSgTrend As Label
    Friend WithEvents LabelTimeChange As Label
    Friend WithEvents LabelTrendArrows As Label
    Friend WithEvents LabelTrendValue As Label
    Friend WithEvents Last24AutoCorrectionLabel As Label
    Friend WithEvents Last24BasalLabel As Label
    Friend WithEvents Last24CarbsValueLabel As Label
    Friend WithEvents Last24DailyDoseLabel As Label
    Friend WithEvents Last24HoursGraphLabel As Label
    Friend WithEvents Last24HTotalsPanel As Panel
    Friend WithEvents Last24ManualBolusLabel As Label
    Friend WithEvents Last24TotalsLabel As Label
    Friend WithEvents LastSGTimeLabel As Label
    Friend WithEvents LastUpdateTimeToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents ListView1 As ListView
    Friend WithEvents LoginStatus As ToolStripStatusLabel
    Friend WithEvents LowTirComplianceLabel As Label
    Friend WithEvents MaxBasalPerHourLabel As Label
    Friend WithEvents MenuHelp As ToolStripMenuItem
    Friend WithEvents MenuHelpAbout As ToolStripMenuItem
    Friend WithEvents MenuHelpCheckForUpdates As ToolStripMenuItem
    Friend WithEvents MenuHelpReportAnIssue As ToolStripMenuItem
    Friend WithEvents MenuOptions As ToolStripMenuItem
    Friend WithEvents MenuOptionsAudioAlerts As ToolStripMenuItem
    Friend WithEvents MenuOptionsAutoLogin As ToolStripMenuItem
    Friend WithEvents MenuOptionsColorPicker As ToolStripMenuItem
    Friend WithEvents MenuOptionsEditPumpSettings As ToolStripMenuItem
    Friend WithEvents MenuOptionsFilterRawJSONData As ToolStripMenuItem
    Friend WithEvents MenuOptionsShowChartLegends As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognitionEnabled As ToolStripMenuItem
    Friend WithEvents MenuOptionsUseLocalTimeZone As ToolStripMenuItem
    Friend WithEvents MenuShowMiniDisplay As ToolStripMenuItem
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
    Friend WithEvents PumpAITLabel As Label
    Friend WithEvents PumpBatteryPictureBox As PictureBox
    Friend WithEvents PumpBatteryRemaining2Label As Label
    Friend WithEvents PumpBatteryRemainingLabel As Label
    Friend WithEvents PumpNameLabel As Label
    Friend WithEvents ReadingsLabel As Label
    Friend WithEvents RemainingInsulinUnits As Label
    Friend WithEvents SensorDaysLeftLabel As Label
    Friend WithEvents SensorMessage As Label
    Friend WithEvents SensorTimeLeftLabel As Label
    Friend WithEvents SensorTimeLeftPanel As Panel
    Friend WithEvents SensorTimeLeftPictureBox As PictureBox
    Friend WithEvents SerialNumberButton As Button
    Friend WithEvents ServerUpdateTimer As Timer
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents SmartGuardLabel As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents StartHereExit As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusStripSpacerRight As ToolStripStatusLabel
    Friend WithEvents StatusStripSpeech As ToolStripStatusLabel
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
    Friend WithEvents TableLayoutPanelSgReadings As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSgReadingsTop As TableLayputPanelTop.TableLayoutPanelTopEx
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
    Friend WithEvents TabPage05Insulin As TabPage
    Friend WithEvents TabPage06Meal As TabPage
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
    Friend WithEvents TabPageCalibration As TabPage
    Friend WithEvents TabPageCountryDataPg1 As TabPage
    Friend WithEvents TabPageCountryDataPg2 As TabPage
    Friend WithEvents TabPageCountryDataPg3 As TabPage
    Friend WithEvents TabPageCurrentUser As TabPage
    Friend WithEvents TabPageLastAlarm As TabPage
    Friend WithEvents TabPageLastSG As TabPage
    Friend WithEvents TabPageLowGlucoseSuspended As TabPage
    Friend WithEvents TabPageSessionProfile As TabPage
    Friend WithEvents TabPageSgReadings As TabPage
    Friend WithEvents TabPageTimeChange As TabPage
    Friend WithEvents TemporaryUseAdvanceAITDecayCheckBox As CheckBox
    Friend WithEvents TempTargetLabel As Label
    Friend WithEvents TimeInRangeChartLabel As Label
    Friend WithEvents TimeInRangeLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents TimeZoneToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents TirComplianceLabel As Label
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSplitButton1 As ToolStripSplitButton
    Friend WithEvents ToolStripSplitButton2 As ToolStripSplitButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TransmitterBatteryPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents UpdateAvailableStatusStripLabel As ToolStripStatusLabel
    Friend WithEvents WebView As Microsoft.Web.WebView2.WinForms.WebView2
End Class
