' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Friend Module Form1Helpers

    <Extension>
    Friend Function DoOptionalLoginAndUpdateData(MeForm As Form1, UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        MeForm.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MeForm.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                MeForm.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(RepoDownloadName)
                MeForm.RecentData = Loads(File.ReadAllText(LastDownloadWithPath))
                MeForm.ShowMiniDisplay.Visible = Debugger.IsAttached
                MeForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(LastDownloadWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.TestData
                MeForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim testDataWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
                MeForm.RecentData = Loads(File.ReadAllText(testDataWithPath))
                MeForm.ShowMiniDisplay.Visible = Debugger.IsAttached
                MeForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(testDataWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.Login
                MeForm.Text = SavedTitle
                Do Until MeForm.LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop
                If MeForm.client Is Nothing OrElse Not MeForm.client.LoggedIn Then
                    MeForm.ServerUpdateTimer.Interval = s_fiveMinutesInMilliseconds
                    MeForm.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MeForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If CareLinkClient.NetworkDown Then
                        MeForm.LoginStatus.Text = "Network Down"
                        Return False
                    End If

                    MeForm.LastUpdateTime.Text = "Unknown"
                    Return False
                End If
                MeForm.RecentData = MeForm.client.GetRecentData(MeForm.LoginDialog.LoggedOnUser.CountryCode)
                MeForm.ServerUpdateTimer.Interval = s_twoMinutesInMilliseconds
                MeForm.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MeForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                If CareLinkClient.NetworkDown Then
                    MeForm.LoginStatus.Text = "Network Down"
                    Return False
                End If
                MeForm.ShowMiniDisplay.Visible = True
                MeForm.LoginStatus.Text = "OK"
        End Select
        MeForm.FinishInitialization()
        If UpdateAllTabs Then
            MeForm.AllTabPagesUpdate()
        End If
        Return True
    End Function

    <Extension>
    Friend Sub FinishInitialization(MeForm As Form1)
        MeForm.Cursor = Cursors.Default
        Application.DoEvents()

        MeForm.InitializeHomePageChart()
        MeForm.InitializeActiveInsulinTabChart()
        MeForm.InitializeTimeInRangeArea()

        MeForm.Initialized = True
    End Sub

    <Extension>
    Friend Function ScaleMarker(innerdic As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Dim newMarker As New Dictionary(Of String, String)
        For Each kvp As KeyValuePair(Of String, String) In innerdic
            Select Case kvp.Key
                Case "value"
                    newMarker.Add(kvp.Key, kvp.scaleValue(2))
                Case Else
                    newMarker.Add(kvp.Key, kvp.Value)
            End Select
        Next
        Return newMarker
    End Function

End Module
