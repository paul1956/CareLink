' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports Microsoft.Web.WebView2.Core

Public Class OAuthBrowserForm

    Private ReadOnly _password As String
    Private ReadOnly _startUrl As String
    Private ReadOnly _userName As String
    Private ReadOnly _redirectUri As String
    Public Property Result As RedirectResult
    Private Shared ReadOnly s_separator As Char() = New Char() {"&"c}
    Private _loginFilled As Boolean = False

    ''' <summary>
    '''  Initializes a new instance of the <see cref="OAuthBrowserForm"/> class
    '''  with the specified parameters.
    ''' </summary>
    ''' <param name="startUrl">The URL to start the OAuth flow.</param>
    ''' <param name="redirectUri">The redirect URI for the OAuth flow.</param>
    ''' <param name="userName">The username for the login.</param>
    ''' <param name="password">The password for the login.</param>
    Public Sub New(startUrl As String, redirectUri As String, userName As String, password As String)
        Me.InitializeComponent()
        _startUrl = startUrl
        _redirectUri = redirectUri
        _userName = userName
        _password = password

        Me.Text = "Carelink Login"
        Me.Width = Math.Min(1200, Screen.PrimaryScreen.WorkingArea.Width - 100)
        Me.Height = Math.Min(900, Screen.PrimaryScreen.WorkingArea.Height - 100)
    End Sub

    Private Sub CaptureAndClose(uriString As String)
        Dim uri As New Uri(uriString)

        Dim queryString As String = If(uri.Query, String.Empty)
        If queryString.StartsWithNoCase(value:="?"c) Then
            queryString = queryString.Substring(startIndex:=1)
        End If

        Dim pairs As String() = If(String.IsNullOrEmpty(value:=queryString),
                                   Array.Empty(Of String)(),
                                   queryString.Split(s_separator, options:=StringSplitOptions.RemoveEmptyEntries))
        Dim dict As New Dictionary(Of String, String)(comparer:=StringComparer.OrdinalIgnoreCase)

        For Each pair As String In pairs
            Dim idx As Integer = pair.IndexOf(value:="="c)
            Dim key As String
            Dim value As String
            If idx >= 0 Then
                key = Uri.UnescapeDataString(charsToUnescape:=pair.AsSpan(start:=0, length:=idx))
                value = Uri.UnescapeDataString(charsToUnescape:=pair.AsSpan(start:=idx + 1))
            Else
                key = Uri.UnescapeDataString(stringToUnescape:=pair)
                value = String.Empty
            End If
            dict(key) = value
        Next

        Dim code As String = Nothing
        Dim state As String = Nothing
        dict.TryGetValue(key:="code", value:=code)
        dict.TryGetValue(key:="state", value:=state)

        Me.Result = New RedirectResult With {
            .Code = code,
            .State = state
        }

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Async Function FillLoginAsync() As Task
        Await Me.SetFieldAsync(selector:="#username", value:=_userName)
        Await Me.SetFieldAsync(selector:="#password", value:=_password)
    End Function

    Private Async Function InitializeAsync() As Task
        Await Me.WebView21.EnsureCoreWebView2Async()

        AddHandler Me.WebView21.CoreWebView2.NavigationStarting, AddressOf Me.WebView21_NavigationStarting
        AddHandler Me.WebView21.CoreWebView2.NavigationCompleted, AddressOf Me.WebView21_NavigationCompleted
        AddHandler Me.WebView21.CoreWebView2.SourceChanged, AddressOf Me.WebView21_SourceChanged

        Me.WebView21.CoreWebView2.Navigate(uri:=_startUrl)
    End Function

    Private Async Sub OAuthBrowserForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Await Me.InitializeAsync()
    End Sub


    Private Async Function SetFieldAsync(selector As String, value As String) As Task
        Dim javaScript As String =
        $"(() => {{
            const el = document.querySelector({JsonSerializer.Serialize(value:=selector)});
            if (!el) return false;
            el.focus();
            el.value = {JsonSerializer.Serialize(value)};
            el.dispatchEvent(new Event('input', {{ bubbles: true }}));
            el.dispatchEvent(new Event('change', {{ bubbles: true }}));
            return true;
        }})()"

        Await Me.WebView21.CoreWebView2.ExecuteScriptAsync(javaScript)
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
