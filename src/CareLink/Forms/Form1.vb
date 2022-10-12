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
Imports DataGridViewColumnControls
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
    Private ReadOnly _markersMealRecords As New List(Of MealRecord)
    Private ReadOnly _markersTimeChange As New List(Of Dictionary(Of String, String))
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
    Private ReadOnly _updatingLock As New Object

    Private _activeInsulinChartAbsoluteRectangle As RectangleF = RectangleF.Empty
    Private _formScale As New SizeF(1.0F, 1.0F)
    Private _homePageAbsoluteRectangle As RectangleF
    Private _inMouseMove As Boolean = False
    Private _lastHomeTabIndex As Integer = 0
    Private _lastMarkerTabIndex As Integer = 0
    Private _showBaloonTip As Boolean = True
    Private _treatmentMarkerAbsoluteRectangle As RectangleF
    Private _updating As Boolean

    Public ReadOnly Property client As CareLinkClient
        Get
            Return Me.LoginDialog?.Client
        End Get
    End Property

    Public Property Initialized As Boolean = False
    Public ReadOnly Property LoginDialog As New LoginForm1

#Region "Pump Data"

    Friend Property RecentData As New Dictionary(Of String, String)

#End Region ' Pump Data

#Region "Chart Objects"

#Region "Charts"

    Private WithEvents ActiveInsulinChart As Chart
    Private WithEvents TreatmentMarkersChart As Chart
    Private WithEvents HomeTabChart As Chart
    Private WithEvents HomeTabTimeInRangeChart As Chart

#End Region

#Region "Legends"

    Private WithEvents ActiveInsulinChartLegend As Legend
    Friend WithEvents TreatmentMarkersChartLegend As Legend

#End Region

#Region "Series"

#Region "Common Series"

    Private WithEvents ActiveInsulinBGSeries As Series
    Private WithEvents ActiveInsulinSeries As Series
    Private WithEvents ActiveInsulinTimeChangeSeries As Series

    Private WithEvents HomeTabBasalSeries As Series
    Private WithEvents HomeTabBGSeries As Series
    Private WithEvents HomeTabHighLimitSeries As Series
    Private WithEvents HomeTabLowLimitSeries As Series
    Private WithEvents HomeTabMarkerSeries As Series
    Private WithEvents HomeTabTimeChangeSeries As Series
    Private WithEvents HomeTabTimeInRangeSeries As New Series

    Private WithEvents TreatmentMarkerBGSeries As Series
    Private WithEvents TreatmentMarkerBasalSeries As Series
    Private WithEvents TreatmentMarkerMarkersSeries As Series
    Private WithEvents TreatmentMarkerTimeChangeSeries As Series

#End Region

#End Region

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As Title
    Private WithEvents TreatmentMarkersChartTitle As Title
    Private _client As CareLinkClient

#End Region

#End Region ' Chart Objects

#Region "Events"

#Region "Form Events"

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

        s_allUserSettingsData.LoadUserRecords()

        AddHandler My.Settings.SettingChanging, AddressOf Me.MySettings_SettingChanging

#If SupportMailServer <> "True" Then
        Me.MenuOptionsSetupEmailServer.Visible = False
#End If
        s_timeZoneList = TimeZoneInfo.GetSystemTimeZones.ToList
        Me.AITComboBox = New ToolStripComboBoxEx With {
            .BackColor = Color.Black,
            .DataSource = s_aitItemsBindingSource,
            .DisplayMember = "Key",
            .ValueMember = "Value",
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold, GraphicsUnit.Point),
            .ForeColor = Color.White,
            .FormattingEnabled = True,
            .Location = New Point(226, 3),
            .Name = "AITComboBox",
            .SelectedIndex = -1,
            .SelectedItem = Nothing,
            .Size = New Size(78, 23),
            .TabIndex = 0
        }

        With Me.CareLinkUsersAITComboBox
            .DataSource = s_aitItemsBindingSource
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold, GraphicsUnit.Point)
            .ForeColor = Color.White
            .FormattingEnabled = True
            .Size = New Size(78, 23)
            .DisplayMember = "Key"
            .ValueMember = "Value"
        End With

        Me.MenuStrip1.Items.Insert(2, Me.AITComboBox)
        Me.AITComboBox.SelectedIndex = Me.AITComboBox.FindStringExact($"AIT {My.Settings.AIT.ToString("hh\:mm").Substring(1)}")
        Me.MenuOptionsUseAdvancedAITDecay.CheckState = If(My.Settings.UseAdvancedAITDecay, CheckState.Checked, CheckState.Unchecked)
        AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf Me.PowerModeChanged
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Fix(Me)

        Me.CurrentBGLabel.Parent = Me.CalibrationShieldPictureBox
        Me.ShieldUnitsLabel.Parent = Me.CalibrationShieldPictureBox
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Parent = Me.SensorTimeLeftPictureBox
        Me.SensorMessage.Parent = Me.CalibrationShieldPictureBox
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        s_useLocalTimeZone = My.Settings.UseLocalTimeZone
        Me.MenuOptionsUseLocalTimeZone.Checked = s_useLocalTimeZone
        CheckForUpdatesAsync(Me, False)
        If Me.DoOptionalLoginAndUpdateData(False, FileToLoadOptions.Login) Then
            Me.AllTabPagesUpdate()
        End If
    End Sub

#End Region ' Form Events

#Region "Form Menu Events"

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

#Region "Start Here Menu Events"

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
                Debug.Print($"In {NameOf(MenuStartHereExceptionReportLoad_Click)}, {NameOf(Me.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
                If File.Exists(fileNameWithPath) Then
                    Me.RecentData.Clear()
                    ExceptionHandlerForm.ReportFileNameWithPath = fileNameWithPath
                    If ExceptionHandlerForm.ShowDialog() = DialogResult.OK Then
                        ExceptionHandlerForm.ReportFileNameWithPath = ""
                        Me.RecentData = Loads(ExceptionHandlerForm.LocalRawData)
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
                    Debug.Print($"In {NameOf(MenuStartHereLoadSavedDataFile_Click)}, {NameOf(Me.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
                    CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"{RepoName}", True)
                    Me.RecentData = Loads(File.ReadAllText(openFileDialog1.FileName))
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
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, fileToLoad:=FileToLoadOptions.Login)
    End Sub

    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) Handles MenuStartHereSnapshotSave.Click
        Using jd As JsonDocument = JsonDocument.Parse(Me.RecentData.CleanUserData(), New JsonDocumentOptions)
            File.WriteAllText(GetDataFileName(RepoSnapshotName, CurrentDateCulture.Name, "json", True).withPath, JsonSerializer.Serialize(jd, JsonFormattingOptions))
        End Using
    End Sub

    Private Sub MenuStartHereUseLastSavedFile_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseLastSavedFile.Click
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, fileToLoad:=FileToLoadOptions.LastSaved)
        Me.MenuStartHereSnapshotSave.Enabled = False
    End Sub

    Private Sub MenuStartHereUseTestData_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseTestData.Click
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, fileToLoad:=FileToLoadOptions.TestData)
        Me.MenuStartHereSnapshotSave.Enabled = False
    End Sub

#End Region ' Start Here Menu Events

