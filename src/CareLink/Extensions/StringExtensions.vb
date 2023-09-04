' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module StringExtensions

    Private ReadOnly s_commaOrPeriod As Char() = {"."c, ","c}

    <Extension>
    Public Function CleanSpaces(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return ""
        Return value.Replace("  ", " ").Replace("  ", " ").Trim
    End Function

    <Extension()>
    Friend Function Count(s As String, c As Char) As Integer
        Return s.Count(Function(c1 As Char) c1 = c)
    End Function

    ''' <summary>
    ''' Converts a string of words separated by a space or underscore
    ''' to a Title where the first letter of every word capitalized and the rest are not
    ''' </summary>
    ''' <param name="inStr">A string like THIS_IS A TITLE</param>
    ''' <returns>This Is A Title</returns>
    <Extension()>
    Friend Function ToTitle(inStr As String) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If

        Dim result As New StringBuilder(Char.ToUpperInvariant(inStr(0)))
        Dim firstLetterOfWord As Boolean = False
        For Each c As Char In inStr.Substring(1)
            If c = " "c Or c = "_"c Then
                firstLetterOfWord = True
                result.Append(" "c)
            ElseIf firstLetterOfWord Then
                result.Append(Char.ToUpperInvariant(c))
                firstLetterOfWord = False
            Else
                result.Append(Char.ToLowerInvariant(c))
            End If
        Next
        Return result.ToString.Replace("Bg ", "BG ").Replace("Sg ", "SG ")
    End Function

    ''' <summary>
    ''' Converts a single string of characters that contains words that
    ''' start with an uppercase character without spaces
    ''' to a Title where the first letter of every word capitalized and
    ''' words are separated by spaces
    ''' </summary>
    ''' <param name="inStr">A string like ThisIsATitle</param>
    ''' <returns>This Is A Title</returns>
    <Extension()>
    Friend Function ToTitleCase(inStr As String, Optional separateNumbers As Boolean = True) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If

        Dim result As New StringBuilder(Char.ToUpperInvariant(inStr(0)))
        Dim lastWasNumeric As Boolean = Char.IsNumber(inStr(0))
        For Each c As Char In inStr.Substring(1)
            If Char.IsLower(c) OrElse lastWasNumeric Then
                result.Append(c)
                lastWasNumeric = False
            ElseIf Char.IsNumber(c) AndAlso Not separateNumbers Then
                result.Append(c)
                lastWasNumeric = True
            Else
                result.Append($" {Char.ToUpperInvariant(c)}")
                lastWasNumeric = False
            End If
        Next
        Dim resultString As String = result.Replace("Care Link", "CareLink").ToString
        If Not resultString.Contains("™"c) Then
            resultString = resultString.Replace("CareLink", "CareLink™")
        End If
        resultString = resultString.Replace("S G", "SG", StringComparison.InvariantCulture)
        Return resultString.Replace("time", " Time", False, CurrentUICulture)
    End Function

    ''' <summary>
    ''' Escape characters not allowed in CSV data
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns>Translates String</returns>
    <Extension>
    Public Function CleanCsvString(value As String) As String
        Return $"{value?.ToString _
                        .Replace("""", """""") _
                        .Replace(",", "\,") _
                        .Replace(vbCrLf, $"\{vbCrLf}") _
                        .Replace("\", "\\")},"
    End Function

    ''' <summary>
    ''' Convert all multiple spaces with one space
    ''' </summary>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    <Extension>
    Public Function RemoveExtraSpaces(Value As String) As String
        Dim sbOut As New StringBuilder()
        If Not String.IsNullOrEmpty(Value) Then
            Dim isWhiteSpace As Boolean = False
            For i As Integer = 0 To Value.Length - 1
                If Char.IsWhiteSpace(Value.Chars(i)) Then 'Comparison with WhiteSpace
                    If Not isWhiteSpace Then 'Comparison with previous Char
                        sbOut.Append(Value.Chars(i))
                        isWhiteSpace = True
                    End If
                Else
                    isWhiteSpace = False
                    sbOut.Append(Value.Chars(i))
                End If
            Next i
        End If
        Return sbOut.ToString()
    End Function

    <Extension>
    Public Function TruncateSingleString(s As String, decimalDigits As Integer) As String
        Dim i As Integer = s.IndexOfAny(s_commaOrPeriod)
        If i < 0 Then
            If Not IsNumeric(s) Then
                Return s
            End If
            i = s.Length
            s &= CurrentUICulture.NumberFormat.NumberDecimalSeparator
        End If
        s &= New String("0"c, decimalDigits + 1)
        Return s.Substring(0, i + decimalDigits + 1)
    End Function

End Module
