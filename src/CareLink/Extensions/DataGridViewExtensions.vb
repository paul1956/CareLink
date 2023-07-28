' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DataGridViewExtensions

    <Extension>
    Friend Sub dateTimeCellFormatting(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, dateTimeKey As String)
        If e.Value Is Nothing Then
            Return
        End If
        If dgv.Columns(e.ColumnIndex).Name.Equals(dateTimeKey, StringComparison.Ordinal) Then
            Try
                Dim dateValue As Date
                dateValue = e.Value.ToString.ParseDate("")
                e.Value = dateValue.ToShortDateTimeString
            Catch ex As Exception
                e.Value = e.Value.ToString
            End Try
            e.FormattingApplied = True
        End If
    End Sub

    <Extension>
    Friend Sub InitializeDgv(dGV As DataGridView)
        With dGV
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                    .BackColor = Color.Silver
                }
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                    .Alignment = DataGridViewContentAlignment.MiddleCenter,
                    .BackColor = SystemColors.Control,
                    .Font = New Font("Segoe UI", 9.0!, FontStyle.Regular, GraphicsUnit.Point),
                    .ForeColor = SystemColors.WindowText,
                    .SelectionBackColor = SystemColors.Highlight,
                    .SelectionForeColor = SystemColors.HighlightText,
                    .WrapMode = DataGridViewTriState.True
                }
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .Dock = DockStyle.Fill
            .Location = New Point(3, 3)
            .ReadOnly = True
            .RowTemplate.Height = 25
            .TabIndex = 0
        End With
    End Sub

    <Extension>
    Friend Sub SgValueCellFormatting(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, partialKey As String)
        Dim sgColumnName As String = dgv.Columns(e.ColumnIndex).Name
        If Not sgColumnName.StartsWith(partialKey, StringComparison.InvariantCultureIgnoreCase) Then
            Return
        End If

        Dim sensorValue As Single = ParseSingle(e.Value, 2)
        If Single.IsNaN(sensorValue) Then
            FormatCell(e, Color.Gray)
            Return
        End If
        Select Case sgColumnName
            Case partialKey
                e.Value = If(NativeMmolL, sensorValue.ToString("F2", CurrentUICulture), e.Value.ToString)
                If sensorValue < TirLowLimit(NativeMmolL) Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit(NativeMmolL) Then
                    FormatCell(e, Color.Yellow)
                End If
            Case partialKey & "MmDl"
                e.Value = e.Value.ToString
                If sensorValue < TirLowLimit(False) Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit(False) Then
                    FormatCell(e, Color.Yellow)
                End If
            Case partialKey & "MmolL"
                e.Value = sensorValue.RoundSingle(2, False).ToString("F2", CurrentUICulture)
                If sensorValue < TirLowLimit(True) Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit(True) Then
                    FormatCell(e, Color.Yellow)
                End If
        End Select
    End Sub

End Module
