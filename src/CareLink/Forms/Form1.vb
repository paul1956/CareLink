' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1

    Private ReadOnly _bgMiniDisplay As New BGMiniWindow
    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _insulinImage As Bitmap = My.Resources.InsulinVial_Tiny
    Private ReadOnly _loginDialog As New LoginForm1
    Private _client As CareLinkClient
    Private _homePageAbsoluteRectangle As RectangleF
    Private _homePageChartRelitivePosition As RectangleF = RectangleF.Empty
    Private _initialized As Boolean = False
    Private _inMenuOptions As Boolean
    Private _inMouseMove As Boolean = False
    Private _recentDataSameCount As Integer
    Private _showBaloonTip As Boolean = True
    Private _updating As Boolean = False

    Private Property FormScale As New SizeF(1.0F, 1.0F)
    Private ReadOnly Property SensorLifeToolTip As New ToolTip()

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

    Private WithEvents ActiveInsulinCurrentBGSeries As Series
    Private WithEvents ActiveInsulinMarkerSeries As Series
    Private WithEvents ActiveInsulinSeries As Series
    Private WithEvents HomeTabCurrentBGSeries As Series
    Private WithEvents HomeTabHighLimitSeries As Series
    Private WithEvents HomeTabLowLimitSeries As Series
    Private WithEvents HomeTabMarkerSeries As Series
    Private WithEvents HomeTabTimeChangeSeries As Series
    Private WithEvents HomeTabTimeInRangeSeries As New Series

#End Region

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As Title

#End Region

#End Region

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

        If My.Settings.UseTestData Then
            Me.MenuOptionsUseLastSavedData.Checked = False
            Me.MenuOptionsUseTestData.Checked = True
        ElseIf My.Settings.UseLastSavedData AndAlso Me.MenuStartHereLoadSavedDataFile.Enabled Then
            Me.MenuOptionsUseLastSavedData.Checked = True
            Me.MenuOptionsUseTestData.Checked = False
        End If
        s_timeZoneList = TimeZoneInfo.GetSystemTimeZones.ToList

        Me.AITComboBox.SelectedIndex = Me.AITComboBox.FindStringExact(My.Settings.AIT.ToString("hh\:mm").Substring(1))
        Me.MenuOptionsUseAdvancedAITDecay.CheckState = If(My.Settings.UseAdvancedAITDecay, CheckState.Checked, CheckState.Unchecked)

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Fix(Me)

        Me.ShieldUnitsLabel.Parent = Me.ShieldPictureBox
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Parent = Me.SensorTimeLeftPictureBox
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Left = (Me.SensorTimeLeftPictureBox.Width \ 2) - (Me.SensorDaysLeftLabel.Width \ 2)
        Me.SensorDaysLeftLabel.Top = (Me.SensorTimeLeftPictureBox.Height \ 2) - (Me.SensorDaysLeftLabel.Height \ 2)
        If Me.FormScale.Height > 1 Then
            Me.SplitContainer1.SplitterDistance = 0
        End If
        s_useLocalTimeZone = My.Settings.UseLocalTimeZone
        Me.MenuOptionsUseLocalTimeZone.Checked = s_useLocalTimeZone
        CheckForUpdatesAsync(Me, False)
        If Me.DoOptionalLoginAndUpdateData(False) Then
            Me.FinishInitialization()
            Me.UpdateAllTabPages()
        End If
    End Sub

    Friend Sub FinishInitialization()
        If _initialized Then
            Exit Sub
        End If
        _homePageChartRelitivePosition = RectangleF.Empty
        _updating = True
        Me.Cursor = Cursors.WaitCursor
        _updating = False
        Me.Cursor = Cursors.Default
        Application.DoEvents()

        Me.InitializeHomePageChart()
        Me.InitializeActiveInsulinTabChart()
        Me.InitializeTimeInRangeArea()
        Me.SGsDataGridView.AutoGenerateColumns = True
        Me.SGsDataGridView.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
            .Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        Me.SummaryDataGridView.AutoGenerateColumns = True
        Me.SummaryDataGridView.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
            .Alignment = DataGridViewContentAlignment.MiddleCenter
            }

        _initialized = True
    End Sub

#End Region

#Region "Form Menu Events"

#Region "Start Here Menus"

    Private Sub MenuStartHere_DropDownOpened(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpened
        Me.MenuStartHereLoadSavedDataFile.Enabled = Directory.GetFiles(MyDocumentsPath, $"{RepoName}*.json").Length > 0
        Me.MenuStartHereSnapshotSave.Enabled = RecentData IsNot Nothing
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
                    RecentData.Clear()
                    Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
                    Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
                    ExceptionHandlerForm.ReportFileNameWithPath = fileNameWithPath
                    If ExceptionHandlerForm.ShowDialog() = DialogResult.OK Then
                        ExceptionHandlerForm.ReportFileNameWithPath = ""
                        Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                        RecentData = Loads(ExceptionHandlerForm.LocalRawData)
                        _initialized = False
                        Me.FinishInitialization()
                        Me.UpdateAllTabPages()
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
                    Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
                    Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
                    CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"{RepoName}", True)

                    RecentData = Loads(File.ReadAllText(openFileDialog1.FileName))
                    _initialized = False
                    Me.FinishInitialization()
                    Me.Text = $"{SavedTitle} Using file {Path.GetFileName(openFileDialog1.FileName)}"
                    Me.UpdateAllTabPages()
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.Message}")
            End Try
        End If
    End Sub

    Private Sub MenuStartHereLogin_Click(sender As Object, e As EventArgs) Handles MenuStartHereLogin.Click
        Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
        Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True)
    End Sub

    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) Handles MenuStartHereSnapshotSave.Click
        Using jd As JsonDocument = JsonDocument.Parse(RecentData.CleanUserData(), New JsonDocumentOptions)
            File.WriteAllText(GetDataFileName(RepoSnapshotName, CurrentDateCulture.Name, "json", True).withPath, JsonSerializer.Serialize(jd, JsonFormattingOptions))
        End Using
    End Sub

#End Region

#Region "Option Menus"

    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked
    End Sub

    Private Sub MenuOptionsSetupEmailServer_Click(sender As Object, e As EventArgs) Handles MenuOptionsSetupEmailServer.Click
        MailSetupDialog.ShowDialog()
    End Sub

    Private Sub MenuOptionsUseAdvancedAITDecay_CheckStateChanged(sender As Object, e As EventArgs) Handles MenuOptionsUseAdvancedAITDecay.CheckStateChanged
        Dim increments As Double = TimeSpan.Parse(My.Settings.AIT.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan
        If Me.MenuOptionsUseAdvancedAITDecay.Checked Then
            s_activeInsulinIncrements = CInt(increments * 1.4)
            My.Settings.UseAdvancedAITDecay = True
            Me.AITLabel.Text = "Advanced AIT Decay"
        Else
            s_activeInsulinIncrements = CInt(increments)
            My.Settings.UseAdvancedAITDecay = False
            Me.AITLabel.Text = "Active Insulin Time"
        End If
        My.Settings.Save()
        Me.UpdateActiveInsulinChart()

    End Sub

    Private Sub MenuOptionsUseLastSavedData_CheckStateChanged(sender As Object, e As EventArgs) Handles MenuOptionsUseLastSavedData.CheckStateChanged
        If _inMenuOptions Then Exit Sub
        Select Case Me.MenuOptionsUseLastSavedData.CheckState
            Case CheckState.Checked
                Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
                My.Settings.UseLastSavedData = True
                My.Settings.UseTestData = False
                Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True)
            Case CheckState.Unchecked
                My.Settings.UseLastSavedData = False
                If _initialized AndAlso Not (Me.MenuOptionsUseTestData.Checked OrElse Me.MenuOptionsUseLastSavedData.Checked) Then
                    Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True)
                End If
            Case CheckState.Indeterminate
                _inMenuOptions = True
                My.Settings.UseLastSavedData = False
                Me.MenuOptionsUseLastSavedData.Checked = False
                _inMenuOptions = False
        End Select
        Me.MenuStartHereSnapshotSave.Enabled = Me.MenuOptionsUseLastSavedData.Checked

        My.Settings.Save()
    End Sub

    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) Handles MenuOptionsUseLocalTimeZone.Click
        My.Settings.UseLocalTimeZone = Me.MenuOptionsUseLocalTimeZone.Checked
        My.Settings.Save()
        s_clientTimeZone = CalculateTimeZone()
    End Sub

    Private Sub MenuOptionsUseTestData_Checkchange(sender As Object, e As EventArgs) Handles MenuOptionsUseTestData.CheckStateChanged
        If _inMenuOptions Then Exit Sub
        Select Case Me.MenuOptionsUseTestData.CheckState
            Case CheckState.Checked
                Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
                My.Settings.UseLastSavedData = False
                My.Settings.UseTestData = True
                Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True)
            Case CheckState.Unchecked
                My.Settings.UseTestData = False
                If _initialized AndAlso Not Me.MenuOptionsUseLastSavedData.Checked Then
                    Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True)
                End If
            Case CheckState.Indeterminate
                _inMenuOptions = True
                My.Settings.UseTestData = False
                Me.MenuOptionsUseTestData.Checked = False
                _inMenuOptions = False
        End Select
        My.Settings.Save()
    End Sub

