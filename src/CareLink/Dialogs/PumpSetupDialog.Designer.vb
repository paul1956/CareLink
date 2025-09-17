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
        SplitContainer1 = New SplitContainer()
        OK_Button = New Button()
        MenuStrip1 = New MenuStrip()
        PrintToolStripMenuItem = New ToolStripMenuItem()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
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
        RtbMainRight.Location = New Point(588, 0)
        RtbMainRight.Name = "RtbMainRight"
        RtbMainRight.Size = New Size(586, 901)
        RtbMainRight.TabIndex = 1
        RtbMainRight.Text = ""
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 24)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(RtbMainRight)
        SplitContainer1.Panel1.Controls.Add(RtbMainLeft)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.AutoScroll = True
        SplitContainer1.Panel2.Controls.Add(OK_Button)
        SplitContainer1.Size = New Size(1174, 952)
        SplitContainer1.SplitterDistance = 901
        SplitContainer1.TabIndex = 1
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Location = New Point(1081, 10)
        OK_Button.Margin = New Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(77, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {PrintToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1174, 24)
        MenuStrip1.TabIndex = 2
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' PrintToolStripMenuItem
        ' 
        PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        PrintToolStripMenuItem.Size = New Size(44, 20)
        PrintToolStripMenuItem.Text = "Print"
        ' 
        ' PumpSetupDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1174, 976)
        Me.Controls.Add(SplitContainer1)
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
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents OK_Button As Button
    Friend WithEvents RtbMainLeft As RichTextBox
    Friend WithEvents RtbMainRight As RichTextBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents PrintToolStripMenuItem As ToolStripMenuItem
End Class
