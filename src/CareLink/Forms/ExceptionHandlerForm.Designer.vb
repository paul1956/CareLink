' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class ExceptionHandlerForm
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
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents OK As System.Windows.Forms.Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.OK = New Button()
        Me.Cancel = New Button()
        Me.StackTraceTextBox = New TextBox()
        Me.AutoModeStatusLabel = New Label()
        Me.ExceptionTextBox = New TextBox()
        Me.BgReadingLabel = New Label()
        Me.InstructionsRichTextBox = New RichTextBox()
        Me.SuspendLayout()
        ' 
        ' OK
        ' 
        Me.OK.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.OK.DialogResult = DialogResult.OK
        Me.OK.Location = New Point(1190, 348)
        Me.OK.Name = "OK"
        Me.OK.Size = New Size(94, 23)
        Me.OK.TabIndex = 4
        Me.OK.Text = "&OK"
        ' 
        ' Cancel
        ' 
        Me.Cancel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.Cancel.DialogResult = DialogResult.Cancel
        Me.Cancel.Location = New Point(1311, 348)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New Size(94, 23)
        Me.Cancel.TabIndex = 4
        Me.Cancel.Text = "&Cancel"
        ' 
        ' StackTraceTextBox
        ' 
        Me.StackTraceTextBox.Location = New Point(5, 63)
        Me.StackTraceTextBox.Multiline = True
        Me.StackTraceTextBox.Name = "StackTraceTextBox"
        Me.StackTraceTextBox.Size = New Size(1136, 308)
        Me.StackTraceTextBox.TabIndex = 6
        ' 
        ' AutoModeStatusLabel
        ' 
        Me.AutoModeStatusLabel.AutoSize = True
        Me.AutoModeStatusLabel.Location = New Point(7, 6)
        Me.AutoModeStatusLabel.Name = "AutoModeStatusLabel"
        Me.AutoModeStatusLabel.Size = New Size(62, 15)
        Me.AutoModeStatusLabel.TabIndex = 7
        Me.AutoModeStatusLabel.Text = "Exception:"
        ' 
        ' ExceptionTextBox
        ' 
        Me.ExceptionTextBox.Location = New Point(75, 2)
        Me.ExceptionTextBox.Name = "ExceptionTextBox"
        Me.ExceptionTextBox.Size = New Size(1330, 23)
        Me.ExceptionTextBox.TabIndex = 8
        ' 
        ' BgReadingLabel
        ' 
        Me.BgReadingLabel.AutoSize = True
        Me.BgReadingLabel.Location = New Point(11, 39)
        Me.BgReadingLabel.Name = "BgReadingLabel"
        Me.BgReadingLabel.Size = New Size(68, 15)
        Me.BgReadingLabel.TabIndex = 9
        Me.BgReadingLabel.Text = "Stack Trace:"
        ' 
        ' InstructionsRichTextBox
        ' 
        Me.InstructionsRichTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Me.InstructionsRichTextBox.Location = New Point(1147, 60)
        Me.InstructionsRichTextBox.Name = "InstructionsRichTextBox"
        Me.InstructionsRichTextBox.Size = New Size(261, 282)
        Me.InstructionsRichTextBox.TabIndex = 10
        Me.InstructionsRichTextBox.Text = ""
        ' 
        ' ExceptionHandlerForm
        ' 
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1417, 383)
        Me.Controls.Add(Me.InstructionsRichTextBox)
        Me.Controls.Add(Me.BgReadingLabel)
        Me.Controls.Add(Me.ExceptionTextBox)
        Me.Controls.Add(Me.AutoModeStatusLabel)
        Me.Controls.Add(Me.StackTraceTextBox)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Cancel)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExceptionHandlerForm"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Unhandled Exception"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents StackTraceTextBox As TextBox
    Friend WithEvents AutoModeStatusLabel As Label
    Friend WithEvents ExceptionTextBox As TextBox
    Friend WithEvents BgReadingLabel As Label
    Friend WithEvents InstructionsRichTextBox As RichTextBox
End Class