#End Region

#Region "View Menus"

    Private Sub MenuViewShowMiniDisplay_Click(sender As Object, e As EventArgs) Handles MenuViewShowMiniDisplay.Click
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

#Region "HomePage Tab Events"

    Private Sub AITComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AITComboBox.SelectedIndexChanged
        Dim aitTimeSpan As TimeSpan = TimeSpan.Parse(Me.AITComboBox.SelectedItem.ToString())
        My.Settings.AIT = aitTimeSpan
        My.Settings.Save()
        s_activeInsulinIncrements = CInt(TimeSpan.Parse(aitTimeSpan.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan)
        Me.UpdateActiveInsulinChart()
    End Sub

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {Now.AddMinutes(s_timeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub

#Region "Home Page Chart Events"

    Private Sub HomePageChart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles HomeTabChart.CursorPositionChanging
        If Not _initialized Then Exit Sub

        Me.CursorTimer.Interval = s_thirtySecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    Private Sub HomePageChart_MouseMove(sender As Object, e As MouseEventArgs) Handles HomeTabChart.MouseMove

        If Not _initialized Then
            Exit Sub
        End If
        _inMouseMove = True
        Dim yInPixels As Double = Me.HomeTabChart.ChartAreas(NameOf(HomeTabChartArea)).AxisY2.ValueToPixelPosition(e.Y)
        If Double.IsNaN(yInPixels) Then
            Exit Sub
        End If
        Dim result As HitTestResult
        Try
            result = Me.HomeTabChart.HitTest(e.X, e.Y)
            If result?.PointIndex >= -1 Then
                If result.Series IsNot Nothing Then
                    Me.CursorTimeLabel.Left = e.X - (Me.CursorTimeLabel.Width \ 2)
                    Select Case result.Series.Name
                        Case NameOf(HomeTabHighLimitSeries), NameOf(HomeTabLowLimitSeries)
                            Me.CursorMessage1Label.Visible = False
                            Me.CursorMessage2Label.Visible = False
                            Me.CursorPictureBox.Image = Nothing
                            Me.CursorTimeLabel.Visible = False
                            Me.CursorValueLabel.Visible = False
                        Case NameOf(HomeTabMarkerSeries)
                            Dim markerToolTip() As String = result.Series.Points(result.PointIndex).ToolTip.Split(":"c)
                            Dim xValue As Date = Date.FromOADate(result.Series.Points(result.PointIndex).XValue)
                            Me.CursorTimeLabel.Visible = True
                            Me.CursorTimeLabel.Text = xValue.ToString(s_timeWithMinuteFormat)
                            Me.CursorTimeLabel.Tag = xValue
                            markerToolTip(0) = markerToolTip(0).Trim
                            Me.CursorValueLabel.Visible = True
                            Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                            Me.CursorPictureBox.Visible = True
                            Select Case markerToolTip.Length
                                Case 2
                                    Me.CursorMessage1Label.Text = markerToolTip(0)
                                    Select Case markerToolTip(0)
                                        Case "Auto Correction", "Basal", "Bolus"
                                            Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                            Me.CursorMessage1Label.Visible = True
                                        Case "Meal"
                                            Me.CursorPictureBox.Image = My.Resources.MealImageLarge
                                            Me.CursorMessage1Label.Visible = True
                                        Case Else
                                            Me.CursorPictureBox.Image = Nothing
                                            Me.CursorMessage1Label.Visible = False

                                    End Select
                                    Me.CursorMessage2Label.Visible = False
                                    Me.CursorValueLabel.Top = Me.CursorMessage1Label.PositionBelow
                                    Me.CursorValueLabel.Text = markerToolTip(1).Trim
                                Case 3
                                    Select Case markerToolTip(1).Trim
                                        Case "Calibration accepted", "Calibration not accepted"
                                            Me.CursorPictureBox.Image = My.Resources.CalibrationDotRed
                                        Case "Not used For calibration"
                                            Me.CursorPictureBox.Image = My.Resources.CalibrationDot
                                        Case Else
                                            Stop
                                    End Select
                                    Me.CursorMessage1Label.Text = markerToolTip(0)
                                    Me.CursorMessage1Label.Top = Me.CursorPictureBox.PositionBelow
                                    Me.CursorMessage1Label.Visible = True
                                    Me.CursorMessage2Label.Text = markerToolTip(1).Trim
                                    Me.CursorMessage2Label.Top = Me.CursorMessage1Label.PositionBelow
                                    Me.CursorMessage2Label.Visible = True
                                    Me.CursorValueLabel.Text = markerToolTip(2).Trim
                                    Me.CursorValueLabel.Top = Me.CursorMessage2Label.PositionBelow
                                Case Else
                                    Stop
                            End Select
                        Case NameOf(HomeTabCurrentBGSeries)
                            Me.CursorPictureBox.Image = Nothing
                            Me.CursorMessage1Label.Visible = False
                            Me.CursorMessage2Label.Visible = False
                            Me.CursorValueLabel.Visible = False
                            Me.CursorTimeLabel.Text = Date.FromOADate(result.Series.Points(result.PointIndex).XValue).ToString(s_timeWithMinuteFormat)
                            Me.CursorTimeLabel.Visible = True
                            Me.CursorMessage1Label.Text = $"{result.Series.Points(result.PointIndex).YValues(0).RoundDouble(3)} {BgUnitsString}"
                            Me.CursorMessage1Label.Visible = True
                    End Select
                End If
            Else
                Me.CursorMessage1Label.Visible = False
                Me.CursorMessage2Label.Visible = False
                Me.CursorPictureBox.Image = Nothing
                Me.CursorTimeLabel.Visible = False
                Me.CursorValueLabel.Visible = False
            End If
        Catch ex As Exception
            result = Nothing
        Finally
            _inMouseMove = False
        End Try
    End Sub

    <DebuggerNonUserCode()>
    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomeTabChart.PostPaint
        If Not _initialized OrElse _updating OrElse _inMouseMove Then
            Exit Sub
        End If
        If _homePageChartRelitivePosition.IsEmpty Then
            _homePageChartRelitivePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.X, s_sGs(0).OADate))
            _homePageChartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, s_markerRow))
            _homePageChartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, s_limitHigh)))) - _homePageChartRelitivePosition.Y
            _homePageChartRelitivePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.X, s_sGs.Last.OADate)) - _homePageChartRelitivePosition.X
            _homePageChartRelitivePosition = e.ChartGraphics.GetAbsoluteRectangle(_homePageChartRelitivePosition)
        End If

        Dim homePageChartY As Integer = CInt(_homePageChartRelitivePosition.Y)
        Dim homePageChartWidth As Integer = CInt(_homePageChartRelitivePosition.Width)
        Dim highLimitY As Double = e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, s_limitHigh)
        Dim lowLimitY As Double = e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, s_limitLow)

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            Dim highHeight As Integer = CInt(255 * Me.FormScale.Height)
            Dim homePagelocation As New Point(CInt(_homePageChartRelitivePosition.X), homePageChartY)
            Dim highAreaRectangle As New Rectangle(homePagelocation,
                                                   New Size(homePageChartWidth, highHeight))
            e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)

            Dim lowOffset As Integer = CInt((10 + _homePageChartRelitivePosition.Height) * Me.FormScale.Height)
            Dim lowStartLocation As New Point(CInt(_homePageChartRelitivePosition.X), lowOffset)

            Dim lowRawHeight As Integer = CInt((50 - homePageChartY) * Me.FormScale.Height)
            Dim lowHeight As Integer = If(Me.HomeTabChartArea.AxisX.ScrollBar.IsVisible,
                                          CInt(lowRawHeight - Me.HomeTabChartArea.AxisX.ScrollBar.Size),
                                          lowRawHeight
                                         )
            Dim lowAreaRectangle As New Rectangle(lowStartLocation,
                                                  New Size(homePageChartWidth, lowHeight))
            e.ChartGraphics.Graphics.FillRectangle(b, lowAreaRectangle)
            If Me.CursorTimeLabel.Tag IsNot Nothing Then
                Me.CursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.X, Me.CursorTimeLabel.Tag.ToString.ParseDate("").ToOADate))
            End If
        End Using

        e.PaintMarker(s_mealImage, s_markerMealDictionary, 0)
        e.PaintMarker(_insulinImage, s_markerInsulinDictionary, -6)
    End Sub

    Private Sub SensorAgeLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles SensorDaysLeftLabel.MouseHover
        If s_sensorDurationHours < 24 Then
            Me.SensorLifeToolTip.SetToolTip(Me.CalibrationDueImage, $"Sensor will expire in {s_sensorDurationHours} hours")
        End If
    End Sub

