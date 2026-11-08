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
        PreviousRawDataRTB = New RichTextBox()
        RawDataRTB = New RichTextBox()
        txtFind = New TextBox()
        btnFindNext = New Button()
        btnFindAll = New Button()
        CheckBoxDifferenceOnly = New CheckBox()
        btnRefresh = New Button()
        TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.Controls.Add(OK_Button, 0, 0)
        TableLayoutPanel1.Controls.Add(Cancel_Button, 1, 0)
        TableLayoutPanel1.Location = New Point(1220, 579)
        TableLayoutPanel1.Margin = New Padding(4, 3, 4, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        TableLayoutPanel1.Size = New Size(170, 33)
        TableLayoutPanel1.TabIndex = 2
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
        ' PreviousRawDataRTB
        ' 
        PreviousRawDataRTB.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        PreviousRawDataRTB.Location = New Point(0, 0)
        PreviousRawDataRTB.Name = "PreviousRawDataRTB"
        PreviousRawDataRTB.Size = New Size(701, 576)
        PreviousRawDataRTB.TabIndex = 1
        PreviousRawDataRTB.Text = ""
        ' 
        ' RawDataRTB
        ' 
        RawDataRTB.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        RawDataRTB.Location = New Point(704, 0)
        RawDataRTB.Name = "RawDataRTB"
        RawDataRTB.Size = New Size(701, 576)
        RawDataRTB.TabIndex = 2
        RawDataRTB.Text = ""
        ' 
        ' txtFind
        ' 
        txtFind.Location = New Point(12, 584)
        txtFind.Name = "txtFind"
        txtFind.Size = New Size(200, 23)
        txtFind.TabIndex = 0
        ' 
        ' btnFindNext
        ' 
        btnFindNext.Location = New Point(226, 582)
        btnFindNext.Name = "btnFindNext"
        btnFindNext.Size = New Size(103, 27)
        btnFindNext.TabIndex = 1
        btnFindNext.Text = "Find Next"
        btnFindNext.UseVisualStyleBackColor = True
        ' 
        ' btnFindAll
        ' 
        btnFindAll.Location = New Point(343, 582)
        btnFindAll.Name = "btnFindAll"
        btnFindAll.Size = New Size(103, 27)
        btnFindAll.TabIndex = 1
        btnFindAll.Text = "Find All"
        btnFindAll.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDifferenceOnly
        ' 
        CheckBoxDifferenceOnly.Appearance = Appearance.ToggleSwitch
        CheckBoxDifferenceOnly.AutoSize = True
        CheckBoxDifferenceOnly.Checked = True
        CheckBoxDifferenceOnly.CheckState = CheckState.Checked
        CheckBoxDifferenceOnly.Location = New Point(526, 586)
        CheckBoxDifferenceOnly.Name = "CheckBoxDifferenceOnly"
        CheckBoxDifferenceOnly.Size = New Size(145, 19)
        CheckBoxDifferenceOnly.TabIndex = 3
        CheckBoxDifferenceOnly.Text = "Show Differences Only"
        CheckBoxDifferenceOnly.UseVisualStyleBackColor = True
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Location = New Point(714, 582)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(103, 27)
        btnRefresh.TabIndex = 4
        btnRefresh.Text = "Refresh"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' RawDataViewerDialog
        ' 
        Me.AcceptButton = OK_Button
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Cancel_Button
        Me.ClientSize = New Size(1404, 619)
        Me.Controls.Add(btnRefresh)
        Me.Controls.Add(CheckBoxDifferenceOnly)
        Me.Controls.Add(btnFindAll)
        Me.Controls.Add(btnFindNext)
        Me.Controls.Add(txtFind)
        Me.Controls.Add(PreviousRawDataRTB)
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
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents PreviousRawDataRTB As RichTextBox
    Friend WithEvents RawDataRTB As RichTextBox
    Friend WithEvents txtFind As TextBox
    Friend WithEvents btnFindNext As Button
    Friend WithEvents btnFindAll As Button
    Friend WithEvents CheckBoxDifferenceOnly As CheckBox
    Friend WithEvents btnRefresh As Button

End Class
