' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module PositionHelpers
    ''' Centers a form relative to another form, even if it's not the parent.
    ''' </summary>
    <Extension>
    Friend Sub CenterFormOnAnother(child As Form, reference As Form)
        If child Is Nothing OrElse reference Is Nothing Then Exit Sub

        ' Calculate centered position
        Dim x As Integer = reference.Left + ((reference.Width - child.Width) \ 2)
        Dim y As Integer = reference.Top + ((reference.Height - child.Height) \ 2)

        ' Ensure the form stays fully visible on screen
        Dim screenBounds As Rectangle = Screen.FromControl(reference).WorkingArea
        x = Math.Max(screenBounds.Left, Math.Min(x, screenBounds.Right - child.Width))
        y = Math.Max(screenBounds.Top, Math.Min(y, screenBounds.Bottom - child.Height))

        child.StartPosition = FormStartPosition.Manual
        child.Location = New Point(x, y)
    End Sub
End Module
