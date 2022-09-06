' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Public Module ControlExtensions

    <Extension>
    Friend Function FindHorizontalMidpoint(ctrl As Control) As Integer
        Return ctrl.Left + (ctrl.Width \ 2)
    End Function

    <Extension>
    Friend Function FindVerticalMidpoint(ctrl As Control) As Integer
        Return ctrl.Top + (ctrl.Height \ 2)
    End Function

    <Extension>
    Friend Sub PaintMarker(e As ChartPaintEventArgs, markerImage As Bitmap, markerDictionary As Dictionary(Of OADate, Single), noImageOffset As Boolean, paintOnY2 As Boolean)
        ' Draw the cloned portion of the Bitmap object.
        Dim halfHeight As Single = CSng(If(noImageOffset, 0, markerImage.Height / 2))
        Dim halfWidth As Single = CSng(markerImage.Width / 2)
        For Each markerKvp As KeyValuePair(Of OADate, Single) In markerDictionary
            Dim imagePosition As RectangleF = RectangleF.Empty
            imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.X, markerKvp.Key))
            If paintOnY2 Then
                imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y2, markerKvp.Value))
            Else
                imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(ChartAreaName, AxisName.Y, markerKvp.Value))
            End If
            imagePosition.Width = markerImage.Width
            imagePosition.Height = markerImage.Height
            imagePosition = e.ChartGraphics.GetAbsoluteRectangle(imagePosition)
            imagePosition.Y -= halfHeight
            imagePosition.X -= halfWidth
            ' Draw image
            e.ChartGraphics.Graphics.DrawImage(markerImage, imagePosition.X, imagePosition.Y)
        Next
    End Sub

End Module
