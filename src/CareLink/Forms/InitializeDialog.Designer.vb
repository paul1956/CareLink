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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Me.TableLayoutPanel1 = New TableLayoutPanel()
        Me.OK_Button = New Button()
        Me.Cancel_Button = New Button()
        Me.ComboBoxAITAdvancedDelay = New ComboBox()
        Me.AITLabel = New Label()
        Me.CheckBox1 = New CheckBox()
        Me.InitializeDataGridView = New DataGridView()
        Me.ColumnDeleteRow = New DataGridViewColumnControls.DataGridViewDisableButtonColumn()
        Me.ColumnStart = New DataGridViewComboBoxColumn()
        Me.ColumnEnd = New DataGridViewComboBoxColumn()
        Me.ColumnNumericUpDown = New DataGridViewNumericUpDownColumn()
        Me.ColumnSave = New DataGridViewColumnControls.DataGridViewDisableButtonColumn()
        Me.UseAdvancedAITDecayHelpLabel = New Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.InitializeDataGridView, ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New Point(323, 316)
        Me.TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        Me.TableLayoutPanel1.Size = New Size(170, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        Me.OK_Button.Anchor = AnchorStyles.None
        Me.OK_Button.Enabled = False
        Me.OK_Button.Location = New Point(4, 3)
        Me.OK_Button.Margin = New Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New Size(77, 27)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"' 
        ' Cancel_Button
        ' 
        Me.Cancel_Button.Anchor = AnchorStyles.None
        Me.Cancel_Button.Location = New Point(89, 3)
        Me.Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New Size(77, 27)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"' 
        ' ComboBoxAITAdvancedDelay
        ' 
        Me.ComboBoxAITAdvancedDelay.FormattingEnabled = True
        Me.ComboBoxAITAdvancedDelay.Location = New Point(122, 5)
        Me.ComboBoxAITAdvancedDelay.Name = "ComboBoxAITAdvancedDelay"
        Me.ComboBoxAITAdvancedDelay.Size = New Size(121, 23)
        Me.ComboBoxAITAdvancedDelay.TabIndex = 1
        ' 
        ' AITLabel
        ' 
        Me.AITLabel.AutoSize = True
        Me.AITLabel.Location = New Point(16, 9)
        Me.AITLabel.Name = "AITLabel"
        Me.AITLabel.Size = New Size(93, 15)
        Me.AITLabel.TabIndex = 2
        Me.AITLabel.Text = "Select Pump AIT"' 
        ' CheckBox1
        ' 
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = CheckState.Checked
        Me.CheckBox1.Location = New Point(337, 34)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New Size(156, 19)
        Me.CheckBox1.TabIndex = 0
        Me.CheckBox1.Text = "Use Advanced AIT Decay"
        Me.CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' InitializeDataGridView
        ' 
        Me.InitializeDataGridView.AllowUserToAddRows = False
        Me.InitializeDataGridView.AllowUserToResizeColumns = False
        Me.InitializeDataGridView.AllowUserToResizeRows = False
        Me.InitializeDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.InitializeDataGridView.Columns.AddRange(New DataGridViewColumn() {Me.ColumnDeleteRow, Me.ColumnStart, Me.ColumnEnd, Me.ColumnNumericUpDown, Me.ColumnSave})
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
        ' InitializeDialog
        ' 
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New Size(507, 363)
        Me.Controls.Add(Me.UseAdvancedAITDecayHelpLabel)
        Me.Controls.Add(Me.InitializeDataGridView)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.AITLabel)
        Me.Controls.Add(Me.ComboBoxAITAdvancedDelay)
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
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents ComboBoxAITAdvancedDelay As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents AITLabel As Label
    Friend WithEvents UseAdvancedAITDecayHelpLabel As Label
    Friend WithEvents ColumnDeleteRow As DataGridViewColumnControls.DataGridViewDisableButtonColumn
    Friend WithEvents ColumnStart As DataGridViewComboBoxColumn
    Friend WithEvents ColumnEnd As DataGridViewComboBoxColumn
    Friend WithEvents ColumnNumericUpDown As DataGridViewNumericUpDownColumn
    Friend WithEvents ColumnSave As DataGridViewColumnControls.DataGridViewDisableButtonColumn
    Friend WithEvents InitializeDataGridView As DataGridView
End Class