#Region "Option Menus"

    Private Sub MenuOptionsAutoLogin_CheckChanger(sender As Object, e As EventArgs) Handles MenuOptionsAutoLogin.CheckedChanged
        My.Settings.AutoLogin = Me.MenuOptionsAutoLogin.Checked
    End Sub

    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked
        For Each c As DataGridViewColumn In Me.DataGridViewAutoBasalDelivery.Columns
            c.Visible = Not AutoBasalDeliveryRecordHelpers.HideColumn(c.DataPropertyName)
        Next

        For Each c As DataGridViewColumn In Me.DataGridViewCareLinkUsers.Columns
            c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
        Next

        For Each c As DataGridViewColumn In Me.DataGridViewInsulin.Columns
            c.Visible = Not InsulinRecordHelpers.HideColumn(c.DataPropertyName)
        Next

        For Each c As DataGridViewColumn In Me.DataGridViewMeal.Columns
            c.Visible = Not MealRecordHelpers.HideColumn(c.DataPropertyName)
        Next

        For Each c As DataGridViewColumn In Me.DataGridViewSGs.Columns
            c.Visible = Not SgRecordHelpers.HideColumn(c.DataPropertyName)
        Next
    End Sub

#If SupportMailServer = "True" Then
    Private Sub MenuOptionsSetupEmailServer_Click(sender As Object, e As EventArgs) Handles MenuOptionsSetupEmailServer.Click
        MailSetupDialog.ShowDialog()
    End Sub

#End If

    Private Sub MenuOptionsUseAdvancedAITDecay_CheckStateChanged(sender As Object, e As EventArgs) Handles MenuOptionsUseAdvancedAITDecay.CheckStateChanged
        Dim increments As Double = TimeSpan.Parse(_LoginDialog.LoggedOnUser.AIT.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan
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

#End Region ' Option Menus

#Region "View Menu Events"

    Private Sub ShowMiniDisplay_Click(sender As Object, e As EventArgs) Handles ShowMiniDisplay.Click
        Me.Hide()
        _bgMiniDisplay.Show()
    End Sub

#End Region ' View Menu Events

#Region "Help Menu Events"

    Private Sub MenuHelpAbout_Click(sender As Object, e As EventArgs) Handles MenuHelpAbout.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub MenuHelpCheckForUpdates_Click(sender As Object, e As EventArgs) Handles MenuHelpCheckForUpdates.Click
        CheckForUpdatesAsync(Me, reportResults:=True)
    End Sub

    Private Sub MenuHelpReportAnIssue_Click(sender As Object, e As EventArgs) Handles MenuHelpReportAnIssue.Click
        OpenUrlInBrowser($"{GitHubCareLinkUrl}issues")
    End Sub

#End Region ' Help Menu Events

#End Region 'Form Menu Events

#Region "HomePage Tab Events"

    Private Sub TabControlHomePage_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlHomePage.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPageAllLocalUsers)
                Me.DataGridViewCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DataGridViewCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
                Me.CareLinkUsersAITComboBox.Width = Me.AITComboBox.Width
                Me.CareLinkUsersAITComboBox.SelectedIndex = Me.AITComboBox.SelectedIndex
                Me.CareLinkUsersAITComboBox.Visible = False
                Me.DataGridViewCareLinkUsers.Columns(NameOf(DataGridViewTextBoxColumnCareLinkAIT)).Width = Me.AITComboBox.Width
            Case NameOf(TabPage16Markers)
                Me.TabControlPage2.SelectedIndex = _lastMarkerTabIndex
                Me.TabControlHomePage.Visible = False
                Exit Sub
        End Select
        _lastHomeTabIndex = e.TabPageIndex
    End Sub

    Private Sub TabControlMarkers_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage2.Selecting
        Select Case e.TabPage.Name
            Case NameOf(TabPageBackToHomePage)
                Me.TabControlHomePage.SelectedIndex = _lastHomeTabIndex
                Me.TabControlHomePage.Visible = True
                Exit Sub
            Case NameOf(TabPageAllLocalUsers)
                Me.DataGridViewCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DataGridViewCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
                Me.CareLinkUsersAITComboBox.Width = Me.AITComboBox.Width
                Me.CareLinkUsersAITComboBox.SelectedIndex = Me.AITComboBox.SelectedIndex
                Me.CareLinkUsersAITComboBox.Visible = False
                Me.DataGridViewCareLinkUsers.Columns(NameOf(DataGridViewTextBoxColumnCareLinkAIT)).Width = Me.AITComboBox.Width
        End Select
        _lastMarkerTabIndex = e.TabPageIndex
    End Sub

#End Region ' HomePage Tab Events

#Region "Home Page Events"

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {Now.AddMinutes(s_timeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub

    Private Sub HomePageChart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles HomeTabChart.CursorPositionChanging
        If Not _Initialized Then Exit Sub

        Me.CursorTimer.Interval = s_thirtySecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    Private Sub HomePageChart_MouseMove(sender As Object, e As MouseEventArgs) Handles HomeTabChart.MouseMove

        If Not _Initialized Then
            Exit Sub
        End If
        _inMouseMove = True
        Dim yInPixels As Double
        Try
            yInPixels = Me.HomeTabChart.ChartAreas(NameOf(ChartArea)).AxisY2.ValueToPixelPosition(e.Y)
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
                Exit Sub
            End If

            Dim currentDataPoint As DataPoint = result.Series.Points(result.PointIndex)

            If currentDataPoint.IsEmpty OrElse currentDataPoint.Color = Color.Transparent Then
                Me.CursorPanel.Visible = False
                Exit Sub
            End If

            Select Case result.Series.Name
                Case HighLimitSeriesName,
                     LowLimitSeriesName
                    Me.CursorPanel.Visible = False
                Case MarkerSeriesName, BasalSeriesName
                    Dim markerToolTip() As String = currentDataPoint.ToolTip.Split(":"c)
                    If markerToolTip.Length <= 1 Then
                        Me.CursorPanel.Visible = True
                        Exit Sub
                    End If
                    markerToolTip(0) = markerToolTip(0).Trim
                    Dim xValue As Date = Date.FromOADate(currentDataPoint.XValue)
                    Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                    Me.CursorPictureBox.Visible = True
                    Select Case markerToolTip.Length
                        Case 2
                            Me.CursorMessage1Label.Text = markerToolTip(0)
                            Me.CursorMessage1Label.Visible = True
                            Me.CursorMessage2Label.Text = markerToolTip(1).Trim
                            Me.CursorMessage2Label.Visible = True
                            Me.CursorMessage3Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(s_timeWithMinuteFormat)
                            Me.CursorMessage3Label.Visible = True
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
                            Me.CursorMessage3Label.Text = $"{markerToolTip(2).Trim}@{xValue.ToString(s_timeWithMinuteFormat)}"
                            Me.CursorMessage3Label.Visible = True
                            Me.CursorPanel.Visible = True
                        Case Else
                            Stop
                            Me.CursorPanel.Visible = False
                    End Select
                Case BgSeriesName
                    Me.CursorMessage1Label.Text = "Blood Glucose"
                    Me.CursorMessage1Label.Visible = True
                    Me.CursorMessage2Label.Text = $"{currentDataPoint.YValues(0).RoundToSingle(3)} {BgUnitsString}"
                    Me.CursorMessage2Label.Visible = True
                    Me.CursorMessage3Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(s_timeWithMinuteFormat)
                    Me.CursorMessage3Label.Visible = True
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorPanel.Visible = True
                Case TimeChangeSeriesName
                    Me.CursorMessage1Label.Visible = False
                    Me.CursorMessage1Label.Visible = False
                    Me.CursorMessage2Label.Visible = False
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorMessage3Label.Visible = False
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
    Private Sub ActiveInsulinChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles ActiveInsulinChart.PostPaint

        If Not _Initialized OrElse _inMouseMove Then
            Exit Sub
        End If
        Debug.Print($"In {NameOf(ActiveInsulinChart_PostPaint)} before SyncLock")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(ActiveInsulinChart_PostPaint)} in SyncLock")
            If _updating Then
                Debug.Print($"Exiting {NameOf(ActiveInsulinChart_PostPaint)} due to {NameOf(_updating)}")
                Exit Sub
            End If
            e.PostPaintSupport(_activeInsulinChartAbsoluteRectangle,
                Nothing,
                Nothing,
                True,
                True)
        End SyncLock
        Debug.Print($"In {NameOf(ActiveInsulinChart_PostPaint)} exited SyncLock")
    End Sub

    '<DebuggerNonUserCode()>
    Private Sub TreatmentMarkersChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles TreatmentMarkersChart.PostPaint

        If Not _Initialized OrElse _inMouseMove Then
            Exit Sub
        End If
        Debug.Print($"In {NameOf(TreatmentMarkersChart_PostPaint)} before SyncLock")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(TreatmentMarkersChart_PostPaint)} in SyncLock")
            If _updating Then
                Debug.Print($"Exiting {NameOf(TreatmentMarkersChart_PostPaint)} due to {NameOf(_updating)}")
                Exit Sub
            End If
            e.PostPaintSupport(_treatmentMarkerAbsoluteRectangle,
                s_treatmentMarkerInsulinDictionary,
                s_treatmentMarkerMealDictionary,
                offsetInsulinImage:=False,
                paintOnY2:=False)
        End SyncLock
        Debug.Print($"In {NameOf(TreatmentMarkersChart_PostPaint)} exited SyncLock")
    End Sub

    <DebuggerNonUserCode()>
    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomeTabChart.PostPaint

        If Not _Initialized OrElse _inMouseMove Then
            Exit Sub
        End If
        Debug.Print($"In {NameOf(HomePageChart_PostPaint)} before SyncLock")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(HomePageChart_PostPaint)} in SyncLock")
            If _updating Then
                Debug.Print($"Exiting {NameOf(HomePageChart_PostPaint)} due to {NameOf(_updating)}")
                Exit Sub
            End If
            e.PostPaintSupport(_homePageAbsoluteRectangle,
                s_homeTabMarkerInsulinDictionary,
                s_homeTabMarkerMealDictionary,
                True,
                True)
        End SyncLock
        Debug.Print($"In {NameOf(HomePageChart_PostPaint)} exited SyncLock")
    End Sub

