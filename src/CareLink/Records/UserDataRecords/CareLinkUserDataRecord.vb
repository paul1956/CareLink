' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

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
            ._countryCode = If(My.Settings.CountryCode, ""),
             ._useLocalTimeZone = My.Settings.UseLocalTimeZone,
            ._autoLogin = My.Settings.AutoLogin
        }
    End Sub

    Public Sub New(parent As CareLinkUserDataList, headerRow As String(), currentRow As String())
        _userData = New CareLinkUserData With {
            ._iD = parent.Count
        }
        For Each e As IndexClass(Of String) In currentRow.WithIndex
            Dim value As String = If(e.Value, "")
            Me.UpdateValue(headerRow(e.Index), value)
        Next
    End Sub

    <DisplayName("Auto Login")>
    <Column(Order:=5, TypeName:=NameOf([Boolean]))>
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
    <Column(Order:=2, TypeName:=NameOf([String]))>
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
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property CareLinkUserName As String
        Get
            Return _userData._careLinkUserName
        End Get
        Set
            _userData._careLinkUserName = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Country Code")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
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
    <Column(Order:=0, TypeName:=NameOf([Int32]))>
    Public ReadOnly Property ID() As Integer
        Get
            Return _userData._iD
        End Get
    End Property

    <DisplayName("Use Local Time Zone")>
    <Column(Order:=4, TypeName:=NameOf([Boolean]))>
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

    Friend Function ToCsvString() As String
        Return $"{Me.CareLinkUserName},{Me.CareLinkPassword}," &
               $"{Me.CountryCode},{Me.UseLocalTimeZone}," &
                $"{Me.AutoLogin}"
    End Function

    Friend Sub UpdateSettings()
        My.Settings.CareLinkUserName = _userData._careLinkUserName
        My.Settings.CareLinkPassword = _userData._careLinkPassword
        My.Settings.CountryCode = _userData._countryCode
        My.Settings.UseLocalTimeZone = _userData._useLocalTimeZone
        My.Settings.AutoLogin = _userData._autoLogin
    End Sub

    ''' <summary>
    ''' Replace the value of entryName with Value
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    Friend Sub UpdateValue(key As String, value As String)
        Select Case key
            Case NameOf(CareLinkUserName)
                Me.CareLinkUserName = value
            Case NameOf(CareLinkPassword)
                Me.CareLinkPassword = value
            Case NameOf(CountryCode)
                Me.CountryCode = value
            Case NameOf(UseLocalTimeZone)
                Me.UseLocalTimeZone = CBool(value)
            Case NameOf(AutoLogin)
                Me.AutoLogin = CBool(value)
            Case Else
        End Select
    End Sub

    Public Structure CareLinkUserData
        Friend _autoLogin As Boolean
        Friend _careLinkPassword As String
        Friend _careLinkUserName As String
        Friend _countryCode As String
        Friend _iD As Integer
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

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim record As CareLinkUserDataRecord = TryCast(obj, CareLinkUserDataRecord)
        Return record IsNot Nothing AndAlso
               Me.ID = record.ID AndAlso
               Me.AutoLogin = record.AutoLogin AndAlso
               Me.CareLinkPassword = record.CareLinkPassword AndAlso
               Me.CareLinkUserName = record.CareLinkUserName AndAlso
               Me.CountryCode = record.CountryCode AndAlso
               Me.UseLocalTimeZone = record.UseLocalTimeZone
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New NotImplementedException()
    End Function

#End Region ' Implements IEditableObject

End Class
