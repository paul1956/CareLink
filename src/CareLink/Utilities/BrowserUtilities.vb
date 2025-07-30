' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports Microsoft.Win32

''' <summary>
'''  Provides utility methods for launching the user's default web browser.
''' </summary>
Friend Module BrowserUtilities

    ''' <summary>
    '''  Attempts to launch the default web browser with the specified URL.
    ''' </summary>
    ''' <param name="url">The URL to open in the browser.</param>
    ''' <returns>
    '''  <see langword="True"/> if the browser was successfully launched or a message was shown to the user;
    '''  <see langword="False"/> if the default browser could not be determined.
    ''' </returns>
    ''' <remarks>
    '''  This method determines the default browser by reading the Windows registry and attempts to launch it.
    '''  If the browser cannot be found, a message box is shown to the user.
    ''' </remarks>
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
            Dim programFiles As String = Environment.ExpandEnvironmentVariables(name:="%ProgramW6432%")
            Dim programFilesX86 As String = Environment.ExpandEnvironmentVariables(name:="%ProgramFiles(x86)%")
            Dim browserPath As String = Nothing
            If progIdValue.ContainsIgnoreCase("chrome") Then
                browserPath = If(File.Exists($"{programFiles}\Google\Chrome\Application\chrome.exe"),
                                 $"{programFiles}\Google\Chrome\Application\chrome.exe",
                                 $"{programFilesX86}\Google\Chrome\Application\chrome.exe")
            ElseIf progIdValue.ContainsIgnoreCase("Firefox") Then
                browserPath = $"{programFiles}\Mozilla Firefox\Firefox.exe"
            ElseIf progIdValue.ContainsIgnoreCase("msEdgeHtm") Then
                browserPath = If(File.Exists($"{programFiles}\Microsoft\Edge\Application\msEdge.exe"),
                                 $"{programFiles}\Microsoft\Edge\Application\msEdge.exe",
                                 $"{programFilesX86}\Microsoft\Edge\Application\msEdge.exe")
            End If

            If Not String.IsNullOrWhiteSpace(value:=browserPath) Then
                Dim startInfo As New ProcessStartInfo(
                    fileName:=Environment.ExpandEnvironmentVariables(name:=browserPath),
                    arguments:=url)
                Process.Start(startInfo)
            Else
                MsgBox(
                    heading:=$"Your default browser can't be found!",
                    text:=$"Please use any browser and navigate to {url}.",
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground,
                    title:="Error Launching Browser")
            End If
        End Using
        Return True
    End Function

    ''' <summary>
    '''  Opens the specified web address in the user's default browser,
    '''  updating the main form's cursor during the operation.
    ''' </summary>
    ''' <param name="webAddress">The web address (URL) to open.</param>
    ''' <remarks>
    '''  This method sets the cursor to AppStarting while attempting to open the browser, and resets it afterward.
    ''' </remarks>
    Friend Sub OpenUrlInBrowser(webAddress As String)
        Dim mainForm As Form1 = My.Forms.Form1
        Try
            mainForm.Cursor = Cursors.AppStarting
            Application.DoEvents()
            LaunchBrowser(webAddress)
        Catch ex As Exception
            Throw
        Finally
            mainForm.Cursor = Cursors.Default
            Application.DoEvents()
        End Try
    End Sub

End Module
