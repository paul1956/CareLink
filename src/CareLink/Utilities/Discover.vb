' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Text.Json
Imports WebView2.DevTools.Dom

Public Module Discover
    Private ReadOnly s_discoverUrl As String = "https://clcloud.minimed.com/connect/carepartner/v11/discover/android/3.2"

    Private Function GetConfigJson(country As String, jsonElementData As JsonElement) As JsonElement
        Dim config As JsonElement
        Dim region As JsonElement
        For Each c As JsonElement In jsonElementData.GetProperty("supportedCountries").EnumerateArray()
            If c.TryGetProperty(country.ToUpper(), region) Then
                Exit For
            End If
        Next

        If region.ValueKind.IsNullOrUndefined Then
            Throw New Exception($"ERROR: country code {country} is not supported")
        End If
        Debug.WriteLine($"   region: {region}")
        Dim countryInfo As CountryInfo = JsonSerializer.Deserialize(Of CountryInfo)(JsonSerializer.Serialize(region))
        Try
            For Each c As JsonElement In jsonElementData.GetProperty("CP").EnumerateArray
                Dim cpInfo As CPInfo = JsonSerializer.Deserialize(Of CPInfo)(JsonSerializer.Serialize(c))
                If countryInfo.Region = cpInfo.Region Then
                    config = c
                    Exit For
                End If
            Next
        Catch ex As Exception
            Stop
        End Try
        If config.ValueKind.IsNullOrUndefined Then
            Throw New Exception($"ERROR: failed to get config base URLs for region {region}")
        End If
        Return config
    End Function

    Public Function GetConfigElement(httpClient As HttpClient, discoveryUrl As String, country As String) As JsonElement
        Debug.WriteLine(NameOf(GetConfigElement))
        Dim response As String = httpClient.GetStringAsync(discoveryUrl).Result
        Dim jsonElementData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(response)
        Dim configurationData As ConfigRecord = JsonSerializer.Deserialize(Of ConfigRecord)(response)
        Dim configJson As JsonElement = GetConfigJson(country, jsonElementData)

        Dim ssoConfigResponse As String = httpClient.GetStringAsync(configJson.GetProperty("SSOConfiguration").GetString()).Result
        Dim ssoConfig As SsoConfig = JsonSerializer.Deserialize(Of SsoConfig)(ssoConfigResponse)
        Dim ssoBaseUrl As String = $"https://{ssoConfig.Server.Hostname}:{ssoConfig.Server.Port}/{ssoConfig.Server.Prefix}"
        Dim tokenUrl As String = $"{ssoBaseUrl}{ssoConfig.OAuth.SystemEndpoints.TokenEndpointPath}"

        Dim mutableConfig As Dictionary(Of String, JsonElement) = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(configJson.GetRawText())
        mutableConfig("token_url") = JsonSerializer.Deserialize(Of JsonElement)($"""{tokenUrl}""")
        Return JsonSerializer.Deserialize(Of JsonElement)(JsonSerializer.Serialize(mutableConfig, s_jsonSerializerOptions))
    End Function

    Public Function GetDiscoveryData() As ConfigRecord
        Try
            Return DownloadAndDecodeJson(Of ConfigRecord)(s_discoverUrl)
        Catch ex As HttpRequestException
            Debug.WriteLine($"Error downloading JSON: {ex.Message}")
        Catch ex As JsonException
            Debug.WriteLine($"Error decoding JSON: {ex.Message}")
        End Try
        Return Nothing
    End Function

End Module
