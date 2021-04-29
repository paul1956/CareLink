''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Text.RegularExpressions

Imports Microsoft.Web.WebView2.Core

Public Class Form1
    Private Const carelinkServerAddress As String = "carelink.minimed.com"

    '<a _ngcontent-gdb-c3="" class="cl-device-warning-continue-btn active"> Continue </a>
    ' This script runs but nothing happens
    Private ReadOnly BrowserAcceptScript = <script>
try {
    document.getElementsByClassName("cl-device-warning-continue-btn")[0].click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    '<a _ngcontent-qek-c7="" class="cl-sidebar-link ng-star-inserted" id="h-connect" routerlink="/connect" href="/app/connect" style=""><img _ngcontent-qek-c7="" alt="connect" class="cl-menu-icon" src="assets/img/icons/connect-icon.png"><span _ngcontent-qek-c7="" class="cl-sidebar-link-text">Connect</span></a>
    Private ReadOnly connectScript = <script>
try {
    document.getElementById('h-connect').click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    '    <a _ngcontent-gsk-c5="" class="cl-landing-login-button" tabindex="-1" href="patient/sso/login?country=us&amp;lang=en"> Continue </a>
    Private ReadOnly continueScript = <script>
try {
    document.getElementsByClassName("cl-landing-login-button")[0].click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    Private ReadOnly loginScript = <script>
try {
    // set username
    document.getElementsByName("username")[0].value = "MyUserID";
    // set password...
    document.getElementsByName("password")[0].value = "MyPassword";
        // 'click' the submit button
    document.getElementById('form-login-en').submit();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    Private initializized As Boolean = False
    Private loginSuccessfully As Boolean = False

    Private Shared Sub WebView21_ContentLoading(sender As Object, e As CoreWebView2ContentLoadingEventArgs) Handles WebView21.ContentLoading
        Debug.Print($"Is Error Page = {e.IsErrorPage}")
    End Sub

    Private Shared Sub WebView21_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs) Handles WebView21.WebMessageReceived
        Debug.Print($"Web Message As Json = {e.WebMessageAsJson}, URL = {e.Source}")
    End Sub

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Await WebView21.EnsureCoreWebView2Async()
        AddHandler WebView21.CoreWebView2.DOMContentLoaded, AddressOf WebView_CoreWebView2_DomContentLoaded
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        WebView21.Size = ClientSize - New Size(WebView21.Location)
    End Sub

    Private Async Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim htmlToParse As String = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
        htmlToParse = Regex.Unescape(htmlToParse)
        htmlToParse = htmlToParse.Remove(0, 1)
        Dim parsedHtml As String = htmlToParse.Remove(htmlToParse.Length - 1, 1)
        Dim i As Integer = parsedHtml.IndexOf("<div class=""sensor-value""", StringComparison.InvariantCulture)
        If i >= 0 Then
            i = parsedHtml.IndexOf(">", i + 1, StringComparison.InvariantCulture)
            i += 1
            Dim lessThanIndex As Integer = parsedHtml.IndexOf("<", i, StringComparison.InvariantCulture)
            CurrentBGToolStripTextBox.Text = parsedHtml.Substring(i, lessThanIndex - i)
        End If
    End Sub

    Private Sub UpdateAddressBar(sender As Object, args As CoreWebView2WebMessageReceivedEventArgs)
        Dim uri As String = args.TryGetWebMessageAsString()
        AddressBar.Text = uri
        WebView21.CoreWebView2.PostWebMessageAsString(uri)
    End Sub

    Private Sub WebView_CoreWebView2_DomContentLoaded(sender As Object, e As CoreWebView2DOMContentLoadedEventArgs)
        Stop
    End Sub

    Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs) Handles WebView21.CoreWebView2InitializationCompleted
        InitializeAsync()
    End Sub

    Private Async Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        Debug.Print($"Web Error Status = {e.WebErrorStatus}")
        If e.IsSuccess Then
            If AddressBar.Text = $"https://{carelinkServerAddress}/" Then
                Dim htmlToParse As String = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
                htmlToParse = Regex.Unescape(htmlToParse)
                htmlToParse = htmlToParse.Remove(0, 1)
                Dim parsedHtml As String = htmlToParse.Remove(htmlToParse.Length - 1, 1)
                If parsedHtml.Contains("mat-checkbox-1-input") Then
                    Await WebView21.ExecuteScriptAsync(BrowserAcceptScript)
                    AddressBar.Text = $"https://{carelinkServerAddress}/app/login"
                    Application.DoEvents()
                End If
            End If

            If AddressBar.Text = $"https://{carelinkServerAddress}/app/login" Then
                Dim htmlToParse As String = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
                htmlToParse = Regex.Unescape(htmlToParse)
                htmlToParse = htmlToParse.Remove(0, 1)
                Dim parsedHtml As String = htmlToParse.Remove(htmlToParse.Length - 1, 1)
                ' <a _ngcontent-rme-c5="" class="cl-landing-login-button" tabindex="-1" href="patient/sso/login?country=us&amp;lang=en"> Continue </a>
                Dim indexOfLanding As Integer = parsedHtml.IndexOf("class=""cl-landing-login-button""", StringComparison.CurrentCultureIgnoreCase)
                If indexOfLanding > 0 Then
                    Dim indexOfContinue As Integer = parsedHtml.Substring(indexOfLanding).IndexOf("Continue", StringComparison.CurrentCultureIgnoreCase)
                    If indexOfContinue > 0 Then
                        Await WebView21.ExecuteScriptAsync(continueScript)
                        Exit Sub
                    End If
                End If
            End If
            If AddressBar.Text.Contains($"https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login?action=display") Then
                ' Get UserName and password from user
                ' This is only for testing
                Dim userName As String = InputBox("Enter User Name")
                Dim password As String = InputBox("Enter Password")
                Await WebView21.ExecuteScriptAsync(loginScript.ToString().Replace("MyUserID", userName).Replace("MyPassword", password))
            End If
            If AddressBar.Text = $"https://{carelinkServerAddress}/app/home" Then
                Await WebView21.ExecuteScriptAsync(connectScript.ToString())
                Timer1.Interval = 10000
                Timer1.Enabled = True
            End If
        Else
            Stop
        End If

    End Sub

    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles WebView21.NavigationStarting
        InitializeAsync()
        AddressBar.Text = e.Uri
        If e.Uri = "https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/consent" Then
            loginSuccessfully = True
        ElseIf e.Uri = $"https://{carelinkServerAddress}/" AndAlso loginSuccessfully Then
            AddressBar.Text = $"https://{carelinkServerAddress}/app/home"
        End If
    End Sub

    Private Sub WebView21_SourceChanged(sender As Object, e As CoreWebView2SourceChangedEventArgs) Handles WebView21.SourceChanged
        Debug.Print($"Is New Document = {e.IsNewDocument}")
    End Sub

    Public Async Sub InitializeAsync()
        Await WebView21.EnsureCoreWebView2Async(Nothing)
        If Not initializized Then
            initializized = True
            AddHandler WebView21.CoreWebView2.WebMessageReceived, AddressOf UpdateAddressBar
        End If
    End Sub

End Class
