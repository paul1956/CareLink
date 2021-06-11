' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1

    ' ReSharper disable InconsistentNaming

    Public WithEvents ActiveInsulinPageChart As Chart
    Public WithEvents HighLimitSeries As Series
    Public WithEvents HomePageChart As Chart
    Public WithEvents LowLimitSeries As Series
    Public WithEvents MarkerSeries As Series
    Public WithEvents TimeInRangeChart As Chart
    Private Const InsulinRow As Integer = 50

    ' ReSharper restore InconsistentNaming
    Private Const MarkerRow As Integer = 400

    Private Const MilitaryTimeWithMinuteFormat As String = "HH:mm"
    Private Const TwelveHourTimeWithMinuteFormat As String = "hh:mm tt"

    Private Shared ReadOnly s_alwaysFilter As New List(Of String) From {
        "kind",
        "relativeOffset",
        "version"
        }

    Private Shared ReadOnly s_lastAlarmFilter As New List(Of String) From {
        "code",
        "GUID",
        "instanceId",
        "kind",
        "referenceGUID",
        "relativeOffset",
        "version"
        }

    Private Shared ReadOnly s_markersFilter As New List(Of String) From {
        "id",
        "index",
        "kind",
        "relativeOffset",
        "version"
        }

    Private Shared ReadOnly s_notificationHistoryFilter As New List(Of String) From {
        "faultId",
        "id",
        "index",
        "instanceId",
        "kind",
        "referenceGUID",
        "relativeOffset",
        "version"
        }

    ' do not rename or move up
    Private Shared ReadOnly s_zFilterList As New Dictionary(Of Integer, List(Of String)) From {
        {ItemIndexs.lastAlarm, s_lastAlarmFilter},
        {ItemIndexs.lastSG, s_alwaysFilter},
        {ItemIndexs.markers, s_markersFilter},
        {ItemIndexs.notificationHistory, s_notificationHistoryFilter},
        {ItemIndexs.sgs, s_alwaysFilter}
        }

    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
    Private ReadOnly _insulinImage As Bitmap = My.Resources.InsulinVial_Tiny

    Private ReadOnly _listOfSingleItems As New List(Of Integer) From {
                    ItemIndexs.lastSG,
                    ItemIndexs.lastAlarm,
                    ItemIndexs.activeInsulin,
                    ItemIndexs.sgs,
                    ItemIndexs.limits,
                    ItemIndexs.markers,
                    ItemIndexs.notificationHistory,
                    ItemIndexs.basal}

    Private ReadOnly _loginDialog As New LoginForm1
    Private ReadOnly _markerInsulinDictionary As New Dictionary(Of Double, Integer)
    Private ReadOnly _markerMealDictionary As New Dictionary(Of Double, Integer)
    Private ReadOnly _mealImage As Bitmap = My.Resources.MealImage
    Private _activeInsulinIncrements As Integer
    Private _client As CarelinkClient.CareLinkClient
    Private _imagePosition As RectangleF = RectangleF.Empty
    Private _initialized As Boolean = False
    Private _timeFormat As String
    Private _updating As Boolean = False
    Public ReadOnly _FiveMinutes As New TimeSpan(hours:=0, minutes:=5, seconds:=0)

#Region "Chart Objects"

    Private _activeInsulinPageChartArea As ChartArea
    Private _homePageChartChartArea As ChartArea

#End Region

#Region "Messages"

    Private ReadOnly _messages As New Dictionary(Of String, String) From {
        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Calibrate sensor"},
        {"CALIBRATING", "Calibrating ..."},
        {"CALIBRATION_REQUIRED", "Calibration required"},
        {"NO_ERROR_MESSAGE", ""},
        {"SEARCHING_FOR_SENSOR_SIGNAL", "Searching for sensor signal"},
        {"SENSOR_DISCONNECTED", "Sensor disconnected"},
        {"UNKNOWN", "Unknown"},
        {"WARM_UP", "Sensor warm up..."},
        {"WAIT_TO_CALIBRATE", "Wait To Calibrate..."}
        }

    ' Add additional units here, default
    Private ReadOnly _unitsStrings As New Dictionary(Of String, String) From {
        {"MGDL", "mg/dl"},
        {"MMOLL", "mmol/L"}
        }

    Private _bgUnitsString As String

#End Region

    ' ReSharper disable InconsistentNaming
    Public HighLimit As Single

    Public LowLimit As Single
    Public RecentData As Dictionary(Of String, String)

#Region "Variables to hold Sensor Values"

    Public AboveHyperLimit As Integer
    Public ActiveInsulin As Dictionary(Of String, String)
    Public AverageSG As Double
    Public AverageSGFloat As Double
    Public Basal As Dictionary(Of String, String)
    Public BelowHypoLimit As Integer
    Public BgUnits As String
    Public CalibStatus As String
    Public ClientTimeZoneName As String
    Public ConduitBatteryLevel As Integer
    Public ConduitBatteryStatus As String
    Public ConduitInRange As Boolean
    Public ConduitMedicalDeviceInRange As Boolean
    Public ConduitSensorInRange As Boolean
    Public ConduitSerialNumber As Guid
    Public CurrentServerTime As String
    Public FirstName As String
    Public GstBatteryLevel As Integer
    Public GstCommunicationState As Boolean
    Public Kind As String
    Public LastAlarm As Dictionary(Of String, String)
    Public LastConduitDateTime As String
    Public LastConduitTime As String
    Public LastConduitUpdateServerTime As String
    Public LastMedicalDeviceDataUpdateServerTime As String
    Public LastName As String
    Public LastSensorTime As String
    Public LastSensorTS As String
    Public LastSensorTSAsString As String
    Public LastSG As Dictionary(Of String, String)
    Public LastSGTrend As String
    Public Limits As List(Of Dictionary(Of String, String))
    Public Markers As List(Of Dictionary(Of String, String))
    Public MaxAutoBasalRate As Double
    Public MaxBolusAmount As Double
    Public MedicalDeviceBatteryLevelPercent As Integer
    Public MedicalDeviceFamily As String
    Public MedicalDeviceSerialNumber As String
    Public MedicalDeviceSuspended As Boolean
    Public MedicalDeviceTime As String
    Public MedicalDeviceTimeAsString As String
    Public NotificationHistory As New Dictionary(Of String, List(Of Dictionary(Of String, String)))
    Public PumpBannerState As List(Of Dictionary(Of String, String))
    Public PumpCommunicationState As Boolean
    Public PumpModelNumber As String
    Public ReservoirAmount As Integer
    Public ReservoirLevelPercent As Integer
    Public ReservoirRemainingUnits As Double
    Public SensorDurationHours As Integer
    Public SensorDurationMinutes As Integer
    Public SensorState As String
    Public SgBelowLimit As Integer
    Public SGs As New List(Of Dictionary(Of String, String))
    Public SLastSensorTime As Date
    Public SMedicalDeviceTime As Date
    Public SystemStatusMessage As String
    Public TherapyAlgorithmState As Dictionary(Of String, String)
    Public TimeFormat As String
    Public TimeInRange As Integer
    Public TimeToNextCalibHours As Byte = Byte.MaxValue
    Public TimeToNextCalibrationMinutes As Integer
    Public Version As String

