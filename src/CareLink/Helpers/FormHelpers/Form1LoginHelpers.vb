' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module Form1LoginHelpers
    Public ReadOnly Property LoginDialog As New LoginForm1

    <Extension>
    Friend Function DoOptionalLoginAndUpdateData(MainForm As Form1, UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        MainForm.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfManualBasal.Clear()
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                MainForm.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = GetPathToLastDownloadFile().ExtractCultureFromFileName(SavedLastDownloadBaseName)
                RecentData = Loads(File.ReadAllText(GetPathToLastDownloadFile()))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetPathToLastDownloadFile())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(MainForm, GetPathToTestSettingsFile())
                fromFile = True
            Case FileToLoadOptions.TestData
                MainForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                RecentData = Loads(File.ReadAllText(GetPathToTestData()))
                MainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetPathToTestData())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(MainForm, GetPathToTestSettingsFile)
                fromFile = True
            Case FileToLoadOptions.Login
                MainForm.Text = SavedTitle
                Do Until LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop

                If MainForm.Client Is Nothing OrElse Not MainForm.Client.LoggedIn Then
                    MainForm.ServerUpdateTimer.Interval = CInt(s_5MinutesInMilliseconds)
                    MainForm.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If NetworkUnavailable() Then
                        ReportLoginStatus(MainForm.LoginStatus)
                        Return False
                    End If

                    SetLastUpdateTime("Last Update time is unknown!", "", True, Nothing)
                    Return False
                End If

                Dim userSettingsPath As String = GetPathToUserSettingsFile(My.Settings.CareLinkUserName)
                RecentData = MainForm.Client.GetRecentData(MainForm)
                SetUpCareLinkUser(MainForm, userSettingsPath)
                MainForm.ServerUpdateTimer.Interval = CInt(s_1MinutesInMilliseconds)
                MainForm.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(MainForm.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkUnavailable() Then
                    ReportLoginStatus(MainForm.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(MainForm.LoginStatus, RecentData Is Nothing OrElse RecentData.Count = 0, MainForm.Client.GetLastErrorMessage)

                MainForm.MenuShowMiniDisplay.Visible = True
                fromFile = False
        End Select

        If MainForm.Client IsNot Nothing Then
            MainForm.Client.SessionProfile?.SetInsulinType(CurrentUser.InsulinTypeName)
            With MainForm.DgvSessionProfile
                .InitializeDgv()
                .DataSource = MainForm.Client.SessionProfile.ToDataSource
            End With
        End If

        MainForm.PumpAITLabel.Text = CurrentUser.GetPumpAitString
        MainForm.InsulinTypeLabel.Text = CurrentUser.InsulinTypeName
        MainForm.FinishInitialization()
        If UpdateAllTabs Then
            MainForm.UpdateAllTabPages(fromFile)
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

        ProgramInitialized = True
    End Sub

    <Extension>
    Friend Sub SetLastUpdateTime(msg As String, suffixMessage As String, highLight As Boolean, isDaylightSavingTime? As Boolean)
        Dim foreColor As Color
        Dim backColor As Color

        If highLight = True Then
            foreColor = GetGraphLineColor("High Limit")
            backColor = foreColor.GetContrastingColor
        Else
            foreColor = SystemColors.ControlText
            backColor = SystemColors.Control
        End If

        With Form1.LastUpdateTimeToolStripStatusLabel
            If Not String.IsNullOrWhiteSpace(msg) Then
                .Text = $"{msg}"
            End If
            .ForeColor = foreColor
            .BackColor = backColor
        End With

        With Form1.TimeZoneToolStripStatusLabel
            If isDaylightSavingTime Is Nothing Then
                .Text = ""
            Else
                Dim timeZoneName As String = Nothing
                If RecentData?.TryGetValue(NameOf(ItemIndexes.clientTimeZoneName), timeZoneName) Then
                    Dim timeZoneInfo As TimeZoneInfo = CalculateTimeZone(timeZoneName)
                    .Text = $"{If(isDaylightSavingTime, timeZoneInfo.DaylightName, timeZoneInfo.StandardName)} {suffixMessage}".Trim
                Else
                    .Text = ""
                End If
            End If
            .ForeColor = foreColor
            .BackColor = backColor
        End With

    End Sub

    Friend Sub SetUpCareLinkUser(mainForm As Form1, userSettingsPath As String)
        Dim contents As String
        If Path.Exists(userSettingsPath) Then
            contents = File.ReadAllText(userSettingsPath)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, JsonFormattingOptions)
        Else
            CurrentUser = New CurrentUserRecord(My.Settings.CareLinkUserName, If(Not (RecentData?.Is770G), CheckState.Checked, CheckState.Indeterminate))
            Dim f As New InitializeDialog With {
                .CurrentUser = CurrentUser,
                .InitializeDialogRecentData = RecentData
            }
            f.ShowDialog()
            CurrentUser = f.CurrentUser
        End If

    End Sub

End Module
