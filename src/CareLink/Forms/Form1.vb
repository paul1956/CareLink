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

    Private Const MilitaryTimeWithMinuteFormat As String = "HH:mm"
    Private Const TwelveHourTimeWithMinuteFormat As String = "h:mm tt"
    Private ReadOnly _bgMiniDisplay As New BGMiniWindow
    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _insulinImage As Bitmap = My.Resources.InsulinVial_Tiny
    Private ReadOnly _loginDialog As New LoginForm1
    Private ReadOnly _markerInsulinDictionary As New Dictionary(Of Double, Single)
    Private ReadOnly _markerMealDictionary As New Dictionary(Of Double, Single)
    Private ReadOnly _mealImage As Bitmap = My.Resources.MealImage
    Private ReadOnly _thirtySecondInMilliseconds As Integer = CInt(New TimeSpan(0, 0, seconds:=30).TotalMilliseconds)

    Private _activeInsulinIncrements As Integer
    Private _client As CareLinkClient
    Private _filterJsonData As Boolean = True
    Private _initialized As Boolean = False
    Private _inMouseMove As Boolean = False
    Private _limitHigh As Single
    Private _limitLow As Single
    Private _recentDatalast As Dictionary(Of String, String)
    Private _recentDataSameCount As Integer
    Private _timeFormat As String
    Private _updating As Boolean = False
    Private _showBaloonTip As Boolean = False
    Private _lastBGValue As Double = 0

    Private Property FormScale As New SizeF(1.0F, 1.0F)
    Private ReadOnly Property SensorLifeToolTip As New ToolTip()
    Friend Property BgUnitsString As String
    Public Property RecentData As Dictionary(Of String, String)

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
    Private WithEvents HomeTabTimeInRangeSeries As New Series

#End Region

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As Title

#End Region

    Private _homePageAbsoluteRectangle As RectangleF
    Private _homePageChartRelitivePosition As RectangleF = RectangleF.Empty
    Private _inMenuOptions As Boolean
    Private _insulinRow As Single
    Private _markerRow As Single

    Private Property InsulinRow As Single
        Get
            If _insulinRow = 0 Then
                Throw New ArgumentNullException(NameOf(_insulinRow))
            End If
            Return _insulinRow
        End Get
        Set
            _insulinRow = Value
        End Set
    End Property

    Private Property MarkerRow As Single
        Get
            If _markerRow = 0 Then
                Throw New ArgumentNullException(NameOf(_markerRow))
            End If
            Return _markerRow
        End Get
        Set
            _markerRow = Value
        End Set
    End Property

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
        If Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=False) Then
            Me.FinishInitialization()
            Me.UpdateAllTabPages()
        End If
    End Sub

    ' Save the current scale value
    ' ScaleControl() is called during the Form's constructor
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        Me.FormScale = New SizeF(Me.FormScale.Width * factor.Width, Me.FormScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
    End Sub

#End Region

#Region "Form Menu Events"

#Region "Start Here Menus"

    Private Sub MenuStartHere_DropDownOpened(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpened
        Me.MenuStartHereLoadSavedDataFile.Enabled = Directory.GetFiles(MyDocumentsPath, $"{RepoName}*.json").Length > 0
        Me.MenuStartHereSnapshotSave.Enabled = _RecentData IsNot Nothing
        Me.MenuStartHereExceptionReportLoadToolStripMenuItem.Visible = Path.Combine(MyDocumentsPath, $"{RepoErrorReportName}*.txt").Length > 0
    End Sub

    Private Sub MenuStartHereExceptionReportLoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuStartHereExceptionReportLoadToolStripMenuItem.Click
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
                    Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
                    Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
                    ExceptionHandlerForm.ReportFileNameWithPath = fileNameWithPath
                    If ExceptionHandlerForm.ShowDialog() = DialogResult.OK Then
                        ExceptionHandlerForm.ReportFileNameWithPath = ""
                        Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                        Me.RecentData = Loads(ExceptionHandlerForm.LocalRawData)
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

        If openFileDialog1.ShowDialog() = Global.System.Windows.Forms.DialogResult.OK Then
            Try
                If File.Exists(openFileDialog1.FileName) Then
                    Me.ServerUpdateTimer.Stop()
                    Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
                    Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
                    CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"{RepoName}", True)

                    _RecentData = Loads(File.ReadAllText(openFileDialog1.FileName))
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
        Using jd As JsonDocument = JsonDocument.Parse(_RecentData.CleanUserData(), New JsonDocumentOptions)
            File.WriteAllText(GetDataFileName(RepoSnapshotName, CurrentDateCulture.Name, "json", True).withPath, JsonSerializer.Serialize(jd, JsonFormattingOptions))
        End Using
    End Sub

#End Region

#Region "Option Menus"

    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        _filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked
    End Sub

    Private Sub MenuOptionsSetupEmailServer_Click(sender As Object, e As EventArgs) Handles MenuOptionsSetupEmailServer.Click
        MailSetupDialog.ShowDialog()
    End Sub

    Private Sub MenuOptionsUseAdvancedAITDecay_CheckStateChanged(sender As Object, e As EventArgs) Handles MenuOptionsUseAdvancedAITDecay.CheckStateChanged
        Dim increments As Double = TimeSpan.Parse(My.Settings.AIT.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan
        If Me.MenuOptionsUseAdvancedAITDecay.Checked Then
            _activeInsulinIncrements = CInt(increments * 1.4)
            My.Settings.UseAdvancedAITDecay = True
            Me.AITLabel.Text = "Advanced AIT Decay"
        Else
            _activeInsulinIncrements = CInt(increments)
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
        AboutBox1.Show()
    End Sub

    Private Sub MenuHelpCheckForUpdates_Click(sender As Object, e As EventArgs) Handles MenuHelpCheckForUpdatesMenuItem.Click
        CheckForUpdatesAsync(Me, reportResults:=True)
    End Sub

    Private Sub MenuHelpReportIssueMenuItem_Click(sender As Object, e As EventArgs) Handles MenuHelpReportAProblem.Click
        OpenUrlInBrowser($"{GitHubCareLinkUrl}issues")
    End Sub

#End Region

#Region "HomePage Tab Events"

    Private Sub AITComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AITComboBox.SelectedIndexChanged
        Dim aitTimeSpan As TimeSpan = TimeSpan.Parse(Me.AITComboBox.SelectedItem.ToString())
        My.Settings.AIT = aitTimeSpan
        My.Settings.Save()
        _activeInsulinIncrements = CInt(TimeSpan.Parse(aitTimeSpan.ToString("hh\:mm").Substring(1)) / s_fiveMinuteSpan)
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

        Me.CursorTimer.Interval = _thirtySecondInMilliseconds
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
                            Me.CursorTimeLabel.Text = xValue.ToString(_timeFormat)
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
                            Me.CursorTimeLabel.Text = Date.FromOADate(result.Series.Points(result.PointIndex).XValue).ToString(_timeFormat)
                            Me.CursorTimeLabel.Visible = True
                            Me.CursorMessage1Label.Text = $"{result.Series.Points(result.PointIndex).YValues(0).RoundDouble(3)} {Me.BgUnitsString}"
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

    <System.Diagnostics.DebuggerNonUserCode()>
    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomeTabChart.PostPaint
        If Not _initialized OrElse _updating OrElse _inMouseMove Then
            Exit Sub
        End If
        If _homePageChartRelitivePosition.IsEmpty Then
            _homePageChartRelitivePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.X, s_sGs(0).OADate))
            _homePageChartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, _markerRow))
            _homePageChartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, _limitHigh)))) - _homePageChartRelitivePosition.Y
            _homePageChartRelitivePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.X, s_sGs.Last.OADate)) - _homePageChartRelitivePosition.X
            _homePageChartRelitivePosition = e.ChartGraphics.GetAbsoluteRectangle(_homePageChartRelitivePosition)
        End If

        Dim homePageChartY As Integer = CInt(_homePageChartRelitivePosition.Y)
        Dim homePageChartWidth As Integer = CInt(_homePageChartRelitivePosition.Width)
        Dim highLimitY As Double = e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, _limitHigh)
        Dim lowLimitY As Double = e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.Y, _limitLow)

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            Dim highHeight As Integer = CInt(255 * Me.FormScale.Height)
            Dim homePagelocation As New Point(CInt(_homePageChartRelitivePosition.X), homePageChartY)
            Dim highAreaRectangle As New Rectangle(homePagelocation,
                                                   New Size(homePageChartWidth, highHeight))
            e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)
        End Using

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
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
        End Using
        If Me.CursorTimeLabel.Tag IsNot Nothing Then
            Me.CursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis(NameOf(HomeTabChartArea), AxisName.X, Me.CursorTimeLabel.Tag.ToString.DateParse.ToOADate))
        End If

        e.PaintMarker(_mealImage, _markerMealDictionary, 0)
        e.PaintMarker(_insulinImage, _markerInsulinDictionary, -6)
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
                Dim dateValue As Date = e.Value.ToString.DateParse
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
            If .Name <> NameOf(SgRecord.RecordNumber) Then
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End If
        End With
    End Sub

    Private Sub SGsDataGridView_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles SGsDataGridView.ColumnHeaderMouseClick
        Dim currentSortOrder As SortOrder = Me.SGsDataGridView.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection
        If Me.SGsDataGridView.Columns(e.ColumnIndex).Name = NameOf(SgRecord.RecordNumber) Then
            If currentSortOrder = SortOrder.None OrElse currentSortOrder = SortOrder.Ascending Then
                Me.SGsDataGridView.DataSource = s_sGs.OrderByDescending(Function(x) x.RecordNumber).ToList
                currentSortOrder = SortOrder.Descending
            Else
                Me.SGsDataGridView.DataSource = s_sGs.OrderBy(Function(x) x.RecordNumber).ToList
                currentSortOrder = SortOrder.Ascending
            End If
        End If
        Me.SGsDataGridView.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = currentSortOrder
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
        _RecentData = _client.GetRecentData()
        If Me.IsRecentDataUpdated Then
            Me.UpdateAllTabPages()
        ElseIf _RecentData Is Nothing Then
            _client = New CareLinkClient(Me.LoginStatus, My.Settings.CareLinkUserName, My.Settings.CareLinkPassword, My.Settings.CountryCode)
            _loginDialog.Client = _client
            _RecentData = _client.GetRecentData()
            If Me.IsRecentDataUpdated Then
                Me.UpdateAllTabPages()
            End If
        End If
        Application.DoEvents()
        Me.ServerUpdateTimer.Interval = CType(New TimeSpan(0, minutes:=1, 0).TotalMilliseconds, Integer)
        Me.ServerUpdateTimer.Start()
        Debug.Print($"Me.ServerUpdateTimer Started at {Now}")
        Me.Cursor = Cursors.Default
    End Sub

