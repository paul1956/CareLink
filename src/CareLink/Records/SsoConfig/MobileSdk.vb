' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MobileSdk

    <JsonPropertyName("sso_enabled")>
    Public Property SsoEnabled As Boolean

    <JsonPropertyName("location_enabled")>
    Public Property LocationEnabled As Boolean

    <JsonPropertyName("location_provider")>
    Public Property LocationProvider As String

    <JsonPropertyName("msisdn_enabled")>
    Public Property MsisdnEnabled As Boolean

    <JsonPropertyName("enable_public_key_pinning")>
    Public Property EnablePublicKeyPinning As Boolean

    <JsonPropertyName("trusted_public_pki")>
    Public Property TrustedPublicPki As Boolean

    <JsonPropertyName("trusted_cert_pinned_public_key_hashes")>
    Public Property TrustedCertPinnedPublicKeyHashes As List(Of String)

    <JsonPropertyName("client_cert_rsa_keybits")>
    Public Property ClientCertRsaKeybits As Integer

End Class
