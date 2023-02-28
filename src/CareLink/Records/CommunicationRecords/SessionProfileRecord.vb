' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class SessionProfileRecord
    Private _hasValue As Boolean
    Private ReadOnly _profileDictionary As New Dictionary(Of String, String)

    Public Sub New(jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If

        For Each row As KeyValuePair(Of String, String) In jsonData
            Dim key As String = row.Key.ToTitleCase(False)
            _profileDictionary.Add(key, row.Value)
            Select Case row.Key
                Case NameOf(username)
                    Me.username = row.Value
                Case NameOf(middleName)
                    Me.middleName = row.Value
                Case NameOf(guardianParent)
                    Me.guardianParent = row.Value
                Case NameOf(parentFirstName)
                    Me.parentFirstName = row.Value
                Case NameOf(parentMiddleName)
                    Me.parentMiddleName = row.Value
                Case NameOf(parentLastName)
                    Me.parentLastName = row.Value
                Case NameOf(address)
                    Me.address = row.Value
                Case NameOf(city)
                    Me.city = row.Value
                Case NameOf(stateProvince)
                    Me.stateProvince = row.Value
                Case NameOf(postalCode)
                    Me.postalCode = row.Value
                Case NameOf(country)
                    Me.country = row.Value
                Case NameOf(dateOfBirth)
                    Me.dateOfBirth = row.Value.Epoch2DateString
                    _profileDictionary(key) = Me.dateOfBirth
                Case NameOf(phone)
                    Me.phone = row.Value
                Case NameOf(phoneLegacy)
                    Me.phoneLegacy = row.Value
                Case NameOf(email)
                    Me.email = row.Value
                Case NameOf(gender)
                    Me.gender = row.Value
                Case NameOf(diabetesType)
                    Me.diabetesType = row.Value
                Case NameOf(therapyType)
                    Me.therapyType = row.Value
                Case NameOf(ageRange)
                    Me.ageRange = row.Value
                Case NameOf(insulinType)
                    Me.insulinType = row.Value
                Case NameOf(patientNickname)
                    Me.patientNickname = row.Value
                Case NameOf(textNotification)
                    Me.textNotification = row.Value
                Case NameOf(a1C)
                    Me.a1C = row.Value
                Case NameOf(firstName)
                    Me.firstName = row.Value
                Case NameOf(lastName)
                    Me.lastName = row.Value
                Case NameOf(race)
                    Dim raceRecord As New RaceRecord(Loads(row.Value))
                    Me.race = raceRecord.ToString
                    _profileDictionary(key) = Me.race
                Case Else
                    Stop
            End Select
        Next

        _hasValue = True
    End Sub

    Public Sub New()
        _hasValue = False
    End Sub

    <DisplayName("A1C")>
    <Column(Order:=23, TypeName:=NameOf([String]))>
    Public Property a1C As String

    <DisplayName("Address")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property address As String

    <DisplayName("Age Range")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property ageRange As String

    <DisplayName("City")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    Public Property city As String

    <DisplayName("Country")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property country As String

    <DisplayName("Date Of Birth")>
    <Column(Order:=11, TypeName:=NameOf([String]))>
    Public Property dateOfBirth As String

    <DisplayName("Diabetes Type")>
    <Column(Order:=17, TypeName:=NameOf([String]))>
    Public Property diabetesType As String

    <DisplayName("eMail")>
    <Column(Order:=14, TypeName:=NameOf([String]))>
    Public Property email As String

    <DisplayName("First Name")>
    <Column(Order:=24, TypeName:=NameOf([String]))>
    Public Property firstName As String

    <DisplayName("Gender")>
    <Column(Order:=15, TypeName:=NameOf([String]))>
    Public Property gender As String

    <DisplayName("Guardian Parent")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property guardianParent As String

    <DisplayName("Insulin Type")>
    <Column(Order:=20, TypeName:=NameOf([String]))>
    Public Property insulinType As String

    <DisplayName("Last Name")>
    <Column(Order:=25, TypeName:=NameOf([String]))>
    Public Property lastName As String

    <DisplayName("Middle Name")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property middleName As String

    <DisplayName("Parent First Name")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property parentFirstName As String

    <DisplayName("Parent Last Name")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    Public Property parentLastName As String

    <DisplayName("Parent Middle Name")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    Public Property parentMiddleName As String

    <DisplayName("Patient Nickname")>
    <Column(Order:=21, TypeName:=NameOf([String]))>
    Public Property patientNickname As String

    <DisplayName("Phone")>
    <Column(Order:=12, TypeName:=NameOf([String]))>
    Public Property phone As String

    <DisplayName("Phone Legacy")>
    <Column(Order:=13, TypeName:=NameOf([String]))>
    Public Property phoneLegacy As String

    <DisplayName("Postal Code")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    Public Property postalCode As String

    <DisplayName("Race")>
    <Column(Order:=16, TypeName:=NameOf([String]))>
    Public Property race As String

    <DisplayName("State/Province")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public Property stateProvince As String

    <DisplayName("Text Notification")>
    <Column(Order:=22, TypeName:=NameOf([String]))>
    Public Property textNotification As String

    <DisplayName("Therapy Type")>
    <Column(Order:=18, TypeName:=NameOf([String]))>
    Public Property therapyType As String

    <DisplayName("Username")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property username As String

    Public Sub SetInsulinType(insulinType As String)
        _profileDictionary(NameOf(insulinType)) = insulinType
    End Sub

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

    Public Function ToDataSource() As List(Of KeyValuePair(Of String, String))
        Return _profileDictionary.ToDataSource
    End Function

End Class
