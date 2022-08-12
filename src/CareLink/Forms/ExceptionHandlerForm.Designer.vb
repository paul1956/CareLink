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
        Me.OK = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.StackTraceTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ExceptionTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.InstructionsRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'OK
        '
        Me.OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK.Location = New System.Drawing.Point(1190, 348)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(94, 23)
        Me.OK.TabIndex = 4
        Me.OK.Text = "&OK"
        '
        'Cancel
        '
        Me.Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(1311, 348)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(94, 23)
        Me.Cancel.TabIndex = 4
        Me.Cancel.Text = "&Cancel"
        '
        'StackTraceTextBox
        '
        Me.StackTraceTextBox.Location = New System.Drawing.Point(5, 63)
        Me.StackTraceTextBox.Multiline = True
        Me.StackTraceTextBox.Name = "StackTraceTextBox"
        Me.StackTraceTextBox.Size = New System.Drawing.Size(1136, 308)
        Me.StackTraceTextBox.TabIndex = 6
        '
        'LabelAutoModeStatus
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 6)
        Me.Label1.Name = "LabelAutoModeStatus"
        Me.Label1.Size = New System.Drawing.Size(62, 15)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Exception:"
        '
        'ExceptionTextBox
        '
        Me.ExceptionTextBox.Location = New System.Drawing.Point(75, 2)
        Me.ExceptionTextBox.Name = "ExceptionTextBox"
        Me.ExceptionTextBox.Size = New System.Drawing.Size(1330, 23)
        Me.ExceptionTextBox.TabIndex = 8
        '
        'LabelBgReading
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 39)
        Me.Label2.Name = "LabelBgReading"
        Me.Label2.Size = New System.Drawing.Size(68, 15)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Stack Trace:"
        '
        'InstructionsRichTextBox
        '
        Me.InstructionsRichTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InstructionsRichTextBox.Location = New System.Drawing.Point(1147, 60)
        Me.InstructionsRichTextBox.Name = "InstructionsRichTextBox"
        Me.InstructionsRichTextBox.Size = New System.Drawing.Size(261, 282)
        Me.InstructionsRichTextBox.TabIndex = 10
        Me.InstructionsRichTextBox.Text = ""
        '
        'ExceptionHandlerForm
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1417, 383)
        Me.Controls.Add(Me.InstructionsRichTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ExceptionTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.StackTraceTextBox)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Cancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExceptionHandlerForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Unhandled Exception"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StackTraceTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ExceptionTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents InstructionsRichTextBox As RichTextBox
End Class
