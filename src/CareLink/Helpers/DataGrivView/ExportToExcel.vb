' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports ClosedXML.Excel

Module ExportToExcel

    Public Sub ExportToExcelWithFormatting(dgv As DataGridView, baseFileName As String)
        Dim saveFileDialog1 As New SaveFileDialog With {
            .Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
            .Title = "To Excel",
            .FileName = $"{baseFileName} ({Date.Now:yyyy-MM-dd})"
        }

        If saveFileDialog1.ShowDialog() = Global.System.Windows.Forms.DialogResult.OK Then
            Dim workbook As New XLWorkbook()
            Dim worksheet As IXLWorksheet = workbook.Worksheets.Add(baseFileName)
            For i As Integer = 0 To dgv.Columns.Count - 1
                worksheet.Cell(1, i + 1).Value = dgv.Columns(i).Name
            Next i

            For i As Integer = 0 To dgv.Rows.Count - 1
                For j As Integer = 0 To dgv.Columns.Count - 1
                    worksheet.Cell(i + 2, j + 1).Value = dgv.Rows(i).Cells(j).Value.ToString()

                    If worksheet.Cell(i + 2, j + 1).Value.ToString().Length > 0 Then
                        Dim align As XLAlignmentHorizontalValues

                        Select Case dgv.Rows(i).Cells(j).Style.Alignment
                            Case DataGridViewContentAlignment.BottomRight
                                align = XLAlignmentHorizontalValues.Right
                            Case DataGridViewContentAlignment.MiddleRight
                                align = XLAlignmentHorizontalValues.Right
                            Case DataGridViewContentAlignment.TopRight
                                align = XLAlignmentHorizontalValues.Right

                            Case DataGridViewContentAlignment.BottomCenter
                                align = XLAlignmentHorizontalValues.Center
                            Case DataGridViewContentAlignment.MiddleCenter
                                align = XLAlignmentHorizontalValues.Center
                            Case DataGridViewContentAlignment.TopCenter
                                align = XLAlignmentHorizontalValues.Center

                            Case Else
                                align = XLAlignmentHorizontalValues.Left
                        End Select

                        worksheet.Cell(i + 2, j + 1).Style.Alignment.Horizontal = align

                        Dim xlColor As XLColor = XLColor.FromColor(dgv.Rows(i).Cells(j).Style.SelectionBackColor)
                        worksheet.Cell(i + 2, j + 1).AddConditionalFormat().WhenLessThan(1).Fill.SetBackgroundColor(xlColor)

                        worksheet.Cell(i + 2, j + 1).Style.Font.FontName = dgv.Font.Name
                        worksheet.Cell(i + 2, j + 1).Style.Font.FontSize = dgv.Font.Size

                    End If
                Next j
            Next i
            worksheet.Columns().AdjustToContents()
            workbook.SaveAs(saveFileDialog1.FileName)
        End If
    End Sub

End Module
