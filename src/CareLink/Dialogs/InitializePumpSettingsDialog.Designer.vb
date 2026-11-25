' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class InitializePumpSettingsDialog
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
    Private components As IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(InitializePumpSettingsDialog))
        TableLayoutPanel1 = New TableLayoutPanel()
        OK_Button = New Button()
        Cancel_Button = New Button()
        PumpAitComboBox = New ComboBox()
        SelectAITLabel = New Label()
        UseAITAdvancedDecayCheckBox = New CheckBox()
        InitializeDataGridView = New DataGridView()
        ColumnDeleteRow = New DataGridViewColumnControls.DataGridViewDisableButtonColumn()
        ColumnStart = New DataGridViewComboBoxColumn()
        ColumnEnd = New DataGridViewComboBoxColumn()
        ColumnNumericUpDown = New DataGridViewNumericUpDownColumn()
        ColumnSave = New DataGridViewColumnControls.DataGridViewDisableButtonColumn()
        ErrorProvider1 = New ErrorProvider(components)
        InsulinTypeComboBox = New ComboBox()
        InsulinTypeLabel = New Label()
        InstructionsLabel = New Label()
        TargetSgComboBox = New ComboBox()
        TargetSgValueLabel = New Label()
        TableLayoutPanel1.SuspendLayout()
        CType(InitializeDataGridView, ISupportInitialize).BeginInit()
        CType(ErrorProvider1, ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.Controls.Add(OK_Button, 0, 0)
        TableLayoutPanel1.Controls.Add(Cancel_Button, 1, 0)
        TableLayoutPanel1.Location = New Point(268, 386)
        TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.Size = New Size(265, 33)
        TableLayoutPanel1.TabIndex = 9
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Enabled = False
        OK_Button.Location = New Point(6, 3)
        OK_Button.Margin = New Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(119, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.None
        Cancel_Button.Location = New Point(139, 3)
        Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(119, 27)
        Cancel_Button.TabIndex = 1
        Cancel_Button.Text = "Cancel"
        ' 
        ' PumpAitComboBox
        ' 
        PumpAitComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        PumpAitComboBox.FormattingEnabled = True
        PumpAitComboBox.Location = New Point(111, 5)
        PumpAitComboBox.Name = "PumpAitComboBox"
        PumpAitComboBox.Size = New Size(78, 23)
        PumpAitComboBox.TabIndex = 1
        ' 
        ' SelectAITLabel
        ' 
        SelectAITLabel.AutoSize = True
        SelectAITLabel.Location = New Point(11, 9)
        SelectAITLabel.Name = "SelectAITLabel"
        SelectAITLabel.Size = New Size(97, 15)
        SelectAITLabel.TabIndex = 0
        SelectAITLabel.Text = "Select Pump AIT:"
        ' 
        ' UseAITAdvancedDecayCheckBox
        ' 
        UseAITAdvancedDecayCheckBox.AutoCheck = False
        UseAITAdvancedDecayCheckBox.AutoSize = True
        UseAITAdvancedDecayCheckBox.Checked = True
        UseAITAdvancedDecayCheckBox.CheckState = CheckState.Checked
        UseAITAdvancedDecayCheckBox.Enabled = False
        UseAITAdvancedDecayCheckBox.Location = New Point(11, 34)
        UseAITAdvancedDecayCheckBox.Name = "UseAITAdvancedDecayCheckBox"
        UseAITAdvancedDecayCheckBox.Size = New Size(236, 34)
        UseAITAdvancedDecayCheckBox.TabIndex = 4
        UseAITAdvancedDecayCheckBox.Text = "Use Advanced Decay: Checking this box" & vbCrLf & "decays AIT to more closely match body"
        UseAITAdvancedDecayCheckBox.UseVisualStyleBackColor = True
        ' 
        ' InitializeDataGridView
        ' 
        InitializeDataGridView.AllowUserToAddRows = False
        InitializeDataGridView.AllowUserToResizeColumns = False
        InitializeDataGridView.AllowUserToResizeRows = False
        InitializeDataGridView.CausesValidation = False
        InitializeDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        InitializeDataGridView.Columns.AddRange(New DataGridViewColumn() {ColumnDeleteRow, ColumnStart, ColumnEnd, ColumnNumericUpDown, ColumnSave})
        InitializeDataGridView.Enabled = False
        InitializeDataGridView.Location = New Point(14, 153)
        InitializeDataGridView.Name = "InitializeDataGridView"
        InitializeDataGridView.Size = New Size(519, 220)
        InitializeDataGridView.TabIndex = 8
        ' 
        ' ColumnDeleteRow
        ' 
        ColumnDeleteRow.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        ColumnDeleteRow.HeaderText = ""
        ColumnDeleteRow.Name = "ColumnDeleteRow"
        ColumnDeleteRow.Text = "Delete Row"
        ColumnDeleteRow.UseColumnTextForButtonValue = True
        ColumnDeleteRow.Width = 5
        ' 
        ' ColumnStart
        ' 
        ColumnStart.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        ColumnStart.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        ColumnStart.HeaderText = "Start"
        ColumnStart.Name = "ColumnStart"
        ColumnStart.Resizable = DataGridViewTriState.False
        ColumnStart.Width = 37
        ' 
        ' ColumnEnd
        ' 
        ColumnEnd.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        ColumnEnd.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        ColumnEnd.HeaderText = "End"
        ColumnEnd.Name = "ColumnEnd"
        ColumnEnd.Resizable = DataGridViewTriState.False
        ColumnEnd.Width = 33
        ' 
        ' ColumnNumericUpDown
        ' 
        ColumnNumericUpDown.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        ColumnNumericUpDown.DecimalPlaces = 1
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        ColumnNumericUpDown.DefaultCellStyle = DataGridViewCellStyle1
        ColumnNumericUpDown.HeaderText = "Carb Ratio g/U"
        ColumnNumericUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        ColumnNumericUpDown.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        ColumnNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        ColumnNumericUpDown.Name = "ColumnNumericUpDown"
        ColumnNumericUpDown.Width = 91
        ' 
        ' ColumnSave
        ' 
        ColumnSave.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        ColumnSave.HeaderText = ""
        ColumnSave.Name = "ColumnSave"
        ColumnSave.Resizable = DataGridViewTriState.False
        ColumnSave.Text = "Save"
        ColumnSave.UseColumnTextForButtonValue = True
        ColumnSave.Width = 5
        ' 
        ' ErrorProvider1
        ' 
        ErrorProvider1.ContainerControl = Me
        ' 
        ' InsulinTypeComboBox
        ' 
        InsulinTypeComboBox.CausesValidation = False
        InsulinTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        InsulinTypeComboBox.Enabled = False
        InsulinTypeComboBox.FormattingEnabled = True
        InsulinTypeComboBox.Location = New Point(345, 5)
        InsulinTypeComboBox.Name = "InsulinTypeComboBox"
        InsulinTypeComboBox.Size = New Size(176, 23)
        InsulinTypeComboBox.TabIndex = 3
        ' 
        ' InsulinTypeLabel
        ' 
        InsulinTypeLabel.AutoSize = True
        InsulinTypeLabel.Location = New Point(266, 9)
        InsulinTypeLabel.Name = "InsulinTypeLabel"
        InsulinTypeLabel.Size = New Size(73, 15)
        InsulinTypeLabel.TabIndex = 2
        InsulinTypeLabel.Text = "Insulin Type:"
        ' 
        ' InstructionsLabel
        ' 
        InstructionsLabel.AutoSize = True
        InstructionsLabel.Location = New Point(7, 75)
        InstructionsLabel.Name = "InstructionsLabel"
        InstructionsLabel.Size = New Size(491, 75)
        InstructionsLabel.TabIndex = 7
        InstructionsLabel.Text = resources.GetString("InstructionsLabel.Text")
        ' 
        ' TargetSgComboBox
        ' 
        TargetSgComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        TargetSgComboBox.FormattingEnabled = True
        TargetSgComboBox.Location = New Point(389, 40)
        TargetSgComboBox.Name = "TargetSgComboBox"
        TargetSgComboBox.Size = New Size(132, 23)
        TargetSgComboBox.TabIndex = 6
        ' 
        ' TargetSgValueLabel
        ' 
        TargetSgValueLabel.AutoSize = True
        TargetSgValueLabel.Location = New Point(266, 44)
        TargetSgValueLabel.Name = "TargetSgValueLabel"
        TargetSgValueLabel.Size = New Size(91, 15)
        TargetSgValueLabel.TabIndex = 5
        TargetSgValueLabel.Text = "Target SG Value:"
        ' 
        ' InitializePumpSettingsDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.AutoValidate = AutoValidate.EnableAllowFocusChange
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(547, 430)
        Me.Controls.Add(TargetSgValueLabel)
        Me.Controls.Add(TargetSgComboBox)
        Me.Controls.Add(InstructionsLabel)
        Me.Controls.Add(InsulinTypeLabel)
        Me.Controls.Add(InsulinTypeComboBox)
        Me.Controls.Add(InitializeDataGridView)
        Me.Controls.Add(UseAITAdvancedDecayCheckBox)
        Me.Controls.Add(SelectAITLabel)
        Me.Controls.Add(PumpAitComboBox)
        Me.Controls.Add(TableLayoutPanel1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InitializePumpSettingsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Initialize Pump Settings"
        TableLayoutPanel1.ResumeLayout(False)
        CType(InitializeDataGridView, ISupportInitialize).EndInit()
        CType(ErrorProvider1, ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button
    Friend WithEvents PumpAitComboBox As ComboBox
    Friend WithEvents InsulinTypeLabel As Label
    Friend WithEvents UseAITAdvancedDecayCheckBox As CheckBox
    Friend WithEvents SelectAITLabel As Label
    Friend WithEvents ColumnDeleteRow As DataGridViewColumnControls.DataGridViewDisableButtonColumn
    Friend WithEvents ColumnStart As DataGridViewComboBoxColumn
    Friend WithEvents ColumnEnd As DataGridViewComboBoxColumn
    Friend WithEvents ColumnNumericUpDown As DataGridViewNumericUpDownColumn
    Friend WithEvents ColumnSave As DataGridViewColumnControls.DataGridViewDisableButtonColumn
    Friend WithEvents InitializeDataGridView As DataGridView
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents InsulinTypeComboBox As ComboBox
    Friend WithEvents InstructionsLabel As Label
    Friend WithEvents TargetSgValueLabel As Label
    Friend WithEvents TargetSgComboBox As ComboBox
End Class
