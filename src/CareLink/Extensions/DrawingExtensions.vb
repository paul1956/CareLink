' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DrawingExtensions

    ''' <summary>
    '''  Gets a <see cref="Color"/> based on the time remaining until the next calibration.
    '''  The color will be red if less than 2 hours, yellow if between 2 and 4 hours,
    '''  and lime if more than 4 hours.
    ''' </summary>
    ''' <param name="hoursToNextCalibration">
    '''  The time in hours until the next calibration.
    ''' </param>
    ''' <remarks>
    '''  This function is used to determine the color for a calibration indicator.
    ''' </remarks>
    ''' <returns>
    '''  A <see cref="Color"/> representing the urgency of the calibration.
    ''' </returns>
    Private Function GetColorFromTimeToNextCalib(hoursToNextCalibration As Double) As Color
        If hoursToNextCalibration <= 1.9 Then
            Return Color.Red
        ElseIf hoursToNextCalibration < 4 Then
            Return Color.Yellow
        Else
            Return Color.Lime
        End If
    End Function

    ''' <summary>
    '''  Draws a centered arc on the provided bitmap based on the time remaining
    '''  until the next calibration.
    ''' </summary>
    ''' <param name="backImage">The background image to draw on.</param>
    ''' <param name="minutesToNextCalibration">
    '''  The time in minutes until the next calibration.
    ''' </param>
    ''' <returns>
    '''  A new bitmap with the drawn arc.
    ''' </returns>
    <Extension>
    Friend Function DrawCenteredArc(
        backImage As Bitmap,
        minutesToNextCalibration As Integer) As Bitmap

        If minutesToNextCalibration = 0 Then
            Return backImage
        End If
        Dim targetImage As Bitmap = backImage
        Dim myGraphics As Graphics = Graphics.FromImage(targetImage)
        myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim hoursToNextCalibration As Double = minutesToNextCalibration / 60
        Dim pen As New Pen(
            color:=GetColorFromTimeToNextCalib(hoursToNextCalibration),
            width:=4)

        Dim rect As New Rectangle(
            x:=4,
            y:=2,
            width:=backImage.Width - 6,
            height:=backImage.Height - 6)

        Dim sweepAngle As Integer =
            CInt(30 + (Math.Min(minutesToNextCalibration, 720) / 720.0 * (360 - 30)))
        myGraphics.DrawArc(pen, rect, startAngle:=-90, sweepAngle:=-sweepAngle)
        myGraphics.Dispose()
        Return targetImage
    End Function

    ''' <summary>
    '''  Creates a text icon with the specified string and background color.
    ''' </summary>
    ''' <param name="s">The string to display in the icon.</param>
    ''' <param name="backColor">The background color of the icon.</param>
    ''' <returns>An <see cref="Icon"/> containing the text.</returns>
    Public Function CreateTextIcon(s As String, backColor As Color) As Icon
        Dim brush As Brush = New SolidBrush(color:=backColor.ContrastingColor())
        Dim bitmapText As New Bitmap(width:=16, height:=16)
        Using g As Graphics = Graphics.FromImage(bitmapText)
            g.Clear(color:=backColor)
            g.TextRenderingHint = Text.TextRenderingHint.SingleBitPerPixelGridFit
            Dim fontToUse As New Font(
                FamilyName,
                emSize:=10,
                style:=FontStyle.Regular,
                unit:=GraphicsUnit.Pixel)
            g.DrawString(s, font:=fontToUse, brush, x:=-2, y:=0)
        End Using

        Return Icon.FromHandle(bitmapText.GetHicon())
    End Function

End Module
