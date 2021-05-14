' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http

Public Module carelink_client
    Private Const CARELINK_AUTH_TOKEN_COOKIE_NAME As String = "auth_tmp_token"
    Private Const CARELINK_CONNECT_SERVER_EU As String = "carelink.minimed.eu"
    Private Const CARELINK_CONNECT_SERVER_US As String = "carelink.minimed.com"
    Private Const CARELINK_LANGUAGE_EN As String = "en"
    Private Const CARELINK_LOCALE_EN As String = "en"
    Private Const CARELINK_TOKEN_VALIDTO_COOKIE_NAME As String = "c_token_valid_to"

    Public Class CareLinkClient
        Inherits Object

        Private ReadOnly __carelinkCountry As String
        Private ReadOnly __carelinkPassword As String
        Private ReadOnly __carelinkUsername As String
        Private ReadOnly __commonHeaders As Dictionary(Of String, String)
        Private ReadOnly __httpClient As HttpClient
        Private ReadOnly __httpClientHandler As HttpClientHandler
        Private __lastDataSuccess As Boolean
        Private __lastResponseCode As HttpStatusCode
        Private __loginInProcess As Boolean
        Private __sessionCountrySettings As Dictionary(Of String, String)
        Private __sessionMonitorData As Dictionary(Of String, String)
        Private __sessionProfile As Dictionary(Of String, String)
        Private __sessionUser As Dictionary(Of String, String)

        Public Sub New(carelinkUsername As String, carelinkPassword As String, carelinkCountry As String)
            ' User info
            __carelinkUsername = carelinkUsername
            __carelinkPassword = carelinkPassword
            __carelinkCountry = carelinkCountry
            ' Session info
            __sessionUser = Nothing
            __sessionProfile = Nothing
            __sessionCountrySettings = Nothing
            __sessionMonitorData = Nothing
            ' State info
            __loginInProcess = False
            LoggedIn = False
            __lastDataSuccess = False
            __lastResponseCode = Nothing
            LastErrorMessage = Nothing
            __commonHeaders = New Dictionary(Of String, String) From {
                {
                    "Accept-Language",
                    "en;q=0.9, *;q=0.8"},
                {
                    "Connection",
                    "keep-alive"},
                {
                    "sec-ch-ua",
                    """Google Chrome"";v=""87"", "" Not;A Brand"";v=""99"", ""Chromium"";v=""87"""},
                {
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36"},
                {
                    "Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"}}

            Dim cookieContainer As New CookieContainer()
            __httpClientHandler = New HttpClientHandler With {.CookieContainer = cookieContainer}
            __httpClient = New HttpClient(__httpClientHandler)
        End Sub

        Public Property LastErrorMessage As String
        Public Property LoggedIn As Boolean

        Private Shared Function parse_qsl(loginSessionResponse As HttpResponseMessage) As Dictionary(Of String, String)
            Dim result As New Dictionary(Of String, String)
            Dim AbsoluteUri As String = loginSessionResponse.RequestMessage.RequestUri.AbsoluteUri
            Dim splitAbsoluteUri() As String = AbsoluteUri.Split("&")
            For Each item As String In splitAbsoluteUri
                Dim splitItem() As String
                If Not result.Any Then
                    item = item.Split("?")(1)
                End If
                splitItem = item.Split("=")
                result.Add(splitItem(0), splitItem(1))
            Next
            Return result
        End Function

        Private Function GetCookies(url As String) As CookieCollection
            If String.IsNullOrWhiteSpace(url) Then
                Return Nothing
            End If
            Return __httpClientHandler.CookieContainer.GetCookies(New Uri($"https://{url}"))
        End Function

        Private Function GetCookieValue(url As String, cookieName As String) As String
            If String.IsNullOrWhiteSpace(url) Then
                Return Nothing
            End If
            Dim cookie As Cookie = __httpClientHandler.CookieContainer.GetCookies(New Uri($"https://{url}")).Cast(Of Cookie)().FirstOrDefault(Function(x) x.Name = cookieName)
            Return cookie?.Value
        End Function

        Public Shared Sub Printdbg(msg As String)
            Console.WriteLine(msg)
            Application.DoEvents()
        End Sub

        ' Get server URL
        Public Overridable Function __careLinkServer() As String
            Return If(__carelinkCountry = "us", CARELINK_CONNECT_SERVER_US, CARELINK_CONNECT_SERVER_EU)
        End Function

        Public Overridable Function __correctTimeInRecentData(recentData As Dictionary(Of String, String)) As Boolean
            ' TODO
            Return True
        End Function

        Public Overridable Function __doConsent(doLoginResponse As HttpResponseMessage) As HttpResponseMessage

            ' Extract data for consent
            Dim doLoginRespBody As String = doLoginResponse.Text
            Dim url As String = __extractResponseData(doLoginRespBody, "<form action=", " ")
            Dim sessionID As String = __extractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionID"" value=", ">")
            Dim sessionData As String = __extractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionData"" value=", ">")
            ' Send consent
            Dim form As New Dictionary(Of String, String) From {
                {
                    "action",
                    "consent"},
                {
                    "sessionID",
                    sessionID},
                {
                    "sessionData",
                    sessionData},
                {
                    "response_type",
                    "code"},
                {
                    "response_mode",
                    "query"}}
            ' Add header
            Dim consentHeaders As Dictionary(Of String, String) = __commonHeaders
            consentHeaders("Content-Type") = "application/x-www-form-urlencoded"

            Try
                Dim response As HttpResponseMessage = __httpClient.Post(url, _headers:=consentHeaders, data:=form)
                If response.StatusCode = HttpStatusCode.OK Then
                    Printdbg("__doConsent() success")
                    Return response
                ElseIf response.StatusCode = HttpStatusCode.BadRequest Then
                    Printdbg("Login Failure")
                    Printdbg("__doConsent() failed")
                    Return response
                Else
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                Printdbg(e.Message)
                Printdbg("__doConsent() failed")
            End Try

            Return Nothing
        End Function

        Public Overridable Function __doLogin(loginSessionResponse As HttpResponseMessage) As HttpResponseMessage

            Dim queryParameters As Dictionary(Of String, String) = parse_qsl(loginSessionResponse)
            Const url As String = "https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login"
            Dim payload As New Dictionary(Of String, String) From {
                {
                    "country",
                    __carelinkCountry},
                {
                    "locale",
                    CARELINK_LOCALE_EN}}
            Dim form As New Dictionary(Of String, String) From {
                {
                    "sessionID",
                    queryParameters("sessionID")},
                {
                    "sessionData",
                    queryParameters("sessionData")},
                {
                    "locale",
                    CARELINK_LOCALE_EN},
                {
                    "action",
                    "login"},
                {
                    "username",
                    __carelinkUsername},
                {
                    "password",
                    __carelinkPassword},
                {
                    "actionButton",
                    "Log in"}}
            Try
                Dim response As HttpResponseMessage = __httpClient.Post(url, __commonHeaders, params:=payload, data:=form)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
                Printdbg("__doLogin() success")
                Return response
            Catch e As Exception
                Printdbg(e.Message)
                Printdbg("__doLogin() failed")
            End Try
            Return Nothing
        End Function

        Public Overridable Function __executeLoginProcedure() As Boolean
            Dim lastLoginSuccess As Boolean = False
            __loginInProcess = True
            LastErrorMessage = Nothing
            Try
                ' Clear cookies
                __httpClient.DefaultRequestHeaders.Clear()

                ' Clear basic infos
                __sessionUser = Nothing
                __sessionProfile = Nothing
                __sessionCountrySettings = Nothing
                __sessionMonitorData = Nothing

                ' Open login(get SessionId And SessionData)
                Dim loginSessionResponse As HttpResponseMessage = __getLoginSessionAsync()
                __lastResponseCode = loginSessionResponse.StatusCode

                ' Login
                Dim doLoginResponse As HttpResponseMessage = __doLogin(loginSessionResponse)
                __lastResponseCode = doLoginResponse.StatusCode

                'setLastResponseBody(loginSessionResponse)
                loginSessionResponse.Dispose()

                ' Consent
                Dim consentResponse As HttpResponseMessage = __doConsent(doLoginResponse)
                __lastResponseCode = consentResponse.StatusCode
                If consentResponse.StatusCode = HttpStatusCode.BadRequest Then
                    LastErrorMessage = "Login Failure"
                    Return lastLoginSuccess
                End If
                'setLastResponseBody(consentResponse);
                doLoginResponse.Dispose()
                consentResponse.Dispose()

                ' Get sessions infos if required
                If __sessionUser Is Nothing Then
                    __sessionUser = __getMyUser()
                End If
                If __sessionProfile Is Nothing Then
                    __sessionProfile = __getMyProfile()
                End If
                If __sessionCountrySettings Is Nothing Then
                    __sessionCountrySettings = __getCountrySettings(__carelinkCountry, CARELINK_LANGUAGE_EN)
                End If
                If __sessionMonitorData Is Nothing Then
                    __sessionMonitorData = __getMonitorData()
                End If

                ' Set login success if everything was OK:
                If __sessionUser IsNot Nothing AndAlso __sessionProfile IsNot Nothing AndAlso __sessionCountrySettings IsNot Nothing AndAlso __sessionMonitorData IsNot Nothing Then
                    lastLoginSuccess = True
                End If
            Catch e As Exception
                Printdbg(e.Message)
                LastErrorMessage = e.Message
            Finally
                __loginInProcess = False
                LoggedIn = lastLoginSuccess
            End Try
            Return lastLoginSuccess

        End Function

        Public Overridable Function __extractResponseData(responseBody As String, begstr As String, endstr As String) As String
            Dim beg As Integer = responseBody.IndexOf(begstr, StringComparison.Ordinal) + begstr.Length
            Dim [end] As Integer = responseBody.IndexOf(endstr, beg, StringComparison.Ordinal)
            Return responseBody.Substring(beg, [end] - beg).Replace("""", "")
        End Function

        Public Overridable Function __getAuthorizationToken() As String
            Dim auth_token As String = GetCookieValue(__careLinkServer, CARELINK_AUTH_TOKEN_COOKIE_NAME)
            Dim auth_token_validto As String = GetCookies(__careLinkServer)?.Item(CARELINK_TOKEN_VALIDTO_COOKIE_NAME)?.Value
            ' New token is needed:
            ' a) no token or about to expire => execute authentication
            ' b) last response 401
            If auth_token Is Nothing OrElse auth_token_validto Is Nothing OrElse New List(Of Object) From {
                401,
                403
            }.Contains(__lastResponseCode) Then
                ' TODO: add check for expired token
                ' execute new login process | null, if error OR already doing login
                'if loginInProcess or not executeLoginProcedure():
                If __loginInProcess Then
                    Printdbg("loginInProcess")
                    Return Nothing
                End If
                If Not __executeLoginProcedure() Then
                    Printdbg("__executeLoginProcedure failed")
                    Return Nothing
                End If
                Printdbg($"auth_token_validto = {GetCookies(__careLinkServer).Item(CARELINK_TOKEN_VALIDTO_COOKIE_NAME).Value}")
            End If
            ' there can be only one
            Return $"Bearer {GetCookieValue(__careLinkServer, CARELINK_AUTH_TOKEN_COOKIE_NAME)}"
        End Function

        ' Periodic data from CareLink Cloud
        Public Overridable Function __getConnectDisplayMessage(username As String, role As String, endpointUrl As String) As Dictionary(Of String, String)

            Printdbg("__getConnectDisplayMessage()")
            ' Build user json for request
            Dim userJson As New Dictionary(Of String, String) From {
                {
                    "username",
                    username},
                {
                    "role",
                    role}}
            Dim recentData As Dictionary(Of String, String) = __getData(Nothing, endpointUrl, Nothing, userJson)
            If recentData IsNot Nothing Then
                __correctTimeInRecentData(recentData)
            End If

            Return recentData
        End Function

        Public Overridable Function __getCountrySettings(country As String, language As String) As Dictionary(Of String, String)
            Printdbg("__getCountrySettings()")
            Dim queryParams As New Dictionary(Of String, String) From {
                {
                    "countryCode",
                    country},
                {
                    "language",
                    language}}
            Return __getData(__careLinkServer(), "patient/countries/settings", queryParams, Nothing)
        End Function

        Public Overridable Function __getData(host As String, path As String, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
            Dim url As String
            Printdbg("__getData()")
            __lastDataSuccess = False
            If host Is Nothing Then
                url = path
            Else
                url = $"https://{host}/{path}"
            End If
            Dim payload As Dictionary(Of String, String) = queryParams

            Dim jsondata As Dictionary(Of String, String) = Nothing
            ' Get auth token
            Dim authToken As String = __getAuthorizationToken()
            If authToken IsNot Nothing Then
                Try
                    Dim response As HttpResponseMessage
                    ' Add header
                    Dim headers As Dictionary(Of String, String) = __commonHeaders
                    headers("Authorization") = authToken
                    If requestBody Is Nothing OrElse requestBody.Count = 0 Then
                        headers("Accept") = "application/json, text/plain, */*"
                        headers("Content-Type") = "application/json; charset=utf-8"
                        response = __httpClient.Get(url, headers, params:=payload)
                        __lastResponseCode = response.StatusCode
                        If Not response.StatusCode = HttpStatusCode.OK Then
                            Throw New Exception("session get response is not OK")
                        End If
                    Else
                        headers("Accept") = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"
                        'headers("Content-Type") = "application/x-www-form-urlencoded"
                        __httpClient.DefaultRequestHeaders.Clear()
                        For Each header As KeyValuePair(Of String, String) In headers
                            If header.Key <> "Content-Type" Then
                                __httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                            End If
                        Next
                        Dim postRequest As New HttpRequestMessage(HttpMethod.Post, New Uri(url)) With {.Content = Http.Json.JsonContent.Create(requestBody)}
                        response = __httpClient.SendAsync(postRequest).Result ' Post(url, headers, data:=requestBody)
                        If Not response.StatusCode = HttpStatusCode.OK Then
                            Throw New Exception("session get response is not OK")
                        End If
                    End If
                    jsondata = Json.Loads(response.Text)
                    __lastDataSuccess = True
                Catch e As Exception
                    Printdbg(e.Message)
                    Printdbg("__getData() failed")
                End Try
            End If
            Return jsondata
        End Function

        ' Old last24hours webapp data
        Public Overridable Function __getLast24Hours() As Dictionary(Of String, String)
            Printdbg("__getLast24Hours")
            Dim queryParams As New Dictionary(Of String, String) From {
                {
                    "cpSerialNumber",
                    "NONE"},
                {
                    "msgType",
                    "last24hours"},
                {
                    "requestTime",
                    Convert.ToInt32((Date.UtcNow - New DateTime(1970, 1, 1)) * 1000).ToString()}
                }
            Return __getData(__careLinkServer(), "patient/connect/data", queryParams, Nothing)
        End Function

        Public Overridable Function __getLoginSessionAsync() As HttpResponseMessage
            ' https://carelink.minimed.com/patient/sso/login?country=us&lang=en
            Dim url As String = "https://" & __careLinkServer() & "/patient/sso/login"
            Dim payload As New Dictionary(Of String, String) From {
                {
                    "country",
                    __carelinkCountry},
                {
                    "lang",
                    CARELINK_LANGUAGE_EN}
                    }
            Dim response As HttpResponseMessage = Nothing

            Try
                response = __httpClient.Get(url, _headers:=__commonHeaders, params:=payload)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception($"session response is not OK, {response.ReasonPhrase}")
                End If
            Catch e As Exception
                Printdbg(e.Message)
                Printdbg("__getLoginSession() failed")
            End Try

            Printdbg("__getLoginSession() success")
            Return response
        End Function

        Public Overridable Function __getMonitorData() As Dictionary(Of String, String)
            Printdbg("__getMonitorData()")
            Return __getData(__careLinkServer(), "patient/monitor/data", Nothing, Nothing)
        End Function

        Public Overridable Function __getMyProfile() As Dictionary(Of String, String)
            Printdbg("__getMyProfile()")
            Return __getData(__careLinkServer(), "patient/users/me/profile", Nothing, Nothing)
        End Function

        Public Overridable Function __getMyUser() As Dictionary(Of String, String)
            Printdbg("__getMyUser()")
            Return __getData(__careLinkServer(), "patient/users/me", Nothing, Nothing)
        End Function

        Public Overridable Function getLastDataSuccess() As Object
            Return __lastDataSuccess
        End Function

        Public Overridable Function getLastErrorMessage() As String
            Return LastErrorMessage
        End Function

        Public Overridable Function getLastResponseCode() As HttpStatusCode
            Return __lastResponseCode
        End Function

        ' Wrapper for data retrieval methods
        Public Overridable Function getRecentData() As Dictionary(Of String, String)
            ' Force login to get basic info
            If __getAuthorizationToken() IsNot Nothing Then
                If __carelinkCountry = "us" OrElse __sessionMonitorData("deviceFamily") = "BLE_X" Then
                    Dim role As String = If(New List(Of Object) From {
                        "CARE_PARTNER",
                        "CARE_PARTNER_OUS"
                    }.Contains(__sessionUser("role")), "carepartner", "patient")
                    Return __getConnectDisplayMessage(__sessionProfile("username").ToString(), role, __sessionCountrySettings("blePereodicDataEndpoint"))
                Else
                    Return __getLast24Hours()
                End If
            Else
                Return Nothing
            End If
        End Function

        ' Authentication methods
        Public Overridable Function login() As Boolean
            If Not LoggedIn Then
                __executeLoginProcedure()
            End If

            Return LoggedIn
        End Function

    End Class

End Module
