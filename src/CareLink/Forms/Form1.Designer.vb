Imports TableLayputPanelTop

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As DataGridViewCellStyle = New DataGridViewCellStyle()
        AboveHighLimitMessageLabel = New Label()
        AboveHighLimitValueLabel = New Label()
        ActiveInsulinValue = New Label()
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
        CareLinkUserDataRecordBindingSource = New BindingSource(components)
        CurrentSgLabel = New Label()
        CursorMessage1Label = New Label()
        CursorMessage2Label = New Label()
        CursorMessage3Label = New Label()
        CursorMessage4Label = New Label()
        CursorPanel = New Panel()
        CursorPictureBox = New PictureBox()
        CursorTimer = New Timer(components)
        DgvAutoBasalDelivery = New DataGridView()
        DgvCareLinkUsers = New DataGridView()
        DgvCurrentUser = New DataGridView()
        DgvInsulin = New DataGridView()
        DgvMeal = New DataGridView()
        DgvSGs = New DataGridView()
        DgvSummary = New DataGridView()
        FullNameLabel = New Label()
        HighTirComplianceLabel = New Label()
        ImageList1 = New ImageList(components)
        InRangeMessageLabel = New Label()
        InsulinLevelPictureBox = New PictureBox()
        InsulinTypeLabel = New Label()
        LabelSgTrend = New Label()
        LabelTimeChange = New Label()
        LabelTrendArrows = New Label()
        LabelTrendValue = New Label()
        Last24AutoCorrectionLabel = New Label()
        Last24AutoCorrectionPercentLabel = New Label()
        Last24AutoCorrectionUnitsLabel = New Label()
        Last24BasalLabel = New Label()
        Last24BasalPercentLabel = New Label()
        Last24BasalUnitsLabel = New Label()
        Last24CarbsLabel = New Label()
        Last24CarbsValueLabel = New Label()
        Last24HoursGraphLabel = New Label()
        Last24HTotalsPanel = New Panel()
        Last24ManualBolusPercentLabel = New Label()
        Last24ManualBolusUnitsLabel = New Label()
        Last24MealBolusLabel = New Label()
        Last24TotalInsulinLabel = New Label()
        Last24TotalInsulinUnitsLabel = New Label()
        Last24TotalsLabel = New Label()
        LastSgOrExitTimeLabel = New Label()
        LastUpdateTimeToolStripStatusLabel = New ToolStripStatusLabel()
        LoginStatus = New ToolStripStatusLabel()
        LowTirComplianceLabel = New Label()
        MaxBasalPerHourLabel = New Label()
        MenuHelp = New ToolStripMenuItem()
        MenuHelpAbout = New ToolStripMenuItem()
        MenuHelpCheckForUpdates = New ToolStripMenuItem()
        MenuHelpReportAnIssue = New ToolStripMenuItem()
        MenuOptions = New ToolStripMenuItem()
        MenuOptionsAdvancedOptions = New ToolStripMenuItem()
        MenuOptionsAudioAlerts = New ToolStripMenuItem()
        MenuOptionsAutoLogin = New ToolStripMenuItem()
        MenuOptionsColorPicker = New ToolStripMenuItem()
        MenuOptionsEditPumpSettings = New ToolStripMenuItem()
        MenuOptionsFilterRawJSONData = New ToolStripMenuItem()
        MenuOptionsShowChartLegends = New ToolStripMenuItem()
        MenuOptionsSpeechHelpShown = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition80 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition85 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition90 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition95 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionConfidence = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionDisabled = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionEnabled = New ToolStripMenuItem()
        MenuOptionsUseLocalTimeZone = New ToolStripMenuItem()
        MenuShowMiniDisplay = New ToolStripMenuItem()
        MenuStartHere = New ToolStripMenuItem()
        MenuStartHereCleanUpObsoleteFiles = New ToolStripMenuItem()
        MenuStartHereExceptionReportLoad = New ToolStripMenuItem()
        MenuStartHereExit = New ToolStripMenuItem()
        MenuStartHereLoadSavedDataFile = New ToolStripMenuItem()
        MenuStartHereLogin = New ToolStripMenuItem()
        MenuStartHereManuallyImportDeviceSettings = New ToolStripMenuItem()
        MenuStartHereShowPumpSetup = New ToolStripMenuItem()
        MenuStartHereSnapshotSave = New ToolStripMenuItem()
        MenuStartHereUseLastSavedFile = New ToolStripMenuItem()
        MenuStartHereUseTestData = New ToolStripMenuItem()
        MenuStrip1 = New MenuStrip()
        ModelLabel = New Label()
        NotifyIcon1 = New NotifyIcon(components)
        PumpAITLabel = New Label()
        PumpBannerStateLabel = New Label()
        PumpBatteryPictureBox = New PictureBox()
        PumpBatteryRemaining2Label = New Label()
        PumpBatteryRemainingLabel = New Label()
        PumpNameLabel = New Label()
        ReadingsLabel = New Label()
        RemainingInsulinUnits = New Label()
        SensorDaysLeftLabel = New Label()
        SensorMessageLabel = New Label()
        SensorTimeLeftLabel = New Label()
        SensorTimeLeftPanel = New Panel()
        SensorTimeLeftPictureBox = New PictureBox()
        SerialNumberButton = New Button()
        ServerUpdateTimer = New Timer(components)
        ShieldUnitsLabel = New Label()
        SmartGuardLabel = New Label()
        SmartGuardShieldPictureBox = New PictureBox()
        SplitContainer1 = New SplitContainer()
        SplitContainer2 = New SplitContainer()
        SplitContainer3 = New SplitContainer()
        StatusStrip1 = New StatusStrip()
        StatusStripSpacerRight = New ToolStripStatusLabel()
        StatusStripSpeech = New ToolStripStatusLabel()
        TabControlPage1 = New TabControl()
        TabControlPage2 = New TabControl()
        TableLayoutPanelActiveInsulin = New TableLayoutPanel()
        TableLayoutPanelActiveInsulinTop = New TableLayoutPanelTopEx()
        TableLayoutPanelAutoBasalDelivery = New TableLayoutPanel()
        TableLayoutPanelAutoBasalDeliveryTop = New TableLayoutPanelTopEx()
        TableLayoutPanelAutoModeStatus = New TableLayoutPanel()
        TableLayoutPanelAutoModeStatusTop = New TableLayoutPanelTopEx()
        TableLayoutPanelBannerState = New TableLayoutPanel()
        TableLayoutPanelBannerStateTop = New TableLayoutPanelTopEx()
        TableLayoutPanelBasal = New TableLayoutPanel()
        TableLayoutPanelBasalTop = New TableLayoutPanelTopEx()
        TableLayoutPanelBgReadingsTop = New TableLayoutPanelTopEx()
        TableLayoutPanelCalibration = New TableLayoutPanel()
        TableLayoutPanelCalibrationTop = New TableLayoutPanelTopEx()
        TableLayoutPanelInsulin = New TableLayoutPanel()
        TableLayoutPanelInsulinTop = New TableLayoutPanelTopEx()
        TableLayoutPanelLastAlarm = New TableLayoutPanel()
        TableLayoutPanelLastAlarmTop = New TableLayoutPanelTopEx()
        TableLayoutPanelLastSG = New TableLayoutPanel()
        TableLayoutPanelLastSgTop = New TableLayoutPanelTopEx()
        TableLayoutPanelLimits = New TableLayoutPanel()
        TableLayoutPanelLimitsTop = New TableLayoutPanelTopEx()
        TableLayoutPanelLowGlucoseSuspended = New TableLayoutPanel()
        TableLayoutPanelLowGlucoseSuspendedTop = New TableLayoutPanelTopEx()
        TableLayoutPanelMeal = New TableLayoutPanel()
        TableLayoutPanelMealTop = New TableLayoutPanelTopEx()
        TableLayoutPanelNotificationHistory = New TableLayoutPanel()
        TableLayoutPanelNotificationHistoryTop = New TableLayoutPanelTopEx()
        TableLayoutPanelSgReadings = New TableLayoutPanel()
        TableLayoutPanelSgs = New TableLayoutPanel()
        TableLayoutPanelSgsTop = New TableLayoutPanelTopEx()
        TableLayoutPanelTherapyAlgorithm = New TableLayoutPanel()
        TableLayoutPanelTherapyAlgorithmTop = New TableLayoutPanelTopEx()
        TableLayoutPanelTimeChange = New TableLayoutPanel()
        TableLayoutPanelTimeChangeTop = New TableLayoutPanelTopEx()
        TabPage01HomePage = New TabPage()
        TabPage02RunningIOB = New TabPage()
        TabPage03TreatmentDetails = New TabPage()
        TabPage04SummaryData = New TabPage()
        TabPage05Insulin = New TabPage()
        TabPage06Meal = New TabPage()
        TabPage07ActiveInsulin = New TabPage()
        TabPage08SensorGlucose = New TabPage()
        TabPage09Limits = New TabPage()
        TabPage10NotificationHistory = New TabPage()
        TabPage11TherapyAlgorithm = New TabPage()
        TabPage12BannerState = New TabPage()
        TabPage13Basal = New TabPage()
        TabPage14Markers = New TabPage()
        TabPageAllUsers = New TabPage()
        TabPageAutoBasalDelivery = New TabPage()
        TabPageAutoModeStatus = New TabPage()
        TabPageBackToHomePage = New TabPage()
        TabPageBgReadings = New TabPage()
        TabPageCalibration = New TabPage()
        TabPageCurrentUser = New TabPage()
        TabPageLastAlarm = New TabPage()
        TabPageLastSG = New TabPage()
        TabPageLowGlucoseSuspended = New TabPage()
        TabPageTimeChange = New TabPage()
        TemporaryUseAdvanceAITDecayCheckBox = New CheckBox()
        TimeInRangeChartLabel = New Label()
        TimeInRangeLabel = New Label()
        TimeInRangeSummaryPercentCharLabel = New Label()
        TimeInRangeValueLabel = New Label()
        TimeZoneToolStripStatusLabel = New ToolStripStatusLabel()
        TirComplianceLabel = New Label()
        ToolStripSeparator1 = New ToolStripSeparator()
        ToolStripSeparator2 = New ToolStripSeparator()
        ToolStripSeparator3 = New ToolStripSeparator()
        ToolStripSeparator4 = New ToolStripSeparator()
        ToolStripSeparator5 = New ToolStripSeparator()
        ToolStripSeparator6 = New ToolStripSeparator()
        ToolStripSeparator7 = New ToolStripSeparator()
        ToolStripSeparator8 = New ToolStripSeparator()
        ToolTip1 = New ToolTip(components)
        TransmitterBatteryPercentLabel = New Label()
        TransmitterBatteryPictureBox = New PictureBox()
        UpdateAvailableStatusStripLabel = New ToolStripStatusLabel()
        WebView = New Microsoft.Web.WebView2.WinForms.WebView2()
        MenuStrip1.SuspendLayout()
        CType(CalibrationDueImage, ComponentModel.ISupportInitialize).BeginInit()
        CalibrationShieldPanel.SuspendLayout()
        CType(SmartGuardShieldPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).BeginInit()
        CursorPanel.SuspendLayout()
        CType(CursorPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCareLinkUsers, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCurrentUser, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvInsulin, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvMeal, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSGs, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSummary, ComponentModel.ISupportInitialize).BeginInit()
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
        TabPage06Meal.SuspendLayout()
        TabPage07ActiveInsulin.SuspendLayout()
        TabPage08SensorGlucose.SuspendLayout()
        TabPage09Limits.SuspendLayout()
        TabPage10NotificationHistory.SuspendLayout()
        TabPage11TherapyAlgorithm.SuspendLayout()
        TabPage12BannerState.SuspendLayout()
        TabPage13Basal.SuspendLayout()
        TabPageLastSG.SuspendLayout()
        TabPageLastAlarm.SuspendLayout()
        TabControlPage2.SuspendLayout()
        TabPageAutoBasalDelivery.SuspendLayout()
        TabPageAutoModeStatus.SuspendLayout()
        TabPageBgReadings.SuspendLayout()
        TabPageCalibration.SuspendLayout()
        TabPageLowGlucoseSuspended.SuspendLayout()
        TabPageTimeChange.SuspendLayout()
        CType(WebView, ComponentModel.ISupportInitialize).BeginInit()
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
        MenuStartHere.DropDownItems.AddRange(New ToolStripItem() {MenuStartHereLogin, ToolStripSeparator8, MenuStartHereManuallyImportDeviceSettings, MenuStartHereShowPumpSetup, ToolStripSeparator1, MenuStartHereLoadSavedDataFile, MenuStartHereExceptionReportLoad, ToolStripSeparator4, MenuStartHereUseLastSavedFile, MenuStartHereUseTestData, ToolStripSeparator2, MenuStartHereSnapshotSave, MenuStartHereCleanUpObsoleteFiles, ToolStripSeparator3, MenuStartHereExit})
        MenuStartHere.Name = "MenuStartHere"
        MenuStartHere.Size = New Size(71, 20)
        MenuStartHere.Text = "Start Here"
        ' 
        ' MenuStartHereLogin
        ' 
        MenuStartHereLogin.Name = "MenuStartHereLogin"
        MenuStartHereLogin.Size = New Size(245, 22)
        MenuStartHereLogin.Text = "Login"
        ' 
        ' ToolStripSeparator8
        ' 
        ToolStripSeparator8.Name = "ToolStripSeparator8"
        ToolStripSeparator8.Size = New Size(242, 6)
        ' 
        ' MenuStartHereManuallyImportDeviceSettings
        ' 
        MenuStartHereManuallyImportDeviceSettings.Name = "MenuStartHereManuallyImportDeviceSettings"
        MenuStartHereManuallyImportDeviceSettings.Size = New Size(245, 22)
        MenuStartHereManuallyImportDeviceSettings.Text = "Manually Import Device Settings"
        ' 
        ' MenuStartHereShowPumpSetup
        ' 
        MenuStartHereShowPumpSetup.Enabled = False
        MenuStartHereShowPumpSetup.Name = "MenuStartHereShowPumpSetup"
        MenuStartHereShowPumpSetup.Size = New Size(245, 22)
        MenuStartHereShowPumpSetup.Text = "Show Pump Setup"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(242, 6)
        ' 
        ' MenuStartHereLoadSavedDataFile
        ' 
        MenuStartHereLoadSavedDataFile.Name = "MenuStartHereLoadSavedDataFile"
        MenuStartHereLoadSavedDataFile.Size = New Size(245, 22)
        MenuStartHereLoadSavedDataFile.Text = "Load A Saved Data File"
        ' 
        ' MenuStartHereExceptionReportLoad
        ' 
        MenuStartHereExceptionReportLoad.Name = "MenuStartHereExceptionReportLoad"
        MenuStartHereExceptionReportLoad.Size = New Size(245, 22)
        MenuStartHereExceptionReportLoad.Text = "Load An Exception Report"
        ' 
        ' ToolStripSeparator4
        ' 
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New Size(242, 6)
        ' 
        ' MenuStartHereUseLastSavedFile
        ' 
        MenuStartHereUseLastSavedFile.Name = "MenuStartHereUseLastSavedFile"
        MenuStartHereUseLastSavedFile.Size = New Size(245, 22)
        MenuStartHereUseLastSavedFile.Text = "Use Last Data File"
        ' 
        ' MenuStartHereUseTestData
        ' 
        MenuStartHereUseTestData.Name = "MenuStartHereUseTestData"
        MenuStartHereUseTestData.Size = New Size(245, 22)
        MenuStartHereUseTestData.Text = "Use Test Data"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(242, 6)
        ' 
        ' MenuStartHereSnapshotSave
        ' 
        MenuStartHereSnapshotSave.Name = "MenuStartHereSnapshotSave"
        MenuStartHereSnapshotSave.ShortcutKeys = Keys.Control Or Keys.S
        MenuStartHereSnapshotSave.Size = New Size(245, 22)
        MenuStartHereSnapshotSave.Text = "Snapshot &Save"
        ' 
        ' MenuStartHereCleanUpObsoleteFiles
        ' 
        MenuStartHereCleanUpObsoleteFiles.Name = "MenuStartHereCleanUpObsoleteFiles"
        MenuStartHereCleanUpObsoleteFiles.Size = New Size(245, 22)
        MenuStartHereCleanUpObsoleteFiles.Text = "Clean Up Obsolete Files"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(242, 6)
        ' 
        ' MenuStartHereExit
        ' 
        MenuStartHereExit.Image = My.Resources.Resources.AboutBox
        MenuStartHereExit.Name = "MenuStartHereExit"
        MenuStartHereExit.ShortcutKeys = Keys.Alt Or Keys.X
        MenuStartHereExit.Size = New Size(245, 22)
        MenuStartHereExit.Text = "E&xit"
        ' 
        ' MenuOptions
        ' 
        MenuOptions.DropDownItems.AddRange(New ToolStripItem() {MenuOptionsAudioAlerts, MenuOptionsSpeechRecognitionEnabled, MenuOptionsShowChartLegends, MenuOptionsSpeechHelpShown, ToolStripSeparator5, MenuOptionsAutoLogin, ToolStripSeparator6, MenuOptionsAdvancedOptions, MenuOptionsFilterRawJSONData, MenuOptionsUseLocalTimeZone, ToolStripSeparator7, MenuOptionsColorPicker, MenuOptionsEditPumpSettings})
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
        MenuOptionsAudioAlerts.Size = New Size(184, 22)
        MenuOptionsAudioAlerts.Text = "Audio Alerts Enabled"
        ' 
        ' MenuOptionsSpeechRecognitionEnabled
        ' 
        MenuOptionsSpeechRecognitionEnabled.DropDownItems.AddRange(New ToolStripItem() {MenuOptionsSpeechRecognitionDisabled, MenuOptionsSpeechRecognitionConfidence, MenuOptionsSpeechRecognition95, MenuOptionsSpeechRecognition90, MenuOptionsSpeechRecognition85, MenuOptionsSpeechRecognition80})
        MenuOptionsSpeechRecognitionEnabled.Name = "MenuOptionsSpeechRecognitionEnabled"
        MenuOptionsSpeechRecognitionEnabled.Size = New Size(184, 22)
        MenuOptionsSpeechRecognitionEnabled.Text = "Speech Recognition"
        MenuOptionsSpeechRecognitionEnabled.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' MenuOptionsSpeechRecognitionDisabled
        ' 
        MenuOptionsSpeechRecognitionDisabled.Name = "MenuOptionsSpeechRecognitionDisabled"
        MenuOptionsSpeechRecognitionDisabled.Size = New Size(135, 22)
        MenuOptionsSpeechRecognitionDisabled.Text = "Disabled"
        ' 
        ' MenuOptionsSpeechRecognitionConfidence
        ' 
        MenuOptionsSpeechRecognitionConfidence.Enabled = False
        MenuOptionsSpeechRecognitionConfidence.Name = "MenuOptionsSpeechRecognitionConfidence"
        MenuOptionsSpeechRecognitionConfidence.Size = New Size(135, 22)
        MenuOptionsSpeechRecognitionConfidence.Text = "Confidence"
        ' 
        ' MenuOptionsSpeechRecognition95
        ' 
        MenuOptionsSpeechRecognition95.CheckOnClick = True
        MenuOptionsSpeechRecognition95.Name = "MenuOptionsSpeechRecognition95"
        MenuOptionsSpeechRecognition95.Size = New Size(135, 22)
        MenuOptionsSpeechRecognition95.Text = "95%"
        MenuOptionsSpeechRecognition95.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' MenuOptionsSpeechRecognition90
        ' 
        MenuOptionsSpeechRecognition90.CheckOnClick = True
        MenuOptionsSpeechRecognition90.Name = "MenuOptionsSpeechRecognition90"
        MenuOptionsSpeechRecognition90.Size = New Size(135, 22)
        MenuOptionsSpeechRecognition90.Text = "90%"
        MenuOptionsSpeechRecognition90.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' MenuOptionsSpeechRecognition85
        ' 
        MenuOptionsSpeechRecognition85.CheckOnClick = True
        MenuOptionsSpeechRecognition85.Name = "MenuOptionsSpeechRecognition85"
        MenuOptionsSpeechRecognition85.Size = New Size(135, 22)
        MenuOptionsSpeechRecognition85.Text = "85%"
        MenuOptionsSpeechRecognition85.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' MenuOptionsSpeechRecognition80
        ' 
        MenuOptionsSpeechRecognition80.Checked = True
        MenuOptionsSpeechRecognition80.CheckOnClick = True
        MenuOptionsSpeechRecognition80.CheckState = CheckState.Checked
        MenuOptionsSpeechRecognition80.Name = "MenuOptionsSpeechRecognition80"
        MenuOptionsSpeechRecognition80.Size = New Size(135, 22)
        MenuOptionsSpeechRecognition80.Text = "80%"
        ' 
        ' MenuOptionsShowChartLegends
        ' 
        MenuOptionsShowChartLegends.Checked = True
        MenuOptionsShowChartLegends.CheckOnClick = True
        MenuOptionsShowChartLegends.CheckState = CheckState.Checked
        MenuOptionsShowChartLegends.Name = "MenuOptionsShowChartLegends"
        MenuOptionsShowChartLegends.Size = New Size(184, 22)
        MenuOptionsShowChartLegends.Text = "Show Chart Legends"
        ' 
        ' MenuOptionsSpeechHelpShown
        ' 
        MenuOptionsSpeechHelpShown.Checked = True
        MenuOptionsSpeechHelpShown.CheckOnClick = True
        MenuOptionsSpeechHelpShown.CheckState = CheckState.Checked
        MenuOptionsSpeechHelpShown.Name = "MenuOptionsSpeechHelpShown"
        MenuOptionsSpeechHelpShown.Size = New Size(184, 22)
        MenuOptionsSpeechHelpShown.Text = "Disable Speech Help"
        '
        ' TableLayoutPanelInsulinTop
        '
        TableLayoutPanelInsulinTop.AutoSize = True
        TableLayoutPanelInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelInsulinTop.ColumnCount = 2
        TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelInsulinTop.Dock = DockStyle.Fill
        TableLayoutPanelInsulinTop.LabelText = "Insulin"
        TableLayoutPanelInsulinTop.Location = New Point(6, 6)
        TableLayoutPanelInsulinTop.Name = "TableLayoutPanelInsulinTop"
        TableLayoutPanelInsulinTop.RowCount = 1
        TableLayoutPanelInsulinTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelInsulinTop.Size = New Size(1358, 37)
        TableLayoutPanelInsulinTop.TabIndex = 1
        TableLayoutPanelInsulin.Controls.Add(TableLayoutPanelInsulinTop, 0, 0)
        '
        ' TableLayoutPanelMealTop
        '
        TableLayoutPanelMealTop.AutoSize = True
        TableLayoutPanelMealTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelMealTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelMealTop.ColumnCount = 2
        TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelMealTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelMealTop.Dock = DockStyle.Fill
        TableLayoutPanelMealTop.LabelText = "Meal"
        TableLayoutPanelMealTop.Location = New Point(6, 6)
        TableLayoutPanelMealTop.Name = "TableLayoutPanelMealTop"
        TableLayoutPanelMealTop.RowCount = 1
        TableLayoutPanelMealTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelMealTop.Size = New Size(1358, 37)
        TableLayoutPanelMealTop.TabIndex = 1
        TableLayoutPanelMeal.Controls.Add(TableLayoutPanelMealTop, 0, 0)
        '
        ' TableLayoutPanelActiveInsulinTop
        '
        TableLayoutPanelActiveInsulinTop.AutoSize = True
        TableLayoutPanelActiveInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelActiveInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelActiveInsulinTop.ColumnCount = 2
        TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelActiveInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelActiveInsulinTop.Dock = DockStyle.Fill
        TableLayoutPanelActiveInsulinTop.LabelText = "Active Insulin"
        TableLayoutPanelActiveInsulinTop.Location = New Point(3, 3)
        TableLayoutPanelActiveInsulinTop.Name = "TableLayoutPanelActiveInsulinTop"
        TableLayoutPanelActiveInsulinTop.RowCount = 1
        TableLayoutPanelActiveInsulinTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelActiveInsulinTop.Size = New Size(1364, 37)
        TableLayoutPanelActiveInsulinTop.TabIndex = 1
        TableLayoutPanelActiveInsulin.Controls.Add(TableLayoutPanelActiveInsulinTop, 0, 0)
        '
        ' TableLayoutPanelSgsTop
        '
        TableLayoutPanelSgsTop.AutoSize = True
        TableLayoutPanelSgsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelSgsTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelSgsTop.ColumnCount = 2
        TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelSgsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelSgsTop.Dock = DockStyle.Fill
        TableLayoutPanelSgsTop.LabelText = "SGs"
        TableLayoutPanelSgsTop.Location = New Point(3, 3)
        TableLayoutPanelSgsTop.Name = "TableLayoutPanelSgsTop"
        TableLayoutPanelSgsTop.RowCount = 1
        TableLayoutPanelSgsTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelSgsTop.Size = New Size(1364, 37)
        TableLayoutPanelSgsTop.TabIndex = 1
        TableLayoutPanelSgs.Controls.Add(TableLayoutPanelSgsTop, 0, 0)
        TableLayoutPanelSgs.Controls.Add(DgvSGs, 0, 1)
        '
        ' TableLayoutPanelLimitsTop
        '
        TableLayoutPanelLimitsTop.AutoSize = True
        TableLayoutPanelLimitsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLimitsTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelLimitsTop.ColumnCount = 2
        TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelLimitsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLimitsTop.Dock = DockStyle.Fill
        TableLayoutPanelLimitsTop.LabelText = "Limits"
        TableLayoutPanelLimitsTop.Location = New Point(3, 3)
        TableLayoutPanelLimitsTop.Name = "TableLayoutPanelLimitsTop"
        TableLayoutPanelLimitsTop.RowCount = 1
        TableLayoutPanelLimitsTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelLimitsTop.Size = New Size(1364, 37)
        TableLayoutPanelLimitsTop.TabIndex = 1
        TableLayoutPanelLimits.Controls.Add(TableLayoutPanelLimitsTop, 0, 0)
        '
        ' TableLayoutPanelNotificationHistoryTop
        '
        TableLayoutPanelNotificationHistoryTop.AutoSize = True
        TableLayoutPanelNotificationHistoryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelNotificationHistoryTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelNotificationHistoryTop.ColumnCount = 2
        TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelNotificationHistoryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelNotificationHistoryTop.Dock = DockStyle.Fill
        TableLayoutPanelNotificationHistoryTop.LabelText = "Notification History"
        TableLayoutPanelNotificationHistoryTop.Location = New Point(3, 3)
        TableLayoutPanelNotificationHistoryTop.Name = "TableLayoutPanelNotificationHistoryTop"
        TableLayoutPanelNotificationHistoryTop.RowCount = 1
        TableLayoutPanelNotificationHistoryTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelNotificationHistoryTop.Size = New Size(1364, 37)
        TableLayoutPanelNotificationHistoryTop.TabIndex = 1
        TableLayoutPanelNotificationHistory.Controls.Add(TableLayoutPanelNotificationHistoryTop, 1, 0)
        '
        ' TableLayoutPanelTherapyAlgorithmTop
        '
        TableLayoutPanelTherapyAlgorithmTop.AutoSize = True
        TableLayoutPanelTherapyAlgorithmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelTherapyAlgorithmTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelTherapyAlgorithmTop.ColumnCount = 2
        TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelTherapyAlgorithmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelTherapyAlgorithmTop.Dock = DockStyle.Fill
        TableLayoutPanelTherapyAlgorithmTop.LabelText = "Therapy Algorithm"
        TableLayoutPanelTherapyAlgorithmTop.Location = New Point(3, 3)
        TableLayoutPanelTherapyAlgorithmTop.Name = "TableLayoutPanelTherapyAlgorithmTop"
        TableLayoutPanelTherapyAlgorithmTop.RowCount = 1
        TableLayoutPanelTherapyAlgorithmTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelTherapyAlgorithmTop.Size = New Size(1364, 37)
        TableLayoutPanelTherapyAlgorithmTop.TabIndex = 1
        TableLayoutPanelTherapyAlgorithm.Controls.Add(TableLayoutPanelTherapyAlgorithmTop, 0, 0)
        '
        ' TableLayoutPanelBannerStateTop
        '
        TableLayoutPanelBannerStateTop.AutoSize = True
        TableLayoutPanelBannerStateTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelBannerStateTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelBannerStateTop.ColumnCount = 2
        TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelBannerStateTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBannerStateTop.Dock = DockStyle.Fill
        TableLayoutPanelBannerStateTop.LabelText = "Banner State"
        TableLayoutPanelBannerStateTop.Location = New Point(3, 3)
        TableLayoutPanelBannerStateTop.Name = "TableLayoutPanelBannerStateTop"
        TableLayoutPanelBannerStateTop.RowCount = 1
        TableLayoutPanelBannerStateTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelBannerStateTop.Size = New Size(1364, 37)
        TableLayoutPanelBannerStateTop.TabIndex = 1
        TableLayoutPanelBannerState.Controls.Add(TableLayoutPanelBannerStateTop, 0, 0)
        '
        ' TableLayoutPanelBasalTop
        '
        TableLayoutPanelBasalTop.AutoSize = True
        TableLayoutPanelBasalTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelBasalTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelBasalTop.ColumnCount = 2
        TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelBasalTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBasalTop.Dock = DockStyle.Fill
        TableLayoutPanelBasalTop.LabelText = "Basal"
        TableLayoutPanelBasalTop.Location = New Point(3, 3)
        TableLayoutPanelBasalTop.Name = "TableLayoutPanelBasalTop"
        TableLayoutPanelBasalTop.RowCount = 1
        TableLayoutPanelBasalTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelBasalTop.Size = New Size(1364, 37)
        TableLayoutPanelBasalTop.TabIndex = 1
        TableLayoutPanelBasal.Controls.Add(TableLayoutPanelBasalTop, 0, 0)
        '
        ' TableLayoutPanelLastSgTop
        '
        TableLayoutPanelLastSgTop.AutoSize = True
        TableLayoutPanelLastSgTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLastSgTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelLastSgTop.ColumnCount = 2
        TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelLastSgTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLastSgTop.Dock = DockStyle.Fill
        TableLayoutPanelLastSgTop.LabelText = "last SG"
        TableLayoutPanelLastSgTop.Location = New Point(3, 3)
        TableLayoutPanelLastSgTop.Name = "TableLayoutPanelLastSgTop"
        TableLayoutPanelLastSgTop.RowCount = 1
        TableLayoutPanelLastSgTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelLastSgTop.Size = New Size(1364, 37)
        TableLayoutPanelLastSgTop.TabIndex = 1
        TableLayoutPanelLastSG.Controls.Add(TableLayoutPanelLastSgTop, 0, 0)
        '
        ' TableLayoutPanelLastAlarmTop
        '
        TableLayoutPanelLastAlarmTop.AutoSize = True
        TableLayoutPanelLastAlarmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLastAlarmTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelLastAlarmTop.ColumnCount = 2
        TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelLastAlarmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLastAlarmTop.Dock = DockStyle.Fill
        TableLayoutPanelLastAlarmTop.LabelText = "Last Alarm"
        TableLayoutPanelLastAlarmTop.Location = New Point(3, 3)
        TableLayoutPanelLastAlarmTop.Name = "TableLayoutPanelLastAlarmTop"
        TableLayoutPanelLastAlarmTop.RowCount = 1
        TableLayoutPanelLastAlarmTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelLastAlarmTop.Size = New Size(1364, 37)
        TableLayoutPanelLastAlarmTop.TabIndex = 1
        TableLayoutPanelLastAlarm.Controls.Add(TableLayoutPanelLastAlarmTop, 0, 0)
        '
        ' TableLayoutPanelAutoBasalDeliveryTop
        '
        TableLayoutPanelAutoBasalDeliveryTop.AutoSize = True
        TableLayoutPanelAutoBasalDeliveryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelAutoBasalDeliveryTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelAutoBasalDeliveryTop.ColumnCount = 2
        TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelAutoBasalDeliveryTop.Dock = DockStyle.Fill
        TableLayoutPanelAutoBasalDeliveryTop.LabelText = "Basal"
        TableLayoutPanelAutoBasalDeliveryTop.Location = New Point(6, 6)
        TableLayoutPanelAutoBasalDeliveryTop.Name = "TableLayoutPanelAutoBasalDeliveryTop"
        TableLayoutPanelAutoBasalDeliveryTop.RowCount = 1
        TableLayoutPanelAutoBasalDeliveryTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelAutoBasalDeliveryTop.Size = New Size(1358, 37)
        TableLayoutPanelAutoBasalDeliveryTop.TabIndex = 1
        TableLayoutPanelAutoBasalDelivery.Controls.Add(TableLayoutPanelAutoBasalDeliveryTop, 0, 0)
        TableLayoutPanelAutoBasalDelivery.Controls.Add(DgvAutoBasalDelivery, 0, 1)
        '
        ' TableLayoutPanelAutoModeStatusTop
        '
        TableLayoutPanelAutoModeStatusTop.AutoSize = True
        TableLayoutPanelAutoModeStatusTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelAutoModeStatusTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelAutoModeStatusTop.ColumnCount = 2
        TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelAutoModeStatusTop.Dock = DockStyle.Fill
        TableLayoutPanelAutoModeStatusTop.LabelText = "Auto Mode Status"
        TableLayoutPanelAutoModeStatusTop.Location = New Point(6, 6)
        TableLayoutPanelAutoModeStatusTop.Name = "TableLayoutPanelAutoModeStatusTop"
        TableLayoutPanelAutoModeStatusTop.RowCount = 1
        TableLayoutPanelAutoModeStatusTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelAutoModeStatusTop.Size = New Size(1358, 37)
        TableLayoutPanelAutoModeStatusTop.TabIndex = 1
        TableLayoutPanelAutoModeStatus.Controls.Add(TableLayoutPanelAutoModeStatusTop, 0, 0)
        '
        ' TableLayoutPanelBgReadingsTop
        '
        TableLayoutPanelBgReadingsTop.AutoSize = True
        TableLayoutPanelBgReadingsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelBgReadingsTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelBgReadingsTop.ColumnCount = 2
        TableLayoutPanelBgReadingsTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelBgReadingsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBgReadingsTop.Dock = DockStyle.Fill
        TableLayoutPanelBgReadingsTop.LabelText = "Sg Readings"
        TableLayoutPanelBgReadingsTop.Location = New Point(6, 6)
        TableLayoutPanelBgReadingsTop.Name = "TableLayoutPanelBgReadingsTop"
        TableLayoutPanelBgReadingsTop.RowCount = 1
        TableLayoutPanelBgReadingsTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelBgReadingsTop.Size = New Size(1358, 37)
        TableLayoutPanelBgReadingsTop.TabIndex = 1
        TableLayoutPanelSgReadings.Controls.Add(TableLayoutPanelBgReadingsTop, 0, 0)
        '
        ' TableLayoutPanelCalibrationTop
        '
        TableLayoutPanelCalibrationTop.AutoSize = True
        TableLayoutPanelCalibrationTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelCalibrationTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelCalibrationTop.ColumnCount = 2
        TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelCalibrationTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelCalibrationTop.Dock = DockStyle.Fill
        TableLayoutPanelCalibrationTop.LabelText = "Calibration"
        TableLayoutPanelCalibrationTop.Location = New Point(6, 6)
        TableLayoutPanelCalibrationTop.Name = "TableLayoutPanelCalibrationTop"
        TableLayoutPanelCalibrationTop.RowCount = 1
        TableLayoutPanelCalibrationTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelCalibrationTop.Size = New Size(1358, 37)
        TableLayoutPanelCalibrationTop.TabIndex = 1
        TableLayoutPanelCalibration.Controls.Add(TableLayoutPanelCalibrationTop, 0, 0)
        '
        ' TableLayoutPanelLowGlucoseSuspendedTop
        '
        TableLayoutPanelLowGlucoseSuspendedTop.AutoSize = True
        TableLayoutPanelLowGlucoseSuspendedTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelLowGlucoseSuspendedTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelLowGlucoseSuspendedTop.ColumnCount = 2
        TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLowGlucoseSuspendedTop.Dock = DockStyle.Fill
        TableLayoutPanelLowGlucoseSuspendedTop.LabelText = "Low Glucose Suspended"
        TableLayoutPanelLowGlucoseSuspendedTop.Location = New Point(6, 6)
        TableLayoutPanelLowGlucoseSuspendedTop.Name = "TableLayoutPanelLowGlucoseSuspendedTop"
        TableLayoutPanelLowGlucoseSuspendedTop.RowCount = 1
        TableLayoutPanelLowGlucoseSuspendedTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelLowGlucoseSuspendedTop.Size = New Size(1358, 37)
        TableLayoutPanelLowGlucoseSuspendedTop.TabIndex = 1
        TableLayoutPanelLowGlucoseSuspended.Controls.Add(TableLayoutPanelLowGlucoseSuspendedTop, 0, 0)
        '
        ' TableLayoutPanelTimeChangeTop
        '
        TableLayoutPanelTimeChangeTop.AutoSize = True
        TableLayoutPanelTimeChangeTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelTimeChangeTop.ButtonText = "Return To 'Summary Data' Tab"
        TableLayoutPanelTimeChangeTop.ColumnCount = 2
        TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanelTimeChangeTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelTimeChangeTop.Dock = DockStyle.Fill
        TableLayoutPanelTimeChangeTop.LabelText = "Time Change"
        TableLayoutPanelTimeChangeTop.Location = New Point(6, 6)
        TableLayoutPanelTimeChangeTop.Name = "TableLayoutPanelTimeChangeTop"
        TableLayoutPanelTimeChangeTop.RowCount = 1
        TableLayoutPanelTimeChangeTop.RowStyles.Add(New RowStyle())
        TableLayoutPanelTimeChangeTop.Size = New Size(1358, 37)
        TableLayoutPanelTimeChangeTop.TabIndex = 1
        TableLayoutPanelTimeChange.Controls.Add(TableLayoutPanelTimeChangeTop, 0, 0)
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(181, 6)
        ' 
        ' MenuOptionsAutoLogin
        ' 
        MenuOptionsAutoLogin.CheckOnClick = True
        MenuOptionsAutoLogin.Name = "MenuOptionsAutoLogin"
        MenuOptionsAutoLogin.Size = New Size(184, 22)
        MenuOptionsAutoLogin.Text = "Auto Login"
        ' 
        ' ToolStripSeparator6
        ' 
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New Size(181, 6)
        ' 
        ' MenuOptionsAdvancedOptions
        ' 
        MenuOptionsAdvancedOptions.Enabled = False
        MenuOptionsAdvancedOptions.Name = "MenuOptionsAdvancedOptions"
        MenuOptionsAdvancedOptions.Size = New Size(184, 22)
        MenuOptionsAdvancedOptions.Text = "Advanced Options"
        ' 
        ' MenuOptionsFilterRawJSONData
        ' 
        MenuOptionsFilterRawJSONData.Checked = True
        MenuOptionsFilterRawJSONData.CheckOnClick = True
        MenuOptionsFilterRawJSONData.CheckState = CheckState.Checked
        MenuOptionsFilterRawJSONData.Name = "MenuOptionsFilterRawJSONData"
        MenuOptionsFilterRawJSONData.Size = New Size(184, 22)
        MenuOptionsFilterRawJSONData.Text = "Filter Raw JSON Data"
        ' 
        ' MenuOptionsUseLocalTimeZone
        ' 
        MenuOptionsUseLocalTimeZone.Checked = True
        MenuOptionsUseLocalTimeZone.CheckOnClick = True
        MenuOptionsUseLocalTimeZone.CheckState = CheckState.Indeterminate
        MenuOptionsUseLocalTimeZone.Name = "MenuOptionsUseLocalTimeZone"
        MenuOptionsUseLocalTimeZone.Size = New Size(184, 22)
        MenuOptionsUseLocalTimeZone.Text = "Use Local TImeZone"
        ' 
        ' ToolStripSeparator7
        ' 
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New Size(181, 6)
        ' 
        ' MenuOptionsColorPicker
        ' 
        MenuOptionsColorPicker.Image = CType(resources.GetObject("MenuOptionsColorPicker.Image"), Image)
        MenuOptionsColorPicker.Name = "MenuOptionsColorPicker"
        MenuOptionsColorPicker.Size = New Size(184, 22)
        MenuOptionsColorPicker.Text = "Color Picker..."
        ' 
        ' MenuOptionsEditPumpSettings
        ' 
        MenuOptionsEditPumpSettings.Name = "MenuOptionsEditPumpSettings"
        MenuOptionsEditPumpSettings.Size = New Size(184, 22)
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
        AboveHighLimitMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        AboveHighLimitValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        ActiveInsulinValue.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        FullNameLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        AverageSGMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        AverageSGValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        BelowLowLimitMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        BelowLowLimitValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        CalibrationShieldPanel.Controls.Add(LastSgOrExitTimeLabel)
        CalibrationShieldPanel.Controls.Add(PumpBannerStateLabel)
        CalibrationShieldPanel.Controls.Add(ShieldUnitsLabel)
        CalibrationShieldPanel.Controls.Add(CurrentSgLabel)
        CalibrationShieldPanel.Controls.Add(SensorMessageLabel)
        CalibrationShieldPanel.Controls.Add(SmartGuardShieldPictureBox)
        CalibrationShieldPanel.Dock = DockStyle.Left
        CalibrationShieldPanel.Location = New Point(0, 0)
        CalibrationShieldPanel.Margin = New Padding(0)
        CalibrationShieldPanel.Name = "CalibrationShieldPanel"
        CalibrationShieldPanel.Size = New Size(116, 134)
        CalibrationShieldPanel.TabIndex = 64
        ' 
        ' LastSgOrExitTimeLabel
        ' 
        LastSgOrExitTimeLabel.BackColor = Color.Transparent
        LastSgOrExitTimeLabel.Dock = DockStyle.Bottom
        LastSgOrExitTimeLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        LastSgOrExitTimeLabel.ForeColor = Color.White
        LastSgOrExitTimeLabel.Location = New Point(0, 113)
        LastSgOrExitTimeLabel.Name = "LastSgOrExitTimeLabel"
        LastSgOrExitTimeLabel.Size = New Size(116, 21)
        LastSgOrExitTimeLabel.TabIndex = 55
        LastSgOrExitTimeLabel.Text = "Exit in 4:27"
        LastSgOrExitTimeLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' PumpBannerStateLabel
        ' 
        PumpBannerStateLabel.BackColor = Color.Lime
        PumpBannerStateLabel.Dock = DockStyle.Top
        PumpBannerStateLabel.Font = New Font("Segoe UI", 8.0F)
        PumpBannerStateLabel.ForeColor = Color.Black
        PumpBannerStateLabel.Location = New Point(0, 0)
        PumpBannerStateLabel.Name = "PumpBannerStateLabel"
        PumpBannerStateLabel.Size = New Size(116, 13)
        PumpBannerStateLabel.TabIndex = 56
        PumpBannerStateLabel.Text = "Target 150 2:00 Hr"
        PumpBannerStateLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' ShieldUnitsLabel
        ' 
        ShieldUnitsLabel.AutoSize = True
        ShieldUnitsLabel.BackColor = Color.Transparent
        ShieldUnitsLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold)
        ShieldUnitsLabel.ForeColor = Color.White
        ShieldUnitsLabel.Location = New Point(38, 76)
        ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        ShieldUnitsLabel.Size = New Size(40, 13)
        ShieldUnitsLabel.TabIndex = 8
        ShieldUnitsLabel.Text = "XX/XX"
        ' 
        ' CurrentSgLabel
        ' 
        CurrentSgLabel.BackColor = Color.Transparent
        CurrentSgLabel.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)
        CurrentSgLabel.ForeColor = Color.White
        CurrentSgLabel.Location = New Point(22, 35)
        CurrentSgLabel.Name = "CurrentSgLabel"
        CurrentSgLabel.Size = New Size(72, 32)
        CurrentSgLabel.TabIndex = 9
        CurrentSgLabel.Text = "---"
        CurrentSgLabel.TextAlign = ContentAlignment.MiddleCenter
        CurrentSgLabel.Visible = False
        ' 
        ' SensorMessageLabel
        ' 
        SensorMessageLabel.BackColor = Color.Transparent
        SensorMessageLabel.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        SensorMessageLabel.ForeColor = Color.White
        SensorMessageLabel.Location = New Point(0, 13)
        SensorMessageLabel.Name = "SensorMessageLabel"
        SensorMessageLabel.Size = New Size(116, 57)
        SensorMessageLabel.TabIndex = 1
        SensorMessageLabel.Text = "Calibration Required"
        SensorMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SmartGuardShieldPictureBox
        ' 
        SmartGuardShieldPictureBox.Image = My.Resources.Resources.Shield
        SmartGuardShieldPictureBox.Location = New Point(0, 0)
        SmartGuardShieldPictureBox.Margin = New Padding(5)
        SmartGuardShieldPictureBox.Name = "SmartGuardShieldPictureBox"
        SmartGuardShieldPictureBox.Size = New Size(119, 116)
        SmartGuardShieldPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        SmartGuardShieldPictureBox.TabIndex = 5
        SmartGuardShieldPictureBox.TabStop = False
        ' 
        ' CareLinkUserDataRecordBindingSource
        ' 
        CareLinkUserDataRecordBindingSource.DataSource = GetType(CareLinkUserDataRecord)
        ' 
        ' CursorMessage1Label
        ' 
        CursorMessage1Label.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        CursorMessage1Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        CursorMessage2Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        CursorMessage3Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        CursorMessage4Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        DgvAutoBasalDelivery.Location = New Point(6, 9)
        DgvAutoBasalDelivery.Name = "DgvAutoBasalDelivery"
        DgvAutoBasalDelivery.ReadOnly = True
        DgvAutoBasalDelivery.Size = New Size(1358, 640)
        DgvAutoBasalDelivery.TabIndex = 0
        ' 
        ' DgvCareLinkUsers
        ' 
        DgvCareLinkUsers.AllowUserToAddRows = False
        DgvCareLinkUsers.AllowUserToResizeColumns = False
        DgvCareLinkUsers.AllowUserToResizeRows = False
        DgvCareLinkUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DgvCareLinkUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
        DgvCareLinkUsers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        DgvCareLinkUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle4.WrapMode = DataGridViewTriState.False
        DgvCareLinkUsers.DefaultCellStyle = DataGridViewCellStyle4
        DgvCareLinkUsers.Dock = DockStyle.Fill
        DgvCareLinkUsers.EditMode = DataGridViewEditMode.EditOnEnter
        DgvCareLinkUsers.Location = New Point(3, 3)
        DgvCareLinkUsers.Name = "DgvCareLinkUsers"
        DgvCareLinkUsers.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvCareLinkUsers.Size = New Size(1370, 655)
        DgvCareLinkUsers.TabIndex = 0
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
        DgvCurrentUser.Size = New Size(1376, 661)
        DgvCurrentUser.TabIndex = 0
        ' 
        ' DgvInsulin
        ' 
        DgvInsulin.Dock = DockStyle.Fill
        DgvInsulin.Location = New Point(6, 52)
        DgvInsulin.Name = "DgvInsulin"
        DgvInsulin.ReadOnly = True
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
        DgvSGs.Location = New Point(3, 3)
        DgvSGs.Name = "DgvSGs"
        DgvSGs.Size = New Size(1364, 649)
        DgvSGs.TabIndex = 1
        ' 
        ' DgvSummary
        ' 
        DgvSummary.Dock = DockStyle.Fill
        DgvSummary.Location = New Point(3, 3)
        DgvSummary.Name = "DgvSummary"
        DgvSummary.ReadOnly = True
        DataGridViewCellStyle7.WrapMode = DataGridViewTriState.True
        DgvSummary.RowsDefaultCellStyle = DataGridViewCellStyle7
        DgvSummary.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvSummary.Size = New Size(1370, 655)
        DgvSummary.TabIndex = 0
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
        InRangeMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        InRangeMessageLabel.ForeColor = Color.Lime
        InRangeMessageLabel.Location = New Point(81, 269)
        InRangeMessageLabel.Name = "InRangeMessageLabel"
        InRangeMessageLabel.Size = New Size(73, 21)
        InRangeMessageLabel.TabIndex = 30
        InRangeMessageLabel.Text = "In range"
        InRangeMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Last24TotalInsulinUnitsLabel
        ' 
        Last24TotalInsulinUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24TotalInsulinUnitsLabel.ForeColor = Color.White
        Last24TotalInsulinUnitsLabel.Location = New Point(131, 103)
        Last24TotalInsulinUnitsLabel.Name = "Last24TotalInsulinUnitsLabel"
        Last24TotalInsulinUnitsLabel.Size = New Size(70, 21)
        Last24TotalInsulinUnitsLabel.TabIndex = 67
        Last24TotalInsulinUnitsLabel.Text = " 100.0U"
        Last24TotalInsulinUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' InsulinLevelPictureBox
        ' 
        InsulinLevelPictureBox.Image = CType(resources.GetObject("InsulinLevelPictureBox.Image"), Image)
        InsulinLevelPictureBox.InitialImage = Nothing
        InsulinLevelPictureBox.Location = New Point(224, 0)
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
        LabelSgTrend.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        LabelTrendArrows.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
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
        LabelTrendValue.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        Last24AutoCorrectionLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24AutoCorrectionLabel.ForeColor = Color.White
        Last24AutoCorrectionLabel.Location = New Point(0, 86)
        Last24AutoCorrectionLabel.Name = "Last24AutoCorrectionLabel"
        Last24AutoCorrectionLabel.Size = New Size(131, 21)
        Last24AutoCorrectionLabel.TabIndex = 64
        Last24AutoCorrectionLabel.Text = "Auto Correction"
        Last24AutoCorrectionLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24AutoCorrectionPercentLabel
        ' 
        Last24AutoCorrectionPercentLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24AutoCorrectionPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24AutoCorrectionPercentLabel.ForeColor = Color.White
        Last24AutoCorrectionPercentLabel.Location = New Point(200, 86)
        Last24AutoCorrectionPercentLabel.Name = "Last24AutoCorrectionPercentLabel"
        Last24AutoCorrectionPercentLabel.Size = New Size(46, 21)
        Last24AutoCorrectionPercentLabel.TabIndex = 73
        Last24AutoCorrectionPercentLabel.Text = "20%"
        Last24AutoCorrectionPercentLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24AutoCorrectionUnitsLabel
        ' 
        Last24AutoCorrectionUnitsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24AutoCorrectionUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24AutoCorrectionUnitsLabel.ForeColor = Color.White
        Last24AutoCorrectionUnitsLabel.Location = New Point(131, 86)
        Last24AutoCorrectionUnitsLabel.Name = "Last24AutoCorrectionUnitsLabel"
        Last24AutoCorrectionUnitsLabel.Size = New Size(70, 21)
        Last24AutoCorrectionUnitsLabel.TabIndex = 72
        Last24AutoCorrectionUnitsLabel.Text = "20.0U"
        Last24AutoCorrectionUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24BasalLabel
        ' 
        Last24BasalLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24BasalLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24BasalLabel.ForeColor = Color.White
        Last24BasalLabel.Location = New Point(0, 45)
        Last24BasalLabel.Name = "Last24BasalLabel"
        Last24BasalLabel.Size = New Size(131, 21)
        Last24BasalLabel.TabIndex = 62
        Last24BasalLabel.Text = "Basal"
        Last24BasalLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24BasalPercentLabel
        ' 
        Last24BasalPercentLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24BasalPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24BasalPercentLabel.ForeColor = Color.White
        Last24BasalPercentLabel.Location = New Point(200, 45)
        Last24BasalPercentLabel.Name = "Last24BasalPercentLabel"
        Last24BasalPercentLabel.Size = New Size(46, 21)
        Last24BasalPercentLabel.TabIndex = 69
        Last24BasalPercentLabel.Text = "50%"
        Last24BasalPercentLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24BasalUnitsLabel
        ' 
        Last24BasalUnitsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24BasalUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24BasalUnitsLabel.ForeColor = Color.White
        Last24BasalUnitsLabel.Location = New Point(131, 45)
        Last24BasalUnitsLabel.Name = "Last24BasalUnitsLabel"
        Last24BasalUnitsLabel.Size = New Size(70, 21)
        Last24BasalUnitsLabel.TabIndex = 68
        Last24BasalUnitsLabel.Text = "50.0U"
        Last24BasalUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24CarbsValueLabel
        ' 
        Last24CarbsValueLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24CarbsValueLabel.ForeColor = Color.White
        Last24CarbsValueLabel.Location = New Point(0, 21)
        Last24CarbsValueLabel.Name = "Last24CarbsValueLabel"
        Last24CarbsValueLabel.Size = New Size(249, 21)
        Last24CarbsValueLabel.TabIndex = 74
        Last24CarbsValueLabel.Text = " 100 Grams"
        Last24CarbsValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Last24CarbsLabel
        ' 
        Last24CarbsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24CarbsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24CarbsLabel.ForeColor = Color.White
        Last24CarbsLabel.Location = New Point(0, 21)
        Last24CarbsLabel.Name = "Last24CarbsLabel"
        Last24CarbsLabel.Size = New Size(70, 21)
        Last24CarbsLabel.TabIndex = 66
        Last24CarbsLabel.Text = "Carbs"
        Last24CarbsLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HoursGraphLabel
        ' 
        Last24HoursGraphLabel.Anchor = AnchorStyles.Top
        Last24HoursGraphLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        Last24HTotalsPanel.Controls.Add(Last24TotalInsulinUnitsLabel)
        Last24HTotalsPanel.Controls.Add(Last24AutoCorrectionUnitsLabel)
        Last24HTotalsPanel.Controls.Add(Last24ManualBolusUnitsLabel)
        Last24HTotalsPanel.Controls.Add(Last24BasalUnitsLabel)
        Last24HTotalsPanel.Controls.Add(Last24TotalInsulinLabel)
        Last24HTotalsPanel.Controls.Add(Last24CarbsLabel)
        Last24HTotalsPanel.Controls.Add(Last24CarbsValueLabel)
        Last24HTotalsPanel.Controls.Add(Last24AutoCorrectionPercentLabel)
        Last24HTotalsPanel.Controls.Add(Last24AutoCorrectionLabel)
        Last24HTotalsPanel.Controls.Add(Last24ManualBolusPercentLabel)
        Last24HTotalsPanel.Controls.Add(Last24MealBolusLabel)
        Last24HTotalsPanel.Controls.Add(Last24BasalPercentLabel)
        Last24HTotalsPanel.Controls.Add(Last24BasalLabel)
        Last24HTotalsPanel.Controls.Add(Last24TotalsLabel)
        Last24HTotalsPanel.Location = New Point(724, 0)
        Last24HTotalsPanel.Name = "Last24HTotalsPanel"
        Last24HTotalsPanel.Size = New Size(253, 129)
        Last24HTotalsPanel.TabIndex = 66
        ' 
        ' Last24ManualBolusUnitsLabel
        ' 
        Last24ManualBolusUnitsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24ManualBolusUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24ManualBolusUnitsLabel.ForeColor = Color.White
        Last24ManualBolusUnitsLabel.Location = New Point(131, 66)
        Last24ManualBolusUnitsLabel.Name = "Last24ManualBolusUnitsLabel"
        Last24ManualBolusUnitsLabel.Size = New Size(70, 21)
        Last24ManualBolusUnitsLabel.TabIndex = 70
        Last24ManualBolusUnitsLabel.Text = "30.0U"
        Last24ManualBolusUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24TotalInsulinLabel
        ' 
        Last24TotalInsulinLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24TotalInsulinLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24TotalInsulinLabel.ForeColor = Color.White
        Last24TotalInsulinLabel.Location = New Point(0, 103)
        Last24TotalInsulinLabel.Name = "Last24TotalInsulinLabel"
        Last24TotalInsulinLabel.Size = New Size(131, 21)
        Last24TotalInsulinLabel.TabIndex = 61
        Last24TotalInsulinLabel.Text = "Total Insulin"
        Last24TotalInsulinLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24ManualBolusPercentLabel
        ' 
        Last24ManualBolusPercentLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24ManualBolusPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24ManualBolusPercentLabel.ForeColor = Color.White
        Last24ManualBolusPercentLabel.Location = New Point(200, 66)
        Last24ManualBolusPercentLabel.Name = "Last24ManualBolusPercentLabel"
        Last24ManualBolusPercentLabel.Size = New Size(46, 21)
        Last24ManualBolusPercentLabel.TabIndex = 71
        Last24ManualBolusPercentLabel.Text = "30%"
        Last24ManualBolusPercentLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24MealBolusLabel
        ' 
        Last24MealBolusLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24MealBolusLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24MealBolusLabel.ForeColor = Color.White
        Last24MealBolusLabel.Location = New Point(0, 66)
        Last24MealBolusLabel.Name = "Last24MealBolusLabel"
        Last24MealBolusLabel.Size = New Size(131, 21)
        Last24MealBolusLabel.TabIndex = 63
        Last24MealBolusLabel.Text = "Meal Bolus"
        Last24MealBolusLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24TotalsLabel
        ' 
        Last24TotalsLabel.BackColor = Color.White
        Last24TotalsLabel.Dock = DockStyle.Top
        Last24TotalsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24TotalsLabel.ForeColor = Color.Black
        Last24TotalsLabel.Location = New Point(0, 0)
        Last24TotalsLabel.Name = "Last24TotalsLabel"
        Last24TotalsLabel.Size = New Size(249, 23)
        Last24TotalsLabel.TabIndex = 65
        Last24TotalsLabel.Text = "Last 24 Hr Totals"
        Last24TotalsLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' MaxBasalPerHourLabel
        ' 
        MaxBasalPerHourLabel.AutoSize = True
        MaxBasalPerHourLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        ModelLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        ModelLabel.ForeColor = Color.White
        ModelLabel.Location = New Point(1140, 26)
        ModelLabel.Name = "ModelLabel"
        ModelLabel.Size = New Size(230, 21)
        ModelLabel.TabIndex = 57
        ModelLabel.Text = "Model"
        ' 
        ' PumpNameLabel
        ' 
        PumpNameLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
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
        PumpBatteryPictureBox.Location = New Point(127, 0)
        PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        PumpBatteryPictureBox.Size = New Size(74, 84)
        PumpBatteryPictureBox.TabIndex = 43
        PumpBatteryPictureBox.TabStop = False
        ' 
        ' PumpBatteryRemainingLabel
        ' 
        PumpBatteryRemainingLabel.BackColor = Color.Transparent
        PumpBatteryRemainingLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        PumpBatteryRemainingLabel.ForeColor = Color.White
        PumpBatteryRemainingLabel.Location = New Point(119, 89)
        PumpBatteryRemainingLabel.Name = "PumpBatteryRemainingLabel"
        PumpBatteryRemainingLabel.Size = New Size(87, 21)
        PumpBatteryRemainingLabel.TabIndex = 11
        PumpBatteryRemainingLabel.Text = "Unknown"
        PumpBatteryRemainingLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' InsulinTypeLabel
        ' 
        InsulinTypeLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        ReadingsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        RemainingInsulinUnits.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        RemainingInsulinUnits.ForeColor = Color.White
        RemainingInsulinUnits.Location = New Point(209, 90)
        RemainingInsulinUnits.Name = "RemainingInsulinUnits"
        RemainingInsulinUnits.Size = New Size(80, 21)
        RemainingInsulinUnits.TabIndex = 12
        RemainingInsulinUnits.Text = "000.0U"
        RemainingInsulinUnits.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SensorDaysLeftLabel
        ' 
        SensorDaysLeftLabel.BackColor = Color.Transparent
        SensorDaysLeftLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        SensorTimeLeftLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        SensorTimeLeftPanel.Location = New Point(636, 0)
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
        SerialNumberButton.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        PumpAITLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        PumpBatteryRemaining2Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        PumpBatteryRemaining2Label.ForeColor = Color.White
        PumpBatteryRemaining2Label.Location = New Point(119, 106)
        PumpBatteryRemaining2Label.Name = "PumpBatteryRemaining2Label"
        PumpBatteryRemaining2Label.Size = New Size(87, 21)
        PumpBatteryRemaining2Label.TabIndex = 69
        PumpBatteryRemaining2Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TransmitterBatteryPercentLabel
        ' 
        TransmitterBatteryPercentLabel.BackColor = Color.Transparent
        TransmitterBatteryPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
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
        TimeInRangeLabel.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)
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
        TimeInRangeChartLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        TimeInRangeSummaryPercentCharLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        TimeInRangeValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
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
        TirComplianceLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
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
        HighTirComplianceLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
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
        LowTirComplianceLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
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
        SmartGuardLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
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
        TabControlPage1.Font = New Font("Segoe UI", 9.0F)
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
        TableLayoutPanelInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelInsulin.Dock = DockStyle.Fill
        TableLayoutPanelInsulin.Location = New Point(3, 3)
        TableLayoutPanelInsulin.Name = "TableLayoutPanelInsulin"
        TableLayoutPanelInsulin.RowCount = 2
        TableLayoutPanelInsulin.RowStyles.Add(New RowStyle())
        TableLayoutPanelInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelInsulin.Size = New Size(1370, 655)
        TableLayoutPanelInsulin.TabIndex = 1
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
        TableLayoutPanelMeal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelMeal.Dock = DockStyle.Fill
        TableLayoutPanelMeal.Location = New Point(3, 3)
        TableLayoutPanelMeal.Name = "TableLayoutPanelMeal"
        TableLayoutPanelMeal.RowCount = 2
        TableLayoutPanelMeal.RowStyles.Add(New RowStyle())
        TableLayoutPanelMeal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelMeal.Size = New Size(1370, 655)
        TableLayoutPanelMeal.TabIndex = 1
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
        TableLayoutPanelActiveInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelActiveInsulin.Dock = DockStyle.Fill
        TableLayoutPanelActiveInsulin.Location = New Point(3, 3)
        TableLayoutPanelActiveInsulin.Name = "TableLayoutPanelActiveInsulin"
        TableLayoutPanelActiveInsulin.RowCount = 2
        TableLayoutPanelActiveInsulin.RowStyles.Add(New RowStyle())
        TableLayoutPanelActiveInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelActiveInsulin.Size = New Size(1370, 655)
        TableLayoutPanelActiveInsulin.TabIndex = 0
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
        TableLayoutPanelSgs.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelSgs.ColumnCount = 1
        TableLayoutPanelSgs.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelSgs.Dock = DockStyle.Fill
        TableLayoutPanelSgs.Location = New Point(3, 3)
        TableLayoutPanelSgs.Name = "TableLayoutPanelSgs"
        TableLayoutPanelSgs.RowCount = 2
        TableLayoutPanelSgs.RowStyles.Add(New RowStyle())
        TableLayoutPanelSgs.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelSgs.Size = New Size(1370, 655)
        TableLayoutPanelSgs.TabIndex = 1
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
        TableLayoutPanelLimits.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelLimits.ColumnCount = 1
        TableLayoutPanelLimits.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLimits.Dock = DockStyle.Fill
        TableLayoutPanelLimits.Location = New Point(3, 3)
        TableLayoutPanelLimits.Name = "TableLayoutPanelLimits"
        TableLayoutPanelLimits.RowCount = 2
        TableLayoutPanelLimits.RowStyles.Add(New RowStyle())
        TableLayoutPanelLimits.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLimits.Size = New Size(1370, 655)
        TableLayoutPanelLimits.TabIndex = 0
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
        TableLayoutPanelNotificationHistory.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelNotificationHistory.ColumnCount = 1
        TableLayoutPanelNotificationHistory.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelNotificationHistory.Dock = DockStyle.Fill
        TableLayoutPanelNotificationHistory.Location = New Point(3, 3)
        TableLayoutPanelNotificationHistory.Name = "TableLayoutPanelNotificationHistory"
        TableLayoutPanelNotificationHistory.RowCount = 2
        TableLayoutPanelNotificationHistory.RowStyles.Add(New RowStyle())
        TableLayoutPanelNotificationHistory.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelNotificationHistory.Size = New Size(1370, 655)
        TableLayoutPanelNotificationHistory.TabIndex = 0
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
        TableLayoutPanelTherapyAlgorithm.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelTherapyAlgorithm.ColumnCount = 1
        TableLayoutPanelTherapyAlgorithm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelTherapyAlgorithm.Dock = DockStyle.Fill
        TableLayoutPanelTherapyAlgorithm.Location = New Point(3, 3)
        TableLayoutPanelTherapyAlgorithm.Name = "TableLayoutPanelTherapyAlgorithm"
        TableLayoutPanelTherapyAlgorithm.RowCount = 2
        TableLayoutPanelTherapyAlgorithm.RowStyles.Add(New RowStyle())
        TableLayoutPanelTherapyAlgorithm.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelTherapyAlgorithm.Size = New Size(1370, 655)
        TableLayoutPanelTherapyAlgorithm.TabIndex = 0
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
        TableLayoutPanelBannerState.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelBannerState.ColumnCount = 1
        TableLayoutPanelBannerState.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBannerState.Dock = DockStyle.Fill
        TableLayoutPanelBannerState.Location = New Point(3, 3)
        TableLayoutPanelBannerState.Margin = New Padding(0)
        TableLayoutPanelBannerState.Name = "TableLayoutPanelBannerState"
        TableLayoutPanelBannerState.RowCount = 2
        TableLayoutPanelBannerState.RowStyles.Add(New RowStyle())
        TableLayoutPanelBannerState.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBannerState.Size = New Size(1370, 655)
        TableLayoutPanelBannerState.TabIndex = 0
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
        TableLayoutPanelBasal.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelBasal.ColumnCount = 1
        TableLayoutPanelBasal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBasal.Dock = DockStyle.Fill
        TableLayoutPanelBasal.Location = New Point(3, 3)
        TableLayoutPanelBasal.Name = "TableLayoutPanelBasal"
        TableLayoutPanelBasal.RowCount = 2
        TableLayoutPanelBasal.RowStyles.Add(New RowStyle())
        TableLayoutPanelBasal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelBasal.Size = New Size(1370, 655)
        TableLayoutPanelBasal.TabIndex = 0
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
        TableLayoutPanelLastSG.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelLastSG.ColumnCount = 1
        TableLayoutPanelLastSG.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLastSG.Dock = DockStyle.Fill
        TableLayoutPanelLastSG.Location = New Point(3, 3)
        TableLayoutPanelLastSG.Name = "TableLayoutPanelLastSG"
        TableLayoutPanelLastSG.RowCount = 2
        TableLayoutPanelLastSG.RowStyles.Add(New RowStyle())
        TableLayoutPanelLastSG.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLastSG.Size = New Size(1370, 655)
        TableLayoutPanelLastSG.TabIndex = 1
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
        TableLayoutPanelLastAlarm.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelLastAlarm.ColumnCount = 1
        TableLayoutPanelLastAlarm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLastAlarm.Dock = DockStyle.Fill
        TableLayoutPanelLastAlarm.Location = New Point(3, 3)
        TableLayoutPanelLastAlarm.Margin = New Padding(0)
        TableLayoutPanelLastAlarm.Name = "TableLayoutPanelLastAlarm"
        TableLayoutPanelLastAlarm.RowCount = 2
        TableLayoutPanelLastAlarm.RowStyles.Add(New RowStyle())
        TableLayoutPanelLastAlarm.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLastAlarm.Size = New Size(1370, 655)
        TableLayoutPanelLastAlarm.TabIndex = 0
        ' 
        ' TabControlPage2
        ' 
        TabControlPage2.Appearance = TabAppearance.Buttons
        TabControlPage2.Controls.Add(TabPageAutoBasalDelivery)
        TabControlPage2.Controls.Add(TabPageAutoModeStatus)
        TabControlPage2.Controls.Add(TabPageBgReadings)
        TabControlPage2.Controls.Add(TabPageCalibration)
        TabControlPage2.Controls.Add(TabPageLowGlucoseSuspended)
        TabControlPage2.Controls.Add(TabPageTimeChange)
        TabControlPage2.Controls.Add(TabPageLastSG)
        TabControlPage2.Controls.Add(TabPageLastAlarm)
        TabControlPage2.Controls.Add(TabPageCurrentUser)
        TabControlPage2.Controls.Add(TabPageAllUsers)
        TabControlPage2.Controls.Add(TabPageBackToHomePage)
        TabControlPage2.Dock = DockStyle.Fill
        TabControlPage2.Font = New Font("Segoe UI", 9.0F)
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
        TableLayoutPanelAutoBasalDelivery.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelAutoBasalDelivery.Dock = DockStyle.Fill
        TableLayoutPanelAutoBasalDelivery.Location = New Point(3, 3)
        TableLayoutPanelAutoBasalDelivery.Name = "TableLayoutPanelAutoBasalDelivery"
        TableLayoutPanelAutoBasalDelivery.RowCount = 2
        TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New RowStyle())
        TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelAutoBasalDelivery.Size = New Size(1370, 655)
        TableLayoutPanelAutoBasalDelivery.TabIndex = 0
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
        TableLayoutPanelAutoModeStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelAutoModeStatus.Dock = DockStyle.Fill
        TableLayoutPanelAutoModeStatus.Location = New Point(3, 3)
        TableLayoutPanelAutoModeStatus.Name = "TableLayoutPanelAutoModeStatus"
        TableLayoutPanelAutoModeStatus.RowCount = 2
        TableLayoutPanelAutoModeStatus.RowStyles.Add(New RowStyle())
        TableLayoutPanelAutoModeStatus.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelAutoModeStatus.Size = New Size(1370, 655)
        TableLayoutPanelAutoModeStatus.TabIndex = 0
        ' 
        ' TabPageBgReadings
        ' 
        TabPageBgReadings.Controls.Add(TableLayoutPanelSgReadings)
        TabPageBgReadings.Location = New Point(4, 27)
        TabPageBgReadings.Name = "TabPageBgReadings"
        TabPageBgReadings.Padding = New Padding(3)
        TabPageBgReadings.Size = New Size(1376, 661)
        TabPageBgReadings.TabIndex = 2
        TabPageBgReadings.Text = "Bg Readings"
        TabPageBgReadings.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanelSgReadings
        ' 
        TableLayoutPanelSgReadings.AutoScroll = True
        TableLayoutPanelSgReadings.AutoSize = True
        TableLayoutPanelSgReadings.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanelSgReadings.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TableLayoutPanelSgReadings.ColumnCount = 1
        TableLayoutPanelSgReadings.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelSgReadings.Dock = DockStyle.Fill
        TableLayoutPanelSgReadings.Location = New Point(3, 3)
        TableLayoutPanelSgReadings.Name = "TableLayoutPanelSgReadings"
        TableLayoutPanelSgReadings.RowCount = 2
        TableLayoutPanelSgReadings.RowStyles.Add(New RowStyle())
        TableLayoutPanelSgReadings.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelSgReadings.Size = New Size(1370, 655)
        TableLayoutPanelSgReadings.TabIndex = 1
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
        TableLayoutPanelCalibration.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelCalibration.Dock = DockStyle.Fill
        TableLayoutPanelCalibration.Location = New Point(3, 3)
        TableLayoutPanelCalibration.Name = "TableLayoutPanelCalibration"
        TableLayoutPanelCalibration.RowCount = 2
        TableLayoutPanelCalibration.RowStyles.Add(New RowStyle())
        TableLayoutPanelCalibration.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelCalibration.Size = New Size(1370, 655)
        TableLayoutPanelCalibration.TabIndex = 1
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
        TableLayoutPanelLowGlucoseSuspended.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLowGlucoseSuspended.Dock = DockStyle.Fill
        TableLayoutPanelLowGlucoseSuspended.Location = New Point(3, 3)
        TableLayoutPanelLowGlucoseSuspended.Name = "TableLayoutPanelLowGlucoseSuspended"
        TableLayoutPanelLowGlucoseSuspended.RowCount = 2
        TableLayoutPanelLowGlucoseSuspended.RowStyles.Add(New RowStyle())
        TableLayoutPanelLowGlucoseSuspended.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelLowGlucoseSuspended.Size = New Size(1370, 655)
        TableLayoutPanelLowGlucoseSuspended.TabIndex = 1
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
        TableLayoutPanelTimeChange.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelTimeChange.Dock = DockStyle.Fill
        TableLayoutPanelTimeChange.Location = New Point(3, 3)
        TableLayoutPanelTimeChange.Name = "TableLayoutPanelTimeChange"
        TableLayoutPanelTimeChange.RowCount = 2
        TableLayoutPanelTimeChange.RowStyles.Add(New RowStyle())
        TableLayoutPanelTimeChange.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanelTimeChange.Size = New Size(1370, 655)
        TableLayoutPanelTimeChange.TabIndex = 1
        ' 
        ' WebView
        ' 
        WebView.AllowExternalDrop = False
        WebView.CreationProperties = Nothing
        WebView.DefaultBackgroundColor = Color.White
        WebView.Dock = DockStyle.Fill
        WebView.Location = New Point(597, 6)
        WebView.Name = "WebView"
        WebView.Size = New Size(767, 643)
        WebView.TabIndex = 3
        WebView.ZoomFactor = 1.0R
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
        StatusStripSpeech.Size = New Size(404, 20)
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
        StatusStripSpacerRight.Size = New Size(404, 20)
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
        UpdateAvailableStatusStripLabel.Size = New Size(154, 20)
        UpdateAvailableStatusStripLabel.Text = "Update Status Unknown"
        ' 
        ' Form1
        ' 
        Me.AutoScaleDimensions = New SizeF(96.0F, 96.0F)
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
        CType(SmartGuardShieldPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).EndInit()
        CursorPanel.ResumeLayout(False)
        CType(CursorPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCareLinkUsers, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCurrentUser, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvInsulin, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvMeal, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSGs, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSummary, ComponentModel.ISupportInitialize).EndInit()
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
        TabPage06Meal.ResumeLayout(False)
        TabPage06Meal.PerformLayout()
        TabPage07ActiveInsulin.ResumeLayout(False)
        TabPage07ActiveInsulin.PerformLayout()
        TabPage08SensorGlucose.ResumeLayout(False)
        TabPage08SensorGlucose.PerformLayout()
        TabPage09Limits.ResumeLayout(False)
        TabPage09Limits.PerformLayout()
        TabPage10NotificationHistory.ResumeLayout(False)
        TabPage10NotificationHistory.PerformLayout()
        TabPage11TherapyAlgorithm.ResumeLayout(False)
        TabPage11TherapyAlgorithm.PerformLayout()
        TabPage12BannerState.ResumeLayout(False)
        TabPage12BannerState.PerformLayout()
        TabPage13Basal.ResumeLayout(False)
        TabPageLastSG.ResumeLayout(False)
        TabPageLastSG.PerformLayout()
        TabPageLastAlarm.ResumeLayout(False)
        TabPageLastAlarm.PerformLayout()
        TabControlPage2.ResumeLayout(False)
        TabPageAutoBasalDelivery.ResumeLayout(False)
        TabPageAutoBasalDelivery.PerformLayout()
        TabPageAutoModeStatus.ResumeLayout(False)
        TabPageAutoModeStatus.PerformLayout()
        TabPageBgReadings.ResumeLayout(False)
        TabPageBgReadings.PerformLayout()
        TabPageCalibration.ResumeLayout(False)
        TabPageCalibration.PerformLayout()
        TabPageLowGlucoseSuspended.ResumeLayout(False)
        TabPageLowGlucoseSuspended.PerformLayout()
        TabPageTimeChange.ResumeLayout(False)
        TabPageTimeChange.PerformLayout()
        CType(WebView, ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents CareLinkUserDataRecordBindingSource As BindingSource
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
    Friend WithEvents DgvCurrentUser As DataGridView
    Friend WithEvents DgvInsulin As DataGridView
    Friend WithEvents DgvMeal As DataGridView
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
    Friend WithEvents Last24AutoCorrectionPercentLabel As Label
    Friend WithEvents Last24AutoCorrectionUnitsLabel As Label
    Friend WithEvents Last24BasalLabel As Label
    Friend WithEvents Last24BasalPercentLabel As Label
    Friend WithEvents Last24BasalUnitsLabel As Label
    Friend WithEvents Last24CarbsLabel As Label
    Friend WithEvents Last24CarbsValueLabel As Label
    Friend WithEvents Last24HoursGraphLabel As Label
    Friend WithEvents Last24HTotalsPanel As Panel
    Friend WithEvents Last24ManualBolusPercentLabel As Label
    Friend WithEvents Last24ManualBolusUnitsLabel As Label
    Friend WithEvents Last24MealBolusLabel As Label
    Friend WithEvents Last24TotalInsulinLabel As Label
    Friend WithEvents Last24TotalInsulinUnitsLabel As Label
    Friend WithEvents Last24TotalsLabel As Label
    Friend WithEvents LastSgOrExitTimeLabel As Label
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
    Friend WithEvents MenuOptionsAdvancedOptions As ToolStripMenuItem
    Friend WithEvents MenuOptionsAudioAlerts As ToolStripMenuItem
    Friend WithEvents MenuOptionsAutoLogin As ToolStripMenuItem
    Friend WithEvents MenuOptionsColorPicker As ToolStripMenuItem
    Friend WithEvents MenuOptionsEditPumpSettings As ToolStripMenuItem
    Friend WithEvents MenuOptionsFilterRawJSONData As ToolStripMenuItem
    Friend WithEvents MenuOptionsShowChartLegends As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechHelpShown As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognition80 As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognition85 As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognition90 As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognition95 As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognitionConfidence As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognitionDisabled As ToolStripMenuItem
    Friend WithEvents MenuOptionsSpeechRecognitionEnabled As ToolStripMenuItem
    Friend WithEvents MenuOptionsUseLocalTimeZone As ToolStripMenuItem
    Friend WithEvents MenuShowMiniDisplay As ToolStripMenuItem
    Friend WithEvents MenuStartHere As ToolStripMenuItem
    Friend WithEvents MenuStartHereCleanUpObsoleteFiles As ToolStripMenuItem
    Friend WithEvents MenuStartHereExceptionReportLoad As ToolStripMenuItem
    Friend WithEvents MenuStartHereExit As ToolStripMenuItem
    Friend WithEvents MenuStartHereLoadSavedDataFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereLogin As ToolStripMenuItem
    Friend WithEvents MenuStartHereManuallyImportDeviceSettings As ToolStripMenuItem
    Friend WithEvents MenuStartHereShowPumpSetup As ToolStripMenuItem
    Friend WithEvents MenuStartHereSnapshotSave As ToolStripMenuItem
    Friend WithEvents MenuStartHereUseLastSavedFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereUseTestData As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ModelLabel As Label
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents PumpAITLabel As Label
    Friend WithEvents PumpBannerStateLabel As Label
    Friend WithEvents PumpBatteryPictureBox As PictureBox
    Friend WithEvents PumpBatteryRemaining2Label As Label
    Friend WithEvents PumpBatteryRemainingLabel As Label
    Friend WithEvents PumpNameLabel As Label
    Friend WithEvents ReadingsLabel As Label
    Friend WithEvents RemainingInsulinUnits As Label
    Friend WithEvents SensorDaysLeftLabel As Label
    Friend WithEvents SensorMessageLabel As Label
    Friend WithEvents SensorTimeLeftLabel As Label
    Friend WithEvents SensorTimeLeftPanel As Panel
    Friend WithEvents SensorTimeLeftPictureBox As PictureBox
    Friend WithEvents SerialNumberButton As Button
    Friend WithEvents ServerUpdateTimer As Timer
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents SmartGuardLabel As Label
    Friend WithEvents SmartGuardShieldPictureBox As PictureBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusStripSpacerRight As ToolStripStatusLabel
    Friend WithEvents StatusStripSpeech As ToolStripStatusLabel
    Friend WithEvents TabControlPage1 As TabControl
    Friend WithEvents TabControlPage2 As TabControl
    Friend WithEvents TableLayoutPanelActiveInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelActiveInsulinTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelAutoBasalDelivery As TableLayoutPanel
    Friend WithEvents TableLayoutPanelAutoBasalDeliveryTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelAutoModeStatus As TableLayoutPanel
    Friend WithEvents TableLayoutPanelAutoModeStatusTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelBannerState As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBannerStateTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelBasal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBasalTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelBgReadingsTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelCalibration As TableLayoutPanel
    Friend WithEvents TableLayoutPanelCalibrationTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelInsulinTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLastAlarm As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLastAlarmTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLastSG As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLastSgTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLimits As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLimitsTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelLowGlucoseSuspended As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLowGlucoseSuspendedTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelMeal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelMealTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelNotificationHistory As TableLayoutPanel
    Friend WithEvents TableLayoutPanelNotificationHistoryTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelSgReadings As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSgs As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSgsTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelTherapyAlgorithm As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTherapyAlgorithmTop As TableLayoutPanelTopEx
    Friend WithEvents TableLayoutPanelTimeChange As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTimeChangeTop As TableLayoutPanelTopEx
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
    Friend WithEvents TabPageBgReadings As TabPage
    Friend WithEvents TabPageCalibration As TabPage
    Friend WithEvents TabPageCurrentUser As TabPage
    Friend WithEvents TabPageLastAlarm As TabPage
    Friend WithEvents TabPageLastSG As TabPage
    Friend WithEvents TabPageLowGlucoseSuspended As TabPage
    Friend WithEvents TabPageTimeChange As TabPage
    Friend WithEvents TemporaryUseAdvanceAITDecayCheckBox As CheckBox
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
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ToolStripSplitButton1 As ToolStripSplitButton
    Friend WithEvents ToolStripSplitButton2 As ToolStripSplitButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TransmitterBatteryPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents UpdateAvailableStatusStripLabel As ToolStripStatusLabel
    Friend WithEvents WebView As Microsoft.Web.WebView2.WinForms.WebView2
End Class
