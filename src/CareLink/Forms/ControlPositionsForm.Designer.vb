' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ControlPositionsForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        DgvControlPositions = New DataGridView()
        CType(DgvControlPositions, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' DgvControlPositions
        ' 
        DgvControlPositions.AllowUserToAddRows = False
        DgvControlPositions.AllowUserToDeleteRows = False
        DgvControlPositions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DgvControlPositions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DgvControlPositions.Dock = DockStyle.Fill
        DgvControlPositions.Location = New Point(0, 0)
        DgvControlPositions.Name = "DgvControlPositions"
        DgvControlPositions.ReadOnly = True
        DgvControlPositions.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DgvControlPositions.Size = New Size(1108, 500)
        DgvControlPositions.TabIndex = 0
        ' 
        ' ControlPositionsForm
        ' 
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1108, 500)
        Me.Controls.Add(DgvControlPositions)
        Me.Name = "ControlPositionsForm"
        Me.Text = "Control Positions"
        CType(DgvControlPositions, ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

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

    Friend WithEvents DgvControlPositions As DataGridView
End Class
