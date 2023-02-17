' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Friend Module Form1LoginHelpers

    <Extension>
    Friend Function DoOptionalLoginAndUpdateData(MainForm As Form1, UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        MainForm.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        s_listOfAutoBasalDeliveryMarkers.Clear()
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                MainForm.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(SavedLastDownloadName)
                MainForm.RecentData = Loads(File.ReadAllText(LastDownloadWithPath))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(LastDownloadWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.TestData
                MainForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim testDataWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
                MainForm.RecentData = Loads(File.ReadAllText(testDataWithPath))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.LastUpdateTime.Text = $"{File.GetLastWriteTime(testDataWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.Login
                MainForm.Text = SavedTitle
                Do Until MainForm.LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop

                If MainForm.Client Is Nothing OrElse Not MainForm.Client.LoggedIn Then
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
                s_listOfManualBasal.Clear()
                MainForm.RecentData = MainForm.Client.GetRecentData(MainForm)
                MainForm.ServerUpdateTimer.Interval = s_oneMinutesInMilliseconds
                MainForm.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkDown Then
                    ReportLoginStatus(MainForm.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(MainForm.LoginStatus, MainForm.RecentData Is Nothing OrElse MainForm.RecentData.Count = 0, MainForm.Client.GetLastErrorMessage)

                MainForm.MenuShowMiniDisplay.Visible = True
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

        MainForm.InitializeSummaryTabCharts()
        MainForm.InitializeActiveInsulinTabChart()
        MainForm.InitializeTimeInRangeArea()

        MainForm.Initialized = True
    End Sub

End Module
