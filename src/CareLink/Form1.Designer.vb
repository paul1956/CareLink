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
        Me.StartDisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilterRawJSONDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseTestDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerUpdateTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanelSummyData = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelTop1 = New System.Windows.Forms.TableLayoutPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanelTop2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1HomePage = New System.Windows.Forms.TabPage()
        Me.PumpBatteryRemainingLabel = New System.Windows.Forms.Label()
        Me.TransmatterBatterPercentLabel = New System.Windows.Forms.Label()
        Me.TransmitterBatteryPictureBox = New System.Windows.Forms.PictureBox()
        Me.SensorTimeLeftLabel = New System.Windows.Forms.Label()
        Me.SensorDaysLeftLabel = New System.Windows.Forms.Label()
        Me.SensorTimeLefPictureBox = New System.Windows.Forms.PictureBox()
        Me.PumpBatteryPictureBox = New System.Windows.Forms.PictureBox()
        Me.AITLabel = New System.Windows.Forms.Label()
        Me.AITComboBox = New System.Windows.Forms.ComboBox()
        Me.CursorMessage2Label = New System.Windows.Forms.Label()
        Me.CursorValueLabel = New System.Windows.Forms.Label()
        Me.CursorPictureBox = New System.Windows.Forms.PictureBox()
        Me.CursorTimeLabel = New System.Windows.Forms.Label()
        Me.CursorMessage1Label = New System.Windows.Forms.Label()
        Me.BelowLowLimitMessageLabel = New System.Windows.Forms.Label()
        Me.InRangeMessageLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitMessageLabel = New System.Windows.Forms.Label()
        Me.BelowLowLimitPercentPercentCharLabel = New System.Windows.Forms.Label()
        Me.BelowLowLimitValueLabel = New System.Windows.Forms.Label()
        Me.TimeInRangePercentPercentChar = New System.Windows.Forms.Label()
        Me.TimeInRangeValueLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitPercentCharLabel = New System.Windows.Forms.Label()
        Me.AboveHighLimitValueLabel = New System.Windows.Forms.Label()
        Me.ShieldUnitsLabel = New System.Windows.Forms.Label()
        Me.AverageSGUnitsLabel = New System.Windows.Forms.Label()
        Me.AverageSGValueLabel = New System.Windows.Forms.Label()
        Me.AverageSGMessageLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeSummaryPercentCharLabel = New System.Windows.Forms.Label()
        Me.TimeInRangeSummaryLabel = New System.Windows.Forms.Label()
        Me.SensorMessage = New System.Windows.Forms.Label()
        Me.RemainingInsulinUnits = New System.Windows.Forms.Label()
        Me.InsulinLevelPictureBox = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinValue = New System.Windows.Forms.Label()
        Me.CalibrationDueImage = New System.Windows.Forms.PictureBox()
        Me.ActiveInsulinLabel = New System.Windows.Forms.Label()
        Me.CurrentBG = New System.Windows.Forms.Label()
        Me.ShieldPictureBox = New System.Windows.Forms.PictureBox()
        Me.TabPage2RunningActiveInsulin = New System.Windows.Forms.TabPage()
        Me.TabPage3SummaryData = New System.Windows.Forms.TabPage()
        Me.TabPage4ActiveInsulin = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelActiveInsulin = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage5SGS = New System.Windows.Forms.TabPage()
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
        Me.MenuStrip1.SuspendLayout
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SplitContainer1.Panel1.SuspendLayout
        Me.SplitContainer1.Panel2.SuspendLayout
        Me.SplitContainer1.SuspendLayout
        Me.TabControl1.SuspendLayout
        Me.TabPage1HomePage.SuspendLayout
        CType(Me.TransmitterBatteryPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.SensorTimeLefPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PumpBatteryPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.CursorPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.InsulinLevelPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.CalibrationDueImage,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.ShieldPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        Me.TabPage3SummaryData.SuspendLayout
        Me.TabPage4ActiveInsulin.SuspendLayout
        Me.TabPage5SGS.SuspendLayout
        CType(Me.SGsDataGridView,System.ComponentModel.ISupportInitialize).BeginInit
        Me.TabPage6Limits.SuspendLayout
        Me.TabPage7Markers.SuspendLayout
        Me.TabPage8NotificationHistory.SuspendLayout
        Me.TabPage9Basal.SuspendLayout
        Me.SuspendLayout
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartDisplayToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1384, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'StartDisplayToolStripMenuItem
        '
        Me.StartDisplayToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoginToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.StartDisplayToolStripMenuItem.Name = "StartDisplayToolStripMenuItem"
        Me.StartDisplayToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
        Me.StartDisplayToolStripMenuItem.Text = "Start Here"
        '
        'LoginToolStripMenuItem
        '
        Me.LoginToolStripMenuItem.Name = "LoginToolStripMenuItem"
        Me.LoginToolStripMenuItem.Size = New System.Drawing.Size(104, 22)
        Me.LoginToolStripMenuItem.Text = "Login"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(104, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FilterRawJSONDataToolStripMenuItem, Me.UseTestDataToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'FilterRawJSONDataToolStripMenuItem
        '
        Me.FilterRawJSONDataToolStripMenuItem.Checked = true
        Me.FilterRawJSONDataToolStripMenuItem.CheckOnClick = true
        Me.FilterRawJSONDataToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.FilterRawJSONDataToolStripMenuItem.Name = "FilterRawJSONDataToolStripMenuItem"
        Me.FilterRawJSONDataToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.FilterRawJSONDataToolStripMenuItem.Text = "Filter Raw JSON Data"
        '
        'UseTestDataToolStripMenuItem
        '
        Me.UseTestDataToolStripMenuItem.CheckOnClick = true
        Me.UseTestDataToolStripMenuItem.Name = "UseTestDataToolStripMenuItem"
        Me.UseTestDataToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.UseTestDataToolStripMenuItem.Text = "Use Test Data"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'ServerUpdateTimer
        '
        Me.ServerUpdateTimer.Interval = 300000
        '
        'TableLayoutPanelSummyData
        '
        Me.TableLayoutPanelSummyData.AutoScroll = true
        Me.TableLayoutPanelSummyData.AutoSize = true
        Me.TableLayoutPanelSummyData.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelSummyData.ColumnCount = 2
        Me.TableLayoutPanelSummyData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.47268!))
        Me.TableLayoutPanelSummyData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.52732!))
        Me.TableLayoutPanelSummyData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelSummyData.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelSummyData.Name = "TableLayoutPanelSummyData"
        Me.TableLayoutPanelSummyData.Padding = New System.Windows.Forms.Padding(5)
        Me.TableLayoutPanelSummyData.RowCount = 53
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanelSummyData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanelSummyData.Size = New System.Drawing.Size(1370, 700)
        Me.TableLayoutPanelSummyData.TabIndex = 3
        '
        'TableLayoutPanelTop1
        '
        Me.TableLayoutPanelTop1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTop1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTop1.ColumnCount = 2
        Me.TableLayoutPanelTop1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25!))
        Me.TableLayoutPanelTop1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75!))
        Me.TableLayoutPanelTop1.Location = New System.Drawing.Point(5, 0)
        Me.TableLayoutPanelTop1.Name = "TableLayoutPanelTop1"
        Me.TableLayoutPanelTop1.RowCount = 1
        Me.TableLayoutPanelTop1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelTop1.Size = New System.Drawing.Size(503, 129)
        Me.TableLayoutPanelTop1.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanelTop2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanelTop1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1384, 876)
        Me.SplitContainer1.SplitterDistance = 140
        Me.SplitContainer1.SplitterWidth = 2
        Me.SplitContainer1.TabIndex = 22
        '
        'TableLayoutPanelTop2
        '
        Me.TableLayoutPanelTop2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanelTop2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelTop2.CausesValidation = false
        Me.TableLayoutPanelTop2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelTop2.ColumnCount = 2
        Me.TableLayoutPanelTop2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10!))
        Me.TableLayoutPanelTop2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90!))
        Me.TableLayoutPanelTop2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TableLayoutPanelTop2.Location = New System.Drawing.Point(530, 0)
        Me.TableLayoutPanelTop2.Name = "TableLayoutPanelTop2"
        Me.TableLayoutPanelTop2.RowCount = 1
        Me.TableLayoutPanelTop2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelTop2.Size = New System.Drawing.Size(826, 136)
        Me.TableLayoutPanelTop2.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1HomePage)
        Me.TabControl1.Controls.Add(Me.TabPage2RunningActiveInsulin)
        Me.TabControl1.Controls.Add(Me.TabPage3SummaryData)
        Me.TabControl1.Controls.Add(Me.TabPage4ActiveInsulin)
        Me.TabControl1.Controls.Add(Me.TabPage5SGS)
        Me.TabControl1.Controls.Add(Me.TabPage6Limits)
        Me.TabControl1.Controls.Add(Me.TabPage7Markers)
        Me.TabControl1.Controls.Add(Me.TabPage8NotificationHistory)
        Me.TabControl1.Controls.Add(Me.TabPage9Basal)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1384, 734)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1HomePage
        '
        Me.TabPage1HomePage.BackColor = System.Drawing.Color.Black
        Me.TabPage1HomePage.Controls.Add(Me.PumpBatteryRemainingLabel)
        Me.TabPage1HomePage.Controls.Add(Me.TransmatterBatterPercentLabel)
        Me.TabPage1HomePage.Controls.Add(Me.TransmitterBatteryPictureBox)
        Me.TabPage1HomePage.Controls.Add(Me.SensorTimeLeftLabel)
        Me.TabPage1HomePage.Controls.Add(Me.SensorDaysLeftLabel)
        Me.TabPage1HomePage.Controls.Add(Me.SensorTimeLefPictureBox)
        Me.TabPage1HomePage.Controls.Add(Me.PumpBatteryPictureBox)
        Me.TabPage1HomePage.Controls.Add(Me.AITLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AITComboBox)
        Me.TabPage1HomePage.Controls.Add(Me.CursorMessage2Label)
        Me.TabPage1HomePage.Controls.Add(Me.CursorValueLabel)
        Me.TabPage1HomePage.Controls.Add(Me.CursorPictureBox)
        Me.TabPage1HomePage.Controls.Add(Me.CursorTimeLabel)
        Me.TabPage1HomePage.Controls.Add(Me.CursorMessage1Label)
        Me.TabPage1HomePage.Controls.Add(Me.BelowLowLimitMessageLabel)
        Me.TabPage1HomePage.Controls.Add(Me.InRangeMessageLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AboveHighLimitMessageLabel)
        Me.TabPage1HomePage.Controls.Add(Me.BelowLowLimitPercentPercentCharLabel)
        Me.TabPage1HomePage.Controls.Add(Me.BelowLowLimitValueLabel)
        Me.TabPage1HomePage.Controls.Add(Me.TimeInRangePercentPercentChar)
        Me.TabPage1HomePage.Controls.Add(Me.TimeInRangeValueLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AboveHighLimitPercentCharLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AboveHighLimitValueLabel)
        Me.TabPage1HomePage.Controls.Add(Me.ShieldUnitsLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AverageSGUnitsLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AverageSGValueLabel)
        Me.TabPage1HomePage.Controls.Add(Me.AverageSGMessageLabel)
        Me.TabPage1HomePage.Controls.Add(Me.TimeInRangeSummaryPercentCharLabel)
        Me.TabPage1HomePage.Controls.Add(Me.TimeInRangeSummaryLabel)
        Me.TabPage1HomePage.Controls.Add(Me.SensorMessage)
        Me.TabPage1HomePage.Controls.Add(Me.RemainingInsulinUnits)
        Me.TabPage1HomePage.Controls.Add(Me.InsulinLevelPictureBox)
        Me.TabPage1HomePage.Controls.Add(Me.ActiveInsulinValue)
        Me.TabPage1HomePage.Controls.Add(Me.CalibrationDueImage)
        Me.TabPage1HomePage.Controls.Add(Me.ActiveInsulinLabel)
        Me.TabPage1HomePage.Controls.Add(Me.CurrentBG)
        Me.TabPage1HomePage.Controls.Add(Me.ShieldPictureBox)
        Me.TabPage1HomePage.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1HomePage.Name = "TabPage1HomePage"
        Me.TabPage1HomePage.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1HomePage.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage1HomePage.TabIndex = 7
        Me.TabPage1HomePage.Text = "Home Page"
        '
        'PumpBatteryRemainingLabel
        '
        Me.PumpBatteryRemainingLabel.BackColor = System.Drawing.Color.Transparent
        Me.PumpBatteryRemainingLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.PumpBatteryRemainingLabel.ForeColor = System.Drawing.Color.White
        Me.PumpBatteryRemainingLabel.Location = New System.Drawing.Point(526, 95)
        Me.PumpBatteryRemainingLabel.Name = "PumpBatteryRemainingLabel"
        Me.PumpBatteryRemainingLabel.Size = New System.Drawing.Size(55, 21)
        Me.PumpBatteryRemainingLabel.TabIndex = 49
        Me.PumpBatteryRemainingLabel.Text = "???"
        Me.PumpBatteryRemainingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TransmatterBatterPercentLabel
        '
        Me.TransmatterBatterPercentLabel.BackColor = System.Drawing.Color.Transparent
        Me.TransmatterBatterPercentLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TransmatterBatterPercentLabel.ForeColor = System.Drawing.Color.White
        Me.TransmatterBatterPercentLabel.Location = New System.Drawing.Point(677, 95)
        Me.TransmatterBatterPercentLabel.Name = "TransmatterBatterPercentLabel"
        Me.TransmatterBatterPercentLabel.Size = New System.Drawing.Size(55, 21)
        Me.TransmatterBatterPercentLabel.TabIndex = 48
        Me.TransmatterBatterPercentLabel.Text = "???"
        Me.TransmatterBatterPercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TransmitterBatteryPictureBox
        '
        Me.TransmitterBatteryPictureBox.ErrorImage = Nothing
        Me.TransmitterBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.TransmitterBatteryUnknown
        Me.TransmitterBatteryPictureBox.Location = New System.Drawing.Point(670, 14)
        Me.TransmitterBatteryPictureBox.Name = "TransmitterBatteryPictureBox"
        Me.TransmitterBatteryPictureBox.Size = New System.Drawing.Size(68, 78)
        Me.TransmitterBatteryPictureBox.TabIndex = 47
        Me.TransmitterBatteryPictureBox.TabStop = false
        '
        'SensorTimeLeftLabel
        '
        Me.SensorTimeLeftLabel.BackColor = System.Drawing.Color.Transparent
        Me.SensorTimeLeftLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorTimeLeftLabel.ForeColor = System.Drawing.Color.White
        Me.SensorTimeLeftLabel.Location = New System.Drawing.Point(747, 95)
        Me.SensorTimeLeftLabel.Name = "SensorTimeLeftLabel"
        Me.SensorTimeLeftLabel.Size = New System.Drawing.Size(100, 21)
        Me.SensorTimeLeftLabel.TabIndex = 46
        Me.SensorTimeLeftLabel.Text = "???"
        Me.SensorTimeLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SensorDaysLeftLabel
        '
        Me.SensorDaysLeftLabel.BackColor = System.Drawing.Color.Transparent
        Me.SensorDaysLeftLabel.Font = New System.Drawing.Font("Segoe UI", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorDaysLeftLabel.ForeColor = System.Drawing.Color.White
        Me.SensorDaysLeftLabel.Location = New System.Drawing.Point(777, 23)
        Me.SensorDaysLeftLabel.Name = "SensorDaysLeftLabel"
        Me.SensorDaysLeftLabel.Size = New System.Drawing.Size(37, 52)
        Me.SensorDaysLeftLabel.TabIndex = 45
        Me.SensorDaysLeftLabel.Text = "5"
        Me.SensorDaysLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SensorDaysLeftLabel.Visible = false
        '
        'SensorTimeLefPictureBox
        '
        Me.SensorTimeLefPictureBox.ErrorImage = Nothing
        Me.SensorTimeLefPictureBox.Image = Global.CareLink.My.Resources.Resources.SensorExpirationUnknown
        Me.SensorTimeLefPictureBox.Location = New System.Drawing.Point(763, 6)
        Me.SensorTimeLefPictureBox.Name = "SensorTimeLefPictureBox"
        Me.SensorTimeLefPictureBox.Size = New System.Drawing.Size(68, 78)
        Me.SensorTimeLefPictureBox.TabIndex = 44
        Me.SensorTimeLefPictureBox.TabStop = false
        '
        'PumpBatteryPictureBox
        '
        Me.PumpBatteryPictureBox.ErrorImage = Nothing
        Me.PumpBatteryPictureBox.Image = Global.CareLink.My.Resources.Resources.PumpBatteryFull
        Me.PumpBatteryPictureBox.Location = New System.Drawing.Point(521, 16)
        Me.PumpBatteryPictureBox.Name = "PumpBatteryPictureBox"
        Me.PumpBatteryPictureBox.Size = New System.Drawing.Size(64, 74)
        Me.PumpBatteryPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PumpBatteryPictureBox.TabIndex = 43
        Me.PumpBatteryPictureBox.TabStop = false
        '
        'AITLabel
        '
        Me.AITLabel.AutoSize = true
        Me.AITLabel.BackColor = System.Drawing.Color.Transparent
        Me.AITLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AITLabel.ForeColor = System.Drawing.Color.White
        Me.AITLabel.Location = New System.Drawing.Point(893, 44)
        Me.AITLabel.Name = "AITLabel"
        Me.AITLabel.Size = New System.Drawing.Size(156, 21)
        Me.AITLabel.TabIndex = 42
        Me.AITLabel.Text = "Active Insulin TIme"
        '
        'AITComboBox
        '
        Me.AITComboBox.BackColor = System.Drawing.Color.Black
        Me.AITComboBox.ForeColor = System.Drawing.Color.White
        Me.AITComboBox.FormattingEnabled = true
        Me.AITComboBox.Items.AddRange(New Object() {"2:00", "2:15", "2:30", "2:45", "3:00", "3:15", "3:30", "3:45", "4:00", "4:15", "4:30", "4:45", "5:00", "5:15", "5:30", "5:45", "6:00"})
        Me.AITComboBox.Location = New System.Drawing.Point(944, 12)
        Me.AITComboBox.Name = "AITComboBox"
        Me.AITComboBox.Size = New System.Drawing.Size(54, 23)
        Me.AITComboBox.TabIndex = 41
        '
        'CursorMessage2Label
        '
        Me.CursorMessage2Label.Font = New System.Drawing.Font("Segoe UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage2Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage2Label.Location = New System.Drawing.Point(155, 74)
        Me.CursorMessage2Label.Name = "CursorMessage2Label"
        Me.CursorMessage2Label.Size = New System.Drawing.Size(235, 15)
        Me.CursorMessage2Label.TabIndex = 40
        Me.CursorMessage2Label.Text = "Message For Cursor 2"
        Me.CursorMessage2Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorMessage2Label.Visible = false
        '
        'CursorValueLabel
        '
        Me.CursorValueLabel.Font = New System.Drawing.Font("Segoe UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorValueLabel.ForeColor = System.Drawing.Color.White
        Me.CursorValueLabel.Location = New System.Drawing.Point(210, 93)
        Me.CursorValueLabel.Name = "CursorValueLabel"
        Me.CursorValueLabel.Size = New System.Drawing.Size(125, 15)
        Me.CursorValueLabel.TabIndex = 39
        Me.CursorValueLabel.Text = "Value For Cursor"
        Me.CursorValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorValueLabel.Visible = false
        '
        'CursorPictureBox
        '
        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.MealImageLarge
        Me.CursorPictureBox.InitialImage = Nothing
        Me.CursorPictureBox.Location = New System.Drawing.Point(250, 6)
        Me.CursorPictureBox.Name = "CursorPictureBox"
        Me.CursorPictureBox.Size = New System.Drawing.Size(39, 45)
        Me.CursorPictureBox.TabIndex = 38
        Me.CursorPictureBox.TabStop = false
        Me.CursorPictureBox.Visible = false
        '
        'CursorTimeLabel
        '
        Me.CursorTimeLabel.AutoSize = true
        Me.CursorTimeLabel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CursorTimeLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorTimeLabel.ForeColor = System.Drawing.Color.Black
        Me.CursorTimeLabel.Location = New System.Drawing.Point(201, 159)
        Me.CursorTimeLabel.Name = "CursorTimeLabel"
        Me.CursorTimeLabel.Size = New System.Drawing.Size(99, 17)
        Me.CursorTimeLabel.TabIndex = 37
        Me.CursorTimeLabel.Text = "TimeForCursor"
        Me.CursorTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorTimeLabel.Visible = false
        '
        'CursorMessage1Label
        '
        Me.CursorMessage1Label.Font = New System.Drawing.Font("Segoe UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CursorMessage1Label.ForeColor = System.Drawing.Color.White
        Me.CursorMessage1Label.Location = New System.Drawing.Point(210, 55)
        Me.CursorMessage1Label.Name = "CursorMessage1Label"
        Me.CursorMessage1Label.Size = New System.Drawing.Size(125, 15)
        Me.CursorMessage1Label.TabIndex = 36
        Me.CursorMessage1Label.Text = "Message For Cursor 1"
        Me.CursorMessage1Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CursorMessage1Label.Visible = false
        '
        'BelowLowLimitMessageLabel
        '
        Me.BelowLowLimitMessageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.BelowLowLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.BelowLowLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitMessageLabel.ForeColor = System.Drawing.Color.Red
        Me.BelowLowLimitMessageLabel.Location = New System.Drawing.Point(1182, 601)
        Me.BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        Me.BelowLowLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.BelowLowLimitMessageLabel.TabIndex = 32
        Me.BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        Me.BelowLowLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InRangeMessageLabel
        '
        Me.InRangeMessageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.InRangeMessageLabel.AutoSize = true
        Me.InRangeMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.InRangeMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.InRangeMessageLabel.ForeColor = System.Drawing.Color.Lime
        Me.InRangeMessageLabel.Location = New System.Drawing.Point(1219, 525)
        Me.InRangeMessageLabel.Name = "InRangeMessageLabel"
        Me.InRangeMessageLabel.Size = New System.Drawing.Size(73, 21)
        Me.InRangeMessageLabel.TabIndex = 30
        Me.InRangeMessageLabel.Text = "In range"
        Me.InRangeMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AboveHighLimitMessageLabel
        '
        Me.AboveHighLimitMessageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.AboveHighLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AboveHighLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitMessageLabel.ForeColor = System.Drawing.Color.Orange
        Me.AboveHighLimitMessageLabel.Location = New System.Drawing.Point(1182, 449)
        Me.AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        Me.AboveHighLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.AboveHighLimitMessageLabel.TabIndex = 28
        Me.AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        Me.AboveHighLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitPercentPercentCharLabel
        '
        Me.BelowLowLimitPercentPercentCharLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.BelowLowLimitPercentPercentCharLabel.AutoSize = true
        Me.BelowLowLimitPercentPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.BelowLowLimitPercentPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitPercentPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.BelowLowLimitPercentPercentCharLabel.Location = New System.Drawing.Point(1270, 569)
        Me.BelowLowLimitPercentPercentCharLabel.Name = "BelowLowLimitPercentPercentCharLabel"
        Me.BelowLowLimitPercentPercentCharLabel.Size = New System.Drawing.Size(31, 30)
        Me.BelowLowLimitPercentPercentCharLabel.TabIndex = 27
        Me.BelowLowLimitPercentPercentCharLabel.Text = "%"
        Me.BelowLowLimitPercentPercentCharLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'BelowLowLimitValueLabel
        '
        Me.BelowLowLimitValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.BelowLowLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.BelowLowLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.BelowLowLimitValueLabel.Location = New System.Drawing.Point(1204, 566)
        Me.BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        Me.BelowLowLimitValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.BelowLowLimitValueLabel.TabIndex = 26
        Me.BelowLowLimitValueLabel.Text = "2"
        Me.BelowLowLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TimeInRangePercentPercentChar
        '
        Me.TimeInRangePercentPercentChar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TimeInRangePercentPercentChar.AutoSize = true
        Me.TimeInRangePercentPercentChar.BackColor = System.Drawing.Color.Transparent
        Me.TimeInRangePercentPercentChar.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangePercentPercentChar.ForeColor = System.Drawing.Color.White
        Me.TimeInRangePercentPercentChar.Location = New System.Drawing.Point(1270, 493)
        Me.TimeInRangePercentPercentChar.Name = "TimeInRangePercentPercentChar"
        Me.TimeInRangePercentPercentChar.Size = New System.Drawing.Size(31, 30)
        Me.TimeInRangePercentPercentChar.TabIndex = 25
        Me.TimeInRangePercentPercentChar.Text = "%"
        Me.TimeInRangePercentPercentChar.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'TimeInRangeValueLabel
        '
        Me.TimeInRangeValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeValueLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeValueLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeValueLabel.Location = New System.Drawing.Point(1204, 490)
        Me.TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        Me.TimeInRangeValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.TimeInRangeValueLabel.TabIndex = 24
        Me.TimeInRangeValueLabel.Text = "90"
        Me.TimeInRangeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AboveHighLimitPercentCharLabel
        '
        Me.AboveHighLimitPercentCharLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.AboveHighLimitPercentCharLabel.AutoSize = true
        Me.AboveHighLimitPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.AboveHighLimitPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.AboveHighLimitPercentCharLabel.Location = New System.Drawing.Point(1270, 417)
        Me.AboveHighLimitPercentCharLabel.Name = "AboveHighLimitPercentCharLabel"
        Me.AboveHighLimitPercentCharLabel.Size = New System.Drawing.Size(31, 30)
        Me.AboveHighLimitPercentCharLabel.TabIndex = 23
        Me.AboveHighLimitPercentCharLabel.Text = "%"
        Me.AboveHighLimitPercentCharLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'AboveHighLimitValueLabel
        '
        Me.AboveHighLimitValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.AboveHighLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.AboveHighLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.AboveHighLimitValueLabel.Location = New System.Drawing.Point(1204, 414)
        Me.AboveHighLimitValueLabel.Name = "AboveHighLimitValueLabel"
        Me.AboveHighLimitValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.AboveHighLimitValueLabel.TabIndex = 22
        Me.AboveHighLimitValueLabel.Text = "8"
        Me.AboveHighLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ShieldUnitsLabel
        '
        Me.ShieldUnitsLabel.AutoSize = true
        Me.ShieldUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ShieldUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.ShieldUnitsLabel.Location = New System.Drawing.Point(425, 59)
        Me.ShieldUnitsLabel.Name = "ShieldUnitsLabel"
        Me.ShieldUnitsLabel.Size = New System.Drawing.Size(35, 13)
        Me.ShieldUnitsLabel.TabIndex = 17
        Me.ShieldUnitsLabel.Text = "XX/XX"
        '
        'AverageSGUnitsLabel
        '
        Me.AverageSGUnitsLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.AverageSGUnitsLabel.AutoSize = true
        Me.AverageSGUnitsLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGUnitsLabel.Location = New System.Drawing.Point(1263, 651)
        Me.AverageSGUnitsLabel.Name = "AverageSGUnitsLabel"
        Me.AverageSGUnitsLabel.Size = New System.Drawing.Size(57, 21)
        Me.AverageSGUnitsLabel.TabIndex = 16
        Me.AverageSGUnitsLabel.Text = "XX/XX"
        Me.AverageSGUnitsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGValueLabel
        '
        Me.AverageSGValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.AverageSGValueLabel.BackColor = System.Drawing.Color.Black
        Me.AverageSGValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGValueLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGValueLabel.Location = New System.Drawing.Point(1197, 639)
        Me.AverageSGValueLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.AverageSGValueLabel.Name = "AverageSGValueLabel"
        Me.AverageSGValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.AverageSGValueLabel.TabIndex = 1
        Me.AverageSGValueLabel.Text = "100"
        Me.AverageSGValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AverageSGMessageLabel
        '
        Me.AverageSGMessageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.AverageSGMessageLabel.AutoSize = true
        Me.AverageSGMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGMessageLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGMessageLabel.Location = New System.Drawing.Point(1219, 672)
        Me.AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        Me.AverageSGMessageLabel.Size = New System.Drawing.Size(97, 21)
        Me.AverageSGMessageLabel.TabIndex = 0
        Me.AverageSGMessageLabel.Text = "Average SG"
        Me.AverageSGMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimeInRangeSummaryPercentCharLabel
        '
        Me.TimeInRangeSummaryPercentCharLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeSummaryPercentCharLabel.AutoSize = true
        Me.TimeInRangeSummaryPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.TimeInRangeSummaryPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryPercentCharLabel.Location = New System.Drawing.Point(1250, 303)
        Me.TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        Me.TimeInRangeSummaryPercentCharLabel.Size = New System.Drawing.Size(34, 32)
        Me.TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        Me.TimeInRangeSummaryPercentCharLabel.Text = "%"
        '
        'TimeInRangeSummaryLabel
        '
        Me.TimeInRangeSummaryLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TimeInRangeSummaryLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeSummaryLabel.Font = New System.Drawing.Font("Segoe UI", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryLabel.Location = New System.Drawing.Point(1229, 261)
        Me.TimeInRangeSummaryLabel.Name = "TimeInRangeSummaryLabel"
        Me.TimeInRangeSummaryLabel.Size = New System.Drawing.Size(77, 47)
        Me.TimeInRangeSummaryLabel.TabIndex = 2
        Me.TimeInRangeSummaryLabel.Text = "100"
        Me.TimeInRangeSummaryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SensorMessage
        '
        Me.SensorMessage.BackColor = System.Drawing.Color.Transparent
        Me.SensorMessage.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.SensorMessage.ForeColor = System.Drawing.Color.White
        Me.SensorMessage.Location = New System.Drawing.Point(393, 14)
        Me.SensorMessage.Name = "SensorMessage"
        Me.SensorMessage.Size = New System.Drawing.Size(100, 66)
        Me.SensorMessage.TabIndex = 15
        Me.SensorMessage.Text = "Calibration Required"
        Me.SensorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RemainingInsulinUnits
        '
        Me.RemainingInsulinUnits.AutoSize = true
        Me.RemainingInsulinUnits.BackColor = System.Drawing.Color.Transparent
        Me.RemainingInsulinUnits.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.RemainingInsulinUnits.ForeColor = System.Drawing.Color.White
        Me.RemainingInsulinUnits.Location = New System.Drawing.Point(594, 95)
        Me.RemainingInsulinUnits.Name = "RemainingInsulinUnits"
        Me.RemainingInsulinUnits.Size = New System.Drawing.Size(66, 21)
        Me.RemainingInsulinUnits.TabIndex = 13
        Me.RemainingInsulinUnits.Text = "000.0 U"
        Me.RemainingInsulinUnits.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'InsulinLevelPictureBox
        '
        Me.InsulinLevelPictureBox.Image = CType(resources.GetObject("InsulinLevelPictureBox.Image"),System.Drawing.Image)
        Me.InsulinLevelPictureBox.InitialImage = Nothing
        Me.InsulinLevelPictureBox.Location = New System.Drawing.Point(610, 28)
        Me.InsulinLevelPictureBox.Name = "InsulinLevelPictureBox"
        Me.InsulinLevelPictureBox.Size = New System.Drawing.Size(35, 51)
        Me.InsulinLevelPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.InsulinLevelPictureBox.TabIndex = 12
        Me.InsulinLevelPictureBox.TabStop = false
        '
        'ActiveInsulinValue
        '
        Me.ActiveInsulinValue.AutoSize = true
        Me.ActiveInsulinValue.BackColor = System.Drawing.Color.Transparent
        Me.ActiveInsulinValue.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinValue.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinValue.Location = New System.Drawing.Point(13, 30)
        Me.ActiveInsulinValue.Name = "ActiveInsulinValue"
        Me.ActiveInsulinValue.Size = New System.Drawing.Size(80, 25)
        Me.ActiveInsulinValue.TabIndex = 6
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
        Me.CalibrationDueImage.TabStop = false
        '
        'ActiveInsulinLabel
        '
        Me.ActiveInsulinLabel.AutoSize = true
        Me.ActiveInsulinLabel.Font = New System.Drawing.Font("Verdana", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinLabel.ForeColor = System.Drawing.Color.White
        Me.ActiveInsulinLabel.Location = New System.Drawing.Point(13, 55)
        Me.ActiveInsulinLabel.Name = "ActiveInsulinLabel"
        Me.ActiveInsulinLabel.Size = New System.Drawing.Size(104, 18)
        Me.ActiveInsulinLabel.TabIndex = 1
        Me.ActiveInsulinLabel.Text = "Active Insulin"
        Me.ActiveInsulinLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CurrentBG
        '
        Me.CurrentBG.AutoSize = true
        Me.CurrentBG.BackColor = System.Drawing.Color.Transparent
        Me.CurrentBG.Font = New System.Drawing.Font("Segoe UI", 18!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.CurrentBG.ForeColor = System.Drawing.Color.White
        Me.CurrentBG.Location = New System.Drawing.Point(422, 23)
        Me.CurrentBG.Name = "CurrentBG"
        Me.CurrentBG.Size = New System.Drawing.Size(44, 32)
        Me.CurrentBG.TabIndex = 0
        Me.CurrentBG.Text = "---"
        Me.CurrentBG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ShieldPictureBox
        '
        Me.ShieldPictureBox.Image = Global.CareLink.My.Resources.Resources.Shield
        Me.ShieldPictureBox.Location = New System.Drawing.Point(396, 3)
        Me.ShieldPictureBox.Name = "ShieldPictureBox"
        Me.ShieldPictureBox.Size = New System.Drawing.Size(100, 100)
        Me.ShieldPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ShieldPictureBox.TabIndex = 4
        Me.ShieldPictureBox.TabStop = false
        '
        'TabPage2RunningActiveInsulin
        '
        Me.TabPage2RunningActiveInsulin.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2RunningActiveInsulin.Name = "TabPage2RunningActiveInsulin"
        Me.TabPage2RunningActiveInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2RunningActiveInsulin.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage2RunningActiveInsulin.TabIndex = 8
        Me.TabPage2RunningActiveInsulin.Text = "Running Active Insulin"
        Me.TabPage2RunningActiveInsulin.UseVisualStyleBackColor = true
        '
        'TabPage3SummaryData
        '
        Me.TabPage3SummaryData.Controls.Add(Me.TableLayoutPanelSummyData)
        Me.TabPage3SummaryData.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3SummaryData.Name = "TabPage3SummaryData"
        Me.TabPage3SummaryData.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3SummaryData.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage3SummaryData.TabIndex = 0
        Me.TabPage3SummaryData.Text = "Summary Data"
        Me.TabPage3SummaryData.UseVisualStyleBackColor = true
        '
        'TabPage4ActiveInsulin
        '
        Me.TabPage4ActiveInsulin.Controls.Add(Me.TableLayoutPanelActiveInsulin)
        Me.TabPage4ActiveInsulin.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4ActiveInsulin.Name = "TabPage4ActiveInsulin"
        Me.TabPage4ActiveInsulin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4ActiveInsulin.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage4ActiveInsulin.TabIndex = 1
        Me.TabPage4ActiveInsulin.Text = "Active Insulin"
        Me.TabPage4ActiveInsulin.UseVisualStyleBackColor = true
        '
        'TableLayoutPanelActiveInsulin
        '
        Me.TableLayoutPanelActiveInsulin.AutoScroll = true
        Me.TableLayoutPanelActiveInsulin.AutoSize = true
        Me.TableLayoutPanelActiveInsulin.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelActiveInsulin.ColumnCount = 1
        Me.TableLayoutPanelActiveInsulin.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelActiveInsulin.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelActiveInsulin.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelActiveInsulin.Name = "TableLayoutPanelActiveInsulin"
        Me.TableLayoutPanelActiveInsulin.RowCount = 2
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelActiveInsulin.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelActiveInsulin.Size = New System.Drawing.Size(1370, 9)
        Me.TableLayoutPanelActiveInsulin.TabIndex = 0
        '
        'TabPage5SGS
        '
        Me.TabPage5SGS.Controls.Add(Me.SGsDataGridView)
        Me.TabPage5SGS.Location = New System.Drawing.Point(4, 24)
        Me.TabPage5SGS.Name = "TabPage5SGS"
        Me.TabPage5SGS.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5SGS.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage5SGS.TabIndex = 2
        Me.TabPage5SGS.Text = "SGS"
        Me.TabPage5SGS.UseVisualStyleBackColor = true
        '
        'SGsDataGridView
        '
        Me.SGsDataGridView.AllowUserToAddRows = false
        Me.SGsDataGridView.AllowUserToDeleteRows = false
        Me.SGsDataGridView.AllowUserToResizeRows = false
        Me.SGsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.SGsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.SGsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SGsDataGridView.Location = New System.Drawing.Point(3, 3)
        Me.SGsDataGridView.Name = "SGsDataGridView"
        Me.SGsDataGridView.RowTemplate.Height = 25
        Me.SGsDataGridView.Size = New System.Drawing.Size(1370, 700)
        Me.SGsDataGridView.TabIndex = 1
        '
        'TabPage6Limits
        '
        Me.TabPage6Limits.Controls.Add(Me.TableLayoutPanelLimits)
        Me.TabPage6Limits.Location = New System.Drawing.Point(4, 24)
        Me.TabPage6Limits.Name = "TabPage6Limits"
        Me.TabPage6Limits.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6Limits.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage6Limits.TabIndex = 3
        Me.TabPage6Limits.Text = "Limits"
        Me.TabPage6Limits.UseVisualStyleBackColor = true
        '
        'TableLayoutPanelLimits
        '
        Me.TableLayoutPanelLimits.AutoScroll = true
        Me.TableLayoutPanelLimits.AutoSize = true
        Me.TableLayoutPanelLimits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelLimits.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelLimits.ColumnCount = 1
        Me.TableLayoutPanelLimits.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
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
        Me.TabPage7Markers.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage7Markers.TabIndex = 4
        Me.TabPage7Markers.Text = "Markers"
        Me.TabPage7Markers.UseVisualStyleBackColor = true
        '
        'TableLayoutPanelMarkers
        '
        Me.TableLayoutPanelMarkers.AutoScroll = true
        Me.TableLayoutPanelMarkers.AutoSize = true
        Me.TableLayoutPanelMarkers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelMarkers.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelMarkers.ColumnCount = 1
        Me.TableLayoutPanelMarkers.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelMarkers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMarkers.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelMarkers.Name = "TableLayoutPanelMarkers"
        Me.TableLayoutPanelMarkers.RowCount = 1
        Me.TableLayoutPanelMarkers.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelMarkers.Size = New System.Drawing.Size(1370, 700)
        Me.TableLayoutPanelMarkers.TabIndex = 0
        '
        'TabPage8NotificationHistory
        '
        Me.TabPage8NotificationHistory.Controls.Add(Me.TableLayoutPanelNotificationHistory)
        Me.TabPage8NotificationHistory.Location = New System.Drawing.Point(4, 24)
        Me.TabPage8NotificationHistory.Name = "TabPage8NotificationHistory"
        Me.TabPage8NotificationHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8NotificationHistory.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage8NotificationHistory.TabIndex = 5
        Me.TabPage8NotificationHistory.Text = "Notification History"
        Me.TabPage8NotificationHistory.UseVisualStyleBackColor = true
        '
        'TableLayoutPanelNotificationHistory
        '
        Me.TableLayoutPanelNotificationHistory.AutoScroll = true
        Me.TableLayoutPanelNotificationHistory.AutoSize = true
        Me.TableLayoutPanelNotificationHistory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelNotificationHistory.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelNotificationHistory.ColumnCount = 1
        Me.TableLayoutPanelNotificationHistory.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelNotificationHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelNotificationHistory.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelNotificationHistory.Name = "TableLayoutPanelNotificationHistory"
        Me.TableLayoutPanelNotificationHistory.RowCount = 2
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelNotificationHistory.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelNotificationHistory.Size = New System.Drawing.Size(1370, 700)
        Me.TableLayoutPanelNotificationHistory.TabIndex = 0
        '
        'TabPage9Basal
        '
        Me.TabPage9Basal.Controls.Add(Me.TableLayoutPanelBasal)
        Me.TabPage9Basal.Location = New System.Drawing.Point(4, 24)
        Me.TabPage9Basal.Name = "TabPage9Basal"
        Me.TabPage9Basal.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9Basal.Size = New System.Drawing.Size(1376, 706)
        Me.TabPage9Basal.TabIndex = 6
        Me.TabPage9Basal.Text = "Basal"
        Me.TabPage9Basal.UseVisualStyleBackColor = true
        '
        'TableLayoutPanelBasal
        '
        Me.TableLayoutPanelBasal.AutoScroll = true
        Me.TableLayoutPanelBasal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanelBasal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanelBasal.ColumnCount = 1
        Me.TableLayoutPanelBasal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelBasal.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanelBasal.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelBasal.Name = "TableLayoutPanelBasal"
        Me.TableLayoutPanelBasal.RowCount = 2
        Me.TableLayoutPanelBasal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanelBasal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanelBasal.Size = New System.Drawing.Size(1370, 379)
        Me.TableLayoutPanelBasal.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"),System.Windows.Forms.ImageListStreamer)
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 900)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "Form1"
        Me.MenuStrip1.ResumeLayout(false)
        Me.MenuStrip1.PerformLayout
        Me.SplitContainer1.Panel1.ResumeLayout(false)
        Me.SplitContainer1.Panel2.ResumeLayout(false)
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer1.ResumeLayout(false)
        Me.TabControl1.ResumeLayout(false)
        Me.TabPage1HomePage.ResumeLayout(false)
        Me.TabPage1HomePage.PerformLayout
        CType(Me.TransmitterBatteryPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.SensorTimeLefPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PumpBatteryPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.CursorPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.InsulinLevelPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.CalibrationDueImage,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.ShieldPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        Me.TabPage3SummaryData.ResumeLayout(false)
        Me.TabPage3SummaryData.PerformLayout
        Me.TabPage4ActiveInsulin.ResumeLayout(false)
        Me.TabPage4ActiveInsulin.PerformLayout
        Me.TabPage5SGS.ResumeLayout(false)
        CType(Me.SGsDataGridView,System.ComponentModel.ISupportInitialize).EndInit
        Me.TabPage6Limits.ResumeLayout(false)
        Me.TabPage6Limits.PerformLayout
        Me.TabPage7Markers.ResumeLayout(false)
        Me.TabPage7Markers.PerformLayout
        Me.TabPage8NotificationHistory.ResumeLayout(false)
        Me.TabPage8NotificationHistory.PerformLayout
        Me.TabPage9Basal.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ServerUpdateTimer As Timer
    Friend WithEvents StartDisplayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoginToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage3SummaryData As TabPage
    Friend WithEvents TabPage4ActiveInsulin As TabPage
    Friend WithEvents TabPage5SGS As TabPage
    Friend WithEvents TabPage6Limits As TabPage
    Friend WithEvents TabPage7Markers As TabPage
    Friend WithEvents TabPage8NotificationHistory As TabPage
    Friend WithEvents TabPage9Basal As TabPage
    Friend WithEvents TableLayoutPanelTop1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanelSummyData As TableLayoutPanel
    Friend WithEvents TableLayoutPanelActiveInsulin As TableLayoutPanel
    Friend WithEvents TableLayoutPanelLimits As TableLayoutPanel
    Friend WithEvents TableLayoutPanelMarkers As TableLayoutPanel
    Friend WithEvents TableLayoutPanelNotificationHistory As TableLayoutPanel
    Friend WithEvents TableLayoutPanelBasal As TableLayoutPanel
    Friend WithEvents ListView1 As ListView
    Friend WithEvents TableLayoutPanelTop2 As TableLayoutPanel
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UseTestDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CurrentBG As Label
    Friend WithEvents ActiveInsulinLabel As Label
    Friend WithEvents ShieldPictureBox As PictureBox
    Friend WithEvents TabPage1HomePage As TabPage
    Friend WithEvents CalibrationDueImage As PictureBox
    Friend WithEvents ActiveInsulinValue As Label
    Friend WithEvents InsulinLevelPictureBox As PictureBox
    Friend WithEvents RemainingInsulinUnits As Label
    Friend WithEvents SensorMessage As Label
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents AverageSGMessageLabel As Label
    Friend WithEvents AverageSGValueLabel As Label
    Friend WithEvents TimeInRangeSummaryLabel As Label
    Friend WithEvents TimeInRangeSummaryPercentCharLabel As Label
    Friend WithEvents AverageSGUnitsLabel As Label
    Friend WithEvents ShieldUnitsLabel As Label
    Friend WithEvents AboveHighLimitPercentCharLabel As Label
    Friend WithEvents AboveHighLimitValueLabel As Label
    Friend WithEvents BelowLowLimitPercentPercentCharLabel As Label
    Friend WithEvents BelowLowLimitValueLabel As Label
    Friend WithEvents TimeInRangePercentPercentChar As Label
    Friend WithEvents TimeInRangeValueLabel As Label
    Friend WithEvents AboveHighLimitMessageLabel As Label
    Friend WithEvents BelowLowLimitMessageLabel As Label
    Friend WithEvents InRangeMessageLabel As Label
    Friend WithEvents CursorMessage1Label As Label
    Friend WithEvents CursorTimeLabel As Label
    Friend WithEvents CursorPictureBox As PictureBox
    Friend WithEvents CursorValueLabel As Label
    Friend WithEvents CursorMessage2Label As Label
    Friend WithEvents CursorTimer As Timer
    Friend WithEvents AITComboBox As ComboBox
    Friend WithEvents AITLabel As Label
    Friend WithEvents TabPage2RunningActiveInsulin As TabPage
    Friend WithEvents PumpBatteryPictureBox As PictureBox
    Friend WithEvents SensorTimeLefPictureBox As PictureBox
    Friend WithEvents SensorDaysLeftLabel As Label
    Friend WithEvents SensorTimeLeftLabel As Label
    Friend WithEvents TransmitterBatteryPictureBox As PictureBox
    Friend WithEvents TransmatterBatterPercentLabel As Label
    Friend WithEvents PumpBatteryRemainingLabel As Label
    Friend WithEvents FilterRawJSONDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SGsDataGridView As DataGridView
End Class
