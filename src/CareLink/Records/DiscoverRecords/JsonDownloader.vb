' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http

Public Class JsonDownloader
    Public Shared Function DownloadJson(url As String) As String
        Using client As New HttpClient()
            Return client.GetStringAsync(url).Result
        End Using
    End Function
End Class
