' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Class Discover
    Private Shared ReadOnly s_discoverUrl As String = "https://clcloud.minimed.com/connect/carepartner/v11/discover/android/3.2"

    <JsonPropertyName("config")>
    Public Property Config As String

    <JsonPropertyName("supportedCountries")>
    Public Property SupportedCountries As List(Of Dictionary(Of String, CountryInfo))
    <JsonPropertyName("CP")>
    Public Property CP As List(Of CPInfo)

    <JsonPropertyName("certificates")>
    Public Property Certificates As List(Of CertificateInfo)

    Public Shared Function GetDiscoveryData() As Discover
        Try
            Return DownloadAndDecodeJson(Of Discover)(s_discoverUrl)
        Catch ex As HttpRequestException
            Console.WriteLine($"Error downloading JSON: {ex.Message}")
        Catch ex As JsonException
            Console.WriteLine($"Error decoding JSON: {ex.Message}")
        End Try
        Return Nothing
    End Function
End Class
