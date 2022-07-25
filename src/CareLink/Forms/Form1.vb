' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
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
    Private ReadOnly _savedTitle As String = Me.Text
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
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
    Private Property formScale As New SizeF(1.0F, 1.0F)
    Friend Property BgUnitsString As String
    Public ReadOnly Property RecentData As Dictionary(Of String, String)

#Region "Chart Objects"

    Public WithEvents ActiveInsulinTabChart As Chart
    Public WithEvents CurrentBGSeries As Series
    Public WithEvents HighLimitSeries As Series
    Public WithEvents HomePageChart As Chart
    Public WithEvents LowLimitSeries As Series
    Public WithEvents MarkerSeries As Series
    Public WithEvents TimeInRangeChart As Chart

    Private _activeInsulinTabChartArea As ChartArea
    Private _homePageAbsoluteRectangle As RectangleF
    Private _homePageChartChartArea As ChartArea
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
        ElseIf My.Settings.UseLastSavedData AndAlso Me.MenuStartHereSnapshotLoad.Enabled Then
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
        If Me.formScale.Height > 1 Then
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
        Me.formScale = New SizeF(Me.formScale.Width * factor.Width, Me.formScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
    End Sub

#End Region

#Region "Form Menu Events"

#Region "Start Here Menus"
    Private Sub MenuStartHere_DropDownOpened(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpened
        Me.MenuStartHereSnapshotLoad.Enabled = Directory.GetFiles(MyDocumentsPath, $"{RepoName}*.json").Length > 0
        Me.MenuStartHereSnapshotSave.Enabled = _RecentData IsNot Nothing
        Me.MenuStartHereExceptionReportLoadToolStripMenuItem.Visible = Path.Combine(MyDocumentsPath, $"{ErrorReportName}*.txt").Length > 0
    End Sub

    Private Sub MenuStartHereExceptionReportLoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuStartHereExceptionReportLoadToolStripMenuItem.Click
        Dim fileList As String() = Directory.GetFiles(MyDocumentsPath, $"{ErrorReportName}*.txt")
        Dim openFileDialog1 As New OpenFileDialog With {
            .CheckFileExists = True,
            .CheckPathExists = True,
            .FileName = If(fileList.Length > 0, Path.GetFileName(fileList(0)), RepoName),
            .Filter = $"Error files (*.txt)|{ErrorReportName}*.txt",
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
                If File.Exists(fileNameWithPath) Then
                    Me.ServerUpdateTimer.Stop()
                    Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
                    Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
                    Dim partialFileNameWithoutPath As String = Path.GetFileNameWithoutExtension(fileNameWithPath).Replace($"{ErrorReportName}(", "")
                    Dim indexOfClosedParen As Integer = partialFileNameWithoutPath.IndexOf(")"c)
                    CurrentUICulture = CultureInfo.GetCultureInfo(partialFileNameWithoutPath.Substring(0, indexOfClosedParen))
                    Dim errorFileData As String = File.ReadAllText(fileNameWithPath)
                    Dim indexOfStackTraceTerminatingString As Integer = errorFileData.IndexOf(StackTraceTerminatingString) + StackTraceTerminatingString.Length
                    _RecentData = Loads(errorFileData.Substring(indexOfStackTraceTerminatingString))
                    Me.FinishInitialization()
                    Me.Text = $"{_savedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                    Me.UpdateAllTabPages()
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.Message}")
            End Try
        End If

    End Sub
    Private Sub MenuStartHereExit_Click(sender As Object, e As EventArgs) Handles StartHereExit.Click
        Me.CleanUpNotificationIcon()
    End Sub

    Private Sub MenuStartHereLogin_Click(sender As Object, e As EventArgs) Handles MenuStartHereLogin.Click
        Me.MenuOptionsUseTestData.CheckState = CheckState.Indeterminate
        Me.MenuOptionsUseLastSavedData.CheckState = CheckState.Indeterminate
        Me.DoOptionalLoginAndUpdateData(UpdateAllTabs:=True)
    End Sub

    Private Sub MenuStartHereSnapshotLoad_Click(sender As Object, e As EventArgs) Handles MenuStartHereSnapshotLoad.Click
        Dim fileList As String() = Directory.GetFiles(MyDocumentsPath, $"{RepoName}*.json")
        Dim openFileDialog1 As New OpenFileDialog With {
            .CheckFileExists = True,
            .CheckPathExists = True,
            .FileName = If(fileList.Length > 0, Path.GetFileName(fileList(0)), RepoName),
            .Filter = "json files (*.json)|CareLink*.json",
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
                    _RecentData = Loads(File.ReadAllText(openFileDialog1.FileName))
                    Me.FinishInitialization()
                    Me.Text = $"{_savedTitle} Using file {Path.GetFileName(openFileDialog1.FileName)}"
                    Me.UpdateAllTabPages()
                End If
            Catch ex As Exception
                MessageBox.Show($"Cannot read file from disk. Original error: {ex.Message}")
            End Try
        End If
    End Sub

    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) Handles MenuStartHereSnapshotSave.Click
        Using jd As JsonDocument = JsonDocument.Parse(_RecentData.CleanUserData(), New JsonDocumentOptions)
            File.WriteAllText(MyDocumentsCareLinkSnapshotDocPath, JsonSerializer.Serialize(jd, JsonFormattingOptions))
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

    Private Sub HomePageChart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles HomePageChart.CursorPositionChanging
        If Not _initialized Then Exit Sub

        Me.CursorTimer.Interval = _thirtySecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    Private Sub HomePageChart_MouseMove(sender As Object, e As MouseEventArgs) Handles HomePageChart.MouseMove

        If Not _initialized Then
            Exit Sub
        End If
        _inMouseMove = True
        Dim yInPixels As Double = Me.HomePageChart.ChartAreas("Default").AxisY2.ValueToPixelPosition(e.Y)
        If Double.IsNaN(yInPixels) Then
            Exit Sub
        End If
        Dim result As HitTestResult
        Try
            result = Me.HomePageChart.HitTest(e.X, e.Y)
            If result?.PointIndex >= -1 Then
                If result.Series IsNot Nothing Then
                    Me.CursorTimeLabel.Left = e.X - (Me.CursorTimeLabel.Width \ 2)
                    Select Case result.Series.Name
                        Case NameOf(HighLimitSeries), NameOf(LowLimitSeries)
                            Me.CursorMessage1Label.Visible = False
                            Me.CursorMessage2Label.Visible = False
                            Me.CursorPictureBox.Image = Nothing
                            Me.CursorTimeLabel.Visible = False
                            Me.CursorValueLabel.Visible = False
                        Case NameOf(MarkerSeries)
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
                        Case "Default"
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

    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomePageChart.PostPaint
        If Not _initialized OrElse _updating OrElse _inMouseMove Then
            Exit Sub
        End If
        If _homePageChartRelitivePosition.IsEmpty Then
            _homePageChartRelitivePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, s_sGs(0).OADate))
            _homePageChartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, _markerRow))
            _homePageChartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, _limitHigh)))) - _homePageChartRelitivePosition.Y
            _homePageChartRelitivePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, s_sGs.Last.OADate)) - _homePageChartRelitivePosition.X
            _homePageChartRelitivePosition = e.ChartGraphics.GetAbsoluteRectangle(_homePageChartRelitivePosition)
        End If

        Dim homePageChartY As Integer = CInt(_homePageChartRelitivePosition.Y)
        Dim homePageChartWidth As Integer = CInt(_homePageChartRelitivePosition.Width)
        Dim highLimitY As Double = e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, _limitHigh)
        Dim lowLimitY As Double = e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, _limitLow)

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            Dim highHeight As Integer = CInt(255 * Me.formScale.Height)
            Dim homePagelocation As New Point(CInt(_homePageChartRelitivePosition.X), homePageChartY)
            Dim highAreaRectangle As New Rectangle(homePagelocation,
                                                   New Size(homePageChartWidth, highHeight))
            e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)
        End Using

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            Dim lowOffset As Integer = CInt((10 + _homePageChartRelitivePosition.Height) * Me.formScale.Height)
            Dim lowStartLocation As New Point(CInt(_homePageChartRelitivePosition.X), lowOffset)

            Dim lowRawHeight As Integer = CInt((50 - homePageChartY) * Me.formScale.Height)
            Dim lowHeight As Integer = If(_homePageChartChartArea.AxisX.ScrollBar.IsVisible,
                                          CInt(lowRawHeight - _homePageChartChartArea.AxisX.ScrollBar.Size),
                                          lowRawHeight
                                         )
            Dim lowAreaRectangle As New Rectangle(lowStartLocation,
                                                  New Size(homePageChartWidth, lowHeight))
            e.ChartGraphics.Graphics.FillRectangle(b, lowAreaRectangle)
        End Using
        If Me.CursorTimeLabel.Tag IsNot Nothing Then
            Me.CursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, CDate(Me.CursorTimeLabel.Tag).ToOADate))
        End If

        e.PaintMarker(_mealImage, _markerMealDictionary, 0)
        e.PaintMarker(_insulinImage, _markerInsulinDictionary, -6)
    End Sub

    Private Sub SensorAgeLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles SensorDaysLeftLabel.MouseHover
        If s_sensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(Me.CalibrationDueImage, $"Sensor will expire in {s_sensorDurationHours} hours")
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
                Dim dateValue As Date = CDate(e.Value)
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
        If Not _homePageChartChartArea.AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            _homePageChartChartArea.CursorX.Position = Double.NaN
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
        Me.ActiveInsulinTabChart = New Chart With {
             .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
             .BackColor = Color.WhiteSmoke,
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.White,
             .BorderlineColor = Color.FromArgb(26, 59, 105),
             .BorderlineDashStyle = ChartDashStyle.Solid,
             .BorderlineWidth = 2,
             .Dock = DockStyle.Fill,
             .Name = "chart1",
             .TabIndex = 0
         }

        _activeInsulinTabChartArea = New ChartArea With {
             .BackColor = Color.FromArgb(180, 23, 47, 19),
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.FromArgb(180, 29, 56, 26),
             .BorderColor = Color.FromArgb(64, 64, 64, 64),
             .BorderDashStyle = ChartDashStyle.Solid,
             .Name = "Default",
             .ShadowColor = Color.Transparent
         }

        With _activeInsulinTabChartArea
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
            .AxisY.Interval = 2
            .AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount
            .AxisY.IsInterlaced = True
            .AxisY.IsLabelAutoFit = False
            .AxisY.IsMarginVisible = False
            .AxisY.IsStartedFromZero = True
            .AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .AxisY.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
            .AxisY.MajorTickMark = New TickMark() With {.Interval = Me.InsulinRow, .Enabled = False}
            .AxisY.Maximum = 25
            .AxisY.Minimum = 0
            .AxisY.ScaleView.Zoomable = False
            .AxisY.Title = "Active Insulin"
            .AxisY.TitleForeColor = Color.HotPink
            .AxisY2.Maximum = Me.MarkerRow
            .AxisY2.Minimum = 0
            .AxisY2.Title = "BG Value"
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

        Me.ActiveInsulinTabChart.ChartAreas.Add(_activeInsulinTabChartArea)

        Me.ActiveInsulinTabChart.Legends.Add(New Legend With {
                                     .BackColor = Color.Transparent,
                                     .Enabled = False,
                                     .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
                                     .IsTextAutoFit = False,
                                     .Name = "Default"
                                     }
                                  )
        Dim activeInsulinChart As New Series With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = "Default",
            .ChartType = SeriesChartType.Line,
            .Color = Color.HotPink,
            .Legend = "Default",
            .Name = "Default",
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }
        Dim currentBGChart As New Series With {
            .BorderColor = Color.FromArgb(180, 26, 59, 105),
            .BorderWidth = 4,
            .ChartArea = "Default",
            .ChartType = SeriesChartType.Line,
            .Color = Color.Blue,
            .Legend = "Default",
            .Name = NameOf(CurrentBGSeries),
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Secondary
        }
        Dim markerChart As New Series With {
            .BorderColor = Color.Transparent,
            .BorderWidth = 1,
            .ChartArea = "Default",
            .ChartType = SeriesChartType.Point,
            .Color = Color.HotPink,
            .Name = NameOf(MarkerSeries),
            .MarkerSize = 8,
            .MarkerStyle = MarkerStyle.Circle,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Primary
        }

        Me.ActiveInsulinTabChart.Series.Add(activeInsulinChart)
        Me.ActiveInsulinTabChart.Series.Add(currentBGChart)
        Me.ActiveInsulinTabChart.Series.Add(markerChart)

        Me.ActiveInsulinTabChart.Series("Default").EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinTabChart.Series("Default").EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinTabChart.Titles.Add(New Title With {
                                    .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                                    .ForeColor = Color.FromArgb(26, 59, 105),
                                    .Name = "Title1",
                                    .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                                    .ShadowOffset = 3
                                    }
                                 )
        Me.TabPage2RunningActiveInsulin.Controls.Add(Me.ActiveInsulinTabChart)
        Application.DoEvents()

    End Sub

    Private Sub InitializeHomePageChart()
        Me.HomePageChart = New Chart With {
             .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
             .BackColor = Color.WhiteSmoke,
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.White,
             .BorderlineColor = Color.FromArgb(26, 59, 105),
             .BorderlineDashStyle = ChartDashStyle.Solid,
             .BorderlineWidth = 2,
             .Dock = DockStyle.Fill,
             .Name = "chart1",
             .TabIndex = 0
         }

        _homePageChartChartArea = New ChartArea With {
             .BackColor = Color.FromArgb(180, 23, 47, 19),
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.FromArgb(180, 29, 56, 26),
             .BorderColor = Color.FromArgb(64, 64, 64, 64),
             .BorderDashStyle = ChartDashStyle.Solid,
             .Name = "Default",
             .ShadowColor = Color.Transparent
         }
        With _homePageChartChartArea
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

        Me.HomePageChart.ChartAreas.Add(_homePageChartChartArea)

        Dim defaultLegend As New Legend With {
                .BackColor = Color.Transparent,
                .Enabled = False,
                .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
                .IsTextAutoFit = False,
                .Name = "Default"
            }
        Me.CurrentBGSeries = New Series With {
                .BorderColor = Color.FromArgb(180, 26, 59, 105),
                .BorderWidth = 4,
                .ChartArea = "Default",
                .ChartType = SeriesChartType.Line,
                .Color = Color.White,
                .Legend = "Default",
                .Name = NameOf(CurrentBGSeries),
                .ShadowColor = Color.Black,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }
        Me.MarkerSeries = New Series With {
                .BorderColor = Color.Transparent,
                .BorderWidth = 1,
                .ChartArea = "Default",
                .ChartType = SeriesChartType.Point,
                .Color = Color.HotPink,
                .Name = NameOf(MarkerSeries),
                .MarkerSize = 12,
                .MarkerStyle = MarkerStyle.Circle,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }

        Me.HighLimitSeries = New Series With {
                .BorderColor = Color.FromArgb(180, Color.Orange),
                .BorderWidth = 2,
                .ChartArea = "Default",
                .ChartType = SeriesChartType.StepLine,
                .Color = Color.Orange,
                .Name = NameOf(HighLimitSeries),
                .ShadowColor = Color.Black,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }
        Me.LowLimitSeries = New Series With {
                .BorderColor = Color.FromArgb(180, Color.Red),
                .BorderWidth = 2,
                .ChartArea = "Default",
                .ChartType = SeriesChartType.StepLine,
                .Color = Color.Red,
                .Name = NameOf(LowLimitSeries),
                .ShadowColor = Color.Black,
                .XValueType = ChartValueType.DateTime,
                .YAxisType = AxisType.Secondary
            }

        Me.SplitContainer3.Panel1.Controls.Add(Me.HomePageChart)
        Application.DoEvents()
        Me.HomePageChart.Series.Add(Me.CurrentBGSeries)
        Me.HomePageChart.Series.Add(Me.MarkerSeries)
        Me.HomePageChart.Series.Add(Me.HighLimitSeries)
        Me.HomePageChart.Series.Add(Me.LowLimitSeries)
        Me.HomePageChart.Legends.Add(defaultLegend)
        Me.HomePageChart.Series(NameOf(CurrentBGSeries)).EmptyPointStyle.BorderWidth = 4
        Me.HomePageChart.Series(NameOf(CurrentBGSeries)).EmptyPointStyle.Color = Color.Transparent
        Application.DoEvents()
    End Sub

    Private Sub InitializeTimeInRangeArea()
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
            .Size = New Size(width1, width1)
        }

        With Me.TimeInRangeChart
            .BorderSkin.BackSecondaryColor = Color.Transparent
            .BorderSkin.SkinStyle = BorderSkinStyle.None
            Dim timeInRangeChartChartArea As New ChartArea With {
                .Name = "TimeInRangeChartChartArea",
                .BackColor = Color.Black
                }
            .ChartAreas.Add(timeInRangeChartChartArea)
            .Location = New Point(Me.TimeInRangeChartLabel.FindHorizontalMidpoint - (.Width \ 2),
                                  CInt(Me.TimeInRangeChartLabel.FindVerticalMidpoint() - Math.Round(.Height / 2.5)))
            .Name = "Default"
            .Series.Add(New Series With {.ChartArea = "TimeInRangeChartChartArea",
                                              .ChartType = SeriesChartType.Doughnut,
                                              .Name = "Default"
                                             }
                        )
            .Series("Default")("DoughnutRadius") = "17"
        End With

        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeChart)
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
            Dim tableLevel1Blue As New TableLayoutPanel With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .AutoScroll = False,
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .ColumnCount = 2,
                    .Dock = DockStyle.Fill,
                    .Margin = New Padding(0),
                    .Name = "InnerTable",
                    .Padding = New Padding(0)
                }
            layoutPanel.Controls.Add(tableLevel1Blue, column:=1, row:=jsonEntry.Index)
            GetInnerTable(jsonEntry.Value, tableLevel1Blue, rowIndex, filterJsonData, timeFormat, Me.formScale.Height <> 1)
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

        With Me.ActiveInsulinTabChart
            .Titles("Title1").Text = $"Running Active Insulin in Pink"
            .ChartAreas("Default").AxisX.Minimum = s_sGs(0).OADate()
            .ChartAreas("Default").AxisX.Maximum = s_sGs.Last.OADate()
            .Series("Default").Points.Clear()
            .Series(NameOf(CurrentBGSeries)).Points.Clear()
            .Series(NameOf(MarkerSeries)).Points.Clear()
            .ChartAreas("Default").AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Hours
            .ChartAreas("Default").AxisX.MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
            .ChartAreas("Default").AxisX.MajorGrid.Interval = 1
            .ChartAreas("Default").AxisX.IntervalType = DateTimeIntervalType.Hours
            .ChartAreas("Default").AxisX.Interval = 2
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

        _activeInsulinTabChartArea.AxisY2.Maximum = Me.MarkerRow

        ' walk all markers, adjust active insulin and then add new marker
        Dim maxActiveInsulin As Double = 0
        For i As Integer = 0 To remainingInsulinList.Count - 1
            If i < _activeInsulinIncrements Then
                Me.ActiveInsulinTabChart.Series("Default").Points.AddXY(remainingInsulinList(i).OaTime, Double.NaN)
                Me.ActiveInsulinTabChart.Series("Default").Points.Last.IsEmpty = True
                If i > 0 Then
                    remainingInsulinList.Adjustlist(0, i)
                End If
                Continue For
            End If
            Dim startIndex As Integer = i - _activeInsulinIncrements + 1
            Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, _activeInsulinIncrements)
            maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
            Dim x As Integer = Me.ActiveInsulinTabChart.Series("Default").Points.AddXY(remainingInsulinList(i).OaTime, sum)
            remainingInsulinList.Adjustlist(startIndex, _activeInsulinIncrements)
            Application.DoEvents()
        Next
        _activeInsulinTabChartArea.AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
        maxActiveInsulin = _activeInsulinTabChartArea.AxisY.Maximum

        s_totalAutoCorrection = 0
        s_totalBasal = 0
        s_totalCarbs = 0
        s_totalDailyDose = 0
        s_totalManualBolus = 0

        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            sgOaDateTime = s_markers.SafeGetSgDateTime(sgListIndex.Index).RoundTimeDown(RoundTo.Minute).ToOADate
            With Me.ActiveInsulinTabChart.Series(NameOf(MarkerSeries))
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

            Me.ActiveInsulinTabChart.Series(NameOf(CurrentBGSeries)).PlotOnePoint(
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

    Private Sub UpdateAllTabPages()
        If _RecentData Is Nothing OrElse _updating Then
            Exit Sub
        End If
        _updating = True
        Me.UpdateDataTables(_RecentData,
                            Me.formScale.Height <> 1 _
                            OrElse Me.formScale.Width <> 1)
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

    Private Sub UpdateDataTables(localRecentData As Dictionary(Of String, String), isScaledForm As Boolean)
        If localRecentData Is Nothing Then
            Exit Sub
        End If
        _updating = True
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        Me.TableLayoutPanelSummaryData.Controls.Clear()
        Dim rowCount As Integer = Me.TableLayoutPanelSummaryData.RowCount
        Dim newRowCount As Integer = localRecentData.Count - 9
        If rowCount < newRowCount Then
            Me.TableLayoutPanelSummaryData.RowCount = newRowCount
            For i As Integer = rowCount To newRowCount
                Me.TableLayoutPanelSummaryData.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 22.0!))
            Next
        End If

        Dim currentRowIndex As Integer = 0
        Dim singleItem As Boolean
        Dim layoutPanel1 As TableLayoutPanel
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
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
                    s_sMedicalDeviceTime = CDate(row.Value)
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
                        _homePageChartChartArea.AxisX.LabelStyle.Format = "hh tt"
                        _activeInsulinTabChartArea.AxisX.LabelStyle.Format = "hh tt"
                    Else
                        S_criticalLow = 2.7
                        _limitHigh = 10.0
                        _limitLow = (70 / 18).RoundSingle(1)
                        _markerRow = (400 / 18).RoundSingle(1)
                        _activeInsulinTabChartArea.AxisX.LabelStyle.Format = "HH"
                    End If
                    Me.AboveHighLimitMessageLabel.Text = $"Above {_limitHigh} {Me.BgUnitsString}"
                    Me.BelowLowLimitMessageLabel.Text = $"Below {_limitLow} {Me.BgUnitsString}"
                Case ItemIndexs.timeFormat
                    s_timeFormat = row.Value
                    _timeFormat = If(s_timeFormat = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                Case ItemIndexs.lastSensorTime
                    s_lastSensorTime = row.Value
                Case ItemIndexs.sLastSensorTime
                    s_sLastSensorTime = CDate(row.Value)
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
            End Select

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
                    Dim rowTextBox As New TextBox With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
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
                Dim tableLevel1Blue As New TableLayoutPanel With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .AutoScroll = True,
                    .AutoSize = True,
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    .ColumnCount = 2,
                    .Dock = DockStyle.Fill,
                    .Margin = New Padding(0),
                    .Name = "InnerTable",
                    .Padding = New Padding(0)
                    }
                layoutPanel1.Controls.Add(tableLevel1Blue,
                                          If(singleItem AndAlso Not (rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm), 0, 1),
                                          tableRelitiveRow)
                If rowIndex = ItemIndexs.notificationHistory Then
                    tableLevel1Blue.AutoScroll = False
                End If
                GetInnerTable(innerJson, tableLevel1Blue, rowIndex, _filterJsonData, _timeFormat, isScaledForm)
            Else
                Dim rowTextBox As New TextBox With {
                                        .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                        .AutoSize = True,
                                        .ReadOnly = True,
                                        .Text = row.Value}
                layoutPanel1.Controls.Add(rowTextBox,
                                          If(singleItem, 0, 1),
                                          tableRelitiveRow)
            End If
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

