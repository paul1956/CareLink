' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class SgRecordHelpers

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(SgRecord.OAdatetime),
                        NameOf(SgRecord.kind),
                        NameOf(SgRecord.relativeOffset),
                        NameOf(SgRecord.version)
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
        dgv.dgvCellFormatting(e, NameOf(SgRecord.datetime))
    End Sub

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.ColumnHeaderCellChanged, AddressOf DataGridView_ColumnHeaderCellChanged
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(SgRecord.sg),
                 NameOf(SgRecord.relativeOffset),
                 NameOf(SgRecord.OAdatetime)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(10, 1, 1, 1))
            Case NameOf(SgRecord.RecordNumber), NameOf(SgRecord.timeChange),
                 NameOf(SgRecord.kind), NameOf(SgRecord.version)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(1))
            Case NameOf(SgRecord.datetime),
                 NameOf(SgRecord.datetimeAsString),
                 NameOf(SgRecord.Message),
                 NameOf(SgRecord.sensorState)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case Else
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

End Class
