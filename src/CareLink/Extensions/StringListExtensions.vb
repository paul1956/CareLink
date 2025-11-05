' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module StringListExtensions

    ''' <summary>
    '''  Tries to find a string in the list that <paramref name="headerText"/>
    '''  starts with, ignoring case.
    ''' </summary>
    ''' <param name="list">The list of strings to search.</param>
    ''' <param name="headerText">The header string to check against the list.</param>
    ''' <param name="result">
    '''  When this method returns, contains the matching string if found;
    '''  otherwise, result is unmodified.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if a matching string is found;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Friend Function TryGetPrefixMatch(list As List(Of String), headerText As String, ByRef result As String) As Boolean
        For Each value As String In list
            If headerText.StartsWithNoCase(value) Then
                result = value
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    '''  Finds the first line in the list that contains the specified target string.
    ''' </summary>
    ''' <param name="lines">The list of lines represented as Strings to search.</param>
    ''' <param name="value">The string to search for within each line.</param>
    ''' <returns>
    '''  The first line containing the target string,
    '''  or an empty string if no such line is found.
    ''' </returns>
    <Extension>
    Public Function FindLine(lines As List(Of String), value As String) As String
        Dim index As Integer = FindLineNumber(lines, value)
        Return If(index >= 0,
                  lines(index),
                  "")
    End Function

    ''' <summary>
    '''  Finds the index of the first line in the <see cref="List"/> that contains
    '''  the specified <paramref name="value"/> <see langword="String"/>.
    ''' </summary>
    ''' <param name="lines">The list of strings to search.</param>
    ''' <param name="value">The string to search for within each line.</param>
    ''' <returns>
    '''  The zero-based index of the first line containing the target string;
    '''  otherwise, -1 if not found.
    ''' </returns>
    <Extension>
    Public Function FindLineNumber(lines As List(Of String), value As String) As Integer
        For Each e As IndexClass(Of String) In lines.WithIndex
            Dim line As String = e.Value
            If line.Contains(value) Then
                Return e.Index
            End If
        Next
        Return -1
    End Function

End Module
