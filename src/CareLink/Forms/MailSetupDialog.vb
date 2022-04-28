' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

Public Class MailSetupDialog
    Private ReadOnly _invalidNameCharacters As String = "\/:*?""<>| "

    Private ReadOnly _servers As New Dictionary(Of String, String) From {
                {"Microsoft Exchange", ""},
                {"Comcast/Xfinity", "smtp.comcast.net"},
                {"Gmail", "smtp.live.com"},
                {"GoDaddy", "smtpout.secureserver.net"},
                {"Yahoo", "smtp.mail.yahoo.com"}
            }
    Private _useExchange As Boolean = True

    Private Shared Function IsValidEmailAddress(mailServerUserName As String, ByRef errorMsg As String) As Boolean
        If String.IsNullOrWhiteSpace(mailServerUserName) Then
            errorMsg = "Required"
            Return False
        End If

        Try
            Dim tempVar As New System.Net.Mail.MailAddress(mailServerUserName)
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

    Private Sub Cancel_Button_Click(sender As Object, e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
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

    Private Sub MailServerUserNameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MailServerUserEmailTextBox.KeyPress
        Dim keyValue As Char = e.KeyChar

        e.Handled = True
        If _invalidNameCharacters.Contains(e.KeyChar) OrElse (Me.MailServerUserEmailTextBox.SelectionStart = 0 AndAlso e.KeyChar = ".") Then
            Return
        End If
        ' Allow nothing else
        e.Handled = False
    End Sub

    Private Sub MailServerUserEmailTextBox_Validated(sender As Object, e As System.EventArgs) Handles MailServerUserEmailTextBox.Validated
        ' If all conditions have been met, clear the error provider of errors.
        Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, "")
    End Sub

    Private Sub MailServerUserEmailTextBox_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MailServerUserEmailTextBox.Validating
        Dim errorMsg As String = ""
        If Not IsValidEmailAddress(Me.MailServerUserEmailTextBox.Text, errorMsg) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            Me.MailServerUserEmailTextBox.Select(0, Me.MailServerUserEmailTextBox.Text.Length)

            ' Set the ErrorProvider error with the text to display.
            Me.ErrorProvider1.SetError(Me.MailServerUserEmailTextBox, errorMsg)
        End If
    End Sub

    Private Sub MailSetupDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.OutGoingMailServerComboBox.DataSource = New BindingSource(_servers, Nothing)
        Me.OutGoingMailServerComboBox.DisplayMember = "Key"
        Me.OutGoingMailServerComboBox.ValueMember = "Value"

        If Not String.IsNullOrWhiteSpace(My.Settings.OutGoingMailServer) Then
            Me.OutGoingMailServerComboBox.SelectedValue = My.Settings.OutGoingMailServer
        Else
            _useExchange = True
        End If

        If Not _useExchange Then
            If My.Settings.MailServerPort = 0 Then
                Me.ErrorProvider1.SetError(Me.MailServerPortTextBox, "Required")
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

        If String.IsNullOrWhiteSpace(My.Settings.AlertPhoneNumber) Then
            Me.ErrorProvider1.SetError(Me.AlertPhoneNumberMaskedTextBox, "Required")
        Else
            Dim alertPhoneNumber As String = My.Settings.AlertPhoneNumber
            Me.AlertPhoneNumberMaskedTextBox.Text = $"({alertPhoneNumber.Substring(0, 3)}) {alertPhoneNumber.Substring(3, 3)}-{alertPhoneNumber.Substring(6, 4)}"
            Me.AlertPhoneNumberMaskedTextBox.ValidateText()
            Me.ErrorProvider1.SetError(Me.AlertPhoneNumberMaskedTextBox, "")

        End If

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As System.EventArgs) Handles OK_Button.Click
        If Me.AnyControlErrors Then
            Exit Sub
        End If
        Try
            If Me.SendTestMessageCheckBox.Checked Then
                If _useExchange Then
                    Me.Cursor = Cursors.WaitCursor
                    Dim mailTestClient As New SendMail(UseExchange:=True,
                                                       Me.MailServerUserEmailTextBox.Text,
                                                       Me.MailServerPasswordTextBox.Text)
                    mailTestClient.SendUsingExchange($"{Me.AlertPhoneNumberMaskedTextBox.Text}@txt.att.net",
                                                    $"{Form1.LastSG("sg")} {Form1.BgUnitsString}")
                    Me.Cursor = Cursors.Default
                Else
                    Dim port As Integer
                    If Integer.TryParse(Me.MailServerPortTextBox.Text, port) Then
                        Dim mailTestClient As New SendMail(UseExchange:=False,
                            userEmailAddress:=Me.MailServerUserEmailTextBox.Text,
                            userPassword:=Me.MailServerPasswordTextBox.Text,
                            Host:=Me.OutGoingMailServerComboBox.SelectedValue?.ToString(),
                            Port:=port)
                        mailTestClient.Send($"{Me.AlertPhoneNumberMaskedTextBox.Text}@txt.att.net",
                                            Me.MailServerUserEmailTextBox.Text,
                                            $"{Form1.LastSG("sg")} {Form1.BgUnitsString}")
                        My.Settings.MailServerPort = port
                    Else
                        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                        Exit Sub
                    End If
                End If
                My.Settings.OutGoingMailServer = Me.OutGoingMailServerComboBox.SelectedValue.ToString
                My.Settings.MailServerUserName = Me.MailServerUserEmailTextBox.Text
                My.Settings.MailServerPassword = Me.MailServerPasswordTextBox.Text
                My.Settings.AlertPhoneNumber = Me.AlertPhoneNumberMaskedTextBox.Text
                My.Settings.Save()
            End If
            Me.DialogResult = DialogResult.OK
        Catch ex As Exception
            If MsgBox($"Mail send error {ex.Message}, retry?", MsgBoxStyle.OkCancel, "Server Validation Error") = MsgBoxResult.Cancel Then
                Me.DialogResult = DialogResult.Cancel
            Else
                Exit Sub
            End If
        End Try

        Me.Close()
    End Sub

    Private Sub OutGoingMailServerComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles OutGoingMailServerComboBox.SelectedIndexChanged
        _useExchange = String.IsNullOrWhiteSpace(Me.OutGoingMailServerComboBox.SelectedValue.ToString)
        Me.MailServerPortLabel.Visible = Not _useExchange
        Me.MailServerPortTextBox.Visible = Not _useExchange
    End Sub

    Private Sub ShowPasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPasswordCheckBox.CheckedChanged
        Me.MailServerPasswordTextBox.UseSystemPasswordChar = Not Me.ShowPasswordCheckBox.Checked
    End Sub

End Class
