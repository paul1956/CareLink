' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PumpSetup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PumpSetup))
        FolderBrowserDialog1 = New FolderBrowserDialog()
        Exit_Button = New Button()
        InstructionsRtb = New RichTextBox()
        ctxMenu = New ContextMenuStrip(components)
        copyUrlMenuItem = New ToolStripMenuItem()
        UserName = New TextBox()
        Accept_Button = New Button()
        ComboBoxPDFs = New ComboBox()
        ctxMenu.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' Exit_Button
        ' 
        Exit_Button.Anchor = AnchorStyles.None
        Exit_Button.Location = New Point(252, 198)
        Exit_Button.Margin = New Padding(4, 3, 4, 3)
        Exit_Button.Name = "Exit_Button"
        Exit_Button.Size = New Size(119, 27)
        Exit_Button.TabIndex = 5
        Exit_Button.Text = "Exit"
        ' 
        ' InstructionsRtb
        ' 
        InstructionsRtb.ContextMenuStrip = ctxMenu
        InstructionsRtb.Dock = DockStyle.Top
        InstructionsRtb.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        InstructionsRtb.Location = New Point(0, 0)
        InstructionsRtb.Name = "InstructionsRtb"
        InstructionsRtb.Size = New Size(622, 156)
        InstructionsRtb.TabIndex = 0
        InstructionsRtb.Text = ""
        ' 
        ' ctxMenu
        ' 
        ctxMenu.Items.AddRange(New ToolStripItem() {copyUrlMenuItem})
        ctxMenu.Name = "ctxMenu"
        ctxMenu.Size = New Size(127, 26)
        ' 
        ' copyUrlMenuItem
        ' 
        copyUrlMenuItem.Name = "copyUrlMenuItem"
        copyUrlMenuItem.Size = New Size(126, 22)
        copyUrlMenuItem.Text = "Copy URL"
        ' 
        ' UserName
        ' 
        UserName.Location = New Point(273, 164)
        UserName.Name = "UserName"
        UserName.PlaceholderText = "CareLink™ UserName"
        UserName.Size = New Size(240, 23)
        UserName.TabIndex = 1
        ' 
        ' Accept_Button
        ' 
        Accept_Button.Enabled = False
        Accept_Button.Location = New Point(533, 164)
        Accept_Button.Name = "Accept_Button"
        Accept_Button.Size = New Size(75, 23)
        Accept_Button.TabIndex = 2
        Accept_Button.Text = "Accept"
        Accept_Button.UseVisualStyleBackColor = True
        ' 
        ' ComboBoxPDFs
        ' 
        ComboBoxPDFs.FormattingEnabled = True
        ComboBoxPDFs.Location = New Point(17, 161)
        ComboBoxPDFs.Name = "ComboBoxPDFs"
        ComboBoxPDFs.Size = New Size(240, 23)
        ComboBoxPDFs.TabIndex = 6
        ' 
        ' PumpSetup
        ' 
        Me.AutoScaleDimensions = New SizeF(96.0F, 96.0F)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.ClientSize = New Size(622, 237)
        Me.Controls.Add(ComboBoxPDFs)
        Me.Controls.Add(Accept_Button)
        Me.Controls.Add(UserName)
        Me.Controls.Add(InstructionsRtb)
        Me.Controls.Add(Exit_Button)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Me.MaximizeBox = False
        Me.Name = "PumpSetup"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Pump Setup Instructions"
        ctxMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents Exit_Button As Button
    Friend WithEvents InstructionsRtb As RichTextBox
    Friend WithEvents ctxMenu As ContextMenuStrip
    Friend WithEvents copyUrlMenuItem As ToolStripMenuItem
    Friend WithEvents UserName As TextBox
    Friend WithEvents Accept_Button As Button
    Friend WithEvents ComboBoxPDFs As ComboBox

End Class
