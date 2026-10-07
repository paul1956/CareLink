' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

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
    Friend Function DrawCenteredArc(backImage As Bitmap, minutesToNextCalibration As Integer) As Bitmap
        ArgumentNullException.ThrowIfNull(argument:=backImage)
        If minutesToNextCalibration <= 0 Then
            Return backImage
        End If

        Dim hoursToNextCalibration As Double = minutesToNextCalibration / 60.0
        Dim clampedMinutes As Integer = Math.Min(Math.Max(minutesToNextCalibration, 0), 720)

        ' Clone into a known, non-indexed pixel format and draw on the clone. This avoids
        ' Graphics.FromImage ExternalException that can occur for some source bitmaps
        ' (indexed formats or malformed images). We return the cloned image with the
        ' arc drawn so callers receive a drawable Bitmap instance.
        ' Produce a drawable 32bpp ARGB bitmap without modifying or disposing the caller's image.
        Dim resultBmp As Bitmap = BitmapCache.ToDrawableBitmap(src:=backImage)

        Try
            Using myGraphics As Graphics = Graphics.FromImage(resultBmp)
                myGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                Using pen As New Pen(color:=GetColorFromTimeToNextCalib(hoursToNextCalibration), width:=4)
                    ' Ensure rectangle dimensions are valid for DrawArc
                    Dim rectWidth As Integer = Math.Max(resultBmp.Width - 6, 1)
                    Dim rectHeight As Integer = Math.Max(resultBmp.Height - 6, 1)
                    Dim rect As New Rectangle(x:=4, y:=2, width:=rectWidth, height:=rectHeight)
                    Dim sweepAngle As Integer = CInt(30 + (clampedMinutes / 720.0 * (360 - 30)))
                    myGraphics.DrawArc(pen, rect, startAngle:=-90, sweepAngle)
                End Using
            End Using
        Catch ex As ExternalException
            ' Diagnostics: avoid crashing the caller. Log and return the original image.
            Try
                Debug.WriteLine(message:=$"DrawCenteredArc Graphics.FromImage failed: {ex.GetType().FullName} - {ex.Message}")
                Debug.WriteLine(message:=$"backImage size={backImage.Width}x{backImage.Height} format={backImage.PixelFormat}")
            Catch
            End Try

            Return backImage
        End Try

        Return resultBmp
    End Function

    ''' <summary>
    '''  Creates a text icon with the specified string and background color.
    ''' </summary>
    ''' <param name="s">The string to display in the icon.</param>
    ''' <param name="backColor">The background color of the icon.</param>
    ''' <returns>An <see cref="Icon"/> containing the text.</returns>
    Public Function CreateTextIcon(s As String, backColor As Color) As Icon
        Dim brush As New SolidBrush(color:=backColor.ContrastingColor())
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
            Return Icon.FromHandle(bitmapText.GetHicon())
        End Using
    End Function

End Module
