' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module CreateChartItems

    Private ReadOnly s_mgdLValues As New List(Of Single) From {50, 100, 150, 200, 250, 300, 350, 400}

    Private ReadOnly s_mmolLValues As New List(Of Single) From {2.8, 5, 8, 11, 14, 17, 20, 22.2}

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
                            .ShadowColor = lineColor.GetContrastingColor(),
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

    Friend Sub AddAutoCorrectionLegend(activeInsulinChartLegend As Legend, homeChartLegend As Legend, treatmentMarkersChartLegend As Legend)
        Dim i As Integer = activeInsulinChartLegend.CustomItems.IndexOfAutoCorrection()
        activeInsulinChartLegend.CustomItems(i).Enabled = True
        i = homeChartLegend.CustomItems.IndexOfAutoCorrection()
        homeChartLegend.CustomItems(i).Enabled = True
        i = treatmentMarkersChartLegend.CustomItems.IndexOfAutoCorrection()
        treatmentMarkersChartLegend.CustomItems(i).Enabled = True
    End Sub

    Friend Function CreateChart(chartName As String) As Chart
        Dim chart As New Chart With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .BackColor = Color.Black,
                    .BorderlineColor = Color.MidnightBlue,
                    .BorderlineDashStyle = ChartDashStyle.Solid,
                    .BorderlineWidth = 2,
                    .Dock = DockStyle.Fill,
                    .Name = chartName,
                    .TabIndex = 0
                }
        chart.Annotations.Add(s_calloutAnnotations(chartName))
        Return chart
    End Function

    Friend Function CreateChartArea(containingChart As Chart) As ChartArea
        Dim tmpChartArea As New ChartArea(NameOf(ChartArea)) With {
                     .BackColor = Color.FromArgb(90, 107, 87),
                     .BorderColor = Color.FromArgb(64, Color.DimGray),
                     .BorderDashStyle = ChartDashStyle.Solid,
                     .ShadowColor = Color.Transparent
                 }
        With tmpChartArea
            Dim labelColor As Color = containingChart.BackColor.GetContrastingColor()
            Dim labelFont As New Font("Segoe UI", 12.0F, FontStyle.Bold)

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
                    .Format = "{0}"
                End With
                .LineColor = Color.FromArgb(64, labelColor)
                With .MajorGrid
                    .Enabled = False
                End With
                .ScaleView.Zoomable = False
            End With
            Dim firstAxis As List(Of Single)
            Dim secondAxis As List(Of Single)
            If NativeMmolL Then
                firstAxis = s_mmolLValues
                secondAxis = s_mgdLValues
            Else
                firstAxis = s_mgdLValues
                secondAxis = s_mmolLValues
            End If

            With .AxisY2
                .Maximum = firstAxis.Last
                .Minimum = firstAxis.First

                .IsMarginVisible = False
                .IsStartedFromZero = False
                With .LabelStyle
                    .Font = labelFont
                    .ForeColor = labelColor
                    .Format = GetSgFormat(False)
                End With
                .LineColor = Color.FromArgb(64, labelColor)

                With .MajorGrid
                    .Interval = firstAxis(0)
                    .LineColor = Color.FromArgb(64, labelColor)
                End With
                With .MajorTickMark
                    .Enabled = True
                    .Interval = firstAxis(0)
                    .LineColor = Color.FromArgb(64, labelColor)
                End With

                For i As Integer = 0 To s_mmolLValues.Count - 1
                    Dim yMin As Single = GetYMinValue(NativeMmolL)
                    .CustomLabels.Add(New CustomLabel(firstAxis(i) - yMin,
                                                           firstAxis(i) + yMin,
                                                           $"{firstAxis(i).ToString(If(NativeMmolL, "F1", "F0"), CurrentUICulture).Replace(",0", "")}",
                                                           0,
                                                           LabelMarkStyle.None) With {.ForeColor = labelColor})
                    .CustomLabels.Add(New CustomLabel(firstAxis(i) - yMin,
                                                           firstAxis(i) + yMin,
                                                           $"{secondAxis(i).ToString(If(NativeMmolL, "F0", "F1"), CurrentUICulture).Replace(".0", "").Replace(",0", "")}",
                                                           1,
                                                           LabelMarkStyle.None) With {.ForeColor = labelColor})
                Next

                .Title = "Sensor Glucose Value"
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
                        .Docking = Docking.Bottom,
                        .Enabled = Form1.MenuOptionsShowChartLegends.Checked,
                        .Font = New Font("Segoe UI", 20.0F, FontStyle.Bold),
                        .ForeColor = .BackColor.GetContrastingColor(),
                        .IsTextAutoFit = True
                    }
    End Function

    Friend Function CreateSeriesActiveInsulin() As Series
        Dim s As Series = CreateSeriesBase(ActiveInsulinSeriesName, "Active Insulin", 4, AxisType.Primary)
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

    Friend Function CreateSeriesLimitsAndTarget(limitsLegend As Legend, seriesName As String) As Series
        Dim legendText As String
        Dim lineColor As Color
        Dim boarderWidth As Integer = 2
        Select Case seriesName
            Case HighLimitSeriesName
                legendText = "High Limit"
                lineColor = GetGraphLineColor("High Limit")
            Case LowLimitSeriesName
                legendText = "Low Limit"
                lineColor = GetGraphLineColor("Low Limit")
            Case Else
                legendText = "SG Target"
                lineColor = GetGraphLineColor("SG Target")
                boarderWidth = 4
        End Select

        Dim s As Series = CreateSeriesBase(seriesName, legendText, boarderWidth, AxisType.Secondary)
        s.IsVisibleInLegend = False
        s.EmptyPointStyle.Color = Color.Transparent
        limitsLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        Return s
    End Function

    Friend Function CreateSeriesSg(sgLegend As Legend) As Series
        Const legendText As String = "SG Series"
        Dim s As Series = CreateSeriesBase(SgSeriesName, legendText, 4, AxisType.Secondary)
        s.IsVisibleInLegend = False
        sgLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        Return s
    End Function

    Friend Function CreateSeriesSuspend(basalLegend As Legend) As Series
        Const legendText As String = "Suspend"
        Dim s As Series = CreateSeriesBase(SuspendSeriesName, legendText, 1, AxisType.Primary)
        s.IsVisibleInLegend = False
        basalLegend.CustomItems.Add(New LegendItem(legendText, Color.FromArgb(128, Color.Red), ""))
        With s.EmptyPointStyle
            .BorderWidth = 4
            .Color = Color.Transparent
        End With
        Return s
    End Function

    Friend Function CreateSeriesTimeChange(basalLegend As Legend) As Series
        Const legendText As String = "Time Change"
        Dim s As Series = CreateSeriesBase(TimeChangeSeriesName, legendText, 1, AxisType.Primary)
        s.IsVisibleInLegend = False
        basalLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        With s.EmptyPointStyle
            .BorderWidth = 4
            .Color = Color.Transparent
        End With
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
        s.SmartLabelStyle.Enabled = False
        s.SmartLabelStyle.CalloutLineWidth = 1
        s.SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Box
        s.SmartLabelStyle.IsMarkerOverlappingAllowed = False
        s.SmartLabelStyle.IsOverlappedHidden = False

        With s.EmptyPointStyle
            .BorderWidth = 4
            .Color = Color.Transparent
        End With

        Return s
    End Function

    Friend Function CreateTitle(chartTitle As String, name As String, foreColor As Color) As Title
        Return New Title With {
                        .BackColor = foreColor.GetContrastingColor(),
                        .Font = New Font("Segoe UI", 14.0F, FontStyle.Bold),
                        .ForeColor = foreColor,
                        .Name = name,
                        .ShadowColor = Color.FromArgb(32, Color.Black),
                        .ShadowOffset = 3,
                        .Text = chartTitle
                    }
    End Function

    <Extension>
    Friend Sub UpdateChartAreaSgAxisX(c As ChartArea)
        With c
            With .AxisX
                .Interval = 2
                .IntervalType = DateTimeIntervalType.Hours
                With .MajorGrid
                    .Interval = 1
                    .IntervalOffsetType = DateTimeIntervalType.Hours
                    .IntervalType = DateTimeIntervalType.Hours
                End With
                If s_listOfSgRecords.Count = 0 Then
                    .Maximum = New OADate(PumpNow)
                    .Minimum = New OADate(PumpNow.AddDays(-1))
                Else
                    .Maximum = s_listOfSgRecords.LastOrDefault.OaDateTime
                    .Minimum = s_listOfSgRecords.FirstOrDefault.OaDateTime
                End If
            End With
        End With
    End Sub

End Module
