' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class ServerConfig

    <JsonPropertyName("hostname")>
    Public Property Hostname As String

    <JsonPropertyName("port")>
    Public Property Port As Integer

    <JsonPropertyName("prefix")>
    Public Property Prefix As String

    <JsonPropertyName("server_certs")>
    Public Property ServerCerts As List(Of List(Of String))

End Class
