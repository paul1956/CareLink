' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LanguageRecord
    Public code As String
    Public name As String

    Public Sub New(values As Dictionary(Of String, String))
        If values.Count <> 2 Then
            Throw New Exception($"{NameOf(LanguageRecord)}({values}) contains {values.Count} entries.")
        End If
        name = values(NameOf(name))
        code = values(NameOf(code))
    End Sub

    Private Function GetDebuggerDisplay() As String
        Return $"{NameOf(name)} = {name}, {NameOf(code)} = {code}"
    End Function

    Public Function GetCsvKeys() As String
        Return $"{NameOf(name)}, {NameOf(code)}"
    End Function

    Public Function GetCsvValues() As String
        Return $"{name}, {code}"
    End Function

End Class
