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
Imports DataGridViewColumnControls
Imports ToolStripControls

Public Class Form1
    Private WithEvents AITComboBox As ToolStripComboBoxEx

    Private ReadOnly _bgMiniDisplay As New BGMiniWindow(Me)
    Private ReadOnly _calibrationToolTip As New ToolTip()
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

    Public Property Client As CareLinkClient
        Get
            Return Me.LoginDialog?.Client
        End Get
        Set(value As CareLinkClient)
            Me.LoginDialog.Client = value
        End Set
    End Property

    Public Property Initialized As Boolean = False
    Public ReadOnly Property LoginDialog As New LoginForm1

#Region "Pump Data"

    Friend Property RecentData As New Dictionary(Of String, String)

#End Region ' Pump Data

#Region "Chart Objects"

#Region "Charts"

    Private WithEvents ActiveInsulinChart As Chart
    Private WithEvents HomeTabChart As Chart
    Private WithEvents HomeTabTimeInRangeChart As Chart
    Private WithEvents TreatmentMarkersChart As Chart

#End Region

#Region "Legends"

    Private WithEvents ActiveInsulinChartLegend As Legend
    Friend WithEvents TreatmentMarkersChartLegend As Legend

#End Region

#Region "Series"

#Region "Common Series"

    Private WithEvents ActiveInsulinBasalSeries As Series
    Private WithEvents ActiveInsulinBGSeries As Series
    Private WithEvents ActiveInsulinMarkerSeries As Series
    Private WithEvents ActiveInsulinSeries As Series
    Private WithEvents ActiveInsulinTimeChangeSeries As Series

    Private WithEvents HomeTabBasalSeries As Series
    Private WithEvents HomeTabBGSeries As Series
    Private WithEvents HomeTabHighLimitSeries As Series
    Private WithEvents HomeTabLowLimitSeries As Series
    Private WithEvents HomeTabMarkerSeries As Series
    Private WithEvents HomeTabTimeChangeSeries As Series
    Private WithEvents HomeTabTimeInRangeSeries As New Series

    Private WithEvents TreatmentMarkerBasalSeries As Series
    Private WithEvents TreatmentMarkerBGSeries As Series
    Private WithEvents TreatmentMarkerMarkersSeries As Series
    Private WithEvents TreatmentMarkerTimeChangeSeries As Series

#End Region

#End Region

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As New Title
    Private WithEvents TreatmentMarkersChartTitle As Title

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
        Me.MenuOptionsSetupEMailServer.Visible = False
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
            Me.UpdateAllTabPages()
        End If
    End Sub

#End Region ' Form Events

#Region "Form Menu Events"

    Private Sub AITComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AITComboBox.SelectedIndexChanged
        If Me.AITComboBox.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim aitTimeSpan As TimeSpan = TimeSpan.Parse(Me.AITComboBox.SelectedValue.ToString)
        If My.Settings.AIT <> aitTimeSpan Then
            My.Settings.AIT = aitTimeSpan
            My.Settings.Save()
            s_activeInsulinIncrements = CInt(TimeSpan.Parse(aitTimeSpan.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan)
            Me.UpdateActiveInsulinChart(Me.ActiveInsulinChart)
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
                    Me.RecentData?.Clear()
                    ExceptionHandlerForm.ReportFileNameWithPath = fileNameWithPath
                    If ExceptionHandlerForm.ShowDialog() = DialogResult.OK Then
                        ExceptionHandlerForm.ReportFileNameWithPath = ""
                        Try
                            Me.RecentData = Loads(ExceptionHandlerForm.LocalRawData)
                        Catch ex As Exception
                            MessageBox.Show($"Error reading date file. Original error: {ex.DecodeException()}")
                        End Try
                        Me.ShowMiniDisplay.Visible = Debugger.IsAttached
                        Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                        Me.LastUpdateTime.ForeColor = SystemColors.ControlText
                        Me.LastUpdateTime.Text = $"{File.GetLastWriteTime(fileNameWithPath).ToShortDateTimeString} from file"
                        Try
                            Me.FinishInitialization()
                            Try
                                Me.UpdateAllTabPages()
                            Catch ex As ArgumentException
                                MessageBox.Show($"Error in {NameOf(UpdateAllTabPages)}. Original error: {ex.Message}")
                            End Try
                        Catch ex As ArgumentException
                            MessageBox.Show($"Error in {NameOf(FinishInitialization)}. Original error: {ex.Message}")
                        End Try
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.DecodeException()}")
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
                    Me.LastUpdateTime.ForeColor = SystemColors.ControlText
                    Me.LastUpdateTime.Text = File.GetLastWriteTime(openFileDialog1.FileName).ToShortDateTimeString
                    Me.FinishInitialization()
                    Me.UpdateAllTabPages()
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.DecodeException()}")
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
        Private Sub MenuOptionsSetupEMailServer_Click(sender As Object, e As EventArgs) Handles MenuOptionsSetupEMailServer.Click
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
        Me.UpdateActiveInsulinChart(Me.ActiveInsulinChart)

    End Sub

    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) Handles MenuOptionsUseLocalTimeZone.Click
        Dim useLocalTimeZoneChecked As Boolean = Me.MenuOptionsUseLocalTimeZone.Checked
        My.Settings.UseLocalTimeZone = useLocalTimeZoneChecked
        My.Settings.Save()
        s_clientTimeZoneName = s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.clientTimeZoneName))
        s_clientTimeZone = CalculateTimeZone(s_clientTimeZoneName)
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

    Private Sub TabControlHomePage_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage1.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPageAllUsers)
                Me.DataGridViewCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DataGridViewCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
                Me.CareLinkUsersAITComboBox.Width = Me.AITComboBox.Width
                Me.CareLinkUsersAITComboBox.SelectedIndex = Me.AITComboBox.SelectedIndex
                Me.CareLinkUsersAITComboBox.Visible = False
                Me.DataGridViewCareLinkUsers.Columns(NameOf(DataGridViewTextBoxColumnCareLinkAIT)).Width = Me.AITComboBox.Width
            Case NameOf(TabPage14Markers)
                Me.TabControlPage2.SelectedIndex = _lastMarkerTabIndex
                Me.TabControlPage1.Visible = False
                Exit Sub
        End Select
        _lastHomeTabIndex = e.TabPageIndex
    End Sub

    Private Sub TabControlMarkers_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage2.Selecting
        Select Case e.TabPage.Name
            Case NameOf(TabPageBackToHomePage)
                Me.TabControlPage1.SelectedIndex = _lastHomeTabIndex
                Me.TabControlPage1.Visible = True
                Exit Sub
            Case NameOf(TabPageAllUsers)
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

    Private Sub Chart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles ActiveInsulinChart.CursorPositionChanging, HomeTabChart.CursorPositionChanging
        If Not _Initialized Then Exit Sub

        Me.CursorTimer.Interval = s_thirtySecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    Private Sub Chart_MouseMove(sender As Object, e As MouseEventArgs) Handles HomeTabChart.MouseMove, ActiveInsulinChart.MouseMove

        If Not _Initialized Then
            Exit Sub
        End If
        _inMouseMove = True
        Dim yInPixels As Double
        Dim chart1 As Chart = CType(sender, Chart)
        Dim isHomePage As Boolean = chart1.Name = "HomeTabChart"
        Try
            yInPixels = chart1.ChartAreas(NameOf(ChartArea)).AxisY2.ValueToPixelPosition(e.Y)
        Catch ex As Exception
            yInPixels = Double.NaN
        End Try
        If Double.IsNaN(yInPixels) Then
            _inMouseMove = False
            Exit Sub
        End If
        Dim result As HitTestResult
        Try
            result = chart1.HitTest(e.X, e.Y, True)
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
                    If isHomePage Then
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
                    Else
                        Select Case markerToolTip.Length
                            Case 1
                                currentDataPoint.ToolTip = $"{markerToolTip(0)}"
                            Case 2
                                currentDataPoint.ToolTip = $"{markerToolTip(0)} {markerToolTip(1)}"
                            Case 3
                                currentDataPoint.ToolTip = $"{markerToolTip(0)} {markerToolTip(1)} {markerToolTip(2)}"
                            Case Else
                                Stop
                        End Select
                    End If
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
                Case ActiveInsulinSeriesName
                    currentDataPoint.ToolTip = $"{currentDataPoint.YValues.FirstOrDefault:F3} U"
                Case Else
                    Stop
            End Select
        Catch ex As Exception
            result = Nothing
        Finally
            _inMouseMove = False
        End Try
    End Sub

    Private Sub SensorAgeLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles TransmitterBatteryPictureBox.MouseHover
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
                s_ActiveInsulinMarkerInsulinDictionary,
                Nothing,
                True,
                True)
        End SyncLock
        Debug.Print($"In {NameOf(ActiveInsulinChart_PostPaint)} exited SyncLock")
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

