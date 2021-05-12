Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Json
Imports System.Text
Imports System.Text.Json

Public Module carelink_client
    Private Const VERSION As Object = "0.1"
    Private Const CARELINK_CONNECT_SERVER_EU As String = "carelink.minimed.eu"
    Private Const CARELINK_CONNECT_SERVER_US As String = "carelink.minimed.com"
    Private Const CARELINK_LANGUAGE_EN As String = "en"
    Private Const CARELINK_LOCALE_EN As String = "en"
    Private Const CARELINK_AUTH_TOKEN_COOKIE_NAME As String = "auth_tmp_token"
    Private Const CARELINK_TOKEN_VALIDTO_COOKIE_NAME As String = "c_token_valid_to"
    Private Const AUTH_EXPIRE_DEADLINE_MINUTES As Integer = 1

    Public Class CareLinkClient
        Inherits Object


        Private ReadOnly __MainForm As Form1
        Private ReadOnly __carelinkUsername As String
        Private ReadOnly __carelinkPassword As String
        Private ReadOnly __carelinkCountry As String
        Private __sessionUser As Dictionary(Of String, String)
        Private __sessionProfile As Dictionary(Of String, String)
        Private __sessionCountrySettings As Dictionary(Of String, String)
        Private __sessionMonitorData As Dictionary(Of String, String)
        Private __loginInProcess As Boolean
        Private __loggedIn As Boolean
        Private __lastDataSuccess As Boolean
        Private __lastResponseCode As HttpStatusCode
        Private __lastErrorMessage As String
        Private ReadOnly __commonHeaders As Dictionary(Of String, String)
        Private ReadOnly __httpClient As HttpClient

        Public Sub printdbg(msg As String)
#If DEBUG Then
            Console.WriteLine(msg)
            Me.__MainForm.RichTextBox1.AppendText($"{msg}{vbCrLf}")
