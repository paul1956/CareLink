' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Text.Json

''' <summary>
'''  Provides methods for discovering and retrieving configuration data for supported countries and regions.
''' </summary>
Public Module Discover

    ''' <summary>
    '''  Retrieves the configuration JSON element for a specific country from the provided JSON data.
    ''' </summary>
    ''' <param name="country">The country code to look up.</param>
    ''' <param name="jsonElementData">The root JSON element containing supported countries and configuration data.</param>
    ''' <returns>
    '''  The <see cref="JsonElement"/> representing the configuration for the specified country.
    ''' </returns>
    ''' <exception cref="Exception">
    '''  Thrown if the country code is not supported or if the configuration cannot be found.
    ''' </exception>
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
        For Each c As JsonElement In jsonElementData.GetProperty("CP").EnumerateArray
            Try
                Dim cpInfo As CPInfo = JsonSerializer.Deserialize(Of CPInfo)(JsonSerializer.Serialize(c))
                If countryInfo.Region = cpInfo.Region Then
                    config = c
                    Exit For
                End If
            Catch ex As Exception
                ' ignore here error will be handled outside the loop
            End Try
        Next
        If config.ValueKind.IsNullOrUndefined Then
            Throw New Exception($"ERROR: failed to get config base URLs for region {region}")
        End If
        Return config
    End Function

    ''' <summary>
    '''  Retrieves the configuration element for a given country using the provided <see cref="HttpClient"/>.
    ''' </summary>
    ''' <param name="httpClient">The <see cref="HttpClient"/> used to fetch configuration data.</param>
    ''' <param name="country">The country code to retrieve configuration for.</param>
    ''' <returns>
    '''  A <see cref="JsonElement"/> containing the configuration for the specified country, including a computed token URL.
    ''' </returns>
    ''' <exception cref="Exception">
    '''  Thrown if the country code is not supported or if configuration data cannot be retrieved.
    ''' </exception>
    Public Function GetConfigElement(httpClient As HttpClient, country As String) As JsonElement
        Debug.WriteLine(NameOf(GetConfigElement))
        Dim isUsRegion As Boolean = country.Equals("US", StringComparison.OrdinalIgnoreCase)
        Dim discoveryUrl As String = If(isUsRegion, s_discoverUrl("US"), s_discoverUrl("EU"))
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

    ''' <summary>
    '''  Downloads and decodes the discovery configuration data for a given country.
    ''' </summary>
    ''' <param name="country">The country code to retrieve discovery data for.</param>
    ''' <returns>
    '''  A <see cref="ConfigRecord"/> containing the configuration data for the specified country,
    '''  or <see langword="Nothing"/> if an error occurs.
    ''' </returns>
    Public Function GetDiscoveryData() As ConfigRecord
        Try
            Dim region As String = If(s_countryCode.Equals("US", StringComparison.OrdinalIgnoreCase),
                                      "US",
                                      "EU")
            Return DownloadAndDecodeJson(Of ConfigRecord)(s_discoverUrl(region))
        Catch ex As HttpRequestException
            Debug.WriteLine($"Error downloading JSON: {ex.Message}")
        Catch ex As JsonException
            Debug.WriteLine($"Error decoding JSON: {ex.Message}")
        End Try
        Return Nothing
    End Function

End Module
