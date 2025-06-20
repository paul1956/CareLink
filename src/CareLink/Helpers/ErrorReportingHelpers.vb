' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides helper methods for error reporting and login status updates.
''' </summary>
Friend Module ErrorReportingHelpers

    ''' <summary>
    '''  Reports a default login status indicating no internet connection.
    ''' </summary>
    ''' <param name="loginStatus">The <see cref="ToolStripStatusLabel"/> to update.</param>
    Friend Sub ReportLoginStatus(loginStatus As ToolStripStatusLabel)
        ReportLoginStatus(loginStatus, hasErrors:=True, lastErrorMessage:="Login Status: No Internet Connection!")
    End Sub

    ''' <summary>
    '''  Reports the login status with error handling and custom message.
    ''' </summary>
    ''' <param name="loginStatus">The <see cref="ToolStripStatusLabel"/> to update.</param>
    ''' <param name="hasErrors">Indicates whether there are errors.</param>
    ''' <param name="lastErrorMessage">The last error message to display. Defaults to empty string.</param>
    Friend Sub ReportLoginStatus(loginStatus As ToolStripStatusLabel, hasErrors As Boolean, Optional lastErrorMessage As String = "")
        If hasErrors Then
            loginStatus.ForeColor = If(lastErrorMessage = "OK", Form1.MenuStrip1.ForeColor, Color.Red)
            loginStatus.Text = $"Login Status: {lastErrorMessage}"
        Else
            loginStatus.ForeColor = Form1.MenuStrip1.ForeColor
            loginStatus.Text = "Login Status: OK"
        End If
    End Sub

    ''' <summary>
    '''  Gets a formatted title string from a stack frame, indicating the file and line number.
    ''' </summary>
    ''' <param name="stackFrame">The <see cref="StackFrame"/> to extract information from.</param>
    ''' <returns>
    '''  A string containing the file name and line number, or a default message if unavailable.
    ''' </returns>
    Public Function GetTitleFromStack(stackFrame As StackFrame) As String
        Return If(stackFrame?.GetFileName Is Nothing,
                  "Error Location External Code",
                  $"{stackFrame.GetFileName.Split("\").Last} line:{stackFrame.GetFileLineNumber()}"
                 )
    End Function

End Module
