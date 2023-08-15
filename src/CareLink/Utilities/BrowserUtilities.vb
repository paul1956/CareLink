' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Imports Microsoft.Win32

Friend Module BrowserUtilities

    Private Function LaunchBrowser(url As String) As Boolean
        Using userChoiceKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")
            If userChoiceKey Is Nothing Then
                Return False
            End If
            Dim progIdObject As Object = userChoiceKey.GetValue("ProgId")
            If progIdObject Is Nothing Then
                Return False
            End If
            Dim progIdValue As String = progIdObject.ToString
            If progIdValue Is Nothing Then
                Return False
            End If
            Dim programFiles As String = Environment.ExpandEnvironmentVariables("%ProgramW6432%")
            Dim programFilesX86 As String = Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%")
            Dim browserPath As String = Nothing
            If progIdValue.Contains("chrome", StringComparison.OrdinalIgnoreCase) Then
                browserPath = If(File.Exists($"{programFiles}\Google\Chrome\Application\chrome.exe"),
                                 $"{programFiles}\Google\Chrome\Application\chrome.exe",
                                 $"{programFilesX86}\Google\Chrome\Application\chrome.exe"
                                )
            ElseIf progIdValue.Contains("Firefox", StringComparison.OrdinalIgnoreCase) Then
                browserPath = $"{programFiles}\Mozilla Firefox\Firefox.exe"
            ElseIf progIdValue.Contains("msEdgeHtm", StringComparison.OrdinalIgnoreCase) Then
                browserPath = If(File.Exists($"{programFiles}\Microsoft\Edge\Application\msEdge.exe"),
                                 $"{programFiles}\Microsoft\Edge\Application\msEdge.exe",
                                 $"{programFilesX86}\Microsoft\Edge\Application\msEdge.exe"
                                )
            End If

            If Not String.IsNullOrWhiteSpace(browserPath) Then
                Dim info As New ProcessStartInfo(Environment.ExpandEnvironmentVariables(browserPath), url)
                Process.Start(info)
            Else
                Dim msgResult As MsgBoxResult = MsgBox($"Your default browser can't be found!", $"Please use any browser and navigate to {url}.",
                                                       MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, "Error Launching Browser")

            End If
        End Using
        Return True
    End Function

    Friend Sub OpenUrlInBrowser(webAddress As String)
        Try
            Form1.Cursor = Cursors.AppStarting
            Application.DoEvents()
            LaunchBrowser(webAddress)
        Catch ex As Exception
            Throw
        Finally
            Form1.Cursor = Cursors.AppStarting
            Application.DoEvents()
        End Try
    End Sub

End Module
