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
    ''' <param name="log">
    '''  Indicates whether to use a short form of the message.
    ''' </param>
    ''' <returns>
    '''  A string message indicating the validation result. Returns Nothing if the token is valid.
    ''' </returns>
    <Extension>
    Public Function IsTokenValid(client2 As Client2, Optional log As Boolean = True) As String
        Dim message As String = If(log,
                                   $"In {NameOf(IsTokenValid)} ",
                                   String.Empty)

        Dim startKey As String
        If client2.AccessTokenPayload Is Nothing Then
            startKey = "AccessToken Empty"
            message &= startKey
            If log Then
                UpdateMessage(message, startKey)
            End If
            Return message
        End If
        Try
            Dim unixTime As Long = client2.AccessTokenPayload(key:="exp").GetInt64()
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiffSeconds As Long = unixTime - unixCurrentTime
            Dim absDiffMinutes As Long = Math.Abs(value:=tDiffSeconds \ 60)
            If tDiffSeconds <= 0 Then
                startKey = $"access token has expired "
                message &= $"access token has expired {absDiffMinutes.ToHoursMinutes} ago"
                If log Then
                    UpdateMessage(message, startKey)
                End If
                Return message
            End If
            If tDiffSeconds < 600 Then
                startKey = $"access token is about to expire in "
                message &= $"access token is about to expire in {absDiffMinutes.ToHoursMinutes}"
                If log Then
                    UpdateMessage(message, startKey)
                End If
                Return message
            End If

            Dim utcTime As DateTimeOffset =
                DateTimeOffset.FromUnixTimeSeconds(seconds:=unixTime)
            ' Convert to local time
            Dim localTime As DateTimeOffset = utcTime.ToLocalTime()
            Dim formatted As String =
                localTime.ToString(format:="M/d/yyyy h:mm tt",
                                   formatProvider:=CultureInfo.InvariantCulture)

            startKey = $"Access token expires in "
            message &= $"{startKey}{absDiffMinutes.ToHoursMinutes()} at {formatted}"
            ' Valid token: return String.Empty to indicate success;
            ' For logging callers treat empty as success;
            ' When used for DisplayMessage we always want text.
            Return If(Not log,
                      message,
                      String.Empty)
        Catch ex As Exception
            message &=
                $"missing nameValueCollection in access token. {ex.DecodeException()}"
            LogMessage(message)
            Return message
        End Try
    End Function

End Module
