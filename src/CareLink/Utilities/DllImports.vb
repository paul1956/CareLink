' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.InteropServices

Friend Module DllImports

    ' <summary>
    '''  Specifies the border color for a window.
    '''  This is used to set the color of the window border in Windows.
    ''' </summary>
    ''' <remarks>
    '''  Requires Windows 10, version 1607 (build 14393) or later.
    ''' </remarks>
    Friend Const DWMWA_BORDER_COLOR As Integer = 34

    ''' <summary>
    '''  Specifies the use of an immersive dark mode for the window.
    '''  This is used to enable dark mode in applications on Windows 10 and later.
    ''' </summary>
    ''' <remarks>
    '''  Requires Windows 10, version 1809 (build 17763) or later.
    ''' </remarks>
    Friend Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20

    ''' <summary>
    '''  Sets a window attribute for the specified window handle.
    '''  This function is used to set various attributes of a window, such as border color.
    ''' </summary>
    ''' <param name="hwnd">A handle to the window.</param>
    ''' <param name="attr">The attribute to set.</param>
    ''' <param name="attrValue">The value of the attribute.</param>
    ''' <param name="attrSize">The size of the attribute value.</param>
    ''' <returns>An integer indicating success or failure.</returns>
    <DllImport("dwmapi.dll")>
    Friend Function DwmSetWindowAttribute(
        ByVal hwnd As IntPtr,
        ByVal attr As Integer,
        ByRef attrValue As Integer,
        ByVal attrSize As Integer
    ) As Integer
    End Function

    ''' <summary>
    '''  Retrieves the current state of the specified key.
    ''' </summary>
    ''' <param name="key">The virtual-key code of the key to be checked.</param>
    ''' <returns>The state of the key.</returns>
    <DllImport("USER32.DLL", CharSet:=CharSet.Auto)>
    Friend Function VkKeyScan(key As Char) As Short
    End Function

End Module
