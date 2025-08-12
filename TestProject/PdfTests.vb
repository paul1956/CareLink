' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class PdfTests

    Public Shared ReadOnly Property PdfFiles As IEnumerable(Of Object())
        Get
            Dim path As String = GetTestDataPath()
            Dim files As String() = IO.Directory.GetFiles(path, searchPattern:="test??.pdf")
            Array.Sort(array:=files)
            Dim selector As Func(Of String, Object()) =
                Function(f)
                    Return New Object() {f}
                End Function
            Return files.Select(selector)
        End Get
    End Property

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

    <Theory(Skip:="Interactive test - do not run automatically")>
    <MemberData(NameOf(PdfFiles))>
    Public Sub PdfFilesHaveContent(pdfFileNameWithPath As String)
        IO.File.Exists(pdfFileNameWithPath).Should().BeTrue(because:=$"The file {IO.Path.GetFileName(pdfFileNameWithPath)} should exist in the TestData directory.")
        Dim currentPdf As New PdfSettingsRecord(pdfFileNameWithPath)
        currentPdf.Should().NotBeNull(because:=$"The PDF settings record for {IO.Path.GetFileName(pdfFileNameWithPath)} should not be null after loading the file.")
        currentPdf.IsValid().Should().BeTrue(because:=$"The PDF settings record for {IO.Path.GetFileName(pdfFileNameWithPath)} should be valid after loading the file.")

        Using dialog As New PumpSetupDialog
            dialog.Pdf = currentPdf
            Dim dialogResult As DialogResult = dialog.ShowDialog()
            dialogResult.Should().Be(expected:=DialogResult.OK, because:="The dialog result should be OK after setting the PDF.")
        End Using
    End Sub

End Class
