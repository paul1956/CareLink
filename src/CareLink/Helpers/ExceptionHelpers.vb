' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides helper methods and properties for exception handling and decoding.
''' </summary>
Friend Module ExceptionHelpers
    Private Const InnerExceptionMessage As String = ", see inner exception."

    ''' <summary>
    '''  Gets an <see cref="ArgumentOutOfRangeException"/> indicating
    '''  an unreachable program location.
    ''' </summary>
    ''' <param name="paramName">
    '''  The name of the property or parameter that caused the exception.
    ''' </param>
    ''' <param name="memberName">
    '''  The name of the member where the exception occurred.
    '''  Automatically supplied by the compiler.</param>
    ''' <param name="sourceLineNumber">
    '''  The line number in the source file where the exception occurred.
    '''  Automatically supplied by the compiler.
    ''' </param>
    ''' <returns>
    '''  An <see cref="ArgumentOutOfRangeException"/> describing the unreachable code location.
    ''' </returns>
    <ExcludeFromCodeCoverage>
    Public ReadOnly Property UnreachableException(
        paramName As String,
        <CallerMemberName> Optional memberName As String = Nothing,
        <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Exception

        Get
            Dim message As String =
                $"The program location {memberName} line {sourceLineNumber}" &
                " is thought to be unreachable."
            Return New ArgumentOutOfRangeException(paramName, message)
        End Get
    End Property

    ''' <summary>
    '''  Decodes an <see cref="Exception"/> to a readable string, recursively including inner exception messages.
    ''' </summary>
    ''' <param name="ex">The exception to decode.</param>
    ''' <returns>
    '''  A string containing the exception message and any inner exception messages, formatted for readability.
    ''' </returns>
    <Extension>
    Public Function DecodeException(ex As Exception) As String
        Dim errorMsg As String = ex.Message
        If errorMsg.Contains(InnerExceptionMessage) Then
            Dim innerExMessage As String = ex.InnerException.Message
            errorMsg = If(innerExMessage.Contains(InnerExceptionMessage),
                          DecodeException(ex.InnerException),
                          $"{errorMsg.Replace(InnerExceptionMessage, SentenceSeparator)}{vbCrLf}{innerExMessage}"
                         )
        End If

        Return errorMsg
    End Function

End Module
