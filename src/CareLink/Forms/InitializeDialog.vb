' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports DataGridViewColumnControls

Public Class InitializeDialog

    Private ReadOnly _aitBindingSource As New BindingSource(
        New Dictionary(Of String, TimeSpan) From {
                    {"AIT 2:00", New TimeSpan(2, 0, 0)},
                    {"AIT 2:15", New TimeSpan(2, 15, 0)},
                    {"AIT 2:30", New TimeSpan(2, 30, 0)},
                    {"AIT 2:45", New TimeSpan(2, 45, 0)},
                    {"AIT 3:00", New TimeSpan(3, 0, 0)},
                    {"AIT 3:15", New TimeSpan(3, 15, 0)},
                    {"AIT 3:30", New TimeSpan(3, 30, 0)},
                    {"AIT 3:45", New TimeSpan(3, 45, 0)},
                    {"AIT 4:00", New TimeSpan(4, 0, 0)},
                    {"AIT 4:15", New TimeSpan(4, 15, 0)},
                    {"AIT 4:30", New TimeSpan(4, 30, 0)},
                    {"AIT 4:45", New TimeSpan(4, 45, 0)},
                    {"AIT 5:00", New TimeSpan(5, 0, 0)},
                    {"AIT 5:15", New TimeSpan(5, 15, 0)},
                    {"AIT 5:30", New TimeSpan(5, 30, 0)},
                    {"AIT 5:45", New TimeSpan(5, 45, 0)},
                    {"AIT 6:00", New TimeSpan(6, 0, 0)}}, Nothing)

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

    Private Sub AitAdvancedDelayComboBox_Leave(sender As Object, e As EventArgs) Handles AitAdvancedDelayComboBox.Leave
        Dim c As ComboBox = CType(sender, ComboBox)
        c.Enabled = False
        Me.InsulinTypeComboBox.CausesValidation = True
    End Sub

    Private Sub AitAdvancedDelayComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AitAdvancedDelayComboBox.SelectedIndexChanged
        Dim c As ComboBox = CType(sender, ComboBox)
        Me.InsulinTypeComboBox.Enabled = c.SelectedIndex > -1
    End Sub

    Private Sub AitAdvancedDelayComboBox_Validating(sender As Object, e As CancelEventArgs) Handles AitAdvancedDelayComboBox.Validating
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex > -1 Then
            Me.ErrorProvider1.SetError(c, "")
            Me.InsulinTypeComboBox.Enabled = True
            e.Cancel = False
        Else
            Me.ErrorProvider1.SetError(c, "You must select an AIT Value!")
            c.Enabled = True
            e.Cancel = True
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If Me.Cancel_Button.Text = "Confirm Exit!" Then End
        If MsgBox("If you continue, the program will exit!", MsgBoxStyle.RetryCancel, "Exit or Retry") = MsgBoxResult.Cancel Then
            End
        End If
        Me.DialogResult = DialogResult.None
    End Sub

    Private Sub Cancel_Button_GotFocus(sender As Object, e As EventArgs) Handles Cancel_Button.GotFocus
        Me.Cancel_Button.Text = "Confirm Exit!"
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
                Stop
            Case NameOf(ColumnNumericUpDown)

            Case NameOf(ColumnSave)
                With Me.InitializeDataGridView
                    If .Rows(e.RowIndex).Cells(NameOf(ColumnEnd)).Value.ToString = _midnight Then
                        Me.OK_Button.Enabled = True
                        Me.OK_Button.Focus()
                        Me.InitializeDataGridView.Enabled = False
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
                        Me.OK_Button.Enabled = False
                        Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnStart)), DataGridViewComboBoxCell)
                        Dim columnEndCell As DataGridViewCell = Me.InitializeDataGridView.Rows(e.RowIndex).Cells(NameOf(ColumnEnd))
                        columnEndCell.ErrorText = ""
                        Dim timeOnly As TimeOnly = TimeOnly.Parse(columnEndCell.Value.ToString)
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

        Me.Text = $"Initialize CareLink{TmChar} For {My.Settings.CareLinkUserName}"

        With Me.InsulinTypeComboBox
            .DataSource = _insulinTypesBindingSource
            .DisplayMember = "Key"
            .ValueMember = "Value"
            .SelectedIndex = -1
        End With

        With Me.AitAdvancedDelayComboBox
            .DataSource = _aitBindingSource
            .DisplayMember = "Key"
            .ValueMember = "Value"
            .SelectedIndex = -1
            .Focus()
        End With

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

    End Sub

    Private Sub InsulinTypeComboBox_Leave(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.Leave
        Dim c As ComboBox = CType(sender, ComboBox)
        c.Enabled = False
        Me.UseAITAdvancedDecayCheckBox.CausesValidation = True
    End Sub

    Private Sub InsulinTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.SelectedIndexChanged
        Dim c As ComboBox = CType(sender, ComboBox)
        Me.UseAITAdvancedDecayCheckBox.Enabled = c.SelectedIndex > -1
    End Sub

    Private Sub InsulinTypeComboBox_Validating(sender As Object, e As CancelEventArgs) Handles InsulinTypeComboBox.Validating
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex > -1 Then
            Me.ErrorProvider1.SetError(c, "")
            e.Cancel = False
        Else
            Me.ErrorProvider1.SetError(c, $"Value must be {_midnight}")
            e.Cancel = True
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

    Private Sub UseAITAdvancedDecayCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles UseAITAdvancedDecayCheckBox.CheckedChanged
        Me.InitializeDataGridView.Enabled = Me.UseAITAdvancedDecayCheckBox.CheckState <> CheckState.Indeterminate
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Click(sender As Object, e As EventArgs) Handles UseAITAdvancedDecayCheckBox.Click
        Dim chkBox As CheckBox = CType(sender, CheckBox)
        Select Case chkBox.CheckState
            Case CheckState.Indeterminate, CheckState.Unchecked
                chkBox.CheckState = CheckState.Checked
            Case CheckState.Checked
                chkBox.CheckState = CheckState.Unchecked
        End Select
        Me.InitializeDataGridView.Enabled = True
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Leave(sender As Object, e As EventArgs) Handles UseAITAdvancedDecayCheckBox.Leave
        If Me.UseAITAdvancedDecayCheckBox.CheckState <> CheckState.Indeterminate Then
            Me.InitializeDataGridView.Enabled = True
            Me.UseAITAdvancedDecayCheckBox.Enabled = False
            Me.InitializeDataGridView.CurrentCell = Me.InitializeDataGridView.Rows(0).Cells(NameOf(ColumnEnd))
        End If
    End Sub

End Class
