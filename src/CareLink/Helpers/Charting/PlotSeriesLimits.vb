' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSeriesLimits

    Private Function GetLimitsList(count As Integer) As Integer()
        Dim limitsIndexList(count) As Integer
        Dim limitsIndex As Integer = 0
        For i As Integer = 0 To s_listOfLimitRecords.Count - 1
            s_listOfLimitRecords(i).Index = i
        Next

        For i As Integer = 0 To limitsIndexList.GetUpperBound(0)
            If limitsIndex + 1 < s_listOfLimitRecords.Count AndAlso s_listOfLimitRecords(limitsIndex + 1).Index < i Then
                limitsIndex += 1
            End If
            limitsIndexList(i) = limitsIndex
        Next
        Return limitsIndexList
    End Function

    ''' <summary>
    ''' Plot Sg and optionally High and Low Limits
    ''' </summary>
    ''' <param name="chart">The Chart to plot onto</param>
    ''' <param name="targetSsOnly">Only plot Limits if True</param>
    <Extension>
    Friend Sub PlotHighLowLimitsAndTargetSg(chart As Chart, targetSsOnly As Boolean)
        If s_listOfLimitRecords.Count = 0 Then Exit Sub
        Dim limitsIndexList() As Integer = GetLimitsList(s_listOfSgRecords.Count - 1)
        Dim targetSG As Single = If(CurrentUser Is Nothing, 0, CurrentUser.CurrentTarget)
        If Not targetSG.AlmostZero() Then
            chart.Series(TargetSgSeriesName).Points.AddXY(s_listOfSgRecords(0).OaDateTime(), targetSG)
            chart.Series(TargetSgSeriesName).Points.AddXY(s_listOfSgRecords.Last.OaDateTime(), targetSG)
        End If
        If targetSsOnly Then Exit Sub
        For Each sgListIndex As IndexClass(Of SG) In s_listOfSgRecords.WithIndex()
            Dim sgOADateTime As OADate = sgListIndex.Value.OaDateTime()
            Try
                Dim limitsLowValue As Single = s_listOfLimitRecords(limitsIndexList(sgListIndex.Index)).LowLimit
                Dim limitsHighValue As Single = s_listOfLimitRecords(limitsIndexList(sgListIndex.Index)).HighLimit
                If limitsHighValue <> 0 Then
                    chart.Series(HighLimitSeriesName).Points.AddXY(sgOADateTime, limitsHighValue)
                End If
                If limitsLowValue <> 0 Then
                    chart.Series(LowLimitSeriesName).Points.AddXY(sgOADateTime, limitsLowValue)
                End If
            Catch ex As Exception
                Stop
                Throw New Exception($"{ex.DecodeException()} exception while plotting Limits in {NameOf(PlotHighLowLimitsAndTargetSg)}")
            End Try
        Next
    End Sub

End Module
