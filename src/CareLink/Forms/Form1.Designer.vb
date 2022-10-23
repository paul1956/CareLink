Imports DataGridViewColumnControls

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MenuStartHere = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStartHereLogin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStartHereLoadSavedDataFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStartHereExceptionReportLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStartHereUseLastSavedFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStartHereUseTestData = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStartHereSnapshotSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.StartHereExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsAutoLogin = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsFilterRawJSONData = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsSetupEmailServer = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsUseAdvancedAITDecay = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsUseLocalTimeZone = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpReportAnIssue = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowMiniDisplay = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerUpdateTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TabControlHomePage = New System.Windows.Forms.TabControl()
        Me.TabPage01HomePage = New System.Windows.Forms.TabPage()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.LabelTrendValue = New System.Windows.Forms.Label()
        Me.MaxBasalPerHourLabel = New System.Windows.Forms.Label()
        Me.Last24HTotalsPanel = New System.Windows.Forms.Panel()
        Me.Last24CarbsValueLabel = New System.Windows.Forms.Label()
        Me.TotalsLabel = New System.Windows.Forms.Label()
        Me.AutoCorrectionLabel = New System.Windows.Forms.Label()
        Me.ManualBolusLabel = New System.Windows.Forms.Label()
        Me.BasalLabel = New System.Windows.Forms.Label()
        Me.DailyDoseLabel = New System.Windows.Forms.Label()
        Me.SensorTimeLeftPanel = New System.Windows.Forms.Panel()
        Me.SensorDaysLeftLabel = New System.Windows.Forms.Label()
        Me.SensorTimeLeftLabel = New System.Windows.Forms.Label()
        Me.SensorTimeLeftPictureBox = New System.Windows.Forms.PictureBox()
        Me.LabelTrendArrows = New System.Windows.Forms.Label()
        Me.LabelSgTrend = New System.Windows.Forms.Label()
        Me.ModelLabel = New System.Windows.Forms.Label()
        Me.SerialNumberLabel = New System.Windows.Forms.Label()
        Me.FullNameLabel = New System.Windows.Forms.Label()
        Me.ReadingIntervalLabel = New System.Windows.Forms.Label()
        Me.ReadingsLabel = New System.Windows.Forms.Label()
        Me.PumpBatteryRemainingLabel = New System.Windows.Forms.Label()
        Me.TransmatterBatterPercentLabel = New System.Windows.Forms.Label()
        Me.TransmitterBatteryPictureBox = New System.Windows.Forms.PictureBox()
        Me.PumpBatteryPictureBox = New System.Windows.Forms.PictureBox()
        Me.AITAlgorithmLabel = New System.Windows.Forms.Label()
        Me.RemainingInsulinUnits = New System.Windows.Forms.Label()
        Me.InsulinLevelPictureBox = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinValue = New System.Windows.Forms.Label()
        Me.CalibrationDueImage = New System.Windows.Forms.PictureBox()
        Me.CursorPanel = New System.Windows.Forms.Panel()
        Me.CursorPictureBox = New System.Windows.Forms.PictureBox()
        Me.CursorMessage1Label = New System.Windows.Forms.Label()
        Me.CursorMessage2Label = New System.Windows.Forms.Label()
        Me.CursorMessage3Label = New System.Windows.Forms.Label()
        Me.CalibrationShieldPanel = New System.Windows.Forms.Panel()
        Me.TempTargetLabel = New System.Windows.Forms.Label()
        Me.ShieldUnitsLabel = New System.Windows.Forms.Label()
        Me.LastSGTimeLabel = New System.Windows.Forms.Label()
        Me.CurrentBGLabel = New System.Windows.Forms.Label()
        Me.SensorMessage = New System.Windows.Forms.Label()
        Me.CalibrationShieldPictureBox = New System.Windows.Forms.PictureBox()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.Last24HoursLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeSummaryPercentCharLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeChartLabel = New System.Windows.Forms.Label()
        Me.InRangeMessageLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitMessageLabel = New System.Windows.Forms.Label()
        Me.BelowLowLimitValueLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeValueLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitValueLabel = New System.Windows.Forms.Label()
        Me.BelowLowLimitMessageLabel = New System.Windows.Forms.Label()
        Me.AverageSGValueLabel = New System.Windows.Forms.Label()
        Me.AverageSGMessageLabel = New System.Windows.Forms.Label()
        Me.TabPage02RunningIOB = New System.Windows.Forms.TabPage()
        Me.TabPage03TreatmentDetails = New System.Windows.Forms.TabPage()
        Me.TabPage04SummaryData = New System.Windows.Forms.TabPage()
        Me.DataGridViewSummary = New System.Windows.Forms.DataGridView()
        Me.TabPage05LastSG = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelLastSG = New System.Windows.Forms.TableLayoutPanel()
        Me.LastSGLabel = New System.Windows.Forms.Label()
        Me.TabPage06LastAlarm = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelLastAlarm = New System.Windows.Forms.TableLayoutPanel()
        Me.LastAlarmLabel = New System.Windows.Forms.Label()
        Me.TabPage07ActiveInsulin = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelActiveInsulin = New System.Windows.Forms.TableLayoutPanel()
        Me.ActiveInsulinTabLabel = New System.Windows.Forms.Label()
        Me.TabPage06SensorGlucose = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelSgs = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelSgs = New System.Windows.Forms.Label()
        Me.DataGridViewSGs = New System.Windows.Forms.DataGridView()
        Me.TabPage07Limits = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelLimits = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelLimits = New System.Windows.Forms.Label()
        Me.TabPage08NotificationHistory = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelNotificationHistory = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelNotificationHistory = New System.Windows.Forms.Label()
        Me.TabPage09Basal = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelBasal = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelBasal = New System.Windows.Forms.Label()
        Me.TabPage10TherapyAlgorithm = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelTherapyAlgorthm = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelTherapyAlgorthm = New System.Windows.Forms.Label()
        Me.TabPage11BannerState = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelBannerState = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelBannerState = New System.Windows.Forms.Label()
        Me.TabPage16Markers = New System.Windows.Forms.TabPage()
        Me.TabControlPage2 = New System.Windows.Forms.TabControl()
        Me.TabPageAutoBasalDelivery = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelAutoBasalDelivery = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelAutoBasalDelivery = New System.Windows.Forms.Label()
        Me.DataGridViewAutoBasalDelivery = New System.Windows.Forms.DataGridView()
        Me.TabPageAutoModeStatus = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelAutoModeStatus = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelAutoModeStatus = New System.Windows.Forms.Label()
        Me.TabPageBgReadings = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelBgReadings = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelBgReading = New System.Windows.Forms.Label()
        Me.TabPageCalibration = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelCalibration = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelCalibration = New System.Windows.Forms.Label()
        Me.TabPageInsulin = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelInsulin = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelInsulin = New System.Windows.Forms.Label()
        Me.DataGridViewInsulin = New System.Windows.Forms.DataGridView()
        Me.DataGridViewMeal = New System.Windows.Forms.DataGridView()
        Me.TabPageLowGlusoseSuspended = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelLowGlusoseSuspended = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelLowGlusoseSuspended = New System.Windows.Forms.Label()
        Me.TabPageMeal = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelMeal = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelMeal = New System.Windows.Forms.Label()
        Me.TabPageTimeChange = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelTimeChange = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelTimeChange = New System.Windows.Forms.Label()
        Me.TabPageCountryDataPg1 = New System.Windows.Forms.TabPage()
        Me.TabPageCountryDataPg2 = New System.Windows.Forms.TabPage()
        Me.TabPageCountryDataPg3 = New System.Windows.Forms.TabPage()
        Me.DataGridViewCountryItemsPage1 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewCountryItemsPage2 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewCountryItemsPage3 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumnCountrySettingsCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsKey = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Category = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Key = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Category = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Key = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPg3NotFor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCountrySettingsValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabPageUserProfile = New System.Windows.Forms.TabPage()
        Me.DataGridViewUserProfile = New System.Windows.Forms.DataGridView()
        Me.TabPageCurrentUser = New System.Windows.Forms.TabPage()
        Me.DataGridViewCurrentUser = New System.Windows.Forms.DataGridView()
        Me.TabPageAllUsers = New System.Windows.Forms.TabPage()
        Me.CareLinkUsersAITComboBox = New System.Windows.Forms.ComboBox()
        Me.DataGridViewCareLinkUsers = New System.Windows.Forms.DataGridView()
        Me.DataGridViewButtonColumnCareLinkDeleteRow = New DataGridViewDisableButtonColumn()
        Me.DataGridViewTextBoxColumnCareLinkUsersID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkUserName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkPassword = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkAIT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkCountryCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkMailserverPassword = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkMailServerPort = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkMailserverUserName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkSettingsVersion = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewCheckBoxColumnCareLinkAutoLogin = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.CareLinkUserDataRecordBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TabPageBackToHomePage = New System.Windows.Forms.TabPage()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.CursorTimer = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.LoginStatusLabel = New System.Windows.Forms.Label()
        Me.LoginStatus = New System.Windows.Forms.Label()
        Me.LastUpdateTimeLabel = New System.Windows.Forms.Label()
        Me.LastUpdateTime = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.TabControlHomePage.SuspendLayout()
        Me.TabPage01HomePage.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Last24HTotalsPanel.SuspendLayout()
        Me.SensorTimeLeftPanel.SuspendLayout()
        CType(Me.SensorTimeLeftPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransmitterBatteryPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PumpBatteryPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsulinLevelPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CalibrationDueImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CursorPanel.SuspendLayout()
        CType(Me.CursorPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CalibrationShieldPanel.SuspendLayout()
        CType(Me.CalibrationShieldPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.TabPage04SummaryData.SuspendLayout()
        CType(Me.DataGridViewSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage05LastSG.SuspendLayout()
        Me.TableLayoutPanelLastSG.SuspendLayout()
        Me.TabPage06LastAlarm.SuspendLayout()
        Me.TableLayoutPanelLastAlarm.SuspendLayout()
        Me.TabPage07ActiveInsulin.SuspendLayout()
        Me.TableLayoutPanelActiveInsulin.SuspendLayout()
        Me.TabPage06SensorGlucose.SuspendLayout()
        Me.TableLayoutPanelSgs.SuspendLayout()
        CType(Me.DataGridViewSGs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage07Limits.SuspendLayout()
        Me.TableLayoutPanelLimits.SuspendLayout()
        Me.TabPage08NotificationHistory.SuspendLayout()
        Me.TableLayoutPanelNotificationHistory.SuspendLayout()
        Me.TabPage09Basal.SuspendLayout()
        Me.TableLayoutPanelBasal.SuspendLayout()
        Me.TabPage10TherapyAlgorithm.SuspendLayout()
        Me.TableLayoutPanelTherapyAlgorthm.SuspendLayout()
        Me.TabPage11BannerState.SuspendLayout()
        Me.TableLayoutPanelBannerState.SuspendLayout()
        Me.TabControlPage2.SuspendLayout()
        Me.TabPageAutoBasalDelivery.SuspendLayout()
        Me.TableLayoutPanelAutoBasalDelivery.SuspendLayout()
        CType(Me.DataGridViewAutoBasalDelivery, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageAutoModeStatus.SuspendLayout()
        Me.TableLayoutPanelAutoModeStatus.SuspendLayout()
        Me.TabPageBgReadings.SuspendLayout()
        Me.TableLayoutPanelBgReadings.SuspendLayout()
        Me.TabPageCalibration.SuspendLayout()
        Me.TableLayoutPanelCalibration.SuspendLayout()
        Me.TabPageInsulin.SuspendLayout()
        Me.TableLayoutPanelInsulin.SuspendLayout()
        CType(Me.DataGridViewInsulin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageLowGlusoseSuspended.SuspendLayout()
        Me.TableLayoutPanelLowGlusoseSuspended.SuspendLayout()
        Me.TabPageMeal.SuspendLayout()
        Me.TableLayoutPanelMeal.SuspendLayout()
        CType(Me.DataGridViewMeal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageTimeChange.SuspendLayout()
        Me.TableLayoutPanelTimeChange.SuspendLayout()
        Me.TabPageCountryDataPg1.SuspendLayout()
        Me.TabPageCountryDataPg2.SuspendLayout()
        Me.TabPageCountryDataPg3.SuspendLayout()
        CType(Me.DataGridViewCountryItemsPage1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewCountryItemsPage2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewCountryItemsPage3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageUserProfile.SuspendLayout()
        CType(Me.DataGridViewUserProfile, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageCurrentUser.SuspendLayout()
        CType(Me.DataGridViewCurrentUser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageAllUsers.SuspendLayout()
        CType(Me.DataGridViewCareLinkUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CareLinkUserDataRecordBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuStartHere, Me.MenuOptions, Me.MenuHelp, Me.ShowMiniDisplay})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1384, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MenuStartHere
        '
        Me.MenuStartHere.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuStartHereLogin, Me.ToolStripSeparator1, Me.MenuStartHereLoadSavedDataFile, Me.MenuStartHereExceptionReportLoad, Me.ToolStripSeparator4, Me.MenuStartHereUseLastSavedFile, Me.MenuStartHereUseTestData, Me.ToolStripSeparator2, Me.MenuStartHereSnapshotSave, Me.ToolStripSeparator3, Me.StartHereExit})
        Me.MenuStartHere.Name = "MenuStartHere"
        Me.MenuStartHere.Size = New System.Drawing.Size(71, 20)
        Me.MenuStartHere.Text = "Start Here"
        '
        'MenuStartHereLogin
        '
        Me.MenuStartHereLogin.Name = "MenuStartHereLogin"
        Me.MenuStartHereLogin.Size = New System.Drawing.Size(211, 22)
        Me.MenuStartHereLogin.Text = "Login"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(208, 6)
        '
        'MenuStartHereLoadSavedDataFile
        '
        Me.MenuStartHereLoadSavedDataFile.Name = "MenuStartHereLoadSavedDataFile"
        Me.MenuStartHereLoadSavedDataFile.Size = New System.Drawing.Size(211, 22)
        Me.MenuStartHereLoadSavedDataFile.Text = "Load A Saved Data File"
        '
        'MenuStartHereExceptionReportLoad
        '
        Me.MenuStartHereExceptionReportLoad.Name = "MenuStartHereExceptionReportLoad"
        Me.MenuStartHereExceptionReportLoad.Size = New System.Drawing.Size(211, 22)
        Me.MenuStartHereExceptionReportLoad.Text = "Load An Exception Report"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(208, 6)
        '
        'MenuStartHereUseLastSavedFile
        '
        Me.MenuStartHereUseLastSavedFile.Name = "MenuStartHereUseLastSavedFile"
        Me.MenuStartHereUseLastSavedFile.Size = New System.Drawing.Size(211, 22)
        Me.MenuStartHereUseLastSavedFile.Text = "Use Last Data File"
        '
        'MenuStartHereUseTestData
        '
        Me.MenuStartHereUseTestData.Name = "MenuStartHereUseTestData"
        Me.MenuStartHereUseTestData.Size = New System.Drawing.Size(211, 22)
        Me.MenuStartHereUseTestData.Text = "Use Test Data"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(208, 6)
        '
        'MenuStartHereSnapshotSave
        '
        Me.MenuStartHereSnapshotSave.Name = "MenuStartHereSnapshotSave"
        Me.MenuStartHereSnapshotSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.MenuStartHereSnapshotSave.Size = New System.Drawing.Size(211, 22)
        Me.MenuStartHereSnapshotSave.Text = "Snapshot &Save"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(208, 6)
        '
        'StartHereExit
        '
        Me.StartHereExit.Image = Global.CareLink.My.Resources.Resources.AboutBox
        Me.StartHereExit.Name = "StartHereExit"
        Me.StartHereExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.StartHereExit.Size = New System.Drawing.Size(211, 22)
        Me.StartHereExit.Text = "E&xit"
        '
        'MenuOptions
        '
        Me.MenuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOptionsAutoLogin, Me.MenuOptionsFilterRawJSONData, Me.MenuOptionsSetupEmailServer, Me.MenuOptionsUseAdvancedAITDecay, Me.MenuOptionsUseLocalTimeZone})
        Me.MenuOptions.Name = "MenuOptions"
        Me.MenuOptions.Size = New System.Drawing.Size(61, 20)
        Me.MenuOptions.Text = "Options"
        '
        'MenuOptionsAutoLogin
        '
        Me.MenuOptionsAutoLogin.CheckOnClick = True
        Me.MenuOptionsAutoLogin.Name = "MenuOptionsAutoLogin"
        Me.MenuOptionsAutoLogin.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsAutoLogin.Text = "Auto Login"
        '
        'MenuOptionsFilterRawJSONData
        '
        Me.MenuOptionsFilterRawJSONData.Checked = True
        Me.MenuOptionsFilterRawJSONData.CheckOnClick = True
        Me.MenuOptionsFilterRawJSONData.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuOptionsFilterRawJSONData.Name = "MenuOptionsFilterRawJSONData"
        Me.MenuOptionsFilterRawJSONData.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsFilterRawJSONData.Text = "Filter Raw JSON Data"
        '
        'MenuOptionsSetupEmailServer
        '
        Me.MenuOptionsSetupEmailServer.Name = "MenuOptionsSetupEmailServer"
        Me.MenuOptionsSetupEmailServer.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsSetupEmailServer.Text = "Setup Email Server"
        Me.MenuOptionsSetupEmailServer.Visible = False
        '
        'MenuOptionsUseAdvancedAITDecay
        '
        Me.MenuOptionsUseAdvancedAITDecay.Checked = True
        Me.MenuOptionsUseAdvancedAITDecay.CheckOnClick = True
        Me.MenuOptionsUseAdvancedAITDecay.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.MenuOptionsUseAdvancedAITDecay.Name = "MenuOptionsUseAdvancedAITDecay"
        Me.MenuOptionsUseAdvancedAITDecay.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsUseAdvancedAITDecay.Text = "Use Advanced AIT Decay"
        '
        'MenuOptionsUseLocalTimeZone
        '
        Me.MenuOptionsUseLocalTimeZone.Checked = True
        Me.MenuOptionsUseLocalTimeZone.CheckOnClick = True
        Me.MenuOptionsUseLocalTimeZone.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.MenuOptionsUseLocalTimeZone.Name = "MenuOptionsUseLocalTimeZone"
        Me.MenuOptionsUseLocalTimeZone.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsUseLocalTimeZone.Text = "Use Local TImeZone"
        '
        'MenuHelp
        '
        Me.MenuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuHelpReportAnIssue, Me.MenuHelpCheckForUpdates, Me.MenuHelpAbout})
        Me.MenuHelp.Name = "MenuHelp"
        Me.MenuHelp.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.MenuHelp.Size = New System.Drawing.Size(44, 20)
        Me.MenuHelp.Text = "&Help"
        '
        'MenuHelpReportAnIssue
        '
        Me.MenuHelpReportAnIssue.Image = Global.CareLink.My.Resources.Resources.FeedbackSmile_16x
        Me.MenuHelpReportAnIssue.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MenuHelpReportAnIssue.Name = "MenuHelpReportAnIssue"
        Me.MenuHelpReportAnIssue.Size = New System.Drawing.Size(177, 22)
        Me.MenuHelpReportAnIssue.Text = "Report A Problem..."
        '
        'MenuHelpCheckForUpdates
        '
        Me.MenuHelpCheckForUpdates.Name = "MenuHelpCheckForUpdates"
        Me.MenuHelpCheckForUpdates.Size = New System.Drawing.Size(177, 22)
        Me.MenuHelpCheckForUpdates.Text = "Check For Updates"
        '
        'MenuHelpAbout
        '
        Me.MenuHelpAbout.Image = Global.CareLink.My.Resources.Resources.AboutBox
        Me.MenuHelpAbout.Name = "MenuHelpAbout"
        Me.MenuHelpAbout.Size = New System.Drawing.Size(177, 22)
        Me.MenuHelpAbout.Text = "&About..."
        '
        'ShowMiniDisplay
        '
        Me.ShowMiniDisplay.Image = Global.CareLink.My.Resources.Resources.ExitFullScreen
        Me.ShowMiniDisplay.Name = "ShowMiniDisplay"
        Me.ShowMiniDisplay.Size = New System.Drawing.Size(105, 20)
        Me.ShowMiniDisplay.Text = "Show Widget"
        Me.ShowMiniDisplay.ToolTipText = "Minimize and show Widget"
        Me.ShowMiniDisplay.Visible = False
        '
        'ServerUpdateTimer
        '
        Me.ServerUpdateTimer.Interval = 300000
        '
        'TabControlHomePage
        '
        Me.TabControlHomePage.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.TabControlHomePage.Controls.Add(Me.TabPage01HomePage)
        Me.TabControlHomePage.Controls.Add(Me.TabPage02RunningIOB)
        Me.TabControlHomePage.Controls.Add(Me.TabPage03TreatmentDetails)
        Me.TabControlHomePage.Controls.Add(Me.TabPage04SummaryData)
        Me.TabControlHomePage.Controls.Add(Me.TabPage05LastSG)
        Me.TabControlHomePage.Controls.Add(Me.TabPage06LastAlarm)
        Me.TabControlHomePage.Controls.Add(Me.TabPage07ActiveInsulin)
        Me.TabControlHomePage.Controls.Add(Me.TabPage06SensorGlucose)
        Me.TabControlHomePage.Controls.Add(Me.TabPage07Limits)
        Me.TabControlHomePage.Controls.Add(Me.TabPage08NotificationHistory)
        Me.TabControlHomePage.Controls.Add(Me.TabPage09Basal)
        Me.TabControlHomePage.Controls.Add(Me.TabPage10TherapyAlgorithm)
        Me.TabControlHomePage.Controls.Add(Me.TabPage11BannerState)
        Me.TabControlHomePage.Controls.Add(Me.TabPage16Markers)
        Me.TabControlHomePage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlHomePage.Location = New System.Drawing.Point(0, 24)
        Me.TabControlHomePage.Name = "TabControlHomePage"
        Me.TabControlHomePage.SelectedIndex = 0
        Me.TabControlHomePage.Size = New System.Drawing.Size(1384, 670)
        Me.TabControlHomePage.TabIndex = 0
        '
        'TabPage01HomePage
        '
        Me.TabPage01HomePage.BackColor = System.Drawing.Color.Black
        Me.TabPage01HomePage.Controls.Add(Me.SplitContainer2)
        Me.TabPage01HomePage.Location = New System.Drawing.Point(4, 27)
        Me.TabPage01HomePage.Name = "TabPage01HomePage"
        Me.TabPage01HomePage.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage01HomePage.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage01HomePage.TabIndex = 7
        Me.TabPage01HomePage.Text = "Home Page"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelTrendValue)
        Me.SplitContainer2.Panel1.Controls.Add(Me.MaxBasalPerHourLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Last24HTotalsPanel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorTimeLeftPanel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelTrendArrows)
        Me.SplitContainer2.Panel1.Controls.Add(Me.LabelSgTrend)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ModelLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SerialNumberLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.FullNameLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ReadingIntervalLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ReadingsLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryRemainingLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TransmatterBatterPercentLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TransmitterBatteryPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.AITAlgorithmLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.RemainingInsulinUnits)
        Me.SplitContainer2.Panel1.Controls.Add(Me.InsulinLevelPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ActiveInsulinValue)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CalibrationDueImage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CursorPanel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CalibrationShieldPanel)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Size = New System.Drawing.Size(1370, 633)
        Me.SplitContainer2.SplitterDistance = 134
        Me.SplitContainer2.TabIndex = 52
        '
        'LabelTrendValue
        '
        Me.LabelTrendValue.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.LabelTrendValue.ForeColor = System.Drawing.Color.White
        Me.LabelTrendValue.Location = New System.Drawing.Point(461, 84)
        Me.LabelTrendValue.Name = "LabelTrendValue"
        Me.LabelTrendValue.Size = New System.Drawing.Size(84, 21)
        Me.LabelTrendValue.TabIndex = 68
        Me.LabelTrendValue.Text = "+ 5"
        Me.LabelTrendValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaxBasalPerHourLabel
        '
        Me.MaxBasalPerHourLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.MaxBasalPerHourLabel.ForeColor = System.Drawing.Color.White
        Me.MaxBasalPerHourLabel.Location = New System.Drawing.Point(1159, 81)
        Me.MaxBasalPerHourLabel.Name = "MaxBasalPerHourLabel"
        Me.MaxBasalPerHourLabel.Size = New System.Drawing.Size(211, 21)
        Me.MaxBasalPerHourLabel.TabIndex = 67
        Me.MaxBasalPerHourLabel.Text = "Max Basal/Hr ~ 2.0 U"
        '
        'Last24HTotalsPanel
        '
        Me.Last24HTotalsPanel.Controls.Add(Me.Last24CarbsValueLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.TotalsLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.AutoCorrectionLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.ManualBolusLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.BasalLabel)
        Me.Last24HTotalsPanel.Controls.Add(Me.DailyDoseLabel)
        Me.Last24HTotalsPanel.Location = New System.Drawing.Point(740, 0)
        Me.Last24HTotalsPanel.Name = "Last24HTotalsPanel"
        Me.Last24HTotalsPanel.Size = New System.Drawing.Size(237, 129)
        Me.Last24HTotalsPanel.TabIndex = 66
        '
        'Last24CarbsValueLabel
        '
        Me.Last24CarbsValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Last24CarbsValueLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Last24CarbsValueLabel.ForeColor = System.Drawing.Color.White
        Me.Last24CarbsValueLabel.Location = New System.Drawing.Point(0, 106)
        Me.Last24CarbsValueLabel.Name = "Last24CarbsValueLabel"
        Me.Last24CarbsValueLabel.Size = New System.Drawing.Size(220, 21)
        Me.Last24CarbsValueLabel.TabIndex = 66
        Me.Last24CarbsValueLabel.Text = "Carbs 100 Grams"
        Me.Last24CarbsValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalsLabel
        '
        Me.TotalsLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TotalsLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalsLabel.ForeColor = System.Drawing.Color.White
        Me.TotalsLabel.Location = New System.Drawing.Point(0, 1)
        Me.TotalsLabel.Name = "TotalsLabel"
        Me.TotalsLabel.Size = New System.Drawing.Size(237, 21)
        Me.TotalsLabel.TabIndex = 65
        Me.TotalsLabel.Text = "Last 24 Hr Totals"
        Me.TotalsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AutoCorrectionLabel
        '
        Me.AutoCorrectionLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AutoCorrectionLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AutoCorrectionLabel.ForeColor = System.Drawing.Color.White
        Me.AutoCorrectionLabel.Location = New System.Drawing.Point(0, 85)
        Me.AutoCorrectionLabel.Name = "AutoCorrectionLabel"
        Me.AutoCorrectionLabel.Size = New System.Drawing.Size(237, 21)
        Me.AutoCorrectionLabel.TabIndex = 64
        Me.AutoCorrectionLabel.Text = "Auto Correction 20 U | 50%"
        Me.AutoCorrectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ManualBolusLabel
        '
        Me.ManualBolusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ManualBolusLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ManualBolusLabel.ForeColor = System.Drawing.Color.White
        Me.ManualBolusLabel.Location = New System.Drawing.Point(0, 64)
        Me.ManualBolusLabel.Name = "ManualBolusLabel"
        Me.ManualBolusLabel.Size = New System.Drawing.Size(237, 21)
        Me.ManualBolusLabel.TabIndex = 63
        Me.ManualBolusLabel.Text = "Manual Bolus 30 U | 30%"
        Me.ManualBolusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BasalLabel
        '
        Me.BasalLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BasalLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BasalLabel.ForeColor = System.Drawing.Color.White
        Me.BasalLabel.Location = New System.Drawing.Point(0, 43)
        Me.BasalLabel.Name = "BasalLabel"
        Me.BasalLabel.Size = New System.Drawing.Size(237, 21)
        Me.BasalLabel.TabIndex = 62
        Me.BasalLabel.Text = "Basal 50 U | 50%"
        Me.BasalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DailyDoseLabel
        '
        Me.DailyDoseLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DailyDoseLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.DailyDoseLabel.ForeColor = System.Drawing.Color.White
        Me.DailyDoseLabel.Location = New System.Drawing.Point(0, 22)
        Me.DailyDoseLabel.Name = "DailyDoseLabel"
        Me.DailyDoseLabel.Size = New System.Drawing.Size(237, 21)
        Me.DailyDoseLabel.TabIndex = 61
        Me.DailyDoseLabel.Text = "Dose 100 U"
        Me.DailyDoseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SensorTimeLeftPanel
        '
        Me.SensorTimeLeftPanel.Controls.Add(Me.SensorDaysLeftLabel)
        Me.SensorTimeLeftPanel.Controls.Add(Me.SensorTimeLeftLabel)
        Me.SensorTimeLeftPanel.Controls.Add(Me.SensorTimeLeftPictureBox)
        Me.SensorTimeLeftPanel.Location = New System.Drawing.Point(638, 0)
        Me.SensorTimeLeftPanel.Name = "SensorTimeLeftPanel"
        Me.SensorTimeLeftPanel.Size = New System.Drawing.Size(94, 129)
        Me.SensorTimeLeftPanel.TabIndex = 65
        '
        'SensorDaysLeftLabel
        '
        Me.SensorDaysLeftLabel.BackColor = System.Drawing.Color.Transparent
        Me.SensorDaysLeftLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorDaysLeftLabel.ForeColor = System.Drawing.Color.White
        Me.SensorDaysLeftLabel.Location = New System.Drawing.Point(17, 16)
        Me.SensorDaysLeftLabel.Name = "SensorDaysLeftLabel"
        Me.SensorDaysLeftLabel.Size = New System.Drawing.Size(37, 52)
        Me.SensorDaysLeftLabel.TabIndex = 45
        Me.SensorDaysLeftLabel.Text = "5"
        Me.SensorDaysLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SensorDaysLeftLabel.Visible = False
        '
        'SensorTimeLeftLabel
        '
        Me.SensorTimeLeftLabel.BackColor = System.Drawing.Color.Transparent
        Me.SensorTimeLeftLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorTimeLeftLabel.ForeColor = System.Drawing.Color.White
        Me.SensorTimeLeftLabel.Location = New System.Drawing.Point(0, 95)
        Me.SensorTimeLeftLabel.Name = "SensorTimeLeftLabel"
        Me.SensorTimeLeftLabel.Size = New System.Drawing.Size(80, 21)
        Me.SensorTimeLeftLabel.TabIndex = 46
        Me.SensorTimeLeftLabel.Text = "???"
        Me.SensorTimeLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SensorTimeLeftPictureBox
        '
        Me.SensorTimeLeftPictureBox.ErrorImage = Nothing
        Me.SensorTimeLeftPictureBox.Image = Global.CareLink.My.Resources.Resources.SensorExpirationUnknown
        Me.SensorTimeLeftPictureBox.Location = New System.Drawing.Point(15, 6)
        Me.SensorTimeLeftPictureBox.Name = "SensorTimeLeftPictureBox"
        Me.SensorTimeLeftPictureBox.Size = New System.Drawing.Size(74, 84)
        Me.SensorTimeLeftPictureBox.TabIndex = 47
        Me.SensorTimeLeftPictureBox.TabStop = False
        '
        'LabelTrendArrows
        '
        Me.LabelTrendArrows.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.LabelTrendArrows.ForeColor = System.Drawing.Color.White
        Me.LabelTrendArrows.Location = New System.Drawing.Point(461, 106)
        Me.LabelTrendArrows.Name = "LabelTrendArrows"
        Me.LabelTrendArrows.Size = New System.Drawing.Size(84, 21)
        Me.LabelTrendArrows.TabIndex = 62
        Me.LabelTrendArrows.Text = "↑↔↓"
        Me.LabelTrendArrows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelSgTrend
        '
        Me.LabelSgTrend.BackColor = System.Drawing.Color.Black
        Me.LabelSgTrend.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.LabelSgTrend.ForeColor = System.Drawing.Color.White
        Me.LabelSgTrend.Location = New System.Drawing.Point(461, 64)
        Me.LabelSgTrend.Name = "LabelSgTrend"
        Me.LabelSgTrend.Size = New System.Drawing.Size(84, 21)
        Me.LabelSgTrend.TabIndex = 61
        Me.LabelSgTrend.Text = "SG Trend"
        Me.LabelSgTrend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ModelLabel
        '
        Me.ModelLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ModelLabel.ForeColor = System.Drawing.Color.White
        Me.ModelLabel.Location = New System.Drawing.Point(1159, 26)
        Me.ModelLabel.Name = "ModelLabel"
        Me.ModelLabel.Size = New System.Drawing.Size(211, 21)
        Me.ModelLabel.TabIndex = 57
        Me.ModelLabel.Text = "Model"
        '
        'SerialNumberLabel
        '
        Me.SerialNumberLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SerialNumberLabel.ForeColor = System.Drawing.Color.White
        Me.SerialNumberLabel.Location = New System.Drawing.Point(1159, 53)
        Me.SerialNumberLabel.Name = "SerialNumberLabel"
        Me.SerialNumberLabel.Size = New System.Drawing.Size(211, 21)
        Me.SerialNumberLabel.TabIndex = 56
        Me.SerialNumberLabel.Text = "Serial Number"
        '
        'FullNameLabel
        '
        Me.FullNameLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.FullNameLabel.ForeColor = System.Drawing.Color.White
        Me.FullNameLabel.Location = New System.Drawing.Point(1159, 0)
        Me.FullNameLabel.Name = "FullNameLabel"
        Me.FullNameLabel.Size = New System.Drawing.Size(211, 21)
        Me.FullNameLabel.TabIndex = 55
        Me.FullNameLabel.Text = "Full Name"
        '
        'ReadingIntervalLabel
        '
        Me.ReadingIntervalLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ReadingIntervalLabel.ForeColor = System.Drawing.Color.White
        Me.ReadingIntervalLabel.Location = New System.Drawing.Point(978, 80)
        Me.ReadingIntervalLabel.Name = "ReadingIntervalLabel"
        Me.ReadingIntervalLabel.Size = New System.Drawing.Size(162, 21)
        Me.ReadingIntervalLabel.TabIndex = 54
        Me.ReadingIntervalLabel.Text = "5 minute readings"
        Me.ReadingIntervalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ReadingsLabel
        '
        Me.ReadingsLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ReadingsLabel.ForeColor = System.Drawing.Color.White
        Me.ReadingsLabel.Location = New System.Drawing.Point(993, 108)
        Me.ReadingsLabel.Name = "ReadingsLabel"
        Me.ReadingsLabel.Size = New System.Drawing.Size(132, 21)
        Me.ReadingsLabel.TabIndex = 53
        Me.ReadingsLabel.Text = "280/288"
        Me.ReadingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PumpBatteryRemainingLabel
        '
        Me.PumpBatteryRemainingLabel.BackColor = System.Drawing.Color.Transparent
        Me.PumpBatteryRemainingLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.PumpBatteryRemainingLabel.ForeColor = System.Drawing.Color.White
        Me.PumpBatteryRemainingLabel.Location = New System.Drawing.Point(121, 86)
        Me.PumpBatteryRemainingLabel.Name = "PumpBatteryRemainingLabel"
        Me.PumpBatteryRemainingLabel.Size = New System.Drawing.Size(80, 21)
        Me.PumpBatteryRemainingLabel.TabIndex = 11
        Me.PumpBatteryRemainingLabel.Text = "???"
        Me.PumpBatteryRemainingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TransmatterBatterPercentLabel
        '
        Me.TransmatterBatterPercentLabel.BackColor = System.Drawing.Color.Transparent
        Me.TransmatterBatterPercentLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TransmatterBatterPercentLabel.ForeColor = System.Drawing.Color.White
        Me.TransmatterBatterPercentLabel.Location = New System.Drawing.Point(551, 95)
        Me.TransmatterBatterPercentLabel.Name = "TransmatterBatterPercentLabel"
        Me.TransmatterBatterPercentLabel.Size = New System.Drawing.Size(80, 21)
        Me.TransmatterBatterPercentLabel.TabIndex = 13
        Me.TransmatterBatterPercentLabel.Text = "???"
        Me.TransmatterBatterPercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TransmitterBatteryPictureBox
        '
        Me.TransmitterBatteryPictureBox.ErrorImage = Nothing
        Me.TransmitterBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.TransmitterBatteryUnknown
        Me.TransmitterBatteryPictureBox.Location = New System.Drawing.Point(554, 0)
        Me.TransmitterBatteryPictureBox.Name = "TransmitterBatteryPictureBox"
        Me.TransmitterBatteryPictureBox.Size = New System.Drawing.Size(74, 84)
        Me.TransmitterBatteryPictureBox.TabIndex = 47
        Me.TransmitterBatteryPictureBox.TabStop = False
        '
        'PumpBatteryPictureBox
        '
        Me.PumpBatteryPictureBox.ErrorImage = Nothing
        Me.PumpBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.PumpBatteryFull
        Me.PumpBatteryPictureBox.Location = New System.Drawing.Point(124, 0)
        Me.PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        Me.PumpBatteryPictureBox.Size = New System.Drawing.Size(74, 84)
        Me.PumpBatteryPictureBox.TabIndex = 43
        Me.PumpBatteryPictureBox.TabStop = False
        '
        'AITAlgorithmLabel
        '
        Me.AITAlgorithmLabel.BackColor = System.Drawing.Color.Transparent
        Me.AITAlgorithmLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AITAlgorithmLabel.ForeColor = System.Drawing.Color.White
        Me.AITAlgorithmLabel.Location = New System.Drawing.Point(972, 3)
        Me.AITAlgorithmLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.AITAlgorithmLabel.Name = "AITAlgorithmLabel"
        Me.AITAlgorithmLabel.Size = New System.Drawing.Size(175, 21)
        Me.AITAlgorithmLabel.TabIndex = 8
        Me.AITAlgorithmLabel.Text = "Active Insulin TIme"
        Me.AITAlgorithmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RemainingInsulinUnits
        '
        Me.RemainingInsulinUnits.BackColor = System.Drawing.Color.Transparent
        Me.RemainingInsulinUnits.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.RemainingInsulinUnits.ForeColor = System.Drawing.Color.White
        Me.RemainingInsulinUnits.Location = New System.Drawing.Point(206, 67)
        Me.RemainingInsulinUnits.Name = "RemainingInsulinUnits"
        Me.RemainingInsulinUnits.Size = New System.Drawing.Size(80, 21)
        Me.RemainingInsulinUnits.TabIndex = 12
        Me.RemainingInsulinUnits.Text = "000.0 U"
        Me.RemainingInsulinUnits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InsulinLevelPictureBox
        '
        Me.InsulinLevelPictureBox.Image = CType(resources.GetObject("InsulinLevelPictureBox.Image"), System.Drawing.Image)
        Me.InsulinLevelPictureBox.InitialImage = Nothing
        Me.InsulinLevelPictureBox.Location = New System.Drawing.Point(221, 0)
        Me.InsulinLevelPictureBox.Name = "InsulinLevelPictureBox"
        Me.InsulinLevelPictureBox.Padding = New System.Windows.Forms.Padding(10)
        Me.InsulinLevelPictureBox.Size = New System.Drawing.Size(51, 67)
        Me.InsulinLevelPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.InsulinLevelPictureBox.TabIndex = 12
        Me.InsulinLevelPictureBox.TabStop = False
        '
        'ActiveInsulinValue
        '
        Me.ActiveInsulinValue.BackColor = System.Drawing.Color.Transparent
        Me.ActiveInsulinValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ActiveInsulinValue.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinValue.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinValue.Location = New System.Drawing.Point(995, 32)
        Me.ActiveInsulinValue.Name = "ActiveInsulinValue"
        Me.ActiveInsulinValue.Size = New System.Drawing.Size(128, 48)
        Me.ActiveInsulinValue.TabIndex = 0
        Me.ActiveInsulinValue.Text = "Active Insulin 0.000 U"
        Me.ActiveInsulinValue.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CalibrationDueImage
        '
        Me.CalibrationDueImage.BackColor = System.Drawing.Color.Transparent
        Me.CalibrationDueImage.Image = Global.CareLink.My.Resources.Resources.CalibrationUnavailable
        Me.CalibrationDueImage.Location = New System.Drawing.Point(480, 7)
        Me.CalibrationDueImage.Name = "CalibrationDueImage"
        Me.CalibrationDueImage.Size = New System.Drawing.Size(47, 47)
        Me.CalibrationDueImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.CalibrationDueImage.TabIndex = 5
        Me.CalibrationDueImage.TabStop = False
        '
        'CursorPanel
        '
        Me.CursorPanel.Controls.Add(Me.CursorPictureBox)
        Me.CursorPanel.Controls.Add(Me.CursorMessage1Label)
        Me.CursorPanel.Controls.Add(Me.CursorMessage2Label)
        Me.CursorPanel.Controls.Add(Me.CursorMessage3Label)
        Me.CursorPanel.Location = New System.Drawing.Point(284, 0)
        Me.CursorPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.CursorPanel.Name = "CursorPanel"
        Me.CursorPanel.Size = New System.Drawing.Size(178, 135)
        Me.CursorPanel.TabIndex = 63
        '
        'CursorPictureBox
        '
        Me.CursorPictureBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CursorPictureBox.Image = CType(resources.GetObject("CursorPictureBox.Image"), System.Drawing.Image)
        Me.CursorPictureBox.InitialImage = Nothing
        Me.CursorPictureBox.Location = New System.Drawing.Point(68, 16)
        Me.CursorPictureBox.Name = "CursorPictureBox"
        Me.CursorPictureBox.Size = New System.Drawing.Size(42, 56)
        Me.CursorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.CursorPictureBox.TabIndex = 42
        Me.CursorPictureBox.TabStop = False
        '
        'CursorMessage1Label
        '
        Me.CursorMessage1Label.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CursorMessage1Label.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage1Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage1Label.Location = New System.Drawing.Point(0, 74)
        Me.CursorMessage1Label.Name = "CursorMessage1Label"
        Me.CursorMessage1Label.Size = New System.Drawing.Size(178, 21)
        Me.CursorMessage1Label.TabIndex = 39
        Me.CursorMessage1Label.Text = "Blood Glucose"
        Me.CursorMessage1Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CursorMessage2Label
        '
        Me.CursorMessage2Label.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CursorMessage2Label.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage2Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage2Label.Location = New System.Drawing.Point(0, 95)
        Me.CursorMessage2Label.Name = "CursorMessage2Label"
        Me.CursorMessage2Label.Size = New System.Drawing.Size(178, 21)
        Me.CursorMessage2Label.TabIndex = 40
        Me.CursorMessage2Label.Text = "Calibration Accepted"
        Me.CursorMessage2Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CursorMessage3Label
        '
        Me.CursorMessage3Label.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.CursorMessage3Label.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage3Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage3Label.Location = New System.Drawing.Point(0, 114)
        Me.CursorMessage3Label.Name = "CursorMessage3Label"
        Me.CursorMessage3Label.Size = New System.Drawing.Size(178, 21)
        Me.CursorMessage3Label.TabIndex = 41
        Me.CursorMessage3Label.Text = "156 ml/dl"
        Me.CursorMessage3Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CalibrationShieldPanel
        '
        Me.CalibrationShieldPanel.Controls.Add(Me.TempTargetLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.ShieldUnitsLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.LastSGTimeLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.CurrentBGLabel)
        Me.CalibrationShieldPanel.Controls.Add(Me.SensorMessage)
        Me.CalibrationShieldPanel.Controls.Add(Me.CalibrationShieldPictureBox)
        Me.CalibrationShieldPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.CalibrationShieldPanel.Location = New System.Drawing.Point(0, 0)
        Me.CalibrationShieldPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.CalibrationShieldPanel.Name = "CalibrationShieldPanel"
        Me.CalibrationShieldPanel.Size = New System.Drawing.Size(116, 134)
        Me.CalibrationShieldPanel.TabIndex = 64
        '
        'TempTargetLabel
        '
        Me.TempTargetLabel.BackColor = System.Drawing.Color.Lime
        Me.TempTargetLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TempTargetLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TempTargetLabel.ForeColor = System.Drawing.Color.Black
        Me.TempTargetLabel.Location = New System.Drawing.Point(0, 0)
        Me.TempTargetLabel.Name = "TempTargetLabel"
        Me.TempTargetLabel.Size = New System.Drawing.Size(116, 21)
        Me.TempTargetLabel.TabIndex = 56
        Me.TempTargetLabel.Text = "Target 150 2:00 Hr"
        Me.TempTargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ShieldUnitsLabel
        '
        Me.ShieldUnitsLabel.AutoSize = True
        Me.ShieldUnitsLabel.BackColor = System.Drawing.Color.Transparent
        Me.ShieldUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ShieldUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.ShieldUnitsLabel.Location = New System.Drawing.Point(38, 76)
        Me.ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        Me.ShieldUnitsLabel.Size = New System.Drawing.Size(40, 13)
        Me.ShieldUnitsLabel.TabIndex = 8
        Me.ShieldUnitsLabel.Text = "XX/XX"
        '
        'LastSGTimeLabel
        '
        Me.LastSGTimeLabel.BackColor = System.Drawing.Color.Transparent
        Me.LastSGTimeLabel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LastSGTimeLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.LastSGTimeLabel.ForeColor = System.Drawing.Color.White
        Me.LastSGTimeLabel.Location = New System.Drawing.Point(0, 113)
        Me.LastSGTimeLabel.Name = "LastSGTimeLabel"
        Me.LastSGTimeLabel.Size = New System.Drawing.Size(116, 21)
        Me.LastSGTimeLabel.TabIndex = 55
        Me.LastSGTimeLabel.Text = "Time"
        Me.LastSGTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CurrentBGLabel
        '
        Me.CurrentBGLabel.BackColor = System.Drawing.Color.Transparent
        Me.CurrentBGLabel.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CurrentBGLabel.ForeColor = System.Drawing.Color.White
        Me.CurrentBGLabel.Location = New System.Drawing.Point(22, 35)
        Me.CurrentBGLabel.Name = "CurrentBGLabel"
        Me.CurrentBGLabel.Size = New System.Drawing.Size(72, 32)
        Me.CurrentBGLabel.TabIndex = 9
        Me.CurrentBGLabel.Text = "---"
        Me.CurrentBGLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CurrentBGLabel.Visible = False
        '
        'SensorMessage
        '
        Me.SensorMessage.BackColor = System.Drawing.Color.Transparent
        Me.SensorMessage.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorMessage.ForeColor = System.Drawing.Color.White
        Me.SensorMessage.Location = New System.Drawing.Point(8, 23)
        Me.SensorMessage.Name = "SensorMessage"
        Me.SensorMessage.Size = New System.Drawing.Size(100, 57)
        Me.SensorMessage.TabIndex = 1
        Me.SensorMessage.Text = "Calibration Required"
        Me.SensorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CalibrationShieldPictureBox
        '
        Me.CalibrationShieldPictureBox.Image = Global.CareLink.My.Resources.Resources.Shield
        Me.CalibrationShieldPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.CalibrationShieldPictureBox.Margin = New System.Windows.Forms.Padding(5)
        Me.CalibrationShieldPictureBox.Name = "CalibrationShieldPictureBox"
        Me.CalibrationShieldPictureBox.Size = New System.Drawing.Size(116, 116)
        Me.CalibrationShieldPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.CalibrationShieldPictureBox.TabIndex = 5
        Me.CalibrationShieldPictureBox.TabStop = False
        '
        'SplitContainer3
        '
        Me.SplitContainer3.IsSplitterFixed = True
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.BackColor = System.Drawing.Color.Black
        '
        'SplitContainer3.Panel2
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
        Me.SplitContainer3.Size = New System.Drawing.Size(1370, 486)
        Me.SplitContainer3.SplitterDistance = 1136
        Me.SplitContainer3.TabIndex = 0
        '
        'Last24HoursLabel
        '
        Me.Last24HoursLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Last24HoursLabel.AutoSize = True
        Me.Last24HoursLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Last24HoursLabel.ForeColor = System.Drawing.Color.White
        Me.Last24HoursLabel.Location = New System.Drawing.Point(61, 29)
        Me.Last24HoursLabel.Name = "Last24HoursLabel"
        Me.Last24HoursLabel.Size = New System.Drawing.Size(109, 21)
        Me.Last24HoursLabel.TabIndex = 34
        Me.Last24HoursLabel.Text = "Last 24 hours"
        '
        'TimeInRangeLabel
        '
        Me.TimeInRangeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TimeInRangeLabel.AutoSize = True
        Me.TimeInRangeLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeLabel.Location = New System.Drawing.Point(58, 7)
        Me.TimeInRangeLabel.Name = "TimeInRangeLabel"
        Me.TimeInRangeLabel.Size = New System.Drawing.Size(115, 21)
        Me.TimeInRangeLabel.TabIndex = 33
        Me.TimeInRangeLabel.Text = "Time in range"
        '
        'TimeInRangeSummaryPercentCharLabel
        '
        Me.TimeInRangeSummaryPercentCharLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TimeInRangeSummaryPercentCharLabel.AutoSize = True
        Me.TimeInRangeSummaryPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.TimeInRangeSummaryPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryPercentCharLabel.Location = New System.Drawing.Point(94, 136)
        Me.TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        Me.TimeInRangeSummaryPercentCharLabel.Size = New System.Drawing.Size(42, 40)
        Me.TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        Me.TimeInRangeSummaryPercentCharLabel.Text = "%"
        '
        'TimeInRangeChartLabel
        '
        Me.TimeInRangeChartLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TimeInRangeChartLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeChartLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeChartLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeChartLabel.Location = New System.Drawing.Point(65, 97)
        Me.TimeInRangeChartLabel.Name = "TimeInRangeChartLabel"
        Me.TimeInRangeChartLabel.Size = New System.Drawing.Size(100, 47)
        Me.TimeInRangeChartLabel.TabIndex = 2
        Me.TimeInRangeChartLabel.Text = "100"
        Me.TimeInRangeChartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InRangeMessageLabel
        '
        Me.InRangeMessageLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.InRangeMessageLabel.AutoSize = True
        Me.InRangeMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.InRangeMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.InRangeMessageLabel.ForeColor = System.Drawing.Color.Lime
        Me.InRangeMessageLabel.Location = New System.Drawing.Point(81, 312)
        Me.InRangeMessageLabel.Name = "InRangeMessageLabel"
        Me.InRangeMessageLabel.Size = New System.Drawing.Size(73, 21)
        Me.InRangeMessageLabel.TabIndex = 30
        Me.InRangeMessageLabel.Text = "In range"
        Me.InRangeMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AboveHighLimitMessageLabel
        '
        Me.AboveHighLimitMessageLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.AboveHighLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AboveHighLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitMessageLabel.ForeColor = System.Drawing.Color.Orange
        Me.AboveHighLimitMessageLabel.Location = New System.Drawing.Point(30, 251)
        Me.AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        Me.AboveHighLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.AboveHighLimitMessageLabel.TabIndex = 28
        Me.AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        Me.AboveHighLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitValueLabel
        '
        Me.BelowLowLimitValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BelowLowLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.BelowLowLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.BelowLowLimitValueLabel.Location = New System.Drawing.Point(55, 340)
        Me.BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        Me.BelowLowLimitValueLabel.Size = New System.Drawing.Size(120, 33)
        Me.BelowLowLimitValueLabel.TabIndex = 26
        Me.BelowLowLimitValueLabel.Text = "2 %"
        Me.BelowLowLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimeInRangeValueLabel
        '
        Me.TimeInRangeValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TimeInRangeValueLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeValueLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeValueLabel.Location = New System.Drawing.Point(55, 279)
        Me.TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        Me.TimeInRangeValueLabel.Size = New System.Drawing.Size(120, 33)
        Me.TimeInRangeValueLabel.TabIndex = 24
        Me.TimeInRangeValueLabel.Text = "90 %"
        Me.TimeInRangeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AboveHighLimitValueLabel
        '
        Me.AboveHighLimitValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.AboveHighLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.AboveHighLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.AboveHighLimitValueLabel.Location = New System.Drawing.Point(55, 218)
        Me.AboveHighLimitValueLabel.Name = "AboveHighLimitValueLabel"
        Me.AboveHighLimitValueLabel.Size = New System.Drawing.Size(120, 33)
        Me.AboveHighLimitValueLabel.TabIndex = 22
        Me.AboveHighLimitValueLabel.Text = "8 %"
        Me.AboveHighLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitMessageLabel
        '
        Me.BelowLowLimitMessageLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BelowLowLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.BelowLowLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitMessageLabel.ForeColor = System.Drawing.Color.Red
        Me.BelowLowLimitMessageLabel.Location = New System.Drawing.Point(30, 373)
        Me.BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        Me.BelowLowLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.BelowLowLimitMessageLabel.TabIndex = 32
        Me.BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        Me.BelowLowLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGValueLabel
        '
        Me.AverageSGValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.AverageSGValueLabel.BackColor = System.Drawing.Color.Black
        Me.AverageSGValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGValueLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGValueLabel.Location = New System.Drawing.Point(55, 401)
        Me.AverageSGValueLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.AverageSGValueLabel.Name = "AverageSGValueLabel"
        Me.AverageSGValueLabel.Size = New System.Drawing.Size(120, 33)
        Me.AverageSGValueLabel.TabIndex = 1
        Me.AverageSGValueLabel.Text = "100 %"
        Me.AverageSGValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGMessageLabel
        '
        Me.AverageSGMessageLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.AverageSGMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGMessageLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGMessageLabel.Location = New System.Drawing.Point(3, 436)
        Me.AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        Me.AverageSGMessageLabel.Size = New System.Drawing.Size(224, 21)
        Me.AverageSGMessageLabel.TabIndex = 0
        Me.AverageSGMessageLabel.Text = "Average SG in XX/XX"
        Me.AverageSGMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage02RunningIOB
        '
        Me.TabPage02RunningIOB.Location = New System.Drawing.Point(4, 27)
        Me.TabPage02RunningIOB.Name = "TabPage02RunningIOB"
        Me.TabPage02RunningIOB.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage02RunningIOB.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage02RunningIOB.TabIndex = 15
        Me.TabPage02RunningIOB.Text = "Running IOB"
        Me.TabPage02RunningIOB.UseVisualStyleBackColor = True
        '
        'TabPage03TreatmentDetails
        '
        Me.TabPage03TreatmentDetails.Location = New System.Drawing.Point(4, 27)
        Me.TabPage03TreatmentDetails.Name = "TabPage03TreatmentDetails"
        Me.TabPage03TreatmentDetails.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage03TreatmentDetails.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage03TreatmentDetails.TabIndex = 8
        Me.TabPage03TreatmentDetails.Text = "Treatment Details"
        Me.TabPage03TreatmentDetails.UseVisualStyleBackColor = True
        '
        'TabPage04SummaryData
        '
        Me.TabPage04SummaryData.Controls.Add(Me.DataGridViewSummary)
        Me.TabPage04SummaryData.Location = New System.Drawing.Point(4, 27)
        Me.TabPage04SummaryData.Name = "TabPage04SummaryData"
        Me.TabPage04SummaryData.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage04SummaryData.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage04SummaryData.TabIndex = 0
        Me.TabPage04SummaryData.Text = "Summary Data"
        Me.TabPage04SummaryData.UseVisualStyleBackColor = True
        '
        'DataGridViewSummary
        '
        Me.DataGridViewSummary.AllowUserToAddRows = False
        Me.DataGridViewSummary.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewSummary.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewSummary.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewSummary.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewSummary.Name = "DataGridViewSummary"
        Me.DataGridViewSummary.ReadOnly = True
        Me.DataGridViewSummary.RowTemplate.Height = 25
        Me.DataGridViewSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridViewSummary.Size = New System.Drawing.Size(1370, 633)
        Me.DataGridViewSummary.TabIndex = 0
        '
        'TabPage05LastSG
        '
        Me.TabPage05LastSG.Controls.Add(Me.TableLayoutPanelLastSG)
        Me.TabPage05LastSG.Location = New System.Drawing.Point(4, 27)
        Me.TabPage05LastSG.Name = "TabPage05LastSG"
        Me.TabPage05LastSG.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage05LastSG.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage05LastSG.TabIndex = 16
        Me.TabPage05LastSG.Text = "Last SG"
        Me.TabPage05LastSG.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelLastSG
        '
        Me.TableLayoutPanelLastSG.AutoSize = True
        Me.TableLayoutPanelLastSG.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastSG.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLastSG.ColumnCount = 1
        Me.TableLayoutPanelLastSG.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLastSG.Controls.Add(Me.LastSGLabel, 0, 0)
        Me.TableLayoutPanelLastSG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelLastSG.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelLastSG.Name = "TableLayoutPanelLastSG"
        Me.TableLayoutPanelLastSG.RowCount = 2
        Me.TableLayoutPanelLastSG.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelLastSG.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLastSG.TabIndex = 1
        '
        'LastSGLabel
        '
        Me.LastSGLabel.AutoSize = True
        Me.LastSGLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LastSGLabel.Location = New System.Drawing.Point(0, 0)
        Me.LastSGLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.LastSGLabel.Name = "LastSGLabel"
        Me.LastSGLabel.Size = New System.Drawing.Size(1358, 30)
        Me.LastSGLabel.TabIndex = 0
        Me.LastSGLabel.Text = "LastSGLabel"
        Me.LastSGLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage06LastAlarm
        '
        Me.TabPage06LastAlarm.Controls.Add(Me.TableLayoutPanelLastAlarm)
        Me.TabPage06LastAlarm.Location = New System.Drawing.Point(4, 27)
        Me.TabPage06LastAlarm.Name = "TabPage06LastAlarm"
        Me.TabPage06LastAlarm.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage06LastAlarm.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage06LastAlarm.TabIndex = 17
        Me.TabPage06LastAlarm.Text = "Last Alarm"
        Me.TabPage06LastAlarm.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelLastAlarm
        '
        Me.TableLayoutPanelLastAlarm.AutoSize = True
        Me.TableLayoutPanelLastAlarm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLastAlarm.CausesValidation = False
        Me.TableLayoutPanelLastAlarm.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLastAlarm.ColumnCount = 1
        Me.TableLayoutPanelLastAlarm.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLastAlarm.Controls.Add(Me.LastAlarmLabel, 0, 0)
        Me.TableLayoutPanelLastAlarm.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TableLayoutPanelLastAlarm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelLastAlarm.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelLastAlarm.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanelLastAlarm.Name = "TableLayoutPanelLastAlarm"
        Me.TableLayoutPanelLastAlarm.RowCount = 2
        Me.TableLayoutPanelLastAlarm.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelLastAlarm.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLastAlarm.TabIndex = 2
        '
        'LastAlarmLabel
        '
        Me.LastAlarmLabel.AutoSize = True
        Me.LastAlarmLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LastAlarmLabel.Location = New System.Drawing.Point(0, 0)
        Me.LastAlarmLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.LastAlarmLabel.Name = "LastAlarmLabel"
        Me.LastAlarmLabel.Size = New System.Drawing.Size(1358, 30)
        Me.LastAlarmLabel.TabIndex = 0
        Me.LastAlarmLabel.Text = "Last Alarm"
        Me.LastAlarmLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage07ActiveInsulin
        '
        Me.TabPage07ActiveInsulin.Controls.Add(Me.TableLayoutPanelActiveInsulin)
        Me.TabPage07ActiveInsulin.Location = New System.Drawing.Point(4, 27)
        Me.TabPage07ActiveInsulin.Name = "TabPage07ActiveInsulin"
        Me.TabPage07ActiveInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage07ActiveInsulin.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage07ActiveInsulin.TabIndex = 1
        Me.TabPage07ActiveInsulin.Text = "Active Insulin"
        Me.TabPage07ActiveInsulin.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelActiveInsulin
        '
        Me.TableLayoutPanelActiveInsulin.AutoScroll = True
        Me.TableLayoutPanelActiveInsulin.AutoSize = True
        Me.TableLayoutPanelActiveInsulin.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelActiveInsulin.ColumnCount = 1
        Me.TableLayoutPanelActiveInsulin.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelActiveInsulin.Controls.Add(Me.ActiveInsulinTabLabel, 0, 0)
        Me.TableLayoutPanelActiveInsulin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelActiveInsulin.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelActiveInsulin.Name = "TableLayoutPanelActiveInsulin"
        Me.TableLayoutPanelActiveInsulin.RowCount = 2
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelActiveInsulin.Size = New System.Drawing.Size(1370, 39)
        Me.TableLayoutPanelActiveInsulin.TabIndex = 0
        '
        'ActiveInsulinTabLabel
        '
        Me.ActiveInsulinTabLabel.AutoSize = True
        Me.ActiveInsulinTabLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ActiveInsulinTabLabel.Location = New System.Drawing.Point(0, 0)
        Me.ActiveInsulinTabLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.ActiveInsulinTabLabel.Name = "ActiveInsulinTabLabel"
        Me.ActiveInsulinTabLabel.Size = New System.Drawing.Size(1358, 30)
        Me.ActiveInsulinTabLabel.Text = "Active Insulin"
        Me.ActiveInsulinTabLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage06SensorGlucose
        '
        Me.TabPage06SensorGlucose.Controls.Add(Me.TableLayoutPanelSgs)
        Me.TabPage06SensorGlucose.Location = New System.Drawing.Point(4, 27)
        Me.TabPage06SensorGlucose.Name = "TabPage06SensorGlucose"
        Me.TabPage06SensorGlucose.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage06SensorGlucose.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage06SensorGlucose.TabIndex = 2
        Me.TabPage06SensorGlucose.Text = "Sensor Glucose"
        Me.TabPage06SensorGlucose.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelSgs
        '
        Me.TableLayoutPanelSgs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelSgs.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelSgs.ColumnCount = 1
        Me.TableLayoutPanelSgs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelSgs.Controls.Add(Me.LabelSgs, 0, 0)
        Me.TableLayoutPanelSgs.Controls.Add(Me.DataGridViewSGs, 0, 1)
        Me.TableLayoutPanelSgs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelSgs.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelSgs.Name = "TableLayoutPanelSgs"
        Me.TableLayoutPanelSgs.RowCount = 2
        Me.TableLayoutPanelSgs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelSgs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelSgs.Size = New System.Drawing.Size(1370, 621)
        Me.TableLayoutPanelSgs.TabIndex = 2
        '
        'LabelSgs
        '
        Me.LabelSgs.AutoSize = True
        Me.LabelSgs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelSgs.Location = New System.Drawing.Point(0, 0)
        Me.LabelSgs.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelSgs.Name = "LabelSgs"
        Me.LabelSgs.Size = New System.Drawing.Size(1358, 30)
        Me.LabelSgs.Text = "Sgs"
        Me.LabelSgs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewSGs
        '
        Me.DataGridViewSGs.AllowUserToAddRows = False
        Me.DataGridViewSGs.AllowUserToDeleteRows = False
        Me.DataGridViewSGs.AllowUserToResizeColumns = False
        Me.DataGridViewSGs.AllowUserToResizeRows = False
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewSGs.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewSGs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewSGs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewSGs.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewSGs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridViewSGs.Location = New System.Drawing.Point(3, 33)
        Me.DataGridViewSGs.Name = "DataGridViewSGs"
        Me.DataGridViewSGs.RowTemplate.Height = 25
        Me.DataGridViewSGs.Size = New System.Drawing.Size(1364, 597)
        Me.DataGridViewSGs.TabIndex = 1
        '
        'TabPage07Limits
        '
        Me.TabPage07Limits.Controls.Add(Me.TableLayoutPanelLimits)
        Me.TabPage07Limits.Location = New System.Drawing.Point(4, 27)
        Me.TabPage07Limits.Name = "TabPage07Limits"
        Me.TabPage07Limits.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage07Limits.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage07Limits.TabIndex = 3
        Me.TabPage07Limits.Text = "Limits"
        Me.TabPage07Limits.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelLimits
        '
        Me.TableLayoutPanelLimits.AutoScroll = True
        Me.TableLayoutPanelLimits.AutoSize = True
        Me.TableLayoutPanelLimits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLimits.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLimits.ColumnCount = 1
        Me.TableLayoutPanelLimits.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLimits.Controls.Add(Me.LabelLimits, 0, 0)
        Me.TableLayoutPanelLimits.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelLimits.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelLimits.Name = "TableLayoutPanelLimits"
        Me.TableLayoutPanelLimits.RowCount = 2
        Me.TableLayoutPanelLimits.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelLimits.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLimits.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelLimits.TabIndex = 0
        '
        'LabelLimits
        '
        Me.LabelLimits.AutoSize = True
        Me.LabelLimits.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelLimits.Location = New System.Drawing.Point(0, 0)
        Me.LabelLimits.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelLimits.Name = "LabelLimits"
        Me.LabelLimits.Size = New System.Drawing.Size(1358, 30)
        Me.LabelLimits.TabIndex = 0
        Me.LabelLimits.Text = "Limits"
        Me.LabelLimits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage08NotificationHistory
        '
        Me.TabPage08NotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistory)
        Me.TabPage08NotificationHistory.Location = New System.Drawing.Point(4, 27)
        Me.TabPage08NotificationHistory.Name = "TabPage08NotificationHistory"
        Me.TabPage08NotificationHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage08NotificationHistory.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage08NotificationHistory.TabIndex = 5
        Me.TabPage08NotificationHistory.Text = "Notification History"
        Me.TabPage08NotificationHistory.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelNotificationHistory
        '
        Me.TableLayoutPanelNotificationHistory.AutoScroll = True
        Me.TableLayoutPanelNotificationHistory.AutoSize = True
        Me.TableLayoutPanelNotificationHistory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelNotificationHistory.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelNotificationHistory.ColumnCount = 1
        Me.TableLayoutPanelNotificationHistory.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelNotificationHistory.Controls.Add(Me.LabelNotificationHistory, 0, 0)
        Me.TableLayoutPanelNotificationHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelNotificationHistory.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelNotificationHistory.Name = "TableLayoutPanelNotificationHistory"
        Me.TableLayoutPanelNotificationHistory.RowCount = 2
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelNotificationHistory.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelNotificationHistory.TabIndex = 0
        '
        'LabelNotificationHistory
        '
        Me.LabelNotificationHistory.AutoSize = True
        Me.LabelNotificationHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelNotificationHistory.Location = New System.Drawing.Point(0, 0)
        Me.LabelNotificationHistory.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelNotificationHistory.Name = "LabelNotificationHistory"
        Me.LabelNotificationHistory.Size = New System.Drawing.Size(1358, 30)
        Me.LabelNotificationHistory.TabIndex = 0
        Me.LabelNotificationHistory.Text = "Notification History"
        Me.LabelNotificationHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage09Basal
        '
        Me.TabPage09Basal.Controls.Add(Me.TableLayoutPanelBasal)
        Me.TabPage09Basal.Location = New System.Drawing.Point(4, 27)
        Me.TabPage09Basal.Name = "TabPage09Basal"
        Me.TabPage09Basal.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage09Basal.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage09Basal.TabIndex = 6
        Me.TabPage09Basal.Text = "Basal"
        Me.TabPage09Basal.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelBasal
        '
        Me.TableLayoutPanelBasal.AutoScroll = True
        Me.TableLayoutPanelBasal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBasal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBasal.ColumnCount = 1
        Me.TableLayoutPanelBasal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBasal.Controls.Add(Me.LabelBasal, 0, 0)
        Me.TableLayoutPanelBasal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelBasal.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelBasal.Name = "TableLayoutPanelBasal"
        Me.TableLayoutPanelBasal.RowCount = 2
        Me.TableLayoutPanelBasal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelBasal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBasal.TabIndex = 0
        '
        'LabelBasal
        '
        Me.LabelBasal.AutoSize = True
        Me.LabelBasal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelBasal.Location = New System.Drawing.Point(0, 0)
        Me.LabelLimits.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelBasal.Name = "LabelBasal"
        Me.LabelBasal.Size = New System.Drawing.Size(1358, 30)
        Me.LabelBasal.TabIndex = 0
        Me.LabelBasal.Text = "Basal"
        Me.LabelBasal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage10TherapyAlgorithm
        '
        Me.TabPage10TherapyAlgorithm.Controls.Add(Me.TableLayoutPanelTherapyAlgorthm)
        Me.TabPage10TherapyAlgorithm.Location = New System.Drawing.Point(4, 27)
        Me.TabPage10TherapyAlgorithm.Name = "TabPage10TherapyAlgorithm"
        Me.TabPage10TherapyAlgorithm.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage10TherapyAlgorithm.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage10TherapyAlgorithm.TabIndex = 9
        Me.TabPage10TherapyAlgorithm.Text = "Therapy Algorithm"
        Me.TabPage10TherapyAlgorithm.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelTherapyAlgorthm
        '
        Me.TableLayoutPanelTherapyAlgorthm.AutoSize = True
        Me.TableLayoutPanelTherapyAlgorthm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTherapyAlgorthm.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTherapyAlgorthm.ColumnCount = 1
        Me.TableLayoutPanelTherapyAlgorthm.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelTherapyAlgorthm.Controls.Add(Me.LabelTherapyAlgorthm, 0, 0)
        Me.TableLayoutPanelTherapyAlgorthm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelTherapyAlgorthm.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelTherapyAlgorthm.Name = "TableLayoutPanelTherapyAlgorthm"
        Me.TableLayoutPanelTherapyAlgorthm.RowCount = 2
        Me.TableLayoutPanelTherapyAlgorthm.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelTherapyAlgorthm.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelTherapyAlgorthm.TabIndex = 0
        '
        'LabelTherapyAlgorthm
        '
        Me.LabelTherapyAlgorthm.AutoSize = True
        Me.LabelTherapyAlgorthm.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTherapyAlgorthm.Location = New System.Drawing.Point(0, 0)
        Me.LabelTherapyAlgorthm.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelTherapyAlgorthm.Name = "LabelTherapyAlgorthm"
        Me.LabelTherapyAlgorthm.Size = New System.Drawing.Size(1358, 30)
        Me.LabelTherapyAlgorthm.TabIndex = 0
        Me.LabelTherapyAlgorthm.Text = "Therapy Algorthm"
        Me.LabelTherapyAlgorthm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage11BannerState
        '
        Me.TabPage11BannerState.Controls.Add(Me.TableLayoutPanelBannerState)
        Me.TabPage11BannerState.Location = New System.Drawing.Point(4, 27)
        Me.TabPage11BannerState.Name = "TabPage11BannerState"
        Me.TabPage11BannerState.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage11BannerState.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage11BannerState.TabIndex = 10
        Me.TabPage11BannerState.Text = "Banner State"
        Me.TabPage11BannerState.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelBannerState
        '
        Me.TableLayoutPanelBannerState.AutoSize = True
        Me.TableLayoutPanelBannerState.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBannerState.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBannerState.ColumnCount = 1
        Me.TableLayoutPanelBannerState.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBannerState.Controls.Add(Me.LabelBannerState, 0, 0)
        Me.TableLayoutPanelBannerState.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelBannerState.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelBannerState.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanelBannerState.Name = "TableLayoutPanelBannerState"
        Me.TableLayoutPanelBannerState.RowCount = 2
        Me.TableLayoutPanelBannerState.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelBannerState.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBannerState.TabIndex = 0
        '
        'LabelBannerState
        '
        Me.LabelBannerState.AutoSize = True
        Me.LabelBannerState.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelBannerState.Location = New System.Drawing.Point(0, 0)
        Me.LabelBannerState.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelBannerState.Name = "LabelBannerState"
        Me.LabelBannerState.Size = New System.Drawing.Size(1358, 30)
        Me.LabelBannerState.TabIndex = 0
        Me.LabelBannerState.Text = "Banner State"
        Me.LabelBannerState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage16Markers
        '
        Me.TabPage16Markers.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.TabPage16Markers.Location = New System.Drawing.Point(4, 27)
        Me.TabPage16Markers.Name = "TabPage16Markers"
        Me.TabPage16Markers.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage16Markers.Size = New System.Drawing.Size(1376, 639)
        Me.TabPage16Markers.TabIndex = 4
        Me.TabPage16Markers.Text = "Markers and More..."
        '
        'TabControlPage2
        '
        Me.TabControlPage2.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.TabControlPage2.Controls.Add(Me.TabPageAutoBasalDelivery)
        Me.TabControlPage2.Controls.Add(Me.TabPageAutoModeStatus)
        Me.TabControlPage2.Controls.Add(Me.TabPageBgReadings)
        Me.TabControlPage2.Controls.Add(Me.TabPageCalibration)
        Me.TabControlPage2.Controls.Add(Me.TabPageInsulin)
        Me.TabControlPage2.Controls.Add(Me.TabPageLowGlusoseSuspended)
        Me.TabControlPage2.Controls.Add(Me.TabPageMeal)
        Me.TabControlPage2.Controls.Add(Me.TabPageTimeChange)
        Me.TabControlPage2.Controls.Add(Me.TabPageCountryDataPg1)
        Me.TabControlPage2.Controls.Add(Me.TabPageCountryDataPg2)
        Me.TabControlPage2.Controls.Add(Me.TabPageCountryDataPg3)
        Me.TabControlPage2.Controls.Add(Me.TabPageUserProfile)
        Me.TabControlPage2.Controls.Add(Me.TabPageCurrentUser)
        Me.TabControlPage2.Controls.Add(Me.TabPageAllUsers)
        Me.TabControlPage2.Controls.Add(Me.TabPageBackToHomePage)
        Me.TabControlPage2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlPage2.Location = New System.Drawing.Point(0, 24)
        Me.TabControlPage2.Name = "TabControlPage2"
        Me.TabControlPage2.SelectedIndex = 0
        Me.TabControlPage2.Size = New System.Drawing.Size(1384, 670)
        Me.TabControlPage2.TabIndex = 0
        '
        'TabPageAutoBasalDelivery
        '
        Me.TabPageAutoBasalDelivery.Controls.Add(Me.TableLayoutPanelAutoBasalDelivery)
        Me.TabPageAutoBasalDelivery.Location = New System.Drawing.Point(4, 27)
        Me.TabPageAutoBasalDelivery.Name = "TabPageAutoBasalDelivery"
        Me.TabPageAutoBasalDelivery.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageAutoBasalDelivery.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageAutoBasalDelivery.TabIndex = 1
        Me.TabPageAutoBasalDelivery.Text = "Auto Basal Delivery"
        Me.TabPageAutoBasalDelivery.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelAutoBasalDelivery
        '
        Me.TableLayoutPanelAutoBasalDelivery.AutoSize = True
        Me.TableLayoutPanelAutoBasalDelivery.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoBasalDelivery.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoBasalDelivery.ColumnCount = 1
        Me.TableLayoutPanelAutoBasalDelivery.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelAutoBasalDelivery.Controls.Add(Me.LabelAutoBasalDelivery, 0, 0)
        Me.TableLayoutPanelAutoBasalDelivery.Controls.Add(Me.DataGridViewAutoBasalDelivery, 0, 1)
        Me.TableLayoutPanelAutoBasalDelivery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAutoBasalDelivery.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelAutoBasalDelivery.Name = "TableLayoutPanelAutoBasalDelivery"
        Me.TableLayoutPanelAutoBasalDelivery.RowCount = 2
        Me.TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelAutoBasalDelivery.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelAutoBasalDelivery.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelAutoBasalDelivery.TabIndex = 0
        '
        'LabelAutoBasalDelivery
        '
        Me.LabelAutoBasalDelivery.AutoSize = True
        Me.LabelAutoBasalDelivery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelAutoBasalDelivery.Location = New System.Drawing.Point(0, 0)
        Me.LabelAutoBasalDelivery.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelAutoBasalDelivery.Name = "LabelAutoBasalDelivery"
        Me.LabelAutoBasalDelivery.Size = New System.Drawing.Size(1358, 30)
        Me.LabelAutoBasalDelivery.TabIndex = 1
        Me.LabelAutoBasalDelivery.Text = "Auto Basal Delivery"
        Me.LabelAutoBasalDelivery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewAutoBasalDelivery
        '
        Me.DataGridViewAutoBasalDelivery.AllowUserToAddRows = False
        Me.DataGridViewAutoBasalDelivery.AllowUserToDeleteRows = False
        Me.DataGridViewAutoBasalDelivery.AllowUserToResizeColumns = False
        Me.DataGridViewAutoBasalDelivery.AllowUserToResizeRows = False
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewAutoBasalDelivery.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewAutoBasalDelivery.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewAutoBasalDelivery.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewAutoBasalDelivery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewAutoBasalDelivery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewAutoBasalDelivery.Location = New System.Drawing.Point(3, 33)
        Me.DataGridViewAutoBasalDelivery.Name = "DataGridViewAutoBasalDelivery"
        Me.DataGridViewAutoBasalDelivery.ReadOnly = True
        Me.DataGridViewAutoBasalDelivery.RowTemplate.Height = 25
        Me.DataGridViewAutoBasalDelivery.Size = New System.Drawing.Size(1364, 597)
        Me.DataGridViewAutoBasalDelivery.TabIndex = 0
        '
        'TabPageAutoModeStatus
        '
        Me.TabPageAutoModeStatus.Controls.Add(Me.TableLayoutPanelAutoModeStatus)
        Me.TabPageAutoModeStatus.Location = New System.Drawing.Point(4, 27)
        Me.TabPageAutoModeStatus.Name = "TabPageAutoModeStatus"
        Me.TabPageAutoModeStatus.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageAutoModeStatus.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageAutoModeStatus.TabIndex = 0
        Me.TabPageAutoModeStatus.Text = "Auto Mode Status"
        Me.TabPageAutoModeStatus.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelAutoModeStatus
        '
        Me.TableLayoutPanelAutoModeStatus.AutoScroll = True
        Me.TableLayoutPanelAutoModeStatus.AutoSize = True
        Me.TableLayoutPanelAutoModeStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelAutoModeStatus.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelAutoModeStatus.ColumnCount = 1
        Me.TableLayoutPanelAutoModeStatus.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelAutoModeStatus.Controls.Add(Me.LabelAutoModeStatus, 0, 0)
        Me.TableLayoutPanelAutoModeStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelAutoModeStatus.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelAutoModeStatus.Name = "TableLayoutPanelAutoModeStatus"
        Me.TableLayoutPanelAutoModeStatus.RowCount = 2
        Me.TableLayoutPanelAutoModeStatus.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelAutoModeStatus.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelAutoModeStatus.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelAutoModeStatus.TabIndex = 0
        '
        'LabelAutoModeStatus
        '
        Me.LabelAutoModeStatus.AutoSize = True
        Me.LabelAutoModeStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelAutoModeStatus.Location = New System.Drawing.Point(0, 0)
        Me.LabelAutoModeStatus.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelAutoModeStatus.Name = "LabelAutoModeStatus"
        Me.LabelAutoModeStatus.Size = New System.Drawing.Size(1358, 30)
        Me.LabelAutoModeStatus.TabIndex = 0
        Me.LabelAutoModeStatus.Text = "Auto Mode Status"
        Me.LabelAutoModeStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageBgReadings
        '
        Me.TabPageBgReadings.Controls.Add(Me.TableLayoutPanelBgReadings)
        Me.TabPageBgReadings.Location = New System.Drawing.Point(4, 27)
        Me.TabPageBgReadings.Name = "TabPageBgReadings"
        Me.TabPageBgReadings.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageBgReadings.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageBgReadings.TabIndex = 2
        Me.TabPageBgReadings.Text = "BG Readings"
        Me.TabPageBgReadings.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelBgReadings
        '
        Me.TableLayoutPanelBgReadings.AutoScroll = True
        Me.TableLayoutPanelBgReadings.AutoSize = True
        Me.TableLayoutPanelBgReadings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBgReadings.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBgReadings.ColumnCount = 1
        Me.TableLayoutPanelBgReadings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBgReadings.Controls.Add(Me.LabelBgReading, 0, 0)
        Me.TableLayoutPanelBgReadings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelBgReadings.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelBgReadings.Name = "TableLayoutPanelBgReadings"
        Me.TableLayoutPanelBgReadings.RowCount = 2
        Me.TableLayoutPanelBgReadings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelBgReadings.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBgReadings.Size = New System.Drawing.Size(1370, 39)
        Me.TableLayoutPanelBgReadings.TabIndex = 1
        '
        'LabelBgReading
        '
        Me.LabelBgReading.AutoSize = True
        Me.LabelBgReading.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelBgReading.Location = New System.Drawing.Point(0, 0)
        Me.LabelBgReading.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelBgReading.Name = "LabelBgReading"
        Me.LabelBgReading.Size = New System.Drawing.Size(1358, 30)
        Me.LabelBgReading.TabIndex = 0
        Me.LabelBgReading.Text = "Bg Reading"
        Me.LabelBgReading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageCalibration
        '
        Me.TabPageCalibration.Controls.Add(Me.TableLayoutPanelCalibration)
        Me.TabPageCalibration.Location = New System.Drawing.Point(4, 27)
        Me.TabPageCalibration.Name = "TabPageCalibration"
        Me.TabPageCalibration.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageCalibration.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageCalibration.TabIndex = 3
        Me.TabPageCalibration.Text = "Calibration"
        Me.TabPageCalibration.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelCalibration
        '
        Me.TableLayoutPanelCalibration.AutoScroll = True
        Me.TableLayoutPanelCalibration.AutoSize = True
        Me.TableLayoutPanelCalibration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelCalibration.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelCalibration.ColumnCount = 1
        Me.TableLayoutPanelCalibration.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelCalibration.Controls.Add(Me.LabelCalibration, 0, 0)
        Me.TableLayoutPanelCalibration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelCalibration.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelCalibration.Name = "TableLayoutPanelCalibration"
        Me.TableLayoutPanelCalibration.RowCount = 2
        Me.TableLayoutPanelCalibration.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelCalibration.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelCalibration.TabIndex = 1
        '
        'LabelCalibration
        '
        Me.LabelCalibration.AutoSize = True
        Me.LabelCalibration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelCalibration.Location = New System.Drawing.Point(0, 0)
        Me.LabelCalibration.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelCalibration.Name = "LabelCalibration"
        Me.LabelCalibration.Size = New System.Drawing.Size(1358, 30)
        Me.LabelCalibration.TabIndex = 0
        Me.LabelCalibration.Text = "Calibration"
        Me.LabelCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageInsulin
        '
        Me.TabPageInsulin.Controls.Add(Me.TableLayoutPanelInsulin)
        Me.TabPageInsulin.Location = New System.Drawing.Point(4, 27)
        Me.TabPageInsulin.Name = "TabPageInsulin"
        Me.TabPageInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageInsulin.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageInsulin.TabIndex = 4
        Me.TabPageInsulin.Text = "Insulin"
        Me.TabPageInsulin.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelInsulin
        '
        Me.TableLayoutPanelInsulin.AutoScroll = True
        Me.TableLayoutPanelInsulin.AutoSize = True
        Me.TableLayoutPanelInsulin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelInsulin.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelInsulin.ColumnCount = 1
        Me.TableLayoutPanelInsulin.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelInsulin.Controls.Add(Me.LabelInsulin, 0, 0)
        Me.TableLayoutPanelInsulin.Controls.Add(Me.DataGridViewInsulin, 0, 1)
        Me.TableLayoutPanelInsulin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelInsulin.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelInsulin.Name = "TableLayoutPanelInsulin"
        Me.TableLayoutPanelInsulin.RowCount = 2
        Me.TableLayoutPanelInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelInsulin.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelInsulin.TabIndex = 1
        '
        'LabelInsulin
        '
        Me.LabelInsulin.AutoSize = True
        Me.LabelInsulin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelInsulin.Location = New System.Drawing.Point(0, 0)
        Me.LabelInsulin.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelInsulin.Name = "LabelInsulin"
        Me.LabelInsulin.Size = New System.Drawing.Size(1358, 30)
        Me.LabelInsulin.TabIndex = 1
        Me.LabelInsulin.Text = "Insulin"
        Me.LabelInsulin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewInsulin
        '
        Me.DataGridViewInsulin.AllowUserToAddRows = False
        Me.DataGridViewInsulin.AllowUserToDeleteRows = False
        Me.DataGridViewInsulin.AllowUserToResizeColumns = False
        Me.DataGridViewInsulin.AllowUserToResizeRows = False
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewInsulin.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle7
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewInsulin.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewInsulin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewInsulin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewInsulin.Location = New System.Drawing.Point(6, 39)
        Me.DataGridViewInsulin.Name = "DataGridViewInsulin"
        Me.DataGridViewInsulin.ReadOnly = True
        Me.DataGridViewInsulin.RowTemplate.Height = 25
        Me.DataGridViewInsulin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridViewInsulin.TabIndex = 0
        '
        'TabPageLowGlusoseSuspended
        '
        Me.TabPageLowGlusoseSuspended.Controls.Add(Me.TableLayoutPanelLowGlusoseSuspended)
        Me.TabPageLowGlusoseSuspended.Location = New System.Drawing.Point(4, 27)
        Me.TabPageLowGlusoseSuspended.Name = "TabPageLowGlusoseSuspended"
        Me.TabPageLowGlusoseSuspended.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageLowGlusoseSuspended.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageLowGlusoseSuspended.TabIndex = 5
        Me.TabPageLowGlusoseSuspended.Text = "Low Glusose Suspended"
        Me.TabPageLowGlusoseSuspended.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelLowGlusoseSuspended
        '
        Me.TableLayoutPanelLowGlusoseSuspended.AutoScroll = True
        Me.TableLayoutPanelLowGlusoseSuspended.AutoSize = True
        Me.TableLayoutPanelLowGlusoseSuspended.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLowGlusoseSuspended.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLowGlusoseSuspended.ColumnCount = 1
        Me.TableLayoutPanelLowGlusoseSuspended.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLowGlusoseSuspended.Controls.Add(Me.LabelLowGlusoseSuspended, 0, 0)
        Me.TableLayoutPanelLowGlusoseSuspended.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelLowGlusoseSuspended.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelLowGlusoseSuspended.Name = "TableLayoutPanelLowGlusoseSuspended"
        Me.TableLayoutPanelLowGlusoseSuspended.RowCount = 2
        Me.TableLayoutPanelLowGlusoseSuspended.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelLowGlusoseSuspended.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLowGlusoseSuspended.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelLowGlusoseSuspended.TabIndex = 1
        '
        'LabelLowGlusoseSuspended
        '
        Me.LabelLowGlusoseSuspended.AutoSize = True
        Me.LabelLowGlusoseSuspended.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelLowGlusoseSuspended.Location = New System.Drawing.Point(0, 0)
        Me.LabelLowGlusoseSuspended.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelLowGlusoseSuspended.Name = "LabelLowGlusoseSuspended"
        Me.LabelLowGlusoseSuspended.Size = New System.Drawing.Size(1358, 30)
        Me.LabelLowGlusoseSuspended.TabIndex = 0
        Me.LabelLowGlusoseSuspended.Text = "Low Glusose Suspended"
        Me.LabelLowGlusoseSuspended.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageMeal
        '
        Me.TabPageMeal.Controls.Add(Me.TableLayoutPanelMeal)
        Me.TabPageMeal.Location = New System.Drawing.Point(4, 27)
        Me.TabPageMeal.Name = "TabPageMeal"
        Me.TabPageMeal.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageMeal.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageMeal.TabIndex = 6
        Me.TabPageMeal.Text = "Meal"
        Me.TabPageMeal.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelMeal
        '
        Me.TableLayoutPanelMeal.AutoScroll = True
        Me.TableLayoutPanelMeal.AutoSize = True
        Me.TableLayoutPanelMeal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelMeal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelMeal.ColumnCount = 1
        Me.TableLayoutPanelMeal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMeal.Controls.Add(Me.LabelMeal, 0, 0)
        Me.TableLayoutPanelMeal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMeal.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelMeal.Name = "TableLayoutPanelMeal"
        Me.TableLayoutPanelMeal.RowCount = 2
        Me.TableLayoutPanelMeal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelMeal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMeal.Size = New System.Drawing.Size(1370, 633)
        Me.TableLayoutPanelMeal.TabIndex = 1
        '
        'LabelMeal
        '
        Me.LabelMeal.AutoSize = True
        Me.LabelMeal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelMeal.Location = New System.Drawing.Point(0, 0)
        Me.LabelMeal.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelMeal.Name = "LabelMeal"
        Me.LabelMeal.Size = New System.Drawing.Size(1358, 30)
        Me.LabelMeal.TabIndex = 0
        Me.LabelMeal.Text = "Meal"
        Me.LabelMeal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageTimeChange
        '
        Me.TabPageTimeChange.Controls.Add(Me.TableLayoutPanelTimeChange)
        Me.TabPageTimeChange.Location = New System.Drawing.Point(4, 27)
        Me.TabPageTimeChange.Name = "TabPageTimeChange"
        Me.TabPageTimeChange.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageTimeChange.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageTimeChange.TabIndex = 7
        Me.TabPageTimeChange.Text = "Time Change"
        Me.TabPageTimeChange.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelTimeChange
        '
        Me.TableLayoutPanelTimeChange.AutoScroll = True
        Me.TableLayoutPanelTimeChange.AutoSize = True
        Me.TableLayoutPanelTimeChange.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTimeChange.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTimeChange.ColumnCount = 1
        Me.TableLayoutPanelTimeChange.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelTimeChange.Controls.Add(Me.LabelTimeChange, 0, 0)
        Me.TableLayoutPanelTimeChange.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelTimeChange.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelTimeChange.Name = "TableLayoutPanelTimeChange"
        Me.TableLayoutPanelTimeChange.RowCount = 2
        Me.TableLayoutPanelTimeChange.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.TableLayoutPanelTimeChange.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelTimeChange.TabIndex = 1
        '
        'LabelTimeChange
        '
        Me.LabelTimeChange.AutoSize = True
        Me.LabelTimeChange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTimeChange.Location = New System.Drawing.Point(0, 0)
        Me.LabelTimeChange.Margin = New System.Windows.Forms.Padding(3)
        Me.LabelTimeChange.Name = "LabelTimeChange"
        Me.LabelTimeChange.Size = New System.Drawing.Size(1358, 30)
        Me.LabelTimeChange.TabIndex = 0
        Me.LabelTimeChange.Text = "Tme Change"
        Me.LabelTimeChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPageCountryDataPg1
        '
        Me.TabPageCountryDataPg1.Controls.Add(Me.DataGridViewCountryItemsPage1)
        Me.TabPageCountryDataPg1.Location = New System.Drawing.Point(4, 27)
        Me.TabPageCountryDataPg1.Name = "TabPageCountryDataPg1"
        Me.TabPageCountryDataPg1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageCountryDataPg1.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageCountryDataPg1.TabIndex = 11
        Me.TabPageCountryDataPg1.Text = "Country Data Pg1"
        Me.TabPageCountryDataPg1.UseVisualStyleBackColor = True
        '
        'DataGridViewCountryItemsPage1
        '
        Me.DataGridViewCountryItemsPage1.AllowUserToAddRows = False
        Me.DataGridViewCountryItemsPage1.AllowUserToDeleteRows = False
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewCountryItemsPage1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle9
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCountryItemsPage1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewCountryItemsPage1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewCountryItemsPage1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber, Me.DataGridViewTextBoxColumnCountrySettingsCategory, Me.DataGridViewTextBoxColumnCountrySettingsKey, Me.DataGridViewTextBoxColumnCountrySettingsValue})
        Me.DataGridViewCountryItemsPage1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewCountryItemsPage1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewCountryItemsPage1.Name = "DataGridViewCountryItemsPage1"
        Me.DataGridViewCountryItemsPage1.ReadOnly = True
        Me.DataGridViewCountryItemsPage1.RowTemplate.Height = 25
        Me.DataGridViewCountryItemsPage1.Size = New System.Drawing.Size(1370, 633)
        Me.DataGridViewCountryItemsPage1.TabIndex = 1
        '
        'DataGridViewTextBoxColumnCountrySettingsRecordNumber
        '
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber.HeaderText = "Record Number"
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber.MinimumWidth = 60
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber.Name = "DataGridViewTextBoxColumnCountrySettingsRecordNumber"
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsRecordNumber.Width = 60
        '
        'DataGridViewTextBoxColumnCountrySettingsCategory
        '
        Me.DataGridViewTextBoxColumnCountrySettingsCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsCategory.HeaderText = "Category"
        Me.DataGridViewTextBoxColumnCountrySettingsCategory.Name = "DataGridViewTextBoxColumnCountrySettingsCategory"
        Me.DataGridViewTextBoxColumnCountrySettingsCategory.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsCategory.Width = 80
        '
        'DataGridViewTextBoxColumnCountrySettingsKey
        '
        Me.DataGridViewTextBoxColumnCountrySettingsKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsKey.HeaderText = "Key"
        Me.DataGridViewTextBoxColumnCountrySettingsKey.Name = "DataGridViewTextBoxColumnCountrySettingsKey"
        Me.DataGridViewTextBoxColumnCountrySettingsKey.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsKey.Width = 51
        '
        'DataGridViewTextBoxColumnCountrySettingsValue
        '
        Me.DataGridViewTextBoxColumnCountrySettingsValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnCountrySettingsValue.HeaderText = "Value"
        Me.DataGridViewTextBoxColumnCountrySettingsValue.Name = "DataGridViewTextBoxColumnCountrySettingsValue"
        Me.DataGridViewTextBoxColumnCountrySettingsValue.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsValue.Width = 60
        '
        'TabPageCountryDataPg2
        '
        Me.TabPageCountryDataPg2.Controls.AddRange(New System.Windows.Forms.DataGridView() {Me.DataGridViewCountryItemsPage2, Me.DataGridViewCountryItemsPage3})
        Me.TabPageCountryDataPg2.Location = New System.Drawing.Point(4, 27)
        Me.TabPageCountryDataPg2.Name = "TabPageCountryDataPg2"
        Me.TabPageCountryDataPg2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageCountryDataPg2.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageCountryDataPg2.TabIndex = 11
        Me.TabPageCountryDataPg2.Text = "Country Data Pg2"
        Me.TabPageCountryDataPg2.UseVisualStyleBackColor = True
        '
        'DataGridViewCountryItemsPage2
        '
        Me.DataGridViewCountryItemsPage2.AllowUserToAddRows = False
        Me.DataGridViewCountryItemsPage2.AllowUserToDeleteRows = False
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewCountryItemsPage2.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle11
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCountryItemsPage2.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewCountryItemsPage2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewCountryItemsPage2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber, Me.DataGridViewTextBoxColumnCountrySettingsPg2Category, Me.DataGridViewTextBoxColumnCountrySettingsPg2Key, Me.DataGridViewTextBoxColumnCountrySettingsPg2Value})
        Me.DataGridViewCountryItemsPage2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewCountryItemsPage2.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewCountryItemsPage2.Name = "DataGridViewCountryItemsPage2"
        Me.DataGridViewCountryItemsPage2.ReadOnly = True
        Me.DataGridViewCountryItemsPage2.RowTemplate.Height = 25
        Me.DataGridViewCountryItemsPage2.Size = New System.Drawing.Size(1370, 633)
        Me.DataGridViewCountryItemsPage2.TabIndex = 1
        '
        'DataGridViewTextBoxColumnCountrySettingsPgRecordNumber
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber.HeaderText = "Record Number"
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber.MinimumWidth = 60
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber.Name = "DataGridViewTextBoxColumnCountrySettingsPgRecordNumber"
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPgRecordNumber.Width = 60
        '
        'DataGridViewTextBoxColumnCountrySettingsPg2Category
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Category.HeaderText = "Category"
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Category.Name = "DataGridViewTextBoxColumnCountrySettingsPg2Category"
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Category.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Category.Width = 80
        '
        'DataGridViewTextBoxColumnCountrySettingsPg2Key
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Key.HeaderText = "Key"
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Key.Name = "DataGridViewTextBoxColumnCountrySettingsPg2Key"
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Key.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Key.Width = 51
        '
        'DataGridViewTextBoxColumnCountrySettingsPg2Value
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Value.HeaderText = "Value"
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Value.Name = "DataGridViewTextBoxColumnCountrySettingsPg2Value"
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Value.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg2Value.Width = 60
        '
        'TabPageCountryDataPg3
        '
        Me.TabPageCountryDataPg3.Controls.Add(Me.DataGridViewCountryItemsPage3)
        Me.TabPageCountryDataPg3.Location = New System.Drawing.Point(4, 27)
        Me.TabPageCountryDataPg3.Name = "TabPageCountryDataPg3"
        Me.TabPageCountryDataPg3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageCountryDataPg3.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageCountryDataPg3.TabIndex = 11
        Me.TabPageCountryDataPg3.Text = "Country Data Pg3"
        Me.TabPageCountryDataPg3.UseVisualStyleBackColor = True
        '
        'DataGridViewCountryItemsPage3
        '
        Me.DataGridViewCountryItemsPage3.AllowUserToAddRows = False
        Me.DataGridViewCountryItemsPage3.AllowUserToDeleteRows = False
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewCountryItemsPage3.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle13
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCountryItemsPage3.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewCountryItemsPage3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewCountryItemsPage3.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber, Me.DataGridViewTextBoxColumnCountrySettingsPg3Category, Me.DataGridViewTextBoxColumnCountrySettingsPg3Key, Me.DataGridViewTextBoxColumnCountrySettingsPg3Value, Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor, Me.DataGridViewTextBoxColumnCountrySettingsPg3NotFor})
        Me.DataGridViewCountryItemsPage3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewCountryItemsPage3.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewCountryItemsPage3.Name = "DataGridViewCountryItemsPage3"
        Me.DataGridViewCountryItemsPage3.ReadOnly = True
        Me.DataGridViewCountryItemsPage3.RowTemplate.Height = 25
        Me.DataGridViewCountryItemsPage3.Size = New System.Drawing.Size(1370, 633)
        Me.DataGridViewCountryItemsPage3.TabIndex = 1
        '
        'DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber.HeaderText = "Record Number"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber.MinimumWidth = 60
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber.Name = "DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber.Width = 60
        '
        'DataGridViewTextBoxColumnCountrySettingsPg3Category
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Category.HeaderText = "Category"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Category.Name = "DataGridViewTextBoxColumnCountrySettingsPg3Category"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Category.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Category.Width = 80
        '
        'DataGridViewTextBoxColumnCountrySettingsPg3Key
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Key.HeaderText = "Key"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Key.Name = "DataGridViewTextBoxColumnCountrySettingsPg3Key"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Key.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Key.Width = 51
        '
        'DataGridViewTextBoxColumnCountrySettingsPg3Value
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value.HeaderText = "Value"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value.Name = "DataGridViewTextBoxColumnCountrySettingsPg3Value"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value.Width = 60
        '
        'DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor.HeaderText = "Report Only For"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor.Name = "DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor.Width = 200
        '
        'DataGridViewTextBoxColumnCountrySettingsPg3NotFor
        '
        Me.DataGridViewTextBoxColumnCountrySettingsPg3NotFor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnCountrySettingsPg3NotFor.HeaderText = "Report Not For"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3NotFor.Name = "DataGridViewTextBoxColumnCountrySettingsPg3NotFor"
        Me.DataGridViewTextBoxColumnCountrySettingsPg3NotFor.ReadOnly = True
        Me.DataGridViewTextBoxColumnCountrySettingsPg3Value.Width = 200
        '
        'TabPageUserProfile
        '
        Me.TabPageUserProfile.Controls.Add(Me.DataGridViewUserProfile)
        Me.TabPageUserProfile.Location = New System.Drawing.Point(4, 27)
        Me.TabPageUserProfile.Name = "TabPageUserProfile"
        Me.TabPageUserProfile.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageUserProfile.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageUserProfile.TabIndex = 12
        Me.TabPageUserProfile.Text = "User Profile"
        Me.TabPageUserProfile.UseVisualStyleBackColor = True
        '
        'DataGridViewUserProfile
        '
        Me.DataGridViewUserProfile.AllowUserToAddRows = False
        Me.DataGridViewUserProfile.AllowUserToDeleteRows = False
        Me.DataGridViewUserProfile.AllowUserToResizeColumns = False
        Me.DataGridViewUserProfile.AllowUserToResizeRows = False
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewUserProfile.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle15
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewUserProfile.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle16
        Me.DataGridViewUserProfile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridViewUserProfile.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridViewUserProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewUserProfile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewUserProfile.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewUserProfile.Name = "DataGridViewUserProfile"
        Me.DataGridViewUserProfile.ReadOnly = True
        Me.DataGridViewUserProfile.RowHeadersVisible = False
        Me.DataGridViewUserProfile.RowTemplate.Height = 25
        Me.DataGridViewUserProfile.Size = New System.Drawing.Size(1370, 633)
        Me.DataGridViewUserProfile.TabIndex = 0
        '
        'TabPageCurrentUser
        '
        Me.TabPageCurrentUser.Controls.Add(Me.DataGridViewCurrentUser)
        Me.TabPageCurrentUser.Location = New System.Drawing.Point(4, 27)
        Me.TabPageCurrentUser.Name = "TabPageCurrentUser"
        Me.TabPageCurrentUser.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageCurrentUser.TabIndex = 13
        Me.TabPageCurrentUser.Text = "Current User"
        Me.TabPageCurrentUser.UseVisualStyleBackColor = True
        '
        'DataGridViewCurrentUser
        '
        Me.DataGridViewCurrentUser.AllowUserToAddRows = False
        Me.DataGridViewCurrentUser.AllowUserToDeleteRows = False
        Me.DataGridViewCurrentUser.AllowUserToResizeColumns = False
        DataGridViewCellStyle17.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewCurrentUser.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle17
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle18.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCurrentUser.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle18
        Me.DataGridViewCurrentUser.AllowUserToResizeRows = False
        Me.DataGridViewCurrentUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridViewCurrentUser.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridViewCurrentUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewCurrentUser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewCurrentUser.Location = New System.Drawing.Point(0, 0)
        Me.DataGridViewCurrentUser.Name = "DataGridViewCurrentUser"
        Me.DataGridViewCurrentUser.ReadOnly = True
        Me.DataGridViewCurrentUser.RowHeadersVisible = False
        Me.DataGridViewCurrentUser.RowTemplate.Height = 25
        Me.DataGridViewCurrentUser.Size = New System.Drawing.Size(1376, 639)
        Me.DataGridViewCurrentUser.TabIndex = 0
        '
        'TabPageAllUsers
        '
        Me.TabPageAllUsers.Controls.Add(Me.CareLinkUsersAITComboBox)
        Me.TabPageAllUsers.Controls.Add(Me.DataGridViewCareLinkUsers)
        Me.TabPageAllUsers.Location = New System.Drawing.Point(4, 27)
        Me.TabPageAllUsers.Name = "TabPageAllUsers"
        Me.TabPageAllUsers.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageAllUsers.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageAllUsers.TabIndex = 14
        Me.TabPageAllUsers.Text = "All Users"
        Me.TabPageAllUsers.UseVisualStyleBackColor = True
        '
        'CareLinkUsersAITComboBox
        '
        Me.CareLinkUsersAITComboBox.FormattingEnabled = True
        Me.CareLinkUsersAITComboBox.Location = New System.Drawing.Point(796, 63)
        Me.CareLinkUsersAITComboBox.Name = "CareLinkUsersAITComboBox"
        Me.CareLinkUsersAITComboBox.Size = New System.Drawing.Size(121, 23)
        Me.CareLinkUsersAITComboBox.TabIndex = 1
        '
        'DataGridViewCareLinkUsers
        '
        Me.DataGridViewCareLinkUsers.AllowUserToAddRows = False
        Me.DataGridViewCareLinkUsers.AllowUserToResizeColumns = False
        Me.DataGridViewCareLinkUsers.AllowUserToResizeRows = False
        Me.DataGridViewCareLinkUsers.AutoGenerateColumns = False
        Me.DataGridViewCareLinkUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridViewCareLinkUsers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle19.BackColor = System.Drawing.Color.Silver
        Me.DataGridViewCareLinkUsers.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle19
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        DataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCareLinkUsers.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle20
        Me.DataGridViewCareLinkUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewCareLinkUsers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewButtonColumnCareLinkDeleteRow, Me.DataGridViewTextBoxColumnCareLinkUsersID, Me.DataGridViewTextBoxColumnCareLinkUserName, Me.DataGridViewTextBoxColumnCareLinkPassword, Me.DataGridViewTextBoxColumnCareLinkAIT, Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber, Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain, Me.DataGridViewTextBoxColumnCareLinkCountryCode, Me.DataGridViewTextBoxColumnCareLinkMailserverPassword, Me.DataGridViewTextBoxColumnCareLinkMailServerPort, Me.DataGridViewTextBoxColumnCareLinkMailserverUserName, Me.DataGridViewTextBoxColumnCareLinkSettingsVersion, Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer, Me.DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay, Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone, Me.DataGridViewCheckBoxColumnCareLinkAutoLogin})
        Me.DataGridViewCareLinkUsers.DataSource = Me.CareLinkUserDataRecordBindingSource
        Me.DataGridViewCareLinkUsers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewCareLinkUsers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridViewCareLinkUsers.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewCareLinkUsers.Name = "DataGridViewCareLinkUsers"
        Me.DataGridViewCareLinkUsers.RowTemplate.Height = 25
        Me.DataGridViewCareLinkUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridViewCareLinkUsers.Size = New System.Drawing.Size(1370, 633)
        Me.DataGridViewCareLinkUsers.TabIndex = 0
        '
        'DataGridViewButtonColumnCareLinkDeleteRow
        '
        Me.DataGridViewButtonColumnCareLinkDeleteRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewButtonColumnCareLinkDeleteRow.DataPropertyName = "DeleteRow"
        Me.DataGridViewButtonColumnCareLinkDeleteRow.HeaderText = ""
        Me.DataGridViewButtonColumnCareLinkDeleteRow.MinimumWidth = 50
        Me.DataGridViewButtonColumnCareLinkDeleteRow.Name = "DataGridViewButtonColumnCareLinkDeleteRow"
        Me.DataGridViewButtonColumnCareLinkDeleteRow.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnCareLinkDeleteRow.Text = "Delete Row"
        Me.DataGridViewButtonColumnCareLinkDeleteRow.UseColumnTextForButtonValue = True
        Me.DataGridViewButtonColumnCareLinkDeleteRow.Width = 50
        '
        'DataGridViewTextBoxColumnCareLinkUsersID
        '
        Me.DataGridViewTextBoxColumnCareLinkUsersID.DataPropertyName = "ID"
        Me.DataGridViewTextBoxColumnCareLinkUsersID.HeaderText = "ID"
        Me.DataGridViewTextBoxColumnCareLinkUsersID.Name = "DataGridViewTextBoxColumnCareLinkUsersID"
        Me.DataGridViewTextBoxColumnCareLinkUsersID.ReadOnly = True
        Me.DataGridViewTextBoxColumnCareLinkUsersID.Width = 43
        '
        'DataGridViewTextBoxColumnCareLinkUserName
        '
        Me.DataGridViewTextBoxColumnCareLinkUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkUserName.DataPropertyName = "CareLinkUserName"
        Me.DataGridViewTextBoxColumnCareLinkUserName.HeaderText = "CareLink UserName"
        Me.DataGridViewTextBoxColumnCareLinkUserName.MinimumWidth = 125
        Me.DataGridViewTextBoxColumnCareLinkUserName.Name = "DataGridViewTextBoxColumnCareLinkUserName"
        Me.DataGridViewTextBoxColumnCareLinkUserName.Width = 125
        '
        'DataGridViewTextBoxColumnCareLinkPassword
        '
        Me.DataGridViewTextBoxColumnCareLinkPassword.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkPassword.DataPropertyName = "CareLinkPassword"
        Me.DataGridViewTextBoxColumnCareLinkPassword.HeaderText = "CareLink Password"
        Me.DataGridViewTextBoxColumnCareLinkPassword.Name = "DataGridViewTextBoxColumnCareLinkPassword"
        Me.DataGridViewTextBoxColumnCareLinkPassword.Width = 120
        '
        'DataGridViewTextBoxColumnCareLinkAIT
        '
        Me.DataGridViewTextBoxColumnCareLinkAIT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.DataGridViewTextBoxColumnCareLinkAIT.DataPropertyName = "AIT"
        Me.DataGridViewTextBoxColumnCareLinkAIT.HeaderText = "AIT"
        Me.DataGridViewTextBoxColumnCareLinkAIT.Name = "DataGridViewTextBoxColumnCareLinkAIT"
        Me.DataGridViewTextBoxColumnCareLinkAIT.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumnCareLinkAIT.Width = 150
        '
        'DataGridViewTextBoxColumnCareLinkAlertPhoneNumber
        '
        Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber.DataPropertyName = "AlertPhoneNumber"
        Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber.HeaderText = "Alert Phone Number"
        Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber.Name = "DataGridViewTextBoxColumnCareLinkAlertPhoneNumber"
        Me.DataGridViewTextBoxColumnCareLinkAlertPhoneNumber.Width = 129
        '
        'DataGridViewTextBoxColumnCareLinkCarrierTextingDomain
        '
        Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain.DataPropertyName = "CarrierTextingDomain"
        Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain.HeaderText = "Carrier Texting Domain"
        Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain.Name = "DataGridViewTextBoxColumnCareLinkCarrierTextingDomain"
        Me.DataGridViewTextBoxColumnCareLinkCarrierTextingDomain.Width = 140
        '
        'DataGridViewTextBoxColumnCareLinkCountryCode
        '
        Me.DataGridViewTextBoxColumnCareLinkCountryCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkCountryCode.DataPropertyName = "CountryCode"
        Me.DataGridViewTextBoxColumnCareLinkCountryCode.HeaderText = "Country Code"
        Me.DataGridViewTextBoxColumnCareLinkCountryCode.Name = "DataGridViewTextBoxColumnCareLinkCountryCode"
        Me.DataGridViewTextBoxColumnCareLinkCountryCode.Width = 97
        '
        'DataGridViewTextBoxColumnCareLinkMailserverPassword
        '
        Me.DataGridViewTextBoxColumnCareLinkMailserverPassword.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkMailserverPassword.DataPropertyName = "MailserverPassword"
        Me.DataGridViewTextBoxColumnCareLinkMailserverPassword.HeaderText = "Mailserver Password"
        Me.DataGridViewTextBoxColumnCareLinkMailserverPassword.Name = "DataGridViewTextBoxColumnCareLinkMailserverPassword"
        Me.DataGridViewTextBoxColumnCareLinkMailserverPassword.Width = 127
        '
        'DataGridViewTextBoxColumnCareLinkMailServerPort
        '
        Me.DataGridViewTextBoxColumnCareLinkMailServerPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkMailServerPort.DataPropertyName = "MailServerPort"
        Me.DataGridViewTextBoxColumnCareLinkMailServerPort.HeaderText = "MailServer Port"
        Me.DataGridViewTextBoxColumnCareLinkMailServerPort.Name = "DataGridViewTextBoxColumnCareLinkMailServerPort"
        Me.DataGridViewTextBoxColumnCareLinkMailServerPort.Width = 103
        '
        'DataGridViewTextBoxColumnCareLinkMailserverUserName
        '
        Me.DataGridViewTextBoxColumnCareLinkMailserverUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkMailserverUserName.DataPropertyName = "MailserverUserName"
        Me.DataGridViewTextBoxColumnCareLinkMailserverUserName.HeaderText = "Mailserver User Name"
        Me.DataGridViewTextBoxColumnCareLinkMailserverUserName.Name = "DataGridViewTextBoxColumnCareLinkMailserverUserName"
        Me.DataGridViewTextBoxColumnCareLinkMailserverUserName.Width = 106
        '
        'DataGridViewTextBoxColumnCareLinkSettingsVersion
        '
        Me.DataGridViewTextBoxColumnCareLinkSettingsVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkSettingsVersion.DataPropertyName = "SettingsVersion"
        Me.DataGridViewTextBoxColumnCareLinkSettingsVersion.HeaderText = "Settings Version"
        Me.DataGridViewTextBoxColumnCareLinkSettingsVersion.Name = "DataGridViewTextBoxColumnCareLinkSettingsVersion"
        Me.DataGridViewTextBoxColumnCareLinkSettingsVersion.Width = 106
        '
        'DataGridViewTextBoxColumnCareLinkOutGoingMailServer
        '
        Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer.DataPropertyName = "OutGoingMailServer"
        Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer.HeaderText = "Outgoing Mail Server"
        Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer.Name = "DataGridViewTextBoxColumnCareLinkOutGoingMailServer"
        Me.DataGridViewTextBoxColumnCareLinkOutGoingMailServer.Width = 103
        '
        'DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay
        '
        Me.DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay.DataPropertyName = "UseAdvancedAITDecay"
        Me.DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay.HeaderText = "Use Advanced AIT Decay"
        Me.DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay.Name = "DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay"
        '
        'DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone
        '
        Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone.DataPropertyName = "UseLocalTimeZone"
        Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone.HeaderText = "Use Local Time Zone"
        Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone.Name = "DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone"
        Me.DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone.Width = 86
        '
        'DataGridViewCheckBoxColumnCareLinkAutoLogin
        '
        Me.DataGridViewCheckBoxColumnCareLinkAutoLogin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewCheckBoxColumnCareLinkAutoLogin.DataPropertyName = "AutoLogin"
        Me.DataGridViewCheckBoxColumnCareLinkAutoLogin.HeaderText = "Auto Login"
        Me.DataGridViewCheckBoxColumnCareLinkAutoLogin.Name = "DataGridViewCheckBoxColumnCareLinkAutoLogin"
        Me.DataGridViewCheckBoxColumnCareLinkAutoLogin.Width = 65
        '
        'CareLinkUserDataRecordBindingSource
        '
        Me.CareLinkUserDataRecordBindingSource.DataSource = GetType(CareLink.CareLinkUserDataRecord)
        '
        'TabPageBackToHomePage
        '
        Me.TabPageBackToHomePage.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.TabPageBackToHomePage.Location = New System.Drawing.Point(4, 27)
        Me.TabPageBackToHomePage.Name = "TabPageBackToHomePage"
        Me.TabPageBackToHomePage.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageBackToHomePage.Size = New System.Drawing.Size(1376, 639)
        Me.TabPageBackToHomePage.TabIndex = 8
        Me.TabPageBackToHomePage.Text = "Back.."
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "ReservoirRemains0.png")
        Me.ImageList1.Images.SetKeyName(1, "ReservoirRemains1+.png")
        Me.ImageList1.Images.SetKeyName(2, "ReservoirRemains15+.png")
        Me.ImageList1.Images.SetKeyName(3, "ReservoirRemains29+.png")
        Me.ImageList1.Images.SetKeyName(4, "ReservoirRemains43+.png")
        Me.ImageList1.Images.SetKeyName(5, "ReservoirRemains57+.png")
        Me.ImageList1.Images.SetKeyName(6, "ReservoirRemains71+.png")
        Me.ImageList1.Images.SetKeyName(7, "ReservoirRemains85+.png")
        '
        'CursorTimer
        '
        Me.CursorTimer.Interval = 60000
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "CareLink For Windows"
        Me.NotifyIcon1.Visible = True
        '
        'LoginStatusLabel
        '
        Me.LoginStatusLabel.AutoSize = True
        Me.LoginStatusLabel.Location = New System.Drawing.Point(484, 3)
        Me.LoginStatusLabel.Name = "LoginStatusLabel"
        Me.LoginStatusLabel.Size = New System.Drawing.Size(75, 15)
        Me.LoginStatusLabel.TabIndex = 23
        Me.LoginStatusLabel.Text = "Login Status:"
        '
        'LoginStatus
        '
        Me.LoginStatus.AutoSize = True
        Me.LoginStatus.Location = New System.Drawing.Point(608, 3)
        Me.LoginStatus.Name = "LoginStatus"
        Me.LoginStatus.Size = New System.Drawing.Size(58, 15)
        Me.LoginStatus.TabIndex = 24
        Me.LoginStatus.Text = "Unknown"
        '
        'LastUpdateTimeLabel
        '
        Me.LastUpdateTimeLabel.AutoSize = True
        Me.LastUpdateTimeLabel.Location = New System.Drawing.Point(1061, 3)
        Me.LastUpdateTimeLabel.Name = "LastUpdateTimeLabel"
        Me.LastUpdateTimeLabel.Size = New System.Drawing.Size(101, 15)
        Me.LastUpdateTimeLabel.TabIndex = 23
        Me.LastUpdateTimeLabel.Text = "Last Update Time:"
        '
        'LastUpdateTime
        '
        Me.LastUpdateTime.AutoSize = True
        Me.LastUpdateTime.Location = New System.Drawing.Point(1214, 3)
        Me.LastUpdateTime.Name = "LastUpdateTime"
        Me.LastUpdateTime.Size = New System.Drawing.Size(58, 15)
        Me.LastUpdateTime.TabIndex = 24
        Me.LastUpdateTime.Text = "Unknown"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1384, 694)
        Me.Controls.Add(Me.LoginStatusLabel)
        Me.Controls.Add(Me.LoginStatus)
        Me.Controls.Add(Me.LastUpdateTime)
        Me.Controls.Add(Me.LastUpdateTimeLabel)
        Me.Controls.Add(Me.TabControlHomePage)
        Me.Controls.Add(Me.TabControlPage2)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximumSize = New System.Drawing.Size(1400, 960)
        Me.Name = "Form1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CareLink For Windows"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TabControlHomePage.ResumeLayout(False)
        Me.TabPage01HomePage.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.Last24HTotalsPanel.ResumeLayout(False)
        Me.SensorTimeLeftPanel.ResumeLayout(False)
        CType(Me.SensorTimeLeftPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransmitterBatteryPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PumpBatteryPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsulinLevelPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CalibrationDueImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CursorPanel.ResumeLayout(False)
        CType(Me.CursorPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CalibrationShieldPanel.ResumeLayout(False)
        Me.CalibrationShieldPanel.PerformLayout()
        CType(Me.CalibrationShieldPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.Panel2.PerformLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.TabPage04SummaryData.ResumeLayout(False)
        CType(Me.DataGridViewSummary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage05LastSG.ResumeLayout(False)
        Me.TableLayoutPanelLastSG.ResumeLayout(False)
        Me.TabPage06LastAlarm.ResumeLayout(False)
        Me.TableLayoutPanelLastAlarm.ResumeLayout(False)
        Me.TabPage07ActiveInsulin.ResumeLayout(False)
        Me.TabPage07ActiveInsulin.PerformLayout()
        Me.TableLayoutPanelActiveInsulin.ResumeLayout(False)
        Me.TableLayoutPanelActiveInsulin.PerformLayout()
        Me.TabPage06SensorGlucose.ResumeLayout(False)
        Me.TableLayoutPanelSgs.ResumeLayout(False)
        Me.TableLayoutPanelSgs.PerformLayout()
        CType(Me.DataGridViewSGs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage07Limits.ResumeLayout(False)
        Me.TabPage07Limits.PerformLayout()
        Me.TableLayoutPanelLimits.ResumeLayout(False)
        Me.TableLayoutPanelLimits.PerformLayout()
        Me.TabPage08NotificationHistory.ResumeLayout(False)
        Me.TabPage08NotificationHistory.PerformLayout()
        Me.TableLayoutPanelNotificationHistory.ResumeLayout(False)
        Me.TableLayoutPanelNotificationHistory.PerformLayout()
        Me.TabPage09Basal.ResumeLayout(False)
        Me.TableLayoutPanelBasal.ResumeLayout(False)
        Me.TableLayoutPanelBasal.PerformLayout()
        Me.TabPage10TherapyAlgorithm.ResumeLayout(False)
        Me.TabPage10TherapyAlgorithm.PerformLayout()
        Me.TableLayoutPanelTherapyAlgorthm.ResumeLayout(False)
        Me.TableLayoutPanelTherapyAlgorthm.PerformLayout()
        Me.TabPage11BannerState.ResumeLayout(False)
        Me.TableLayoutPanelBannerState.ResumeLayout(False)
        Me.TableLayoutPanelBannerState.PerformLayout()
        Me.TabControlPage2.ResumeLayout(False)
        Me.TabPageAutoBasalDelivery.ResumeLayout(False)
        Me.TableLayoutPanelAutoBasalDelivery.ResumeLayout(False)
        CType(Me.DataGridViewAutoBasalDelivery, System.ComponentModel.ISupportInitialize).EndInit()
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
        Me.TabPageInsulin.ResumeLayout(False)
        Me.TabPageInsulin.PerformLayout()
        Me.TableLayoutPanelInsulin.ResumeLayout(False)
        Me.TableLayoutPanelInsulin.PerformLayout()
        CType(Me.DataGridViewInsulin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageLowGlusoseSuspended.ResumeLayout(False)
        Me.TabPageLowGlusoseSuspended.PerformLayout()
        Me.TableLayoutPanelLowGlusoseSuspended.ResumeLayout(False)
        Me.TableLayoutPanelLowGlusoseSuspended.PerformLayout()
        Me.TabPageMeal.ResumeLayout(False)
        Me.TabPageMeal.PerformLayout()
        Me.TableLayoutPanelMeal.ResumeLayout(False)
        Me.TableLayoutPanelMeal.PerformLayout()
        Me.TabPageTimeChange.ResumeLayout(False)
        Me.TabPageTimeChange.PerformLayout()
        Me.TableLayoutPanelTimeChange.ResumeLayout(False)
        Me.TableLayoutPanelTimeChange.PerformLayout()
        Me.TabPageCountryDataPg1.ResumeLayout(False)
        Me.TabPageCountryDataPg2.ResumeLayout(False)
        CType(Me.DataGridViewCountryItemsPage1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewCountryItemsPage2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewCountryItemsPage3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageUserProfile.ResumeLayout(False)
        CType(Me.DataGridViewUserProfile, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageCurrentUser.ResumeLayout(False)
        CType(Me.DataGridViewCurrentUser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageAllUsers.ResumeLayout(False)
        CType(Me.DataGridViewCareLinkUsers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CareLinkUserDataRecordBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AboveHighLimitMessageLabel As Label
    Friend WithEvents AboveHighLimitValueLabel As Label
    Friend WithEvents ActiveInsulinValue As Label
    Friend WithEvents AITAlgorithmLabel As Label
    Friend WithEvents AutoCorrectionLabel As Label
    Friend WithEvents AverageSGMessageLabel As Label
    Friend WithEvents AverageSGValueLabel As Label
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
    Friend WithEvents CursorPanel As Panel
    Friend WithEvents CursorPictureBox As PictureBox
    Friend WithEvents CursorTimer As Timer
    Friend WithEvents CursorMessage3Label As Label
    Friend WithEvents DailyDoseLabel As Label
    Friend WithEvents DataGridViewAutoBasalDelivery As DataGridView
    Friend WithEvents DataGridViewCareLinkUsers As DataGridView
    Friend WithEvents DataGridViewCountryItemsPage1 As DataGridView
    Friend WithEvents DataGridViewCountryItemsPage2 As DataGridView
    Friend WithEvents DataGridViewCountryItemsPage3 As DataGridView
    Friend WithEvents DataGridViewInsulin As DataGridView
    Friend WithEvents DataGridViewMeal As DataGridView
    Friend WithEvents DataGridViewUserProfile As DataGridView
    Friend WithEvents DataGridViewCurrentUser As DataGridView
    Friend WithEvents DataGridViewSGs As DataGridView
    Friend WithEvents DataGridViewSummary As DataGridView
    Friend WithEvents DataGridViewTextBoxColumnCareLinkUsersPassword As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountry As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsCategory As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsKey As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg2Category As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg2Key As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg2Value As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg3Category As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg3OnlyFor As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg3Key As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg3NotFor As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg3RecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPg3Value As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsPgRecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsRecordNumber As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCountrySettingsValue As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnFirstName As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnLastName As DataGridViewTextBoxColumn
    Friend WithEvents FullNameLabel As Label
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents InRangeMessageLabel As Label
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents LastAlarmLabel As Label
    Friend WithEvents LabelAutoBasalDelivery As Label
    Friend WithEvents LabelAutoModeStatus As Label
    Friend WithEvents LabelBannerState As Label
    Friend WithEvents LabelBasal As Label
    Friend WithEvents LabelBgReading As Label
    Friend WithEvents LabelCalibration As Label
    Friend WithEvents LabelInsulin As Label
    Friend WithEvents LabelLimits As Label
    Friend WithEvents LabelLowGlusoseSuspended As Label
    Friend WithEvents LabelMeal As Label
    Friend WithEvents LabelNotificationHistory As Label
    Friend WithEvents LabelSgs As Label
    Friend WithEvents LabelSgTrend As Label
    Friend WithEvents LabelTherapyAlgorthm As Label
    Friend WithEvents LabelTimeChange As Label
    Friend WithEvents LabelTrendArrows As Label
    Friend WithEvents LabelTrendValue As Label
    Friend WithEvents Last24CarbsValueLabel As Label
    Friend WithEvents Last24HoursLabel As Label
    Friend WithEvents Last24HTotalsPanel As Panel
    Friend WithEvents LastUpdateTime As Label
    Friend WithEvents LastUpdateTimeLabel As Label
    Friend WithEvents ListView1 As ListView
    Friend WithEvents LoginStatus As Label
    Friend WithEvents LoginStatusLabel As Label
    Friend WithEvents ManualBolusLabel As Label
    Friend WithEvents MaxBasalPerHourLabel As Label
    Friend WithEvents MenuHelp As ToolStripMenuItem
    Friend WithEvents MenuHelpAbout As ToolStripMenuItem
    Friend WithEvents MenuHelpCheckForUpdates As ToolStripMenuItem
    Friend WithEvents MenuHelpReportAnIssue As ToolStripMenuItem
    Friend WithEvents MenuOptions As ToolStripMenuItem
    Friend WithEvents MenuOptionsAutoLogin As ToolStripMenuItem
    Friend WithEvents MenuOptionsFilterRawJSONData As ToolStripMenuItem
    Friend WithEvents MenuOptionsSetupEmailServer As ToolStripMenuItem
    Friend WithEvents MenuOptionsUseAdvancedAITDecay As ToolStripMenuItem
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
    Friend WithEvents PumpBatteryPictureBox As PictureBox
    Friend WithEvents PumpBatteryRemainingLabel As Label
    Friend WithEvents ReadingIntervalLabel As Label
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
    Friend WithEvents ShowMiniDisplay As ToolStripMenuItem
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents StartHereExit As ToolStripMenuItem
    Friend WithEvents TabControlHomePage As TabControl
    Friend WithEvents TabControlPage2 As TabControl
    Friend WithEvents TableLayoutPanelActiveInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelAutoBasalDelivery As TableLayoutPanel
    Friend WithEvents TableLayoutPanelAutoModeStatus As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBannerState As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBasal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBgReadings As TableLayoutPanel
    Friend WithEvents TableLayoutPanelCalibration As TableLayoutPanel
    Friend WithEvents TableLayoutPanelInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLimits As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLowGlusoseSuspended As TableLayoutPanel
    Friend WithEvents TableLayoutPanelMeal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelNotificationHistory As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSgs As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTherapyAlgorthm As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTimeChange As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLastSG As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLastAlarm As TableLayoutPanel
    Friend WithEvents TabPage01HomePage As TabPage
    Friend WithEvents TabPage02RunningIOB As TabPage
    Friend WithEvents TabPage03TreatmentDetails As TabPage
    Friend WithEvents TabPage04SummaryData As TabPage
    Friend WithEvents TabPage07ActiveInsulin As TabPage
    Friend WithEvents TabPage06SensorGlucose As TabPage
    Friend WithEvents TabPage07Limits As TabPage
    Friend WithEvents TabPage08NotificationHistory As TabPage
    Friend WithEvents TabPage09Basal As TabPage
    Friend WithEvents TabPage10TherapyAlgorithm As TabPage
    Friend WithEvents TabPage11BannerState As TabPage
    Friend WithEvents TabPage16Markers As TabPage
    Friend WithEvents TabPageAllUsers As TabPage
    Friend WithEvents TabPageAutoBasalDelivery As TabPage
    Friend WithEvents TabPageAutoModeStatus As TabPage
    Friend WithEvents TabPageBgReadings As TabPage
    Friend WithEvents TabPageCalibration As TabPage
    Friend WithEvents TabPageCountryDataPg1 As TabPage
    Friend WithEvents TabPageCountryDataPg2 As TabPage
    Friend WithEvents TabPageCountryDataPg3 As TabPage
    Friend WithEvents TabPageCurrentUser As TabPage
    Friend WithEvents TabPageInsulin As TabPage
    Friend WithEvents TabPageLowGlusoseSuspended As TabPage
    Friend WithEvents TabPageMeal As TabPage
    Friend WithEvents TabPageTimeChange As TabPage
    Friend WithEvents TabPageUserProfile As TabPage
    Friend WithEvents TimeInRangeChartLabel As Label
    Friend WithEvents TimeInRangeLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TotalsLabel As Label
    Friend WithEvents TransmatterBatterPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents TabPageBackToHomePage As TabPage
    Friend WithEvents LastSGTimeLabel As Label
    Friend WithEvents ActiveInsulinTabLabel As Label
    Friend WithEvents TempTargetLabel As Label
    Friend WithEvents CareLinkUsersAITComboBox As ComboBox
    Friend WithEvents DataGridViewButtonColumnCareLinkDeleteRow As DataGridViewDisableButtonColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkUsersID As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkUserName As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkPassword As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkAIT As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkAlertPhoneNumber As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkCarrierTextingDomain As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkCountryCode As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkMailserverPassword As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkMailServerPort As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkMailserverUserName As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkSettingsVersion As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCareLinkOutGoingMailServer As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumnCareLinkUseAdvancedAITDecay As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumnCareLinkUseLocalTimeZone As DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumnCareLinkAutoLogin As DataGridViewCheckBoxColumn
    Friend WithEvents TabPage05LastSG As TabPage
    Friend WithEvents TabPage06LastAlarm As TabPage
    Friend WithEvents LastSGLabel As Label
End Class
