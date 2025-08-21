' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

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

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CleanupStaleFilesDialog_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim searchPattern As String = $"{BaseNameSavedErrorReport}*.txt"
        Dim fileList As String() = IO.Directory.GetFiles(path:=GetProjectDataDirectory(), searchPattern)
        With Me.TreeView1
            .Nodes.Clear()
            .CheckBoxes = True
            .BeginUpdate()
            .Nodes.Add(text:="Error Files")
            .Nodes(index:=0).Checked = True
            For Each file As String In fileList
                .Nodes(index:=0).Nodes.Add(text:=file.Split(separator:="\").Last)
                .Nodes(index:=0).LastNode.Checked = True
            Next
            .Nodes.Add(text:="WebCaches")
            Dim path As String = IO.Path.Join(GetProjectDataDirectory(), "WebCache")
            fileList = IO.Directory.GetDirectories(path)
            For Each file As String In fileList
                Dim webCacheFileName As String = file.Split(separator:="\").Last
                .Nodes(index:=1).Nodes.Add(text:=webCacheFileName)
                .Nodes(index:=1).LastNode.Checked =
                    Not GetWebViewCacheDirectory().EndsWithIgnoreCase(value:=webCacheFileName)
            Next
            .ExpandAll()
            .EndUpdate()
        End With

    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = Me.OptionalConfirmFileDelete(confirm:=True)
        Me.Close()
    End Sub

    Private Sub OkDoNotConfirm_Button_Click(sender As Object, e As EventArgs) _
        Handles OkDoNotConfirm_Button.Click

        Me.DialogResult = Me.OptionalConfirmFileDelete(confirm:=False)
        Me.Close()
    End Sub

    Private Function OptionalConfirmFileDelete(confirm As Boolean) As DialogResult
        Dim result As DialogResult = DialogResult.OK
        With Me.TreeView1
            For Each node As TreeNode In .Nodes(index:=0).Nodes
                Dim msgBoxResult As MsgBoxResult = MsgBoxResult.Yes
                If node.Checked Then
                    If confirm Then
                        msgBoxResult = MsgBox(
                            heading:="",
                            prompt:=$"File {node.Text} will be deleted are you sure?",
                            buttonStyle:=MsgBoxStyle.YesNoCancel,
                            title:="File Deletion")
                    End If
                    Select Case msgBoxResult
                        Case MsgBoxResult.Yes
                            Dim file As String = IO.Path.Join(GetProjectDataDirectory(), node.Text)
                            Dim showUI As FileIO.UIOption = FileIO.UIOption.OnlyErrorDialogs
                            Dim recycle As FileIO.RecycleOption = FileIO.RecycleOption.SendToRecycleBin
                            My.Computer.FileSystem.DeleteFile(file, showUI, recycle)
                        Case MsgBoxResult.Cancel
                            result = DialogResult.Cancel
                            Exit For
                        Case MsgBoxResult.No
                            result = DialogResult.Ignore
                    End Select
                End If
            Next
            For Each node As TreeNode In .Nodes(index:=1).Nodes
                If node.Checked Then
                    Try
                        Dim path As String = IO.Path.Join(GetProjectDataDirectory(), "WebCache", node.Text)
                        IO.Directory.Delete(path, recursive:=True)
                    Catch ex As Exception
                        Stop
                        ' Ignore ones I can't delete
                    End Try
                End If
            Next
        End With
        Return result
    End Function

    Private Sub TreeView1_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCheck
        If e.Action <> TreeViewAction.Unknown Then
            If e.Node.Text = "Error Files" Then
                e.Node.Checked = HasCheckChileNodes(e.Node)
            ElseIf e.Node.Text.StartsWith(value:=BaseNameSavedErrorReport) Then
                e.Node.Parent.Checked = HasCheckChileNodes(node:=e.Node.Parent)
            End If
        End If
    End Sub

    Private Sub TreeView1_BeforeCheck(sender As Object, e As TreeViewCancelEventArgs) _
        Handles TreeView1.BeforeCheck

        If e.Node.Text.StartsWith(value:=BaseNameSavedErrorReport) Then
            e.Cancel = False
            Exit Sub
        End If
        If e.Action = TreeViewAction.Unknown Then
            e.Cancel = False
            Exit Sub
        End If
        If e.Node.Text = "Error Files" Then
            If e.Action = TreeViewAction.ByKeyboard OrElse
               e.Action = TreeViewAction.ByMouse Then

                SetChildNodes(e.Node.Nodes, newValue:=Not e.Node.Checked)
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

End Class
