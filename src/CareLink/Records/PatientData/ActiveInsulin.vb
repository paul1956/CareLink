' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class ActiveInsulin
    Private _amount As Single

    <Column(Order:=0, TypeName:=NameOf([Single]))>
    <DisplayName("Amount")>
    <JsonPropertyName("amount")>
    Public Property amount As Single
        Get
            Return _amount
        End Get
        Set
            _amount = Value.RoundSingle(3, False)
        End Set
    End Property

    <DisplayName(NameOf(ActiveInsulin.datetime))>
    <Column(Order:=1, TypeName:="Date")>
    <JsonPropertyName("datetime")>
    Public Property datetime As Date

    <DisplayName(NameOf(OAdatetime))>
    <Column(Order:=2, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdatetime As OADate
        Get
            Return New OADate(Me.datetime)
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
