' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module UnicodeConstants

    ''' <summary>
    '''  Carriage Return, U+000D
    ''' </summary>
    Public Const Cr As Char = ChrW(&HD)

    ''' <summary>
    '''  Form Feed, U+000C
    ''' </summary>
    Public Const Ff As Char = ChrW(&HC)

    ''' <summary>
    '''  Gear, U+2699
    '''  Represents a gear symbol, often used to indicate settings or configuration options.
    ''' </summary>
    Public Const Gear As String = ChrW(CharCode:=&H2699)

    ''' <summary>
    '''  Line Feed, U+000A
    ''' </summary>
    Public Const Lf As Char = ChrW(&HA)

    ''' <summary>
    '''  Line Separator, U+2028
    ''' </summary>
    Public Const Ls As Char = ChrW(&H2028)

    ''' <summary>
    '''  Next Line, U+0085
    ''' </summary>
    Public Const Nel As Char = ChrW(&H85)

    ''' <summary>
    '''  Paragraph Separator, U+2029
    ''' </summary>
    Public Const Ps As Char = ChrW(&H2029)

    ''' <summary>
    '''  Vertical Tab, U+000B
    ''' </summary>
    Public Const Vt As Char = ChrW(&HB)

End Module
