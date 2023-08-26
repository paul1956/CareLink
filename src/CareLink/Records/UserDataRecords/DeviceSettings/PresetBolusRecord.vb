' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetBolusRecord

    Public Sub New()
    End Sub

    Public Sub New(line As String)
        If String.IsNullOrWhiteSpace(line) Then
            Exit Sub
        End If
        Stop
        'Me.Normal = Normal
        'Me.Square = Square
        Me.IsValid = True
    End Sub

    Private Shared ReadOnly Property ColumnTitles As New List(Of String) From {
                        {"Name"},
                        {NameOf(Normal)},
                        {NameOf(Square)}
                    }

    Public Property IsValid As Boolean = False

    Public Property Normal As String = ""

    Public Property Square As String = ""

    Public Shared Function GetColumnTitle() As String
        Return ColumnTitles.ToArray.JoinLines(" ")
    End Function

End Class
