' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotMarkers

    <Extension>
    Private Sub AddSgReadingPoint(markerSeriesPoints As DataPointCollection, markerOADateTime As OADate, sgValueString As String, sgValue As Single)
        AddMarkerPoint(markerSeriesPoints, markerOADateTime, sgValue, Color.DarkOrange)
        If Not Single.IsNaN(sgValue) Then
            markerSeriesPoints.Last.Tag = $"Blood Glucose: Not used for calibration: {sgValueString} {BgUnitsNativeString}"
        End If
    End Sub

    <Extension>
    Private Sub AddCalibrationPoint(markerSeriesPoints As DataPointCollection, markerOADateTime As OADate, sgValue As Single, entry As Marker)
        AddMarkerPoint(markerSeriesPoints, markerOADateTime, sgValue, Color.Red)
        markerSeriesPoints.Last.Tag = $"Blood Glucose: Calibration {If(CBool(entry.GetStringValueFromJson("calibrationSuccess")), "accepted", "not accepted")}: {entry.GetSingleValueFromJson("unitValue")} {BgUnitsNativeString}"
    End Sub

    Private Sub AddMarkerPoint(markerSeriesPoints As DataPointCollection, markerOADateTime As OADate, sgValue As Single, markerColor As Color)
        markerSeriesPoints.AddXY(markerOADateTime, sgValue)
        markerSeriesPoints.Last.BorderColor = markerColor
        markerSeriesPoints.Last.Color = Color.FromArgb(5, markerColor)
        markerSeriesPoints.Last.MarkerBorderWidth = 3
        markerSeriesPoints.Last.MarkerSize = 8
        markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Circle
    End Sub

    <Extension>
    Private Sub AdjustXAxisStartTime(ByRef axisX As Axis, lastTimeChangeRecord As TimeChange)
        Dim latestTime As Date = If(lastTimeChangeRecord.DisplayTime > lastTimeChangeRecord.Timestamp, lastTimeChangeRecord.DisplayTime, lastTimeChangeRecord.Timestamp)
        Dim timeOffset As Double = (latestTime - s_listOfSgRecords(0).Timestamp).TotalMinutes
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
    Friend Sub PlotMarkers(
        pageChart As Chart,
        timeChangeSeries As Series,
        markerInsulinDictionary As Dictionary(Of OADate, Single),
        markerMealDictionary As Dictionary(Of OADate, Single))

        Dim lastTimeChangeRecord As TimeChange = Nothing
        markerInsulinDictionary.Clear()
        markerMealDictionary?.Clear()
        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Dim markerEntry As Marker = markerWithIndex.Value
            Try
                Dim markerDateTimestamp As Date = markerEntry.GetMarkerTimestamp
                Dim markerOADatetime As New OADate(markerDateTimestamp)
                Dim sgValue As Single
                sgValue = markerEntry.GetSingleValueFromJson("unitValue")

                If Not Single.IsNaN(sgValue) Then
                    sgValue = Math.Min(GetYMaxValue(), sgValue)
                    sgValue = Math.Max(GetYMinValue(), sgValue)
                End If
                Dim markerSeriesPoints As DataPointCollection = pageChart.Series(MarkerSeriesName).Points
                Select Case markerEntry.Type
                    Case "BG_READING"
                        If Not Single.IsNaN(sgValue) Then
                            markerSeriesPoints.AddSgReadingPoint(markerOADatetime, sgValue.ToString, sgValue)
                        End If
                    Case "CALIBRATION"
                        markerSeriesPoints.AddCalibrationPoint(markerOADatetime, sgValue, markerEntry)
                    Case "AUTO_BASAL_DELIVERY"
                        Dim amount As Single = markerEntry.GetSingleValueFromJson("bolusAmount", 3)
                        With pageChart.Series(BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADatetime,
                                amount,
                                bolusRow:=GetYMaxValue(),
                                insulinRow:=GetInsulinYValue(),
                                legendText:="Basal Series",
                                DrawFromBottom:=False,
                                tag:=GetToolTip(markerEntry.Type, amount))
                        End With
                    Case "MANUAL_BASAL_DELIVERY"
                        Dim amount As Single = markerEntry.GetSingleValueFromJson("", 3)
                        With pageChart.Series(BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADatetime,
                                amount,
                                bolusRow:=GetYMaxValue(),
                                insulinRow:=GetInsulinYValue(),
                                legendText:="Basal Series",
                                DrawFromBottom:=False,
                                tag:=GetToolTip(markerEntry.Type, amount))
                        End With
                    Case "INSULIN"
                        Select Case markerEntry.GetStringValueFromJson(NameOf(Insulin.ActivationType))
                            Case "AUTOCORRECTION"
                                Dim autoCorrection As Single = markerEntry.GetSingleValueFromJson("deliveredFastAmount", 3)
                                With pageChart.Series(BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADatetime,
                                        amount:=autoCorrection,
                                        bolusRow:=GetYMaxValue(),
                                        insulinRow:=GetInsulinYValue(),
                                        legendText:="Auto Correction",
                                        DrawFromBottom:=False,
                                        tag:=$"Auto Correction: {autoCorrection}U")
                                End With
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If markerInsulinDictionary.TryAdd(markerOADatetime, CInt(GetInsulinYValue())) Then
                                    markerSeriesPoints.AddXY(markerOADatetime, GetInsulinYValue() - If(NativeMmolL, 0.555, 10))
                                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                                    markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Black)
                                    markerSeriesPoints.Last.MarkerSize = 20
                                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        markerSeriesPoints.Last.Color = Color.Transparent
                                        markerSeriesPoints.Last.MarkerSize = 0
                                    Else
                                        markerSeriesPoints.Last.Color = Color.FromArgb(30, Color.LightBlue)
                                        Dim autoCorrection As Single = markerEntry.GetSingleValueFromJson("deliveredFastAmount", 3)
                                        markerSeriesPoints.Last.Tag = $"Bolus: {autoCorrection}U"
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        If markerMealDictionary Is Nothing Then Continue For
                        If markerMealDictionary.TryAdd(markerOADatetime, GetYMinValue()) Then
                            markerSeriesPoints.AddXY(markerOADatetime, GetYMinValue() + If(NativeMmolL, s_mealImage.Height / 2 / MmolLUnitsDivisor, s_mealImage.Height / 2))
                            markerSeriesPoints.Last.Color = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerBorderWidth = 2
                            markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(10, Color.Yellow)
                            markerSeriesPoints.Last.MarkerSize = 20
                            markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                            markerSeriesPoints.Last.Tag = $"Meal:{markerEntry.GetSingleValueFromJson("amount", 0)} grams"
                        End If
                    Case "TIME_CHANGE"
                        With pageChart.Series(TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChange(markerEntry, recordNumber:=1)
                            markerOADatetime = New OADate(lastTimeChangeRecord.Timestamp)
                            .AddXY(markerOADatetime, 0)
                            .AddXY(markerOADatetime, GetYMaxValue())
                            .AddXY(markerOADatetime, Double.NaN)
                        End With
                    Case "LOW_GLUCOSE_SUSPENDED"
                        If PatientData.ConduitSensorInRange AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
                            Dim timeOrderedMarkers As SortedDictionary(Of OADate, Single) = GetManualBasalValues(markerWithIndex)
                            For Each kvp As KeyValuePair(Of OADate, Single) In timeOrderedMarkers
                                With pageChart.Series(BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADateTime:=kvp.Key,
                                        amount:=kvp.Value,
                                        bolusRow:=GetYMaxValue(),
                                        insulinRow:=GetInsulinYValue(),
                                        legendText:="Basal Series",
                                        DrawFromBottom:=False,
                                        tag:=$"Manual Basal: {kvp.Value.ToString.TruncateSingleString(3)}U")
                                End With
                            Next

                        End If
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
                '      Throw New Exception($"{ex.DecodeException()} exception in {memberName} at {sourceLineNumber}")
            End Try
        Next
        If s_listOfTimeChangeMarkers.Count > 0 Then
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
        Dim lastTimeChangeRecord As TimeChange = Nothing
        s_treatmentMarkerInsulinDictionary.Clear()
        s_treatmentMarkerMealDictionary.Clear()
        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Try
                Dim markerEntry As Marker = markerWithIndex.Value
                Dim markerDateTime As Date = markerEntry.GetMarkerTimestamp
                Dim markerOADate As New OADate(markerDateTime)
                Dim markerOADateTimestamp As New OADate(markerEntry.GetMarkerTimestamp)
                Dim markerSeriesPoints As DataPointCollection = treatmentChart.Series(MarkerSeriesName).Points
                Select Case markerEntry.Type
                    Case "AUTO_BASAL_DELIVERY"
                        Dim amount As Single = markerEntry.GetSingleValueFromJson(NameOf(AutoBasalDelivery.bolusAmount), 3)
                        With treatmentChart.Series(BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADateTime:=markerOADateTimestamp,
                                amount,
                                bolusRow:=MaxBasalPerDose,
                                insulinRow:=TreatmentInsulinRow,
                                legendText:="Basal Series",
                                DrawFromBottom:=True,
                                tag:=GetToolTip(markerEntry.Type, amount))

                        End With
                    Case "MANUAL_BASAL_DELIVERY"
                        Dim amount As Single = markerEntry.GetSingleValueFromJson(NameOf(AutoBasalDelivery.bolusAmount), decimalDigits:=3)
                        With treatmentChart.Series(BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADateTime:=markerOADateTimestamp,
                                amount,
                                bolusRow:=MaxBasalPerDose,
                                insulinRow:=TreatmentInsulinRow,
                                legendText:="Basal Series",
                                DrawFromBottom:=True,
                                tag:=GetToolTip(markerEntry.Type, amount))

                        End With
                    Case "INSULIN"
                        Select Case markerEntry.GetStringValueFromJson(NameOf(Insulin.ActivationType))
                            Case "AUTOCORRECTION"
                                Dim amount As Single = markerEntry.GetSingleValueFromJson(NameOf(Insulin.DeliveredFastAmount), decimalDigits:=3)
                                With treatmentChart.Series(BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADateTimestamp,
                                        amount,
                                        bolusRow:=MaxBasalPerDose,
                                        insulinRow:=TreatmentInsulinRow,
                                        legendText:="Auto Correction",
                                        DrawFromBottom:=True,
                                        tag:=$"Auto Correction: {amount}U")
                                End With
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If s_treatmentMarkerInsulinDictionary.TryAdd(markerOADateTimestamp, TreatmentInsulinRow) Then
                                    markerSeriesPoints.AddXY(markerOADateTimestamp, TreatmentInsulinRow)
                                    Dim lastDataPoint As DataPoint = markerSeriesPoints.Last
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        lastDataPoint.Color = Color.Transparent
                                        lastDataPoint.MarkerSize = 0
                                    Else
                                        lastDataPoint.Color = Color.FromArgb(30, Color.LightBlue)
                                        CreateCallout(
                                            treatmentChart,
                                            lastDataPoint,
                                            markerBorderColor:=Color.FromArgb(10, Color.Black),
                                            tagText:=$"Bolus {markerEntry.GetSingleValueFromJson(NameOf(Insulin.DeliveredFastAmount), 3)}U")
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        Dim value As Single = CSng(TreatmentInsulinRow * 0.95).RoundSingle(3, False)
                        If s_treatmentMarkerMealDictionary.TryAdd(key:=markerOADateTimestamp, value) Then
                            markerSeriesPoints.AddXY(xValue:=markerOADateTimestamp, yValue:=value)
                            CreateCallout(
                                treatmentChart,
                                lastDataPoint:=markerSeriesPoints.Last,
                                markerBorderColor:=Color.FromArgb(10, Color.Yellow),
                                tagText:=$"Meal {markerEntry.GetSingleValueFromJson("amount", 0)} grams")
                        End If
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "TIME_CHANGE"
                        With treatmentChart.Series(TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChange(markerEntry, recordNumber:=1)
                            markerOADateTimestamp = New OADate(lastTimeChangeRecord.Timestamp)
                            .AddXY(xValue:=markerOADateTimestamp, yValue:=0)
                            .AddXY(xValue:=markerOADateTimestamp, yValue:=TreatmentInsulinRow)
                            .AddXY(xValue:=markerOADateTimestamp, yValue:=Double.NaN)
                        End With
                    Case "LOW_GLUCOSE_SUSPENDED"
                        If PatientData.ConduitSensorInRange AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
                            Dim timeOrderedMarkers As SortedDictionary(Of OADate, Single) = GetManualBasalValues(markerWithIndex)
                            For Each kvp As KeyValuePair(Of OADate, Single) In timeOrderedMarkers
                                With treatmentChart.Series(BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADateTime:=kvp.Key,
                                        amount:=kvp.Value,
                                        bolusRow:=MaxBasalPerDose,
                                        insulinRow:=TreatmentInsulinRow,
                                        legendText:=BasalSeriesName,
                                        DrawFromBottom:=True,
                                        tag:=$"Manual Basal: {kvp.Value.ToString.TruncateSingleString(3)}U")
                                End With
                            Next
                        End If
                    Case Else
                        Stop
                End Select
            Catch ex As Exception
                Stop
            End Try
        Next
        treatmentChart.Annotations.Last.BringToFront()

        If s_listOfTimeChangeMarkers.Count <> 0 Then
            treatmentMarkerTimeChangeSeries.IsVisibleInLegend = True
            treatmentChart.ChartAreas(NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            treatmentChart.Legends(0).CustomItems.Last.Enabled = True
        Else
            treatmentMarkerTimeChangeSeries.IsVisibleInLegend = False
            treatmentChart.Legends(0).CustomItems.Last.Enabled = False
        End If
    End Sub

End Module
