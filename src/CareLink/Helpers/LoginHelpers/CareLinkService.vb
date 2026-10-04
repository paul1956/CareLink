' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http
Imports System.Text.Json
Imports System.Text
Imports System.IO
Imports System.IO.Compression

Public Class CareLinkService

    ' Cache resolved endpoint configurations per server region so we only resolve
    ' them when the region changes.
    Private Shared ReadOnly s_endpointCache As New Dictionary(Of ServerLocation, EndpointConfig)()

    Private Shared ReadOnly s_endpointCacheLock As New Object()
    Private Shared ReadOnly s_http As New HttpClient(
        New HttpClientHandler() With {
            .AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate Or DecompressionMethods.Brotli
        }) With {.Timeout = TimeSpan.FromSeconds(120)}

    ' Reuse JsonSerializerOptions to avoid allocations on each parse
    Private Shared ReadOnly s_jsonOptions As New JsonSerializerOptions With {
        .PropertyNameCaseInsensitive = True
    }

    Public Const DiscoveryUrlEu As String =
        "https://clcloud.minimed.eu/connect/carepartner/v13/discover/android/3.6"

    Public Const DiscoveryUrlUs As String =
        "https://clcloud.minimed.com/connect/carepartner/v13/discover/android/3.6"

    Private Shared Function EscapeKVP(Name As String, value As String) As String
        Return $"{Name}={Uri.EscapeDataString(stringToEscape:=value)}"
    End Function

    ''' <summary>
    '''  Invokes the provided work on the application's UI thread (if an open form exists) and
    '''  returns the result. This ensures COM/STA-bound UI operations execute correctly.
    ''' </summary>
    Private Shared Function InvokeOnUiThreadAsync(Of T)(work As Func(Of T)) As Task(Of T)
        Dim tcs As New TaskCompletionSource(Of T)()

        Try
            If Application.OpenForms IsNot Nothing AndAlso Application.OpenForms.Count > 0 Then
                Dim ctrl As Control = Application.OpenForms(index:=0)
                Dim method As New MethodInvoker(
                    Sub()
                        Try
                            Dim result As T = work()
                            tcs.SetResult(result)
                        Catch ex As Exception
                            tcs.SetException(ex)
                        End Try
                    End Sub)
                ctrl.BeginInvoke(method)
            Else
                ' No open forms available; run synchronously on the current thread as a fallback.
                ' This may still fail if not on an STA/UI thread, but in normal app lifetime
                ' there is a main form.
                Dim result As T = work()
                tcs.SetResult(result)
            End If
        Catch ex As Exception
            tcs.SetException(ex)
        End Try

        Return tcs.Task
    End Function

    ' Normalize and validate SSO JSON returned by the discovery endpoint.
    Private Shared Function NormalizeSsoJson(s As String) As String
        If s Is Nothing Then Return s

        ' Replace common VB literal concatenation tokens with actual whitespace/newlines.
        s = s.Replace(" & vbCrLf & ", vbCrLf) _
             .Replace(" & vbTab & ", vbTab) _
             .Replace(""" & vbCrLf & """, vbCrLf)

        Return s.Trim()
    End Function

    Private Shared Function ParseAndValidateSsoJson(ssoJson As String) As SsoConfig
        Dim normalized As String = NormalizeSsoJson(ssoJson)

        Dim opts As JsonSerializerOptions = s_jsonOptions

        Dim sso As SsoConfig
        Try
            sso = JsonSerializer.Deserialize(Of SsoConfig)(normalized, opts)
        Catch ex As Exception
            Throw New Exception(message:=$"Failed to parse SSO JSON: {ex.Message}", innerException:=ex)
        End Try

        If sso Is Nothing OrElse sso.Server Is Nothing Then
            Throw New Exception(message:="SSO JSON missing 'server' node.")
        End If

        If sso.Server.ServerCerts IsNot Nothing Then
            For Each certLines As List(Of String) In sso.Server.ServerCerts
                Dim pem As String =
                    String.Join(separator:=vbCrLf, values:=certLines)
                If Not (pem.Contains(value:="-----BEGIN CERTIFICATE-----") AndAlso
                    pem.Contains(value:="-----END CERTIFICATE-----")) Then
                    Const message As String =
                        "One or more server certificates appear incomplete or truncated."
                    Throw New Exception(message:=message)
                End If
            Next
        End If

        Return sso
    End Function

    ''' <summary>
    ''' Resolve endpoint configuration from an already-obtained DiscoveryRoot.
    ''' This contains the logic that previously lived in ResolveEndpointConfigAsync
    ''' after discovery JSON was fetched.
    ''' </summary>
    Private Shared Async Function ResolveEndpointConfigFromDiscoveryAsync(
        discovery As DiscoveryRoot,
        serverRegion As ServerLocation) As Task(Of EndpointConfig)

        Dim message As String
        If discovery Is Nothing OrElse discovery.CP Is Nothing Then
            Throw New Exception(message:="Discovery JSON did not contain CP entries.")
        End If

        Dim targetRegion As String = serverRegion.ToString()
        For Each c As CPEntry In discovery.CP

            If EqualsNoCase(a:=c.Region, b:=targetRegion) Then
                Dim lookupName As String = c.UseSSOConfiguration
                If String.IsNullOrWhiteSpace(value:=lookupName) Then
                    message = $"SSO lookup name missing for region {serverRegion}"
                    Throw New Exception(message)
                End If

                Dim ssoUrl As String =
                    ClassHelpers.GetPropertyValue(instance:=c, propertyName:=lookupName)
                If String.IsNullOrWhiteSpace(value:=ssoUrl) Then
                    Throw New Exception(message:=$"SSO URL is empty for region {serverRegion}")
                End If

                Dim ssoJson As String =
                    Await s_http.GetStringAsync(requestUri:=ssoUrl).ConfigureAwaitFalse()
                Dim ssoDoc As SsoConfig = Nothing
                Try
                    ssoDoc = ParseAndValidateSsoJson(ssoJson)
                Catch ex As Exception
                    Throw New Exception(message:=$"Failed to parse SSO JSON: {ex.Message}")
                End Try

                If ssoDoc Is Nothing OrElse ssoDoc.Server Is Nothing Then
                    Throw New Exception(message:=$"Invalid SSO JSON for region {serverRegion}")
                End If

                Dim hostname As String = ssoDoc.Server.Hostname
                Dim portNum As Integer = ssoDoc.Server.Port
                Dim prefix As String = ssoDoc.Server.Prefix
                Dim hostPart As String = If(portNum = 443, hostname, $"{hostname}:{portNum}")
                Dim apiBaseUrl As String = $"https://{hostPart}/{prefix}".TrimEnd("/"c)

                Return New EndpointConfig With {
                    .SsoJson = ssoJson,
                    .ApiBaseUrl = apiBaseUrl}
            End If
        Next

        message =
            $"Could not find server configuration for region {serverRegion}"
        Throw New Exception(message)
    End Function

    Public Shared Async Function DoLoginAuth0Async(endpointConfig As EndpointConfig,
                                                   outputFile As String,
                                                   userName As String,
                                                   password As String) As Task(Of TokenData)

        Dim message As String
        Dim ssoConfig As SsoConfig = Nothing
        Try
            ssoConfig = ParseAndValidateSsoJson(endpointConfig.SsoJson)
        Catch ex As Exception
            Throw New ApplicationException(message:="Failed to parse SSO configuration JSON.")
        End Try
        If ssoConfig Is Nothing Then
            Throw New ApplicationException(message:="Failed to parse SSO configuration JSON.")
        End If
        Dim client As Client = ssoConfig.Client
        Dim clientId As String = client.ClientId
        Dim scope As String = client.Scope
        Dim redirectUri As String = client.RedirectUri
        Dim audience As String = client.Audience

        Dim authorizePath As String = ssoConfig.SystemEndpoints.AuthorizationEndpointPath
        Dim tokenPath As String = ssoConfig.SystemEndpoints.TokenEndpointPath

        Dim authUrl As String = $"{endpointConfig.ApiBaseUrl}{authorizePath}"
        Dim fullUrl As String =
            $"{authUrl}?{EscapeKVP(Name:="client_id", value:=clientId)}&" &
            $"response_type=code&" &
            $"{EscapeKVP(Name:="scope", value:=scope)}&" &
            $"{EscapeKVP(Name:="redirect_uri", value:=redirectUri)}&" &
            $"{EscapeKVP(Name:="audience", value:=audience)}"

        Dim redirectResult As RedirectResult

        ' Ensure the UI dialog and WebView2 initialization run on the UI thread.
        redirectResult = Await InvokeOnUiThreadAsync(
           work:=Function()
                     Do
                         Using frm As New OAuthBrowserForm(startUrl:=fullUrl,
                                                            redirectUri,
                                                            userName,
                                                            password)
                             ' Determine the best owner for the OAuth dialog. Prefer the
                             ' currently active form (which may be a modal dialog like
                             ' the LoginDialog). Fall back to the main Form1 if none.
                             Dim ownerForm As System.Windows.Forms.Form = If(System.Windows.Forms.Form.ActiveForm, My.Forms.Form1)
                             Dim dr As DialogResult = frm.ShowDialog(owner:=ownerForm)
                             If dr = DialogResult.OK Then
                                 LoginRetryCount = 1
                                 Return frm.Result
                             ElseIf dr = DialogResult.Retry Then
                                 ' Caller will recreate the dialog and try again
                                 Continue Do
                             Else
                                 If LoginRetryCount > 0 Then
                                     LoginRetryCount -= 1
                                     Continue Do
                                 End If
                                 LoginRetryCount = 1
                                 s_firstTimeNavigationCompleted = False
                                 Throw New Exception(message:="Login was cancelled.")
                             End If
                         End Using
                     Loop
                 End Function)

        If redirectResult Is Nothing OrElse IsNullOrWhiteSpace(value:=redirectResult.Code) Then
            message = "Authorization code was not captured."
            Throw New Exception(message)
        End If

        Dim tokenUrl As String = $"{endpointConfig.ApiBaseUrl}{tokenPath}"
        Dim form As New List(Of KeyValuePair(Of String, String)) From {
            New KeyValuePair(Of String, String)(key:="grant_type", value:="authorization_code"),
            New KeyValuePair(Of String, String)(key:="client_id", value:=clientId),
            New KeyValuePair(Of String, String)(key:="code", value:=redirectResult.Code),
            New KeyValuePair(Of String, String)(key:="redirect_uri", value:=redirectUri)}

        Dim content As New FormUrlEncodedContent(nameValueCollection:=form)
        ' Use an explicit HttpRequestMessage and mimic python-requests headers to reduce
        ' the chance of triggering WAF/edge rules that differ by User-Agent/Accept.
        Dim response As HttpResponseMessage
        Using req As New HttpRequestMessage(method:=HttpMethod.Post, requestUri:=New Uri(tokenUrl))
            req.Content = content
            Try
                req.Headers.TryAddWithoutValidation("User-Agent", "python-requests/2.31.0")
            Catch
            End Try
            Try
                req.Headers.TryAddWithoutValidation("Accept", "*/*")
            Catch
            End Try

            response = Await s_http.SendAsync(request:=req)
        End Using

        ' Read raw bytes first to allow robust decoding when the response contains
        ' unexpected binary/encoding; then produce a best-effort string for logging.
        Dim rawBytes As Byte() = Nothing
        Try
            rawBytes = Await response.Content.ReadAsByteArrayAsync()
        Catch
            rawBytes = Nothing
        End Try

        Dim body As String = String.Empty
        If rawBytes IsNot Nothing Then
            Dim charset As String = Nothing
            If response.Content IsNot Nothing AndAlso response.Content.Headers IsNot Nothing AndAlso response.Content.Headers.ContentType IsNot Nothing Then
                charset = response.Content.Headers.ContentType.CharSet
            End If

            Dim enc As Encoding = Nothing
            If Not String.IsNullOrWhiteSpace(charset) Then
                Try
                    enc = Encoding.GetEncoding(charset)
                Catch
                    enc = Nothing
                End Try
            End If
            If enc Is Nothing Then enc = Encoding.UTF8

            ' HttpClientHandler.AutomaticDecompression is enabled for s_http.
            ' The content returned by HttpClient will be decompressed automatically
            ' when the server sends Content-Encoding (gzip/deflate/brotli). Therefore,
            ' we can safely decode the received bytes using the declared charset or
            ' UTF-8 fallback.
            Try
                body = enc.GetString(rawBytes)
            Catch
                Try
                    body = Encoding.GetEncoding(28591).GetString(rawBytes)
                Catch
                    body = String.Empty
                End Try
            End Try
        End If

        If Not response.IsSuccessStatusCode Then
            ' Previously we wrote diagnostic dumps containing sensitive token data.
            ' Remove persistent dumps in favor of throwing a non-descriptive error
            ' and logging a short message to the in-memory logger only.
            Try
                LogMessage(message:=$"Token request failed with status {CInt(response.StatusCode)} {response.ReasonPhrase}")
            Catch
            End Try

            message = $"Could not get token data in {NameOf(DoLoginAuth0Async)}"
            Throw New Exception(message)
        End If

        Dim token As TokenData = Nothing
        Try
            Dim mediaType As String = Nothing
            If response.Content IsNot Nothing AndAlso response.Content.Headers IsNot Nothing AndAlso response.Content.Headers.ContentType IsNot Nothing Then
                mediaType = response.Content.Headers.ContentType.MediaType
            End If

            If mediaType IsNot Nothing AndAlso mediaType.Contains("json", StringComparison.OrdinalIgnoreCase) Then
                Try
                    ' First attempt: parse as-is
                    token = body.FromJson(Of TokenData)()
                Catch jex As JsonException
                    ' The body may contain unexpected binary or encoding. Use rawBytes already read above.
                    ' rawBytes was pre-read before attempting JSON parsing.

                    Dim parsed As Boolean = False
                    If rawBytes IsNot Nothing Then
                        Dim encodings As Encoding() = New Encoding() {
                            Encoding.UTF8,
                            Encoding.Unicode,
                            Encoding.BigEndianUnicode,
                            Encoding.UTF32,
                            Encoding.GetEncoding(28591) ' ISO-8859-1
                        }

                        For Each enc As Encoding In encodings
                            Try
                                Dim candidate As String = enc.GetString(rawBytes)
                                token = candidate.FromJson(Of TokenData)()
                                parsed = True
                                Exit For
                            Catch
                                ' try next encoding
                            End Try
                        Next
                    End If

                    If Not parsed Then
                        ' Persist raw bytes and text to temp files for offline inspection then throw
                        Try
                            Dim dumpBase As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"carelink_token_raw_{Date.UtcNow:yyyyMMddHHmmss}")
                            If rawBytes IsNot Nothing Then
                                System.IO.File.WriteAllBytes(path:=dumpBase & ".bin", bytes:=rawBytes)
                            End If
                            System.IO.File.WriteAllText(path:=dumpBase & ".txt", contents:=body)
                        Catch
                        End Try

                        message = $"Failed to parse token response JSON after multiple encodings. Raw dump: see temp files starting with carelink_token_raw_. Body starts with: {If(body?.Substring(0, Math.Min(200, body.Length)), "<empty>")}"
                        Throw New ApplicationException(message, jex)
                    End If
                End Try
            Else
                message = $"Token endpoint returned non-JSON content. Content-Type={mediaType}. Body={body}"
                Throw New ApplicationException(message)
            End If
        Catch ex As JsonException
            message = $"Failed to parse token response JSON. Body={body}"
            Throw New ApplicationException(message, ex)
        End Try
        token.ClientId = clientId
        WriteTokenFile(token, path:=outputFile)
        Return token
    End Function

    ''' <summary>
    ''' Gets the EndpointConfig for the given serverRegion. Uses an in-memory cache
    ''' and only resolves (network calls) when the region hasn't been resolved yet
    ''' or the region has changed.
    ''' </summary>
    Public Shared Async Function GetEndpointConfigAsync(
        serverRegion As ServerLocation) As Task(Of EndpointConfig)

        Dim cfg As EndpointConfig = Nothing
        SyncLock s_endpointCacheLock
            If s_endpointCache.TryGetValue(key:=serverRegion, value:=cfg) Then
                Return cfg
            End If
        End SyncLock

        ' Ensure discovery JSON is cached per region and only fetched when needed.
        Dim discovery As DiscoveryRoot =
            Await GetCachedDiscoveryAsync(serverRegion).ConfigureAwaitFalse()
        Dim resolved As EndpointConfig =
            Await ResolveEndpointConfigFromDiscoveryAsync(discovery,
                                                          serverRegion).ConfigureAwaitFalse()

        SyncLock s_endpointCacheLock
            s_endpointCache(key:=serverRegion) = resolved
        End SyncLock

        Return resolved
    End Function

    Public Shared Async Function ResolveEndpointConfigAsync(
        serverRegion As ServerLocation) As Task(Of EndpointConfig)

        ' Use the cached discovery (or fetch it once if missing) and resolve from that.
        Dim discovery As DiscoveryRoot =
            Await GetCachedDiscoveryAsync(serverRegion).ConfigureAwaitFalse()
        Return Await ResolveEndpointConfigFromDiscoveryAsync(discovery,
                                                             serverRegion).ConfigureAwaitFalse()
    End Function

End Class
