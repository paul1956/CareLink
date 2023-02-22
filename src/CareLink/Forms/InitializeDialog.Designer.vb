' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InitializeDialog
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
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Me.TableLayoutPanel1 = New TableLayoutPanel()
        Me.OK_Button = New Button()
        Me.Cancel_Button = New Button()
        Me.AitAdvancedDelayComboBox = New ComboBox()
        Me.SelectAITLabel = New Label()
        Me.UseAITAdvancedDecayCheckBox = New CheckBox()
        Me.InitializeDataGridView = New DataGridView()
        Me.ColumnDeleteRow = New DataGridViewColumnControls.DataGridViewDisableButtonColumn()
        Me.ColumnStart = New DataGridViewComboBoxColumn()
        Me.ColumnEnd = New DataGridViewComboBoxColumn()
        Me.ColumnNumericUpDown = New DataGridViewNumericUpDownColumn()
        Me.ColumnSave = New DataGridViewColumnControls.DataGridViewDisableButtonColumn()
        Me.UseAdvancedAITDecayHelpLabel = New Label()
        Me.ErrorProvider1 = New ErrorProvider(components)
        Me.InsulinTypeComboBox = New ComboBox()
        Me.InsulinTypeLabel = New Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.InitializeDataGridView, ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' TableLayoutPanel1
        ' 
        Me.TableLayoutPanel1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        Me.TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New Point(228, 316)
        Me.TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        Me.TableLayoutPanel1.Size = New Size(265, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        Me.OK_Button.Anchor = AnchorStyles.None
        Me.OK_Button.Enabled = False
        Me.OK_Button.Location = New Point(6, 3)
        Me.OK_Button.Margin = New Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New Size(119, 27)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"' 
        ' Cancel_Button
        ' 
        Me.Cancel_Button.Anchor = AnchorStyles.None
        Me.Cancel_Button.Location = New Point(139, 3)
        Me.Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New Size(119, 27)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"' 
        ' AitAdvancedDelayComboBox
        ' 
        Me.AitAdvancedDelayComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        Me.AitAdvancedDelayComboBox.FormattingEnabled = True
        Me.AitAdvancedDelayComboBox.Location = New Point(117, 5)
        Me.AitAdvancedDelayComboBox.Name = "AitAdvancedDelayComboBox"
        Me.AitAdvancedDelayComboBox.Size = New Size(78, 23)
        Me.AitAdvancedDelayComboBox.TabIndex = 0
        ' 
        ' SelectAITLabel
        ' 
        Me.SelectAITLabel.AutoSize = True
        Me.SelectAITLabel.Location = New Point(18, 9)
        Me.SelectAITLabel.Name = "SelectAITLabel"
        Me.SelectAITLabel.Size = New Size(96, 15)
        Me.SelectAITLabel.TabIndex = 2
        Me.SelectAITLabel.Text = "Select Pump AIT:"' 
        ' UseAITAdvancedDecayCheckBox
        ' 
        Me.UseAITAdvancedDecayCheckBox.AutoCheck = False
        Me.UseAITAdvancedDecayCheckBox.AutoSize = True
        Me.UseAITAdvancedDecayCheckBox.Checked = True
        Me.UseAITAdvancedDecayCheckBox.CheckState = CheckState.Indeterminate
        Me.UseAITAdvancedDecayCheckBox.Enabled = False
        Me.UseAITAdvancedDecayCheckBox.Location = New Point(337, 34)
        Me.UseAITAdvancedDecayCheckBox.Name = "UseAITAdvancedDecayCheckBox"
        Me.UseAITAdvancedDecayCheckBox.Size = New Size(156, 19)
        Me.UseAITAdvancedDecayCheckBox.TabIndex = 2
        Me.UseAITAdvancedDecayCheckBox.Text = "Use Advanced AIT Decay"
        Me.UseAITAdvancedDecayCheckBox.UseVisualStyleBackColor = True
        ' 
        ' InitializeDataGridView
        ' 
        Me.InitializeDataGridView.AllowUserToAddRows = False
        Me.InitializeDataGridView.AllowUserToResizeColumns = False
        Me.InitializeDataGridView.AllowUserToResizeRows = False
        Me.InitializeDataGridView.CausesValidation = False
        Me.InitializeDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.InitializeDataGridView.Columns.AddRange(New DataGridViewColumn() {Me.ColumnDeleteRow, Me.ColumnStart, Me.ColumnEnd, Me.ColumnNumericUpDown, Me.ColumnSave})
        Me.InitializeDataGridView.Enabled = False
        Me.InitializeDataGridView.Location = New Point(14, 65)
        Me.InitializeDataGridView.Name = "InitializeDataGridView"
        Me.InitializeDataGridView.RowTemplate.Height = 25
        Me.InitializeDataGridView.Size = New Size(481, 245)
        Me.InitializeDataGridView.TabIndex = 3
        ' 
        ' ColumnDeleteRow
        ' 
        Me.ColumnDeleteRow.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.ColumnDeleteRow.HeaderText = ""
        Me.ColumnDeleteRow.Name = "ColumnDeleteRow"
        Me.ColumnDeleteRow.Text = "Delete Row"
        Me.ColumnDeleteRow.UseColumnTextForButtonValue = True
        Me.ColumnDeleteRow.Width = 5
        ' 
        ' ColumnStart
        ' 
        Me.ColumnStart.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.ColumnStart.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        Me.ColumnStart.HeaderText = "Start"
        Me.ColumnStart.Name = "ColumnStart"
        Me.ColumnStart.Resizable = DataGridViewTriState.False
        Me.ColumnStart.Width = 37
        ' 
        ' ColumnEnd
        ' 
        Me.ColumnEnd.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.ColumnEnd.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        Me.ColumnEnd.HeaderText = "End"
        Me.ColumnEnd.Name = "ColumnEnd"
        Me.ColumnEnd.Resizable = DataGridViewTriState.False
        Me.ColumnEnd.Width = 33
        ' 
        ' ColumnNumericUpDown
        ' 
        Me.ColumnNumericUpDown.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.ColumnNumericUpDown.DecimalPlaces = 1
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.ColumnNumericUpDown.DefaultCellStyle = DataGridViewCellStyle1
        Me.ColumnNumericUpDown.HeaderText = "Carb Ratio g/U"
        Me.ColumnNumericUpDown.Increment = New [Decimal](New Integer() {1, 0, 0, 65536})
        Me.ColumnNumericUpDown.Maximum = New [Decimal](New Integer() {25, 0, 0, 0})
        Me.ColumnNumericUpDown.Minimum = New [Decimal](New Integer() {1, 0, 0, 0})
        Me.ColumnNumericUpDown.Name = "ColumnNumericUpDown"
        Me.ColumnNumericUpDown.Width = 91
        ' 
        ' ColumnSave
        ' 
        Me.ColumnSave.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Me.ColumnSave.HeaderText = ""
        Me.ColumnSave.Name = "ColumnSave"
        Me.ColumnSave.Resizable = DataGridViewTriState.False
        Me.ColumnSave.Text = "Save"
        Me.ColumnSave.UseColumnTextForButtonValue = True
        Me.ColumnSave.Width = 5
        ' 
        ' UseAdvancedAITDecayHelpLabel
        ' 
        Me.UseAdvancedAITDecayHelpLabel.AutoSize = True
        Me.UseAdvancedAITDecayHelpLabel.Location = New Point(16, 36)
        Me.UseAdvancedAITDecayHelpLabel.Name = "UseAdvancedAITDecayHelpLabel"
        Me.UseAdvancedAITDecayHelpLabel.Size = New Size(312, 15)
        Me.UseAdvancedAITDecayHelpLabel.TabIndex = 4
        Me.UseAdvancedAITDecayHelpLabel.Text = "Checking this box decays AIT to more closely match body"' 
        ' ErrorProvider1
        ' 
        Me.ErrorProvider1.ContainerControl = Me
        ' 
        ' InsulinTypeComboBox
        ' 
        Me.InsulinTypeComboBox.CausesValidation = False
        Me.InsulinTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        Me.InsulinTypeComboBox.Enabled = False
        Me.InsulinTypeComboBox.FormattingEnabled = True
        Me.InsulinTypeComboBox.Location = New Point(306, 5)
        Me.InsulinTypeComboBox.Name = "InsulinTypeComboBox"
        Me.InsulinTypeComboBox.Size = New Size(181, 23)
        Me.InsulinTypeComboBox.TabIndex = 1
        ' 
        ' InsulinTypeLabel
        ' 
        Me.InsulinTypeLabel.AutoSize = True
        Me.InsulinTypeLabel.Location = New Point(228, 9)
        Me.InsulinTypeLabel.Name = "InsulinTypeLabel"
        Me.InsulinTypeLabel.Size = New Size(72, 15)
        Me.InsulinTypeLabel.TabIndex = 6
        Me.InsulinTypeLabel.Text = "Insulin Type:"' 
        ' InitializeDialog
        ' 
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.AutoValidate = AutoValidate.EnableAllowFocusChange
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New Size(507, 363)
        Me.Controls.Add(Me.InsulinTypeLabel)
        Me.Controls.Add(Me.InsulinTypeComboBox)
        Me.Controls.Add(Me.UseAdvancedAITDecayHelpLabel)
        Me.Controls.Add(Me.InitializeDataGridView)
        Me.Controls.Add(Me.UseAITAdvancedDecayCheckBox)
        Me.Controls.Add(Me.SelectAITLabel)
        Me.Controls.Add(Me.AitAdvancedDelayComboBox)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InitializeDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Initialize CareLink"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.InitializeDataGridView, ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents AitAdvancedDelayComboBox As ComboBox
    Friend WithEvents InsulinTypeLabel As Label
    Friend WithEvents UseAITAdvancedDecayCheckBox As CheckBox
    Friend WithEvents SelectAITLabel As Label
    Friend WithEvents UseAdvancedAITDecayHelpLabel As Label
    Friend WithEvents ColumnDeleteRow As DataGridViewColumnControls.DataGridViewDisableButtonColumn
    Friend WithEvents ColumnStart As DataGridViewComboBoxColumn
    Friend WithEvents ColumnEnd As DataGridViewComboBoxColumn
    Friend WithEvents ColumnNumericUpDown As DataGridViewNumericUpDownColumn
    Friend WithEvents ColumnSave As DataGridViewColumnControls.DataGridViewDisableButtonColumn
    Friend WithEvents InitializeDataGridView As DataGridView
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents InsulinTypeComboBox As ComboBox
End Class
