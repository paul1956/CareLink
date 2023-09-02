' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module StringListExtensions

    <Extension>
    Public Function FindLineContaining(lines As List(Of String), target As String) As String
        Dim index As Integer = FindLineNumberContaining(lines, target)
        Return If(index >= 0, lines(index), "")
    End Function

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
