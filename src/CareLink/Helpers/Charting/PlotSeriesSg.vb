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
        For Each sgRecordWithIndex As IndexClass(Of SG) In s_sgRecords.WithIndex()
            Try
                With chart.Series(name:=SgSeriesName).Points
                    Dim sgRecord As SG = sgRecordWithIndex.Value
                    Dim xValue As OADate = sgRecord.OaDateTime()
                    Dim f As Single = sgRecord.sg
                    If Single.IsNaN(f) OrElse f.AlmostZero Then
                        .AddXY(xValue, yValue:=HomePageMealRow)
                        .Last().Color = Color.Transparent
                        .Last().IsEmpty = True
                    Else
                        If .Count > 0 AndAlso
                           (Not .Last.IsEmpty) AndAlso
                           New OADate(oADateAsDouble:= .Last.XValue).Within6Minutes(xValue) Then

                            .AddXY(.Last.XValue, yValue:=Double.NaN)
                            .Last().Color = Color.Transparent
                            .Last().IsEmpty = True
                        End If
                        .AddXY(xValue, yValue:=f)
                        If f > GetTirHighLimit() Then
                            .Last.Color = Color.Yellow
                        ElseIf f < GetTirLowLimit() Then
                            .Last.Color = Color.Red
                        Else
                            .Last.Color = Color.White
                        End If

                    End If
                End With
            Catch innerException As Exception
                Stop
                Dim str As String = innerException.DecodeException()
                Dim local As String = $"{NameOf(PlotSgSeries)}"
                Throw New ApplicationException(
                    message:=$"{str} exception in {local}",
                    innerException)
            End Try
        Next
    End Sub

End Module
