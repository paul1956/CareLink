' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Friend Module DebugSupport

    Public Sub DebugPrint(message As String, <CallerMemberName> Optional memberName As String = "", <CallerLineNumber> Optional sourceLineNumber As Integer = 0)
        If message.StartsWith("("c) Then
            Debug.Print($"{memberName}{message}")
        Else
            Debug.Print($"Function:{memberName} Line:{sourceLineNumber,4} {message}")
        End If
    End Sub

    Public Sub DebugPrintUrl(label As String, s As String, width As Integer)
        Dim lines() As String = s.Split("?"(0))
        Debug.Print($"{label} = {lines(0)}?")
        If lines.Length > 1 Then
            Dim params() As String = lines(1).Split("&")
            For Each l As String In params
                lines = l.Split("=")
                If lines(1).Length < 100 Then
                    Debug.Print($"{Space(label.Length + 8)}{lines(0)} = {lines(1)}")
                Else
                    Dim mc As MatchCollection = Regex.Matches(lines(1), $".{{1,{width}}}")
                    For Each e As IndexClass(Of Match) In mc.WithIndex
                        Dim m As Match = e.Value
                        If e.IsFirst Then
                            Debug.Print($"{Space(label.Length + 8)}{lines(0)} = {m.Value}")
                        Else
                            Debug.Print($"{Space(label.Length + lines(0).Length + 11)}{m.Value}")
                        End If
                    Next
                End If
            Next
        End If
    End Sub

End Module
