' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text
Imports ClosedXML.Excel

Friend Module ExportDataGridView

    <Extension>
    Public Sub CopyToClipboard(dgv As DataGridView, copyHeader As Boolean)

        Dim clipboard_string As New StringBuilder()
        If copyHeader Then
            For i As Integer = 0 To dgv.Columns.Count - 1
                Dim dgvColumn As DataGridViewColumn = dgv.Columns(i)
                If dgvColumn.Visible Then
                    Dim value As String = dgvColumn.Name
                    If i = (dgv.Columns.Count - 1) Then
                        clipboard_string.Append($"{value}{s_environmentNewLine}")
                    Else
                        clipboard_string.Append($"{value}{vbTab}")
                    End If
                End If
            Next i
        End If
        For Each row As DataGridViewRow In dgv.Rows
            For i As Integer = 0 To row.Cells.Count - 1
                If Not dgv.Columns(i).Visible Then Continue For
                Dim value As String = row.Cells(i).Value.ToString
                If i = (row.Cells.Count - 1) Then
                    clipboard_string.Append($"{value}{s_environmentNewLine}")
                Else
                    clipboard_string.Append($"{value}{vbTab}")
                End If
            Next i
        Next row

        Clipboard.SetText(clipboard_string.ToString())
    End Sub

    <Extension>
    Public Sub ExportToExcelWithFormatting(dgv As DataGridView)
        Dim baseFileName As String = dgv.Name.Replace("dgv", "")
        Dim saveFileDialog1 As New SaveFileDialog With {
            .Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
            .Title = "To Excel",
            .FileName = $"{baseFileName} ({Date.Now:yyyy-MM-dd})"
        }
        Dim excelColumn As Integer = 1

        If saveFileDialog1.ShowDialog() = Global.System.Windows.Forms.DialogResult.OK Then
            Dim workbook As New XLWorkbook()
            Dim worksheet As IXLWorksheet = workbook.Worksheets.Add(baseFileName)
            For i As Integer = 0 To dgv.Columns.Count - 1
                Dim dgvColumn As DataGridViewColumn = dgv.Columns(i)
                If dgvColumn.Visible Then
                    worksheet.Cell(1, excelColumn).Value = dgvColumn.Name
                    worksheet.Cell(1, excelColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    excelColumn += 1
                End If
            Next i

            For i As Integer = 0 To dgv.Rows.Count - 1
                excelColumn = 1
                For j As Integer = 0 To dgv.Columns.Count - 1
                    If Not dgv.Columns(j).Visible Then Continue For
                    Dim dgvCell As DataGridViewCell = dgv.Rows(i).Cells(j)
                    Dim value As Object = dgvCell.Value

                    If value.ToString().Length = 0 Then
                        worksheet.Cell(i + 2, excelColumn).Value = ""
                    Else
                        Dim align As XLAlignmentHorizontalValues
                        Select Case dgvCell.ValueType.Name
                            Case NameOf([Int32])
                                align = XLAlignmentHorizontalValues.Right
                                worksheet.Cell(i + 2, excelColumn).Value = CType(value, Integer)
                            Case NameOf([Single])
                                align = XLAlignmentHorizontalValues.Right
                                worksheet.Cell(i + 2, excelColumn).Value = value.ToString.ParseSingle()
                            Case NameOf([Double])
                                align = XLAlignmentHorizontalValues.Right
                                worksheet.Cell(i + 2, excelColumn).Value = value.ToString.ParseSingle()
                            Case NameOf([Decimal])
                                align = XLAlignmentHorizontalValues.Right
                                worksheet.Cell(i + 2, excelColumn).Value = value.ToString.ParseSingle()
                            Case NameOf([Boolean])
                                align = XLAlignmentHorizontalValues.Center
                                worksheet.Cell(i + 2, excelColumn).Value = CType(value, Boolean)
                            Case NameOf([String])
                                align = XLAlignmentHorizontalValues.Left
                                worksheet.Cell(i + 2, excelColumn).Value = value.ToString
                            Case NameOf([DateTime])
                                align = XLAlignmentHorizontalValues.Left
                                worksheet.Cell(i + 2, excelColumn).Value = value.ToString
                            Case Else
                                Stop
                                align = XLAlignmentHorizontalValues.Left
                                worksheet.Cell(i + 2, excelColumn).Value = value.ToString
                        End Select
                        worksheet.Cell(i + 2, excelColumn).Style.Alignment.Horizontal = align
                        Dim xlColor As XLColor = XLColor.FromColor(dgvCell.Style.BackColor)
                        worksheet.Cell(i + 2, excelColumn).AddConditionalFormat().WhenLessThan(1).Fill.SetBackgroundColor(xlColor)
                        worksheet.Cell(i + 2, excelColumn).Style.Font.FontName = dgv.Font.Name
                        worksheet.Cell(i + 2, excelColumn).Style.Font.FontSize = dgv.Font.Size

                    End If
                    excelColumn += 1
                Next j
            Next i
            worksheet.Columns().AdjustToContents()
            workbook.SaveAs(saveFileDialog1.FileName)
        End If
    End Sub

End Module
