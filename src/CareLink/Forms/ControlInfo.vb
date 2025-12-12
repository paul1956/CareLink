' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class ControlInfo
    Public Property ParentName As String
    Public Property ControlName As String
    Public Property X As Integer
    Public Property Y As Integer
    Public Property Width As Integer
    Public Property Height As Integer
    Public Property AnchorStyle As String
    Public Property BorderStyle As String

    Public Shared Function FormatAnchor(a As AnchorStyles) As String
        If a = AnchorStyles.None Then
            Return ""
        End If

        Dim parts As New List(Of String)

        If (a And AnchorStyles.Left) = AnchorStyles.Left Then
            parts.Add("Left")
        End If
        If (a And AnchorStyles.Right) = AnchorStyles.Right Then
            parts.Add("Right")
        End If
        If (a And AnchorStyles.Top) = AnchorStyles.Top Then
            parts.Add("Top")
        End If
        If (a And AnchorStyles.Bottom) = AnchorStyles.Bottom Then
            parts.Add("Bottom")
        End If

        Return String.Join(" or ", parts)
    End Function
End Class
