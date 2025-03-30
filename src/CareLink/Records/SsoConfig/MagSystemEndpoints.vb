' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MagSystemEndpoints

    <JsonPropertyName("device_register_endpoint_path")>
    Public Property DeviceRegisterEndpointPath As String

    <JsonPropertyName("device_renew_endpoint_path")>
    Public Property DeviceRenewEndpointPath As String

    <JsonPropertyName("device_client_register_endpoint_path")>
    Public Property DeviceClientRegisterEndpointPath As String

    <JsonPropertyName("device_remove_endpoint_path")>
    Public Property DeviceRemoveEndpointPath As String

    <JsonPropertyName("client_credential_init_endpoint_path")>
    Public Property ClientCredentialInitEndpointPath As String

    <JsonPropertyName("authenticate_otp_endpoint_path")>
    Public Property AuthenticateOtpEndpointPath As String

End Class
