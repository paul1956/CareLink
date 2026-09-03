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
                MessageBox.Show("No transparent pixels found.")
                Return Rectangle.Empty
            End If

            ' Create a Rectangle from the bounds
            Return New Rectangle(x:=minX, y:=minY, width:=maxX - minX + 1, height:=maxY - minY + 1)
        Catch ex As Exception
            Return Rectangle.Empty
        End Try
    End Function

    ''' <summary>
    '''  Merge 2 images into a PictureBox
    ''' </summary>
    ''' <param name="baseImage"></param>
    ''' <param name="overlayImage"></param>
    Public Sub OverlayTransparentImages(pictureBox As PictureBox, baseImageName As String, overlayImageName As String)
        Dim name As String = $"{baseImageName}:{overlayImageName}"
        Dim mergedImage As Bitmap = GetBitmapFromCache(name)

        If mergedImage Is Nothing Then
            ' 2. Create a new blank bitmap matching the dimensions of the base image
            mergedImage = New Bitmap(pictureBox.Width,
                                     pictureBox.Height)
            Dim baseImage As Bitmap
            Dim overlayImage As Bitmap
            baseImage = GetBitmapFromCache(Name:=baseImageName)
            overlayImage = GetBitmapFromCache(Name:=overlayImageName)

            ' 3. Create a Graphics object to draw onto the new blank bitmap
            Using g As Graphics = Graphics.FromImage(mergedImage)
                Try
                    ' (Optional) Set high quality rendering options
                    g.CompositingMode = Drawing2D.CompositingMode.SourceOver
                    g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                    g.InterpolationMode =
                    Drawing2D.InterpolationMode.HighQualityBicubic

                    ' 4. Draw the base image first
                    Dim width As Integer = pictureBox.Width
                    Dim halfWidth As Integer = width \ 2
                    Dim height As Integer = pictureBox.Height
                    Dim x As Single = 30
                    Dim y As Single = 0
                    g.DrawImage(image:=baseImage, x, y, baseImage.Width, height)

                    ' 5. Draw the transparent overlay image on top
                    ' Center on the left half
                    x = Math.Max(0, width - overlayImage.Width)
                    y = Math.Max(0, (height - overlayImage.Height) \ 2)
                    g.DrawImage(image:=overlayImage, x, y, overlayImage.Width, overlayImage.Height)
                Catch ex As Exception
                    Stop
                End Try
            End Using ' The Graphics object is safely disposed of here
            ' 6. Free up individual image resources if they are no longer needed
            s_bitmaps(key:=name) = mergedImage
            baseImage.Dispose()
            overlayImage.Dispose()
        End If

        ' 7. Clean up old resources in the PictureBox to prevent memory leaks
        pictureBox.Image = Nothing
        pictureBox.Image?.Dispose()

        ' 8. Assign the new combined image to your WinForms PictureBox

        pictureBox.Image = mergedImage

    End Sub

End Module
