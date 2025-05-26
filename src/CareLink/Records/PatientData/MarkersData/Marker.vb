' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Marker

    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:="String")>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=1, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=2, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return TryParseDateStr(Me.TimestampAsString)
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
            Return TryParseDateStr(Me.DisplayTimeAsString)
        End Get
    End Property

    <DisplayName("Data")>
    <Column(Order:=5, TypeName:="Date")>
    <JsonPropertyName("data")>
    Public Property Data As MarkerData

    <JsonPropertyName("views")>
    Public Property Views As List(Of MarkerView)

End Class
