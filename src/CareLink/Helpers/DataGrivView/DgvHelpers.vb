' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DgvHelpers

    ''' <summary>
    '''  Sets the cell value to <see cref="String.Empty"/> if the value is <see langword="Nothing"/> or "0".
    '''  This is used to avoid displaying "0" in cells where it is not meaningful, such as in a duration column.
    ''' </summary>
    ''' <param name="e"></param>
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
    ''' </summary>
    ''' <param name="dgv"></param>
    ''' <param name="e"></param>
    ''' <param name="highlightColor"></param>
    ''' <param name="isUri"></param>
    <Extension>
    Public Sub CellFormattingApplyColor(
        dgv As DataGridView,
        ByRef e As DataGridViewCellFormattingEventArgs,
        highlightColor As Color,
        isUri As Boolean)

        e.Value = Convert.ToString(e.Value)
        With e.CellStyle
            If isUri Then
                Dim foregroundColor As Color = dgv.Rows(e.RowIndex).GetTextColor(Color.Purple)
                If dgv.Rows(e.RowIndex).IsDarkRow() Then
                    .SelectionBackColor = foregroundColor.GetContrastingColor()
                    .SelectionForeColor = foregroundColor
                Else
                    .SelectionBackColor = foregroundColor
                    .SelectionForeColor = foregroundColor.GetContrastingColor()
                End If
            Else
                .ForeColor = dgv.Rows(e.RowIndex).GetTextColor(highlightColor)
            End If
            .Font = New Font(prototype:= .Font, newStyle:=FontStyle.Bold)
        End With
        e.FormattingApplied = True
    End Sub

    ''' <summary>
    '''  Formats the cell value as a date time string.
    '''  If the value is not a valid date, it will leave the value unchanged.
    '''  It also sets the foreground color based on the row's text color.
    ''' </summary>
    ''' <param name="dgv"></param>
    ''' <param name="e"></param>
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

    <Extension>
    Public Sub CellFormattingInteger(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, message As String)
        e.Value = $"{e.Value} {message}"
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    <Extension>
    Public Sub CellFormattingSgValue(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, partialKey As String)
        Dim sgColumnName As String = dgv.Columns(e.ColumnIndex).Name
        Dim sensorValue As Single = ParseSingle(e.Value, decimalDigits:=2)
        If Single.IsNaN(sensorValue) Then
            dgv.CellFormattingApplyColor(e, highlightColor:=Color.Red, isUri:=False)
        Else
            Select Case sgColumnName
                Case partialKey
                    e.Value = If(NativeMmolL, sensorValue.ToString("F2", Provider), sensorValue.ToString)
                    If sensorValue < GetTirLowLimit() Then
                        dgv.CellFormattingApplyColor(e, highlightColor:=Color.Red, isUri:=False)
                    ElseIf sensorValue > GetTirHighLimit() Then
                        dgv.CellFormattingApplyColor(e, highlightColor:=Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MgdL"
                    e.Value = Convert.ToString(e.Value)
                    If sensorValue < GetTirLowLimit(asMmolL:=False) Then
                        dgv.CellFormattingApplyColor(e, highlightColor:=Color.Red, isUri:=False)
                    ElseIf sensorValue > GetTirHighLimit(asMmolL:=False) Then
                        dgv.CellFormattingApplyColor(e, highlightColor:=Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MmolL"
                    e.Value = sensorValue.ToString("F2", Provider)
                    If sensorValue.RoundSingle(decimalDigits:=1, considerValue:=False) < GetTirLowLimit(asMmolL:=True) Then
                        dgv.CellFormattingApplyColor(e, highlightColor:=Color.Red, isUri:=False)
                    ElseIf sensorValue > GetTirHighLimit(asMmolL:=True) Then
                        dgv.CellFormattingApplyColor(e, highlightColor:=Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case Else
                    Stop
            End Select
        End If
    End Sub

    <Extension>
    Public Function CellFormattingSingleValue(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, digits As Integer) As Single
        Dim amount As Single = ParseSingle(e.Value, digits)
        e.Value = amount.ToString($"F{digits}", Provider)
        dgv.CellFormattingSetForegroundColor(e)
        Return amount
    End Function

    <Extension>
    Public Sub CellFormattingToTitle(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = Convert.ToString(e.Value).Replace(vbCrLf, " ").ToTitle
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    <Extension>
    Public Sub CellFormattingUrl(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = Convert.ToString(e.Value)
        If dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Equals(dgv.CurrentCell) Then
            dgv.CellFormattingApplyColor(e, highlightColor:=Color.Purple, isUri:=True)
        Else
            dgv.CellFormattingApplyColor(e, highlightColor:=Color.FromArgb(red:=&H0, green:=&H66, blue:=&HCC), isUri:=True)
        End If
        e.FormattingApplied = True
    End Sub

    Public Sub DgvPaintNoRecordsFound(sender As Object, e As PaintEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Rows.Count = 0 Then
            TextRenderer.DrawText(
                dc:=e.Graphics,
                text:="No records found.",
                font:=New Font(family:=dgv.Font.FontFamily, emSize:=20),
                bounds:=dgv.ClientRectangle,
                foreColor:=dgv.ForeColor,
                backColor:=dgv.BackgroundColor,
                flags:=TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End If
    End Sub

    <Extension>
    Public Sub DisplayEmptyDGV(realPanel As TableLayoutPanel, className As String)
        Dim dGVIndex As Integer = realPanel.Controls.Count - 1
        Dim dGV As DataGridView = Nothing
        If dGVIndex >= 0 Then
            dGV = TryCast(realPanel.Controls(dGVIndex), DataGridView)
        End If
        If dGV Is Nothing Then
            dGV = New DataGridView With {
                .AutoSize = True,
                .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                .ColumnHeadersVisible = False,
                .Dock = DockStyle.Fill,
                .Name = $"DataGridView{className}",
                .RowHeadersVisible = False}
            realPanel.Controls.Add(control:=dGV, column:=0, row:=1)
        End If
        RemoveHandler dGV.Paint, AddressOf DgvPaintNoRecordsFound
        AddHandler dGV.Paint, AddressOf DgvPaintNoRecordsFound
    End Sub

    ''' <summary>
    '''  Hide column based on column name matching name returned by <paramref name="hideColumnFunction"/>
    ''' </summary>
    ''' <param name="dgv"></param>
    ''' <param name="hideColumnFunction"></param>
    Public Sub HideDataGridViewColumnsByName(ByRef dgv As DataGridView, hideColumnFunction As Func(Of String, Boolean))
        Dim lastColumnIndex As Integer = dgv.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            If i > 0 AndAlso String.IsNullOrWhiteSpace(dgv.Columns(i).DataPropertyName) Then
                Stop
            End If
            dgv.Columns(i).Visible = Not hideColumnFunction(dgv.Columns(i).DataPropertyName)
        Next
    End Sub

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

    <Extension>
    Public Function IsDarkRow(row As DataGridViewRow) As Boolean
        Dim backColor As Color = If(
            row.Index Mod 2 = 0,
            row.DataGridView.DefaultCellStyle.BackColor,
            row.DataGridView.AlternatingRowsDefaultCellStyle.BackColor)

        If backColor = Color.Empty Then
            backColor = row.DataGridView.DefaultCellStyle.BackColor
        End If
        Return backColor.IsDarkColor()
    End Function

    <Extension>
    Public Sub CellFormattingSetForegroundColor(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        Dim col As DataGridViewTextBoxColumn = TryCast(dgv.Columns(e.ColumnIndex), DataGridViewTextBoxColumn)
        If col IsNot Nothing Then
            e.Value = $"{e.Value}"
            e.CellStyle.ForeColor = dgv.Rows(e.RowIndex).GetTextColor(Color.White)
            e.FormattingApplied = True
        End If
    End Sub
End Module
