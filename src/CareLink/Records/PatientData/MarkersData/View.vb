' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class View

    <JsonPropertyName("viewType")>
    Public Property ViewType As String

    <JsonPropertyName("items")>
    Public Property Items As List(Of Item)

End Class
