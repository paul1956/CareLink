' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module BGReadingRecordHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
             NameOf(BGReadingRecord.kind),
             NameOf(BGReadingRecord.relativeOffset),
             NameOf(BGReadingRecord.version)
        }

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
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

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Sub DataGridViewView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(BGReadingRecord.dateTime))
        If dgv.Columns(e.ColumnIndex).Name.Equals(NameOf(BGReadingRecord.value), StringComparison.OrdinalIgnoreCase) Then
            Dim sensorValue As Single = e.Value.ToString().ParseSingle(2)
            If Single.IsNaN(sensorValue) Then
                e.CellStyle.BackColor = Color.Gray
            ElseIf sensorValue < s_limitLow Then
                e.CellStyle.BackColor = Color.Red
            ElseIf sensorValue > s_limitHigh Then
                e.CellStyle.BackColor = Color.Yellow
            End If
            e.CellStyle.ForeColor = e.CellStyle.BackColor.GetContrastingColor()
        End If

    End Sub

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of BGReadingRecord)(s_alignmentTable, columnName)
    End Function

    Private Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Friend Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

End Module
