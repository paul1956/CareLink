''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

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
        {SingleItems.LastAlarm, LastAlarmFilter},
        {SingleItems.LastSG, LastSgFilter},
        {SingleItems.Markers, markersFilter},
        {SingleItems.NotificationHistory, NotificationHistoryFilter},
        {SingleItems.SGs, sgsFilter}
        }

    Private ReadOnly chartArea1 As New ChartArea()

    Private ReadOnly legend1 As New Legend()

    Private ReadOnly listOfSingleItems As New List(Of Integer) From {
                SingleItems.LastSG,
        SingleItems.LastAlarm,
        SingleItems.ActiveInsulin,
        SingleItems.SGs,
        SingleItems.Limits,
        SingleItems.Markers,
        SingleItems.NotificationHistory,
        SingleItems.Basal
    }

    Private ReadOnly loginDialog As New LoginForm1
    Private ReadOnly series1 As New Series()
    Private ReadOnly title1 As New Title()

    Private Client As CareLinkClient
    Private SGValues As New List(Of Dictionary(Of String, String))

    Public RecentData As Dictionary(Of String, String)

    Enum SingleItems As Integer
        LastSG = 36
        LastAlarm = 37
        ActiveInsulin = 38
        SGs = 39
        Limits = 40
        Markers = 41
        NotificationHistory = 42
        Basal = 45
    End Enum

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Shared Sub GetInnerTable(tableLevel1Blue As TableLayoutPanel, innerJson As Dictionary(Of String, String), itemIndex As Integer)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.BackColor = Color.LightBlue

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            If zFilterList.ContainsKey(itemIndex) Then
                If zFilterList(itemIndex).Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            If itemIndex = SingleItems.SGs Then
                If sgsFilter.Contains(innerRow.Key) Then
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
        If itemIndex = SingleItems.LastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        End If
        Application.DoEvents()
    End Sub

    Private Sub Chart1_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles Chart1.PostPaint
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
        TabPage1.BackColor = System.Drawing.Color.White
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

        Select Case CInt($"0{CurrentBG.Text}")
            Case < 60
                CurrentBG.BackColor = Color.Red
            Case < 70
                CurrentBG.BackColor = Color.Yellow
            Case > 180
                CurrentBG.BackColor = Color.Red
            Case > 150
                CurrentBG.BackColor = Color.Yellow
            Case Else
                CurrentBG.BackColor = Color.White
        End Select

        Application.DoEvents()
    End Sub

    Private Sub UpdateAllTabPages()
        CurrentBG.Text = UpdateDataTableWithSG(RecentData)
        Dim lastValue As Integer = 0
        ' Fill series data
        For Each SGList As IndexClass(Of Dictionary(Of String, String)) In SGValues.WithIndex()
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

    End Sub

    Private Function UpdateDataTableWithSG(localRecentData As Dictionary(Of String, String)) As String
        Cursor = Cursors.WaitCursor
        Dim returnValue As String = CurrentBG.Text
        TableLayoutPanel1.Controls.Clear()
        TableLayoutPanel1.RowCount = localRecentData.Count - 8
        Dim currentRowIndex As Integer = 0
        Dim singleItem As Boolean
        Dim LayoutPanel1 As TableLayoutPanel
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
            Dim singleItemIndex As Integer = 0
            Select Case c.Index
                Case SingleItems.LastSG
                    LayoutPanel1 = TableLayoutPanelTop1
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.LastAlarm
                    LayoutPanel1 = TableLayoutPanelTop2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.ActiveInsulin
                    LayoutPanel1 = TableLayoutPanel2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.SGs
                    LayoutPanel1 = TableLayoutPanel3
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.Limits
                    LayoutPanel1 = TableLayoutPanel4
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.Markers
                    LayoutPanel1 = TableLayoutPanel5
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.NotificationHistory
                    LayoutPanel1 = TableLayoutPanel6
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SingleItems.Basal
                    LayoutPanel1 = TableLayoutPanel7
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case Else
                    LayoutPanel1 = TableLayoutPanel1
                    singleItem = False
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
            Dim row As KeyValuePair(Of String, String) = c.Value
            If Not singleItem Then
                LayoutPanel1.Controls.Add(New Label With {
                                                  .Text = $"{c.Index} {row.Key}",
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True
                                                  }, 0, tableRelitiveRow)
            End If
            If row.Value.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
                If c.Index = SingleItems.SGs Then
                    SGValues = innerJson
                End If
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
                        GetInnerTable(tableLevel1Blue, Dic.Value, c.Index)
                    Next
                Else
                    LayoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)

                End If
            ElseIf row.Value.StartsWith("{") Then
                LayoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
                Dim innerJson As Dictionary(Of String, String) = Json.Loads(row.Value)
                If row.Key = "lastSG" Then
                    returnValue = innerJson("sg")
                End If
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
                GetInnerTable(tableLevel1Blue, innerJson, c.Index)
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
        Return returnValue
    End Function

End Class
