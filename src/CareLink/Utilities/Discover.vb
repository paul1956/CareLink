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
    ''' <param name="discoveryElement">
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
    Private Function GetConfigJson(country As String, discoveryElement As JsonElement) As JsonElement
        Dim config As JsonElement
        Dim region As JsonElement
        Dim arrayEnumerator As JsonElement.ArrayEnumerator =
            discoveryElement.GetProperty(propertyName:="supportedCountries").EnumerateArray()

        For Each c As JsonElement In arrayEnumerator
            If c.TryGetProperty(propertyName:=country.ToUpper(), value:=region) Then
                Exit For
            End If
        Next
        Dim message As String
        If region.IsNullOrUndefined Then
            message = $"ERROR: country code {country} is not supported"
            Throw New ApplicationException(message)
        End If
        Debug.WriteLine(message:=$"   region: {region}")
        Dim json As String = JsonSerializer.Serialize(value:=region)
        Dim countryInfo As CountryInfo = JsonSerializer.Deserialize(Of CountryInfo)(json)
        For Each value As JsonElement In discoveryElement.GetProperty(propertyName:="CP").EnumerateArray()
            Try
                Dim json1 As String = JsonSerializer.Serialize(value)
                Dim cpInfo As CPInfo = JsonSerializer.Deserialize(Of CPInfo)(json:=json1)
                If countryInfo.Region = cpInfo.Region Then
                    config = value
                    Exit For
                End If
            Catch ex As Exception
                ' ignore here error will be handled outside the loop
            End Try
        Next
        If config.IsNullOrUndefined Then
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
    ''' <param name="discoveryUrl"></param>
    ''' <returns>
    '''  A <see cref="JsonElement"/> containing the configuration for the
    '''  specified country, including a computed token URL.
    ''' </returns>
    ''' <exception cref="Exception">
    '''  Thrown if the country code is not supported or if configuration
    '''  data cannot be retrieved.
    ''' </exception>
    ''' <param name="country">The country code to retrieve configuration for.</param>
    Public Function GetConfig(httpClient As HttpClient, discoveryUrl As String, country As String) As JsonElement
        Debug.WriteLine(NameOf(GetConfig))

        Dim json As String = httpClient.GetStringAsync(requestUri:=discoveryUrl).Result
        Dim discoveryElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json)
        Dim configJson As JsonElement = GetConfigJson(country, discoveryElement)

        Dim ssoConfigurationKey As String = configJson.GetProperty(propertyName:="UseSSOConfiguration").GetString()
        Dim resp As String =
            httpClient.GetStringAsync(configJson.GetProperty(propertyName:=ssoConfigurationKey).GetString()).Result
        Dim ssoConfig As SsoConfig = JsonSerializer.Deserialize(Of SsoConfig)(resp)

        Dim hostname As String = ssoConfig.Server.Hostname
        Dim ssoBaseUrl As String = $"https://{hostname}:{ssoConfig.Server.Port}/{ssoConfig.Server.Prefix}"
        If ssoBaseUrl.EndsWith("/"c) Then
            ssoBaseUrl = ssoBaseUrl.TrimEnd("/"c)
        End If
        Dim tokenUrl As String = $"{ssoBaseUrl}{ssoConfig.OAuth.UserInfoEndpointPath}"

        json = configJson.GetRawText()
        Dim mutableConfig As Dictionary(Of String, JsonElement) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(json)
        mutableConfig("token_url") = JsonSerializer.Deserialize(Of JsonElement)(json:=$"{Quote}{tokenUrl}{Quote}")
        json = JsonSerializer.Serialize(value:=mutableConfig, options:=s_jsonSerializerOptions)
        Return JsonSerializer.Deserialize(Of JsonElement)(json)
    End Function

    ''' <summary>
    ''' Downloads and decodes the discovery configuration data for a given country,
    ''' capturing the HTTP status code and any error messages.
    ''' </summary>
    ''' <param name="lastErrorMsg">Output parameter to receive the last error message if any.</param>
    ''' <param name="httpStatusCode">Output parameter to receive the HTTP status code of the response.</param>
    ''' <returns>
    ''' A <see cref="DiscoveryRecord"/> containing the configuration data for the specified country,
    ''' or <see langword="Nothing"/> if an error occurs.
    ''' </returns>
    Public Function GetDiscoveryData(ByRef lastErrorMsg As String, ByRef httpStatusCode As Integer) As DiscoveryRecord
        Dim discoveryUrl As String = If(s_countryCode.EqualsNoCase("US"),
                                        s_discoverUrl(key:="US"),
                                        s_discoverUrl(key:="EU"))
        httpStatusCode = 0 ' Default value meaning no response received yet
        Try
            Using client As New HttpClient()
                Dim response As HttpResponseMessage = client.GetAsync(discoveryUrl).Result
                httpStatusCode = CType(response.StatusCode, Integer)

                ' Use centralized response inspection to ensure common statuses are surfaced.
                Try
                    response.ThrowIfFailure()
                Catch uaEx As UnauthorizedAccessException
                    lastErrorMsg = $"Unauthorized access when fetching discovery data: {uaEx.Message}"
                    Debug.WriteLine(lastErrorMsg)
                    Return Nothing
                Catch argEx As ArgumentException
                    lastErrorMsg = $"Bad request fetching discovery data: {argEx.Message}"
                    Debug.WriteLine(lastErrorMsg)
                    Return Nothing
                Catch httpEx As HttpRequestException
                    lastErrorMsg = $"HTTP request failed: {httpEx.Message}"
                    Debug.WriteLine(lastErrorMsg)
                    Return Nothing
                End Try

                Dim json As String = response.Content.ReadAsStringAsync().Result
                Dim result As DiscoveryRecord
                Try
                    result = JsonSerializer.Deserialize(Of DiscoveryRecord)(json, options:=s_jsonDesterilizeOptions)
                Catch ex As Exception
                    Stop
                    Throw
                End Try
                Return result
            End Using

        Catch ex As AggregateException
            ' AggregateException is common for .Result on async methods when faulted
            Dim messages As New List(Of String)

            If ex.InnerExceptions.Count = 1 Then
                lastErrorMsg = ex.InnerExceptions(0).Message
                If lastErrorMsg.Contains("No such host is known") Then
                    httpStatusCode = 1
                End If
            Else
                For Each innerEx As Exception In ex.InnerExceptions
                    messages.Add(innerEx.Message)
                Next
                lastErrorMsg = $"Multiple errors: {String.Join("; ", messages)}"
            End If
            Debug.WriteLine(lastErrorMsg)

        Catch ex As HttpRequestException
            lastErrorMsg = $"HTTP request error: {ex.Message}"
            Debug.WriteLine(lastErrorMsg)

        Catch ex As TaskCanceledException
            lastErrorMsg = "The request timed out."
            Debug.WriteLine(lastErrorMsg)

        Catch ex As JsonException
            lastErrorMsg = $"JSON deserialization error: {ex.Message}"
            Debug.WriteLine(lastErrorMsg)

        Catch ex As Exception
            lastErrorMsg = $"Unexpected error: {ex.Message}"
            Debug.WriteLine(lastErrorMsg)
            Stop
        End Try

        Return Nothing
    End Function

End Module
