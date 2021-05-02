' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text

Public Enum UnicodeNewline
    Unknown

    ''' <summary>
    ''' Line Feed, U+000A
    ''' </summary>
    LF = &HA

    CRLF = &HD0A

    ''' <summary>
    ''' Carriage Return, U+000D
    ''' </summary>
    CR = &HD

    ''' <summary>
    ''' Next Line, U+0085
    ''' </summary>
    NEL = &H85

    ''' <summary>
    ''' Vertical Tab, U+000B
    ''' </summary>
    VT = &HB

    ''' <summary>
    ''' Form Feed, U+000C
    ''' </summary>
    FF = &HC

    ''' <summary>
    ''' Line Separator, U+2028
    ''' </summary>
    LS = &H2028

    ''' <summary>
    ''' Paragraph Separator, U+2029
    ''' </summary>
    PS = &H2029

End Enum

''' <summary>
''' Defines Unicode new lines according to Unicode Technical Report #13
''' http://www.unicode.org/standard/reports/tr13/tr13-5.html
''' </summary>

Public Module NewLine

    ''' <summary>
    ''' Carriage Return, U+000D
    ''' </summary>
    Public Const CR As Char = ChrW(&HD)

    ''' <summary>
    ''' Line Feed, U+000A
    ''' </summary>
    Public Const LF As Char = ChrW(&HA)

    ''' <summary>
    ''' Next Line, U+0085
    ''' </summary>
    Public Const NEL As Char = ChrW(&H85)

    ''' <summary>
    ''' Vertical Tab, U+000B
    ''' </summary>
    Public Const VT As Char = ChrW(&HB)

    ''' <summary>
    ''' Form Feed, U+000C
    ''' </summary>
    Public Const FF As Char = ChrW(&HC)

    ''' <summary>
    ''' Line Separator, U+2028
    ''' </summary>
    Public Const LS As Char = ChrW(&H2028)

    ''' <summary>
    ''' Paragraph Separator, U+2029
    ''' </summary>
    Public Const PS As Char = ChrW(&H2029)

    ''' <summary>
    ''' Determines if a char is a new line delimiter.
    ''' </summary>
    ''' <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
    ''' <param name="curChar">The current character.</param>
    ''' <param name="nextChar">The next character (if != LF then length will always be 0 or 1).</param>
    Public Function GetDelimiterLength(curChar As Char, nextChar As Char) As Integer
        If curChar = CR Then
            If nextChar = LF Then
                Return 2
            End If
            Return 1
        End If

        If curChar = LF OrElse curChar = NEL OrElse curChar = VT OrElse curChar = FF OrElse curChar = LS OrElse curChar = PS Then
            Return 1
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Determines if a char is a new line delimiter.
    ''' </summary>
    ''' <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
    ''' <param name="curChar">The current character.</param>
    ''' <param name = "length">The length of the delimiter</param>
    ''' <param name = "type">The type of the delimiter</param>
    ''' <param name="nextChar">A callback getting the next character (may be null).</param>
    Friend Function TryGetDelimiterLengthAndType(curChar As Char, <Out()> ByRef length As Integer, <Out()> ByRef type As UnicodeNewline, Optional nextChar As Func(Of Char) = Nothing) As Boolean
        If curChar = CR Then
            If nextChar IsNot Nothing AndAlso nextChar() = LF Then
                length = 2
                type = UnicodeNewline.CRLF
            Else
                length = 1
                type = UnicodeNewline.CR

            End If
            Return True
        End If

        Select Case curChar
            Case LF
                type = UnicodeNewline.LF
                length = 1
                Return True
            Case NEL
                type = UnicodeNewline.NEL
                length = 1
                Return True
            Case VT
                type = UnicodeNewline.VT
                length = 1
                Return True
            Case FF
                type = UnicodeNewline.FF
                length = 1
                Return True
            Case LS
                type = UnicodeNewline.LS
                length = 1
                Return True
            Case PS
                type = UnicodeNewline.PS
                length = 1
                Return True
        End Select
        length = -1
        type = UnicodeNewline.Unknown
        Return False
    End Function

    ''' <summary>
    ''' Determines if a string is a new line delimiter.
    '''
    ''' Note that the only 2 char wide new line is CR LF
    ''' </summary>
    <Extension>
    Friend Function IsNewLine(str As String) As Boolean
        If String.IsNullOrEmpty(str) Then
            Return False
        End If
        Dim ch As Char = str.Chars(0)
        Select Case str.Length
            Case 0
                Return False
            Case 1, 2
                Return ch = CR OrElse ch = LF OrElse ch = NEL OrElse ch = VT OrElse ch = FF OrElse ch = LS OrElse ch = PS
            Case Else
                Return False
        End Select
    End Function

    <Extension>
    Public Function SplitLines(text As String) As String()
        Dim result As New List(Of String)()
        If text Is Nothing Then
            Return result.ToArray
        End If
        Dim sb As New StringBuilder()

        Dim length As Integer = Nothing
        Dim type As UnicodeNewline = Nothing

        For index As Integer = 0 To text.Length - 1
            Dim ch As Char = text.Chars(index)
            ' Do not delete the next line
            Dim j As Integer = index
            If TryGetDelimiterLengthAndType(ch, length, type, Function() If(j < text.Length - 1, text.Chars(j + 1), ControlChars.NullChar)) Then
                result.Add(sb.ToString)
                sb.Length = 0
                ' ReSharper disable once RedundantAssignment
                index += length - 1
                Continue For
            End If
            sb.Append(ch)
        Next index
        If sb.Length > 0 Then
            result.Add(sb.ToString)
        End If

        Return result.ToArray
    End Function

    <Extension>
    Friend Function JoinLines(lines As String(), delimiter As String) As String
        Return String.Join(separator:=delimiter, lines)
    End Function

    <Extension>
    Friend Function NormalizeLineEndings(lines As String, Optional delimiter As String = vbCrLf) As String
        Return lines.SplitLines.JoinLines(delimiter)
    End Function

    ''' <summary>
    ''' Replace Unicode NewLines with ControlChars.NullChar or Specified Character
    ''' </summary>
    ''' <param name="text">Source Test</param>
    ''' <param name="substituteChar">Default is vbNullChar</param>
    ''' <returns>String with Unicode NewLines replaced with SubstituteChar</returns>
    <Extension>
    Public Function WithoutNewLines(text As String, Optional substituteChar As Char = ControlChars.NullChar) As String
        If text Is Nothing Then
            Throw New ArgumentNullException(NameOf(text))
        End If

        Dim sb As New StringBuilder()
        Dim length As Integer = Nothing
        Dim type As UnicodeNewline = Nothing

        For index As Integer = 0 To text.Length - 1
            Dim ch As Char = text.Chars(index)
            ' Do not delete the next line
            Dim j As Integer = index
            If TryGetDelimiterLengthAndType(ch, length, type, Function() If(j < text.Length - 1, text.Chars(j + 1), substituteChar)) Then
                ' ReSharper disable once RedundantAssignment
                index += length - 1
                Continue For
            End If
            sb.Append(ch)
        Next index
        Return sb.ToString
    End Function

End Module
