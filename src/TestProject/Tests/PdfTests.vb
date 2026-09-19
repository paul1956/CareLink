' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class PdfTests

    ''' <summary>
    '''  Gets the list of PDF files in the TestData directory for testing.
    ''' </summary>
    ''' <returns>
    '''  An enumerable of object arrays, each containing the file path
    '''  of a PDF file.
    ''' </returns>
    Public Shared ReadOnly Property PdfFiles As IEnumerable(Of Object())
        Get
            Dim path As String = GetTestDataPath()
            Dim files As String() = Directory.GetFiles(path, searchPattern:="test*.pdf")
            Array.Sort(array:=files)
            Dim selector As Func(Of String, Object()) =
                Function(f As String) As Object()
                    Return New Object() {f}
                End Function
            Return files.Select(selector)
        End Get
    End Property

    ''' <summary>
    '''  Gets the list of PDF V2 files in the TestData directory for testing.
    ''' </summary>
    ''' <returns>
    '''  An enumerable of object arrays, each containing the file path
    '''  of a PDF V2 file.
    ''' </returns>
    Public Shared ReadOnly Property PdfV2Files As IEnumerable(Of Object())
        Get
            Dim path As String = GetTestDataPath()
            Dim files As String() = Directory.GetFiles(path, searchPattern:="testDataV2*.pdf")
            Array.Sort(array:=files)
            Dim selector As Func(Of String, Object()) =
                Function(f As String) As Object()
                    Return New Object() {f}
                End Function
            Return files.Select(selector)
        End Get
    End Property

    <Fact>
    Public Sub PdfFileExists()
        If Not Debugger.IsAttached Then
            Return
        End If
        Dim path As String = IO.Path.Combine(GetTestDataPath(), "Test01.pdf")
        Dim because As String = "The Test01.pdf should exist in TestData directory."

        ' Use the file path in your test
        File.Exists(path).Should().BeTrue(because)
    End Sub

    <Theory>
    <MemberData(NameOf(PdfFiles))>
    Public Sub PdfFilesHaveContent(pdfFilePath As String)
        If Not Debugger.IsAttached Then
            Return
        End If
        Dim path As String = IO.Path.GetFileName(path:=pdfFilePath)
        Dim because As String = $"File {path} should exist in the TestData directory."
        File.Exists(path:=pdfFilePath).Should().BeTrue(because)
        Dim currentPdf As New PdfSettingsRecord(pdfFilePath)
        because = $"The PDF settings record for {path} should  not be null after loading the file."
        currentPdf.Should().NotBeNull(because)
        because = $"The PDF settings record for {path} should be valid after loading the file."
        currentPdf.IsValid().Should().BeTrue(because)

        ' Verify PdfSettingsRecord contents directly (do not show interactive dialog in tests)
        currentPdf.UserName.Should().NotBeNullOrWhiteSpace(because)
        If Not String.IsNullOrEmpty(value:=currentPdf.DeviceFamily) Then
            currentPdf.DeviceFamily.Should().NotBeNullOrWhiteSpace(because)
        End If
        If Not String.IsNullOrEmpty(value:=currentPdf.DeviceModel) Then
            currentPdf.DeviceModel.Should().NotBeNullOrWhiteSpace(because)
        End If

        ' Ensure some key sections were parsed
        currentPdf.IsValid().Should().BeTrue(because)
        currentPdf.Basal.Should().NotBeNull(because)
        currentPdf.Bolus.Should().NotBeNull(because)
    End Sub

    <Theory>
    <MemberData(NameOf(PdfV2Files))>
    Public Sub PdfV2FilesHaveContent(pdfFilePath As String)
        If Not Debugger.IsAttached Then
            Return
        End If
        Dim path As String = IO.Path.GetFileName(path:=pdfFilePath)
        Dim because As String = $"File {path} should exist in the TestData directory."
        File.Exists(path:=pdfFilePath).Should().BeTrue(because)
        Dim currentPdf As New PdfSettingsRecord(pdfFilePath)
        because = $"The PDF settings record for {path} should  not be null after loading the file."
        currentPdf.Should().NotBeNull(because)
        currentPdf.IsValid().Should().BeTrue(because)

        ' Verify PdfSettingsRecord contents directly (do not show interactive dialog in tests)
        because = $"The user name record for {path} should be valid after loading the file."
        currentPdf.UserName.Should().NotBeNullOrWhiteSpace(because)

        because = $"The device family record for {path} should be valid after loading the file."
        currentPdf.DeviceFamily.Should().NotBeNullOrWhiteSpace(because)

        because = $"The device model record for {path} should be valid after loading the file."
        currentPdf.DeviceModel.Should().NotBeNullOrWhiteSpace(because)

        ' Ensure some key sections were parsed
        because = $"The PDF record for {path} should be valid after loading the file."
        currentPdf.IsValid().Should().BeTrue(because)

        because = $"The basal record for {path} should be valid after loading the file."
        currentPdf.Basal.Should().NotBeNull(because)

        because = $"The bolus record for {path} should be valid after loading the file."
        currentPdf.Bolus.Should().NotBeNull(because)

        because = $"The this test should only the Flex base Version 2 PDF Files."
        currentPdf.IsFlex.Should().BeTrue(because)
    End Sub

End Class
