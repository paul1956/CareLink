' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for iterating over collections with additional context.
''' </summary>
Public Module ForEachExtensions

    ''' <summary>
    '''  Enumerates a sequence and yields each element wrapped in an <see cref="IndexClass(Of T)"/>,
    '''  which provides access to the element, its index, and enumeration state.
    ''' </summary>
    ''' <typeparam name="T">The type of elements in the source sequence.</typeparam>
    ''' <param name="source">The sequence to enumerate.</param>
    ''' <returns>
    '''  An <see cref="IEnumerable(Of IndexClass(Of T))"/> that yields each element with its index and enumeration context.
    ''' </returns>
    ''' <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <c>Nothing</c>.</exception>
    <Extension>
    Public Iterator Function WithIndex(Of T)(source As IEnumerable(Of T)) As IEnumerable(Of IndexClass(Of T))
        ArgumentNullException.ThrowIfNull(source)

        Using enumerator As IEnumerator(Of T) = source.GetEnumerator
            Dim hasNext As Boolean = enumerator.MoveNext
            Dim index As Integer = -1
            While hasNext
                Dim wi As New IndexClass(Of T) With {.Index = index, .Enumerator = enumerator}
                wi.MoveNext()
                Yield wi
                hasNext = Not wi.IsLast
                index = wi.Index ' if .MoveNext was used
            End While
        End Using
    End Function
End Module