#End Region ' Post Paint Events

#End Region ' Home Page Events

#Region "DataGridView Events"

#Region "All Users Tab DataGridView Events"

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

    Private Sub DataGridViewCareLinkUsers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewCareLinkUsers.CellEndEdit
        'after you've filled your dataSet, on event above try something like this
        Try
            '
        Catch ex As Exception
            MessageBox.Show(ex.DecodeException())
        End Try

    End Sub

    Private Sub DataGridViewCareLinkUsers_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridViewCareLinkUsers.CellValidating
        If e.ColumnIndex = 0 Then
            Exit Sub
        End If

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
        For i As Integer = e.RowIndex To e.RowIndex + (e.RowCount - 1)
            Dim disableButtonCell As DataGridViewDisableButtonCell = CType(dgv.Rows(i).Cells(NameOf(DataGridViewButtonColumnCareLinkDeleteRow)), DataGridViewDisableButtonCell)
            disableButtonCell.Enabled = s_allUserSettingsData(i).CareLinkUserName <> _LoginDialog.LoggedOnUser.CareLinkUserName
        Next
    End Sub

#End Region ' All Users Tab DataGridView Events

#Region "My User Tab DataGridView Events"

    Private Sub DataGridViewMyUserData_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewCurrentUser.ColumnAdded
        e.DgvColumnAdded(New DataGridViewCellStyle().SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1)),
                         False,
                         True,
                         Nothing)

    End Sub

    Private Sub DataGridViewMyUserData_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewCurrentUser.DataError
        Stop
    End Sub

#End Region ' My User Tab DataGridView Events

#Region "Profile Tab DataGridView Events"

    Private Sub DataGridViewUserProfile_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewUserProfile.ColumnAdded
        e.DgvColumnAdded(New DataGridViewCellStyle().SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1)),
                         False,
                         True,
                         Nothing)

    End Sub

    Private Sub DataGridViewUserProfile_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewUserProfile.DataError
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

    Private Sub DataGridViewViewInsulin_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewInsulin.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(InsulinRecord.dateTime))
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(InsulinRecord.programmedFastAmount)
                If e.Value.ToString <> dgv.Rows(e.RowIndex).Cells(NameOf(InsulinRecord.deliveredFastAmount)).Value.ToString Then
                    e.CellStyle.BackColor = Color.Red
                End If
            Case NameOf(InsulinRecord.deliveredFastAmount)
                If e.Value.ToString <> dgv.Rows(e.RowIndex).Cells(NameOf(InsulinRecord.programmedFastAmount)).Value.ToString Then
                    e.CellStyle.BackColor = Color.Red
                End If
            Case NameOf(InsulinRecord.programmedExtendedAmount)
                If e.Value.ToString <> dgv.Rows(e.RowIndex).Cells(NameOf(InsulinRecord.deliveredExtendedAmount)).Value.ToString Then
                    e.CellStyle.BackColor = Color.Red
                End If
            Case NameOf(InsulinRecord.deliveredExtendedAmount)
                If e.Value.ToString <> dgv.Rows(e.RowIndex).Cells(NameOf(InsulinRecord.programmedExtendedAmount)).Value.ToString Then
                    e.CellStyle.BackColor = Color.Red
                End If
        End Select
    End Sub

