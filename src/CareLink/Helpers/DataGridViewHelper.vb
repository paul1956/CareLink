' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module DataGridViewHelper

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(GetCellStyle(e.Column.Name),
                        False,
                        True,
                        caption)
    End Sub

    Private Function GetDisplayName(Of T As Class)(_displayNameMapping As Dictionary(Of String, String), name As String) As String
        If _displayNameMapping.Count = 0 Then
            _displayNameMapping = ClassPropertiesToDisplayNames(Of T)()
        End If
        Return _displayNameMapping(name)
    End Function

    Friend Function CreateDefaultDataGridView(dgvName As String) As DataGridView
        Dim dGV As New DataGridView With {
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
                    .BackColor = Color.Silver
                },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                    .Alignment = DataGridViewContentAlignment.MiddleCenter,
                    .BackColor = SystemColors.Control,
                    .Font = New System.Drawing.Font("Segoe UI", 9.0!, FontStyle.Regular, GraphicsUnit.Point),
                    .ForeColor = SystemColors.WindowText,
                    .SelectionBackColor = SystemColors.Highlight,
                    .SelectionForeColor = SystemColors.HighlightText,
                    .WrapMode = DataGridViewTriState.True
                },
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            .Dock = DockStyle.Fill,
            .Location = New System.Drawing.Point(3, 3),
            .Name = dgvName,
            .[ReadOnly] = True,
            .SelectionMode = DataGridViewSelectionMode.CellSelect,
            .TabIndex = 0
        }
        dGV.RowTemplate.Height = 25
        Return dGV
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
    End Sub

    Public Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(SummaryRecord.RecordNumber)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(1))
            Case NameOf(SummaryRecord.Key),
                 NameOf(SummaryRecord.Message)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case NameOf(SummaryRecord.Value)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case Else
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Public Function GetColumnAlignment(Of T As Class)(_columnAlignmentMapping As Dictionary(Of String, DataGridViewContentAlignment), name As String) As DataGridViewContentAlignment
        If _columnAlignmentMapping.Count = 0 Then
            _columnAlignmentMapping = ClassPropertiesToCoumnAlignment(Of T)()
        End If
        Return _columnAlignmentMapping(name)
    End Function

End Module
