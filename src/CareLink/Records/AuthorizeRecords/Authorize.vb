' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Partial Public Class Authorize
    <JsonPropertyName("idp")>
    Public Property Idp As String

    <JsonPropertyName("providers")>
    Public Property Providers As List(Of ProviderWrapper)
End Class
