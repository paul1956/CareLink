' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PlotSeriesLimits

    ''' <summary>
    '''  Generates an array mapping each <see cref="SG"/> record index to the corresponding limit record index.
    ''' </summary>
    ''' <param name="count">The number of SG records minus one.</param>
    ''' <returns>
    '''  An array of integers where each element represents the index of the limit record
    '''  to be used for the corresponding SG record.
    ''' </returns>
    Private Function GetLimitsList(count As Integer) As Integer()
        Dim limitsIndexList(count) As Integer
        Dim limitsIndex As Integer = 0
        For i As Integer = 0 To s_limitRecords.Count - 1
            s_limitRecords(i).Index = i
        Next

        For i As Integer = 0 To limitsIndexList.GetUpperBound(0)
            If limitsIndex + 1 < s_limitRecords.Count AndAlso s_limitRecords(limitsIndex + 1).Index < i Then
                limitsIndex += 1
            End If
            limitsIndexList(i) = limitsIndex
        Next
        Return limitsIndexList
    End Function

    ''' <summary>
    '''  Plots the SG (sensor glucose) target line and, optionally, the high and low limit lines on the specified chart.
    ''' </summary>
    ''' <param name="chart">The <see cref="Chart"/> control to plot onto.</param>
    ''' <param name="targetSsOnly">
    '''  If <c>True</c>, only the target SG line is plotted; if <c>False</c>, high and low limits are also plotted.
    ''' </param>
    ''' <remarks>
    '''  This method uses the global lists <see cref="s_sgRecords"/> and <see cref="s_limitRecords"/> to
    '''  determine the data points for plotting. It adds points to the chart's series for target SG, high limit,
    '''  and low limit.
    '''  If an exception occurs while plotting, an <see cref="ApplicationException"/> is thrown with details.
    ''' </remarks>
    <Extension>
    Friend Sub PlotHighLowLimitsAndTargetSg(chart As Chart, targetSsOnly As Boolean)
        If s_limitRecords.Count = 0 Then Exit Sub
        Dim limitsIndexList() As Integer = GetLimitsList(s_sgRecords.Count - 1)
        Dim targetSG As Single = If(CurrentUser Is Nothing, 0, CurrentUser.CurrentTarget)
        If Not targetSG.AlmostZero() Then
            chart.Series(TargetSgSeriesName).Points.AddXY(s_sgRecords(0).OaDateTime(), targetSG)
            chart.Series(TargetSgSeriesName).Points.AddXY(s_sgRecords.Last.OaDateTime(), targetSG)
        End If
        If targetSsOnly Then Exit Sub
        For Each sgListIndex As IndexClass(Of SG) In s_sgRecords.WithIndex()
            Dim sgOADateTime As OADate = sgListIndex.Value.OaDateTime()
            Try
                Dim limitsLowValue As Single = s_limitRecords(limitsIndexList(sgListIndex.Index)).LowLimit
                Dim limitsHighValue As Single = s_limitRecords(limitsIndexList(sgListIndex.Index)).HighLimit
                If limitsHighValue <> 0 Then
                    chart.Series(HighLimitSeriesName).Points.AddXY(sgOADateTime, limitsHighValue)
                End If
                If limitsLowValue <> 0 Then
                    chart.Series(LowLimitSeriesName).Points.AddXY(sgOADateTime, limitsLowValue)
                End If
            Catch innerException As Exception
                Stop
                Throw New ApplicationException(
                    message:=$"{innerException.DecodeException()} exception while plotting Limits in {NameOf(PlotHighLowLimitsAndTargetSg)}",
                    innerException)
            End Try
        Next
    End Sub

End Module
