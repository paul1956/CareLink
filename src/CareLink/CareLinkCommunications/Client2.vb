' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Text.Json
Imports DocumentFormat.OpenXml.Office2016.Excel

' This class is intentionally not part of the public API.
' It is designed to be used internally within the assembly and is not intended for external consumption.
Friend Class Client2
    Private Const TokenBaseFileName As String = "loginData.json"
    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _tokenBaseFileName As String
    Private _accessTokenPayload As Dictionary(Of String, JsonElement)
    Private _country As String
    Private _lastHttpStatusCode As HttpStatusCode
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
        _Config = Nothing
        _country = Nothing
        Me.ServerRegion = serverRegion
        _httpClient = If(httpClient, New HttpClient)
        _httpClient.SetDefaultRequestHeaders()
    End Sub

    Private Enum DataKeyCount
        NoData
        SingleData
        RecentData
    End Enum

    Friend Shared ReadOnly Property Auth_Error_Codes As Integer() = {401, 403}

    Friend Property Config As ConfigRecord
    Friend Property LoggedIn As Boolean
    Friend Property PatientPersonalData As New PatientPersonalInfo
    Friend Property ServerRegion As Region
    Friend Property UserElementDictionary As Dictionary(Of String, JsonElement)

    ''' <summary>
    '''  Gets the last HTTP status code from the most recent operation.
    ''' </summary>
    ''' <returns>The last HTTP status code.</returns>
    Public ReadOnly Property HttpStatusCode As HttpStatusCode
        Get
            Return _lastHttpStatusCode
        End Get
    End Property

    ''' <summary>
    ''' Build request headers preferring configJsonElement mag-identifier then tokenDataElement.
    ''' </summary>
    Private Shared Function BuildHeaders(configJsonElement As JsonElement,
                                         token_data As JsonElement) As Dictionary(Of String, String)

        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        Dim magId As String = Nothing
        If TryGetStringProperty(element:=configJsonElement, propertyName:="mag-identifier", value:=magId) Then
            If IsNotNullOrWhiteSpace(value:=magId) Then
                headers(key:="mag-identifier") = magId
            End If
        ElseIf TryGetStringProperty(element:=token_data, propertyName:="mag-identifier", value:=magId) Then
            If IsNotNullOrWhiteSpace(value:=magId) Then
                headers(key:="mag-identifier") = magId
            End If
        End If

        Dim access As String = Nothing
        If TryGetStringProperty(element:=token_data, propertyName:="access_token", value:=access) Then
            headers(key:="Authorization") = $"Bearer {access}"
        End If

        Return headers
    End Function

    Public Async Function DownloadFileAsync(requestUri As String,
                                            path As String,
                                            localTime As Date) As Task
        ' Create a single static instance of HttpClient for performance (recommended)
        Try
            ' Send a GET request to fetch the file data
            Const completionOption As HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead
            Dim tokenData As Dictionary(Of String, String) =
                _tokenDataElement.ToStringDictionary()

            ' Set the Authorization header with the Bearer token
            _httpClient.DefaultRequestHeaders.Authorization =
                New AuthenticationHeaderValue(scheme:="Bearer",
                                              parameter:=tokenData(key:="access_token"))

            Using response As HttpResponseMessage =
                Await _httpClient.GetAsync(requestUri, completionOption)
                response.EnsureSuccessStatusCode() ' Throw if not successful

                ' Read the file bytes as a stream
                Using fileStream As Stream = Await response.Content.ReadAsStreamAsync(),
                      destination As Stream = File.Create(path)

                    ' Copy the content to the local file stream
                    Await fileStream.CopyToAsync(destination)
                End Using
            End Using

            File.SetCreationTime(path, creationTime:=localTime)
            File.SetLastAccessTime(path, lastAccessTime:=localTime)
        Catch ex As Exception
            LoggerManager.LogMessage(message:=$"Error downloading file: {ex.Message}")
        End Try
    End Function

    Private Shared Function GetAccessTokenPayload(token_data As JsonElement) As Dictionary(Of String, JsonElement)
        Try
            If token_data.IsEmpty Then
                Return Nothing
            End If
            Dim token As String = token_data.JsonElementToDictionary(key:="access_token").ToString
            Dim payload_b64 As String = token.Split(separator:="."c)(1)
            Dim payload_b64_bytes As Byte() = Encoding.UTF8.GetBytes(s:=payload_b64)
            Dim count As Integer = (4 - (payload_b64_bytes.Length Mod 4)) Mod 4
            If count > 0 Then
                payload_b64 &= New String(c:="="c, count)
            End If
            Dim bytes As Byte() = Convert.FromBase64String(s:=payload_b64)
            Dim json As String = Encoding.UTF8.GetString(bytes)
            Dim dict As Dictionary(Of String, JsonElement) = Nothing
            Return If(Not json.TryFromJson(result:=dict),
                                           Nothing,
                                           dict)
        Catch ex As Exception
            Dim str As String = ex.DecodeException()
            Dim location As String = NameOf(GetAccessTokenPayload)
            Dim message As String = $"No access token found or malformed access token: {str} in {location}"
            LoggerManager.LogMessage(message)
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
    Private Shared Function IsTokenValid(access_token_payload As Dictionary(Of String, JsonElement),
                                         ByRef message As String) As Boolean

        If access_token_payload Is Nothing Then
            message = "AccessToken Empty"
            Return False
        End If
        Try

            Dim unixTime As Long = access_token_payload(key:="exp").GetInt64()
            Dim unixCurrentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = unixTime - unixCurrentTime
            If tDiff < 0 Then
                Dim absDiff As Long = Math.Abs(value:=tDiff)
                message = $"In {NameOf(IsTokenValid)} access token has expired {absDiff}s ago"
                LoggerManager.LogMessage(message)
                Return False
            End If
            Dim startKey As String
            If tDiff < 600 Then
                startKey = $"In {NameOf(IsTokenValid)} access token is about to expire in "
                message = $"In {NameOf(IsTokenValid)} access token is about to expire in {tDiff}s"
                LoggerManager.UpdateMessage(message, startKey)
                Return False
            End If

            Const format As String = "ddd MMM dd HH:mm:ss UTC yyyy"
            Dim utcTime As DateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(seconds:=unixTime)
            Dim authTokenValidTo As String = utcTime.ToString(format)

            ' Convert to local time
            Dim localTime As DateTimeOffset = utcTime.ToLocalTime()
            ' Format as needed
            Dim formatted As String = localTime.ToString(format:="M/d/yyyy h:mm tt")

            startKey = $"In {NameOf(IsTokenValid)} access token expires in"
            message = $"{startKey} {(tDiff \ 60).ToHoursMinutes()} at {authTokenValidTo} or {formatted}"
            LoggerManager.UpdateMessage(message, startKey)
            Return True
        Catch ex As Exception
            message =
                $"In {NameOf(IsTokenValid)} missing nameValueCollection in access token. {ex.DecodeException()}"
            LoggerManager.LogMessage(message)
            Return False
        End Try
    End Function

    ''' <summary>
    '''  Async version of GetData that uses Await and centralized resp inspection.
    '''  This variant implements a small retry loop for transient failures.
    ''' </summary>
    ''' <param name="username">The username for the nameValueCollection request.</param>
    ''' <param name="role">The role of the user (e.g., patient, carePartner).</param>
    ''' <param name="patientId">The patient ID, if applicable.</param>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing the requested
    '''  nameValueCollection as a dictionary.
    ''' </returns>
    Private Async Function GetDataAsync(username As String,
                                        role As String,
                                        patientId As String) As Task(Of Dictionary(Of String, JsonElement))

        _httpClient.SetDefaultRequestHeaders()
        Dim requestUri As String = $"{Me.Config.BaseUrlCumulus}/display/message"
        Dim tokenData As Dictionary(Of String, String) = _tokenDataElement.ToStringDictionary()
        Dim value As New Dictionary(Of String, String) From {{"username", username}}
        If role.ContainsNoCase(value:="Partner") Then
            value(key:="role") = "carePartner".ToLower()
            value(key:="patientId") = patientId
        Else
            value(key:="role") = "patient"
        End If
        value(key:="appVersion") = "3.6.0"

        Dim headers As New Dictionary(Of String, String)
        headers(key:="Authorization") = $"Bearer {tokenData(key:="access_token")}"

        Dim magidentifier As String = Nothing
        If tokenData.TryGetValue(key:="mag-identifier", value:=magidentifier) AndAlso
           IsNotNullOrWhiteSpace(value:=magidentifier) Then

            headers(key:="mag-identifier") = magidentifier
        End If

        Dim contentJson As String = String.Empty
        If Not value.TryToJson(contentJson) Then
            LoggerManager.LogMessage(message:=$"ERROR: failed serializing request body for GetDataAsync")
            contentJson = "{}"
        End If
        Using content As New StringContent(content:=contentJson,
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

                        Using response As HttpResponseMessage = Await _httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext:=False)
                            _lastHttpStatusCode = response.StatusCode
                            LoggerManager.UpdateMessage(message:=$"   status: {_lastHttpStatusCode}",
                                                        startKey:=$"   status: ")

                            ' Centralized resp inspection; may throw UnauthorizedAccessException,
                            ' ArgumentException (bad request) or HttpRequestException (transient/server).
                            Await response.ThrowIfFailureAsync().ConfigureAwait(continueOnCapturedContext:=False)

                            Dim json As String = Await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:=False)
                            Dim d As Dictionary(Of String, JsonElement) = Nothing
                            Return If(Not json.TryFromJson(result:=d),
                                                           Nothing,
                                                           d)
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
                    Await Task.Delay(millisecondsDelay:=retryDelayMs)
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
        If TryGetStringProperty(element:=configJsonElement, propertyName:="mag-identifier", value:=magId) Then
            headers(key:="mag-identifier") = magId
        End If

        headers = BuildHeaders(configJsonElement, token_data)

        _lastHttpStatusCode = HttpStatusCode.OK
        Const key As String = "baseUrlCareLink"
        Dim configDict As Dictionary(Of String, JsonElement) =
            configJsonElement.JsonElementToDictionary()

        Dim baseUrl As String = String.Empty
        Dim baseElem As JsonElement = Nothing
        If configDict.TryGetValue(key, value:=baseElem) Then
            baseUrl = baseElem.ElementToString()
        End If

        Dim requestUri As String = $"{baseUrl}/links/patients"
        Using request As New HttpRequestMessage(method:=HttpMethod.Get, requestUri:=requestUri)
            For Each header As KeyValuePair(Of String, String) In headers
                request.Headers.TryAddWithoutValidation(name:=header.Key, value:=header.Value)
            Next

            Using response As HttpResponseMessage = Await _httpClient.SendAsync(request)
                _lastHttpStatusCode = response.StatusCode
                If _lastHttpStatusCode <> HttpStatusCode.OK Then
                    LoggerManager.LogMessage(message:=$"   status: {_lastHttpStatusCode}")
                End If

                ' Ensure non-success status codes are not silently ignored.
                Try
                    Await response.ThrowIfFailureAsync().ConfigureAwait(continueOnCapturedContext:=False)
                Catch ex As Exception
                    response.Dispose()
                    LoggerManager.LogMessage(message:=$"GetPatient HTTP failure: {ex.Message}")
                    Return Nothing
                End Try

                Dim patients As List(Of Dictionary(Of String, String))
                Dim json As String =
                    Await response.Content.ReadAsStringAsync()
                Dim p As List(Of Dictionary(Of String, String)) = Nothing
                patients = If(Not json.TryFromJson(result:=p),
                              New List(Of Dictionary(Of String, String))(),
                              p)
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
    ''' <param name="configElement">
    '''  The configuration JSON tokenDataElement containing base URL information.
    ''' </param>
    ''' <param name="tokenData">
    '''  The token nameValueCollection JSON tokenDataElement containing authentication tokens.
    ''' </param>
    ''' <returns>A JSON string representing the user information.</returns>
    Private Async Function GetUserStringAsync(config As ConfigRecord, tokenData As JsonElement) As Task(Of String)
        Dim requestUri As String = $"{config.BaseUrlCareLink}/users/me"
        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)

        Dim magId As String = Nothing
        If TryGetStringProperty(element:=tokenData, propertyName:="mag-identifier", value:=magId) Then
            headers(key:="mag-identifier") = magId
        End If
        headers(key:="Authorization") = $"Bearer {tokenData.GetProperty(propertyName:="access_token").GetString()}"
        headers(key:="Accept-Language") = "en-US"

        Using request As New HttpRequestMessage(method:=HttpMethod.Get, requestUri:=requestUri)
            request.Headers.Accept.Add(item:=New MediaTypeWithQualityHeaderValue(mediaType:="application/json"))
            For Each header As KeyValuePair(Of String, String) In headers.Sort
                request.Headers.Add(name:=header.Key, header.Value)
            Next

            Using response As HttpResponseMessage =
                Await _httpClient.SendAsync(request).
                                  ConfigureAwait(continueOnCapturedContext:=False)
                _lastHttpStatusCode = response.StatusCode
                LoggerManager.LogMessage(message:=$"   status: {_lastHttpStatusCode}")

                ' Use centralized failure handling and translate to Nothing for older call-sites.
                Try
                    Await response.ThrowIfFailureAsync().ConfigureAwait(continueOnCapturedContext:=False)
                Catch ex As UnauthorizedAccessException
                    LoggerManager.LogMessage(message:=$"GetUserString unauthorized: {ex.Message}")
                    Return Nothing
                Catch ex As ArgumentException
                    LoggerManager.LogMessage(message:=$"GetUserString bad request: {ex.Message}")
                    Return Nothing
                Catch ex As HttpRequestException
                    LoggerManager.LogMessage(message:=$"GetUserString HTTP error: {ex.Message}")
                    Return Nothing
                End Try

                Return Await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:=False)
            End Using
        End Using
    End Function

    ''' <summary>
    '''  Initializes the client by reading token nameValueCollection and user information.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing a boolean indicating success or failure.
    ''' </returns>
    Private Async Function internalInit() As Task(Of Boolean)
        _tokenDataElement = ReadTokenFile(tokenBaseFileName:=_tokenBaseFileName)
        If _tokenDataElement.IsEmpty Then
            Me.LoggedIn = False
            Return False
        End If

        _accessTokenPayload =
            GetAccessTokenPayload(token_data:=_tokenDataElement)
        If _accessTokenPayload Is Nothing Then
            Return False
        End If

        Dim refreshTask As Task(Of JsonElement) = Nothing
        Dim hadException As Boolean = False
        Dim configJsonElement As JsonElement

        Try
            Application.DoEvents()
            Dim element As JsonElement = _accessTokenPayload(key:="token_details")
            Dim payload As AccessTokenDetails = Nothing
            If Not element.TryFromJson(result:=payload) Then
                payload = Nothing
            End If
            _country = If(payload.Country, s_countryCode)

            configJsonElement =
                Await GetConfigAsync(httpClient:=_httpClient, country:=_country, Me.ServerRegion)

            Dim cfg As ConfigRecord = Nothing
            If Not configJsonElement.TryFromJson(result:=cfg) Then
                Throw New ApplicationException(message:="Failed to parse configuration JSON.")
            End If
            Me.Config = cfg

            ' Call user string; handle typed failures
            Dim json As String =
                Await Me.GetUserStringAsync(Me.Config, tokenData:=_tokenDataElement)
            If IsNullOrWhiteSpace(value:=json) Then
                Throw New UnauthorizedAccessException
            End If

            Dim tmpDict As Dictionary(Of String, JsonElement) = Nothing
            If Not json.TryFromJson(result:=tmpDict) Then
                tmpDict = New Dictionary(Of String, JsonElement)()
            End If
            Me.UserElementDictionary = tmpDict

            Dim ppd As PatientPersonalInfo = Nothing
            If Not json.TryFromJson(result:=ppd) Then
                ppd = New PatientPersonalInfo()
            End If
            _PatientPersonalData = ppd

            Dim role As String = _PatientPersonalData.Role
            If role.ContainsNoCase(value:="Partner") Then
                Await Me.GetPatient(configJsonElement, token_data:=_tokenDataElement)
            End If
        Catch ex As Exception
            hadException = True

            If Auth_Error_Codes.Contains(value:=_lastHttpStatusCode) Then
                ' Start refresh task without Await inside Catch
                Try
                    If Not configJsonElement.ValueKind = JsonValueKind.Undefined Then
                        refreshTask = Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement)
                    End If
                Catch innerEx As Exception
                    LoggerManager.LogMessage(message:=innerEx.ToString())
                End Try
            End If
        End Try

        ' If an exception occurred in the Try block, handle refresh attempt now (outside Catch).
        If hadException Then
            If refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement =
                        Await refreshTask.ConfigureAwait(continueOnCapturedContext:=False)
                    If Not refreshedToken.IsEmpty Then
                        _tokenDataElement = refreshedToken
                        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                        WriteTokenFile(token:=_tokenDataElement)
                    End If
                Catch refreshEx As Exception
                    LoggerManager.LogMessage(message:=refreshEx.ToString())
                End Try
            End If

            Me.LoggedIn = False
            Return False
        End If

        Me.LoggedIn = True
        Return True
    End Function

    ''' <summary>
    '''  Asynchronous initialization function that prepares the client for use.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation, containing True if initialization succeeded;
    '''  otherwise, False.
    ''' </returns>
    Friend Async Function InitAsync() As Task(Of Boolean)
        If Not Await Me.internalInit() Then
            ' Force user login
            Await GetLoginData(Me.ServerRegion,
                               userName:=s_userName,
                               password:=s_password)
            If Not Await Me.internalInit() Then
                Return False
            End If
        End If
        Return True
    End Function

    ''' <summary>
    '''  Sets the user element dictionary for testing purposes to allow access to UserElementDictionary.
    '''  This method is Friend to allow access from test assemblies.
    ''' </summary>
    Friend Sub SetUserElementDictionaryForTests(value As Dictionary(Of String, JsonElement))
        Me.UserElementDictionary = value
    End Sub

    ''' <summary>
    '''  Asynchronously retrieves login data for the specified server region,
    '''  username, and password.
    ''' </summary>
    ''' <param name="serverRegion">The server <see cref="Region"/> to use.</param>
    ''' <param name="userName">The username for login.</param>
    ''' <param name="password">The password for login.</param>
    ''' <param name="tokenData">The current token data.</param>
    ''' <returns>A task representing the asynchronous operation.</returns>
    Public Shared Async Function GetLoginData(serverRegion As Region,
                                              userName As String,
                                              password As String,
                                              Optional tokenData As TokenData = Nothing) As Task
        If tokenData Is Nothing Then
            Try
                Dim discoveryUrl As String = If(serverRegion <> Region.Europe,
                                                CareLinkService.DiscoveryUrlNa,
                                                CareLinkService.DiscoveryUrlEu)
                Dim outputFile As String = GetLoginDataFileName()

                Dim endpointConfig As EndpointConfig =
                    Await CareLinkService.ResolveEndpointConfigAsync(discoveryUrl, serverRegion)

                Dim result As TokenData =
                    Await CareLinkService.DoLoginAsync(endpointConfig,
                                                       outputFile,
                                                       userName,
                                                       password)
            Catch ex As Exception
                If ex.Message <> "Login was cancelled." Then
                    MessageBox.Show(text:=ex.Message,
                                    caption:="Error",
                                    buttons:=MessageBoxButtons.OK,
                                    icon:=MessageBoxIcon.Error)
                End If
            End Try
        End If
    End Function

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
    Public Async Function DoRefreshAsync(config As ConfigRecord,
                                         tokenElement As JsonElement) As Task(Of JsonElement)
        Dim result As Dictionary(Of String, JsonElement) = Nothing
        Dim message As String
        If Not tokenElement.TryFromJson(result) Then
            message = $"{NameOf(DoRefreshAsync)}: token element could not be parsed"
            LoggerManager.LogMessage(message)
            Return Nothing
        End If
        Dim tokenData As Dictionary(Of String, JsonElement) = result

        ' Validate required keys
        Dim refreshTok As String = Nothing
        Dim clientId As String = Nothing
        If tokenData.TryGetValue(key:="refresh_token", value:=New JsonElement) Then
            Try
                refreshTok = tokenData(key:="refresh_token").GetString()
            Catch
                refreshTok = Nothing
            End Try
        End If
        If tokenData.TryGetValue(key:="client_id", value:=New JsonElement) Then
            Try
                clientId = tokenData(key:="client_id").GetString()
            Catch
                clientId = Nothing
            End Try
        End If
        If IsNullOrWhiteSpace(value:=refreshTok) Then
            message =
                $"{NameOf(DoRefreshAsync)}: Missing refresh_token in stored token data."
            LoggerManager.LogMessage(message)
            Return Nothing
        End If
        If IsNullOrWhiteSpace(value:=clientId) Then
            message =
                $"{NameOf(DoRefreshAsync)}: Missing client_id in stored token data."
            LoggerManager.LogMessage(message)
            Return Nothing
        End If

        ' Build form data (grant_type always present)
        Dim formData As New List(Of KeyValuePair(Of String, String)) From {
            New KeyValuePair(Of String, String)(key:="refresh_token", value:=refreshTok),
            New KeyValuePair(Of String, String)(key:="client_id", value:=clientId),
            New KeyValuePair(Of String, String)(key:="grant_type", value:="refresh_token")}

        ' If client_secret exists, prefer sending it as HTTP Basic auth (common requirement),
        ' otherwise include in form body if provider expects that.
        Dim clientSecret As String = Nothing
        Dim hasClientSecret As Boolean = False
        Dim ce As JsonElement = Nothing
        If tokenData.TryGetValue(key:="client_secret", value:=ce) Then
            Try
                clientSecret = ce.GetString()
                hasClientSecret = Not IsNullOrWhiteSpace(clientSecret)
            Catch
                hasClientSecret = False
            End Try
        End If

        Using client As New HttpClient()
            ' Add mag-identifier header if present
            Dim magElem As JsonElement = Nothing
            If tokenData.TryGetValue(key:="mag-identifier", value:=magElem) Then
                Try
                    Dim mag As String = magElem.GetString()
                    If Not IsNullOrWhiteSpace(mag) Then
                        client.DefaultRequestHeaders.Add(name:="mag-identifier", value:=mag)
                    End If
                Catch
                End Try
            End If

            Dim succeeded As Boolean = False
            Dim lastResponseBody As String = String.Empty

            ' Strategy: if we have a client_secret, try Basic auth first (preferred).
            ' If that fails and provider may expect client_secret in body,
            ' retry with client_secret in form.
            Dim attempts As New List(Of Tuple(Of Boolean, Boolean))
            If hasClientSecret Then
                attempts.Add(item:=Tuple.Create(True, False))   ' Basic auth, no client_secret in form
                attempts.Add(item:=Tuple.Create(False, True))   ' No basic auth, include client_secret in form
            Else
                attempts.Add(item:=Tuple.Create(False, False))  ' No client_secret available
            End If

            For Each attempt As Tuple(Of Boolean, Boolean) In attempts
                Dim resp As HttpResponseMessage = Nothing
                Try
                    ' Configure auth header for this attempt
                    If attempt.Item1 AndAlso hasClientSecret Then
                        Dim cred As String = Convert.ToBase64String(inArray:=System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))
                        client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", cred)
                    Else
                        client.DefaultRequestHeaders.Authorization = Nothing
                    End If

                    ' Build actual form for this attempt
                    Dim actualForm As New List(Of KeyValuePair(Of String, String))(collection:=formData)
                    If attempt.Item2 AndAlso hasClientSecret Then
                        actualForm.Add(item:=New KeyValuePair(Of String, String)(key:="client_secret", value:=clientSecret))
                    End If

                    Using content As New FormUrlEncodedContent(nameValueCollection:=actualForm)
                        resp = Await client.PostAsync(requestUri:=config.TokenUrl, content).ConfigureAwait(continueOnCapturedContext:=False)
                    End Using
                Catch ex As Exception
                    message = $"{NameOf(DoRefreshAsync)}: HTTP request failed: {ex.Message}"
                    LoggerManager.LogMessage(message)
                    Continue For
                End Try

                _lastHttpStatusCode = resp.StatusCode
                Dim respBody As String =
                    Await resp.Content.ReadAsStringAsync().
                                       ConfigureAwait(continueOnCapturedContext:=False)
                lastResponseBody = respBody

                If resp.StatusCode = HttpStatusCode.OK Then
                    Try
                        Using newData As JsonDocument = JsonDocument.Parse(json:=respBody)
                            Dim root As JsonElement = newData.RootElement
                            tokenData(key:="access_token") =
                                root.GetProperty(propertyName:="access_token").Clone()
                            tokenData(key:="refresh_token") =
                                root.GetProperty(propertyName:="refresh_token").Clone()
                        End Using
                        succeeded = True
                        Exit For
                    Catch ex As Exception
                        message =
                            $"{NameOf(DoRefreshAsync)}: failed parsing token refresh response: {ex.Message}"
                        LoggerManager.LogMessage(message)
                        Return Nothing
                    End Try
                Else
                    message =
                        $"{NameOf(DoRefreshAsync)}: token refresh attempt failed. useBasic={attempt.Item1} " &
                        $"includeSecret={attempt.Item2} Status={CInt(resp.StatusCode)} Body={respBody}"
                    LoggerManager.LogMessage(message)
                End If
            Next

            If Not succeeded Then
                message =
                    $"{NameOf(DoRefreshAsync)}: all refresh attempts failed. Last response: {lastResponseBody}"
                LoggerManager.LogMessage(message)

                ' Attempt interactive login using WebView2 (fallback)
                Try
                    Dim discoveryUrl As String =
                        If(Me.ServerRegion = Region.NorthAmerica,
                           s_discoverUrl(key:="US"),
                           s_discoverUrl(key:="EU"))
                    Dim endpointConfig As EndpointConfig =
                        Await CareLinkService.ResolveEndpointConfigAsync(discoveryUrl, Me.ServerRegion)

                    ' Do interactive login (user will be prompted). DoLoginAsync writes token file.
                    Dim tokenResult As TokenData =
                        Await CareLinkService.DoLoginAsync(endpointConfig,
                                                           outputFile:=_tokenBaseFileName,
                                                           userName:=String.Empty,
                                                           password:=String.Empty)
                    If tokenResult Is Nothing Then
                        message =
                            $"{NameOf(DoRefreshAsync)}: interactive login returned no token."
                        LoggerManager.LogMessage(message)
                        Return Nothing
                    End If

                    ' Convert TokenData to JsonElement
                    Dim tdJson2 As String = String.Empty
                    If Not tokenResult.TryToJson(json:=tdJson2) Then
                        message =
                            $"{NameOf(DoRefreshAsync)}: failed serializing TokenData after interactive login."
                        LoggerManager.LogMessage(message)
                        Return Nothing
                    End If
                    Dim tdElem2 As JsonElement
                    If Not tdJson2.TryFromJson(result:=tdElem2) Then
                        message =
                            $"{NameOf(DoRefreshAsync)}: failed parsing TokenData JSON after interactive login."
                        LoggerManager.LogMessage(message)
                        Return Nothing
                    End If
                    Return tdElem2
                Catch ex As Exception
                    message =
                        $"{NameOf(DoRefreshAsync)}: interactive login failed: {ex.Message}"
                    LoggerManager.LogMessage(message)
                    Return Nothing
                End Try
            End If
        End Using

        Dim tdJson As String = String.Empty
        If Not tokenData.TryToJson(json:=tdJson) Then
            message =
                $"{NameOf(DoRefreshAsync)}: failed serializing token data to JSON."
            LoggerManager.LogMessage(message)
            Return Nothing
        End If
        Dim tdElem As JsonElement
        Return If(Not tdJson.TryFromJson(result:=tdElem),
                  Nothing,
                  tdElem)
    End Function

    ''' <summary>
    '''  Async variant of GetRecentData that uses Await and centralized resp inspection.
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
                _tokenDataElement = Await Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement)
                _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                WriteTokenFile(token:=_tokenDataElement)
            Catch ex As Exception
                LoggerManager.LogMessage(message:=ex.ToString())
            End Try

            If Not IsTokenValid(access_token_payload:=_accessTokenPayload, message:=lastErrorMessage) Then
                LoggerManager.LogMessage(message:=lastErrorMessage)
                Return lastErrorMessage
            End If
        End If

        Dim data As Dictionary(Of String, JsonElement) = Nothing
        Try
            Dim role As String = _PatientPersonalData.Role.ToJson
            ' Call GetDataAsync and handle typed exceptions without Await inside Catch.
            Try
                data = Await Me.GetDataAsync(username:=GetUserName(),
                                             role,
                                             patientId:=EmptyString)
            Catch uaEx As UnauthorizedAccessException
                ' schedule refresh, will await below and then retry once
                hadAuthException = True
                Try
                    refreshTask = Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement)
                Catch innerEx As Exception
                    LoggerManager.LogMessage(message:=innerEx.ToString())
                End Try
            Catch argEx As ArgumentException
                LoggerManager.LogMessage(message:=$"GetRecentData bad request: {argEx.Message}")
                Return argEx.Message
            Catch httpEx As HttpRequestException
                LoggerManager.LogMessage(message:=$"GetRecentData network/server error: {httpEx.Message}")
                Return $"Network/server error: {httpEx.Message}"
            End Try

            ' If we scheduled a refresh due to auth, await it now and retry GetDataAsync once.
            If hadAuthException AndAlso refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement = Await refreshTask
                    If Not refreshedToken.IsEmpty Then
                        _tokenDataElement = refreshedToken
                        _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                        WriteTokenFile(token:=_tokenDataElement)
                        ' retry
                        data = Await Me.GetDataAsync(username:=GetUserName(),
                                                     role:=role,
                                                     patientId:=EmptyString)
                    End If
                Catch refreshEx As Exception
                    LoggerManager.LogMessage(message:=refreshEx.ToString())
                    Return "ERROR: failed to refresh token"
                End Try
            End If

            If data Is Nothing OrElse data.Count = DataKeyCount.NoData OrElse
               (data.Count = DataKeyCount.RecentData AndAlso
                CType(data("patientData"), JsonElement).ValueKind = JsonValueKind.Array) Then

                PatientData = Nothing
                Dim message As String =
                    $"{NameOf(GetRecentDataAsync)}: No nameValueCollection returned from GetData for user {GetUserName()}"
                LoggerManager.LogMessage(message)
                Return "No nameValueCollection received from server"
            End If
        Catch ex As Exception
            PatientData = Nothing
            LoggerManager.LogMessage(message:=ex.DecodeException())
            Return ex.DecodeException()
        End Try

        ' If a call earlier produced an auth status code, attempt refresh proactively.
        If Auth_Error_Codes.Contains(value:=_lastHttpStatusCode) Then
            Try
                _tokenDataElement = Await Me.DoRefreshAsync(Me.Config, tokenElement:=_tokenDataElement)
                _accessTokenPayload = GetAccessTokenPayload(token_data:=_tokenDataElement)
                WriteTokenFile(token:=_tokenDataElement)
            Catch ex As Exception
                LoggerManager.LogMessage(message:=ex.ToString())
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

        Try
            Dim metaDataElement As JsonElement =
                CType(data.Values(index:=0), JsonElement)
            Dim metaData As Metadata = Nothing
            If Not metaDataElement.TryFromJson(metaData) Then
                Stop
                Const message As String = "Failed to parse metadata element."
                Throw New ApplicationException(message)
            End If
            Dim requestUri As String = metaData.IconResourceBundle.IconBundleUrl
            Dim zipFileName As String = requestUri.Split(separator:="/").Last
            Dim destinationPath As String =
                Path.Combine(GetMyDocuments(), "CareLink", zipFileName)

            ' Download the file
            Dim utcString As String =
                metaData.IconResourceBundle.IconBundleTimestamp
            Const styles As DateTimeStyles =
                DateTimeStyles.AdjustToUniversal Or DateTimeStyles.AssumeUniversal
            Dim utcTime As Date = Date.Parse(s:=utcString,
                                             provider:=Nothing,
                                             styles)
            ' Convert to local time
            Dim localTime As Date = utcTime.ToLocalTime()

            If Not File.Exists(path:=destinationPath) OrElse
                File.GetCreationTime(path:=destinationPath) < localTime Then
                Await Me.DownloadFileAsync(requestUri,
                                           path:=destinationPath,
                                           localTime)
            End If
        Catch ex As Exception
            Stop
        End Try

        Try
            PatientDataElement = data.Values(index:=1)
            DeserializePatientElement()
            WriteTokenFile(token:=PatientDataElement, path:=GetLastDownloadFileWithPath())
        Catch ex As Exception
            LoggerManager.LogMessage(message:=ex.DecodeException())
            Return ex.DecodeException()
        End Try

        Return lastErrorMessage
    End Function

End Class
