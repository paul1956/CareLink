' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms.DataVisualization.Charting

Imports DataGridViewColumnControls

Imports TableLayputPanelTop

Public Class Form1

    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
    Private ReadOnly _sgMiniDisplay As New BGMiniWindow(Me)
    Private ReadOnly _updatingLock As New Object
    Private _activeInsulinChartAbsoluteRectangle As RectangleF = RectangleF.Empty
    Private _formScale As New SizeF(1.0F, 1.0F)
    Private _inMouseMove As Boolean = False
    Private _lastMarkerTabIndex As (page As Integer, tab As Integer) = (0, 0)
    Private _lastSummaryTabIndex As Integer = 0
    Private _previousLoc As Point
    Private _showBalloonTip As Boolean = True
    Private _summaryChartAbsoluteRectangle As RectangleF
    Private _treatmentMarkerAbsoluteRectangle As RectangleF
    Private _updating As Boolean

    Public Property Initialized As Boolean = False

    Public Property Client As CareLinkClient
        Get
            Return Me.LoginDialog?.Client
        End Get
        Set(value As CareLinkClient)
            Me.LoginDialog.Client = value
        End Set
    End Property

    Public ReadOnly Property LoginDialog As New LoginForm1

#Region "Pump Data"

    Friend Property RecentData As New Dictionary(Of String, String)

#End Region ' Pump Data

#Region "Chart Objects"

#Region "Charts"

    Private WithEvents ActiveInsulinChart As Chart
    Private WithEvents SummaryChart As Chart
    Private WithEvents TimeInRangeChart As Chart
    Private WithEvents TreatmentMarkersChart As Chart

#End Region

#Region "Legends"

    Friend _activeInsulinChartLegend As Legend
    Friend _summaryChartLegend As Legend
    Friend _treatmentMarkersChartLegend As Legend

#End Region

#Region "Series"

#Region "Common Series"

    Public ActiveInsulinActiveInsulinSeries As Series
    Public ActiveInsulinAutoCorrectionSeries As Series
    Public ActiveInsulinBasalSeries As Series
    Public ActiveInsulinMarkerSeries As Series
    Public ActiveInsulinMinBasalSeries As Series
    Public ActiveInsulinSgSeries As Series
    Public ActiveInsulinTargetSeries As Series
    Public ActiveInsulinTimeChangeSeries As Series

    Public SummaryAutoCorrectionSeries As Series
    Public SummaryBasalSeries As Series
    Public SummaryHighLimitSeries As Series
    Public SummaryLowLimitSeries As Series
    Public SummaryMarkerSeries As Series
    Public SummaryMinBasalSeries As Series
    Public SummarySgSeries As Series
    Public SummaryTargetSgSeries As Series
    Public SummaryTimeChangeSeries As Series

    Public TimeInRangeSeries As New Series

    Public TreatmentMarkerAutoCorrectionSeries As Series
    Public TreatmentMarkerBasalSeries As Series
    Public TreatmentMarkerMarkersSeries As Series
    Public TreatmentMarkerMinBasalSeries As Series
    Public TreatmentMarkerSgSeries As Series
    Public TreatmentMarkerTimeChangeSeries As Series
    Public TreatmentTargetSeries As Series

#End Region

#End Region

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As New Title
    Private WithEvents TreatmentMarkersChartTitle As Title

#End Region

#End Region ' Chart Objects

#Region "Events"

#Region "Form Events"

    Private Sub ActiveInsulinValue_Paint(sender As Object, e As PaintEventArgs) Handles ActiveInsulinValue.Paint
        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.LimeGreen, ButtonBorderStyle.Solid)
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        Me.CleanUpNotificationIcon()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.CleanUpNotificationIcon()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.UpgradeRequired Then
            My.Settings.Upgrade()
            My.Settings.UpgradeRequired = False
            My.Settings.Save()
        End If

        Dim currentAllUserLoginFile As String = GetPathToAllUserLoginInfo(True)
        If Not Directory.Exists(GetDirectoryForProjectData()) Then
            Dim lastError As String = $"Can't create required project directories!"
            Directory.CreateDirectory(GetDirectoryForProjectData())
            Directory.CreateDirectory(GetDirectoryForSettings())
            Try
                MoveIfExists(GetPathToAllUserLoginInfo(False), currentAllUserLoginFile, lastError)
                MoveIfExists(GetPathToGraphColorsFile(False), GetPathToGraphColorsFile(True), lastError)
                MoveIfExists(GetPathToShowLegendFile(False), GetPathToShowLegendFile(True), lastError)

                ' Move files with unique/variable names
                ' Error Reports
                lastError = $"Moving {SavedErrorReportBaseName} files!"
                MoveFiles(GetDirectoryForMyDocuments, GetDirectoryForProjectData, $"{SavedErrorReportBaseName}*.txt")
                lastError = $"Moving {SavedLastDownloadBaseName} files!"
                MoveFiles(GetDirectoryForMyDocuments, GetDirectoryForProjectData, $"{SavedLastDownloadBaseName}*.json")
                lastError = $"Moving {SavedSnapshotBaseName} files!"
                MoveFiles(GetDirectoryForMyDocuments, GetDirectoryForProjectData, $"{SavedSnapshotBaseName}*.json")
            Catch ex As Exception
                MsgBox($"{lastError}{s_environmentNewLine}{ex.Message}", MsgBoxStyle.Critical, "Fatal Error")
                End
            End Try
        End If
        If Not Directory.Exists(GetDirectoryForSettings) Then
            Directory.CreateDirectory(GetDirectoryForSettings())
        End If

        If File.Exists(currentAllUserLoginFile) Then
            s_allUserSettingsData.LoadUserRecords(currentAllUserLoginFile)
        End If

        AddHandler My.Settings.SettingChanging, AddressOf Me.MySettings_SettingChanging

        If File.Exists(GetPathToGraphColorsFile(True)) Then
            OptionsDialog.GetColorDictionaryFromFile()
            Me.MenuOptionsShowLegend.Checked = File.Exists(GetPathToShowLegendFile(True))
        Else
            OptionsDialog.WriteColorDictionaryToFile()
            File.Create(GetPathToShowLegendFile(True))
        End If

        Me.InsulinTypeLabel.Text = s_insulinTypes.Keys(1)
        AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf Me.PowerModeChanged
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
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
            Me.UpdateAllTabPages(False)
        End If
    End Sub

#End Region ' Form Events

#Region "Form Menu Events"

