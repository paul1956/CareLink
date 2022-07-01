' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module MathExtensions

    Public Enum RoundTo
        Second
        Minute
        Hour
        Day
    End Enum

    <Extension>
    Friend Function GetDecimalValue(item As Dictionary(Of String, String), ParamArray values() As String) As Double
        Dim returnValueString As String = ""
        For Each value As String In values
            If item.TryGetValue(value, returnValueString) Then
                Return Double.Parse(returnValueString)
            End If
        Next
        Return Double.NaN
    End Function

    <Extension>
    Friend Function RoundDouble(value As Double, decimalDigits As Integer) As Double

        Return Math.Round(value, decimalDigits)
    End Function

    <Extension>
    Friend Function RoundDouble(value As String, decimalDigits As Integer) As Double

        Return Math.Round(Double.Parse(value), decimalDigits)
    End Function

    <Extension>
    Friend Function RoundSingle(value As Double, decimalDigits As Integer) As Single

        Return CSng(Math.Round(value, decimalDigits))
    End Function

    <Extension>
    Public Function RoundDown(d As Date, rt As RoundTo) As Date
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
