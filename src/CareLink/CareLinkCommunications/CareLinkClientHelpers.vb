' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions

Imports CareLink
Imports Microsoft.Web.WebView2.Core.DevToolsProtocolExtension
Imports Octokit

Public Module CareLinkClientHelpers

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

    Private Function ParseQsl(loginSessionResponse As HttpResponseMessage) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)
        Dim absoluteUri As String = loginSessionResponse.RequestMessage.RequestUri.AbsoluteUri
        Dim splitAbsoluteUri() As String = absoluteUri.Split("&")
        For Each item As String In splitAbsoluteUri
            Dim splitItem() As String
            If result.Count = 0 Then
                item = item.Split("?")(1)
            End If
            splitItem = item.Split("=")
            result.Add(splitItem(0), splitItem(1))
        Next
        Return result
    End Function

    Friend Function DoConsent(ByRef httpClient As HttpClient, doLoginResponse As HttpResponseMessage, ByRef lastErrorMessage As String) As HttpResponseMessage

        ' Extract data for consent
        Dim doLoginRespBody As String = doLoginResponse.ResultText
        Dim url As New StringBuilder(doLoginRespBody.ExtractResponseData("<form action=", " "))
        Dim sessionId As String = doLoginRespBody.ExtractResponseData("<input type=""hidden"" name=""sessionID"" value=", "/>")
        Dim sessionData As String = doLoginRespBody.ExtractResponseData("<input type=""hidden"" name=""sessionData"" value=", "/>")
        lastErrorMessage = doLoginRespBody.ExtractResponseData("LoginFailed"">", "</p>")

        ' Send consent
        Dim form As New Dictionary(Of String, String) From {
            {"action", "consent"},
            {"sessionID", sessionId},
            {"sessionData", sessionData},
            {"response_type", "code"},
            {"response_mode", "query"}}
        ' Add header
        Dim consentHeaders As Dictionary(Of String, String) = s_commonHeaders.Clone()
        consentHeaders("Content-Type") = "application/x-www-form-urlencoded"

        Try
            Dim response As HttpResponseMessage = httpClient.Post(url, headers:=consentHeaders, data:=form)
            Return DecodeResponse(response, lastErrorMessage)
        Catch ex As Exception
            Dim message As String = $"failed with {ex.DecodeException()}"
            lastErrorMessage = message
            DebugPrint(message.Replace(vbCrLf, " "))
        End Try
        Return New HttpResponseMessage(HttpStatusCode.NotImplemented)
    End Function

    Public Function GetAuthorizeUrl(client As HttpClient, SsoConfig As RootConfig, ApiBaseUrl As String) As String
        ' Step 1: Initialize
        Dim data As New Dictionary(Of String, String) From {
            {"client_id", SsoConfig.OAuth.Client.ClientIds(0).ClientId},
            {"nonce", RandomUuid()}
        }

        Dim clientInitUrl As String = $"{ApiBaseUrl}{SsoConfig.Mag.SystemEndpoints.ClientCredentialInitEndpointPath}"

        ' Add headers to the HttpClient

        ' Create the content for the POST request
        Dim content As New FormUrlEncodedContent(data)
        content.Headers.Add("device-id", "MDg5ODAxMGQ4NTc3ODZmNDQ3MTdiZTI4YmZmMGU5ZWNkYzk1Y2JiYjA4OTFmODhiY2I0ZmI2ZjVhYmU5YjRkNw==")
        'content.Headers.Add("device-id", RandomDeviceId)
        ' Send the POST request
        Dim response As HttpResponseMessage = client.PostAsync(clientInitUrl, content).Result
        ' Read the response content
        Dim responseContent As String = response.Content.ReadAsStringAsync().Result
        ' Deserialize the JSON response
        Dim clientInitResponse As ClientInitData = JsonSerializer.Deserialize(Of ClientInitData)(responseContent)

        ' Step 2: Authorize
        ' Generate client_code_verifier
        Dim clientCodeVerifier As String = GenerateRandomBase64String(40)
        clientCodeVerifier = Regex.Replace(clientCodeVerifier, "[^a-zA-Z0-9]+", "")
        ' Generate client_code_challenge
        Dim challengeBytes As Byte() = SHA256.HashData(Encoding.UTF8.GetBytes(clientCodeVerifier))
        Dim clientCodeChallenge As String = Convert.ToBase64String(challengeBytes).Replace("+", "-").Replace("/", "_").TrimEnd("="c)
        Dim clientState As String = GenerateRandomBase64String(22)
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
        Dim providersResponse As JsonElement = GetRequestAsync(authorizeUrl, authParams).Result
        Dim authorize As Authorize = JsonSerializer.Deserialize(Of Authorize)(providersResponse)

        Dim captchaUrl As String = authorize.Providers(0).Provider.AuthUrl

        Debug.WriteLine($"captcha url: {captchaUrl}")
        Return captchaUrl
    End Function

    Public Function PostRequest(url As String, data As Dictionary(Of String, String), headers As Dictionary(Of String, String)) As String
        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim content As New FormUrlEncodedContent(data)
            Dim response As HttpResponseMessage = client.PostAsync(url, content).Result
            Return response.Content.ReadAsStringAsync().Result
        End Using
    End Function

    Private Async Function GetRequestAsync(authorizeUrl As String, authParams As Dictionary(Of String, String)) As Task(Of JsonElement)
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = client.GetAsync(authorizeUrl & "?" & String.Join("&", authParams.Select(Function(kvp) kvp.Key & "=" & Uri.EscapeDataString(kvp.Value)))).Result
            ' This will handle the redirect automatically
            Dim responseContent As String = Await response.Content.ReadAsStringAsync()
            Dim providers As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(responseContent)
            Return providers
        End Using

    End Function

    Friend Function DoLogin(ByRef client As HttpClient, loginSessionResponse As HttpResponseMessage, userName As String, password As String, ByRef lastErrorMessage As String) As JsonElement?
        Dim tokenData As JsonElement? = ReadTokenDataFile(GetLoginDataFileName(userName))

        If tokenData IsNot Nothing Then
            Return tokenData
        End If

        Try
            Dim endpointConfig As (SsoConfig As RootConfig, ApiBaseUri As String) = ResolveEndpointConfigAsync(DiscoveryUrl, isUsRegion:=True)
            Dim authorizeUrl As String = GetAuthorizeUrl(client, endpointConfig.SsoConfig, endpointConfig.ApiBaseUri)
        Catch ex As Exception

        End Try


        ' RICHARD I thing this is where WebView2 should be used


        'Dim queryParameters As Dictionary(Of String, String) = ParseQsl(loginSessionResponse)
        'Dim url As StringBuilder
        'With loginSessionResponse.RequestMessage.RequestUri
        '    url = New StringBuilder($"{ .Scheme}://{ .Host}{Join(loginSessionResponse.RequestMessage.RequestUri.Segments, "")}")
        'End With

        'Dim webForm As New Dictionary(Of String, String) From {
        '    {"sessionID", queryParameters.GetValueOrDefault("sessionID")},
        '    {"sessionData", queryParameters.GetValueOrDefault("sessionData")},
        '    {"locale", "en"},
        '    {"action", "login"},
        '    {"username", userName},
        '    {"password", password},
        '    {"actionButton", "Log in"}}

        'Dim payload As New Dictionary(Of String, String) From {
        '    {"country", queryParameters.GetValueOrDefault("CountryCode".ToLower)},
        '    {"locale", "en"},
        '    {"g-recaptcha-response", "abc"}
        '}

        'Dim response As HttpResponseMessage = Nothing
        'Try
        '    response = client.Post(url, s_commonHeaders, payload, webForm)
        'Catch ex As Exception
        '    Stop
        '    lastErrorMessage = $"HTTP Response is not OK, {response?.StatusCode}"
        'End Try

