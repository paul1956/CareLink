' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Policy
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Nodes

Public Class Client2

    ' Constants
    Private Const DEFAULT_FILENAME As String = "logindata.json"

    Private Const VERSION As String = "1.2"

    Private ReadOnly _tokenBaseFileName As String
    Private ReadOnly _version As String
    Private _accessTokenPayload As Dictionary(Of String, Object)
    Private _config As Dictionary(Of String, Object)
    Private _country As String
    Private ReadOnly _httpClient As HttpClient
    Private _lastApiStatus As Integer
    Private _patientElement As Dictionary(Of String, String)
    Private _tokenDataElement As JsonElement
    Private _userElementDictionary As Dictionary(Of String, Object)
    Public Property LoggedIn As Boolean
    Public Property PatientPersonalData As New PatientPersonalInfo
    Public Property UserElementDictionary As Dictionary(Of String, Object)
        Get
            Return _userElementDictionary
        End Get
        Set
            _userElementDictionary = Value
        End Set
    End Property

    Public Sub New(Optional tokenFile As String = DEFAULT_FILENAME)
        _version = VERSION

        ' Authorization
        _tokenBaseFileName = tokenFile
        _tokenDataElement = Nothing
        _accessTokenPayload = Nothing

        ' API config
        _config = Nothing

        ' User info
        _patientElement = Nothing
        _country = Nothing

        ' API status
        _lastApiStatus = Nothing

        _httpClient = New HttpClient
        _httpClient.SetDefaultRequestHeaders()
    End Sub

    Public Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}

    Private Function DoRefresh(config As Dictionary(Of String, Object), tokenDataElement As JsonElement) As Task(Of JsonElement)
        Debug.WriteLine(NameOf(DoRefresh))
        _httpClient.SetDefaultRequestHeaders()
        Dim tokenUrl As String = CStr(config("token_url"))
        Dim tokenData As Dictionary(Of String, Object) = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(tokenDataElement)

        Dim data As New Dictionary(Of String, String) From {
            {"refresh_token", tokenData("refresh_token").ToString},
            {"client_id", tokenData("client_id").ToString},
            {"client_secret", tokenData("client_secret").ToString},
            {"grant_type", "refresh_token"}}

        Dim content As New FormUrlEncodedContent(data)

        Dim headers As New Dictionary(Of String, String) From {
            {"mag-identifier", tokenData("mag-identifier").ToString}}

        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        Dim response As HttpResponseMessage = _httpClient.PostAsync(tokenUrl, content).Result

        _lastApiStatus = CInt(response.StatusCode)
        Debug.WriteLine($"   status: {_lastApiStatus}")

        If response.StatusCode <> Net.HttpStatusCode.OK Then
            Throw New Exception("ERROR: failed to refresh token")
        End If

        Dim responseBody As String = response.Content.ReadAsStringAsync().Result
        Dim newData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(responseBody)
        tokenData("access_token") = newData.GetProperty("access_token").GetString()
        tokenData("refresh_token") = newData.GetProperty("refresh_token").GetString()
        Dim modifiedJsonString As String = JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions)
        Return Task.FromResult(JsonSerializer.Deserialize(Of JsonElement)(modifiedJsonString))
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
            Debug.WriteLine("   no access token found or malformed access token")
            Return Nothing
        End Try
    End Function

    Friend Function TryGetDeviceSettingsPdfFile(pdfFileName As String) As Boolean
        Dim authToken As String = ""

        ' CareLink Partners do not support download
        If My.Settings.CareLinkPartner Then Return False
