' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module BannerStateHelpers

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(BannerState.timeRemaining)
                CellFormatting0Value(e)
            Case Else
                dgv.CellFormattingSetForegroundColor(e)
        End Select
    End Sub

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            e.DgvColumnAdded(GetCellStyle(.Name),
                             True,
                             True,
                             CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of BannerState)(s_alignmentTable, columnName)
    End Function

    Friend Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        RemoveHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

End Module
