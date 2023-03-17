' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module LimitsRecordHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
             NameOf(LimitsRecord.kind),
             NameOf(LimitsRecord.version)
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

    Private Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of LimitsRecord)(s_alignmentTable, columnName)
    End Function

    Private Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Friend Sub UpdateListOfLimitRecords(row As KeyValuePair(Of String, String))
        s_listOfLimitRecords.Clear()
        For Each e As IndexClass(Of Dictionary(Of String, String)) In LoadList(row.Value).WithIndex
            Dim newLimit As New Dictionary(Of String, String)
            For Each kvp As KeyValuePair(Of String, String) In e.Value
                Select Case kvp.Key
                    Case "lowLimit", "highLimit"
                        newLimit.Add(kvp.Key, kvp.ScaleSgToString)
                    Case Else
                        newLimit.Add(kvp.Key, kvp.Value)
                End Select
            Next
            s_listOfLimitRecords.Add(DictionaryToClass(Of LimitsRecord)(e.Value, s_listOfLimitRecords.Count + 1))
        Next
    End Sub

    Friend Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

End Module
