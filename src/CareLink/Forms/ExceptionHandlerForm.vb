' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports Octokit

Public Class ExceptionHandlerForm
    Private Const OwnerName As String = "paul1956"
    Private Const RepoName As String = "CareLink"
    Private ReadOnly _productInformation As New ProductHeaderValue("CareLink.Issues")
    Public Property UnhandledException As ApplicationServices.UnhandledExceptionEventArgs

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim client As New GitHubClient(_productInformation)

        Me.InstructionsRichTextBox.Text = "By clicking OK, the Stack Trace and Exception will be sent a new issue and posted to GitHub along with the CareLink data file that caused the error." & vbCrLf _
            & $"You can review it at {client.Repository.Get(OwnerName, RepoName).Result.HtmlUrl}/issues"
        Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
        Me.StackTraceTextBox.Text = Me.UnhandledException.Exception.StackTrace
    End Sub

    Private Async Sub OK_ClickAsync(sender As Object, e As EventArgs) Handles OK.Click
        If Me.UsernameTextBox.Text.Length = 0 Then
            Me.UsernameTextBox.Focus()
            Exit Sub
        End If
        If Me.PasswordTextBox.Text.Length = 0 Then
            Me.PasswordTextBox.Focus()
            Exit Sub
        End If

        Try
            Dim client As New GitHubClient(_productInformation) With {
                .Credentials = New Credentials(Me.UsernameTextBox.Text, Me.PasswordTextBox.Text)
            }
            Dim issuesClient As IIssuesClient = client.Issue
            Dim newIssue As Issue = Await issuesClient.Create(OwnerName, RepoName, New NewIssue(Me.UnhandledException.Exception.Message))
        Catch ex As Exception
            Stop
        End Try
        Me.Close()
    End Sub

    Private Sub PasswordTextBox_Validating(sender As Object, e As CancelEventArgs) Handles PasswordTextBox.Validating
        If String.IsNullOrWhiteSpace(Me.PasswordTextBox.Text) Then
            Me.ErrorProvider1.SetError(Me.PasswordTextBox, "Password can't be empty.")
        Else
            ' Clear the error.
            Me.ErrorProvider1.SetError(Me.PasswordTextBox, "")
        End If

    End Sub

    Private Sub ShowPasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPasswordCheckBox.CheckedChanged
        If Me.ShowPasswordCheckBox.Checked Then
            Me.PasswordTextBox.PasswordChar = Nothing
        Else
            Me.PasswordTextBox.PasswordChar = "*"c
        End If

    End Sub

    Private Sub UsernameTextBox_Validating(sender As Object, e As CancelEventArgs) Handles UsernameTextBox.Validating
        If String.IsNullOrWhiteSpace(Me.UsernameTextBox.Text) Then
            Me.ErrorProvider1.SetError(Me.UsernameTextBox, "Username can't be empty.")
        Else
            ' Clear the error.
            Me.ErrorProvider1.SetError(Me.UsernameTextBox, "")
        End If
    End Sub

End Class
