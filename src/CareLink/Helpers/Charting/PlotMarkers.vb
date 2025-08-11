' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

''' <summary>
'''  Provides extension methods and helpers for plotting marker data points and treatment markers on charts.
''' </summary>
Friend Module PlotMarkers

    ''' <summary>
    '''  Adds a sensor glucose (SG) reading point to the marker series.
    ''' </summary>
    ''' <param name="markerSeriesPoints">The collection of data points to add to.</param>
    ''' <param name="markerOADateTime">The OADate timestamp for the marker.</param>
    ''' <param name="f">The numeric SG value.</param>
    '''
    <Extension>
    Private Sub AddSgReadingPoint(markerSeriesPoints As DataPointCollection, markerOADateTime As OADate, f As Single)
        AddMarkerPoint(markerSeriesPoints, markerOADateTime, f, markerColor:=Color.DarkOrange)
        If Not Single.IsNaN(f) Then
            markerSeriesPoints.Last.Tag = $"Blood Glucose: Not used for calibration: {f} {GetBgUnits()}"
        End If
    End Sub

    ''' <summary>
    '''  Adds a calibration point to the marker series.
    ''' </summary>
    ''' <param name="markerSeriesPoints">The collection of data points to add to.</param>
    ''' <param name="markerOADateTime">The OADate timestamp for the marker.</param>
    ''' <param name="f">The numeric SG value.</param>
    ''' <param name="item">The marker entry containing calibration data.</param>
    <Extension>
    Private Sub AddCalibrationPoint(markerSeriesPoints As DataPointCollection, markerOADateTime As OADate, f As Single, item As Marker)
        AddMarkerPoint(markerSeriesPoints, markerOADateTime, f, markerColor:=Color.Red)

        Dim calibrationStatus As String = If(CBool(item.GetStringFromJson(key:="calibrationSuccess")),
                                             "accepted",
                                             "not accepted")
        Dim key As String = "unitValue"
        Dim unitValue As String = item.GetSingleFromJson(key, digits:=2, considerValue:=True).ToString
        markerSeriesPoints.Last.Tag = $"Blood Glucose: Calibration {calibrationStatus}: {unitValue} {GetBgUnits()}"
    End Sub

    ''' <summary>
    '''  Adds a generic marker point to the marker series with the specified color and formatting.
    ''' </summary>
    ''' <param name="markerSeriesPoints">The collection of data points to add to.</param>
    ''' <param name="markerOADateTime">The OADate timestamp for the marker.</param>
    ''' <param name="f">The single numeric value for the marker.</param>
    ''' <param name="markerColor">The color to use for the marker.</param>
    Private Sub AddMarkerPoint(
        markerSeriesPoints As DataPointCollection,
        markerOADateTime As OADate,
        f As Single,
        markerColor As Color)

        markerSeriesPoints.AddXY(xValue:=markerOADateTime, yValue:=f)
        markerSeriesPoints.Last.BorderColor = markerColor
        markerSeriesPoints.Last.Color = Color.FromArgb(alpha:=5, baseColor:=markerColor)
        markerSeriesPoints.Last.MarkerBorderWidth = 3
        markerSeriesPoints.Last.MarkerSize = 8
        markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Circle
    End Sub

    ''' <summary>
    '''  Adjusts the X-axis start time based on the last time change record.
    ''' </summary>
    ''' <param name="axisX">The X-axis to adjust.</param>
    ''' <param name="lastTimeChangeRecord">The last time change record to use for adjustment.</param>
    <Extension>
    Private Sub AdjustXAxisStartTime(ByRef axisX As Axis, lastTimeChangeRecord As TimeChange)
        Dim latestTime As Date = If(lastTimeChangeRecord.DisplayTime > lastTimeChangeRecord.Timestamp,
                                    lastTimeChangeRecord.DisplayTime,
                                    lastTimeChangeRecord.Timestamp)
        Dim timeOffset As Double = (latestTime - s_sgRecords(index:=0).Timestamp).TotalMinutes
        axisX.IntervalOffset = timeOffset
        axisX.IntervalOffsetType = DateTimeIntervalType.Minutes
    End Sub

    ''' <summary>
    '''  Returns a tooltip string for a given basal delivery type and amount.
    ''' </summary>
    ''' <param name="type">The type of basal delivery.</param>
    ''' <param name="amount">The amount delivered.</param>
    ''' <returns>A formatted tooltip string.</returns>
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

    ''' <summary>
    '''  Plots all markers on the specified chart, including insulin, meal, and time change markers.
    ''' </summary>
    ''' <param name="pageChart">The chart to plot markers on.</param>
    ''' <param name="timeChangeSeries">The series used for time change markers.</param>
    ''' <param name="markerInsulinDictionary">Dictionary to store insulin marker positions.</param>
    ''' <param name="markerMealDictionary">Dictionary to store meal marker positions.</param>
    ''' <param name="memberName">
    '''  Optional. The name of the calling member, automatically supplied by the compiler.
    ''' </param>
    ''' <param name="sourceLineNumber">
    '''  Optional. The line number in the source file at which the method is called, automatically supplied
    '''  by the compiler.
    ''' </param>
    <Extension>
    Friend Sub PlotMarkers(
        pageChart As Chart,
        timeChangeSeries As Series,
        markerInsulinDictionary As Dictionary(Of OADate, Single),
        markerMealDictionary As Dictionary(Of OADate, Single),
        <CallerMemberName> Optional memberName As String = Nothing,
        <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)

        Dim lastTimeChangeRecord As TimeChange = Nothing
        markerInsulinDictionary.Clear()
        markerMealDictionary?.Clear()
        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Dim item As Marker = markerWithIndex.Value
            Try
                Dim markerDateTimestamp As Date = item.GetMarkerTimestamp
                Dim markerOADatetime As New OADate(asDate:=markerDateTimestamp)
                Dim f As Single
                f = item.GetSingleFromJson(key:="unitValue")

                If Not Single.IsNaN(f) Then
                    f = Math.Min(GetYMaxValueFromNativeMmolL(), f)
                    f = Math.Max(GetYMinValueFromNativeMmolL(), f)
                End If
                Dim markerSeriesPoints As DataPointCollection = pageChart.Series(name:=MarkerSeriesName).Points
                Select Case item.Type
                    Case "BG_READING"
                        If Not Single.IsNaN(f) Then
                            markerSeriesPoints.AddSgReadingPoint(markerOADatetime, f)
                        End If
                    Case "CALIBRATION"
                        markerSeriesPoints.AddCalibrationPoint(markerOADatetime, f, item)
                    Case "AUTO_BASAL_DELIVERY"
                        Dim amount As Single = item.GetSingleFromJson(key:="bolusAmount", digits:=3)
                        With pageChart.Series(name:=BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADatetime,
                                amount,
                                bolusRow:=GetYMaxValueFromNativeMmolL(),
                                insulinRow:=GetInsulinYValue(),
                                legendText:="Basal Series",
                                DrawFromBottom:=False,
                                tag:=GetToolTip(item.Type, amount))
                        End With
                    Case "MANUAL_BASAL_DELIVERY"
                        Dim amount As Single = item.GetSingleFromJson(key:="", digits:=3)
                        With pageChart.Series(name:=BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADatetime,
                                amount,
                                bolusRow:=GetYMaxValueFromNativeMmolL(),
                                insulinRow:=GetInsulinYValue(),
                                legendText:="Basal Series",
                                DrawFromBottom:=False,
                                tag:=GetToolTip(item.Type, amount))
                        End With
                    Case "INSULIN"
                        Dim key As String
                        Select Case item.GetStringFromJson(key:=NameOf(Insulin.ActivationType))
                            Case "AUTOCORRECTION"
                                key = "deliveredFastAmount"
                                Dim autoCorrection As Single = item.GetSingleFromJson(key, digits:=3)
                                With pageChart.Series(name:=BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADatetime,
                                        amount:=autoCorrection,
                                        bolusRow:=GetYMaxValueFromNativeMmolL(),
                                        insulinRow:=GetInsulinYValue(),
                                        legendText:="Auto Correction",
                                        DrawFromBottom:=False,
                                        tag:=$"Auto Correction: {autoCorrection}U")
                                End With
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If markerInsulinDictionary.TryAdd(key:=markerOADatetime, value:=CInt(GetInsulinYValue())) Then
                                    Dim yValue As Double = GetInsulinYValue() - If(NativeMmolL, 0.555, 10)
                                    markerSeriesPoints.AddXY(xValue:=markerOADatetime, yValue)
                                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                                    markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(alpha:=10, baseColor:=Color.Black)
                                    markerSeriesPoints.Last.MarkerSize = 20
                                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        markerSeriesPoints.Last.Color = Color.Transparent
                                        markerSeriesPoints.Last.MarkerSize = 0
                                    Else
                                        markerSeriesPoints.Last.Color = Color.FromArgb(alpha:=30, baseColor:=Color.LightBlue)
                                        key = "deliveredFastAmount"
                                        Dim autoCorrection As Single = item.GetSingleFromJson(key, digits:=3)
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
                        If markerMealDictionary.TryAdd(key:=markerOADatetime, value:=GetYMinValueFromNativeMmolL()) Then
                            Dim height As Double =
                                If(NativeMmolL,
                                   s_mealImage.Height / 2 / MmolLUnitsDivisor,
                                   s_mealImage.Height / 2)
                            markerSeriesPoints.AddXY(xValue:=markerOADatetime, yValue:=GetYMinValueFromNativeMmolL() + height)
                            markerSeriesPoints.Last.Color = Color.FromArgb(alpha:=10, baseColor:=Color.Yellow)
                            markerSeriesPoints.Last.MarkerBorderWidth = 2
                            markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(alpha:=10, baseColor:=Color.Yellow)
                            markerSeriesPoints.Last.MarkerSize = 20
                            markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                            Dim amount As Integer = CInt(item.GetSingleFromJson(key:="amount", digits:=0))
                            markerSeriesPoints.Last.Tag = $"Meal:{amount} grams"
                        End If
                    Case "TIME_CHANGE"
                        With pageChart.Series(name:=TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChange(item, recordNumber:=1)
                            markerOADatetime = New OADate(asDate:=lastTimeChangeRecord.Timestamp)
                            .AddXY(xValue:=markerOADatetime, yValue:=0)
                            .AddXY(xValue:=markerOADatetime, yValue:=GetYMaxValueFromNativeMmolL())
                            .AddXY(xValue:=markerOADatetime, yValue:=Double.NaN)
                        End With
                    Case "LOW_GLUCOSE_SUSPENDED"
                        If PatientData.ConduitSensorInRange AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
                            Dim timeOrderedMarkers As SortedDictionary(Of OADate, Single) = GetManualBasalValues(markerWithIndex)
                            For Each kvp As KeyValuePair(Of OADate, Single) In timeOrderedMarkers
                                With pageChart.Series(name:=BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADateTime:=kvp.Key,
                                        amount:=kvp.Value,
                                        bolusRow:=GetYMaxValueFromNativeMmolL(),
                                        insulinRow:=GetInsulinYValue(),
                                        legendText:="Basal Series",
                                        DrawFromBottom:=False,
                                        tag:=$"Manual Basal: {kvp.Value.ToString.TruncateSingle(digits:=3)}U")
                                End With
                            Next

                        End If
                    Case Else
                        Stop
                End Select
            Catch innerException As Exception
                Stop
                Throw New ApplicationException(
                    message:=$"{innerException.DecodeException()} exception in {memberName} at {sourceLineNumber}",
                    innerException)
            End Try
        Next
        If s_timeChangeMarkers.Count > 0 Then
            timeChangeSeries.IsVisibleInLegend = True
            pageChart.ChartAreas(name:=NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            pageChart.Legends(index:=0).CustomItems.Last.Enabled = True
        Else
            timeChangeSeries.IsVisibleInLegend = False
            pageChart.Legends(index:=0).CustomItems.Last.Enabled = False
        End If
    End Sub

    ''' <summary>
    '''  Plots treatment markers on the specified treatment chart, including insulin and meal markers.
    ''' </summary>
    ''' <param name="treatmentChart">The chart to plot treatment markers on.</param>
    ''' <param name="treatmentMarkerTimeChangeSeries">The series used for time change markers in the treatment chart.</param>
    ''' <param name="memberName">
    '''  Optional. The name of the calling member, automatically supplied by the compiler.
    ''' </param>
    ''' <param name="sourceLineNumber">
    '''  Optional. The line number in the source file at which the method is called, automatically supplied
    '''  by the compiler.
    ''' </param>
    <Extension>
    Friend Sub PlotTreatmentMarkers(
        treatmentChart As Chart,
        treatmentMarkerTimeChangeSeries As Series,
        <CallerMemberName> Optional memberName As String = Nothing,
        <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)

        Dim lastTimeChangeRecord As TimeChange = Nothing
        s_treatmentMarkerInsulinDictionary.Clear()
        s_treatmentMarkerMealDictionary.Clear()
        Dim key As String
        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Try
                Dim item As Marker = markerWithIndex.Value
                Dim markerDateTime As Date = item.GetMarkerTimestamp
                Dim markerOADate As New OADate(asDate:=markerDateTime)
                Dim markerOADateTime As New OADate(asDate:=item.GetMarkerTimestamp)
                Dim markerSeriesPoints As DataPointCollection = treatmentChart.Series(name:=MarkerSeriesName).Points
                Select Case item.Type
                    Case "AUTO_BASAL_DELIVERY"
                        key = NameOf(AutoBasalDelivery.BolusAmount)
                        Dim amount As Single = item.GetSingleFromJson(key, digits:=3)
                        With treatmentChart.Series(name:=BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADateTime,
                                amount,
                                bolusRow:=MaxBasalPerDose,
                                insulinRow:=TreatmentInsulinRow,
                                legendText:="Basal Series",
                                DrawFromBottom:=True,
                                tag:=GetToolTip(item.Type, amount))

                        End With
                    Case "MANUAL_BASAL_DELIVERY"
                        key = NameOf(AutoBasalDelivery.BolusAmount)
                        Dim amount As Single = item.GetSingleFromJson(key, digits:=3)
                        With treatmentChart.Series(name:=BasalSeriesName)
                            .PlotBasalSeries(
                                markerOADateTime,
                                amount,
                                bolusRow:=MaxBasalPerDose,
                                insulinRow:=TreatmentInsulinRow,
                                legendText:="Basal Series",
                                DrawFromBottom:=True,
                                tag:=GetToolTip(item.Type, amount))

                        End With
                    Case "INSULIN"
                        key = NameOf(Insulin.ActivationType)
                        Select Case item.GetStringFromJson(key)
                            Case "AUTOCORRECTION"
                                key = NameOf(Insulin.DeliveredFastAmount)
                                Dim amount As Single = item.GetSingleFromJson(key, digits:=3)
                                With treatmentChart.Series(name:=BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADateTime,
                                        amount,
                                        bolusRow:=MaxBasalPerDose,
                                        insulinRow:=TreatmentInsulinRow,
                                        legendText:="Auto Correction",
                                        DrawFromBottom:=True,
                                        tag:=$"Auto Correction: {amount}U")
                                End With
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If s_treatmentMarkerInsulinDictionary.TryAdd(key:=markerOADateTime, value:=TreatmentInsulinRow) Then
                                    markerSeriesPoints.AddXY(xValue:=markerOADateTime, yValue:=TreatmentInsulinRow)
                                    Dim lastDataPoint As DataPoint = markerSeriesPoints.Last
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        lastDataPoint.Color = Color.Transparent
                                        lastDataPoint.MarkerSize = 0
                                    Else
                                        lastDataPoint.Color = Color.FromArgb(alpha:=30, baseColor:=Color.LightBlue)
                                        key = NameOf(Insulin.DeliveredFastAmount)
                                        CreateCallout(
                                            treatmentChart,
                                            lastDataPoint,
                                            markerBorderColor:=Color.FromArgb(alpha:=10, baseColor:=Color.Black),
                                            tagText:=$"Bolus {item.GetSingleFromJson(key, digits:=3)}U")
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        Dim value As Single = CSng(TreatmentInsulinRow * 0.95).RoundToSingle(digits:=3)
                        If s_treatmentMarkerMealDictionary.TryAdd(key:=markerOADateTime, value) Then
                            markerSeriesPoints.AddXY(xValue:=markerOADateTime, yValue:=value)
                            CreateCallout(
                                treatmentChart,
                                lastDataPoint:=markerSeriesPoints.Last,
                                markerBorderColor:=Color.FromArgb(alpha:=10, baseColor:=Color.Yellow),
                                tagText:=$"Meal {item.GetSingleFromJson(key:="amount", digits:=0)} grams")
                        End If
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "TIME_CHANGE"
                        With treatmentChart.Series(name:=TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChange(item, recordNumber:=1)
                            markerOADateTime = New OADate(asDate:=lastTimeChangeRecord.Timestamp)
                            .AddXY(xValue:=markerOADateTime, yValue:=0)
                            .AddXY(xValue:=markerOADateTime, yValue:=TreatmentInsulinRow)
                            .AddXY(xValue:=markerOADateTime, yValue:=Double.NaN)
                        End With
                    Case "LOW_GLUCOSE_SUSPENDED"
                        If PatientData.ConduitSensorInRange AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
                            Dim timeOrderedMarkers As SortedDictionary(Of OADate, Single) = GetManualBasalValues(markerWithIndex)
                            For Each kvp As KeyValuePair(Of OADate, Single) In timeOrderedMarkers
                                With treatmentChart.Series(name:=BasalSeriesName)
                                    .PlotBasalSeries(
                                        markerOADateTime:=kvp.Key,
                                        amount:=kvp.Value,
                                        bolusRow:=MaxBasalPerDose,
                                        insulinRow:=TreatmentInsulinRow,
                                        legendText:=BasalSeriesName,
                                        DrawFromBottom:=True,
                                        tag:=$"Manual Basal: {kvp.Value.ToString.TruncateSingle(digits:=3)}U")
                                End With
                            Next
                        End If
                    Case Else
                        Stop
                End Select
            Catch innerException As Exception
                Stop
                Dim decodedException As String = innerException.DecodeException()
                Dim message As String = $"{decodedException} exception in {NameOf(PlotTreatmentMarkers)} at {memberName} line {sourceLineNumber}"
                Throw New ApplicationException(message, innerException)
            End Try
        Next
        treatmentChart.Annotations.Last.BringToFront()

        If s_timeChangeMarkers.Count <> 0 Then
            treatmentMarkerTimeChangeSeries.IsVisibleInLegend = True
            treatmentChart.ChartAreas(name:=NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            treatmentChart.Legends(index:=0).CustomItems.Last.Enabled = True
        Else
            treatmentMarkerTimeChangeSeries.IsVisibleInLegend = False
            treatmentChart.Legends(index:=0).CustomItems.Last.Enabled = False
        End If
    End Sub

End Module
