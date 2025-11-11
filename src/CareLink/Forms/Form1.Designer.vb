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
        LastSgOrExitTimeLabel = New Label()
        PumpBannerStateLabel = New Label()
        ShieldUnitsLabel = New Label()
        CurrentSgLabel = New Label()
        SensorMessageLabel = New Label()
        SmartGuardShieldPictureBox = New PictureBox()
        CareLinkUserDataRecordBindingSource = New BindingSource(components)
        CursorMessage1Label = New Label()
        CursorMessage2Label = New Label()
        CursorMessage3Label = New Label()
        CursorMessage4Label = New Label()
        CursorPanel = New Panel()
        CursorPictureBox = New PictureBox()
        CursorTimer = New Timer(components)
        DgvActiveInsulin = New DataGridView()
        DgvAutoBasalDelivery = New DataGridView()
        DgvAutoModeStatus = New DataGridView()
        DgvBasal = New DataGridView()
        DgvBasalPerHour = New DataGridView()
        DgvCalibration = New DataGridView()
        DgvCareLinkUsers = New DataGridView()
        DgvCurrentUser = New DataGridView()
        DgvInsulin = New DataGridView()
        DgvLastAlarm = New DataGridView()
        DgvLastSensorGlucose = New DataGridView()
        DgvLimits = New DataGridView()
        DgvLowGlucoseSuspended = New DataGridView()
        DgvMeal = New DataGridView()
        DgvPumpBannerState = New DataGridView()
        DgvSensorBgReadings = New DataGridView()
        DgvSGs = New DataGridView()
        DgvSummary = New DataGridView()
        DgvTherapyAlgorithmState = New DataGridView()
        DgvTimeChange = New DataGridView()
        FullNameLabel = New Label()
        GraphLast24HoursLabel = New Label()
        HighTirComplianceLabel = New Label()
        ImageList1 = New ImageList(components)
        InsulinLevelPictureBox = New PictureBox()
        InsulinTypeLabel = New Label()
        Last24HrAutoCorrectionLabel = New Label()
        Last24HrAutoCorrectionPercentLabel = New Label()
        Last24HrAutoCorrectionUnitsLabel = New Label()
        Last24HrBasalLabel = New Label()
        Last24HrBasalPercentLabel = New Label()
        Last24HrBasalUnitsLabel = New Label()
        Last24HrCarbsLabel = New Label()
        Last24HrCarbsValueLabel = New Label()
        Last24HrMealBolusLabel = New Label()
        Last24HrMealBolusPercentLabel = New Label()
        Last24HrMealBolusUnitsLabel = New Label()
        Last24HrSummaryLabel = New Label()
        Last24HrTotalInsulinLabel = New Label()
        Last24HrTotalInsulinUnitsLabel = New Label()
        Last24HrTotalsPanel = New Panel()
        LastUpdateTimeToolStripStatusLabel = New ToolStripStatusLabel()
        LoginStatus = New ToolStripStatusLabel()
        LowTirComplianceLabel = New Label()
        MaxBasalPerHourLabel = New Label()
        MenuHelp = New ToolStripMenuItem()
        MenuHelpReportAnIssue = New ToolStripMenuItem()
        MenuHelpCheckForUpdates = New ToolStripMenuItem()
        MenuHelpAbout = New ToolStripMenuItem()
        MenuOptions = New ToolStripMenuItem()
        MenuOptionsAudioAlerts = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionEnabled = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionDisabled = New ToolStripMenuItem()
        MenuOptionsSpeechRecognitionConfidence = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition95 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition90 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition85 = New ToolStripMenuItem()
        MenuOptionsSpeechRecognition80 = New ToolStripMenuItem()
        MenuOptionsShowChartLegends = New ToolStripMenuItem()
        MenuOptionsSpeechHelpShown = New ToolStripMenuItem()
        ToolStripSeparator5 = New ToolStripSeparator()
        MenuOptionsAutoLogin = New ToolStripMenuItem()
        ToolStripSeparator6 = New ToolStripSeparator()
        MenuOptionsAdvancedOptions = New ToolStripMenuItem()
        MenuOptionsFilterRawJSONData = New ToolStripMenuItem()
        MenuOptionsUseLocalTimeZone = New ToolStripMenuItem()
        ToolStripSeparator7 = New ToolStripSeparator()
        MenuOptionsColorPicker = New ToolStripMenuItem()
        MenuOptionsConfigureTiTR = New ToolStripMenuItem()
        MenuOptionsEditPumpSettings = New ToolStripMenuItem()
        MenuShowMiniDisplay = New ToolStripMenuItem()
        MenuStartHere = New ToolStripMenuItem()
        MenuStartHereUserLogin = New ToolStripMenuItem()
        ToolStripSeparator8 = New ToolStripSeparator()
        MenuStartHereManuallyImportDeviceSettings = New ToolStripMenuItem()
        MenuStartHereShowPumpSetup = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        MenuStartHereLoadSavedDataFile = New ToolStripMenuItem()
        MenuStartHereUseExceptionReport = New ToolStripMenuItem()
        ToolStripSeparator4 = New ToolStripSeparator()
        MenuStartHereUseLastSavedFile = New ToolStripMenuItem()
        MenuStartHereUseTestData = New ToolStripMenuItem()
        ToolStripSeparator2 = New ToolStripSeparator()
        MenuStartHereSaveSnapshotFile = New ToolStripMenuItem()
        MenuStartHereCleanUpObsoleteFiles = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        MenuStartHereExit = New ToolStripMenuItem()
        MenuStrip1 = New MenuStrip()
        ModelLabel = New Label()
        NotifyIcon1 = New NotifyIcon(components)
        PumpAITLabel = New Label()
        PumpBatteryPictureBox = New PictureBox()
        PumpBatteryRemaining2Label = New Label()
        PumpBatteryRemainingLabel = New Label()
        PumpNameLabel = New Label()
        ReadingsLabel = New Label()
        RemainingInsulinUnits = New Label()
        SensorDaysLeftLabel = New Label()
        SensorTimeLeftLabel = New Label()
        SensorTimeLeftPanel = New Panel()
        SensorTimeLeftPictureBox = New PictureBox()
        SerialNumberButton = New Button()
        ServerUpdateTimer = New Timer(components)
        SgTrendLabel = New Label()
        SmartGuardLabel = New Label()
        SplitContainer1 = New SplitContainer()
        TemporaryUseAdvanceAITDecayCheckBox = New CheckBox()
        SplitContainer2 = New SplitContainer()
        TrendValueLabel = New Label()
        TrendArrowsLabel = New Label()
        TransmitterBatteryPercentLabel = New Label()
        TransmitterBatteryPictureBox = New PictureBox()
        SplitContainer3 = New SplitContainer()
        TimeInRangeHeaderLabel = New Label()
        TimeInRangeChartLabel = New Label()
        TimeInRangeSummaryPercentCharLabel = New Label()
        TimeInRangeValueLabel = New Label()
        TimeInTightRangeValueLabel = New Label()
        TimeInRangeMessageLabel = New Label()
        TirComplianceLabel = New Label()
        TiTRMgsLabel = New Label()
        TiTRMgsLabel2 = New Label()
        SplitContainerNotificationsCleared = New SplitContainer()
        TlpNotificationsClearedTop = New TableLayoutPanelTopEx()
        TlpNotificationsCleared = New TableLayoutPanel()
        StatusStrip1 = New StatusStrip()
        StatusStripSpeech = New ToolStripStatusLabel()
        TimeZoneToolStripStatusLabel = New ToolStripStatusLabel()
        StatusStripSpacerRight = New ToolStripStatusLabel()
        UpdateAvailableStatusStripLabel = New ToolStripStatusLabel()
        TabPage01HomePage = New TabPage()
        TabPage02RunningIOB = New TabPage()
        TabPage03TreatmentDetails = New TabPage()
        TabPage04SummaryData = New TabPage()
        TabPage05ActiveInsulin = New TabPage()
        TlpActiveInsulin = New TableLayoutPanel()
        TlpActiveInsulinTop = New TableLayoutPanelTopEx()
        TabPage06Basal = New TabPage()
        TlpBasal = New TableLayoutPanel()
        TlpBasalTop = New TableLayoutPanelTopEx()
        TabPage07LastAlarm = New TabPage()
        TlpLastAlarm = New TableLayoutPanel()
        TlpLastAlarmTop = New TableLayoutPanelTopEx()
        TabPage08LastSG = New TabPage()
        TlpLastSG = New TableLayoutPanel()
        TlpLastSgTop = New TableLayoutPanelTopEx()
        TabPage09Limits = New TabPage()
        TlpLimits = New TableLayoutPanel()
        TlpLimitsTop = New TableLayoutPanelTopEx()
        TabPage10NotificationActive = New TabPage()
        TlpNotificationActive = New TableLayoutPanel()
        TlpNotificationActiveTop = New TableLayoutPanelTopEx()
        TabPage11NotificationsCleared = New TabPage()
        TabPage12PumpBannerState = New TabPage()
        TlpPumpBannerState = New TableLayoutPanel()
        TlpPumpBannerStateTop = New TableLayoutPanelTopEx()
        TabPage13SensorGlucose = New TabPage()
        TlpSgs = New TableLayoutPanel()
        TlpSgsTop = New TableLayoutPanelTopEx()
        TabPage14TherapyAlgorithmState = New TabPage()
        TlpTherapyAlgorithmState = New TableLayoutPanel()
        TlpTherapyAlgorithmStateTop = New TableLayoutPanelTopEx()
        TabPage15More = New TabPage()
        TabPage01AutoBasalDelivery = New TabPage()
        TlpAutoBasalDelivery = New TableLayoutPanel()
        TlpAutoBasalDeliveryTop = New TableLayoutPanelTopEx()
        TabPage02AutoModeStatus = New TabPage()
        TlpAutoModeStatus = New TableLayoutPanel()
        TlpAutoModeStatusTop = New TableLayoutPanelTopEx()
        TabPage03BgReadings = New TabPage()
        TlpBgReadings = New TableLayoutPanel()
        TlpBgReadingsTop = New TableLayoutPanelTopEx()
        TabPage04Calibration = New TabPage()
        TlpCalibration = New TableLayoutPanel()
        TlpCalibrationTop = New TableLayoutPanelTopEx()
        TabPage05Insulin = New TabPage()
        TlpInsulin = New TableLayoutPanel()
        TlpInsulinTop = New TableLayoutPanelTopEx()
        TabPage06LowGlucoseSuspended = New TabPage()
        TlpLowGlucoseSuspended = New TableLayoutPanel()
        TlpLowGlucoseSuspendedTop = New TableLayoutPanelTopEx()
        TabPage07Meal = New TabPage()
        TlpMeal = New TableLayoutPanel()
        TlpMealTop = New TableLayoutPanelTopEx()
        TabPage08TimeChange = New TabPage()
        TlpTimeChange = New TableLayoutPanel()
        TlpTimeChangeTop = New TableLayoutPanelTopEx()
        TabPage09BasalPerHour = New TabPage()
        TabPage10CurrentUser = New TabPage()
        TabPage11AllUsers = New TabPage()
        TabPage12BackToHomePage = New TabPage()
        TabControlPage1 = New TabControl()
        TabControlPage2 = New TabControl()
        TimeChangeLabel = New Label()
        TimeInTightRangeLabel = New Label()
        ToolTip1 = New ToolTip(components)
        WebView = New Microsoft.Web.WebView2.WinForms.WebView2()
        CType(CalibrationDueImage, ComponentModel.ISupportInitialize).BeginInit()
        CalibrationShieldPanel.SuspendLayout()
        CType(SmartGuardShieldPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).BeginInit()
        CursorPanel.SuspendLayout()
        CType(CursorPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvActiveInsulin, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvAutoModeStatus, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvBasal, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvBasalPerHour, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCalibration, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCareLinkUsers, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvCurrentUser, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvInsulin, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvLastAlarm, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvLastSensorGlucose, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvLimits, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvLowGlucoseSuspended, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvMeal, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvPumpBannerState, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSensorBgReadings, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSGs, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvSummary, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvTherapyAlgorithmState, ComponentModel.ISupportInitialize).BeginInit()
        CType(DgvTimeChange, ComponentModel.ISupportInitialize).BeginInit()
        CType(InsulinLevelPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Last24HrTotalsPanel.SuspendLayout()
        MenuStrip1.SuspendLayout()
        CType(PumpBatteryPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SensorTimeLeftPanel.SuspendLayout()
        CType(SensorTimeLeftPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        CType(TransmitterBatteryPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer3, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer3.Panel2.SuspendLayout()
        SplitContainer3.SuspendLayout()
        CType(SplitContainerNotificationsCleared, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainerNotificationsCleared.Panel1.SuspendLayout()
        SplitContainerNotificationsCleared.Panel2.SuspendLayout()
        SplitContainerNotificationsCleared.SuspendLayout()
        StatusStrip1.SuspendLayout()
        TabPage01HomePage.SuspendLayout()
        TabPage02RunningIOB.SuspendLayout()
        TabPage04SummaryData.SuspendLayout()
        TabPage05ActiveInsulin.SuspendLayout()
        TlpActiveInsulin.SuspendLayout()
        TabPage06Basal.SuspendLayout()
        TlpBasal.SuspendLayout()
        TabPage07LastAlarm.SuspendLayout()
        TlpLastAlarm.SuspendLayout()
        TabPage08LastSG.SuspendLayout()
        TlpLastSG.SuspendLayout()
        TabPage09Limits.SuspendLayout()
        TlpLimits.SuspendLayout()
        TabPage10NotificationActive.SuspendLayout()
        TlpNotificationActive.SuspendLayout()
        TabPage11NotificationsCleared.SuspendLayout()
        TabPage12PumpBannerState.SuspendLayout()
        TlpPumpBannerState.SuspendLayout()
        TabPage13SensorGlucose.SuspendLayout()
        TlpSgs.SuspendLayout()
        TabPage14TherapyAlgorithmState.SuspendLayout()
        TlpTherapyAlgorithmState.SuspendLayout()
        TabPage01AutoBasalDelivery.SuspendLayout()
        TlpAutoBasalDelivery.SuspendLayout()
        TabPage02AutoModeStatus.SuspendLayout()
        TlpAutoModeStatus.SuspendLayout()
        TabPage03BgReadings.SuspendLayout()
        TlpBgReadings.SuspendLayout()
        TabPage04Calibration.SuspendLayout()
        TlpCalibration.SuspendLayout()
        TabPage05Insulin.SuspendLayout()
        TlpInsulin.SuspendLayout()
        TabPage06LowGlucoseSuspended.SuspendLayout()
        TlpLowGlucoseSuspended.SuspendLayout()
        TabPage07Meal.SuspendLayout()
        TlpMeal.SuspendLayout()
        TabPage08TimeChange.SuspendLayout()
        TlpTimeChange.SuspendLayout()
        TabPage09BasalPerHour.SuspendLayout()
        TabPage10CurrentUser.SuspendLayout()
        TabPage11AllUsers.SuspendLayout()
        TabControlPage1.SuspendLayout()
        TabControlPage2.SuspendLayout()
        CType(WebView, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' AboveHighLimitMessageLabel
        ' 
        AboveHighLimitMessageLabel.Anchor = AnchorStyles.Top
        AboveHighLimitMessageLabel.AutoSize = True
        AboveHighLimitMessageLabel.BackColor = Color.Transparent
        AboveHighLimitMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        AboveHighLimitMessageLabel.ForeColor = Color.Yellow
        AboveHighLimitMessageLabel.Location = New Point(44, 211)
        AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        AboveHighLimitMessageLabel.Size = New Size(144, 21)
        AboveHighLimitMessageLabel.TabIndex = 28
        AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        AboveHighLimitMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' AboveHighLimitValueLabel
        ' 
        AboveHighLimitValueLabel.Anchor = AnchorStyles.Top
        AboveHighLimitValueLabel.AutoSize = True
        AboveHighLimitValueLabel.BackColor = Color.Black
        AboveHighLimitValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
        AboveHighLimitValueLabel.ForeColor = Color.Yellow
        AboveHighLimitValueLabel.Location = New Point(87, 169)
        AboveHighLimitValueLabel.Name = "AboveHighLimitValueLabel"
        AboveHighLimitValueLabel.Size = New Size(59, 40)
        AboveHighLimitValueLabel.TabIndex = 22
        AboveHighLimitValueLabel.Text = "8%"
        AboveHighLimitValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' ActiveInsulinValue
        ' 
        ActiveInsulinValue.BackColor = Color.Transparent
        ActiveInsulinValue.BorderStyle = BorderStyle.FixedSingle
        ActiveInsulinValue.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        ActiveInsulinValue.ForeColor = Color.LightGray
        ActiveInsulinValue.Location = New Point(995, 53)
        ActiveInsulinValue.Name = "ActiveInsulinValue"
        ActiveInsulinValue.Size = New Size(128, 48)
        ActiveInsulinValue.TabIndex = 0
        ActiveInsulinValue.Text = "Active Insulin 0.000U"
        ActiveInsulinValue.TextAlign = ContentAlignment.TopCenter
        ' 
        ' AverageSGMessageLabel
        ' 
        AverageSGMessageLabel.Anchor = AnchorStyles.Top
        AverageSGMessageLabel.AutoSize = True
        AverageSGMessageLabel.BackColor = Color.Transparent
        AverageSGMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        AverageSGMessageLabel.ForeColor = Color.LightGray
        AverageSGMessageLabel.Location = New Point(33, 398)
        AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        AverageSGMessageLabel.Size = New Size(167, 21)
        AverageSGMessageLabel.TabIndex = 0
        AverageSGMessageLabel.Text = "Average SG in XX/XX"
        AverageSGMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' AverageSGValueLabel
        ' 
        AverageSGValueLabel.Anchor = AnchorStyles.Top
        AverageSGValueLabel.AutoSize = True
        AverageSGValueLabel.BackColor = Color.Black
        AverageSGValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
        AverageSGValueLabel.ForeColor = Color.LightGray
        AverageSGValueLabel.Location = New Point(82, 356)
        AverageSGValueLabel.Margin = New Padding(0)
        AverageSGValueLabel.Name = "AverageSGValueLabel"
        AverageSGValueLabel.Size = New Size(68, 40)
        AverageSGValueLabel.TabIndex = 1
        AverageSGValueLabel.Text = "100"
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
        BelowLowLimitMessageLabel.AutoSize = True
        BelowLowLimitMessageLabel.BackColor = Color.Transparent
        BelowLowLimitMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        BelowLowLimitMessageLabel.ForeColor = Color.Red
        BelowLowLimitMessageLabel.Location = New Point(45, 333)
        BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        BelowLowLimitMessageLabel.Size = New Size(142, 21)
        BelowLowLimitMessageLabel.TabIndex = 32
        BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        BelowLowLimitMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' BelowLowLimitValueLabel
        ' 
        BelowLowLimitValueLabel.Anchor = AnchorStyles.Top
        BelowLowLimitValueLabel.AutoSize = True
        BelowLowLimitValueLabel.BackColor = Color.Black
        BelowLowLimitValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
        BelowLowLimitValueLabel.ForeColor = Color.Red
        BelowLowLimitValueLabel.Location = New Point(87, 291)
        BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        BelowLowLimitValueLabel.Size = New Size(59, 40)
        BelowLowLimitValueLabel.TabIndex = 26
        BelowLowLimitValueLabel.Text = "2%"
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
        CalibrationShieldPanel.Size = New Size(120, 132)
        CalibrationShieldPanel.TabIndex = 64
        ' 
        ' LastSgOrExitTimeLabel
        ' 
        LastSgOrExitTimeLabel.AutoSize = True
        LastSgOrExitTimeLabel.BackColor = Color.Transparent
        LastSgOrExitTimeLabel.Dock = DockStyle.Bottom
        LastSgOrExitTimeLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        LastSgOrExitTimeLabel.ForeColor = Color.LightGray
        LastSgOrExitTimeLabel.Location = New Point(0, 90)
        LastSgOrExitTimeLabel.Name = "LastSgOrExitTimeLabel"
        LastSgOrExitTimeLabel.Size = New Size(124, 21)
        LastSgOrExitTimeLabel.TabIndex = 55
        LastSgOrExitTimeLabel.Text = "Exit in: 3:27 hrs"
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
        PumpBannerStateLabel.Size = New Size(120, 15)
        PumpBannerStateLabel.TabIndex = 56
        PumpBannerStateLabel.Text = "Target 150 2:00 Hr"
        PumpBannerStateLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' ShieldUnitsLabel
        ' 
        ShieldUnitsLabel.AutoSize = True
        ShieldUnitsLabel.BackColor = Color.Transparent
        ShieldUnitsLabel.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold)
        ShieldUnitsLabel.ForeColor = Color.LightGray
        ShieldUnitsLabel.Location = New Point(38, 60)
        ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        ShieldUnitsLabel.Size = New Size(40, 13)
        ShieldUnitsLabel.TabIndex = 8
        ShieldUnitsLabel.Text = "XX/XX"
        ' 
        ' CurrentSgLabel
        ' 
        CurrentSgLabel.BackColor = Color.Transparent
        CurrentSgLabel.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)
        CurrentSgLabel.ForeColor = Color.LightGray
        CurrentSgLabel.Location = New Point(22, 30)
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
        SensorMessageLabel.ForeColor = Color.LightGray
        SensorMessageLabel.Location = New Point(0, 8)
        SensorMessageLabel.Name = "SensorMessageLabel"
        SensorMessageLabel.Size = New Size(116, 57)
        SensorMessageLabel.TabIndex = 1
        SensorMessageLabel.Text = "Calibration Required"
        SensorMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SmartGuardShieldPictureBox
        ' 
        SmartGuardShieldPictureBox.Image = My.Resources.Resources.Shield
        SmartGuardShieldPictureBox.Location = New Point(0, 16)
        SmartGuardShieldPictureBox.Margin = New Padding(5)
        SmartGuardShieldPictureBox.Name = "SmartGuardShieldPictureBox"
        SmartGuardShieldPictureBox.Size = New Size(116, 100)
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
        CursorMessage1Label.ForeColor = Color.LightGray
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
        CursorMessage2Label.ForeColor = Color.LightGray
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
        CursorMessage3Label.ForeColor = Color.LightGray
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
        CursorMessage4Label.ForeColor = Color.LightGray
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
        ' DgvActiveInsulin
        ' 
        DgvActiveInsulin.Dock = DockStyle.Fill
        DgvActiveInsulin.Location = New Point(3, 46)
        DgvActiveInsulin.Name = "DgvActiveInsulin"
        DgvActiveInsulin.ReadOnly = True
        DgvActiveInsulin.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvActiveInsulin.Size = New Size(1370, 609)
        DgvActiveInsulin.TabIndex = 0
        ' 
        ' DgvAutoBasalDelivery
        ' 
        DgvAutoBasalDelivery.Dock = DockStyle.Fill
        DgvAutoBasalDelivery.Location = New Point(6, 52)
        DgvAutoBasalDelivery.Name = "DgvAutoBasalDelivery"
        DgvAutoBasalDelivery.ReadOnly = True
        DgvAutoBasalDelivery.RowHeadersVisible = False
        DgvAutoBasalDelivery.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvAutoBasalDelivery.Size = New Size(1364, 600)
        DgvAutoBasalDelivery.TabIndex = 0
        ' 
        ' DgvAutoModeStatus
        ' 
        DgvAutoModeStatus.Dock = DockStyle.Fill
        DgvAutoModeStatus.Location = New Point(6, 52)
        DgvAutoModeStatus.Name = "DgvAutoModeStatus"
        DgvAutoModeStatus.ReadOnly = True
        DgvAutoModeStatus.RowHeadersVisible = False
        DgvAutoModeStatus.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvAutoModeStatus.Size = New Size(1364, 600)
        DgvAutoModeStatus.TabIndex = 0
        ' 
        ' DgvBasal
        ' 
        DgvBasal.Dock = DockStyle.Fill
        DgvBasal.Location = New Point(6, 52)
        DgvBasal.Name = "DgvBasal"
        DgvBasal.ReadOnly = True
        DgvBasal.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvBasal.Size = New Size(1364, 600)
        DgvBasal.TabIndex = 0
        ' 
        ' DgvBasalPerHour
        ' 
        DgvBasalPerHour.Dock = DockStyle.Fill
        DgvBasalPerHour.Location = New Point(0, 0)
        DgvBasalPerHour.Name = "DgvBasalPerHour"
        DgvBasalPerHour.ReadOnly = True
        DgvBasalPerHour.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvBasalPerHour.Size = New Size(1376, 658)
        DgvBasalPerHour.TabIndex = 0
        ' 
        ' DgvCalibration
        ' 
        DgvCalibration.AllowUserToAddRows = False
        DgvCalibration.AllowUserToDeleteRows = False
        DgvCalibration.Dock = DockStyle.Fill
        DgvCalibration.Location = New Point(6, 52)
        DgvCalibration.Name = "DgvCalibration"
        DgvCalibration.ReadOnly = True
        DgvCalibration.Size = New Size(1364, 600)
        DgvCalibration.TabIndex = 2
        ' 
        ' DgvCareLinkUsers
        ' 
        DgvCareLinkUsers.AllowUserToAddRows = False
        DgvCareLinkUsers.AllowUserToResizeColumns = False
        DgvCareLinkUsers.AllowUserToResizeRows = False
        DgvCareLinkUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DgvCareLinkUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DgvCareLinkUsers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DgvCareLinkUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = SystemColors.Control
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        DgvCareLinkUsers.DefaultCellStyle = DataGridViewCellStyle2
        DgvCareLinkUsers.Dock = DockStyle.Fill
        DgvCareLinkUsers.EditMode = DataGridViewEditMode.EditOnEnter
        DgvCareLinkUsers.Location = New Point(0, 0)
        DgvCareLinkUsers.Name = "DgvCareLinkUsers"
        DgvCareLinkUsers.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvCareLinkUsers.Size = New Size(1376, 658)
        DgvCareLinkUsers.TabIndex = 0
        ' 
        ' DgvCurrentUser
        ' 
        DgvCurrentUser.Dock = DockStyle.Fill
        DgvCurrentUser.Location = New Point(0, 0)
        DgvCurrentUser.Name = "DgvCurrentUser"
        DgvCurrentUser.ReadOnly = True
        DgvCurrentUser.RowHeadersVisible = False
        DgvCurrentUser.Size = New Size(1376, 658)
        DgvCurrentUser.TabIndex = 0
        ' 
        ' DgvInsulin
        ' 
        DgvInsulin.Dock = DockStyle.Fill
        DgvInsulin.Location = New Point(6, 52)
        DgvInsulin.Name = "DgvInsulin"
        DgvInsulin.ReadOnly = True
        DgvInsulin.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvInsulin.Size = New Size(1364, 600)
        DgvInsulin.TabIndex = 0
        ' 
        ' DgvLastAlarm
        ' 
        DgvLastAlarm.Dock = DockStyle.Fill
        DgvLastAlarm.Location = New Point(6, 52)
        DgvLastAlarm.Name = "DgvLastAlarm"
        DgvLastAlarm.ReadOnly = True
        DgvLastAlarm.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvLastAlarm.Size = New Size(1364, 600)
        DgvLastAlarm.TabIndex = 0
        ' 
        ' DgvLastSensorGlucose
        ' 
        DgvLastSensorGlucose.Dock = DockStyle.Fill
        DgvLastSensorGlucose.Location = New Point(6, 52)
        DgvLastSensorGlucose.Name = "DgvLastSensorGlucose"
        DgvLastSensorGlucose.ReadOnly = True
        DgvLastSensorGlucose.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvLastSensorGlucose.Size = New Size(1364, 600)
        DgvLastSensorGlucose.TabIndex = 0
        ' 
        ' DgvLimits
        ' 
        DgvLimits.Dock = DockStyle.Fill
        DgvLimits.Location = New Point(6, 52)
        DgvLimits.Name = "DgvLimits"
        DgvLimits.ReadOnly = True
        DgvLimits.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvLimits.Size = New Size(1364, 600)
        DgvLimits.TabIndex = 0
        ' 
        ' DgvLowGlucoseSuspended
        ' 
        DgvLowGlucoseSuspended.Dock = DockStyle.Fill
        DgvLowGlucoseSuspended.Location = New Point(6, 52)
        DgvLowGlucoseSuspended.Name = "DgvLowGlucoseSuspended"
        DgvLowGlucoseSuspended.ReadOnly = True
        DgvLowGlucoseSuspended.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvLowGlucoseSuspended.Size = New Size(1364, 600)
        DgvLowGlucoseSuspended.TabIndex = 2
        ' 
        ' DgvMeal
        ' 
        DgvMeal.Dock = DockStyle.Fill
        DgvMeal.Location = New Point(6, 52)
        DgvMeal.Name = "DgvMeal"
        DgvMeal.ReadOnly = True
        DgvMeal.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvMeal.Size = New Size(1364, 600)
        DgvMeal.TabIndex = 0
        ' 
        ' DgvPumpBannerState
        ' 
        DgvPumpBannerState.Dock = DockStyle.Fill
        DgvPumpBannerState.Location = New Point(6, 52)
        DgvPumpBannerState.Name = "DgvPumpBannerState"
        DgvPumpBannerState.ReadOnly = True
        DgvPumpBannerState.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvPumpBannerState.Size = New Size(1364, 600)
        DgvPumpBannerState.TabIndex = 0
        ' 
        ' DgvSensorBgReadings
        ' 
        DgvSensorBgReadings.Dock = DockStyle.Fill
        DgvSensorBgReadings.Location = New Point(6, 52)
        DgvSensorBgReadings.Name = "DgvSensorBgReadings"
        DgvSensorBgReadings.ReadOnly = True
        DgvSensorBgReadings.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvSensorBgReadings.Size = New Size(1364, 600)
        DgvSensorBgReadings.TabIndex = 2
        ' 
        ' DgvSGs
        ' 
        DgvSGs.Dock = DockStyle.Fill
        DgvSGs.Location = New Point(6, 52)
        DgvSGs.Name = "DgvSGs"
        DgvSGs.ReadOnly = True
        DgvSGs.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvSGs.Size = New Size(1364, 600)
        DgvSGs.TabIndex = 0
        ' 
        ' DgvSummary
        ' 
        DgvSummary.Dock = DockStyle.Fill
        DgvSummary.Location = New Point(0, 0)
        DgvSummary.Name = "DgvSummary"
        DgvSummary.ReadOnly = True
        DgvSummary.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvSummary.Size = New Size(1376, 658)
        DgvSummary.TabIndex = 0
        ' 
        ' DgvTherapyAlgorithmState
        ' 
        DgvTherapyAlgorithmState.Dock = DockStyle.Fill
        DgvTherapyAlgorithmState.Location = New Point(6, 52)
        DgvTherapyAlgorithmState.Name = "DgvTherapyAlgorithmState"
        DgvTherapyAlgorithmState.ReadOnly = True
        DgvTherapyAlgorithmState.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvTherapyAlgorithmState.Size = New Size(1364, 600)
        DgvTherapyAlgorithmState.TabIndex = 0
        ' 
        ' DgvTimeChange
        ' 
        DgvTimeChange.Dock = DockStyle.Fill
        DgvTimeChange.Location = New Point(6, 52)
        DgvTimeChange.Name = "DgvTimeChange"
        DgvTimeChange.ReadOnly = True
        DgvTimeChange.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvTimeChange.Size = New Size(1364, 600)
        DgvTimeChange.TabIndex = 2
        ' 
        ' FullNameLabel
        ' 
        FullNameLabel.BackColor = Color.Transparent
        FullNameLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        FullNameLabel.ForeColor = Color.LightGray
        FullNameLabel.Location = New Point(1140, 0)
        FullNameLabel.Margin = New Padding(0)
        FullNameLabel.Name = "FullNameLabel"
        FullNameLabel.Size = New Size(230, 21)
        FullNameLabel.TabIndex = 8
        FullNameLabel.Text = "User Name"
        FullNameLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' GraphLast24HoursLabel
        ' 
        GraphLast24HoursLabel.Anchor = AnchorStyles.Top
        GraphLast24HoursLabel.AutoSize = True
        GraphLast24HoursLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        GraphLast24HoursLabel.ForeColor = Color.Gray
        GraphLast24HoursLabel.Location = New Point(62, 22)
        GraphLast24HoursLabel.Name = "GraphLast24HoursLabel"
        GraphLast24HoursLabel.Size = New Size(109, 21)
        GraphLast24HoursLabel.TabIndex = 34
        GraphLast24HoursLabel.Text = "Last 24 hours"
        GraphLast24HoursLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' HighTirComplianceLabel
        ' 
        HighTirComplianceLabel.AutoSize = True
        HighTirComplianceLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        HighTirComplianceLabel.ForeColor = Color.LimeGreen
        HighTirComplianceLabel.Location = New Point(142, 479)
        HighTirComplianceLabel.Name = "HighTirComplianceLabel"
        HighTirComplianceLabel.Size = New Size(68, 34)
        HighTirComplianceLabel.TabIndex = 37
        HighTirComplianceLabel.Text = "High" & vbCrLf & "Excellent²"
        HighTirComplianceLabel.TextAlign = ContentAlignment.MiddleCenter
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
        ' InsulinTypeLabel
        ' 
        InsulinTypeLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        InsulinTypeLabel.ForeColor = Color.LightGray
        InsulinTypeLabel.Location = New Point(978, 3)
        InsulinTypeLabel.Name = "InsulinTypeLabel"
        InsulinTypeLabel.Size = New Size(162, 21)
        InsulinTypeLabel.TabIndex = 54
        InsulinTypeLabel.Text = "Humalog/Novolog"
        InsulinTypeLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HrAutoCorrectionLabel
        ' 
        Last24HrAutoCorrectionLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrAutoCorrectionLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrAutoCorrectionLabel.ForeColor = Color.Gray
        Last24HrAutoCorrectionLabel.Location = New Point(0, 43)
        Last24HrAutoCorrectionLabel.Name = "Last24HrAutoCorrectionLabel"
        Last24HrAutoCorrectionLabel.Size = New Size(145, 21)
        Last24HrAutoCorrectionLabel.TabIndex = 64
        Last24HrAutoCorrectionLabel.Text = "Auto Correction:"
        Last24HrAutoCorrectionLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HrAutoCorrectionPercentLabel
        ' 
        Last24HrAutoCorrectionPercentLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrAutoCorrectionPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrAutoCorrectionPercentLabel.ForeColor = Color.LightGray
        Last24HrAutoCorrectionPercentLabel.Location = New Point(200, 43)
        Last24HrAutoCorrectionPercentLabel.Name = "Last24HrAutoCorrectionPercentLabel"
        Last24HrAutoCorrectionPercentLabel.Size = New Size(46, 21)
        Last24HrAutoCorrectionPercentLabel.TabIndex = 73
        Last24HrAutoCorrectionPercentLabel.Text = " 20%"
        Last24HrAutoCorrectionPercentLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrAutoCorrectionUnitsLabel
        ' 
        Last24HrAutoCorrectionUnitsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrAutoCorrectionUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrAutoCorrectionUnitsLabel.ForeColor = Color.LightGray
        Last24HrAutoCorrectionUnitsLabel.Location = New Point(131, 43)
        Last24HrAutoCorrectionUnitsLabel.Name = "Last24HrAutoCorrectionUnitsLabel"
        Last24HrAutoCorrectionUnitsLabel.Size = New Size(70, 21)
        Last24HrAutoCorrectionUnitsLabel.TabIndex = 72
        Last24HrAutoCorrectionUnitsLabel.Text = " 20.0U"
        Last24HrAutoCorrectionUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrBasalLabel
        ' 
        Last24HrBasalLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrBasalLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrBasalLabel.ForeColor = Color.Gray
        Last24HrBasalLabel.Location = New Point(0, 23)
        Last24HrBasalLabel.Name = "Last24HrBasalLabel"
        Last24HrBasalLabel.Size = New Size(110, 21)
        Last24HrBasalLabel.TabIndex = 62
        Last24HrBasalLabel.Text = "Basal:"
        Last24HrBasalLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HrBasalPercentLabel
        ' 
        Last24HrBasalPercentLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrBasalPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrBasalPercentLabel.ForeColor = Color.LightGray
        Last24HrBasalPercentLabel.Location = New Point(200, 23)
        Last24HrBasalPercentLabel.Name = "Last24HrBasalPercentLabel"
        Last24HrBasalPercentLabel.Size = New Size(46, 21)
        Last24HrBasalPercentLabel.TabIndex = 69
        Last24HrBasalPercentLabel.Text = "50%"
        Last24HrBasalPercentLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrBasalUnitsLabel
        ' 
        Last24HrBasalUnitsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrBasalUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrBasalUnitsLabel.ForeColor = Color.LightGray
        Last24HrBasalUnitsLabel.Location = New Point(131, 23)
        Last24HrBasalUnitsLabel.Name = "Last24HrBasalUnitsLabel"
        Last24HrBasalUnitsLabel.Size = New Size(70, 21)
        Last24HrBasalUnitsLabel.TabIndex = 68
        Last24HrBasalUnitsLabel.Text = " 50.0U"
        Last24HrBasalUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrCarbsLabel
        ' 
        Last24HrCarbsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrCarbsLabel.AutoSize = True
        Last24HrCarbsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrCarbsLabel.ForeColor = Color.Gray
        Last24HrCarbsLabel.Location = New Point(0, 103)
        Last24HrCarbsLabel.Name = "Last24HrCarbsLabel"
        Last24HrCarbsLabel.Size = New Size(98, 21)
        Last24HrCarbsLabel.TabIndex = 66
        Last24HrCarbsLabel.Text = "Total Carbs:"
        Last24HrCarbsLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HrCarbsValueLabel
        ' 
        Last24HrCarbsValueLabel.AutoSize = True
        Last24HrCarbsValueLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrCarbsValueLabel.ForeColor = Color.LightGray
        Last24HrCarbsValueLabel.Location = New Point(157, 103)
        Last24HrCarbsValueLabel.Name = "Last24HrCarbsValueLabel"
        Last24HrCarbsValueLabel.Size = New Size(95, 21)
        Last24HrCarbsValueLabel.TabIndex = 74
        Last24HrCarbsValueLabel.Text = "100 Grams³"
        Last24HrCarbsValueLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrMealBolusLabel
        ' 
        Last24HrMealBolusLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrMealBolusLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrMealBolusLabel.ForeColor = Color.Gray
        Last24HrMealBolusLabel.Location = New Point(0, 64)
        Last24HrMealBolusLabel.Name = "Last24HrMealBolusLabel"
        Last24HrMealBolusLabel.Size = New Size(110, 21)
        Last24HrMealBolusLabel.TabIndex = 63
        Last24HrMealBolusLabel.Text = "Meal Bolus:"
        Last24HrMealBolusLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HrMealBolusPercentLabel
        ' 
        Last24HrMealBolusPercentLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrMealBolusPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrMealBolusPercentLabel.ForeColor = Color.LightGray
        Last24HrMealBolusPercentLabel.Location = New Point(200, 64)
        Last24HrMealBolusPercentLabel.Name = "Last24HrMealBolusPercentLabel"
        Last24HrMealBolusPercentLabel.Size = New Size(46, 21)
        Last24HrMealBolusPercentLabel.TabIndex = 71
        Last24HrMealBolusPercentLabel.Text = " 30%"
        Last24HrMealBolusPercentLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrMealBolusUnitsLabel
        ' 
        Last24HrMealBolusUnitsLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrMealBolusUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrMealBolusUnitsLabel.ForeColor = Color.LightGray
        Last24HrMealBolusUnitsLabel.Location = New Point(131, 64)
        Last24HrMealBolusUnitsLabel.Name = "Last24HrMealBolusUnitsLabel"
        Last24HrMealBolusUnitsLabel.Size = New Size(70, 21)
        Last24HrMealBolusUnitsLabel.TabIndex = 70
        Last24HrMealBolusUnitsLabel.Text = " 30.0U"
        Last24HrMealBolusUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrSummaryLabel
        ' 
        Last24HrSummaryLabel.BackColor = Color.DimGray
        Last24HrSummaryLabel.Dock = DockStyle.Top
        Last24HrSummaryLabel.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        Last24HrSummaryLabel.ForeColor = Color.Black
        Last24HrSummaryLabel.Location = New Point(0, 0)
        Last24HrSummaryLabel.Name = "Last24HrSummaryLabel"
        Last24HrSummaryLabel.Size = New Size(253, 23)
        Last24HrSummaryLabel.TabIndex = 65
        Last24HrSummaryLabel.Text = "Last 24 Hr Summary"
        Last24HrSummaryLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Last24HrTotalInsulinLabel
        ' 
        Last24HrTotalInsulinLabel.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        Last24HrTotalInsulinLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrTotalInsulinLabel.ForeColor = Color.Gray
        Last24HrTotalInsulinLabel.Location = New Point(0, 85)
        Last24HrTotalInsulinLabel.Name = "Last24HrTotalInsulinLabel"
        Last24HrTotalInsulinLabel.Size = New Size(110, 21)
        Last24HrTotalInsulinLabel.TabIndex = 61
        Last24HrTotalInsulinLabel.Text = "Total Insulin:"
        Last24HrTotalInsulinLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Last24HrTotalInsulinUnitsLabel
        ' 
        Last24HrTotalInsulinUnitsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        Last24HrTotalInsulinUnitsLabel.ForeColor = Color.LightGray
        Last24HrTotalInsulinUnitsLabel.Location = New Point(131, 85)
        Last24HrTotalInsulinUnitsLabel.Name = "Last24HrTotalInsulinUnitsLabel"
        Last24HrTotalInsulinUnitsLabel.Size = New Size(70, 21)
        Last24HrTotalInsulinUnitsLabel.TabIndex = 67
        Last24HrTotalInsulinUnitsLabel.Text = "100.0U"
        Last24HrTotalInsulinUnitsLabel.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Last24HrTotalsPanel
        ' 
        Last24HrTotalsPanel.Controls.Add(Last24HrCarbsValueLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrAutoCorrectionLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrAutoCorrectionPercentLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrAutoCorrectionUnitsLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrBasalLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrBasalPercentLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrBasalUnitsLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrCarbsLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrMealBolusLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrMealBolusPercentLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrMealBolusUnitsLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrTotalInsulinLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrTotalInsulinUnitsLabel)
        Last24HrTotalsPanel.Controls.Add(Last24HrSummaryLabel)
        Last24HrTotalsPanel.Location = New Point(724, 0)
        Last24HrTotalsPanel.Name = "Last24HrTotalsPanel"
        Last24HrTotalsPanel.Size = New Size(253, 129)
        Last24HrTotalsPanel.TabIndex = 66
        ' 
        ' LastUpdateTimeToolStripStatusLabel
        ' 
        LastUpdateTimeToolStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
        LastUpdateTimeToolStripStatusLabel.BorderStyle = Border3DStyle.RaisedOuter
        LastUpdateTimeToolStripStatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Text
        LastUpdateTimeToolStripStatusLabel.Name = "LastUpdateTimeToolStripStatusLabel"
        LastUpdateTimeToolStripStatusLabel.Size = New Size(160, 20)
        LastUpdateTimeToolStripStatusLabel.Text = "Last Update Time: Unknown"
        ' 
        ' LoginStatus
        ' 
        LoginStatus.BorderSides = ToolStripStatusLabelBorderSides.Right
        LoginStatus.BorderStyle = Border3DStyle.RaisedOuter
        LoginStatus.DisplayStyle = ToolStripItemDisplayStyle.Text
        LoginStatus.Name = "LoginStatus"
        LoginStatus.Size = New Size(133, 20)
        LoginStatus.Text = "Login Status: Unknown"
        ' 
        ' LowTirComplianceLabel
        ' 
        LowTirComplianceLabel.AutoSize = True
        LowTirComplianceLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        LowTirComplianceLabel.ForeColor = Color.Red
        LowTirComplianceLabel.Location = New Point(3, 479)
        LowTirComplianceLabel.Name = "LowTirComplianceLabel"
        LowTirComplianceLabel.Size = New Size(138, 34)
        LowTirComplianceLabel.TabIndex = 36
        LowTirComplianceLabel.Text = "(6.3) Low" & vbCrLf & "Needs Improvement²"
        LowTirComplianceLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' MaxBasalPerHourLabel
        ' 
        MaxBasalPerHourLabel.AutoSize = True
        MaxBasalPerHourLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        MaxBasalPerHourLabel.ForeColor = Color.LightGray
        MaxBasalPerHourLabel.Location = New Point(978, 108)
        MaxBasalPerHourLabel.Name = "MaxBasalPerHourLabel"
        MaxBasalPerHourLabel.Size = New Size(161, 21)
        MaxBasalPerHourLabel.TabIndex = 67
        MaxBasalPerHourLabel.Text = "Max Basal/Hr ~2.0U"
        MaxBasalPerHourLabel.TextAlign = ContentAlignment.MiddleLeft
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
        ' MenuOptions
        ' 
        MenuOptions.DropDownItems.AddRange(New ToolStripItem() {MenuOptionsAudioAlerts, MenuOptionsSpeechRecognitionEnabled, MenuOptionsShowChartLegends, MenuOptionsSpeechHelpShown, ToolStripSeparator5, MenuOptionsAutoLogin, ToolStripSeparator6, MenuOptionsAdvancedOptions, MenuOptionsFilterRawJSONData, MenuOptionsUseLocalTimeZone, ToolStripSeparator7, MenuOptionsColorPicker, MenuOptionsConfigureTiTR, MenuOptionsEditPumpSettings})
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
        MenuOptionsAudioAlerts.Size = New Size(186, 22)
        MenuOptionsAudioAlerts.Text = "Audio Alerts Enabled"
        ' 
        ' MenuOptionsSpeechRecognitionEnabled
        ' 
        MenuOptionsSpeechRecognitionEnabled.DropDownItems.AddRange(New ToolStripItem() {MenuOptionsSpeechRecognitionDisabled, MenuOptionsSpeechRecognitionConfidence, MenuOptionsSpeechRecognition95, MenuOptionsSpeechRecognition90, MenuOptionsSpeechRecognition85, MenuOptionsSpeechRecognition80})
        MenuOptionsSpeechRecognitionEnabled.Name = "MenuOptionsSpeechRecognitionEnabled"
        MenuOptionsSpeechRecognitionEnabled.Size = New Size(186, 22)
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
        MenuOptionsSpeechRecognition95.Text = " 95%"
        MenuOptionsSpeechRecognition95.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' MenuOptionsSpeechRecognition90
        ' 
        MenuOptionsSpeechRecognition90.CheckOnClick = True
        MenuOptionsSpeechRecognition90.Name = "MenuOptionsSpeechRecognition90"
        MenuOptionsSpeechRecognition90.Size = New Size(135, 22)
        MenuOptionsSpeechRecognition90.Text = " 90%"
        MenuOptionsSpeechRecognition90.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' MenuOptionsSpeechRecognition85
        ' 
        MenuOptionsSpeechRecognition85.CheckOnClick = True
        MenuOptionsSpeechRecognition85.Name = "MenuOptionsSpeechRecognition85"
        MenuOptionsSpeechRecognition85.Size = New Size(135, 22)
        MenuOptionsSpeechRecognition85.Text = " 85%"
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
        MenuOptionsShowChartLegends.Size = New Size(186, 22)
        MenuOptionsShowChartLegends.Text = "Show Chart Legends"
        ' 
        ' MenuOptionsSpeechHelpShown
        ' 
        MenuOptionsSpeechHelpShown.Checked = True
        MenuOptionsSpeechHelpShown.CheckOnClick = True
        MenuOptionsSpeechHelpShown.CheckState = CheckState.Checked
        MenuOptionsSpeechHelpShown.Name = "MenuOptionsSpeechHelpShown"
        MenuOptionsSpeechHelpShown.Size = New Size(186, 22)
        MenuOptionsSpeechHelpShown.Text = "Disable Speech Help"
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(183, 6)
        ' 
        ' MenuOptionsAutoLogin
        ' 
        MenuOptionsAutoLogin.CheckOnClick = True
        MenuOptionsAutoLogin.Name = "MenuOptionsAutoLogin"
        MenuOptionsAutoLogin.Size = New Size(186, 22)
        MenuOptionsAutoLogin.Text = "Auto Login"
        ' 
        ' ToolStripSeparator6
        ' 
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New Size(183, 6)
        ' 
        ' MenuOptionsAdvancedOptions
        ' 
        MenuOptionsAdvancedOptions.Enabled = False
        MenuOptionsAdvancedOptions.Name = "MenuOptionsAdvancedOptions"
        MenuOptionsAdvancedOptions.Size = New Size(186, 22)
        MenuOptionsAdvancedOptions.Text = "Advanced Options"
        ' 
        ' MenuOptionsFilterRawJSONData
        ' 
        MenuOptionsFilterRawJSONData.Checked = True
        MenuOptionsFilterRawJSONData.CheckOnClick = True
        MenuOptionsFilterRawJSONData.CheckState = CheckState.Checked
        MenuOptionsFilterRawJSONData.Name = "MenuOptionsFilterRawJSONData"
        MenuOptionsFilterRawJSONData.Size = New Size(186, 22)
        MenuOptionsFilterRawJSONData.Text = "Filter Raw JSON Data"
        ' 
        ' MenuOptionsUseLocalTimeZone
        ' 
        MenuOptionsUseLocalTimeZone.Checked = True
        MenuOptionsUseLocalTimeZone.CheckOnClick = True
        MenuOptionsUseLocalTimeZone.CheckState = CheckState.Indeterminate
        MenuOptionsUseLocalTimeZone.Name = "MenuOptionsUseLocalTimeZone"
        MenuOptionsUseLocalTimeZone.Size = New Size(186, 22)
        MenuOptionsUseLocalTimeZone.Text = "Use Local TImeZone"
        ' 
        ' ToolStripSeparator7
        ' 
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New Size(183, 6)
        ' 
        ' MenuOptionsColorPicker
        ' 
        MenuOptionsColorPicker.Image = CType(resources.GetObject("MenuOptionsColorPicker.Image"), Image)
        MenuOptionsColorPicker.Name = "MenuOptionsColorPicker"
        MenuOptionsColorPicker.Size = New Size(186, 22)
        MenuOptionsColorPicker.Text = "Color Picker..."
        ' 
        ' MenuOptionsConfigureTiTR
        ' 
        MenuOptionsConfigureTiTR.Name = "MenuOptionsConfigureTiTR"
        MenuOptionsConfigureTiTR.Size = New Size(186, 22)
        MenuOptionsConfigureTiTR.Text = "Configure TiTR (70)..."
        ' 
        ' MenuOptionsEditPumpSettings
        ' 
        MenuOptionsEditPumpSettings.Name = "MenuOptionsEditPumpSettings"
        MenuOptionsEditPumpSettings.Size = New Size(186, 22)
        MenuOptionsEditPumpSettings.Text = "Edit Pump Settings..."
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
        ' MenuStartHere
        ' 
        MenuStartHere.DropDownItems.AddRange(New ToolStripItem() {MenuStartHereUserLogin, ToolStripSeparator8, MenuStartHereManuallyImportDeviceSettings, MenuStartHereShowPumpSetup, ToolStripSeparator1, MenuStartHereLoadSavedDataFile, MenuStartHereUseExceptionReport, ToolStripSeparator4, MenuStartHereUseLastSavedFile, MenuStartHereUseTestData, ToolStripSeparator2, MenuStartHereSaveSnapshotFile, MenuStartHereCleanUpObsoleteFiles, ToolStripSeparator3, MenuStartHereExit})
        MenuStartHere.Name = "MenuStartHere"
        MenuStartHere.Size = New Size(71, 20)
        MenuStartHere.Text = "Start Here"
        ' 
        ' MenuStartHereUserLogin
        ' 
        MenuStartHereUserLogin.Name = "MenuStartHereUserLogin"
        MenuStartHereUserLogin.Size = New Size(245, 22)
        MenuStartHereUserLogin.Text = "Login"
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
        MenuStartHereLoadSavedDataFile.Text = "Load Saved Data File"
        ' 
        ' MenuStartHereUseExceptionReport
        ' 
        MenuStartHereUseExceptionReport.Name = "MenuStartHereUseExceptionReport"
        MenuStartHereUseExceptionReport.Size = New Size(245, 22)
        MenuStartHereUseExceptionReport.Text = "Load An Exception Report"
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
        ' MenuStartHereSaveSnapshotFile
        ' 
        MenuStartHereSaveSnapshotFile.Name = "MenuStartHereSaveSnapshotFile"
        MenuStartHereSaveSnapshotFile.ShortcutKeys = Keys.Control Or Keys.S
        MenuStartHereSaveSnapshotFile.Size = New Size(245, 22)
        MenuStartHereSaveSnapshotFile.Text = "&Save Snapshot File"
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
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {MenuStartHere, MenuOptions, MenuHelp, MenuShowMiniDisplay})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1384, 24)
        MenuStrip1.TabIndex = 0
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ModelLabel
        ' 
        ModelLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        ModelLabel.ForeColor = Color.LightGray
        ModelLabel.Location = New Point(1140, 26)
        ModelLabel.Name = "ModelLabel"
        ModelLabel.Size = New Size(230, 21)
        ModelLabel.TabIndex = 57
        ModelLabel.Text = "Model"
        ' 
        ' NotifyIcon1
        ' 
        NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), Icon)
        NotifyIcon1.Text = "CareLink™ For Windows"
        ' 
        ' PumpAITLabel
        ' 
        PumpAITLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        PumpAITLabel.ForeColor = Color.LightGray
        PumpAITLabel.Location = New Point(978, 28)
        PumpAITLabel.Name = "PumpAITLabel"
        PumpAITLabel.Size = New Size(162, 21)
        PumpAITLabel.TabIndex = 71
        PumpAITLabel.Text = "Pump AIT 3:00"
        PumpAITLabel.TextAlign = ContentAlignment.MiddleLeft
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
        ' PumpBatteryRemaining2Label
        ' 
        PumpBatteryRemaining2Label.BackColor = Color.Transparent
        PumpBatteryRemaining2Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        PumpBatteryRemaining2Label.ForeColor = Color.LightGray
        PumpBatteryRemaining2Label.Location = New Point(119, 106)
        PumpBatteryRemaining2Label.Name = "PumpBatteryRemaining2Label"
        PumpBatteryRemaining2Label.Size = New Size(87, 21)
        PumpBatteryRemaining2Label.TabIndex = 69
        PumpBatteryRemaining2Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' PumpBatteryRemainingLabel
        ' 
        PumpBatteryRemainingLabel.BackColor = Color.Transparent
        PumpBatteryRemainingLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        PumpBatteryRemainingLabel.ForeColor = Color.LightGray
        PumpBatteryRemainingLabel.Location = New Point(119, 89)
        PumpBatteryRemainingLabel.Name = "PumpBatteryRemainingLabel"
        PumpBatteryRemainingLabel.Size = New Size(87, 21)
        PumpBatteryRemainingLabel.TabIndex = 11
        PumpBatteryRemainingLabel.Text = "Unknown"
        PumpBatteryRemainingLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' PumpNameLabel
        ' 
        PumpNameLabel.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        PumpNameLabel.ForeColor = Color.LightGray
        PumpNameLabel.Location = New Point(1140, 52)
        PumpNameLabel.Name = "PumpNameLabel"
        PumpNameLabel.Size = New Size(230, 21)
        PumpNameLabel.TabIndex = 70
        PumpNameLabel.Text = "Pump Name"
        ' 
        ' ReadingsLabel
        ' 
        ReadingsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        ReadingsLabel.ForeColor = Color.LightGray
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
        RemainingInsulinUnits.ForeColor = Color.LightGray
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
        SensorDaysLeftLabel.ForeColor = Color.LightGray
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
        SensorTimeLeftLabel.BorderStyle = BorderStyle.None
        SensorTimeLeftLabel.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        SensorTimeLeftLabel.ForeColor = Color.LightGray
        SensorTimeLeftLabel.Location = New Point(0, 77)
        SensorTimeLeftLabel.Name = "SensorTimeLeftLabel"
        SensorTimeLeftLabel.Size = New Size(94, 60)
        SensorTimeLeftLabel.TabIndex = 46
        SensorTimeLeftLabel.Text = "1 day 2 hrs" & vbCrLf & "(Followed by 24" & vbCrLf & "hr grace period)"
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
        SerialNumberButton.ForeColor = Color.LightGray
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
        ' SgTrendLabel
        ' 
        SgTrendLabel.BackColor = Color.Black
        SgTrendLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        SgTrendLabel.ForeColor = Color.LightGray
        SgTrendLabel.Location = New Point(461, 64)
        SgTrendLabel.Name = "SgTrendLabel"
        SgTrendLabel.Size = New Size(84, 21)
        SgTrendLabel.TabIndex = 61
        SgTrendLabel.Text = "SG Trend"
        SgTrendLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SmartGuardLabel
        ' 
        SmartGuardLabel.Anchor = AnchorStyles.Top
        SmartGuardLabel.AutoSize = True
        SmartGuardLabel.BackColor = Color.Transparent
        SmartGuardLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
        SmartGuardLabel.ForeColor = Color.DodgerBlue
        SmartGuardLabel.Location = New Point(28, 421)
        SmartGuardLabel.Name = "SmartGuardLabel"
        SmartGuardLabel.Size = New Size(176, 25)
        SmartGuardLabel.TabIndex = 35
        SmartGuardLabel.Text = "SmartGuard 100%"
        SmartGuardLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.FixedPanel = FixedPanel.Panel1
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.BackColor = Color.Black
        SplitContainer1.Panel1.Controls.Add(TemporaryUseAdvanceAITDecayCheckBox)
        SplitContainer1.Size = New Size(1376, 658)
        SplitContainer1.SplitterDistance = 30
        SplitContainer1.TabIndex = 0
        ' 
        ' TemporaryUseAdvanceAITDecayCheckBox
        ' 
        TemporaryUseAdvanceAITDecayCheckBox.AutoSize = True
        TemporaryUseAdvanceAITDecayCheckBox.Location = New Point(12, 6)
        TemporaryUseAdvanceAITDecayCheckBox.Name = "TemporaryUseAdvanceAITDecayCheckBox"
        TemporaryUseAdvanceAITDecayCheckBox.Size = New Size(147, 19)
        TemporaryUseAdvanceAITDecayCheckBox.TabIndex = 0
        TemporaryUseAdvanceAITDecayCheckBox.Text = "AIT Decay over 3 hours"
        TemporaryUseAdvanceAITDecayCheckBox.UseVisualStyleBackColor = False
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Dock = DockStyle.Fill
        SplitContainer2.IsSplitterFixed = True
        SplitContainer2.Location = New Point(0, 0)
        SplitContainer2.Name = "SplitContainer2"
        SplitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(PumpAITLabel)
        SplitContainer2.Panel1.Controls.Add(TrendValueLabel)
        SplitContainer2.Panel1.Controls.Add(MaxBasalPerHourLabel)
        SplitContainer2.Panel1.Controls.Add(Last24HrTotalsPanel)
        SplitContainer2.Panel1.Controls.Add(SensorTimeLeftPanel)
        SplitContainer2.Panel1.Controls.Add(TrendArrowsLabel)
        SplitContainer2.Panel1.Controls.Add(SgTrendLabel)
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
        SplitContainer2.Size = New Size(1376, 658)
        SplitContainer2.SplitterDistance = 132
        SplitContainer2.TabIndex = 52
        ' 
        ' TrendValueLabel
        ' 
        TrendValueLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        TrendValueLabel.ForeColor = Color.LightGray
        TrendValueLabel.Location = New Point(461, 89)
        TrendValueLabel.Name = "TrendValueLabel"
        TrendValueLabel.Size = New Size(84, 21)
        TrendValueLabel.TabIndex = 68
        TrendValueLabel.Text = "+ 5"
        TrendValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TrendArrowsLabel
        ' 
        TrendArrowsLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
        TrendArrowsLabel.ForeColor = Color.LightGray
        TrendArrowsLabel.Location = New Point(461, 103)
        TrendArrowsLabel.Name = "TrendArrowsLabel"
        TrendArrowsLabel.Size = New Size(84, 24)
        TrendArrowsLabel.TabIndex = 62
        TrendArrowsLabel.Text = "↑↔↓"
        TrendArrowsLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TransmitterBatteryPercentLabel
        ' 
        TransmitterBatteryPercentLabel.AutoSize = True
        TransmitterBatteryPercentLabel.BackColor = Color.Transparent
        TransmitterBatteryPercentLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        TransmitterBatteryPercentLabel.ForeColor = Color.LightGray
        TransmitterBatteryPercentLabel.Location = New Point(547, 89)
        TransmitterBatteryPercentLabel.Name = "TransmitterBatteryPercentLabel"
        TransmitterBatteryPercentLabel.Size = New Size(92, 21)
        TransmitterBatteryPercentLabel.TabIndex = 13
        TransmitterBatteryPercentLabel.Text = "Connected"
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
        SplitContainer3.Dock = DockStyle.Fill
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
        SplitContainer3.Panel2.Controls.Add(TimeInRangeHeaderLabel)
        SplitContainer3.Panel2.Controls.Add(GraphLast24HoursLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeChartLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeSummaryPercentCharLabel)
        SplitContainer3.Panel2.Controls.Add(AboveHighLimitValueLabel)
        SplitContainer3.Panel2.Controls.Add(AboveHighLimitMessageLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeValueLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInTightRangeValueLabel)
        SplitContainer3.Panel2.Controls.Add(TimeInRangeMessageLabel)
        SplitContainer3.Panel2.Controls.Add(BelowLowLimitValueLabel)
        SplitContainer3.Panel2.Controls.Add(BelowLowLimitMessageLabel)
        SplitContainer3.Panel2.Controls.Add(AverageSGValueLabel)
        SplitContainer3.Panel2.Controls.Add(AverageSGMessageLabel)
        SplitContainer3.Panel2.Controls.Add(TirComplianceLabel)
        SplitContainer3.Panel2.Controls.Add(HighTirComplianceLabel)
        SplitContainer3.Panel2.Controls.Add(LowTirComplianceLabel)
        SplitContainer3.Panel2.Controls.Add(SmartGuardLabel)
        SplitContainer3.Panel2.Controls.Add(TiTRMgsLabel)
        SplitContainer3.Panel2.Controls.Add(TiTRMgsLabel2)
        SplitContainer3.Size = New Size(1376, 522)
        SplitContainer3.SplitterDistance = 1140
        SplitContainer3.TabIndex = 0
        ' 
        ' TimeInRangeHeaderLabel
        ' 
        TimeInRangeHeaderLabel.Anchor = AnchorStyles.Top
        TimeInRangeHeaderLabel.AutoSize = True
        TimeInRangeHeaderLabel.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)
        TimeInRangeHeaderLabel.ForeColor = Color.Gray
        TimeInRangeHeaderLabel.Location = New Point(47, 0)
        TimeInRangeHeaderLabel.Name = "TimeInRangeHeaderLabel"
        TimeInRangeHeaderLabel.Size = New Size(138, 25)
        TimeInRangeHeaderLabel.TabIndex = 46
        TimeInRangeHeaderLabel.Text = "Time in Range"
        TimeInRangeHeaderLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInRangeChartLabel
        ' 
        TimeInRangeChartLabel.Anchor = AnchorStyles.Top
        TimeInRangeChartLabel.AutoSize = True
        TimeInRangeChartLabel.BackColor = Color.Black
        TimeInRangeChartLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
        TimeInRangeChartLabel.ForeColor = Color.LightGray
        TimeInRangeChartLabel.Location = New Point(82, 74)
        TimeInRangeChartLabel.Name = "TimeInRangeChartLabel"
        TimeInRangeChartLabel.Size = New Size(68, 40)
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
        TimeInRangeSummaryPercentCharLabel.ForeColor = Color.LightGray
        TimeInRangeSummaryPercentCharLabel.Location = New Point(95, 113)
        TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        TimeInRangeSummaryPercentCharLabel.Size = New Size(42, 40)
        TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        TimeInRangeSummaryPercentCharLabel.Text = "%"
        TimeInRangeSummaryPercentCharLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInRangeValueLabel
        ' 
        TimeInRangeValueLabel.Anchor = AnchorStyles.Top
        TimeInRangeValueLabel.AutoSize = True
        TimeInRangeValueLabel.BackColor = Color.Black
        TimeInRangeValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
        TimeInRangeValueLabel.ForeColor = Color.DarkGreen
        TimeInRangeValueLabel.Location = New Point(16, 234)
        TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        TimeInRangeValueLabel.Size = New Size(84, 40)
        TimeInRangeValueLabel.TabIndex = 24
        TimeInRangeValueLabel.Text = " 90%"
        TimeInRangeValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInTightRangeValueLabel
        ' 
        TimeInTightRangeValueLabel.Anchor = AnchorStyles.Top
        TimeInTightRangeValueLabel.AutoSize = True
        TimeInTightRangeValueLabel.BackColor = Color.Black
        TimeInTightRangeValueLabel.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold)
        TimeInTightRangeValueLabel.ForeColor = Color.Red
        TimeInTightRangeValueLabel.Location = New Point(135, 234)
        TimeInTightRangeValueLabel.Name = "TimeInTightRangeValueLabel"
        TimeInTightRangeValueLabel.Size = New Size(84, 40)
        TimeInTightRangeValueLabel.TabIndex = 24
        TimeInTightRangeValueLabel.Text = " 50%"
        TimeInTightRangeValueLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInRangeMessageLabel
        ' 
        TimeInRangeMessageLabel.AutoSize = True
        TimeInRangeMessageLabel.BackColor = Color.Transparent
        TimeInRangeMessageLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        TimeInRangeMessageLabel.ForeColor = Color.DarkGreen
        TimeInRangeMessageLabel.Location = New Point(21, 270)
        TimeInRangeMessageLabel.Name = "TimeInRangeMessageLabel"
        TimeInRangeMessageLabel.Size = New Size(77, 21)
        TimeInRangeMessageLabel.TabIndex = 30
        TimeInRangeMessageLabel.Text = "In Range"
        TimeInRangeMessageLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TirComplianceLabel
        ' 
        TirComplianceLabel.Anchor = AnchorStyles.Top
        TirComplianceLabel.AutoSize = True
        TirComplianceLabel.BackColor = Color.Transparent
        TirComplianceLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
        TirComplianceLabel.ForeColor = Color.LightGray
        TirComplianceLabel.Location = New Point(38, 448)
        TirComplianceLabel.Name = "TirComplianceLabel"
        TirComplianceLabel.Size = New Size(157, 25)
        TirComplianceLabel.TabIndex = 35
        TirComplianceLabel.Text = "TIR Compliance¹"
        TirComplianceLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TiTRMgsLabel
        ' 
        TiTRMgsLabel.AutoSize = True
        TiTRMgsLabel.BackColor = Color.Transparent
        TiTRMgsLabel.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        TiTRMgsLabel.ForeColor = Color.Red
        TiTRMgsLabel.Location = New Point(125, 270)
        TiTRMgsLabel.Name = "TiTRMgsLabel"
        TiTRMgsLabel.Size = New Size(102, 21)
        TiTRMgsLabel.TabIndex = 30
        TiTRMgsLabel.Text = "Tight Range"
        TiTRMgsLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TiTRMgsLabel2
        ' 
        TiTRMgsLabel2.AutoSize = True
        TiTRMgsLabel2.BackColor = Color.Transparent
        TiTRMgsLabel2.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        TiTRMgsLabel2.ForeColor = Color.Red
        TiTRMgsLabel2.Location = New Point(157, 291)
        TiTRMgsLabel2.Name = "TiTRMgsLabel2"
        TiTRMgsLabel2.Size = New Size(67, 21)
        TiTRMgsLabel2.TabIndex = 30
        TiTRMgsLabel2.Text = "70/50%"
        TiTRMgsLabel2.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' SplitContainerNotificationsCleared
        ' 
        SplitContainerNotificationsCleared.Dock = DockStyle.Fill
        SplitContainerNotificationsCleared.Location = New Point(0, 0)
        SplitContainerNotificationsCleared.Name = "SplitContainerNotificationsCleared"
        SplitContainerNotificationsCleared.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainerNotificationsCleared.Panel1
        ' 
        SplitContainerNotificationsCleared.Panel1.Controls.Add(TlpNotificationsClearedTop)
        ' 
        ' SplitContainerNotificationsCleared.Panel2
        ' 
        SplitContainerNotificationsCleared.Panel2.Controls.Add(TlpNotificationsCleared)
        SplitContainerNotificationsCleared.Size = New Size(1376, 658)
        SplitContainerNotificationsCleared.SplitterDistance = 39
        SplitContainerNotificationsCleared.TabIndex = 0
        ' 
        ' TlpNotificationsClearedTop
        ' 
        TlpNotificationsClearedTop.AutoSize = True
        TlpNotificationsClearedTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpNotificationsClearedTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpNotificationsClearedTop.ColumnCount = 2
        TlpNotificationsClearedTop.ColumnStyles.Add(New ColumnStyle())
        TlpNotificationsClearedTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpNotificationsClearedTop.Dock = DockStyle.Fill
        TlpNotificationsClearedTop.LabelText = "Notifications Cleared"
        TlpNotificationsClearedTop.Location = New Point(0, 0)
        TlpNotificationsClearedTop.Name = "TlpNotificationsClearedTop"
        TlpNotificationsClearedTop.RowCount = 1
        TlpNotificationsClearedTop.RowStyles.Add(New RowStyle())
        TlpNotificationsClearedTop.Size = New Size(1376, 39)
        TlpNotificationsClearedTop.TabIndex = 1
        ' 
        ' TlpNotificationsCleared
        ' 
        TlpNotificationsCleared.AutoScroll = True
        TlpNotificationsCleared.AutoSize = True
        TlpNotificationsCleared.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpNotificationsCleared.ColumnCount = 1
        TlpNotificationsCleared.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpNotificationsCleared.Dock = DockStyle.Fill
        TlpNotificationsCleared.Location = New Point(0, 0)
        TlpNotificationsCleared.Name = "TlpNotificationsCleared"
        TlpNotificationsCleared.RowCount = 1
        TlpNotificationsCleared.RowStyles.Add(New RowStyle())
        TlpNotificationsCleared.Size = New Size(1376, 615)
        TlpNotificationsCleared.TabIndex = 0
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
        ' StatusStripSpeech
        ' 
        StatusStripSpeech.DisplayStyle = ToolStripItemDisplayStyle.Text
        StatusStripSpeech.Name = "StatusStripSpeech"
        StatusStripSpeech.Size = New Size(401, 20)
        StatusStripSpeech.Spring = True
        StatusStripSpeech.Text = " "
        ' 
        ' TimeZoneToolStripStatusLabel
        ' 
        TimeZoneToolStripStatusLabel.BorderSides = ToolStripStatusLabelBorderSides.Right
        TimeZoneToolStripStatusLabel.BorderStyle = Border3DStyle.RaisedOuter
        TimeZoneToolStripStatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Text
        TimeZoneToolStripStatusLabel.Name = "TimeZoneToolStripStatusLabel"
        TimeZoneToolStripStatusLabel.Size = New Size(119, 20)
        TimeZoneToolStripStatusLabel.Text = "TimeZone Unknown"
        ' 
        ' StatusStripSpacerRight
        ' 
        StatusStripSpacerRight.Name = "StatusStripSpacerRight"
        StatusStripSpacerRight.Size = New Size(401, 20)
        StatusStripSpacerRight.Spring = True
        StatusStripSpacerRight.Text = " "
        ' 
        ' UpdateAvailableStatusStripLabel
        ' 
        UpdateAvailableStatusStripLabel.BorderSides = ToolStripStatusLabelBorderSides.Left
        UpdateAvailableStatusStripLabel.BorderStyle = Border3DStyle.RaisedOuter
        UpdateAvailableStatusStripLabel.Image = My.Resources.Resources.NotificationAlertRed_16x
        UpdateAvailableStatusStripLabel.Name = "UpdateAvailableStatusStripLabel"
        UpdateAvailableStatusStripLabel.Size = New Size(154, 20)
        UpdateAvailableStatusStripLabel.Text = "Update Status Unknown"
        ' 
        ' TabPage01HomePage
        ' 
        TabPage01HomePage.BackColor = Color.Black
        TabPage01HomePage.Controls.Add(SplitContainer2)
        TabPage01HomePage.Location = New Point(4, 30)
        TabPage01HomePage.Name = "TabPage01HomePage"
        TabPage01HomePage.Size = New Size(1376, 658)
        TabPage01HomePage.TabIndex = 1
        TabPage01HomePage.Text = "Summary"
        TabPage01HomePage.UseVisualStyleBackColor = True
        ' 
        ' TabPage02RunningIOB
        ' 
        TabPage02RunningIOB.Controls.Add(SplitContainer1)
        TabPage02RunningIOB.Location = New Point(4, 30)
        TabPage02RunningIOB.Name = "TabPage02RunningIOB"
        TabPage02RunningIOB.Size = New Size(1376, 658)
        TabPage02RunningIOB.TabIndex = 2
        TabPage02RunningIOB.Text = "Running IOB"
        TabPage02RunningIOB.UseVisualStyleBackColor = True
        ' 
        ' TabPage03TreatmentDetails
        ' 
        TabPage03TreatmentDetails.Location = New Point(4, 30)
        TabPage03TreatmentDetails.Name = "TabPage03TreatmentDetails"
        TabPage03TreatmentDetails.Size = New Size(1376, 658)
        TabPage03TreatmentDetails.TabIndex = 3
        TabPage03TreatmentDetails.Text = "Treatment Details"
        TabPage03TreatmentDetails.UseVisualStyleBackColor = True
        ' 
        ' TabPage04SummaryData
        ' 
        TabPage04SummaryData.Controls.Add(DgvSummary)
        TabPage04SummaryData.Location = New Point(4, 30)
        TabPage04SummaryData.Name = "TabPage04SummaryData"
        TabPage04SummaryData.Size = New Size(1376, 658)
        TabPage04SummaryData.TabIndex = 4
        TabPage04SummaryData.Text = "Summary Data"
        TabPage04SummaryData.UseVisualStyleBackColor = True
        ' 
        ' TabPage05ActiveInsulin
        ' 
        TabPage05ActiveInsulin.Controls.Add(TlpActiveInsulin)
        TabPage05ActiveInsulin.Location = New Point(4, 30)
        TabPage05ActiveInsulin.Name = "TabPage05ActiveInsulin"
        TabPage05ActiveInsulin.Size = New Size(1376, 658)
        TabPage05ActiveInsulin.TabIndex = 18
        TabPage05ActiveInsulin.Text = "Active Insulin"
        TabPage05ActiveInsulin.UseVisualStyleBackColor = True
        ' 
        ' TlpActiveInsulin
        ' 
        TlpActiveInsulin.AutoSize = True
        TlpActiveInsulin.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpActiveInsulin.ColumnCount = 1
        TlpActiveInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpActiveInsulin.Controls.Add(TlpActiveInsulinTop, 0, 0)
        TlpActiveInsulin.Controls.Add(DgvActiveInsulin, 0, 1)
        TlpActiveInsulin.Dock = DockStyle.Fill
        TlpActiveInsulin.Location = New Point(0, 0)
        TlpActiveInsulin.Name = "TlpActiveInsulin"
        TlpActiveInsulin.RowCount = 2
        TlpActiveInsulin.RowStyles.Add(New RowStyle())
        TlpActiveInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpActiveInsulin.Size = New Size(1376, 658)
        TlpActiveInsulin.TabIndex = 0
        ' 
        ' TlpActiveInsulinTop
        ' 
        TlpActiveInsulinTop.AutoSize = True
        TlpActiveInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpActiveInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpActiveInsulinTop.ColumnCount = 2
        TlpActiveInsulinTop.ColumnStyles.Add(New ColumnStyle())
        TlpActiveInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpActiveInsulinTop.Dock = DockStyle.Fill
        TlpActiveInsulinTop.LabelText = "Active Insulin"
        TlpActiveInsulinTop.Location = New Point(3, 3)
        TlpActiveInsulinTop.Name = "TlpActiveInsulinTop"
        TlpActiveInsulinTop.RowCount = 1
        TlpActiveInsulinTop.RowStyles.Add(New RowStyle())
        TlpActiveInsulinTop.Size = New Size(1370, 37)
        TlpActiveInsulinTop.TabIndex = 1
        ' 
        ' TabPage06Basal
        ' 
        TabPage06Basal.Controls.Add(TlpBasal)
        TabPage06Basal.Location = New Point(4, 30)
        TabPage06Basal.Name = "TabPage06Basal"
        TabPage06Basal.Size = New Size(1376, 658)
        TabPage06Basal.TabIndex = 23
        TabPage06Basal.Text = "Basal"
        TabPage06Basal.UseVisualStyleBackColor = True
        ' 
        ' TlpBasal
        ' 
        TlpBasal.AutoScroll = True
        TlpBasal.AutoSize = True
        TlpBasal.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpBasal.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpBasal.ColumnCount = 1
        TlpBasal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpBasal.Controls.Add(TlpBasalTop, 0, 0)
        TlpBasal.Controls.Add(DgvBasal, 0, 1)
        TlpBasal.Dock = DockStyle.Fill
        TlpBasal.Location = New Point(0, 0)
        TlpBasal.Margin = New Padding(0)
        TlpBasal.Name = "TlpBasal"
        TlpBasal.RowCount = 2
        TlpBasal.RowStyles.Add(New RowStyle())
        TlpBasal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpBasal.Size = New Size(1376, 658)
        TlpBasal.TabIndex = 0
        ' 
        ' TlpBasalTop
        ' 
        TlpBasalTop.AutoSize = True
        TlpBasalTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpBasalTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpBasalTop.ColumnCount = 2
        TlpBasalTop.ColumnStyles.Add(New ColumnStyle())
        TlpBasalTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpBasalTop.Dock = DockStyle.Fill
        TlpBasalTop.LabelText = "Basal"
        TlpBasalTop.Location = New Point(6, 6)
        TlpBasalTop.Name = "TlpBasalTop"
        TlpBasalTop.RowCount = 1
        TlpBasalTop.RowStyles.Add(New RowStyle())
        TlpBasalTop.Size = New Size(1364, 37)
        TlpBasalTop.TabIndex = 1
        ' 
        ' TabPage07LastAlarm
        ' 
        TabPage07LastAlarm.Controls.Add(TlpLastAlarm)
        TabPage07LastAlarm.Location = New Point(4, 30)
        TabPage07LastAlarm.Name = "TabPage07LastAlarm"
        TabPage07LastAlarm.Size = New Size(1376, 658)
        TabPage07LastAlarm.TabIndex = 17
        TabPage07LastAlarm.Text = "Last Alarm"
        TabPage07LastAlarm.UseVisualStyleBackColor = True
        ' 
        ' TlpLastAlarm
        ' 
        TlpLastAlarm.AutoSize = True
        TlpLastAlarm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLastAlarm.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpLastAlarm.ColumnCount = 1
        TlpLastAlarm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLastAlarm.Controls.Add(TlpLastAlarmTop, 0, 0)
        TlpLastAlarm.Controls.Add(DgvLastAlarm, 0, 1)
        TlpLastAlarm.Dock = DockStyle.Fill
        TlpLastAlarm.Location = New Point(0, 0)
        TlpLastAlarm.Margin = New Padding(0)
        TlpLastAlarm.Name = "TlpLastAlarm"
        TlpLastAlarm.RowCount = 2
        TlpLastAlarm.RowStyles.Add(New RowStyle())
        TlpLastAlarm.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpLastAlarm.Size = New Size(1376, 658)
        TlpLastAlarm.TabIndex = 0
        ' 
        ' TlpLastAlarmTop
        ' 
        TlpLastAlarmTop.AutoSize = True
        TlpLastAlarmTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLastAlarmTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpLastAlarmTop.ColumnCount = 2
        TlpLastAlarmTop.ColumnStyles.Add(New ColumnStyle())
        TlpLastAlarmTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLastAlarmTop.Dock = DockStyle.Fill
        TlpLastAlarmTop.LabelText = "Last Alarm"
        TlpLastAlarmTop.Location = New Point(6, 6)
        TlpLastAlarmTop.Name = "TlpLastAlarmTop"
        TlpLastAlarmTop.RowCount = 1
        TlpLastAlarmTop.RowStyles.Add(New RowStyle())
        TlpLastAlarmTop.Size = New Size(1364, 37)
        TlpLastAlarmTop.TabIndex = 1
        ' 
        ' TabPage08LastSG
        ' 
        TabPage08LastSG.Controls.Add(TlpLastSG)
        TabPage08LastSG.Location = New Point(4, 30)
        TabPage08LastSG.Name = "TabPage08LastSG"
        TabPage08LastSG.Size = New Size(1376, 658)
        TabPage08LastSG.TabIndex = 16
        TabPage08LastSG.Text = "Last SG"
        TabPage08LastSG.UseVisualStyleBackColor = True
        ' 
        ' TlpLastSG
        ' 
        TlpLastSG.AutoSize = True
        TlpLastSG.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLastSG.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpLastSG.ColumnCount = 1
        TlpLastSG.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLastSG.Controls.Add(TlpLastSgTop, 0, 0)
        TlpLastSG.Controls.Add(DgvLastSensorGlucose, 0, 1)
        TlpLastSG.Dock = DockStyle.Fill
        TlpLastSG.Location = New Point(0, 0)
        TlpLastSG.Name = "TlpLastSG"
        TlpLastSG.RowCount = 2
        TlpLastSG.RowStyles.Add(New RowStyle())
        TlpLastSG.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpLastSG.Size = New Size(1376, 658)
        TlpLastSG.TabIndex = 1
        ' 
        ' TlpLastSgTop
        ' 
        TlpLastSgTop.AutoSize = True
        TlpLastSgTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLastSgTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpLastSgTop.ColumnCount = 2
        TlpLastSgTop.ColumnStyles.Add(New ColumnStyle())
        TlpLastSgTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLastSgTop.Dock = DockStyle.Fill
        TlpLastSgTop.LabelText = "Last Sensor Glucose"
        TlpLastSgTop.Location = New Point(6, 6)
        TlpLastSgTop.Name = "TlpLastSgTop"
        TlpLastSgTop.RowCount = 1
        TlpLastSgTop.RowStyles.Add(New RowStyle())
        TlpLastSgTop.Size = New Size(1364, 37)
        TlpLastSgTop.TabIndex = 1
        ' 
        ' TabPage09Limits
        ' 
        TabPage09Limits.Controls.Add(TlpLimits)
        TabPage09Limits.Location = New Point(4, 30)
        TabPage09Limits.Name = "TabPage09Limits"
        TabPage09Limits.Size = New Size(1376, 658)
        TabPage09Limits.TabIndex = 9
        TabPage09Limits.Text = "Limits"
        TabPage09Limits.UseVisualStyleBackColor = True
        ' 
        ' TlpLimits
        ' 
        TlpLimits.AutoSize = True
        TlpLimits.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLimits.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpLimits.ColumnCount = 1
        TlpLimits.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLimits.Controls.Add(TlpLimitsTop, 0, 0)
        TlpLimits.Controls.Add(DgvLimits, 0, 1)
        TlpLimits.Dock = DockStyle.Fill
        TlpLimits.Location = New Point(0, 0)
        TlpLimits.Name = "TlpLimits"
        TlpLimits.RowCount = 2
        TlpLimits.RowStyles.Add(New RowStyle())
        TlpLimits.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpLimits.Size = New Size(1376, 658)
        TlpLimits.TabIndex = 0
        ' 
        ' TlpLimitsTop
        ' 
        TlpLimitsTop.AutoSize = True
        TlpLimitsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLimitsTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpLimitsTop.ColumnCount = 2
        TlpLimitsTop.ColumnStyles.Add(New ColumnStyle())
        TlpLimitsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLimitsTop.Dock = DockStyle.Fill
        TlpLimitsTop.LabelText = "Limits"
        TlpLimitsTop.Location = New Point(6, 6)
        TlpLimitsTop.Name = "TlpLimitsTop"
        TlpLimitsTop.RowCount = 1
        TlpLimitsTop.RowStyles.Add(New RowStyle())
        TlpLimitsTop.Size = New Size(1364, 37)
        TlpLimitsTop.TabIndex = 1
        ' 
        ' TabPage10NotificationActive
        ' 
        TabPage10NotificationActive.Controls.Add(TlpNotificationActive)
        TabPage10NotificationActive.Location = New Point(4, 30)
        TabPage10NotificationActive.Name = "TabPage10NotificationActive"
        TabPage10NotificationActive.Size = New Size(1376, 658)
        TabPage10NotificationActive.TabIndex = 13
        TabPage10NotificationActive.Text = "Notification - Active"
        TabPage10NotificationActive.UseVisualStyleBackColor = True
        ' 
        ' TlpNotificationActive
        ' 
        TlpNotificationActive.AutoScroll = True
        TlpNotificationActive.AutoSize = True
        TlpNotificationActive.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpNotificationActive.ColumnCount = 1
        TlpNotificationActive.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpNotificationActive.Controls.Add(TlpNotificationActiveTop, 0, 0)
        TlpNotificationActive.Dock = DockStyle.Fill
        TlpNotificationActive.Location = New Point(0, 0)
        TlpNotificationActive.Name = "TlpNotificationActive"
        TlpNotificationActive.RowCount = 1
        TlpNotificationActive.RowStyles.Add(New RowStyle())
        TlpNotificationActive.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpNotificationActive.Size = New Size(1376, 658)
        TlpNotificationActive.TabIndex = 0
        ' 
        ' TlpNotificationActiveTop
        ' 
        TlpNotificationActiveTop.AutoSize = True
        TlpNotificationActiveTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpNotificationActiveTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpNotificationActiveTop.ColumnCount = 2
        TlpNotificationActiveTop.ColumnStyles.Add(New ColumnStyle())
        TlpNotificationActiveTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpNotificationActiveTop.Dock = DockStyle.Fill
        TlpNotificationActiveTop.LabelText = "Active Notification"
        TlpNotificationActiveTop.Location = New Point(6, 6)
        TlpNotificationActiveTop.Name = "TlpNotificationActiveTop"
        TlpNotificationActiveTop.RowCount = 1
        TlpNotificationActiveTop.RowStyles.Add(New RowStyle())
        TlpNotificationActiveTop.Size = New Size(1364, 37)
        TlpNotificationActiveTop.TabIndex = 1
        ' 
        ' TabPage11NotificationsCleared
        ' 
        TabPage11NotificationsCleared.Controls.Add(SplitContainerNotificationsCleared)
        TabPage11NotificationsCleared.Location = New Point(4, 30)
        TabPage11NotificationsCleared.Name = "TabPage11NotificationsCleared"
        TabPage11NotificationsCleared.Size = New Size(1376, 658)
        TabPage11NotificationsCleared.TabIndex = 5
        TabPage11NotificationsCleared.Text = "Notifications - Cleared"
        TabPage11NotificationsCleared.UseVisualStyleBackColor = True
        ' 
        ' TabPage12PumpBannerState
        ' 
        TabPage12PumpBannerState.Controls.Add(TlpPumpBannerState)
        TabPage12PumpBannerState.Location = New Point(4, 30)
        TabPage12PumpBannerState.Name = "TabPage12PumpBannerState"
        TabPage12PumpBannerState.Size = New Size(1376, 658)
        TabPage12PumpBannerState.TabIndex = 22
        TabPage12PumpBannerState.Text = "Pump Banner State"
        TabPage12PumpBannerState.UseVisualStyleBackColor = True
        ' 
        ' TlpPumpBannerState
        ' 
        TlpPumpBannerState.AutoSize = True
        TlpPumpBannerState.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpPumpBannerState.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpPumpBannerState.ColumnCount = 1
        TlpPumpBannerState.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpPumpBannerState.Controls.Add(TlpPumpBannerStateTop, 0, 0)
        TlpPumpBannerState.Controls.Add(DgvPumpBannerState, 0, 1)
        TlpPumpBannerState.Dock = DockStyle.Fill
        TlpPumpBannerState.Location = New Point(0, 0)
        TlpPumpBannerState.Margin = New Padding(0)
        TlpPumpBannerState.Name = "TlpPumpBannerState"
        TlpPumpBannerState.RowCount = 2
        TlpPumpBannerState.RowStyles.Add(New RowStyle())
        TlpPumpBannerState.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpPumpBannerState.Size = New Size(1376, 658)
        TlpPumpBannerState.TabIndex = 0
        ' 
        ' TlpPumpBannerStateTop
        ' 
        TlpPumpBannerStateTop.AutoSize = True
        TlpPumpBannerStateTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpPumpBannerStateTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpPumpBannerStateTop.ColumnCount = 2
        TlpPumpBannerStateTop.ColumnStyles.Add(New ColumnStyle())
        TlpPumpBannerStateTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpPumpBannerStateTop.Dock = DockStyle.Fill
        TlpPumpBannerStateTop.LabelText = "Pump Banner State"
        TlpPumpBannerStateTop.Location = New Point(6, 6)
        TlpPumpBannerStateTop.Name = "TlpPumpBannerStateTop"
        TlpPumpBannerStateTop.RowCount = 1
        TlpPumpBannerStateTop.RowStyles.Add(New RowStyle())
        TlpPumpBannerStateTop.Size = New Size(1364, 37)
        TlpPumpBannerStateTop.TabIndex = 1
        ' 
        ' TabPage13SensorGlucose
        ' 
        TabPage13SensorGlucose.Controls.Add(TlpSgs)
        TabPage13SensorGlucose.Location = New Point(4, 30)
        TabPage13SensorGlucose.Name = "TabPage13SensorGlucose"
        TabPage13SensorGlucose.Size = New Size(1376, 658)
        TabPage13SensorGlucose.TabIndex = 19
        TabPage13SensorGlucose.Text = "Sensor Glucose"
        TabPage13SensorGlucose.UseVisualStyleBackColor = True
        ' 
        ' TlpSgs
        ' 
        TlpSgs.AutoSize = True
        TlpSgs.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpSgs.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpSgs.ColumnCount = 1
        TlpSgs.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpSgs.Controls.Add(TlpSgsTop, 0, 0)
        TlpSgs.Controls.Add(DgvSGs, 0, 1)
        TlpSgs.Dock = DockStyle.Fill
        TlpSgs.Location = New Point(0, 0)
        TlpSgs.Name = "TlpSgs"
        TlpSgs.RowCount = 2
        TlpSgs.RowStyles.Add(New RowStyle())
        TlpSgs.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpSgs.Size = New Size(1376, 658)
        TlpSgs.TabIndex = 1
        ' 
        ' TlpSgsTop
        ' 
        TlpSgsTop.AutoSize = True
        TlpSgsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpSgsTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpSgsTop.ColumnCount = 2
        TlpSgsTop.ColumnStyles.Add(New ColumnStyle())
        TlpSgsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpSgsTop.Dock = DockStyle.Fill
        TlpSgsTop.LabelText = "Sensor Glucose Values"
        TlpSgsTop.Location = New Point(6, 6)
        TlpSgsTop.Name = "TlpSgsTop"
        TlpSgsTop.RowCount = 1
        TlpSgsTop.RowStyles.Add(New RowStyle())
        TlpSgsTop.Size = New Size(1364, 37)
        TlpSgsTop.TabIndex = 1
        ' 
        ' TabPage14TherapyAlgorithmState
        ' 
        TabPage14TherapyAlgorithmState.Controls.Add(TlpTherapyAlgorithmState)
        TabPage14TherapyAlgorithmState.Location = New Point(4, 30)
        TabPage14TherapyAlgorithmState.Name = "TabPage14TherapyAlgorithmState"
        TabPage14TherapyAlgorithmState.Size = New Size(1376, 658)
        TabPage14TherapyAlgorithmState.TabIndex = 10
        TabPage14TherapyAlgorithmState.Text = "Therapy Algorithm State"
        TabPage14TherapyAlgorithmState.UseVisualStyleBackColor = True
        ' 
        ' TlpTherapyAlgorithmState
        ' 
        TlpTherapyAlgorithmState.AutoSize = True
        TlpTherapyAlgorithmState.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpTherapyAlgorithmState.ColumnCount = 1
        TlpTherapyAlgorithmState.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpTherapyAlgorithmState.Controls.Add(TlpTherapyAlgorithmStateTop, 0, 0)
        TlpTherapyAlgorithmState.Controls.Add(DgvTherapyAlgorithmState, 0, 1)
        TlpTherapyAlgorithmState.Dock = DockStyle.Fill
        TlpTherapyAlgorithmState.Location = New Point(0, 0)
        TlpTherapyAlgorithmState.Name = "TlpTherapyAlgorithmState"
        TlpTherapyAlgorithmState.RowCount = 2
        TlpTherapyAlgorithmState.RowStyles.Add(New RowStyle())
        TlpTherapyAlgorithmState.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpTherapyAlgorithmState.Size = New Size(1376, 658)
        TlpTherapyAlgorithmState.TabIndex = 0
        ' 
        ' TlpTherapyAlgorithmStateTop
        ' 
        TlpTherapyAlgorithmStateTop.AutoSize = True
        TlpTherapyAlgorithmStateTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpTherapyAlgorithmStateTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpTherapyAlgorithmStateTop.ColumnCount = 2
        TlpTherapyAlgorithmStateTop.ColumnStyles.Add(New ColumnStyle())
        TlpTherapyAlgorithmStateTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpTherapyAlgorithmStateTop.Dock = DockStyle.Fill
        TlpTherapyAlgorithmStateTop.LabelText = "Therapy Algorithm State"
        TlpTherapyAlgorithmStateTop.Location = New Point(6, 6)
        TlpTherapyAlgorithmStateTop.Name = "TlpTherapyAlgorithmStateTop"
        TlpTherapyAlgorithmStateTop.RowCount = 1
        TlpTherapyAlgorithmStateTop.RowStyles.Add(New RowStyle())
        TlpTherapyAlgorithmStateTop.Size = New Size(1364, 37)
        TlpTherapyAlgorithmStateTop.TabIndex = 1
        ' 
        ' TabPage15More
        ' 
        TabPage15More.Location = New Point(4, 30)
        TabPage15More.Name = "TabPage15More"
        TabPage15More.Size = New Size(1376, 658)
        TabPage15More.TabIndex = 24
        TabPage15More.Text = "More..."
        TabPage15More.UseVisualStyleBackColor = True
        ' 
        ' TabPage01AutoBasalDelivery
        ' 
        TabPage01AutoBasalDelivery.Controls.Add(TlpAutoBasalDelivery)
        TabPage01AutoBasalDelivery.Location = New Point(4, 30)
        TabPage01AutoBasalDelivery.Name = "TabPage01AutoBasalDelivery"
        TabPage01AutoBasalDelivery.Size = New Size(1376, 658)
        TabPage01AutoBasalDelivery.TabIndex = 1
        TabPage01AutoBasalDelivery.Text = "Auto Basal Delivery"
        TabPage01AutoBasalDelivery.UseVisualStyleBackColor = True
        ' 
        ' TlpAutoBasalDelivery
        ' 
        TlpAutoBasalDelivery.AutoSize = True
        TlpAutoBasalDelivery.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpAutoBasalDelivery.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpAutoBasalDelivery.ColumnCount = 1
        TlpAutoBasalDelivery.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpAutoBasalDelivery.Controls.Add(TlpAutoBasalDeliveryTop, 0, 0)
        TlpAutoBasalDelivery.Controls.Add(DgvAutoBasalDelivery, 0, 1)
        TlpAutoBasalDelivery.Dock = DockStyle.Fill
        TlpAutoBasalDelivery.Location = New Point(0, 0)
        TlpAutoBasalDelivery.Name = "TlpAutoBasalDelivery"
        TlpAutoBasalDelivery.RowCount = 2
        TlpAutoBasalDelivery.RowStyles.Add(New RowStyle())
        TlpAutoBasalDelivery.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpAutoBasalDelivery.Size = New Size(1376, 658)
        TlpAutoBasalDelivery.TabIndex = 0
        ' 
        ' TlpAutoBasalDeliveryTop
        ' 
        TlpAutoBasalDeliveryTop.AutoSize = True
        TlpAutoBasalDeliveryTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpAutoBasalDeliveryTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpAutoBasalDeliveryTop.ColumnCount = 2
        TlpAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle())
        TlpAutoBasalDeliveryTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpAutoBasalDeliveryTop.Dock = DockStyle.Fill
        TlpAutoBasalDeliveryTop.LabelText = "Marker : Auto Basal Delivery"
        TlpAutoBasalDeliveryTop.Location = New Point(6, 6)
        TlpAutoBasalDeliveryTop.Name = "TlpAutoBasalDeliveryTop"
        TlpAutoBasalDeliveryTop.RowCount = 1
        TlpAutoBasalDeliveryTop.RowStyles.Add(New RowStyle())
        TlpAutoBasalDeliveryTop.Size = New Size(1364, 37)
        TlpAutoBasalDeliveryTop.TabIndex = 1
        ' 
        ' TabPage02AutoModeStatus
        ' 
        TabPage02AutoModeStatus.Controls.Add(TlpAutoModeStatus)
        TabPage02AutoModeStatus.Location = New Point(4, 30)
        TabPage02AutoModeStatus.Name = "TabPage02AutoModeStatus"
        TabPage02AutoModeStatus.Size = New Size(1376, 658)
        TabPage02AutoModeStatus.TabIndex = 2
        TabPage02AutoModeStatus.Text = "Auto Mode Status"
        TabPage02AutoModeStatus.UseVisualStyleBackColor = True
        ' 
        ' TlpAutoModeStatus
        ' 
        TlpAutoModeStatus.AutoSize = True
        TlpAutoModeStatus.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpAutoModeStatus.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpAutoModeStatus.ColumnCount = 1
        TlpAutoModeStatus.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpAutoModeStatus.Controls.Add(TlpAutoModeStatusTop, 0, 0)
        TlpAutoModeStatus.Controls.Add(DgvAutoModeStatus, 0, 1)
        TlpAutoModeStatus.Dock = DockStyle.Fill
        TlpAutoModeStatus.Location = New Point(0, 0)
        TlpAutoModeStatus.Name = "TlpAutoModeStatus"
        TlpAutoModeStatus.RowCount = 2
        TlpAutoModeStatus.RowStyles.Add(New RowStyle())
        TlpAutoModeStatus.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpAutoModeStatus.Size = New Size(1376, 658)
        TlpAutoModeStatus.TabIndex = 0
        ' 
        ' TlpAutoModeStatusTop
        ' 
        TlpAutoModeStatusTop.AutoSize = True
        TlpAutoModeStatusTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpAutoModeStatusTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpAutoModeStatusTop.ColumnCount = 2
        TlpAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle())
        TlpAutoModeStatusTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpAutoModeStatusTop.Dock = DockStyle.Fill
        TlpAutoModeStatusTop.LabelText = "Marker : Auto Mode Status"
        TlpAutoModeStatusTop.Location = New Point(6, 6)
        TlpAutoModeStatusTop.Name = "TlpAutoModeStatusTop"
        TlpAutoModeStatusTop.RowCount = 1
        TlpAutoModeStatusTop.RowStyles.Add(New RowStyle())
        TlpAutoModeStatusTop.Size = New Size(1364, 37)
        TlpAutoModeStatusTop.TabIndex = 1
        ' 
        ' TabPage03BgReadings
        ' 
        TabPage03BgReadings.Controls.Add(TlpBgReadings)
        TabPage03BgReadings.Location = New Point(4, 30)
        TabPage03BgReadings.Name = "TabPage03BgReadings"
        TabPage03BgReadings.Size = New Size(1376, 658)
        TabPage03BgReadings.TabIndex = 3
        TabPage03BgReadings.Text = "Bg Readings"
        TabPage03BgReadings.UseVisualStyleBackColor = True
        ' 
        ' TlpBgReadings
        ' 
        TlpBgReadings.AutoScroll = True
        TlpBgReadings.AutoSize = True
        TlpBgReadings.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpBgReadings.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpBgReadings.ColumnCount = 1
        TlpBgReadings.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpBgReadings.Controls.Add(TlpBgReadingsTop, 0, 0)
        TlpBgReadings.Controls.Add(DgvSensorBgReadings, 0, 1)
        TlpBgReadings.Dock = DockStyle.Fill
        TlpBgReadings.Location = New Point(0, 0)
        TlpBgReadings.Name = "TlpBgReadings"
        TlpBgReadings.RowCount = 2
        TlpBgReadings.RowStyles.Add(New RowStyle())
        TlpBgReadings.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpBgReadings.Size = New Size(1376, 658)
        TlpBgReadings.TabIndex = 1
        ' 
        ' TlpBgReadingsTop
        ' 
        TlpBgReadingsTop.AutoSize = True
        TlpBgReadingsTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpBgReadingsTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpBgReadingsTop.ColumnCount = 2
        TlpBgReadingsTop.ColumnStyles.Add(New ColumnStyle())
        TlpBgReadingsTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpBgReadingsTop.Dock = DockStyle.Fill
        TlpBgReadingsTop.LabelText = "Marker : Blood Glucose Readings"
        TlpBgReadingsTop.Location = New Point(6, 6)
        TlpBgReadingsTop.Name = "TlpBgReadingsTop"
        TlpBgReadingsTop.RowCount = 1
        TlpBgReadingsTop.RowStyles.Add(New RowStyle())
        TlpBgReadingsTop.Size = New Size(1364, 37)
        TlpBgReadingsTop.TabIndex = 1
        ' 
        ' TabPage04Calibration
        ' 
        TabPage04Calibration.Controls.Add(TlpCalibration)
        TabPage04Calibration.Location = New Point(4, 30)
        TabPage04Calibration.Name = "TabPage04Calibration"
        TabPage04Calibration.Size = New Size(1376, 658)
        TabPage04Calibration.TabIndex = 4
        TabPage04Calibration.Text = "Calibration"
        TabPage04Calibration.UseVisualStyleBackColor = True
        ' 
        ' TlpCalibration
        ' 
        TlpCalibration.AutoScroll = True
        TlpCalibration.AutoSize = True
        TlpCalibration.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpCalibration.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpCalibration.ColumnCount = 1
        TlpCalibration.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpCalibration.Controls.Add(TlpCalibrationTop, 0, 0)
        TlpCalibration.Controls.Add(DgvCalibration, 0, 1)
        TlpCalibration.Dock = DockStyle.Fill
        TlpCalibration.Location = New Point(0, 0)
        TlpCalibration.Name = "TlpCalibration"
        TlpCalibration.RowCount = 2
        TlpCalibration.RowStyles.Add(New RowStyle())
        TlpCalibration.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpCalibration.Size = New Size(1376, 658)
        TlpCalibration.TabIndex = 1
        ' 
        ' TlpCalibrationTop
        ' 
        TlpCalibrationTop.AutoSize = True
        TlpCalibrationTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpCalibrationTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpCalibrationTop.ColumnCount = 2
        TlpCalibrationTop.ColumnStyles.Add(New ColumnStyle())
        TlpCalibrationTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpCalibrationTop.Dock = DockStyle.Fill
        TlpCalibrationTop.LabelText = "Marker : Calibration"
        TlpCalibrationTop.Location = New Point(6, 6)
        TlpCalibrationTop.Name = "TlpCalibrationTop"
        TlpCalibrationTop.RowCount = 1
        TlpCalibrationTop.RowStyles.Add(New RowStyle())
        TlpCalibrationTop.Size = New Size(1364, 37)
        TlpCalibrationTop.TabIndex = 1
        ' 
        ' TabPage05Insulin
        ' 
        TabPage05Insulin.Controls.Add(TlpInsulin)
        TabPage05Insulin.Location = New Point(4, 30)
        TabPage05Insulin.Name = "TabPage05Insulin"
        TabPage05Insulin.Size = New Size(1376, 658)
        TabPage05Insulin.TabIndex = 5
        TabPage05Insulin.Text = "Insulin"
        TabPage05Insulin.UseVisualStyleBackColor = True
        ' 
        ' TlpInsulin
        ' 
        TlpInsulin.AutoScroll = True
        TlpInsulin.AutoSize = True
        TlpInsulin.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpInsulin.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpInsulin.ColumnCount = 1
        TlpInsulin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpInsulin.Controls.Add(TlpInsulinTop, 0, 0)
        TlpInsulin.Controls.Add(DgvInsulin, 0, 1)
        TlpInsulin.Dock = DockStyle.Fill
        TlpInsulin.Location = New Point(0, 0)
        TlpInsulin.Name = "TlpInsulin"
        TlpInsulin.RowCount = 2
        TlpInsulin.RowStyles.Add(New RowStyle())
        TlpInsulin.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpInsulin.Size = New Size(1376, 658)
        TlpInsulin.TabIndex = 1
        ' 
        ' TlpInsulinTop
        ' 
        TlpInsulinTop.AutoSize = True
        TlpInsulinTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpInsulinTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpInsulinTop.ColumnCount = 2
        TlpInsulinTop.ColumnStyles.Add(New ColumnStyle())
        TlpInsulinTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpInsulinTop.Dock = DockStyle.Fill
        TlpInsulinTop.LabelText = "Marker : Insulin"
        TlpInsulinTop.Location = New Point(6, 6)
        TlpInsulinTop.Name = "TlpInsulinTop"
        TlpInsulinTop.RowCount = 1
        TlpInsulinTop.RowStyles.Add(New RowStyle())
        TlpInsulinTop.Size = New Size(1364, 37)
        TlpInsulinTop.TabIndex = 1
        ' 
        ' TabPage06LowGlucoseSuspended
        ' 
        TabPage06LowGlucoseSuspended.Controls.Add(TlpLowGlucoseSuspended)
        TabPage06LowGlucoseSuspended.Location = New Point(4, 30)
        TabPage06LowGlucoseSuspended.Name = "TabPage06LowGlucoseSuspended"
        TabPage06LowGlucoseSuspended.Size = New Size(1376, 658)
        TabPage06LowGlucoseSuspended.TabIndex = 6
        TabPage06LowGlucoseSuspended.Text = "Low Glucose Suspended"
        TabPage06LowGlucoseSuspended.UseVisualStyleBackColor = True
        ' 
        ' TlpLowGlucoseSuspended
        ' 
        TlpLowGlucoseSuspended.AutoScroll = True
        TlpLowGlucoseSuspended.AutoSize = True
        TlpLowGlucoseSuspended.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLowGlucoseSuspended.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpLowGlucoseSuspended.ColumnCount = 1
        TlpLowGlucoseSuspended.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLowGlucoseSuspended.Controls.Add(TlpLowGlucoseSuspendedTop, 0, 0)
        TlpLowGlucoseSuspended.Controls.Add(DgvLowGlucoseSuspended, 0, 1)
        TlpLowGlucoseSuspended.Dock = DockStyle.Fill
        TlpLowGlucoseSuspended.Location = New Point(0, 0)
        TlpLowGlucoseSuspended.Name = "TlpLowGlucoseSuspended"
        TlpLowGlucoseSuspended.RowCount = 2
        TlpLowGlucoseSuspended.RowStyles.Add(New RowStyle())
        TlpLowGlucoseSuspended.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpLowGlucoseSuspended.Size = New Size(1376, 658)
        TlpLowGlucoseSuspended.TabIndex = 1
        ' 
        ' TlpLowGlucoseSuspendedTop
        ' 
        TlpLowGlucoseSuspendedTop.AutoSize = True
        TlpLowGlucoseSuspendedTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpLowGlucoseSuspendedTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpLowGlucoseSuspendedTop.ColumnCount = 2
        TlpLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle())
        TlpLowGlucoseSuspendedTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpLowGlucoseSuspendedTop.Dock = DockStyle.Fill
        TlpLowGlucoseSuspendedTop.LabelText = "Marker : Low Glucose Suspended"
        TlpLowGlucoseSuspendedTop.Location = New Point(6, 6)
        TlpLowGlucoseSuspendedTop.Name = "TlpLowGlucoseSuspendedTop"
        TlpLowGlucoseSuspendedTop.RowCount = 1
        TlpLowGlucoseSuspendedTop.RowStyles.Add(New RowStyle())
        TlpLowGlucoseSuspendedTop.Size = New Size(1364, 37)
        TlpLowGlucoseSuspendedTop.TabIndex = 1
        ' 
        ' TabPage07Meal
        ' 
        TabPage07Meal.Controls.Add(TlpMeal)
        TabPage07Meal.Location = New Point(4, 30)
        TabPage07Meal.Name = "TabPage07Meal"
        TabPage07Meal.Size = New Size(1376, 658)
        TabPage07Meal.TabIndex = 7
        TabPage07Meal.Text = "Meal"
        TabPage07Meal.UseVisualStyleBackColor = True
        ' 
        ' TlpMeal
        ' 
        TlpMeal.AutoScroll = True
        TlpMeal.AutoSize = True
        TlpMeal.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpMeal.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpMeal.ColumnCount = 1
        TlpMeal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpMeal.Controls.Add(TlpMealTop, 0, 0)
        TlpMeal.Controls.Add(DgvMeal, 0, 1)
        TlpMeal.Dock = DockStyle.Fill
        TlpMeal.Location = New Point(0, 0)
        TlpMeal.Name = "TlpMeal"
        TlpMeal.RowCount = 2
        TlpMeal.RowStyles.Add(New RowStyle())
        TlpMeal.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpMeal.Size = New Size(1376, 658)
        TlpMeal.TabIndex = 1
        ' 
        ' TlpMealTop
        ' 
        TlpMealTop.AutoSize = True
        TlpMealTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpMealTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpMealTop.ColumnCount = 2
        TlpMealTop.ColumnStyles.Add(New ColumnStyle())
        TlpMealTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpMealTop.Dock = DockStyle.Fill
        TlpMealTop.LabelText = "Marker : Meal"
        TlpMealTop.Location = New Point(6, 6)
        TlpMealTop.Name = "TlpMealTop"
        TlpMealTop.RowCount = 1
        TlpMealTop.RowStyles.Add(New RowStyle())
        TlpMealTop.Size = New Size(1364, 37)
        TlpMealTop.TabIndex = 1
        ' 
        ' TabPage08TimeChange
        ' 
        TabPage08TimeChange.Controls.Add(TlpTimeChange)
        TabPage08TimeChange.Location = New Point(4, 30)
        TabPage08TimeChange.Name = "TabPage08TimeChange"
        TabPage08TimeChange.Size = New Size(1376, 658)
        TabPage08TimeChange.TabIndex = 8
        TabPage08TimeChange.Text = "Time Change"
        TabPage08TimeChange.UseVisualStyleBackColor = True
        ' 
        ' TlpTimeChange
        ' 
        TlpTimeChange.AutoScroll = True
        TlpTimeChange.AutoSize = True
        TlpTimeChange.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpTimeChange.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble
        TlpTimeChange.ColumnCount = 1
        TlpTimeChange.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpTimeChange.Controls.Add(TlpTimeChangeTop, 0, 0)
        TlpTimeChange.Controls.Add(DgvTimeChange, 0, 1)
        TlpTimeChange.Dock = DockStyle.Fill
        TlpTimeChange.Location = New Point(0, 0)
        TlpTimeChange.Name = "TlpTimeChange"
        TlpTimeChange.RowCount = 2
        TlpTimeChange.RowStyles.Add(New RowStyle())
        TlpTimeChange.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TlpTimeChange.Size = New Size(1376, 658)
        TlpTimeChange.TabIndex = 1
        ' 
        ' TlpTimeChangeTop
        ' 
        TlpTimeChangeTop.AutoSize = True
        TlpTimeChangeTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TlpTimeChangeTop.ButtonText = "Return To 'Summary Data' Tab"
        TlpTimeChangeTop.ColumnCount = 2
        TlpTimeChangeTop.ColumnStyles.Add(New ColumnStyle())
        TlpTimeChangeTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TlpTimeChangeTop.Dock = DockStyle.Fill
        TlpTimeChangeTop.LabelText = "Marker : Time Change"
        TlpTimeChangeTop.Location = New Point(6, 6)
        TlpTimeChangeTop.Name = "TlpTimeChangeTop"
        TlpTimeChangeTop.RowCount = 1
        TlpTimeChangeTop.RowStyles.Add(New RowStyle())
        TlpTimeChangeTop.Size = New Size(1364, 37)
        TlpTimeChangeTop.TabIndex = 1
        ' 
        ' TabPage09BasalPerHour
        ' 
        TabPage09BasalPerHour.Controls.Add(DgvBasalPerHour)
        TabPage09BasalPerHour.Location = New Point(4, 30)
        TabPage09BasalPerHour.Name = "TabPage09BasalPerHour"
        TabPage09BasalPerHour.Size = New Size(1376, 658)
        TabPage09BasalPerHour.TabIndex = 9
        TabPage09BasalPerHour.Text = "SmartGuard Basal Per Hour"
        TabPage09BasalPerHour.UseVisualStyleBackColor = True
        ' 
        ' TabPage10CurrentUser
        ' 
        TabPage10CurrentUser.Controls.Add(DgvCurrentUser)
        TabPage10CurrentUser.Location = New Point(4, 30)
        TabPage10CurrentUser.Name = "TabPage10CurrentUser"
        TabPage10CurrentUser.Size = New Size(1376, 658)
        TabPage10CurrentUser.TabIndex = 10
        TabPage10CurrentUser.Text = "Current User"
        TabPage10CurrentUser.UseVisualStyleBackColor = True
        ' 
        ' TabPage11AllUsers
        ' 
        TabPage11AllUsers.Controls.Add(DgvCareLinkUsers)
        TabPage11AllUsers.Location = New Point(4, 30)
        TabPage11AllUsers.Name = "TabPage11AllUsers"
        TabPage11AllUsers.Size = New Size(1376, 658)
        TabPage11AllUsers.TabIndex = 11
        TabPage11AllUsers.Text = "All Users"
        TabPage11AllUsers.UseVisualStyleBackColor = True
        ' 
        ' TabPage12BackToHomePage
        ' 
        TabPage12BackToHomePage.Location = New Point(4, 30)
        TabPage12BackToHomePage.Name = "TabPage12BackToHomePage"
        TabPage12BackToHomePage.Size = New Size(1376, 658)
        TabPage12BackToHomePage.TabIndex = 12
        TabPage12BackToHomePage.Text = "Back.."
        TabPage12BackToHomePage.UseVisualStyleBackColor = True
        ' 
        ' TabControlPage1
        ' 
        TabControlPage1.Controls.Add(TabPage01HomePage)
        TabControlPage1.Controls.Add(TabPage02RunningIOB)
        TabControlPage1.Controls.Add(TabPage03TreatmentDetails)
        TabControlPage1.Controls.Add(TabPage04SummaryData)
        TabControlPage1.Controls.Add(TabPage05ActiveInsulin)
        TabControlPage1.Controls.Add(TabPage06Basal)
        TabControlPage1.Controls.Add(TabPage07LastAlarm)
        TabControlPage1.Controls.Add(TabPage08LastSG)
        TabControlPage1.Controls.Add(TabPage09Limits)
        TabControlPage1.Controls.Add(TabPage10NotificationActive)
        TabControlPage1.Controls.Add(TabPage11NotificationsCleared)
        TabControlPage1.Controls.Add(TabPage12PumpBannerState)
        TabControlPage1.Controls.Add(TabPage13SensorGlucose)
        TabControlPage1.Controls.Add(TabPage14TherapyAlgorithmState)
        TabControlPage1.Controls.Add(TabPage15More)
        TabControlPage1.Dock = DockStyle.Fill
        TabControlPage1.Font = New Font("Segoe UI", 9.0F)
        TabControlPage1.Location = New Point(0, 24)
        TabControlPage1.Name = "TabControlPage1"
        TabControlPage1.Padding = New Point(6, 6)
        TabControlPage1.SelectedIndex = 0
        TabControlPage1.Size = New Size(1384, 692)
        TabControlPage1.TabIndex = 1
        ' 
        ' TabControlPage2
        ' 
        TabControlPage2.Controls.Add(TabPage01AutoBasalDelivery)
        TabControlPage2.Controls.Add(TabPage02AutoModeStatus)
        TabControlPage2.Controls.Add(TabPage03BgReadings)
        TabControlPage2.Controls.Add(TabPage04Calibration)
        TabControlPage2.Controls.Add(TabPage05Insulin)
        TabControlPage2.Controls.Add(TabPage06LowGlucoseSuspended)
        TabControlPage2.Controls.Add(TabPage07Meal)
        TabControlPage2.Controls.Add(TabPage08TimeChange)
        TabControlPage2.Controls.Add(TabPage09BasalPerHour)
        TabControlPage2.Controls.Add(TabPage10CurrentUser)
        TabControlPage2.Controls.Add(TabPage11AllUsers)
        TabControlPage2.Controls.Add(TabPage12BackToHomePage)
        TabControlPage2.Dock = DockStyle.Fill
        TabControlPage2.Font = New Font("Segoe UI", 9.0F)
        TabControlPage2.Location = New Point(0, 24)
        TabControlPage2.Name = "TabControlPage2"
        TabControlPage2.Padding = New Point(6, 6)
        TabControlPage2.SelectedIndex = 0
        TabControlPage2.Size = New Size(1384, 692)
        TabControlPage2.TabIndex = 0
        ' 
        ' TimeChangeLabel
        ' 
        TimeChangeLabel.AutoSize = True
        TimeChangeLabel.Dock = DockStyle.Fill
        TimeChangeLabel.Location = New Point(6, 6)
        TimeChangeLabel.Margin = New Padding(3)
        TimeChangeLabel.Name = "TimeChangeLabel"
        TimeChangeLabel.Size = New Size(174, 15)
        TimeChangeLabel.TabIndex = 0
        TimeChangeLabel.Text = "Time Change"
        TimeChangeLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TimeInTightRangeLabel
        ' 
        TimeInTightRangeLabel.Location = New Point(0, 0)
        TimeInTightRangeLabel.Name = "TimeInTightRangeLabel"
        TimeInTightRangeLabel.Size = New Size(100, 23)
        TimeInTightRangeLabel.TabIndex = 0
        ' 
        ' WebView
        ' 
        WebView.AllowExternalDrop = False
        WebView.CreationProperties = Nothing
        WebView.DefaultBackgroundColor = Color.LightGray
        WebView.Dock = DockStyle.Fill
        WebView.Location = New Point(597, 6)
        WebView.Name = "WebView"
        WebView.Size = New Size(767, 643)
        WebView.TabIndex = 3
        WebView.ZoomFactor = 1.0R
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
        Me.Margin = New Padding(0, 3, 0, 3)
        Me.MaximumSize = New Size(1400, 960)
        Me.Name = "Form1"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "CareLink™ For Windows"
        CType(CalibrationDueImage, ComponentModel.ISupportInitialize).EndInit()
        CalibrationShieldPanel.ResumeLayout(False)
        CalibrationShieldPanel.PerformLayout()
        CType(SmartGuardShieldPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(CareLinkUserDataRecordBindingSource, ComponentModel.ISupportInitialize).EndInit()
        CursorPanel.ResumeLayout(False)
        CType(CursorPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvActiveInsulin, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvAutoBasalDelivery, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvAutoModeStatus, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvBasal, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvBasalPerHour, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCalibration, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCareLinkUsers, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvCurrentUser, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvInsulin, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvLastAlarm, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvLastSensorGlucose, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvLimits, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvLowGlucoseSuspended, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvMeal, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvPumpBannerState, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSensorBgReadings, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSGs, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvSummary, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvTherapyAlgorithmState, ComponentModel.ISupportInitialize).EndInit()
        CType(DgvTimeChange, ComponentModel.ISupportInitialize).EndInit()
        CType(InsulinLevelPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Last24HrTotalsPanel.ResumeLayout(False)
        Last24HrTotalsPanel.PerformLayout()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        CType(PumpBatteryPictureBox, ComponentModel.ISupportInitialize).EndInit()
        SensorTimeLeftPanel.ResumeLayout(False)
        CType(SensorTimeLeftPictureBox, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel1.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
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
        SplitContainerNotificationsCleared.Panel1.ResumeLayout(False)
        SplitContainerNotificationsCleared.Panel1.PerformLayout()
        SplitContainerNotificationsCleared.Panel2.ResumeLayout(False)
        SplitContainerNotificationsCleared.Panel2.PerformLayout()
        CType(SplitContainerNotificationsCleared, ComponentModel.ISupportInitialize).EndInit()
        SplitContainerNotificationsCleared.ResumeLayout(False)
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        TabPage01HomePage.ResumeLayout(False)
        TabPage02RunningIOB.ResumeLayout(False)
        TabPage04SummaryData.ResumeLayout(False)
        TabPage05ActiveInsulin.ResumeLayout(False)
        TabPage05ActiveInsulin.PerformLayout()
        TlpActiveInsulin.ResumeLayout(False)
        TlpActiveInsulin.PerformLayout()
        TabPage06Basal.ResumeLayout(False)
        TabPage06Basal.PerformLayout()
        TlpBasal.ResumeLayout(False)
        TlpBasal.PerformLayout()
        TabPage07LastAlarm.ResumeLayout(False)
        TabPage07LastAlarm.PerformLayout()
        TlpLastAlarm.ResumeLayout(False)
        TlpLastAlarm.PerformLayout()
        TabPage08LastSG.ResumeLayout(False)
        TabPage08LastSG.PerformLayout()
        TlpLastSG.ResumeLayout(False)
        TlpLastSG.PerformLayout()
        TabPage09Limits.ResumeLayout(False)
        TabPage09Limits.PerformLayout()
        TlpLimits.ResumeLayout(False)
        TlpLimits.PerformLayout()
        TabPage10NotificationActive.ResumeLayout(False)
        TabPage10NotificationActive.PerformLayout()
        TlpNotificationActive.ResumeLayout(False)
        TlpNotificationActive.PerformLayout()
        TabPage11NotificationsCleared.ResumeLayout(False)
        TabPage12PumpBannerState.ResumeLayout(False)
        TabPage12PumpBannerState.PerformLayout()
        TlpPumpBannerState.ResumeLayout(False)
        TlpPumpBannerState.PerformLayout()
        TabPage13SensorGlucose.ResumeLayout(False)
        TabPage13SensorGlucose.PerformLayout()
        TlpSgs.ResumeLayout(False)
        TlpSgs.PerformLayout()
        TabPage14TherapyAlgorithmState.ResumeLayout(False)
        TabPage14TherapyAlgorithmState.PerformLayout()
        TlpTherapyAlgorithmState.ResumeLayout(False)
        TlpTherapyAlgorithmState.PerformLayout()
        TabPage01AutoBasalDelivery.ResumeLayout(False)
        TabPage01AutoBasalDelivery.PerformLayout()
        TlpAutoBasalDelivery.ResumeLayout(False)
        TlpAutoBasalDelivery.PerformLayout()
        TabPage02AutoModeStatus.ResumeLayout(False)
        TabPage02AutoModeStatus.PerformLayout()
        TlpAutoModeStatus.ResumeLayout(False)
        TlpAutoModeStatus.PerformLayout()
        TabPage03BgReadings.ResumeLayout(False)
        TabPage03BgReadings.PerformLayout()
        TlpBgReadings.ResumeLayout(False)
        TlpBgReadings.PerformLayout()
        TabPage04Calibration.ResumeLayout(False)
        TabPage04Calibration.PerformLayout()
        TlpCalibration.ResumeLayout(False)
        TlpCalibration.PerformLayout()
        TabPage05Insulin.ResumeLayout(False)
        TabPage05Insulin.PerformLayout()
        TlpInsulin.ResumeLayout(False)
        TlpInsulin.PerformLayout()
        TabPage06LowGlucoseSuspended.ResumeLayout(False)
        TabPage06LowGlucoseSuspended.PerformLayout()
        TlpLowGlucoseSuspended.ResumeLayout(False)
        TlpLowGlucoseSuspended.PerformLayout()
        TabPage07Meal.ResumeLayout(False)
        TabPage07Meal.PerformLayout()
        TlpMeal.ResumeLayout(False)
        TlpMeal.PerformLayout()
        TabPage08TimeChange.ResumeLayout(False)
        TabPage08TimeChange.PerformLayout()
        TlpTimeChange.ResumeLayout(False)
        TlpTimeChange.PerformLayout()
        TabPage09BasalPerHour.ResumeLayout(False)
        TabPage10CurrentUser.ResumeLayout(False)
        TabPage11AllUsers.ResumeLayout(False)
        TabControlPage1.ResumeLayout(False)
        TabControlPage2.ResumeLayout(False)
        CType(WebView, ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents DgvActiveInsulin As DataGridView
    Friend WithEvents DgvAutoBasalDelivery As DataGridView
    Friend WithEvents DgvAutoModeStatus As DataGridView
    Friend WithEvents DgvBasal As DataGridView
    Friend WithEvents DgvBasalPerHour As DataGridView
    Friend WithEvents DgvCalibration As DataGridView
    Friend WithEvents DgvCareLinkUsers As DataGridView
    Friend WithEvents DgvCareLinkUsersUserPassword As DataGridViewTextBoxColumn
    Friend WithEvents DgvCurrentUser As DataGridView
    Friend WithEvents DgvInsulin As DataGridView
    Friend WithEvents DgvLastAlarm As DataGridView
    Friend WithEvents DgvLastSensorGlucose As DataGridView
    Friend WithEvents DgvLimits As DataGridView
    Friend WithEvents DgvLowGlucoseSuspended As DataGridView
    Friend WithEvents DgvMeal As DataGridView
    Friend WithEvents DgvPumpBannerState As DataGridView
    Friend WithEvents DgvSensorBgReadings As DataGridView
    Friend WithEvents DgvSGs As DataGridView
    Friend WithEvents DgvSummary As DataGridView
    Friend WithEvents DgvTherapyAlgorithmState As DataGridView
    Friend WithEvents DgvTimeChange As DataGridView
    Friend WithEvents FullNameLabel As Label
    Friend WithEvents GraphLast24HoursLabel As Label
    Friend WithEvents HighTirComplianceLabel As Label
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents InsulinTypeLabel As Label
    Friend WithEvents Last24HrAutoCorrectionLabel As Label
    Friend WithEvents Last24HrAutoCorrectionPercentLabel As Label
    Friend WithEvents Last24HrAutoCorrectionUnitsLabel As Label
    Friend WithEvents Last24HrBasalLabel As Label
    Friend WithEvents Last24HrBasalPercentLabel As Label
    Friend WithEvents Last24HrBasalUnitsLabel As Label
    Friend WithEvents Last24HrCarbsLabel As Label
    Friend WithEvents Last24HrCarbsValueLabel As Label
    Friend WithEvents Last24HrMealBolusLabel As Label
    Friend WithEvents Last24HrMealBolusPercentLabel As Label
    Friend WithEvents Last24HrMealBolusUnitsLabel As Label
    Friend WithEvents Last24HrSummaryLabel As Label
    Friend WithEvents Last24HrTotalInsulinLabel As Label
    Friend WithEvents Last24HrTotalInsulinUnitsLabel As Label
    Friend WithEvents Last24HrTotalsPanel As Panel
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
    Friend WithEvents MenuOptionsConfigureTiTR As ToolStripMenuItem
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
    Friend WithEvents MenuStartHereExit As ToolStripMenuItem
    Friend WithEvents MenuStartHereLoadSavedDataFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereManuallyImportDeviceSettings As ToolStripMenuItem
    Friend WithEvents MenuStartHereSaveSnapshotFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereShowPumpSetup As ToolStripMenuItem
    Friend WithEvents MenuStartHereUseExceptionReport As ToolStripMenuItem
    Friend WithEvents MenuStartHereUseLastSavedFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereUserLogin As ToolStripMenuItem
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
    Friend WithEvents SgTrendLabel As Label
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents SmartGuardLabel As Label
    Friend WithEvents SmartGuardShieldPictureBox As PictureBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents SplitContainerNotificationsCleared As SplitContainer
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusStripSpacerRight As ToolStripStatusLabel
    Friend WithEvents StatusStripSpeech As ToolStripStatusLabel
    Friend WithEvents TabControlPage1 As TabControl
    Friend WithEvents TabControlPage2 As TabControl
    Friend WithEvents TabPage01AutoBasalDelivery As TabPage
    Friend WithEvents TabPage01HomePage As TabPage
    Friend WithEvents TabPage02AutoModeStatus As TabPage
    Friend WithEvents TabPage02RunningIOB As TabPage
    Friend WithEvents TabPage03BgReadings As TabPage
    Friend WithEvents TabPage03TreatmentDetails As TabPage
    Friend WithEvents TabPage04Calibration As TabPage
    Friend WithEvents TabPage04SummaryData As TabPage
    Friend WithEvents TabPage05ActiveInsulin As TabPage
    Friend WithEvents TabPage05Insulin As TabPage
    Friend WithEvents TabPage06Basal As TabPage
    Friend WithEvents TabPage06LowGlucoseSuspended As TabPage
    Friend WithEvents TabPage07LastAlarm As TabPage
    Friend WithEvents TabPage07Meal As TabPage
    Friend WithEvents TabPage08LastSG As TabPage
    Friend WithEvents TabPage08TimeChange As TabPage
    Friend WithEvents TabPage09BasalPerHour As TabPage
    Friend WithEvents TabPage09Limits As TabPage
    Friend WithEvents TabPage10CurrentUser As TabPage
    Friend WithEvents TabPage10NotificationActive As TabPage
    Friend WithEvents TabPage11AllUsers As TabPage
    Friend WithEvents TabPage11NotificationsCleared As TabPage
    Friend WithEvents TabPage12BackToHomePage As TabPage
    Friend WithEvents TabPage12PumpBannerState As TabPage
    Friend WithEvents TabPage13SensorGlucose As TabPage
    Friend WithEvents TabPage14TherapyAlgorithmState As TabPage
    Friend WithEvents TabPage15More As TabPage
    Friend WithEvents TemporaryUseAdvanceAITDecayCheckBox As CheckBox
    Friend WithEvents TimeChangeLabel As Label
    Friend WithEvents TimeInRangeChartLabel As Label
    Friend WithEvents TimeInRangeHeaderLabel As Label
    Friend WithEvents TimeInRangeMessageLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents TimeInTightRangeLabel As Label
    Friend WithEvents TimeInTightRangeValueLabel As Label
    Friend WithEvents TimeZoneToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents TirComplianceLabel As Label
    Friend WithEvents TiTRMgsLabel As Label
    Friend WithEvents TiTRMgsLabel2 As Label
    Friend WithEvents TlpActiveInsulin As TableLayoutPanel
    Friend WithEvents TlpActiveInsulinTop As TableLayoutPanelTopEx
    Friend WithEvents TlpAutoBasalDelivery As TableLayoutPanel
    Friend WithEvents TlpAutoBasalDeliveryTop As TableLayoutPanelTopEx
    Friend WithEvents TlpAutoModeStatus As TableLayoutPanel
    Friend WithEvents TlpAutoModeStatusTop As TableLayoutPanelTopEx
    Friend WithEvents TlpBasal As TableLayoutPanel
    Friend WithEvents TlpBasalTop As TableLayoutPanelTopEx
    Friend WithEvents TlpBgReadings As TableLayoutPanel
    Friend WithEvents TlpBgReadingsTop As TableLayoutPanelTopEx
    Friend WithEvents TlpCalibration As TableLayoutPanel
    Friend WithEvents TlpCalibrationTop As TableLayoutPanelTopEx
    Friend WithEvents TlpInsulin As TableLayoutPanel
    Friend WithEvents TlpInsulinTop As TableLayoutPanelTopEx
    Friend WithEvents TlpLastAlarm As TableLayoutPanel
    Friend WithEvents TlpLastAlarmTop As TableLayoutPanelTopEx
    Friend WithEvents TlpLastSG As TableLayoutPanel
    Friend WithEvents TlpLastSgTop As TableLayoutPanelTopEx
    Friend WithEvents TlpLimits As TableLayoutPanel
    Friend WithEvents TlpLimitsTop As TableLayoutPanelTopEx
    Friend WithEvents TlpLowGlucoseSuspended As TableLayoutPanel
    Friend WithEvents TlpLowGlucoseSuspendedTop As TableLayoutPanelTopEx
    Friend WithEvents TlpMeal As TableLayoutPanel
    Friend WithEvents TlpMealTop As TableLayoutPanelTopEx
    Friend WithEvents TlpNotificationActive As TableLayoutPanel
    Friend WithEvents TlpNotificationActiveTop As TableLayoutPanelTopEx
    Friend WithEvents TlpNotificationsCleared As TableLayoutPanel
    Friend WithEvents TlpNotificationsClearedTop As TableLayoutPanelTopEx
    Friend WithEvents TlpPumpBannerState As TableLayoutPanel
    Friend WithEvents TlpPumpBannerStateTop As TableLayoutPanelTopEx
    Friend WithEvents TlpSgs As TableLayoutPanel
    Friend WithEvents TlpSgsTop As TableLayoutPanelTopEx
    Friend WithEvents TlpTherapyAlgorithmState As TableLayoutPanel
    Friend WithEvents TlpTherapyAlgorithmStateTop As TableLayoutPanelTopEx
    Friend WithEvents TlpTimeChange As TableLayoutPanel
    Friend WithEvents TlpTimeChangeTop As TableLayoutPanelTopEx
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TransmitterBatteryPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents TrendArrowsLabel As Label
    Friend WithEvents TrendValueLabel As Label
    Friend WithEvents UpdateAvailableStatusStripLabel As ToolStripStatusLabel
    Friend WithEvents WebView As Microsoft.Web.WebView2.WinForms.WebView2
End Class
