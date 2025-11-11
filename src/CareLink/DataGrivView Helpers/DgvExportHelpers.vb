' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports ClosedXML.Excel

''' <summary>
'''  Provides extension methods for exporting and copying data
'''  from a <see cref="DataGridView"/>.
''' </summary>
Friend Module DgvExportHelpers

    Private ReadOnly Property zeroX37 As String = StrDup(Number:=37, Character:="0"c)

    ''' <summary>
    '''  Determines whether any cell is selected in the specified column
    '''  of the <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to check.</param>
    ''' <param name="index">The column index to check for selected cells.</param>
    ''' <returns>
    '''  <see langword="True"/> if any cell is selected in the specified column;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Private Function AnyCellSelected(dgv As DataGridView, index As Integer) As Boolean
        Dim anySelected As Boolean = False
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(index).Selected Then
                anySelected = True
                Exit For
            End If
        Next

        Return anySelected
    End Function

    ''' <summary>
    '''  Copies the selected cells or all cells from the <see cref="DataGridView"/>
    '''  to the <see cref="Clipboard"/>,
    '''  with optional headers.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to copy from.</param>
    ''' <param name="copyHeaders">
    '''  Specifies whether to include headers in the copied data.
    ''' </param>
    ''' <param name="copyAll">
    '''  <see langword="True"/>, copies all cells;
    '''  otherwise, only selected cells.
    ''' </param>
    <Extension>
    Private Sub CopyToClipboard(dgv As DataGridView, copyHeaders As DataGridViewClipboardCopyMode, copyAll As Boolean)
        If copyAll OrElse dgv.GetCellCount(includeFilter:=DataGridViewElementStates.Selected) > 0 Then
            Dim dataGridViewCells As List(Of DataGridViewCell) =
                dgv.SelectedCells.Cast(Of DataGridViewCell).ToList()

            Dim selector As Func(Of DataGridViewCell, Integer) = Function(c As DataGridViewCell) As Integer
                                                                     Return c.ColumnIndex
                                                                 End Function
            Dim colLow As Integer = If(copyAll,
                                       0,
                                       dataGridViewCells.Min(selector))

            Dim colHigh As Integer = If(copyAll,
                                        dgv.Columns.Count - 1,
                                        dataGridViewCells.Max(selector))

            selector = Function(c As DataGridViewCell) As Integer
                           Return c.RowIndex
                       End Function
            Dim rowLow As Integer = If(copyAll,
                                       0,
                                       dataGridViewCells.Min(selector))

            Dim rowHigh As Integer = If(copyAll,
                                        dgv.RowCount - 1,
                                        dataGridViewCells.Max(selector))

            Dim clipboard_string As New StringBuilder()
            If copyHeaders <> DataGridViewClipboardCopyMode.EnableWithoutHeaderText Then
                For index As Integer = colLow To colHigh
                    If Not (dgv.Columns(index).Visible AndAlso (copyAll OrElse dgv.AnyCellSelected(index))) Then
                        Continue For
                    End If

                    Dim value As String = $"{dgv.Columns(index).HeaderText.Remove(s:=vbCrLf)}"
                    Dim fieldSeparator As String = If(index = colHigh,
                                                      vbCrLf,
                                                      vbTab)

                    clipboard_string.Append(value:=$"{value}{fieldSeparator}")
                Next index
            End If
            For rowIndex As Integer = rowLow To rowHigh
                Dim row As DataGridViewRow = dgv.Rows(index:=rowIndex)
                For index As Integer = colLow To colHigh
                    If Not (dgv.Columns(index).Visible AndAlso (copyAll OrElse dgv.AnyCellSelected(index))) Then
                        Continue For
                    End If
                    Dim currentCell As DataGridViewCell = row.Cells(index)
                    Dim cellValue As Object = If(copyAll OrElse currentCell.Selected,
                                                 currentCell.Value,
                                                 "")

                    Dim fieldSeparator As String = If(index = colHigh,
                                                      vbCrLf,
                                                      vbTab)

                    clipboard_string.Append(value:=$"{cellValue}{fieldSeparator}")
                Next index
            Next
            Clipboard.SetText(clipboard_string.ToString())
        End If
    End Sub

    ''' <summary>
    '''  Exports the contents of the <see cref="DataGridView"/> to an
    '''  Excel file with formatting.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to export.</param>
    <Extension>
    Private Sub ToExcelWithFormatting(dgv As DataGridView)
        Dim baseFileName As String = dgv.Name.Remove(s:="dgv")
        Dim saveFileDialog1 As New SaveFileDialog With {
                .CheckPathExists = True,
                .FileName = $"{baseFileName} ({Date.Now:yyyy-MM-dd})",
                .Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                .InitialDirectory = GetProjectDataDirectory(),
                .OverwritePrompt = True,
                .Title = "To Excel"}

        If saveFileDialog1.ShowDialog(owner:=My.Forms.Form1) = DialogResult.OK Then
            Dim workbook As New XLWorkbook()
            Dim worksheet As IXLWorksheet = workbook.Worksheets.Add(sheetName:=baseFileName)
            Dim column As Integer = 1
            For index As Integer = 0 To dgv.Columns.Count - 1
                Dim dgvColumn As DataGridViewColumn = dgv.Columns(index)
                If dgvColumn.Visible Then
                    If dgvColumn.Name.EqualsNoCase("dateTime") Then
                        worksheet.Cell(row:=1, column).Value = "Date"
                        worksheet.Cell(row:=1, column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                        column += 1
                        worksheet.Cell(row:=1, column).Value = "Time"
                    Else
                        worksheet.Cell(row:=1, column).Value = dgvColumn.HeaderCell.Value.ToString
                    End If
                    worksheet.Cell(row:=1, column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    column += 1
                End If
            Next index

            For i As Integer = 0 To dgv.Rows.Count - 1
                column = 1
                For index As Integer = 0 To dgv.Columns.Count - 1
                    If Not dgv.Columns(index).Visible Then Continue For
                    Dim dgvCell As DataGridViewCell = dgv.Rows(index:=i).Cells(index)
                    Dim valueObject As Object = dgvCell.Value
                    Dim value As String = valueObject?.ToString
                    If String.IsNullOrWhiteSpace(value) Then
                        worksheet.Cell(row:=i + 2, column).Value = ""
                        With worksheet.Cell(row:=i + 2, column).Style
                            Dim cellStyle As DataGridViewCellStyle =
                                dgvCell.GetFormattedStyle()
                            Call .Fill.SetBackgroundColor(
                                value:=GetXlColor(cellStyle, ForeGround:=False))
                            Call .Font.SetFontColor(
                                value:=GetXlColor(cellStyle, ForeGround:=True))
                            .Font.Bold = cellStyle.Font.Bold
                            .Font.FontName = dgv.Font.Name
                            .Font.FontSize = dgv.Font.Size
                        End With
                    Else
                        Dim align As XLAlignmentHorizontalValues
                        With worksheet.Cell(row:=i + 2, column)
                            Select Case dgvCell.ValueType.Name
                                Case NameOf([Int32])
                                    align = If(dgv.Columns(index).Name.EqualsNoCase("RecordNumber"),
                                               XLAlignmentHorizontalValues.Center,
                                               XLAlignmentHorizontalValues.Right)

                                    .Value = CInt(valueObject)
                                Case NameOf(OADate)
                                    align = XLAlignmentHorizontalValues.Left
                                    Dim format As String = $"0{DecimalSeparator}{zeroX37}"
                                    Dim result As Double
                                    If Double.TryParse(value, result) Then
                                        .Value = result
                                        .Style.NumberFormat.Format = format
                                    Else
                                        .Value = $"'{value}"
                                    End If
                                Case NameOf([Decimal]),
                                     NameOf([Double]),
                                     NameOf([Single])
                                    Dim valueASingle As Single =
                                        ParseSingle(value:=valueObject, digits:=3)
                                    If Single.IsNaN(valueASingle) Then
                                        .Value = "'Infinity"
                                        align = XLAlignmentHorizontalValues.Center
                                    Else
                                        .Value = valueASingle
                                        .Style.NumberFormat.Format = If(dgv.Columns(index).Name.EqualsNoCase("sg"),
                                                                        GetSgFormat(withSign:=False),
                                                                        $"0{DecimalSeparator}000")

                                        align = XLAlignmentHorizontalValues.Right
                                    End If
                                Case NameOf([Boolean])
                                    align = XLAlignmentHorizontalValues.Center
                                    .Value = CBool(valueObject)
                                Case NameOf([String])
                                    align = XLAlignmentHorizontalValues.Left
                                    .Value = value
                                Case NameOf([DateTime])
                                    .Value = CDate(valueObject).Date
                                    Dim cellStyle As DataGridViewCellStyle = dgvCell.GetFormattedStyle()
                                    With .Style
                                        .Alignment.Horizontal =
                                            XLAlignmentHorizontalValues.Left
                                        .Fill.SetBackgroundColor(
                                            value:=cellStyle.GetXlColor(ForeGround:=False))
                                        .Font.SetFontColor(
                                            value:=cellStyle.GetXlColor(ForeGround:=True))
                                        .Font.Bold = cellStyle.Font.Bold
                                        .Font.FontName = dgv.Font.Name
                                        .Font.FontSize = dgv.Font.Size
                                    End With
                                    column += 1

                                    align = XLAlignmentHorizontalValues.Right
                                    worksheet.Cell(row:=i + 2, column).Value =
                                        CDate(valueObject).TimeOfDay
                                    worksheet.Cell(row:=i + 2, column).Style.DateFormat _
                                             .SetFormat(value:="[$-x-systime]h:mm:ss AM/PM")
                                Case Else
                                    Stop
                                    align = XLAlignmentHorizontalValues.Left
                                    .Value = value
                            End Select
                        End With

                        With worksheet.Cell(row:=i + 2, column).Style
                            Dim cellStyle As DataGridViewCellStyle =
                                dgvCell.GetFormattedStyle()

                            .Alignment.Horizontal = align
                            .Fill.SetBackgroundColor(
                                value:=cellStyle.GetXlColor(ForeGround:=False))
                            .Font.SetFontColor(
                                value:=cellStyle.GetXlColor(ForeGround:=True))
                            .Font.Bold = cellStyle.Font.Bold
                            .Font.FontName = dgv.Font.Name
                            .Font.FontSize = dgv.Font.Size
                        End With

                    End If
                    column += 1
                Next index
            Next i
            worksheet.Columns().AdjustToContents()
            Try
                workbook.SaveAs(file:=saveFileDialog1.FileName)
                If File.Exists(path:=saveFileDialog1.FileName) Then
                    Dim startInfo As New ProcessStartInfo With {
                        .FileName = saveFileDialog1.FileName,
                        .UseShellExecute = True}
                    Dim result As Process = Process.Start(startInfo)
                End If
            Catch ex As IOException
                MsgBox(
                    heading:="Error saving file!",
                    prompt:=ex.Message,
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                    title:="I/O Error")
            Catch ex1 As Exception
                Stop
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Retrieves the <see cref="DataGridView"/> associated with the sender
    '''  of a <see cref="ToolStripMenuItem"/> event.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <returns>The <see cref="DataGridView"/> associated with the context menu.</returns>
    Private Function GetDgvFromMenuItem(sender As Object) As DataGridView
        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim contextStrip As ContextMenuStrip =
            CType(menuItem.GetCurrentParent, ContextMenuStrip)
        Return CType(contextStrip.SourceControl, DataGridView)
    End Function

    ''' <summary>
    '''  Converts a <see cref="Color"/> to an <see cref="XLColor"/>.
    ''' </summary>
    ''' <param name="color">The <see cref="Color"/> to convert.</param>
    ''' <returns>The corresponding <see cref="XLColor"/>.</returns>
    <Extension>
    Private Function GetXlColor(
        cellStyle As DataGridViewCellStyle,
        ForeGround As Boolean) As XLColor

        Return If(ForeGround,
                  If(Application.IsDarkModeEnabled,
                     XLColor.FromColor(cellStyle.BackColor),
                     XLColor.FromColor(cellStyle.ForeColor)),
                  If(Application.IsDarkModeEnabled,
                     XLColor.FromColor(cellStyle.ForeColor),
                     XLColor.FromColor(cellStyle.BackColor)))
    End Function

    ''' <summary>
    '''  Copies the selected cells of a <see cref="DataGridView"/>
    '''  to the <see cref="Clipboard"/>, including headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvCopySelectedCellsToClipBoardWithHeaders(sender As Object, e As EventArgs)
        GetDgvFromMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText,
            copyAll:=False)
    End Sub

    ''' <summary>
    '''  Copies the selected cells of a <see cref="DataGridView"/> to
    '''  the <see cref="Clipboard"/>, without headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvCopySelectedCellsToClipBoardWithoutHeaders(
        sender As Object,
        e As EventArgs)

        GetDgvFromMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
            copyAll:=False)
    End Sub

    ''' <summary>
    '''  Copies all cells of a <see cref="DataGridView"/> to the <see cref="Clipboard"/>,
    '''  including headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvExportToClipBoardWithHeaders(sender As Object, e As EventArgs)
        GetDgvFromMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText,
            copyAll:=True)
    End Sub

    ''' <summary>
    '''  Copies all cells of a <see cref="DataGridView"/> to the <see cref="Clipboard"/>,
    '''  without headers.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvExportToClipBoardWithoutHeaders(sender As Object, e As EventArgs)
        GetDgvFromMenuItem(sender).CopyToClipboard(
            copyHeaders:=DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
            copyAll:=False)
    End Sub

    ''' <summary>
    '''  Exports the contents of a <see cref="DataGridView"/> to an Excel file.
    ''' </summary>
    ''' <param name="sender">The sender object from the event.</param>
    ''' <param name="e">The event arguments.</param>
    Public Sub DgvExportToExcel(sender As Object, e As EventArgs)
        GetDgvFromMenuItem(sender).ToExcelWithFormatting()
    End Sub

End Module
