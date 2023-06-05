' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports ClosedXML.Excel

Friend Module ExportDataGridView

    <Extension>
    Private Function AnyCellSelected(dgv As DataGridView, col As Integer) As Boolean
        Dim anySelected As Boolean = False
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(col).Selected Then
                anySelected = True
                Exit For
            End If
        Next

        Return anySelected
    End Function

    <Extension>
    Public Sub CopyToClipboard(dgv As DataGridView, copyHeaders As DataGridViewClipboardCopyMode, copyAll As Boolean)
        If copyAll OrElse dgv.GetCellCount(DataGridViewElementStates.Selected) > 0 Then
            Dim dataGridViewCells As List(Of DataGridViewCell) = dgv.SelectedCells.Cast(Of DataGridViewCell).ToList()
            Dim colLow As Integer = If(copyAll, 0, dataGridViewCells.Min(Function(c As DataGridViewCell) c.ColumnIndex))
            Dim colHigh As Integer = If(copyAll, dgv.Columns.Count - 1, dataGridViewCells.Max(Function(c As DataGridViewCell) c.ColumnIndex))
            Dim rowLow As Integer = If(copyAll, 0, dataGridViewCells.Min(Function(c As DataGridViewCell) c.RowIndex))
            Dim rowHigh As Integer = If(copyAll, dgv.RowCount - 1, dataGridViewCells.Max(Function(c As DataGridViewCell) c.RowIndex))
            Dim clipboard_string As New StringBuilder()
            If copyHeaders <> DataGridViewClipboardCopyMode.EnableWithoutHeaderText Then
                For col As Integer = colLow To colHigh
                    If Not (dgv.Columns(col).Visible AndAlso (copyAll OrElse dgv.AnyCellSelected(col))) Then Continue For
                    clipboard_string.Append($"{dgv.Columns(col).HeaderText.Replace(vbCrLf, "")}{If(col = colHigh, vbCrLf, vbTab)}")
                Next col
            End If
            For rowIndex As Integer = rowLow To rowHigh
                Dim row As DataGridViewRow = dgv.Rows(rowIndex)
                For col As Integer = colLow To colHigh
                    If Not (dgv.Columns(col).Visible AndAlso (copyAll OrElse dgv.AnyCellSelected(col))) Then Continue For
                    Dim currentCell As DataGridViewCell = row.Cells(col)
                    clipboard_string.Append($"{If(copyAll OrElse currentCell.Selected, currentCell.Value.ToString, "")}{If(col = colHigh, vbCrLf, vbTab)}")
                Next col
            Next
            Clipboard.SetText(clipboard_string.ToString())
        End If
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
                                    Dim valueASingle As Single = ParseSingle(value, 3)
                                    If Single.IsNaN(valueASingle) Then
                                        .Value = "'Infinity"
                                        align = XLAlignmentHorizontalValues.Center
                                    Else
                                        .Value = valueASingle
                                        If dgv.Columns(j).Name.Equals("sg", StringComparison.OrdinalIgnoreCase) Then
                                            If valueASingle > 40 Then
                                                .Value = CInt(valueASingle)
                                                .Style.NumberFormat.Format = "0"
                                            Else
                                                .Style.NumberFormat.Format = "0.00"
                                            End If
                                        Else
                                            .Style.NumberFormat.Format = "0.000"
                                        End If

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
                                    worksheet.Cell(i + 2, excelColumn).Style.DateFormat.SetFormat("[$-x-systime]h:mm:ss AM/PM")
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
