' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class Ble
    <JsonPropertyName("msso_ble_service_uuid")>
    Public Property MssoBleServiceUuid As String

    <JsonPropertyName("msso_ble_characteristic_uuid")>
    Public Property MssoBleCharacteristicUuid As String

    <JsonPropertyName("msso_ble_rssi")>
    Public Property MssoBleRssi As Integer
End Class
