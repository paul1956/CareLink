' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports CareLink

Public Module ExceptionHandlerFormHelpers

    Private Sub ReportInvalidErrorFile(currentLine As String, expectedLine As String)
        Throw New NotImplementedException()
    End Sub

    Friend Sub CreateReportFile(exceptionText As String, stackTraceText As String, UniqueFileNameWithPath As String, jsonData As Dictionary(Of String, String))
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
            Using jd As JsonDocument = JsonDocument.Parse(jsonData.CleanUserData(), New JsonDocumentOptions)
                stream.Write(JsonSerializer.Serialize(jd, JsonFormattingOptions))
            End Using
        End Using
    End Sub

    Friend Function DecomposeReportFile(ExceptionTextBox As TextBox, stackTraceTextBox As TextBox, ReportFileNameWithPath As String) As String

        Using stream As StreamReader = File.OpenText(ReportFileNameWithPath)
            ' read exception header
            Dim currentLine As String = stream.ReadLine()
            If currentLine <> ExceptionStartingString Then
                ReportInvalidErrorFile(currentLine, ExceptionStartingString)
            End If

            ' read exception
            ExceptionTextBox.Text = stream.ReadLine

            ' read exception trailer
            currentLine = stream.ReadLine
            If currentLine <> ExceptionTerminatingString Then
                ReportInvalidErrorFile(currentLine, ExceptionTerminatingString)
            End If

            ' read stack trace header
            currentLine = stream.ReadLine
            If currentLine <> StackTraceStartingStr Then
                ReportInvalidErrorFile(currentLine, StackTraceStartingStr)
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
                ReportInvalidErrorFile(currentLine, StackTraceTerminatingStr)
            End If
            stackTraceTextBox.Text = sb.ToString
            Return stream.ReadToEnd
        End Using
    End Function

    Friend Function TrimmedStackTrace(stackTrace As String) As String
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingStr)
        If index < 0 Then
            Return stackTrace
        End If
        Return stackTrace.Substring(0, index - 1)
    End Function

End Module
