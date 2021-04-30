''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Imports HtmlAgilityPack

Imports Microsoft.Web.WebView2.Core

Public Class Form1
    Private Const carelinkServerAddress As String = "carelink.minimed.com"

    '<a _ngcontent-gdb-c3="" class="cl-device-warning-continue-btn active"> Continue </a>
    ' This script runs but nothing happens
    Private ReadOnly BrowserAcceptScript As String = <script>
try {
    document.getElementsByClassName("cl-device-warning-continue-btn")[0].click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    '<a _ngcontent-qek-c7="" class="cl-sidebar-link ng-star-inserted" id="h-connect" routerlink="/connect" href="/app/connect" style=""><img _ngcontent-qek-c7="" alt="connect" class="cl-menu-icon" src="assets/img/icons/connect-icon.png"><span _ngcontent-qek-c7="" class="cl-sidebar-link-text">Connect</span></a>
    Private ReadOnly connectScript As String = <script>
try {
    document.getElementById('h-connect').click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    '    <a _ngcontent-gsk-c5="" class="cl-landing-login-button" tabindex="-1" href="patient/sso/login?country=us&amp;lang=en"> Continue </a>
    Private ReadOnly continueScript As String = <script>
try {
    document.getElementsByClassName("cl-landing-login-button")[0].click();
    }
catch(err) {
    alert(err.message);
}
                 </script>.Value

    Private ReadOnly loginScript As String = <script>
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

    Private loginSuccessfully As Boolean = False

    Private Shared Sub WebView21_ContentLoading(sender As Object, e As CoreWebView2ContentLoadingEventArgs) Handles WebView21.ContentLoading
        Debug.Print($"Is Error Page = {e.IsErrorPage}")
    End Sub

    Private Shared Sub WebView21_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs) Handles WebView21.WebMessageReceived
        Debug.Print($"Web Message As Json = {e.WebMessageAsJson}, URL = {e.Source}")
    End Sub

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClientSize = New Size(CInt(Screen.PrimaryScreen.WorkingArea.Width * 0.9), CType(Screen.PrimaryScreen.WorkingArea.Height * 0.9, Integer))
        Await WebView21.EnsureCoreWebView2Async()
        AddHandler WebView21.CoreWebView2.DOMContentLoaded, AddressOf WebView_CoreWebView2_DomContentLoaded
        AddHandler WebView21.CoreWebView2.WebMessageReceived, AddressOf UpdateAddressBar
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim halfWidth As Integer = ClientSize.Width \ 2
        WebView21.Size = ClientSize - New Size(halfWidth, WebView21.Location.Y)
        RichTextBox1.Width = halfWidth
        RichTextBox1.Height = ClientSize.Height
        RichTextBox1.Left = halfWidth
    End Sub

    Private Function parseHTML(htmlToParse As String) As String
        htmlToParse = Regex.Unescape(htmlToParse)
        htmlToParse = htmlToParse.Remove(0, 1)
        htmlToParse = htmlToParse.Remove(htmlToParse.Length - 1, 1)
        Dim htmlDoc As New HtmlDocument With {
            .OptionFixNestedTags = True
        }
        htmlDoc.LoadHtml(htmlToParse)

        If htmlDoc.DocumentNode.HasChildNodes Then
            Dim wrapperNeeded As Boolean = htmlDoc.DocumentNode.ChildNodes.Cast(Of HtmlNode)().Any(Function(n) n.NodeType <> HtmlNodeType.Element)

            If wrapperNeeded Then
                Dim wrapper As HtmlNode = htmlDoc.CreateElement("div")
                wrapper.AppendChildren(htmlDoc.DocumentNode.ChildNodes)
                htmlDoc.DocumentNode.RemoveAllChildren()
                htmlDoc.DocumentNode.AppendChild(wrapper)
            End If
        End If
        Dim html As String
        Using writer As New StringWriter()
            htmlDoc.Save(writer)
            html = writer.ToString()
        End Using
        html = html.Replace("&quot;", """")
        html = html.Replace("><", $">{vbCrLf}<")
        RichTextBox1.Text = html.Replace(vbLf, vbCrLf)
        Return htmlToParse
    End Function

    Private Async Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;"))
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
        'Stop
    End Sub

    Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs) Handles WebView21.CoreWebView2InitializationCompleted
        InitializeAsync()
    End Sub

    Private Async Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        Thread.Sleep(2000)
        Application.DoEvents()
        Debug.Print($"Web Error Status = {e.WebErrorStatus}")
        If e.IsSuccess Then
            If AddressBar.Text = $"https://{carelinkServerAddress}/" Then
                Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;"))
                If parsedHtml.Contains("mat-checkbox-1-input") Then
                    Await WebView21.ExecuteScriptAsync(BrowserAcceptScript)
                    AddressBar.Text = $"https://{carelinkServerAddress}/app/login"
                    Application.DoEvents()
                End If
            End If

            If AddressBar.Text = $"https://{carelinkServerAddress}/app/login" Then
                Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;"))
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
                Using loginDialog As New LoginForm1
                    loginDialog.ShowDialog()
                    Await WebView21.ExecuteScriptAsync(loginScript.ToString().Replace("MyUserID", loginDialog.UserName).Replace("MyPassword", loginDialog.Password))
                End Using
            End If
            If AddressBar.Text = $"https://{carelinkServerAddress}/app/home" Then
                Await WebView21.ExecuteScriptAsync(connectScript)
                Timer1.Interval = 5000
                Timer1.Enabled = True
            End If
        Else
            Stop
        End If

    End Sub

    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles WebView21.NavigationStarting
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
    End Sub

End Class
