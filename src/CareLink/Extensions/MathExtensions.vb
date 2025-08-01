' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    Private Function GetDigits(value As Double, digits As Integer, considerValue As Boolean) As Integer
        Return If(considerValue AndAlso value < 10, 2, digits)
    End Function

    ''' <summary>
    '''  Rounds a Single value to the specified number of decimal digits.
    '''  If <paramref name="digits"/> is 3, rounds to the nearest 0.025 increment.
    ''' </summary>
    ''' <param name="value">The Single value to round.</param>
    ''' <param name="digits">The number of decimal digits to round to.</param>
    ''' <returns>The rounded Single value.</returns>
    <Extension>
    Friend Function GetRoundedValue(value As Single, digits As Integer) As Single
        Return If(digits = 3,
                  value.RoundTo025,
                  value.RoundSingle(digits, considerValue:=False))
    End Function

    ''' <summary>
    '''  Rounds a <see langword="Single"/> value to the specified number of <paramref name="digits"/>.
    '''  If <paramref name="considerValue"/> is True and the value is less than 10, rounds to 2 decimal digits.
    ''' </summary>
    ''' <param name="value">The Single value to round.</param>
    ''' <param name="digits">The number of decimal digits to round to.</param>
    ''' <param name="considerValue">Whether to consider the value for special rounding.</param>
    ''' <returns>The rounded Single value.</returns>
    <Extension>
    Friend Function RoundSingle(value As Single, digits As Integer, considerValue As Boolean) As Single
        digits = GetDigits(value, digits, considerValue)
        Return CSng(Math.Round(value, digits))
    End Function

    ''' <summary>
    '''  Rounds a Double value to the specified number of <paramref name="digits"/> and returns as Single.
    '''  If <paramref name="considerValue"/> is True and the value is less than 10, rounds to 2 decimal digits.
    ''' </summary>
    ''' <param name="value">The Double value to round.</param>
    ''' <param name="digits">The number of decimal digits to round to.</param>
    ''' <param name="considerValue">Whether to consider the value for special rounding (optional).</param>
    ''' <returns>The rounded value as Single.</returns>
    <Extension>
    Friend Function RoundToSingle(value As Double, digits As Integer, Optional considerValue As Boolean = False) As Single
        digits = GetDigits(value, digits, considerValue)
        Return CSng(Math.Round(value, digits))
    End Function

    ''' <summary>
    '''  Checks whether the single value is close enough to zero within a reasonable tolerance.
    ''' </summary>
    ''' <param name="value">The Single value to check.</param>
    ''' <param name="tolerance">The tolerance level for considering the value as zero.</param>
    ''' <remarks>
    '''  This is useful for comparing floating-point numbers to zero, accounting for precision issues.
    ''' </remarks>
    ''' <returns>
    '''  <see langword="True"/> if the value is almost zero; otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function AlmostZero(value As Single, Optional tolerance As Single = 0.000001F) As Boolean
        Return Math.Abs(value) <= tolerance
    End Function

    ''' <summary>
    '''  Determines whether a Single value is an invalid sensor glucose (SG) value.
    ''' </summary>
    ''' <param name="f">The Single value to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the value is invalid; otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSgInvalid(f As Single) As Boolean
        Return Single.IsNaN(f) OrElse Single.IsInfinity(f) OrElse f <= 0
    End Function

    ''' <summary>
    '''  Determines whether a Single value is a valid sensor glucose (SG) value.
    ''' </summary>
    ''' <param name="number">The Single value to check.</param>
    '''  <see langword="True"/> if the value is valid; otherwise, <see langword="False"/>.
    <Extension>
    Public Function IsSgValid(number As Single) As Boolean
        Return Not number.IsSgInvalid
    End Function

    ''' <summary>
    '''  Determines whether <paramref name="singleValue"/> is equal to <paramref name="integerValue"/>,
    '''  within <see cref="Single.Epsilon"/>.
    ''' </summary>
    ''' <param name="singleValue">The Single value to compare.</param>
    ''' <param name="integerValue">The Integer value to compare.</param>
    ''' <param name="tolerance">The tolerance level for considering the value as zero.</param>
    ''' <returns>
    '''  <see langword="True"/> if the values are almost equal; otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSingleEqualToInteger(
        singleValue As Single,
        integerValue As Integer,
        Optional tolerance As Single = 0.000001F) As Boolean

        ' Optionally check if integerValue fits in Single range - typically integer fits in Single exactly up to 2^24
        Const maxExactInteger As Integer = 16777216 ' 2^24
        If integerValue > maxExactInteger OrElse integerValue < -maxExactInteger Then
            ' Beyond this range, Single might not represent integer exactly.
            ' So approximate equality may not be meaningful.
            Return False
        End If

        Return Math.Abs(value:=singleValue - integerValue) <= tolerance
    End Function

    ''' <summary>
    '''  Parses <paramref name="value"/> to a Single value,
    '''  optionally rounding to the specified number of <paramref name="digits"/>.
    ''' </summary>
    ''' <param name="value">The string to parse.</param>
    ''' <param name="digits">The number of decimal digits to round to. If -1, determines from the string.</param>
    ''' <returns>The parsed and rounded Single value, or <see cref="Single.NaN"/> if parsing fails.</returns>
    <Extension>
    Public Function ParseSingle(value As String, Optional digits As Integer = -1) As Single
        If String.IsNullOrWhiteSpace(value) Then
            Return Single.NaN
        End If
        value = value.Trim
        If value.Contains(value:=","c) AndAlso value.Contains(value:=CareLinkDecimalSeparator) Then
            Dim message As String = $"{NameOf(value)} = {value}, contains both a comma and period."
            Throw New ArgumentException(message, paramName:=NameOf(value))
        End If
        value = value.Replace(oldChar:=","c, newChar:=CareLinkDecimalSeparator)
        If digits = -1 Then
            Dim startIndex As Integer = value.IndexOf(CareLinkDecimalSeparator)
            digits = If(startIndex = -1, 0, value.Substring(startIndex).Length)
        End If
        Dim result As Single
        Return If(Single.TryParse(s:=value, style:=NumberStyles.Number, provider:=usDataCulture, result),
                  result.GetRoundedValue(digits),
                  Single.NaN)
    End Function

    ''' <summary>
    '''  Parses an object to a Single value, rounding to the specified number of <paramref name="digits"/>.
    ''' </summary>
    ''' <param name="value">The object to parse (String, Single, Double, or Decimal).</param>
    ''' <param name="digits">The number of decimal digits to round to.</param>
    ''' <returns>The parsed and rounded Single value, or <see cref="Single.NaN"/> if parsing fails.</returns>
    Public Function ParseSingle(value As Object, digits As Integer) As Single
        If value Is Nothing Then
            Return Single.NaN
        End If

        Dim returnSingle As Single
        Select Case True
            Case TypeOf value Is String
                Return CStr(value).ParseSingle(digits)
            Case TypeOf value Is Single
                returnSingle = CSng(value)
            Case TypeOf value Is Double
                returnSingle = CSng(value)
            Case TypeOf value Is Decimal
                returnSingle = CSng(value)
            Case Else
                Throw UnreachableException(propertyName:=value.GetType.Name)
        End Select

        Return GetRoundedValue(value:=returnSingle, digits)
    End Function

    ''' <summary>
    '''  Rounds a Single value to the nearest 0.025 increment.
    ''' </summary>
    ''' <param name="f">The Single value to round.</param>
    ''' <returns>The rounded Single value, or <see cref="Single.NaN"/> if the input is NaN.</returns>
    <Extension>
    Public Function RoundTo025(f As Single) As Single
        Return If(Single.IsNaN(f),
                  Single.NaN,
                  CDbl(f).RoundTo025)
    End Function

    ''' <summary>
    '''  Rounds a Double value to the nearest 0.025 increment and returns as Single.
    ''' </summary>
    ''' <param name="d">The Double value to round.</param>
    ''' <returns>The rounded value as Single, or <see cref="Single.NaN"/> if the input is NaN.</returns>
    <Extension>
    Public Function RoundTo025(d As Double) As Single
        If Double.IsNaN(d) Then
            Return Single.NaN
        Else
            Dim inverse As Single = 1 / 0.025
            Dim dividend As Double = d * inverse
            dividend = Math.Round(dividend)
            Return CSng(dividend / inverse)
        End If
    End Function

End Module