#Region "Home Page Update Utilities"

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
            Me.NotifyIcon1.Text = $"{s_lastSG("sg")} {Me.BgUnitsString}"
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
            If _messages.TryGetValue(s_sensorState, message) Then
                message = s_sensorState.Replace("_", " ")
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
        With Me.TimeInRangeChart
            .Series("Default").Points.Clear()
            .Series("Default").Points.AddXY($"{s_aboveHyperLimit}% Above {_limitHigh} {Me.BgUnitsString}", s_aboveHyperLimit / 100)
            .Series("Default").Points.Last().Color = Color.Orange
            .Series("Default").Points.Last().BorderColor = Color.Black
            .Series("Default").Points.Last().BorderWidth = 2
            .Series("Default").Points.AddXY($"{s_belowHypoLimit}% Below {_limitLow} {Me.BgUnitsString}", s_belowHypoLimit / 100)
            .Series("Default").Points.Last().Color = Color.Red
            .Series("Default").Points.Last().BorderColor = Color.Black
            .Series("Default").Points.Last().BorderWidth = 2
            .Series("Default").Points.AddXY($"{s_timeInRange}% In Range", s_timeInRange / 100)
            .Series("Default").Points.Last().Color = Color.LawnGreen
            .Series("Default").Points.Last().BorderColor = Color.Black
            .Series("Default").Points.Last().BorderWidth = 2
            .Series("Default")("PieLabelStyle") = "Disabled"
            .Series("Default")("PieStartAngle") = "270"
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
        Me.HomePageChart.Series(NameOf(CurrentBGSeries)).Points.Clear()
        Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Clear()
        Me.HomePageChart.Series(NameOf(HighLimitSeries)).Points.Clear()
        Me.HomePageChart.Series(NameOf(LowLimitSeries)).Points.Clear()
        _markerInsulinDictionary.Clear()
        _markerMealDictionary.Clear()
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Dim sgOaDateTime As Double = s_markers.SafeGetSgDateTime(sgListIndex.Index).ToOADate()
            Dim bgValueString As String = ""
            Dim bgValue As Single
            If sgListIndex.Value.TryGetValue("value", bgValueString) Then
                Single.TryParse(bgValueString, NumberStyles.Number, CurrentDataCulture, bgValue)
            End If
            With Me.HomePageChart.Series(NameOf(MarkerSeries))
                Select Case sgListIndex.Value("type")
                    Case "BG_READING"
                        Single.TryParse(sgListIndex.Value("value"), NumberStyles.Number, CurrentDataCulture, bgValue)
                        .Points.AddXY(sgOaDateTime, bgValue)
                        .Points.Last.BorderColor = Color.Gainsboro
                        .Points.Last.Color = Color.Transparent
                        .Points.Last.MarkerBorderWidth = 2
                        .Points.Last.MarkerSize = 10
                        .Points.Last.ToolTip = $"Blood Glucose: Not used For calibration: {bgValue.ToString(CurrentUICulture)} {Me.BgUnitsString}"
                    Case "CALIBRATION"
                        .Points.AddXY(sgOaDateTime, bgValue)
                        .Points.Last.BorderColor = Color.Red
                        .Points.Last.Color = Color.Transparent
                        .Points.Last.MarkerBorderWidth = 2
                        .Points.Last.MarkerSize = 8
                        .Points.Last.ToolTip = $"Blood Glucose: Calibration {If(CBool(sgListIndex.Value("calibrationSuccess")), "accepted", "not accepted")}: {sgListIndex.Value("value")} {Me.BgUnitsString}"
                    Case "INSULIN"
                        _markerInsulinDictionary.Add(sgOaDateTime, CInt(Me.MarkerRow))
                        .Points.AddXY(sgOaDateTime, Me.MarkerRow)
                        Dim result As Single
                        Single.TryParse(sgListIndex.Value("deliveredFastAmount"), NumberStyles.Number, CurrentDataCulture, result)
                        Select Case sgListIndex.Value("activationType")
                            Case "AUTOCORRECTION"
                                .Points.Last.Color = Color.FromArgb(60, Color.MediumPurple)
                                .Points.Last.ToolTip = $"Auto Correction: {result.ToString(CurrentUICulture)} U"
                            Case "RECOMMENDED", "UNDETERMINED"
                                .Points.Last.Color = Color.FromArgb(30, Color.LightBlue)
                                .Points.Last.ToolTip = $"Bolus: {result.ToString(CurrentUICulture)} U"
                            Case Else
                                Stop
                        End Select
                        .Points.Last.MarkerBorderWidth = 0
                        .Points.Last.MarkerSize = 30
                        .Points.Last.MarkerStyle = MarkerStyle.Square
                    Case "MEAL"
                        _markerMealDictionary.Add(sgOaDateTime, Me.InsulinRow)
                        .Points.AddXY(sgOaDateTime, Me.InsulinRow)
                        .Points.Last.Color = Color.FromArgb(30, Color.Yellow)
                        .Points.Last.MarkerBorderWidth = 0
                        .Points.Last.MarkerSize = 30
                        .Points.Last.MarkerStyle = MarkerStyle.Square
                        Dim result As Single
                        Single.TryParse(sgListIndex.Value("amount"), NumberStyles.Number, CurrentDataCulture, result)
                        .Points.Last.ToolTip = $"Meal:{result.ToString(CurrentUICulture)} grams"
                    Case "AUTO_BASAL_DELIVERY"
                        .Points.AddXY(sgOaDateTime, Me.MarkerRow)
                        Dim bolusAmount As String = sgListIndex.Value("bolusAmount")
                        .Points.Last.MarkerBorderColor = Color.Black
                        .Points.Last.ToolTip = $"Basal:{bolusAmount.RoundDouble(3).ToString(CurrentUICulture)} U"
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
            PlotOnePoint(Me.HomePageChart.Series(NameOf(CurrentBGSeries)), sgOaDateTime, sgListIndex.Value.sg, Color.White, Me.InsulinRow, _limitHigh, _limitLow)
            Dim limitsLowValue As Integer = CInt(s_limits(limitsIndexList(sgListIndex.Index))("lowLimit"))
            Dim limitsHighValue As Integer = CInt(s_limits(limitsIndexList(sgListIndex.Index))("highLimit"))
            Me.HomePageChart.Series(NameOf(HighLimitSeries)).Points.AddXY(sgOaDateTime, limitsHighValue)
            Me.HomePageChart.Series(NameOf(LowLimitSeries)).Points.AddXY(sgOaDateTime, limitsLowValue)
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
            Me.Text = $"{_savedTitle} Using Test Data"
            _RecentData = Loads(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")))
        ElseIf Me.MenuOptionsUseLastSavedData.Checked Then
            Me.MenuView.Visible = False
            Me.Text = $"{_savedTitle} Using Last Saved Data"
            _RecentData = Loads(File.ReadAllText(CareLinkLastDownloadDocPath))
        Else
            Me.Text = _savedTitle
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

    Private Sub FinishInitialization()
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

    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical, Me.formScale.Width, Me.formScale.Height)
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

End Class
