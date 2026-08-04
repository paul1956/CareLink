' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Text.Json

' This class is intentionally not part of the public API.
' It is designed to be used internally within the assembly and is not intended for external consumption.
Friend Class Client2
    Private Const TokenBaseFileName As String = "loginData.json"
    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _tokenBaseFileName As String
    Private _accessTokenPayload As Dictionary(Of String, Object)
    Private _config As Dictionary(Of String, String)
    Private _country As String
    Private _lastHttpStatusCode As HttpStatusCode
    Private _patientElement As New Dictionary(Of String, String)
    Private _tokenDataElement As JsonElement

    ''' <summary>
    '''  Initializes a new instance of the <see cref="Client2"/> class.
    ''' </summary>
    ''' <param name="serverRegion">Indicates whether the region is US.</param>
    ''' <param name="httpClient">The HTTP client to use for requests.</param>
    ''' <param name="tokenFile">The file path for the token nameValueCollection.</param>
    ''' <remarks>
    '''  This Class is intentionally not part of the public API.
    ''' </remarks>
    Friend Sub New(serverRegion As Region,
                   Optional httpClient As HttpClient = Nothing,
                   Optional tokenFile As String = TokenBaseFileName)

        _tokenBaseFileName = tokenFile
        _tokenDataElement = Nothing
        _accessTokenPayload = Nothing
        _config = Nothing
        _patientElement = Nothing
        _country = Nothing
        _lastHttpStatusCode = 0
        Me.serverRegion = serverRegion
        _httpClient = If(httpClient, New HttpClient)
        _httpClient.SetDefaultRequestHeaders()
    End Sub

    Friend Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}

    Friend Property Config As Dictionary(Of String, String)
        Get
            Return _config
        End Get
        Set(value As Dictionary(Of String, String))
            _config = value
        End Set
    End Property

    Friend Property serverRegion As Region

    Friend Property LoggedIn As Boolean

    Friend Property PatientPersonalData As New PatientPersonalInfo

    Friend Property UserElementDictionary As Dictionary(Of String, Object)

    Private Enum DataKeyCount
        NoData
        SingleData
        RecentData
    End Enum

    ''' <summary>
    ''' Build request headers from tokenDataElement.
    ''' </summary>
    Private Shared Function BuildHeaders(token_data As JsonElement) As Dictionary(Of String, String)
        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        Dim magId As String = Nothing
        If TryGetStringProperty(element:=token_data, propertyName:="mag-identifier", value:=magId) Then
            If Not String.IsNullOrWhiteSpace(value:=magId) Then
                headers(key:="mag-identifier") = magId
            End If
        End If

        Dim access As String = Nothing
        If TryGetStringProperty(element:=token_data, propertyName:="access_token", value:=access) Then
            headers(key:="Authorization") = $"Bearer {access}"
        End If

        Return headers
    End Function

    ''' <summary>
    ''' Build request headers preferring config mag-identifier then tokenDataElement.
    ''' </summary>
    Private Shared Function BuildHeaders(configJsonElement As JsonElement, token_data As JsonElement) As Dictionary(Of String, String)
        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        Dim magId As String = Nothing
        If TryGetStringProperty(element:=configJsonElement, propertyName:="mag-identifier", value:=magId) Then
            If Not String.IsNullOrWhiteSpace(value:=magId) Then
                headers(key:="mag-identifier") = magId
            End If
        ElseIf TryGetStringProperty(element:=token_data, propertyName:="mag-identifier", value:=magId) Then
            If Not String.IsNullOrWhiteSpace(value:=magId) Then
                headers(key:="mag-identifier") = magId
            End If
        End If

        Dim access As String = Nothing
        If TryGetStringProperty(element:=token_data, propertyName:="access_token", value:=access) Then
            headers(key:="Authorization") = $"Bearer {access}"
        End If

        Return headers
    End Function

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
    ''' <param name="message"></param>
    Private Shared Function IsTokenValid(access_token_payload As Dictionary(Of String, Object), ByRef message As String) As Boolean
        Try
            Dim unixTime As Long = CType(access_token_payload(key:="exp"), JsonElement).GetInt64()
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = unixTime - unixCurrentTime
            If tDiff < 0 Then
                Dim absDiff As Long = Math.Abs(value:=tDiff)
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
            message =
                $"In {NameOf(IsTokenValid)} missing nameValueCollection in access token. {ex.DecodeException()}"
            Debug.WriteLine(message)
            Return False
        End Try
    End Function

    ''' <summary>
    '''  Async version of GetData that uses Await and centralized response inspection.
    '''  This variant implements a small retry loop for transient failures.
    ''' </summary>
    ''' <param name="username">The username for the nameValueCollection request.</param>
    ''' <param name="role">The role of the user (e.g., patient, carePartner).</param>
    ''' <param name="patientId">The patient ID, if applicable.</param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing the requested
    '''  nameValueCollection as a dictionary.
    ''' </returns>
    Private Async Function GetDataAsync(
        username As String,
        role As String,
        patientId As String) As Task(Of Dictionary(Of String, Object))

        _httpClient.SetDefaultRequestHeaders()
        Dim requestUri As String = $"{Me.Config(key:="baseUrlCumulus")}/display/message"
        Dim tokenData As Dictionary(Of String, String) = _tokenDataElement.ToStringDictionary()
        Dim value As New Dictionary(Of String, Object) From {{"username", username}}
        If role.ContainsNoCase(value:="Partner") Then
            value(key:="role") = "carePartner".ToLower()
            value(key:="patientId") = patientId
        Else
            value(key:="role") = "patient"
        End If
        value(key:="appVersion") = "3.6.0"

        _lastHttpStatusCode = 0

        Dim headers As New Dictionary(Of String, String)
        headers(key:="Authorization") = $"Bearer {tokenData(key:="access_token")}"
        Dim magidentifier As String = Nothing
        If tokenData.TryGetValue(key:="mag-identifier", value:=magidentifier) AndAlso Not String.IsNullOrWhiteSpace(value:=magidentifier) Then
            headers(key:="mag-identifier") = magidentifier
        End If

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
                    Using request As New HttpRequestMessage(method:=HttpMethod.Post,
                                                            requestUri:=requestUri) With {.Content = content}
                        For Each header As KeyValuePair(Of String, String) In headers
                            request.Headers.TryAddWithoutValidation(name:=header.Key, value:=header.Value)
                        Next

                        Using response As HttpResponseMessage = _httpClient.Send(request)
                            _lastHttpStatusCode = response.StatusCode
                            Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

                            ' Centralized response inspection; may throw UnauthorizedAccessException,
                            ' ArgumentException (bad request) or HttpRequestException (transient/server).
                            response.ThrowIfFailure()

                            Dim json As String =
                                Await response.Content.ReadAsStringAsync() _
                                                      .ConfigureAwait(continueOnCapturedContext:=False)
                            Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json)
                        End Using
                    End Using
                Catch hex As HttpRequestException
                    lastEx = hex
                    ' Prepare retry information; do not Await inside Catch.
                    If attempt >= maxAttempts Then
                        ' no retry left
                        needRetry = False
                    Else
                        needRetry = True
                        retryDelayMs = CInt(200 * Math.Pow(x:=2, y:=attempt - 1))
                    End If
                End Try

                If needRetry Then
                    Await Task.Delay(millisecondsDelay:=retryDelayMs).ConfigureAwait(continueOnCapturedContext:=True)
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
    ''' <param name="configJsonElement">The configuration JSON tokenDataElement containing base URL information.</param>
    ''' <param name="token_data">The token nameValueCollection JSON tokenDataElement containing authentication tokens.</param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing a dictionary of patient information.
    ''' </returns>
    Private Async Function GetPatient(configJsonElement As JsonElement,
                                      token_data As JsonElement) As Task(Of Dictionary(Of String, String))

        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        Dim magId As String = Nothing
        If TryGetStringProperty(configJsonElement, propertyName:="mag-identifier", value:=magId) Then
            headers(key:="mag-identifier") = magId
        End If

        headers = BuildHeaders(configJsonElement, token_data)

        _lastHttpStatusCode = 0
        Const key As String = "baseUrlCareLink"
        Dim requestUri As String = $"{CStr(configJsonElement.ToObjectDictionary(key))}/links/patients"
        Using request As New HttpRequestMessage(method:=HttpMethod.Get, requestUri:=requestUri)
            For Each header As KeyValuePair(Of String, String) In headers
                request.Headers.TryAddWithoutValidation(name:=header.Key, value:=header.Value)
            Next

            Using response As HttpResponseMessage =
                Await _httpClient.SendAsync(request).
                                  ConfigureAwait(continueOnCapturedContext:=True)
                _lastHttpStatusCode = response.StatusCode
                Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

                ' Ensure non-success status codes are not silently ignored.
                Try
                    response.ThrowIfFailure()
                Catch ex As Exception
                    response.Dispose()
                    Debug.WriteLine(message:=$"GetPatient HTTP failure: {ex.Message}")
                    Return Nothing
                End Try

                Dim patients As List(Of Dictionary(Of String, String))
                Dim json As String =
                    Await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:=True)
                patients = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, String)))(json)
                If patients.Count > 0 Then
                    Return patients(index:=0)
                End If
            End Using
        End Using

        Return Nothing
    End Function

    ''' <summary>
    '''  Retrieves user information as a JSON string.
    ''' </summary>
    ''' <param name="config">The configuration JSON tokenDataElement containing base URL information.</param>
    ''' <param name="tokenData">The token nameValueCollection JSON tokenDataElement containing authentication tokens.</param>
    ''' <returns>A JSON string representing the user information.</returns>
    Private Function GetUserString(config As JsonElement, tokenData As JsonElement) As String
        Dim requestUri As String = $"{config.GetProperty(propertyName:="baseUrlCareLink").GetString()}/users/me"
        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        Dim magId As String = Nothing
        If TryGetStringProperty(tokenData, propertyName:="mag-identifier", value:=magId) Then
            headers(key:="mag-identifier") = magId
        End If
        headers(key:="Authorization") = $"Bearer {tokenData.GetProperty(propertyName:="access_token").GetString()}"
        headers(key:="Accept-Language") = "en-US"

        Using request As New HttpRequestMessage(method:=HttpMethod.Get, requestUri:=requestUri)
            request.Headers.Accept.Add(item:=New MediaTypeWithQualityHeaderValue(mediaType:="application/json"))
            For Each header As KeyValuePair(Of String, String) In headers.Sort
                request.Headers.Add(name:=header.Key, header.Value)
            Next

            Using response As HttpResponseMessage = _httpClient.SendAsync(request).Result
                _lastHttpStatusCode = response.StatusCode
                Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

                ' Use centralized failure handling and translate to Nothing for older call-sites.
                Try
                    response.ThrowIfFailure()
                Catch ex As UnauthorizedAccessException
                    Debug.WriteLine(message:=$"GetUserString unauthorized: {ex.Message}")
                    Return Nothing
                Catch ex As ArgumentException
                    Debug.WriteLine(message:=$"GetUserString bad request: {ex.Message}")
                    Return Nothing
                Catch ex As HttpRequestException
                    Debug.WriteLine(message:=$"GetUserString HTTP error: {ex.Message}")
                    Return Nothing
                End Try

                Return response.Content.ReadAsStringAsync().Result
            End Using
        End Using
    End Function

    ''' <summary>
    '''  Asynchronous initialization function that prepares the client for use.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing True if initialization succeeded;
    '''  otherwise, False.
    ''' </returns>
    Friend Async Function InitAsync() As Task(Of Boolean)
        If Not Await Me.internalInit().ConfigureAwait(continueOnCapturedContext:=True) Then
            Await GetLoginData(Me.serverRegion, tokenData:=Nothing)
            If Not Await Me.internalInit().ConfigureAwait(continueOnCapturedContext:=True) Then
                Return False
            End If
        End If
        Return True
    End Function

    ''' <summary>
    '''  Initializes the client by reading token nameValueCollection and user information.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing a boolean indicating success or failure.
    ''' </returns>
    Private Async Function internalInit() As Task(Of Boolean)
        _tokenDataElement = ReadTokenFile(tokenBaseFileName:=_tokenBaseFileName)
        If _tokenDataElement.IsNullOrUndefined Then
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
            configJsonElement = GetConfig(httpClient:=_httpClient, country:=_country, serverRegion:=Me.serverRegion)
            Me.Config = configJsonElement.ToStringDictionary()

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
                    Await Me.GetPatient(configJsonElement, token_data:=_tokenDataElement) _
                            .ConfigureAwait(continueOnCapturedContext:=True)
            End If
        Catch ex As Exception
            hadException = True

            If Auth_Error_Codes.Contains(value:=_lastHttpStatusCode) Then
                ' Start refresh task without Await inside Catch
                Try
                    If Not configJsonElement.ValueKind = JsonValueKind.Undefined Then
                        Dim config As ConfigRecord =
                            JsonSerializer.Deserialize(Of ConfigRecord)(
                                json:=JsonSerializer.Serialize(value:=configJsonElement),
                                options:=s_jsonDesterilizeOptions)
                        refreshTask = Me.DoRefreshAsync(config:=configJsonElement.ToStringDictionary(),
                                                        tokenElement:=_tokenDataElement)
                    End If
                Catch innerEx As Exception
                    Debug.WriteLine(message:=innerEx.ToString())
                End Try
            End If
        End Try

        ' If an exception occurred in the Try block, handle refresh attempt now (outside Catch).
        If hadException Then
            If refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement =
                        Await refreshTask.ConfigureAwait(continueOnCapturedContext:=false)
                    If Not refreshedToken.IsNullOrUndefined Then
                        _tokenDataElement = refreshedToken
                        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                        WriteTokenFile(token:=_tokenDataElement)
                    End If
                Catch refreshEx As Exception
                    Debug.WriteLine(message:=refreshEx.ToString())
                End Try
            End If

            Me.LoggedIn = False
            Return False
        End If

        Me.LoggedIn = True
        Return True
    End Function

    ''' <summary>
    '''  Synchronous wrapper for InitAsync.
    ''' </summary>
    ''' <returns>True if initialization succeeded; otherwise, False.</returns>
    Friend Function Init() As Boolean
        ' Safe synchronous wrapper:
        ' - If called from a background thread, dispatch InitAsync to the UI thread and
        '   synchronously wait for the unwrapped Task to complete (safe; UI thread is not
        '   blocked by the caller).
        ' - If called from the UI thread, throw to avoid deadlock; callers on the UI
        '   thread should use Await InitAsync() instead.
        Dim app As System.Windows.Application = System.Windows.Application.Current
        If app IsNot Nothing Then
            Dim disp As System.Windows.Threading.Dispatcher = app.Dispatcher
            If disp.CheckAccess() Then
                Throw New InvalidOperationException("Synchronous Init() cannot be called from the UI thread. Use Await InitAsync() instead.")
            Else
                Dim op As System.Windows.Threading.DispatcherOperation(Of Task(Of Boolean)) =
                    disp.InvokeAsync(callback:=Function() Me.InitAsync())
                Dim taskOfTask As System.Threading.Tasks.Task(Of System.Threading.Tasks.Task(Of Boolean)) = op.Task
                Dim unwrapped As System.Threading.Tasks.Task(Of Boolean) = taskOfTask.Unwrap()
                Return unwrapped.GetAwaiter().GetResult()
            End If
        End If

        ' No dispatcher available: run InitAsync synchronously on the thread-pool and return result.
        Return System.Threading.Tasks.Task.Run(Function()
                                                   Return Me.InitAsync().GetAwaiter().GetResult()
                                               End Function).GetAwaiter().GetResult()
    End Function

    ''' <summary>
    '''  Sets the user element dictionary for testing purposes to allow access to UserElementDictionary.
    '''  This method is Friend to allow access from test assemblies.
    ''' </summary>
    Friend Sub SetUserElementDictionaryForTests(value As Dictionary(Of String, Object))
        Me.UserElementDictionary = value
    End Sub

    ''' <summary>
    '''  Async variant of DoRefresh that uses Await.
    ''' </summary>
    ''' <param name="config">Configuration settings as a dictionary.</param>
    ''' <param name="tokenDataElement">
    '''  The JSON tokenDataElement containing token information.
    ''' </param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing the refreshed token as a JSON tokenDataElement.
    ''' </returns>
    Public Async Function DoRefreshAsync(config As Dictionary(Of String, String),
                                         tokenElement As JsonElement) As Task(Of JsonElement)


        Dim tokenUrl As String = config(key:="token_url")

        Dim tokenData As Dictionary(Of String, Object) =
            JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(element:=tokenElement)

        ' Prepare form data
        Dim data As New Dictionary(Of String, String) From {
            {"refresh_token", tokenData(key:="refresh_token").ToString()},
            {"client_id", tokenData(key:="client_id").ToString()},
            {"grant_type", "refresh_token"}}

        Dim clientSecret As Object = Nothing
        If tokenData.TryGetValue(key:="client_secret", value:=clientSecret) Then
            If IsNotNullOrEmpty(value:=clientSecret?.ToString) Then
                data(key:="client_secret") = clientSecret.ToString
            End If
        End If

        Dim magIdentifier As Object = Nothing

        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        ' Prepare optional headers
        If tokenData.TryGetValue(key:="mag-identifier", value:=magIdentifier) Then
            headers(key:="mag-identifier") = magIdentifier?.ToString
        End If

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(name:=header.Key, header.Value)
            Next

            Using content As New FormUrlEncodedContent(nameValueCollection:=data)
                ' POST request
                Using response As HttpResponseMessage =
                   Await client.PostAsync(requestUri:=tokenUrl, content) _
                               .ConfigureAwait(continueOnCapturedContext:=False)
                    _lastHttpStatusCode = response.StatusCode
                    Debug.WriteLine(message:=$"   status: {_lastHttpStatusCode}")

                    response.ThrowIfFailure()

                    Dim responseBody As String =
                        Await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:=True)
                    Dim newData As Dictionary(Of String, String) =
                        JsonSerializer.Deserialize(Of Dictionary(Of String, String))(json:=responseBody)

                    tokenData(key:="access_token") = newData(key:="access_token")
                    tokenData(key:="refresh_token") = newData(key:="refresh_token")
                End Using
            End Using
        End Using

        Dim json As String = JsonSerializer.Serialize(value:=tokenData, options:=s_jsonSerializerOptions)
        Return JsonSerializer.Deserialize(Of JsonElement)(json)
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
    ''' <returns>
    '''  A String containing an error message if the operation failed, or Nothing if successful.
    '''  On success, PatientData and RecentData properties are populated.
    '''  </returns>
    Public Function GetRecentData() As String
        Return Me.GetRecentDataAsync().GetAwaiter().GetResult()
    End Function

    ''' <summary>
    '''  Async variant of GetRecentData that uses Await and centralized response inspection.
    ''' </summary>
    ''' <returns>
    '''  Returns the last error message if the operation fails;
    '''  otherwise, returns the same <see langword="String"/> result as GetRecentData after setting
    '''  the PatientData and RecentData public variables.
    ''' </returns>
    Public Async Function GetRecentDataAsync() As Task(Of String)
        Dim lastErrorMessage As String = Nothing
        Dim refreshTask As Task(Of JsonElement) = Nothing
        Dim hadAuthException As Boolean = False
        If Not IsTokenValid(access_token_payload:=_accessTokenPayload, message:=lastErrorMessage) Then
            Try
                _tokenDataElement = Await Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement) _
                                            .ConfigureAwait(continueOnCapturedContext:=True)
                _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                WriteTokenFile(token:=_tokenDataElement)
            Catch ex As Exception
                Debug.WriteLine(message:=ex.ToString())
            End Try

            If Not IsTokenValid(access_token_payload:=_accessTokenPayload, message:=lastErrorMessage) Then
                Debug.WriteLine(message:=lastErrorMessage)
                Return lastErrorMessage
            End If
        End If

        Dim data As Dictionary(Of String, Object) = Nothing
        Try
            Dim role As String = CStr(Me.UserElementDictionary("role"))
            ' Call GetDataAsync and handle typed exceptions without Await inside Catch.
            Try
                data = Await Me.GetDataAsync(username:=GetUserName(),
                                             role,
                                             patientId:=EmptyString) _
                                .ConfigureAwait(continueOnCapturedContext:=True)

            Catch uaEx As UnauthorizedAccessException
                ' schedule refresh, will await below and then retry once
                hadAuthException = True
                Try
                    refreshTask = Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement)
                Catch innerEx As Exception
                    Debug.WriteLine(message:=innerEx.ToString())
                End Try
            Catch argEx As ArgumentException
                Debug.WriteLine(message:=$"GetRecentData bad request: {argEx.Message}")
                Return argEx.Message
            Catch httpEx As HttpRequestException
                Debug.WriteLine(message:=$"GetRecentData network/server error: {httpEx.Message}")
                Return $"Network/server error: {httpEx.Message}"
            End Try

            ' If we scheduled a refresh due to auth, await it now and retry GetDataAsync once.
            If hadAuthException AndAlso refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement =
                        Await refreshTask.ConfigureAwait(continueOnCapturedContext:=True)
                    If Not refreshedToken.IsNullOrUndefined Then
                        _tokenDataElement = refreshedToken
                        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                        WriteTokenFile(token:=_tokenDataElement)
                        ' retry
                        data = Await Me.GetDataAsync(username:=GetUserName(),
                                                     role:=role,
                                                     patientId:=EmptyString) _
                                       .ConfigureAwait(continueOnCapturedContext:=True)
                    End If
                Catch refreshEx As Exception
                    Debug.WriteLine(message:=refreshEx.ToString())
                    Return "ERROR: failed to refresh token"
                End Try
            End If

            If data Is Nothing OrElse data.Count = DataKeyCount.NoData OrElse
               (data.Count = DataKeyCount.RecentData AndAlso
                CType(data("patientData"), JsonElement).ValueKind = JsonValueKind.Array) Then

                PatientData = Nothing
                RecentData = Nothing
                Dim message As String =
                    $"{NameOf(GetRecentDataAsync)}: No nameValueCollection returned from GetData for user {GetUserName()}"
                Debug.WriteLine(message)
                Return "No nameValueCollection received from server"
            End If
        Catch ex As Exception
            PatientData = Nothing
            RecentData = Nothing
            Debug.WriteLine(message:=ex.DecodeException())
            Return ex.DecodeException()
        End Try

        ' If a call earlier produced an auth status code, attempt refresh proactively.
        If Auth_Error_Codes.Contains(value:=_lastHttpStatusCode) Then
            Try
                _tokenDataElement =
                    Await Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement) _
                            .ConfigureAwait(continueOnCapturedContext:=True)
                _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                WriteTokenFile(token:=_tokenDataElement)
            Catch ex As Exception
                Debug.WriteLine(message:=ex.ToString())
            End Try
        End If

        Select Case data.Keys.Count
            Case DataKeyCount.NoData
                lastErrorMessage = "No Data Found"
            Case DataKeyCount.SingleData
                lastErrorMessage = $"No Data Found for {data.Keys(index:=0)}"
            Case DataKeyCount.RecentData
                lastErrorMessage = Nothing
            Case Else
                lastErrorMessage = $"Unexpected keys in Data: {String.Join(separator:=", ", values:=data.Keys)}"
        End Select

        If data.Values.Count < DataKeyCount.RecentData Then
            Return lastErrorMessage
        End If

        Dim unusedMetaData As JsonElement = CType(data.Values(index:=0), JsonElement)
        Try
            PatientDataElement = CType(data.Values(index:=1), JsonElement)
            DeserializePatientElement()
            WriteTokenFile(token:=PatientDataElement, path:=GetLastDownloadFileWithPath())
        Catch ex As Exception
            Debug.WriteLine(message:=ex.DecodeException())
            Return ex.DecodeException()
        End Try

        Return lastErrorMessage
    End Function

End Class
