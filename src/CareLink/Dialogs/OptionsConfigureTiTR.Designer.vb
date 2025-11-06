' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OptionsConfigureTiTR
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
        TableLayoutPanel1 = New TableLayoutPanel()
        OK_Button = New Button()
        Cancel_Button = New Button()
        LowThresholdLabel = New Label()
        ThresholdNumericUpDown = New NumericUpDown()
        TreatmentTargetPercentLabel = New Label()
        TreatmentTargetPercentUpDown = New PercentUpDown()
        UnitsComboBox = New ComboBox()
        TableLayoutPanel1.SuspendLayout()
        CType(ThresholdNumericUpDown, ComponentModel.ISupportInitialize).BeginInit()
        CType(TreatmentTargetPercentUpDown, ComponentModel.ISupportInitialize).BeginInit()
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
        TableLayoutPanel1.Location = New Point(204, 89)
        TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.Size = New Size(170, 33)
        TableLayoutPanel1.TabIndex = 0
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
        ' LowThresholdLabel
        ' 
        LowThresholdLabel.AutoSize = True
        LowThresholdLabel.Location = New Point(2, 9)
        LowThresholdLabel.Name = "LowThresholdLabel"
        LowThresholdLabel.Size = New Size(198, 15)
        LowThresholdLabel.TabIndex = 1
        LowThresholdLabel.Text = "Time In Tight Range Low Threshold:"
        ' 
        ' ThresholdNumericUpDown
        ' 
        ThresholdNumericUpDown.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        ThresholdNumericUpDown.Location = New Point(252, 5)
        ThresholdNumericUpDown.Maximum = New Decimal(New Integer() {70, 0, 0, 0})
        ThresholdNumericUpDown.Minimum = New Decimal(New Integer() {60, 0, 0, 0})
        ThresholdNumericUpDown.Name = "ThresholdNumericUpDown"
        ThresholdNumericUpDown.Size = New Size(45, 23)
        ThresholdNumericUpDown.TabIndex = 2
        ThresholdNumericUpDown.TextAlign = HorizontalAlignment.Right
        ThresholdNumericUpDown.UpDownAlign = LeftRightAlignment.Left
        ThresholdNumericUpDown.Value = New Decimal(New Integer() {70, 0, 0, 0})
        ' 
        ' TreatmentTargetPercentLabel
        ' 
        TreatmentTargetPercentLabel.AutoSize = True
        TreatmentTargetPercentLabel.Location = New Point(2, 58)
        TreatmentTargetPercentLabel.Name = "TreatmentTargetPercentLabel"
        TreatmentTargetPercentLabel.Size = New Size(182, 15)
        TreatmentTargetPercentLabel.TabIndex = 1
        TreatmentTargetPercentLabel.Text = " TITR Treatment Target: 40%-70%"
        ' 
        ' TreatmentTargetPercentUpDown
        ' 
        TreatmentTargetPercentUpDown.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        TreatmentTargetPercentUpDown.Location = New Point(325, 54)
        TreatmentTargetPercentUpDown.Maximum = New Decimal(New Integer() {70, 0, 0, 0})
        TreatmentTargetPercentUpDown.Minimum = New Decimal(New Integer() {45, 0, 0, 0})
        TreatmentTargetPercentUpDown.Name = "TreatmentTargetPercentUpDown"
        TreatmentTargetPercentUpDown.Size = New Size(45, 23)
        TreatmentTargetPercentUpDown.TabIndex = 2
        TreatmentTargetPercentUpDown.TextAlign = HorizontalAlignment.Right
        TreatmentTargetPercentUpDown.UpDownAlign = LeftRightAlignment.Left
        TreatmentTargetPercentUpDown.Value = New Decimal(New Integer() {70, 0, 0, 0})
        ' 
        ' UnitsComboBox
        ' 
        UnitsComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        UnitsComboBox.FormattingEnabled = True
        UnitsComboBox.Items.AddRange(New Object() {"mg/dl", "mmol/L"})
        UnitsComboBox.Location = New Point(318, 5)
        UnitsComboBox.MaxDropDownItems = 2
        UnitsComboBox.Name = "UnitsComboBox"
        UnitsComboBox.Size = New Size(56, 23)
        UnitsComboBox.TabIndex = 3
        ' 
        ' OptionsConfigureTiTR
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(388, 136)
        Me.Controls.Add(UnitsComboBox)
        Me.Controls.Add(TreatmentTargetPercentUpDown)
        Me.Controls.Add(TreatmentTargetPercentLabel)
        Me.Controls.Add(ThresholdNumericUpDown)
        Me.Controls.Add(LowThresholdLabel)
        Me.Controls.Add(TableLayoutPanel1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsConfigureTiTR"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Configure Time In Tight Range (TiTR)"
        TableLayoutPanel1.ResumeLayout(False)
        CType(ThresholdNumericUpDown, ComponentModel.ISupportInitialize).EndInit()
        CType(TreatmentTargetPercentUpDown, ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents LowThresholdLabel As Label
    Friend WithEvents ThresholdNumericUpDown As NumericUpDown
    Friend WithEvents TreatmentTargetPercentLabel As Label
    Friend WithEvents TreatmentTargetPercentUpDown As PercentUpDown
    Friend WithEvents UnitsComboBox As ComboBox

End Class
