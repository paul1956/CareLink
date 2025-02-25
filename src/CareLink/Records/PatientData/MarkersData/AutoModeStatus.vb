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
        Me.Timestamp = markerEntry.Timestamp
        Me.DisplayTime = markerEntry.DisplayTime
        Me.Data = markerEntry.Data
        Me.Views = markerEntry.Views
#If False Then ' TODO
        Me.index = markerEntry.index
        Me.kind = markerEntry.kind
        Me.version = markerEntry.version
        Me.relativeOffset = markerEntry.relativeOffset
        Me.autoModeOn = markerEntry.autoModeOn
#End If
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property kind As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property Timestamp As Date

    <DisplayName("Display Time")>
    <Column(Order:=6, TypeName:=NameOf([DateTime]))>
    Public Property DisplayTime As Date

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=7, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer


    <DisplayName("Auto Mode On")>
    <Column(Order:=8, TypeName:=NameOf([Boolean]))>
    Public Property autoModeOn As Boolean

    <JsonPropertyName("data")>
    Public Property Data As MarkerData

    <JsonPropertyName("views")>
    Public Property Views As List(Of MarkerView)

    <JsonPropertyName("displaytime")>
    Public Property DisplayTimeAsString As String
        Get
            Return _DisplayTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
        Set(value As String)
            _DisplayTime = Date.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Set
    End Property

    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String
        Get
            Return _Timestamp.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
        Set(value As String)
            _Timestamp = Date.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Set
    End Property

End Class
