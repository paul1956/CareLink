' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Module ChartingExtensions

    <Extension>
    Friend Sub DrawBasalMarker(ByRef basalSeries As Series, markerOADateTime As Double, amount As Single, bolusRow As Double, insulinRow As Double, lineColor As Color, toolTip As String)
        Dim markerBasalPoints As DataPointCollection = basalSeries.Points
        Dim yValue As Double = bolusRow - ((bolusRow - insulinRow) * (amount / s_maxBasalPerDose))
        Dim twoMinutes As Double = s_fiveMinuteOADate / 2
        markerBasalPoints.AddXY(markerOADateTime + twoMinutes, bolusRow)
        markerBasalPoints.Last.ToolTip = toolTip
        markerBasalPoints.Last.Color = lineColor
        markerBasalPoints.AddXY(markerOADateTime + twoMinutes, yValue)
        markerBasalPoints.Last.ToolTip = toolTip
        markerBasalPoints.Last.Color = lineColor
        markerBasalPoints.AddXY(markerOADateTime + twoMinutes, yValue)
        markerBasalPoints.Last.ToolTip = toolTip
        markerBasalPoints.Last.Color = lineColor
        markerBasalPoints.AddXY(markerOADateTime + twoMinutes, bolusRow)
        markerBasalPoints.Last.ToolTip = toolTip
        markerBasalPoints.Last.Color = lineColor
        markerBasalPoints.AddXY(markerOADateTime, Double.NaN)
        markerBasalPoints.Last.ToolTip = toolTip
        markerBasalPoints.Last.Color = lineColor
    End Sub

    Friend Sub PlotMarkers(chart As Chart, bolusRow As Double, insulinRow As Double, mealRow As Single, markerInsulinDictionary As Dictionary(Of Double, Single), markerMealDictionary As Dictionary(Of Double, Single))
        For Each markerWithIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Dim markerDateTime As Date = s_markers.SafeGetSgDateTime(markerWithIndex.Index)
            Dim markerOADateTime As Double = markerDateTime.ToOADate()
            Dim bgValueString As String = ""
            Dim bgValue As Single
            Dim entry As Dictionary(Of String, String) = markerWithIndex.Value

            If entry.TryGetValue("value", bgValueString) Then
                bgValueString.TryParseSingle(bgValue)
            End If
            Dim markerSeriesPoints As DataPointCollection = chart.Series(MarkerSeriesName).Points
            Select Case entry("type")
                Case "BG_READING"
                    If String.IsNullOrWhiteSpace(bgValueString) Then
                        markerSeriesPoints.AddXY(markerOADateTime, bgValue)
                        markerSeriesPoints.Last.BorderColor = Color.Gainsboro
                        markerSeriesPoints.Last.Color = Color.Transparent
                        markerSeriesPoints.Last.MarkerBorderWidth = 2
                        markerSeriesPoints.Last.MarkerSize = 10
                        markerSeriesPoints.Last.ToolTip = $"Blood Glucose: Not used For calibration: {bgValueString} {BgUnitsString}"
                    End If
                Case "CALIBRATION"
                    markerSeriesPoints.AddXY(markerOADateTime, bgValue)
                    markerSeriesPoints.Last.BorderColor = Color.Red
                    markerSeriesPoints.Last.Color = Color.Transparent
                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                    markerSeriesPoints.Last.MarkerSize = 8
                    markerSeriesPoints.Last.ToolTip = $"Blood Glucose: Calibration {If(CBool(entry("calibrationSuccess")), "accepted", "not accepted")}: {entry("value")} {BgUnitsString}"
                Case "AUTO_BASAL_DELIVERY"
                    Dim bolusAmount As String = entry("bolusAmount")
                    DrawBasalMarker(chart.Series(BasalSeriesName), markerOADateTime, bolusAmount.ParseSingle, bolusRow, insulinRow, Color.HotPink, $"Auto Basal:{bolusAmount.TruncateSingleString(3)} U")
                Case "INSULIN"
                    Select Case entry("activationType")
                        Case "AUTOCORRECTION"
                            Dim autoCorrection As String = entry("deliveredFastAmount")
                            DrawBasalMarker(chart.Series(BasalSeriesName), markerOADateTime, autoCorrection.ParseSingle, bolusRow, insulinRow, Color.Aqua, $"Auto Correction: {autoCorrection.TruncateSingleString(3)} U")
                        Case "RECOMMENDED", "UNDETERMINED"
                            If markerInsulinDictionary.TryAdd(markerOADateTime, CInt(insulinRow)) Then
                                markerSeriesPoints.AddXY(markerOADateTime, insulinRow)
                                markerSeriesPoints.Last.Color = Color.FromArgb(30, Color.LightBlue)
                                markerSeriesPoints.Last.ToolTip = $"Bolus: {entry("deliveredFastAmount")} U"
                            Else
                                Stop
                            End If
                        Case Else
                            Stop
                    End Select
                    markerSeriesPoints.Last.MarkerBorderWidth = 0
                    markerSeriesPoints.Last.MarkerSize = 15
                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                Case "MEAL"
                    If markerMealDictionary.TryAdd(markerOADateTime, mealRow) Then
                        markerSeriesPoints.AddXY(markerOADateTime, mealRow)
                        markerSeriesPoints.Last.Color = Color.FromArgb(30, Color.Yellow)
                        markerSeriesPoints.Last.MarkerBorderWidth = 0
                        markerSeriesPoints.Last.MarkerSize = 30
                        markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                        markerSeriesPoints.Last.ToolTip = $"Meal:{entry("amount")} grams"
                    End If
                Case "AUTO_MODE_STATUS", "LOW_GLUCOSE_SUSPENDED"
                Case "TIME_CHANGE"
                    With chart.Series(TimeChangeSeriesName).Points
                        .AddXY(markerOADateTime, 0)
                        .AddXY(markerOADateTime, bolusRow)
                        .AddXY(markerOADateTime, Double.NaN)
                    End With
                Case Else
                    Stop
            End Select
        Next

    End Sub

    <Extension>
    Friend Sub PlotOnePoint(plotSeries As Series, sgOADateTime As Double, bgValue As Single, mainLineColor As Color, MealRow As Double, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            With plotSeries
                If Single.IsNaN(bgValue) OrElse Math.Abs(bgValue - 0) < Single.Epsilon Then
                    .Points.AddXY(sgOADateTime, MealRow)
                    .Points.Last().IsEmpty = True
                Else
                    .Points.AddXY(sgOADateTime, bgValue)
                    If bgValue > s_limitHigh Then
                        .Points.Last.Color = Color.Lime
                    ElseIf bgValue < s_limitLow Then
                        .Points.Last.Color = Color.Red
                    Else
                        .Points.Last.Color = mainLineColor
                    End If

                End If
            End With
        Catch ex As Exception
            Throw New Exception($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try

    End Sub

    <Extension>
    Friend Sub PlotSgSeries(chartSeries As Series, MealRow As Double)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_bindingSourceSGs.WithIndex()
            chartSeries.PlotOnePoint(sgListIndex.Value.OADate(),
                                    sgListIndex.Value.sg,
                                    Color.Black,
                                    MealRow)
        Next
    End Sub

    <Extension>
    Friend Sub PostPaintSupport(ByRef chartRelitivePosition As RectangleF, e As ChartPaintEventArgs, insulinRow As Single, insulinDictionary As Dictionary(Of Double, Single), mealDictionary As Dictionary(Of Double, Single), Optional homePageCursorTimeLabel As Label = Nothing)
        Debug.Print("At SyncLock")

        If chartRelitivePosition.IsEmpty Then
            chartRelitivePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, s_bindingSourceSGs(0).OADate))
            chartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, insulinRow))
            chartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_limitHigh)))) - chartRelitivePosition.Y
            chartRelitivePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, s_bindingSourceSGs.Last.OADate)) - chartRelitivePosition.X
        End If
        Dim highLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_limitHigh))
        Dim lowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_limitLow))
        Dim criticalLowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_criticalLow))
        Dim chartAbsoluteHighRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(New RectangleF(chartRelitivePosition.X, chartRelitivePosition.Y, chartRelitivePosition.Width, highLimitY - chartRelitivePosition.Y))
        Dim chartAbsoluteLowRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(New RectangleF(chartRelitivePosition.X, lowLimitY, chartRelitivePosition.Width, criticalLowLimitY - lowLimitY))

        Using b As New SolidBrush(Color.FromArgb(15, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, chartAbsoluteHighRectangle)
            e.ChartGraphics.Graphics.FillRectangle(b, chartAbsoluteLowRectangle)
        End Using

        If homePageCursorTimeLabel?.Tag IsNot Nothing Then
            homePageCursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, homePageCursorTimeLabel.Tag.ToString.ParseDate("").ToOADate))
        End If

        e.PaintMarker(s_mealImage, mealDictionary)
        e.PaintMarker(s_insulinImage, insulinDictionary)
    End Sub

End Module
