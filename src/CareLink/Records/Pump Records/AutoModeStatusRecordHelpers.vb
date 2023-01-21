' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class AutoModeStatusRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
             NameOf(AutoModeStatusRecord.kind),
             NameOf(AutoModeStatusRecord.relativeOffset),
             NameOf(AutoModeStatusRecord.version)
        }

    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Shared Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            If HideColumn(.Name) Then
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

    Private Shared Sub DataGridView_ColumnHeaderCellChanged(sender As Object, e As DataGridViewColumnEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridViewView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(AutoModeStatusRecord.dateTime))
    End Sub

    Friend Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of AutoModeStatusRecord)(s_alignmentTable, columnName)
    End Function

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.ColumnHeaderCellChanged, AddressOf DataGridView_ColumnHeaderCellChanged
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

End Class
