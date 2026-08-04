' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Module LoginHelper

    ''' <summary>
    '''  Ensures local login data exists by invoking the embedded helper executable when
    '''  no <paramref name="tokenData"/> is supplied.
    ''' </summary>
    ''' <param name="serverRegion">
    '''  The server region to use when invoking the helper EXE.
    ''' </param>
    ''' <param name="tokenData">
    '''  The current token data. If <c>Nothing</c>, the method will extract an embedded helper EXE,
    '''  run it to produce a JSON file, and move that file to the configured login data destination.
    ''' </param>
    ''' <remarks>
    '''  The method performs file I/O and launches an external process. It:
    '''  - Extracts the embedded resource <c>carelink_carepartner_api_login</c> to a temporary EXE file.
    '''  - Executes the EXE (optionally with the <c>--us</c> switch) and waits for it to
    '''    create a temporary JSON output file.
    '''  - Moves the generated JSON to the destination returned by <see cref="GetLoginDataFileName"/>
    '''    (s_userName) and cleans up temporary files and the helper process.
    '''  Callers should avoid invoking this on the UI thread because it performs blocking I/O and process operations.
    ''' </remarks>
    ''' <exception cref="IOException">
    '''  Propagates I/O exceptions from writing, moving, or deleting files.
    ''' </exception>
    ''' 
    ''' 
    Public Async Function GetLoginData(serverRegion As Region, tokenData As TokenData) As Task
        If tokenData Is Nothing Then
            Try
                Dim discoveryUrl As String = If(serverRegion <> Region.Europe,
                                                CareLinkService.DiscoveryUrlNa,
                                                CareLinkService.DiscoveryUrlEu)
                Dim outputFile As String = GetLoginDataFileName()

                Dim endpointConfig As EndpointConfig =
                    Await CareLinkService.ResolveEndpointConfigAsync(discoveryUrl, serverRegion)

                Dim result As TokenData = Await CareLinkService.DoLoginAsync(endpointConfig, outputFile)

                'MessageBox.Show(text:=$"Login completed.{Environment.NewLine}Access token saved.",
                '                caption:="Success",
                '                buttons:=MessageBoxButtons.OK,
                '                icon:=MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(text:=ex.Message, caption:="Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
            End Try
        End If
    End Function

End Module
