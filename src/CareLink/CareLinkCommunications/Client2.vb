' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class Client2



    ' Constants
    Private Const CARELINK_CONFIG_URL As String = "https://clcloud.minimed.eu/connect/carepartner/v11/discover/android/3.2"
    Private Const DEFAULT_FILENAME As String = "logindata.json"
    Private Const VERSION As String = "1.2"

    Private ReadOnly _tokenBaseFileName As String
    Private ReadOnly _version As String
    Private _accessTokenPayload As Dictionary(Of String, Object)
    Private _config As Dictionary(Of String, Object)
    Private _country As String
    Private _lastApiStatus As Integer
    Private _patient As Dictionary(Of String, String)
    Private _tokenDataElement As JsonElement
    Private _user As Dictionary(Of String, Object)
    Private _username As String
    Public Property LoggedIn As Boolean

    Public Shared ReadOnly s_common_Headers As New Dictionary(Of String, String) From {
        {"Accept", "application/json"},
        {"Content-Type", "application/json"},
        {"sec-ch-ua", """Not/A)Brand"";v=""99"", ""Microsoft Edge"";v=""115"", ""Chromium"";v=""115"""},
        {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.203"}}

    Public Sub New(Optional tokenFile As String = DEFAULT_FILENAME)
        _version = VERSION

        ' Authorization
        _tokenBaseFileName = tokenFile
        _tokenDataElement = Nothing
        _accessTokenPayload = Nothing

        ' API config
        _config = Nothing

        ' User info
        _username = Nothing
        _user = Nothing
        _patient = Nothing
        _country = Nothing

        ' API status
        _lastApiStatus = Nothing
    End Sub

    Public Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}
    Public Property SessionUser As SessionUserRecord
    Private Shared Function DoRefresh(config As Dictionary(Of String, Object), tokenDataElement As JsonElement) As JsonElement
        Console.WriteLine(NameOf(DoRefresh))
        Dim tokenUrl As String = CStr(config("token_url"))

        Dim tokenData As Dictionary(Of String, String) = tokenDataElement.ConvertJsonElementToStringDictionary

        Dim data As New Dictionary(Of String, String) From {
        {"refresh_token", tokenData("refresh_token")},
        {"client_id", tokenData("client_id")},
        {"client_secret", tokenData("client_secret")},
        {"grant_type", "refresh_token"}
    }

        Dim headers As New Dictionary(Of String, String) From {
        {"mag-identifier", tokenData("mag-identifier")}
    }

        Using httpClient As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim content As New FormUrlEncodedContent(data)
            Dim response As HttpResponseMessage = httpClient.PostAsync(tokenUrl, content).Result

            Console.WriteLine($"   status: {response.StatusCode}")

            If response.StatusCode <> Net.HttpStatusCode.OK Then
                Throw New Exception("ERROR: failed to refresh token")
            End If

            Dim responseBody As String = response.Content.ReadAsStringAsync().Result
            Dim newData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(responseBody)

            tokenData("access_token") = newData.GetProperty("access_token").GetString()
            tokenData("refresh_token") = newData.GetProperty("refresh_token").GetString()

            Return JsonSerializer.Deserialize(Of JsonElement)(JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions))
        End Using
    End Function

    Private Shared Function GetAccessTokenPayload(token_data As JsonElement) As Dictionary(Of String, Object)
        Debug.WriteLine(NameOf(GetAccessTokenPayload))
        Try
            Dim token As String = CStr(token_data.ConvertJsonElementToDictionary("access_token"))
            Dim payload_b64 As String = token.Split("."c)(1)
            Dim payload_b64_bytes As Byte() = Encoding.UTF8.GetBytes(payload_b64)
            Dim missing_padding As Integer = (4 - (payload_b64_bytes.Length Mod 4)) Mod 4
            If missing_padding > 0 Then
                payload_b64 &= New String("="c, missing_padding)
            End If
            Dim payload_bytes As Byte() = Convert.FromBase64String(payload_b64)
            Dim payload As String = Encoding.UTF8.GetString(payload_bytes)
            Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(payload)
        Catch ex As Exception
            Console.WriteLine("   no access token found or malformed access token")
            Return Nothing
        End Try
    End Function

    Friend Function TryGetDeviceSettingsPdfFile(pdfFileName As String) As Boolean
