' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Text.Json

''' <summary>
'''  Provides methods for discovering and retrieving configuration data
'''  for supported countries and regions.
''' </summary>
Public Module Discover

    ''' <summary>
    '''  Retrieves the configuration JSON element for a specific country
    '''  from the provided JSON data.
    ''' </summary>
    ''' <param name="country">The country code to look up.</param>
    ''' <param name="jsonElementData">
    '''  The root JSON element containing supported countries and configuration data.
    ''' </param>
    ''' <returns>
    '''  The <see cref="JsonElement"/> representing the configuration for
    '''  the specified country.
    ''' </returns>
    ''' <exception cref="Exception">
    '''  Thrown if the country code is not supported or if the configuration
    '''  cannot be found.
    ''' </exception>
    Private Function GetConfigJson(
        country As String,
        jsonElementData As JsonElement) As JsonElement

        Dim config As JsonElement
        Dim region As JsonElement
        Dim arrayEnumerator As JsonElement.ArrayEnumerator =
            jsonElementData.GetProperty(propertyName:="supportedCountries").EnumerateArray()

        For Each c As JsonElement In arrayEnumerator
            If c.TryGetProperty(propertyName:=country.ToUpper(), value:=region) Then
                Exit For
            End If
        Next
        Dim message As String
        If region.ValueKind.IsNullOrUndefined Then
            message = $"ERROR: country code {country} is not supported"
            Throw New ApplicationException(message)
        End If
        Debug.WriteLine($"   region: {region}")
        Dim json As String = JsonSerializer.Serialize(value:=region)
        Dim countryInfo As CountryInfo =
            JsonSerializer.Deserialize(Of CountryInfo)(json)
        For Each value As JsonElement In
            jsonElementData.GetProperty(propertyName:="CP").EnumerateArray()

            Try
                Dim json1 As String = JsonSerializer.Serialize(value)
                Dim cpInfo As CPInfo =
                    JsonSerializer.Deserialize(Of CPInfo)(json:=json1)
                If countryInfo.Region = cpInfo.Region Then
                    config = value
                    Exit For
                End If
            Catch ex As Exception
                ' ignore here error will be handled outside the loop
            End Try
        Next
        If config.ValueKind.IsNullOrUndefined Then
            message = $"ERROR: failed to get config base URLs for region {region}"
            Throw New ApplicationException(message)
        End If
        Return config
    End Function

    ''' <summary>
    '''  Retrieves the configuration element for a given country using
    '''  the provided <see cref="HttpClient"/>.
    ''' </summary>
    ''' <param name="httpClient">
    '''  The <see cref="HttpClient"/> used to fetch configuration data.
    ''' </param>
    ''' <param name="country">The country code to retrieve configuration for.</param>
    ''' <returns>
    '''  A <see cref="JsonElement"/> containing the configuration for the
    '''  specified country, including a computed token URL.
    ''' </returns>
    ''' <exception cref="Exception">
    '''  Thrown if the country code is not supported or if configuration
    '''  data cannot be retrieved.
    ''' </exception>
    Public Function GetConfigElement(
        httpClient As HttpClient,
        country As String) As JsonElement

        Debug.WriteLine(NameOf(GetConfigElement))
        Dim isUsRegion As Boolean = country.EqualsNoCase(b:="US")
        Dim requestUri As String =
            If(isUsRegion,
               s_discoverUrl(key:="US"),
               s_discoverUrl(key:="EU"))
        Dim json As String = httpClient.GetStringAsync(requestUri).Result
        Dim jsonElementData As JsonElement =
            JsonSerializer.Deserialize(Of JsonElement)(json)
        Dim configurationData As ConfigRecord =
            JsonSerializer.Deserialize(Of ConfigRecord)(json)
        Dim configJson As JsonElement = GetConfigJson(country, jsonElementData)

        Dim requestUri1 As String =
            configJson.GetProperty(propertyName:="SSOConfiguration").GetString()
        Dim ssoConfigResponse As String =
            httpClient.GetStringAsync(requestUri:=requestUri1).Result
        Dim ssoConfig As SsoConfig =
            JsonSerializer.Deserialize(Of SsoConfig)(ssoConfigResponse)
        Dim hostname As String = ssoConfig.Server.Hostname
        Dim ssoBaseUrl As String =
            $"https://{hostname}:{ssoConfig.Server.Port}/{ssoConfig.Server.Prefix}"
        Dim tokenUrl As String =
            $"{ssoBaseUrl}{ssoConfig.OAuth.SystemEndpoints.TokenEndpointPath}"

        json = configJson.GetRawText()
        Dim mutableConfig As Dictionary(Of String, JsonElement) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(json)
        mutableConfig("token_url") =
            JsonSerializer.Deserialize(Of JsonElement)(json:=$"""{tokenUrl}""")
        Dim options As JsonSerializerOptions = s_jsonSerializerOptions
        json = JsonSerializer.Serialize(value:=mutableConfig, options)
        Return JsonSerializer.Deserialize(Of JsonElement)(json)
    End Function

    ''' <summary>
    '''  Downloads and decodes the discovery configuration data for a given country.
    ''' </summary>
    ''' <param name="country">The country code to retrieve discovery data for.</param>
    ''' <returns>
    '''  A <see cref="ConfigRecord"/> containing the configuration data
    '''  for the specified country,
    '''  or <see langword="Nothing"/> if an error occurs.
    ''' </returns>
    Public Function GetDiscoveryData() As ConfigRecord
        Try
            Dim region As String = If(s_countryCode.EqualsNoCase("US"), "US", "EU")
            Return DownloadAndDecodeJson(Of ConfigRecord)(s_discoverUrl(region))
        Catch ex As HttpRequestException
            Debug.WriteLine($"Error downloading JSON: {ex.Message}")
        Catch ex As JsonException
            Debug.WriteLine($"Error decoding JSON: {ex.Message}")
        End Try
        Return Nothing
    End Function

End Module
