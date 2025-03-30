' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class CustomConfig

    <JsonPropertyName("oauth_demo_protected_api_endpoint_path")>
    Public Property OAuthDemoProtectedApiEndpointPath As String

    <JsonPropertyName("mag_demo_products_endpoint_path")>
    Public Property MagDemoProductsEndpointPath As String

End Class
