' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions

Public Class CareLinkService
    Private Shared ReadOnly s_http As New HttpClient With {.Timeout = TimeSpan.FromSeconds(120)}
    Public Const DiscoveryUrlEu As String = "https://clcloud.minimed.eu/connect/carepartner/v13/discover/android/3.6"
    Public Const DiscoveryUrlNa As String = "https://clcloud.minimed.com/connect/carepartner/v13/discover/android/3.6"
    Public Const KeySizeInBits As Integer = 2048

    Private Shared Function Base64UrlEncode(bytes As Byte()) As String
        Dim s As String = Convert.ToBase64String(inArray:=bytes)
        s = s.Replace("+", "-").Replace("/", "_").TrimEnd("="c)
        Return s
    End Function

    Private Shared Function CreateCsrPem(cn As String, ou As String, dc As String, o As String, keySizeInBits As Integer) As String
        Using rsa As RSA = RSA.Create(keySizeInBits)
            Dim subjectName As New X500DistinguishedName(distinguishedName:=$"CN={cn},OU={ou},DC={dc},O={o}")
            Dim req As New CertificateRequest(subjectName:=subjectName,
                                              key:=rsa,
                                              hashAlgorithm:=HashAlgorithmName.SHA256,
                                              padding:=RSASignaturePadding.Pkcs1)
            Dim csr As Byte() = req.CreateSigningRequest()
            Dim b64 As String = Convert.ToBase64String(inArray:=csr)
            Dim sb As New StringBuilder()
            sb.AppendLine(value:="-----BEGIN CERTIFICATE REQUEST-----")
            For i As Integer = 0 To b64.Length - 1 Step 64
                sb.AppendLine(value:=b64.Substring(startIndex:=i, length:=Math.Min(64, b64.Length - i)))
            Next
            sb.AppendLine(value:="-----END CERTIFICATE REQUEST-----")
            Return sb.ToString()
        End Using
    End Function

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

    Private Shared Function RandomAndroidModel() As String
        Dim models As String() = {"SM-G973F", "SM-G988U1", "SM-G981W", "SM-G9600"}
        Dim index As Integer = RandomNumberGenerator.GetInt32(toExclusive:=models.Length)
        Return models(index)
    End Function

    Private Shared Function RandomB64String(length As Integer) As String
        Dim bytes As Byte() = New Byte(length + 10 - 1) {}
        RandomNumberGenerator.Fill(data:=bytes)
        Dim s As String = Convert.ToBase64String(inArray:=bytes)
        If s.Length >= length Then Return s.Substring(startIndex:=0, length:=length)
        Return s.PadRight(totalWidth:=length, paddingChar:="A"c)
    End Function

    Private Shared Function RandomDeviceId() As String
        Dim bytes(39) As Byte
        RandomNumberGenerator.Fill(data:=bytes)
        Dim inArray As Byte() = SHA256.HashData(source:=bytes)
        Return Convert.ToHexString(inArray)
    End Function

    Private Shared Function RandomUuidString() As String
        Return Guid.NewGuid().ToString()
    End Function

    Private Shared Function ReformatCsr(csrPem As String) As String
        Dim raw As String = csrPem.Replace("-----BEGIN CERTIFICATE REQUEST-----", "").
            Replace(oldValue:="-----END CERTIFICATE REQUEST-----", newValue:="").
            Replace(oldValue:=vbCr, newValue:="").
            Replace(oldValue:=vbLf, newValue:="").
            Trim()
        Dim bytes As Byte() = Convert.FromBase64String(raw)
        Return Base64UrlEncode(bytes)
    End Function

    ' Note: legacy DoLoginAsync/NonAuth0 paths removed; Auth0 is the supported login flow.

    Public Shared Function ParseRegion(value As String) As String
        Dim v As String = value.Trim().ToUpperInvariant()
        If v = "NORTHAMERICA" OrElse v = "NA" OrElse v = "TRIAL" OrElse v = "TR" Then
            Return "NorthAmerica"
        End If
        If v = "EUROPE" OrElse v = "EU" Then
            Return "Europe"
        End If
        Throw New ArgumentException(message:="Invalid region. Use NorthAmerica/NA, Europe/EU, or Trial/TR.")
    End Function

    Public Shared Async Function ResolveEndpointConfigAsync(discoveryUrl As String, serverRegion As Region) As Task(Of EndpointConfig)
        Dim discoveryJson As String =
            Await s_http.GetStringAsync(requestUri:=discoveryUrl)
        Using discoveryDoc As JsonDocument = JsonDocument.Parse(json:=discoveryJson)
            Dim cp As JsonElement = discoveryDoc.RootElement.GetProperty(propertyName:="CP")
            Dim targetRegion As String

            Select Case serverRegion
                Case Region.NorthAmerica
                    targetRegion = "us"
                Case Region.Europe
                    targetRegion = "eu"
                Case Region.Trial
                    targetRegion = "clinical"
                Case Else
                    Throw New ArgumentException(message:="Invalid server region.", paramName:=NameOf(serverRegion))
            End Select

            For Each c As JsonElement In cp.EnumerateArray()
                Dim regionProp As String = c.GetProperty(propertyName:="region").GetString()

                If regionProp.EqualsNoCase(targetRegion) Then
                    Dim keyName As String = Nothing
                    For Each prop As JsonProperty In c.EnumerateObject()
                        If prop.Name.ContainsNoCase(value:="UseSSOConfiguration") Then
                            ' The property named like "UseSSOConfiguration*" contains the name
                            ' of the actual SSO configuration property. We need the property's
                            ' value (the lookup key), not the property name itself.
                            keyName = prop.Value.GetString()
                            Exit For
                        End If
                    Next

                    If keyName Is Nothing Then
                        Throw New Exception(message:=$"Could not get SSO config url for region {serverRegion}")
                    End If

                    Dim ssoUrl As String = c.GetProperty(propertyName:=keyName).GetString()
                    Dim ssoJson As String = Await s_http.GetStringAsync(requestUri:=ssoUrl)

                    Using ssoDoc As JsonDocument = JsonDocument.Parse(json:=ssoJson)
                        Try
                            Dim server As JsonElement = ssoDoc.RootElement.GetProperty(propertyName:="server")
                            Dim hostname As String = server.GetProperty(propertyName:="hostname").GetString()
                            Dim port As String = server.GetProperty(propertyName:="port").ToString()
                            Dim prefix As String = server.GetProperty(propertyName:="prefix").GetString()
                            Dim apiBaseUrl As String =
                                $"https://{hostname}:{port}/{prefix}".TrimEnd(trimChar:="/"c)

                            Return New EndpointConfig With {
                                .SsoJson = ssoJson,
                                .ApiBaseUrl = apiBaseUrl}
                        Catch ex As Exception
                            Stop
                        End Try
                    End Using
                End If
            Next
        End Using
        Return Nothing
    End Function

End Class
