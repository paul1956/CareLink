' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LanguageRecord

    Public Sub New(values As Dictionary(Of String, String))
        If values.Count <> 2 Then
            Throw New ApplicationException(
                message:=$"{NameOf(LanguageRecord)}({values}) contains {values.Count} entries.",
                innerException:=New ApplicationException("Invalid Language record structure."))
        End If
        Me.name = values(NameOf(name))
        Me.code = values(NameOf(code))
    End Sub

    Public Property code As String
    Public Property name As String

    Private Function GetDebuggerDisplay() As String
        Return $"{NameOf(name)} = {Me.name}, {NameOf(code)} = {Me.code}"
    End Function

    Public Function GetCsvKeys() As String
        Return $"{NameOf(name)}, {NameOf(code)}"
    End Function

    Public Function GetCsvValues() As String
        Return $"{Me.name}, {Me.code}"
    End Function

End Class
