' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ForEachExtensions

    <Extension>
    Public Iterator Function WithIndex(Of T)(
                                         source As IEnumerable(Of T)
                                         ) As IEnumerable(Of IndexClass(Of T))
        If source Is Nothing Then
            Throw New ArgumentNullException(NameOf(source))
        End If

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
