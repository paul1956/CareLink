' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

''' <summary>
'''  Provides extension methods for painting markers and post-painting
'''  support on chart controls.
''' </summary>
Friend Module PaintMarkerExtensions
    Private Const ChartAreaName As String = NameOf(ChartArea)

    ''' <summary>
    '''  Returns the vertical offset (value subtracted from imagePosition.Y)
    '''  for the given marker image and offset mode.
    ''' </summary>
    Private Function GetVerticalOffset(markerImage As Bitmap, offset As ImageOffsetEnum) As Single
        Select Case offset
            Case ImageOffsetEnum.None
                Return 0
            Case ImageOffsetEnum.Center
                Return CSng(markerImage.Height / 2)
            Case ImageOffsetEnum.HeightUp
                Return markerImage.Height
            Case ImageOffsetEnum.HeightDown
                ' Negative value so subtracting it moves the image down.
                Return -markerImage.Height
            Case Else
                Return CSng(markerImage.Height / 2)
        End Select
    End Function

    ''' <summary>
    '''  Core implementation that draws marker images for a sequence of (x,y) positions.
    '''  The caller is responsible for converting their dictionary values into the
    '''  supplied IEnumerable(Of KeyValuePair(Of OADate, Double)) where the Double
    '''  represents the Y-axis data value to be passed to GetPositionFromAxis.
    ''' </summary>
    <Extension>
    Private Sub PaintMarkerCore(e As ChartPaintEventArgs,
                                markerImage As Bitmap,
                                markerPoints As IEnumerable(Of KeyValuePair(Of OADate, Double)),
                                offset As ImageOffsetEnum,
                                paintOnY2 As Boolean)

        Dim verticalOffset As Single =
            GetVerticalOffset(markerImage, offset)
        Dim halfWidth As Single =
            CSng(markerImage.Width / 2)

        For Each markerKvp As KeyValuePair(Of OADate, Double) In markerPoints
            Dim rectangle As RectangleF = RectangleF.Empty
            rectangle.X = CSng(e.ChartGraphics.GetPositionFromAxis(
                               ChartAreaName,
                               axis:=AxisName.X,
                               axisValue:=markerKvp.Key))
            Dim y As Double
            If paintOnY2 Then
                y = e.ChartGraphics.GetPositionFromAxis(
                    ChartAreaName,
                    axis:=AxisName.Y2,
                    axisValue:=markerKvp.Value)
                rectangle.Y = CSng(y)
            Else
                y = e.ChartGraphics.GetPositionFromAxis(
                        ChartAreaName,
                        axis:=AxisName.Y,
                        axisValue:=markerKvp.Value)
                rectangle.Y = CSng(y)
            End If

            rectangle.Width = markerImage.Width
            rectangle.Height = markerImage.Height

            Dim imagePosition As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(rectangle)
            imagePosition.Y -= verticalOffset
            imagePosition.X -= halfWidth
            ' Draw image
            e.ChartGraphics.Graphics.DrawImage(image:=markerImage, imagePosition.X, imagePosition.Y)
        Next
    End Sub

    ''' <summary>
    '''  Provides post-painting support for the chart, including filling high/low limit
    '''  rectangles and painting insulin/meal markers.
    ''' </summary>
    ''' <param name="e">
    '''  The <see cref="ChartPaintEventArgs"/> containing chart graphics context.
    ''' </param>
    ''' <param name="chartRelativePosition">
    '''  A <see cref="RectangleF"/> representing the relative position of the chart area.
    '''  Will be initialized if <see langword="Nothing"/>.
    '''  DO NOT pass a new RectangleF() each time;
    '''  pass the same variable to avoid unnecessary recalculation.
    ''' </param>
    ''' <param name="insulinDictionary">
    '''  A dictionary mapping OADate values to Y-axis values for insulin markers. Can be null.
    ''' </param>
    ''' <param name="mealDictionary">
    '''  A dictionary mapping OADate values to Y-axis values for meal markers. Can be null.
    ''' </param>
    ''' <param name="offset">
    '''  Specifies the vertical offset mode for marker images.
    '''  Uses <see cref="ImageOffsetEnum"/>.
    ''' </param>
    ''' <param name="paintOnY2">
    '''  If <see langword="True"/>, markers are painted on the Y2 axis;
    '''  otherwise, on the Y axis.
    ''' </param>
    <DebuggerNonUserCode()>
    <Extension>
    Friend Sub PostPaintSupport(
        e As ChartPaintEventArgs,
        ByRef chartRelativePosition As RectangleF,
        insulinDictionary As Dictionary(Of OADate, Double),
        mealDictionary As Dictionary(Of OADate, Double),
        otherDictionary As Dictionary(Of OADate, Double),
        timeChangedDictionary As Dictionary(Of OADate, Double),
        offset As ImageOffsetEnum,
        paintOnY2 As Boolean)

        If s_sgRecords.Count = 0 OrElse Not ProgramInitialized Then
            Return
        End If

        If chartRelativePosition.IsEmpty Then
            chartRelativePosition.X =
                CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName,
                                                         axis:=AxisName.X,
                                                         axisValue:=s_sgRecords(index:=0).OaDateTime))
            chartRelativePosition.Y =
                CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName,
                                                         axis:=AxisName.Y2,
                                                         axisValue:=GetYMaxNativeMmolL()))
            ' Use the native-min Y value as the axis data value, then convert to
            ' chart pixel position for height calculation.
            Dim axisValue As Single =
                GetYMinNativeMmolL()

            Dim y2 As Single =
                CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName,
                                                         axis:=AxisName.Y2,
                                                         axisValue:=axisValue))
            chartRelativePosition.Height =
                y2 - chartRelativePosition.Y

            Dim x As Single =
                CSng(e.ChartGraphics.
                       GetPositionFromAxis(ChartAreaName,
                                           axis:=AxisName.X,
                                           axisValue:=s_sgRecords.Last.OaDateTime))
            chartRelativePosition.Width =
                x - chartRelativePosition.X
        End If

        Dim highLimitY As Single =
            CSng(e.ChartGraphics.
                   GetPositionFromAxis(ChartAreaName,
                                       axis:=AxisName.Y2,
                                       axisValue:=GetTirHighLimit()))
        Dim lowLimitY As Single =
            CSng(e.ChartGraphics.
                   GetPositionFromAxis(ChartAreaName,
                                       axis:=AxisName.Y2,
                                       axisValue:=GetTirLowLimit()))
        Dim criticalLowLimitY As Single =
            CSng(e.ChartGraphics.
                   GetPositionFromAxis(ChartAreaName,
                                       axis:=AxisName.Y2,
                                       axisValue:=GetYMinNativeMmolL()))

        Dim height As Single =
            highLimitY - chartRelativePosition.Y
        Dim rectangle As New RectangleF(chartRelativePosition.X,
                                        chartRelativePosition.Y,
                                        chartRelativePosition.Width,
                                        height)
        Dim chartAbsoluteHighRectangle As RectangleF =
            e.ChartGraphics.GetAbsoluteRectangle(rectangle)

        rectangle = New RectangleF(chartRelativePosition.X,
                                   y:=lowLimitY,
                                   chartRelativePosition.Width,
                                   height:=criticalLowLimitY - lowLimitY)
        Dim chartAbsoluteLowRectangle As RectangleF =
            e.ChartGraphics.GetAbsoluteRectangle(rectangle)

        Using brush As New SolidBrush(color:=Color.FromArgb(alpha:=5, baseColor:=Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(brush, rect:=chartAbsoluteHighRectangle)
            e.ChartGraphics.Graphics.FillRectangle(brush, rect:=chartAbsoluteLowRectangle)
        End Using

        If insulinDictionary IsNot Nothing Then
            e.PaintMarkerCore(markerImage:=InsulinVialTiny,
                              markerPoints:=insulinDictionary,
                              offset:=offset,
                              paintOnY2)
        End If
        If mealDictionary IsNot Nothing Then
            e.PaintMarkerCore(markerImage:=MealImage,
                              markerPoints:=mealDictionary,
                              offset:=ImageOffsetEnum.HeightUp,
                              paintOnY2)
        End If
        If otherDictionary IsNot Nothing Then
            e.PaintMarkerCore(markerImage:=NoteImage,
                              markerPoints:=otherDictionary,
                              offset:=offset,
                              paintOnY2)
        End If
        If timeChangedDictionary IsNot Nothing Then
            e.PaintMarkerCore(markerImage:=TimeChangedImage,
                              markerPoints:=timeChangedDictionary,
                              offset:=ImageOffsetEnum.HeightUp,
                              paintOnY2)
        End If
    End Sub

End Module
