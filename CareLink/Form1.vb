''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1
    Private WithEvents HomePageChart As Chart
    Private WithEvents TimeInRangeChart As Chart

    Private Shared ReadOnly LastAlarmFilter As New List(Of String) From {
        "code",
        "instanceId",
        "kind",
        "referenceGUID",
        "relativeOffset",
        "version"
        }

    Private Shared ReadOnly LastSgFilter As New List(Of String) From {
        "kind",
        "version",
        "relativeOffset"
        }

    Private Shared ReadOnly markersFilter As New List(Of String) From {
        "id",
        "index",
        "kind",
        "version",
        "relativeOffset"
        }

    Private Shared ReadOnly NotificationHistoryFilter As New List(Of String) From {
        "faultId",
        "id",
        "index",
        "instanceId",
        "kind",
        "version",
        "referenceGUID",
        "relativeOffset"
        }

    Private Shared ReadOnly sgsFilter As New List(Of String) From {
        "kind",
        "version",
        "relativeOffset"
        }

    ' do not rename or move up
    Private Shared ReadOnly zFilterList As New Dictionary(Of Integer, List(Of String)) From {
        {ItemIndexs.lastAlarm, LastAlarmFilter},
        {ItemIndexs.lastSG, LastSgFilter},
        {ItemIndexs.markers, markersFilter},
        {ItemIndexs.notificationHistory, NotificationHistoryFilter},
        {ItemIndexs.sgs, sgsFilter}
        }

    Private ReadOnly listOfSingleItems As New List(Of Integer) From {
                ItemIndexs.lastSG,
                ItemIndexs.lastAlarm,
                ItemIndexs.activeInsulin,
                ItemIndexs.sgs,
                ItemIndexs.limits,
                ItemIndexs.markers,
                ItemIndexs.notificationHistory,
                ItemIndexs.basal
            }

    Private ReadOnly loginDialog As New LoginForm1
    Private Client As CareLinkClient

#Region "Chart Objects"

    Private HomePageChartChartArea As ChartArea

#End Region

#Region "Messages"

    Private ReadOnly Messages As New Dictionary(Of String, String) From {
        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Calibrate sensor"},
        {"CALIBRATING", "Calibrating..."},
        {"CALIBRATION_REQUIRED", "Calibration required"},
        {"NO_ERROR_MESSAGE", ""},
        {"WAIT_TO_CALIBRATE", "Wait To Calibrate..."}
        }

#End Region

#Region "Variables to hold Sensor Values"

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
    Public SLastSensorTime As DateTime
    Public SMedicalDeviceTime As DateTime
    Public SystemStatusMessage As String
    Public TherapyAlgorithmState As Dictionary(Of String, String)
    Public TimeFormat As String
    Public TimeInRange As Integer
    Public TimeToNextCalibHours As Integer = -1
    Public TimeToNextCalibrationMinutes As Integer
    Public Version As String

