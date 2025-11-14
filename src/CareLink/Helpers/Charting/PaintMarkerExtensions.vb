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
    '''  Paints markers on the chart using the specified image and dictionary of markers.
    '''  The markers are drawn at their respective positions on the X-axis and Y-axis.
    '''  If <paramref name="noImageOffset"/> is <see langword="True"/>,
    '''  the image is drawn without any vertical offset.
    '''  If <paramref name="paintOnY2"/> is <see langword="True"/>,
    '''  the markers are painted on the Y2 axis; otherwise, they are painted on the Y axis.
    ''' </summary>
    ''' <param name="e">
    '''  The <see cref="ChartPaintEventArgs"/> containing chart graphics context.
    ''' </param>
    ''' <param name="markerImage">
    '''  The <see cref="Bitmap"/> image to use for the marker.
    ''' </param>
    ''' <param name="markerDictionary">
    '''  A dictionary mapping OADate values to Y-axis values for marker positions.
    ''' </param>
    ''' <param name="noImageOffset">
    '''  If <see langword="True"/>, the image is drawn without vertical offset;
    '''  otherwise, it is centered on the marker position.</param>
    ''' <param name="paintOnY2">
    '''  If <see langword="True"/>, markers are painted on the Y2 axis;
    '''  otherwise, on the Y axis.
    ''' </param>
    <Extension>
    Private Sub PaintMarker(
        e As ChartPaintEventArgs,
        markerImage As Bitmap,
        markerDictionary As Dictionary(Of OADate, Single),
        noImageOffset As Boolean,
        paintOnY2 As Boolean)

        ' Draw the cloned portion of the Bitmap object.
        Dim halfHeight As Single = CSng(If(noImageOffset,
                                           0,
                                           markerImage.Height / 2))

        Dim halfWidth As Single = CSng(markerImage.Width / 2)
        For Each markerKvp As KeyValuePair(Of OADate, Single) In markerDictionary
            Dim rectangle As RectangleF = RectangleF.Empty
            rectangle.X = CSng(e.ChartGraphics.GetPositionFromAxis(
                                ChartAreaName,
                                axis:=AxisName.X,
                                axisValue:=markerKvp.Key))

            rectangle.Y = If(paintOnY2,
                             CSng(e.ChartGraphics.GetPositionFromAxis(
                                ChartAreaName,
                                axis:=AxisName.Y2,
                                axisValue:=markerKvp.Value)),
                             CSng(e.ChartGraphics.GetPositionFromAxis(
                                ChartAreaName,
                                axis:=AxisName.Y,
                                axisValue:=markerKvp.Value)))

            rectangle.Width = markerImage.Width
            rectangle.Height = markerImage.Height

            Dim imagePosition As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(rectangle)
            imagePosition.Y -= halfHeight
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
    '''  Will be initialized if empty.
    ''' </param>
    ''' <param name="insulinDictionary">
    '''  A dictionary mapping OADate values to Y-axis values for insulin markers. Can be null.
    ''' </param>
    ''' <param name="mealDictionary">
    '''  A dictionary mapping OADate values to Y-axis values for meal markers. Can be null.
    ''' </param>
    ''' <param name="offsetInsulinImage">
    '''  If <see langword="True"/>, insulin marker images are vertically offset
    '''  to center on the marker position.
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
        insulinDictionary As Dictionary(Of OADate, Single),
        mealDictionary As Dictionary(Of OADate, Single),
        offsetInsulinImage As Boolean,
        paintOnY2 As Boolean)

        If s_sgRecords.Count = 0 OrElse Not ProgramInitialized Then
            Exit Sub
        End If

        If chartRelativePosition.IsEmpty Then
            chartRelativePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(
                ChartAreaName,
                axis:=AxisName.X,
                axisValue:=s_sgRecords(index:=0).OaDateTime))
            chartRelativePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(
                ChartAreaName,
                axis:=AxisName.Y2,
                axisValue:=GetYMaxNativeMmolL()))
            chartRelativePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(
                ChartAreaName,
                axis:=AxisName.Y2,
                axisValue:=CSng(e.ChartGraphics.GetPositionFromAxis(
                    ChartAreaName,
                    axis:=AxisName.Y2,
                    axisValue:=GetTirHighLimit())))) - chartRelativePosition.Y
            chartRelativePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(
                ChartAreaName,
                axis:=AxisName.X,
                axisValue:=s_sgRecords.Last.OaDateTime)) - chartRelativePosition.X
        End If

        Dim highLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(
            ChartAreaName,
            axis:=AxisName.Y2,
            axisValue:=GetTirHighLimit()))
        Dim lowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(
            ChartAreaName,
            axis:=AxisName.Y2,
            axisValue:=GetTirLowLimit()))
        Dim criticalLowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(
            ChartAreaName,
            axis:=AxisName.Y2,
            axisValue:=GetYMinNativeMmolL()))

        Dim rectangle As New RectangleF(
            chartRelativePosition.X,
            chartRelativePosition.Y,
            chartRelativePosition.Width,
            height:=highLimitY - chartRelativePosition.Y)
        Dim chartAbsoluteHighRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(rectangle)

        rectangle = New RectangleF(
            chartRelativePosition.X,
            y:=lowLimitY,
            width:=chartRelativePosition.Width,
            height:=criticalLowLimitY - lowLimitY)
        Dim chartAbsoluteLowRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(rectangle)

        Using brush As New SolidBrush(color:=Color.FromArgb(alpha:=5, baseColor:=Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(brush, rect:=chartAbsoluteHighRectangle)
            e.ChartGraphics.Graphics.FillRectangle(brush, rect:=chartAbsoluteLowRectangle)
        End Using

        If insulinDictionary IsNot Nothing Then
            e.PaintMarker(
                markerImage:=s_insulinImage,
                markerDictionary:=insulinDictionary,
                noImageOffset:=offsetInsulinImage,
                paintOnY2)
        End If
        If mealDictionary IsNot Nothing Then
            e.PaintMarker(
                markerImage:=s_mealImage,
                markerDictionary:=mealDictionary,
                noImageOffset:=False,
                paintOnY2)
        End If
    End Sub

End Module