#Region "Start Here Menu Events"

    Private Sub MenuStartHere_DropDownOpening(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpening
        Me.MenuStartHereLoadSavedDataFile.Enabled = AnyMatchingFiles($"{ProjectName}*.json")
        Me.MenuStartHereSnapshotSave.Enabled = Me.RecentData IsNot Nothing AndAlso Me.RecentData.Count > 0
        Me.MenuStartHereExceptionReportLoad.Visible = AnyMatchingFiles($"{SavedErrorReportBaseName}*.txt")
    End Sub

    Private Sub MenuStartHereExceptionReportLoad_Click(sender As Object, e As EventArgs) Handles MenuStartHereExceptionReportLoad.Click
        Dim fileList As String() = Directory.GetFiles(GetDirectoryForProjectData(), $"{SavedErrorReportBaseName}*.txt")
        Dim openFileDialog1 As New OpenFileDialog With {
            .CheckFileExists = True,
            .CheckPathExists = True,
            .FileName = If(fileList.Length > 0, Path.GetFileName(fileList(0)), ProjectName),
            .Filter = $"Error files (*.txt)|{SavedErrorReportBaseName}*.txt",
            .InitialDirectory = GetDirectoryForProjectData(),
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select {ProjectName}{TmChar} saved snapshot to load",
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
                        CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"{ProjectName}", True)
                        Me.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                        Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                        Me.SetLastUpdateTime($"{s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2DateTime.ToShortDateTimeString} from file", False)
                        SetUpCareLinkUser(Me, GetPathToTestSettingsFile())

                        Try
                            Me.FinishInitialization()
                            Try
                                Me.UpdateAllTabPages(True)
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

    Private Sub MenuStartHereLoadSavedDataFile_Click(sender As Object, e As EventArgs) Handles MenuStartHereLoadSavedDataFile.Click
        Dim di As New DirectoryInfo(GetDirectoryForProjectData())
        Dim fileList As String() = New DirectoryInfo(GetDirectoryForProjectData()).
                                        EnumerateFiles($"{ProjectName}*.json").
                                        OrderBy(Function(f As FileInfo) f.LastWriteTime).
                                        Select(Function(f As FileInfo) f.Name).ToArray
        Dim openFileDialog1 As New OpenFileDialog With {
            .CheckFileExists = True,
            .CheckPathExists = True,
            .FileName = If(fileList.Length > 0, fileList.Last, ProjectName),
            .Filter = $"json files (*.json)|{ProjectName}*.json",
            .InitialDirectory = GetDirectoryForProjectData(),
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select CareLink{TmChar} saved snapshot to load",
            .ValidateNames = True
        }

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                If File.Exists(openFileDialog1.FileName) Then
                    Me.ServerUpdateTimer.Stop()
                    SetUpCareLinkUser(Me, GetPathToTestSettingsFile())
                    Debug.Print($"In {NameOf(MenuStartHereLoadSavedDataFile_Click)}, {NameOf(Me.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
                    CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"{ProjectName}", True)
                    CurrentUICulture = CurrentDateCulture

                    Me.RecentData = Loads(File.ReadAllText(openFileDialog1.FileName))
                    Me.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                    Me.Text = $"{SavedTitle} Using file {Path.GetFileName(openFileDialog1.FileName)}"
                    Me.SetLastUpdateTime(File.GetLastWriteTime(openFileDialog1.FileName).ToShortDateTimeString, False)
                    Me.FinishInitialization()
                    Me.UpdateAllTabPages(True)
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
            File.WriteAllText(GetDataFileName(SavedSnapshotBaseName, CurrentDateCulture.Name, "json", True).withPath, JsonSerializer.Serialize(jd, JsonFormattingOptions))
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

    Private Sub StartHereExit_Click(sender As Object, e As EventArgs) Handles StartHereExit.Click
        Me.CleanUpNotificationIcon()
    End Sub

#End Region ' Start Here Menu Events

#Region "Option Menus"

    Private Sub MenuOptionsAutoLogin_CheckedChanged(sender As Object, e As EventArgs) Handles MenuOptionsAutoLogin.CheckedChanged
        My.Settings.AutoLogin = Me.MenuOptionsAutoLogin.Checked
    End Sub

    Private Sub MenuOptionsColorPicker_Click(sender As Object, e As EventArgs) Handles MenuOptionsColorPicker.Click
        Using o As New OptionsDialog()
            o.ShowDialog(Me)
        End Using
    End Sub

    Private Sub MenuOptionsEditPumpSettings_Click(sender As Object, e As EventArgs) Handles MenuOptionsEditPumpSettings.Click
        Dim contents As String = File.ReadAllText(GetPathToUserSettingsFile(My.Settings.CareLinkUserName))
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, JsonFormattingOptions)
        Dim f As New InitializeDialog With {
            .CurrentUser = CurrentUser,
            .RecentData = Me.RecentData
            }
        If f.ShowDialog() = DialogResult.OK Then
            CurrentUser = f.CurrentUser
        End If

    End Sub

    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked

        Dim lastColumnIndex As Integer = Me.DgvAutoBasalDelivery.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvAutoBasalDelivery.Columns(i)
            If i > 0 AndAlso String.IsNullOrWhiteSpace(c.DataPropertyName) Then
                Stop
            End If
            c.Visible = Not AutoBasalDeliveryRecordHelpers.HideColumn(c.DataPropertyName)
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Next

        lastColumnIndex = Me.DgvCareLinkUsers.Columns.Count - 1

        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvCareLinkUsers.Columns(i)
            If i > 0 AndAlso String.IsNullOrWhiteSpace(c.DataPropertyName) Then
                Stop
            End If
            c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
            c.AutoSizeMode = If(i = lastColumnIndex, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.AllCells)
        Next

        lastColumnIndex = Me.DgvInsulin.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvInsulin.Columns(i)
            c.Visible = Not InsulinRecordHelpers.HideColumn(c.DataPropertyName)
        Next

        lastColumnIndex = Me.DgvMeal.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvMeal.Columns()(i)
            c.AutoSizeMode = If(i = lastColumnIndex, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.AllCells)
        Next

        lastColumnIndex = Me.DgvSGs.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvSGs.Columns()(i)
            c.Visible = Not SgRecordHelpers.HideColumn(c.DataPropertyName)
            c.AutoSizeMode = If(c.HeaderText = "Sensor Message", DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.AllCells)
        Next
    End Sub

    Private Sub MenuOptionsShowLegend_CheckStateChanged(sender As Object, e As EventArgs) Handles MenuOptionsShowLegend.CheckStateChanged
        If Not Me.Initialized Then Exit Sub
        If Me.MenuOptionsShowLegend.Checked Then
            File.Create(GetPathToShowLegendFile(True))
            _activeInsulinChartLegend.Enabled = True
            _summaryChartLegend.Enabled = True
            _treatmentMarkersChartLegend.Enabled = True
        Else
            File.Delete(GetPathToShowLegendFile(True))
            _activeInsulinChartLegend.Enabled = False
            _summaryChartLegend.Enabled = False
            _treatmentMarkersChartLegend.Enabled = False
        End If
    End Sub

    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) Handles MenuOptionsUseLocalTimeZone.Click
        Dim saveRequired As Boolean = Me.MenuOptionsUseLocalTimeZone.Checked <> My.Settings.UseLocalTimeZone
        If Me.MenuOptionsUseLocalTimeZone.Checked Then
            PumpTimeZoneInfo = TimeZoneInfo.Local
            My.Settings.UseLocalTimeZone = True
        Else
            PumpTimeZoneInfo = CalculateTimeZone(Me.RecentData(NameOf(ItemIndexes.clientTimeZoneName)))
            My.Settings.UseLocalTimeZone = False
        End If
        If saveRequired Then My.Settings.Save()
    End Sub

#End Region ' Option Menus

#Region "View Menu Events"

    Private Sub MenuShowMiniDisplay_Click(sender As Object, e As EventArgs) Handles MenuShowMiniDisplay.Click
        Me.Hide()
        _sgMiniDisplay.Show()
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

#Region "Tab Events"

    Private Sub TabControlPage1_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage1.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPage14Markers)
                Me.DgvCareLinkUsers.InitializeDgv

                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
                If _lastMarkerTabIndex.page = 0 Then
                    Me.TabControlPage2.SelectedIndex = 0
                Else
                    Me.TabControlPage2.SelectedIndex = _lastMarkerTabIndex.tab
                End If
                Me.TabControlPage1.Visible = False
                Exit Sub
            Case NameOf(TabPage05Insulin)
                _lastMarkerTabIndex = (0, e.TabPageIndex)
            Case NameOf(TabPage06Meal)
                _lastMarkerTabIndex = (0, e.TabPageIndex)
        End Select
        _lastSummaryTabIndex = e.TabPageIndex
    End Sub

    Private Sub TabControlPage2_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage2.Selecting
        Select Case e.TabPage.Name
            Case NameOf(TabPageBackToHomePage)
                Me.TabControlPage1.SelectedIndex = _lastSummaryTabIndex
                Me.TabControlPage1.Visible = True
                Exit Sub
            Case NameOf(TabPageAllUsers)
                Me.DgvCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
            Case Else
                If e.TabPageIndex < Me.TabControlPage2.TabPages.Count - 2 Then
                    _lastMarkerTabIndex = (1, e.TabPageIndex)
                End If
        End Select
    End Sub

#End Region ' Tab Events