#End Region

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

    Private Shared Sub Chart1_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles HomePageChart.PostPaint
        ' Painting series object
        Dim area As ChartArea = TryCast(sender, ChartArea)

        Dim highAreaRectangle As Rectangle = New Rectangle(New Point(35, 74), New Size(756, 240))
        Dim lowAreaRectangle As Rectangle = New Rectangle(New Point(35, 450), New Size(756, 61))
        Using b As New SolidBrush(Color.FromArgb(30 * 255 \ 100, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)
        End Using
        Using b As New SolidBrush(Color.FromArgb(30 * 255 \ 100, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, lowAreaRectangle)
        End Using
        If area IsNot Nothing Then

            Dim format As New StringFormat With {
                    .Alignment = StringAlignment.Center
                    }

            Dim rect As New RectangleF(area.Position.X, area.Position.Y, area.Position.Width, 6)

            rect = e.ChartGraphics.GetAbsoluteRectangle(rect)
            e.ChartGraphics.Graphics.DrawString(area.Name, New Font("Arial", 10), Brushes.Black, rect, format)

        End If
    End Sub

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Function DrawCenteredArc(backImage As Bitmap, arcPercentage As Double, Optional colorTable As Dictionary(Of String, Color) = Nothing, Optional segmentName As String = "") As Bitmap
        If arcPercentage < Double.Epsilon Then
            Return backImage
        End If
        Dim targetImage As Bitmap = backImage
        Dim myGraphics As Graphics = Graphics.FromImage(targetImage)
        Dim rect As New Rectangle(1, 1, backImage.Width - 2, backImage.Height - 2)
        myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim pen As Pen
        If colorTable Is Nothing Then
            pen = New Pen(GetColorFromTimeToNextCalib(), 2)
        Else
            pen = New Pen(colorTable(segmentName), 5)
        End If
        myGraphics.DrawArc(pen, rect, -90, -CInt(360 * arcPercentage))
        Return targetImage
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        InitializeHomePageChart()
        InitializeTimeInRangeChart()
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
            If zFilterList.ContainsKey(itemIndex) Then
                If zFilterList(itemIndex).Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0))
            If itemIndex = ItemIndexs.limits OrElse itemIndex = ItemIndexs.markers Then
                tableLevel1Blue.AutoSize = True
            End If
            Dim label As Label = New Label With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .Text = innerRow.Key,
                    .AutoSize = True
                    }

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = Json.LoadList(innerRow.Value)
                If itemIndex = ItemIndexs.notificationHistory Then
                    NotificationHistory.Add(innerRow.Key, innerJson1)
                End If
                If innerJson1.Count > 0 Then
                    Dim tableLevel2 As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .BorderStyle = BorderStyle.Fixed3D,
                            .ColumnCount = 1,
                            .AutoSizeMode = AutoSizeMode.GrowOnly,
                            .Dock = System.Windows.Forms.DockStyle.Fill,
                            .RowCount = innerJson1.Count
                            }
                    tableLevel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 80.0))
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson1.WithIndex()
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        tableLevel2.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 4 + dic.Keys.Count * 22))
                        Dim tableLevel3 As New TableLayoutPanel With {
                                .Anchor=AnchorStyles.Left Or AnchorStyles.Right,
                                .AutoScroll = False,
                                .AutoSize = True,
                                .AutoSizeMode = AutoSizeMode.GrowOnly,
                                .ColumnCount = 2,
                                .Dock = System.Windows.Forms.DockStyle.None
                                }
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            If zFilterList.ContainsKey(itemIndex) Then
                                If zFilterList(itemIndex).Contains(e.Value.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3.RowCount += 1
                            tableLevel3.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0))
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
                    tableLevel1Blue.Controls.AddRange({label, tableLevel2})
                Else
                    tableLevel1Blue.Controls.AddRange({label, New TextBox With {
                                               .Text = "",
                                               .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                               .AutoSize = True
                                               }})
                End If
            Else
                tableLevel1Blue.Controls.AddRange({label, New TextBox With {
                                           .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                           .AutoSize = True,
                                           .Text = innerRow.Value}})
            End If
        Next
        If itemIndex = ItemIndexs.lastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        End If
        Application.DoEvents()
    End Sub

    Private Sub InitializeHomePageChart()
        HomePageChart = New Chart With {
            .BackColor = System.Drawing.Color.WhiteSmoke,
            .BackGradientStyle = GradientStyle.TopBottom,
            .BackSecondaryColor = System.Drawing.Color.White,
            .BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105),
            .BorderlineDashStyle = ChartDashStyle.Solid,
            .BorderlineWidth = 2,
            .Location = New Point(3, BGImage.Height + 3),
            .Name = "chart1",
            .Size = New Size(TabPage1.ClientSize.Width - (TabPage1.ClientSize.Width - DisplayStartTimeLabel.Left), TabPage1.ClientSize.Height - (BGImage.Height + BGImage.Top + 6)),
            .TabIndex = 0
        }
        HomePageChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss
        HomePageChartChartArea = New ChartArea With {
            .BackColor = System.Drawing.Color.FromArgb(180, 23, 47, 19),
            .BackGradientStyle = GradientStyle.TopBottom,
            .BackSecondaryColor = System.Drawing.Color.FromArgb(180, 29, 56, 26),
            .BorderColor = System.Drawing.Color.FromArgb(64, 64, 64, 64),
            .BorderDashStyle = ChartDashStyle.Solid,
            .Name = "Default",
            .ShadowColor = System.Drawing.Color.Transparent
        }
        HomePageChartChartArea.AxisX.IsInterlaced = True
        HomePageChartChartArea.AxisX.IsMarginVisible = True
        HomePageChartChartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
        HomePageChartChartArea.AxisX.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold)
        HomePageChartChartArea.AxisX.LabelStyle.Format = "hh tt"
        HomePageChartChartArea.AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        HomePageChartChartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        HomePageChartChartArea.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black
        HomePageChartChartArea.AxisX.ScrollBar.Size = 10
        HomePageChartChartArea.AxisY.InterlacedColor = Color.FromArgb(120, Color.LightSlateGray)
        HomePageChartChartArea.AxisY.IsInterlaced = True
        HomePageChartChartArea.AxisY.IsMarginVisible = False
        HomePageChartChartArea.AxisY.IsStartedFromZero = False
        HomePageChartChartArea.AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold)
        HomePageChartChartArea.AxisY.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        HomePageChartChartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        HomePageChartChartArea.AxisY.Maximum = 400
        HomePageChartChartArea.AxisY.Minimum = 50
        HomePageChartChartArea.AxisY.ScrollBar.LineColor = System.Drawing.Color.Black
        HomePageChartChartArea.AxisY.ScrollBar.Size = 10
        HomePageChartChartArea.CursorX.IsUserEnabled = True
        HomePageChartChartArea.CursorX.IsUserSelectionEnabled = True
        HomePageChartChartArea.CursorY.IsUserEnabled = True
        HomePageChartChartArea.CursorY.IsUserSelectionEnabled = True

        HomePageChart.ChartAreas.Add(HomePageChartChartArea)
        HomePageChart.Legends.Add(New Legend With {
                                     .BackColor = System.Drawing.Color.Transparent,
                                     .Enabled = False,
                                     .Font = New Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold),
                                     .IsTextAutoFit = False,
                                     .Name = "Default"
                                     }
                                  )
        HomePageChart.Series.Add(New Series With {
                                    .BorderColor = System.Drawing.Color.FromArgb(180, 26, 59, 105),
                                    .BorderWidth = 4,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.FastLine,
                                    .Color = Color.White,
                                    .Legend = "Default",
                                    .Name = "Default",
                                    .ShadowColor = System.Drawing.Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    })
        HomePageChart.Series.Add(New Series With {
                                    .BorderColor = System.Drawing.Color.FromArgb(180, 26, 59, 105),
                                    .BorderWidth = 4,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.FastLine,
                                    .Color = Color.DeepPink,
                                    .Name = "Markers",
                                    .ShadowColor = System.Drawing.Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    })

        HomePageChart.Series.Add(New Series With {
                                    .BorderColor = System.Drawing.Color.FromArgb(180, 26, 59, 105),
                                    .BorderWidth = 4,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.FastLine,
                                    .Color = Color.Orange,
                                    .Name = "HighLimit",
                                    .ShadowColor = System.Drawing.Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    })
        HomePageChart.Series.Add(New Series With {
                                    .BorderColor = System.Drawing.Color.FromArgb(180, 26, 59, 105),
                                    .BorderWidth = 4,
                                    .ChartArea = "Default",
                                    .ChartType = SeriesChartType.FastLine,
                                    .Color = Color.Red,
                                    .Name = "LowLimit",
                                    .ShadowColor = System.Drawing.Color.Black,
                                    .XValueType = ChartValueType.DateTime,
                                    .YAxisType = AxisType.Secondary
                                    })
        HomePageChart.Series("Default").EmptyPointStyle.Color = Color.Transparent
        HomePageChart.Series("Default").EmptyPointStyle.BorderWidth = 4
        HomePageChart.Titles.Add(New Title With {
                                    .Font = New Font("Trebuchet MS", 12.0F, System.Drawing.FontStyle.Bold),
                                    .ForeColor = System.Drawing.Color.FromArgb(26, 59, 105),
                                    .Name = "Title1",
                                    .ShadowColor = System.Drawing.Color.FromArgb(32, 0, 0, 0),
                                    .ShadowOffset = 3
                                    }
                                 )
        TabPage1.Controls.Add(HomePageChart)
        Application.DoEvents()
    End Sub

    Private Sub InitializeTimeInRangeChart()
        TimeInRangeChart = New Chart With {
            .BackColor = System.Drawing.Color.Transparent,
            .BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.None,
            .BackSecondaryColor = System.Drawing.Color.Transparent,
            .BorderlineColor = System.Drawing.Color.Transparent,
            .BorderlineWidth = 0,
            .Size = New Size(280, 357)
        }
        TimeInRangeChart.BorderSkin.BackSecondaryColor = System.Drawing.Color.Transparent
        TimeInRangeChart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.None
        TimeInRangeChart.ChartAreas.Add(New ChartArea With {.Name = "TimeInRangeChartChartArea",
                                                            .BackColor = Color.Black})
        TimeInRangeChart.Location = New Point((TimeInRangeLabel.Left + (TimeInRangeLabel.Width \ 2)) - (TimeInRangeChart.Width \ 2), 79)
        TimeInRangeChart.Name = "Default"

        TimeInRangeChart.Series.Add(New Series With {
            .ChartArea = "TimeInRangeChartChartArea",
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut,
            .Name = "Default"})
        TimeInRangeChart.Series("Default")("DoughnutRadius") = "30"
        TimeInRangeChart.Legends.Add(New Legend With {
                                     .BackColor = System.Drawing.Color.Transparent,
                                     .Docking = Docking.Bottom,
                                     .Enabled = True,
                                     .ForeColor = Color.White,
                                     .TableStyle = LegendTableStyle.Tall,
                                     .Font = New Font("Trebuchet MS", 15.0!, System.Drawing.FontStyle.Regular),
                                     .IsTextAutoFit = False,
                                     .Name = "Default"
                                     }
                                  )

        TimeInRangeChart.Titles.Add(New Title With {
                                        .Name = "TimeInRangeChartTitle",
                                        .Text = "Time In Range Last 24 Hours"}
                                    )
        TabPage1.Controls.Add(TimeInRangeChart)
        Application.DoEvents()

    End Sub 'Page_Load

    Private Sub LoadDataTableWithSG(localRecentData As Dictionary(Of String, String))
        If localRecentData Is Nothing Then
            Exit Sub
        End If
        Cursor = Cursors.WaitCursor
        TableLayoutPanel1.Controls.Clear()
        TableLayoutPanel1.RowCount = localRecentData.Count - 8
        Dim currentRowIndex As Integer = 0
        Dim singleItem As Boolean
        Dim LayoutPanel1 As TableLayoutPanel
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
            Dim singleItemIndex As Integer = 0
            LayoutPanel1 = TableLayoutPanel1
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
                    LayoutPanel1 = TableLayoutPanelTop1
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.lastAlarm
                    LayoutPanel1 = TableLayoutPanelTop2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.activeInsulin
                    LayoutPanel1 = TableLayoutPanel2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.sgs
                    LayoutPanel1 = TableLayoutPanel3
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.limits
                    LayoutPanel1 = TableLayoutPanel4
                    LayoutPanel1.Controls.Clear()
                    LayoutPanel1.AutoSize = True
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True

                Case ItemIndexs.markers
                    LayoutPanel1 = TableLayoutPanel5
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.notificationHistory
                    LayoutPanel1 = TableLayoutPanel6
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.therapyAlgorithmState
                    ' handled elsewhere
                Case ItemIndexs.pumpBannerState
                    ' handled elsewhere
                Case ItemIndexs.basal
                    LayoutPanel1 = TableLayoutPanel7
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = rowIndex
                    LayoutPanel1.RowCount = 1
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
            LayoutPanel1.Visible = False

            If listOfSingleItems.Contains(rowIndex) OrElse singleItem Then
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
            LayoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
            If Not singleItem OrElse rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm Then
                LayoutPanel1.Controls.Add(New Label With {
                                                  .Text = $"{rowIndex} {row.Key}",
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True
                                                  }, 0, tableRelitiveRow)
            End If
            If row.Value.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
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
                    For Each Dic As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        Dim tableLevel1Blue As New TableLayoutPanel With {
                                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                .AutoScroll = False,
                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                .ColumnCount = 2,
                                .Margin = New Padding(0),
                                .Name = "InnerTable",
                                .Padding = New Padding(0)
                                }
                        LayoutPanel1.Controls.Add(tableLevel1Blue, 1, Dic.Index)
                        GetInnerTable(tableLevel1Blue, Dic.Value, CType(rowIndex, ItemIndexs))
                    Next
                Else
                    LayoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = ""}, If(singleItem, 0, 1), tableRelitiveRow)

                End If
            ElseIf row.Value.StartsWith("{") Then
                LayoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
                Dim innerJson As Dictionary(Of String, String) = Json.Loads(row.Value)
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
                        .Dock = System.Windows.Forms.DockStyle.Fill,
                        .Margin = New Padding(0),
                        .Name = "InnerTable",
                        .Padding = New Padding(0)
                        }
                LayoutPanel1.Controls.Add(tableLevel1Blue, If(singleItem AndAlso Not (rowIndex = ItemIndexs.lastSG OrElse rowIndex = ItemIndexs.lastAlarm), 0, 1), tableRelitiveRow)
                GetInnerTable(tableLevel1Blue, innerJson, CType(rowIndex, ItemIndexs))
            Else
                LayoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)
            End If
            LayoutPanel1.Visible = True
            Application.DoEvents()
        Next
        Cursor = Cursors.Default
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Timer1.Enabled = False
        If UseTestDataToolStripMenuItem.Checked Then
            RecentData = Json.Loads(IO.File.ReadAllText(IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")))
        Else
            loginDialog.ShowDialog()
            Client = loginDialog.Client
            If Client IsNot Nothing AndAlso Client.LoggedIn Then
                RecentData = Client.getRecentData()
            End If
            If RecentData Is Nothing Then
                Exit Sub
            End If
            Timer1.Interval = CType(New TimeSpan(0, 5, 0).TotalMilliseconds, Integer)
            Timer1.Enabled = True
        End If
        UpdateAllTabPages()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        RecentData = Client.getRecentData()
        If RecentData Is Nothing Then
            Cursor = Cursors.Default
            Exit Sub
        End If
        Timer1.Enabled = True

        UpdateAllTabPages()

        Application.DoEvents()
    End Sub

    Private Sub UpdateActiveInsulin()

        ActiveInsulinValue.Text = $"{ActiveInsulin("amount"):N3} U"
    End Sub

    Private Sub UpdateAllTabPages()
        If RecentData Is Nothing Then
            Exit Sub
        End If
        LoadDataTableWithSG(RecentData)
        If RecentData.Count > ItemIndexs.averageSGFloat + 1 Then
            Stop
        End If
        UpdateReminingInsulin()
        UpdateActiveInsulin()
        UpdateBGChart()
        UpdateAutoModeShield()
        UpdateInsulinLevel()
        UpdateCalibrationTimeRemaining()
        UpdateTIR()
    End Sub

    Private Sub UpdateAutoModeShield()

        BGImage.Location = New Point(HomePageChart.Left + (HomePageChart.Width \ 2) - (BGImage.Width \ 2), 3)
        CurrentBG.Parent = BGImage
        CurrentBG.BackColor = Color.Transparent
        CurrentBG.ForeColor = Color.White
        CurrentBG.Location = New Point((BGImage.Width \ 2) - (CurrentBG.Width \ 2), BGImage.Height \ 4)

        If LastSG("sg") <> "0" Then
            CurrentBG.Text = LastSG("sg")
            CurrentBG.Left = (BGImage.Width \ 2) - (CurrentBG.Width \ 2)
            SensorMessage.Visible = False
        Else
            CurrentBG.Text = $"---"
            SensorMessage.Text = Messages(SensorState)
            SensorMessage.Visible = True
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdateBGChart()
        HomePageChart.Titles("Title1").Text = $"Summary of last {TimeScaleNumericUpDown.Value} hours"
        HomePageChart.ChartAreas("Default").AxisX.Minimum = Date.Parse(SGs.First().Item("datetime").ToString()).ToOADate()
        HomePageChart.ChartAreas("Default").AxisX.Maximum = Date.Parse(SGs.Last().Item("datetime").ToString()).ToOADate()
        Dim previousHour As DateTime
        StartTimeComboBox.Items.Clear()
        HomePageChart.Series("Default").Points.Clear()
        HomePageChart.ChartAreas("Default").AxisX.MajorGrid.Interval = 1 / 24
        For Each SGList As IndexClass(Of Dictionary(Of String, String)) In SGs.WithIndex()
            Dim bgValue As Integer = CInt(SGList.Value("sg"))

            Dim sgDateTime As Date = Date.Parse(SGList.Value("datetime"))
            Dim currentHour As Date = sgDateTime.RoundToHour()
            If SGList.IsFirst Then
                previousHour = sgDateTime
                StartTimeComboBox.Items.Add($"{currentHour:hh tt}")
            ElseIf previousHour.Hour <> currentHour.Hour Then
                StartTimeComboBox.Items.Add($"{currentHour:hh tt}")
                previousHour = currentHour
            End If
            StartTimeComboBox.SelectedIndex = 0
            If bgValue = 0 Then
                HomePageChart.Series("Default").Points.AddXY(sgDateTime, 50)
                HomePageChart.Series("Default").Points.Last().IsEmpty = True
            Else
                HomePageChart.Series("Default").Points.AddXY(sgDateTime, bgValue)
            End If

            HomePageChart.Series("Markers").Points.AddXY(sgDateTime, 399)

            HomePageChart.Series("HighLimit").Points.AddXY(sgDateTime, 180)
            HomePageChart.Series("LowLimit").Points.AddXY(sgDateTime, 60)
            Application.DoEvents()
        Next
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        If TimeToNextCalibHours = -1 Then
            Exit Sub
        End If
        If TimeToNextCalibHours <= 2 Then
            CalibrationDueImage.Image = DrawCenteredArc(My.Resources.Resources.CalibrationDotRed, TimeToNextCalibHours / 12)
        Else
            CalibrationDueImage.Image = DrawCenteredArc(My.Resources.Resources.CalibrationDot, TimeToNextCalibHours / 12)
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdateInsulinLevel()
        If ReservoirLevelPercent = 0 Then
            InsulinLevelPictureBox.Image = ImageList1.Images(0)
            Exit Sub
        End If
        Select Case ReservoirLevelPercent
            Case > 85
                InsulinLevelPictureBox.Image = ImageList1.Images(7)
            Case > 71
                InsulinLevelPictureBox.Image = ImageList1.Images(6)
            Case > 57
                InsulinLevelPictureBox.Image = ImageList1.Images(5)
            Case > 43
                InsulinLevelPictureBox.Image = ImageList1.Images(4)
            Case > 29
                InsulinLevelPictureBox.Image = ImageList1.Images(3)
            Case > 15
                InsulinLevelPictureBox.Image = ImageList1.Images(2)
            Case > 1
                InsulinLevelPictureBox.Image = ImageList1.Images(1)
            Case Else
                InsulinLevelPictureBox.Image = ImageList1.Images(0)
        End Select
        Application.DoEvents()
    End Sub

    Private Sub UpdateReminingInsulin()

        RemainingInsulinUnits.Text = $"{ReservoirRemainingUnits:N1} U"
    End Sub

    Private Sub UpdateTIR()
        TimeInRangeChart.Series("Default").Points.Clear()
        TimeInRangeChart.Series("Default").Points.AddXY($"{AboveHyperLimit}% Above 180 mg/dl", AboveHyperLimit / 100)
        TimeInRangeChart.Series("Default").Points.Last().Color = Color.Orange
        TimeInRangeChart.Series("Default").Points.AddXY($"{TimeInRange}% In Range", TimeInRange / 100)
        TimeInRangeChart.Series("Default").Points.Last().Color = Color.LawnGreen
        TimeInRangeChart.Series("Default").Points.AddXY($"{BelowHypoLimit}% Below 70 mg/dl", BelowHypoLimit / 100)
        TimeInRangeChart.Series("Default")("PieLabelStyle") = "Disabled"
        TimeInRangeChart.Series("Default").Points.Last().Color = Color.Red
        AverageSGLabel.Text = $"{AverageSG} mg/dl"
    End Sub

End Class
