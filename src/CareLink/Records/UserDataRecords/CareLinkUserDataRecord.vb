' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class CareLinkUserDataRecord

    Public Property CareLinkUserName As String
    Public Property CareLinkPassword As String
    Public Property AIT As TimeSpan
    Public Property AlertPhoneNumber As String
    Public Property CarrierTextingDomain As String
    Public Property CountryCode As String
    Public Property MailserverPassword As String
    Public Property MailServerPort As Integer
    Public Property MailserverUserName As String
    Public Property SettingsVersion As String
    Public Property OutGoingMailServer As String
    Public Property UseAdvancedAITDecay As Boolean
    Public Property UseLocalTimeZone As Boolean
    Public Property AutoLogin As Boolean

    Public Sub New()
        Me.CareLinkUserName = My.Settings.CareLinkUserName
        Me.CareLinkPassword = My.Settings.CareLinkPassword
        Me.AIT = My.Settings.AIT
        Me.AlertPhoneNumber = My.Settings.AlertPhoneNumber
        Me.CarrierTextingDomain = My.Settings.CarrierTextingDomain
        Me.CountryCode = My.Settings.CountryCode
        Me.MailserverPassword = My.Settings.MailServerPassword
        Me.MailServerPort = My.Settings.MailServerPort
        Me.MailserverUserName = My.Settings.MailServerUserName
        Me.SettingsVersion = My.Settings.SettingsVersion
        Me.OutGoingMailServer = My.Settings.OutGoingMailServer
        Me.UseAdvancedAITDecay = My.Settings.UseAdvancedAITDecay
        Me.UseLocalTimeZone = My.Settings.UseLocalTimeZone
        Me.AutoLogin = My.Settings.AutoLogin
    End Sub

    Public Sub New(currentRow As String())
        Dim entryName As String
        For Each e As IndexClass(Of String) In currentRow.WithIndex
            Dim value As String = e.Value

            entryName = CareLinkUserDataRecordHelpers.GetColumnName(e.Index)
            If String.IsNullOrEmpty(value) Then
                If entryName = NameOf(My.Settings.SettingsVersion) Then
                    value = "1.0"
                Else
                    Continue For
                End If
            End If

            Me.Update(entryName, value)
        Next
    End Sub

    Friend Function ToCsvString() As String
        Return $"{Me.CareLinkUserName},{Me.CareLinkPassword}," &
                $"{Me.AIT},{Me.AlertPhoneNumber},{Me.CarrierTextingDomain}," &
                $"{Me.CountryCode},{Me.MailserverPassword}," &
                $"{Me.MailServerPort},{Me.MailserverUserName}," &
                $"{Me.MailserverUserName},{Me.OutGoingMailServer}," &
                $"{Me.UseAdvancedAITDecay},{Me.UseLocalTimeZone},{Me.AutoLogin}"
    End Function

    ''' <summary>
    ''' Replace the value of entryName with Value
    ''' </summary>
    ''' <param name="entryName"></param>
    ''' <param name="value"></param>
    Friend Sub Update(entryName As String, value As String)
        Select Case entryName
            Case NameOf(CareLinkUserName)
                Me.CareLinkUserName = value
            Case NameOf(CareLinkPassword)
                Me.CareLinkPassword = value
            Case NameOf(AIT)
                Me.AIT = TimeSpan.Parse(value)
            Case NameOf(AlertPhoneNumber)
                Me.AlertPhoneNumber = value
            Case NameOf(CarrierTextingDomain)
                Me.CarrierTextingDomain = value
            Case NameOf(CountryCode)
                Me.CountryCode = value
            Case NameOf(MailserverPassword)
                Me.MailserverPassword = value
            Case NameOf(MailServerPort)
                Me.MailServerPort = CInt(value)
            Case NameOf(MailserverUserName)
                Me.MailserverUserName = value
            Case NameOf(SettingsVersion)
                Me.SettingsVersion = value
            Case NameOf(OutGoingMailServer)
                Me.OutGoingMailServer = value
            Case NameOf(UseAdvancedAITDecay)
                Me.UseAdvancedAITDecay = CBool(value)
            Case NameOf(UseLocalTimeZone)
                Me.UseLocalTimeZone = CBool(value)
            Case NameOf(AutoLogin)
                Me.AutoLogin = CBool(value)
            Case Else
        End Select
    End Sub

    Friend Sub UpdateSettings()
        My.Settings.CareLinkUserName = Me.CareLinkUserName
        My.Settings.CareLinkPassword = Me.CareLinkPassword
        My.Settings.AIT = Me.AIT
        My.Settings.AlertPhoneNumber = Me.AlertPhoneNumber
        My.Settings.CarrierTextingDomain = Me.CarrierTextingDomain
        My.Settings.CountryCode = Me.CountryCode
        My.Settings.MailServerPassword = Me.MailserverPassword
        My.Settings.MailServerPort = Me.MailServerPort
        My.Settings.MailServerUserName = Me.MailserverUserName
        My.Settings.SettingsVersion = Me.SettingsVersion
        My.Settings.OutGoingMailServer = Me.OutGoingMailServer
        My.Settings.UseAdvancedAITDecay = Me.UseAdvancedAITDecay
        My.Settings.UseLocalTimeZone = Me.UseLocalTimeZone
        My.Settings.AutoLogin = Me.AutoLogin
    End Sub

    ''' <summary>
    ''' Delete any user specific sensitive data
    ''' </summary>
    Friend Sub clean(userName As String)
        Me.CareLinkUserName = userName
        Me.AlertPhoneNumber = ""
        Me.CareLinkPassword = ""
        Me.CarrierTextingDomain = ""
        Me.MailserverPassword = ""
        Me.MailServerPort = 0
        Me.MailserverUserName = ""
        Me.OutGoingMailServer = ""
    End Sub

End Class
