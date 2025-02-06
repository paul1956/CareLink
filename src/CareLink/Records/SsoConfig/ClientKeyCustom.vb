' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class ClientKeyCustom
    <JsonPropertyName("lifetimes")>
    Public Property Lifetimes As Lifetimes

    <JsonPropertyName("appname")>
    Public Property AppName As String
End Class
