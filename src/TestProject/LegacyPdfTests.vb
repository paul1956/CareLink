' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports CareLink
Imports FluentAssertions
Imports Spire.Pdf.Utilities
Imports Xunit

Public Class LegacyPdfTests

    Public Shared ReadOnly Property PdfFiles As IEnumerable(Of Object())
        Get
            Dim path As String = GetTestDataPath()
            Dim files As String() = Directory.GetFiles(path, searchPattern:="test??.pdf")
            Array.Sort(array:=files)
            Dim selector As Func(Of String, Object()) =
                Function(f As String) As Object()
                    Return New Object() {f}
                End Function
            Return files.Select(selector)
        End Get
    End Property

    Private Shared Function GetTestDataPath(<CallerFilePath> Optional path As String = "") As String
        Return IO.Path.Combine(Directory.GetParent(path).FullName, "TestData")
    End Function

    <Theory>
    <MemberData(NameOf(PdfFiles))>
    Public Sub LegacyParserParsesFiles(pdfFilePath As String)
        ' Keep same debugging guard as existing PdfTests
        If Not Debugger.IsAttached Then
            Return
        End If

        File.Exists(path:=pdfFilePath).Should().BeTrue($"The file {pdfFilePath} should exist in TestData.")

        ' Detect V2 (Flex) by checking page 2 for known marker; skip if V2
        Dim page2Text As String = String.Empty
        Try
            page2Text = PDFParserUtilities.ExtractTextFromPage(filename:=pdfFilePath, startPageNumber:=1, endPageNumber:=1)
        Catch
        End Try

        Const comparisonType As StringComparison =
            StringComparison.OrdinalIgnoreCase
        If Not String.IsNullOrEmpty(value:=page2Text) AndAlso
            page2Text.Contains(value:="MiniMed Flex", comparisonType) Then
            ' Skip V2 files - this test file set is intended for Legacy format only
            Return
        End If

        ' Create an empty record instance (avoid invoking file-based constructor)
        Dim record As New PdfSettingsRecord()

        ' Extract tables and text once and pass into the legacy parser
        Dim pageText As String = String.Empty
        Try
            pageText = PDFParserUtilities.ExtractTextFromPage(filename:=pdfFilePath, startPageNumber:=0, endPageNumber:=1)
        Catch
        End Try

        Dim tables As Dictionary(Of String, PdfTable) =
            GetTableList(fileName:=pdfFilePath)
        LegacyPdfParser.ParseLegacy(record, tables, pageText)

        ' Basic assertions to ensure parser produced a usable record
        record.Should().NotBeNull(because:=$"Record should not be null after legacy parsing of {Path.GetFileName(pdfFilePath)}")
        record.IsValid.Should().BeTrue(because:=$"Record should be marked valid for legacy PDF {Path.GetFileName(pdfFilePath)}")
        record.UserName.Should().NotBeNullOrWhiteSpace(because:=$"UserName should be parsed for legacy PDF {Path.GetFileName(pdfFilePath)}")
        record.Basal.Should().NotBeNull(because:=$"Basal section should be present for legacy PDF {Path.GetFileName(pdfFilePath)}")
        record.Bolus.Should().NotBeNull(because:=$"Bolus section should be present for legacy PDF {Path.GetFileName(pdfFilePath)}")
    End Sub

End Class
