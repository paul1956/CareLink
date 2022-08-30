' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms.DataVisualization.Charting
Imports ToolStripControls

Public Class Form1
    Private WithEvents AITComboBox As ToolStripComboBoxEx
    Private ReadOnly _bgMiniDisplay As New BGMiniWindow
    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _markersAutoBasalDelivery As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersAutoModeStatus As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersBgReading As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersCalibration As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersInsulin As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersLowGlusoseSuspended As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersMeal As New List(Of Dictionary(Of String, String))
    Private ReadOnly _markersTimeChange As New List(Of Dictionary(Of String, String))
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
    Private ReadOnly _updatingLock As New Object
    Private _client As CareLinkClient
    Private _activeInsulinAbsoluteRectangle As RectangleF
    Private _activeInsulinChartRelitivePosition As RectangleF = RectangleF.Empty
    Private _homePageAbsoluteRectangle As RectangleF
    Private _inMouseMove As Boolean = False

    Private _showBaloonTip As Boolean = True

    Private _updating As Boolean

#Region "Pump Data"

    Friend Property RecentData As New Dictionary(Of String, String)

#End Region

    Friend ReadOnly _loginDialog As New LoginForm1

    Private Enum FileToLoadOptions As Integer
        LastSaved = 0
        TestData = 1
        Login = 2
    End Enum

    Private Property FormScale As New SizeF(1.0F, 1.0F)

    Private Property Initialized As Boolean = False

#Region "Chart Objects"

#Region "Charts"

    Private WithEvents ActiveInsulinChart As Chart
    Private WithEvents HomeTabChart As Chart
    Private WithEvents HomeTabTimeInRangeChart As Chart

#End Region

#Region "ChartAreas"

    Private WithEvents ActiveInsulinChartArea As ChartArea
    Public WithEvents HomeTabChartArea As ChartArea
    Private WithEvents TimeInRangeChartArea As ChartArea

#End Region

#Region "Legends"

    Private WithEvents ActiveInsulinChartLegend As Legend

#End Region

#Region "Series"

#Region "Common Series"

    Private WithEvents ActiveInsulinBasalSeries As Series
    Private WithEvents ActiveInsulinBGSeries As Series
    Private WithEvents ActiveInsulinMarkerSeries As Series
    Private WithEvents ActiveInsulinTimeChangeSeries As Series

    Private WithEvents HomeTabBasalSeries As Series
    Private WithEvents HomeTabBGSeries As Series
    Private WithEvents HomeTabMarkerSeries As Series
    Private WithEvents HomeTabTimeChangeSeries As Series

#End Region

    Private WithEvents ActiveInsulinSeries As Series

    Private WithEvents HomeTabHighLimitSeries As Series
    Private WithEvents HomeTabLowLimitSeries As Series
    Private WithEvents HomeTabTimeInRangeSeries As New Series

#End Region

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As Title

#End Region

#End Region

#Region "Events"

#Region "Form Events"

    Private Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        Me.ServerUpdateTimer.Stop()
        Debug.Print($"Me.ServerUpdateTimer stopped at {Now}")
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                Me.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(RepoDownloadName)
                Me.RecentData = Loads(File.ReadAllText(LastDownloadWithPath), BolusRow, InsulinRow, MealRow)
                Me.ShowMiniDisplay.Visible = Debugger.IsAttached
                Me.LastUpdateTime.Text = $"{File.GetLastWriteTime(LastDownloadWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.TestData
                Me.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                Dim testDataWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
                Me.RecentData = Loads(File.ReadAllText(testDataWithPath), BolusRow, InsulinRow, MealRow)
                Me.ShowMiniDisplay.Visible = Debugger.IsAttached
                Me.LastUpdateTime.Text = $"{File.GetLastWriteTime(testDataWithPath).ToShortDateTimeString} from file"
            Case FileToLoadOptions.Login
                Me.Text = SavedTitle
                Do Until _loginDialog.ShowDialog() <> DialogResult.Retry
                Loop
                _client = _loginDialog.Client
                If _client Is Nothing OrElse Not _client.LoggedIn Then
                    Me.ServerUpdateTimer.Interval = s_fiveMinutesInMilliseconds
                    Me.ServerUpdateTimer.Start()
                    If CareLinkClient.NetworkDown Then
                        Me.LoginStatus.Text = "Network Down"
                        Return False
                    End If

                    Me.LastUpdateTime.Text = "Unknown"
                    Return False
                End If
                Me.RecentData = _client.GetRecentData(_loginDialog.LoggedOnUser.CountryCode)
                Me.ServerUpdateTimer.Interval = s_twoMinutesInMilliseconds
                Me.ServerUpdateTimer.Start()
                If CareLinkClient.NetworkDown Then
                    Me.LoginStatus.Text = "Network Down"
                    Return False
                End If
                Me.ShowMiniDisplay.Visible = True
                Debug.Print($"Me.ServerUpdateTimer Started at {Now}")
                Me.LoginStatus.Text = "OK"
        End Select
        Me.FinishInitialization()
        If UpdateAllTabs Then
            Me.AllTabPagesUpdate()
        End If
        Return True
    End Function

    Private Sub FinishInitialization()
        Me.Cursor = Cursors.Default
        Application.DoEvents()

        Me.InitializeHomePageChart()
        Me.InitializeActiveInsulinTabChart()
        Me.InitializeTimeInRangeArea()

        Me.Initialized = True
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.CleanUpNotificationIcon()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.CleanUpNotificationIcon()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        If My.Settings.UpgradeRequired Then
            My.Settings.Upgrade()
            My.Settings.UpgradeRequired = False
            My.Settings.Save()
        End If

        If CareLinkUserDataRecordHelpers.s_allUserSettingsData.Keys.Count = 0 Then
            CareLinkUserDataRecordHelpers.LoadAllUserRecords(Me.CareLinkUserDataRecordBindingSource)
        End If

        AddHandler My.Settings.SettingChanging, AddressOf Me.MySettings_SettingChanging

#If SupportMailServer <> "True" Then
        Me.MenuOptionsSetupEmailServer.Visible = False
#End If
        s_timeZoneList = TimeZoneInfo.GetSystemTimeZones.ToList
        Me.AITComboBox = New ToolStripComboBoxEx With {
            .BackColor = Color.Black,
            .DataSource = New BindingSource(New Dictionary(Of String, String) From {
                {"AIT 2:00", "2:00"}, {"AIT 2:15", "2:15"},
                {"AIT 2:30", "2:30"}, {"AIT 2:45", "2:45"},
                {"AIT 3:00", "3:00"}, {"AIT 3:15", "3:15"},
                {"AIT 3:30", "3:30"}, {"AIT 3:45", "3:45"},
                {"AIT 4:00", "4:00"}, {"AIT 4:15", "4:15"},
                {"AIT 4:30", "4:30"}, {"AIT 4:45", "4:45"},
                {"AIT 5:00", "5:00"}, {"AIT 5:15", "5:15"},
                {"AIT 5:30", "5:30"}, {"AIT 5:45", "5:45"},
                {"AIT 6:00", "6:00"}
            }, Nothing),
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold, GraphicsUnit.Point),
            .ForeColor = Color.White,
            .FormattingEnabled = True,
            .Location = New Point(226, 3),
            .Name = "AITComboBox",
            .SelectedIndex = -1,
            .SelectedItem = Nothing,
            .Size = New Size(78, 23),
            .TabIndex = 0,
            .DisplayMember = "Key",
            .ValueMember = "Value"
        }
        Me.MenuStrip1.Items.Insert(3, Me.AITComboBox)
        Me.AITComboBox.SelectedIndex = Me.AITComboBox.FindStringExact($"AIT {My.Settings.AIT.ToString("hh\:mm").Substring(1)}")
        Me.MenuOptionsUseAdvancedAITDecay.CheckState = If(My.Settings.UseAdvancedAITDecay, CheckState.Checked, CheckState.Unchecked)
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Fix(Me)

        Me.CurrentBG.Parent = Me.CalibrationShieldPictureBox
        Me.ShieldUnitsLabel.Parent = Me.CalibrationShieldPictureBox
        Me.SensorDaysLeftLabel.Parent = Me.SensorTimeLeftPictureBox
        Me.SensorMessage.Parent = Me.CalibrationShieldPictureBox
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        If Me.FormScale.Height > 1 Then
            Me.SplitContainer1.SplitterDistance = 0
        End If
        s_useLocalTimeZone = My.Settings.UseLocalTimeZone
        Me.MenuOptionsUseLocalTimeZone.Checked = s_useLocalTimeZone
        CheckForUpdatesAsync(Me, False)
        If Me.DoOptionalLoginAndUpdateData(False, FileToLoadOptions.Login) Then
            Me.AllTabPagesUpdate()
        End If
    End Sub

#End Region

#Region "Form Menu Events"

