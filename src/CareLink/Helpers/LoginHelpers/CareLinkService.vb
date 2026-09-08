' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Text.Json

Public Class CareLinkService
    Private Shared ReadOnly s_http As New HttpClient With {.Timeout = TimeSpan.FromSeconds(120)}
    Public Const DiscoveryUrlEu As String = "https://clcloud.minimed.eu/connect/carepartner/v13/discover/android/3.6"
    Public Const DiscoveryUrlUs As String = "https://clcloud.minimed.com/connect/carepartner/v13/discover/android/3.6"

    Public Shared Async Function DoLoginAuth0Async(endpointConfig As EndpointConfig,
                                                    outputFile As String,
                                                    userName As String,
                                                    password As String) As Task(Of TokenData)

        Dim message As String
        Dim ssoConfig As SsoConfig = Nothing
        If Not endpointConfig.SsoJson.TryFromJson(result:=ssoConfig) Then
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
                         Dim retryCount As Integer = 1
                         Using frm As New OAuthBrowserForm(startUrl:=fullUrl,
                                                           redirectUri,
                                                           userName,
                                                           password)
                             Dim dr As DialogResult = frm.ShowDialog()
                             If dr = DialogResult.OK Then
                                 Return frm.Result
                             ElseIf dr = DialogResult.Retry Then
                                 ' Caller will recreate the dialog and try again
                                 Continue Do
                             Else
                                 If retryCount > 0 Then
                                     retryCount -= 1
                                     Continue Do
                                 End If
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
        Dim response As HttpResponseMessage = Await s_http.PostAsync(requestUri:=tokenUrl, content)
        Dim body As String = Await response.Content.ReadAsStringAsync()

        If Not response.IsSuccessStatusCode Then
            message = $"Could not get token data in {NameOf(DoLoginAuth0Async)}: {body}"
            Throw New Exception(message)
        End If

        Dim token As TokenData = Nothing
        If Not body.TryFromJson(result:=token) Then
            message = "Failed to parse token response JSON."
            Throw New ApplicationException(message)
        End If
        token.ClientId = clientId
        WriteTokenFile(token, path:=outputFile)
        Return token
    End Function

    ' Cache resolved endpoint configurations per server region so we only resolve
    ' them when the region changes.
    Private Shared ReadOnly s_endpointCache As New Dictionary(Of ServerLocation, EndpointConfig)()

    Private Shared ReadOnly s_endpointCacheLock As New Object()

    ''' <summary>
    ''' Gets the EndpointConfig for the given serverRegion. Uses an in-memory cache
    ''' and only resolves (network calls) when the region hasn't been resolved yet
    ''' or the region has changed.
    ''' </summary>
    Public Shared Async Function GetEndpointConfigAsync(serverRegion As ServerLocation) As Task(Of EndpointConfig)
        Dim cfg As EndpointConfig = Nothing
        SyncLock s_endpointCacheLock
            If s_endpointCache.TryGetValue(key:=serverRegion, value:=cfg) Then
                Return cfg
            End If
        End SyncLock

        ' Ensure discovery JSON is cached per region and only fetched when needed.
        Dim discovery As DiscoveryRoot = Await Discover.GetCachedDiscoveryAsync(serverRegion).ConfigureAwait(False)
        Dim resolved As EndpointConfig = Await ResolveEndpointConfigFromDiscoveryAsync(discovery, serverRegion).ConfigureAwait(False)

        SyncLock s_endpointCacheLock
            s_endpointCache(serverRegion) = resolved
        End SyncLock

        Return resolved
    End Function

    ''' <summary>
    ''' Resolve endpoint configuration from an already-obtained DiscoveryRoot.
    ''' This contains the logic that previously lived in ResolveEndpointConfigAsync
    ''' after discovery JSON was fetched.
    ''' </summary>
    Private Shared Async Function ResolveEndpointConfigFromDiscoveryAsync(discovery As DiscoveryRoot, serverRegion As ServerLocation) As Task(Of EndpointConfig)
        If discovery Is Nothing OrElse discovery.CP Is Nothing Then
            Throw New Exception(message:="Discovery JSON did not contain CP entries.")
        End If

        Dim targetRegion As String = serverRegion.ToString()
        Const comparisonType As StringComparison = StringComparison.OrdinalIgnoreCase
        For Each c As CPEntry In discovery.CP

            If String.Equals(c.Region, targetRegion, comparisonType) Then
                Dim lookupName As String = c.UseSSOConfiguration
                If String.IsNullOrWhiteSpace(value:=lookupName) Then
                    Throw New Exception(message:=$"SSO lookup name missing for region {serverRegion}")
                End If

                Dim ssoUrl As String = ClassHelpers.GetPropertyValue(c, lookupName)
                If String.IsNullOrWhiteSpace(value:=ssoUrl) Then
                    Throw New Exception(message:=$"SSO URL is empty for region {serverRegion}")
                End If

                Dim ssoJson As String = Await s_http.GetStringAsync(requestUri:=ssoUrl).ConfigureAwait(False)
                Dim ssoDoc As SsoConfig = Nothing
                Try
                    ssoDoc = JsonSerializer.Deserialize(Of SsoConfig)(json:=ssoJson, options:=JsonExtensions.DeserializationOptions)
                Catch ex As Exception
                    Throw New Exception(message:=$"Failed to parse SSO JSON: {ex.Message}")
                End Try

                If ssoDoc Is Nothing OrElse ssoDoc.Server Is Nothing Then
                    Throw New Exception(message:=$"Invalid SSO JSON for region {serverRegion}")
                End If

                Dim hostname As String = ssoDoc.Server.Hostname
                Dim port As String = ssoDoc.Server.Port.ToString()
                Dim prefix As String = ssoDoc.Server.Prefix
                Dim apiBaseUrl As String =
                    $"https://{hostname}:{port}/{prefix}".TrimEnd(trimChar:="/"c)

                Return New EndpointConfig With {
                    .SsoJson = ssoJson,
                    .ApiBaseUrl = apiBaseUrl}
            End If
        Next

        Throw New Exception(message:=$"Could not find server configuration for region {serverRegion}")
    End Function

    Private Shared Function EscapeKVP(Name As String, value As String) As String
        Return $"{Name}={Uri.EscapeDataString(stringToEscape:=value)}"
    End Function

    ''' <summary>
    ''' Invokes the provided work on the application's UI thread (if an open form exists) and returns the result.
    ''' This ensures COM/STA-bound UI operations execute correctly.
    ''' </summary>
    Private Shared Function InvokeOnUiThreadAsync(Of T)(work As Func(Of T)) As Task(Of T)
        Dim tcs As New TaskCompletionSource(Of T)()

        Try
            If Application.OpenForms IsNot Nothing AndAlso Application.OpenForms.Count > 0 Then
                Dim ctrl As Control = Application.OpenForms(index:=0)
                ctrl.BeginInvoke(method:=New MethodInvoker(
                                             Sub()
                                                 Try
                                                     Dim result As T = work()
                                                     tcs.SetResult(result)
                                                 Catch ex As Exception
                                                     tcs.SetException(ex)
                                                 End Try
                                             End Sub))
            Else
                ' No open forms available; run synchronously on the current thread as a fallback.
                ' This may still fail if not on an STA/UI thread, but in normal app lifetime there is a main form.
                Dim result As T = work()
                tcs.SetResult(result)
            End If
        Catch ex As Exception
            tcs.SetException(ex)
        End Try

        Return tcs.Task
    End Function

    Public Shared Async Function ResolveEndpointConfigAsync(serverRegion As ServerLocation) As Task(Of EndpointConfig)
        ' Use the cached discovery (or fetch it once if missing) and resolve from that.
        Dim discovery As DiscoveryRoot = Await Discover.GetCachedDiscoveryAsync(serverRegion).ConfigureAwait(False)
        Return Await ResolveEndpointConfigFromDiscoveryAsync(discovery, serverRegion).ConfigureAwait(False)
    End Function

End Class
