''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1
    Private WithEvents Chart1 As Chart = New Chart()

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
        {ItemIndexs.LastAlarm, LastAlarmFilter},
        {ItemIndexs.LastSG, LastSgFilter},
        {ItemIndexs.Markers, markersFilter},
        {ItemIndexs.NotificationHistory, NotificationHistoryFilter},
        {ItemIndexs.SGs, sgsFilter}
        }

    Private ReadOnly chartArea1 As New ChartArea()

    Private ReadOnly legend1 As New Legend()

    Private ReadOnly listOfSingleItems As New List(Of Integer) From {
                ItemIndexs.LastSG,
                ItemIndexs.LastAlarm,
                ItemIndexs.ActiveInsulin,
                ItemIndexs.SGs,
                ItemIndexs.Limits,
                ItemIndexs.Markers,
                ItemIndexs.NotificationHistory,
                ItemIndexs.Basal
            }

    Private ReadOnly loginDialog As New LoginForm1
    Private ReadOnly series1 As New Series()
    Private ReadOnly title1 As New Title()

    Private Client As CareLinkClient
    Public RecentData As Dictionary(Of String, String)

    Public LastSensorTS As String ' 0
    Public MedicalDeviceTimeAsString As String ' 1
    Public LastSensorTSAsString As String ' 2
    Public Kind As String ' 3
    Public Version As String ' 4
    Public PumpModelNumber As String ' 5
    Public CurrentServerTime As String ' 6
    Public LastConduitTime As String ' 7
    Public LastConduitUpdateServerTime As String ' 8
    Public LastMedicalDeviceDataUpdateServerTime As String ' 9
    Public FirstName As String ' 10
    Public LastName As String ' 11
    Public ConduitSerialNumber As Guid ' 12
    Public ConduitBatteryLevel As Integer ' 13
    Public ConduitBatteryStatus As String ' 14
    Public ConduitInRange As Boolean ' 15
    Public ConduitMedicalDeviceInRange As Boolean ' 16
    Public ConduitSensorInRange As Boolean ' 17
    Public MedicalDeviceFamily As String ' 18
    Public SensorState As String ' 19
    Public MedicalDeviceSerialNumber As String ' 20
    Public MedicalDeviceTime As String ' 21
    Public SMedicalDeviceTime As DateTime ' 22
    Public ReservoirLevelPercent As Integer ' 23
    Public ReservoirAmount As Integer ' 24
    Public ReservoirRemainingUnits As Double ' 25
    Public MedicalDeviceBatteryLevelPercent As Integer ' 26
    Public SensorDurationHours As Integer ' 27
    Public TimeToNextCalibHours As Integer ' 28
    Public CalibStatus As String ' 29
    Public BgUnits As String ' 30
    Public TimeFormat As String ' 31
    Public LastSensorTime As String ' 32
    Public SLastSensorTime As DateTime ' 33
    Public MedicalDeviceSuspended As Boolean ' 34
    Public LastSGTrend As String ' 35
    Public LastSG As Dictionary(Of String, String) ' 36
    Public LastAlarm As Dictionary(Of String, String) ' 37
    Public ActiveInsulin As Dictionary(Of String, String) ' 38
    Public SGs As New List(Of Dictionary(Of String, String)) ' 39
    Public Limits As List(Of Dictionary(Of String, String)) ' 40
    Public Markers As List(Of Dictionary(Of String, String)) ' 41
    Public NotificationHistory As New Dictionary(Of String, List(Of Dictionary(Of String, String))) ' 42
    Public TherapyAlgorithmState As Dictionary(Of String, String) ' 43
    Public PumpBannerState As List(Of Dictionary(Of String, String)) ' 44
    Public Basal As Dictionary(Of String, String) ' 45
    Public SystemStatusMessage As String ' 46
    Public AverageSG As Integer ' 47
    Public BelowHypoLimit As Integer ' 48
    Public AboveHyperLimit As Integer ' 49
    Public TimeInRange As Integer ' 50
    Public PumpCommunicationState As Boolean ' 51
    Public GstCommunicationState As Boolean ' 52
    Public GstBatteryLevel As Integer ' 53
    Public LastConduitDateTime As String ' 54
    Public MaxAutoBasalRate As Double ' 55
    Public MaxBolusAmount As Double ' 56
    Public SensorDurationMinutes As Integer ' 57
    Public TimeToNextCalibrationMinutes As Integer ' 58
    Public ClientTimeZoneName As String ' 59
    Public SgBelowLimit As Integer ' 60
    Public AverageSGFloat As Double ' 61


    Enum ItemIndexs As Integer
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
        LastSG = 36
        LastAlarm = 37
        ActiveInsulin = 38
        SGs = 39
        Limits = 40
        Markers = 41
        NotificationHistory = 42
        therapyAlgorithmState = 43
        pumpBannerState = 44
        Basal = 45
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

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub GetInnerTable(tableLevel1Blue As TableLayoutPanel, innerJson As Dictionary(Of String, String), itemIndex As ItemIndexs)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.BackColor = Color.LightBlue
        If itemIndex = ItemIndexs.NotificationHistory Then
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
            Dim label As Label = New Label With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .Text = innerRow.Key,
                    .AutoSize = True
                    }

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = Json.LoadList(innerRow.Value)
                If itemIndex = ItemIndexs.NotificationHistory Then
                    NotificationHistory.Add(innerRow.Key, innerJson1)
                End If
                If innerJson1.Count > 0 Then
                    Dim tableLevel2Green As New TableLayoutPanel With {
                            .AutoScroll = True,
                            .AutoSize = True,
                            .BackColor = Color.LawnGreen,
                            .BorderStyle = BorderStyle.Fixed3D,
                            .ColumnCount = 1,
                            .AutoSizeMode = AutoSizeMode.GrowOnly,
                            .Dock = System.Windows.Forms.DockStyle.Fill,
                            .RowCount = innerJson1.Count
                            }
                    tableLevel2Green.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 80.0))
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson1.WithIndex()
                        tableLevel2Green.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.AutoSize))
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        Dim tableLevel3Orange As New TableLayoutPanel With {
                                .AutoScroll = True,
                                .AutoSize = True,
                                .BackColor = Color.Orange,
                                .AutoSizeMode = AutoSizeMode.GrowOnly,
                                .ColumnCount = 2,
                                .Dock = System.Windows.Forms.DockStyle.Fill
                                }
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            If zFilterList.ContainsKey(itemIndex) Then
                                If zFilterList(itemIndex).Contains(e.Value.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3Orange.RowCount += 1
                            tableLevel3Orange.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0))
                            tableLevel3Orange.Controls.AddRange({New Label With {
                                                                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                    .Text = e.Value.Key,
                                                                    .AutoSize = True},
                                                                 New TextBox With {
                                                                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                    .AutoSize = True,
                                                                     .Text = e.Value.Value}})
                            Application.DoEvents()
                        Next
                        tableLevel3Orange.Height += 40
                        tableLevel2Green.Controls.Add(tableLevel3Orange, 0, innerDictionary.Index)
                        tableLevel2Green.Height += 4
                        Application.DoEvents()
                    Next
                    tableLevel1Blue.Controls.AddRange({label, tableLevel2Green})
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
        If itemIndex = ItemIndexs.LastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        End If
        Application.DoEvents()
    End Sub

    Private Shared Sub Chart1_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles Chart1.PostPaint
        ' Painting series object
        Dim area As ChartArea = TryCast(sender, ChartArea)
        If area IsNot Nothing Then

            Dim format As New StringFormat With {
                    .Alignment = StringAlignment.Center
                    }

            Dim rect As New RectangleF(area.Position.X, area.Position.Y, area.Position.Width, 6)

            rect = e.ChartGraphics.GetAbsoluteRectangle(rect)
            e.ChartGraphics.Graphics.DrawString(area.Name, New Font("Arial", 10), Brushes.Black, rect, format)

        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        initializeChart()
    End Sub

    Private Sub initializeChart()
        Chart1.BackColor = System.Drawing.Color.WhiteSmoke
        Chart1.BackGradientStyle = GradientStyle.TopBottom
        Chart1.BackSecondaryColor = System.Drawing.Color.White
        Chart1.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105)
        Chart1.BorderlineDashStyle = ChartDashStyle.Solid
        Chart1.BorderlineWidth = 2
        Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss
        Chart1.Name = "chart1"
        Chart1.TabIndex = 0
        Chart1.Size = New Size(612, TabPage1.ClientSize.Height - (BGImage.Height + BGImage.Top + 6))
        BGImage.Location = New Point(Chart1.Left + (Chart1.Width \ 2) - (BGImage.Width \ 2), 3)
        CurrentBG.Parent = BGImage
        CurrentBG.BackColor = Color.Transparent
        CurrentBG.ForeColor = Color.White
        CurrentBG.Location = New Point((BGImage.Width \ 2) - (CurrentBG.Width \ 2), BGImage.Height \ 4)
        Chart1.Location = New Point(3, BGImage.Height + 3)
        chartArea1.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
        chartArea1.AxisX.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold)
        chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        chartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black
        chartArea1.AxisX.ScrollBar.Size = 10
        chartArea1.AxisY.LabelStyle.Font = New Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold)
        chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        chartArea1.AxisY.ScrollBar.LineColor = System.Drawing.Color.Black
        chartArea1.AxisY.ScrollBar.Size = 10
        chartArea1.BackColor = System.Drawing.Color.Gainsboro
        chartArea1.BackGradientStyle = GradientStyle.TopBottom
        chartArea1.BackSecondaryColor = System.Drawing.Color.White
        chartArea1.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64, 64)
        chartArea1.BorderDashStyle = ChartDashStyle.Solid
        chartArea1.CursorX.IsUserEnabled = True
        chartArea1.CursorX.IsUserSelectionEnabled = True
        chartArea1.CursorY.IsUserEnabled = True
        chartArea1.CursorY.IsUserSelectionEnabled = True
        chartArea1.Name = "Default"
        chartArea1.ShadowColor = System.Drawing.Color.Transparent
        legend1.BackColor = System.Drawing.Color.Transparent
        legend1.Enabled = False
        legend1.Font = New Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold)
        legend1.IsTextAutoFit = False
        legend1.Name = "Default"
        series1.BorderColor = System.Drawing.Color.FromArgb(180, 26, 59, 105)
        series1.ChartArea = "Default"
        series1.ChartType = SeriesChartType.FastLine
        series1.Legend = "Default"
        series1.Name = "Default"
        series1.ShadowColor = System.Drawing.Color.Black
        title1.Font = New Font("Trebuchet MS", 12.0F, System.Drawing.FontStyle.Bold)
        title1.ForeColor = System.Drawing.Color.FromArgb(26, 59, 105)
        title1.Name = "Title1"
        title1.ShadowColor = System.Drawing.Color.FromArgb(32, 0, 0, 0)
        title1.ShadowOffset = 3
        title1.Text = "Summary of last 24 hours"
        Chart1.ChartAreas.Add(chartArea1)
        Chart1.Legends.Add(legend1)
        Chart1.Series.Add(series1)
        Chart1.Titles.Add(title1)
        TabPage1.Controls.Add(Chart1)
        Application.DoEvents()
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
        Timer1.Enabled = True

        UpdateAllTabPages()

        Application.DoEvents()
    End Sub

    Private Sub UpdateAllTabPages()
        UpdateDataTableWithSG(RecentData)
        CurrentBG.Text = LastSG("sg")
        RemainingInsulinUnits.Text = $"{ReservoirRemainingUnits} U"
        ActiveInsulinValue.Text = ActiveInsulin("amount").ToString & "U"
        Dim lastValue As Integer = 0
        ' Fill Chart1
        For Each SGList As IndexClass(Of Dictionary(Of String, String)) In SGs.WithIndex()
            Dim p As Integer = CInt(SGList.Value("sg"))
            If SGList.IsFirst Then
                lastValue = p
            End If
            If p = 0 Then
                p = lastValue
            End If
            Chart1.Series("Default").Points.AddY(p)
        Next
        ' Set fast line chart type
        Chart1.Series("Default").ChartType = SeriesChartType.FastLine
        DrawInsulinLevel()

    End Sub

    Private Sub DrawInsulinLevel()
        If ReservoirAmount = 0 Then Exit Sub
        Dim insulinImage As New Bitmap(ImageList1.Images("InsulinVial.png"))
        Dim myGraphics As Graphics = Graphics.FromImage(insulinImage)
        Dim scale As Double = 4.0
        Dim scaledInsulinLevel As Integer = CInt(ReservoirLevelPercent / scale)
        Dim myRectangle As Rectangle = New Rectangle(x:=15, y:=55 - scaledInsulinLevel, width:=33, height:=scaledInsulinLevel)

        'draw rectangle from pen and rectangle objects
        ' Create solid brush.
        Dim insulinLevelBrush As SolidBrush
        If ReservoirLevelPercent > 40 Then
            insulinLevelBrush = New SolidBrush(Color.Green)
        ElseIf ReservoirLevelPercent > 20 Then
            insulinLevelBrush = New SolidBrush(Color.Yellow)
        Else
            insulinLevelBrush = New SolidBrush(Color.Red)
        End If

        ' Fill rectangle to screen.
        myGraphics.FillRectangle(insulinLevelBrush, myRectangle)
        InsulinLevelPictureBox.Image = insulinImage
        Application.DoEvents()
    End Sub

    Private Sub UpdateDataTableWithSG(localRecentData As Dictionary(Of String, String))
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
            Select Case c.Index
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
                Case ItemIndexs.LastSG
                    LayoutPanel1 = TableLayoutPanelTop1
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.LastAlarm
                    LayoutPanel1 = TableLayoutPanelTop2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.ActiveInsulin
                    LayoutPanel1 = TableLayoutPanel2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.SGs
                    LayoutPanel1 = TableLayoutPanel3
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.Limits
                    LayoutPanel1 = TableLayoutPanel4
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.Markers
                    LayoutPanel1 = TableLayoutPanel5
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.NotificationHistory
                    LayoutPanel1 = TableLayoutPanel6
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ItemIndexs.therapyAlgorithmState
                    ' handled elsewhere
                Case ItemIndexs.pumpBannerState
                    ' handled elsewhere
                Case ItemIndexs.Basal
                    LayoutPanel1 = TableLayoutPanel7
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
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
            End Select
            LayoutPanel1.Visible = False
            LayoutPanel1.AutoSize = True

            If listOfSingleItems.Contains(c.Index) OrElse singleItem Then
                If Not (singleItem AndAlso singleItemIndex = c.Index) Then
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
            If Not singleItem Then
                LayoutPanel1.Controls.Add(New Label With {
                                                  .Text = $"{c.Index} {row.Key}",
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True
                                                  }, 0, tableRelitiveRow)
            End If
            If row.Value.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
                Select Case c.Index
                    Case ItemIndexs.SGs
                        SGs = innerJson
                    Case ItemIndexs.Limits
                        Limits = innerJson
                    Case ItemIndexs.Markers
                        Markers = innerJson
                    Case ItemIndexs.NotificationHistory
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
                                .AutoSize = True,
                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                .ColumnCount = 2,
                                .Dock = System.Windows.Forms.DockStyle.Fill,
                                .Margin = New Padding(0),
                                .Name = "InnerTable",
                                .Padding = New Padding(0)
                                }
                        LayoutPanel1.Controls.Add(tableLevel1Blue, 1, Dic.Index)
                        GetInnerTable(tableLevel1Blue, Dic.Value, CType(c.Index, ItemIndexs))
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
                Select Case c.Index
                    Case ItemIndexs.LastSG
                        LastSG = innerJson
                    Case ItemIndexs.LastAlarm
                        LastAlarm = innerJson
                    Case ItemIndexs.ActiveInsulin
                        ActiveInsulin = innerJson
                    Case ItemIndexs.NotificationHistory
                    Case ItemIndexs.therapyAlgorithmState
                        TherapyAlgorithmState = innerJson
                    Case ItemIndexs.Basal
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
                LayoutPanel1.Controls.Add(tableLevel1Blue, If(singleItem, 0, 1), tableRelitiveRow)
                GetInnerTable(tableLevel1Blue, innerJson, CType(c.Index, ItemIndexs))
            Else
                LayoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)
            End If
            Application.DoEvents()
            LayoutPanel1.Visible = True
        Next
        Cursor = Cursors.Default
    End Sub

End Class
