' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class RaceRecord

    <DisplayName("Base")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    <JsonPropertyName("base")>
    Public Property Base As String

    <DisplayName("extra")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("extra")>
    Public Property Extra As String

    Public Sub New(jsonDictionary As Dictionary(Of String, String))
        For Each e As KeyValuePair(Of String, String) In jsonDictionary
            Select Case e.Key
                Case NameOf(Base)
                    Me.Base = e.Value
                Case NameOf(Extra)
                    Me.Extra = e.Value
                Case Else
                    Stop
            End Select
        Next
    End Sub

    Public Overrides Function ToString() As String
        Return $"{NameOf(Base)} = '{Me.Base}', {NameOf(Extra)} = '{Me.Extra}'"
    End Function

End Class
