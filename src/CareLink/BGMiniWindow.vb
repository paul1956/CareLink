' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BGMiniWindow

    Private Sub BGMiniWindow_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        Me.BGTextBox.SelectionStart = 0
        Me.BGTextBox.SelectionLength = 0
        Me.BGTextBox.HideSelection = True
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
                Beep()
            Case <= 180
                Me.BGTextBox.ForeColor = Color.Green
            Case Else
                Me.BGTextBox.ForeColor = Color.Red
                Beep()
        End Select
    End Sub

End Class
