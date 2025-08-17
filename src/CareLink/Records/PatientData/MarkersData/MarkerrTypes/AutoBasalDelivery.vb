' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class AutoBasalDelivery

    Public Sub New(item As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.TimestampAsString = item.TimestampAsString
        Me.DisplayTimeAsString = item.DisplayTimeAsString
        Me.BolusAmount =
            item.Data.DataValues(key:=NameOf(BolusAmount).ToLowerCamelCase).ToString _
                     .ParseSingle(digits:=10)
        Me.MaxAutoBasalRate =
            item.Data.DataValues(key:=NameOf(MaxAutoBasalRate).ToLowerCamelCase).ToString _
                     .ParseSingle(digits:=10)
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=1, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=2, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.TimestampAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("Display Time From Pump")>
    <Column(Order:=3, TypeName:="String")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    <DisplayName("Display Time As Date")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return Me.DisplayTimeAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("OA Date Time")>
    <Column(Order:=5, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Bolus Amount")>
    <Column(Order:=6, TypeName:=NameOf([Single]))>
    <JsonPropertyName("bolusAmount")>
    Public Property BolusAmount As Single

    <DisplayName("Max Auto Basal Rate")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    <JsonPropertyName("maxAutoBasalRate")>
    Public Property MaxAutoBasalRate As Single

End Class
