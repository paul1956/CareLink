' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TaskDialogHelpers

    Private Function GetPrompt(prompt As String, autoCloseTimeOut As Integer, remainingTenthSeconds As Integer) As String
        If autoCloseTimeOut < 0 Then Return prompt
        Return String.Format($"{prompt}{vbCrLf}{"Closing in {0} seconds..."}", (remainingTenthSeconds + 9) \ 10)
    End Function

    <Extension>
    Public Function MsgBoxTest(prompt As String, buttonStyle As MsgBoxStyle, title As String, Optional autoCloseTimeOutSeconds As Integer = -1) As MsgBoxResult
        Dim remainingTenthSeconds As Integer = autoCloseTimeOutSeconds * 10

        Dim page As New TaskDialogPage() With
        {
            .Heading = title,
            .Text = GetPrompt(prompt, autoCloseTimeOutSeconds, remainingTenthSeconds)
         }
        If autoCloseTimeOutSeconds > -1 Then
            page.ProgressBar = New TaskDialogProgressBar() With
            {
                .State = TaskDialogProgressBarState.Paused
            }
        End If
        Dim buttonCollection As New TaskDialogButtonCollection
        Dim okButton As New TaskDialogButton("Ok")
        Select Case buttonStyle And &H7
            Case MsgBoxStyle.OkOnly
                buttonCollection.Add(okButton)

            Case MsgBoxStyle.OkCancel
                buttonCollection.Add(okButton)
                buttonCollection.Add(TaskDialogButton.Cancel)

            Case MsgBoxStyle.AbortRetryIgnore
                buttonCollection.Add(TaskDialogButton.Abort)
                buttonCollection.Add(TaskDialogButton.Retry)
                buttonCollection.Add(TaskDialogButton.Ignore)

            Case MsgBoxStyle.YesNoCancel
                buttonCollection.Add(TaskDialogButton.Yes)
                buttonCollection.Add(TaskDialogButton.No)
                buttonCollection.Add(TaskDialogButton.Cancel)

            Case MsgBoxStyle.YesNo
                buttonCollection.Add(TaskDialogButton.Yes)
                buttonCollection.Add(TaskDialogButton.No)

            Case MsgBoxStyle.RetryCancel
                buttonCollection.Add(TaskDialogButton.Retry)
                buttonCollection.Add(TaskDialogButton.Cancel)
            Case Else
                Stop
        End Select

        Select Case buttonStyle And &H30
            Case 0
            Case MsgBoxStyle.Critical
                page.Icon = TaskDialogIcon.Error
            Case MsgBoxStyle.Exclamation
                page.Icon = TaskDialogIcon.Warning
            Case MsgBoxStyle.Information
                page.Icon = TaskDialogIcon.Information
            Case MsgBoxStyle.Question
                page.Icon = New TaskDialogIcon(My.Resources.QuestionMark)
            Case Else
                Stop
        End Select

        Select Case buttonStyle And &H300
            Case MsgBoxStyle.DefaultButton1
                page.DefaultButton = buttonCollection(0)
            Case MsgBoxStyle.DefaultButton2
                page.DefaultButton = buttonCollection(1)
            Case MsgBoxStyle.DefaultButton3
                page.DefaultButton = buttonCollection(2)
            Case Else
                Exit Select
        End Select

        Select Case buttonStyle And &H15000
            Case 0
            Case MsgBoxStyle.SystemModal
                Exit Select
            Case MsgBoxStyle.MsgBoxHelp
                ' To be implemented
            Case MsgBoxStyle.MsgBoxSetForeground
                Exit Select
            Case Else
                Stop
        End Select
        page.Buttons = buttonCollection

        ' Display the form's icon in the task dialog.
        ' Note however that the task dialog will not scale the icon.

        ' Create a WinForms timer that raises the Tick event every tenth second.
        Using timer As New Timer() With {
            .Enabled = True,
            .Interval = 100
        }
            If autoCloseTimeOutSeconds > -1 Then
                AddHandler timer.Tick,
                    Sub(s, e)
                        remainingTenthSeconds -= 1
                        If remainingTenthSeconds > 0 Then
                            ' Update the remaining time and progress bar.
                            page.Text = GetPrompt(prompt, autoCloseTimeOutSeconds, remainingTenthSeconds)
                            Dim barPercent As Integer = 100 - (100 * (remainingTenthSeconds \ (autoCloseTimeOutSeconds * 10)))
                            If barPercent < 0 Then
                                barPercent = 0
                            End If
                            page.ProgressBar.Value = barPercent
                        Else
                            ' Stop the timer and click the "Reconnect" button - this will
                            ' close the dialog.
                            timer.Enabled = False
                            page.DefaultButton.PerformClick()
                        End If
                    End Sub
            End If

            Dim result As TaskDialogButton = TaskDialog.ShowDialog(Form1, page)
            Return CType([Enum].Parse(GetType(MsgBoxResult), result.ToString), MsgBoxResult)
        End Using
    End Function

End Module