#Region "Summary Events"

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {PumpNow.AddMinutes(s_timeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub

    Private Sub SensorDaysLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles SensorDaysLeftLabel.MouseHover
        If s_sensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(Me.CalibrationDueImage, $"Sensor will expire in {s_sensorDurationHours} hours")
        End If
    End Sub

#End Region ' Summary Events

#Region "Chart Events"

    Private Sub Chart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles ActiveInsulinChart.CursorPositionChanging, SummaryChart.CursorPositionChanging
        If Not _Initialized Then Exit Sub

        Me.CursorTimer.Interval = CInt(s_30SecondInMilliseconds)
        Me.CursorTimer.Start()
    End Sub

    Private Sub Chart_MouseLeave(sender As Object, e As EventArgs) Handles SummaryChart.MouseLeave, ActiveInsulinChart.MouseLeave, TreatmentMarkersChart.MouseLeave
        Dim name As String = CType(sender, Chart).Name
        SetCalloutVisibility(name)
    End Sub

    Private Sub Chart_MouseMove(sender As Object, e As MouseEventArgs) Handles SummaryChart.MouseMove, ActiveInsulinChart.MouseMove, TreatmentMarkersChart.MouseMove

        If Not _Initialized Then
            Exit Sub
        End If
        If e.Button <> MouseButtons.None OrElse e.Clicks > 0 OrElse e.Location = _previousLoc Then
            Return
        End If
        _inMouseMove = True
        _previousLoc = e.Location
        Dim yInPixels As Double
        Dim chart1 As Chart = CType(sender, Chart)
        Dim isHomePage As Boolean = chart1.Name = NameOf(SummaryChart)
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
                Case HighLimitSeriesName, LowLimitSeriesName, TargetSgSeriesName
                    Me.CursorPanel.Visible = False
                Case MarkerSeriesName, BasalSeriesNameName
                    Dim markerTag() As String = currentDataPoint.Tag.ToString.Split(":"c)
                    If markerTag.Length <= 1 Then
                        If chart1.Name = NameOf(TreatmentMarkersChart) Then
                            Dim callout As CalloutAnnotation = chart1.FindAnnotation(currentDataPoint)
                            callout.BringToFront()
                        Else
                            Me.CursorPanel.Visible = True
                        End If
                        Exit Sub
                    End If
                    markerTag(0) = markerTag(0).Trim
                    If isHomePage Then
                        Dim xValue As Date = Date.FromOADate(currentDataPoint.XValue)
                        Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                        Me.CursorPictureBox.Visible = True
                        Select Case markerTag.Length
                            Case 2
                                Me.CursorMessage1Label.Text = markerTag(0)
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTag(1).Trim
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorMessage3Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(s_timeWithMinuteFormat)
                                Me.CursorMessage3Label.Visible = True
                                Select Case markerTag(0)
                                    Case "Auto Correction",
                                         "Auto Basal",
                                         "Basal",
                                         "Min Auto Basal"
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
                                Select Case markerTag(1).Trim
                                    Case "Calibration accepted",
                                           "Calibration not accepted"
                                        Me.CursorPictureBox.Image = My.Resources.CalibrationDotRed
                                    Case "Not used For calibration"
                                        Me.CursorPictureBox.Image = My.Resources.CalibrationDot
                                    Case Else
                                        Stop
                                End Select
                                Me.CursorMessage1Label.Text = markerTag(0)
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTag(1).Trim
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorMessage3Label.Text = $"{markerTag(2).Trim}@{xValue.ToString(s_timeWithMinuteFormat)}"
                                Me.CursorMessage3Label.Visible = True
                                Me.CursorPanel.Visible = True
                            Case Else
                                Stop
                                Me.CursorPanel.Visible = False
                        End Select
                    End If
                    chart1.SetUpCallout(currentDataPoint, markerTag)

                Case SgSeriesName
                    Me.CursorMessage1Label.Text = "Blood Glucose"
                    Me.CursorMessage1Label.Visible = True
                    Me.CursorMessage2Label.Text = $"{currentDataPoint.YValues(0).RoundToSingle(3)} {BgUnitsString}"
                    Me.CursorMessage2Label.Visible = True
                    Me.CursorMessage3Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(s_timeWithMinuteFormat)
                    Me.CursorMessage3Label.Visible = True
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorPanel.Visible = True
                    chart1.SetupCallout(currentDataPoint, $"Blood Glucose {Me.CursorMessage2Label.Text}")
                Case TimeChangeSeriesName
                    Me.CursorMessage1Label.Visible = False
                    Me.CursorMessage1Label.Visible = False
                    Me.CursorMessage2Label.Visible = False
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorMessage3Label.Visible = False
                    Me.CursorPanel.Visible = False
                Case ActiveInsulinSeriesName
                    chart1.SetupCallout(currentDataPoint, $"Theoretical Active Insulin {currentDataPoint.YValues.FirstOrDefault:F3} U")
                Case Else
                    Stop
            End Select
        Catch ex As Exception
            result = Nothing
        Finally
            _inMouseMove = False
        End Try
    End Sub

    Private Sub TemporaryUseAdvanceAITDecayCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TemporaryUseAdvanceAITDecayCheckBox.CheckedChanged
        If Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState = CheckState.Checked Then
            Me.TemporaryUseAdvanceAITDecayCheckBox.Text = $"Advanced Decay, AIT will decay over {CurrentUser.InsulinRealAit} hours while using {CurrentUser.InsulinTypeName}"
        Else
            Me.TemporaryUseAdvanceAITDecayCheckBox.Text = $"AIT will decay over {CurrentUser.PumpAit.ToHoursMinutes} while using {CurrentUser.InsulinTypeName}"
        End If
        CurrentUser.UseAdvancedAitDecay = Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState
        Me.UpdateActiveInsulinChart()
    End Sub

#Region "Post Paint Events"

    <DebuggerNonUserCode()>
    Private Sub ActiveInsulinChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles ActiveInsulinChart.PostPaint

        If Not _Initialized OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            e.PostPaintSupport(_activeInsulinChartAbsoluteRectangle,
                s_activeInsulinMarkerInsulinDictionary,
                Nothing,
                True,
                True)
        End SyncLock
    End Sub

    <DebuggerNonUserCode()>
    Private Sub SummaryChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles SummaryChart.PostPaint

        If Not _Initialized OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            e.PostPaintSupport(_summaryChartAbsoluteRectangle,
                s_summaryMarkerInsulinDictionary,
                s_summaryMarkerMealDictionary,
                True,
                True)
        End SyncLock
    End Sub

    <DebuggerNonUserCode()>
    Private Sub TreatmentMarkersChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles TreatmentMarkersChart.PostPaint

        If Not _Initialized OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            e.PostPaintSupport(_treatmentMarkerAbsoluteRectangle,
                s_treatmentMarkerInsulinDictionary,
                s_treatmentMarkerMealDictionary,
                offsetInsulinImage:=False,
                paintOnY2:=False)
        End SyncLock
    End Sub

#End Region ' Post Paint Events

#End Region ' Chart Events

#Region "DataGridView Events"

    Private WithEvents DgvCopyMenuStrip As New ContextMenuStrip

    Private Shared Function GetDataGridView(sender As Object) As DataGridView
        Dim contextStrip As ContextMenuStrip = CType(CType(sender, ToolStripMenuItem).GetCurrentParent, ContextMenuStrip)
        Dim dgv As DataGridView = CType(contextStrip.SourceControl, DataGridView)
        Return dgv
    End Function

    Private Sub Dgv_CellContextMenuStripNeeded(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles _
                        DgvAutoBasalDelivery.CellContextMenuStripNeeded,
                        DgvInsulin.CellContextMenuStripNeeded,
                        DgvMeal.CellContextMenuStripNeeded,
                        DgvSGs.CellContextMenuStripNeeded

        If e.RowIndex >= 0 Then
            e.ContextMenuStrip = Me.DgvCopyMenuStrip
        End If

    End Sub

    Private Sub DgvCopyMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles DgvCopyMenuStrip.Opening
        ' Acquire references to the owning control and item.

        Me.DgvCopyMenuStrip.Tag = CType(Me.DgvCopyMenuStrip.SourceControl, DataGridView)

        ' Clear the ContextMenuStrip control's Items collection.
        Me.DgvCopyMenuStrip.Items.Clear()

        ' Populate the ContextMenuStrip control with its default items.
        Me.DgvCopyMenuStrip.Items.Add("Copy with Header", My.Resources.Copy, AddressOf Me.DgvCopyToClipBoardWithHeader)
        Me.DgvCopyMenuStrip.Items.Add("Copy without Header", My.Resources.Copy, AddressOf Me.DgvCopyToClipBoardWithoutHeader)
        Me.DgvCopyMenuStrip.Items.Add("Save To Excel", My.Resources.ExportData, AddressOf Me.DgvExportToExcel)

        ' Set Cancel to false.
        ' It is optimized to true based on empty entry.
        e.Cancel = False
    End Sub

    Private Sub DgvCopyToClipBoardWithHeader(sender As Object, e As EventArgs)
        GetDataGridView(sender).CopyToClipboard(True)
    End Sub

    Private Sub DgvCopyToClipBoardWithoutHeader(sender As Object, e As EventArgs)
        GetDataGridView(sender).CopyToClipboard(False)
    End Sub

    Private Sub DgvExportToExcel(sender As Object, e As EventArgs)
        GetDataGridView(sender).ExportToExcelWithFormatting()
    End Sub

#Region "Dgv Summary Events"

    Private Sub DgvSummary_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvSummary.CellFormatting
        If e.Value Is Nothing OrElse e.ColumnIndex <> 2 Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String = dgv.Rows(e.RowIndex).Cells("key").Value.ToString
        Select Case CType([Enum].Parse(GetType(ItemIndexes), key), ItemIndexes)
            Case ItemIndexes.lastSensorTS, ItemIndexes.medicalDeviceTimeAsString,
                 ItemIndexes.lastSensorTSAsString, ItemIndexes.kind,
                 ItemIndexes.pumpModelNumber, ItemIndexes.currentServerTime,
                 ItemIndexes.lastConduitTime, ItemIndexes.lastConduitUpdateServerTime,
                 ItemIndexes.lastMedicalDeviceDataUpdateServerTime,
                 ItemIndexes.firstName, ItemIndexes.lastName, ItemIndexes.conduitSerialNumber,
                 ItemIndexes.conduitBatteryStatus, ItemIndexes.medicalDeviceFamily,
                 ItemIndexes.sensorState, ItemIndexes.medicalDeviceSerialNumber,
                 ItemIndexes.medicalDeviceTime, ItemIndexes.sMedicalDeviceTime,
                 ItemIndexes.calibStatus, ItemIndexes.bgUnits, ItemIndexes.timeFormat,
                 ItemIndexes.lastSensorTime, ItemIndexes.sLastSensorTime,
                 ItemIndexes.lastSGTrend, ItemIndexes.systemStatusMessage,
                 ItemIndexes.lastConduitDateTime, ItemIndexes.clientTimeZoneName
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case ItemIndexes.averageSG, ItemIndexes.version, ItemIndexes.conduitBatteryLevel,
                 ItemIndexes.reservoirLevelPercent, ItemIndexes.reservoirAmount,
                 ItemIndexes.reservoirRemainingUnits, ItemIndexes.medicalDeviceBatteryLevelPercent,
                 ItemIndexes.sensorDurationHours, ItemIndexes.timeToNextCalibHours,
                 ItemIndexes.belowHypoLimit, ItemIndexes.aboveHyperLimit,
                 ItemIndexes.timeInRange, ItemIndexes.gstBatteryLevel,
                 ItemIndexes.maxAutoBasalRate, ItemIndexes.maxBolusAmount,
                 ItemIndexes.sensorDurationMinutes,
                 ItemIndexes.timeToNextCalibrationMinutes, ItemIndexes.sgBelowLimit,
                 ItemIndexes.averageSGFloat,
                 ItemIndexes.timeToNextCalibrationRecommendedMinutes
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(0, 1, 1, 1))
            Case Else
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(1))
        End Select

    End Sub

    Private Sub DgvSummary_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvSummary.CellMouseClick
        If e.RowIndex < 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim value As String = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
        If value.StartsWith(ClickToShowDetails) Then
            With Me.TabControlPage1
                Select Case dgv.Rows(e.RowIndex).Cells("key").Value.ToString.GetItemIndex()
                    Case ItemIndexes.lastSG
                        Me.TabControlPage2.SelectedIndex = 6
                        _lastMarkerTabIndex = (1, 6)
                        .Visible = False
                    Case ItemIndexes.lastAlarm
                        Me.TabControlPage2.SelectedIndex = 7
                        _lastMarkerTabIndex = (1, 7)
                        .Visible = False
                    Case ItemIndexes.activeInsulin
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage07ActiveInsulin))
                    Case ItemIndexes.sgs
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage08SensorGlucose))
                    Case ItemIndexes.limits
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage09Limits))
                    Case ItemIndexes.markers
                        Dim page As Integer = _lastMarkerTabIndex.page
                        Dim tab As Integer = _lastMarkerTabIndex.tab
                        If page = 0 Then
                            If tab = 0 Then
                                _lastMarkerTabIndex = (0, 4)
                            End If
                            Me.TabControlPage1.SelectedIndex = _lastMarkerTabIndex.tab
                        Else
                            If 5 < tab Then
                                Me.TabControlPage2.SelectedIndex = 0
                            Else
                                Me.TabControlPage2.SelectedIndex = _lastMarkerTabIndex.tab
                            End If
                            .Visible = False
                        End If
                    Case ItemIndexes.notificationHistory
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage10NotificationHistory))
                    Case ItemIndexes.therapyAlgorithmState
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage11TherapyAlgorithm))
                    Case ItemIndexes.pumpBannerState
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage12BannerState))
                    Case ItemIndexes.basal
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage13Basal))
                End Select
            End With
        End If
    End Sub

    Private Sub DgvSummary_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvSummary.ColumnAdded
        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            e.DgvColumnAdded(SummaryRecordHelpers.GetCellStyle(.Name),
                             False,
                             True,
                             CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    Private Sub DgvSummary_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvSummary.DataError
        Stop
    End Sub

#End Region ' Dgv Summary Events

#Region "Dgv SGs Events"

    Private Sub DgvSGs_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvSGs.CellFormatting
        If e.Value Is Nothing Then
            Return
        End If
        With e.CellStyle
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim columnName As String = dgv.Columns(e.ColumnIndex).Name
            ' Set the background to red for negative values in the Balance column.
            If columnName.Equals(NameOf(SgRecord.sensorState), StringComparison.OrdinalIgnoreCase) Then
                If e.Value.ToString <> "NO_ERROR_MESSAGE" Then
                    FormatCell(e, Color.Red)
                End If
            End If
            dgv.dgvCellFormatting(e, NameOf(SgRecord.datetime))
            If columnName.Equals(NameOf(SgRecord.sg), StringComparison.OrdinalIgnoreCase) Then
                Dim sensorValue As Single = ParseSingle(e.Value, 2)
                If Single.IsNaN(sensorValue) Then
                    FormatCell(e, Color.Gray)
                ElseIf sensorValue < TirLowLimit() Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit() Then
                    FormatCell(e, Color.Yellow)
                End If
            End If
        End With

    End Sub

    Private Sub DgvSGs_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvSGs.ColumnAdded
        With e.Column
            If SgRecordHelpers.HideColumn(.Name) Then
                .Visible = False
                Exit Sub
            End If

            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(SgRecordHelpers.GetCellStyle(.Name),
                                 False,
                                 True,
                                 CType(dgv.DataSource, DataTable).Columns(.Index).Caption)

            Select Case .Index
                Case 0
                    .SortMode = DataGridViewColumnSortMode.Programmatic
                    .HeaderCell.SortGlyphDirection = SortOrder.Descending
                Case 1
                    .SortMode = DataGridViewColumnSortMode.Automatic
                    .HeaderCell.SortGlyphDirection = SortOrder.None
                Case Else
                    .SortMode = DataGridViewColumnSortMode.NotSortable
            End Select
        End With
    End Sub

    Private Sub DgvSGs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvSGs.ColumnHeaderMouseClick
        If e.ColumnIndex <> 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim col As DataGridViewColumn = dgv.Columns(e.ColumnIndex)
        Dim dir As ListSortDirection

        Select Case col.HeaderCell.SortGlyphDirection
            Case SortOrder.None, SortOrder.Ascending
                dir = ListSortDirection.Descending
            Case SortOrder.Descending
                dir = ListSortDirection.Ascending
        End Select

        dgv.Sort(col, dir)
    End Sub

    Private Sub DgvSGs_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DgvSGs.DataBindingComplete
        Dim dgv As DataGridView = CType(sender, DataGridView)
        For Each column As DataGridViewColumn In dgv.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Next
        dgv.Columns(dgv.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Dim order As SortOrder = SortOrder.None
        If dgv.RowCount > 0 Then
            Dim value As String = dgv.Rows(0).Cells(0).Value.ToString
            If value = "288" Then
                order = SortOrder.Descending
            ElseIf value = "1" Then
                order = SortOrder.Ascending
            End If
        End If
        dgv.Columns(0).HeaderCell.SortGlyphDirection = order
    End Sub

#End Region ' Dgv SGs Events

#Region "Dgv Auto Basal Delivery (Basal) Events"

    Private Sub DgvAutoBasalDelivery_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvAutoBasalDelivery.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.Value Is Nothing Then
            Return
        End If
        ' Set the background to red for negative values in the Balance column.
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(AutoBasalDeliveryRecord.bolusAmount), StringComparison.OrdinalIgnoreCase) Then
            Dim basalAmount As Single = ParseSingle(e.Value, 3)
            e.Value = basalAmount.ToString("F3", CurrentUICulture)
            If basalAmount.IsMinBasal Then
                FormatCell(e, GetGraphLineColor("Min Basal"))
            End If
            e.FormattingApplied = True
            Return
        End If
        dgv.dgvCellFormatting(e, NameOf(AutoBasalDeliveryRecord.dateTime))
    End Sub

    Private Sub DgvAutoBasalDelivery_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvAutoBasalDelivery.ColumnAdded
        With e.Column
            If AutoBasalDeliveryRecordHelpers.HideColumn(.Name) Then
                .Visible = False
                Exit Sub
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(AutoBasalDeliveryRecordHelpers.GetCellStyle(.Name),
                             False,
                             True,
                             CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Auto Basal Delivery (Basal) Events

#Region "Dgv Insulin Events"

    Private Sub DgvInsulin_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvInsulin.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Columns(e.ColumnIndex).ValueType = GetType(Single) Then
            Dim value As Single = ParseSingle(e.Value, 3)
            e.Value = value.ToString("F3", CurrentDataCulture)
            If value <> 0 AndAlso dgv.Columns(e.ColumnIndex).Name = NameOf(InsulinRecord.SafeMealReduction) Then
                e.CellStyle.ForeColor = Color.OrangeRed
            End If
            e.FormattingApplied = True
            Exit Sub
        End If
        dgv.dgvCellFormatting(e, NameOf(InsulinRecord.dateTime))
    End Sub

    Private Sub DgvInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvInsulin.ColumnAdded
        With e.Column
            If InsulinRecordHelpers.HideColumn(.Name) Then
                .Visible = False
                Exit Sub
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(InsulinRecordHelpers.GetCellStyle(.Name),
                             True,
                             True,
                             CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    Private Sub DgvInsulin_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvInsulin.DataError
        Stop
    End Sub

#End Region ' Dgv Insulin Events

#Region "Dgv Meal Events"

    Private Sub DgvMeal_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvMeal.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Columns(e.ColumnIndex).Name = NameOf(MealRecord.amount) Then
            e.Value = $"{e.Value} {s_sessionCountrySettings.carbDefaultUnit}"
            e.FormattingApplied = True
        Else
            dgv.dgvCellFormatting(e, NameOf(MealRecord.dateTime))
        End If
    End Sub

    Private Sub DgvMeal_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvMeal.ColumnAdded
        With e.Column
            If MealRecordHelpers.HideColumn(.Name) Then
                .Visible = False
                Exit Sub
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(MealRecordHelpers.GetCellStyle(.Name),
                            True,
                            True,
                            CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    Private Sub DgvMeal_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvMeal.DataError
        Stop
    End Sub

#End Region ' Dgv Meal Events

#Region "Dgv Session Profile Events"

    Private Sub DgvSessionProfile_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvSessionProfile.ColumnAdded
        e.DgvColumnAdded(New DataGridViewCellStyle().SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1)),
                         False,
                         True,
                         Nothing)

    End Sub

    Private Sub DgvSessionProfile_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvSessionProfile.DataError
        Stop
    End Sub

#End Region ' Dgv Session Profile Events

#Region "Dgv Current User Events"

    Private Sub DgvCurrentUser_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvCurrentUser.ColumnAdded
        e.DgvColumnAdded(New DataGridViewCellStyle().SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1)),
                         False,
                         True,
                         Nothing)

    End Sub

    Private Sub DgvCurrentUser_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvCurrentUser.DataError
        Stop
    End Sub