#End Region

#End Region

#Region "SGS Tab Events"

    Private Sub SGsDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles SGsDataGridView.CellFormatting
        ' Set the background to red for negative values in the Balance column.
        If Me.SGsDataGridView.Columns(e.ColumnIndex).Name.Equals(NameOf(s_sensorState), StringComparison.OrdinalIgnoreCase) Then
            If CStr(e.Value) <> "NO_ERROR_MESSAGE" Then
                e.CellStyle.BackColor = Color.Yellow
            End If
        End If
        If Me.SGsDataGridView.Columns(e.ColumnIndex).Name.Equals(NameOf(DateTime), StringComparison.OrdinalIgnoreCase) Then
            If e.Value IsNot Nothing Then
                Dim dateValue As Date = e.Value.ToString.ParseDate("")
                e.Value = $"{dateValue.ToShortDateString()} {dateValue.ToShortTimeString()}"
            End If
        End If
        If Me.SGsDataGridView.Columns(e.ColumnIndex).Name.Equals(NameOf(SgRecord.sg), StringComparison.OrdinalIgnoreCase) Then
            If e.Value IsNot Nothing Then
                Dim sendorValue As Single = CSng(e.Value)
                If Single.IsNaN(sendorValue) Then
                    e.CellStyle.BackColor = Color.Gray
                ElseIf sendorValue < 70 Then
                    e.CellStyle.BackColor = Color.Red
                ElseIf sendorValue > 180 Then
                    e.CellStyle.BackColor = Color.Orange
                End If
            End If
        End If

    End Sub

    Private Sub SGsDataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles SGsDataGridView.ColumnAdded
        With e.Column
            If .Name = NameOf(SgRecord.OADate) Then
                .Visible = False
                Exit Sub
            End If
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .ReadOnly = True
            .Resizable = DataGridViewTriState.False
            .HeaderText = .Name.ToTitleCase()
            .DefaultCellStyle = SgRecord.GetCellStyle(.Name)
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    'Private Sub SGsDataGridView_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles SGsDataGridView.ColumnHeaderMouseClick
    '    Dim dgv As DataGridView = CType(sender, DataGridView)
    '    Dim currentSortOrder As SortOrder = dgv.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection
    '    If currentSortOrder = SortOrder.None OrElse currentSortOrder = SortOrder.Ascending Then
    '        _sGsBindingSource.Sort = $"{NameOf(SgRecord.RecordNumber)} Desc"
    '        currentSortOrder = SortOrder.Descending
    '    Else
    '        _sGsBindingSource.Sort = $"{NameOf(SgRecord.RecordNumber)} Asc"
    '        currentSortOrder = SortOrder.Ascending
    '    End If
    '    Me.SGsDataGridView.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = currentSortOrder
    'End Sub

#End Region

#Region "Summary Tab Events"

    Private Sub SummaryDataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles SummaryDataGridView.ColumnAdded
        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .ReadOnly = True
            .Resizable = DataGridViewTriState.False
            .HeaderText = .Name.ToTitleCase()
            .DefaultCellStyle = SummaryRecord.GetCellStyle(.Name)
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    Private Sub SummaryDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles SummaryDataGridView.DataError
        Stop
    End Sub

#End Region

#End Region

#Region "Timer Events"

    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not Me.HomeTabChartArea.AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.HomeTabChartArea.CursorX.Position = Double.NaN
        End If
    End Sub

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        Me.ServerUpdateTimer.Stop()
        RecentData = _client.GetRecentData()
        If Me.IsRecentDataUpdated Then
            Me.UpdateAllTabPages()
        ElseIf RecentData Is Nothing Then
            _client = New CareLinkClient(Me.LoginStatus, My.Settings.CareLinkUserName, My.Settings.CareLinkPassword, My.Settings.CountryCode)
            _loginDialog.Client = _client
            RecentData = _client.GetRecentData()
            If Me.IsRecentDataUpdated Then
                Me.UpdateAllTabPages()
            End If
        End If
        Application.DoEvents()
        Me.ServerUpdateTimer.Interval = s_minuteInMilliseconds
        Me.ServerUpdateTimer.Start()
        Debug.Print($"Me.ServerUpdateTimer Started at {Now}")
        Me.Cursor = Cursors.Default
    End Sub

#End Region ' Timer

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Home Tab Charts"

    Private Sub InitializeHomePageChart()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.HomeTabChart = CreateNewChart(NameOf(ActiveInsulinChart))

        Me.HomeTabChartArea = CreateNewChartArea(NameOf(HomeTabChartArea))
        With Me.HomeTabChartArea
            With .AxisY
                .MajorTickMark = New TickMark() With {.Interval = InsulinRow, .Enabled = False}
                .Maximum = MarkerRow
                .Minimum = InsulinRow
            End With
            With .AxisY2
                .Interval = InsulinRow
                .IsMarginVisible = False
                .IsStartedFromZero = False
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid = New Grid With {
                        .Interval = InsulinRow,
                        .LineColor = Color.FromArgb(64, 64, 64, 64)
                    }
                .MajorTickMark = New TickMark() With {.Interval = InsulinRow, .Enabled = True}
                .Maximum = MarkerRow
                .Minimum = InsulinRow
            End With
        End With
        Me.HomeTabChart.ChartAreas.Add(Me.HomeTabChartArea)

        Dim defaultLegend As Legend = CreateDefaultLegend(NameOf(defaultLegend))
        Me.HomeTabCurrentBGSeries = CreateBgSeries(NameOf(HomeTabCurrentBGSeries), NameOf(HomeTabChartArea), NameOf(defaultLegend))
        Me.HomeTabMarkerSeries = New Series With {
                .BorderColor = Color.Transparent,
                .BorderWidth = 1,
                .ChartArea = NameOf(HomeTabChartArea),
                .ChartType = SeriesChartType.Point,
                .Color = Color.HotPink,
                .Name = NameOf(HomeTabMarkerSeries),
                .MarkerSize = 12,
                .MarkerStyle = MarkerStyle.Circle,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }

        Me.HomeTabHighLimitSeries = New Series With {
                .BorderColor = Color.FromArgb(180, Color.Orange),
                .BorderWidth = 2,
                .ChartArea = NameOf(HomeTabChartArea),
                .ChartType = SeriesChartType.StepLine,
                .Color = Color.Orange,
                .Name = NameOf(HomeTabHighLimitSeries),
                .ShadowColor = Color.Black,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }
        Me.HomeTabLowLimitSeries = New Series With {
                .BorderColor = Color.FromArgb(180, Color.Red),
                .BorderWidth = 2,
                .ChartArea = NameOf(HomeTabChartArea),
                .ChartType = SeriesChartType.StepLine,
                .Color = Color.Red,
                .Name = NameOf(HomeTabLowLimitSeries),
                .ShadowColor = Color.Black,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }
        Me.HomeTabTimeChangeSeries = CreateTimeChangeSeries(NameOf(HomeTabTimeChangeSeries))

        Me.SplitContainer3.Panel1.Controls.Add(Me.HomeTabChart)
        Application.DoEvents()
        Me.HomeTabChart.Legends.Add(defaultLegend)
        Me.HomeTabChart.Series.Add(Me.HomeTabCurrentBGSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabMarkerSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabHighLimitSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabLowLimitSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabTimeChangeSeries)
        Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)).EmptyPointStyle.BorderWidth = 4
        Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)).EmptyPointStyle.Color = Color.Transparent
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

