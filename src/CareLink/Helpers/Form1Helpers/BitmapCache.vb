' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

Public Module BitmapCache

    Private Const MaxTempBitmaps As Integer = 6

    ' Temporary composite images cache (small, bounded). Keys are application-defined
    ' and should include any parameters that affect rendering (eg: percent, state, size).
    Private ReadOnly s_tempBitmaps As New Dictionary(Of String, Bitmap)(Comparer)

    Private ReadOnly s_tempLock As New Object()

    Private ReadOnly s_tempOrder As New List(Of String)()

    ' Storage for the preloaded Bitmaps
    Friend ReadOnly s_bitmaps As New Dictionary(Of String, Bitmap)(Comparer)

    ''' <summary>
    ''' Ensure the provided bitmap is 32bpp ARGB. If it already is, the same instance is returned.
    ''' Otherwise a new 32bpp ARGB bitmap is created, the source is drawn into it, the source
    ''' is disposed, and the new bitmap is returned.
    ''' </summary>
    Private Function EnsureBitmap32bpp(src As Bitmap) As Bitmap
        If src Is Nothing Then
            Return Nothing
        End If

        If src.PixelFormat = Imaging.PixelFormat.Format32bppArgb Then
            Return src
        End If

        Try
            Dim converted As New Bitmap(src.Width,
                                        src.Height,
                                        format:=Imaging.PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(converted)
                g.Clear(color:=Color.Transparent)
                g.DrawImage(image:=src, x:=0, y:=0, src.Width, src.Height)
            End Using
            Try
                src.Dispose()
            Catch
            End Try
            Return converted
        Catch
            ' If conversion fails, fall back to returning the original (caller should handle nulls where appropriate).
            Return src
        End Try
    End Function

    ''' <summary>
    ''' Loads a bitmap from a byte buffer and converts it to 32bpp ARGB.
    ''' Returns Nothing on failure.
    ''' </summary>
    Private Function LoadBitmapFromBytes(buffer As Byte()) As Bitmap
        If buffer Is Nothing OrElse buffer.Length = 0 Then
            Return Nothing
        End If
        Try
            Using ms As New MemoryStream(buffer)
                Using src As Image = Image.FromStream(ms)
                    Dim bmp As New Bitmap(src.Width,
                                          src.Height,
                                          format:=Imaging.PixelFormat.Format32bppArgb)
                    Using g As Graphics = Graphics.FromImage(bmp)
                        g.Clear(color:=Color.Transparent)
                        g.DrawImage(image:=src,
                                    x:=0,
                                    y:=0,
                                    src.Width,
                                    src.Height)
                    End Using
                    Return bmp
                End Using
            End Using
        Catch
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  Trim fully-transparent border pixels from a 32bpp ARGB bitmap.
    '''  Returns a new bitmap containing only the non-transparent bounds. If the
    '''  source is fully transparent, returns a 1x1 transparent bitmap.
    ''' </summary>
    Private Function TrimTransparentBorder(src As Bitmap) As Bitmap
        If src Is Nothing Then
            Return Nothing
        End If

        Dim w As Integer = src.Width
        Dim h As Integer = src.Height
        Dim left As Integer = w
        Dim top As Integer = h
        Dim right As Integer = -1
        Dim bottom As Integer = -1

        Dim bitmapData As Imaging.BitmapData = Nothing
        Dim bytes() As Byte = Nothing
        Try
            Dim rect As New Rectangle(0, 0, w, h)
            bitmapData = src.LockBits(rect, Imaging.ImageLockMode.ReadOnly, Imaging.PixelFormat.Format32bppArgb)
            Dim total As Integer = Math.Abs(bitmapData.Stride) * bitmapData.Height
            ReDim bytes(total - 1)
            Marshal.Copy(source:=bitmapData.Scan0, destination:=bytes, startIndex:=0, length:=total)

            For y As Integer = 0 To h - 1
                Dim row As Integer = y * bitmapData.Stride
                For x As Integer = 0 To w - 1
                    Dim alpha As Integer = bytes(row + (x * 4) + 3)
                    If alpha <> 0 Then
                        If x < left Then left = x
                        If x > right Then right = x
                        If y < top Then top = y
                        If y > bottom Then bottom = y
                    End If
                Next
            Next
        Finally
            If bitmapData IsNot Nothing Then
                Try
                    src.UnlockBits(bitmapData)
                Catch
                End Try
            End If
        End Try

        ' If no non-transparent pixels found, return a 1x1 transparent bitmap.
        If right < left OrElse bottom < top Then
            Return New Bitmap(width:=1,
                              height:=1,
                              format:=Imaging.PixelFormat.Format32bppArgb)
        End If

        Dim rectW As Integer = right - left + 1
        Dim rectH As Integer = bottom - top + 1
        Dim outBmp As New Bitmap(width:=rectW,
                                 height:=rectH,
                                 format:=Imaging.PixelFormat.Format32bppArgb)
        Using g As Graphics = Graphics.FromImage(outBmp)
            g.Clear(color:=Color.Transparent)
            g.DrawImage(image:=src,
                        destRect:=New Rectangle(x:=0, y:=0, width:=rectW, height:=rectH),
                        srcRect:=New Rectangle(x:=left, y:=top, width:=rectW, height:=rectH),
                        srcUnit:=GraphicsUnit.Pixel)
        End Using

        Return outBmp
    End Function

    ''' <summary>
    '''  Gets a trimmed <see cref="Bitmap"/> from <see cref="s_bitmaps"/> after translating imageId to Name.
    '''  Fully-transparent border pixels are removed.
    ''' </summary>
    ''' <param name="imageId"><see cref="ImageEnum"/></param>
    ''' <returns>Trimmed bitmap from s_bitmaps</returns>
    Friend Function GetBitmapFromCache(imageId As ImageEnum) As Bitmap
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=imageId.Description, value) Then
            ' Cached bitmaps are stored trimmed and as 32bpp ARGB. Return a clone so
            ' the caller owns the returned instance and can dispose it safely.
            Try
                Return CType(value.Clone(), Bitmap)
            Catch
                ' Fallback: attempt ToDrawableBitmap if clone fails for any reason
                Return ToDrawableBitmap(src:=value)
            End Try
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    '''  Gets a bitmap from the cache and sizes it to the provided canvas. This overload
    '''  scales the cached trimmed image so its largest dimension matches the canvas
    '''  largest dimension and centers it on a transparent canvas of the requested size.
    '''  This is a convenience overload that forwards to the full-parameter form.
    ''' </summary>
    ''' <param name="imageId"><see cref="ImageEnum"/></param>
    ''' <param name="canvasSize">Target canvas size.</param>
    ''' <returns>New centered/scaled bitmap sized to canvasSize</returns>
    Friend Function GetBitmapFromCache(imageId As ImageEnum, canvasSize As Size) As Bitmap
        ' Build on the single-parameter helper: get the trimmed clone and scale/center it
        ' onto a transparent canvas of the requested size. This overload does NOT update
        ' the persistent cache (caching is handled by the full-parameter implementation)
        ' so it is safe to use in a paint loop where callers only need a sized bitmap.

        If canvasSize.Width <= 0 OrElse canvasSize.Height <= 0 Then
            Throw New ArgumentException(message:="canvasSize must have positive Width and Height.", paramName:=NameOf(canvasSize))
        End If

        ' Retrieve the trimmed cached bitmap (clone) and size it into the requested canvas.
        Dim trimmed As Bitmap = GetBitmapFromCache(imageId)
        If trimmed Is Nothing Then
            Return Nothing
        End If

        Try
            Dim contentTargetLargest As Integer =
                Math.Max(canvasSize.Width, canvasSize.Height)
            Dim srcLargest As Integer =
                Math.Max(trimmed.Width, trimmed.Height)
            Dim scale As Double = 1.0
            If srcLargest > 0 Then
                scale = CDbl(contentTargetLargest) / CDbl(srcLargest)
            End If

            Dim scaledW As Integer = Math.Max(1, CInt(Math.Round(trimmed.Width * scale)))
            Dim scaledH As Integer = Math.Max(1, CInt(Math.Round(trimmed.Height * scale)))

            Dim result As New Bitmap(canvasSize.Width, canvasSize.Height, Imaging.PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(result)
                g.Clear(color:=Color.Transparent)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                Dim offsetX As Integer = (canvasSize.Width - scaledW) \ 2
                Dim offsetY As Integer = (canvasSize.Height - scaledH) \ 2
                g.DrawImage(image:=trimmed,
                            rect:=New Rectangle(x:=offsetX,
                                                y:=offsetY,
                                                width:=scaledW,
                                                height:=scaledH))
            End Using

            Return result
        Finally
            Try
                trimmed.Dispose()
            Catch
            End Try
        End Try
    End Function

    ''' <summary>
    '''  Gets <see cref="Bitmap"/> from <see cref="s_bitmaps"/> after translating imageId to Name.
    '''  Returns a new image sized to canvasSize with the source image trimmed of transparent borders,
    '''  scaled so its LARGEST dimension matches contentMaxSize largest dimension (if provided),
    '''  otherwise the canvasSize largest dimension. The scaled content is centered on a transparent
    '''  canvas of size canvasSize.
    ''' </summary>
    ''' <param name="imageId"><see cref="ImageEnum"/></param>
    ''' <param name="canvasSize">Required target canvas size (e.g., PictureBox.Size).</param>
    ''' <param name="contentMaxSize">Desired largest dimension for the content; if empty (0,0) the canvas largest dimension is used.</param>
    ''' <returns>New centered/scaled bitmap sized to canvasSize</returns>
    Friend Function GetBitmapFromCache(imageId As ImageEnum, canvasSize As Size, contentMaxSize As Size, arcMinutes As Integer) As Bitmap
        If canvasSize.Width <= 0 OrElse canvasSize.Height <= 0 Then
            Throw New ArgumentException(message:="canvasSize must have positive Width and Height.", paramName:=NameOf(canvasSize))
        End If

        Dim result As Bitmap = Nothing

        ' If caller didn't specify a contentMaxSize,
        ' reuse the two-parameter sizing helper.
        If contentMaxSize.Width <= 0 AndAlso contentMaxSize.Height <= 0 Then
            result = GetBitmapFromCache(imageId, canvasSize)
        Else
            ' Scale using contentMaxSize as the target largest dimension.
            Dim trimmed As Bitmap = GetBitmapFromCache(imageId)
            If trimmed Is Nothing Then
                Return Nothing
            End If

            Try
                Dim contentTargetLargest As Integer = Math.Max(contentMaxSize.Width, contentMaxSize.Height)
                If contentTargetLargest <= 0 Then
                    contentTargetLargest = Math.Max(canvasSize.Width, canvasSize.Height)
                End If

                Dim srcLargest As Integer = Math.Max(trimmed.Width, trimmed.Height)
                Dim scale As Double = 1.0
                If srcLargest > 0 Then
                    scale = CDbl(contentTargetLargest) / CDbl(srcLargest)
                End If

                Dim scaledW As Integer = Math.Max(1, CInt(Math.Round(trimmed.Width * scale)))
                Dim scaledH As Integer = Math.Max(1, CInt(Math.Round(trimmed.Height * scale)))

                result = New Bitmap(canvasSize.Width, canvasSize.Height, Imaging.PixelFormat.Format32bppArgb)
                Using g As Graphics = Graphics.FromImage(result)
                    g.Clear(color:=Color.Transparent)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                    g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                    Dim offsetX As Integer = (canvasSize.Width - scaledW) \ 2
                    Dim offsetY As Integer = (canvasSize.Height - scaledH) \ 2
                    g.DrawImage(image:=trimmed,
                                rect:=New Rectangle(x:=offsetX,
                                                    y:=offsetY,
                                                    width:=scaledW,
                                                    height:=scaledH))
                End Using
            Finally
                Try
                    trimmed.Dispose()
                Catch
                End Try
            End Try
        End If

        If result Is Nothing Then
            Return Nothing
        End If

        ' Update the cached bitmap for this imageId to the newly-sized canvas.
        ' We only update the cache when no arc overlay is requested because the
        ' arc is time-dependent and should not replace the stored source image.
        Try
            If arcMinutes < 0 Then
                Dim cacheKey As String = imageId.Description
                Dim newCacheBmp As Bitmap = Nothing
                Try
                    newCacheBmp = EnsureBitmap32bpp(src:=CType(result.Clone(), Bitmap))
                    Dim oldBmp As Bitmap = Nothing
                    If s_bitmaps.TryGetValue(key:=cacheKey, value:=oldBmp) Then
                        Try
                            oldBmp.Dispose()
                        Catch
                        End Try
                    End If
                    s_bitmaps(cacheKey) = newCacheBmp
                    newCacheBmp = Nothing ' ownership transferred to cache
                Finally
                    If newCacheBmp IsNot Nothing Then
                        Try
                            newCacheBmp.Dispose()
                        Catch
                        End Try
                    End If
                End Try
            End If
        Catch
            ' Swallow any cache update failures; caching is best-effort.
        End Try

        ' If caller requested an arc overlay, draw it on the canvas before returning.
        If arcMinutes >= 0 Then
            Try
                Dim withArc As Bitmap = result.DrawCenteredArc(minutesToNextCalibration:=arcMinutes)
                Try
                    result.Dispose()
                Catch
                End Try
                Return withArc
            Catch
                ' If drawing fails, fall back to returning the canvas result.
                Return result
            End Try
        End If

        Return result

    End Function

    ''' <summary>
    ''' Get or create a temporary composite bitmap. The caller provides a stable key (includes parameters)
    ''' and a generator Func that creates the Bitmap when missing. The cache size is bounded to avoid memory growth.
    ''' The returned Bitmap is a clone; ownership/disposal rules: caller may dispose the returned image.
    ''' </summary>
    Friend Function GetOrCreateTempBitmap(key As String, generator As Func(Of Bitmap)) As Bitmap
        If String.IsNullOrEmpty(value:=key) Then
            Throw New ArgumentNullException(paramName:=NameOf(key))
        End If
        ArgumentNullException.ThrowIfNull(argument:=generator)

        SyncLock s_tempLock
            Dim existing As Bitmap = Nothing
            If s_tempBitmaps.TryGetValue(key, value:=existing) Then
                ' Cached temp bitmaps are normalized to 32bpp ARGB on insert, so a simple clone is sufficient.
                Return CType(existing.Clone(), Bitmap)
            End If

            ' Create and store the new composite
            Dim created As Bitmap = generator()
            If created Is Nothing Then Return Nothing

            ' Ensure temp cache stores 32bpp ARGB bitmaps to make clones cheaper and consistent.
            created = EnsureBitmap32bpp(src:=created)

            ' Enforce cap
            If s_tempBitmaps.Count >= MaxTempBitmaps Then
                Dim oldestKey As String = Nothing
                If s_tempOrder.Count > 0 Then
                    oldestKey = s_tempOrder(index:=0)
                End If
                If Not String.IsNullOrEmpty(value:=oldestKey) Then
                    Dim oldBmp As Bitmap = Nothing
                    If s_tempBitmaps.TryGetValue(key:=oldestKey, value:=oldBmp) Then
                        Try
                            oldBmp.Dispose()
                        Catch
                        End Try
                    End If
                    s_tempBitmaps.Remove(key:=oldestKey)
                    s_tempOrder.RemoveAt(index:=0)
                End If
            End If

            s_tempBitmaps(key) = created
            s_tempOrder.Add(item:=key)

            ' Return a clone so the cache owns the stored instance
            Return CType(created.Clone(), Bitmap)
        End SyncLock
    End Function

    ''' <summary>
    ''' Return a new drawable 32bpp ARGB bitmap based on the provided source.
    ''' The returned bitmap is always a new instance and the source is not disposed.
    ''' </summary>
    Friend Function ToDrawableBitmap(src As Bitmap) As Bitmap
        If src Is Nothing Then
            Return Nothing
        End If

        Try
            If src.PixelFormat = Imaging.PixelFormat.Format32bppArgb Then
                Return CType(src.Clone(), Bitmap)
            End If

            Dim bmp As New Bitmap(src.Width,
                                  src.Height,
                                  format:=Imaging.PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.Clear(color:=Color.Transparent)
                g.DrawImage(image:=src,
                            x:=0,
                            y:=0,
                            src.Width,
                            src.Height)
            End Using
            Return bmp
        Catch
            ' Fallback: attempt a simple clone to avoid returning the original.
            Try
                Return CType(src.Clone(), Bitmap)
            Catch
                Return Nothing
            End Try
        End Try
    End Function

    ''' <summary>
    '''  Cleans up all Bitmaps from memory.
    ''' </summary>
    Public Sub CleanUp()
        ' Allow charting/plotting helpers to dispose their module-level caches first.
        Try
            PlotMarkers.CleanUpMarkers()
        Catch
        End Try

        For Each kvp As KeyValuePair(Of String, Bitmap) In s_bitmaps
            kvp.Value?.Dispose()
        Next
        s_bitmaps.Clear()
    End Sub

    ''' <summary>
    '''  Loads all PNG files as Bitmaps into memory.
    ''' </summary>
    Public Sub PreloadBitmaps()
        Dim manifestMap As New Dictionary(Of String, String)(Comparer)

        Try
            Dim projectDataDir As String = GetProjectDataDirectory()
            If Directory.Exists(path:=projectDataDir) Then
                Dim manifestFiles As String() =
                    Directory.GetFiles(path:=projectDataDir,
                                       searchPattern:="*.manifest.json",
                                       searchOption:=SearchOption.TopDirectoryOnly)

                For Each manifestFile As String In manifestFiles
                    Try
                        Dim manifest As IconBundleManifest = Nothing
                        If IconBundleManifest.TryLoadFromFile(path:=manifestFile, manifest:=manifest) AndAlso
                           manifest IsNot Nothing AndAlso
                           Not String.IsNullOrEmpty(value:=manifest.extractedPath) AndAlso
                           manifest.files IsNot Nothing Then

                            For Each relativePath As String In manifest.files
                                Try
                                    Dim normalizedRelativePath As String =
                                        relativePath.Replace(oldChar:="/"c, newChar:=Path.DirectorySeparatorChar)
                                    Dim fullPath As String = Path.Combine(manifest.extractedPath, normalizedRelativePath)
                                    Dim fileNameKey As String = Path.GetFileNameWithoutExtension(path:=normalizedRelativePath)

                                    manifestMap.TryAdd(key:=fileNameKey, value:=fullPath)
                                Catch
                                End Try
                            Next
                        End If
                    Catch
                    End Try
                Next
            End If
        Catch
        End Try

        Dim imagesFolder As String = Path.Combine(Application.StartupPath, "Images")
        Dim enumValues As Array = [Enum].GetValues(Of ImageEnum)()

        For Each enumValue As Object In enumValues
            Dim imageId As ImageEnum = CType(enumValue, ImageEnum)
            Dim key As String = imageId.Description
            Dim enumName As String = imageId.ToString()
            Dim loaded As Boolean = False

            If manifestMap.Count > 0 Then
                Dim selectedPath As String = Nothing

                If manifestMap.TryGetValue(key, value:=selectedPath) OrElse
                   manifestMap.TryGetValue(key:=enumName, value:=selectedPath) Then
                    If File.Exists(path:=selectedPath) Then
                        Try
                            Dim buffer As Byte() = File.ReadAllBytes(path:=selectedPath)
                            Dim bmpLoaded As Bitmap = LoadBitmapFromBytes(buffer:=buffer)
                            If bmpLoaded IsNot Nothing Then
                                ' Normalize to drawable 32bpp and trim transparent borders before caching.
                                Dim drawable As Bitmap = ToDrawableBitmap(src:=bmpLoaded)
                                Try
                                    If drawable IsNot bmpLoaded Then
                                        Try
                                            bmpLoaded.Dispose()
                                        Catch
                                        End Try
                                    End If
                                Catch
                                End Try

                                Dim trimmed As Bitmap = TrimTransparentBorder(src:=drawable)
                                Try
                                    drawable.Dispose()
                                Catch
                                End Try

                                If trimmed IsNot Nothing Then
                                    s_bitmaps(key:=key) = trimmed
                                    loaded = True
                                End If
                            End If
                        Catch
                        End Try
                    End If
                Else
                    For Each pair As KeyValuePair(Of String, String) In manifestMap
                        If (pair.Key.StartsWith(value:=key, comparisonType:=StringComparison.OrdinalIgnoreCase) OrElse
                            pair.Key.StartsWith(value:=enumName, comparisonType:=StringComparison.OrdinalIgnoreCase)) AndAlso
                           File.Exists(path:=pair.Value) Then
                            Try
                                Dim buffer As Byte() = File.ReadAllBytes(path:=pair.Value)
                                Dim bmpLoaded As Bitmap = LoadBitmapFromBytes(buffer:=buffer)
                                If bmpLoaded IsNot Nothing Then
                                    Dim drawable As Bitmap = ToDrawableBitmap(src:=bmpLoaded)
                                    Try
                                        If drawable IsNot bmpLoaded Then
                                            Try
                                                bmpLoaded.Dispose()
                                            Catch
                                            End Try
                                        End If
                                    Catch
                                    End Try

                                    Dim trimmed As Bitmap = TrimTransparentBorder(src:=drawable)
                                    Try
                                        drawable.Dispose()
                                    Catch
                                    End Try

                                    If trimmed IsNot Nothing Then
                                        s_bitmaps(key:=key) = trimmed
                                        loaded = True
                                    End If
                                End If
                            Catch
                            End Try
                            Exit For
                        End If
                    Next
                End If
            End If

            If loaded Then Continue For

            If Directory.Exists(path:=imagesFolder) Then
                Dim fallbackPath As String = Path.Combine(imagesFolder, key & ".png")
                If File.Exists(path:=fallbackPath) Then
                    Try
                        Dim buffer As Byte() = File.ReadAllBytes(path:=fallbackPath)
                        Dim bmpLoaded As Bitmap = LoadBitmapFromBytes(buffer:=buffer)
                        If bmpLoaded IsNot Nothing Then
                            Dim drawable As Bitmap = ToDrawableBitmap(src:=bmpLoaded)
                            Try
                                If drawable IsNot bmpLoaded Then
                                    Try
                                        bmpLoaded.Dispose()
                                    Catch
                                    End Try
                                End If
                            Catch
                            End Try

                            Dim trimmed As Bitmap = TrimTransparentBorder(src:=drawable)
                            Try
                                drawable.Dispose()
                            Catch
                            End Try

                            If trimmed IsNot Nothing Then
                                s_bitmaps(key:=key) = trimmed
                            End If
                        End If
                    Catch
                    End Try
                End If
            End If
        Next
    End Sub

    ''' <summary>
    '''  Gets <see cref="Bitmap"/> from <see cref="s_bitmaps"/> after translating imageId
    '''  to Name and assigns it to PictureBox.Image
    ''' </summary>
    ''' <param name="pictureBox">
    '''  The PictureBox to assign the bitmap to.
    ''' </param>
    ''' <param name="imageId">
    '''  The image ID.
    ''' </param>
    <Extension>
    Public Sub UpdatePictureBox(pictureBox As PictureBox, imageId As ImageEnum)
        ' Dispose previous image to avoid leaking GDI objects
        Try
            Dim prev As Image = pictureBox.Image
            pictureBox.Image = Nothing
            If prev IsNot Nothing Then
                Try
                    prev.Dispose()
                Catch
                End Try
            End If
        Catch
        End Try

        ' Request a bitmap sized to the picture box so drawing overlays align correctly.
        pictureBox.Image =
            GetBitmapFromCache(imageId,
                               canvasSize:=pictureBox.Size)
    End Sub

End Module
