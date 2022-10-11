' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Runtime.CompilerServices

Public Class CareLinkUserDataRecord
    Implements IEditableObject

    Private _backupData As CareLinkUserData

    Private _inTxn As Boolean = False

    Private _userData As CareLinkUserData

    Public Parent As CareLinkUserDataList

    Public Sub New(parent As CareLinkUserDataList)
        Me.Parent = parent
        _userData = New CareLinkUserData With {
            ._iD = parent.Count,
            ._careLinkUserName = If(My.Settings.CareLinkUserName, ""),
            ._careLinkPassword = If(My.Settings.CareLinkPassword, ""),
            ._aIT = My.Settings.AIT,
            ._alertPhoneNumber = If(My.Settings.AlertPhoneNumber, ""),
            ._carrierTextingDomain = If(My.Settings.CarrierTextingDomain, ""),
            ._countryCode = If(My.Settings.CountryCode, ""),
            ._mailserverPassword = If(My.Settings.MailServerPassword, ""),
            ._mailServerPort = My.Settings.MailServerPort,
            ._mailserverUserName = If(My.Settings.MailServerUserName, ""),
            ._settingsVersion = If(My.Settings.SettingsVersion, ""),
            ._outgoingMailServer = If(My.Settings.OutGoingMailServer, ""),
            ._useAdvancedAITDecay = My.Settings.UseAdvancedAITDecay,
            ._useLocalTimeZone = My.Settings.UseLocalTimeZone,
            ._autoLogin = My.Settings.AutoLogin
        }
    End Sub

    Public Sub New(parent As CareLinkUserDataList, currentRow As String())
        _userData = New CareLinkUserData With {
            ._iD = parent.Count
        }
        Dim entryName As String
        For Each e As IndexClass(Of String) In currentRow.WithIndex
            Dim value As String = If(e.Value, "")

            entryName = CareLinkUserDataRecordHelpers.GetColumnName(e.Index)
            If String.IsNullOrEmpty(value) Then
                If entryName = NameOf(My.Settings.SettingsVersion) Then
                    value = "1.0"
                Else
                    Continue For
                End If
            End If

            Me.UpdateValue(entryName, value)
        Next
    End Sub

    <DisplayName("AIT")>
    <Column(Order:=3)>
    Public Property AIT As TimeSpan
        Get
            Return _userData._aIT
        End Get
        Set
            _userData._aIT = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Alert Phone Number")>
    <Column(Order:=4)>
    Public Property AlertPhoneNumber As String
        Get
            Return _userData._alertPhoneNumber
        End Get
        Set
            _userData._alertPhoneNumber = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Auto Login")>
    <Column(Order:=14)>
    Public Property AutoLogin As Boolean
        Get
            Return _userData._autoLogin
        End Get
        Set
            _userData._autoLogin = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("CareLink Password")>
    <Column(Order:=2)>
    Public Property CareLinkPassword As String
        Get
            Return _userData._careLinkPassword
        End Get
        Set
            _userData._careLinkPassword = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("CareLink UserName")>
    <Column(Order:=1)>
    Public Property CareLinkUserName As String
        Get
            Return _userData._careLinkUserName
        End Get
        Set
            _userData._careLinkUserName = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Carrier Texting Domain")>
    <Column(Order:=5)>
    Public Property CarrierTextingDomain As String
        Get
            Return _userData._carrierTextingDomain
        End Get
        Set
            _userData._carrierTextingDomain = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Country Code")>
    <Column(Order:=6)>
    Public Property CountryCode As String
        Get
            Return _userData._countryCode
        End Get
        Set
            _userData._countryCode = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Id")>
    <Column(Order:=0)>
    Public ReadOnly Property ID() As Integer
        Get
            Return _userData._iD
        End Get
    End Property

    <DisplayName("Mailserver Password")>
    <Column(Order:=8)>
    Public Property MailserverPassword As String
        Get
            Return _userData._mailserverPassword
        End Get
        Set
            _userData._mailserverPassword = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("MailServer Port")>
    <Column(Order:=9)>
    Public Property MailServerPort As Integer
        Get
            Return _userData._mailServerPort
        End Get
        Set
            _userData._mailServerPort = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Mailserver User Name")>
    <Column(Order:=10)>
    Public Property MailserverUserName As String
        Get
            Return _userData._mailserverUserName
        End Get
        Set
            _userData._mailserverUserName = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Outgoing Mail Server")>
    <Column(Order:=11)>
    Public Property OutgoingMailServer As String
        Get
            Return _userData._outgoingMailServer
        End Get
        Set
            Me.OnCareLinkUserChanged()
            _userData._outgoingMailServer = Value
        End Set
    End Property

    <DisplayName("SettingsVersion")>
    <Column(Order:=7)>
    Public Property SettingsVersion As String
        Get
            Return _userData._settingsVersion
        End Get
        Set
            _userData._settingsVersion = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Use Advanced AIT Decay")>
    <Column(Order:=12)>
    Public Property UseAdvancedAITDecay As Boolean
        Get
            Return _userData._useAdvancedAITDecay
        End Get
        Set
            _userData._useAdvancedAITDecay = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Use Local Time Zone")>
    <Column(Order:=13)>
    Public Property UseLocalTimeZone As Boolean
        Get
            Return _userData._useLocalTimeZone
        End Get
        Set
            _userData._useLocalTimeZone = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Private Sub OnCareLinkUserChanged()
        If Not _inTxn And (Parent IsNot Nothing) Then
            Parent.CareLinkUserChanged(Me)
        End If
    End Sub

    ''' <summary>
    ''' Delete any user specific sensitive data
    ''' </summary>
    Friend Sub clean()
        _userData._alertPhoneNumber = ""
        _userData._carrierTextingDomain = ""
        _userData._mailserverPassword = ""
        _userData._mailServerPort = 0
        _userData._mailserverUserName = ""
        _userData._outgoingMailServer = ""
    End Sub

    Friend Function GetValueByName(entryName As String) As String
        Select Case entryName
            Case NameOf(CareLinkUserName)
                Return Me.CareLinkUserName
            Case NameOf(CareLinkPassword)
                Return Me.CareLinkPassword
            Case NameOf(AIT)
                Return Me.AIT.ToString
            Case NameOf(AlertPhoneNumber)
                Return Me.AlertPhoneNumber
            Case NameOf(CarrierTextingDomain)
                Return Me.CarrierTextingDomain
            Case NameOf(CountryCode)
                Return Me.CountryCode
            Case NameOf(MailserverPassword)
                Return Me.MailserverPassword
            Case NameOf(MailServerPort)
                Return CStr(Me.MailServerPort)
            Case NameOf(MailserverUserName)
                Return Me.MailserverUserName
            Case NameOf(SettingsVersion)
                Return Me.SettingsVersion
            Case NameOf(OutgoingMailServer)
                Return Me.OutgoingMailServer
            Case NameOf(UseAdvancedAITDecay)
                Return CStr(Me.UseAdvancedAITDecay)
            Case NameOf(UseLocalTimeZone)
                Return CStr(Me.UseLocalTimeZone)
            Case NameOf(AutoLogin)
                Return CStr(Me.AutoLogin)
            Case Else
                Throw UnreachableException()
        End Select
    End Function

    Friend Function ToCsvString() As String
        Return $"{Me.CareLinkUserName},{Me.CareLinkPassword}," &
                $"{Me.AIT},{Me.AlertPhoneNumber},{Me.CarrierTextingDomain}," &
                $"{Me.CountryCode},{Me.MailserverPassword}," &
                $"{Me.MailServerPort},{Me.MailserverUserName}," &
                $"{Me.MailserverUserName},{Me.OutgoingMailServer}," &
                $"{Me.UseAdvancedAITDecay},{Me.UseLocalTimeZone},{Me.AutoLogin}"
    End Function

    Friend Sub UpdateSettings()
        My.Settings.CareLinkUserName = _userData._careLinkUserName
        My.Settings.CareLinkPassword = _userData._careLinkPassword
        My.Settings.AIT = _userData._aIT
        My.Settings.AlertPhoneNumber = _userData._alertPhoneNumber
        My.Settings.CarrierTextingDomain = _userData._carrierTextingDomain
        My.Settings.CountryCode = _userData._countryCode
        My.Settings.MailServerPassword = _userData._mailserverPassword
        My.Settings.MailServerPort = _userData._mailServerPort
        My.Settings.MailServerUserName = _userData._mailserverUserName
        My.Settings.SettingsVersion = _userData._settingsVersion
        My.Settings.OutGoingMailServer = _userData._outgoingMailServer
        My.Settings.UseAdvancedAITDecay = _userData._useAdvancedAITDecay
        My.Settings.UseLocalTimeZone = _userData._useLocalTimeZone
        My.Settings.AutoLogin = _userData._autoLogin
    End Sub

    ''' <summary>
    ''' Replace the value of entryName with Value
    ''' </summary>
    ''' <param name="entryName"></param>
    ''' <param name="value"></param>
    Friend Sub UpdateValue(entryName As String, value As String)
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
            Case NameOf(OutgoingMailServer)
                Me.OutgoingMailServer = value
            Case NameOf(UseAdvancedAITDecay)
                Me.UseAdvancedAITDecay = CBool(value)
            Case NameOf(UseLocalTimeZone)
                Me.UseLocalTimeZone = CBool(value)
            Case NameOf(AutoLogin)
                Me.AutoLogin = CBool(value)
            Case Else
        End Select
    End Sub

    Public Structure CareLinkUserData
        Friend _iD As Integer
        Friend _aIT As TimeSpan
        Friend _alertPhoneNumber As String
        Friend _autoLogin As Boolean
        Friend _careLinkPassword As String
        Friend _careLinkUserName As String
        Friend _carrierTextingDomain As String
        Friend _countryCode As String
        Friend _mailserverPassword As String
        Friend _mailServerPort As Integer
        Friend _mailserverUserName As String
        Friend _outgoingMailServer As String
        Friend _settingsVersion As String
        Friend _useAdvancedAITDecay As Boolean
        Friend _useLocalTimeZone As Boolean
    End Structure

