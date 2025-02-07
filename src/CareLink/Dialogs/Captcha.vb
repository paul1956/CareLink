' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Text.RegularExpressions
Imports Microsoft.Web.WebView2.Core
Imports WebView2.DevTools.Dom

Public Class Captcha
    Private WithEvents DevContext As WebView2DevToolsContext

    Private ReadOnly _ignoredURLs As New List(Of String) From {
        "https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/consent"}

    Private _authTokenValue As String
    Private _autoClick As Boolean
    Private _doCancel As Boolean
    Private _lastUrl As String

    Public Sub New(countryCode As String, password As String, userName As String)
        Me._countryCode = countryCode
        Me._password = password
        Me._userName = userName
    End Sub

    Private Property _countryCode As String
    Private Property _password As String
    Private Property _userName As String
    Public Property Client As CareLinkClient1

    Private Async Sub Captcha_Load(sender As Object, e As EventArgs) Handles Me.Load
        Await Me.EnsureCoreWebView2(Form1.WebViewCacheDirectory)
        Form1.WebViewProcessId = Convert.ToInt32(Me.WebView21.CoreWebView2?.BrowserProcessId)
    End Sub

    Private Async Function EnsureCoreWebView2(webViewCacheDirectory As String) As Task
        If Me.WebView21.Source Is Nothing Then
            Dim task As Task(Of CoreWebView2Environment) = CoreWebView2Environment.CreateAsync(Nothing, webViewCacheDirectory, Nothing)
            Await Me.WebView21.EnsureCoreWebView2Async((Await task))
        End If
        Me.DevContext = Await Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
    End Function

    Private Async Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        Dim foundAuthToken As Boolean = False
        Dim cookies As New CookieContainer()

        If Not e.IsSuccess Then
            If Me.DevContext Is Nothing Then
                Me.DevContext = Await Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
            End If
            Dim errorBodyElement As HtmlBodyElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlBodyElement)(".generic-error")
            If errorBodyElement IsNot Nothing Then
                Dim t As Task = Task.Run(Async Function()
                                             Await Task.Delay(2000)
                                             Me.Invoke(Sub()
                                                           _doCancel = True
                                                           'Me.OK_Button_Click(Nothing, Nothing)
                                                       End Sub
                                             )
                                         End Function
                    )
            End If
            Exit Sub
        End If
        Dim cookieList As List(Of CoreWebView2Cookie) = Await Me.WebView21.CoreWebView2.CookieManager.GetCookiesAsync(_lastUrl)
        For i As Integer = 0 To cookieList.Count - 1
            Dim cookie As CoreWebView2Cookie = Me.WebView21.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList(i).ToSystemNetCookie())

            If cookie.Name.StartsWith("auth") Then
                foundAuthToken = True
                _authTokenValue = cookie.Value
                Me.Visible = False
                _autoClick = True
            End If
            cookies.Add(New Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain))
        Next i
        DebugPrint(_lastUrl)
        If _ignoredURLs.Contains(_lastUrl) Then
            Exit Sub
        End If

        If foundAuthToken Then
            Me.Client = New CareLinkClient1(cookies, s_userName, s_password, s_countryCode)
            Exit Sub
        End If
        If Me.WebView21.Source.ToString.StartsWith("https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login") OrElse Me.WebView21.Source.ToString.StartsWith("https://mdtlogin-ocl.medtronic.com/mmcl/auth/oauth/v2/authorize/login") Then
            Dim t As Task(Of WebView2DevToolsContext) = Nothing
            If Me.DevContext Is Nothing Then
                t = Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
                Await Task.Delay(2000)
                If t Is Nothing OrElse t.IsFaulted Then
                    Exit Sub
                End If
                Me.DevContext = t.Result
            End If
            Dim userNameInputElement As HtmlInputElement = Nothing
            Dim passwordInputElement As HtmlInputElement = Nothing
            Dim loginButtonElement As HtmlInputElement = Nothing
            Dim html As String
            html = Await Me.WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")

            ' The Html comes back with Unicode character codes, other escaped characters, and
            ' wrapped in double quotes, so I'm using this code to clean it up for what I'm doing.
            html = Regex.Unescape(html)
            html = html.Remove(0, 1)
            html = html.Remove(html.Length - 1, 1)
            Try
                userNameInputElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInputElement)("#username")
                passwordInputElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInputElement)("#password")
                loginButtonElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInputElement)("[name=""actionButton""]")
            Catch ex As Exception
            End Try
            If userNameInputElement Is Nothing OrElse passwordInputElement Is Nothing OrElse loginButtonElement Is Nothing Then
                Exit Sub
            End If
            Await userNameInputElement.SetValueAsync(s_userName)
            Await passwordInputElement.SetValueAsync(s_password)

            Dim captchaFrame As HtmlInlineFrameElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInlineFrameElement)("[title=""reCAPTCHA""]")
            Await captchaFrame.PressAsync("Enter")
            Await Task.Delay(2000)

            Dim isCaptchaOpen As Boolean = True

            While isCaptchaOpen
                If _doCancel Then Exit Sub
                Await Task.Delay(250)
                Dim captchaPopupElement() As Element = Await Me.DevContext.XPathAsync("/html/body/div[2]")
                If captchaPopupElement?.Length > 0 Then
                    Dim captchaPopupStyleAttributes As String = Await captchaPopupElement(0).GetAttributeAsync("style")
                    captchaPopupStyleAttributes = captchaPopupStyleAttributes.ToLower.Replace(" ", "")
                    isCaptchaOpen = captchaPopupStyleAttributes.Contains("visibility:visible")
                Else
                    Exit Sub
                End If
            End While
            Await loginButtonElement.ClickElementAsync
            Exit Sub
        End If
        DebugPrint($"Me.WebView21.Source.ToString={ Me.WebView21.Source}")
        If Me.WebView21.Source.ToString.Contains("CareLink.MiniMed.", StringComparison.InvariantCultureIgnoreCase) Then
            Exit Sub
        End If
        Stop
    End Sub

    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles WebView21.NavigationStarting
        _lastUrl = e.Uri
        DebugPrintUrl($"In {NameOf(WebView21_NavigationStarting)} URL", e.Uri, 100)
    End Sub

End Class
