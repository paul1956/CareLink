' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class postalRecord
    Private ReadOnly _asList As New List(Of KeyValuePair(Of String, String))
    Public postalFormat As String
    Public regExpStr As String

    Public Sub New(jsonValue As String)
        Dim values As Dictionary(Of String, String) = Loads(jsonValue)
        If values.Keys.Count <> 2 Then
            Throw New Exception($"{NameOf(postalRecord)}({jsonValue}) contains {values.Count} entries, 2 expected.")
        End If

        postalFormat = values(NameOf(postalFormat))
        _asList.Add(KeyValuePair.Create(NameOf(postalFormat), postalFormat))
        regExpStr = values(NameOf(regExpStr))
        _asList.Add(KeyValuePair.Create(NameOf(regExpStr), regExpStr))

    End Sub

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

    Public Function ToList() As List(Of KeyValuePair(Of String, String))
        Return _asList
    End Function

End Class
