' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SgMiniForm
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        SgTextBox = New TextBox()
        ActiveInsulinTextBox = New TextBox()
        HiddenTextBox = New TextBox()
        DeltaTextBox = New TextBox()
        CloseButton = New Button()
        ChkTopMost = New CheckBox()
        Me.SuspendLayout()
        ' 
        ' SgTextBox
        ' 
        SgTextBox.BorderStyle = BorderStyle.None
        SgTextBox.Font = New Font("Segoe UI", 36.0F, FontStyle.Regular, GraphicsUnit.Point)
        SgTextBox.Location = New Point(1, 29)
        SgTextBox.Margin = New Padding(0)
        SgTextBox.Name = "SgTextBox"
        SgTextBox.Size = New Size(134, 64)
        SgTextBox.TabIndex = 1
        SgTextBox.Text = "---"
        SgTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' ActiveInsulinTextBox
        ' 
        ActiveInsulinTextBox.BorderStyle = BorderStyle.None
        ActiveInsulinTextBox.Dock = DockStyle.Top
        ActiveInsulinTextBox.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point)
        ActiveInsulinTextBox.Location = New Point(0, 0)
        ActiveInsulinTextBox.Name = "ActiveInsulinTextBox"
        ActiveInsulinTextBox.Size = New Size(264, 26)
        ActiveInsulinTextBox.TabIndex = 0
        ActiveInsulinTextBox.Text = "Active Insulin: ???"
        ActiveInsulinTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' HiddenTextBox
        ' 
        HiddenTextBox.Location = New Point(0, -1)
        HiddenTextBox.Name = "HiddenTextBox"
        HiddenTextBox.Size = New Size(100, 23)
        HiddenTextBox.TabIndex = 2
        ' 
        ' DeltaTextBox
        ' 
        DeltaTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DeltaTextBox.BorderStyle = BorderStyle.None
        DeltaTextBox.Font = New Font("Segoe UI", 36.0F, FontStyle.Regular, GraphicsUnit.Point)
        DeltaTextBox.Location = New Point(135, 40)
        DeltaTextBox.Margin = New Padding(0)
        DeltaTextBox.Name = "DeltaTextBox"
        DeltaTextBox.Size = New Size(121, 64)
        DeltaTextBox.TabIndex = 2
        DeltaTextBox.Text = "+10.0"
        DeltaTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' CloseButton
        ' 
        CloseButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CloseButton.Location = New Point(82, 99)
        CloseButton.Name = "CloseButton"
        CloseButton.Size = New Size(182, 23)
        CloseButton.TabIndex = 3
        CloseButton.Text = "Show Main Display ALT+W"
        CloseButton.UseVisualStyleBackColor = True
        ' 
        ' ChkTopMost
        ' 
        ChkTopMost.AutoSize = True
        ChkTopMost.Checked = True
        ChkTopMost.CheckState = CheckState.Checked
        ChkTopMost.Location = New Point(4, 103)
        ChkTopMost.Name = "ChkTopMost"
        ChkTopMost.Size = New Size(72, 19)
        ChkTopMost.TabIndex = 4
        ChkTopMost.Text = "Topmost"
        ChkTopMost.UseVisualStyleBackColor = True
        ' 
        ' SgMiniForm
        ' 
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = SystemColors.Window
        Me.ClientSize = New Size(264, 123)
        Me.ControlBox = False
        Me.Controls.Add(ChkTopMost)
        Me.Controls.Add(CloseButton)
        Me.Controls.Add(DeltaTextBox)
        Me.Controls.Add(ActiveInsulinTextBox)
        Me.Controls.Add(SgTextBox)
        Me.Controls.Add(HiddenTextBox)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SgMiniForm"
        Me.ShowInTaskbar = False
        Me.Text = "Current Glucose Value"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents ActiveInsulinTextBox As TextBox
    Friend WithEvents ChkTopMost As CheckBox
    Friend WithEvents CloseButton As Button
    Friend WithEvents DeltaTextBox As TextBox
    Friend WithEvents HiddenTextBox As TextBox
    Friend WithEvents SgTextBox As TextBox
End Class
