' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module KeyValuePairExtensions

    <Extension>
    Private Function JsonToSingle(item As KeyValuePair(Of String, Object)) As Single
        Return CType(item.Value, JsonElement).ToString.ParseSingle(2)
    End Function

    <Extension>
    Private Function ScaleSgToString(value As Single) As String
        Return If(NativeMmolL,
                  (value / MmolLUnitsDivisor).RoundSingle(If(NativeMmolL, 2, 0), False).ToString(CurrentUICulture),
                  value.ToString(CurrentUICulture)
                 )
    End Function

    <Extension>
    Public Function ScaleSgToString(item As KeyValuePair(Of String, Object)) As String
        Dim jsonToSingle As Single = item.JsonToSingle
        Return ScaleSgToString(jsonToSingle)
    End Function

    <Extension>
    Public Function ScaleSgToString(item As JsonElement) As String
        Dim itemAsSingle As Single
        Select Case item.ValueKind
            Case JsonValueKind.String
                itemAsSingle = Single.Parse(item.GetString())
            Case JsonValueKind.Null
                Return String.Empty
            Case JsonValueKind.Undefined
                Return String.Empty
            Case JsonValueKind.Number
                itemAsSingle = item.GetSingle
            Case Else
                Stop
        End Select

        Return If(NativeMmolL,
                  (itemAsSingle / MmolLUnitsDivisor).RoundSingle(If(NativeMmolL, 2, 0), False).ToString(CurrentUICulture),
                  itemAsSingle.ToString(CurrentUICulture))
    End Function

    <Extension>
    Public Function ScaleSgToString(item As KeyValuePair(Of String, String)) As String
        Return item.Value.ParseSingle(If(NativeMmolL, 2, 0)).ScaleSgToString()
    End Function

    <Extension>
    Public Function ScaleSgToString(value As String) As String
        Return value.ParseSingle(If(NativeMmolL, 2, 0)).ScaleSgToString()
    End Function

End Module
