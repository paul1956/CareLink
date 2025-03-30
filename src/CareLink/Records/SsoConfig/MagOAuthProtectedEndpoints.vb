' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MagOAuthProtectedEndpoints

    <JsonPropertyName("enterprise_browser_endpoint_path")>
    Public Property EnterpriseBrowserEndpointPath As String

    <JsonPropertyName("device_list_endpoint_path")>
    Public Property DeviceListEndpointPath As String

    <JsonPropertyName("device_metadata_endpoint_path")>
    Public Property DeviceMetadataEndpointPath As String

End Class