#End Region ' Post Paint Events

#End Region ' Home Page Events

#Region "Home Page DataGridView Events"

#Region "All Users Tab DataGridView Events"

    Private Sub DataGridViewCareLinkUsers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridViewCareLinkUsers.CellBeginEdit
        Dim dgv As DataGridView = CType(sender, DataGridView)
        'Here we save a current value of cell to some variable, that later we can compare with a new value
        'For example using of dgv.Tag property
        If e.RowIndex >= 0 AndAlso e.ColumnIndex > 0 Then
            dgv.Tag = dgv.CurrentCell.Value.ToString
        End If
        'If dgv.Columns(e.ColumnIndex).DataPropertyName = NameOf(CareLinkUserDataRecord.AIT) Then
        '    Me.CareLinkUsersAITComboBox.Visible = True
        'End If

    End Sub

    Private Sub CareLinkUserDataRecordHelpers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewCareLinkUsers.CellContentClick
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.ColumnIndex = 0 Then
            If Not CType(dgv.Rows(e.RowIndex).Cells(0), DataGridViewDisableButtonCell).Enabled Then
                Exit Sub
            End If

            dgv.DataSource = Nothing
            s_allUserSettingsData.RemoveAt(e.RowIndex)
            dgv.DataSource = s_allUserSettingsData
            s_allUserSettingsData.SaveAllUserRecords()
        End If

    End Sub

    Private Sub DataGridViewCareLinkUsers_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridViewCareLinkUsers.CellValidating
        If e.ColumnIndex = 0 Then
            Exit Sub
        End If
        'For example used Integer check
        'Dim iTemp As Integer
        'If Integer.TryParse(dgv.CurrentCell.Value, iTemp) = True AndAlso iTemp > 0 Then
        '    'value is OK
        'Else
        '    e.Cancel = True
        'End If

    End Sub

    Private Sub DataGridViewCareLinkUsers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewCareLinkUsers.CellEndEdit
        'after you've filled your ds, on event above try something like this
        Try
            '
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub DataGridViewCareLinkUsers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewCareLinkUsers.ColumnAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable)?.Columns(e.Column.Index).Caption
        If CareLinkUserDataRecordHelpers.HideColumn(e.Column.DataPropertyName) Then
            e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(e.Column.DataPropertyName),
                             False,
                             False,
                             caption)
            e.Column.Visible = False
            Exit Sub
        End If
        e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(e.Column.DataPropertyName),
                         False,
                         True,
                         caption)

    End Sub

    Private Sub DataGridViewCareLinkUsers_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewCareLinkUsers.DataError
        Stop
    End Sub

    Private Sub DataGridViewCareLinkUsers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridViewCareLinkUsers.RowsAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim disableButtonCell As DataGridViewDisableButtonCell = CType(dgv.Rows(e.RowIndex).Cells(NameOf(DataGridViewButtonColumnCareLinkDeleteRow)), DataGridViewDisableButtonCell)
        disableButtonCell.Enabled = s_allUserSettingsData(e.RowIndex).CareLinkUserName <> _LoginDialog.LoggedOnUser.CareLinkUserName
    End Sub

#End Region ' All Users Tab DataGridView Events

#Region "My User Tab DataGridView Events"

    Private Sub DataGridViewMyUserData_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewMyUserData.ColumnAdded
        e.DgvColumnAdded(New DataGridViewCellStyle().CellStyleMiddleLeft,
                         False,
                         True,
                         Nothing)

    End Sub

    Private Sub DataGridViewMyUserData_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewMyUserData.DataError
        Stop
    End Sub

#End Region ' My User Tab DataGridView Events

#Region "Profile Tab DataGridView Events"

    Private Sub DataGridViewProfile_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewMyProfile.ColumnAdded
        e.DgvColumnAdded(New DataGridViewCellStyle().CellStyleMiddleLeft,
                         False,
                         True,
                         Nothing)

    End Sub

    Private Sub DataGridViewMyProfile_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewMyProfile.DataError
        Stop
    End Sub

#End Region ' Profile Tab DataGridView Events

#Region "Auto Basal Delivery DataGridView Events"

    Private Sub DataGridViewAutoBasalDelivery_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewAutoBasalDelivery.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.Value Is Nothing Then
            Return
        End If
        ' Set the background to red for negative values in the Balance column.
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(AutoBasalDeliveryRecord.bolusAmount), StringComparison.OrdinalIgnoreCase) Then
            If e.Value.ToString = "0.025" Then
                e.CellStyle.BackColor = Color.LightYellow
            Else
                e.Value = CSng(e.Value).ToString("F3", CurrentUICulture)
            End If
        End If
        dgv.dgvCellFormatting(e, NameOf(AutoBasalDeliveryRecord.dateTime))
    End Sub

    Private Sub DataGridViewAutoBasalDelivery_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewAutoBasalDelivery.ColumnAdded
        If AutoBasalDeliveryRecordHelpers.HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(AutoBasalDeliveryRecordHelpers.GetCellStyle(e.Column.Name),
                         False,
                         True,
                         caption)
    End Sub

#End Region ' Auto Basal Delivery DataGridView Events

