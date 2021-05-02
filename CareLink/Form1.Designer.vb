Imports Microsoft.Web.WebView2.WinForms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AddressBar = New System.Windows.Forms.TextBox()
        Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.StartDisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CurrentBGToolStripTextBox = New System.Windows.Forms.ToolStripTextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.FindWhat = New System.Windows.Forms.ToolStripTextBox()
        Me.FindNext = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.WebView21,System.ComponentModel.ISupportInitialize).BeginInit
        Me.MenuStrip1.SuspendLayout
        Me.SuspendLayout
        '
        'AddressBar
        '
        Me.AddressBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.AddressBar.Location = New System.Drawing.Point(0, 27)
        Me.AddressBar.Name = "AddressBar"
        Me.AddressBar.Size = New System.Drawing.Size(1416, 23)
        Me.AddressBar.TabIndex = 1
        '
        'WebView21
        '
        Me.WebView21.CreationProperties = Nothing
        Me.WebView21.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView21.Location = New System.Drawing.Point(0, 53)
        Me.WebView21.Name = "WebView21"
        Me.WebView21.Size = New System.Drawing.Size(800, 454)
        Me.WebView21.Source = New System.Uri("https://carelink.minimed.com/", System.UriKind.Absolute)
        Me.WebView21.TabIndex = 0
        Me.WebView21.ZoomFactor = 1R
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartDisplayToolStripMenuItem, Me.HelpToolStripMenuItem, Me.CurrentBGToolStripTextBox, Me.FindNext, Me.FindWhat})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1416, 27)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'StartDisplayToolStripMenuItem
        '
        Me.StartDisplayToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.StartDisplayToolStripMenuItem.Name = "StartDisplayToolStripMenuItem"
        Me.StartDisplayToolStripMenuItem.Size = New System.Drawing.Size(57, 23)
        Me.StartDisplayToolStripMenuItem.Text = "Display"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(93, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 23)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'CurrentBGToolStripTextBox
        '
        Me.CurrentBGToolStripTextBox.Name = "CurrentBGToolStripTextBox"
        Me.CurrentBGToolStripTextBox.Size = New System.Drawing.Size(100, 23)
        '
        'Timer1
        '
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(807, 57)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(609, 425)
        Me.RichTextBox1.TabIndex = 3
        Me.RichTextBox1.Text = ""
        Me.RichTextBox1.WordWrap = false
        '
        'FindWhat
        '
        Me.FindWhat.Name = "FindWhat"
        Me.FindWhat.Size = New System.Drawing.Size(100, 23)
        '
        'FindNext
        '
        Me.FindNext.Name = "FindNext"
        Me.FindNext.Size = New System.Drawing.Size(67, 23)
        Me.FindNext.Text = "FindNext"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1416, 519)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.AddressBar)
        Me.Controls.Add(Me.WebView21)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.WebView21,System.ComponentModel.ISupportInitialize).EndInit
        Me.MenuStrip1.ResumeLayout(false)
        Me.MenuStrip1.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents AddressBar As TextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents StarDisplayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents StartDisplayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CurrentBGToolStripTextBox As ToolStripTextBox
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents FindNext As ToolStripMenuItem
    Friend WithEvents FindWhat As ToolStripTextBox
End Class
