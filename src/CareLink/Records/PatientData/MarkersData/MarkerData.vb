' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class MarkerData

    <JsonPropertyName("dataValues")>
    Public Property DataValues As Dictionary(Of String, Object)

    <JsonPropertyName("resourceValues")>
    Public Property ResourceValues As Dictionary(Of String, Object)

End Class
