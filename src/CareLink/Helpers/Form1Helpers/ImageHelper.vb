' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ImageHelper

    ' A fill change smaller than this many percentage points is not visible to the user.
    Private Const MinNoticeablePercentChange As Single = 10.0F

    ' A fill change smaller than this many pixels is not visible to the user.
    Private Const MinNoticeablePixelChange As Single = 2.0F

    Private ReadOnly s_compositeLock As New Object()
    Private ReadOnly s_composites As New Dictionary(Of ImageEnum, CompositeEntry)

    ''' <summary>
    '''  Determines whether the cached composite still looks the same as one built for the requested values.
    ''' </summary>
    Private Function IsCacheValid(entry As CompositeEntry,
                                  canvasSize As Size,
                                  currentPercent As Single,
                                  fillColor As Color) As Boolean
        If entry.CanvasSize <> canvasSize OrElse entry.FillColor.ToArgb() <> fillColor.ToArgb() Then
            Return False
        End If

        Dim delta As Single = Math.Abs(value:=entry.Percent - currentPercent)
        If delta = 0.0F Then
            Return True
        End If

        ' Empty and full are always shown accurately.
        If currentPercent <= 0.0F OrElse currentPercent >= 100.0F Then
            Return False
        End If

        Dim pixelChange As Single = delta / 100.0F * canvasSize.Height
        Return delta < MinNoticeablePercentChange OrElse pixelChange < MinNoticeablePixelChange
    End Function

    ''' <summary>
    '''  Disposes all cached composites.
    ''' </summary>
    Public Sub ClearCompositeCache()
        SyncLock s_compositeLock
            For Each entry As CompositeEntry In s_composites.Values
                entry.Bitmap.Dispose()
            Next
            s_composites.Clear()
        End SyncLock
    End Sub

    ''' <summary>
    '''  Gets a composite for <paramref name="imageId"/>. Only one composite is cached per image and it is
    '''  replaced only when the canvas size or fill color changes, or the fill change would be noticeable.
    ''' </summary>
    ''' <returns>A clone the caller may dispose.</returns>
    Public Function GetOrCreateComposite(imageId As ImageEnum,
                                         canvasSize As Size,
                                         currentPercent As Single,
                                         fillColor As Color) As Bitmap

        SyncLock s_compositeLock
            Dim entry As CompositeEntry = Nothing
            If s_composites.TryGetValue(imageId, value:=entry) Then
                If IsCacheValid(entry, canvasSize, currentPercent, fillColor) Then
                    Return CType(entry.Bitmap.Clone(), Bitmap)
                End If
            End If

            Dim baseBmp As Bitmap = GetBitmapFromCache(imageId, canvasSize)
            If baseBmp Is Nothing Then
                Return Nothing
            End If

            Dim composed As Bitmap
            Try
                composed = CreateImageComposite(baseBmp, canvasSize, currentPercent, fillColor)
            Finally
                baseBmp.Dispose()
            End Try
            If composed Is Nothing Then
                Return Nothing
            End If

            entry?.Bitmap.Dispose()
            s_composites(key:=imageId) = New CompositeEntry(composed, canvasSize, currentPercent, fillColor)
            Return CType(composed.Clone(), Bitmap)
        End SyncLock
    End Function

    ''' <summary>
    '''  The single cached composite for an image, with the parameters it was built from.
    ''' </summary>
    Private NotInheritable Class CompositeEntry

        Public Sub New(bitmap As Bitmap, canvasSize As Size, percent As Single, fillColor As Color)
            Me.Bitmap = bitmap
            Me.CanvasSize = canvasSize
            Me.Percent = percent
            Me.FillColor = fillColor
        End Sub

        Public ReadOnly Property Bitmap As Bitmap
        Public ReadOnly Property CanvasSize As Size
        Public ReadOnly Property FillColor As Color
        Public ReadOnly Property Percent As Single
    End Class
End Module
