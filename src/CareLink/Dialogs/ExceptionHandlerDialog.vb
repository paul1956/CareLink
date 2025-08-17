' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices

Imports Octokit

''' <summary>
'''  Dialog for handling unhandled exceptions, generating error reports, and guiding users to report issues.
''' </summary>
Public Class ExceptionHandlerDialog

    ''' <summary>
    '''  The GitHub client used for issue reporting.
    ''' </summary>
    Private _gitClient As GitHubClient

    ''' <summary>
    '''  Gets or sets the local raw data extracted from the error report file.
    ''' </summary>
    Public Property LocalRawData As String

    ''' <summary>
    '''  Gets or sets the unhandled exception event arguments.
    ''' </summary>
    Public Property UnhandledException As UnhandledExceptionEventArgs

    ''' <summary>
    '''  Gets or sets the full path to the generated error report file.
    ''' </summary>
    Public Property ReportFileNameWithPath As String

    ''' <summary>
    '''  Creates a report file containing exception and stack trace information, as well as cleaned patient data.
    ''' </summary>
    ''' <param name="exceptionText">The exception message text.</param>
    ''' <param name="stackTraceText">The stack trace text.</param>
    ''' <param name="UniqueFileNameWithPath">The full path for the report file.</param>
    Private Shared Sub CreateReportFile(
            exceptionText As String,
            stackTraceText As String,
            UniqueFileNameWithPath As String)

        Using stream As StreamWriter = File.CreateText(UniqueFileNameWithPath)
            ' write exception header
            stream.WriteLine(value:=ExceptionStartingString)
            ' write exception
            stream.WriteLine(value:=exceptionText)
            ' write exception trailer
            stream.WriteLine(value:=ExceptionTerminatingString)
            ' write stack trace header
            stream.WriteLine(value:=StackTraceStartingStr)
            ' write stack trace
            stream.WriteLine(value:=stackTraceText)
            ' write stack trace trailer
            stream.WriteLine(value:=StackTraceTerminatingStr)
            ' write out data file
            stream.Write(value:=CleanPatientData())
        End Using
    End Sub

    ''' <summary>
    '''  Trims the stack trace string at the terminating marker, if present.
    ''' </summary>
    ''' <param name="stackTrace">The stack trace string to trim.</param>
    ''' <returns>The trimmed stack trace string.</returns>
    Private Shared Function TrimmedStackTrace(stackTrace As String) As String
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingStr)
        Return If(index < 0,
                  stackTrace,
                  stackTrace.Substring(startIndex:=0, length:=index - 1)
                 )
    End Function

    ''' <summary>
    '''  Handles the Cancel button click event. Deletes the report file if it exists and closes the dialog.
    ''' </summary>
    ''' <param name="sender">The sender of the event.</param>
    ''' <param name="e">The event arguments.</param>
    ''' <remarks>This method is called when the Cancel button is clicked.</remarks>
    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        If Not String.IsNullOrWhiteSpace(value:=Me.ReportFileNameWithPath) Then
            File.Delete(path:=Me.ReportFileNameWithPath)
        End If
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary>
    '''  Handles the form load event. Initializes the dialog,
    '''  sets up the GitHub client, and prepares the instructions.
    ''' </summary>
    ''' <param name="sender">The sender of the event.</param>
    ''' <param name="e">The event arguments.</param>
    ''' <remarks>This method is called when the dialog is loaded.</remarks>
    Private Sub ExceptionHandlerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartOrStopServerUpdateTimer(Start:=False)
        Dim rtb As RichTextBox = Me.InstructionsRichTextBox
        Dim newFont As Font = rtb.Font
        _gitClient = New GitHubClient(
            productInformation:=New ProductHeaderValue(name:="CareLink.Issues"),
            baseAddress:=New Uri(uriString:=GitHubCareLinkUrl))
        Dim fontBold As New Font(prototype:=rtb.Font, newStyle:=FontStyle.Bold)
        If String.IsNullOrWhiteSpace(value:=Me.ReportFileNameWithPath) Then
            ' Create error report and issue
            Me.ExceptionTextBox.Text = Me.UnhandledException.Exception.Message
            Me.StackTraceTextBox.Text =
                TrimmedStackTrace(Me.UnhandledException.Exception.StackTrace)

            rtb.Text =
                "By clicking OK, the Stack Trace, Exception " &
                "and the CareLink™ data that caused the error will" &
                $" be package as a text file called{vbCrLf}"
            Dim uniqueFileNameResult As FileNameStruct = GetUniqueDataFileName(
                baseName:=BaseNameSavedErrorReport,
                cultureName:=CurrentDateCulture.Name,
                extension:="txt",
                mustBeUnique:=True)

            Dim fileLink As String =
                $"{uniqueFileNameResult.withoutPath}: file://{uniqueFileNameResult.withPath}"
            AppendTextWithFontChange(rtb, text:=fileLink, newFont:=fontBold, padRight:=0)
            AppendTextWithFontChange(
                rtb,
                text:="and stored in",
                newFont:=newFont,
                padRight:=0)
            AppendTextWithFontChange(
                rtb,
                text:=GetProjectDataDirectory(),
                newFont:=fontBold,
                padRight:=0)
            AppendTextWithFontChange(
                rtb,
                text:="You can review what is being stored and" &
                      " then attach it to a new issue at",
                newFont:=newFont,
                padRight:=0)
            AppendTextWithFontChange(
                rtb,
                text:="You can review what is being stored and" &
                      " then attach it to a new issue at",
                newFont:=newFont,
                padRight:=0)
            AppendTextWithFontChange(
                rtb,
                text:=$"{_gitClient.Repository.Get(
                            owner:=GitOwnerName,
                            name:="CareLink").Result.HtmlUrl}/issues.",
                newFont:=newFont,
                padRight:=0)
            AppendTextWithFontChange(
                rtb,
                text:="This will help me isolate issues quickly.",
                newFont:=newFont,
                padRight:=0)
            CreateReportFile(
                exceptionText:=Me.ExceptionTextBox.Text,
                stackTraceText:=Me.StackTraceTextBox.Text,
                UniqueFileNameWithPath:=uniqueFileNameResult.withPath)
        Else
            CurrentDateCulture =
                Me.ReportFileNameWithPath.ExtractCultureFromFileName(FixedPart:=BaseNameSavedErrorReport)
            If CurrentDateCulture Is Nothing Then
                Me.Close()
                Exit Sub
            End If
            rtb.Text = $"Clicking OK will rerun the data file that caused the error{vbCrLf}"
            Dim fileLink As String =
                $"{Path.GetFileName(path:=Me.ReportFileNameWithPath)}: file://{Me.ReportFileNameWithPath}"
            AppendTextWithFontChange(rtb, text:=fileLink, newFont:=fontBold, padRight:=0)
            AppendTextWithFontChange(rtb, text:="and stored in", newFont:=newFont, padRight:=0)
            AppendTextWithFontChange(rtb, text:=GetProjectDataDirectory(), newFont:=fontBold, padRight:=0)
            Me.LocalRawData =
                Me.DecomposeReportFile(Me.ExceptionTextBox, Me.StackTraceTextBox, Me.ReportFileNameWithPath)
        End If
    End Sub

    ''' <summary>
    '''  Handles the form shown event. Hides the main form and sets the dialog to be topmost.
    ''' </summary>
    ''' <param name="sender">The sender of the event.</param>
    ''' <param name="e">The event arguments.</param>
    ''' <remarks>This is used to ensure the dialog is displayed above other forms.</remarks>
    Private Sub ExceptionHandlerForm_Shown(sender As Object, e As EventArgs) _
        Handles MyBase.Shown

        My.Forms.SgMiniForm.Hide()
        My.Forms.Form1.Show()
        Me.TopMost = True
    End Sub

    ''' <summary>
    '''  Handles the link clicked event in the instructions rich text box.
    '''  Opens the link in the default web browser or Explorer if it is a file link.
    ''' </summary>
    ''' <param name="sender">The sender of the event.</param>
    ''' <param name="e">The event arguments containing the link text.</param>
    ''' <remarks>
    '''  This method is called when a link in the instructions rich text box is clicked.
    ''' </remarks>
    Private Sub InstructionsRichTextBox_LinkClicked(
            sender As Object,
            e As LinkClickedEventArgs) Handles InstructionsRichTextBox.LinkClicked

        Const value As String = "file://"
        Dim startIndex As Integer = value.Length
        If e.LinkText.StartsWith(value) Then
            Process.Start(fileName:="Explorer.exe", arguments:=e.LinkText.Substring(startIndex))
        Else
            OpenUrlInBrowser(url:=e.LinkText)
        End If
    End Sub

    ''' <summary>
    '''  Handles the OK button click event. Sets the dialog result based on whether a report file is specified.
    ''' </summary>
    ''' <param name="sender">The sender of the event.</param>
    ''' <param name="e">The event arguments.</param>
    ''' <remarks>This method is called when the OK button is clicked.</remarks>
    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        Me.OK.Enabled = False
        Me.Cancel.Enabled = False
        If String.IsNullOrWhiteSpace(value:=Me.ReportFileNameWithPath) Then
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

    ''' <summary>
    '''  Reports an invalid error file by throwing a NotImplementedException.
    ''' </summary>
    ''' <param name="currentLine">The current line read from the file.</param>
    ''' <param name="exceptionStartingString">The expected starting string for the exception section.</param>
    Private Sub ReportInvalidErrorFile(currentLine As String, exceptionStartingString As String)
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    '''  Decomposes a report file into its exception, stack trace, and data components.
    ''' </summary>
    ''' <param name="ExceptionTextBox">The TextBox to receive the exception message.</param>
    ''' <param name="stackTraceTextBox">The TextBox to receive the stack trace.</param>
    ''' <param name="ReportFileNameWithPath">The full path to the report file.</param>
    ''' <returns>The raw data portion of the report file.</returns>
    Friend Function DecomposeReportFile(
        ExceptionTextBox As TextBox,
        stackTraceTextBox As TextBox,
        ReportFileNameWithPath As String) As String

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
                Me.ReportInvalidErrorFile(currentLine, exceptionStartingString:=ExceptionTerminatingString)
            End If

            ' read stack trace header
            currentLine = stream.ReadLine
            If currentLine <> StackTraceStartingStr Then
                Me.ReportInvalidErrorFile(currentLine, exceptionStartingString:=StackTraceStartingStr)
            End If

            ' read stack trace
            Dim sb As New StringBuilder
            While stream.Peek > 0
                currentLine = stream.ReadLine
                If currentLine <> StackTraceTerminatingStr Then
                    sb.AppendLine(value:=currentLine)
                Else
                    Exit While
                End If
                currentLine = ""
            End While
            If currentLine <> StackTraceTerminatingStr Then
                Me.ReportInvalidErrorFile(currentLine, exceptionStartingString:=StackTraceTerminatingStr)
            End If
            stackTraceTextBox.Text = sb.ToString
            Return stream.ReadToEnd
        End Using
    End Function

End Class
