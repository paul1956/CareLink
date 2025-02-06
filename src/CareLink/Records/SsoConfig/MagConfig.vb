' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MagConfig
    <JsonPropertyName("system_endpoints")>
    Public Property SystemEndpoints As MagSystemEndpoints

    <JsonPropertyName("oauth_protected_endpoints")>
    Public Property OAuthProtectedEndpoints As MagOAuthProtectedEndpoints

    <JsonPropertyName("mobile_sdk")>
    Public Property MobileSdk As MobileSdk

    <JsonPropertyName("ble")>
    Public Property Ble As Ble
End Class
