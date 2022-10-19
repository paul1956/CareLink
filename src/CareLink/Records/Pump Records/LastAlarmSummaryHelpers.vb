' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module LastAlarmSummaryHelpers
    Private _mapping As New Dictionary(Of String, String)

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(SummaryHelpers.GetCellStyle(e.Column.Name),
                        False,
                        True,
                        caption)
    End Sub

    Public Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
    End Sub

    Public Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(SummaryRecord.RecordNumber)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(SummaryRecord.Key),
                NameOf(SummaryRecord.Message)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(SummaryRecord.Value)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case Else
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Public Function GetDisplayName(name As String) As String
        If _mapping.Count = 0 Then
            _mapping = ClassPropertiesToDisplayNames(Of LastAlarmRecord)()
        End If
        Return _mapping(name)
    End Function

End Module
