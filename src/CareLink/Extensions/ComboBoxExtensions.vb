' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ComboBoxExtensions

    <Extension>
    Public Function GetKey(Of Tk, Tv)(kvp As KeyValuePair(Of Tk, Tv)) As Tk
        Return CType(kvp.Key, Tk)
    End Function

    <Extension>
    Public Function GetValue(Of Tk, Tv)(kvp As KeyValuePair(Of Tk, Tv)) As Tv
        Return CType(kvp.Value, Tv)
    End Function

    <Extension>
    Public Function IndexOfKey(Of Tk, Tv)(objectCollection As ComboBox.ObjectCollection, key As Tk) As Integer
        If String.IsNullOrWhiteSpace(key?.ToString) Then
            Return -1
        End If

        For i As Integer = 0 To objectCollection.Count - 1
            Dim item As KeyValuePair(Of Tk, Tv) = CType(objectCollection(i), KeyValuePair(Of Tk, Tv))
            If item.Key.Equals(key) Then
                Return i
            End If
        Next
        Return -1
    End Function

    <Extension>
    Public Function IndexOfValue(Of Tk, Tv)(objectCollection As ComboBox.ObjectCollection, value As Tv) As Integer
        If String.IsNullOrWhiteSpace(value?.ToString) Then
            Return -1
        End If
        For i As Integer = 0 To objectCollection.Count - 1
            Dim item As KeyValuePair(Of Tk, Tv) = CType(objectCollection(i), KeyValuePair(Of Tk, Tv))
            If item.Value.Equals(value) Then
                Return i
            End If
        Next
        Return -1
    End Function

End Module
