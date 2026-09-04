' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ImageHelper

    ''' <summary>
    '''  Gets a Bitmap for PNG files stored in Images directory from Cache
    ''' </summary>
    ''' <param name="name">
    '''  The name of the file without the extension.
    ''' </param>
    ''' <returns>Bitmap of file</returns>
    Private Function GetBitmapFromCache(Name As String) As Bitmap
        Dim value As Bitmap = Nothing
        If s_bitmaps.TryGetValue(key:=Name, value) Then
            ' Assign the preloaded Bitmap safely
            Return CType(value.Clone, Bitmap)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    '''  Finds the bounds of the transparent pixels in a PictureBox's image.
    ''' </summary>
    ''' <param name="pb">The PictureBox containing the image.</param>
    ''' <returns>
    '''  A Rectangle representing the bounds of the transparent pixels,
    '''  or Rectangle.Empty if none found.
    ''' </returns>
    <Extension>
    Public Function FindTransparentBounds(bmp As Bitmap) As Rectangle
        Try
            If bmp Is Nothing Then
                Return Rectangle.Empty
            End If

            Dim w As Integer = bmp.Width
            Dim h As Integer = bmp.Height

            ' Initialize bounds
            Dim minX As Integer = w
            Dim maxX As Integer = 0
            Dim minY As Integer = h
            Dim maxY As Integer = 0

            ' Loop through each pixel
            For y As Integer = 0 To h - 1
                For x As Integer = 0 To w - 1
                    Dim color As Color = bmp.GetPixel(x, y)
                    If color.A = 0 Then ' Fully transparent
                        minX = Math.Min(minX, x)
                        maxX = Math.Max(maxX, x)
                        minY = Math.Min(minY, y)
                        maxY = Math.Max(maxY, y)
                    End If
                Next
            Next

            ' If no transparent pixels found
            If minX = w Then
                MessageBox.Show(text:="No transparent pixels found.")
                Return Rectangle.Empty
            End If

            ' Create a Rectangle from the bounds
            Return New Rectangle(x:=minX, y:=minY, width:=maxX - minX + 1, height:=maxY - minY + 1)
        Catch ex As Exception
            Return Rectangle.Empty
        End Try
    End Function

End Module
