' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Text.Json

Public Class Client2

    ' Constants
    Private Const TokenBaseFileName As String = "logindata.json"
    Private ReadOnly _tokenBaseFileName As String
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

    Public Sub New(Optional tokenFile As String = TokenBaseFileName)
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
        _lastApiStatus = 0

        _httpClient = New HttpClient
        _httpClient.SetDefaultRequestHeaders()
    End Sub

    Public Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}

    ''' <summary>
    '''  Reads the token file and returns the token data as a <see cref="JsonElement"/>.
    ''' </summary>
    ''' <param name="config"></param>
    ''' <param name="tokenDataElement"></param>
    ''' <returns>
    '''  A <see cref="JsonElement"/> containing the token data.
    '''  If the file does not exist or is empty, returns an empty <see cref="JsonElement"/>.
    ''' </returns>
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

        Dim response As New HttpResponseMessage With {.StatusCode = Net.HttpStatusCode.GatewayTimeout}
        Try
            response = _httpClient.PostAsync(tokenUrl, content).Result
            _lastApiStatus = response.StatusCode
            Debug.WriteLine($"   status: {_lastApiStatus}")

            If response.StatusCode <> Net.HttpStatusCode.OK Then
                If MsgBox(
                    heading:=$"ERROR: Failed to refresh token, status {_lastApiStatus}",
                    text:="Do you want to try logging in again?",
                    buttonStyle:=MsgBoxStyle.YesNo,
                    title:="New Login Required") <> MsgBoxResult.Yes Then

                    Throw New Exception("ERROR: Failed to refresh token!")
                Else
                    Dim fileWithPath As String = GetLoginDataFileName(s_userName, TokenBaseFileName)
                    If File.Exists(fileWithPath) Then
                        File.Delete(fileWithPath)
                    End If
                    If Not DoOptionalLoginAndUpdateData(
                        mainForm:=My.Forms.Form1,
                        updateAllTabs:=False,
                        fileToLoad:=FileToLoadOptions.Login) Then

                        Throw New Exception("ERROR: Failed to refresh token!")
                    End If
                End If
            End If
            Dim responseBody As String = response.Content.ReadAsStringAsync().Result
            Dim newData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(responseBody)
            tokenData("access_token") = newData.GetProperty("access_token").GetString()
            tokenData("refresh_token") = newData.GetProperty("refresh_token").GetString()
            Dim modifiedJsonString As String = JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions)
            Return Task.FromResult(JsonSerializer.Deserialize(Of JsonElement)(modifiedJsonString))
        Catch ex As Exception
        Finally
            response.Dispose()
        End Try
        Return Task.FromResult(New JsonElement)
    End Function

    ''' <summary>
    '''  Reads the token file and returns the token data as a <see cref="JsonElement"/>.
    ''' </summary>
    ''' <param name="userName">The username for which to read the token file.</param>
    ''' <param name="tokenBaseFileName">The base filename for the token file.</param>
    ''' <returns>A <see cref="JsonElement"/> containing the token data.</returns>
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

        Dim tokenData As Dictionary(Of String, String) = _tokenDataElement.ConvertJsonElementToStringDictionary()

        Dim response As HttpResponseMessage = Nothing
        Dim serverUri As New Uri(CStr(_config("baseUrlCumulus")))
        Dim headers As New Dictionary(Of String, String) From {
            {"Accept", "application/json, text/plain, */*"},
            {"Accept-Encoding", "gzip, deflate, br"},
            {"application_language", "en"},
            {"Authorization", $"Bearer {tokenData("access_token")}"},
            {"Host", serverUri.Host},
            {"mag-identifier", tokenData("mag-identifier")},
            {"Origin", $"https://{serverUri.Host}"}
        }

        _httpClient.SetDefaultRequestHeaders()
        For Each header As KeyValuePair(Of String, String) In headers.Sort
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next
        Dim jsonData As Dictionary(Of String, String)
        Dim lastResponseCode As Integer
        Try
            Dim requestUri As New Uri($"{CStr(_config("baseUrlCumulus"))}/patient/reports/generateReport")
            Dim request As New HttpRequestMessage(HttpMethod.Get, requestUri)
            request.Headers.Referrer = New Uri($"{CStr(_config("baseUrlCumulus"))}/app/reports")
            Dim inputValue As GetReportsSettingsRecord = New GetReportsSettingsRecord With {
                .patientId = CStr(Form1.Client.UserElementDictionary("accountId"))
            }

            Dim postRequest As New HttpRequestMessage(HttpMethod.Post, requestUri) With {.Content = Json.JsonContent.Create(inputValue)}
            response = _httpClient.SendAsync(postRequest).Result
            lastResponseCode = response.StatusCode
            If Not (response?.IsSuccessStatusCode) Then
                Return False
            End If
        Catch ex As Exception
            Dim lastErrorMessage As String = ex.DecodeException()
            DebugPrint($"{lastErrorMessage.Replace(vbCrLf, "")}, status {response?.StatusCode}")
            Return False
        End Try
        Try
            jsonData = JsonToDictionary(response.ResultText)
            Dim uuidString As String = $"{jsonData.Keys(0)}={jsonData.Values(0)}"
            Dim requestUri As New Uri($"https://{CStr(_config("baseUrlCumulus"))}/patient/reports/reportStatus?{uuidString}")
            Dim delay As Integer = 250
            DebugPrint($"{NameOf(requestUri)} = {requestUri}")
            While True
                Dim t As Task(Of HttpResponseMessage) = _httpClient.SendAsync(New HttpRequestMessage(HttpMethod.Get, requestUri))
                For i As Integer = 0 To delay
                    If t.IsCompleted Then
                        Exit For
                    End If
                    Threading.Thread.Sleep(10)
                Next
                response = t.Result
                lastResponseCode = response.StatusCode
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
            requestUri = New Uri($"https://{CStr(_config("baseUrlCumulus"))}/patient/reports/reportPdf?{uuidString}")
            _httpClient.SetDefaultRequestHeaders()
            response = _httpClient.SendAsync(New HttpRequestMessage(HttpMethod.Get, requestUri)).Result
            lastResponseCode = response.StatusCode
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

    ''' <summary>
    '''  Initialize the client
    '''  This function reads the token file, retrieves the access token payload,
    '''  and sets up the configuration and user information.
    ''' </summary>
    ''' <returns><see langword="True"/> if initialization is successful, <see langword="False"/> otherwise.</returns>
    Private Function internalInit() As Boolean
        _tokenDataElement = ReadTokenFile(userName:=s_userName, tokenBaseFileName:=_tokenBaseFileName)
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

            If Auth_Error_Codes.Contains(_lastApiStatus) Then
                Try
                    _tokenDataElement = Me.DoRefresh(jsonConfigElement.ConvertJsonElementToDictionary, _tokenDataElement).Result
                    If Not (_tokenDataElement.ValueKind = JsonValueKind.Undefined OrElse _tokenDataElement.ValueKind = JsonValueKind.Null) Then
                        _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                        WriteTokenFile(_tokenDataElement, s_userName)
                    End If
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

    ''' <summary>
    '''  Get data from the API
    '''  This function sends a request to the API to retrieve data based on the provided parameters.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="role"></param>
    ''' <param name="patientId"></param>
    ''' <returns><see cref="Dictionary"/> containing the retrieved data.</returns>
    Private Function GetData(username As String, role As String, patientId As String) As Dictionary(Of String, Object)
        Debug.WriteLine(NameOf(GetData))
        _httpClient.SetDefaultRequestHeaders()
        Dim requestUri As String = $"{CStr(_config("baseUrlCumulus"))}/display/message"
        Dim tokenData As Dictionary(Of String, String) = _tokenDataElement.ConvertJsonElementToStringDictionary()

        Dim value As New Dictionary(Of String, Object) From {{"username", username}}

        If role.Contains("Partner", StringComparison.InvariantCultureIgnoreCase) Then
            value("role") = "carepartner"
            value("patientId") = patientId
        Else
            value("role") = "patient"
        End If

        _lastApiStatus = 0

        Dim headers As New Dictionary(Of String, String)
        headers("mag-identifier") = tokenData("mag-identifier")
        headers("Authorization") = $"Bearer {tokenData("access_token")}"
        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        Using content As New StringContent(
            content:=JsonSerializer.Serialize(value),
            encoding:=Encoding.UTF8,
            mediaType:="application/json")
            Using response As HttpResponseMessage = _httpClient.PostAsync(requestUri, content).Result
                _lastApiStatus = response.StatusCode
                Debug.WriteLine($"   status: {_lastApiStatus}")

                If response.IsSuccessStatusCode Then
                    Dim json As String = response.Content.ReadAsStringAsync().Result
                    Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json)
                End If
            End Using
        End Using

        Return Nothing
    End Function

    Private Async Function GetPatient(config As JsonElement, token_data As JsonElement) As Task(Of Dictionary(Of String, String))
        Debug.WriteLine(NameOf(GetPatient))
        Dim url As String = $"{CStr(config.ConvertJsonElementToDictionary("baseUrlCareLink"))}/links/patients"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        headers("mag-identifier") = CStr(token_data.ConvertJsonElementToDictionary("mag-identifier"))
        headers("Authorization") = $"Bearer {CStr(token_data.ConvertJsonElementToDictionary("access_token"))}"

        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        _lastApiStatus = 0
        Using response As HttpResponseMessage = Await _httpClient.GetAsync(url)
            _lastApiStatus = response.StatusCode
            Debug.WriteLine($"   status: {_lastApiStatus}")

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
        _lastApiStatus = response.StatusCode
        Debug.WriteLine($"   status: {_lastApiStatus}")

        Return If(response.IsSuccessStatusCode,
            response.Content.ReadAsStringAsync().Result,
            Nothing)
    End Function

    ''' <summary>
    '''  Gets the last HTTP response status code received from the API.
    ''' </summary>
    ''' <returns>
    '''  An <see langword="Integer"/> representing the last API response status code.
    ''' </returns>

    Public Function GetLastResponseCode() As Integer
        Return _lastApiStatus
    End Function

    ''' <summary>
    '''  Retrieves the most recent data from the API.
    '''  This function checks if the access token is valid, attempts to refresh it if necessary,
    '''  and then sends a request to the API to obtain the latest patient data.
    '''  If the access token cannot be refreshed or the API call fails, an error message is returned.
    ''' </summary>
    ''' <returns>
    '''  A <see cref="String"/> indicating the status of the operation, or an error message if the operation fails.
    ''' </returns>
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
        PatientDataElement = CType(data.Values(1), JsonElement)
        Try
            File.WriteAllTextAsync(
                path:=GetLastDownloadFileWithPath(),
                contents:=JsonSerializer.Serialize(PatientDataElement, s_jsonSerializerOptions))
            DeserializePatientElement()
        Catch ex As Exception
            Stop
        End Try

        Return lastErrorMessage
    End Function

    Public Function Init() As Boolean
        ' First try
        If Not Me.internalInit() Then
            ' Second try (after token refresh)
            If Not Me.internalInit() Then
                ' Failed permanently
                'Log.Error("ERROR: unable to initialize")
                Return False
            End If
        End If
        Return True
    End Function

End Class
