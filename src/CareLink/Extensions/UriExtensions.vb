' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Friend Module UriExtensions

    Private ReadOnly s_reservedCharacters As String = "!*'();:@&=+$,/?%#[]"

    <Extension>
    Public Function UriParameterEncode(value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return String.Empty
        End If

        Dim sb As New StringBuilder()

        For Each c As Char In value
            If Not s_reservedCharacters.Contains(c) Then
                sb.Append(c)
            Else
                sb.AppendFormat("%{0:X2}", AscW(c))
            End If
        Next c
        Return sb.ToString()
    End Function

End Module
