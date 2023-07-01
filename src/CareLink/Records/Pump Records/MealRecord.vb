' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class MealRecord
    <DisplayName(NameOf(MealRecord.dateTime))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property [dateTime] As Date

    <DisplayName("Carbs (amount)")>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    Public Property amount As Integer

    <DisplayName("dateTime As String")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property dateTimeAsString As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName(NameOf(OAdateTime))>
    <Column(Order:=7, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.dateTime)
        End Get
    End Property

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

End Class
