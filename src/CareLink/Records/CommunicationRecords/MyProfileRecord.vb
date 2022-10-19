' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MyProfileRecord
    Private _hasValue As Boolean

    Public Sub New(jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If

        Dim profile As New List(Of KeyValuePair(Of String, String))
        For Each row As KeyValuePair(Of String, String) In jsonData
            profile.Add(KeyValuePair.Create(row.Key.ToTitleCase(False), row.Value))
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
                    profile(profile.Count - 1) = KeyValuePair.Create(row.Key.ToTitleCase, Me.dateOfBirth)
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
                Case Else
                    Stop
            End Select
        Next

        My.Forms.Form1.DataGridViewUserProfile.DataSource = profile

        _hasValue = True
    End Sub

    Public Sub New()
        _hasValue = False
    End Sub

    Public Property a1C As String
    Public Property address As String
    Public Property ageRange As String
    Public Property city As String
    Public Property country As String
    Public Property dateOfBirth As String
    Public Property diabetesType As String
    Public Property email As String
    Public Property firstName As String
    Public Property gender As String
    Public Property guardianParent As String
    Public Property insulinType As String
    Public Property lastName As String
    Public Property middleName As String
    Public Property parentFirstName As String
    Public Property parentLastName As String
    Public Property parentMiddleName As String
    Public Property patientNickname As String
    Public Property phone As String
    Public Property phoneLegacy As String
    Public Property postalCode As String
    Public Property stateProvince As String
    Public Property textNotification As String
    Public Property therapyType As String
    Public Property username As String

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
