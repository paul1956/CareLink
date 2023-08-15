' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Enum UnicodeNewlines
    Unknown

    ''' <summary>
    ''' Line Feed, U+000A
    ''' </summary>
    Lf = &HA

    CrLf = &HD0A

    ''' <summary>
    ''' Carriage Return, U+000D
    ''' </summary>
    Cr = &HD

    ''' <summary>
    ''' Next Line, U+0085
    ''' </summary>
    Nel = &H85

    ''' <summary>
    ''' Vertical Tab, U+000B
    ''' </summary>
    Vt = &HB

    ''' <summary>
    ''' Form Feed, U+000C
    ''' </summary>
    Ff = &HC

    ''' <summary>
    ''' Line Separator, U+2028
    ''' </summary>
    Ls = &H2028

    ''' <summary>
    ''' Paragraph Separator, U+2029
    ''' </summary>
    Ps = &H2029

End Enum
