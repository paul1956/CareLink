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
    ''' <param name="curChar">The current character.</param>
    ''' <param name = "length">The length of the delimiter</param>
    ''' <param name = "type">The type of the delimiter</param>
    ''' <param name="nextChar">A callback getting the next character (may be null).</param>
    ''' <returns>
    '''  0 = no new line;
    '''  otherwise it returns either 1 or 2 depending on the length of the delimiter.
    ''' </returns>
    Private Function TryGetDelimiterInfo(curChar As Char,
                                        <Out()> ByRef length As Integer,
                                        <Out()> ByRef type As UnicodeNewlines,
                                        Optional nextChar As Func(Of Char) = Nothing) As Boolean

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
    <Extension>
    Public Function SplitLines(text As String,
                               Optional Trim As Boolean = False) As List(Of String)

        Dim result As New List(Of String)()
        If text Is Nothing Then
            Return result
        End If
        Dim sb As New StringBuilder()

        Dim length As Integer = Nothing
        Dim type As UnicodeNewlines = Nothing

        For index As Integer = 0 To text.Length - 1
            Dim curChar As Char = text.Chars(index)
            ' Do not delete the next line
            Dim j As Integer = index
            Dim nextChar As Func(Of Char) =
                Function() As Char
                    Return If(j < text.Length - 1,
                              text.Chars(index:=j + 1),
                              ControlChars.NullChar)
                End Function

            If TryGetDelimiterInfo(curChar, length, type, nextChar) Then
                If Trim Then
                    result.Add(item:=sb.ToString.Trim)
                Else
                    result.Add(item:=sb.ToString)
                End If
                sb.Length = 0
                index += length - 1
                Continue For
            End If
            sb.Append(value:=curChar)
        Next index
        If sb.Length > 0 Then
            If Trim Then
                result.Add(item:=sb.ToString.Trim)
            Else
                result.Add(item:=sb.ToString)
            End If
        End If

        Return result
    End Function

End Module
