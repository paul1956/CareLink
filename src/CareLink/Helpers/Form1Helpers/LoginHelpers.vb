' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module LoginHelpers

    Public Property CurrentPdf As PdfSettingsRecord
    Public ReadOnly Property LoginDialog As New LoginDialog

    ''' <summary>
    '''  Deserializes the patient data element and updates related global variables.
    ''' </summary>
    Friend Sub DeserializePatientElement()
        Try
            Dim options As JsonSerializerOptions = s_jsonDeserializerOptions
            PatientData = JsonSerializer.Deserialize(Of PatientDataInfo)(element:=PatientDataElement, options)
            RecentData = PatientDataElement.ConvertJsonElementToStringDictionary()
        Catch ex As Exception
            MessageBox.Show(
                text:=$"Error deserializing patient data: {ex.Message}",
                caption:="Deserialization Error",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Error)
            Stop
        End Try

        If PatientData.TimeFormat = "HR_12" Then
            s_timeWithMinuteFormat = TimeFormatTwelveHourWithMinutes
            s_timeWithoutMinuteFormat = TimeFormatTwelveHourWithoutMinutes
        Else
            s_timeWithMinuteFormat = TimeFormatMilitaryWithMinutes
            s_timeWithoutMinuteFormat = TimeFormatMilitaryWithoutMinutes
        End If
    End Sub

    ''' <summary>
    '''  Handles optional login and updates user data based on the specified file load option.
    ''' </summary>
    ''' <param name="owner">The main application form.</param>
    ''' <param name="updateAllTabs">
    '''  <see langword="True"/> to update all tabs after login;
    '''  otherwise, <see langword="False"/>.
    ''' </param>
    ''' <param name="fileToLoad">The file load option to use.</param>
    ''' <returns>
    '''  <see langword="True"/> if login and data update succeeded;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function DoOptionalLoginAndUpdateData(
        owner As Form1,
        updateAllTabs As Boolean,
        fileToLoad As FileToLoadOptions) As Boolean

        Dim serverTimerEnabled As Boolean = SetServerUpdateTimer(Start:=False)
        s_autoBasalDeliveryMarkers.Clear()
        ProgramInitialized = False
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.TestData
                owner.Text = $"{SavedTitle} Using Test Data from 'SampleUserV2Data.json'"
                CurrentDateCulture = New CultureInfo(name:="en-US")
                Dim path As String = GetTestDataPath()
                Dim json As String = File.ReadAllText(path)
                PatientDataElement = JsonSerializer.Deserialize(Of JsonElement)(json)
                DeserializePatientElement()
                owner.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(path)
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
                            SetServerUpdateTimer(Start:=serverTimerEnabled)
                            Return False
                        Case DialogResult.Retry
                    End Select
                Loop

                If Form1.Client Is Nothing OrElse Not Form1.Client.LoggedIn Then
                    SetServerUpdateTimer(Start:=True, interval:=FiveMinutesInMilliseconds)

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
                SetServerUpdateTimer(Start:=True, interval:=OneMinuteInMilliseconds)

                If NetworkUnavailable() Then
                    ReportLoginStatus(owner.LoginStatus)
                    Return False
                End If
                ReportLoginStatus(owner.LoginStatus, hasErrors:=RecentDataEmpty, lastErrorMessage)
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
                        fixedPart = BaseDownloadName
                        lastDownloadFileWithPath = GetLastDownloadFileWithPath()
                    Case FileToLoadOptions.Snapshot
                        fixedPart = "CareLink"
                        owner.Text = $"{SavedTitle} Using Snapshot Data"
                        Dim path As String = GetProjectDataDirectory()
                        Dim di As New DirectoryInfo(path)
                        Dim keySelector As Func(Of FileInfo, Date) =
                            Function(f As FileInfo) As Date
                                Return f.LastWriteTime
                            End Function

                        Dim selector As Func(Of FileInfo, String) =
                            Function(f As FileInfo) As String
                                Return f.Name
                            End Function

                        Dim fileList As String() =
                            New DirectoryInfo(path).EnumerateFiles(searchPattern:=$"CareLinkSnapshot*.json") _
                                                   .OrderBy(keySelector) _
                                                   .Select(selector).ToArray

                        Using openFileDialog1 As New OpenFileDialog With {
                            .AddExtension = True,
                            .AddToRecent = False,
                            .CheckFileExists = True,
                            .CheckPathExists = True,
                            .DefaultExt = "json",
                            .Filter = $"json files (*.json)|CareLink*.json",
                            .InitialDirectory = path,
                            .Multiselect = False,
                            .ReadOnlyChecked = True,
                            .RestoreDirectory = True,
                            .ShowPreview = False,
                            .SupportMultiDottedExtensions = False,
                            .Title = $"Select CareLink™ saved snapshot to load",
                            .ValidateNames = True}

                            If openFileDialog1.ShowDialog(owner) = DialogResult.OK Then
                                lastDownloadFileWithPath = openFileDialog1.FileName
                                If Not File.Exists(path:=lastDownloadFileWithPath) Then
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
                CurrentDateCulture = lastDownloadFileWithPath.ExtractCulture(fixedPart, fuzzy:=True)
                Dim json As String = File.ReadAllText(path:=lastDownloadFileWithPath)
                PatientDataElement = JsonSerializer.Deserialize(Of JsonElement)(json)
                DeserializePatientElement()
                owner.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(path:=lastDownloadFileWithPath)
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
    '''  Completes initialization of the <paramref name="mainForm"/>
    '''  after login and data loading.
    ''' </summary>
    ''' <param name="mainForm">The main application form.</param>
    Friend Sub FinishInitialization(mainForm As Form1)
        mainForm.Cursor = Cursors.Default
        mainForm.InitializeSummaryTabCharts()
        mainForm.InitializeActiveInsulinTabChart()
        mainForm.InitializeTimeInRangeArea()
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Determines whether the network is unavailable.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if the network is unavailable;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function NetworkUnavailable() As Boolean
        Return Not My.Computer.Network.IsAvailable
    End Function

    ''' <summary>
    '''  Sets the last update time and time zone information on the main form's status bar.
    ''' </summary>
    ''' <param name="form1">The main application form.</param>
    ''' <param name="msg">The message to display for the last update time.</param>
    ''' <param name="suffixMessage">The suffix message for the time zone label.</param>
    ''' <param name="highLight">
    '''  <see langword="True"/> to highlight the status label;
    '''  otherwise, <see langword="False"/>.
    ''' </param>
    ''' <param name="isDaylightSavingTime">
    '''  <see langword="Nothing"/> if unknown;
    '''  otherwise, <see langword="True"/> or <see langword="False"/>.
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
                .ForeColor = GetGraphLineColor(key:="High Alert")
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
                Const key As String = NameOf(ServerDataEnum.clientTimeZoneName)
                If RecentData?.TryGetValue(key, value:=timeZoneName) Then
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
    '''  Starts or stops the server update timer.
    ''' </summary>
    ''' <param name="Start">
    '''  <see langword="True"/> to start the timer;
    '''  otherwise, <see langword="False"/> to stop it.
    ''' </param>
    ''' <param name="interval">
    '''  The timer interval in milliseconds. Default is -1 (no change).
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the timer was running before the call;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function SetServerUpdateTimer(Start As Boolean, Optional interval As Integer = -1) As Boolean
        GC.Collect()
        GC.WaitForPendingFinalizers()
        ReportMemory()

        If Start Then
            If interval > -1 Then
                Form1.ServerUpdateTimer.Interval = interval
            End If
            Form1.ServerUpdateTimer.Start()
            DebugPrint(message:=$"started at {Now:T}")
            Return True
        Else
            If Form1.ServerUpdateTimer.Enabled Then
                Form1.ServerUpdateTimer.Stop()
                DebugPrint(message:=$"stopped at {Now:T}")
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    '''  Loads and deserializes the user settings from JSON file.
    ''' </summary>
    Friend Sub SetUpCareLinkUser()
        Dim path As String = GetUserSettingsPath()
        Dim json As String = File.ReadAllText(path)
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(json, options:=s_jsonDeserializerOptions)
    End Sub

    ''' <summary>
    '''  Loads and optionally updates the user settings, prompting the user if necessary.
    ''' </summary>
    ''' <param name="forceUI">
    '''  <see langword="True"/> to force the user interface for updating settings;
    '''  otherwise, <see langword="False"/>.
    ''' </param>
    '''
    Friend Sub SetUpCareLinkUser(forceUI As Boolean)
        Dim currentUserUpdateNeeded As Boolean = False
        Dim newPdfFile As Boolean = False

        Dim pdfFilePath As String = GetUserPdfPath()
        If File.Exists(path:=GetUserSettingsPath()) Then
            Dim json As String = File.ReadAllText(path:=GetUserSettingsPath())
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(json, options:=s_jsonSerializerOptions)

            If CurrentUser.InsulinRealAit = 0 Then
                CurrentUser.InsulinRealAit = s_insulinTypes.Values(index:=0).AitHours
            End If
            If String.IsNullOrEmpty(CurrentUser.InsulinTypeName) Then
                CurrentUser.InsulinTypeName = s_insulinTypes.Keys(index:=0)
            End If

            If File.Exists(path:=pdfFilePath) Then
                Dim lastWriteTime As Date =
                    File.GetLastWriteTime(GetUserSettingsPath())
                newPdfFile = Not IsFileReadOnly(path:=GetUserSettingsPath()) AndAlso
                      File.GetLastWriteTime(path:=pdfFilePath) > lastWriteTime
            Else
                While Not File.Exists(path:=pdfFilePath)
                    If MsgBox(
                        heading:=$"No Device Setting PDF file exists!",
                        prompt:="Do you want to load one now, if not the program will exit?",
                        buttonStyle:=MsgBoxStyle.OkCancel,
                        title:="Missing PDF Device Settings File") = MsgBoxResult.Cancel Then
                        End
                    End If

                    Form1.MenuStartHereManuallyImportDeviceSettings.PerformClick()

                    newPdfFile = True
                    Stop
                End While
            End If
            If Not forceUI Then
                If Not newPdfFile Then
                    ' If the PDF file exists and is valid, load it without prompting
                    ' the user.
                    Form1.Cursor = Cursors.WaitCursor
                    Application.DoEvents()
                    CurrentPdf = New PdfSettingsRecord(pdfFilePath)
                End If
            End If
        Else
            Dim useAdvancedAitDecay As CheckState = If(Is700Series(),
                                                       CheckState.Indeterminate,
                                                       CheckState.Checked)

            CurrentUser = New CurrentUserRecord(userName:=s_userName, useAdvancedAitDecay)
            currentUserUpdateNeeded = True
        End If

        Form1.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Dim ait As Single = 2
        Dim carbRatios As New List(Of CarbRatioRecord)
        Dim currentTarget As Single = 120

        If Form1.Client?.TryGetDeviceSettingsPdfFile(pdfFilePath) OrElse
           newPdfFile OrElse
           File.Exists(path:=pdfFilePath) Then

            CurrentPdf = New PdfSettingsRecord(pdfFilePath)

            If CurrentPdf.IsValid Then
                If CurrentUser.PumpAit <> CurrentPdf.Bolus.BolusWizard.ActiveInsulinTime Then
                    currentUserUpdateNeeded = True
                End If
                ait = CurrentPdf.Bolus.BolusWizard.ActiveInsulinTime
                If CurrentUser.CurrentTarget <> CurrentPdf.SmartGuard.Target Then
                    currentUserUpdateNeeded = True
                End If
                currentTarget = CurrentPdf.SmartGuard.Target
                Dim deviceCarbRatios As List(Of DeviceCarbRatioRecord) =
                    CurrentPdf.Bolus.DeviceCarbohydrateRatios

                If Not deviceCarbRatios.EqualCarbRatios(CurrentUser.CarbRatios) Then
                    currentUserUpdateNeeded = True
                End If
                carbRatios = deviceCarbRatios.ToCarbRatioList
            End If
        End If
        If currentUserUpdateNeeded OrElse forceUI Then
            Using f As New InitializeDialog(ait, currentTarget, carbRatios)
                Dim result As DialogResult = f.ShowDialog(owner:=My.Forms.Form1)
                If result = DialogResult.OK Then
                    currentUserUpdateNeeded =
                        currentUserUpdateNeeded OrElse
                        Not CurrentUser.Equals(other:=f.CurrentUser)
                    CurrentUser = f.CurrentUser.Clone
                End If
            End Using
        End If
        If currentUserUpdateNeeded Then
            File.WriteAllTextAsync(
                path:=GetUserSettingsPath(),
                contents:=JsonSerializer.Serialize(value:=CurrentUser, options:=s_jsonSerializerOptions))
        Else
            TouchFile(GetUserSettingsPath())
        End If
        Form1.Cursor = Cursors.Default
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Converts a <see cref="Dictionary(Of String, Object)"/> to a
    '''  <see cref="List(Of KeyValuePair(Of String, String))"/>.
    ''' </summary>
    ''' <param name="dic">The <see cref="Dictionary(Of String, Object)"/> to convert.</param>
    ''' <returns>
    '''  A list of key-value pairs where the value is converted to a <see langword="String"/>.
    ''' </returns>
    <Extension>
    Friend Function ToDataSource(dic As Dictionary(Of String, Object)) As List(Of KeyValuePair(Of String, String))
        Dim dataSource As New List(Of KeyValuePair(Of String, String))
        For Each kvp As KeyValuePair(Of String, Object) In dic
            Dim item As KeyValuePair(Of String, String) = KeyValuePair.Create(kvp.Key, value:=CType(kvp.Value, String))
            dataSource.Add(item)
        Next
        Return dataSource
    End Function

    ''' <summary>
    '''  Converts a PNG <see cref="Bitmap"/> to an <see cref="Icon"/>
    '''  object (with 32x32 size).
    ''' </summary>
    ''' <param name="original">The <see cref="Bitmap"/> to convert.</param>
    ''' <returns>
    '''  An <see cref="Icon"/> object created from the bitmap.
    ''' </returns>
    Public Function PngBitmapToIcon(original As Bitmap) As Icon
        ' Optionally resize to 32x32 for best icon compatibility
        Using resizedBmp As New Bitmap(original, newSize:=New Size(width:=32, height:=32))
            Return Icon.FromHandle(handle:=resizedBmp.GetHicon())
        End Using
    End Function

End Module
