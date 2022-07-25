' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization
Imports System.Media

Public Class BGMiniWindow
    Private _alarmPlayedHigh As Boolean
    Private _alarmPlayedLow As Boolean
    Private _currentBGValue As Double = Double.NaN
    Private _lastBGValue As Double
    Private _normalizedBG As Double

    Public Sub SetCurrentBGString(Value As String)
        _lastBGValue = _currentBGValue
        _currentBGValue = Value.ParseDouble()
        If Not Double.IsNaN(_currentBGValue) Then
            _normalizedBG = _currentBGValue
            If Form1.BgUnitsString <> "mg/dl" Then
                _normalizedBG *= 18
            End If
        End If
        Me.BGTextBox.Text = Value
    End Sub

    Private Sub ActiveInsulinTextBox_GotFocus(sender As Object, e As EventArgs) Handles ActiveInsulinTextBox.GotFocus
        Me.HiddenTextBox.Focus()
    End Sub

    Private Sub BGMiniWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Form1.Visible = True
    End Sub

    Private Sub BGMiniWindow_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        Me.HiddenTextBox.Focus()
    End Sub

    Private Sub BGTextBox_GotFocus(sender As Object, e As EventArgs) Handles BGTextBox.GotFocus
        Me.HiddenTextBox.Focus()
    End Sub

    Private Sub BGTextBox_TextChanged(sender As Object, e As EventArgs) Handles BGTextBox.TextChanged
        Me.Text = $"Glusose at {Now}"
        If Me.BGTextBox.Text.Length = 0 OrElse Me.BGTextBox.Text = "---" OrElse Me.BGTextBox.Text = "999" Then
            _currentBGValue = Double.NaN
            Me.DeltaTextBox.Text = ""
        Else
            If Double.IsNaN(_currentBGValue) OrElse _currentBGValue = 0 OrElse Double.IsNaN(_lastBGValue) OrElse _lastBGValue = 0 Then
                Me.DeltaTextBox.Text = ""
            Else
                Dim delta As Double = _currentBGValue - _lastBGValue
                Me.DeltaTextBox.Text = delta.ToString("+0;-#")
                Select Case delta
                    Case Is = 0
                        Me.DeltaTextBox.Text = ""
                    Case Is > 0
                        Me.DeltaTextBox.ForeColor = Color.Blue
                    Case Is < 0
                        Me.DeltaTextBox.ForeColor = Color.Orange
                End Select
            End If
            Select Case _normalizedBG
                Case <= 70
                    Me.BGTextBox.ForeColor = Color.Orange
                    If Not _alarmPlayedLow Then
                        Me.playSoundFromResource("Low Alarm")
                        _alarmPlayedLow = True
                        _alarmPlayedHigh = False
                    End If
                Case <= 180
                    Me.BGTextBox.ForeColor = Color.Green
                    _alarmPlayedLow = False
                    _alarmPlayedHigh = False
                Case Else
                    Me.BGTextBox.ForeColor = Color.Red
                    If Not _alarmPlayedHigh Then
                        Me.playSoundFromResource("High Alarm")
                        _alarmPlayedLow = False
                        _alarmPlayedHigh = True
                    End If
            End Select
        End If

    End Sub

    Private Sub playSoundFromResource(SoundName As String)
        Using player As New SoundPlayer(My.Resources.ResourceManager.GetStream(SoundName, CurrentUICulture))
            player.Play()
        End Using
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        My.Forms.Form1.Visible = True
        Me.Hide()
    End Sub

End Class
