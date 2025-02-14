' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class AccessToken
    Public Property access_token As String
    Public Property refresh_token As String
    Public Property scope As String
    Public Property resource As List(Of String)
    Public Property client_id As String
    Public Property client_secret As String
    <JsonPropertyName("mag-identifier")>
    Public Property mag_identifier As String
End Class
