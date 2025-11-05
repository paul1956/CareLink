' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides extension methods for ComboBox.ObjectCollection to search for keys and values.
''' </summary>
Imports System.Runtime.CompilerServices

Public Module ComboBoxExtensions

    ''' <summary>
    '''  Returns the index of the first occurrence of the specified key
    '''  in the <see cref="ComboBox.ObjectCollection"/>.
    ''' </summary>
    ''' <typeparam name="Tk">The type of the key.</typeparam>
    ''' <typeparam name="Tv">The type of the value.</typeparam>
    ''' <param name="objCollection">The ComboBox.ObjectCollection to search.</param>
    ''' <param name="key">The key to locate in the collection.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of the key within the collection,
    '''  or -1 if not found.
    ''' </returns>
    <Extension>
    Public Function IndexOfKey(Of Tk, Tv)(objCollection As ComboBox.ObjectCollection, key As Tk) As Integer
        Dim count As Integer = objCollection.Count
        If count = 0 Then
            Return -1
        End If

        Dim comparer As EqualityComparer(Of Tk) = EqualityComparer(Of Tk).Default
        For i As Integer = 0 To count - 1
            Dim obj As Object = objCollection(index:=i)
            If TypeOf obj Is KeyValuePair(Of Tk, Tv) Then
                Dim pair As KeyValuePair(Of Tk, Tv) = DirectCast(obj, KeyValuePair(Of Tk, Tv))
                If comparer.Equals(pair.Key, key) Then
                    Return i
                End If
            End If
        Next
        Return -1
    End Function

    ''' <summary>
    '''  Returns the index of the first occurrence of the specified
    '''  value in the <see cref="ComboBox.ObjectCollection"/>.
    ''' </summary>
    ''' <typeparam name="Tk">The type of the key.</typeparam>
    ''' <typeparam name="Tv">The type of the value.</typeparam>
    ''' <param name="objCollection">The ComboBox.ObjectCollection to search.</param>
    ''' <param name="y">The value to locate in the collection.</param>
    ''' <returns>
    '''  The zero-based index of the first occurrence of the value within the collection;
    '''  otherwise -1 if not found.
    ''' </returns>
    <Extension>
    Public Function IndexOfY(Of Tk, Tv)(objCollection As ComboBox.ObjectCollection, y As Tv) As Integer
        ' If valueObject is Nothing and Tv is a class type, return -1 early
        If y Is Nothing AndAlso Not GetType(Tv).IsValueType Then
            Return -1
        End If

        Dim count As Integer = objCollection.Count
        If count = 0 Then
            Return -1
        End If

        Dim comparer As EqualityComparer(Of Tv) = EqualityComparer(Of Tv).Default
        For index As Integer = 0 To count - 1
            Dim obj As Object = objCollection(index)
            If TypeOf obj Is KeyValuePair(Of Tk, Tv) Then
                Dim item As KeyValuePair(Of Tk, Tv) = DirectCast(obj, KeyValuePair(Of Tk, Tv))
                If comparer.Equals(x:=item.Value, y) Then
                    Return index
                End If
            End If
        Next
        Return -1
    End Function

End Module
