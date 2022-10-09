' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class SummaryRecord

    Protected Friend Sub New(index As ItemIndexs, entry As KeyValuePair(Of String, String))
        Me.RecordNumber = index
        Me.Key = entry.Key
        Me.Value = entry.Value?.ToString(CurrentUICulture)
    End Sub

    Protected Friend Sub New(index As ItemIndexs, key As String, value As String)
        Me.RecordNumber = index
        Me.Key = key
        Me.Value = value
    End Sub

    <DisplayName(NameOf(Key))>
    <Column(Order:=1)>
    Public Property Key As String

    <DisplayName(NameOf(RecordNumber))>
    <Column(Order:=0)>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(Value))>
    <Column(Order:=2)>
    Public Property Value As String

End Class
