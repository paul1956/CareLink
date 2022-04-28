' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MailSetupDialog
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MailSetupDialog))
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.MailServerPasswordTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.MailServerUserEmailTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MailServerPortLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OutGoingMailServerComboBox = New System.Windows.Forms.ComboBox()
        Me.MailServerPortTextBox = New System.Windows.Forms.MaskedTextBox()
        Me.SendTestMessageCheckBox = New System.Windows.Forms.CheckBox()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Label5 = New System.Windows.Forms.Label()
        Me.AlertPhoneNumberMaskedTextBox = New System.Windows.Forms.MaskedTextBox()
        Me.ShowPasswordCheckBox = New System.Windows.Forms.CheckBox()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.Location = New System.Drawing.Point(193, 233)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(77, 27)
        Me.OK_Button.TabIndex = 12
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.Location = New System.Drawing.Point(278, 235)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(77, 27)
        Me.Cancel_Button.TabIndex = 13
        Me.Cancel_Button.Text = "Cancel"
        '
        'MailServerPasswordTextBox
        '
        Me.MailServerPasswordTextBox.Location = New System.Drawing.Point(161, 136)
        Me.MailServerPasswordTextBox.MaxLength = 32
        Me.MailServerPasswordTextBox.Name = "MailServerPasswordTextBox"
        Me.MailServerPasswordTextBox.Size = New System.Drawing.Size(127, 23)
        Me.MailServerPasswordTextBox.TabIndex = 7
        Me.MailServerPasswordTextBox.UseSystemPasswordChar = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 140)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(121, 15)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Mail Server Password:"
        '
        'MailServerUserEmailTextBox
        '
        Me.MailServerUserEmailTextBox.Location = New System.Drawing.Point(161, 100)
        Me.MailServerUserEmailTextBox.MaxLength = 100
        Me.MailServerUserEmailTextBox.Name = "MailServerUserEmailTextBox"
        Me.MailServerUserEmailTextBox.Size = New System.Drawing.Size(194, 23)
        Me.MailServerUserEmailTextBox.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(126, 15)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Mail Server User Email:"
        '
        'MailServerPortLabel
        '
        Me.MailServerPortLabel.AutoSize = True
        Me.MailServerPortLabel.Location = New System.Drawing.Point(18, 68)
        Me.MailServerPortLabel.Name = "MailServerPortLabel"
        Me.MailServerPortLabel.Size = New System.Drawing.Size(93, 15)
        Me.MailServerPortLabel.TabIndex = 2
        Me.MailServerPortLabel.Text = "Mail Server Port:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Outgoing Mail Server:"
        '
        'OutGoingMailServerComboBox
        '
        Me.OutGoingMailServerComboBox.FormattingEnabled = True
        Me.OutGoingMailServerComboBox.Location = New System.Drawing.Point(161, 28)
        Me.OutGoingMailServerComboBox.Name = "OutGoingMailServerComboBox"
        Me.OutGoingMailServerComboBox.Size = New System.Drawing.Size(194, 23)
        Me.OutGoingMailServerComboBox.TabIndex = 1
        '
        'MailServerPortTextBox
        '
        Me.MailServerPortTextBox.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        Me.MailServerPortTextBox.Location = New System.Drawing.Point(161, 64)
        Me.MailServerPortTextBox.Mask = "000"
        Me.MailServerPortTextBox.Name = "MailServerPortTextBox"
        Me.MailServerPortTextBox.Size = New System.Drawing.Size(30, 23)
        Me.MailServerPortTextBox.TabIndex = 3
        Me.MailServerPortTextBox.ValidatingType = GetType(Integer)
        '
        'SendTestMessageCheckBox
        '
        Me.SendTestMessageCheckBox.AutoSize = True
        Me.SendTestMessageCheckBox.Location = New System.Drawing.Point(23, 212)
        Me.SendTestMessageCheckBox.Name = "SendTestMessageCheckBox"
        Me.SendTestMessageCheckBox.Size = New System.Drawing.Size(124, 19)
        Me.SendTestMessageCheckBox.TabIndex = 11
        Me.SendTestMessageCheckBox.Text = "SendUsingExchange Test Message"
        Me.SendTestMessageCheckBox.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 176)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(136, 15)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Alert Phone, Digits Only:"
        '
        'AlertPhoneNumberMaskedTextBox
        '
        Me.AlertPhoneNumberMaskedTextBox.Location = New System.Drawing.Point(161, 172)
        Me.AlertPhoneNumberMaskedTextBox.Mask = "(999) 000-0000"
        Me.AlertPhoneNumberMaskedTextBox.Name = "AlertPhoneNumberMaskedTextBox"
        Me.AlertPhoneNumberMaskedTextBox.Size = New System.Drawing.Size(84, 23)
        Me.AlertPhoneNumberMaskedTextBox.TabIndex = 10
        Me.AlertPhoneNumberMaskedTextBox.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'ShowPasswordCheckBox
        '
        Me.ShowPasswordCheckBox.AutoSize = True
        Me.ShowPasswordCheckBox.Location = New System.Drawing.Point(300, 138)
        Me.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox"
        Me.ShowPasswordCheckBox.Size = New System.Drawing.Size(55, 19)
        Me.ShowPasswordCheckBox.TabIndex = 8
        Me.ShowPasswordCheckBox.Text = "Show"
        Me.ShowPasswordCheckBox.UseVisualStyleBackColor = True
        '
        'MailSetupDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(377, 283)
        Me.Controls.Add(Me.ShowPasswordCheckBox)
        Me.Controls.Add(Me.AlertPhoneNumberMaskedTextBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.SendTestMessageCheckBox)
        Me.Controls.Add(Me.MailServerPortTextBox)
        Me.Controls.Add(Me.OutGoingMailServerComboBox)
        Me.Controls.Add(Me.MailServerPasswordTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.MailServerUserEmailTextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MailServerPortLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Cancel_Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MailSetupDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Mail Server Setup"
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents MailServerPasswordTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents MailServerPortLabel As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents OutGoingMailServerComboBox As ComboBox
    Friend WithEvents MailServerPortTextBox As MaskedTextBox
    Friend WithEvents SendTestMessageCheckBox As CheckBox
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents AlertPhoneNumberMaskedTextBox As MaskedTextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ShowPasswordCheckBox As CheckBox
    Friend WithEvents MailServerUserEmailTextBox As TextBox
End Class
