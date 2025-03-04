' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class UserInfo
    <JsonPropertyName("loginDateUTC")>
    Public Property LoginDateUTC As String

    <JsonPropertyName("id")>
    Public Property Id As String

    <JsonPropertyName("country")>
    Public Property Country As String

    <JsonPropertyName("language")>
    Public Property Language As String

    <JsonPropertyName("lastName")>
    Public Property LastName As String

    <JsonPropertyName("firstName")>
    Public Property FirstName As String

    <JsonPropertyName("accountId")>
    Public Property AccountId As Integer

    <JsonPropertyName("role")>
    Public Property Role As String

    <JsonPropertyName("cpRegistrationStatus")>
    Public Property CpRegistrationStatus As String

    <JsonPropertyName("accountSuspended")>
    Public Property AccountSuspended As Boolean?

    <JsonPropertyName("needToReconsent")>
    Public Property NeedToReconsent As Boolean

    <JsonPropertyName("mfaRequired")>
    Public Property MfaRequired As Boolean

    <JsonPropertyName("mfaEnabled")>
    Public Property MfaEnabled As Boolean

    <JsonPropertyName("username")>
    Public Property Username As String
End Class
