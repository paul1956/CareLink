' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PumpSetupDialog
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        RtbMainLeft = New RichTextBox()
        RtbMainRight = New RichTextBox()
        MenuStrip1 = New MenuStrip()
        PrintToolStripMenuItem = New ToolStripMenuItem()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        PrintDocument1 = New Printing.PrintDocument()
        MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' RtbMainLeft
        ' 
        RtbMainLeft.BorderStyle = BorderStyle.None
        RtbMainLeft.Dock = DockStyle.Left
        RtbMainLeft.Location = New Point(0, 0)
        RtbMainLeft.Name = "RtbMainLeft"
        RtbMainLeft.Size = New Size(586, 901)
        RtbMainLeft.TabIndex = 0
        RtbMainLeft.Text = ""
        ' 
        ' RtbMainRight
        ' 
        RtbMainRight.BorderStyle = BorderStyle.None
        RtbMainRight.Dock = DockStyle.Right
        RtbMainRight.Location = New Point(541, 0)
        RtbMainRight.Name = "RtbMainRight"
        RtbMainRight.Size = New Size(681, 901)
        RtbMainRight.TabIndex = 1
        RtbMainRight.Text = ""
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {PrintToolStripMenuItem, ExitToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1222, 24)
        MenuStrip1.TabIndex = 2
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' PrintToolStripMenuItem
        ' 
        PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        PrintToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.P
        PrintToolStripMenuItem.Size = New Size(44, 20)
        PrintToolStripMenuItem.Text = "Print"
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.X
        ExitToolStripMenuItem.Size = New Size(37, 20)
        ExitToolStripMenuItem.Text = "Exit"
        ' 
        ' PumpSetupDialog
        ' 
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1222, 976)
        Me.Controls.Add(RtbMainRight)
        Me.Controls.Add(RtbMainLeft)
        Me.Controls.Add(MenuStrip1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MainMenuStrip = MenuStrip1
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PumpSetupDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Pump Setup Instructions"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents PrintDocument1 As Drawing.Printing.PrintDocument
    Friend WithEvents PrintToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RtbMainLeft As RichTextBox
    Friend WithEvents RtbMainRight As RichTextBox
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
End Class