#End Region

#Region "Initialize Active Insulin Tab Charts"

    Private Sub InitializeActiveInsulinTabChart()
        Me.TabPage2RunningActiveInsulin.Controls.Clear()

        Me.ActiveInsulinChart = CreateNewChart(NameOf(HomeTabChart))
        Me.ActiveInsulinChartArea = CreateNewChartArea(NameOf(ActiveInsulinChartArea))
        With Me.ActiveInsulinChartArea
            With .AxisY
                .MajorTickMark = New TickMark() With {.Interval = InsulinRow, .Enabled = False}
                .Maximum = 25
                .Minimum = 0
                .Interval = 4
                .Title = "Active Insulin"
                .TitleForeColor = Color.HotPink
            End With
            With .AxisY2
                .Interval = InsulinRow
                .Maximum = MarkerRow
                .Minimum = InsulinRow
                .Title = "BG Value"
            End With
        End With
        Me.ActiveInsulinChart.ChartAreas.Add(Me.ActiveInsulinChartArea)

        Me.ActiveInsulinChartLegend = CreateDefaultLegend(NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinChart.Legends.Add(Me.ActiveInsulinChartLegend)
        Me.ActiveInsulinSeries = New Series With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = NameOf(ActiveInsulinChartArea),
            .ChartType = SeriesChartType.Line,
            .Color = Color.HotPink,
            .Legend = NameOf(ActiveInsulinChartLegend),
            .Name = NameOf(ActiveInsulinSeries),
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }
        Me.ActiveInsulinCurrentBGSeries = CreateBgSeries(NameOf(ActiveInsulinCurrentBGSeries), NameOf(ActiveInsulinChartArea), NameOf(ActiveInsulinChartLegend))
        Me.ActiveInsulinMarkerSeries = New Series With {
            .BorderColor = Color.Transparent,
            .BorderWidth = 1,
            .ChartArea = NameOf(ActiveInsulinChartArea),
            .ChartType = SeriesChartType.Point,
            .Color = Color.HotPink,
            .Name = NameOf(ActiveInsulinMarkerSeries),
            .MarkerSize = 8,
            .MarkerStyle = MarkerStyle.Circle,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }

        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinCurrentBGSeries)
        Me.ActiveInsulinChart.Series.Add(Me.ActiveInsulinMarkerSeries)

        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinCurrentBGSeries)).EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinCurrentBGSeries)).EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinChartTitle = New Title With {
                .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(26, 59, 105),
                .Name = NameOf(ActiveInsulinChartTitle),
                .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                .ShadowOffset = 3
            }
        Me.ActiveInsulinChart.Titles.Add(Me.ActiveInsulinChartTitle)
        Me.TabPage2RunningActiveInsulin.Controls.Add(Me.ActiveInsulinChart)
        Application.DoEvents()

    End Sub

#End Region

#End Region

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

    Private Sub FillOneRowOfTableLayoutPanel(layoutPanel As TableLayoutPanel, innerJson As List(Of Dictionary(Of String, String)), rowIndex As ItemIndexs, filterJsonData As Boolean, timeFormat As String)
        For Each jsonEntry As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
            Dim innerTableBlue As TableLayoutPanel = CreateTableLayoutPanel(NameOf(innerTableBlue), 0, SystemColors.Control)
            layoutPanel.Controls.Add(innerTableBlue, 1, jsonEntry.Index)
            Application.DoEvents()
            Me.GetInnerTable(jsonEntry.Value, innerTableBlue, rowIndex, filterJsonData, timeFormat, Me.FormScale.Height <> 1)
            Application.DoEvents()
        Next
    End Sub

    Private Sub GetInnerTable(innerJson As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel, itemIndex As ItemIndexs, filterJsonData As Boolean, timeFormat As String, isScaledForm As Boolean)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.BackColor = Color.LightBlue
        Dim messageOrDefault As KeyValuePair(Of String, String) = innerJson.Where(Function(kvp As KeyValuePair(Of String, String)) kvp.Key = "messageId").FirstOrDefault
        If itemIndex = ItemIndexs.lastAlarm AndAlso messageOrDefault.Key IsNot Nothing Then
            tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            Dim keyLabel As Label = CreateBasicLabel("messageId")
            tableLevel1Blue.RowCount += 1
            Dim textBox1 As TextBox = CreateValueTextBox(innerJson, messageOrDefault, timeFormat, isScaledForm)

            If textBox1.Text.Length > 100 Then
                My.Forms.Form1.ToolTip1.SetToolTip(textBox1, textBox1.Text)
            Else
                My.Forms.Form1.ToolTip1.SetToolTip(textBox1, Nothing)
            End If
            tableLevel1Blue.Controls.AddRange({keyLabel,
                                                       textBox1
                                                      }
                                             )
        End If

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Application.DoEvents()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            ' Comment out 4 lines below to see all data fields.
            ' I did not see any use to display the filtered out ones
            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) AndAlso innerJson.Count > 4 Then
                If s_zFilterList(itemIndex).Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            If innerRow.Key = "activeNotifications" Then
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            Else
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            End If
            tableLevel1Blue.RowCount += 1
            If itemIndex = ItemIndexs.limits OrElse itemIndex = ItemIndexs.markers OrElse (itemIndex = ItemIndexs.notificationHistory AndAlso c.Value.Key = "activeNotifications") Then
                tableLevel1Blue.AutoSize = True
            End If

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = LoadList(innerRow.Value)
                If innerJson1.Count > 0 Then
                    Dim tableLevel2 As TableLayoutPanel = CreateTableLayoutPanel(NameOf(tableLevel2), innerJson1.Count, Color.LightBlue)

                    For i As Integer = 0 To innerJson1.Count - 1
                        tableLevel2.RowStyles.Add(New RowStyle() With {.SizeType = SizeType.AutoSize})
                    Next
                    tableLevel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 80.0))
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson1.WithIndex()
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        tableLevel2.RowStyles.Add(New RowStyle(SizeType.Absolute, 4 + (dic.Keys.Count * 22)))
                        Dim tableLevel3 As TableLayoutPanel = CreateTableLayoutPanel(NameOf(tableLevel3), 0, Color.Aqua)
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            Dim eValue As KeyValuePair(Of String, String) = e.Value

                            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) Then
                                If s_zFilterList(itemIndex).Contains(eValue.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3.RowCount += 1
                            tableLevel3.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0))

                            Dim valueLabel As Label = CreateBasicLabel(eValue.Key)
                            If eValue.Key.Equals("messageid", StringComparison.OrdinalIgnoreCase) Then
                                tableLevel3.Controls.AddRange({valueLabel, CreateBasicTextBox("")})
                                valueLabel = CreateBasicLabel("Message")
                            End If
                            tableLevel3.Controls.AddRange({valueLabel, CreateValueTextBox(dic, eValue, timeFormat, isScaledForm)})
                            Application.DoEvents()
                        Next
                        tableLevel3.Height += 40
                        tableLevel2.Controls.Add(tableLevel3, 0, innerDictionary.Index)
                        tableLevel2.Height += 4
                        Application.DoEvents()
                    Next
                    tableLevel1Blue.Controls.AddRange({CreateBasicLabel(innerRow.Key), tableLevel2})
                Else
                    tableLevel1Blue.Controls.AddRange({CreateBasicLabel(innerRow.Key), CreateBasicTextBox("")})
                End If
            Else
                ' This is ItemIndexs.lastAlarm and its already been done
                If innerRow.Key <> "messageId" Then
                    Dim textBox1 As TextBox = CreateValueTextBox(innerJson, innerRow, timeFormat, isScaledForm)
                    My.Forms.Form1.ToolTip1.SetToolTip(textBox1, textBox1.Text)
                    tableLevel1Blue.Controls.AddRange({CreateBasicLabel(innerRow.Key), textBox1})
                End If
            End If
        Next

        If itemIndex = ItemIndexs.lastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        ElseIf itemIndex = ItemIndexs.lastAlarm Then
            Dim parentTableLayoutPanel As TableLayoutPanel = CType(tableLevel1Blue.Parent, TableLayoutPanel)
            parentTableLayoutPanel.AutoSize = False
            tableLevel1Blue.Dock = DockStyle.Fill
            Application.DoEvents()
            tableLevel1Blue.ColumnStyles(1).SizeType = SizeType.Absolute
            If tableLevel1Blue.RowCount > 7 Then
                parentTableLayoutPanel.AutoScroll = True
            Else
                parentTableLayoutPanel.Width = 870
                tableLevel1Blue.AutoScroll = False
            End If
            Dim tableLevel1BlueWidth As Integer = tableLevel1Blue.Width
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Height = 22 * (tableLevel1Blue.RowCount - 1)
            tableLevel1Blue.Dock = DockStyle.None
            Application.DoEvents()
            tableLevel1Blue.Width = tableLevel1BlueWidth - 30
            Application.DoEvents()
            tableLevel1Blue.Dock = DockStyle.Fill
            Application.DoEvents()
        ElseIf itemIndex = ItemIndexs.notificationHistory Then
            tableLevel1Blue.RowStyles(1).SizeType = SizeType.AutoSize
        End If
        Application.DoEvents()
    End Sub

    Private Function IsRecentDataUpdated() As Boolean
        If _recentDataSameCount >= 4 Then
            _recentDataSameCount = 0
            Return True
        Else
            _recentDataSameCount += 1
            If s_recentDatalast Is Nothing OrElse RecentData Is Nothing Then
                Return True
            End If

            Dim v1 As String = Nothing
            Dim v2 As String = Nothing

            If Not RecentData.TryGetValue(NameOf(ItemIndexs.lastSG), v1) Then
                Return True
            End If
            If Not s_recentDatalast.TryGetValue(NameOf(ItemIndexs.lastSG), v2) Then
                Return True
            End If
            Dim entry1 As Dictionary(Of String, String) = Loads(v1)
            Dim entry2 As Dictionary(Of String, String) = Loads(v1)
            If Not entry1.TryGetValue("datetime", v1) Then
                Return True
            End If
            If Not entry2.TryGetValue("datetime", v2) Then
                Return True
            End If
            If v1 <> v2 Then
                _recentDataSameCount = 0
                Return True
            End If
            Return False
        End If
    End Function

    Private Sub ProcesListOfDictionary(layoutPanel1 As TableLayoutPanel, innerListDictionary As List(Of Dictionary(Of String, String)), rowIndex As ItemIndexs)
        Dim rowHeader As Label = InitializeColumnLabel(layoutPanel1, rowIndex)
        layoutPanel1.Parent.Parent.UseWaitCursor = True
        Application.DoEvents()
        layoutPanel1.Invoke(Sub()
                                Me.FillOneRowOfTableLayoutPanel(
                                            layoutPanel1,
                                            innerListDictionary,
                                            rowIndex,
                                            s_filterJsonData,
                                            s_timeWithMinuteFormat)
                            End Sub)
        layoutPanel1.Parent.Parent.UseWaitCursor = False
        Application.DoEvents()
    End Sub

