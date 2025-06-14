' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Class CleanupStaleFilesDialog

    Private Shared Function HasCheckChileNodes(node As TreeNode) As Boolean
        If node.Nodes.Count = 0 Then
            Return False
        End If
        Dim childNode As TreeNode = Nothing
        For Each childNode In node.Nodes
            If childNode.Checked Then
                Return True
            End If
        Next
        Return HasCheckChileNodes(childNode)
    End Function

    Private Shared Sub SetChildNodes(nodes As TreeNodeCollection, newValue As Boolean)
        For Each node As TreeNode In nodes
            node.Checked = newValue
        Next
    End Sub

    Private Function OptionalConfirmFileDelete(confirm As Boolean) As DialogResult
        Dim result As DialogResult = DialogResult.OK
        With Me.TreeView1
            For Each node As TreeNode In .Nodes(0).Nodes
                Dim msgBoxResult As MsgBoxResult = MsgBoxResult.Yes
                If node.Checked Then
                    If confirm Then
                        msgBoxResult = MsgBox(
                            heading:="",
                            text:=$"File {node.Text} will be deleted are you sure?",
                            buttonStyle:=MsgBoxStyle.YesNoCancel,
                            title:="File Deletion")
                    End If
                    Select Case msgBoxResult
                        Case MsgBoxResult.Yes
                            My.Computer.FileSystem.DeleteFile(Path.Join(DirectoryForProjectData, node.Text), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                        Case MsgBoxResult.Cancel
                            result = DialogResult.Cancel
                            Exit For
                        Case MsgBoxResult.No
                            result = DialogResult.Ignore
                    End Select
                End If
            Next
            For Each node As TreeNode In .Nodes(1).Nodes
                If node.Checked Then
                    Try
                        Directory.Delete(Path.Join(DirectoryForProjectData, "WebCache", node.Text), True)
                    Catch ex As Exception
                        Stop
                        ' Ignore ones I can't delete
                    End Try
                End If
            Next
        End With
        Return result
    End Function

#Region "Events"

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CleanupStaleFilesDialog_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim fileList As String() = Directory.GetFiles(DirectoryForProjectData, $"{BaseNameSavedErrorReport}*.txt")
        With Me.TreeView1
            .Nodes.Clear()
            .CheckBoxes = True
            .BeginUpdate()
            .Nodes.Add("Error Files")
            .Nodes(0).Checked = True
            For Each fi As String In fileList
                .Nodes(0).Nodes.Add(fi.Split("\").Last)
                .Nodes(0).LastNode.Checked = True
            Next
            .Nodes.Add("WebCaches")
            fileList = Directory.GetDirectories(Path.Join(DirectoryForProjectData, "WebCache"))
            For Each fi As String In fileList
                Dim webCacheFileName As String = fi.Split("\").Last
                .Nodes(1).Nodes.Add(webCacheFileName)
                .Nodes(1).LastNode.Checked = Not GetWebViewCacheDirectory().EndsWith(webCacheFileName)
            Next
            .ExpandAll()
            .EndUpdate()
        End With

    End Sub

    Private Sub TreeView1_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCheck
        If e.Action <> TreeViewAction.Unknown Then
            If e.Node.Text = "Error Files" Then
                e.Node.Checked = HasCheckChileNodes(e.Node)
            ElseIf e.Node.Text.StartsWith(BaseNameSavedErrorReport) Then
                e.Node.Parent.Checked = HasCheckChileNodes(e.Node.Parent)
            End If
        End If
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = Me.OptionalConfirmFileDelete(confirm:=True)
        Me.Close()
    End Sub

    Private Sub OkDoNotConfirm_Button_Click(sender As Object, e As EventArgs) Handles OkDoNotConfirm_Button.Click
        Me.DialogResult = Me.OptionalConfirmFileDelete(confirm:=False)
        Me.Close()
    End Sub

    Private Sub TreeView1_BeforeCheck(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView1.BeforeCheck
        If e.Node.Text.StartsWith(BaseNameSavedErrorReport) Then
            e.Cancel = False
            Exit Sub
        End If
        If e.Action = TreeViewAction.Unknown Then
            e.Cancel = False
            Exit Sub
        End If
        If e.Node.Text = "Error Files" Then
            If e.Action = TreeViewAction.ByKeyboard OrElse e.Action = TreeViewAction.ByMouse Then
                SetChildNodes(e.Node.Nodes, Not e.Node.Checked)
                e.Cancel = False
                Exit Sub
            End If
            e.Cancel = True
            Exit Sub
        End If
        If e.Node.Text = "WebCaches" Then
            e.Cancel = True
            Exit Sub
        End If
        e.Cancel = True
    End Sub

#End Region ' Events

End Class
