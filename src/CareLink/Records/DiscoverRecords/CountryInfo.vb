' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class CountryInfo

    <JsonPropertyName("region")>
    Public Property Region As String

    <JsonPropertyName("isoCode")>
    Public Property IsoCode As String

    <JsonPropertyName("iso3Code")>
    Public Property Iso3Code As String

    <JsonPropertyName("flag")>
    Public Property Flag As String

End Class
