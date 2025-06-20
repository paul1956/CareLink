﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices

Imports Octokit

Public Class ExceptionHandlerDialog
    Private _gitClient As GitHubClient
    Public Property LocalRawData As String
    Public Property UnhandledException As UnhandledExceptionEventArgs
    Public Property ReportFileNameWithPath As String

    Private Shared Sub CreateReportFile(exceptionText As String, stackTraceText As String, UniqueFileNameWithPath As String)
        Using stream As StreamWriter = File.CreateText(UniqueFileNameWithPath)
            ' write exception header
            stream.WriteLine(ExceptionStartingString)
            ' write exception
            stream.WriteLine(exceptionText)
            ' write exception trailer
            stream.WriteLine(ExceptionTerminatingString)
            ' write stack trace header
            stream.WriteLine(StackTraceStartingStr)
            ' write stack trace
            stream.WriteLine(stackTraceText)
            ' write stack trace trailer
            stream.WriteLine(StackTraceTerminatingStr)
            ' write out data file
            stream.Write(CleanPatientData())
        End Using
    End Sub

    Private Shared Function TrimmedStackTrace(stackTrace As String) As String
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingStr)
        Return If(index < 0,
                  stackTrace,
                  stackTrace.Substring(startIndex:=0, length:=index - 1)
                 )
    End Function

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        If Not String.IsNullOrWhiteSpace(Me.ReportFileNameWithPath) Then
            File.Delete(Me.ReportFileNameWithPath)
        End If
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartOrStopServerUpdateTimer(Start:=False)
        Dim fontBold As New Font(prototype:=Me.InstructionsRichTextBox.Font, newStyle:=FontStyle.Bold)
        Dim newFont As Font = Me.InstructionsRichTextBox.Font
        _gitClient = New GitHubClient(
            productInformation:=New ProductHeaderValue(name:="CareLink.Issues"),
            baseAddress:=New Uri(uriString:=GitHubCareLinkUrl))
        If String.IsNullOrWhiteSpace(Me.ReportFileNameWithPath) Then
            ' Create error report and issue
            Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
            Me.StackTraceTextBox.Text = TrimmedStackTrace(Me.UnhandledException.Exception.StackTrace)

            Me.InstructionsRichTextBox.Text = $"By clicking OK, the Stack Trace, Exception and the CareLink™ data that caused the error will be package as a text file called{vbCrLf}"
            Dim uniqueFileNameResult As FileNameStruct = GetUniqueDataFileName(BaseNameSavedErrorReport, CurrentDateCulture.Name, "txt", mustBeUnique:=True)
            Dim fileLink As String = $"{uniqueFileNameResult.withoutPath}: file://{uniqueFileNameResult.withPath}"
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", newFont)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, DirectoryForProjectData, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "You can review what is being stored and then attach it to a new issue at", newFont)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, $"{_gitClient.Repository.Get(GitOwnerName, "CareLink").Result.HtmlUrl}/issues.", newFont)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "This will help me isolate issues quickly.", newFont)
            CreateReportFile(Me.ExceptionTextBox.Text, Me.StackTraceTextBox.Text, uniqueFileNameResult.withPath)
        Else
            CurrentDateCulture = Me.ReportFileNameWithPath.ExtractCultureFromFileName(BaseNameSavedErrorReport)
            If CurrentDateCulture Is Nothing Then
                Me.Close()
                Exit Sub
            End If
            Me.InstructionsRichTextBox.Text = $"Clicking OK will rerun the data file that caused the error{vbCrLf}"
            Dim fileLink As String = $"{Path.GetFileName(Me.ReportFileNameWithPath)}: file://{Me.ReportFileNameWithPath}"
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, fileLink, fontBold)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, "and stored in", newFont)
            AppendTextWithFontAndColor(Me.InstructionsRichTextBox, DirectoryForProjectData, fontBold)
            Me.LocalRawData = Me.DecomposeReportFile(Me.ExceptionTextBox, Me.StackTraceTextBox, Me.ReportFileNameWithPath)
        End If
    End Sub

    Private Sub ExceptionHandlerForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        My.Forms.SgMiniForm.Hide()
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

    Private Sub ReportInvalidErrorFile(currentLine As String, exceptionStartingString As String)
        Throw New NotImplementedException()
    End Sub

    Friend Function DecomposeReportFile(ExceptionTextBox As TextBox, stackTraceTextBox As TextBox, ReportFileNameWithPath As String) As String

        Using stream As StreamReader = File.OpenText(ReportFileNameWithPath)
            ' read exception header
            Dim currentLine As String = stream.ReadLine()
            If currentLine <> ExceptionStartingString Then
                Me.ReportInvalidErrorFile(currentLine, ExceptionStartingString)
            End If

            ' read exception
            ExceptionTextBox.Text = stream.ReadLine

            ' read exception trailer
            currentLine = stream.ReadLine
            If currentLine <> ExceptionTerminatingString Then
                Me.ReportInvalidErrorFile(currentLine, ExceptionTerminatingString)
            End If

            ' read stack trace header
            currentLine = stream.ReadLine
            If currentLine <> StackTraceStartingStr Then
                Me.ReportInvalidErrorFile(currentLine, StackTraceStartingStr)
            End If

            ' read stack trace
            Dim sb As New StringBuilder
            While stream.Peek > 0
                currentLine = stream.ReadLine
                If currentLine <> StackTraceTerminatingStr Then
                    sb.AppendLine(currentLine)
                Else
                    Exit While
                End If
                currentLine = ""
            End While
            If currentLine <> StackTraceTerminatingStr Then
                Me.ReportInvalidErrorFile(currentLine, StackTraceTerminatingStr)
            End If
            stackTraceTextBox.Text = sb.ToString
            Return stream.ReadToEnd
        End Using
    End Function

End Class
