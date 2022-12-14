' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class RaceRecord
    <DisplayName("Base")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property base As String

    <DisplayName("extra")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property extra As String

    Public Sub New(jsonDictionary As Dictionary(Of String, String))
        For Each e As KeyValuePair(Of String, String) In jsonDictionary
            Select Case e.Key
                Case NameOf(base)
                    Me.base = e.Value
                Case NameOf(extra)
                    Me.extra = e.Value
                Case Else
                    Stop
            End Select
        Next
    End Sub

    Public Overrides Function ToString() As String
        Return $"{NameOf(base)} = '{Me.base}', {NameOf(extra)} = '{Me.extra}'"
    End Function

End Class
