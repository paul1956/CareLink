' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Friend Module StringBuilderExtensions

    <Extension>
    Public Function TrimEnd(sb As StringBuilder, trimString As String) As StringBuilder
        Dim value As String = sb.ToString.TrimEnd(trimString)
        sb.Clear()
        Return sb.Append(value)
    End Function

    <Extension>
    Public Function TrimEnd(sb As StringBuilder, trimString As Char) As StringBuilder
        Dim value As String = sb.ToString.TrimEnd(trimString)
        sb.Clear()
        Return sb.Append(value)
    End Function

End Module