#Region "Start Here Menus"

    Private Sub MenuStartHere_DropDownOpened(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpened
        Me.MenuStartHereLoadSavedDataFile.Enabled = Directory.GetFiles(MyDocumentsPath, $"{RepoName}*.json").Length > 0
        Me.MenuStartHereSnapshotSave.Enabled = Me.RecentData IsNot Nothing
        Me.MenuStartHereExceptionReportLoad.Visible = Path.Combine(MyDocumentsPath, $"{RepoErrorReportName}*.txt").Length > 0
    End Sub

    Private Sub MenuStartHereExceptionReportLoad_Click(sender As Object, e As EventArgs) Handles MenuStartHereExceptionReportLoad.Click
        Dim fileList As String() = Directory.GetFiles(MyDocumentsPath, $"{RepoErrorReportName}*.txt")
        Dim openFileDialog1 As New OpenFileDialog With {
            .CheckFileExists = True,
            .CheckPathExists = True,
            .FileName = If(fileList.Length > 0, Path.GetFileName(fileList(0)), RepoName),
            .Filter = $"Error files (*.txt)|{RepoErrorReportName}*.txt",
            .InitialDirectory = MyDocumentsPath,
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .SupportMultiDottedExtensions = False,
            .Title = "Select CareLink saved snapshot to load",
            .ValidateNames = True
        }

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                Dim fileNameWithPath As String = openFileDialog1.FileName
                Me.ServerUpdateTimer.Stop()
                If File.Exists(fileNameWithPath) Then
                    Me.RecentData.Clear()
                    ExceptionHandlerForm.ReportFileNameWithPath = fileNameWithPath
                    If ExceptionHandlerForm.ShowDialog() = DialogResult.OK Then
                        ExceptionHandlerForm.ReportFileNameWithPath = ""
                        Me.RecentData = Loads(ExceptionHandlerForm.LocalRawData, BolusRow, InsulinRow, MealRow)
                        Me.ShowMiniDisplay.Visible = Debugger.IsAttached
                        Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                        Me.LastUpdateTime.Text = $"{File.GetLastWriteTime(fileNameWithPath).ToShortDateTimeString} from file"
                        Me.FinishInitialization()
                        Me.AllTabPagesUpdate()
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.Message}")
            End Try
        End If

    End Sub

    Private Sub MenuStartHereExit_Click(sender As Object, e As EventArgs) Handles StartHereExit.Click
        Me.CleanUpNotificationIcon()
    End Sub

    Private Sub MenuStartHereLoadSavedDataFile_Click(sender As Object, e As EventArgs) Handles MenuStartHereLoadSavedDataFile.Click
        Dim di As New DirectoryInfo(MyDocumentsPath)
        Dim fileList As String() = New DirectoryInfo(MyDocumentsPath).
                                        EnumerateFiles($"{RepoName}*.json").
                                        OrderBy(Function(f As FileInfo) f.LastWriteTime).
                                        Select(Function(f As FileInfo) f.Name).ToArray
        Dim openFileDialog1 As New OpenFileDialog With {
            .CheckFileExists = True,
            .CheckPathExists = True,
            .FileName = If(fileList.Length > 0, fileList.Last, RepoName),
            .Filter = $"json files (*.json)|{RepoName}*.json",
            .InitialDirectory = MyDocumentsPath,
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .SupportMultiDottedExtensions = False,
            .Title = "Select CareLink saved snapshot to load",
            .ValidateNames = True
        }

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                If File.Exists(openFileDialog1.FileName) Then
                    Me.ServerUpdateTimer.Stop()
                    CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"{RepoName}", True)
                    Me.RecentData = Loads(File.ReadAllText(openFileDialog1.FileName), BolusRow, InsulinRow, MealRow)
                    Me.ShowMiniDisplay.Visible = Debugger.IsAttached
                    Me.Text = $"{SavedTitle} Using file {Path.GetFileName(openFileDialog1.FileName)}"
                    Me.LastUpdateTime.Text = File.GetLastWriteTime(openFileDialog1.FileName).ToShortDateTimeString
                    Me.FinishInitialization()
                    Me.AllTabPagesUpdate()
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.Message}")
            End Try
        End If
    End Sub

    Private Sub MenuStartHereLogin_Click(sender As Object, e As EventArgs) Handles MenuStartHereLogin.Click
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, FileToLoadOptions.Login)
    End Sub

    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) Handles MenuStartHereSnapshotSave.Click
        Using jd As JsonDocument = JsonDocument.Parse(Me.RecentData.CleanUserData(), New JsonDocumentOptions)
            File.WriteAllText(GetDataFileName(RepoSnapshotName, CurrentDateCulture.Name, "json", True).withPath, JsonSerializer.Serialize(jd, JsonFormattingOptions))
        End Using
    End Sub

    Private Sub MenuStartHereUseLastSavedFile_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseLastSavedFile.Click
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, FileToLoadOptions.LastSaved)
        Me.MenuStartHereSnapshotSave.Enabled = False
    End Sub

    Private Sub MenuStartHereUseTestData_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseTestData.Click
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, FileToLoadOptions.TestData)
        Me.MenuStartHereSnapshotSave.Enabled = False
    End Sub

#End Region

#Region "Option Menus"

    Private Sub MenuOptionsAutoLogin_CheckChanger(sender As Object, e As EventArgs) Handles MenuOptionsAutoLogin.CheckedChanged
        My.Settings.AutoLogin = Me.MenuOptionsAutoLogin.Checked
    End Sub

    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked
    End Sub

#If SupportMailServer = "True" Then
    Private Sub MenuOptionsSetupEmailServer_Click(sender As Object, e As EventArgs) Handles MenuOptionsSetupEmailServer.Click
        MailSetupDialog.ShowDialog()
    End Sub

#End If

    Private Sub MenuOptionsUseAdvancedAITDecay_CheckStateChanged(sender As Object, e As EventArgs) Handles MenuOptionsUseAdvancedAITDecay.CheckStateChanged
        Dim increments As Double = TimeSpan.Parse(_loginDialog.LoggedOnUser.AIT.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan
        If Me.MenuOptionsUseAdvancedAITDecay.Checked Then
            s_activeInsulinIncrements = CInt(increments * 1.4)
            My.Settings.UseAdvancedAITDecay = True
            Me.AITAlgorithmLabel.Text = "Advanced AIT Decay"
            Me.AITAlgorithmLabel.ForeColor = Color.Yellow
        Else
            s_activeInsulinIncrements = CInt(increments)
            My.Settings.UseAdvancedAITDecay = False
            Me.AITAlgorithmLabel.Text = "Default AIT Decay"
            Me.AITAlgorithmLabel.ForeColor = Color.White
        End If
        My.Settings.Save()
        Me.UpdateActiveInsulinChart()

    End Sub

    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) Handles MenuOptionsUseLocalTimeZone.Click
        Dim useLocalTimeZoneChecked As Boolean = Me.MenuOptionsUseLocalTimeZone.Checked
        My.Settings.UseLocalTimeZone = useLocalTimeZoneChecked
        My.Settings.Save()
        s_clientTimeZone = CalculateTimeZone()
    End Sub

#End Region

#Region "View Menus"

    Private Sub ShowMiniDisplay_Click(sender As Object, e As EventArgs) Handles ShowMiniDisplay.Click
        Me.Hide()
        _bgMiniDisplay.Show()
    End Sub

#End Region

#Region "Help Menus"

    Private Sub MenuHelpAbout_Click(sender As Object, e As EventArgs) Handles MenuHelpAbout.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub MenuHelpCheckForUpdates_Click(sender As Object, e As EventArgs) Handles MenuHelpCheckForUpdates.Click
        CheckForUpdatesAsync(Me, reportResults:=True)
    End Sub

    Private Sub MenuHelpReportAnIssue_Click(sender As Object, e As EventArgs) Handles MenuHelpReportAnIssue.Click
        OpenUrlInBrowser($"{GitHubCareLinkUrl}issues")
    End Sub

