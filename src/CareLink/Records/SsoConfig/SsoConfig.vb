' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class SsoConfig

    <JsonPropertyName("server")>
    Public Property Server As ServerConfig

    <JsonPropertyName("oauth")>
    Public Property OAuth As OAuthConfig

    <JsonPropertyName("mag")>
    Public Property Mag As MagConfig

    <JsonPropertyName("custom")>
    Public Property Custom As CustomConfig

End Class
