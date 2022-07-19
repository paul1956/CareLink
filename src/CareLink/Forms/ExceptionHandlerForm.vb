' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Octokit

Public Class ExceptionHandlerForm
    Private _issuesClient As IssuesClient
    Public Property UnhandledException As ApplicationServices.UnhandledExceptionEventArgs
    Private Async Sub OK_ClickAsync(sender As Object, e As EventArgs) Handles OK.Click
        Dim createIssue As New NewIssue(Me.UnhandledException.Exception.Message)
        Try
            Dim issueResult As Issue = Await _issuesClient.Create("paul1956", "careLink", createIssue)
        Catch ex As Exception
            Stop
        End Try
        Me.Close()
    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim baseUrl As New Uri("https://github.com/paul1956/")
        Dim client As New GitHubClient(New ProductHeaderValue("CareLink"), baseUrl)
        Dim apiconn As IApiConnection = New ApiConnection(client.Connection)
        _issuesClient = New IssuesClient(apiconn)
        Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
        Me.StackTraceTextBox.Text = Me.UnhandledException.Exception.StackTrace
    End Sub
End Class
