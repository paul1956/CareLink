' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class ActiveInsulinRecord
    Private _datetime As Date
    Private _amount As Single

    <DisplayName(NameOf(_amount))>
    <Column(Order:=0)>
    Public Property amount As Single
        Get
            Return _amount
        End Get
        Set
            _amount = Value.RoundSingle(3)
        End Set
    End Property

    <DisplayName(NameOf(datetime))>
    <Column(Order:=1)>
    Public Property datetime As Date
        Get
            Return _datetime
        End Get
        Set
            _datetime = Value

        End Set
    End Property

    <DisplayName(NameOf(datetimeAsString))>
    <Column(Order:=2)>
    Public Property datetimeAsString As String

    <DisplayName(NameOf(OAdatetime))>
    <Column(Order:=3)>
    Public ReadOnly Property OAdatetime As OADate
        Get
            Return New OADate(_datetime)
        End Get
    End Property

    <DisplayName(NameOf(kind))>
    <Column(Order:=4)>
    Public Property kind As String

    <DisplayName(NameOf(precision))>
    <Column(Order:=5)>
    Public Property precision As String

    <DisplayName(NameOf(version))>
    <Column(Order:=6)>
    Public Property version As Integer

End Class
