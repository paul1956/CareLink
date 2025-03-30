' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Public Enum FileToLoadOptions As Integer
    LastSaved = 0
    Login = 1
    NewUser = 2
    Snapshot = 3
    TestData = 4
End Enum

Friend Module Form1LoginHelpers
    Public ReadOnly Property LoginDialog As New LoginDialog
    Public Property CurrentPdf As PdfSettingsRecord

    Friend Sub DeserializePatientElement(patientDataElement As JsonElement)
        Try
            PatientData = JsonSerializer.Deserialize(Of PatientDataInfo)(patientDataElement, s_jsonDeserializerOptions)
        Catch ex As Exception
            Stop
        End Try
        RecentData = patientDataElement.ConvertJsonElementToStringDictionary()
        s_timeFormat = PatientData.TimeFormat
        s_timeWithMinuteFormat = If(s_timeFormat = "HR_12", TimeFormatTwelveHourWithMinutes, TimeFormatMilitaryWithMinutes)
        s_timeWithoutMinuteFormat = If(s_timeFormat = "HR_12", TimeFormatTwelveHourWithoutMinutes, TimeFormatMilitaryWithoutMinutes)
    End Sub

    Friend Function DoOptionalLoginAndUpdateData(mainForm As Form1, updateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        Dim serverTimerEnabled As Boolean = StartOrStopServerUpdateTimer(False)
        s_listOfAutoBasalDeliveryMarkers.Clear()
        ProgramInitialized = False
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.TestData
                mainForm.Text = $"{SavedTitle} Using Test Data from 'SampleUserV2Data.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim patientDataElementAsText As String = File.ReadAllText(TestDataFileNameWithPath)
                Dim patientDataElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(patientDataElementAsText)
                DeserializePatientElement(patientDataElement)
                mainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(TestDataFileNameWithPath)
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(TestSettingsFileNameWithPath)
                fromFile = True
            Case FileToLoadOptions.Login, FileToLoadOptions.NewUser
                mainForm.Text = SavedTitle
                Do While True
                    LoginDialog.LoginSourceAutomatic = fileToLoad
                    Dim result As DialogResult = LoginDialog.ShowDialog(mainForm)
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
                        ReportLoginStatus(mainForm.LoginStatus, hasErrors:=True, lastErrorMessage:="Network Unavailable")
                        Return False
                    End If

                    SetLastUpdateTime(
                        msg:="Last Update time is unknown!",
                        suffixMessage:=String.Empty,
                        highLight:=True,
                        isDaylightSavingTime:=Nothing)
                    Return False
                End If
                Dim lastErrorMessage As String = LoginDialog.Client.GetRecentData()

                SetUpCareLinkUser(GetUserSettingsJsonFileNameWithPath, forceUI:=False)
                StartOrStopServerUpdateTimer(True, s_1MinutesInMilliseconds)

                If NetworkUnavailable() Then
                    ReportLoginStatus(mainForm.LoginStatus)
                    Return False
                End If
                ErrorReportingHelpers.ReportLoginStatus(mainForm.LoginStatus, RecentDataEmpty, lastErrorMessage)
                mainForm.MenuShowMiniDisplay.Visible = True
                fromFile = False
            Case FileToLoadOptions.LastSaved, FileToLoadOptions.Snapshot
                Dim lastDownloadFileWithPath As String = String.Empty
                Dim fixedPart As String = String.Empty

                Select Case fileToLoad
                    Case FileToLoadOptions.LastSaved
                        mainForm.Text = $"{SavedTitle} Using Last Saved Data"
                        fixedPart = BaseNameSavedLastDownload
                        lastDownloadFileWithPath = GetLastDownloadFileWithPath()
                    Case FileToLoadOptions.Snapshot
                        fixedPart = BaseNameSavedSnapshot
                        mainForm.Text = $"{SavedTitle} Using Snapshot Data"
                        Dim di As New DirectoryInfo(DirectoryForProjectData)
                        Dim fileList As String() = New DirectoryInfo(DirectoryForProjectData).EnumerateFiles($"CareLinkSnapshot*.json").OrderBy(Function(f As FileInfo) f.LastWriteTime).Select(Function(f As FileInfo) f.Name).ToArray
                        Using openFileDialog1 As New OpenFileDialog With {
                        .AddExtension = True,
                        .AddToRecent = False,
                        .CheckFileExists = True,
                        .CheckPathExists = True,
                        .DefaultExt = "json",
                        .Filter = $"json files (*.json)|CareLinkSnapshot*.json",
                        .InitialDirectory = DirectoryForProjectData,
                        .Multiselect = False,
                        .ReadOnlyChecked = True,
                        .RestoreDirectory = True,
                        .ShowPreview = False,
                        .SupportMultiDottedExtensions = False,
                        .Title = $"Select CareLink™ saved snapshot to load",
                        .ValidateNames = True}

                            If openFileDialog1.ShowDialog(mainForm) = DialogResult.OK Then
                                lastDownloadFileWithPath = openFileDialog1.FileName
                                If Not File.Exists(lastDownloadFileWithPath) Then
                                    Return False
                                End If
                            Else
                                Return False
                            End If
                        End Using
                    Case FileToLoadOptions.Snapshot
                End Select
                CurrentDateCulture = lastDownloadFileWithPath.ExtractCultureFromFileName(fixedPart)
                Dim patientDataElementAsText As String = File.ReadAllText(lastDownloadFileWithPath)
                Dim patientDataElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(patientDataElementAsText)
                DeserializePatientElement(patientDataElement)
                mainForm.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(lastDownloadFileWithPath)
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(TestSettingsFileNameWithPath)
                fromFile = True
        End Select
        If Form1.Client IsNot Nothing Then
            Form1.Client.PatientPersonalData.InsulinType = CurrentUser.InsulinTypeName
            With mainForm.DgvCurrentUser
                .InitializeDgv()
                .DataSource = Form1.Client.UserElementDictionary.ToDataSource
            End With
        End If

        mainForm.PumpAITLabel.Text = CurrentUser.GetPumpAitString
        mainForm.InsulinTypeLabel.Text = CurrentUser.InsulinTypeName
        mainForm.UpdateAllTabPages(fromFile)
        Return True
    End Function

    Friend Sub FinishInitialization(mainForm As Form1)
        mainForm.Cursor = Cursors.Default
        Application.DoEvents()
        mainForm.InitializeSummaryTabCharts()
        mainForm.InitializeActiveInsulinTabChart()
        mainForm.InitializeTimeInRangeArea()
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
                If RecentData?.TryGetValue(NameOf(ServerDataIndexes.clientTimeZoneName), timeZoneName) Then
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
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(userSettingsJson, s_jsonDeserializerOptions)
    End Sub

    Friend Sub SetUpCareLinkUser(userSettingsFile As String, forceUI As Boolean)
        Dim currentUserUpdateNeeded As Boolean = False
        Dim pdfNewerThanUserSettings As Boolean = False
        Dim pdfFileNameWithPath As String = GetUserSettingsPdfFileNameWithPath()

        If File.Exists(GetUserSettingsJsonFileNameWithPath) Then
            Dim userSettingsJson As String = File.ReadAllText(GetUserSettingsJsonFileNameWithPath)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(userSettingsJson, s_jsonSerializerOptions)

            If CurrentUser.InsulinRealAit = 0 Then
                CurrentUser.InsulinRealAit = s_insulinTypes.Values(0).AitHours
            End If
            If String.IsNullOrEmpty(CurrentUser.InsulinTypeName) Then
                CurrentUser.InsulinTypeName = s_insulinTypes.Keys(0)
            End If

            pdfNewerThanUserSettings = (File.Exists(pdfFileNameWithPath) AndAlso File.GetLastWriteTime(pdfFileNameWithPath) > File.GetLastWriteTime(GetUserSettingsJsonFileNameWithPath)) OrElse IsFileReadOnly(pdfFileNameWithPath)

            If Not forceUI Then
                If Not (pdfNewerThanUserSettings AndAlso IsFileStale(GetUserSettingsJsonFileNameWithPath)) Then
                    CurrentPdf = New PdfSettingsRecord(pdfFileNameWithPath)
                    Exit Sub
                End If
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

        If Form1.Client?.TryGetDeviceSettingsPdfFile(pdfFileNameWithPath) OrElse pdfNewerThanUserSettings OrElse File.Exists(pdfFileNameWithPath) Then
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
            Dim contents As String = JsonSerializer.Serialize(CurrentUser, s_jsonSerializerOptions)
            File.WriteAllTextAsync(
                path:=GetUserSettingsJsonFileNameWithPath,
                contents)
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
    Friend Function StartOrStopServerUpdateTimer(Start As Boolean, Optional interval As Integer = -1) As Boolean
        If Start Then
            If interval > -1 Then
                Form1.ServerUpdateTimer.Interval = interval
            End If
            Form1.ServerUpdateTimer.Start()
            DebugPrint($"started at {Now.ToLongTimeString}")
            Return True
        Else
            If Form1.ServerUpdateTimer.Enabled Then
                Form1.ServerUpdateTimer.Stop()
                DebugPrint($"stopped at {Now.ToLongTimeString}")
                Return True
            End If
        End If
        Return False
    End Function

End Module
