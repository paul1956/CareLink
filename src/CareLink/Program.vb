' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.


Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports System.Net.Http
Imports Octokit

Friend Module Program
    Private ReadOnly s_options As New JsonSerializerOptions With {.WriteIndented = True}

    Private Const LoginDataFile As String = "logindata.json"
    Private Const DiscoveryUrl As String = "https://clcloud.minimed.eu/connect/carepartner/v11/discover/android/3.2"
    Private Const RsaKeySize As Integer = 2048

    Public Sub Main(args As String())
        Dim isUsRegion As Boolean = args.Contains("--us")
        MainAsync(isUsRegion).Wait()
    End Sub

    Public Async Function MainAsync(isUsRegion As Boolean) As Task
        SetupLogging()

        Dim tokenData As JsonElement? = ReadDataFile(LoginDataFile)

        If tokenData Is Nothing Then
            Console.WriteLine("Performing login...")
            Dim endpointConfig As (SsoConfig As JsonElement, ApiBaseUri As String) = Await ResolveEndpointConfigAsync(DiscoveryUrl, isUsRegion)
            tokenData = Await DoLogin(endpointConfig)
        Else
            Console.WriteLine("Token data file already exists")
        End If
    End Function

    Public Sub SetupLogging()
        ' VB.NET doesn't have a direct equivalent to Python's HTTPConnection.debugLevel
        ' You might need to use a different logging mechanism or library for HTTP debugging
        ' For now, we'll just set up basic console logging
        Console.SetError(Console.Out)
    End Sub

    Public Function RandomB64Str(length As Integer) As String
        Dim random As New Random()
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim result As New String(Enumerable.Repeat(chars, length + 10).Select(Function(s) s(random.Next(s.Length))).ToArray())
        Return Convert.ToBase64String(Encoding.UTF8.GetBytes(result)).Substring(0, length)
    End Function

    Public Function RandomUuid() As String
        Return Guid.NewGuid().ToString()
    End Function

    Public Function RandomAndroidModel() As String
        Dim models() As String = {"SM-G973F", "SM-G988U1", "SM-G981W", "SM-G9600"}
        Return models(New Random().Next(models.Length))
    End Function

    Public Function RandomDeviceId() As String
        Dim randomBytes(39) As Byte
        Using rng As RandomNumberGenerator = RandomNumberGenerator.Create
            rng.GetBytes(randomBytes)
        End Using
        Return Convert.ToHexStringLower(SHA256.HashData(randomBytes))
    End Function

    ' Note: The CreateCsr function is omitted as it requires OpenSSL, which is not natively available in .NET
    ' You might need to use a different library or approach for CSR creation in VB.NET

    Public Function ReformatCsr(csr As String) As String
        csr = csr.Replace(vbCrLf, "").Replace("-----BEGIN CERTIFICATE REQUEST-----", "").Replace("-----END CERTIFICATE REQUEST-----", "")
        Dim csrRaw() As Byte = Convert.FromBase64String(csr)
        Return Convert.ToBase64String(csrRaw).Replace("+", "-").Replace("/", "_").Replace("=", "")
    End Function

    ' Note: The DoCaptcha function is omitted as it requires Selenium, which is not natively available in .NET
    ' You might need to use a different library or approach for browser automation in VB.NET

    Public Async Function ResolveEndpointConfigAsync(discoveryUrl As String, isUsRegion As Boolean) As Task(Of (SsoConfig As JsonElement, ApiBaseUrl As String))
        Using client As New HttpClient()
            Dim discoverResp As String = Await client.GetStringAsync(discoveryUrl)
            Dim discoverJson As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(discoverResp)

            Dim ssoUrl As String = Nothing
            For Each c As JsonElement In discoverJson.GetProperty("CP").EnumerateArray()
                If (c.GetProperty("region").GetString().Equals("us", StringComparison.CurrentCultureIgnoreCase) AndAlso isUsRegion) OrElse
                   (c.GetProperty("region").GetString().Equals("eu", StringComparison.CurrentCultureIgnoreCase) AndAlso Not isUsRegion) Then
                    ssoUrl = c.GetProperty("SSOConfiguration").GetString()
                    Exit For
                End If
            Next

            If ssoUrl Is Nothing Then
                Throw New Exception("Could not get SSO config url")
            End If

            Dim ssoConfig As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(Await client.GetStringAsync(ssoUrl))
            Dim apiBaseUrl As String = $"https://{ssoConfig.GetProperty("server").GetProperty("hostname").GetString()}:{ssoConfig.GetProperty("server").GetProperty("port").GetString()}/{ssoConfig.GetProperty("server").GetProperty("prefix").GetString()}"

            Return (ssoConfig, apiBaseUrl)
        End Using
    End Function

    Public Sub WriteDataFile(obj As Object, filename As String)
        Console.WriteLine("Wrote data file")
        File.WriteAllText(filename, JsonSerializer.Serialize(obj, s_options))
    End Sub

    Public Async Function DoLogin(endpointConfig As (SsoConfig As Object, ApiBaseUrl As String)) As Task(Of Dictionary(Of String, String))
        Dim ssoConfig As Object = endpointConfig.SsoConfig
        Dim apiBaseUrl As String = endpointConfig.ApiBaseUrl

        ' Step 1: Initialize
        Dim data As New Dictionary(Of String, String) From {
            {"client_id", ssoConfig("oauth")("client")("client_ids")(0)("client_id")},
            {"nonce", Guid.NewGuid().ToString()}
        }

        Dim headers As New Dictionary(Of String, String) From {
            {"device-id", Convert.ToBase64String(Encoding.UTF8.GetBytes(RandomDeviceId()))}
        }

        Dim clientInitUrl As String = apiBaseUrl & ssoConfig("mag")("system_endpoints")("client_credential_init_endpoint_path")
        Dim clientInitResponse As String = Await PostRequestAsync(clientInitUrl, data, headers)
        Dim clientInitResponseObj As Dictionary(Of String, JsonElement) = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(clientInitResponse)

        ' Step 2: Authorize
        Dim clientCodeVerifier As String = GenerateCodeVerifier()
        Dim clientCodeChallenge As String = GenerateCodeChallenge(clientCodeVerifier)

        Dim clientState As String = RandomBase64String(22)
        Dim authParams As New Dictionary(Of String, String) From {
            {"client_id", clientInitResponseObj("client_id").GetString()},
            {"response_type", "code"},
            {"display", "social_login"},
            {"scope", ssoConfig("oauth")("client")("client_ids")(0)("scope")},
            {"redirect_uri", ssoConfig("oauth")("client")("client_ids")(0)("redirect_uri")},
            {"code_challenge", clientCodeChallenge},
            {"code_challenge_method", "S256"},
            {"state", clientState}
        }

        Dim authorizeUrl As String = apiBaseUrl & ssoConfig("oauth")("system_endpoints")("authorization_endpoint_path")
        Dim providersResponse As String = Await GetRequestAsync(authorizeUrl, authParams)
        Dim providers As Dictionary(Of String, JsonElement) = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(providersResponse)
        Dim captchaUrl As Dictionary(Of String, String) = providers("providers")(0)("provider")("auth_url").GetString()

        ' Step 3: Captcha login and consent
        Console.WriteLine($"captcha url: {captchaUrl}")
        ' TODO: Implement DoCaptcha function
        ' Dim (captchaCode, captchaSsoState) = DoCaptcha(captchaUrl, ssoConfig("oauth")("client")("client_ids")(0)("redirect_uri"))
        ' Console.WriteLine($"sso state after captcha: {captchaSsoState}")

        ' Step 4: Registration
        Dim registerDeviceId As String = RandomDeviceId()
        Dim clientAuthStr As String = $"{clientInitResponseObj("client_id").GetString()}:{clientInitResponseObj("client_secret").GetString()}"

        Dim androidModel As String = RandomAndroidModel()
        Dim androidModelSafe As String = Regex.Replace(androidModel, "[^a-zA-Z0-9]", "")

        ' TODO: Implement CSR creation using BouncyCastle or other cryptography library
        ' Dim csr = CreateCSR(keypair, "socialLogin", registerDeviceId, androidModelSafe, ssoConfig("oauth")("client")("organization"))

        Dim regHeaders As New Dictionary(Of String, String) From {
            {"device-name", Convert.ToBase64String(Encoding.UTF8.GetBytes(androidModel))},
            {"authorization", $"Bearer {captchaCode}"},
            {"cert-format", "pem"},
            {"client-authorization", "Basic " & Convert.ToBase64String(Encoding.UTF8.GetBytes(clientAuthStr))},
            {"create-session", "true"},
            {"code-verifier", clientCodeVerifier},
            {"device-id", Convert.ToBase64String(Encoding.UTF8.GetBytes(registerDeviceId))},
            {"redirect-uri", ssoConfig("oauth")("client")("client_ids")(0)("redirect_uri")}
        }

        ' TODO: Implement ReformatCSR function
        ' csr = ReformatCSR(csr)

        Dim regUrl As String = apiBaseUrl & ssoConfig("mag")("system_endpoints")("device_register_endpoint_path")
        ' TODO: Implement PostRequestWithHeadersAsync function
        ' Dim regResponse As HttpResponseMessage = Await PostRequestWithHeadersAsync(regUrl, regHeaders, csr)

        ' If regResponse.StatusCode <> HttpStatusCode.OK Then
        '     Throw New Exception($"Could not register: {JsonSerializer.Deserialize(Of Dictionary(Of String, String))(Await regResponse.Content.ReadAsStringAsync())("error_description")}")
        ' End If

        ' Step 5: Token
        Dim tokenReqUrl As String = apiBaseUrl & ssoConfig("oauth")("system_endpoints")("token_endpoint_path")
        Dim tokenReqData As New Dictionary(Of String, String) From {
            {"assertion", regResponse.Headers.GetValues("id-token").FirstOrDefault()},
            {"client_id", clientInitResponseObj("client_id").GetString()},
            {"client_secret", clientInitResponseObj("client_secret").GetString()},
            {"scope", ssoConfig("oauth")("client")("client_ids")(0)("scope")},
            {"grant_type", regResponse.Headers.GetValues("id-token-type").FirstOrDefault()}
        }

        Dim tokenReqHeaders As New Dictionary(Of String, String) From {
            {"mag-identifier", regResponse.Headers.GetValues("mag-identifier").FirstOrDefault()}
        }

        Dim tokenResponse As String = Await PostRequestAsync(tokenReqUrl, tokenReqData, tokenReqHeaders)
        Dim tokenData As Dictionary(Of String, JsonElement) = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(tokenResponse)

        Console.WriteLine("got token data from server")

        tokenData("client_id") = tokenReqData("client_id")
        tokenData("client_secret") = tokenReqData("client_secret")
        tokenData.Remove("expires_in")
        tokenData.Remove("token_type")
        tokenData("mag-identifier") = regResponse.Headers.GetValues("mag-identifier").FirstOrDefault()

        ' TODO: Implement WriteDataFile function
        ' WriteDataFile(tokenData, logindata_file)

        Return tokenData
    End Function

    ' Helper functions (implement these as needed)

    Private Function GenerateCodeVerifier() As String
        ' Implementation needed
    End Function

    Private Function GenerateCodeChallenge(codeVerifier As String) As String
        ' Implementation needed
    End Function

    Private Async Function PostRequestAsync(url As String, data As Dictionary(Of String, String), headers As Dictionary(Of String, String)) As Task(Of String)
        ' Implementation needed
    End Function

    Private Async Function GetRequestAsync(url As String, parameters As Dictionary(Of String, String)) As Task(Of String)
        ' Implementation needed
    End Function

    Public Function ReadDataFile(fileWithPath As String) As JsonElement?
        If File.Exists(fileWithPath) Then
            Try
                Dim tokenData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(File.ReadAllText(fileWithPath))
                Dim requiredFields() As String = {
                    "access_token",
                    "refresh_token",
                    "scope",
                    "client_id",
                    "client_secret",
                    "mag-identifier"}

                For Each field As String In requiredFields
                    If Not tokenData.TryGetProperty(field, Nothing) Then
                        Console.WriteLine($"Field {field} is missing from data file")
                        Return Nothing
                    End If
                Next
                Return tokenData
            Catch ex As JsonException
                Console.WriteLine("Failed parsing JSON")
            End Try
        End If
        Return Nothing
    End Function
End Module
