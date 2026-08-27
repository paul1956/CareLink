' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Net
Imports System.Net.Http

Public Class LoginDialog
    Private _doCancel As Boolean
    Private _httpClient As HttpClient
    Private _initialHeight As Integer
    Private _mySource As AutoCompleteStringCollection
    Private _showTcs As TaskCompletionSource(Of DialogResult)
    Public Const CareLinkAuthTokenCookieName As String = "auth_tmp_token"

    Public Property ClientDiscover As DiscoveryRecord
    Public Property LoggedOnUser As CareLinkUserDataRecord
    Public Property LoginSourceAutomatic As FileToLoadOptions

    ''' <summary>
    '''  Updates the login status UI based on the result of the login attempt.
    ''' </summary>
    ''' <param name="loginStatus">The <see cref="TextBox"/> to display status.</param>
    ''' <param name="hasErrors">Indicates if errors occurred.</param>
    ''' <param name="lastErrorMsg">The last error message, if any.</param>
    ''' <param name="lastHttpStatusCode">The last HttpStatusCode code.</param>
    Private Shared Sub ReportLoginStatus(loginStatus As TextBox,
                                         hasErrors As Boolean,
                                         Optional lastErrorMsg As String = Nothing,
                                         Optional lastHttpStatusCode As Integer = HttpStatusCode.OK)

        If Client2.Auth_Error_Codes.Contains(lastHttpStatusCode) Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = "Invalid Login Credentials"
            My.Settings.AutoLogin = False
            Exit Sub
        End If

        If hasErrors Then
            loginStatus.ForeColor = Color.Red
            loginStatus.Text = If(lastErrorMsg, "Unknown Login Issue")
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
        If _showTcs IsNot Nothing Then
            _showTcs.TrySetResult(DialogResult.Cancel)
            Me.Close()
        Else
            Me.DialogResult = DialogResult.Cancel
            Me.Hide()
        End If
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
           IsNullOrWhiteSpace(value:=Me.PatientUserIDTextBox.Text) Then
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
            Dim selectedKVP As KeyValuePair(Of String, String) =
                CType(selectedValueObj, KeyValuePair(Of String, String))
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
        Me.LoggedOnUser = New CareLinkUserDataRecord(parent:=s_allUserSettingsData)
        Me.Icon = If(Application.IsDarkModeEnabled,
                     PngBitmapToIcon(original:=My.Resources.LoginLight),
                     PngBitmapToIcon(original:=My.Resources.LoginDark))

        _httpClient = New HttpClient()
        _httpClient.SetDefaultRequestHeaders()
        If _initialHeight = 0 Then
            _initialHeight = Me.Height
        End If
        Me.CenterFormOnAnother(reference:=Form1)

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

        _mySource = New AutoCompleteStringCollection()
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
            If IsNotNullOrWhiteSpace(value:=GetUserName()) Then
                .SelectedIndex = -1
            Else
            End If
            .Text = GetUserName()
            Me.PasswordTextBox.Text =
                If(s_allUserSettingsData?.ContainsKey(key:= .Text),
                   s_allUserSettingsData(itemName:= .Text).CareLinkPassword,
                   String.Empty)
        End With

        With Me.RegionComboBox
            .DisplayMember = NameOf(KeyValuePair(Of WorldRegion, String).Value)
            .ValueMember = NameOf(KeyValuePair(Of WorldRegion, String).Key)
            .DataSource = New BindingSource(dataSource:=s_regionDictionary, dataMember:=Nothing)
        End With

        If IsNullOrEmpty(value:=My.Settings.CountryCode) Then
            My.Settings.CountryCode = "US"
        End If

        Me.RegionComboBox.SelectedValue = My.Settings.CountryCode.GetRegionFromCode
        Me.CountryComboBox.SelectedValue = My.Settings.CountryCode

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
    Private Async Sub OK_Button_Click(sender As Object, e As EventArgs) Handles Ok_Button.Click
        If Me.UsernameComboBox.Text.Length = 0 Then
            Me.UsernameComboBox.Focus()
            Exit Sub
        End If
        If Me.PasswordTextBox.Text.Length = 0 Then
            Me.PasswordTextBox.Focus()
            Exit Sub
        End If

        SetUserName(value:=Me.UsernameComboBox.Text)
        s_password = Me.PasswordTextBox.Text
        s_countryCode = Me.CountryComboBox.SelectedValue.ToString
        Try
            Me.LoginStatus.Text = "Checking token file..."
            Dim lastErrorMsg As String
            Dim httpStatusCode As Integer = 0
            Dim discoveryTuple As (discoveryRecord As DiscoveryRecord, lastErrorMsg As String, httpStatusCode As Integer) =
                Await GetDiscoveryDataAsync()
            Me.ClientDiscover = discoveryTuple.discoveryRecord
            lastErrorMsg = discoveryTuple.lastErrorMsg
            httpStatusCode = discoveryTuple.httpStatusCode
            If Me.ClientDiscover IsNot Nothing Then
                Me.Ok_Button.Enabled = False
                Application.DoEvents()
                Dim value As String = Me.RegionComboBox.SelectedValue.ToString().Replace(oldValue:=" ", newValue:="")
                Dim serverRegion As Region = [Enum].Parse(Of Region)(value:=value)
                Await Client2.GetLoginData(serverRegion:=serverRegion,
                                           userName:=s_userName,
                                           password:=s_password,
                                           tokenData:=ReadTokenDataFile())
                Form1.Client = New Client2(serverRegion)
                Const loginFailed As String = "Login failed: Client.InitAsync() did not complete successfully."
                lastErrorMsg = If(Not Await Form1.Client.InitAsync(),
                                  loginFailed,
                                  Await Form1.Client.GetRecentDataAsync())
            End If
            If IsNullOrWhiteSpace(value:=lastErrorMsg) Then
                s_lastMedicalDeviceDataUpdateServerEpoch = 0
                ReportLoginStatus(Me.LoginStatus, hasErrors:=False, lastErrorMsg)

                Me.Ok_Button.Enabled = True
                Me.Cancel_Button.Enabled = True

                My.Settings.CountryCode = Me.CountryComboBox.SelectedValue.ToString
                My.Settings.CareLinkUserName = GetUserName()
                My.Settings.CareLinkPassword = Me.PasswordTextBox.Text
                My.Settings.CareLinkPatientUserID = Me.PatientUserIDTextBox.Text
                Dim checked As Boolean = Me.CarePartnerCheckBox.Checked
                My.Settings.CareLinkPartner = checked OrElse IsNotNullOrWhiteSpace(value:=Me.PatientUserIDTextBox.Text)
                My.Settings.Save()
                Dim key As String = GetUserName()
                If Not s_allUserSettingsData.TryGetValue(key, userRecord:=Me.LoggedOnUser) Then
                    s_allUserSettingsData.SaveAllUserRecords(
                        loggedOnUser:=New CareLinkUserDataRecord(parent:=s_allUserSettingsData),
                        key:=NameOf(CareLinkUserDataRecord.CareLinkUserName), value:=GetUserName())
                End If
                If _showTcs IsNot Nothing Then
                    _showTcs.TrySetResult(DialogResult.OK)
                    Me.Close()
                Else
                    Me.DialogResult = DialogResult.OK
                    Me.Hide()
                End If
            Else
                httpStatusCode = If(httpStatusCode <> 0,
                                    httpStatusCode,
                                    Form1.Client.HttpStatusCode)
                Me.LoginStatus.Text = lastErrorMsg
                ReportLoginStatus(Me.LoginStatus, hasErrors:=True, lastErrorMsg, httpStatusCode)
                If Client2.Auth_Error_Codes.Contains(value:=httpStatusCode) Then
                    Me.PasswordTextBox.Text = String.Empty
                    Dim userRecord As CareLinkUserDataRecord = Nothing
                    If s_allUserSettingsData.TryGetValue(key:=GetUserName(), userRecord) Then
                        s_allUserSettingsData.Remove(value:=userRecord)
                    End If
                End If

                Dim networkDownMessage As String =
                    If(NetworkUnavailable(),
                       "Due to network being unavailable",
                       $"Network Response Code = {httpStatusCode}")

                Dim heading As String

                Dim buttonsAvailable As MsgBoxStyle
                Dim buttonStyle As MsgBoxStyle
                If httpStatusCode <> 1 Then
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
                        If _showTcs IsNot Nothing Then
                            _showTcs.TrySetResult(result:=DialogResult.Ignore)
                            Me.Close()
                        Else
                            Me.DialogResult = DialogResult.Ignore
                        End If
                    Case MsgBoxResult.Retry
                        If _showTcs IsNot Nothing Then
                            _showTcs.TrySetResult(result:=DialogResult.Retry)
                        Else
                            Me.DialogResult = DialogResult.Retry
                        End If
                    Case MsgBoxResult.Ok
                        If _showTcs IsNot Nothing Then
                            _showTcs.TrySetResult(result:=DialogResult.OK)
                            Me.Close()
                        Else
                            Me.DialogResult = DialogResult.OK
                        End If
                    Case MsgBoxResult.Cancel
                        If _showTcs IsNot Nothing Then
                            _showTcs.TrySetResult(result:=DialogResult.Cancel)
                            Me.Close()
                        Else
                            Me.DialogResult = DialogResult.Cancel
                        End If
                End Select
            End If
        Catch ex As Exception
            Stop
        Finally
            Me.Ok_Button.Enabled = True
            Me.Cancel_Button.Enabled = True
        End Try
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
        Dim selectedRegion As WorldRegion = s_regionToServerMapping.Keys(index:=Me.RegionComboBox.SelectedIndex)
        For Each kvp As KeyValuePair(Of String, WorldRegion) In s_countryNameToRegionList
            If kvp.Value = selectedRegion Then
                countriesInRegion.Add(kvp.Key, value:=s_countryToCodeList(kvp.Key))
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
                SetUserName(value:=Me.UsernameComboBox.Text)
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
            Stop
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
        If Me.UsernameComboBox.SelectedValue IsNot Nothing AndAlso
           s_allUserSettingsData.TryGetValue(key, userRecord) Then

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
    Private Sub UsernameComboBox_Validating(sender As Object, e As CancelEventArgs) _
        Handles UsernameComboBox.Validating

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
    ''' Shows the dialog in an async-friendly way. Disables the owner to emulate modal behavior
    ''' and returns when the dialog completes (OK/Cancel/Retry/etc.).
    ''' </summary>
    Public Overloads Async Function ShowDialogAsync(owner As IWin32Window) As Task(Of DialogResult)
        If _showTcs Is Nothing Then
            _showTcs = New TaskCompletionSource(Of DialogResult)
        End If

        Dim ownerForm As Form = TryCast(owner, Form)
        If ownerForm IsNot Nothing Then
            ownerForm.Enabled = False
        End If

        If Me.Visible Then
            Me.Visible = False
        End If
        ' Show modelessly with owner so dialog is positioned properly
        Me.Show(owner)

        Dim result As DialogResult = Await _showTcs.Task

        If ownerForm IsNot Nothing Then
            ownerForm.Enabled = True
        End If

        Return result
    End Function

End Class
