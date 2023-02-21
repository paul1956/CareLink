' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports DataGridViewColumnControls

Public Class InitializeDialog

    Private ReadOnly _insulinTypesBindingSource As New BindingSource(
        New Dictionary(Of String, String) From {
                    {$"Humalog{RegisteredTrademark}/Novolog{RegisteredTrademark}", "04:00"},
                    {$"Lyumjev{RegisteredTrademark}/FIASP{RegisteredTrademark}", "03:00"}
                                                        }, Nothing)

    Private ReadOnly _midnight As String = New TimeOnly(0, 0).ToString

    Private Shared Sub InitializeComboList(items As DataGridViewComboBoxCell.ObjectCollection, start As Integer)
        For i As Integer = start To 47
            Dim t As New TimeOnly(i \ 2, (i Mod 2) * 30)
            items.Add(t.ToString)
        Next
        items.Add("12:00 AM")
    End Sub

    Private Sub AitAdvancedDelayComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles AitAdvancedDelayComboBox.SelectedValueChanged
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex <> -1 Then
            Me.ErrorProvider1.SetError(c, "")
        End If

    End Sub

    Private Sub AitAdvancedDelayComboBox_Validating(sender As Object, e As CancelEventArgs) Handles AitAdvancedDelayComboBox.Validating
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex = -1 Then
            Me.ErrorProvider1.SetError(c, "You must select an AIT Value!")
            e.Cancel = True
        Else
            Me.ErrorProvider1.SetError(c, "")
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub InitializeDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles InitializeDataGridView.CellContentClick
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim cell As DataGridViewCell = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex)
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(ColumnDeleteRow)
                If Not CType(cell, DataGridViewDisableButtonCell).Enabled Then Exit Sub
                dgv.Rows.Remove(dgv.Rows(e.RowIndex))
                Dim currentRow As Integer = e.RowIndex - 1
                With dgv.Rows(currentRow)
                    Dim buttonCell As DataGridViewDisableButtonCell = CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell)
                    buttonCell.Enabled = False
                    buttonCell.ReadOnly = True
                    Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnEnd)), DataGridViewComboBoxCell)
                    Dim startTime As TimeOnly = TimeOnly.Parse(Me.InitializeDataGridView.Rows(currentRow).Cells(NameOf(ColumnEnd)).Value.ToString)
                    Dim value As String = startTime.ToString
                    InitializeComboList(c.Items, CInt(startTime.ToTimeSpan.TotalMinutes / 30))
                    c.Value = _midnight
                    c.ReadOnly = False
                    buttonCell = CType(.Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell)
                    buttonCell.ReadOnly = False
                    buttonCell.Enabled = True
                End With
                Stop

            Case NameOf(ColumnStart)
                dgv.CurrentCell = dgv.Rows(e.RowIndex).Cells(NameOf(ColumnEnd))

            Case NameOf(ColumnEnd)

            Case NameOf(ColumnNumericUpDown)

            Case NameOf(ColumnSave)
                With Me.InitializeDataGridView
                    If .Rows(e.RowIndex).Cells(NameOf(ColumnEnd)).Value.ToString = _midnight Then
                        Me.AitAdvancedDelayComboBox.CausesValidation = True
                        Me.InsulinTypeComboBox.CausesValidation = True
                        If Me.ValidateChildren() Then
                            Me.OK_Button.Focus()
                            Me.OK_Button.Enabled = True
                        End If
                        Exit Sub
                    End If
                    With .Rows(e.RowIndex)
                        CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell).Enabled = False
                        CType(.Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell).Enabled = False
                    End With
                    For Each c As DataGridViewCell In .Rows(e.RowIndex).Cells
                        c.ReadOnly = True
                    Next
                    Me.InitializeDataGridView.Rows.Add()
                    With .Rows(.Rows.Count - 1)
                        Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnStart)), DataGridViewComboBoxCell)
                        Dim timeOnly As TimeOnly = TimeOnly.Parse(Me.InitializeDataGridView.Rows(e.RowIndex).Cells(NameOf(ColumnEnd)).Value.ToString)
                        Dim value As String = timeOnly.ToString
                        c.Items.Add(value)
                        c.Value = value
                        c = CType(.Cells(NameOf(ColumnEnd)), DataGridViewComboBoxCell)
                        InitializeComboList(c.Items, CInt(timeOnly.ToTimeSpan.TotalMinutes / 30) + 1)
                        c.Value = _midnight
                        .Cells(NameOf(ColumnNumericUpDown)).Value = 15.0
                        CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell).Enabled = True
                    End With
                End With
        End Select

    End Sub

    Private Sub InitializeDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles InitializeDataGridView.DataError
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

        With Me.InitializeDataGridView
            .Rows.Add()
            With .Rows(0)
                Dim buttonCell As DataGridViewDisableButtonCell = CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell)
                buttonCell.Enabled = False
                Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnStart)), DataGridViewComboBoxCell)
                c.Items.Add(_midnight)
                c.Value = _midnight
                c.ReadOnly = True

                c = CType(.Cells(NameOf(ColumnEnd)), DataGridViewComboBoxCell)
                InitializeComboList(c.Items, 1)
                c.Value = _midnight
                Dim numericCell As DataGridViewNumericUpDownCell = CType(.Cells(NameOf(ColumnNumericUpDown)), DataGridViewNumericUpDownCell)
                numericCell.Value = 15.0

            End With
            For Each col As DataGridViewColumn In .Columns
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With

        Me.InitializeDataGridView.CurrentCell = Me.InitializeDataGridView.Rows(0).Cells(2)

        With Me.InsulinTypeComboBox
            .DataSource = _insulinTypesBindingSource
            .DisplayMember = "Key"
            .ValueMember = "Value"
            .SelectedIndex = -1
        End With

        With Me.AitAdvancedDelayComboBox
            .DataSource = s_aitItemsBindingSource
            .DisplayMember = "Key"
            .ValueMember = "Value"
            .SelectedIndex = -1
        End With
    End Sub

    Private Sub InsulinTypeComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.SelectedValueChanged
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex <> -1 Then
            Me.ErrorProvider1.SetError(c, "")
        End If

    End Sub

    Private Sub InsulinTypeComboBox_Validating(sender As Object, e As CancelEventArgs) Handles InsulinTypeComboBox.Validating
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex = -1 Then
            Me.ErrorProvider1.SetError(c, "You must select an Insulin Type!")
            e.Cancel = True
        Else
            Me.ErrorProvider1.SetError(c, "")
        End If

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(Me.InitializeDataGridView.RowCount - 1).Cells(NameOf(ColumnEnd))
        If cell.Value.ToString <> _midnight Then
            cell.ErrorText = $"Value must be {_midnight}"
            Me.InitializeDataGridView.CurrentCell = cell
            Me.DialogResult = DialogResult.None
        Else
            cell.ErrorText = ""
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

End Class
