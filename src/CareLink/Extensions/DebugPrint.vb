' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

''' <summary>
'''  Provides debugging support methods for printing debug information
'''  and formatted URLs.
''' </summary>
Friend Module DebugSupport

    ''' <summary>
    '''  Prints a debug message to the output window,
    '''  including the calling member name and line number.
    '''  If the message starts with '(', only the member name is prepended.
    ''' </summary>
    ''' <param name="message">The message to print.</param>
    ''' <param name="memberName">
    '''  The name of the calling member. Automatically supplied by the compiler.
    ''' </param>
    ''' <param name="sourceLineNumber">
    '''  The line number in the source file at which the method is called.
    '''  Automatically supplied by the compiler.
    ''' </param>
    Public Sub DebugPrint(
            message As String,
            <CallerMemberName> Optional memberName As String = "",
            <CallerLineNumber> Optional sourceLineNumber As Integer = 0)

        If message.StartsWith(value:="("c) Then
            Debug.Print(message:=$"{memberName}{message}")
        Else

            Debug.Print(message:=$"Function:{memberName} Line:{sourceLineNumber,4} {message}")
        End If
    End Sub

    ''' <summary>
    '''  Prints a URL and its query parameters in a formatted way to the debug output.
    '''  Long parameter values are split into multiple lines for readability.
    ''' </summary>
    ''' <param name="label">A label to prefix the URL output.</param>
    ''' <param name="s">The URL string to print.</param>
    ''' <param name="width">The maximum width for splitting long parameter values.</param>
    Public Sub DebugPrintUrl(label As String, s As String, width As Integer)
        Dim lines() As String = s.Split(separator:="?"c)
        Debug.Print(message:=$"{label} = {lines(0)}?")
        Dim message As String
        If lines.Length > 1 Then
            Dim params() As String = lines(1).Split(separator:="&"c)
            For Each l As String In params
                lines = l.Split(separator:="="c)
                If lines(1).Length < 100 Then
                    message = $"{Space(Number:=label.Length + 8)}{lines(0)} = {lines(1)}"
                    Debug.Print(message)
                Else
                    Dim mc As MatchCollection = Regex.Matches(input:=lines(1), pattern:=$".{{1,{width}}}")
                    For Each e As IndexClass(Of Match) In mc.WithIndex
                        Dim m As Match = e.Value
                        Dim spaces As String
                        If e.IsFirst Then
                            spaces = Space(Number:=label.Length + 8)
                            message = $"{spaces}{lines(0)} = {m.Value}"
                            Debug.Print(message)
                        Else
                            spaces = Space(Number:=label.Length + lines(0).Length + 11)
                            Debug.Print(message:=$"{spaces}{m.Value}")
                        End If
                    Next
                End If
            Next
        End If
    End Sub

End Module