#End Region

    Private Sub AITComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AITComboBox.SelectedIndexChanged
        If Me.AITComboBox.SelectedIndex < 1 Then
            Exit Sub
        End If
        Dim aitTimeSpan As TimeSpan = TimeSpan.Parse(Me.AITComboBox.SelectedValue.ToString)
        If My.Settings.AIT <> aitTimeSpan Then
            My.Settings.AIT = aitTimeSpan
            My.Settings.Save()
            s_activeInsulinIncrements = CInt(TimeSpan.Parse(aitTimeSpan.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan)
            Me.UpdateActiveInsulinChart()
        End If
    End Sub

#End Region

#Region "HomePage Tab Events"

    Private Sub TabControlHomePage_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlHomePage.Selecting
        If e.TabPage.Name = NameOf(TabPage14AllUsers) Then
            For Each c As DataGridViewColumn In Me.DataGridViewCareLinkUsers.Columns
                c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                c.HeaderText = If(c.HeaderText.Contains(" "c), c.HeaderText, c.HeaderText.ToTitleCase)
            Next
        End If
    End Sub

#Region "Home Page Chart Events"

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {Now.AddMinutes(s_timeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub

    Private Sub HomePageChart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles HomeTabChart.CursorPositionChanging
        If Not Me.Initialized Then Exit Sub

        Me.CursorTimer.Interval = s_thirtySecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    Private Sub HomePageChart_MouseMove(sender As Object, e As MouseEventArgs) Handles HomeTabChart.MouseMove

        If Not Me.Initialized Then
            Exit Sub
        End If
        _inMouseMove = True
        Dim yInPixels As Double
        Try
            yInPixels = Me.HomeTabChart.ChartAreas(ChartAreaName).AxisY2.ValueToPixelPosition(e.Y)
        Catch ex As Exception
            yInPixels = Double.NaN
        End Try
        If Double.IsNaN(yInPixels) Then
            _inMouseMove = False
            Exit Sub
        End If
        Dim result As HitTestResult
        Try
            result = Me.HomeTabChart.HitTest(e.X, e.Y)
            If result.Series Is Nothing OrElse
                result.PointIndex = -1 Then
                Me.CursorPanel.Visible = False
                Me.CursorTimeLabel.Visible = False
                Exit Sub
            End If
            If result.Series.Points(result.PointIndex).Color = Color.Transparent Then
                Me.CursorPanel.Visible = False
                Me.CursorTimeLabel.Visible = False
                Exit Sub
            End If

            Me.CursorTimeLabel.Left = e.X - (Me.CursorTimeLabel.Width \ 2)
            Select Case result.Series.Name
                Case HighLimitSeriesName,
                     LowLimitSeriesName
                    Me.CursorPanel.Visible = False
                Case MarkerSeriesName, BasalSeriesName
                    Dim markerToolTip() As String = result.Series.Points(result.PointIndex).ToolTip.Split(":"c)
                    If markerToolTip.Length <= 1 Then
                        Me.CursorPanel.Visible = True
                        Exit Sub
                    End If
                    markerToolTip(0) = markerToolTip(0).Trim
                    Dim xValue As Date = Date.FromOADate(result.Series.Points(result.PointIndex).XValue)
                    Me.CursorTimeLabel.Text = xValue.ToString(s_timeWithMinuteFormat)
                    Me.CursorTimeLabel.Tag = xValue
                    Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                    Me.CursorPictureBox.Visible = True
                    Me.CursorTimeLabel.Visible = True
                    Select Case markerToolTip.Length
                        Case 2
                            Me.CursorMessage1Label.Text = markerToolTip(0)
                            Me.CursorMessage1Label.Visible = True
                            Me.CursorMessage2Label.Text = markerToolTip(1).Trim
                            Me.CursorMessage2Label.Visible = True
                            Me.CursorValueLabel.Visible = False
                            Select Case markerToolTip(0)
                                Case "Auto Correction",
                                     "Auto Basal",
                                     "Basal"
                                    Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                Case "Bolus"
                                    Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                Case "Meal"
                                    Me.CursorPictureBox.Image = My.Resources.MealImageLarge
                                Case Else
                                    Stop
                                    Me.CursorMessage1Label.Visible = False
                                    Me.CursorMessage2Label.Visible = False
                                    Me.CursorPictureBox.Image = Nothing
                            End Select
                            Me.CursorPanel.Visible = True
                        Case 3
                            Select Case markerToolTip(1).Trim
                                Case "Calibration accepted",
                                       "Calibration not accepted"
                                    Me.CursorPictureBox.Image = My.Resources.CalibrationDotRed
                                Case "Not used For calibration"
                                    Me.CursorPictureBox.Image = My.Resources.CalibrationDot
                                Case Else
                                    Stop
                            End Select
                            Me.CursorMessage1Label.Text = markerToolTip(0)
                            Me.CursorMessage1Label.Visible = True
                            Me.CursorMessage2Label.Text = markerToolTip(1).Trim
                            Me.CursorMessage2Label.Visible = True
                            Me.CursorValueLabel.Text = markerToolTip(2).Trim
                            Me.CursorValueLabel.Visible = True
                            Me.CursorPanel.Visible = True
                        Case Else
                            Stop
                            Me.CursorPanel.Visible = False
                    End Select
                Case BgSeriesName
                    Me.CursorMessage1Label.Text = $"{result.Series.Points(result.PointIndex).YValues(0).RoundToSingle(3)} {BgUnitsString}"
                    Me.CursorMessage1Label.Visible = True
                    Me.CursorMessage2Label.Visible = False
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorTimeLabel.Text = Date.FromOADate(result.Series.Points(result.PointIndex).XValue).ToString(s_timeWithMinuteFormat)
                    Me.CursorTimeLabel.Visible = True
                    Me.CursorValueLabel.Visible = False
                    Me.CursorPanel.Visible = True
                Case TimeChangeSeriesName
                    Me.CursorMessage1Label.Visible = False
                    Me.CursorMessage1Label.Visible = False
                    Me.CursorMessage2Label.Visible = False
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorTimeLabel.Text = Date.FromOADate(result.Series.Points(result.PointIndex).XValue).ToString(s_timeWithMinuteFormat)
                    Me.CursorTimeLabel.Visible = True
                    Me.CursorValueLabel.Visible = False
                    Me.CursorPanel.Visible = False
                Case Else
                    Stop
            End Select
        Catch ex As Exception
            result = Nothing
        Finally
            _inMouseMove = False
        End Try
    End Sub

    Private Sub SensorAgeLeftLabel_MouseHover(sender As Object, e As EventArgs)
        If s_sensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(Me.CalibrationDueImage, $"Sensor will expire in {s_sensorDurationHours} hours")
        End If
    End Sub

#Region "Post Paint Events"

    <DebuggerNonUserCode()>
    Private Sub ActiveInsulitChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles ActiveInsulinChart.PostPaint

        If Not Me.Initialized OrElse _updating OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            PostPaintSupport(_activeInsulinAbsoluteRectangle, e, InsulinRow, s_activeInsulinMarkerInsulinDictionary, s_activeInsulinMarkerMealDictionary)
        End SyncLock
    End Sub

    <DebuggerNonUserCode()>
    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomeTabChart.PostPaint

        If Not Me.Initialized OrElse _updating OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            PostPaintSupport(_homePageAbsoluteRectangle, e, InsulinRow, s_homeTabMarkerInsulinDictionary, s_homeTabMarkerMealDictionary, Me.CursorTimeLabel)
        End SyncLock
    End Sub

#End Region ' Post Paint Events

#End Region ' Home Page Chart Events

#Region "Home Page DataGridView Events"

#Region "All Users Tab DataGridView Events"

    Private Sub DataGridViewCareLinkUsers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewCareLinkUsers.ColumnAdded
        e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(), wrapHeader:=False)
        If CareLinkUserDataRecordHelpers.HideColumn(e.Column.DataPropertyName) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim cellStyle As DataGridViewCellStyle = CareLinkUserDataRecordHelpers.GetCellStyle()
        e.DgvColumnAdded(cellStyle, wrapHeader:=False)

    End Sub

#End Region ' All Users Tab DataGridView Events
#Region "My User Tab DataGridView Events"
    Private Sub DataGridViewMyUserData_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewMyUserData.ColumnAdded
        e.DgvColumnAdded(MyProfileRecordHelpers.GetCellStyle(), wrapHeader:=False)
        If MyProfileRecordHelpers.HideColumn(e.Column.DataPropertyName) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim cellStyle As DataGridViewCellStyle = MyProfileRecordHelpers.GetCellStyle()
        e.DgvColumnAdded(cellStyle, wrapHeader:=False)

    End Sub

#End Region ' My User Tab DataGridView Events

#Region "Profile Tab DataGridView Events"

    Private Sub DataGridViewProfile_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewMyProfile.ColumnAdded
        e.DgvColumnAdded(MyProfileRecordHelpers.GetCellStyle(), wrapHeader:=False)
        If MyProfileRecordHelpers.HideColumn(e.Column.DataPropertyName) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim cellStyle As DataGridViewCellStyle = MyProfileRecordHelpers.GetCellStyle()
        e.DgvColumnAdded(cellStyle, wrapHeader:=False)

    End Sub

#End Region ' Profile Tab DataGridView Events

#Region "Auto Basal Delivery DataGridView Events"

    Private Sub DataGridViewAutoBasalDelivery_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewAutoBasalDelivery.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(AutoBasalDeliveryRecord.dateTime))
    End Sub

    Private Sub DataGridViewAutoBasalDelivery_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewAutoBasalDelivery.ColumnAdded
        If AutoBasalDeliveryRecordHelpers.HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim cellStyle As DataGridViewCellStyle = AutoBasalDeliveryRecordHelpers.GetCellStyle(e.Column.Name)
        e.DgvColumnAdded(cellStyle, wrapHeader:=False)
    End Sub

#End Region ' Auto Basal Delivery DataGridView Events

#Region "Insulin DataGridView Events"

    Private Sub DataGridViewViewInsulin_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewInsulin.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(InsulinRecord.dateTime))
    End Sub

    Private Sub DataGridViewInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewInsulin.ColumnAdded
        If InsulinRecordHelpers.HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim cellStyle As DataGridViewCellStyle = InsulinRecordHelpers.GetCellStyle(e.Column.Name)
        e.DgvColumnAdded(cellStyle, wrapHeader:=True)
    End Sub

    Private Sub DataGridViewInsulin_ColumnHeaderCellChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewInsulin.ColumnHeaderCellChanged
        Stop
    End Sub

    Private Sub DataGridViewInsulin_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewInsulin.DataError
        Stop
    End Sub

#End Region 'Insulin DataGridView Events

#Region "Report Tab DataGridView Events"

    Private Sub DataGridViewSuportedReports_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        e.DgvColumnAdded(supportedReportRecordHelpers.GetCellStyle(), wrapHeader:=False)
    End Sub

#End Region 'Report Tab DataGridView Events

#Region "SGS Tab DataGridView Events"

    Private Sub SGsDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles SGsDataGridView.CellFormatting
        If e.Value Is Nothing Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        ' Set the background to red for negative values in the Balance column.
        If Me.SGsDataGridView.Columns(e.ColumnIndex).Name.Equals(NameOf(s_sensorState), StringComparison.OrdinalIgnoreCase) Then
            If e.Value.ToString <> "NO_ERROR_MESSAGE" Then
                e.CellStyle.BackColor = Color.Yellow
            End If
        End If
        dgv.dgvCellFormatting(e, "")
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(SgRecord.sg), StringComparison.OrdinalIgnoreCase) Then
            Dim sendorValue As Single = e.Value.ToString().ParseSingle
            If Single.IsNaN(sendorValue) Then
                e.CellStyle.BackColor = Color.Gray
            ElseIf sendorValue < s_limitLow Then
                e.CellStyle.BackColor = Color.Red
            ElseIf sendorValue > s_limitHigh Then
                e.CellStyle.BackColor = Color.Orange
            End If
        End If

    End Sub

    Private Sub SGsDataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles SGsDataGridView.ColumnAdded
        If SgRecordHelpers.HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim cellStyle As DataGridViewCellStyle = SgRecordHelpers.GetCellStyle(e.Column.Name)
        e.DgvColumnAdded(cellStyle, wrapHeader:=False)
    End Sub

#End Region ' SGS Tab DataGridView Events

