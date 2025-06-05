' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text

Public Module UnicodeNewline

    ''' <summary>
    '''  Determines if a char is a new line delimiter.
    ''' </summary>
    ''' <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
    ''' <param name="curChar">The current character.</param>
    ''' <param name="nextChar">The next character (if != LF then length will always be 0 or 1).</param>
    Public Function GetDelimiterLength(curChar As Char, nextChar As Char) As Integer
        Return If(curChar = Cr,
                  If(nextChar = Lf, 2, 1),
                  If(curChar = Lf OrElse curChar = Nel OrElse curChar = Vt OrElse curChar = Ff OrElse curChar = Ls OrElse curChar = Ps,
                     1,
                     0
                    )
                 )
    End Function

    ''' <summary>
    '''  Determines if a char is a new line delimiter.
    ''' </summary>
    ''' <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
    ''' <param name="curChar">The current character.</param>
    ''' <param name = "length">The length of the delimiter</param>
    ''' <param name = "type">The type of the delimiter</param>
    ''' <param name="nextChar">A callback getting the next character (may be null).</param>
    Friend Function TryGetDelimiterLengthAndType(curChar As Char, <Out()> ByRef length As Integer, <Out()> ByRef type As UnicodeNewlines, Optional nextChar As Func(Of Char) = Nothing) As Boolean
        If curChar = Cr Then
            If nextChar IsNot Nothing AndAlso nextChar() = Lf Then
                length = 2
                type = UnicodeNewlines.CrLf
            Else
                length = 1
                type = UnicodeNewlines.Cr

            End If
            Return True
        End If

        Select Case curChar
            Case Lf
                type = UnicodeNewlines.Lf
                length = 1
                Return True
            Case Nel
                type = UnicodeNewlines.Nel
                length = 1
                Return True
            Case Vt
                type = UnicodeNewlines.Vt
                length = 1
                Return True
            Case Ff
                type = UnicodeNewlines.Ff
                length = 1
                Return True
            Case Ls
                type = UnicodeNewlines.Ls
                length = 1
                Return True
            Case Ps
                type = UnicodeNewlines.Ps
                length = 1
                Return True
        End Select
        length = -1
        type = UnicodeNewlines.Unknown
        Return False
    End Function

    ''' <summary>
    '''  Determines if a string is a new line delimiter.
    ''' </summary>
    ''' <remarks>
    '''  Note that the only 2 character wide new line is CR LF
    ''' </remarks>
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
                Return ch = Cr OrElse ch = Lf OrElse ch = Nel OrElse ch = Vt OrElse ch = Ff OrElse ch = Ls OrElse ch = Ps
            Case Else
                Return False
        End Select
    End Function

    ''' <summary>
    '''  Determines if a string is a new line delimiter.
    ''' </summary>
    <Extension>
    Public Function SplitLines(text As String, Optional Trim As Boolean = False) As List(Of String)
        Dim result As New List(Of String)()
        If text Is Nothing Then
            Return result
        End If
        Dim sb As New StringBuilder()

        Dim length As Integer = Nothing
        Dim type As UnicodeNewlines = Nothing

        For index As Integer = 0 To text.Length - 1
            Dim ch As Char = text.Chars(index)
            ' Do not delete the next line
            Dim j As Integer = index
            If TryGetDelimiterLengthAndType(ch, length, type, Function() If(j < text.Length - 1, text.Chars(j + 1), ControlChars.NullChar)) Then
                If Trim Then
                    result.Add(sb.ToString.Trim)
                Else
                    result.Add(sb.ToString)
                End If
                sb.Length = 0
                index += length - 1
                Continue For
            End If
            sb.Append(ch)
        Next index
        If sb.Length > 0 Then
            If Trim Then
                result.Add(sb.ToString.Trim)
            Else
                result.Add(sb.ToString)
            End If
        End If

        Return result
    End Function

    ''' <Summary>
    '''  Joins an array of strings into a single string with the specified delimiter.
    ''' </summary>
    ''' <param name="lines">Array of strings to join.</param>
    ''' <param name="delimiter">Delimiter to use between each string.</param>
    ''' <returns>A single string with all elements joined by the specified delimiter.</returns>
    <Extension>
    Friend Function JoinLines(lines As String(), delimiter As String) As String
        Return String.Join(separator:=delimiter, lines)
    End Function

    ''' <Summary>
    '''  Normalizes line endings in a string to a specified delimiter (default is vbCrLf).
    ''' </Summary>
    ''' <param name="lines">String containing lines to normalize.</param>
    ''' <param name="delimiter">Delimiter to use for normalization (default is vbCrLf).</param>
    ''' <returns>A string with normalized line endings.</returns>
    <Extension>
    Friend Function NormalizeLineEndings(lines As String, Optional delimiter As String = vbCrLf) As String
        Return lines.SplitLines.ToArray.JoinLines(delimiter)
    End Function

    ''' <summary>
    '''  Replace Unicode NewLines with ControlChars.NullChar or Specified Character
    ''' </summary>
    ''' <param name="text">Source Text</param>
    ''' <param name="substituteChar">Default is vbNullChar</param>
    ''' <returns>String with Unicode NewLines replaced with SubstituteChar</returns>
    <Extension>
    Public Function WithoutNewLines(text As String, Optional substituteChar As Char = ControlChars.NullChar) As String
        ArgumentNullException.ThrowIfNull(text)

        Dim sb As New StringBuilder()
        Dim length As Integer = Nothing
        Dim type As UnicodeNewlines = Nothing

        For index As Integer = 0 To text.Length - 1
            Dim ch As Char = text.Chars(index)
            ' Do not delete the next line
            Dim j As Integer = index
            If TryGetDelimiterLengthAndType(ch, length, type, Function() If(j < text.Length - 1, text.Chars(j + 1), substituteChar)) Then
                index += length - 1
                Continue For
            End If
            sb.Append(ch)
        Next index
        Return sb.ToString
    End Function

End Module
