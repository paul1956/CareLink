' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Web.WebView2.Core

Public Class OAuthBrowserForm

    Private Shared ReadOnly s_separator As Char() = New Char() {"&"c}

    Private ReadOnly _password As String
    Private ReadOnly _redirectUri As String

    Private ReadOnly _startUrl As String

    Private ReadOnly _userName As String

    Private _clientSecret As String
    Private _loginFilled As Boolean = False
    Private _state As String

    ''' <summary>
    '''  Initializes a new instance of the <see cref="OAuthBrowserForm"/> class
    '''  with the specified parameters.
    ''' </summary>
    ''' <param name="startUrl">The URL to start the OAuth flow.</param>
    ''' <param name="redirectUri">The redirect URI for the OAuth flow.</param>
    ''' <param name="userName">The username for the login.</param>
    ''' <param name="password">The password for the login.</param>
    '''
    Public Sub New(startUrl As String,
                   redirectUri As String,
                   userName As String,
                   password As String)
        Me.InitializeComponent()
        _startUrl = startUrl
        _redirectUri = redirectUri
        _userName = userName
        _password = password

        Me.Text = "Carelink Login"
        Me.Width = Math.Min(1200, Screen.PrimaryScreen.WorkingArea.Width - 100)
        Me.Height = Math.Min(900, Screen.PrimaryScreen.WorkingArea.Height - 100)
    End Sub

    Public Property Result As RedirectResult

    Private Shared Function GetResponseHeaderValue(headers As CoreWebView2HttpResponseHeaders,
                                                   name As String) As String
        Try
            Return headers.GetHeader(name)
        Catch ex As COMException
            Return String.Empty
        End Try
    End Function

    Private Shared Function HeadersToText(
        headers As CoreWebView2HttpRequestHeaders) As String

        Dim result As New StringBuilder()
        Dim header As KeyValuePair(Of String, String)

        For Each header In headers
            result.AppendLine(value:=$"{header.Key}: {header.Value}")
        Next

        Return result.ToString()
    End Function

    Private Shared Function HeadersToText(headers As CoreWebView2HttpResponseHeaders) As String
        Dim result As New StringBuilder()
        Dim header As KeyValuePair(Of String, String)

        For Each header In headers
            result.AppendLine(value:=$"{header.Key}: {header.Value}")
        Next

        Return result.ToString()
    End Function

    Private Shared Function IsTextResponse(contentType As String) As Boolean
        If String.IsNullOrWhiteSpace(value:=contentType) Then
            Return False
        End If

        ' Remove optional parameters, for example:
        ' "application/json; charset=utf-8" -> "application/json"
        Dim semicolonIndex As Integer = contentType.IndexOf(value:=";"c)

        Dim mediaType As String =
            If(semicolonIndex >= 0,
               contentType.Substring(startIndex:=0, length:=semicolonIndex).Trim(),
               contentType.Trim())

        Const comparisonType As StringComparison = StringComparison.OrdinalIgnoreCase
        Return mediaType.StartsWith(value:="text/", comparisonType) OrElse
               mediaType.Equals(value:="application/json", comparisonType) OrElse
               mediaType.EndsWith(value:="+json", comparisonType) OrElse
               mediaType.Equals(value:="application/xml", comparisonType) OrElse
               mediaType.EndsWith(value:="+xml", comparisonType) OrElse
               mediaType.Equals(value:="application/javascript", comparisonType) OrElse
               mediaType.Equals(value:="application/x-javascript", comparisonType) OrElse
               mediaType.Equals(value:="application/xhtml+xml", comparisonType) OrElse
               mediaType.Equals(value:="application/graphql", comparisonType) OrElse
               mediaType.Equals(value:="application/wasm", comparisonType) OrElse
               mediaType.Equals(value:="application/sql", comparisonType)
    End Function

    Private Shared Function ResponseCanHaveBody(statusCode As Integer) As Boolean
        ' 1xx informational responses, 204 No Content, 205 Reset Content,
        ' and 304 Not Modified do not contain a message body.
        Return statusCode < 100 OrElse
               (statusCode >= 200 AndAlso
                statusCode <> 204 AndAlso
                statusCode <> 205 AndAlso
                statusCode <> 304)
    End Function

    Private Sub CaptureAndClose(uriString As String)
        Dim uri As New Uri(uriString)

        Dim selector As Func(Of String, String()) =
            Function(part)
                Dim separator As Char() = {"="c}
                Return part.Split(separator, count:=2)
            End Function

        Dim elementSelector As Func(Of String(), String) =
            Function(parts)
                Return If(parts.Length > 1,
                          Uri.UnescapeDataString(stringToUnescape:=parts(1)),
                          "")
            End Function

        Dim keySelector As Func(Of String(), String) =
            Function(parts)
                Return Uri.UnescapeDataString(stringToUnescape:=parts(0))
            End Function

        Dim parameters As Dictionary(Of String, String) =
            uri.Query.TrimStart(trimChar:="?"c).
                      Split(separator:="&"c, options:=StringSplitOptions.RemoveEmptyEntries).
                      Select(selector).
                      ToDictionary(keySelector,
                                   elementSelector,
                                   comparer:=StringComparer.OrdinalIgnoreCase)

        Dim code As String = Nothing
        parameters.TryGetValue(key:="code", value:=code)

        parameters.TryGetValue(key:="state", value:=_state)
        Me.Result = New RedirectResult With {
            .Code = code,
            .State = _state}

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Async Sub CoreWebView2_WebResourceResponseReceived(
                        sender As Object,
                        e As CoreWebView2WebResourceResponseReceivedEventArgs)

        Dim request As CoreWebView2WebResourceRequest = e.Request
        Dim response As CoreWebView2WebResourceResponseView = e.Response
        Dim requestHeaders As String = HeadersToText(request.Headers)
        Dim responseHeaders As String = HeadersToText(response.Headers)
        Dim logEntry As New StringBuilder()
        Dim contentType As String =
            GetResponseHeaderValue(response.Headers, name:="Content-Type")
        logEntry.AppendLine($"URI: {request.Uri}")
        logEntry.AppendLine($"Method: {request.Method}")
        logEntry.AppendLine(
            value:=$"Status: {response.StatusCode} {response.ReasonPhrase}")
        logEntry.AppendLine("Request headers:")
        logEntry.AppendLine(requestHeaders)
        logEntry.AppendLine("Response headers:")
        logEntry.AppendLine(responseHeaders)

        If IsTextResponse(contentType) AndAlso
           ResponseCanHaveBody(response.StatusCode) Then

            Try
                Using content As Stream = Await response.GetContentAsync()
                    If content IsNot Nothing Then
                        Using reader As New StreamReader(
                            stream:=content,
                            encoding:=Encoding.UTF8,
                            detectEncodingFromByteOrderMarks:=True)

                            Dim body As String = Await reader.ReadToEndAsync()

                            logEntry.AppendLine(value:="Body:")
                            logEntry.AppendLine(value:=body)
                        End Using
                    Else
                        logEntry.AppendLine(value:="Body: [no content]")
                    End If
                End Using
            Catch ex As COMException
                logEntry.AppendLine(value:=$"Body: [unavailable: {ex.Message}]")
            Catch ex As IOException
                logEntry.AppendLine(value:=$"Body: [read error: {ex.Message}]")
            End Try
        Else
            logEntry.AppendLine(
                value:=$"Body: [not read; Content-Type={contentType}]")
        End If

        Const comparisonType As StringComparison = StringComparison.InvariantCulture

        Dim logEntryAsString As String = logEntry.ToString

        Const separator As String = "/authorize/resume?state="
        If logEntryAsString.Contains(value:=separator, comparisonType) Then
            Dim logEntrySplit As String() = logEntryAsString.Split(separator)
            _state = logEntrySplit(1)
        End If
        If logEntryAsString.Contains(value:="client_secret", comparisonType) Then
            Dim logEntrySplit As String() = logEntryAsString.Split(separator)
            _clientSecret = logEntrySplit(1)
        End If

    End Sub

    Private Async Function FillLoginAsync() As Task
        Await Me.SetFieldAsync(selector:="#username", value:=_userName)
        Await Me.SetFieldAsync(selector:="#password", value:=_password)
    End Function

    Private Async Function InitializeAsync() As Task
        Await Me.WebView21.EnsureCoreWebView2Async()
        With Me.WebView21.CoreWebView2
            AddHandler .NavigationStarting, AddressOf Me.WebView21_NavigationStarting
            AddHandler .NavigationCompleted, AddressOf Me.WebView21_NavigationCompleted
            AddHandler .SourceChanged, AddressOf Me.WebView21_SourceChanged
            AddHandler .WebResourceResponseReceived, AddressOf Me.CoreWebView2_WebResourceResponseReceived
            .Navigate(uri:=_startUrl)
        End With

    End Function

    Private Async Sub OAuthBrowserForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Await Me.InitializeAsync()
    End Sub

    Private Async Function SetFieldAsync(selector As String, value As String) As Task
        Dim javaScript As String =
            $"(() => {{
                const el = document.querySelector({selector.ToJson()});
                if (!el) return false;

                el.focus();
                el.value = {value.ToJson()};

                el.dispatchEvent(new Event('input', {{ bubbles: true }}));
                el.dispatchEvent(new Event('change', {{ bubbles: true }}));
                el.blur();

                return true;
            }})()"

        Dim result As String = Await Me.WebView21.CoreWebView2.ExecuteScriptAsync(javaScript)
    End Function

    Private Async Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)
        Dim currentUrl As String = Me.WebView21.Source?.ToString()
        If IsNullOrWhiteSpace(value:=currentUrl) Then Return

        ' If we've reached the redirect URI, capture result and close.
        If currentUrl.StartsWithNoCase(value:=_redirectUri) Then
            Me.CaptureAndClose(uriString:=currentUrl)
            Return
        End If

        ' Fill the login fields once after the first successful navigation to the login page.
        If Not _loginFilled Then
            _loginFilled = True
            Try
                Await Me.FillLoginAsync()
            Catch
                ' Swallow any errors from attempting to auto-fill; it's non-critical.
            End Try
        End If
    End Sub

    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs)
        If IsNullOrWhiteSpace(value:=e.Uri) Then Return
        If e.Uri.StartsWithNoCase(value:=_redirectUri) Then
            Me.CaptureAndClose(uriString:=e.Uri)
        End If
    End Sub

    Private Sub WebView21_SourceChanged(sender As Object, e As CoreWebView2SourceChangedEventArgs)
        Dim currentUrl As String = Me.WebView21.Source?.ToString()
        If IsNullOrWhiteSpace(value:=currentUrl) Then Return
        If currentUrl.StartsWithNoCase(value:=_redirectUri) Then
            Me.CaptureAndClose(uriString:=currentUrl)
        End If
    End Sub

End Class
