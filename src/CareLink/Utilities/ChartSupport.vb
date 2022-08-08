' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Windows.Forms.DataVisualization.Charting

Module ChartSupport

    Friend Function CreateBgSeries(seriesName As String, chartAreaName As String, legendName As String) As Series
        Return New Series With {
                     .BorderColor = Color.FromArgb(180, 26, 59, 105),
                     .BorderWidth = 4,
                     .ChartArea = chartAreaName,
                     .ChartType = SeriesChartType.Line,
                     .Color = Color.White,
                     .Legend = legendName,
                     .Name = seriesName,
                     .ShadowColor = Color.Black,
                     .XValueType = ChartValueType.DateTime,
                     .YAxisType = AxisType.Secondary
                 }
    End Function

    Friend Function CreateDefaultLegend(legendName As String) As Legend
        Return New Legend(legendName) With {
                        .BackColor = Color.Transparent,
                        .Enabled = False,
                        .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold),
                        .IsTextAutoFit = False
                    }
    End Function

    Friend Function CreateNewChart(chartName As String) As Chart
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

    Friend Function CreateNewChartArea(areaName As String) As ChartArea
        Dim tmpChartArea As New ChartArea(areaName) With {
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

        Return tmpChartArea
    End Function

    Friend Function CreateTimeChangeSeries(seriesName As String) As Series
        Return New Series(seriesName) With {
            .ChartType = SeriesChartType.FastLine
        }
    End Function

End Module
