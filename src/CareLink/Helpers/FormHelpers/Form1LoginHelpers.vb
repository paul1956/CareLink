' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module Form1LoginHelpers
    Public ReadOnly Property LoginDialog As New LoginForm1

    Friend Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        Form1.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(Form1.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfManualBasal.Clear()
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                Form1.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = GetPathToLastDownloadFile().ExtractCultureFromFileName(SavedLastDownloadBaseName)
                RecentData = Loads(File.ReadAllText(GetPathToLastDownloadFile()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetPathToLastDownloadFile())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(GetPathToTestSettingsFile())
                fromFile = True
            Case FileToLoadOptions.TestData
                Form1.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                RecentData = Loads(File.ReadAllText(GetPathToTestData()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetPathToTestData())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(GetPathToTestSettingsFile)
                fromFile = True
            Case FileToLoadOptions.Login
                Form1.Text = SavedTitle
                Do Until LoginDialog.ShowDialog() <> DialogResult.Retry
                Loop

                If Form1.Client Is Nothing OrElse Not Form1.Client.LoggedIn Then
                    Form1.ServerUpdateTimer.Interval = CInt(s_5MinutesInMilliseconds)
                    Form1.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(Form1.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If NetworkUnavailable() Then
                        ReportLoginStatus(Form1.LoginStatus)
                        Return False
                    End If

                    SetLastUpdateTime("Last Update time is unknown!", "", True, Nothing)
                    Return False
                End If

                Dim userSettingsPath As String = GetPathToUserSettingsFile(My.Settings.CareLinkUserName)
                RecentData = Form1.Client.GetRecentData()
                SetUpCareLinkUser(userSettingsPath)
                Form1.ServerUpdateTimer.Interval = CInt(s_1MinutesInMilliseconds)
                Form1.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(Form1.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkUnavailable() Then
                    ReportLoginStatus(Form1.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(Form1.LoginStatus, RecentData Is Nothing OrElse RecentData.Count = 0, Form1.Client.GetLastErrorMessage)
                SpeechSupportReported = False
                Form1.MenuShowMiniDisplay.Visible = True
                fromFile = False
        End Select

        If Form1.Client IsNot Nothing Then
            Form1.Client.SessionProfile?.SetInsulinType(CurrentUser.InsulinTypeName)
            With Form1.DgvSessionProfile
                .InitializeDgv()
                .DataSource = Form1.Client.SessionProfile.ToDataSource
            End With
        End If

        Form1.PumpAITLabel.Text = CurrentUser.GetPumpAitString
        Form1.InsulinTypeLabel.Text = CurrentUser.InsulinTypeName
        FinishInitialization()
        If UpdateAllTabs Then
            Form1.UpdateAllTabPages(fromFile)
        End If
        Return True
    End Function

    Friend Sub FinishInitialization()
        Form1.Cursor = Cursors.Default
        Application.DoEvents()

        Form1.InitializeSummaryTabCharts()
        Form1.InitializeActiveInsulinTabChart()
        Form1.InitializeTimeInRangeArea()

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

    Friend Sub SetUpCareLinkUser(userSettingsPath As String)
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
