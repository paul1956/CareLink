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
    Private Const TokenBaseFileName As String = "loginData.json"

    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _tokenBaseFileName As String
    Private _accessTokenPayload As Dictionary(Of String, Object)
    Private _config As Dictionary(Of String, Object)
    Private _country As String
    Private _lastHttpStatusCode As HttpStatusCode
    Private _patientElement As Dictionary(Of String, String)
    Private _tokenDataElement As JsonElement

    ''' <summary>
    '''  Initializes a new instance of the <see cref="Client2"/> class.
    ''' </summary>
    ''' <param name="tokenFile">The file path for the token nameValueCollection.</param>
    Public Sub New(Optional tokenFile As String = TokenBaseFileName, Optional httpClient As HttpClient = Nothing)
        _tokenBaseFileName = tokenFile
        _tokenDataElement = Nothing
        _accessTokenPayload = Nothing
        _config = Nothing
        _patientElement = Nothing
        _country = Nothing
        _lastHttpStatusCode = 0

        _httpClient = If(httpClient, New HttpClient)
        _httpClient.SetDefaultRequestHeaders()
    End Sub

    Public Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}

    Public Property Config As Dictionary(Of String, Object)
        Get
            Return _config
        End Get
        Set(value As Dictionary(Of String, Object))
            _config = value
        End Set
    End Property

    Public Property LoggedIn As Boolean
    Public Property PatientPersonalData As New PatientPersonalInfo
    Public Property UserElementDictionary As Dictionary(Of String, Object)

    Private Shared Function GetAccessTokenPayload(token_data As JsonElement) As Dictionary(Of String, Object)
        Debug.WriteLine(message:=NameOf(GetAccessTokenPayload))
        Try
            Dim token As String = CStr(token_data.ToObjectDictionary("access_token"))
            Dim payload_b64 As String = token.Split(separator:="."c)(1)
            Dim payload_b64_bytes As Byte() = Encoding.UTF8.GetBytes(s:=payload_b64)
            Dim count As Integer = (4 - (payload_b64_bytes.Length Mod 4)) Mod 4
            If count > 0 Then
                payload_b64 &= New String(c:="="c, count)
            End If
            Dim bytes As Byte() = Convert.FromBase64String(s:=payload_b64)
            Dim json As String = Encoding.UTF8.GetString(bytes)
            Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json)
        Catch ex As Exception
            Dim str As String = ex.DecodeException()
            Dim location As String = NameOf(GetAccessTokenPayload)
            Dim message As String = $"No access token found or malformed access token: {str} in {location}"
            Debug.WriteLine(message)
            Stop
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Validates the access token based on its expiration time.
    ''' </summary>
    ''' <param name="access_token_payload">The payload of the access token as a dictionary.</param>
    ''' <returns>True if the token is valid; otherwise, false.</returns>
    Private Shared Function IsTokenValid(access_token_payload As Dictionary(Of String, Object)) As Boolean
        Debug.WriteLine(NameOf(IsTokenValid))
        Try
            Dim unixTime As Long = CType(access_token_payload("exp"), JsonElement).GetInt64()
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = unixTime - unixCurrentTime
            Dim message As String
            If tDiff < 0 Then
                Dim absDiff As Long = Math.Abs(tDiff)
                message = $"In {NameOf(IsTokenValid)} access token has expired {absDiff}s ago"
                Debug.WriteLine(message)
                Return False
            End If

            If tDiff < 600 Then
                message = $"In {NameOf(IsTokenValid)} access token is about to expire in {tDiff}s"
                Debug.WriteLine(message)
                Return False
            End If

            Const format As String = "ddd MMM dd HH:mm:ss UTC yyyy"
            Dim authTokenValidTo As String = DateTimeOffset.FromUnixTimeSeconds(seconds:=unixTime).ToString(format)
            message = $"In {NameOf(IsTokenValid)} access token expires in {tDiff} seconds at {authTokenValidTo}"
            Debug.WriteLine(message)
            Return True
        Catch ex As Exception
            Debug.WriteLine(message:=$"In {NameOf(IsTokenValid)} missing nameValueCollection in access token. {ex.DecodeException()}")
            Return False
        End Try
    End Function

    ''' <summary>
    '''  Async variant of DoRefresh that uses Await.
    ''' </summary>
    ''' <param name="config">Configuration settings as a dictionary.</param>
    ''' <param name="element">
    '''  The JSON element containing token information.
    ''' </param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing the refreshed token as a JSON element.
    ''' </returns>
    Private Async Function DoRefreshAsync(config As Dictionary(Of String, Object),
                                          element As JsonElement) As Task(Of JsonElement)

        Debug.WriteLine(message:=NameOf(DoRefreshAsync))
        _httpClient.SetDefaultRequestHeaders()

        Dim tokenData As Dictionary(Of String, Object) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(element)

        Dim data As New Dictionary(Of String, String) From {
            {"refresh_token", tokenData(key:="refresh_token").ToString()},
            {"client_id", tokenData(key:="client_id").ToString()},
            {"client_secret", tokenData(key:="client_secret").ToString()},
            {"grant_type", "refresh_token"}}

        Dim content As New FormUrlEncodedContent(nameValueCollection:=data)

        Dim headers As New Dictionary(Of String, String) From {
            {"mag-identifier", tokenData(key:="mag-identifier").ToString()}}

        For Each header As KeyValuePair(Of String, String) In headers
            If Not _httpClient.DefaultRequestHeaders.Contains(header.Key) Then
                _httpClient.DefaultRequestHeaders.Add(name:=header.Key, header.Value)
            End If
        Next

        Dim requestUri As String = CStr(config(key:="token_url"))
        Using response As HttpResponseMessage = Await _httpClient.PostAsync(requestUri, content).ConfigureAwait(False)
            _lastHttpStatusCode = response.StatusCode
            Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

            response.ThrowIfFailure()

            Dim responseBody As String = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
            Dim newData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json:=responseBody)
            tokenData(key:="access_token") = newData.GetProperty(propertyName:="access_token").GetString()
            tokenData(key:="refresh_token") = newData.GetProperty(propertyName:="refresh_token").GetString()
            Dim json As String = JsonSerializer.Serialize(value:=tokenData, options:=s_jsonSerializerOptions)
            Return JsonSerializer.Deserialize(Of JsonElement)(json)
        End Using
    End Function

    ''' <summary>
    '''  Async version of GetData that uses Await and centralized response inspection.
    '''  This variant implements a small retry loop for transient failures.
    ''' </summary>
    ''' <param name="username">The username for the nameValueCollection request.</param>
    ''' <param name="role">The role of the user (e.g., patient, carePartner).</param>
    ''' <param name="patientId">The patient ID, if applicable.</param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing the requested nameValueCollection as a dictionary.
    ''' </returns>
    Private Async Function GetDataAsync(
        username As String,
        role As String,
        patientId As String) As Task(Of Dictionary(Of String, Object))

        _httpClient.SetDefaultRequestHeaders()
        Dim requestUri As String = $"{CStr(Me.Config("baseUrlCumulus"))}/display/message"
        Dim tokenData As Dictionary(Of String, String) = _tokenDataElement.ToStringDictionary()
        Dim value As New Dictionary(Of String, Object) From {{"username", username}}

        If role.ContainsNoCase("Partner") Then
            value("role") = "carePartner".ToLower
            value("patientId") = patientId
        Else
            value("role") = "patient"
        End If

        _lastHttpStatusCode = 0

        Dim headers As New Dictionary(Of String, String)
        headers("mag-identifier") = tokenData("mag-identifier")
        headers("Authorization") = $"Bearer {tokenData("access_token")}"
        For Each header As KeyValuePair(Of String, String) In headers
            If Not _httpClient.DefaultRequestHeaders.Contains(header.Key) Then
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
            End If
        Next

        Using content As New StringContent(
            content:=JsonSerializer.Serialize(value),
            encoding:=Encoding.UTF8,
            mediaType:="application/json")

            Const maxAttempts As Integer = 3
            Dim attempt As Integer = 0
            Dim lastEx As Exception = Nothing

            While attempt < maxAttempts
                attempt += 1
                Dim needRetry As Boolean = False
                Dim retryDelayMs As Integer = 0
                Try
                    Using response As HttpResponseMessage =
                        Await _httpClient.PostAsync(requestUri, content).ConfigureAwait(False)

                        _lastHttpStatusCode = response.StatusCode
                        Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

                        ' Centralized response inspection; may throw UnauthorizedAccessException,
                        ' ArgumentException (bad request) or HttpRequestException (transient/server).
                        response.ThrowIfFailure()

                        Dim json As String = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
                        Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json)
                    End Using
                Catch hex As HttpRequestException
                    lastEx = hex
                    ' Prepare retry information; do not Await inside Catch.
                    If attempt >= maxAttempts Then
                        ' no retry left
                        needRetry = False
                    Else
                        needRetry = True
                        retryDelayMs = CInt(200 * Math.Pow(2, attempt - 1))
                    End If
                End Try

                If needRetry Then
                    Await Task.Delay(retryDelayMs).ConfigureAwait(False)
                    Continue While
                End If
            End While

            If lastEx IsNot Nothing Then
                Throw lastEx
            End If

        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Retrieves patient information asynchronously.
    ''' </summary>
    ''' <param name="configJsonElement">The configuration JSON element containing base URL information.</param>
    ''' <param name="token_data">The token nameValueCollection JSON element containing authentication tokens.</param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing a dictionary of patient information.
    ''' </returns>
    Private Async Function GetPatient(configJsonElement As JsonElement,
                                      token_data As JsonElement) As Task(Of Dictionary(Of String, String))

        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        headers(key:="mag-identifier") = CStr(token_data.ToObjectDictionary(key:="mag-identifier"))
        headers(key:="Authorization") = $"Bearer {CStr(token_data.ToObjectDictionary(key:="access_token"))}"

        For Each header As KeyValuePair(Of String, String) In headers
            _httpClient.DefaultRequestHeaders.Add(name:=header.Key, header.Value)
        Next

        _lastHttpStatusCode = 0
        Const key As String = "baseUrlCareLink"
        Dim requestUri As String = $"{CStr(configJsonElement.ToObjectDictionary(key))}/links/patients"
        Using response As HttpResponseMessage = Await _httpClient.GetAsync(requestUri).ConfigureAwait(False)
            _lastHttpStatusCode = response.StatusCode
            Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

            ' Ensure non-success status codes are not silently ignored.
            Try
                response.ThrowIfFailure()
            Catch ex As Exception
                Debug.WriteLine($"GetPatient HTTP failure: {ex.Message}")
                Return Nothing
            End Try

            Dim patients As List(Of Dictionary(Of String, String))
            Dim json As String = Await response.Content.ReadAsStringAsync().ConfigureAwait(False)
            patients = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, String)))(json)
            If patients.Count > 0 Then
                Return patients(index:=0)
            End If
        End Using

        Return Nothing
    End Function

    ''' <summary>
    '''  Retrieves user information as a JSON string.
    ''' </summary>
    ''' <param name="config">The configuration JSON element containing base URL information.</param>
    ''' <param name="tokenData">The token nameValueCollection JSON element containing authentication tokens.</param>
    ''' <returns>A JSON string representing the user information.</returns>
    Private Function GetUserString(config As JsonElement, tokenData As JsonElement) As String
        Dim requestUri As String = $"{config.GetProperty(propertyName:="baseUrlCareLink").GetString()}/users/me"
        Dim headers As New Dictionary(Of String, String)
        headers("mag-identifier") = tokenData.GetProperty(propertyName:="mag-identifier").GetString()
        headers("Authorization") = $"Bearer {tokenData.GetProperty(propertyName:="access_token").GetString()}"
        headers("Accept-Language") = "en-US"

        Dim request As New HttpRequestMessage(HttpMethod.Get, requestUri)
        Dim item As New MediaTypeWithQualityHeaderValue("application/json")
        request.Headers.Accept.Add(item)
        For Each header As KeyValuePair(Of String, String) In headers.Sort
            request.Headers.Add(header.Key, header.Value)
        Next

        Dim response As HttpResponseMessage = _httpClient.SendAsync(request).Result
        _lastHttpStatusCode = response.StatusCode
        Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

        ' Use centralized failure handling and translate to Nothing for older call-sites.
        Try
            response.ThrowIfFailure()
        Catch ex As UnauthorizedAccessException
            Debug.WriteLine($"GetUserString unauthorized: {ex.Message}")
            Return Nothing
        Catch ex As ArgumentException
            Debug.WriteLine($"GetUserString bad request: {ex.Message}")
            Return Nothing
        Catch ex As HttpRequestException
            Debug.WriteLine($"GetUserString HTTP error: {ex.Message}")
            Return Nothing
        End Try

        Return response.Content.ReadAsStringAsync().Result
    End Function

    ''' <summary>
    '''  Initializes the client by reading token nameValueCollection and user information.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing a boolean indicating success or failure.
    ''' </returns>
    Private Async Function internalInit() As Task(Of Boolean)
        _tokenDataElement = ReadTokenFile(userName:=s_userName, tokenBaseFileName:=_tokenBaseFileName)
        If _tokenDataElement.ValueKind.IsNullOrUndefined Then
            Me.LoggedIn = False
            Return False
        End If

        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
        If _accessTokenPayload Is Nothing Then
            Return False
        End If

        Dim refreshTask As Task(Of JsonElement) = Nothing
        Dim hadException As Boolean = False
        Dim configJsonElement As JsonElement

        Try
            Application.DoEvents()
            Dim element As JsonElement = CType(_accessTokenPayload(key:="token_details"), JsonElement)

            Dim options As JsonSerializerOptions = s_jsonDesterilizeOptions
            Dim payload As AccessTokenDetails = JsonSerializer.Deserialize(Of AccessTokenDetails)(element, options)
            _country = If(payload.Country, s_countryCode)
            configJsonElement = GetConfigElement(httpClient:=_httpClient, country:=_country)
            Me.Config = configJsonElement.ToObjectDictionary()

            ' Call user string; handle typed failures
            Dim json As String = Me.GetUserString(config:=configJsonElement, tokenData:=_tokenDataElement)
            If IsNullOrWhiteSpace(value:=json) Then
                Throw New UnauthorizedAccessException
            End If

            Me.UserElementDictionary = JsonSerializer.Deserialize(Of JsonElement)(json).ToObjectDictionary()
            _PatientPersonalData = JsonSerializer.Deserialize(Of PatientPersonalInfo)(json)

            Dim role As String = _PatientPersonalData.role
            If role.ContainsNoCase(value:="Partner") Then
                _patientElement =
                    Await Me.GetPatient(configJsonElement, token_data:=_tokenDataElement).ConfigureAwait(False)
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.ToString())

            hadException = True

            If Auth_Error_Codes.Contains(_lastHttpStatusCode) Then
                ' Start refresh task without Await inside Catch
                Try
                    If Not configJsonElement.ValueKind = JsonValueKind.Undefined Then
                        refreshTask = Me.DoRefreshAsync(
                            config:=configJsonElement.ToObjectDictionary,
                            element:=_tokenDataElement)
                    End If
                Catch innerEx As Exception
                    Debug.WriteLine(innerEx.ToString())
                End Try
            End If
        End Try

        ' If an exception occurred in the Try block, handle refresh attempt now (outside Catch).
        If hadException Then
            If refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement = Await refreshTask.ConfigureAwait(False)
                    If Not (refreshedToken.ValueKind = JsonValueKind.Undefined OrElse
                            refreshedToken.ValueKind = JsonValueKind.Null) Then
                        _tokenDataElement = refreshedToken
                        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                        WriteTokenFile(value:=_tokenDataElement, userName:=s_userName)
                    End If
                Catch refreshEx As Exception
                    Debug.WriteLine(refreshEx.ToString())
                End Try
            End If

            Me.LoggedIn = False
            Return False
        End If

        Me.LoggedIn = True
        Return True
    End Function

    ''' <summary>
    '''  Gets the last HTTP status code from the most recent operation.
    ''' </summary>
    ''' <returns>The last HTTP status code.</returns>
    Public Function GetHttpStatusCode() As HttpStatusCode
        Return _lastHttpStatusCode
    End Function

    ''' <summary>
    '''  Synchronous wrapper for GetRecentDataAsync.
    ''' </summary>
    ''' <returns>A JSON string representing the recent nameValueCollection.</returns>
    Public Function GetRecentData() As String
        Return Me.GetRecentDataAsync().GetAwaiter().GetResult()
    End Function

    ''' <summary>
    '''  Async variant of GetRecentData that uses Await and centralized response inspection.
    ''' </summary>
    ''' <returns>A JSON string representing the recent nameValueCollection.</returns>
    Public Async Function GetRecentDataAsync() As Task(Of String)
        Dim lastErrorMessage As String
        Dim refreshTask As Task(Of JsonElement) = Nothing
        Dim hadAuthException As Boolean = False

        If Not IsTokenValid(_accessTokenPayload) Then
            Try
                _tokenDataElement = Await Me.DoRefreshAsync(Me.Config, _tokenDataElement).ConfigureAwait(False)
                _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                WriteTokenFile(_tokenDataElement, s_userName)
            Catch ex As Exception
                Debug.WriteLine(ex.ToString())
            End Try

            If Not IsTokenValid(_accessTokenPayload) Then
                lastErrorMessage = "ERROR: unable to get valid access token"
                Debug.WriteLine(lastErrorMessage)
                Return lastErrorMessage
            End If
        End If

        Dim data As Dictionary(Of String, Object) = Nothing
        Try
            Dim role As String = CStr(Me.UserElementDictionary("role"))
            ' Call GetDataAsync and handle typed exceptions without Await inside Catch.
            Try
                data = Await Me.GetDataAsync(username:=s_userName,
                                             role:=role,
                                             patientId:=EmptyString).ConfigureAwait(False)
            Catch uaEx As UnauthorizedAccessException
                ' schedule refresh, will await below and then retry once
                hadAuthException = True
                Try
                    refreshTask = Me.DoRefreshAsync(config:=Me.Config, element:=_tokenDataElement)
                Catch innerEx As Exception
                    Debug.WriteLine(innerEx.ToString())
                End Try
            Catch argEx As ArgumentException
                Debug.WriteLine($"GetRecentData bad request: {argEx.Message}")
                Return argEx.Message
            Catch httpEx As HttpRequestException
                Debug.WriteLine($"GetRecentData network/server error: {httpEx.Message}")
                Return $"Network/server error: {httpEx.Message}"
            End Try

            ' If we scheduled a refresh due to auth, await it now and retry GetDataAsync once.
            If hadAuthException AndAlso refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement = Await refreshTask.ConfigureAwait(False)
                    If Not (refreshedToken.ValueKind =
                        JsonValueKind.Undefined OrElse refreshedToken.ValueKind = JsonValueKind.Null) Then

                        _tokenDataElement = refreshedToken
                        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                        WriteTokenFile(value:=_tokenDataElement, userName:=s_userName)
                        ' retry
                        data = Await Me.GetDataAsync(username:=s_userName,
                                                     role:=role,
                                                     patientId:=EmptyString).ConfigureAwait(False)
                    End If
                Catch refreshEx As Exception
                    Debug.WriteLine(refreshEx.ToString())
                    Return "ERROR: failed to refresh token"
                End Try
            End If

            If data Is Nothing OrElse
               data.Count = 0 OrElse
               (data.Count = 2 AndAlso CType(data("patientData"), JsonElement).ValueKind <> JsonValueKind.Object) Then

                PatientData = Nothing
                RecentData = Nothing
                Dim message As String =
                    $"{NameOf(GetRecentDataAsync)}: No nameValueCollection returned from GetData for user {s_userName}"
                Debug.WriteLine(message)
                Return "No nameValueCollection received from server"
            End If
        Catch ex As Exception
            PatientData = Nothing
            RecentData = Nothing
            Debug.WriteLine(ex.DecodeException())
            Return ex.DecodeException()
        End Try

        ' If a call earlier produced an auth status code, attempt refresh proactively.
        If Auth_Error_Codes.Contains(_lastHttpStatusCode) Then
            Try
                _tokenDataElement = Await Me.DoRefreshAsync(Me.Config, _tokenDataElement).ConfigureAwait(False)
                _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                WriteTokenFile(_tokenDataElement, s_userName)
            Catch ex As Exception
                Debug.WriteLine(ex.ToString())
            End Try
        End If

        Dim lastError As String = Nothing
        Select Case data.Keys.Count
            Case 0
                lastError = "No Data Found"
            Case 1
                lastError = $"No Data Found for {data.Keys(index:=0)}"
            Case 2
            Case Else
                lastError = $"No Data Found for {String.Join(separator:=", ", values:=data.Keys)}"
        End Select

        If data.Values.Count < 2 Then
            Return lastError
        End If

        Dim unusedMetaData As JsonElement = CType(data.Values(index:=0), JsonElement)
        Try
            PatientDataElement = CType(data.Values(index:=1), JsonElement)
            DeserializePatientElement()
            File.WriteAllText(
                path:=GetLastDownloadFileWithPath(),
                contents:=JsonSerializer.Serialize(value:=PatientDataElement, options:=s_jsonSerializerOptions))
        Catch ex As Exception
            Debug.WriteLine(ex.DecodeException())
            Return ex.DecodeException()
        End Try

        Return lastError
    End Function

    ''' <summary>
    '''  Synchronous wrapper for InitAsync.
    ''' </summary>
    ''' <returns>True if initialization succeeded; otherwise, False.</returns>
    Public Function Init() As Boolean
        Return Me.InitAsync().GetAwaiter().GetResult()
    End Function

    ''' <summary>
    '''  Asynchronous initialization function that prepares the client for use.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing True if initialization succeeded; otherwise, False.
    ''' </returns>
    Public Async Function InitAsync() As Task(Of Boolean)
        If Not Await Me.internalInit().ConfigureAwait(False) Then
            If Not Await Me.internalInit().ConfigureAwait(False) Then
                Return False
            End If
        End If
        Return True
    End Function

End Class
