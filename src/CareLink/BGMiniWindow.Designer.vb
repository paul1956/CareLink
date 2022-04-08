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
        Me.SuspendLayout
        '
        'BGTextBox
        '
        Me.BGTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BGTextBox.Font = New System.Drawing.Font("Segoe UI", 72!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.BGTextBox.Location = New System.Drawing.Point(0, 0)
        Me.BGTextBox.Multiline = true
        Me.BGTextBox.Name = "BGTextBox"
        Me.BGTextBox.Size = New System.Drawing.Size(224, 123)
        Me.BGTextBox.TabIndex = 0
        Me.BGTextBox.Text = "999"
        Me.BGTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'BGMiniWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 15!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(224, 123)
        Me.Controls.Add(Me.BGTextBox)
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
End Class
