' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports Spire.Pdf
Imports Spire.Pdf.Texts
Imports Spire.Pdf.Utilities

''' <summary>
'''  Provides utility functions for extracting tables and text from PDF documents using Spire.PDF.
''' </summary>
Public Module PDFParser

    ''' <summary>
    '''  Extracts the text content from a <see cref="PdfTable"/> and returns it as a <see cref="StringTable"/>.
    ''' </summary>
    ''' <param name="table">The PDF table to extract text from.</param>
    ''' <returns>A <see cref="StringTable"/> containing the extracted text.</returns>
    Friend Function ExtractTableText(table As PdfTable) As StringTable
        'Get row number and column number of a certain table

        Dim builder As New StringTable
        'Loop though the row and column
        For i As Integer = 0 To table.GetRowCount() - 1
            Dim columns As New List(Of String)
            For j As Integer = 0 To table.GetColumnCount() - 1
                'Get text from the specific cell
                'Add text to the string builder
                columns.Add(table.GetText(i, j).Replace(vbLf, " "))
            Next
            builder.Rows.Add(New StringTable.Row(columns))
        Next
        Return builder
    End Function

    ''' <summary>
    '''  Converts a <see cref="PdfTable"/> to a <see cref="StringTable"/> and validates the header.
    ''' </summary>
    ''' <param name="table">The PDF table to convert.</param>
    ''' <param name="tableHeader">The expected header string for validation.</param>
    ''' <returns>
    '''  A <see cref="StringTable"/> if the header matches; otherwise, a new empty <see cref="StringTable"/>.
    ''' </returns>
    Public Function ConvertPdfTableToStringTable(table As PdfTable, tableHeader As String) As StringTable
        Dim sTable As StringTable = ExtractTableText(table)
        If sTable.IsValid AndAlso sTable.Rows(0).Columns(0).StartsWith(tableHeader) Then
            Return sTable
        Else
            Stop
            Return New StringTable
        End If
    End Function

    ''' <summary>
    '''  Extracts text from a range of pages in a PDF file.
    ''' </summary>
    ''' <param name="filename">The path to the PDF file.</param>
    ''' <param name="startPageNumber">The starting page number (zero-based).</param>
    ''' <param name="endPageNumber">The ending page number (zero-based). If 0, only the start page is used.</param>
    ''' <returns>The extracted text from the specified page range.</returns>
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

    ''' <summary>
    '''  Creates a <see cref="PdfTableExtractor"/> for the specified PDF file.
    ''' </summary>
    ''' <param name="filename">The path to the PDF file.</param>
    ''' <returns>A <see cref="PdfTableExtractor"/> instance for the loaded PDF document.</returns>
    Public Function GetPdfExtractor(filename As String) As PdfTableExtractor
        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load the sample PDF file
        doc.LoadFromFile(filename)

        'Initialize an instance of PdfTableExtractor class
        Dim extractor As New PdfTableExtractor(doc)
        Return extractor
    End Function

    ''' <summary>
    '''  Extracts all tables from a range of pages in a PDF file and returns them in a dictionary with descriptive keys.
    ''' </summary>
    ''' <param name="filename">The path to the PDF file.</param>
    ''' <param name="startPageNumber">The starting page number (zero-based).</param>
    ''' <param name="endPageNumber">The ending page number (zero-based). If 0, only the start page is used.</param>
    ''' <returns>
    '''  A dictionary mapping descriptive table titles to <see cref="PdfTable"/> objects.
    ''' </returns>
    Public Function GetTableList(filename As String, startPageNumber As Integer, Optional endPageNumber As Integer = 0) As Dictionary(Of String, PdfTable)
        Dim results As New Dictionary(Of String, PdfTable)
        If Not File.Exists(filename) Then
            Return results
        End If

        Dim extractor As PdfTableExtractor = GetPdfExtractor(filename)
        'Declare a PdfTable array
        'Extract tableList from a specific page
        Dim sub24HourTotal As Integer = 0
        Dim subTime As Integer = 0
        Dim subNameRate As Integer = 0
        For i As Integer = startPageNumber To endPageNumber
            For Each table As PdfTable In extractor.ExtractTable(i).ToList()
                Dim sTable As StringTable = ExtractTableText(table)
                Dim item As String = sTable.Rows(0).Columns(0)
                Dim itemTitle As String
                Select Case True
                    Case item.StartsWith("Maximum Basal Rate")
                        itemTitle = "Maximum Basal Rate"
                    Case item.StartsWith("24-Hour Total")
                        sub24HourTotal += 1
                        itemTitle = $"24 Hour Total({sub24HourTotal})"
                    Case item.StartsWith("Bolus Wizard")
                        itemTitle = "Bolus Wizard"
                    Case item.StartsWith("Easy Bolus")
                        itemTitle = "Easy Bolus"
                    Case item.StartsWith("Time Ratio")
                        itemTitle = "Time Ratio"
                    Case item.StartsWith("Time Sensitivity")
                        itemTitle = "Time Sensitivity"
                    Case item.StartsWith("Time Low")
                        itemTitle = "Time Low"
                    Case item.StartsWith("Name Normal")
                        itemTitle = "Name Normal"
                    Case item.StartsWith("Time U/Hr")
                        subTime += 1
                        itemTitle = $"Time U/Hr({subTime})"
                    Case item.StartsWith("Name Rate")
                        subNameRate += 1
                        itemTitle = $"Name Rate({subNameRate})"
                    Case item.StartsWith("Sensor")
                        itemTitle = "Sensor"
                    Case item.StartsWith("SmartGuard")
                        itemTitle = "SmartGuard"
                    Case item.StartsWith("Low Reservoir")
                        itemTitle = "Low Reservoir"
                    Case item.StartsWith("Start High")
                        itemTitle = "Start High"
                    Case item.StartsWith("Start Low")
                        itemTitle = "Start Low"
                    Case item.StartsWith("Auto Calibration")
                        itemTitle = "Auto Calibration"
                    Case item.StartsWith("Name Start")
                        itemTitle = "Name Start"
                    Case item.StartsWith("Name Time")
                        itemTitle = "Name Time"
                    Case item.StartsWith("Calibration Reminder")
                        itemTitle = "Calibration Reminder"
                    Case item.StartsWith("Block Mode")
                        itemTitle = "Block Mode"
                    Case String.IsNullOrWhiteSpace(item)
                        Continue For
                    Case Else
                        itemTitle = item
                End Select
                results.Add(itemTitle, table)
            Next
        Next
        Return results
    End Function

#If False Then

    ''' <summary>
    '''  Extracts text from a specified rectangular area on a page in a PDF file.
    ''' </summary>
    ''' <param name="fileName">The path to the PDF file.</param>
    ''' <param name="startPageNumber">The page number (zero-based) to extract from.</param>
    ''' <param name="x">The X coordinate of the rectangle.</param>
    ''' <param name="y">The Y coordinate of the rectangle.</param>
    ''' <param name="Width">The width of the rectangle.</param>
    ''' <param name="Height">The height of the rectangle.</param>
    ''' <returns>The extracted text from the specified rectangle area.</returns>
    Public Function ExtractTextFromRectangleArea(fileName As String, startPageNumber As Integer, x As Integer, y As Integer, Width As Integer, Height As Integer) As String
        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load a PDF file
        doc.LoadFromFile(fileName)

        'Get the second page
        Dim page As PdfPageBase = doc.Pages(startPageNumber)

        'Create a PdfTextExtractor object
        Dim textExtractor As New PdfTextExtractor(page)

        'Create a PdfTextExtractOptions object
        'Set the rectangle area
        Dim extractOptions As New PdfTextExtractOptions With {.ExtractArea = New RectangleF(x, y, Width, Height)}

        'Extract text from the rectangle
        Return textExtractor.ExtractText(extractOptions)

    End Function

    ''' <summary>
    '''  Performs a simple text extraction from a specific page in a PDF file.
    ''' </summary>
    ''' <param name="filename">The path to the PDF file.</param>
    ''' <param name="startPageNumber">The page number (zero-based) to extract from.</param>
    ''' <returns>The extracted text from the specified page.</returns>
    Public Function SimpleExtraction(filename As String, startPageNumber As Integer) As String
        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load a PDF file
        doc.LoadFromFile(filename)

        'Get the first page
        Dim page As PdfPageBase = doc.Pages(startPageNumber)

        'Create a PdfTextExtractor object
        Dim textExtractor As New PdfTextExtractor(page)

        'Create a PdfTextExtractOptions object
        'Set IsSimpleExtraction to true
        Dim extractOptions As New PdfTextExtractOptions With {.IsSimpleExtraction = True}

        'Extract text from the selected page
        Return textExtractor.ExtractText(extractOptions)

    End Function
#End If
End Module
