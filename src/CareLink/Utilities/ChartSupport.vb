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

    Friend Function CreateSeriesActiveInsulin(activeInsulinChartLegendName As String) As Series
        Return New Series(ActiveInsulinSeriesName) With {
                    .BorderColor = Color.FromArgb(180, 26, 59, 105),
                    .BorderWidth = 4,
                    .ChartArea = NameOf(ChartArea),
                    .ChartType = SeriesChartType.Line,
                    .Color = GetGraphColor("Active Insulin"),
                    .Legend = activeInsulinChartLegendName,
                    .LegendText = "Active Insulin",
                    .MarkerColor = Color.Black,
                    .MarkerSize = 4,
                    .MarkerStyle = MarkerStyle.Circle,
                    .ShadowColor = Color.Black,
                    .XValueType = ChartValueType.DateTime,
                    .YAxisType = AxisType.Primary
                }
    End Function

    Friend Function CreateSeriesBasal(SeriesName As String, legendName As String, YAxisType As AxisType) As Series
        Dim basalColor As Color = GetGraphColor(legendName)
        Dim s As New Series(SeriesName) With {
                     .BorderWidth = 2,
                     .BorderColor = basalColor,
                     .ChartArea = NameOf(ChartArea),
                     .ChartType = SeriesChartType.Line,
                     .Color = basalColor,
                     .LegendText = legendName,
                     .XValueType = ChartValueType.DateTime,
                     .YAxisType = YAxisType
                 }
        s.EmptyPointStyle.BorderWidth = 4
        s.EmptyPointStyle.Color = Color.Transparent

        Return s
    End Function

    Friend Function CreateSeriesBg(legendName As String) As Series
        Dim lineColor As Color = GetGraphColor("BG Series")
        Return New Series(BgSeriesName) With {
                     .BorderColor = Color.FromArgb(180, 26, 59, 105),
                     .BorderWidth = 4,
                     .ChartArea = NameOf(ChartArea),
                     .ChartType = SeriesChartType.Line,
                     .Color = lineColor,
                     .Legend = legendName,
                     .LegendText = "BG",
                     .ShadowColor = lineColor.GetContrastingColor,
                     .XValueType = ChartValueType.DateTime,
                     .YAxisType = AxisType.Secondary
                 }
    End Function

    Friend Function CreateSeriesLimits(seriesName As String) As Series
        Dim legendName As String
        Dim lineColor As Color
        If seriesName.Equals(HighLimitSeriesName) Then
            legendName = "High Limit"
            lineColor = Color.Yellow
        Else
            legendName = "Low Limit"
            lineColor = Color.Red
        End If
        Dim tmpSeries As New Series(seriesName) With {
                            .BorderColor = Color.FromArgb(180, lineColor),
                            .BorderWidth = 2,
                            .ChartArea = NameOf(ChartArea),
                            .ChartType = SeriesChartType.Line,
                            .Color = lineColor,
                            .LegendText = legendName,
                            .ShadowColor = Color.Black,
                            .XValueType = ChartValueType.DateTime,
                            .YAxisType = AxisType.Secondary
                        }
        tmpSeries.EmptyPointStyle.Color = Color.Transparent
        Return tmpSeries
    End Function

    Friend Function CreateSeriesMarker(YAxisType As AxisType) As Series
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

    Friend Function CreateSeriesTimeChange() As Series
        Dim s As New Series(TimeChangeSeriesName) With {
                        .ChartType = SeriesChartType.Line,
                        .BorderColor = Color.Transparent,
                        .BorderWidth = 1,
                        .ChartArea = NameOf(ChartArea),
                        .Color = Color.LightGoldenrodYellow,
                        .LegendText = "Time Change",
                        .ShadowColor = Color.Transparent,
                        .XValueType = ChartValueType.DateTime,
                        .YAxisType = AxisType.Primary
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
                        .ShadowColor = Color.FromArgb(32, 0, 0, 0),
                        .ShadowOffset = 3,
                        .Text = chartTitle
                    }
    End Function

End Module
