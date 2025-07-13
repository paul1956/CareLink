' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DgvHelpers

    ''' <summary>
    '''  Applies dark mode styling to the DataGridView column headers.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to style.</param>
    <Extension>
    Public Sub ApplyDarkModeFixes(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.Padding = New Padding(all:=0)
        dgv.BorderStyle = BorderStyle.None
    End Sub

    ''' <summary>
    '''  Sets the cell value to <see cref="String.Empty"/> if the value is <see langword="Nothing"/> or "0".
    '''  This is used to avoid displaying "0" in cells where it is not meaningful, such as in a duration column.
    ''' </summary>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    Public Sub CellFormatting0Value(ByRef e As DataGridViewCellFormattingEventArgs)
        Dim value As String = Convert.ToString(e.Value)
        If value = String.Empty OrElse value = "0" Then
            e.Value = String.Empty
            e.FormattingApplied = True
        End If
    End Sub

    ''' <summary>
    '''  Applies a <see cref="Color"/> to the cell based on whether it is a URI or not.
    '''  If it is a URI, it applies a purple color; otherwise, it applies the specified highlight color.
    '''  Colored cells are also set to bold font style.
    '''  This is useful for highlighting important cells, such as those containing URIs or other significant values.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    ''' <param name="textColor">The color to use for highlighting.</param>
    ''' <param name="isUri">Indicates if the cell value is a URI.</param>
    ''' <param name="emIncrease"></param>
    <Extension>
    Public Sub CellFormattingApplyBoldColor(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        textColor As Color,
        isUri As Boolean,
        Optional emIncrease As Integer = 0)

        Dim value As String = Convert.ToString(e.Value)
        If String.IsNullOrEmpty(value) Then
            e.Value = String.Empty
            e.FormattingApplied = True
            Return
        End If
        If value = ClickToShowDetails Then
            e.Value = value
            Dim row As DataGridViewRow = dgv.Rows(index:=e.RowIndex)
            row.Cells(index:=e.ColumnIndex).ToolTipText = $"{ClickToShowDetails} for {row.Cells(index:=1).Value}."
        End If
        With e.CellStyle
            If isUri Then
                Dim foregroundColor As Color = dgv.Rows(e.RowIndex).GetTextColor(textColor:=Color.Purple)
                If dgv.Rows(index:=e.RowIndex).IsDarkRow() Then
                    .SelectionBackColor = foregroundColor.ContrastingColor()
                    .SelectionForeColor = foregroundColor
                Else
                    .SelectionBackColor = foregroundColor
                    .SelectionForeColor = foregroundColor.ContrastingColor()
                End If
            Else
                .ForeColor = dgv.Rows(e.RowIndex).GetTextColor(textColor)
            End If
            .Font = New Font(family:= .Font.FontFamily, emSize:= .Font.Size + emIncrease, style:=FontStyle.Bold)
        End With
        e.FormattingApplied = True
    End Sub

    ''' <summary>
    '''  Formats the cell value as a date time string.
    '''  If the value is not a valid date, it will leave the value unchanged.
    '''  It also sets the foreground color based on the row's text color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    <Extension>
    Public Sub CellFormattingDateTime(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        Dim value As String = Convert.ToString(e.Value)
        If value <> String.Empty Then
            Try
                e.Value = value.ParseDate("").ToShortDateTimeString
            Catch ex As Exception
                e.Value = value
            End Try
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Formats the cell value as an integer with a message appended, and sets the foreground color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    ''' <param name="message">The message to append to the value.</param>
    <Extension>
    Public Sub CellFormattingInteger(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, message As String)
        e.Value = $"{e.Value} {message}"
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Sets the foreground <see cref="Color"/> of a cell based on the row's text color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    ''' <param name="bold">If set to <see langword="True"/>, applies bold font style to the cell.</param>
    <Extension>
    Public Sub CellFormattingSetForegroundColor(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        Optional bold As Boolean = False)
        Dim col As DataGridViewTextBoxColumn = TryCast(dgv.Columns(e.ColumnIndex), DataGridViewTextBoxColumn)

        If col IsNot Nothing Then
            e.Value = $"{e.Value}"
            Dim textColor As Color = e.CellStyle.ForeColor
            Dim argb As Integer = textColor.ToArgb()
            If argb <> Color.Black.ToArgb() AndAlso argb <> Color.White.ToArgb() Then
                e.CellStyle.ForeColor = dgv.Rows(e.RowIndex).GetTextColor(textColor)
            End If
            e.CellStyle.Font = If(bold,
                New Font(prototype:=e.CellStyle.Font, newStyle:=FontStyle.Bold),
                New Font(prototype:=e.CellStyle.Font, newStyle:=FontStyle.Regular))
            e.FormattingApplied = True
        End If
    End Sub

    ''' <summary>
    '''  Formats the cell value as an SG (Sensor Glucose) value, applying color based on TIR (Time In Range) limits.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    ''' <param name="partialKey">The partial column name key to match for formatting.</param>
    <Extension>
    Public Sub CellFormattingSgValue(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, partialKey As String)
        Dim sgColumnName As String = dgv.Columns(e.ColumnIndex).Name
        Dim sensorValue As Single = ParseSingle(e.Value, digits:=1)
        If Single.IsNaN(sensorValue) Then
            e.Value = "NaN"
            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red, isUri:=False)
        Else
            Select Case sgColumnName
                Case partialKey
                    e.Value = If(NativeMmolL, sensorValue.ToString("F1", Provider), sensorValue.ToString)
                    If sensorValue < GetTirLowLimit() Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red, isUri:=False)
                    ElseIf sensorValue > GetTirHighLimit() Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MgdL"
                    e.Value = Convert.ToString(e.Value)
                    If sensorValue < GetTirLowLimit(asMmolL:=False) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red, isUri:=False)
                    ElseIf sensorValue > GetTirHighLimit(asMmolL:=False) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MmolL"
                    e.Value = sensorValue.ToString("F1", Provider)
                    If sensorValue.RoundSingle(digits:=1, considerValue:=False) < GetTirLowLimit(asMmolL:=True) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red, isUri:=False)
                    ElseIf sensorValue > GetTirHighLimit(asMmolL:=True) Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case Else
                    Stop
            End Select
        End If
    End Sub

    ''' <summary>
    '''  Formats the cell value as a single-precision floating point value with the specified number of digits.
    '''  Also sets the foreground color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    ''' <param name="digits">The number of decimal digits to display.</param>
    ''' <returns>The parsed single value.</returns>
    <Extension>
    Public Function CellFormattingSingleValue(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        digits As Integer) As Single

        Dim amount As Single = ParseSingle(e.Value, digits)
        e.Value = amount.ToString($"F{digits}", Provider)
        dgv.CellFormattingSetForegroundColor(e)
        Return amount
    End Function

    ''' <summary>
    '''  Formats the cell value to center-align it if it contains a single word (no spaces) and column is not 0.
    '''  This is useful for displaying single-word values in a visually appealing manner.
    '''  Only alphabetic words (A-Z, a-z) are considered.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    <Extension>
    Public Sub CellFormattingSingleWord(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        Dim value As String = Convert.ToString(e.Value)
        If e.ColumnIndex > 0 AndAlso Text.RegularExpressions.Regex.IsMatch(value, pattern:="^[A-Za-z]+$") Then
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Formats the cell value as a title-cased string, replacing line breaks with spaces.
    '''  Also sets the foreground color.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    ''' <param name="bold">If set to <see langword="True"/>, applies bold font style to the cell.</param>
    <Extension>
    Public Sub CellFormattingToTitle(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        Optional bold As Boolean = False)

        e.Value = Convert.ToString(e.Value).Replace(vbCrLf, " ").ToTitle
        dgv.CellFormattingSetForegroundColor(e, bold)
    End Sub

    ''' <summary>
    '''  Formats the cell as a URL, applying a color based on selection state.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the cell.</param>
    ''' <param name="e">The <see cref="DataGridViewCellFormattingEventArgs"/> for the cell being formatted.</param>
    <Extension>
    Public Sub CellFormattingUrl(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = Convert.ToString(e.Value)
        If dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Equals(dgv.CurrentCell) Then
            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Purple, isUri:=True)
        Else
            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.FromArgb(red:=0, green:=160, blue:=204), isUri:=True)
        End If
        e.FormattingApplied = True
    End Sub

    ''' <summary>
    '''  Paints a "No records found." message on the <see cref="DataGridView"/> if it contains no rows.
    ''' </summary>
    ''' <param name="sender">The DataGridView being painted.</param>
    ''' <param name="e">The <see cref="PaintEventArgs"/> for the paint event.</param>
    Public Sub DgvPaintNoRecordsFound(sender As Object, e As PaintEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Rows.Count = 0 Then
            TextRenderer.DrawText(
                dc:=e.Graphics,
                text:="No records found.",
                font:=New Font(family:=dgv.Font.FontFamily, emSize:=20),
                bounds:=dgv.DisplayRectangle,
                foreColor:=dgv.ForeColor,
                backColor:=dgv.BackgroundColor,
                flags:=TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End If
    End Sub

    ''' <summary>
    '''  Displays an empty <see cref="DataGridView"/> in the specified panel if one does not already exist.
    ''' </summary>
    ''' <param name="realPanel">The <see cref="TableLayoutPanel"/> to add the DataGridView to.</param>
    ''' <param name="className">The class name to use for naming the DataGridView.</param>
    <Extension>
    Public Sub DisplayEmptyDGV(realPanel As TableLayoutPanel, className As String)
        Dim dgvIndex As Integer = realPanel.Controls.Count - 1
        Dim dgv As DataGridView = Nothing
        If dgvIndex >= 0 Then
            dgv = TryCast(realPanel.Controls(dgvIndex), DataGridView)
        End If
        If dgv Is Nothing Then
            dgv = New DataGridView With {
                .AutoSize = True,
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                .BorderStyle = BorderStyle.None,
                .ColumnHeadersVisible = False,
                .Dock = DockStyle.Fill,
                .Name = $"DataGridView{className}",
                .RowHeadersVisible = False}
            realPanel.Controls.Add(control:=dgv, column:=0, row:=1)
        Else
            If dgv.DataSource IsNot Nothing Then
                dgv.DataSource = Nothing
            Else
                dgv.Rows.Clear()
                dgv.Columns.Clear()
            End If
        End If
        RemoveHandler dgv.Paint, AddressOf DgvPaintNoRecordsFound
        AddHandler dgv.Paint, AddressOf DgvPaintNoRecordsFound
        dgv.Refresh()
    End Sub

    ''' <summary>
    '''  Hide column based on column name matching name returned by <paramref name="hideColumnFunction"/>
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> whose columns will be hidden.</param>
    ''' <param name="hideColumnFunction">A function that returns True for column names to hide.</param>
    Public Sub HideDataGridViewColumnsByName(ByRef dgv As DataGridView, hideColumnFunction As Func(Of String, Boolean))
        Dim lastColumnIndex As Integer = dgv.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            If i > 0 AndAlso String.IsNullOrWhiteSpace(dgv.Columns(index:=i).DataPropertyName) Then
                Stop
            End If
            dgv.Columns(index:=i).Visible = Not hideColumnFunction(dgv.Columns(index:=i).DataPropertyName)
        Next
    End Sub

    ''' <summary>
    '''  Hides a column if all its cell values match the specified value.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> containing the column.</param>
    ''' <param name="columnName">The name of the column to check.</param>
    ''' <param name="value">The value to compare against each cell in the column.</param>
    Public Sub HideUnneededColumns(ByRef dgv As DataGridView, columnName As String, value As String)
        Dim isColumnNeeded As Boolean = False
        Dim column As DataGridViewColumn = dgv.Columns(columnName)

        ' Check each cell in the current column
        For Each row As DataGridViewRow In dgv.Rows
            ' Skip checking new rows (if AllowUserToAddRows is True)
            If Not row.IsNewRow Then
                Dim cellValue As Object = row.Cells(column.Index).Value

                ' If the cell has value different then string then the column is needed
                If cellValue IsNot Nothing AndAlso cellValue.ToString <> value Then
                    isColumnNeeded = True
                    Exit For
                End If
            End If
        Next
        column.Visible = isColumnNeeded
    End Sub

    ''' <summary>
    '''  Determines if the row uses a dark background color.
    ''' </summary>
    ''' <param name="row">The <see cref="DataGridViewRow"/> to check.</param>
    ''' <returns><see langword="True"/> if the row uses a dark color; otherwise, <see langword="False"/>.</returns>
    <Extension>
    Public Function IsDarkRow(row As DataGridViewRow) As Boolean
        Return row.Index Mod 2 = 1
    End Function

End Module
