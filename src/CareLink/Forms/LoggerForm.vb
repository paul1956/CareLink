' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LoggerForm

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Visible = False
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

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
        End If
    End Sub

    ''' <summary>
    '''  Replace message in log with new message.
    '''  If endKey is <see cref="String.Empty"/> then replace the whole line.
    ''' </summary>
    ''' <param name="startKey">The Text that starts the message to </param>
    ''' <param name="endKey"></param>
    ''' <param name="message">Message to be added to Log</param>
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