#Region "Update Data and Tables"

    Private Shared Sub ResetAllVariables()
        s_limits.Clear()
        s_markerInsulinDictionary.Clear()
        s_markerMealDictionary.Clear()
        s_markers.Clear()
        s_sGs.Clear()
        s_sGsBindingSource.Clear()
        s_summaryBindingSource.Clear()
    End Sub

    Private Sub ProcessAllSingleEntries(row As KeyValuePair(Of String, String), rowIndex As ItemIndexs, ByRef firstName As String)
        Select Case rowIndex
            Case ItemIndexs.lastSensorTS
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.medicalDeviceTimeAsString
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSensorTSAsString
                If s_summaryBindingSource.Count < ItemIndexs.lastSensorTSAsString Then
                    s_summaryBindingSource.Insert(ItemIndexs.lastSensorTS, New SummaryRecord(ItemIndexs.lastSensorTS, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastSensorTS), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastSensorTS), CurrentUICulture))))
                End If
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.kind,
                 ItemIndexs.version
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.pumpModelNumber
                Me.ModelLabel.Text = row.Value
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.currentServerTime,
               ItemIndexs.lastConduitTime,
               ItemIndexs.lastConduitUpdateServerTime,
               ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.firstName
                firstName = row.Value
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastName
                Me.FullNameLabel.Text = $"{firstName} {row.Value}"
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitSerialNumber,
                 ItemIndexs.conduitBatteryLevel,
                 ItemIndexs.conduitBatteryStatus
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitInRange
                s_conduitSensorInRange = CBool(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitMedicalDeviceInRange,
                 ItemIndexs.conduitSensorInRange,
                 ItemIndexs.medicalDeviceFamily
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorState
                s_sensorState = row.Value
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceSerialNumber
                Me.SerialNumberLabel.Text = row.Value
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceTime
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.sMedicalDeviceTime
                If s_summaryBindingSource.Count < ItemIndexs.sMedicalDeviceTime Then
                    s_summaryBindingSource.Add(New SummaryRecord(ItemIndexs.medicalDeviceTime, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.medicalDeviceTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.medicalDeviceTime), CurrentUICulture))))
                End If
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirLevelPercent
                s_reservoirLevelPercent = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirAmount
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirRemainingUnits
                s_reservoirRemainingUnits = row.Value.ParseDouble
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceBatteryLevelPercent
                s_medicalDeviceBatteryLevelPercent = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorDurationHours
                s_sensorDurationHours = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeToNextCalibHours
                s_timeToNextCalibHours = CUShort(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.calibStatus
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.bgUnits
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))
                Me.AboveHighLimitMessageLabel.Text = $"Above {s_limitHigh} {BgUnitsString}"
                Me.BelowLowLimitMessageLabel.Text = $"Below {s_limitLow} {BgUnitsString}"

            Case ItemIndexs.timeFormat
                s_timeWithMinuteFormat = If(row.Value = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                s_timeWithoutMinuteFormat = If(row.Value = "HR_12", TwelveHourTimeWithoutMinuteFormat, MilitaryTimeWithoutMinuteFormat)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSensorTime
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.sLastSensorTime
                If s_summaryBindingSource.Count < ItemIndexs.sLastSensorTime Then
                    s_summaryBindingSource.Add(New SummaryRecord(ItemIndexs.lastSensorTime, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastSensorTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.medicalDeviceTime), CurrentUICulture))))
                End If
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceSuspended,
             ItemIndexs.lastSGTrend
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.systemStatusMessage
                s_systemStatusMessage = row.Value
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.averageSG
                s_averageSG = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.belowHypoLimit
                s_belowHypoLimit = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.aboveHyperLimit
                s_aboveHyperLimit = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeInRange
                s_timeInRange = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.pumpCommunicationState,
             ItemIndexs.gstCommunicationState
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.gstBatteryLevel
                s_gstBatteryLevel = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastConduitDateTime
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastConduitDateTime), CurrentUICulture))))

            Case ItemIndexs.maxAutoBasalRate,
             ItemIndexs.maxBolusAmount
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorDurationMinutes
                s_sensorDurationMinutes = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeToNextCalibrationMinutes
                s_timeToNextCalibrationMinutes = CInt(row.Value)
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.clientTimeZoneName
                s_clientTimeZoneName = row.Value
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sgBelowLimit,
             ItemIndexs.averageSGFloat,
             ItemIndexs.timeToNextCalibrationRecommendedMinutes,
             ItemIndexs.calFreeSensor,
             ItemIndexs.finalCalibration
                s_summaryBindingSource.Add(New SummaryRecord(rowIndex, row))
            Case Else
                Stop
        End Select
    End Sub

    Private Sub UpdateDataTables(isScaledForm As Boolean)
        If RecentData Is Nothing Then
            Exit Sub
        End If
        _updating = True
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Dim firstName As String = ""
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In RecentData.WithIndex()
            Dim layoutPanel1 As TableLayoutPanel = Nothing
            Dim row As KeyValuePair(Of String, String) = c.Value
            Dim rowIndex As ItemIndexs = CType([Enum].Parse(GetType(ItemIndexs), c.Value.Key), ItemIndexs)

            If rowIndex = ItemIndexs.sgs Then
                s_sGs = LoadList(row.Value).ToSgList()
                s_sGsBindingSource.DataSource = s_sGs
                Me.SGsDataGridView.DataSource = s_sGsBindingSource
                For Each column As DataGridViewTextBoxColumn In Me.SGsDataGridView.Columns
                    If s_filterJsonData AndAlso s_alwaysFilter.Contains(column.Name) Then
                        Me.SGsDataGridView.Columns(column.Name).Visible = False
                    End If
                Next
                Me.ReadingsLabel.Text = $"{s_sGs.Where(Function(entry As SgRecord) Not Double.IsNaN(entry.sg)).Count}/288"
                Continue For
            End If

            If rowIndex <= ItemIndexs.lastSGTrend OrElse rowIndex >= ItemIndexs.systemStatusMessage Then
                Me.ProcessAllSingleEntries(row, rowIndex, firstName)
                Continue For
            End If

            If rowIndex = ItemIndexs.limits OrElse
                rowIndex = ItemIndexs.markers OrElse
                rowIndex = ItemIndexs.pumpBannerState Then

                Dim innerListDictionary As New List(Of Dictionary(Of String, String))
                Select Case rowIndex
                    Case ItemIndexs.limits
                        InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelLimits, True)
                        For Each innerdic As Dictionary(Of String, String) In LoadList(row.Value)
                            Dim newLimit As New Dictionary(Of String, String)
                            For Each kvp As KeyValuePair(Of String, String) In innerdic
                                Select Case kvp.Key
                                    Case "lowLimit", "highLimit"
                                        newLimit.Add(kvp.Key, CStr((CDbl(kvp.Value) / scaleUnitsDivisor).RoundDouble(1)))
                                    Case Else
                                        newLimit.Add(kvp.Key, kvp.Value)
                                End Select
                            Next
                            s_limits.Add(newLimit)
                        Next
                        innerListDictionary = s_limits
                    Case ItemIndexs.markers
                        InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelMarkers)
                        For Each innerdic As Dictionary(Of String, String) In LoadList(row.Value)
                            Dim newMarker As New Dictionary(Of String, String)
                            Select Case innerdic("type")
                                Case "AUTO_BASAL_DELIVERY",
                                     "AUTO_MODE_STATUS",
                                     "INSULIN",
                                     "LOW_GLUCOSE_SUSPENDED",
                                     "MEAL",
                                     "TIME_CHANGE"
                                    newMarker = innerdic
                                Case "BG_READING", "CALIBRATION"
                                    For Each kvp As KeyValuePair(Of String, String) In innerdic
                                        Select Case kvp.Key
                                            Case "value"
                                                newMarker.Add(kvp.Key, CStr((CDbl(kvp.Value) / scaleUnitsDivisor).RoundDouble(1)))
                                            Case Else
                                                newMarker.Add(kvp.Key, kvp.Value)
                                        End Select
                                    Next

                                Case Else
                                    Stop
                            End Select
                            s_markers.Add(newMarker)
                        Next
                        innerListDictionary = s_markers
                    Case ItemIndexs.pumpBannerState
                        InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelBannerState, True)
                        If row.Value IsNot Nothing Then
                            innerListDictionary = LoadList(row.Value)
                        Else
                            innerListDictionary = New List(Of Dictionary(Of String, String))
                        End If
                End Select

                If innerListDictionary.Count = 0 Then
                    Dim rowHeader As Label = InitializeColumnLabel(layoutPanel1, rowIndex)
                    Dim rowTextBox As TextBox = CreateBasicTextBox("")
                    rowTextBox.BackColor = Color.LightGray

                    layoutPanel1.Controls.Add(rowTextBox, 1, 0)
                Else
                    Me.ProcesListOfDictionary(layoutPanel1, innerListDictionary, rowIndex)
                End If
                Continue For
            End If

            Dim docStyle As DockStyle = DockStyle.Fill
            Select Case rowIndex
                Case ItemIndexs.lastSG
                    InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelTop1)
                    s_lastSG = Loads(row.Value)

                Case ItemIndexs.lastAlarm
                    InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelTop2)

                Case ItemIndexs.activeInsulin
                    InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelActiveInsulin)
                    s_activeInsulin = Loads(row.Value)

                Case ItemIndexs.notificationHistory
                    InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelNotificationHistory)

                Case ItemIndexs.therapyAlgorithmState
                    InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelTherapyAlgorthm, True)
                    docStyle = DockStyle.Top

                Case ItemIndexs.basal
                    InitializeWorkingPanel(layoutPanel1, Me.TableLayoutPanelBasal)
                    docStyle = DockStyle.Top

                Case Else
                    Stop
                    Exit Select
            End Select

            Try
                Dim rowHeader As Label = InitializeColumnLabel(layoutPanel1, rowIndex)
                layoutPanel1.RowStyles(0).SizeType = SizeType.AutoSize
                Dim innerJsonDictionary As Dictionary(Of String, String) = Loads(row.Value)
                Dim innerTableBlue As TableLayoutPanel = CreateTableLayoutPanel(NameOf(innerTableBlue), 0, Color.Aqua)
                innerTableBlue.AutoScroll = rowIndex <> ItemIndexs.notificationHistory
                layoutPanel1.Controls.Add(innerTableBlue,
                                          1,
                                          0)
                Me.GetInnerTable(innerJsonDictionary, innerTableBlue, rowIndex, s_filterJsonData, s_timeWithMinuteFormat, isScaledForm)
            Catch ex As Exception
                Stop
                Throw
            End Try
        Next
        _initialized = True
        _updating = False
        Me.Cursor = Cursors.Default
    End Sub

