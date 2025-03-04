' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization
Imports Octokit

Public Class TokenData
    <JsonPropertyName("access_token")>
    Public Property AccessToken As String

    <JsonPropertyName("refresh_token")>
    Public Property RefreshToken As String

    <JsonPropertyName("scope")>
    Public Property Scope As String

    <JsonPropertyName("resource")>
    Public Property Resource As List(Of String)

    <JsonPropertyName("client_id")>
    Public Property ClientId As String

    <JsonPropertyName("client_secret")>
    Public Property ClientSecret As String

    <JsonPropertyName("mag-identifier")>
    Public Property MagIdentifier As String
End Class