#Region "Implements IEditableObject"

    Public Sub BeginEdit() Implements IEditableObject.BeginEdit
        Debug.WriteLine($"Start EndEdit{_userData._iD}{_userData._careLinkUserName}")
        If Not _inTxn Then
            _backupData = _userData
            _inTxn = True

            Debug.WriteLine($"BeginEdit  - {_userData._iD}{_userData._careLinkUserName}")
        End If
    End Sub

    Public Sub CancelEdit() Implements IEditableObject.CancelEdit
        Debug.WriteLine("Start CancelEdit")
        If _inTxn Then
            _userData = _backupData
            Debug.WriteLine($"CancelEdit - {_userData._iD}{_userData._careLinkUserName}")
        End If
        Debug.WriteLine("End CancelEdit")
    End Sub

    Public Sub EndEdit() Implements IEditableObject.EndEdit
        Debug.WriteLine($"Start EndEdit{_userData._iD}{_userData._careLinkUserName}")
        If _inTxn Then
            _backupData = New CareLinkUserData()
            _inTxn = False
            Debug.WriteLine($"Done EndEdit - {_userData._iD}{_userData._careLinkUserName}")
        End If
        Debug.WriteLine("End EndEdit")
    End Sub

#End Region ' Implements IEditableObject

End Class
