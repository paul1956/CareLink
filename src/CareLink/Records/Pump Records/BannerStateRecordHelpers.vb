' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class BannerStateRecordHelpers

    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

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
        Return ClassPropertiesToColumnAlignment(Of BannerStateRecord)(s_alignmentTable, columnName)
    End Function

End Class
