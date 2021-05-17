' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LoginForm1
    Public Client As CareLinkClient
    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Close()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        UsernameTextBox.Text = My.Settings.username
        PasswordTextBox.Text = My.Settings.password
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        OK.Enabled = False
        Cancel.Enabled = False
        If Client Is Nothing Then
            Client = New CareLinkClient(UsernameTextBox.Text, PasswordTextBox.Text, "us")
        End If
        If Not Client.LoggedIn Then
            Dim RecentData As Dictionary(Of String, String) = Client.getRecentData()
            If RecentData IsNot Nothing AndAlso RecentData.Count > 0 Then
                OK.Enabled = True
                Cancel.Enabled = True
                If SaveCredentials.CheckState = CheckState.Checked Then
                    My.Settings.username = UsernameTextBox.Text
                    My.Settings.password = PasswordTextBox.Text
                End If

                My.Settings.Save()
                Hide()
                Exit Sub
            End If
        Else
            OK.Enabled = True
            Cancel.Enabled = True
            Hide()
            Exit Sub
        End If

        If MsgBox("Login Unsuccessful. try again? If no program will exit!", Buttons:=MsgBoxStyle.YesNo, Title:="Login Failed") = MsgBoxResult.No Then
            End
        End If
        OK.Enabled = True
        Cancel.Enabled = True
    End Sub

End Class
