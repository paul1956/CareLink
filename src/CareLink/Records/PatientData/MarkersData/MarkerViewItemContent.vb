﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MarkerViewItemContent

    <JsonPropertyName("type")>
    Public Property Type As String

    <JsonPropertyName("resourceKey")>
    Public Property ResourceKey As String

End Class
