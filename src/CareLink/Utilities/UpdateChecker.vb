' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Threading

''' <summary>
'''  Provides functionality to check for application updates by comparing the current version
'''  with the latest release version available on GitHub.
''' </summary>
Friend Module UpdateChecker
    Private ReadOnly s_versionSearchKey As String = $"<a hRef=""/{GitOwnerName}/CareLink/releases/tag/"
    Private s_inCheckForUpdate As Integer = 0
    Private s_updateSleepCount As Integer = 0

    ''' <summary>
    '''  Retrieves the latest version string from the GitHub releases page.
    ''' </summary>
    ''' <returns>
    '''  A <see cref="Task(Of String)"/> representing the asynchronous operation. The result contains the version string,
    '''  or "0.0.0.0" if the version could not be determined.
    ''' </returns>
    Private Async Function GetVersionString() As Task(Of String)
        Dim versionStr As String = "0.0.0.0"
        Dim responseBody As String
        Try
            Using httpClient As New HttpClient()
                responseBody = Await httpClient.GetStringAsync($"{GitHubCareLinkUrl}releases")
            End Using
        Catch ex1 As HttpRequestException
            ' GitHub not reachable
            Return versionStr
        Catch ex As Exception
            Return versionStr
        End Try
        Dim index As Integer
        For Each e As IndexClass(Of String) In responseBody.SplitLines().WithIndex()
            Dim line As String = e.Value
            If line.ContainsIgnoreCase(s_versionSearchKey) Then
                index = line.IndexOfIgnoreCase(s_versionSearchKey) + s_versionSearchKey.Length
                If index < 0 Then
                    Exit For
                End If
                Dim versionLength As Integer = line.IndexOf(value:=""""c, startIndex:=index) - index
                versionStr = line.Substring(startIndex:=index, length:=versionLength)
                If versionStr.Contains("-"c) Then
                    Continue For
                End If
                Return versionStr
            End If
        Next

        Return versionStr
    End Function

    ''' <summary>
    '''  Compares the GitHub version string with the current application version.
    ''' </summary>
    ''' <param name="gitHubVersions">The version string retrieved from GitHub.</param>
    ''' <param name="appVersion">The current application version.</param>
    ''' <returns><see langword="True"/> if application version or converter version is different</returns>
    Private Function IsNewerVersion(gitHubVersions As String, appVersion As Version) As Boolean
        Return gitHubVersions IsNot Nothing AndAlso
            Not String.IsNullOrWhiteSpace(gitHubVersions) AndAlso
            Version.Parse(gitHubVersions) > Version.Parse(appVersion.ToString())
    End Function

    ''' <summary>
    '''  Checks for updates by comparing the current application version with the latest version available on GitHub.
    '''  If a newer version is found, notifies the user and optionally prompts to install the update.
    ''' </summary>
    ''' <param name="reportSuccessfulResult">
    '''  If <see langword="True"/>, always reports the result to the user, even if no update is available.
    ''' </param>
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
                        If Interlocked.Exchange(location1:=s_inCheckForUpdate, 1) = 0 Then
                            If MsgBox(
                                heading:=$"There is a newer version available, do you want to install now?",
                                text:=$"Current version {My.Application.Info.Version}{vbCrLf}New version {gitHubVersion}",
                                buttonStyle:=MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
                                title:="Updates Available") = MsgBoxResult.Yes Then

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
                Form1.UpdateAvailableStatusStripLabel.ForeColor = Form1.MenuStrip1.ForeColor
                If reportSuccessfulResult Then
                    MsgBox(
                        heading:="You are running the latest version",
                        text:="",
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                        title:="No Updates Available")
                End If
            End If
        Catch ex As Exception
            If reportSuccessfulResult Then
                MsgBox(
                    heading:="Connection failed while checking for new version",
                    text:=ex.DecodeException(),
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                    title:="Version Check Failed")
            End If
        End Try

    End Sub

End Module
