' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Imports Octokit

Public Class CareLinkClient
    Private Const CareLinkAuthTokenCookieName As String = "auth_tmp_token"

    Private Const CareLinkTokenValidToCookieName As String = "c_token_valid_to"

    Private ReadOnly _careLinkPartnerType As New List(Of String) From {
            "CARE_PARTNER",
            "CARE_PARTNER_OUS"
        }

    Private _httpClient As HttpClient

    Private _inLoginInProcess As Boolean

    Private _lastErrorMessage As String

    Private _lastResponseCode? As HttpStatusCode

    Private _sessionMonitorData As New SessionMonitorDataRecord

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

    Private Enum GetAuthorizationTokenResult
        InLoginProcess
        LoginFailed
        NetworkDown
        OK
    End Enum

#Region "Current CareLink User Information"

    Private ReadOnly Property CareLinkCountry As String
    Private ReadOnly Property CareLinkPassword As String
    Private ReadOnly Property CareLinkUsername As String

#End Region

    Private Property ClientHandler As HttpClientHandler
    Friend Property LoggedIn As Boolean
    Friend Property SessionProfile As New SessionProfileRecord
    Friend Property SessionUser As New SessionUserRecord

    ''' <summary>
    ''' Get server URL from Country Code, only US today has a special server
    ''' </summary>
    ''' <param name="countryCode"></param>
    ''' <returns>Server URL</returns>
    Private Shared Function GetServerUrl(countryCode As String) As String
        Select Case If(String.IsNullOrWhiteSpace(countryCode), "US", countryCode)
            Case "US"
                Return "carelink.minimed.com"
            Case Else
                Return "carelink.minimed.eu"
        End Select
    End Function

    ''' <summary>
    ''' Logs in user and collects Records with User, Profile, CountrySettings and Device Family
    ''' </summary>
    ''' <param name="serverUrl"></param>
    '''
    ''' <returns>True is login successful</returns>
    Private Function ExecuteLoginProcedure(serverUrl As String) As Boolean
        If NetworkUnavailable() Then
            _lastErrorMessage = "No Internet Connection!"
            ReportLoginStatus(Form1.LoginStatus)
            Return False
        End If
        _inLoginInProcess = True
        _lastErrorMessage = Nothing
        Dim message As String
        Dim lastLoginSuccess As Boolean = False
        Try
            ' Clear cookies
            _httpClient.DefaultRequestHeaders.Clear()

            ' Clear basic session records
            Me.SessionUser.Clear()
            Me.SessionProfile.Clear()
            s_sessionCountrySettings.Clear()
            _sessionMonitorData.Clear()

            ' Open login(get SessionId And SessionData)
            Using loginSessionResponse As HttpResponseMessage = Me.GetLoginSession(serverUrl)
                _lastResponseCode = loginSessionResponse.StatusCode
                If Not loginSessionResponse.IsSuccessStatusCode Then
                    If Not loginSessionResponse.ReasonPhrase = "Not Implemented" Then
                        _lastErrorMessage = loginSessionResponse.ReasonPhrase
                    End If
                    Return False
                End If

                ' Login
                Using doLoginResponse As HttpResponseMessage = DoLogin(_httpClient, loginSessionResponse, Me.CareLinkUsername, Me.CareLinkPassword, _lastErrorMessage)
                    Try
                        If doLoginResponse Is Nothing Then
                            _lastErrorMessage = "Login Failure with reason unknown"
                            Return False
                        ElseIf Not doLoginResponse.IsSuccessStatusCode Then
                            If doLoginResponse.ReasonPhrase.Replace(" ", "") <> HttpStatusCode.NetworkAuthenticationRequired.ToString Then
                                _lastErrorMessage = doLoginResponse.ReasonPhrase
                            End If
                        End If
                    Catch ex As Exception
                        _lastErrorMessage = $"Login Failure {ex.DecodeException()}, in {NameOf(ExecuteLoginProcedure)}."
                        Return False
                    Finally
                        _lastResponseCode = If(doLoginResponse Is Nothing,
                                               HttpStatusCode.NoContent,
                                               doLoginResponse.StatusCode
                                              )
                    End Try

                    If Not String.IsNullOrWhiteSpace(_lastErrorMessage) Then
                        Return False
                    End If

                    ' If we are here we at least were successful logging in

                    ' Consent
                    Using consentResponse As HttpResponseMessage = DoConsent(_httpClient, doLoginResponse, _lastErrorMessage)
                        _lastResponseCode = consentResponse?.StatusCode
                        If Not (consentResponse?.IsSuccessStatusCode) Then
                            _lastErrorMessage = consentResponse.ReasonPhrase
                            _lastResponseCode = consentResponse.StatusCode
                            Return False
                        End If
                    End Using
                End Using
            End Using

            Dim authToken As String = Me.GetBearerToken(GetServerUrl(Me.CareLinkCountry))

            ' MUST BE FIRST DO NOT MOVE NEXT LINE
            s_sessionCountrySettings = New CountrySettingsRecord(Me.GetCountrySettings(authToken))
            Me.SessionUser = Me.GetMyUser(authToken)
            Me.SessionProfile = Me.GetMyProfile(authToken)
            _sessionMonitorData = Me.GetMonitorData(authToken)
            'Dim reports As Object = Me.GetReports(authToken)
            ' Set login success if everything was OK:
            If Me.SessionUser.HasValue _
               AndAlso Me.SessionProfile.HasValue _
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

    Private Function GetAuthorizationToken(ByRef authToken As String) As GetAuthorizationTokenResult
        If NetworkUnavailable() Then
            _lastErrorMessage = "No Internet Connection!"
            Return GetAuthorizationTokenResult.NetworkDown
        End If

        Dim serverUrl As String = GetServerUrl(Me.CareLinkCountry)
        ' New token is needed:
        ' a) no token or about to expire => execute authentication
        ' b) last response 401
        If Me.GetCookieValue(serverUrl, CareLinkAuthTokenCookieName) Is Nothing OrElse
            Me.GetCookies(serverUrl)?.Item(CareLinkTokenValidToCookieName)?.Value Is Nothing OrElse
            New List(Of Object)() From {401, 403}.Contains(_lastResponseCode) Then
            ' TODO: add check for expired token
            ' execute new login process | null, if error OR already doing login
            'if loginInProcess or not executeLoginProcedure():
            If _inLoginInProcess Then
                Debug.Print("Already In login Process")
                Return GetAuthorizationTokenResult.InLoginProcess
            End If
            If Not Me.ExecuteLoginProcedure(serverUrl) Then
                If NetworkUnavailable() Then
                    _lastErrorMessage = "No Internet Connection!"
                    Debug.Print("No Internet Connection!")
                    Return GetAuthorizationTokenResult.NetworkDown
                End If
                Debug.Print("__executeLoginProcedure failed")
                Return GetAuthorizationTokenResult.LoginFailed
            End If
            Debug.Print($"auth_token_validTo = {Me.GetCookies(serverUrl).Item(CareLinkTokenValidToCookieName).Value}")
        End If
        ' there can be only one
        authToken = Me.GetBearerToken(serverUrl)
        Return GetAuthorizationTokenResult.OK
    End Function

    ' Periodic data from CareLink Cloud
    Private Function GetConnectDisplayMessage(username As String, role As String, endpointUrl As String, Optional patientUserName As String = "") As Dictionary(Of String, String)

        Debug.Print("__getConnectDisplayMessage()")
        ' Build user Json for request
        Dim userJson As New Dictionary(Of String, String) From {
            {"username", username},
            {"role", role}}

        If role.Equals("CarePartner", StringComparison.InvariantCultureIgnoreCase) Then
            If String.IsNullOrWhiteSpace(patientUserName) Then
                endpointUrl = endpointUrl.Replace("v6", "v5")
            Else
                userJson.Add("patientId", patientUserName)
            End If
        End If
        Return Me.GetData(endpointUrl, userJson)
    End Function

    Private Function GetCookies(url As String) As CookieCollection
        Return If(String.IsNullOrWhiteSpace(url),
                  Nothing,
                  Me.ClientHandler.CookieContainer.GetCookies(New Uri($"https://{url}"))
                 )
    End Function

    Private Function GetCookieValue(serverUrl As String, cookieName As String) As String
        If String.IsNullOrWhiteSpace(serverUrl) Then
            Return Nothing
        End If
        Dim cookie As Cookie = Me.ClientHandler.CookieContainer.GetCookies(New Uri($"https://{serverUrl}")).Cast(Of Cookie)().FirstOrDefault(Function(c As Cookie) c.Name = cookieName)
        Return cookie?.Value
    End Function

    Private Function GetCountrySettings(authToken As String) As Dictionary(Of String, String)
        Debug.Print("__getCountrySettings()")
        Dim queryParams As New Dictionary(Of String, String) From {
            {"countryCode", Me.CareLinkCountry},
            {"language", "en"}}

        Return Me.GetData(authToken, GetServerUrl(Me.CareLinkCountry), "patient/countries/settings", queryParams, Nothing)
    End Function

    Private Function GetLoginSession(serverUrl As String) As HttpResponseMessage
        ' https://CareLink.MiniMed.com/patient/sso/login?country=us&lang=en
        Dim requestUri As New StringBuilder($"https://{serverUrl}/patient/sso/login")
        Dim payload As New Dictionary(Of String, String) From {
                            {"country", Me.CareLinkCountry},
                            {"lang", "en"}
                        }
        Dim response As HttpResponseMessage = Nothing

        Try
            response = _httpClient.Get(requestUri, _lastErrorMessage, s_commonHeaders, payload)
        Catch ex As Exception
            If NetworkUnavailable() Then
                _lastErrorMessage = "No Internet Connection!"
                Debug.Print("No Internet Connection!")
                Return response
            End If
            Debug.Print($"__getLoginSession() failed {ex.DecodeException().Replace(vbCrLf, "")}")
            Return response
        End Try

        Return response
    End Function

    Private Function GetMonitorData(authToken As String) As SessionMonitorDataRecord
        Debug.Print(NameOf(GetMonitorData))
        Dim sessionMonitorData As New SessionMonitorDataRecord(Me.GetData(authToken, GetServerUrl(Me.CareLinkCountry), "patient/monitor/data", Nothing, Nothing))
        Return sessionMonitorData
    End Function

    Private Function GetMyProfile(authToken As String) As SessionProfileRecord
        Debug.Print(NameOf(GetMyProfile))
        Dim sessionProfile As New SessionProfileRecord(Me.GetData(authToken, GetServerUrl(Me.CareLinkCountry), "patient/users/me/profile", Nothing, Nothing))
        Return sessionProfile
    End Function

    Private Function GetMyUser(authToken As String) As SessionUserRecord
        Debug.Print(NameOf(GetMyUser))
        Dim myUserRecord As New SessionUserRecord(Form1.DgvCurrentUser, Me.GetData(authToken, GetServerUrl(Me.CareLinkCountry), "patient/users/me", Nothing, Nothing))
        Return myUserRecord
    End Function

    Private Function NewHttpClientWithCookieContainer() As HttpClient
        Dim cookieContainer As New CookieContainer()
        Me.ClientHandler = New HttpClientHandler With {.CookieContainer = cookieContainer}
        Return New HttpClient(Me.ClientHandler)
    End Function

    Friend Function GetData(authToken As String, host As String, endPointPath As String, queryParams As Dictionary(Of String, String), requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Debug.Print($"GetData(serverUrl = {host}, endPointPath = {endPointPath}, queryParams = {queryParams.ToCsv}, requestBody = {requestBody.ToCsv}")
        Dim jsonData As Dictionary(Of String, String) = Nothing
        If authToken IsNot Nothing Then
            Dim response As New HttpResponseMessage
            Try
                ' Add header
                Dim headers As Dictionary(Of String, String) = s_commonHeaders.Clone
                headers("Authorization") = authToken

                Dim requestUri As New StringBuilder(If(host Is Nothing, endPointPath, $"https://{host}/{endPointPath}"))
                If requestBody Is Nothing OrElse requestBody.Count = 0 Then
                    headers("Accept") = "application/json, text/plain, */*"
                    headers("Content-Type") = "application/json; charset=utf-8"
                    response = _httpClient.Get(requestUri, _lastErrorMessage, headers, params:=queryParams)
                    _lastResponseCode = response.StatusCode
                Else
                    headers("Accept") = "application/json, text/plain, */*"
                    headers("Content-Type") = "application/json; charset=utf-8"
                    _httpClient.DefaultRequestHeaders.Clear()
                    For Each header As KeyValuePair(Of String, String) In headers
                        If header.Key <> "Content-Type" Then
                            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                        End If
                    Next
                    Dim jsonContent As Json.JsonContent = Json.JsonContent.Create(requestBody)
                    Dim postRequest As New HttpRequestMessage(HttpMethod.Post, New Uri(requestUri.ToString)) With {.Content = jsonContent}
                    response = _httpClient.SendAsync(postRequest).Result
                    _lastResponseCode = response.StatusCode
                End If
                If response?.IsSuccessStatusCode Then
                    Dim resultText As String = response.ResultText
                    jsonData = Loads(resultText)
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

    Friend Function GetDeviceSettings() As String
        Dim requestPayload As New Dictionary(Of String, Object) From {
            {"clientTime", $"""{Now:O}"""},
                {"dailyDetailReportDays", "[]"},
                {"endDate", $"""{Now.Year}-{Now.Month:D2}-{Now.Day:D2}"""},
                {"patientId", $"""{Form1.Client.SessionUser.id}"""},
                {"reportFileFormat", """PDF"""},
                {"reportShowAdherence", False},
                {"reportShowAssessmentAndProgress", False},
                {"reportShowBolusWizardFoodBolus", False},
                {"reportShowDashBoard", False},
                {"reportShowDataTable", False},
                {"reportShowDeviceSettings", True},
                {"reportShowEpisodeSummary", False},
                {"reportShowLogbook", False},
                {"reportShowOverview", False},
                {"reportShowWeeklyReview", False},
                {"startDate", $"""{Now.Year}-{Now.Month:D2}-{Now.Day:D2}"""}
            }
        Dim authToken As String = ""
        Dim deviceSettings As String = ""
        Dim jsonData As Dictionary(Of String, String)
        If Me.GetAuthorizationToken(authToken) = GetAuthorizationTokenResult.OK Then
            Dim response As HttpResponseMessage = Nothing
            Try
                ' Add header
                Dim headers As Dictionary(Of String, String) = s_commonHeaders.Clone
                headers("Authorization") = authToken
                headers.Add("Origin", "https://carelink.minimed.com")
                headers.Add("Host", "carelink.minimed.com")
                headers("Accept") = "application/json, text/plain, */*"
                headers.Add("Accept-Encoding", "gzip, deflate, br")
                _httpClient.DefaultRequestHeaders.Clear()
                For Each header As KeyValuePair(Of String, String) In headers.Sort
                    If header.Key <> "Content-Type" Then
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                    End If
                Next
                _httpClient.DefaultRequestHeaders.Referrer = New Uri("https://carelink.minimed.com/app/reports")

                Dim jsonContent As Json.JsonContent = Json.JsonContent.Create(requestPayload)
                Dim requestUri As New Uri($"https://{GetServerUrl(Me.CareLinkCountry)}/patient/reports/generateReport")
                Dim postRequest As New HttpRequestMessage(HttpMethod.Post, requestUri) With {.Content = jsonContent}
                ' Bad Request !!!
                response = _httpClient.SendAsync(postRequest).Result

                _lastResponseCode = response.StatusCode
                If response?.IsSuccessStatusCode Then
                    Dim resultText As String = response.ResultText
                    jsonData = Loads(resultText)
                    response.Dispose()
                Else
                    Return ""
                End If
            Catch ex As Exception
                _lastErrorMessage = ex.DecodeException()
                Debug.Print($"{NameOf(GetDeviceSettings)} {_lastErrorMessage.Replace(vbCrLf, "")}, status {response?.StatusCode}")
            End Try
            Try
                Dim requestUri As New StringBuilder($"https://{GetServerUrl(Me.CareLinkCountry)}/app/reportStatus")
                Dim lastError As String = ""
                Dim saveFileNameJson As String = ""
                While True
                    Dim headers As Dictionary(Of String, String) = s_commonHeaders.Clone
                    headers("Authorization") = authToken
                    headers("Accept") = "application/json, text/plain, */*"
                    Dim downloadPayload As New Dictionary(Of String, String) From {
                        {"saveFileName", ""}
                        }
                    response = _httpClient.Get(requestUri, lastError, headers, downloadPayload)
                    If response.Headers.Contains("Content-Disposition") Then
                        For Each header As KeyValuePair(Of String, IEnumerable(Of String)) In response.Headers.AsEnumerable
                            If header.Key = "Content-Disposition" Then
                                saveFileNameJson = header.Value.First
                                Exit While
                            End If
                        Next
                    End If
                End While
                requestUri = New StringBuilder($"https://{GetServerUrl(Me.CareLinkCountry)}/app/reports/ReportStatus")
                Dim fileContents As Byte() = response.Content.ReadAsByteArrayAsync.Result
                Dim tmpPath As String = Path.GetTempPath
                Dim fileName As String = Loads(saveFileNameJson)("filename").Replace("%20", " ")
                ByteArrayToFile($"{tmpPath}{fileName}", fileContents)
                deviceSettings = Encoding.UTF8.GetString(fileContents)
                Stop
            Catch ex As Exception
                If NetworkUnavailable() Then
                    Debug.Print($"{NameOf(GetDeviceSettings)} has no Internet Connection!")
                    Return ""
                End If
                Debug.Print($"{NameOf(GetDeviceSettings)} failed {ex.DecodeException().Replace(vbCrLf, "")}")
                Return ""
            End Try
        End If
        Stop
        Return deviceSettings
    End Function

    Public Overridable Function GetLastErrorMessage() As String
        Return If(_lastErrorMessage, "OK")
    End Function

    ' Wrapper for data retrieval methods
    Public Overridable Function GetRecentData() As Dictionary(Of String, String)
        If NetworkUnavailable() Then
            _lastErrorMessage = "No Internet Connection!"
            ReportLoginStatus(Form1.LoginStatus)
            Debug.Print("No Internet Connection!")
            Return Nothing
        End If

        ' Force login to get basic info
        Try
            Dim authToken As String = Nothing
            If Me.GetAuthorizationToken(authToken) = GetAuthorizationTokenResult.OK AndAlso
               ((s_sessionCountrySettings.HasValue AndAlso Not String.IsNullOrWhiteSpace(Me.CareLinkCountry)) OrElse
               _sessionMonitorData.deviceFamily?.Equals("BLE_X", StringComparison.Ordinal)) Then
                Return If(_careLinkPartnerType.Contains(Me.SessionUser.role, StringComparer.InvariantCultureIgnoreCase),
                          Me.GetConnectDisplayMessage(
                                        Me.SessionProfile.username,
                                        "CarePartner".ToLower,
                                        s_sessionCountrySettings.blePereodicDataEndpoint,
                                        My.Settings.CareLinkPatientUserID),
                          Me.GetConnectDisplayMessage(
                                        Me.SessionProfile.username,
                                        "patient",
                                        s_sessionCountrySettings.blePereodicDataEndpoint)
                                       )
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

    Private Function GetData(endPointPath As String, requestBody As Dictionary(Of String, String)) As Dictionary(Of String, String)
        ' Get authorization token
        Dim authToken As String = Nothing
        Select Case Me.GetAuthorizationToken(authToken)
            Case GetAuthorizationTokenResult.OK
                Dim jsonDictionary As Dictionary(Of String, String) = Me.GetData(authToken, Nothing, endPointPath, Nothing, requestBody)
                If _lastResponseCode <> HttpStatusCode.OK Then
                    ReportLoginStatus(Form1.LoginStatus)
                    _httpClient = Me.NewHttpClientWithCookieContainer
                End If
                Return jsonDictionary
            Case GetAuthorizationTokenResult.NetworkDown
                ReportLoginStatus(Form1.LoginStatus)
            Case GetAuthorizationTokenResult.InLoginProcess
                Exit Select
            Case GetAuthorizationTokenResult.LoginFailed
                ReportLoginStatus(Form1.LoginStatus, True, _lastErrorMessage)
        End Select
        Return Nothing
    End Function

End Class
