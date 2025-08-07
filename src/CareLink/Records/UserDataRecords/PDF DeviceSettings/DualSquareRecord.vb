' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class DualSquareRecord

    Public Sub New(line As String)
        If String.IsNullOrWhiteSpace(value:=line) Then
            Me.Dual = "Off"
            Me.Square = "Off"
            Exit Sub
        End If
        Dim values() As String = line.Split(separator:="/")
        Me.Dual = values(0)
        Me.Square = values(1)
    End Sub

    Public Property Dual As String = "Off"
    Public Property Square As String = "Off"

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

    Public Overrides Function ToString() As String
        Return $"{Me.Dual}/{Me.Square}"
    End Function

End Class
