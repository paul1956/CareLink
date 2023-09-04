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
        Dim DataGridViewCellStyle8 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As DataGridViewCellStyle = New DataGridViewCellStyle()
        RtbMainLeft = New RichTextBox()
        RtbMainRight = New RichTextBox()
        SplitContainer1 = New SplitContainer()
        OK_Button = New Button()
        RtbLowSnoozeMenu = New RichTextBox()
        DataGridViewLowAlert = New DataGridView()
        RtbLowAlertMenu = New RichTextBox()
        RtbHighSnoozeMenu = New RichTextBox()
        DataGridViewHighAlert = New DataGridView()
        RtbHighAlertMenu = New RichTextBox()
        ColumnStartTimeHigh = New DataGridViewTextBoxColumn()
        ColumnEndTimeHigh = New DataGridViewTextBoxColumn()
        ColumnHighLimit = New DataGridViewTextBoxColumn()
        ColumnAlertBeforeHigh = New DataGridViewCheckBoxColumn()
        ColumnTimeBeforeHighText = New DataGridViewTextBoxColumn()
        ColumnAlertOnHigh = New DataGridViewCheckBoxColumn()
        ColumnRiseAlert = New DataGridViewCheckBoxColumn()
        ColumnRiseLimit = New DataGridViewTextBoxColumn()
        ColumnStartTimeLow = New DataGridViewTextBoxColumn()
        ColumnEndTimeLow = New DataGridViewTextBoxColumn()
        ColumnLowLimit = New DataGridViewTextBoxColumn()
        ColumnSuspend = New DataGridViewTextBoxColumn()
        ColumnAlertOnLow = New DataGridViewCheckBoxColumn()
        ColumnAlertBeforeLow = New DataGridViewCheckBoxColumn()
        ColumnResumeBasalAlert = New DataGridViewCheckBoxColumn()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(DataGridViewLowAlert, ComponentModel.ISupportInitialize).BeginInit()
        CType(DataGridViewHighAlert, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' RtbMainLeft
        ' 
        RtbMainLeft.BorderStyle = BorderStyle.None
        RtbMainLeft.Dock = DockStyle.Left
        RtbMainLeft.Location = New Point(0, 0)
        RtbMainLeft.Name = "RtbMainLeft"
        RtbMainLeft.Size = New Size(586, 530)
        RtbMainLeft.TabIndex = 0
        RtbMainLeft.Text = ""
        ' 
        ' RtbMainRight
        ' 
        RtbMainRight.BorderStyle = BorderStyle.None
        RtbMainRight.Dock = DockStyle.Right
        RtbMainRight.Location = New Point(588, 0)
        RtbMainRight.Name = "RtbMainRight"
        RtbMainRight.Size = New Size(586, 530)
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
        SplitContainer1.Panel2.Controls.Add(OK_Button)
        SplitContainer1.Panel2.Controls.Add(RtbLowSnoozeMenu)
        SplitContainer1.Panel2.Controls.Add(DataGridViewLowAlert)
        SplitContainer1.Panel2.Controls.Add(RtbLowAlertMenu)
        SplitContainer1.Panel2.Controls.Add(RtbHighSnoozeMenu)
        SplitContainer1.Panel2.Controls.Add(DataGridViewHighAlert)
        SplitContainer1.Panel2.Controls.Add(RtbHighAlertMenu)
        SplitContainer1.Size = New Size(1174, 951)
        SplitContainer1.SplitterDistance = 530
        SplitContainer1.TabIndex = 1
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Location = New Point(1083, 385)
        OK_Button.Margin = New Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(77, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' RtbLowSnoozeMenu
        ' 
        RtbLowSnoozeMenu.Font = New Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point)
        RtbLowSnoozeMenu.Location = New Point(0, 377)
        RtbLowSnoozeMenu.Name = "RtbLowSnoozeMenu"
        RtbLowSnoozeMenu.Size = New Size(1069, 35)
        RtbLowSnoozeMenu.TabIndex = 2
        RtbLowSnoozeMenu.Text = "SNOOZE Menu (Low Alert)"
        ' 
        ' DataGridViewLowAlert
        ' 
        DataGridViewLowAlert.AllowUserToAddRows = False
        DataGridViewLowAlert.AllowUserToDeleteRows = False
        DataGridViewLowAlert.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewLowAlert.Columns.AddRange(New DataGridViewColumn() {ColumnStartTimeLow, ColumnEndTimeLow, ColumnLowLimit, ColumnSuspend, ColumnAlertOnLow, ColumnAlertBeforeLow, ColumnResumeBasalAlert})
        DataGridViewLowAlert.Location = New Point(0, 242)
        DataGridViewLowAlert.Name = "DataGridViewLowAlert"
        DataGridViewLowAlert.ReadOnly = True
        DataGridViewLowAlert.RowHeadersVisible = False
        DataGridViewLowAlert.RowTemplate.Height = 25
        DataGridViewLowAlert.Size = New Size(1171, 135)
        DataGridViewLowAlert.TabIndex = 4
        ' 
        ' RtbLowAlertMenu
        ' 
        RtbLowAlertMenu.Font = New Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point)
        RtbLowAlertMenu.Location = New Point(0, 207)
        RtbLowAlertMenu.Name = "RtbLowAlertMenu"
        RtbLowAlertMenu.Size = New Size(1171, 35)
        RtbLowAlertMenu.TabIndex = 3
        RtbLowAlertMenu.Text = "LOW ALERT Menu"
        ' 
        ' RtbHighSnoozeMenu
        ' 
        RtbHighSnoozeMenu.Font = New Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point)
        RtbHighSnoozeMenu.Location = New Point(1, 172)
        RtbHighSnoozeMenu.Name = "RtbHighSnoozeMenu"
        RtbHighSnoozeMenu.Size = New Size(1171, 35)
        RtbHighSnoozeMenu.TabIndex = 2
        RtbHighSnoozeMenu.Text = "SNOOZE Menu (High Alert)"
        ' 
        ' DataGridViewHighAlert
        ' 
        DataGridViewHighAlert.AllowUserToAddRows = False
        DataGridViewHighAlert.AllowUserToDeleteRows = False
        DataGridViewHighAlert.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewHighAlert.Columns.AddRange(New DataGridViewColumn() {ColumnStartTimeHigh, ColumnEndTimeHigh, ColumnHighLimit, ColumnAlertBeforeHigh, ColumnTimeBeforeHighText, ColumnAlertOnHigh, ColumnRiseAlert, ColumnRiseLimit})
        DataGridViewHighAlert.Location = New Point(1, 37)
        DataGridViewHighAlert.Name = "DataGridViewHighAlert"
        DataGridViewHighAlert.ReadOnly = True
        DataGridViewHighAlert.RowHeadersVisible = False
        DataGridViewHighAlert.RowTemplate.Height = 25
        DataGridViewHighAlert.Size = New Size(1171, 135)
        DataGridViewHighAlert.TabIndex = 0
        ' 
        ' RtbHighAlertMenu
        ' 
        RtbHighAlertMenu.Dock = DockStyle.Top
        RtbHighAlertMenu.Font = New Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point)
        RtbHighAlertMenu.Location = New Point(0, 0)
        RtbHighAlertMenu.Name = "RtbHighAlertMenu"
        RtbHighAlertMenu.Size = New Size(1174, 35)
        RtbHighAlertMenu.TabIndex = 1
        RtbHighAlertMenu.Text = "HIGH ALERT"
        ' 
        ' ColumnStartTimeHigh
        ' 
        DataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle8.WrapMode = DataGridViewTriState.False
        ColumnStartTimeHigh.DefaultCellStyle = DataGridViewCellStyle8
        ColumnStartTimeHigh.HeaderText = "Start Time"
        ColumnStartTimeHigh.Name = "ColumnStartTimeHigh"
        ColumnStartTimeHigh.ReadOnly = True
        ColumnStartTimeHigh.Resizable = DataGridViewTriState.False
        ColumnStartTimeHigh.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnEndTimeHigh
        ' 
        DataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle9.WrapMode = DataGridViewTriState.False
        ColumnEndTimeHigh.DefaultCellStyle = DataGridViewCellStyle9
        ColumnEndTimeHigh.HeaderText = "End Time"
        ColumnEndTimeHigh.Name = "ColumnEndTimeHigh"
        ColumnEndTimeHigh.ReadOnly = True
        ColumnEndTimeHigh.Resizable = DataGridViewTriState.False
        ColumnEndTimeHigh.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnHighLimit
        ' 
        ColumnHighLimit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle10.WrapMode = DataGridViewTriState.False
        ColumnHighLimit.DefaultCellStyle = DataGridViewCellStyle10
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
        DataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle11.NullValue = False
        DataGridViewCellStyle11.WrapMode = DataGridViewTriState.False
        DataGridViewCellStyle11.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle11.WrapMode = DataGridViewTriState.False
        ColumnAlertBeforeHigh.DefaultCellStyle = DataGridViewCellStyle11
        ColumnAlertBeforeHigh.FillWeight = 50.0F
        ColumnAlertBeforeHigh.HeaderText = "Alert Before High"
        ColumnAlertBeforeHigh.MinimumWidth = 125
        ColumnAlertBeforeHigh.Name = "ColumnAlertBeforeHigh"
        ColumnAlertBeforeHigh.ReadOnly = True
        ColumnAlertBeforeHigh.Resizable = DataGridViewTriState.False
        ColumnAlertBeforeHigh.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnAlertBeforeHigh.Width = 125
        ' 
        ' ColumnTimeBeforeHighText
        ' 
        ColumnTimeBeforeHighText.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        DataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle12.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle12.WrapMode = DataGridViewTriState.False
        ColumnTimeBeforeHighText.DefaultCellStyle = DataGridViewCellStyle12
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
        DataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle13.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle13.NullValue = False
        DataGridViewCellStyle13.WrapMode = DataGridViewTriState.False
        ColumnAlertOnHigh.DefaultCellStyle = DataGridViewCellStyle13
        ColumnAlertOnHigh.FillWeight = 50.0F
        ColumnAlertOnHigh.HeaderText = "Alert On High"
        ColumnAlertOnHigh.MinimumWidth = 125
        ColumnAlertOnHigh.Name = "ColumnAlertOnHigh"
        ColumnAlertOnHigh.ReadOnly = True
        ColumnAlertOnHigh.Resizable = DataGridViewTriState.False
        ColumnAlertOnHigh.Width = 125
        ColumnAlertOnHigh.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnRiseAlert
        ' 
        ColumnRiseAlert.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle14.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle14.NullValue = False
        DataGridViewCellStyle14.WrapMode = DataGridViewTriState.False
        ColumnRiseAlert.DefaultCellStyle = DataGridViewCellStyle14
        ColumnRiseAlert.FillWeight = 75.0F
        ColumnRiseAlert.HeaderText = "Rise Alert"
        ColumnRiseAlert.MinimumWidth = 125
        ColumnRiseAlert.Name = "ColumnRiseAlert"
        ColumnRiseAlert.ReadOnly = True
        ColumnRiseAlert.Resizable = DataGridViewTriState.False
        ColumnRiseAlert.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnRiseAlert.Width = 125
        ' 
        ' ColumnRiseLimit
        ' 
        ColumnRiseLimit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle15.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle15.WrapMode = DataGridViewTriState.False
        ColumnRiseLimit.DefaultCellStyle = DataGridViewCellStyle15
        ColumnRiseLimit.HeaderText = "Rise Limit"
        ColumnRiseLimit.MinimumWidth = 125
        ColumnRiseLimit.Name = "ColumnRiseLimit"
        ColumnRiseLimit.ReadOnly = True
        ColumnRiseLimit.Resizable = DataGridViewTriState.False
        ColumnRiseLimit.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnRiseLimit.Width = 125
        ' 
        ' ColumnStartTimeLow
        ' 
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        ColumnStartTimeLow.DefaultCellStyle = DataGridViewCellStyle1
        ColumnStartTimeLow.Frozen = True
        ColumnStartTimeLow.HeaderText = "Start Time"
        ColumnStartTimeLow.Name = "ColumnStartTimeLow"
        ColumnStartTimeLow.ReadOnly = True
        ColumnStartTimeLow.Resizable = DataGridViewTriState.False
        ColumnStartTimeLow.SortMode = DataGridViewColumnSortMode.NotSortable
        ' 
        ' ColumnEndTimeLow
        ' 
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        ColumnEndTimeLow.DefaultCellStyle = DataGridViewCellStyle2
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
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        ColumnLowLimit.DefaultCellStyle = DataGridViewCellStyle3
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
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle4.WrapMode = DataGridViewTriState.False
        ColumnSuspend.DefaultCellStyle = DataGridViewCellStyle4
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
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle5.NullValue = False
        DataGridViewCellStyle5.WrapMode = DataGridViewTriState.False
        ColumnAlertOnLow.DefaultCellStyle = DataGridViewCellStyle5
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
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle6.NullValue = False
        DataGridViewCellStyle6.WrapMode = DataGridViewTriState.False
        ColumnAlertBeforeLow.DefaultCellStyle = DataGridViewCellStyle6
        ColumnAlertBeforeLow.HeaderText = "Alert before low"
        ColumnAlertBeforeLow.MinimumWidth = 125
        ColumnAlertBeforeLow.Name = "ColumnAlertBeforeLow"
        ColumnAlertBeforeLow.ReadOnly = True
        ColumnAlertBeforeLow.Resizable = DataGridViewTriState.False
        ColumnAlertBeforeLow.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnAlertBeforeLow.Width = 125
        ' 
        ' ColumnResumeBasalAlert
        ' 
        ColumnResumeBasalAlert.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        DataGridViewCellStyle7.NullValue = False
        DataGridViewCellStyle7.WrapMode = DataGridViewTriState.False
        ColumnResumeBasalAlert.DefaultCellStyle = DataGridViewCellStyle7
        ColumnResumeBasalAlert.HeaderText = "Resume Basal Alert"
        ColumnResumeBasalAlert.MinimumWidth = 125
        ColumnResumeBasalAlert.Name = "ColumnResumeBasalAlert"
        ColumnResumeBasalAlert.ReadOnly = True
        ColumnResumeBasalAlert.Resizable = DataGridViewTriState.False
        ColumnResumeBasalAlert.SortMode = DataGridViewColumnSortMode.NotSortable
        ColumnResumeBasalAlert.Width = 125
        ' 
        ' PumpSetupDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1174, 951)
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
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents DataGridViewHighAlert As DataGridView
    Friend WithEvents DataGridViewLowAlert As DataGridView
    Friend WithEvents OK_Button As Button
    Friend WithEvents RtbHighAlertMenu As RichTextBox
    Friend WithEvents RtbHighSnoozeMenu As RichTextBox
    Friend WithEvents RtbLowAlertMenu As RichTextBox
    Friend WithEvents RtbLowSnoozeMenu As RichTextBox
    Friend WithEvents RtbMainLeft As RichTextBox
    Friend WithEvents RtbMainRight As RichTextBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ColumnStartTimeLow As DataGridViewTextBoxColumn
    Friend WithEvents ColumnEndTimeLow As DataGridViewTextBoxColumn
    Friend WithEvents ColumnLowLimit As DataGridViewTextBoxColumn
    Friend WithEvents ColumnSuspend As DataGridViewTextBoxColumn
    Friend WithEvents ColumnAlertOnLow As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnAlertBeforeLow As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnResumeBasalAlert As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnStartTimeHigh As DataGridViewTextBoxColumn
    Friend WithEvents ColumnEndTimeHigh As DataGridViewTextBoxColumn
    Friend WithEvents ColumnHighLimit As DataGridViewTextBoxColumn
    Friend WithEvents ColumnAlertBeforeHigh As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnTimeBeforeHighText As DataGridViewTextBoxColumn
    Friend WithEvents ColumnAlertOnHigh As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnRiseAlert As DataGridViewCheckBoxColumn
    Friend WithEvents ColumnRiseLimit As DataGridViewTextBoxColumn
End Class
