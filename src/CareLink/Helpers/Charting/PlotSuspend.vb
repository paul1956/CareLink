' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSuspend

    ''' <summary>
    '''  Plots the area on the <see cref="Chart"/> where insulin delivery was suspended
    '''  due to low glucose events.
    ''' </summary>
    ''' <param name="pageChart">The <see cref="Chart"/> control to plot onto.</param>
    ''' <param name="SuspendSeries">
    '''  The <see cref="Series"/> object representing the suspend area.
    ''' </param>
    ''' <remarks>
    '''  This method iterates through the list of low glucose suspension markers
    '''  and plots the corresponding area on the chart, using a specific color
    '''  for the suspend period. If a suspension is detected, it fills the area between
    '''  the suspension start and end times, handling multiple suspension events.
    ''' </remarks>
    <Extension>
    Friend Sub PlotSuspendArea(pageChart As Chart, SuspendSeries As Series)
        If s_suspendedMarkers.Count = 1 AndAlso
            Not s_suspendedMarkers(index:=0).deliverySuspended Then

            Exit Sub
        End If

        Dim lineColor As Color = GetGraphLineColor(key:="Suspend")
        With pageChart.Series(name:=SuspendSeriesName).Points
            Dim suspended As Boolean = False
            For Each e As IndexClass(Of LowGlucoseSuspended) In s_suspendedMarkers.WithIndex

                Dim suspendRecord As LowGlucoseSuspended = e.Value
                Dim xValue As Date = suspendRecord.Timestamp

                If suspendRecord.deliverySuspended Then
                    suspended = True
                    .AddXY(xValue, 0)
                    .Last.Color = lineColor
                    .AddXY(xValue, GetYMaxValueFromNativeMmolL())
                    Dim stopTimeSpan As TimeSpan =
                        If(e.IsLast,
                            PumpNow() - xValue,
                            s_suspendedMarkers(index:=e.Index + 1).Timestamp - xValue)

                    Dim incrementSpans As Integer =
                        CInt(Math.Ceiling(stopTimeSpan.TotalMinutes / 5)) - 1
                    For i As Long = 1 To incrementSpans
                        .AddXY(xValue.AddMinutes(value:=i * 5), 0)
                        .Last.Color = lineColor
                        .AddXY(
                            xValue.AddMinutes(value:=i * 5),
                            GetYMaxValueFromNativeMmolL())
                        .Last.Color = lineColor
                    Next

                    If Not e.IsLast Then
                        .AddXY(
                            s_suspendedMarkers(index:=e.Index + 1).Timestamp,
                            Double.NaN)
                    End If
                    .Last.Color = Color.Transparent
                Else
                    .AddXY(xValue, Double.NaN)
                    .Last.Color = Color.Transparent
                End If
            Next
        End With

    End Sub

End Module