#Region "Summary Tab DataGridView Events"

    Private Sub SummaryDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles SummaryDataGridView.CellFormatting
        If e.Value Is Nothing OrElse e.ColumnIndex <> 2 Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String = dgv.Rows(e.RowIndex).Cells("key").Value.ToString
        Select Case CType([Enum].Parse(GetType(ItemIndexs), key), ItemIndexs)
            Case ItemIndexs.lastSensorTS
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.medicalDeviceTimeAsString
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.lastSensorTSAsString
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.kind
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.version
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.pumpModelNumber
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.currentServerTime
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.lastConduitTime
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.lastConduitUpdateServerTime
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.firstName
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.lastName
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.conduitSerialNumber
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.conduitBatteryLevel
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.conduitBatteryStatus
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.conduitInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.conduitMedicalDeviceInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.conduitSensorInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.medicalDeviceFamily
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.sensorState
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.medicalDeviceSerialNumber
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.medicalDeviceTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.sMedicalDeviceTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.reservoirLevelPercent
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.reservoirAmount
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.reservoirRemainingUnits
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.medicalDeviceBatteryLevelPercent
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.sensorDurationHours
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.timeToNextCalibHours
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.calibStatus
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.bgUnits
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.timeFormat
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.lastSensorTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.sLastSensorTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.medicalDeviceSuspended
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.lastSGTrend
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.systemStatusMessage
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.averageSG
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.belowHypoLimit
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.aboveHyperLimit
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.timeInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.pumpCommunicationState
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.gstCommunicationState
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.gstBatteryLevel
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.lastConduitDateTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.maxAutoBasalRate
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.maxBolusAmount
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.sensorDurationMinutes
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.timeToNextCalibrationMinutes
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.clientTimeZoneName
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.sgBelowLimit
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.averageSGFloat
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.timeToNextCalibrationRecommendedMinutes
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.calFreeSensor
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.finalCalibration
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case Else
                Stop
        End Select

    End Sub

    Private Sub SummaryDataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles SummaryDataGridView.ColumnAdded
        Dim cellStyle As DataGridViewCellStyle = SummaryRecordHelpers.GetCellStyle(e.Column.Name)
        e.DgvColumnAdded(cellStyle, wrapHeader:=False)
    End Sub

    Private Sub SummaryDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles SummaryDataGridView.DataError
        Stop
    End Sub

#End Region 'Summary Tab DataGridView Events

#End Region 'Home Page DataGridView Events

#End Region ' HomePage Tab Events

#Region "Settings Events"

    Private Sub MySettings_SettingChanging(sender As Object, e As SettingChangingEventArgs)
        Dim newValue As String = If(IsNothing(e.NewValue), "", e.NewValue.ToString)
        If My.Settings(e.SettingName).ToString.ToUpperInvariant.Equals(newValue.ToString.ToUpperInvariant, StringComparison.Ordinal) Then
            Exit Sub
        End If
        If e.SettingName = "CareLinkUserName" Then
            Dim userSettings As New CareLinkUserDataRecord
            If CareLinkUserDataRecordHelpers.s_allUserSettingsData.ContainsKey(e.NewValue.ToString) Then
                _loginDialog.LoggedOnUser = CareLinkUserDataRecordHelpers.s_allUserSettingsData(e.NewValue.ToString)
                Exit Sub
            Else
                userSettings.Update(e.SettingName, e.NewValue.ToString)
                CareLinkUserDataRecordHelpers.s_allUserSettingsData.Add(userSettings.CareLinkUserName, userSettings)
            End If
        End If
        CareLinkUserDataRecordHelpers.SaveAllUserRecords(_loginDialog.LoggedOnUser, e.SettingName, e.NewValue.ToString)
    End Sub

#End Region ' Settings Events

#Region "Timer Events"

    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not Me.HomeTabChartArea.AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.HomeTabChartArea.CursorX.Position = Double.NaN
        End If
    End Sub

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        If Not _updating Then
            Me.ServerUpdateTimer.Stop()
            Me.RecentData = _client.GetRecentData(_loginDialog.LoggedOnUser.CountryCode)
            If Me.RecentData IsNot Nothing Then
                Me.LastUpdateTime.Text = "Unknown"
            Else
                _client = New CareLinkClient(Me.LoginStatus, My.Settings.CareLinkUserName, My.Settings.CareLinkPassword, My.Settings.CountryCode)
                _loginDialog.Client = _client
                Me.RecentData = _client.GetRecentData(_loginDialog.LoggedOnUser.CountryCode)
                If Me.RecentData Is Nothing Then
                    Me.LastUpdateTime.Text = "Unknown due to Login failure, try logging in again!"
                Else
                    Me.LastUpdateTime.Text = Now.ToShortDateTimeString
                End If
            End If
            Me.AllTabPagesUpdate()
            Me.Cursor = Cursors.Default
            Application.DoEvents()
        End If
        Me.ServerUpdateTimer.Interval = s_twoMinutesInMilliseconds
        Me.ServerUpdateTimer.Start()
        Debug.Print($"Me.ServerUpdateTimer Started at {Now}")
    End Sub