#End Region

    Private Sub UpdateActiveInsulinChart()
        If Not _initialized Then
            Exit Sub
        End If

        With Me.ActiveInsulinChart
            .Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Active Insulin in Pink"
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.Minimum = s_sGs(0).OADate()
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.Maximum = s_sGs.Last.OADate()
            For Each s As Series In .Series
                s.Points.Clear()
            Next
        End With

        ' Order all markers by time
        Dim timeOrderedMarkers As New SortedDictionary(Of Double, Double)
        Dim sgOaDateTime As Double

        For Each marker As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            sgOaDateTime = s_markers.SafeGetSgDateTime(marker.Index).RoundTimeDown(RoundTo.Minute).ToOADate
            Select Case marker.Value("type").ToString
                Case "AUTO_BASAL_DELIVERY"
                    Dim bolusAmount As Double = marker.Value.GetDoubleValue("bolusAmount")
                    If timeOrderedMarkers.ContainsKey(sgOaDateTime) Then
                        timeOrderedMarkers(sgOaDateTime) += bolusAmount
                    Else
                        timeOrderedMarkers.Add(sgOaDateTime, bolusAmount)
                    End If
                Case "AUTO_MODE_STATUS"
                Case "BG_READING"
                Case "CALIBRATION"
                Case "INSULIN"
                    Dim bolusAmount As Double = marker.Value.GetDoubleValue("deliveredFastAmount")
                    If timeOrderedMarkers.ContainsKey(sgOaDateTime) Then
                        timeOrderedMarkers(sgOaDateTime) += bolusAmount
                    Else
                        timeOrderedMarkers.Add(sgOaDateTime, bolusAmount)
                    End If
                Case "LOW_GLUCOSE_SUSPENDED"
                Case "MEAL"
                Case "TIME_CHANGE"
                Case Else
                    Stop
            End Select
        Next

        ' set up table that holds active insulin for every 5 minutes
        Dim remainingInsulinList As New List(Of Insulin)
        Dim currentMarker As Integer = 0

        For i As Integer = 0 To 287
            Dim initialBolus As Double = 0
            Dim oaTime As Double = (s_sGs(0).datetime + (s_fiveMinuteSpan * i)).RoundTimeDown(RoundTo.Minute).ToOADate()
            While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= oaTime
                initialBolus += timeOrderedMarkers.Values(currentMarker)
                currentMarker += 1
            End While
            remainingInsulinList.Add(New Insulin(oaTime, initialBolus, s_activeInsulinIncrements, Me.MenuOptionsUseAdvancedAITDecay.Checked))
        Next

        Me.ActiveInsulinChartArea.AxisY2.Maximum = MarkerRow

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

        s_totalAutoCorrection = 0
        s_totalBasal = 0
        s_totalCarbs = 0
        s_totalDailyDose = 0
        s_totalManualBolus = 0

        For Each marker As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            sgOaDateTime = s_markers.SafeGetSgDateTime(marker.Index).RoundTimeDown(RoundTo.Minute).ToOADate
            With Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinMarkerSeries))
                Select Case marker.Value("type")
                    Case "INSULIN"
                        .Points.AddXY(sgOaDateTime, maxActiveInsulin)
                        Dim deliveredAmount As Single = marker.Value("deliveredFastAmount").ParseSingle
                        s_totalDailyDose += deliveredAmount
                        Select Case marker.Value("activationType")
                            Case "AUTOCORRECTION"
                                .Points.Last.ToolTip = $"Auto Correction: {deliveredAmount.ToString(CurrentUICulture)} U"
                                .Points.Last.Color = Color.MediumPurple
                                s_totalAutoCorrection += deliveredAmount
                            Case "RECOMMENDED", "UNDETERMINED"
                                .Points.Last.ToolTip = $"Bolus: {deliveredAmount.ToString(CurrentUICulture)} U"
                                .Points.Last.Color = Color.LightBlue
                                s_totalManualBolus += deliveredAmount
                            Case Else
                                Stop
                        End Select
                        .Points.Last.MarkerSize = 15
                        .Points.Last.MarkerStyle = MarkerStyle.Square

                    Case "AUTO_BASAL_DELIVERY"
                        Dim bolusAmount As Double = marker.Value.GetDoubleValue("bolusAmount")
                        .Points.AddXY(sgOaDateTime, maxActiveInsulin)
                        .Points.Last.ToolTip = $"Basal: {bolusAmount.RoundDouble(3).ToString(CurrentUICulture)} U"
                        .Points.Last.MarkerSize = 8
                        s_totalBasal += CSng(bolusAmount)
                        s_totalDailyDose += CSng(bolusAmount)
                    Case "MEAL"
                        s_totalCarbs += marker.Value.GetDoubleValue("amount")
                    Case "AUTO_MODE_STATUS"
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "LOW_GLUCOSE_SUSPENDED"
                    Case "TIME_CHANGE"
                    Case Else
                        Stop
                End Select
            End With
        Next
        For Each sgListIndex As IndexClass(Of SgRecord) In s_sGs.WithIndex()
            Dim bgValue As Single = sgListIndex.Value.sg

            Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinCurrentBGSeries)).PlotOnePoint(
                sgListIndex.Value.OADate(),
                sgListIndex.Value.sg,
                Color.Black)
        Next
        _initialized = True
        Application.DoEvents()
    End Sub

