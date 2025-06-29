<CompilerServices.DesignerGenerated()>
Partial Class OptionsDialog
    Inherits Form

    'Form overrides dispose to clean up the component list.
    <Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsDialog))
        TableLayoutPanel1 = New TableLayoutPanel()
        OK_Button = New Button()
        Cancel_Button = New Button()
        UpdateForegroundButton = New Button()
        ItemNameComboBox = New NameColorComboBox()
        KnownColorsComboBox1 = New KnownColorComboBox()
        TableLayoutPanel2 = New TableLayoutPanel()
        TableLayoutPanel1.SuspendLayout()
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
        TableLayoutPanel1.Location = New Point(303, 99)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.Size = New Size(170, 33)
        TableLayoutPanel1.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Location = New Point(9, 5)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(66, 23)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.None
        Cancel_Button.DialogResult = DialogResult.Cancel
        Cancel_Button.Location = New Point(94, 5)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(66, 23)
        Cancel_Button.TabIndex = 1
        Cancel_Button.Text = "Cancel"
        ' 
        ' ItemNameComboBox
        ' 
        ItemNameComboBox.DropDownHeight = 400
        ItemNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        ItemNameComboBox.DropDownWidth = 300
        ItemNameComboBox.FormattingEnabled = True
        ItemNameComboBox.IntegralHeight = False
        ItemNameComboBox.Location = New Point(10, 56)
        ItemNameComboBox.MaxDropDownItems = 20
        ItemNameComboBox.Name = "ItemNameComboBox"
        ItemNameComboBox.SelectedItem = CType(resources.GetObject("ItemNameComboBox.SelectedItem"), KeyValuePair(Of String, KnownColor))
        ItemNameComboBox.Size = New Size(287, 23)
        ItemNameComboBox.TabIndex = 3
        '
        ' UpdateForegroundButton
        ' 
        UpdateForegroundButton.Location = New Point(3, 3)
        UpdateForegroundButton.Name = "UpdateForegroundButton"
        UpdateForegroundButton.Size = New Size(159, 27)
        UpdateForegroundButton.TabIndex = 5
        UpdateForegroundButton.Text = "Update Foreground Color"
        ' 
        ' 
        ' KnownColorsComboBox1
        ' 
        KnownColorsComboBox1.DropDownHeight = 120
        KnownColorsComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        KnownColorsComboBox1.FormattingEnabled = True
        KnownColorsComboBox1.IntegralHeight = False
        KnownColorsComboBox1.Location = New Point(303, 56)
        KnownColorsComboBox1.Name = "KnownColorsComboBox1"
        KnownColorsComboBox1.SelectedItem = CType(resources.GetObject("KnownColorsComboBox1.SelectedItem"), KeyValuePair(Of String, KnownColor))
        KnownColorsComboBox1.Size = New Size(169, 23)
        KnownColorsComboBox1.TabIndex = 6
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.Controls.Add(UpdateForegroundButton, 0, 0)
        TableLayoutPanel2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel2.ColumnCount = 1
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanel2.Location = New Point(303, 7)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanel2.Size = New Size(180, 32)
        TableLayoutPanel2.TabIndex = 7
        ' 
        ' OptionsDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(486, 143)
        Me.Controls.Add(KnownColorsComboBox1)
        Me.Controls.Add(ItemNameComboBox)
        Me.Controls.Add(TableLayoutPanel1)
        Me.Controls.Add(TableLayoutPanel2)
        Me.Font = New Font("Segoe UI", 9.0F)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Color Editor"
        TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button
    Friend WithEvents ItemNameComboBox As NameColorComboBox
    Friend WithEvents UpdateForegroundButton As Button
    Friend WithEvents OptionsBindingSource As BindingSource
    Friend WithEvents KnownColorsComboBox1 As KnownColorComboBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
End Class
