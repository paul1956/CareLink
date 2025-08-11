' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class PdfTests

    Private Shared Function GetTestDataPath(<CallerFilePath> Optional path As String = "") As String
        ' Get the currently executing assembly location
        Return IO.Path.Combine(IO.Directory.GetParent(path).FullName, "TestData")
    End Function

    <Fact>
    Public Sub PdfFileExists()

        Dim path As String = IO.Path.Combine(GetTestDataPath(), "Test01.pdf")

        ' Use the file path in your test
        IO.File.Exists(path).Should().BeTrue(because:="The file Test01.pdf should exist in the TestData directory.")
    End Sub

    <Fact>
    Public Sub PdfFileHasContent()
        Dim pdfFileNameWithPath As String = IO.Path.Combine(GetTestDataPath(), "Test01.pdf")
        ' Check if the file exists
        IO.File.Exists(path:=pdfFileNameWithPath).Should().BeTrue(because:="The file Test01.pdf should exist in the TestData directory.")
        Dim currentPdf As PdfSettingsRecord
        currentPdf = New PdfSettingsRecord(pdfFileNameWithPath)
        currentPdf.Should().NotBeNull(because:="The PDF settings record should not be null after loading the file.")
        currentPdf.IsValid().Should().BeTrue(because:="The PDF settings record should be valid after loading the file.")

        Using dialog As New PumpSetupDialog
            dialog.Pdf = currentPdf
            dialog.ShowDialog()
        End Using

    End Sub

End Class
