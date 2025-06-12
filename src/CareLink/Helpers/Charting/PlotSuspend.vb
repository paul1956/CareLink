﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSuspend

    <Extension>
    Friend Sub PlotSuspendArea(pageChart As Chart, SuspendSeries As Series)
        If s_listOfLowGlucoseSuspendedMarkers.Count = 1 AndAlso s_listOfLowGlucoseSuspendedMarkers(0).deliverySuspended = False Then
            Exit Sub
        End If

        Dim lineColor As Color = GetGraphLineColor("Suspend")
        With pageChart.Series(SuspendSeriesName).Points
            Dim suspended As Boolean = False
            For Each e As IndexClass(Of LowGlucoseSuspended) In s_listOfLowGlucoseSuspendedMarkers.WithIndex
                Dim suspendRecord As LowGlucoseSuspended = e.Value
                If suspendRecord.deliverySuspended Then
                    suspended = True
                    .AddXY(suspendRecord.Timestamp, 0)
                    .Last.Color = lineColor
                    .AddXY(suspendRecord.Timestamp, GetYMaxValueFromNativeMmolL())
                    Dim stopTimeSpan As TimeSpan =
                                      If(Not e.IsLast,
                                         s_listOfLowGlucoseSuspendedMarkers(e.Index + 1).Timestamp - suspendRecord.Timestamp,
                                         PumpNow() - suspendRecord.Timestamp
                                        )

                    For i As Long = 1 To CInt(Math.Ceiling(stopTimeSpan.TotalMinutes / 5)) - 1
                        .AddXY(suspendRecord.Timestamp.AddMinutes(i * 5), 0)
                        .Last.Color = lineColor
                        .AddXY(suspendRecord.Timestamp.AddMinutes(i * 5), GetYMaxValueFromNativeMmolL())
                        .Last.Color = lineColor
                    Next

                    If Not e.IsLast Then
                        .AddXY(s_listOfLowGlucoseSuspendedMarkers(e.Index + 1).Timestamp, Double.NaN)
                    End If
                    .Last.Color = Color.Transparent
                Else
                    .AddXY(suspendRecord.Timestamp, Double.NaN)
                    .Last.Color = Color.Transparent
                End If
            Next
        End With

    End Sub

End Module
