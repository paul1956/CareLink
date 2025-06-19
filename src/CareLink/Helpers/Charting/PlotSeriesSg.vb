' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSeriesSg

    ''' <summary>
    '''  Plots the SG <see cref="Series"/> on the specified <paramref name="chart"/>,
    '''  handling missing or out-of-range values and coloring points based on TIR (Time In Range) limits.
    ''' </summary>
    ''' <param name="chart">The <see cref="Chart"/> control to plot onto.</param>
    ''' <param name="HomePageMealRow">The Y-value to use for missing or invalid SG data points.</param>
    ''' <remarks>
    '''  This method iterates through the global list of SG records, plotting each value on the chart.
    '''  Points are colored based on their value relative to TIR limits:
    '''  - Yellow if above the high limit
    '''  - Red if below the low limit
    '''  - White if within range
    '''  Missing or invalid values are plotted as transparent and marked as empty.
    '''  If two consecutive valid points are within 6 minutes, a gap is inserted.
    '''  Exceptions are caught, stopped for debugging, and rethrown as <see cref="ApplicationException"/>.
    ''' </remarks>
    <Extension>
    Friend Sub PlotSgSeries(chart As Chart, HomePageMealRow As Double)
        For Each sgRecordWithIndex As IndexClass(Of SG) In s_listOfSgRecords.WithIndex()
            Try
                With chart.Series(SgSeriesName).Points
                    Dim sgRecord As SG = sgRecordWithIndex.Value
                    Dim sgOADateTime As OADate = sgRecord.OaDateTime()
                    Dim sgValue As Single = sgRecord.sg
                    If Single.IsNaN(sgValue) OrElse sgValue.AlmostZero Then
                        .AddXY(sgOADateTime, HomePageMealRow)
                        .Last().Color = Color.Transparent
                        .Last().IsEmpty = True
                    Else
                        If .Count > 0 AndAlso (Not .Last.IsEmpty) AndAlso New OADate(.Last.XValue).Within6Minutes(sgOADateTime) Then
                            .AddXY(.Last.XValue, Double.NaN)
                            .Last().Color = Color.Transparent
                            .Last().IsEmpty = True
                        End If
                        .AddXY(sgOADateTime, sgValue)
                        If sgValue > GetTirHighLimit() Then
                            .Last.Color = Color.Yellow
                        ElseIf sgValue < GetTirLowLimit() Then
                            .Last.Color = Color.Red
                        Else
                            .Last.Color = Color.White
                        End If

                    End If
                End With
            Catch innerException As Exception
                Stop
                Throw New ApplicationException(
                    message:=$"{innerException.DecodeException()} exception in {NameOf(PlotSgSeries)}",
                    innerException)
            End Try
        Next
    End Sub

End Module
