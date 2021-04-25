''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Text.RegularExpressions

Imports Microsoft.Web.WebView2.Core

Public Class Form1
    Private initializized As Boolean = False
    Private Const carelinkServerAddress As String = "carelink.minimed.com"
    Private ReadOnly deviceWarning As String = $"https://{carelinkServerAddress}/app/device-warning"

    '<label class="mat-checkbox-layout" for="mat-checkbox-1-input"><div class="mat-checkbox-inner-container"><input class="mat-checkbox-input cdk-visually-hidden" type="checkbox" id="mat-checkbox-1-input" tabindex="0" aria-checked="false"><div class="mat-checkbox-ripple mat-ripple" matripple=""><div class="mat-ripple-element mat-checkbox-persistent-ripple"></div></div><div class="mat-checkbox-frame"></div><div class="mat-checkbox-background"><svg xml:space="preserve" class="mat-checkbox-checkmark" focusable="false" version="1.1" viewBox="0 0 24 24"><path class="mat-checkbox-checkmark-path" d="M4.1,12.7 9,17.6 20.3,6.3" fill="none" stroke="white"></path></svg><div class="mat-checkbox-mixedmark"></div></div></div><span class="mat-checkbox-label"><span style="display:none">&nbsp;</span> Don`t ask me again </span></label>
    Private ReadOnly BrowserAcceptScript = <script>
try {
    document.getElementsByName("mat-checkbox-1-input").checked=true;
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    '    <a _ngcontent-gsk-c5="" class="cl-landing-login-button" tabindex="-1" href="patient/sso/login?country=us&amp;lang=en"> Continue </a>
    Private ReadOnly continueScript = <script>
try {
    document.getElementsByName("cl-landing-login-button")[0].click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    Private ReadOnly loginScript = <script>
try {
    // set username
    document.getElementsByName("login")[0].value = "MyUserID";
    // set password...
    document.getElementsByName("password")[0].value = "MyPassword";
        // 'click' the submit button
    document.getElementsByName("B1")[0].click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

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

    Private Sub StartDisplayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartDisplayToolStripMenuItem.Click
        Timer1.Interval = 10000
        Timer1.Enabled = True
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
            If AddressBar.Text = $"https://{carelinkServerAddress}/app/login" Then

                '-----> HANGS Here

                Dim htmlToParse As String = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
                htmlToParse = Regex.Unescape(htmlToParse)
                htmlToParse = htmlToParse.Remove(0, 1)
                Dim parsedHtml As String = htmlToParse.Remove(htmlToParse.Length - 1, 1)
                If parsedHtml.Contains("mat-checkbox-1-input") Then
                    Await WebView21.ExecuteScriptAsync(BrowserAcceptScript)
                End If
            End If
            Stop
        Else
            Stop
        End If

    End Sub

    Private Async Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles WebView21.NavigationStarting
        InitializeAsync()
        AddressBar.Text = e.Uri
        If e.Uri.Contains($"https://{carelinkServerAddress}/patient/sso/login?country=us&lang=en") Then
            Await WebView21.ExecuteScriptAsync(continueScript)
        ElseIf e.Uri.Contains($"https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login?action=display") Then
            ' Get UserName and password from user
            ' This is only for testing
            Dim userName As String = InputBox("Enter User Name")
            Dim password As String = InputBox("Enter Password")
            Await WebView21.ExecuteScriptAsync(loginScript.ToString().Replace("MyUserID", userName).Replace("MyPassword", password))
        Else
            Stop
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
