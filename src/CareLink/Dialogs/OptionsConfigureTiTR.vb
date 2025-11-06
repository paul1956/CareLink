' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class OptionsConfigureTiTR
    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) _
        Handles Cancel_Button.Click

        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        My.Settings.TiTrLowThreshold = CInt(Me.ThresholdNumericUpDown.Value)
        My.Settings.TiTrTreatmentTargetPercent = CInt(Me.TreatmentTargetPercentUpDown.Value)
        My.Settings.Save()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private _unitsDesiredIndex As Integer

    Private Sub OptionsConfigureTiTR_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Ensure the combo looks enabled but cannot be changed by the user.
        Me.UnitsComboBox.DropDownStyle = ComboBoxStyle.DropDownList

        If NativeMmolL Then
            Me.ThresholdNumericUpDown.DecimalPlaces = 1
            Me.ThresholdNumericUpDown.Minimum = CDec(3.3)
            Me.ThresholdNumericUpDown.Maximum = CDec(3.9)
            Me.ThresholdNumericUpDown.Increment = 0.1D
            _unitsDesiredIndex = 1
        Else
            Me.ThresholdNumericUpDown.DecimalPlaces = 0
            Me.ThresholdNumericUpDown.Minimum = 60
            Me.ThresholdNumericUpDown.Maximum = 70
            Me.ThresholdNumericUpDown.Increment = 1
            _unitsDesiredIndex = 0
        End If
        Me.UnitsComboBox.SelectedIndex = _unitsDesiredIndex

        Me.ThresholdNumericUpDown.Value = My.Settings.TiTrLowThreshold

        Me.TreatmentTargetPercentUpDown.Value = My.Settings.TiTrTreatmentTargetPercent
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

    Public Function GetTiTrMsg() As String
        Return $"{My.Settings.TiTrLowThreshold}/{My.Settings.TiTrTreatmentTargetPercent}%"
    End Function

    Private Sub UnitsComboBox_DropDown(sender As Object, e As EventArgs) Handles UnitsComboBox.DropDown
        ' Immediately close the dropdown so the user cannot change the value with the mouse.
        CType(sender, ComboBox).DroppedDown = False
    End Sub

    Private Sub UnitsComboBox_KeyDown(sender As Object, e As KeyEventArgs) Handles UnitsComboBox.KeyDown
        ' Suppress any keyboard attempts to change the combo selection.
        e.SuppressKeyPress = True
        e.Handled = True
    End Sub

    Private Sub UnitsComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles UnitsComboBox.SelectionChangeCommitted
        ' revert any user selection back to the programmatic value
        CType(sender, ComboBox).SelectedIndex = _unitsDesiredIndex
    End Sub

End Class
