' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports CareLink

Public Module CareLinkClientHelpers
    Private s_clientCodeVerifier As String

    Private Function DecodeResponse(response As HttpResponseMessage, ByRef lastErrorMessage As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As HttpResponseMessage
        Dim message As String
        If response?.IsSuccessStatusCode Then
            Dim resultText As String = response.ResultText
            lastErrorMessage = ExtractResponseData(resultText, "login_page.error.LoginFailed"">", "<")
            If lastErrorMessage = "" Then
                DebugPrint($"success from {memberName}, line {sourceLineNumber}.")
                Return response
            End If
            DebugPrint($"failed from {memberName}, line {sourceLineNumber}.")
            Return New HttpResponseMessage(HttpStatusCode.NetworkAuthenticationRequired)
        ElseIf response?.StatusCode = HttpStatusCode.BadRequest Then
            message = $"{NameOf(DecodeResponse)} failed with {HttpStatusCode.BadRequest}"
            lastErrorMessage = $"Login Failure {message}"
            DebugPrint($"{message} from {memberName}, line {sourceLineNumber}")
            Return response
        Else
            message = $"{NameOf(DecodeResponse)} failed, session response is {response?.StatusCode.ToString}"
            lastErrorMessage = message
            DebugPrint($"{message} from {memberName}, line {sourceLineNumber}")
            Return response
        End If
    End Function

    <Extension>
    Private Function ExtractResponseData(responseBody As String, startStr As String, endStr As String) As String
        If String.IsNullOrWhiteSpace(responseBody) Then
            Return ""
        End If

        Dim startIndex As Integer = responseBody.IndexOf(startStr, StringComparison.Ordinal)
        If startIndex = -1 Then
            Return ""
        End If
        startIndex += startStr.Length
        Dim endIndex As Integer = responseBody.IndexOf(endStr, startIndex, StringComparison.Ordinal)
        Return If(endIndex = -1,
                  "",
                  responseBody.Substring(startIndex, endIndex - startIndex).Replace("""", "")
                 )
    End Function

    Public Function GetAuthorizeUrlData(httpClient As HttpClient, SsoConfig As SsoConfig, ApiBaseUrl As String) As (String, clientInitResponse As ClientInitData)
        ' Step 1: Initialize
        Dim data As New Dictionary(Of String, String) From {
            {"client_id", SsoConfig.OAuth.Client.ClientIds(0).ClientId},
            {"nonce", RandomUuid()}
        }

        Dim clientInitUrl As String = $"{ApiBaseUrl}{SsoConfig.Mag.SystemEndpoints.ClientCredentialInitEndpointPath}"

        ' Add headers to the HttpClient

        ' Create the content for the POST request
        Dim content As New FormUrlEncodedContent(data)
        content.Headers.Add("device-id", Convert.ToBase64String(Encoding.UTF8.GetBytes(RandomDeviceId())))
        ' Send the POST request
        Dim response As HttpResponseMessage = httpClient.PostAsync(clientInitUrl, content).Result
        ' Read the response content
        Dim responseContent As String = response.Content.ReadAsStringAsync().Result

        ' Step 2: Authorize
        ' Generate client_code_verifier
        s_clientCodeVerifier = GenerateRandomBase64String(40)
        s_clientCodeVerifier = Regex.Replace(s_clientCodeVerifier, "[^a-zA-Z0-9]+", "")
        ' Generate client_code_challenge
        Dim challengeBytes As Byte() = SHA256.HashData(Encoding.UTF8.GetBytes(s_clientCodeVerifier))
        Dim clientCodeChallenge As String = Convert.ToBase64String(challengeBytes).Replace("+", "-").Replace("/", "_").TrimEnd("="c)
        Dim clientState As String = GenerateRandomBase64String(22)
        ' Deserialize the JSON response
        Dim clientInitResponse As ClientInitData = JsonSerializer.Deserialize(Of ClientInitData)(responseContent)
        Dim authParams As New Dictionary(Of String, String) From {
            {"client_id", clientInitResponse.client_id},
            {"response_type", "code"},
            {"display", "social_login"},
            {"scope", SsoConfig.OAuth.Client.ClientIds(0).Scope},
            {"redirect_uri", SsoConfig.OAuth.Client.ClientIds(0).RedirectUri},
            {"code_challenge", clientCodeChallenge},
            {"code_challenge_method", "S256"},
            {"state", clientState}
        }

        Dim authorizeUrl As String = $"{ApiBaseUrl}{SsoConfig.OAuth.SystemEndpoints.AuthorizationEndpointPath}"
        Dim providersResponse As JsonElement = httpClient.GetRequestAsync(authorizeUrl, authParams).Result
        Dim authorize As Authorize = JsonSerializer.Deserialize(Of Authorize)(providersResponse)
        Dim captchaUrl As String = authorize.Providers(0).Provider.AuthUrl

        Debug.WriteLine($"captcha url: {captchaUrl}")
        Return (captchaUrl, clientInitResponse)
    End Function

#If False Then

    Friend Sub DoLogin(httpClient As HttpClient, userName As String, isUsRegion As Boolean)
        Dim tokenData As TokenData = ReadTokenDataFile(userName)

        If tokenData IsNot Nothing Then
            Return
        End If

        Dim endpointConfig As (SsoConfig As SsoConfig, ApiBaseUrl As String) = Nothing
        Dim authorizeUrl As String = Nothing
        Dim ssoConfig As SsoConfig = Nothing
        Dim authorizeUrlData As (authorizeUrl As String, clientInitData As ClientInitData) = Nothing
        Try
            Dim discoveryUrl As String = If(isUsRegion, s_discoverUrl("US"), s_discoverUrl("EU"))
            endpointConfig = ResolveEndpointConfigAsync(httpClient, discoveryUrl, isUsRegion)
            ssoConfig = endpointConfig.SsoConfig
            authorizeUrlData = GetAuthorizeUrlData(httpClient, ssoConfig, endpointConfig.ApiBaseUrl)
            authorizeUrl = authorizeUrlData.authorizeUrl
        Catch ex As Exception
            Stop
        End Try

        ' Step 3: Captcha login and consent
        Dim captcha As (captchaCode As String, captchaSsoState As String) = DoLoginWithCaptcha(authorizeUrl, ssoConfig.OAuth.Client.ClientIds(0).RedirectUri)
        Debug.WriteLine($"sso state after captcha: {captcha.captchaSsoState}")

        ' Step 4: Registration
        Dim registerDeviceId As String = RandomDeviceId()
        Dim clientAuthStr As String = $"{endpointConfig.SsoConfig.OAuth.Client.ClientIds(0).ClientId}:{authorizeUrlData.clientInitData.client_secret}"

        Dim androidModel As String = RandomAndroidModel()
        Dim androidModelSafe As String = Regex.Replace(androidModel, "[^a-zA-Z0-9]", "")

        ' Generate key pair
        Using rsa As RSA = RSA.Create(2048)
            ' Create CSR
            Dim csr As String = CreateCSR(rsa, "socialLogin", registerDeviceId, androidModelSafe, ssoConfig.OAuth.Client.Organization)


            ' Reformat CSR (implement this function as needed)
            csr = ReformatCsr(csr)

            ' Prepare URL
            Dim registrationUrl As String = $"{endpointConfig.ApiBaseUrl}{ssoConfig.Mag.SystemEndpoints.DeviceRegisterEndpointPath}"

            Dim registrationResponse As HttpResponseMessage = Nothing
            Try
                Dim registrationRequest As New HttpRequestMessage(HttpMethod.Post, registrationUrl)

                ' Prepare headers
                Dim registrationHeaders As New Dictionary(Of String, String) From {
                    {"device-name", Convert.ToBase64String(Encoding.UTF8.GetBytes(androidModel))},
                    {"authorization", $"Bearer {captcha.captchaCode}"},
                    {"cert-format", "pem"},
                    {"client-authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(clientAuthStr))}"},
                    {"create-session", "true"},
                    {"code-verifier", s_clientCodeVerifier},
                    {"device-id", Convert.ToBase64String(Encoding.UTF8.GetBytes(registerDeviceId))},
                    {"redirect-uri", ssoConfig.OAuth.Client.ClientIds(0).RedirectUri}
                }
                For Each header As KeyValuePair(Of String, String) In registrationHeaders
                    registrationRequest.Headers.Add(header.Key, header.Value)
                Next
                Dim content As New StringContent(csr, Encoding.UTF8, "application/x-www-form-urlencoded")
                registrationRequest.Content = content
                ' Send POST request
                registrationResponse = httpClient.SendAsync(registrationRequest).Result
                registrationResponse.EnsureSuccessStatusCode()
            Catch ex As Exception
                Stop
                Return
            End Try
            If Not registrationResponse.IsSuccessStatusCode Then
                Dim errorContent As String = registrationResponse.Content.ReadAsStringAsync().Result
                Dim errorJson As JsonDocument = JsonDocument.Parse(errorContent)
                Dim errorDescription As String = errorJson.RootElement.GetProperty("error_description").GetString()
                Throw New Exception($"Could not register: {errorDescription}")
            End If

            If registrationResponse.StatusCode <> HttpStatusCode.OK Then
                Throw New Exception($"Could not register: {JsonSerializer.Deserialize(Of Dictionary(Of String, String))(registrationResponse.Content.ReadAsStringAsync().Result)("error_description")}")
            End If

            ' Step 5: Token
            Stop
            tokenData = GetTokenDataAsync(
                httpClient,
                endpointConfig.ApiBaseUrl,
                ssoConfig,
                regReq:=registrationResponse,
                clientInitResponse:=authorizeUrlData.clientInitData,
                userName).Result
        End Using

        Return
    End Sub
#End If

    Private Function DoLoginWithCaptcha(captchaUrl As String, redirectUri As String) As (captchaCode As String, captchaSsoState As String)
        Dim captchaWindow As New Captcha(s_countryCode, s_password, s_userName)
        Dim t As Task = captchaWindow.Captcha_Load()
        While t.IsCompleted = False
            Task.Delay(10).Wait()
            Application.DoEvents()
        End While
        t.Wait()
        Return captchaWindow.Execute(captchaUrl, redirectUri)
    End Function

    Private Async Function GetTokenDataAsync(
        httpClient As HttpClient,
        apiBaseUrl As String,
        ssoConfig As SsoConfig,
        regReq As HttpResponseMessage,
        clientInitResponse As ClientInitData,
        userName As String) As Task(Of TokenData)

        Dim tokenReqUrl As String = $"{apiBaseUrl}{ssoConfig.OAuth.SystemEndpoints.TokenEndpointPath}"
        Dim tokenReqData As New Dictionary(Of String, String) From {
            {"assertion", regReq.Headers.GetValues("id-token").FirstOrDefault()},
            {"client_id", clientInitResponse.client_id},
            {"client_secret", clientInitResponse.client_secret},
            {"scope", ssoConfig.OAuth.Client.ClientIds(0).Scope},
            {"grant_type", regReq.Headers.GetValues("id-token-type").FirstOrDefault()}
        }

        Dim tokenReq As New HttpRequestMessage(HttpMethod.Post, tokenReqUrl)
        tokenReq.Headers.Add("mag-identifier", regReq.Headers.GetValues("mag-identifier").FirstOrDefault())
        tokenReq.Content = New FormUrlEncodedContent(tokenReqData)

        Dim tokenResp As HttpResponseMessage = Await httpClient.SendAsync(tokenReq)

        If Not tokenResp.IsSuccessStatusCode Then
            'Debug.WriteLine($"{Environment.NewLine}{Environment.NewLine}{ToCurl(tokenReq)}")
            Throw New Exception("Could not get token data")
        End If

        Dim tokenDataStr As String = Await tokenResp.Content.ReadAsStringAsync()
        'Dim tokenData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(tokenDataStr)
        Debug.WriteLine("got token data from server")

        Dim tokenDataToSave As TokenData = JsonSerializer.Deserialize(Of TokenData)(tokenDataStr)

        WriteTokenDataFile(tokenDataToSave, userName)
        Return tokenDataToSave
    End Function

    Friend Function NetworkUnavailable() As Boolean
        Return Not My.Computer.Network.IsAvailable
    End Function

    Public Function ResolveEndpointConfigAsync(httpClient As HttpClient, discoveryUrl As String, isUsRegion As Boolean) As (SsoConfig As SsoConfig, ApiBaseUrl As String)
        Dim discoverResp As String = httpClient.GetStringAsync(discoveryUrl).Result
        Dim discover As ConfigRecord = JsonSerializer.Deserialize(Of ConfigRecord)(discoverResp, s_jsonDeserializerOptions)

        Dim ssoUrl As String = Nothing
        For Each cp As CPInfo In discover.CP
            If (cp.Region.Equals("us", StringComparison.CurrentCultureIgnoreCase) AndAlso isUsRegion) OrElse
                   (cp.Region.Equals("eu", StringComparison.CurrentCultureIgnoreCase) AndAlso Not isUsRegion) Then
                ssoUrl = cp.SSOConfiguration
                Exit For
            End If
        Next

        If ssoUrl Is Nothing Then
            Throw New Exception("Could not get SSO config url")
        End If

        Dim jsonString As String = httpClient.GetStringAsync(ssoUrl).Result
        Dim ssoConfig As SsoConfig = JsonSerializer.Deserialize(Of SsoConfig)(jsonString)
        Dim apiBaseUrl As String = $"https://{ssoConfig.Server.Hostname}:{ssoConfig.Server.Port}/{ssoConfig.Server.Prefix}"

        Return (ssoConfig, apiBaseUrl)
    End Function

End Module
