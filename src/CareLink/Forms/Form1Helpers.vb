' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Friend Module Form1Helpers

    <Extension>
    Friend Function DoOptionalLoginAndUpdateData(MainForm As Form1, UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        MainForm.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                MainForm.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(RepoDownloadName)
                MainForm.RecentData = Loads(File.ReadAllText(LastDownloadWithPath))
                MainForm.ShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(LastDownloadWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.TestData
                MainForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim testDataWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
                MainForm.RecentData = Loads(File.ReadAllText(testDataWithPath))
                MainForm.ShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(testDataWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.Login
                MainForm.Text = SavedTitle
                Do Until MainForm.LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop

                If MainForm.client Is Nothing OrElse Not MainForm.client.LoggedIn Then
                    MainForm.ServerUpdateTimer.Interval = s_fiveMinutesInMilliseconds
                    MainForm.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If NetworkDown Then
                        ReportLoginStatus(MainForm.LoginStatus)
                        Return False
                    End If

                    MainForm.LastUpdateTime.Text = "Unknown"
                    Return False
                End If
                MainForm.RecentData = MainForm.client.GetRecentData(MainForm)
                MainForm.ServerUpdateTimer.Interval = s_oneMinutesInMilliseconds
                MainForm.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkDown Then
                    ReportLoginStatus(MainForm.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(MainForm.LoginStatus, MainForm.RecentData Is Nothing OrElse MainForm.RecentData.Count = 0, MainForm.client.GetLastErrorMessage)
                MainForm.ShowMiniDisplay.Visible = True
        End Select
        MainForm.FinishInitialization()
        If UpdateAllTabs Then
            MainForm.UpdateAllTabPages()
        End If
        Return True
    End Function

    <Extension>
    Friend Sub FinishInitialization(MainForm As Form1)
        MainForm.Cursor = Cursors.Default
        Application.DoEvents()

        MainForm.InitializeHomePageChart()
        MainForm.InitializeActiveInsulinTabChart()
        MainForm.InitializeTimeInRangeArea()

        MainForm.Initialized = True
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

    Public Sub ReportLoginStatus(loginStatus As Label)
        ReportLoginStatus(loginStatus, True, "No Internet Connection!")
    End Sub

    Public Sub ReportLoginStatus(loginStatus As Label, hasErrors As Boolean, Optional lastErrorMessage As String = "")
        If hasErrors Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = lastErrorMessage
        Else
            loginStatus.ForeColor = Color.Black
            loginStatus.Text = "OK"
        End If
    End Sub

    Public Sub ReportLoginStatus(loginStatus As TextBox, hasErrors As Boolean, Optional lastErrorMessage As String = "")
        If hasErrors Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = lastErrorMessage
        Else
            loginStatus.ForeColor = Color.Black
            loginStatus.Text = "OK"
        End If
    End Sub

End Module
