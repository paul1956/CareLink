' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Class BannerStateRecordHelpers

    Private Shared Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
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
        If e.Value Is Nothing Then
            Return
        End If
        Dim columnName As String = dgv.Columns(e.ColumnIndex).Name
        If columnName.Equals(NameOf(BannerStateRecord.timeRemaining), StringComparison.Ordinal) Then
            If e.Value IsNot Nothing AndAlso e.Value.ToString = "0" Then
                e.Value = ""
            End If
        End If
    End Sub

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(BannerStateRecord.type),
                 NameOf(BannerStateRecord.message)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case NameOf(BannerStateRecord.RecordNumber)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0))
            Case NameOf(BannerStateRecord.timeRemaining)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(0, 1, 1, 1))
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

End Class
