' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports ClosedXML.Excel

''' <summary>
'''  Provides extension methods for exporting and copying data from a <see cref="DataGridView"/>.
''' </summary>
Friend Module ExportDataGridView

    ''' <summary>
    '''  Determines whether any cell is selected in the specified column of the <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to check.</param>
    ''' <param name="col">The column index to check for selected cells.</param>
    ''' <returns>
    '''  <see langword="True"/> if any cell is selected in the specified column; otherwise, <see langword="False"/>.
    ''' </returns>
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

    ''' <summary>
    '''  Copies the selected cells or all cells from the <see cref="DataGridView"/> to the <see cref="Clipboard"/>,
    '''  with optional headers.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to copy from.</param>
    ''' <param name="copyHeaders">Specifies whether to include headers in the copied data.</param>
    ''' <param name="copyAll">If <see langword="True"/>, copies all cells; otherwise, only selected cells.</param>
    <Extension>
    Private Sub CopyToClipboard(dgv As DataGridView, copyHeaders As DataGridViewClipboardCopyMode, copyAll As Boolean)
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

    ''' <summary>
    '''  Exports the contents of the <see cref="DataGridView"/> to an Excel file with formatting.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to export.</param>
    <Extension>
    Private Sub ExportToExcelWithFormatting(dgv As DataGridView)
        Dim baseFileName As String = dgv.Name.Replace("dgv", "", StringComparison.CurrentCultureIgnoreCase)
        Dim saveFileDialog1 As New SaveFileDialog With {
                .CheckPathExists = True,
                .FileName = $"{baseFileName} ({Date.Now:yyyy-MM-dd})",
                .Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                .InitialDirectory = DirectoryForProjectData,
                .OverwritePrompt = True,
                .Title = "To Excel"
               }

        If saveFileDialog1.ShowDialog(My.Forms.Form1) = DialogResult.OK Then
            Dim workbook As New XLWorkbook()
            Dim worksheet As IXLWorksheet = workbook.Worksheets.Add(baseFileName)
            Dim excelColumn As Integer = 1
            For j As Integer = 0 To dgv.Columns.Count - 1
                Dim dgvColumn As DataGridViewColumn = dgv.Columns(j)
                If dgvColumn.Visible Then
                    If dgvColumn.Name.EqualsIgnoreCase("dateTime") Then
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
                                    align = If(dgv.Columns(j).Name.EqualsIgnoreCase("RecordNumber"),
                                                    XLAlignmentHorizontalValues.Center,
                                                    XLAlignmentHorizontalValues.Right)
                                    .Value = CInt(value)
                                Case NameOf(OADate)
                                    align = XLAlignmentHorizontalValues.Left
                                    Dim result As Double
                                    If Double.TryParse(valueAsString, result) Then
                                        .Value = result
                                        .Style.NumberFormat.Format = $"0{Provider.NumberFormat.NumberDecimalSeparator}{StrDup(Number:=37, Character:="0"c)}"
                                    Else
                                        .Value = $"'{valueAsString}"
                                    End If
                                Case NameOf([Decimal]), NameOf([Double]), NameOf([Single])
                                    Dim valueASingle As Single = ParseSingle(value, digits:=3)
                                    If Single.IsNaN(valueASingle) Then
                                        .Value = "'Infinity"
                                        align = XLAlignmentHorizontalValues.Center
                                    Else
                                        .Value = valueASingle
                                        .Style.NumberFormat.Format = If(dgv.Columns(j).Name.EqualsIgnoreCase("sg"),
                                                                        GetSgFormat(withSign:=False),
                                                                        $"0{Provider.NumberFormat.NumberDecimalSeparator}000"
                                                                       )

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
                MsgBox(
                    heading:="Error saving file!",
                    text:=ex.Message,
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                    title:="I/O Error")
            Catch ex1 As Exception
                Stop
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Retrieves the <see cref="DataGridView"/> associated with the sender of a <see cref="ToolStripMenuItem"/> event.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <returns>The <see cref="DataGridView"/> associated with the context menu.</returns>
    Private Function GetDgvFromToolStripMenuItem(sender As Object) As DataGridView
        Dim contextStrip As ContextMenuStrip = CType(CType(sender, ToolStripMenuItem).GetCurrentParent, ContextMenuStrip)
        Return CType(contextStrip.SourceControl, DataGridView)
    End Function

    ''' <summary>
    '''  Copies the selected cells of a <see cref="DataGridView"/> to the <see cref="Clipboard"/>, including headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvCopySelectedCellsToClipBoardWithHeaders(sender As Object, e As EventArgs)
        GetDgvFromToolStripMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText,
            copyAll:=False)
    End Sub

    ''' <summary>
    '''  Copies the selected cells of a <see cref="DataGridView"/> to the <see cref="Clipboard"/>, without headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvCopySelectedCellsToClipBoardWithoutHeaders(sender As Object, e As EventArgs)
        GetDgvFromToolStripMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
            copyAll:=False)
    End Sub

    ''' <summary>
    '''  Copies all cells of a <see cref="DataGridView"/> to the <see cref="Clipboard"/>, including headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvExportToClipBoardWithHeaders(sender As Object, e As EventArgs)
        GetDgvFromToolStripMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText,
            copyAll:=True)
    End Sub

    ''' <summary>
    '''  Copies all cells of a <see cref="DataGridView"/> to the <see cref="Clipboard"/>, without headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvExportToClipBoardWithoutHeaders(sender As Object, e As EventArgs)
        GetDgvFromToolStripMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
            copyAll:=False)
    End Sub

    ''' <summary>
    '''  Exports the contents of a <see cref="DataGridView"/> to an Excel file.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvExportToExcel(sender As Object, e As EventArgs)
        GetDgvFromToolStripMenuItem(sender).ExportToExcelWithFormatting()
    End Sub

End Module