#If False Then

        Dim authToken As String = ""

        ' CareLink Partners do not support download
        If My.Settings.CareLinkPartner Then Return False
        If Me.GetAuthorizationToken(authToken) = GetAuthorizationTokenResult.OK Then
            Dim response As HttpResponseMessage = Nothing
            Dim serverUrl As String = GetServerUrl(Me.CareLinkCountry)
            Dim referrerUri As New Uri($"https://{serverUrl}/app/reports")
            Dim headers As Dictionary(Of String, String) = s_commonHeaders.Clone
            headers("Accept") = "application/json, text/plain, */*"
            headers("Authorization") = authToken
            headers("application_language") = "en"
            headers.Add("Accept-Encoding", "gzip, deflate, br")
            headers.Add("Host", serverUrl)
            headers.Add("Origin", $"https://{serverUrl}")
            Dim jsonData As Dictionary(Of String, String)
            Try
                _httpClient.SetDefaultRequestHeaders(headers, referrerUri)
                Dim requestUri As New Uri($"https://{serverUrl}/patient/reports/generateReport")
                Dim postRequest As New HttpRequestMessage(HttpMethod.Post, requestUri) With {.Content = Json.JsonContent.Create(New GetReportsSettingsRecord)}
                response = _httpClient.SendAsync(postRequest).Result
                _lastResponseCode = response.StatusCode
                If Not (response?.IsSuccessStatusCode) Then
                    Return False
                End If
            Catch ex As Exception
                _lastErrorMessage = ex.DecodeException()
                DebugPrint($"{_lastErrorMessage.Replace(vbCrLf, "")}, status {response?.StatusCode}")
                Return False
            End Try
            Try
                jsonData = JsonToDictionary(response.ResultText)
                Dim uuidString As String = $"{jsonData.Keys(0)}={jsonData.Values(0)}"
                Dim requestUri As New Uri($"https://{serverUrl}/patient/reports/reportStatus?{uuidString}")
                Dim delay As Integer = 250
                DebugPrint($"{NameOf(requestUri)} = {requestUri}")
                While True
                    _httpClient.SetDefaultRequestHeaders(headers, referrerUri)
                    Dim t As Task(Of HttpResponseMessage) = _httpClient.SendAsync(New HttpRequestMessage(HttpMethod.Get, requestUri))
                    For i As Integer = 0 To delay
                        If t.IsCompleted Then
                            Exit For
                        End If
                        Threading.Thread.Sleep(10)
                    Next
                    response = t.Result
                    _lastResponseCode = response.StatusCode
                    DebugPrint($"{NameOf(response)}?.IsSuccessStatusCode={response?.IsSuccessStatusCode}")
                    If response?.IsSuccessStatusCode Then
                        Dim resultText As String = response.ResultText
                        DebugPrint($"resultText={resultText}")
                        Select Case JsonToDictionary(resultText)("status")
                            Case "READY"
                                DebugPrint("READY")
                                Exit While
                            Case "NOT_READY"
                                DebugPrint("NOT_READY")
                            Case Else
                                Stop
                        End Select
                    Else
                        Return False
                    End If
                End While
                requestUri = New Uri($"https://{serverUrl}/patient/reports/reportPdf?{uuidString}")
                _httpClient.SetDefaultRequestHeaders(headers, referrerUri)
                response = _httpClient.SendAsync(New HttpRequestMessage(HttpMethod.Get, requestUri)).Result
                _lastResponseCode = response.StatusCode
                If response?.IsSuccessStatusCode Then
                    Dim fileNameOrDefault As String = response.Content.Headers.GetValues("Content-Disposition").FirstOrDefault
                    If fileNameOrDefault.StartsWith("filename=") Then
                        Dim fileContents As Byte() = response.Content.ReadAsByteArrayAsync.Result
                        ByteArrayToFile(pdfFileName, fileContents)

                    End If
                End If
                Return True
            Catch ex As Exception
                If NetworkUnavailable() Then
                    DebugPrint($"has no Internet Connection!")
                End If
                DebugPrint($"failed {ex.DecodeException().Replace(vbCrLf, "")}")
            End Try
        End If
        Stop
