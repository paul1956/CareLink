' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MetadataInfo

    <JsonPropertyName("kind")>
    Public Property Kind As String

    <JsonPropertyName("version")>
    Public Property Version As Integer

    <JsonPropertyName("typeCast")>
    Public Property TypeCast As Boolean

    <JsonPropertyName("iconResourceBundle")>
    Public Property IconResourceBundle As IconResourceBundle

    <JsonPropertyName("clientDateTime")>
    Public Property ClientDateTime As String

End Class