#End Region 'Insulin DataGridView Events

#Region "Report Tab DataGridView Events"

    Private Sub DataGridViewSuportedReports_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index - 1).Caption
        e.DgvColumnAdded(supportedReportRecordHelpers.GetCellStyle(e.Column.Name),
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
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(SgRecord.sensorState), StringComparison.OrdinalIgnoreCase) Then
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
                e.CellStyle.BackColor = Color.Yellow
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

    Private Sub DataGridViewSummary_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewSummary.CellFormatting
        If e.Value Is Nothing OrElse e.ColumnIndex <> 2 Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String = dgv.Rows(e.RowIndex).Cells("key").Value.ToString
        Select Case CType([Enum].Parse(GetType(ItemIndexs), key), ItemIndexs)
            Case ItemIndexs.lastSensorTS, ItemIndexs.medicalDeviceTimeAsString,
                 ItemIndexs.lastSensorTSAsString, ItemIndexs.kind,
                 ItemIndexs.pumpModelNumber, ItemIndexs.currentServerTime,
                 ItemIndexs.lastConduitTime, ItemIndexs.lastConduitUpdateServerTime,
                 ItemIndexs.lastMedicalDeviceDataUpdateServerTime,
                 ItemIndexs.firstName, ItemIndexs.lastName, ItemIndexs.conduitSerialNumber,
                 ItemIndexs.conduitBatteryStatus, ItemIndexs.medicalDeviceFamily,
                 ItemIndexs.sensorState, ItemIndexs.medicalDeviceSerialNumber,
                 ItemIndexs.medicalDeviceTime, ItemIndexs.sMedicalDeviceTime,
                 ItemIndexs.calibStatus, ItemIndexs.bgUnits, ItemIndexs.timeFormat,
                 ItemIndexs.lastSensorTime, ItemIndexs.sLastSensorTime,
                 ItemIndexs.lastSGTrend, ItemIndexs.systemStatusMessage,
                 ItemIndexs.lastConduitDateTime, ItemIndexs.clientTimeZoneName
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case ItemIndexs.averageSG, ItemIndexs.version, ItemIndexs.conduitBatteryLevel,
                 ItemIndexs.reservoirLevelPercent, ItemIndexs.reservoirAmount,
                 ItemIndexs.reservoirRemainingUnits, ItemIndexs.medicalDeviceBatteryLevelPercent,
                 ItemIndexs.sensorDurationHours, ItemIndexs.timeToNextCalibHours,
                 ItemIndexs.belowHypoLimit, ItemIndexs.aboveHyperLimit,
                 ItemIndexs.timeInRange, ItemIndexs.gstBatteryLevel,
                 ItemIndexs.maxAutoBasalRate, ItemIndexs.maxBolusAmount,
                 ItemIndexs.sensorDurationMinutes,
                 ItemIndexs.timeToNextCalibrationMinutes, ItemIndexs.sgBelowLimit,
                 ItemIndexs.averageSGFloat,
                 ItemIndexs.timeToNextCalibrationRecommendedMinutes
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(0, 1, 1, 1))
            Case Else
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(1))
        End Select

    End Sub

    Private Sub DataGridViewSummary_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridViewSummary.CellMouseClick
        If e.RowIndex < 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim value As String = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
        Dim key As String = dgv.Rows(e.RowIndex).Cells("key").Value.ToString
        If value.StartsWith(ClickToShowDetails) Then

            Select Case GetItemIndex(key)
                Case ItemIndexs.lastSG
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage05LastSG))
                Case ItemIndexs.lastAlarm
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage06LastAlarm))
                Case ItemIndexs.activeInsulin
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage07ActiveInsulin))
                Case ItemIndexs.sgs
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage08SensorGlucose))
                Case ItemIndexs.limits
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage09Limits))
                Case ItemIndexs.markers
                    _lastMarkerTabIndex = If(_lastMarkerTabIndex < 8, _lastMarkerTabIndex, 0)
                    Me.TabControlPage2.SelectedIndex = _lastMarkerTabIndex
                    Me.TabControlPage1.Visible = False
                    Exit Select
                Case ItemIndexs.notificationHistory
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage10NotificationHistory))
                Case ItemIndexs.therapyAlgorithmState
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage11TherapyAlgorithm))
                Case ItemIndexs.pumpBannerState
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage12BannerState))
                Case ItemIndexs.basal
                    Me.TabControlPage1.SelectedIndex = GetTabIndexFromName(NameOf(TabPage13Basal))
            End Select
        End If
    End Sub

    Private Sub DataGridViewSummary_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridViewSummary.ColumnAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(GetCellStyle(e.Column.Name),
                         False,
                         True,
                         caption)
    End Sub

    Private Sub DataGridViewSummary_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridViewSummary.DataError
        Stop
    End Sub

#End Region 'Summary Tab DataGridView Events

#End Region ' DataGridView Events

#Region "Tab Button Events"

    Private Sub TableLayoutPanelTopButton_Click(sender As Object, e As EventArgs) _
        Handles TableLayoutPanelLastSgTop.ButtonClick, TableLayoutPanelLastAlarmTop.ButtonClick,
        TableLayoutPanelActiveInsulinTop.ButtonClick, SgsButton.Click,
        TableLayoutPanelSgsTop.ButtonClick, TableLayoutPanelLimitsTop.ButtonClick,
        TableLayoutPanelNotificationHistoryTop.ButtonClick,
        TableLayoutPanelTherapyAlgorithmTop.ButtonClick, TableLayoutPanelBannerStateTop.ButtonClick,
        TableLayoutPanelBasalTop.ButtonClick, TableLayoutPanelAutoBasalDeliveryTop.ButtonClick,
        TableLayoutPanelAutoModeStatusTop.ButtonClick, TableLayoutPanelBgReadingsTop.ButtonClick,
        TableLayoutPanelCalibrationTop.ButtonClick, TableLayoutPanelInsulinTop.ButtonClick,
        TableLayoutPanelLowGlucoseSuspendedTop.ButtonClick, TableLayoutPanelMealTop.ButtonClick,
        TableLayoutPanelTimeChangeTop.ButtonClick
        Me.TabControlPage1.SelectedIndex = 3
        Me.TabControlPage1.Visible = True
    End Sub

