' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutBox1
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

    Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents LabelProductName As System.Windows.Forms.Label
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents LabelCompanyName As System.Windows.Forms.Label
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents LabelCopyright As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(AboutBox1))
        Me.TableLayoutPanel = New TableLayoutPanel()
        Me.LogoPictureBox = New PictureBox()
        Me.LabelProductName = New Label()
        Me.LabelVersion = New Label()
        Me.LabelCopyright = New Label()
        Me.LabelCompanyName = New Label()
        Me.TextBoxDescription = New TextBox()
        Me.OKButton = New Button()
        Me.TableLayoutPanel.SuspendLayout()
        CType(Me.LogoPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' TableLayoutPanel
        ' 
        Me.TableLayoutPanel.AutoSize = True
        Me.TableLayoutPanel.ColumnCount = 2
        Me.TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33F))
        Me.TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 67F))
        Me.TableLayoutPanel.Controls.Add(Me.LogoPictureBox, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.LabelProductName, 1, 0)
        Me.TableLayoutPanel.Controls.Add(Me.LabelVersion, 1, 1)
        Me.TableLayoutPanel.Controls.Add(Me.LabelCopyright, 1, 2)
        Me.TableLayoutPanel.Controls.Add(Me.LabelCompanyName, 1, 3)
        Me.TableLayoutPanel.Controls.Add(Me.TextBoxDescription, 1, 4)
        Me.TableLayoutPanel.Controls.Add(Me.OKButton, 1, 5)
        Me.TableLayoutPanel.Dock = DockStyle.Fill
        Me.TableLayoutPanel.Location = New Point(10, 10)
        Me.TableLayoutPanel.Margin = New Padding(4, 3, 4, 3)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 6
        Me.TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 15F))
        Me.TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 5F))
        Me.TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        Me.TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        Me.TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        Me.TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10F))
        Me.TableLayoutPanel.Size = New Size(562, 311)
        Me.TableLayoutPanel.TabIndex = 0
        ' 
        ' LogoPictureBox
        ' 
        Me.LogoPictureBox.Anchor = AnchorStyles.None
        Me.LogoPictureBox.BackColor = Color.Transparent
        Me.LogoPictureBox.ErrorImage = Nothing
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), Image)
        Me.LogoPictureBox.Location = New Point(7, 31)
        Me.LogoPictureBox.Margin = New Padding(4, 3, 4, 3)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.TableLayoutPanel.SetRowSpan(Me.LogoPictureBox, 6)
        Me.LogoPictureBox.Size = New Size(171, 248)
        Me.LogoPictureBox.SizeMode = PictureBoxSizeMode.AutoSize
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        ' 
        ' LabelProductName
        ' 
        Me.LabelProductName.Dock = DockStyle.Fill
        Me.LabelProductName.Location = New Point(192, 0)
        Me.LabelProductName.Margin = New Padding(7, 0, 4, 0)
        Me.LabelProductName.MaximumSize = New Size(0, 20)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New Size(366, 20)
        Me.LabelProductName.TabIndex = 0
        Me.LabelProductName.Text = "Product Name"
        Me.LabelProductName.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LabelVersion
        ' 
        Me.LabelVersion.Dock = DockStyle.Fill
        Me.LabelVersion.Location = New Point(192, 46)
        Me.LabelVersion.Margin = New Padding(7, 0, 4, 0)
        Me.LabelVersion.MaximumSize = New Size(0, 20)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New Size(366, 15)
        Me.LabelVersion.TabIndex = 0
        Me.LabelVersion.Text = "Version"
        Me.LabelVersion.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LabelCopyright
        ' 
        Me.LabelCopyright.Dock = DockStyle.Fill
        Me.LabelCopyright.Location = New Point(192, 61)
        Me.LabelCopyright.Margin = New Padding(7, 0, 4, 0)
        Me.LabelCopyright.MaximumSize = New Size(0, 20)
        Me.LabelCopyright.Name = "LabelCopyright"
        Me.LabelCopyright.Size = New Size(366, 20)
        Me.LabelCopyright.TabIndex = 0
        Me.LabelCopyright.Text = "Copyright"
        Me.LabelCopyright.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LabelCompanyName
        ' 
        Me.LabelCompanyName.Dock = DockStyle.Fill
        Me.LabelCompanyName.Location = New Point(192, 92)
        Me.LabelCompanyName.Margin = New Padding(7, 0, 4, 0)
        Me.LabelCompanyName.MaximumSize = New Size(0, 20)
        Me.LabelCompanyName.Name = "LabelCompanyName"
        Me.LabelCompanyName.Size = New Size(366, 20)
        Me.LabelCompanyName.TabIndex = 0
        Me.LabelCompanyName.Text = "Company Name"
        Me.LabelCompanyName.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBoxDescription
        ' 
        Me.TextBoxDescription.Dock = DockStyle.Fill
        Me.TextBoxDescription.Location = New Point(192, 126)
        Me.TextBoxDescription.Margin = New Padding(7, 3, 4, 3)
        Me.TextBoxDescription.Multiline = True
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxDescription.ScrollBars = ScrollBars.Both
        Me.TextBoxDescription.Size = New Size(366, 149)
        Me.TextBoxDescription.TabIndex = 0
        Me.TextBoxDescription.TabStop = False
        Me.TextBoxDescription.Text = resources.GetString("TextBoxDescription.Text")
        ' 
        ' OKButton
        ' 
        Me.OKButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.OKButton.DialogResult = DialogResult.Cancel
        Me.OKButton.Location = New Point(470, 285)
        Me.OKButton.Margin = New Padding(4, 3, 4, 3)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New Size(88, 23)
        Me.OKButton.TabIndex = 0
        Me.OKButton.Text = "&OK"
        ' 
        ' AboutBox1
        ' 
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New Size(582, 331)
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutBox1"
        Me.Padding = New Padding(10)
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "About CareLink for Windows"
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.TableLayoutPanel.PerformLayout()
        CType(Me.LogoPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

End Class
