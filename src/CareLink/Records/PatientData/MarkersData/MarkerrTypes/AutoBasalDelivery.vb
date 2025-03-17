' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class AutoBasalDelivery

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.bolusAmount = markerEntry.Data.DataValues(NameOf(bolusAmount)).ToString.ParseSingle(decimalDigits:=10)
        Me.maxAutoBasalRate = markerEntry.Data.DataValues(NameOf(maxAutoBasalRate)).ToString.ParseSingle(decimalDigits:=10)
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=1, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("TimestampAsDate")>
    <Column(Order:=2, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Date.ParseExact(Me.TimestampAsString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
    End Property

    <DisplayName(NameOf(DisplayTime))>
    <Column(Order:=3, TypeName:="String")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    <DisplayName("DisplayTimeAsDate")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return Date.ParseExact(Me.DisplayTimeAsString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
    End Property

    <DisplayName(NameOf(OAdateTime))>
    <Column(Order:=5, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Bolus Amount")>
    <Column(Order:=6, TypeName:=NameOf([Single]))>
    Public Property bolusAmount As Single

    <DisplayName("Basal Rate")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public Property basalRate As Single

    <DisplayName("Max Auto Basal Rate")>
    <Column(Order:=8, TypeName:=NameOf([Single]))>
    Public Property maxAutoBasalRate As Single


End Class
