' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

Public Class LoginForm1
    Private ReadOnly _mySource As New AutoCompleteStringCollection()
    Public Property Client As CareLinkClient
    Public Property LoggedOnUser As New CareLinkUserDataRecord(s_allUserSettingsData)

    Private Shared Sub ReportLoginStatus(loginStatus As TextBox, hasErrors As Boolean, Optional lastErrorMessage As String = "")
        If hasErrors Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = lastErrorMessage
            My.Settings.AutoLogin = False
        Else
            loginStatus.ForeColor = Color.Black
            loginStatus.Text = "OK"
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CarePartnerCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CarePartnerCheckBox.CheckedChanged
        Dim careLinkPartner As Boolean = Me.CarePartnerCheckBox.Checked
        Me.PatientUserIDLabel.Visible = careLinkPartner
        Me.PatientUserIDTextBox.Visible = careLinkPartner
        If careLinkPartner AndAlso String.IsNullOrWhiteSpace(Me.PatientUserIDTextBox.Text) Then
            Me.PatientUserIDTextBox.Focus()
        End If
    End Sub

    Private Sub CountryComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles CountryComboBox.SelectedValueChanged
        If TypeOf Me.CountryComboBox.SelectedValue Is String Then
            CurrentDateCulture = Me.CountryComboBox.SelectedValue.ToString.GetCurrentDateCulture
        Else
            CurrentDateCulture = CType(Me.CountryComboBox.SelectedValue, KeyValuePair(Of String, String)).Value.GetCurrentDateCulture
        End If
    End Sub

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim commandLineArguments As String() = Environment.GetCommandLineArgs()

        If commandLineArguments.Length > 1 Then
            Dim userRecord As CareLinkUserDataRecord = Nothing
            Dim param As String = commandLineArguments(1)
            Select Case True
                Case param.StartsWith("/Safe", StringComparison.InvariantCultureIgnoreCase)
                    My.Settings.AutoLogin = False
                    My.Settings.Save()
                Case param.StartsWith("UserName", StringComparison.InvariantCultureIgnoreCase) ' username=name
                    Dim arg As String() = param.Split("=")
                    If arg.Length = 2 AndAlso s_allUserSettingsData.TryGetValue(arg(1), userRecord) Then
                        userRecord.UpdateSettings()
                    End If
            End Select
        End If

        If AllUserLoginInfoFileExists() Then
            _mySource.AddRange(s_allUserSettingsData.Keys.ToArray)
            Me.UsernameComboBox.DataSource = s_allUserSettingsData.Keys
        ElseIf Not String.IsNullOrWhiteSpace(My.Settings.CareLinkUserName) Then
            _mySource.Add(My.Settings.CareLinkUserName)
            Me.UsernameComboBox.Text = My.Settings.CareLinkUserName
        Else
            _mySource.Clear()
            Me.UsernameComboBox.Text = ""
        End If

        With Me.UsernameComboBox
            .AutoCompleteCustomSource = _mySource
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
            .Text = My.Settings.CareLinkUserName
        End With

        If s_allUserSettingsData?.ContainsKey(My.Settings.CareLinkUserName) Then
            Me.PasswordTextBox.Text = s_allUserSettingsData(My.Settings.CareLinkUserName).CareLinkPassword
        Else
            Me.PasswordTextBox.Text = My.Settings.CareLinkPassword
        End If

        Me.RegionComboBox.DataSource = New BindingSource(s_regionList, Nothing)
        Me.RegionComboBox.DisplayMember = "Key"
        Me.RegionComboBox.ValueMember = "Value"
        If String.IsNullOrEmpty(My.Settings.CountryCode) Then
            My.Settings.CountryCode = "US"
        End If
        Me.RegionComboBox.SelectedValue = My.Settings.CountryCode.GetRegionFromCode
        Me.CountryComboBox.Text = My.Settings.CountryCode.GetCountryFromCode

        Me.PatientUserIDTextBox.Text = My.Settings.CareLinkPatientUserID
        Dim careLinkPartner As Boolean = My.Settings.CareLinkPartner
        Me.PatientUserIDLabel.Visible = careLinkPartner
        Me.PatientUserIDTextBox.Visible = careLinkPartner
        Me.CarePartnerCheckBox.Checked = careLinkPartner
    End Sub

    Private Sub LoginForm1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If My.Settings.AutoLogin Then
            Me.OK_Button_Click(Me.Ok_Button, Nothing)
        End If
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As System.EventArgs) Handles Ok_Button.Click
        If Me.UsernameComboBox.Text.Length = 0 Then
            Me.UsernameComboBox.Focus()
            Exit Sub
        End If
        If Me.PasswordTextBox.Text.Length = 0 Then
            Me.PasswordTextBox.Focus()
            Exit Sub
        End If
        Me.Ok_Button.Enabled = False
        Me.Cancel_Button.Enabled = False
        Dim countryCode As String = Me.CountryComboBox.SelectedValue.ToString
        My.Settings.CountryCode = countryCode
        Me.Client = New CareLinkClient(Me.UsernameComboBox.Text, Me.PasswordTextBox.Text, countryCode)
        If Not Me.Client.LoggedIn Then
            Dim savePatientID As String = My.Settings.CareLinkPatientUserID
            My.Settings.CareLinkPatientUserID = Me.PatientUserIDTextBox.Text
            Dim recentData As Dictionary(Of String, String) = Me.Client.GetRecentData(My.Forms.Form1)
            If recentData?.Count > 0 Then
                ReportLoginStatus(Me.LoginStatus, False)

                Me.Ok_Button.Enabled = True
                Me.Cancel_Button.Enabled = True
                My.Settings.CareLinkUserName = Me.UsernameComboBox.Text
                My.Settings.CareLinkPassword = Me.PasswordTextBox.Text
                My.Settings.CareLinkPatientUserID = Me.PatientUserIDTextBox.Text
                My.Settings.CareLinkPartner = Me.CarePartnerCheckBox.Checked OrElse Not String.IsNullOrWhiteSpace(Me.PatientUserIDTextBox.Text)
                My.Settings.Save()
                If Not s_allUserSettingsData.TryGetValue(Me.UsernameComboBox.Text, Me.LoggedOnUser) Then
                    s_allUserSettingsData.SaveAllUserRecords(New CareLinkUserDataRecord(s_allUserSettingsData), NameOf(CareLinkUserDataRecord.CareLinkUserName), Me.UsernameComboBox.Text)
                End If
                Me.DialogResult = DialogResult.OK
                Me.Hide()
                Exit Sub
            Else
                My.Settings.CareLinkPatientUserID = savePatientID
                ReportLoginStatus(Me.LoginStatus, True, Me.Client.GetLastErrorMessage)
            End If
        Else
            Me.Ok_Button.Enabled = True
            Me.Cancel_Button.Enabled = True
            Me.DialogResult = DialogResult.OK
            Me.Hide()
            Exit Sub
        End If

        Dim networkDownMessage As String = If(NetworkDown, "due to network being down", $"'{Me.Client.GetLastErrorMessage}'")
        Dim result As MsgBoxResult = MsgBox($"Login Unsuccessful, {networkDownMessage}. try again? If 'Abort' program will exit!", MsgBoxStyle.AbortRetryIgnore, Title:="Login Failed")
        Select Case result
            Case MsgBoxResult.Abort
                End
            Case MsgBoxResult.Ignore
                Me.DialogResult = DialogResult.Ignore
            Case MsgBoxResult.Retry
                Me.DialogResult = DialogResult.Retry
        End Select
        Me.Ok_Button.Enabled = True
        Me.Cancel_Button.Enabled = True
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

    Private Sub UsernameComboBox_Leave(sender As Object, e As EventArgs) Handles UsernameComboBox.Leave
        Try

            Dim userSettings As CareLinkUserDataRecord = Nothing
            If s_allUserSettingsData.TryGetValue(Me.UsernameComboBox.Text, userSettings) Then
                If userSettings.CareLinkUserName.Equals(Me.UsernameComboBox.Text, StringComparison.OrdinalIgnoreCase) Then
                    Me.UsernameComboBox.Text = userSettings.CareLinkUserName
                End If
                My.Settings.CareLinkUserName = Me.UsernameComboBox.Text
                Me.PasswordTextBox.Text = userSettings.CareLinkPassword
                Me.RegionComboBox.SelectedValue = userSettings.CountryCode.GetRegionFromCode
                Me.PatientUserIDTextBox.Text = userSettings.CareLinkPatientUserID
                Me.CountryComboBox.Text = userSettings.CountryCode.GetCountryFromCode
                Me.CarePartnerCheckBox.Checked = userSettings.CareLinkPartner
            Else
                Me.PasswordTextBox.Text = ""
                Me.RegionComboBox.SelectedIndex = 0
                Me.PatientUserIDTextBox.Text = ""
                Me.CountryComboBox.Text = ""
                Me.CarePartnerCheckBox.Checked = False
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub UsernameComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles UsernameComboBox.SelectionChangeCommitted
        Dim userSettings As CareLinkUserDataRecord = Nothing

        If Me.UsernameComboBox.SelectedValue IsNot Nothing AndAlso s_allUserSettingsData.TryGetValue(Me.UsernameComboBox.SelectedValue.ToString, userSettings) Then
            If Not userSettings.CareLinkUserName.Equals(Me.UsernameComboBox.Text, StringComparison.OrdinalIgnoreCase) Then
                Me.UsernameComboBox.Text = userSettings.CareLinkUserName
            End If
            My.Settings.CareLinkUserName = Me.UsernameComboBox.Text
            Me.PasswordTextBox.Text = userSettings.CareLinkPassword
            Me.RegionComboBox.SelectedValue = userSettings.CountryCode.GetRegionFromCode
            Me.PatientUserIDTextBox.Text = userSettings.CareLinkPatientUserID
            Me.CountryComboBox.Text = userSettings.CountryCode.GetCountryFromCode
            Me.CarePartnerCheckBox.Checked = userSettings.CareLinkPartner
        End If

    End Sub

    Private Sub UsernameComboBox_Validating(sender As Object, e As CancelEventArgs) Handles UsernameComboBox.Validating
        If String.IsNullOrWhiteSpace(Me.UsernameComboBox.Text) Then
            e.Cancel = True
            Me.UsernameComboBox.Focus()
        End If
    End Sub

End Class
