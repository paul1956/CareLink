' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class CgmInfo

    <JsonPropertyName("sensorType")>
    Public Property SensorType As String

    <JsonPropertyName("sensorSerialNumber")>
    Public Property SensorSerialNumber As String

    <JsonPropertyName("sensorSoftwareRevision")>
    Public Property SensorSoftwareRevision As String

    <JsonPropertyName("sensorFirmwareRevision")>
    Public Property SensorFirmwareRevision As String

    <JsonPropertyName("sensorProductModel")>
    Public Property SensorProductModel As String
End Class
