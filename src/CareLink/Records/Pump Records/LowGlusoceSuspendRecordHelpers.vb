' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Class LowGlusoceSuspendRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
            NameOf(LowGlusoceSuspendRecord.index),
            NameOf(LowGlusoceSuspendRecord.kind),
            NameOf(LowGlusoceSuspendRecord.relativeOffset),
            NameOf(LowGlusoceSuspendRecord.version)
        }

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

    Private Shared Sub DataGridView_ColumnHeaderCellChanged(sender As Object, e As DataGridViewColumnEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridViewView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(ActiveInsulinRecord.datetime))
    End Sub

    Friend Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(LowGlusoceSuspendRecord.[dateTime]),
                 NameOf(LowGlusoceSuspendRecord.dateTimeAsString),
                 NameOf(LowGlusoceSuspendRecord.type)
                cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case NameOf(LowGlusoceSuspendRecord.RecordNumber),
                 NameOf(LowGlusoceSuspendRecord.kind),
                 NameOf(LowGlusoceSuspendRecord.index),
                 NameOf(LowGlusoceSuspendRecord.deliverySuspended)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0))
            Case NameOf(LowGlusoceSuspendRecord.version),
                 NameOf(LowGlusoceSuspendRecord.relativeOffset)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(0, 1, 1, 1))
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
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
