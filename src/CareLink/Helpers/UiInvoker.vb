' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports DocumentFormat.OpenXml.Office2016.Drawing.Charts

Friend Module UiInvoker
    <Extension>
    Friend Sub UpdateDgvCellSafe(dgv As DataGridView, text As String)
        Try
            ' Check if we need to marshal to the UI thread
            If dgv.InvokeRequired Then
                ' Use Invoke to run on the UI thread without requiring form-specific delegates
                Dim method As Action =
                    Sub()
                        dgv.CurrentCell.Value = text
                    End Sub
                dgv.Invoke(method)
            Else
                ' Safe to update directly
                dgv.CurrentCell.Value = text
            End If
        Catch ex As Exception
            Stop
        End Try

    End Sub

    ''' <summary>
    '''  Updates the text of a Label in a thread-safe manner,
    '''  ensuring that the update occurs on the UI thread.
    ''' </summary>
    ''' <param name="lbl">The Label whose text needs to be updated.</param>
    ''' <param name="text">The new text for the Label.</param>
    <Extension>
    Friend Sub UpdateLabelSafe(lbl As Label, text As String)
        Try
            ' Check if we need to marshal to the UI thread
            If lbl.InvokeRequired Then
                ' Use Invoke to run on the UI thread without requiring form-specific delegates
                Dim method As Action =
                    Sub()
                        lbl.Text = text
                    End Sub
                lbl.Invoke(method)
            Else
                ' Safe to update directly
                lbl.Text = text
            End If
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''  Updates the text of a TextBox in a thread-safe manner,
    '''  ensuring that the update occurs on the UI thread.
    ''' </summary>
    ''' <param name="txtBox">The TextBox whose text needs to be updated.</param>
    ''' <param name="text">The new text for the TextBox.</param>
    <Extension>
    Friend Sub UpdateTextBoxSafe(txtBox As TextBox, text As String)
        Try
            ' Check if we need to marshal to the UI thread
            If txtBox.InvokeRequired Then
                ' Use Invoke to run on the UI thread without requiring form-specific delegates
                Dim method As Action =
                    Sub()
                        txtBox.Text = text
                    End Sub
                txtBox.Invoke(method)
            Else
                ' Safe to update directly
                txtBox.Text = text
            End If
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''   Updates the text of a form in a thread-safe manner,
    '''   ensuring that the update occurs on the UI thread.
    ''' </summary>
    ''' <param name="form">The form whose text needs to be updated.</param>
    ''' <param name="text">The new text for the form.</param>
    <Extension>
    Friend Sub UpdateFormSafe(form As Form, text As String)
        Try
            ' Check if we need to marshal to the UI thread
            If form.InvokeRequired Then
                ' Use Invoke to run on the UI thread without requiring form-specific delegates
                Dim method As Action =
                    Sub()
                        form.Text = text
                    End Sub
                form.Invoke(method)
            Else
                ' Safe to update directly
                form.Text = text
            End If
        Catch ex As Exception
            Stop
        End Try
    End Sub

    Public Sub Invoke(method As Action)
        If method Is Nothing Then
            Return
        End If

        Try
            Dim uiForm As Form = Nothing
            If Application.OpenForms.Count > 0 Then
                uiForm = Application.OpenForms(index:=0)
            End If

            If uiForm IsNot Nothing Then
                If uiForm.InvokeRequired Then
                    uiForm.Invoke(method)
                Else
                    method()
                End If
            Else
                ' No open forms available to marshal to; run the method directly.
                ' This may still fail if called from an MTA thread and the method creates
                ' OLE/COM objects that require STA. Prefer calling this helper when a UI
                ' form is available.
                method()
            End If
        Catch ex As Exception
            ' Best effort only; rethrow to surface issues to caller
            Throw
        End Try
    End Sub

    ''' <summary>
    '''  Invoke an action on the UI thread using the provided owner form as the marshal target.
    ''' </summary>
    Public Sub Invoke(owner As Form, method As Action)
        Try
            If method Is Nothing Then
                Return
            End If

            If owner Is Nothing Then
                Invoke(method)
                Return
            End If

            If owner.InvokeRequired Then
                owner.Invoke(method)
            Else
                method()
            End If
        Catch ex As Exception
            Stop
        End Try
    End Sub

End Module
