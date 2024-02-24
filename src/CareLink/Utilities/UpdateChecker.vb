' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Threading

Friend Module UpdateChecker
    Private ReadOnly s_httpClient As New HttpClient()
    Private ReadOnly s_versionSearchKey As String = $"<a hRef=""/{GitOwnerName}/CareLink/releases/tag/"
    Private s_inCheckForUpdate As Integer = 0
    Private s_updateSleepCount As Integer = 0

    Private Async Function GetVersionString() As Task(Of String)
        Dim versionStr As String = "0.0.0.0"
        Dim responseBody As String
        Try
            responseBody = Await s_httpClient.GetStringAsync($"{GitHubCareLinkUrl}releases")
        Catch ex As Exception
            Return versionStr
        End Try
        Dim index As Integer
        For Each e As IndexClass(Of String) In responseBody.SplitLines().WithIndex()
            Dim line As String = e.Value
            If line.Contains(s_versionSearchKey, StringComparison.OrdinalIgnoreCase) Then
                index = line.IndexOf(s_versionSearchKey, StringComparison.OrdinalIgnoreCase) + s_versionSearchKey.Length
                If index < 0 Then
                    Exit For
                End If
                Dim versionLength As Integer = line.IndexOf(""""c, index) - index
                versionStr = line.Substring(index, versionLength)
                If versionStr.Contains("-"c) Then
                    Continue For
                End If
                Exit For
            End If
        Next

        Return versionStr
    End Function

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
                s_updateSleepCount = 0
            End If
            Dim gitHubVersion As String = Await GetVersionString()
            If IsNewerVersion(gitHubVersion, My.Application.Info.Version) Then
                If s_updateSleepCount > 0 Then
                    s_updateSleepCount -= 1
                Else
                    Form1.UpdateAvailableStatusStripLabel.Text = $"Update {gitHubVersion} available"
                    Form1.UpdateAvailableStatusStripLabel.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    Form1.UpdateAvailableStatusStripLabel.Image = My.Resources.NotificationAlertRed_16x
                    Form1.UpdateAvailableStatusStripLabel.ImageAlign = ContentAlignment.MiddleLeft
                    Form1.UpdateAvailableStatusStripLabel.ForeColor = Color.Red
                    If reportSuccessfulResult Then
                        If Interlocked.Exchange(s_inCheckForUpdate, 1) = 0 Then
                            If MsgBox($"There is a newer version available, do you want to install now?", $"Current version {My.Application.Info.Version}{vbCrLf}New version {gitHubVersion}", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Updates Available") = MsgBoxResult.Yes Then
                                OpenUrlInBrowser($"{GitHubCareLinkUrl}releases/")
                                End
                            End If
                            s_inCheckForUpdate = 0
                            s_updateSleepCount = 288
                        End If
                    End If
                End If
            Else
                Form1.UpdateAvailableStatusStripLabel.DisplayStyle = ToolStripItemDisplayStyle.Text
                Form1.UpdateAvailableStatusStripLabel.Text = $"Current version {My.Application.Info.Version}"
                Form1.UpdateAvailableStatusStripLabel.ImageAlign = ContentAlignment.MiddleLeft
                Form1.UpdateAvailableStatusStripLabel.ForeColor = Color.Black
                If reportSuccessfulResult Then
                    MsgBox("You are running latest version", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "No Updates Available")
                End If
            End If
        Catch ex As Exception
            If reportSuccessfulResult Then
                MsgBox("Connection failed while checking for new version", ex.DecodeException(), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, "Version Check Failed")
            End If
        End Try

    End Sub

End Module
