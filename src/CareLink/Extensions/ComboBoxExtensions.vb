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
    ''' <param name="keyObject">The key to locate in the collection.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of the key within the collection, or -1 if not found.
    ''' </returns>
    <Extension>
    Public Function IndexOfKey(Of Tk, Tv)(objectCollection As ComboBox.ObjectCollection, keyObject As Tk) As Integer
        For i As Integer = 0 To objectCollection.Count - 1
            Dim obj As Object = objectCollection(index:=i)
            If TypeOf obj Is KeyValuePair(Of Tk, Tv) AndAlso
               Equals(CType(obj, KeyValuePair(Of Tk, Tv)).Key, keyObject) Then
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
    ''' <param name="valueObject">The value to locate in the collection.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of the value within the collection, or -1 if not found.
    ''' </returns>
    <Extension>
    Public Function IndexOfValue(Of Tk, Tv)(objectCollection As ComboBox.ObjectCollection, valueObject As Tv) As Integer
        ' If valueObject is Nothing and Tv is a class type, return -1 early
        If valueObject Is Nothing Then
            Return -1
        End If

        For i As Integer = 0 To objectCollection.Count - 1
            Dim item As KeyValuePair(Of Tk, Tv) = DirectCast(objectCollection(index:=i), KeyValuePair(Of Tk, Tv))
            If EqualityComparer(Of Tv).Default.Equals(item.Value, valueObject) Then
                Return i
            End If
        Next
        Return -1
    End Function

End Module