#End Region ' Dgv Current User Events

#Region "Dgv CareLink Users Events"

    Private Sub DgvCareLinkUsers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DgvCareLinkUsers.CellBeginEdit
        Dim dgv As DataGridView = CType(sender, DataGridView)
        'Here we save a current value of cell to some variable, that later we can compare with a new value
        'For example using of dgv.Tag property
        If e.RowIndex >= 0 AndAlso e.ColumnIndex > 0 Then
            dgv.Tag = dgv.CurrentCell.Value.ToString
        End If

    End Sub

    Private Sub DgvCareLinkUsers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvCareLinkUsers.CellContentClick
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim dataGridViewDisableButtonCell As DataGridViewDisableButtonCell = TryCast(dgv.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewDisableButtonCell)
        If dataGridViewDisableButtonCell IsNot Nothing Then

            If Not dataGridViewDisableButtonCell.Enabled Then
                Exit Sub
            End If

            dgv.DataSource = Nothing
            s_allUserSettingsData.RemoveAt(e.RowIndex)
            dgv.DataSource = s_allUserSettingsData
            s_allUserSettingsData.SaveAllUserRecords()
        End If

    End Sub

    Private Sub DgvCareLinkUsers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgvCareLinkUsers.CellEndEdit
        'after you've filled your dataSet, on event above try something like this
        Try
            '
        Catch ex As Exception
            MessageBox.Show(ex.DecodeException())
        End Try

    End Sub

    Private Sub DgvCareLinkUsers_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DgvCareLinkUsers.CellValidating
        If e.ColumnIndex = 0 Then
            Exit Sub
        End If

    End Sub

    Private Sub DgvCareLinkUsers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvCareLinkUsers.ColumnAdded
        With e.Column
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim caption As String = CType(dgv.DataSource, DataTable)?.Columns(.Index).Caption
            Dim dataPropertyName As String = e.Column.DataPropertyName
            If String.IsNullOrWhiteSpace(caption) Then
                caption = dataPropertyName.Replace("DgvCareLinkUsers", "")
            End If
            If caption.Contains("DeleteRow", StringComparison.OrdinalIgnoreCase) Then
                caption = ""
            Else
                If .Index > 0 AndAlso String.IsNullOrWhiteSpace(dataPropertyName) AndAlso String.IsNullOrWhiteSpace(caption) Then
                    dataPropertyName = s_headerColumns(.Index - 2)
                End If
            End If
            If CareLinkUserDataRecordHelpers.HideColumn(dataPropertyName) Then
                e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(dataPropertyName),
                                 False,
                                 False,
                                 caption)
                .Visible = False
            Else
                e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(dataPropertyName),
                                 False,
                                 True,
                                 caption)
                e.Column.HeaderText = caption.ToTitleCase
            End If
        End With
    End Sub

    Private Sub DgvCareLinkUsers_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvCareLinkUsers.DataError
        Stop
    End Sub

    Private Sub DgvCareLinkUsers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DgvCareLinkUsers.RowsAdded
        If s_allUserSettingsData.Count = 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        For i As Integer = e.RowIndex To e.RowIndex + (e.RowCount - 1)
            Dim disableButtonCell As DataGridViewDisableButtonCell = CType(dgv.Rows(i).Cells(NameOf(DgvCareLinkUsersDeleteRow)), DataGridViewDisableButtonCell)
            disableButtonCell.Enabled = s_allUserSettingsData(i).CareLinkUserName <> _LoginDialog.LoggedOnUser.CareLinkUserName
        Next
    End Sub

