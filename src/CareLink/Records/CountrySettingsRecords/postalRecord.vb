' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.CodeAnalysis

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class PostalRecord
    Private ReadOnly _asList As New Dictionary(Of String, String)

    Public postalFormat As String

    <StringSyntax(StringSyntaxAttribute.Regex)>
    Public regExpStr As String

    Public Sub New(<StringSyntax(StringSyntaxAttribute.Json)> jsonData As String)
        _asList = Loads(jsonData)
        If _asList.Keys.Count <> 2 Then
            Throw New Exception($"{NameOf(PostalRecord)}({NameOf(jsonData)}) contains {jsonData.Length} entries, 2 expected.")
        End If

        postalFormat = _asList(NameOf(postalFormat))
        regExpStr = _asList(NameOf(regExpStr))

    End Sub

    Private Function GetDebuggerDisplay() As String
        Return $"{NameOf(postalFormat)} = {postalFormat}, {NameOf(regExpStr)} = {regExpStr}"
    End Function

    Public Function ToList() As Dictionary(Of String, String)
        Return _asList
    End Function

End Class
