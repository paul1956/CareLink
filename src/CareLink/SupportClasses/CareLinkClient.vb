' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Public Class CareLinkClient

    Private ReadOnly _carelinkCountry As String

    Private ReadOnly _carelinkPassword As String

    Private ReadOnly _carelinkUsername As String

    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _httpClientHandler As HttpClientHandler
    Private _lastErrorMessage As String
    Private _lastResponseCode? As HttpStatusCode
    Private _loginInProcess As Boolean
    Private _sessionMonitorData As New MonitorDataRecord
    Private _sessionProfile As New MyProfileRecord
    Private _sessionUser As New MyUserRecord

    Public Sub New(carelinkUsername As String, carelinkPassword As String, carelinkCountry As String)
        ' User info
        _carelinkUsername = carelinkUsername
        _carelinkPassword = carelinkPassword
        _carelinkCountry = carelinkCountry
        ' Session info
        _sessionUser.Clear()
        _sessionProfile.Clear()
        s_sessionCountrySettings.Clear()
        _sessionMonitorData.Clear()
        ' State info
        _loginInProcess = False
        Me.LoggedIn = False
        _lastErrorMessage = Nothing
        _lastResponseCode = Nothing

        Dim cookieContainer As New CookieContainer()
        _httpClientHandler = New HttpClientHandler With {.CookieContainer = cookieContainer}
        _httpClient = New HttpClient(_httpClientHandler)
    End Sub

    Public Property LoggedIn As Boolean
    Public Shared Property NetworkDown As Boolean = False

    Private Shared Function CorrectTimeInRecentData(recentData As Dictionary(Of String, String)) As Boolean
        ' TODO
        Return recentData IsNot Nothing
    End Function

    Private Shared Function ExtractResponseData(responseBody As String, begstr As String, endstr As String) As String
        Dim beg As Integer = responseBody.IndexOf(begstr, StringComparison.Ordinal) + begstr.Length
        If begstr.Length = beg + 1 Then
            Return ""
        End If
        Dim [end] As Integer = responseBody.IndexOf(endstr, beg, StringComparison.Ordinal)
        Return responseBody.Substring(beg, [end] - beg).Replace("""", "")
    End Function

    Private Shared Function ParseQsl(loginSessionResponse As HttpResponseMessage) As Dictionary(Of String, String)
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

    ' Get server URL
    Private Function CareLinkServer() As String
        Select Case _carelinkCountry.GetRegionFromCode
            Case "North America"
                Return CarelinkConnectServerUs
            Case "Europe"
                Return CarelinkConnectServerEu
            Case Else
                Return CarelinkConnectServerOther
        End Select
    End Function

    Private Function DecodeResponse(response As HttpResponseMessage, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As HttpResponseMessage
        Dim message As String
        If response.StatusCode = Global.System.Net.HttpStatusCode.OK Then
            _lastErrorMessage = Nothing
            Debug.Print($"{memberName} success")
            Return response
        ElseIf response.StatusCode = HttpStatusCode.BadRequest Then
            _lastErrorMessage = BadRequestMessage
            Debug.Print($"{memberName} at {sourceLineNumber} BadRequestMessage")
            Return response
        Else
            message = $"session response is {response.StatusCode}"
            _lastErrorMessage = message
            Debug.Print(message)
            Throw New Exception(message)
        End If
    End Function

    Private Function DoConsent(doLoginResponse As HttpResponseMessage) As HttpResponseMessage

        ' Extract data for consent
        Dim doLoginRespBody As String = doLoginResponse.Text
        Dim url As String = ExtractResponseData(doLoginRespBody, "<form action=", " ")
        Dim sessionId As String = ExtractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionID"" value=", ">")
        Dim sessionData As String = ExtractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionData"" value=", ">")
        _lastErrorMessage = ExtractResponseData(doLoginRespBody, "LoginFailed"">", "</p>")
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
            Return Me.DecodeResponse(response)
        Catch e As Exception
            Dim message As String = $"__doConsent() failed with {e.Message}"
            _lastErrorMessage = message
            Debug.Print(message)
        End Try

        Return Nothing
    End Function

    Private Function DoLogin(loginSessionResponse As HttpResponseMessage) As HttpResponseMessage

        Dim queryParameters As Dictionary(Of String, String) = ParseQsl(loginSessionResponse)
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
        Dim response As HttpResponseMessage = _httpClient.Post(url, _commonHeaders, params:=payload, data:=webForm)
        If Not response.StatusCode = HttpStatusCode.OK Then
            Throw New Exception($"HTTP Response is not OK, {response.StatusCode}")
        End If
        Return Me.DecodeResponse(response)
    End Function

    Private Function ExecuteLoginProcedure(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Boolean
        Dim lastLoginSuccess As Boolean = False
        If NetworkDown Then
            _lastErrorMessage = "Network down"
            Return lastLoginSuccess
        End If
        _loginInProcess = True
        _lastErrorMessage = Nothing
        Dim message As String
        Try
            ' Clear cookies
            _httpClient.DefaultRequestHeaders.Clear()

            ' Clear basic infos
            _sessionUser.Clear()
            _sessionProfile.Clear()
            s_sessionCountrySettings.Clear()
            _sessionMonitorData.Clear()

            ' Open login(get SessionId And SessionData)
            Dim loginSessionResponse As HttpResponseMessage = Me.GetLoginSessionAsync()
            If loginSessionResponse Is Nothing Then
                _lastErrorMessage = "Login Failure"
                Return lastLoginSuccess
            End If
            _lastResponseCode = loginSessionResponse.StatusCode

            ' Login
            Dim doLoginResponse As New HttpResponseMessage
            Try
                doLoginResponse = Me.DoLogin(loginSessionResponse)
                If doLoginResponse Is Nothing Then
                    _lastErrorMessage = "Login Failure"
                    Return lastLoginSuccess
                Else
                    _lastErrorMessage = Nothing
                End If
            Catch ex As Exception
                _lastErrorMessage = $"Login Failure {ex.Message}, in {memberName} line {sourceLineNumber}."
                Return lastLoginSuccess
            Finally
                _lastResponseCode = doLoginResponse.StatusCode
            End Try

            loginSessionResponse.Dispose()

            ' Consent
            Dim consentResponse As HttpResponseMessage = Me.DoConsent(doLoginResponse)
            _lastResponseCode = consentResponse?.StatusCode
            If consentResponse Is Nothing OrElse consentResponse.StatusCode = HttpStatusCode.BadRequest Then
                _lastErrorMessage = "Login Failure"
                Return lastLoginSuccess
            End If
            'setLastResponseBody(consentResponse);
            doLoginResponse.Dispose()
            consentResponse.Dispose()

            ' Get sessions infos if required
            If Not _sessionUser.HasValue Then
                _sessionUser = Me.GetMyUser()
            End If
            If Not _sessionProfile.HasValue Then
                _sessionProfile = Me.GetMyProfile()
            End If
            If Not s_sessionCountrySettings.HasValue Then
                s_sessionCountrySettings = New CountrySettingsRecord(Me.GetCountrySettings(_carelinkCountry, CarelinkLanguageEn))
            End If
            If Not _sessionMonitorData.HasValue Then
                _sessionMonitorData = Me.GetMonitorData()

            End If

            ' Set login success if everything was OK:
            If _sessionUser.HasValue AndAlso _sessionProfile.HasValue AndAlso s_sessionCountrySettings.HasValue AndAlso _sessionMonitorData.HasValue Then
                lastLoginSuccess = True
            End If
        Catch e As Exception
            message = $"__executeLoginProcedure failed with {e.Message}"
            Debug.Print(message)
            _lastErrorMessage = e.Message
        Finally
            _loginInProcess = False
            Me.LoggedIn = lastLoginSuccess
        End Try
        Return lastLoginSuccess

    End Function

    Private Function GetAuthorizationToken() As String
        If NetworkDown Then
            Debug.Print("Network Down")
            Return Nothing
        End If
        Dim authToken As String = Me.GetCookieValue(Me.CareLinkServer, CarelinkAuthTokenCookieName)
        Dim authTokenValidto As String = Me.GetCookies(Me.CareLinkServer)?.Item(CarelinkTokenValidtoCookieName)?.Value
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
                Debug.Print("Already In login Process")
                Return Nothing
            End If
            If Not Me.ExecuteLoginProcedure() Then
                If NetworkDown Then
                    Debug.Print("Network Down")
                End If
                Debug.Print("__executeLoginProcedure failed")
                Return Nothing
            End If
            Debug.Print($"auth_token_validto = {Me.GetCookies(Me.CareLinkServer).Item(CarelinkTokenValidtoCookieName).Value}")
        End If
        ' there can be only one
        Return $"Bearer {Me.GetCookieValue(Me.CareLinkServer, CarelinkAuthTokenCookieName)}"
    End Function

    ' Periodic data from CareLink Cloud
    Private Function GetConnectDisplayMessage(username As String, role As String, endpointUrl As String) As Dictionary(Of String, String)

        Debug.Print("__getConnectDisplayMessage()")
        ' Build user json for request
        Dim userJson As New Dictionary(Of String, String) From {
            {
                "username",
                username},
            {
                "role",
                role}}
        Dim recentData As Dictionary(Of String, String) = Me.GetData(Nothing, endpointUrl, Nothing, userJson)
        If recentData IsNot Nothing Then
            CorrectTimeInRecentData(recentData)
        End If
        Return recentData
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

    Private Function GetCountrySettings(country As String, language As String) As Dictionary(Of String, String)
        Debug.Print("__getCountrySettings()")
        Dim queryParams As New Dictionary(Of String, String) From {
            {
                "countryCode",
                country},
            {
                "language",
                language}}
        Return Me.GetData(Me.CareLinkServer(), "patient/countries/settings", queryParams, Nothing)
    End Function

    Private Function GetData(host As String, endPointPath As String, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Dim url As String
        Debug.Print("__getData()")
        If host Is Nothing Then
            url = endPointPath
        Else
            url = $"https://{host}/{endPointPath}"
        End If
        Dim payload As Dictionary(Of String, String) = queryParams

        Dim jsonData As Dictionary(Of String, String) = Nothing
        ' Get authorization token
        Dim authToken As String = Me.GetAuthorizationToken()
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
                    Dim postRequest As New HttpRequestMessage(HttpMethod.Post, New Uri(url)) With {.Content = Json.JsonContent.Create(requestBody)}
                    response = _httpClient.SendAsync(postRequest).Result ' Post(url, headers, data:=requestBody)
                End If
                If response?.StatusCode = HttpStatusCode.OK Then
                    jsonData = Loads(response.Text)
                    If jsonData.Count > 61 Then

                        Dim contents As String = JsonSerializer.Serialize(jsonData, New JsonSerializerOptions)
                        Dim options As New JsonDocumentOptions
                        Using jDocument As JsonDocument = JsonDocument.Parse(contents, options)
                            File.WriteAllText(LastDownloadWithPath, JsonSerializer.Serialize(jDocument, JsonFormattingOptions))
                        End Using
                    End If
                ElseIf response?.StatusCode = HttpStatusCode.Unauthorized Then
                    _lastResponseCode = response?.StatusCode
                    _lastErrorMessage = "Unauthorized"
                Else
                    Throw New Exception("session get response is not OK")
                End If
            Catch e As Exception
                Debug.Print($"__getData() failed {e.Message}")
            End Try
        End If
        Return jsonData
    End Function

    Private Function GetLoginSessionAsync() As HttpResponseMessage
        ' https://carelink.minimed.com/patient/sso/login?country=us&lang=en
        Dim url As String = "https://" & Me.CareLinkServer() & "/patient/sso/login"
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
            If NetworkDown Then
                Debug.Print("Network Down")
                Return Nothing
            End If
            Debug.Print($"__getLoginSession() failed {e.Message}")
            Return response
        End Try

        Debug.Print("__getLoginSession() success")
        Return response
    End Function

    Private Function GetMonitorData() As MonitorDataRecord
        Debug.Print("__getMonitorData()")
        Return New MonitorDataRecord(Me.GetData(Me.CareLinkServer(), "patient/monitor/data", Nothing, Nothing))
    End Function

    Private Function GetMyProfile() As MyProfileRecord
        Debug.Print("__getMyProfile()")
        Dim myProfileRecord As New MyProfileRecord(Me.GetData(Me.CareLinkServer(), "patient/users/me/profile", Nothing, Nothing))
        Return myProfileRecord
    End Function

    Private Function GetMyUser() As MyUserRecord
        Debug.Print("__getMyUser()")
        Dim myUserRecord As New MyUserRecord(Me.GetData(Me.CareLinkServer(), "patient/users/me", Nothing, Nothing))
        Return myUserRecord
    End Function

    Public Overridable Function GetLastErrorMessage() As String
        Return If(_lastErrorMessage, "OK")
    End Function

    Public Overridable Function HasErrors() As Boolean
        Return Not (String.IsNullOrWhiteSpace(_lastErrorMessage) OrElse _lastErrorMessage = "OK")
    End Function

    ' Wrapper for data retrieval methods
    Public Overridable Function GetRecentData(countryCode As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Dictionary(Of String, String)
        If NetworkDown Then
            Debug.Print("Network Down")
            Return Nothing
        End If

        ' Force login to get basic info
        Try
            If Me.GetAuthorizationToken() IsNot Nothing Then
                If _carelinkCountry = countryCode OrElse _sessionMonitorData.deviceFamily?.Equals("BLE_X", StringComparison.Ordinal) Then
                    Return Me.GetConnectDisplayMessage(
                        _sessionProfile.username,
                        If(_carelinkPartnerType.Contains(_sessionUser.role), "carepartner", "patient"),
                        s_sessionCountrySettings.blePereodicDataEndpoint)
                End If
            End If
        Catch ex As Exception
            Stop
        End Try
        Return Nothing
    End Function

End Class
