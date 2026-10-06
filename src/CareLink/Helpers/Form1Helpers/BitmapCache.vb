' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

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
    '''  Cleans up all Bitmaps from memory.
    ''' </summary>
    Public Sub CleanUp()
        For Each kvp As KeyValuePair(Of String, Bitmap) In s_bitmaps
            kvp.Value?.Dispose()
        Next
        s_bitmaps.Clear()
    End Sub

    ''' <summary>
    '''  Clears temporary composite cache.
    ''' </summary>
    Public Sub ClearTempCache()
        SyncLock s_tempLock
            For Each kvp As KeyValuePair(Of String, Bitmap) In s_tempBitmaps
                Try
                    kvp.Value.Dispose()
                Catch
                End Try
            Next
            s_tempBitmaps.Clear()
            s_tempOrder.Clear()
        End SyncLock
    End Sub

    ''' <summary>
    '''  Gets <see cref="Bitmap"/> from <see cref="s_bitmaps"/> after translating imageId to Name
    ''' </summary>
    ''' <param name="imageId"><see cref="ImageEnum"/></param>
    ''' <returns>Bitmap from s_bitmaps</returns>
    Public Function GetBitmapFromCache(imageId As ImageEnum) As Bitmap
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=imageId.Description, value) Then
            ' Assign the preloaded Bitmap safely
            Return CType(value.Clone, Bitmap)
        Else
            Return Nothing
        End If

    End Function

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
    Public Sub GetBitmapFromCache(pictureBox As PictureBox, imageId As ImageEnum)
        pictureBox.Image = Nothing
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=imageId.Description, value) Then
            ' Assign the preloaded Bitmap safely
            pictureBox.Image = CType(value.Clone, Bitmap)
        End If
    End Sub

    ''' <summary>
    ''' Get or create a temporary composite bitmap. The caller provides a stable key (includes parameters)
    ''' and a generator Func that creates the Bitmap when missing. The cache size is bounded to avoid memory growth.
    ''' The returned Bitmap is a clone; ownership/disposal rules: caller may dispose the returned image.
    ''' </summary>
    Public Function GetOrCreateTempBitmap(key As String, generator As Func(Of Bitmap)) As Bitmap
        If String.IsNullOrEmpty(value:=key) Then
            Throw New ArgumentNullException(paramName:=NameOf(key))
        End If
        ArgumentNullException.ThrowIfNull(argument:=generator)

        SyncLock s_tempLock
            Dim existing As Bitmap = Nothing
            If s_tempBitmaps.TryGetValue(key, value:=existing) Then
                Return CType(existing.Clone(), Bitmap)
            End If

            ' Create and store the new composite
            Dim created As Bitmap = generator()
            If created Is Nothing Then Return Nothing

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
    '''  Gets a temporary composite Bitmap from the temp cache by key. Returns a clone or Nothing.
    ''' </summary>
    ''' <param name="key">The key for the temporary composite Bitmap.</param>
    ''' <returns>A clone of the Bitmap if found; otherwise, Nothing.</returns>
    Public Function GetTempBitmapFromCache(key As String) As Bitmap
        If String.IsNullOrEmpty(value:=key) Then Return Nothing
        SyncLock s_tempLock
            Dim bmp As Bitmap = Nothing
            If s_tempBitmaps.TryGetValue(key, value:=bmp) Then
                Return CType(bmp.Clone(), Bitmap)
            End If
        End SyncLock
        Return Nothing
    End Function

    ''' <summary>
    '''  Loads all PNG files as Bitmaps into memory.
    ''' </summary>
    Public Sub PreloadBitmaps()
        Dim manifestMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

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

                If manifestMap.TryGetValue(key:=key, value:=selectedPath) OrElse
                   manifestMap.TryGetValue(key:=enumName, value:=selectedPath) Then
                    If File.Exists(path:=selectedPath) Then
                        Try
                            Dim buffer As Byte() = File.ReadAllBytes(path:=selectedPath)
                            Using ms As New MemoryStream(buffer)
                                Dim bmp As Bitmap = DirectCast(Image.FromStream(ms).Clone(), Bitmap)
                                s_bitmaps(key:=key) = bmp
                                loaded = True
                            End Using
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
                                Using ms As New MemoryStream(buffer)
                                    Dim bmp As Bitmap = DirectCast(Image.FromStream(ms).Clone(), Bitmap)
                                    s_bitmaps(key:=key) = bmp
                                    loaded = True
                                End Using
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
                        Using ms As New MemoryStream(buffer)
                            Dim bmp As Bitmap = DirectCast(Image.FromStream(ms).Clone(), Bitmap)
                            s_bitmaps(key:=key) = bmp
                        End Using
                    Catch
                    End Try
                End If
            End If
        Next
    End Sub

    ''' <summary>
    '''  Replace and dispose any existing temp bitmap for the given key.
    '''  Useful when a parameter changes and the old composite is no longer needed.
    ''' </summary>
    ''' <param name="key">The key for the temporary composite Bitmap.</param>
    ''' <param name="newBitmap">
    '''  The new Bitmap to replace the existing one.
    ''' </param>
    Public Sub ReplaceTempBitmap(key As String, newBitmap As Bitmap)
        If String.IsNullOrEmpty(value:=key) Then Return
        SyncLock s_tempLock
            Dim old As Bitmap = Nothing
            If s_tempBitmaps.TryGetValue(key, value:=old) Then
                Try
                    old.Dispose()
                Catch
                End Try
                s_tempBitmaps.Remove(key)
                s_tempOrder.Remove(item:=key)
            End If

            If newBitmap IsNot Nothing Then
                If s_tempBitmaps.Count >= MaxTempBitmaps Then
                    Dim oldestKey As String = Nothing
                    If s_tempOrder.Count > 0 Then
                        oldestKey = s_tempOrder(index:=0)
                    End If
                    If Not String.IsNullOrEmpty(value:=oldestKey) Then
                        Dim oldBmp2 As Bitmap = Nothing
                        If s_tempBitmaps.TryGetValue(key:=oldestKey, value:=oldBmp2) Then
                            Try
                                oldBmp2.Dispose()
                            Catch
                            End Try
                        End If
                        s_tempBitmaps.Remove(key:=oldestKey)
                        s_tempOrder.RemoveAt(index:=0)
                    End If
                End If

                s_tempBitmaps(key) = newBitmap
                s_tempOrder.Add(item:=key)
            End If
        End SyncLock
    End Sub

End Module
