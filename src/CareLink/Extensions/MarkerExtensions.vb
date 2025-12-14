' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module MarkerExtensions

    ''' <summary>
    '''  Retrieves a <see langword="Boolean"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="Boolean"/> value if found;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    <Extension>
    Public Function GetBoolean(item As Marker, key As String) As Boolean
        Dim value As Object = Nothing
        Dim result As Boolean = False
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.True
                    Return True
                Case JsonValueKind.False
                    Return False
                Case Else
                    Stop
            End Select
        Else
            Return Nothing
        End If
        Return result
    End Function

    ''' <summary>
    '''  Retrieves a <see langword="Double"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="Double"/> value if found;
    '''  otherwise, <see cref="Double.NaN"/>.
    ''' </returns>
    <Extension>
    Public Function GetDouble(item As Marker, key As String) As Double
        Dim result As Double = Double.NaN
        Dim value As Object = Nothing
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    result = element.GetString.ParseDoubleInvariant
                Case JsonValueKind.Number
                    result = element.GetDouble
                Case Else
                    Stop
            End Select
        Else
            Stop
        End If
        Return result
    End Function

    ''' <summary>
    '''  Retrieves an <see langword="Integer"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="Integer"/> value if found;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    <Extension>
    Public Function GetInteger(item As Marker, key As String) As Integer
        Dim value As Object = Nothing
        Dim result As Integer = 0
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    result = Integer.Parse(element.GetString)
                Case JsonValueKind.Number
                    result = element.GetInt32
                Case Else
                    Stop
            End Select
        Else
            Return Nothing
        End If
        Return result
    End Function

    ''' <summary>
    '''  Retrieves a <see langword="Single"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    '''  Optionally rounds the value to a specified number of <paramref name="digits"/>.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <param name="digits">
    '''  The number of decimal digits to round to. Use -1 for no rounding.
    ''' </param>
    ''' <param name="considerValue">
    '''  Whether to consider the value when rounding.
    ''' </param>
    ''' <returns>
    '''  The <see langword="Single"/> value if found;
    '''  otherwise, <see cref="Single.NaN"/>.
    ''' </returns>
    <Extension>
    Public Function GetSingle(item As Marker,
                              key As String,
                              Optional digits As Integer = -1,
                              Optional considerValue As Boolean = False) As Single

        Dim value As Object = Nothing
        Dim result As Single = Single.NaN
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Select Case True
                Case TypeOf value Is JsonElement
                    Dim element As JsonElement = CType(value, JsonElement)
                    Select Case element.ValueKind
                        Case JsonValueKind.String
                            result = element.GetString.ParseSingleInvariant
                        Case JsonValueKind.Number
                            result = element.GetSingle
                        Case Else
                            Stop
                            Return result
                    End Select
                Case TypeOf value Is String
                    result = CStr(value).ParseSingleInvariant
                Case Else
                    Stop
            End Select

            If digits = -1 Then Return result
            Return result.RoundToSingle(digits, considerValue)
        Else
            Return Single.NaN
        End If
    End Function

    ''' <summary>
    '''  Retrieves a <see langword="String"/> value from a
    '''  <see cref="Marker"/> entry's JSON data by field name.
    ''' </summary>
    ''' <param name="item">The marker entry containing JSON data.</param>
    ''' <param name="key">The field name to retrieve.</param>
    ''' <returns>
    '''  The <see langword="String"/> value if found;
    '''  otherwise, <see cref="String.Empty"/>.
    ''' </returns>
    <Extension>
    Public Function GetString(item As Marker, key As String) As String
        Dim value As Object = Nothing
        key = key.ToLowerCamelCase
        If item.Data.DataValues.TryGetValue(key, value) Then
            Dim element As JsonElement = CType(value, JsonElement)
            Select Case element.ValueKind
                Case JsonValueKind.String
                    Return element.GetString
                Case JsonValueKind.Undefined
                    Stop
                    Return String.Empty
                Case JsonValueKind.Object
                    Stop
                    Return String.Empty
                Case JsonValueKind.Array
                    Stop
                    Return String.Empty
                Case JsonValueKind.Number
                    Return element.ToString
                Case JsonValueKind.True
                    Return "True"
                Case JsonValueKind.False
                    Return "False"
                Case JsonValueKind.Null
                    Return String.Empty
            End Select
        End If
        Return String.Empty
    End Function

End Module