#End Region ' Timer Events

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Home Tab Charts"

    Private Sub InitializeHomePageChart()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.SplitContainer3.Panel1.Controls.Add(Me.CursorTimeLabel)
        Me.HomeTabChart = CreateChart(NameOf(HomeTabChart))
        Me.HomeTabChartArea = CreateChartArea()
        With Me.HomeTabChartArea
            With .AxisY2
                .Interval = MealRow
                .IsMarginVisible = False
                .IsStartedFromZero = False
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid = New Grid With {
                        .Interval = MealRow,
                        .LineColor = Color.FromArgb(64, 64, 64, 64)
                    }
                .MajorTickMark = New TickMark() With {.Interval = MealRow, .Enabled = True}
                .Maximum = BolusRow
                .Minimum = MealRow
                .Title = "BG Value"
            End With
        End With
        Me.HomeTabChart.ChartAreas.Add(Me.HomeTabChartArea)

        Dim defaultLegend As Legend = CreateChartLegend(NameOf(defaultLegend))

        Me.HomeTabBasalSeries = CreateBasalSeries()
        Me.HomeTabBGSeries = CreateBgSeries(NameOf(defaultLegend))
        Me.HomeTabMarkerSeries = CreateMarkerSeries()

        Me.HomeTabHighLimitSeries = CreateSeriesLimits(HighLimitSeriesName, ChartAreaName, Color.Orange)
        Me.HomeTabLowLimitSeries = CreateSeriesLimits(LowLimitSeriesName, ChartAreaName, Color.Red)
        Me.HomeTabTimeChangeSeries = CreateTimeChangeSeries()
        Me.SplitContainer3.Panel1.Controls.Add(Me.HomeTabChart)
        Application.DoEvents()
        With Me.HomeTabChart
            With .Series
                .Add(Me.HomeTabBGSeries)
                .Add(Me.HomeTabBasalSeries)
                .Add(Me.HomeTabMarkerSeries)
                .Add(Me.HomeTabHighLimitSeries)
                .Add(Me.HomeTabLowLimitSeries)
                .Add(Me.HomeTabTimeChangeSeries)
            End With
            .Legends.Add(defaultLegend)
            .Series(BgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(BgSeriesName).EmptyPointStyle.Color = Color.Transparent
        End With
        Application.DoEvents()
    End Sub

    Private Sub InitializeTimeInRangeArea()
        If Me.SplitContainer3.Panel2.Controls.Count > 12 Then
            Me.SplitContainer3.Panel2.Controls.RemoveAt(Me.SplitContainer3.Panel2.Controls.Count - 1)
        End If
        Dim width1 As Integer = Me.SplitContainer3.Panel2.Width - 65
        Dim splitPanelMidpoint As Integer = Me.SplitContainer3.Panel2.Width \ 2
        For Each control1 As Control In Me.SplitContainer3.Panel2.Controls
            control1.Left = splitPanelMidpoint - (control1.Width \ 2)
        Next
        Me.HomeTabTimeInRangeChart = New Chart With {
            .Anchor = AnchorStyles.Top,
            .BackColor = Color.Transparent,
            .BackGradientStyle = GradientStyle.None,
            .BackSecondaryColor = Color.Transparent,
            .BorderlineColor = Color.Transparent,
            .BorderlineWidth = 0,
            .Size = New Size(width1, width1)
        }

        With Me.HomeTabTimeInRangeChart
            .BorderSkin.BackSecondaryColor = Color.Transparent
            .BorderSkin.SkinStyle = BorderSkinStyle.None
            Me.TimeInRangeChartArea = New ChartArea With {
                    .Name = NameOf(TimeInRangeChartArea),
                    .BackColor = Color.Black
                }
            .ChartAreas.Add(Me.TimeInRangeChartArea)
            .Location = New Point(Me.TimeInRangeChartLabel.FindHorizontalMidpoint - (.Width \ 2),
                                  CInt(Me.TimeInRangeChartLabel.FindVerticalMidpoint() - Math.Round(.Height / 2.5)))
            .Name = NameOf(HomeTabTimeInRangeChart)
            Me.HomeTabTimeInRangeSeries = New Series With {
                    .ChartArea = NameOf(TimeInRangeChartArea),
                    .ChartType = SeriesChartType.Doughnut,
                    .Name = NameOf(HomeTabTimeInRangeSeries)
                }
            .Series.Add(Me.HomeTabTimeInRangeSeries)
            .Series(NameOf(HomeTabTimeInRangeSeries))("DoughnutRadius") = "17"
        End With

        Me.SplitContainer3.Panel2.Controls.Add(Me.HomeTabTimeInRangeChart)
        Application.DoEvents()
    End Sub

#End Region ' Initialize Home Tab Charts

#Region "Initialize Active Insulin Tab Charts"

    Private Sub InitializeActiveInsulinTabChart()
        Me.TabPage02RunningActiveInsulin.Controls.Clear()

        Me.ActiveInsulinChart = CreateChart(NameOf(ActiveInsulinChart))
        Me.ActiveInsulinChartArea = CreateChartArea()
        With Me.ActiveInsulinChartArea
            With .AxisY
                .MajorTickMark = New TickMark() With {.Interval = MealRow, .Enabled = False}
                .Maximum = 25
                .Minimum = 0
                .Interval = 4
                .Title = "Active Insulin"
                .TitleForeColor = Color.HotPink
            End With
            With .AxisY2
                .Interval = MealRow
                .IsMarginVisible = False
                .IsStartedFromZero = False
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid = New Grid With {
                        .Interval = MealRow,
                        .LineColor = Color.FromArgb(64, 64, 64, 64)
                    }
                .Maximum = BolusRow
                .Minimum = MealRow
                .Title = "BG Value"
            End With
        End With
        Me.ActiveInsulinChart.ChartAreas.Add(Me.ActiveInsulinChartArea)

        Me.ActiveInsulinChartLegend = CreateChartLegend(NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinChart.Legends.Add(Me.ActiveInsulinChartLegend)
        Me.ActiveInsulinSeries = New Series With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = ChartAreaName,
            .ChartType = SeriesChartType.Line,
            .Color = Color.HotPink,
            .Legend = NameOf(ActiveInsulinChartLegend),
            .Name = NameOf(ActiveInsulinSeries),
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinSeries)

        Me.ActiveInsulinBasalSeries = CreateBasalSeries()
        Me.ActiveInsulinBGSeries = CreateBgSeries(NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinMarkerSeries = CreateMarkerSeries()
        Me.ActiveInsulinTimeChangeSeries = CreateTimeChangeSeries()
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinBasalSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinBGSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinMarkerSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinTimeChangeSeries())

        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChart.Series(BgSeriesName).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChart.Series(BgSeriesName).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChartTitle = New Title With {
                .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(26, 59, 105),
                .Name = NameOf(ActiveInsulinChartTitle),
                .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                .ShadowOffset = 3
            }
        Me.ActiveInsulinChart.Titles.Add(Me.ActiveInsulinChartTitle)
        Me.TabPage02RunningActiveInsulin.Controls.Add(Me.ActiveInsulinChart)
        Application.DoEvents()

    End Sub

#End Region

#End Region ' Initialize Charts

#Region "Update Data/Tables"

    Private Shared Function GetLimitsList(count As Integer) As Integer()
        Dim limitsIndexList(count) As Integer
        Dim limitsIndex As Integer = 0
        For i As Integer = 0 To limitsIndexList.GetUpperBound(0)
            If limitsIndex + 1 < s_limits.Count AndAlso CInt(s_limits(limitsIndex + 1)("index")) < i Then
                limitsIndex += 1
            End If
            limitsIndexList(i) = limitsIndex
        Next
        Return limitsIndexList
    End Function

#Region "Update Data and Tables"

    Private Shared Function ScaleOneMarker(innerdic As Dictionary(Of String, String)) As Dictionary(Of String, String)
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

    Private Sub CollectMarkers(row As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Dim recordNumberAutoBasalDelivery As Integer = 0
        Dim recordNumberInsulin As Integer = 0
        Dim newMarker As Dictionary(Of String, String)
        Dim basalDictionary As New Dictionary(Of Double, Single)
        s_maxBasalPerHour = 0
        s_maxBasalPerDose = 0
        For Each innerdic As Dictionary(Of String, String) In LoadList(row)
            Select Case innerdic("type")
                Case "AUTO_BASAL_DELIVERY"
                    newMarker = innerdic
                    _markersAutoBasalDelivery.Add(newMarker)
                    recordNumberAutoBasalDelivery += 1
                    Dim item As New AutoBasalDeliveryRecord(newMarker, recordNumberAutoBasalDelivery)
                    s_bindingSourceMarkersAutoBasalDelivery.Add(item)
                    basalDictionary.Add(item.OADate, item.bolusAmount)
                Case "AUTO_MODE_STATUS"
                    newMarker = innerdic
                    _markersAutoModeStatus.Add(newMarker)
                Case "BG_READING"
                    newMarker = ScaleOneMarker(innerdic)
                    _markersBgReading.Add(newMarker)
                Case "CALIBRATION"
                    newMarker = ScaleOneMarker(innerdic)
                    _markersCalibration.Add(newMarker)
                Case "INSULIN"
                    newMarker = innerdic
                    _markersInsulin.Add(newMarker)
                    recordNumberInsulin += 1
                    Dim item1 As New InsulinRecord(newMarker, recordNumberInsulin)
                    s_bindingSourceMarkersInsulin.Add(item1)
                    Select Case newMarker("activationType")
                        Case "AUTOCORRECTION"
                            basalDictionary.Add(item1.OADate, item1.deliveredFastAmount)
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    newMarker = innerdic
                    _markersLowGlusoseSuspended.Add(newMarker)
                Case "MEAL"
                    newMarker = innerdic
                    _markersMeal.Add(newMarker)
                Case "TIME_CHANGE"
                    newMarker = innerdic
                    _markersTimeChange.Add(newMarker)
                Case Else
                    Stop
                    Throw UnreachableException(memberName, sourceLineNumber)
            End Select
        Next
        Dim endOADate As Double = basalDictionary.Last.Key
        Dim i As Integer = 0
        While i < basalDictionary.Count AndAlso basalDictionary.Keys(i) <= endOADate
            Dim sum As Single = 0
            Dim j As Integer = i
            Dim startOADate As Double = basalDictionary.Keys(j)
            While j < basalDictionary.Count AndAlso basalDictionary.Keys(j) <= startOADate + s_hourAsOADate
                sum += basalDictionary.Values(j)
                j += 1
            End While
            s_maxBasalPerHour = Math.Max(s_maxBasalPerHour, sum)
            s_maxBasalPerDose = Math.Max(s_maxBasalPerDose, basalDictionary.Values(i))
            i += 1
        End While
        Me.MaxBasalPerHour.Text = $"Max Basal/Hr ~ {s_maxBasalPerHour.RoundSingle(3)} U"
        s_markers.AddRange(_markersAutoBasalDelivery)
        s_markers.AddRange(_markersAutoModeStatus)
        s_markers.AddRange(_markersBgReading)
        s_markers.AddRange(_markersCalibration)
        s_markers.AddRange(_markersInsulin)
        s_markers.AddRange(_markersLowGlusoseSuspended)
        s_markers.AddRange(_markersMeal)
        s_markers.AddRange(_markersTimeChange)
    End Sub

    Private Sub ProcessAllSingleEntries(row As KeyValuePair(Of String, String), rowIndex As ItemIndexs, ByRef firstName As String)
        Select Case rowIndex
            Case ItemIndexs.lastSensorTS
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.medicalDeviceTimeAsString
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSensorTSAsString
                If s_bindingSourceSummary.Count < ItemIndexs.lastSensorTSAsString Then
                    s_bindingSourceSummary.Insert(ItemIndexs.lastSensorTS, New SummaryRecord(ItemIndexs.lastSensorTS, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastSensorTS), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastSensorTS), CurrentUICulture))))
                End If
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.kind,
                 ItemIndexs.version
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.pumpModelNumber
                Me.ModelLabel.Text = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.currentServerTime,
               ItemIndexs.lastConduitTime,
               ItemIndexs.lastConduitUpdateServerTime,
               ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.firstName
                firstName = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastName
                Me.FullNameLabel.Text = $"{firstName} {row.Value}"
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitSerialNumber,
                 ItemIndexs.conduitBatteryLevel,
                 ItemIndexs.conduitBatteryStatus
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitInRange
                s_conduitSensorInRange = CBool(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitMedicalDeviceInRange,
                 ItemIndexs.conduitSensorInRange,
                 ItemIndexs.medicalDeviceFamily
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorState
                s_sensorState = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceSerialNumber
                Me.SerialNumberLabel.Text = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceTime
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.sMedicalDeviceTime
                If s_bindingSourceSummary.Count < ItemIndexs.sMedicalDeviceTime Then
                    s_bindingSourceSummary.Add(New SummaryRecord(ItemIndexs.medicalDeviceTime, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.medicalDeviceTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.medicalDeviceTime), CurrentUICulture))))
                End If
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirLevelPercent
                s_reservoirLevelPercent = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirAmount
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirRemainingUnits
                s_reservoirRemainingUnits = row.Value.ParseSingle(0)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceBatteryLevelPercent
                s_medicalDeviceBatteryLevelPercent = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorDurationHours
                s_sensorDurationHours = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeToNextCalibHours
                s_timeToNextCalibHours = CUShort(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.calibStatus
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.bgUnits
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))
                Me.AboveHighLimitMessageLabel.Text = $"Above {s_limitHigh} {BgUnitsString}"
                Me.BelowLowLimitMessageLabel.Text = $"Below {s_limitLow} {BgUnitsString}"

            Case ItemIndexs.timeFormat
                s_timeWithMinuteFormat = If(row.Value = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                s_timeWithoutMinuteFormat = If(row.Value = "HR_12", TwelveHourTimeWithoutMinuteFormat, MilitaryTimeWithoutMinuteFormat)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSensorTime
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.sLastSensorTime
                If s_bindingSourceSummary.Count < ItemIndexs.sLastSensorTime Then
                    s_bindingSourceSummary.Add(New SummaryRecord(ItemIndexs.lastSensorTime, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastSensorTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.medicalDeviceTime), CurrentUICulture))))
                End If
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceSuspended
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSGTrend
                Dim arrows As String = Nothing
                If Trends.TryGetValue(row.Value, arrows) Then
                    Me.LabelTrendArrows.Text = Trends(row.Value)
                Else
                    Me.LabelTrendArrows.Text = $"{row.Value}"
                End If
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.systemStatusMessage
                s_systemStatusMessage = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.averageSG
                s_averageSG = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.belowHypoLimit
                s_belowHypoLimit = row.Value.ParseSingle(1)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.aboveHyperLimit
                s_aboveHyperLimit = row.Value.ParseSingle(1)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeInRange
                s_timeInRange = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.pumpCommunicationState,
             ItemIndexs.gstCommunicationState
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.gstBatteryLevel
                s_gstBatteryLevel = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastConduitDateTime
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastConduitDateTime), CurrentUICulture))))

            Case ItemIndexs.maxAutoBasalRate,
             ItemIndexs.maxBolusAmount
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorDurationMinutes
                s_sensorDurationMinutes = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeToNextCalibrationMinutes
                s_timeToNextCalibrationMinutes = CInt(row.Value)
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.clientTimeZoneName
                s_clientTimeZoneName = row.Value
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sgBelowLimit,
             ItemIndexs.averageSGFloat,
             ItemIndexs.timeToNextCalibrationRecommendedMinutes,
             ItemIndexs.calFreeSensor,
             ItemIndexs.finalCalibration
                s_bindingSourceSummary.Add(New SummaryRecord(rowIndex, row))
            Case Else
                Stop
        End Select
    End Sub

    Private Sub UpdateDataTables(isScaledForm As Boolean)
        If Me.RecentData Is Nothing Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        _markersAutoBasalDelivery.Clear()
        _markersAutoModeStatus.Clear()
        _markersBgReading.Clear()
        _markersCalibration.Clear()
        _markersInsulin.Clear()
        _markersLowGlusoseSuspended.Clear()
        _markersMeal.Clear()
        _markersTimeChange.Clear()
        s_bindingSourceMarkersAutoBasalDelivery.Clear()
        s_bindingSourceMarkersInsulin.Clear()
        s_bindingSourceSGs.Clear()
        s_bindingSourceSummary.Clear()
        s_limits.Clear()
        s_activeInsulinMarkerInsulinDictionary.Clear()
        s_activeInsulinMarkerMealDictionary.Clear()
        s_homeTabMarkerInsulinDictionary.Clear()
        s_homeTabMarkerMealDictionary.Clear()
        s_markers.Clear()

        Dim markerRowString As String = ""
        If Me.RecentData.TryGetValue(ItemIndexs.markers.ToString, markerRowString) Then
            Me.CollectMarkers(markerRowString)
        End If

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In Me.RecentData.WithIndex()
            Dim layoutPanel1 As TableLayoutPanel
            Dim row As KeyValuePair(Of String, String) = c.Value
            Dim rowIndex As ItemIndexs = CType([Enum].Parse(GetType(ItemIndexs), c.Value.Key), ItemIndexs)

            If rowIndex <= ItemIndexs.lastSGTrend OrElse rowIndex >= ItemIndexs.systemStatusMessage Then
                Me.ProcessAllSingleEntries(row, rowIndex, s_firstName)
                Continue For
            End If

            If rowIndex = ItemIndexs.sgs Then
                s_bindingSourceSGs = New BindingList(Of SgRecord)(LoadList(row.Value).ToSgList())
                ProcessListOfDictionary(Me.TableLayoutPanelSgs, Me.SGsDataGridView, s_bindingSourceSGs, rowIndex)
                Me.ReadingsLabel.Text = $"{s_bindingSourceSGs.Where(Function(entry As SgRecord) Not Double.IsNaN(entry.sg)).Count}/288"
                Continue For
            End If

            If rowIndex = ItemIndexs.limits OrElse
                rowIndex = ItemIndexs.markers OrElse
                rowIndex = ItemIndexs.pumpBannerState Then

                Select Case rowIndex
                    Case ItemIndexs.limits
                        For Each innerdic As Dictionary(Of String, String) In LoadList(row.Value)
                            Dim newLimit As New Dictionary(Of String, String)
                            For Each kvp As KeyValuePair(Of String, String) In innerdic
                                Select Case kvp.Key
                                    Case "lowLimit", "highLimit"
                                        newLimit.Add(kvp.Key, kvp.scaleValue(1))
                                    Case Else
                                        newLimit.Add(kvp.Key, kvp.Value)
                                End Select
                            Next
                            s_limits.Add(newLimit)
                        Next
                        ProcessListOfDictionary(Me.TableLayoutPanelLimits, s_limits, rowIndex, Me.FormScale.Height <> 1)
                    Case ItemIndexs.markers
                        ProcessListOfDictionary(Me.TableLayoutPanelAutoBasalDelivery, Me.DataGridViewAutoBasalDelivery, s_bindingSourceMarkersAutoBasalDelivery, rowIndex)
                        ProcessListOfDictionary(Me.TableLayoutPanelAutoModeStatus, _markersAutoModeStatus, rowIndex, Me.FormScale.Height <> 1)
                        ProcessListOfDictionary(Me.TableLayoutPanelBgReading, _markersBgReading, rowIndex, Me.FormScale.Height <> 1)
                        ProcessListOfDictionary(Me.TableLayoutPanelCalibration, _markersCalibration, rowIndex, Me.FormScale.Height <> 1)
                        ProcesListOfDictionary(Me.TableLayoutPanelInsulin, Me.DataGridViewInsulin, s_bindingSourceMarkersInsulin, rowIndex)
                        ProcessListOfDictionary(Me.TableLayoutPanelLowGlusoseSuspended, _markersLowGlusoseSuspended, rowIndex, Me.FormScale.Height <> 1)
                        ProcessListOfDictionary(Me.TableLayoutPanelMeal, _markersMeal, rowIndex, Me.FormScale.Height <> 1)
                        ProcessListOfDictionary(Me.TableLayoutPanelTimeChange, _markersTimeChange, rowIndex, Me.FormScale.Height <> 1)
                    Case ItemIndexs.pumpBannerState
                        If row.Value Is Nothing Then
                            ProcessListOfDictionary(Me.TableLayoutPanelBannerState, New List(Of Dictionary(Of String, String)), rowIndex, Me.FormScale.Height <> 1)
                        Else
                            ProcessListOfDictionary(Me.TableLayoutPanelBannerState, LoadList(row.Value), rowIndex, Me.FormScale.Height <> 1)
                        End If
                End Select
                Continue For
            End If
            Dim docStyle As DockStyle = DockStyle.Fill
            Dim isColumnHeader As Boolean = False
            Select Case rowIndex
                Case ItemIndexs.lastSG
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelTop1, False)
                    s_lastSG = Loads(row.Value, BolusRow, InsulinRow, MealRow)
                    isColumnHeader = False

                Case ItemIndexs.lastAlarm
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelTop2, False)
                    isColumnHeader = False

                Case ItemIndexs.activeInsulin
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelActiveInsulin, True)
                    s_activeInsulin = Loads(row.Value, BolusRow, InsulinRow, MealRow)
                    isColumnHeader = True

                Case ItemIndexs.notificationHistory
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelNotificationHistory, True)
                    isColumnHeader = True

                Case ItemIndexs.therapyAlgorithmState
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelTherapyAlgorthm, True)
                    docStyle = DockStyle.Top
                    isColumnHeader = True

                Case ItemIndexs.basal
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelBasal)
                    docStyle = DockStyle.Fill
                    isColumnHeader = True

                Case Else
                    Stop
                    Throw UnreachableException(NameOf(rowIndex))
            End Select

            Try
                layoutPanel1.SuspendLayout()
                InitializeColumnLabel(layoutPanel1, rowIndex, isColumnHeader)
                If layoutPanel1.RowStyles.Count = 1 Then
                    layoutPanel1.RowStyles(0) = New RowStyle(SizeType.AutoSize, 0)
                Else
                    layoutPanel1.RowStyles(1) = New RowStyle(SizeType.AutoSize, 0)
                End If
                Dim innerJsonDictionary As Dictionary(Of String, String) = Loads(row.Value, BolusRow, InsulinRow, MealRow)
                Dim innerTableBlue As TableLayoutPanel = CreateTableLayoutPanel(NameOf(innerTableBlue), 0, Color.Aqua)
                innerTableBlue.AutoScroll = True
                layoutPanel1.Controls.Add(innerTableBlue,
                                      1,
                                      0)
                GetInnerTable(innerJsonDictionary, innerTableBlue, rowIndex, s_filterJsonData, s_timeWithMinuteFormat, isScaledForm)
                layoutPanel1.ResumeLayout()
            Catch ex As Exception
                Stop
                Throw
            End Try
        Next
        Me.Cursor = Cursors.Default
    End Sub

