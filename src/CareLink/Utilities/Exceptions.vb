' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices

Friend Module Exceptions
    Private Const InnerExceptionMessage As String = ", see inner exception."

    <ExcludeFromCodeCoverage>
    Public ReadOnly Property UnreachableException(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Exception
        Get
            Return New InvalidOperationException($"The program location {memberName} line {sourceLineNumber} is thought to be unreachable.")
        End Get
    End Property

    <Extension>
    Public Function DecodeException(ex As Exception) As String
        Dim errorMsg As String = ex.Message
        If errorMsg.Contains(InnerExceptionMessage) Then
            Dim innerExMessage As String = ex.InnerException.Message
            If innerExMessage.Contains(InnerExceptionMessage) Then
                errorMsg = DecodeException(ex.InnerException)
            Else
                errorMsg = $"{errorMsg.Replace(InnerExceptionMessage, ".")}{s_environmentNewLine}{innerExMessage}"
            End If
        End If

        Return errorMsg
    End Function

End Module
