' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class CareLinkClient
    Private Const CareLinkAuthTokenCookieName As String = "auth_tmp_token"
    Private Const CareLinkTokenValidToCookieName As String = "c_token_valid_to"

    Private ReadOnly _careLinkPartnerType As New List(Of String) From {
                        "CARE_PARTNER",
                        "CARE_PARTNER_OUS"}

    Private _httpClient As HttpClient
    Private _httpClientHandler As HttpClientHandler
    Private _inLoginInProcess As Boolean
    Private _lastErrorMessage As String
    Private _lastResponseCode? As HttpStatusCode
    Private _sessionMonitorData As New SessionMonitorDataRecord
    Private _sessionProfile As New SessionProfileRecord
    Private _sessionUser As New SessionUserRecord

    Public Sub New(username As String, password As String, country As String)
        ' User info
        Me.CareLinkUsername = username
        Me.CareLinkPassword = password
        Me.CareLinkCountry = country

        ' State info
        _inLoginInProcess = False
        Me.LoggedIn = False
        _lastErrorMessage = Nothing
        _lastResponseCode = Nothing

        _httpClient = Me.NewHttpClientWithCookieContainer
    End Sub

    Public Property LoggedIn As Boolean

    Public Property SessionProfile As SessionProfileRecord
        Get
            Return _sessionProfile
        End Get
        Set(value As SessionProfileRecord)
            _sessionProfile = value
        End Set
    End Property

    Private ReadOnly Property CareLinkCountry As String = Nothing
    Private ReadOnly Property CareLinkPassword As String
    Private ReadOnly Property CareLinkUsername As String

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

            ' Clear basic session records
            _sessionUser.Clear()
            _sessionProfile.Clear()
            s_sessionCountrySettings.Clear()
            _sessionMonitorData.Clear()

            ' Open login(get SessionId And SessionData)
            Using loginSessionResponse As HttpResponseMessage = Me.GetLoginSession(host)
                If Not loginSessionResponse.IsSuccessStatusCode Then
                    Return lastLoginSuccess
                End If
                _lastResponseCode = loginSessionResponse.StatusCode

                ' Login
                Using doLoginResponse As HttpResponseMessage = DoLogin(_httpClient, loginSessionResponse, Me.CareLinkUsername, Me.CareLinkPassword, Me.CareLinkCountry, _lastErrorMessage)
                    Try
                        If doLoginResponse Is Nothing Then
                            _lastErrorMessage = "Login Failure"
                            Return lastLoginSuccess
                        Else
                            _lastErrorMessage = Nothing
                        End If
                    Catch ex As Exception
                        _lastErrorMessage = $"Login Failure {ex.DecodeException()}, in {NameOf(ExecuteLoginProcedure)}."
                        Return lastLoginSuccess
                    Finally
                        If doLoginResponse IsNot Nothing Then
                            _lastResponseCode = doLoginResponse.StatusCode
                        Else
                            _lastResponseCode = HttpStatusCode.NoContent
                        End If
                    End Try

                    ' Consent
                    Using consentResponse As HttpResponseMessage = DoConsent(_httpClient, doLoginResponse, _lastErrorMessage)
                        _lastResponseCode = consentResponse?.StatusCode
                        If consentResponse?.IsSuccessStatusCode Then
                        Else
                            _lastErrorMessage = "Login Failure"
                            Return lastLoginSuccess
                        End If
                    End Using
                End Using
            End Using

            Dim authToken As String = Me.GetBearerToken(CareLinkServerURL(Me.CareLinkCountry))

            ' MUST BE FIRST DO NOT MOVE NEXT LINE
            s_sessionCountrySettings = New CountrySettingsRecord(mainForm, Me.GetCountrySettings(authToken))
            _sessionUser = Me.GetMyUser(mainForm, authToken)
            _sessionProfile = Me.GetMyProfile(authToken)
            _sessionMonitorData = Me.GetMonitorData(authToken)

            ' Set login success if everything was OK:
            If _sessionUser.HasValue _
               AndAlso _sessionProfile.HasValue _
               AndAlso s_sessionCountrySettings.HasValue _
               AndAlso _sessionMonitorData.HasValue Then
                lastLoginSuccess = True
            End If
        Catch ex As Exception
            message = $"__executeLoginProcedure failed with {ex.DecodeException()}"
            Debug.Print(message.Replace(vbCrLf, ""))
            _lastErrorMessage = ex.DecodeException()
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

        Dim url As String = CareLinkServerURL(Me.CareLinkCountry)
        ' New token is needed:
        ' a) no token or about to expire => execute authentication
        ' b) last response 401
        If Me.GetCookieValue(url, CareLinkAuthTokenCookieName) Is Nothing OrElse
            Me.GetCookies(url)?.Item(CareLinkTokenValidToCookieName)?.Value Is Nothing OrElse
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
            Debug.Print($"auth_token_validTo = {Me.GetCookies(url).Item(CareLinkTokenValidToCookieName).Value}")
        End If
        ' there can be only one
        authToken = Me.GetBearerToken(url)
        Return GetAuthorizationTokenResult.OK
    End Function

    ' Periodic data from CareLink Cloud
    Private Function GetConnectDisplayMessage(MainForm As Form1, username As String, role As String, endpointUrl As String, Optional patient_user_name As String = "") As Dictionary(Of String, String)

        Debug.Print("__getConnectDisplayMessage()")
        ' Build user Json for request
        Dim userJson As New Dictionary(Of String, String) From {
            {
                "username",
                username},
            {
                "role",
                role}}
        If role = "carepartner" Then
            userJson.Add("patientId", patient_user_name)
        End If
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
                Me.CareLinkCountry},
            {
                "language",
                "en"}}
        Return Me.GetData(authToken, CareLinkServerURL(Me.CareLinkCountry), "patient/countries/settings", queryParams, Nothing)
    End Function

    Private Function GetData(authToken As String, host As String, endPointPath As String, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Debug.Print($"GetData(mainForm As {NameOf(Form1)},host = {host}, endPointPath = {endPointPath}, queryParams = {queryParams.ToCsv}, requestBody = {requestBody.ToCsv}")
        Dim jsonData As Dictionary(Of String, String) = Nothing
        If authToken IsNot Nothing Then
            Dim response As New HttpResponseMessage
            Try
                ' Add header
                Dim headers As Dictionary(Of String, String) = s_commonHeaders.Clone
                headers("Authorization") = authToken

                Dim url As New StringBuilder(If(host Is Nothing, endPointPath, $"https://{host}/{endPointPath}"))
                If requestBody Is Nothing OrElse requestBody.Count = 0 Then
                    headers("Accept") = "application/json, text/plain, */*"
                    headers("Content-Type") = "application/json; charset=utf-8"
                    response = _httpClient.Get(url, _lastErrorMessage, headers, params:=queryParams)
                    _lastResponseCode = response.StatusCode
                Else
                    headers("Accept") = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webP,image/apng,*/*;q=0.8,application/signed-exchange;deviceFamily=b3;q=0.9"
                    'headers("Content-Type") = "application/x-www-form-urlEncoded"
                    _httpClient.DefaultRequestHeaders.Clear()
                    For Each header As KeyValuePair(Of String, String) In headers
                        If header.Key <> "Content-Type" Then
                            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                        End If
                    Next
                    ' Post(url, headers, data:=requestBody)
                    Dim postRequest As New HttpRequestMessage(HttpMethod.Post, New Uri(url.ToString)) With {.Content = Json.JsonContent.Create(requestBody)}
                    response = _httpClient.SendAsync(postRequest).Result
                    _lastResponseCode = response.StatusCode
                End If
                If response?.IsSuccessStatusCode Then
                    jsonData = Loads(response.Text)
                    If jsonData.Count > 61 Then

                        Dim contents As String = JsonSerializer.Serialize(jsonData, New JsonSerializerOptions)
                        Using jDocument As JsonDocument = JsonDocument.Parse(contents, New JsonDocumentOptions)
                            File.WriteAllText(GetPathToLastDownloadFile(), JsonSerializer.Serialize(jDocument, JsonFormattingOptions))
                        End Using
                    End If
                ElseIf response?.StatusCode = HttpStatusCode.Unauthorized Then
                    _lastResponseCode = response?.StatusCode
                    _lastErrorMessage = "Unauthorized"
                ElseIf response?.StatusCode = HttpStatusCode.InternalServerError Then
                    _lastResponseCode = response?.StatusCode
                    _lastErrorMessage = $"{ProjectName} Server Down"
                Else
                    Throw New Exception($"session get response is not OK, last error = {_lastErrorMessage}")
                End If
                response.Dispose()
            Catch ex As Exception
                _lastErrorMessage = ex.DecodeException()
                Debug.Print($"__getData() failed {_lastErrorMessage.Replace(vbCrLf, "")}, status {response?.StatusCode}")
            End Try
        End If
        Return jsonData
    End Function

    Private Function GetLoginSession(host As String) As HttpResponseMessage
        ' https://CareLink.MiniMed.com/patient/sso/login?country=us&lang=en
        Dim url As New StringBuilder($"https://{host}/patient/sso/login")
        Dim payload As New Dictionary(Of String, String) From {
            {
                "country",
                Me.CareLinkCountry},
            {
                "lang",
                "en"}
                }
        Dim response As HttpResponseMessage = Nothing

        Try
            response = _httpClient.Get(url, _lastErrorMessage, s_commonHeaders, payload)
            If Not response.IsSuccessStatusCode Then
                Throw New Exception($"session response is not OK, {response.ReasonPhrase}")
            End If
        Catch ex As Exception
            If NetworkDown Then
                _lastErrorMessage = "No Internet Connection!"
                Debug.Print("No Internet Connection!")
                Return response
            End If
            Debug.Print($"__getLoginSession() failed {ex.DecodeException().Replace(vbCrLf, "")}")
            Return response
        End Try

        Debug.Print("__getLoginSession() success")
        Return response
    End Function

    Private Function GetMonitorData(authToken As String) As SessionMonitorDataRecord
        Debug.Print("__getMonitorData()")
        Return New SessionMonitorDataRecord(Me.GetData(authToken, CareLinkServerURL(Me.CareLinkCountry), "patient/monitor/data", Nothing, Nothing))
    End Function

    Private Function GetMyProfile(authToken As String) As SessionProfileRecord
        Debug.Print("__getMyProfile()")
        Return New SessionProfileRecord(Me.GetData(authToken, CareLinkServerURL(Me.CareLinkCountry), "patient/users/me/profile", Nothing, Nothing))
    End Function

    Private Function GetMyUser(MainForm As Form1, authToken As String) As SessionUserRecord
        Debug.Print("__getMyUser()")
        Dim myUserRecord As New SessionUserRecord(MainForm.DgvCurrentUser, Me.GetData(authToken, CareLinkServerURL(Me.CareLinkCountry), "patient/users/me", Nothing, Nothing))
        Return myUserRecord
    End Function

    Private Function NewHttpClientWithCookieContainer() As HttpClient
        Dim cookieContainer As New CookieContainer()
        _httpClientHandler = New HttpClientHandler With {.CookieContainer = cookieContainer}
        Return New HttpClient(_httpClientHandler)
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
                If (s_sessionCountrySettings.HasValue _
                        AndAlso Not String.IsNullOrWhiteSpace(Me.CareLinkCountry)) OrElse
                        _sessionMonitorData.deviceFamily?.Equals("BLE_X", StringComparison.Ordinal) Then
                    If _careLinkPartnerType.Contains(_sessionUser.role, StringComparer.InvariantCultureIgnoreCase) Then
                        Return Me.GetConnectDisplayMessage(
                        MainForm,
                        _sessionProfile.username,
                        "carepartner",
                        s_sessionCountrySettings.blePereodicDataEndpoint, My.Settings.CareLinkPatientUserID)
                    Else
                        Return Me.GetConnectDisplayMessage(
                        MainForm,
                        _sessionProfile.username,
                        "patient",
                        s_sessionCountrySettings.blePereodicDataEndpoint)
                    End If
                End If
            End If
        Catch ex As Exception
            Stop
        End Try
        Return Nothing
    End Function

    Public Overridable Function HasErrors() As Boolean
        Return Not (String.IsNullOrWhiteSpace(_lastErrorMessage) OrElse _lastErrorMessage = "OK")
    End Function

    Private Function GetBearerToken(url As String) As String
        Return $"Bearer {Me.GetCookieValue(url, CareLinkAuthTokenCookieName)}"
    End Function

    Private Function GetData(MainForm As Form1, endPointPath As String, requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Debug.Print($"GetData(mainForm = {MainForm.Name}, endPointPath = {endPointPath}, requestBody = {requestBody.ToCsv}")
        ' Get authorization token
        Dim authToken As String = Nothing
        Select Case Me.GetAuthorizationToken(MainForm, authToken)
            Case GetAuthorizationTokenResult.OK
                Dim jsonDictionary As Dictionary(Of String, String) = Me.GetData(authToken, Nothing, endPointPath, Nothing, requestBody)
                If _lastResponseCode <> HttpStatusCode.OK Then
                    ReportLoginStatus(MainForm.LoginStatus)
                    _httpClient = Me.NewHttpClientWithCookieContainer
                End If
                Return jsonDictionary
            Case GetAuthorizationTokenResult.NetworkDown
                ReportLoginStatus(MainForm.LoginStatus)
            Case GetAuthorizationTokenResult.InLoginProcess
                Exit Select
            Case GetAuthorizationTokenResult.LoginFailed
                ReportLoginStatus(MainForm.LoginStatus, True, _lastErrorMessage)
        End Select
        Return Nothing
    End Function

End Class
