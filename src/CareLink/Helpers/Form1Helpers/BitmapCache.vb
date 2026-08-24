' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Module BitmapCache
    ' Storage for the preloaded Bitmaps
    Friend ReadOnly s_bitmaps As New Dictionary(Of String, Bitmap)(comparer:=StringComparer.OrdinalIgnoreCase)

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
    '''  Gets a Bitmap for PNG files stored in Images directory from Cache
    ''' </summary>
    ''' <param name="name">
    '''  The name of the file without the extension.
    ''' </param>
    ''' <returns>Bitmap of file</returns>
    Public Function GetBitmapFromCache(name As String) As Bitmap
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=name, value) Then
            ' Assign the preloaded Bitmap safely
            Return value
        Else
            Return Nothing
        End If
    End Function
End Module
