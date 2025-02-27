' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class Marker

    Public Property RecordNumber As Integer

    <JsonPropertyName("type")>
    Public Property Type As String

    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    <JsonPropertyName("data")>
    Public Property Data As MarkerData

    <JsonPropertyName("views")>
    Public Property Views As List(Of MarkerView)

    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Date.ParseExact(Me.TimestampAsString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
    End Property

    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return Date.ParseExact(Me.DisplayTimeAsString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
    End Property

    Friend Shared Function Convert(r As Basal) As Marker
        Dim convertedMarker As New Marker With {
            .RecordNumber = -1,
            .Type = r.tempBasalType,
            .TimestampAsString = Date.FromOADate(r.GetOaGetTime).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            .DisplayTimeAsString = .TimestampAsString
       }
        Return convertedMarker
    End Function

End Class
