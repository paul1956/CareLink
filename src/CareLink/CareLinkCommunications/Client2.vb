' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
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
    Private _lastHttpStatusCode As HttpStatusCode
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

    ''' <summary>
    '''  Initializes a new instance of the <see cref="Client2"/> class.
    ''' </summary>
    ''' <param name="tokenFile">
    '''  Optional. The token file name to use for authentication. Defaults to <c>logindata.json</c>.
    ''' </param>
    ''' <remarks>
    '''  The constructor sets up the <see cref="HttpClient"/>, initializes configuration and token fields,
    '''  and prepares the client for authentication and API communication.
    ''' </remarks>
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
        _lastHttpStatusCode = 0

        _httpClient = New HttpClient
        _httpClient.SetDefaultRequestHeaders()
    End Sub

    Public Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}

    ''' <summary>
    '''  Refreshes the authentication token using the provided configuration and token data.
    ''' </summary>
    ''' <param name="config">The API configuration as a <see cref="Dictionary(Of String, Object)"/>.</param>
    ''' <param name="tokenDataElement">The current token data as a <see cref="JsonElement"/>.</param>
    ''' <returns>
    '''  A <see cref="Task(Of JsonElement)"/> containing the refreshed token data.
    ''' </returns>
    ''' <remarks>
    '''  This method attempts to refresh the access token using the refresh token. If the refresh fails,
    '''  the user may be prompted to log in again.
    ''' </remarks>
    ''' <exception cref="ApplicationException">
    '''  Thrown if the refresh token operation fails and the user does not choose to log in again.
    ''' </exception>
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
            _lastHttpStatusCode = response.StatusCode
            Debug.WriteLine($"   status: {_lastHttpStatusCode}")

            If response.StatusCode <> Net.HttpStatusCode.OK Then
                If MsgBox(
                    heading:=$"ERROR: Failed to refresh token, status {_lastHttpStatusCode}",
                    text:="Do you want to try logging in again?",
                    buttonStyle:=MsgBoxStyle.YesNo,
                    title:="New Login Required") <> MsgBoxResult.Yes Then

                    Throw New ApplicationException(
                        message:="ERROR: Failed to refresh token!",
                        innerException:=New Exception("Refresh token operation failed."))
                Else
                    Dim fileWithPath As String = GetLoginDataFileName(s_userName, TokenBaseFileName)
                    If File.Exists(fileWithPath) Then
                        File.Delete(fileWithPath)
                    End If
                    If Not DoOptionalLoginAndUpdateData(
                        owner:=My.Forms.Form1,
                        updateAllTabs:=False,
                        fileToLoad:=FileToLoadOptions.Login) Then

                        Throw New ApplicationException(
                            message:="ERROR: Failed to refresh token!",
                            innerException:=New Exception("Refresh token operation failed."))
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
            Debug.WriteLine($"ERROR: {ex.DecodeException()} in {NameOf(DoRefresh)}")
            Stop
        Finally
            response.Dispose()
        End Try
        Return Task.FromResult(New JsonElement)
    End Function

    ''' <summary>
    '''  Extracts the access token payload from the given token data.
    ''' </summary>
    ''' <param name="token_data">The token data as a <see cref="JsonElement"/>.</param>
    ''' <returns>
    '''  A <see cref="Dictionary(Of String, Object)"/> containing the access token payload,
    '''  or <see langword="Nothing"/> if extraction fails.
    ''' </returns>
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
            Debug.WriteLine($"No access token found or malformed access token: {ex.DecodeException()} in {NameOf(GetAccessTokenPayload)}")
            Stop
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Attempts to download the device settings PDF file for the current user.
    ''' </summary>
    ''' <param name="pdfFileNameWithPath">The file name to save the PDF as.</param>
    ''' <returns>
    '''  <see langword="True"/> if the PDF was successfully downloaded; otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>
    '''  <para>
    '''   This method is not implemented for CareLink Partners and will always return <see langword="False"/>.
    '''  </para>
    ''' </remarks>
    Friend Function TryGetDeviceSettingsPdfFile(pdfFileNameWithPath As String) As Boolean
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

    ''' <summary>
    '''  Determines whether the provided access token payload is valid and not expired.
    ''' </summary>
    ''' <param name="access_token_payload">
    '''  The access token payload as a <see cref="Dictionary(Of String, Object)"/>.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the token is valid; otherwise, <see langword="False"/>.
    ''' </returns>
    Private Shared Function IsTokenValid(access_token_payload As Dictionary(Of String, Object)) As Boolean
        Debug.WriteLine(NameOf(IsTokenValid))
        Try
            ' Get expiration time stamp
            Dim unixTimeToValidate As Long = CType(access_token_payload("exp"), JsonElement).GetInt64()

            ' Check expiration time stamp
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = unixTimeToValidate - unixCurrentTime

            If tDiff < 0 Then
                Debug.WriteLine($"In {NameOf(IsTokenValid)} access token has expired {Math.Abs(tDiff)}s ago")
                Return False
            End If

            If tDiff < 600 Then
                Debug.WriteLine($"In {NameOf(IsTokenValid)} access token is about to expire in {tDiff}s")
                Return False
            End If

            ' Token is valid
            Dim authTokenValidTo As String = DateTimeOffset.FromUnixTimeSeconds(unixTimeToValidate) _
                                                           .ToString("ddd MMM dd HH:mm:ss UTC yyyy")
            Debug.WriteLine($"In {NameOf(IsTokenValid)} access token expires in {tDiff}s ({authTokenValidTo})")
            Return True
        Catch ex As Exception
            Debug.WriteLine($"In {NameOf(IsTokenValid)} missing data in access token. {ex.DecodeException()}")
            Return False
        End Try
    End Function

    ''' <summary>
    '''  Initializes the client by reading the token file and setting up configuration and user data.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if initialization is successful; otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>
    '''  <para>
    '''   This method reads the token file, retrieves the access token payload, and sets up the API
    '''   configuration and user information. If the token is invalid or expired, it attempts to refresh
    '''   the token.
    '''  </para>
    ''' </remarks>
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
        Dim configJsonElement As JsonElement
        Try
            Dim element As JsonElement = CType(_accessTokenPayload("token_details"), JsonElement)
            Application.DoEvents()
            Dim payload As AccessTokenDetails = JsonSerializer.Deserialize(Of AccessTokenDetails)(element, s_jsonDeserializerOptions)
            _country = payload.Country
            configJsonElement = GetConfigElement(_httpClient, payload.Country)
            _config = configJsonElement.ConvertJsonElementToDictionary()
            Dim userString As String = Me.GetUserString(configJsonElement, _tokenDataElement)
            If String.IsNullOrWhiteSpace(userString) Then
                Throw New UnauthorizedAccessException
            End If
            _userElementDictionary = If(String.IsNullOrWhiteSpace(userString),
                Nothing,
                JsonSerializer.Deserialize(Of JsonElement)(userString).ConvertJsonElementToDictionary)
            _PatientPersonalData = JsonSerializer.Deserialize(Of PatientPersonalInfo)(userString)

            Dim role As String = _PatientPersonalData.role
            If role.ContainsIgnoreCase("Partner") Then
                _patientElement = Me.GetPatient(configJsonElement, _tokenDataElement).Result
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.ToString())

            If Auth_Error_Codes.Contains(_lastHttpStatusCode) Then
                Try
                    _tokenDataElement = Me.DoRefresh(configJsonElement.ConvertJsonElementToDictionary, _tokenDataElement).Result
                    If Not (_tokenDataElement.ValueKind = JsonValueKind.Undefined OrElse _tokenDataElement.ValueKind = JsonValueKind.Null) Then
                        _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                        WriteTokenFile(value:=_tokenDataElement, userName:=s_userName)
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
    '''  Sends a request to the API to retrieve data for the specified user and role.
    ''' </summary>
    ''' <param name="username">The username for which to retrieve data.</param>
    ''' <param name="role">The role of the user (e.g., "patient" or "carepartner").</param>
    ''' <param name="patientId">The patient ID, if applicable.</param>
    ''' <returns>
    '''  A <see cref="Dictionary(Of String, Object)"/> containing the retrieved data,
    '''  or <see langword="Nothing"/> if the request fails.
    ''' </returns>
    Private Function GetData(username As String, role As String, patientId As String) As Dictionary(Of String, Object)
        Debug.WriteLine(NameOf(GetData))
        _httpClient.SetDefaultRequestHeaders()
        Dim requestUri As String = $"{CStr(_config("baseUrlCumulus"))}/display/message"
        Dim tokenData As Dictionary(Of String, String) = _tokenDataElement.ConvertJsonElementToStringDictionary()

        Dim value As New Dictionary(Of String, Object) From {{"username", username}}

        If role.ContainsIgnoreCase("Partner") Then
            value("role") = "carepartner"
            value("patientId") = patientId
        Else
            value("role") = "patient"
        End If

        _lastHttpStatusCode = 0

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
                _lastHttpStatusCode = response.StatusCode
                Debug.WriteLine($"   status: {_lastHttpStatusCode}")

                If response.IsSuccessStatusCode Then
                    Dim json As String = response.Content.ReadAsStringAsync().Result
                    Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json)
                End If
            End Using
        End Using

        Return Nothing
    End Function

    ''' <summary>
    '''  Retrieves the patient information for the current user asynchronously.
    ''' </summary>
    ''' <param name="configJsonElement">The API configuration as a <see cref="JsonElement"/>.</param>
    ''' <param name="token_data">The token data as a <see cref="JsonElement"/>.</param>
    ''' <returns>
    '''  A <see cref="Task(Of Dictionary(Of String, String))"/> containing the patient information,
    '''  or <see langword="Nothing"/> if not found.
    ''' </returns>
    Private Async Function GetPatient(configJsonElement As JsonElement, token_data As JsonElement) As Task(Of Dictionary(Of String, String))
        Debug.WriteLine(NameOf(GetPatient))
        Dim url As String = $"{CStr(configJsonElement.ConvertJsonElementToDictionary("baseUrlCareLink"))}/links/patients"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        headers("mag-identifier") = CStr(token_data.ConvertJsonElementToDictionary("mag-identifier"))
        headers("Authorization") = $"Bearer {CStr(token_data.ConvertJsonElementToDictionary("access_token"))}"

        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
        Next

        _lastHttpStatusCode = 0
        Using response As HttpResponseMessage = Await _httpClient.GetAsync(url)
            _lastHttpStatusCode = response.StatusCode
            Debug.WriteLine($"   status: {_lastHttpStatusCode}")

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

    ''' <summary>
    '''  Retrieves the user information string from the API.
    ''' </summary>
    ''' <param name="config">The API configuration as a <see cref="JsonElement"/>.</param>
    ''' <param name="tokenData">The token data as a <see cref="JsonElement"/>.</param>
    ''' <returns>
    '''  A <see langword="String"/> containing the user information in JSON format,
    '''  or <see langword="Nothing"/> if the request fails.
    ''' </returns>
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
        _lastHttpStatusCode = response.StatusCode
        Debug.WriteLine($"   status: {_lastHttpStatusCode}")

        Return If(response.IsSuccessStatusCode,
            response.Content.ReadAsStringAsync().Result,
            Nothing)
    End Function

    ''' <summary>
    '''  Gets the last <see cref="HttpStatusCode"/> received from the API.
    ''' </summary>
    ''' <returns>
    '''  An <see langword="Integer"/> representing the last API response status code.
    ''' </returns>
    Public Function GetHttpStatusCode() As HttpStatusCode
        Return _lastHttpStatusCode
    End Function

    ''' <summary>
    '''  Retrieves the most recent data from the API.
    '''  This function checks if the access token is valid, attempts to refresh it if necessary,
    '''  and then sends a request to the API to obtain the latest patient data.
    '''  If the access token cannot be refreshed or the API call fails, an error message is returned.
    ''' </summary>
    ''' <returns>
    '''  This function checks if the access token is valid, attempts to refresh it if necessary, and then
    '''  sends a request to the API to obtain the latest patient data. If the access token cannot be
    '''  refreshed or the API call fails, an error message is returned.
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
        If Auth_Error_Codes.Contains(_lastHttpStatusCode) Then
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
        Dim unusedMetaData As JsonElement = CType(data.Values(index:=0), JsonElement)
        Try
            PatientDataElement = CType(data.Values(1), JsonElement)
            DeserializePatientElement()
            File.WriteAllTextAsync(
                path:=GetLastDownloadFileWithPath(),
                contents:=JsonSerializer.Serialize(value:=PatientDataElement, options:=s_jsonSerializerOptions))
        Catch ex As Exception
            Stop
            Return ex.DecodeException()
        End Try

        Return lastErrorMessage
    End Function

    ''' <summary>
    '''  Initializes the client and attempts to authenticate and set up user data.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if initialization is successful; otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>
    '''  <para>
    '''   This method attempts to initialize the client twice, refreshing the token if necessary. If both
    '''   attempts fail, it returns <see langword="False"/>.
    '''  </para>
    ''' </remarks>
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
