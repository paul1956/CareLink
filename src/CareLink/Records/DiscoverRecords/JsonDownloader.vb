' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Text.Json

Public Module JsonDownloader

    Public Function DownloadAndDecodeJson(Of T)(url As String) As T

        Dim jsonContent As String = DownloadJson(url)
        Return JsonSerializer.Deserialize(Of T)(jsonContent, s_jsonDeserializerOptions)
    End Function

    Private Function DownloadJson(url As String) As String
        Using client As New HttpClient()
            Return client.GetStringAsync(url).Result
        End Using
    End Function
End Module
