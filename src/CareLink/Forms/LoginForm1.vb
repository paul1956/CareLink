' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Linq
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Public Class LoginForm1
    Public Property Client As CareLinkClient

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.UsernameTextBox.Text = My.Settings.CareLinkUserName
        Me.PasswordTextBox.Text = My.Settings.CareLinkPassword
        Me.RegionComboBox.DataSource = New BindingSource(s_regionList, Nothing)
        Me.RegionComboBox.DisplayMember = "Key"
        Me.RegionComboBox.ValueMember = "Value"

        If String.IsNullOrEmpty(My.Settings.CountryCode) Then
            My.Settings.CountryCode = "US"
        End If
        Me.RegionComboBox.SelectedValue = My.Settings.CountryCode.GetRegionFromCode
        Me.CountryComboBox.Text = My.Settings.CountryCode.GetCountryFromCode
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        Me.OK.Enabled = False
        Me.Cancel.Enabled = False
        Me.Client = New CareLinkClient(Me.LoginStatus, Me.UsernameTextBox.Text, Me.PasswordTextBox.Text, Me.CountryComboBox.SelectedValue.ToString)
        If Not Me.Client.LoggedIn Then
            Dim recentData As Dictionary(Of String, String) = Me.Client.GetRecentData()
            If recentData IsNot Nothing AndAlso recentData.Count > 0 Then
                Me.OK.Enabled = True
                Me.Cancel.Enabled = True
                If Me.SaveCredentials.CheckState = CheckState.Checked Then
                    My.Settings.CareLinkUserName = Me.UsernameTextBox.Text
                    My.Settings.CareLinkPassword = Me.PasswordTextBox.Text
                    My.Settings.CountryCode = Me.CountryComboBox.SelectedValue.ToString
                End If

                My.Settings.Save()
                Me.Hide()
                Exit Sub
            End If
        Else
            Me.OK.Enabled = True
            Me.Cancel.Enabled = True
            Me.Hide()
            Exit Sub
        End If

        If MsgBox("Login Unsuccessful. try again? If no program will exit!", Buttons:=MsgBoxStyle.YesNo, Title:="Login Failed") = MsgBoxResult.No Then
            End
        End If
        Me.OK.Enabled = True
        Me.Cancel.Enabled = True
    End Sub

    Private Sub RegionComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RegionComboBox.SelectedIndexChanged
        Dim countriesInRegion As New Dictionary(Of String, String)
        Dim selectedRegion As String = s_regionList.Values(Me.RegionComboBox.SelectedIndex)
        For Each kvp As KeyValuePair(Of String, String) In s_regionCountryList

            If kvp.Value = selectedRegion Then
                countriesInRegion.Add(kvp.Key, s_countryCodeList(kvp.Key))
            End If
        Next
        If countriesInRegion.Count > 0 Then
            Me.CountryComboBox.DataSource = New BindingSource(countriesInRegion, Nothing)
            Me.CountryComboBox.DisplayMember = "Key"
            Me.CountryComboBox.ValueMember = "Value"
            Me.CountryComboBox.Enabled = True
        Else
            Me.CountryComboBox.Enabled = False
        End If
    End Sub

    Private Sub ShowPasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPasswordCheckBox.CheckedChanged
        If Me.ShowPasswordCheckBox.Checked Then
            Me.PasswordTextBox.PasswordChar = Nothing
        Else
            Me.PasswordTextBox.PasswordChar = "*"c
        End If
    End Sub
End Class