#End Region ' Timer

#End Region ' Events

#Region "Initialize Charts"

    Private Sub InitializeActiveInsulinTabChart()
        Me.ActiveInsulinChart = New Chart With {
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
            .BackColor = Color.WhiteSmoke,
            .BackGradientStyle = GradientStyle.TopBottom,
            .BackSecondaryColor = Color.White,
            .BorderlineColor = Color.FromArgb(26, 59, 105),
            .BorderlineDashStyle = ChartDashStyle.Solid,
            .BorderlineWidth = 2,
            .Dock = DockStyle.Fill,
            .Name = NameOf(ActiveInsulinChart),
            .TabIndex = 0
        }

        Me.ActiveInsulinChartArea = New ChartArea With {
            .BackColor = Color.FromArgb(180, 23, 47, 19),
            .BackGradientStyle = GradientStyle.TopBottom,
            .BackSecondaryColor = Color.FromArgb(180, 29, 56, 26),
            .BorderColor = Color.FromArgb(64, 64, 64, 64),
            .BorderDashStyle = ChartDashStyle.Solid,
            .Name = NameOf(ActiveInsulinChartArea),
            .ShadowColor = Color.Transparent
        }

        With Me.ActiveInsulinChartArea
            With .AxisX
                .Interval = 2
                .IntervalType = DateTimeIntervalType.Hours
                .IsInterlaced = True
                .IsMarginVisible = True
                .LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
                .ScaleView.Zoomable = True
                With .ScrollBar
                    .BackColor = Color.White
                    .ButtonColor = Color.Lime
                    .IsPositionedInside = True
                    .LineColor = Color.Black
                    .Size = 15
                End With
            End With
            With .AxisY
                .InterlacedColor = Color.FromArgb(120, Color.LightSlateGray)
                .Interval = 2
                .IntervalAutoMode = IntervalAutoMode.FixedCount
                .IsInterlaced = True
                .IsLabelAutoFit = False
                .IsMarginVisible = False
                .IsStartedFromZero = True
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorTickMark = New TickMark() With {.Interval = Me.InsulinRow, .Enabled = False}
                .Maximum = 25
                .Minimum = 0
                .ScaleView.Zoomable = False
                .Title = "Active Insulin"
                .TitleForeColor = Color.HotPink
            End With
            With .AxisY2
                .Maximum = Me.MarkerRow
                .Minimum = 0
                .Title = "BG Value"
            End With
            With .CursorX
                .AutoScroll = True
                .AxisType = AxisType.Primary
                .Interval = 0
                .IsUserEnabled = True
                .IsUserSelectionEnabled = True
            End With
            With .CursorY
                .AutoScroll = False
                .AxisType = AxisType.Secondary
                .Interval = 0
                .IsUserEnabled = False
                .IsUserSelectionEnabled = False
                .LineColor = Color.Transparent
            End With
        End With

        Me.ActiveInsulinChart.ChartAreas.Add(Me.ActiveInsulinChartArea)

        Me.ActiveInsulinChartLegend = New Legend With {
            .BackColor = Color.Transparent,
            .Enabled = False,
            .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
            .IsTextAutoFit = False,
            .Name = NameOf(ActiveInsulinChartLegend)
        }
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
        Me.ActiveInsulinCurrentBGSeries = New Series With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = NameOf(ActiveInsulinChartArea),
            .ChartType = SeriesChartType.Line,
            .Color = Color.Blue,
            .Legend = NameOf(ActiveInsulinChartLegend),
            .Name = NameOf(ActiveInsulinCurrentBGSeries),
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Secondary
        }
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

    Private Sub InitializeHomePageChart()
        Me.HomeTabChart = New Chart With {
             .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
             .BackColor = Color.WhiteSmoke,
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.White,
             .BorderlineColor = Color.FromArgb(26, 59, 105),
             .BorderlineDashStyle = ChartDashStyle.Solid,
             .BorderlineWidth = 2,
             .Dock = DockStyle.Fill,
             .Name = NameOf(HomeTabChart),
             .TabIndex = 0
         }

        Me.HomeTabChartArea = New ChartArea With {
             .BackColor = Color.FromArgb(180, 23, 47, 19),
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.FromArgb(180, 29, 56, 26),
             .BorderColor = Color.FromArgb(64, 64, 64, 64),
             .BorderDashStyle = ChartDashStyle.Solid,
             .Name = NameOf(HomeTabChartArea),
             .ShadowColor = Color.Transparent
         }
        With Me.HomeTabChartArea
            .AxisX.Interval = 2
            .AxisX.IntervalType = DateTimeIntervalType.Hours
            .AxisX.IsInterlaced = True
            .AxisX.IsMarginVisible = True
            .AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
            .AxisX.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .AxisX.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisX.ScaleView.Zoomable = True
            .AxisX.ScrollBar.BackColor = Color.White
            .AxisX.ScrollBar.ButtonColor = Color.Lime
            .AxisX.ScrollBar.IsPositionedInside = True
            .AxisX.ScrollBar.LineColor = Color.Black
            .AxisX.ScrollBar.Size = 15
            .AxisY.InterlacedColor = Color.FromArgb(120, Color.LightSlateGray)
            .AxisY.Interval = Me.InsulinRow
            .AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount
            .AxisY.IsInterlaced = True
            .AxisY.IsLabelAutoFit = False
            .AxisY.IsMarginVisible = False
            .AxisY.IsStartedFromZero = False
            .AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .AxisY.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisY.MajorTickMark = New TickMark() With {.Interval = Me.InsulinRow, .Enabled = False}
            .AxisY.Maximum = Me.MarkerRow
            .AxisY.Minimum = Me.InsulinRow
            .AxisY.ScaleBreakStyle = New AxisScaleBreakStyle() With {
                .Enabled = True,
                .StartFromZero = StartFromZero.No,
                .BreakLineStyle = BreakLineStyle.Straight
                }
            .AxisY.ScaleView.Zoomable = False
            .AxisY2.Interval = Me.InsulinRow
            .AxisY2.IsMarginVisible = False
            .AxisY2.IsStartedFromZero = False
            .AxisY2.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .AxisY2.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisY2.MajorGrid = New Grid With {
                .Interval = Me.InsulinRow,
                .LineColor = Color.FromArgb(64, 64, 64, 64)
            }
            .AxisY2.MajorTickMark = New TickMark() With {.Interval = Me.InsulinRow, .Enabled = True}
            .AxisY2.Maximum = Me.MarkerRow
            .AxisY2.Minimum = Me.InsulinRow
            .AxisY2.ScaleView.Zoomable = False
            .CursorX.AutoScroll = True
            .CursorX.AxisType = AxisType.Primary
            .CursorX.Interval = 0
            .CursorX.IsUserEnabled = True
            .CursorX.IsUserSelectionEnabled = True
            .CursorY.AutoScroll = False
            .CursorY.AxisType = AxisType.Secondary
            .CursorY.Interval = 0
            .CursorY.IsUserEnabled = False
            .CursorY.IsUserSelectionEnabled = False
            .CursorY.LineColor = Color.Transparent
        End With

        Me.HomeTabChart.ChartAreas.Add(Me.HomeTabChartArea)

        Dim defaultLegend As New Legend With {
                .BackColor = Color.Transparent,
                .Enabled = False,
                .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
                .IsTextAutoFit = False,
                .Name = NameOf(defaultLegend)
            }
        Me.HomeTabCurrentBGSeries = New Series With {
                .BorderColor = Color.FromArgb(180, 26, 59, 105),
                .BorderWidth = 4,
                .ChartArea = NameOf(HomeTabChartArea),
                .ChartType = SeriesChartType.Line,
                .Color = Color.White,
                .Legend = NameOf(defaultLegend),
                .Name = NameOf(HomeTabCurrentBGSeries),
                .ShadowColor = Color.Black,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }
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

        Me.SplitContainer3.Panel1.Controls.Add(Me.HomeTabChart)
        Application.DoEvents()
        Me.HomeTabChart.Series.Add(Me.HomeTabCurrentBGSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabMarkerSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabHighLimitSeries)
        Me.HomeTabChart.Series.Add(Me.HomeTabLowLimitSeries)
        Me.HomeTabChart.Legends.Add(defaultLegend)
        Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)).EmptyPointStyle.BorderWidth = 4
        Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)).EmptyPointStyle.Color = Color.Transparent
        Application.DoEvents()
    End Sub

    Private Sub InitializeTimeInRangeArea()
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

