' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DataGridViewExtensions

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
            .SelectionMode = DataGridViewSelectionMode.CellSelect
            .TabIndex = 0
        End With
    End Sub

    <Extension>
    Public Sub dgvCellFormatting(dgv As DataGridView, ByRef e As DataGridViewCellFormattingEventArgs, key As String)
        If e.Value Is Nothing Then
            Return
        End If
        If dgv.Columns(e.ColumnIndex).Name.Equals(key, StringComparison.Ordinal) Then
            Dim dateValue As Date = e.Value.ToString.ParseDate("")
            e.Value = dateValue.ToShortDateTimeString
        End If
    End Sub

    <Extension>
    Public Sub DgvColumnAdded(ByRef e As DataGridViewColumnEventArgs, cellStyle As DataGridViewCellStyle, wrapHeader As Boolean, forceReadOnly As Boolean, caption As String)
        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            Dim idHeaderName As Boolean = .DataPropertyName = "ID"
            .ReadOnly = forceReadOnly OrElse idHeaderName
            .Resizable = DataGridViewTriState.False
            Dim title As New StringBuilder
            Dim titleInTitleCase As String = .Name
            If Not .Name.Contains("OADateTime", StringComparison.InvariantCultureIgnoreCase) Then
                titleInTitleCase = If(.DataPropertyName.Length < 4, .Name, .Name.ToTitleCase())
            End If

            If wrapHeader Then
                Dim titleSplit As String() = titleInTitleCase.Replace("A I T", "AIT").Split(" "c)
                For Each s As String In titleSplit
                    If s.Length < 5 Then
                        title.Append(s)
                    Else
                        title.AppendLine(s)
                    End If
                Next
            Else
                title.Append(titleInTitleCase.Replace("Care Link", $"{ProjectName}"))
            End If
            .HeaderText = title.TrimEnd(Environment.NewLine).ToString
            .DefaultCellStyle = cellStyle
            If .HeaderText <> "Record Number" Then
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End If
            If String.IsNullOrWhiteSpace(caption) Then Return
            .HeaderText = caption.Replace("_", "")
            If .DataPropertyName.Contains("message", StringComparison.OrdinalIgnoreCase) Then
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            ElseIf .DataPropertyName.Equals("RecordNumber", StringComparison.OrdinalIgnoreCase) Then
                .SortMode = DataGridViewColumnSortMode.Automatic
                .HeaderCell.SortGlyphDirection = SortOrder.Ascending
            End If
        End With

    End Sub

    <Extension>
    Public Function SetCellStyle(cellStyle As DataGridViewCellStyle, alignment As DataGridViewContentAlignment, padding As Padding) As DataGridViewCellStyle
        cellStyle.Alignment = alignment
        cellStyle.Padding = padding
        Return cellStyle
    End Function

End Module
