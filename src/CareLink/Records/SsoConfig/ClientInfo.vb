' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class ClientInfo

    <JsonPropertyName("client_id")>
    Public Property ClientId As String

    <JsonPropertyName("scope")>
    Public Property Scope As String

    <JsonPropertyName("redirect_uri")>
    Public Property RedirectUri As String

    <JsonPropertyName("environment")>
    Public Property Environment As String

    <JsonPropertyName("status")>
    Public Property Status As String

    <JsonPropertyName("registered_by")>
    Public Property RegisteredBy As String

    <JsonPropertyName("service_ids")>
    Public Property ServiceIds As String

    <JsonPropertyName("account_plan_mapping_ids")>
    Public Property AccountPlanMappingIds As String

    <JsonPropertyName("client_key_custom")>
    Public Property ClientKeyCustom As ClientKeyCustom

End Class
