' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    Public Enum RoundTo
        Second
        Minute
        Hour
        Day
    End Enum

    <Extension>
    Friend Function GetDoubleValue(item As Dictionary(Of String, String), value As String) As Double
        Dim returnValueString As String = ""
        If item.TryGetValue(value, returnValueString) Then
            Return returnValueString.ParseDouble
        End If
        Return Double.NaN
    End Function

    <Extension>
    Friend Function RoundDouble(doubleValue As Double, decimalDigits As Integer) As Double

        Return Math.Round(doubleValue, decimalDigits)
    End Function

    <Extension>
    Friend Function RoundDouble(value As String, decimalDigits As Integer) As Double

        Return Math.Round(value.ParseDouble, decimalDigits)
    End Function

    <Extension>
    Friend Function RoundSingle(doubleValue As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(doubleValue, decimalDigits))
    End Function

    <Extension>
    Friend Function RoundSingle(singleValue As Single, decimalDigits As Integer) As Single

        Return CSng(Math.Round(singleValue, decimalDigits))
    End Function

    <Extension>
    Public Function ParseDouble(valueString As String) As Double
        Dim returnDouble As Double
        If Double.TryParse(valueString, NumberStyles.Number, CurrentDataCulture, returnDouble) Then
            Return returnDouble
        End If
        If Double.TryParse(valueString, NumberStyles.Number, CurrentUICulture, returnDouble) Then
            Return returnDouble
        End If
        Return Double.NaN
    End Function

    <Extension>
    Public Function ParseSingle(valueString As String) As Single
        Dim returnSingle As Single
        If Single.TryParse(valueString, NumberStyles.Number, CurrentDataCulture, returnSingle) Then
            Return returnSingle
        End If
        If Single.TryParse(valueString, NumberStyles.Number, CurrentUICulture, returnSingle) Then
            Return returnSingle
        End If
        Return Single.NaN
    End Function

    <Extension>
    Public Function RoundTimeDown(d As Date, rt As RoundTo) As Date
        Dim dtRounded As New DateTime()

        Select Case rt
            Case RoundTo.Second
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second)
            Case RoundTo.Minute
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0)
            Case RoundTo.Hour
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0)
            Case RoundTo.Day
                dtRounded = New DateTime(d.Year, d.Month, d.Day, 0, 0, 0)
        End Select

        Return dtRounded
    End Function

End Module
