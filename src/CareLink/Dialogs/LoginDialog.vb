' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Net
Imports System.Threading
Imports Microsoft.Web.WebView2.Core
Imports WebView2.DevTools.Dom

Public Class LoginDialog
    Private WithEvents DevContext As WebView2DevToolsContext

    Private ReadOnly _ignoredURLs As New List(Of String) From {
                 "https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/consent"}

    Private ReadOnly _mySource As New AutoCompleteStringCollection()
    Private _authTokenValue As String
    Private _autoClick As Boolean = True
    Private _doCancel As Boolean
    Private _initialHeight As Integer = 0
    Private _lastUrl As String
    Private _loginSourceAutomatic As FileToLoadOptions = FileToLoadOptions.NewUser
    Public Const CareLinkAuthTokenCookieName As String = "auth_tmp_token"
    Public Property LoggedOnUser As New CareLinkUserDataRecord(s_allUserSettingsData)
    Public Property Client As CareLinkClient

    Public Property LoginSourceAutomatic As FileToLoadOptions
        Get
            Return _loginSourceAutomatic
        End Get
        Set
            _loginSourceAutomatic = Value
        End Set
    End Property

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
        _doCancel = True
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
        CurrentDateCulture = If(TypeOf Me.CountryComboBox.SelectedValue Is String,
                                Me.CountryComboBox.SelectedValue.ToString.GetCurrentDateCulture,
                                CType(Me.CountryComboBox.SelectedValue, KeyValuePair(Of String, String)).Value.GetCurrentDateCulture
                               )
    End Sub

    Private Async Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.WebView21.Hide()
        If _initialHeight = 0 Then
            _initialHeight = Me.Height
        End If

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
            If s_userName = "" Then
                .Text = My.Settings.CareLinkUserName
            Else
                .SelectedIndex = -1
                .Text = s_userName
            End If
            Me.PasswordTextBox.Text = If(s_allUserSettingsData?.ContainsKey(.Text),
                                         s_allUserSettingsData(.Text).CareLinkPassword,
                                         ""
                                        )
        End With

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
        Await Me.WebView21.EnsureCoreWebView2Async()
        Await Me.WebView21.CoreWebView2.Profile.ClearBrowsingDataAsync()
        Me.DevContext = Await Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
    End Sub

    Private Sub LoginForm1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Height = _initialHeight
        Me.Visible = True
        If Me.LoginSourceAutomatic = FileToLoadOptions.Login Then
            Me.OK_Button_Click(Nothing, Nothing)
        End If
    End Sub

    Private Async Sub OK_Button_Click(sender As Object, e As EventArgs) Handles Ok_Button.Click
        If Me.UsernameComboBox.Text.Length = 0 Then
            Me.UsernameComboBox.Focus()
            Exit Sub
        End If
        If Me.PasswordTextBox.Text.Length = 0 Then
            Me.PasswordTextBox.Focus()
            Exit Sub
        End If
        s_userName = Me.UsernameComboBox.Text
        Me.Ok_Button.Enabled = False
        Me.WebView21.Visible = True
        Me.Height = CInt(Me.Height * 1.7)
        Me.WebView21.BringToFront()
        Application.DoEvents()
        _authTokenValue = ""
        _doCancel = False
        ' https://CareLink.MiniMed.com/patient/sso/login?country=us&lang=en
        Dim countryCode As String = Me.CountryComboBox.SelectedValue.ToString
        If countryCode.Length = 0 Then
            Stop
        End If
        Dim serverUrl As String = CareLinkClient.GetServerUrl(countryCode)
        If Me.WebView21 Is Nothing Then
            Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2
            Await Me.WebView21.EnsureCoreWebView2Async()
            Me.DevContext = Await Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
        End If
        If Me.WebView21.CoreWebView2 Is Nothing Then
            Await Me.WebView21.EnsureCoreWebView2Async()
            Me.DevContext = Await Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
        End If
        Me.WebView21.CoreWebView2.Navigate($"https://{serverUrl}/patient/sso/login?country={countryCode}&lang=en")
        Dim savePatientID As String = My.Settings.CareLinkPatientUserID
        My.Settings.CareLinkPatientUserID = Me.PatientUserIDTextBox.Text
        While _authTokenValue.Length = 0
            If _doCancel Then
                Exit Sub
            End If
            Thread.Sleep(100)
            Application.DoEvents()
        End While

        Dim recentData As Dictionary(Of String, String) = Me.Client.GetRecentData()
        If recentData?.Count > 0 Then
            s_lastMedicalDeviceDataUpdateServerEpoch = 0
            ReportLoginStatus(Me.LoginStatus, False)

            Me.Ok_Button.Enabled = True
            Me.Cancel_Button.Enabled = True
            My.Settings.CountryCode = countryCode
            My.Settings.CareLinkUserName = s_userName
            My.Settings.CareLinkPassword = Me.PasswordTextBox.Text
            My.Settings.CareLinkPatientUserID = Me.PatientUserIDTextBox.Text
            My.Settings.CareLinkPartner = Me.CarePartnerCheckBox.Checked OrElse Not String.IsNullOrWhiteSpace(Me.PatientUserIDTextBox.Text)
            My.Settings.Save()
            If Not s_allUserSettingsData.TryGetValue(s_userName, Me.LoggedOnUser) Then
                s_allUserSettingsData.SaveAllUserRecords(New CareLinkUserDataRecord(s_allUserSettingsData), NameOf(CareLinkUserDataRecord.CareLinkUserName), s_userName)
            End If
            Me.DialogResult = DialogResult.OK
            Me.Hide()
        Else
            ReportLoginStatus(Me.LoginStatus, True, Me.Client.GetLastErrorMessage)
            If Me.Client.GetLastErrorMessage = "Invalid username or password" Then
                Me.PasswordTextBox.Text = ""
                Dim userRecord As CareLinkUserDataRecord = Nothing
                If s_allUserSettingsData.TryGetValue(s_userName, userRecord) Then
                    s_allUserSettingsData.Remove(userRecord)
                End If
            End If

            Dim networkDownMessage As String = If(NetworkUnavailable(), "due to network being unavailable", $"'{Me.Client.GetLastErrorMessage}'")
            Select Case MsgBox($"Login Unsuccessful, try again?{vbCrLf}Abort, will exit program!", networkDownMessage, MsgBoxStyle.AbortRetryIgnore Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question, "Login Failed")
                Case MsgBoxResult.Abort
                    End
                Case MsgBoxResult.Ignore
                    Me.DialogResult = DialogResult.Ignore
                Case MsgBoxResult.Retry
                    Me.DialogResult = DialogResult.Retry
            End Select
        End If
        Me.Cancel_Button.Enabled = True
    End Sub

    Private Sub PasswordTextBox_Validating(sender As Object, e As CancelEventArgs) Handles PasswordTextBox.Validating
        If String.IsNullOrWhiteSpace(Me.PasswordTextBox.Text) Then
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
        Me.PasswordTextBox.PasswordChar = If(Me.ShowPasswordCheckBox.Checked,
                                             Nothing,
                                             "*"c
                                            )
    End Sub

    Private Sub UsernameComboBox_Leave(sender As Object, e As EventArgs) Handles UsernameComboBox.Leave
        Try

            Dim userSettings As CareLinkUserDataRecord = Nothing
            If s_allUserSettingsData.TryGetValue(Me.UsernameComboBox.Text, userSettings) Then
                If userSettings.CareLinkUserName.Equals(Me.UsernameComboBox.Text, StringComparison.OrdinalIgnoreCase) Then
                    Me.UsernameComboBox.Text = userSettings.CareLinkUserName
                End If
                s_userName = Me.UsernameComboBox.Text
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
        Else
            If Me.PasswordTextBox.Text.Length > 0 Then
                Me.Ok_Button.Enabled = True
            Else
                Me.PasswordTextBox.Focus()
            End If
        End If
    End Sub

    Private Async Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        Dim foundAuthToken As Boolean = False
        Dim cookies As New CookieContainer()

        If Not e.IsSuccess Then
            If Me.DevContext Is Nothing Then
                Me.DevContext = Await Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
            End If
            Dim errorBodyElement As HtmlBodyElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlBodyElement)(".generic-error")
            If errorBodyElement IsNot Nothing Then
                Dim t As Task = Task.Run(Async Function()
                                             Await Task.Delay(2000)
                                             Me.Invoke(Sub()
                                                           _doCancel = True
                                                           Me.OK_Button_Click(Nothing, Nothing)
                                                       End Sub
                                             )
                                         End Function
                    )
            End If
            Exit Sub
        End If
        Dim cookieList As List(Of CoreWebView2Cookie) = Await Me.WebView21.CoreWebView2.CookieManager.GetCookiesAsync(_lastUrl)
        For i As Integer = 0 To cookieList.Count - 1
            Dim cookie As CoreWebView2Cookie = Me.WebView21.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList(i).ToSystemNetCookie())

            If cookie.Name.StartsWith("auth") Then
                foundAuthToken = True
                _authTokenValue = cookie.Value
                Me.Visible = False
                _autoClick = True
            End If
            cookies.Add(New Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain))
        Next i
        Debug.Print(_lastUrl)
        If _ignoredURLs.Contains(_lastUrl) Then
            Exit Sub
        End If

        If foundAuthToken Then
            Me.Client = New CareLinkClient(cookies, s_userName, Me.PasswordTextBox.Text, Me.CountryComboBox.SelectedValue.ToString)
            Exit Sub
        End If
        If Me.WebView21.Source.ToString.StartsWith("https://mdtlogin.medtronic.com/mmcl/auth/oauth/v2/authorize/login") OrElse Me.WebView21.Source.ToString.StartsWith("https://mdtlogin-ocl.medtronic.com/mmcl/auth/oauth/v2/authorize/login") Then
            Dim t As Task(Of WebView2DevToolsContext) = Nothing
            If Me.DevContext Is Nothing Then
                t = Me.WebView21.CoreWebView2.CreateDevToolsContextAsync()
            End If
            Await Task.Delay(2000)
            If t IsNot Nothing Then
                Me.DevContext = t.Result
            End If
            Dim userNameInputElement As HtmlInputElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInputElement)("#username")
            Dim passwordInputElement As HtmlInputElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInputElement)("#password")
            Dim loginButtonElement As HtmlInputElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInputElement)("[name=""actionButton""]")
            If userNameInputElement Is Nothing OrElse passwordInputElement Is Nothing OrElse loginButtonElement Is Nothing Then
                Exit Sub
            End If
            Await userNameInputElement.SetValueAsync(s_userName)
            Await passwordInputElement.SetValueAsync(Me.PasswordTextBox.Text)

            Dim captchaFrame As HtmlInlineFrameElement = Await Me.DevContext.QuerySelectorAsync(Of HtmlInlineFrameElement)("[title=""reCAPTCHA""]")
            Await captchaFrame.PressAsync("Enter")
            Await Task.Delay(2000)

            Dim isCaptchaOpen As Boolean = True

            While isCaptchaOpen
                If _doCancel Then Exit Sub
                Await Task.Delay(250)
                Dim captchaPopupElement() As Element = Await Me.DevContext.XPathAsync("/html/body/div[2]")
                If captchaPopupElement?.Length > 0 Then
                    Dim captchaPopupStyleAttributes As String = Await captchaPopupElement(0).GetAttributeAsync("style")
                    captchaPopupStyleAttributes = captchaPopupStyleAttributes.ToLower.Replace(" ", "")
                    isCaptchaOpen = captchaPopupStyleAttributes.Contains("visibility:visible")
                Else
                    Exit Sub
                End If
            End While
            Await loginButtonElement.ClickElementAsync
            Exit Sub
        End If
        Debug.Print($" Me.WebView21.Source.ToString={ Me.WebView21.Source}")
        If Me.WebView21.Source.ToString.Contains("CareLink.MiniMed.", StringComparison.InvariantCultureIgnoreCase) Then
            Exit Sub
        End If
        Stop
    End Sub

    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs) Handles WebView21.NavigationStarting
        _lastUrl = e.Uri
        DebugPrintUrl($"In {NameOf(WebView21_NavigationStarting)} URL", e.Uri, 100)
    End Sub

End Class
