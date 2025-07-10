' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module UnicodeConstants

    ''' <summary>
    '''  Carriage Return, U+000D
    ''' </summary>
    Public Const Cr As Char = ChrW(CharCode:=&HD)

    ''' <summary>
    '''  Form Feed, U+000C
    ''' </summary>
    Public Const Ff As Char = ChrW(CharCode:=&HC)

    ''' <summary>
    '''  Gear, U+2699
    '''  Represents a gear symbol, often used to indicate settings or configuration options.
    ''' </summary>
    Public Const Gear As String = ChrW(CharCode:=&H2699)

    ''' <summary>
    '''  Line Feed, U+000A
    ''' </summary>
    Public Const Lf As Char = ChrW(CharCode:=&HA)

    ''' <summary>
    '''  Line Separator, U+2028
    ''' </summary>
    Public Const Ls As Char = ChrW(CharCode:=&H2028)

    ''' <summary>
    '''  Next Line, U+0085
    ''' </summary>
    Public Const Nel As Char = ChrW(CharCode:=&H85)

    ''' <summary>
    '''  Non-Breaking Space, U+00A0
    ''' </summary>
    Public Const NonBreakingSpace As Char = ChrW(CharCode:=&HA0)

    ''' <summary>
    '''  Paragraph Separator, U+2029
    ''' </summary>
    Public Const Ps As Char = ChrW(CharCode:=&H2029)

    ''' <summary>
    '''  Registered Trademark, U+00AE
    ''' </summary>
    Public Const RegisteredTrademark As Char = ChrW(CharCode:=&HAE)

    ''' <summary>
    '''  Superscript1, U+B9
    '''  Represents the superscript one character.
    ''' </summary>
    Public Const Superscript1 As Char = ChrW(CharCode:=&HB9)

    ''' <summary>
    '''  Superscript2, U+B2
    '''  Represents the superscript two character, commonly used
    '''  in mathematical expressions or to denote squared measurements.
    ''' </summary>
    Public Const Superscript2 As Char = ChrW(&HB2)

    ''' <summary>
    '''  Superscript3, U+B3
    '''  Represents the superscript three character, commonly used
    '''  in mathematical expressions or to denote cubic measurements.
    ''' </summary>
    Public Const Superscript3 As Char = ChrW(CharCode:=&HB3)

    '''  Vertical Tab, U+000B
    ''' </summary>
    Public Const Vt As Char = ChrW(CharCode:=&HB)

End Module
