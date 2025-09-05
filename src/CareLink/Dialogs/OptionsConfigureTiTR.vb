' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class OptionsConfigureTiTR
    Private _lowThreshold As Integer
    Private _treatmentTargetPercent As Integer

    Public Property LowThreshold As Integer
        Get
            Return _lowThreshold
        End Get
        Set
            _lowThreshold = Value
        End Set
    End Property

    Public Property TreatmentTargetPercent As Integer
        Get
            Return _treatmentTargetPercent
        End Get
        Set
            _treatmentTargetPercent = Value
        End Set
    End Property

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) _
        Handles Cancel_Button.Click

        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        _lowThreshold = CInt(Me.ThresholdNumericUpDown.Value)
        My.Settings.TiTrLowThreshold = _lowThreshold
        _treatmentTargetPercent = CInt(Me.TreatmentTargetPercentUpDown.Value)
        My.Settings.TiTrTreatmentTargetPercent = _treatmentTargetPercent
        My.Settings.Save()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub OptionsConfigureTiTR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.ThresholdNumericUpDown.Value = Me.LowThreshold
        Me.TreatmentTargetPercentUpDown.Value = Me.TreatmentTargetPercent
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
        Return $"{_lowThreshold}/{_treatmentTargetPercent}%"
    End Function

End Class
