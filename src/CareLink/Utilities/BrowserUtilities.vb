' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports Microsoft.Win32

Friend Module BrowserUtilities
    Private ReadOnly s_httpClient As New HttpClient()
    Private ReadOnly s_versionSearchKey As String = $"<a href=""/{OwnerName}/{RepoName}/releases/tag/"

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
        Return Version.Parse(gitHubVersions) > Version.Parse(appVersion.ToString)
    End Function

    Private Function LaunchBrowser(url As String) As Boolean
        Using userChoiceKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice")
            If userChoiceKey Is Nothing Then
                Return False
            End If
            Dim progIdObject As Object = userChoiceKey.GetValue("Progid")
            If progIdObject Is Nothing Then
                Return False
            End If
            Dim progIdValue As String = progIdObject.ToString
            If progIdValue Is Nothing Then
                Return False
            End If
            Dim browserPath As String = Nothing
            If progIdValue.Contains("chrome", StringComparison.OrdinalIgnoreCase) Then
                browserPath = "%ProgramFiles(x86)%\Google\Chrome\Application\chrome.exe"
            ElseIf progIdValue.Contains("Firefox", StringComparison.OrdinalIgnoreCase) Then
                browserPath = "C:\Program Files\Mozilla Firefox\Firefox.exe"
            ElseIf progIdValue.Contains("msEdgeHtm", StringComparison.OrdinalIgnoreCase) Then
                browserPath = "%ProgramFiles(x86)%\Microsoft\Edge\Application\msEdge.exe"
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
    ''' https://github.com/paul1956/CareLink/releases
    ''' Then look for version
    ''' <a href="/paul1956/CareLink/releases/tag/3.4.0.3" data-view-component="true" class="Link--primary">CareLink Display 3.4.0.3 x64</a>
    ''' </summary>
    ''' <param name="mainForm"></param>
    ''' <param name="reportResults">Always report result when true</param>
    Friend Async Sub CheckForUpdatesAsync(mainForm As Form1, reportResults As Boolean)
        Try
            Dim responseBody As String = Await s_httpClient.GetStringAsync($"{GitHubCareLinkUrl}releases")
            Dim index As Integer
            Dim versionStr As String = "0.0.0.0"
            For Each e As IndexClass(Of String) In responseBody.SplitLines().ToList().WithIndex()
                Dim line As String = e.Value
                If line.Contains(s_versionSearchKey, StringComparison.Ordinal) Then
                    index = line.IndexOf(s_versionSearchKey, StringComparison.OrdinalIgnoreCase) + s_versionSearchKey.Length
                    If index < 0 Then
                        Exit Sub
                    End If
                    Dim versionLength As Integer = line.IndexOf("""", index) - index
                    versionStr = line.Substring(index, versionLength)
                    If versionStr.Contains("-"c) Then
                        Continue For
                    End If
                    Exit For
                End If
            Next

            Dim gitHubVersion As String = versionStr
            If IsNewerVersion(gitHubVersion, My.Application.Info.Version) Then
                If MsgBox("There is a newer version available, do you want to install now?", MsgBoxStyle.YesNo, "Updates Available") = MsgBoxResult.Yes Then
                    OpenUrlInBrowser($"{GitHubCareLinkUrl}releases/")
                End If
            Else
                If reportResults Then
                    MsgBox("You are running latest version", MsgBoxStyle.OkOnly, "No Updates Available")
                End If
            End If
        Catch ex As Exception
            If reportResults Then
                MsgBox($"Connection failed while checking for new version:{Environment.NewLine}{Environment.NewLine}{ex.DecodeException()}", MsgBoxStyle.Information, "Version Check Failed")
            End If
        End Try
    End Sub

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
