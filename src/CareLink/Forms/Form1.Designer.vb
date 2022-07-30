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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MenuStartHere = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStartHereLogin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStartHereLoadSavedDataFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStartHereExceptionReportLoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStartHereSnapshotSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.StartHereExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsFilterRawJSONData = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsSetupEmailServer = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsUseTestData = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsUseLastSavedData = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionsUseAdvancedAITDecay = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuView = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuViewShowMiniDisplay = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpReportAProblem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpCheckForUpdatesMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerUpdateTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanelSummaryData = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelTop1 = New System.Windows.Forms.TableLayoutPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanelTop2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1HomePage = New System.Windows.Forms.TabPage()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.ShieldUnitsLabel = New System.Windows.Forms.Label()
        Me.SensorMessage = New System.Windows.Forms.Label()
        Me.CurrentBG = New System.Windows.Forms.Label()
        Me.Last24CarbsValueLabel = New System.Windows.Forms.Label()
        Me.TotalCarbs24MessageLabel = New System.Windows.Forms.Label()
        Me.TotalCarbsMessageLabel = New System.Windows.Forms.Label()
        Me.ModelLabel = New System.Windows.Forms.Label()
        Me.SerialNumberLabel = New System.Windows.Forms.Label()
        Me.FullNameLabel = New System.Windows.Forms.Label()
        Me.ReadingIntervalLabel = New System.Windows.Forms.Label()
        Me.ReadingsLabel = New System.Windows.Forms.Label()
        Me.TotalsLabel = New System.Windows.Forms.Label()
        Me.AutoCorrectionLabel = New System.Windows.Forms.Label()
        Me.ManualBolusLabel = New System.Windows.Forms.Label()
        Me.BasalLabel = New System.Windows.Forms.Label()
        Me.DailyDoseLabel = New System.Windows.Forms.Label()
        Me.PumpBatteryRemainingLabel = New System.Windows.Forms.Label()
        Me.TransmatterBatterPercentLabel = New System.Windows.Forms.Label()
        Me.TransmitterBatteryPictureBox = New System.Windows.Forms.PictureBox()
        Me.SensorTimeLeftLabel = New System.Windows.Forms.Label()
        Me.SensorDaysLeftLabel = New System.Windows.Forms.Label()
        Me.SensorTimeLeftPictureBox = New System.Windows.Forms.PictureBox()
        Me.PumpBatteryPictureBox = New System.Windows.Forms.PictureBox()
        Me.AITLabel = New System.Windows.Forms.Label()
        Me.AITComboBox = New System.Windows.Forms.ComboBox()
        Me.CursorMessage2Label = New System.Windows.Forms.Label()
        Me.CursorValueLabel = New System.Windows.Forms.Label()
        Me.CursorPictureBox = New System.Windows.Forms.PictureBox()
        Me.CursorMessage1Label = New System.Windows.Forms.Label()
        Me.RemainingInsulinUnits = New System.Windows.Forms.Label()
        Me.InsulinLevelPictureBox = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinValue = New System.Windows.Forms.Label()
        Me.CalibrationDueImage = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinLabel = New System.Windows.Forms.Label()
        Me.ShieldPictureBox = New System.Windows.Forms.PictureBox()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.CursorTimeLabel = New System.Windows.Forms.Label()
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
        Me.TabPage2RunningActiveInsulin = New System.Windows.Forms.TabPage()
        Me.TabPage3SummaryData = New System.Windows.Forms.TabPage()
        Me.TabPage4ActiveInsulin = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelActiveInsulin = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage5SensorGlucose = New System.Windows.Forms.TabPage()
        Me.SGsDataGridView = New System.Windows.Forms.DataGridView()
        Me.TabPage6Limits = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelLimits = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage7Markers = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelMarkers = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage8NotificationHistory = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelNotificationHistory = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage9Basal = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelBasal = New System.Windows.Forms.TableLayoutPanel()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.CursorTimer = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.LoginStatusLabel = New System.Windows.Forms.Label()
        Me.LoginStatus = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer4.Panel1.SuspendLayout()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1HomePage.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.TransmitterBatteryPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SensorTimeLeftPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PumpBatteryPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CursorPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InsulinLevelPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CalibrationDueImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShieldPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.TabPage3SummaryData.SuspendLayout()
        Me.TabPage4ActiveInsulin.SuspendLayout()
        Me.TabPage5SensorGlucose.SuspendLayout()
        CType(Me.SGsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage6Limits.SuspendLayout()
        Me.TabPage7Markers.SuspendLayout()
        Me.TabPage8NotificationHistory.SuspendLayout()
        Me.TabPage9Basal.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuStartHere, Me.MenuOptions, Me.MenuView, Me.MenuHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1384, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MenuStartHere
        '
        Me.MenuStartHere.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuStartHereLogin, Me.ToolStripSeparator1, Me.MenuStartHereLoadSavedDataFile, Me.MenuStartHereExceptionReportLoadToolStripMenuItem, Me.MenuStartHereSnapshotSave, Me.ToolStripSeparator2, Me.StartHereExit})
        Me.MenuStartHere.Name = "MenuStartHere"
        Me.MenuStartHere.Size = New System.Drawing.Size(71, 20)
        Me.MenuStartHere.Text = "Start Here"
        '
        'MenuStartHereLogin
        '
        Me.MenuStartHereLogin.Name = "MenuStartHereLogin"
        Me.MenuStartHereLogin.Size = New System.Drawing.Size(193, 22)
        Me.MenuStartHereLogin.Text = "Login"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(190, 6)
        '
        'MenuStartHereLoadSavedDataFile
        '
        Me.MenuStartHereLoadSavedDataFile.Name = "MenuStartHereLoadSavedDataFile"
        Me.MenuStartHereLoadSavedDataFile.Size = New System.Drawing.Size(193, 22)
        Me.MenuStartHereLoadSavedDataFile.Text = "Load Saved Data File"
        '
        'MenuStartHereExceptionReportLoadToolStripMenuItem
        '
        Me.MenuStartHereExceptionReportLoadToolStripMenuItem.Name = "MenuStartHereExceptionReportLoadToolStripMenuItem"
        Me.MenuStartHereExceptionReportLoadToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.MenuStartHereExceptionReportLoadToolStripMenuItem.Text = "Exception Report Load"
        '
        'MenuStartHereSnapshotSave
        '
        Me.MenuStartHereSnapshotSave.Name = "MenuStartHereSnapshotSave"
        Me.MenuStartHereSnapshotSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.MenuStartHereSnapshotSave.Size = New System.Drawing.Size(193, 22)
        Me.MenuStartHereSnapshotSave.Text = "Snapshot &Save"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(190, 6)
        '
        'StartHereExit
        '
        Me.StartHereExit.Image = Global.CareLink.My.Resources.Resources.AboutBox
        Me.StartHereExit.Name = "StartHereExit"
        Me.StartHereExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.StartHereExit.Size = New System.Drawing.Size(193, 22)
        Me.StartHereExit.Text = "E&xit"
        '
        'MenuOptions
        '
        Me.MenuOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOptionsFilterRawJSONData, Me.MenuOptionsSetupEmailServer, Me.MenuOptionsUseTestData, Me.MenuOptionsUseLastSavedData, Me.MenuOptionsUseAdvancedAITDecay})
        Me.MenuOptions.Name = "MenuOptions"
        Me.MenuOptions.Size = New System.Drawing.Size(61, 20)
        Me.MenuOptions.Text = "Options"
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
        '
        'MenuOptionsUseTestData
        '
        Me.MenuOptionsUseTestData.CheckOnClick = True
        Me.MenuOptionsUseTestData.Name = "MenuOptionsUseTestData"
        Me.MenuOptionsUseTestData.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsUseTestData.Text = "Use Test Data"
        '
        'MenuOptionsUseLastSavedData
        '
        Me.MenuOptionsUseLastSavedData.CheckOnClick = True
        Me.MenuOptionsUseLastSavedData.Name = "MenuOptionsUseLastSavedData"
        Me.MenuOptionsUseLastSavedData.Size = New System.Drawing.Size(204, 22)
        Me.MenuOptionsUseLastSavedData.Text = "Use Last Saved Data"
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
        'MenuView
        '
        Me.MenuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuViewShowMiniDisplay})
        Me.MenuView.Name = "MenuView"
        Me.MenuView.Size = New System.Drawing.Size(44, 20)
        Me.MenuView.Text = "View"
        Me.MenuView.Visible = False
        '
        'MenuViewShowMiniDisplay
        '
        Me.MenuViewShowMiniDisplay.Name = "MenuViewShowMiniDisplay"
        Me.MenuViewShowMiniDisplay.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.MenuViewShowMiniDisplay.Size = New System.Drawing.Size(243, 22)
        Me.MenuViewShowMiniDisplay.Text = "Show Mini Display"
        '
        'MenuHelp
        '
        Me.MenuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuHelpReportAProblem, Me.MenuHelpCheckForUpdatesMenuItem, Me.MenuHelpAbout})
        Me.MenuHelp.Name = "MenuHelp"
        Me.MenuHelp.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.MenuHelp.Size = New System.Drawing.Size(44, 20)
        Me.MenuHelp.Text = "&Help"
        '
        'MenuHelpReportAProblem
        '
        Me.MenuHelpReportAProblem.Image = Global.CareLink.My.Resources.Resources.FeedbackSmile_16x
        Me.MenuHelpReportAProblem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MenuHelpReportAProblem.Name = "MenuHelpReportAProblem"
        Me.MenuHelpReportAProblem.Size = New System.Drawing.Size(177, 22)
        Me.MenuHelpReportAProblem.Text = "Report A Problem..."
        '
        'MenuHelpCheckForUpdatesMenuItem
        '
        Me.MenuHelpCheckForUpdatesMenuItem.Name = "MenuHelpCheckForUpdatesMenuItem"
        Me.MenuHelpCheckForUpdatesMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.MenuHelpCheckForUpdatesMenuItem.Text = "Check For Updates"
        '
        'MenuHelpAbout
        '
        Me.MenuHelpAbout.Image = Global.CareLink.My.Resources.Resources.AboutBox
        Me.MenuHelpAbout.Name = "MenuHelpAbout"
        Me.MenuHelpAbout.Size = New System.Drawing.Size(177, 22)
        Me.MenuHelpAbout.Text = "&About..."
        '
        'ServerUpdateTimer
        '
        Me.ServerUpdateTimer.Interval = 300000
        '
        'TableLayoutPanelSummaryData
        '
        Me.TableLayoutPanelSummaryData.AutoScroll = True
        Me.TableLayoutPanelSummaryData.AutoSize = True
        Me.TableLayoutPanelSummaryData.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelSummaryData.ColumnCount = 2
        Me.TableLayoutPanelSummaryData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.47268!))
        Me.TableLayoutPanelSummaryData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.52732!))
        Me.TableLayoutPanelSummaryData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelSummaryData.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelSummaryData.Name = "TableLayoutPanelSummaryData"
        Me.TableLayoutPanelSummaryData.Padding = New System.Windows.Forms.Padding(5)
        Me.TableLayoutPanelSummaryData.RowCount = 53
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelSummaryData.Size = New System.Drawing.Size(1370, 611)
        Me.TableLayoutPanelSummaryData.TabIndex = 3
        '
        'TableLayoutPanelTop1
        '
        Me.TableLayoutPanelTop1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTop1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTop1.ColumnCount = 2
        Me.TableLayoutPanelTop1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanelTop1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.0!))
        Me.TableLayoutPanelTop1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelTop1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelTop1.Name = "TableLayoutPanelTop1"
        Me.TableLayoutPanelTop1.RowCount = 1
        Me.TableLayoutPanelTop1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelTop1.Size = New System.Drawing.Size(423, 155)
        Me.TableLayoutPanelTop1.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Top
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer4)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1384, 897)
        Me.SplitContainer1.SplitterDistance = 155
        Me.SplitContainer1.SplitterWidth = 2
        Me.SplitContainer1.TabIndex = 22
        '
        'SplitContainer4
        '
        Me.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer4.IsSplitterFixed = True
        Me.SplitContainer4.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer4.Name = "SplitContainer4"
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.Controls.Add(Me.TableLayoutPanelTop1)
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.TableLayoutPanelTop2)
        Me.SplitContainer4.Size = New System.Drawing.Size(1384, 155)
        Me.SplitContainer4.SplitterDistance = 423
        Me.SplitContainer4.TabIndex = 0
        '
        'TableLayoutPanelTop2
        '
        Me.TableLayoutPanelTop2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTop2.CausesValidation = False
        Me.TableLayoutPanelTop2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTop2.ColumnCount = 2
        Me.TableLayoutPanelTop2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.0!))
        Me.TableLayoutPanelTop2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 92.0!))
        Me.TableLayoutPanelTop2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TableLayoutPanelTop2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelTop2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelTop2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanelTop2.Name = "TableLayoutPanelTop2"
        Me.TableLayoutPanelTop2.RowCount = 1
        Me.TableLayoutPanelTop2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelTop2.Size = New System.Drawing.Size(957, 155)
        Me.TableLayoutPanelTop2.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1HomePage)
        Me.TabControl1.Controls.Add(Me.TabPage2RunningActiveInsulin)
        Me.TabControl1.Controls.Add(Me.TabPage3SummaryData)
        Me.TabControl1.Controls.Add(Me.TabPage4ActiveInsulin)
        Me.TabControl1.Controls.Add(Me.TabPage5SensorGlucose)
        Me.TabControl1.Controls.Add(Me.TabPage6Limits)
        Me.TabControl1.Controls.Add(Me.TabPage7Markers)
        Me.TabControl1.Controls.Add(Me.TabPage8NotificationHistory)
        Me.TabControl1.Controls.Add(Me.TabPage9Basal)
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1384, 645)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1HomePage
        '
        Me.TabPage1HomePage.BackColor = System.Drawing.Color.Black
        Me.TabPage1HomePage.Controls.Add(Me.SplitContainer2)
        Me.TabPage1HomePage.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1HomePage.Name = "TabPage1HomePage"
        Me.TabPage1HomePage.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1HomePage.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage1HomePage.TabIndex = 7
        Me.TabPage1HomePage.Text = "Home Page"
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
        Me.SplitContainer2.Panel1.Controls.Add(Me.ShieldUnitsLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorMessage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CurrentBG)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Last24CarbsValueLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalCarbs24MessageLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalCarbsMessageLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ModelLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SerialNumberLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.FullNameLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ReadingIntervalLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ReadingsLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalsLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.AutoCorrectionLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ManualBolusLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.BasalLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.DailyDoseLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryRemainingLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TransmatterBatterPercentLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TransmitterBatteryPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorTimeLeftLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorDaysLeftLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorTimeLeftPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.PumpBatteryPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.AITLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.AITComboBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CursorMessage2Label)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CursorValueLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CursorPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CursorMessage1Label)
        Me.SplitContainer2.Panel1.Controls.Add(Me.RemainingInsulinUnits)
        Me.SplitContainer2.Panel1.Controls.Add(Me.InsulinLevelPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ActiveInsulinValue)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CalibrationDueImage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ActiveInsulinLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ShieldPictureBox)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Size = New System.Drawing.Size(1370, 611)
        Me.SplitContainer2.SplitterDistance = 132
        Me.SplitContainer2.TabIndex = 52
        '
        'ShieldUnitsLabel
        '
        Me.ShieldUnitsLabel.AutoSize = True
        Me.ShieldUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ShieldUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.ShieldUnitsLabel.Location = New System.Drawing.Point(422, 59)
        Me.ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        Me.ShieldUnitsLabel.Size = New System.Drawing.Size(35, 13)
        Me.ShieldUnitsLabel.TabIndex = 7
        Me.ShieldUnitsLabel.Text = "XX/XX"
        '
        'SensorMessage
        '
        Me.SensorMessage.BackColor = System.Drawing.Color.Transparent
        Me.SensorMessage.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorMessage.ForeColor = System.Drawing.Color.White
        Me.SensorMessage.Location = New System.Drawing.Point(389, 14)
        Me.SensorMessage.Name = "SensorMessage"
        Me.SensorMessage.Size = New System.Drawing.Size(100, 66)
        Me.SensorMessage.TabIndex = 1
        Me.SensorMessage.Text = "Calibration Required"
        Me.SensorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CurrentBG
        '
        Me.CurrentBG.BackColor = System.Drawing.Color.Transparent
        Me.CurrentBG.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CurrentBG.ForeColor = System.Drawing.Color.White
        Me.CurrentBG.Location = New System.Drawing.Point(403, 23)
        Me.CurrentBG.Name = "CurrentBG"
        Me.CurrentBG.Size = New System.Drawing.Size(72, 32)
        Me.CurrentBG.TabIndex = 3
        Me.CurrentBG.Text = "---"
        Me.CurrentBG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Last24CarbsValueLabel
        '
        Me.Last24CarbsValueLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Last24CarbsValueLabel.ForeColor = System.Drawing.Color.White
        Me.Last24CarbsValueLabel.Location = New System.Drawing.Point(1, 107)
        Me.Last24CarbsValueLabel.Name = "Last24CarbsValueLabel"
        Me.Last24CarbsValueLabel.Size = New System.Drawing.Size(128, 23)
        Me.Last24CarbsValueLabel.TabIndex = 60
        Me.Last24CarbsValueLabel.Text = "100"
        Me.Last24CarbsValueLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TotalCarbs24MessageLabel
        '
        Me.TotalCarbs24MessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalCarbs24MessageLabel.ForeColor = System.Drawing.Color.White
        Me.TotalCarbs24MessageLabel.Location = New System.Drawing.Point(1, 88)
        Me.TotalCarbs24MessageLabel.Name = "TotalCarbs24MessageLabel"
        Me.TotalCarbs24MessageLabel.Size = New System.Drawing.Size(128, 23)
        Me.TotalCarbs24MessageLabel.TabIndex = 59
        Me.TotalCarbs24MessageLabel.Text = "Last 24 hours"
        Me.TotalCarbs24MessageLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TotalCarbsMessageLabel
        '
        Me.TotalCarbsMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalCarbsMessageLabel.ForeColor = System.Drawing.Color.White
        Me.TotalCarbsMessageLabel.Location = New System.Drawing.Point(1, 65)
        Me.TotalCarbsMessageLabel.Name = "TotalCarbsMessageLabel"
        Me.TotalCarbsMessageLabel.Size = New System.Drawing.Size(128, 23)
        Me.TotalCarbsMessageLabel.TabIndex = 58
        Me.TotalCarbsMessageLabel.Text = "Total Carbs"
        Me.TotalCarbsMessageLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'ModelLabel
        '
        Me.ModelLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ModelLabel.ForeColor = System.Drawing.Color.White
        Me.ModelLabel.Location = New System.Drawing.Point(1181, 26)
        Me.ModelLabel.Name = "ModelLabel"
        Me.ModelLabel.Size = New System.Drawing.Size(170, 23)
        Me.ModelLabel.TabIndex = 57
        Me.ModelLabel.Text = "Model"
        '
        'SerialNumberLabel
        '
        Me.SerialNumberLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SerialNumberLabel.ForeColor = System.Drawing.Color.White
        Me.SerialNumberLabel.Location = New System.Drawing.Point(1181, 53)
        Me.SerialNumberLabel.Name = "SerialNumberLabel"
        Me.SerialNumberLabel.Size = New System.Drawing.Size(170, 23)
        Me.SerialNumberLabel.TabIndex = 56
        Me.SerialNumberLabel.Text = "Serial Number"
        '
        'FullNameLabel
        '
        Me.FullNameLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.FullNameLabel.ForeColor = System.Drawing.Color.White
        Me.FullNameLabel.Location = New System.Drawing.Point(1181, 0)
        Me.FullNameLabel.Name = "FullNameLabel"
        Me.FullNameLabel.Size = New System.Drawing.Size(170, 23)
        Me.FullNameLabel.TabIndex = 55
        Me.FullNameLabel.Text = "Full Name"
        '
        'ReadingIntervalLabel
        '
        Me.ReadingIntervalLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ReadingIntervalLabel.ForeColor = System.Drawing.Color.White
        Me.ReadingIntervalLabel.Location = New System.Drawing.Point(839, 107)
        Me.ReadingIntervalLabel.Name = "ReadingIntervalLabel"
        Me.ReadingIntervalLabel.Size = New System.Drawing.Size(162, 23)
        Me.ReadingIntervalLabel.TabIndex = 54
        Me.ReadingIntervalLabel.Text = "5 minute readings"
        '
        'ReadingsLabel
        '
        Me.ReadingsLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ReadingsLabel.ForeColor = System.Drawing.Color.White
        Me.ReadingsLabel.Location = New System.Drawing.Point(839, 81)
        Me.ReadingsLabel.Name = "ReadingsLabel"
        Me.ReadingsLabel.Size = New System.Drawing.Size(162, 21)
        Me.ReadingsLabel.TabIndex = 53
        Me.ReadingsLabel.Text = "280/288"
        Me.ReadingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TotalsLabel
        '
        Me.TotalsLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalsLabel.ForeColor = System.Drawing.Color.White
        Me.TotalsLabel.Location = New System.Drawing.Point(1003, 0)
        Me.TotalsLabel.Name = "TotalsLabel"
        Me.TotalsLabel.Size = New System.Drawing.Size(167, 21)
        Me.TotalsLabel.TabIndex = 52
        Me.TotalsLabel.Text = "Totals"
        Me.TotalsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AutoCorrectionLabel
        '
        Me.AutoCorrectionLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AutoCorrectionLabel.ForeColor = System.Drawing.Color.White
        Me.AutoCorrectionLabel.Location = New System.Drawing.Point(1003, 108)
        Me.AutoCorrectionLabel.Name = "AutoCorrectionLabel"
        Me.AutoCorrectionLabel.Size = New System.Drawing.Size(253, 21)
        Me.AutoCorrectionLabel.TabIndex = 51
        Me.AutoCorrectionLabel.Text = "Auto Correction 20 U | 50%"
        Me.AutoCorrectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ManualBolusLabel
        '
        Me.ManualBolusLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ManualBolusLabel.ForeColor = System.Drawing.Color.White
        Me.ManualBolusLabel.Location = New System.Drawing.Point(1003, 81)
        Me.ManualBolusLabel.Name = "ManualBolusLabel"
        Me.ManualBolusLabel.Size = New System.Drawing.Size(253, 21)
        Me.ManualBolusLabel.TabIndex = 50
        Me.ManualBolusLabel.Text = "Manual Bolus 30 U | 30%"
        Me.ManualBolusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BasalLabel
        '
        Me.BasalLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BasalLabel.ForeColor = System.Drawing.Color.White
        Me.BasalLabel.Location = New System.Drawing.Point(1003, 54)
        Me.BasalLabel.Name = "BasalLabel"
        Me.BasalLabel.Size = New System.Drawing.Size(204, 21)
        Me.BasalLabel.TabIndex = 49
        Me.BasalLabel.Text = "Basal 50 U | 50%"
        Me.BasalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DailyDoseLabel
        '
        Me.DailyDoseLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.DailyDoseLabel.ForeColor = System.Drawing.Color.White
        Me.DailyDoseLabel.Location = New System.Drawing.Point(1003, 27)
        Me.DailyDoseLabel.Name = "DailyDoseLabel"
        Me.DailyDoseLabel.Size = New System.Drawing.Size(167, 21)
        Me.DailyDoseLabel.TabIndex = 48
        Me.DailyDoseLabel.Text = "Daily Dose 100 U"
        Me.DailyDoseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PumpBatteryRemainingLabel
        '
        Me.PumpBatteryRemainingLabel.BackColor = System.Drawing.Color.Transparent
        Me.PumpBatteryRemainingLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.PumpBatteryRemainingLabel.ForeColor = System.Drawing.Color.White
        Me.PumpBatteryRemainingLabel.Location = New System.Drawing.Point(502, 95)
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
        Me.TransmatterBatterPercentLabel.Location = New System.Drawing.Point(687, 95)
        Me.TransmatterBatterPercentLabel.Name = "TransmatterBatterPercentLabel"
        Me.TransmatterBatterPercentLabel.Size = New System.Drawing.Size(55, 21)
        Me.TransmatterBatterPercentLabel.TabIndex = 13
        Me.TransmatterBatterPercentLabel.Text = "???"
        Me.TransmatterBatterPercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TransmitterBatteryPictureBox
        '
        Me.TransmitterBatteryPictureBox.ErrorImage = Nothing
        Me.TransmitterBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.TransmitterBatteryUnknown
        Me.TransmitterBatteryPictureBox.Location = New System.Drawing.Point(680, 6)
        Me.TransmitterBatteryPictureBox.Name = "TransmitterBatteryPictureBox"
        Me.TransmitterBatteryPictureBox.Size = New System.Drawing.Size(68, 78)
        Me.TransmitterBatteryPictureBox.TabIndex = 47
        Me.TransmitterBatteryPictureBox.TabStop = False
        '
        'SensorTimeLeftLabel
        '
        Me.SensorTimeLeftLabel.BackColor = System.Drawing.Color.Transparent
        Me.SensorTimeLeftLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorTimeLeftLabel.ForeColor = System.Drawing.Color.White
        Me.SensorTimeLeftLabel.Location = New System.Drawing.Point(752, 95)
        Me.SensorTimeLeftLabel.Name = "SensorTimeLeftLabel"
        Me.SensorTimeLeftLabel.Size = New System.Drawing.Size(100, 21)
        Me.SensorTimeLeftLabel.TabIndex = 14
        Me.SensorTimeLeftLabel.Text = "???"
        Me.SensorTimeLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SensorDaysLeftLabel
        '
        Me.SensorDaysLeftLabel.BackColor = System.Drawing.Color.Transparent
        Me.SensorDaysLeftLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorDaysLeftLabel.ForeColor = System.Drawing.Color.White
        Me.SensorDaysLeftLabel.Location = New System.Drawing.Point(780, 19)
        Me.SensorDaysLeftLabel.Name = "SensorDaysLeftLabel"
        Me.SensorDaysLeftLabel.Size = New System.Drawing.Size(37, 52)
        Me.SensorDaysLeftLabel.TabIndex = 4
        Me.SensorDaysLeftLabel.Text = "5"
        Me.SensorDaysLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SensorDaysLeftLabel.Visible = False
        '
        'SensorTimeLeftPictureBox
        '
        Me.SensorTimeLeftPictureBox.ErrorImage = Nothing
        Me.SensorTimeLeftPictureBox.Image = Global.CareLink.My.Resources.Resources.SensorExpirationUnknown
        Me.SensorTimeLeftPictureBox.Location = New System.Drawing.Point(768, 6)
        Me.SensorTimeLeftPictureBox.Name = "SensorTimeLeftPictureBox"
        Me.SensorTimeLeftPictureBox.Size = New System.Drawing.Size(68, 78)
        Me.SensorTimeLeftPictureBox.TabIndex = 44
        Me.SensorTimeLeftPictureBox.TabStop = False
        '
        'PumpBatteryPictureBox
        '
        Me.PumpBatteryPictureBox.ErrorImage = Nothing
        Me.PumpBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.PumpBatteryFull
        Me.PumpBatteryPictureBox.Location = New System.Drawing.Point(510, 6)
        Me.PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        Me.PumpBatteryPictureBox.Size = New System.Drawing.Size(64, 74)
        Me.PumpBatteryPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PumpBatteryPictureBox.TabIndex = 43
        Me.PumpBatteryPictureBox.TabStop = False
        '
        'AITLabel
        '
        Me.AITLabel.BackColor = System.Drawing.Color.Transparent
        Me.AITLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AITLabel.ForeColor = System.Drawing.Color.White
        Me.AITLabel.Location = New System.Drawing.Point(826, 0)
        Me.AITLabel.Name = "AITLabel"
        Me.AITLabel.Size = New System.Drawing.Size(189, 21)
        Me.AITLabel.TabIndex = 8
        Me.AITLabel.Text = "Active Insulin TIme"
        Me.AITLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AITComboBox
        '
        Me.AITComboBox.BackColor = System.Drawing.Color.Black
        Me.AITComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AITComboBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AITComboBox.ForeColor = System.Drawing.Color.White
        Me.AITComboBox.Items.AddRange(New Object() {"2:00", "2:15", "2:30", "2:45", "3:00", "3:15", "3:30", "3:45", "4:00", "4:15", "4:30", "4:45", "5:00", "5:15", "5:30", "5:45", "6:00"})
        Me.AITComboBox.Location = New System.Drawing.Point(881, 26)
        Me.AITComboBox.Name = "AITComboBox"
        Me.AITComboBox.Size = New System.Drawing.Size(78, 23)
        Me.AITComboBox.TabIndex = 0
        '
        'CursorMessage2Label
        '
        Me.CursorMessage2Label.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage2Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage2Label.Location = New System.Drawing.Point(154, 81)
        Me.CursorMessage2Label.Name = "CursorMessage2Label"
        Me.CursorMessage2Label.Size = New System.Drawing.Size(235, 21)
        Me.CursorMessage2Label.TabIndex = 9
        Me.CursorMessage2Label.Text = "Message For Cursor 2"
        Me.CursorMessage2Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorMessage2Label.Visible = False
        '
        'CursorValueLabel
        '
        Me.CursorValueLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorValueLabel.ForeColor = System.Drawing.Color.White
        Me.CursorValueLabel.Location = New System.Drawing.Point(154, 108)
        Me.CursorValueLabel.Name = "CursorValueLabel"
        Me.CursorValueLabel.Size = New System.Drawing.Size(235, 21)
        Me.CursorValueLabel.TabIndex = 10
        Me.CursorValueLabel.Text = "Value For Cursor"
        Me.CursorValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorValueLabel.Visible = False
        '
        'CursorPictureBox
        '
        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.MealImageLarge
        Me.CursorPictureBox.InitialImage = Nothing
        Me.CursorPictureBox.Location = New System.Drawing.Point(252, 6)
        Me.CursorPictureBox.Name = "CursorPictureBox"
        Me.CursorPictureBox.Size = New System.Drawing.Size(39, 45)
        Me.CursorPictureBox.TabIndex = 38
        Me.CursorPictureBox.TabStop = False
        Me.CursorPictureBox.Visible = False
        '
        'CursorMessage1Label
        '
        Me.CursorMessage1Label.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage1Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage1Label.Location = New System.Drawing.Point(154, 54)
        Me.CursorMessage1Label.Name = "CursorMessage1Label"
        Me.CursorMessage1Label.Size = New System.Drawing.Size(235, 21)
        Me.CursorMessage1Label.TabIndex = 6
        Me.CursorMessage1Label.Text = "Message For Cursor 1"
        Me.CursorMessage1Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorMessage1Label.Visible = False
        '
        'RemainingInsulinUnits
        '
        Me.RemainingInsulinUnits.AutoSize = True
        Me.RemainingInsulinUnits.BackColor = System.Drawing.Color.Transparent
        Me.RemainingInsulinUnits.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.RemainingInsulinUnits.ForeColor = System.Drawing.Color.White
        Me.RemainingInsulinUnits.Location = New System.Drawing.Point(594, 95)
        Me.RemainingInsulinUnits.Name = "RemainingInsulinUnits"
        Me.RemainingInsulinUnits.Size = New System.Drawing.Size(66, 21)
        Me.RemainingInsulinUnits.TabIndex = 12
        Me.RemainingInsulinUnits.Text = "000.0 U"
        Me.RemainingInsulinUnits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InsulinLevelPictureBox
        '
        Me.InsulinLevelPictureBox.Image = CType(resources.GetObject("InsulinLevelPictureBox.Image"), System.Drawing.Image)
        Me.InsulinLevelPictureBox.InitialImage = Nothing
        Me.InsulinLevelPictureBox.Location = New System.Drawing.Point(595, 6)
        Me.InsulinLevelPictureBox.Name = "InsulinLevelPictureBox"
        Me.InsulinLevelPictureBox.Size = New System.Drawing.Size(64, 74)
        Me.InsulinLevelPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.InsulinLevelPictureBox.TabIndex = 12
        Me.InsulinLevelPictureBox.TabStop = False
        '
        'ActiveInsulinValue
        '
        Me.ActiveInsulinValue.BackColor = System.Drawing.Color.Transparent
        Me.ActiveInsulinValue.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinValue.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinValue.Location = New System.Drawing.Point(1, 21)
        Me.ActiveInsulinValue.Name = "ActiveInsulinValue"
        Me.ActiveInsulinValue.Size = New System.Drawing.Size(114, 21)
        Me.ActiveInsulinValue.TabIndex = 0
        Me.ActiveInsulinValue.Text = "0.000 U"
        Me.ActiveInsulinValue.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CalibrationDueImage
        '
        Me.CalibrationDueImage.BackColor = System.Drawing.Color.Transparent
        Me.CalibrationDueImage.Image = Global.CareLink.My.Resources.Resources.CalibrationUnavailable
        Me.CalibrationDueImage.Location = New System.Drawing.Point(129, 6)
        Me.CalibrationDueImage.Name = "CalibrationDueImage"
        Me.CalibrationDueImage.Size = New System.Drawing.Size(47, 47)
        Me.CalibrationDueImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.CalibrationDueImage.TabIndex = 5
        Me.CalibrationDueImage.TabStop = False
        '
        'ActiveInsulinLabel
        '
        Me.ActiveInsulinLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinLabel.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinLabel.Location = New System.Drawing.Point(1, 0)
        Me.ActiveInsulinLabel.Name = "ActiveInsulinLabel"
        Me.ActiveInsulinLabel.Size = New System.Drawing.Size(114, 21)
        Me.ActiveInsulinLabel.TabIndex = 5
        Me.ActiveInsulinLabel.Text = "Active Insulin"
        Me.ActiveInsulinLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ShieldPictureBox
        '
        Me.ShieldPictureBox.Image = Global.CareLink.My.Resources.Resources.Shield
        Me.ShieldPictureBox.Location = New System.Drawing.Point(389, 3)
        Me.ShieldPictureBox.Name = "ShieldPictureBox"
        Me.ShieldPictureBox.Size = New System.Drawing.Size(100, 100)
        Me.ShieldPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ShieldPictureBox.TabIndex = 4
        Me.ShieldPictureBox.TabStop = False
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
        Me.SplitContainer3.Panel1.Controls.Add(Me.CursorTimeLabel)
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
        Me.SplitContainer3.Size = New System.Drawing.Size(1370, 471)
        Me.SplitContainer3.SplitterDistance = 1136
        Me.SplitContainer3.TabIndex = 0
        '
        'CursorTimeLabel
        '
        Me.CursorTimeLabel.AutoSize = True
        Me.CursorTimeLabel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CursorTimeLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorTimeLabel.ForeColor = System.Drawing.Color.Black
        Me.CursorTimeLabel.Location = New System.Drawing.Point(176, 5)
        Me.CursorTimeLabel.Name = "CursorTimeLabel"
        Me.CursorTimeLabel.Size = New System.Drawing.Size(99, 17)
        Me.CursorTimeLabel.TabIndex = 15
        Me.CursorTimeLabel.Text = "TimeForCursor"
        Me.CursorTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorTimeLabel.Visible = False
        '
        'Last24HoursLabel
        '
        Me.Last24HoursLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Last24HoursLabel.AutoSize = True
        Me.Last24HoursLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Last24HoursLabel.ForeColor = System.Drawing.Color.White
        Me.Last24HoursLabel.Location = New System.Drawing.Point(70, 29)
        Me.Last24HoursLabel.Name = "Last24HoursLabel"
        Me.Last24HoursLabel.Size = New System.Drawing.Size(90, 17)
        Me.Last24HoursLabel.TabIndex = 34
        Me.Last24HoursLabel.Text = "Last 24 hours"
        '
        'TimeInRangeLabel
        '
        Me.TimeInRangeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TimeInRangeLabel.AutoSize = True
        Me.TimeInRangeLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeLabel.Location = New System.Drawing.Point(74, 7)
        Me.TimeInRangeLabel.Name = "TimeInRangeLabel"
        Me.TimeInRangeLabel.Size = New System.Drawing.Size(83, 15)
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
        'TabPage2RunningActiveInsulin
        '
        Me.TabPage2RunningActiveInsulin.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2RunningActiveInsulin.Name = "TabPage2RunningActiveInsulin"
        Me.TabPage2RunningActiveInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2RunningActiveInsulin.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage2RunningActiveInsulin.TabIndex = 8
        Me.TabPage2RunningActiveInsulin.Text = "Running Active Insulin"
        Me.TabPage2RunningActiveInsulin.UseVisualStyleBackColor = True
        '
        'TabPage3SummaryData
        '
        Me.TabPage3SummaryData.Controls.Add(Me.TableLayoutPanelSummaryData)
        Me.TabPage3SummaryData.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3SummaryData.Name = "TabPage3SummaryData"
        Me.TabPage3SummaryData.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3SummaryData.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage3SummaryData.TabIndex = 0
        Me.TabPage3SummaryData.Text = "Summary Data"
        Me.TabPage3SummaryData.UseVisualStyleBackColor = True
        '
        'TabPage4ActiveInsulin
        '
        Me.TabPage4ActiveInsulin.Controls.Add(Me.TableLayoutPanelActiveInsulin)
        Me.TabPage4ActiveInsulin.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4ActiveInsulin.Name = "TabPage4ActiveInsulin"
        Me.TabPage4ActiveInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4ActiveInsulin.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage4ActiveInsulin.TabIndex = 1
        Me.TabPage4ActiveInsulin.Text = "Active Insulin"
        Me.TabPage4ActiveInsulin.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelActiveInsulin
        '
        Me.TableLayoutPanelActiveInsulin.AutoScroll = True
        Me.TableLayoutPanelActiveInsulin.AutoSize = True
        Me.TableLayoutPanelActiveInsulin.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelActiveInsulin.ColumnCount = 1
        Me.TableLayoutPanelActiveInsulin.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelActiveInsulin.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelActiveInsulin.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelActiveInsulin.Name = "TableLayoutPanelActiveInsulin"
        Me.TableLayoutPanelActiveInsulin.RowCount = 2
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelActiveInsulin.Size = New System.Drawing.Size(1370, 9)
        Me.TableLayoutPanelActiveInsulin.TabIndex = 0
        '
        'TabPage5SensorGlucose
        '
        Me.TabPage5SensorGlucose.Controls.Add(Me.SGsDataGridView)
        Me.TabPage5SensorGlucose.Location = New System.Drawing.Point(4, 24)
        Me.TabPage5SensorGlucose.Name = "TabPage5SensorGlucose"
        Me.TabPage5SensorGlucose.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5SensorGlucose.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage5SensorGlucose.TabIndex = 2
        Me.TabPage5SensorGlucose.Text = "Sensor Glucose"
        Me.TabPage5SensorGlucose.UseVisualStyleBackColor = True
        '
        'SGsDataGridView
        '
        Me.SGsDataGridView.AllowUserToAddRows = False
        Me.SGsDataGridView.AllowUserToDeleteRows = False
        Me.SGsDataGridView.AllowUserToResizeRows = False
        Me.SGsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.SGsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.SGsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SGsDataGridView.Location = New System.Drawing.Point(3, 3)
        Me.SGsDataGridView.Name = "SGsDataGridView"
        Me.SGsDataGridView.RowTemplate.Height = 25
        Me.SGsDataGridView.Size = New System.Drawing.Size(1370, 611)
        Me.SGsDataGridView.TabIndex = 1
        '
        'TabPage6Limits
        '
        Me.TabPage6Limits.Controls.Add(Me.TableLayoutPanelLimits)
        Me.TabPage6Limits.Location = New System.Drawing.Point(4, 24)
        Me.TabPage6Limits.Name = "TabPage6Limits"
        Me.TabPage6Limits.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6Limits.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage6Limits.TabIndex = 3
        Me.TabPage6Limits.Text = "Limits"
        Me.TabPage6Limits.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelLimits
        '
        Me.TableLayoutPanelLimits.AutoScroll = True
        Me.TableLayoutPanelLimits.AutoSize = True
        Me.TableLayoutPanelLimits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLimits.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLimits.ColumnCount = 1
        Me.TableLayoutPanelLimits.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLimits.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelLimits.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelLimits.Name = "TableLayoutPanelLimits"
        Me.TableLayoutPanelLimits.RowCount = 2
        Me.TableLayoutPanelLimits.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelLimits.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelLimits.Size = New System.Drawing.Size(1370, 9)
        Me.TableLayoutPanelLimits.TabIndex = 0
        '
        'TabPage7Markers
        '
        Me.TabPage7Markers.Controls.Add(Me.TableLayoutPanelMarkers)
        Me.TabPage7Markers.Location = New System.Drawing.Point(4, 24)
        Me.TabPage7Markers.Name = "TabPage7Markers"
        Me.TabPage7Markers.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7Markers.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage7Markers.TabIndex = 4
        Me.TabPage7Markers.Text = "Markers"
        Me.TabPage7Markers.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelMarkers
        '
        Me.TableLayoutPanelMarkers.AutoScroll = True
        Me.TableLayoutPanelMarkers.AutoSize = True
        Me.TableLayoutPanelMarkers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelMarkers.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelMarkers.ColumnCount = 1
        Me.TableLayoutPanelMarkers.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMarkers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMarkers.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelMarkers.Name = "TableLayoutPanelMarkers"
        Me.TableLayoutPanelMarkers.RowCount = 1
        Me.TableLayoutPanelMarkers.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelMarkers.Size = New System.Drawing.Size(1370, 611)
        Me.TableLayoutPanelMarkers.TabIndex = 0
        '
        'TabPage8NotificationHistory
        '
        Me.TabPage8NotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistory)
        Me.TabPage8NotificationHistory.Location = New System.Drawing.Point(4, 24)
        Me.TabPage8NotificationHistory.Name = "TabPage8NotificationHistory"
        Me.TabPage8NotificationHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8NotificationHistory.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage8NotificationHistory.TabIndex = 5
        Me.TabPage8NotificationHistory.Text = "Notification History"
        Me.TabPage8NotificationHistory.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelNotificationHistory
        '
        Me.TableLayoutPanelNotificationHistory.AutoScroll = True
        Me.TableLayoutPanelNotificationHistory.AutoSize = True
        Me.TableLayoutPanelNotificationHistory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelNotificationHistory.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelNotificationHistory.ColumnCount = 1
        Me.TableLayoutPanelNotificationHistory.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelNotificationHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelNotificationHistory.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelNotificationHistory.Name = "TableLayoutPanelNotificationHistory"
        Me.TableLayoutPanelNotificationHistory.RowCount = 2
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelNotificationHistory.Size = New System.Drawing.Size(1370, 611)
        Me.TableLayoutPanelNotificationHistory.TabIndex = 0
        '
        'TabPage9Basal
        '
        Me.TabPage9Basal.Controls.Add(Me.TableLayoutPanelBasal)
        Me.TabPage9Basal.Location = New System.Drawing.Point(4, 24)
        Me.TabPage9Basal.Name = "TabPage9Basal"
        Me.TabPage9Basal.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9Basal.Size = New System.Drawing.Size(1376, 617)
        Me.TabPage9Basal.TabIndex = 6
        Me.TabPage9Basal.Text = "Basal"
        Me.TabPage9Basal.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelBasal
        '
        Me.TableLayoutPanelBasal.AutoScroll = True
        Me.TableLayoutPanelBasal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBasal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBasal.ColumnCount = 1
        Me.TableLayoutPanelBasal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBasal.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelBasal.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelBasal.Name = "TableLayoutPanelBasal"
        Me.TableLayoutPanelBasal.RowCount = 2
        Me.TableLayoutPanelBasal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelBasal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelBasal.Size = New System.Drawing.Size(1370, 379)
        Me.TableLayoutPanelBasal.TabIndex = 0
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
        Me.LoginStatusLabel.Location = New System.Drawing.Point(600, 3)
        Me.LoginStatusLabel.Name = "LoginStatusLabel"
        Me.LoginStatusLabel.Size = New System.Drawing.Size(75, 15)
        Me.LoginStatusLabel.TabIndex = 23
        Me.LoginStatusLabel.Text = "Login Status:"
        '
        'LoginStatus
        '
        Me.LoginStatus.AutoSize = True
        Me.LoginStatus.Location = New System.Drawing.Point(700, 3)
        Me.LoginStatus.Name = "LoginStatus"
        Me.LoginStatus.Size = New System.Drawing.Size(58, 15)
        Me.LoginStatus.TabIndex = 24
        Me.LoginStatus.Text = "Unknown"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1384, 826)
        Me.Controls.Add(Me.LoginStatusLabel)
        Me.Controls.Add(Me.LoginStatus)
        Me.Controls.Add(Me.SplitContainer1)
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
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer4.Panel1.ResumeLayout(False)
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1HomePage.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.TransmitterBatteryPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SensorTimeLeftPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PumpBatteryPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CursorPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InsulinLevelPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CalibrationDueImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShieldPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.Panel2.PerformLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.TabPage3SummaryData.ResumeLayout(False)
        Me.TabPage3SummaryData.PerformLayout()
        Me.TabPage4ActiveInsulin.ResumeLayout(False)
        Me.TabPage4ActiveInsulin.PerformLayout()
        Me.TabPage5SensorGlucose.ResumeLayout(False)
        CType(Me.SGsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage6Limits.ResumeLayout(False)
        Me.TabPage6Limits.PerformLayout()
        Me.TabPage7Markers.ResumeLayout(False)
        Me.TabPage7Markers.PerformLayout()
        Me.TabPage8NotificationHistory.ResumeLayout(False)
        Me.TabPage8NotificationHistory.PerformLayout()
        Me.TabPage9Basal.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuHelpAbout As ToolStripMenuItem
    Friend WithEvents AboveHighLimitMessageLabel As Label
    Friend WithEvents AboveHighLimitValueLabel As Label
    Friend WithEvents ActiveInsulinLabel As Label
    Friend WithEvents ActiveInsulinValue As Label
    Friend WithEvents AITComboBox As ComboBox
    Friend WithEvents AITLabel As Label
    Friend WithEvents AverageSGValueLabel As Label
    Friend WithEvents BelowLowLimitValueLabel As Label
    Friend WithEvents CalibrationDueImage As PictureBox
    Friend WithEvents CurrentBG As Label
    Friend WithEvents CursorMessage1Label As Label
    Friend WithEvents CursorMessage2Label As Label
    Friend WithEvents CursorPictureBox As PictureBox
    Friend WithEvents CursorTimeLabel As Label
    Friend WithEvents CursorTimer As Timer
    Friend WithEvents CursorValueLabel As Label
    Friend WithEvents StartHereExit As ToolStripMenuItem
    Friend WithEvents MenuOptionsFilterRawJSONData As ToolStripMenuItem
    Friend WithEvents MenuHelp As ToolStripMenuItem
    Friend WithEvents MenuHelpCheckForUpdatesMenuItem As ToolStripMenuItem
    Friend WithEvents MenuHelpReportAProblem As ToolStripMenuItem
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents InRangeMessageLabel As Label
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents ListView1 As ListView
    Friend WithEvents MenuStartHereLogin As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents MenuOptions As ToolStripMenuItem
    Friend WithEvents PumpBatteryPictureBox As PictureBox
    Friend WithEvents PumpBatteryRemainingLabel As Label
    Friend WithEvents RemainingInsulinUnits As Label
    Friend WithEvents SensorDaysLeftLabel As Label
    Friend WithEvents SensorMessage As Label
    Friend WithEvents SensorTimeLeftPictureBox As PictureBox
    Friend WithEvents SensorTimeLeftLabel As Label
    Friend WithEvents LoginStatus As Label
    Friend WithEvents LoginStatusLabel As Label
    Friend WithEvents ServerUpdateTimer As Timer
    Friend WithEvents MenuOptionsSetupEmailServer As ToolStripMenuItem
    Friend WithEvents SGsDataGridView As DataGridView
    Friend WithEvents ShieldPictureBox As PictureBox
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents MenuViewShowMiniDisplay As ToolStripMenuItem
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents MenuStartHere As ToolStripMenuItem
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TableLayoutPanelActiveInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBasal As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLimits As TableLayoutPanel
    Friend WithEvents TableLayoutPanelMarkers As TableLayoutPanel
    Friend WithEvents TableLayoutPanelNotificationHistory As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSummaryData As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTop1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanelTop2 As TableLayoutPanel
    Friend WithEvents TabPage1HomePage As TabPage
    Friend WithEvents TabPage2RunningActiveInsulin As TabPage
    Friend WithEvents TabPage3SummaryData As TabPage
    Friend WithEvents TabPage4ActiveInsulin As TabPage
    Friend WithEvents TabPage5SensorGlucose As TabPage
    Friend WithEvents TabPage6Limits As TabPage
    Friend WithEvents TabPage7Markers As TabPage
    Friend WithEvents TabPage8NotificationHistory As TabPage
    Friend WithEvents TabPage9Basal As TabPage
    Friend WithEvents TimeInRangeChartLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents TransmatterBatterPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents MenuOptionsUseTestData As ToolStripMenuItem
    Friend WithEvents MenuView As ToolStripMenuItem
    Friend WithEvents MenuOptionsUseLastSavedData As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents MenuStartHereLoadSavedDataFile As ToolStripMenuItem
    Friend WithEvents MenuStartHereSnapshotSave As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents AutoCorrectionLabel As Label
    Friend WithEvents ManualBolusLabel As Label
    Friend WithEvents BasalLabel As Label
    Friend WithEvents DailyDoseLabel As Label
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents Last24HoursLabel As Label
    Friend WithEvents TimeInRangeLabel As Label
    Friend WithEvents SplitContainer4 As SplitContainer
    Friend WithEvents TotalsLabel As Label
    Friend WithEvents ReadingIntervalLabel As Label
    Friend WithEvents ReadingsLabel As Label
    Friend WithEvents ModelLabel As Label
    Friend WithEvents SerialNumberLabel As Label
    Friend WithEvents FullNameLabel As Label
    Friend WithEvents BelowLowLimitMessageLabel As Label
    Friend WithEvents AverageSGMessageLabel As Label
    Friend WithEvents Last24CarbsValueLabel As Label
    Friend WithEvents TotalCarbs24MessageLabel As Label
    Friend WithEvents TotalCarbsMessageLabel As Label
    Friend WithEvents MenuOptionsUseAdvancedAITDecay As ToolStripMenuItem
    Friend WithEvents MenuStartHereExceptionReportLoadToolStripMenuItem As ToolStripMenuItem
End Class
