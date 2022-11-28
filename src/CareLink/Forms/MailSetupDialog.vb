' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports System.Text.RegularExpressions

Public Class MailSetupDialog

    Private Const InvalidNameCharacters As String = "\/:*?""<>| "

    <StringSyntax(StringSyntaxAttribute.Regex)>
    Private Const SpacePattern As String = "\s+"

    Private ReadOnly _defaultPorts As New Dictionary(Of String, Integer) From {
                    {"Microsoft Exchange", 0},
                    {"smtp.comcast.net", 587},
                    {"smtp.gmail.com", 587},
                    {"smtpout.secureserver.net", 587},
                    {"smtp.mail.yahoo.com", 587}
                }

    Private ReadOnly _servers As New Dictionary(Of String, String) From {
                    {"Microsoft Exchange", ""},
                    {"Comcast/Xfinity", "smtp.comcast.net"},
                    {"Gmail", "smtp.gmail.com"},
                    {"GoDaddy", "smtpout.secureserver.net"},
                    {"Yahoo", "smtp.mail.yahoo.com"}
                }

    Private _useExchange As Boolean = True

    Private Shared Function IsValidEmailAddress(MailServerUserName As String, ByRef errorMsg As String) As Boolean
        If String.IsNullOrWhiteSpace(MailServerUserName) Then
            errorMsg = "Required"
            Return False
        End If

        Try
            Dim tempVar As New Net.Mail.MailAddress(MailServerUserName)
        Catch e1 As ArgumentException
            errorMsg = "Required"
            Return False
        Catch e2 As FormatException
            'textBox contains no valid mail address
            errorMsg = e2.Message
            Return False
        End Try
        Return True
    End Function

    Private Shared Sub validateDomainLKeyPress(sender As TextBox, ByRef e As KeyPressEventArgs)
        Dim keyValue As Char = e.KeyChar

        e.Handled = True
        If InvalidNameCharacters.Contains(e.KeyChar) OrElse (sender.SelectionStart = 0 AndAlso e.KeyChar = ".") Then
            Return
        End If
        ' Allow nothing else
        e.Handled = False
    End Sub

    Private Sub AlertPhoneNumberMaskedTextBox_Validated(sender As Object, e As EventArgs) Handles AlertPhoneNumberMaskedTextBox.Validated
        ' If all conditions have been met, clear the error provider of errors.
        Me.ErrorProvider1.SetError(Me.AlertPhoneNumberMaskedTextBox, "")

    End Sub

    Private Sub AlertPhoneNumberMaskedTextBox_Validating(sender As Object, e As CancelEventArgs) Handles AlertPhoneNumberMaskedTextBox.Validating
        If Me.AlertPhoneNumberMaskedTextBox.Text.Length <> 10 Then
            ' Set the ErrorProvider error with the text to display.
            Me.ErrorProvider1.SetError(Me.AlertPhoneNumberMaskedTextBox, "Invalid telephone number!")
        End If
    End Sub

    Private Function AnyControlErrors() As Boolean
        If Me.ErrorProvider1.GetError(Me.MailServerPortTextBox).Length > 0 Then
            Me.MailServerPortTextBox.SelectionStart = 0
            Me.MailServerPortTextBox.Focus()
            Return True
        End If
        If Me.ErrorProvider1.GetError(Me.MailServerUserEmailTextBox).Length > 0 Then
            Me.MailServerUserEmailTextBox.SelectionStart = 0
            Me.MailServerUserEmailTextBox.Focus()
            Return True
        End If
        If Me.ErrorProvider1.GetError(Me.MailServerPasswordTextBox).Length > 0 Then
            Me.MailServerPasswordTextBox.SelectionStart = 0
            Me.MailServerPasswordTextBox.Focus()
            Return True
        End If
        If Me.ErrorProvider1.GetError(Me.AlertPhoneNumberMaskedTextBox).Length > 0 Then
            Me.AlertPhoneNumberMaskedTextBox.SelectionStart = 0
            Me.AlertPhoneNumberMaskedTextBox.Focus()
            Return True
        End If
        Return False

    End Function

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CarrierDomainTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CarrierDomainTextBox.KeyPress
        validateDomainLKeyPress(CType(sender, TextBox), e)
    End Sub

    Private Sub MailServerPasswordTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MailServerPasswordTextBox.KeyPress
        ' we don't accept whitespace characters
        If Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub MailServerPasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles MailServerPasswordTextBox.TextChanged
        ' We remove white spaces from text inserted
        TryCast(sender, TextBox).Text = Regex.Replace(TryCast(sender, TextBox).Text, SpacePattern, "")

    End Sub

    Private Sub MailServerPasswordTextBox_Validated(sender As Object, e As EventArgs) Handles MailServerPasswordTextBox.Validated
        ' If all conditions have been met, clear the error provider of errors.
        Me.ErrorProvider1.SetError(Me.MailServerPasswordTextBox, "")

    End Sub

    Private Sub MailServerPasswordTextBox_Validating(sender As Object, e As CancelEventArgs) Handles MailServerPasswordTextBox.Validating
        If Me.MailServerPasswordTextBox.Text.Length < 6 Then
            ' Set the ErrorProvider error with the text to display.
            Me.ErrorProvider1.SetError(Me.MailServerPasswordTextBox, "userPassword must be at least 6 characters")
        End If

    End Sub

    Private Sub MailServerPortTextBox_Validated(sender As Object, e As EventArgs) Handles MailServerPortTextBox.Validated
        ' If all conditions have been met, clear the error provider of errors.
        Me.ErrorProvider1.SetError(Me.MailServerPortTextBox, "")

    End Sub

    Private Sub MailServerPortTextBox_Validating(sender As Object, e As CancelEventArgs) Handles MailServerPortTextBox.Validating
        If Me.MailServerPortTextBox.Text.Length = 0 Then
            ' Set the ErrorProvider error with the text to display.
            Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, "Server Port Required")
            Exit Sub
        End If
        Dim port As Integer = 0
        If Not Integer.TryParse(Me.MailServerPortTextBox.Text, port) Then
            ' Set the ErrorProvider error with the text to display.
            Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, "Server Port can't be 0")
        End If
    End Sub

    Private Sub MailServerUserEmailTextBox_Validated(sender As Object, e As EventArgs) Handles MailServerUserEmailTextBox.Validated
        ' If all conditions have been met, clear the error provider of errors.
        Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, "")
    End Sub

    Private Sub MailServerUserEmailTextBox_Validating(sender As Object, e As CancelEventArgs) Handles MailServerUserEmailTextBox.Validating
        Dim errorMsg As String = ""
        If Not IsValidEmailAddress(Me.MailServerUserEmailTextBox.Text, errorMsg) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            Me.MailServerUserEmailTextBox.Select(0, Me.MailServerUserEmailTextBox.Text.Length)

            ' Set the ErrorProvider error with the text to display.
            Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, errorMsg)
        End If
    End Sub

    Private Sub MailServerUserNameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MailServerUserEmailTextBox.KeyPress
        validateDomainLKeyPress(CType(sender, TextBox), e)
    End Sub

    Private Sub MailSetupDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.OutGoingMailServerComboBox.DataSource = New BindingSource(_servers, Nothing)
        Me.OutGoingMailServerComboBox.DisplayMember = "Key"
        Me.OutGoingMailServerComboBox.ValueMember = "Value"
        Dim outGoingMailServer As String = My.Settings.OutGoingMailServer

        If Not String.IsNullOrWhiteSpace(outGoingMailServer) Then
            Me.OutGoingMailServerComboBox.SelectedValue = outGoingMailServer
            If outGoingMailServer = "smtp.gmail.com" Then
                Me.ServerPasswordLabel.Text = "Google App Password"
            Else
                Me.ServerPasswordLabel.Text = "Mail Server Password:"
            End If
        Else
            _useExchange = True
        End If

        If Not _useExchange Then
            If My.Settings.MailServerPort = 0 Then
                Me.MailServerPortTextBox.Text = _defaultPorts(outGoingMailServer).ToString
            Else
                Me.MailServerPortTextBox.Text = My.Settings.MailServerPort.ToString
            End If
        End If

        Dim errorMsg As String = ""

        If IsValidEmailAddress(My.Settings.MailServerUserName, errorMsg) Then
            Me.MailServerUserEmailTextBox.Text = My.Settings.MailServerUserName
        Else
            Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, errorMsg)
        End If

        If String.IsNullOrWhiteSpace(My.Settings.MailServerPassword) Then
            Me.ErrorProvider1.SetError(Me.MailServerPasswordTextBox, "Required")
        Else
            Me.MailServerPasswordTextBox.Text = My.Settings.MailServerPassword
        End If

        Dim alertPhoneNumber As String = My.Settings.AlertPhoneNumber
        If String.IsNullOrWhiteSpace(alertPhoneNumber) Then
            Me.ErrorProvider1.SetError(Me.AlertPhoneNumberMaskedTextBox, "Required")
        Else
            Me.AlertPhoneNumberMaskedTextBox.Text = $"({alertPhoneNumber.Substring(0, 3)}) {alertPhoneNumber.Substring(3, 3)}-{alertPhoneNumber.Substring(6, 4)}"
            Me.AlertPhoneNumberMaskedTextBox.ValidateText()
            Me.ErrorProvider1.SetError(Me.AlertPhoneNumberMaskedTextBox, "")

        End If
        If String.IsNullOrWhiteSpace(My.Settings.CarrierTextingDomain) Then
            Me.ErrorProvider1.SetError(Me.CarrierDomainTextBox, "Required")
        Else
            Me.CarrierDomainTextBox.Text = My.Settings.CarrierTextingDomain
            Me.ErrorProvider1.SetError(Me.CarrierDomainTextBox, "")
        End If
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Me.AnyControlErrors Then
            Exit Sub
        End If
        Try
            If Me.SendTestMessageCheckBox.Checked Then
                If _useExchange Then
                    Me.Cursor = Cursors.WaitCursor
                    Dim mailTestClient As New SendMail(True,
                                                       Me.MailServerUserEmailTextBox.Text,
                                                       Me.MailServerPasswordTextBox.Text)
                    mailTestClient.SendUsingExchange($"{Me.AlertPhoneNumberMaskedTextBox.Text}@{Me.CarrierDomainTextBox.Text}",
                                                    $"{s_lastSgRecord.sg} {BgUnitsString}")
                    Me.Cursor = Cursors.Default
                Else
                    Dim port As Integer
                    If Integer.TryParse(Me.MailServerPortTextBox.Text, port) Then
                        Dim mailTestClient As New SendMail(False,
                                                           Me.MailServerUserEmailTextBox.Text,
                                                           Me.MailServerPasswordTextBox.Text,
                                                           Me.OutGoingMailServerComboBox.SelectedValue?.ToString(),
                                                           port)
                        mailTestClient.Send($"{Me.AlertPhoneNumberMaskedTextBox.Text}@{Me.CarrierDomainTextBox.Text}",
                                            Me.MailServerUserEmailTextBox.Text,
                                            $"{s_lastSgRecord.sg} {BgUnitsString}")
                        My.Settings.MailServerPort = port
                    Else
                        Me.DialogResult = DialogResult.Cancel
                        Exit Sub
                    End If
                End If
                My.Settings.OutGoingMailServer = Me.OutGoingMailServerComboBox.SelectedValue.ToString
                My.Settings.MailServerUserName = Me.MailServerUserEmailTextBox.Text
                My.Settings.MailServerPassword = Me.MailServerPasswordTextBox.Text
                My.Settings.AlertPhoneNumber = Me.AlertPhoneNumberMaskedTextBox.Text
                My.Settings.CarrierTextingDomain = Me.CarrierDomainTextBox.Text
                My.Settings.Save()
            End If
            Me.DialogResult = DialogResult.OK
        Catch ex As Exception
            If MsgBox($"Mail send error {ex.Message}, retry?",
                      MsgBoxStyle.OkCancel,
                      "Server Validation Error") = MsgBoxResult.Cancel Then
                Me.DialogResult = DialogResult.Cancel
            Else
                Exit Sub
            End If
        End Try

        Me.Close()
    End Sub

    Private Sub OutGoingMailServerComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles OutGoingMailServerComboBox.SelectedIndexChanged
        Dim outgoingServer As String = Me.OutGoingMailServerComboBox.SelectedValue.ToString
        If outgoingServer.Contains(","c) Then Exit Sub
        Me.SMTPServerURLTextBox.Text = outgoingServer
        _useExchange = String.IsNullOrWhiteSpace(outgoingServer)

        If _useExchange Then
            Me.MailServerPortLabel.Visible = False
            Me.MailServerPortTextBox.Visible = False
            Me.SMTPServerURLLabel.Visible = False
            Me.SMTPServerURLTextBox.Visible = False
        Else
            Me.MailServerPortLabel.Visible = True
            Me.MailServerPortTextBox.Text = _defaultPorts(outgoingServer).ToString
            Me.MailServerPortTextBox.Visible = True
            Me.SMTPServerURLLabel.Visible = True
            Me.SMTPServerURLTextBox.Visible = True
        End If
    End Sub

    Private Sub ShowPasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPasswordCheckBox.CheckedChanged
        Me.MailServerPasswordTextBox.UseSystemPasswordChar = Not Me.ShowPasswordCheckBox.Checked
    End Sub

End Class
