' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class AutoModeStatus
    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.Timestamp = markerEntry.Timestamp
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTime = markerEntry.DisplayTime
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.AutoModeOn = PatientData.TherapyAlgorithmState.AutoModeShieldState = "AUTO_BASAL"
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName(NameOf(Index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property Index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public ReadOnly Property Kind As String

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

    <DisplayName("Auto Mode On")>
    <Column(Order:=8, TypeName:=NameOf([Boolean]))>
    Public ReadOnly Property AutoModeOn As Boolean

End Class
