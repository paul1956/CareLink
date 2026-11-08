' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions
    Public Const Tolerance As Single = 0.000001F

    ''' <summary>
    '''  Checks whether the Double value is close enough to zero
    '''  within a reasonable <see cref="Tolerance"/>.
    ''' </summary>
    ''' <param name="value">The Double value to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the value is almost zero;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>
    '''  This is useful for comparing floating-point numbers to zero,
    '''  accounting for precision issues.
    ''' </remarks>
    <Extension>
    Public Function AlmostZero(value As Double) As Boolean
        Return Math.Abs(value) <= Tolerance
    End Function

    ''' <summary>
    '''  Checks whether the single value is close enough to zero within
    '''  a reasonable <see cref="Tolerance"/>.
    ''' </summary>
    ''' <param name="value">The Single value to check.</param>
    ''' <remarks>
    '''  This is useful for comparing floating-point numbers to zero,
    '''  accounting for precision issues.
    ''' </remarks>
    ''' <returns>
    '''  <see langword="True"/> if the value is almost zero;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function AlmostZero(value As Single) As Boolean
        Return Math.Abs(value) <= Tolerance
    End Function

    ''' <summary>
    '''  Formats a Double value with at most one trailing zero.
    ''' </summary>
    ''' <param name="value">The Double value to format.</param>
    ''' <returns>The formatted string.</returns>
    <Extension>
    Public Function FormatWithTrailingZero(value As Single) As String
        Dim rounded As Single = RoundToSingle(value, digits:=3)

        ' Use general format to avoid unnecessary trailing zeros
        Dim str As String = rounded.ToString(format:="G", provider:=CultureInfo.InvariantCulture)

        ' Ensure one trailing zero if integer after rounding
        If Not str.Contains(value:="."c) Then
            str &= ".0"
        End If

        Return str
    End Function

    ''' <summary>
    '''  Gets the fractional part of a Decimal value.
    ''' </summary>
    ''' <param name="d">The Decimal value to get the fractional part from.</param>
    ''' <returns>The fractional part of the Decimal value.</returns>
    <Extension>
    Public Function FractionalPart(d As Decimal) As Single
        Return d - Math.Floor(d)
    End Function

    ''' <summary>
    '''  Determines whether <paramref name="singleValue"/> is equal
    '''  to <paramref name="integerValue"/>,
    '''  within <see cref="Single.Epsilon"/>.
    ''' </summary>
    ''' <param name="singleValue">The Single value to compare.</param>
    ''' <param name="integerValue">The Integer value to compare.</param>
    ''' <returns>
    '''  <see langword="True"/> if the values are almost equal;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsEqual(singleValue As Single,
                            integerValue As Integer) As Boolean

        ' Optionally check if integerValue fits in Single range - typically integer
        ' fits in Single exactly up to 2^24
        Const maxExactInteger As Integer = 16777216 ' 2^24
        If integerValue > maxExactInteger OrElse integerValue < -maxExactInteger Then
            ' Beyond this range, Single might not represent integer exactly.
            ' So approximate equality may not be meaningful.
            Return False
        End If

        Return Math.Abs(value:=singleValue - integerValue) <= Tolerance
    End Function

    ''' <summary>
    '''  Determines whether a Single value is an invalid sensor glucose (SG) value.
    ''' </summary>
    ''' <param name="f">The Single value to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the value is invalid;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSgInvalid(f As Single) As Boolean
        Return Not Single.IsFinite(f) OrElse f <= 0
    End Function

    ''' <summary>
    '''  Determines whether a Single value is a valid sensor glucose (SG) value.
    ''' </summary>
    ''' <param name="number">The Single value to check.</param>
    '''  <see langword="True"/> if the value is valid;
    '''  otherwise, <see langword="False"/>.
    <Extension>
    Public Function IsSgValid(number As Single) As Boolean
        Return Not number.IsSgInvalid
    End Function

    ''' <summary>
    '''  Parses an object to a Single value, without rounding.
    ''' </summary>
    ''' <param name="value">
    '''  The object to parse (String, Single, Double, or Decimal).
    ''' </param>
    ''' <returns>
    '''  The parsed <see langword="Single"/> value, or <see cref="Single.NaN"/> if parsing fails.
    ''' </returns>
    Public Function ParseAsSingle(value As Object) As Single
        If value Is Nothing Then
            Return Single.NaN
        End If

        Select Case True
            Case TypeOf value Is String
                Return CStr(value).ParseSingle()
            Case TypeOf value Is Single
                Return CSng(value)
            Case TypeOf value Is Double
                Return CSng(value)
            Case TypeOf value Is Decimal
                Return CSng(value)
        End Select

        Throw GetUnreachableException(paramName:=value.GetType.Name)
    End Function

    ''' <summary>
    '''  Parses <paramref name="s"/> to a Single value,
    '''  optionally rounding to the specified number of <paramref name="digits"/>.
    ''' </summary>
    ''' <param name="s">The string to parse.</param>
    ''' <param name="digits">
    '''  The number of decimal roundingType to round to. If -1, determines from the string.
    ''' </param>
    ''' <returns>
    '''  The parsed and rounded Single value, or <see cref="Single.NaN"/> if parsing fails.
    ''' </returns>
    <Extension>
    Public Function ParseSingle(s As String) As Single
        If IsNullOrWhiteSpace(value:=s) Then
            Return Single.NaN
        End If
        s = s.Trim
        If s.Contains(value:=","c) AndAlso s.Contains(value:=CareLinkDecimalSeparator) Then
            Dim message As String = $"{NameOf(s)} = {s}, contains both a comma and period."
            Throw New ArgumentException(message, paramName:=NameOf(s))
        End If
        s = s.Replace(oldChar:=","c, newChar:=CareLinkDecimalSeparator)
        Dim result As Single
        Const style As NumberStyles = NumberStyles.Number
        Return If(Single.TryParse(s, style, provider:=UsDataCulture, result),
                  result,
                  Single.NaN)
    End Function

    ''' <summary>
    '''  Rounds a SG based on it value, small values are rounded to 2 decimal places,
    '''  larger values to 1 decimal place.
    ''' </summary>
    ''' <param name="value">The SG value to round.</param>
    ''' <returns>The rounded SG value.</returns>
    <Extension>
    Public Function RoundSg(value As Single) As Single
        Dim digits As Integer = If(value < 10,
                                   2,
                                   1)
        Return RoundToSingle(value, digits)
    End Function

    ''' <summary>
    '''  Rounds a <see langword="Single"/> value to the specified number of decimal places.
    ''' </summary>
    ''' <param name="value">The <see langword="Single"/> value to round.</param>
    ''' <param name="digits">The number of decimal places to round to.</param>
    ''' <returns>The rounded <see langword="Single"/> value.</returns>
    <Extension>
    Public Function RoundToSingle(value As Single, digits As Integer) As Single
        Return If(Single.IsNaN(f:=value),
                  value,
                  CSng(Math.Round(value, digits)))
    End Function

    ''' <summary>
    '''  Converts a Single value to a string with a comma as the decimal separator.
    ''' </summary>
    ''' <param name="s">The Single value to convert.</param>
    ''' <returns>
    '''  The string representation of the Single value with
    '''  a comma as the decimal separator.
    ''' </returns>
    <Extension>
    Public Function ToCommaDelimited(s As Single) As String
        Return s.ToString.Replace(oldValue:=".", newValue:=",")
    End Function

    ''' <summary>
    ''' Converts a Single value to a string with a period as the decimal separator.
    ''' </summary>
    ''' <param name="s">The Single value to convert.</param>
    ''' <returns>
    '''  The string representation of the Single value with
    '''  a period as the decimal separator.
    ''' </returns>
    <Extension>
    Public Function ToPeriodDelimited(s As Single) As String
        Return s.ToString.Replace(oldValue:=",", newValue:=".")
    End Function

End Module
