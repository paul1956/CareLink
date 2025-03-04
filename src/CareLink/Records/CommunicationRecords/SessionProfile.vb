' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class SessionProfile
    Private ReadOnly _profileDictionary As New Dictionary(Of String, String)
    Private _hasValue As Boolean

    Public Sub New()
        _hasValue = False
    End Sub

    <DisplayName("First Name")>
    <Column(Order:=29, TypeName:=NameOf([String]))>
    Public Property firstName As String

    <DisplayName("Last Name")>
    <Column(Order:=30, TypeName:=NameOf([String]))>
    Public Property lastName As String

    <DisplayName("Username")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property username As String

    <DisplayName("Middle Name")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property middleName As String

    <DisplayName("Guardian Parent")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property guardianParent As String

    <DisplayName("Parent First Name")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property parentFirstName As String

    <DisplayName("Parent Middle Name")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    Public Property parentMiddleName As String

    <DisplayName("Parent Last Name")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    Public Property parentLastName As String

    <DisplayName("Address")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property address As String

    <DisplayName("City")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    Public Property city As String

    <DisplayName("State/Province")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public Property stateProvince As String

    <DisplayName("Postal Code")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    Public Property postalCode As String

    <DisplayName("Country")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property country As String

    <DisplayName("Date Of Birth")>
    <Column(Order:=11, TypeName:=NameOf([String]))>
    Public Property dateOfBirth As String

    <DisplayName("Phone")>
    <Column(Order:=12, TypeName:=NameOf([String]))>
    Public Property phone As String

    <DisplayName("Phone Legacy")>
    <Column(Order:=13, TypeName:=NameOf([String]))>
    Public Property phoneLegacy As String

    <DisplayName("eMail")>
    <Column(Order:=14, TypeName:=NameOf([String]))>
    Public Property email As String

    <DisplayName("Gender")>
    <Column(Order:=15, TypeName:=NameOf([String]))>
    Public Property gender As String

    <DisplayName("Race")>
    <Column(Order:=16, TypeName:=NameOf([String]))>
    Public Property race As String

    <DisplayName("Diabetes Type")>
    <Column(Order:=17, TypeName:=NameOf([String]))>
    Public Property diabetesType As String

    <DisplayName("Therapy Type")>
    <Column(Order:=18, TypeName:=NameOf([String]))>
    Public Property therapyType As String

    <DisplayName("Age Range")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property ageRange As String

    <DisplayName("Insulin Type")>
    <Column(Order:=20, TypeName:=NameOf([String]))>
    Public Property insulinType As String

    <DisplayName("Patient Nickname")>
    <Column(Order:=21, TypeName:=NameOf([String]))>
    Public Property patientNickname As String

    <DisplayName("Text Notification")>
    <Column(Order:=22, TypeName:=NameOf([String]))>
    Public Property textNotification As String

    <DisplayName("Clinic Restrictions Enable")>
    <Column(Order:=23, TypeName:=NameOf([String]))>
    Public Property clinicRestrictionsEnable As String

    <DisplayName("A1C")>
    <Column(Order:=24, TypeName:=NameOf([String]))>
    Public Property a1C As String

    <DisplayName("Street Address 1")>
    <Column(Order:=25, TypeName:=NameOf([String]))>
    Public Property StreetAddress1 As String

    <DisplayName("Zip")>
    <Column(Order:=26, TypeName:=NameOf([String]))>
    Public Property Zip As String

    <DisplayName("Country Code")>
    <Column(Order:=27, TypeName:=NameOf([String]))>
    Public Property countryCode As String

    <DisplayName("State")>
    <Column(Order:=28, TypeName:=NameOf([String]))>
    Public Property state As String

    <DisplayName("Role")>
    <Column(Order:=31, TypeName:=NameOf([String]))>
    Public Property role As String

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

    Public Sub SetInsulinType(insulinType As String)
        _profileDictionary(NameOf(insulinType).ToTitleCase(False)) = insulinType
    End Sub

    Public Function ToDataSource() As List(Of KeyValuePair(Of String, String))
        Return _profileDictionary.ToDataSource
    End Function

End Class
