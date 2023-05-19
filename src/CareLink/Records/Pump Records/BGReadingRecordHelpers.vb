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

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dgvCellFormatting(e, NameOf(BGReadingRecord.dateTime))
        Dim columnName As String = dgv.Columns(e.ColumnIndex).Name
        If columnName.StartsWith(NameOf(BGReadingRecord.value), StringComparison.InvariantCultureIgnoreCase) Then
            Dim sensorValue As Single = ParseSingle(e.Value, 2)
            If Single.IsNaN(sensorValue) Then
                FormatCell(e, Color.Gray)
            ElseIf columnName.Equals(NameOf(BGReadingRecord.value), StringComparison.OrdinalIgnoreCase) Then
                e.Value = If(ScalingNeeded, sensorValue.ToString("F2", CurrentDataCulture), e.Value.ToString)
                If sensorValue < TirLowLimit(ScalingNeeded) Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit(ScalingNeeded) Then
                    FormatCell(e, Color.Yellow)
                End If
            ElseIf columnName.Equals(NameOf(BGReadingRecord.valueMmDl), StringComparison.OrdinalIgnoreCase) Then
                e.Value = e.Value.ToString
                If sensorValue < TirLowLimit(False) Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit(False) Then
                    FormatCell(e, Color.Yellow)
                End If
            ElseIf columnName.Equals(NameOf(BGReadingRecord.valueMmolL), StringComparison.OrdinalIgnoreCase) Then
                e.Value = sensorValue.RoundSingle(2).ToString("F2", CurrentDataCulture)
                If sensorValue < TirLowLimit(True) Then
                    FormatCell(e, Color.Red)
                ElseIf sensorValue > TirHighLimit(True) Then
                    FormatCell(e, Color.Yellow)
                End If
            End If
        End If

    End Sub

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

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of BGReadingRecord)(s_alignmentTable, columnName)
    End Function

    Private Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
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
