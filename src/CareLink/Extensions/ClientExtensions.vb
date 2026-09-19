' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module ClientExtensions

    ''' <summary>
    '''  Validates the access token based on its expiration time.
    ''' </summary>
    ''' <param name="client2">
    '''  The client instance containing the access token.
    ''' </param>
    ''' <param name="message">
    '''  Output message describing the validation result.
    ''' </param>
    ''' <param name="log">
    '''  Indicates whether to log/update a short form of the message.
    ''' </param>
    ''' <returns>
    '''  True if the token is valid (not expired and not about to expire); otherwise False.
    ''' </returns>
    <Extension>
    Public Function IsTokenValid(client2 As Client2,
                                 ByRef message As String,
                                 Optional log As Boolean = True) As Boolean
        Dim startKey As String
        If client2.AccessTokenPayload Is Nothing Then
            startKey = "AccessToken Empty"
            If log Then
                UpdateMessage(message:=startKey, startKey)
            End If
            Return False
        End If
        Try
            Dim unixTime As Long = client2.AccessTokenPayload(key:="exp").GetInt64()
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiffSeconds As Long = unixTime - unixCurrentTime
            Dim absDiffMinutes As Long = Math.Abs(value:=tDiffSeconds \ 60)
            If tDiffSeconds <= 0 Then
                startKey = $"Access token has expired"
                message = $"{startKey} {absDiffMinutes.ToHoursMinutes} ago."
                If log Then
                    UpdateMessage(message, startKey)
                End If
                Return False
            End If
            If tDiffSeconds < 600 Then
                startKey = $"Access token is about to expire in"
                message = $"{startKey} {absDiffMinutes.ToHoursMinutes}."
                If log Then
                    UpdateMessage(message, startKey)
                End If
                Return False
            End If

            Dim utcTime As DateTimeOffset =
                DateTimeOffset.FromUnixTimeSeconds(seconds:=unixTime)
            ' Convert to local time
            Dim localTime As DateTimeOffset = utcTime.ToLocalTime()
            Dim formatted As String =
                localTime.ToString(format:="M/d/yyyy h:mm tt",
                                   formatProvider:=CultureInfo.InvariantCulture)

            startKey = $"Access token expires in"
            message = $"{startKey} {absDiffMinutes.ToHoursMinutes()} at {formatted}"
            ' Valid token: return True.
            If log Then
                UpdateMessage(message, startKey)
            End If
            Return True
        Catch ex As Exception
            message &= $"missing nameValueCollection in access token. {ex.DecodeException()}"
            LogMessage(message)
            Return False
        End Try
    End Function

End Module
