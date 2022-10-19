' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class AutoModeStatusRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
             NameOf(AutoModeStatusRecord.kind), NameOf(AutoModeStatusRecord.version),
             NameOf(AutoModeStatusRecord.relativeOffset), NameOf(AutoModeStatusRecord.index)
        }

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.ColumnHeaderCellChanged, AddressOf DataGridView_ColumnHeaderCellChanged
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

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
        dgv.dgvCellFormatting(e, NameOf(AutoModeStatusRecord.dateTime))
    End Sub

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(AutoModeStatusRecord.kind), NameOf(AutoModeStatusRecord.type),
             NameOf(AutoModeStatusRecord.dateTime), NameOf(AutoModeStatusRecord.dateTimeAsString)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(AutoModeStatusRecord.RecordNumber),
                 NameOf(AutoModeStatusRecord.autoModeOn)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(AutoModeStatusRecord.index),
                 NameOf(AutoModeStatusRecord.relativeOffset),
                 NameOf(AutoModeStatusRecord.version)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function
End Class
