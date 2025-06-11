' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Friend Module StringBuilderExtensions

    ''' <summary>
    '''  Removes the specified string <paramref name="trimString"/> from
    '''  the end of the <see cref="StringBuilder"/> instance if it exists.
    ''' </summary>
    ''' <param name="sb">
    '''  The StringBuilder instance to trim.
    ''' </param>
    ''' <param name="trimString">
    '''  The string to remove from the end of the StringBuilder.
    ''' </param>
    ''' <returns>
    '''  The modified <see cref="StringBuilder"/> instance with the specified string removed from the end, if present.
    ''' </returns>
    <Extension>
    Public Function TrimEnd(sb As StringBuilder, trimString As String) As StringBuilder
        If sb Is Nothing OrElse sb.Length = 0 OrElse String.IsNullOrEmpty(trimString) Then Return sb
        Dim endIndex As Integer = sb.Length - trimString.Length
        If endIndex >= 0 AndAlso sb.ToString(startIndex:=endIndex, trimString.Length) = trimString Then
            sb.Remove(startIndex:=endIndex, trimString.Length)
        End If
        Return sb
    End Function

    ''' <summary>
    '''  Removes the specified character from the end of the <see cref="StringBuilder"/> instance if it exists.
    ''' </summary>
    ''' <param name="sb">
    '''  The StringBuilder instance to trim.
    ''' </param>
    ''' <param name="charToTrim">
    '''  The character to remove from the end of the StringBuilder.
    ''' </param>
    ''' <returns>
    '''  The modified StringBuilder instance with the specified character removed from the end, if present.
    ''' </returns>
    <Extension>
    Public Function TrimEnd(sb As StringBuilder, charToTrim As Char) As StringBuilder
        ' Check if the last character is the one to trim
        If sb.Length > 0 AndAlso sb(sb.Length - 1) = charToTrim Then
            sb.Remove(startIndex:=sb.Length - 1, length:=1)
        End If
        Return sb
    End Function

End Module
