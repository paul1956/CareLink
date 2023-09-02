' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module CellFormattingHelpers

    Friend Sub CellFormatting0Value(e As DataGridViewCellFormattingEventArgs)
        If e.Value Is Nothing OrElse e.Value.ToString = "0" Then
            e.Value = ""
            e.FormattingApplied = True
        End If
    End Sub

    Friend Sub CellFormattingApplyColor(ByRef e As DataGridViewCellFormattingEventArgs, highlightColor As Color, AlternateIndex As Integer, isUri As Boolean)
        e.Value = e.Value.ToString
        With e.CellStyle
            If e.RowIndex Mod 2 = AlternateIndex Then
                .BackColor = highlightColor
                .ForeColor = highlightColor.GetContrastingColor()
                If isUri Then
                    .SelectionForeColor = Color.Purple
                    .SelectionBackColor = Color.Purple.GetContrastingColor()
                End If
            Else
                .ForeColor = highlightColor
                .BackColor = highlightColor.GetContrastingColor()
                If isUri Then
                    .SelectionBackColor = Color.Purple
                    .SelectionForeColor = Color.Purple.GetContrastingColor()
                End If
            End If
            .Font = New Font(.Font, FontStyle.Bold)
        End With
        e.FormattingApplied = True
    End Sub

    Friend Sub CellFormattingDateTime(ByRef e As DataGridViewCellFormattingEventArgs)
        If e.Value Is Nothing Then
            e.Value = ""
            e.FormattingApplied = True
        End If
        Try
            Dim dateValue As Date
            dateValue = e.Value.ToString.ParseDate("")
            e.Value = dateValue.ToShortDateTimeString
        Catch ex As Exception
            e.Value = e.Value.ToString
        End Try
        e.FormattingApplied = True
    End Sub

    Friend Sub CellFormattingInteger(ByRef e As DataGridViewCellFormattingEventArgs, message As String)
        e.Value = $"{e.Value} {message}"
        e.FormattingApplied = True
    End Sub

    <Extension>
    Friend Sub CellFormattingSgValue(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, partialKey As String, AlternativeRowIndex As Integer)
        Dim sgColumnName As String = dgv.Columns(e.ColumnIndex).Name
        Dim sensorValue As Single = ParseSingle(e.Value, 2)
        If Single.IsNaN(sensorValue) Then
            CellFormattingApplyColor(e, Color.Gray, 1 - AlternativeRowIndex, False)
        Else
            Select Case sgColumnName
                Case partialKey
                    e.Value = If(NativeMmolL, sensorValue.ToString("F2", CurrentUICulture), sensorValue.ToString)
                    If sensorValue < TirLowLimit(NativeMmolL) Then
                        CellFormattingApplyColor(e, Color.Red, 1 - AlternativeRowIndex, False)
                    ElseIf sensorValue > TirHighLimit(NativeMmolL) Then
                        CellFormattingApplyColor(e, Color.Yellow, AlternativeRowIndex, False)
                    End If
                Case $"{partialKey}MmDl"
                    e.Value = e.Value.ToString
                    If sensorValue < TirLowLimit(False) Then
                        CellFormattingApplyColor(e, Color.Red, 1 - AlternativeRowIndex, False)
                    ElseIf sensorValue > TirHighLimit(False) Then
                        CellFormattingApplyColor(e, Color.Yellow, AlternativeRowIndex, False)
                    End If
                Case $"{partialKey}MmolL"
                    e.Value = sensorValue.ToString("F2", CurrentUICulture)
                    If sensorValue.RoundSingle(1, False) < TirLowLimit(True) Then
                        CellFormattingApplyColor(e, Color.Red, 1 - AlternativeRowIndex, False)
                    ElseIf sensorValue > TirHighLimit(True) Then
                        CellFormattingApplyColor(e, Color.Yellow, AlternativeRowIndex, False)
                    End If
            End Select
        End If
    End Sub

    Friend Function CellFormattingSingleValue(ByRef e As DataGridViewCellFormattingEventArgs, digits As Integer) As Single
        Dim amount As Single = ParseSingle(e.Value, digits)
        e.Value = amount.ToString($"F{digits}", CurrentUICulture)
        e.FormattingApplied = True
        Return amount
    End Function

    Friend Sub CellFormattingToTitle(ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = e.Value.ToString.ToTitle
        e.FormattingApplied = True
    End Sub

    <Extension>
    Friend Sub CellFormattingUrl(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs)
        e.Value = e.Value.ToString
        If dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Equals(dgv.CurrentCell) Then
            CellFormattingApplyColor(e, Color.Purple, 0, True)
        Else
            CellFormattingApplyColor(e, Color.FromArgb(&H0, &H66, &HCC), 1, True)
        End If
        e.FormattingApplied = True

    End Sub

End Module
