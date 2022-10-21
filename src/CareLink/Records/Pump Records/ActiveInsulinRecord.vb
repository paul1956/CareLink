' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class ActiveInsulinRecord

    Private _datetime As Date
    Private _amount As Single

    <DisplayName("Amount")>
    <Column(Order:=0, TypeName:="Single")>
    Public Property amount As Single
        Get
            Return _amount
        End Get
        Set
            _amount = Value.RoundSingle(3)
        End Set
    End Property

    <DisplayName(NameOf(ActiveInsulinRecord.datetime))>
    <Column(Order:=1, TypeName:="Date")>
    Public Property datetime As Date
        Get
            Return _datetime
        End Get
        Set
            _datetime = Value

        End Set
    End Property

    <DisplayName("datetime As String")>
    <Column(Order:=2, TypeName:="String")>
    Public Property datetimeAsString As String

    <DisplayName("OA datetime")>
    <Column(Order:=3, TypeName:="Double")>
    Public ReadOnly Property OAdatetime As OADate
        Get
            Return New OADate(_datetime)
        End Get
    End Property

    <DisplayName("Kind")>
    <Column(Order:=4, TypeName:="String")>
    Public Property kind As String

    <DisplayName("Precision")>
    <Column(Order:=5, TypeName:="String")>
    Public Property precision As String

    <DisplayName("Version")>
    <Column(Order:=6, TypeName:="Integer")>
    Public Property version As Integer
End Class
