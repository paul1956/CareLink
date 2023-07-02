' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotMarkers

    <Extension>
    Private Sub AddSgReadingPoint(markerSeriesPoints As DataPointCollection, markerOADate As OADate, sgValueString As String, sgValue As Single)
        AddMarkerPoint(markerSeriesPoints, markerOADate, sgValue, Color.DarkOrange)
        If Not Single.IsNaN(sgValue) Then
            markerSeriesPoints.Last.Tag = $"Blood Glucose: Not used for calibration: {sgValueString} {SgUnitsNativeString}"
        End If
    End Sub

    <Extension>
    Private Sub AddCalibrationPoint(markerSeriesPoints As DataPointCollection, markerOADate As OADate, sgValue As Single, entry As Dictionary(Of String, String))
        AddMarkerPoint(markerSeriesPoints, markerOADate, sgValue, Color.Red)
        markerSeriesPoints.Last.Tag = $"Blood Glucose: Calibration {If(CBool(entry("calibrationSuccess")), "accepted", "not accepted")}: {entry("value")} {SgUnitsNativeString}"
    End Sub

    Private Sub AddMarkerPoint(markerSeriesPoints As DataPointCollection, markerOADate As OADate, sgValue As Single, markerColor As Color)
        markerSeriesPoints.AddXY(markerOADate, sgValue)
        markerSeriesPoints.Last.BorderColor = markerColor
        markerSeriesPoints.Last.Color = Color.FromArgb(5, markerColor)
        markerSeriesPoints.Last.MarkerBorderWidth = 3
        markerSeriesPoints.Last.MarkerSize = 8
        markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Circle
    End Sub

    <Extension>
    Private Sub AdjustXAxisStartTime(ByRef axisX As Axis, lastTimeChangeRecord As TimeChangeRecord)
        Dim latestTime As Date = If(lastTimeChangeRecord.previousDateTime > lastTimeChangeRecord.dateTime, lastTimeChangeRecord.previousDateTime, lastTimeChangeRecord.dateTime)
        Dim timeOffset As Double = (latestTime - s_listOfSgRecords(0).datetime).TotalMinutes
        axisX.IntervalOffset = timeOffset
        axisX.IntervalOffsetType = DateTimeIntervalType.Minutes
    End Sub

    Private Function GetToolTip(type As String, amount As Single) As String
        Dim minBasalMsg As String = ""
        If amount.IsMinBasal() Then
            minBasalMsg = "Min "
        End If

        Select Case type
            Case "AUTO_BASAL_DELIVERY"
                Return $"{minBasalMsg}Auto Basal: {amount}U"
            Case "MANUAL_BASAL_DELIVERY"
                Return $"{minBasalMsg}Manual Basal: {amount}U"
            Case Else
                Stop
                Return $"{minBasalMsg}Basal: {amount}U"
        End Select
    End Function

    <Extension>
    Friend Sub PlotMarkers(pageChart As Chart, timeChangeSeries As Series, chartRelativePosition As RectangleF, markerInsulinDictionary As Dictionary(Of OADate, Single), markerMealDictionary As Dictionary(Of OADate, Single), <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Dim lastTimeChangeRecord As TimeChangeRecord = Nothing
        markerInsulinDictionary.Clear()
        markerMealDictionary?.Clear()

        For Each markerWithIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Try
                Dim markerDateTime As Date = markerWithIndex.Value.GetMarkerDateTime
                Dim markerOADateTime As New OADate(markerDateTime)
                Dim sgValueString As String = ""
                Dim sgValue As Single
                Dim entry As Dictionary(Of String, String) = markerWithIndex.Value

                If entry.TryGetValue("value", sgValueString) Then
                    sgValueString.TryParseSingle(sgValue)
                    If Not Single.IsNaN(sgValue) Then
                        sgValue = Math.Min(GetYMaxValue(nativeMmolL), sgValue)
                        sgValue = Math.Max(GetYMinValue(nativeMmolL), sgValue)
                    End If
                End If
                Dim markerSeriesPoints As DataPointCollection = pageChart.Series(MarkerSeriesName).Points
                Select Case entry("type")
                    Case "BG_READING"
                        If Not String.IsNullOrWhiteSpace(sgValueString) Then
                            markerSeriesPoints.AddSgReadingPoint(markerOADateTime, sgValueString, sgValue)
                        End If
                    Case "CALIBRATION"
                        markerSeriesPoints.AddCalibrationPoint(markerOADateTime, sgValue, entry)
                    Case "AUTO_BASAL_DELIVERY"
                        Dim amount As Single = entry(NameOf(AutoBasalDeliveryRecord.bolusAmount)).ParseSingle(3)
                        With pageChart.Series(BasalSeriesNameName)
                            .PlotBasalSeries(markerOADateTime,
                                             amount,
                                             GetYMaxValue(nativeMmolL),
                                             GetInsulinYValue(),
                                             GetGraphLineColor("Basal Series"),
                                             False,
                                             GetToolTip(entry("type"), amount))
                        End With
                    Case "MANUAL_BASAL_DELIVERY"
                        Dim amount As Single = entry(NameOf(AutoBasalDeliveryRecord.bolusAmount)).ParseSingle(3)
                        With pageChart.Series(BasalSeriesNameName)
                            .PlotBasalSeries(markerOADateTime,
                                             amount,
                                             GetYMaxValue(nativeMmolL),
                                             GetInsulinYValue(),
                                             GetGraphLineColor("Basal Series"),
                                             False,
                                             GetToolTip(entry("type"), amount))
                        End With
                    Case "INSULIN"
                        Select Case entry(NameOf(InsulinRecord.activationType))
                            Case "AUTOCORRECTION"
                                Dim autoCorrection As String = entry(NameOf(InsulinRecord.deliveredFastAmount))
                                With pageChart.Series(BasalSeriesNameName)
                                    .PlotBasalSeries(markerOADateTime,
                                                     autoCorrection.ParseSingle(3),
                                                     GetYMaxValue(nativeMmolL),
                                                     GetInsulinYValue(),
                                                     GetGraphLineColor("Auto Correction"),
                                                     False,
                                                     $"Auto Correction: {autoCorrection.TruncateSingleString(3)}U")
                                End With
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If markerInsulinDictionary.TryAdd(markerOADateTime, CInt(GetInsulinYValue())) Then
                                    markerSeriesPoints.AddXY(markerOADateTime, GetInsulinYValue() - If(nativeMmolL, 0.555, 10))
                                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                                    markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Black)
                                    markerSeriesPoints.Last.MarkerSize = 20
                                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        markerSeriesPoints.Last.Color = Color.Transparent
                                        markerSeriesPoints.Last.MarkerSize = 0
                                    Else
                                        markerSeriesPoints.Last.Color = Color.FromArgb(30, Color.LightBlue)
                                        markerSeriesPoints.Last.Tag = $"Bolus: {entry(NameOf(InsulinRecord.deliveredFastAmount))}U"
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        If markerMealDictionary Is Nothing Then Continue For
                        If markerMealDictionary.TryAdd(markerOADateTime, GetYMinValue(nativeMmolL)) Then
                            markerSeriesPoints.AddXY(markerOADateTime, GetYMinValue(nativeMmolL) + If(nativeMmolL, s_mealImage.Height / 2 / MmolLUnitsDivisor, s_mealImage.Height / 2))
                            markerSeriesPoints.Last.Color = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerBorderWidth = 2
                            markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerSize = 20
                            markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                            markerSeriesPoints.Last.Tag = $"Meal:{entry("amount")} grams"
                        End If
                    Case "TIME_CHANGE"
                        With pageChart.Series(CreateChartItems.TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChangeRecord(entry)
                            markerOADateTime = New OADate(lastTimeChangeRecord.GetLatestTime)
                            Call .AddXY(markerOADateTime, 0)
                            Call .AddXY(markerOADateTime, GetYMaxValue(nativeMmolL))
                            Call .AddXY(markerOADateTime, Double.NaN)
                        End With
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
                '      Throw New Exception($"{ex.DecodeException()} exception in {memberName} at {sourceLineNumber}")
            End Try
        Next
        If s_listOfTimeChangeMarkers.Any Then
            timeChangeSeries.IsVisibleInLegend = True
            pageChart.ChartAreas(NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            pageChart.Legends(0).CustomItems.Last.Enabled = True
        Else
            timeChangeSeries.IsVisibleInLegend = False
            pageChart.Legends(0).CustomItems.Last.Enabled = False
        End If
    End Sub

    <Extension>
    Friend Sub PlotTreatmentMarkers(treatmentChart As Chart, treatmentMarkerTimeChangeSeries As Series)
        Dim lastTimeChangeRecord As TimeChangeRecord = Nothing
        s_treatmentMarkerInsulinDictionary.Clear()
        s_treatmentMarkerMealDictionary.Clear()
        For Each markerWithIndex As IndexClass(Of Dictionary(Of String, String)) In s_markers.WithIndex()
            Try
                Dim markerOADateTime As New OADate(markerWithIndex.Value.GetMarkerDateTime())
                Dim sgValue As Single
                Dim sgValueString As String = ""
                Dim entry As Dictionary(Of String, String) = markerWithIndex.Value

                If entry.TryGetValue("value", sgValueString) Then
                    sgValueString.TryParseSingle(sgValue)
                End If
                Dim markerSeriesPoints As DataPointCollection = treatmentChart.Series(MarkerSeriesName).Points
                Select Case entry("type")
                    Case "AUTO_BASAL_DELIVERY"
                        Dim amount As Single = entry(NameOf(AutoBasalDeliveryRecord.bolusAmount)).ParseSingle(3)
                        With treatmentChart.Series(BasalSeriesNameName)
                            .PlotBasalSeries(markerOADateTime,
                                             amount,
                                             MaxBasalPerDose,
                                             TreatmentInsulinRow,
                                             GetGraphLineColor("Basal Series"),
                                             True,
                                             GetToolTip(entry("type"), amount))

                        End With
                    Case "MANUAL_BASAL_DELIVERY"
                        Dim amount As Single = entry(NameOf(AutoBasalDeliveryRecord.bolusAmount)).ParseSingle(3)
                        With treatmentChart.Series(BasalSeriesNameName)
                            .PlotBasalSeries(markerOADateTime,
                                             amount,
                                             MaxBasalPerDose,
                                             TreatmentInsulinRow,
                                             GetGraphLineColor("Basal Series"),
                                             True,
                                             GetToolTip(entry("type"), amount))

                        End With
                    Case "INSULIN"
                        Select Case entry(NameOf(InsulinRecord.activationType))
                            Case "AUTOCORRECTION"
                                Dim autoCorrection As String = entry(NameOf(InsulinRecord.deliveredFastAmount))
                                With treatmentChart.Series(BasalSeriesNameName)
                                    .PlotBasalSeries(markerOADateTime,
                                                     autoCorrection.ParseSingle(3),
                                                     MaxBasalPerDose,
                                                     TreatmentInsulinRow,
                                                     GetGraphLineColor("Auto Correction"),
                                                     True,
                                                     $"Auto Correction: {autoCorrection.TruncateSingleString(3)}U")
                                End With
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If s_treatmentMarkerInsulinDictionary.TryAdd(markerOADateTime, TreatmentInsulinRow) Then
                                    markerSeriesPoints.AddXY(markerOADateTime, TreatmentInsulinRow)
                                    Dim lastDataPoint As DataPoint = markerSeriesPoints.Last
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        lastDataPoint.Color = Color.Transparent
                                        lastDataPoint.MarkerSize = 0
                                    Else
                                        lastDataPoint.Color = Color.FromArgb(30, Color.LightBlue)
                                        CreateCallout(treatmentChart,
                                                      lastDataPoint,
                                                      Color.FromArgb(10, Color.Black),
                                                      $"Bolus {entry(NameOf(InsulinRecord.deliveredFastAmount))}U")
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        Dim mealRow As Single = CSng(TreatmentInsulinRow * 0.95).RoundSingle(3, False)
                        If s_treatmentMarkerMealDictionary.TryAdd(markerOADateTime, mealRow) Then
                            markerSeriesPoints.AddXY(markerOADateTime, mealRow)
                            CreateCallout(treatmentChart,
                                          markerSeriesPoints.Last,
                                          Color.FromArgb(10, Color.Yellow),
                                          $"Meal {entry("amount")} grams")
                        End If
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "TIME_CHANGE"
                        With treatmentChart.Series(TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChangeRecord(entry)
                            markerOADateTime = New OADate(lastTimeChangeRecord.GetLatestTime)
                            .AddXY(markerOADateTime, 0)
                            .AddXY(markerOADateTime, TreatmentInsulinRow)
                            .AddXY(markerOADateTime, Double.NaN)
                        End With
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
                '      Throw New Exception($"{ex.DecodeException()} exception in {memberName} at {sourceLineNumber}")
            End Try
        Next
        treatmentChart.Annotations.Last.BringToFront()

        If s_listOfTimeChangeMarkers.Any Then
            treatmentMarkerTimeChangeSeries.IsVisibleInLegend = True
            treatmentChart.ChartAreas(NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            treatmentChart.Legends(0).CustomItems.Last.Enabled = True
        Else
            treatmentMarkerTimeChangeSeries.IsVisibleInLegend = False
            treatmentChart.Legends(0).CustomItems.Last.Enabled = False
        End If

    End Sub

End Module