#Region "Insulin DataGridView Events"

    Private Sub DataGridViewViewInsulin_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewInsulin.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(InsulinRecord.dateTime))
    End Sub

    Private Sub DataGridViewInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewInsulin.ColumnAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        If InsulinRecordHelpers.HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        e.DgvColumnAdded(InsulinRecordHelpers.GetCellStyle(e.Column.Name),
                         True,
                         True, caption)
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
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index - 1).Caption
        e.DgvColumnAdded(supportedReportRecordHelpers.GetCellStyle(),
                         False,
                         True,
                         caption)
    End Sub

#End Region 'Report Tab DataGridView Events

#Region "SGS Tab DataGridView Events"

    Private Sub DataGridViewSGs_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewSGs.CellFormatting
        If e.Value Is Nothing Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        ' Set the background to red for negative values in the Balance column.
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(s_sensorState), StringComparison.OrdinalIgnoreCase) Then
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

    Private Sub DataGridViewSGs_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewSGs.ColumnAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        If SgRecordHelpers.HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        e.DgvColumnAdded(SgRecordHelpers.GetCellStyle(e.Column.Name),
                         False,
                         True,
                         caption)

    End Sub

#End Region ' SGS Tab DataGridView Events

#Region "Summary Tab DataGridView Events"

    Private Sub SummaryDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewSummary.CellFormatting
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
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.version
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.pumpModelNumber
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.currentServerTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.lastConduitTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.lastConduitUpdateServerTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.firstName
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.lastName
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.conduitSerialNumber
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.conduitBatteryLevel
                e.CellStyle = e.CellStyle.CellStyleMiddleRight(0)
            Case ItemIndexs.conduitBatteryStatus
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.conduitInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.conduitMedicalDeviceInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.conduitSensorInRange
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.medicalDeviceFamily
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.sensorState
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.medicalDeviceSerialNumber
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
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
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.bgUnits
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.timeFormat
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.lastSensorTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.sLastSensorTime
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.medicalDeviceSuspended
                e.CellStyle = e.CellStyle.CellStyleMiddleCenter
            Case ItemIndexs.lastSGTrend
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
            Case ItemIndexs.systemStatusMessage
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
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
                e.CellStyle = e.CellStyle.CellStyleMiddleLeft
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

    Private Sub SummaryDataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewSummary.ColumnAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(SummaryRecordHelpers.GetCellStyle(e.Column.Name),
                         False,
                         True,
                         caption)
    End Sub

    Private Sub SummaryDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewSummary.DataError
        Stop
    End Sub

#End Region 'Summary Tab DataGridView Events

#End Region 'Home Page DataGridView Events

#Region "Settings Events"

    Private Sub MySettings_SettingChanging(sender As Object, e As SettingChangingEventArgs)
        Dim newValue As String = If(IsNothing(e.NewValue), "", e.NewValue.ToString)
        If My.Settings(e.SettingName).ToString.ToUpperInvariant.Equals(newValue.ToString.ToUpperInvariant, StringComparison.Ordinal) Then
            Exit Sub
        End If
        If e.SettingName = "CareLinkUserName" Then
            If s_allUserSettingsData?.ContainsKey(e.NewValue.ToString) Then
                _LoginDialog.LoggedOnUser = s_allUserSettingsData(e.NewValue.ToString)
                Exit Sub
            Else
                Dim userSettings As New CareLinkUserDataRecord(s_allUserSettingsData)
                userSettings.UpdateValue(e.SettingName, e.NewValue.ToString)
                s_allUserSettingsData.Add(userSettings)
            End If
        End If
        s_allUserSettingsData.SaveAllUserRecords(_LoginDialog.LoggedOnUser, e.SettingName, e.NewValue?.ToString)
    End Sub

#End Region ' Settings Events

#Region "Timer Events"

    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not Me.HomeTabChart.ChartAreas(NameOf(ChartArea)).AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.HomeTabChart.ChartAreas(NameOf(ChartArea)).CursorX.Position = Double.NaN
        End If
    End Sub

    Public Sub PowerModeChanged(sender As Object, e As Microsoft.Win32.PowerModeChangedEventArgs)
        Select Case e.Mode
            Case Microsoft.Win32.PowerModes.Suspend
                Me.ServerUpdateTimer.Stop()
                Me.LastUpdateTime.Text = "Sleeping"
            Case Microsoft.Win32.PowerModes.Resume
                Me.LastUpdateTime.Text = "Awake"
                Me.ServerUpdateTimer.Interval = s_thirtySecondInMilliseconds \ 3
                Me.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(PowerModeChanged)}, restarted after wake. {NameOf(ServerUpdateTimer)} started at {Now.ToLongTimeString}")
        End Select

    End Sub

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        Me.ServerUpdateTimer.Stop()
        Debug.Print($"Before SyncLock in {NameOf(ServerUpdateTimer_Tick)}, {NameOf(ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(ServerUpdateTimer_Tick)}, inside SyncLock at {Now.ToLongTimeString}")
            If Not _updating Then
                _updating = True
                Me.RecentData = _client?.GetRecentData(_LoginDialog.LoggedOnUser.CountryCode)
                If Me.RecentData Is Nothing Then
                    If _client Is Nothing OrElse _client.HasErrors Then
                        _client = New CareLinkClient(My.Settings.CareLinkUserName, My.Settings.CareLinkPassword, My.Settings.CountryCode)
                        _LoginDialog.Client = _client
                    End If
                    Me.RecentData = _client.GetRecentData(_LoginDialog.LoggedOnUser.CountryCode)
                End If
                Me.LoginStatus.Text = _client.GetLastErrorMessage
                Me.Cursor = Cursors.Default
                Application.DoEvents()
            End If
            _updating = False
        End SyncLock
        Dim lastMedicalDeviceDataUpdateServerTime As String = ""
        If Me.RecentData Is Nothing Then
            Me.LoginStatus.Text = _client.GetLastErrorMessage
        Else
            If Me.RecentData?.TryGetValue(NameOf(lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerTime) Then
                If CLng(lastMedicalDeviceDataUpdateServerTime) = s_lastMedicalDeviceDataUpdateServerTime Then
                    Me.RecentData = Nothing
                Else
                    Me.LastUpdateTime.Text = Now.ToShortDateTimeString
                    Me.AllTabPagesUpdate()
                End If
            End If
        End If

        Me.ServerUpdateTimer.Interval = s_twoMinutesInMilliseconds
        Me.ServerUpdateTimer.Start()
        Debug.Print($"In {NameOf(ServerUpdateTimer_Tick)}, exited SyncLock. {NameOf(ServerUpdateTimer)} started at {Now.ToLongTimeString}")
    End Sub

#End Region ' Timer Events

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Home Tab Charts"

    Friend Sub InitializeHomePageChart()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.HomeTabChart = CreateChart(NameOf(HomeTabChart))
        Dim homeTabChartArea As ChartArea = CreateChartArea()
        Me.HomeTabChart.ChartAreas.Add(homeTabChartArea)

        Dim defaultLegend As Legend = CreateChartLegend(NameOf(defaultLegend))

        Me.HomeTabBasalSeries = CreateBasalSeries(AxisType.Secondary)
        Me.HomeTabBGSeries = CreateBgSeries(NameOf(defaultLegend))
        Me.HomeTabMarkerSeries = CreateMarkerSeries(AxisType.Secondary)

        Me.HomeTabHighLimitSeries = CreateSeriesLimits(HighLimitSeriesName, Color.Orange)
        Me.HomeTabLowLimitSeries = CreateSeriesLimits(LowLimitSeriesName, Color.Red)
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

    Friend Sub InitializeTimeInRangeArea()
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
            .Size = New Size(width1,
                             width1)
                            }

        With Me.HomeTabTimeInRangeChart
            .BorderSkin.BackSecondaryColor = Color.Transparent
            .BorderSkin.SkinStyle = BorderSkinStyle.None
            Dim timeInRangeChartArea As New ChartArea With {
                    .Name = NameOf(timeInRangeChartArea),
                    .BackColor = Color.Black
                }
            .ChartAreas.Add(timeInRangeChartArea)
            .Location = New Point(Me.TimeInRangeChartLabel.FindHorizontalMidpoint - (.Width \ 2),
                                  CInt(Me.TimeInRangeChartLabel.FindVerticalMidpoint() - Math.Round(.Height / 2.5)))
            .Name = NameOf(HomeTabTimeInRangeChart)
            Me.HomeTabTimeInRangeSeries = New Series(NameOf(HomeTabTimeInRangeSeries)) With {
                    .ChartArea = NameOf(timeInRangeChartArea),
                    .ChartType = SeriesChartType.Doughnut
                }
            .Series.Add(Me.HomeTabTimeInRangeSeries)
            .Series(NameOf(HomeTabTimeInRangeSeries))("DoughnutRadius") = "17"
        End With

        Me.SplitContainer3.Panel2.Controls.Add(Me.HomeTabTimeInRangeChart)
        Application.DoEvents()
    End Sub

