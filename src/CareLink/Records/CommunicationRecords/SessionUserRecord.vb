' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SessionUserRecord
    Private _hasValue As Boolean

    Public Sub New(dgv As DataGridView, jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If

        Dim myUser As New List(Of KeyValuePair(Of String, String))
        For Each row As KeyValuePair(Of String, String) In jsonData
            myUser.Add(KeyValuePair.Create(row.Key.ToTitleCase, row.Value))
            Select Case row.Key
                Case NameOf(loginDateUTC)
                    Me.loginDateUTC = row.Value
                Case NameOf(id)
                    Me.id = row.Value
                Case NameOf(country)
                    Me.country = row.Value
                Case NameOf(language)
                    Me.language = row.Value
                Case NameOf(lastName)
                    Me.lastName = row.Value
                Case NameOf(firstName)
                    Me.firstName = row.Value
                Case NameOf(accountId)
                    Me.accountId = row.Value
                Case NameOf(role)
                    Me.role = row.Value
                Case NameOf(cpRegistrationStatus)
                    Me.cpRegistrationStatus = row.Value
                Case NameOf(accountSuspended)
                    Me.accountSuspended = row.Value
                Case NameOf(needToReconsent)
                    Me.needToReconsent = row.Value
                Case NameOf(mfaRequired)
                    Me.mfaRequired = row.Value
                Case NameOf(mfaEnabled)
                    Me.mfaEnabled = row.Value
            End Select

        Next
        With dgv
            .InitializeDgv()
            .DataSource = myUser
        End With
        _hasValue = True

    End Sub

    Public Sub New()
        _hasValue = False
    End Sub

    Public Property accountId As String
    Public Property accountSuspended As String
    Public Property country As String
    Public Property cpRegistrationStatus As String
    Public Property firstName As String
    Public Property id As String
    Public Property language As String
    Public Property lastName As String
    Public Property loginDateUTC As String
    Public Property mfaEnabled As String
    Public Property mfaRequired As String
    Public Property needToReconsent As String
    Public Property role As String

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
