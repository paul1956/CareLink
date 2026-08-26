' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RawDataViewerDialog
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
        TableLayoutPanel1 = New TableLayoutPanel()
        OK_Button = New Button()
        Cancel_Button = New Button()
        RawDataRTB = New RichTextBox()
        TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Controls.Add(OK_Button, 0, 0)
        TableLayoutPanel1.Controls.Add(Cancel_Button, 1, 0)
        TableLayoutPanel1.Location = New Point(654, 579)
        TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Size = New Size(170, 33)
        TableLayoutPanel1.TabIndex = 0
        ' 
        ' OK_Button
        ' 
        OK_Button.Anchor = AnchorStyles.None
        OK_Button.Location = New Point(4, 3)
        OK_Button.Margin = New Padding(4, 3, 4, 3)
        OK_Button.Name = "OK_Button"
        OK_Button.Size = New Size(77, 27)
        OK_Button.TabIndex = 0
        OK_Button.Text = "OK"
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.None
        Cancel_Button.Location = New Point(89, 3)
        Cancel_Button.Margin = New Padding(4, 3, 4, 3)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(77, 27)
        Cancel_Button.TabIndex = 1
        Cancel_Button.Text = "Cancel"
        ' 
        ' RawDataRTB
        ' 
        RawDataRTB.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        RawDataRTB.Location = New Point(-3, 2)
        RawDataRTB.Name = "RawDataRTB"
        RawDataRTB.Size = New Size(843, 564)
        RawDataRTB.TabIndex = 1
        RawDataRTB.Text = ""
        ' 
        ' RawDataViewerDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7F, 15F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(838, 619)
        Me.Controls.Add(RawDataRTB)
        Me.Controls.Add(TableLayoutPanel1)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RawDataViewerDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Raw Data Viewer"
        TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents RawDataRTB As RichTextBox

End Class
