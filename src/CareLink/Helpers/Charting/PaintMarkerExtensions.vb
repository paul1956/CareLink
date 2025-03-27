' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Friend Module PaintMarkerExtensions

    <Extension>
    Private Sub PaintMarker(e As ChartPaintEventArgs, markerImage As Bitmap, markerDictionary As Dictionary(Of OADate, Single), noImageOffset As Boolean, paintOnY2 As Boolean)
        ' Draw the cloned portion of the Bitmap object.
        Dim halfHeight As Single = CSng(If(noImageOffset, 0, markerImage.Height / 2))
        Dim halfWidth As Single = CSng(markerImage.Width / 2)
        For Each markerKvp As KeyValuePair(Of OADate, Single) In markerDictionary
            Dim imagePosition As RectangleF = RectangleF.Empty
            imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.X, markerKvp.Key))
            imagePosition.Y = If(paintOnY2,
                                 CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, markerKvp.Value)),
                                 CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y, markerKvp.Value))
                                )
            imagePosition.Width = markerImage.Width
            imagePosition.Height = markerImage.Height
            imagePosition = e.ChartGraphics.GetAbsoluteRectangle(imagePosition)
            imagePosition.Y -= halfHeight
            imagePosition.X -= halfWidth
            ' Draw image
            e.ChartGraphics.Graphics.DrawImage(markerImage, imagePosition.X, imagePosition.Y)
        Next
    End Sub

    <DebuggerNonUserCode()>
    <Extension>
    Friend Sub PostPaintSupport(e As ChartPaintEventArgs, ByRef chartRelativePosition As RectangleF, insulinDictionary As Dictionary(Of OADate, Single), mealDictionary As Dictionary(Of OADate, Single), offsetInsulinImage As Boolean, paintOnY2 As Boolean)
        If s_listOfSgRecords.Count = 0 OrElse Not ProgramInitialized Then
            Exit Sub
        End If

        If chartRelativePosition.IsEmpty Then
            chartRelativePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.X, s_listOfSgRecords(0).OaDateTime))
            chartRelativePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, GetYMaxValue(NativeMmolL)))
            chartRelativePosition.Height = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, TirHighLimit(NativeMmolL))))) - chartRelativePosition.Y
            chartRelativePosition.Width = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.X, s_listOfSgRecords.Last.OaDateTime)) - chartRelativePosition.X
        End If

        Dim highLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, TirHighLimit(NativeMmolL)))
        Dim lowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, TirLowLimit(NativeMmolL)))
        Dim criticalLowLimitY As Single = CSng(e.ChartGraphics.GetPositionFromAxis(NameOf(ChartArea), AxisName.Y2, GetYMinValue(NativeMmolL)))
        Dim chartAbsoluteHighRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(New RectangleF(chartRelativePosition.X, chartRelativePosition.Y, chartRelativePosition.Width, highLimitY - chartRelativePosition.Y))
        Dim chartAbsoluteLowRectangle As RectangleF = e.ChartGraphics.GetAbsoluteRectangle(New RectangleF(chartRelativePosition.X, lowLimitY, chartRelativePosition.Width, criticalLowLimitY - lowLimitY))

        Using b As New SolidBrush(Color.FromArgb(5, Color.Black))
            e.ChartGraphics.Graphics.FillRectangle(b, chartAbsoluteHighRectangle)
            e.ChartGraphics.Graphics.FillRectangle(b, chartAbsoluteLowRectangle)
        End Using

        If insulinDictionary IsNot Nothing Then
            e.PaintMarker(s_insulinImage, insulinDictionary, offsetInsulinImage, paintOnY2)
        End If
        If mealDictionary IsNot Nothing Then
            e.PaintMarker(s_mealImage, mealDictionary, False, paintOnY2)
        End If
    End Sub

End Module
