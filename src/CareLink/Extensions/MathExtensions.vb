' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions
    Public Const Tolerance As Single = 0.000001F

    ''' <summary>
    '''  Checks if the Double obj is close enough to zero
    '''  within a reasonable <see cref="Tolerance"/>.
    ''' </summary>
    ''' <param name="d">The Double obj to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the obj is almost zero;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <remarks>
    '''  This is useful for comparing floating-point numbers to zero,
    '''  accounting for precision issues.
    ''' </remarks>
    <Extension>
    Public Function AlmostZero(d As Double) As Boolean
        Return Math.Abs(value:=d) <= Tolerance
    End Function

    ''' <summary>
    '''  Checks whether the single obj is close enough to zero within
    '''  a reasonable <see cref="Tolerance"/>.
    ''' </summary>
    ''' <param name="value">The Single obj to check.</param>
    ''' <remarks>
    '''  This is useful for comparing floating-point numbers to zero,
    '''  accounting for precision issues.
    ''' </remarks>
    ''' <returns>
    '''  <see langword="True"/> if the obj is almost zero;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function AlmostZero(value As Single) As Boolean
        Return Math.Abs(value) <= Tolerance
    End Function

    ''' <summary>
    '''  Formats a Double obj with at most one trailing zero.
    ''' </summary>
    ''' <param name="f">The Double obj to format.</param>
    ''' <returns>The formatted string.</returns>
    <Extension>
    Public Function FormatWithTrailingZero(f As Single) As String
        Dim rounded As Single = RoundToSingle(f, digits:=3)

        ' Use general format to avoid unnecessary trailing zeros
        Dim str As String = rounded.ToString(format:="G", provider:=CultureInfo.InvariantCulture)

        ' Ensure one trailing zero if integer after rounding
        If Not str.Contains(value:="."c) Then
            str &= ".0"
        End If

        Return str
    End Function

    ''' <summary>
    '''  Gets the fractional part of a Decimal obj.
    ''' </summary>
    ''' <param name="d">The Decimal obj to get the fractional part from.</param>
    ''' <returns>The fractional part of the Decimal obj.</returns>
    <Extension>
    Public Function FractionalPart(d As Decimal) As Single
        Return d - Math.Floor(d)
    End Function

    ''' <summary>
    '''  Determines whether <paramref name="singleValue"/> is equal
    '''  to <paramref name="integerValue"/>,
    '''  within <see cref="Single.Epsilon"/>.
    ''' </summary>
    ''' <param name="singleValue">The Single obj to compare.</param>
    ''' <param name="integerValue">The Integer obj to compare.</param>
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
    '''  Determines whether a Single obj is an invalid sensor glucose (SG) obj.
    ''' </summary>
    ''' <param name="f">The Single obj to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the obj is invalid;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSgInvalid(f As Single) As Boolean
        ' Test if it's  NaN or Infinity and Less than or equal 0
        Return Single.IsNaN(f) _
               OrElse Single.IsInfinity(f) _
               OrElse f <= 0
    End Function

    ''' <summary>
    '''  Determines whether a SG record is valid based on its SG obj and timestamp.
    ''' </summary>
    ''' <param name="sg">The SG record to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the SG record is valid;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSgRecordValid(sg As SG) As Boolean
        Return sg.Sg.IsSgValid OrElse sg.Timestamp <> Date.MinValue
    End Function

    ''' <summary>
    '''  Determines whether a Single obj is a valid sensor glucose (SG) obj.
    ''' </summary>
    ''' <param name="number">The Single obj to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the obj is within a valid range for Sensor Glucose (SG);
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsSgValid(number As Single) As Boolean
        Return Not number.IsSgInvalid
    End Function

    ''' <summary>
    '''  Parses an object to a Single obj, without rounding.
    ''' </summary>
    ''' <param name="obj">
    '''  The object to parse (String, Single, Double, or Decimal).
    ''' </param>
    ''' <returns>
    '''  The parsed <see langword="Single"/> obj, or <see cref="Single.NaN"/> if parsing fails.
    ''' </returns>
    Public Function ParseAsSingle(obj As Object) As Single
        Select Case True
            Case obj Is Nothing
                Return Single.NaN
            Case TypeOf obj Is String
                Dim value As String = CStr(obj)
                Return If(String.IsNullOrWhiteSpace(value),
                    Single.NaN,
                    value.ParseSingle())
            Case TypeOf obj Is Single
                Return CSng(obj)
            Case TypeOf obj Is Double
                Return CSng(obj)
            Case TypeOf obj Is Decimal
                Return CSng(obj)
        End Select

        Throw GetUnreachableException(paramName:=obj.GetType.Name)
    End Function

    ''' <summary>
    '''  Parses <paramref name="s"/> to a Single obj.
    ''' </summary>
    ''' <param name="s">The string to parse.</param>
    ''' <returns>
    '''  The parsed Single, or <see cref="Single.NaN"/> if parsing fails.
    ''' </returns>
    <Extension>
    Public Function ParseSingle(s As String) As Single
        If String.IsNullOrWhiteSpace(value:=s) Then
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
    '''  Rounds a SG based on it obj, small values are rounded to 2 decimal places,
    '''  larger values to 1 decimal place.
    ''' </summary>
    ''' <param name="f">The SG obj to round.</param>
    ''' <returns>The rounded SG obj.</returns>
    <Extension>
    Public Function RoundSg(f As Single) As Single
        Dim digits As Integer = If(f < 10,
                                   2,
                                   1)
        Return RoundToSingle(f, digits)
    End Function

    ''' <summary>
    '''  Rounds a <see langword="Single"/> obj to the specified number of decimal places.
    ''' </summary>
    ''' <param name="f">The <see langword="Single"/> obj to round.</param>
    ''' <param name="digits">The number of decimal places to round to.</param>
    ''' <returns>The rounded <see langword="Single"/> obj.</returns>
    <Extension>
    Public Function RoundToSingle(f As Single, digits As Integer) As Single
        Return If(Single.IsNaN(f),
                  f,
                  CSng(Math.Round(value:=f, digits)))
    End Function

    ''' <summary>
    '''  Converts a Single obj to a string with a comma as the decimal separator.
    ''' </summary>
    ''' <param name="f">The Single obj to convert.</param>
    ''' <returns>
    '''  The string representation of the Single obj with
    '''  a comma as the decimal separator.
    ''' </returns>
    <Extension>
    Public Function ToCommaDelimited(f As Single) As String
        Return f.ToString.Replace(oldValue:=".", newValue:=",")
    End Function

    ''' <summary>
    ''' Converts a Single obj to a string with a period as the decimal separator.
    ''' </summary>
    ''' <param name="s">The Single obj to convert.</param>
    ''' <returns>
    '''  The string representation of the Single obj with
    '''  a period as the decimal separator.
    ''' </returns>
    <Extension>
    Public Function ToPeriodDelimited(f As Single) As String
        Return f.ToString.Replace(oldValue:=",", newValue:=".")
    End Function

End Module
