' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CareLinkUserDataRecord
    Implements IEditableObject

    Structure CareLinkUserData
        Friend _aIT As TimeSpan
        Friend _alertPhoneNumber As String
        Friend _autoLogin As Boolean
        Friend _careLinkPassword As String
        Friend _careLinkUserName As String
        Friend _carrierTextingDomain As String
        Friend _countryCode As String
        Friend _iD As String
        Friend _mailserverPassword As String
        Friend _mailServerPort As Integer
        Friend _mailserverUserName As String
        Friend _outGoingMailServer As String
        Friend _settingsVersion As String
        Friend _useAdvancedAITDecay As Boolean
        Friend _useLocalTimeZone As Boolean
    End Structure

    Public Parent As CareLinkUserDataList
    Private _userData As CareLinkUserData
    Private _backupData As CareLinkUserData
    Private _inTxn As Boolean = False

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

    Public Sub New(Id As String)
        _userData = New CareLinkUserData With {
            ._iD = Id
        }
    End Sub

    Public Sub New()
        _userData = New CareLinkUserData With {
            ._iD = s_allUserSettingsData.Count.ToString,
            ._careLinkUserName = My.Settings.CareLinkUserName,
            ._careLinkPassword = My.Settings.CareLinkPassword,
            ._aIT = My.Settings.AIT,
            ._alertPhoneNumber = My.Settings.AlertPhoneNumber,
            ._carrierTextingDomain = My.Settings.CarrierTextingDomain,
            ._countryCode = My.Settings.CountryCode,
            ._mailserverPassword = My.Settings.MailServerPassword,
            ._mailServerPort = My.Settings.MailServerPort,
            ._mailserverUserName = My.Settings.MailServerUserName,
            ._settingsVersion = My.Settings.SettingsVersion,
            ._outGoingMailServer = My.Settings.OutGoingMailServer,
            ._useAdvancedAITDecay = My.Settings.UseAdvancedAITDecay,
            ._useLocalTimeZone = My.Settings.UseLocalTimeZone,
            ._autoLogin = My.Settings.AutoLogin
        }
    End Sub

    Public Sub New(id As Integer, currentRow As String())
        _userData = New CareLinkUserData With {
            ._iD = id.ToString
        }
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

            Me.UpdateValue(entryName, value)
        Next
    End Sub

#If True Then ' Do not move properties

#Region "Properties"

    Public ReadOnly Property ID() As String
        Get
            Return _userData._iD
        End Get
    End Property

    Public Property CareLinkUserName As String
        Get
            Return _userData._careLinkUserName
        End Get
        Set
            _userData._careLinkUserName = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property CareLinkPassword As String
        Get
            Return _userData._careLinkPassword
        End Get
        Set
            _userData._careLinkPassword = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property AIT As TimeSpan
        Get
            Return _userData._aIT
        End Get
        Set
            _userData._aIT = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property AlertPhoneNumber As String
        Get
            Return _userData._alertPhoneNumber
        End Get
        Set
            _userData._alertPhoneNumber = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property CarrierTextingDomain As String
        Get
            Return _userData._carrierTextingDomain
        End Get
        Set
            _userData._carrierTextingDomain = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property CountryCode As String
        Get
            Return _userData._countryCode
        End Get
        Set
            _userData._countryCode = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property SettingsVersion As String
        Get
            Return _userData._settingsVersion
        End Get
        Set
            _userData._settingsVersion = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property MailserverPassword As String
        Get
            Return _userData._mailserverPassword
        End Get
        Set
            _userData._mailserverPassword = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property MailServerPort As Integer
        Get
            Return _userData._mailServerPort
        End Get
        Set
            _userData._mailServerPort = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property MailserverUserName As String
        Get
            Return _userData._mailserverUserName
        End Get
        Set
            _userData._mailserverUserName = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property OutGoingMailServer As String
        Get
            Return _userData._outGoingMailServer
        End Get
        Set
            Me.OnCareLinkUserChanged()
            _userData._outGoingMailServer = Value
        End Set
    End Property

    Public Property UseAdvancedAITDecay As Boolean
        Get
            Return _userData._useAdvancedAITDecay
        End Get
        Set
            _userData._useAdvancedAITDecay = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property UseLocalTimeZone As Boolean
        Get
            Return _userData._useLocalTimeZone
        End Get
        Set
            _userData._useLocalTimeZone = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    Public Property AutoLogin As Boolean
        Get
            Return _userData._autoLogin
        End Get
        Set
            _userData._autoLogin = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

#End Region

#End If  ' Do not move properties

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
        _userData._outGoingMailServer = ""
    End Sub

    Friend Function GetValueByName(entryName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As String
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
            Case NameOf(OutGoingMailServer)
                Return Me.OutGoingMailServer
            Case NameOf(UseAdvancedAITDecay)
                Return CStr(Me.UseAdvancedAITDecay)
            Case NameOf(UseLocalTimeZone)
                Return CStr(Me.UseLocalTimeZone)
            Case NameOf(AutoLogin)
                Return CStr(Me.AutoLogin)
            Case Else
                Throw UnreachableException(memberName, sourceLineNumber)
        End Select
    End Function

    Friend Function ToCsvString() As String
        Return $"{Me.CareLinkUserName},{Me.CareLinkPassword}," &
                $"{Me.AIT},{Me.AlertPhoneNumber},{Me.CarrierTextingDomain}," &
                $"{Me.CountryCode},{Me.MailserverPassword}," &
                $"{Me.MailServerPort},{Me.MailserverUserName}," &
                $"{Me.MailserverUserName},{Me.OutGoingMailServer}," &
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
        My.Settings.OutGoingMailServer = _userData._outGoingMailServer
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

End Class
