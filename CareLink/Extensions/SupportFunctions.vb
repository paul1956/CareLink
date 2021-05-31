' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Public Module SupportFunctions
    <Extension>
    Friend Function GetMilitaryHour(selectedStartTime As String) As Integer
        Return CInt(Format(Date.Parse(selectedStartTime), "HH"))
    End Function

    <Extension>
    Friend Function SafeGetSgDateTime(sgList As IReadOnlyList(Of Dictionary(Of String, String)), index As Integer) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString)
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString.Split("-")(0))
        Else
            sgDateTime = Now
        End If
        If sgList(index).Count < 7 Then
            sgDateTime = sgDateTime.AddMinutes(5)
        End If
        Return sgDateTime
    End Function

    <Extension>
    Friend Sub PaintMarker(e As ChartPaintEventArgs, markerImage As Bitmap, marketDictionary As Dictionary(Of Double, Integer), imageYOffset As Integer)
        ' Draw the cloned portion of the Bitmap object.
        Dim halfHeight As Single = CSng(markerImage.Height / 2)
        Dim halfWidth As Single = CSng(markerImage.Width / 2)
        For Each markerKvp As KeyValuePair(Of Double, Integer) In marketDictionary
            Dim imagePosition As RectangleF = RectangleF.Empty
            imagePosition.X = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.X, markerKvp.Key))
            imagePosition.Y = CSng(e.ChartGraphics.GetPositionFromAxis("Default", AxisName.Y, markerKvp.Value))
            imagePosition = e.ChartGraphics.GetAbsoluteRectangle(imagePosition)
            imagePosition.Width = markerImage.Width
            imagePosition.Height = markerImage.Height
            imagePosition.Y -= halfHeight
            imagePosition.X -= halfWidth
            ' Draw image
            e.ChartGraphics.Graphics.DrawImage(markerImage, imagePosition.X, imagePosition.Y + imageYOffset)
        Next
    End Sub

End Module
