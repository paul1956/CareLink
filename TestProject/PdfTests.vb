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
            Dim files As String() =
                IO.Directory.GetFiles(path,
                                      searchPattern:="test??.pdf")
            Array.Sort(array:=files)
            Dim selector As Func(Of String, Object()) =
                Function(f)
                    Return New Object() {f}
                End Function
            Return files.Select(selector)
        End Get
    End Property

    Private Shared Function GetTestDataPath(
        <CallerFilePath> Optional path As String = "") As String

        ' Get the currently executing assembly location
        Return IO.Path.Combine(
            IO.Directory.GetParent(path).FullName,
            "TestData")
    End Function

    <Fact>
    Public Sub PdfFileExists()
        If Not Debugger.IsAttached Then
            Return
        End If
        Dim path As String =
            IO.Path.Combine(GetTestDataPath(), "Test01.pdf")
        Dim because As String =
            "The Test01.pdf should exist in TestData directory."

        ' Use the file path in your test
        IO.File.Exists(path).Should().BeTrue(because)
    End Sub

    <Theory>
    <MemberData(NameOf(PdfFiles))>
    Public Sub PdfFilesHaveContent(pdfFilePath As String)
        If Not Debugger.IsAttached Then
            Return
        End If
        Dim path As String =
            IO.Path.GetFileName(path:=pdfFilePath)
        Dim because As String =
            $"File {path} should exist in the TestData directory."
        IO.File.Exists(path:=pdfFilePath) _
               .Should() _
               .BeTrue(because)
        Dim currentPdf As New PdfSettingsRecord(pdfFilePath)
        because =
            $"The PDF settings record for {path} should  " &
            "not be null after loading the file."
        currentPdf.Should().NotBeNull(because)
        because =
            $"The PDF settings record for {path} should " &
            "be valid after loading the file."
        currentPdf.IsValid().Should().BeTrue(because)

        Using dialog As New PumpSetupDialog
            dialog.Pdf = currentPdf
            Dim dialogResult As DialogResult = dialog.ShowDialog()
            because =
                "The dialog result should be OK after " &
                "setting the PDF."
            dialogResult.Should() _
                        .Be(expected:=DialogResult.OK, because)
        End Using
    End Sub

End Class
