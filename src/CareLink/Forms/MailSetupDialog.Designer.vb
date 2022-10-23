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
        Me.ServerPasswordLabel = New System.Windows.Forms.Label()
        Me.MailServerUserEmailTextBox = New System.Windows.Forms.TextBox()
        Me.MailServerUserNameLabel = New System.Windows.Forms.Label()
        Me.MailServerPortLabel = New System.Windows.Forms.Label()
        Me.OutGoingMailServerLabel = New System.Windows.Forms.Label()
        Me.OutGoingMailServerComboBox = New System.Windows.Forms.ComboBox()
        Me.MailServerPortTextBox = New System.Windows.Forms.MaskedTextBox()
        Me.SendTestMessageCheckBox = New System.Windows.Forms.CheckBox()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Label5 = New System.Windows.Forms.Label()
        Me.AlertPhoneNumberMaskedTextBox = New System.Windows.Forms.MaskedTextBox()
        Me.ShowPasswordCheckBox = New System.Windows.Forms.CheckBox()
        Me.SMTPServerURLLabel = New System.Windows.Forms.Label()
        Me.SMTPServerURLTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CarrierDomainTextBox = New System.Windows.Forms.TextBox()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.Location = New System.Drawing.Point(193, 266)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(77, 27)
        Me.OK_Button.TabIndex = 14
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.Location = New System.Drawing.Point(278, 268)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(77, 27)
        Me.Cancel_Button.TabIndex = 15
        Me.Cancel_Button.Text = "Cancel"
        '
        'MailServerPasswordTextBox
        '
        Me.MailServerPasswordTextBox.Location = New System.Drawing.Point(144, 164)
        Me.MailServerPasswordTextBox.MaxLength = 20
        Me.MailServerPasswordTextBox.Name = "MailServerPasswordTextBox"
        Me.MailServerPasswordTextBox.Size = New System.Drawing.Size(150, 23)
        Me.MailServerPasswordTextBox.TabIndex = 9
        Me.MailServerPasswordTextBox.UseSystemPasswordChar = True
        '
        'ServerPasswordLabel
        '
        Me.ServerPasswordLabel.AutoSize = True
        Me.ServerPasswordLabel.Location = New System.Drawing.Point(18, 168)
        Me.ServerPasswordLabel.Name = "ServerPasswordLabel"
        Me.ServerPasswordLabel.Size = New System.Drawing.Size(121, 15)
        Me.ServerPasswordLabel.TabIndex = 8
        Me.ServerPasswordLabel.Text = "Mail Server Password:"
        '
        'MailServerUserEmailTextBox
        '
        Me.MailServerUserEmailTextBox.Location = New System.Drawing.Point(144, 130)
        Me.MailServerUserEmailTextBox.MaxLength = 100
        Me.MailServerUserEmailTextBox.Name = "MailServerUserEmailTextBox"
        Me.MailServerUserEmailTextBox.Size = New System.Drawing.Size(194, 23)
        Me.MailServerUserEmailTextBox.TabIndex = 7
        '
        'MailServerUserNameLabel
        '
        Me.MailServerUserNameLabel.AutoSize = True
        Me.MailServerUserNameLabel.Location = New System.Drawing.Point(18, 134)
        Me.MailServerUserNameLabel.Name = "MailServerUserNameLabel"
        Me.MailServerUserNameLabel.Size = New System.Drawing.Size(126, 15)
        Me.MailServerUserNameLabel.TabIndex = 6
        Me.MailServerUserNameLabel.Text = "Mail Server UserName:"
        '
        'MailServerPortLabel
        '
        Me.MailServerPortLabel.AutoSize = True
        Me.MailServerPortLabel.Location = New System.Drawing.Point(18, 100)
        Me.MailServerPortLabel.Name = "MailServerPortLabel"
        Me.MailServerPortLabel.Size = New System.Drawing.Size(93, 15)
        Me.MailServerPortLabel.TabIndex = 4
        Me.MailServerPortLabel.Text = "Mail Server Port:"
        '
        'OutGoingMailServerLabel
        '
        Me.OutGoingMailServerLabel.AutoSize = True
        Me.OutGoingMailServerLabel.Location = New System.Drawing.Point(18, 32)
        Me.OutGoingMailServerLabel.Name = "OutGoingMailServerLabel"
        Me.OutGoingMailServerLabel.Size = New System.Drawing.Size(122, 15)
        Me.OutGoingMailServerLabel.TabIndex = 0
        Me.OutGoingMailServerLabel.Text = "Outgoing Mail Server:"
        '
        'OutGoingMailServerComboBox
        '
        Me.OutGoingMailServerComboBox.FormattingEnabled = True
        Me.OutGoingMailServerComboBox.Location = New System.Drawing.Point(144, 28)
        Me.OutGoingMailServerComboBox.Name = "OutGoingMailServerComboBox"
        Me.OutGoingMailServerComboBox.Size = New System.Drawing.Size(194, 23)
        Me.OutGoingMailServerComboBox.TabIndex = 1
        '
        'MailServerPortTextBox
        '
        Me.MailServerPortTextBox.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        Me.MailServerPortTextBox.Location = New System.Drawing.Point(144, 96)
        Me.MailServerPortTextBox.Mask = "000"
        Me.MailServerPortTextBox.Name = "MailServerPortTextBox"
        Me.MailServerPortTextBox.Size = New System.Drawing.Size(30, 23)
        Me.MailServerPortTextBox.TabIndex = 5
        Me.MailServerPortTextBox.ValidatingType = GetType(Integer)
        '
        'SendTestMessageCheckBox
        '
        Me.SendTestMessageCheckBox.AutoSize = True
        Me.SendTestMessageCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SendTestMessageCheckBox.Checked = True
        Me.SendTestMessageCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SendTestMessageCheckBox.Location = New System.Drawing.Point(18, 236)
        Me.SendTestMessageCheckBox.Name = "SendTestMessageCheckBox"
        Me.SendTestMessageCheckBox.Size = New System.Drawing.Size(124, 19)
        Me.SendTestMessageCheckBox.TabIndex = 13
        Me.SendTestMessageCheckBox.Text = "Send Test Message"
        Me.SendTestMessageCheckBox.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'LabelLowGlucoseSuspended
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 202)
        Me.Label5.Name = "LabelLowGlucoseSuspended"
        Me.Label5.Size = New System.Drawing.Size(105, 15)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Alert Phone Digits:"
        '
        'AlertPhoneNumberMaskedTextBox
        '
        Me.AlertPhoneNumberMaskedTextBox.Location = New System.Drawing.Point(144, 198)
        Me.AlertPhoneNumberMaskedTextBox.Mask = "(999) 000-0000"
        Me.AlertPhoneNumberMaskedTextBox.Name = "AlertPhoneNumberMaskedTextBox"
        Me.AlertPhoneNumberMaskedTextBox.Size = New System.Drawing.Size(84, 23)
        Me.AlertPhoneNumberMaskedTextBox.TabIndex = 12
        Me.AlertPhoneNumberMaskedTextBox.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'ShowPasswordCheckBox
        '
        Me.ShowPasswordCheckBox.AutoSize = True
        Me.ShowPasswordCheckBox.Location = New System.Drawing.Point(300, 166)
        Me.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox"
        Me.ShowPasswordCheckBox.Size = New System.Drawing.Size(55, 19)
        Me.ShowPasswordCheckBox.TabIndex = 10
        Me.ShowPasswordCheckBox.Text = "Show"
        Me.ShowPasswordCheckBox.UseVisualStyleBackColor = True
        '
        'SMTPServerURLLabel
        '
        Me.SMTPServerURLLabel.AutoSize = True
        Me.SMTPServerURLLabel.Location = New System.Drawing.Point(18, 66)
        Me.SMTPServerURLLabel.Name = "SMTPServerURLLabel"
        Me.SMTPServerURLLabel.Size = New System.Drawing.Size(99, 15)
        Me.SMTPServerURLLabel.TabIndex = 2
        Me.SMTPServerURLLabel.Text = "SMTP Server URL:"
        '
        'SMTPServerURLTextBox
        '
        Me.SMTPServerURLTextBox.Location = New System.Drawing.Point(145, 62)
        Me.SMTPServerURLTextBox.Name = "SMTPServerURLTextBox"
        Me.SMTPServerURLTextBox.ReadOnly = True
        Me.SMTPServerURLTextBox.Size = New System.Drawing.Size(193, 23)
        Me.SMTPServerURLTextBox.TabIndex = 3
        '
        'LabelAutoModeStatus
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(229, 202)
        Me.Label1.Name = "LabelAutoModeStatus"
        Me.Label1.Size = New System.Drawing.Size(18, 15)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "@"
        '
        'CarrierDomainTextBox
        '
        Me.CarrierDomainTextBox.Location = New System.Drawing.Point(248, 198)
        Me.CarrierDomainTextBox.Name = "CarrierDomainTextBox"
        Me.CarrierDomainTextBox.Size = New System.Drawing.Size(100, 23)
        Me.CarrierDomainTextBox.TabIndex = 17
        Me.CarrierDomainTextBox.Text = "txt.att.net"
        '
        'MailSetupDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(377, 316)
        Me.Controls.Add(Me.CarrierDomainTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SMTPServerURLTextBox)
        Me.Controls.Add(Me.SMTPServerURLLabel)
        Me.Controls.Add(Me.ShowPasswordCheckBox)
        Me.Controls.Add(Me.AlertPhoneNumberMaskedTextBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.SendTestMessageCheckBox)
        Me.Controls.Add(Me.MailServerPortTextBox)
        Me.Controls.Add(Me.OutGoingMailServerComboBox)
        Me.Controls.Add(Me.MailServerPasswordTextBox)
        Me.Controls.Add(Me.ServerPasswordLabel)
        Me.Controls.Add(Me.MailServerUserEmailTextBox)
        Me.Controls.Add(Me.MailServerUserNameLabel)
        Me.Controls.Add(Me.MailServerPortLabel)
        Me.Controls.Add(Me.OutGoingMailServerLabel)
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
    Friend WithEvents ServerPasswordLabel As Label
    Friend WithEvents MailServerUserNameLabel As Label
    Friend WithEvents MailServerPortLabel As Label
    Friend WithEvents OutGoingMailServerLabel As Label
    Friend WithEvents OutGoingMailServerComboBox As ComboBox
    Friend WithEvents MailServerPortTextBox As MaskedTextBox
    Friend WithEvents SendTestMessageCheckBox As CheckBox
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents AlertPhoneNumberMaskedTextBox As MaskedTextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ShowPasswordCheckBox As CheckBox
    Friend WithEvents MailServerUserEmailTextBox As TextBox
    Friend WithEvents SMTPServerURLTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SMTPServerURLLabel As Label
    Friend WithEvents CarrierDomainTextBox As TextBox
    Friend WithEvents Label1 As Label
End Class