#End If
        Return False
    End Function

    Private Shared Function IsTokenValid(access_token_payload As Dictionary(Of String, Object)) As Boolean
        Console.WriteLine(NameOf(IsTokenValid))
        Try
            ' Get expiration time stamp
            Dim tokenValidTo As Long = CType(access_token_payload("exp"), JsonElement).GetInt64()

            ' Check expiration time stamp
            Dim currentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = tokenValidTo - currentTime

            If tDiff < 0 Then
                Console.WriteLine($"   access token has expired {Math.Abs(tDiff)}s ago")
                Return False
            End If

            If tDiff < 600 Then
                Console.WriteLine($"   access token is about to expire in {tDiff}s")
                Return False
            End If

            ' Token is valid
            Dim authTokenValidTo As String = DateTimeOffset.FromUnixTimeSeconds(tokenValidTo).ToString("ddd MMM dd HH:mm:ss UTC yyyy")
            Console.WriteLine($"   access token expires in {tDiff}s ({authTokenValidTo})")
            Return True
        Catch ex As Exception
            Console.WriteLine("   missing data in access token")
            Return False
        End Try
    End Function

    Private Function _init() As Boolean
        _tokenDataElement = ReadTokenFile(s_userName, _tokenBaseFileName)
        If _tokenDataElement.ValueKind = JsonValueKind.Null Then
            Me.LoggedIn = False
            Return False
        End If

        _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
        If _accessTokenPayload Is Nothing Then
            Return False
        End If
        Dim jsonConfigElement As JsonElement
        Try
            'Richard Headers are not being handled correctly
            Dim element As JsonElement = CType(_accessTokenPayload("token_details"), JsonElement)
            Dim payload As AccessTokenDetails = JsonSerializer.Deserialize(Of AccessTokenDetails)(element, s_jsonDeserializerOptions)
            _country = payload.Country
            jsonConfigElement = GetConfigElement(CARELINK_CONFIG_URL, _country)
            _config = jsonConfigElement.ConvertJsonElementToDictionary
            _username = payload.Name
            _user = Me.GetUser(jsonConfigElement, _tokenDataElement).ConvertJsonElementToDictionary
            Dim role As String = CStr(_user("role"))
            If role = "CARE_PARTNER" OrElse role = "CARE_PARTNER_OUS" Then
                _patient = Me.GetPatient(jsonConfigElement, _tokenDataElement).Result
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString())

            If Auth_Error_Codes.Contains(CInt(_lastApiStatus)) Then
                Try
                    _tokenDataElement = DoRefresh(jsonConfigElement.ConvertJsonElementToDictionary, _tokenDataElement)
                    _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                    WriteTokenFile(_tokenDataElement, s_userName)
                Catch refreshEx As Exception
                    Console.WriteLine(refreshEx.ToString())
                End Try
            End If

            Me.LoggedIn = False
            Return False
        End Try

        Me.LoggedIn = True
        Return True
    End Function

    Private Function GetData(config As Dictionary(Of String, Object), tokenDataElement As JsonElement, username As String, role As String, patientId As String) As Dictionary(Of String, Object)
        Console.WriteLine("_get_data()")
        Dim url As String = $"{CStr(config("baseUrlCumulus"))}/display/message"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        Dim tokenData As Dictionary(Of String, String) = tokenDataElement.ConvertJsonElementToStringDictionary
        headers("mag-identifier") = tokenData("mag-identifier")
        headers("Authorization") = $"Bearer {tokenData("access_token")}"

        Dim data As New Dictionary(Of String, Object) From {
            {"username", username}
        }

        If role = "CARE_PARTNER" OrElse role = "CARE_PARTNER_OUS" Then
            data("role") = "carepartner"
            data("patientId") = patientId
        Else
            data("role") = "patient"
        End If

        _lastApiStatus = Nothing

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim jsonContent As New StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
            Dim response As HttpResponseMessage = client.PostAsync(url, jsonContent).Result
            _lastApiStatus = CInt(response.StatusCode)
            Console.WriteLine($"   status: {_lastApiStatus}")

            If response.IsSuccessStatusCode Then
                Dim content As String = response.Content.ReadAsStringAsync().Result
                Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(content)
            End If
        End Using
        Return Nothing
    End Function

    Private Async Function GetPatient(config As JsonElement, token_data As JsonElement) As Task(Of Dictionary(Of String, String))
        Console.WriteLine(NameOf(GetPatient))
        Dim url As String = $"{CStr(config.ConvertJsonElementToDictionary("baseUrlCareLink"))}/links/patients"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        headers("mag-identifier") = CStr(token_data.ConvertJsonElementToDictionary("mag-identifier"))
        headers("Authorization") = $"Bearer {CStr(token_data.ConvertJsonElementToDictionary("access_token"))}"

        _lastApiStatus = Nothing

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim response As HttpResponseMessage = Await client.GetAsync(url)
            _lastApiStatus = CInt(response.StatusCode)
            Console.WriteLine($"   status: {_lastApiStatus}")

            If response.IsSuccessStatusCode Then
                Dim content As String = Await response.Content.ReadAsStringAsync()
                Dim patients As List(Of Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, String)))(content)
                If patients.Count > 0 Then
                    Return patients(0)
                End If
            End If
        End Using

        Return Nothing
    End Function

    Private Function GetUser(config As JsonElement, tokenData As JsonElement) As JsonElement
        Console.WriteLine(NameOf(GetUser))
        Dim url As String = config.GetProperty("baseUrlCareLink").GetString() & "/users/me"
        'Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        Dim headers As New Dictionary(Of String, String)
        headers("mag-identifier") = tokenData.GetProperty("mag-identifier").GetString()
        headers("Authorization") = "Bearer " & tokenData.GetProperty("access_token").GetString()

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim response As HttpResponseMessage = client.GetAsync(url).Result
            _lastApiStatus = CInt(response.StatusCode)
            Console.WriteLine($"   status: {_lastApiStatus}")

            If response.IsSuccessStatusCode Then
                Return JsonSerializer.Deserialize(Of JsonElement)(response.Content.ReadAsStringAsync().Result)
            End If
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Get Client library version
    ''' </summary>
    Public Function GetClientVersion() As String
        Return _version
    End Function

    ''' <summary>
    ''' Get last API response code
    ''' </summary>
    Public Function GetLastResponseCode() As Integer
        Return _lastApiStatus
    End Function

    ''' <summary>
    ''' Get recent periodic pump data
    ''' </summary>
    Public Function GetRecentData() As Dictionary(Of String, Object)
        ' Check if access token is valid
        If Not IsTokenValid(_accessTokenPayload) Then
            _tokenDataElement = DoRefresh(_config, _tokenDataElement)
            _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
            WriteTokenFile(_tokenDataElement, s_userName)
            If Not IsTokenValid(_accessTokenPayload) Then
                Console.Error.WriteLine("ERROR: unable to get valid access token")
                Return Nothing
            End If
        End If

        Dim patientId As String = Nothing
        If _patient IsNot Nothing Then
            patientId = _patient("username")
        End If

        ' Get data: first try
        Dim data As Dictionary(Of String, Object) = Me.GetData(config:=_config,
                                           tokenDataElement:=_tokenDataElement,
                                           username:=_username,
                                           role:=CStr(_user("role")),
                                           patientId:=patientId)

        ' Check API response
        If Auth_Error_Codes.Contains(_lastApiStatus) Then
            ' Try to refresh token
            _tokenDataElement = DoRefresh(_config, _tokenDataElement)
            _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
            WriteTokenFile(_tokenDataElement, s_userName)

            ' Get data: second try
            data = Me.GetData(config:=_config,
                           _tokenDataElement,
                           _username,
                           CStr(_user("role")),
                           patientId)

            ' Check API response
            If Auth_Error_Codes.Contains(_lastApiStatus) Then
                ' Failed permanently
                Console.Error.WriteLine("ERROR: unable to get data")
                Return Nothing
            End If
        End If

        Return data
    End Function

    Public Function Init() As Boolean
        ' First try
        If Not Me._init() Then
            ' Second try (after token refresh)
            If Not Me._init() Then
                ' Failed permanently
                'Log.Error("ERROR: unable to initialize")
                Return False
            End If
        End If
        Return True
    End Function

    Public Sub PrintUserInfo()
        Console.WriteLine("User Info:")
        Console.WriteLine($"   user:     {_username} ({_user("firstName")} {_user("lastName")})")
        Console.WriteLine($"   role:     {_user("role")}")
        Console.WriteLine($"   country:  {_country}")
        If _patient IsNot Nothing Then
            Console.WriteLine($"   patient:  {_patient("username")} ({_patient("firstName")} {_patient("lastName")})")
        End If
    End Sub

End Class