#End Region ' Dgv CareLink Users Events

#End Region ' DataGridView Events

#Region "TableLayoutPanelTop Button Events"

    Private Sub TableLayoutPanelTopButton_Click(sender As Object, e As EventArgs) _
            Handles TableLayoutPanelActiveInsulinTop.ButtonClick,
                    TableLayoutPanelAutoBasalDeliveryTop.ButtonClick,
                    TableLayoutPanelAutoModeStatusTop.ButtonClick,
                    TableLayoutPanelBannerStateTop.ButtonClick,
                    TableLayoutPanelBasalTop.ButtonClick,
                    TableLayoutPanelBgReadingsTop.ButtonClick,
                    TableLayoutPanelCalibrationTop.ButtonClick,
                    TableLayoutPanelInsulinTop.ButtonClick,
                    TableLayoutPanelLastAlarmTop.ButtonClick,
                    TableLayoutPanelLastSgTop.ButtonClick,
                    TableLayoutPanelLimitsTop.ButtonClick,
                    TableLayoutPanelLowGlucoseSuspendedTop.ButtonClick,
                    TableLayoutPanelMealTop.ButtonClick,
                    TableLayoutPanelNotificationHistoryTop.ButtonClick,
                    TableLayoutPanelSgsTop.ButtonClick,
                    TableLayoutPanelTherapyAlgorithmTop.ButtonClick,
                    TableLayoutPanelTimeChangeTop.ButtonClick
        Me.TabControlPage1.SelectedIndex = 3
        Me.TabControlPage1.Visible = True
        Dim topTable As TableLayoutPanelTopEx = CType(CType(sender, Button).Parent, TableLayoutPanelTopEx)
        Dim dgv As DataGridView = CType(Me.TabControlPage1.TabPages(3).Controls(0), DataGridView)
        Dim tabName As String = topTable.LabelText.Substring(3)
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(1).FormattedValue.ToString = tabName Then
                dgv.CurrentCell = row.Cells(2)
                Exit For
            End If
        Next

    End Sub

#End Region ' TableLayoutPanelTop Button Events

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
        If Not Me.SummaryChart.ChartAreas(NameOf(ChartArea)).AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.SummaryChart.ChartAreas(NameOf(ChartArea)).CursorX.Position = Double.NaN
        End If
    End Sub

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        Me.ServerUpdateTimer.Stop()
        SyncLock _updatingLock
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

            _sgMiniDisplay.SetCurrentBGString("---")
        Else
            If Me.RecentData?.TryGetValue(NameOf(ItemIndexes.lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerEpochString) Then
                If CLng(lastMedicalDeviceDataUpdateServerEpochString) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                    If lastMedicalDeviceDataUpdateServerEpochString.Epoch2DateTime + s_5MinuteSpan < Now() Then
                        Me.LastUpdateTime.UpdateHighLightInLastUpdateTime(True)
                        _sgMiniDisplay.SetCurrentBGString("---")
                    Else
                        Me.LastUpdateTime.UpdateHighLightInLastUpdateTime(False)
                        _sgMiniDisplay.SetCurrentBGString(s_lastSgRecord?.sg.ToString)
                    End If
                    Me.RecentData = Nothing
                Else
                    Me.UpdateAllTabPages(False)
                End If
            Else
                Stop
            End If
        End If
        LastServerUpdateTime = lastMedicalDeviceDataUpdateServerEpochString.Epoch2DateTime
        Me.ServerUpdateTimer.Interval = CInt(s_1MinutesInMilliseconds)
        Me.ServerUpdateTimer.Start()
    End Sub

    Public Sub PowerModeChanged(sender As Object, e As Microsoft.Win32.PowerModeChangedEventArgs)
        Select Case e.Mode
            Case Microsoft.Win32.PowerModes.Suspend
                Me.ServerUpdateTimer.Stop()
                Me.SetLastUpdateTime("Sleeping", True)
            Case Microsoft.Win32.PowerModes.Resume
                Me.SetLastUpdateTime("Awake", True)
                Me.ServerUpdateTimer.Interval = CInt(s_30SecondInMilliseconds) \ 3
                Me.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(PowerModeChanged)}, restarted after wake. {NameOf(ServerUpdateTimer)} started at {Now.ToLongTimeString}")
        End Select

    End Sub

#End Region ' Timer Events

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Summary Charts"

    Friend Sub InitializeSummaryTabCharts()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.SummaryChart = CreateChart(NameOf(SummaryChart))
        Dim summaryTitle As Title = CreateTitle("Summary",
                                                NameOf(summaryTitle),
                                                Me.SummaryChart.BackColor.GetContrastingColor)

        Dim summaryChartArea As ChartArea = CreateChartArea(Me.SummaryChart)
        Me.SummaryChart.ChartAreas.Add(summaryChartArea)
        _summaryChartLegend = CreateChartLegend(NameOf(_summaryChartLegend))

        SummaryAutoCorrectionSeries = CreateSeriesBasal(AutoCorrectionSeriesName, _summaryChartLegend, "Auto Correction", AxisType.Secondary)
        SummaryBasalSeries = CreateSeriesBasal(BasalSeriesNameName, _summaryChartLegend, "Basal Series", AxisType.Secondary)
        SummaryMinBasalSeries = CreateSeriesBasal(MinBasalSeriesName, _summaryChartLegend, "Min Basal", AxisType.Secondary)

        SummaryHighLimitSeries = CreateSeriesLimitsAndTarget(_summaryChartLegend, HighLimitSeriesName)
        SummaryTargetSgSeries = CreateSeriesLimitsAndTarget(_summaryChartLegend, TargetSgSeriesName)
        SummarySgSeries = CreateSeriesSg(_summaryChartLegend)
        SummaryLowLimitSeries = CreateSeriesLimitsAndTarget(_summaryChartLegend, LowLimitSeriesName)

        SummaryMarkerSeries = CreateSeriesWithoutVisibleLegend(AxisType.Secondary)

        SummaryTimeChangeSeries = CreateSeriesTimeChange(_summaryChartLegend)

        Me.SplitContainer3.Panel1.Controls.Add(Me.SummaryChart)
        Application.DoEvents()

        With Me.SummaryChart
            With .Series
                .Add(SummaryHighLimitSeries)
                .Add(SummaryTargetSgSeries)
                .Add(SummaryLowLimitSeries)
                .Add(SummaryTimeChangeSeries)

                .Add(SummaryAutoCorrectionSeries)
                .Add(SummaryBasalSeries)
                .Add(SummaryMinBasalSeries)

                .Add(SummarySgSeries)
                .Add(SummaryMarkerSeries)

            End With
            With .Series(SgSeriesName).EmptyPointStyle
                .BorderWidth = 4
                .Color = Color.Transparent
            End With
            .Legends.Add(_summaryChartLegend)
            .Titles.Add(summaryTitle)
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
        Me.TimeInRangeChart = New Chart With {
            .Anchor = AnchorStyles.Top,
            .BackColor = Color.Transparent,
            .BackGradientStyle = GradientStyle.None,
            .BackSecondaryColor = Color.Transparent,
            .BorderlineColor = Color.Transparent,
            .BorderlineWidth = 0,
            .Size = New Size(width1,
                             width1)
                            }

        With Me.TimeInRangeChart
            .BorderSkin.BackSecondaryColor = Color.Transparent
            .BorderSkin.SkinStyle = BorderSkinStyle.None
            Dim timeInRangeChartArea As New ChartArea With {
                    .Name = NameOf(timeInRangeChartArea),
                    .BackColor = Color.Black
                }
            .ChartAreas.Add(timeInRangeChartArea)
            .Location = New Point(Me.TimeInRangeChartLabel.FindHorizontalMidpoint - (.Width \ 2),
                                  CInt(Me.TimeInRangeChartLabel.FindVerticalMidpoint() - Math.Round(.Height / 2.5)))
            .Name = NameOf(TimeInRangeChart)
            TimeInRangeSeries = New Series(NameOf(TimeInRangeSeries)) With {
                    .ChartArea = NameOf(timeInRangeChartArea),
                    .ChartType = SeriesChartType.Doughnut
                }
            .Series.Add(TimeInRangeSeries)
            .Series(NameOf(TimeInRangeSeries))("DoughnutRadius") = "17"
        End With

        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeChart)
        Application.DoEvents()
    End Sub

