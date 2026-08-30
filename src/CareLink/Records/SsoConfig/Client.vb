' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class Client

    <JsonPropertyName("client_id")>
    Public Property ClientId As String

    <JsonPropertyName("scope")>
    Public Property Scope As String

    <JsonPropertyName("redirect_uri")>
    Public Property RedirectUri As String

    <JsonPropertyName("audience")>
    Public Property Audience As String

End Class