#If False Then
        ' Step 3: Captcha login and consent
        Dim tokenData As Dictionary(Of String, JsonElement) = Nothing
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
        {"client-authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(clientAuthStr))}"},
        {"create-session", "true"},
        {"code-verifier", clientCodeVerifier},
        {"device-id", Convert.ToBase64String(Encoding.UTF8.GetBytes(registerDeviceId))},
        {"redirect-uri", SsoConfig.OAuth.Client.ClientIds(0).RedirectUri}
    }

        ' TODO: Implement ReformatCSR function
        ' csr = ReformatCSR(csr)

        Dim regUrl As String = $"{ApiBaseUrl}{SsoConfig.Mag.SystemEndpoints.DeviceRegisterEndpointPath}"
        Dim regResponse As HttpResponseMessage = PostRequestWithHeadersAsync(regUrl, regHeaders, csr)

        If regResponse.StatusCode <> HttpStatusCode.OK Then
            Throw New Exception($"Could not register: {JsonSerializer.Deserialize(Of Dictionary(Of String, String))(regResponse.Content.ReadAsStringAsync().Result)("error_description")}")
        End If

        ' Step 5: Token
        Dim tokenReqUrl As String = ApiBaseUrl & SsoConfig.OAuth.SystemEndpoints.TokenEndpointPath
        Dim tokenReqData As New Dictionary(Of String, String) From {
        {"assertion", regResponse.Headers.GetValues("id-token").FirstOrDefault()},
        {"client_id", clientInitResponseObj("client_id").GetString()},
        {"client_secret", clientInitResponseObj("client_secret").GetString()},
        {"scope", SsoConfig.OAuth.Client.ClientIds(0).Scope},
        {"grant_type", regResponse.Headers.GetValues("id-token-type").FirstOrDefault()}
    }

        Dim tokenReqHeaders As New Dictionary(Of String, String) From {
        {"mag-identifier", regResponse.Headers.GetValues("mag-identifier").FirstOrDefault()}
    }

        Dim tokenResponse As String = Await PostRequestAsync(tokenReqUrl, tokenReqData, tokenReqHeaders)
        tokenData = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(tokenResponse)

        Debug.WriteLine("got token data from server")

        tokenData("client_id") = tokenReqData("client_id")
        tokenData("client_secret") = tokenReqData("client_secret")
        tokenData.Remove("expires_in")
        tokenData.Remove("token_type")
        tokenData("mag-identifier") = regResponse.Headers.GetValues("mag-identifier").FirstOrDefault()
#End If

        WriteTokenDataFile(tokenData, GetLoginDataFileName(userName))
        Return tokenData

    End Function

    Private Function GetLoginDataFileName(userName As String) As String
        Return Path.Combine(SettingsDirectory, $"{userName}loginData.json")
    End Function

    Friend Function NetworkUnavailable() As Boolean
        Return Not My.Computer.Network.IsAvailable
    End Function

    Public Function ResolveEndpointConfigAsync(discoveryUrl As String, isUsRegion As Boolean) As (SsoConfig As RootConfig, ApiBaseUrl As String)
        Using client As New HttpClient()
            Dim discoverResp As String = client.GetStringAsync(discoveryUrl).Result
            Dim discover As Discover = JsonSerializer.Deserialize(Of Discover)(discoverResp, s_jsonDeserializerOptions)

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

            Dim jsonString As String = client.GetStringAsync(ssoUrl).Result
            Dim ssoConfig As RootConfig = JsonSerializer.Deserialize(Of RootConfig)(jsonString)
            Dim apiBaseUrl As String = $"https://{ssoConfig.Server.Hostname}:{ssoConfig.Server.Port}/{ssoConfig.Server.Prefix}"

            Return (ssoConfig, apiBaseUrl)
        End Using
    End Function


End Module
