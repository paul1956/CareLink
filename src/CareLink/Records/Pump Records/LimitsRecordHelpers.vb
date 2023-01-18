' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class LimitsRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
             NameOf(LimitsRecord.kind),
             NameOf(LimitsRecord.version)
        }

    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

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

    Private Shared Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Stop
    End Sub

    Private Shared Sub DataGridViewView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
    End Sub

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function

    Friend Shared Sub UpdateListOflimitRecords(row As KeyValuePair(Of String, String))
        s_listOfLimitRecords.Clear()
        For Each e As IndexClass(Of Dictionary(Of String, String)) In LoadList(row.Value).WithIndex
            Dim newLimit As New Dictionary(Of String, String)
            For Each kvp As KeyValuePair(Of String, String) In e.Value
                Select Case kvp.Key
                    Case "lowLimit", "highLimit"
                        newLimit.Add(kvp.Key, kvp.scaleValue(1))
                    Case Else
                        newLimit.Add(kvp.Key, kvp.Value)
                End Select
            Next
            s_listOfLimitRecords.Add(DictionaryToClass(Of LimitsRecord)(e.Value, s_listOfLimitRecords.Count + 1))
        Next
    End Sub

    Public Shared Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridViewView_CellFormatting
    End Sub

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of LimitsRecord)(s_alignmentTable, columnName)
    End Function

End Class
