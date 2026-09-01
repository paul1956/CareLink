' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class Data

    <JsonPropertyName("dataValues")>
    Public Property DataValues As DataValues

    <JsonPropertyName("resourceValues")>
    Public Property ResourceValues As ResourceValues

End Class
