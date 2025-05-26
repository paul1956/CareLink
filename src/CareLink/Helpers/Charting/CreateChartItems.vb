' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module CreateChartItems

    Private ReadOnly s_mgdLValues As New List(Of Single) From {50, 100, 150, 200, 250, 300, 350, 400}

    Private ReadOnly s_mmolLValues As New List(Of Single) From {2.8, 5, 8, 11, 14, 17, 20, 22.2}

    ''' <summary>
    '''  Creates a base series with common properties for charting.
    '''  This method is used to create series for active insulin, basal rates, limits, and sensor glucose values.
    ''' </summary>
    ''' <param name="seriesName">The name of the series.</param>
    ''' <param name="legendText">The text displayed in the legend.</param>
    ''' <param name="borderWidth">The width of the border.</param>
    ''' <param name="yAxisType">The Y-axis type for the series.</param>
    ''' <returns>A Series object configured with the specified properties.</returns>
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
            .YAxisType = yAxisType}
        Return tmpSeries
    End Function

    ''' <summary>
    '''  Finds the index of a label in a collection of custom legend items.
    '''  This method is used to locate the index of a specific legend item by its name.
    ''' </summary>
    ''' <param name="customItems"></param>
    ''' <param name="legendString"></param>
    ''' <returns>The index of the legend item, or -1 if not found.</returns>
    <Extension>
    Private Function IndexOfLabel(customItems As LegendItemsCollection, legendString As String) As Integer
        For Each item As IndexClass(Of LegendItem) In customItems.WithIndex
            If item.Value.Name = legendString Then
                Return item.Index
            End If
        Next
        Stop
        Return -1
    End Function

    ''' <summary>
    '''  Shows or hides a legend item in the active insulin chart, home chart, and treatment markers chart.
    '''  This method updates the visibility of a specific legend item based on the provided parameters.
    ''' </summary>
    ''' <param name="showLegend"></param>
    ''' <param name="legendString"></param>
    ''' <param name="activeInsulinChartLegend"></param>
    ''' <param name="homeChartLegend"></param>
    ''' <param name="treatmentMarkersChartLegend"></param>
    Friend Sub ShowHideLegendItem(showLegend As Boolean, legendString As String, activeInsulinChartLegend As Legend, homeChartLegend As Legend, treatmentMarkersChartLegend As Legend)
        Dim i As Integer = activeInsulinChartLegend.CustomItems.IndexOfLabel(legendString)
        If i < 0 Then
            ' Legend item not found, nothing to do
            Return
        End If
        activeInsulinChartLegend.CustomItems(i).Enabled = showLegend
        i = homeChartLegend.CustomItems.IndexOfLabel(legendString)
        homeChartLegend.CustomItems(i).Enabled = showLegend
        i = treatmentMarkersChartLegend.CustomItems.IndexOfLabel(legendString)
        treatmentMarkersChartLegend.CustomItems(i).Enabled = showLegend
    End Sub

    ''' <summary>
    '''  Creates a new chart with specified properties.
    '''  This method is used to initialize a chart for displaying data with a specific name and appearance.
    ''' </summary>
    ''' <param name="chartName">The name of the chart.</param>
    ''' <returns>A Chart object configured with the specified properties.</returns>
    Friend Function CreateChart(chartName As String) As Chart
        Dim chart As New Chart With {
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
            .BackColor = Color.Black,
            .BorderlineColor = Color.MidnightBlue,
            .BorderlineDashStyle = ChartDashStyle.Solid,
            .BorderlineWidth = 2,
            .Dock = DockStyle.Fill,
            .Name = chartName,
            .TabIndex = 0}
        chart.Annotations.Add(s_calloutAnnotations(chartName))
        Return chart
    End Function

    ''' <summary>
    '''  Creates a chart area with specific properties for displaying data in a chart.
    '''  This method is used to define the appearance and behavior of the chart area, including axes and grid lines.
    ''' </summary>
    ''' <param name="containingChart">The chart that contains this chart area.</param>
    ''' <returns>A ChartArea object configured with the specified properties.
    ''' </summary>
    ''' <param name="containingChart"></param>
    ''' <returns></returns>
    Friend Function CreateChartArea(containingChart As Chart) As ChartArea
        Dim tmpChartArea As New ChartArea(NameOf(ChartArea)) With {
            .BackColor = Color.FromArgb(red:=90, green:=107, blue:=87),
            .BorderColor = Color.FromArgb(alpha:=64, baseColor:=Color.DimGray),
            .BorderDashStyle = ChartDashStyle.Solid,
            .ShadowColor = Color.Transparent}
        With tmpChartArea
            Dim baseColor As Color = containingChart.BackColor.GetContrastingColor()
            Dim labelFont As New Font(familyName:="Segoe UI", emSize:=12.0F, style:=FontStyle.Bold)

            With .AxisX
                .Interval = 2
                .IntervalType = DateTimeIntervalType.Hours
                .IsInterlaced = True
                .IsMarginVisible = True
                .LabelAutoFitStyle = LabelAutoFitStyles.IncreaseFont Or LabelAutoFitStyles.DecreaseFont Or LabelAutoFitStyles.WordWrap
                With .LabelStyle
                    .Font = labelFont
                    .ForeColor = baseColor
                    .Format = s_timeWithoutMinuteFormat
                End With
                .LineColor = Color.FromArgb(alpha:=64, baseColor)
                With .MajorGrid
                    .Interval = 1
                    .IntervalOffsetType = DateTimeIntervalType.Hours
                    .IntervalType = DateTimeIntervalType.Hours
                    .LineColor = Color.FromArgb(alpha:=64, baseColor)
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
                .InterlacedColor = Color.FromArgb(alpha:=120, baseColor:=Color.LightSlateGray)
                .IntervalAutoMode = IntervalAutoMode.FixedCount
                .IsInterlaced = True
                .IsLabelAutoFit = False
                .IsMarginVisible = False
                .IsStartedFromZero = True
                With .LabelStyle
                    .Font = labelFont
                    .ForeColor = baseColor
                    .Format = "{0}"
                End With
                .LineColor = Color.FromArgb(alpha:=64, baseColor)
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
                    .ForeColor = baseColor
                    .Format = GetSgFormat(withSign:=False)
                End With
                .LineColor = Color.FromArgb(alpha:=64, baseColor)

                With .MajorGrid
                    .Interval = firstAxis(0)
                    .LineColor = Color.FromArgb(alpha:=64, baseColor)
                End With
                With .MajorTickMark
                    .Enabled = True
                    .Interval = firstAxis(0)
                    .LineColor = Color.FromArgb(alpha:=64, baseColor)
                End With

                For i As Integer = 0 To s_mmolLValues.Count - 1
                    Dim yMin As Single = GetYMinValue()
                    .CustomLabels.Add(
                        item:=New CustomLabel(
                            fromPosition:=firstAxis(index:=i) - yMin,
                            toPosition:=firstAxis(index:=i) + yMin,
                            text:=$"{firstAxis(index:=i).ToString(format:=If(NativeMmolL, "F1", "F0"), Provider).Replace(oldValue:=",0", newValue:="")}",
                            labelRow:=0,
                            markStyle:=LabelMarkStyle.None) With {.ForeColor = baseColor})
                    .CustomLabels.Add(
                        item:=New CustomLabel(
                            fromPosition:=firstAxis(index:=i) - yMin,
                            toPosition:=firstAxis(index:=i) + yMin,
                            text:=$"{secondAxis(index:=i).ToString(format:=If(NativeMmolL, "F0", "F1"), Provider).Replace(oldValue:=",0", newValue:="")}",
                            labelRow:=1,
                            markStyle:=LabelMarkStyle.None) With {.ForeColor = baseColor})
                Next

                .Title = "Sensor Glucose Value"
                .TitleFont = New Font(family:=labelFont.FontFamily, emSize:=14)
                .TitleForeColor = baseColor
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

    ''' <summary>
    '''  Creates a chart legend with specific properties.
    '''  This method is used to initialize a legend for displaying series information in a chart.
    ''' </summary>
    ''' <param name="legendName">The name of the legend.</param>
    ''' <returns>A Legend object configured with the specified properties.</returns>
    Friend Function CreateChartLegend(legendName As String) As Legend
        Return New Legend(legendName) With {
            .BackColor = Color.Gray,
            .BorderWidth = 0,
            .Docking = Docking.Bottom,
            .Enabled = Form1.MenuOptionsShowChartLegends.Checked,
            .Font = New Font(familyName:="Segoe UI", emSize:=20.0F, style:=FontStyle.Bold),
            .ForeColor = .BackColor.GetContrastingColor(),
            .IsTextAutoFit = True}
    End Function

    ''' <summary>
    '''  Creates a series for displaying active insulin values in a chart.
    '''  This method is used to initialize a series specifically for active insulin data.
    ''' </summary>
    ''' <returns>A Series object configured for active insulin values.</returns>
    Friend Function CreateSeriesActiveInsulin() As Series
        Dim s As Series = CreateSeriesBase(ActiveInsulinSeriesName, legendText:="Active Insulin", borderWidth:=4, yAxisType:=AxisType.Primary)
        s.MarkerColor = Color.Black
        s.MarkerSize = 4
        s.MarkerStyle = MarkerStyle.Circle
        Return s
    End Function

    ''' <summary>
    '''  Creates a series for displaying basal insulin values in a chart.
    '''  This method is used to initialize a series specifically for basal insulin data.
    ''' </summary>
    ''' <param name="SeriesName"></param>
    ''' <param name="basalLegend"></param>
    ''' <param name="legendText"></param>
    ''' <param name="YAxisType"></param>
    ''' <returns>A Series object configured for basal insulin values.</returns>
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

    ''' <summary>
    '''  Creates a series for displaying limits and target values in a chart.
    '''  This method is used to initialize series for high and low limits, as well as the sensor glucose target.
    ''' </summary>
    ''' <param name="limitsLegend">The legend associated with the limits series.</param>
    ''' <param name="seriesName">The name of the series.</param>
    ''' <returns>A Series object configured for limit and target values.</returns>
    Friend Function CreateSeriesLimitsAndTarget(limitsLegend As Legend, seriesName As String) As Series
        Dim legendText As String
        Dim lineColor As Color
        Dim borderWidth As Integer = 2
        Select Case seriesName
            Case HighLimitSeriesName
                legendText = "High Limit"
            Case LowLimitSeriesName
                legendText = "Low Limit"
            Case Else
                legendText = "SG Target"
                borderWidth = 4
        End Select
        lineColor = GetGraphLineColor(legendText)
        Dim s As Series = CreateSeriesBase(seriesName, legendText, borderWidth, AxisType.Secondary)
        s.IsVisibleInLegend = False
        s.EmptyPointStyle.Color = Color.Transparent
        limitsLegend.CustomItems.Add(New LegendItem(name:=legendText, color:=GetGraphLineColor(legendText), image:=""))
        Return s
    End Function

    ''' <summary>
    '''  Creates a series for displaying sensor glucose (SG) values in a chart.
    '''  This method is used to initialize a series specifically for sensor glucose data.
    ''' </summary>
    ''' <param name="sgLegend">The legend associated with the sensor glucose series.</param>
    ''' <returns>A Series object configured for sensor glucose values.</returns>
    Friend Function CreateSeriesSg(sgLegend As Legend) As Series
        Const legendText As String = "SG Series"
        Dim s As Series = CreateSeriesBase(SgSeriesName, legendText, 4, AxisType.Secondary)
        s.IsVisibleInLegend = False
        sgLegend.CustomItems.Add(New LegendItem(legendText, GetGraphLineColor(legendText), ""))
        Return s
    End Function

    ''' <summary>
    '''  Creates a series for displaying sensor glucose (SG) values in a chart with a specific name.
    '''  This method is used to initialize a series specifically for sensor glucose data with a custom name.
    ''' </summary>
    ''' <param name="sgLegend">The legend associated with the sensor glucose series.</param>
    ''' <param name="seriesName">The name of the series.</param>
    ''' <returns>A Series object configured for sensor glucose values.</returns>
    ''' <param name="basalLegend"></param>
    ''' <returns></returns>
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

    ''' <summary>
    '''  Creates a series for displaying time change values in a chart.
    '''  This method is used to initialize a series specifically for time change data.
    ''' </summary>
    ''' <param name="basalLegend">The legend associated with the time change series.</param>
    ''' <returns>A Series object configured for time change values.</returns>
    Friend Function CreateSeriesTimeChange(basalLegend As Legend) As Series
        Const legendName As String = "Time Change"
        Dim s As Series = CreateSeriesBase(TimeChangeSeriesName, legendName, borderWidth:=1, AxisType.Primary)
        s.IsVisibleInLegend = False
        basalLegend.CustomItems.Add(New LegendItem(legendName, GetGraphLineColor(legendName), ""))
        With s.EmptyPointStyle
            .BorderWidth = 4
            .Color = Color.Transparent
        End With
        Return s
    End Function

    ''' <summary>
    '''  Creates a series for displaying markers in a chart without showing the legend.
    '''  This method is used to initialize a series specifically for markers data, ensuring it does not appear in the legend.
    ''' </summary>
    ''' <param name="YAxisType">The Y-axis type for the series.</param>
    ''' <returns>A Series object configured for markers without visible legend.</returns>
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
            .YAxisType = YAxisType}
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

    ''' <summary>
    '''  Creates a title for a chart with specific properties.
    '''  This method is used to initialize a title for the chart, including text, font, and color.
    ''' </summary>
    ''' <param name="chartTitle">The text of the chart title.</param>
    ''' <param name="name">The name of the title.</param>
    ''' <param name="foreColor">The foreground color of the title.</param>
    ''' <returns>A Title object configured with the specified properties.</returns>
    Friend Function CreateTitle(chartTitle As String, name As String, foreColor As Color) As Title
        Return New Title With {
            .BackColor = foreColor.GetContrastingColor(),
            .Font = New Font(familyName:="Segoe UI", emSize:=14.0F, style:=FontStyle.Bold),
            .ForeColor = foreColor,
            .Name = name,
            .ShadowColor = Color.FromArgb(alpha:=32, baseColor:=Color.Black),
            .ShadowOffset = 3,
            .Text = chartTitle}
    End Function

    ''' <summary>
    '''  Updates the X-axis of a chart area for sensor glucose (SG) data.
    '''  This method adjusts the X-axis properties based on the current SG records and the pump's current time.
    ''' </summary>
    ''' <param name="c">The ChartArea to update.</param>
    ''' <remarks>This method sets the interval, grid lines, and maximum/minimum values for the X-axis.</remarks>
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
                    c.AxisX.Maximum = New OADate(PumpNow)
                    c.AxisX.Minimum = New OADate(PumpNow.AddDays(-1))
                Else
                    c.AxisX.Maximum = s_listOfSgRecords.Aggregate(Function(i1, i2) If(i1.OaDateTime > i2.OaDateTime, i1, i2)).OaDateTime
                    c.AxisX.Minimum = s_listOfSgRecords.Aggregate(Function(i1, i2) If(i1.OaDateTime < i2.OaDateTime, i1, i2)).OaDateTime
                End If
            End With
        End With
    End Sub

End Module