#End Region ' Initialize Home Tab Charts

#Region "Initialize Chart Tabs"

#Region "Initialize Active Insulin Chart"

    Friend Sub InitializeActiveInsulinTabChart()
        Me.SplitContainer1.Panel2.Controls.Clear()

        Me.ActiveInsulinChart = CreateChart(NameOf(ActiveInsulinChart))
        Dim activeInsulinChartArea As ChartArea = CreateChartArea(Me.ActiveInsulinChart)
        Dim labelColor As Color = Me.ActiveInsulinChart.BackColor.GetContrastingColor
        Dim labelFont As New Font("Trebuchet MS", 12.0F, FontStyle.Bold)

        With activeInsulinChartArea.AxisY
            .Interval = 2
            .IsInterlaced = False
            With .LabelStyle
                .Font = labelFont
                .ForeColor = labelColor
                .Format = "{0}"
            End With
            With .MajorTickMark
                .Interval = 4
                .Enabled = False
            End With
            .Maximum = 25
            .Minimum = 0
            .Title = "Active Insulin"
            .TitleFont = New Font(labelFont.FontFamily, 14)
            .TitleForeColor = labelColor
        End With
        Me.ActiveInsulinChart.ChartAreas.Add(activeInsulinChartArea)
        _activeInsulinChartLegend = CreateChartLegend(NameOf(_activeInsulinChartLegend))
        Me.ActiveInsulinChartTitle = CreateTitle($"Running Insulin On Board (IOB)",
                                                 NameOf(ActiveInsulinChartTitle),
                                                 GetGraphLineColor("Active Insulin"))
        ActiveInsulinActiveInsulinSeries = CreateSeriesActiveInsulin()
        ActiveInsulinTargetSeries = CreateSeriesLimitsAndTarget(_activeInsulinChartLegend, TargetSgSeriesName)

        ActiveInsulinAutoCorrectionSeries = CreateSeriesBasal(AutoCorrectionSeriesName, _activeInsulinChartLegend, "Auto Correction", AxisType.Secondary)
        ActiveInsulinBasalSeries = CreateSeriesBasal(BasalSeriesNameName, _activeInsulinChartLegend, "Basal Series", AxisType.Secondary)
        ActiveInsulinMinBasalSeries = CreateSeriesBasal(MinBasalSeriesName, _activeInsulinChartLegend, "Min Basal", AxisType.Secondary)

        ActiveInsulinSgSeries = CreateSeriesSg(_activeInsulinChartLegend)
        ActiveInsulinMarkerSeries = CreateSeriesWithoutVisibleLegend(AxisType.Secondary)
        ActiveInsulinTimeChangeSeries = CreateSeriesTimeChange(_activeInsulinChartLegend)

        With Me.ActiveInsulinChart
            With .Series
                .Add(ActiveInsulinTargetSeries)
                .Add(ActiveInsulinTimeChangeSeries)

                .Add(ActiveInsulinActiveInsulinSeries)

                .Add(ActiveInsulinAutoCorrectionSeries)
                .Add(ActiveInsulinBasalSeries)
                .Add(ActiveInsulinMinBasalSeries)

                .Add(ActiveInsulinSgSeries)
                .Add(ActiveInsulinMarkerSeries)
            End With
            .Series(SgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(SgSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(ActiveInsulinSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(ActiveInsulinSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Legends.Add(_activeInsulinChartLegend)
        End With

        Me.ActiveInsulinChart.Titles.Add(Me.ActiveInsulinChartTitle)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ActiveInsulinChart)
        Application.DoEvents()

    End Sub

#End Region ' Initialize Active Insulin Chart

#Region "Initialize Treatment Markers Chart"

    Private Sub InitializeTreatmentMarkersChart()
        Me.TabPage03TreatmentDetails.Controls.Clear()

        Me.TreatmentMarkersChart = CreateChart(NameOf(TreatmentMarkersChart))
        Dim treatmentMarkersChartArea As ChartArea = CreateChartArea(Me.TreatmentMarkersChart)

        TreatmentInsulinRow = SetTreatmentInsulinRow()

        Dim labelColor As Color = Me.TreatmentMarkersChart.BackColor.GetContrastingColor
        Dim labelFont As New Font("Trebuchet MS", 12.0F, FontStyle.Bold)

        With treatmentMarkersChartArea.AxisY
            Dim interval As Single = (TreatmentInsulinRow / 10).RoundSingle(3)
            .Interval = interval
            .IsInterlaced = False
            .IsMarginVisible = False
            With .LabelStyle
                .Font = labelFont
                .ForeColor = labelColor
                .Format = "{0.00}"
            End With
            .LineColor = Color.FromArgb(64, labelColor)
            With .MajorTickMark
                .Enabled = True
                .Interval = interval
                .LineColor = Color.FromArgb(64, labelColor)
            End With
            .Maximum = TreatmentInsulinRow
            .Minimum = 0
            .Title = "Delivered Insulin"
            .TitleFont = New Font(labelFont.FontFamily, 14)
            .TitleForeColor = labelColor
        End With

        Me.TreatmentMarkersChart.ChartAreas.Add(treatmentMarkersChartArea)
        _treatmentMarkersChartLegend = CreateChartLegend(NameOf(_treatmentMarkersChartLegend))

        Me.TreatmentMarkersChartTitle = CreateTitle("Treatment Details", NameOf(TreatmentMarkersChartTitle), Me.TreatmentMarkersChart.BackColor.GetContrastingColor)
        TreatmentTargetSeries = CreateSeriesLimitsAndTarget(_treatmentMarkersChartLegend, TargetSgSeriesName)
        TreatmentMarkerAutoCorrectionSeries = CreateSeriesBasal(AutoCorrectionSeriesName, _treatmentMarkersChartLegend, "Auto Correction", AxisType.Primary)
        TreatmentMarkerBasalSeries = CreateSeriesBasal(BasalSeriesNameName, _treatmentMarkersChartLegend, "Basal Series", AxisType.Primary)
        TreatmentMarkerMinBasalSeries = CreateSeriesBasal(MinBasalSeriesName, _treatmentMarkersChartLegend, "Min Basal", AxisType.Primary)

        TreatmentMarkerSgSeries = CreateSeriesSg(_treatmentMarkersChartLegend)
        TreatmentMarkerMarkersSeries = CreateSeriesWithoutVisibleLegend(AxisType.Primary)
        TreatmentMarkerTimeChangeSeries = CreateSeriesTimeChange(_treatmentMarkersChartLegend)

        With Me.TreatmentMarkersChart
            With .Series
                .Add(TreatmentTargetSeries)
                .Add(TreatmentMarkerTimeChangeSeries)

                .Add(TreatmentMarkerAutoCorrectionSeries)
                .Add(TreatmentMarkerBasalSeries)
                .Add(TreatmentMarkerMinBasalSeries)

                .Add(TreatmentMarkerSgSeries)
                .Add(TreatmentMarkerMarkersSeries)
            End With
            .Legends.Add(_treatmentMarkersChartLegend)
            .Series(SgSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(SgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(BasalSeriesNameName).EmptyPointStyle.Color = Color.Transparent
            .Series(BasalSeriesNameName).EmptyPointStyle.BorderWidth = 4
            .Series(MarkerSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(MarkerSeriesName).EmptyPointStyle.BorderWidth = 4
        End With

        Me.TreatmentMarkersChart.Titles.Add(Me.TreatmentMarkersChartTitle)
        Me.TabPage03TreatmentDetails.Controls.Add(Me.TreatmentMarkersChart)
        Application.DoEvents()

    End Sub

#End Region ' Initialize Treatment Markers Chart

#End Region ' Initialize Chart Tabs

#End Region ' Initialize Charts

#Region "Update Home Tab"

    Private Sub UpdateActiveInsulin()
        Try
            Dim activeInsulinStr As String = $"{s_activeInsulin.amount:N3}"
            Me.ActiveInsulinValue.Text = $"Active Insulin{s_environmentNewLine}{activeInsulinStr} U"
            _sgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr}U"
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}")
        End Try
    End Sub

    Private Sub UpdateActiveInsulinChart()
        If Not Me.Initialized Then
            Exit Sub
        End If

        Try
            Me.TemporaryUseAdvanceAITDecayCheckBox.Checked = CurrentUser.UseAdvancedAitDecay = CheckState.Checked
            For Each s As Series In Me.ActiveInsulinChart.Series
                s.Points.Clear()
            Next
            With Me.ActiveInsulinChart
                .Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Insulin On Board (IOB) - {s_listOfManualBasal.GetSubTitle}"
                .ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()

                ' Order all markers by time
                Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)
                Dim markerOADateTime As OADate

                Dim lastTimeChangeRecord As TimeChangeRecord = Nothing
                For Each marker As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
                    markerOADateTime = New OADate(s_markers(marker.Index).GetMarkerDateTime)
                    Select Case marker.Value(NameOf(InsulinRecord.type)).ToString
                        Case "AUTO_BASAL_DELIVERY"
                            Dim bolusAmount As Single = marker.Value.GetSingleValue(NameOf(AutoBasalDeliveryRecord.bolusAmount))
                            If timeOrderedMarkers.ContainsKey(markerOADateTime) Then
                                timeOrderedMarkers(markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(markerOADateTime, bolusAmount)
                            End If
                        Case "MANUAL_BASAL_DELIVERY"
                            Dim bolusAmount As Single = marker.Value.GetSingleValue(NameOf(AutoBasalDeliveryRecord.bolusAmount))
                            If timeOrderedMarkers.ContainsKey(markerOADateTime) Then
                                timeOrderedMarkers(markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(markerOADateTime, bolusAmount)
                            End If
                        Case "INSULIN"
                            Dim bolusAmount As Single = marker.Value.GetSingleValue(NameOf(InsulinRecord.deliveredFastAmount))
                            If timeOrderedMarkers.ContainsKey(markerOADateTime) Then
                                timeOrderedMarkers(markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(markerOADateTime, bolusAmount)
                            End If
                        Case "BG_READING"
                        Case "CALIBRATION"
                        Case "MEAL"
                        Case "TIME_CHANGE"
                        Case Else
                            Stop
                    End Select
                Next

                ' set up table that holds active insulin for every 5 minutes
                Dim remainingInsulinList As New List(Of RunningActiveInsulinRecord)
                Dim currentMarker As Integer = 0

                For i As Integer = 0 To 287
                    Dim initialBolus As Single = 0
                    Dim firstNotSkippedOaTime As New OADate((s_listOfSGs(0).datetime + (s_5MinuteSpan * i)).RoundTimeDown(RoundTo.Minute))
                    While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= firstNotSkippedOaTime
                        initialBolus += timeOrderedMarkers.Values(currentMarker)
                        currentMarker += 1
                    End While
                    remainingInsulinList.Add(New RunningActiveInsulinRecord(firstNotSkippedOaTime, initialBolus, CurrentUser))
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY2.Maximum = GetYMaxValue()
                ' walk all markers, adjust active insulin and then add new marker
                Dim maxActiveInsulin As Double = 0
                For i As Integer = 0 To remainingInsulinList.Count - 1
                    If i < CurrentUser.GetActiveInsulinIncrements Then
                        With ActiveInsulinActiveInsulinSeries
                            .Points.AddXY(remainingInsulinList(i).OaDateTime, Double.NaN)
                            .Points.Last.IsEmpty = True
                        End With
                        If i > 0 Then
                            remainingInsulinList.AdjustList(0, i)
                        End If
                        Continue For
                    End If
                    Dim startIndex As Integer = i - CurrentUser.GetActiveInsulinIncrements + 1
                    Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, CurrentUser.GetActiveInsulinIncrements)
                    maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                    ActiveInsulinActiveInsulinSeries.Points.AddXY(remainingInsulinList(i).OaDateTime, sum)
                    remainingInsulinList.AdjustList(startIndex, CurrentUser.GetActiveInsulinIncrements)
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
                .PlotMarkers(ActiveInsulinTimeChangeSeries,
                             _summaryChartAbsoluteRectangle,
                             s_activeInsulinMarkerInsulinDictionary,
                             Nothing)
                .PlotSgSeries(GetYMinValue())
                .PlotHighLowLimitsAndTargetSg(True)
            End With
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulinChart)}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateAllSummarySeries()
        Try
            With Me.SummaryChart

                For Each s As Series In .Series
                    s.Points.Clear()
                Next
                .ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()
                .Titles(0).Text = $"Status - {s_listOfManualBasal.GetSubTitle}"
                .PlotMarkers(SummaryTimeChangeSeries,
                             _summaryChartAbsoluteRectangle,
                             s_summaryMarkerInsulinDictionary,
                             s_summaryMarkerMealDictionary)
                .PlotSgSeries(GetYMinValue())
                .PlotHighLowLimitsAndTargetSg(False)
                Application.DoEvents()
            End With
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception while plotting Markers in {NameOf(UpdateAllSummarySeries)}")
        End Try

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
                _sgMiniDisplay.SetCurrentBGString(s_lastSgRecord.sg.ToString)
                Me.SensorMessage.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield
                Me.ShieldUnitsLabel.Visible = True
            Else
                _sgMiniDisplay.SetCurrentBGString("---")
                Me.CurrentBGLabel.Visible = False
                Me.CalibrationShieldPictureBox.Image = My.Resources.Shield_Disabled
                Me.SensorMessage.Visible = True
                Me.SensorMessage.BackColor = Color.Transparent
                Dim message As String = ""
                If s_sensorMessages.TryGetValue(s_sensorState, message) Then
                    Dim splitMessage As String = message.Split(".")(0)
                    message = If(message.Contains("..."), $"{splitMessage}...", splitMessage)
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
            If _sgMiniDisplay.Visible Then
                _sgMiniDisplay.BGTextBox.SelectionLength = 0
            End If
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateAutoModeShield)}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        Try
            If s_timeToNextCalibrationHours > Byte.MaxValue Then
                Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(720)
            ElseIf s_timeToNextCalibrationHours = 0 Then
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
                    Dim deliveredAmount As String = marker.Value(NameOf(InsulinRecord.deliveredFastAmount))
                    s_totalDailyDose += deliveredAmount.ParseSingle(3)
                    Select Case marker.Value(NameOf(InsulinRecord.activationType))
                        Case "AUTOCORRECTION"
                            s_totalAutoCorrection += deliveredAmount.ParseSingle(3)
                        Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                            s_totalManualBolus += deliveredAmount.ParseSingle(3)
                    End Select

                Case "AUTO_BASAL_DELIVERY"
                    Dim amount As Single = marker.Value(NameOf(AutoBasalDeliveryRecord.bolusAmount)).ParseSingle(3)
                    s_totalBasal += amount
                    s_totalDailyDose += amount
                Case "MANUAL_BASAL_DELIVERY"
                    Dim amount As Single = marker.Value(NameOf(AutoBasalDeliveryRecord.bolusAmount)).ParseSingle(3)
                    s_totalBasal += amount
                    s_totalDailyDose += amount
                Case "MEAL"
                    s_totalCarbs += CInt(marker.Value("amount"))
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

    Private Sub UpdateInsulinLevel()

        Me.InsulinLevelPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        If Not s_pumpInRangeOfPhone Then
            Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(8)
            Me.RemainingInsulinUnits.Text = "???U"
        Else
            Me.RemainingInsulinUnits.Text = $"{s_listOfSummaryRecords.GetValue(Of String)(NameOf(ItemIndexes.reservoirRemainingUnits)).ParseSingle(1):N1} U"
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
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdatePumpBattery()
        If Not s_pumpInRangeOfPhone Then
            Me.PumpBatteryPictureBox.Image = My.Resources.PumpConnectivityToPhoneNotOK
            Me.PumpBatteryRemainingLabel.Text = "Pump out"
            Me.PumpBatteryRemaining2Label.Text = "of range"
            Exit Sub
        End If

        Dim batteryLeftPercent As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(NameOf(ItemIndexes.medicalDeviceBatteryLevelPercent))
        Select Case batteryLeftPercent
            Case > 90
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryFull
                Me.PumpBatteryRemainingLabel.Text = "Full"
                Me.PumpBatteryRemaining2Label.Text = $"{batteryLeftPercent}%"
            Case > 50
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryHigh
                Me.PumpBatteryRemainingLabel.Text = "High"
                Me.PumpBatteryRemaining2Label.Text = $"{batteryLeftPercent}%"
            Case > 25
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryMedium
                Me.PumpBatteryRemainingLabel.Text = $"Medium"
                Me.PumpBatteryRemaining2Label.Text = $"{batteryLeftPercent}%"
            Case > 10
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryLow
                Me.PumpBatteryRemainingLabel.Text = "Low"
                Me.PumpBatteryRemaining2Label.Text = $"{batteryLeftPercent}%"

            Case Else
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryCritical
                Me.PumpBatteryRemainingLabel.Text = "Critical"
                Me.PumpBatteryRemaining2Label.Text = $"{batteryLeftPercent}%"
        End Select
    End Sub

    Private Sub UpdateSensorLife()

        Select Case s_sensorDurationHours
            Case Is >= 255
                Me.SensorDaysLeftLabel.Text = ""
                Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                Me.SensorTimeLeftLabel.Text = "Unknown"
            Case Is >= 24
                Me.SensorDaysLeftLabel.Text = Math.Ceiling(s_sensorDurationHours / 24).ToString()
                Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeOK
                Me.SensorTimeLeftLabel.Text = $"{Me.SensorDaysLeftLabel.Text} Days"
            Case > 0
                Me.SensorDaysLeftLabel.Text = $"<{Math.Ceiling(s_sensorDurationHours / 24)}"
                Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                Me.SensorTimeLeftLabel.Text = $"{s_sensorDurationHours} Hours"
            Case 0
                Dim sensorDurationMinutes As Integer = s_listOfSummaryRecords.GetValue(NameOf(ItemIndexes.sensorDurationMinutes), False, -1)
                Select Case sensorDurationMinutes
                    Case > 0
                        Me.SensorDaysLeftLabel.Text = "0"
                        Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                        Me.SensorTimeLeftLabel.Text = $"{sensorDurationMinutes} minutes"
                    Case 0
                        Me.SensorDaysLeftLabel.Text = ""
                        Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpired
                        Me.SensorTimeLeftLabel.Text = "Expired"
                    Case Else
                        Me.SensorDaysLeftLabel.Text = ""
                        Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                        Me.SensorTimeLeftLabel.Text = "Unknown"
                End Select

            Case Else
                Me.SensorDaysLeftLabel.Text = ""
                Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                Me.SensorTimeLeftLabel.Text = "Unknown"
        End Select
        Me.SensorDaysLeftLabel.Visible = True
    End Sub

    Private Sub UpdateTimeInRange()
        If Me.TimeInRangeChart Is Nothing Then
            Stop
            Exit Sub
        End If
        With Me.TimeInRangeChart
            With .Series(NameOf(TimeInRangeSeries)).Points
                .Clear()
                .AddXY($"{s_belowHypoLimit}% Below {TirLowLimit()} {BgUnitsString}", s_belowHypoLimit / 100)
                .Last().Color = Color.Red
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_aboveHyperLimit}% Above {TirHighLimit()} {BgUnitsString}", s_aboveHyperLimit / 100)
                .Last().Color = Color.Yellow
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_timeInRange}% In Range", s_timeInRange / 100)
                .Last().Color = Color.LimeGreen
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
            End With
            .Series(NameOf(TimeInRangeSeries))("PieLabelStyle") = "Disabled"
            .Series(NameOf(TimeInRangeSeries))("PieStartAngle") = "270"
        End With

        Dim averageSgStr As String = Me.RecentData.GetStringValueOrEmpty(NameOf(ItemIndexes.averageSG))
        Me.AboveHighLimitValueLabel.Text = $"{s_aboveHyperLimit} %"
        Me.AverageSGMessageLabel.Text = $"Average SG in {BgUnitsString}"
        Me.AverageSGValueLabel.Text = If(BgUnitsString = "mg/dl", averageSgStr, averageSgStr.TruncateSingleString(2))
        Me.BelowLowLimitValueLabel.Text = $"{s_belowHypoLimit} %"
        Me.SerialNumberLabel.Text = Me.RecentData.GetStringValueOrEmpty(NameOf(ItemIndexes.medicalDeviceSerialNumber))
        Me.TimeInRangeChartLabel.Text = s_timeInRange.ToString
        Me.TimeInRangeValueLabel.Text = $"{s_timeInRange} %"

    End Sub

    Private Sub UpdateTreatmentChart()
        If Not _Initialized Then
            Exit Sub
        End If
        Try
            Me.InitializeTreatmentMarkersChart()
            Me.TreatmentMarkersChart.Titles(NameOf(TreatmentMarkersChartTitle)).Text = $"Treatment Details - {s_listOfManualBasal.GetSubTitle}"
            Me.TreatmentMarkersChart.ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()
            Me.TreatmentMarkersChart.PlotTreatmentMarkers(TreatmentMarkerTimeChangeSeries)
            Me.TreatmentMarkersChart.PlotSgSeries(GetYMinValue())
            Me.TreatmentMarkersChart.PlotHighLowLimitsAndTargetSg(True)
        Catch ex As Exception
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(InitializeTreatmentMarkersChart)}")
        End Try
        Application.DoEvents()
    End Sub

    Friend Sub UpdateAllTabPages(fromFile As Boolean)
        If Me.RecentData Is Nothing Then
            Debug.Print($"Exiting {NameOf(UpdateAllTabPages)}, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Dim lastMedicalDeviceDataUpdateServerTimeEpoch As String = ""
        If Me.RecentData?.TryGetValue(NameOf(ItemIndexes.lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerTimeEpoch) Then
            If CLng(lastMedicalDeviceDataUpdateServerTimeEpoch) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                Me.RecentData = Nothing
                Exit Sub
            End If
        End If

        If Me.RecentData.Count > ItemIndexes.finalCalibration + 1 Then
            Stop
        End If
        SyncLock _updatingLock
            _updating = True ' prevent paint
            _summaryChartAbsoluteRectangle = RectangleF.Empty
            _treatmentMarkerAbsoluteRectangle = RectangleF.Empty
            Me.MenuStartHere.Enabled = False
            If fromFile Then
                Me.LoginStatus.Text = "Login Status: N/A From Saved File"
            Else
                Me.SetLastUpdateTime(PumpNow.ToShortDateTimeString, False)
            End If
            Me.CursorPanel.Visible = False

            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            UpdateDataTables(Me, Me.RecentData)
            Application.DoEvents()
            Me.Cursor = Cursors.Default
            _updating = False
        End SyncLock

        Dim rowValue As String = Me.RecentData.GetStringValueOrEmpty(NameOf(ItemIndexes.lastSGTrend))
        Dim arrows As String = Nothing
        If Trends.TryGetValue(rowValue, arrows) Then
            Me.LabelTrendArrows.Text = Trends(rowValue)
        Else
            Me.LabelTrendArrows.Text = $"{rowValue}"
        End If
        UpdateSummaryTab(Me.DgvSummary)
        Me.UpdateActiveInsulinChart()
        Me.UpdateActiveInsulin()
        Me.UpdateAutoModeShield()
        Me.UpdateCalibrationTimeRemaining()
        Me.UpdateInsulinLevel()
        Me.UpdatePumpBattery()
        Me.UpdateSensorLife()
        Me.UpdateTimeInRange()
        Me.UpdateTransmitterBattery()
        Me.UpdateAllSummarySeries()
        Me.UpdateDosingAndCarbs()

        Me.AboveHighLimitMessageLabel.Text = $"Above {TirHighLimit()} {BgUnitsString}"
        Me.BelowLowLimitMessageLabel.Text = $"Below {TirLowLimit()} {BgUnitsString}"
        Me.FullNameLabel.Text = $"{s_firstName} {Me.RecentData.GetStringValueOrEmpty(NameOf(ItemIndexes.lastName))}"
        Dim modelNumber As String = Me.RecentData.GetStringValueOrEmpty(NameOf(ItemIndexes.pumpModelNumber))
        Me.ModelLabel.Text = modelNumber
        Me.PumpNameLabel.Text = GetPumpName(modelNumber)
        Me.ReadingsLabel.Text = $"{s_listOfSGs.Where(Function(entry As SgRecord) Not Single.IsNaN(entry.sg)).Count}/288 Readings"

        Me.TableLayoutPanelLastSG.DisplayDataTableInDGV(
                              ClassCollectionToDataTable({s_lastSgRecord}.ToList),
                              NameOf(SgRecord),
                              AddressOf SgRecordHelpers.AttachHandlers,
                              ItemIndexes.lastSG,
                              True)

        Me.TableLayoutPanelLastAlarm.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(GetSummaryRecords(s_lastAlarmValue)),
                              NameOf(LastAlarmRecord),
                              AddressOf SummaryRecordHelpers.AttachHandlers,
                              ItemIndexes.lastAlarm,
                              True)

        Me.TableLayoutPanelActiveInsulin.DisplayDataTableInDGV(
                              ClassCollectionToDataTable({s_activeInsulin}.ToList),
                              NameOf(ActiveInsulinRecord),
                              AddressOf ActiveInsulinRecordHelpers.AttachHandlers,
                              ItemIndexes.activeInsulin,
                              True)

        Me.UpdateSgsTab()

        Me.TableLayoutPanelLimits.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfLimitRecords),
                              NameOf(LimitsRecord),
                              AddressOf LimitsRecordHelpers.AttachHandlers,
                              ItemIndexes.limits,
                              False)

        Me.UpdateMarkerTabs()

        Me.UpdateNotificationTab()

        Me.TableLayoutPanelTherapyAlgorithm.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(GetSummaryRecords(s_therapyAlgorithmStateValue)),
                              NameOf(SummaryRecord),
                              AddressOf SummaryRecordHelpers.AttachHandlers,
                              ItemIndexes.therapyAlgorithmState,
                              True)

        Me.UpdatePumpBannerStateTab()

        Me.TableLayoutPanelBasal.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfManualBasal.ToList),
                              NameOf(BasalRecord),
                              AddressOf BasalRecordHelpers.AttachHandlers,
                              ItemIndexes.basal,
                              True)

        s_previousRecentData = Me.RecentData
        Me.MenuStartHere.Enabled = True
        Me.UpdateTreatmentChart()
        If s_totalAutoCorrection > 0 Then
            AddAutoCorrectionLegend(_activeInsulinChartLegend, _summaryChartLegend, _treatmentMarkersChartLegend)
        End If

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
            sp.SplitterDistance -= CInt(Math.Truncate(cs * sc)) - cs
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

    Private Sub MenuOptions_DropDownOpening(sender As Object, e As EventArgs) Handles MenuOptions.DropDownOpening
        Me.MenuOptionsEditPumpSettings.Enabled = Not String.IsNullOrWhiteSpace(CurrentUser?.UserName)
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
                        Case <= TirLowLimit()
                            bgColor = Color.Yellow
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(10000, $"{ProjectName}{TmChar} Alert", $"SG below {TirLowLimit()} {BgUnitsString}", Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                        Case <= TirHighLimit()
                            bgColor = Color.Green
                            _showBalloonTip = True
                        Case Else
                            bgColor = Color.Red
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(10000, $"{ProjectName}{TmChar} Alert", $"SG above {TirHighLimit()} {BgUnitsString}", Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
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
                    notStr.Append(s_environmentNewLine)
                    notStr.Append($"Last SG {str} {BgUnitsString}")
                    If s_lastBGValue = 0 Then
                        Me.LabelTrendValue.Text = ""
                    Else
                        notStr.Append(s_environmentNewLine)
                        Dim diffSg As Double = sg - s_lastBGValue
                        notStr.Append("SG Trend ")
                        If Math.Abs(diffSg) < Single.Epsilon Then
                            If (Now - s_lastBGTime) < s_5MinuteSpan Then
                                diffSg = s_lastBGDiff
                            Else
                                s_lastBGDiff = diffSg
                                s_lastBGTime = Now
                            End If
                        Else
                            s_lastBGTime = Now
                            s_lastBGDiff = diffSg
                        End If
                        Me.LabelTrendValue.Text = diffSg.ToString(If(BgUnits.StartsWith("MG", StringComparison.InvariantCultureIgnoreCase), "+0;-#", "+ 0.00;-#.00"), CultureInfo.InvariantCulture)
                        Me.LabelTrendValue.ForeColor = bgColor
                        notStr.Append(diffSg.ToString(If(BgUnits.StartsWith("MG", StringComparison.InvariantCultureIgnoreCase), "+0;-#", "+ 0.00;-#.00"), CultureInfo.InvariantCulture))
                    End If
                    notStr.Append(s_environmentNewLine)
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
