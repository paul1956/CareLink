﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports CareLink

Partial Class ExceptionHandlerForm

    Private Shared Sub CreateReportFile(exceptionText As String, stackTraceText As String, UniqueFileNameWithPath As String, jsonData As Dictionary(Of String, String))
        Using stream As StreamWriter = File.CreateText(UniqueFileNameWithPath)
            ' write exception header
            stream.WriteLine(ExceptionStartingString)
            ' write exception
            stream.WriteLine(exceptionText)
            ' write exception trailer
            stream.WriteLine(ExceptionTerminatingString)
            ' write stack trace header
            stream.WriteLine(StackTraceStartingString)
            ' write stack trace
            stream.WriteLine(stackTraceText)
            ' write stack trace trailer
            stream.WriteLine(StackTraceTerminatingString)
            ' write out data file
            Using jd As JsonDocument = JsonDocument.Parse(jsonData.CleanUserData(), New JsonDocumentOptions)
                stream.Write(JsonSerializer.Serialize(jd, JsonFormattingOptions))
            End Using
        End Using
    End Sub

    Private Shared Function TrimedStackTrace(stackTrace As String) As String
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingString)
        If index < 0 Then
            Return stackTrace
        End If
        Return stackTrace.Substring(0, index - 1)
    End Function

    Private Function DecomposeReportFile(ExceptionTextBox As TextBox, stackTraceTextBox As TextBox) As String

        Using stream As StreamReader = File.OpenText(Me.ReportFileNameWithPath)
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
            stackTraceTextBox.Text = sb.ToString
            Return stream.ReadToEnd
        End Using
    End Function

    Private Sub ReportInvalidErrorFile(currentLine As String, expectedLine As String)
        Throw New NotImplementedException()
    End Sub

End Class