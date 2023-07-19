' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TaskDialogHelpers

    Public Function ShowAutoClosingTaskDialog(parentForm As Form, prompt As String, title As String) As TaskDialogButton
        Const textFormat As String = "Closing in {0} seconds..."
        Dim remainingTenthSeconds As Integer = 50

        Dim closeButton As New TaskDialogButton("&Close now")
        Dim cancelButton As TaskDialogButton = TaskDialogButton.Cancel

        ' Display the form's icon in the task dialog.
        ' Note however that the task dialog will not scale the icon.
        Dim page As New TaskDialogPage() With
        {
            .Heading = title,
            .Text = String.Format($"{prompt}{vbCrLf}{textFormat}", (remainingTenthSeconds + 9) \ 10),
            .Icon = New TaskDialogIcon(parentForm.Icon),
            .ProgressBar = New TaskDialogProgressBar() With
            {
                .State = TaskDialogProgressBarState.Paused
            },
            .Buttons = New TaskDialogButtonCollection() From
            {
                closeButton,
                cancelButton
            }
        }

        ' Create a WinForms timer that raises the Tick event every tenth second.
        Using timer As New Timer() With {
            .Enabled = True,
            .Interval = 100
        }
            AddHandler timer.Tick,
                Sub(s, e)
                    remainingTenthSeconds -= 1
                    If remainingTenthSeconds > 0 Then
                        ' Update the remaining time and progress bar.
                        page.Text = String.Format($"{prompt}{vbCrLf}{textFormat}", (remainingTenthSeconds + 9) \ 10)
                        page.ProgressBar.Value = 100 - (remainingTenthSeconds * 2)
                    Else
                        ' Stop the timer and click the "Reconnect" button - this will
                        ' close the dialog.
                        timer.Enabled = False
                        closeButton.PerformClick()
                    End If
                End Sub

            Dim result As TaskDialogButton = TaskDialog.ShowDialog(parentForm, page)
            If result = closeButton Then
                Console.WriteLine("Reconnecting.")
            Else
                Console.WriteLine("Not reconnecting.")
            End If
            Return result
        End Using
    End Function

End Module