#End If
        End Sub

        Public Sub New(MainForm As Form1, carelinkUsername As String, carelinkPassword As String, carelinkCountry As String)
            Me.__MainForm = MainForm
            ' User info
            Me.__carelinkUsername = carelinkUsername
            Me.__carelinkPassword = carelinkPassword
            Me.__carelinkCountry = carelinkCountry
            ' Session info
            Me.__sessionUser = Nothing
            Me.__sessionProfile = Nothing
            Me.__sessionCountrySettings = Nothing
            Me.__sessionMonitorData = Nothing
            ' State info
            Me.__loginInProcess = False
            Me.__loggedIn = False
            Me.__lastDataSuccess = False
            Me.__lastResponseCode = Nothing
            Me.__lastErrorMessage = Nothing
            Me.__commonHeaders = New Dictionary(Of String, String) From {
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
            Me.__httpClient = New HttpClient()
        End Sub

        Public Overridable Function getLastDataSuccess() As Object
            Return Me.__lastDataSuccess
        End Function

        Public Overridable Function getLastResponseCode() As HttpStatusCode
            Return Me.__lastResponseCode
        End Function

        Public Overridable Function getLastErrorMessage() As String
            Return Me.__lastErrorMessage
        End Function

        ' Get server URL
        Public Overridable Function __careLinkServer() As String
            Return If(Me.__carelinkCountry = "us", CARELINK_CONNECT_SERVER_US, CARELINK_CONNECT_SERVER_EU)
        End Function

        Public Overridable Function __extractResponseData(responseBody As String, begstr As String, endstr As String) As String
            Dim beg As Integer = responseBody.IndexOf(begstr, StringComparison.Ordinal) + begstr.Length
            Dim [end] As Integer = responseBody.IndexOf(endstr, beg, StringComparison.Ordinal)
            Return responseBody.Substring(beg, [end] - beg).Replace("""", "")
        End Function

        Public Overridable Function __getLoginSessionAsync() As HttpResponseMessage
            ' https://carelink.minimed.com/patient/sso/login?country=us&lang=en
            Dim url As String = "https://" & Me.__careLinkServer & "/patient/sso/login"
            Dim payload As New Dictionary(Of String, String) From {
                {
                    "country",
                    Me.__carelinkCountry},
                {
                    "lang",
                    CARELINK_LANGUAGE_EN}
                    }
            Dim response As HttpResponseMessage = Nothing

            Try
                response = Me.__httpClient.Get(url, headers:=Me.__commonHeaders, params:=payload)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                printdbg(e.Message)
                printdbg("__getLoginSession() failed")
            End Try

            Return response
        End Function

        Public Overridable Function __doLogin(loginSessionResponse As HttpResponseMessage) As HttpResponseMessage

            Dim queryParameters As Dictionary(Of String, String) = parse_qsl(loginSessionResponse)
            Dim url As String = "https://mdtlogin.medtronic.com" & "/mmcl/auth/oauth/v2/authorize/login"
            Dim payload As New Dictionary(Of String, String) From {
                {
                    "country",
                    Me.__carelinkCountry},
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
                    Me.__carelinkUsername},
                {
                    "password",
                    Me.__carelinkPassword},
                {
                    "actionButton",
                    "Log in"}}
            Dim response As HttpResponseMessage = Nothing
            Try
                response = Me.__httpClient.Post(url, Me.__commonHeaders, params:=payload, data:=form)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                printdbg(e.Message)
                printdbg("__doLogin() failed")
            End Try

            Return response
        End Function

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

        Public Overridable Function __doConsent(doLoginResponse As HttpResponseMessage) As HttpResponseMessage

            ' Extract data for consent
            Dim doLoginRespBody As String = doLoginResponse.Text
            Dim url As String = Me.__extractResponseData(doLoginRespBody, "<form action=", " ")
            Dim sessionID As String = Me.__extractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionID"" value=", ">")
            Dim sessionData As String = Me.__extractResponseData(doLoginRespBody, "<input type=""hidden"" name=""sessionData"" value=", ">")
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
            Dim consentHeaders As Dictionary(Of String, String) = Me.__commonHeaders
            consentHeaders("Content-Type") = "application/x-www-form-urlencoded"
            Dim response As HttpResponseMessage = Nothing
            Try
                response = Me.__httpClient.Post(url, _headers:=consentHeaders, data:=form)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                printdbg(e.Message)
                printdbg("__doConsent() failed")
            End Try

            Return response
        End Function

        Public Overridable Function __getData(host As String, path As String, queryParams As Dictionary(Of String, String), requestBody As String) As Dictionary(Of String, String)
            Dim url As String
            printdbg("__getData()")
            Me.__lastDataSuccess = False
            If host Is Nothing Then
                url = path
            Else
                url = $"https://{host}/{path}"
            End If
            Dim payload As Dictionary(Of String, String) = queryParams
            Dim data As New Dictionary(Of String, String)
            If requestBody IsNot Nothing Then
                data = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(requestBody)
            End If

            Dim jsondata As Dictionary(Of String, String) = Nothing
            ' Get auth token
            Dim authToken As String = Me.__getAuthorizationToken()
            If authToken IsNot Nothing Then
                Try
                    ' Add header
                    Dim headers As Dictionary(Of String, String) = Me.__commonHeaders
                    headers("Authorization") = authToken
                    If data.Count = 0 Then
                        headers("Accept") = "application/json, text/plain, */*"
                        headers("Content-Type") = "application/json; charset=utf-8"
                        Dim response As HttpResponseMessage = Me.__httpClient.Get(url, headers:=headers, params:=payload)
                        response.EnsureSuccessStatusCode()

                        Dim responseBody As String = response.Text()
                        'jsondata = Json.loads(response.Text)
                        Me.__lastResponseCode = response.StatusCode
                        If Not response.StatusCode = HttpStatusCode.OK Then
                            Throw New Exception("session get response is not OK")
                        End If
                    Else
                        headers("Accept") = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"
                        headers("Content-Type") = "application/x-www-form-urlencoded"

                        'response = Me.__httpClient.post(url, headers:=headers, data:=data)
                        Using request As New HttpRequestMessage(HttpMethod.Post, url)
                            Dim jsonString As String = JsonSerializer.Serialize(data)
                            Using stringContent As New StringContent(jsonString, Encoding.UTF8, "application/json")
                                request.Content = stringContent

                                Using response As HttpResponseMessage = Me.__httpClient.Post(url, _headers:=headers, data:=data)
                                    response.EnsureSuccessStatusCode()
                                    Me.__lastResponseCode = response.StatusCode
                                    If Not response.StatusCode = HttpStatusCode.OK Then
                                        printdbg(response.StatusCode.ToString())
                                        Throw New Exception("session post response is not OK")
                                    End If
                                    'jsondata = Json.loads(response.Text)
                                    Me.__lastDataSuccess = True
                                End Using
                            End Using
                        End Using
                    End If
                Catch e As Exception
                    printdbg(e.Message)
                    printdbg("__getData() failed")
                End Try
            End If
            Return jsondata
        End Function

        Public Overridable Function __getMyUser() As Dictionary(Of String, String)
            printdbg("__getMyUser()")
            Dim result As Object = Me.__getData(Me.__careLinkServer(), "patient/users/me", Nothing, Nothing)
            Return CType(result, Dictionary(Of String, String))
        End Function

        Public Overridable Function __getMyProfile() As Dictionary(Of String, String)
            printdbg("__getMyProfile()")
            Dim result As Object = Me.__getData(Me.__careLinkServer(), "patient/users/me/profile", Nothing, Nothing)
            Return CType(result, Dictionary(Of String, String))
        End Function

        Public Overridable Function __getCountrySettings(country As String, language As String) As Dictionary(Of String, String)
            printdbg("__getCountrySettings()")
            Dim queryParams As New Dictionary(Of String, String) From {
                {
                    "countryCode",
                    country},
                {
                    "language",
                    language}}
            Dim result As Object = Me.__getData(Me.__careLinkServer(), "patient/countries/settings", queryParams, Nothing)
            Return CType(result, Dictionary(Of String, String))
        End Function

        Public Overridable Function __getMonitorData() As Dictionary(Of String, String)
            printdbg("__getMonitorData()")
            Dim result As Object = Me.__getData(Me.__careLinkServer(), "patient/monitor/data", Nothing, Nothing)
            Return CType(result, Dictionary(Of String, String))
        End Function

        ' Old last24hours webapp data
        Public Overridable Function __getLast24Hours() As Object
            printdbg("__getLast24Hours")
            Dim queryParams As New Dictionary(Of String, String) From {
                {
                    "cpSerialNumber",
                    "NONE"},
                {
                    "msgType",
                    "last24hours"},
                {
                    "requestTime",
                    Convert.ToInt32((DateTime.UtcNow - New DateTime(1970, 1, 1)) * 1000).ToString()}
                }
            Return Me.__getData(Me.__careLinkServer(), "patient/connect/data", queryParams, Nothing)
        End Function

        ' Periodic data from CareLink Cloud
        Public Overridable Function __getConnectDisplayMessage(username As String, role As String, endpointUrl As String) As Object

            printdbg("__getConnectDisplayMessage()")
            ' Build user json for request
            Dim userJson As New Dictionary(Of String, String) From {
                {
                    "username",
                    username},
                {
                    "role",
                    role}}
            Dim requestBody As String = dumps(userJson)
            Dim recentData As Object = Me.__getData(Nothing, endpointUrl, Nothing, requestBody)
            If recentData IsNot Nothing Then
                Me.__correctTimeInRecentData(recentData)
            End If

            Return recentData
        End Function

        Public Overridable Function __correctTimeInRecentData(recentData As Object) As Boolean
            ' TODO
        End Function

        Public Overridable Function __executeLoginProcedure() As Boolean
            Dim lastLoginSuccess As Boolean = False
            Me.__loginInProcess = True
            Me.__lastErrorMessage = Nothing
            Try
                ' Clear cookies
                Me.__httpClient.DefaultRequestHeaders.Clear()
                ' Clear basic infos
                Me.__sessionUser = Nothing
                Me.__sessionProfile = Nothing
                Me.__sessionCountrySettings = Nothing
                Me.__sessionMonitorData = Nothing
                ' Open login(get SessionId And SessionData)
                Dim loginSessionResponse As HttpResponseMessage = Me.__getLoginSessionAsync()
                Me.__lastResponseCode = loginSessionResponse.StatusCode
                ' Login
                Dim doLoginResponse As HttpResponseMessage = Me.__doLogin(loginSessionResponse)
                Me.__lastResponseCode = doLoginResponse.StatusCode
                'setLastResponseBody(loginSessionResponse)
                loginSessionResponse.Dispose()
                ' Consent
                Dim consentResponse As HttpResponseMessage = Me.__doConsent(doLoginResponse)
                Me.__lastResponseCode = consentResponse.StatusCode
                'setLastResponseBody(consentResponse);
                doLoginResponse.Dispose()
                consentResponse.Dispose()
                ' Get sessions infos if required
                If Me.__sessionUser Is Nothing Then
                    Me.__sessionUser = Me.__getMyUser()
                End If
                If Me.__sessionProfile Is Nothing Then
                    Me.__sessionProfile = Me.__getMyProfile()
                End If
                If Me.__sessionCountrySettings Is Nothing Then
                    Me.__sessionCountrySettings = Me.__getCountrySettings(Me.__carelinkCountry, CARELINK_LANGUAGE_EN)
                End If
                If Me.__sessionMonitorData Is Nothing Then
                    Me.__sessionMonitorData = Me.__getMonitorData()
                End If
                ' Set login success if everything was ok:
                If Me.__sessionUser IsNot Nothing AndAlso Me.__sessionProfile IsNot Nothing AndAlso Me.__sessionCountrySettings IsNot Nothing AndAlso Me.__sessionMonitorData IsNot Nothing Then
                    lastLoginSuccess = True
                End If
            Catch e As Exception
                printdbg(e.Message)
                Me.__lastErrorMessage = e.Message
            End Try
            Me.__loginInProcess = False
            Me.__loggedIn = lastLoginSuccess
            Return lastLoginSuccess
        End Function
        Private Async Function GetCookieValue(cookieName As String) As Task(Of String)
            Dim uri As Uri = Me.__httpClient.BaseAddress
            If uri Is Nothing Then
                Return Nothing
            End If
            Dim cookieContainer As New CookieContainer()
            Using httpClientHandler As New HttpClientHandler With {.CookieContainer = cookieContainer}
                Using httpClient As New HttpClient(httpClientHandler)
                    Await httpClient.GetAsync(uri)
                    Dim cookie As Cookie = cookieContainer.GetCookies(uri).Cast(Of Cookie)().FirstOrDefault(Function(x) x.Name = cookieName)
                    Return cookie?.Value
                End Using
            End Using
        End Function

        Private Async Function GetCookies() As Task(Of CookieCollection)
            Dim uri As Uri = Me.__httpClient.BaseAddress
            If uri Is Nothing Then
                Return Nothing
            End If
            Dim cookieContainer As New CookieContainer()
            Using httpClientHandler As New HttpClientHandler With {.CookieContainer = cookieContainer}
                Using httpClient As New HttpClient(httpClientHandler)
                    Await httpClient.GetAsync(uri)
                    Return cookieContainer.GetCookies(uri)
                End Using
            End Using
        End Function
        Public Overridable Function __getAuthorizationToken() As String
            Dim auth_token As String = GetCookieValue(CARELINK_AUTH_TOKEN_COOKIE_NAME).Result
            Dim auth_token_validto As String = GetCookies.Result?.Item(CARELINK_TOKEN_VALIDTO_COOKIE_NAME).Value
            ' New token is needed:
            ' a) no token or about to expire => execute authentication
            ' b) last response 401
            If auth_token Is Nothing OrElse auth_token_validto Is Nothing OrElse New List(Of Object) From {
                401,
                403
            }.Contains(Me.__lastResponseCode) Then
                ' TODO: add check for expired token
                ' execute new login process | null, if error OR already doing login
                'if loginInProcess or not executeLoginProcedure():
                If Me.__loginInProcess Then
                    printdbg("loginInProcess")
                    Return Nothing
                End If
                If Not Me.__executeLoginProcedure() Then
                    printdbg("__executeLoginProcedure failed")
                    Return Nothing
                End If
                printdbg($"auth_token_validto = {GetCookies.Result.Item(CARELINK_TOKEN_VALIDTO_COOKIE_NAME).Value}")
            End If
            ' there can be only one
            Return $"Bearer {GetCookieValue(CARELINK_AUTH_TOKEN_COOKIE_NAME).Result}"
        End Function

        ' Wrapper for data retrieval methods
        Public Overridable Function getRecentData() As Object
            ' Force login to get basic info
            If Me.__getAuthorizationToken() IsNot Nothing Then
                If Me.__carelinkCountry = "us" OrElse Me.__sessionMonitorData("deviceFamily") = "BLE_X" Then
                    Dim role As String = If(New List(Of Object) From {
                        "CARE_PARTNER",
                        "CARE_PARTNER_OUS"
                    }.Contains(Me.__sessionUser("role")), "carepartner", "patient")
                    Return Me.__getConnectDisplayMessage(Me.__sessionProfile("username"), role, Me.__sessionCountrySettings("blePereodicDataEndpoint"))
                Else
                    Return Me.__getLast24Hours()
                End If
            Else
                Return Nothing
            End If
        End Function

        ' Authentication methods
        Public Overridable Function login() As Object
            If Not Me.__loggedIn Then
                Me.__executeLoginProcedure()
            End If

            Return Me.__loggedIn
        End Function
#If False Then

        Sub Main(args As String())
            Dim request As HttpWebRequest = TryCast(WebRequest.Create("https://api.openrouteservice.org/v2/directions/driving-car/gpx"), HttpWebRequest)

            request.Method = "POST"

            request.Accept = "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8"
            request.Headers.Add("Authorization", "xxxxMYKEYxxxx")
            request.Headers.Add("Content-Type", "application/json; charset=utf-8")

            Using writer As New StreamWriter(request.GetRequestStream())
                Dim byteArray As Byte() = Text.Encoding.UTF8.GetBytes("coordinates:[[8.681495,49.41461],[8.686507,49.41943],[8.687872,49.420318]]")
                request.ContentLength = byteArray.Length
                writer.Write(byteArray)
                writer.Close()
            End Using
            Dim responseContent As String
            Using response As HttpWebResponse = TryCast(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    responseContent = reader.ReadToEnd()
                    Console.WriteLine(responseContent.ToString())
                End Using
            End Using
        End Sub

        Private Function SendRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, method As String) As String
            Dim response As String
            Dim post_str As String = ""   'What needs to be sent goes in this variable.
            Dim CookieJar As New CookieContainer()    'The CookieContainer that will keep all the cookies.
            'DO NOT CLEAR THIS BETWEEN REQUESTS! ONLY CLEAR TO "Log Out".

            Dim req As HttpWebRequest = CType(HttpWebRequest.Create("<login URL goes here>"), HttpWebRequest)

            req.Method = "POST"
            req.Accept = "text/html, application/xhtml+xml, */*"   'This may be a bit different in your case. Refer to what Fiddler will say.
            req.CookieContainer = CookieJar
            req.ContentLength = post_str.Length
            req.ContentType = "application/x-www-form-urlencoded"    'Also, any useragent will do.
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1"
            req.Headers.Add("Accept-Language", "en-US,en;q=0.7,ru;q=0.3")
            req.Headers.Add("Accept-Encoding", "gzip, deflate")
            req.Headers.Add("DNT", "1")       'Make sure to add all headers that you found using Fiddler!
            req.Headers.Add("Pragma", "no-cache")

            Dim RequestWriter As New IO.StreamWriter(req.GetRequestStream())
            RequestWriter.Write(post_str)      'Write the post string that contains the log in, password, etc.
            RequestWriter.Close()
            RequestWriter.Dispose()

            Dim ResponceReader As New IO.StreamReader(req.GetResponse().GetResponseStream())
            Dim ResponceData As String = ResponceReader.ReadToEnd()
            ResponceReader.Close()
            ResponceReader.Dispose()

            req.GetResponse.Close()

            'In the long run, you can check the ResponceData to verify that the log in was successful.
            'Dim response As String
            Dim request As WebRequest

            request = WebRequest.Create(uri)
            request.ContentLength = jsonDataBytes.Length
            request.ContentType = contentType
            request.Method = method

            Using requestStream As Stream = request.GetRequestStream
                requestStream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
                requestStream.Close()

                Using responseStream As Stream = request.GetResponse.GetResponseStream
                    Using reader As New StreamReader(responseStream)
                        response = reader.ReadToEnd()
                    End Using
                End Using
            End Using


            req = CType(HttpWebRequest.Create("<page to send request to goes here>"), HttpWebRequest)

            req.Method = "POST"
            req.Accept = "text/html, application/xhtml+xml, */*"   'May be different in your case
            req.CookieContainer = CookieJar    'Please note: this HAS to be the same CookieJar as you used to login.
            req.ContentLength = post_str.Length
            req.ContentType = "application/x-www-form-urlencoded"
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1"
            req.Headers.Add("Accept-Language", "en-US,en;q=0.7,ru;q=0.3")
            req.Headers.Add("Accept-Encoding", "gzip, deflate")
            req.Headers.Add("DNT", "1")     'Add all headers here.
            req.Headers.Add("Pragma", "no-cache")

            RequestWriter = New IO.StreamWriter(req.GetRequestStream())
            RequestWriter.Write(post_str)
            RequestWriter.Close()
            RequestWriter.Dispose()

            ResponceReader = New IO.StreamReader(req.GetResponse().GetResponseStream())
            ResponceData = ResponceReader.ReadToEnd()
            ResponceReader.Close()
            ResponceReader.Dispose()

            req.GetResponse.Close()

            'You may want to read the ResponceData.
            Return response
        End Function
#End If

    End Class

End Module
