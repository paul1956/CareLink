' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text
Imports Spire.Pdf
Imports Spire.Pdf.Texts
Imports Spire.Pdf.Utilities

Public Module PDFParser

    Friend Function ExtractTableText(table As PdfTable) As String
        'Get row number and column number of a certain table

        'Create a StringBuilder object
        Dim builder As New StringBuilder()
        'Loop though the row and colunm
        For i As Integer = 0 To table.GetRowCount() - 1
            For j As Integer = 0 To table.GetColumnCount() - 1
                'Get text from the specific cell
                'Add text to the string builder
                builder.Append($"{table.GetText(i, j)} ")
            Next
            builder.AppendLine()
        Next
        Return builder.ToString()
    End Function

    Public Function ExtractPdfTableLines(table As PdfTable, tableHeader As String) As List(Of String)
        Dim txt As String = ExtractTableText(table)
        Return If(txt.StartsWith(tableHeader) OrElse tableHeader = "", txt.SplitLines(), New List(Of String))
    End Function

    Public Function ExtractPdfTableLines(tableList As List(Of PdfTable), tableHeader As String) As List(Of String)
        'Determine if the table list is null
        Dim lines As New List(Of String)
        If tableList.Count > 0 Then
            'Loop through the table in the list
            For Each t As PdfTable In tableList
                lines = ExtractPdfTableLines(t, tableHeader)
                If lines IsNot Nothing Then
                    Return lines
                End If
            Next
        End If
        Return lines
    End Function

    Public Function GetPdfExtractor(filename As String) As PdfTableExtractor
        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load the sample PDF file
        doc.LoadFromFile(filename)

        'Initialize an instance of PdfTableExtractor class
        Dim extractor As New PdfTableExtractor(doc)
        Return extractor
    End Function

    Public Function GetTableList(filename As String, startPageNumber As Integer, Optional endPageNumber As Integer = 0) As List(Of PdfTable)
        Dim extractor As PdfTableExtractor = GetPdfExtractor(filename)
        'Declare a PdfTable array
        'Extract tableList from a specific page
        Dim results As New List(Of PdfTable)
        For i As Integer = startPageNumber To endPageNumber
            results.AddRange(extractor.ExtractTable(i).ToList)
        Next
        Return results
    End Function

    Public Function ExtractTextFromPage(filename As String, startPageNumber As Integer, Optional endPageNumber As Integer = 0) As String
        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load a PDF file
        doc.LoadFromFile(filename)
        Dim text As String = ""
        For i As Integer = startPageNumber To endPageNumber

            'Get the page
            Dim page As PdfPageBase = doc.Pages(i)

            'Create a PdfTextExtractor object
            Dim textExtractor As New PdfTextExtractor(page)

            'Create a PdfTextExtractOptions object
            'Set isExtractAllText to true
            Dim extractOptions As New PdfTextExtractOptions With {
                .IsExtractAllText = True
            }
            text &= textExtractor.ExtractText(extractOptions)
        Next
        Return text
    End Function

    'Public Function ExtractTextFromRectangleArea(fileName As String, startPageNumber As Integer, x As Integer, y As Integer, Width As Integer, Height As Integer) As String
    '    'Create a PdfDocument object
    '    Dim doc As New PdfDocument()

    '    'Load a PDF file
    '    doc.LoadFromFile(fileName)

    '    'Get the second page
    '    Dim page As PdfPageBase = doc.Pages(startPageNumber)

    '    'Create a PdfTextExtractor object
    '    Dim textExtractor As New PdfTextExtractor(page)

    '    'Create a PdfTextExtractOptions object
    '    'Set the rectangle area
    '    Dim extractOptions As New PdfTextExtractOptions With {
    '        .ExtractArea = New RectangleF(x, y, Width, Height)
    '    }

    '    'Extract text from the rectangle
    '    Return textExtractor.ExtractText(extractOptions)

    'End Function

    'Public Function SimpleExtraction(filename As String, startPageNumber As Integer) As String
    '    'Create a PdfDocument object
    '    Dim doc As New PdfDocument()

    '    'Load a PDF file
    '    doc.LoadFromFile(filename)

    '    'Get the first page
    '    Dim page As PdfPageBase = doc.Pages(startPageNumber)

    '    'Create a PdfTextExtractor object
    '    Dim textExtractor As New PdfTextExtractor(page)

    '    'Create a PdfTextExtractOptions object
    '    'Set IsSimpleExtraction to true
    '    Dim extractOptions As New PdfTextExtractOptions With {
    '        .IsSimpleExtraction = True
    '    }

    '    'Extract text from the selected page
    '    Return textExtractor.ExtractText(extractOptions)

    'End Function

End Module
