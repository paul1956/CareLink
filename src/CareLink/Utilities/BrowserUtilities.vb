' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32

Friend Module BrowserUtilities

    ''' <summary>
    ''' Compare version of executable with ReadMe.MkDir from GitHub
    ''' </summary>
    ''' <param name="gitHubVersions"></param>
    ''' <param name="appVersion"></param>
    ''' <param name="converterVersion"></param>
    ''' <returns>True if application version or converter version is different</returns>
    ''' <remarks>Uses equality is comparison to allow testing before upload to GitHub</remarks>
    Private Function IsNewerVersion(gitHubVersions As String, appVersion As Version) As Boolean
        If gitHubVersions Is Nothing OrElse String.IsNullOrWhiteSpace(gitHubVersions) Then
            Return False
        End If
        If Version.Parse(gitHubVersions) <> Version.Parse(appVersion.ToString) Then
            Return True
        End If
        Return False
    End Function

    Private Sub LaunchBrowser(url As String)
        Using userChoiceKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")
            If userChoiceKey Is Nothing Then
                Exit Sub
            End If
            Dim progIdObject As Object = userChoiceKey.GetValue("Progid")
            If progIdObject Is Nothing Then
                Exit Sub
            End If
            Dim progIdValue As String = CStr(progIdObject)
            If progIdValue Is Nothing Then
                Exit Sub
            End If
            Dim msgResult As MsgBoxResult = MsgBoxResult.Ok
            Dim browserPath As String = "%ProgramFiles(x86)%\Internet Explorer\iExplore.exe"
            If progIdValue.Contains("chrome", StringComparison.OrdinalIgnoreCase) Then
                browserPath = "%ProgramFiles(x86)%\Google\Chrome\Application\chrome.exe"
            ElseIf progIdValue.Contains("Firefox", StringComparison.OrdinalIgnoreCase) Then
                browserPath = "C:\Program Files\Mozilla Firefox\Firefox.exe"
            ElseIf progIdValue.Contains("msEdgeHtm", StringComparison.OrdinalIgnoreCase) Then
                browserPath = "%ProgramFiles(x86)%\Microsoft\Edge\Application\msEdge.exe"
                'ElseIf progIdValue.Contains("opera", StringComparison.OrdinalIgnoreCase) Then
                '    browserPath = "opera.exe"
            Else
                msgResult = MsgBox($"Your default browser {progIdValue} is not supported, iExplorer will be used if you select OK!, please enter an issue with browser 'ProgId' and full path",
                                   MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground)
            End If
            If msgResult = MsgBoxResult.Ok Then
                Dim info As New ProcessStartInfo(Environment.ExpandEnvironmentVariables(browserPath), url)
                Process.Start(info)
            End If
        End Using
    End Sub

    Friend Async Sub CheckForUpdatesAsync(mainForm As Form1, reportResults As Boolean)
        Try
            Dim responseBody As String = Await mainForm._httpClient.GetStringAsync($"{Form1.GitHubCareLinkUrl}blob/master/README.md")
            Dim index As Integer
            Dim versionStr As String = "0.0.0.0"
            For Each e As IndexClass(Of String) In responseBody.SplitLines().ToList().WithIndex()
                Dim line As String = e.Value
                If line.Contains("What's New in this release", StringComparison.Ordinal) Then
                    If e.IsLast Then
                        MsgBox($"Failed while checking for new version: File '{Form1.GitHubCareLinkUrl}blob/master/ReadMe.MD' is corrupt", MsgBoxStyle.Information, "Version Check Failed")
                        Exit Sub
                    End If
                    e.MoveNext()
                    line = e.Value
                    index = line.IndexOf("New in ", StringComparison.OrdinalIgnoreCase)
                    If index < 0 Then
                        Exit Sub
                    End If
                    versionStr = line.Substring(index + "New In ".Length)
                    Exit For
                End If
            Next

            index = versionStr.IndexOf("<"c)
            If index > 0 Then
                versionStr = versionStr.Substring(0, index)
            End If
            Dim gitHubVersion As String = versionStr
            If IsNewerVersion(gitHubVersion, My.Application.Info.Version) Then
                If reportResults Then
                    If MsgBox("There is a newer version available, do you want to install now?", MsgBoxStyle.YesNo, "Updates Available") = MsgBoxResult.Yes Then
                        OpenUrlInBrowser($"{Form1.GitHubCareLinkUrl}releases/")
                    End If
                End If
            Else
                If reportResults Then
                    MsgBox("You are running latest version", MsgBoxStyle.OkOnly, "No Updates Available")
                End If
            End If
        Catch ex As Exception
            If reportResults Then
                MsgBox("Failed while checking for new  version: " + ex.Message, MsgBoxStyle.Information, "Version Check Failed")
            End If
        End Try
    End Sub

    Friend Sub OpenUrlInBrowser(webAddress As String)
        Try
            'Devices.Mouse.OverrideCursor = Cursors.AppStarting
            Form1.Cursor = Cursors.AppStarting
            LaunchBrowser(webAddress)
        Catch ex As Exception
            Throw
        Finally
            Form1.Cursor = Cursors.AppStarting
            'Devices.Mouse.OverrideCursor = Nothing
        End Try
    End Sub

End Module
