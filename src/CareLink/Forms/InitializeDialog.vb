' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports DataGridViewColumnControls

Public Class InitializeDialog
    Private ReadOnly _midnight As String = New TimeOnly(0, 0).ToString

    Private Shared Sub InitializeComboList(items As DataGridViewComboBoxCell.ObjectCollection, start As Integer)
        For i As Integer = start To 47
            Dim t As New TimeOnly(i \ 2, (i Mod 2) * 30)
            items.Add(t.ToString)
        Next
        items.Add("12:00 AM")
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles InitializeDataGridView.DataError
        Stop
    End Sub

    Private Sub InitializeDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles InitializeDataGridView.CellContentClick
        Select Case e.ColumnIndex
            Case 0 ' Delete row
                If e.RowIndex = 0 Then Exit Sub
            Case 1 ' Start
                If e.RowIndex = 0 Then Exit Sub
            Case 2 ' End
                If e.RowIndex = 0 Then Exit Sub
            Case 3 ' U value
                If e.RowIndex = 0 Then Exit Sub
            Case 4 ' Save
                ' Validate Row
                With Me.InitializeDataGridView
                    For Each c As DataGridViewCell In .Rows(e.RowIndex).Cells
                        c.ReadOnly = True
                    Next
                    If .Rows(e.RowIndex).Cells(NameOf(ColumnEnd)).Value.ToString = _midnight Then
                        Me.OK_Button.Select()
                        Exit Sub
                    End If
                    Me.InitializeDataGridView.Rows.Add()
                    With .Rows(.Rows.Count - 1)
                        Dim c As DataGridViewComboBoxCell = CType(.Cells(1), DataGridViewComboBoxCell)
                        Dim timeOnly As TimeOnly = TimeOnly.Parse(Me.InitializeDataGridView.Rows(e.RowIndex).Cells(2).Value.ToString)
                        Dim value As String = timeOnly.ToString
                        c.Items.Add(value)
                        c.Value = value
                        c = CType(.Cells(2), DataGridViewComboBoxCell)
                        InitializeComboList(c.Items, CInt(timeOnly.ToTimeSpan.TotalMinutes / 30))
                        c.Value = _midnight
                        .Cells(3).Value = 15.0
                        CType(.Cells(0), DataGridViewDisableButtonCell).Enabled = False
                    End With
                End With
                Stop
        End Select

    End Sub

    Private Sub InitializeDataGridView_Validating(sender As Object, e As CancelEventArgs) Handles InitializeDataGridView.Validating
        Stop
    End Sub

    Private Sub InitializeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ColumnStart.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        Me.ColumnEnd.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        Dim endList As New List(Of String)

        For i As Integer = 1 To 47
            Dim t As New TimeOnly(i \ 2, (i Mod 2) * 30)
            endList.Add(t.ToString)
        Next
        endList.Add(_midnight)
        Me.InitializeDataGridView.Rows.Add()

        With Me.InitializeDataGridView
            With .Rows(0)
                Dim c As DataGridViewComboBoxCell = CType(.Cells(1), DataGridViewComboBoxCell)
                c.Items.Add(_midnight)
                c.Value = _midnight
                c = CType(.Cells(2), DataGridViewComboBoxCell)
                InitializeComboList(c.Items, 1)
                c.Value = _midnight
                .Cells(3).Value = 15.0
                CType(.Cells(0), DataGridViewDisableButtonCell).Enabled = False
            End With

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        Me.ColumnNumericUpDown.DecimalPlaces = 1
        Me.InitializeDataGridView.CurrentCell = Me.InitializeDataGridView.Rows(0).Cells(2)

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class
