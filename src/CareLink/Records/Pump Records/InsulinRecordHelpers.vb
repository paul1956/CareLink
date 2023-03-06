' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module InsulinRecordHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
            NameOf(InsulinRecord.dateTimeAsString),
            NameOf(InsulinRecord.effectiveDuration),
            NameOf(InsulinRecord.id),
            NameOf(InsulinRecord.kind),
            NameOf(InsulinRecord.OAdateTime),
            NameOf(InsulinRecord.relativeOffset),
            NameOf(InsulinRecord.version)
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
        If dgv.Columns(e.ColumnIndex).ValueType = GetType(Single) Then
            Dim value As Single = CSng(e.Value)

            If value <> 0 AndAlso dgv.Columns(e.ColumnIndex).Name = NameOf(InsulinRecord.SafeMealReduction) Then
                e.Value = value.RoundTo025.ToString
                e.CellStyle.ForeColor = Color.OrangeRed
                e.FormattingApplied = True
                Exit Sub
            End If
        End If
        dgv.dgvCellFormatting(e, NameOf(InsulinRecord.dateTime))
    End Sub

    Friend Sub AddHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of InsulinRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

End Module
