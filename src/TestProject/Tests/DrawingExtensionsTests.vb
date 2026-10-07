' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Drawing
Imports System.Drawing.Imaging
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class DrawingExtensionsTests

    Private ReadOnly _output As ITestOutputHelper

    Public Sub New(output As ITestOutputHelper)
        _output = output
    End Sub

    <Fact>
    Public Sub DrawCenteredArc_DoesNotThrow_ForVariousBitmapSizes()
        ' Try a range of small and typical bitmap sizes to reproduce the Graphics.FromImage/DrawArc failure.
        Dim sizes As Integer() = {4, 6, 8, 12, 16, 24, 32}

        For Each w As Integer In sizes
            For Each h As Integer In sizes
                Using bmp As New Bitmap(width:=w, height:=h, format:=PixelFormat.Format32bppArgb)
                    Try
                        Dim minutes As Integer = 30
                        Dim out As Bitmap = bmp.DrawCenteredArc(minutesToNextCalibration:=minutes)
                        out.Should().NotBeNull()
                    Catch ex As Exception
                        Dim message As String =
                            $"DrawCenteredArc threw for size {w}x{h}: {ex.GetType().FullName} - {ex.Message}"
                        _output.WriteLine(message)
                        ' Re-throw so the test fails and the exception shows up in test results for diagnosis
                        Throw
                    End Try
                End Using
            Next
        Next
    End Sub

End Class
