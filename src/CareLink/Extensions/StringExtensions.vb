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
    ''' <param name="input">The input string to clean.</param>
    ''' <returns>
    '''  A <see langword="String"/> with multiple spaces replaced by a single space
    '''  and trimmed.
    ''' </returns>
    <Extension>
    Public Function CleanSpaces(input As String) As String
        Return If(String.IsNullOrWhiteSpace(value:=input),
                  String.Empty,
                  Regex.Replace(input, pattern:="\s+", replacement:=" ").Trim)
    End Function

    ''' <summary>
    '''  Counts the number of times a specific character
    '''  <paramref name="c"/> appears in a string.
    ''' </summary>
    ''' <param name="s">The string to search.</param>
    ''' <param name="c">The character to count.</param>
    ''' <returns>
    '''  The number of occurrences of the character <paramref name="c"/> in the string.
    ''' </returns>
    <Extension()>
    Public Function Count(s As String, c As Char) As Integer
        Dim predicate As Func(Of Char, Boolean) = Function(c1 As Char) As Boolean
                                                      Return c1 = c
                                                  End Function

        Return s.Count(predicate)
    End Function

    ''' <summary>
    '''  Find the index of the first occurrence of any character in
    '''  the <paramref name="chars"/> list
    ''' </summary>
    ''' <param name="inputString">The string to search.</param>
    ''' <param name="chars">The list of characters to find.</param>
    ''' <param name="startIndex">The index to start searching from.</param>
    ''' <returns>
    '''  The index of the first occurrence of any character in the list, or -1 if not found.
    ''' </returns>
    <Extension>
    Public Function FindIndexOfAnyChar(inputString As String, chars As List(Of Char), startIndex As Integer) As Integer
        If inputString Is Nothing Then
            Return -1
        End If

        If chars Is Nothing OrElse startIndex < 0 OrElse startIndex >= inputString.Length Then
            Throw New ArgumentException(message:="Invalid input parameters.")
        End If
        For i As Integer = startIndex To inputString.Length - 1
            If chars.Contains(item:=inputString(index:=i)) Then
                Return i
            End If
        Next
        Return -1 ' Return -1 if no character is found
    End Function

    ''' <summary>
    '''  Converts a <see langword="String"/> to a <see langword="Double"/>
    '''  using <see cref="CultureInfo.InvariantCulture"/>.
    ''' </summary>
    ''' <param name="value">The string to convert to a <see langword="Double"/>.</param>
    ''' <returns>The converted <see langword="Double"/> value.</returns>
    ''' <exception cref="FormatException">
    '''  Thrown if the string is not a valid double.
    ''' </exception>
    <Extension>
    Public Function ParseDoubleInvariant(value As String) As Double
        Dim s As String = value.Replace(oldChar:=","c, newChar:=CareLinkDecimalSeparator)
        Return Double.Parse(s, provider:=CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    '''  Converts a <see langword="String"/> to a <see langword="Single"/>
    '''  using <see cref="CultureInfo.InvariantCulture"/>.
    ''' </summary>
    ''' <param name="value">The string to convert to a Single.</param>
    ''' <returns>The converted <see langword="Single"/> value.</returns>
    ''' <exception cref="FormatException">
    '''  Thrown if the string is not a valid single.
    ''' </exception>
    <Extension>
    Public Function ParseSingleInvariant(value As String) As Single
        Dim s As String = value.Replace(oldChar:=","c, newChar:=CareLinkDecimalSeparator)
        Return Single.Parse(s, provider:=CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    '''  Converts a string where the first letter of the string is not capitalized
    ''' </summary>
    ''' <param name="value">A <see langword="String"/> like THIS_IS A TITLE</param>
    ''' <returns>A <see langword="String"/> where the first character is lower case</returns>
    ''' <remarks>Used for converting strings that are not capitalized at the start</remarks>
    ''' <example>doNotCapitalizedFirstLetterString</example>
    <Extension()>
    Public Function ToLowerCamelCase(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If

        Dim result As New StringBuilder(value:=Char.ToLowerInvariant(value(index:=0)))
        If value.Length > 1 Then
            result.Append(value:=value.AsSpan(start:=1))
        End If
        Return result.ToString
    End Function

    ''' <summary>
    '''  Converts an unsigned integer representing total units
    '''  (years, days, months, hours, minutes, or seconds) to a formatted string.
    ''' </summary>
    ''' <param name="totalUnits">The total units to convert.</param>
    ''' <param name="Unit">The unit  to use, e.g., "minute" or "hour".</param>
    ''' <returns>A formatted string representing the total units.</returns>
    ''' <param name="includeValue"></param>
    <Extension>
    Public Function ToUnits(totalUnits As UInteger, unit As String, Optional includeValue As Boolean = True) As String
        Dim unitOnly As String = If(totalUnits = 1,
                                    unit,
                                    $"{unit}s")
        Return If(includeValue,
                  $"{totalUnits:N0} {unitOnly}",
                  unitOnly)
    End Function

    ''' <summary>
    '''  Converts an integer representing total units
    '''  (e.g., years, days, months, hours, minutes, or seconds)
    '''  to a formatted string with optional prefix and suffix.
    ''' </summary>
    ''' <param name="totalUnits">The total units to convert.</param>
    ''' <param name="unit">The unit to use, e.g., "minute" or "hour".</param>
    ''' <param name="prefix">Optional prefix to prepend to the formatted string.</param>
    ''' <param name="suffix">Optional suffix to append to the formatted string.</param>
    ''' <param name="includeValue">
    '''  If true, includes the numeric value in the formatted string.
    ''' </param>
    ''' <returns>
    '''  A formatted string representing the total units with optional prefix and suffix.
    ''' </returns>
    <Extension>
    Public Function ToUnits(
        totalUnits As Integer,
        unit As String,
        Optional prefix As String = EmptyString,
        Optional suffix As String = EmptyString,
        Optional includeValue As Boolean = True) As String

        Dim unitOnly As String = If(totalUnits = 1,
                                    unit,
                                    $"{unit}s")
        Return If(includeValue,
                  $"{prefix}{totalUnits:N0}{unitOnly}{suffix}",
                  $"{prefix}{unitOnly}{suffix}")
    End Function

    ''' <summary>
    '''  Converts a string of words separated by a space or underscore to a title case string,
    '''  where the first letter of every word is capitalized and the rest are lower case.
    ''' </summary>
    ''' <param name="value">A string like "THIS_IS A TITLE".</param>
    ''' <param name="separateDigits">If true, separates numbers into their own words.</param>
    ''' <returns>A title-cased string.</returns>
    <Extension()>
    Public Function ToTitle(value As String, Optional separateDigits As Boolean = False) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If

        Dim c As Char = value(index:=0)
        Dim previousCharNumeric As Boolean = False
        Dim firstLetterOfWord As Boolean = False
        Dim result As New StringBuilder(value:=Char.ToUpperInvariant(c))

        If separateDigits AndAlso IsNumeric(Expression:=c) Then
            previousCharNumeric = True
            firstLetterOfWord = True
        End If

        For Each c In value.Substring(startIndex:=1)
            If c = " "c Or c = "_"c Then
                firstLetterOfWord = True
                previousCharNumeric = False
                result.Append(value:=" "c)
            ElseIf separateDigits AndAlso IsNumeric(Expression:=c) Then
                firstLetterOfWord = True
                previousCharNumeric = True
                result.Append(value:=" "c)
                result.Append(value:=Char.ToLowerInvariant(c))
            ElseIf firstLetterOfWord Then
                If previousCharNumeric Then
                    previousCharNumeric = False
                    result.Append(value:=" "c)
                End If
                firstLetterOfWord = False
                result.Append(value:=Char.ToUpperInvariant(c))
            Else
                previousCharNumeric = IsNumeric(Expression:=c)
                result.Append(value:=Char.ToLowerInvariant(c))
            End If
        Next
        Return result.ToString.Replace(oldValue:="Bg ", newValue:="BG ") _
                              .Replace(oldValue:="Sg ", newValue:="SG ") _
                              .Replace(oldValue:=" Am.", newValue:=" AM.") _
                              .Replace(oldValue:=" Pm.", newValue:=" PM.")
    End Function

    ''' <summary>
    '''  Converts a string of concatenated words (PascalCase or camelCase)
    '''  to a title case string, where the first letter of every word is capitalized
    '''  and words are separated by spaces.
    ''' </summary>
    ''' <param name="value">A string like "ThisIsATitle".</param>
    ''' <param name="separateNumbers">
    '''  If true, separates numbers into their own words.
    ''' </param>
    ''' <returns>A title-cased string with spaces between words.</returns>
    <Extension()>
    Public Function ToTitleCase(value As String, Optional separateNumbers As Boolean = True) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If
        If value.ContainsNoCase(value:="MmolL") Then
            Return value
        End If
        Dim result As New StringBuilder(value:=Char.ToUpperInvariant(value(index:=0)))
        Dim lastWasNumeric As Boolean = Char.IsNumber(value(index:=0))
        For Each c As Char In value.Substring(startIndex:=1)
            If Char.IsLower(c) Then
                If lastWasNumeric Then
                    result.Append(value:=Char.ToUpperInvariant(c))
                Else
                    result.Append(value:=c)
                End If
                lastWasNumeric = False
            ElseIf Char.IsSymbol(c) Then
                ' If the character is a symbol, we assume it is part of a word
                result.Append(value:=c)
                lastWasNumeric = False
            ElseIf Char.IsNumber(c) AndAlso Not separateNumbers Then
                result.Append(value:=c)
                lastWasNumeric = True
            Else
                ' add a space before the uppercase letter
                result.Append(value:=" "c)
                result.Append(value:=Char.ToUpperInvariant(c))
                lastWasNumeric = False
            End If
        Next
        Dim resultString As String = result.Replace(oldValue:="Care Link", newValue:="CareLink").ToString
        resultString = If(Not resultString.Contains(value:="™"c),
                          resultString.ReplaceNoCase(oldValue:="CareLink", newValue:="CareLink™"),
                          resultString.ReplaceNoCase(oldValue:="CareLink", newValue:="CareLink"))
        resultString = resultString.Replace(oldValue:="S G", newValue:="Sensor Glucose")

        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Return resultString.Replace(oldValue:="time", newValue:=" Time", ignoreCase:=False, culture:=provider)
    End Function

    ''' <summary>
    '''  Truncates a string that represents a <see langword="Single"/>
    '''  to a specified number of decimal digits.
    ''' </summary>
    ''' <param name="expression">The string to truncate.</param>
    ''' <param name="digits">
    '''  The number of decimal digits to keep.
    ''' </param>
    ''' <returns>
    '''  A truncated string representation of the <see langword="Single"/> value.
    ''' </returns>
    ''' <remarks>
    '''  Used for truncating values to a specific number of decimal places.
    ''' </remarks>
    <Extension>
    Public Function TruncateSingle(expression As String, digits As Integer) As String
        Dim i As Integer = expression.IndexOfAny(anyOf:=s_commaOrPeriod)
        If i < 0 Then
            If Not IsNumeric(expression) Then
                Return expression
            End If
            i = expression.Length
            expression &= DecimalSeparator
        End If
        expression &= New String("0"c, count:=digits + 1)
        Return expression.Substring(startIndex:=0, length:=i + digits + 1)
    End Function

#Region "IgnoreCase String Comparisons"

    ''' <summary>
    '''  Checks if a string contains another string, ignoring case.
    ''' </summary>
    ''' <param name="s1">The string to search in.</param>
    ''' <param name="value">The string to search for.</param>
    ''' <returns>
    '''  <see langword="True"/> if <paramref name="s1"/> contains <paramref name="value"/>;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension()>
    Public Function ContainsNoCase(s1 As String, value As String) As Boolean
        If s1 Is Nothing Then Return False
        Return s1.Contains(value, comparisonType:=StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>
    '''  Checks if a string ends with another string, ignoring case.
    ''' </summary>
    ''' <param name="s">The string to search in.</param>
    ''' <param name="value">The string to search for.</param>
    ''' <returns>
    '''  <see langword="True"/> if <paramref name="s"/> ends with <paramref name="value"/>;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>Used for case-insensitive substring checks.</remarks>
    <Extension()>
    Public Function EndsWithNoCase(s As String, value As String) As Boolean
        If s Is Nothing OrElse value Is Nothing Then Return False
        Return s.EndsWith(value, comparisonType:=StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>
    '''  Checks if two strings are equal, ignoring case.
    ''' </summary>
    ''' <param name="a">The first string to compare.</param>
    ''' <param name="b">The second string to compare.</param>
    ''' <returns>
    '''  <see langword="True"/> if the strings are equal, ignoring case;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>Used for case-insensitive string comparisons.</remarks>
    <Extension()>
    Public Function EqualsNoCase(a As String, b As String) As Boolean
        If a Is Nothing OrElse b Is Nothing Then Return False
        Return String.Equals(a, b, comparisonType:=StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>
    '''  Checks if an object is equal to a string, ignoring case.
    '''  This method is useful for comparing an object that may be a string
    '''  or null with a string.
    ''' </summary>
    ''' <param name="a">The first object to compare.</param>
    ''' <param name="b">The second string to compare.</param>
    ''' <returns>
    '''  <see langword="True"/> if the strings are equal, ignoring case;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>
    '''  Used for case-insensitive <see langword="Object"/> to string comparisons.
    ''' </remarks>
    Public Function EqualsNoCase(a As Object, b As String) As Boolean
        If a Is Nothing OrElse b Is Nothing OrElse TypeOf a IsNot String Then Return False
        Return EqualsNoCase(a.ToString, b)
    End Function

    ''' <summary>
    '''  Finds the index of the first occurrence of a string within another string,
    '''  ignoring case.
    ''' </summary>
    ''' <param name="s1">The string to search in.</param>
    ''' <param name="value">The string to search for.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of<paramref name="value"/>
    '''  in <paramref name="s1"/>;
    '''  or -1 if not found.
    ''' </returns>
    ''' <remarks>Used for case-insensitive substring searches.</remarks>
    <Extension()>
    Public Function IndexOfNoCase(s1 As String, value As String) As Integer
        If s1 Is Nothing OrElse value Is Nothing Then Return -1
        Return s1.IndexOf(value, comparisonType:=StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>
    '''  Removes all occurrences of a specified string from the
    '''  <paramref name="input"/> string, ignoring case.
    ''' </summary>
    ''' <param name="input">The original string.</param>
    ''' <param name="s">The string to remove.</param>
    ''' <returns>
    '''  A new string that is equivalent to the current string except
    '''  that all occurrences of
    '''  <paramref name="s"/> are removed.
    ''' </returns>
    ''' <remarks>
    '''  This method is used for case-insensitive string removals,
    '''  ensuring that the removal respects
    '''  the current UI culture.
    ''' </remarks>
    <Extension()>
    Public Function Remove(input As String, s As String) As String
        If input Is Nothing Then Return Nothing

        Dim culture As CultureInfo = CultureInfo.CurrentUICulture
        Return input.Replace(oldValue:=s, newValue:=String.Empty, ignoreCase:=True, culture)
    End Function

    ''' <summary>
    '''  Returns a new string in which all occurrences of a specified string
    '''  in the current instance are replaced with another specified string,
    '''  using CultureInfo.CurrentUICulture and case insensitivity.
    ''' </summary>
    ''' <param name="s">Original string</param>
    ''' <param name="oldValue">The string to replace</param>
    ''' <param name="newValue">The replacement string</param>
    ''' <returns>
    '''  A string that is equivalent to the current string except that all
    '''  occurrences of <paramref name="oldValue"/> are replaced by
    '''  <paramref name="newValue"/>. If <paramref name="oldValue"/> is
    '''  <see cref="String.Empty"/>, the method returns the current string unchanged.
    ''' </returns>
    ''' <exception cref="ArgumentNullException">
    '''  Thrown if <paramref name="oldValue"/> or <paramref name="newValue"/>
    '''  is <see langword="Nothing"/>.
    ''' </exception>
    ''' <remarks>
    '''  This method is used for case-insensitive string replacements,
    '''  ensuring that the replacement respects the current UI culture.
    ''' </remarks>
    <Extension()>
    Public Function ReplaceNoCase(
        s As String,
        oldValue As String,
        newValue As String) As String

        If s Is Nothing Then
            Return Nothing
        End If
        Dim culture As CultureInfo = CultureInfo.CurrentUICulture
        Return s.Replace(oldValue, newValue, ignoreCase:=True, culture)
    End Function

    ''' <summary>
    '''  Checks if a string starts with another string, ignoring case.
    ''' </summary>
    ''' <param name="s">The string to search in.</param>
    ''' <param name="value">The string to search for.</param>
    ''' <returns>
    '''  <see langword="True"/> if <paramref name="s"/> starts with <paramref name="value"/>;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>Used for case-insensitive substring checks.</remarks>
    <Extension()>
    Public Function StartsWithNoCase(s As String, value As String) As Boolean
        If s Is Nothing OrElse value Is Nothing Then Return False
        Return s.StartsWith(value, comparisonType:=StringComparison.OrdinalIgnoreCase)
    End Function

#End Region ' IgnoreCase String Comparisons

End Module