#End Region ' Initialize Home Tab Charts

#Region "Initialize Treatment Details Tab Charts"

#Region "Running Active Insulin Chart"

    Friend Sub InitializeActiveInsulinTabChart()
        Me.TabPage02RunningIOB.Controls.Clear()

        Me.ActiveInsulinChart = CreateChart(NameOf(ActiveInsulinChart))
        Dim activeInsulinChartArea As ChartArea = CreateChartArea()
        With activeInsulinChartArea
            With .AxisY
                .MajorTickMark = New TickMark() With {.Interval = HomePageMealRow, .Enabled = False}
                .Maximum = 25
                .Minimum = 0
                .Interval = 4
                .Title = "Active Insulin"
                .TitleForeColor = Color.HotPink
            End With
        End With
        Me.ActiveInsulinChart.ChartAreas.Add(activeInsulinChartArea)

        Me.ActiveInsulinChartLegend = CreateChartLegend(NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinChart.Legends.Add(Me.ActiveInsulinChartLegend)
        Me.ActiveInsulinSeries = New Series(NameOf(ActiveInsulinSeries)) With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = NameOf(ChartArea),
            .ChartType = SeriesChartType.Line,
            .Color = Color.HotPink,
            .Legend = NameOf(ActiveInsulinChartLegend),
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinSeries)

        Me.ActiveInsulinBGSeries = CreateBgSeries(NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinTimeChangeSeries = CreateTimeChangeSeries()
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinBGSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinTimeChangeSeries())

        Me.ActiveInsulinChart.Series(BgSeriesName).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChart.Series(BgSeriesName).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChartTitle = New Title With {
                .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(26, 59, 105),
                .Name = NameOf(ActiveInsulinChartTitle),
                .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                .ShadowOffset = 3
            }
        Me.ActiveInsulinChart.Titles.Add(Me.ActiveInsulinChartTitle)
        Me.TabPage02RunningIOB.Controls.Add(Me.ActiveInsulinChart)
        Application.DoEvents()

    End Sub

#End Region

#Region "Initialize Treatment Markers Chart"

    Private Sub InitializeTreatmentMarkersChart()
        Me.TabPage03TreatmentDetails.Controls.Clear()

        Me.TreatmentMarkersChart = CreateChart(NameOf(TreatmentMarkersChart))
        Dim treatmentMarkersChartArea As ChartArea = CreateChartArea()

        Select Case MaxBasalPerDose
            Case < 0.25
                TreatmentInsulinRow = 0.5
            Case < 0.5
                TreatmentInsulinRow = 0.5
            Case < 0.75
                TreatmentInsulinRow = 0.75
            Case < 1
                TreatmentInsulinRow = 1
            Case < 1.25
                TreatmentInsulinRow = 1.25
            Case < 1.5
                TreatmentInsulinRow = 1.5
            Case < 1.75
                TreatmentInsulinRow = 1.75
            Case < 2
                TreatmentInsulinRow = 2
            Case Else
                TreatmentInsulinRow = CSng(MaxBasalPerDose + 0.025)
        End Select
        TreatmentInsulinRow = TreatmentInsulinRow.RoundSingle(3)
        With treatmentMarkersChartArea.AxisY
            Dim interval As Single = (TreatmentInsulinRow / 10).RoundSingle(3)
            .Interval = interval
            .IsMarginVisible = False
            .IsStartedFromZero = False
            .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .LabelStyle.Format = "{0.000}"
            .LineColor = Color.FromArgb(64, 64, 64, 64)
            .MajorGrid = New Grid() With {
                    .Interval = interval,
                    .LineColor = Color.FromArgb(64, 64, 64, 64)
                }
            .MajorTickMark = New TickMark() With {
                    .Interval = interval,
                    .Enabled = True
                }
            .Maximum = TreatmentInsulinRow
            .Minimum = 0
            .Title = "Delivered Insulin"
        End With

        Me.TreatmentMarkersChart.ChartAreas.Add(treatmentMarkersChartArea)
        Me.TreatmentMarkersChartLegend = CreateChartLegend(NameOf(TreatmentMarkersChartLegend))
        Me.TreatmentMarkersChart.Legends.Add(Me.TreatmentMarkersChartLegend)

        Me.TreatmentMarkerBasalSeries = CreateBasalSeries(AxisType.Primary)
        Me.TreatmentMarkerBGSeries = CreateBgSeries(Me.TreatmentMarkersChartLegend.Name)
        Me.TreatmentMarkerMarkersSeries = CreateMarkerSeries(AxisType.Primary)
        Me.TreatmentMarkerTimeChangeSeries = CreateTimeChangeSeries()
        Me.TreatmentMarkersChart.Series.Add(Me.TreatmentMarkerBasalSeries)
        Me.TreatmentMarkersChart.Series.Add(Me.TreatmentMarkerMarkersSeries)
        Me.TreatmentMarkersChart.Series.Add(Me.TreatmentMarkerTimeChangeSeries())
        Me.TreatmentMarkersChart.Series.Add(Me.TreatmentMarkerBGSeries)

        Me.TreatmentMarkersChart.Series(BgSeriesName).EmptyPointStyle.Color = Color.Transparent
        Me.TreatmentMarkersChart.Series(BgSeriesName).EmptyPointStyle.BorderWidth = 4
        Me.TreatmentMarkersChart.Series(BasalSeriesName).EmptyPointStyle.Color = Color.Transparent
        Me.TreatmentMarkersChart.Series(BasalSeriesName).EmptyPointStyle.BorderWidth = 4
        Me.TreatmentMarkersChart.Series(MarkerSeriesName).EmptyPointStyle.Color = Color.Transparent
        Me.TreatmentMarkersChart.Series(MarkerSeriesName).EmptyPointStyle.BorderWidth = 4
        Me.TreatmentMarkersChartTitle = New Title With {
                .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(26, 59, 105),
                .Name = NameOf(TreatmentMarkersChartTitle),
                .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                .ShadowOffset = 3
            }
        Me.TreatmentMarkersChart.Titles.Add(Me.TreatmentMarkersChartTitle)
        Me.TabPage03TreatmentDetails.Controls.Add(Me.TreatmentMarkersChart)
        Application.DoEvents()

    End Sub

