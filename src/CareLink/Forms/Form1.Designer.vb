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
        Me.StartHereToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartHereLoginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.StartHereSnapshotLoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartHereSnapshotSaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.StartHereExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsFilterRawJSONDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsSetupEmailServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsUseTestDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsUseLastSavedDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewShowMiniDisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpAboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpReportIssueMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuHelpCheckForUpdatesMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerUpdateTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanelSummaryData = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelTop1 = New System.Windows.Forms.TableLayoutPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanelTop2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1HomePage = New System.Windows.Forms.TabPage()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.TotalAutoCorrectionLabel = New System.Windows.Forms.Label()
        Me.TotalManualBolusLabel = New System.Windows.Forms.Label()
        Me.TotalBasalLabel = New System.Windows.Forms.Label()
        Me.TotalDailyDoseLabel = New System.Windows.Forms.Label()
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
        Me.ShieldUnitsLabel = New System.Windows.Forms.Label()
        Me.SensorMessage = New System.Windows.Forms.Label()
        Me.RemainingInsulinUnits = New System.Windows.Forms.Label()
        Me.InsulinLevelPictureBox = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinValue = New System.Windows.Forms.Label()
        Me.CalibrationDueImage = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinLabel = New System.Windows.Forms.Label()
        Me.CurrentBG = New System.Windows.Forms.Label()
        Me.ShieldPictureBox = New System.Windows.Forms.PictureBox()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.CursorTimeLabel = New System.Windows.Forms.Label()
        Me.Last24HoursLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeSummaryPercentCharLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeSummaryLabel = New System.Windows.Forms.Label()
        Me.InRangeMessageLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitMessageLabel = New System.Windows.Forms.Label()
        Me.BelowLowLimitValueLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeValueLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitValueLabel = New System.Windows.Forms.Label()
        Me.BelowLowLimitMessageLabel = New System.Windows.Forms.Label()
        Me.AverageSGUnitsLabel = New System.Windows.Forms.Label()
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
        Me.WatchdogTimer = New System.Windows.Forms.Timer(Me.components)
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
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartHereToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.ViewToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1384, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'StartHereToolStripMenuItem
        '
        Me.StartHereToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartHereLoginToolStripMenuItem, Me.ToolStripSeparator1, Me.StartHereSnapshotLoadToolStripMenuItem, Me.StartHereSnapshotSaveToolStripMenuItem, Me.ToolStripSeparator2, Me.StartHereExitToolStripMenuItem})
        Me.StartHereToolStripMenuItem.Name = "StartHereToolStripMenuItem"
        Me.StartHereToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
        Me.StartHereToolStripMenuItem.Text = "Start Here"
        '
        'StartHereLoginToolStripMenuItem
        '
        Me.StartHereLoginToolStripMenuItem.Name = "StartHereLoginToolStripMenuItem"
        Me.StartHereLoginToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.StartHereLoginToolStripMenuItem.Text = "Login"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(187, 6)
        '
        'StartHereSnapshotLoadToolStripMenuItem
        '
        Me.StartHereSnapshotLoadToolStripMenuItem.Name = "StartHereSnapshotLoadToolStripMenuItem"
        Me.StartHereSnapshotLoadToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.StartHereSnapshotLoadToolStripMenuItem.Text = "Snapshot Load"
        '
        'StartHereSnapshotSaveToolStripMenuItem
        '
        Me.StartHereSnapshotSaveToolStripMenuItem.Name = "StartHereSnapshotSaveToolStripMenuItem"
        Me.StartHereSnapshotSaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.StartHereSnapshotSaveToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.StartHereSnapshotSaveToolStripMenuItem.Text = "Snapshot &Save"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(187, 6)
        '
        'StartHereExitToolStripMenuItem
        '
        Me.StartHereExitToolStripMenuItem.Image = Global.CareLink.My.Resources.Resources.AboutBox
        Me.StartHereExitToolStripMenuItem.Name = "StartHereExitToolStripMenuItem"
        Me.StartHereExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.StartHereExitToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.StartHereExitToolStripMenuItem.Text = "E&xit"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsFilterRawJSONDataToolStripMenuItem, Me.OptionsSetupEmailServerToolStripMenuItem, Me.OptionsUseTestDataToolStripMenuItem, Me.OptionsUseLastSavedDataToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'OptionsFilterRawJSONDataToolStripMenuItem
        '
        Me.OptionsFilterRawJSONDataToolStripMenuItem.Checked = True
        Me.OptionsFilterRawJSONDataToolStripMenuItem.CheckOnClick = True
        Me.OptionsFilterRawJSONDataToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.OptionsFilterRawJSONDataToolStripMenuItem.Name = "OptionsFilterRawJSONDataToolStripMenuItem"
        Me.OptionsFilterRawJSONDataToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OptionsFilterRawJSONDataToolStripMenuItem.Text = "Filter Raw JSON Data"
        '
        'OptionsSetupEmailServerToolStripMenuItem
        '
        Me.OptionsSetupEmailServerToolStripMenuItem.Name = "OptionsSetupEmailServerToolStripMenuItem"
        Me.OptionsSetupEmailServerToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OptionsSetupEmailServerToolStripMenuItem.Text = "Setup Email Server"
        '
        'OptionsUseTestDataToolStripMenuItem
        '
        Me.OptionsUseTestDataToolStripMenuItem.CheckOnClick = True
        Me.OptionsUseTestDataToolStripMenuItem.Name = "OptionsUseTestDataToolStripMenuItem"
        Me.OptionsUseTestDataToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OptionsUseTestDataToolStripMenuItem.Text = "Use Test Data"
        '
        'OptionsUseLastSavedDataToolStripMenuItem
        '
        Me.OptionsUseLastSavedDataToolStripMenuItem.CheckOnClick = True
        Me.OptionsUseLastSavedDataToolStripMenuItem.Name = "OptionsUseLastSavedDataToolStripMenuItem"
        Me.OptionsUseLastSavedDataToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.OptionsUseLastSavedDataToolStripMenuItem.Text = "Use Last Saved Data"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewShowMiniDisplayToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        Me.ViewToolStripMenuItem.Visible = False
        '
        'ViewShowMiniDisplayToolStripMenuItem
        '
        Me.ViewShowMiniDisplayToolStripMenuItem.Name = "ViewShowMiniDisplayToolStripMenuItem"
        Me.ViewShowMiniDisplayToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.ViewShowMiniDisplayToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ViewShowMiniDisplayToolStripMenuItem.Text = "Show Mini Display"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuHelpReportIssueMenuItem, Me.MenuHelpCheckForUpdatesMenuItem, Me.HelpAboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'HelpAboutToolStripMenuItem
        '
        Me.HelpAboutToolStripMenuItem.Image = Global.CareLink.My.Resources.Resources.AboutBox
        Me.HelpAboutToolStripMenuItem.Name = "HelpAboutToolStripMenuItem"
        Me.HelpAboutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HelpAboutToolStripMenuItem.Text = "&About..."
        '
        'MenuHelpReportIssueMenuItem
        '
        Me.MenuHelpReportIssueMenuItem.Image = Global.CareLink.My.Resources.Resources.FeedbackSmile_16x
        Me.MenuHelpReportIssueMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MenuHelpReportIssueMenuItem.Name = "MenuHelpReportIssueMenuItem"
        Me.MenuHelpReportIssueMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.MenuHelpReportIssueMenuItem.Text = "Report A Problem..."
        '
        'MenuHelpCheckForUpdatesMenuItem
        '
        Me.MenuHelpCheckForUpdatesMenuItem.Name = "MenuHelpCheckForUpdatesMenuItem"
        Me.MenuHelpCheckForUpdatesMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.MenuHelpCheckForUpdatesMenuItem.Text = "Check For Updates"
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
        Me.TableLayoutPanelSummaryData.Size = New System.Drawing.Size(1370, 706)
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
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1384, 740)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1HomePage
        '
        Me.TabPage1HomePage.BackColor = System.Drawing.Color.Black
        Me.TabPage1HomePage.Controls.Add(Me.SplitContainer2)
        Me.TabPage1HomePage.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1HomePage.Name = "TabPage1HomePage"
        Me.TabPage1HomePage.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1HomePage.Size = New System.Drawing.Size(1376, 712)
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
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalAutoCorrectionLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalManualBolusLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalBasalLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.TotalDailyDoseLabel)
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
        Me.SplitContainer2.Panel1.Controls.Add(Me.ShieldUnitsLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SensorMessage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.RemainingInsulinUnits)
        Me.SplitContainer2.Panel1.Controls.Add(Me.InsulinLevelPictureBox)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ActiveInsulinValue)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CalibrationDueImage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ActiveInsulinLabel)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CurrentBG)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ShieldPictureBox)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Size = New System.Drawing.Size(1370, 706)
        Me.SplitContainer2.SplitterDistance = 140
        Me.SplitContainer2.TabIndex = 52
        '
        'TotalAutoCorrectionLabel
        '
        Me.TotalAutoCorrectionLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalAutoCorrectionLabel.ForeColor = System.Drawing.Color.White
        Me.TotalAutoCorrectionLabel.Location = New System.Drawing.Point(1057, 94)
        Me.TotalAutoCorrectionLabel.Name = "TotalAutoCorrectionLabel"
        Me.TotalAutoCorrectionLabel.Size = New System.Drawing.Size(287, 21)
        Me.TotalAutoCorrectionLabel.TabIndex = 51
        Me.TotalAutoCorrectionLabel.Text = "Total Autocorrection 20 U | 50%"
        Me.TotalAutoCorrectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalManualBolusLabel
        '
        Me.TotalManualBolusLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalManualBolusLabel.ForeColor = System.Drawing.Color.White
        Me.TotalManualBolusLabel.Location = New System.Drawing.Point(1057, 64)
        Me.TotalManualBolusLabel.Name = "TotalManualBolusLabel"
        Me.TotalManualBolusLabel.Size = New System.Drawing.Size(287, 21)
        Me.TotalManualBolusLabel.TabIndex = 50
        Me.TotalManualBolusLabel.Text = "Total Manual Bolus 30 U | 30%"
        Me.TotalManualBolusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalBasalLabel
        '
        Me.TotalBasalLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalBasalLabel.ForeColor = System.Drawing.Color.White
        Me.TotalBasalLabel.Location = New System.Drawing.Point(1057, 34)
        Me.TotalBasalLabel.Name = "TotalBasalLabel"
        Me.TotalBasalLabel.Size = New System.Drawing.Size(287, 21)
        Me.TotalBasalLabel.TabIndex = 49
        Me.TotalBasalLabel.Text = "Total Basal 50 U | 50%"
        Me.TotalBasalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalDailyDoseLabel
        '
        Me.TotalDailyDoseLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TotalDailyDoseLabel.ForeColor = System.Drawing.Color.White
        Me.TotalDailyDoseLabel.Location = New System.Drawing.Point(1057, 4)
        Me.TotalDailyDoseLabel.Name = "TotalDailyDoseLabel"
        Me.TotalDailyDoseLabel.Size = New System.Drawing.Size(287, 21)
        Me.TotalDailyDoseLabel.TabIndex = 48
        Me.TotalDailyDoseLabel.Text = "TDD 100 U"
        Me.TotalDailyDoseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PumpBatteryRemainingLabel
        '
        Me.PumpBatteryRemainingLabel.BackColor = System.Drawing.Color.Transparent
        Me.PumpBatteryRemainingLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.PumpBatteryRemainingLabel.ForeColor = System.Drawing.Color.White
        Me.PumpBatteryRemainingLabel.Location = New System.Drawing.Point(507, 95)
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
        Me.TransmatterBatterPercentLabel.Location = New System.Drawing.Point(692, 95)
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
        Me.TransmitterBatteryPictureBox.Location = New System.Drawing.Point(685, 6)
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
        Me.SensorTimeLeftLabel.Location = New System.Drawing.Point(757, 95)
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
        Me.SensorDaysLeftLabel.Location = New System.Drawing.Point(785, 19)
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
        Me.SensorTimeLeftPictureBox.Location = New System.Drawing.Point(773, 6)
        Me.SensorTimeLeftPictureBox.Name = "SensorTimeLeftPictureBox"
        Me.SensorTimeLeftPictureBox.Size = New System.Drawing.Size(68, 78)
        Me.SensorTimeLeftPictureBox.TabIndex = 44
        Me.SensorTimeLeftPictureBox.TabStop = False
        '
        'PumpBatteryPictureBox
        '
        Me.PumpBatteryPictureBox.ErrorImage = Nothing
        Me.PumpBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.PumpBatteryFull
        Me.PumpBatteryPictureBox.Location = New System.Drawing.Point(515, 6)
        Me.PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        Me.PumpBatteryPictureBox.Size = New System.Drawing.Size(64, 74)
        Me.PumpBatteryPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PumpBatteryPictureBox.TabIndex = 43
        Me.PumpBatteryPictureBox.TabStop = False
        '
        'AITLabel
        '
        Me.AITLabel.AutoSize = True
        Me.AITLabel.BackColor = System.Drawing.Color.Transparent
        Me.AITLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AITLabel.ForeColor = System.Drawing.Color.White
        Me.AITLabel.Location = New System.Drawing.Point(863, 44)
        Me.AITLabel.Name = "AITLabel"
        Me.AITLabel.Size = New System.Drawing.Size(156, 21)
        Me.AITLabel.TabIndex = 8
        Me.AITLabel.Text = "Active Insulin TIme"
        '
        'AITComboBox
        '
        Me.AITComboBox.BackColor = System.Drawing.Color.Black
        Me.AITComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AITComboBox.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AITComboBox.ForeColor = System.Drawing.Color.White
        Me.AITComboBox.Items.AddRange(New Object() {"2:00", "2:15", "2:30", "2:45", "3:00", "3:15", "3:30", "3:45", "4:00", "4:15", "4:30", "4:45", "5:00", "5:15", "5:30", "5:45", "6:00"})
        Me.AITComboBox.Location = New System.Drawing.Point(907, 6)
        Me.AITComboBox.Name = "AITComboBox"
        Me.AITComboBox.Size = New System.Drawing.Size(68, 23)
        Me.AITComboBox.TabIndex = 0
        '
        'CursorMessage2Label
        '
        Me.CursorMessage2Label.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage2Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage2Label.Location = New System.Drawing.Point(138, 74)
        Me.CursorMessage2Label.Name = "CursorMessage2Label"
        Me.CursorMessage2Label.Size = New System.Drawing.Size(235, 15)
        Me.CursorMessage2Label.TabIndex = 9
        Me.CursorMessage2Label.Text = "Message For Cursor 2"
        Me.CursorMessage2Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorMessage2Label.Visible = False
        '
        'CursorValueLabel
        '
        Me.CursorValueLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorValueLabel.ForeColor = System.Drawing.Color.White
        Me.CursorValueLabel.Location = New System.Drawing.Point(193, 93)
        Me.CursorValueLabel.Name = "CursorValueLabel"
        Me.CursorValueLabel.Size = New System.Drawing.Size(125, 15)
        Me.CursorValueLabel.TabIndex = 10
        Me.CursorValueLabel.Text = "Value For Cursor"
        Me.CursorValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorValueLabel.Visible = False
        '
        'CursorPictureBox
        '
        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.MealImageLarge
        Me.CursorPictureBox.InitialImage = Nothing
        Me.CursorPictureBox.Location = New System.Drawing.Point(236, 6)
        Me.CursorPictureBox.Name = "CursorPictureBox"
        Me.CursorPictureBox.Size = New System.Drawing.Size(39, 45)
        Me.CursorPictureBox.TabIndex = 38
        Me.CursorPictureBox.TabStop = False
        Me.CursorPictureBox.Visible = False
        '
        'CursorMessage1Label
        '
        Me.CursorMessage1Label.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage1Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage1Label.Location = New System.Drawing.Point(193, 55)
        Me.CursorMessage1Label.Name = "CursorMessage1Label"
        Me.CursorMessage1Label.Size = New System.Drawing.Size(125, 15)
        Me.CursorMessage1Label.TabIndex = 6
        Me.CursorMessage1Label.Text = "Message For Cursor 1"
        Me.CursorMessage1Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorMessage1Label.Visible = False
        '
        'ShieldUnitsLabel
        '
        Me.ShieldUnitsLabel.AutoSize = True
        Me.ShieldUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ShieldUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.ShieldUnitsLabel.Location = New System.Drawing.Point(427, 59)
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
        Me.SensorMessage.Location = New System.Drawing.Point(394, 14)
        Me.SensorMessage.Name = "SensorMessage"
        Me.SensorMessage.Size = New System.Drawing.Size(100, 66)
        Me.SensorMessage.TabIndex = 1
        Me.SensorMessage.Text = "Calibration Required"
        Me.SensorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RemainingInsulinUnits
        '
        Me.RemainingInsulinUnits.AutoSize = True
        Me.RemainingInsulinUnits.BackColor = System.Drawing.Color.Transparent
        Me.RemainingInsulinUnits.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.RemainingInsulinUnits.ForeColor = System.Drawing.Color.White
        Me.RemainingInsulinUnits.Location = New System.Drawing.Point(599, 95)
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
        Me.InsulinLevelPictureBox.Location = New System.Drawing.Point(600, 6)
        Me.InsulinLevelPictureBox.Name = "InsulinLevelPictureBox"
        Me.InsulinLevelPictureBox.Size = New System.Drawing.Size(64, 74)
        Me.InsulinLevelPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.InsulinLevelPictureBox.TabIndex = 12
        Me.InsulinLevelPictureBox.TabStop = False
        '
        'ActiveInsulinValue
        '
        Me.ActiveInsulinValue.AutoSize = True
        Me.ActiveInsulinValue.BackColor = System.Drawing.Color.Transparent
        Me.ActiveInsulinValue.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinValue.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinValue.Location = New System.Drawing.Point(25, 6)
        Me.ActiveInsulinValue.Name = "ActiveInsulinValue"
        Me.ActiveInsulinValue.Size = New System.Drawing.Size(66, 21)
        Me.ActiveInsulinValue.TabIndex = 0
        Me.ActiveInsulinValue.Text = "0.000 U"
        Me.ActiveInsulinValue.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.ActiveInsulinLabel.AutoSize = True
        Me.ActiveInsulinLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinLabel.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinLabel.Location = New System.Drawing.Point(1, 55)
        Me.ActiveInsulinLabel.Name = "ActiveInsulinLabel"
        Me.ActiveInsulinLabel.Size = New System.Drawing.Size(114, 21)
        Me.ActiveInsulinLabel.TabIndex = 5
        Me.ActiveInsulinLabel.Text = "Active Insulin"
        Me.ActiveInsulinLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CurrentBG
        '
        Me.CurrentBG.AutoSize = True
        Me.CurrentBG.BackColor = System.Drawing.Color.Transparent
        Me.CurrentBG.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CurrentBG.ForeColor = System.Drawing.Color.White
        Me.CurrentBG.Location = New System.Drawing.Point(422, 23)
        Me.CurrentBG.Name = "CurrentBG"
        Me.CurrentBG.Size = New System.Drawing.Size(44, 32)
        Me.CurrentBG.TabIndex = 3
        Me.CurrentBG.Text = "---"
        Me.CurrentBG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ShieldPictureBox
        '
        Me.ShieldPictureBox.Image = Global.CareLink.My.Resources.Resources.Shield
        Me.ShieldPictureBox.Location = New System.Drawing.Point(394, 3)
        Me.ShieldPictureBox.Name = "ShieldPictureBox"
        Me.ShieldPictureBox.Size = New System.Drawing.Size(100, 100)
        Me.ShieldPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ShieldPictureBox.TabIndex = 4
        Me.ShieldPictureBox.TabStop = False
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.IsSplitterFixed = True
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.CursorTimeLabel)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.Last24HoursLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeSummaryPercentCharLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeSummaryLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.InRangeMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AboveHighLimitMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.BelowLowLimitValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AboveHighLimitValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.BelowLowLimitMessageLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AverageSGUnitsLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AverageSGValueLabel)
        Me.SplitContainer3.Panel2.Controls.Add(Me.AverageSGMessageLabel)
        Me.SplitContainer3.Size = New System.Drawing.Size(1370, 562)
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
        Me.Last24HoursLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Last24HoursLabel.AutoSize = True
        Me.Last24HoursLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Last24HoursLabel.ForeColor = System.Drawing.Color.White
        Me.Last24HoursLabel.Location = New System.Drawing.Point(75, 37)
        Me.Last24HoursLabel.Name = "Last24HoursLabel"
        Me.Last24HoursLabel.Size = New System.Drawing.Size(90, 17)
        Me.Last24HoursLabel.TabIndex = 34
        Me.Last24HoursLabel.Text = "Last 24 hours"
        '
        'TimeInRangeLabel
        '
        Me.TimeInRangeLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeLabel.AutoSize = True
        Me.TimeInRangeLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeLabel.Location = New System.Drawing.Point(76, 15)
        Me.TimeInRangeLabel.Name = "TimeInRangeLabel"
        Me.TimeInRangeLabel.Size = New System.Drawing.Size(83, 15)
        Me.TimeInRangeLabel.TabIndex = 33
        Me.TimeInRangeLabel.Text = "Time in range"
        '
        'TimeInRangeSummaryPercentCharLabel
        '
        Me.TimeInRangeSummaryPercentCharLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeSummaryPercentCharLabel.AutoSize = True
        Me.TimeInRangeSummaryPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.TimeInRangeSummaryPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryPercentCharLabel.Location = New System.Drawing.Point(98, 182)
        Me.TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        Me.TimeInRangeSummaryPercentCharLabel.Size = New System.Drawing.Size(42, 40)
        Me.TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        Me.TimeInRangeSummaryPercentCharLabel.Text = "%"
        '
        'TimeInRangeSummaryLabel
        '
        Me.TimeInRangeSummaryLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeSummaryLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeSummaryLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryLabel.Location = New System.Drawing.Point(77, 135)
        Me.TimeInRangeSummaryLabel.Name = "TimeInRangeSummaryLabel"
        Me.TimeInRangeSummaryLabel.Size = New System.Drawing.Size(77, 47)
        Me.TimeInRangeSummaryLabel.TabIndex = 2
        Me.TimeInRangeSummaryLabel.Text = "100"
        Me.TimeInRangeSummaryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InRangeMessageLabel
        '
        Me.InRangeMessageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InRangeMessageLabel.AutoSize = True
        Me.InRangeMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.InRangeMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.InRangeMessageLabel.ForeColor = System.Drawing.Color.Lime
        Me.InRangeMessageLabel.Location = New System.Drawing.Point(79, 370)
        Me.InRangeMessageLabel.Name = "InRangeMessageLabel"
        Me.InRangeMessageLabel.Size = New System.Drawing.Size(73, 21)
        Me.InRangeMessageLabel.TabIndex = 30
        Me.InRangeMessageLabel.Text = "In range"
        Me.InRangeMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AboveHighLimitMessageLabel
        '
        Me.AboveHighLimitMessageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AboveHighLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AboveHighLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitMessageLabel.ForeColor = System.Drawing.Color.Orange
        Me.AboveHighLimitMessageLabel.Location = New System.Drawing.Point(30, 298)
        Me.AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        Me.AboveHighLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.AboveHighLimitMessageLabel.TabIndex = 28
        Me.AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        Me.AboveHighLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitValueLabel
        '
        Me.BelowLowLimitValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BelowLowLimitValueLabel.AutoSize = True
        Me.BelowLowLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.BelowLowLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.BelowLowLimitValueLabel.Location = New System.Drawing.Point(81, 411)
        Me.BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        Me.BelowLowLimitValueLabel.Size = New System.Drawing.Size(68, 33)
        Me.BelowLowLimitValueLabel.TabIndex = 26
        Me.BelowLowLimitValueLabel.Text = "2 %"
        Me.BelowLowLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimeInRangeValueLabel
        '
        Me.TimeInRangeValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeValueLabel.AutoSize = True
        Me.TimeInRangeValueLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeValueLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeValueLabel.Location = New System.Drawing.Point(73, 339)
        Me.TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        Me.TimeInRangeValueLabel.Size = New System.Drawing.Size(85, 33)
        Me.TimeInRangeValueLabel.TabIndex = 24
        Me.TimeInRangeValueLabel.Text = "90 %"
        Me.TimeInRangeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AboveHighLimitValueLabel
        '
        Me.AboveHighLimitValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AboveHighLimitValueLabel.AutoSize = True
        Me.AboveHighLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.AboveHighLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.AboveHighLimitValueLabel.Location = New System.Drawing.Point(81, 266)
        Me.AboveHighLimitValueLabel.Name = "AboveHighLimitValueLabel"
        Me.AboveHighLimitValueLabel.Size = New System.Drawing.Size(68, 33)
        Me.AboveHighLimitValueLabel.TabIndex = 22
        Me.AboveHighLimitValueLabel.Text = "8 %"
        Me.AboveHighLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitMessageLabel
        '
        Me.BelowLowLimitMessageLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BelowLowLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.BelowLowLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitMessageLabel.ForeColor = System.Drawing.Color.Red
        Me.BelowLowLimitMessageLabel.Location = New System.Drawing.Point(30, 442)
        Me.BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        Me.BelowLowLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.BelowLowLimitMessageLabel.TabIndex = 32
        Me.BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        Me.BelowLowLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGUnitsLabel
        '
        Me.AverageSGUnitsLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AverageSGUnitsLabel.AutoSize = True
        Me.AverageSGUnitsLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGUnitsLabel.Location = New System.Drawing.Point(87, 514)
        Me.AverageSGUnitsLabel.Name = "AverageSGUnitsLabel"
        Me.AverageSGUnitsLabel.Size = New System.Drawing.Size(57, 21)
        Me.AverageSGUnitsLabel.TabIndex = 16
        Me.AverageSGUnitsLabel.Text = "XX/XX"
        Me.AverageSGUnitsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGValueLabel
        '
        Me.AverageSGValueLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AverageSGValueLabel.BackColor = System.Drawing.Color.Black
        Me.AverageSGValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGValueLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGValueLabel.Location = New System.Drawing.Point(75, 477)
        Me.AverageSGValueLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.AverageSGValueLabel.Name = "AverageSGValueLabel"
        Me.AverageSGValueLabel.Size = New System.Drawing.Size(81, 33)
        Me.AverageSGValueLabel.TabIndex = 1
        Me.AverageSGValueLabel.Text = "100 %"
        Me.AverageSGValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGMessageLabel
        '
        Me.AverageSGMessageLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AverageSGMessageLabel.AutoSize = True
        Me.AverageSGMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGMessageLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGMessageLabel.Location = New System.Drawing.Point(67, 535)
        Me.AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        Me.AverageSGMessageLabel.Size = New System.Drawing.Size(97, 21)
        Me.AverageSGMessageLabel.TabIndex = 0
        Me.AverageSGMessageLabel.Text = "Average SG"
        Me.AverageSGMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage2RunningActiveInsulin
        '
        Me.TabPage2RunningActiveInsulin.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2RunningActiveInsulin.Name = "TabPage2RunningActiveInsulin"
        Me.TabPage2RunningActiveInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2RunningActiveInsulin.Size = New System.Drawing.Size(1376, 712)
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
        Me.TabPage3SummaryData.Size = New System.Drawing.Size(1376, 712)
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
        Me.TabPage4ActiveInsulin.Size = New System.Drawing.Size(1376, 712)
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
        Me.TabPage5SensorGlucose.Size = New System.Drawing.Size(1376, 712)
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
        Me.SGsDataGridView.Size = New System.Drawing.Size(1370, 706)
        Me.SGsDataGridView.TabIndex = 1
        '
        'TabPage6Limits
        '
        Me.TabPage6Limits.Controls.Add(Me.TableLayoutPanelLimits)
        Me.TabPage6Limits.Location = New System.Drawing.Point(4, 24)
        Me.TabPage6Limits.Name = "TabPage6Limits"
        Me.TabPage6Limits.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6Limits.Size = New System.Drawing.Size(1376, 712)
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
        Me.TabPage7Markers.Size = New System.Drawing.Size(1376, 712)
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
        Me.TableLayoutPanelMarkers.Size = New System.Drawing.Size(1370, 706)
        Me.TableLayoutPanelMarkers.TabIndex = 0
        '
        'TabPage8NotificationHistory
        '
        Me.TabPage8NotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistory)
        Me.TabPage8NotificationHistory.Location = New System.Drawing.Point(4, 24)
        Me.TabPage8NotificationHistory.Name = "TabPage8NotificationHistory"
        Me.TabPage8NotificationHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8NotificationHistory.Size = New System.Drawing.Size(1376, 712)
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
        Me.TableLayoutPanelNotificationHistory.Size = New System.Drawing.Size(1370, 706)
        Me.TableLayoutPanelNotificationHistory.TabIndex = 0
        '
        'TabPage9Basal
        '
        Me.TabPage9Basal.Controls.Add(Me.TableLayoutPanelBasal)
        Me.TabPage9Basal.Location = New System.Drawing.Point(4, 24)
        Me.TabPage9Basal.Name = "TabPage9Basal"
        Me.TabPage9Basal.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9Basal.Size = New System.Drawing.Size(1376, 712)
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
        Me.NotifyIcon1.Text = "CareLink Display"
        Me.NotifyIcon1.Visible = True
        '
        'WatchdogTimer
        '
        Me.WatchdogTimer.Interval = 360000
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
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 921)
        Me.Controls.Add(Me.LoginStatusLabel)
        Me.Controls.Add(Me.LoginStatus)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "Form1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
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
    Friend WithEvents HelpAboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboveHighLimitMessageLabel As Label
    Friend WithEvents AboveHighLimitValueLabel As Label
    Friend WithEvents ActiveInsulinLabel As Label
    Friend WithEvents ActiveInsulinValue As Label
    Friend WithEvents AITComboBox As ComboBox
    Friend WithEvents AITLabel As Label
    Friend WithEvents AverageSGMessageLabel As Label
    Friend WithEvents AverageSGUnitsLabel As Label
    Friend WithEvents AverageSGValueLabel As Label
    Friend WithEvents BelowLowLimitMessageLabel As Label
    Friend WithEvents BelowLowLimitValueLabel As Label
    Friend WithEvents CalibrationDueImage As PictureBox
    Friend WithEvents CurrentBG As Label
    Friend WithEvents CursorMessage1Label As Label
    Friend WithEvents CursorMessage2Label As Label
    Friend WithEvents CursorPictureBox As PictureBox
    Friend WithEvents CursorTimeLabel As Label
    Friend WithEvents CursorTimer As Timer
    Friend WithEvents CursorValueLabel As Label
    Friend WithEvents StartHereExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OptionsFilterRawJSONDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MenuHelpCheckForUpdatesMenuItem As ToolStripMenuItem
    Friend WithEvents MenuHelpReportIssueMenuItem As ToolStripMenuItem
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents InRangeMessageLabel As Label
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents ListView1 As ListView
    Friend WithEvents StartHereLoginToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
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
    Friend WithEvents OptionsSetupEmailServerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SGsDataGridView As DataGridView
    Friend WithEvents ShieldPictureBox As PictureBox
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents ViewShowMiniDisplayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents StartHereToolStripMenuItem As ToolStripMenuItem
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
    Friend WithEvents TimeInRangeSummaryLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents TransmatterBatterPercentLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents OptionsUseTestDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WatchdogTimer As Timer
    Friend WithEvents OptionsUseLastSavedDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents StartHereSnapshotLoadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartHereSnapshotSaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TotalAutoCorrectionLabel As Label
    Friend WithEvents TotalManualBolusLabel As Label
    Friend WithEvents TotalBasalLabel As Label
    Friend WithEvents TotalDailyDoseLabel As Label
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents SplitContainer3 As SplitContainer
    Friend WithEvents Last24HoursLabel As Label
    Friend WithEvents TimeInRangeLabel As Label
    Friend WithEvents SplitContainer4 As SplitContainer
End Class
