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
    ''' <param name="basalSeries">The <see cref="Series"/> to which the basal data point will be added.</param>
    ''' <param name="startX">The X value (OADate) for the new point.</param>
    ''' <param name="StartY">The Y value for the new point.</param>
    ''' <param name="lineColor">The <see cref="Color"/> to use for the point.</param>
    ''' <param name="tagString">A tag string to associate with the point.</param>
    ''' <remarks>
    '''   <para>
    '''    Handles special cases for empty points and color assignment. If the previous point is not empty and is within
    '''    6 minutes of the new point, an empty point is inserted for visual separation.
    '''   </para>
    ''' </remarks>
    <Extension>
    Private Sub AddBasalPoint(
        basalSeries As Series,
        startX As OADate,
        StartY As Double,
        lineColor As Color,
        tagString As String)

        If basalSeries.Points.Count > 0 AndAlso (Not basalSeries.Points.Last.IsEmpty) AndAlso
           New OADate(basalSeries.Points.Last.XValue).Within6Minutes(startX) Then
            basalSeries.Points.AddXY(basalSeries.Points.Last, Double.NaN)
            basalSeries.Points.Last().Color = Color.Transparent
            basalSeries.Points.Last().IsEmpty = True
        End If
        basalSeries.Points.AddXY(startX, StartY)
        If Double.IsNaN(StartY) Then
            basalSeries.Points.Last.Color = Color.Transparent
            basalSeries.Points.Last.MarkerSize = 0
            basalSeries.Points.Last.IsEmpty = True
        Else
            basalSeries.Points.Last.Color = lineColor
            basalSeries.Points.Last.Tag = tagString
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
    '''   <para>
    '''    Draws the basal profile either from the bottom or at a specified row, with color and tag information.
    '''    Uses <see cref="AddBasalPoint"/> to add each segment of the basal series.
    '''   </para>
    ''' </remarks>
    <Extension>
    Friend Sub PlotBasalSeries(
         ByRef basalSeries As Series,
         markerOADateTime As OADate,
         amount As Single,
         bolusRow As Double,
         insulinRow As Double,
         legendText As String,
         DrawFromBottom As Boolean,
         tag As String)

        Dim startX As OADate
        Dim startY As Double
        Dim lineColor As Color = If(amount.IsMinBasal(),
                                     GetGraphLineColor("Min Basal"),
                                     GetGraphLineColor(legendText)
                                    )

        If DrawFromBottom Then
            startX = markerOADateTime + TwoMinutes30SecondsOADate
            startY = amount.RoundTo025
            basalSeries.AddBasalPoint(startX, 0, lineColor, tag)
            basalSeries.AddBasalPoint(startX, startY, lineColor, tag)
            basalSeries.AddBasalPoint(startX, 0, lineColor, tag)
            basalSeries.AddBasalPoint(startX, Double.NaN, Color.Transparent, tag)
        Else
            startX = markerOADateTime + TwoMinutes30SecondsOADate
            startY = bolusRow - ((bolusRow - insulinRow) * (amount / MaxBasalPerDose))
            basalSeries.AddBasalPoint(startX, bolusRow, lineColor, tag)
            basalSeries.AddBasalPoint(startX, startY, lineColor, tag)
            basalSeries.AddBasalPoint(startX, bolusRow, lineColor, tag)
            basalSeries.AddBasalPoint(startX, Double.NaN, Color.Transparent, tag)
        End If
    End Sub

End Module
