' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class LanguageRecord

    Public Sub New(values As Dictionary(Of String, String))
        If values.Count <> 2 Then
            Dim message As String = $"{NameOf(LanguageRecord)}({values}) contains {values.Count} entries."
            Throw New ApplicationException(
                message,
                innerException:=New ApplicationException(message:="Invalid Language record structure."))
        End If
        Me.name = values(key:=NameOf(name))
        Me.code = values(key:=NameOf(code))
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
