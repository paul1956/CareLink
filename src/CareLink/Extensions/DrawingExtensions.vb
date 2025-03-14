﻿' Licensed to the .NET Foundation under one or more agreements.
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
    Friend Function DrawCenteredArc(backImage As Bitmap, minutesToNextCalibration As Integer) As Bitmap

        If minutesToNextCalibration = 0 Then
            Return backImage
        End If
        Dim targetImage As Bitmap = backImage
        Dim myGraphics As Graphics = Graphics.FromImage(targetImage)
        myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim newPen As New Pen(GetColorFromTimeToNextCalib(minutesToNextCalibration / 60), 4)
        Dim backImageRectangle As New Rectangle(4, 2, backImage.Width - 6, backImage.Height - 6)

        Dim arcAngle As Integer
        Select Case minutesToNextCalibration
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
        myGraphics.DrawArc(newPen, backImageRectangle, -90, -arcAngle)
        Return targetImage
    End Function

    Public Function CreateTextIcon(s As String, backColor As Color) As Icon
        Dim fontToUse As New Font(familyName:="Segoe UI", emSize:=10, style:=FontStyle.Regular, unit:=GraphicsUnit.Pixel)
        Dim brushToUse As Brush = New SolidBrush(backColor.GetContrastingColor())
        Dim bitmapText As New Bitmap(16, 16)
        Dim g As Graphics = Graphics.FromImage(bitmapText)
        g.Clear(backColor)
        g.TextRenderingHint = Text.TextRenderingHint.SingleBitPerPixelGridFit
        g.DrawString(s, font:=fontToUse, brush:=brushToUse, x:=-2, y:=0)
        Return Icon.FromHandle(bitmapText.GetHicon())
    End Function

End Module
