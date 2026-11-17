' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Friend Module StringBuilderExtensions

    ''' <summary>
    '''  Removes the specified string <paramref name="value"/> from
    '''  the end of the <see cref="StringBuilder"/> instance if it exists.
    ''' </summary>
    ''' <param name="sb">
    '''  The StringBuilder instance to trim.
    ''' </param>
    ''' <param name="value">
    '''  The string to remove from the end of the StringBuilder.
    ''' </param>
    ''' <returns>
    '''  The modified <see cref="StringBuilder"/> instance with the
    '''  specified string removed from the end, if present.
    ''' </returns>
    <Extension>
    Public Function TrimEnd(sb As StringBuilder, value As String) As StringBuilder
        If sb Is Nothing OrElse sb.Length = 0 OrElse String.IsNullOrEmpty(value) Then
            Return sb
        End If

        Dim endIndex As Integer = sb.Length - value.Length
        If endIndex >= 0 Then
            Dim matched As Boolean = True
            For i As Integer = 0 To value.Length - 1
                If sb(index:=endIndex + i) <> value(index:=i) Then
                    matched = False
                    Exit For
                End If
            Next

            If matched Then
                sb.Remove(startIndex:=endIndex, value.Length)
            End If
        End If

        Return sb
    End Function

    ' <summary>
    '''  Removes all occurrences of the specified character <paramref name="trimChar"/>
    '''  from the end of the <see cref="StringBuilder"/> instance.
    ''' </summary>
    ''' <param name="sb">
    '''  The StringBuilder instance to trim.
    ''' </param>
    ''' <param name="trimChar">
    '''  The character to remove from the end of the StringBuilder.
    ''' </param>
    ''' <returns>
    '''  The modified <see cref="StringBuilder"/> instance with
    '''  the specified character removed from the end.
    ''' </returns>
    <Extension>
    Public Function TrimEnd(sb As StringBuilder, trimChar As Char) As StringBuilder
        Dim i As Integer = sb.Length - 1
        While i >= 0 AndAlso sb(index:=i) = trimChar
            i -= 1
        End While
        If i < sb.Length - 1 Then
            sb.Length = i + 1
        End If
        Return sb
    End Function

End Module
