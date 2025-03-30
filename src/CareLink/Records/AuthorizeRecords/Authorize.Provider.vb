' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Partial Public Class Authorize

    Public Class Provider

        <JsonPropertyName("id")>
        Public Property Id As String

        <JsonPropertyName("auth_url")>
        Public Property AuthUrl As String

        <JsonPropertyName("poll_url")>
        Public Property PollUrl As String

    End Class
End Class
