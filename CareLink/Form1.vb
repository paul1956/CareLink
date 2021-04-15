''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Text.RegularExpressions
Imports Microsoft.Web.WebView2.Core

Public Class Form1
    Private WithEvents Timer1 As New Timer

    Private Shared Sub WebView21_ContentLoading(sender As Object, e As CoreWebView2ContentLoadingEventArgs) Handles WebView21.ContentLoading
        Debug.Print($"Is Error Page = {e.IsErrorPage}")
    End Sub

    Private Shared Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        Debug.Print($"Web Error Status = {e.WebErrorStatus}")
    End Sub

    Private Shared Sub WebView21_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs) Handles WebView21.WebMessageReceived
        Debug.Print($"Web Message As Json = {e.WebMessageAsJson}, URL = {e.Source}")
    End Sub

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Await WebView21.EnsureCoreWebView2Async()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        WebView21.Size = ClientSize - New Size(WebView21.Location)
    End Sub

    Private Async Function parseMyHtml() As Task(Of String)
        Dim htmlToParse = Await WebView21.ExecuteScriptAsync("document.documentElement.outerHTML;")
        htmlToParse = Regex.Unescape(htmlToParse)
        htmlToParse = htmlToParse.Remove(0, 1)
        htmlToParse = htmlToParse.Remove(htmlToParse.Length - 1, 1)

        Dim i As Integer = htmlToParse.IndexOf("<div class=""sensor-value""", StringComparison.InvariantCulture)
        If i >= 0 Then
            i = htmlToParse.IndexOf(">", i + 1, StringComparison.InvariantCulture)
            i += 1
            Dim lessThanIndex As Integer = htmlToParse.IndexOf("<", i, StringComparison.InvariantCulture)
            Return htmlToParse.Substring(i, lessThanIndex - i)
        End If
        Return ""
    End Function

    Private Async Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Await parseMyHtml()
    End Sub

    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles WebView21.NavigationStarting
        AddressBar.Text = e.Uri
    End Sub

    Private Sub WebView21_SourceChanged(sender As Object, e As CoreWebView2SourceChangedEventArgs) Handles WebView21.SourceChanged
        Debug.Print($"Is New Document = {e.IsNewDocument}")

        If Timer1.Enabled Then
            Timer1.Stop() 'Timer starts functioning
            Timer1.Interval = 10000
        End If
        Timer1.Start() 'Timer starts functioning

    End Sub

End Class
