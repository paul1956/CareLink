' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LoggerForm

    Private Sub ClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearToolStripMenuItem.Click
        Try
            Me.txtLog.Clear()
        Catch
        End Try
    End Sub

    Private Sub ContextMenuClear_Click(sender As Object, e As EventArgs) Handles ContextMenuClear.Click
        Try
            Me.txtLog.Clear()
        Catch
        End Try
    End Sub

    Private Sub ContextMenuCopy_Click(sender As Object, e As EventArgs) Handles ContextMenuCopy.Click
        Me.CopyTextToClipboard()
    End Sub

    Private Sub ContextMenuSaveSelection_Click(sender As Object, e As EventArgs) Handles ContextMenuSaveSelection.Click
        ' Save only the selected text when user chooses Save Selection
        Me.SaveText(SelectionOnly:=True)
    End Sub

    Private Sub CopyTextToClipboard()
        Try
            If Me.txtLog.SelectionLength > 0 Then
                Me.txtLog.Copy()
            End If
        Catch
        End Try
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Me.CopyTextToClipboard()
    End Sub

    Private Sub EditClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditMenuClearToolStripMenuItem.Click
        Me.txtLog.Clear()
    End Sub

    Private Sub EditCopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditMenuCopyToolStripMenuItem.Click
        Me.CopyTextToClipboard()
    End Sub

    Private Sub FileMenuExitMenuItem_Click(sender As Object, e As EventArgs) Handles FileMenuExitMenuItem.Click
        Me.Close()
    End Sub

    Private Sub FileMenuSaveAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileMenuSaveAllToolStripMenuItem.Click
        Try
            Me.SaveText()
        Catch ex As Exception
            Try
                MessageBox.Show(text:=ex.Message, caption:="Save failed", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
            Catch
            End Try
        End Try
    End Sub

    Private Sub FileMenuSaveSelectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileMenuSaveSelectionToolStripMenuItem.Click
        Try
            ' Save only the selected text when user chooses Save Selection
            Me.SaveText(SelectionOnly:=True)
        Catch ex As Exception
            Try
                MessageBox.Show(text:=ex.Message, caption:="Save failed", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Error)
            Catch
            End Try
        End Try
    End Sub

    Private Sub HideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideToolStripMenuItem.Click
        Me.Hide()
    End Sub

    Private Sub SaveText(Optional SelectionOnly As Boolean = False)
        Using dlg As New SaveFileDialog()
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            dlg.DefaultExt = "txt"
            dlg.FileName = "CareLink_Log.txt"
            If dlg.ShowDialog(owner:=Me) = DialogResult.OK Then
                Dim contents As String = Me.txtLog.Text
                If SelectionOnly AndAlso Me.txtLog.SelectionLength > 0 Then
                    contents = Me.txtLog.SelectedText
                End If

                IO.File.WriteAllText(path:=dlg.FileName, contents)
            End If
        End Using
    End Sub

    Private Sub tsbSave_Click(sender As Object, e As EventArgs)
        ' Delegate to shared SaveText implementation
        Me.SaveText()
    End Sub

    Private Sub txtLog_KeyDown(sender As Object, e As KeyEventArgs) Handles txtLog.KeyDown
        Try
            If e.Control AndAlso e.KeyCode = Keys.C Then
                If Me.txtLog.SelectionLength > 0 Then
                    Me.txtLog.Copy()
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub ViewTopmostToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewTopmostToolStripMenuItem.Click
        Try
            Me.TopMost = Me.ViewTopmostToolStripMenuItem.Checked
        Catch
        End Try
    End Sub

    ''' <summary>
    '''  Logs a message to the logger form. If the method is called
    '''  from a thread other than the UI thread, it uses BeginInvoke to marshal
    '''  the call to the UI thread.
    ''' </summary>
    ''' <param name="message">
    '''  The message to log.
    ''' </param>
    Public Sub LogMessage(message As String)
        If Me.InvokeRequired Then
            Dim method As New Action(Of String)(AddressOf Me.LogMessage)
            Me.BeginInvoke(method, message)
        Else
            If Not String.IsNullOrWhiteSpace(value:=Me.txtLog.Text) AndAlso
                Me.txtLog.Text.Last <> vbLf Then
                Me.txtLog.AppendNewLine
            End If
            Me.txtLog.AppendText(text:=message)
            Me.txtLog.AppendNewLine
            Me.Visible = Me.txtLog.Lines.Length > 1
            Me.txtLog.ScrollToCaret()
        End If
    End Sub

    ''' <summary>
    '''  Replace message in log with new message.
    '''  If endKey is <see cref="String.Empty"/> then replace the whole line.
    ''' </summary>
    ''' <param name="startKey">The Text that starts the message.</param>
    ''' <param name="endKey">
    '''  The key identifying the end of the message to update.
    ''' </param>
    ''' <param name="message">
    '''  The new message to replace the existing one.
    ''' </param>
    Public Sub UpdateLogMessage(startKey As String,
                                endKey As String,
                                message As String)

        If Me.InvokeRequired Then
            Dim method As New Action(Of String, String, String)(AddressOf Me.UpdateLogMessage)
            Me.BeginInvoke(method, startKey, endKey, message)
        Else
            Dim startIndex As Integer =
            Me.txtLog.Text.IndexOf(value:=startKey)
            Dim endIndex As Integer = -1

            If startIndex <> -1 Then
                If endKey = String.Empty Then
                    ' Replace until the end of the current line when no endKey is provided
                    Dim newlineIndex As Integer =
                        Me.txtLog.Text.IndexOf(value:=vbLf,
                                               startIndex:=startIndex + startKey.Length)
                    endIndex = If(newlineIndex = -1,
                                  Me.txtLog.Text.Length,
                                  newlineIndex)
                Else
                    endIndex =
                        Me.txtLog.Text.IndexOf(value:=endKey,
                                               startIndex:=startIndex + startKey.Length)
                End If
            End If

            If startIndex <> -1 AndAlso
               endIndex <> -1 AndAlso
               endIndex > startIndex Then

                ' Calculate the range to replace INCLUDING the startKey and endKey (if present)
                Dim replaceStart As Integer = startIndex
                Dim lengthToReplace As Integer

                If endKey = String.Empty Then
                    ' endIndex points to the newline or end of text; replace from startKey to before newline
                    lengthToReplace = endIndex - replaceStart
                Else
                    ' endIndex points to start of endKey; include endKey in the replacement
                    lengthToReplace = endIndex + endKey.Length - replaceStart
                End If

                ' Replace the text including the keywords
                Me.txtLog.Select(start:=replaceStart, length:=lengthToReplace)
                Me.txtLog.SelectedText = message
            Else
                Me.txtLog.AppendText(text:=message)
                Me.txtLog.AppendNewLine
            End If
            Me.Visible = Me.txtLog.Lines.Length > 1
        End If
    End Sub

End Class
