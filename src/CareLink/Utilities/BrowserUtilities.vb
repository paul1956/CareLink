' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides utility methods for launching the user's default web browser.
''' </summary>
Friend Module BrowserUtilities

    ''' <summary>
    '''  Launches the specified URL in the user's default web browser.
    ''' </summary>
    ''' <param name="url">The web address (URL) to open.</param>
    Private Sub LaunchBrowser(url As String)
        Process.Start(startInfo:=New ProcessStartInfo(fileName:=url) With {.UseShellExecute = True})
    End Sub

    ''' <summary>
    '''  Opens the specified web address in the user's default browser,
    '''  updating the main form's cursor during the operation.
    ''' </summary>
    ''' <param name="url">The web address (URL) to open.</param>
    ''' <remarks>
    '''  This method sets the cursor to AppStarting while attempting to open the browser,
    '''  and resets it afterward.
    ''' </remarks>
    Friend Sub OpenUrlInBrowser(url As String)
        Dim mainForm As Form1 = My.Forms.Form1
        Try
            mainForm.Cursor = Cursors.AppStarting
            Application.DoEvents()
            LaunchBrowser(url)
        Finally
            mainForm.Cursor = Cursors.Default
            Application.DoEvents()
        End Try
    End Sub

End Module
