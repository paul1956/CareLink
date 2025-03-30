' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.IO
Imports System.Text.Json

Imports DataGridViewColumnControls

Public Class InitializeDialog
    Private ReadOnly _fromPdf As Boolean

    Private ReadOnly _insulinTypesBindingSource As New BindingSource(s_insulinTypes, Nothing)

    Private _currentUserBackup As CurrentUserRecord = Nothing

    Public Sub New(currentUser As CurrentUserRecord)
        MyBase.New
        Me.InitializeComponent()
        _CurrentUser = currentUser
        _fromPdf = False
    End Sub

    Public Sub New(currentUser As CurrentUserRecord, ait As Single, currentTarget As Single, CarbRatios As List(Of CarbRatioRecord))
        MyBase.New
        Me.InitializeComponent()
        Me.CurrentUser = currentUser
        Me.CurrentUser.PumpAit = ait
        Me.CurrentUser.CarbRatios = CarbRatios
        Me.CurrentUser.CurrentTarget = currentTarget
        _fromPdf = True
    End Sub

    Public Property CurrentUser As CurrentUserRecord

    Private Shared Sub InitializeComboList(comboBoxCell As DataGridViewComboBoxCell, start As Integer)
        Dim data As New Dictionary(Of String, TimeOnly)
        For i As Integer = start To 47
            Dim t As New TimeOnly(i \ 2, (i Mod 2) * 30)
            data.Add(t.ToHoursMinutes, t)
        Next
        data.Add(s_eleven59Str, s_eleven59)
        comboBoxCell.DataSource = data.ToArray
        comboBoxCell.DisplayMember = "Key"
        comboBoxCell.ValueMember = "Value"
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If _currentUserBackup Is Nothing Then
            If MsgBox("If you cancel, the program will exit", "Retry will allow editing.", MsgBoxStyle.RetryCancel Or MsgBoxStyle.Exclamation, "Exit Or Retry") = MsgBoxResult.Cancel Then
                End
            End If
            Me.PumpAitComboBox.Enabled = True
            Me.DialogResult = DialogResult.None
        Else
            If Not Me.CurrentUser.Equals(_currentUserBackup) Then
                ' TODO Warn editing will be lost
                Me.CurrentUser = _currentUserBackup.Clone
            End If
            Me.DialogResult = DialogResult.OK
        End If
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
                    InitializeComboList(c, CInt(startTime.ToTimeSpan.TotalMinutes / 30))
                    c.Value = s_eleven59
                    c.ReadOnly = False
                    buttonCell = CType(.Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell)
                    buttonCell.ReadOnly = False
                    buttonCell.Enabled = True
                End With

            Case NameOf(ColumnStart)
                dgv.CurrentCell = dgv.Rows(e.RowIndex).Cells(NameOf(ColumnEnd))

            Case NameOf(ColumnEnd)
            Case NameOf(ColumnNumericUpDown)

            Case NameOf(ColumnSave)
                With Me.InitializeDataGridView
                    If .Rows(e.RowIndex).Cells(NameOf(ColumnEnd)).Value.ToString = s_eleven59Str OrElse .RowCount = 12 Then
                        Me.OK_Button.Enabled = True
                        Dim buttonCell As DataGridViewDisableButtonCell = CType(.Rows(.RowCount - 1).Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell)
                        buttonCell.ReadOnly = True
                        buttonCell.Enabled = False
                        Me.InitializeDataGridView.Enabled = False
                        Me.OK_Button.Focus()
                        Exit Sub
                    End If
                    With .Rows(e.RowIndex)
                        CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell).Enabled = False
                        CType(.Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell).Enabled = False
                    End With
                    For Each c As DataGridViewCell In .Rows(e.RowIndex).Cells
                        c.ReadOnly = Not c.OwningColumn.HeaderText = "Carb Ratio g/U"
                    Next
                    .Rows.Add()
                    With .Rows(.Rows.Count - 1)
                        Me.OK_Button.Enabled = False
                        Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnStart)), DataGridViewComboBoxCell)
                        Dim columnEndCell As DataGridViewCell = Me.InitializeDataGridView.Rows(e.RowIndex).Cells(NameOf(ColumnEnd))
                        columnEndCell.ErrorText = ""
                        Dim timeOnly As TimeOnly = TimeOnly.Parse(columnEndCell.Value.ToString)
                        Dim value As String = timeOnly.ToHoursMinutes
                        c.Items.Add(value)
                        c.Value = value
                        c = CType(.Cells(NameOf(ColumnEnd)), DataGridViewComboBoxCell)
                        InitializeComboList(c, CInt(timeOnly.ToTimeSpan.TotalMinutes / 30) + 1)
                        c.Value = s_eleven59
                        .Cells(NameOf(ColumnNumericUpDown)).Value = 15.0
                        CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell).Enabled = True
                    End With

                End With
        End Select

    End Sub

    Private Sub InitializeDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles InitializeDataGridView.DataError
        Stop
    End Sub

    Private Sub InitializeDataGridView_Enter(sender As Object, e As EventArgs) Handles InitializeDataGridView.Enter
        Me.InitializeDataGridView.CausesValidation = True
    End Sub

    Private Sub InitializeDataGridView_Validating(sender As Object, e As CancelEventArgs) Handles InitializeDataGridView.Validating
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(Me.InitializeDataGridView.RowCount - 1).Cells(NameOf(ColumnEnd))
        If cell.Value.ToString = s_midnightStr Then
            cell.ErrorText = ""
        Else
            With Me.InitializeDataGridView
                If .RowCount = 12 Then
                    cell.Value = s_eleven59Str
                    cell.ErrorText = ""
                    Dim buttonCell As DataGridViewDisableButtonCell = CType(.Rows(.RowCount - 1).Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell)
                    buttonCell.ReadOnly = True
                    buttonCell.Enabled = False
                    .Enabled = False
                    Me.OK_Button.Focus()
                Else
                    cell.ErrorText = $"Value must be {s_midnightStr}"
                    .CurrentCell = cell
                    Me.DialogResult = DialogResult.None
                End If
            End With
        End If
    End Sub

    Private Sub InitializeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.ColumnStart.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox
        Me.ColumnEnd.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox

        Me.CurrentUser.CurrentTarget = GetSgTarget()
        With Me.TargetSgComboBox
            .DataSource = If(NativeMmolL,
                             If(Provider.NumberFormat.NumberDecimalSeparator = ".",
                                New BindingSource(MmolLItemsPeriod, Nothing),
                                New BindingSource(MmolLItemsComma, Nothing)
                               ),
                             New BindingSource(MgDlItems, Nothing)
                            )

            .DisplayMember = "Key"
            .ValueMember = "Value"
            .SelectedIndex = Me.TargetSgComboBox.Items.IndexOfValue(Of String, Single)(Me.CurrentUser.CurrentTarget)
            .Enabled = Not (Is700Series() OrElse _fromPdf)
        End With

        Me.Text = $"Initialize CareLink™ For {Me.CurrentUser.UserName}"

        With Me.PumpAitComboBox
            .DataSource = New BindingSource(s_aitLengths, Nothing)
            .DisplayMember = "Key"
            .ValueMember = "Value"
            If Me.CurrentUser.PumpAit = 0 Then
                .SelectedIndex = -1
            Else
                _currentUserBackup = Me.CurrentUser.Clone
                .SelectedIndex = .Items.IndexOfValue(Of String, Single)(Me.CurrentUser.PumpAit)
            End If
            If _fromPdf Then
                .Enabled = False
            Else
                .Enabled = True
                .Focus()
            End If
        End With

        With Me.InsulinTypeComboBox
            .DataSource = _insulinTypesBindingSource
            .DisplayMember = "Key"
            .ValueMember = "Value"
            .Enabled = True
            .SelectedIndex = If(String.IsNullOrWhiteSpace(Me.CurrentUser.InsulinTypeName),
                                -1,
                                .Items.IndexOfKey(Of String, InsulinActivationRecord)(Me.CurrentUser.InsulinTypeName)
                               )
            If _fromPdf Then
                .Focus()
            End If
        End With

        If Is700Series() Then
            Me.UseAITAdvancedDecayCheckBox.CheckState = Me.CurrentUser.UseAdvancedAitDecay
            Me.UseAITAdvancedDecayCheckBox.Enabled = True
        Else
            Me.UseAITAdvancedDecayCheckBox.CheckState = CheckState.Checked
            Me.UseAITAdvancedDecayCheckBox.Enabled = False
        End If

        With Me.InitializeDataGridView
            .Rows.Clear()
            .Enabled = Not _fromPdf
            If Me.CurrentUser.CarbRatios.Count > 0 Then
                For Each i As IndexClass(Of CarbRatioRecord) In Me.CurrentUser.CarbRatios.WithIndex
                    Dim value As CarbRatioRecord = i.Value
                    .Rows.Add()
                    With .Rows(i.Index)
                        Dim buttonCell As DataGridViewDisableButtonCell = CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell)
                        buttonCell.Enabled = i.IsLast
                        Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnStart)), DataGridViewComboBoxCell)
                        c.Items.Add(value.StartTime.ToHoursMinutes)
                        c.Value = value.StartTime.ToHoursMinutes()
                        c.ReadOnly = True
                        c = CType(.Cells(NameOf(ColumnEnd)), DataGridViewComboBoxCell)
                        InitializeComboList(c, CInt((New TimeSpan(value.StartTime.Hour, value.StartTime.Minute, 0) / s_30MinuteSpan) + 1))

                        c.Value = value.EndTime
                        c.ReadOnly = i.Index >= 11 OrElse
                                     (i.IsLast AndAlso Not i.IsFirst)
                        Dim numericCell As DataGridViewNumericUpDownCell = CType(.Cells(NameOf(ColumnNumericUpDown)), DataGridViewNumericUpDownCell)
                        numericCell.Value = value.CarbRatio
                        numericCell.ReadOnly = False
                        buttonCell = CType(.Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell)
                        buttonCell.ReadOnly = False
                        buttonCell.Enabled = i.IsLast
                    End With
                Next
                Me.InitializeDataGridView.Enabled = Not _fromPdf
            Else
                .Rows.Add()
                With .Rows(0)
                    Dim buttonCell As DataGridViewDisableButtonCell = CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell)
                    buttonCell.Enabled = False
                    Dim c As DataGridViewComboBoxCell = CType(.Cells(NameOf(ColumnStart)), DataGridViewComboBoxCell)
                    c.Items.Add(s_midnightStr)
                    c.Value = s_midnightStr
                    c.ReadOnly = True

                    c = CType(.Cells(NameOf(ColumnEnd)), DataGridViewComboBoxCell)
                    InitializeComboList(c, 1)
                    c.Value = s_eleven59
                    Dim numericCell As DataGridViewNumericUpDownCell = CType(.Cells(NameOf(ColumnNumericUpDown)), DataGridViewNumericUpDownCell)
                    numericCell.Value = 15.0
                End With
                Me.InitializeDataGridView.Enabled = False
            End If
            For Each col As DataGridViewColumn In .Columns
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With

        Me.OK_Button.Enabled = _fromPdf AndAlso Me.InsulinTypeComboBox.SelectedIndex >= 0

    End Sub

    Private Sub InsulinTypeComboBox_Enter(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.Enter
        Me.InsulinTypeComboBox.CausesValidation = True
    End Sub

    Private Sub InsulinTypeComboBox_Leave(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.Leave
        Dim c As ComboBox = CType(sender, ComboBox)
        c.Enabled = False

    End Sub

    Private Sub InsulinTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.SelectedIndexChanged
        Dim c As ComboBox = CType(sender, ComboBox)
        If Is700Series() Then
            Me.UseAITAdvancedDecayCheckBox.Enabled = c.SelectedIndex > -1
        Else
            If _fromPdf Then
                Me.OK_Button.Enabled = True
            Else
                Me.InitializeDataGridView.Enabled = True
            End If
        End If
    End Sub

    Private Sub InsulinTypeComboBox_Validating(sender As Object, e As CancelEventArgs) Handles InsulinTypeComboBox.Validating
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex > -1 Then
            Me.ErrorProvider1.SetError(c, "")
        Else
            Me.ErrorProvider1.SetError(c, $"Value must be {s_midnightStr}")
            e.Cancel = True
        End If

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(Me.InitializeDataGridView.RowCount - 1).Cells(NameOf(ColumnEnd))
        cell.ErrorText = ""
        Me.DialogResult = DialogResult.OK

        Me.CurrentUser.PumpAit = ParseSingle(Me.PumpAitComboBox.SelectedValue, decimalDigits:=2)

        Me.CurrentUser.InsulinTypeName = Me.InsulinTypeComboBox.Text
        Me.CurrentUser.InsulinRealAit = CType(Me.InsulinTypeComboBox.SelectedValue, InsulinActivationRecord).AitHours

        Me.CurrentUser.UseAdvancedAitDecay = Me.UseAITAdvancedDecayCheckBox.CheckState
        Me.CurrentUser.CurrentTarget = CType(Me.TargetSgComboBox.SelectedItem, KeyValuePair(Of String, Single)).Value

        Me.CurrentUser.CarbRatios.Clear()

        ' Save all carb ratios
        Dim rowIndex As Integer = 0
        For Each row As DataGridViewRow In Me.InitializeDataGridView.Rows
            Dim carbRecord As New CarbRatioRecord
            cell = row.Cells(NameOf(ColumnStart))
            carbRecord.StartTime = TimeOnly.Parse(cell.Value.ToString, CurrentDateCulture)
            cell = row.Cells(NameOf(ColumnEnd))
            carbRecord.EndTime = TimeOnly.Parse(cell.Value.ToString, CurrentDateCulture)
            Dim numericCell As DataGridViewNumericUpDownCell = CType(row.Cells(NameOf(ColumnNumericUpDown)), DataGridViewNumericUpDownCell)
            carbRecord.CarbRatio = ParseSingle(numericCell.Value, decimalDigits:=1)
            Me.CurrentUser.CarbRatios.Add(carbRecord)
        Next

        File.WriteAllTextAsync(
            path:=GetUserSettingsJsonFileNameWithPath,
            contents:=JsonSerializer.Serialize(Me.CurrentUser, s_jsonSerializerOptions))
        Me.Close()
    End Sub

    Private Sub PumpAitComboBoxComboBox_Leave(sender As Object, e As EventArgs) Handles PumpAitComboBox.Leave
        Dim c As ComboBox = CType(sender, ComboBox)
        c.Enabled = False
    End Sub

    Private Sub PumpAitComboBoxComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PumpAitComboBox.SelectedIndexChanged
        Dim c As ComboBox = CType(sender, ComboBox)
        Me.InsulinTypeComboBox.Enabled = c.SelectedIndex > -1
    End Sub

    Private Sub PumpAitComboBoxComboBox_Validating(sender As Object, e As CancelEventArgs) Handles PumpAitComboBox.Validating
        Dim c As ComboBox = CType(sender, ComboBox)
        If c.SelectedIndex > -1 Then
            Me.ErrorProvider1.SetError(c, "")
            Me.InsulinTypeComboBox.Enabled = True
        Else
            Me.ErrorProvider1.SetError(c, "You must select an AIT Value!")
            c.Enabled = True
            e.Cancel = True
        End If
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Click(sender As Object, e As EventArgs) Handles UseAITAdvancedDecayCheckBox.Click
        Dim chkBox As CheckBox = CType(sender, CheckBox)
        Select Case chkBox.CheckState
            Case CheckState.Indeterminate, CheckState.Unchecked
                chkBox.CheckState = CheckState.Checked
            Case CheckState.Checked
                chkBox.CheckState = CheckState.Unchecked
        End Select
        chkBox.Enabled = chkBox.CheckState = CheckState.Checked
        Dim dgv As DataGridView = Me.InitializeDataGridView
        Dim cell As DataGridViewCell = dgv.Rows(Me.InitializeDataGridView.RowCount - 1).Cells(NameOf(ColumnEnd))
        cell.Value = s_eleven59Str
        Me.InitializeDataGridView.Enabled = True
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Enter(sender As Object, e As EventArgs) Handles UseAITAdvancedDecayCheckBox.Enter
        Me.UseAITAdvancedDecayCheckBox.CausesValidation = True
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Leave(sender As Object, e As EventArgs) Handles UseAITAdvancedDecayCheckBox.Leave
        If Me.UseAITAdvancedDecayCheckBox.CheckState <> CheckState.Indeterminate Then
            Me.InitializeDataGridView.Enabled = True
            Me.UseAITAdvancedDecayCheckBox.Enabled = False
            Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(Me.InitializeDataGridView.RowCount - 1).Cells(NameOf(ColumnEnd))
            Me.InitializeDataGridView.CurrentCell = cell
        End If
    End Sub

End Class
