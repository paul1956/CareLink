' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module Form1LoginHelpers

    <Extension>
    Friend Function DoOptionalLoginAndUpdateData(MainForm As Form1, UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        MainForm.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfManualBasal.Clear()
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                MainForm.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = GetPathToLastDownloadFile().ExtractCultureFromFileName(SavedLastDownloadBaseName)
                MainForm.RecentData = Loads(File.ReadAllText(GetPathToLastDownloadFile()))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.SetLastUpdateTime($"{File.GetLastWriteTime(GetPathToLastDownloadFile()).ToShortDateTimeString} from file", False)
                SetUpCareLinkUser(MainForm, GetPathToTestSettingsFile())
            Case FileToLoadOptions.TestData
                MainForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                MainForm.RecentData = Loads(File.ReadAllText(GetPathToTestData()))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                MainForm.SetLastUpdateTime($"{File.GetLastWriteTime(GetPathToTestData()).ToShortDateTimeString} from file", False)
                SetUpCareLinkUser(MainForm, GetPathToTestSettingsFile)
            Case FileToLoadOptions.Login
                MainForm.Text = SavedTitle
                Do Until MainForm.LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop

                If MainForm.Client Is Nothing OrElse Not MainForm.Client.LoggedIn Then
                    MainForm.ServerUpdateTimer.Interval = CInt(s_5MinutesInMilliseconds)
                    MainForm.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If NetworkDown Then
                        ReportLoginStatus(MainForm.LoginStatus)
                        Return False
                    End If

                    MainForm.SetLastUpdateTime("Unknown", True)
                    Return False
                End If

                Dim userSettingsPath As String = GetPathToUserSettingsFile(My.Settings.CareLinkUserName)
                SetUpCareLinkUser(MainForm, userSettingsPath)

                MainForm.RecentData = MainForm.Client.GetRecentData(MainForm)
                MainForm.ServerUpdateTimer.Interval = CInt(s_1MinutesInMilliseconds)
                MainForm.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkDown Then
                    ReportLoginStatus(MainForm.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(MainForm.LoginStatus, MainForm.RecentData Is Nothing OrElse MainForm.RecentData.Count = 0, MainForm.Client.GetLastErrorMessage)

                MainForm.MenuShowMiniDisplay.Visible = True
        End Select

        MainForm.Client.SessionProfile.SetInsulinType(CurrentUser.InsulinTypeName)
        With MainForm.DgvSessionProfile
            .InitializeDgv()
            .DataSource = MainForm.Client.SessionProfile.ToDataSource
        End With

        MainForm.PumpAITLabel.Text = CurrentUser.GetPumpAitString
        MainForm.InsulinTypeLabel.Text = CurrentUser.InsulinTypeName
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

    <Extension>
    Friend Sub UpdateHighLightInLastUpdateTime(statusLabel As ToolStripStatusLabel, highLight As Boolean)
        If highLight = True Then
            statusLabel.ForeColor = GetGraphLineColor("High Limit")
            statusLabel.BackColor = statusLabel.ForeColor.GetContrastingColor
        Else
            statusLabel.ForeColor = SystemColors.ControlText
            statusLabel.BackColor = SystemColors.Control
        End If
    End Sub

    <Extension>
    Friend Sub SetLastUpdateTime(mainForm As Form1, msg As String, highLight As Boolean)
        UpdateHighLightInLastUpdateTime(mainForm.LastUpdateTime, highLight)
        mainForm.LastUpdateTime.Text = $"Last Update Time: {msg}"
        mainForm.LastUpdateTime.Spring = True
        'Dim spaceWidth As Integer = TextRenderer.MeasureText(" "c, mainForm.LastUpdateTime.Font).Width
        'Dim desiredMidWidth As Integer = mainForm.StatusStrip1.Width - mainForm.LoginStatus.Width
        'desiredMidWidth -= mainForm.LastUpdateTime.Width \ 2
        'If desiredMidWidth > 0 Then
        '    Dim neededSpaces As Integer = desiredMidWidth \ spaceWidth
        '    Dim nSpaces As String = StrDup(neededSpaces, " "c)
        '    mainForm.LastUpdateTime.Text = $"{nSpaces}{$"Last Update Time: {msg}"}{nSpaces}"
        'End If

    End Sub

    Friend Sub SetUpCareLinkUser(mainForm As Form1, userSettingsPath As String)
        Dim contents As String
        If Path.Exists(userSettingsPath) Then
            contents = File.ReadAllText(userSettingsPath)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, JsonFormattingOptions)
        Else
            CurrentUser = New CurrentUserRecord(My.Settings.CareLinkUserName)
            Dim f As New InitializeDialog With {.CurrentUser = CurrentUser}
            f.ShowDialog()
            CurrentUser = f.CurrentUser
        End If

    End Sub

End Module
