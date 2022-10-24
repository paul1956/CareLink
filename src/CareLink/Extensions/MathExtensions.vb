' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    <Extension>
    Friend Function GetSingleValue(item As Dictionary(Of String, String), value As String) As Single
        Dim returnString As String = ""
        If item.TryGetValue(value, returnString) Then
            Return returnString.ParseSingle
        End If
        Return Single.NaN
    End Function

    <Extension>
    Friend Function RoundSingle(singleValue As Single, decimalDigits As Integer) As Single

        Return CSng(Math.Round(singleValue, decimalDigits))
    End Function

    <Extension>
    Friend Function RoundToSingle(doubleValue As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(doubleValue, decimalDigits))
    End Function

    <Extension>
    Public Function IsEven(num As Integer) As Boolean
        Return num Mod 2 = 0
    End Function

    <Extension>
    Public Function ParseSingle(valueString As String, Optional decimalDigits As Integer = 10) As Single
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
    Public Function TryParseSingle(valueString As String, ByRef result As Single, Optional decimalDigits As Integer = 10, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Boolean
        If valueString.Contains(","c) AndAlso valueString.Contains("."c) Then
            Throw New Exception($"{NameOf(valueString)} = {valueString}, and contains both comma and period in Line {sourceLineNumber} in {memberName}.")
        End If

        If Single.TryParse(valueString.Replace(",", "."), NumberStyles.Number, CurrentDataCulture, result) Then
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