#End Region

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

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        Me.ServerUpdateTimer.Stop()
        Debug.Print($"Before SyncLock in {NameOf(ServerUpdateTimer_Tick)}, {NameOf(ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(ServerUpdateTimer_Tick)}, inside SyncLock at {Now.ToLongTimeString}")
            If Not _updating Then
                _updating = True
                Me.RecentData = Me.Client?.GetRecentData(Me)
                If Me.RecentData Is Nothing Then
                    If Me.Client Is Nothing OrElse Me.Client.HasErrors Then
                        Me.Client = New CareLinkClient(My.Settings.CareLinkUserName, My.Settings.CareLinkPassword, My.Settings.CountryCode)
                    End If
                    Me.RecentData = Me.Client.GetRecentData(Me)
                End If
                ReportLoginStatus(Me.LoginStatus, Me.RecentData Is Nothing OrElse Me.RecentData.Count = 0, Me.Client.GetLastErrorMessage)

                Me.Cursor = Cursors.Default
                Application.DoEvents()
            End If
            _updating = False
        End SyncLock

        Dim lastMedicalDeviceDataUpdateServerEpochString As String = ""
        If Me.RecentData Is Nothing OrElse Me.RecentData.Count = 0 Then
            ReportLoginStatus(Me.LoginStatus, True, Me.Client.GetLastErrorMessage)

            _bgMiniDisplay.SetCurrentBGString("---")
        Else
            If Me.RecentData?.TryGetValue(NameOf(ItemIndexs.lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerEpochString) Then
                If CLng(lastMedicalDeviceDataUpdateServerEpochString) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                    If lastMedicalDeviceDataUpdateServerEpochString.Epoch2DateTime + s_fiveMinuteSpan < Now Then
                        Me.LastUpdateTime.ForeColor = Color.Red
                        _bgMiniDisplay.SetCurrentBGString("---")
                    Else
                        Me.LastUpdateTime.ForeColor = SystemColors.ControlText
                        _bgMiniDisplay.SetCurrentBGString(s_lastSgRecord?.sg.ToString)
                    End If
                    Me.RecentData = Nothing
                Else
                    Me.LastUpdateTime.ForeColor = SystemColors.ControlText
                    Me.LastUpdateTime.Text = Now.ToShortDateTimeString
                    Me.UpdateAllTabPages()
                End If
            Else
                Stop
            End If
        End If
        LastServerUpdateTime = lastMedicalDeviceDataUpdateServerEpochString.Epoch2DateTime
        Me.ServerUpdateTimer.Interval = s_oneMinutesInMilliseconds
        Me.ServerUpdateTimer.Start()
        Debug.Print($"In {NameOf(ServerUpdateTimer_Tick)}, exited SyncLock. {NameOf(ServerUpdateTimer)} started at {Now.ToLongTimeString}")
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

        Me.HomeTabBasalSeries = CreateSeriesBasal(AxisType.Secondary)
        Me.HomeTabBGSeries = CreateSeriesBg(NameOf(defaultLegend))
        Me.HomeTabMarkerSeries = CreateSeriesMarker(AxisType.Secondary)

        Me.HomeTabHighLimitSeries = CreateSeriesLimits(HighLimitSeriesName, Color.Yellow)
        Me.HomeTabLowLimitSeries = CreateSeriesLimits(LowLimitSeriesName, Color.Red)
        Me.HomeTabTimeChangeSeries = CreateSeriesTimeChange()
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
        Me.ActiveInsulinBasalSeries = CreateSeriesBasal(AxisType.Secondary)
        Me.ActiveInsulinBGSeries = CreateSeriesBg(Me.ActiveInsulinChartLegend.Name)
        Me.ActiveInsulinMarkerSeries = CreateSeriesMarker(AxisType.Secondary)

        Me.ActiveInsulinSeries = New Series(NameOf(ActiveInsulinSeries)) With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = NameOf(ChartArea),
            .ChartType = SeriesChartType.Line,
            .Color = Color.HotPink,
            .Legend = NameOf(ActiveInsulinChartLegend),
            .MarkerColor = Color.Black,
            .MarkerSize = 4,
            .MarkerStyle = MarkerStyle.Circle,
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinBasalSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinMarkerSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinSeries)

        Me.ActiveInsulinBGSeries = CreateSeriesBg(NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinTimeChangeSeries = CreateSeriesTimeChange()
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinBGSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinTimeChangeSeries())

        Me.ActiveInsulinChart.Series(BgSeriesName).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChart.Series(BgSeriesName).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChartTitle = New Title With {
                .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                .ForeColor = Color.HotPink,
                .Name = NameOf(ActiveInsulinChartTitle),
                .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                .ShadowOffset = 3,
                .Text = "Running Active Insulin in Pink"
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
        Me.TreatmentMarkerBasalSeries = CreateSeriesBasal(AxisType.Primary)
        Me.TreatmentMarkerBGSeries = CreateSeriesBg(Me.TreatmentMarkersChartLegend.Name)
        Me.TreatmentMarkerMarkersSeries = CreateSeriesMarker(AxisType.Primary)
        Me.TreatmentMarkerTimeChangeSeries = CreateSeriesTimeChange()
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

    Private Sub UpdateDataTables()

        If Me.RecentData Is Nothing Then
            Debug.Print($"Exiting {NameOf(UpdateDataTables)}, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        s_listOfSGs.Clear()

        s_listOfSummaryRecords.Clear()
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In Me.RecentData.WithIndex()
            Dim row As KeyValuePair(Of String, String) = c.Value
            If row.Value Is Nothing Then
                row = KeyValuePair.Create(row.Key, "")
            End If
            Dim rowIndex As ItemIndexs = GetItemIndex(row.Key)
            Select Case rowIndex
                Case ItemIndexs.lastSensorTS,
                     ItemIndexs.medicalDeviceTimeAsString,
                     ItemIndexs.lastSensorTSAsString,
                     ItemIndexs.kind,
                     ItemIndexs.version,
                     ItemIndexs.pumpModelNumber
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.currentServerTime,
                     ItemIndexs.lastConduitTime,
                     ItemIndexs.lastConduitUpdateServerTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                    s_lastMedicalDeviceDataUpdateServerEpoch = CLng(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, row.Value.Epoch2DateTimeString))

                Case ItemIndexs.firstName
                    s_firstName = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, s_firstName))

                Case ItemIndexs.lastName
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.conduitSerialNumber
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.conduitBatteryLevel
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery is at {row.Value}%."))

                Case ItemIndexs.conduitBatteryStatus
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone battery status is {row.Value}"))

                Case ItemIndexs.conduitInRange
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Phone {If(CBool(row.Value) = True, "is", "is not")} in range of pump"))

                Case ItemIndexs.conduitMedicalDeviceInRange
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump {If(CBool(row.Value) = True, "is", "is not")} in range of phone"))

                Case ItemIndexs.conduitSensorInRange
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Transmitter {If(CBool(row.Value) = True, "is", "is not")} in range of pump"))

                Case ItemIndexs.medicalDeviceFamily
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.sensorState
                    s_sensorState = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case ItemIndexs.medicalDeviceSerialNumber
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump serial number is {row.Value}."))

                Case ItemIndexs.medicalDeviceTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.sMedicalDeviceTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.reservoirLevelPercent
                    s_reservoirLevelPercent = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir is {row.Value}%"))

                Case ItemIndexs.reservoirAmount
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Full reservoir holds {row.Value}U"))

                Case ItemIndexs.reservoirRemainingUnits
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Reservoir has {row.Value}U remaining"))

                Case ItemIndexs.medicalDeviceBatteryLevelPercent
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Pump battery is at {row.Value}%"))

                Case ItemIndexs.sensorDurationHours
                    s_sensorDurationHours = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.timeToNextCalibHours
                    s_timeToNextCalibHours = CUShort(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.calibStatus
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_calibrationMessages, NameOf(s_calibrationMessages)))

                Case ItemIndexs.bgUnits,
                     ItemIndexs.timeFormat,
                     ItemIndexs.lastSensorTime,
                     ItemIndexs.sLastSensorTime,
                     ItemIndexs.medicalDeviceSuspended,
                     ItemIndexs.lastSGTrend
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.lastSG
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_lastSgRecord = New SgRecord(Loads(row.Value))

                Case ItemIndexs.lastAlarm
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_lastAlarmValue = Loads(row.Value)

                Case ItemIndexs.activeInsulin
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_activeInsulin = DictionaryToClass(Of ActiveInsulinRecord)(Loads(row.Value), 0)

                Case ItemIndexs.sgs
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_listOfSGs = LoadList(row.Value).ToSgList()
                    If s_listOfSGs.Count > 2 Then
                        s_lastBGValue = s_listOfSGs.Item(s_listOfSGs.Count - 2).sg
                    End If

                Case ItemIndexs.limits
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    LimitsRecordHelpers.UpdateListOflimitRecords(row)

                Case ItemIndexs.markers
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ItemIndexs.notificationHistory
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_notificationHistoryValue = Loads(row.Value)

                Case ItemIndexs.therapyAlgorithmState
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    s_theraphyAlgorthmStateValue = Loads(row.Value)

                Case ItemIndexs.pumpBannerState
                    s_pumpBannerStateValue = LoadList(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))
                    Me.TempTargetLabel.Visible = False

                Case ItemIndexs.basal
                    s_basalValue = Loads(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, ClickToShowDetails))

                Case ItemIndexs.systemStatusMessage
                    s_systemStatusMessage = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, s_sensorMessages, NameOf(s_sensorMessages)))

                Case ItemIndexs.averageSG
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.belowHypoLimit
                    s_belowHypoLimit = row.Value.ParseSingle(1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time below limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ItemIndexs.aboveHyperLimit
                    s_aboveHyperLimit = row.Value.ParseSingle(1)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time above limit = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ItemIndexs.timeInRange
                    s_timeInRange = CInt(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"Time in range = {ConvertPercent24HoursToDisplayValueString(row.Value)}"))

                Case ItemIndexs.pumpCommunicationState
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.gstCommunicationState
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                    Dim gstBatteryLevel As String = Nothing
                    If Me.RecentData.TryGetValue(NameOf(ItemIndexs.gstBatteryLevel), gstBatteryLevel) Then
                        Continue For
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexs.gstBatteryLevel, "0", "No data from pump"))

                Case ItemIndexs.gstBatteryLevel
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row, $"{row.Value}%"))

                Case ItemIndexs.lastConduitDateTime
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastConduitDateTime), CurrentUICulture))))

                Case ItemIndexs.maxAutoBasalRate
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.maxBolusAmount
                    s_listOfSummaryRecords.Add(New SummaryRecord(GetItemIndex(c.Value.Key), row))
                    Dim tempStr As String = Nothing
                    If Not Me.RecentData.TryGetValue(NameOf(ItemIndexs.sensorDurationMinutes), tempStr) Then
                        s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexs.sensorDurationMinutes, "-1", "No data from pump"))
                    End If
                    If Not Me.RecentData.TryGetValue(NameOf(ItemIndexs.timeToNextCalibrationMinutes), tempStr) Then
                        s_timeToNextCalibrationMinutes = UShort.MaxValue
                        s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexs.timeToNextCalibrationMinutes, s_timeToNextCalibrationMinutes.ToString, "No data from pump"))
                    End If
                Case ItemIndexs.sensorDurationMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.timeToNextCalibrationMinutes
                    s_timeToNextCalibrationMinutes = CUShort(row.Value)
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.clientTimeZoneName
                    s_clientTimeZoneName = row.Value
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, s_clientTimeZoneName))

                Case ItemIndexs.sgBelowLimit,
                        ItemIndexs.averageSGFloat
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.timeToNextCalibrationRecommendedMinutes
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

                Case ItemIndexs.calFreeSensor,
                         ItemIndexs.finalCalibration
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
            End Select
        Next

        Me.Cursor = Cursors.Default
    End Sub

