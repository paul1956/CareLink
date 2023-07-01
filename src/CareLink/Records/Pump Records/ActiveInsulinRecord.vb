' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class ActiveInsulinRecord
    Private _amount As Single

    <DisplayName("Amount")>
    <Column(Order:=0, TypeName:=NameOf([Single]))>
    Public Property amount As Single
        Get
            Return _amount
        End Get
        Set
            _amount = Value.RoundSingle(3, False)
        End Set
    End Property

    <DisplayName(NameOf(ActiveInsulinRecord.datetime))>
    <Column(Order:=1, TypeName:="Date")>
    Public Property datetime As Date

    <DisplayName("datetime As String")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property datetimeAsString As String

    <DisplayName(NameOf(OAdatetime))>
    <Column(Order:=3, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdatetime As OADate
        Get
            Return New OADate(Me.datetime)
        End Get
    End Property

    <DisplayName("Kind")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Precision")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    Public Property precision As String

    <DisplayName("Version")>
    <Column(Order:=6, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

End Class
