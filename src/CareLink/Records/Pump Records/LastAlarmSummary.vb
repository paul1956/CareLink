' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LastAlarmSummary

    Protected Friend Sub New(kvp As KeyValuePair(Of String, String), Index As Integer)
        Me.RecordNumber = Index
        Me.Key = GetDisplayName(kvp.Key)
        Me.Value = kvp.Value
        Me.Message = ""
    End Sub

    <DisplayName(NameOf(Key))>
    <Column(Order:=1)>
    Public Property Key As String

    <DisplayName(NameOf(Message))>
    <Column(Order:=3)>
    Public Property Message As String = ""

    <DisplayName("Record Number")>
    <Column(Order:=0)>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(Value))>
    <Column(Order:=2)>
    Public Property Value As String
End Class
