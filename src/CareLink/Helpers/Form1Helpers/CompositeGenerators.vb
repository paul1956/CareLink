Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices

Public Module CompositeGenerators

    ''' <summary>
    '''  Finds the bounds of the transparent pixels in a PictureBox's image.
    ''' </summary>
    ''' <param name="pb">The PictureBox containing the image.</param>
    ''' <returns>
    '''  A Rectangle representing the bounds of the transparent pixels,
    '''  or Rectangle.Empty if none found.
    ''' </returns>
    <Extension>
    Private Function FindTransparentBounds(bmp As Bitmap) As Rectangle
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
            Return New Rectangle(x:=minX,
                                 y:=minY,
                                 width:=maxX - minX + 1,
                                 height:=maxY - minY + 1)
        Catch ex As Exception
            Return Rectangle.Empty
        End Try
    End Function

    ''' <summary>
    '''  Creates an image composite by filling the transparent area of the base image
    '''  with a vertical fill representing the battery level in minutes, scaling the
    '''  transparent paint rectangle from the base image to the target size.
    ''' </summary>
    ''' <param name="baseBmp">
    '''  The base bitmap containing the transparent area.
    ''' </param>
    ''' <param name="targetSize">The size of the output bitmap.</param>
    ''' <param name="currentPercent">
    '''  The percentage of the resource available.
    ''' </param>
    ''' <param name="paintRect">
    '''  The rectangle in the base image to fill.
    ''' </param>
    ''' <param name="fillColor">
    '''  The color to use for the fill.
    ''' </param>
    ''' <returns>The created image composite bitmap.</returns>
    Public Function CreateImageComposite(baseBmp As Bitmap,
                                         targetSize As Size,
                                         currentPercent As Single,
                                         fillColor As Color) As Bitmap
        If baseBmp Is Nothing Then
            Return Nothing
        End If
        Dim paintRect As Rectangle = FindTransparentBounds(bmp:=baseBmp)

        Dim outBmp As New Bitmap(targetSize.Width,
                                 targetSize.Height,
                                 format:=Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(outBmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.Clear(color:=Color.Transparent)

            ' Scale paintRect from baseBmp coordinates to targetSize
            Dim scaleX As Single = targetSize.Width / CSng(baseBmp.Width)
            Dim scaleY As Single = targetSize.Height / CSng(baseBmp.Height)

            Dim scaledRect As New RectangleF(x:=paintRect.X * scaleX,
                                             y:=paintRect.Y * scaleY,
                                             width:=paintRect.Width * scaleX,
                                             height:=paintRect.Height * scaleY)

            Dim fillHeight As Single =
                currentPercent / 100.0F * scaledRect.Height
            Dim fillTopY As Single =
                scaledRect.Y + (scaledRect.Height - fillHeight)

            If currentPercent > 0.0F Then
                Using br As New SolidBrush(color:=fillColor)
                    Dim fillRect As New RectangleF(scaledRect.X,
                                                   y:=fillTopY,
                                                   scaledRect.Width,
                                                   height:=fillHeight)
                    g.FillRectangle(brush:=br, rect:=fillRect)
                End Using
            End If

            ' Draw the base image scaled to the target on top (base image contains transparent area)
            g.DrawImage(image:=baseBmp,
                        x:=0,
                        y:=0,
                        targetSize.Width,
                        targetSize.Height)
        End Using

        Return outBmp
    End Function

End Module
