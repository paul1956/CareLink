' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DgvHelpers

    Private Function IsDarkRow(rowIndex As Integer) As Boolean
        Dim rowMod2 As Integer = rowIndex Mod 2
        Return If(IsDarkMode(), rowMod2 = 0, rowMod2 = 1)
    End Function

    Friend Sub CellFormatting0Value(ByRef e As DataGridViewCellFormattingEventArgs)
        If e.Value Is Nothing OrElse e.Value.ToString = "0" Then
            e.Value = ""
            e.FormattingApplied = True
        End If
    End Sub

    Friend Sub CellFormattingApplyColor(ByRef e As DataGridViewCellFormattingEventArgs, highlightColor As Color, isUri As Boolean)
        e.Value = e.Value.ToString
        With e.CellStyle
            If IsDarkMode() Then
                If IsDarkRow(e.RowIndex) Then
                    .ForeColor = highlightColor

                    If isUri Then
                        .SelectionForeColor = Color.Purple
                        .SelectionBackColor = Color.Purple.GetContrastingColor()
                    End If
                Else
                    .ForeColor = highlightColor.InvertColor
                    If isUri Then
                        .SelectionBackColor = Color.Purple
                        .SelectionForeColor = Color.Purple.GetContrastingColor()
                    End If
                End If
            Else
                If IsDarkRow(e.RowIndex) Then
                    .ForeColor = highlightColor
                    If isUri Then
                        .SelectionForeColor = Color.Purple
                        .SelectionBackColor = Color.Purple.GetContrastingColor()
                    End If
                Else
                    .ForeColor = highlightColor.InvertColor
                    If isUri Then
                        .SelectionBackColor = Color.Purple
                        .SelectionForeColor = Color.Purple.GetContrastingColor()

                    End If
                End If
            End If
            .Font = New Font(prototype:= .Font, newStyle:=FontStyle.Bold)
        End With
        e.FormattingApplied = True
    End Sub

    <Extension>
    Friend Sub CellFormattingDateTime(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        If e.Value Is Nothing Then
            e.Value = ""
        Else
            Try
                Dim dateValue As Date
                dateValue = e.Value.ToString.ParseDate("")
                e.Value = dateValue.ToShortDateTimeString
            Catch ex As Exception
                e.Value = e.Value.ToString
            End Try
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    <Extension>
    Friend Sub CellFormattingDuration(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = $"{e.Value:h\:mm} left."
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    <Extension>
    Friend Sub CellFormattingInteger(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, message As String)
        e.Value = $"{e.Value} {message}"
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    <Extension>
    Friend Sub CellFormattingSgValue(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, partialKey As String)
        Dim sgColumnName As String = dgv.Columns(e.ColumnIndex).Name
        Dim sensorValue As Single = ParseSingle(e.Value, decimalDigits:=2)
        If Single.IsNaN(sensorValue) Then
            CellFormattingApplyColor(e, Color.Gray, isUri:=False)
        Else
            Select Case sgColumnName
                Case partialKey
                    e.Value = If(NativeMmolL, sensorValue.ToString("F2", Provider), sensorValue.ToString)
                    If sensorValue < TirLowLimit(NativeMmolL) Then
                        CellFormattingApplyColor(e, Color.Red, isUri:=False)
                    ElseIf sensorValue > TirHighLimit(NativeMmolL) Then
                        CellFormattingApplyColor(e, Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MgdL"
                    e.Value = e.Value.ToString
                    If sensorValue < TirLowLimit(False) Then
                        CellFormattingApplyColor(e, Color.Red, isUri:=False)
                    ElseIf sensorValue > TirHighLimit(False) Then
                        CellFormattingApplyColor(e, Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case $"{partialKey}MmolL"
                    e.Value = sensorValue.ToString("F2", Provider)
                    If sensorValue.RoundSingle(1, False) < TirLowLimit(True) Then
                        CellFormattingApplyColor(e, Color.Red, isUri:=False)
                    ElseIf sensorValue > TirHighLimit(True) Then
                        CellFormattingApplyColor(e, Color.Yellow, isUri:=False)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case Else
                    Stop
            End Select
        End If
    End Sub

    <Extension>
    Friend Function CellFormattingSingleValue(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, digits As Integer) As Single
        Dim amount As Single = ParseSingle(e.Value, digits)
        e.Value = amount.ToString($"F{digits}", Provider)
        dgv.CellFormattingSetForegroundColor(e)
        Return amount
    End Function

    <Extension>
    Friend Sub CellFormattingToTitle(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)

        e.Value = e.Value.ToString.Replace(vbCrLf, " ").ToTitle
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    <Extension>
    Friend Sub CellFormattingUrl(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = e.Value.ToString
        If dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Equals(dgv.CurrentCell) Then
            CellFormattingApplyColor(e, Color.Purple, isUri:=True)
        Else
            CellFormattingApplyColor(e, Color.FromArgb(&H0, &H66, &HCC), isUri:=True)
        End If
        e.FormattingApplied = True
    End Sub

    Friend Sub DgvPaint(sender As Object, e As PaintEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.Rows.Count = 0 Then
            TextRenderer.DrawText(e.Graphics, "No records found.",
                font:=New Font(family:=dgv.Font.FontFamily, emSize:=20), bounds:=dgv.ClientRectangle,
                foreColor:=dgv.ForeColor, backColor:=dgv.BackgroundColor,
                flags:=TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
        End If
    End Sub

    <Extension>
    Friend Sub DisplayEmptyDGV(realPanel As TableLayoutPanel, className As String)
        Dim dGVIndex As Integer = realPanel.Controls.Count - 1
        Dim dGV As DataGridView = TryCast(realPanel.Controls(dGVIndex), DataGridView)
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
        RemoveHandler dGV.Paint, AddressOf DgvPaint
        AddHandler dGV.Paint, AddressOf DgvPaint
    End Sub

    Friend Sub HideDataGridViewColumns(ByRef dgv As DataGridView, hideColumnFunction As Func(Of String, Boolean))
        Dim lastColumnIndex As Integer = dgv.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            If i > 0 AndAlso String.IsNullOrWhiteSpace(dgv.Columns(i).DataPropertyName) Then
                Stop
            End If
            dgv.Columns(i).Visible = Not hideColumnFunction(dgv.Columns(i).DataPropertyName)
        Next
    End Sub

    <Extension>
    Friend Sub CellFormattingSetForegroundColor(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        Dim col As DataGridViewTextBoxColumn = TryCast(dgv.Columns(e.ColumnIndex), DataGridViewTextBoxColumn)
        If col IsNot Nothing Then
            e.Value = $"{e.Value}"
            e.CellStyle.ForeColor = If(IsDarkMode(), If(IsDarkRow(e.RowIndex), Color.White, Color.Black), Color.Black)
            e.FormattingApplied = True
        End If
    End Sub

End Module
