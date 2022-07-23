' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Windows
Imports Microsoft.VisualBasic.ApplicationServices
Imports Octokit


Public Class ExceptionHandlerForm
    Private Const OwnerName As String = "paul1956"
    Private Const RepoName As String = "CareLink"
    Private Const TerminatingString As String = "--- End of stack trace from previous location ---"

    Private ReadOnly _productInformation As New ProductHeaderValue("CareLink.Issues")

    Private _reportFileName As String
    Public Property UnhandledException As UnhandledExceptionEventArgs

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Form1.ServerUpdateTimer.Stop()
        _reportFileName = GetUniqueFileName(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{RepoName}({CultureInfo.CurrentUICulture.Name})ErrorReport.txt"))
        Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
        Me.StackTraceTextBox.Text = Me.TrimedStackTrace()
    End Sub

    Private Sub ExceptionHandlerForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim fontBold As New Font(Me.InstructionsRichTextBox.Font, FontStyle.Bold)
        Dim fontNormal As Font = Me.InstructionsRichTextBox.Font
        Dim client As New GitHubClient(_productInformation)

        Me.InstructionsRichTextBox.Text = $"By clicking OK, the Stack Trace, Exception and the CareLink data that caused the error will be package as a text file called" & vbCrLf
        Forms.Application.DoEvents()
        Me.InstructionsRichTextBox.Select(Me.InstructionsRichTextBox.Text.Length, 0)
        Forms.Application.DoEvents()
        Me.InstructionsRichTextBox.SelectionFont = fontBold
        Me.InstructionsRichTextBox.Text &= Path.GetFileName(_reportFileName) & vbCrLf
        Forms.Application.DoEvents()
        Me.InstructionsRichTextBox.Select(Me.InstructionsRichTextBox.Text.Length, 0)
        Me.InstructionsRichTextBox.SelectionFont = fontNormal
        Me.InstructionsRichTextBox.Text &= "and stored in" & vbCrLf
        Forms.Application.DoEvents()
        Me.InstructionsRichTextBox.Select(Me.InstructionsRichTextBox.Text.Length, 0)
        Me.InstructionsRichTextBox.SelectionFont = fontBold
        Me.InstructionsRichTextBox.Text &= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & vbCrLf
        Forms.Application.DoEvents()
        Me.InstructionsRichTextBox.Select(Me.InstructionsRichTextBox.Text.Length, 0)
        Me.InstructionsRichTextBox.SelectionFont = fontNormal
        Forms.Application.DoEvents()
        Me.InstructionsRichTextBox.Text &= $"You can review what is being sent and then attach it to a new issue at" & vbCrLf
        Me.InstructionsRichTextBox.Text &= $"{client.Repository.Get(OwnerName, RepoName).Result.HtmlUrl}/issues." & vbCrLf
        Me.InstructionsRichTextBox.Text &= "This will help me isolate issues quickly."

    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        Try
        Catch ex As Exception
            Stop
        End Try
        Me.Close()
    End Sub
    Private Function TrimedStackTrace() As String
        Dim stackTrace As String = Me.UnhandledException.Exception.StackTrace
        Dim index As Integer = stackTrace.IndexOf(TerminatingString)
        If index < 0 Then
            Return stackTrace
        End If
        Return stackTrace.Substring(0, index - 1)
    End Function

End Class
