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

    Public Sub New(r As Basal, recordNumber As Integer, index As Integer)
        Me.type = r.GetBasalType
        Me.index = index
        Me.kind = "Marker"
        Me.version = 1
        Me.Timestamp = Date.FromOADate(r.GetOaGetTime)
        Me.id = 13
        Me.RecordNumber = recordNumber
        Me.bolusAmount = r.GetBasal
    End Sub

    Public Sub New(marker As Marker, index As Integer)
        Me.type = marker.Type
        Me.index = index
        Me.kind = "Marker"
        Me.version = 1
        Me.Timestamp = marker.Timestamp
        Me.bolusAmount = marker.Data.DataValues(NameOf(bolusAmount)).ToString.ParseSingle(10)
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property kind As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property Timestamp As Date

    <DisplayName(NameOf(OAdateTime))>
    <Column(Order:=6, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=7, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName(NameOf(id))>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property id As Integer

    <DisplayName("Bolus Amount")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public Property bolusAmount As Single

    <DisplayName("Basal Rate")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public Property basalRate As Single

    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String
        Get
            Return _Timestamp.ToString("yyyy-MM-ddTHH:mm:ss")
        End Get
        Set(value As String)
            _Timestamp = Date.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Set
    End Property

End Class
