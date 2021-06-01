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
        Me.UseTestDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerUpdateTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelTop1 = New System.Windows.Forms.TableLayoutPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanelTop2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.MessageForCursor2Label = New System.Windows.Forms.Label()
        Me.ValueForCursorLabel = New System.Windows.Forms.Label()
        Me.MessagePictureBox = New System.Windows.Forms.PictureBox()
        Me.TimeForCursorLabel = New System.Windows.Forms.Label()
        Me.MessageForCursor1Label = New System.Windows.Forms.Label()
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
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.CursorTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1.SuspendLayout
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SplitContainer1.Panel1.SuspendLayout
        Me.SplitContainer1.Panel2.SuspendLayout
        Me.SplitContainer1.SuspendLayout
        Me.TabControl1.SuspendLayout
        Me.TabPage1.SuspendLayout
        CType(Me.MessagePictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.InsulinLevelPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.CalibrationDueImage,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.ShieldPictureBox,System.ComponentModel.ISupportInitialize).BeginInit
        Me.TabPage3.SuspendLayout
        Me.TabPage4.SuspendLayout
        Me.TabPage5.SuspendLayout
        Me.TabPage6.SuspendLayout
        Me.TabPage7.SuspendLayout
        Me.TabPage8.SuspendLayout
        Me.TabPage9.SuspendLayout
        Me.SuspendLayout
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartDisplayToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1174, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'StartDisplayToolStripMenuItem
        '
        Me.StartDisplayToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoginToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.StartDisplayToolStripMenuItem.Name = "StartDisplayToolStripMenuItem"
        Me.StartDisplayToolStripMenuItem.Size = New System.Drawing.Size(57, 20)
        Me.StartDisplayToolStripMenuItem.Text = "Display"
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
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UseTestDataToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'UseTestDataToolStripMenuItem
        '
        Me.UseTestDataToolStripMenuItem.CheckOnClick = true
        Me.UseTestDataToolStripMenuItem.Name = "UseTestDataToolStripMenuItem"
        Me.UseTestDataToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
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
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoScroll = true
        Me.TableLayoutPanel1.AutoSize = true
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.47268!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.52732!))
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(5)
        Me.TableLayoutPanel1.RowCount = 53
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1160, 700)
        Me.TableLayoutPanel1.TabIndex = 3
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
        Me.SplitContainer1.Size = New System.Drawing.Size(1174, 876)
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
        Me.TableLayoutPanelTop2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100!))
        Me.TableLayoutPanelTop2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 616!))
        Me.TableLayoutPanelTop2.Location = New System.Drawing.Point(514, 0)
        Me.TableLayoutPanelTop2.Name = "TableLayoutPanelTop2"
        Me.TableLayoutPanelTop2.RowCount = 1
        Me.TableLayoutPanelTop2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanelTop2.Size = New System.Drawing.Size(656, 136)
        Me.TableLayoutPanelTop2.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Controls.Add(Me.TabPage9)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1174, 734)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.Black
        Me.TabPage1.Controls.Add(Me.MessageForCursor2Label)
        Me.TabPage1.Controls.Add(Me.ValueForCursorLabel)
        Me.TabPage1.Controls.Add(Me.MessagePictureBox)
        Me.TabPage1.Controls.Add(Me.TimeForCursorLabel)
        Me.TabPage1.Controls.Add(Me.MessageForCursor1Label)
        Me.TabPage1.Controls.Add(Me.BelowLowLimitMessageLabel)
        Me.TabPage1.Controls.Add(Me.InRangeMessageLabel)
        Me.TabPage1.Controls.Add(Me.AboveHighLimitMessageLabel)
        Me.TabPage1.Controls.Add(Me.BelowLowLimitPercentPercentCharLabel)
        Me.TabPage1.Controls.Add(Me.BelowLowLimitValueLabel)
        Me.TabPage1.Controls.Add(Me.TimeInRangePercentPercentChar)
        Me.TabPage1.Controls.Add(Me.TimeInRangeValueLabel)
        Me.TabPage1.Controls.Add(Me.AboveHighLimitPercentCharLabel)
        Me.TabPage1.Controls.Add(Me.AboveHighLimitValueLabel)
        Me.TabPage1.Controls.Add(Me.ShieldUnitsLabel)
        Me.TabPage1.Controls.Add(Me.AverageSGUnitsLabel)
        Me.TabPage1.Controls.Add(Me.AverageSGValueLabel)
        Me.TabPage1.Controls.Add(Me.AverageSGMessageLabel)
        Me.TabPage1.Controls.Add(Me.TimeInRangeSummaryPercentCharLabel)
        Me.TabPage1.Controls.Add(Me.TimeInRangeSummaryLabel)
        Me.TabPage1.Controls.Add(Me.SensorMessage)
        Me.TabPage1.Controls.Add(Me.RemainingInsulinUnits)
        Me.TabPage1.Controls.Add(Me.InsulinLevelPictureBox)
        Me.TabPage1.Controls.Add(Me.ActiveInsulinValue)
        Me.TabPage1.Controls.Add(Me.CalibrationDueImage)
        Me.TabPage1.Controls.Add(Me.ActiveInsulinLabel)
        Me.TabPage1.Controls.Add(Me.CurrentBG)
        Me.TabPage1.Controls.Add(Me.ShieldPictureBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage1.TabIndex = 7
        Me.TabPage1.Text = "Home Page"
        '
        'MessageForCursor2Label
        '
        Me.MessageForCursor2Label.Font = New System.Drawing.Font("Segoe UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.MessageForCursor2Label.ForeColor = System.Drawing.Color.White
        Me.MessageForCursor2Label.Location = New System.Drawing.Point(133, 71)
        Me.MessageForCursor2Label.Name = "MessageForCursor2Label"
        Me.MessageForCursor2Label.Size = New System.Drawing.Size(235, 15)
        Me.MessageForCursor2Label.TabIndex = 40
        Me.MessageForCursor2Label.Text = "Message For Cursor 2"
        Me.MessageForCursor2Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ValueForCursorLabel
        '
        Me.ValueForCursorLabel.Font = New System.Drawing.Font("Segoe UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ValueForCursorLabel.ForeColor = System.Drawing.Color.White
        Me.ValueForCursorLabel.Location = New System.Drawing.Point(188, 95)
        Me.ValueForCursorLabel.Name = "ValueForCursorLabel"
        Me.ValueForCursorLabel.Size = New System.Drawing.Size(125, 15)
        Me.ValueForCursorLabel.TabIndex = 39
        Me.ValueForCursorLabel.Text = "Value For Cursor"
        Me.ValueForCursorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MessagePictureBox
        '
        Me.MessagePictureBox.Image = Global.CareLink.My.Resources.Resources.CalibrationDotRed
        Me.MessagePictureBox.InitialImage = Nothing
        Me.MessagePictureBox.Location = New System.Drawing.Point(228, 6)
        Me.MessagePictureBox.Name = "MessagePictureBox"
        Me.MessagePictureBox.Size = New System.Drawing.Size(45, 39)
        Me.MessagePictureBox.TabIndex = 38
        Me.MessagePictureBox.TabStop = false
        '
        'TimeForCursorLabel
        '
        Me.TimeForCursorLabel.AutoSize = true
        Me.TimeForCursorLabel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TimeForCursorLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeForCursorLabel.ForeColor = System.Drawing.Color.Black
        Me.TimeForCursorLabel.Location = New System.Drawing.Point(201, 159)
        Me.TimeForCursorLabel.Name = "TimeForCursorLabel"
        Me.TimeForCursorLabel.Size = New System.Drawing.Size(99, 17)
        Me.TimeForCursorLabel.TabIndex = 37
        Me.TimeForCursorLabel.Text = "TimeForCursor"
        Me.TimeForCursorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MessageForCursor1Label
        '
        Me.MessageForCursor1Label.Font = New System.Drawing.Font("Segoe UI", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.MessageForCursor1Label.ForeColor = System.Drawing.Color.White
        Me.MessageForCursor1Label.Location = New System.Drawing.Point(188, 47)
        Me.MessageForCursor1Label.Name = "MessageForCursor1Label"
        Me.MessageForCursor1Label.Size = New System.Drawing.Size(125, 15)
        Me.MessageForCursor1Label.TabIndex = 36
        Me.MessageForCursor1Label.Text = "Message For Cursor 1"
        Me.MessageForCursor1Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitMessageLabel
        '
        Me.BelowLowLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.BelowLowLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitMessageLabel.ForeColor = System.Drawing.Color.Red
        Me.BelowLowLimitMessageLabel.Location = New System.Drawing.Point(963, 549)
        Me.BelowLowLimitMessageLabel.Name = "BelowLowLimitMessageLabel"
        Me.BelowLowLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.BelowLowLimitMessageLabel.TabIndex = 32
        Me.BelowLowLimitMessageLabel.Text = "Below XXX XX/XX"
        Me.BelowLowLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InRangeMessageLabel
        '
        Me.InRangeMessageLabel.AutoSize = true
        Me.InRangeMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.InRangeMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.InRangeMessageLabel.ForeColor = System.Drawing.Color.Lime
        Me.InRangeMessageLabel.Location = New System.Drawing.Point(1000, 473)
        Me.InRangeMessageLabel.Name = "InRangeMessageLabel"
        Me.InRangeMessageLabel.Size = New System.Drawing.Size(73, 21)
        Me.InRangeMessageLabel.TabIndex = 30
        Me.InRangeMessageLabel.Text = "In range"
        Me.InRangeMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AboveHighLimitMessageLabel
        '
        Me.AboveHighLimitMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AboveHighLimitMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitMessageLabel.ForeColor = System.Drawing.Color.Orange
        Me.AboveHighLimitMessageLabel.Location = New System.Drawing.Point(963, 397)
        Me.AboveHighLimitMessageLabel.Name = "AboveHighLimitMessageLabel"
        Me.AboveHighLimitMessageLabel.Size = New System.Drawing.Size(170, 21)
        Me.AboveHighLimitMessageLabel.TabIndex = 28
        Me.AboveHighLimitMessageLabel.Text = "Above XXX XX/XX"
        Me.AboveHighLimitMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BelowLowLimitPercentPercentCharLabel
        '
        Me.BelowLowLimitPercentPercentCharLabel.AutoSize = true
        Me.BelowLowLimitPercentPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.BelowLowLimitPercentPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitPercentPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.BelowLowLimitPercentPercentCharLabel.Location = New System.Drawing.Point(1051, 515)
        Me.BelowLowLimitPercentPercentCharLabel.Name = "BelowLowLimitPercentPercentCharLabel"
        Me.BelowLowLimitPercentPercentCharLabel.Size = New System.Drawing.Size(31, 30)
        Me.BelowLowLimitPercentPercentCharLabel.TabIndex = 27
        Me.BelowLowLimitPercentPercentCharLabel.Text = "%"
        Me.BelowLowLimitPercentPercentCharLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'BelowLowLimitValueLabel
        '
        Me.BelowLowLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.BelowLowLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.BelowLowLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.BelowLowLimitValueLabel.Location = New System.Drawing.Point(985, 512)
        Me.BelowLowLimitValueLabel.Name = "BelowLowLimitValueLabel"
        Me.BelowLowLimitValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.BelowLowLimitValueLabel.TabIndex = 26
        Me.BelowLowLimitValueLabel.Text = "2"
        Me.BelowLowLimitValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TimeInRangePercentPercentChar
        '
        Me.TimeInRangePercentPercentChar.AutoSize = true
        Me.TimeInRangePercentPercentChar.BackColor = System.Drawing.Color.Transparent
        Me.TimeInRangePercentPercentChar.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangePercentPercentChar.ForeColor = System.Drawing.Color.White
        Me.TimeInRangePercentPercentChar.Location = New System.Drawing.Point(1051, 439)
        Me.TimeInRangePercentPercentChar.Name = "TimeInRangePercentPercentChar"
        Me.TimeInRangePercentPercentChar.Size = New System.Drawing.Size(31, 30)
        Me.TimeInRangePercentPercentChar.TabIndex = 25
        Me.TimeInRangePercentPercentChar.Text = "%"
        Me.TimeInRangePercentPercentChar.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'TimeInRangeValueLabel
        '
        Me.TimeInRangeValueLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeValueLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeValueLabel.Location = New System.Drawing.Point(985, 436)
        Me.TimeInRangeValueLabel.Name = "TimeInRangeValueLabel"
        Me.TimeInRangeValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.TimeInRangeValueLabel.TabIndex = 24
        Me.TimeInRangeValueLabel.Text = "90"
        Me.TimeInRangeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AboveHighLimitPercentCharLabel
        '
        Me.AboveHighLimitPercentCharLabel.AutoSize = true
        Me.AboveHighLimitPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.AboveHighLimitPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.AboveHighLimitPercentCharLabel.Location = New System.Drawing.Point(1051, 363)
        Me.AboveHighLimitPercentCharLabel.Name = "AboveHighLimitPercentCharLabel"
        Me.AboveHighLimitPercentCharLabel.Size = New System.Drawing.Size(31, 30)
        Me.AboveHighLimitPercentCharLabel.TabIndex = 23
        Me.AboveHighLimitPercentCharLabel.Text = "%"
        Me.AboveHighLimitPercentCharLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'AboveHighLimitValueLabel
        '
        Me.AboveHighLimitValueLabel.BackColor = System.Drawing.Color.Black
        Me.AboveHighLimitValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AboveHighLimitValueLabel.ForeColor = System.Drawing.Color.White
        Me.AboveHighLimitValueLabel.Location = New System.Drawing.Point(985, 360)
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
        Me.AverageSGUnitsLabel.AutoSize = true
        Me.AverageSGUnitsLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGUnitsLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGUnitsLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGUnitsLabel.Location = New System.Drawing.Point(1044, 604)
        Me.AverageSGUnitsLabel.Name = "AverageSGUnitsLabel"
        Me.AverageSGUnitsLabel.Size = New System.Drawing.Size(57, 21)
        Me.AverageSGUnitsLabel.TabIndex = 16
        Me.AverageSGUnitsLabel.Text = "XX/XX"
        Me.AverageSGUnitsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AverageSGValueLabel
        '
        Me.AverageSGValueLabel.BackColor = System.Drawing.Color.Black
        Me.AverageSGValueLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGValueLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGValueLabel.Location = New System.Drawing.Point(978, 592)
        Me.AverageSGValueLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.AverageSGValueLabel.Name = "AverageSGValueLabel"
        Me.AverageSGValueLabel.Size = New System.Drawing.Size(66, 33)
        Me.AverageSGValueLabel.TabIndex = 1
        Me.AverageSGValueLabel.Text = "100"
        Me.AverageSGValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AverageSGMessageLabel
        '
        Me.AverageSGMessageLabel.AutoSize = true
        Me.AverageSGMessageLabel.BackColor = System.Drawing.Color.Transparent
        Me.AverageSGMessageLabel.Font = New System.Drawing.Font("Segoe UI", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.AverageSGMessageLabel.ForeColor = System.Drawing.Color.White
        Me.AverageSGMessageLabel.Location = New System.Drawing.Point(1000, 625)
        Me.AverageSGMessageLabel.Name = "AverageSGMessageLabel"
        Me.AverageSGMessageLabel.Size = New System.Drawing.Size(97, 21)
        Me.AverageSGMessageLabel.TabIndex = 0
        Me.AverageSGMessageLabel.Text = "Average SG"
        Me.AverageSGMessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimeInRangeSummaryPercentCharLabel
        '
        Me.TimeInRangeSummaryPercentCharLabel.AutoSize = true
        Me.TimeInRangeSummaryPercentCharLabel.BackColor = System.Drawing.Color.Transparent
        Me.TimeInRangeSummaryPercentCharLabel.Font = New System.Drawing.Font("Segoe UI", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryPercentCharLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryPercentCharLabel.Location = New System.Drawing.Point(1007, 211)
        Me.TimeInRangeSummaryPercentCharLabel.Name = "TimeInRangeSummaryPercentCharLabel"
        Me.TimeInRangeSummaryPercentCharLabel.Size = New System.Drawing.Size(34, 32)
        Me.TimeInRangeSummaryPercentCharLabel.TabIndex = 3
        Me.TimeInRangeSummaryPercentCharLabel.Text = "%"
        '
        'TimeInRangeSummaryLabel
        '
        Me.TimeInRangeSummaryLabel.AutoSize = true
        Me.TimeInRangeSummaryLabel.BackColor = System.Drawing.Color.Black
        Me.TimeInRangeSummaryLabel.Font = New System.Drawing.Font("Segoe UI", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TimeInRangeSummaryLabel.ForeColor = System.Drawing.Color.White
        Me.TimeInRangeSummaryLabel.Location = New System.Drawing.Point(987, 169)
        Me.TimeInRangeSummaryLabel.Name = "TimeInRangeSummaryLabel"
        Me.TimeInRangeSummaryLabel.Size = New System.Drawing.Size(77, 47)
        Me.TimeInRangeSummaryLabel.TabIndex = 2
        Me.TimeInRangeSummaryLabel.Text = "100"
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
        Me.RemainingInsulinUnits.Location = New System.Drawing.Point(612, 78)
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
        Me.InsulinLevelPictureBox.Location = New System.Drawing.Point(628, 28)
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
        Me.CalibrationDueImage.Image = Global.CareLink.My.Resources.Resources.CalibrationDot
        Me.CalibrationDueImage.Location = New System.Drawing.Point(563, 28)
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
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "Summary Data"
        Me.TabPage3.UseVisualStyleBackColor = true
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPage4.Location = New System.Drawing.Point(4, 24)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage4.TabIndex = 1
        Me.TabPage4.Text = "Active Insulin"
        Me.TabPage4.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.AutoScroll = true
        Me.TableLayoutPanel2.AutoSize = true
        Me.TableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1160, 9)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.TableLayoutPanel3)
        Me.TabPage5.Location = New System.Drawing.Point(4, 24)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage5.TabIndex = 2
        Me.TabPage5.Text = "SGS"
        Me.TabPage5.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.AutoScroll = true
        Me.TableLayoutPanel3.AutoSize = true
        Me.TableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(1160, 700)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.TableLayoutPanel4)
        Me.TabPage6.Location = New System.Drawing.Point(4, 24)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage6.TabIndex = 3
        Me.TabPage6.Text = "Limits"
        Me.TabPage6.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.AutoScroll = true
        Me.TableLayoutPanel4.AutoSize = true
        Me.TableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(1160, 9)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.TableLayoutPanel5)
        Me.TabPage7.Location = New System.Drawing.Point(4, 24)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage7.TabIndex = 4
        Me.TabPage7.Text = "Markers"
        Me.TabPage7.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.AutoScroll = true
        Me.TableLayoutPanel5.AutoSize = true
        Me.TableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel5.ColumnCount = 1
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(1160, 700)
        Me.TableLayoutPanel5.TabIndex = 0
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.TableLayoutPanel6)
        Me.TabPage8.Location = New System.Drawing.Point(4, 24)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage8.TabIndex = 5
        Me.TabPage8.Text = "Notification History"
        Me.TabPage8.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.AutoScroll = true
        Me.TableLayoutPanel6.AutoSize = true
        Me.TableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel6.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel6.ColumnCount = 1
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 1
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(1160, 6)
        Me.TableLayoutPanel6.TabIndex = 0
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.TableLayoutPanel7)
        Me.TabPage9.Location = New System.Drawing.Point(4, 24)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9.Size = New System.Drawing.Size(1166, 706)
        Me.TabPage9.TabIndex = 6
        Me.TabPage9.Text = "Basal"
        Me.TabPage9.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.AutoScroll = true
        Me.TableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel7.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.TableLayoutPanel7.ColumnCount = 1
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 2
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(1160, 379)
        Me.TableLayoutPanel7.TabIndex = 0
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
        Me.ClientSize = New System.Drawing.Size(1174, 900)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(false)
        Me.MenuStrip1.PerformLayout
        Me.SplitContainer1.Panel1.ResumeLayout(false)
        Me.SplitContainer1.Panel2.ResumeLayout(false)
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer1.ResumeLayout(false)
        Me.TabControl1.ResumeLayout(false)
        Me.TabPage1.ResumeLayout(false)
        Me.TabPage1.PerformLayout
        CType(Me.MessagePictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.InsulinLevelPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.CalibrationDueImage,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.ShieldPictureBox,System.ComponentModel.ISupportInitialize).EndInit
        Me.TabPage3.ResumeLayout(false)
        Me.TabPage3.PerformLayout
        Me.TabPage4.ResumeLayout(false)
        Me.TabPage4.PerformLayout
        Me.TabPage5.ResumeLayout(false)
        Me.TabPage5.PerformLayout
        Me.TabPage6.ResumeLayout(false)
        Me.TabPage6.PerformLayout
        Me.TabPage7.ResumeLayout(false)
        Me.TabPage7.PerformLayout
        Me.TabPage8.ResumeLayout(false)
        Me.TabPage8.PerformLayout
        Me.TabPage9.ResumeLayout(false)
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
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents TabPage7 As TabPage
    Friend WithEvents TabPage8 As TabPage
    Friend WithEvents TabPage9 As TabPage
    Friend WithEvents TableLayoutPanelTop1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel6 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel7 As TableLayoutPanel
    Friend WithEvents ListView1 As ListView
    Friend WithEvents TableLayoutPanelTop2 As TableLayoutPanel
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UseTestDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CurrentBG As Label
    Friend WithEvents ActiveInsulinLabel As Label
    Friend WithEvents ShieldPictureBox As PictureBox
    Friend WithEvents TabPage1 As TabPage
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
    Friend WithEvents MessageForCursor1Label As Label
    Friend WithEvents TimeForCursorLabel As Label
    Friend WithEvents MessagePictureBox As PictureBox
    Friend WithEvents ValueForCursorLabel As Label
    Friend WithEvents MessageForCursor2Label As Label
    Friend WithEvents CursorTimer As Timer
End Class
