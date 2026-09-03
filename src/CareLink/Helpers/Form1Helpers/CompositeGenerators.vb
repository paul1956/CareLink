Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Module CompositeGenerators

    ''' <summary>
    '''  Creates a pump battery composite image by filling the transparent area of the base image
    '''  with a vertical fill representing <paramref name="currentPercent"/>, scaling the
    '''  transparent paint rectangle from the base image to the target size.
    '''  The caller owns the returned <see cref="Bitmap"/> instance and should dispose it when no longer needed.
    ''' </summary>
    ''' <param name="baseBmp">
    '''  The base bitmap containing the transparent area.
    ''' </param>
    ''' <param name="targetSize">The size of the output bitmap.</param>
    ''' <param name="currentPercent">
    '''  The percentage of the battery level to display.
    ''' </param>
    ''' <param name="paintRect">
    '''  The rectangle in the base image to fill.
    ''' </param>
    ''' <param name="fillColor">
    '''  The color to use for the fill.
    ''' </param>
    ''' <returns>The created pump battery composite bitmap.</returns>
    Public Function CreatePumpBatteryComposite(baseBmp As Bitmap,
                                               targetSize As Size,
                                               currentPercent As Single,
                                               paintRect As Rectangle,
                                               fillColor As Color) As Bitmap
        If baseBmp Is Nothing Then
            Return Nothing
        End If

        Dim outBmp As New Bitmap(targetSize.Width, targetSize.Height, format:=Imaging.PixelFormat.Format32bppArgb)
        Using g As Graphics = Graphics.FromImage(outBmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.Clear(color:=Color.Transparent)

            ' Scale paintRect from baseBmp coordinates to targetSize
            Dim scaleX As Single = CSng(targetSize.Width) / CSng(baseBmp.Width)
            Dim scaleY As Single = CSng(targetSize.Height) / CSng(baseBmp.Height)

            Dim scaledRect As New RectangleF(x:=paintRect.X * scaleX,
                                             y:=paintRect.Y * scaleY,
                                             width:=paintRect.Width * scaleX,
                                             height:=paintRect.Height * scaleY)

            Dim fillHeight As Single = currentPercent / 100.0F * scaledRect.Height
            Dim fillTopY As Single = scaledRect.Y + (scaledRect.Height - fillHeight)

            If currentPercent > 0.0F Then
                Using br As New SolidBrush(color:=fillColor)
                    Dim fillRect As New RectangleF(scaledRect.X, fillTopY, scaledRect.Width, fillHeight)
                    g.FillRectangle(brush:=br, rect:=fillRect)
                End Using
            End If

            ' Draw the base image scaled to the target on top (base image contains transparent area)
            g.DrawImage(image:=baseBmp, x:=0, y:=0, targetSize.Width, targetSize.Height)
        End Using

        Return outBmp
    End Function

    ''' <summary>
    ''' Builds a cache key for pump battery composites.
    ''' </summary>
    Private Function BuildPumpBatteryKey(imageId As ImageEnum, hours As Integer, remainingMinutes As Integer, targetSize As Size, Optional extraKey As String = Nothing) As String
        Dim key As String = String.Format("{0}_{1}_{2}_{3}x{4}", imageId.Description, hours, remainingMinutes, targetSize.Width, targetSize.Height)
        If Not String.IsNullOrEmpty(extraKey) Then
            key = String.Concat(key, "_", extraKey)
        End If
        Return key
    End Function

    ''' <summary>
    ''' Returns a cached pump battery composite or creates one using the BitmapCache and ImageHelper.
    ''' The returned Bitmap is a clone and the caller owns/disposes it.
    ''' </summary>
    ''' <remarks>
    ''' Usage:
    ''' <code>
    ''' Dim bmp As Bitmap = CompositeGenerators.GetOrCreatePumpBatteryComposite(ImageEnum.PumpBatteryFlexMaster, targetSize, pumpMinutes)
    ''' pictureBox.Image = bmp ' caller owns and should Dispose when replacing
    ''' </code>
    ''' The cache key includes the image id, hours, minutes and target size. If you have additional
    ''' parameters that affect rendering (for example a "solut" string), pass them via the optional extraKey
    ''' parameter to avoid accidental cache reuse.
    ''' </remarks>
    Public Function GetOrCreatePumpBatteryComposite(imageId As ImageEnum,
                                                    targetSize As Size,
                                                    pumpBatteryLevelMinutes As Integer,
                                                    Optional extraKey As String = Nothing) As Bitmap
        Dim hours As Integer = pumpBatteryLevelMinutes \ 60
        Dim remainingMinutes As Integer = pumpBatteryLevelMinutes Mod 60

        Dim currentPercent As Single
        Dim fillColor As Color
        If hours > 10 Then
            currentPercent = 100.0F
            fillColor = Color.Lime
        ElseIf hours > 1 Then
            currentPercent = hours * 5.0F
            fillColor = Color.Yellow
        Else
            currentPercent = remainingMinutes * 0.167F
            fillColor = Color.Red
        End If

        Dim key As String = BuildPumpBatteryKey(imageId,
                                                hours,
                                                remainingMinutes,
                                                targetSize,
                                                extraKey)

        Dim generator As Func(Of Bitmap) =
            Function()
                Dim baseBmp As Bitmap = GetBitmapFromCache(imageId)
                If baseBmp Is Nothing Then
                    Return Nothing
                End If
                Dim paintRect As Rectangle =
                    ImageHelper.FindTransparentBounds(bmp:=baseBmp)
                Dim composed As Bitmap =
                    CreatePumpBatteryComposite(baseBmp,
                                               targetSize,
                                               currentPercent,
                                               paintRect,
                                               fillColor)
                baseBmp.Dispose()
                Return composed
            End Function

        Return BitmapCache.GetOrCreateTempBitmap(key, generator)
    End Function

    ''' <summary>
    ''' Replace the cached pump battery composite for the imageId/parameters and return the new cached clone.
    ''' Useful when you want to force a refresh for the same key instead of using a different key.
    ''' </summary>
    ''' <remarks>
    ''' This forces the cache entry to be replaced for the computed key and returns the newly-created
    ''' clone. Use this when you need to update the existing cache entry in-place (for example when a
    ''' background parameter changes but you want to keep the same logical cache key).
    ''' Example:
    ''' <code>
    ''' Dim bmp As Bitmap = CompositeGenerators.ReplacePumpBatteryComposite(ImageEnum.PumpBatteryFlexMaster, targetSize, pumpMinutes, extraKey)
    ''' pictureBox.Image = bmp
    ''' </code>
    ''' </remarks>
    Public Function ReplacePumpBatteryComposite(imageId As ImageEnum, targetSize As Size, pumpBatteryLevelMinutes As Integer, Optional extraKey As String = Nothing) As Bitmap
        Dim hours As Integer = pumpBatteryLevelMinutes \ 60
        Dim remainingMinutes As Integer = pumpBatteryLevelMinutes Mod 60

        Dim currentPercent As Single
        Dim brushColor As Color
        If hours > 10 Then
            currentPercent = 100.0F
            brushColor = Color.Lime
        ElseIf hours > 1 Then
            currentPercent = hours * 5.0F
            brushColor = Color.Yellow
        Else
            currentPercent = remainingMinutes * 0.167F
            brushColor = Color.Red
        End If

        Dim key As String = BuildPumpBatteryKey(imageId, hours, remainingMinutes, targetSize, extraKey)

        Dim baseBmp As Bitmap = BitmapCache.GetBitmapFromCache(imageId)
        If baseBmp Is Nothing Then Return Nothing
        Dim paintRect As Rectangle = ImageHelper.FindTransparentBounds(bmp:=baseBmp)
        Dim composed As Bitmap = CreatePumpBatteryComposite(baseBmp:=baseBmp,
                                                           targetSize:=targetSize,
                                                           currentPercent:=currentPercent,
                                                           paintRect:=paintRect,
                                                           fillColor:=brushColor)
        baseBmp.Dispose()

        BitmapCache.ReplaceTempBitmap(key, composed)
        ' Return a clone of the stored one
        Return BitmapCache.GetTempBitmapFromCache(key)
    End Function

End Module
