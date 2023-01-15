' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module ChartSupport
    Friend Const ActiveInsulinSeries As String = NameOf(ActiveInsulinSeries)
    Friend Const AutoCorrectionSeries As String = NameOf(AutoCorrectionSeries)
    Friend Const BasalSeries As String = NameOf(BasalSeries)
    Friend Const BgSeries As String = NameOf(BgSeries)
    Friend Const HighLimitSeries As String = NameOf(HighLimitSeries)
    Friend Const LowLimitSeries As String = NameOf(LowLimitSeries)
    Friend Const MarkerSeries As String = NameOf(MarkerSeries)
    Friend Const MinBasalSeries As String = NameOf(MinBasalSeries)
    Friend Const TimeChangeSeries As String = NameOf(TimeChangeSeries)

    Private Function CreateSeriesBase(seriesName As String, legendText As String, borderWidth As Integer, yAxisType As AxisType) As Series
        Dim lineColor As Color = GetGraphLineColor(legendText)
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

    <Extension>
    Private Function IndexOfAutoCorrection(customItems As LegendItemsCollection) As Integer
        For Each item As IndexClass(Of LegendItem) In customItems.WithIndex
            If item.Value.Name = "Auto Correction" Then
                Return item.Index
            End If
        Next
        Stop
        Return -1
    End Function

    Friend Function CreateChart(chartName As String) As Chart
        Return New Chart With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .BackColor = Color.Black,
                    .BorderlineColor = Color.MidnightBlue,
                    .BorderlineDashStyle = ChartDashStyle.Solid,
                    .BorderlineWidth = 2,
                    .Dock = DockStyle.Fill,
                    .Name = chartName,
                    .TabIndex = 0
                }
    End Function

    Friend Function CreateChartArea(containingChart As Chart) As ChartArea
        Dim tmpChartArea As New ChartArea(NameOf(ChartArea)) With {
                     .BackColor = Color.FromArgb(90, 107, 87),
                     .BorderColor = Color.FromArgb(64, Color.DimGray),
                     .BorderDashStyle = ChartDashStyle.Solid,
                     .ShadowColor = Color.Transparent
                 }
        With tmpChartArea
            Dim labelColor As Color = containingChart.BackColor.GetContrastingColor
            Dim labelFont As New Font("Trebuchet MS", 12.0F, FontStyle.Bold)

            With .AxisX
                .Interval = 2
                .IntervalType = DateTimeIntervalType.Hours
                .IsInterlaced = True
                .IsMarginVisible = True
                .LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
                With .LabelStyle
                    .Font = labelFont
                    .ForeColor = labelColor
                    .Format = s_timeWithoutMinuteFormat
                End With
                .LineColor = Color.FromArgb(64, labelColor)
                With .MajorGrid
                    .Interval = 1
                    .IntervalOffsetType = DateTimeIntervalType.Hours
                    .IntervalType = DateTimeIntervalType.Hours
                    .LineColor = Color.FromArgb(64, labelColor)
                End With
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
                With .LabelStyle
                    .Font = labelFont
                    .ForeColor = labelColor
                End With
                .LineColor = Color.FromArgb(64, labelColor)
                With .MajorGrid
                    .Enabled = False
                End With
                .ScaleView.Zoomable = False
            End With
            With .AxisY2
                .Interval = HomePageMealRow
                .IsMarginVisible = False
                .IsStartedFromZero = False
                With .LabelStyle
                    .Font = labelFont
                    .ForeColor = labelColor
                End With
                .LineColor = Color.FromArgb(64, labelColor)
                With .MajorGrid
                    .Interval = HomePageMealRow
                    .LineColor = Color.FromArgb(64, labelColor)
                End With
                With .MajorTickMark
                    .Enabled = True
                    .Interval = HomePageMealRow
                    .LineColor = Color.FromArgb(64, labelColor)
                End With

                .Maximum = HomePageBasalRow
                .Minimum = HomePageMealRow
                .Title = "Blood Glucose Value"
                .TitleFont = New Font(labelFont.FontFamily, 14)
                .TitleForeColor = labelColor
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
        Dim s As Series = CreateSeriesBase(ActiveInsulinSeries, "Active Insulin", 4, AxisType.Primary)
        s.MarkerColor = Color.Black
        s.MarkerSize = 4
        s.MarkerStyle = MarkerStyle.Circle
        Return s
    End Function

    Friend Function CreateSeriesBasal(SeriesName As String, basalLegend As Legend, legendText As String, YAxisType As AxisType) As Series
        Dim s As Series = CreateSeriesBase(SeriesName, legendText, 2, YAxisType)
        s.IsVisibleInLegend = False
        Dim lineColor As Color = GetGraphLineColor(legendText)
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
        With s.EmptyPointStyle
            .BorderWidth = 2
            .Color = Color.Transparent
        End With
        Return s
    End Function

    Friend Function CreateSeriesBg(bgLegend As Legend) As Series
        Const legendText As String = "BG Series"
        Dim s As Series = CreateSeriesBase(BgSeries, legendText, 4, AxisType.Secondary)
        s.IsVisibleInLegend = False
        bgLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        Return s
    End Function

    Friend Function CreateSeriesLimits(limitsLegend As Legend, seriesName As String) As Series
        Dim legendText As String
        Dim lineColor As Color
        If seriesName.Equals(HighLimitSeries) Then
            legendText = "High Limit"
            lineColor = Color.Yellow
        Else
            legendText = "Low Limit"
            lineColor = Color.Red
        End If

        Dim s As Series = CreateSeriesBase(seriesName, legendText, 2, AxisType.Secondary)
        s.IsVisibleInLegend = False
        s.EmptyPointStyle.Color = Color.Transparent
        limitsLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        Return s
    End Function

    Friend Function CreateSeriesTimeChange(basalLegend As Legend) As Series
        Const legendText As String = "Time Change"
        Dim s As Series = CreateSeriesBase(TimeChangeSeries, legendText, 1, AxisType.Primary)
        s.IsVisibleInLegend = False
        basalLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        With s.EmptyPointStyle
            .BorderWidth = 4
            .Color = Color.Transparent
        End With
        Return s
    End Function

    Friend Function CreateSeriesWithoutVisibleLegend(YAxisType As AxisType) As Series
        Dim s As New Series(MarkerSeries) With {
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
        With s.EmptyPointStyle
            .BorderWidth = 4
            .Color = Color.Transparent
        End With

        Return s
    End Function

    Friend Function CreateTitle(chartTitle As String, name As String, foreColor As Color) As Title
        Return New Title With {
                        .BackColor = foreColor.GetContrastingColor(),
                        .Font = New Font("Trebuchet MS", 14.0F, FontStyle.Bold),
                        .ForeColor = foreColor,
                        .Name = name,
                        .ShadowColor = Color.FromArgb(32, Color.Black),
                        .ShadowOffset = 3,
                        .Text = chartTitle
                    }
    End Function

    Friend Sub EnableAutoCorrectionLegend(activeInsulinChartLegend As Legend, homeChartLegend As Legend, treatmentMarkersChartLegend As Legend)
        Dim i As Integer = activeInsulinChartLegend.CustomItems.IndexOfAutoCorrection()
        activeInsulinChartLegend.CustomItems(i).Enabled = True
        i = homeChartLegend.CustomItems.IndexOfAutoCorrection()
        homeChartLegend.CustomItems(i).Enabled = True
        i = treatmentMarkersChartLegend.CustomItems.IndexOfAutoCorrection()
        treatmentMarkersChartLegend.CustomItems(i).Enabled = True
    End Sub

    <Extension>
    Friend Sub UpdateChartAreaBGAxisX(c As ChartArea)
        With c
            With .AxisX
                .Interval = 2
                .IntervalType = DateTimeIntervalType.Hours
                With .MajorGrid
                    .Interval = 1
                    .IntervalOffsetType = DateTimeIntervalType.Hours
                    .IntervalType = DateTimeIntervalType.Hours
                End With
                .Maximum = s_listOfSGs.Last.OAdatetime
                .Minimum = s_listOfSGs(0).OAdatetime
            End With
        End With
    End Sub

End Module
