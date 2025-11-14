' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.IO
Imports System.Text.Json

Imports DataGridViewColumnControls

Public Class InitializeDialog
    Private ReadOnly _fromPdf As Boolean

    Private ReadOnly _insulinTypesBindingSource As _
        New BindingSource(dataSource:=s_insulinTypes, dataMember:=Nothing)

    Private _currentUserBackup As CurrentUserRecord = Nothing

    Public Sub New(ait As Single, currentTarget As Single, CarbRatios As List(Of CarbRatioRecord))
        MyBase.New
        Me.InitializeComponent()
        Me.CurrentUser = SystemVariables.CurrentUser
        Me.CurrentUser.PumpAit = ait
        Me.CurrentUser.CarbRatios = CarbRatios
        Me.CurrentUser.CurrentTarget = currentTarget
        _fromPdf = True
    End Sub

    Public Property CurrentUser As CurrentUserRecord

    Private Shared Sub InitializeComboList(comboBoxCell As DataGridViewComboBoxCell, start As Integer)
        Dim data As New Dictionary(Of String, TimeOnly)
        For i As Integer = start To 47
            Dim value As New TimeOnly(hour:=i \ 2, minute:=(i Mod 2) * 30)
            data.Add(key:=value.ToHoursMinutes, value)
        Next
        data.Add(key:=Eleven59Str, value:=Eleven59)
        comboBoxCell.DataSource = data.ToArray
        comboBoxCell.DisplayMember = "Key"
        comboBoxCell.ValueMember = "Value"
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If _currentUserBackup Is Nothing Then
            If MsgBox(
                heading:="If you cancel, the program will exit",
                prompt:="Retry will allow editing.",
                buttonStyle:=MsgBoxStyle.RetryCancel Or MsgBoxStyle.Exclamation,
                title:="Exit Or Retry") = MsgBoxResult.Cancel Then

                End
            End If
            Me.PumpAitComboBox.Enabled = True
            Me.DialogResult = DialogResult.None
        Else
            If Not Me.CurrentUser.Equals(other:=_currentUserBackup) Then
                ' TODO Warn editing will be lost
                Me.CurrentUser = _currentUserBackup.Clone
            End If
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub InitializeDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles InitializeDataGridView.CellContentClick

        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim cell As DataGridViewCell
        Dim start As Integer
        Dim colName As String = dgv.Columns(index:=e.ColumnIndex).Name
        Dim index As Integer
        Select Case colName
            Case NameOf(ColumnDeleteRow)
                cell = dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex)
                If Not CType(cell, DataGridViewDisableButtonCell).Enabled Then
                    Exit Sub
                End If
                index = e.RowIndex
                dgv.Rows.Remove(dataGridViewRow:=dgv.Rows(index))
                index -= 1
                With dgv.Rows(index)
                    colName = NameOf(ColumnDeleteRow)
                    Dim btnCell As DataGridViewDisableButtonCell = CType(.Cells(colName), DataGridViewDisableButtonCell)
                    btnCell.Enabled = False
                    btnCell.ReadOnly = True
                    colName = NameOf(ColumnEnd)
                    Dim comboBoxCell As DataGridViewComboBoxCell = CType(.Cells(colName), DataGridViewComboBoxCell)
                    colName = NameOf(ColumnEnd)
                    Dim dgvRow As DataGridViewRow = Me.InitializeDataGridView.Rows(index)
                    Dim startTime As TimeOnly = TimeOnly.Parse(s:=dgvRow.Cells(colName).Value.ToString)
                    start = CInt(startTime.ToTimeSpan.TotalMinutes / 30)
                    InitializeComboList(comboBoxCell, start)
                    comboBoxCell.Value = Eleven59
                    comboBoxCell.ReadOnly = False
                    colName = NameOf(ColumnSave)
                    btnCell = CType(.Cells(colName), DataGridViewDisableButtonCell)
                    btnCell.ReadOnly = False
                    btnCell.Enabled = True
                End With

            Case NameOf(ColumnStart)
                dgv.CurrentCell = dgv.Rows(index:=e.RowIndex).Cells(columnName:=NameOf(ColumnEnd))

            Case NameOf(ColumnEnd)
            Case NameOf(ColumnNumericUpDown)

            Case NameOf(ColumnSave)
                With Me.InitializeDataGridView
                    colName = NameOf(ColumnEnd)
                    cell = Me.InitializeDataGridView.Rows(index:=e.RowIndex).Cells(colName)
                    If cell.Value.ToString = Eleven59Str OrElse .RowCount = 12 Then
                        Me.OK_Button.Enabled = True
                        colName = NameOf(ColumnSave)
                        index = .RowCount - 1
                        Dim dgvCell As DataGridViewCell = .Rows(index).Cells(colName)
                        Dim buttonCell As DataGridViewDisableButtonCell = CType(dgvCell, DataGridViewDisableButtonCell)

                        buttonCell.ReadOnly = True
                        buttonCell.Enabled = False
                        Me.InitializeDataGridView.Enabled = False
                        Me.OK_Button.Focus()
                        Exit Sub
                    End If
                    With .Rows(index:=e.RowIndex)
                        CType(.Cells(NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell).Enabled = False
                        CType(.Cells(NameOf(ColumnSave)), DataGridViewDisableButtonCell).Enabled = False
                    End With
                    For Each c As DataGridViewCell In .Rows(index:=e.RowIndex).Cells
                        c.ReadOnly = Not c.OwningColumn.HeaderText = "Carb Ratio g/U"
                    Next
                    .Rows.Add()
                    With .Rows(index:= .Rows.Count - 1)
                        Me.OK_Button.Enabled = False
                        colName = NameOf(ColumnStart)
                        Dim comboBoxCell As DataGridViewComboBoxCell = CType(.Cells(colName), DataGridViewComboBoxCell)
                        colName = NameOf(ColumnEnd)
                        Dim colEndCell As DataGridViewCell = Me.InitializeDataGridView.Rows(index:=e.RowIndex) _
                                                                                      .Cells(colName)
                        colEndCell.ErrorText = ""
                        Dim timeOnly As TimeOnly = TimeOnly.Parse(colEndCell.Value.ToString)
                        Dim item As String = timeOnly.ToHoursMinutes
                        comboBoxCell.Items.Add(item)
                        comboBoxCell.Value = item
                        colName = NameOf(ColumnEnd)
                        comboBoxCell = CType(.Cells(colName), DataGridViewComboBoxCell)
                        start = CInt(timeOnly.ToTimeSpan.TotalMinutes / 30) + 1
                        InitializeComboList(comboBoxCell, start)
                        comboBoxCell.Value = Eleven59
                        colName = NameOf(ColumnNumericUpDown)
                        .Cells(colName).Value = 15.0
                        colName = NameOf(ColumnDeleteRow)
                        CType(.Cells(colName), DataGridViewDisableButtonCell).Enabled = True
                    End With

                End With
        End Select
    End Sub

    Private Sub InitializeDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) _
        Handles InitializeDataGridView.DataError

        Stop
    End Sub

    Private Sub InitializeDataGridView_Enter(sender As Object, e As EventArgs) Handles InitializeDataGridView.Enter
        Me.InitializeDataGridView.CausesValidation = True
    End Sub

    Private Sub InitializeDataGridView_Validating(sender As Object, e As CancelEventArgs) _
        Handles InitializeDataGridView.Validating

        Dim index As Integer = Me.InitializeDataGridView.RowCount - 1
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(index).Cells(columnName:=NameOf(ColumnEnd))
        If cell.Value.ToString = MidnightStr Then
            cell.ErrorText = ""
        Else
            With Me.InitializeDataGridView
                If .RowCount = 12 Then
                    cell.Value = Eleven59Str
                    cell.ErrorText = ""
                    Dim obj As Object = .Rows(index).Cells(columnName:=NameOf(ColumnSave))
                    Dim buttonCell As DataGridViewDisableButtonCell = CType(obj, DataGridViewDisableButtonCell)
                    buttonCell.ReadOnly = True
                    buttonCell.Enabled = False
                    .Enabled = False
                    Me.OK_Button.Focus()
                Else
                    cell.ErrorText = $"Value must be {MidnightStr}"
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
                             If(DecimalSeparator = CareLinkDecimalSeparator,
                                New BindingSource(dataSource:=MmolLItemsPeriod, dataMember:=Nothing),
                                New BindingSource(dataSource:=MmolLItemsComma, dataMember:=Nothing)),
                             New BindingSource(dataSource:=MgDlItems, dataMember:=Nothing))

            .DisplayMember = "Key"
            .ValueMember = "Value"
            Dim currentTarget As Single = Me.CurrentUser.CurrentTarget
            .SelectedIndex = Me.TargetSgComboBox.Items.IndexOfY(Of String, Single)(y:=currentTarget)
            .Enabled = Not (Is700Series() OrElse _fromPdf)
        End With

        Me.Text = $"Initialize CareLink™ For {Me.CurrentUser.UserName}"

        With Me.PumpAitComboBox
            .DataSource = New BindingSource(dataSource:=s_aitLengths, dataMember:=Nothing)
            .DisplayMember = "Key"
            .ValueMember = "Value"
            If Me.CurrentUser.PumpAit = 0 Then
                .SelectedIndex = -1
            Else
                _currentUserBackup = Me.CurrentUser.Clone
                .SelectedIndex = .Items.IndexOfY(Of String, Single)(y:=Me.CurrentUser.PumpAit)
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
            Dim key As String = Me.CurrentUser.InsulinTypeName
            .SelectedIndex = If(String.IsNullOrWhiteSpace(value:=key),
                                -1,
                               .Items.IndexOfKey(Of String, InsulinActivationRecord)(key))
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
            For Each col As DataGridViewColumn In .Columns
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            If Me.CurrentUser.CarbRatios.Count > 0 Then
                For Each i As IndexClass(Of CarbRatioRecord) In
                    Me.CurrentUser.CarbRatios.WithIndex

                    Dim value As CarbRatioRecord = i.Value
                    .Rows.Add()
                    With .Rows(i.Index)
                        Dim colName As String = NameOf(ColumnDeleteRow)
                        Dim cell As DataGridViewDisableButtonCell = CType(.Cells(colName), DataGridViewDisableButtonCell)
                        cell.Enabled = i.IsLast
                        colName = NameOf(ColumnStart)
                        With CType(.Cells(colName), DataGridViewComboBoxCell)
                            .Items.Add(item:=value.StartTime.ToHoursMinutes)
                            .Value = value.StartTime.ToHoursMinutes()
                            .ReadOnly = True
                        End With
                        Dim timeSpan As New TimeSpan(hours:=value.StartTime.Hour,
                                                     minutes:=value.StartTime.Minute,
                                                     seconds:=0)
                        colName = NameOf(ColumnEnd)
                        Dim comboBoxCell As DataGridViewComboBoxCell = CType(.Cells(colName), DataGridViewComboBoxCell)
                        InitializeComboList(comboBoxCell, start:=CInt((timeSpan / ThirtyMinuteSpan) + 1))
                        With comboBoxCell
                            .Value = value.EndTime
                            .ReadOnly = i.Index >= 11 OrElse (i.IsLast AndAlso Not i.IsFirst)
                        End With
                        colName = NameOf(ColumnNumericUpDown)
                        Dim numericCell As DataGridViewNumericUpDownCell = CType(.Cells(colName), DataGridViewNumericUpDownCell)
                        numericCell.Value = value.CarbRatio
                        numericCell.ReadOnly = False
                        colName = NameOf(ColumnSave)
                        With CType(.Cells(colName), DataGridViewDisableButtonCell)
                            .ReadOnly = False
                            .Enabled = i.IsLast
                        End With
                    End With
                Next
                Me.InitializeDataGridView.Enabled = Not _fromPdf
            Else
                .Rows.Add()
                With .Rows(index:=0)
                    Dim colName As String = NameOf(ColumnDeleteRow)
                    CType(.Cells(colName), DataGridViewDisableButtonCell).Enabled = False
                    colName = NameOf(ColumnStart)
                    With CType(.Cells(colName), DataGridViewComboBoxCell)
                        .Items.Add(item:=MidnightStr)
                        .Value = MidnightStr
                        .ReadOnly = True
                    End With
                    colName = NameOf(ColumnEnd)
                    Dim comboBoxCell As DataGridViewComboBoxCell = CType(.Cells(colName), DataGridViewComboBoxCell)
                    InitializeComboList(comboBoxCell, start:=1)

                    colName = NameOf(ColumnEnd)
                    CType(.Cells(colName), DataGridViewComboBoxCell).Value = Eleven59

                    colName = NameOf(ColumnNumericUpDown)
                    CType(.Cells(colName), DataGridViewNumericUpDownCell).Value = 15.0
                End With
                Me.InitializeDataGridView.Enabled = False
            End If
        End With

        Me.OK_Button.Enabled = _fromPdf AndAlso Me.InsulinTypeComboBox.SelectedIndex >= 0
    End Sub

    Private Sub InsulinTypeComboBox_Enter(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.Enter
        Me.InsulinTypeComboBox.CausesValidation = True
    End Sub

    Private Sub InsulinTypeComboBox_Leave(sender As Object, e As EventArgs) Handles InsulinTypeComboBox.Leave
        CType(sender, ComboBox).Enabled = False
    End Sub

    Private Sub InsulinTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles InsulinTypeComboBox.SelectedIndexChanged

        Dim comboBox As ComboBox = CType(sender, ComboBox)
        If Is700Series() Then
            Me.UseAITAdvancedDecayCheckBox.Enabled = comboBox.SelectedIndex > -1
        Else
            If _fromPdf Then
                Me.OK_Button.Enabled = True
            Else
                Me.InitializeDataGridView.Enabled = True
            End If
        End If
    End Sub

    Private Sub InsulinTypeComboBox_Validating(sender As Object, e As CancelEventArgs) _
        Handles InsulinTypeComboBox.Validating

        Dim control As ComboBox = CType(sender, ComboBox)
        If control.SelectedIndex > -1 Then
            Me.ErrorProvider1.SetError(control, value:="")
        Else
            Me.ErrorProvider1.SetError(control, value:=$"Value must be {MidnightStr}")
            e.Cancel = True
        End If

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim index As Integer = Me.InitializeDataGridView.RowCount - 1
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(index).Cells(columnName:=NameOf(ColumnEnd))
        cell.ErrorText = ""
        Me.DialogResult = DialogResult.OK

        Me.CurrentUser.PumpAit = ParseSingle(value:=Me.PumpAitComboBox.SelectedValue, digits:=2)

        Me.CurrentUser.InsulinTypeName = Me.InsulinTypeComboBox.Text
        Me.CurrentUser.InsulinRealAit = CType(Me.InsulinTypeComboBox.SelectedValue, InsulinActivationRecord).AitHours

        Me.CurrentUser.UseAdvancedAitDecay = Me.UseAITAdvancedDecayCheckBox.CheckState
        Me.CurrentUser.CurrentTarget = CType(Me.TargetSgComboBox.SelectedItem, KeyValuePair(Of String, Single)).Value

        Me.CurrentUser.CarbRatios.Clear()

        ' Save all carb ratios
        Dim rowIndex As Integer = 0
        For Each row As DataGridViewRow In Me.InitializeDataGridView.Rows
            Dim carbRecord As New CarbRatioRecord
            Dim colName As String = NameOf(ColumnStart)
            cell = row.Cells(colName)
            carbRecord.StartTime = TimeOnly.Parse(cell.Value.ToString, provider:=CurrentDateCulture)
            colName = NameOf(ColumnEnd)
            cell = row.Cells(colName)
            carbRecord.EndTime = TimeOnly.Parse(cell.Value.ToString, provider:=CurrentDateCulture)
            colName = NameOf(ColumnNumericUpDown)
            Dim numericCell As DataGridViewNumericUpDownCell = CType(row.Cells(colName), DataGridViewNumericUpDownCell)
            carbRecord.CarbRatio = ParseSingle(numericCell.Value, digits:=1)
            Me.CurrentUser.CarbRatios.Add(carbRecord)
        Next

        Dim contents As String = JsonSerializer.Serialize(value:=Me.CurrentUser, options:=s_jsonSerializerOptions)
        File.WriteAllTextAsync(path:=GetUserSettingsPath(), contents)
        Me.Close()
    End Sub

    Private Sub PumpAitComboBoxComboBox_Leave(sender As Object, e As EventArgs) Handles PumpAitComboBox.Leave
        CType(sender, Control).Enabled = False
    End Sub

    Private Sub PumpAitComboBoxComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles PumpAitComboBox.SelectedIndexChanged

        Me.InsulinTypeComboBox.Enabled = CType(sender, ComboBox).SelectedIndex > -1
    End Sub

    Private Sub PumpAitComboBoxComboBox_Validating(sender As Object, e As CancelEventArgs) _
        Handles PumpAitComboBox.Validating

        Dim control As ComboBox = CType(sender, ComboBox)
        If control.SelectedIndex > -1 Then
            Me.ErrorProvider1.SetError(control, value:="")
            Me.InsulinTypeComboBox.Enabled = True
        Else
            Me.ErrorProvider1.SetError(control, value:="You must select an AIT Value!")
            control.Enabled = True
            e.Cancel = True
        End If
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Click(sender As Object, e As EventArgs) _
        Handles UseAITAdvancedDecayCheckBox.Click

        Dim chkBox As CheckBox = CType(sender, CheckBox)
        Select Case chkBox.CheckState
            Case CheckState.Indeterminate, CheckState.Unchecked
                chkBox.CheckState = CheckState.Checked
            Case CheckState.Checked
                chkBox.CheckState = CheckState.Unchecked
        End Select
        chkBox.Enabled = chkBox.CheckState = CheckState.Checked
        Dim dgv As DataGridView = Me.InitializeDataGridView
        Dim colName As String = NameOf(ColumnEnd)
        Dim cell As DataGridViewCell = dgv.Rows(index:=Me.InitializeDataGridView.RowCount - 1).Cells(colName)
        cell.Value = Eleven59Str
        Me.InitializeDataGridView.Enabled = True
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Enter(sender As Object, e As EventArgs) _
        Handles UseAITAdvancedDecayCheckBox.Enter

        Me.UseAITAdvancedDecayCheckBox.CausesValidation = True
    End Sub

    Private Sub UseAITAdvancedDecayCheckBox_Leave(sender As Object, e As EventArgs) _
        Handles UseAITAdvancedDecayCheckBox.Leave

        If Me.UseAITAdvancedDecayCheckBox.CheckState <> CheckState.Indeterminate Then
            Me.InitializeDataGridView.Enabled = True
            Me.UseAITAdvancedDecayCheckBox.Enabled = False

            Dim colName As String = NameOf(ColumnEnd)
            Dim index As Integer = Me.InitializeDataGridView.RowCount - 1
            Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(index).Cells(colName)
            Me.InitializeDataGridView.CurrentCell = cell
        End If
    End Sub

    ''' <summary>
    '''  Overrides the OnHandleCreated method to enable dark mode
    '''  for the dialog when its handle is created.
    ''' </summary>
    ''' <param name="e">The event data.</param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        EnableDarkMode(hwnd:=Me.Handle)
    End Sub

End Class
