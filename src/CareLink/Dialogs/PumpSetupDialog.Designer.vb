' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PumpSetupDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As DataGridViewCellStyle = New DataGridViewCellStyle()
        RtbMainLeft = New RichTextBox()
        RtbMainRight = New RichTextBox()
        SplitContainer1 = New SplitContainer()
        RtbLowSnoozeMenu = New RichTextBox()
        DataGridViewLowAlert = New DataGridView()
        ColumnStartTimeLow = New DataGridViewTextBoxColumn()
        ColumnEndTimeLow = New DataGridViewTextBoxColumn()
        ColumnLowLimit = New DataGridViewTextBoxColumn()
        ColumnSuspend = New DataGridViewTextBoxColumn()
        ColumnAlertOnLow = New DataGridViewCheckBoxColumn()
        ColumnAlertBeforeLow = New DataGridViewCheckBoxColumn()
        ColumnResumeBasalAlert = New DataGridViewCheckBoxColumn()
        RtbLowAlertMenu = New RichTextBox()
        RtbHighSnoozeMenu = New RichTextBox()
        DataGridViewHighAlert = New DataGridView()
        ColumnStartTimeHigh = New DataGridViewTextBoxColumn()
        ColumnEndTimeHigh = New DataGridViewTextBoxColumn()
        ColumnHighLimit = New DataGridViewTextBoxColumn()
        ColumnAlertBeforeHigh = New DataGridViewCheckBoxColumn()
        ColumnTimeBeforeHighText = New DataGridViewTextBoxColumn()
        ColumnAlertOnHigh = New DataGridViewCheckBoxColumn()
        ColumnRiseAlert = New DataGridViewCheckBoxColumn()
        ColumnRiseLimit = New DataGridViewTextBoxColumn()
        RtbHighAlertMenu = New RichTextBox()
        TableLayoutPanel2 = New TableLayoutPanel()
        OK_Button = New Button()
        Cancel_Button = New Button()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(DataGridViewLowAlert, ComponentModel.ISupportInitialize).BeginInit()
        CType(DataGridViewHighAlert, ComponentModel.ISupportInitialize).BeginInit()
        TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' RtbMainLeft
        ' 
        RtbMainLeft.BorderStyle = BorderStyle.None
        RtbMainLeft.Dock = DockStyle.Left
        RtbMainLeft.Location = New Point(0, 0)
        RtbMainLeft.Name = "RtbMainLeft"
        RtbMainLeft.Size = New Size(571, 528)
        RtbMainLeft.TabIndex = 0
        RtbMainLeft.Text = ""
        ' 
        ' RtbMainRight
        ' 
        RtbMainRight.BorderStyle = BorderStyle.None
        RtbMainRight.Dock = DockStyle.Right
        RtbMainRight.Location = New Point(607, 0)
        RtbMainRight.Name = "RtbMainRight"
        RtbMainRight.Size = New Size(567, 528)
        RtbMainRight.TabIndex = 1
        RtbMainRight.Text = ""
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(RtbMainRight)
        SplitContainer1.Panel1.Controls.Add(RtbMainLeft)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.AutoScroll = True
        SplitContainer1.Panel2.Controls.Add(RtbLowSnoozeMenu)
        SplitContainer1.Panel2.Controls.Add(DataGridViewLowAlert)
        SplitContainer1.Panel2.Controls.Add(RtbLowAlertMenu)
        SplitContainer1.Panel2.Controls.Add(RtbHighSnoozeMenu)
        SplitContainer1.Panel2.Controls.Add(DataGridViewHighAlert)
        SplitContainer1.Panel2.Controls.Add(RtbHighAlertMenu)
        SplitContainer1.Size = New Size(1174, 931)
        SplitContainer1.SplitterDistance = 528
        SplitContainer1.TabIndex = 1
        ' 
        ' RtbLowSnoozeMenu
        ' 
        RtbLowSnoozeMenu.Location = New Point(0, 364)
        RtbLowSnoozeMenu.Name = "RtbLowSnoozeMenu"
        RtbLowSnoozeMenu.Size = New Size(983, 29)
        RtbLowSnoozeMenu.TabIndex = 2
        RtbLowSnoozeMenu.Text = "SNOOZE Menu (Low Alert)"
        ' 
        ' DataGridViewLowAlert
        ' 
        DataGridViewLowAlert.AllowUserToAddRows = False
        DataGridViewLowAlert.AllowUserToDeleteRows = False
        DataGridViewLowAlert.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewLowAlert.Columns.AddRange(New DataGridViewColumn() {ColumnStartTimeLow, ColumnEndTimeLow, ColumnLowLimit, ColumnSuspend, ColumnAlertOnLow, ColumnAlertBeforeLow, ColumnResumeBasalAlert})
        DataGridViewLowAlert.Location = New Point(0, 225)
        DataGridViewLowAlert.Name = "DataGridViewLowAlert"
        DataGridViewLowAlert.ReadOnly = True
        DataGridViewLowAlert.RowTemplate.Height = 25
        DataGridViewLowAlert.Size = New Size(1171, 135)
        DataGridViewLowAlert.TabIndex = 4
        ' 
        ' ColumnStartTimeLow
        ' 
        ColumnStartTimeLow.Frozen = True
        ColumnStartTimeLow.HeaderText = "Start Time"
        ColumnStartTimeLow.Name = "ColumnStartTimeLow"
        ColumnStartTimeLow.ReadOnly = True
        ColumnStartTimeLow.Resizable = DataGridViewTriState.False
        ' 
        ' ColumnEndTimeLow
        ' 
        ColumnEndTimeLow.Frozen = True
        ColumnEndTimeLow.HeaderText = "End TIme"
        ColumnEndTimeLow.Name = "ColumnEndTimeLow"
        ColumnEndTimeLow.ReadOnly = True
        ColumnEndTimeLow.Resizable = DataGridViewTriState.False
        ColumnEndTimeLow.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnLowLimit
        ' 
        ColumnLowLimit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        ColumnLowLimit.DefaultCellStyle = DataGridViewCellStyle1
        ColumnLowLimit.Frozen = True
        ColumnLowLimit.HeaderText = "Low Limit"
        ColumnLowLimit.MinimumWidth = 150
        ColumnLowLimit.Name = "ColumnLowLimit"
        ColumnLowLimit.ReadOnly = True
        ColumnLowLimit.Resizable = DataGridViewTriState.False
        ColumnLowLimit.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnLowLimit.Width = 150
        ' 
        ' ColumnSuspend
        ' 
        ColumnSuspend.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        ColumnSuspend.DefaultCellStyle = DataGridViewCellStyle2
        ColumnSuspend.HeaderText = "Suspend"
        ColumnSuspend.MinimumWidth = 125
        ColumnSuspend.Name = "ColumnSuspend"
        ColumnSuspend.ReadOnly = True
        ColumnSuspend.Resizable = DataGridViewTriState.False
        ColumnSuspend.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnSuspend.Width = 125
        ' 
        ' ColumnAlertOnLow
        ' 
        ColumnAlertOnLow.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.NullValue = False
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        ColumnAlertOnLow.DefaultCellStyle = DataGridViewCellStyle3
        ColumnAlertOnLow.HeaderText = "Alert On Low"
        ColumnAlertOnLow.MinimumWidth = 125
        ColumnAlertOnLow.Name = "ColumnAlertOnLow"
        ColumnAlertOnLow.ReadOnly = True
        ColumnAlertOnLow.Resizable = DataGridViewTriState.False
        ColumnAlertOnLow.Width = 125
        ' 
        ' ColumnAlertBeforeLow
        ' 
        ColumnAlertBeforeLow.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.NullValue = False
        DataGridViewCellStyle4.WrapMode = DataGridViewTriState.False
        ColumnAlertBeforeLow.DefaultCellStyle = DataGridViewCellStyle4
        ColumnAlertBeforeLow.HeaderText = "Alert before low"
        ColumnAlertBeforeLow.MinimumWidth = 125
        ColumnAlertBeforeLow.Name = "ColumnAlertBeforeLow"
        ColumnAlertBeforeLow.ReadOnly = True
        ColumnAlertBeforeLow.Resizable = DataGridViewTriState.False
        ColumnAlertBeforeLow.Width = 125
        ' 
        ' ColumnResumeBasalAlert
        ' 
        ColumnResumeBasalAlert.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.NullValue = False
        DataGridViewCellStyle5.WrapMode = DataGridViewTriState.False
        ColumnResumeBasalAlert.DefaultCellStyle = DataGridViewCellStyle5
        ColumnResumeBasalAlert.HeaderText = "Resume Basal Alert"
        ColumnResumeBasalAlert.MinimumWidth = 125
        ColumnResumeBasalAlert.Name = "ColumnResumeBasalAlert"
        ColumnResumeBasalAlert.ReadOnly = True
        ColumnResumeBasalAlert.Resizable = DataGridViewTriState.False
        ColumnResumeBasalAlert.Width = 125
        ' 
        ' RtbLowAlertMenu
        ' 
        RtbLowAlertMenu.Location = New Point(0, 193)
        RtbLowAlertMenu.Name = "RtbLowAlertMenu"
        RtbLowAlertMenu.Size = New Size(1171, 29)
        RtbLowAlertMenu.TabIndex = 3
        RtbLowAlertMenu.Text = "LOW ALERT Menu"
        ' 
        ' RtbHighSnoozeMenu
        ' 
        RtbHighSnoozeMenu.Location = New Point(0, 164)
        RtbHighSnoozeMenu.Name = "RtbHighSnoozeMenu"
        RtbHighSnoozeMenu.Size = New Size(1171, 29)
        RtbHighSnoozeMenu.TabIndex = 2
        RtbHighSnoozeMenu.Text = "SNOOZE Menu (High Alert)"
        ' 
        ' DataGridViewHighAlert
        ' 
        DataGridViewHighAlert.AllowUserToAddRows = False
        DataGridViewHighAlert.AllowUserToDeleteRows = False
        DataGridViewHighAlert.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewHighAlert.Columns.AddRange(New DataGridViewColumn() {ColumnStartTimeHigh, ColumnEndTimeHigh, ColumnHighLimit, ColumnAlertBeforeHigh, ColumnTimeBeforeHighText, ColumnAlertOnHigh, ColumnRiseAlert, ColumnRiseLimit})
        DataGridViewHighAlert.Location = New Point(0, 29)
        DataGridViewHighAlert.Name = "DataGridViewHighAlert"
        DataGridViewHighAlert.ReadOnly = True
        DataGridViewHighAlert.RowTemplate.Height = 25
        DataGridViewHighAlert.Size = New Size(1171, 135)
        DataGridViewHighAlert.TabIndex = 0
        ' 
        ' ColumnStartTimeHigh
        ' 
        ColumnStartTimeHigh.HeaderText = "Start Time"
        ColumnStartTimeHigh.Name = "ColumnStartTimeHigh"
        ColumnStartTimeHigh.ReadOnly = True
        ColumnStartTimeHigh.Resizable = DataGridViewTriState.False
        ColumnStartTimeHigh.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnEndTimeHigh
        ' 
        ColumnEndTimeHigh.HeaderText = "End Time"
        ColumnEndTimeHigh.Name = "ColumnEndTimeHigh"
        ColumnEndTimeHigh.ReadOnly = True
        ColumnEndTimeHigh.Resizable = DataGridViewTriState.False
        ColumnEndTimeHigh.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnHighLimit
        ' 
        ColumnHighLimit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.WrapMode = DataGridViewTriState.False
        ColumnHighLimit.DefaultCellStyle = DataGridViewCellStyle6
        ColumnHighLimit.FillWeight = 50.0F
        ColumnHighLimit.HeaderText = "High Limit"
        ColumnHighLimit.MinimumWidth = 150
        ColumnHighLimit.Name = "ColumnHighLimit"
        ColumnHighLimit.ReadOnly = True
        ColumnHighLimit.Resizable = DataGridViewTriState.False
        ColumnHighLimit.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnHighLimit.Width = 150
        ' 
        ' ColumnAlertBeforeHigh
        ' 
        ColumnAlertBeforeHigh.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.NullValue = False
        DataGridViewCellStyle7.WrapMode = DataGridViewTriState.False
        ColumnAlertBeforeHigh.DefaultCellStyle = DataGridViewCellStyle7
        ColumnAlertBeforeHigh.FillWeight = 50.0F
        ColumnAlertBeforeHigh.HeaderText = "Alert Before High"
        ColumnAlertBeforeHigh.MinimumWidth = 125
        ColumnAlertBeforeHigh.Name = "ColumnAlertBeforeHigh"
        ColumnAlertBeforeHigh.ReadOnly = True
        ColumnAlertBeforeHigh.Resizable = DataGridViewTriState.False
        ColumnAlertBeforeHigh.Width = 125
        ' 
        ' ColumnTimeBeforeHighText
        ' 
        ColumnTimeBeforeHighText.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.WrapMode = DataGridViewTriState.False
        ColumnTimeBeforeHighText.DefaultCellStyle = DataGridViewCellStyle8
        ColumnTimeBeforeHighText.FillWeight = 50.0F
        ColumnTimeBeforeHighText.HeaderText = "Time Before High"
        ColumnTimeBeforeHighText.MinimumWidth = 125
        ColumnTimeBeforeHighText.Name = "ColumnTimeBeforeHighText"
        ColumnTimeBeforeHighText.ReadOnly = True
        ColumnTimeBeforeHighText.Resizable = DataGridViewTriState.False
        ColumnTimeBeforeHighText.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnTimeBeforeHighText.Width = 125
        ' 
        ' ColumnAlertOnHigh
        ' 
        ColumnAlertOnHigh.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle9.NullValue = False
        DataGridViewCellStyle9.WrapMode = DataGridViewTriState.False
        ColumnAlertOnHigh.DefaultCellStyle = DataGridViewCellStyle9
        ColumnAlertOnHigh.FillWeight = 50.0F
        ColumnAlertOnHigh.HeaderText = "Alert On High"
        ColumnAlertOnHigh.MinimumWidth = 125
        ColumnAlertOnHigh.Name = "ColumnAlertOnHigh"
        ColumnAlertOnHigh.ReadOnly = True
        ColumnAlertOnHigh.Resizable = DataGridViewTriState.False
        ColumnAlertOnHigh.Width = 125
        ' 
        ' ColumnRiseAlert
        ' 
        ColumnRiseAlert.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle10.NullValue = False
        DataGridViewCellStyle10.WrapMode = DataGridViewTriState.False
        ColumnRiseAlert.DefaultCellStyle = DataGridViewCellStyle10
        ColumnRiseAlert.FillWeight = 75.0F
        ColumnRiseAlert.HeaderText = "Rise Alert"
        ColumnRiseAlert.MinimumWidth = 125
        ColumnRiseAlert.Name = "ColumnRiseAlert"
        ColumnRiseAlert.ReadOnly = True
        ColumnRiseAlert.Resizable = DataGridViewTriState.False
        ColumnRiseAlert.Width = 125
        ' 
        ' ColumnRiseLimit
        ' 
        ColumnRiseLimit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.WrapMode = DataGridViewTriState.False
        ColumnRiseLimit.DefaultCellStyle = DataGridViewCellStyle11
        ColumnRiseLimit.HeaderText = "Rise Limit"
        ColumnRiseLimit.MinimumWidth = 125
        ColumnRiseLimit.Name = "ColumnRiseLimit"
        ColumnRiseLimit.ReadOnly = True
        ColumnRiseLimit.Resizable = DataGridViewTriState.False
        ColumnRiseLimit.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnRiseLimit.Width = 125
        ' 
        ' RtbHighAlertMenu
        ' 
        RtbHighAlertMenu.Dock = DockStyle.Top
        RtbHighAlertMenu.Location = New Point(0, 0)
        RtbHighAlertMenu.Name = "RtbHighAlertMenu"
        RtbHighAlertMenu.Size = New Size(1174, 29)
        RtbHighAlertMenu.TabIndex = 1
        RtbHighAlertMenu.Text = "HIGH ALERT"
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel2.Controls.Add(OK_Button, 0, 0)
        TableLayoutPanel2.Controls.Add(Cancel_Button, 1, 0)
        TableLayoutPanel2.Location = New Point(990, 896)
        TableLayoutPanel2.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel2.Size = New Size(170, 33)
        TableLayoutPanel2.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Location = New Point(4, 3)
        OK_Button.Margin = New Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(77, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.None
        Cancel_Button.Location = New Point(89, 3)
        Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(77, 27)
        Cancel_Button.TabIndex = 1
        Cancel_Button.Text = "Cancel"
        ' 
        ' PumpSetupDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(1174, 931)
        Me.Controls.Add(TableLayoutPanel2)
        Me.Controls.Add(SplitContainer1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PumpSetupDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Pump Setup Dialog"
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        CType(DataGridViewLowAlert, ComponentModel.ISupportInitialize).EndInit()
        CType(DataGridViewHighAlert, ComponentModel.ISupportInitialize).EndInit()
        TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents ColumnAlertBeforeHigh As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnAlertBeforeLow As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnAlertOnHigh As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnAlertOnLow As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnEndTimeHigh As DataGridViewTextBoxColumn
    Friend WithEvents ColumnEndTimeLow As DataGridViewTextBoxColumn
    Friend WithEvents ColumnHighLimit As DataGridViewTextBoxColumn
    Friend WithEvents ColumnLowLimit As DataGridViewTextBoxColumn
    Friend WithEvents ColumnResumeBasalAlert As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnRiseAlert As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnRiseLimit As DataGridViewTextBoxColumn
    Friend WithEvents ColumnStartTimeHigh As DataGridViewTextBoxColumn
    Friend WithEvents ColumnStartTimeLow As DataGridViewTextBoxColumn
    Friend WithEvents ColumnSuspend As DataGridViewTextBoxColumn
    Friend WithEvents ColumnTimeBeforeHighText As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewHighAlert As DataGridView
    Friend WithEvents DataGridViewLowAlert As DataGridView
    Friend WithEvents RtbHighAlertMenu As RichTextBox
    Friend WithEvents RtbHighSnoozeMenu As RichTextBox
    Friend WithEvents RtbLowAlertMenu As RichTextBox
    Friend WithEvents RtbLowSnoozeMenu As RichTextBox
    Friend WithEvents RtbMainLeft As RichTextBox
    Friend WithEvents RtbMainRight As RichTextBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button

End Class
