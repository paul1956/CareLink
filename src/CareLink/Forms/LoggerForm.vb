' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LoggerForm

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Public Sub LogMessage(message As String)
        If Me.InvokeRequired Then
            Dim method As New Action(Of String)(AddressOf Me.LogMessage)
            Me.BeginInvoke(method, $"{message}{vbCrLf}")
        Else
            Me.txtLog.AppendText(text:=$"{message}{vbCrLf}")
            'Me.txtLog.AppendNewLine()
        End If
    End Sub

End Class
