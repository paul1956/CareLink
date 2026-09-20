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

' This class is intentionally not part of the public API.
' It is designed to be used internally within the assembly and is not intended
' for external consumption.
Friend Class Client2
    Private ReadOnly _httpClient As HttpClient
    Private _country As String
    Private _lastHttpStatus As HttpStatusCode

    ''' <summary>
    '''  Initializes a new instance of the <see cref="Client2"/> class.
    ''' </summary>
    ''' <param name="serverRegion">Indicates whether the region is US.</param>
    ''' <param name="httpClient">The HTTP client to use for requests.</param>
    ''' <param name="tokenFile">The file path for the tokenDataElement nameValueCollection.</param>
    ''' <remarks>
    '''  This Class is intentionally not part of the public API.
    ''' </remarks>
    Friend Sub New(serverRegion As ServerLocation,
                   Optional httpClient As HttpClient = Nothing,
                   Optional tokenFile As String = "loginData.json")

        Me.TokenBaseFileName = tokenFile
        Me.TokenDataElement = Nothing
        Me.AccessTokenPayload = Nothing
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

    Private Property TokenBaseFileName As String

    Friend Shared ReadOnly Property Auth_Error_Codes As Integer() =
        {401, 403}

    Friend Property Config As ConfigRecord
    Friend Property LoggedIn As Boolean
    Friend Property PatientPersonalData As New PatientPersonalInfo
    Friend Property ServerRegion As ServerLocation
    Friend Property TokenDataElement As JsonElement
    Friend Property UserElementDictionary As Dictionary(Of String, JsonElement)
    Public Property AccessTokenPayload As Dictionary(Of String, JsonElement)

    ''' <summary>
    '''  Gets the last HTTP status code from the most recent operation.
    ''' </summary>
    ''' <returns>The last HTTP status code.</returns>
    Public ReadOnly Property LastHttpStatusCode As HttpStatusCode
        Get
            Return _lastHttpStatus
        End Get
    End Property

    ''' <summary>
    ''' Build request headers using tokenDataElement Authorization only.
    ''' </summary>
    Private Shared Function BuildHeaders(token_data As JsonElement) As Dictionary(Of String, String)

        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)
        Dim access As String = Nothing
        If TryGetStringProperty(element:=token_data,
                                propertyName:="access_token",
                                value:=access) Then
            headers(key:="Authorization") = $"Bearer {access}"
        End If

        Return headers
    End Function

    ''' <summary>
    '''  Extracts the payload from the access tokenDataElement in
    '''  the provided tokenDataElement JSON tokenDataElement.
    ''' </summary>
    ''' <param name="tokenDataElement">
    '''  The JSON tokenDataElement containing the access tokenDataElement.
    ''' </param>
    ''' <returns>
    '''  A dictionary representing the payload of the access tokenDataElement,
    '''  or Nothing if extraction fails.
    ''' </returns>
    Private Shared Function GetAccessTokenPayload(tokenDataElement As JsonElement) As Dictionary(Of String, JsonElement)
        Try
            If tokenDataElement.IsEmpty Then
                Return Nothing
            End If
            Dim token As String =
                tokenDataElement.JsonElementToDictionary(key:="access_token").ToString
            Dim payload_b64 As String = token.Split(separator:="."c)(1)
            Dim payload_b64_bytes As Byte() = Encoding.UTF8.GetBytes(s:=payload_b64)
            Dim count As Integer = (4 - (payload_b64_bytes.Length Mod 4)) Mod 4
            If count > 0 Then
                payload_b64 &= New String(c:="="c, count)
            End If
            Dim bytes As Byte() = Convert.FromBase64String(s:=payload_b64)
            Dim json As String = Encoding.UTF8.GetString(bytes)
            Dim dict As Dictionary(Of String, JsonElement) = Nothing
            Try
                dict = json.FromJson(Of Dictionary(Of String, JsonElement))()
            Catch ex As Exception
                dict = Nothing
            End Try
            Return dict
        Catch ex As Exception
            Dim exception As String = ex.DecodeException()
            Dim location As String = NameOf(GetAccessTokenPayload)
            Dim message As String =
                $"No or malformed access {NameOf(tokenDataElement)} found: {exception} in {location}"
            LogMessage(message)
            Stop
            Return Nothing
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
        Dim tokenData As Dictionary(Of String, String) = Me.TokenDataElement.ToStringDictionary()
        Dim value As New Dictionary(Of String, String) From {{"username", username}}
        If role.ContainsNoCase(value:="Partner") Then
            value(key:="role") = "carePartner".ToLower()
            value(key:="patientId") = patientId
        Else
            value(key:="role") = "patient"
        End If
        value(key:="appVersion") = "3.6.0"

        Dim headers As New Dictionary(Of String, String)
        headers(key:="Authorization") =
            $"Bearer {tokenData(key:="access_token")}"

        Dim contentJson As String = String.Empty
        If Not value.TryToJson(json:=contentJson) Then
            LogMessage(message:=$"ERROR: failed serializing request body for GetDataAsync")
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
                                                            requestUri:=requestUri) With {
                                                            .Content = content}
                        For Each header As KeyValuePair(Of String, String) In headers
                            request.Headers.TryAddWithoutValidation(name:=header.Key,
                                                                    header.Value)
                        Next

                        Using response As HttpResponseMessage =
                            Await _httpClient.SendAsync(request).
                                              ConfigureAwaitFalse()

                            _lastHttpStatus = response.StatusCode
                            UpdateMessage(message:=$"   status: {_lastHttpStatus}",
                                          startKey:=$"   status: ")

                            ' Centralized resp inspection; may throw UnauthorizedAccessException,
                            ' ArgumentException (bad request) or HttpRequestException
                            ' (transient/server).
                            Await response.ThrowIfFailureAsync().
                                           ConfigureAwaitFalse()

                            Dim json As String =
                                Await response.Content.ReadAsStringAsync().
                                                       ConfigureAwaitFalse()
                            Dim d As Dictionary(Of String, JsonElement) = Nothing
                            Try
                                d = json.FromJson(Of Dictionary(Of String, JsonElement))()
                            Catch ex As Exception
                                d = Nothing
                            End Try
                            Return d
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
    ''' <param name="configJsonElement">
    '''  The configuration JSON tokenDataElement containing base URL information.
    ''' </param>
    ''' <param name="token_data">
    '''  The tokenDataElement nameValueCollection JSON tokenDataElement
    '''  containing authentication tokens.
    ''' </param>
    ''' <returns>
    '''  A task representing the asynchronous operation,
    '''  containing a dictionary of patient information.
    ''' </returns>
    Private Async Function GetPatient(
        configJsonElement As JsonElement,
        token_data As JsonElement) As Task(Of Dictionary(Of String, String))

        _lastHttpStatus = HttpStatusCode.OK
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

            For Each header As KeyValuePair(Of String, String) In BuildHeaders(token_data)
                request.Headers.TryAddWithoutValidation(name:=header.Key, value:=header.Value)
            Next

            Using response As HttpResponseMessage = Await _httpClient.SendAsync(request)
                _lastHttpStatus = response.StatusCode
                If _lastHttpStatus <> HttpStatusCode.OK Then
                    LogMessage(message:=$"   status: {_lastHttpStatus}")
                End If

                ' Ensure non-success status codes are not silently ignored.
                Try
                    Await response.ThrowIfFailureAsync().
                                   ConfigureAwaitFalse()
                Catch ex As Exception
                    response.Dispose()
                    LogMessage(message:=$"GetPatient HTTP failure: {ex.Message}")
                    Return Nothing
                End Try

                Dim patients As List(Of Dictionary(Of String, String))
                Dim json As String =
                    Await response.Content.ReadAsStringAsync()
                Dim p As List(Of Dictionary(Of String, String)) = Nothing
                Try
                    p = json.FromJson(Of List(Of Dictionary(Of String, String)))()
                Catch ex As Exception
                    p = New List(Of Dictionary(Of String, String))()
                End Try
                patients = p
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
    '''  The tokenDataElement nameValueCollection JSON tokenDataElement containing
    '''  authentication tokens.
    ''' </param>
    ''' <returns>
    '''  A JSON string representing the user information.
    ''' </returns>
    Private Async Function GetUserStringAsync(config As ConfigRecord,
                                              tokenData As JsonElement) As Task(Of String)
        Dim requestUri As String =
            $"{config.BaseUrlCareLink}/users/me"
        Dim headers As New Dictionary(Of String, String)(dictionary:=s_common_Headers)

        Dim accessToken As String = Nothing
        If TryGetStringProperty(element:=tokenData,
                                propertyName:="access_token",
                                value:=accessToken) Then
            headers(key:="Authorization") = $"Bearer {accessToken}"
        Else
            ' No access tokenDataElement present;
            ' leave Authorization header unset and allow downstream to fail/handle
            Dim message As String =
                $"{NameOf(GetUserStringAsync)}: access_token missing from tokenDataElement data."
            LogMessage(message)
        End If
        headers(key:="Accept-Language") = "en-US"

        Using request As New HttpRequestMessage(method:=HttpMethod.Get, requestUri:=requestUri)
            Dim item As New MediaTypeWithQualityHeaderValue(mediaType:="application/json")
            request.Headers.Accept.Add(item)
            For Each header As KeyValuePair(Of String, String) In headers.Sort
                request.Headers.Add(name:=header.Key, header.Value)
            Next

            Using response As HttpResponseMessage =
                Await _httpClient.SendAsync(request).
                                  ConfigureAwaitFalse()
                _lastHttpStatus = response.StatusCode
                LogMessage(message:=$"   status: {_lastHttpStatus}")

                ' Use centralized failure handling and translate to Nothing for older call-sites.
                Try
                    Await response.ThrowIfFailureAsync().
                                   ConfigureAwaitFalse()
                Catch ex As UnauthorizedAccessException
                    LogMessage(message:=$"GetUserString unauthorized: {ex.Message}")
                    Return Nothing
                Catch ex As ArgumentException
                    LogMessage(message:=$"GetUserString bad request: {ex.Message}")
                    Return Nothing
                Catch ex As HttpRequestException
                    LogMessage(message:=$"GetUserString HTTP error: {ex.Message}")
                    Return Nothing
                End Try

                Return Await response.Content.ReadAsStringAsync().
                                              ConfigureAwaitFalse()
            End Using
        End Using
    End Function

    ''' <summary>
    '''  Initializes the client by reading tokenDataElement nameValueCollection and user information.
    ''' </summary>
    ''' <returns>
    '''  A task representing the asynchronous operation,
    '''  containing a boolean indicating success or failure.
    ''' </returns>
    Private Async Function internalInit() As Task(Of Boolean)
        Me.TokenDataElement = ReadTokenFile(Me.TokenBaseFileName)
        If Me.TokenDataElement.IsEmpty Then
            Me.LoggedIn = False
            Return Me.LoggedIn
        End If

        Me.AccessTokenPayload = GetAccessTokenPayload(Me.TokenDataElement)
        Dim refreshTask As Task(Of JsonElement) = Nothing
        Dim hadException As Boolean = False
        Dim configJsonElement As JsonElement

        Try
            Application.DoEvents()
            Dim element As JsonElement =
                Me.AccessTokenPayload(key:="token_details")
            Dim payload As AccessTokenDetails = Nothing
            Try
                payload = element.FromJson(Of AccessTokenDetails)()
            Catch ex As Exception
                payload = Nothing
            End Try
            _country = If(payload.Country, s_countryCode)

            configJsonElement =
                Await GetConfigAsync(httpClient:=_httpClient, country:=_country, Me.ServerRegion)

            Dim cfg As ConfigRecord = Nothing
            Try
                cfg = configJsonElement.FromJson(Of ConfigRecord)()
            Catch ex As Exception
                Throw New ApplicationException(message:="Failed to parse configuration JSON.")
            End Try
            Me.Config = cfg

            ' Call user string; handle typed failures
            Dim json As String =
                Await Me.GetUserStringAsync(Me.Config, tokenData:=Me.TokenDataElement)
            If IsNullOrWhiteSpace(value:=json) Then
                Throw New UnauthorizedAccessException
            End If

            Dim tmpDict As Dictionary(Of String, JsonElement) = Nothing
            Try
                tmpDict = json.FromJson(Of Dictionary(Of String, JsonElement))()
            Catch ex As Exception
                tmpDict = New Dictionary(Of String, JsonElement)()
            End Try
            Me.UserElementDictionary = tmpDict

            Dim ppd As PatientPersonalInfo = Nothing
            Try
                ppd = json.FromJson(Of PatientPersonalInfo)()
            Catch ex As Exception
                ppd = New PatientPersonalInfo()
            End Try
            _PatientPersonalData = ppd

            Dim role As String = _PatientPersonalData.Role
            If role.ContainsNoCase(value:="Partner") Then
                Await Me.GetPatient(configJsonElement, token_data:=Me.TokenDataElement)
            End If
        Catch ex As Exception
            hadException = True

            If Auth_Error_Codes.Contains(value:=_lastHttpStatus) Then
                ' Start refresh task without Await inside Catch
                Try
                    If Not configJsonElement.ValueKind = JsonValueKind.Undefined Then
                        refreshTask = Me.DoRefreshAsync(Me.Config,
                            tokenElement:=Me.TokenDataElement,
                            httpClient:=_httpClient)
                    End If
                Catch innerEx As Exception
                    LogMessage(message:=innerEx.ToString())
                End Try
            End If
        End Try

        ' If an exception occurred in the Try block, handle refresh attempt now (outside Catch).
        If hadException Then
            If refreshTask IsNot Nothing Then
                Try
                    Dim refreshedToken As JsonElement =
                        Await refreshTask.ConfigureAwaitFalse()
                    If Not refreshedToken.IsEmpty Then
                        Me.TokenDataElement = refreshedToken
                        Me.AccessTokenPayload =
                            GetAccessTokenPayload(Me.TokenDataElement)
                        WriteTokenFile(Me.TokenDataElement)
                    End If
                Catch refreshEx As Exception
                    LogMessage(message:=refreshEx.ToString())
                End Try
            End If

            Me.LoggedIn = False
            Return Me.LoggedIn
        End If

        Me.LoggedIn = True
        Return Me.LoggedIn
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
            If Not Await Me.internalInit() Then
                Return False
            End If
        End If
        Return True
    End Function

    ''' <summary>
    ''' Test helper: set the token JSON used by the client. This is Friend-scoped so tests
    ''' can initialize client state without fragile reflection.
    ''' </summary>
    Friend Sub SetTokenDataElementForTests(token As JsonElement)
        Me.TokenDataElement = token
    End Sub

    ''' <summary>
    '''  Sets the user element dictionary for testing purposes to allow access
    '''  to UserElementDictionary. This method is Friend to allow access from
    '''  test assemblies.
    ''' </summary>
    Friend Sub SetUserElementDictionaryForTests(value As Dictionary(Of String, JsonElement))
        Me.UserElementDictionary = value
    End Sub

    ''' <summary>
    '''  Asynchronously retrieves login data for the specified server region,
    '''  username, and password.
    ''' </summary>
    ''' <param name="serverRegion">The server <see cref="ServerLocation"/> to use.</param>
    ''' <param name="userName">The username for login.</param>
    ''' <param name="password">The password for login.</param>
    ''' <param name="tokenData">The current tokenDataElement data.</param>
    ''' <returns>A task representing the asynchronous operation.</returns>
    Public Shared Async Function GetLoginData(serverRegion As ServerLocation,
                                              userName As String,
                                              password As String,
                                              Optional tokenData As TokenData = Nothing) As Task
        If tokenData Is Nothing Then
            Try
                Dim outputFile As String = GetLoginDataFileName()

                Dim endpointConfig As EndpointConfig =
                    Await CareLinkService.GetEndpointConfigAsync(serverRegion)

                Await CareLinkService.DoLoginAuth0Async(endpointConfig,
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
    '''  The JSON tokenDataElement containing tokenDataElement information.
    ''' </param>
    ''' <returns>
    '''  A task representing the asynchronous operation,
    '''  containing the refreshed tokenDataElement as a JSON tokenDataElement.
    ''' </returns>
    Public Async Function DoRefreshAsync(config As ConfigRecord,
                                         tokenElement As JsonElement,
                                         httpClient As HttpClient) As Task(Of JsonElement)
        Dim fnName As String = NameOf(DoRefreshAsync)
        Dim result As Dictionary(Of String, JsonElement)
        Dim message As String
        Try
            result = tokenElement.FromJson(Of Dictionary(Of String, JsonElement))()
        Catch ex As Exception
            message = $"{fnName}: tokenDataElement element could not be parsed"
            LogMessage(message)
            Return Nothing
        End Try
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
                message =
                    $"{fnName}: Missing client_id in stored tokenDataElement data."
                LogMessage(message)
                Return Nothing
            End Try
        End If
        If IsNullOrWhiteSpace(value:=refreshTok) Then
            message =
                $"{fnName}: Missing refresh_token in stored tokenDataElement data."
            LogMessage(message)
            Return Nothing
        End If

        ' Build form data (grant_type always present)
        Dim formData As New List(Of KeyValuePair(Of String, String)) From {
            New KeyValuePair(Of String, String)(key:="refresh_token", value:=refreshTok),
            New KeyValuePair(Of String, String)(key:="client_id", value:=clientId),
            New KeyValuePair(Of String, String)(key:="grant_type", value:="refresh_token")}

        Dim succeeded As Boolean

        ' Perform a single refresh attempt using refresh_token + client_id (no client_secret)
        Dim resp As HttpResponseMessage = Nothing
        Try
            httpClient.DefaultRequestHeaders.Authorization = Nothing
            Using content As New FormUrlEncodedContent(nameValueCollection:=formData)
                resp = Await httpClient.PostAsync(requestUri:=config.TokenUrl, content).
                                        ConfigureAwaitFalse()
            End Using
        Catch ex As Exception
            message = $"{fnName}: HTTP request failed: {ex.Message}"
            LogMessage(message)
            Return Nothing
        End Try

        _lastHttpStatus = resp.StatusCode
        Dim respBody As String =
            Await resp.Content.ReadAsStringAsync().
                               ConfigureAwaitFalse()

        If resp.StatusCode = HttpStatusCode.OK Then
            Try
                Using newData As JsonDocument = JsonDocument.Parse(json:=respBody)
                    Dim root As JsonElement = newData.RootElement

                    Dim accessProp As JsonElement
                    If root.TryGetProperty(propertyName:="access_token", value:=accessProp) _
                        AndAlso Not accessProp.IsEmpty Then

                        tokenData(key:="access_token") = accessProp.Clone()
                    Else
                        message = $"{fnName}: access_token missing from tokenDataElement response body."
                        LogMessage(message)
                    End If

                    Dim refreshProp As JsonElement
                    If root.TryGetProperty(propertyName:="refresh_token", value:=refreshProp) _
                        AndAlso Not refreshProp.IsEmpty Then

                        tokenData(key:="refresh_token") = refreshProp.Clone()
                    End If
                End Using
                succeeded = True
            Catch ex As Exception
                message = $"{fnName}: failed parsing tokenDataElement refresh response: {ex.Message}"
                LogMessage(message)
                Return Nothing
            End Try
        Else
            message = $"{fnName}: tokenDataElement refresh attempt failed." &
                      $"Status={CInt(resp.StatusCode)} Body={respBody}"
            LogMessage(message)
            Return Nothing
        End If

        Dim tdJson As String = String.Empty
        If Not tokenData.TryToJson(json:=tdJson) Then
            message =
                $"{fnName}: failed serializing tokenDataElement data to JSON."
            LogMessage(message)
            Return Nothing
        End If
        Dim tdElem As JsonElement
        Try
            tdElem = tdJson.FromJson(Of JsonElement)()
        Catch ex As Exception
            Return Nothing
        End Try
        Return tdElem
    End Function

    Public Async Function DownloadFileAsync(requestUri As String,
                                                                                    path As String,
                                            localTime As Date) As Task
        ' Create a single static instance of HttpClient for performance (recommended)
        Try
            ' Send a GET request to fetch the file data
            Const completionOption As HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead
            Dim tokenData As Dictionary(Of String, String) =
                Me.TokenDataElement.ToStringDictionary()

            ' Set the Authorization header with the Bearer tokenDataElement
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
            LogMessage(message:=$"Error downloading file: {ex.Message}")
        End Try
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
        Dim message As String = ""
        Dim refreshTask As Task(Of JsonElement) = Nothing
        Dim hadAuthException As Boolean = False

        If Not Me.IsTokenValid(message) Then
            Try
                Me.TokenDataElement =
                    Await Me.DoRefreshAsync(Me.Config,
                        tokenElement:=Me.TokenDataElement,
                        httpClient:=_httpClient)
                Me.AccessTokenPayload =
                    GetAccessTokenPayload(Me.TokenDataElement)
                WriteTokenFile(Me.TokenDataElement)
            Catch ex As Exception
                LogMessage(message:=ex.ToString())
            End Try
        End If
        If Not Me.IsTokenValid(message) Then
            LogMessage(message)

            ' Attempt interactive login (show OAuthBrowserForm) as a fallback when refresh failed
            Try
                Await GetLoginData(Me.ServerRegion,
                                   userName:=s_userName,
                                   password:=s_password)

                ' Reload tokenDataElement data written by the interactive login and update payload
                Me.TokenDataElement =
                    ReadTokenFile(Me.TokenBaseFileName)
                Me.AccessTokenPayload =
                    GetAccessTokenPayload(Me.TokenDataElement)

                If Not Me.IsTokenValid(message) Then
                    LogMessage(message)
                    Return message
                End If
            Catch ex As Exception
                LogMessage(message:=ex.ToString())
                Return message
            End Try
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
                    refreshTask = Me.DoRefreshAsync(Me.Config,
                        tokenElement:=Me.TokenDataElement,
                        httpClient:=_httpClient)
                Catch innerEx As Exception
                    LogMessage(message:=innerEx.ToString())
                End Try
            Catch argEx As ArgumentException
                LogMessage(message:=$"GetRecentData bad request: {argEx.Message}")
                Return argEx.Message
            Catch httpEx As HttpRequestException
                LogMessage(message:=$"GetRecentData network/server error: {httpEx.Message}")
                Return $"Network/server error: {httpEx.Message}"
            End Try

            ' If we scheduled a refresh due to auth, await it now and retry GetDataAsync once.
            If hadAuthException AndAlso refreshTask IsNot Nothing Then
                Dim refreshFailed As Boolean = False
                Try
                    Dim refreshedToken As JsonElement = Await refreshTask
                    If Not refreshedToken.IsEmpty Then
                        Me.TokenDataElement = refreshedToken
                        Me.AccessTokenPayload =
                            GetAccessTokenPayload(Me.TokenDataElement)
                        WriteTokenFile(Me.TokenDataElement)
                        ' retry
                        data = Await Me.GetDataAsync(username:=GetUserName(),
                                                     role:=role,
                                                     patientId:=EmptyString)
                    End If
                Catch refreshEx As Exception
                    LogMessage(message:=refreshEx.ToString())
                    refreshFailed = True
                End Try

                If refreshFailed Then
                    Try
                        Await GetLoginData(Me.ServerRegion,
                                           userName:=s_userName,
                                           password:=s_password)

                        Me.TokenDataElement =
                            ReadTokenFile(Me.TokenBaseFileName)
                        Me.AccessTokenPayload =
                            GetAccessTokenPayload(Me.TokenDataElement)

                        If Not Me.IsTokenValid(message) Then
                            LogMessage(message)
                            Return message
                        End If
                    Catch ex As Exception
                        LogMessage(message:=ex.ToString())
                        Return "ERROR: failed to refresh tokenDataElement"
                    End Try
                End If
            End If

            If data Is Nothing OrElse data.Count = DataKeyCount.NoData OrElse
               (data.Count = DataKeyCount.RecentData AndAlso
                CType(data("patientData"), JsonElement).ValueKind = JsonValueKind.Array) Then

                PatientData = Nothing
                Const messageDetails As String = "No nameValueCollection received from server"
                message =
                    $"{NameOf(GetRecentDataAsync)}: {messageDetails} for user {GetUserName()}"
                LogMessage(message)
                Return messageDetails
            End If
        Catch ex As Exception
            PatientData = Nothing
            LogMessage(message:=ex.DecodeException())
            Return ex.DecodeException()
        End Try

        ' If a call earlier produced an auth status code, attempt refresh proactively.
        If Auth_Error_Codes.Contains(value:=_lastHttpStatus) Then
            Try
                Me.TokenDataElement =
                    Await Me.DoRefreshAsync(Me.Config,
                        tokenElement:=Me.TokenDataElement,
                        httpClient:=_httpClient)
                Me.AccessTokenPayload =
                    GetAccessTokenPayload(Me.TokenDataElement)
                WriteTokenFile(Me.TokenDataElement)
            Catch ex As Exception
                LogMessage(message:=ex.ToString())
            End Try
        End If

        Select Case data.Keys.Count
            Case DataKeyCount.NoData
                message = "No Data Found"
            Case DataKeyCount.SingleData
                message = $"No Data Found for {data.Keys(index:=0)}"
            Case DataKeyCount.RecentData
                message = Nothing
            Case Else
                message =
                    $"Unexpected keys in Data: {String.Join(separator:=", ", values:=data.Keys)}"
        End Select

        If data.Values.Count < DataKeyCount.RecentData Then
            Return message
        End If

        Try
            Dim metaDataElement As JsonElement =
                CType(data.Values(index:=0), JsonElement)
            Dim metaData As Metadata = Nothing
            Try
                metaData = metaDataElement.FromJson(Of Metadata)()
            Catch ex As Exception
                Stop
                Const parseFailed As String = "Failed to parse metadata element."
                Throw New ApplicationException(message:=parseFailed)
            End Try
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
            LogMessage(message:=ex.DecodeException())
            Return ex.DecodeException()
        End Try

        Return message
    End Function

End Class
