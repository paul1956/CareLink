' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>
'''  Provides extension methods for <see langword="String"/> manipulation and conversion.
''' </summary>
Public Module StringExtensions

    ''' <summary>
    '''  Characters used as decimal separators for parsing numbers.
    ''' </summary>
    Private ReadOnly s_commaOrPeriod As Char() = {CareLinkDecimalSeparator, ","c}

    ''' <summary>
    '''  Replace multiple spaces with 1 and trim the ends
    ''' </summary>
    ''' <param name="value">The input string to clean.</param>
    ''' <returns>A <see langword="String"/> with multiple spaces replaced by a single space and trimmed.</returns>
    <Extension>
    Public Function CleanSpaces(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return ""
        Return Regex.Replace(value, "\s+", " ").Trim
    End Function

    ''' <summary>
    '''  Counts the number of times a specific character <paramref name="c"/> appears in a string.
    ''' </summary>
    ''' <param name="s">The string to search.</param>
    ''' <param name="c">The character to count.</param>
    ''' <returns>The number of occurrences of the character <paramref name="c"/> in the string.</returns>
    <Extension()>
    Public Function Count(s As String, c As Char) As Integer
        Return s.Count(Function(c1 As Char) c1 = c)
    End Function

    ''' <summary>
    '''  Find the index of the first occurrence of any character in the <paramref name="chars"/> list
    ''' </summary>
    ''' <param name="inputString">The string to search.</param>
    ''' <param name="chars">The list of characters to find.</param>
    ''' <param name="startIndex">The index to start searching from.</param>
    ''' <returns>The index of the first occurrence of any character in the list, or -1 if not found.</returns>
    ''' <exception cref="ArgumentException">Thrown if input parameters are invalid.</exception>
    <Extension>
    Public Function FindIndexOfAnyChar(inputString As String, chars As List(Of Char), startIndex As Integer) As Integer
        If inputString Is Nothing OrElse chars Is Nothing OrElse startIndex < 0 OrElse startIndex >= inputString.Length Then
            Throw New ArgumentException("Invalid input parameters.")
        End If

        For i As Integer = startIndex To inputString.Length - 1
            If chars.Contains(inputString(i)) Then
                Return i
            End If
        Next
        Return -1 ' Return -1 if no character is found
    End Function

    ''' <summary>
    '''  Converts a <see langword="String"/> to a <see langword="Double"/> using <see cref="CultureInfo.InvariantCulture"/>.
    ''' </summary>
    ''' <param name="value">The string to convert to a <see langword="Double"/>.</param>
    ''' <returns>The converted <see langword="Double"/> value.</returns>
    ''' <exception cref="FormatException">Thrown if the string is not a valid double.</exception>
    <Extension>
    Public Function ParseDoubleInvariant(value As String) As Double
        Return Double.Parse(value.Replace(",", CareLinkDecimalSeparator), CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    '''  Converts a <see langword="String"/> to a <see langword="Single"/> using <see cref="CultureInfo.InvariantCulture"/>.
    ''' </summary>
    ''' <param name="value">The string to convert to a Single.</param>
    ''' <returns>The converted <see langword="Single"/> value.</returns>
    ''' <exception cref="FormatException">Thrown if the string is not a valid single.</exception>
    <Extension>
    Public Function ParseSingleInvariant(value As String) As Single
        Return Single.Parse(value.Replace(",", CareLinkDecimalSeparator), CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    '''  Converts a string where the first letter of the string is not capitalized
    ''' </summary>
    ''' <param name="inStr">A <see langword="String"/> like THIS_IS A TITLE</param>
    ''' <returns>A <see langword="String"/> where the first character is lower case</returns>
    ''' <remarks>Used for converting strings that are not capitalized at the start</remarks>
    ''' <example>doNotCapitalizedFirstLetterString</example>
    <Extension()>
    Public Function ToLowerCamelCase(inStr As String) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If

        Dim result As New StringBuilder(Char.ToLowerInvariant(inStr(0)))
        If inStr.Length > 1 Then
            result.Append(inStr.AsSpan(1))
        End If
        Return result.ToString
    End Function

    ''' <summary>
    '''  Converts a string of words separated by a space or underscore to a title case string,
    '''  where the first letter of every word is capitalized and the rest are lower case.
    ''' </summary>
    ''' <param name="inStr">A string like "THIS_IS A TITLE".</param>
    ''' <param name="separateNumbers">If true, separates numbers into their own words.</param>
    ''' <returns>A title-cased string.</returns>
    <Extension()>
    Public Function ToTitle(inStr As String, Optional separateNumbers As Boolean = False) As String
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
                firstLetterOfWord = False
                result.Append(Char.ToUpperInvariant(c))
            ElseIf separateNumbers AndAlso IsNumeric(c) Then
                firstLetterOfWord = True
                result.Append(" "c)
                result.Append(Char.ToLowerInvariant(c))
            Else
                result.Append(Char.ToLowerInvariant(c))
            End If
        Next
        Return result.ToString.Replace("Bg ", "BG ").Replace("Sg ", "SG ")
    End Function

    ''' <summary>
    '''  Converts a string of concatenated words (PascalCase or camelCase) to a title case string,
    '''  where the first letter of every word is capitalized and words are separated by spaces.
    ''' </summary>
    ''' <param name="inStr">A string like "ThisIsATitle".</param>
    ''' <param name="separateNumbers">If true, separates numbers into their own words.</param>
    ''' <returns>A title-cased string with spaces between words.</returns>
    <Extension()>
    Public Function ToTitleCase(inStr As String, Optional separateNumbers As Boolean = True) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If
        If inStr.Contains("MmolL", StringComparison.InvariantCultureIgnoreCase) Then
            Return inStr
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
        resultString = resultString.Replace("S G", "Sensor Glucose", StringComparison.InvariantCulture)
        Return resultString.Replace("time", " Time", False, Provider)
    End Function

    ''' <summary>
    '''  Truncates a string that represents a <see langword="Single"/> to a specified number of decimal digits.
    ''' </summary>
    ''' <param name="s">The string to truncate.</param>
    ''' <param name="decimalDigits">The number of decimal digits to keep.</param>
    ''' <returns>A truncated string representation of the <see langword="Single"/> value.</returns>
    ''' <remarks>Used for truncating values to a specific number of decimal places.</remarks>
    <Extension>
    Public Function TruncateSingleString(s As String, decimalDigits As Integer) As String
        Dim i As Integer = s.IndexOfAny(s_commaOrPeriod)
        If i < 0 Then
            If Not IsNumeric(s) Then
                Return s
            End If
            i = s.Length
            s &= Provider.NumberFormat.NumberDecimalSeparator
        End If
        s &= New String("0"c, decimalDigits + 1)
        Return s.Substring(0, i + decimalDigits + 1)
    End Function

End Module
