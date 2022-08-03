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
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoginForm1))
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.UsernameLabel = New System.Windows.Forms.Label()
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.UsernameTextBox = New System.Windows.Forms.TextBox()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.OK = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.SaveCredentials = New System.Windows.Forms.CheckBox()
        Me.CountryComboBox = New System.Windows.Forms.ComboBox()
        Me.RegionComboBox = New System.Windows.Forms.ComboBox()
        Me.SelectRegionLabel = New System.Windows.Forms.Label()
        Me.SelectCountryLabel = New System.Windows.Forms.Label()
        Me.ShowPasswordCheckBox = New System.Windows.Forms.CheckBox()
        Me.LoginStatus = New System.Windows.Forms.TextBox()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(12, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), System.Drawing.Image)
        Me.LogoPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(195, 224)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        '
        'UsernameLabel
        '
        Me.UsernameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.UsernameLabel.Location = New System.Drawing.Point(201, 6)
        Me.UsernameLabel.Name = "UsernameLabel"
        Me.UsernameLabel.Size = New System.Drawing.Size(220, 23)
        Me.UsernameLabel.TabIndex = 2
        Me.UsernameLabel.Text = "&User name"
        Me.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PasswordLabel
        '
        Me.PasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.PasswordLabel.Location = New System.Drawing.Point(201, 57)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(220, 23)
        Me.PasswordLabel.TabIndex = 4
        Me.PasswordLabel.Text = "&Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UsernameTextBox
        '
        Me.UsernameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.UsernameTextBox.Location = New System.Drawing.Point(201, 30)
        Me.UsernameTextBox.Name = "UsernameTextBox"
        Me.UsernameTextBox.Size = New System.Drawing.Size(220, 23)
        Me.UsernameTextBox.TabIndex = 3
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.PasswordTextBox.Location = New System.Drawing.Point(201, 79)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(159, 23)
        Me.PasswordTextBox.TabIndex = 5
        '
        'OK
        '
        Me.OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK.Location = New System.Drawing.Point(235, 277)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(94, 23)
        Me.OK.TabIndex = 0
        Me.OK.Text = "&OK"
        '
        'Cancel
        '
        Me.Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(338, 277)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(94, 23)
        Me.Cancel.TabIndex = 1
        Me.Cancel.Text = "&Cancel"
        '
        'SaveCredentials
        '
        Me.SaveCredentials.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.SaveCredentials.AutoSize = True
        Me.SaveCredentials.Location = New System.Drawing.Point(204, 207)
        Me.SaveCredentials.Name = "SaveCredentials"
        Me.SaveCredentials.Size = New System.Drawing.Size(187, 19)
        Me.SaveCredentials.TabIndex = 11
        Me.SaveCredentials.Text = "Save User Name and Password"
        Me.SaveCredentials.UseVisualStyleBackColor = True
        '
        'CountryComboBox
        '
        Me.CountryComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.CountryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CountryComboBox.Enabled = False
        Me.CountryComboBox.FormattingEnabled = True
        Me.CountryComboBox.Location = New System.Drawing.Point(201, 177)
        Me.CountryComboBox.Name = "CountryComboBox"
        Me.CountryComboBox.Size = New System.Drawing.Size(220, 23)
        Me.CountryComboBox.TabIndex = 10
        '
        'RegionComboBox
        '
        Me.RegionComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.RegionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RegionComboBox.FormattingEnabled = True
        Me.RegionComboBox.Location = New System.Drawing.Point(201, 128)
        Me.RegionComboBox.Name = "RegionComboBox"
        Me.RegionComboBox.Size = New System.Drawing.Size(220, 23)
        Me.RegionComboBox.TabIndex = 8
        '
        'SelectRegionLabel
        '
        Me.SelectRegionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.SelectRegionLabel.AutoSize = True
        Me.SelectRegionLabel.Location = New System.Drawing.Point(201, 110)
        Me.SelectRegionLabel.Name = "SelectRegionLabel"
        Me.SelectRegionLabel.Size = New System.Drawing.Size(78, 15)
        Me.SelectRegionLabel.TabIndex = 7
        Me.SelectRegionLabel.Text = "Select Region"
        '
        'SelectCountryLabel
        '
        Me.SelectCountryLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.SelectCountryLabel.AutoSize = True
        Me.SelectCountryLabel.Location = New System.Drawing.Point(201, 160)
        Me.SelectCountryLabel.Name = "SelectCountryLabel"
        Me.SelectCountryLabel.Size = New System.Drawing.Size(84, 15)
        Me.SelectCountryLabel.TabIndex = 9
        Me.SelectCountryLabel.Text = "Select Country"
        '
        'ShowPasswordCheckBox
        '
        Me.ShowPasswordCheckBox.AutoSize = True
        Me.ShowPasswordCheckBox.Location = New System.Drawing.Point(366, 81)
        Me.ShowPasswordCheckBox.Name = "ShowPasswordCheckBox"
        Me.ShowPasswordCheckBox.Size = New System.Drawing.Size(55, 19)
        Me.ShowPasswordCheckBox.TabIndex = 6
        Me.ShowPasswordCheckBox.Text = "Show"
        Me.ShowPasswordCheckBox.UseVisualStyleBackColor = True
        '
        'LoginStatus
        '
        Me.LoginStatus.BackColor = System.Drawing.SystemColors.Control
        Me.LoginStatus.Location = New System.Drawing.Point(4, 230)
        Me.LoginStatus.Multiline = True
        Me.LoginStatus.Name = "LoginStatus"
        Me.LoginStatus.Size = New System.Drawing.Size(191, 80)
        Me.LoginStatus.TabIndex = 12
        Me.LoginStatus.Text = "Login Status: Unknown"
        '
        'LoginForm1
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(442, 313)
        Me.Controls.Add(Me.LoginStatus)
        Me.Controls.Add(Me.ShowPasswordCheckBox)
        Me.Controls.Add(Me.SelectCountryLabel)
        Me.Controls.Add(Me.SelectRegionLabel)
        Me.Controls.Add(Me.RegionComboBox)
        Me.Controls.Add(Me.CountryComboBox)
        Me.Controls.Add(Me.SaveCredentials)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.UsernameTextBox)
        Me.Controls.Add(Me.PasswordLabel)
        Me.Controls.Add(Me.UsernameLabel)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login"
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SaveCredentials As CheckBox
    Friend WithEvents CountryComboBox As ComboBox
    Friend WithEvents RegionComboBox As ComboBox
    Friend WithEvents SelectRegionLabel As Label
    Friend WithEvents SelectCountryLabel As Label
    Friend WithEvents ShowPasswordCheckBox As CheckBox
    Friend WithEvents LoginStatus As TextBox
    Friend WithEvents SelectCultureLabel As Label
    Friend WithEvents CultureComboBox As ComboBox
End Class
