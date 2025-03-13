' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module InsulinHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
            NameOf(Insulin.effectiveDuration)}

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(Insulin.timestamp)
                dgv.CellFormattingDateTime(e)
            Case NameOf(Insulin.safeMealReduction)
                If dgv.CellFormattingSingleValue(e, digits:=3) >= 0.0025 Then
                    CellFormattingApplyColor(e, highlightColor:=Color.OrangeRed, isUri:=False)
                Else
                    dgv.CellFormattingSetForegroundColor(e)
                End If
            Case NameOf(Insulin.ActivationType)
                Dim value As String = e.Value.ToString
                Select Case value
                    Case "AUTOCORRECTION"
                        e.Value = "Auto Correction"
                        CellFormattingApplyColor(e, GetGraphLineColor("Auto Correction"), isUri:=False)
                    Case "FAST", "RECOMMENDED", "UNDETERMINED"
                        dgv.CellFormattingToTitle(e)
                    Case Else
                        dgv.CellFormattingSetForegroundColor(e)
                End Select
            Case NameOf(Insulin.BolusType)
                dgv.CellFormattingToTitle(e)
            Case Else
                If dgv.Columns(e.ColumnIndex).ValueType = GetType(Single) Then
                    dgv.CellFormattingSingleValue(e, 3)
                Else
                    dgv.CellFormattingSetForegroundColor(e)
                End If
        End Select
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
        Return ClassPropertiesToColumnAlignment(Of Insulin)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

End Module
