' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides extension methods for ComboBox.ObjectCollection to search for keys and values.
''' </summary>
Imports System.Runtime.CompilerServices

Public Module ComboBoxExtensions

    ''' <summary>
    '''  Returns the index of the first occurrence of the specified key in the ComboBox.ObjectCollection.
    ''' </summary>
    ''' <typeparam name="Tk">The type of the key.</typeparam>
    ''' <typeparam name="Tv">The type of the value.</typeparam>
    ''' <param name="objectCollection">The ComboBox.ObjectCollection to search.</param>
    ''' <param name="key">The key to locate in the collection.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of the key within the collection, or -1 if not found.
    ''' </returns>
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

    ''' <summary>
    '''  Returns the index of the first occurrence of the specified value in the ComboBox.ObjectCollection.
    ''' </summary>
    ''' <typeparam name="Tk">The type of the key.</typeparam>
    ''' <typeparam name="Tv">The type of the value.</typeparam>
    ''' <param name="objectCollection">The ComboBox.ObjectCollection to search.</param>
    ''' <param name="value">The value to locate in the collection.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of the value within the collection, or -1 if not found.
    ''' </returns>
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
