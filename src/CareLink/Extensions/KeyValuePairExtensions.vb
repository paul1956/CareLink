' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module KeyValuePairExtensions

    ''' <summary>
    '''  Converts a KeyValuePair to a Single value, extracting the value from a JsonElement.
    ''' </summary>
    ''' <param name="item">The KeyValuePair to convert.</param>
    ''' <returns>A Single value extracted from the JsonElement.</returns>
    ''' <remarks>
    '''  This function is used to convert a KeyValuePair where the value is a JsonElement
    '''  representing a numeric value, typically in the context of glucose readings.
    ''' </remarks>
    <Extension>
    Private Function JsonToSingle(item As KeyValuePair(Of String, Object)) As Single
        Return CType(item.Value, JsonElement).ToString.ParseSingle(digits:=2)
    End Function

    ''' <summary>
    '''  Converts a Single value to a string representation, scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="value">The Single value to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Private Function ScaleSgToString(value As Single) As String
        Return If(NativeMmolL,
                  (value / MmolLUnitsDivisor).RoundSingle(digits:=If(NativeMmolL, 2, 0), considerValue:=False).ToString(Provider),
                  value.ToString(Provider)
                 )
    End Function

    ''' <summary>
    '''  Converts a KeyValuePair to a string representation of the value, scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="item">The KeyValuePair to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSgToString(item As KeyValuePair(Of String, Object)) As String
        Dim jsonToSingle As Single = item.JsonToSingle
        Return ScaleSgToString(jsonToSingle)
    End Function

    ''' <summary>
    '''  Converts a JsonElement to a string representation of the value, scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="item">The JsonElement to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSgToString(item As JsonElement) As String
        Dim itemAsSingle As Single
        Select Case item.ValueKind
            Case JsonValueKind.String
                itemAsSingle = Single.Parse(item.GetString(), Provider)
            Case JsonValueKind.Null
                Return String.Empty
            Case JsonValueKind.Undefined
                Return String.Empty
            Case JsonValueKind.Number
                itemAsSingle = item.GetSingle
            Case Else
                Stop
        End Select

        Dim s As Single = If(
            NativeMmolL,
            (itemAsSingle / MmolLUnitsDivisor).RoundSingle(digits:=If(NativeMmolL, 2, 0), considerValue:=False),
            itemAsSingle)
        Return s.ToString(Provider)
    End Function

    ''' <summary>
    '''  Converts a String representation of a value to a
    '''  string representation of the value, scaled according to the <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <param name="value">The string representation of the value to convert.</param>
    ''' <returns>A <see langword="String"/> representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSgToString(value As String) As String
        Return value.ParseSingle(digits:=If(NativeMmolL, 2, 0)).ScaleSgToString()
    End Function

End Module
