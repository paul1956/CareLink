' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module MealRecordHelpers

    Private ReadOnly s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private ReadOnly s_columnsToHide As New List(Of String) From {
                                NameOf(MealRecord.kind),
                                NameOf(MealRecord.relativeOffset),
                                NameOf(MealRecord.OAdateTime),
                                NameOf(MealRecord.version)
                            }

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        dgv.dateTimeCellFormatting(e, NameOf(CalibrationRecord.dateTime))
    End Sub

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        With e.Column
            If HideColumn(.Name) Then
                .Visible = False
            Else
                e.DgvColumnAdded(GetCellStyle(.Name),
                             True,
                             True,
                             CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Friend Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithExcel
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        RemoveHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithExcel
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of MealRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Function TryGetMealRecord(index As Integer, ByRef meal As MealRecord) As Boolean
        For Each m As MealRecord In s_listOfMealMarkers
            If m.index = index Then
                meal = m
                Return True
            End If
        Next
        Return False
    End Function

End Module
