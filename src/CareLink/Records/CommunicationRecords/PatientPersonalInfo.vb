' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class PatientPersonalInfo
    Private ReadOnly _profileDictionary As New Dictionary(Of String, String)
    Private _hasValue As Boolean

    Public Sub New()
        _hasValue = False
    End Sub

    <DisplayName("login Date UTC")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("loginDateUTC")>
    Public Property LoginDateUTC As String

    <DisplayName("ID")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    <JsonPropertyName("id")>
    Public Property Id As String

    <DisplayName("Country")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    <JsonPropertyName("country")>
    Public Property Country As String

    <DisplayName("Language")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    <JsonPropertyName("language")>
    Public Property Language As String

    <DisplayName("First Name")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    <JsonPropertyName("firstName")>
    Public Property FirstName As String

    <DisplayName("Last Name")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    <JsonPropertyName("lastName")>
    Public Property LastName As String

    <DisplayName("Patient Nickname")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    Public Property patientNickname As String

    <DisplayName("AccountId")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("accountId")>
    Public Property AccountId As Integer

    <DisplayName("Role")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    <JsonPropertyName("role")>
    Public Property role As String

    <DisplayName("cpRegistrationStatus")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    <JsonPropertyName("cpRegistrationStatus")>
    Public Property CpRegistrationStatus As String

    <DisplayName("AccountSuspended")>
    <Column(Order:=11, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("accountSuspended")>
    Public Property AccountSuspended As Boolean?

    <DisplayName("Need To Reconsent")>
    <Column(Order:=12, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("needToReconsent")>
    Public Property NeedToReconsent As Boolean

    <DisplayName("MFA Enabled")>
    <Column(Order:=13, TypeName:=NameOf([String]))>
    <JsonPropertyName("mfaEnabled")>
    Public Property MfaEnabled As Boolean

    <DisplayName("MFA ERequired")>
    <Column(Order:=14, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("mfaRequired")>
    Public Property MfaRequired As Boolean

    <DisplayName("Username")>
    <Column(Order:=15, TypeName:=NameOf([String]))>
    <JsonPropertyName("username")>
    Public Property username As String

    <DisplayName("Insulin Type")>
    <Column(Order:=16, TypeName:=NameOf([String]))>
    <JsonPropertyName("insulinType")>
    Public Property InsulinType As String

    Public Sub Clear()
        Me.AccountId = 0
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        _hasValue = Me.AccountId > 0
        Return _hasValue
    End Function

    Public Sub SetInsulinType(insulinType As String)
        _profileDictionary(NameOf(insulinType).ToTitleCase(separateNumbers:=False)) = insulinType
    End Sub

End Class
