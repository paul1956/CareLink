' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class CPInfo
    <JsonPropertyName("region")>
    Public Property Region As String

    <JsonPropertyName("SSOConfiguration")>
    Public Property SSOConfiguration As String

    <JsonPropertyName("baseUrlCareLink")>
    Public Property BaseUrlCareLink As String

    <JsonPropertyName("baseUrlCumulus")>
    Public Property BaseUrlCumulus As String

    <JsonPropertyName("baseUrlPde")>
    Public Property BaseUrlPde As String

    <JsonPropertyName("baseUrlAem")>
    Public Property BaseUrlAem As String
End Class
