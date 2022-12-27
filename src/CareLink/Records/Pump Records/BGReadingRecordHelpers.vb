' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class BGReadingRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
             NameOf(BGReadingRecord.kind),
             NameOf(BGReadingRecord.relativeOffset),
             NameOf(BGReadingRecord.version)
        }

    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Shared Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        If HideColumn(e.Column.Name) Then
            e.Column.Visible = False
            Exit Sub
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(GetCellStyle(e.Column.Name),
                         True,
                         True,
                         caption)
    End Sub

    Private Shared Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridViewView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(BGReadingRecord.dateTime))
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(BGReadingRecord.value), StringComparison.OrdinalIgnoreCase) Then
            Dim sendorValue As Single = e.Value.ToString().ParseSingle
            If Single.IsNaN(sendorValue) Then
                e.CellStyle.BackColor = Color.Gray
            ElseIf sendorValue < s_limitLow Then
                e.CellStyle.BackColor = Color.Red
            ElseIf sendorValue > s_limitHigh Then
                e.CellStyle.BackColor = Color.Yellow
            End If
        End If

    End Sub

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToCoumnAlignment(Of BGReadingRecord)(s_alignmentTable, columnName)
    End Function

End Class
