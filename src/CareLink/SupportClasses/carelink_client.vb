' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http

Public Class CareLinkClient
    Inherits Object

    Private Const CarelinkAuthTokenCookieName As String = "auth_tmp_token"
    Private Const CarelinkConnectServerAu As String = "carelink.minimed.au"
    Private Const CarelinkConnectServerEu As String = "carelink.minimed.eu"
    Private Const CarelinkConnectServerUs As String = "carelink.minimed.com"
    Private Const CarelinkLanguageEn As String = "en"
    Private Const CarelinkLocaleEn As String = "en"
    Private Const CarelinkTokenValidtoCookieName As String = "c_token_valid_to"
    Private ReadOnly _carelinkCountry As String

    Private ReadOnly _carelinkPartnerType As New List(Of String) From {
                        "CARE_PARTNER",
                        "CARE_PARTNER_OUS"}

    Private ReadOnly _carelinkPassword As String

    Private ReadOnly _carelinkUsername As String

    Private ReadOnly _commonHeaders As New Dictionary(Of String, String) From {
                        {
                            "Accept-Language",
                            "en;q=0.9, *;q=0.8"},
                        {
                            "Connection",
                            "keep-alive"},
                        {
                            "sec-ch-ua",
                            """Google Chrome"";deviceFamily=""87"", "" Not;A Brand"";deviceFamily=""99"", ""Chromium"";deviceFamily=""87"""},
                        {
                            "User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36"},
                        {
                            "Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;deviceFamily=b3;q=0.9"}}

    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _httpClientHandler As HttpClientHandler
    Private _lastDataSuccess As Boolean
    Private _lastResponseCode As HttpStatusCode
    Private _loginInProcess As Boolean
    Private _sessionCountrySettings As Dictionary(Of String, String)
    Private _sessionMonitorData As Dictionary(Of String, String)
    Private _sessionProfile As Dictionary(Of String, String)
    Private _sessionUser As Dictionary(Of String, String)

    Public Sub New(carelinkUsername As String, carelinkPassword As String, carelinkCountry As String)
        ' User info
        _carelinkUsername = carelinkUsername
        _carelinkPassword = carelinkPassword
        _carelinkCountry = carelinkCountry
        ' Session info
        _sessionUser = Nothing
        _sessionProfile = Nothing
        _sessionCountrySettings = Nothing
        _sessionMonitorData = Nothing
        ' State info
        _loginInProcess = False
        Me.LoggedIn = False
        _lastDataSuccess = False
        _lastResponseCode = Nothing
        Me.LastErrorMessage = Nothing

        Dim cookieContainer As New CookieContainer()
        _httpClientHandler = New HttpClientHandler With {.CookieContainer = cookieContainer}
        _httpClient = New HttpClient(_httpClientHandler)
    End Sub

    Public Property LastErrorMessage As String
    Public Property LoggedIn As Boolean

    Private Shared Function Parse_qsl(loginSessionResponse As HttpResponseMessage) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)
        Dim absoluteUri As String = loginSessionResponse.RequestMessage.RequestUri.AbsoluteUri
        Dim splitAbsoluteUri() As String = absoluteUri.Split("&")
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
        Return _httpClientHandler.CookieContainer.GetCookies(New Uri($"https://{url}"))
    End Function

    Private Function GetCookieValue(url As String, cookieName As String) As String
        If String.IsNullOrWhiteSpace(url) Then
            Return Nothing
        End If
        Dim cookie As Cookie = _httpClientHandler.CookieContainer.GetCookies(New Uri($"https://{url}")).Cast(Of Cookie)().FirstOrDefault(Function(c As Cookie) c.Name = cookieName)
        Return cookie?.Value
    End Function

    ' Get server URL
    Public Overridable Function __careLinkServer() As String
        Select Case _carelinkCountry.ToUpperInvariant
            Case "US"
                Return CarelinkConnectServerUs
            Case "AU"
                Return CarelinkConnectServerAu
            Case Else
                Return CarelinkConnectServerEu
        End Select
    End Function

    Public Overridable Function __correctTimeInRecentData(recentData As Dictionary(Of String, String)) As Boolean
        ' TODO
        Return True
    End Function

    Public Overridable Function __doConsent(doLoginResponse As HttpResponseMessage) As HttpResponseMessage

        ' Extract data for consent
        Dim doLoginRespBody As String = doLoginResponse.Text
        Dim url As String = Me.__extractResponseData(doLoginRespBody, "<form action=", " ")
        Dim sessionId As String = Me.__extractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionID"" value=", ">")
        Dim sessionData As String = Me.__extractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionData"" value=", ">")
        ' Send consent
        Dim form As New Dictionary(Of String, String) From {
            {
                "action",
                "consent"},
            {
                "sessionID",
                sessionId},
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
        Dim consentHeaders As Dictionary(Of String, String) = _commonHeaders
        consentHeaders("Content-Type") = "application/x-www-form-urlencoded"

        Try
            Dim response As HttpResponseMessage = _httpClient.Post(url, headers:=consentHeaders, data:=form)
            If response.StatusCode = HttpStatusCode.OK Then
                Debug.Print("__doConsent() success")
                Return response
            ElseIf response.StatusCode = HttpStatusCode.BadRequest Then
                Debug.Print("Login Failure __doConsent() failed with HttpStatusCode.BadRequest")
                Return response
            Else
                Debug.Print($"session response is {response.StatusCode}")
                Throw New Exception("session response is not OK")
            End If
        Catch e As Exception
            Debug.Print($"__doConsent() failed with {e.Message}")
        End Try

        Return Nothing
    End Function

    Public Overridable Function __doLogin(loginSessionResponse As HttpResponseMessage) As HttpResponseMessage

        Dim queryParameters As Dictionary(Of String, String) = Parse_qsl(loginSessionResponse)
        Const url As String = "https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login"
        Dim payload As New Dictionary(Of String, String) From {
            {
                "country",
                _carelinkCountry},
            {
                "locale",
                CarelinkLocaleEn}}
        Dim webForm As New Dictionary(Of String, String) From {
            {
                "sessionID",
                queryParameters("sessionID")},
            {
                "sessionData",
                queryParameters("sessionData")},
            {
                "locale",
                CarelinkLocaleEn},
            {
                "action",
                "login"},
            {
                "username",
                _carelinkUsername},
            {
                "password",
                _carelinkPassword},
            {
                "actionButton",
                "Log in"}}
        Try
            Dim response As HttpResponseMessage = _httpClient.Post(url, _commonHeaders, params:=payload, data:=webForm)
            If Not response.StatusCode = HttpStatusCode.OK Then
                Throw New Exception("session response is not OK")
            End If
            Debug.Print("__doLogin() success")
            Return response
        Catch e As Exception
            Debug.Print($"__doLogin() failed with {e.Message}")
        End Try
        Return Nothing
    End Function

    Public Overridable Function __executeLoginProcedure() As Boolean
        Dim lastLoginSuccess As Boolean = False
        _loginInProcess = True
        Me.LastErrorMessage = Nothing
        Try
            ' Clear cookies
            _httpClient.DefaultRequestHeaders.Clear()

            ' Clear basic infos
            _sessionUser = Nothing
            _sessionProfile = Nothing
            _sessionCountrySettings = Nothing
            _sessionMonitorData = Nothing

            ' Open login(get SessionId And SessionData)
            Dim loginSessionResponse As HttpResponseMessage = Me.__getLoginSessionAsync()
            _lastResponseCode = loginSessionResponse.StatusCode

            ' Login
            Dim doLoginResponse As HttpResponseMessage = Me.__doLogin(loginSessionResponse)
            If doLoginResponse Is Nothing Then
                Me.LastErrorMessage = "Login Failure"
                Return lastLoginSuccess

            End If
            _lastResponseCode = doLoginResponse.StatusCode

            'setLastResponseBody(loginSessionResponse)
            loginSessionResponse.Dispose()

            ' Consent
            Dim consentResponse As HttpResponseMessage = Me.__doConsent(doLoginResponse)
            _lastResponseCode = consentResponse.StatusCode
            If consentResponse.StatusCode = HttpStatusCode.BadRequest Then
                Me.LastErrorMessage = "Login Failure"
                Return lastLoginSuccess
            End If
            'setLastResponseBody(consentResponse);
            doLoginResponse.Dispose()
            consentResponse.Dispose()

            ' Get sessions infos if required
            If _sessionUser Is Nothing Then
                _sessionUser = Me.__getMyUser()
            End If
            If _sessionProfile Is Nothing Then
                _sessionProfile = Me.__getMyProfile()
            End If
            If _sessionCountrySettings Is Nothing Then
                _sessionCountrySettings = Me.__getCountrySettings(_carelinkCountry, CarelinkLanguageEn)
            End If
            If _sessionMonitorData Is Nothing Then
                _sessionMonitorData = Me.__getMonitorData()
            End If

            ' Set login success if everything was OK:
            If _sessionUser IsNot Nothing AndAlso _sessionProfile IsNot Nothing AndAlso _sessionCountrySettings IsNot Nothing AndAlso _sessionMonitorData IsNot Nothing Then
                lastLoginSuccess = True
            End If
        Catch e As Exception
            Debug.Print($"__executeLoginProcedure failed with {e.Message}")
            Me.LastErrorMessage = e.Message
        Finally
            _loginInProcess = False
            Me.LoggedIn = lastLoginSuccess
        End Try
        Return lastLoginSuccess

    End Function

    Public Overridable Function __extractResponseData(responseBody As String, begstr As String, endstr As String) As String
        Dim beg As Integer = responseBody.IndexOf(begstr, StringComparison.Ordinal) + begstr.Length
        Dim [end] As Integer = responseBody.IndexOf(endstr, beg, StringComparison.Ordinal)
        Return responseBody.Substring(beg, [end] - beg).Replace("""", "")
    End Function

    Public Overridable Function __getAuthorizationToken() As String
        Dim authToken As String = Me.GetCookieValue(Me.__careLinkServer, CarelinkAuthTokenCookieName)
        Dim authTokenValidto As String = Me.GetCookies(Me.__careLinkServer)?.Item(CarelinkTokenValidtoCookieName)?.Value
        ' New token is needed:
        ' a) no token or about to expire => execute authentication
        ' b) last response 401
        If authToken Is Nothing OrElse authTokenValidto Is Nothing OrElse New List(Of Object) From {
            401,
            403
        }.Contains(_lastResponseCode) Then
            ' TODO: add check for expired token
            ' execute new login process | null, if error OR already doing login
            'if loginInProcess or not executeLoginProcedure():
            If _loginInProcess Then
                Debug.Print("loginInProcess")
                Return Nothing
            End If
            If Not Me.__executeLoginProcedure() Then
                Debug.Print("__executeLoginProcedure failed")
                Return Nothing
            End If
            Debug.Print($"auth_token_validto = {Me.GetCookies(Me.__careLinkServer).Item(CarelinkTokenValidtoCookieName).Value}")
        End If
        ' there can be only one
        Return $"Bearer {Me.GetCookieValue(Me.__careLinkServer, CarelinkAuthTokenCookieName)}"
    End Function

    ' Periodic data from CareLink Cloud
    Public Overridable Function __getConnectDisplayMessage(username As String, role As String, endpointUrl As String) As Dictionary(Of String, String)

        Debug.Print("__getConnectDisplayMessage()")
        ' Build user json for request
        Dim userJson As New Dictionary(Of String, String) From {
            {
                "username",
                username},
            {
                "role",
                role}}
        Dim recentData As Dictionary(Of String, String) = Me.__getData(Nothing, endpointUrl, Nothing, userJson)
        If recentData IsNot Nothing Then
            Me.__correctTimeInRecentData(recentData)
        End If

        Return recentData
    End Function

    Public Overridable Function __getCountrySettings(country As String, language As String) As Dictionary(Of String, String)
        Debug.Print("__getCountrySettings()")
        Dim queryParams As New Dictionary(Of String, String) From {
            {
                "countryCode",
                country},
            {
                "language",
                language}}
        Return Me.__getData(Me.__careLinkServer(), "patient/countries/settings", queryParams, Nothing)
    End Function

    Public Overridable Function __getData(host As String, path As String, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Dim url As String
        Debug.Print("__getData()")
        _lastDataSuccess = False
        If host Is Nothing Then
            url = path
        Else
            url = $"https://{host}/{path}"
        End If
        Dim payload As Dictionary(Of String, String) = queryParams

        Dim jsondata As Dictionary(Of String, String) = Nothing
        ' Get auth token
        Dim authToken As String = Me.__getAuthorizationToken()
        If authToken IsNot Nothing Then
            Try
                Dim response As HttpResponseMessage
                ' Add header
                Dim headers As Dictionary(Of String, String) = _commonHeaders
                headers("Authorization") = authToken
                If requestBody Is Nothing OrElse requestBody.Count = 0 Then
                    headers("Accept") = "application/json, text/plain, */*"
                    headers("Content-Type") = "application/json; charset=utf-8"
                    response = _httpClient.Get(url, headers, params:=payload)
                    _lastResponseCode = response.StatusCode
                Else
                    headers("Accept") = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;deviceFamily=b3;q=0.9"
                    'headers("Content-Type") = "application/x-www-form-urlencoded"
                    _httpClient.DefaultRequestHeaders.Clear()
                    For Each header As KeyValuePair(Of String, String) In headers
                        If header.Key <> "Content-Type" Then
                            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                        End If
                    Next
                    Dim postRequest As New HttpRequestMessage(HttpMethod.Post, New Uri(url)) With {.Content = Http.Json.JsonContent.Create(requestBody)}
                    response = _httpClient.SendAsync(postRequest).Result ' Post(url, headers, data:=requestBody)
                End If
                If response.StatusCode = HttpStatusCode.OK Then
                    jsondata = Loads(response.Text)
                    _lastDataSuccess = True
                ElseIf response.StatusCode = HttpStatusCode.Unauthorized Then
                    ' Ignore handled elsewhere
                Else
                    Throw New Exception("session get response is not OK")
                End If
            Catch e As Exception
                Debug.Print($"__getData() failed {e.Message}")
            End Try
        End If
        Return jsondata
    End Function

    Public Overridable Function __getLoginSessionAsync() As HttpResponseMessage
        ' https://carelink.minimed.com/patient/sso/login?country=us&lang=en
        Dim url As String = "https://" & Me.__careLinkServer() & "/patient/sso/login"
        Dim payload As New Dictionary(Of String, String) From {
            {
                "country",
                _carelinkCountry},
            {
                "lang",
                CarelinkLanguageEn}
                }
        Dim response As HttpResponseMessage = Nothing

        Try
            response = _httpClient.Get(url, headers:=_commonHeaders, params:=payload)
            If Not response.StatusCode = HttpStatusCode.OK Then
                Throw New Exception($"session response is not OK, {response.ReasonPhrase}")
            End If
        Catch e As Exception
            Debug.Print($"__getLoginSession() failed {e.Message}")
        End Try

        Debug.Print("__getLoginSession() success")
        Return response
    End Function

    Public Overridable Function __getMonitorData() As Dictionary(Of String, String)
        Debug.Print("__getMonitorData()")
        Return Me.__getData(Me.__careLinkServer(), "patient/monitor/data", Nothing, Nothing)
    End Function

    Public Overridable Function __getMyProfile() As Dictionary(Of String, String)
        Debug.Print("__getMyProfile()")
        Return Me.__getData(Me.__careLinkServer(), "patient/users/me/profile", Nothing, Nothing)
    End Function

    Public Overridable Function __getMyUser() As Dictionary(Of String, String)
        Debug.Print("__getMyUser()")
        Return Me.__getData(Me.__careLinkServer(), "patient/users/me", Nothing, Nothing)
    End Function

    Public Overridable Function GetLastDataSuccess() As Object
        Return _lastDataSuccess
    End Function

    Public Overridable Function GetLastErrorMessage() As String
        Return Me.LastErrorMessage
    End Function

    Public Overridable Function GetLastResponseCode() As HttpStatusCode
        Return _lastResponseCode
    End Function

    ' Wrapper for data retrieval methods
    Public Overridable Function GetRecentData() As Dictionary(Of String, String)
        ' Force login to get basic info
        If Me.__getAuthorizationToken() IsNot Nothing Then
            Dim deviceFamily As String = Nothing
            _sessionMonitorData.TryGetValue("deviceFamily", deviceFamily)
            If _carelinkCountry = My.Settings.CountryCode OrElse deviceFamily?.Equals("BLE_X") Then
                Dim role As String = If(_carelinkPartnerType.Contains(_sessionUser("role")), "carepartner", "patient")
                Return Me.__getConnectDisplayMessage(_sessionProfile("username").ToString(), role, _sessionCountrySettings("blePereodicDataEndpoint"))
            End If
        End If
        Return Nothing
    End Function

    ' Authentication methods
    Public Overridable Function Login() As Boolean
        If Not Me.LoggedIn Then
            Me.__executeLoginProcedure()
        End If

        Return Me.LoggedIn
    End Function

End Class
