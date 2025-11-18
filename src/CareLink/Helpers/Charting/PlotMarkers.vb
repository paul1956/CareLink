' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

''' <summary>
'''  Provides extension methods and helpers for plotting marker data points
'''  and treatment markers on charts.
''' </summary>
Friend Module PlotMarkers

    ''' <summary>
    '''  Adds a calibration point to the marker series.
    ''' </summary>
    ''' <param name="seriesPts">The collection of data points to add to.</param>
    ''' <param name="markerOA">The OADate timestamp for the marker.</param>
    ''' <param name="f">The numeric SG value.</param>
    ''' <param name="item">The marker entry containing calibration data.</param>
    <Extension>
    Private Sub AddCalibrationPt(seriesPts As DataPointCollection, markerOA As OADate, f As Single, item As Marker)
        seriesPts.AddMarkerPt(markerOA, f, markerColor:=Color.Red)
        Dim status As Boolean = CBool(item.GetStringFromJson(key:="calibrationSuccess"))
        Dim calibrationStatus As String = If(status, "accepted", "not accepted")
        Dim key As String = "unitValue"
        Dim unitValue As String = item.GetSingleFromJson(key, digits:=2, considerValue:=True).ToString
        seriesPts.Last.Tag = $"Blood Glucose: Calibration {calibrationStatus}: {unitValue} {BgUnits}"
    End Sub

    ''' <summary>
    '''  Adds a generic marker point to the marker series with the specified color
    '''  and formatting.
    ''' </summary>
    ''' <param name="seriesPts">The collection of data points to add to.</param>
    ''' <param name="markerOA">The OADate timestamp for the marker.</param>
    ''' <param name="f">The single numeric value for the marker.</param>
    ''' <param name="markerColor">The color to use for the marker.</param>
    <Extension>
    Private Sub AddMarkerPt(seriesPts As DataPointCollection, markerOA As OADate, f As Single, markerColor As Color)
        seriesPts.AddXY(xValue:=markerOA, yValue:=f)
        seriesPts.Last.BorderColor = markerColor
        seriesPts.Last.Color = Color.FromArgb(alpha:=5, baseColor:=markerColor)
        seriesPts.Last.MarkerBorderWidth = 3
        seriesPts.Last.MarkerSize = 8
        seriesPts.Last.MarkerStyle = MarkerStyle.Circle
    End Sub

    ''' <summary>
    '''  Adds a sensor glucose (SG) reading point to the marker series.
    ''' </summary>
    ''' <param name="seriesPts">The collection of data points to add to.</param>
    ''' <param name="markerOA">The OADate timestamp for the marker.</param>
    ''' <param name="f">The numeric SG value.</param>
    '''
    <Extension>
    Private Sub AddSgReadingPoint(seriesPts As DataPointCollection, markerOA As OADate, f As Single)
        seriesPts.AddMarkerPt(markerOA, f, markerColor:=Color.DarkOrange)
        If Not Single.IsNaN(f) Then
            seriesPts.Last.Tag = $"Blood Glucose: Not used for calibration: {f} {BgUnits}"
        End If
    End Sub

    ''' <summary>
    '''  Adjusts the X-axis start time based on the last time change record.
    ''' </summary>
    ''' <param name="axisX">The X-axis to adjust.</param>
    ''' <param name="lastTimeChangeRecord">
    '''  The last time change record to use for adjustment.
    ''' </param>
    <Extension>
    Private Sub AdjustXAxisStartTime(ByRef axisX As Axis, lastTimeChangeRecord As TimeChange)
        Dim latestTime As Date = If(lastTimeChangeRecord.DisplayTime > lastTimeChangeRecord.Timestamp,
                                    lastTimeChangeRecord.DisplayTime,
                                    lastTimeChangeRecord.Timestamp)

        axisX.IntervalOffset = (latestTime - s_sgRecords(index:=0).Timestamp).TotalMinutes
        axisX.IntervalOffsetType = DateTimeIntervalType.Minutes
    End Sub

    ''' <summary>
    '''  Gets the Y value for insulin plotting based on the current blood glucose (SG) records and the unit system.
    ''' </summary>
    ''' <returns>
    '''  The Y value for insulin plotting suitable for display scale, adjusted to mmol/L or mg/dL units.
    ''' </returns>
    Private Function GetInsulinYValue() As Single
        ' Selector function to extract blood glucose value (sg) from SG records
        Dim getSgValue As Func(Of SG, Single) = Function(sgRecord As SG) sgRecord.sg

        ' Calculate max blood glucose value from the records, offset by 2 for padding
        Dim maxYscaled As Single = s_sgRecords.Max(getSgValue) + 2

        ' Handle case where maxYscaled is NaN (no valid records)
        If maxYscaled.IsSgInvalid Then
            ' Return default plotting Y value based on unit system
            Return If(NativeMmolL,
                      330 / MmolLUnitsDivisor,
                      330)
        End If

        Dim noRecords As Boolean = s_sgRecords.Count = 0
        If NativeMmolL Then
            ' Calculate Y value for mmol/L scale with fallback defaults and minimum thresholds
            Return If(noRecords OrElse maxYscaled > 330 / MmolLUnitsDivisor,
                      342 / MmolLUnitsDivisor, ' Upper default if no records or max is larger than scale max
                      Math.Max(maxYscaled, 260 / MmolLUnitsDivisor))
        Else
            ' Calculate Y value for mg/dL scale with fallback defaults and minimum thresholds
            Return If(noRecords OrElse maxYscaled > 330,
                      342, ' Upper default for no records or large values
                      Math.Max(maxYscaled, 260)) ' Use max with minimum threshold
        End If
    End Function

    ''' <summary>
    '''  Returns a tooltip string for a given basal delivery type and amount.
    ''' </summary>
    ''' <param name="type">The type of basal delivery.</param>
    ''' <param name="amount">The amount delivered.</param>
    ''' <returns>A formatted tooltip string.</returns>
    Private Function GetToolTip(type As String, amount As Single) As String
        Dim minBasalMsg As String = EmptyString
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
    '''  Plots all markers on the specified chart, including insulin, meal,
    '''  and time change markers.
    ''' </summary>
    ''' <param name="pageChart">The chart to plot markers on.</param>
    ''' <param name="timeChangeSeries">The series used for time change markers.</param>
    ''' <param name="markerInsulinDictionary">
    '''  Dictionary to store insulin marker positions.
    ''' </param>
    ''' <param name="markerMealDictionary">Dictionary to store meal marker positions.</param>
    ''' <param name="memberName">
    '''  Optional. The name of the calling member, automatically supplied by the compiler.
    ''' </param>
    ''' <param name="sourceLineNumber">
    '''  Optional. The line number in the source file at which the method is called,
    '''  automatically supplied by the compiler.
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
        Dim bolusRow As Single = GetYMaxNativeMmolL()
        Dim insulinRow As Single = GetInsulinYValue()
        Dim yMinNativeMmolL As Single = GetYMinNativeMmolL()
        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Dim item As Marker = markerWithIndex.Value
            Try
                Dim markerDateTimestamp As Date = item.GetMarkerTimestamp
                Dim markerOA As New OADate(asDate:=markerDateTimestamp)
                Dim f As Single = item.GetSingleFromJson(key:="unitValue")

                If Not Single.IsNaN(f) Then
                    f = Math.Min(val1:=bolusRow, val2:=f)
                    f = Math.Max(val1:=yMinNativeMmolL, val2:=f)
                End If
                Dim markerSeriesPoints As DataPointCollection = pageChart.Series(name:=MarkerSeriesName).Points
                Select Case item.Type
                    Case "BG_READING"
                        If Not Single.IsNaN(f) Then
                            markerSeriesPoints.AddSgReadingPoint(markerOA, f)
                        End If
                    Case "CALIBRATION"
                        markerSeriesPoints.AddCalibrationPt(markerOA, f, item)
                    Case "AUTO_BASAL_DELIVERY"
                        Dim amount As Single = item.GetSingleFromJson(key:="bolusAmount", digits:=3)
                        pageChart.Series(name:=BasalSeriesName).PlotBasalSeries(
                            markerOA,
                            amount,
                            bolusRow,
                            insulinRow,
                            legendText:="Basal Series",
                            DrawFromBottom:=False,
                            tag:=GetToolTip(item.Type, amount))
                    Case "MANUAL_BASAL_DELIVERY"
                        Dim amount As Single = item.GetSingleFromJson(key:=EmptyString, digits:=3)
                        pageChart.Series(name:=BasalSeriesName).PlotBasalSeries(
                            markerOA,
                            amount,
                            bolusRow,
                            insulinRow,
                            legendText:="Basal Series",
                            DrawFromBottom:=False,
                            tag:=GetToolTip(item.Type, amount))
                    Case "INSULIN"
                        Dim key As String
                        Select Case item.GetStringFromJson(key:=NameOf(Insulin.ActivationType))
                            Case "AUTOCORRECTION"
                                key = "deliveredFastAmount"
                                pageChart.Series(name:=BasalSeriesName).PlotBasalSeries(
                                    markerOA,
                                    amount:=item.GetSingleFromJson(key, digits:=3),
                                    bolusRow,
                                    insulinRow,
                                    legendText:="Auto Correction",
                                    DrawFromBottom:=False,
                                    tag:=$"Auto Correction: {item.GetSingleFromJson(key, digits:=3)}U")
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                Dim baseColor As Color
                                If markerInsulinDictionary.TryAdd(key:=markerOA, value:=CInt(GetInsulinYValue())) Then
                                    Dim yValue As Double = GetInsulinYValue() - If(NativeMmolL, 0.555, 10)
                                    markerSeriesPoints.AddXY(xValue:=markerOA, yValue)
                                    markerSeriesPoints.Last.MarkerBorderWidth = 2
                                    baseColor = Color.Black
                                    markerSeriesPoints.Last.MarkerBorderColor = Color.FromArgb(alpha:=10, baseColor)
                                    markerSeriesPoints.Last.MarkerSize = 20
                                    markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                                    If Single.IsNaN(GetInsulinYValue()) Then
                                        markerSeriesPoints.Last.Color = Color.Transparent
                                        markerSeriesPoints.Last.MarkerSize = 0
                                    Else
                                        baseColor = Color.LightBlue
                                        markerSeriesPoints.Last.Color = Color.FromArgb(alpha:=30, baseColor)
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
                        If markerMealDictionary.TryAdd(key:=markerOA, value:=yMinNativeMmolL) Then
                            Dim height As Double = If(NativeMmolL,
                                                      s_mealImage.Height / 2 / MmolLUnitsDivisor,
                                                      s_mealImage.Height / 2)
                            markerSeriesPoints.AddXY(xValue:=markerOA, yValue:=yMinNativeMmolL + height)
                            Dim markerColor As Color = Color.FromArgb(alpha:=10, baseColor:=Color.Yellow)
                            markerSeriesPoints.Last.Color = markerColor
                            markerSeriesPoints.Last.MarkerBorderWidth = 2
                            markerSeriesPoints.Last.MarkerBorderColor = markerColor
                            markerSeriesPoints.Last.MarkerSize = 20
                            markerSeriesPoints.Last.MarkerStyle = MarkerStyle.Square
                            Dim amount As Integer = CInt(item.GetSingleFromJson(key:="amount", digits:=0))
                            markerSeriesPoints.Last.Tag = $"Meal:{amount} {GetCarbDefaultUnit()}"
                        End If
                    Case "TIME_CHANGE"
                        With pageChart.Series(name:=TimeChangeSeriesName).Points
                            lastTimeChangeRecord = New TimeChange(item, recordNumber:=1)
                            markerOA = New OADate(asDate:=lastTimeChangeRecord.Timestamp)
                            .AddXY(xValue:=markerOA, yValue:=0)
                            .AddXY(xValue:=markerOA, yValue:=bolusRow)
                            .AddXY(xValue:=markerOA, yValue:=Double.NaN)
                        End With
                    Case "LOW_GLUCOSE_SUSPENDED"
                        Dim timeOrderedMarkers As SortedDictionary(Of OADate, Single)
                        If PatientData.ConduitSensorInRange AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
                            timeOrderedMarkers = GetManualBasalValues(markerWithIndex)
                            For Each kvp As KeyValuePair(Of OADate, Single) In timeOrderedMarkers
                                pageChart.Series(name:=BasalSeriesName).PlotBasalSeries(
                                    markerOADateTime:=kvp.Key,
                                    amount:=kvp.Value,
                                    bolusRow,
                                    insulinRow,
                                    legendText:="Basal Series",
                                    DrawFromBottom:=False,
                                    tag:=$"Manual Basal: {kvp.Value.ToString.TruncateSingle(digits:=3)}U")
                            Next
                        End If
                    Case Else
                        Stop
                End Select
            Catch innerException As Exception
                Stop
                Dim str As String = innerException.DecodeException()
                Dim message As String = $"{str} exception in {memberName} at {sourceLineNumber}"
                Throw New ApplicationException(message, innerException)
            End Try
        Next
        If s_timeChangeMarkers.Count > 0 Then
            timeChangeSeries.IsVisibleInLegend = True
            Const name As String = NameOf(ChartArea)
            pageChart.ChartAreas(name).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            pageChart.Legends(index:=0).CustomItems.Last.Enabled = True
        Else
            timeChangeSeries.IsVisibleInLegend = False
            pageChart.Legends(index:=0).CustomItems.Last.Enabled = False
        End If
    End Sub

    ''' <summary>
    '''  Plots treatment markers on the specified treatment chart,
    '''  including insulin and meal markers.
    ''' </summary>
    ''' <param name="chart">The chart to plot treatment markers on.</param>
    ''' <param name="treatmentTimeChangeSeries">
    '''  The series used for time change markers in the treatment chart.
    ''' </param>
    ''' <param name="memberName">
    '''  Optional. The name of the calling member, automatically supplied by the compiler.
    ''' </param>
    ''' <param name="sourceLineNumber">
    '''  Optional. The line number in the source file at which the
    '''  method is called, automatically supplied by the compiler.
    ''' </param>
    <Extension>
    Friend Sub PlotTreatmentMarkers(
        chart As Chart,
        treatmentTimeChangeSeries As Series,
        <CallerMemberName> Optional memberName As String = Nothing,
        <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)

        Dim lastTimeChangeRecord As TimeChange = Nothing
        s_treatmentMarkersInsulin.Clear()
        s_treatmentMarkersMeal.Clear()
        Dim key As String
        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Try
                Dim item As Marker = markerWithIndex.Value
                Dim markerDateTime As Date = item.GetMarkerTimestamp
                Dim markerOADate As New OADate(asDate:=markerDateTime)
                Dim markerOADateTime As New OADate(asDate:=item.GetMarkerTimestamp)
                Dim markerSeriesPoints As DataPointCollection = chart.Series(name:=MarkerSeriesName).Points
                Dim markerBorderColor As Color
                Select Case item.Type
                    Case "AUTO_BASAL_DELIVERY"
                        key = NameOf(AutoBasalDelivery.BolusAmount)
                        Dim amount As Single = item.GetSingleFromJson(key, digits:=3)
                        With chart.Series(name:=BasalSeriesName)
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
                        chart.Series(name:=BasalSeriesName).PlotBasalSeries(
                            markerOADateTime,
                            amount,
                            bolusRow:=MaxBasalPerDose,
                            insulinRow:=TreatmentInsulinRow,
                            legendText:="Basal Series",
                            DrawFromBottom:=True,
                            tag:=GetToolTip(item.Type, amount))

                    Case "INSULIN"
                        key = NameOf(Insulin.ActivationType)
                        Select Case item.GetStringFromJson(key)
                            Case "AUTOCORRECTION"
                                key = NameOf(Insulin.DeliveredFastAmount)
                                Dim amount As Single = item.GetSingleFromJson(key, digits:=3)
                                chart.Series(name:=BasalSeriesName).PlotBasalSeries(
                                    markerOADateTime,
                                    amount,
                                    bolusRow:=MaxBasalPerDose,
                                    insulinRow:=TreatmentInsulinRow,
                                    legendText:="Auto Correction",
                                    DrawFromBottom:=True,
                                    tag:=$"Auto Correction: {amount}U")
                            Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                                If s_treatmentMarkersInsulin.TryAdd(
                                    key:=markerOADateTime,
                                    value:=TreatmentInsulinRow) Then

                                    markerSeriesPoints.AddXY(xValue:=markerOADateTime, yValue:=TreatmentInsulinRow)
                                    Dim lastDataPoint As DataPoint = markerSeriesPoints.Last
                                    If Double.IsNaN(GetInsulinYValue()) Then
                                        lastDataPoint.Color = Color.Transparent
                                        lastDataPoint.MarkerSize = 0
                                    Else
                                        lastDataPoint.Color = Color.FromArgb(alpha:=30, baseColor:=Color.LightBlue)
                                        key = NameOf(Insulin.DeliveredFastAmount)
                                        markerBorderColor = Color.FromArgb(alpha:=10, baseColor:=Color.Black)
                                        Dim singleValue As Single = item.GetSingleFromJson(key, digits:=3)
                                        CreateCallout(
                                            chart,
                                            lastDataPoint,
                                            markerBorderColor,
                                            text:=$"Bolus {singleValue}U")
                                    End If
                                Else
                                    Stop
                                End If
                            Case Else
                                Stop
                        End Select
                    Case "MEAL"
                        Dim value As Single = CSng(TreatmentInsulinRow * 0.95).RoundToSingle(digits:=3)
                        If s_treatmentMarkersMeal.TryAdd(key:=markerOADateTime, value) Then
                            markerSeriesPoints.AddXY(xValue:=markerOADateTime, yValue:=value)
                            markerBorderColor = Color.FromArgb(alpha:=10, baseColor:=Color.Yellow)
                            Dim amount As Integer = CInt(item.GetSingleFromJson(key:="amount", digits:=0))
                            CreateCallout(
                                chart,
                                lastDataPoint:=markerSeriesPoints.Last,
                                markerBorderColor,
                                text:=$"Meal {amount} {GetCarbDefaultUnit()}")
                        End If
                    Case "BG_READING"
                    Case "CALIBRATION"
                    Case "TIME_CHANGE"
                        With chart.Series(name:=TimeChangeSeriesName)
                            lastTimeChangeRecord = New TimeChange(item, recordNumber:=1)
                            markerOADateTime = New OADate(asDate:=lastTimeChangeRecord.Timestamp)
                            .Points.AddXY(xValue:=markerOADateTime, yValue:=0)
                            .Points.AddXY(xValue:=markerOADateTime, yValue:=TreatmentInsulinRow)
                            .Points.AddXY(xValue:=markerOADateTime, yValue:=Double.NaN)
                        End With
                    Case "LOW_GLUCOSE_SUSPENDED"
                        If PatientData.ConduitSensorInRange AndAlso
                            CurrentPdf?.IsValid AndAlso
                            Not InAutoMode Then

                            For Each kvp As KeyValuePair(Of OADate, Single) In GetManualBasalValues(markerWithIndex)
                                Dim tag As String = $"Manual Basal: {kvp.Value.RoundToSingle(digits:=3)}U"
                                chart.Series(name:=BasalSeriesName).PlotBasalSeries(
                                    markerOADateTime:=kvp.Key,
                                    amount:=kvp.Value,
                                    bolusRow:=MaxBasalPerDose,
                                    insulinRow:=TreatmentInsulinRow,
                                    legendText:=BasalSeriesName,
                                    DrawFromBottom:=True,
                                    tag)
                            Next
                        End If
                    Case Else
                        Stop
                End Select
            Catch innerException As Exception
                Stop
                Dim str As String = innerException.DecodeException()
                Dim local As String = NameOf(PlotTreatmentMarkers)
                Dim message As String = $"{str} exception in {local} at {memberName} line {sourceLineNumber}"
                Throw New ApplicationException(message, innerException)
            End Try
        Next
        chart.Annotations.Last.BringToFront()

        If s_timeChangeMarkers.Count <> 0 Then
            treatmentTimeChangeSeries.IsVisibleInLegend = True
            chart.ChartAreas(name:=NameOf(ChartArea)).AxisX.AdjustXAxisStartTime(lastTimeChangeRecord)
            chart.Legends(index:=0).CustomItems.Last.Enabled = True
        Else
            treatmentTimeChangeSeries.IsVisibleInLegend = False
            chart.Legends(index:=0).CustomItems.Last.Enabled = False
        End If
    End Sub

End Module
