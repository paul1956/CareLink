' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Module ChartingExtensions

    <Extension>
    Friend Sub PlotOnePoint(plotSeries As Series, sgOaDateTime As Double, bgValue As Single, mainLineColor As Color, MealRow As Double, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)
        Try
            With plotSeries
                If Single.IsNaN(bgValue) OrElse Math.Abs(bgValue - 0) < Single.Epsilon Then
                    .Points.AddXY(sgOaDateTime, MealRow)
                    .Points.Last().IsEmpty = True
                Else
                    .Points.AddXY(sgOaDateTime, bgValue)
                    If bgValue > s_limitHigh Then
                        .Points.Last.Color = Color.Lime
                    ElseIf bgValue < s_limitLow Then
                        .Points.Last.Color = Color.Red
                    Else
                        .Points.Last.Color = mainLineColor
                    End If

                End If
            End With
        Catch ex As Exception
            Throw New Exception($"{ex.Message} exception in {memberName} at {sourceLineNumber}")
        End Try

    End Sub

    <Extension>
    Friend Sub InitializeChartArea(c As ChartArea)
        With c
            .AxisX.Minimum = s_bindingSourceSGs(0).OADate()
            .AxisX.Maximum = s_bindingSourceSGs.Last.OADate()
            .AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Hours
            .AxisX.MajorGrid.IntervalOffsetType = DateTimeIntervalType.Hours
            .AxisX.MajorGrid.Interval = 1
            .AxisX.IntervalType = DateTimeIntervalType.Hours
            .AxisX.Interval = 2
        End With
    End Sub

    <Extension>
    Friend Sub PlotSgSeries(chartSeries As Series, MealRow As Double)
        For Each sgListIndex As IndexClass(Of SgRecord) In s_bindingSourceSGs.WithIndex()
            chartSeries.PlotOnePoint(sgListIndex.Value.OADate(),
                                    sgListIndex.Value.sg,
                                    Color.Black,
                                    MealRow)
        Next
    End Sub

    <Extension>
    Friend Sub PostPaintSupport(ByRef chartRelitivePosition As RectangleF, e As ChartPaintEventArgs, scaleHeight As Single, BolusRow As Single, insulinDictionary As Dictionary(Of Double, Single), mealDictionary As Dictionary(Of Double, Single), Optional homePageCursorTimeLabel As Label = Nothing)
        Debug.Print("At SyncLock")
        Dim useYAxis As Boolean = homePageCursorTimeLabel IsNot Nothing
        Dim highLimitY As Double
        Dim lowLimitY As Double
        Dim chartAreaName As String = e.Chart.ChartAreas(0).Name
        If chartRelitivePosition.IsEmpty Then
            chartRelitivePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.X, s_bindingSourceSGs(0).OADate))
            If useYAxis Then
                chartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, BolusRow))
                chartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, s_limitHigh)))) - chartRelitivePosition.Y
                highLimitY = e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, s_limitHigh)
                lowLimitY = e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, s_limitLow)
            Else
                chartRelitivePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y2, BolusRow))
                chartRelitivePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y2, CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, s_limitHigh)))) - chartRelitivePosition.Y
                highLimitY = e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y2, s_limitHigh)
                lowLimitY = e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y2, s_limitLow)
            End If
            chartRelitivePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.X, s_bindingSourceSGs.Last.OADate)) - chartRelitivePosition.X
            chartRelitivePosition = e.ChartGraphics.GetAbsoluteRectangle(chartRelitivePosition)
        End If
        If useYAxis Then

            Using b As New SolidBrush(Color.FromArgb(30, Color.Black))
                Dim highHeight As Integer = CInt(255 * scaleHeight)

                Dim chartY As Integer = CInt(chartRelitivePosition.Y)
                Dim homePagelocation As New Point(CInt(chartRelitivePosition.X), chartY)
                Dim homePageChartWidth As Integer = CInt(chartRelitivePosition.Width)
                Dim highAreaRectangle As New Rectangle(homePagelocation,
                                                       New Size(homePageChartWidth, highHeight))
                e.ChartGraphics.Graphics.FillRectangle(b, highAreaRectangle)

                Dim lowOffset As Integer = CInt((10 + chartRelitivePosition.Height) * scaleHeight)
                Dim lowStartLocation As New Point(CInt(chartRelitivePosition.X), lowOffset)

                Dim lowRawHeight As Integer = CInt((50 - chartY) * scaleHeight)
                Dim lowHeight As Integer = If(e.Chart.ChartAreas(0).AxisX.ScrollBar.IsVisible,
                                              CInt(lowRawHeight - e.Chart.ChartAreas(0).AxisX.ScrollBar.Size),
                                              lowRawHeight
                                             )
                Dim lowAreaRectangle As New Rectangle(lowStartLocation,
                                                      New Size(homePageChartWidth, lowHeight))
                e.ChartGraphics.Graphics.FillRectangle(b, lowAreaRectangle)
                If homePageCursorTimeLabel?.Tag IsNot Nothing Then
                    homePageCursorTimeLabel.Left = CInt(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.X, homePageCursorTimeLabel.Tag.ToString.ParseDate("").ToOADate))
                End If
            End Using

        End If
        e.PaintMarker(s_mealImage, mealDictionary, useYAxis)
        e.PaintMarker(s_insulinImage, insulinDictionary, useYAxis)
    End Sub

End Module