#End Region

#End Region

#End Region ' Initialize Charts

#Region "Update Data and Tables"

    Private Sub CollectMarkers(row As String)
        Dim recordNumberAutoBasalDelivery As Integer = 0
        Dim recordNumberInsulin As Integer = 0
        Dim recordNumberMealRecord As Integer = 0

        Dim basalDictionary As New Dictionary(Of OADate, Single)
        MaxBasalPerHour = 0
        MaxBasalPerDose = 0
        For Each newMarker As Dictionary(Of String, String) In LoadList(row)
            Select Case newMarker("type")
                Case "AUTO_BASAL_DELIVERY"
                    _markersAutoBasalDelivery.Add(newMarker)
                    Dim item As AutoBasalDeliveryRecord = DictionaryToClass(Of AutoBasalDeliveryRecord)(newMarker)
                    recordNumberAutoBasalDelivery += 1
                    item.RecordNumber = recordNumberAutoBasalDelivery
                    s_listOfAutoBasalDeliveryMarkers.Add(item)
                    basalDictionary.Add(item.OAdateTime, item.bolusAmount)
                Case "AUTO_MODE_STATUS"
                    _markersAutoModeStatus.Add(newMarker)
                Case "BG_READING"
                    _markersBgReading.Add(newMarker.ScaleMarker)
                Case "CALIBRATION"
                    _markersCalibration.Add(newMarker.ScaleMarker)
                Case "INSULIN"
                    _markersInsulin.Add(newMarker)
                    recordNumberInsulin += 1
                    Dim item1 As InsulinRecord = DictionaryToClass(Of InsulinRecord)(newMarker)
                    item1.RecordNumber = recordNumberInsulin
                    s_listOfInsulinMarkers.Add(item1)
                    Select Case newMarker(NameOf(InsulinRecord.activationType))
                        Case "AUTOCORRECTION"
                            basalDictionary.Add(item1.OAdateTime, item1.deliveredFastAmount)
                        Case "UNDETERMINED",
                             "RECOMMENDED"
                            '
                        Case Else
                            Throw UnreachableException()
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    _markersLowGlusoseSuspended.Add(newMarker)
                Case "MEAL"
                    Dim item As MealRecord = DictionaryToClass(Of MealRecord)(newMarker)
                    recordNumberMealRecord += 1
                    item.RecordNumber = recordNumberMealRecord
                    _markersMealRecords.Add(item)
                    _markersMeal.Add(newMarker)
                Case "TIME_CHANGE"
                    _markersTimeChange.Add(newMarker)
                    s_markersTimeChange.Add(New TimeChangeRecord(newMarker))
                Case Else
                    Stop
                    Throw UnreachableException()
            End Select
        Next
        Dim endOADate As OADate = basalDictionary.Last.Key
        Dim i As Integer = 0
        While i < basalDictionary.Count AndAlso basalDictionary.Keys(i) <= endOADate
            Dim sum As Single = 0
            Dim j As Integer = i
            Dim startOADate As OADate = basalDictionary.Keys(j)
            While j < basalDictionary.Count AndAlso basalDictionary.Keys(j) <= startOADate + s_hourAsOADate
                sum += basalDictionary.Values(j)
                j += 1
            End While
            MaxBasalPerHour = Math.Max(MaxBasalPerHour, sum)
            MaxBasalPerDose = Math.Max(MaxBasalPerDose, basalDictionary.Values(i))
            i += 1
        End While
        Me.MaxBasalPerHourLabel.Text = $"Max Basal/Hr ~ {MaxBasalPerHour.RoundSingle(3)} U"
        s_markers.AddRange(_markersAutoBasalDelivery)
        s_markers.AddRange(_markersAutoModeStatus)
        s_markers.AddRange(_markersBgReading)
        s_markers.AddRange(_markersCalibration)
        s_markers.AddRange(_markersInsulin)
        s_markers.AddRange(_markersLowGlusoseSuspended)
        s_markers.AddRange(_markersMeal)
        s_markers.AddRange(_markersTimeChange)
    End Sub

    Private Sub UpdateDataTables(isScaledForm As Boolean)
        If Me.RecentData Is Nothing Then
            Debug.Print($"Exiting {NameOf(UpdateDataTables)}, {NameOf(RecentData)} has no data!")
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
        s_markersTimeChange.Clear()
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfInsulinMarkers.Clear()
        s_bindingSourceSGs.Clear()
        s_listOfSummaryRecords.Clear()
        s_limits.Clear()
        s_treatmentMarkerInsulinDictionary.Clear()
        s_treatmentMarkerMealDictionary.Clear()
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
                ProcessSummaryEntry(row, rowIndex, s_firstName)
                Continue For
            End If

            If rowIndex = ItemIndexs.sgs Then
                Dim sglist As List(Of SgRecord) = LoadList(row.Value).ToSgList()
                s_bindingSourceSGs = New List(Of SgRecord)(sglist)
                If s_bindingSourceSGs.Count > 2 Then
                    s_lastBGValue = s_bindingSourceSGs.Item(s_bindingSourceSGs.Count - 2).sg
                End If
                ProcessListOfSGs(Me.TableLayoutPanelSgs, Me.DataGridViewSGs, ClassToDatatable(sglist.ToArray), rowIndex)
                Me.ReadingsLabel.Text = $"{s_bindingSourceSGs.Where(Function(entry As SgRecord) Not Double.IsNaN(entry.sg)).Count}/288"
                Continue For
            End If

            If rowIndex = ItemIndexs.limits OrElse
                rowIndex = ItemIndexs.markers OrElse
                rowIndex = ItemIndexs.pumpBannerState Then

                Select Case rowIndex
                    Case ItemIndexs.limits
                        Dim dataTable1 As DataTable = ClassToDatatable(Of LimitsRecord)()

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
                        ProcessInnerListDictionary(Me.TableLayoutPanelLimits, s_limits, rowIndex, _formScale.Height <> 1)
                    Case ItemIndexs.markers
                        ProcessListOfAutoBasalDeliveryRecords(Me.TableLayoutPanelAutoBasalDelivery, Me.DataGridViewAutoBasalDelivery, s_listOfAutoBasalDeliveryMarkers, rowIndex)
                        ProcessInnerListDictionary(Me.TableLayoutPanelAutoModeStatus, _markersAutoModeStatus, rowIndex, _formScale.Height <> 1)
                        ProcessInnerListDictionary(Me.TableLayoutPanelBgReadings, _markersBgReading, rowIndex, _formScale.Height <> 1)
                        ProcessInnerListDictionary(Me.TableLayoutPanelCalibration, _markersCalibration, rowIndex, _formScale.Height <> 1)
                        ProcessListOfInsulinRecords(Me.TableLayoutPanelInsulin, Me.DataGridViewInsulin, s_listOfInsulinMarkers, rowIndex)
                        ProcessInnerListDictionary(Me.TableLayoutPanelLowGlusoseSuspended, _markersLowGlusoseSuspended, rowIndex, _formScale.Height <> 1)
                        ProcessListOfMealRecords(Me.TableLayoutPanelMeal, _markersMealRecords, rowIndex)
                        ProcessInnerListDictionary(Me.TableLayoutPanelTimeChange, _markersTimeChange, rowIndex, _formScale.Height <> 1)
                    Case ItemIndexs.pumpBannerState
                        If row.Value Is Nothing Then
                            Me.TempTargetLabel.Visible = False
                            ProcessInnerListDictionary(Me.TableLayoutPanelBannerState, New List(Of Dictionary(Of String, String)), rowIndex, _formScale.Height <> 1)
                        Else
                            Dim innerListDictionary As List(Of Dictionary(Of String, String)) = LoadList(row.Value)
                            Me.TempTargetLabel.Visible = False
                            For Each dic As Dictionary(Of String, String) In innerListDictionary
                                Dim typeValue As String = ""
                                If dic.TryGetValue("type", typeValue) AndAlso typeValue = "TEMP_TARGET" Then
                                    Dim minutes As Integer = CInt(dic("timeRemaining"))
                                    Me.TempTargetLabel.Text = $"Target 150   {New TimeSpan(0, minutes \ 60, minutes Mod 60).ToString.Substring(4)} hr"
                                    Me.TempTargetLabel.Visible = True
                                End If
                            Next
                            ProcessInnerListDictionary(Me.TableLayoutPanelBannerState, innerListDictionary, rowIndex, _formScale.Height <> 1)
                        End If
                End Select
                Continue For
            End If
            Dim docStyle As DockStyle = DockStyle.Fill
            Select Case rowIndex
                Case ItemIndexs.lastSG
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelLastSG, ItemIndexs.lastSG)
                    s_lastSG = New SgRecord(Loads(row.Value))

                Case ItemIndexs.lastAlarm
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelLastAlarm, ItemIndexs.lastAlarm)

                Case ItemIndexs.activeInsulin
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelActiveInsulin, ItemIndexs.activeInsulin)
                    s_activeInsulin = DictionaryToClass(Of ActiveInsulinRecord)(Loads(row.Value))

                Case ItemIndexs.notificationHistory
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelNotificationHistory, ItemIndexs.notificationHistory)

                Case ItemIndexs.therapyAlgorithmState
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelTherapyAlgorthm, ItemIndexs.therapyAlgorithmState)

                Case ItemIndexs.basal
                    layoutPanel1 = InitializeWorkingPanel(Me.TableLayoutPanelBasal, ItemIndexs.basal)

                Case Else
                    Stop
                    Throw UnreachableException()
            End Select

            Try
                layoutPanel1.SuspendLayout()
                layoutPanel1.Controls(0).Text = $"{CInt(rowIndex)} {rowIndex}"
                If layoutPanel1.RowStyles.Count = 1 Then
                    layoutPanel1.RowStyles(0) = New RowStyle(SizeType.AutoSize, 0)
                Else
                    layoutPanel1.RowStyles(1) = New RowStyle(SizeType.AutoSize, 0)
                End If
                Dim innerJsonDictionary As Dictionary(Of String, String) = Loads(row.Value)
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
        Me.AboveHighLimitMessageLabel.Text = $"Above {s_limitHigh} {BgUnitsString}"
        Me.BelowLowLimitMessageLabel.Text = $"Below {s_limitLow} {BgUnitsString}"
        Me.ModelLabel.Text = s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.pumpModelNumber))
        Me.FullNameLabel.Text = $"{s_firstName} {s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.lastName))}"
        Me.SerialNumberLabel.Text = s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.medicalDeviceSerialNumber))
        Dim rowValue As String = s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.lastSGTrend))
        Dim arrows As String = Nothing
        If Trends.TryGetValue(rowValue, arrows) Then
            Me.LabelTrendArrows.Text = Trends(rowValue)
        Else
            Me.LabelTrendArrows.Text = $"{rowValue}"
        End If
        Me.Cursor = Cursors.Default
    End Sub

