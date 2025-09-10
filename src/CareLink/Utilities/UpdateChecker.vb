' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Threading

''' <summary>
'''  Provides functionality to check for application updates by comparing the
'''  current version with the latest release version available on GitHub.
''' </summary>
''' <remarks>
'''  This module interacts with the GitHub releases page to determine if a newer version
'''  of the application is available. It updates the UI to notify the user and can prompt
'''  for installation of the update.
'''  <para>
'''   All update checks are performed asynchronously to avoid blocking the UI thread.
'''  </para>
''' </remarks>
Friend Module UpdateChecker

    ''' <summary>
    '''  The search key used to locate the version string in the GitHub releases page HTML.
    ''' </summary>
    Private ReadOnly s_versionSearchKey As String =
        $"<a hRef=""/{GitOwnerName}/CareLink/releases/tag/"

    ''' <summary>
    '''  Indicates if an update check is currently in progress.
    ''' </summary>
    ''' <remarks>
    '''  Used for thread-safety to prevent concurrent update checks.
    ''' </remarks>
    Private s_inCheckForUpdate As Integer = 0

    ''' <summary>
    '''  Controls the frequency of update notifications to the user.
    ''' </summary>
    ''' <remarks>
    '''  When greater than zero, update notifications are suppressed until
    '''  the count reaches zero.
    ''' </remarks>
    Private s_updateSleepCount As Integer = 0

    ''' <summary>
    '''  Retrieves the latest version string from the GitHub releases page.
    ''' </summary>
    ''' <returns>
    '''  A <see cref="Task(Of String)"/> representing the asynchronous operation.
    '''  The result contains the version string, or <c>"0.0.0.0"</c> if the version could not
    '''  be determined.
    ''' </returns>
    ''' <exception cref="HttpRequestException">
    '''  Thrown if the GitHub releases page cannot be reached.
    ''' </exception>
    ''' <exception cref="Exception">
    '''  Thrown if an unexpected error occurs while retrieving or parsing the version string.
    ''' </exception>
    ''' <remarks>
    '''  Pre-release versions (containing a dash) are ignored.
    ''' </remarks>
    ''' <example>
    '''  <code language="vbNet">
    '''  Dim latestVersion As String = Await GetVersionString()
    '''  </code>
    ''' </example>
    Private Async Function GetVersionString() As Task(Of String)
        Dim versionStr As String = "0.0.0.0"
        Dim responseBody As String
        Try
            Using httpClient As New HttpClient()
                responseBody = Await httpClient.GetStringAsync(
                    requestUri:=$"{GitHubCareLinkUrl}releases")
            End Using
        Catch ex1 As HttpRequestException
            ' GitHub not reachable
            Return versionStr
        Catch ex As Exception
            Return versionStr
        End Try
        Dim startIndex As Integer
        For Each e As IndexClass(Of String) In responseBody.SplitLines().WithIndex()
            Dim line As String = e.Value
            If line.ContainsNoCase(s_versionSearchKey) Then
                startIndex = line.IndexOfNoCase(value:=s_versionSearchKey)
                If startIndex >= 0 Then
                    startIndex += s_versionSearchKey.Length

                    Dim quotePos As Integer = line.IndexOf(value:=""""c, startIndex)
                    If quotePos > startIndex Then
                        versionStr =
                            line.Substring(startIndex, length:=quotePos - startIndex)

                        ' Skip versions with a dash (e.g., pre-release builds)
                        If Not versionStr.Contains("-"c) Then
                            Return versionStr
                        End If
                    End If
                End If
            End If
        Next

        Return versionStr
    End Function

    ''' <summary>
    '''  Compares the GitHub version string with the current application version.
    ''' </summary>
    ''' <param name="gitHubVersion">
    '''  The version string retrieved from GitHub. See <paramref name="gitHubVersion"/>.
    ''' </param>
    ''' <param name="version">
    '''  The current application version. See <paramref name="version"/>.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the GitHub version is newer than the
    '''  current application version;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <exception cref="FormatException">
    '''  Thrown if either version string cannot be parsed as a <see cref="Version"/>.
    ''' </exception>
    ''' <remarks>
    '''  Uses <see cref="Version.Parse"/> for comparison.
    ''' </remarks>
    ''' <example>
    '''  <code language="vbNet">
    '''  Dim isNewer As Boolean = IsNewerVersion("5.1.0.0", My.Application.Info.Version)
    '''  </code>
    ''' </example>
    Private Function IsNewerVersion(gitHubVersion As String, version As Version) As Boolean
        Return gitHubVersion IsNot Nothing AndAlso
            Not String.IsNullOrWhiteSpace(value:=gitHubVersion) AndAlso
            Version.Parse(input:=gitHubVersion) > Version.Parse(input:=version.ToString())
    End Function

    ''' <summary>
    '''  Checks for updates by comparing the current application version
    '''  with the latest version available on GitHub.
    '''  If a newer version is found, notifies the user and
    '''  optionally prompts to install the update.
    ''' </summary>
    ''' <param name="reportSuccessfulResult">
    '''  If <see langword="True"/>, always reports the result to the user,
    '''  even if no update is available. See <paramref name="reportSuccessfulResult"/>.
    ''' </param>
    ''' <remarks>
    '''  Updates the status strip label in the main form to indicate update status.
    '''  If the user chooses to install the update, the application will exit
    '''  after opening the releases page. Handles exceptions by displaying
    '''  an error message if <paramref name="reportSuccessfulResult"/> is
    '''  <see langword="True"/>.
    ''' </remarks>
    ''' <exception cref="Exception">
    '''  Any exception thrown during the update check is caught and reported to the
    '''  user if requested.
    ''' </exception>
    ''' <example>
    '''  <code language="vbNet">
    '''  Await CheckForUpdatesAsync(True)
    '''  </code>
    ''' </example>
    Friend Async Sub CheckForUpdatesAsync(reportSuccessfulResult As Boolean)
        Const heading As String =
            "There is a newer version available, do you want to install now?"
        Try
            If reportSuccessfulResult Then
                s_updateSleepCount = 0
            End If
            Dim gitHubVersion As String = Await GetVersionString()
            If IsNewerVersion(gitHubVersion, My.Application.Info.Version) Then
                If s_updateSleepCount > 0 Then
                    s_updateSleepCount -= 1
                Else
                    Form1.UpdateAvailableStatusStripLabel.Text =
                        $"Update {gitHubVersion} available"

                    Form1.UpdateAvailableStatusStripLabel.DisplayStyle =
                        ToolStripItemDisplayStyle.ImageAndText

                    Form1.UpdateAvailableStatusStripLabel.Image =
                        My.Resources.NotificationAlertRed_16x

                    Form1.UpdateAvailableStatusStripLabel.ImageAlign =
                        ContentAlignment.MiddleLeft

                    Form1.UpdateAvailableStatusStripLabel.ForeColor = Color.Red
                    If reportSuccessfulResult Then
                        If Interlocked.Exchange(
                                location1:=s_inCheckForUpdate,
                                value:=1) = 0 Then
                            Dim prompt As String =
                                $"Current version {My.Application.Info.Version}" & vbCrLf &
                                $"New version {gitHubVersion}"

                            If MsgBox(
                                heading,
                                prompt,
                                buttonStyle:=MsgBoxStyle.YesNo Or MsgBoxStyle.Question,
                                title:="Updates Available") = MsgBoxResult.Yes Then

                                OpenUrlInBrowser(url:=$"{GitHubCareLinkUrl}releases/")
                                End
                            End If
                            s_inCheckForUpdate = 0
                            s_updateSleepCount = 288
                        End If
                    End If
                End If
            Else
                Form1.UpdateAvailableStatusStripLabel.DisplayStyle =
                    ToolStripItemDisplayStyle.Text
                Form1.UpdateAvailableStatusStripLabel.Text =
                    $"Current version {My.Application.Info.Version}"
                Form1.UpdateAvailableStatusStripLabel.ImageAlign =
                    ContentAlignment.MiddleLeft
                Form1.UpdateAvailableStatusStripLabel.ForeColor =
                    Form1.MenuStrip1.ForeColor
                If reportSuccessfulResult Then
                    MsgBox(
                        heading:="You are running the latest version",
                        prompt:="",
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                        title:="No Updates Available")
                End If
            End If
        Catch ex As Exception
            If reportSuccessfulResult Then
                MsgBox(
                    heading:="Connection failed while checking for new version",
                    prompt:=ex.DecodeException(),
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                    title:="Version Check Failed")
            End If
        End Try

    End Sub

End Module