#End Region ' Update Data and Tables

#End Region ' Update Data/Tables

#Region "Update Home Tab"

    Friend Sub AllTabPagesUpdate()
        Me.SummaryDataGridView.DataSource = s_bindingSourceSummary
        Me.SummaryDataGridView.RowHeadersVisible = False
        If Me.RecentData Is Nothing Then
            Exit Sub
        End If
        If Me.RecentData.Count > ItemIndexs.finalCalibration + 1 Then
            Stop
        End If
        SyncLock _updatingLock
            _updating = True ' prevent paint
            Me.MenuStartHere.Enabled = False
            If Not Me.LastUpdateTime.Text.Contains("from file") Then
                Me.LastUpdateTime.Text = Now.ToShortDateTimeString
            End If
            Me.CursorPanel.Visible = False
            Me.UpdateDataTables(Me.FormScale.Height <> 1 OrElse Me.FormScale.Width <> 1)
            Me.UpdateActiveInsulinChart()
            Me.UpdateActiveInsulin()
            Me.UpdateAutoModeShield()
            Me.UpdateCalibrationTimeRemaining()
            Me.UpdateInsulinLevel()
            Me.UpdatePumpBattery()
            Me.UpdateRemainingInsulin()
            Me.UpdateSensorLife()
            Me.UpdateTimeInRange()
            Me.UpdateTransmitterBatttery()
            Me.UpdateHomeTabSerieses()
            Me.UpdateDosingAndCarbs()
            s_recentDatalast = Me.RecentData
            Me.MenuStartHere.Enabled = True
            _updating = False
        End SyncLock
        Application.DoEvents()
    End Sub

    Private Sub UpdateActiveInsulin(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            Dim activeInsulinStr As String = $"{s_activeInsulin("amount"):N3}"
            Me.ActiveInsulinValue.Text = $"Active Insulin{Environment.NewLine}{activeInsulinStr} U"
            _bgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr}U"
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
    End Sub

    Private Sub UpdateActiveInsulinChart(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        If Not Me.Initialized Then
            Exit Sub
        End If
        Try

            With Me.ActiveInsulinChart
                For Each s As Series In .Series
                    s.Points.Clear()
                Next

                .Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Active Insulin in Pink"
                InitializeChartArea(.ChartAreas(ChartAreaName))
            End With

            ' Order all markers by time
            Dim timeOrderedMarkers As New SortedDictionary(Of Double, Single)
            Dim sgOADateTime As Double

            For Each marker As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
                sgOADateTime = s_markers.SafeGetSgDateTime(marker.Index).RoundTimeDown(RoundTo.Minute).ToOADate
                Select Case marker.Value("type").ToString
                    Case "AUTO_BASAL_DELIVERY"
                        Dim bolusAmount As Single = marker.Value.GetSingleValue("bolusAmount")
                        If timeOrderedMarkers.ContainsKey(sgOADateTime) Then
                            timeOrderedMarkers(sgOADateTime) += bolusAmount
                        Else
                            timeOrderedMarkers.Add(sgOADateTime, bolusAmount)
                        End If
                    Case "AUTO_MODE_STATUS"
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "INSULIN"
                        Dim bolusAmount As Single = marker.Value.GetSingleValue("deliveredFastAmount")
                        If timeOrderedMarkers.ContainsKey(sgOADateTime) Then
                            timeOrderedMarkers(sgOADateTime) += bolusAmount
                        Else
                            timeOrderedMarkers.Add(sgOADateTime, bolusAmount)
                        End If
                    Case "LOW_GLUCOSE_SUSPENDED"
                    Case "MEAL"
                    Case "TIME_CHANGE"
                    Case Else
                        Stop
                End Select
            Next

            ' set up table that holds active insulin for every 5 minutes
            Dim remainingInsulinList As New List(Of ActiveInsulinRecord)
            Dim currentMarker As Integer = 0

            For i As Integer = 0 To 287
                Dim initialBolus As Single = 0
                Dim oaTime As Double = (s_bindingSourceSGs(0).datetime + (s_fiveMinuteSpan * i)).RoundTimeDown(RoundTo.Minute).ToOADate()
                While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= oaTime
                    initialBolus += timeOrderedMarkers.Values(currentMarker)
                    currentMarker += 1
                End While
                remainingInsulinList.Add(New ActiveInsulinRecord(oaTime, initialBolus, s_activeInsulinIncrements, Me.MenuOptionsUseAdvancedAITDecay.Checked))
            Next

            Me.ActiveInsulinChartArea.AxisY2.Maximum = BolusRow

            ' walk all markers, adjust active insulin and then add new marker
            Dim maxActiveInsulin As Double = 0
            For i As Integer = 0 To remainingInsulinList.Count - 1
                If i < s_activeInsulinIncrements Then
                    Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaTime, Double.NaN)
                    Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.Last.IsEmpty = True
                    If i > 0 Then
                        remainingInsulinList.Adjustlist(0, i)
                    End If
                    Continue For
                End If
                Dim startIndex As Integer = i - s_activeInsulinIncrements + 1
                Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, s_activeInsulinIncrements)
                maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                Dim x As Integer = Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaTime, sum)
                remainingInsulinList.Adjustlist(startIndex, s_activeInsulinIncrements)
                Application.DoEvents()
            Next
            Me.ActiveInsulinChartArea.AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
            maxActiveInsulin = Me.ActiveInsulinChartArea.AxisY.Maximum

            Me.ActiveInsulinChart.PlotMarkers(_activeInsulinAbsoluteRectangle, BolusRow, InsulinRow, MealRow, s_activeInsulinMarkerInsulinDictionary, s_activeInsulinMarkerMealDictionary)
            PlotSgSeries(Me.ActiveInsulinChart.Series(BgSeriesName), MealRow)
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateAutoModeShield(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            If s_lastSG("sg") <> "0" Then
                Me.CurrentBG.Visible = True
                Me.CurrentBG.Text = s_lastSG("sg")
                Me.UpdateNotifyIcon()
                _bgMiniDisplay.SetCurrentBGString(s_lastSG("sg"))
                Me.SensorMessage.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield
                Me.ShieldUnitsLabel.Visible = True
                Me.ShieldUnitsLabel.BackColor = Color.Transparent
                Me.ShieldUnitsLabel.Text = BgUnitsString
            Else
                _bgMiniDisplay.SetCurrentBGString("---")
                Me.CurrentBG.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield_Disabled
                Me.ShieldUnitsLabel.Text = BgUnitsString
                Me.SensorMessage.Visible = True
                Me.SensorMessage.BackColor = Color.Transparent
                Dim message As String = ""
                If s_sensorMessages.TryGetValue(s_sensorState, message) Then
                    message = message.ToTitle
                Else
                    MsgBox($"{s_sensorState} is unknown sensor message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                End If
                Me.SensorMessage.Text = message
                Me.ShieldUnitsLabel.Visible = False
                Me.SensorMessage.Visible = True
                Application.DoEvents()
            End If
            If _bgMiniDisplay.Visible Then
                _bgMiniDisplay.BGTextBox.SelectionLength = 0
            End If
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            If s_timeToNextCalibHours = Byte.MaxValue Then
                Me.CalibrationDueImage.Image = My.Resources.CalibrationUnavailable
            ElseIf s_timeToNextCalibHours < 1 Then
                Me.CalibrationDueImage.Image = If(s_systemStatusMessage = "WAIT_TO_CALIBRATE" OrElse s_sensorState = "WARM_UP",
                My.Resources.CalibrationNotReady,
                My.Resources.CalibrationDotRed.DrawCenteredArc(s_timeToNextCalibHours, s_timeToNextCalibrationMinutes / 60))
            Else
                Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(s_timeToNextCalibrationMinutes / 60, s_timeToNextCalibrationMinutes / 60 / 12)
            End If
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try

        Application.DoEvents()
    End Sub

    Private Sub UpdateDosingAndCarbs()
        s_totalAutoCorrection = 0
        s_totalBasal = 0
        s_totalCarbs = 0
        s_totalDailyDose = 0
        s_totalManualBolus = 0

        For Each marker As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Select Case marker.Value("type")
                Case "INSULIN"
                    Dim amountString As String = marker.Value("deliveredFastAmount").TruncateSingleString(3)
                    s_totalDailyDose += amountString.ParseSingle()
                    Select Case marker.Value("activationType")
                        Case "AUTOCORRECTION"
                            s_totalAutoCorrection += amountString.ParseSingle()
                        Case "RECOMMENDED", "UNDETERMINED"
                            s_totalManualBolus += amountString.ParseSingle()
                    End Select

                Case "AUTO_BASAL_DELIVERY"
                    Dim amountString As String = marker.Value("bolusAmount").TruncateSingleString(3)
                    Dim basalAmount As Single = amountString.ParseSingle
                    s_totalBasal += basalAmount
                    s_totalDailyDose += basalAmount
                Case "MEAL"
                    s_totalCarbs += marker.Value("amount").ParseSingle
            End Select
        Next

        Dim totalPercent As String
        If s_totalDailyDose = 0 Then
            totalPercent = "???"
        Else
            totalPercent = $"{CInt(s_totalBasal / s_totalDailyDose * 100)}"
        End If
        Me.BasalLabel.Text = $"Basal {s_totalBasal.RoundSingle(1)} U | {totalPercent}%"

        Me.DailyDoseLabel.Text = $"Daily Dose {s_totalDailyDose.RoundSingle(1)} U"

        If s_totalAutoCorrection > 0 Then
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalAutoCorrection / s_totalDailyDose * 100).ToString
            End If
            Me.AutoCorrectionLabel.Text = $"Auto Correction {s_totalAutoCorrection.RoundSingle(1)} U | {totalPercent}%"
            Me.AutoCorrectionLabel.Visible = True
            Dim totalBolus As Single = s_totalManualBolus + s_totalAutoCorrection
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.ManualBolusLabel.Text = $"Manual Bolus {totalBolus.RoundSingle(1)} U | {totalPercent}%"
        Else
            Me.AutoCorrectionLabel.Visible = False
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.ManualBolusLabel.Text = $"Manual Bolus {s_totalManualBolus.RoundSingle(1)} U | {totalPercent}%"
        End If
        Me.Last24CarbsValueLabel.Text = $"Carbs = {s_totalCarbs} {s_sessionCountrySettings.carbohydrateUnitsDefault.ToTitle}"
    End Sub

    Private Sub UpdateHomeTabSerieses(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            With Me.HomeTabChart
                For Each s As Series In .Series
                    s.Points.Clear()
                Next
                InitializeChartArea(.ChartAreas(ChartAreaName))
                Me.HomeTabChart.PlotMarkers(_homePageAbsoluteRectangle, BolusRow, InsulinRow, MealRow, s_homeTabMarkerInsulinDictionary, s_homeTabMarkerMealDictionary)
            End With
        Catch ex As Exception
            Throw New Exception($"{ex.Message} exception while plotting Markers in {memberName} at {sourceLineNumber}")
        End Try

        Dim limitsIndexList() As Integer = GetLimitsList(s_bindingSourceSGs.Count - 1)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_bindingSourceSGs.WithIndex()
            Dim sgOADateTime As Double = sgListIndex.Value.OADate()
            Try
                Me.HomeTabChart.Series(BgSeriesName).PlotOnePoint(
                                    sgOADateTime,
                                    sgListIndex.Value.sg,
                                    Color.White,
                                    MealRow)
            Catch ex As Exception
                Throw New Exception($"{ex.Message} exception while SG Values in {memberName} at {sourceLineNumber}")
            End Try
            Try
                Dim limitsLowValue As Single = s_limits(limitsIndexList(sgListIndex.Index))("lowLimit").ParseSingle
                Dim limitsHighValue As Single = s_limits(limitsIndexList(sgListIndex.Index))("highLimit").ParseSingle
                If limitsHighValue <> 0 Then
                    Me.HomeTabChart.Series(HighLimitSeriesName).Points.AddXY(sgOADateTime, limitsHighValue)
                End If
                If limitsLowValue <> 0 Then
                    Me.HomeTabChart.Series(LowLimitSeriesName).Points.AddXY(sgOADateTime, limitsLowValue)
                End If
            Catch ex As Exception
                Throw New Exception($"{ex.Message} exception while plotting Limits in {memberName} at {sourceLineNumber}")
            End Try
        Next
    End Sub

    Private Sub UpdateInsulinLevel()
        Select Case s_reservoirLevelPercent
            Case > 85
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(7)
            Case > 71
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(6)
            Case > 57
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(5)
            Case > 43
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(4)
            Case > 29
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(3)
            Case > 15
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(2)
            Case > 1
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(1)
            Case Else
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(0)
        End Select
        Application.DoEvents()
    End Sub

    Private Sub UpdatePumpBattery()
        If Not s_conduitSensorInRange Then
            Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryUnknown
            Me.PumpBatteryRemainingLabel.Text = $"Unknown"
            Exit Sub
        End If

        Select Case s_medicalDeviceBatteryLevelPercent
            Case > 90
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryFull
                Me.PumpBatteryRemainingLabel.Text = $"Full"
            Case > 50
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryHigh
                Me.PumpBatteryRemainingLabel.Text = $"High"
            Case > 25
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryMedium
                Me.PumpBatteryRemainingLabel.Text = $"Medium"
            Case > 10
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryLow
                Me.PumpBatteryRemainingLabel.Text = $"Low"
            Case Else
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryCritical
                Me.PumpBatteryRemainingLabel.Text = $"Critical"
        End Select
    End Sub

    Private Sub UpdateRemainingInsulin(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            Me.RemainingInsulinUnits.Text = $"{s_reservoirRemainingUnits:N1} U"
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
    End Sub

    Private Sub UpdateSensorLife()
        If s_sensorDurationHours = 255 Then
            Me.SensorDaysLeftLabel.Text = $"???"
            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
            Me.SensorTimeLeftLabel.Text = ""
        ElseIf s_sensorDurationHours >= 24 Then
            Me.SensorDaysLeftLabel.Text = Math.Ceiling(s_sensorDurationHours / 24).ToString(CurrentUICulture)
            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeOK
            Me.SensorTimeLeftLabel.Text = $"{Me.SensorDaysLeftLabel.Text} Days"
        Else
            If s_sensorDurationHours = 0 Then
                If s_sensorDurationMinutes = 0 Then
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpired
                    Me.SensorTimeLeftLabel.Text = $"Expired"
                Else
                    Me.SensorDaysLeftLabel.Text = $"1"
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                    Me.SensorTimeLeftLabel.Text = $"{s_sensorDurationMinutes} Minutes"
                End If
            Else
                Me.SensorDaysLeftLabel.Text = $"1"
                Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                Me.SensorTimeLeftLabel.Text = $"{s_sensorDurationHours + 1} Hours"
            End If
        End If
        Me.SensorDaysLeftLabel.Visible = True
    End Sub

    Private Sub UpdateTimeInRange()
        With Me.HomeTabTimeInRangeChart
            With .Series(NameOf(HomeTabTimeInRangeSeries)).Points
                .Clear()
                .AddXY($"{s_aboveHyperLimit}% Above {s_limitHigh} {BgUnitsString}", s_aboveHyperLimit / 100)
                .Last().Color = Color.Orange
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_belowHypoLimit}% Below {s_limitLow} {BgUnitsString}", s_belowHypoLimit / 100)
                .Last().Color = Color.Red
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_timeInRange}% In Range", s_timeInRange / 100)
                .Last().Color = Color.LawnGreen
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
            End With
            .Series(NameOf(HomeTabTimeInRangeSeries))("PieLabelStyle") = "Disabled"
            .Series(NameOf(HomeTabTimeInRangeSeries))("PieStartAngle") = "270"
        End With

        Me.TimeInRangeChartLabel.Text = s_timeInRange.ToString
        Me.TimeInRangeValueLabel.Text = $"{s_timeInRange} %"
        Me.AboveHighLimitValueLabel.Text = $"{s_aboveHyperLimit} %"
        Me.BelowLowLimitValueLabel.Text = $"{s_belowHypoLimit} %"
        Me.AverageSGMessageLabel.Text = $"Average SG in {BgUnitsString}"
        Me.AverageSGValueLabel.Text = If(BgUnitsString = "mg/dl", s_averageSG, s_averageSG.TruncateSingleString(2))

    End Sub

    Private Sub UpdateTransmitterBatttery()
        Me.TransmatterBatterPercentLabel.Text = $"{s_gstBatteryLevel}%"
        If s_conduitSensorInRange Then
            Select Case s_gstBatteryLevel
                Case 100
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryFull
                Case > 50
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryOK
                Case > 20
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryMedium
                Case > 0
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryLow
            End Select
        Else
            Me.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryUnknown
            Me.TransmatterBatterPercentLabel.Text = $"???"
        End If

    End Sub

#End Region ' Update Home Tab

#Region "Scale Split Containers"

    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical, Me.FormScale.Width, Me.FormScale.Height)
        If sp.FixedPanel = FixedPanel.Panel1 Then
            sp.SplitterDistance = CInt(Math.Truncate(Math.Round(sp.SplitterDistance * sc)))
        ElseIf sp.FixedPanel = FixedPanel.Panel2 Then
            Dim cs As Integer = If(sp.Orientation = Orientation.Vertical, sp.Panel2.ClientSize.Width, sp.Panel2.ClientSize.Height)
            Dim newcs As Integer = CInt(Math.Truncate(cs * sc))
            sp.SplitterDistance -= newcs - cs
        End If
    End Sub

    ' Recursively search for SplitContainer controls
    Private Sub Fix(c As Control)
        For Each child As Control In c.Controls
            If TypeOf child Is SplitContainer Then
                Dim sp As SplitContainer = CType(child, SplitContainer)
                Me.Fix(sp)
                Me.Fix(sp.Panel1)
                Me.Fix(sp.Panel2)
            Else
                Me.Fix(child)
            End If
        Next child
    End Sub

    ' Save the current scale value
    ' ScaleControl() is called during the Form'AiTimeInterval constructor
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        Me.FormScale = New SizeF(Me.FormScale.Width * factor.Width, Me.FormScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
    End Sub

#End Region ' Scale Split Containers

#Region "NotifyIcon Support"

    Private Sub CleanUpNotificationIcon()
        If Me.NotifyIcon1 IsNot Nothing Then
            Me.NotifyIcon1.Visible = False
            Me.NotifyIcon1.Icon?.Dispose()
            Me.NotifyIcon1.Icon = Nothing
            Me.NotifyIcon1.Visible = False
            Me.NotifyIcon1.Dispose()
            Application.DoEvents()
        End If
        End
    End Sub

    Private Sub UpdateNotifyIcon()
        Dim str As String = s_lastSG("sg")
        Dim fontToUse As New Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim color As Color = Color.White
        Dim bgColor As Color
        Dim sg As Single = str.ParseSingle
        Dim bitmapText As New Bitmap(16, 16)
        Dim notStr As New StringBuilder

        Using g As Graphics = Graphics.FromImage(bitmapText)
            Select Case sg
                Case <= s_limitLow
                    bgColor = Color.Orange
                    If _showBaloonTip Then
                        Me.NotifyIcon1.ShowBalloonTip(10000, "CareLink Alert", $"SG below {s_limitLow} {BgUnitsString}", Me.ToolTip1.ToolTipIcon)
                    End If
                    _showBaloonTip = False
                Case <= s_limitHigh
                    bgColor = Color.Green
                    _showBaloonTip = True
                Case Else
                    bgColor = Color.Red
                    If _showBaloonTip Then
                        Me.NotifyIcon1.ShowBalloonTip(10000, "CareLink Alert", $"SG above {s_limitHigh} {BgUnitsString}", Me.ToolTip1.ToolTipIcon)
                    End If
                    _showBaloonTip = False
            End Select
            Dim brushToUse As New SolidBrush(color)
            g.Clear(bgColor)
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
            If Math.Floor(Math.Log10(sg) + 1) = 3 Then
                g.DrawString(str, fontToUse, brushToUse, -2, 0)
            Else
                g.DrawString(str, fontToUse, brushToUse, 1.5, 0)
            End If
            Dim hIcon As IntPtr = bitmapText.GetHicon()
            Me.NotifyIcon1.Icon = Icon.FromHandle(hIcon)
            notStr.Append(Date.Now().ToShortDateTimeString.Replace($"{CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator}{Now.Year}", ""))
            notStr.Append(Environment.NewLine)
            notStr.Append($"Last SG {str} {BgUnitsString}")
            If Not s_lastBGValue = 0 Then
                notStr.Append(Environment.NewLine)
                Dim diffsg As Double = sg - s_lastBGValue
                notStr.Append("SG Trend ")
                If diffsg = 0 Then
                    If (Now - s_lastBGTime) < s_fiveMinuteSpan Then
                        diffsg = s_lastBGDiff
                    Else
                        s_lastBGDiff = diffsg
                        s_lastBGTime = Now
                    End If
                Else
                    s_lastBGTime = Now
                    s_lastBGDiff = diffsg
                End If
                Dim formattedTrend As String = diffsg.ToString("+0;-#", CultureInfo.InvariantCulture)
                Me.LabelTrendValue.Text = formattedTrend
                Me.LabelTrendValue.ForeColor = bgColor
                notStr.Append(formattedTrend)
            End If
            notStr.Append(Environment.NewLine)
            notStr.Append("Active ins. ")
            notStr.Append(s_activeInsulin("amount"))
            notStr.Append("U"c)
            Me.NotifyIcon1.Text = notStr.ToString
            s_lastBGValue = sg
            bitmapText.Dispose()
            g.Dispose()
        End Using
    End Sub

#End Region 'NotifyIcon Support

End Class
