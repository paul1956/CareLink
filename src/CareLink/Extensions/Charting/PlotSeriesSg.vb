' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSeriesSg

    <Extension>
    Friend Sub PlotSgSeries(chart As Chart, HomePageMealRow As Double)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_listOfSGs.WithIndex()
            Try
                With chart.Series(BgSeriesName).Points
                    Dim sgOADateTime As OADate = sgListIndex.Value.OaDateTime()
                    Dim bgValue As Single = sgListIndex.Value.sg
                    If Single.IsNaN(bgValue) OrElse Math.Abs(bgValue - 0) < Single.Epsilon Then
                        .AddXY(sgOADateTime, HomePageMealRow)
                        .Last().Color = Color.Transparent
                        .Last().IsEmpty = True
                    Else
                        If .Count > 0 AndAlso (Not .Last.IsEmpty) AndAlso New OADate(.Last.XValue).Within10Minutes(sgOADateTime) Then
                            .AddXY(.Last.XValue, Double.NaN)
                            .Last().Color = Color.Transparent
                            .Last().IsEmpty = True
                        End If
                        .AddXY(sgOADateTime, bgValue)
                        If bgValue > s_limitHigh Then
                            .Last.Color = Color.Yellow
                        ElseIf bgValue < s_limitLow Then
                            .Last.Color = Color.Red
                        Else
                            .Last.Color = Color.White
                        End If

                    End If
                End With
            Catch ex As Exception
                Stop
                Throw New Exception($"{ex.DecodeException()} exception in {NameOf(PlotSgSeries)}")
            End Try
        Next
    End Sub

End Module
