' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LoggerForm
    Inherits Form

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
        components = New ComponentModel.Container()
        txtLog = New RichTextBox()
        ContextMenuStripLogger = New ContextMenuStrip(components)
        ContextMenuCopy = New ToolStripMenuItem()
        ContextMenuClear = New ToolStripMenuItem()
        ContextMenuSaveSelection = New ToolStripMenuItem()
        ContextMenuSaveAll = New ToolStripMenuItem()
        MenuStripLogger = New MenuStrip()
        tsbFile = New ToolStripMenuItem()
        FileMenuExitMenuItem = New ToolStripMenuItem()
        FileMenuSaveAllToolStripMenuItem = New ToolStripMenuItem()
        FileMenuSaveSelectionToolStripMenuItem = New ToolStripMenuItem()
        EditMenuClearToolStripMenuItem = New ToolStripMenuItem()
        EditMenuCopyToolStripMenuItem = New ToolStripMenuItem()
        EditToolStripMenuItem = New ToolStripMenuItem()
        ViewToolStripMenuItem = New ToolStripMenuItem()
        ViewTopmostToolStripMenuItem = New ToolStripMenuItem()
        HideToolStripMenuItem = New ToolStripMenuItem()
        ClearToolStripMenuItem = New ToolStripMenuItem()
        CopyToolStripMenuItem = New ToolStripMenuItem()
        ContextMenuStripLogger.SuspendLayout()
        MenuStripLogger.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' txtLog
        ' 
        txtLog.ContextMenuStrip = ContextMenuStripLogger
        txtLog.Dock = DockStyle.Top
        txtLog.Location = New Point(0, 24)
        txtLog.Name = "txtLog"
        txtLog.ReadOnly = True
        txtLog.ScrollBars = RichTextBoxScrollBars.Vertical
        txtLog.Size = New Size(1461, 399)
        txtLog.TabIndex = 1
        txtLog.Text = ""
        ' 
        ' ContextMenuStripLogger
        ' 
        ContextMenuStripLogger.Items.AddRange(New ToolStripItem() {ContextMenuCopy, ContextMenuClear, ContextMenuSaveSelection, ContextMenuSaveAll})
        ContextMenuStripLogger.Name = "ContextMenuStripLogger"
        ContextMenuStripLogger.Size = New Size(108, 70)
        ' 
        ' ContextMenuCopy
        ' 
        ContextMenuCopy.Name = "ContextMenuCopy"
        ContextMenuCopy.Size = New Size(107, 22)
        ContextMenuCopy.Text = "Copy"
        ' 
        ' ContextMenuClear
        ' 
        ContextMenuClear.Name = "ContextMenuClear"
        ContextMenuClear.Size = New Size(107, 22)
        ContextMenuClear.Text = "Clear"
        ' 
        ' ContextMenuSaveSelection
        ' 
        ContextMenuSaveSelection.Name = "ContextMenuSaveSelection"
        ContextMenuSaveSelection.Size = New Size(107, 22)
        ContextMenuSaveSelection.Text = "Save Selection..."
        ' 
        ' ContextMenuSaveAll
        ' 
        ContextMenuSaveAll.Name = "ContextMenuSaveAll"
        ContextMenuSaveAll.Size = New Size(107, 22)
        ContextMenuSaveAll.Text = "Save All..."
        ' 
        ' MenuStripLogger
        ' 
        MenuStripLogger.Items.AddRange(New ToolStripItem() {tsbFile, EditToolStripMenuItem, ViewToolStripMenuItem})
        MenuStripLogger.Location = New Point(0, 0)
        MenuStripLogger.Name = "MenuStripLogger"
        MenuStripLogger.Size = New Size(1461, 24)
        MenuStripLogger.TabIndex = 2
        MenuStripLogger.Text = "MenuStripLogger"
        ' 
        ' tsbFile
        ' 
        tsbFile.DropDownItems.AddRange(New ToolStripItem() {FileMenuSaveAllToolStripMenuItem, FileMenuSaveSelectionToolStripMenuItem, FileMenuExitMenuItem})
        tsbFile.Name = "tsbFile"
        tsbFile.ShowShortcutKeys = False
        tsbFile.Size = New Size(37, 20)
        tsbFile.Text = "File"
        ' 
        ' FileMenuSaveAllToolStripMenuItem
        ' 
        FileMenuSaveAllToolStripMenuItem.Name = "FileMenuSaveAllToolStripMenuItem"
        FileMenuSaveAllToolStripMenuItem.Size = New Size(180, 22)
        FileMenuSaveAllToolStripMenuItem.Text = "Save All..."
        ' 
        ' FileMenuSaveSelectionToolStripMenuItem
        ' 
        FileMenuSaveSelectionToolStripMenuItem.Name = "FileMenuSaveSelectionToolStripMenuItem"
        FileMenuSaveSelectionToolStripMenuItem.Size = New Size(180, 22)
        FileMenuSaveSelectionToolStripMenuItem.Text = "Save Selection..."
        ' 
        ' FileMenuExitMenuItem
        ' 
        FileMenuExitMenuItem.Name = "FileMenuExitMenuItem"
        FileMenuExitMenuItem.Size = New Size(180, 22)
        FileMenuExitMenuItem.Text = "Exit"
        ' 
        ' EditToolStripMenuItem
        ' 
        EditToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {EditMenuClearToolStripMenuItem, EditMenuCopyToolStripMenuItem})
        EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        EditToolStripMenuItem.Size = New Size(39, 20)
        EditToolStripMenuItem.Text = "Edit"
        ' 
        ' EditMenuClearToolStripMenuItem
        ' 
        EditMenuClearToolStripMenuItem.Name = "EditMenuClearToolStripMenuItem"
        EditMenuClearToolStripMenuItem.Size = New Size(102, 22)
        EditMenuClearToolStripMenuItem.Text = "Clear"
        ' 
        ' EditMenuCopyToolStripMenuItem
        ' 
        EditMenuCopyToolStripMenuItem.Name = "EditMenuCopyToolStripMenuItem"
        EditMenuCopyToolStripMenuItem.Size = New Size(102, 22)
        EditMenuCopyToolStripMenuItem.Text = "Copy"
        ' 
        ' ViewToolStripMenuItem
        ' 
        ViewToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ViewTopmostToolStripMenuItem, HideToolStripMenuItem})
        ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        ViewToolStripMenuItem.Size = New Size(44, 20)
        ViewToolStripMenuItem.Text = "View"
        ' 
        ' ViewTopmostToolStripMenuItem
        ' 
        ViewTopmostToolStripMenuItem.Checked = True
        ViewTopmostToolStripMenuItem.CheckOnClick = True
        ViewTopmostToolStripMenuItem.CheckState = CheckState.Checked
        ViewTopmostToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text
        ViewTopmostToolStripMenuItem.Name = "ViewTopmostToolStripMenuItem"
        ViewTopmostToolStripMenuItem.Size = New Size(121, 22)
        ViewTopmostToolStripMenuItem.Text = "Topmost"
        ' 
        ' HideToolStripMenuItem
        ' 
        HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        HideToolStripMenuItem.Size = New Size(121, 22)
        HideToolStripMenuItem.Text = "Hide"
        ' 
        ' ClearToolStripMenuItem
        ' 
        ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        ClearToolStripMenuItem.Size = New Size(32, 19)
        ' 
        ' CopyToolStripMenuItem
        ' 
        CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        CopyToolStripMenuItem.Size = New Size(32, 19)
        ' 
        ' LoggerForm
        ' 
        Me.AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(1461, 480)
        Me.Controls.Add(txtLog)
        Me.Controls.Add(MenuStripLogger)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Margin = New Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoggerForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Debug Logger"
        ContextMenuStripLogger.ResumeLayout(False)
        MenuStripLogger.ResumeLayout(False)
        MenuStripLogger.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ClearToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuClear As ToolStripMenuItem
    Friend WithEvents ContextMenuCopy As ToolStripMenuItem
    Friend WithEvents ContextMenuSaveAll As ToolStripMenuItem
    Friend WithEvents ContextMenuSaveSelection As ToolStripMenuItem
    Friend WithEvents ContextMenuStripLogger As ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditMenuClearToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditMenuCopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FileMenuExitMenuItem As ToolStripMenuItem
    Friend WithEvents FileMenuSaveAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FileMenuSaveSelectionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MenuStripLogger As MenuStrip
    Friend WithEvents tsbFile As ToolStripMenuItem
    Friend WithEvents txtLog As RichTextBox
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewTopmostToolStripMenuItem As ToolStripMenuItem

End Class
