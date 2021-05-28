' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1
    ' ReSharper disable InconsistentNaming
    Public WithEvents HighLimitSeries As Series
    Public WithEvents HomePageChart As Chart
    Public WithEvents LowLimitSeries As Series
    Public WithEvents MarkerSeries As Series
    Public WithEvents TimeInRangeChart As Chart
    ' ReSharper restore InconsistentNaming

    Private Shared ReadOnly s_lastAlarmFilter As New List(Of String) From {
        "code",
        "instanceId",
        "kind",
        "referenceGUID",
        "relativeOffset",
        "version"
        }

    Private Shared ReadOnly s_lastSgFilter As New List(Of String) From {
        "kind",
        "version",
        "relativeOffset"
        }

    Private Shared ReadOnly s_markersFilter As New List(Of String) From {
        "id",
        "index",
        "kind",
        "version",
        "relativeOffset"
        }

    Private Shared ReadOnly s_notificationHistoryFilter As New List(Of String) From {
        "faultId",
        "id",
        "index",
        "instanceId",
        "kind",
        "version",
        "referenceGUID",
        "relativeOffset"
        }

    Private Shared ReadOnly s_sgsFilter As New List(Of String) From {
        "kind",
        "version",
        "relativeOffset"
        }

    ' do not rename or move up
    Private Shared ReadOnly s_zFilterList As New Dictionary(Of Integer, List(Of String)) From {
        {ItemIndexs.lastAlarm, s_lastAlarmFilter},
        {ItemIndexs.lastSG, s_lastSgFilter},
        {ItemIndexs.markers, s_markersFilter},
        {ItemIndexs.notificationHistory, s_notificationHistoryFilter},
        {ItemIndexs.sgs, s_sgsFilter}
        }

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
    Private ReadOnly _mealImage As Bitmap = My.Resources.FoodImage
    Private _client As CareLinkClient
    Private _initialized As Boolean = False
    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private _updating As Boolean = False

#Region "Chart Objects"

    Private _homePageChartChartArea As ChartArea

#End Region

#Region "Messages"

    Private ReadOnly _messages As New Dictionary(Of String, String) From {
        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Calibrate sensor"},
        {"CALIBRATING", "Calibrating ..."},
        {"CALIBRATION_REQUIRED", "Calibration required"},
        {"NO_ERROR_MESSAGE", ""},
        {"WARM_UP", "Sensor warm up..."},
        {"WAIT_TO_CALIBRATE", "Wait To Calibrate..."}
        }

    ' Add additional units here, default
    Private ReadOnly _unitsStrings As New Dictionary(Of String, String) From {
        {"MGDL", "mg/dl"}
        }

    Private _bgUnitsString As String

#End Region

#Region "Variables to hold Sensor Values"

    ' ReSharper disable InconsistentNaming
    Public AboveHyperLimit As Integer
    Public ActiveInsulin As Dictionary(Of String, String)
    Public AverageSG As Integer
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
    Public RecentData As Dictionary(Of String, String)
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
    Public TimeToNextCalibHours As Integer = -1
    Public TimeToNextCalibrationMinutes As Integer
    Public Version As String
    ' ReSharper restore InconsistentNaming

