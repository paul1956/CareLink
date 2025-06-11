' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module StringListExtensions

    ''' <summary>
    '''  Finds the first line in the list that contains the specified target string.
    ''' </summary>
    ''' <param name="lines">The list of lines represented as Strings to search.</param>
    ''' <param name="target">The string to search for within each line.</param>
    ''' <returns>
    '''  The first line containing the target string, or an empty string if no such line is found.
    ''' </returns>
    <Extension>
    Public Function FindLineContaining(lines As List(Of String), target As String) As String
        Dim index As Integer = FindLineNumberContaining(lines, target)
        Return If(index >= 0, lines(index), "")
    End Function

    ''' <summary>
    '''  Finds the index of the first line in the <see cref="List"/> that contains
    '''  the specified <paramref name="target"/> <see langword="String"/>.
    ''' </summary>
    ''' <param name="lines">The list of strings to search.</param>
    ''' <param name="target">The string to search for within each line.</param>
    ''' <returns>
    '''  The zero-based index of the first line containing the target string, or -1 if not found.
    ''' </returns>
    <Extension>
    Public Function FindLineNumberContaining(lines As List(Of String), target As String) As Integer
        For Each e As IndexClass(Of String) In lines.WithIndex
            Dim line As String = e.Value
            If line.Contains(target) Then
                Return e.Index
            End If
        Next
        Return -1
    End Function

End Module
