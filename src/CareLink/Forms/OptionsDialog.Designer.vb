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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(OptionsDialog))
        Me.TableLayoutPanel1 = New TableLayoutPanel()
        Me.OK_Button = New Button()
        Me.Cancel_Button = New Button()
        Me.ItemNameComboBox = New NameColorComboBox()
        Me.UpdateForeground_Button = New Button()
        Me.FontDialog1 = New FontDialog()
        Me.KnownColorsComboBox1 = New KnownColorComboBox()
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New Point(303, 99)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        Me.TableLayoutPanel1.Size = New Size(170, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        Me.OK_Button.Anchor = AnchorStyles.None
        Me.OK_Button.Location = New Point(9, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New Size(66, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        ' 
        ' Cancel_Button
        ' 
        Me.Cancel_Button.Anchor = AnchorStyles.None
        Me.Cancel_Button.DialogResult = DialogResult.Cancel
        Me.Cancel_Button.Location = New Point(94, 5)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New Size(66, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        ' 
        ' ItemNameComboBox
        ' 
        Me.ItemNameComboBox.DrawMode = DrawMode.OwnerDrawFixed
        Me.ItemNameComboBox.DropDownHeight = 400
        Me.ItemNameComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        Me.ItemNameComboBox.DropDownWidth = 300
        Me.ItemNameComboBox.FormattingEnabled = True
        Me.ItemNameComboBox.IntegralHeight = False
        Me.ItemNameComboBox.Location = New Point(10, 56)
        Me.ItemNameComboBox.MaxDropDownItems = 20
        Me.ItemNameComboBox.Name = "ItemNameComboBox"
        Me.ItemNameComboBox.SelectedItem = CType(resources.GetObject("ItemNameComboBox.SelectedItem"), KeyValuePair(Of String, KnownColor))
        Me.ItemNameComboBox.Size = New Size(287, 24)
        Me.ItemNameComboBox.TabIndex = 3
        ' 
        ' UpdateForeground_Button
        ' 
        Me.UpdateForeground_Button.Location = New Point(314, 12)
        Me.UpdateForeground_Button.Name = "UpdateForeground_Button"
        Me.UpdateForeground_Button.Size = New Size(159, 27)
        Me.UpdateForeground_Button.TabIndex = 5
        Me.UpdateForeground_Button.Text = "Update Foreground Color"
        Me.UpdateForeground_Button.UseVisualStyleBackColor = True
        ' 
        ' KnownColorsComboBox1
        ' 
        Me.KnownColorsComboBox1.DrawMode = DrawMode.OwnerDrawFixed
        Me.KnownColorsComboBox1.DropDownHeight = 120
        Me.KnownColorsComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        Me.KnownColorsComboBox1.FormattingEnabled = True
        Me.KnownColorsComboBox1.IntegralHeight = False
        Me.KnownColorsComboBox1.Location = New Point(303, 56)
        Me.KnownColorsComboBox1.Name = "KnownColorsComboBox1"
        Me.KnownColorsComboBox1.SelectedItem = CType(resources.GetObject("KnownColorsComboBox1.SelectedItem"), KeyValuePair(Of String, KnownColor))
        Me.KnownColorsComboBox1.Size = New Size(169, 24)
        Me.KnownColorsComboBox1.TabIndex = 6
        ' 
        ' OptionsDialog
        ' 
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New Size(486, 143)
        Me.Controls.Add(Me.KnownColorsComboBox1)
        Me.Controls.Add(Me.UpdateForeground_Button)
        Me.Controls.Add(Me.ItemNameComboBox)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Color Editor"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button
    Friend WithEvents ItemNameComboBox As NameColorComboBox
    Friend WithEvents UpdateForeground_Button As Button
    Friend WithEvents OptionsBindingSource As BindingSource
    Friend WithEvents FontDialog1 As FontDialog
    Friend WithEvents KnownColorsComboBox1 As KnownColorComboBox
End Class
