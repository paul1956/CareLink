' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class OAuthConfig
    <JsonPropertyName("client")>
    Public Property Client As ClientConfig

    <JsonPropertyName("system_endpoints")>
    Public Property SystemEndpoints As SystemEndpoints

    <JsonPropertyName("oauth_protected_endpoints")>
    Public Property OAuthProtectedEndpoints As OAuthProtectedEndpoints
End Class
