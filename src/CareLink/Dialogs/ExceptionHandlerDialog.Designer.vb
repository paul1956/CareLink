' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class ExceptionHandlerDialog
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
    Friend WithEvents Cancel As Button
    Friend WithEvents OK As Button

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        OK = New Button()
        Cancel = New Button()
        traceTextBox = New TextBox()
        AutoModeStatusLabel = New Label()
        exTextBox = New TextBox()
        BgReadingLabel = New Label()
        InstructionRtb = New RichTextBox()
        Me.SuspendLayout()
        ' 
        ' OK
        ' 
        OK.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        OK.DialogResult = DialogResult.OK
        OK.Location = New Point(1190, 348)
        OK.Name = "OK"
        OK.Size = New Size(94, 23)
        OK.TabIndex = 4
        OK.Text = "&OK"
        ' 
        ' Cancel
        ' 
        Cancel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Cancel.DialogResult = DialogResult.Cancel
        Cancel.Location = New Point(1311, 348)
        Cancel.Name = "Cancel"
        Cancel.Size = New Size(94, 23)
        Cancel.TabIndex = 4
        Cancel.Text = "&Cancel"
        ' 
        ' traceTextBox
        ' 
        traceTextBox.Location = New Point(5, 63)
        traceTextBox.Multiline = True
        traceTextBox.Name = "traceTextBox"
        traceTextBox.Size = New Size(1136, 308)
        traceTextBox.TabIndex = 6
        ' 
        ' AutoModeStatusLabel
        ' 
        AutoModeStatusLabel.AutoSize = True
        AutoModeStatusLabel.Location = New Point(7, 6)
        AutoModeStatusLabel.Name = "AutoModeStatusLabel"
        AutoModeStatusLabel.Size = New Size(62, 15)
        AutoModeStatusLabel.TabIndex = 7
        AutoModeStatusLabel.Text = "Exception:"
        ' 
        ' exTextBox
        ' 
        exTextBox.Location = New Point(75, 2)
        exTextBox.Name = "exTextBox"
        exTextBox.Size = New Size(1330, 23)
        exTextBox.TabIndex = 8
        ' 
        ' BgReadingLabel
        ' 
        BgReadingLabel.AutoSize = True
        BgReadingLabel.Location = New Point(11, 39)
        BgReadingLabel.Name = "BgReadingLabel"
        BgReadingLabel.Size = New Size(68, 15)
        BgReadingLabel.TabIndex = 9
        BgReadingLabel.Text = "Stack Trace:"
        ' 
        ' InstructionRtb
        ' 
        InstructionRtb.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        InstructionRtb.Location = New Point(1147, 60)
        InstructionRtb.Name = "InstructionRtb"
        InstructionRtb.Size = New Size(261, 282)
        InstructionRtb.TabIndex = 10
        InstructionRtb.Text = ""
        ' 
        ' ExceptionHandlerDialog
        ' 
        Me.AcceptButton = OK
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1417, 383)
        Me.Controls.Add(InstructionRtb)
        Me.Controls.Add(BgReadingLabel)
        Me.Controls.Add(exTextBox)
        Me.Controls.Add(AutoModeStatusLabel)
        Me.Controls.Add(traceTextBox)
        Me.Controls.Add(OK)
        Me.Controls.Add(Cancel)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExceptionHandlerDialog"
        Me.SizeGripStyle = SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Unhandled Exception"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents traceTextBox As TextBox
    Friend WithEvents AutoModeStatusLabel As Label
    Friend WithEvents exTextBox As TextBox
    Friend WithEvents BgReadingLabel As Label
    Friend WithEvents InstructionRtb As RichTextBox
End Class
