Imports Microsoft.VisualBasic
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.ItemNameComboBox = New System.Windows.Forms.ComboBox()
        Me.UpdateForeground_Button = New System.Windows.Forms.Button()
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.KnownColorsComboBox = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(303, 99)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(170, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(9, 5)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(66, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(94, 5)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(66, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'ItemNameComboBox
        '
        Me.ItemNameComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ItemNameComboBox.DropDownHeight = 400
        Me.ItemNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ItemNameComboBox.DropDownWidth = 300
        Me.ItemNameComboBox.FormattingEnabled = True
        Me.ItemNameComboBox.IntegralHeight = False
        Me.ItemNameComboBox.Location = New System.Drawing.Point(10, 56)
        Me.ItemNameComboBox.MaxDropDownItems = 20
        Me.ItemNameComboBox.Name = "ItemNameComboBox"
        Me.ItemNameComboBox.Size = New System.Drawing.Size(287, 24)
        Me.ItemNameComboBox.TabIndex = 3
        '
        'UpdateForeground_Button
        '
        Me.UpdateForeground_Button.Location = New System.Drawing.Point(314, 12)
        Me.UpdateForeground_Button.Name = "UpdateForeground_Button"
        Me.UpdateForeground_Button.Size = New System.Drawing.Size(159, 27)
        Me.UpdateForeground_Button.TabIndex = 5
        Me.UpdateForeground_Button.Text = "Update Foreground Color"
        Me.UpdateForeground_Button.UseVisualStyleBackColor = True
        '
        'KnownColorsComboBox
        '
        Me.KnownColorsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.KnownColorsComboBox.DropDownHeight = 120
        Me.KnownColorsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.KnownColorsComboBox.FormattingEnabled = True
        Me.KnownColorsComboBox.IntegralHeight = False
        Me.KnownColorsComboBox.Location = New System.Drawing.Point(303, 56)
        Me.KnownColorsComboBox.Name = "KnownColorsComboBox"
        Me.KnownColorsComboBox.Size = New System.Drawing.Size(169, 24)
        Me.KnownColorsComboBox.TabIndex = 6
        '
        'OptionsDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(486, 143)
        Me.Controls.Add(Me.KnownColorsComboBox)
        Me.Controls.Add(Me.UpdateForeground_Button)
        Me.Controls.Add(Me.ItemNameComboBox)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Color Editor"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button
    Friend WithEvents ItemNameComboBox As ComboBox
    Friend WithEvents UpdateForeground_Button As Button
    Friend WithEvents OptionsBindingSource As BindingSource
    Friend WithEvents FontDialog1 As FontDialog
    Friend WithEvents KnownColorsComboBox As ComboBox
End Class