#End Region

    ' Do not rename these name are matched used in case sensitive matching
    Public Enum ItemIndexs As Integer
        lastSensorTS = 0
        medicalDeviceTimeAsString = 1
        lastSensorTSAsString = 2
        kind = 3
        version = 4
        pumpModelNumber = 5
        currentServerTime = 6
        lastConduitTime = 7
        lastConduitUpdateServerTime = 8
        lastMedicalDeviceDataUpdateServerTime = 9
        firstName = 10
        lastName = 11
        conduitSerialNumber = 12
        conduitBatteryLevel = 13
        conduitBatteryStatus = 14
        conduitInRange = 15
        conduitMedicalDeviceInRange = 16
        conduitSensorInRange = 17
        medicalDeviceFamily = 18
        sensorState = 19
        medicalDeviceSerialNumber = 20
        medicalDeviceTime = 21
        sMedicalDeviceTime = 22
        reservoirLevelPercent = 23
        reservoirAmount = 24
        reservoirRemainingUnits = 25
        medicalDeviceBatteryLevelPercent = 26
        sensorDurationHours = 27
        timeToNextCalibHours = 28
        calibStatus = 29
        bgUnits = 30
        timeFormat = 31
        lastSensorTime = 32
        sLastSensorTime = 33
        medicalDeviceSuspended = 34
        lastSGTrend = 35
        lastSG = 36
        lastAlarm = 37
        activeInsulin = 38
        sgs = 39
        limits = 40
        markers = 41
        notificationHistory = 42
        therapyAlgorithmState = 43
        pumpBannerState = 44
        basal = 45
        systemStatusMessage = 46
        averageSG = 47
        belowHypoLimit = 48
        aboveHyperLimit = 49
        timeInRange = InsulinRow
        pumpCommunicationState = 51
        gstCommunicationState = 52
        gstBatteryLevel = 53
        lastConduitDateTime = 54
        maxAutoBasalRate = 55
        maxBolusAmount = 56
        sensorDurationMinutes = 57
        timeToNextCalibrationMinutes = 58
        clientTimeZoneName = 59
        sgBelowLimit = 60
        averageSGFloat = 61
    End Enum

    ' ReSharper restore InconsistentNaming

