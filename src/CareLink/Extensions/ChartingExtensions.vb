' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Module ChartingExtensions

    <Extension>
    Private Sub AddBasalPoint(basalSeries As Series, startX As OADate, StartY As Double, lineColor As Color, toolTip As String)
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
            basalSeries.Points.Last.ToolTip = toolTip
        End If

    End Sub

    <Extension>
    Private Sub AddBgReadingPoint(markerSeriesPoints As DataPointCollection, markerOADate As OADate, bgValueString As String, bgValue As Single)
        markerSeriesPoints.AddXY(markerOADate, bgValue)
        markerSeriesPoints.Last.BorderColor = Color.Gainsboro
        markerSeriesPoints.Last.Color = Color.FromArgb(5, Color.Gainsboro)
        markerSeriesPoints.Last.MarkerSize = 10
        markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Circle
        If Not Single.IsNaN(bgValue) Then
            markerSeriesPoints.Last.ToolTip = $"Blood Glucose: Not used For calibration: {bgValueString} {BgUnitsString}"
        End If
    End Sub

    <Extension>
    Private Sub AddCalibrationPoint(markerSeriesPoints As DataPointCollection, markerOADate As OADate, bgValue As Single, entry As Dictionary(Of String, String))
        markerSeriesPoints.AddXY(markerOADate, bgValue)
        markerSeriesPoints.Last.BorderColor = Color.Red
        markerSeriesPoints.Last.Color = Color.FromArgb(5, Color.Red)
        markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Circle
        markerSeriesPoints.Last.MarkerBorderWidth = 2
        markerSeriesPoints.Last.MarkerSize = 8
        markerSeriesPoints.Last.ToolTip = $"Blood Glucose: Calibration {If(CBool(entry("calibrationSuccess")), "accepted", "not accepted")}: {entry("value")} {BgUnitsString}"
    End Sub

    <Extension>
    Friend Sub DrawBasalMarker(ByRef basalSeries As Series, markerOADate As OADate, amount As Single, bolusRow As Double, insulinRow As Double, lineColor As Color, toolTip As String)
        Dim startX As OADate
        Dim startY As Double
        If Math.Abs(amount - 0.025) < 0.001 Then
            lineColor = Color.LightYellow
        End If
        If basalSeries.Legend.StartsWith(NameOf(My.Forms.Form1.TreatmentMarkersChartLegend)) Then
            startX = markerOADate + s_twoHalfMinuteOADate
            startY = amount.RoundSingle(3)
            basalSeries.AddBasalPoint(startX, 0, lineColor, toolTip)
            basalSeries.AddBasalPoint(startX, startY, lineColor, toolTip)
            basalSeries.AddBasalPoint(startX, 0, lineColor, toolTip)
            basalSeries.AddBasalPoint(startX, Double.NaN, Color.Transparent, toolTip)
        Else
            startX = markerOADate + s_twoHalfMinuteOADate
            startY = bolusRow - ((bolusRow - insulinRow) * (amount / MaxBasalPerDose))
            basalSeries.AddBasalPoint(startX, bolusRow, lineColor, toolTip)
            basalSeries.AddBasalPoint(startX, startY, lineColor, toolTip)
            basalSeries.AddBasalPoint(startX, bolusRow, lineColor, toolTip)
            basalSeries.AddBasalPoint(startX, Double.NaN, Color.Transparent, toolTip)
        End If

    End Sub

    <Extension>
    Friend Sub PlotHomePageMarkers(chart As Chart, chartRelitivePosition As RectangleF, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        For Each markerWithIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Try
                Dim markerDateTime As Date = s_markers.SafeGetSgDateTime(markerWithIndex.Index)
                Dim markerOADateTime As New OADate(markerDateTime)
                Dim bgValueString As String = ""
                Dim bgValue As Single
                Dim entry As Dictionary(Of String, String) = markerWithIndex.Value

                If entry.TryGetValue("value", bgValueString) Then
                    bgValueString.TryParseSingle(bgValue)
                End If
                Dim markerSeriesPoints As DataPointCollection = chart.Series(MarkerSeriesName).Points
                Select Case entry("type")
                    Case "BG_READING"
                        If Not String.IsNullOrWhiteSpace(bgValueString) Then
                            markerSeriesPoints.AddBgReadingPoint(markerOADateTime, bgValueString, bgValue)
                        End If
                    Case "CALIBRATION"
                        markerSeriesPoints.AddCalibrationPoint(markerOADateTime, bgValue, entry)
                    Case "AUTO_BASAL_DELIVERY"
                        Dim bolusAmount As String = entry("bolusAmount")
                        DrawBasalMarker(chart.Series(BasalSeriesName), markerOADateTime, bolusAmount.ParseSingle, HomePageBasalRow, HomePageInsulinRow, Color.HotPink, $"Auto Basal:{bolusAmount.TruncateSingleString(3)} U")
                    Case "INSULIN"
                        Select Case entry("activationType")
                            Case "AUTOCORRECTION"
                                Dim autoCorrection As String = entry("deliveredFastAmount")
                                DrawBasalMarker(chart.Series(BasalSeriesName), markerOADateTime, autoCorrection.ParseSingle, HomePageBasalRow, HomePageInsulinRow, Color.Aqua, $"Auto Correction: {autoCorrection.TruncateSingleString(3)} U")
                            Case "RECOMMENDED", "UNDETERMINED"
                                If s_homeTabMarkerInsulinDictionary.TryAdd(markerOADateTime, CInt(HomePageInsulinRow)) Then
                                    markerSeriesPoints.AddXY(markerOADateTime, HomePageInsulinRow - 10)
                                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                                    markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Black)
                                    markerSeriesPoints.Last.MarkerSize = 20
                                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                                    If Double.IsNaN(HomePageInsulinRow) Then
                                        markerSeriesPoints.Last.Color = Color.Transparent
                                        markerSeriesPoints.Last.MarkerSize = 0
                                    Else
                                        markerSeriesPoints.Last.Color = Color.FromArgb(30, Color.LightBlue)
                                        markerSeriesPoints.Last.ToolTip = $"Bolus: {entry("deliveredFastAmount")} U"
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        If s_homeTabMarkerMealDictionary.TryAdd(markerOADateTime, HomePageMealRow) Then
                            markerSeriesPoints.AddXY(markerOADateTime, HomePageMealRow + (s_mealImage.Height / 2))
                            markerSeriesPoints.Last.Color = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerBorderWidth = 2
                            markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerSize = 20
                            markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                            markerSeriesPoints.Last.ToolTip = $"Meal:{entry("amount")} grams"
                        End If
                    Case "AUTO_MODE_STATUS", "LOW_GLUCOSE_SUSPENDED"
                    Case "TIME_CHANGE"
                        With chart.Series(TimeChangeSeriesName).Points
                            markerOADateTime = New TimeChangeRecord(s_markers(markerWithIndex.Index)).currentOADate
                            .AddXY(markerOADateTime, 0)
                            .AddXY(markerOADateTime, HomePageBasalRow)
                            .AddXY(markerOADateTime, Double.NaN)
                        End With
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
                '      Throw New Exception($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
            End Try
        Next

    End Sub

    <Extension>
    Friend Sub PlotOnePoint(plotSeries As Series, sgOADateTime As OADate, bgValue As Single, mainLineColor As Color, HomePageMealRow As Double, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            With plotSeries.Points
                Dim sgDouble As Double = sgOADateTime
                If Single.IsNaN(bgValue) OrElse Math.Abs(bgValue - 0) < Single.Epsilon Then
                    .AddXY(sgDouble, HomePageMealRow)
                    .Last().Color = Color.Transparent
                    .Last().IsEmpty = True
                Else
                    If .Count > 0 AndAlso (Not .Last.IsEmpty) AndAlso New OADate(plotSeries.Points.Last.XValue).Within10Minutes(sgOADateTime) Then
                        plotSeries.Points.AddXY(.Last.XValue, Double.NaN)
                        .Last().Color = Color.Transparent
                        .Last().IsEmpty = True
                    End If
                    .AddXY(sgDouble, bgValue)
                    If bgValue > s_limitHigh Then
                        .Last.Color = Color.Lime
                    ElseIf bgValue < s_limitLow Then
                        .Last.Color = Color.Red
                    Else
                        .Last.Color = mainLineColor
                    End If

                End If
            End With
        Catch ex As Exception
            Throw New Exception($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try

    End Sub

    <Extension>
    Friend Sub PlotSgSeries(chartSeries As Series, HomePageMealRow As Double)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_bindingSourceSGs.WithIndex()
            chartSeries.PlotOnePoint(sgListIndex.Value.OADate(),
                                    sgListIndex.Value.sg,
                                    Color.Black,
                                    HomePageMealRow)
        Next
    End Sub

    <Extension>
    Friend Sub PlotTreatmentMarkers(chart As Chart, chartRelitivePosition As RectangleF, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        For Each markerWithIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Try
                Dim markerDateTime As Date = s_markers.SafeGetSgDateTime(markerWithIndex.Index)
                Dim markerOADateTime As New OADate(markerDateTime)
                Dim bgValueString As String = ""
                Dim bgValue As Single
                Dim entry As Dictionary(Of String, String) = markerWithIndex.Value

                If entry.TryGetValue("value", bgValueString) Then
                    bgValueString.TryParseSingle(bgValue)
                End If
                Dim markerSeriesPoints As DataPointCollection = chart.Series(MarkerSeriesName).Points
                Select Case entry("type")
                    Case "AUTO_BASAL_DELIVERY"
                        Dim bolusAmount As String = entry("bolusAmount")
                        DrawBasalMarker(chart.Series(BasalSeriesName), markerOADateTime, bolusAmount.ParseSingle.RoundSingle(3), MaxBasalPerDose, TreatmentInsulinRow, Color.HotPink, $"Auto Basal:{bolusAmount.TruncateSingleString(3)} U")
                    Case "INSULIN"
                        Select Case entry("activationType")
                            Case "AUTOCORRECTION"
                                Dim autoCorrection As String = entry("deliveredFastAmount")
                                DrawBasalMarker(chart.Series(BasalSeriesName), markerOADateTime, autoCorrection.ParseSingle, MaxBasalPerDose, TreatmentInsulinRow, Color.Aqua, $"Auto Correction: {autoCorrection.TruncateSingleString(3)} U")
                            Case "RECOMMENDED", "UNDETERMINED"
                                If s_treatmentMarkerInsulinDictionary.TryAdd(markerOADateTime, TreatmentInsulinRow) Then
                                    markerSeriesPoints.AddXY(markerOADateTime, TreatmentInsulinRow)
                                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                                    markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Black)
                                    markerSeriesPoints.Last.MarkerSize = 20
                                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                                    If Double.IsNaN(HomePageInsulinRow) Then
                                        markerSeriesPoints.Last.Color = Color.Transparent
                                        markerSeriesPoints.Last.MarkerSize = 0
                                    Else
                                        markerSeriesPoints.Last.Color = Color.FromArgb(30, Color.LightBlue)
                                        markerSeriesPoints.Last.ToolTip = $"Bolus: {entry("deliveredFastAmount")} U"
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        Dim mealRow As Single = CSng(TreatmentInsulinRow * 0.95).RoundSingle(3)
                        If s_treatmentMarkerMealDictionary.TryAdd(markerOADateTime, mealRow) Then
                            markerSeriesPoints.AddXY(markerOADateTime, mealRow)
                            markerSeriesPoints.Last.Color = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerBorderWidth = 2
                            markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerSize = 20
                            markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                            markerSeriesPoints.Last.ToolTip = $"Meal:{entry("amount")} grams"
                        End If
                    Case "AUTO_MODE_STATUS",
                         "BG_READING",
                         "CALIBRATION",
                         "LOW_GLUCOSE_SUSPENDED"
                    Case "TIME_CHANGE"
                        With chart.Series(TimeChangeSeriesName).Points
                            markerOADateTime = New TimeChangeRecord(s_markers(markerWithIndex.Index)).previousOADate
                            .AddXY(markerOADateTime, 0)
                            .AddXY(markerOADateTime, TreatmentInsulinRow)
                            .AddXY(markerOADateTime, Double.NaN)
                        End With
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
                '      Throw New Exception($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
            End Try
        Next

    End Sub

    <Extension>
    Friend Sub PostPaintSupport(e As ChartPaintEventArgs, ByRef chartRelitivePosition As RectangleF, topRow As Single, insulinDictionary As Dictionary(Of OADate, Single), mealDictionary As Dictionary(Of OADate, Single), doShading As Boolean, offsetInsulinImage As Boolean, Optional homePageCursorTimeLabel As Label = Nothing)
        Debug.Print("At SyncLock")

        If chartRelitivePosition.IsEmpty Then
            chartRelitivePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, s_bindingSourceSGs(0).OADate))
            chartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, topRow))
            chartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_limitHigh)))) - chartRelitivePosition.Y
            chartRelitivePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, s_bindingSourceSGs.Last.OADate)) - chartRelitivePosition.X
        End If

        If homePageCursorTimeLabel?.Tag IsNot Nothing Then
            homePageCursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, homePageCursorTimeLabel.Tag.ToString.ParseDate("").ToOADate))
        End If

        If doShading Then
            Dim highLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_limitHigh))
            Dim lowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_limitLow))
            Dim criticalLowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, s_criticalLow))
            Dim chartAbsoluteHighRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(New RectangleF(chartRelitivePosition.X, chartRelitivePosition.Y, chartRelitivePosition.Width, highLimitY - chartRelitivePosition.Y))
            Dim chartAbsoluteLowRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(New RectangleF(chartRelitivePosition.X, lowLimitY, chartRelitivePosition.Width, criticalLowLimitY - lowLimitY))
            Using b As New SolidBrush(Color.FromArgb(15, Color.Black))
                e.ChartGraphics.Graphics.FillRectangle(b, chartAbsoluteHighRectangle)
                e.ChartGraphics.Graphics.FillRectangle(b, chartAbsoluteLowRectangle)
            End Using
        End If

        If insulinDictionary IsNot Nothing Then
            e.PaintMarker(s_mealImage, mealDictionary, False)
            e.PaintMarker(s_insulinImage, insulinDictionary, offsetInsulinImage)
        End If
    End Sub

End Module
