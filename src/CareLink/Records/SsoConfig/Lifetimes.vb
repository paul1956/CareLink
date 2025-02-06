' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class Lifetimes
    <JsonPropertyName("oauth2_access_token_lifetime_sec")>
    Public Property OAuth2AccessTokenLifetimeSec As Integer

    <JsonPropertyName("oauth2_refresh_token_lifetime_sec")>
    Public Property OAuth2RefreshTokenLifetimeSec As Integer

    <JsonPropertyName("id_token_lifetime_s")>
    Public Property IdTokenLifetimeS As Integer
End Class