#Region "Update Data/Tables"

    Private Shared Sub GetLimitsList(ByRef limitsIndexList As Integer())

        Dim limitsIndex As Integer = 0
        For i As Integer = 0 To limitsIndexList.GetUpperBound(0)
            If limitsIndex + 1 < s_limits.Count AndAlso CInt(s_limits(limitsIndex + 1)("index")) < i Then
                limitsIndex += 1
            End If
            limitsIndexList(i) = limitsIndex
        Next
    End Sub

    Private Sub FillOneRowOfTableLayoutPanel(layoutPanel As TableLayoutPanel, innerJson As List(Of Dictionary(Of String, String)), rowIndex As ItemIndexs, filterJsonData As Boolean, timeFormat As String)
        For Each jsonEntry As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
            Dim innerTableBlue As New TableLayoutPanel With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .AutoScroll = False,
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .ColumnCount = 2,
                    .Dock = DockStyle.Fill,
                    .Margin = New Padding(0),
                    .Name = NameOf(innerTableBlue),
                    .Padding = New Padding(0)
                }
            layoutPanel.Controls.Add(innerTableBlue, column:=1, row:=jsonEntry.Index)
            GetInnerTable(jsonEntry.Value, innerTableBlue, rowIndex, filterJsonData, timeFormat, Me.FormScale.Height <> 1)
            Application.DoEvents()
        Next
    End Sub

    Private Function IsRecentDataUpdated() As Boolean
        If _recentDatalast Is Nothing OrElse _RecentData Is Nothing Then
            Return False
        End If
        If _recentDataSameCount < 5 Then
            _recentDataSameCount += 1
            Dim i As Integer
            For i = 0 To _RecentData.Keys.Count - 1
                If _recentDatalast.Keys(i) <> "currentServerTime" AndAlso _recentDatalast.Values(i) <> _RecentData.Values(i) Then
                    _recentDataSameCount = 0
                    Return True
                End If
            Next
            Return False
        End If
        _recentDataSameCount = 0
        Return True
    End Function

    Private Sub UpdateActiveInsulinChart()
        If Not _initialized Then
            Exit Sub
        End If

        With Me.ActiveInsulinChart
            .Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Active Insulin in Pink"
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.Minimum = s_sGs(0).OADate()
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.Maximum = s_sGs.Last.OADate()
            .Series(NameOf(ActiveInsulinSeries)).Points.Clear()
            .Series(NameOf(ActiveInsulinCurrentBGSeries)).Points.Clear()
            .Series(NameOf(ActiveInsulinMarkerSeries)).Points.Clear()
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Hours
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.MajorGrid.Interval = 1
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.IntervalType = DateTimeIntervalType.Hours
            .ChartAreas(NameOf(ActiveInsulinChartArea)).AxisX.Interval = 2
        End With

        ' Order all markers by time
        Dim timeOrderedMarkers As New SortedDictionary(Of Double, Double)
        Dim sgOaDateTime As Double

        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            sgOaDateTime = s_markers.SafeGetSgDateTime(sgListIndex.Index).RoundTimeDown(RoundTo.Minute).ToOADate
            Select Case sgListIndex.Value("type")
                Case "INSULIN"
                    Dim bolusAmount As Double = sgListIndex.Value.GetDoubleValue("deliveredFastAmount")
                    If timeOrderedMarkers.ContainsKey(sgOaDateTime) Then
                        timeOrderedMarkers(sgOaDateTime) += bolusAmount
                    Else
                        timeOrderedMarkers.Add(sgOaDateTime, bolusAmount)
                    End If
                Case "AUTO_BASAL_DELIVERY"
                    Dim bolusAmount As Double = sgListIndex.Value.GetDoubleValue("bolusAmount")
                    If timeOrderedMarkers.ContainsKey(sgOaDateTime) Then
                        timeOrderedMarkers(sgOaDateTime) += bolusAmount
                    Else
                        timeOrderedMarkers.Add(sgOaDateTime, bolusAmount)
                    End If
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
            remainingInsulinList.Add(New Insulin(oaTime, initialBolus, _activeInsulinIncrements, Me.MenuOptionsUseAdvancedAITDecay.Checked))
        Next

        Me.ActiveInsulinChartArea.AxisY2.Maximum = Me.MarkerRow

        ' walk all markers, adjust active insulin and then add new marker
        Dim maxActiveInsulin As Double = 0
        For i As Integer = 0 To remainingInsulinList.Count - 1
            If i < _activeInsulinIncrements Then
                Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaTime, Double.NaN)
                Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.Last.IsEmpty = True
                If i > 0 Then
                    remainingInsulinList.Adjustlist(0, i)
                End If
                Continue For
            End If
            Dim startIndex As Integer = i - _activeInsulinIncrements + 1
            Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, _activeInsulinIncrements)
            maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
            Dim x As Integer = Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinSeries)).Points.AddXY(remainingInsulinList(i).OaTime, sum)
            remainingInsulinList.Adjustlist(startIndex, _activeInsulinIncrements)
            Application.DoEvents()
        Next
        Me.ActiveInsulinChartArea.AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
        maxActiveInsulin = Me.ActiveInsulinChartArea.AxisY.Maximum

        s_totalAutoCorrection = 0
        s_totalBasal = 0
        s_totalCarbs = 0
        s_totalDailyDose = 0
        s_totalManualBolus = 0

        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            sgOaDateTime = s_markers.SafeGetSgDateTime(sgListIndex.Index).RoundTimeDown(RoundTo.Minute).ToOADate
            With Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinMarkerSeries))
                Select Case sgListIndex.Value("type")
                    Case "INSULIN"
                        .Points.AddXY(sgOaDateTime, maxActiveInsulin)
                        Dim deliveredAmount As Single = sgListIndex.Value("deliveredFastAmount").ParseSingle
                        s_totalDailyDose += deliveredAmount
                        Select Case sgListIndex.Value("activationType")
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
                        Dim bolusAmount As Double = sgListIndex.Value.GetDoubleValue("bolusAmount")
                        .Points.AddXY(sgOaDateTime, maxActiveInsulin)
                        .Points.Last.ToolTip = $"Basal: {bolusAmount.RoundDouble(3).ToString(CurrentUICulture)} U"
                        .Points.Last.MarkerSize = 8
                        s_totalBasal += CSng(bolusAmount)
                        s_totalDailyDose += CSng(bolusAmount)
                    Case "MEAL"
                        s_totalCarbs += sgListIndex.Value.GetDoubleValue("amount")
                End Select
            End With
        Next
        For Each sgListIndex As IndexClass(Of SgRecord) In s_sGs.WithIndex()
            Dim bgValue As Single = sgListIndex.Value.sg

            Me.ActiveInsulinChart.Series(NameOf(ActiveInsulinCurrentBGSeries)).PlotOnePoint(
                sgListIndex.Value.OADate(),
                sgListIndex.Value.sg,
                Color.Black,
                Me.InsulinRow,
                _limitHigh,
                _limitLow
                )
        Next
        _initialized = True
        Application.DoEvents()
    End Sub

    Private Sub UpdateDataTables(isScaledForm As Boolean)
        If _RecentData Is Nothing Then
            Exit Sub
        End If
        _updating = True
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        Me.TableLayoutPanelSummaryData.Controls.Clear()
        Dim rowCount As Integer = Me.TableLayoutPanelSummaryData.RowCount
        Dim newRowCount As Integer = _RecentData.Count - 9
        If rowCount < newRowCount Then
            Me.TableLayoutPanelSummaryData.RowCount = newRowCount
            For i As Integer = rowCount To newRowCount
                Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 22.0!))
            Next
        End If

        Dim currentRowIndex As Integer = 0
        Dim singleItem As Boolean
        Dim layoutPanel1 As TableLayoutPanel

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In _RecentData.WithIndex()
            layoutPanel1 = Me.TableLayoutPanelSummaryData
            singleItem = False
            Dim row As KeyValuePair(Of String, String) = c.Value
            Dim rowIndex As ItemIndexs = CType([Enum].Parse(GetType(ItemIndexs), c.Value.Key), ItemIndexs)
            Dim singleItemIndex As ItemIndexs

            Select Case rowIndex
                Case ItemIndexs.lastSensorTS
                    s_lastSensorTS = row.Value
                Case ItemIndexs.medicalDeviceTimeAsString
                    s_medicalDeviceTimeAsString = row.Value
                Case ItemIndexs.lastSensorTSAsString
                    s_lastSensorTSAsString = row.Value
                Case ItemIndexs.kind
                    s_kind = row.Value
                Case ItemIndexs.version
                    s_version = row.Value
                Case ItemIndexs.pumpModelNumber
                    s_pumpModelNumber = row.Value
                    Me.ModelLabel.Text = s_pumpModelNumber
                Case ItemIndexs.currentServerTime
                    s_currentServerTime = row.Value
                Case ItemIndexs.lastConduitTime
                    s_lastConduitTime = row.Value
                Case ItemIndexs.lastConduitUpdateServerTime
                    s_lastConduitUpdateServerTime = row.Value
                Case ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                    s_lastMedicalDeviceDataUpdateServerTime = row.Value
                Case ItemIndexs.firstName
                    s_firstName = row.Value
                Case ItemIndexs.lastName
                    s_lastName = row.Value
                    Me.FullNameLabel.Text = $"{s_firstName} {s_lastName}"
                Case ItemIndexs.conduitSerialNumber
                    s_conduitSerialNumber = row.Value
                Case ItemIndexs.conduitBatteryLevel
                    s_conduitBatteryLevel = CInt(row.Value)
                Case ItemIndexs.conduitBatteryStatus
                    s_conduitBatteryStatus = row.Value
                Case ItemIndexs.conduitInRange
                    s_conduitInRange = CBool(row.Value)
                Case ItemIndexs.conduitMedicalDeviceInRange
                    s_conduitMedicalDeviceInRange = CBool(row.Value)
                Case ItemIndexs.conduitSensorInRange
                    s_conduitSensorInRange = CBool(row.Value)
                Case ItemIndexs.medicalDeviceFamily
                    s_medicalDeviceFamily = row.Value
                Case ItemIndexs.sensorState
                    s_sensorState = row.Value
                Case ItemIndexs.medicalDeviceSerialNumber
                    s_medicalDeviceSerialNumber = row.Value
                    Me.SerialNumberLabel.Text = s_medicalDeviceSerialNumber
                Case ItemIndexs.medicalDeviceTime
                    s_medicalDeviceTime = row.Value
                Case ItemIndexs.sMedicalDeviceTime
                    s_sMedicalDeviceTime = row.Value.DateParse
                Case ItemIndexs.reservoirLevelPercent
                    s_reservoirLevelPercent = CInt(row.Value)
                Case ItemIndexs.reservoirAmount
                    s_reservoirAmount = row.Value.ParseDouble
                Case ItemIndexs.reservoirRemainingUnits
                    s_reservoirRemainingUnits = row.Value.ParseDouble
                Case ItemIndexs.medicalDeviceBatteryLevelPercent
                    s_medicalDeviceBatteryLevelPercent = CInt(row.Value)
                Case ItemIndexs.sensorDurationHours
                    s_sensorDurationHours = CInt(row.Value)
                Case ItemIndexs.timeToNextCalibHours
                    s_timeToNextCalibHours = CUShort(row.Value)
                Case ItemIndexs.calibStatus
                    s_calibStatus = row.Value
                Case ItemIndexs.bgUnits
                    s_bgUnits = row.Value
                    Me.BgUnitsString = GetLocalizedUnits(s_bgUnits)

                    If Me.BgUnitsString = "mg/dl" Then
                        S_criticalLow = 50
                        _limitHigh = 180
                        _limitLow = 70
                        _markerRow = 400
                        Me.HomeTabChartArea.AxisX.LabelStyle.Format = "hh tt"
                        Me.ActiveInsulinChartArea.AxisX.LabelStyle.Format = "hh tt"
                    Else
                        S_criticalLow = 2.7
                        _limitHigh = 10.0
                        _limitLow = (70 / 18).RoundSingle(1)
                        _markerRow = (400 / 18).RoundSingle(1)
                        Me.ActiveInsulinChartArea.AxisX.LabelStyle.Format = "HH"
                    End If
                    Me.AboveHighLimitMessageLabel.Text = $"Above {_limitHigh} {Me.BgUnitsString}"
                    Me.BelowLowLimitMessageLabel.Text = $"Below {_limitLow} {Me.BgUnitsString}"
                Case ItemIndexs.timeFormat
                    s_timeFormat = row.Value
                    _timeFormat = If(s_timeFormat = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                Case ItemIndexs.lastSensorTime
                    s_lastSensorTime = row.Value
                Case ItemIndexs.sLastSensorTime
                    s_sLastSensorTime = row.Value.DateParse
                Case ItemIndexs.medicalDeviceSuspended
                    s_medicalDeviceSuspended = CBool(row.Value)
                Case ItemIndexs.lastSGTrend
                    s_lastSGTrend = row.Value
                Case ItemIndexs.lastSG
                    layoutPanel1 = Me.TableLayoutPanelTop1
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = ItemIndexs.lastSG
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.lastAlarm
                    layoutPanel1 = Me.TableLayoutPanelTop2
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = ItemIndexs.lastAlarm
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.activeInsulin
                    layoutPanel1 = Me.TableLayoutPanelActiveInsulin
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = ItemIndexs.activeInsulin
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.sgs
                    s_sGs = LoadList(row.Value, True).ToSgList()
                    Me.SGsDataGridView.DataSource = s_sGs
                    For Each column As DataGridViewTextBoxColumn In Me.SGsDataGridView.Columns
                        If _filterJsonData AndAlso s_alwaysFilter.Contains(column.Name) Then
                            Me.SGsDataGridView.Columns(column.Name).Visible = False
                        End If
                    Next
                    Me.ReadingsLabel.Text = $"{s_sGs.Where(Function(entry As SgRecord) Not Double.IsNaN(entry.sg)).Count}/288"
                    Continue For
                Case ItemIndexs.limits
                    layoutPanel1 = Me.TableLayoutPanelLimits
                    layoutPanel1.Controls.Clear()
                    layoutPanel1.AutoSize = True
                    singleItemIndex = ItemIndexs.limits
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.markers
                    layoutPanel1 = Me.TableLayoutPanelMarkers
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = ItemIndexs.markers
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.notificationHistory
                    layoutPanel1 = Me.TableLayoutPanelNotificationHistory
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = ItemIndexs.notificationHistory
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.therapyAlgorithmState
                    ' handled elsewhere
                Case ItemIndexs.pumpBannerState
                    ' handled elsewhere
                Case ItemIndexs.basal
                    layoutPanel1 = Me.TableLayoutPanelBasal
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = ItemIndexs.basal
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.systemStatusMessage
                    s_systemStatusMessage = row.Value
                Case ItemIndexs.averageSG
                    s_averageSG = CInt(row.Value)
                Case ItemIndexs.belowHypoLimit
                    s_belowHypoLimit = CInt(row.Value)
                Case ItemIndexs.aboveHyperLimit
                    s_aboveHyperLimit = CInt(row.Value)
                Case ItemIndexs.timeInRange
                    s_timeInRange = CInt(row.Value)
                Case ItemIndexs.pumpCommunicationState
                    s_pumpCommunicationState = CBool(row.Value)
                Case ItemIndexs.gstCommunicationState
                    s_gstCommunicationState = CBool(row.Value)
                Case ItemIndexs.gstBatteryLevel
                    s_gstBatteryLevel = CInt(row.Value)
                Case ItemIndexs.lastConduitDateTime
                    s_lastConduitDateTime = row.Value
                Case ItemIndexs.maxAutoBasalRate
                    s_maxAutoBasalRate = row.Value.ParseDouble
                Case ItemIndexs.maxBolusAmount
                    s_maxBolusAmount = row.Value.ParseDouble
                Case ItemIndexs.sensorDurationMinutes
                    s_sensorDurationMinutes = CInt(row.Value)
                Case ItemIndexs.timeToNextCalibrationMinutes
                    s_timeToNextCalibrationMinutes = CInt(row.Value)
                Case ItemIndexs.clientTimeZoneName
                    s_clientTimeZoneName = row.Value
                Case ItemIndexs.sgBelowLimit
                    s_sgBelowLimit = CInt(row.Value)
                Case ItemIndexs.averageSGFloat
                    s_averageSGFloat = row.Value.ParseDouble
                Case ItemIndexs.timeToNextCalibrationRecommendedMinutes
                    s_timeToNextCalibrationRecommendedMinutes = CUShort(row.Value)
                Case ItemIndexs.calFreeSensor
                    s_calFreeSensor = CBool(row.Value)
                Case ItemIndexs.finalCalibration
                    s_finalCalibration = CBool(row.Value)
                Case Else
                    Stop
                    Exit Select
            End Select

            Try
                If s_listOfSingleItems.Contains(rowIndex) OrElse singleItem Then
                    If Not (singleItem AndAlso singleItemIndex = rowIndex) Then
                        Continue For
                    End If
                End If
                Dim tableRelitiveRow As Integer
                If singleItem Then
                    tableRelitiveRow = 0
                Else
                    tableRelitiveRow = currentRowIndex
                    currentRowIndex += 1
                End If
                layoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
                If Not singleItem OrElse rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm Then
                    Dim columnHeaderLabel As New Label With {
                            .Text = $"{CInt(rowIndex)} {row.Key}",
                            .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                            .AutoSize = True
                        }
                    layoutPanel1.Controls.Add(columnHeaderLabel, 0, tableRelitiveRow)
                End If
                If row.Value?.StartsWith("[") Then
                    Dim innerJson As List(Of Dictionary(Of String, String)) = LoadList(row.Value, False)
                    Select Case rowIndex
                        Case ItemIndexs.limits
                            s_limits = innerJson
                        Case ItemIndexs.markers
                            s_markers = innerJson
                        Case ItemIndexs.notificationHistory
                        ' handled elsewhere
                        Case ItemIndexs.pumpBannerState
                            s_pumpBannerState = innerJson
                        Case Else
                            Stop
                    End Select
                    If innerJson.Count > 0 Then
                        layoutPanel1.Parent.Parent.UseWaitCursor = True
                        Application.DoEvents()
                        layoutPanel1.Invoke(Sub()
                                                Me.FillOneRowOfTableLayoutPanel(layoutPanel1,
                                                                              innerJson,
                                                                              rowIndex,
                                                                              _filterJsonData,
                                                                              _timeFormat)
                                            End Sub)
                        Application.DoEvents()

                        layoutPanel1.Parent.Parent.UseWaitCursor = False
                        Application.DoEvents()
                    Else
                        Dim rowTextBox As New TextBox With {
                                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                .AutoSize = True,
                                .ReadOnly = True,
                                .Text = ""
                            }
                        layoutPanel1.Controls.Add(rowTextBox,
                                                  If(singleItem, 0, 1),
                                                  tableRelitiveRow)

                    End If
                ElseIf row.Value?.StartsWith("{") Then
                    layoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
                    Dim innerJson As Dictionary(Of String, String) = Loads(row.Value)
                    Select Case rowIndex
                        Case ItemIndexs.lastSG
                            s_lastSG = innerJson
                        Case ItemIndexs.lastAlarm
                            s_lastAlarm = innerJson
                        Case ItemIndexs.activeInsulin
                            s_activeInsulin = innerJson
                        Case ItemIndexs.notificationHistory
                        ' handled elsewhere
                        Case ItemIndexs.therapyAlgorithmState
                            s_therapyAlgorithmState = innerJson
                        Case ItemIndexs.basal
                            s_basal = innerJson
                        Case Else
                            Stop
                    End Select
                    Dim innerTableBlue As New TableLayoutPanel With {
                            .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                            .AutoScroll = True,
                            .AutoSize = True,
                            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            .ColumnCount = 2,
                            .Dock = DockStyle.Fill,
                            .Margin = New Padding(0),
                            .Name = NameOf(innerTableBlue),
                            .Padding = New Padding(0)
                        }
                    layoutPanel1.Controls.Add(innerTableBlue,
                                              If(singleItem AndAlso Not (rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm), 0, 1),
                                              tableRelitiveRow)
                    If rowIndex = ItemIndexs.notificationHistory Then
                        innerTableBlue.AutoScroll = False
                    End If
                    GetInnerTable(innerJson, innerTableBlue, rowIndex, _filterJsonData, _timeFormat, isScaledForm)
                Else
                    Dim rowTextBox As New TextBox With {
                        .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                        .AutoSize = True,
                        .ReadOnly = True,
                        .Text = row.Value
                    }
                    layoutPanel1.Controls.Add(rowTextBox,
                                              If(singleItem, 0, 1),
                                              tableRelitiveRow)
                End If
            Catch ex As Exception
                Stop
                'Throw
            End Try
        Next
        If _RecentData.Count > ItemIndexs.finalCalibration + 1 Then
            Stop
        End If
        _initialized = True
        _updating = False
        Me.Cursor = Cursors.Default
    End Sub

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

    Private Sub UpdateRegionalData(localRecentData As Dictionary(Of String, String))
        _updating = True
        If localRecentData Is Nothing Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        If localRecentData.TryGetValue(ItemIndexs.bgUnits.ToString, s_bgUnits) Then
            Me.BgUnitsString = GetLocalizedUnits(s_bgUnits)
            If Me.BgUnitsString = "mg/dl" Then
                _markerRow = 400
                _limitHigh = 180
                _limitLow = 70
                _insulinRow = 50
            Else
                _markerRow = (400 / 18).RoundSingle(1)
                _limitHigh = (180 / 18).RoundSingle(1)
                _limitLow = (70 / 18).RoundSingle(1)
                _insulinRow = (50 / 18).RoundSingle(1)
            End If
        End If
        Dim internaltimeFormat As String = Nothing
        If localRecentData.TryGetValue(ItemIndexs.timeFormat.ToString, internaltimeFormat) Then
            _timeFormat = If(internaltimeFormat = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
        End If
        _updating = False
        Me.Cursor = Cursors.Default
        Application.DoEvents()
    End Sub

    Friend Sub UpdateAllTabPages()
        If _RecentData Is Nothing OrElse _updating Then
            Exit Sub
        End If
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
        _recentDatalast = _RecentData
        _initialized = True
        _updating = False
        Application.DoEvents()
    End Sub

#Region "Home Page Update Utilities"

    Private Sub UpdateActiveInsulin()
        Dim activeInsulinStr As String = $"{s_activeInsulin("amount"):N3}"
        Me.ActiveInsulinValue.Text = $"{activeInsulinStr} U"
        _bgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr}U"
    End Sub

    Private Sub UpdateNotifyIcon()
        Dim str As String = s_lastSG("sg")
        Dim fontToUse As Font = New Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim color As Color = Color.White
        Dim bgColor As Color
        Dim sg As Double = str.ParseDouble
        Dim bitmapText As New Bitmap(16, 16)
        Dim g As Graphics = System.Drawing.Graphics.FromImage(bitmapText)
        Dim notStr As New StringBuilder
        Dim diffsg As Double

        Select Case sg
            Case <= _limitLow
                bgColor = Color.Orange
                If _showBaloonTip Then
                    Me.NotifyIcon1.ShowBalloonTip(10000, "Carelink Alert", "SG below " + _limitLow.ToString + " " + Me.BgUnitsString, Me.ToolTip1.ToolTipIcon)
                End If
                _showBaloonTip = False
            Case <= _limitHigh
                bgColor = Color.Green
                _showBaloonTip = True
            Case Else
                bgColor = Color.Red
                If _showBaloonTip Then
                    Me.NotifyIcon1.ShowBalloonTip(10000, "Carelink Alert", "SG above " + _limitHigh.ToString + " " + Me.BgUnitsString, Me.ToolTip1.ToolTipIcon)
                End If
                _showBaloonTip = False
        End Select
        Dim brushToUse As Brush = New SolidBrush(color)
        g.Clear(bgColor)
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
        g.DrawString(str, fontToUse, brushToUse, -2, 0)
        Dim hIcon As IntPtr = (bitmapText.GetHicon())
        Me.NotifyIcon1.Icon = System.Drawing.Icon.FromHandle(hIcon)
        notStr.Append(Date.Now().ToString)
        notStr.Append(Environment.NewLine)
        notStr.Append("Last SG " + str + " " + Me.BgUnitsString)
        If Not _lastBGValue = 0 Then
            notStr.Append(Environment.NewLine)
            diffsg = sg - _lastBGValue
            notStr.Append("SG Trend ")
            notStr.Append(diffsg.ToString("+0;-#"))
        End If
        notStr.Append(Environment.NewLine)
        notStr.Append("Active ins. ")
        notStr.Append(s_activeInsulin("amount"))
        notStr.Append("U")
        Me.NotifyIcon1.Text = notStr.ToString
        _lastBGValue = sg
        bitmapText.Dispose()
        g.Dispose()
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
            Me.ShieldUnitsLabel.Text = Me.BgUnitsString
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
            If s_messages.TryGetValue(s_sensorState, message) Then
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
        If s_reservoirLevelPercent = 0 Then
            Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(0)
            Exit Sub
        End If
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
            Case > 66
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryFull
                Me.PumpBatteryRemainingLabel.Text = $"High"
            Case >= 45
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryMedium
                Me.PumpBatteryRemainingLabel.Text = $"Medium"
            Case > 25
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryLow
                Me.PumpBatteryRemainingLabel.Text = $"Low"
            Case = 0
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
                .AddXY($"{s_aboveHyperLimit}% Above {_limitHigh} {Me.BgUnitsString}", s_aboveHyperLimit / 100)
                .Last().Color = Color.Orange
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_belowHypoLimit}% Below {_limitLow} {Me.BgUnitsString}", s_belowHypoLimit / 100)
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
        Me.AverageSGMessageLabel.Text = $"Average SG in {Me.BgUnitsString}"
        Me.AverageSGValueLabel.Text = If(Me.BgUnitsString = "mg/dl", s_averageSG.ToString, s_averageSG.RoundDouble(1).ToString())

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
        Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)).Points.Clear()
        Me.HomeTabChart.Series(NameOf(HomeTabMarkerSeries)).Points.Clear()
        Me.HomeTabChart.Series(NameOf(HomeTabHighLimitSeries)).Points.Clear()
        Me.HomeTabChart.Series(NameOf(HomeTabLowLimitSeries)).Points.Clear()
        _markerInsulinDictionary.Clear()
        _markerMealDictionary.Clear()
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Dim sgOaDateTime As Double = s_markers.SafeGetSgDateTime(sgListIndex.Index).ToOADate()
            Dim bgValueString As String = ""
            Dim bgValue As Single
            If sgListIndex.Value.TryGetValue("value", bgValueString) Then
                Single.TryParse(bgValueString, NumberStyles.Number, CurrentDataCulture, bgValue)
            End If
            With Me.HomeTabChart.Series(NameOf(HomeTabMarkerSeries)).Points
                Select Case sgListIndex.Value("type")
                    Case "BG_READING"
                        Single.TryParse(sgListIndex.Value("value"), NumberStyles.Number, CurrentDataCulture, bgValue)
                        .AddXY(sgOaDateTime, bgValue)
                        .Last.BorderColor = Color.Gainsboro
                        .Last.Color = Color.Transparent
                        .Last.MarkerBorderWidth = 2
                        .Last.MarkerSize = 10
                        .Last.ToolTip = $"Blood Glucose: Not used For calibration: {bgValue.ToString(CurrentUICulture)} {Me.BgUnitsString}"
                    Case "CALIBRATION"
                        .AddXY(sgOaDateTime, bgValue)
                        .Last.BorderColor = Color.Red
                        .Last.Color = Color.Transparent
                        .Last.MarkerBorderWidth = 2
                        .Last.MarkerSize = 8
                        .Last.ToolTip = $"Blood Glucose: Calibration {If(CBool(sgListIndex.Value("calibrationSuccess")), "accepted", "not accepted")}: {sgListIndex.Value("value")} {Me.BgUnitsString}"
                    Case "INSULIN"
                        _markerInsulinDictionary.Add(sgOaDateTime, CInt(Me.MarkerRow))
                        .AddXY(sgOaDateTime, Me.MarkerRow)
                        Dim result As Single
                        Single.TryParse(sgListIndex.Value("deliveredFastAmount"), NumberStyles.Number, CurrentDataCulture, result)
                        Select Case sgListIndex.Value("activationType")
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
                        _markerMealDictionary.Add(sgOaDateTime, Me.InsulinRow)
                        .AddXY(sgOaDateTime, Me.InsulinRow)
                        .Last.Color = Color.FromArgb(30, Color.Yellow)
                        .Last.MarkerBorderWidth = 0
                        .Last.MarkerSize = 30
                        .Last.MarkerStyle = MarkerStyle.Square
                        Dim result As Single
                        Single.TryParse(sgListIndex.Value("amount"), NumberStyles.Number, CurrentDataCulture, result)
                        .Last.ToolTip = $"Meal:{result.ToString(CurrentUICulture)} grams"
                    Case "AUTO_BASAL_DELIVERY"
                        .AddXY(sgOaDateTime, Me.MarkerRow)
                        Dim bolusAmount As String = sgListIndex.Value("bolusAmount")
                        .Last.MarkerBorderColor = Color.Black
                        .Last.ToolTip = $"Basal:{bolusAmount.RoundDouble(3).ToString(CurrentUICulture)} U"
                    Case "TIME_CHANGE"
                        ' need to handle
                    Case "AUTO_MODE_STATUS", "LOW_GLUCOSE_SUSPENDED"
                        'Stop
                    Case Else
                        Stop
                End Select
            End With
        Next
        Dim limitsIndexList(s_sGs.Count - 1) As Integer
        GetLimitsList(limitsIndexList)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_sGs.WithIndex()
            Dim sgOaDateTime As Double = sgListIndex.Value.OADate()
            PlotOnePoint(Me.HomeTabChart.Series(NameOf(HomeTabCurrentBGSeries)), sgOaDateTime, sgListIndex.Value.sg, Color.White, Me.InsulinRow, _limitHigh, _limitLow)
            Dim limitsLowValue As Integer = CInt(s_limits(limitsIndexList(sgListIndex.Index))("lowLimit"))
            Dim limitsHighValue As Integer = CInt(s_limits(limitsIndexList(sgListIndex.Index))("highLimit"))
            Me.HomeTabChart.Series(NameOf(HomeTabHighLimitSeries)).Points.AddXY(sgOaDateTime, limitsHighValue)
            Me.HomeTabChart.Series(NameOf(HomeTabLowLimitSeries)).Points.AddXY(sgOaDateTime, limitsLowValue)
        Next
    End Sub