#If False Then ' TODO: Implement

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
                (_httpClient.SetDefaultRequestHeadersheaders, referrerUri)
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
        Debug.WriteLine(NameOf(IsTokenValid))
        Try
            ' Get expiration time stamp
            Dim unixTimeToValidate As Long = CType(access_token_payload("exp"), JsonElement).GetInt64()

            ' Check expiration time stamp
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = unixTimeToValidate - unixCurrentTime

            If tDiff < 0 Then
                Debug.WriteLine($"   access token has expired {Math.Abs(tDiff)}s ago")
                Return False
            End If

            If tDiff < 600 Then
                Debug.WriteLine($"   access token is about to expire in {tDiff}s")
                Return False
            End If

            ' Token is valid
            Dim authTokenValidTo As String = DateTimeOffset.FromUnixTimeSeconds(unixTimeToValidate).ToString("ddd MMM dd HH:mm:ss UTC yyyy")
            Debug.WriteLine($"   access token expires in {tDiff}s ({authTokenValidTo})")
            Return True
        Catch ex As Exception
            Debug.WriteLine("   missing data in access token")
            Return False
        End Try
    End Function

    Private Function _init() As Boolean
        _tokenDataElement = ReadTokenFile(s_userName, _tokenBaseFileName)
        If _tokenDataElement.ValueKind.IsNullOrUndefined Then
            Me.LoggedIn = False
            Return False
        End If

        _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
        If _accessTokenPayload Is Nothing Then
            Return False
        End If
        Dim jsonConfigElement As JsonElement
        Try
            Dim element As JsonElement = CType(_accessTokenPayload("token_details"), JsonElement)
            Application.DoEvents()
            Dim payload As AccessTokenDetails = JsonSerializer.Deserialize(Of AccessTokenDetails)(element, s_jsonDeserializerOptions)
            _country = payload.Country
            jsonConfigElement = GetConfigElement(_httpClient, payload.Country)
            _config = jsonConfigElement.ConvertJsonElementToDictionary()
            Dim userString As String = Me.GetUserString(jsonConfigElement, _tokenDataElement)
            If String.IsNullOrWhiteSpace(userString) Then
                Throw New UnauthorizedAccessException
            End If
            _userElementDictionary = If(String.IsNullOrWhiteSpace(userString),
                Nothing,
                JsonSerializer.Deserialize(Of JsonElement)(userString).ConvertJsonElementToDictionary)
            _PatientPersonalData = JsonSerializer.Deserialize(Of PatientPersonalInfo)(userString)

            Dim role As String = _PatientPersonalData.role
            If role.Contains("Partner", StringComparison.InvariantCultureIgnoreCase) Then
                _patientElement = Me.GetPatient(jsonConfigElement, _tokenDataElement).Result
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.ToString())

            If Auth_Error_Codes.Contains(CInt(_lastApiStatus)) Then
                Try
                    _tokenDataElement = Me.DoRefresh(jsonConfigElement.ConvertJsonElementToDictionary, _tokenDataElement).Result
                    _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                    WriteTokenFile(_tokenDataElement, s_userName)
                Catch refreshEx As Exception
                    Debug.WriteLine(refreshEx.ToString())
                End Try
            End If

            Me.LoggedIn = False
            Return False
        End Try

        Me.LoggedIn = True
        Return True
    End Function

    Private Function GetData(config As Dictionary(Of String, Object), tokenDataElement As JsonElement, username As String, role As String, patientId As String) As Dictionary(Of String, Object)
        Debug.WriteLine(NameOf(GetData))
        _httpClient.SetDefaultRequestHeaders()
        Dim url As String = $"{CStr(config("baseUrlCumulus"))}/display/message"
        Dim tokenData As Dictionary(Of String, String) = tokenDataElement.ConvertJsonElementToStringDictionary()

        Dim data As New Dictionary(Of String, Object) From {
            {"username", username}
        }

        If role.Contains("Partner", StringComparison.InvariantCultureIgnoreCase) Then
            data("role") = "carepartner"
            data("patientId") = patientId
        Else
            data("role") = "patient"
        End If

        _lastApiStatus = Nothing

        Dim headers As New Dictionary(Of String, String)
        headers("mag-identifier") = tokenData("mag-identifier")
        headers("Authorization") = $"Bearer {tokenData("access_token")}"
        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        Dim jsonContent As New StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        Dim response As HttpResponseMessage = _httpClient.PostAsync(url, jsonContent).Result
        _lastApiStatus = CInt(response.StatusCode)
        Debug.WriteLine($"   status: {_lastApiStatus}")

        If response.IsSuccessStatusCode Then
            Dim content As String = response.Content.ReadAsStringAsync().Result
            Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(content)
        End If
        Return Nothing
    End Function

    Private Async Function GetPatient(config As JsonElement, token_data As JsonElement) As Task(Of Dictionary(Of String, String))
        Debug.WriteLine(NameOf(GetPatient))
        Dim url As String = $"{CStr(config.ConvertJsonElementToDictionary("baseUrlCareLink"))}/links/patients"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        headers("mag-identifier") = CStr(token_data.ConvertJsonElementToDictionary("mag-identifier"))
        headers("Authorization") = $"Bearer {CStr(token_data.ConvertJsonElementToDictionary("access_token"))}"

        _lastApiStatus = Nothing

        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        Dim response As HttpResponseMessage = Await _httpClient.GetAsync(url)
        _lastApiStatus = CInt(response.StatusCode)
        Debug.WriteLine($"   status: {_lastApiStatus}")

        If response.IsSuccessStatusCode Then
            Dim content As String = Await response.Content.ReadAsStringAsync()
            Dim patients As List(Of Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, String)))(content)
            If patients.Count > 0 Then
                Return patients(0)
            End If
        End If

        Return Nothing
    End Function

    Private Function GetUserString(config As JsonElement, tokenData As JsonElement) As String
        Debug.WriteLine(NameOf(GetUserString))
        Dim url As String = $"{config.GetProperty("baseUrlCareLink").GetString()}/users/me"
        Dim headers As New Dictionary(Of String, String)
        headers("mag-identifier") = tokenData.GetProperty("mag-identifier").GetString()
        headers("Authorization") = $"Bearer {tokenData.GetProperty("access_token").GetString()}"

        Dim request As New HttpRequestMessage(HttpMethod.Get, url)
        request.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        For Each header As KeyValuePair(Of String, String) In headers.Sort
            request.Headers.Add(header.Key, header.Value)
        Next

        ' Send the request
        Dim response As HttpResponseMessage = _httpClient.SendAsync(request).Result
        _lastApiStatus = CInt(response.StatusCode)
        Debug.WriteLine($"   status: {_lastApiStatus}")

        Return If(response.IsSuccessStatusCode,
            response.Content.ReadAsStringAsync().Result,
            Nothing)
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
    Public Function GetRecentData() As String
        ' Check if access token is valid
        Dim lastErrorMessage As String = Nothing
        If Not IsTokenValid(_accessTokenPayload) Then
            _tokenDataElement = Me.DoRefresh(_config, _tokenDataElement).Result
            _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
            WriteTokenFile(_tokenDataElement, s_userName)
            If Not IsTokenValid(_accessTokenPayload) Then
                lastErrorMessage = "ERROR: unable to get valid access token"
                Debug.WriteLine(lastErrorMessage)
                Return lastErrorMessage
            End If
        End If
        Dim data As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
        Try
            Dim tempData As Dictionary(Of String, Object) = Me.GetData(
                   config:=_config,
                   tokenDataElement:=_tokenDataElement,
                   username:=s_userName,
                   role:=CStr(_userElementDictionary("role")),
                   patientId:="")
            For Each kvp As KeyValuePair(Of String, Object) In tempData
                data(kvp.Key) = kvp.Value
            Next kvp
        Catch ex As Exception
            PatientData = Nothing
            RecentData = Nothing
            Stop
            Return ex.DecodeException()
        End Try

        ' Check API response
        If Auth_Error_Codes.Contains(_lastApiStatus) Then
            ' Try to refresh token
            _tokenDataElement = Me.DoRefresh(_config, _tokenDataElement).Result
            _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
            WriteTokenFile(_tokenDataElement, s_userName)
        End If
        Select Case data.Keys.Count
            Case 0
                lastErrorMessage = "No Data Found"
            Case 1
                lastErrorMessage = "No Data Found for " & data.Keys(0)
            Case 2
            Case Else
                lastErrorMessage = "No Data Found for " & String.Join(", ", data.Keys)
        End Select
        Dim metaData As JsonElement = CType(data.Values(0), JsonElement)
        Dim patientDataElement As JsonElement = CType(data.Values(1), JsonElement)
        Try
            Dim patientDataElementAsText As String = patientDataElement.GetRawText()
            PatientData = JsonSerializer.Deserialize(Of PatientDataInfo)(patientDataElement, s_jsonDeserializerOptions)
            Stop
            RecentData = patientDataElement.ConvertJsonElementToStringDictionary()
        Catch ex As Exception
            Stop
        End Try

        Return lastErrorMessage
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

End Class
