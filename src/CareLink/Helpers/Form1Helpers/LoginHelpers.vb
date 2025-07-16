' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

''' <summary>
'''  Specifies the options for which file to load during login or data initialization.
''' </summary>
Public Enum FileToLoadOptions As Integer
    LastSaved = 0
    Login = 1
    NewUser = 2
    Snapshot = 3
    TestData = 4
End Enum

Friend Module LoginHelpers
    Public Property CurrentPdf As PdfSettingsRecord
    Public ReadOnly Property LoginDialog As New LoginDialog

    ''' <summary>
    '''  Converts a <see cref="Dictionary(Of String, Object)"/> to a <see cref="List(Of KeyValuePair(Of String, String))"/>.
    ''' </summary>
    ''' <param name="dic">The <see cref="Dictionary(Of String, Object)"/> to convert.</param>
    ''' <returns>
    '''  A list of key-value pairs where the value is converted to a <see langword="String"/>.
    ''' </returns>
    <Extension>
    Private Function ToDataSource(dic As Dictionary(Of String, Object)) As List(Of KeyValuePair(Of String, String))
        Dim dataSource As New List(Of KeyValuePair(Of String, String))
        For Each kvp As KeyValuePair(Of String, Object) In dic
            dataSource.Add(KeyValuePair.Create(kvp.Key, CType(kvp.Value, String)))
        Next
        Return dataSource
    End Function

    ''' <summary>
    '''  Deserializes the patient data element and updates related global variables.
    ''' </summary>
    Friend Sub DeserializePatientElement()
        Try
            PatientData = JsonSerializer.Deserialize(Of PatientDataInfo)(PatientDataElement, s_jsonDeserializerOptions)
            RecentData = PatientDataElement.ConvertJsonElementToStringDictionary()
        Catch ex As Exception
            MessageBox.Show(
                text:=$"Error deserializing patient data: {ex.Message}",
                caption:="Deserialization Error",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Error)
            Stop
        End Try
        s_timeWithMinuteFormat = If(PatientData.TimeFormat = "HR_12", TimeFormatTwelveHourWithMinutes, TimeFormatMilitaryWithMinutes)
        s_timeWithoutMinuteFormat = If(PatientData.TimeFormat = "HR_12", TimeFormatTwelveHourWithoutMinutes, TimeFormatMilitaryWithoutMinutes)
    End Sub

    ''' <summary>
    '''  Handles optional login and updates user data based on the specified file load option.
    ''' </summary>
    ''' <param name="owner">The main application form.</param>
    ''' <param name="updateAllTabs">
    '''  <see langword="True"/> to update all tabs after login; otherwise, <see langword="False"/>.
    ''' </param>
    ''' <param name="fileToLoad">The file load option to use.</param>
    ''' <returns>
    '''  <see langword="True"/> if login and data update succeeded; otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function DoOptionalLoginAndUpdateData(
        owner As Form1,
        updateAllTabs As Boolean,
        fileToLoad As FileToLoadOptions) As Boolean

        Dim serverTimerEnabled As Boolean = StartOrStopServerUpdateTimer(Start:=False)
        s_listOfAutoBasalDeliveryMarkers.Clear()
        ProgramInitialized = False
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.TestData
                owner.Text = $"{SavedTitle} Using Test Data from 'SampleUserV2Data.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim json As String = File.ReadAllText(TestDataFileNameWithPath)
                PatientDataElement = JsonSerializer.Deserialize(Of JsonElement)(json)
                DeserializePatientElement()
                owner.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(TestDataFileNameWithPath)
                owner.SetLastUpdateTime(
                    msg:=fileDate.ToShortDateTimeString,
                    suffixMessage:="from file",
                    highLight:=False,
                    isDaylightSavingTime:=fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser()
                fromFile = True
                owner.TabControlPage1.Visible = True
                owner.TabControlPage2.Visible = True
            Case FileToLoadOptions.Login, FileToLoadOptions.NewUser
                owner.Text = SavedTitle
                Do While True
                    LoginDialog.LoginSourceAutomatic = fileToLoad
                    Dim result As DialogResult = LoginDialog.ShowDialog(owner)
                    Select Case result
                        Case DialogResult.OK
                            Exit Do
                        Case DialogResult.Cancel
                            owner.TabControlPage1.Visible = False
                            owner.TabControlPage2.Visible = False
                            StartOrStopServerUpdateTimer(serverTimerEnabled)
                            Return False
                        Case DialogResult.Retry
                    End Select
                Loop

                If Form1.Client Is Nothing OrElse Not Form1.Client.LoggedIn Then
                    StartOrStopServerUpdateTimer(Start:=True, interval:=FiveMinutesInMilliseconds)

                    If NetworkUnavailable() Then
                        ReportLoginStatus(owner.LoginStatus, hasErrors:=True, lastErrorMessage:="Network Unavailable")
                        Return False
                    End If

                    owner.SetLastUpdateTime(
                        msg:="Last Update time is unknown!",
                        suffixMessage:=String.Empty,
                        highLight:=True,
                        isDaylightSavingTime:=Nothing)
                    Return False
                End If
                Dim lastErrorMessage As String = LoginDialog.Client.GetRecentData()

                SetUpCareLinkUser(forceUI:=False)
                StartOrStopServerUpdateTimer(Start:=True, interval:=OneMinutesInMilliseconds)

                If NetworkUnavailable() Then
                    ReportLoginStatus(owner.LoginStatus)
                    Return False
                End If
                ErrorReportingHelpers.ReportLoginStatus(owner.LoginStatus, hasErrors:=RecentDataEmpty, lastErrorMessage)
                owner.MenuShowMiniDisplay.Visible = True
                fromFile = False
                owner.TabControlPage1.Visible = True
                owner.TabControlPage2.Visible = True
            Case FileToLoadOptions.LastSaved, FileToLoadOptions.Snapshot
                Dim lastDownloadFileWithPath As String = String.Empty
                Dim fixedPart As String = String.Empty

                Select Case fileToLoad
                    Case FileToLoadOptions.LastSaved
                        owner.Text = $"{SavedTitle} Using Last Saved Data"
                        fixedPart = BaseNameSavedLastDownload
                        lastDownloadFileWithPath = GetLastDownloadFileWithPath()
                    Case FileToLoadOptions.Snapshot
                        fixedPart = "CareLink"
                        owner.Text = $"{SavedTitle} Using Snapshot Data"
                        Dim di As New DirectoryInfo(DirectoryForProjectData)
                        Dim fileList As String() = New DirectoryInfo(path:=DirectoryForProjectData) _
                            .EnumerateFiles(searchPattern:=$"CareLinkSnapshot*.json") _
                            .OrderBy(Function(f As FileInfo) f.LastWriteTime) _
                            .Select(Function(f As FileInfo) f.Name).ToArray

                        Using openFileDialog1 As New OpenFileDialog With {
                            .AddExtension = True,
                            .AddToRecent = False,
                            .CheckFileExists = True,
                            .CheckPathExists = True,
                            .DefaultExt = "json",
                            .Filter = $"json files (*.json)|CareLink*.json",
                            .InitialDirectory = DirectoryForProjectData,
                            .Multiselect = False,
                            .ReadOnlyChecked = True,
                            .RestoreDirectory = True,
                            .ShowPreview = False,
                            .SupportMultiDottedExtensions = False,
                            .Title = $"Select CareLink™ saved snapshot to load",
                            .ValidateNames = True}

                            If openFileDialog1.ShowDialog(owner) = DialogResult.OK Then
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
                owner.TabControlPage1.Visible = True
                owner.TabControlPage2.Visible = True
                CurrentDateCulture = lastDownloadFileWithPath.ExtractCultureFromFileName(fixedPart, fuzzy:=True)
                Dim json As String = File.ReadAllText(lastDownloadFileWithPath)
                PatientDataElement = JsonSerializer.Deserialize(Of JsonElement)(json)
                DeserializePatientElement()
                owner.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(lastDownloadFileWithPath)
                owner.SetLastUpdateTime(
                    msg:=fileDate.ToShortDateTimeString,
                    suffixMessage:="from file",
                    highLight:=False,
                    isDaylightSavingTime:=fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser()
                fromFile = True
        End Select
        If Form1.Client IsNot Nothing Then
            Form1.Client.PatientPersonalData.InsulinType = CurrentUser.InsulinTypeName
            With owner.DgvCurrentUser
                .InitializeDgv()
                .DataSource = Form1.Client.UserElementDictionary.ToDataSource
            End With
        End If

        owner.PumpAITLabel.Text = CurrentUser.GetPumpAitString
        owner.InsulinTypeLabel.Text = CurrentUser.InsulinTypeName
        If updateAllTabs Then
            owner.UpdateAllTabPages(fromFile)
        End If
        Return True
    End Function

    ''' <summary>
    '''  Completes initialization of the <paramref name="mainForm"/> after login and data loading.
    ''' </summary>
    ''' <param name="mainForm">The main application form.</param>
    Friend Sub FinishInitialization(mainForm As Form1)
        mainForm.Cursor = Cursors.Default
        Application.DoEvents()
        mainForm.InitializeSummaryTabCharts()
        mainForm.InitializeActiveInsulinTabChart()
        mainForm.InitializeTimeInRangeArea()
    End Sub

    ''' <summary>
    '''  Determines whether the network is unavailable.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if the network is unavailable; otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function NetworkUnavailable() As Boolean
        Return Not My.Computer.Network.IsAvailable
    End Function

    ''' <summary>
    '''  Converts a PNG <see cref="Bitmap"/> to an <see cref="Icon"/> object (with 32x32 size).
    ''' </summary>
    ''' <param name="bmp">The <see cref="Bitmap"/> to convert.</param>
    ''' <returns>
    '''  An <see cref="Icon"/> object created from the bitmap.
    ''' </returns>
    Public Function PngBitmapToIcon(bmp As Bitmap) As Icon
        ' Optionally resize to 32x32 for best icon compatibility
        Using resizedBmp As New Bitmap(bmp, New Size(32, 32))
            Dim hIcon As IntPtr = resizedBmp.GetHicon()
            Return Icon.FromHandle(hIcon)
        End Using
    End Function

    ''' <summary>
    '''  Sets the last update time and time zone information on the main form's status bar.
    ''' </summary>
    ''' <param name="form1">The main application form.</param>
    ''' <param name="msg">The message to display for the last update time.</param>
    ''' <param name="suffixMessage">The suffix message for the time zone label.</param>
    ''' <param name="highLight">
    '''  <see langword="True"/> to highlight the status label; otherwise, <see langword="False"/>.
    ''' </param>
    ''' <param name="isDaylightSavingTime">
    '''  <see langword="Nothing"/> if unknown; otherwise, <see langword="True"/> or <see langword="False"/>.
    ''' </param>
    <Extension>
    Friend Sub SetLastUpdateTime(
        form1 As Form1,
        msg As String,
        suffixMessage As String,
        highLight As Boolean,
        isDaylightSavingTime? As Boolean)

        With form1.LastUpdateTimeToolStripStatusLabel
            If Not String.IsNullOrWhiteSpace(msg) Then
                .Text = $"{msg}"
            End If
            If highLight Then
                .ForeColor = GetGraphLineColor("High Alert")
                .BackColor = .ForeColor.ContrastingColor()
            Else
                .BackColor = form1.MenuStrip1.BackColor
                .ForeColor = form1.MenuStrip1.ForeColor
            End If
        End With

        With form1.TimeZoneToolStripStatusLabel
            .Text = ""
            .ForeColor = form1.MenuStrip1.ForeColor
            If isDaylightSavingTime IsNot Nothing Then
                Dim timeZoneName As String = Nothing
                If RecentData?.TryGetValue(NameOf(ServerDataIndexes.clientTimeZoneName), timeZoneName) Then
                    Dim timeZoneInfo As TimeZoneInfo = CalculateTimeZone(timeZoneName)
                    Dim dst As String = If(isDaylightSavingTime,
                                           timeZoneInfo.DaylightName,
                                           timeZoneInfo.StandardName)
                    .Text = $"{dst} {suffixMessage}".Trim
                End If
            End If
        End With

    End Sub

    ''' <summary>
    '''  Loads and deserializes the user settings from JSON file.
    ''' </summary>
    Friend Sub SetUpCareLinkUser()
        Dim path As String = UserSettingsFileWithPath()
        Dim json As String = File.ReadAllText(path)
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(json, options:=s_jsonDeserializerOptions)
    End Sub

    ''' <summary>
    '''  Loads and optionally updates the user settings, prompting the user if necessary.
    ''' </summary>
    ''' <param name="forceUI">
    '''  <see langword="True"/> to force the user interface for updating settings; otherwise, <see langword="False"/>.
    ''' </param>
    '''
    Friend Sub SetUpCareLinkUser(forceUI As Boolean)
        Dim currentUserUpdateNeeded As Boolean = False
        Dim newPdfFile As Boolean = False
        Dim pdfFileNameWithPath As String = UserSettingsPdfFileWithPath

        Dim path As String = UserSettingsFileWithPath()

        If File.Exists(path) Then
            Dim userSettingsJson As String = File.ReadAllText(path)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(
                json:=userSettingsJson,
                options:=s_jsonSerializerOptions)

            If CurrentUser.InsulinRealAit = 0 Then
                CurrentUser.InsulinRealAit = s_insulinTypes.Values(index:=0).AitHours
            End If
            If String.IsNullOrEmpty(CurrentUser.InsulinTypeName) Then
                CurrentUser.InsulinTypeName = s_insulinTypes.Keys(index:=0)
            End If

            If File.Exists(path:=pdfFileNameWithPath) Then
                newPdfFile = Not IsFileReadOnly(path) AndAlso
                    File.GetLastWriteTime(path:=pdfFileNameWithPath) > File.GetLastWriteTime(path)
            End If

            If Not forceUI Then
                If Not newPdfFile Then
                    ' If the PDF file exists and is valid, load it without prompting the user.
                    CurrentPdf = New PdfSettingsRecord(pdfFileNameWithPath)
                    Exit Sub
                End If
            End If
        Else
            Dim useAdvancedAitDecay As CheckState = If(Is700Series(), CheckState.Indeterminate, CheckState.Checked)
            CurrentUser = New CurrentUserRecord(userName:=s_userName, useAdvancedAitDecay)
            currentUserUpdateNeeded = True
        End If

        Form1.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Dim ait As Single = 2
        Dim carbRatios As New List(Of CarbRatioRecord)
        Dim currentTarget As Single = 120

        If Form1.Client?.TryGetDeviceSettingsPdfFile(pdfFileNameWithPath) OrElse
           newPdfFile OrElse
           File.Exists(pdfFileNameWithPath) Then

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
            Using f As New InitializeDialog(ait, currentTarget, carbRatios)
                Dim result As DialogResult = f.ShowDialog(owner:=My.Forms.Form1)
                If result = DialogResult.OK Then
                    currentUserUpdateNeeded = currentUserUpdateNeeded OrElse Not CurrentUser.Equals(f.CurrentUser)
                    CurrentUser = f.CurrentUser.Clone
                End If
            End Using
        End If
        If currentUserUpdateNeeded Then
            File.WriteAllTextAsync(
                path,
                contents:=JsonSerializer.Serialize(value:=CurrentUser, options:=s_jsonSerializerOptions))
        Else
            TouchFile(path)
        End If
        Form1.Cursor = Cursors.Default
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Starts or stops the server update timer.
    ''' </summary>
    ''' <param name="Start">
    '''  <see langword="True"/> to start the timer; <see langword="False"/> to stop it.
    ''' </param>
    ''' <param name="interval">The timer interval in milliseconds. Default is -1 (no change).</param>
    ''' <returns>
    '''  <see langword="True"/> if the timer was running before the call; otherwise, <see langword="False"/>.
    ''' </returns>
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
