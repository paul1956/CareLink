' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class MealRecordHelpers

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
        NameOf(MealRecord.index),
        NameOf(MealRecord.kind),
        NameOf(MealRecord.relativeOffset),
        NameOf(MealRecord.type),
        NameOf(MealRecord.version)
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
        If dgv.Columns(e.ColumnIndex).Name = NameOf(MealRecord.amount) Then
            e.Value = $"{e.Value} {s_sessionCountrySettings.carbDefaultUnit}"
            e.FormattingApplied = True
        Else
            dgv.dgvCellFormatting(e, NameOf(MealRecord.dateTime))
        End If
    End Sub

    Private Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(MealRecord.[dateTime]),
                    NameOf(MealRecord.dateTimeAsString),
                    NameOf(MealRecord.type)
                cellStyle.CellStyleMiddleLeft
            Case NameOf(MealRecord.RecordNumber),
                    NameOf(MealRecord.kind),
                    NameOf(MealRecord.index),
                    NameOf(MealRecord.amount)
                cellStyle = cellStyle.CellStyleMiddleCenter
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(MealRecord.amount),
                    NameOf(MealRecord.version),
                    NameOf(MealRecord.OAdateTime),
                    NameOf(MealRecord.relativeOffset)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.ColumnHeaderCellChanged, AddressOf DataGridView_ColumnHeaderCellChanged
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

End Class
