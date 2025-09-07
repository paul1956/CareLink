' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Spire.Pdf
Imports Spire.Pdf.Texts
Imports Spire.Pdf.Utilities

''' <summary>
'''  Provides utility functions for extracting tables and text from PDF documents
'''  using Spire.PDF.
''' </summary>
Public Module PDFParser

    ''' <summary>
    '''  Extracts the text content from a <see cref="PdfTable"/> and
    '''  returns it as a <see cref="StringTable"/>.
    ''' </summary>
    ''' <param name="table">The PDF table to extract text from.</param>
    ''' <returns>A <see cref="StringTable"/> containing the extracted text.</returns>
    <Extension>
    Friend Function ExtractTableText(table As PdfTable) As StringTable
        'Get row number and column number of a certain table

        Dim builder As New StringTable
        'Loop though the row and column
        For rowIndex As Integer = 0 To table.GetRowCount() - 1
            Dim columns As New List(Of String)
            For columnIndex As Integer = 0 To table.GetColumnCount() - 1
                'Get text from the specific cell
                'Add text to the string builder
                Dim item As String =
                    table.GetText(rowIndex, columnIndex).
                        Replace(oldValue:=vbLf, newValue:=" ")
                columns.Add(item)
            Next
            builder.Rows.Add(item:=New StringTable.Row(columns))
        Next
        Return builder
    End Function

    ''' <summary>
    '''  Converts a <see cref="PdfTable"/> to a <see cref="StringTable"/> and
    '''  validates the header.
    ''' </summary>
    ''' <param name="table">The PDF table to convert.</param>
    ''' <param name="tableHeader">The expected header string for validation.</param>
    ''' <returns>
    '''  A <see cref="StringTable"/> if the header matches;
    '''  otherwise, a new empty <see cref="StringTable"/>.
    ''' </returns>
    <Extension>
    Public Function PdfTableToStringTable(
        table As PdfTable,
        tableHeader As String) As StringTable

        Dim sTable As StringTable = table.ExtractTableText()
        If sTable.IsValid AndAlso
           sTable.Rows(index:=0).Columns(index:=0).StartsWith(value:=tableHeader) Then
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
    ''' <param name="endPageNumber">
    '''  The ending page number (zero-based). If 0, only the start page is used.
    ''' </param>
    ''' <returns>The extracted text from the specified page range.</returns>
    Public Function ExtractTextFromPage(
        filename As String,
        startPageNumber As Integer,
        Optional endPageNumber As Integer = 0) As String

        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load a PDF file
        doc.LoadFromFile(filename)
        Dim text As String = ""
        For i As Integer = startPageNumber To endPageNumber

            'Get the page
            Dim page As PdfPageBase = doc.Pages(index:=i)

            'Create a PdfTextExtractor object
            Dim textExtractor As New PdfTextExtractor(page)

            'Create a PdfTextExtractOptions object
            'Set isExtractAllText to true
            Dim options As New PdfTextExtractOptions With {
                .IsExtractAllText = True
            }
            text &= textExtractor.ExtractText(options)
        Next
        Return text
    End Function

    ''' <summary>
    '''  Creates a <see cref="PdfTableExtractor"/> for the specified PDF file.
    ''' </summary>
    ''' <param name="filename">The path to the PDF file.</param>
    ''' <returns>
    '''  A <see cref="PdfTableExtractor"/> instance for the loaded PDF document.
    ''' </returns>
    Public Function GetPdfExtractor(filename As String) As PdfTableExtractor
        'Create a PdfDocument object
        Dim document As New PdfDocument()

        'Load the PDF file
        document.LoadFromFile(filename)

        'Initialize an instance of PdfTableExtractor class
        Dim extractor As New PdfTableExtractor(document)
        Return extractor
    End Function

    ''' <summary>
    '''  Extracts all tables from a range of pages in a PDF file and
    '''  returns them in a dictionary with descriptive keys.
    ''' </summary>
    ''' <param name="fileName">The path to the PDF file.</param>
    ''' <param name="startPageNumber">The starting page number (zero-based).</param>
    ''' <param name="endPageNumber">
    '''  The ending page number (zero-based). If 0, only the start page is used.
    ''' </param>
    ''' <returns>
    '''  A dictionary mapping descriptive table titles to <see cref="PdfTable"/> objects.
    ''' </returns>
    Public Function GetTableList(
        fileName As String,
        startPageNumber As Integer,
        Optional endPageNumber As Integer = 0) As Dictionary(Of String, PdfTable)

        Dim results As New Dictionary(Of String, PdfTable)
        If Not File.Exists(path:=fileName) Then
            Return results
        End If

        Dim extractor As PdfTableExtractor = GetPdfExtractor(fileName)
        'Declare a PdfTable array
        'Extract tableList from a specific page
        Dim sub24HourTotal As Integer = 0
        Dim subTime As Integer = 0
        Dim subNameRate As Integer = 0
        For i As Integer = startPageNumber To endPageNumber
            For Each table As PdfTable In extractor.ExtractTable(pageIndex:=i).ToList()
                Dim sTable As StringTable = table.ExtractTableText()
                Dim value As String = sTable.Rows(index:=0).Columns(index:=0)
                Dim key As String
                Select Case True
                    Case value.StartsWith(value:="Maximum Basal Rate")
                        key = "Maximum Basal Rate"
                    Case value.StartsWith(value:="24-Hour Total")
                        sub24HourTotal += 1
                        key = $"24 Hour Total({sub24HourTotal})"
                    Case value.StartsWith(value:="Bolus Wizard")
                        key = "Bolus Wizard"
                    Case value.StartsWith(value:="Easy Bolus")
                        key = "Easy Bolus"
                    Case value.StartsWith(value:="Time Ratio")
                        key = "Time Ratio"
                    Case value.StartsWith(value:="Time Sensitivity")
                        key = "Time Sensitivity"
                    Case value.StartsWith(value:="Time Low")
                        key = "Time Low"
                    Case value.StartsWith(value:="Name Normal")
                        key = "Name Normal"
                    Case value.StartsWith(value:="Time U/Hr")
                        subTime += 1
                        key = $"Time U/Hr({subTime})"
                    Case value.StartsWith(value:="Name Rate")
                        subNameRate += 1
                        key = $"Name Rate({subNameRate})"
                    Case value.StartsWith(value:="Sensor")
                        key = "Sensor"
                    Case value.StartsWith(value:="SmartGuard")
                        key = "SmartGuard"
                    Case value.StartsWith(value:="Low Reservoir")
                        key = "Low Reservoir"
                    Case value.StartsWith(value:="Start High")
                        key = "Start High"
                    Case value.StartsWith(value:="Start Low")
                        key = "Start Low"
                    Case value.StartsWith(value:="Auto Calibration")
                        key = "Auto Calibration"
                    Case value.StartsWith(value:="Name Start")
                        key = "Name Start"
                    Case value.StartsWith(value:="Name Time")
                        key = "Name Time"
                    Case value.StartsWith(value:="Calibration Reminder")
                        key = "Calibration Reminder"
                    Case value.StartsWith(value:="Block Mode")
                        key = "Block Mode"
                    Case String.IsNullOrWhiteSpace(value)
                        Continue For
                    Case Else
                        key = value
                End Select
                results.Add(key, value:=table)
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
    Public Function ExtractTextFromRectangleArea(
        fileName As String,
        startPageNumber As Integer,
        x As Integer,
        y As Integer,
        Width As Integer,
        Height As Integer) As String

        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load a PDF file
        doc.LoadFromFile(fileName)

        'Get the second page
        Dim page As PdfPageBase = doc.Pages(index:=startPageNumber)

        'Create a PdfTextExtractor object
        Dim textExtractor As New PdfTextExtractor(page)

        'Create a PdfTextExtractOptions object
        'Set the rectangle area
        Dim extractOptions As New PdfTextExtractOptions With {
            .ExtractArea = New RectangleF(x, y, Width, Height)}

        'Extract text from the rectangle
        Return textExtractor.ExtractText(options:=extractOptions)

    End Function

    ''' <summary>
    '''  Performs a simple text extraction from a specific page in a PDF file.
    ''' </summary>
    ''' <param name="filename">The path to the PDF file.</param>
    ''' <param name="startPageNumber">The page number (zero-based) to extract from.</param>
    ''' <returns>The extracted text from the specified page.</returns>
    Public Function SimpleExtraction(
        filename As String,
        startPageNumber As Integer) As String

        'Create a PdfDocument object
        Dim doc As New PdfDocument()

        'Load a PDF file
        doc.LoadFromFile(filename)

        'Get the first page
        Dim page As PdfPageBase = doc.Pages(index:=startPageNumber)

        'Create a PdfTextExtractor object
        Dim textExtractor As New PdfTextExtractor(page)

        'Create a PdfTextExtractOptions object
        'Set IsSimpleExtraction to true
        Dim extractOptions As New PdfTextExtractOptions With {
            .IsSimpleExtraction = True}

        'Extract text from the selected page
        Return textExtractor.ExtractText(options:=extractOptions)

    End Function
#End If
End Module
