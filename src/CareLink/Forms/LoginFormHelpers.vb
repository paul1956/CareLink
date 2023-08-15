' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module LoginFormHelpers

    Friend Sub ReportLoginStatus(loginStatus As ToolStripStatusLabel)
        ReportLoginStatus(loginStatus, True, "Login Status: No Internet Connection!")
    End Sub

    Friend Sub ReportLoginStatus(loginStatus As ToolStripStatusLabel, hasErrors As Boolean, Optional lastErrorMessage As String = "")
        If hasErrors Then
            loginStatus.ForeColor = If(lastErrorMessage = "OK", Color.Black, Color.Red)
            loginStatus.Text = $"Login Status: {lastErrorMessage}"
        Else
            loginStatus.ForeColor = Color.Black
            loginStatus.Text = "Login Status: OK"
        End If
    End Sub

End Module
