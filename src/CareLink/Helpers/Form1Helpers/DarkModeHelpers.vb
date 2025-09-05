' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.InteropServices

Public Module DarkModeHelpers

    ''' <summary>
    '''  Enables dark mode for the form and its controls.
    '''  Sets the window attributes for immersive dark mode and border color.
    ''' </summary>
    ''' <remarks>
    '''  This method uses the DWM API to enable dark mode and set the border color.
    ''' </remarks>
    Public Sub EnableDarkMode(hwnd As IntPtr)
        ' Enable immersive dark mode
        Dim useDarkMode As Integer = 1
        Dim result As Integer = DwmSetWindowAttribute(
            hwnd,
            attr:=DWMWA_USE_IMMERSIVE_DARK_MODE,
            attrValue:=useDarkMode,
            attrSize:=Marshal.SizeOf([structure]:=useDarkMode))

        If result <> 0 Then
            ' Handle error if dark mode could not be enabled
            MessageBox.Show(
                text:="Failed to enable dark mode.",
                caption:="Error",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Error)
            Return
        End If
        ' Set border color (BGR format, e.g., &H202020 for dark gray)
        Dim borderColor As Integer = &H202020
        result = DwmSetWindowAttribute(
            hwnd,
            attr:=DWMWA_BORDER_COLOR,
            attrValue:=borderColor,
            attrSize:=Marshal.SizeOf([structure]:=borderColor))
        If result <> 0 Then
            MessageBox.Show(
                text:="Failed to set border color.",
                caption:="Error",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Error)
        End If
    End Sub

End Module