#Region "All Home Tab Charts"

    Private Sub UpdateDosingAndCarbs()
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
            Me.ManualBolusLabel.Text = $"Bolus {s_totalManualBolus.RoundSingle(1)} U | {totalPercent}%"
        End If
        Me.Last24CarbsValueLabel.Text = s_totalCarbs.ToString
    End Sub

#End Region

    Friend Sub UpdateAllTabPages()
        If RecentData Is Nothing OrElse _updating Then
            Exit Sub
        End If
        If RecentData.Count > ItemIndexs.finalCalibration + 1 Then
            Stop
        End If
        Me.LastUpdateTime.Text = Now.ToShortTimeString
        ResetAllVariables()
        Me.SummaryDataGridView.DataSource = s_summaryBindingSource

        _updating = True
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

        Me.UpdateZHomeTabSerieses()
        Me.UpdateDosingAndCarbs()
        s_recentDatalast = RecentData
        _initialized = True
        _updating = False
        Application.DoEvents()
    End Sub

#Region "Home Tab Update Utilities"

    Private Sub UpdateActiveInsulin()
        Dim activeInsulinStr As String = $"{s_activeInsulin("amount"):N3}"
        Me.ActiveInsulinValue.Text = $"{activeInsulinStr} U"
        _bgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr}U"
    End Sub

    Private Sub UpdateAutoModeShield()
        Me.SensorMessage.Location = New Point(Me.ShieldPictureBox.Left + (Me.ShieldPictureBox.Width \ 2) - (Me.SensorMessage.Width \ 2), Me.SensorMessage.Top)
        If s_lastSG("sg") <> "0" Then
            Me.CurrentBG.Visible = True
            Me.CurrentBG.Location = New Point((Me.ShieldPictureBox.Width \ 2) - (Me.CurrentBG.Width \ 2), Me.ShieldPictureBox.Height \ 4)
            Me.CurrentBG.Parent = Me.ShieldPictureBox
            Me.CurrentBG.Text = s_lastSG("sg")
            Me.UpdateNotifyIcon()
            _bgMiniDisplay.SetCurrentBGString(s_lastSG("sg"))
            Me.SensorMessage.Visible = False
            Me.ShieldPictureBox.Image = My.Resources.Shield
            Me.ShieldUnitsLabel.Visible = True
            Me.ShieldUnitsLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.Parent = Me.ShieldPictureBox
            Me.ShieldUnitsLabel.Left = (Me.ShieldPictureBox.Width \ 2) - (Me.ShieldUnitsLabel.Width \ 2)
            Me.ShieldUnitsLabel.Text = BgUnitsString
            Me.ShieldUnitsLabel.Visible = True
        Else
            _bgMiniDisplay.SetCurrentBGString("---")
            Me.CurrentBG.Visible = False
            Me.ShieldPictureBox.Image = My.Resources.Shield_Disabled
            Me.SensorMessage.Visible = True
            Me.SensorMessage.Parent = Me.ShieldPictureBox
            Me.SensorMessage.Left = 0
            Me.SensorMessage.BackColor = Color.Transparent
            Dim message As String = ""
            If s_sensorMessages.TryGetValue(s_sensorState, message) Then
                message = s_sensorState.ToTitle
            Else
                MsgBox($"{s_sensorState} is unknown sensor message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
            End If
            Me.SensorMessage.Text = message
            Me.ShieldUnitsLabel.Visible = False
            Me.SensorMessage.Visible = True
        End If
        If _bgMiniDisplay.Visible Then
            _bgMiniDisplay.BGTextBox.SelectionLength = 0
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        If s_timeToNextCalibHours = Byte.MaxValue Then
            Me.CalibrationDueImage.Image = My.Resources.CalibrationUnavailable
        ElseIf s_timeToNextCalibHours < 1 Then
            Me.CalibrationDueImage.Image = If(s_systemStatusMessage = "WAIT_TO_CALIBRATE" OrElse s_sensorState = "WARM_UP",
            My.Resources.CalibrationNotReady,
            My.Resources.CalibrationDotRed.DrawCenteredArc(s_timeToNextCalibHours, s_timeToNextCalibHours / 12))
        Else
            Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(s_timeToNextCalibHours, s_timeToNextCalibHours / 12)
        End If

        Application.DoEvents()
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

    Private Sub UpdateRemainingInsulin()
        Me.RemainingInsulinUnits.Text = $"{s_reservoirRemainingUnits:N1} U"
    End Sub

    Private Sub UpdateSensorLife()
        If s_sensorDurationHours = 255 Then
            Me.SensorDaysLeftLabel.Text = $"???"
            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
            Me.SensorTimeLeftLabel.Text = ""
        ElseIf s_sensorDurationHours >= 24 Then
            Me.SensorDaysLeftLabel.Text = CStr(Math.Ceiling(s_sensorDurationHours / 24))
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
        Me.AverageSGValueLabel.Text = If(BgUnitsString = "mg/dl", s_averageSG.ToString, s_averageSG.RoundDouble(1).ToString())

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

    Private Sub UpdateZHomeTabSerieses()
        Dim timeChangeMarkers As New List(Of Dictionary(Of String, String))
        For Each s As Series In Me.HomeTabChart.Series
            s.Points.Clear()
        Next

        For Each markerWithIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Dim markerDateTime As Date = s_markers.SafeGetSgDateTime(markerWithIndex.Index)
            Dim markerOaDateTime As Double = markerDateTime.ToOADate()
            Dim bgValueString As String = ""
            Dim bgValue As Single
            If markerWithIndex.Value.TryGetValue("value", bgValueString) Then
                Single.TryParse(bgValueString, NumberStyles.Number, CurrentDataCulture, bgValue)
            End If
            With Me.HomeTabChart.Series(NameOf(HomeTabMarkerSeries)).Points
                Select Case markerWithIndex.Value("type")
                    Case "BG_READING"
                        Single.TryParse(markerWithIndex.Value("value"), NumberStyles.Number, CurrentDataCulture, bgValue)
                        .AddXY(markerOaDateTime, bgValue)
                        .Last.BorderColor = Color.Gainsboro
                        .Last.Color = Color.Transparent
                        .Last.MarkerBorderWidth = 2
                        .Last.MarkerSize = 10
                        .Last.ToolTip = $"Blood Glucose: Not used For calibration: {bgValue.ToString(CurrentUICulture)} {BgUnitsString}"
                    Case "CALIBRATION"
                        .AddXY(markerOaDateTime, bgValue)
                        .Last.BorderColor = Color.Red
                        .Last.Color = Color.Transparent
                        .Last.MarkerBorderWidth = 2
                        .Last.MarkerSize = 8
                        .Last.ToolTip = $"Blood Glucose: Calibration {If(CBool(markerWithIndex.Value("calibrationSuccess")), "accepted", "not accepted")}: {markerWithIndex.Value("value")} {BgUnitsString}"
                    Case "INSULIN"
                        s_markerInsulinDictionary.Add(markerOaDateTime, CInt(MarkerRow))
                        .AddXY(markerOaDateTime, MarkerRow)
                        Dim result As Single
                        Single.TryParse(markerWithIndex.Value("deliveredFastAmount"), NumberStyles.Number, CurrentDataCulture, result)
                        Select Case markerWithIndex.Value("activationType")
                            Case "AUTOCORRECTION"
                                .Last.Color = Color.FromArgb(60, Color.MediumPurple)
                                .Last.ToolTip = $"Auto Correction: {result.ToString(CurrentUICulture)} U"
                            Case "RECOMMENDED", "UNDETERMINED"
                                .Last.Color = Color.FromArgb(30, Color.LightBlue)
                                .Last.ToolTip = $"Bolus: {result.ToString(CurrentUICulture)} U"
                            Case Else
                                Stop
                        End Select
                        .Last.MarkerBorderWidth = 0
                        .Last.MarkerSize = 30
                        .Last.MarkerStyle = MarkerStyle.Square
                    Case "MEAL"
                        s_markerMealDictionary.Add(markerOaDateTime, InsulinRow)
                        .AddXY(markerOaDateTime, InsulinRow)
                        .Last.Color = Color.FromArgb(30, Color.Yellow)
                        .Last.MarkerBorderWidth = 0
                        .Last.MarkerSize = 30
                        .Last.MarkerStyle = MarkerStyle.Square
                        Dim result As Single
                        Single.TryParse(markerWithIndex.Value("amount"), NumberStyles.Number, CurrentDataCulture, result)
                        .Last.ToolTip = $"Meal:{result.ToString(CurrentUICulture)} grams"
                    Case "AUTO_BASAL_DELIVERY"
                        .AddXY(markerOaDateTime, MarkerRow)
                        Dim bolusAmount As String = markerWithIndex.Value("bolusAmount")
                        .Last.MarkerBorderColor = Color.Black
                        .Last.ToolTip = $"Basal:{bolusAmount.RoundDouble(3).ToString(CurrentUICulture)} U"
                    Case "AUTO_MODE_STATUS", "LOW_GLUCOSE_SUSPENDED"
                    Case "TIME_CHANGE"
                        timeChangeMarkers.Add(markerWithIndex.Value)

                    Case Else
                        Stop
                End Select
            End With
        Next
        Dim limitsIndexList() As Integer = GetLimitsList(s_sGs.Count - 1)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_sGs.WithIndex()
            Dim sgOaDateTime As Double = sgListIndex.Value.OADate()
            Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)).PlotOnePoint(sgOaDateTime, sgListIndex.Value.sg, Color.White)
            Dim limitsLowValue As Single = CSng(s_limits(limitsIndexList(sgListIndex.Index))("lowLimit"))
            Dim limitsHighValue As Single = CSng(s_limits(limitsIndexList(sgListIndex.Index))("highLimit"))
            If limitsHighValue <> 0 Then
                Me.HomeTabChart.Series(NameOf(HomeTabHighLimitSeries)).Points.AddXY(sgOaDateTime, limitsHighValue)
            End If
            If limitsLowValue <> 0 Then
                Me.HomeTabChart.Series(NameOf(HomeTabLowLimitSeries)).Points.AddXY(sgOaDateTime, limitsLowValue)
            End If
        Next
    End Sub

