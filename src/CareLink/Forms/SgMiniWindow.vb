' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Media
Imports System.Speech.Synthesis

Public Class SgMiniWindow
    Private ReadOnly _form1 As Form1
    Private _alarmPlayedHigh As Boolean
    Private _alarmPlayedLow As Boolean
    Private _currentSgValue As Single = Double.NaN
    Private _lastSgValue As Single
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
                  $"{s_firstName}'s Updated {CInt((PumpNow() - s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2DateTime).TotalMinutes)} minutes ago"
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

    Private Sub PlaySoundFromResource(SoundName As String)
        Using player As New SoundPlayer(My.Resources.ResourceManager.GetStream(SoundName, CurrentUICulture))
            player.Play()
        End Using
    End Sub

    Private Sub PlayText(text As String)
        Dim synth As New SpeechSynthesizer
        synth.SetOutputToDefaultAudioDevice()
        synth.Speak(text)
    End Sub

    Private Sub SgMiniWindow_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
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
        Me.Text = GetLastUpdateMessage()
        If Me.SgTextBox.Text.Length = 0 OrElse Me.SgTextBox.Text = "---" OrElse Me.SgTextBox.Text = "9999" Then
            _currentSgValue = Double.NaN
            Me.DeltaTextBox.Text = ""
        Else
            If Double.IsNaN(_currentSgValue) OrElse _currentSgValue = 0 OrElse Double.IsNaN(_lastSgValue) OrElse _lastSgValue = 0 Then
                Me.DeltaTextBox.Text = ""
            Else
                Dim delta As Double = _currentSgValue - _lastSgValue
                Me.DeltaTextBox.Text = delta.ToString(If(nativeMmolL, $"+0.00;-#.00", "+0;-#0"), CurrentUICulture)
                Select Case delta
                    Case Is = 0
                        Me.DeltaTextBox.Text = ""
                    Case Is > 0
                        Me.DeltaTextBox.ForeColor = Color.Blue
                    Case Is < 0
                        Me.DeltaTextBox.ForeColor = Color.Orange
                End Select
            End If
            Select Case _normalizedSg
                Case = 0
                    Me.SgTextBox.BackColor = SystemColors.Window
                    Me.SgTextBox.ForeColor = Color.Black
                Case < 70
                    Me.SgTextBox.BackColor = SystemColors.Window
                    Me.SgTextBox.ForeColor = Color.Red
                    If Not _alarmPlayedLow Then
                        If s_speechOn Then
                            Me.PlayText($"Low Alarm current blood glucose {_normalizedSg}")
                        Else
                            Me.PlaySoundFromResource("Low Alarm")
                        End If
                        _alarmPlayedLow = True
                        _alarmPlayedHigh = False
                    End If
                Case <= 180
                    Me.SgTextBox.BackColor = SystemColors.Window
                    Me.SgTextBox.ForeColor = Color.Green
                    _alarmPlayedLow = False
                    _alarmPlayedHigh = False
                Case Else
                    Me.SgTextBox.BackColor = GetContrastingColor(Color.Yellow)
                    Me.SgTextBox.ForeColor = Color.Yellow
                    If Not _alarmPlayedHigh Then
                        If s_speechOn Then
                            Me.PlayText($"High alarm current blood glucose {_normalizedSg}")
                        Else
                            Me.PlaySoundFromResource("High Alarm")
                        End If
                        _alarmPlayedLow = False
                        _alarmPlayedHigh = True
                    End If
            End Select
        End If

    End Sub

    Public Sub SetCurrentSgString(Value As String)
        If String.IsNullOrEmpty(Value) Then
            Value = "---"
        End If

        _lastSgValue = _currentSgValue
        _currentSgValue = Value.ParseSingle(2)
        If Not Double.IsNaN(_currentSgValue) Then
            _normalizedSg = _currentSgValue
            If nativeMmolL Then
                _normalizedSg *= MmolLUnitsDivisor
            End If
            Me.SgTextBox.ForeColor = SystemColors.ControlText
            Me.SgTextBox.Text = If(nativeMmolL, Value.ParseSingle(1).ToString(CurrentUICulture), CInt(_currentSgValue).ToString)
        Else
            Me.SgTextBox.ForeColor = Color.Red
            Me.SgTextBox.Text = Value
        End If
        Me.Text = GetLastUpdateMessage()
    End Sub

End Class
