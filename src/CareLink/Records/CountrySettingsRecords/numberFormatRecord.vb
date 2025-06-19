﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class NumberFormatRecord
    Private ReadOnly _asList As New Dictionary(Of String, String)

    Public Sub New(jsonData As String)
        _asList = JsonToDictionary(jsonData)
        If _asList.Count <> 2 Then
            Throw New ApplicationException(
                message:=$"{NameOf(NumberFormatRecord)}({NameOf(jsonData)}) contains {jsonData.Length} entries.",
                innerException:=New ApplicationException("Invalid number format record structure."))
        End If
        Me.decimalSeparator = _asList(NameOf(decimalSeparator))
        Me.groupsSeparator = _asList(NameOf(groupsSeparator))
    End Sub

    Public Property decimalSeparator As String

    Public Property groupsSeparator As String

    Private Function GetDebuggerDisplay() As String
        Return $"{NameOf(decimalSeparator)} = {Me.decimalSeparator}, {NameOf(groupsSeparator)} = {Me.groupsSeparator}"
    End Function

    Public Function ToList() As Dictionary(Of String, String)
        Return _asList
    End Function

End Class
