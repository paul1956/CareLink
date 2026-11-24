' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.IO

Imports DataGridViewColumnControls

Public Class InitializePumpSettingsDialog
    Private ReadOnly _fromPdf As Boolean

    Private ReadOnly _insulinTypesBindingSource As _
        New BindingSource(dataSource:=s_insulinTypes, dataMember:=Nothing)

    Private _currentUserBackup As CurrentUserRecord = Nothing

    Public Sub New(CurrentUserRecord As CurrentUserRecord, newUser As Boolean)
        MyBase.New
        Me.InitializeComponent()
        Me.CurrentUser = CurrentUserRecord.Clone
        Me.CurrentUser.PumpAit = CurrentUserRecord.PumpAit
        Me.CurrentUser.CarbRatios = CurrentUserRecord.CarbRatios
        Me.CurrentUser.CurrentTarget = CurrentUserRecord.CurrentTarget
        _fromPdf = newUser
    End Sub

    Public Property CurrentUser As CurrentUserRecord

    Private Shared Sub InitializeComboList(comboBoxCell As DataGridViewComboBoxCell,
                                           start As Integer,
                                           Optional currentEnd As TimeOnly? = Nothing)
        Dim data As New Dictionary(Of String, TimeOnly)(Comparer)
        For i As Integer = start To 47
            Dim value As New TimeOnly(hour:=i \ 2, minute:=(i Mod 2) * 30)
            data.Add(key:=value.ToHoursMinutes, value)
        Next
        data.Add(key:=Eleven59PM, value:=Eleven59)
        If currentEnd.HasValue Then
            data(key:=currentEnd.Value.ToHoursMinutes()) = currentEnd.Value
        End If
        Dim keySelector As Func(Of KeyValuePair(Of String, TimeOnly), TimeOnly) =
            Function(item As KeyValuePair(Of String, TimeOnly))
                Return item.Value
            End Function
        comboBoxCell.DataSource = data.OrderBy(keySelector).ToArray()
        comboBoxCell.DisplayMember = "Key"
        comboBoxCell.ValueMember = "Value"
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If _currentUserBackup Is Nothing Then
            If MsgBox(heading:="If you cancel, the program will exit",
                      prompt:="Retry will allow editing.",
                      buttonStyle:=MsgBoxStyle.RetryCancel Or MsgBoxStyle.Exclamation,
                      title:="Exit Or Retry") = MsgBoxResult.Cancel Then

                End
            End If
            Me.PumpAitComboBox.Enabled = True
            Me.DialogResult = DialogResult.None
        Else
            If Not Me.CurrentUser.Equals(other:=_currentUserBackup) Then
                If MsgBox(heading:="If you cancel, all changes will be lost.",
                          prompt:="Retry will allow editing.",
                          buttonStyle:=MsgBoxStyle.RetryCancel Or MsgBoxStyle.Exclamation,
                          title:="Exit Or Retry") = MsgBoxResult.Cancel Then
                    Me.DialogResult = DialogResult.Cancel
                    Exit Sub
                End If
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
        Dim columnName As String = dgv.Columns(index:=e.ColumnIndex).Name
        Dim index As Integer
        Select Case columnName
            Case NameOf(ColumnDeleteRow)
                cell = dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex)
                If Not CType(cell, DataGridViewDisableButtonCell).Enabled Then
                    Exit Sub
                End If
                index = e.RowIndex
                dgv.Rows.Remove(dataGridViewRow:=dgv.Rows(index))
                index -= 1
                With dgv.Rows(index)
                    columnName = NameOf(ColumnDeleteRow)
                    Dim btnCell As DataGridViewDisableButtonCell =
                        CType(.Cells(columnName), DataGridViewDisableButtonCell)
                    btnCell.Enabled = False
                    btnCell.ReadOnly = True
                    columnName = NameOf(ColumnEnd)
                    Dim comboBoxCell As DataGridViewComboBoxCell = CType(.Cells(columnName), DataGridViewComboBoxCell)
                    columnName = NameOf(ColumnEnd)
                    Dim dgvRow As DataGridViewRow = Me.InitializeDataGridView.Rows(index)
                    Dim startTime As TimeOnly = TimeOnly.Parse(s:=dgvRow.Cells(columnName).Value.ToString)
                    start = CInt(startTime.ToTimeSpan.TotalMinutes / 30)
                    InitializeComboList(comboBoxCell, start)
                    comboBoxCell.Value = Eleven59
                    comboBoxCell.ReadOnly = False
                    columnName = NameOf(ColumnSave)
                    btnCell = CType(.Cells(columnName), DataGridViewDisableButtonCell)
                    btnCell.ReadOnly = False
                    btnCell.Enabled = True
                End With

            Case NameOf(ColumnStart)
                dgv.CurrentCell = dgv.Rows(index:=e.RowIndex).Cells(columnName:=NameOf(ColumnEnd))

            Case NameOf(ColumnEnd)
            Case NameOf(ColumnNumericUpDown)

            Case NameOf(ColumnSave)
                With Me.InitializeDataGridView
                    columnName = NameOf(ColumnEnd)
                    cell = Me.InitializeDataGridView.Rows(index:=e.RowIndex).Cells(columnName)
                    If DirectCast(cell.Value, TimeOnly) = Eleven59 OrElse .RowCount = 12 Then
                        Me.OK_Button.Enabled = True
                        columnName = NameOf(ColumnSave)
                        index = .RowCount - 1
                        Dim dgvCell As DataGridViewCell = .Rows(index).Cells(columnName)
                        Dim buttonCell As DataGridViewDisableButtonCell =
                            CType(dgvCell, DataGridViewDisableButtonCell)

                        buttonCell.ReadOnly = True
                        buttonCell.Enabled = False
                        Me.InitializeDataGridView.Enabled = False
                        Me.OK_Button.Focus()
                        Exit Sub
                    End If
                    With .Rows(index:=e.RowIndex)
                        CType(.Cells(columnName:=NameOf(ColumnDeleteRow)), DataGridViewDisableButtonCell).Enabled = False
                        CType(.Cells(columnName:=NameOf(ColumnSave)), DataGridViewDisableButtonCell).Enabled = False
                    End With
                    For Each c As DataGridViewCell In .Rows(index:=e.RowIndex).Cells
                        c.ReadOnly = Not c.OwningColumn.HeaderText = "Carb Ratio g/U"
                    Next
                    .Rows.Add()
                    With .Rows(index:= .Rows.Count - 1)
                        Me.OK_Button.Enabled = False
                        columnName = NameOf(ColumnStart)
                        Dim comboBoxCell As DataGridViewComboBoxCell =
                            CType(.Cells(columnName), DataGridViewComboBoxCell)
                        columnName = NameOf(ColumnEnd)
                        Dim colEndCell As DataGridViewCell =
                            Me.InitializeDataGridView.Rows(index:=e.RowIndex).
                                                      Cells(columnName)
                        colEndCell.ErrorText = String.Empty
                        Dim timeOnly As TimeOnly = TimeOnly.Parse(s:=colEndCell.Value.ToString)
                        Dim item As String = timeOnly.ToHoursMinutes
                        comboBoxCell.Items.Add(item)
                        comboBoxCell.Value = item
                        columnName = NameOf(ColumnEnd)
                        comboBoxCell = CType(.Cells(columnName), DataGridViewComboBoxCell)
                        start = CInt(timeOnly.ToTimeSpan.TotalMinutes / 30) + 1
                        InitializeComboList(comboBoxCell, start)
                        comboBoxCell.Value = Eleven59
                        columnName = NameOf(ColumnNumericUpDown)
                        .Cells(columnName).Value = 15D
                        columnName = NameOf(ColumnDeleteRow)
                        CType(.Cells(columnName), DataGridViewDisableButtonCell).Enabled = True
                    End With

                End With
        End Select
    End Sub

    Private Sub InitializeDataGridView_Enter(sender As Object, e As EventArgs) Handles InitializeDataGridView.Enter
        Me.InitializeDataGridView.CausesValidation = True
    End Sub

    Private Sub InitializeDataGridView_Validating(sender As Object, e As CancelEventArgs) _
        Handles InitializeDataGridView.Validating

        Dim index As Integer = Me.InitializeDataGridView.RowCount - 1
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(index).Cells(columnName:=NameOf(ColumnEnd))
        If DirectCast(cell.Value, TimeOnly) = Eleven59 Then
            cell.ErrorText = String.Empty
        Else
            With Me.InitializeDataGridView
                If .RowCount = 12 Then
                    cell.Value = Eleven59
                    cell.ErrorText = String.Empty
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
        Me.InitializeDataGridView.ApplyCellFontIncrease()
        Me.InitializeDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
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
            .DataSource = New BindingSource(dataSource:=AitLengths, dataMember:=Nothing)
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
        ElseIf IsFlex() Then
            Me.UseAITAdvancedDecayCheckBox.CheckState = CheckState.Unchecked
            Me.UseAITAdvancedDecayCheckBox.Enabled = False
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
                        Dim columnName As String = NameOf(ColumnDeleteRow)
                        Dim cell As DataGridViewDisableButtonCell =
                            CType(.Cells(columnName), DataGridViewDisableButtonCell)
                        cell.Enabled = i.IsLast
                        columnName = NameOf(ColumnStart)
                        With CType(.Cells(columnName), DataGridViewComboBoxCell)
                            .Items.Add(item:=value.StartTime.ToHoursMinutes)
                            .Value = value.StartTime.ToHoursMinutes()
                            .ReadOnly = True
                        End With
                        Dim timeSpan As New TimeSpan(hours:=value.StartTime.Hour,
                                                     minutes:=value.StartTime.Minute,
                                                     seconds:=0)
                        columnName = NameOf(ColumnEnd)
                        Dim comboBoxCell As DataGridViewComboBoxCell =
                            CType(.Cells(columnName), DataGridViewComboBoxCell)
                        Dim endTime As TimeOnly = value.EndTime
                        If endTime.Hour = 23 AndAlso endTime.Minute = 59 Then
                            endTime = Eleven59
                        End If
                        InitializeComboList(comboBoxCell, start:=CInt((timeSpan / ThirtyMinuteSpan) + 1),
                                            currentEnd:=endTime)
                        With comboBoxCell
                            .Value = endTime
                            .ReadOnly = i.Index >= 11 OrElse (i.IsLast AndAlso Not i.IsFirst)
                        End With
                        columnName = NameOf(ColumnNumericUpDown)
                        Dim numericCell As DataGridViewNumericUpDownCell =
                            CType(.Cells(columnName), DataGridViewNumericUpDownCell)
                        numericCell.Value = CDec(value.CarbRatio)
                        numericCell.ReadOnly = False
                        columnName = NameOf(ColumnSave)
                        With CType(.Cells(columnName), DataGridViewDisableButtonCell)
                            .ReadOnly = False
                            .Enabled = i.IsLast
                        End With
                    End With
                Next
                Me.InitializeDataGridView.Enabled = Not _fromPdf
            Else
                .Rows.Add()
                With .Rows(index:=0)
                    Dim columnName As String = NameOf(ColumnDeleteRow)
                    CType(.Cells(columnName), DataGridViewDisableButtonCell).Enabled = False
                    columnName = NameOf(ColumnStart)
                    With CType(.Cells(columnName), DataGridViewComboBoxCell)
                        .Items.Add(item:=MidnightStr)
                        .Value = MidnightStr
                        .ReadOnly = True
                    End With
                    columnName = NameOf(ColumnEnd)
                    Dim comboBoxCell As DataGridViewComboBoxCell = CType(.Cells(columnName), DataGridViewComboBoxCell)
                    InitializeComboList(comboBoxCell, start:=1)

                    columnName = NameOf(ColumnEnd)
                    CType(.Cells(columnName), DataGridViewComboBoxCell).Value = Eleven59

                    columnName = NameOf(ColumnNumericUpDown)
                    CType(.Cells(columnName), DataGridViewNumericUpDownCell).Value = 15D
                End With
                Me.InitializeDataGridView.Enabled = False
            End If
        End With

        Me.InitializeDataGridView.ConfigureContentColumnSizing()
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
            Me.ErrorProvider1.SetError(control, value:=String.Empty)
        Else
            Me.ErrorProvider1.SetError(control, value:=$"Value must be {MidnightStr}")
            e.Cancel = True
        End If

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim index As Integer = Me.InitializeDataGridView.RowCount - 1
        Dim cell As DataGridViewCell = Me.InitializeDataGridView.Rows(index).Cells(columnName:=NameOf(ColumnEnd))
        cell.ErrorText = String.Empty
        Me.DialogResult = DialogResult.OK

        Me.CurrentUser.PumpAit =
            ParseAsSingle(obj:=Me.PumpAitComboBox.SelectedValue).RoundToSingle(digits:=2)

        Me.CurrentUser.InsulinTypeName = Me.InsulinTypeComboBox.Text
        Me.CurrentUser.InsulinRealAit = CType(Me.InsulinTypeComboBox.SelectedValue, InsulinActivationRecord).AitHours

        Me.CurrentUser.UseAdvancedAitDecay =
            Me.UseAITAdvancedDecayCheckBox.CheckState
        Me.CurrentUser.CurrentTarget =
            CType(Me.TargetSgComboBox.SelectedItem, KeyValuePair(Of String, Single)).Value

        Me.CurrentUser.CarbRatios.Clear()

        ' Save all carb ratios
        Dim rowIndex As Integer = 0
        For Each row As DataGridViewRow In Me.InitializeDataGridView.Rows
            Dim carbRecord As New CarbRatioRecord
            Dim columnName As String = NameOf(ColumnStart)
            cell = row.Cells(columnName)
            carbRecord.StartTime = TimeOnly.Parse(cell.Value.ToString, provider:=CurrentDateCulture)
            columnName = NameOf(ColumnEnd)
            cell = row.Cells(columnName)
            carbRecord.EndTime = DirectCast(cell.Value, TimeOnly)
            columnName = NameOf(ColumnNumericUpDown)
            Dim numericCell As DataGridViewNumericUpDownCell =
                CType(row.Cells(columnName), DataGridViewNumericUpDownCell)
            carbRecord.CarbRatio =
                ParseAsSingle(numericCell.Value).RoundToSingle(digits:=1)
            Me.CurrentUser.CarbRatios.Add(item:=carbRecord)
        Next

        Dim contents As String
        Try
            contents = Me.CurrentUser.ToJson()
            File.WriteAllTextAsync(path:=GetUserSettingsPath(), contents)
        Catch ex As Exception
            LogMessage(message:=$"ERROR: failed serializing CurrentUser settings to {GetUserSettingsPath()}")
        End Try
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
            Me.ErrorProvider1.SetError(control, value:=String.Empty)
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
            Case CheckState.Indeterminate
                chkBox.CheckState = CheckState.Checked
            Case CheckState.Unchecked
                chkBox.CheckState = CheckState.Checked
            Case CheckState.Checked
                chkBox.CheckState = CheckState.Unchecked
        End Select
        chkBox.Enabled = chkBox.CheckState = CheckState.Checked
        Dim dgv As DataGridView = Me.InitializeDataGridView
        Dim colName As String = NameOf(ColumnEnd)
        Dim cell As DataGridViewCell = dgv.Rows(index:=Me.InitializeDataGridView.RowCount - 1).Cells(colName)
        cell.Value = Eleven59
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

End Class
