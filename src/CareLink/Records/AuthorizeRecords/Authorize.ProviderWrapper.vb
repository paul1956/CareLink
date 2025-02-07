' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Partial Public Class Authorize
    Public Class ProviderWrapper
        <JsonPropertyName("provider")>
        Public Property Provider As Provider
    End Class
End Class
