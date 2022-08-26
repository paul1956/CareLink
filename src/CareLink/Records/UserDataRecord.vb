' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class UserDataRecord

    Public Shared ReadOnly _headerColumns As New List(Of String) From
            {
            NameOf(My.Settings.CareLinkUserName),
            NameOf(My.Settings.CareLinkPassword),
            NameOf(My.Settings.AIT),
            NameOf(My.Settings.AlertPhoneNumber),
            NameOf(My.Settings.CarrierTextingDomain),
            NameOf(My.Settings.CountryCode),
            NameOf(My.Settings.MailServerPassword),
            NameOf(My.Settings.MailServerPort),
            NameOf(My.Settings.MailServerUserName),
            NameOf(My.Settings.NotificationPhoneNumber), NameOf(My.Settings.OutGoingMailServer),
            NameOf(My.Settings.UseAdvancedAITDecay),
            NameOf(My.Settings.UseLocalTimeZone),
            NameOf(My.Settings.AutoLogin)
         }

    Public CareLinkUserName As String
    Public CareLinkPassword As String
    Public AIT As TimeSpan
    Public AlertPhoneNumber As String
    Public CarrierTextingDomain As String
    Public CountryCode As String
    Public MailserverPassword As String
    Public MailServerPort As Integer
    Public MailserverUserName As String
    Public NotificationPhoneNumber As String
    Public OutGoingMailServer As String
    Public UseAdvancedAITDecay As Boolean
    Public UseLocalTimeZone As Boolean
    Public AutoLogin As Boolean

    Public Sub New()
        CareLinkUserName = My.Settings.CareLinkUserName
        CareLinkPassword = My.Settings.CareLinkPassword
        AIT = My.Settings.AIT
        AlertPhoneNumber = My.Settings.AlertPhoneNumber
        CarrierTextingDomain = My.Settings.CarrierTextingDomain
        CountryCode = My.Settings.CountryCode
        MailserverPassword = My.Settings.MailServerPassword
        MailServerPort = My.Settings.MailServerPort
        MailserverUserName = My.Settings.MailServerUserName
        NotificationPhoneNumber = My.Settings.NotificationPhoneNumber
        OutGoingMailServer = My.Settings.OutGoingMailServer
        UseAdvancedAITDecay = My.Settings.UseAdvancedAITDecay
        UseLocalTimeZone = My.Settings.UseLocalTimeZone
        AutoLogin = My.Settings.AutoLogin
    End Sub

    Public Sub New(currentRow As String())
        Dim entryName As String
        For Each e As IndexClass(Of String) In currentRow.WithIndex
            Dim value As String = e.Value

            If String.IsNullOrEmpty(value) Then
                Continue For
            End If
            entryName = _headerColumns(e.Index)

            Me.Update(entryName, value)
        Next
    End Sub

    Friend Function GetValueFromName(entryName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As String
        Select Case entryName
            Case NameOf(My.Settings.CareLinkUserName)
                Return CareLinkUserName
            Case NameOf(My.Settings.CareLinkPassword)
                Return CareLinkPassword
            Case NameOf(My.Settings.AIT)
                Return AIT.ToString
            Case NameOf(My.Settings.AlertPhoneNumber)
                Return AlertPhoneNumber
            Case NameOf(My.Settings.CarrierTextingDomain)
                Return CarrierTextingDomain
            Case NameOf(My.Settings.CountryCode)
                Return CountryCode
            Case NameOf(My.Settings.MailServerPassword)
                Return MailserverPassword
            Case NameOf(My.Settings.MailServerPort)
                Return MailServerPort.ToString
            Case NameOf(My.Settings.MailServerUserName)
                Return MailserverUserName
            Case NameOf(My.Settings.NotificationPhoneNumber)
                Return NotificationPhoneNumber
            Case NameOf(My.Settings.OutGoingMailServer)
                Return OutGoingMailServer
            Case NameOf(My.Settings.UseAdvancedAITDecay)
                Return UseAdvancedAITDecay.ToString
            Case NameOf(My.Settings.UseLocalTimeZone)
                Return UseLocalTimeZone.ToString
            Case NameOf(My.Settings.AutoLogin)
                Return AutoLogin.ToString
        End Select
        Throw UnreachableException(memberName, sourceLineNumber)
    End Function

    Friend Function ToCsvString() As String
        Return $"{CareLinkUserName},{CareLinkPassword}," &
                $"{AIT},{AlertPhoneNumber},{CarrierTextingDomain}," &
                $"{CountryCode},{MailserverPassword}," &
                $"{MailServerPort},{MailserverUserName}," &
                $"{NotificationPhoneNumber}," & $"{OutGoingMailServer}," &
                $"{UseAdvancedAITDecay}," & $"{UseLocalTimeZone}," &
                $"{AutoLogin}"
    End Function

    ''' <summary>
    ''' Replace the value of entryName with Value
    ''' </summary>
    ''' <param name="entryName"></param>
    ''' <param name="value"></param>
    Friend Sub Update(entryName As String, value As String)
        Select Case entryName
            Case NameOf(CareLinkUserName)
                CareLinkUserName = value
            Case NameOf(CareLinkPassword)
                CareLinkPassword = value
            Case NameOf(AIT)
                AIT = TimeSpan.Parse(value)
            Case NameOf(AlertPhoneNumber)
                AlertPhoneNumber = value
            Case NameOf(CarrierTextingDomain)
                CarrierTextingDomain = value
            Case NameOf(CountryCode)
                CountryCode = value
            Case NameOf(MailserverPassword)
                MailserverPassword = value
            Case NameOf(MailServerPort)
                MailServerPort = CInt(value)
            Case NameOf(MailserverUserName)
                MailserverUserName = value
            Case NameOf(NotificationPhoneNumber)
                NotificationPhoneNumber = value
            Case NameOf(OutGoingMailServer)
                OutGoingMailServer = value
            Case NameOf(UseAdvancedAITDecay)
                UseAdvancedAITDecay = CBool(value)
            Case NameOf(UseLocalTimeZone)
                UseLocalTimeZone = CBool(value)
            Case NameOf(AutoLogin)
                AutoLogin = CBool(value)
            Case Else
        End Select
    End Sub

    Friend Sub UpdateSettings()
        My.Settings.CareLinkUserName = CareLinkUserName
        My.Settings.CareLinkPassword = CareLinkPassword
        My.Settings.AIT = AIT
        My.Settings.AlertPhoneNumber = AlertPhoneNumber
        My.Settings.CarrierTextingDomain = CarrierTextingDomain
        My.Settings.CountryCode = CountryCode
        My.Settings.MailServerPassword = MailserverPassword
        My.Settings.MailServerPort = MailServerPort
        My.Settings.MailServerUserName = MailserverUserName
        My.Settings.NotificationPhoneNumber = NotificationPhoneNumber
        My.Settings.OutGoingMailServer = OutGoingMailServer
        My.Settings.UseAdvancedAITDecay = UseAdvancedAITDecay
        My.Settings.UseLocalTimeZone = UseLocalTimeZone
        My.Settings.AutoLogin = AutoLogin
    End Sub
End Class
