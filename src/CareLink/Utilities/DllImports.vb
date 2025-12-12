' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.InteropServices
Imports System.Text

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
    '''  Message to format a range of text in a RichTextBox control.
    '''  This is used to apply formatting to a specified range of text.
    ''' </summary>
    Friend Const EM_FORMATRANGE As Integer = WM_USER + 57

    ''' <summary>
    '''  Base message for user-defined messages.
    '''  This is used as a starting point for defining custom messages.
    ''' </summary>
    Friend Const WM_USER As Integer = &H400

    ''' <summary>
    '''  Sets a window attribute for the specified window handle.
    '''  This function is used to set various attributes of a window, such as border color.
    ''' </summary>
    ''' <param name="hwnd">A handle to the window.</param>
    ''' <param name="attr">The attribute to set.</param>
    ''' <param name="attrValue">The value of the attribute.</param>
    ''' <param name="attrSize">The size of the attribute value.</param>
    ''' <returns>An integer indicating success or failure.</returns>
    <DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Friend Function DwmSetWindowAttribute(
        hwnd As IntPtr,
        attr As Integer,
        ByRef attrValue As Integer,
        attrSize As Integer) As Integer
    End Function

    ''' <summary>
    '''  Retrieves the class name of the specified window.
    ''' </summary>
    ''' <param name="hWnd">A handle to the window.</param>
    ''' <param name="lpClassName">A StringBuilder to receive the class name.</param>
    ''' <param name="nMaxCount">The maximum number of characters to copy to the StringBuilder.</param>
    ''' <returns>The length of the class name string, in characters.</returns>
    <DllImport("user32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Friend Function GetClassName(
        hWnd As IntPtr,
        <Out(), MarshalAs(UnmanagedType.LPWStr)> lpClassName As StringBuilder,
        nMaxCount As Integer) As Integer
    End Function

    ''' <summary>
    '''  Forwards keyboard messages to the child <see cref="TextBox"/> control.
    ''' </summary>
    ''' <param name="hWnd">Handle to the window receiving the message.</param>
    ''' <param name="msg">Message ID.</param>
    ''' <param name="wParam">First message parameter.</param>
    ''' <param name="lParam">Second message parameter.</param>
    ''' <returns>Result of the message processing.</returns>
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Function SendMessage(
        hWnd As IntPtr,
        msg As Integer,
        wParam As IntPtr,
        lParam As IntPtr) As IntPtr
    End Function

    ''' <summary>
    '''  Structure used to define the range of text to be formatted in a RichTextBox control.
    ''' </summary>
    ''' <param name="hWnd">Handle to the window receiving the message.</param>
    ''' <param name="msg">Message ID.</param>
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Function SendMessage(
        hWnd As IntPtr,
        msg As Integer,
        wParam As IntPtr,
        ByRef lParam As STRUCT_FORMATRANGE) As IntPtr
    End Function

    ''' <summary>
    '''  Retrieves the current state of the specified key.
    ''' </summary>
    ''' <param name="key">The virtual-key code of the key to be checked.</param>
    ''' <returns>The state of the key.</returns>
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Function VkKeyScan(key As Char) As Short
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure STRUCT_CHARRANGE
        Public cpMin As Integer
        Public cpMax As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure STRUCT_FORMATRANGE
        Public hdc As IntPtr
        Public hdcTarget As IntPtr
        Public rc As STRUCT_RECT
        Public rcPage As STRUCT_RECT
        Public chrg As STRUCT_CHARRANGE
    End Structure

    ' Windows API for printing RichTextBox content with formatting
    <StructLayout(LayoutKind.Sequential)>
    Friend Structure STRUCT_RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure
End Module
