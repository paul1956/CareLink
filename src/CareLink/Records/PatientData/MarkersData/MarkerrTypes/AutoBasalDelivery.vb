' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class AutoBasalDelivery

    Public Sub New()
    End Sub

    Public Sub New(r As Basal, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.type = "Basal"
        Me.Kind = "Marker"
        'Me.Timestamp = r.Timestamp
        'Me.TimestampAsString = r.TimestampAsString
        'Me.DisplayTime = r.DisplayTime
        'Me.DisplayTimeAsString = r.DisplayTimeAsString
        'Me.bolusAmount = r.Data.DataValues(NameOf(bolusAmount)).ToString.ParseSingle(10)
        'Me.maxAutoBasalRate = r.Data.DataValues(NameOf(maxAutoBasalRate)).ToString.ParseSingle(10)
    End Sub

    Public Sub New(markerEntry As Marker, recordNumber As Integer, index As Integer)
        Me.RecordNumber = recordNumber
        Me.index = index
        Me.type = markerEntry.Type
        Me.Kind = "Marker"
        Me.Timestamp = markerEntry.Timestamp
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTime = markerEntry.DisplayTime
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.bolusAmount = markerEntry.Data.DataValues(NameOf(bolusAmount)).ToString.ParseSingle(10)
        Me.maxAutoBasalRate = markerEntry.Data.DataValues(NameOf(maxAutoBasalRate)).ToString.ParseSingle(10)
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property Index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property Kind As String

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=4, TypeName:="Date")>
    Public Property Timestamp As Date

    <DisplayName(NameOf(TimestampAsString))>
    <Column(Order:=5, TypeName:="String")>
    Public Property TimestampAsString As String

    <DisplayName("Display Time")>
    <Column(Order:=6, TypeName:=NameOf([DateTime]))>
    Public Property DisplayTime As Date

    <DisplayName(NameOf(DisplayTimeAsString))>
    <Column(Order:=7, TypeName:="String")>
    Public Property DisplayTimeAsString As String

    <DisplayName(NameOf(OAdateTime))>
    <Column(Order:=8, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Bolus Amount")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public Property bolusAmount As Single

    <DisplayName("Basal Rate")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public Property basalRate As Single

    <DisplayName("Max Auto Basal Rate")>
    <Column(Order:=11, TypeName:=NameOf([Single]))>
    Public Property maxAutoBasalRate As Single


End Class
