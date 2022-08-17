' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports Microsoft.VisualBasic.ApplicationServices
Imports Octokit

Public Class ExceptionHandlerForm
    Private _gitClient As GitHubClient
    Private _uniqueFileNameResult As (withPath As String, withoutPath As String) = Nothing
    Public Property LocalRawData As String
    Public Property ReportFileNameWithPath As String
    Public Property UnhandledException As UnhandledExceptionEventArgs

#Region "Form Events"

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        If Not String.IsNullOrWhiteSpace(_uniqueFileNameResult.withPath) Then
            File.Delete(_uniqueFileNameResult.withPath)
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
            Me.StackTraceTextBox.Text = Me.TrimedStackTrace()

            Me.InstructionsRichTextBox.Text = $"By clicking OK, the Stack Trace, Exception and the CareLink data that caused the error will be package as a text file called" & Environment.NewLine
            Dim uniqueFileNameResult As (withPath As String, withoutPath As String) = GetDataFileName(RepoErrorReportName, CurrentDateCulture.Name, "txt", True)
            Dim fileLink As String = $"{uniqueFileNameResult.withoutPath}: file://{uniqueFileNameResult.withPath}"
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, MyDocumentsPath, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "You can review what is being stored and then attach it to a new issue at", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, $"{_gitClient.Repository.Get(OwnerName, RepoName).Result.HtmlUrl}/issues.", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "This will help me isolate issues quickly.", fontNormal)
            Me.CreateReportFile(uniqueFileNameResult.withPath)
        Else
            CurrentDateCulture = Me.ReportFileNameWithPath.ExtractCultureFromFileName(RepoErrorReportName)
            If CurrentDateCulture Is Nothing Then
                Me.Close()
                Exit Sub
            End If
            Me.InstructionsRichTextBox.Text = $"Clicking OK will rerun the data file that caused the error" & Environment.NewLine
            Dim fileLink As String = $"{Path.GetFileName(Me.ReportFileNameWithPath)}: file://{Me.ReportFileNameWithPath}"
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", fontNormal)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, MyDocumentsPath, fontBold)
            Me.LocalRawData = Me.DecomposeReportFile()
        End If
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

#End Region

    Private Sub CreateReportFile(UniqueFileNameWithPath As String)
        Using stream As StreamWriter = File.CreateText(UniqueFileNameWithPath)
            ' write exception header
            stream.WriteLine(ExceptionStartingString)
            ' write exception
            stream.WriteLine(Me.ExceptionTextBox.Text)
            ' write exception trailer
            stream.WriteLine(ExceptionTerminatingString)
            ' write stack trace header
            stream.WriteLine(StackTraceStartingString)
            ' write stack trace
            stream.WriteLine(Me.StackTraceTextBox.Text)
            ' write stack trace trailer
            stream.WriteLine(StackTraceTerminatingString)
            ' write out data file
            Using jd As JsonDocument = JsonDocument.Parse(RecentData.CleanUserData(), New JsonDocumentOptions)
                stream.Write(JsonSerializer.Serialize(jd, JsonFormattingOptions))
            End Using
        End Using
    End Sub

    Private Function DecomposeReportFile() As String

        Using stream As StreamReader = File.OpenText(Me.ReportFileNameWithPath)
            ' read exception header
            Dim currentLine As String = stream.ReadLine()
            If currentLine <> ExceptionStartingString Then
                Me.ReportInvalidErrorFile(currentLine, ExceptionStartingString)
            End If

            ' read exception
            Me.ExceptionTextBox.Text = stream.ReadLine

            ' read exception trailer
            currentLine = stream.ReadLine
            If currentLine <> ExceptionTerminatingString Then
                Me.ReportInvalidErrorFile(currentLine, ExceptionTerminatingString)
            End If

            ' read stack trace header
            currentLine = stream.ReadLine
            If currentLine <> StackTraceStartingString Then
                Me.ReportInvalidErrorFile(currentLine, StackTraceStartingString)
            End If

            ' read stack trace
            Dim sb As New StringBuilder
            While stream.Peek > 0
                currentLine = stream.ReadLine
                If currentLine <> StackTraceTerminatingString Then
                    sb.AppendLine(currentLine)
                Else
                    Exit While
                End If
                currentLine = ""
            End While
            If currentLine <> StackTraceTerminatingString Then
                Me.ReportInvalidErrorFile(currentLine, StackTraceTerminatingString)
            End If
            Me.StackTraceTextBox.Text = sb.ToString
            Return stream.ReadToEnd
        End Using
    End Function

    Private Sub ReportInvalidErrorFile(currentLine As String, expectedLine As String)
        Throw New NotImplementedException()
    End Sub

    Private Function TrimedStackTrace() As String
        Dim stackTrace As String = Me.UnhandledException.Exception.StackTrace
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingString)
        If index < 0 Then
            Return stackTrace
        End If
        Return stackTrace.Substring(0, index - 1)
    End Function

    Private Sub ExceptionHandlerForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        My.Forms.BGMiniWindow.Hide()
        My.Forms.Form1.Show()
        Me.TopMost = True
    End Sub
End Class