#Region "Events"

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If TimeToNextCalibrationMinutes > 0 AndAlso TimeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {Now.AddMinutes(TimeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub
    Private Sub SensorAgeLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles SensorDaysLeftLabel.MouseHover
        If SensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(Me.CalibrationDueImage, $"Sensor will expire in {SensorDurationHours} hours")
        End If
    End Sub

    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not _homePageChartChartArea.AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            _homePageChartChartArea.CursorX.Position = Double.NaN
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.ShieldUnitsLabel.Parent = Me.ShieldPictureBox
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Parent = Me.SensorTimeLefPictureBox
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Left = (Me.SensorTimeLefPictureBox.Width \ 2) - (Me.SensorDaysLeftLabel.Width \ 2)
        Me.SensorDaysLeftLabel.Top = (Me.SensorTimeLefPictureBox.Height \ 2) - (Me.SensorDaysLeftLabel.Height \ 2)
        Me.AITComboBox.SelectedIndex = Me.AITComboBox.FindStringExact(My.Settings.AIT.ToString("hh\:mm").Substring(1))
        _activeInsulinIncrements = CInt(TimeSpan.Parse(My.Settings.AIT.ToString("hh\:mm").Substring(1)) / _FiveMinutes)
        Me.InitializeHomePageChart()
        Me.InitializeActiveInsulinPageChart()
        Me.InitializeTimeInRangeChart()
    End Sub

    Private Sub HomePageChart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles HomePageChart.CursorPositionChanging
        If Not _initialized Then Exit Sub
        Me.CursorTimer.Interval = CType(New TimeSpan(0, minutes:=0, seconds:=30).TotalMilliseconds, Integer)
        Me.CursorTimer.Enabled = True
        Me.CursorTimer.Start()
    End Sub

    Private Sub HomePageChart_MouseMove(sender As Object, e As MouseEventArgs) Handles HomePageChart.MouseMove

        If Not _initialized Then
            Exit Sub
        End If
        Dim yInPixels As Double = Me.HomePageChart.ChartAreas("Default").AxisY2.ValueToPixelPosition(e.Y)
        If Double.IsNaN(yInPixels) Then
            Exit Sub
        End If
        Dim result As HitTestResult = Me.HomePageChart.HitTest(e.X, e.Y)
        If result.PointIndex >= -1 Then
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
                        Dim marketToolTip() As String = result.Series.Points(result.PointIndex).ToolTip.Split(","c)
                        Dim xValue As Date = Date.FromOADate(result.Series.Points(result.PointIndex).XValue)
                        Me.CursorTimeLabel.Visible = True
                        Me.CursorTimeLabel.Text = xValue.ToString(_timeFormat)
                        Me.CursorTimeLabel.Tag = xValue
                        marketToolTip(0) = marketToolTip(0).Trim
                        Me.CursorValueLabel.Visible = True
                        Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                        Me.CursorPictureBox.Visible = True
                        Select Case marketToolTip.Length
                            Case 2
                                Me.CursorMessage1Label.Text = marketToolTip(0)
                                Select Case marketToolTip(0)
                                    Case "Basal"
                                        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.InsulinVial
                                    Case "Bolus"
                                        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.InsulinVial
                                    Case "Meal"
                                        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.MealImageLarge
                                    Case Else
                                        Me.CursorPictureBox.Image = Nothing
                                End Select
                                Me.CursorMessage2Label.Visible = False
                                Me.CursorValueLabel.Top = Me.CursorMessage1Label.PositionBelow
                                Me.CursorValueLabel.Text = marketToolTip(1).Trim
                            Case 3
                                Select Case marketToolTip(1).Trim
                                    Case "Calibration accepted", "Calibration not accepted"
                                        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.CalibrationDotRed
                                    Case "Not used For calibration"
                                        Me.CursorPictureBox.Image = Global.CareLink.My.Resources.Resources.CalibrationDot
                                    Case Else
                                        Stop
                                End Select
                                Me.CursorMessage1Label.Text = marketToolTip(0)
                                Me.CursorMessage1Label.Top = Me.CursorPictureBox.PositionBelow
                                Me.CursorMessage2Label.Text = marketToolTip(1).Trim
                                Me.CursorMessage2Label.Top = Me.CursorMessage1Label.PositionBelow
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorValueLabel.Text = marketToolTip(2).Trim
                                Me.CursorValueLabel.Top = Me.CursorMessage2Label.PositionBelow
                            Case Else
                                Stop
                        End Select
                    Case "Default"
                        Me.CursorPictureBox.Image = Nothing
                        Me.CursorMessage2Label.Visible = False
                        Me.CursorValueLabel.Visible = False
                        Me.CursorTimeLabel.Text = Date.FromOADate(result.Series.Points(result.PointIndex).XValue).ToString(_timeFormat)
                        Me.CursorTimeLabel.Visible = True
                        Me.CursorMessage1Label.Text = $"{result.Series.Points(result.PointIndex).YValues(0).RoundDouble(3)} {_bgUnitsString}"
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
    End Sub

    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomePageChart.PostPaint
        If Not _initialized Then Exit Sub
        If _imagePosition = Rectangle.Empty Then
            _imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, SGs.SafeGetSgDateTime(0).ToOADate))
            _imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, 400))
            _imagePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, HighLimit)))) - _imagePosition.Y
            _imagePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, SGs.SafeGetSgDateTime(SGs.Count - 1).ToOADate)) - _imagePosition.X
            _imagePosition = e.ChartGraphics.GetAbsoluteRectangle(_imagePosition)
        End If

        Dim highAreaRectangle As New Rectangle(New Point(CInt(_imagePosition.X), CInt(_imagePosition.Y)), New Size(CInt(_imagePosition.Width), 292))

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)
        End Using
        Dim lowHeight As Integer
        If _homePageChartChartArea.AxisX.ScrollBar.IsVisible Then
            lowHeight = CInt(25 - _homePageChartChartArea.AxisX.ScrollBar.Size)
        Else
            lowHeight = 25
        End If
        Dim lowAreaRectangle As New Rectangle(New Point(CInt(_imagePosition.X), 504), New Size(CInt(_imagePosition.Width), lowHeight))
        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, lowAreaRectangle)
        End Using
        If Me.CursorTimeLabel.Tag IsNot Nothing Then
            Me.CursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, CDate(Me.CursorTimeLabel.Tag).ToOADate))
        End If

        e.PaintMarker(_mealImage, _markerMealDictionary, 0)
        e.PaintMarker(_insulinImage, _markerInsulinDictionary, -6)
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Me.ServerUpdateTimer.Enabled = False
        If Me.UseTestDataToolStripMenuItem.Checked Then
            RecentData = Loads(IO.File.ReadAllText(IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")))
        Else
            _loginDialog.ShowDialog()
            _client = _loginDialog.Client
            If _client IsNot Nothing AndAlso _client.LoggedIn Then
                RecentData = _client.GetRecentData()
            End If
            If RecentData Is Nothing Then
                Exit Sub
            End If
            Me.ServerUpdateTimer.Interval = CType(New TimeSpan(0, 5, 0).TotalMilliseconds, Integer)
            Me.ServerUpdateTimer.Enabled = True
        End If
        Me.UpdateAllTabPages()

    End Sub

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        Me.ServerUpdateTimer.Enabled = False
        RecentData = _client.GetRecentData()
        If RecentData Is Nothing Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        Me.UpdateAllTabPages()
        Application.DoEvents()
        Me.ServerUpdateTimer.Enabled = True
    End Sub

#End Region

    Private Sub AITComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AITComboBox.SelectedIndexChanged
        My.Settings.AIT = TimeSpan.Parse(Me.AITComboBox.SelectedItem.ToString())
        My.Settings.Save()
    End Sub

    Private Function DrawCenteredArc(backImage As Bitmap, arcPercentage As Double, Optional colorTable As IReadOnlyDictionary(Of String, Color) = Nothing, Optional segmentName As String = "") As Bitmap
        If arcPercentage < Double.Epsilon Then
            Return backImage
        End If
        Dim targetImage As Bitmap = backImage
        Dim myGraphics As Graphics = Graphics.FromImage(targetImage)
        Dim rect As New Rectangle(1, 1, backImage.Width - 2, backImage.Height - 2)
        myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim pen As Pen
        If colorTable Is Nothing Then
            pen = New Pen(Me.GetColorFromTimeToNextCalib(), 2)
        Else
            pen = New Pen(colorTable(segmentName), 5)
        End If
        myGraphics.DrawArc(pen, rect, -90, -CInt(360 * arcPercentage))
        Return targetImage
    End Function

    Private Function GetColorFromTimeToNextCalib() As Color
        If TimeToNextCalibHours <= 2 Then
            Return Color.Red
        ElseIf TimeToNextCalibHours < 4 Then
            Return Color.Yellow
        Else
            Return Color.Lime
        End If
    End Function

    Private Sub GetInnerTable(tableLevel1Blue As TableLayoutPanel, innerJson As Dictionary(Of String, String), itemIndex As ItemIndexs)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.BackColor = Color.LightBlue
        If itemIndex = ItemIndexs.notificationHistory Then
            NotificationHistory = New Dictionary(Of String, List(Of Dictionary(Of String, String)))
        End If
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            ' Comment out 4 lines below to see all data fields.
            ' I did not see any use to display the filtered out ones
            If s_zFilterList.ContainsKey(itemIndex) Then
                If s_zFilterList(itemIndex).Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0))
            If itemIndex = ItemIndexs.limits OrElse itemIndex = ItemIndexs.markers Then
                tableLevel1Blue.AutoSize = True
            End If
            Dim label1 As New Label With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .Text = innerRow.Key,
                    .AutoSize = True
                    }

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = LoadList(innerRow.Value)
                If itemIndex = ItemIndexs.notificationHistory Then
                    NotificationHistory.Add(innerRow.Key, innerJson1)
                End If
                If innerJson1.Count > 0 Then
                    Dim tableLevel2 As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .BorderStyle = BorderStyle.Fixed3D,
                            .BackColor = Color.Aqua,
                            .ColumnCount = 1,
                            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            .Dock = DockStyle.Top,
                            .RowCount = innerJson1.Count
                            }
                    ' ReSharper disable once RedundantAssignment
                    For i As Integer = 0 To innerJson1.Count - 1
                        tableLevel2.RowStyles.Add(New RowStyle() With {.SizeType = SizeType.AutoSize})
                    Next
                    tableLevel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 80.0))
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson1.WithIndex()
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        tableLevel2.RowStyles.Add(New RowStyle(SizeType.Absolute, 4 + (dic.Keys.Count * 22)))
                        Dim tableLevel3 As New TableLayoutPanel With {
                                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                .AutoScroll = False,
                                .AutoSize = True,
                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                .BorderStyle = BorderStyle.FixedSingle,
                                .ColumnCount = 2,
                                .Dock = DockStyle.Top
                                }
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            If s_zFilterList.ContainsKey(itemIndex) Then
                                If s_zFilterList(itemIndex).Contains(e.Value.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3.RowCount += 1
                            tableLevel3.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0))
                            tableLevel3.Controls.AddRange({New Label With {
                                                                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                    .Text = e.Value.Key,
                                                                    .AutoSize = True},
                                                                 New TextBox With {
                                                                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                    .AutoSize = True,
                                                                     .Text = e.Value.Value}})
                            Application.DoEvents()
                        Next
                        tableLevel3.Height += 40
                        tableLevel2.Controls.Add(tableLevel3, 0, innerDictionary.Index)
                        tableLevel2.Height += 4
                        Application.DoEvents()
                    Next
                    tableLevel1Blue.Controls.AddRange({label1, tableLevel2})
                Else
                    tableLevel1Blue.Controls.AddRange({label1, New TextBox With {
                                               .Text = "",
                                               .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                               .AutoSize = True
                                               }})
                End If
            Else
                tableLevel1Blue.Controls.AddRange({label1, New TextBox With {
                                           .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                           .AutoSize = True,
                                           .Text = innerRow.Value}})
            End If
        Next

        Dim tableLayoutParent As TableLayoutPanel = CType(tableLevel1Blue.Parent, TableLayoutPanel)
        If itemIndex = ItemIndexs.lastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        ElseIf itemIndex = ItemIndexs.lastAlarm Then
            tableLevel1Blue.Dock = DockStyle.None
            If tableLevel1Blue.RowCount > 5 Then
                tableLevel1Blue.AutoSize = True
                tableLevel1Blue.ColumnStyles(1).SizeType = SizeType.Absolute
                tableLevel1Blue.ColumnStyles(1).Width = 280
                tableLayoutParent.AutoScroll = False
                tableLayoutParent.Height = 22 * tableLevel1Blue.RowCount
                tableLayoutParent.HorizontalScroll.Visible = False
            Else
                tableLevel1Blue.ColumnStyles(1).SizeType = SizeType.Absolute
                tableLevel1Blue.ColumnStyles(1).Width = 200
                tableLevel1Blue.Width = 450
                tableLayoutParent.Width = 640
            End If
        ElseIf itemIndex = ItemIndexs.notificationHistory Then
            tableLevel1Blue.RowStyles(1).SizeType = SizeType.AutoSize
        End If
        Application.DoEvents()
    End Sub

    Private Sub GetLimitsList(ByRef limitsIndexList As Integer())

        Dim limitsIndex As Integer = 0
        For i As Integer = 0 To limitsIndexList.GetUpperBound(0)
            If limitsIndex + 1 < Limits.Count AndAlso CInt(Limits(limitsIndex + 1)("index")) < i Then
                limitsIndex += 1
            End If
            limitsIndexList(i) = limitsIndex
        Next
    End Sub

    Private Sub InitializeActiveInsulinPageChart()
        Me.ActiveInsulinPageChart = New Chart With {
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

        _activeInsulinPageChartArea = New ChartArea With {
             .BackColor = Color.FromArgb(180, 23, 47, 19),
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.FromArgb(180, 29, 56, 26),
             .BorderColor = Color.FromArgb(64, 64, 64, 64),
             .BorderDashStyle = ChartDashStyle.Solid,
             .Name = "Default",
             .ShadowColor = Color.Transparent
         }
        _activeInsulinPageChartArea.AxisX.IsInterlaced = True
        _activeInsulinPageChartArea.AxisX.IsMarginVisible = True
        _activeInsulinPageChartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
        _activeInsulinPageChartArea.AxisX.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        _activeInsulinPageChartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64)
        _activeInsulinPageChartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
        _activeInsulinPageChartArea.AxisX.ScaleView.Zoomable = False
        _activeInsulinPageChartArea.AxisY.InterlacedColor = Color.FromArgb(120, Color.LightSlateGray)
        _activeInsulinPageChartArea.AxisY.Interval = 2
        _activeInsulinPageChartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount
        _activeInsulinPageChartArea.AxisY.IsInterlaced = True
        _activeInsulinPageChartArea.AxisY.IsLabelAutoFit = False
        _activeInsulinPageChartArea.AxisY.IsMarginVisible = False
        _activeInsulinPageChartArea.AxisY.IsStartedFromZero = True
        _activeInsulinPageChartArea.AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        _activeInsulinPageChartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64)
        _activeInsulinPageChartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
        _activeInsulinPageChartArea.AxisY.MajorTickMark = New TickMark() With {.Interval = InsulinRow, .Enabled = False}
        _activeInsulinPageChartArea.AxisY.Maximum = 25
        _activeInsulinPageChartArea.AxisY.Minimum = 0
        _activeInsulinPageChartArea.AxisY2.Maximum = 25
        _activeInsulinPageChartArea.AxisY2.Minimum = 0

        Me.ActiveInsulinPageChart.ChartAreas.Add(_activeInsulinPageChartArea)

        Me.ActiveInsulinPageChart.Legends.Add(New Legend With {
                                     .BackColor = Color.Transparent,
                                     .Enabled = False,
                                     .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
                                     .IsTextAutoFit = False,
                                     .Name = "Default"
                                     }
                                  )
        Me.ActiveInsulinPageChart.Series.Add(New Series With {
                                    .BorderColor = Color.FromArgb(180, 26, 59, 105),
                                    .BorderWidth = 4,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.Line,
                                    .Color = Color.White,
                                    .Legend = "Default",
                                    .Name = "Default",
                                    .ShadowColor = Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Primary
                                    })
        Me.ActiveInsulinPageChart.Series.Add(New Series With {
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
                                                })
        Me.ActiveInsulinPageChart.Series("Default").EmptyPointStyle.Color = Color.Transparent
        Me.ActiveInsulinPageChart.Series("Default").EmptyPointStyle.BorderWidth = 4
        Me.ActiveInsulinPageChart.Titles.Add(New Title With {
                                    .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                                    .ForeColor = Color.FromArgb(26, 59, 105),
                                    .Name = "Title1",
                                    .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                                    .ShadowOffset = 3
                                    }
                                 )
        Me.TabPage2.Controls.Add(Me.ActiveInsulinPageChart)
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
             .Location = New Point(3, Me.ShieldPictureBox.Height + 23),
             .Name = "chart1",
             .Size = New Size(Me.TabPage1.ClientSize.Width - 240, Me.TabPage1.ClientSize.Height - (Me.ShieldPictureBox.Height + Me.ShieldPictureBox.Top + 26)),
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
        _homePageChartChartArea.AxisX.IsInterlaced = True
        _homePageChartChartArea.AxisX.IsMarginVisible = True
        _homePageChartChartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
        _homePageChartChartArea.AxisX.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        _homePageChartChartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisX.ScaleView.Zoomable = True
        _homePageChartChartArea.AxisX.ScrollBar.BackColor = Color.White
        _homePageChartChartArea.AxisX.ScrollBar.ButtonColor = Color.Lime
        _homePageChartChartArea.AxisX.ScrollBar.IsPositionedInside = True
        _homePageChartChartArea.AxisX.ScrollBar.LineColor = Color.Yellow
        _homePageChartChartArea.AxisX.ScrollBar.LineColor = Color.Black
        _homePageChartChartArea.AxisX.ScrollBar.Size = 15
        _homePageChartChartArea.AxisY.InterlacedColor = Color.FromArgb(120, Color.LightSlateGray)
        _homePageChartChartArea.AxisY.Interval = InsulinRow
        _homePageChartChartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount
        _homePageChartChartArea.AxisY.IsInterlaced = True
        _homePageChartChartArea.AxisY.IsLabelAutoFit = False
        _homePageChartChartArea.AxisY.IsMarginVisible = False
        _homePageChartChartArea.AxisY.IsStartedFromZero = False
        _homePageChartChartArea.AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        _homePageChartChartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisY.MajorTickMark = New TickMark() With {.Interval = InsulinRow, .Enabled = False}
        _homePageChartChartArea.AxisY.Maximum = 400
        _homePageChartChartArea.AxisY.Minimum = InsulinRow
        _homePageChartChartArea.AxisY.ScaleBreakStyle = New AxisScaleBreakStyle() With {
                                                            .Enabled = True,
                                                            .StartFromZero = StartFromZero.No,
                                                            .BreakLineStyle = BreakLineStyle.Straight
                                                            }
        _homePageChartChartArea.AxisY.ScaleView.Zoomable = False
        _homePageChartChartArea.AxisY2.Interval = InsulinRow
        _homePageChartChartArea.AxisY2.IsMarginVisible = False
        _homePageChartChartArea.AxisY2.IsStartedFromZero = False
        _homePageChartChartArea.AxisY2.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold) '
        _homePageChartChartArea.AxisY2.LineColor = Color.FromArgb(64, 64, 64, 64) '
        _homePageChartChartArea.AxisY2.MajorGrid = New Grid() With {.Interval = InsulinRow}
        _homePageChartChartArea.AxisY2.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64) '
        _homePageChartChartArea.AxisY2.MajorTickMark = New TickMark() With {.Interval = InsulinRow, .Enabled = True} '
        _homePageChartChartArea.AxisY2.Maximum = 400
        _homePageChartChartArea.AxisY2.Minimum = InsulinRow
        _homePageChartChartArea.AxisY2.ScaleView.Zoomable = False
        _homePageChartChartArea.CursorX.AutoScroll = True
        _homePageChartChartArea.CursorX.AxisType = AxisType.Primary
        _homePageChartChartArea.CursorX.Interval = 0
        _homePageChartChartArea.CursorX.IsUserEnabled = True
        _homePageChartChartArea.CursorX.IsUserSelectionEnabled = False
        _homePageChartChartArea.CursorY.AutoScroll = False
        _homePageChartChartArea.CursorY.AxisType = AxisType.Secondary
        _homePageChartChartArea.CursorY.LineColor = Color.Transparent
        _homePageChartChartArea.CursorY.Interval = 0
        _homePageChartChartArea.CursorY.IsUserEnabled = False
        _homePageChartChartArea.CursorY.IsUserSelectionEnabled = False

        Me.HomePageChart.ChartAreas.Add(_homePageChartChartArea)
        Me.HomePageChart.Legends.Add(New Legend With {
                                     .BackColor = Color.Transparent,
                                     .Enabled = False,
                                     .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
                                     .IsTextAutoFit = False,
                                     .Name = "Default"
                                     }
                                  )
        Me.HomePageChart.Series.Add(New Series With {
                                    .BorderColor = Color.FromArgb(180, 26, 59, 105),
                                    .BorderWidth = 4,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.Line,
                                    .Color = Color.White,
                                    .Legend = "Default",
                                    .Name = "Default",
                                    .ShadowColor = Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    })
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

        Me.HomePageChart.Series.Add(Me.MarkerSeries)

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
        Me.HomePageChart.Series.Add(Me.HighLimitSeries)
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
        Me.HomePageChart.Series.Add(Me.LowLimitSeries)
        Me.HomePageChart.Series("Default").EmptyPointStyle.Color = Color.Transparent
        Me.HomePageChart.Series("Default").EmptyPointStyle.BorderWidth = 4
        Me.HomePageChart.Titles.Add(New Title With {
                                    .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                                    .ForeColor = Color.FromArgb(26, 59, 105),
                                    .Name = "Title1",
                                    .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                                    .ShadowOffset = 3
                                    }
                                 )
        Me.TabPage1.Controls.Add(Me.HomePageChart)
        Application.DoEvents()

    End Sub

    Private Sub InitializeTimeInRangeChart()
        Me.TimeInRangeChart = New Chart With {
            .Anchor = AnchorStyles.Right,
            .BackColor = Color.Transparent,
            .BackGradientStyle = GradientStyle.None,
            .BackSecondaryColor = Color.Transparent,
            .BorderlineColor = Color.Transparent,
            .BorderlineWidth = 0,
            .Size = New Size(220, 220)
        }

        Me.TimeInRangeChart.BorderSkin.BackSecondaryColor = Color.Transparent
        Me.TimeInRangeChart.BorderSkin.SkinStyle = BorderSkinStyle.None
        Me.TimeInRangeChart.ChartAreas.Add(New ChartArea With {.Name = "TimeInRangeChartChartArea",
                                                            .BackColor = Color.Black})
        Me.TimeInRangeChart.Location = New Point(Me.TimeInRangeSummaryLabel.FindHorizontalMidpoint - (Me.TimeInRangeChart.Width \ 2),
                                                 Me.TimeInRangeSummaryLabel.FindVerticalMidpoint() - (Me.TimeInRangeChart.Height \ 2))
        Me.TimeInRangeChart.Name = "Default"

        Me.TimeInRangeChart.Series.Add(New Series With {
            .ChartArea = "TimeInRangeChartChartArea",
            .ChartType = SeriesChartType.Doughnut,
            .Name = "Default"})
        Me.TimeInRangeChart.Series("Default")("DoughnutRadius") = "20"
        Me.TimeInRangeChart.Titles.Add(New Title With {
                                        .Name = "TimeInRangeChartTitle",
                                        .Text = "Time In Range Last 24 Hours"}
                                    )
        Me.TabPage1.Controls.Add(Me.TimeInRangeChart)
        Application.DoEvents()
    End Sub

    Private Sub LoadDataTables(localRecentData As Dictionary(Of String, String))
        If localRecentData Is Nothing Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Me.TableLayoutPanel1.Controls.Clear()
        Me.TableLayoutPanel1.RowCount = localRecentData.Count - 8
        Dim currentRowIndex As Integer = 0
        Dim singleItem As Boolean
        Dim layoutPanel1 As TableLayoutPanel
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
            Dim singleItemIndex As Integer = 0
            layoutPanel1 = Me.TableLayoutPanel1
            singleItem = False
            Dim row As KeyValuePair(Of String, String) = c.Value
            Dim rowIndex As Integer = CInt([Enum].Parse(GetType(ItemIndexs), c.Value.Key))

            Select Case rowIndex
                Case ItemIndexs.lastSensorTS
                    LastSensorTS = row.Value
                Case ItemIndexs.medicalDeviceTimeAsString
                    MedicalDeviceTimeAsString = row.Value
                Case ItemIndexs.lastSensorTSAsString
                    LastSensorTSAsString = row.Value
                Case ItemIndexs.kind
                    Kind = row.Value
                Case ItemIndexs.version
                    Version = row.Value
                Case ItemIndexs.pumpModelNumber
                    PumpModelNumber = row.Value
                Case ItemIndexs.currentServerTime
                    CurrentServerTime = row.Value
                Case ItemIndexs.lastConduitTime
                    LastConduitTime = row.Value
                Case ItemIndexs.lastConduitUpdateServerTime
                    LastConduitUpdateServerTime = row.Value
                Case ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                    LastMedicalDeviceDataUpdateServerTime = row.Value
                Case ItemIndexs.firstName
                    FirstName = row.Value
                Case ItemIndexs.lastName
                    LastName = row.Value
                Case ItemIndexs.conduitSerialNumber
                    ConduitSerialNumber = New Guid(row.Value)
                Case ItemIndexs.conduitBatteryLevel
                    ConduitBatteryLevel = CInt(row.Value)
                Case ItemIndexs.conduitBatteryStatus
                    ConduitBatteryStatus = row.Value
                Case ItemIndexs.conduitInRange
                    ConduitInRange = CBool(row.Value)
                Case ItemIndexs.conduitMedicalDeviceInRange
                    ConduitMedicalDeviceInRange = CBool(row.Value)
                Case ItemIndexs.conduitSensorInRange
                    ConduitSensorInRange = CBool(row.Value)
                Case ItemIndexs.medicalDeviceFamily
                    MedicalDeviceFamily = row.Value
                Case ItemIndexs.sensorState
                    SensorState = row.Value
                Case ItemIndexs.medicalDeviceSerialNumber
                    MedicalDeviceSerialNumber = row.Value
                Case ItemIndexs.medicalDeviceTime
                    MedicalDeviceTime = row.Value
                Case ItemIndexs.sMedicalDeviceTime
                    SMedicalDeviceTime = CDate(row.Value)
                Case ItemIndexs.reservoirLevelPercent
                    ReservoirLevelPercent = CInt(row.Value)
                Case ItemIndexs.reservoirAmount
                    ReservoirAmount = CInt(CDbl(row.Value))
                Case ItemIndexs.reservoirRemainingUnits
                    ReservoirRemainingUnits = CType(row.Value, Double)
                Case ItemIndexs.medicalDeviceBatteryLevelPercent
                    MedicalDeviceBatteryLevelPercent = CInt(row.Value)
                Case ItemIndexs.sensorDurationHours
                    SensorDurationHours = CInt(row.Value)
                Case ItemIndexs.timeToNextCalibHours
                    TimeToNextCalibHours = CByte(row.Value)
                Case ItemIndexs.calibStatus
                    CalibStatus = row.Value
                Case ItemIndexs.bgUnits
                    BgUnits = row.Value
                    _bgUnitsString = _unitsStrings(BgUnits)
                    If _bgUnitsString = "mg/dl" Then
                        HighLimit = 180
                        LowLimit = 70
                        _timeFormat = TwelveHourTimeWithMinuteFormat
                        _homePageChartChartArea.AxisX.LabelStyle.Format = "hh tt"
                        _activeInsulinPageChartArea.AxisX.LabelStyle.Format = "hh tt"
                    Else
                        HighLimit = 10.0
                        LowLimit = (70 / 18).RoundSingle(1)
                        _timeFormat = MilitaryTimeWithMinuteFormat
                        _activeInsulinPageChartArea.AxisX.LabelStyle.Format = "HH"
                    End If
                    Me.AboveHighLimitMessageLabel.Text = $"Above {HighLimit} {_bgUnitsString}"
                    Me.AverageSGUnitsLabel.Text = _bgUnitsString
                    Me.BelowLowLimitMessageLabel.Text = $"Below {LowLimit} {_bgUnitsString}"
                Case ItemIndexs.timeFormat
                    TimeFormat = row.Value
                Case ItemIndexs.lastSensorTime
                    LastSensorTime = row.Value
                Case ItemIndexs.sLastSensorTime
                    SLastSensorTime = CDate(row.Value)
                Case ItemIndexs.medicalDeviceSuspended
                    MedicalDeviceSuspended = CBool(row.Value)
                Case ItemIndexs.lastSGTrend
                    LastSGTrend = row.Value
                Case ItemIndexs.lastSG
                    layoutPanel1 = Me.TableLayoutPanelTop1
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.lastAlarm
                    layoutPanel1 = Me.TableLayoutPanelTop2
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.activeInsulin
                    layoutPanel1 = Me.TableLayoutPanel2
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.sgs
                    layoutPanel1 = Me.TableLayoutPanel3
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.limits
                    layoutPanel1 = Me.TableLayoutPanel4
                    layoutPanel1.Controls.Clear()
                    layoutPanel1.AutoSize = True
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True

                Case ItemIndexs.markers
                    layoutPanel1 = Me.TableLayoutPanel5
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.notificationHistory
                    layoutPanel1 = Me.TableLayoutPanel6
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.therapyAlgorithmState
                    ' handled elsewhere
                Case ItemIndexs.pumpBannerState
                    ' handled elsewhere
                Case ItemIndexs.basal
                    layoutPanel1 = Me.TableLayoutPanel7
                    layoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    layoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.systemStatusMessage
                    SystemStatusMessage = row.Value
                Case ItemIndexs.averageSG
                    AverageSG = CInt(row.Value)
                Case ItemIndexs.belowHypoLimit
                    BelowHypoLimit = CInt(row.Value)
                Case ItemIndexs.aboveHyperLimit
                    AboveHyperLimit = CInt(row.Value)
                Case ItemIndexs.timeInRange
                    TimeInRange = CInt(row.Value)
                Case ItemIndexs.pumpCommunicationState
                    PumpCommunicationState = CBool(row.Value)
                Case ItemIndexs.gstCommunicationState
                    GstCommunicationState = CBool(row.Value)
                Case ItemIndexs.gstBatteryLevel
                    GstBatteryLevel = CInt(row.Value)
                Case ItemIndexs.lastConduitDateTime
                    LastConduitDateTime = row.Value
                Case ItemIndexs.maxAutoBasalRate
                    MaxAutoBasalRate = CDbl(row.Value)
                Case ItemIndexs.maxBolusAmount
                    MaxBolusAmount = CDbl(row.Value)
                Case ItemIndexs.sensorDurationMinutes
                    SensorDurationMinutes = CInt(row.Value)
                Case ItemIndexs.timeToNextCalibrationMinutes
                    TimeToNextCalibrationMinutes = CInt(row.Value)
                Case ItemIndexs.clientTimeZoneName
                    ClientTimeZoneName = row.Value
                Case ItemIndexs.sgBelowLimit
                    SgBelowLimit = CInt(row.Value)
                Case ItemIndexs.averageSGFloat
                    AverageSGFloat = CDbl(row.Value)
                Case Else
                    Stop
            End Select

            If _listOfSingleItems.Contains(rowIndex) OrElse singleItem Then
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
                layoutPanel1.Controls.Add(New Label With {
                                                  .Text = $"{rowIndex} {row.Key}",
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True
                                                  }, 0, tableRelitiveRow)
            End If
            If row.Value?.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = LoadList(row.Value)
                Select Case rowIndex
                    Case ItemIndexs.sgs
                        SGs = innerJson
                    Case ItemIndexs.limits
                        Limits = innerJson
                    Case ItemIndexs.markers
                        Markers = innerJson
                    Case ItemIndexs.notificationHistory
                        ' handled elsewhere
                    Case ItemIndexs.pumpBannerState
                        PumpBannerState = innerJson
                    Case Else
                        Stop
                End Select
                If innerJson.Count > 0 Then
                    For Each dic As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
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
                        layoutPanel1.Controls.Add(tableLevel1Blue, column:=1, row:=dic.Index)
                        Me.GetInnerTable(tableLevel1Blue, dic.Value, CType(rowIndex, ItemIndexs))
                    Next
                Else
                    layoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = ""}, If(singleItem, 0, 1), tableRelitiveRow)

                End If
            ElseIf row.Value?.StartsWith("{") Then
                layoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
                Dim innerJson As Dictionary(Of String, String) = Loads(row.Value)
                Select Case rowIndex
                    Case ItemIndexs.lastSG
                        LastSG = innerJson
                    Case ItemIndexs.lastAlarm
                        LastAlarm = innerJson
                    Case ItemIndexs.activeInsulin
                        ActiveInsulin = innerJson
                    Case ItemIndexs.notificationHistory
                    Case ItemIndexs.therapyAlgorithmState
                        TherapyAlgorithmState = innerJson
                    Case ItemIndexs.basal
                        Basal = innerJson
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
                layoutPanel1.Controls.Add(tableLevel1Blue, If(singleItem AndAlso Not (rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm), 0, 1), tableRelitiveRow)
                If rowIndex = ItemIndexs.notificationHistory Then
                    tableLevel1Blue.AutoScroll = False
                End If
                Me.GetInnerTable(tableLevel1Blue, innerJson, CType(rowIndex, ItemIndexs))
            Else
                layoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)
            End If
            Application.DoEvents()
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub UpdateActiveInsulin()
        Me.ActiveInsulinValue.Text = $"{ActiveInsulin("amount"):N3} U"
    End Sub

    Private Sub UpdateActiveInsulinChart()
        If Not _initialized Then
            Exit Sub
        End If
        _initialized = False

        Me.ActiveInsulinPageChart.Titles("Title1").Text = $"Summary"
        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.Minimum = SGs.SafeGetSgDateTime(0).ToOADate()
        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.Maximum = SGs.SafeGetSgDateTime(SGs.Count - 1).ToOADate()
        Me.ActiveInsulinPageChart.Series("Default").Points.Clear()
        Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Clear()

        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Hours
        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.MajorGrid.Interval = 1
        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.IntervalType = DateTimeIntervalType.Hours
        Me.ActiveInsulinPageChart.ChartAreas("Default").AxisX.Interval = 2

        ' Order all markers by time
        Dim timeOrderedMarkers As New SortedDictionary(Of Double, Double)
        Dim maxActiveInsulin As Double = 0
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In Markers.WithIndex()
            Dim sgDateTime As Date = Markers.SafeGetSgDateTime(sgListIndex.Index)
            Dim sgOaDateTime As Double = sgDateTime.RoundDown(RoundTo.Minute).ToOADate
            Select Case sgListIndex.Value("type")
                Case "INSULIN"
                    Dim bolusAmount As Double = sgListIndex.Value.GetDecimalValue("programmedFastAmount")
                    If timeOrderedMarkers.ContainsKey(sgOaDateTime) Then
                        timeOrderedMarkers(sgOaDateTime) += bolusAmount
                    Else
                        timeOrderedMarkers.Add(sgOaDateTime, bolusAmount)
                    End If
                    maxActiveInsulin = Math.Max(timeOrderedMarkers(sgOaDateTime), maxActiveInsulin)
                Case "AUTO_BASAL_DELIVERY"
                    Dim bolusAmount As Double = sgListIndex.Value.GetDecimalValue("bolusAmount")
                    If timeOrderedMarkers.ContainsKey(sgOaDateTime) Then
                        timeOrderedMarkers(sgOaDateTime) += bolusAmount
                    Else
                        timeOrderedMarkers.Add(sgOaDateTime, bolusAmount)
                    End If
                    maxActiveInsulin = Math.Max(timeOrderedMarkers(sgOaDateTime), maxActiveInsulin)
            End Select
        Next
        maxActiveInsulin = Math.Ceiling(maxActiveInsulin) + 1
        _activeInsulinPageChartArea.AxisY.Maximum = maxActiveInsulin
        _activeInsulinPageChartArea.AxisY2.Maximum = maxActiveInsulin

        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In Markers.WithIndex()
            Dim sgDateTime As Date = Markers.SafeGetSgDateTime(sgListIndex.Index)
            Dim sgOaDateTime As Double = sgDateTime.RoundDown(RoundTo.Minute).ToOADate
            Select Case sgListIndex.Value("type")
                Case "INSULIN"
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, maxActiveInsulin)
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Bolus, {sgListIndex.Value("programmedFastAmount")} U"
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Last.Color = Color.LightBlue
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerSize = 15
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerStyle = MarkerStyle.Square
                Case "AUTO_BASAL_DELIVERY"
                    Dim bolusAmount As Double = sgListIndex.Value.GetDecimalValue("bolusAmount")
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, maxActiveInsulin)
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Basal, {bolusAmount.RoundDouble(3)} U"
                    Me.ActiveInsulinPageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerSize = 8
            End Select
        Next

        ' set up table that holds active insulin for every 5 minutes
        Dim remainingInsulinList As New List(Of Insulin)
        Dim currentMarker As Integer = 0
        Dim getSgDateTime As Date = SGs.SafeGetSgDateTime(0)

        For i As Integer = 0 To 287
            Dim initialBolus As Double = 0
            Dim oaTime As Double = (getSgDateTime + (_FiveMinutes * i)).RoundDown(RoundTo.Minute).ToOADate()
            While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= oaTime
                initialBolus += timeOrderedMarkers.Values(currentMarker)
                currentMarker += 1
            End While
            remainingInsulinList.Add(New Insulin(oaTime, initialBolus, _activeInsulinIncrements))
        Next

        ' walk all markers, adjust active insulin and then add new marker
        For i As Integer = 0 To remainingInsulinList.Count - 1
            If i < _activeInsulinIncrements Then
                Me.ActiveInsulinPageChart.Series("Default").Points.AddXY(remainingInsulinList(i).OaTime, Double.NaN)
                Me.ActiveInsulinPageChart.Series("Default").Points.Last.IsEmpty = True
                If i > 0 Then
                    remainingInsulinList.Adjustlist(0, i)
                End If
                Continue For
            End If
            Dim startIndex As Integer = i - _activeInsulinIncrements + 1
            Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, _activeInsulinIncrements)
            Me.ActiveInsulinPageChart.Series("Default").Points.AddXY(remainingInsulinList(i).OaTime, sum)
            remainingInsulinList.Adjustlist(startIndex, _activeInsulinIncrements)
        Next
        _initialized = True
        Application.DoEvents()
    End Sub

    Private Sub UpdateAllTabPages()
        If RecentData Is Nothing OrElse _updating Then
            Exit Sub
        End If
        _updating = True
        Me.LoadDataTables(RecentData)
        If RecentData.Count > ItemIndexs.averageSGFloat + 1 Then
            Stop
        End If
        _initialized = True
        Me.UpdateActiveInsulinChart()
        Me.UpdateHomeTabPage()
        _updating = False
    End Sub

    Private Sub UpdateAutoModeShield()
        Me.SensorMessage.Location = New Point(Me.ShieldPictureBox.Left + (Me.ShieldPictureBox.Width \ 2) - (Me.SensorMessage.Width \ 2), Me.SensorMessage.Top)
        If LastSG("sg") <> "0" Then
            Me.CurrentBG.Location = New Point((Me.ShieldPictureBox.Width \ 2) - (Me.CurrentBG.Width \ 2), Me.ShieldPictureBox.Height \ 4)
            Me.CurrentBG.Parent = Me.ShieldPictureBox
            Me.CurrentBG.Text = LastSG("sg")
            Me.CurrentBG.Visible = True
            Me.SensorMessage.Visible = False
            Me.SensorMessage.Visible = False
            Me.ShieldPictureBox.Image = My.Resources.Shield
            Me.ShieldUnitsLabel.Visible = True
            Me.ShieldUnitsLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.Parent = Me.ShieldPictureBox
            Me.ShieldUnitsLabel.Left = (Me.ShieldPictureBox.Width \ 2) - (Me.ShieldUnitsLabel.Width \ 2)
            Me.ShieldUnitsLabel.Text = _bgUnitsString
            Me.ShieldUnitsLabel.Visible = True
        Else
            Me.CurrentBG.Visible = False
            Me.ShieldPictureBox.Image = My.Resources.Shield_Disabled
            Me.SensorMessage.Visible = True
            Me.SensorMessage.Parent = Me.ShieldPictureBox
            Me.SensorMessage.Left = 0
            Me.SensorMessage.BackColor = Color.Transparent
            If SensorState = "NO_ERROR_MESSAGE" Then
                Me.SensorMessage.Text = $"---"
            Else
                Me.SensorMessage.Text = _messages(SensorState)
            End If
            Me.ShieldUnitsLabel.Visible = False
            Me.SensorMessage.Visible = True
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        If TimeToNextCalibHours = Byte.MaxValue Then
            Me.CalibrationDueImage.Image = My.Resources.Resources.CalibrationUnavailable
        ElseIf TimeToNextCalibHours < 1 Then
            If SystemStatusMessage = "WAIT_TO_CALIBRATE" OrElse SensorState = "WARM_UP" Then
                Me.CalibrationDueImage.Image = My.Resources.Resources.CalibrationNotReady
            Else
                Me.CalibrationDueImage.Image = Me.DrawCenteredArc(My.Resources.CalibrationDotRed, TimeToNextCalibHours / 12)
            End If
        Else
            Me.CalibrationDueImage.Image = Me.DrawCenteredArc(My.Resources.CalibrationDot, TimeToNextCalibHours / 12)
        End If

        Application.DoEvents()
    End Sub

    Private Sub UpdateHomePageSerieses(limitsIndexList As Integer())
        Me.HomePageChart.Series("Default").Points.Clear()
        Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Clear()
        Me.HomePageChart.Series(NameOf(HighLimitSeries)).Points.Clear()
        Me.HomePageChart.Series(NameOf(LowLimitSeries)).Points.Clear()
        _markerInsulinDictionary.Clear()
        _markerMealDictionary.Clear()
        Dim bgValue As Single
        Dim sgDateTime As Date
        Dim sgOaDateTime As Double
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In Markers.WithIndex()
            sgDateTime = Markers.SafeGetSgDateTime(sgListIndex.Index)
            sgOaDateTime = sgDateTime.ToOADate()
            Dim bgValueString As String = ""
            If sgListIndex.Value.TryGetValue("value", bgValueString) Then
                bgValue = CInt(bgValueString)
                If bgValue < InsulinRow Then Stop
            End If
            Select Case sgListIndex.Value("type")
                Case "BG_READING"
                    bgValue = CInt(sgListIndex.Value("value"))
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, bgValue)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.BorderColor = Color.Gainsboro
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.Color = Color.Transparent
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerBorderWidth = 2
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerSize = 10
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Blood Glucose, Not used For calibration, {sgListIndex.Value("value")} {_bgUnitsString}"
                Case "CALIBRATION"
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, bgValue)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.BorderColor = Color.Red
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.Color = Color.Transparent
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerBorderWidth = 2
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerSize = 8
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Blood Glucose, Calibration {If(CBool(sgListIndex.Value("calibrationSuccess")), "accepted", "not accepted")}, {sgListIndex.Value("value")} {_bgUnitsString}"
                Case "INSULIN"
                    _markerInsulinDictionary.Add(sgOaDateTime, MarkerRow)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, MarkerRow)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.Color = Color.FromArgb(30, Color.LightBlue)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerBorderWidth = 0
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerSize = 30
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerStyle = MarkerStyle.Square
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Bolus, {sgListIndex.Value("programmedFastAmount")} U"
                Case "MEAL"
                    _markerMealDictionary.Add(sgOaDateTime, InsulinRow)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, InsulinRow)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.Color = Color.FromArgb(30, Color.Yellow)
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerBorderWidth = 0
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerSize = 30
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerStyle = MarkerStyle.Square
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Meal, {sgListIndex.Value("amount")} grams"
                Case "AUTO_BASAL_DELIVERY"
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.AddXY(sgOaDateTime, MarkerRow)
                    Dim bolusAmount As String = sgListIndex.Value("bolusAmount")
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.MarkerBorderColor = Color.Black
                    Me.HomePageChart.Series(NameOf(MarkerSeries)).Points.Last.ToolTip = $"Basal, {bolusAmount.RoundDouble(3)} U"
                Case "AUTO_MODE_STATUS", "TIME_CHANGE"
                    'Stop
                Case Else
                    Stop
            End Select
        Next
        _initialized = True
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In SGs.WithIndex()
            sgDateTime = SGs.SafeGetSgDateTime(sgListIndex.Index)
            sgOaDateTime = sgDateTime.ToOADate()
            bgValue = CInt(sgListIndex.Value("sg"))
            If Math.Abs(bgValue - 0) < Single.Epsilon Then
                Me.HomePageChart.Series("Default").Points.AddXY(sgOaDateTime, InsulinRow)
                Me.HomePageChart.Series("Default").Points.Last().IsEmpty = True
            Else
                Me.HomePageChart.Series("Default").Points.AddXY(sgOaDateTime, bgValue)
                If bgValue > HighLimit Then
                    Me.HomePageChart.Series("Default").Points.Last.Color = Color.Lime
                ElseIf bgValue < LowLimit Then
                    Me.HomePageChart.Series("Default").Points.Last.Color = Color.Red
                Else
                    Me.HomePageChart.Series("Default").Points.Last.Color = Color.White
                End If

            End If

            Dim limitsLowValue As Integer = CInt(Limits(limitsIndexList(sgListIndex.Index))("lowLimit"))
            Dim limitsHighValue As Integer = CInt(Limits(limitsIndexList(sgListIndex.Index))("highLimit"))
            Me.HomePageChart.Series(NameOf(HighLimitSeries)).Points.AddXY(sgOaDateTime, limitsHighValue)
            Me.HomePageChart.Series(NameOf(LowLimitSeries)).Points.AddXY(sgOaDateTime, limitsLowValue)
        Next
    End Sub

    Private Sub UpdateHomeTabPage()
        If Not _initialized Then
            Exit Sub
        End If
        _initialized = False
        Me.UpdateActiveInsulin()
        Me.UpdateAutoModeShield()
        Me.UpdateCalibrationTimeRemaining()
        Me.UpdateInsulinLevel()
        Me.UpdatePumpBattery()
        Me.UpdateRemainingInsulin()
        Me.UpdateSensorLife()
        Me.UpdateTimeInRange()
        Me.UpdateTransmitterBatttery()

        Me.HomePageChart.Titles("Title1").Text = $"Summary of last 24 hours"
        Dim sgDateTime As Date = SGs.SafeGetSgDateTime(0)
        With _homePageChartChartArea.AxisX
            .Minimum = sgDateTime.ToOADate()
            .Maximum = SGs.SafeGetSgDateTime(SGs.Count - 1).ToOADate()
            .MajorGrid.IntervalType = DateTimeIntervalType.Hours
            .MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
            .MajorGrid.Interval = 1
            .IntervalType = DateTimeIntervalType.Hours
            .Interval = 2
            .ScrollBar.Enabled = True
        End With
        _homePageChartChartArea.CursorX.IsUserSelectionEnabled = True

        Dim limitsIndexList(SGs.Count - 1) As Integer
        Me.GetLimitsList(limitsIndexList)
        Me.UpdateHomePageSerieses(limitsIndexList)

        Application.DoEvents()
    End Sub

    Private Sub UpdateInsulinLevel()
        If ReservoirLevelPercent = 0 Then
            Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(0)
            Exit Sub
        End If
        Select Case ReservoirLevelPercent
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
        If Not ConduitSensorInRange Then
            Me.PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryUnknown
            Me.PumpBatteryRemainingLabel.Text = $"Unknown"
            Exit Sub
        End If

        Select Case MedicalDeviceBatteryLevelPercent
            Case > 66
                Me.PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryFull
                Me.PumpBatteryRemainingLabel.Text = $"High"
            Case > 50
                Me.PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryMedium
                Me.PumpBatteryRemainingLabel.Text = $"Medium"
            Case > 25
                Me.PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryLow
                Me.PumpBatteryRemainingLabel.Text = $"Low"
            Case = 0
                Me.PumpBatteryPictureBox.Image = My.Resources.Resources.PumpBatteryCritical
                Me.PumpBatteryRemainingLabel.Text = $"Critical"
        End Select
    End Sub

    Private Sub UpdateRemainingInsulin()
        Me.RemainingInsulinUnits.Text = $"{ReservoirRemainingUnits:N1} U"
    End Sub

    Private Sub UpdateSensorLife()
        If SensorDurationHours = 255 Then
            Me.SensorDaysLeftLabel.Text = $"???"
            Me.SensorTimeLefPictureBox.Image = My.Resources.Resources.SensorExpirationUnknown
            Me.SensorTimeLeftLabel.Text = ""
        ElseIf SensorDurationHours >= 24 Then
            Me.SensorDaysLeftLabel.Text = CStr(Math.Ceiling(SensorDurationHours / 24))
            Me.SensorTimeLefPictureBox.Image = My.Resources.Resources.SensorLifeOK
            Me.SensorTimeLeftLabel.Text = $"{Me.SensorDaysLeftLabel.Text} Days"
        Else
            If SensorDurationHours = 0 Then
                If SensorDurationMinutes = 0 Then
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLefPictureBox.Image = My.Resources.Resources.SensorExpired
                    Me.SensorTimeLeftLabel.Text = $"Expired"
                Else
                    Me.SensorDaysLeftLabel.Text = $"1"
                    Me.SensorTimeLefPictureBox.Image = My.Resources.Resources.SensorLifeNotOK
                    Me.SensorTimeLeftLabel.Text = $"{SensorDurationMinutes} Minutes"
                End If
            Else
                Me.SensorDaysLeftLabel.Text = $"1"
                Me.SensorTimeLefPictureBox.Image = My.Resources.Resources.SensorLifeNotOK
                Me.SensorTimeLeftLabel.Text = $"{SensorDurationHours + 1} Hours"
            End If
        End If
        Me.SensorDaysLeftLabel.Visible = True
    End Sub

    Private Sub UpdateTimeInRange()
        Me.TimeInRangeChart.Series("Default").Points.Clear()
        Me.TimeInRangeChart.Series("Default").Points.AddXY($"{AboveHyperLimit}% Above {HighLimit} {_bgUnitsString}", AboveHyperLimit / 100)
        Me.TimeInRangeChart.Series("Default").Points.Last().Color = Color.Orange
        Me.TimeInRangeChart.Series("Default").Points.Last().BorderColor = Color.Black
        Me.TimeInRangeChart.Series("Default").Points.Last().BorderWidth = 2
        Me.TimeInRangeChart.Series("Default").Points.AddXY($"{BelowHypoLimit}% Below {LowLimit} {_bgUnitsString}", BelowHypoLimit / 100)
        Me.TimeInRangeChart.Series("Default").Points.Last().Color = Color.Red
        Me.TimeInRangeChart.Series("Default").Points.Last().BorderColor = Color.Black
        Me.TimeInRangeChart.Series("Default").Points.Last().BorderWidth = 2
        Me.TimeInRangeChart.Series("Default").Points.AddXY($"{TimeInRange}% In Range", TimeInRange / 100)
        Me.TimeInRangeChart.Series("Default").Points.Last().Color = Color.LawnGreen
        Me.TimeInRangeChart.Series("Default").Points.Last().BorderColor = Color.Black
        Me.TimeInRangeChart.Series("Default").Points.Last().BorderWidth = 2
        Me.TimeInRangeChart.Series("Default")("PieLabelStyle") = "Disabled"
        Me.TimeInRangeChart.Series("Default")("PieStartAngle") = "270"

        If _bgUnitsString = "mg/dl" Then
            Me.AverageSGValueLabel.Text = AverageSG.ToString
        Else
            Me.AverageSGValueLabel.Text = AverageSG.RoundDouble(1).ToString()
        End If
        Me.AboveHighLimitValueLabel.Text = AboveHyperLimit.ToString()
        Me.BelowLowLimitValueLabel.Text = BelowHypoLimit.ToString()
        Me.TimeInRangeSummaryLabel.Left = Me.TimeInRangeSummaryPercentCharLabel.HorizontalCenterOn(Me.TimeInRangeSummaryLabel)
        Me.TimeInRangeSummaryLabel.Text = TimeInRange.ToString
        Me.TimeInRangeSummaryPercentCharLabel.Left = Me.TimeInRangeChart.HorizontalCenterOn(Me.TimeInRangeSummaryPercentCharLabel)
        Me.TimeInRangeValueLabel.Text = TimeInRange.ToString

    End Sub

    Private Sub UpdateTransmitterBatttery()
        Me.TransmatterBatterPercentLabel.Text = $"{GstBatteryLevel}%"
        If ConduitSensorInRange Then
            Select Case GstBatteryLevel
                Case 100
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryFull
                Case > 50
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryOK
                Case > 20
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryMedium
                Case > 0
                    Me.TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryLow
            End Select
        Else
            Me.TransmitterBatteryPictureBox.Image = My.Resources.Resources.TransmitterBatteryUnknown
            Me.TransmatterBatterPercentLabel.Text = $"???"
        End If

    End Sub

End Class
