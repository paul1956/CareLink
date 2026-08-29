' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class AccessTokenDetails

    <JsonPropertyName("scope")>
    Public Property Scope As String

    <JsonPropertyName("expires_in")>
    Public Property ExpiresIn As Integer

    <JsonPropertyName("token_type")>
    Public Property TokenType As String

    <JsonPropertyName("preferred_username")>
    Public Property PreferredUsername As String

    <JsonPropertyName("name")>
    Public Property Name As String

    <JsonPropertyName("given_name")>
    Public Property GivenName As String

    <JsonPropertyName("family_name")>
    Public Property FamilyName As String

    <JsonPropertyName("locale")>
    Public Property Locale As String

    <JsonPropertyName("country")>
    Public Property Country As String

    <JsonPropertyName("roles")>
    Public Property Roles As String()

End Class
