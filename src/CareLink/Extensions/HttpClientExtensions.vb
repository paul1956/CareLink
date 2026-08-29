' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for the <see cref="HttpClient"/> class
'''  to simplify HTTP requests and response handling,
'''  including setting default headers, sending GET and POST requests, and
'''  retrieving response content as text or JSON.
''' </summary>
Friend Module HttpClientExtensions

    ''' <summary>
    '''  Sets the default request headers for the specified <see cref="HttpClient"/>
    '''  instance using the common headers defined in <see cref="s_common_Headers"/>.
    ''' </summary>
    ''' <param name="httpClient">The <see cref="HttpClient"/> instance to configure.</param>
    <Extension>
    Public Sub SetDefaultRequestHeaders(ByRef httpClient As HttpClient)
        httpClient.DefaultRequestHeaders.Clear()
        For Each header As KeyValuePair(Of String, String) In s_common_Headers.Sort
            httpClient.DefaultRequestHeaders.Add(name:=header.Key, header.Value)
        Next
    End Sub

End Module
