' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Enum CheckForUpdate As Integer
    Always = 0
    Never = 1
    Ask = 2
End Enum

Friend Enum FileToLoadOptions As Integer
    LastSaved = 0
    TestData = 1
    Login = 2
End Enum

Friend Module Form1LoginHelpers
    Public ReadOnly Property LoginDialog As New LoginDialog

    Friend Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        Dim serverTimerEnabled As Boolean = StartOrStopServerUpdateTimer(False)
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfManualBasal.Clear()
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                Form1.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = GetLastDownloadFileWithPath().ExtractCultureFromFileName(SavedLastDownloadBaseName)
                RecentData = Loads(File.ReadAllText(GetLastDownloadFileWithPath()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetLastDownloadFileWithPath())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(GetTestSettingsFileNameWihtPath(), CheckForUpdate.Never)
                fromFile = True
            Case FileToLoadOptions.TestData
                Form1.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                RecentData = Loads(File.ReadAllText(GetTestDataFileNameWithPath()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetTestDataFileNameWithPath())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(GetTestSettingsFileNameWihtPath, CheckForUpdate.Never)
                fromFile = True
            Case FileToLoadOptions.Login
                Form1.Text = SavedTitle
                Do While True
                    Dim result As DialogResult = LoginDialog.ShowDialog
                    Select Case result
                        Case DialogResult.OK
                            Exit Do
                        Case DialogResult.Cancel
                            StartOrStopServerUpdateTimer(serverTimerEnabled)
                            Return False
                        Case DialogResult.Retry
                    End Select
                Loop

                If Form1.Client Is Nothing OrElse Not Form1.Client.LoggedIn Then
                    StartOrStopServerUpdateTimer(True, s_5MinutesInMilliseconds)

                    If NetworkUnavailable() Then
                        ReportLoginStatus(Form1.LoginStatus)
                        Return False
                    End If

                    SetLastUpdateTime("Last Update time is unknown!", "", True, Nothing)
                    Return False
                End If

                RecentData = Form1.Client.GetRecentData()

                SetUpCareLinkUser(GetUserSettingsFileNameWithPath("json"), CheckForUpdate.Always)
                StartOrStopServerUpdateTimer(True, s_1MinutesInMilliseconds)

                If NetworkUnavailable() Then
                    ReportLoginStatus(Form1.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(Form1.LoginStatus, RecentDataEmpty, Form1.Client.GetLastErrorMessage)
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
            backColor = foreColor.GetContrastingColor()
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

    Friend Sub SetUpCareLinkUser(userSettingsFileWithPath As String, checkForUpdate As CheckForUpdate)
        Dim page As New TaskDialogPage
        Dim ait As Single = 2
        Dim carbRatios As New List(Of CarbRatioRecord)
        Dim lastUpdateTimeString As String = "Your device setting file has never been downloaded!"
        Dim currentUserUpdateNeeded As Boolean = False
        If File.Exists(userSettingsFileWithPath) Then
            Dim lastUpdateTime As Date = File.GetLastWriteTime(userSettingsFileWithPath)
            lastUpdateTimeString = $"Your Last Update was {lastUpdateTime.ToShortDateString}"
            Dim contents As String = File.ReadAllText(userSettingsFileWithPath)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, JsonFormattingOptions)

            If checkForUpdate = CheckForUpdate.Never Then Exit Sub
            If lastUpdateTime > Now - s_30DaysSpan AndAlso File.GetLastWriteTime(GetUserSettingsFileNameWithPath("pdf")) < lastUpdateTime Then
                Exit Sub
            End If
        Else
            CurrentUser = New CurrentUserRecord(My.Settings.CareLinkUserName, If(Not Is770G(), CheckState.Checked, CheckState.Indeterminate))
            currentUserUpdateNeeded = True
        End If

        If checkForUpdate = CheckForUpdate.Always OrElse MsgBox($"Would you like to update from latest {ProjectName}™ Device Settings PDF File", lastUpdateTimeString, MsgBoxStyle.YesNo, $"Use {ProjectName}™ Settings File", -1, page) = MsgBoxResult.Yes Then
            Dim pdfFileNamepdfFileName As String = ""
            Form1.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            If Form1.Client.TryGetDeviceSettingsPdfFile(pdfFileNamepdfFileName) Then
                Dim pdf As New PdfSettingsRecord(pdfFileNamepdfFileName)
                If CurrentUser.PumpAit <> pdf.Bolus.BolusWizard.ActiveInsulinTime Then
                    ait = pdf.Bolus.BolusWizard.ActiveInsulinTime
                    currentUserUpdateNeeded = True
                End If
                If pdf.Bolus.DeviceCarbohydrateRatios.CompareToCarbRatios(CurrentUser.CarbRatios) Then
                    carbRatios = pdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
                    currentUserUpdateNeeded = True
                End If
            Else
                If currentUserUpdateNeeded Then
                    CurrentUser = New CurrentUserRecord(My.Settings.CareLinkUserName, If(Not Is770G(), CheckState.Checked, CheckState.Indeterminate))
                    currentUserUpdateNeeded = True
                End If
            End If
        End If
        If currentUserUpdateNeeded Then
            Dim f As New InitializeDialog(CurrentUser, ait, carbRatios)
            Dim result As DialogResult = f.ShowDialog()
            If result = DialogResult.OK Then
                currentUserUpdateNeeded = Not CurrentUser.Equals(f.CurrentUser)
                CurrentUser = f.CurrentUser.Clone
            End If
        End If
        If currentUserUpdateNeeded Then
            File.WriteAllTextAsync(GetUserSettingsFileNameWithPath("json"),
                      JsonSerializer.Serialize(CurrentUser, JsonFormattingOptions))

        End If
        Form1.Cursor = Cursors.Default
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''Starts or stops ServerUpdateTimer
    ''' </summary>
    ''' <param name="Start"></param>
    ''' <param name="interval">Timer interval in milliseconds</param>
    ''' <param name="memberName"></param>
    ''' <param name="sourceLineNumber"></param>
    ''' <returns>State of Timer before function was called</returns>
    Friend Function StartOrStopServerUpdateTimer(Start As Boolean, Optional interval As Integer = -1, <CallerMemberName> Optional memberName As String = "", <CallerLineNumber> Optional sourceLineNumber As Integer = 0) As Boolean
        If Start Then
            If interval > -1 Then
                Form1.ServerUpdateTimer.Interval = interval
            End If
            Form1.ServerUpdateTimer.Start()
            Debug.Print($"In {memberName} line {sourceLineNumber}, {NameOf(Form1.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
            Return True
        Else
            If Form1.ServerUpdateTimer.Enabled Then
                Form1.ServerUpdateTimer.Stop()
                Debug.Print($"In {memberName} line {sourceLineNumber}, {NameOf(Form1.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
                Return True
            End If
        End If
        Return False
    End Function

End Module
