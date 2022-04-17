' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Media

Public Class BGMiniWindow
    Private _alarmPlayedHigh As Boolean
    Private _alarmPlayedLow As Boolean

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
            Exit Sub
        End If
        Dim normalizedBG As Double = Double.Parse(Me.BGTextBox.Text)
        If Form1.BgUnitsString <> "mg/dl" Then
            normalizedBG *= 18
        End If
        Select Case normalizedBG
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
    End Sub

    Private Sub playSoundFromResource(SoundName As String)
        Using player As New SoundPlayer(My.Resources.ResourceManager.GetStream(SoundName, Globalization.CultureInfo.CurrentUICulture))
            player.Play()
        End Using
    End Sub

End Class
