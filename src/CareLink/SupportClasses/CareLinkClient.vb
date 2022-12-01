' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text.Json

Public Class CareLinkClient
    Private Const CarelinkAuthTokenCookieName As String = "auth_tmp_token"
    Private Const CarelinkTokenValidtoCookieName As String = "c_token_valid_to"

    Private ReadOnly _carelinkPartnerType As New List(Of String) From {
                        "CARE_PARTNER",
                        "CARE_PARTNER_OUS"}

    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _httpClientHandler As HttpClientHandler
    Private _inLoginInProcess As Boolean
    Private _lastErrorMessage As String
    Private _lastResponseCode? As HttpStatusCode
    Private _sessionMonitorData As New MonitorDataRecord
    Private _sessionProfile As New MyProfileRecord
    Private _sessionUser As New MyUserRecord

    Public Sub New(username As String, password As String, country As String)
        ' User info
        Me.CarelinkUsername = username
        Me.CarelinkPassword = password
        Me.CarelinkCountry = country

        ' State info
        _inLoginInProcess = False
        Me.LoggedIn = False
        _lastErrorMessage = Nothing
        _lastResponseCode = Nothing

        Dim cookieContainer As New CookieContainer()
        _httpClientHandler = New HttpClientHandler With {.CookieContainer = cookieContainer}
        _httpClient = New HttpClient(_httpClientHandler)
    End Sub

    Public Property LoggedIn As Boolean
    Private ReadOnly Property CarelinkCountry As String = Nothing

    Private ReadOnly Property CarelinkPassword As String

    Private ReadOnly Property CarelinkUsername As String

    Private Shared Function CorrectTimeInRecentData(recentData As Dictionary(Of String, String)) As Boolean
        ' TODO
        Return recentData IsNot Nothing
    End Function

    Private Function ExecuteLoginProcedure(mainForm As Form1, host As String) As Boolean
        Dim lastLoginSuccess As Boolean = False
        If NetworkDown Then
            _lastErrorMessage = "No Internet Connection!"
            ReportLoginStatus(mainForm.LoginStatus)
            Return lastLoginSuccess
        End If
        _inLoginInProcess = True
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
            Using loginSessionResponse As HttpResponseMessage = Me.GetLoginSessionAsync(host)
                If loginSessionResponse Is Nothing Then
                    _lastErrorMessage = "Login Failure"
                    Return lastLoginSuccess
                End If
                _lastResponseCode = loginSessionResponse.StatusCode

                ' Login
                Using doLoginResponse As HttpResponseMessage = DoLogin(_httpClient, loginSessionResponse, Me.CarelinkUsername, Me.CarelinkPassword, Me.CarelinkCountry, _lastErrorMessage)
                    Try
                        If doLoginResponse Is Nothing Then
                            _lastErrorMessage = "Login Failure"
                            Return lastLoginSuccess
                        Else
                            _lastErrorMessage = Nothing
                        End If
                    Catch ex As Exception
                        _lastErrorMessage = $"Login Failure {ex.Message}, in {NameOf(ExecuteLoginProcedure)}."
                        Return lastLoginSuccess
                    Finally
                        _lastResponseCode = doLoginResponse.StatusCode
                    End Try

                    ' Consent
                    Using consentResponse As HttpResponseMessage = DoConsent(_httpClient, doLoginResponse, _lastErrorMessage)
                        _lastResponseCode = consentResponse?.StatusCode
                        If consentResponse Is Nothing OrElse consentResponse.StatusCode = HttpStatusCode.BadRequest Then
                            _lastErrorMessage = "Login Failure"
                            Return lastLoginSuccess
                        End If
                    End Using
                End Using
            End Using

            Dim authToken As String = Me.GetBearerToken(CareLinkServerURL(Me.CarelinkCountry))

            ' MUST BE FIRST DO NOT MOVE NEXT LINE
            s_sessionCountrySettings = New CountrySettingsRecord(mainForm, Me.GetCountrySettings(authToken))
            _sessionUser = Me.GetMyUser(authToken)
            _sessionProfile = Me.GetMyProfile(authToken)
            _sessionMonitorData = Me.GetMonitorData(authToken)

            ' Set login success if everything was OK:
            If _sessionUser.HasValue AndAlso _sessionProfile.HasValue AndAlso s_sessionCountrySettings.HasValue AndAlso _sessionMonitorData.HasValue Then
                lastLoginSuccess = True
            End If
        Catch e As Exception
            message = $"__executeLoginProcedure failed with {e.Message}"
            Debug.Print(message)
            _lastErrorMessage = e.Message
        Finally
            _inLoginInProcess = False
            Me.LoggedIn = lastLoginSuccess
        End Try
        Return lastLoginSuccess

    End Function

    Private Function GetAuthorizationToken(MainForm As Form1, ByRef authToken As String) As GetAuthorizationTokenResult
        If NetworkDown Then
            _lastErrorMessage = "No Internet Connection!"
            Return GetAuthorizationTokenResult.NetworkDown
        End If

        Dim url As String = CareLinkServerURL(Me.CarelinkCountry)
        ' New token is needed:
        ' a) no token or about to expire => execute authentication
        ' b) last response 401
        If Me.GetCookieValue(url, CarelinkAuthTokenCookieName) Is Nothing OrElse
            Me.GetCookies(url)?.Item(CarelinkTokenValidtoCookieName)?.Value Is Nothing OrElse
            New List(Of Object)() From {401, 403}.Contains(_lastResponseCode) Then
            ' TODO: add check for expired token
            ' execute new login process | null, if error OR already doing login
            'if loginInProcess or not executeLoginProcedure():
            If _inLoginInProcess Then
                Debug.Print("Already In login Process")
                Return GetAuthorizationTokenResult.InLoginProcess
            End If
            If Not Me.ExecuteLoginProcedure(MainForm, url) Then
                If NetworkDown Then
                    _lastErrorMessage = "No Internet Connection!"
                    Debug.Print("No Internet Connection!")
                    Return GetAuthorizationTokenResult.NetworkDown
                End If
                Debug.Print("__executeLoginProcedure failed")
                Return GetAuthorizationTokenResult.LoginFailed
            End If
            Debug.Print($"auth_token_validto = {Me.GetCookies(url).Item(CarelinkTokenValidtoCookieName).Value}")
        End If
        ' there can be only one
        authToken = Me.GetBearerToken(url)
        Return GetAuthorizationTokenResult.OK
    End Function

    Private Function GetBearerToken(url As String) As String
        Return $"Bearer {Me.GetCookieValue(url, CarelinkAuthTokenCookieName)}"
    End Function

    ' Periodic data from CareLink Cloud
    Private Function GetConnectDisplayMessage(MainForm As Form1, username As String, role As String, endpointUrl As String) As Dictionary(Of String, String)

        Debug.Print("__getConnectDisplayMessage()")
        ' Build user json for request
        Dim userJson As New Dictionary(Of String, String) From {
            {
                "username",
                username},
            {
                "role",
                role}}
        Dim recentData As Dictionary(Of String, String) = Me.GetData(MainForm, endpointUrl, userJson)
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

    Private Function GetCountrySettings(authToken As String) As Dictionary(Of String, String)
        Debug.Print("__getCountrySettings()")
        Dim queryParams As New Dictionary(Of String, String) From {
            {
                "countryCode",
                Me.CarelinkCountry},
            {
                "language",
                "en"}}
        Return Me.GetData(authToken, CareLinkServerURL(Me.CarelinkCountry), "patient/countries/settings", queryParams, Nothing)
    End Function

    Private Function GetData(MainForm As Form1, endPointPath As String, requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Debug.Print($"GetData(mainForm As Form1, {endPointPath}, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)")
        ' Get authorization token
        Dim authToken As String = Nothing
        Select Case Me.GetAuthorizationToken(MainForm, authToken)
            Case GetAuthorizationTokenResult.OK
                Return Me.GetData(authToken, Nothing, endPointPath, Nothing, requestBody)
            Case GetAuthorizationTokenResult.NetworkDown
                ReportLoginStatus(MainForm.LoginStatus)
            Case GetAuthorizationTokenResult.InLoginProcess
                Exit Select
            Case GetAuthorizationTokenResult.LoginFailed
                ReportLoginStatus(MainForm.LoginStatus, True, _lastErrorMessage)
        End Select
        Return Nothing
    End Function

    Private Function GetLoginSessionAsync(host As String) As HttpResponseMessage
        ' https://carelink.minimed.com/patient/sso/login?country=us&lang=en
        Dim url As String = $"https://{host}/patient/sso/login"
        Dim payload As New Dictionary(Of String, String) From {
            {
                "country",
                Me.CarelinkCountry},
            {
                "lang",
                "en"}
                }
        Dim response As HttpResponseMessage = Nothing

        Try
            response = _httpClient.Get(url, headers:=CommonHeaders, params:=payload)
            If Not response.StatusCode = HttpStatusCode.OK Then
                Throw New Exception($"session response is not OK, {response.ReasonPhrase}")
            End If
        Catch e As Exception
            If NetworkDown Then
                _lastErrorMessage = "No Internet Connection!"
                Debug.Print("No Internet Connection!")
                Return Nothing
            End If
            Debug.Print($"__getLoginSession() failed {e.Message}")
            Return response
        End Try

        Debug.Print("__getLoginSession() success")
        Return response
    End Function

    Private Function GetMonitorData(authToken As String) As MonitorDataRecord
        Debug.Print("__getMonitorData()")
        Return New MonitorDataRecord(Me.GetData(authToken, CareLinkServerURL(Me.CarelinkCountry), "patient/monitor/data", Nothing, Nothing))
    End Function

    Private Function GetMyProfile(authToken As String) As MyProfileRecord
        Debug.Print("__getMyProfile()")
        Dim myProfileRecord As New MyProfileRecord(Me.GetData(authToken, CareLinkServerURL(Me.CarelinkCountry), "patient/users/me/profile", Nothing, Nothing))
        Return myProfileRecord
    End Function

    Private Function GetMyUser(authToken As String) As MyUserRecord
        Debug.Print("__getMyUser()")
        Dim myUserRecord As New MyUserRecord(Me.GetData(authToken, CareLinkServerURL(Me.CarelinkCountry), "patient/users/me", Nothing, Nothing))
        Return myUserRecord
    End Function

    Public Overridable Function GetLastErrorMessage() As String
        Return If(_lastErrorMessage, "OK")
    End Function

    ' Wrapper for data retrieval methods
    Public Overridable Function GetRecentData(MainForm As Form1) As Dictionary(Of String, String)
        If NetworkDown Then
            _lastErrorMessage = "No Internet Connection!"
            ReportLoginStatus(MainForm.LoginStatus)
            Debug.Print("No Internet Connection!")
            Return Nothing
        End If

        ' Force login to get basic info
        Try
            Dim authToken As String = Nothing
            If Me.GetAuthorizationToken(MainForm, authToken) = GetAuthorizationTokenResult.OK Then
                If Me.CarelinkCountry IsNot Nothing OrElse _sessionMonitorData.deviceFamily?.Equals("BLE_X", StringComparison.Ordinal) Then

                    Return Me.GetConnectDisplayMessage(
                        MainForm,
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

    Private Function GetData(authToken As String, host As String, endPointPath As String, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Dim jsonData As Dictionary(Of String, String) = Nothing
        If authToken IsNot Nothing Then
            Dim response As New HttpResponseMessage
            Try
                ' Add header
                Dim headers As Dictionary(Of String, String) = CommonHeaders
                headers("Authorization") = authToken

                Dim url As String = If(host Is Nothing, endPointPath, $"https://{host}/{endPointPath}")
                If requestBody Is Nothing OrElse requestBody.Count = 0 Then
                    headers("Accept") = "application/json, text/plain, */*"
                    headers("Content-Type") = "application/json; charset=utf-8"
                    response = _httpClient.Get(url, headers, params:=queryParams)
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
                    ' Post(url, headers, data:=requestBody)
                    Dim postRequest As New HttpRequestMessage(HttpMethod.Post, New Uri(url)) With {.Content = Json.JsonContent.Create(requestBody)}
                    response = _httpClient.SendAsync(postRequest).Result
                    _lastResponseCode = response.StatusCode
                End If
                If response?.StatusCode = HttpStatusCode.OK Then
                    jsonData = Loads(response.Text)
                    If jsonData.Count > 61 Then

                        Dim contents As String = JsonSerializer.Serialize(jsonData, New JsonSerializerOptions)
                        Using jDocument As JsonDocument = JsonDocument.Parse(contents, New JsonDocumentOptions)
                            File.WriteAllText(LastDownloadWithPath, JsonSerializer.Serialize(jDocument, JsonFormattingOptions))
                        End Using
                    End If
                ElseIf response?.StatusCode = HttpStatusCode.Unauthorized Then
                    _lastResponseCode = response?.StatusCode
                    _lastErrorMessage = "Unauthorized"
                ElseIf response?.StatusCode = HttpStatusCode.InternalServerError Then
                    _lastResponseCode = response?.StatusCode
                    _lastErrorMessage = "CareLink Server Down"
                Else
                    Throw New Exception("session get response is not OK")
                End If
                response.Dispose()
            Catch e As Exception
                Debug.Print($"__getData() failed {e.Message}, status {response?.StatusCode}")
            End Try
        End If
        Return jsonData
    End Function

    Public Overridable Function HasErrors() As Boolean
        Return Not (String.IsNullOrWhiteSpace(_lastErrorMessage) OrElse _lastErrorMessage = "OK")
    End Function

End Class
