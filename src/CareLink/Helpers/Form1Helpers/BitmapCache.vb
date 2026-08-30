' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices

Public Module BitmapCache

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
    '''  Gets <see cref="Bitmap"/> from <see cref="s_bitmaps"/> after translating id to Name
    ''' </summary>
    ''' <param name="id"><see cref="ImageEnum"/></param>
    ''' <returns>Bitmap from s_bitmaps</returns>
    Public Function GetBitmapFromCache(id As ImageEnum) As Bitmap
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=id.Description, value) Then
            ' Assign the preloaded Bitmap safely
            Return CType(value.Clone, Bitmap)
        Else
            Return Nothing
        End If

    End Function

    <Extension>
    Public Sub GetBitmapFromCache(pictureBox As PictureBox, id As ImageEnum)
        pictureBox.Image = Nothing
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=id.Description, value) Then
            ' Assign the preloaded Bitmap safely
            pictureBox.Image = CType(value.Clone, Bitmap)
        End If
    End Sub

    ''' <summary>
    '''  Loads all PNG files as Bitmaps into memory.
    ''' </summary>
    Public Sub PreloadBitmaps()
        Dim folderPath As String = Path.Combine(Application.StartupPath, "Images")
        If Not Directory.Exists(path:=folderPath) Then Exit Sub

        Dim files As String() = Directory.GetFiles(path:=folderPath, searchPattern:="*.png")

        For Each filePath As String In files
            Dim fileName As String = Path.GetFileName(path:=filePath).
                                          ReplaceNoCase(oldValue:=".png", newValue:="")
            Try
                Dim buffer As Byte() = File.ReadAllBytes(path:=filePath)

                Using ms As New IO.MemoryStream(buffer)
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

End Module
