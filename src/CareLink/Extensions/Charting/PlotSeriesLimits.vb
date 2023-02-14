' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSeriesLimits

    Private Function GetLimitsList(count As Integer) As Integer()
        Dim limitsIndexList(count) As Integer
        Dim limitsIndex As Integer = 0
        For i As Integer = 0 To limitsIndexList.GetUpperBound(0)
            If limitsIndex + 1 < s_listOfLimitRecords.Count AndAlso s_listOfLimitRecords(limitsIndex + 1).index < i Then
                limitsIndex += 1
            End If
            limitsIndexList(i) = limitsIndex
        Next
        Return limitsIndexList
    End Function

    <Extension>
    Friend Sub PlotHighLowLimits(chart As Chart)
        If s_listOfLimitRecords.Count = 0 Then Exit Sub
        Dim limitsIndexList() As Integer = GetLimitsList(s_listOfSGs.Count - 1)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_listOfSGs.WithIndex()
            Dim sgOADateTime As OADate = sgListIndex.Value.OaDateTime()
            Try
                Dim limitsLowValue As Single = s_listOfLimitRecords(limitsIndexList(sgListIndex.Index)).lowLimit
                Dim limitsHighValue As Single = s_listOfLimitRecords(limitsIndexList(sgListIndex.Index)).highLimit
                If limitsHighValue <> 0 Then
                    chart.Series(HighLimitSeriesName).Points.AddXY(sgOADateTime, limitsHighValue)
                End If
                If limitsLowValue <> 0 Then
                    chart.Series(LowLimitSeriesName).Points.AddXY(sgOADateTime, limitsLowValue)
                End If
            Catch ex As Exception
                Stop
                Throw New Exception($"{ex.DecodeException()} exception while plotting Limits in {NameOf(PlotHighLowLimits)}")
            End Try
        Next
    End Sub

End Module
