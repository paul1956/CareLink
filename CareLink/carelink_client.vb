Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Windows.Forms.VisualStyles

Imports parse_qsl = urllib.parse.parse_qsl
Imports urlparse = urllib.parse.urlparse

Public Module carelink_client
    Public VERSION As Object = "0.1"
    Public CARELINK_CONNECT_SERVER_EU As String = "carelink.minimed.eu"
    Public CARELINK_CONNECT_SERVER_US As String = "carelink.minimed.com"
    Public CARELINK_LANGUAGE_EN As String = "en"
    Public CARELINK_LOCALE_EN As String = "en"
    Public CARELINK_AUTH_TOKEN_COOKIE_NAME As String = "auth_tmp_token"
    Public CARELINK_TOKEN_VALIDTO_COOKIE_NAME As String = "c_token_valid_to"
    Public AUTH_EXPIRE_DEADLINE_MINUTES As Integer = 1
    Public DEBUG As Boolean = False

    Public Sub printdbg(msg As Object)
#If DEBUG Then
        Console.WriteLine(msg)
#End If
    End Sub

    Public Class CareLinkClient
        Inherits Object

        Private __carelinkUsername As String
        Private __carelinkPassword As String
        Private __carelinkCountry As String
        Private __sessionUser As Dictionary(Of String, String)
        Private __sessionProfile As Dictionary(Of String, String)
        Private __sessionCountrySettings As Dictionary(Of String, String)
        Private __sessionMonitorData As Dictionary(Of String, String)
        Private __loginInProcess As Boolean
        Private __loggedIn As Boolean
        Private __lastDataSuccess As Boolean
        Private __lastResponseCode As HttpStatusCode
        Private __lastErrorMessage As String
        Private __commonHeaders As Dictionary(Of String, String)
        Private __httpClient As HttpClient

        Public Sub New(carelinkUsername As String, carelinkPassword As String, carelinkCountry As String)
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
            ' Create main http client session with CookieJar
            Me.__httpClient = requests.Session()
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
            Dim beg As Integer = responseBody.IndexOf(begstr, StringComparison.Ordinal) + begstr.Count
            Dim [end] As Integer = responseBody.IndexOf(endstr, beg, StringComparison.Ordinal)
            Return responseBody.Substring(beg, [end] - beg).Replace("""", "")
        End Function

        Public Overridable Function __getLoginSessionAsync() As HttpResponseMessage
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
                Dim uri As New Uri(url)
                Me.__httpClient.BaseAddress = uri
                For Each header As KeyValuePair(Of String, String) In Me.__commonHeaders
                    Me.__httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                Next
                For Each header As KeyValuePair(Of String, String) In payload
                    Me.__httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
                Next
                Me.__httpClient.DefaultRequestHeaders.Add("Cookie", "auth=ArbitrarySessionToken")
                response = Me.__httpClient.GetAsync(url).Result
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                printdbg(e)
                printdbg("__getLoginSession() failed")
            End Try

            Return response
        End Function

        Public Overridable Function __doLogin(loginSessionResponse As Object) As HttpResponseMessage
            Dim queryParameters As Dictionary(Of String, String) = parse_qsl(urlparse(loginSessionResponse.url).query).ToDictionary()
            Dim url As String = "https://mdtlogin.medtronic.com" & "/mmcl/auth/oauth/v2/authorize/login"
            Dim payload As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object) From {
                {
                    "country",
                    Me.__carelinkCountry},
                {
                    "locale",
CARELINK_LOCALE_EN}}
            Dim form As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object) From {
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
                response = Me.__httpClient.post(url, headers:=Me.__commonHeaders, form)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                printdbg(e)
                printdbg("__doLogin() failed")
            End Try

            Return response
        End Function

        Public Overridable Function __doConsent(doLoginResponse As Object) As HttpResponseMessage
            ' Extract data for consent
            Dim doLoginRespBody As String = doLoginResponse.text
            Dim url As Object = Me.__extractResponseData(doLoginRespBody, "<form action=", " ")
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
                response = Me.__httpClient.post(url, headers:=consentHeaders, data:=form)
                If Not response.StatusCode = HttpStatusCode.OK Then
                    Throw New Exception("session response is not OK")
                End If
            Catch e As Exception
                printdbg(e)
                printdbg("__doConsent() failed")
            End Try

            Return response
        End Function

        Public Overridable Function __getData(host As String, path As String, queryParams As Object, requestBody As Object) As Dictionary(Of String, String)
            Dim response As HttpResponseMessage = Nothing
            Dim url As Object
            printdbg("__getData()")
            Me.__lastDataSuccess = False
            If host Is Nothing Then
                url = path
            Else
                url = $"https://{host}/{path}"
            End If
            Dim payload As Object = queryParams
            Dim data As Object = requestBody
            Dim jsondata As Object = Nothing
            ' Get auth token
            Dim authToken As String = Me.__getAuthorizationToken()
            If authToken IsNot Nothing Then
                Try
                    ' Add header
                    Dim headers As Dictionary(Of String, String) = Me.__commonHeaders
                    headers("Authorization") = authToken
                    If data Is Nothing Then
                        headers("Accept") = "application/json, text/plain, */*"
                        headers("Content-Type") = "application/json; charset=utf-8"
                        response = Me.__httpClient.[get](url, headers:=headers, payload)
                        Me.__lastResponseCode = response.status_code
                        If Not response.StatusCode = HttpStatusCode.OK Then
                            Throw New Exception("session get response is not OK")
                        End If
                    Else
                        headers("Accept") = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"
                        headers("Content-Type") = "application/x-www-form-urlencoded"
                        response = Me.__httpClient.post(url, headers:=headers, data:=data)
                        Me.__lastResponseCode = response.status_code
                        If Not response.StatusCode = HttpStatusCode.OK Then
                            printdbg(response.StatusCode)
                            Throw New Exception("session post response is not OK")
                        End If
                    End If
                Catch e As Exception
                    printdbg(e)
                    printdbg("__getData() failed")
                End Try
            End If

            Return jsondata
        End Function

        Public Overridable Function __getMyUser() As Object
            printdbg("__getMyUser()")
            Return Me.__getData(Me.__careLinkServer(), "patient/users/me", Nothing, Nothing)
        End Function

        Public Overridable Function __getMyProfile() As Object
            printdbg("__getMyProfile()")
            Return Me.__getData(Me.__careLinkServer(), "patient/users/me/profile", Nothing, Nothing)
        End Function

        Public Overridable Function __getCountrySettings(country As Object, language As Object) As Object
            printdbg("__getCountrySettings()")
            Dim queryParams As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object) From {
                {
                    "countryCode",
                    country},
                {
                    "language",
                    language}}
            Return Me.__getData(Me.__careLinkServer(), "patient/countries/settings", queryParams, Nothing)
        End Function

        Public Overridable Function __getMonitorData() As Dictionary(Of String, String)
            printdbg("__getMonitorData()")
            Return Me.__getData(Me.__careLinkServer(), "patient/monitor/data", Nothing, Nothing)
        End Function

        ' Old last24hours webapp data
        Public Overridable Function __getLast24Hours() As Object
            printdbg("__getLast24Hours")
            Dim queryParams As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object) From {
                {
                    "cpSerialNumber",
                    "NONE"},
                {
                    "msgType",
                    "last24hours"},
                {
                    "requestTime",
                    Convert.ToInt32(time.time() * 1000).ToString()}}
            Return Me.__getData(Me.__careLinkServer(), "patient/connect/data", queryParams, Nothing)
        End Function

        ' Periodic data from CareLink Cloud
        Public Overridable Function __getConnectDisplayMessage(username As Object, role As Object, endpointUrl As Object) As Object
            printdbg("__getConnectDisplayMessage()")
            ' Build user json for request
            Dim userJson As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object) From {
                {
                    "username",
                    username},
                {
                    "role",
                    role}}
            Dim requestBody = Json.dumps(userJson)
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
                Open login(get SessionId And SessionData)
                Dim loginSessionResponse As Object = Me.__getLoginSessionAsync()
                Me.__lastResponseCode = loginSessionResponse.status_code
                ' Login
                Dim doLoginResponse As Object = Me.__doLogin(loginSessionResponse)
                Me.__lastResponseCode = doLoginResponse.status_code
                'setLastResponseBody(loginSessionResponse)
                loginSessionResponse.close()
                ' Consent
                Dim consentResponse As Object = Me.__doConsent(doLoginResponse)
                Me.__lastResponseCode = consentResponse.status_code
                'setLastResponseBody(consentResponse);
                doLoginResponse.close()
                consentResponse.close()
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
                printdbg(e)
                Me.__lastErrorMessage = e
            End Try
            Me.__loginInProcess = False
            Me.__loggedIn = lastLoginSuccess
            Return lastLoginSuccess
        End Function

        Public Overridable Function __getAuthorizationToken() As String
            Dim auth_token = Me.__httpClient.[get](CARELINK_AUTH_TOKEN_COOKIE_NAME)
            Dim auth_token_validto = Me.__httpClient.cookies.[get](CARELINK_TOKEN_VALIDTO_COOKIE_NAME)
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
                printdbg("auth_token_validto = " & Me.__httpClient.cookies.[get](CARELINK_TOKEN_VALIDTO_COOKIE_NAME))
            End If
            ' there can be only one
            Return "Bearer " & Me.__httpClient.cookies.[get](CARELINK_AUTH_TOKEN_COOKIE_NAME)
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

    End Class

End Module
