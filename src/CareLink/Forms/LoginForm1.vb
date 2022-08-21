' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Class LoginForm1
    Private ReadOnly _mySource As New AutoCompleteStringCollection()
    Public Property Client As CareLinkClient
    Public Property LoggedOnUser As New UserDataRecord

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub CountryComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles CountryComboBox.SelectedValueChanged
        If TypeOf Me.CountryComboBox.SelectedValue Is String Then
            CurrentDateCulture = Me.CountryComboBox.SelectedValue.ToString.GetCurrentDateCulture
        Else
            CurrentDateCulture = CType(Me.CountryComboBox.SelectedValue, KeyValuePair(Of String, String)).Value.GetCurrentDateCulture
        End If
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim commandLineArgs As String() = Environment.GetCommandLineArgs()

        If commandLineArgs.Length > 1 Then
            Dim arg As String() = commandLineArgs(1).Split("=")
            If arg.Length = 2 Then
                Dim userRecord As UserDataRecord = Nothing
                If s_allUserSettingsData.TryGetValue(arg(1), userRecord) Then
                    If userRecord.AutoLogin Then
                        userRecord.UpdateSettings()
                    End If
                End If
            End If
        End If
        Me.UsernameTextBox.Text = My.Settings.CareLinkUserName
        Me.PasswordTextBox.Text = My.Settings.CareLinkPassword
        If File.Exists(s_settingsCsvFile) Then
            _mySource.AddRange(s_allUserSettingsData.Keys.ToArray)
        ElseIf Not String.IsNullOrWhiteSpace(My.Settings.CareLinkUserName) Then
            _mySource.Add(My.Settings.CareLinkUserName)
        Else
            _mySource.Clear()
        End If
        With Me.UsernameTextBox
            .AutoCompleteCustomSource = _mySource
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With
        Me.RegionComboBox.DataSource = New BindingSource(s_regionList, Nothing)
        Me.RegionComboBox.DisplayMember = "Key"
        Me.RegionComboBox.ValueMember = "Value"
        If String.IsNullOrEmpty(My.Settings.CountryCode) Then
            My.Settings.CountryCode = "US"
        End If
        Me.RegionComboBox.SelectedValue = My.Settings.CountryCode.GetRegionFromCode
        Me.CountryComboBox.Text = My.Settings.CountryCode.GetCountryFromCode
    End Sub

    Private Sub LoginForm1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If My.Settings.AutoLogin Then
            Me.OK_Click(Me.OK, Nothing)
        End If
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        Me.OK.Enabled = False
        Me.Cancel.Enabled = False
        Dim countryCode As String = Me.CountryComboBox.SelectedValue.ToString
        My.Settings.CountryCode = countryCode
        Me.Client = New CareLinkClient(Me.LoginStatus, Me.UsernameTextBox.Text, Me.PasswordTextBox.Text, countryCode)
        If Not Me.Client.LoggedIn Then
            Dim recentData As Dictionary(Of String, String) = Me.Client.GetRecentData(countryCode)
            If recentData IsNot Nothing AndAlso recentData.Count > 0 Then
                Me.LoggedOnUser = New UserDataRecord
                Me.OK.Enabled = True
                Me.Cancel.Enabled = True
                If Me.SaveCredentials.CheckState = CheckState.Checked Then
                    My.Settings.CareLinkUserName = Me.UsernameTextBox.Text
                    My.Settings.CareLinkPassword = Me.PasswordTextBox.Text
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

    Private Sub UsernameTextBox_Leave(sender As Object, e As EventArgs) Handles UsernameTextBox.Leave
        Dim userSettings As UserDataRecord = Nothing
        If s_allUserSettingsData.TryGetValue(Me.UsernameTextBox.Text, userSettings) Then
            If userSettings.CareLinkUserName.Equals(Me.UsernameTextBox.Text, StringComparison.OrdinalIgnoreCase) Then
                Me.UsernameTextBox.Text = My.Settings.CareLinkUserName
            End If

            My.Settings.CareLinkUserName = Me.UsernameTextBox.Text
            Me.PasswordTextBox.Text = userSettings.CareLinkPassword
            Me.RegionComboBox.SelectedValue = userSettings.CountryCode.GetRegionFromCode
            Me.CountryComboBox.Text = userSettings.CountryCode.GetCountryFromCode
        End If
    End Sub

End Class
