' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink

Public Module FlexPdfParser

    Public Sub ParseFlex(record As PdfSettingsRecord, pageText As String)
        Dim allLines As List(Of String) = pageText.SplitLines(Trim:=True)
        Try
            record.IsFlex = True
            ' Get user name from the first line of the header text
            Dim firstLine As String = allLines.FirstOrDefault()
            Dim length As Integer = If(String.IsNullOrEmpty(firstLine), -1, firstLine.IndexOf(value:="  "))
            If length > 0 Then
                record.UserName = firstLine.Substring(startIndex:=0, length).Trim()
            End If

            Dim parser As New PdfSectionParser(allLines, record)
            parser.ParseBasal()
            parser.ParseBolus()
            parser.ParseSmartGuardAndReminders()
            parser.ParseAlertsAndUtilities()

            SetIsValid(record, value:=True)
        Catch ex As Exception
            SetIsValid(record, value:=False)
        End Try
    End Sub

End Module