#End Region

#End Region

    Private Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean) As Boolean
        Me.ServerUpdateTimer.Stop()
        Debug.Print($"Me.ServerUpdateTimer stopped at {Now}")
        If Me.MenuOptionsUseTestData.Checked Then
            Me.MenuView.Visible = False
            Me.Text = $"{SavedTitle} Using Test Data"
            CurrentDateCulture = New CultureInfo("en-US")
            RecentData = Loads(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")))
        ElseIf Me.MenuOptionsUseLastSavedData.Checked Then
            Me.MenuView.Visible = False
            Me.Text = $"{SavedTitle} Using Last Saved Data"
            CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(RepoDownloadName)
            RecentData = Loads(File.ReadAllText(LastDownloadWithPath))
        Else
            Me.Text = SavedTitle
            _loginDialog.ShowDialog()
            _client = _loginDialog.Client
            If _client Is Nothing OrElse Not _client.LoggedIn Then
                Return False
            End If
            RecentData = _client.GetRecentData()
            Me.MenuView.Visible = True
            Me.ServerUpdateTimer.Interval = s_minuteInMilliseconds
            Me.ServerUpdateTimer.Start()
            Debug.Print($"Me.ServerUpdateTimer Started at {Now}")
            Me.LoginStatus.Text = "OK"
        End If
        If Not _initialized Then
            Me.FinishInitialization()
        End If
        If UpdateAllTabs Then
            Me.UpdateAllTabPages()
        End If
        Return True
    End Function

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
    ' ScaleControl() is called during the Form's constructor
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        Me.FormScale = New SizeF(Me.FormScale.Width * factor.Width, Me.FormScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
    End Sub

#End Region

#Region "NotifyIcon Support"

    Private Sub CleanUpNotificationIcon()
        Me.NotifyIcon1.Visible = False
        Me.NotifyIcon1.Icon.Dispose()
        Me.NotifyIcon1.Icon = Nothing
        Me.NotifyIcon1.Visible = False
        Me.NotifyIcon1.Dispose()
        Application.DoEvents()
        End
    End Sub

    Private Sub UpdateNotifyIcon()
        Dim str As String = s_lastSG("sg")
        Dim fontToUse As New Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim color As Color = Color.White
        Dim bgColor As Color
        Dim sg As Double = str.ParseDouble
        Dim bitmapText As New Bitmap(16, 16)
        Dim g As Graphics = System.Drawing.Graphics.FromImage(bitmapText)
        Dim notStr As New StringBuilder
        Dim diffsg As Double

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
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
        If Math.Floor(Math.Log10(sg) + 1) = 3 Then
            g.DrawString(str, fontToUse, brushToUse, -2, 0)
        Else
            g.DrawString(str, fontToUse, brushToUse, 1.5, 0)
        End If
        Dim hIcon As IntPtr = bitmapText.GetHicon()
        Me.NotifyIcon1.Icon = Icon.FromHandle(hIcon)
        notStr.Append(Date.Now().ToString)
        notStr.Append(Environment.NewLine)
        notStr.Append($"Last SG {str} {BgUnitsString}")
        If Not s_lastBGValue = 0 Then
            notStr.Append(Environment.NewLine)
            diffsg = sg - s_lastBGValue
            notStr.Append("SG Trend ")
            notStr.Append(diffsg.ToString("+0;-#"))
        End If
        notStr.Append(Environment.NewLine)
        notStr.Append("Active ins. ")
        notStr.Append(s_activeInsulin("amount"))
        notStr.Append("U"c)
        Me.NotifyIcon1.Text = notStr.ToString
        s_lastBGValue = sg
        bitmapText.Dispose()
        g.Dispose()
    End Sub

#End Region

End Class
