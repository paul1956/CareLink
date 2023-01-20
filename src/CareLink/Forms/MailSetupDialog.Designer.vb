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
        components = New ComponentModel.Container()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(MailSetupDialog))
        Me.OK_Button = New Button()
        Me.Cancel_Button = New Button()
        Me.MailServerPasswordTextBox = New TextBox()
        Me.ServerPasswordLabel = New Label()
        Me.MailServerUserEmailTextBox = New TextBox()
        Me.MailServerUserNameLabel = New Label()
        Me.MailServerPortLabel = New Label()
        Me.OutGoingMailServerLabel = New Label()
        Me.OutGoingMailServerComboBox = New ComboBox()
        Me.MailServerPortTextBox = New MaskedTextBox()
        Me.SendTestMessageCheckBox = New CheckBox()
        Me.ErrorProvider1 = New ErrorProvider(components)
        Me.AlertPhoneLabel = New Label()
        Me.AlertPhoneNumberMaskedTextBox = New MaskedTextBox()
        Me.ShowPasswordCheckBox = New CheckBox()
        Me.SMTPServerURLLabel = New Label()
        Me.SMTPServerURLTextBox = New TextBox()
        Me.AmpersandLabel = New Label()
        Me.CarrierDomainTextBox = New TextBox()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' OK_Button
        ' 
        Me.OK_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.OK_Button.Location = New Point(193, 266)
        Me.OK_Button.Margin = New Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New Size(77, 27)
        Me.OK_Button.TabIndex = 14
        Me.OK_Button.Text = "OK" ' 
        ' Cancel_Button
        ' 
        Me.Cancel_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.Cancel_Button.Location = New Point(278, 268)
        Me.Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New Size(77, 27)
        Me.Cancel_Button.TabIndex = 15
        Me.Cancel_Button.Text = "Cancel" ' 
        ' MailServerPasswordTextBox
        ' 
        Me.MailServerPasswordTextBox.Location = New Point(144, 164)
        Me.MailServerPasswordTextBox.MaxLength = 20
        Me.MailServerPasswordTextBox.Name = "MailServerPasswordTextBox"
        Me.MailServerPasswordTextBox.Size = New Size(150, 23)
        Me.MailServerPasswordTextBox.TabIndex = 9
        Me.MailServerPasswordTextBox.UseSystemPasswordChar = True
        ' 
        ' ServerPasswordLabel
        ' 
        Me.ServerPasswordLabel.AutoSize = True
        Me.ServerPasswordLabel.Location = New Point(18, 168)
        Me.ServerPasswordLabel.Name = "ServerPasswordLabel"
        Me.ServerPasswordLabel.Size = New Size(121, 15)
        Me.ServerPasswordLabel.TabIndex = 8
        Me.ServerPasswordLabel.Text = "Mail Server Password:" ' 
        ' MailServerUserEmailTextBox
        ' 
        Me.MailServerUserEmailTextBox.Location = New Point(144, 130)
        Me.MailServerUserEmailTextBox.MaxLength = 100
        Me.MailServerUserEmailTextBox.Name = "MailServerUserEmailTextBox"
        Me.MailServerUserEmailTextBox.Size = New Size(194, 23)
        Me.MailServerUserEmailTextBox.TabIndex = 7
        ' 
        ' MailServerUserNameLabel
        ' 
        Me.MailServerUserNameLabel.AutoSize = True
        Me.MailServerUserNameLabel.Location = New Point(18, 134)
        Me.MailServerUserNameLabel.Name = "MailServerUserNameLabel"
        Me.MailServerUserNameLabel.Size = New Size(129, 15)
        Me.MailServerUserNameLabel.TabIndex = 6
        Me.MailServerUserNameLabel.Text = "Mail Server User Name:" ' 
        ' MailServerPortLabel
        ' 
        Me.MailServerPortLabel.AutoSize = True
        Me.MailServerPortLabel.Location = New Point(18, 100)
        Me.MailServerPortLabel.Name = "MailServerPortLabel"
        Me.MailServerPortLabel.Size = New Size(93, 15)
        Me.MailServerPortLabel.TabIndex = 4
        Me.MailServerPortLabel.Text = "Mail Server Port:" ' 
        ' OutGoingMailServerLabel
        ' 
        Me.OutGoingMailServerLabel.AutoSize = True
        Me.OutGoingMailServerLabel.Location = New Point(18, 32)
        Me.OutGoingMailServerLabel.Name = "OutGoingMailServerLabel"
        Me.OutGoingMailServerLabel.Size = New Size(122, 15)
        Me.OutGoingMailServerLabel.TabIndex = 0
        Me.OutGoingMailServerLabel.Text = "Outgoing Mail Server:" ' 
        ' OutGoingMailServerComboBox
        ' 
        Me.OutGoingMailServerComboBox.FormattingEnabled = True
        Me.OutGoingMailServerComboBox.Location = New Point(144, 28)
        Me.OutGoingMailServerComboBox.Name = "OutGoingMailServerComboBox"
        Me.OutGoingMailServerComboBox.Size = New Size(194, 23)
        Me.OutGoingMailServerComboBox.TabIndex = 1
        ' 
        ' MailServerPortTextBox
        ' 
        Me.MailServerPortTextBox.CutCopyMaskFormat = MaskFormat.ExcludePromptAndLiterals
        Me.MailServerPortTextBox.Location = New Point(144, 96)
        Me.MailServerPortTextBox.Mask = "000"
        Me.MailServerPortTextBox.Name = "MailServerPortTextBox"
        Me.MailServerPortTextBox.Size = New Size(30, 23)
        Me.MailServerPortTextBox.TabIndex = 5
        Me.MailServerPortTextBox.ValidatingType = GetType(Integer)
        ' 
        ' SendTestMessageCheckBox
        ' 
        Me.SendTestMessageCheckBox.AutoSize = True
        Me.SendTestMessageCheckBox.CheckAlign = ContentAlignment.MiddleRight
        Me.SendTestMessageCheckBox.Checked = True
        Me.SendTestMessageCheckBox.CheckState = CheckState.Checked
        Me.SendTestMessageCheckBox.Location = New Point(18, 236)
        Me.SendTestMessageCheckBox.Name = "SendTestMessageCheckBox"
        Me.SendTestMessageCheckBox.Size = New Size(124, 19)
        Me.SendTestMessageCheckBox.TabIndex = 13
        Me.SendTestMessageCheckBox.Text = "Send Test Message"
        Me.SendTestMessageCheckBox.UseVisualStyleBackColor = True
        ' 
        ' ErrorProvider1
        ' 
        Me.ErrorProvider1.ContainerControl = Me
        ' 
        ' AlertPhoneLabel
        ' 
        Me.AlertPhoneLabel.AutoSize = True
        Me.AlertPhoneLabel.Location = New Point(18, 202)
        Me.AlertPhoneLabel.Name = "AlertPhoneLabel"
        Me.AlertPhoneLabel.Size = New Size(105, 15)
        Me.AlertPhoneLabel.TabIndex = 11
        Me.AlertPhoneLabel.Text = "Alert Phone Digits:" ' 
        ' AlertPhoneNumberMaskedTextBox
        ' 
        Me.AlertPhoneNumberMaskedTextBox.Location = New Point(144, 198)
        Me.AlertPhoneNumberMaskedTextBox.Mask = "(999) 000-0000"
        Me.AlertPhoneNumberMaskedTextBox.Name = "AlertPhoneNumberMaskedTextBox"
        Me.AlertPhoneNumberMaskedTextBox.Size = New Size(84, 23)
        Me.AlertPhoneNumberMaskedTextBox.TabIndex = 12
        Me.AlertPhoneNumberMaskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals
        ' 
        ' ShowPasswordCheckBox
        ' 
        Me.ShowPasswordCheckBox.AutoSize = True
        Me.ShowPasswordCheckBox.Location = New Point(300, 166)
        Me.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox"
        Me.ShowPasswordCheckBox.Size = New Size(55, 19)
        Me.ShowPasswordCheckBox.TabIndex = 10
        Me.ShowPasswordCheckBox.Text = "Show"
        Me.ShowPasswordCheckBox.UseVisualStyleBackColor = True
        ' 
        ' SMTPServerURLLabel
        ' 
        Me.SMTPServerURLLabel.AutoSize = True
        Me.SMTPServerURLLabel.Location = New Point(18, 66)
        Me.SMTPServerURLLabel.Name = "SMTPServerURLLabel"
        Me.SMTPServerURLLabel.Size = New Size(99, 15)
        Me.SMTPServerURLLabel.TabIndex = 2
        Me.SMTPServerURLLabel.Text = "SMTP Server URL:" ' 
        ' SMTPServerURLTextBox
        ' 
        Me.SMTPServerURLTextBox.Location = New Point(145, 62)
        Me.SMTPServerURLTextBox.Name = "SMTPServerURLTextBox"
        Me.SMTPServerURLTextBox.ReadOnly = True
        Me.SMTPServerURLTextBox.Size = New Size(193, 23)
        Me.SMTPServerURLTextBox.TabIndex = 3
        ' 
        ' AmpersandLabel
        ' 
        Me.AmpersandLabel.AutoSize = True
        Me.AmpersandLabel.Location = New Point(229, 202)
        Me.AmpersandLabel.Name = "AmpersandLabel"
        Me.AmpersandLabel.Size = New Size(18, 15)
        Me.AmpersandLabel.TabIndex = 16
        Me.AmpersandLabel.Text = "@" ' 
        ' CarrierDomainTextBox
        ' 
        Me.CarrierDomainTextBox.Location = New Point(248, 198)
        Me.CarrierDomainTextBox.Name = "CarrierDomainTextBox"
        Me.CarrierDomainTextBox.Size = New Size(100, 23)
        Me.CarrierDomainTextBox.TabIndex = 17
        Me.CarrierDomainTextBox.Text = "txt.att.net" ' 
        ' MailSetupDialog
        ' 
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New Size(377, 316)
        Me.Controls.Add(Me.CarrierDomainTextBox)
        Me.Controls.Add(Me.AmpersandLabel)
        Me.Controls.Add(Me.SMTPServerURLTextBox)
        Me.Controls.Add(Me.SMTPServerURLLabel)
        Me.Controls.Add(Me.ShowPasswordCheckBox)
        Me.Controls.Add(Me.AlertPhoneNumberMaskedTextBox)
        Me.Controls.Add(Me.AlertPhoneLabel)
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
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MailSetupDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Mail Server Setup"
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents AlertPhoneLabel As Label
    Friend WithEvents ShowPasswordCheckBox As CheckBox
    Friend WithEvents MailServerUserEmailTextBox As TextBox
    Friend WithEvents SMTPServerURLTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SMTPServerURLLabel As Label
    Friend WithEvents CarrierDomainTextBox As TextBox
    Friend WithEvents AmpersandLabel As Label
End Class
