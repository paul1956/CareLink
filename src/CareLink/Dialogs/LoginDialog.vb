' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Reflection.Metadata

Public Class LoginDialog
    Private ReadOnly _mySource As New AutoCompleteStringCollection()
    Private _doCancel As Boolean
    Private _httpClient As HttpClient
    Private _initialHeight As Integer = 0
    Public Const CareLinkAuthTokenCookieName As String = "auth_tmp_token"
    Public Property Client As Client2
    Public Property ClientDiscover As ConfigRecord
    Public Property LoggedOnUser As New CareLinkUserDataRecord(s_allUserSettingsData)
    Public Property LoginSourceAutomatic As FileToLoadOptions = FileToLoadOptions.NewUser

    ''' <summary>
    '''  Updates the login status UI based on the result of the login attempt.
    ''' </summary>
    ''' <param name="loginStatus">The <see cref="TextBox"/> to display status.</param>
    ''' <param name="hasErrors">Indicates if errors occurred.</param>
    ''' <param name="lastErrorMessage">The last error message, if any.</param>
    ''' <param name="lastHttpStatusCode">The last HttpStatusCode code.</param>
    Private Shared Sub ReportLoginStatus(
        loginStatus As TextBox,
        hasErrors As Boolean,
        Optional lastErrorMessage As String = Nothing,
        Optional lastHttpStatusCode As Integer = HttpStatusCode.OK)

        If Client2.Auth_Error_Codes.Contains(lastHttpStatusCode) Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = "Invalid Login Credentials"
            My.Settings.AutoLogin = False
            Exit Sub
        End If

        If hasErrors Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = If(lastErrorMessage, "Unknown Login Issue")
            My.Settings.AutoLogin = False
        Else
            loginStatus.ForeColor = Color.Black
            loginStatus.Text = "OK"
        End If
    End Sub

    ''' <summary>
    '''  Handles the Cancel button click event, setting a flag to indicate cancellation.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This method sets a flag to indicate that the operation was cancelled
    '''  and hides the dialog.
    ''' </remarks>
    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        _doCancel = True
        Me.DialogResult = DialogResult.Cancel
        Me.Hide()
    End Sub

    ''' <summary>
    '''  Handles the Care Partner checkbox checked change event,
    '''  toggling visibility of the Patient User ID controls.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  If the Care Partner checkbox is checked, the Patient User ID label
    '''  and textbox are made visible. If unchecked, they are hidden.
    ''' </remarks>
    Private Sub CarePartnerCheckBox_CheckedChanged(sender As Object, e As EventArgs) _
        Handles CarePartnerCheckBox.CheckedChanged

        Dim careLinkPartner As Boolean = Me.CarePartnerCheckBox.Checked
        Me.PatientUserIDLabel.Visible = careLinkPartner
        Me.PatientUserIDTextBox.Visible = careLinkPartner
        If careLinkPartner AndAlso
           IsNullOrWhiteSpace(Me.PatientUserIDTextBox.Text) Then
            Me.PatientUserIDTextBox.Focus()
        End If
    End Sub

    ''' <summary>
    '''  Handles the Country ComboBox selected value changed event,
    '''  updating the current date culture.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This method updates the CurrentDateCulture based on the selected
    '''  country in the CountryComboBox.
    ''' </remarks>
    Private Sub CountryComboBox_SelectedValueChanged(sender As Object, e As EventArgs) _
        Handles CountryComboBox.SelectedValueChanged

        Dim selectedValueObj As Object = Me.CountryComboBox.SelectedValue
        If TypeOf selectedValueObj Is String Then
            CurrentDateCulture = selectedValueObj.ToString.GetCurrentDateCulture
        Else
            Dim selectedKVP As KeyValuePair(Of String, String) = CType(selectedValueObj, KeyValuePair(Of String, String))
            CurrentDateCulture = selectedKVP.Value.GetCurrentDateCulture
        End If
    End Sub

    ''' <summary>
    '''  Handles the dialog <see cref="Load"/> event,
    '''  initializes the form controls and settings.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This method sets the dialog icon, initializes the HTTP client,
    '''  loads user settings, and populates the username and region combo boxes.
    ''' </remarks>
    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = If(Application.IsDarkModeEnabled,
                     PngBitmapToIcon(original:=My.Resources.LoginLight),
                     PngBitmapToIcon(original:=My.Resources.LoginDark))

        _httpClient = New HttpClient()
        _httpClient.SetDefaultRequestHeaders()
        If _initialHeight = 0 Then
            _initialHeight = Me.Height
        End If

        Dim commandLineArguments As String() = Environment.GetCommandLineArgs()

        If commandLineArguments.Length > 1 Then
            Dim userRecord As CareLinkUserDataRecord = Nothing
            Dim param As String = commandLineArguments(1)
            Select Case True
                Case param.StartsWithNoCase(value:="/Safe")
                    My.Settings.AutoLogin = False
                    My.Settings.Save()

                     ' username=name
                Case param.StartsWithNoCase(value:="UserName")
                    Dim arg As String() = param.Split(separator:="=")
                    If arg.Length = 2 AndAlso
                       s_allUserSettingsData.TryGetValue(key:=arg(1), userRecord) Then
                        userRecord.UpdateSettings()
                    End If
            End Select
        End If

        If AllUserLoginInfoFileExists() Then
            _mySource.AddRange(s_allUserSettingsData.Keys.ToArray)
            Me.UsernameComboBox.DataSource = s_allUserSettingsData.Keys
        ElseIf IsNotNullOrWhiteSpace(My.Settings.CareLinkUserName) Then
            _mySource.Add(My.Settings.CareLinkUserName)
            Me.UsernameComboBox.Text = My.Settings.CareLinkUserName
        Else
            _mySource.Clear()
            Me.UsernameComboBox.Text = String.Empty
        End If
        With Me.UsernameComboBox
            .AutoCompleteCustomSource = _mySource
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
            If s_userName = String.Empty Then
                .Text = My.Settings.CareLinkUserName
            Else
                .SelectedIndex = -1
                .Text = s_userName
            End If
            Me.PasswordTextBox.Text = If(s_allUserSettingsData?.ContainsKey(key:= .Text),
                                         s_allUserSettingsData(itemName:= .Text).CareLinkPassword,
                                         String.Empty)
        End With

        Me.RegionComboBox.DataSource = New BindingSource(dataSource:=s_regionList, dataMember:=Nothing)
        Me.RegionComboBox.DisplayMember = "Key"
        Me.RegionComboBox.ValueMember = "Value"
        If IsNullOrEmpty(value:=My.Settings.CountryCode) Then
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

    ''' <summary>
    '''  Handles the dialog <see cref="Shown"/> event, sets the initial
    '''  height and visibility.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This method sets the dialog's height to the initial height and makes it visible.
    '''  If the login source is automatic, it triggers the OK button click event.
    ''' </remarks>
    Private Sub LoginForm1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Height = _initialHeight
        Me.Visible = True
        If Me.LoginSourceAutomatic = FileToLoadOptions.Login Then
            Me.OK_Button_Click(sender:=Nothing, e:=Nothing)
        End If
    End Sub

    ''' <summary>
    '''  Handles the OK button click event, validates input and attempts to log in.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles Ok_Button.Click
        If Me.UsernameComboBox.Text.Length = 0 Then
            Me.UsernameComboBox.Focus()
            Exit Sub
        End If
        If Me.PasswordTextBox.Text.Length = 0 Then
            Me.PasswordTextBox.Focus()
            Exit Sub
        End If

        s_userName = Me.UsernameComboBox.Text
        s_password = Me.PasswordTextBox.Text
        s_countryCode = Me.CountryComboBox.SelectedValue.ToString

        Dim lastErrorMessage As String = String.Empty
        Dim lastHttpStatusCode As Integer = 0
        Me.ClientDiscover = GetDiscoveryData(lastErrorMessage, lastHttpStatusCode)
        If Me.ClientDiscover IsNot Nothing Then
            Me.Ok_Button.Enabled = False
            Dim tokenData As TokenData = ReadTokenDataFile(s_userName)
            If tokenData Is Nothing Then
                ' Get the embedded EXE as a byte array
                Dim buffer() As Byte = My.Resources.carelink_carepartner_api_login
                ' Create a temporary file for the EXE
                Dim exePath As String = $"{Path.GetTempFileName()}.exe"
                ' Write the EXE to the temporary file
                Using fs As New FileStream(path:=exePath, mode:=FileMode.Create)
                    fs.Write(buffer, offset:=0, count:=buffer.Length)
                    fs.Flush()
                End Using

                Dim isUsRegion As Boolean = Me.RegionComboBox.SelectedValue.ToString = "North America"
                Dim isUsRegionStr As String = If(isUsRegion,
                                                 "--us",
                                                 String.Empty)

                ' Create a temporary file for the JSON output
                Dim sourceFileName As String = $"{Path.GetTempFileName()}.json"
                Dim startInfo As New ProcessStartInfo With {
                    .FileName = exePath,
                    .Arguments = $"{If(isUsRegion, "--us ", String.Empty)} --output {sourceFileName}",
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .UseShellExecute = False}

                Dim process As New Process With {.StartInfo = startInfo}
                process.Start()

                Dim outputText As String = process.StandardOutput.ReadToEnd()
                Dim standardError As String = process.StandardError.ReadToEnd()
                process.WaitForExit()

                If process.ExitCode = 0 Then
                    Dim destinationFileName As String = GetLoginDataFileName(s_userName)
                    If File.Exists(path:=destinationFileName) Then
                        File.Delete(path:=destinationFileName)
                    End If
                    File.Move(sourceFileName, destinationFileName)
                End If
                File.Delete(exePath)
            End If

            'DoLogin(_httpClient, s_userName, isUsRegion)
            Me.Client = New Client2()
            Me.Client.Init()

            lastErrorMessage = Me.Client.GetRecentData()
        End If
        If IsNullOrWhiteSpace(lastErrorMessage) Then
            s_lastMedicalDeviceDataUpdateServerEpoch = 0
            ReportLoginStatus(Me.LoginStatus, hasErrors:=False, lastErrorMessage)

            Me.Ok_Button.Enabled = True
            Me.Cancel_Button.Enabled = True

            My.Settings.CountryCode = Me.CountryComboBox.SelectedValue.ToString
            My.Settings.CareLinkUserName = s_userName
            My.Settings.CareLinkPassword = Me.PasswordTextBox.Text
            My.Settings.CareLinkPatientUserID = Me.PatientUserIDTextBox.Text
            Dim checked As Boolean = Me.CarePartnerCheckBox.Checked
            My.Settings.CareLinkPartner = checked OrElse IsNotNullOrWhiteSpace(value:=Me.PatientUserIDTextBox.Text)
            My.Settings.Save()
            Dim key As String = s_userName
            If Not s_allUserSettingsData.TryGetValue(key, userRecord:=Me.LoggedOnUser) Then
                s_allUserSettingsData.SaveAllUserRecords(
                    loggedOnUser:=New CareLinkUserDataRecord(parent:=s_allUserSettingsData),
                    key:=NameOf(CareLinkUserDataRecord.CareLinkUserName), value:=s_userName)
            End If
            Me.DialogResult = DialogResult.OK
            Me.Hide()
        Else
            lastHttpStatusCode = If(lastHttpStatusCode <> 0,
                                    lastHttpStatusCode,
                                    Me.Client.GetHttpStatusCode)
            Me.LoginStatus.Text = lastErrorMessage
            ReportLoginStatus(Me.LoginStatus, hasErrors:=True, lastErrorMessage, lastHttpStatusCode)
            If Client2.Auth_Error_Codes.Contains(value:=lastHttpStatusCode) Then
                Me.PasswordTextBox.Text = String.Empty
                Dim userRecord As CareLinkUserDataRecord = Nothing
                If s_allUserSettingsData.TryGetValue(s_userName, userRecord) Then
                    s_allUserSettingsData.Remove(userRecord)
                End If
            End If

            Dim networkDownMessage As String = If(NetworkUnavailable(),
                                                  "Due to network being unavailable",
                                                  $"Network unavailable Response Code = {lastHttpStatusCode}")

            Dim heading As String

            Dim buttonsAvailable As MsgBoxStyle
            Dim buttonStyle As MsgBoxStyle
            If lastHttpStatusCode <> 1 Then
                buttonsAvailable = MsgBoxStyle.AbortRetryIgnore
                buttonStyle = buttonsAvailable Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
                heading = $"Login Unsuccessful, try again?{vbCrLf}Abort, will exit program!"
            Else
                buttonsAvailable = MsgBoxStyle.Critical
                buttonStyle = buttonsAvailable Or MsgBoxStyle.DefaultButton1 Or MsgBoxStyle.Critical
                heading = $"Network down?{vbCrLf}Ok, will exit program!"
            End If

            Const title As String = "Login Failed"
            Dim msgBoxResult As MsgBoxResult = MsgBox(heading, prompt:=networkDownMessage, buttonStyle, title)

            Select Case msgBoxResult
                Case MsgBoxResult.Abort
                    End
                Case MsgBoxResult.Ignore
                    Me.DialogResult = DialogResult.Ignore
                Case MsgBoxResult.Retry
                    Me.DialogResult = DialogResult.Retry
                Case MsgBoxResult.Ok
                    Me.DialogResult = DialogResult.OK
                Case MsgBoxResult.Cancel
                    Me.DialogResult = DialogResult.Cancel
            End Select
        End If
        Me.Cancel_Button.Enabled = True
    End Sub

    ''' <summary>
    '''  Handles the Password TextBox validating event, ensures password is not empty.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="CancelEventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  If the password is empty, it cancels the event and focuses on the PasswordTextBox.
    '''  If a username is selected, it enables the OK button.
    ''' </remarks>
    Private Sub PasswordTextBox_Validating(sender As Object, e As CancelEventArgs) Handles PasswordTextBox.Validating
        If IsNullOrWhiteSpace(Me.PasswordTextBox.Text) Then
            e.Cancel = True
            Me.PasswordTextBox.Focus()
        Else
            If Me.UsernameComboBox.Text.Length > 0 Then
                Me.Ok_Button.Enabled = True
            Else
                Me.UsernameComboBox.Focus()
            End If
        End If

    End Sub

    ''' <summary>
    '''  Handles the Region ComboBox selected index changed event,
    '''  updates the Country ComboBox based on the selected region.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This method populates the CountryComboBox with countries from the selected region.
    ''' </remarks>
    Private Sub RegionComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles RegionComboBox.SelectedIndexChanged

        Dim countriesInRegion As New Dictionary(Of String, String)
        Dim selectedRegion As String = s_regionList.Values(index:=Me.RegionComboBox.SelectedIndex)
        For Each kvp As KeyValuePair(Of String, String) In s_regionCountryList
            If kvp.Value = selectedRegion Then
                countriesInRegion.Add(kvp.Key, value:=s_countryCodeList(kvp.Key))
            End If
        Next
        If countriesInRegion.Count > 0 Then
            Me.CountryComboBox.DataSource = New BindingSource(dataSource:=countriesInRegion, dataMember:=Nothing)
            Me.CountryComboBox.DisplayMember = "Key"
            Me.CountryComboBox.ValueMember = "Value"
            Me.CountryComboBox.Enabled = True
        Else
            Me.CountryComboBox.Enabled = False
        End If
    End Sub

    ''' <summary>
    '''  Handles the Show Password checkbox checked change event,
    '''  toggles the visibility of the password in the PasswordTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">
    '''  The <see cref="EventArgs"/> instance containing the event data.
    ''' </param>
    ''' <remarks>
    '''  If the checkbox is checked, the password is shown as plain text;
    '''  if unchecked, it is masked with an asterisk character.
    ''' </remarks>
    Private Sub ShowPasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) _
        Handles ShowPasswordCheckBox.CheckedChanged

        Me.PasswordTextBox.PasswordChar = If(Me.ShowPasswordCheckBox.Checked,
                                             Nothing,
                                             "*"c)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="UsernameComboBox"/> leave event,
    '''  loads user settings for the entered username.
    ''' </summary>
    Private Sub UsernameComboBox_Leave(sender As Object, e As EventArgs) Handles UsernameComboBox.Leave
        Try

            Dim userRecord As CareLinkUserDataRecord = Nothing
            If s_allUserSettingsData.TryGetValue(Me.UsernameComboBox.Text, userRecord) Then
                If userRecord.CareLinkUserName.EqualsNoCase(Me.UsernameComboBox.Text) Then
                    Me.UsernameComboBox.Text = userRecord.CareLinkUserName
                End If
                s_userName = Me.UsernameComboBox.Text
                Me.PasswordTextBox.Text = userRecord.CareLinkPassword
                Me.RegionComboBox.SelectedValue = userRecord.CountryCode.GetRegionFromCode
                Me.PatientUserIDTextBox.Text = userRecord.CareLinkPatientUserID
                Me.CountryComboBox.Text = userRecord.CountryCode.GetCountryFromCode
                Me.CarePartnerCheckBox.Checked = userRecord.CareLinkPartner
            Else
                Me.PasswordTextBox.Text = String.Empty
                Me.RegionComboBox.SelectedIndex = 0
                Me.PatientUserIDTextBox.Text = String.Empty
                Me.CountryComboBox.Text = String.Empty
                Me.CarePartnerCheckBox.Checked = False
            End If
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    '''  Handles the <see cref="UsernameComboBox"/> selection change committed event,
    '''  loads user settings for the selected username.
    ''' </summary>
    Private Sub UsernameComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) _
        Handles UsernameComboBox.SelectionChangeCommitted

        Dim userRecord As CareLinkUserDataRecord = Nothing
        Dim key As String = Me.UsernameComboBox.SelectedValue.ToString
        If Me.UsernameComboBox.SelectedValue IsNot Nothing AndAlso s_allUserSettingsData.TryGetValue(key, userRecord) Then
            If Not userRecord.CareLinkUserName.EqualsNoCase(Me.UsernameComboBox.Text) Then
                Me.UsernameComboBox.Text = userRecord.CareLinkUserName
            End If
            My.Settings.CareLinkUserName = Me.UsernameComboBox.Text
            Me.PasswordTextBox.Text = userRecord.CareLinkPassword
            Me.RegionComboBox.SelectedValue = userRecord.CountryCode.GetRegionFromCode
            Me.PatientUserIDTextBox.Text = userRecord.CareLinkPatientUserID
            Me.CountryComboBox.Text = userRecord.CountryCode.GetCountryFromCode
            Me.CarePartnerCheckBox.Checked = userRecord.CareLinkPartner
        End If

    End Sub

    ''' <summary>
    '''  Handles the <see cref="UsernameComboBox"/> validating event,
    '''  ensures username is not empty.
    ''' </summary>
    Private Sub UsernameComboBox_Validating(sender As Object, e As CancelEventArgs) Handles UsernameComboBox.Validating
        If IsNullOrWhiteSpace(value:=Me.UsernameComboBox.Text) Then
            e.Cancel = True
            Me.UsernameComboBox.Focus()
        Else
            If Me.PasswordTextBox.Text.Length > 0 Then
                Me.Ok_Button.Enabled = True
            Else
                Me.PasswordTextBox.Focus()
            End If
        End If
    End Sub

    ''' <summary>
    '''  Overrides the OnHandleCreated method to enable dark mode
    '''  for the dialog when its handle is created.
    ''' </summary>
    ''' <param name="e">The event data.</param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        EnableDarkMode(hwnd:=Me.Handle)
    End Sub

End Class
