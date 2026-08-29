' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class ActiveInsulin

    Private _amount As Single
    Private _dateTimeAsString As String

    <Column(Order:=0, TypeName:=NameOf([Single]))>
    <DisplayName("Amount")>
    <JsonPropertyName("amount")>
    Public Property Amount As Single
        Get
            Return _amount
        End Get
        Set
            _amount = Value.RoundToSingle(digits:=3)
        End Set
    End Property

    <DisplayName("Date with Time From Pump")>
    <Column(Order:=1, TypeName:="String")>
    <JsonPropertyName("datetime")>
    Public Property DateTimeAsString As String
        Get
            Return If(_dateTimeAsString, String.Empty)
        End Get
        Set
            _dateTimeAsString = Value
        End Set
    End Property

    <DisplayName("Date with Time As Date")>
    <Column(Order:=2, TypeName:="DateTime")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property DateTime As Date
        Get
            Return If(IsNullOrWhiteSpace(value:=Me.DateTimeAsString),
                      Now,
                      TryParseDateStr(s:=Me.DateTimeAsString))
        End Get
    End Property

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property kind As String

    <DisplayName("Precision")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    <JsonPropertyName("precision")>
    Public Property Precision As String

    <DisplayName("Version")>
    <Column(Order:=5, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("version")>
    Public Property Version As Integer

End Class
