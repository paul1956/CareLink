Imports Microsoft.Web.WebView2.WinForms

Public Class LoginForm1
    Public Password As String
    Public UserName As String

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click

        UserName = UsernameTextBox.Text
        Password = PasswordTextBox.Text
        Close()
    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Close()
    End Sub

End Class
