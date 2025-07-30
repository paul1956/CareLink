' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DataRowExtensions

    ''' <summary>
    '''  Returns a new string in which all occurrences of the single quote character in the current instance
    '''  are replaced with a back-tick character.
    ''' </summary>
    Private Function EscapeSingleQuotes(Input As String) As String
        Return Input.Replace(oldChar:="'"c, newChar:="`"c) ' Replace with back-tick
    End Function

    ''' <summary>
    '''  Enumerates a collection as delimited collection of strings.
    ''' </summary>
    ''' <typeparam name="T">The Type of the collection.</typeparam>
    ''' <param name="Collection">An Enumerator to a collection to populate the string.</param>
    ''' <param name="Prefix">The string to prefix the result.</param>
    ''' <param name="Delimiter">The string that will appear between each item in the specified collection.</param>
    ''' <param name="Postfix">The string to postfix the result.</param>
    <Extension>
    Private Function ToDelimitedString(Of T)(Collection As IEnumerable(Of T), Prefix As String, Delimiter As String, Postfix As String) As String
        If Collection Is Nothing OrElse Not Collection.Any() Then
            Return String.Empty
        End If

        Dim result As New StringBuilder()
        For Each item As T In Collection
            If result.Length <> 0 Then
                result.Append(value:=Delimiter) ' Add comma
            End If

            result.Append(value:=EscapeSingleQuotes(Input:=TryCast(item, String)))
        Next item
        If result.Length < 1 Then
            Return String.Empty
        End If

        result.Insert(index:=0, value:=Prefix)
        result.Append(value:=Postfix)

        Return result.ToString()
    End Function

End Module
