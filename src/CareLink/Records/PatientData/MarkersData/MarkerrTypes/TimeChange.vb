﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class TimeChange

    Public Sub New(item As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = item.Type
        Me.Kind = "Marker"
        Me.TimestampAsString = item.TimestampAsString
        Me.DisplayTimeAsString = item.DisplayTimeAsString
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Kind")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property Kind As String

    <DisplayName("Timestamp")>
    <Column(Order:=3, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.TimestampAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("Display Time")>
    <Column(Order:=5, TypeName:="String")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    <DisplayName("Display Time As Date")>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return Me.DisplayTimeAsString.TryParseDateStr()
        End Get
    End Property

End Class
