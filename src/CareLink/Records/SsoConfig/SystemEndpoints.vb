' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class SystemEndpoints

    <JsonPropertyName("authorization_endpoint_path")>
    Public Property AuthorizationEndpointPath As String

    <JsonPropertyName("token_endpoint_path")>
    Public Property TokenEndpointPath As String

    <JsonPropertyName("token_revocation_endpoint_path")>
    Public Property TokenRevocationEndpointPath As String

    <JsonPropertyName("usersession_logout_endpoint_path")>
    Public Property UserSessionLogoutEndpointPath As String

    <JsonPropertyName("usersession_status_endpoint_path")>
    Public Property UserSessionStatusEndpointPath As String

End Class
