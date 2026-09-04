' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ImageHelper

    ''' <summary>
    '''  Builds a cache key for image composites.
    ''' </summary>
    ''' <param name="imageId">The image ID.</param>
    ''' <param name="targetSize">The target size.</param>
    ''' <param name="currentPercent">The current percentage.</param>
    ''' <returns>The cache key.</returns>
    ''' <param name="brushColor"></param>
    Private Function BuildImageKey(imageId As ImageEnum,
                                   targetSize As Size,
                                   currentPercent As Single,
                                   fillColor As Color) As String
        Dim key As String =
            String.Format(format:="{0}_{1}_{2}_{3}_{4}",
                          imageId.Description, currentPercent, targetSize.Width, targetSize.Height, fillColor.Name)
        Return key
    End Function

    Public Function GetOrCreateComposite(imageId As ImageEnum,
                                         targetSize As Size,
                                         currentPercent As Single,
                                         fillColor As Color) As Bitmap

        Dim key As String = BuildImageKey(imageId,
                                          targetSize,
                                          currentPercent,
                                          fillColor)

        Dim generator As Func(Of Bitmap) =
            Function()
                Dim baseBmp As Bitmap = GetBitmapFromCache(imageId)
                If baseBmp Is Nothing Then
                    Return Nothing
                End If
                Dim composed As Bitmap =
                    CreateImageComposite(baseBmp,
                                         targetSize,
                                         currentPercent,
                                         fillColor)
                baseBmp.Dispose()
                Return composed
            End Function

        Return BitmapCache.GetOrCreateTempBitmap(key, generator)
    End Function

End Module