#End Region

#End Region

    Private Sub CleanUpNotificationIcon()
        Me.NotifyIcon1.Visible = False
        Me.NotifyIcon1.Icon.Dispose()
        Me.NotifyIcon1.Icon = Nothing
        Me.NotifyIcon1.Visible = False
        Me.NotifyIcon1.Dispose()
        Application.DoEvents()
        End
    End Sub

    Private Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean) As Boolean
        Me.ServerUpdateTimer.Stop()
        Debug.Print($"Me.ServerUpdateTimer stopped at {Now}")
        If Me.MenuOptionsUseTestData.Checked Then
            Me.MenuView.Visible = False
            Me.Text = $"{SavedTitle} Using Test Data"
            CurrentDateCulture = New CultureInfo("en-US")
            _RecentData = Loads(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")))
        ElseIf Me.MenuOptionsUseLastSavedData.Checked Then
            Me.MenuView.Visible = False
            Me.Text = $"{SavedTitle} Using Last Saved Data"
            CurrentDateCulture = LastDownloadWithPath.ExtractCultureFromFileName(RepoDownloadName)
            _RecentData = Loads(File.ReadAllText(LastDownloadWithPath))
        Else
            Me.Text = SavedTitle
            _loginDialog.ShowDialog()
            _client = _loginDialog.Client
            If _client Is Nothing OrElse Not _client.LoggedIn Then
                Return False
            End If
            _RecentData = _client.GetRecentData()
            Me.MenuView.Visible = True
            Me.ServerUpdateTimer.Interval = CType(New TimeSpan(0, minutes:=1, 0).TotalMilliseconds, Integer)
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

    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical, Me.FormScale.Width, Me.FormScale.Height)
        If sp.FixedPanel = FixedPanel.Panel1 Then
            sp.SplitterDistance = CInt(Math.Truncate(Math.Round(sp.SplitterDistance * sc)))
        ElseIf sp.FixedPanel = Global.System.Windows.Forms.FixedPanel.Panel2 Then
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

    Friend Sub FinishInitialization()
        If _initialized Then
            Exit Sub
        End If
        _homePageChartRelitivePosition = RectangleF.Empty
        Me.UpdateRegionalData(_RecentData)

        Me.InitializeHomePageChart()
        Me.InitializeActiveInsulinTabChart()
        Me.InitializeTimeInRangeArea()
        Me.SGsDataGridView.AutoGenerateColumns = True
        Me.SGsDataGridView.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
            .Alignment = DataGridViewContentAlignment.MiddleCenter
            }

        _initialized = True
    End Sub

End Class
