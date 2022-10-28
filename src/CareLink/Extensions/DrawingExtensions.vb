' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DrawingExtensions

    Private Function GetColorFromTimeToNextCalib(hoursToNextCalibration As Double) As Color
        If hoursToNextCalibration <= 1.9 Then
            Return Color.Red
        ElseIf hoursToNextCalibration < 4 Then
            Return Color.Yellow
        Else
            Return Color.Lime
        End If
    End Function

    <Extension>
    Friend Function DrawCenteredArc(backImage As Bitmap, minutesToNextCalibration As Integer, Optional colorTable As IReadOnlyDictionary(Of String, Color) = Nothing, Optional segmentName As String = "") As Bitmap


        If minutesToNextCalibration = 0 Then
            Return backImage
        End If
        Dim targetImage As Bitmap = backImage
        Dim myGraphics As Graphics = Graphics.FromImage(targetImage)
        myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim newPen As Pen = If(colorTable Is Nothing, New Pen(GetColorFromTimeToNextCalib(minutesToNextCalibration / 60), 2), New Pen(colorTable(segmentName), 5))
        Dim rect As New Rectangle(1, 1, backImage.Width - 2, backImage.Height - 2)

        Dim arcAngle As Integer
        Select Case minutesToNextCalibration \ 2
            Case Is > 660
                arcAngle = 360
            Case Is > 600
                arcAngle = 330
            Case Is > 540
                arcAngle = 300
            Case Is > 480
                arcAngle = 270
            Case Is > 420
                arcAngle = 240
            Case Is > 360
                arcAngle = 210
            Case Is > 300
                arcAngle = 180
            Case Is > 240
                arcAngle = 150
            Case Is > 180
                arcAngle = 120
            Case Is > 120
                arcAngle = 90
            Case Is > 60
                arcAngle = 60
            Case Else
                arcAngle = 30
        End Select
        myGraphics.DrawArc(newPen, rect, -90, -arcAngle)
        Return targetImage
    End Function

    <Extension()>
    Public Function Split(collection As IEnumerable(Of Integer), parts As Integer) As IEnumerable(Of IEnumerable(Of Integer))
        If collection Is Nothing Then Throw New ArgumentNullException(NameOf(collection))
        If parts < 1 Then Throw New ArgumentOutOfRangeException(NameOf(parts))
        Dim count As Integer = collection.Count()
        If count = 0 Then Return Array.Empty(Of IEnumerable(Of Integer))()
        If parts >= count Then Return {collection}
        Return Enumerable.Range(0, CInt(Math.Ceiling(count / parts))).Select(Function(i) collection.Skip(i * parts).Take(parts))
    End Function
End Module
