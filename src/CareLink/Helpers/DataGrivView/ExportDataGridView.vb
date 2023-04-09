' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
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
        Dim baseFileName As String = dgv.Name.Replace("dgv", "", StringComparison.CurrentCultureIgnoreCase)
        Dim saveFileDialog1 As New SaveFileDialog With {
                .CheckPathExists = True,
                .FileName = $"{baseFileName} ({Date.Now:yyyy-MM-dd})",
                .Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                .InitialDirectory = GetDirectoryForProjectData(),
                .OverwritePrompt = True,
                .Title = "To Excel"
               }

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim workbook As New XLWorkbook()
            Dim worksheet As IXLWorksheet = workbook.Worksheets.Add(baseFileName)
            Dim excelColumn As Integer = 1
            For j As Integer = 0 To dgv.Columns.Count - 1
                Dim dgvColumn As DataGridViewColumn = dgv.Columns(j)
                If dgvColumn.Visible Then
                    If dgvColumn.Name.Equals("dateTime", StringComparison.OrdinalIgnoreCase) Then
                        worksheet.Cell(1, excelColumn).Value = "Date"
                        worksheet.Cell(1, excelColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        excelColumn += 1
                        worksheet.Cell(1, excelColumn).Value = "Time"
                    Else
                        worksheet.Cell(1, excelColumn).Value = dgvColumn.HeaderCell.Value.ToString
                    End If
                    worksheet.Cell(1, excelColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    excelColumn += 1
                End If
            Next j

            For i As Integer = 0 To dgv.Rows.Count - 1
                excelColumn = 1
                For j As Integer = 0 To dgv.Columns.Count - 1
                    If Not dgv.Columns(j).Visible Then Continue For
                    Dim dgvCell As DataGridViewCell = dgv.Rows(i).Cells(j)
                    Dim value As Object = dgvCell.Value
                    Dim valueAsString As String = value?.ToString
                    If String.IsNullOrWhiteSpace(valueAsString) Then
                        worksheet.Cell(i + 2, excelColumn).Value = ""
                        With worksheet.Cell(i + 2, excelColumn).Style
                            Dim cellStyle As DataGridViewCellStyle = dgvCell.GetFormattedStyle()
                            .Fill.SetBackgroundColor(XLColor.FromColor(cellStyle.BackColor))
                            .Font.SetFontColor(XLColor.FromColor(cellStyle.ForeColor))
                            .Font.Bold = cellStyle.Font.Bold
                            .Font.FontName = dgv.Font.Name
                            .Font.FontSize = dgv.Font.Size
                        End With
                    Else
                        Dim align As XLAlignmentHorizontalValues
                        With worksheet.Cell(i + 2, excelColumn)
                            Select Case dgvCell.ValueType.Name
                                Case NameOf([Int32])
                                    align = If(dgv.Columns(j).Name.Equals("RecordNumber", StringComparison.OrdinalIgnoreCase),
                                                    XLAlignmentHorizontalValues.Center,
                                                    XLAlignmentHorizontalValues.Right)
                                    .Value = CInt(value)
                                Case NameOf(OADate)
                                    align = XLAlignmentHorizontalValues.Left
                                    Dim result As Double
                                    If Double.TryParse(valueAsString, result) Then
                                        .Value = result
                                        .Style.NumberFormat.Format = $"0.{Strings.StrDup(37, "0"c)}"
                                    Else
                                        .Value = $"'{valueAsString}"
                                    End If
                                Case NameOf([Decimal]), NameOf([Double]), NameOf([Single])
                                    align = XLAlignmentHorizontalValues.Right
                                    Dim decimalDigits As Integer = If(valueAsString.Contains("."c), 3, 0)

                                    Dim singleValue As Single = ParseSingle(value, decimalDigits)
                                    If Single.IsNaN(singleValue) Then
                                        .Value = "'Infinity"
                                        align = XLAlignmentHorizontalValues.Center
                                    Else
                                        .Value = singleValue
                                        .Style.NumberFormat.Format = If(valueAsString.Contains("."c), "0.000", "0")
                                        align = XLAlignmentHorizontalValues.Right
                                    End If
                                Case NameOf([Boolean])
                                    align = XLAlignmentHorizontalValues.Center
                                    .Value = CBool(value)
                                Case NameOf([String])
                                    align = XLAlignmentHorizontalValues.Left
                                    .Value = valueAsString
                                Case NameOf([DateTime])
                                    .Value = CDate(value).Date
                                    With .Style
                                        Dim cellStyle As DataGridViewCellStyle = dgvCell.GetFormattedStyle()
                                        .Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                                        .Fill.SetBackgroundColor(XLColor.FromColor(cellStyle.BackColor))
                                        .Font.SetFontColor(XLColor.FromColor(cellStyle.ForeColor))
                                        .Font.Bold = cellStyle.Font.Bold
                                        .Font.FontName = dgv.Font.Name
                                        .Font.FontSize = dgv.Font.Size
                                    End With
                                    excelColumn += 1
                                    align = XLAlignmentHorizontalValues.Right
                                    worksheet.Cell(i + 2, excelColumn).Value = CDate(value).TimeOfDay
                                Case Else
                                    Stop
                                    align = XLAlignmentHorizontalValues.Left
                                    .Value = valueAsString
                            End Select
                        End With

                        With worksheet.Cell(i + 2, excelColumn).Style
                            Dim cellStyle As DataGridViewCellStyle = dgvCell.GetFormattedStyle()
                            .Alignment.Horizontal = align
                            .Fill.SetBackgroundColor(XLColor.FromColor(cellStyle.BackColor))
                            .Font.SetFontColor(XLColor.FromColor(cellStyle.ForeColor))
                            .Font.Bold = cellStyle.Font.Bold
                            .Font.FontName = dgv.Font.Name
                            .Font.FontSize = dgv.Font.Size
                        End With

                    End If
                    excelColumn += 1
                Next j
            Next i
            worksheet.Columns().AdjustToContents()
            Try
                workbook.SaveAs(saveFileDialog1.FileName)
                If File.Exists(saveFileDialog1.FileName) Then
                    Dim result As Process = Process.Start(New ProcessStartInfo With {
                                        .FileName = saveFileDialog1.FileName,
                                        .UseShellExecute = True
                                    }
                                  )
                End If
            Catch ex As IOException
                MsgBox(ex.Message, MsgBoxStyle.OkOnly)
            Catch ex1 As Exception
                Stop
            End Try
        End If
    End Sub

End Module
