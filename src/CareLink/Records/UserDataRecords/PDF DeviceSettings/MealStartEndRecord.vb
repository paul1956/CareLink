' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MealStartEndRecord

    Public Sub New()
    End Sub

    Public Sub New(line As String)
        If String.IsNullOrWhiteSpace(line) Then
            Stop
            Exit Sub
        End If
        Dim split() As String = line.CleanSpaces.Split(" ")
        Me.Start = split(0)
        Me.End = split(1)
    End Sub

    Public Property Start As String = "Off"
    Public Property [End] As String = "Off"

End Class
