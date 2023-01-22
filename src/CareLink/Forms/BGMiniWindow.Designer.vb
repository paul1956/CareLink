' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BGMiniWindow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <DebuggerNonUserCode()>
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
        Me.BGTextBox = New TextBox()
        Me.ActiveInsulinTextBox = New TextBox()
        Me.HiddenTextBox = New TextBox()
        Me.DeltaTextBox = New TextBox()
        Me.CloseButton = New Button()
        Me.chkTopMost = New CheckBox()
        Me.SuspendLayout()
        ' 
        ' BGTextBox
        ' 
        Me.BGTextBox.BorderStyle = BorderStyle.None
        Me.BGTextBox.Font = New Font("Segoe UI", 48F, FontStyle.Regular, GraphicsUnit.Point)
        Me.BGTextBox.Location = New Point(4, 29)
        Me.BGTextBox.Margin = New Padding(0)
        Me.BGTextBox.Name = "BGTextBox"
        Me.BGTextBox.Size = New Size(160, 86)
        Me.BGTextBox.TabIndex = 1
        Me.BGTextBox.Text = "999"
        Me.BGTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' ActiveInsulinTextBox
        ' 
        Me.ActiveInsulinTextBox.BorderStyle = BorderStyle.None
        Me.ActiveInsulinTextBox.Dock = DockStyle.Top
        Me.ActiveInsulinTextBox.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point)
        Me.ActiveInsulinTextBox.Location = New Point(0, 0)
        Me.ActiveInsulinTextBox.Name = "ActiveInsulinTextBox"
        Me.ActiveInsulinTextBox.Size = New Size(264, 26)
        Me.ActiveInsulinTextBox.TabIndex = 0
        Me.ActiveInsulinTextBox.Text = "Active Insulin: ???"
        Me.ActiveInsulinTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' HiddenTextBox
        ' 
        Me.HiddenTextBox.Location = New Point(0, -1)
        Me.HiddenTextBox.Name = "HiddenTextBox"
        Me.HiddenTextBox.Size = New Size(100, 23)
        Me.HiddenTextBox.TabIndex = 2
        ' 
        ' DeltaTextBox
        ' 
        Me.DeltaTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Me.DeltaTextBox.BorderStyle = BorderStyle.None
        Me.DeltaTextBox.Font = New Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point)
        Me.DeltaTextBox.Location = New Point(143, 40)
        Me.DeltaTextBox.Margin = New Padding(0)
        Me.DeltaTextBox.Name = "DeltaTextBox"
        Me.DeltaTextBox.Size = New Size(121, 64)
        Me.DeltaTextBox.TabIndex = 2
        Me.DeltaTextBox.Text = "+10"' 
        ' CloseButton
        ' 
        Me.CloseButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Me.CloseButton.Location = New Point(82, 99)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New Size(182, 23)
        Me.CloseButton.TabIndex = 3
        Me.CloseButton.Text = "Show Main Display ALT+W"
        Me.CloseButton.UseVisualStyleBackColor = True
        ' 
        ' chkTopMost
        ' 
        Me.chkTopMost.AutoSize = True
        Me.chkTopMost.Checked = True
        Me.chkTopMost.CheckState = CheckState.Checked
        Me.chkTopMost.Location = New Point(4, 103)
        Me.chkTopMost.Name = "chkTopMost"
        Me.chkTopMost.Size = New Size(72, 19)
        Me.chkTopMost.TabIndex = 4
        Me.chkTopMost.Text = "Topmost"
        Me.chkTopMost.UseVisualStyleBackColor = True
        ' 
        ' BGMiniWindow
        ' 
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = SystemColors.Window
        Me.ClientSize = New Size(264, 123)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkTopMost)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.DeltaTextBox)
        Me.Controls.Add(Me.ActiveInsulinTextBox)
        Me.Controls.Add(Me.BGTextBox)
        Me.Controls.Add(Me.HiddenTextBox)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BGMiniWindow"
        Me.ShowInTaskbar = False
        Me.Text = "Current Glucose Value"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents BGTextBox As TextBox
    Friend WithEvents ActiveInsulinTextBox As TextBox
    Friend WithEvents HiddenTextBox As TextBox
    Friend WithEvents DeltaTextBox As TextBox
    Friend WithEvents CloseButton As Button
    Friend WithEvents chkTopMost As CheckBox
End Class
