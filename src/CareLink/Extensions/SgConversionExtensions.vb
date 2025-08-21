' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module SgConversionExtensions

    ''' <summary>
    '''  Converts a Single value to a string representation,
    '''  scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="value">The Single value to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Private Function ScaleSgToString(value As Single) As String
        Dim digits As Integer = GetPrecisionDigits()
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Return If(NativeMmolL,
                  (value / MmolLUnitsDivisor).RoundToSingle(digits).ToString(provider),
                  value.ToString(provider))
    End Function

    ''' <summary>
    '''  Converts a KeyValuePair to a string representation of the value,
    '''  scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="item">The KeyValuePair to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSgToString(item As KeyValuePair(Of String, Object)) As String
        Dim value As Single = CType(item.Value, JsonElement).ToString.ParseSingle(digits:=2)
        Return ScaleSgToString(value)
    End Function

    ''' <summary>
    '''  Converts a String representation of a value to a
    '''  string representation of the value, scaled according
    '''  to the <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <param name="value">The string representation of the value to convert.</param>
    ''' <returns>A <see langword="String"/> representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSgToString(value As String) As String
        Dim digits As Integer = GetPrecisionDigits()
        Return value.ParseSingle(digits).ScaleSgToString()
    End Function

End Module
