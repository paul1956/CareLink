' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class LastSG

    <DisplayName("Kind")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property Kind As String

    <DisplayName("Version")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    <JsonPropertyName("version")>
    Public Property Version As Integer

    <DisplayName("Sensor Glucose (sg)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    <JsonPropertyName("sg")>
    Public Property Sg As Single

    <DisplayName("Sensor State")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    <JsonPropertyName("sensorState")>
    Public Property SensorState As String

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=5, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.TimestampAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("Is Backfill")>
    <Column(Order:=5, TypeName:="Boolean")>
    <JsonPropertyName("isBackfill")>
    Public Property IsBackfill As Boolean

    Public Overrides Function ToString() As String
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Dim format As String = GetSgFormat()
        Return Me.Sg.ToString(format, provider)
    End Function

End Class
