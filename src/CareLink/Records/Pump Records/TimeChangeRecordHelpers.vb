' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeChangeRecordHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
            NameOf(TimeChangeRecord.kind),
            NameOf(TimeChangeRecord.relativeOffset),
            NameOf(TimeChangeRecord.version)
        }

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            If HideColumn(e.Column.Name) Then
                .Visible = False
                Exit Sub
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(GetCellStyle(.Name),
                             True,
                             True,
                             CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Sub DataGridViewView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        CType(sender, DataGridView).dgvCellFormatting(e, NameOf(TimeChangeRecord.dateTime))
    End Sub

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of TimeChangeRecord)(s_alignmentTable, columnName)
    End Function

    Private Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

End Module
