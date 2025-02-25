' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class NotificationHistory

    <JsonPropertyName("activeNotifications")>
    Public Property ActiveNotifications As List(Of Object)

    <JsonPropertyName("clearedNotifications")>
    Public Property ClearedNotifications As List(Of ClearedNotification)

End Class
