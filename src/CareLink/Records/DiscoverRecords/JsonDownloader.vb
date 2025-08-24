' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Text.Json

Public Module JsonDownloader

    ''' <summary>
    '''  Downloads JSON content from the specified URL.
    '''  This method uses HttpClient to perform the GET request and
    '''  returns the JSON content as a string.
    ''' </summary>
    ''' <param name="url">The URL from which to download the JSON content.</param>
    ''' <returns>A string containing the JSON content.</returns>
    ''' <remarks>This method is synchronous and blocks until the request completes.</remarks>
    ''' <exception cref="HttpRequestException">Thrown when the request fails.</exception>
    ''' <exception cref="TaskCanceledException">Thrown if the request times out.</exception>
    ''' <exception cref="JsonException">Thrown if the response is not valid JSON.</exception>
    Private Function DownloadJson(url As String) As String
        Using client As New HttpClient()
            Return client.GetStringAsync(url).Result
        End Using
    End Function

    ''' <summary>
    '''  Downloads JSON content from the specified URL and deserializes it into
    '''  an object of type T.
    '''  This method uses the <see cref="JsonSerializer"/> to convert the JSON string
    '''  into the specified type.
    ''' </summary>
    ''' <typeparam name="T">
    '''  The type to which the JSON content will be deserialized.
    ''' </typeparam>
    ''' <param name="url">The URL from which to download the JSON content.</param>
    ''' <returns>An object of type T containing the deserialized JSON data.</returns>
    ''' <remarks>
    '''  This method uses the static s_jsonDeserializerOptions for deserialization.
    ''' </remarks>
    ''' <exception cref="HttpRequestException">Thrown when the request fails.</exception>
    ''' <exception cref="JsonException">Thrown when deserialization fails.</exception
    Public Function DownloadAndDecodeJson(Of T)(url As String) As T
        Dim json As String = DownloadJson(url)
        Return JsonSerializer.Deserialize(Of T)(json, options:=s_jsonDeserializerOptions)
    End Function

End Module
