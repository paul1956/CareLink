' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class postalRecord
    Private ReadOnly _asList As New Dictionary(Of String, String)

    Public Sub New(jsonData As String)
        _asList = Loads(jsonData)
        If _asList.Keys.Count <> 2 Then
            Throw New Exception($"{NameOf(postalRecord)}({NameOf(jsonData)}) contains {jsonData.Length} entries, 2 expected.")
        End If

        postalFormat = _asList(NameOf(postalFormat))
        regExpStr = _asList(NameOf(regExpStr))

    End Sub

#If True Then ' Prevent reordering

    Public postalFormat As String
    Public regExpStr As String
#End If  ' Prevent reordering

    Private Function GetDebuggerDisplay() As String
        Return $"{NameOf(postalFormat)} = {postalFormat}, {NameOf(regExpStr)} = {regExpStr}"
    End Function

    Public Function ToList() As Dictionary(Of String, String)
        Return _asList
    End Function

End Class