#End Region ' Update Data and Tables

#Region "Update Home Tab"

    Friend Sub AllTabPagesUpdate()
        If Me.RecentData Is Nothing Then
            Debug.Print($"Exiting {NameOf(AllTabPagesUpdate)}, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Dim lastMedicalDeviceDataUpdateServerTime As String = ""
        If Me.RecentData?.TryGetValue(NameOf(lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerTime) Then
            If CLng(lastMedicalDeviceDataUpdateServerTime) = s_lastMedicalDeviceDataUpdateServerTime Then
                Me.RecentData = Nothing
                Exit Sub
            End If
        End If

        If Me.RecentData.Count > ItemIndexs.finalCalibration + 1 Then
            Stop
        End If
        Debug.Print($"In {NameOf(AllTabPagesUpdate)} before SyncLock")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(AllTabPagesUpdate)} inside SyncLock")
            _updating = True ' prevent paint
            _homePageAbsoluteRectangle = RectangleF.Empty
            _treatmentMarkerAbsoluteRectangle = RectangleF.Empty
            Me.MenuStartHere.Enabled = False
            If Not Me.LastUpdateTime.Text.Contains("from file") Then
                Me.LastUpdateTime.Text = Now.ToShortDateTimeString
            Else
                Me.LastUpdateTime.Text = Now.ToShortDateTimeString
            End If
            Me.CursorPanel.Visible = False
            Me.UpdateDataTables(_formScale.Height <> 1 OrElse _formScale.Width <> 1)
            _updating = False
        End SyncLock
        Debug.Print($"In {NameOf(AllTabPagesUpdate)} exited SyncLock")
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
        Me.UpdateTreatmentChart()
        Me.UpdateSummaryTable()
        Application.DoEvents()
    End Sub

    Private Sub UpdateActiveInsulin(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            Dim activeInsulinStr As String = $"{s_activeInsulin.amount:N3}"
            Me.ActiveInsulinValue.Text = $"Active Insulin{Environment.NewLine}{activeInsulinStr} U"
            _bgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr}U"
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
    End Sub

    Private Sub UpdateActiveInsulinChart(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        If Not _Initialized Then
            Exit Sub
        End If

        Try
            Dim lastTimeChangeRecord As TimeChangeRecord = Nothing
            For Each s As Series In Me.ActiveInsulinChart.Series
                s.Points.Clear()
            Next

            Me.ActiveInsulinChart.Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Active Insulin in Pink"
            Me.ActiveInsulinChart.ChartAreas(NameOf(ChartArea)).InitializeBGChartArea()

            ' Order all markers by time
            Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)
            Dim sgOADateTime As OADate

            For Each marker As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
                sgOADateTime = New OADate(s_markers.SafeGetSgDateTime(marker.Index).RoundTimeDown(RoundTo.Minute))
                Select Case marker.Value(NameOf(InsulinRecord.type)).ToString
                    Case "AUTO_BASAL_DELIVERY"
                        Dim bolusAmount As Single = marker.Value.GetSingleValue(NameOf(AutoBasalDeliveryRecord.bolusAmount))
                        If timeOrderedMarkers.ContainsKey(sgOADateTime) Then
                            timeOrderedMarkers(sgOADateTime) += bolusAmount
                        Else
                            timeOrderedMarkers.Add(sgOADateTime, bolusAmount)
                        End If
                    Case "AUTO_MODE_STATUS"
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "INSULIN"
                        Dim bolusAmount As Single = marker.Value.GetSingleValue(NameOf(InsulinRecord.deliveredFastAmount))
                        If timeOrderedMarkers.ContainsKey(sgOADateTime) Then
                            timeOrderedMarkers(sgOADateTime) += bolusAmount
                        Else
                            timeOrderedMarkers.Add(sgOADateTime, bolusAmount)
                        End If
                    Case "LOW_GLUCOSE_SUSPENDED"
                    Case "MEAL"
                    Case "TIME_CHANGE"
                        lastTimeChangeRecord = New TimeChangeRecord(s_markers(marker.Index))
                    Case Else
                        Stop
                End Select
            Next

            If lastTimeChangeRecord IsNot Nothing Then
                Me.ActiveInsulinChart.ChartAreas(NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            End If
            ' set up table that holds active insulin for every 5 minutes
            Dim remainingInsulinList As New List(Of RunningActiveInsulinRecord)
            Dim currentMarker As Integer = 0

            For i As Integer = 0 To 287
                Dim initialBolus As Single = 0
                Dim firstNotSkippedOaTime As New OADate((s_bindingSourceSGs(0).datetime + (s_fiveMinuteSpan * i)).RoundTimeDown(RoundTo.Minute))
                While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= firstNotSkippedOaTime
                    initialBolus += timeOrderedMarkers.Values(currentMarker)
                    currentMarker += 1
                End While
                remainingInsulinList.Add(New RunningActiveInsulinRecord(firstNotSkippedOaTime, initialBolus, Me.MenuOptionsUseAdvancedAITDecay.Checked))
            Next

            Me.ActiveInsulinChart.ChartAreas(NameOf(ChartArea)).AxisY2.Maximum = HomePageBasalRow
            ' walk all markers, adjust active insulin and then add new marker
            Dim maxActiveInsulin As Double = 0
            For i As Integer = 0 To remainingInsulinList.Count - 1
                If i < s_activeInsulinIncrements Then
                    Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaDateTime, Double.NaN)
                    Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.Last.IsEmpty = True
                    If i > 0 Then
                        remainingInsulinList.Adjustlist(0, i)
                    End If
                    Continue For
                End If
                Dim startIndex As Integer = i - s_activeInsulinIncrements + 1
                Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, s_activeInsulinIncrements)
                maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaDateTime, sum)
                remainingInsulinList.Adjustlist(startIndex, s_activeInsulinIncrements)
            Next

            Me.ActiveInsulinChart.ChartAreas(NameOf(ChartArea)).AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
            maxActiveInsulin = Me.ActiveInsulinChart.ChartAreas(NameOf(ChartArea)).AxisY.Maximum

            Me.ActiveInsulinChart.PlotSgSeries(HomePageMealRow)
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateAutoModeShield(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            Me.LastSGTimeLabel.Text = s_lastSG.datetime.ToShortTimeString
            Me.ShieldUnitsLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.Text = BgUnitsString
            If s_lastSG.sg <> 0 Then
                Me.CurrentBGLabel.Visible = True
                Me.CurrentBGLabel.Text = s_lastSG.sg.ToString
                Me.UpdateNotifyIcon()
                _bgMiniDisplay.SetCurrentBGString(s_lastSG.sg.ToString)
                Me.SensorMessage.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield
            Else
                _bgMiniDisplay.SetCurrentBGString("---")
                Me.CurrentBGLabel.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield_Disabled
                Me.SensorMessage.Visible = True
                Me.SensorMessage.BackColor = Color.Transparent
                Dim message As String = ""
                If Not s_sensorMessages.TryGetValue(s_sensorState, message) Then
                    If Debugger.IsAttached Then
                        MsgBox($"{s_sensorState} is unknown sensor message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                    End If

                    message = message.ToTitle
                End If
                Me.SensorMessage.Text = message
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
            Select Case marker.Value(NameOf(InsulinRecord.type))
                Case "INSULIN"
                    Dim amountString As String = marker.Value(NameOf(InsulinRecord.deliveredFastAmount)).TruncateSingleString(3)
                    s_totalDailyDose += amountString.ParseSingle()
                    Select Case marker.Value(NameOf(InsulinRecord.activationType))
                        Case "AUTOCORRECTION"
                            s_totalAutoCorrection += amountString.ParseSingle()
                        Case "RECOMMENDED", "UNDETERMINED"
                            s_totalManualBolus += amountString.ParseSingle()
                    End Select

                Case "AUTO_BASAL_DELIVERY"
                    Dim amountString As String = marker.Value(NameOf(AutoBasalDeliveryRecord.bolusAmount)).TruncateSingleString(3)
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
            For Each s As Series In Me.HomeTabChart.Series
                s.Points.Clear()
            Next
            Me.HomeTabChart.ChartAreas(NameOf(ChartArea)).InitializeBGChartArea()
            Me.HomeTabChart.PlotHomePageMarkers(_homePageAbsoluteRectangle)
            Me.HomeTabChart.PlotSgSeries(HomePageMealRow)
            Me.HomeTabChart.PlotHighLowLimits(memberName, sourceLineNumber)
        Catch ex As Exception
            Throw New Exception($"{ex.Message} exception while plotting Markers in {memberName} at {sourceLineNumber}")
        End Try

    End Sub

    Private Sub UpdateInsulinLevel()
        Select Case s_reservoirLevelPercent
            Case >= 85
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(7)
            Case >= 71
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(6)
            Case >= 57
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(5)
            Case >= 43
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(4)
            Case >= 29
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(3)
            Case >= 15
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(2)
            Case >= 1
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

    Private Sub UpdateSummaryTable()
        Dim dataTable1 As DataTable = ClassToDatatable(s_listOfSummaryRecords.ToArray)
        Me.DataGridViewSummary.DataSource = dataTable1
        Me.DataGridViewSummary.RowHeadersVisible = False
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

    Private Sub UpdateTreatmentChart(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        If Not _Initialized Then
            Exit Sub
        End If
        Try
            Me.InitializeTreatmentMarkersChart()
            Me.TreatmentMarkersChart.Titles(NameOf(TreatmentMarkersChartTitle)).Text = $"Treatment Makers"
            Me.TreatmentMarkersChart.ChartAreas(NameOf(ChartArea)).InitializeBGChartArea()
            Me.TreatmentMarkersChart.PlotTreatmentMarkers()
            Me.TreatmentMarkersChart.PlotSgSeries(HomePageMealRow)
        Catch ex As Exception
            Throw New ArithmeticException($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try
        Application.DoEvents()
    End Sub

#End Region ' Update Home Tab

#Region "Scale Split Containers"

    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical, _formScale.Width, _formScale.Height)
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
        _formScale = New SizeF(_formScale.Width * factor.Width, _formScale.Height * factor.Height)
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
        Try
            Dim sg As Single = s_lastSG.sg
            Dim str As String = s_lastSG.sg.ToString
            Dim fontToUse As New Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel)
            Dim color As Color = Color.White
            Dim bgColor As Color
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
                If s_lastBGValue = 0 Then
                    Me.LabelTrendValue.Text = ""
                Else
                    notStr.Append(Environment.NewLine)
                    Dim diffsg As Double = sg - s_lastBGValue
                    notStr.Append("SG Trend ")
                    If Math.Abs(diffsg) < Single.Epsilon Then
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
                    Me.LabelTrendValue.Text = diffsg.ToString(If(BgUnits = "MG_DL", "+0;-#", "+ 0.00;-#.00"), CultureInfo.InvariantCulture)
                    Me.LabelTrendValue.ForeColor = bgColor
                    notStr.Append(diffsg.ToString(If(BgUnits = "MG_DL", "+0;-#", "+ 0.00;-#.00"), CultureInfo.InvariantCulture))
                End If
                notStr.Append(Environment.NewLine)
                notStr.Append("Active ins. ")
                notStr.Append(s_activeInsulin.amount)
                notStr.Append("U"c)
                Me.NotifyIcon1.Text = notStr.ToString
                s_lastBGValue = sg
                bitmapText.Dispose()
                g.Dispose()
            End Using
        Catch ex As Exception
            Stop
            ' ignore errors
        End Try
    End Sub

#End Region 'NotifyIcon Support

End Class
