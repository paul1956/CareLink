' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices
Imports Octokit

Public Class ExceptionHandlerForm
    Private _gitClient As GitHubClient
    Public Property LocalRawData As String
    Public Property UnhandledException As UnhandledExceptionEventArgs
    Public Property ReportFileNameWithPath As String

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        If Not String.IsNullOrWhiteSpace(Me.ReportFileNameWithPath) Then
            File.Delete(Me.ReportFileNameWithPath)
        End If
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        My.Forms.Form1.ServerUpdateTimer.Stop()
        Dim fontBold As New Font(Me.InstructionsRichTextBox.Font, FontStyle.Bold)
        Dim fontNormal As Font = Me.InstructionsRichTextBox.Font
        _gitClient = New GitHubClient(New ProductHeaderValue($"{RepoName}.Issues"), New Uri(GitHubCareLinkUrl))
        If String.IsNullOrWhiteSpace(Me.ReportFileNameWithPath) Then
            ' Create error report and issue
            Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
            Me.StackTraceTextBox.Text = TrimedStackTrace(Me.UnhandledException.Exception.StackTrace)

            Me.InstructionsRichTextBox.Text = $"By clicking OK, the Stack Trace, Exception and the CareLink data that caused the error will be package as a text file called" & Environment.NewLine
            Dim uniqueFileNameResult As FileNameStruct = GetDataFileName(SavedErrorReportName, CurrentDateCulture.Name, "txt", True)
            Dim fileLink As String = $"{uniqueFileNameResult.withoutPath}: file://{uniqueFileNameResult.withPath}"
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, MyDocumentsPath, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "You can review what is being stored and then attach it to a new issue at", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, $"{_gitClient.Repository.Get(OwnerName, RepoName).Result.HtmlUrl}/issues.", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "This will help me isolate issues quickly.", fontNormal)
            CreateReportFile(Me.ExceptionTextBox.Text, Me.StackTraceTextBox.Text, uniqueFileNameResult.withPath, My.Forms.Form1.RecentData)
        Else
            CurrentDateCulture = Me.ReportFileNameWithPath.ExtractCultureFromFileName(SavedErrorReportName)
            If CurrentDateCulture Is Nothing Then
                Me.Close()
                Exit Sub
            End If
            Me.InstructionsRichTextBox.Text = $"Clicking OK will rerun the data file that caused the error" & Environment.NewLine
            Dim fileLink As String = $"{Path.GetFileName(Me.ReportFileNameWithPath)}: file://{Me.ReportFileNameWithPath}"
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, MyDocumentsPath, fontBold)
            Me.LocalRawData = Me.DecomposeReportFile(Me.ExceptionTextBox, Me.StackTraceTextBox)
        End If
    End Sub

    Private Sub ExceptionHandlerForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        My.Forms.BGMiniWindow.Hide()
        My.Forms.Form1.Show()
        Me.TopMost = True
    End Sub

    Private Sub InstructionsRichTextBox_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles InstructionsRichTextBox.LinkClicked
        If e.LinkText.StartsWith("file://") Then
            Process.Start("Explorer.exe", e.LinkText.Substring(7))
        Else
            OpenUrlInBrowser(e.LinkText)
        End If
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        Me.OK.Enabled = False
        Me.Cancel.Enabled = False
        If String.IsNullOrWhiteSpace(Me.ReportFileNameWithPath) Then
            ' This branch creates a new report and will exit
            ' program upon return

            Me.DialogResult = DialogResult.Cancel
        Else
            ' This branch displays an existing Error Report
            ' and re-reruns it.
            Me.DialogResult = DialogResult.OK
        End If
        Me.OK.Enabled = True
        Me.Cancel.Enabled = True
        Me.Close()
    End Sub

End Class
