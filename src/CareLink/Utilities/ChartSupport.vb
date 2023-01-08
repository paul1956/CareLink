' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module ChartSupport
    Friend Const ActiveInsulinSeriesName As String = "ActiveInsulinSeries"
    Friend Const AutoCorrectionSeriesName As String = "AutoCorrection"
    Friend Const BasalSeriesName As String = "BasalSeries"
    Friend Const BgSeriesName As String = "BgSeries"
    Friend Const HighLimitSeriesName As String = "HighLimitSeries"
    Friend Const LowLimitSeriesName As String = "LowLimitSeries"
    Friend Const MarkerSeriesName As String = "MarkerSeries"
    Friend Const MinBasalSeriesName As String = "MinBasal"
    Friend Const TimeChangeSeriesName As String = "TimeChangeSeries"

    Private Function CreateBaseSeries(seriesName As String, legendText As String, borderWidth As Integer, yAxisType As AxisType) As Series
        Dim lineColor As Color = GetGraphColor(legendText)
        Dim tmpSeries As New Series(seriesName) With {
                            .BorderColor = Color.FromArgb(180, lineColor),
                            .BorderWidth = borderWidth,
                            .ChartArea = NameOf(ChartArea),
                            .ChartType = SeriesChartType.Line,
                            .Color = lineColor,
                            .IsValueShownAsLabel = False,
                            .LegendText = legendText,
                            .ShadowColor = lineColor.GetContrastingColor,
                            .XValueType = ChartValueType.DateTime,
                            .YAxisType = yAxisType
                        }
        Return tmpSeries
    End Function

    Friend Function CreateChart(chartName As String) As Chart
        Return New Chart With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .BackColor = Color.WhiteSmoke,
                    .BackGradientStyle = GradientStyle.TopBottom,
                    .BackSecondaryColor = Color.White,
                    .BorderlineColor = Color.FromArgb(26, 59, 105),
                    .BorderlineDashStyle = ChartDashStyle.Solid,
                    .BorderlineWidth = 2,
                    .Dock = DockStyle.Fill,
                    .Name = chartName,
                    .TabIndex = 0
                }
    End Function

    Friend Function CreateChartArea() As ChartArea
        Dim tmpChartArea As New ChartArea(NameOf(ChartArea)) With {
                     .BackColor = Color.FromArgb(180, 23, 47, 19),
                     .BackGradientStyle = GradientStyle.TopBottom,
                     .BackSecondaryColor = Color.FromArgb(180, 29, 56, 26),
                     .BorderColor = Color.FromArgb(64, 64, 64, 64),
                     .BorderDashStyle = ChartDashStyle.Solid,
                     .ShadowColor = Color.Transparent
                 }
        With tmpChartArea
            With .AxisX
                .Interval = 2
                .IntervalType = DateTimeIntervalType.Hours
                .IsInterlaced = True
                .IsMarginVisible = True
                .LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
                .MajorGrid.Interval = 1
                .MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
                .MajorGrid.IntervalType = DateTimeIntervalType.Hours
                With .LabelStyle
                    .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                    .Format = s_timeWithoutMinuteFormat
                End With
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
                .IntervalAutoMode = IntervalAutoMode.FixedCount
                .IsInterlaced = True
                .IsLabelAutoFit = False
                .IsMarginVisible = False
                .IsStartedFromZero = True
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64)
                .ScaleView.Zoomable = False
            End With
            With .AxisY2
                .Interval = HomePageMealRow
                .IsMarginVisible = False
                .IsStartedFromZero = False
                .LabelStyle.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
                .LineColor = Color.FromArgb(64, 64, 64, 64)
                .MajorGrid = New Grid With {
                    .Interval = HomePageMealRow,
                    .LineColor = Color.FromArgb(64, 64, 64, 64)
                }
                .MajorTickMark = New TickMark() With {.Interval = HomePageMealRow, .Enabled = True}
                .Maximum = HomePageBasalRow
                .Minimum = HomePageMealRow
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
                .AxisType = AxisType.Primary
                .Interval = 0
                .IsUserEnabled = False
                .IsUserSelectionEnabled = False
                .LineColor = Color.Transparent
            End With

        End With

        Return tmpChartArea
    End Function

    Friend Function CreateChartLegend(legendName As String) As Legend
        Return New Legend(legendName) With {
                        .BackColor = Color.Gray,
                        .BorderWidth = 0,
                        .Enabled = File.Exists(GetShowLegendFileNameWithPath),
                        .Font = New Font("Trebuchet MS", 20.0F, FontStyle.Bold),
                        .ForeColor = .BackColor.GetContrastingColor,
                        .IsTextAutoFit = True,
                        .Title = "Series Legend",
                        .TitleBackColor = Color.White
                    }
    End Function

    Friend Function CreateSeriesActiveInsulin() As Series
        Dim s As Series = CreateBaseSeries(ActiveInsulinSeriesName, "Active Insulin", 4, AxisType.Primary)
        s.MarkerColor = Color.Black
        s.MarkerSize = 4
        s.MarkerStyle = MarkerStyle.Circle
        Return s
    End Function

    Friend Function CreateSeriesBasal(SeriesName As String, basalLegend As Legend, legendText As String, YAxisType As AxisType) As Series
        Dim s As Series = CreateBaseSeries(SeriesName, legendText, 2, YAxisType)
        s.IsVisibleInLegend = False
        Dim lineColor As Color = GetGraphColor(legendText)
        Select Case legendText
            Case "Min Basal"
                lineColor = Color.FromArgb(150, lineColor)
                basalLegend.CustomItems.Add(New LegendItem(legendText, lineColor, ""))
            Case "Auto Correction"
                basalLegend.CustomItems.Add(New LegendItem(legendText, lineColor, ""))
                basalLegend.CustomItems.Last.Enabled = False
            Case "Basal Series"
                basalLegend.CustomItems.Add(New LegendItem(legendText, lineColor, ""))
            Case Else
                Stop
        End Select
        s.EmptyPointStyle.BorderWidth = 2
        s.EmptyPointStyle.Color = Color.Transparent
        Return s
    End Function

    Friend Function CreateSeriesBg(bgLegend As Legend) As Series
        Const legendText As String = "BG Series"
        Dim s As Series = CreateBaseSeries(BgSeriesName, legendText, 4, AxisType.Secondary)
        s.IsVisibleInLegend = False
        bgLegend.CustomItems.Add(New LegendItem(legendText, GetGraphColor(legendText), ""))
        Return s
    End Function

    Friend Function CreateSeriesLimits(limitsLegend As Legend, seriesName As String) As Series
        Dim legendText As String
        Dim lineColor As Color
        If seriesName.Equals(HighLimitSeriesName) Then
            legendText = "High Limit"
            lineColor = Color.Yellow
        Else
            legendText = "Low Limit"
            lineColor = Color.Red
        End If

        Dim s As Series = CreateBaseSeries(seriesName, legendText, 2, AxisType.Secondary)
        s.IsVisibleInLegend = False
        limitsLegend.CustomItems.Add(New LegendItem(legendText, GetGraphColor(legendText), ""))
        s.EmptyPointStyle.Color = Color.Transparent
        Return s
    End Function

    Friend Function CreateSeriesTimeChange(basalLegend As Legend) As Series
        Const legendText As String = "Time Change"
        Dim s As Series = CreateBaseSeries(TimeChangeSeriesName, legendText, 1, AxisType.Primary)
        s.IsVisibleInLegend = False
        basalLegend.CustomItems.Add(New LegendItem(legendText, GetGraphColor(legendText), ""))
        s.EmptyPointStyle.BorderWidth = 4
        s.EmptyPointStyle.Color = Color.Transparent
        Return s
    End Function

    Friend Function CreateSeriesWithoutVisibleLegend(YAxisType As AxisType) As Series
        Dim s As New Series(MarkerSeriesName) With {
                        .BorderColor = Color.Transparent,
                        .BorderWidth = 1,
                        .ChartArea = NameOf(ChartArea),
                        .ChartType = SeriesChartType.Point,
                        .Color = Color.HotPink,
                        .IsVisibleInLegend = False,
                        .MarkerSize = 15,
                        .XValueType = ChartValueType.DateTime,
                        .YAxisType = YAxisType
                    }
        s.EmptyPointStyle.BorderWidth = 4
        s.EmptyPointStyle.Color = Color.Transparent

        Return s
    End Function

    <Extension>
    Friend Sub InitializeChartAreaBG(c As ChartArea)
        With c
            .AxisX.Minimum = s_listOfSGs(0).OAdatetime
            .AxisX.Maximum = s_listOfSGs.Last.OAdatetime
            .AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Hours
            .AxisX.MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
            .AxisX.MajorGrid.Interval = 1
            .AxisX.IntervalType = DateTimeIntervalType.Hours
            .AxisX.Interval = 2
        End With
    End Sub

    Public Function CreateChartTitle(chartTitle As String, name As String, foreColor As Color) As Title
        Return New Title With {
                        .Font = New Font("Trebuchet MS", 12.0F, FontStyle.Bold),
                        .ForeColor = foreColor,
                        .BackColor = foreColor.GetContrastingColor(),
                        .Name = name,
                        .ShadowColor = Color.FromArgb(32, Color.Black),
                        .ShadowOffset = 3,
                        .Text = chartTitle
                    }
    End Function

    Public Sub EnableAutoCorrectionLegend(activeInsulinChartLegend As Legend, homeChartLegend As Legend, treatmentMarkersChartLegend As Legend)
        Dim i As Integer = IndexOfName(activeInsulinChartLegend.CustomItems, "Auto Correction")
        activeInsulinChartLegend.CustomItems(i).Enabled = True
        i = IndexOfName(homeChartLegend.CustomItems, "Auto Correction")
        homeChartLegend.CustomItems(i).Enabled = True
        i = IndexOfName(treatmentMarkersChartLegend.CustomItems, "Auto Correction")
        treatmentMarkersChartLegend.CustomItems(i).Enabled = True
    End Sub

    Private Function IndexOfName(customItems As LegendItemsCollection, name As String) As Integer
        For Each item As IndexClass(Of LegendItem) In customItems.WithIndex
            If item.Value.Name = name Then
                Return item.Index
            End If
        Next
        Stop
        Return -1
    End Function

End Module
