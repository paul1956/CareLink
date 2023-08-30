' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module StringListExtensions

    <Extension>
    Public Function FindLineContaining(lines As List(Of String), target As String) As String
        For Each line As String In lines
            If line.Contains(target) Then
                Return line
            End If
        Next
        Return ""
    End Function

End Module
