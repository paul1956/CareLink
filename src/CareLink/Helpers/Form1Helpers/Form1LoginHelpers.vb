' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Enum FileToLoadOptions As Integer
    LastSaved = 0
    TestData = 1
    Login = 2
End Enum

Friend Module Form1LoginHelpers
    Public ReadOnly Property LoginDialog As New LoginDialog
    Public Property CurrentPdf As PdfSettingsRecord

    Friend Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        Dim serverTimerEnabled As Boolean = StartOrStopServerUpdateTimer(False)
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfManualBasal.Clear()
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                Form1.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = GetLastDownloadFileWithPath().ExtractCultureFromFileName(BaseNameSavedLastDownload)
                RecentData = LoadIndexedItems(File.ReadAllText(GetLastDownloadFileWithPath()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetLastDownloadFileWithPath())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(TestSettingsFileNameWithPath)
                fromFile = True
            Case FileToLoadOptions.TestData
                Form1.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                RecentData = LoadIndexedItems(File.ReadAllText(TestDataFileNameWithPath))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(TestDataFileNameWithPath)
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(TestSettingsFileNameWithPath)
                fromFile = True
            Case FileToLoadOptions.Login
                Form1.Text = SavedTitle
                Do While True
                    Dim result As DialogResult = LoginDialog.ShowDialog(My.Forms.Form1)
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

                SetUpCareLinkUser(GetUserSettingsJsonFileNameWithPath, False)
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

        If highLight Then
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

    Friend Sub SetUpCareLinkUser(userSettingsFileWithPath As String)
        Dim userSettingsJson As String = File.ReadAllText(userSettingsFileWithPath)
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(userSettingsJson, JsonFormattingOptions)
    End Sub

    Friend Sub SetUpCareLinkUser(userSettingsFileWithPath As String, forceUI As Boolean)
        Dim currentUserUpdateNeeded As Boolean = False
        Dim pdfNewerThanUserSettings As Boolean = False
        Dim pdfFileNameWithPath As String = GetUserSettingsPdfFileNameWithPath()

        If File.Exists(userSettingsFileWithPath) Then
            Dim userSettingsJson As String = File.ReadAllText(userSettingsFileWithPath)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(userSettingsJson, JsonFormattingOptions)

            If CurrentUser.InsulinRealAit = 0 Then
                CurrentUser.InsulinRealAit = s_insulinTypes.Values(0).AitHours
            End If
            If String.IsNullOrEmpty(CurrentUser.InsulinTypeName) Then
                CurrentUser.InsulinTypeName = s_insulinTypes.Keys(0)
            End If

            pdfNewerThanUserSettings = IsFileReadOnly(pdfFileNameWithPath) OrElse (File.Exists(pdfFileNameWithPath) AndAlso File.GetLastWriteTime(pdfFileNameWithPath) > File.GetLastWriteTime(userSettingsFileWithPath))

            If pdfNewerThanUserSettings OrElse Not (forceUI OrElse IsFileStale(userSettingsFileWithPath)) Then
                CurrentPdf = New PdfSettingsRecord(pdfFileNameWithPath)
                Exit Sub
            End If
        Else
            CurrentUser = New CurrentUserRecord(s_userName, If(Not Is700Series(), CheckState.Checked, CheckState.Indeterminate))
            currentUserUpdateNeeded = True
        End If

        Form1.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Dim ait As Single = 2
        Dim carbRatios As New List(Of CarbRatioRecord)
        Dim currentTarget As Single = 120

        If pdfNewerThanUserSettings OrElse Form1.Client.TryGetDeviceSettingsPdfFile(pdfFileNameWithPath) OrElse File.Exists(pdfFileNameWithPath) Then
            CurrentPdf = New PdfSettingsRecord(pdfFileNameWithPath)
            If CurrentPdf.IsValid Then
                If CurrentUser.PumpAit <> CurrentPdf.Bolus.BolusWizard.ActiveInsulinTime Then
                    currentUserUpdateNeeded = True
                End If
                ait = CurrentPdf.Bolus.BolusWizard.ActiveInsulinTime
                If CurrentUser.CurrentTarget <> CurrentPdf.SmartGuard.Target Then
                    currentUserUpdateNeeded = True
                End If
                currentTarget = CurrentPdf.SmartGuard.Target
                If Not CurrentPdf.Bolus.DeviceCarbohydrateRatios.EqualCarbRatios(CurrentUser.CarbRatios) Then
                    currentUserUpdateNeeded = True
                End If
                carbRatios = CurrentPdf.Bolus.DeviceCarbohydrateRatios.ToCarbRatioList
            End If
        End If
        If currentUserUpdateNeeded OrElse forceUI Then
            Dim f As New InitializeDialog(CurrentUser, ait, currentTarget, carbRatios)
            Dim result As DialogResult = f.ShowDialog(My.Forms.Form1)
            If result = DialogResult.OK Then
                currentUserUpdateNeeded = currentUserUpdateNeeded OrElse Not CurrentUser.Equals(f.CurrentUser)
                CurrentUser = f.CurrentUser.Clone
            End If
        End If
        If currentUserUpdateNeeded Then
            File.WriteAllTextAsync(userSettingsFileWithPath,
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
