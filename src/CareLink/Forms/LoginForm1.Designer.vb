<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class LoginForm1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Friend WithEvents LogoPictureBox As PictureBox
    Friend WithEvents UsernameLabel As Label
    Friend WithEvents PasswordLabel As Label
    Friend WithEvents UsernameTextBox As TextBox
    Friend WithEvents PasswordTextBox As TextBox
    Friend WithEvents Ok_Button As Button
    Friend WithEvents Cancel_Button As Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(LoginForm1))
        LogoPictureBox = New PictureBox()
        UsernameLabel = New Label()
        PasswordLabel = New Label()
        UsernameTextBox = New TextBox()
        PasswordTextBox = New TextBox()
        Ok_Button = New Button()
        Cancel_Button = New Button()
        CountryComboBox = New ComboBox()
        RegionComboBox = New ComboBox()
        SelectRegionLabel = New Label()
        SelectCountryLabel = New Label()
        ShowPasswordCheckBox = New CheckBox()
        LoginStatus = New TextBox()
        Panel1 = New Panel()
        PatientUserIDLabel = New Label()
        PatientUserIDTextBox = New TextBox()
        CarePartnerCheckBox = New CheckBox()
        CType(LogoPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' LogoPictureBox
        ' 
        LogoPictureBox.BackColor = Color.FromArgb(CByte(12), CByte(36), CByte(82))
        LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), Image)
        LogoPictureBox.Location = New Point(0, 0)
        LogoPictureBox.Name = "LogoPictureBox"
        LogoPictureBox.Size = New Size(195, 224)
        LogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom
        LogoPictureBox.TabIndex = 0
        LogoPictureBox.TabStop = False
        ' 
        ' UsernameLabel
        ' 
        UsernameLabel.Anchor = AnchorStyles.Left
        UsernameLabel.Location = New Point(201, 24)
        UsernameLabel.Name = "UsernameLabel"
        UsernameLabel.Size = New Size(220, 23)
        UsernameLabel.TabIndex = 2
        UsernameLabel.Text = "User Name"
        UsernameLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' PasswordLabel
        ' 
        PasswordLabel.Anchor = AnchorStyles.Left
        PasswordLabel.Location = New Point(201, 75)
        PasswordLabel.Name = "PasswordLabel"
        PasswordLabel.Size = New Size(220, 23)
        PasswordLabel.TabIndex = 4
        PasswordLabel.Text = "Password"
        PasswordLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' UsernameTextBox
        ' 
        UsernameTextBox.Anchor = AnchorStyles.Left
        UsernameTextBox.AutoCompleteMode = AutoCompleteMode.Suggest
        UsernameTextBox.AutoCompleteSource = AutoCompleteSource.RecentlyUsedList
        UsernameTextBox.Location = New Point(201, 48)
        UsernameTextBox.Name = "UsernameTextBox"
        UsernameTextBox.Size = New Size(220, 23)
        UsernameTextBox.TabIndex = 3
        ' 
        ' PasswordTextBox
        ' 
        PasswordTextBox.Anchor = AnchorStyles.Left
        PasswordTextBox.Location = New Point(201, 97)
        PasswordTextBox.Name = "PasswordTextBox"
        PasswordTextBox.PasswordChar = "*"c
        PasswordTextBox.Size = New Size(159, 23)
        PasswordTextBox.TabIndex = 5
        ' 
        ' Ok_Button
        ' 
        Ok_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Ok_Button.DialogResult = DialogResult.OK
        Ok_Button.Location = New Point(235, 312)
        Ok_Button.Name = "Ok_Button"
        Ok_Button.Size = New Size(94, 23)
        Ok_Button.TabIndex = 0
        Ok_Button.Text = "&OK"
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Cancel_Button.DialogResult = DialogResult.Cancel
        Cancel_Button.Location = New Point(338, 312)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(94, 23)
        Cancel_Button.TabIndex = 1
        Cancel_Button.Text = "&Cancel"
        ' 
        ' CountryComboBox
        ' 
        CountryComboBox.Anchor = AnchorStyles.Left
        CountryComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        CountryComboBox.Enabled = False
        CountryComboBox.FormattingEnabled = True
        CountryComboBox.Location = New Point(201, 195)
        CountryComboBox.Name = "CountryComboBox"
        CountryComboBox.Size = New Size(220, 23)
        CountryComboBox.TabIndex = 10
        ' 
        ' RegionComboBox
        ' 
        RegionComboBox.Anchor = AnchorStyles.Left
        RegionComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        RegionComboBox.FormattingEnabled = True
        RegionComboBox.Location = New Point(201, 146)
        RegionComboBox.Name = "RegionComboBox"
        RegionComboBox.Size = New Size(220, 23)
        RegionComboBox.TabIndex = 8
        ' 
        ' SelectRegionLabel
        ' 
        SelectRegionLabel.Anchor = AnchorStyles.Left
        SelectRegionLabel.AutoSize = True
        SelectRegionLabel.Location = New Point(201, 128)
        SelectRegionLabel.Name = "SelectRegionLabel"
        SelectRegionLabel.Size = New Size(78, 15)
        SelectRegionLabel.TabIndex = 7
        SelectRegionLabel.Text = "Select Region"
        ' 
        ' SelectCountryLabel
        ' 
        SelectCountryLabel.Anchor = AnchorStyles.Left
        SelectCountryLabel.AutoSize = True
        SelectCountryLabel.Location = New Point(201, 178)
        SelectCountryLabel.Name = "SelectCountryLabel"
        SelectCountryLabel.Size = New Size(84, 15)
        SelectCountryLabel.TabIndex = 9
        SelectCountryLabel.Text = "Select Country"
        ' 
        ' ShowPasswordCheckBox
        ' 
        ShowPasswordCheckBox.AutoSize = True
        ShowPasswordCheckBox.Location = New Point(366, 81)
        ShowPasswordCheckBox.Name = "ShowPasswordCheckBox"
        ShowPasswordCheckBox.Size = New Size(55, 19)
        ShowPasswordCheckBox.TabIndex = 6
        ShowPasswordCheckBox.Text = "Show"
        ShowPasswordCheckBox.UseVisualStyleBackColor = True
        ' 
        ' LoginStatus
        ' 
        LoginStatus.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        LoginStatus.BackColor = SystemColors.Control
        LoginStatus.Location = New Point(4, 230)
        LoginStatus.Multiline = True
        LoginStatus.Name = "LoginStatus"
        LoginStatus.ScrollBars = ScrollBars.Vertical
        LoginStatus.Size = New Size(225, 115)
        LoginStatus.TabIndex = 12
        LoginStatus.Text = "Login Status: Unknown"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(PatientUserIDLabel)
        Panel1.Controls.Add(PatientUserIDTextBox)
        Panel1.Controls.Add(CarePartnerCheckBox)
        Panel1.Location = New Point(235, 230)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(197, 76)
        Panel1.TabIndex = 13
        ' 
        ' PatientUserIDLabel
        ' 
        PatientUserIDLabel.AutoSize = True
        PatientUserIDLabel.Location = New Point(4, 31)
        PatientUserIDLabel.Name = "PatientUserIDLabel"
        PatientUserIDLabel.Size = New Size(81, 15)
        PatientUserIDLabel.TabIndex = 18
        PatientUserIDLabel.Text = "Patient UserID"
        ' 
        ' PatientUserIDTextBox
        ' 
        PatientUserIDTextBox.Location = New Point(4, 47)
        PatientUserIDTextBox.Name = "PatientUserIDTextBox"
        PatientUserIDTextBox.Size = New Size(190, 23)
        PatientUserIDTextBox.TabIndex = 17
        ' 
        ' CarePartnerCheckBox
        ' 
        CarePartnerCheckBox.AutoSize = True
        CarePartnerCheckBox.Location = New Point(55, 6)
        CarePartnerCheckBox.Name = "CarePartnerCheckBox"
        CarePartnerCheckBox.Size = New Size(88, 19)
        CarePartnerCheckBox.TabIndex = 16
        CarePartnerCheckBox.Text = "CarePartner"
        CarePartnerCheckBox.UseVisualStyleBackColor = True
        ' 
        ' LoginForm1
        ' 
        Me.AcceptButton = Ok_Button
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(442, 348)
        Me.Controls.Add(Panel1)
        Me.Controls.Add(LoginStatus)
        Me.Controls.Add(ShowPasswordCheckBox)
        Me.Controls.Add(SelectCountryLabel)
        Me.Controls.Add(SelectRegionLabel)
        Me.Controls.Add(RegionComboBox)
        Me.Controls.Add(CountryComboBox)
        Me.Controls.Add(Cancel_Button)
        Me.Controls.Add(Ok_Button)
        Me.Controls.Add(PasswordTextBox)
        Me.Controls.Add(UsernameTextBox)
        Me.Controls.Add(PasswordLabel)
        Me.Controls.Add(UsernameLabel)
        Me.Controls.Add(LogoPictureBox)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm1"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Login"
        CType(LogoPictureBox, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents CountryComboBox As ComboBox
    Friend WithEvents RegionComboBox As ComboBox
    Friend WithEvents SelectRegionLabel As Label
    Friend WithEvents SelectCountryLabel As Label
    Friend WithEvents ShowPasswordCheckBox As CheckBox
    Friend WithEvents LoginStatus As TextBox
    Friend WithEvents SelectCultureLabel As Label
    Friend WithEvents CultureComboBox As ComboBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PatientUserIDLabel As Label
    Friend WithEvents PatientUserIDTextBox As TextBox
    Friend WithEvents CarePartnerCheckBox As CheckBox
End Class
