' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BGMiniWindow
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
        Me.BGTextBox = New System.Windows.Forms.TextBox()
        Me.ActiveInsulinTextBox = New System.Windows.Forms.TextBox()
        Me.HiddenTextBox = New System.Windows.Forms.TextBox()
        Me.DeltaTextBox = New System.Windows.Forms.TextBox()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.SuspendLayout
        '
        'BGTextBox
        '
        Me.BGTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.BGTextBox.Font = New System.Drawing.Font("Segoe UI", 48!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.BGTextBox.Location = New System.Drawing.Point(4, 29)
        Me.BGTextBox.Margin = New System.Windows.Forms.Padding(0)
        Me.BGTextBox.Name = "BGTextBox"
        Me.BGTextBox.Size = New System.Drawing.Size(129, 86)
        Me.BGTextBox.TabIndex = 1
        Me.BGTextBox.Text = "999"
        Me.BGTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ActiveInsulinTextBox
        '
        Me.ActiveInsulinTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ActiveInsulinTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.ActiveInsulinTextBox.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ActiveInsulinTextBox.Location = New System.Drawing.Point(0, 0)
        Me.ActiveInsulinTextBox.Name = "ActiveInsulinTextBox"
        Me.ActiveInsulinTextBox.Size = New System.Drawing.Size(230, 26)
        Me.ActiveInsulinTextBox.TabIndex = 0
        Me.ActiveInsulinTextBox.Text = "Active Insulin: ???"
        Me.ActiveInsulinTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'HiddenTextBox
        '
        Me.HiddenTextBox.Location = New System.Drawing.Point(0, -1)
        Me.HiddenTextBox.Name = "HiddenTextBox"
        Me.HiddenTextBox.Size = New System.Drawing.Size(100, 23)
        Me.HiddenTextBox.TabIndex = 2
        '
        'DeltaTextBox
        '
        Me.DeltaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DeltaTextBox.Font = New System.Drawing.Font("Segoe UI", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.DeltaTextBox.Location = New System.Drawing.Point(132, 40)
        Me.DeltaTextBox.Margin = New System.Windows.Forms.Padding(0)
        Me.DeltaTextBox.Name = "DeltaTextBox"
        Me.DeltaTextBox.Size = New System.Drawing.Size(98, 64)
        Me.DeltaTextBox.TabIndex = 2
        Me.DeltaTextBox.Text = "+10"
        '
        'CloseButton
        '
        Me.CloseButton.Location = New System.Drawing.Point(133, 99)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(97, 23)
        Me.CloseButton.TabIndex = 3
        Me.CloseButton.Text = "Hide"
        Me.CloseButton.UseVisualStyleBackColor = true
        '
        'BGMiniWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(230, 123)
        Me.ControlBox = false
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.DeltaTextBox)
        Me.Controls.Add(Me.ActiveInsulinTextBox)
        Me.Controls.Add(Me.BGTextBox)
        Me.Controls.Add(Me.HiddenTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "BGMiniWindow"
        Me.ShowInTaskbar = false
        Me.Text = "Current Glucose Value"
        Me.TopMost = true
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents BGTextBox As TextBox
    Friend WithEvents ActiveInsulinTextBox As TextBox
    Friend WithEvents HiddenTextBox As TextBox
    Friend WithEvents DeltaTextBox As TextBox
    Friend WithEvents CloseButton As Button
End Class
