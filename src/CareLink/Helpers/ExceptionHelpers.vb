' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.CodeAnalysis
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module ExceptionHelpers
    Private Const InnerExceptionMessage As String = ", see inner exception."

    <ExcludeFromCodeCoverage>
    Public ReadOnly Property UnreachableException(propertyName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Exception
        Get
            Return New ArgumentOutOfRangeException(propertyName, $"The program location {memberName} line {sourceLineNumber} is thought to be unreachable.")
        End Get
    End Property

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

    Friend Function TrimmedStackTrace(stackTrace As String) As String
        Dim index As Integer = stackTrace.IndexOf(StackTraceTerminatingStr)
        Return If(index < 0,
                  stackTrace,
                  stackTrace.Substring(0, index - 1)
                 )
    End Function

    <Extension>
    Public Function DecodeException(ex As Exception) As String
        Dim errorMsg As String = ex.Message
        If errorMsg.Contains(InnerExceptionMessage) Then
            Dim innerExMessage As String = ex.InnerException.Message
            errorMsg = If(innerExMessage.Contains(InnerExceptionMessage),
                          DecodeException(ex.InnerException),
                          $"{errorMsg.Replace(InnerExceptionMessage, ".")}{vbCrLf}{innerExMessage}"
                         )
        End If

        Return errorMsg
    End Function

End Module
