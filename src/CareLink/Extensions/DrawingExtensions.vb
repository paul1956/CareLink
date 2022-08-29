' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DrawingExtensions

    Private Function GetColorFromTimeToNextCalib(timeToNextCalibHours As Double) As Color
        If timeToNextCalibHours <= 1.9 Then
            Return Color.Red
        ElseIf timeToNextCalibHours < 4 Then
            Return Color.Yellow
        Else
            Return Color.Lime
        End If
    End Function

    <Extension>
    Friend Function DrawCenteredArc(backImage As Bitmap, TimeToNextCalibration As Double, arcPercentage As Double, Optional colorTable As IReadOnlyDictionary(Of String, Color) = Nothing, Optional segmentName As String = "") As Bitmap
        If arcPercentage < Double.Epsilon Then
            Return backImage
        End If
        Dim targetImage As Bitmap = backImage
        Dim myGraphics As Graphics = Graphics.FromImage(targetImage)
        myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim newPen As Pen = If(colorTable Is Nothing, New Pen(GetColorFromTimeToNextCalib(TimeToNextCalibration), 2), New Pen(colorTable(segmentName), 5))
        Dim rect As New Rectangle(1, 1, backImage.Width - 2, backImage.Height - 2)
        myGraphics.DrawArc(newPen, rect, -90, -CInt(360 * arcPercentage))
        Return targetImage
    End Function


End Module
