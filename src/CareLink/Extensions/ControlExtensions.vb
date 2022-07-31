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
    Friend Function HorizontalCenterOn(parentControl As Control, ParamArray childControls() As Control) As Integer
        Dim totalWidth As Integer = childControls.First.Right - childControls.Last.Left
        Return parentControl.FindHorizontalMidpoint - (totalWidth \ 2)
    End Function

    <Extension>
    Friend Sub PaintMarker(e As ChartPaintEventArgs, markerImage As Bitmap, marketDictionary As Dictionary(Of Double, Single), imageYOffset As Integer)
        ' Draw the cloned portion of the Bitmap object.
        Dim halfHeight As Single = CSng(markerImage.Height / 2)
        Dim halfWidth As Single = CSng(markerImage.Width / 2)
        For Each markerKvp As KeyValuePair(Of Double, Single) In marketDictionary
            Dim imagePosition As RectangleF = RectangleF.Empty
            Dim chartAreaName As String = e.Chart.ChartAreas(0).Name
            imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.X, markerKvp.Key))
            imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis(chartAreaName, AxisName.Y, markerKvp.Value))
            imagePosition = e.ChartGraphics.GetAbsoluteRectangle(imagePosition)
            imagePosition.Width = markerImage.Width
            imagePosition.Height = markerImage.Height
            imagePosition.Y -= halfHeight
            imagePosition.X -= halfWidth
            ' Draw image
            e.ChartGraphics.Graphics.DrawImage(markerImage, imagePosition.X, imagePosition.Y + imageYOffset)
        Next
    End Sub

    <Extension>
    Friend Function PositionBelow(controlAbove As Control) As Integer
        Return controlAbove.Top + controlAbove.Height + 1
    End Function

End Module
