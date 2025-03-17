' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class LastSG
    <JsonPropertyName("kind")>
    Public Property Kind As String

    <JsonPropertyName("version")>
    Public Property Version As Integer

    <JsonPropertyName("sg")>
    Public Property Sg As Single

    <JsonPropertyName("sensorState")>
    Public Property SensorState As String

    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Date.ParseExact(Me.TimestampAsString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        End Get
    End Property

End Class
