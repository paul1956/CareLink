' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class numberFormatRecord
    Private ReadOnly _asList As New List(Of KeyValuePair(Of String, String))
    Public decimalSeparator As String
    Public groupsSeparator As String

    Public Sub New(jsonValue As String)
        Dim values As Dictionary(Of String, String) = Loads(jsonValue)
        If values.Count <> 2 Then
            Throw New Exception($"{NameOf(numberFormatRecord)}({values}) contains {values.Count} entries.")
        End If
        decimalSeparator = values(NameOf(decimalSeparator))
        _asList.Add(KeyValuePair.Create(NameOf(decimalSeparator), decimalSeparator))
        groupsSeparator = values(NameOf(groupsSeparator))
        _asList.Add(KeyValuePair.Create(NameOf(groupsSeparator), groupsSeparator))
    End Sub

    Private Function GetDebuggerDisplay() As String
        Return Me.ToString()
    End Function

    Public Function ToList() As List(Of KeyValuePair(Of String, String))
        Return _asList
    End Function

End Class
