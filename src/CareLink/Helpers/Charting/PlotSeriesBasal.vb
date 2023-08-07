' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSeriesBasal

    <Extension>
    Private Sub AddBasalPoint(basalSeries As Series, startX As OADate, StartY As Double, lineColor As Color, tagString As String)
        If basalSeries.Points.Count > 0 AndAlso (Not basalSeries.Points.Last.IsEmpty) AndAlso New OADate(basalSeries.Points.Last.XValue).Within10Minutes(startX) Then
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

    <Extension>
    Friend Sub PlotBasalSeries(ByRef basalSeries As Series, markerOADate As OADate, amount As Single, bolusRow As Double, insulinRow As Double, seriesName As String, DrawFromBottom As Boolean, tag As String)
        Dim startX As OADate
        Dim startY As Double
        Dim lineColor As Color = If(amount.IsMinBasal(),
                                     GetGraphLineColor("Min Basal"),
                                     GetGraphLineColor(seriesName)
                                    )

        If DrawFromBottom Then
            startX = markerOADate + s_150SecondsOADate
            startY = amount.RoundTo025
            basalSeries.AddBasalPoint(startX, 0, lineColor, tag)
            basalSeries.AddBasalPoint(startX, startY, lineColor, tag)
            basalSeries.AddBasalPoint(startX, 0, lineColor, tag)
            basalSeries.AddBasalPoint(startX, Double.NaN, Color.Transparent, tag)
        Else
            startX = markerOADate + s_150SecondsOADate
            startY = bolusRow - ((bolusRow - insulinRow) * (amount / MaxBasalPerDose))
            basalSeries.AddBasalPoint(startX, bolusRow, lineColor, tag)
            basalSeries.AddBasalPoint(startX, startY, lineColor, tag)
            basalSeries.AddBasalPoint(startX, bolusRow, lineColor, tag)
            basalSeries.AddBasalPoint(startX, Double.NaN, Color.Transparent, tag)
        End If

    End Sub

End Module
