' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module SgHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(SG.Kind),
                        NameOf(SG.OaDateTime),
                        NameOf(SG.Version)
                    }

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

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
        Dim alternateIndex As Integer = If(dgv.Rows(0).Cells(0).Value.ToString = "1", 0, 1)
        Select Case columnName
            Case NameOf(SG.sensorState)
                ' Set the background to red for negative values in the Balance column.
                If Not e.Value.Equals("NO_ERROR_MESSAGE") Then
                    CellFormattingApplyColor(e, Color.Red, isUri:=False)
                End If
                dgv.CellFormattingToTitle(e)
            Case NameOf(SG.Timestamp)
                dgv.CellFormattingDateTime(e)
            Case NameOf(SG.sg), NameOf(SG.sgMmolL), NameOf(SG.sgMmDl)
                dgv.CellFormattingSgValue(e, NameOf(SG.sg))
            Case Else
                dgv.CellFormattingSetForegroundColor(e)
        End Select

    End Sub

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of SG)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        RemoveHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithExcel
        RemoveHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        RemoveHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        RemoveHandler dgv.DataError, AddressOf DataGridView_DataError
        AddHandler dgv.CellContextMenuStripNeeded, AddressOf Form1.Dgv_CellContextMenuStripNeededWithExcel
        AddHandler dgv.CellFormatting, AddressOf DataGridView_CellFormatting
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
        AddHandler dgv.DataError, AddressOf DataGridView_DataError
    End Sub

End Module
