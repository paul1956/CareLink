' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module SgRecordHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(SgRecord.kind),
                        NameOf(SgRecord.OaDateTime),
                        NameOf(SgRecord.relativeOffset),
                        NameOf(SgRecord.version)
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

    ''' <summary>
    ''' Used by LastSG ONLY
    ''' </summary>
    ''' <param columnName="sender"></param>
    ''' <param columnName="e"></param>
    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim columnName As String = dgv.Columns(e.ColumnIndex).Name
        Select Case columnName
            Case NameOf(SgRecord.sensorState)
                ' Set the background to red for negative values in the Balance column.
                If Not e.Value.Equals("NO_ERROR_MESSAGE") Then
                    FormatCell(e, Color.Red)
                End If
            Case NameOf(SgRecord.datetime)
                dgv.dateTimeCellFormatting(e, NameOf(SgRecord.datetime))
            Case NameOf(SgRecord.sg), NameOf(SgRecord.sgMmolL), NameOf(SgRecord.sgMmDl)
                dgv.bgValueCellFormatting(e, NameOf(SgRecord.sg))
        End Select

    End Sub

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of SgRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        RemoveHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

End Module
