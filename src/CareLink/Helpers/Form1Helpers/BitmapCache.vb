' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Public Module BitmapCache

    Private Const MaxTempBitmaps As Integer = 6

    ' Temporary composite images cache (small, bounded). Keys are application-defined
    ' and should include any parameters that affect rendering (eg: percent, state, size).
    Private ReadOnly s_tempBitmaps As New Dictionary(Of String, Bitmap)(comparer:=StringComparer.OrdinalIgnoreCase)

    Private ReadOnly s_tempLock As New Object()

    Private ReadOnly s_tempOrder As New List(Of String)()

    ' Storage for the preloaded Bitmaps
    Friend ReadOnly s_bitmaps As New Dictionary(Of String, Bitmap)(comparer:=StringComparer.OrdinalIgnoreCase)

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
        Dim folderPath As String = Path.Combine(Application.StartupPath, "Images")
        If Not Directory.Exists(path:=folderPath) Then Exit Sub

        Dim files As String() =
            Directory.GetFiles(path:=folderPath,
                               searchPattern:="*.png",
                               searchOption:=SearchOption.TopDirectoryOnly)

        For Each filePath As String In files
            Dim fileName As String =
                Path.GetFileName(path:=filePath).
                     ReplaceNoCase(oldValue:=".png", newValue:="")
            Try
                Dim buffer As Byte() = File.ReadAllBytes(path:=filePath)

                Using ms As New MemoryStream(buffer)
                    ' Load, clone, and cast directly to a Bitmap
                    Dim bmp As Bitmap =
                        DirectCast(Image.FromStream(ms).Clone(), Bitmap)

                    s_bitmaps(key:=fileName) = bmp
                End Using
            Catch ex As Exception
                ' Handle file errors here
                Stop
            End Try
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
