' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class ConfigRecord

    <JsonPropertyName("region")>
    Public Property Region As String

    Public Property SSOConfiguration As String
    Public Property Layer7SSOConfiguration As String
    Public Property Auth0SSOConfiguration As String
    Public Property UseSSOConfiguration As String

    <JsonPropertyName("baseUrlCms")>
    Public Property BaseUrlCms As String

    Public Property BaseUrlCareLink As String

    <JsonPropertyName("baseUrlCumulus")>
    Public Property BaseUrlCumulus As String

    <JsonPropertyName("baseUrlPde")>
    Public Property BaseUrlPdePublic As String

    <JsonPropertyName("baseUrlAem")>
    Public Property BaseUrlAemPublic As String

    <JsonPropertyName("token_url")>
    Public Property TokenUrl As String

End Class
