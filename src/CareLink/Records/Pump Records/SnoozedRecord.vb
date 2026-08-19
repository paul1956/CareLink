' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class SnoozedRecord

    <JsonPropertyName("by")>
    Public Property By As String

    <JsonPropertyName("time")>
    Public Property Time As Date

    <JsonPropertyName("duration")>
    Public Property Duration As Integer

End Class
