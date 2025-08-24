' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.CodeAnalysis

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class PostalRecord
    Private ReadOnly _asList As New Dictionary(Of String, String)

    Public Sub New(<StringSyntax(StringSyntaxAttribute.Json)> jsonData As String)
        _asList = JsonToDictionary(json:=jsonData)
        If _asList.Keys.Count <> 2 Then
            Dim message As String =
                $"{NameOf(PostalRecord)}({NameOf(jsonData)}) contains {jsonData.Length} " &
                 "entries, 2 expected."
            Dim innerException As New ApplicationException(
                                message:="Invalid postal record structure.")
            Throw New ApplicationException(message, innerException)
        End If

        Me.postalFormat = _asList(NameOf(postalFormat))
        Me.regExpStr = _asList(NameOf(regExpStr))

    End Sub

    <StringSyntax(StringSyntaxAttribute.Regex)>
    Private ReadOnly Property regExpStr As String

    Public Property postalFormat As String

    Private Function GetDebuggerDisplay() As String
        Return $"{NameOf(postalFormat)} = {Me.postalFormat}, " &
               $"{NameOf(regExpStr)} = {Me.regExpStr}"
    End Function

    Public Function ToList() As Dictionary(Of String, String)
        Return _asList
    End Function

End Class
