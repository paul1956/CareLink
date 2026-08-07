' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http
Imports System.Runtime.CompilerServices

''' <summary>
'''  Centralized helpers for interpreting HttpResponseMessage results.
'''  Throws typed exceptions with diagnostic text (including response body when available).
''' </summary>
Friend Module HttpResponseExtensions

    <Extension>
    Friend Async Function ThrowIfFailureAsync(response As HttpResponseMessage) As Task
        ArgumentNullException.ThrowIfNull(response)

        If response.IsSuccessStatusCode Then
            Return
        End If

        Dim status As HttpStatusCode = response.StatusCode
        Dim body As String

        Try
            body = Await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:=False)
        Catch ex As Exception
            ' Could not read body — continue with empty body.
            body = $"<unable to read response body: {ex.Message}>"
        End Try

        Dim msg As String = $"HTTP {CInt(status)} {status}: {body}"

        ' Map common status codes to meaningful exceptions so callers can respond correctly.
        Select Case status
            Case HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden
                ' Auth problems: caller may attempt refresh or re-auth.
                Throw New UnauthorizedAccessException(msg)
            Case HttpStatusCode.BadRequest
                ' Client error: surface argument/validation-like error.
                Throw New ArgumentException(msg)
            Case HttpStatusCode.BadGateway,
                 HttpStatusCode.GatewayTimeout,
                 HttpStatusCode.RequestTimeout,
                 HttpStatusCode.ServiceUnavailable
                ' Transient server errors: caller can implement retry with backoff.
                Throw New HttpRequestException(msg)
            Case Else
                ' Generic HTTP failure
                Throw New HttpRequestException(msg)
        End Select
    End Function

    <Extension>
    Public Function GetValueOrNothing(response As HttpResponseMessage, key As String) As String
        ArgumentNullException.ThrowIfNull(argument:=response)
        ArgumentNullException.ThrowIfNull(argument:=key)
        If response.Headers.Contains(name:=key) Then
            Dim valueString As String = response.Headers.GetValues(name:=key).FirstOrDefault()
            If valueString IsNot Nothing Then
                Return valueString
            End If
        End If
        Return Nothing
    End Function
End Module
