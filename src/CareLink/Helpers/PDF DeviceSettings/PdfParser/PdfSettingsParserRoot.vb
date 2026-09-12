' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Reflection
Imports DocumentFormat.OpenXml.EMMA
Imports Spire.Pdf.Utilities

Public Module PdfSettingsParserRoot

    ''' <summary>
    '''  Top-level parser facade that dispatches to format-specific parsers
    '''  and populates a PdfSettingsRecord instance.
    ''' </summary>
    Friend Sub Parse(record As PdfSettingsRecord, pdfFilePath As String)
        If Not File.Exists(path:=pdfFilePath) Then
            Throw New FileNotFoundException(message:="PDF file not found", fileName:=pdfFilePath)
        End If

        ' Try Flex (page 2) detection first
        Dim pageText As String = String.Empty
        Try
            pageText = ExtractTextFromPage(filename:=pdfFilePath,
                                                              startPageNumber:=0,
                                                              endPageNumber:=1)
        Catch
        End Try

        Const comparisonType As StringComparison =
            StringComparison.OrdinalIgnoreCase
        Dim tables As Dictionary(Of String, PdfTable) =
            GetTableList(fileName:=pdfFilePath)
        If String.IsNullOrEmpty(value:=pageText) Then
            Return
        End If

        Dim deviceFamilyAndModel As PdfDeviceInfo =
            GetDeviceFamilyAndModel(pageText)
        record.DeviceModel = deviceFamilyAndModel.Model
        record.DeviceFamily = deviceFamilyAndModel.Family

        If pageText.Contains(value:="MiniMed Flex", comparisonType) Then
            ParseFlex(record, pageText)
        Else
            ParseLegacy(record, tables, pageText)
        End If
    End Sub

    Friend Sub SetIsValid(record As PdfSettingsRecord, value As Boolean)
        Const bindingAttr As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance
        Dim f As FieldInfo = record.GetType().GetField(name:="_isValid", bindingAttr)
        If f IsNot Nothing Then f.SetValue(obj:=record, value)
    End Sub

End Module
