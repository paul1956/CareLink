' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net.Http
Imports System.Threading
Imports Microsoft.Win32

Friend Module BrowserUtilities
    Private ReadOnly s_httpClient As New HttpClient()
    Private ReadOnly s_versionSearchKey As String = $"<a hRef=""/{GitOwnerName}/{ProjectName}/releases/tag/"
    Private updateSleepCount As Integer = 0
    Private inCheckForUpdate As Integer = 0

    ''' <summary>
    ''' Compare version of executable with ReadMe.MkDir from GitHub
    ''' </summary>
    ''' <param name="gitHubVersions"></param>
    ''' <param name="appVersion"></param>
    ''' <param name="converterVersion"></param>
    ''' <returns>True if application version or converter version is different</returns>
    ''' <remarks>Uses equality is comparison to allow testing before upload to GitHub</remarks>
    Private Function IsNewerVersion(gitHubVersions As String, appVersion As Version) As Boolean
        Return gitHubVersions IsNot Nothing AndAlso
               Not String.IsNullOrWhiteSpace(gitHubVersions) AndAlso
               Version.Parse(gitHubVersions) > Version.Parse(appVersion.ToString)
    End Function

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
                Dim msgResult As MsgBoxResult = MsgBox($"Your default browser can't be found!, Please use any browser and navigate to {url}.",
                                                       MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground)

            End If
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Find version string in HTML page
    ''' https://GitHub.com/Paul1956/CareLink/releases
    ''' Then look for version
    ''' <a href="/Paul1956/CareLink/releases/tag/3.4.0.3" data-view-component="true" class="Link--primary">CareLink Display 3.4.0.3 x64</a>
    ''' </summary>
    ''' <param name="reportSuccessfulResult">Always report result when true</param>
    Friend Async Sub CheckForUpdatesAsync(reportSuccessfulResult As Boolean)
        Try
            If reportSuccessfulResult Then
                updateSleepCount = 0
            End If
            Dim gitHubVersion As String = Await GetVersionString()
            If IsNewerVersion(gitHubVersion, My.Application.Info.Version) Then
                If updateSleepCount > 0 Then
                    updateSleepCount -= 1
                Else
                    Form1.UpdateAvailableStatusStripLabel.Text = $"Update {gitHubVersion} available"
                    Form1.UpdateAvailableStatusStripLabel.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    Form1.UpdateAvailableStatusStripLabel.Image = My.Resources.NotificationAlertRed_16x
                    Form1.UpdateAvailableStatusStripLabel.ImageAlign = ContentAlignment.MiddleLeft
                    Form1.UpdateAvailableStatusStripLabel.ForeColor = Color.Red
                    If reportSuccessfulResult Then
                        If Interlocked.Exchange(inCheckForUpdate, 1) = 0 Then
                            If MsgBox($"There is a newer version available, do you want to install now?{vbCrLf}Current version {My.Application.Info.Version}{vbCrLf}New version {gitHubVersion}", MsgBoxStyle.YesNo, "Updates Available") = MsgBoxResult.Yes Then
                                OpenUrlInBrowser($"{GitHubCareLinkUrl}releases/")
                                End
                            End If
                            inCheckForUpdate = 0
                            updateSleepCount = 288
                        End If
                    End If
                End If
            Else
                Form1.UpdateAvailableStatusStripLabel.DisplayStyle = ToolStripItemDisplayStyle.Text
                Form1.UpdateAvailableStatusStripLabel.Text = $"Current version {My.Application.Info.Version}"
                Form1.UpdateAvailableStatusStripLabel.ImageAlign = ContentAlignment.MiddleLeft
                Form1.UpdateAvailableStatusStripLabel.ForeColor = Color.Black
                If reportSuccessfulResult Then
                    MsgBox("You are running latest version", MsgBoxStyle.OkOnly, "No Updates Available")
                End If
            End If
        Catch ex As Exception
            If reportSuccessfulResult Then
                MsgBox($"Connection failed while checking for new version:{vbCrLf}{vbCrLf}{ex.DecodeException()}", MsgBoxStyle.Information, "Version Check Failed")
            End If
        End Try

    End Sub

    Friend Async Function GetVersionString() As Task(Of String)
        Dim versionStr As String = "0.0.0.0"
        Dim responseBody As String = Await s_httpClient.GetStringAsync($"{GitHubCareLinkUrl}releases")
        Dim index As Integer
        For Each e As IndexClass(Of String) In responseBody.SplitLines().ToList().WithIndex()
            Dim line As String = e.Value
            If line.Contains(s_versionSearchKey, StringComparison.OrdinalIgnoreCase) Then
                index = line.IndexOf(s_versionSearchKey, StringComparison.OrdinalIgnoreCase) + s_versionSearchKey.Length
                If index < 0 Then
                    Exit For
                End If
                Dim versionLength As Integer = line.IndexOf("""", index) - index
                versionStr = line.Substring(index, versionLength)
                If versionStr.Contains("-"c) Then
                    Continue For
                End If
                Exit For
            End If
        Next

        Return versionStr
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
