' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class ConfigRecord

    <JsonPropertyName("config")>
    Public Property Config As String

    <JsonPropertyName("supportedCountries")>
    Public Property SupportedCountries As List(Of Dictionary(Of String, CountryInfo))

    <JsonPropertyName("CP")>
    Public Property CP As List(Of CPInfo)

    <JsonPropertyName("certificates")>
    Public Property Certificates As List(Of CertificateInfo)

End Class
