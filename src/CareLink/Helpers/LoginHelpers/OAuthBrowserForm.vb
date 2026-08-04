' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Class OAuthBrowserForm
    Inherits Form

    Private ReadOnly _webView As New WebView2()
    Private ReadOnly _startUrl As String
    Private ReadOnly _redirectUri As String

    Public Property Result As RedirectResult
    Private Shared ReadOnly s_separator As Char() = New Char() {"&"c}

    Public Sub New(startUrl As String, redirectUri As String)
        _startUrl = startUrl
        _redirectUri = redirectUri

        Me.Text = "Carelink Login"
        Me.Width = 1200
        Me.Height = 900

        _webView.Dock = DockStyle.Fill
        Me.Controls.Add(value:=_webView)

        AddHandler Shown, AddressOf Me.OAuthBrowserForm_Shown
    End Sub

    Private Async Sub OAuthBrowserForm_Shown(sender As Object, e As EventArgs)
        Await Me.InitializeAsync()
    End Sub

    Private Async Function InitializeAsync() As Task
        Await _webView.EnsureCoreWebView2Async()

        AddHandler _webView.CoreWebView2.NavigationStarting, AddressOf Me.WebView_NavigationStarting
        AddHandler _webView.CoreWebView2.SourceChanged, AddressOf Me.WebView_SourceChanged

        _webView.CoreWebView2.Navigate(_startUrl)
    End Function

    Private Sub WebView_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs)
        If String.IsNullOrWhiteSpace(value:=e.Uri) Then Return
        If e.Uri.StartsWith(value:=_redirectUri, comparisonType:=StringComparison.OrdinalIgnoreCase) Then
            Me.CaptureAndClose(uriString:=e.Uri)
        End If
    End Sub

    Private Sub WebView_SourceChanged(sender As Object, e As CoreWebView2SourceChangedEventArgs)
        Dim currentUrl As String = _webView.Source?.ToString()
        If String.IsNullOrWhiteSpace(value:=currentUrl) Then Return
        If currentUrl.StartsWith(value:=_redirectUri, comparisonType:=StringComparison.OrdinalIgnoreCase) Then
            Me.CaptureAndClose(uriString:=currentUrl)
        End If
    End Sub

    Private Sub CaptureAndClose(uriString As String)
        Dim uri As New Uri(uriString)

        Dim queryString As String = If(uri.Query, String.Empty)
        If queryString.StartsWith(value:="?"c, comparisonType:=StringComparison.Ordinal) Then
            queryString = queryString.Substring(startIndex:=1)
        End If

        Dim pairs As String() = If(String.IsNullOrEmpty(value:=queryString),
                                   Array.Empty(Of String)(),
                                   queryString.Split(s_separator, options:=StringSplitOptions.RemoveEmptyEntries))
        Dim dict As New Dictionary(Of String, String)(comparer:=StringComparer.OrdinalIgnoreCase)

        For Each pair As String In pairs
            Dim idx As Integer = pair.IndexOf("="c)
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
        dict.TryGetValue("code", code)
        dict.TryGetValue("state", state)

        Me.Result = New RedirectResult With {
            .Code = code,
            .State = state
        }

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class
