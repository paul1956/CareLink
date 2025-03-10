' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class Meal

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.type = markerEntry.Type
        Me.Kind = "Marker"
        Me.Timestamp = markerEntry.Timestamp
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTime = markerEntry.DisplayTime
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Const fieldName As String = "amount"
        Me.amount = CInt(markerEntry.GetSingleValueFromJson(fieldName, decimalDigits:=0))
    End Sub
    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName("Kind")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property Kind As String

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=3, TypeName:="Date")>
    Public Property Timestamp As Date

    <DisplayName(NameOf(TimestampAsString))>
    <Column(Order:=4, TypeName:="String")>
    Public Property TimestampAsString As String

    <DisplayName("Display Time")>
    <Column(Order:=5, TypeName:=NameOf([DateTime]))>
    Public Property DisplayTime As Date

    <DisplayName(NameOf(DisplayTimeAsString))>
    <Column(Order:=6, TypeName:="String")>
    Public Property DisplayTimeAsString As String

    <DisplayName(NameOf(OAdateTime))>
    <Column(Order:=7, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Carbs (amount)")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property amount As Integer

End Class
