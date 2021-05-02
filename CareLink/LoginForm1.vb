' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LoginForm1
    Public Password As String
    Public UserName As String

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Close()
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click

        UserName = UsernameTextBox.Text
        Password = PasswordTextBox.Text
        Close()
    End Sub

End Class
