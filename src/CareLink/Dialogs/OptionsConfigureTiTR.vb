' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class OptionsConfigureTiTR
    Public Property LowThreshold As Integer = 70
    Public Property TreatmentTargetPercent As Integer = 70

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.LowThreshold = CInt(Me.ThresholdNumericUpDown.Value)
        Me.TreatmentTargetPercent = CInt(Me.ThresholdNumericUpDown.Value)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OptionsConfigureTiTR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.ThresholdNumericUpDown.Value = Me.LowThreshold
        Me.TreatmentTargetPercentUpDown.Value = Me.TreatmentTargetPercent
    End Sub
End Class
