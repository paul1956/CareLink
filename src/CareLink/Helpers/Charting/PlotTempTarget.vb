' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Public Module PlotTempTarget

    ''' <summary>
    ''' Plots the temporary target area on the specified chart using the provided series.
    ''' </summary>
    ''' <param name="pageChart">The chart on which to plot the temporary target area.</param>
    ''' <param name="SummaryTempTargetSeries">
    '''  The series used to plot the temporary target area.
    ''' </param>
    <Extension>
    Friend Sub PlotTempTargetArea(pageChart As Chart,
                                  SummaryTempTargetSeries As Series)

        If s_circularOADateBuffer.IsEmpty Then
            Exit Sub
        End If

        Dim tempMarkers As List(Of KeyValuePair(Of OADate, Single)) =
            s_circularOADateBuffer.ToKeyValuePairList()

        Dim lineColor As Color = GetGraphLineColor(key:="Temp Target")

        For index As Integer = 0 To tempMarkers.Count - 1
            Dim f As Single = tempMarkers(index).Value
            If Single.IsNaN(f) OrElse f.AlmostZero Then
                Continue For
            End If

            Dim startX As Double = tempMarkers(index).Key
            Dim endX As Double

            If index < tempMarkers.Count - 1 Then
                ' Use the next timestamp, regardless of its f.
                endX = CDbl(tempMarkers(index:=index + 1).Key)
            Else
                ' No next timestamp is available for the final entry.
                endX = startX + DefaultPeriodDays
            End If

            If endX <= startX Then
                Continue For
            End If

            With SummaryTempTargetSeries.Points
                Dim startIndex As Integer = .AddXY(xValue:=startX, yValue:=f)
                .Item(index:=startIndex).Color = lineColor

                Dim endIndex As Integer = .AddXY(xValue:=endX, yValue:=f)
                .Item(index:=endIndex).Color = lineColor

                ' Prevent a connection to the next visible interval.
                Dim separatorIndex As Integer = .AddXY(xValue:=endX, yValue:=f)

                With .Item(index:=separatorIndex)
                    .IsEmpty = True
                    .Color = Color.Transparent
                End With
            End With
        Next

    End Sub

End Module
