' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

''' <summary>
'''  Provides extension methods for plotting basal insulin data points and series on a chart.
''' </summary>
Friend Module PlotSeriesBasal
    ''' <summary>
    '''  Adds a basal data point to the specified <see cref="Series"/> for charting.
    ''' </summary>
    ''' <param name="basalSeries">
    '''  The <see cref="Series"/> to which the basal data point will be added.
    ''' </param>
    ''' <param name="xValue">The X value (OADate) for the new point.</param>
    ''' <param name="yValue">The Y value for the new point.</param>
    ''' <param name="lineColor">The <see cref="Color"/> to use for the point.</param>
    ''' <param name="tag">A tag string to associate with the point.</param>
    ''' <remarks>
    '''   <para>
    '''    Handles special cases for empty points and color assignment.
    '''    If the previous point is not empty and is within 6 minutes of the new point,
    '''    an empty point is inserted for visual separation.
    '''   </para>
    ''' </remarks>
    <Extension>
    Private Sub AddBasalPoint(
        basalSeries As Series,
        xValue As OADate,
        yValue As Double,
        lineColor As Color,
        tag As String)

        If basalSeries.Points.Count > 0 AndAlso
           (Not basalSeries.Points.Last.IsEmpty) AndAlso
           New OADate(oADateAsDouble:=basalSeries.Points.Last.XValue).Within6Min(xValue) Then

            basalSeries.Points.AddXY(basalSeries.Points.Last, Double.NaN)
            basalSeries.Points.Last().Color = Color.Transparent
            basalSeries.Points.Last().IsEmpty = True
        End If
        basalSeries.Points.AddXY(xValue, yValue)
        If Double.IsNaN(yValue) Then
            basalSeries.Points.Last.Color = Color.Transparent
            basalSeries.Points.Last.MarkerSize = 0
            basalSeries.Points.Last.IsEmpty = True
        Else
            basalSeries.Points.Last.Color = lineColor
            basalSeries.Points.Last.Tag = tag
        End If
    End Sub

    ''' <summary>
    '''  Plots a basal insulin series on the provided <see cref="Series"/> object.
    ''' </summary>
    ''' <param name="basalSeries">The <see cref="Series"/> to plot on.</param>
    ''' <param name="markerOADateTime">The OADate time marker for the basal event.</param>
    ''' <param name="amount">The basal insulin amount.</param>
    ''' <param name="bolusRow">The Y position for the bolus row.</param>
    ''' <param name="insulinRow">The Y position for the insulin row.</param>
    ''' <param name="legendText">The legend text for color selection.</param>
    ''' <param name="DrawFromBottom">
    '''   <see langword="True"/> to draw the basal from the bottom of the chart; otherwise,
    '''   draws at the specified row.
    ''' </param>
    ''' <param name="tag">A tag string to associate with the points.</param>
    ''' <remarks>
    '''  Draws the basal profile either from the bottom or at a specified row,
    '''  with color and tag information.
    '''  Uses <see cref="AddBasalPoint"/> to add each segment of the basal series.
    ''' </remarks>
    <Extension>
    Friend Sub PlotBasalSeries(
         basalSeries As Series,
         markerOADateTime As OADate,
         amount As Single,
         bolusRow As Double,
         insulinRow As Double,
         legendText As String,
         DrawFromBottom As Boolean,
         tag As String)

        Dim xValue As OADate
        Dim yValue As Double
        Dim lineColor As Color = If(amount.IsMinBasal(),
                                    GetGraphLineColor(key:="Min Basal"),
                                    GetGraphLineColor(legendText))

        xValue = markerOADateTime + TwoMinutes30SecondsOADate
        If DrawFromBottom Then
            yValue = amount.RoundTo025()
            basalSeries.AddBasalPoint(xValue, yValue:=0, lineColor, tag)
            basalSeries.AddBasalPoint(xValue, yValue, lineColor, tag)
            basalSeries.AddBasalPoint(xValue, yValue:=0, lineColor, tag)
        Else
            yValue = bolusRow - ((bolusRow - insulinRow) * (amount.RoundTo025() / MaxBasalPerDose))
            basalSeries.AddBasalPoint(xValue, yValue:=bolusRow, lineColor, tag)
            basalSeries.AddBasalPoint(xValue, yValue, lineColor, tag)
            basalSeries.AddBasalPoint(xValue, yValue:=bolusRow, lineColor, tag)
        End If
        basalSeries.AddBasalPoint(xValue, yValue:=Double.NaN, lineColor:=Color.Transparent, tag)
    End Sub

End Module
