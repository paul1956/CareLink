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

    Private Sub OptionsConfigureTiTR_Load(sender As Object, e As EventArgs) Handles Me.Load
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

End Class