#End Region ' Update Data and Tables

#Region "Update Home Tab"

    Private Sub UpdateActiveInsulin()
        Try
            Dim activeInsulinStr As String = $"{s_activeInsulin.amount:N3}"
            Me.ActiveInsulinValue.Text = $"Active Insulin{Environment.NewLine}{activeInsulinStr} U"
            _bgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr}U"
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}")
        End Try
    End Sub

    Private Sub UpdateActiveInsulinChart(aitChart As Chart)
        If Not Me.Initialized Then
            Exit Sub
        End If

        Try
            Dim lastTimeChangeRecord As TimeChangeRecord = Nothing
            For Each s As Series In aitChart.Series
                s.Points.Clear()
            Next
            With aitChart
                .Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Active Insulin in Pink"
                .ChartAreas(NameOf(ChartArea)).InitializeChartAreaBG()

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
                        Case "INSULIN"
                            Dim bolusAmount As Single = marker.Value.GetSingleValue(NameOf(InsulinRecord.deliveredFastAmount))
                            If timeOrderedMarkers.ContainsKey(sgOADateTime) Then
                                timeOrderedMarkers(sgOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(sgOADateTime, bolusAmount)
                            End If
                        Case "TIME_CHANGE"
                            lastTimeChangeRecord = New TimeChangeRecord(s_markers(marker.Index))
                        Case "CALIBRATION"
                        Case "MEAL"
                        Case Else
                            Stop
                    End Select
                Next

                If lastTimeChangeRecord IsNot Nothing Then
                    .ChartAreas(NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
                End If
                ' set up table that holds active insulin for every 5 minutes
                Dim remainingInsulinList As New List(Of RunningActiveInsulinRecord)
                Dim currentMarker As Integer = 0

                For i As Integer = 0 To 287
                    Dim initialBolus As Single = 0
                    Dim firstNotSkippedOaTime As New OADate((s_listOfSGs(0).datetime + (s_fiveMinuteSpan * i)).RoundTimeDown(RoundTo.Minute))
                    While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= firstNotSkippedOaTime
                        initialBolus += timeOrderedMarkers.Values(currentMarker)
                        currentMarker += 1
                    End While
                    remainingInsulinList.Add(New RunningActiveInsulinRecord(firstNotSkippedOaTime, initialBolus, Me.MenuOptionsUseAdvancedAITDecay.Checked))
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY2.Maximum = HomePageBasalRow
                ' walk all markers, adjust active insulin and then add new marker
                Dim maxActiveInsulin As Double = 0
                For i As Integer = 0 To remainingInsulinList.Count - 1
                    If i < s_activeInsulinIncrements Then
                        .Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaDateTime, Double.NaN)
                        .Series(NameOf(ActiveInsulinSeries)).Points.Last.IsEmpty = True
                        If i > 0 Then
                            remainingInsulinList.Adjustlist(0, i)
                        End If
                        Continue For
                    End If
                    Dim startIndex As Integer = i - s_activeInsulinIncrements + 1
                    Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, s_activeInsulinIncrements)
                    maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                    .Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaDateTime, sum)
                    remainingInsulinList.Adjustlist(startIndex, s_activeInsulinIncrements)
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1

                .PlotMarkers(_homePageAbsoluteRectangle, s_homeTabMarkerInsulinDictionary, s_homeTabMarkerMealDictionary)
                .PlotSgSeries(HomePageMealRow)
            End With
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulinChart)}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateAutoModeShield()
        Try
            Me.LastSGTimeLabel.Text = s_lastSgRecord.datetime.ToShortTimeString
            Me.ShieldUnitsLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.Text = BgUnitsString
            If Not Single.IsNaN(s_lastSgRecord.sg) Then
                Me.CurrentBGLabel.Visible = True
                Me.CurrentBGLabel.Text = s_lastSgRecord.sg.ToString
                Me.UpdateNotifyIcon()
                _bgMiniDisplay.SetCurrentBGString(s_lastSgRecord.sg.ToString)
                Me.SensorMessage.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield
                Me.ShieldUnitsLabel.Visible = True
            Else
                _bgMiniDisplay.SetCurrentBGString("---")
                Me.CurrentBGLabel.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield_Disabled
                Me.SensorMessage.Visible = True
                Me.SensorMessage.BackColor = Color.Transparent
                Dim message As String = ""
                If s_sensorMessages.TryGetValue(s_sensorState, message) Then
                    Dim splitMessage As String = message.Split(".")(0)
                    If message.Contains("...") Then
                        message = splitMessage & "..."
                    Else
                        message = splitMessage
                    End If
                Else
                    If Debugger.IsAttached Then
                        MsgBox($"{s_sensorState} is unknown sensor message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                    End If

                    message = message.ToTitle
                End If
                Me.SensorMessage.Text = message
                Me.SensorMessage.Visible = True
                Me.ShieldUnitsLabel.Visible = False
                Application.DoEvents()
            End If
            If _bgMiniDisplay.Visible Then
                _bgMiniDisplay.BGTextBox.SelectionLength = 0
            End If
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateAutoModeShield)}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        Try
            If s_timeToNextCalibHours > Byte.MaxValue Then
                Me.CalibrationDueImage.Image = My.Resources.CalibrationUnavailable
            ElseIf s_timeToNextCalibHours = 0 Then
                Me.CalibrationDueImage.Image = If(s_systemStatusMessage = "WAIT_TO_CALIBRATE" OrElse s_sensorState = "WARM_UP" OrElse s_sensorState = "CHANGE_SENSOR",
                My.Resources.CalibrationNotReady,
                My.Resources.CalibrationDotRed.DrawCenteredArc(s_timeToNextCalibrationMinutes))
            Else
                Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(s_timeToNextCalibrationMinutes)
            End If
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateCalibrationTimeRemaining)}")
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
                        Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
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
        Me.Last24HourBasalLabel.Text = $"Basal {s_totalBasal.RoundSingle(1)} U | {totalPercent}%"

        Me.Last24DailyDoseLabel.Text = $"Daily Dose {s_totalDailyDose.RoundSingle(1)} U"

        If s_totalAutoCorrection > 0 Then
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalAutoCorrection / s_totalDailyDose * 100).ToString
            End If
            Me.Last24AutoCorrectionLabel.Text = $"Auto Correction {s_totalAutoCorrection.RoundSingle(1)} U | {totalPercent}%"
            Me.Last24AutoCorrectionLabel.Visible = True
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24ManualBolusLabel.Text = $"Manual Bolus {s_totalManualBolus.RoundSingle(1)} U | {totalPercent}%"
        Else
            Me.Last24AutoCorrectionLabel.Visible = False
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24ManualBolusLabel.Text = $"Manual Bolus {s_totalManualBolus.RoundSingle(1)} U | {totalPercent}%"
        End If
        Me.Last24CarbsValueLabel.Text = $"Carbs = {s_totalCarbs} {s_sessionCountrySettings.carbohydrateUnitsDefault.ToTitle}"
    End Sub

    Private Sub UpdateHomeTabSerieses()
        Try
            For Each s As Series In Me.HomeTabChart.Series
                s.Points.Clear()
            Next
            Me.HomeTabChart.ChartAreas(NameOf(ChartArea)).InitializeChartAreaBG()
            Me.HomeTabChart.PlotMarkers(_homePageAbsoluteRectangle, s_homeTabMarkerInsulinDictionary, s_homeTabMarkerMealDictionary)
            Me.HomeTabChart.PlotSgSeries(HomePageMealRow)
            Me.HomeTabChart.PlotHighLowLimits()
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception while plotting Markers in {NameOf(UpdateHomeTabSerieses)}")
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
        If Not s_listOfSummaryRecords.GetValue(Of Boolean)(NameOf(ItemIndexs.conduitInRange)) Then
            Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryUnknown
            Me.PumpBatteryRemainingLabel.Text = $"Unknown"
            Exit Sub
        End If

        Dim batteryLeftPercent As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(NameOf(ItemIndexs.medicalDeviceBatteryLevelPercent))
        Select Case batteryLeftPercent
            Case > 90
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryFull
                Me.PumpBatteryRemainingLabel.Text = $"Full{Environment.NewLine}{batteryLeftPercent}%"
            Case > 50
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryHigh
                Me.PumpBatteryRemainingLabel.Text = $"High{Environment.NewLine}{batteryLeftPercent}%"
            Case > 25
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryMedium
                Me.PumpBatteryRemainingLabel.Text = $"Medium{Environment.NewLine}{batteryLeftPercent}%"
            Case > 10
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryLow
                Me.PumpBatteryRemainingLabel.Text = $"Low{Environment.NewLine}{batteryLeftPercent}%"
            Case Else
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryCritical
                Me.PumpBatteryRemainingLabel.Text = $"Critical{Environment.NewLine}{batteryLeftPercent}%"
        End Select
    End Sub

    Private Sub UpdateRemainingInsulin()
        Try
            Me.RemainingInsulinUnits.Text = $"{s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.reservoirRemainingUnits)).ParseSingle(1):N1} U"
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateRemainingInsulin)}")
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
                Dim sensorDurationMinutes As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(NameOf(ItemIndexs.sensorDurationMinutes), False)
                If sensorDurationMinutes = 0 Then
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpired
                    Me.SensorTimeLeftLabel.Text = $"Expired"
                Else
                    Me.SensorDaysLeftLabel.Text = $"1"
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                    Me.SensorTimeLeftLabel.Text = $"{sensorDurationMinutes} Minutes"
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
                .AddXY($"{s_belowHypoLimit}% Below {s_limitLow} {BgUnitsString}", s_belowHypoLimit / 100)
                .Last().Color = Color.Red
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_aboveHyperLimit}% Above {s_limitHigh} {BgUnitsString}", s_aboveHyperLimit / 100)
                .Last().Color = Color.Yellow
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

        Dim averageSgStr As String = s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.averageSG))
        Me.AboveHighLimitValueLabel.Text = $"{s_aboveHyperLimit} %"
        Me.AverageSGMessageLabel.Text = $"Average SG in {BgUnitsString}"
        Me.AverageSGValueLabel.Text = If(BgUnitsString = "mg/dl", averageSgStr, averageSgStr.TruncateSingleString(2))
        Me.BelowLowLimitValueLabel.Text = $"{s_belowHypoLimit} %"
        Me.SerialNumberLabel.Text = s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.medicalDeviceSerialNumber))
        Me.TimeInRangeChartLabel.Text = s_timeInRange.ToString
        Me.TimeInRangeValueLabel.Text = $"{s_timeInRange} %"

    End Sub

    Private Sub UpdateTreatmentChart(<CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        If Not _Initialized Then
            Exit Sub
        End If
        Try
            Me.InitializeTreatmentMarkersChart()
            Me.TreatmentMarkersChart.Titles(NameOf(TreatmentMarkersChartTitle)).Text = $"Treatment Makers"
            Me.TreatmentMarkersChart.ChartAreas(NameOf(ChartArea)).InitializeChartAreaBG()
            Me.TreatmentMarkersChart.PlotTreatmentMarkers()
            Me.TreatmentMarkersChart.PlotSgSeries(HomePageMealRow)
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(InitializeTreatmentMarkersChart)}")
        End Try
        Application.DoEvents()
    End Sub

    Friend Sub UpdateAllTabPages()
        If Me.RecentData Is Nothing Then
            Debug.Print($"Exiting {NameOf(UpdateAllTabPages)}, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Dim lastMedicalDeviceDataUpdateServerTimeEpoch As String = ""
        If Me.RecentData?.TryGetValue(NameOf(ItemIndexs.lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerTimeEpoch) Then
            If CLng(lastMedicalDeviceDataUpdateServerTimeEpoch) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                Me.RecentData = Nothing
                Exit Sub
            End If
        End If

        If Me.RecentData.Count > ItemIndexs.finalCalibration + 1 Then
            Stop
        End If
        Debug.Print($"In {NameOf(UpdateAllTabPages)} before SyncLock")
        SyncLock _updatingLock
            Debug.Print($"In {NameOf(UpdateAllTabPages)} inside SyncLock")
            _updating = True ' prevent paint
            _homePageAbsoluteRectangle = RectangleF.Empty
            _treatmentMarkerAbsoluteRectangle = RectangleF.Empty
            Me.MenuStartHere.Enabled = False
            Me.LastUpdateTime.ForeColor = SystemColors.ControlText
            If Not Me.LastUpdateTime.Text.Contains("from file") Then
                Me.LastUpdateTime.Text = Now.ToShortDateTimeString
            Else
                Me.LastUpdateTime.Text = Now.ToShortDateTimeString
            End If
            Me.CursorPanel.Visible = False

            ' Update all Markets
            Dim markerRowString As String = ""
            If Me.RecentData.TryGetValue(ItemIndexs.markers.ToString, markerRowString) Then
                Me.MaxBasalPerHourLabel.Text = CollectMarkers(markerRowString)
            Else
                Me.MaxBasalPerHourLabel.Text = ""
            End If

            Me.UpdateDataTables()
            _updating = False
        End SyncLock
        Debug.Print($"In {NameOf(UpdateAllTabPages)} exited SyncLock")

        Dim rowValue As String = s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.lastSGTrend))
        Dim arrows As String = Nothing
        If Trends.TryGetValue(rowValue, arrows) Then
            Me.LabelTrendArrows.Text = Trends(rowValue)
        Else
            Me.LabelTrendArrows.Text = $"{rowValue}"
        End If
        Me.UpdateSummaryTab()
        Me.UpdateActiveInsulinChart(Me.ActiveInsulinChart)
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
        Me.ActiveInsulinChart.PlotMarkers(_activeInsulinChartAbsoluteRectangle, s_ActiveInsulinMarkerInsulinDictionary, Nothing)
        Me.UpdateDosingAndCarbs()

        Me.AboveHighLimitMessageLabel.Text = $"Above {s_limitHigh} {BgUnitsString}"
        Me.BelowLowLimitMessageLabel.Text = $"Below {s_limitLow} {BgUnitsString}"
        Me.FullNameLabel.Text = $"{s_firstName} {s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.lastName))}"
        Me.ModelLabel.Text = s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexs.pumpModelNumber))
        Me.ReadingsLabel.Text = $"{s_listOfSGs.Where(Function(entry As SgRecord) Not Single.IsNaN(entry.sg)).Count}/288"

        Me.TableLayoutPanelLastSG.DisplayDataTableInDGV(
                              ClassToDatatable({s_lastSgRecord}.ToArray),
                              NameOf(SgRecord),
                              AddressOf SgRecordHelpers.AttachHandlers,
                              ItemIndexs.lastSG,
                              True)

        Me.TableLayoutPanelLastAlarm.DisplayDataTableInDGV(
                              ClassToDatatable(GetSummaryRecords(s_lastAlarmValue).ToArray),
                              NameOf(LastAlarmRecord),
                              AddressOf SummaryRecordHelpers.AttachHandlers,
                              ItemIndexs.lastAlarm,
                              True)

        Me.TableLayoutPanelActiveInsulin.DisplayDataTableInDGV(
                              ClassToDatatable({s_activeInsulin}.ToArray),
                              NameOf(ActiveInsulinRecord),
                              AddressOf ActiveInsulinRecordHelpers.AttachHandlers,
                              ItemIndexs.activeInsulin,
                              True)

        Me.UpdateSgsTab()

        Me.TableLayoutPanelLimits.DisplayDataTableInDGV(
                              ClassToDatatable(s_listOflimitRecords.ToArray),
                              NameOf(LimitsRecord),
                              AddressOf LimitsRecordHelpers.AttachHandlers,
                              ItemIndexs.limits,
                              False)

        Me.UpdateMarkerTabs()

        Me.UpdateNotificationTab()

        Me.TableLayoutPanelTherapyAlgorithm.DisplayDataTableInDGV(
                              ClassToDatatable(GetSummaryRecords(s_theraphyAlgorthmStateValue).ToArray),
                              NameOf(SummaryRecord),
                              AddressOf SummaryRecordHelpers.AttachHandlers,
                              ItemIndexs.therapyAlgorithmState,
                              True)

        Me.UpdatePumpBannerStateTab()

        Me.TableLayoutPanelBasal.DisplayDataTableInDGV(
                              ClassToDatatable({DictionaryToClass(Of BasalRecord)(s_basalValue, 0)}.ToArray),
                              NameOf(BasalRecord),
                              AddressOf BasalRecordHelpers.AttachHandlers,
                              ItemIndexs.basal,
                              True)

        s_recentDatalast = Me.RecentData
        Me.MenuStartHere.Enabled = True
        Me.UpdateTreatmentChart()
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

    ' Save the current scale value
    ' ScaleControl() is called during the Form'AiTimeInterval constructor
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        _formScale = New SizeF(_formScale.Width * factor.Width, _formScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
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
            Dim sg As Single = s_lastSgRecord.sg
            Dim str As String = s_lastSgRecord.sg.ToString
            Dim fontToUse As New Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel)
            Dim color As Color = Color.White
            Dim bgColor As Color
            Dim notStr As New StringBuilder

            Using bitmapText As New Bitmap(16, 16)
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
                    notStr.Append($"{s_activeInsulin.amount:N3}")
                    notStr.Append("U"c)
                    Me.NotifyIcon1.Text = notStr.ToString
                    s_lastBGValue = sg
                End Using
            End Using
        Catch ex As Exception
            Stop
            ' ignore errors
        End Try
    End Sub

#End Region 'NotifyIcon Support

End Class
