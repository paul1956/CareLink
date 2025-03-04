' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json
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


#If False Then

    <DisplayName("Clinic Restrictions Enable")>
    <Column(Order:=17, TypeName:=NameOf([String]))>
    Public Property clinicRestrictionsEnable As String

    <DisplayName("Date Of Birth")>
    <Column(Order:=18, TypeName:=NameOf([String]))>
    Public Property dateOfBirth As String

    <DisplayName("Diabetes Type")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property diabetesType As String

    <DisplayName("eMail")>
    <Column(Order:=20, TypeName:=NameOf([String]))>
    Public Property email As String

    <DisplayName("Gender")>
    <Column(Order:=21, TypeName:=NameOf([String]))>
    Public Property gender As String

    <DisplayName("Parent First Name")>
    <Column(Order:=22, TypeName:=NameOf([String]))>
    Public Property parentFirstName As String

    <DisplayName("Parent Middle Name")>
    <Column(Order:=23, TypeName:=NameOf([String]))>
    Public Property parentMiddleName As String

    <DisplayName("Parent Last Name")>
    <Column(Order:=24, TypeName:=NameOf([String]))>
    Public Property parentLastName As String

    <DisplayName("Guardian Parent")>
    <Column(Order:=25, TypeName:=NameOf([String]))>
    Public Property guardianParent As String

    <DisplayName("Race")>
    <Column(Order:=26, TypeName:=NameOf([String]))>
    Public Property race As String

    <DisplayName("Street Address 1")>
    <Column(Order:=27, TypeName:=NameOf([String]))>
    Public Property StreetAddress1 As String

    <DisplayName("City")>
    <Column(Order:=28, TypeName:=NameOf([String]))>
    Public Property city As String

    <DisplayName("State")>
    <Column(Order:=29, TypeName:=NameOf([String]))>
    Public Property state As String

    <DisplayName("State/Province")>
    <Column(Order:=30, TypeName:=NameOf([String]))>
    Public Property stateProvince As String

    <DisplayName("Zip")>
    <Column(Order:=31, TypeName:=NameOf([String]))>
    Public Property Zip As String

    <DisplayName("Postal Code")>
    <Column(Order:=32, TypeName:=NameOf([String]))>
    Public Property postalCode As String

    <DisplayName("Country Code")>
    <Column(Order:=33, TypeName:=NameOf([String]))>
    Public Property countryCode As String

    <DisplayName("Phone")>
    <Column(Order:=34, TypeName:=NameOf([String]))>
    Public Property phone As String

    <DisplayName("Phone Legacy")>
    <Column(Order:=35, TypeName:=NameOf([String]))>
    Public Property phoneLegacy As String

    <DisplayName("Text Notification")>
    <Column(Order:=36, TypeName:=NameOf([String]))>
    Public Property textNotification As String

    <DisplayName("Therapy Type")>
    <Column(Order:=37, TypeName:=NameOf([String]))>
    Public Property therapyType As String

    <DisplayName("A1C")>
    <Column(Order:=38, TypeName:=NameOf([String]))>
    Public Property a1C As String

    <DisplayName("Age Range")>
    <Column(Order:=39, TypeName:=NameOf([String]))>
    Public Property ageRange As String
#End If

    Public Sub Clear()
        Me.AccountId = 0
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        _hasValue = Me.AccountId > 0
        Return _hasValue
    End Function

    Public Sub SetInsulinType(insulinType As String)
        _profileDictionary(NameOf(insulinType).ToTitleCase(False)) = insulinType
    End Sub

End Class
