' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class Item

    <JsonPropertyName("action")>
    Public Property Action As String

    <JsonPropertyName("contents")>
    Public Property Contents As List(Of Content)

    <JsonPropertyName("iconIdentifier")>
    Public Property IconIdentifier As String

    <JsonPropertyName("position")>
    Public Property Position As Integer

End Class
