' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class LastAlarm
    <JsonPropertyName("faultId")>
    Public Property FaultId As String

    <JsonPropertyName("version")>
    Public Property Version As String

    <JsonPropertyName("GUID")>
    Public Property GUID As String

    <JsonPropertyName("dateTime")>
    Public Property DateTime As String

    <JsonPropertyName("type")>
    Public Property Type As String

    <JsonPropertyName("additionalInfo")>
    Public Property AdditionalInfo As AdditionalInfo
End Class
