' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    <Extension>
    Friend Function RoundSingle(singleValue As Single, decimalDigits As Integer) As Single

        Return CSng(Math.Round(singleValue, decimalDigits))
    End Function

    <Extension>
    Friend Function RoundToSingle(doubleValue As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(doubleValue, decimalDigits))
    End Function

    <Extension>
    Public Function ParseSingle(valueString As String, Optional decimalDigits As Integer = 10) As Single
        If valueString Is Nothing Then
            Return Single.NaN
        End If

        If valueString.Contains(","c) AndAlso valueString.Contains("."c) Then
            Throw New ArgumentException($"{NameOf(valueString)} = {valueString}, contains both a comma and period.", NameOf(valueString))
        End If

        Dim returnSingle As Single
        If Single.TryParse(valueString.Replace(",", "."), NumberStyles.Number, CurrentDataCulture, returnSingle) Then
            Return returnSingle.RoundSingle(decimalDigits)
        End If
        If Single.TryParse(valueString, NumberStyles.Number, CurrentUICulture, returnSingle) Then
            Return returnSingle.RoundSingle(decimalDigits)
        End If
        Return Single.NaN
    End Function

    <Extension>
    Public Function RoundTo025(originalValue As Single) As Decimal
        Return CDec(originalValue).RoundTo025()
    End Function

    <Extension>
    Public Function RoundTo025(originalValue As Decimal) As Decimal
        Return Math.Floor(Math.Round(originalValue, 3, MidpointRounding.ToZero) / CDec(0.025)) * CDec(0.025)
    End Function

    <Extension>
    Public Function TryParseSingle(valueString As String, ByRef result As Single, Optional decimalDigits As Integer = 10, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Boolean
        If valueString.Contains(","c) AndAlso valueString.Contains("."c) Then
            Throw New Exception($"{NameOf(valueString)} = {valueString}, and contains both comma and period in Line {sourceLineNumber} in {memberName}.")
        End If

        If Single.TryParse(valueString.Replace(",", "."), NumberStyles.Number, usDataCulture, result) Then
            result = result.RoundSingle(decimalDigits)
            Return True
        End If
        If Single.TryParse(valueString, NumberStyles.Number, CurrentUICulture, result) Then
            result = result.RoundSingle(decimalDigits)
            Return True
        End If
        result = Single.NaN
        Return False
    End Function

End Module