#End Region

    ' ReSharper disable InconsistentNaming
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
        timeInRange = 50
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

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Shared Sub PaintMarker(e As ChartPaintEventArgs, markerImage As Bitmap, marketDictionary As Dictionary(Of Double, Integer), imageYOffset As Integer)
        ' Draw the cloned portion of the Bitmap object.
        Dim halfHeight As Single = CSng(markerImage.Height / 2)
        Dim halfWidth As Single = CSng(markerImage.Width / 2)
        For Each markerKvp As KeyValuePair(Of Double, Integer) In marketDictionary
            Dim imagePosition As RectangleF = RectangleF.Empty
            imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, markerKvp.Key))
            imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, markerKvp.Value))
            imagePosition = e.ChartGraphics.GetAbsoluteRectangle(imagePosition)
            imagePosition.Width = markerImage.Width
            imagePosition.Height = markerImage.Height
            imagePosition.Y -= halfHeight
            imagePosition.X -= halfWidth
            ' Draw image
            e.ChartGraphics.Graphics.DrawImage(markerImage, imagePosition.X, imagePosition.Y + imageYOffset)
        Next
    End Sub

    Private Shared Function SafeGetSgDateTime(sgList As IReadOnlyList(Of Dictionary(Of String, String)), index As Integer) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            Stop
        End If
        If sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString)
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString.Split("-")(0))
        Else
            sgDateTime = Now
        End If
        Return sgDateTime
    End Function

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

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.ShieldUnitsLabel.Parent = Me.ShieldPictureBox
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.InitializeHomePageChart()
        Me.InitializeTimeInRangeChart()
    End Sub

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
                            .ColumnCount = 1,
                            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            .Dock = DockStyle.Top,
                            .RowCount = innerJson1.Count
                            }
                    tableLevel2.RowStyles.Add(New RowStyle())
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
            If tableLevel1Blue.RowCount > 5 Then
                tableLayoutParent.AutoScroll = True
                tableLayoutParent.HorizontalScroll.Visible = False
                tableLayoutParent.Height = 22 * tableLevel1Blue.RowCount
            Else
                tableLevel1Blue.RowCount += 1
            End If
        ElseIf itemIndex = ItemIndexs.notificationHistory Then
            tableLevel1Blue.RowStyles(1).SizeType = SizeType.AutoSize
        End If
        Application.DoEvents()
    End Sub

    Private Function GetMilitaryHour() As Integer

        Dim splitTime As String() = Me.StartTimeComboBox.SelectedItem.ToString().Split(" ")
        Dim hour As Integer = CInt(splitTime(0)) - 1
        Return hour + If(splitTime(1) = "PM", 12, 0)
    End Function

    Private Sub HomePageChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomePageChart.PostPaint
        If Not _initialized Then Exit Sub
        Dim imagePosition As RectangleF = RectangleF.Empty
        imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, SafeGetSgDateTime(SGs, 0).ToOADate))
        imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, 400))
        imagePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, 180)))) - imagePosition.Y
        imagePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, SafeGetSgDateTime(SGs, SGs.Count - 1).ToOADate)) - imagePosition.X
        imagePosition = e.ChartGraphics.GetAbsoluteRectangle(imagePosition)

        Dim highAreaRectangle As New Rectangle(New Point(CInt(imagePosition.X), CInt(imagePosition.Y)), New Size(CInt(imagePosition.Width), 292))

        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)
        End Using
        Dim lowAreaRectangle As New Rectangle(New Point(CInt(imagePosition.X), 504), New Size(CInt(imagePosition.Width), 25))
        Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, lowAreaRectangle)
        End Using
        PaintMarker(e, _mealImage, _markerMealDictionary, 0)
        PaintMarker(e, _insulinImage, _markerInsulinDictionary, -6)
    End Sub

    Private Sub InitializeHomePageChart()
        Me.HomePageChart = New Chart With {
             .BackColor = Color.WhiteSmoke,
             .BackGradientStyle = GradientStyle.TopBottom,
             .BackSecondaryColor = Color.White,
             .BorderlineColor = Color.FromArgb(26, 59, 105),
             .BorderlineDashStyle = ChartDashStyle.Solid,
             .BorderlineWidth = 2,
             .Location = New Point(3, Me.ShieldPictureBox.Height + 3),
             .Name = "chart1",
             .Size = New Size(Me.TabPage1.ClientSize.Width - (Me.TabPage1.ClientSize.Width - Me.DisplayStartTimeLabel.Left), Me.TabPage1.ClientSize.Height - (Me.ShieldPictureBox.Height + Me.ShieldPictureBox.Top + 6)),
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
        _homePageChartChartArea.AxisX.LabelStyle.Format = "hh tt"
        _homePageChartChartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisX.ScrollBar.LineColor = Color.Black
        _homePageChartChartArea.AxisX.ScrollBar.Size = 10
        _homePageChartChartArea.AxisY.IsStartedFromZero = False
        _homePageChartChartArea.AxisY.ScaleBreakStyle = New AxisScaleBreakStyle() With {
                                                            .Enabled = True,
                                                            .StartFromZero = StartFromZero.No,
                                                            .BreakLineStyle = BreakLineStyle.Straight
                                                            }
        _homePageChartChartArea.AxisY.InterlacedColor = Color.FromArgb(120, Color.LightSlateGray)
        _homePageChartChartArea.AxisY.IsInterlaced = True
        _homePageChartChartArea.AxisY.IsMarginVisible = False
        _homePageChartChartArea.AxisY.IsStartedFromZero = False
        _homePageChartChartArea.AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        _homePageChartChartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
        _homePageChartChartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount
        _homePageChartChartArea.AxisY.Interval = 50
        _homePageChartChartArea.AxisY.IsLabelAutoFit = False
        _homePageChartChartArea.AxisY.MajorTickMark = New TickMark() With {
                                                                .Interval = 50,
                                                                .Enabled = True}
        _homePageChartChartArea.AxisY.ScrollBar.LineColor = Color.Black
        _homePageChartChartArea.CursorX.IsUserEnabled = True
        _homePageChartChartArea.CursorX.IsUserSelectionEnabled = True
        _homePageChartChartArea.CursorY.IsUserEnabled = True
        _homePageChartChartArea.CursorY.IsUserSelectionEnabled = True

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
                                    .ChartType = SeriesChartType.FastLine,
                                    .Color = Color.White,
                                    .Legend = "Default",
                                    .Name = "Default",
                                    .ShadowColor = Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    })
        Me.MarkerSeries = New Series With {
            .BorderColor = Color.DeepPink,
            .BorderWidth = 5,
            .ChartArea = "Default",
            .ChartType = SeriesChartType.Point,
            .Color = Color.DeepPink,
            .Name = "Markers",
            .ShadowColor = Color.Black,
            .XValueType = ChartValueType.DateTime,
            .YAxisType = AxisType.Secondary
            }

        Me.HomePageChart.Series.Add(Me.MarkerSeries)

        Me.HighLimitSeries = New Series With {
                                    .BorderColor = Color.FromArgb(180, Color.Orange),
                                    .BorderWidth = 2,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.FastLine,
                                    .Color = Color.Orange,
                                    .Name = "HighLimit",
                                    .ShadowColor = Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    }
        Me.HomePageChart.Series.Add(Me.HighLimitSeries)
        Me.LowLimitSeries = New Series With {
                                    .BorderColor = Color.FromArgb(180, Color.Red),
                                    .BorderWidth = 2,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.Line,
                                    .Color = Color.Red,
                                    .Name = "LowLimit",
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

    Private Sub InitializeStartTimeComboBox()
        Dim hourFound As Boolean = False
        Dim selectedIndex As Integer = 0
        Dim selectedItem As String = ""
        If Me.StartTimeComboBox.SelectedItem IsNot Nothing Then
            selectedItem = CStr(Me.StartTimeComboBox.SelectedItem)
        End If
        Dim previousHour As Date
        Me.StartTimeComboBox.Items.Clear()
        For Each sgList As IndexClass(Of Dictionary(Of String, String)) In SGs.WithIndex()
            Dim sgDateTime As Date = SafeGetSgDateTime(SGs, sgList.Index)
            Dim currentHour As Date = sgDateTime.RoundToHour()
            If sgList.IsFirst Then
                previousHour = sgDateTime
                Me.StartTimeComboBox.Items.Add($"{currentHour:hh tt}")
            ElseIf previousHour.Hour <> currentHour.Hour Then
                Me.StartTimeComboBox.Items.Add($"{currentHour:hh tt}")
                previousHour = currentHour
            End If
            If Me.StartTimeComboBox.Items(Me.StartTimeComboBox.Items.Count - 1).ToString() = selectedItem AndAlso Not hourFound Then
                selectedIndex = Me.StartTimeComboBox.Items.Count - 1
                hourFound = True
            End If
        Next
        Me.StartTimeComboBox.SelectedIndex = selectedIndex
    End Sub

    Private Sub InitializeTimeInRangeChart()
        Me.TimeInRangeChart = New Chart With {
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
        Me.TimeInRangeChart.Location = New Point(Me.TimeInRangeSummaryLabel.FindHorizontalMidpoint - (Me.TimeInRangeChart.Width \ 2), 79)
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

    Private Sub LoadDataTableWithSg(localRecentData As Dictionary(Of String, String))
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
                    TimeToNextCalibHours = CInt(row.Value)
                Case ItemIndexs.calibStatus
                    CalibStatus = row.Value
                Case ItemIndexs.bgUnits
                    BgUnits = row.Value
                    _bgUnitsString = _unitsStrings(BgUnits)
                    Me.AverageSGUnitsLabel.Text = _bgUnitsString
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
            layoutPanel1.Visible = False

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
            If row.Value.StartsWith("[") Then
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
                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                .ColumnCount = 2,
                                .Margin = New Padding(0),
                                .Name = "InnerTable",
                                .Padding = New Padding(0)
                                }
                        layoutPanel1.Controls.Add(tableLevel1Blue, 1, dic.Index)
                        Me.GetInnerTable(tableLevel1Blue, dic.Value, CType(rowIndex, ItemIndexs))
                    Next
                Else
                    layoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = ""}, If(singleItem, 0, 1), tableRelitiveRow)

                End If
            ElseIf row.Value.StartsWith("{") Then
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
                        .AutoScroll = False,
                        .AutoSize = True,
                        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        .ColumnCount = 2,
                        .Dock = DockStyle.Fill,
                        .Margin = New Padding(0),
                        .Name = "InnerTable",
                        .Padding = New Padding(0)
                        }
                layoutPanel1.Controls.Add(tableLevel1Blue, If(singleItem AndAlso Not (rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm), 0, 1), tableRelitiveRow)
                Me.GetInnerTable(tableLevel1Blue, innerJson, CType(rowIndex, ItemIndexs))
            Else
                layoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)
            End If
            layoutPanel1.Visible = True
            Application.DoEvents()
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Me.Timer1.Enabled = False
        If Me.UseTestDataToolStripMenuItem.Checked Then
            RecentData = Loads(IO.File.ReadAllText(IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")))
        Else
            _loginDialog.ShowDialog()
            _client = _loginDialog.Client
            If _client IsNot Nothing AndAlso _client.LoggedIn Then
                RecentData = _client.getRecentData()
            End If
            If RecentData Is Nothing Then
                Exit Sub
            End If
            Me.Timer1.Interval = CType(New TimeSpan(0, 5, 0).TotalMilliseconds, Integer)
            Me.Timer1.Enabled = True
        End If
        Me.UpdateAllTabPages()

    End Sub

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If TimeToNextCalibrationMinutes > 0 AndAlso TimeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {Now.AddMinutes(TimeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub

    Private Sub StartTimeComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles StartTimeComboBox.SelectedValueChanged
        Me.UpdateBgChart()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Timer1.Enabled = False
        RecentData = _client.getRecentData()
        If RecentData Is Nothing Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        Me.Timer1.Enabled = True

        Me.UpdateAllTabPages()

        Application.DoEvents()
    End Sub

    Private Sub TimeScaleNumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles TimeScaleNumericUpDown.ValueChanged
        Me.UpdateBgChart()
    End Sub

    Private Sub UpdateActiveInsulin()
        Me.ActiveInsulinValue.Text = $"{ActiveInsulin("amount"):N3} U"
    End Sub

    Private Sub UpdateAllTabPages()
        If RecentData Is Nothing OrElse _updating Then
            Exit Sub
        End If
        _updating = True
        Me.LoadDataTableWithSg(RecentData)
        If RecentData.Count > ItemIndexs.averageSGFloat + 1 Then
            Stop
        End If
        Me.InitializeStartTimeComboBox()
        _initialized = True
        Me.UpdateRemainingInsulin()
        Me.UpdateActiveInsulin()
        Me.UpdateBgChart()
        Me.UpdateAutoModeShield()
        Me.UpdateInsulinLevel()
        Me.UpdateCalibrationTimeRemaining()
        Me.UpdateTimeInRange()
        _updating = False
    End Sub

    Private Sub UpdateAutoModeShield()
        Me.SensorMessage.Location = New Point(Me.ShieldPictureBox.Left + (Me.ShieldPictureBox.Width \ 2) - (Me.SensorMessage.Width \ 2), Me.SensorMessage.Top)
        If LastSG("sg") <> "0" Then
            Me.SensorMessage.Visible = False
            Me.ShieldPictureBox.Image = My.Resources.Shield
            Me.CurrentBG.Text = LastSG("sg")
            Me.CurrentBG.Parent = Me.ShieldPictureBox
            Me.CurrentBG.Visible = True
            Me.CurrentBG.Location = New Point((Me.ShieldPictureBox.Width \ 2) - (Me.CurrentBG.Width \ 2), Me.ShieldPictureBox.Height \ 4)
            Me.ShieldUnitsLabel.Visible = True
            Me.ShieldUnitsLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.Parent = Me.ShieldPictureBox
            Me.ShieldUnitsLabel.Left = (Me.ShieldPictureBox.Width \ 2) - (Me.ShieldUnitsLabel.Width \ 2)
            Application.DoEvents()
            Me.CurrentBG.Visible = True
            Application.DoEvents()
        Else
            Me.CurrentBG.Visible = False
            Me.ShieldUnitsLabel.Visible = False
            Me.ShieldPictureBox.Image = My.Resources.Shield_Disabled
            Me.SensorMessage.Visible = True
            Me.SensorMessage.Parent = Me.ShieldPictureBox
            Me.SensorMessage.Left = 0
            Me.SensorMessage.BackColor = Color.Transparent
            Me.SensorMessage.Text = _messages(SensorState)
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdateBgChart()
        If Not _initialized Then
            Exit Sub
        End If
        Dim foundStartTime As Boolean = False
        If Me.TimeScaleNumericUpDown.Value = 24 Then
            Me.HomePageChart.Titles("Title1").Text = $"Summary of last 24 hours"
        Else
            Dim startIndex As Integer = Me.StartTimeComboBox.SelectedIndex
            Dim endIndex As Integer = (Me.StartTimeComboBox.SelectedIndex + CInt(Me.TimeScaleNumericUpDown.Value)) Mod Me.StartTimeComboBox.Items.Count
            Me.HomePageChart.Titles("Title1").Text = $"Summary from {Me.StartTimeComboBox.Items(startIndex)} till {Me.StartTimeComboBox.Items(endIndex)} hours"
        End If
        Me.HomePageChart.ChartAreas("Default").AxisX.Minimum = SafeGetSgDateTime(SGs, 0).ToOADate()
        Me.HomePageChart.ChartAreas("Default").AxisX.Maximum = SafeGetSgDateTime(SGs, SGs.Count - 1).ToOADate()
        Me.HomePageChart.ChartAreas("Default").AxisY.Maximum = 400
        Me.HomePageChart.ChartAreas("Default").AxisY.Minimum = 50
        Me.HomePageChart.ChartAreas("Default").AxisX2.Minimum = SafeGetSgDateTime(SGs, 0).ToOADate()
        Me.HomePageChart.ChartAreas("Default").AxisX2.Maximum = SafeGetSgDateTime(SGs, SGs.Count - 1).ToOADate()
        Me.HomePageChart.ChartAreas("Default").AxisY2.Maximum = 400
        Me.HomePageChart.ChartAreas("Default").AxisY2.Minimum = 50
        Me.HomePageChart.ChartAreas("Default").AxisY.IsStartedFromZero = False
        Me.HomePageChart.ChartAreas("Default").AxisY2.IsStartedFromZero = False
        Me.HomePageChart.Series("Default").Points.Clear()
        Me.HomePageChart.Series("Markers").Points.Clear()
        Me.HomePageChart.Series("HighLimit").Points.Clear()
        Me.HomePageChart.Series("LowLimit").Points.Clear()
        Application.DoEvents()
        Me.HomePageChart.ChartAreas("Default").AxisX.MajorGrid.Interval = 1 / Me.StartTimeComboBox.Items.Count
        Dim bgValue As Integer
        Dim sgDateTime As Date
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In SGs.WithIndex()
            sgDateTime = SafeGetSgDateTime(SGs, sgListIndex.Index)
            If Not foundStartTime Then
                If sgDateTime.Hour >= Me.GetMilitaryHour() Then
                    foundStartTime = True
                Else
                    Continue For
                End If
            Else
                Exit For
            End If
        Next
        foundStartTime = False
        _markerInsulinDictionary.Clear()
        _markerMealDictionary.Clear()
        _initialized = False
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In Markers.WithIndex()
            sgDateTime = SafeGetSgDateTime(Markers, sgListIndex.Index)
            If Not foundStartTime Then
                If sgDateTime.Hour >= Me.GetMilitaryHour() Then
                    foundStartTime = True
                Else
                    Continue For
                End If
            End If
            Dim bgValueString As String = ""
            If sgListIndex.Value.TryGetValue("value", bgValueString) Then
                bgValue = CInt(bgValueString)
                If bgValue < 50 Then Stop
            End If
            Select Case sgListIndex.Value("type")
                Case "BG_READING"
                    bgValue = CInt(sgListIndex.Value("value"))
                    Me.HomePageChart.Series("Markers").Points.AddXY(sgDateTime.ToOADate(), bgValue)
                    Me.HomePageChart.Series("Markers").Points.Last.MarkerBorderWidth = 0
                    Me.HomePageChart.Series("Markers").Points.Last.MarkerSize = 6
                    Me.HomePageChart.Series("Markers").Points.Last.Color = Color.Gray
                    Me.HomePageChart.Series("Markers").Points.Last.ToolTip = $"Blood Glucose Not used For calibration {sgListIndex.Value("value")} {_bgUnitsString}"
                Case "CALIBRATION"
                    Me.HomePageChart.Series("Markers").Points.AddXY(sgDateTime.ToOADate(), bgValue)
                    Me.HomePageChart.Series("Markers").Points.Last.Color = Color.Red
                    Me.HomePageChart.Series("Markers").Points.Last.ToolTip = $"Blood Glucose Calibration {If(CBool(sgListIndex.Value("calibrationSuccess")), "Accepted", "Not Accepted")}, {sgListIndex.Value("value")} {_bgUnitsString}"
                Case "INSULIN"
                    _markerInsulinDictionary.Add(sgDateTime.ToOADate(), 399)
                    Me.HomePageChart.Series("Markers").Points.AddXY(sgDateTime.ToOADate(), 399)
                    Me.HomePageChart.Series("Markers").Points.Last.Color = Color.FromArgb(30, Color.LightBlue)
                    Me.HomePageChart.Series("Markers").Points.Last.MarkerBorderWidth = 0
                    Me.HomePageChart.Series("Markers").Points.Last.MarkerSize = 30
                    Me.HomePageChart.Series("Markers").Points.Last.ToolTip = $"{sgListIndex.Value("programmedFastAmount")} U"
                Case "MEAL"
                    _markerMealDictionary.Add(sgDateTime.ToOADate(), 50)
                    Me.HomePageChart.Series("Markers").Points.AddXY(sgDateTime.ToOADate(), 50)
                    Me.HomePageChart.Series("Markers").Points.Last.Color = Color.FromArgb(30, Color.Yellow)
                    Me.HomePageChart.Series("Markers").Points.Last.MarkerBorderWidth = 0
                    Me.HomePageChart.Series("Markers").Points.Last.MarkerSize = 30
                    Me.HomePageChart.Series("Markers").Points.Last.ToolTip = $"Meal {sgListIndex.Value("amount")} grams"
                Case "AUTO_BASAL_DELIVERY"
                    Me.HomePageChart.Series("Markers").Points.AddXY(sgDateTime.ToOADate(), 399)
                    Dim bolusAmount As String = sgListIndex.Value("bolusAmount")
                    Me.HomePageChart.Series("Markers").Points.Last.ToolTip = $"{bolusAmount} U"
                Case "AUTO_MODE_STATUS"
                Case Else
                    Stop
            End Select
        Next
        _initialized = True
        foundStartTime = False
        For Each sgListIndex As IndexClass(Of Dictionary(Of String, String)) In SGs.WithIndex()
            sgDateTime = SafeGetSgDateTime(SGs, sgListIndex.Index)
            If Not foundStartTime Then
                If sgDateTime.Hour >= Me.GetMilitaryHour() Then
                    foundStartTime = True
                Else
                    Continue For
                End If
            End If
            bgValue = CInt(sgListIndex.Value("sg"))
            If bgValue = 0 Then
                Me.HomePageChart.Series("Default").Points.AddXY(sgDateTime.ToOADate(), 50)
                Me.HomePageChart.Series("Default").Points.Last().IsEmpty = True
            Else
                Me.HomePageChart.Series("Default").Points.AddXY(sgDateTime.ToOADate(), bgValue)
            End If

            Me.HomePageChart.Series("HighLimit").Points.AddXY(sgDateTime.ToOADate(), 180)
            Me.HomePageChart.Series("LowLimit").Points.AddXY(sgDateTime.ToOADate(), 70)
        Next

        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        If TimeToNextCalibHours = -1 Then
            Exit Sub
        End If
        If TimeToNextCalibHours < 1 Then
            Me.CalibrationDueImage.Image = Me.DrawCenteredArc(My.Resources.CalibrationDotRed, TimeToNextCalibHours / 12)
        Else
            Me.CalibrationDueImage.Image = Me.DrawCenteredArc(My.Resources.CalibrationDot, TimeToNextCalibHours / 12)
        End If

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

    Private Sub UpdateRemainingInsulin()

        Me.RemainingInsulinUnits.Text = $"{ReservoirRemainingUnits:N1} U"
    End Sub

    Private Sub UpdateTimeInRange()
        Me.TimeInRangeChart.Series("Default").Points.Clear()
        Me.TimeInRangeChart.Series("Default").Points.AddXY($"{AboveHyperLimit}% Above 180 {_bgUnitsString}", AboveHyperLimit / 100)
        Me.TimeInRangeChart.Series("Default").Points.Last().Color = Color.Orange
        Me.TimeInRangeChart.Series("Default").Points.AddXY($"{BelowHypoLimit}% Below 70 {_bgUnitsString}", BelowHypoLimit / 100)
        Me.TimeInRangeChart.Series("Default").Points.Last().Color = Color.Red
        Me.TimeInRangeChart.Series("Default").Points.AddXY($"{TimeInRange}% In Range", TimeInRange / 100)
        Me.TimeInRangeChart.Series("Default").Points.Last().Color = Color.LawnGreen
        Me.TimeInRangeChart.Series("Default")("PieLabelStyle") = "Disabled"
        Me.TimeInRangeChart.Series("Default")("PieStartAngle") = "270"

        Me.Above180UnitsLabel.Text = _bgUnitsString
        Me.Above180ValueLabel.Text = AboveHyperLimit.ToString

        Me.TimeInRangeValueLabel.Text = TimeInRange.ToString

        Me.TimeInRangeSummaryLabel.Text = TimeInRange.ToString
        Me.TimeInRangeSummaryPercentCharLabel.Left = Me.TimeInRangeChart.HorizontalCenterOn(Me.TimeInRangeSummaryPercentCharLabel)
        Me.TimeInRangeSummaryLabel.Left = Me.TimeInRangeSummaryPercentCharLabel.HorizontalCenterOn(Me.TimeInRangeSummaryLabel)

        Me.Below70PValueLabel.Text = BelowHypoLimit.ToString
        Me.Below70UnitsLabel.Text = _bgUnitsString

        Me.AverageSGValueLabel.Text = AverageSG.ToString
        Me.AverageSGUnitsLabel.Text = _bgUnitsString
    End Sub

End Class
