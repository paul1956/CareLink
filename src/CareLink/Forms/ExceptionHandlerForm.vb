' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text.Json
Imports Microsoft.VisualBasic.ApplicationServices
Imports Octokit

Public Class ExceptionHandlerForm

    Private _reportFileName As String
    Public Property UnhandledException As UnhandledExceptionEventArgs

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        File.Delete(_reportFileName)
        Me.Close()
    End Sub

    Private Sub CreateReportFile(reportFile As String)
        Using stream As StreamWriter = File.CreateText(reportFile)
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
            Using jd As JsonDocument = JsonDocument.Parse(Form1.RecentData.CleanUserData(), New JsonDocumentOptions)
                stream.Write(JsonSerializer.Serialize(jd, JsonFormattingOptions))
            End Using
        End Using
    End Sub

    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Form1.ServerUpdateTimer.Stop()
        _reportFileName = GetUniqueFileNameWithPath(Path.Combine(MyDocumentsPath, $"{ErrorReportName}({CurrentUICulture.Name}).txt"))
        Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
        Me.StackTraceTextBox.Text = Me.TrimedStackTrace()
        Dim fontBold As New Font(Me.InstructionsRichTextBox.Font, FontStyle.Bold)
        Dim fontNormal As Font = Me.InstructionsRichTextBox.Font
        Dim client As New GitHubClient(New ProductHeaderValue($"{RepoName}.Issues"))

        Me.InstructionsRichTextBox.Text = $"By clicking OK, the Stack Trace, Exception and the CareLink data that caused the error will be package as a text file called" & Environment.NewLine
        Dim fileName As String = Path.GetFileName(_reportFileName)
        Dim fileLink As String = $"{fileName}: file://{Path.Combine(MyDocumentsPath, fileName)}"
        AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
        AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", fontNormal)
        AppendTextWithFontAndColor(Me.InstructionsRichTextBox, MyDocumentsPath, fontBold)
        AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "You can review what is being stored and then attach it to a new issue at", fontNormal)
        AppendTextWithFontAndColor(Me.InstructionsRichTextBox, $"{client.Repository.Get(OwnerName, RepoName).Result.HtmlUrl}/issues.", fontNormal)
        AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "This will help me isolate issues quickly.", fontNormal)
        Me.CreateReportFile(_reportFileName)
    End Sub

    Private Sub InstructionsRichTextBox_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles InstructionsRichTextBox.LinkClicked
        If e.LinkText.StartsWith("file://") Then
            Process.Start("Explorer.exe", e.LinkText.Substring(7))
        Else
            OpenUrlInBrowser(e.LinkText)
        End If
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        Me.Close()
    End Sub

    Private Function TrimedStackTrace() As String
        Dim stackTrace As String = Me.UnhandledException.Exception.StackTrace
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingString)
        If index < 0 Then
            Return stackTrace
        End If
        Return stackTrace.Substring(0, index - 1)
    End Function

End Class
