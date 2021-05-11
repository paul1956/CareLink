''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Imports HtmlAgilityPack

Imports Microsoft.Web.WebView2.Core

Public Class Form1
    Public Client As CareLinkClient
    Public RecentData As Object

    '    Private Const iFrameScript As String = "
    'function getIframeWindow(iframe_object) {
    '  var doc;

    '  if (iframe_object.contentWindow) {
    '    return iframe_object.contentWindow;
    '  }

    '  if (iframe_object.window) {
    '    return iframe_object.window;
    '  }

    '  if (!doc && iframe_object.contentDocument) {
    '    doc = iframe_object.contentDocument;
    '  }

    '  if (!doc && iframe_object.document) {
    '    doc = iframe_object.document;
    '  }

    '  if (doc && doc.defaultView) {
    '   return doc.defaultView;
    '  }

    '  if (doc && doc.parentWindow) {
    '    return doc.parentWindow;
    '  }

    '  return undefined;
    '}
    'try {
    '    // alert(""Starting : iFrameScript"");
    '    // var frameObj = getIframeWindow('connect_frame');
    '    // alert(""Got : frameObj"");
    '    // var frameContent = frameObj.body.innerHTML;
    '    alert(""frame content : "" + frameContent);
    '    return document.getElementsByTagName(""iframe"")[0].contentDocument.documentElement.outerHTML;
    '    }
    'catch(err) {
    '    alert(err.message);
    '}"

    '    '<a _ngcontent-gdb-c3="" class="cl-device-warning-continue-btn active"> Continue </a>
    '    ' This script runs but nothing happens
    '    Private ReadOnly BrowserAcceptScript As String = <script>
    'try {
    '    document.getElementsByClassName("cl-device-warning-continue-btn")[0].click();
    '    }
    'catch(err) {
    '    alert(err.message);
    '}
    '                 </script>.Value

    '    '<a _ngcontent-qek-c7="" class="cl-sidebar-link ng-star-inserted" id="h-connect" routerlink="/connect" href="/app/connect" style=""><img _ngcontent-qek-c7="" alt="connect" class="cl-menu-icon" src="assets/img/icons/connect-icon.png"><span _ngcontent-qek-c7="" class="cl-sidebar-link-text">Connect</span></a>
    '    Private ReadOnly connectScript As String = <script>
    'try {
    '    document.getElementById('h-connect').click();
    '    }
    'catch(err) {
    '    alert(err.message);
    '}
    '                 </script>.Value

    '    '    <a _ngcontent-gsk-c5="" class="cl-landing-login-button" tabindex="-1" href="patient/sso/login?country=us&amp;lang=en"> Continue </a>
    '    Private ReadOnly continueScript As String = <script>
    'try {
    '    document.getElementsByClassName("cl-landing-login-button")[0].click();
    '    }
    'catch(err) {
    '    alert(err.message);
    '}
    '                 </script>.Value

    '    Private ReadOnly loginScript As String = <script>
    'try {
    '    // set username
    '    document.getElementsByName("username")[0].value = "MyUserID";
    '    // set password...
    '    document.getElementsByName("password")[0].value = "MyPassword";
    '        // 'click' the submit button
    '    document.getElementById('form-login-en').submit();
    '    }
    'catch(err) {
    '    alert(err.message);
    '}
    '                 </script>.Value

    '    Private loginSuccessfully As Boolean = False

    '    Private Shared Sub WebView21_ContentLoading(sender As Object, e As CoreWebView2ContentLoadingEventArgs) Handles WebView21.ContentLoading
    '        Diagnostics.Debug.Print($"Is Error Page = {e.IsErrorPage}")
    '    End Sub

    '    Private Shared Sub WebView21_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs) Handles WebView21.WebMessageReceived
    '        Diagnostics.Debug.Print($"Web Message As Json = {e.WebMessageAsJson}, URL = {e.Source}")
    '    End Sub

    '    Private Sub CoreWebView2_DocumentTitleChanged(sender As Object, e As Object)
    '        Text = WebView21.CoreWebView2.DocumentTitle
    '        UpdateTitleWithEvent("DocumentTitleChanged")
    '    End Sub

    '    Private Sub CoreWebView2_HistoryChanged(sender As Object, e As Object)
    '        ' No explicit check for webView2Control initialization because the events can only start
    '        ' firing after the CoreWebView2 and its events exist for us to subscribe.
    '        'btnBack.Enabled = webView2Control.CoreWebView2.CanGoBack
    '        'btnForward.Enabled = webView2Control.CoreWebView2.CanGoForward
    '        UpdateTitleWithEvent("HistoryChanged")
    '    End Sub

    '    Private Sub CoreWebView2_SourceChanged(sender As Object, e As CoreWebView2SourceChangedEventArgs)
    '        AddressBar.Text = WebView21.Source.AbsoluteUri
    '        UpdateTitleWithEvent("SourceChanged")
    '    End Sub

    '    Private Sub FindNext_Click(sender As Object, e As EventArgs) Handles FindNext.Click
    '        Static foundIndex As Integer = Math.Max(RichTextBox1.SelectionStart, 0)
    '        If FindWhat.Text.Length > 0 Then
    '            'find the text that need to be highlighted.
    '            foundIndex = RichTextBox1.Find(FindWhat.Text, foundIndex + 1, -1, RichTextBoxFinds.None)
    '            RichTextBox1.Focus()

    '            If foundIndex = -1 Then
    '                MessageBox.Show($"This document don't contains the text you typed, or any of the text you typed as a whole word or mach case.", $"Find Text Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
    '            Else
    '                'now the text will be highlighted.
    '                RichTextBox1.SelectionBackColor = Color.Yellow
    '                RichTextBox1.Focus()
    '            End If
    '        End If

    '    End Sub

    'Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
    'ClientSize = New Size(CInt(Screen.PrimaryScreen.WorkingArea.Width * 0.9), CType(Screen.PrimaryScreen.WorkingArea.Height * 0.9, Integer))
    'Await WebView21.EnsureCoreWebView2Async()
    'AddHandler WebView21.CoreWebView2.DOMContentLoaded, AddressOf WebView_CoreWebView2_DomContentLoaded
    'AddHandler WebView21.CoreWebView2.WebMessageReceived, AddressOf UpdateAddressBar
    'End Sub

    'Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    '    Dim halfWidth As Integer = ClientSize.Width \ 2
    '    'WebView21.Size = ClientSize - New Size(halfWidth, WebView21.Location.Y)
    '    'RichTextBox1.Top = WebView21.Top
    '    RichTextBox1.Width = halfWidth
    '    RichTextBox1.Height = ClientSize.Height - 50
    '    RichTextBox1.Left = halfWidth
    'End Sub

    'Private Function parseHTML(htmlToParse As String) As String
    '    If htmlToParse Is Nothing Then
    '        Return "Document Null"
    '    End If
    '    htmlToParse = Regex.Unescape(htmlToParse)
    '    htmlToParse = htmlToParse.Remove(0, 1)
    '    htmlToParse = htmlToParse.Remove(htmlToParse.Length - 1, 1)
    '    htmlToParse = htmlToParse.Replace(vbLf, vbCrLf)
    '    htmlToParse = htmlToParse.Replace("&quot;", """")
    '    Dim htmlDoc As New HtmlDocument()
    '    htmlDoc.LoadHtml(htmlToParse)
    '    Dim formattedHtml As New StringBuilder

    '    For Each node As HtmlNode In htmlDoc.DocumentNode.ChildNodes
    '        WriteNode(formattedHtml, node, 0)
    '    Next node
    '    RichTextBox1.Text = formattedHtml.ToString()

    '    Return htmlToParse
    'End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync(iFrameScript))
        'Dim i As Integer = parsedHtml.IndexOf("<div class=""sensor-value""", StringComparison.InvariantCulture)
        'If i >= 0 Then
        '    i = parsedHtml.IndexOf(">", i + 1, StringComparison.InvariantCulture)
        '    i += 1
        '    Dim lessThanIndex As Integer = parsedHtml.IndexOf("<", i, StringComparison.InvariantCulture)
        '    CurrentBGToolStripTextBox.Text = parsedHtml.Substring(i, lessThanIndex - i)
        'End If
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Using loginDialog As New LoginForm1
            loginDialog.ShowDialog()
            Client = New CareLinkClient(loginDialog.UserName, loginDialog.Password, "en")
        End Using

        RecentData = Client.getRecentData()
    End Sub

    'Private Sub UpdateAddressBar(sender As Object, args As CoreWebView2WebMessageReceivedEventArgs)
    '    Dim uri As String = args.TryGetWebMessageAsString()
    '    AddressBar.Text = uri
    '    WebView21.CoreWebView2.PostWebMessageAsString(uri)
    'End Sub

    'Private Sub UpdateTitleWithEvent(message As String)
    '    Dim currentDocumentTitle As String = If(WebView21?.CoreWebView2?.DocumentTitle, "Uninitialized")
    '    Text = $"{currentDocumentTitle} ({message})"
    'End Sub

    'Private Sub WebView_CoreWebView2_DomContentLoaded(sender As Object, e As CoreWebView2DOMContentLoadedEventArgs)
    '    'Stop
    'End Sub

    'Private Async Sub WebView2_FrameNavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)
    '    Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync(iFrameScript))
    '    Dim i As Integer = parsedHtml.IndexOf("<div class=""sensor-value""", StringComparison.InvariantCulture)
    '    If i >= 0 Then
    '        i = parsedHtml.IndexOf(">", i + 1, StringComparison.InvariantCulture)
    '        i += 1
    '        Dim lessThanIndex As Integer = parsedHtml.IndexOf("<", i, StringComparison.InvariantCulture)
    '        CurrentBGToolStripTextBox.Text = parsedHtml.Substring(i, lessThanIndex - i)
    '    End If
    'End Sub

    'Private Sub WebView2_FrameNavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs)
    '    If e.Uri = $"https://{carelinkServerAddress}/assets/dummy/connect/ble/connect.html" Then
    '        'WebView21.Source = New Uri($"https://{carelinkServerAddress}/assets/dummy/connect/ble/connect.html?data-patient=&quot;paulmcohen&quot;,data-role=&quot;PATIENT&quot;,data-language=&quot;en&quot;")
    '        'e.Cancel = True
    '        'Timer1.Interval = 50000
    '        'Timer1.Enabled = True
    '    End If
    'End Sub

    'Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs)
    '    InitializeAsync()
    '    If Not e.IsSuccess Then
    '        MessageBox.Show($"WebView2 creation failed with exception = {e.InitializationException}")
    '        UpdateTitleWithEvent("CoreWebView2InitializationCompleted failed")
    '        Return
    '    End If

    '    AddHandler WebView21.CoreWebView2.SourceChanged, AddressOf CoreWebView2_SourceChanged
    '    AddHandler WebView21.CoreWebView2.HistoryChanged, AddressOf CoreWebView2_HistoryChanged
    '    AddHandler WebView21.CoreWebView2.DocumentTitleChanged, AddressOf CoreWebView2_DocumentTitleChanged
    '    AddHandler WebView21.CoreWebView2.FrameNavigationCompleted, AddressOf WebView2_FrameNavigationCompleted
    '    AddHandler WebView21.CoreWebView2.FrameNavigationStarting, AddressOf WebView2_FrameNavigationStarting
    '    UpdateTitleWithEvent("CoreWebView2InitializationCompleted succeeded")
    'End Sub

    'Private Async Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)
    '    Thread.Sleep(2000)
    '    Application.DoEvents()
    '    Diagnostics.Debug.Print($"Web Error Status = {e.WebErrorStatus}")
    '    If e.IsSuccess Then
    '        If AddressBar.Text = $"https://{carelinkServerAddress}/" Then
    '            Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;"))
    '            If parsedHtml.Contains("mat-checkbox-1-input") Then
    '                Await WebView21.ExecuteScriptAsync(BrowserAcceptScript)
    '                AddressBar.Text = $"https://{carelinkServerAddress}/app/login"
    '                Application.DoEvents()
    '            End If
    '            '<h2 _ngcontent-fiy-c9="" class="cl-dashboard-shared-title" id="h-welcome-title"> Welcome back, <span _ngcontent-fiy-c9="">Paul!</span></h2>
    '            If parsedHtml.Contains(" Welcome back, ") Then
    '                AddressBar.Text = $"https://{carelinkServerAddress}/app/home"
    '            End If
    '        End If

    '        If AddressBar.Text = $"https://{carelinkServerAddress}/app/login" Then
    '            Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;"))
    '            ' <a _ngcontent-rme-c5="" class="cl-landing-login-button" tabindex="-1" href="patient/sso/login?country=us&amp;lang=en"> Continue </a>
    '            Dim indexOfLanding As Integer = parsedHtml.IndexOf("class=""cl-landing-login-button""", StringComparison.CurrentCultureIgnoreCase)
    '            If indexOfLanding > 0 Then
    '                Dim indexOfContinue As Integer = parsedHtml.Substring(indexOfLanding).IndexOf("Continue", StringComparison.CurrentCultureIgnoreCase)
    '                If indexOfContinue > 0 Then
    '                    Await WebView21.ExecuteScriptAsync(continueScript)
    '                    Exit Sub
    '                End If
    '            End If
    '        End If
    '        If AddressBar.Text.Contains($"https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login?action=display") Then
    '            Using loginDialog As New LoginForm1
    '                loginDialog.ShowDialog()
    '                Await WebView21.ExecuteScriptAsync(loginScript.ToString().Replace("MyUserID", loginDialog.UserName).Replace("MyPassword", loginDialog.Password))
    '            End Using
    '        End If
    '        If AddressBar.Text = $"https://{carelinkServerAddress}/app/home" Then
    '            Await WebView21.ExecuteScriptAsync(connectScript)
    '            Dim parsedHtml As String = parseHTML(Await WebView21.ExecuteScriptAsync(iFrameScript))
    '            Dim i As Integer = parsedHtml.IndexOf("<div class=""sensor-value""", StringComparison.InvariantCulture)
    '            If i >= 0 Then
    '                i = parsedHtml.IndexOf(">", i + 1, StringComparison.InvariantCulture)
    '                i += 1
    '                Dim lessThanIndex As Integer = parsedHtml.IndexOf("<", i, StringComparison.InvariantCulture)
    '                CurrentBGToolStripTextBox.Text = parsedHtml.Substring(i, lessThanIndex - i)
    '            End If
    '        End If
    '    Else
    '        Stop
    '    End If

    'End Sub

    'Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs)
    '    AddressBar.Text = e.Uri
    '    If e.Uri = "https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/consent" Then
    '        loginSuccessfully = True
    '    ElseIf e.Uri = $"https://{carelinkServerAddress}/" AndAlso loginSuccessfully Then
    '        AddressBar.Text = $"https://{carelinkServerAddress}/app/home"
    '    End If
    'End Sub

    'Private Sub WebView21_SourceChanged(sender As Object, e As CoreWebView2SourceChangedEventArgs)
    '    Diagnostics.Debug.Print($"Is New Document = {e.IsNewDocument}")
    'End Sub

    'Private Sub WriteNode(_file As StringBuilder, _node As HtmlNode, _indentLevel As Integer)
    '    ' check parameter
    '    If _file Is Nothing Then
    '        Return
    '    End If
    '    If _node Is Nothing Then
    '        Return
    '    End If

    '    ' case: no children

    '    If _node.HasChildNodes = False Then
    '        Dim outerHtml As String = _node.OuterHtml.Replace(vbCrLf, "").Trim
    '        If outerHtml.Length > 0 Then
    '            _file.Append(Space(_indentLevel * 4))
    '            _file.Append(outerHtml)
    '            _file.Append(Environment.NewLine)
    '        End If
    '    Else
    '        If _node.Name = "style" Then
    '            Exit Sub
    '        End If
    '        ' case: node has children
    '        ' indent
    '        _file.Append(Space(_indentLevel * 4))

    '        ' open tag
    '        _file.Append($"<{_node.Name} ")
    '        If _node.HasAttributes Then
    '            For Each attr As HtmlAttribute In _node.Attributes
    '                _file.Append($"{attr.Name}=""{attr.Value.Replace(vbCrLf, "")}"" ")
    '            Next attr
    '        End If
    '        _file.Append($">{vbCrLf}")

    '        ' children
    '        For Each chldNode As HtmlNode In _node.ChildNodes
    '            WriteNode(_file, chldNode, _indentLevel + 1)
    '        Next chldNode

    '        ' close tag
    '        _file.Append(Space(_indentLevel * 4))
    '        _file.Append($"</{_node.Name}>{vbCrLf}")
    '    End If
    'End Sub

    'Public Async Sub InitializeAsync()
    '    Await WebView21.EnsureCoreWebView2Async(Nothing)
    'End Sub

End Class
