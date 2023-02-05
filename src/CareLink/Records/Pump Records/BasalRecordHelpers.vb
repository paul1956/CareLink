' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class BasalRecordHelpers
    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Shared Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.Value Is Nothing Then
            Return
        End If
        ' Set the background to red for negative values in the Balance column.
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(BasalRecord.basalRate), StringComparison.OrdinalIgnoreCase) Then
            e.Value = $"{CSng(e.Value).ToString("F2", CurrentUICulture)} U"
        End If
    End Sub

    Private Shared Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(GetCellStyle(.Name),
                             True,
                             True,
                             CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    Private Shared Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridView_RowAdded(sender As Object, e As DataGridViewRowsAddedEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)

        If s_listOfManualBasal.Count > 1 Then
            Dim currentRow As DataGridViewRow = dgv.Rows(e.RowIndex)
            If s_listOfManualBasal.ToList(e.RowIndex).GetBasal = 0 Then
                currentRow.Visible = False
            End If
        End If
    End Sub

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.RowsAdded, AddressOf DataGridView_RowAdded
    End Sub

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of BasalRecord)(s_alignmentTable, columnName)
    End Function

End Class
