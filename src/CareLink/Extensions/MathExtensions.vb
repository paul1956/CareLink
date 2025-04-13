﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    <Extension>
    Friend Function GetRoundedValue(value As Single, decimalDigits As Integer) As Single
        Return If(decimalDigits = 3, value.RoundTo025, value.RoundSingle(decimalDigits, False))
    End Function

    <Extension>
    Friend Function RoundSingle(singleValue As Single, decimalDigits As Integer, considerValue As Boolean) As Single

        Return CSng(Math.Round(singleValue, If(considerValue AndAlso singleValue < 10, 2, decimalDigits)))
    End Function

    <Extension>
    Friend Function RoundToSingle(doubleValue As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(doubleValue, decimalDigits))
    End Function

    <Extension>
    Public Function AlmostZero(single1 As Single) As Boolean
        Return single1 >= 0 - Single.Epsilon And single1 <= 0 + Single.Epsilon
    End Function

    <Extension>
    Public Function IsSingleEqualToInteger(singleValue As Single, integerValue As Integer) As Boolean
        Return Math.Abs(singleValue - integerValue) < Single.Epsilon AndAlso
           singleValue > Integer.MinValue AndAlso
           singleValue < Integer.MaxValue
    End Function

    <Extension>
    Public Function IsSgValid(number As Single) As Boolean
        Return Not number.IsSgInvalid
    End Function

    <Extension>
    Public Function IsSgInvalid(number As Single) As Boolean
        Return Single.IsNaN(number) OrElse
            Single.IsInfinity(number) OrElse
            Single.IsNegativeInfinity(number) OrElse
            Single.IsPositiveInfinity(number) OrElse
            number <= 0
    End Function

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

    <Extension>
    Public Function RoundTo025(originalValue As Single) As Single
        Return If(Single.IsNaN(originalValue), Single.NaN, CDbl(originalValue).RoundTo025)
    End Function

    <Extension>
    Public Function TryParseSingle(valueString As String, ByRef result As Single, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Boolean
        If valueString.Contains(","c) AndAlso valueString.Contains(ServerDecimalSeparator) Then
            Throw New Exception($"{NameOf(valueString)} = {valueString}, and contains both comma and period in Line {sourceLineNumber} in {memberName}.")
        End If

        If Single.TryParse(valueString.Replace(",", ServerDecimalSeparator), NumberStyles.Number, usDataCulture, result) Then
            result = result.RoundSingle(10, False)
            Return True
        End If
        If Single.TryParse(valueString, NumberStyles.Number, Provider, result) Then
            result = result.RoundSingle(10, False)
            Return True
        End If
        result = Single.NaN
        Return False
    End Function

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
