' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Media

Public Class SgMiniForm
    Private ReadOnly _form1 As Form1
    Private _alarmPlayedHigh As Boolean
    Private _alarmPlayedLow As Boolean
    Private _currentDelta As Single
    Private _currentSgValue As Single = Double.NaN
    Private _normalizedSg As Single

    Public Sub New()
        MyBase.New
        Me.InitializeComponent()
    End Sub

    Public Sub New(form1 As Form1)
        MyBase.New
        Me.InitializeComponent()
        _form1 = form1
    End Sub

    Private Shared Function GetLastUpdateMessage() As String
        Return If(s_lastMedicalDeviceDataUpdateServerEpoch = 0,
                  $"{s_firstName}'s Last Update Unknown",
                  $"{s_firstName}'s Updated {CInt((PumpNow() - s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime).TotalMinutes)} minutes ago"
                 )
    End Function

    Private Sub ActiveInsulinTextBox_GotFocus(sender As Object, e As EventArgs) Handles ActiveInsulinTextBox.GotFocus
        Me.HiddenTextBox.Focus()
    End Sub

    Private Sub ChkTopMost_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTopMost.CheckedChanged
        If Me.ChkTopMost.Checked Then
            Me.TopMost = True
        ElseIf Not Me.ChkTopMost.Checked Then
            Me.TopMost = False
        End If
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        _form1.Visible = True
        Me.Hide()
    End Sub

    Private Sub DeltaTextBox_TextChanged(sender As Object, e As EventArgs) Handles DeltaTextBox.TextChanged
        Select Case True
            Case Me.DeltaTextBox.Text = ""
                Me.DeltaTextBox.ForeColor = SystemColors.Window
                Me.DeltaTextBox.BackColor = SystemColors.Window

            Case Math.Abs(_currentDelta) < 0.001
                Me.DeltaTextBox.Text = ""
                Me.DeltaTextBox.ForeColor = SystemColors.Window
                Me.DeltaTextBox.BackColor = SystemColors.Window

            Case _currentDelta > 0
                Me.DeltaTextBox.ForeColor = Color.Blue
                Me.DeltaTextBox.BackColor = Color.Blue.GetContrastingColor()

            Case Else
                Me.DeltaTextBox.ForeColor = Color.Orange
                Me.DeltaTextBox.BackColor = Color.Orange.GetContrastingColor()

        End Select
    End Sub

    Private Sub PlaySoundFromResource(SoundName As String)
        Using player As New SoundPlayer(My.Resources.ResourceManager.GetStream(SoundName, CurrentUICulture))
            player.Play()
        End Using
    End Sub

    Private Sub SgMiniForm_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible Then
            Me.SgTextBox.SelectionLength = 0
        End If

    End Sub

    Private Sub SgMiniForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _form1.Visible = True
    End Sub

    Private Sub SgMiniWindow_GotFocus(sender As Object, e As EventArgs) Handles MyBase.GotFocus
        Me.HiddenTextBox.Focus()
    End Sub

    Private Sub SgMiniWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Modifiers = Keys.Alt AndAlso e.KeyCode = Keys.W Then
            _form1.Visible = True
            Me.Hide()
        End If
    End Sub

    Private Sub SgTextBox_GotFocus(sender As Object, e As EventArgs) Handles SgTextBox.GotFocus
        Me.HiddenTextBox.Focus()
    End Sub

    Private Sub SgTextBox_TextChanged(sender As Object, e As EventArgs) Handles SgTextBox.TextChanged
        Select Case True
            Case Single.IsNaN(_normalizedSg), _normalizedSg = 0
                Me.SgTextBox.BackColor = Color.Black.GetContrastingColor()
                Me.SgTextBox.ForeColor = Color.Black
            Case _normalizedSg < 70
                Me.SgTextBox.BackColor = Color.Red.GetContrastingColor()
                Me.SgTextBox.ForeColor = Color.Red
                If Not _alarmPlayedLow Then
                    PlayText($"Low Alarm for {s_firstName}, current sensor glucose {_currentSgValue}")
                    _alarmPlayedLow = True
                    _alarmPlayedHigh = False
                End If
            Case _normalizedSg <= 180
                Me.SgTextBox.BackColor = Color.Green.GetContrastingColor()
                Me.SgTextBox.ForeColor = Color.Green
                _alarmPlayedLow = False
                _alarmPlayedHigh = False
            Case Else
                Me.SgTextBox.BackColor = Color.Yellow.GetContrastingColor()
                Me.SgTextBox.ForeColor = Color.Yellow
                If Not _alarmPlayedHigh Then
                    PlayText($"High alarm for {s_firstName}, current sensor glucose {_currentSgValue}")
                    _alarmPlayedLow = False
                    _alarmPlayedHigh = True
                End If
        End Select
        Me.Text = GetLastUpdateMessage()
    End Sub

    Public Sub SetCurrentDeltaValue(deltaString As String, delta As Single)
        Me.DeltaTextBox.Text = If(Math.Abs(delta) < 0.001,
                                  "",
                                  deltaString
                                 )
        _currentDelta = delta
    End Sub

    Public Sub SetCurrentSgString(sgString As String, sgValue As Single)
        Me.SgTextBox.Text = If(Single.IsNaN(sgValue),
                               "---",
                               sgString
                              )
        _currentSgValue = sgValue
        _normalizedSg = sgValue
        If NativeMmolL Then
            _normalizedSg *= MmolLUnitsDivisor
        End If

        Me.Text = GetLastUpdateMessage()
    End Sub

End Class
