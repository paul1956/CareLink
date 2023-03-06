' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module MealRecordHelpers

    Private ReadOnly columnsToHide As New List(Of String) From {
        NameOf(MealRecord.kind),
        NameOf(MealRecord.relativeOffset),
        NameOf(MealRecord.version)
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
        If dgv.Columns(e.ColumnIndex).Name = NameOf(MealRecord.amount) Then
            e.Value = $"{e.Value} {s_sessionCountrySettings.carbDefaultUnit}"
            e.FormattingApplied = True
        Else
            dgv.dgvCellFormatting(e, NameOf(MealRecord.dateTime))
        End If
    End Sub

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of MealRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(dataPropertyName)
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

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
