' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    ''' <summary>
    '''  Rounds a Single value to the specified number of decimal digits.
    '''  If <paramref name="decimalDigits"/> is 3, rounds to the nearest 0.025 increment.
    ''' </summary>
    ''' <param name="value">The Single value to round.</param>
    ''' <param name="decimalDigits">The number of decimal digits to round to.</param>
    ''' <returns>The rounded Single value.</returns>
    <Extension>
    Friend Function GetRoundedValue(value As Single, decimalDigits As Integer) As Single
        Return If(decimalDigits = 3, value.RoundTo025, value.RoundSingle(decimalDigits, False))
    End Function

    ''' <summary>
    '''  Rounds a <see langword="Single"/> value to the specified number of <paramref name="decimalDigits"/>.
    '''  If <paramref name="considerValue"/> is True and the value is less than 10, rounds to 2 decimal digits.
    ''' </summary>
    ''' <param name="singleValue">The Single value to round.</param>
    ''' <param name="decimalDigits">The number of decimal digits to round to.</param>
    ''' <param name="considerValue">Whether to consider the value for special rounding.</param>
    ''' <returns>The rounded Single value.</returns>
    <Extension>
    Friend Function RoundSingle(singleValue As Single, decimalDigits As Integer, considerValue As Boolean) As Single

        Return CSng(Math.Round(singleValue, If(considerValue AndAlso singleValue < 10, 2, decimalDigits)))
    End Function

    ''' <summary>
    '''  Rounds a Double value to the specified number of <paramref name="decimalDigits"/> and returns as Single.
    ''' </summary>
    ''' <param name="doubleValue">The Double value to round.</param>
    ''' <param name="decimalDigits">The number of decimal digits to round to.</param>
    ''' <returns>The rounded value as Single.</returns>

    <Extension>
    Friend Function RoundToSingle(doubleValue As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(doubleValue, decimalDigits))
    End Function

    ''' <summary>
    '''  Determines whether a Single value is almost zero, within the range of <see cref="Single.Epsilon"/>.
    ''' </summary>
    ''' <param name="single1">The Single value to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the value is almost zero; otherwise,
    '''  <see langword="False"/>.
    ''' </returns>

    <Extension>
    Public Function AlmostZero(single1 As Single) As Boolean
        Return single1 >= 0 - Single.Epsilon And single1 <= 0 + Single.Epsilon
    End Function

    ''' <summary>
    '''  Determines whether <paramref name="singleValue"/> is equal to <paramref name="integerValue"/>,
    '''  within <see cref="Single.Epsilon"/>.
    ''' </summary>
    ''' <param name="singleValue">The Single value to compare.</param>
    ''' <param name="integerValue">The Integer value to compare.</param>
    ''' <returns>
    '''  <see langword="True"/> if the values are almost equal; otherwise,
    '''  <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSingleEqualToInteger(singleValue As Single, integerValue As Integer) As Boolean
        Return Math.Abs(singleValue - integerValue) < Single.Epsilon AndAlso
           singleValue > Integer.MinValue AndAlso
           singleValue < Integer.MaxValue
    End Function

    ''' <summary>
    '''  Determines whether a Single value is a valid sensor glucose (SG) value.
    ''' </summary>
    ''' <param name="number">The Single value to check.</param>
    '''  <see langword="True"/> if the value is valid; otherwise,
    '''  <see langword="False"/>.
    <Extension>
    Public Function IsSgValid(number As Single) As Boolean
        Return Not number.IsSgInvalid
    End Function

    ''' <summary>
    '''  Determines whether a Single value is an invalid sensor glucose (SG) value.
    ''' </summary>
    ''' <param name="number">The Single value to check.</param>
    '''  <see langword="True"/> if the value is invalid; otherwise,
    '''  <see langword="False"/>.
    <Extension>
    Public Function IsSgInvalid(number As Single) As Boolean
        Return Single.IsNaN(number) OrElse
            Single.IsInfinity(number) OrElse
            Single.IsNegativeInfinity(number) OrElse
            Single.IsPositiveInfinity(number) OrElse
            number <= 0
    End Function

    ''' <summary>
    '''  Parses <paramref name="valueString"/> to a Single value,
    '''  optionally rounding to the specified number of <paramref name="decimalDigits"/>.
    ''' </summary>
    ''' <param name="valueString">The string to parse.</param>
    ''' <param name="decimalDigits">The number of decimal digits to round to. If -1, determines from the string.</param>
    ''' <returns>The parsed and rounded Single value, or <see cref="Single.NaN"/> if parsing fails.</returns>
    <Extension>
    Public Function ParseSingle(valueString As String, Optional decimalDigits As Integer = -1) As Single
        If valueString Is Nothing Then
            Return Single.NaN
        End If
        If valueString.Contains(","c) AndAlso valueString.Contains(ServerDecimalSeparator) Then
            Throw New ArgumentException($"{NameOf(valueString)} = {valueString}, contains both a comma and period.", NameOf(valueString))
        End If
        valueString = valueString.Replace(",", ServerDecimalSeparator)
        Dim returnSingle As Single
        If decimalDigits = -1 Then
            Dim index As Integer = valueString.IndexOf(ServerDecimalSeparator)
            decimalDigits = If(index = -1,
                               0,
                               valueString.Substring(index).Length
                              )
        End If
        If Single.TryParse(valueString, NumberStyles.Number, usDataCulture, returnSingle) Then
        Else
            Return Single.NaN
        End If
        Return GetRoundedValue(returnSingle, decimalDigits)
    End Function

    ''' <summary>
    '''  Parses an object to a Single value, rounding to the specified number of <paramref name="decimalDigits"/>.
    ''' </summary>
    ''' <param name="valueObject">The object to parse (String, Single, Double, or Decimal).</param>
    ''' <param name="decimalDigits">The number of decimal digits to round to.</param>
    ''' <returns>The parsed and rounded Single value, or <see cref="Single.NaN"/> if parsing fails.</returns>
    Public Function ParseSingle(valueObject As Object, decimalDigits As Integer) As Single
        If valueObject Is Nothing Then
            Return Single.NaN
        End If

        Dim returnSingle As Single
        Select Case True
            Case TypeOf valueObject Is String
                Return CStr(valueObject).ParseSingle(decimalDigits)
            Case TypeOf valueObject Is Single
                returnSingle = CSng(valueObject)
            Case TypeOf valueObject Is Double
                returnSingle = CSng(valueObject)
            Case TypeOf valueObject Is Decimal
                returnSingle = CSng(valueObject)
            Case Else
                Throw UnreachableException(valueObject.GetType.Name)
        End Select

        Return GetRoundedValue(returnSingle, decimalDigits)
    End Function

    ''' <summary>
    '''  Rounds a Single value to the nearest 0.025 increment.
    ''' </summary>
    ''' <param name="originalValue">The Single value to round.</param>
    ''' <returns>The rounded Single value, or <see cref="Single.NaN"/> if the input is NaN.</returns>
    <Extension>
    Public Function RoundTo025(originalValue As Single) As Single
        Return If(Single.IsNaN(originalValue), Single.NaN, CDbl(originalValue).RoundTo025)
    End Function

    ''' <summary>
    '''  Rounds a Double value to the nearest 0.025 increment and returns as Single.
    ''' </summary>
    ''' <param name="originalValue">The Double value to round.</param>
    ''' <returns>The rounded value as Single, or <see cref="Single.NaN"/> if the input is NaN.</returns>
    <Extension>
    Public Function RoundTo025(originalValue As Double) As Single
        If Double.IsNaN(originalValue) Then
            Return Single.NaN
        Else
            Dim inverse As Single = 1 / 0.025
            Dim dividend As Double = originalValue * inverse
            dividend = Math.Round(dividend)
            Return CSng(dividend / inverse)
        End If
    End Function

End Module
