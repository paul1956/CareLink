' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class ClientConfig

    <JsonPropertyName("organization")>
    Public Property Organization As String

    <JsonPropertyName("description")>
    Public Property Description As String

    <JsonPropertyName("client_name")>
    Public Property ClientName As String

    <JsonPropertyName("client_type")>
    Public Property ClientType As String

    <JsonPropertyName("registered_by")>
    Public Property RegisteredBy As String

    <JsonPropertyName("client_custom")>
    Public Property ClientCustom As Object

    <JsonPropertyName("client_ids")>
    Public Property ClientIds As List(Of ClientInfo)

End Class
