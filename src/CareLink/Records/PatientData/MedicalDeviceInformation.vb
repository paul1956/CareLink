' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MedicalDeviceInformation

    <JsonPropertyName("manufacturer")>
    Public Property Manufacturer As String

    <JsonPropertyName("modelNumber")>
    Public Property ModelNumber As String

    <JsonPropertyName("hardwareRevision")>
    Public Property HardwareRevision As String

    <JsonPropertyName("firmwareRevision")>
    Public Property FirmwareRevision As String

    <JsonPropertyName("softwareRevision")>
    Public Property SoftwareRevision As String

    <JsonPropertyName("systemId")>
    Public Property SystemId As String

    <JsonPropertyName("pnpId")>
    Public Property PnpId As String

    <JsonPropertyName("deviceSerialNumber")>
    Public Property DeviceSerialNumber As String

End Class
