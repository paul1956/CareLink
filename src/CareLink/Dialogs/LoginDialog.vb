' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Net
Imports System.Net.Http

Public Class LoginDialog

    Private Const ComparisonType As StringComparison =
        StringComparison.OrdinalIgnoreCase

    Private _doCancel As Boolean
    Private _httpClient As HttpClient
    Private _initialHeight As Integer
    Private _mySource As AutoCompleteStringCollection
    Private _showTcs As TaskCompletionSource(Of DialogResult)
    Public Const CareLinkAuthTokenCookieName As String = "auth_tmp_token"

    Public Property ClientDiscover As DiscoveryRoot
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
    '''  Returns the ISO country code for the currently selected or typed country display name.
    '''  Falls back to the text when a mapping is not available.
    ''' </summary>
    Private Function GetSelectedCountryCode() As String
        Try
            Dim display As String = TryCast(Me.CountryComboBox.SelectedItem, String)
            If String.IsNullOrEmpty(value:=display) Then
                display = Me.CountryComboBox.Text
            End If
            If Not String.IsNullOrEmpty(value:=display) Then
                Dim code As String = Nothing
                Return If(s_countryToCodeList.TryGetValue(key:=display, value:=code),
                          code,
                          display)
            End If
        Catch
        End Try
        Return String.Empty
    End Function

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
            _showTcs.TrySetResult(result:=DialogResult.Cancel)
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
        ' ComboBox is bound to a list of display names (String). Resolve ISO country code and use it to get culture.
        Dim display As String = TryCast(Me.CountryComboBox.SelectedItem, String)
        If String.IsNullOrEmpty(value:=display) Then
            display = Me.CountryComboBox.Text
        End If

        Dim code As String = Nothing
        If Not String.IsNullOrEmpty(value:=display) AndAlso
           s_countryToCodeList.TryGetValue(key:=display, value:=code) Then
            CurrentDateCulture = code.GetCurrentDateCulture
        ElseIf Not String.IsNullOrEmpty(value:=display) Then
            CurrentDateCulture = display.GetCurrentDateCulture
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
        ElseIf IsNotNullOrWhiteSpace(value:=My.Settings.CareLinkUserName) Then
            _mySource.Add(value:=My.Settings.CareLinkUserName)
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
            ' s_regionList is a List(Of String) of region display names.
            ' Bind the list directly and use SelectedItem (a String) to read the chosen region.
            .DataSource = s_regionList
        End With

        If IsNullOrEmpty(value:=My.Settings.CountryCode) Then
            My.Settings.CountryCode = "US"
        End If

        ' Select the region display name matching the saved country code's region
        Dim savedRegionName As String = My.Settings.CountryCode.GetRegionFromCode()
        If Not String.IsNullOrEmpty(value:=savedRegionName) Then
            Me.RegionComboBox.SelectedItem = savedRegionName
        End If
        Dim initialRegionName As String =
            If(Not String.IsNullOrEmpty(value:=savedRegionName),
               savedRegionName,
               Me.RegionComboBox.GetItemText(item:=Me.RegionComboBox.SelectedItem))
        If String.IsNullOrEmpty(value:=initialRegionName) AndAlso Me.RegionComboBox.Items.Count > 0 Then
            initialRegionName = Me.RegionComboBox.GetItemText(item:=Me.RegionComboBox.Items(0))
            Me.RegionComboBox.SelectedItem = initialRegionName
        End If
        Me.PopulateCountriesForRegion(regionName:=initialRegionName)
        Dim savedCountry As String = My.Settings.CountryCode
        Dim matchedCountryIndex As Integer = -1
        Try
            If Not String.IsNullOrEmpty(value:=savedCountry) Then
                For i As Integer = 0 To Me.CountryComboBox.Items.Count - 1
                    Dim item As Object = Me.CountryComboBox.Items(index:=i)
                    Try
                        Dim displayName As String = item?.ToString()
                        Dim code As String = Nothing
                        If Not String.IsNullOrEmpty(value:=displayName) AndAlso s_countryToCodeList.TryGetValue(key:=displayName, value:=code) Then
                            If EqualsNoCase(a:=code, b:=savedCountry) OrElse
                                EqualsNoCase(a:=displayName, b:=savedCountry) Then
                                matchedCountryIndex = i
                                Exit For
                            End If
                        ElseIf EqualsNoCase(a:=displayName, b:=savedCountry) Then
                            matchedCountryIndex = i
                            Exit For
                        End If
                    Catch
                    End Try
                Next
            End If
        Catch
        End Try
        Try
            If matchedCountryIndex >= 0 Then
                Me.CountryComboBox.SelectedIndex = matchedCountryIndex
            ElseIf Me.CountryComboBox.Items.Count > 0 Then
                Me.CountryComboBox.SelectedIndex = 0
            End If
            If Me.CountryComboBox.SelectedIndex >= 0 Then
                Dim selectedItem As Object = Me.CountryComboBox.Items(Me.CountryComboBox.SelectedIndex)
                Dim display As String = selectedItem?.ToString()
                If Not String.IsNullOrEmpty(display) Then
                    Me.CountryComboBox.Text = display
                End If
            End If
        Catch
        End Try

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
        s_countryCode = Me.GetSelectedCountryCode()
        Try
            Me.LoginStatus.Text = "Checking token file..."
            Dim lastErrorMsg As String
            Me.ClientDiscover = Await GetDiscoveryDataAsync(countryCode:=s_countryCode)
            lastErrorMsg = Me.ClientDiscover.lastErrorMsg
            Dim discoveryTupleStatusCode As HttpStatusCode =
                Me.ClientDiscover.httpStatusCode
            If Me.ClientDiscover IsNot Nothing Then
                Me.Ok_Button.Enabled = False
                Application.DoEvents()
                Dim territory As String = TryCast(Me.RegionComboBox.SelectedItem, String)
                Dim serverMapping As String = GetServerMapping(regionName:=territory)
                Dim serverRegion As ServerLocation = If(serverMapping.EqualsNoCase("US"), ServerLocation.US, If(serverMapping.EqualsNoCase("EU"), ServerLocation.EU, ServerLocation.CLINICAL))
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

                My.Settings.CountryCode = Me.GetSelectedCountryCode()
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
                    _showTcs.TrySetResult(result:=DialogResult.OK)
                    Me.Close()
                Else
                    Me.DialogResult = DialogResult.OK
                    Me.Hide()
                End If
            Else
                discoveryTupleStatusCode =
                    If(discoveryTupleStatusCode = HttpStatusCode.OK,
                       Form1.Client.HttpStatusCode,
                       discoveryTupleStatusCode)
                Me.LoginStatus.Text = lastErrorMsg
                ReportLoginStatus(Me.LoginStatus, hasErrors:=True, lastErrorMsg, discoveryTupleStatusCode)
                If Client2.Auth_Error_Codes.Contains(value:=discoveryTupleStatusCode) Then
                    Me.PasswordTextBox.Text = String.Empty
                    Dim userRecord As CareLinkUserDataRecord = Nothing
                    If s_allUserSettingsData.TryGetValue(key:=GetUserName(), userRecord) Then
                        s_allUserSettingsData.Remove(value:=userRecord)
                    End If
                End If

                Dim networkDownMessage As String =
                    If(NetworkUnavailable(),
                       "Due to network being unavailable",
                       $"Network Response Code = {discoveryTupleStatusCode}")

                Dim heading As String

                Dim buttonsAvailable As MsgBoxStyle
                Dim buttonStyle As MsgBoxStyle
                If discoveryTupleStatusCode <> 1 Then
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
    '''  Handles the ServerLocation ComboBox selected key changed event,
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

        Dim selectedRegionName As String = Nothing
        Try
            If Me.RegionComboBox.SelectedIndex >= 0 Then
                selectedRegionName = Me.RegionComboBox.GetItemText(Me.RegionComboBox.SelectedItem)
            End If
        Catch
        End Try
        If String.IsNullOrEmpty(selectedRegionName) Then
            selectedRegionName = If(Not String.IsNullOrEmpty(Me.RegionComboBox.Text), Me.RegionComboBox.Text.Trim(), TryCast(Me.RegionComboBox.SelectedItem, String))
        End If
        Me.PopulateCountriesForRegion(regionName:=selectedRegionName)
    End Sub

    ''' <summary>
    '''  Populates the CountryComboBox for a specified region display name.
    ''' </summary>
    ''' <param name="regionName">The region display name.</param>
    Private Sub PopulateCountriesForRegion(regionName As String)
        Dim countriesInRegion As New Dictionary(Of String, String)(comparer:=StringComparer.OrdinalIgnoreCase)
        ' Clear any previous binding first to avoid conflicts when the selection changes
        Try
            Me.CountryComboBox.DataSource = Nothing
            Me.CountryComboBox.DisplayMember = String.Empty
            Me.CountryComboBox.ValueMember = String.Empty
        Catch
        End Try
        If String.IsNullOrEmpty(value:=regionName) Then
            Me.CountryComboBox.Enabled = False
            Exit Sub
        End If

        ' Populate countries whose region (string) equals the selected region display name
        For Each kvp As KeyValuePair(Of String, String) In s_countryNameToRegionList
            If EqualsNoCase(a:=kvp.Value, b:=regionName) Then
                Dim value As String = Nothing

                If s_countryToCodeList.TryGetValue(kvp.Key, value) Then
                    countriesInRegion.Add(kvp.Key, value)
                End If
            End If
        Next
        If countriesInRegion.Count > 0 Then
            ' Bind a concrete list of display strings for reliable startup selection
            Dim list As List(Of String) = countriesInRegion.Keys.ToList()
            Me.CountryComboBox.DisplayMember = String.Empty
            Me.CountryComboBox.ValueMember = String.Empty
            ' Ensure the control has a created handle and the correct binding context before binding
            Try
                Dim h As IntPtr = Me.CountryComboBox.Handle
            Catch
            End Try
            Try
                Me.CountryComboBox.BindingContext = Me.BindingContext
            Catch
            End Try
            ' Prevent flicker and ensure the control updates immediately when rebinding
            Try
                Me.CountryComboBox.BeginUpdate()
            Catch
            End Try
            Me.CountryComboBox.DataSource = New BindingSource(dataSource:=list, dataMember:=Nothing)
            Me.CountryComboBox.Enabled = True
            Try
                Me.CountryComboBox.EndUpdate()
            Catch
            End Try
            ' Force a visual refresh so the selected item text appears without needing to open the dropdown
            Try
                Me.CountryComboBox.Refresh()
                Me.CountryComboBox.Update()
            Catch
            End Try
            ' Try to select the saved country value via BindingSource.Position
            Try
                Dim saved As String = My.Settings.CountryCode
                Dim bs As BindingSource = TryCast(Me.CountryComboBox.DataSource, BindingSource)
                If bs IsNot Nothing Then
                    Dim desiredPos As Integer = -1
                    If Not String.IsNullOrEmpty(saved) Then
                        For i As Integer = 0 To bs.List.Count - 1
                            Dim item As Object = bs.List(i)
                            Try
                                Dim displayName As String = item?.ToString()
                                Dim code As String = Nothing
                                If Not String.IsNullOrEmpty(displayName) AndAlso s_countryToCodeList.TryGetValue(displayName, code) Then
                                    If EqualsNoCase(a:=code, b:=saved) OrElse EqualsNoCase(a:=displayName, b:=saved) Then
                                        desiredPos = i
                                        Exit For
                                    End If
                                ElseIf EqualsNoCase(a:=displayName, b:=saved) Then
                                    desiredPos = i
                                    Exit For
                                End If
                            Catch
                            End Try
                        Next
                    End If

                    If desiredPos >= 0 Then
                        bs.Position = desiredPos
                    ElseIf bs.List.Count > 0 Then
                        bs.Position = 0
                    End If

                    ' Set SelectedItem directly to the bound object and force visual sync
                    Try
                        Dim pos As Integer = bs.Position
                        If pos >= 0 AndAlso pos < bs.List.Count Then
                            Dim current As Object = bs.List(pos)
                            Me.CountryComboBox.SelectedItem = current
                            Try
                                Dim displayName As String = current?.ToString()
                                Me.CountryComboBox.Text = displayName
                            Catch
                                Me.CountryComboBox.Text = current?.ToString()
                            End Try
                            Try
                                Me.CountryComboBox.Refresh()
                                Me.CountryComboBox.Update()
                                Application.DoEvents()
                            Catch
                            End Try
                        End If
                    Catch
                    End Try

                    If bs.Position >= 0 AndAlso bs.Position < bs.List.Count Then
                        Dim selected As Object = bs.List(bs.Position)
                        Dim display As String = selected?.ToString()
                        If Not String.IsNullOrEmpty(display) Then
                            Me.CountryComboBox.Text = display
                        End If
                    End If
                End If
            Catch
                ' Ignore selection errors
            End Try
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
        ' No-op edit to update file timestamp.
        Try
            Dim userRecord As CareLinkUserDataRecord = Nothing
            If s_allUserSettingsData.TryGetValue(Me.UsernameComboBox.Text, userRecord) Then
                If userRecord.CareLinkUserName.EqualsNoCase(Me.UsernameComboBox.Text) Then
                    Me.UsernameComboBox.Text = userRecord.CareLinkUserName
                End If
                SetUserName(value:=Me.UsernameComboBox.Text)
                Me.PasswordTextBox.Text = userRecord.CareLinkPassword
                Dim userRegionName As String = userRecord.CountryCode.GetRegionFromCode()
                If Not String.IsNullOrEmpty(userRegionName) Then
                    Me.RegionComboBox.SelectedItem = userRegionName
                End If
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
        ' No-op edit to refresh file state.
        If Me.UsernameComboBox.SelectedValue IsNot Nothing AndAlso
           s_allUserSettingsData.TryGetValue(key, userRecord) Then

            If Not userRecord.CareLinkUserName.EqualsNoCase(Me.UsernameComboBox.Text) Then
                Me.UsernameComboBox.Text = userRecord.CareLinkUserName
            End If
            My.Settings.CareLinkUserName = Me.UsernameComboBox.Text
            Me.PasswordTextBox.Text = userRecord.CareLinkPassword
            Dim userRegionName2 As String = userRecord.CountryCode.GetRegionFromCode()
            If Not String.IsNullOrEmpty(userRegionName2) Then
                Me.RegionComboBox.SelectedItem = userRegionName2
            End If
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

        ' Show modelessly with owner so dialog is positioned properly
        Me.Show(owner)

        Dim result As DialogResult = Await _showTcs.Task

        If ownerForm IsNot Nothing Then
            ownerForm.Enabled = True
        End If

        Return result
    End Function

End Class
