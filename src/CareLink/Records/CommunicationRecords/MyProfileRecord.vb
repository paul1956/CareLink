' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink.PumpVariables

Public Class MyProfileRecord
    Private _hasValue As Boolean

#Region "Single Items"

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

#End Region 'Single Items

    Public Sub New(jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If

        My.Forms.Form1.DataGridViewMyProfile.DataSource = (
                                From entry In jsonData
                                Order By entry.Key
                                Select New With {
                                    Key entry.Key.ToTitle,
                                    Key entry.Value
                                }).ToList()

        For Each row As KeyValuePair(Of String, String) In jsonData

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
                    Me.dateOfBirth = row.Value
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

        _hasValue = True
    End Sub

    Public Sub New()
        _hasValue = False
    End Sub

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function
    Private Shared ReadOnly s_columnsToHide As New List(Of String)

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Function GetCellStyle() As DataGridViewCellStyle
        Return New DataGridViewCellStyle().CellStyleMiddleLeft
    End Function

End Class
