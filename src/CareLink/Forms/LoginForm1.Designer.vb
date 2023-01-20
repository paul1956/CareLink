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
    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents UsernameLabel As System.Windows.Forms.Label
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents UsernameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PasswordTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Ok_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(LoginForm1))
        Me.LogoPictureBox = New PictureBox()
        Me.UsernameLabel = New Label()
        Me.PasswordLabel = New Label()
        Me.UsernameTextBox = New TextBox()
        Me.PasswordTextBox = New TextBox()
        Me.Ok_Button = New Button()
        Me.Cancel_Button = New Button()
        Me.CountryComboBox = New ComboBox()
        Me.RegionComboBox = New ComboBox()
        Me.SelectRegionLabel = New Label()
        Me.SelectCountryLabel = New Label()
        Me.ShowPasswordCheckBox = New CheckBox()
        Me.LoginStatus = New TextBox()
        CType(Me.LogoPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' LogoPictureBox
        ' 
        Me.LogoPictureBox.BackColor = Color.FromArgb(CByte(12), CByte(36), CByte(82))
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), Image)
        Me.LogoPictureBox.Location = New Point(0, 0)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New Size(195, 224)
        Me.LogoPictureBox.SizeMode = PictureBoxSizeMode.Zoom
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        ' 
        ' UsernameLabel
        ' 
        Me.UsernameLabel.Anchor = AnchorStyles.Left
        Me.UsernameLabel.Location = New Point(201, 6)
        Me.UsernameLabel.Name = "UsernameLabel"
        Me.UsernameLabel.Size = New Size(220, 23)
        Me.UsernameLabel.TabIndex = 2
        Me.UsernameLabel.Text = "&User name"
        Me.UsernameLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' PasswordLabel
        ' 
        Me.PasswordLabel.Anchor = AnchorStyles.Left
        Me.PasswordLabel.Location = New Point(201, 57)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New Size(220, 23)
        Me.PasswordLabel.TabIndex = 4
        Me.PasswordLabel.Text = "&Password"
        Me.PasswordLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' UsernameTextBox
        ' 
        Me.UsernameTextBox.Anchor = AnchorStyles.Left
        Me.UsernameTextBox.AutoCompleteMode = AutoCompleteMode.Suggest
        Me.UsernameTextBox.AutoCompleteSource = AutoCompleteSource.RecentlyUsedList
        Me.UsernameTextBox.Location = New Point(201, 30)
        Me.UsernameTextBox.Name = "UsernameTextBox"
        Me.UsernameTextBox.Size = New Size(220, 23)
        Me.UsernameTextBox.TabIndex = 3
        ' 
        ' PasswordTextBox
        ' 
        Me.PasswordTextBox.Anchor = AnchorStyles.Left
        Me.PasswordTextBox.Location = New Point(201, 79)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = "*"c
        Me.PasswordTextBox.Size = New Size(159, 23)
        Me.PasswordTextBox.TabIndex = 5
        ' 
        ' Ok_Button
        ' 
        Me.Ok_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.Ok_Button.DialogResult = DialogResult.OK
        Me.Ok_Button.Location = New Point(235, 277)
        Me.Ok_Button.Name = "Ok_Button"
        Me.Ok_Button.Size = New Size(94, 23)
        Me.Ok_Button.TabIndex = 0
        Me.Ok_Button.Text = "&OK" ' 
        ' Cancel_Button
        ' 
        Me.Cancel_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.Cancel_Button.DialogResult = DialogResult.Cancel
        Me.Cancel_Button.Location = New Point(338, 277)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New Size(94, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "&Cancel" ' 
        ' CountryComboBox
        ' 
        Me.CountryComboBox.Anchor = AnchorStyles.Left
        Me.CountryComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CountryComboBox.Enabled = False
        Me.CountryComboBox.FormattingEnabled = True
        Me.CountryComboBox.Location = New Point(201, 177)
        Me.CountryComboBox.Name = "CountryComboBox"
        Me.CountryComboBox.Size = New Size(220, 23)
        Me.CountryComboBox.TabIndex = 10
        ' 
        ' RegionComboBox
        ' 
        Me.RegionComboBox.Anchor = AnchorStyles.Left
        Me.RegionComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        Me.RegionComboBox.FormattingEnabled = True
        Me.RegionComboBox.Location = New Point(201, 128)
        Me.RegionComboBox.Name = "RegionComboBox"
        Me.RegionComboBox.Size = New Size(220, 23)
        Me.RegionComboBox.TabIndex = 8
        ' 
        ' SelectRegionLabel
        ' 
        Me.SelectRegionLabel.Anchor = AnchorStyles.Left
        Me.SelectRegionLabel.AutoSize = True
        Me.SelectRegionLabel.Location = New Point(201, 110)
        Me.SelectRegionLabel.Name = "SelectRegionLabel"
        Me.SelectRegionLabel.Size = New Size(78, 15)
        Me.SelectRegionLabel.TabIndex = 7
        Me.SelectRegionLabel.Text = "Select Region" ' 
        ' SelectCountryLabel
        ' 
        Me.SelectCountryLabel.Anchor = AnchorStyles.Left
        Me.SelectCountryLabel.AutoSize = True
        Me.SelectCountryLabel.Location = New Point(201, 160)
        Me.SelectCountryLabel.Name = "SelectCountryLabel"
        Me.SelectCountryLabel.Size = New Size(84, 15)
        Me.SelectCountryLabel.TabIndex = 9
        Me.SelectCountryLabel.Text = "Select Country" ' 
        ' ShowPasswordCheckBox
        ' 
        Me.ShowPasswordCheckBox.AutoSize = True
        Me.ShowPasswordCheckBox.Location = New Point(366, 81)
        Me.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox"
        Me.ShowPasswordCheckBox.Size = New Size(55, 19)
        Me.ShowPasswordCheckBox.TabIndex = 6
        Me.ShowPasswordCheckBox.Text = "Show"
        Me.ShowPasswordCheckBox.UseVisualStyleBackColor = True
        ' 
        ' LoginStatus
        ' 
        Me.LoginStatus.BackColor = SystemColors.Control
        Me.LoginStatus.Location = New Point(4, 230)
        Me.LoginStatus.Multiline = True
        Me.LoginStatus.Name = "LoginStatus"
        Me.LoginStatus.Size = New Size(191, 80)
        Me.LoginStatus.TabIndex = 12
        Me.LoginStatus.Text = "Login Status: Unknown" ' 
        ' LoginForm1
        ' 
        Me.AcceptButton = Me.Ok_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New Size(442, 313)
        Me.Controls.Add(Me.LoginStatus)
        Me.Controls.Add(Me.ShowPasswordCheckBox)
        Me.Controls.Add(Me.SelectCountryLabel)
        Me.Controls.Add(Me.SelectRegionLabel)
        Me.Controls.Add(Me.RegionComboBox)
        Me.Controls.Add(Me.CountryComboBox)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.Ok_Button)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UsernameTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.Controls.Add(Me.UsernameLabel)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm1"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Login"
        CType(Me.LogoPictureBox, ComponentModel.ISupportInitialize).EndInit()
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
End Class
