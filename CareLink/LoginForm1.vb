' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LoginForm1
    Public Client As CareLinkClient

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Close()
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        OK.Enabled = False
        Cancel.Enabled = False
        If Client Is Nothing Then
            Client = New CareLinkClient(UsernameTextBox.Text, PasswordTextBox.Text, "us")
        End If
        If Not Client.LoggedIn Then
            Dim RecentData As Dictionary(Of String, String) = Client.getRecentData()
            If RecentData.Count > 0 Then
                OK.Enabled = True
                Cancel.Enabled = True
                Close()
                Exit Sub
            End If
        Else
            Close()
            Exit Sub
        End If

        If MsgBox("Login Unsuccessful. try again?", Buttons:=MsgBoxStyle.YesNo, Title:="Login Failed") = MsgBoxResult.No Then
            OK.Enabled = True
            Cancel.Enabled = True
            Close()
        End If
    End Sub

End Class
