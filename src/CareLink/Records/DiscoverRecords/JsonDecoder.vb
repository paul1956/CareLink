' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json

Public Class JsonDecoder
    Private Shared ReadOnly s_options As New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
    Public Shared Function DownloadAndDecodeJson(Of T)(url As String) As T

        Dim jsonContent As String = JsonDownloader.DownloadJson(url)
        Return JsonSerializer.Deserialize(Of T)(jsonContent, s_options)
    End Function
End Class
