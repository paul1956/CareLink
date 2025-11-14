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
    Public Const Gear As String = "⚙️"

    ''' <summary>
    '''  Shield, U+26E8
    '''  Represents a shield symbol, typically used to indicate protection, security, or safety.
    ''' </summary>
    Public Const Shield As String = "⛨"

    ''' <summary>
    '''  Speaker, U+1F50A
    '''  Represents a speaker symbol, often used to indicate audio, volume, or sound output.
    ''' </summary>
    Public Const Speaker As String = "🔊"

    ''' <summary>
    '''  Constant representing an empty string literal for better code readability and maintainability
    ''' </summary>
    Public Const EmptyString As String = ""

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

    Public Const Quote As String = """"

    ''' <summary>
    '''  Registered trademark, U+00AE
    '''  Represents the registered trademark symbol.
    ''' </summary>
    Public Const RegisteredTrademark As Char = "®"c

    ''' <summary>
    '''  Superscript one, U+00B9
    '''  Represents the superscript 1 character.
    ''' </summary>
    Public Const Superscript1 As Char = "¹"c

    ''' <summary>
    '''  Superscript two, U+00B2
    '''  Represents the superscript 2 character.
    ''' </summary>
    Public Const Superscript2 As Char = "²"c

    ''' <summary>
    '''  Superscript three, U+00B3
    '''  Represents the superscript 3 character.
    ''' </summary>
    Public Const Superscript3 As Char = "³"c

    '''  Vertical Tab, U+000B
    ''' </summary>
    Public Const Vt As Char = ChrW(CharCode:=&HB)

End Module
